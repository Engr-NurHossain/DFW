using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;
using System.Linq;

namespace HS.DataAccess
{
    public partial class PurchaseOrderDataAccess
    {
        public DataSet GetPurchaseOrderListByFilters(PurchaseOrderFilter filters, DateTime? start, DateTime? end)
        {
            string SearchTextFilter = "";
            string currentTechQuery = "";
            string Query = "";
            string EstimatorIdFilter = "";
            string DateQuery = "";
            if (!string.IsNullOrWhiteSpace(filters.selectsts) && filters.selectsts.ToLower() != "null")
            {
                string SubQuery = "";
                
                if (filters.selectsts.Contains("1"))
                {
                    SubQuery += " or poware.Status in ('Created','Sent to Vendor','RecieveOn','Paid','Bill Created')";
                }
                if (filters.selectsts.Contains("2"))
                {
                    SubQuery += " or poware.Status = 'Received'";
                }
                if (filters.selectsts.Contains("3"))
                {
                    SubQuery += " or poware.Status = 'Received Partially'";
                }
                if (!string.IsNullOrEmpty(SubQuery))
                {
                    SubQuery = SubQuery.Substring(3, SubQuery.Length - 3);
                    Query = string.Format(" and({0})", SubQuery);
                }
            }
            if (!string.IsNullOrWhiteSpace(filters.Searchtext))
            {
                SearchTextFilter = @"and(CONVERT(nvarchar(11), poware.OrderDate, 101) like @SearchText
                                    or isnull(poware.[PurchaseOrderId], '') like @SearchText 
                                    or poware.[Status] like @SearchText)";
            }
            if(filters.IsAllTechPO != null && filters.IsAllTechPO == false)
            {
                currentTechQuery= string.Format(" and poware.POFor = '{0}'",filters.EmployeeId);
            }
            if (!string.IsNullOrWhiteSpace(filters.EstimatorId) && filters.EstimatorId != "null" && filters.EstimatorId != "undefined" && filters.EstimatorId != "-1")
            {
                EstimatorIdFilter = string.Format(" and poware.EstimatorId = '{0}'", filters.EstimatorId);
            }
            if (start != new DateTime() || end != new DateTime())
            {
                DateQuery = string.Format("and poware.CreatedDate between '{0}' and '{1}'", start, end);

            }
            string sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                    DECLARE @pagestart int
                                    DECLARE @pageend int
                                    DECLARE @pageno int
                                    DECLARE @pagesize int
                                    DECLARE @SearchText nvarchar(50)

                                    SET @SearchText = '%{0}%'
                                    SET @pageno = {1} -- default 1
                                    SET @pagesize = {2} -- default 10
                                    SET @CompanyId = '{3}' -- 97BCF758 - A482 - 47EB - 82B8 - F88BF12293FF

                                    SET @pagestart = (@pageno - 1) * @pagesize
                                        SET @pageend = @pagesize

                                        SELECT DISTINCT poware.Id
                                                , poware.PurchaseOrderId
                                                , poware.CompanyId
                                                , poware.CreatedByUid
                                                , poware.SuplierId
                                                , poware.POFor
                                                , poware.Status
                                                , poware.CreatedDate
                                                , poware.Description
                                                , emp.FirstName + ' ' + emp.LastName as Name
                                                , sp.CompanyName as VendorName
                                                , poder.TotalPrice as TotalOrderPrice
                                                , empPoFor.FirstName + ' ' + empPoFor.LastName as TechnicianName
                                                , (SELECT SUM(TotalPrice) FROM PurchaseOrderDetail WHERE PurchaseOrderId = poware.PurchaseOrderId) AS TotalOrderPriceSum
                                        INTO #DistinctPOData
                                        FROM PurchaseOrderWarehouse poware
                                        LEFT JOIN Employee emp on emp.UserId = poware.CreatedByUid
                                        LEFT JOIN Supplier sp on sp.SupplierId = poware.SuplierId
                                        LEFT JOIN Employee empPoFor on empPoFor.UserId = poware.POFor
                                        LEFT JOIN PurchaseOrderDetail poder on poder.PurchaseOrderId = poware.PurchaseOrderId
                                        WHERE poware.CompanyId = @CompanyId
                                            {4}
                                            {6}
                                            {8}
                                            AND poware.[Status] != 'Init'
                                            AND poware.[Status] != ''
                                            {9}
                                            {7}

                                        ;WITH UniquePOData AS (
                                            SELECT *,
                                                   ROW_NUMBER() OVER (PARTITION BY Id {5}) AS RowNum
                                            FROM #DistinctPOData
                                        )
                                        SELECT *
                                        FROM UniquePOData
                                        WHERE RowNum = 1
                                        {5}
                                        OFFSET @pagestart ROWS FETCH NEXT @pagesize ROWS ONLY

                                        SELECT COUNT(DISTINCT Id) as [TotalCount] 
                                        FROM #DistinctPOData

                                        DROP TABLE #DistinctPOData";
            string OrderBy = "";
            if (!string.IsNullOrEmpty(filters.order) && filters.order != "undefined" && filters.order != "null")
            {
                if (filters.order == "ascending/pono")
                {
                    OrderBy = "order by PurchaseOrderId asc";
                }
                else if (filters.order == "descending/pono")
                {
                    OrderBy = "order by PurchaseOrderId desc";
                }

                else if (filters.order == "ascending/vname")
                {
                    OrderBy = "order by VendorName asc";
                }
                else if (filters.order == "descending/vname")
                {
                    OrderBy = "order by VendorName desc";
                }

                else if (filters.order == "ascending/status")
                {
                    OrderBy = "order by Status asc";
                }
                else if (filters.order == "descending/status")
                {
                    OrderBy = "order by Status desc";
                }

                else if (filters.order == "ascending/cdate")
                {
                    OrderBy = "order by CreatedDate asc";
                }
                else if (filters.order == "descending/cdate")
                {
                    OrderBy = "order by CreatedDate desc";
                }

                else if (filters.order == "ascending/cby")
                {
                    OrderBy = "order by Name asc";
                }
                else if (filters.order == "descending/cby")
                {
                    OrderBy = "order by Name desc";
                }
                else if (filters.order == "ascending/total")
                {
                    OrderBy = "order by TotalAmount asc";
                }
                else if (filters.order == "descending/total")
                {
                    OrderBy = "order by TotalAmount desc";
                }
                else if (filters.order == "ascending/estimatorid")
                {
                    OrderBy = "order by EstimatoId asc";
                }
                else if (filters.order == "descending/estimatorid")
                {
                    OrderBy = "order by EstimatoId desc";
                }
                else if (filters.order == "ascending/technicianName")
                {
                    OrderBy = "order by TechnicianName asc";
                }
                else if (filters.order == "descending/technicianName")
                {
                    OrderBy = "order by TechnicianName desc";
                }
            }
            else
            {
                OrderBy = "order by CreatedDate desc";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        filters.Searchtext, //0
                                        filters.PageNo,  //1
                                        filters.PageSize, //2
                                        filters.CompanyId, //3
                                        SearchTextFilter, //4
                                        OrderBy, //5
                                        currentTechQuery, //6
                                        Query, //7
                                        EstimatorIdFilter, //8
                                        DateQuery //9
                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetPurchaseOrderExportListByFilters(PurchaseOrderFilter filters, DateTime? start, DateTime? end)
        {
            string SearchTextFilter = "";
            string currentTechQuery = "";
            string Query = "";
            string EstimatorIdFilter = "";
            string DateQuery = "";
            if (!string.IsNullOrWhiteSpace(filters.selectsts) && filters.selectsts.ToLower() != "null")
            {
                string SubQuery = "";

                if (filters.selectsts.Contains("1"))
                {
                    SubQuery += " or poware.Status in ('Created','Sent to Vendor','RecieveOn','Paid','Bill Created')";
                }
                if (filters.selectsts.Contains("2"))
                {
                    SubQuery += " or poware.Status = 'Received'";
                }
                if (filters.selectsts.Contains("3"))
                {
                    SubQuery += " or poware.Status = 'Received Partially'";
                }
                if (!string.IsNullOrEmpty(SubQuery))
                {
                    SubQuery = SubQuery.Substring(3, SubQuery.Length - 3);
                    Query = string.Format(" and({0})", SubQuery);
                }
            }
            if (!string.IsNullOrWhiteSpace(filters.Searchtext))
            {
                SearchTextFilter = @"and(CONVERT(nvarchar(11), poware.OrderDate, 101) like @SearchText
                                    or isnull(poware.[PurchaseOrderId], '') like @SearchText 
                                    or poware.[Status] like @SearchText)";
            }
            if (filters.IsAllTechPO != null && filters.IsAllTechPO == false)
            {
                currentTechQuery = string.Format(" and poware.POFor = '{0}'", filters.EmployeeId);
            }
            if (!string.IsNullOrWhiteSpace(filters.EstimatorId) && filters.EstimatorId != "null" && filters.EstimatorId != "undefined" && filters.EstimatorId != "-1")
            {
                EstimatorIdFilter = string.Format(" and poware.EstimatorId = '{0}'", filters.EstimatorId);
            }
            if (start != new DateTime() || end != new DateTime())
            {
                DateQuery = string.Format("and poware.CreatedDate between '{0}' and '{1}'", start, end);

            }
            string sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                    
                                    DECLARE @SearchText nvarchar(50)
                                    
                                    SET @SearchText = '%{0}%' 
                                    SET @CompanyId = '{3}' 
                                     

                                        SELECT DISTINCT poware.Id
                                                , poware.PurchaseOrderId
                                                , poware.CompanyId
                                                , poware.CreatedByUid
                                                , poware.SuplierId
                                                , poware.POFor
                                                , poware.Status
                                                 ,poware.CreatedDate as [Created Date]
                                                --,poware.CreatedDate
                                                , poware.Description
                                                , emp.FirstName + ' ' + emp.LastName as Name
                                                , sp.CompanyName as VendorName
                                                , poder.TotalPrice as TotalOrderPrice
                                                , empPoFor.FirstName + ' ' + empPoFor.LastName as [Received For]
                                                , (SELECT SUM(TotalPrice) FROM PurchaseOrderDetail WHERE PurchaseOrderId = poware.PurchaseOrderId) AS TotalOrderPriceSum
                                        INTO #DistinctPOData
                                        FROM PurchaseOrderWarehouse poware
                                        LEFT JOIN Employee emp on emp.UserId = poware.CreatedByUid
                                        LEFT JOIN Supplier sp on sp.SupplierId = poware.SuplierId
                                        LEFT JOIN Employee empPoFor on empPoFor.UserId = poware.POFor
                                        LEFT JOIN PurchaseOrderDetail poder on poder.PurchaseOrderId = poware.PurchaseOrderId
                                        WHERE poware.CompanyId = @CompanyId
                                            {4}
                                            {6}
                                            {8}
                                            AND poware.[Status] != 'Init'
                                            AND poware.[Status] != ''
                                            {9}
                                            {7}

                                        ;WITH UniquePOData AS (
                                            SELECT *,
                                                   ROW_NUMBER() OVER (PARTITION BY Id ORDER BY Id DESC) AS RowNum
                                            FROM #DistinctPOData
                                        )
                                        SELECT *
                                        FROM UniquePOData
                                        WHERE RowNum = 1
                                        ORDER BY Id DESC
                                        -- OFFSET @pagestart ROWS FETCH NEXT @pagesize ROWS ONLY

                                        SELECT COUNT(DISTINCT Id) as [TotalCount] 
                                        FROM #DistinctPOData

                                        DROP TABLE #DistinctPOData";
            string OrderBy = "";
            if (!string.IsNullOrEmpty(filters.order) && filters.order != "null")
            {
                if (filters.order == "ascending/pono")
                {
                    OrderBy = "order by PurchaseOrderId asc";
                }
                else if (filters.order == "descending/pono")
                {
                    OrderBy = "order by PurchaseOrderId desc";
                }

                else if (filters.order == "ascending/vname")
                {
                    OrderBy = "order by VendorName asc";
                }
                else if (filters.order == "descending/vname")
                {
                    OrderBy = "order by VendorName desc";
                }

                else if (filters.order == "ascending/status")
                {
                    OrderBy = "order by Status asc";
                }
                else if (filters.order == "descending/status")
                {
                    OrderBy = "order by Status desc";
                }

                else if (filters.order == "ascending/cdate")
                {
                    OrderBy = "order by CreatedDate asc";
                }
                else if (filters.order == "descending/cdate")
                {
                    OrderBy = "order by CreatedDate desc";
                }

                else if (filters.order == "ascending/cby")
                {
                    OrderBy = "order by Name asc";
                }
                else if (filters.order == "descending/cby")
                {
                    OrderBy = "order by Name desc";
                }
                else if (filters.order == "ascending/total")
                {
                    OrderBy = "order by TotalAmount asc";
                }
                else if (filters.order == "descending/total")
                {
                    OrderBy = "order by TotalAmount desc";
                }
                else if (filters.order == "ascending/estimatorid")
                {
                    OrderBy = "order by EstimatoId asc";
                }
                else if (filters.order == "descending/estimatorid")
                {
                    OrderBy = "order by EstimatoId desc";
                }
            }
            else
            {
                OrderBy = "order by CreatedDate desc";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        filters.Searchtext, //0
                                        filters.PageNo,  //1
                                        filters.PageSize, //2
                                        filters.CompanyId, //3
                                        SearchTextFilter, //4
                                        OrderBy, //5
                                        currentTechQuery, //6
                                        Query, //7
                                        EstimatorIdFilter, //8
                                        DateQuery //9
                                        );
                
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                { 
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetBadInventoryListFilters(BadInventoryFilter filters, Guid techid)
        {
            string SearchTextFilter = "";

            string StatusIDList = "";
            string TechnicianIDList = "";
            string Purchase_DateFilter = "";

            if (!string.IsNullOrWhiteSpace(filters.StatusIDList) && filters.StatusIDList != "-1" && filters.StatusIDList != "'null'" && filters.StatusIDList != "'undefined'")
            {
                StatusIDList = string.Format("and EqR.Status in ({0})", filters.StatusIDList);
            }
            if (!string.IsNullOrWhiteSpace(filters.TechnicianIDList) && filters.TechnicianIDList != "'null'" && filters.TechnicianIDList != "'undefined'")
            {
                TechnicianIDList = string.Format("and EqR.TechnicianId in ({0})", filters.TechnicianIDList);
            }
            if (filters.Purchase_Date_From != new DateTime() && filters.Purchase_Date_To != new DateTime())
            {
                filters.Purchase_Date_To = filters.Purchase_Date_To.AddHours(23).AddMinutes(59).AddSeconds(59);

                Purchase_DateFilter = string.Format("and EqR.PurchaseDate between '{0}' and '{1}'", filters.Purchase_Date_From, filters.Purchase_Date_To);
            }
            else if (filters.Purchase_Date_From != new DateTime() && filters.Purchase_Date_To == new DateTime())
            {

                Purchase_DateFilter = string.Format("and EqR.PurchaseDate >= '{0}'", filters.Purchase_Date_From);
            }
            else if (filters.Purchase_Date_From == new DateTime() && filters.Purchase_Date_To != new DateTime())
            {
                filters.Purchase_Date_To = filters.Purchase_Date_To.AddHours(23).AddMinutes(59).AddSeconds(59);

                Purchase_DateFilter = string.Format("and EqR.PurchaseDate <= '{0}'", filters.Purchase_Date_To);
            }





            if (!string.IsNullOrWhiteSpace(filters.Searchtext))
            {
                SearchTextFilter = @" and (EqR.InvoiceNo like @SearchText or cus.FirstName + ' ' + cus.LastName like @SearchText or Eqp.Name like @SearchText or EqR.Quantity like @SearchText or EqR.Description like @SearchText)";
            }
            string sqlQuery = @"";
            #region Naming Condition
            string NamingSql = "''";
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            if (gs != null)
            {
                NamingSql = gs.Value;
            }
            #endregion
            if (filters.Start != new DateTime() && filters.End != new DateTime())
            {

                sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int
                                --DECLARE @SearchText nvarchar(50)

                                --SET @SearchText = '%{0}%'
                                SET @pageno = {1}--default 1
                                SET @pagesize = {2}--default 10
                                SET @CompanyId = '{3}'--97BCF758 - A482 - 47EB - 82B8 - F88BF12293FF

                                SET @pagestart = (@pageno - 1) * @pagesize
                                SET @pageend = @pagesize


                                select * into #POData 
                                from(select EqR.*, {9} as CustomerName, cus.Id as CusIdInt,
                                Eqp.Name as EquipmentName, Emp.FirstName + ' ' + Emp.LastName as TechnicianName
                                from EquipmentReturn EqR
                                Left Join Customer cus on cus.CustomerId=EqR.CustomerId
                                left join CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                Left Join Equipment Eqp on Eqp.EquipmentId=EqR.EquipmentId
                                Left Join Employee Emp on Emp.UserId=EqR.TechnicianId
                                where EqR.CompanyId = @CompanyId
                                and ce.IsTestAccount != 1
                                and EqR.PurchaseDate between '{7}' and '{8}'
                                   and Quantity > 0
                                {4} {6} {10} {11} ) as POD
                           

                                 SELECT TOP(@pagesize) * into #TestTable FROM #POData _podata
                                    where   Id NOT IN(Select TOP(@pagestart) Id from #POData)
                                    and Quantity > 0
	                                {5}
                                select  count(Id) as [TotalCount] from #POData where Quantity > 0
                                select * from #TestTable
								select SUM(Quantity) as TotalQuantity from #TestTable
                                DROP TABLE #POData
                                Drop Table #TestTable";
            }
            else
            {
                sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int
                                --DECLARE @SearchText nvarchar(50)

                                --SET @SearchText = '%{0}%'
                                SET @pageno = {1}--default 1
                                SET @pagesize = {2}--default 10
                                SET @CompanyId = '{3}'--97BCF758 - A482 - 47EB - 82B8 - F88BF12293FF

                                SET @pagestart = (@pageno - 1) * @pagesize
                                SET @pageend = @pagesize


                                select * into #POData 
                                from(select EqR.*,{9} as CustomerName, cus.Id as CusIdInt,
                                Eqp.Name as EquipmentName, Emp.FirstName + ' ' + Emp.LastName as TechnicianName
                                from EquipmentReturn EqR
                                Left Join Customer cus on cus.CustomerId=EqR.CustomerId
                                left join CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                Left Join Equipment Eqp on Eqp.EquipmentId=EqR.EquipmentId
                                Left Join Employee Emp on Emp.UserId=EqR.TechnicianId
                                where EqR.CompanyId = @CompanyId
                                and ce.IsTestAccount != 1
                                and Quantity > 0
                                {4} {6} {10} {11} {12} ) as POD

                                         SELECT TOP(@pagesize) * into #TestTable FROM #POData _podata
                                    where   Id NOT IN(Select TOP(@pagestart) Id from #POData)
                                    and Quantity > 0
	                                {5}
                                select  count(Id) as [TotalCount] from #POData where Quantity > 0
                                select * from #TestTable
								select SUM(Quantity) as TotalQuantity from #TestTable
                                DROP TABLE #POData
                                Drop Table #TestTable";
            }
            string subquery = "";
            if (!string.IsNullOrWhiteSpace(filters.order))
            {
                if (filters.order == "descending/id")
                {
                    subquery = "order by _podata.Id desc";
                }
                else if (filters.order == "ascending/id")
                {
                    subquery = "order by _podata.Id asc";
                }

                else if (filters.order == "descending/customer")
                {
                    subquery = "order by CustomerName desc";
                }
                else if (filters.order == "ascending/customer")
                {
                    subquery = "order by CustomerName asc";
                }
                else if (filters.order == "ascending/technician")
                {
                    subquery = "order by TechnicianName asc";
                }
                else if (filters.order == "descending/technician")
                {
                    subquery = "order by TechnicianName desc";
                }

                else if (filters.order == "ascending/equipment")
                {
                    subquery = "order by EquipmentName asc";
                }
                else if (filters.order == "descending/equipment")
                {
                    subquery = "order by EquipmentName desc";
                }
                else if (filters.order == "ascending/quantity")
                {
                    subquery = "order by _podata.Quantity asc";
                }
                else if (filters.order == "descending/quantity")
                {
                    subquery = "order by _podata.Quantity desc";
                }
                else if (filters.order == "ascending/invoice")
                {
                    subquery = "order by _podata.InvoiceNo asc";
                }
                else if (filters.order == "descending/invoice")
                {
                    subquery = "order by _podata.InvoiceNo desc";
                }
                else if (filters.order == "ascending/purchase")
                {
                    subquery = "order by _podata.PurchaseDate asc";
                }
                else if (filters.order == "descending/purchase")
                {
                    subquery = "order by _podata.PurchaseDate desc";
                }
                else if (filters.order == "ascending/wanranty")
                {
                    subquery = "order by _podata.WanrantyAvailable asc";
                }
                else if (filters.order == "descending/wanranty")
                {
                    subquery = "order by _podata.WanrantyAvailable desc";
                }
                else if (filters.order == "ascending/description")
                {
                    subquery = "order by _podata.Description asc";
                }
                else if (filters.order == "descending/description")
                {
                    subquery = "order by _podata.Description desc";
                }
                else if (filters.order == "ascending/status")
                {
                    subquery = "order by _podata.Status asc";
                }
                else if (filters.order == "descending/status")
                {
                    subquery = "order by _podata.Status desc";
                }
                else
                {
                    subquery = "order by _podata.Id desc";
                }
            }
            else
            {
                subquery = "order by _podata.Id desc";
            }
            var TechnicanQuery = "";
            if (techid != new Guid())
            {
                filters.TechnicianId = techid;
            }
            if (filters.TechnicianId != Guid.Empty)
            {
                TechnicanQuery = " AND EqR.TechnicianId='" + filters.TechnicianId + "'";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        filters.Searchtext,//0
                                        filters.PageNo,  //1
                                        filters.PageSize, //2
                                        filters.CompanyId, //3
                                        SearchTextFilter,//4
                                        subquery,//5
                                        TechnicanQuery,//6
                                        filters.Start,//7
                                        filters.End,//8
                                        NamingSql, //9
                                        StatusIDList,//10
                                        TechnicianIDList,//11
                                        Purchase_DateFilter//12
                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", filters.Searchtext)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetBadInventoryReportListFilters(BadInventoryFilter filters)
        {
            string SearchTextFilter = "";
            if (!string.IsNullOrWhiteSpace(filters.Searchtext))
            {
                SearchTextFilter = @" and (EqR.InvoiceNo like @SearchText or cus.FirstName + ' ' + cus.LastName like @SearchText or Eqp.Name like @SearchText or EqR.Quantity like @SearchText or EqR.Description like @SearchText)";
            }

            string StatusIDList = "";
            string TechnicianIDList = "";
            string Purchase_DateFilter = "";

            if (!string.IsNullOrWhiteSpace(filters.StatusIDList) && filters.StatusIDList != "-1" && filters.StatusIDList != "'null'" && filters.StatusIDList != "'undefined'")
            {
                StatusIDList = string.Format("and EqR.Status in ({0})", filters.StatusIDList);
            }
            if (!string.IsNullOrWhiteSpace(filters.TechnicianIDList) && filters.TechnicianIDList != "'null'" && filters.TechnicianIDList != "'undefined'")
            {
                TechnicianIDList = string.Format("and EqR.TechnicianId in ({0})", filters.TechnicianIDList);
            }

            if (filters.Purchase_Date_From != new DateTime() && filters.Purchase_Date_To != new DateTime())
            {
                filters.Purchase_Date_To = filters.Purchase_Date_To.AddHours(23).AddMinutes(59).AddSeconds(59);

                Purchase_DateFilter = string.Format("and EqR.PurchaseDate between '{0}' and '{1}'", filters.Purchase_Date_From, filters.Purchase_Date_To);
            }
            else if (filters.Purchase_Date_From != new DateTime() && filters.Purchase_Date_To == new DateTime())
            {
                Purchase_DateFilter = string.Format("and EqR.PurchaseDate >= '{0}'", filters.Purchase_Date_From);
            }
            else if (filters.Purchase_Date_From == new DateTime() && filters.Purchase_Date_To != new DateTime())
            {
                filters.Purchase_Date_To = filters.Purchase_Date_To.AddHours(23).AddMinutes(59).AddSeconds(59);

                Purchase_DateFilter = string.Format("and EqR.PurchaseDate <= '{0}'", filters.Purchase_Date_To);
            }



            string sqlQuery = @"";
            #region Naming Condition
            string NamingSql = "''";
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            if (gs != null)
            {
                NamingSql = gs.Value;
            }
            #endregion
            if (filters.Start != new DateTime() && filters.End != new DateTime())
            {
                sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int
                                --DECLARE @SearchText nvarchar(50)

                                --SET @SearchText = '%{0}%'
                                SET @pageno = {1}--default 1
                                SET @pagesize = {2}--default 10
                                SET @CompanyId = '{3}'--97BCF758 - A482 - 47EB - 82B8 - F88BF12293FF

                                SET @pagestart = (@pageno - 1) * @pagesize
                                SET @pageend = @pagesize


                                select * into #POData 
                                from(select EqR.Id,{9} as Customer,Eqp.Name as Equipment,EqR.Quantity,EqR.InvoiceNo,convert(date,EqR.PurchaseDate) as [Purchase Date], Emp.FirstName + ' ' + Emp.LastName as Technician,EqR.Description,EqR.Status
                                from EquipmentReturn EqR
                                Left Join Customer cus on cus.CustomerId=EqR.CustomerId
                                left join CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                Left Join Equipment Eqp on Eqp.EquipmentId=EqR.EquipmentId
                                Left Join Employee Emp on Emp.UserId=EqR.TechnicianId
                                where EqR.CompanyId = @CompanyId
                                and ce.IsTestAccount != 1
                                and EqR.LastUpdatedDate between '{7}' and '{8}'
                                and EqR.PurchaseDate between '{7}' and '{8}'
                                {4} {6} {10} {11} {12}) as POD

                                    SELECT TOP(@pagesize) * FROM #POData _podata
                                    where   Id NOT IN(Select TOP(@pagestart) Id from #POData)

									    and Quantity > 0

	                                {5}
                                select  count(Id) as [TotalCount] from #POData 
                                select  count(Id) as [TotalCount] from #POData where Quantity > 0
                                DROP TABLE #POData";
            }
            else
            {
                sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int
                                --DECLARE @SearchText nvarchar(50)

                                --SET @SearchText = '%{0}%'
                                SET @pageno = {1}--default 1
                                SET @pagesize = {2}--default 10
                                SET @CompanyId = '{3}'--97BCF758 - A482 - 47EB - 82B8 - F88BF12293FF

                                SET @pagestart = (@pageno - 1) * @pagesize
                                SET @pageend = @pagesize


                                select * into #POData 
                                from(select EqR.Id,{9} as Customer,Eqp.Name as Equipment,EqR.Quantity,EqR.InvoiceNo,convert(date,EqR.PurchaseDate) as [Purchase Date], Emp.FirstName + ' ' + Emp.LastName as Technician,EqR.Description,EqR.Status
                                from EquipmentReturn EqR
                                Left Join Customer cus on cus.CustomerId=EqR.CustomerId
                                left join CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                Left Join Equipment Eqp on Eqp.EquipmentId=EqR.EquipmentId
                                Left Join Employee Emp on Emp.UserId=EqR.TechnicianId
                                where EqR.CompanyId = @CompanyId
                                and ce.IsTestAccount != 1
                                {4} {6} {10} {11} {12}) as POD

                                    SELECT TOP(@pagesize) * FROM #POData _podata
                                    where   Id NOT IN(Select TOP(@pagestart) Id from #POData)
									    and Quantity > 0

	                                {5}
                                select  count(Id) as [TotalCount] from #POData 
                                select  count(Id) as [TotalCount] from #POData where Quantity > 0
                                DROP TABLE #POData";
            }
            string subquery = "";
            if (!string.IsNullOrWhiteSpace(filters.order))
            {
                if (filters.order == "descending/id")
                {
                    subquery = "order by _podata.Id desc";
                }
                else if (filters.order == "ascending/id")
                {
                    subquery = "order by _podata.Id asc";
                }

                else if (filters.order == "descending/customer")
                {
                    subquery = "order by CustomerName desc";
                }
                else if (filters.order == "ascending/customer")
                {
                    subquery = "order by CustomerName asc";
                }
                else if (filters.order == "ascending/technician")
                {
                    subquery = "order by TechnicianName asc";
                }
                else if (filters.order == "descending/technician")
                {
                    subquery = "order by TechnicianName desc";
                }

                else if (filters.order == "ascending/equipment")
                {
                    subquery = "order by EquipmentName asc";
                }
                else if (filters.order == "descending/equipment")
                {
                    subquery = "order by EquipmentName desc";
                }
                else if (filters.order == "ascending/quantity")
                {
                    subquery = "order by _podata.Quantity asc";
                }
                else if (filters.order == "descending/quantity")
                {
                    subquery = "order by _podata.Quantity desc";
                }
                else if (filters.order == "ascending/invoice")
                {
                    subquery = "order by _podata.InvoiceNo asc";
                }
                else if (filters.order == "descending/invoice")
                {
                    subquery = "order by _podata.InvoiceNo desc";
                }
                else if (filters.order == "ascending/purchase")
                {
                    subquery = "order by _podata.PurchaseDate asc";
                }
                else if (filters.order == "descending/purchase")
                {
                    subquery = "order by _podata.PurchaseDate desc";
                }
                else if (filters.order == "ascending/wanranty")
                {
                    subquery = "order by _podata.WanrantyAvailable asc";
                }
                else if (filters.order == "descending/wanranty")
                {
                    subquery = "order by _podata.WanrantyAvailable desc";
                }
                else if (filters.order == "ascending/description")
                {
                    subquery = "order by _podata.Description asc";
                }
                else if (filters.order == "descending/description")
                {
                    subquery = "order by _podata.Description desc";
                }
                else if (filters.order == "ascending/status")
                {
                    subquery = "order by _podata.Status asc";
                }
                else if (filters.order == "descending/status")
                {
                    subquery = "order by _podata.Status desc";
                }
            }
            else
            {
                subquery = "order by _podata.Id desc";
            }
            var TechnicanQuery = "";
            if (filters.TechnicianId != Guid.Empty)
            {
                TechnicanQuery = " AND EqR.TechnicianId='" + filters.TechnicianId + "'";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        filters.Searchtext,//0
                                        1,  //1
                                        100000000, //2
                                        filters.CompanyId, //3
                                        SearchTextFilter,//4
                                        subquery,//5
                                        TechnicanQuery,//6
                                        filters.Start,//7
                                        filters.End,//8
                                        NamingSql,//9
                                        StatusIDList,//10
                                        TechnicianIDList,//11
                                        Purchase_DateFilter//12
                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", filters.Searchtext)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetPurchaseOrderListByFiltersBranch(PurchaseOrderFilter filters)
        {
            string SearchTextFilter = "";
            string EmployeeIdFilter = "";
            if (filters.EmployeeId != Guid.Empty)
            {
                EmployeeIdFilter = "And ptech.TechnicianId='" + filters.EmployeeId + "'";
            }
            if (!string.IsNullOrWhiteSpace(filters.Searchtext))
            {
                SearchTextFilter = @"and( isnull(ptech.DemandOrderId, '') like @SearchText 
                                    or emp.FirstName + ' ' + emp.LastName like @SearchText
                                    or ptech.[Status] like @SearchText)";
            }
            string sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int
                                DECLARE @SearchText nvarchar(50)

                                 SET @SearchText = '%{0}%'
                                SET @pageno = {1}--default 1
                                SET @pagesize = {2}--default 10
                                SET @CompanyId = '{3}'--97BCF758 - A482 - 47EB - 82B8 - F88BF12293FF

                                SET @pagestart = (@pageno - 1) * @pagesize
                                SET @pageend = @pagesize


                                select * into #POData 
                                from(select pt.Id as DOTId,ptech.*
                                ,pt.TicketId
                                ,emp.FirstName+' '+emp.LastName as TechName
								,emp.Email
                                from PurchaseOrderBranch ptech
                                LEFT JOIN PurchaseOrderTech pt on pt.DemandOrderId=ptech.TechDemandOrderId
								LEFT JOIN Employee emp on emp.UserId=pt.TechnicianId
                                where ptech.CompanyId = @CompanyId
                                --{5}
                                {4}
                                and ptech.[Status] != 'Init'
                                and ptech.[Status] != ''
                                    ) as POD


                                    SELECT TOP(@pagesize) * FROM #POData _podata
                                    where   Id NOT IN(Select TOP(@pagestart) Id from #POData)
	                                {6}
                                select  count(Id) as [TotalCount] from #POData 
                                DROP TABLE #POData";
            string subquery = "order by _podata.DemandOrderId desc";
            if (!string.IsNullOrWhiteSpace(filters.order))
            {
                if (filters.order == "ascending/dono")
                {
                    subquery = "order by _podata.DemandOrderId asc";
                }
                else if (filters.order == "descending/dono")
                {
                    subquery = "order by _podata.DemandOrderId desc";
                }
                else if (filters.order == "ascending/techdetails")
                {
                    subquery = "order by _podata.TechName asc";
                }
                else if (filters.order == "descending/techdetails")
                {
                    subquery = "order by _podata.TechName desc";
                }
                else if (filters.order == "ascending/status")
                {
                    subquery = "order by _podata.Status asc";
                }
                else if (filters.order == "descending/status")
                {
                    subquery = "order by _podata.Status desc";
                }
                else if (filters.order == "ascending/createddate")
                {
                    subquery = "order by _podata.CreatedDate asc";
                }
                else if (filters.order == "descending/createddate")
                {
                    subquery = "order by _podata.CreatedDate desc";
                }
                else if (filters.order == "ascending/ticketid")
                {
                    subquery = "order by _podata.TicketId asc";
                }
                else if (filters.order == "descending/ticketid")
                {
                    subquery = "order by _podata.TicketId desc";
                }
            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        filters.Searchtext,//0
                                        filters.PageNo,  //1
                                        filters.PageSize, //2
                                        filters.CompanyId, //3
                                        SearchTextFilter, //4
                                        EmployeeIdFilter, //5,
                                        subquery
                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //public DataTable GetBadInventoryReportListFilters(BadInventoryFilter filters)
        //{
        //    string SearchTextFilter = "";
        //    if (!string.IsNullOrWhiteSpace(filters.Searchtext))
        //    {
        //        SearchTextFilter = @" and (EqR.InvoiceNo like @SearchText or cus.FirstName + ' ' + cus.LastName like @SearchText or Eqp.Name like @SearchText or EqR.Quantity like @SearchText or EqR.Description like @SearchText)";
        //    }

        //    string StatusIDList = "";
        //    string TechnicianIDList = "";
        //    string Purchase_DateFilter = "";

        //    if (!string.IsNullOrWhiteSpace(filters.StatusIDList) && filters.StatusIDList != "-1" && filters.StatusIDList != "'null'" && filters.StatusIDList != "'undefined'")
        //    {
        //        StatusIDList = string.Format("and EqR.Status in ({0})", filters.StatusIDList);
        //    }
        //    if (!string.IsNullOrWhiteSpace(filters.TechnicianIDList) && filters.TechnicianIDList != "'null'" && filters.TechnicianIDList != "'undefined'")
        //    {
        //        TechnicianIDList = string.Format("and EqR.TechnicianId in ({0})", filters.TechnicianIDList);
        //    }

        //    if (filters.Purchase_Date_From != new DateTime() && filters.Purchase_Date_To != new DateTime())
        //    {
        //        Purchase_DateFilter = string.Format("and EqR.PurchaseDate between '{0}' and '{1}'", filters.Purchase_Date_From, filters.Purchase_Date_To);
        //    }
        //    else if (filters.Purchase_Date_From != new DateTime() && filters.Purchase_Date_To == new DateTime())
        //    {
        //        Purchase_DateFilter = string.Format("and EqR.PurchaseDate >= '{0}'", filters.Purchase_Date_From);
        //    }
        //    else if (filters.Purchase_Date_From == new DateTime() && filters.Purchase_Date_To != new DateTime())
        //    {
        //        Purchase_DateFilter = string.Format("and EqR.PurchaseDate <= '{0}'", filters.Purchase_Date_To);
        //    }



        //    string sqlQuery = @"";
        //    #region Naming Condition
        //    string NamingSql = "''";
        //    GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
        //    GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
        //    if (gs != null)
        //    {
        //        NamingSql = gs.Value;
        //    }
        //    #endregion
        //    if (filters.Start != new DateTime() && filters.End != new DateTime())
        //    {
        //        sqlQuery = @"DECLARE @CompanyId uniqueidentifier
        //                        DECLARE @pagestart int
        //                        DECLARE @pageend int
        //                        DECLARE @pageno int
        //                        DECLARE @pagesize int
        //                        DECLARE @SearchText nvarchar(50)

        //                        SET @SearchText = '%{0}%'
        //                        SET @pageno = {1}--default 1
        //                        SET @pagesize = {2}--default 10
        //                        SET @CompanyId = '{3}'--97BCF758 - A482 - 47EB - 82B8 - F88BF12293FF

        //                        SET @pagestart = (@pageno - 1) * @pagesize
        //                        SET @pageend = @pagesize


        //                        select * into #POData 
        //                        from(select EqR.Id,{9} as Customer,Eqp.Name as Equipment,EqR.Quantity,EqR.InvoiceNo,convert(date,EqR.PurchaseDate) as [Purchase Date], Emp.FirstName + ' ' + Emp.LastName as Technician,EqR.Description,EqR.Status
        //                        from EquipmentReturn EqR
        //                        Left Join Customer cus on cus.CustomerId=EqR.CustomerId
        //                        Left Join Equipment Eqp on Eqp.EquipmentId=EqR.EquipmentId
        //                        Left Join Employee Emp on Emp.UserId=EqR.TechnicianId
        //                        where EqR.CompanyId = @CompanyId
        //                        and EqR.LastUpdatedDate between '{7}' and '{8}'
        //                        {4} {6} {10} {11} {12}) as POD

        //                            SELECT TOP(@pagesize) * FROM #POData _podata
        //                            where   Id NOT IN(Select TOP(@pagestart) Id from #POData)
                                    
	       //                         {5}
        //                        select  count(Id) as [TotalCount] from #POData 
        //                        select  count(Id) as [TotalCount] from #POData
                                
        //                        DROP TABLE #POData";
        //    }
        //    else
        //    {
        //        sqlQuery = @"DECLARE @CompanyId uniqueidentifier
        //                        DECLARE @pagestart int
        //                        DECLARE @pageend int
        //                        DECLARE @pageno int
        //                        DECLARE @pagesize int
        //                        DECLARE @SearchText nvarchar(50)

        //                        SET @SearchText = '%{0}%'
        //                        SET @pageno = {1}--default 1
        //                        SET @pagesize = {2}--default 10
        //                        SET @CompanyId = '{3}'--97BCF758 - A482 - 47EB - 82B8 - F88BF12293FF

        //                        SET @pagestart = (@pageno - 1) * @pagesize
        //                        SET @pageend = @pagesize


        //                        select * into #POData 
        //                        from(select EqR.Id,{9} as Customer,Eqp.Name as Equipment,EqR.Quantity,EqR.InvoiceNo,convert(date,EqR.PurchaseDate) as [Purchase Date], Emp.FirstName + ' ' + Emp.LastName as Technician,EqR.Description,EqR.Status
        //                        from EquipmentReturn EqR
        //                        Left Join Customer cus on cus.CustomerId=EqR.CustomerId
        //                        Left Join Equipment Eqp on Eqp.EquipmentId=EqR.EquipmentId
        //                        Left Join Employee Emp on Emp.UserId=EqR.TechnicianId
        //                        where EqR.CompanyId = @CompanyId
        //                        {4} {6} {10} {11} {12}) as POD

        //                            SELECT TOP(@pagesize) * FROM #POData _podata
        //                            where   Id NOT IN(Select TOP(@pagestart) Id from #POData)
                                    
	       //                         {5}
        //                        select  count(Id) as [TotalCount] from #POData 
        //                        select  count(Id) as [TotalCount] from #POData
                                
        //                        DROP TABLE #POData";
        //    }
        //    string subquery = "";
        //    if (!string.IsNullOrWhiteSpace(filters.order))
        //    {
        //        if (filters.order == "descending/id")
        //        {
        //            subquery = "order by _podata.Id desc";
        //        }
        //        else if (filters.order == "ascending/id")
        //        {
        //            subquery = "order by _podata.Id asc";
        //        }

        //        else if (filters.order == "descending/customer")
        //        {
        //            subquery = "order by CustomerName desc";
        //        }
        //        else if (filters.order == "ascending/customer")
        //        {
        //            subquery = "order by CustomerName asc";
        //        }
        //        else if (filters.order == "ascending/technician")
        //        {
        //            subquery = "order by TechnicianName asc";
        //        }
        //        else if (filters.order == "descending/technician")
        //        {
        //            subquery = "order by TechnicianName desc";
        //        }

        //        else if (filters.order == "ascending/equipment")
        //        {
        //            subquery = "order by EquipmentName asc";
        //        }
        //        else if (filters.order == "descending/equipment")
        //        {
        //            subquery = "order by EquipmentName desc";
        //        }
        //        else if (filters.order == "ascending/quantity")
        //        {
        //            subquery = "order by _podata.Quantity asc";
        //        }
        //        else if (filters.order == "descending/quantity")
        //        {
        //            subquery = "order by _podata.Quantity desc";
        //        }
        //        else if (filters.order == "ascending/invoice")
        //        {
        //            subquery = "order by _podata.InvoiceNo asc";
        //        }
        //        else if (filters.order == "descending/invoice")
        //        {
        //            subquery = "order by _podata.InvoiceNo desc";
        //        }
        //        else if (filters.order == "ascending/purchase")
        //        {
        //            subquery = "order by _podata.PurchaseDate asc";
        //        }
        //        else if (filters.order == "descending/purchase")
        //        {
        //            subquery = "order by _podata.PurchaseDate desc";
        //        }
        //        else if (filters.order == "ascending/wanranty")
        //        {
        //            subquery = "order by _podata.WanrantyAvailable asc";
        //        }
        //        else if (filters.order == "descending/wanranty")
        //        {
        //            subquery = "order by _podata.WanrantyAvailable desc";
        //        }
        //        else if (filters.order == "ascending/description")
        //        {
        //            subquery = "order by _podata.Description asc";
        //        }
        //        else if (filters.order == "descending/description")
        //        {
        //            subquery = "order by _podata.Description desc";
        //        }
        //        else if (filters.order == "ascending/status")
        //        {
        //            subquery = "order by _podata.Status asc";
        //        }
        //        else if (filters.order == "descending/status")
        //        {
        //            subquery = "order by _podata.Status desc";
        //        }
        //    }
        //    else
        //    {
        //        subquery = "order by _podata.Id desc";
        //    }
        //    var TechnicanQuery = "";
        //    if (filters.TechnicianId != Guid.Empty)
        //    {
        //        TechnicanQuery = " AND EqR.TechnicianId='" + filters.TechnicianId + "'";
        //    }
        //    try
        //    {
        //        sqlQuery = string.Format(sqlQuery,
        //                                filters.Searchtext,//0
        //                                1,  //1
        //                                100000000, //2
        //                                filters.CompanyId, //3
        //                                SearchTextFilter,//4
        //                                subquery,//5
        //                                TechnicanQuery,//6
        //                                filters.Start,//7
        //                                filters.End,//8
        //                                NamingSql,//9
        //                                StatusIDList,//10
        //                                TechnicianIDList,//11
        //                                Purchase_DateFilter//12
        //                                );
        //        using (SqlCommand cmd = GetSQLCommand(sqlQuery))
        //        {
        //            DataSet dsResult = GetDataSet(cmd);
        //            return dsResult.Tables[0];
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //public DataSet GetPurchaseOrderListByFiltersBranch(PurchaseOrderFilter filters)
        //{
        //    string SearchTextFilter = "";
        //    string EmployeeIdFilter = "";
        //    if (filters.EmployeeId != Guid.Empty)
        //    {
        //        EmployeeIdFilter = "And ptech.TechnicianId='" + filters.EmployeeId + "'";
        //    }
        //    if (!string.IsNullOrWhiteSpace(filters.Searchtext))
        //    {
        //        SearchTextFilter = @"and( isnull(ptech.DemandOrderId, '') like @SearchText 
        //                            or emp.FirstName + ' ' + emp.LastName like @SearchText
        //                            or ptech.[Status] like @SearchText)";
        //    }
        //    string sqlQuery = @"DECLARE @CompanyId uniqueidentifier
        //                        DECLARE @pagestart int
        //                        DECLARE @pageend int
        //                        DECLARE @pageno int
        //                        DECLARE @pagesize int
        //                        DECLARE @SearchText nvarchar(50)

        //                         SET @SearchText = '%{0}%'
        //                        SET @pageno = {1}--default 1
        //                        SET @pagesize = {2}--default 10
        //                        SET @CompanyId = '{3}'--97BCF758 - A482 - 47EB - 82B8 - F88BF12293FF

        //                        SET @pagestart = (@pageno - 1) * @pagesize
        //                        SET @pageend = @pagesize


        //                        select * into #POData 
        //                        from(select pt.Id as DOTId,ptech.*
        //                        ,pt.TicketId
        //                        ,emp.FirstName+' '+emp.LastName as TechName
								//,emp.Email
        //                        from PurchaseOrderBranch ptech
        //                        LEFT JOIN PurchaseOrderTech pt on pt.DemandOrderId=ptech.TechDemandOrderId
								//LEFT JOIN Employee emp on emp.UserId=pt.TechnicianId
        //                        where ptech.CompanyId = @CompanyId
        //                        --{5}
        //                        {4}
        //                        and ptech.[Status] != 'Init'
        //                        and ptech.[Status] != ''
        //                            ) as POD


        //                            SELECT TOP(@pagesize) * FROM #POData _podata
        //                            where   Id NOT IN(Select TOP(@pagestart) Id from #POData)
	       //                         {6}
        //                        select  count(Id) as [TotalCount] from #POData 
        //                        DROP TABLE #POData";
        //    string subquery = "order by _podata.DemandOrderId desc";
        //    if (!string.IsNullOrWhiteSpace(filters.order))
        //    {
        //        if (filters.order == "ascending/dono")
        //        {
        //            subquery = "order by _podata.DemandOrderId asc";
        //        }
        //        else if (filters.order == "descending/dono")
        //        {
        //            subquery = "order by _podata.DemandOrderId desc";
        //        }
        //        else if (filters.order == "ascending/techdetails")
        //        {
        //            subquery = "order by _podata.TechName asc";
        //        }
        //        else if (filters.order == "descending/techdetails")
        //        {
        //            subquery = "order by _podata.TechName desc";
        //        }
        //        else if (filters.order == "ascending/status")
        //        {
        //            subquery = "order by _podata.Status asc";
        //        }
        //        else if (filters.order == "descending/status")
        //        {
        //            subquery = "order by _podata.Status desc";
        //        }
        //        else if (filters.order == "ascending/createddate")
        //        {
        //            subquery = "order by _podata.CreatedDate asc";
        //        }
        //        else if (filters.order == "descending/createddate")
        //        {
        //            subquery = "order by _podata.CreatedDate desc";
        //        }
        //    }
        //    try
        //    {
        //        sqlQuery = string.Format(sqlQuery,
        //                                filters.Searchtext,//0
        //                                filters.PageNo,  //1
        //                                filters.PageSize, //2
        //                                filters.CompanyId, //3
        //                                SearchTextFilter, //4
        //                                EmployeeIdFilter, //5,
        //                                subquery
        //                                );
        //        using (SqlCommand cmd = GetSQLCommand(sqlQuery))
        //        {
        //            DataSet dsResult = GetDataSet(cmd);
        //            return dsResult;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public DataSet GetPurchaseOrderListByFiltersTech(PurchaseOrderFilter filters)
        {
            string SearchTextFilter = "";
            string EmployeeIdFilter = "";
            if (filters.EmployeeId != Guid.Empty)
            {
                EmployeeIdFilter = "And ptech.TechnicianId='" + filters.EmployeeId + "'";
            }
            if (!string.IsNullOrWhiteSpace(filters.Searchtext) && filters.Searchtext != "undefined")
            {
                SearchTextFilter = @"and(isnull(ptech.[DemandOrderId], '') like @SearchText 
                                    or ptech.[Status] like @SearchText)";
            }
            string sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int
                                DECLARE @SearchText nvarchar(50)

                                 SET @SearchText = '%{0}%'
                                SET @pageno = {1}--default 1
                                SET @pagesize = {2}--default 10
                                SET @CompanyId = '{3}'--97BCF758 - A482 - 47EB - 82B8 - F88BF12293FF

                                SET @pagestart = (@pageno - 1) * @pagesize
                                SET @pageend = @pagesize


                                select * into #POData 
                                from(select ptech.*
                                from PurchaseOrderTech ptech

                                where ptech.CompanyId = @CompanyId
                                {5}
                                {4}
                                and ptech.[Status] != 'Init'
                                and ptech.[Status] != ''
                                    ) as POD


                                    SELECT TOP(@pagesize) * FROM #POData _podata
                                    where   Id NOT IN(Select TOP(@pagestart) Id from #POData)
	                                {6}
                                select  count(Id) as [TotalCount] from #POData 
                                DROP TABLE #POData";
            string subquery = "order by _podata.DemandOrderId desc";
            if (!string.IsNullOrWhiteSpace(filters.order))
            {
                if (filters.order == "ascending/dono")
                {
                    subquery = "order by _podata.DemandOrderId asc";
                }
                else if (filters.order == "descending/dono")
                {
                    subquery = "order by _podata.DemandOrderId desc";
                }
                else if (filters.order == "ascending/status")
                {
                    subquery = "order by _podata.Status asc";
                }
                else if (filters.order == "descending/status")
                {
                    subquery = "order by _podata.Status desc";
                }
                else if (filters.order == "ascending/createddate")
                {
                    subquery = "order by _podata.CreatedDate asc";
                }
                else if (filters.order == "descending/createddate")
                {
                    subquery = "order by _podata.CreatedDate desc";
                }
            }

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        filters.Searchtext,//0
                                        filters.PageNo,  //1
                                        filters.PageSize, //2
                                        filters.CompanyId, //3
                                        SearchTextFilter, //4
                                        EmployeeIdFilter, //5,
                                        subquery
                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetRequestOrderListFilter(DateTime? startDate, DateTime? endDate, Guid userId)
        {
            string datestart = "";
            string dateend = "";
            string subQuery = "";
            string subQuery2 = "";
            if (startDate != null && startDate != new DateTime() && endDate != null && endDate != new DateTime())
            {
                datestart = startDate.Value.ToString("yyyy-MM-dd 00:00:00.000");
                dateend = endDate.Value.ToString("yyyy-MM-dd 23:59:58.999");
                subQuery = string.Format("and ticket.CompletionDate between '{0}' and '{1}'", datestart, dateend);
                subQuery2= string.Format("and _tic.CompletionDate between '{0}' and '{1}'", datestart, dateend);
            }

            //string sqlQuery = @"select distinct cae.Id,tech.TechnicianId, tech.EquipmentId, ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=eqp.EquipmentId and Type='Add')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=eqp.EquipmentId and Type='Release')) as WarehouseQTY
            //                  ,iif((isnull((select sum(Quantity) from InventoryTech where EquipmentId = tech.EquipmentId and TechnicianId = tech.TechnicianId and [Type] = 'Add'), 0) - isnull((select sum(Quantity) from InventoryTech where EquipmentId = tech.EquipmentId and TechnicianId = tech.TechnicianId and [Type] = 'Release'), 0)) < 0, 0, isnull((select sum(Quantity) from InventoryTech where EquipmentId = tech.EquipmentId and TechnicianId = tech.TechnicianId and [Type] = 'Add'), 0) - isnull((select sum(Quantity) from InventoryTech where EquipmentId = tech.EquipmentId and TechnicianId = tech.TechnicianId and [Type] = 'Release'), 0)) as QTYOnHand
            //                  ,eqp.SKU as ProductSKU, manu.Name as ManufacturerName, eqp.Name as EquipmentName
            //                  ,iif(cae.Quantity > iif((isnull((select sum(Quantity) from InventoryTech where EquipmentId = tech.EquipmentId and TechnicianId = tech.TechnicianId and [Type] = 'Add'), 0) - isnull((select sum(Quantity) from InventoryTech where EquipmentId = tech.EquipmentId and TechnicianId = tech.TechnicianId and [Type] = 'Release'), 0)) < 0, 0, isnull((select sum(Quantity) from InventoryTech where EquipmentId = tech.EquipmentId and TechnicianId = tech.TechnicianId and [Type] = 'Add'), 0) - isnull((select sum(Quantity) from InventoryTech where EquipmentId = tech.EquipmentId and TechnicianId = tech.TechnicianId and [Type] = 'Release'), 0)), cae.Quantity - iif((isnull((select sum(Quantity) from InventoryTech where EquipmentId = tech.EquipmentId and TechnicianId = tech.TechnicianId and [Type] = 'Add'), 0) - isnull((select sum(Quantity) from InventoryTech where EquipmentId = tech.EquipmentId and TechnicianId = tech.TechnicianId and [Type] = 'Release'), 0)) < 0, 0, isnull((select sum(Quantity) from InventoryTech where EquipmentId = tech.EquipmentId and TechnicianId = tech.TechnicianId and [Type] = 'Add'), 0) - isnull((select sum(Quantity) from InventoryTech where EquipmentId = tech.EquipmentId and TechnicianId = tech.TechnicianId and [Type] = 'Release'), 0)) , 0) as QTYNeeded
            //                  from InventoryTech tech
            //                  left join CustomerAppointmentEquipment cae on cae.EquipmentId = tech.EquipmentId
            //                  left join Equipment eqp on eqp.EquipmentId = cae.EquipmentId
            //                  left join Manufacturer manu on manu.Id = eqp.ManufacturerId
            //                  left Join TicketUser tu on tu.TiketId = cae.AppointmentId and tu.IsPrimary = 1
            //                  left join Ticket ticket on ticket.TicketId = tu.TiketId
            //                  where tech.TechnicianId = '{0}'
            //                   {1}";
            string sqlQuery = @"select distinct cae.EquipmentId,tu.UserId as TechnicianId, ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=eqp.EquipmentId and Type='Add')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=eqp.EquipmentId and Type='Release')) as WarehouseQTY
                              ,iif((isnull((select sum(Quantity) from InventoryTech where EquipmentId = cae.EquipmentId and TechnicianId = tu.UserId and [Type] = 'Add'), 0) - isnull((select sum(Quantity) from InventoryTech where EquipmentId = cae.EquipmentId and TechnicianId = tu.UserId and [Type] = 'Release'), 0)) < 0, 0, isnull((select sum(Quantity) from InventoryTech where EquipmentId = cae.EquipmentId and TechnicianId = tu.UserId and [Type] = 'Add'), 0) - isnull((select sum(Quantity) from InventoryTech where EquipmentId = cae.EquipmentId and TechnicianId = tu.UserId and [Type] = 'Release'), 0)) as QTYOnHand
                              ,eqp.SKU as ProductSKU, manu.Name as ManufacturerName, eqp.Name as EquipmentName
							  ,(select sum(caeTemp.Quantity) from CustomerAppointmentEquipment caeTemp left join TicketUser _tu on _tu.TiketId=caeTemp.AppointmentId left join Ticket _tic on _tic.TicketId=_tu.TiketId where caeTemp.EquipmentId=cae.EquipmentId and _tu.UserId = '{0}' {2}) as QTYNeeded  
							  ,STUFF((SELECT ', ' + CAST(_tic.Id AS nvarchar(150)) [text()]
                              from Ticket _tic left join TicketUser _tu on _tu.TiketId=_tic.TicketId left join CustomerAppointmentEquipment _cae on _tic.TicketId=_cae.AppointmentId
		                       where _cae.EquipmentId=cae.EquipmentId  and _tu.UserId = '{0}' {2}
                              FOR XML PATH(''), TYPE)
                              .value('.','NVARCHAR(MAX)'),1,2,' ') as TicketsId
							  ,(iif(((select sum(podTemp.Quantity) from PurchaseOrderDetail podTemp left join PurchaseOrderWarehouse powTemp on powTemp.PurchaseOrderId=podTemp.PurchaseOrderId where powTemp.POFor='{0}' and powTemp.Status='Created' and podTemp.EquipmentId=cae.EquipmentId) - (iif((select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp left join PurchaseOrderWarehouse powTemp on powTemp.PurchaseOrderId=podTemp.PurchaseOrderId where powTemp.POFor='{0}' and powTemp.Status='Created' and podTemp.EquipmentId=cae.EquipmentId) is not null,(select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp left join PurchaseOrderWarehouse powTemp on powTemp.PurchaseOrderId=podTemp.PurchaseOrderId where powTemp.POFor='{0}' and powTemp.Status='Created' and podTemp.EquipmentId=cae.EquipmentId),0))) > 0,((select sum(podTemp.Quantity) from PurchaseOrderDetail podTemp left join PurchaseOrderWarehouse powTemp on powTemp.PurchaseOrderId=podTemp.PurchaseOrderId where powTemp.POFor='{0}' and powTemp.Status='Created' and podTemp.EquipmentId=cae.EquipmentId) - (iif((select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp left join PurchaseOrderWarehouse powTemp on powTemp.PurchaseOrderId=podTemp.PurchaseOrderId where powTemp.POFor='{0}' and powTemp.Status='Created' and podTemp.EquipmentId=cae.EquipmentId) is not null,(select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp left join PurchaseOrderWarehouse powTemp on powTemp.PurchaseOrderId=podTemp.PurchaseOrderId where powTemp.POFor='{0}' and powTemp.Status='Created' and podTemp.EquipmentId=cae.EquipmentId),0))),0)) as QTYPending
							  from CustomerAppointmentEquipment cae 
                              left join Equipment eqp on eqp.EquipmentId = cae.EquipmentId
                              left join Manufacturer manu on manu.Id = eqp.ManufacturerId
                              left Join TicketUser tu on tu.TiketId = cae.AppointmentId and tu.IsPrimary = 1
                              left join Ticket ticket on ticket.TicketId = tu.TiketId
                              where tu.UserId = '{0}'
							  and eqp.EquipmentClassId=1
                               {1}";

            try
            {
                sqlQuery = string.Format(sqlQuery, userId, subQuery, subQuery2);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetCompletedInventoryListFilter(int? PageNo, int? PageSize,DateTime startDate, DateTime endDate, Guid companyId, string SearchText,string order)
        {
            string datestart = "";
            string dateend = "";
            string subQuery = "";
            string searchQuery = "";
            string orderQuery1 = "";
            string orderQuery2 = "";
            if(!string.IsNullOrWhiteSpace(SearchText))
            {
                searchQuery = string.Format("and ( _eqpType.Name like '%{0}%' or _eqp.Name like '%{0}%' or _eqp.SKU like '%{0}%')", SearchText);
            }
            if (startDate != null && startDate != new DateTime() && endDate != null && endDate != new DateTime())
            {
                datestart = startDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                dateend = endDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                subQuery = string.Format("and ticket.CompletionDate between '{0}' and '{1}'", datestart, dateend);
            }

            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/producttype")
                {
                    orderQuery1 = "order by #InventoryTechData.ProductType asc";
                    orderQuery2 = "order by ProductType asc";
                }
                else if (order == "descending/producttype")
                {
                    orderQuery1 = "order by #InventoryTechData.ProductType desc";
                    orderQuery2 = "order by ProductType desc";
                }
                if (order == "ascending/productname")
                {
                    orderQuery1 = "order by #InventoryTechData.EquipmentName asc";
                    orderQuery2 = "order by EquipmentName asc";
                }
                else if (order == "descending/productname")
                {
                    orderQuery1 = "order by #InventoryTechData.EquipmentName desc";
                    orderQuery2 = "order by EquipmentName desc";
                }

                else
                {
                    orderQuery1 = "order by #InventoryTechData.[Id]  desc";
                    orderQuery2 = "order by Id desc";
                }

            }
            else
            {
                orderQuery1 = "order by #InventoryTechData.[Id] desc";
                orderQuery2 = "order by Id desc";
            }
            #endregion

            string sqlQuery = @"
                                DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int


                                SET @pageno = {2} --default 1
                                SET @pagesize = {3} --default 10


                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

 
                                SELECT 
                                        distinct _eqp.*,
	                                    ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=_eqp.EquipmentId and Type='Release')) as Quantity,
                                          _eqpType.Name as Category,
                                          eqpv.Cost as VendorCost,
										  (iif(((select sum(podTemp.Quantity) from PurchaseOrderDetail podTemp where  podTemp.EquipmentId=_eqp.EquipmentId) - (iif((select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId) is not null,(select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId),0))) > 0,((select sum(podTemp.Quantity) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId) - (iif((select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId) is not null,(select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId),0))),0)) as QTYOrdered,

										  --CostQTYOrdered 
                                          Format((iif(((select sum(podTemp.Quantity) from PurchaseOrderDetail podTemp where  podTemp.EquipmentId=_eqp.EquipmentId) - (iif((select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId) is not null,(select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId),0))) > 0,((select sum(podTemp.Quantity) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId) - (iif((select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId) is not null,(select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId),0))),0)) *eqpv.Cost,'N2') as CostQTYOrdered ,

										  (select ISNULL(SUM(caeTemp.QuantityLeftEquipment),0) from CustomerAppointmentEquipment caeTemp left join Ticket ticTemp on caeTemp.AppointmentId=ticTemp.TicketId where ticTemp.TicketType='Installation' and caeTemp.EquipmentId=_eqp.EquipmentId) as QTYUsededInstall,
										  
                                          --CostQTYUsededInstall
                                          Format((select ISNULL(SUM(caeTemp.QuantityLeftEquipment),0) from CustomerAppointmentEquipment caeTemp left join Ticket ticTemp on caeTemp.AppointmentId=ticTemp.TicketId where ticTemp.TicketType='Installation' and caeTemp.EquipmentId=_eqp.EquipmentId) *eqpv.Cost,'N2') as CostQTYUsededInstall,
										  
                                          (select ISNULL(SUM(caeTemp.QuantityLeftEquipment),0) from CustomerAppointmentEquipment caeTemp left join Ticket ticTemp on caeTemp.AppointmentId=ticTemp.TicketId where ticTemp.TicketType='Service' and caeTemp.EquipmentId=_eqp.EquipmentId) as QTYUsededService,
                                          --CostQTYUsededService
										  Format((select ISNULL(SUM(caeTemp.QuantityLeftEquipment),0) from CustomerAppointmentEquipment caeTemp left join Ticket ticTemp on caeTemp.AppointmentId=ticTemp.TicketId where ticTemp.TicketType='Service' and caeTemp.EquipmentId=_eqp.EquipmentId) *eqpv.Cost,'N2') as CostQTYUsededService,
										  
                                        (iif(((select sum(podTemp.Quantity) from PurchaseOrderDetail podTemp left join PurchaseOrderWarehouse powTemp on powTemp.PurchaseOrderId=podTemp.PurchaseOrderId where powTemp.Status='Created' and podTemp.EquipmentId=_eqp.EquipmentId) - (iif((select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp left join PurchaseOrderWarehouse powTemp on powTemp.PurchaseOrderId=podTemp.PurchaseOrderId where powTemp.Status='Created' and podTemp.EquipmentId=_eqp.EquipmentId) is not null,(select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp left join PurchaseOrderWarehouse powTemp on powTemp.PurchaseOrderId=podTemp.PurchaseOrderId where powTemp.Status='Created' and podTemp.EquipmentId=_eqp.EquipmentId),0))) > 0,((select sum(podTemp.Quantity) from PurchaseOrderDetail podTemp left join PurchaseOrderWarehouse powTemp on powTemp.PurchaseOrderId=podTemp.PurchaseOrderId where powTemp.Status='Created' and podTemp.EquipmentId=_eqp.EquipmentId) - (iif((select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp left join PurchaseOrderWarehouse powTemp on powTemp.PurchaseOrderId=podTemp.PurchaseOrderId where powTemp.Status='Created' and podTemp.EquipmentId=_eqp.EquipmentId) is not null,(select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp left join PurchaseOrderWarehouse powTemp on powTemp.PurchaseOrderId=podTemp.PurchaseOrderId where powTemp.Status='Created' and podTemp.EquipmentId=_eqp.EquipmentId),0))),0)) as QTYPending,
                                         
                                          
                                          (select ISNULL(SUM(tech.Quantity), 0) from InventoryTech tech where _eqp.EquipmentId = tech.EquipmentId and tech.[Type] = 'Add') - (select ISNULL(SUM(tech.Quantity), 0) from InventoryTech tech where _eqp.EquipmentId = tech.EquipmentId and tech.[Type] = 'Release') as technician,
										  ISNULL((((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=_eqp.EquipmentId and Type='Release'))
										  +
										  (select ISNULL(SUM(tech.Quantity), 0) from InventoryTech tech where _eqp.EquipmentId = tech.EquipmentId and tech.[Type] = 'Add') - (select ISNULL(SUM(tech.Quantity), 0) from InventoryTech tech where _eqp.EquipmentId = tech.EquipmentId and tech.[Type] = 'Release')), 0) as TotalEq,
                                          
                                          --CurrentInventoryValue
										  Format(ISNULL((((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=_eqp.EquipmentId and Type='Release'))
										  +
										  (select ISNULL(SUM(tech.Quantity), 0) from InventoryTech tech where _eqp.EquipmentId = tech.EquipmentId and tech.[Type] = 'Add') - (select ISNULL(SUM(tech.Quantity), 0) from InventoryTech tech where _eqp.EquipmentId = tech.EquipmentId and tech.[Type] = 'Release')), 0)*eqpv.Cost,'N2') as CurrentInventoryValue
                                          
                                          INTO #CustomerData
                                          FROM Equipment _eqp
                                            left JOIN EquipmentVendor eqpv
											ON eqpv.EquipmentId = _eqp.EquipmentId
                                            and eqpv.IsPrimary=1
											 LEFT JOIN EquipmentType _eqpType
		                                    ON _eqp.EquipmentTypeId = _eqpType.Id and _eqp.CompanyId = _eqpType.CompanyId
											left join CustomerAppointmentEquipment cae on cae.EquipmentId = _eqp.EquipmentId
											left join Ticket ticket on ticket.TicketId = cae.AppointmentId
		                                    WHERE 
			                                    _eqp.CompanyId = '{0}'
                                                and ticket.Status='Completed'
                                                AND _eqp.EquipmentClassId = '1'
                                                {1}
								                {4}
                                   
                                           SELECT * INTO #CustomerFilterData
                                           FROM #CustomerData
                                           
                                
	                                       SELECT TOP (@pagesize)
                                           * into #Testtable
                                           FROM #CustomerFilterData _cfd
                                           where   Id NOT IN(Select TOP (@pagestart)  Id from #CustomerData _cd order by _cd.Category)
                                           order by _cfd.Category

                                            select *  from #Testtable
							      --          select sum(FinancedAmount) as TotalAmountByPage from #TestTable 

                                           select count(*) [TotalCount]
                                           from #CustomerFilterData

                                           DROP TABLE #CustomerData
                                           DROP TABLE #CustomerFilterData 
                                           drop table #Testtable
                                          ";

            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, subQuery,PageNo,PageSize, searchQuery, orderQuery1, orderQuery2);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable GetCompletedInventoryListFilterReport(Guid companyId, DateTime? Start, DateTime? End, string SearchText)
        {
            string sqlQuery = @"";
            string searchQuery = "";
            string sdate = "";
            string edate = "";
            string qtype = "";
            string qstatus = "";
            string qassigned = "";
            string filterquery = "";

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                searchQuery = string.Format("and ( _eqpType.Name like '%{0}%' or _eqp.Name like '%{0}%' or _eqp.SKU like '%{0}%')", SearchText);
            }
            
            if (Start.HasValue && End.HasValue)
            {
                sqlQuery = @"
                                       SELECT 
                                        distinct _eqp.EquipmentId,_eqp.Name as [Product Name],_eqp.SKU,_eqpType.Name as [Product Type], Format(eqpv.Cost,'N2') as [Cost Per Item],
                                         (iif(((select sum(podTemp.Quantity) from PurchaseOrderDetail podTemp where  podTemp.EquipmentId=_eqp.EquipmentId) - (iif((select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId) is not null,(select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId),0))) > 0,((select sum(podTemp.Quantity) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId) - (iif((select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId) is not null,(select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId),0))),0)) as [Quantity Ordered],
										  Format((iif(((select sum(podTemp.Quantity) from PurchaseOrderDetail podTemp where  podTemp.EquipmentId=_eqp.EquipmentId) - (iif((select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId) is not null,(select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId),0))) > 0,((select sum(podTemp.Quantity) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId) - (iif((select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId) is not null,(select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId),0))),0)) *eqpv.Cost,'N2') as [Cost of Quantity Ordered] ,
                                         (select ISNULL(SUM(caeTemp.QuantityLeftEquipment),0) from CustomerAppointmentEquipment caeTemp left join Ticket ticTemp on caeTemp.AppointmentId=ticTemp.TicketId where ticTemp.TicketType='Installation' and caeTemp.EquipmentId=_eqp.EquipmentId) as [Quantity Used on Installs],
										  Format((select ISNULL(SUM(caeTemp.QuantityLeftEquipment),0) from CustomerAppointmentEquipment caeTemp left join Ticket ticTemp on caeTemp.AppointmentId=ticTemp.TicketId where ticTemp.TicketType='Installation' and caeTemp.EquipmentId=_eqp.EquipmentId) *eqpv.Cost,'N2') as [Cost of Quantity Used on Installs],
                                          (select ISNULL(SUM(caeTemp.QuantityLeftEquipment),0) from CustomerAppointmentEquipment caeTemp left join Ticket ticTemp on caeTemp.AppointmentId=ticTemp.TicketId where ticTemp.TicketType='Service' and caeTemp.EquipmentId=_eqp.EquipmentId) as [Quantity Used on Services],
										  Format((select ISNULL(SUM(caeTemp.QuantityLeftEquipment),0) from CustomerAppointmentEquipment caeTemp left join Ticket ticTemp on caeTemp.AppointmentId=ticTemp.TicketId where ticTemp.TicketType='Service' and caeTemp.EquipmentId=_eqp.EquipmentId) *eqpv.Cost,'N2') as [Cost of Quantity Used on Services],
	                                    ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=_eqp.EquipmentId and Type='Release')) as [Warehouse Quantity],
                                         (select ISNULL(SUM(tech.Quantity), 0) from InventoryTech tech where _eqp.EquipmentId = tech.EquipmentId and tech.[Type] = 'Add') - (select ISNULL(SUM(tech.Quantity), 0) from InventoryTech tech where _eqp.EquipmentId = tech.EquipmentId and tech.[Type] = 'Release') as Technician,
										  ISNULL((((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=_eqp.EquipmentId and Type='Release'))
										  +
										  (select ISNULL(SUM(tech.Quantity), 0) from InventoryTech tech where _eqp.EquipmentId = tech.EquipmentId and tech.[Type] = 'Add') - (select ISNULL(SUM(tech.Quantity), 0) from InventoryTech tech where _eqp.EquipmentId = tech.EquipmentId and tech.[Type] = 'Release')), 0) as [Total Equipment], 
                                          
										  
										  (iif(((select sum(podTemp.Quantity) from PurchaseOrderDetail podTemp left join PurchaseOrderWarehouse powTemp on powTemp.PurchaseOrderId=podTemp.PurchaseOrderId where powTemp.Status='Created' and podTemp.EquipmentId=_eqp.EquipmentId) - (iif((select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp left join PurchaseOrderWarehouse powTemp on powTemp.PurchaseOrderId=podTemp.PurchaseOrderId where powTemp.Status='Created' and podTemp.EquipmentId=_eqp.EquipmentId) is not null,(select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp left join PurchaseOrderWarehouse powTemp on powTemp.PurchaseOrderId=podTemp.PurchaseOrderId where powTemp.Status='Created' and podTemp.EquipmentId=_eqp.EquipmentId),0))) > 0,((select sum(podTemp.Quantity) from PurchaseOrderDetail podTemp left join PurchaseOrderWarehouse powTemp on powTemp.PurchaseOrderId=podTemp.PurchaseOrderId where powTemp.Status='Created' and podTemp.EquipmentId=_eqp.EquipmentId) - (iif((select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp left join PurchaseOrderWarehouse powTemp on powTemp.PurchaseOrderId=podTemp.PurchaseOrderId where powTemp.Status='Created' and podTemp.EquipmentId=_eqp.EquipmentId) is not null,(select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp left join PurchaseOrderWarehouse powTemp on powTemp.PurchaseOrderId=podTemp.PurchaseOrderId where powTemp.Status='Created' and podTemp.EquipmentId=_eqp.EquipmentId),0))),0)) as [Pending Quantity on Order],
                                         
										  Format(ISNULL((((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=_eqp.EquipmentId and Type='Release'))
										  +
										  (select ISNULL(SUM(tech.Quantity), 0) from InventoryTech tech where _eqp.EquipmentId = tech.EquipmentId and tech.[Type] = 'Add') - (select ISNULL(SUM(tech.Quantity), 0) from InventoryTech tech where _eqp.EquipmentId = tech.EquipmentId and tech.[Type] = 'Release')), 0)*eqpv.Cost,'N2') as [Current Inventory Value]
                                          
                                          --INTO #CustomerData
                                          FROM Equipment _eqp
                                            left JOIN EquipmentVendor eqpv
											ON eqpv.EquipmentId = _eqp.EquipmentId
                                            and eqpv.IsPrimary=1
											 LEFT JOIN EquipmentType _eqpType
		                                    ON _eqp.EquipmentTypeId = _eqpType.Id and _eqp.CompanyId = _eqpType.CompanyId
											left join CustomerAppointmentEquipment cae on cae.EquipmentId = _eqp.EquipmentId
											left join Ticket ticket on ticket.TicketId = cae.AppointmentId
		                                    WHERE 
			                                    _eqp.CompanyId = '{0}'
                                                and ticket.Status='Completed'
                                                AND _eqp.EquipmentClassId = '1'
                                                and ticket.CompletionDate between '{1}' and '{2}'
								                {3}
                                           order by [Product Type]
                            ";
                sdate = Start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
                edate = End.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            else
            {
                sqlQuery = @"SELECT 
                                        distinct _eqp.EquipmentId,_eqp.Name as [Product Name],_eqp.SKU,_eqpType.Name as [Product Type], Format(eqpv.Cost,'N2') as [Cost Per Item],
                                         (iif(((select sum(podTemp.Quantity) from PurchaseOrderDetail podTemp where  podTemp.EquipmentId=_eqp.EquipmentId) - (iif((select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId) is not null,(select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId),0))) > 0,((select sum(podTemp.Quantity) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId) - (iif((select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId) is not null,(select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId),0))),0)) as [Quantity Ordered],
										  Format((iif(((select sum(podTemp.Quantity) from PurchaseOrderDetail podTemp where  podTemp.EquipmentId=_eqp.EquipmentId) - (iif((select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId) is not null,(select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId),0))) > 0,((select sum(podTemp.Quantity) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId) - (iif((select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId) is not null,(select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp where podTemp.EquipmentId=_eqp.EquipmentId),0))),0)) *eqpv.Cost,'N2') as [Cost of Quantity Ordered] ,
                                         (select ISNULL(SUM(caeTemp.QuantityLeftEquipment),0) from CustomerAppointmentEquipment caeTemp left join Ticket ticTemp on caeTemp.AppointmentId=ticTemp.TicketId where ticTemp.TicketType='Installation' and caeTemp.EquipmentId=_eqp.EquipmentId) as [Quantity Used on Installs],
										  Format((select ISNULL(SUM(caeTemp.QuantityLeftEquipment),0) from CustomerAppointmentEquipment caeTemp left join Ticket ticTemp on caeTemp.AppointmentId=ticTemp.TicketId where ticTemp.TicketType='Installation' and caeTemp.EquipmentId=_eqp.EquipmentId) *eqpv.Cost,'N2') as [Cost of Quantity Used on Installs],
                                          (select ISNULL(SUM(caeTemp.QuantityLeftEquipment),0) from CustomerAppointmentEquipment caeTemp left join Ticket ticTemp on caeTemp.AppointmentId=ticTemp.TicketId where ticTemp.TicketType='Service' and caeTemp.EquipmentId=_eqp.EquipmentId) as [Quantity Used on Services],
										  Format((select ISNULL(SUM(caeTemp.QuantityLeftEquipment),0) from CustomerAppointmentEquipment caeTemp left join Ticket ticTemp on caeTemp.AppointmentId=ticTemp.TicketId where ticTemp.TicketType='Service' and caeTemp.EquipmentId=_eqp.EquipmentId) *eqpv.Cost,'N2') as [Cost of Quantity Used on Services],
	                                    ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=_eqp.EquipmentId and Type='Release')) as [Warehouse Quantity],
                                         (select ISNULL(SUM(tech.Quantity), 0) from InventoryTech tech where _eqp.EquipmentId = tech.EquipmentId and tech.[Type] = 'Add') - (select ISNULL(SUM(tech.Quantity), 0) from InventoryTech tech where _eqp.EquipmentId = tech.EquipmentId and tech.[Type] = 'Release') as Technician,
										  ISNULL((((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=_eqp.EquipmentId and Type='Release'))
										  +
										  (select ISNULL(SUM(tech.Quantity), 0) from InventoryTech tech where _eqp.EquipmentId = tech.EquipmentId and tech.[Type] = 'Add') - (select ISNULL(SUM(tech.Quantity), 0) from InventoryTech tech where _eqp.EquipmentId = tech.EquipmentId and tech.[Type] = 'Release')), 0) as [Total Equipment], 
                                          
										  
										  (iif(((select sum(podTemp.Quantity) from PurchaseOrderDetail podTemp left join PurchaseOrderWarehouse powTemp on powTemp.PurchaseOrderId=podTemp.PurchaseOrderId where powTemp.Status='Created' and podTemp.EquipmentId=_eqp.EquipmentId) - (iif((select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp left join PurchaseOrderWarehouse powTemp on powTemp.PurchaseOrderId=podTemp.PurchaseOrderId where powTemp.Status='Created' and podTemp.EquipmentId=_eqp.EquipmentId) is not null,(select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp left join PurchaseOrderWarehouse powTemp on powTemp.PurchaseOrderId=podTemp.PurchaseOrderId where powTemp.Status='Created' and podTemp.EquipmentId=_eqp.EquipmentId),0))) > 0,((select sum(podTemp.Quantity) from PurchaseOrderDetail podTemp left join PurchaseOrderWarehouse powTemp on powTemp.PurchaseOrderId=podTemp.PurchaseOrderId where powTemp.Status='Created' and podTemp.EquipmentId=_eqp.EquipmentId) - (iif((select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp left join PurchaseOrderWarehouse powTemp on powTemp.PurchaseOrderId=podTemp.PurchaseOrderId where powTemp.Status='Created' and podTemp.EquipmentId=_eqp.EquipmentId) is not null,(select sum(podTemp.RecieveQty) from PurchaseOrderDetail podTemp left join PurchaseOrderWarehouse powTemp on powTemp.PurchaseOrderId=podTemp.PurchaseOrderId where powTemp.Status='Created' and podTemp.EquipmentId=_eqp.EquipmentId),0))),0)) as [Pending Quantity on Order],
                                         
										  Format(ISNULL((((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=_eqp.EquipmentId and Type='Release'))
										  +
										  (select ISNULL(SUM(tech.Quantity), 0) from InventoryTech tech where _eqp.EquipmentId = tech.EquipmentId and tech.[Type] = 'Add') - (select ISNULL(SUM(tech.Quantity), 0) from InventoryTech tech where _eqp.EquipmentId = tech.EquipmentId and tech.[Type] = 'Release')), 0)*eqpv.Cost,'N2') as [Current Inventory Value]
                                          
                                          --INTO #CustomerData
                                          FROM Equipment _eqp
                                            left JOIN EquipmentVendor eqpv
											ON eqpv.EquipmentId = _eqp.EquipmentId
                                            and eqpv.IsPrimary=1
											 LEFT JOIN EquipmentType _eqpType
		                                    ON _eqp.EquipmentTypeId = _eqpType.Id and _eqp.CompanyId = _eqpType.CompanyId
											left join CustomerAppointmentEquipment cae on cae.EquipmentId = _eqp.EquipmentId
											left join Ticket ticket on ticket.TicketId = cae.AppointmentId
		                                    WHERE 
			                                    _eqp.CompanyId = '{0}'
                                                and ticket.Status='Completed'
                                                AND _eqp.EquipmentClassId = '1'
								                {3}
                                           order by [Product Type]
                            ";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, sdate, edate, searchQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #region PO Report

        public DataSet GetCreatedPurchaseOrderListByFilters(PurchaseOrderFilter filters, DateTime? start, DateTime? end)
        {
            string SearchTextFilter = "";
            string DateQuery = "";
            if (!string.IsNullOrWhiteSpace(filters.Searchtext) && !(filters.Searchtext == "undefined"))
            {
                SearchTextFilter = @"and(CONVERT(nvarchar(11), poware.OrderDate, 101) like @SearchText
                                    or isnull(poware.[PurchaseOrderId], '') like @SearchText 
                                    or poware.[Status] like @SearchText)";
            }
            if(filters.Searchtext == "undefined")
            {
                filters.Searchtext = "";
            }
            if (start != new DateTime() && end != new DateTime())
            {
                string StartDateQuery = start.Value.ToString("yyyy-MM-dd 00:00:00.000");
                string EndDateQuery = end.Value.ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and poware.CreatedDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }
            string sqlQuery = @"DECLARE @pageno int
                                DECLARE @pagesize int
                                DECLARE @SearchText nvarchar(50)
                                DECLARE @pagestart int
                                DECLARE @pageend int

                                SET @SearchText = '%{0}%'
                                SET @pageno = {1}--default 1
                                SET @pagesize = 20--default 10
                                

                                SET @pagestart = (@pageno - 1) * @pagesize
                                SET @pageend = @pagesize

                                select
                                poware.Id 
                                ,poware.PurchaseOrderId as PONumber
		                        ,emp.FirstName +' '+ emp.LastName as CreatedBy
		                        ,poware.CreatedDate
		                        ,poware.[Status]
		                        ,(Select SUM(PurDet.Quantity) from PurchaseOrderDetail PurDet where PurDet.PurchaseOrderId=poware.PurchaseOrderId) as Quantity
		                        ,(Select SUM(PurDet.TotalPrice) from PurchaseOrderDetail PurDet where PurDet.PurchaseOrderId=poware.PurchaseOrderId) as POAmount
		                         into #CreatedPOReport
                                from PurchaseOrderWarehouse poware
                                LEFT JOIN Employee emp on emp.UserId=poware.CreatedByUid
								left join Employee _emp on _emp.UserId = poware.RecieveByUid
                                
                                where poware.[Status] = 'Created'
                                and poware.PurchaseOrderId  LIKE @SearchText
                                {6}
                                

														SELECT TOP (@pagesize) #CPOR.* into #TestTable
														FROM #CreatedPOReport #CPOR
														where PONumber NOT IN(Select TOP (@pagestart) PONumber from #CreatedPOReport #CPOR {7})
                                                        {8}
														select  count(PONumber) as [TotalCount] from #CreatedPOReport

														select * from #TestTable
														select sum(Quantity) as TotalQuantity
														,sum(POAmount) as TotalAmount from #TestTable

														DROP TABLE #CreatedPOReport
														DROP TABLE #TestTable";
            string OrderBy = "";
            string orderquery = "";
            string orderquery1 = "";
            if (!string.IsNullOrEmpty(filters.order) && filters.order != "null")
            {
                if (filters.order == "ascending/pono")
                {
                    orderquery = "order by #CPOR.[PONumber] asc";
                    orderquery1 = "order by [PONumber] asc";
                }
                else if (filters.order == "descending/pono")
                {
                    orderquery = "order by #CPOR.[PONumber] desc";
                    orderquery1 = "order by [PONumber] desc";
                }

                else if (filters.order == "ascending/qty")
                {
                    orderquery = "order by #CPOR.[Quantity] asc";
                    orderquery1 = "order by [Quantity] asc";
                }
                else if (filters.order == "descending/qty")
                {
                    orderquery = "order by #CPOR.[Quantity] desc";
                    orderquery1 = "order by [Quantity] desc";
                }

                else if (filters.order == "ascending/status")
                {
                    orderquery = "order by #CPOR.[Status] asc";
                    orderquery1 = "order by [Status] asc";
                }
                else if (filters.order == "descending/status")
                {
                    orderquery = "order by #CPOR.[Status] desc";
                    orderquery1 = "order by [Status] desc";
                }

                else if (filters.order == "ascending/cdate")
                {
                    orderquery = "order by #CPOR.[CreatedDate] asc";
                    orderquery1 = "order by [CreatedDate] asc";
                }
                else if (filters.order == "descending/cdate")
                {
                    orderquery = "order by #CPOR.[CreatedDate] desc";
                    orderquery1 = "order by [CreatedDate] desc";
                }

                else if (filters.order == "ascending/cby")
                {
                    orderquery = "order by #CPOR.[CreatedBy] asc";
                    orderquery1 = "order by [CreatedBy] asc";
                }
                else if (filters.order == "descending/cby")
                {
                    orderquery = "order by #CPOR.[CreatedBy] desc";
                    orderquery1 = "order by [CreatedBy] desc";
                }
                else if (filters.order == "ascending/amount")
                {
                    orderquery = "order by #CPOR.[POAmount] asc";
                    orderquery1 = "order by [POAmount] asc";
                }
                else if (filters.order == "descending/amount")
                {
                    orderquery = "order by #CPOR.[POAmount] desc";
                    orderquery1 = "order by [POAmount] desc";
                }
            }
            else
            {
                orderquery = "order by #CPOR.[Id] asc";
                orderquery1 = "order by [Id] asc";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        filters.Searchtext,//0
                                        filters.PageNo,  //1
                                        filters.PageSize, //2
                                        filters.CompanyId, //3
                                        SearchTextFilter, //4
                                        OrderBy,//5
                                        DateQuery ,//6
                                        orderquery , //7
                                        orderquery1 //8
                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataSet GetPurchaseOrderNewListByFilters(PurchaseOrderFilter filters, DateTime? start, DateTime? end, string category, string Vendor, string manufeteture ,string serchtext  ,string  SKU)
        {
            string SearchTextFilter = "";
            string DateQuery = "";
            string SelectedStatuses = "";
           
          
            string  selectedcategory= string.Empty;
            if (!string.IsNullOrWhiteSpace(serchtext))
            {
                SearchTextFilter = @"and(CONVERT(nvarchar(11), poware.OrderDate, 101) like @serchtext
                                    or isnull(poware.[PurchaseOrderId], '') like @serchtext 
                                     or Eqp.[Name]like @serchtext )";

            }
            if (start != new DateTime() && end != new DateTime())
            {
                string StartDateQuery = start.Value.ToString("yyyy-MM-dd 00:00:00.000");
                string EndDateQuery = end.Value.ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and poware.OrderDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }

            string poreportnew = "";
            if (!string.IsNullOrWhiteSpace(filters.selectsts) && (filters.selectsts != "Nothing selected") && (filters.selectsts != "Status"))
            {
                SelectedStatuses = string.Join(",", filters.selectsts.Split(',').Select(status => "'" + status.Trim() + "'"));
                poreportnew = " AND poware.[Status] IN (" + SelectedStatuses + ")";
            }

            if (!string.IsNullOrWhiteSpace(category) && (category != "Category(s)") && (category != "null") && (category!= "Select One"))
            {
                string selectedCategories = string.Join(",", category.Split(',').Select(x => "'" + x.Trim() + "'"));
                poreportnew += " AND ISNULL((SELECT eqt.Name FROM EquipmentType eqt WHERE eqt.Id = Eqp.EquipmentTypeId), 'Unknown') IN (" + selectedCategories + ")";
            }

            if (!string.IsNullOrWhiteSpace(Vendor) && (Vendor != "Vendor(s)") && (Vendor != "null")&& (Vendor != "Select One"))
            {
                string selectedVendor = string.Join(",", Vendor.Split(',').Select(x => "'" + x.Trim() + "'"));
                poreportnew += " AND ISNULL((SELECT SInf.CompanyName FROM Supplier SInf WHERE SInf.SupplierId = poware.SuplierId), 'Unknown') IN (" + selectedVendor + ")";
            }
            if (!string.IsNullOrWhiteSpace(manufeteture) && (manufeteture != "Manufacturer(s)") && (manufeteture != "null")&& (manufeteture!= "Select One"))
            {
                string selectedManufeteture = string.Join(",", manufeteture.Split(',').Select(x => "'" + x.Trim() + "'"));
           
                poreportnew += " AND ISNULL((SELECT M.Name FROM Manufacturer M WHERE M.id = Eqp.ManufacturerId), 'Unknown') IN (" + selectedManufeteture + ")";
            }
            if (!string.IsNullOrWhiteSpace(SKU) && (SKU != "SKU") && (SKU != "null") && (SKU != "Select One"))
            {
                string selectedSKU = string.Join(",", SKU.Split(',').Select(x => "'" + x.Trim() + "'"));

                poreportnew += " AND  Eqp.SKU  IN (" + selectedSKU + ")";
            }
          




            string sqlQuery = @"DECLARE @pageno int
                                DECLARE @pagesize int
                                DECLARE @serchtext nvarchar(50)
                                DECLARE @pagestart int
                                DECLARE @pageend int

                                SET @serchtext = '%{0}%'
                                SET @pageno = {1}--default 1
                                SET @pagesize = 20--default 10
                                

                                SET @pagestart = (@pageno - 1) * @pagesize
                                SET @pageend = @pagesize

                               SELECT
    ID,
    PurchaseOrderId,
    CreatedDate,
    Vendor,
    Category,
    Manufacturer,
    SKU,
    Description,
    EquipName,
	[PO Description],

    Quantity,
    UnitPrice,
    TotalPrice,
    BulkStatus,
     [Status]
INTO #CreatedPOReport
FROM (
    SELECT
        poware.ID,
        poware.PurchaseOrderId,
        poware.CreatedDate,
		ISNULL((SELECT eqt.Name FROM EquipmentType eqt WHERE eqt.Id = Eqp.EquipmentTypeId), 'Unknown') AS Category,
        ISNULL((SELECT SInf.CompanyName FROM Supplier SInf WHERE SInf.SupplierId = poware.SuplierId), 'Unknown') AS Vendor,
        ISNULL((SELECT M.Name FROM Manufacturer M WHERE M.id = Eqp.ManufacturerId), 'Unknown') AS Manufacturer,
        Eqp.SKU AS SKU,
        Eqp.Name AS Description,
        POD.EquipName,
		poware.Description as  [PO Description],
        POD.Quantity,
        POD.UnitPrice,
        POD.TotalPrice,
        POD.BulkStatus,
		poware.[Status]
    FROM PurchaseOrderWarehouse poware
    left JOIN [dbo].[PurchaseOrderDetail] POD ON poware.PurchaseOrderId = POD.PurchaseOrderId
    left JOIN [dbo].[Equipment] Eqp ON POD.EquipmentId = Eqp.EquipmentId
	WHERE poware.Status <> 'init'
{4}
{6}
{9}
) AS T;

                             

														SELECT TOP (@pagesize) #CPOR.* into #TestTable
														FROM #CreatedPOReport #CPOR
														where PurchaseOrderId NOT IN(Select TOP (@pagestart) PurchaseOrderId from #CreatedPOReport #CPOR {7})
                                                        {8}
                                                       
														select  count(PurchaseOrderId) as [TotalCount] from #CreatedPOReport

														select * from #TestTable

														select sum(Quantity) as TotalQuantity
														,sum(TotalPrice) as TotalAmount from #TestTable
SELECT DISTINCT
                                                        ID as Value,
                                                        SKU as Text
                                                        FROM #TestTable

														DROP TABLE #CreatedPOReport
														DROP TABLE #TestTable";
            string OrderBy = "";
            string orderquery = "";
            string orderquery1 = "";
            if (!string.IsNullOrEmpty(filters.order) && filters.order != "null")
            {
                if (filters.order == "ascending/PONo")
                {
                    orderquery = "order by #CPOR.[PurchaseOrderId] asc";
                    orderquery1 = "order by [PurchaseOrderId] asc";
                }
                else if (filters.order == "descending/PONo")
                {
                    orderquery = "order by #CPOR.[PurchaseOrderId] desc";
                    orderquery1 = "order by [PurchaseOrderId] desc";
                }
                else if (filters.order == "ascending/Vendor")
                {
                    orderquery = "order by #CPOR.[Vendor] asc";
                    orderquery1 = "order by [Vendor] asc";
                }
                else if (filters.order == "descending/Vendor")
                {
                    orderquery = "order by #CPOR.[Vendor] desc";
                    orderquery1 = "order by [Vendor] desc";
                }
                else if (filters.order == "ascending/OrderDate")
                {
                    orderquery = "order by #CPOR.[OrderDate] asc";
                    orderquery1 = "order by [OrderDate] asc";
                }
                else if (filters.order == "descending/OrderDate")
                {
                    orderquery = "order by #CPOR.[OrderDate] desc";
                    orderquery1 = "order by [OrderDate] desc";
                }
                else if (filters.order == "ascending/Quantity")
                {
                    orderquery = "order by #CPOR.[Quantity] asc";
                    orderquery1 = "order by [Quantity] asc";
                }
                else if (filters.order == "descending/Quantity")
                {
                    orderquery = "order by #CPOR.[Quantity] desc";
                    orderquery1 = "order by [Quantity] desc";
                }

                else if (filters.order == "ascending/Status")
                {
                    orderquery = "order by #CPOR.[Status] asc";
                    orderquery1 = "order by [Status] asc";
                }
                else if (filters.order == "descending/Status")
                {
                    orderquery = "order by #CPOR.[Status] desc";
                    orderquery1 = "order by [Status] desc";
                }

                else if (filters.order == "ascending/TotalPrice")
                {
                    orderquery = "order by #CPOR.[TotalPrice] asc";
                    orderquery1 = "order by [TotalPrice] asc";
                }
                else if (filters.order == "descending/TotalPrice")
                {
                    orderquery = "order by #CPOR.[TotalPrice] desc";
                    orderquery1 = "order by [TotalPrice] desc";
                }

                else if (filters.order == "ascending/UnitPrice")
                {
                    orderquery = "order by #CPOR.[UnitPrice] asc";
                    orderquery1 = "order by [UnitPrice] asc";
                }
                else if (filters.order == "descending/UnitPrice")
                {
                    orderquery = "order by #CPOR.[UnitPrice] desc";
                    orderquery1 = "order by [UnitPrice] desc";
                }

                else if (filters.order == "ascending/Manufacturer")
                {
                    orderquery = "order by #CPOR.[Manufacturer] asc";
                    orderquery1 = "order by [Manufacturer] asc";
                }
                else if (filters.order == "descending/Manufacturer")
                {
                    orderquery = "order by #CPOR.[Manufacturer] desc";
                    orderquery1 = "order by [Manufacturer] desc";
                }
                else if (filters.order == "ascending/Description")
                {
                    orderquery = "order by #CPOR.[Description] asc";
                    orderquery1 = "order by [Description] asc";
                }
                else if (filters.order == "descending/Description")
                {
                    orderquery = "order by #CPOR.[Description] desc";
                    orderquery1 = "order by [Description] desc";
                }
                else if (filters.order == "ascending/SKU")
                {
                    orderquery = "order by #CPOR.[SKU] asc";
                    orderquery1 = "order by [SKU] asc";
                }
                else if (filters.order == "descending/SKU")
                {
                    orderquery = "order by #CPOR.[SKU] desc";
                    orderquery1 = "order by [SKU] desc";
                }


            }
            else
            {
                orderquery = "order by #CPOR.[Id] asc";
                orderquery1 = "order by [Id] asc";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        serchtext,//0
                                        filters.PageNo,  //1
                                        filters.PageSize, //2
                                        filters.CompanyId, //3
                                        SearchTextFilter, //4
                                        OrderBy,//5
                                        DateQuery,//6
                                        orderquery, //7
                                        orderquery1, //8
                                        poreportnew


                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return null;
            }
        }

        public DataSet GetPurchaseOrderNewListPOReportInventory(PurchaseOrderFilter filters, DateTime? start, DateTime? end, string category, string Vendor, string manufeteture, string serchtext, string SKU)
        {
            string SearchTextFilter = "";
            string DateQuery = "";
            string SelectedStatuses = "";


            string selectedcategory = string.Empty;
            if (!string.IsNullOrWhiteSpace(serchtext))
            {
                SearchTextFilter = @"and(CONVERT(nvarchar(11), poware.OrderDate, 101) like @serchtext
                                    or isnull(poware.[PurchaseOrderId], '') like @serchtext 
                                     or Eqp.Name like @serchtext  or poware.[Description] like @serchtext or empPoFor.FirstName + ' ' + empPoFor.LastName like @serchtext)";

            }
            if (start != new DateTime() && end != new DateTime())
            {
                string StartDateQuery = start.Value.ToString("yyyy-MM-dd 00:00:00.000");
                string EndDateQuery = end.Value.ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and poware.OrderDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }

            string poreportnew = "";
            if (!string.IsNullOrWhiteSpace(filters.selectsts) && (filters.selectsts != "Nothing selected") && (filters.selectsts != "Status"))
            {
                SelectedStatuses = string.Join(",", filters.selectsts.Split(',').Select(status => "'" + status.Trim() + "'"));
                poreportnew = " AND poware.[Status] IN (" + SelectedStatuses + ")";
            }

            if (!string.IsNullOrWhiteSpace(category) && (category != "Category(s)") && (category != "null") && (category != "Select One"))
            {
                string selectedCategories = string.Join(",", category.Split(',').Select(x => "'" + x.Trim() + "'"));
                poreportnew += " AND ISNULL((SELECT eqt.Name FROM EquipmentType eqt WHERE eqt.Id = Eqp.EquipmentTypeId), 'Unknown') IN (" + selectedCategories + ")";
            }

            if (!string.IsNullOrWhiteSpace(Vendor) && (Vendor != "Vendor(s)") && (Vendor != "null") && (Vendor != "Select One"))
            {
                string selectedVendor = string.Join(",", Vendor.Split(',').Select(x => "'" + x.Trim() + "'"));
                poreportnew += " AND ISNULL((SELECT SInf.CompanyName FROM Supplier SInf WHERE SInf.SupplierId = poware.SuplierId), 'Unknown') IN (" + selectedVendor + ")";
            }
            if (!string.IsNullOrWhiteSpace(manufeteture) && (manufeteture != "Manufacturer(s)") && (manufeteture != "null") && (manufeteture != "Select One"))
            {
                string selectedManufeteture = string.Join(",", manufeteture.Split(',').Select(x => "'" + x.Trim() + "'"));

                poreportnew += " AND ISNULL((SELECT M.Name FROM Manufacturer M WHERE M.id = Eqp.ManufacturerId), 'Unknown') IN (" + selectedManufeteture + ")";
            }
            if (!string.IsNullOrWhiteSpace(SKU) && (SKU != "SKU") && (SKU != "null") && (SKU != "Select One"))
            {
                string selectedSKU = string.Join(",", SKU.Split(',').Select(x => "'" + x.Trim() + "'"));

                poreportnew += " AND  Eqp.SKU  IN (" + selectedSKU + ")";
            }





            string sqlQuery = @"DECLARE @pageno int
                                DECLARE @pagesize int
                                DECLARE @serchtext nvarchar(50)
                                DECLARE @pagestart int
                                DECLARE @pageend int

                                SET @serchtext = '%{0}%'
                                SET @pageno = {1}--default 1
                                SET @pagesize = 20--default 10
                                

                                SET @pagestart = (@pageno - 1) * @pagesize
                                SET @pageend = @pagesize

                               SELECT
    ID,
    PurchaseOrderId,
    OrderDate,
    CreatedDate,
    RecieveQty,
    Vendor,
    Category,
    Manufacturer,
    SKU,
    TechnicianName,
    Description,
    EquipName,
	[PO Description],

    Quantity,
    UnitPrice,
    TotalPrice,
    BulkStatus,
     [Status]
INTO #CreatedPOReport
FROM (
    SELECT
        poware.ID,
        poware.PurchaseOrderId,
        poware.CreatedDate as [OrderDate],
		ISNULL((SELECT eqt.Name FROM EquipmentType eqt WHERE eqt.Id = Eqp.EquipmentTypeId), 'Unknown') AS Category,
        ISNULL((SELECT SInf.CompanyName FROM Supplier SInf WHERE SInf.SupplierId = poware.SuplierId), 'Unknown') AS Vendor,
        ISNULL((SELECT M.Name FROM Manufacturer M WHERE M.id = Eqp.ManufacturerId), 'Unknown') AS Manufacturer,
        Eqp.SKU AS SKU,
         empPoFor.FirstName + ' ' + empPoFor.LastName as TechnicianName,
        Eqp.Name AS Description,
        POD.EquipName,
		poware.Description as  [PO Description],
        POD.Quantity,
        POD.UnitPrice,
        POD.TotalPrice,
        POD.CreatedDate,
        POD.RecieveQty,
        POD.BulkStatus,
		poware.[Status]
    FROM PurchaseOrderWarehouse poware
    left JOIN [dbo].[PurchaseOrderDetail] POD ON poware.PurchaseOrderId = POD.PurchaseOrderId
    left JOIN [dbo].[Equipment] Eqp ON POD.EquipmentId = Eqp.EquipmentId
    left JOIN Employee empPoFor on empPoFor.UserId = poware.POFor 
	WHERE poware.Status <> 'init'
{4}
{6}
{9}
) AS T;

                             

														SELECT TOP (@pagesize) #CPOR.* into #TestTable
														FROM #CreatedPOReport #CPOR
														where PurchaseOrderId NOT IN(Select TOP (@pagestart) PurchaseOrderId from #CreatedPOReport #CPOR {7})
                                                        {8}
                                                       
														select  count(PurchaseOrderId) as [TotalCount] from #CreatedPOReport

														select * from #TestTable

														select sum(Quantity) as TotalQuantity
														,sum(TotalPrice) as TotalAmount from #TestTable
SELECT DISTINCT
                                                        ID as Value,
                                                        SKU as Text
                                                        FROM #TestTable

														DROP TABLE #CreatedPOReport
														DROP TABLE #TestTable";
            string OrderBy = "";
            string orderquery = "";
            string orderquery1 = "";
            if (!string.IsNullOrEmpty(filters.order) && filters.order != "null")
            {
                if (filters.order == "ascending/PONo")
                {
                    orderquery = "order by #CPOR.[PurchaseOrderId] asc";
                    orderquery1 = "order by [PurchaseOrderId] asc";
                }
                else if (filters.order == "descending/PONo")
                {
                    orderquery = "order by #CPOR.[PurchaseOrderId] desc";
                    orderquery1 = "order by [PurchaseOrderId] desc";
                }
                else if (filters.order == "ascending/Vendor")
                {
                    orderquery = "order by #CPOR.[Vendor] asc";
                    orderquery1 = "order by [Vendor] asc";
                }
                else if (filters.order == "descending/Vendor")
                {
                    orderquery = "order by #CPOR.[Vendor] desc";
                    orderquery1 = "order by [Vendor] desc";
                }
                else if (filters.order == "ascending/OrderDate")
                {
                    orderquery = "order by #CPOR.[OrderDate] asc";
                    orderquery1 = "order by [OrderDate] asc";
                }
                else if (filters.order == "descending/OrderDate")
                {
                    orderquery = "order by #CPOR.[OrderDate] desc";
                    orderquery1 = "order by [OrderDate] desc";
                }
                else if (filters.order == "ascending/Quantity")
                {
                    orderquery = "order by #CPOR.[Quantity] asc";
                    orderquery1 = "order by [Quantity] asc";
                }
                else if (filters.order == "descending/Quantity")
                {
                    orderquery = "order by #CPOR.[Quantity] desc";
                    orderquery1 = "order by [Quantity] desc";
                }

                else if (filters.order == "ascending/Status")
                {
                    orderquery = "order by #CPOR.[Status] asc";
                    orderquery1 = "order by [Status] asc";
                }
                else if (filters.order == "descending/Status")
                {
                    orderquery = "order by #CPOR.[Status] desc";
                    orderquery1 = "order by [Status] desc";
                }

                else if (filters.order == "ascending/TotalPrice")
                {
                    orderquery = "order by #CPOR.[TotalPrice] asc";
                    orderquery1 = "order by [TotalPrice] asc";
                }
                else if (filters.order == "descending/TotalPrice")
                {
                    orderquery = "order by #CPOR.[TotalPrice] desc";
                    orderquery1 = "order by [TotalPrice] desc";
                }

                else if (filters.order == "ascending/UnitPrice")
                {
                    orderquery = "order by #CPOR.[UnitPrice] asc";
                    orderquery1 = "order by [UnitPrice] asc";
                }
                else if (filters.order == "descending/UnitPrice")
                {
                    orderquery = "order by #CPOR.[UnitPrice] desc";
                    orderquery1 = "order by [UnitPrice] desc";
                }

                else if (filters.order == "ascending/Manufacturer")
                {
                    orderquery = "order by #CPOR.[Manufacturer] asc";
                    orderquery1 = "order by [Manufacturer] asc";
                }
                else if (filters.order == "descending/Manufacturer")
                {
                    orderquery = "order by #CPOR.[Manufacturer] desc";
                    orderquery1 = "order by [Manufacturer] desc";
                }
                else if (filters.order == "ascending/EqDescription")
                {
                    orderquery = "order by #CPOR.[Description] asc";
                    orderquery1 = "order by [Description] asc";
                }
                else if (filters.order == "descending/EqDescription")
                {
                    orderquery = "order by #CPOR.[Description] desc";
                    orderquery1 = "order by [Description] desc";
                }
                else if (filters.order == "ascending/SKU")
                {
                    orderquery = "order by #CPOR.[SKU] asc";
                    orderquery1 = "order by [SKU] asc";
                }
                else if (filters.order == "descending/SKU")
                {
                    orderquery = "order by #CPOR.[SKU] desc";
                    orderquery1 = "order by [SKU] desc";
                }
                else if (filters.order == "ascending/technicianName")
                {
                    OrderBy = "order by TechnicianName asc";
                }
                else if (filters.order == "descending/technicianName")
                {
                    OrderBy = "order by TechnicianName desc";
                }
                else if (filters.order == "ascending/Description")
                {
                    orderquery = "order by #CPOR.[PO Description] asc";
                    orderquery1 = "order by [PO Description] asc";
                }
                else if (filters.order == "descending/Description")
                {
                    orderquery = "order by #CPOR.[PO Description] desc";
                    orderquery1 = "order by [PO Description] desc";
                }




            }
            else
            {
                orderquery = "order by #CPOR.[Id] asc";
                orderquery1 = "order by [Id] asc";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        serchtext,//0
                                        filters.PageNo,  //1
                                        filters.PageSize, //2
                                        filters.CompanyId, //3
                                        SearchTextFilter, //4
                                        OrderBy,//5
                                        DateQuery,//6
                                        orderquery, //7
                                        orderquery1, //8
                                        poreportnew


                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return null;
            }
        }

        public DataSet GetReceivedPurchaseOrderListByFilters(PurchaseOrderFilter filters, DateTime? start, DateTime? end)
        {
            string SearchTextFilter = "";
            string DateQuery = "";
            if (!string.IsNullOrWhiteSpace(filters.Searchtext))
            {
                SearchTextFilter = @"and(CONVERT(nvarchar(11), poware.OrderDate, 101) like @SearchText
                                    or isnull(poware.[PurchaseOrderId], '') like @SearchText 
                                    or poware.[Status] like @SearchText)";
            }
            if (start != new DateTime() && end != new DateTime())
            {
                string StartDateQuery = start.Value.ToString("yyyy-MM-dd 00:00:00.000");
                string EndDateQuery = end.Value.ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and poware.CreatedDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }
            string sqlQuery = @"DECLARE @pageno int
                                DECLARE @pagesize int
                                DECLARE @SearchText nvarchar(50)
                                DECLARE @pagestart int
                                DECLARE @pageend int

                                SET @SearchText = '%{0}%'
                                SET @pageno = {1}--default 1
                                SET @pagesize = 20--default 10
                                

                                SET @pagestart = (@pageno - 1) * @pagesize
                                SET @pageend = @pagesize
                                
                                select	
                                poware.Id 
                                ,poware.PurchaseOrderId as PONumber
		                        ,emp.FirstName +' '+ emp.LastName as CreatedBy
		                        ,poware.CreatedDate
		                        ,poware.[Status]
		                        ,(Select SUM(PurDet.Quantity) from PurchaseOrderDetail PurDet where PurDet.PurchaseOrderId=poware.PurchaseOrderId) as Quantity
		                        ,(Select SUM(PurDet.TotalPrice) from PurchaseOrderDetail PurDet where PurDet.PurchaseOrderId=poware.PurchaseOrderId) as POAmount
		                        ,_emp.FirstName +' '+ _emp.LastName as ReceivedBy
		                        ,poware.RecieveDate into #ReceivedPOReport
                                                        from PurchaseOrderWarehouse poware
                                                        LEFT JOIN Employee emp on emp.UserId=poware.CreatedByUid
								                        left join Employee _emp on _emp.UserId = poware.RecieveByUid
                                
                                                        where poware.[Status] = 'Received'
                                                        and poware.PurchaseOrderId  LIKE @SearchText
                                                        {6}

														SELECT TOP (@pagesize) #RPOR.* into #TestTable
														FROM #ReceivedPOReport #RPOR
														where PONumber NOT IN(Select TOP (@pagestart) PONumber from #ReceivedPOReport #RPOR {7})
                                                        {8}

														select  count(PONumber) as [TotalCount] from #ReceivedPOReport

														select * from #TestTable
														select sum(Quantity) as TotalQuantity
														,sum(POAmount) as TotalAmount from #TestTable

														DROP TABLE #ReceivedPOReport
														DROP TABLE #TestTable";
            string OrderBy = "";
            string orderquery = "";
            string orderquery1 = "";
            if (!string.IsNullOrEmpty(filters.order) && filters.order != "null")
            {
                if (filters.order == "ascending/pono")
                {
                    orderquery = "order by #RPOR.[PONumber] asc";
                    orderquery1 = "order by [PONumber] asc";
                }
                else if (filters.order == "descending/pono")
                {
                    orderquery = "order by #RPOR.[PONumber] desc";
                    orderquery1 = "order by [PONumber] desc";
                }

                else if (filters.order == "ascending/qty")
                {
                    orderquery = "order by #RPOR.[Quantity] asc";
                    orderquery1 = "order by [Quantity] asc";
                }
                else if (filters.order == "descending/qty")
                {
                    orderquery = "order by #RPOR.[Quantity] desc";
                    orderquery1 = "order by [Quantity] desc";
                }

                else if (filters.order == "ascending/status")
                {
                    orderquery = "order by #RPOR.[Status] asc";
                    orderquery1 = "order by [Status] asc";
                }
                else if (filters.order == "descending/status")
                {
                    orderquery = "order by #RPOR.[Status] desc";
                    orderquery1 = "order by [Status] desc";
                }

                else if (filters.order == "ascending/cdate")
                {
                    orderquery = "order by #RPOR.[CreatedDate] asc";
                    orderquery1 = "order by [CreatedDate] asc";
                }
                else if (filters.order == "descending/cdate")
                {
                    orderquery = "order by #RPOR.[CreatedDate] desc";
                    orderquery1 = "order by [CreatedDate] desc";
                }

                else if (filters.order == "ascending/cby")
                {
                    orderquery = "order by #RPOR.[CreatedBy] asc";
                    orderquery1 = "order by [CreatedBy] asc";
                }
                else if (filters.order == "descending/cby")
                {
                    orderquery = "order by #RPOR.[CreatedBy] desc";
                    orderquery1 = "order by [CreatedBy] desc";
                }
                else if (filters.order == "ascending/amount")
                {
                    orderquery = "order by #RPOR.[POAmount] asc";
                    orderquery1 = "order by [POAmount] asc";
                }
                else if (filters.order == "descending/amount")
                {
                    orderquery = "order by #RPOR.[POAmount] desc";
                    orderquery1 = "order by [POAmount] desc";
                }
                else if (filters.order == "ascending/rdate")
                {
                    orderquery = "order by #RPOR.[RecieveDate] asc";
                    orderquery1 = "order by [RecieveDate] asc";
                }
                else if (filters.order == "descending/rdate")
                {
                    orderquery = "order by #RPOR.[CreatedDate] desc";
                    orderquery1 = "order by [CreatedDate] desc";
                }

                else if (filters.order == "ascending/rby")
                {
                    orderquery = "order by #RPOR.[ReceivedBy] asc";
                    orderquery1 = "order by [ReceivedBy] asc";
                }
                else if (filters.order == "descending/rby")
                {
                    orderquery = "order by #RPOR.[ReceivedBy] desc";
                    orderquery1 = "order by [ReceivedBy] desc";
                }
            }
            else
            {
                orderquery = "order by #RPOR.[Id] asc";
                orderquery1 = "order by [Id] asc";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        filters.Searchtext,//0
                                        filters.PageNo,  //1
                                        filters.PageSize, //2
                                        filters.CompanyId, //3
                                        SearchTextFilter, //4
                                        OrderBy,//5
                                        DateQuery, //6
                                        orderquery, //7
                                        orderquery1 //8
                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetCreatedPurchaseOrderListByFiltersForReport(PurchaseOrderFilter filters, DateTime? start, DateTime? end)
        {
            string SearchTextFilter = "";
            string DateQuery = "";
            if (!string.IsNullOrWhiteSpace(filters.Searchtext))
            {
                SearchTextFilter = @"and(CONVERT(nvarchar(11), poware.OrderDate, 101) like @SearchText
                                    or isnull(poware.[PurchaseOrderId], '') like @SearchText 
                                    or poware.[Status] like @SearchText)";
            }
            if (start != new DateTime() && end != new DateTime())
            {
                string StartDateQuery = start.Value.ToString("yyyy-MM-dd 00:00:00.000");
                string EndDateQuery = end.Value.ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and poware.CreatedDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }
            string sqlQuery = @"DECLARE @SearchText nvarchar(50)

                                SET @SearchText = '%{0}%'

                                select	 poware.PurchaseOrderId as PONumber
		                        ,emp.FirstName +' '+ emp.LastName as CreatedBy
		                        ,FORMAT(poware.CreatedDate,'MM/dd/yyyy hh:mm tt') AS CreatedOn
		                        ,poware.[Status]
		                        ,(Select SUM(PurDet.Quantity) from PurchaseOrderDetail PurDet where PurDet.PurchaseOrderId=poware.PurchaseOrderId) as Quantity
		                        ,(Select SUM(PurDet.TotalPrice) from PurchaseOrderDetail PurDet where PurDet.PurchaseOrderId=poware.PurchaseOrderId) as POAmount
                                from PurchaseOrderWarehouse poware
                                LEFT JOIN Employee emp on emp.UserId=poware.CreatedByUid
								left join Employee _emp on _emp.UserId = poware.RecieveByUid
                                
                                where poware.[Status] = 'Created'
                                and poware.PurchaseOrderId  LIKE @SearchText
                                {2}
                                order by poware.PurchaseOrderId desc";


            try
            {
                sqlQuery = string.Format(sqlQuery, filters.Searchtext, SearchTextFilter, DateQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable GetPurchaseOrderNewListByFiltersdownload(PurchaseOrderFilter filters, DateTime? start, DateTime? end, string serchtext, string SKU, string Category, string Manufeturelist, string supplier)
        {
            string SearchTextFilter = "";
            string DateQuery = "";
            string SelectedStatuses = "";


            string selectedcategory = string.Empty;
            if (!string.IsNullOrWhiteSpace(serchtext))
            {
                string Serchtext = serchtext;
                SearchTextFilter = string.Format(@" and(CONVERT(nvarchar(11), poware.OrderDate, 101) like '%{0}%'
                                       or isnull(poware.[PurchaseOrderId], '') like '%{0}%'
                                       or Eqp.[Name] like '%{0}%')", Serchtext);
            }

            if (start != new DateTime() && end != new DateTime())
            {
                string StartDateQuery = start.Value.ToString("yyyy-MM-dd 00:00:00.000");
                string EndDateQuery = end.Value.ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and poware.OrderDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }

            string poreportnew = "";
            if (!string.IsNullOrWhiteSpace(filters.selectsts) && (filters.selectsts != "Nothing selected") && (filters.selectsts != "Status"))
            {
                SelectedStatuses = string.Join(",", filters.selectsts.Split(',').Select(status => "'" + status.Trim() + "'"));
                poreportnew = " AND poware.[Status] IN (" + SelectedStatuses + ")";
            }

            if (!string.IsNullOrWhiteSpace(Category) && (Category != "Category(s)") && (Category != "null") && (Category != "Select One"))
            {
                string selectedCategories = string.Join(",", Category.Split(',').Select(x => "'" + x.Trim() + "'"));
                poreportnew += " AND ISNULL((SELECT eqt.Name FROM EquipmentType eqt WHERE eqt.Id = Eqp.EquipmentTypeId), 'Unknown') IN (" + selectedCategories + ")";
            }

            if (!string.IsNullOrWhiteSpace(supplier) && (supplier != "Vendor(s)") && (supplier != "null") && (supplier != "Select One"))
            {
                string selectedVendor = string.Join(",", supplier.Split(',').Select(x => "'" + x.Trim() + "'"));
                poreportnew += " AND ISNULL((SELECT SInf.CompanyName FROM Supplier SInf WHERE SInf.SupplierId = poware.SuplierId), 'Unknown') IN (" + selectedVendor + ")";
            }
            if (!string.IsNullOrWhiteSpace(Manufeturelist) && (Manufeturelist != "Manufacturer(s)") && (Manufeturelist != "null") && (Manufeturelist != "Select One"))
            {
                string selectedManufeteture = string.Join(",", Manufeturelist.Split(',').Select(x => "'" + x.Trim() + "'"));

                poreportnew += " AND ISNULL((SELECT M.Name FROM Manufacturer M WHERE M.id = Eqp.ManufacturerId), 'Unknown') IN (" + selectedManufeteture + ")";
            }
            if (!string.IsNullOrWhiteSpace(SKU) && (SKU != "SKU") && (SKU != "null") && (SKU != "Select One"))
            {
                string selectedSKU = string.Join(",", SKU.Split(',').Select(x => "'" + x.Trim() + "'"));

            poreportnew += " AND  Eqp.SKU  IN (" + selectedSKU + ")";
        }





        string sqlQuery = @"
    SELECT
  
        poware.PurchaseOrderId,
        poware.CreatedDate as OrderDate,
        ISNULL((SELECT SInf.CompanyName FROM Supplier SInf WHERE SInf.SupplierId = poware.SuplierId), 'Unknown') AS Vendor,
		ISNULL((SELECT eqt.Name FROM EquipmentType eqt WHERE eqt.Id = Eqp.EquipmentTypeId), 'Unknown') AS Category,
        ISNULL((SELECT M.Name FROM Manufacturer M WHERE M.id = Eqp.ManufacturerId), 'Unknown') AS Manufacturer,
        Eqp.SKU AS SKU,
        Eqp.Name AS Description,
        POD.EquipName,
        poware.Description AS [PO Description],
        POD.Quantity,
        POD.UnitPrice,
        POD.TotalPrice,
        POD.BulkStatus,
		poware.[Status]
    FROM PurchaseOrderWarehouse poware
    left JOIN [dbo].[PurchaseOrderDetail] POD ON poware.PurchaseOrderId = POD.PurchaseOrderId
    left JOIN [dbo].[Equipment] Eqp ON POD.EquipmentId = Eqp.EquipmentId
	WHERE poware.Status <> 'init'
{0}
{1}
{2}
 ORDER BY poware.ID ASC";
            try
            {
                sqlQuery = string.Format(sqlQuery, SearchTextFilter, DateQuery, poreportnew);           
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return null;
            }


        }

        public DataTable GetPurchaseOrderNewPOReportInventoryDownload(PurchaseOrderFilter filters, DateTime? start, DateTime? end, string serchtext, string SKU, string Category, string Manufeturelist, string supplier)
        {
            string SearchTextFilter = "";
            string DateQuery = "";
            string SelectedStatuses = "";


            string selectedcategory = string.Empty;
            if (!string.IsNullOrWhiteSpace(serchtext))
            {
                string Serchtext = serchtext;
                SearchTextFilter = string.Format(@" and(CONVERT(nvarchar(11), poware.OrderDate, 101) like '%{0}%'
                                       or isnull(poware.[PurchaseOrderId], '') like '%{0}%'
                                       or Eqp.[Name] like '%{0}%'  or poware.[Description] like '%{0}%'  or empPoFor.FirstName + ' ' + empPoFor.LastName like '%{0}%')", Serchtext);
            }

            
            if (start != new DateTime() && end != new DateTime())
            {
                string StartDateQuery = start.Value.ToString("yyyy-MM-dd 00:00:00.000");
                string EndDateQuery = end.Value.ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and poware.OrderDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }

            string poreportnew = "";
            if (!string.IsNullOrWhiteSpace(filters.selectsts) && (filters.selectsts != "Nothing selected") && (filters.selectsts != "Status"))
            {
                SelectedStatuses = string.Join(",", filters.selectsts.Split(',').Select(status => "'" + status.Trim() + "'"));
                poreportnew = " AND poware.[Status] IN (" + SelectedStatuses + ")";
            }

            if (!string.IsNullOrWhiteSpace(Category) && (Category != "Category(s)") && (Category != "null") && (Category != "Select One"))
            {
                string selectedCategories = string.Join(",", Category.Split(',').Select(x => "'" + x.Trim() + "'"));
                poreportnew += " AND ISNULL((SELECT eqt.Name FROM EquipmentType eqt WHERE eqt.Id = Eqp.EquipmentTypeId), 'Unknown') IN (" + selectedCategories + ")";
            }

            if (!string.IsNullOrWhiteSpace(supplier) && (supplier != "Vendor(s)") && (supplier != "null") && (supplier != "Select One"))
            {
                string selectedVendor = string.Join(",", supplier.Split(',').Select(x => "'" + x.Trim() + "'"));
                poreportnew += " AND ISNULL((SELECT SInf.CompanyName FROM Supplier SInf WHERE SInf.SupplierId = poware.SuplierId), 'Unknown') IN (" + selectedVendor + ")";
            }
            if (!string.IsNullOrWhiteSpace(Manufeturelist) && (Manufeturelist != "Manufacturer(s)") && (Manufeturelist != "null") && (Manufeturelist != "Select One"))
            {
                string selectedManufeteture = string.Join(",", Manufeturelist.Split(',').Select(x => "'" + x.Trim() + "'"));

                poreportnew += " AND ISNULL((SELECT M.Name FROM Manufacturer M WHERE M.id = Eqp.ManufacturerId), 'Unknown') IN (" + selectedManufeteture + ")";
            }
            if (!string.IsNullOrWhiteSpace(SKU) && (SKU != "SKU") && (SKU != "null") && (SKU != "Select One"))
            {
                string selectedSKU = string.Join(",", SKU.Split(',').Select(x => "'" + x.Trim() + "'"));

                poreportnew += " AND  Eqp.SKU  IN (" + selectedSKU + ")";
            }





            string sqlQuery = @"SELECT
    
    PurchaseOrderId,
     OrderDate,
    [SKU Received Date],
    Vendor,
    Category,
    Manufacturer,
 [Equipment Description],
    [Description],
[Received For],
     SKU,
    Quantity,
    UnitPrice,
    TotalPrice,
    BulkStatus,
     [Status]

FROM (
    SELECT
        poware.ID,
        poware.PurchaseOrderId,
        poware.CreatedDate as [OrderDate],
		ISNULL((SELECT eqt.Name FROM EquipmentType eqt WHERE eqt.Id = Eqp.EquipmentTypeId), 'Unknown') AS Category,
        ISNULL((SELECT SInf.CompanyName FROM Supplier SInf WHERE SInf.SupplierId = poware.SuplierId), 'Unknown') AS Vendor,
        ISNULL((SELECT M.Name FROM Manufacturer M WHERE M.id = Eqp.ManufacturerId), 'Unknown') AS Manufacturer,
        Eqp.SKU AS SKU,
        empPoFor.FirstName + ' ' + empPoFor.LastName as [Received For],
        Eqp.Name AS [Equipment Description],
        poware.Description as [Description],
        POD.Quantity,
          CASE 
            WHEN POD.RecieveQty = 0 THEN NULL 
            ELSE POD.CreatedDate 
        END AS [SKU Received Date],
        POD.UnitPrice,
        POD.TotalPrice,
        POD.BulkStatus,
		poware.[Status]
    FROM PurchaseOrderWarehouse poware
    left JOIN [dbo].[PurchaseOrderDetail] POD ON poware.PurchaseOrderId = POD.PurchaseOrderId
    left JOIN [dbo].[Equipment] Eqp ON POD.EquipmentId = Eqp.EquipmentId
    left JOIN [dbo].[Employee] empPoFor on empPoFor.UserId = poware.POFor
	WHERE poware.Status <> 'init'
{0}
{1}
{2}
) AS T ORDER BY T.ID ASC";
            try
            {
                sqlQuery = string.Format(sqlQuery, SearchTextFilter, DateQuery, poreportnew);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return null;
            }


        }

        public DataTable GetReceivedPurchaseOrderListByFiltersForReport(PurchaseOrderFilter filters, DateTime? start, DateTime? end)
        {
            string SearchTextFilter = "";
            string DateQuery = "";
            if (!string.IsNullOrWhiteSpace(filters.Searchtext))
            {
                SearchTextFilter = @"and(CONVERT(nvarchar(11), poware.OrderDate, 101) like @SearchText
                                    or isnull(poware.[PurchaseOrderId], '') like @SearchText 
                                    or poware.[Status] like @SearchText)";
            }
            if (start != new DateTime() && end != new DateTime())
            {
                string StartDateQuery = start.Value.ToString("yyyy-MM-dd 00:00:00.000");
                string EndDateQuery = end.Value.ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and poware.CreatedDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }
            string sqlQuery = @"DECLARE @SearchText nvarchar(50)

                                SET @SearchText = '%{0}%'

                                select	 poware.PurchaseOrderId as PONumber
		                        ,emp.FirstName +' '+ emp.LastName as CreatedBy
		                        ,FORMAT(poware.CreatedDate,'MM/dd/yyyy hh:mm tt') AS CreatedOn
		                        ,poware.[Status]
		                        ,(Select SUM(PurDet.Quantity) from PurchaseOrderDetail PurDet where PurDet.PurchaseOrderId=poware.PurchaseOrderId) as Quantity
		                        ,(Select SUM(PurDet.TotalPrice) from PurchaseOrderDetail PurDet where PurDet.PurchaseOrderId=poware.PurchaseOrderId) as POAmount
		                        ,_emp.FirstName +' '+ _emp.LastName as ReceivedBy
		                        ,FORMAT(poware.RecieveDate,'MM/dd/yyyy') AS ReceivedDate
                                from PurchaseOrderWarehouse poware
                                LEFT JOIN Employee emp on emp.UserId=poware.CreatedByUid
								left join Employee _emp on _emp.UserId = poware.RecieveByUid
                                
                                where poware.[Status] = 'Received'
                                and poware.PurchaseOrderId  LIKE @SearchText
                                {2}
                                order by poware.PurchaseOrderId desc";


            try
            {
                sqlQuery = string.Format(sqlQuery, filters.Searchtext, SearchTextFilter, DateQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        public DataTable GetEstimatorIdListOfPurchaseOrder() //List<string>
        {
            string sqlQuery = @"select distinct EstimatorId
                                from PurchaseOrderWarehouse
                                where EstimatorId != '' AND EstimatorId is not null 
                                order by EstimatorId asc";
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                    //return (from DataRow dr in dsResult.Tables[0].Rows select dr["IdList"].ToString()).ToList();

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetSKU()//List<string>
        {
            string sqlQuery = @"select  DISTINCT
                                                        ID as [Value],
                                                        [Name] as[ Text ]
														FROM EquipmentType  order by Value asc";
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                    //return (from DataRow dr in dsResult.Tables[0].Rows select dr["IdList"].ToString()).ToList();

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
