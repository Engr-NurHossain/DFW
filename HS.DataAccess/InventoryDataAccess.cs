using System;
using System.Data;
using System.Data.SqlClient;
using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Web;
using System.Text.RegularExpressions;
using System.Linq;

namespace HS.DataAccess
{
    public partial class InventoryDataAccess
    {
        private ErrorLogDataAccess _ErrorLogDataAccess;
        public InventoryDataAccess(string ConnectionString) : base(ConnectionString)
        {
            _ErrorLogDataAccess = new ErrorLogDataAccess(ConnectionString);
        }
        public InventoryDataAccess()
        {
            _ErrorLogDataAccess = new ErrorLogDataAccess();
        }
        public DataTable GetEqupmentServiceByEquipmentIdAndComapnyId(Guid EquipmentId, Guid CompanyId)
        {

            string sqlQuery = @"select *
                                from inventory _inv
                                where _inv.CompanyId = '{0}' and _inv.EquipmentId='{1}'";

            try
            {
                sqlQuery = string.Format(sqlQuery, EquipmentId, CompanyId);
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

        public bool DeleteInventoryByEquipmentId(Guid equipmentId)
        {
            string sqlQuery = @"delete from Inventory where EquipmentId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, equipmentId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public DataTable GetLowStockQuantityStatus(Guid CompanyId)
        {
            string sqlQuery = @"select 
                                _eqp.*,
                                _eqpClass.Name as EquipmentClass,
                                _inv.Quantity as Quantity
                                from Equipment _eqp
                                left join Inventory _inv
                                on _eqp.CompanyId = _inv.CompanyId and _eqp.EquipmentId = _inv.EquipmentId
                                left join EquipmentClass _eqpClass
                                on _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                                where _eqp.IsActive = 1 and _inv.Quantity between 1 and _eqp.reorderpoint and _eqp.CompanyId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, CompanyId);
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
        public DataTable GetOutOfStockQuantityStatus(Guid CompanyId)
        {
            string sqlQuery = @"
                            select 
                            _eqp.*,
                            _eqpClass.Name as EquipmentClass,
                            _inv.Quantity as Quantity
                            from Equipment _eqp
                            left join Inventory _inv
                            on _eqp.CompanyId = _inv.CompanyId and _eqp.EquipmentId = _inv.EquipmentId
                            left join EquipmentClass _eqpClass
                            on _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                            where _eqp.IsActive = 1 and _inv.Quantity <= 0  and _eqp.CompanyId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, CompanyId);
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
        public DataTable GetCustomInventoryEquipmentByEquipmentIdAndCompanyId(Guid EquipmentId, Guid CompanyId)
        {

            string sqlQuery = @"select
                                _eqp.Id as EquipmentIntId,
                                _inv.Id as InventoryIntId, 
                                _eqp.EquipmentId as EquipmentId,
                                _eqp.CompanyId as CompanyId,
                                _eqp.Cost as EquipmentCost,
                                _inv.Cost as InventoryCost,
                                _eqp.AsOfDate as EquipmentAsOfDate,
                                _inv.Quantity as InventoryQuantity

                                from Equipment _eqp 
                                join Inventory _inv
                                on _eqp.CompanyId = _inv.CompanyId and _eqp.EquipmentId = _inv.EquipmentId
                                where _eqp.EquipmentId = '{0}' and _eqp.CompanyId = '{1}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, EquipmentId, CompanyId);
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

        public DataTable GetCustomInventoryWarehouseEquipmentByEquipmentIdAndCompanyId(Guid EquipmentId, Guid CompanyId)
        {

            string sqlQuery = @"select
                                _eqp.EquipmentId as EquipmentId,
                                --((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add')
                                -- - (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=_eqp.EquipmentId and Type='Release')
                                -- - isnull((select sum(Quantity) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = _eqp.EquipmentId and 
                                -- b.TechnicianId  = '22222222-2222-2222-2222-222222222222' and b.IsApprove = 0 and b.IsDecline = 0 ),0)) as InventoryQuantity,
                 ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add' And invinner.LocationId = '22222222-2222-2222-2222-222222222222')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=_eqp.EquipmentId and Type='Release' And invinner2.LocationId = '22222222-2222-2222-2222-222222222222')
                                            	-(select ISNull(Sum(b.Quantity),0) from AssignedInventoryTechReceived b where b.EquipmentId = _eqp.EquipmentId AND b.TechnicianId =
	                                       '22222222-2222-2222-2222-222222222222' And b.IsApprove = 0 and b.IsDecline = 0)
                                        ) as  InventoryQuantity,
                                _eqp.Name,
                                _inv.LocationId
                                from InventoryWarehouse _inv 
                                join Equipment _eqp
                                on _eqp.CompanyId = _inv.CompanyId and _eqp.EquipmentId = _inv.EquipmentId
                                where
                                _inv.LocationId = '22222222-2222-2222-2222-222222222222' and
                                _eqp.EquipmentId = '{0}' and _eqp.CompanyId = '{1}'
                                group by 
								_eqp.EquipmentId,
								_eqp.Name,
								_inv.LocationId";
            try
            {
                sqlQuery = string.Format(sqlQuery, EquipmentId, CompanyId);
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

        public DataTable GetCustomInventoryLocationEquipmentByEquipmentIdAndCompanyId(Guid EquipmentId, Guid CompanyId, Guid LocationId)
        {

            string sqlQuery = @"select
                                _eqp.EquipmentId as EquipmentId,
                                ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add')
                                - (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=_eqp.EquipmentId and Type='Release')
                                - isnull((select sum(Quantity) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = _eqp.EquipmentId and 
                                b.TechnicianId  = '22222222-2222-2222-2222-222222222222' and b.IsApprove = 0 and b.IsDecline = 0 ),0)) as InventoryQuantity,
                                _eqp.Name,
                                _inv.LocationId
                                from InventoryWarehouse _inv 
                                join Equipment _eqp
                                on _eqp.CompanyId = _inv.CompanyId and _eqp.EquipmentId = _inv.EquipmentId
                                where
                                _inv.LocationId = '{2}' and
                                _eqp.EquipmentId = '{0}' and _eqp.CompanyId = '{1}'
                                group by 
								_eqp.EquipmentId,
								_eqp.Name";
            try
            {
                sqlQuery = string.Format(sqlQuery, EquipmentId, CompanyId, LocationId);
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

        public DataSet GetProductListByFilter(FilterEquipment filter, DateTime? StartDate, DateTime? EndDate)
        {
            string DateQuery = "";
            if (StartDate != new DateTime() && EndDate != new DateTime())
            {
                DateQuery = string.Format("and _eqp.LastUpdatedDate between  '{0}' and '{1}'", StartDate, EndDate);
            }
            string sqlQuery = @"declare @pagestart int
                                declare @pageend int
                                declare @pageno int
                                declare @pagesize int
                                declare @CompanyId uniqueidentifier
                                set @CompanyId = '{0}'
                                set @pageno={1}
                                set @pagesize ={2}

                                set @pagestart =(@pageno-1)* @pagesize
                                set @pageend = @pagesize

                                SELECT 
                                    _eqp.*,
                                    _eqpClass.Name as EquipmentClass,
                                    _man.Name as ManufacturerName
      
                                        INTO #CustomerData
                                        FROM Equipment _eqp

                                        LEFT JOIN EquipmentClass _eqpClass
		                                ON _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                                        LEFT JOIN Manufacturer _man
		                                ON _man.Id = _eqp.ManufacturerId and _man.CompanyId = @CompanyId
		                                WHERE 
			                                _eqp.CompanyId = @CompanyId
                                            {4}
                                            {5}
                                            {6}
                                            {7}
                                            {10}
                                        ORDER BY Id DESC 
                                        SELECT * INTO #CustomerFilterData
                                        FROM #CustomerData
                                            
	                                    SELECT TOP (@pagesize)
                                        * into #Testtable
                                        FROM #CustomerFilterData
                                        where   Id NOT IN(Select TOP (@pagestart)  Id from #CustomerData {8})
                                        {9}      
                                        select * from #Testtable
                                         select sum(Point) as TotalPoint,sum(SupplierCost) as TotalSupplierCost, Sum(Cost) as TotalCost,Sum(Retail) as TotalRetail from #Testtable

                                        select count(*) [TotalCount]
                                        from #CustomerFilterData

                                        DROP TABLE #CustomerData
                                        DROP TABLE #CustomerFilterData
                                        drop table #Testtable ";

            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string subquery = "";
            string subquery1 = "";
            if (filter.order == "undefined")
            {
                filter.order = null;
            }
            #region Order
            if (!string.IsNullOrWhiteSpace(filter.order))
            {
                if (filter.order == "ascending/name")
                {
                    subquery = "order by #CustomerData.[Name] asc";
                    subquery1 = "order by [Name] asc";
                }
                else if (filter.order == "descending/name")
                {
                    subquery = "order by #CustomerData.[Name] desc";
                    subquery1 = "order by [Name] desc";
                }
                else if (filter.order == "ascending/point")
                {
                    subquery = "order by #CustomerData.[Point] asc";
                    subquery1 = "order by [Point] asc";
                }
                else if (filter.order == "descending/point")
                {
                    subquery = "order by #CustomerData.[Point] desc";
                    subquery1 = "order by [Point] desc";
                }
                else if (filter.order == "ascending/suppliercost")
                {
                    subquery = "order by #CustomerData.[SupplierCost] asc";
                    subquery1 = "order by [SupplierCost] asc";
                }
                else if (filter.order == "descending/suppliercost")
                {
                    subquery = "order by #CustomerData.[SupplierCost] desc";
                    subquery1 = "order by [SupplierCost] desc";
                }
                else if (filter.order == "ascending/cost")
                {
                    subquery = "order by #CustomerData.[Cost] asc";
                    subquery1 = "order by [Cost] asc";
                }
                else if (filter.order == "descending/cost")
                {
                    subquery = "order by #CustomerData.[Cost] desc";
                    subquery1 = "order by [Cost] desc";
                }
                else if (filter.order == "ascending/retailprice")
                {
                    subquery = "order by #CustomerData.[Retail] asc";
                    subquery1 = "order by [Retail] desc";
                }
                else if (filter.order == "descending/retailprice")
                {
                    subquery = "order by #CustomerData.[Retail] asc";
                    subquery1 = "order by [Retail] asc";
                }
                else if (filter.order == "ascending/manufacturer")
                {
                    subquery = "order by #CustomerData.[ManufacturerName] asc";
                    subquery1 = "order by [ManufacturerName] asc";
                }
                else if (filter.order == "descending/manufacturer")
                {
                    subquery = "order by #CustomerData.[ManufacturerName] desc";
                    subquery1 = "order by [ManufacturerName] desc";
                }
                else if (filter.order == "ascending/class")
                {
                    subquery = "order by #CustomerData.[EquipmentClass] asc";
                    subquery1 = "order by [EquipmentClass] asc";
                }
                else if (filter.order == "descending/class")
                {
                    subquery = "order by #CustomerData.[EquipmentClass] desc";
                    subquery1 = "order by [EquipmentClass] desc";
                }
                else if (filter.order == "ascending/sku")
                {
                    subquery = "order by #CustomerData.[SKU] asc";
                    subquery1 = "order by [SKU] asc";
                }
                else if (filter.order == "descending/sku")
                {
                    subquery = "order by #CustomerData.[SKU] desc";
                    subquery1 = "order by [SKU] desc";
                }

            }
            else
            {
                subquery = "order by #CustomerData.[Id] desc";
                subquery1 = "order by Id desc";
            }
            #endregion
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                filterColumntext = @"and( isNULL(_eqp.Name, '') +
								isNULL(_eqp.SKU,'') + 
								isNULL(_eqpClass.Name,''))  FilterText  ";
                filtertext = string.Format("and isNULL(_eqp.Name, '') like '%{0}%' ", filter.SearchText);
            }

            if (filter.ActiveStatus == -1 || filter.ActiveStatus == 1)
            {
                filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");
            }
            else if (filter.ActiveStatus == 0)
            {
                filterByActiveStatus = string.Format("AND _eqp.IsActive = 0");
            }

            if (filter.EquipmentClass != -1 && filter.EquipmentClass != 0)
            {
                filterByEquipmentClass = string.Format("AND _eqp.EquipmentClassId = '{0}'", filter.EquipmentClass);
            }
            if (filter.EquipmentCategory != -1 && filter.EquipmentCategory != 0)
            {
                filterByEquipmentType = string.Format("AND _eqp.EquipmentTypeId = '{0}'", filter.EquipmentCategory);
            }

            try
            {
                sqlQuery = string.Format(sqlQuery,
                    filter.CompanyId,//0
                    filter.PageNo,//1
                    filter.PageSize,//2
                    filterColumntext, //3
                    filtertext, //4 
                    filterByActiveStatus,//5 
                    filterByEquipmentClass, //6
                    filterByEquipmentType, //7
                    subquery,//8
                    subquery1,//9
                    DateQuery//10
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

        public DataTable DownloadProductListByFilter(FilterEquipment filter, DateTime? StartDate, DateTime? EndDate)
        {
            string DateQuery = "";
            if (StartDate != new DateTime() && EndDate != new DateTime())
            {
                DateQuery = string.Format("and _eqp.LastUpdatedDate between  '{0}' and '{1}'", StartDate, EndDate);
            }
            string sqlQuery = @"declare @pagestart int
                                declare @pageend int
                                declare @pageno int
                                declare @pagesize int
                                declare @CompanyId uniqueidentifier
                                set @CompanyId = '{0}'
                                set @pageno={1}
                                set @pagesize ={2}

                                set @pagestart =(@pageno-1)* @pagesize
                                set @pageend = @pagesize

                                SELECT 
                                    _eqp.*,
                                    _man.Name as ManufacturerName,
                                    _eqpClass.Name as EquipmentClass
      
                                        INTO #CustomerData
                                        FROM Equipment _eqp

                                        LEFT JOIN EquipmentClass _eqpClass
		                                ON _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                                        LEFT JOIN Manufacturer _man
		                                ON _man.Id = _eqp.ManufacturerId and _man.CompanyId = @CompanyId
		                                WHERE 
			                                _eqp.CompanyId = @CompanyId
                                            {4}
                                            {5}
                                            {6}
                                            {7}
                                            {10}
                                        ORDER BY Id DESC 
                                        SELECT * INTO #CustomerFilterData
                                        FROM #CustomerData
                                            
	                                    SELECT TOP (select count(*) [TotalCount]
                                        from #CustomerFilterData)
                                        * into #Testtable
                                        FROM #CustomerFilterData
                                        where   Id NOT IN(Select TOP (@pagestart)  Id from #CustomerData {8})
                                        {9}      
                                        --select * from #Testtable
                                        select Name, SKU, Point,
										cast(SupplierCost as decimal(10,2)) as [Supplier Cost],
										cast(Cost as decimal(10,2)) as Cost,
										cast(Retail as decimal(10,2)) as Retail,
										ManufacturerName, EquipmentClass  from #Testtable

                                        DROP TABLE #CustomerData
                                        DROP TABLE #CustomerFilterData
                                        drop table #Testtable ";

            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string subquery = "";
            string subquery1 = "";
            if (filter.order == "undefined")
            {
                filter.order = null;
            }
            #region Order
            if (!string.IsNullOrWhiteSpace(filter.order))
            {
                if (filter.order == "ascending/name")
                {
                    subquery = "order by #CustomerData.[Name] asc";
                    subquery1 = "order by [Name] asc";
                }
                else if (filter.order == "descending/name")
                {
                    subquery = "order by #CustomerData.[Name] desc";
                    subquery1 = "order by [Name] desc";
                }
                else if (filter.order == "ascending/point")
                {
                    subquery = "order by #CustomerData.[Point] asc";
                    subquery1 = "order by [Point] asc";
                }
                else if (filter.order == "descending/point")
                {
                    subquery = "order by #CustomerData.[Point] desc";
                    subquery1 = "order by [Point] desc";
                }
                else if (filter.order == "ascending/suppliercost")
                {
                    subquery = "order by #CustomerData.[SupplierCost] asc";
                    subquery1 = "order by [SupplierCost] asc";
                }
                else if (filter.order == "descending/suppliercost")
                {
                    subquery = "order by #CustomerData.[SupplierCost] desc";
                    subquery1 = "order by [SupplierCost] desc";
                }
                else if (filter.order == "ascending/cost")
                {
                    subquery = "order by #CustomerData.[Cost] asc";
                    subquery1 = "order by [Cost] asc";
                }
                else if (filter.order == "descending/cost")
                {
                    subquery = "order by #CustomerData.[Cost] desc";
                    subquery1 = "order by [Cost] desc";
                }
                else if (filter.order == "ascending/retailprice")
                {
                    subquery = "order by #CustomerData.[Retail] asc";
                    subquery1 = "order by [Retail] desc";
                }
                else if (filter.order == "descending/retailprice")
                {
                    subquery = "order by #CustomerData.[Retail] asc";
                    subquery1 = "order by [Retail] asc";
                }
                else if (filter.order == "ascending/manufacturer")
                {
                    subquery = "order by #CustomerData.[ManufacturerName] asc";
                    subquery1 = "order by [ManufacturerName] asc";
                }
                else if (filter.order == "descending/manufacturer")
                {
                    subquery = "order by #CustomerData.[ManufacturerName] desc";
                    subquery1 = "order by [ManufacturerName] desc";
                }
                else if (filter.order == "ascending/class")
                {
                    subquery = "order by #CustomerData.[EquipmentClass] asc";
                    subquery1 = "order by [EquipmentClass] asc";
                }
                else if (filter.order == "descending/class")
                {
                    subquery = "order by #CustomerData.[EquipmentClass] desc";
                    subquery1 = "order by [EquipmentClass] desc";
                }
                else if (filter.order == "ascending/sku")
                {
                    subquery = "order by #CustomerData.[SKU] asc";
                    subquery1 = "order by [SKU] asc";
                }
                else if (filter.order == "descending/sku")
                {
                    subquery = "order by #CustomerData.[SKU] desc";
                    subquery1 = "order by [SKU] desc";
                }

            }
            else
            {
                subquery = "order by #CustomerData.[Id] desc";
                subquery1 = "order by Id desc";
            }
            #endregion
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                filterColumntext = @"and( isNULL(_eqp.Name, '') +
								isNULL(_eqp.SKU,'') + 
								isNULL(_eqpClass.Name,''))  FilterText  ";
                filtertext = string.Format("and isNULL(_eqp.Name, '') like '%{0}%' ", filter.SearchText);
            }

            if (filter.ActiveStatus == -1 || filter.ActiveStatus == 1)
            {
                filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");
            }
            else if (filter.ActiveStatus == 0)
            {
                filterByActiveStatus = string.Format("AND _eqp.IsActive = 0");
            }

            if (filter.EquipmentClass != -1 && filter.EquipmentClass != 0)
            {
                filterByEquipmentClass = string.Format("AND _eqp.EquipmentClassId = '{0}'", filter.EquipmentClass);
            }
            if (filter.EquipmentCategory != -1 && filter.EquipmentCategory != 0)
            {
                filterByEquipmentType = string.Format("AND _eqp.EquipmentTypeId = '{0}'", filter.EquipmentCategory);
            }

            try
            {
                sqlQuery = string.Format(sqlQuery,
                    filter.CompanyId,//0
                    filter.PageNo,//1
                    filter.PageSize,//2
                    filterColumntext, //3
                    filtertext, //4 
                    filterByActiveStatus,//5 
                    filterByEquipmentClass, //6
                    filterByEquipmentType, //7
                    subquery,//8
                    subquery1,//9
                    DateQuery//10
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


        public DataSet GetEquipmentListByFilter(FilterEquipment filter)
        {
            string sqlQuery = @" declare @pagestart int
                                    declare @pageend int
                                    declare @pageno int
                                    declare @pagesize int
                                    set @pageno = {10}
                                    set @pagesize = {11}


                                    set @pagestart=(@pageno-1)* @pagesize 
                                    set @pageend = @pagesize

                                    SELECT 
                                        _eqp.*,
                                        _eqpClass.Name as EquipmentClass,
	                                    ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add' And invinner.LocationId = '22222222-2222-2222-2222-222222222222')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=_eqp.EquipmentId and Type='Release' And invinner2.LocationId = '22222222-2222-2222-2222-222222222222')
                                            	-(select ISNull(Sum(b.Quantity),0) from AssignedInventoryTechReceived b where b.EquipmentId = _eqp.EquipmentId AND b.TechnicianId =
	                                       '22222222-2222-2222-2222-222222222222' And b.IsApprove = 0 and b.IsDecline = 0)
                                        ) as Quantity,
                                         sup.CompanyName as SupplierName,
                                          _eqpType.Name as Category,
										  --_eqp.SupplierCost as VendorCost,
                                          eqpv.Cost as VendorCost,
										  manu.Name as ManufacturerName,
                                          
                                          (SELECT 
                                          ISNULL(SUM(Quantity),0) AS TotalQty
                                          FROM
                                          (SELECT 
                                          ISNULL(SUM(CASE WHEN Type = 'ADD' THEN Quantity ELSE -Quantity END),0) AS Quantity
                                          FROM 
                                          InventoryTech
                                          WHERE 
                                          EquipmentId = _eqp.EquipmentId AND
										  TechnicianId NOT IN 
										  ('00000000-0000-0000-0000-000000000000','22222222-2222-2222-2222-222222222222', '22222222-2222-2222-2222-222222222223','22222222-2222-2222-2222-222222222224',
	                                     '22222222-2222-2222-2222-222222222225','22222222-2222-2222-2222-222222222226','22222222-2222-2222-2222-222222222231','22222222-2222-2222-2222-222222222232',
	                                     '22222222-2222-2222-2222-222222222233','22222222-2222-2222-2222-222222222222','22222222-2222-2222-2222-222222222221')
                                          GROUP BY 
                                          TechnicianId										  
                                          HAVING 
                                          SUM(CASE WHEN Type = 'ADD' THEN Quantity ELSE -Quantity END) >= 0) AS Qty)-
										 (select ISNULL(Sum(b.Quantity),0) from AssignedInventoryTechReceived b where b.EquipmentId = _eqp.EquipmentId AND b.TechnicianId NOT IN
										 ('00000000-0000-0000-0000-000000000000','22222222-2222-2222-2222-222222222222', '22222222-2222-2222-2222-222222222223','22222222-2222-2222-2222-222222222224',
	                                     '22222222-2222-2222-2222-222222222225','22222222-2222-2222-2222-222222222226','22222222-2222-2222-2222-222222222231','22222222-2222-2222-2222-222222222232',
	                                     '22222222-2222-2222-2222-222222222233','22222222-2222-2222-2222-222222222222','22222222-2222-2222-2222-222222222221') And b.IsApprove = 0 and b.IsDecline = 0)
										  as technician,
                                        
                                        (select AVG(newtab.UnitPrice) from
                                        (select TOP 3 iwin.*, pod.UnitPrice from InventoryWarehouse iwin left join PurchaseOrderDetail pod on pod.PurchaseOrderId=iwin.PurchaseOrderId
                                        where iwin.PurchaseOrderId != '' and (UnitPrice!=0 or UnitPrice is not null) AND iwin.EquipmentId=_eqp.EquipmentId
                                        AND pod.EquipmentId=_eqp.EquipmentId order by LastUpdatedDate desc)
                                        newtab group by EquipmentId) as FIFO,
                                        ([dbo].GetAssignedInventoryTechQuantityCount(null,_eqp.EquipmentId,1)) as InQueue,
                                   ((select ISNULL(SUM(invinner.Quantity),0) from [InventoryWarehouse] invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  and invinner.LocationId IN( '22222222-2222-2222-2222-222222222223','22222222-2222-2222-2222-222222222224',
	                               '22222222-2222-2222-2222-222222222225','22222222-2222-2222-2222-222222222226','22222222-2222-2222-2222-222222222231','22222222-2222-2222-2222-222222222232',
	                              '22222222-2222-2222-2222-222222222233'))

								  -

                                    (select ISNULL(sum(Quantity),0) from dbo.InventoryWarehouse b where b.EquipmentId  = _eqp.EquipmentId  and Type='Release'
			                        AND b.LocationId IN( '22222222-2222-2222-2222-222222222223','22222222-2222-2222-2222-222222222224',
	                               '22222222-2222-2222-2222-222222222225','22222222-2222-2222-2222-222222222226','22222222-2222-2222-2222-222222222231','22222222-2222-2222-2222-222222222232',
	                               '22222222-2222-2222-2222-222222222233'))
								   -



                                   (select ISNULL(Sum(b.Quantity),0) from AssignedInventoryTechReceived b where b.EquipmentId = _eqp.EquipmentId AND b.TechnicianId IN( '22222222-2222-2222-2222-222222222223','22222222-2222-2222-2222-222222222224',
	                               '22222222-2222-2222-2222-222222222225','22222222-2222-2222-2222-222222222226','22222222-2222-2222-2222-222222222231','22222222-2222-2222-2222-222222222232',
	                               '22222222-2222-2222-2222-222222222233') And b.IsApprove = 0 and b.IsDecline = 0)) as LocQoH,
										  ISNULL((((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=_eqp.EquipmentId and Type='Release'))
										  +
										  (select ISNULL(SUM(tech.Quantity), 0) from InventoryTech tech where _eqp.EquipmentId = tech.EquipmentId and tech.[Type] = 'Add'
                                          AND tech.TechnicianId Not IN('22222222-2222-2222-2222-222222222222', '22222222-2222-2222-2222-222222222223','22222222-2222-2222-2222-222222222224',
	                                     '22222222-2222-2222-2222-222222222225','22222222-2222-2222-2222-222222222226','22222222-2222-2222-2222-222222222231','22222222-2222-2222-2222-222222222232',
	                                     '22222222-2222-2222-2222-222222222233')  
                                          ) 
                                       - (select ISNULL(SUM(tech.Quantity), 0) from InventoryTech tech where _eqp.EquipmentId = tech.EquipmentId and tech.[Type] = 'Release'
                                    AND tech.TechnicianId Not IN('22222222-2222-2222-2222-222222222222', '22222222-2222-2222-2222-222222222223','22222222-2222-2222-2222-222222222224',
	                               '22222222-2222-2222-2222-222222222225','22222222-2222-2222-2222-222222222226','22222222-2222-2222-2222-222222222231','22222222-2222-2222-2222-222222222232',
	                               '22222222-2222-2222-2222-222222222233')
                                        )), 0) as TotalEq
                                          {1}
                                          INTO #CustomerData
                                          FROM Equipment _eqp
                                            LEFT JOIN EquipmentClass _eqpClass
		                                    ON _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                                            left JOIN EquipmentVendor eqpv
											ON eqpv.EquipmentId = _eqp.EquipmentId
                                            and eqpv.IsPrimary=1
											 LEFT JOIN EquipmentType _eqpType
		                                    ON _eqp.EquipmentTypeId = _eqpType.Id and _eqp.CompanyId = _eqpType.CompanyId
                                            --left join Supplier sup on sup.Id = _eqp.SupplierId
                                            left join Supplier sup on sup.SupplierId = eqpv.SupplierId
											left join Manufacturer manu
											ON manu.Id = _eqp.ManufacturerId
		                                    WHERE 
			                                    _eqp.CompanyId = '{0}'
                                                {3}
                                                {4}
                                                {5}
                                                {6}
                                
                                           {7}
                                           SELECT 
                                           Id,EquipmentId,CompanyId,Name,SKU,ManufacturerId,SupplierId,EquipmentTypeId,
										   EquipmentClassId,Point,SupplierCost,Cost,Retail,EqOrder,Service,AsOfDate,reorderpoint,
										   IsActive,Comments,CreatedDate,LastUpdatedDate,LastUpdatedBy,POOrder,IsKit,RepCost,RackNo,Location,
										   Type,Model,Finish,Capacity,EquipmentPriceIsCharged,ModelNumber,Barcode,Tag,Note,IsWarrenty,IsARBEnabled,
										   IsUpsold,IsTaxable,OverheadRate,ProfitRate,Unit,TaggedEmail,IsIncludeEstimate,LaborPrice,WarehouseReorder,
										   EquipmentClass,Quantity,SupplierName,Category,VendorCost,ManufacturerName,technician,FIFO,InQueue,LocQoH
										   ,TotalEq = ISNULL(Quantity,0) + ISNULL(technician,0) + ISNULL(LocQoH,0) + ISNULL(InQueue,0)
                                           INTO #CustomerFilterData
                                           FROM #CustomerData
                                           {2}
                                
	                                       SELECT TOP (@pagesize)
                                           *
                                           FROM #CustomerFilterData _cfd
                                           where   Id NOT IN(Select TOP (@pagestart)  Id from #CustomerData _cd {8})
                                           {9}
                                           select count(*) [TotalCount]
                                           from #CustomerFilterData

                                           DROP TABLE #CustomerData
                                           DROP TABLE #CustomerFilterData
";
            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string orderquery = "";
            string orderquery1 = "";
            string orderquery2 = "";
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                filterColumntext = @",isNULL(_eqp.Name, '') +
								isNULL(_eqp.SKU,'') + 
                                isNULL(_eqpType.Name,'') + 
                                isNULL(sup.CompanyName,'') +
                                isNULL(manu.Name,'') + 
								isNULL(_eqpClass.Name,'')  FilterText  ";
                filtertext = string.Format(" Where FilterText like '%{0}%' ", filter.SearchText);
            }
            //if (filter.ActiveStatus == -1)
            //{
            //    filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");
            //}
            //else
            //{
            //    if (filter.ActiveStatus == 1)
            //    {
            //        filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");
            //    }
            //    if (filter.ActiveStatus == 0)
            //    {
            //        filterByActiveStatus = string.Format("AND _eqp.IsActive = 0");
            //    }

            //}
            if (!string.IsNullOrWhiteSpace(filter.ActiveInactiveStatus) && filter.ActiveInactiveStatus != "'null'" && filter.ActiveInactiveStatus != "'undefined'" && filter.ActiveInactiveStatus != "null" && filter.ActiveInactiveStatus != "undefined")
            {
                filterByActiveStatus = string.Format("and _eqp.IsActive in ({0})", filter.ActiveInactiveStatus);
            }
            if (filter.EquipmentClass != -1)
            {
                filterByEquipmentClass = string.Format("AND _eqp.EquipmentClassId = '{0}'", filter.EquipmentClass);
            }
            if (filter.EquipmentCategory != -1)
            {
                filterByEquipmentType = string.Format("AND _eqp.EquipmentTypeId = '{0}'", filter.EquipmentCategory);
            }

            #region OrderQuery
            if (!string.IsNullOrWhiteSpace(filter.order) && filter.order != "null")
            {
                if (filter.order == "ascending/category")
                {
                    orderquery = "order by _eqpType.Name asc";
                    orderquery1 = "order by _cd.Category asc";
                    orderquery2 = "order by _cfd.Category asc";
                }
                else if (filter.order == "descending/category")
                {
                    orderquery = "order by _eqpType.Name desc";
                    orderquery1 = "order by _cd.Category desc";
                    orderquery2 = "order by _cfd.Category desc";
                }
                else if (filter.order == "ascending/monthlyfee")
                {
                    orderquery = "order by _eqp.Retail asc";
                    orderquery1 = "order by _cd.Retail asc";
                    orderquery2 = "order by _cfd.Retail asc";
                }
                else if (filter.order == "descending/monthlyfee")
                {
                    orderquery = "order by _eqp.Retail desc";
                    orderquery1 = "order by _cd.Retail desc";
                    orderquery2 = "order by _cfd.Retail desc";
                }
                else if (filter.order == "ascending/description")
                {
                    orderquery = "order by _eqp.Comments asc";
                    orderquery1 = "order by _cd.Comments asc";
                    orderquery2 = "order by _cfd.Comments asc";
                }
                else if (filter.order == "descending/description")
                {
                    orderquery = "order by _eqp.Comments desc";
                    orderquery1 = "order by _cd.Comments desc";
                    orderquery2 = "order by _cfd.Comments desc";
                }
                else if (filter.order == "ascending/manu")
                {
                    orderquery = "order by manu.Name asc";
                    orderquery1 = "order by _cd.ManufacturerName asc";
                    orderquery2 = "order by _cfd.ManufacturerName asc";
                }
                else if (filter.order == "descending/manu")
                {
                    orderquery = "order by manu.Name desc";
                    orderquery1 = "order by _cd.ManufacturerName desc";
                    orderquery2 = "order by _cfd.ManufacturerName desc";
                }
                else if (filter.order == "ascending/des")
                {
                    orderquery = "order by _eqp.Name asc";
                    orderquery1 = "order by _cd.Name asc";
                    orderquery2 = "order by _cfd.Name asc";
                }
                else if (filter.order == "descending/des")
                {
                    orderquery = "order by _eqp.Name desc";
                    orderquery1 = "order by _cd.Name desc";
                    orderquery2 = "order by _cfd.Name desc";
                }
                else if (filter.order == "ascending/sku")
                {
                    orderquery = "order by _eqp.SKU asc";
                    orderquery1 = "order by _cd.SKU asc";
                    orderquery2 = "order by _cfd.SKU asc";
                }
                else if (filter.order == "descending/sku")
                {
                    orderquery = "order by _eqp.SKU desc";
                    orderquery1 = "order by _cd.SKU desc";
                    orderquery2 = "order by _cfd.SKU desc";
                }
                else if (filter.order == "ascending/repcost")
                {
                    orderquery = "order by _eqp.RepCost asc";
                    orderquery1 = "order by _cd.RepCost asc";
                    orderquery2 = "order by _cfd.RepCost asc";
                }
                else if (filter.order == "descending/repcost")
                {
                    orderquery = "order by _eqp.RepCost desc";
                    orderquery1 = "order by _cd.RepCost desc";
                    orderquery2 = "order by _cfd.RepCost desc";
                }
                else if (filter.order == "ascending/vendor")
                {
                    orderquery = "order by sup.CompanyName asc";
                    orderquery1 = "order by _cd.SupplierName asc";
                    orderquery2 = "order by _cfd.SupplierName asc";
                }
                else if (filter.order == "descending/vendor")
                {
                    orderquery = "order by sup.CompanyName desc";
                    orderquery1 = "order by _cd.SupplierName desc";
                    orderquery2 = "order by _cfd.SupplierName desc";
                }
                else if (filter.order == "ascending/vendorcost")
                {
                    orderquery = "order by _eqp.SupplierCost asc";
                    orderquery1 = "order by _cd.SupplierCost asc";
                    orderquery2 = "order by _cfd.SupplierCost asc";
                }
                else if (filter.order == "descending/vendorcost")
                {
                    orderquery = "order by _eqp.SupplierCost desc";
                    orderquery1 = "order by _cd.SupplierCost desc";
                    orderquery2 = "order by _cfd.SupplierCost desc";
                }
                else if (filter.order == "ascending/type")
                {
                    orderquery = "order by _eqp.Id asc";
                    orderquery1 = "order by _cd.Id asc";
                    orderquery2 = "order by _cfd.Id asc";
                }
                else if (filter.order == "descending/type")
                {
                    orderquery = "order by _eqp.Id desc";
                    orderquery1 = "order by _cd.Id desc";
                    orderquery2 = "order by _cfd.Id desc";
                }
                else if (filter.order == "ascending/qty")
                {
                    orderquery = "order by Quantity asc";
                    orderquery1 = "order by _cd.Quantity asc";
                    orderquery2 = "order by _cfd.Quantity asc";
                }
                else if (filter.order == "descending/qty")
                {
                    orderquery = "order by Quantity desc";
                    orderquery1 = "order by _cd.Quantity desc";
                    orderquery2 = "order by _cfd.Quantity desc";
                }
                else if (filter.order == "ascending/rack")
                {
                    orderquery = "order by _eqp.RackNo asc";
                    orderquery1 = "order by _cd.RackNo asc";
                    orderquery2 = "order by _cfd.RackNo asc";
                }
                else if (filter.order == "descending/rack")
                {
                    orderquery = "order by _eqp.RackNo desc";
                    orderquery1 = "order by _cd.RackNo desc";
                    orderquery2 = "order by _cfd.RackNo desc";
                }
                else
                {
                    orderquery = "order by _eqp.IsActive desc";
                    orderquery1 = "order by _cd.IsActive desc";
                    orderquery2 = "order by _cfd.IsActive desc";
                }
            }
            else
            {
                orderquery = "";
                //orderquery1 = "order by _cd.Category";
                //orderquery2 = "order by _cfd.Category";
                orderquery1 = "order by Id desc";
                orderquery2 = "order by Id desc";
            }
            #endregion
            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filter.CompanyId,
                        filterColumntext,
                        filtertext,
                        filterByActiveStatus,
                        filterByEquipmentClass,
                        filterByEquipmentType,
                        filterByStockStatus,
                        orderquery,
                        orderquery1,
                        orderquery2,
                        filter.PageNo,
                        filter.PageSize);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    //AddParameter(cmd, pInt32("pageno", filter.PageNo));
                    //AddParameter(cmd, pInt32("pagesize", filter.PageSize));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetEquipmentListForDownload(FilterEquipment filter)
        {
            string CategoryQuery = "";
            string ManufacturerQuery = "";
            string DescriptionQuery = "";
            string SKUQuery = "";
            string PrimaryVendorQuery = "";
            string PrimaryVendorCostQuery = "";
            string TypeQuery = "";
            string WarehouseQuery = "";
            string TechnicianQuery = "";
            string TotalQuery = "";
            string Transfers = "";
          
            string Commercial = "";
            string Residential = "";
            string Warehouse = "";
            string RMA  = "";
            string RMA_refund = "";
            string Everything = "";
            string RackBinNumberQuery = "";
            string RepCostQuery = "";
            string RetailPriceQuery = "";
            if (filter.GridList != null)
            {
                foreach (var item in filter.GridList)
                {
                    if (item.SelectedColumn.Trim() == "Category")
                    {
                        CategoryQuery = "_eqpType.Name as Category,";
                    }
                    else if (item.SelectedColumn.Trim() == "Manufacturer")
                    {
                        ManufacturerQuery = "manu.Name as Manufacturer,";
                    }
                    else if (item.SelectedColumn.Trim() == "Description")
                    {
                        DescriptionQuery = "_eqp.Name as [Description],";
                    }
                    else if (item.SelectedColumn.Trim() == "SKU")
                    {
                        SKUQuery = "_eqp.SKU,";
                    }
                    else if (item.SelectedColumn.Trim() == "Primary Vendor")
                    {
                        PrimaryVendorQuery = "sup.CompanyName as [Vendor Name],";
                    }
                    else if (item.SelectedColumn.Trim() == "Primary Vendor Cost")
                    {
                        PrimaryVendorCostQuery = " cast(eqpv.Cost as decimal(12, 2)) as [Vendor Cost],";
                    }
                    else if (item.SelectedColumn.Trim() == "Type")
                    {
                        TypeQuery = "_eqp.Type as [Type],";
                    }
                    else if (item.SelectedColumn.Trim() == "On Hand Qty")
                    {
                        // WarehouseQuery = " ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add' and invinner.LocationId='22222222-2222-2222-2222-222222222222')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=_eqp.EquipmentId and Type='Release' and invinner2.LocationId='22222222-2222-2222-2222-222222222222')- isnull((select sum(Quantity) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = _eqp.EquipmentId and b.IsApprove = 0 and b.IsDecline = 0 and (b.ReqSrc like '%WH-Trf%'  or b.ReqSrc like '%WHTT-Approve%' or (b.ReqSrc like '%WHWH%'   and b.receivedBy!='22222222-2222-2222-2222-222222222222')) ),0)) as [Warehouse Quantity],((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add' and invinner.LocationId='22222222-2222-2222-2222-222222222222')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=_eqp.EquipmentId and Type='Release' and invinner2.LocationId='22222222-2222-2222-2222-222222222222')- isnull((select sum(Quantity) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = _eqp.EquipmentId and b.IsApprove = 0 and b.IsDecline = 0 and (b.ReqSrc like '%WH-Trf%'  or b.ReqSrc like '%WHTT-Approve%' or (b.ReqSrc like '%WHWH%'   and b.receivedBy!='22222222-2222-2222-2222-222222222222')) ),0)) as [Warehouse Quantity],";
                        WarehouseQuery = "  ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add' And invinner.LocationId = '22222222-2222-2222-2222-222222222222')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=_eqp.EquipmentId and Type='Release' And invinner2.LocationId = '22222222-2222-2222-2222-222222222222')\r\n \t-(select ISNull(Sum(b.Quantity),0) from AssignedInventoryTechReceived b where b.EquipmentId = _eqp.EquipmentId AND b.TechnicianId =\r\n\t '22222222-2222-2222-2222-222222222222' And b.IsApprove = 0 and b.IsDecline = 0)\r\n)  AS [Warehouse Quantity], ";
                        TechnicianQuery = "((SELECT ISNULL(SUM(Quantity),0) AS TotalQty FROM (SELECT ISNULL(SUM(CASE WHEN Type = 'ADD' THEN Quantity ELSE -Quantity END),0) AS Quantity FROM InventoryTech WHERE EquipmentId = _eqp.EquipmentId AND TechnicianId NOT IN ('00000000-0000-0000-0000-000000000000','22222222-2222-2222-2222-222222222222', '22222222-2222-2222-2222-222222222223','22222222-2222-2222-2222-222222222224','22222222-2222-2222-2222-222222222225','22222222-2222-2222-2222-222222222226','22222222-2222-2222-2222-222222222231','22222222-2222-2222-2222-222222222232','22222222-2222-2222-2222-222222222233','22222222-2222-2222-2222-222222222222','22222222-2222-2222-2222-222222222221') GROUP BY TechnicianId HAVING SUM(CASE WHEN Type = 'ADD' THEN Quantity ELSE -Quantity END) >= 0) AS Qty)) - isnull((select sum(Quantity) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = _eqp.EquipmentId   AND b.TechnicianId NOT IN ('22222222-2222-2222-2222-222222222223','22222222-2222-2222-2222-222222222222','22222222-2222-2222-2222-222222222224', '22222222-2222-2222-2222-222222222225', '22222222-2222-2222-2222-222222222226', '22222222-2222-2222-2222-222222222232', '22222222-2222-2222-2222-222222222231','22222222-2222-2222-2222-222222222221') and b.IsApprove = 0 and b.IsDecline = 0),0) as [Technician Quantity],";
                        TotalQuery = "ISNULL((((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=_eqp.EquipmentId and Type='Release'))  + (select ISNULL(SUM(tech.Quantity), 0) from InventoryTech tech where _eqp.EquipmentId = tech.EquipmentId and tech.[Type] = 'Add') - (select ISNULL(SUM(tech.Quantity), 0) from InventoryTech tech where _eqp.EquipmentId = tech.EquipmentId and tech.[Type] = 'Release') - isnull((select sum(Quantity) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = _eqp.EquipmentId   AND b.TechnicianId NOT IN ('22222222-2222-2222-2222-222222222223','22222222-2222-2222-2222-222222222222','22222222-2222-2222-2222-222222222224', '22222222-2222-2222-2222-222222222225', '22222222-2222-2222-2222-222222222226', '22222222-2222-2222-2222-222222222232', '22222222-2222-2222-2222-222222222231') and b.IsApprove = 0 and b.IsDecline = 0),0)  ), 0)as [Total Quantity],";
                        Transfers = "isnull((select sum(Quantity) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = _eqp.EquipmentId and b.IsApprove = 0 and b.IsDecline = 0),0) as [Transfers],";
                        Commercial = "((Select ISNULL(SUM(invinner.Quantity),0) from [InventoryWarehouse] invinner where invinner.EquipmentId=_eqp.EquipmentId  \r\nand invinner.LocationId = '22222222-2222-2222-2222-222222222223' AND invinner.Type = 'Add')\r\n\t\t\t- isnull((select sum(Quantity) from dbo.InventoryWarehouse b where b.EquipmentId  = _eqp.EquipmentId \r\n\t\t\tAND b.LocationId = '22222222-2222-2222-2222-222222222223' AND b.Type = 'Release' ),0) - (select ISNull(Sum(b.Quantity),0) from AssignedInventoryTechReceived b where b.EquipmentId = _eqp.EquipmentId AND b.TechnicianId = '22222222-2222-2222-2222-222222222223' And b.IsApprove = 0 and b.IsDecline = 0)) AS[Lost Bucket],";
                        Residential = "((Select ISNULL(SUM(invinner.Quantity),0) from [InventoryWarehouse] invinner where invinner.EquipmentId=_eqp.EquipmentId  \r\nand invinner.LocationId = '22222222-2222-2222-2222-222222222224' AND invinner.Type = 'Add')\r\n\t\t\t- isnull((select sum(Quantity) from dbo.InventoryWarehouse b where b.EquipmentId  = _eqp.EquipmentId \r\n\t\t\tAND b.LocationId = '22222222-2222-2222-2222-222222222224' AND b.Type = 'Release' ),0) - (select ISNull(Sum(b.Quantity),0) from AssignedInventoryTechReceived b where b.EquipmentId = _eqp.EquipmentId AND b.TechnicianId = '22222222-2222-2222-2222-222222222224' And b.IsApprove = 0 and b.IsDecline = 0) ) AS[RMA Bucket],";
                        Warehouse = "((Select ISNULL(SUM(invinner.Quantity),0) from [InventoryWarehouse] invinner where invinner.EquipmentId=_eqp.EquipmentId  \r\nand invinner.LocationId = '22222222-2222-2222-2222-222222222225' AND invinner.Type = 'Add')\r\n\t\t\t- isnull((select sum(Quantity) from dbo.InventoryWarehouse b where b.EquipmentId  = _eqp.EquipmentId \r\n\t\t\tAND b.LocationId = '22222222-2222-2222-2222-222222222225' AND b.Type = 'Release' ),0) -(select ISNull(Sum(b.Quantity),0) from AssignedInventoryTechReceived b where b.EquipmentId = _eqp.EquipmentId AND b.TechnicianId = '22222222-2222-2222-2222-222222222225' And b.IsApprove = 0 and b.IsDecline = 0)) AS[Correction Bucket],";
                        RMA = "((Select ISNULL(SUM(invinner.Quantity),0) from [InventoryWarehouse] invinner where invinner.EquipmentId=_eqp.EquipmentId  \r\nand invinner.LocationId = '22222222-2222-2222-2222-222222222226' AND invinner.Type = 'Add')\r\n\t\t\t- isnull((select sum(Quantity) from dbo.InventoryWarehouse b where b.EquipmentId  = _eqp.EquipmentId \r\n\t\t\tAND b.LocationId = '22222222-2222-2222-2222-222222222226' AND b.Type = 'Release' ),0) -(select ISNull(Sum(b.Quantity),0) from AssignedInventoryTechReceived b where b.EquipmentId = _eqp.EquipmentId AND b.TechnicianId = '22222222-2222-2222-2222-222222222226' And b.IsApprove = 0 and b.IsDecline = 0)) AS[Supplies Bucket],";
                        RMA_refund = "((Select ISNULL(SUM(invinner.Quantity),0) from [InventoryWarehouse] invinner where invinner.EquipmentId=_eqp.EquipmentId  \r\nand invinner.LocationId = '22222222-2222-2222-2222-222222222231' AND invinner.Type = 'Add')\r\n\t\t\t- isnull((select sum(Quantity) from dbo.InventoryWarehouse b where b.EquipmentId  = _eqp.EquipmentId \r\n\t\t\tAND b.LocationId = '22222222-2222-2222-2222-222222222231' AND b.Type = 'Release' ),0) -(select ISNull(Sum(b.Quantity),0) from AssignedInventoryTechReceived b where b.EquipmentId = _eqp.EquipmentId AND b.TechnicianId = '22222222-2222-2222-2222-222222222231' And b.IsApprove = 0 and b.IsDecline = 0)) AS[X-Unused 01 Bucket],";
                        Everything = "((Select ISNULL(SUM(invinner.Quantity),0) from [InventoryWarehouse] invinner where invinner.EquipmentId=_eqp.EquipmentId  \r\nand invinner.LocationId = '22222222-2222-2222-2222-222222222232' AND invinner.Type = 'Add')\r\n\t\t\t- isnull((select sum(Quantity) from dbo.InventoryWarehouse b where b.EquipmentId  = _eqp.EquipmentId \r\n\t\t\tAND b.LocationId = '22222222-2222-2222-2222-222222222232' AND b.Type = 'Release' ),0) -(select ISNull(Sum(b.Quantity),0) from AssignedInventoryTechReceived b where b.EquipmentId = _eqp.EquipmentId AND b.TechnicianId = '22222222-2222-2222-2222-222222222232' And b.IsApprove = 0 and b.IsDecline = 0)) AS[X-Unused 02 Bucket],";
                    }
                    else if (item.SelectedColumn.Trim() == "Rack-Bin Number")
                    {
                        RackBinNumberQuery = "_eqp.RackNo as [Rack Bin Number],";
                    }
                    else if (item.SelectedColumn.Trim() == "Rep Cost")
                    {
                        RepCostQuery = "_eqp.RepCost as [Rep Cost],";
                    }
                    else if (item.SelectedColumn.Trim() == "Retail Price")
                    {
                        RetailPriceQuery = "_eqp.Retail as [Retail Price],";
                    }

                }
            }
            string sqlQuery = @"SELECT
										_eqp.Id as [Equipment Id],
										{10}
										{11}
										{12}
										{13}
										{14}
                                        {15}
	                                    {16}
                                        {17}
										{18}
                                        {20}
                                        {21}
                                        {22}
                                        {23}
                                        {24}
                                        {25}
                                        {26}

                                        {19}
										{27}
                                        {28}
										{29}
										iif(_eqp.IsActive = 1, 'Active', 'Inactive') as [Status]
                                        {1}
                                          INTO #CustomerData
                                          FROM Equipment _eqp
                                            LEFT JOIN EquipmentClass _eqpClass
		                                    ON _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                                            left JOIN EquipmentVendor eqpv
											ON eqpv.EquipmentId = _eqp.EquipmentId
                                            and eqpv.IsPrimary=1
											 LEFT JOIN EquipmentType _eqpType
		                                    ON _eqp.EquipmentTypeId = _eqpType.Id and _eqp.CompanyId = _eqpType.CompanyId
                                            --left join Supplier sup on sup.Id = _eqp.SupplierId
                                            left join Supplier sup on sup.SupplierId = eqpv.SupplierId
											left join Manufacturer manu
											ON manu.Id = _eqp.ManufacturerId
		                                    WHERE 
			                                    _eqp.CompanyId = '{0}'
                                                {3}
                                                {4}
                                                {5}
                                                {6}
                                
                                           {7}
                                           SELECT * INTO #CustomerFilterData
                                           FROM #CustomerData
                                           select [Equipment Id],
                                           Category,
                                           Manufacturer,
                                           Description,
                                           SKU,
                                           [Vendor Name],
                                           [Vendor Cost],
                                           Type,
                                           [Warehouse Quantity] = CASE WHEN [Warehouse Quantity] < 0 THEN 0 ELSE [Warehouse Quantity] END,
                                           [Technician Quantity]= CASE WHEN [Technician Quantity] < 0 THEN 0 ELSE  [Technician Quantity]END,
                                           Transfers,
                                           [Lost Bucket] = CASE WHEN [Lost Bucket] < 0 THEN 0 ELSE [Lost Bucket] END,
                                           [RMA Bucket] = CASE WHEN [RMA Bucket] < 0 THEN 0 ELSE [RMA Bucket] END,
                                           [Correction Bucket]=CASE WHEN [Correction Bucket] < 0 THEN 0 ELSE [Correction Bucket] END,
                                           [Supplies Bucket]=CASE WHEN [Supplies Bucket] < 0 THEN 0 ELSE [Supplies Bucket] END,
                                           [X-Unused 01 Bucket] = CASE WHEN [X-Unused 01 Bucket] < 0 THEN 0 ELSE [X-Unused 01 Bucket] END,
                                           [X-Unused 02 Bucket] = CASE WHEN [X-Unused 02 Bucket] < 0 THEN 0 ELSE [X-Unused 02 Bucket] END,
                                           [Total Quantity] = [Warehouse Quantity] + [Technician Quantity] +  [Lost Bucket] + [RMA Bucket] + [Correction Bucket] + [Supplies Bucket]
	                                       + [X-Unused 01 Bucket] + [X-Unused 02 Bucket] + [Transfers],
                                           [Rack Bin Number],
                                           [Rep Cost],
                                           [Retail Price],
                                           Status from #CustomerData
                                           {2}
                                           {8}

                                           DROP TABLE #CustomerData
                                           DROP TABLE #CustomerFilterData";

            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string orderquery = "";
            string orderquery1 = "";
            string orderquery2 = "";
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                filterColumntext = @",isNULL(_eqp.Name, '') +
								isNULL(_eqp.SKU,'') + 
                                isNULL(_eqpType.Name,'') + 
                                isNULL(sup.CompanyName,'') +
                                isNULL(manu.Name,'') + 
								isNULL(_eqpClass.Name,'')  FilterText  ";
                filtertext = string.Format(" Where FilterText like '%{0}%' ", filter.SearchText);
            }
            //if (filter.ActiveStatus == -1)
            //{
            //    filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");
            //}
            //else
            //{
            //    if (filter.ActiveStatus == 1)
            //    {
            //        filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");
            //    }
            //    if (filter.ActiveStatus == 0)
            //    {
            //        filterByActiveStatus = string.Format("AND _eqp.IsActive = 0");
            //    }

            //}
            if (!string.IsNullOrWhiteSpace(filter.ActiveInactiveStatus) && filter.ActiveInactiveStatus != "'null'" && filter.ActiveInactiveStatus != "'undefined'" && filter.ActiveInactiveStatus != "null" && filter.ActiveInactiveStatus != "undefined")
            {
                filterByActiveStatus = string.Format("and _eqp.IsActive in ({0})", filter.ActiveInactiveStatus);
            }
            if (filter.EquipmentClass != -1)
            {
                filterByEquipmentClass = string.Format("AND _eqp.EquipmentClassId = '{0}'", filter.EquipmentClass);
            }
            if (filter.EquipmentCategory != -1)
            {
                filterByEquipmentType = string.Format("AND _eqp.EquipmentTypeId = '{0}'", filter.EquipmentCategory);
            }

            #region OrderQuery
            if (!string.IsNullOrWhiteSpace(filter.order) && filter.order != "null")
            {
                if (filter.order == "ascending/category")
                {
                    orderquery = "order by _eqpType.Name asc";
                    orderquery1 = "order by _cd.Category asc";
                    orderquery2 = "order by _cfd.Category asc";
                }
                else if (filter.order == "descending/category")
                {
                    orderquery = "order by _eqpType.Name desc";
                    orderquery1 = "order by _cd.Category desc";
                    orderquery2 = "order by _cfd.Category desc";
                }
                else if (filter.order == "ascending/monthlyfee")
                {
                    orderquery = "order by _eqp.Retail asc";
                    orderquery1 = "order by _cd.Retail asc";
                    orderquery2 = "order by _cfd.Retail asc";
                }
                else if (filter.order == "descending/monthlyfee")
                {
                    orderquery = "order by _eqp.Retail desc";
                    orderquery1 = "order by _cd.Retail desc";
                    orderquery2 = "order by _cfd.Retail desc";
                }
                else if (filter.order == "ascending/description")
                {
                    orderquery = "order by _eqp.Comments asc";
                    orderquery1 = "order by _cd.Comments asc";
                    orderquery2 = "order by _cfd.Comments asc";
                }
                else if (filter.order == "descending/description")
                {
                    orderquery = "order by _eqp.Comments desc";
                    orderquery1 = "order by _cd.Comments desc";
                    orderquery2 = "order by _cfd.Comments desc";
                }
                else if (filter.order == "ascending/manu")
                {
                    orderquery = "order by manu.Name asc";
                    orderquery1 = "order by _cd.ManufacturerName asc";
                    orderquery2 = "order by _cfd.ManufacturerName asc";
                }
                else if (filter.order == "descending/manu")
                {
                    orderquery = "order by manu.Name desc";
                    orderquery1 = "order by _cd.ManufacturerName desc";
                    orderquery2 = "order by _cfd.ManufacturerName desc";
                }
                else if (filter.order == "ascending/des")
                {
                    orderquery = "order by _eqp.Name asc";
                    orderquery1 = "order by _cd.Name asc";
                    orderquery2 = "order by _cfd.Name asc";
                }
                else if (filter.order == "descending/des")
                {
                    orderquery = "order by _eqp.Name desc";
                    orderquery1 = "order by _cd.Name desc";
                    orderquery2 = "order by _cfd.Name desc";
                }
                else if (filter.order == "ascending/sku")
                {
                    orderquery = "order by _eqp.SKU asc";
                    orderquery1 = "order by _cd.SKU asc";
                    orderquery2 = "order by _cfd.SKU asc";
                }
                else if (filter.order == "descending/sku")
                {
                    orderquery = "order by _eqp.SKU desc";
                    orderquery1 = "order by _cd.SKU desc";
                    orderquery2 = "order by _cfd.SKU desc";
                }
                else if (filter.order == "ascending/repcost")
                {
                    orderquery = "order by _eqp.RepCost asc";
                    orderquery1 = "order by _cd.RepCost asc";
                    orderquery2 = "order by _cfd.RepCost asc";
                }
                else if (filter.order == "descending/repcost")
                {
                    orderquery = "order by _eqp.RepCost desc";
                    orderquery1 = "order by _cd.RepCost desc";
                    orderquery2 = "order by _cfd.RepCost desc";
                }
                else if (filter.order == "ascending/vendor")
                {
                    orderquery = "order by sup.CompanyName asc";
                    orderquery1 = "order by _cd.SupplierName asc";
                    orderquery2 = "order by _cfd.SupplierName asc";
                }
                else if (filter.order == "descending/vendor")
                {
                    orderquery = "order by sup.CompanyName desc";
                    orderquery1 = "order by _cd.SupplierName desc";
                    orderquery2 = "order by _cfd.SupplierName desc";
                }
                else if (filter.order == "ascending/vendorcost")
                {
                    orderquery = "order by _eqp.SupplierCost asc";
                    orderquery1 = "order by _cd.SupplierCost asc";
                    orderquery2 = "order by _cfd.SupplierCost asc";
                }
                else if (filter.order == "descending/vendorcost")
                {
                    orderquery = "order by _eqp.SupplierCost desc";
                    orderquery1 = "order by _cd.SupplierCost desc";
                    orderquery2 = "order by _cfd.SupplierCost desc";
                }
                else if (filter.order == "ascending/type")
                {
                    orderquery = "order by _eqp.Id asc";
                    orderquery1 = "order by _cd.Id asc";
                    orderquery2 = "order by _cfd.Id asc";
                }
                else if (filter.order == "descending/type")
                {
                    orderquery = "order by _eqp.Id desc";
                    orderquery1 = "order by _cd.Id desc";
                    orderquery2 = "order by _cfd.Id desc";
                }
                else if (filter.order == "ascending/qty")
                {
                    orderquery = "order by Quantity asc";
                    orderquery1 = "order by _cd.Quantity asc";
                    orderquery2 = "order by _cfd.Quantity asc";
                }
                else if (filter.order == "descending/qty")
                {
                    orderquery = "order by Quantity desc";
                    orderquery1 = "order by _cd.Quantity desc";
                    orderquery2 = "order by _cfd.Quantity desc";
                }
                else if (filter.order == "ascending/rack")
                {
                    orderquery = "order by _eqp.RackNo asc";
                    orderquery1 = "order by _cd.RackNo asc";
                    orderquery2 = "order by _cfd.RackNo asc";
                }
                else if (filter.order == "descending/rack")
                {
                    orderquery = "order by _eqp.RackNo desc";
                    orderquery1 = "order by _cd.RackNo desc";
                    orderquery2 = "order by _cfd.RackNo desc";
                }
                else
                {
                    orderquery = "order by _eqp.IsActive desc";
                    orderquery1 = "order by _cd.IsActive desc";
                    orderquery2 = "order by _cfd.IsActive desc";
                }
            }
            else
            {
                orderquery = "";
                //orderquery1 = "order by _cd.Category";
                //orderquery2 = "order by _cfd.Category";
                orderquery1 = "order by [Equipment Id] desc";
                orderquery2 = "order by [Equipment Id] desc";
            }
            #endregion
            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filter.CompanyId, //0
                        filterColumntext, //1
                        filtertext, //2
                        filterByActiveStatus, //3
                        filterByEquipmentClass, //4
                        filterByEquipmentType, //5
                        filterByStockStatus, //6
                        orderquery, //7
                        orderquery1, //8
                        orderquery2, //9
                        CategoryQuery, //10
                        ManufacturerQuery, //11
                        DescriptionQuery, //12
                        SKUQuery, //13
                        PrimaryVendorQuery, //14
                        PrimaryVendorCostQuery, //15
                        TypeQuery, //16
                        WarehouseQuery, //17
                        TechnicianQuery, //18
                        TotalQuery, //19
                        Transfers,//20
                        Commercial ,//21
                        Residential ,//22
                        Warehouse,//23
                        RMA,//24
                        RMA_refund ,//25
                        Everything ,//26
                        RackBinNumberQuery, // 27
                        RepCostQuery, //28
                        RetailPriceQuery); //29
                         
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    //AddParameter(cmd, pInt32("pageno", filter.PageNo));
                    //AddParameter(cmd, pInt32("pagesize", filter.PageSize));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetEquipmentListByFilterBranch(FilterEquipment filter)
        {
            string sqlQuery = @"
                                    declare @pagestart int
                                    declare @pageend int
                                    set @pagestart=(@pageno-1)* @pagesize 
                                    set @pageend = @pagesize

                                    SELECT 
                                        _eqp.*,
                                        _eqpClass.Name as EquipmentClass,
	                                    ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryBranch invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryBranch invinner2 where invinner2.EquipmentId=_eqp.EquipmentId and Type='Release')) as Quantity,
                                        sup.Name as SupplierName
                                          {1}
                                          INTO #CustomerData
                                          FROM InventoryBranch _inv
                                          LEFT JOIN Equipment _eqp
                                            ON _eqp.CompanyId = _inv.CompanyId and _eqp.EquipmentId = _inv.EquipmentId
                                            LEFT JOIN EquipmentClass _eqpClass
		                                    ON _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                                            left join Supplier sup on sup.Id = _eqp.SupplierId
		                                    WHERE 
                                               ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryBranch invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryBranch invinner2 where invinner2.EquipmentId=_eqp.EquipmentId and Type='Release'))!=0 AND
			                                    _eqp.CompanyId = '{0}'
                                                {7}
                                                {3}
                                                {4}
                                                {5}
                                                {6}
                                
                                           ORDER BY Id DESC 
                                           SELECT * INTO #CustomerFilterData
                                           FROM #CustomerData
                                           {2}
                                
	                                       SELECT TOP (@pagesize)
                                           *
                                           FROM #CustomerFilterData
                                           where   Id NOT IN(Select TOP (@pagestart)  Id from #CustomerData ORDER BY Id DESC)
                                           ORDER BY Id DESC      
                                           select count(*) [TotalCount]
                                           from #CustomerFilterData

                                           DROP TABLE #CustomerData
                                           DROP TABLE #CustomerFilterData";

            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string FilterByBranchId = "";

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                filterColumntext = @",isNULL(_eqp.Name, '') +
								isNULL(_eqp.SKU,'') + 
                                isNULL(_eqpType.Name,'') + 
                                isNULL(sup.CompanyName,'') +
                                isNULL(manu.Name,'') + 
								isNULL(_eqpClass.Name,'')  FilterText  ";
                filtertext = string.Format(" Where FilterText like '%{0}%' ", filter.SearchText);
            }
            if (filter.ActiveStatus == -1)
            {
                filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");
            }
            else
            {
                if (filter.ActiveStatus == 1)
                {
                    filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");
                }
                if (filter.ActiveStatus == 0)
                {
                    filterByActiveStatus = string.Format("AND _eqp.IsActive = 0");
                }

            }
            if (filter.EquipmentClass != -1)
            {
                filterByEquipmentClass = string.Format("AND _eqp.EquipmentClassId = '{0}'", filter.EquipmentClass);
            }
            if (filter.EquipmentCategory != -1)
            {
                filterByEquipmentType = string.Format("AND _eqp.EquipmentTypeId = '{0}'", filter.EquipmentCategory);
            }
            if (filter.UserId != Guid.Empty)
            {
                FilterByBranchId = " And _inv.BranchId='" + filter.BranchId + "'";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filter.CompanyId,
                        filterColumntext,
                        filtertext,
                        filterByActiveStatus,
                        filterByEquipmentClass,
                        filterByEquipmentType,
                        filterByStockStatus,
                        FilterByBranchId
                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", filter.PageNo));
                    AddParameter(cmd, pInt32("pagesize", filter.PageSize));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetEquipmentListByFilterTech(FilterEquipment filter)
        {
            string sqlQuery = @"
                                    declare @pagestart int
                                    declare @pageend int
                                    set @pagestart=(@pageno-1)* @pagesize 
                                    set @pageend = @pagesize

                                    SELECT 
                                        _eqp.*,
                                        _eqpClass.Name as EquipmentClass,
	                                    Case When ((Select ISNULL(SUM(invinner.Quantity),0) 
								      from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  
								     And invinner.TechnicianId='{8}')
										-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner
										where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  
										And invinner.TechnicianId='{8}'))
                                        - (select isnull(sum(Quantity),0) from dbo.AssignedInventoryTechReceived b 
										 where b.EquipmentId  =_eqp.EquipmentId  
										 and (b.TechnicianId  = '{8}' or (b.TechnicianId = '22222222-2222-2222-2222-222222222221' and b.ReceivedBy = '{8}')) 
                                       and b.IsApprove = 0 and b.IsDecline = 0 )
									 > 0
										Then ((Select ISNULL(SUM(invinner.Quantity),0) 
								      from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  
								     And invinner.TechnicianId='{8}')
										-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner
										where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  
										And invinner.TechnicianId='{8}'))
                                        - (select isnull(sum(Quantity),0) from dbo.AssignedInventoryTechReceived b 
										 where b.EquipmentId  =_eqp.EquipmentId  
										 and (b.TechnicianId  = '{8}' or (b.TechnicianId = '22222222-2222-2222-2222-222222222221' and b.ReceivedBy = '{8}')) 
                                       and b.IsApprove = 0 and b.IsDecline = 0 )
										ELSE 0
										END
										Quantity,
                                        sup.Name as SupplierName,
                                        _eqpType.Name as Category,
                                        manu.Name as ManufacturerName
                                          {1}
                                          INTO #CustomerData
                                          FROM InventoryTech _inv
                                          LEFT JOIN Equipment _eqp
                                            ON _eqp.CompanyId = _inv.CompanyId and _eqp.EquipmentId = _inv.EquipmentId
                                            LEFT JOIN EquipmentClass _eqpClass
		                                    ON _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                                            left join Supplier sup on sup.Id = _eqp.SupplierId
                                            LEFT JOIN EquipmentType _eqpType
		                                    ON _eqp.EquipmentTypeId = _eqpType.Id and _eqp.CompanyId = _eqpType.CompanyId		                                    
                                            left join Manufacturer manu
											ON manu.Id = _eqp.ManufacturerId
                                                WHERE 
			                                    _eqp.CompanyId = '{0}'
                                                AND ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId='{8}')-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId='{8}'))>0
                                                {7}
                                                {3}
                                                {4}
                                                {5}
                                                {6}
                                           /* Commented Out Section Begins
                                            AND (_inv.Description = 'Release from technician to technician [TI-AIT]' OR _inv.Description = 'Transfer Add from purchase order to technician[PURORD-WHTT]'
                                           or      _inv.Description = 'technician from warehouse 22222222-2222-2222-2222-222222222222 [Inv-Tech-Trf]'
                                           or      _inv.Description = 'technician from warehouse 22222222-2222-2222-2222-222222222223 [Inv-Tech-Trf]'
                                           or      _inv.Description = 'technician from warehouse 22222222-2222-2222-2222-222222222224 [Inv-Tech-Trf]'
                                           or      _inv.Description = 'technician from warehouse 22222222-2222-2222-2222-222222222225 [Inv-Tech-Trf]'
                                           or      _inv.Description = 'technician from warehouse 22222222-2222-2222-2222-222222222226 [Inv-Tech-Trf]'
                                           or      _inv.Description = 'technician from warehouse 22222222-2222-2222-2222-222222222231 [Inv-Tech-Trf]'
                                           or      _inv.Description = 'technician from warehouse 22222222-2222-2222-2222-222222222232 [Inv-Tech-Trf]'
                                           or _inv.Description='Quantity adjustment by technician in ticket'
                                         or      _inv.Description = 'Transfer Add to technician from warehouse 22222222-2222-2222-2222-222222222222 [WHTT-Approve]'
                                           or      _inv.Description = 'Transfer Add to technician from warehouse 22222222-2222-2222-2222-222222222223 [WHTT-Approve]'
                                           or      _inv.Description = 'Transfer Add to technician from warehouse 22222222-2222-2222-2222-222222222224 [WHTT-Approve]'
                                           or      _inv.Description = 'Transfer Add to technician from warehouse 22222222-2222-2222-2222-222222222225 [WHTT-Approve]'
                                           or       _inv.Description = 'Transfer Add to technician from warehouse 22222222-2222-2222-2222-222222222226 [WHTT-Approve]'
                                           or       _inv.Description = 'Transfer Add to technician from warehouse 22222222-2222-2222-2222-222222222231 [WHTT-Approve]'
                                           or  _inv.Description = 'Transfer Add to technician from warehouse 22222222-2222-2222-2222-222222222232 [WHTT-Approve]'
                                            )
                                            Commented Out Section Ends */
                                           SELECT DISTINCT * INTO #CustomerFilterData
                                           FROM #CustomerData
                                           {2}

	                                       SELECT TOP (@pagesize)
                                           *
                                           FROM #CustomerFilterData _cfd
                                           where   Id NOT IN(Select TOP (@pagestart)  Id from #CustomerFilterData _cd {9})
                                           {10}

                                           select count(*) [TotalCount]
                                           from #CustomerFilterData

                                           DROP TABLE #CustomerData
                                           DROP TABLE #CustomerFilterData";

            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string FilterByUserId = "";
            string orderquery1 = "order by _cd.Id desc";
            string orderquery2 = "order by _cfd.Id desc";

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                filterColumntext = @",isNULL(_eqp.Name, '') +
								isNULL(_eqp.SKU,'') + 
                                isNULL(_eqpType.Name,'') + 
                                isNULL(sup.CompanyName,'') +
                                isNULL(manu.Name,'') + 
								isNULL(_eqpClass.Name,'')  FilterText  ";
                filtertext = string.Format(" Where FilterText like '%{0}%' ", filter.SearchText);
            }
            if (filter.ActiveStatus == -1)
            {
                filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");
            }
            else
            {
                if (filter.ActiveStatus == 1)
                {
                    filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");
                }
                if (filter.ActiveStatus == 0)
                {
                    filterByActiveStatus = string.Format("AND _eqp.IsActive = 0");
                }

            }
            if (filter.EquipmentClass != -1)
            {
                filterByEquipmentClass = string.Format("AND _eqp.EquipmentClassId = '{0}'", filter.EquipmentClass);
            }
            if (filter.EquipmentCategory != -1)
            {
                filterByEquipmentType = string.Format("AND _eqp.EquipmentTypeId = '{0}'", filter.EquipmentCategory);
            }
            if (filter.UserId != Guid.Empty)
            {
                FilterByUserId = " And _inv.TechnicianId='" + filter.UserId + "'";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filter.CompanyId,
                        filterColumntext,
                        filtertext,
                        filterByActiveStatus,
                        filterByEquipmentClass,
                        filterByEquipmentType,
                        filterByStockStatus,
                        FilterByUserId,
                        filter.UserId,
                        orderquery1,
                        orderquery2);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", filter.PageNo));
                    AddParameter(cmd, pInt32("pagesize", filter.PageSize));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetEquipmentByFilterTechDownload(FilterEquipment filter)
        {
            string sqlQuery = @"
                                    SELECT
                                        _eqp.Id,
                                        _eqpType.Name as Category,
										_eqp.Name as Description,
										 manu.Name as Manufacturer,
										 _eqp.SKU,
                                         _eqp.cost,
	                                     Case When ((Select ISNULL(SUM(invinner.Quantity),0) 
								      from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  
								     And invinner.TechnicianId='{8}')
										-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner
										where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  
										And invinner.TechnicianId='{8}'))
                                        - (select isnull(sum(Quantity),0) from dbo.AssignedInventoryTechReceived b 
										 where b.EquipmentId  =_eqp.EquipmentId  
										 and (b.TechnicianId  = '{8}' or (b.TechnicianId = '22222222-2222-2222-2222-222222222221' and b.ReceivedBy = '{8}')) 
                                       and b.IsApprove = 0 and b.IsDecline = 0 )
									 > 0
										Then ((Select ISNULL(SUM(invinner.Quantity),0) 
								      from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  
								     And invinner.TechnicianId='{8}')
										-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner
										where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  
										And invinner.TechnicianId='{8}'))
                                        - (select isnull(sum(Quantity),0) from dbo.AssignedInventoryTechReceived b 
										 where b.EquipmentId  =_eqp.EquipmentId  
										 and (b.TechnicianId  = '{8}' or (b.TechnicianId = '22222222-2222-2222-2222-222222222221' and b.ReceivedBy = '{8}')) 
                                       and b.IsApprove = 0 and b.IsDecline = 0 )
										ELSE 0
										END
										Quantity,
										_eqp.RepCost
                                        {1}
                                          INTO #CustomerData
                                          FROM InventoryTech _inv
                                          LEFT JOIN Equipment _eqp
                                            ON _eqp.CompanyId = _inv.CompanyId and _eqp.EquipmentId = _inv.EquipmentId
                                            LEFT JOIN EquipmentClass _eqpClass
		                                    ON _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                                            left join Supplier sup on sup.Id = _eqp.SupplierId
                                            LEFT JOIN EquipmentType _eqpType
		                                    ON _eqp.EquipmentTypeId = _eqpType.Id and _eqp.CompanyId = _eqpType.CompanyId		                                    
                                            left join Manufacturer manu
											ON manu.Id = _eqp.ManufacturerId
                                                WHERE 
			                                    _eqp.CompanyId = '{0}'
                                                AND ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId='{8}')-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId='{8}'))>0
                                                {7}
                                                {3}
                                                {4}
                                                {5}
                                                {6}
                                           SELECT DISTINCT * INTO #CustomerFilterData
                                           FROM #CustomerData
                                           {2}

	                                       SELECT

										   Category,
										   Description,
										   Manufacturer,
										   SKU,
										   Quantity,
										   RepCost,
                                           Cost
										   FROM #CustomerFilterData _cfd
                                           {9}

                                           DROP TABLE #CustomerData
                                           DROP TABLE #CustomerFilterData";

            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string FilterByUserId = "";
            string orderquery = "order by _cfd.Id desc";

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                filterColumntext = @",isNULL(_eqp.Name, '') +
								isNULL(_eqp.SKU,'') + 
                                isNULL(_eqpType.Name,'') + 
                                isNULL(sup.CompanyName,'') +
                                isNULL(manu.Name,'') + 
								isNULL(_eqpClass.Name,'')  FilterText  ";
                filtertext = string.Format(" Where FilterText like '%{0}%' ", filter.SearchText);
            }
            if (filter.ActiveStatus == -1)
            {
                filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");
            }
            else
            {
                if (filter.ActiveStatus == 1)
                {
                    filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");
                }
                if (filter.ActiveStatus == 0)
                {
                    filterByActiveStatus = string.Format("AND _eqp.IsActive = 0");
                }

            }
            if (filter.EquipmentClass != -1)
            {
                filterByEquipmentClass = string.Format("AND _eqp.EquipmentClassId = '{0}'", filter.EquipmentClass);
            }
            if (filter.EquipmentCategory != -1)
            {
                filterByEquipmentType = string.Format("AND _eqp.EquipmentTypeId = '{0}'", filter.EquipmentCategory);
            }
            if (filter.UserId != Guid.Empty)
            {
                FilterByUserId = " And _inv.TechnicianId='" + filter.UserId + "'";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filter.CompanyId,
                        filterColumntext,
                        filtertext,
                        filterByActiveStatus,
                        filterByEquipmentClass,
                        filterByEquipmentType,
                        filterByStockStatus,
                        FilterByUserId,
                        filter.UserId,
                        orderquery);
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
        public DataSet GetEquipmentListByFilterTechForReportOnly(FilterEquipment filter)
        {
            string TechQuery = "";
            string TruckAmount = "";
            string DateQuery = "";
            if ((filter.Transferred_Date_From != null && filter.Transferred_Date_To != null && filter.Transferred_Date_From != new DateTime() && filter.Transferred_Date_To != new DateTime()))
            {
                DateQuery = string.Format("where TransferredDate between '{0}' and '{1}'", filter.Transferred_Date_From, filter.Transferred_Date_To);
            }

            string sqlQuery = @"
                                    declare @pagestart int
                                    declare @pageend int
                                    set @pagestart=(@pageno-1)* @pagesize 
                                    set @pageend = @pagesize

                                    
                                        ;WITH CTE AS (
                                          SELECT
                                        
                                        _eqp.*,
                                        _eqpClass.Name as EquipmentClass,
	                                    {12}
                                        {13}
                                        sup.Name as SupplierName,
                                        _eqpType.Name as Category,
                                        manu.Name as ManufacturerName,
                                        emp.FirstName + ' ' + emp.LastName as TechnicianName,
                                        emp2.FirstName+ ' ' + emp2.LastName as ReceivedBy,
                                        tech.LastUpdatedDate as TransferredDate,
                                        --tech.Quantity as TechQuantity
                                         tech.Id as _inv_Id,
                                          {1}
                                         --   INTO #CustomerData
                                       ROW_NUMBER() OVER (PARTITION BY tech.EquipmentId, tech.TechnicianId ORDER BY tech.LastUpdatedDate DESC) AS RowNum
                                          FROM InventoryTech tech 
                                          LEFT JOIN Equipment _eqp
                                            ON _eqp.EquipmentId = tech.EquipmentId
                                            LEFT JOIN EquipmentClass _eqpClass
		                                    ON _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                                            left join Supplier sup on sup.Id = _eqp.SupplierId
                                            LEFT JOIN EquipmentType _eqpType
		                                    ON _eqp.EquipmentTypeId = _eqpType.Id and _eqp.CompanyId = _eqpType.CompanyId		                                    
                                            left join Manufacturer manu
											ON manu.Id = _eqp.ManufacturerId
                                            left join employee emp on tech.TechnicianId = emp.UserId
                                            left join employee emp2 on tech.technicianId=emp2.UserId
                                                WHERE 
			                                    _eqp.CompanyId = '{0}' and  tech.technicianId not in ( '22222222-2222-2222-2222-222222222223','22222222-2222-2222-2222-222222222222','22222222-2222-2222-2222-222222222224',
	                               '22222222-2222-2222-2222-222222222225','22222222-2222-2222-2222-222222222226','22222222-2222-2222-2222-222222222231','22222222-2222-2222-2222-222222222232',
	                              '22222222-2222-2222-2222-222222222233')

                                               -- AND _inv.IsReceived=1 AND _inv.IsApprove=1
                                                --AND ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId='{8}')-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId='{8}'))>0
                                                {7}
                                                {3}
                                                {4}
                                                {5}
                                                {6}
                                               --and _inv.ReceivedDate between  '{16}' and '{17}'
                                           --  {18}
                                     --           {9}
                                             
                                             )
                                             SELECT
                                               *
                                        INTO #CustomerData
                                          FROM CTE
                                           WHERE RowNum = 1 and  Quantity>0;

                                           SELECT * INTO #CustomerFilterData
                                           FROM #CustomerData
                                            {18}
                                           {2}

	                                       SELECT DISTINCT TOP (@pagesize) * into #TestTable
                                           FROM #CustomerFilterData _cfd
                                           where   {14} NOT IN(Select TOP (@pagestart)  {14} from #CustomerFilterData order by #CustomerFilterData.[TransferredDate]  desc )
                                           {10}
                                           
                                          
                                           select count(*) [TotalCount],
                                            SUM(Quantity) AS TotalQty
                                           from #CustomerFilterData;

                                           select * from #TestTable {9}
										   select sum(0) as TotalSupplierCost,
										   sum(Quantity) as TotalQuantity,
										   sum(AmountTruck) as TotalAmount from #TestTable

                                           DROP TABLE #CustomerData
                                           DROP TABLE #CustomerFilterData
                                           DROP TABLE #TestTable
										  ";

            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string FilterByUserId = "";
            string orderquery = "";
            string orderquery1 = "";
            string orderquery2 = "";
            string OrderingId = "_inv_Id"; //Id
            string ReceivedDateFilte = "";
            if (!string.IsNullOrWhiteSpace(filter.SearchText) && filter.SearchText != "-1" && filter.SearchText != "'null'" && filter.SearchText != "'undefined'" && filter.SearchText != "null" && filter.SearchText != "undefined")
            {
                if (filter.isTransferInventoryReport == true)
                {
                    filterColumntext = @",isNULL(_eqp.Name, '') +
                                isNULL(try_convert(nvarchar,_inv.Quantity),'')    FilterText  ";
                }
                else
                {
                    filterColumntext = @"isNULL(_eqp.Name, '') +
								isNULL(_eqp.SKU,'') + 
                                isNULL(emp.FirstName,'') + 
                                isNULL(emp.LastName,'') +
                                isNULL(try_convert(nvarchar,tech.Quantity),'') + 
								isNULL(try_convert(nvarchar,_eqp.SupplierCost),'')   FilterText,  ";

                }



                //filtertext = string.Format(" Where FilterText like '%{0}%' ", filter.SearchText);
                filtertext = string.Format(" where FilterText like @SearchText ");

            }

            if (filter.Transferred_Date_From != new DateTime() && filter.Transferred_Date_To != new DateTime())
            {
                ReceivedDateFilte = string.Format("where TransferredDate between  '{0}' and '{1}'", filter.Transferred_Date_From, filter.Transferred_Date_To);
            }
            else if (filter.Transferred_Date_From == new DateTime() && filter.Transferred_Date_To != new DateTime())
            {
                ReceivedDateFilte = string.Format("where TransferredDate <=  '{0}' ", filter.Transferred_Date_To);
            }
            else if (filter.Transferred_Date_From != new DateTime() && filter.Transferred_Date_To == new DateTime())
            {
                ReceivedDateFilte = string.Format("where TransferredDate >=  '{0}' ", filter.Transferred_Date_From);
            }


            #region Order
            if (!string.IsNullOrWhiteSpace(filter.order))
            {
                if (filter.order == "ascending/category")
                {
                    orderquery = "order by #TestTable.[Category] asc";
                    orderquery1 = "order by [Category] asc";
                }
                else if (filter.order == "descending/category")
                {
                    orderquery = "order by #TestTable.[Category] desc";
                    orderquery1 = "order by [Category] desc";
                }
                else if (filter.order == "ascending/manufacturer")
                {
                    orderquery = "order by #TestTable.ManufacturerName asc";
                    orderquery1 = "order by ManufacturerName asc";
                }
                else if (filter.order == "descending/manufacturer")
                {
                    orderquery = "order by #TestTable.ManufacturerName desc";
                    orderquery1 = "order by ManufacturerName desc";
                }
                else if (filter.order == "ascending/description")
                {
                    orderquery = "order by #TestTable.[Name] asc";
                    orderquery1 = "order by [Name] asc";
                }
                else if (filter.order == "descending/description")
                {
                    orderquery = "order by #TestTable.[Name] desc";
                    orderquery1 = "order by [Name] desc";
                }
                else if (filter.order == "ascending/SKU")
                {
                    orderquery = "order by #TestTable.[SKU] asc";
                    orderquery1 = "order by [SKU] asc";
                }
                else if (filter.order == "descending/SKU")
                {
                    orderquery = "order by #TestTable.[SKU] desc";
                    orderquery1 = "order by [SKU] desc";
                }
                else if (filter.order == "ascending/username")
                {
                    orderquery = "order by #TestTable.[TechnicianName] asc";
                    orderquery1 = "order by [TechnicianName] asc";
                }
                else if (filter.order == "descending/username")
                {
                    orderquery = "order by #TestTable.[TechnicianName] desc";
                    orderquery1 = "order by [TechnicianName] desc";
                }
                else if (filter.order == "ascending/vendorcost")
                {
                    orderquery = "order by #TestTable.[SupplierCost]  asc";
                    orderquery1 = "order by SupplierCost asc";
                }
                else if (filter.order == "descending/vendorcost")
                {
                    orderquery = "order by #TestTable.[SupplierCost]  desc";
                    orderquery1 = "order by SupplierCost desc";
                }
                else if (filter.order == "ascending/pieces")
                {
                    orderquery = "order by #TestTable.[TechQuantity]  asc";
                    orderquery1 = "order by TechQuantity asc";
                }
                else if (filter.order == "descending/pieces")
                {
                    orderquery = "order by #TestTable.[TechQuantity]  desc";
                    orderquery1 = "order by TechQuantity desc";
                }
                else if (filter.order == "ascending/amount")
                {
                    orderquery = "order by #TestTable.[AmountTruck]  asc";
                    orderquery1 = "order by AmountTruck asc";
                }
                else if (filter.order == "descending/amount")
                {
                    orderquery = "order by #TestTable.[AmountTruck]  desc";
                    orderquery1 = "order by AmountTruck desc";
                }
                else if (filter.order == "ascending/transferfrom")
                {
                    orderquery = "order by #TestTable.[ReceivedBy]  asc";
                    orderquery1 = "order by ReceivedBy asc";
                }
                else if (filter.order == "descending/transferfrom")
                {
                    orderquery = "order by #TestTable.[ReceivedBy]  desc";
                    orderquery1 = "order by ReceivedBy desc";
                }
                else if (filter.order == "ascending/date")
                {
                    orderquery = "order by #TestTable.[TransferredDate]  asc";
                    orderquery1 = "order by TransferredDate asc";
                }
                else if (filter.order == "descending/date")
                {
                    orderquery = "order by #TestTable.[TransferredDate]  desc";
                    orderquery1 = "order by TransferredDate desc";
                }

                else
                {
                    orderquery = "order by #TestTable.[TransferredDate]  desc";
                    orderquery1 = "order by TransferredDate desc";
                }


            }
            else
            {
                orderquery = "order by #TestTable.[TransferredDate] desc";
                orderquery1 = "order by TransferredDate desc";
            }
            #endregion
            if (!string.IsNullOrWhiteSpace(filter.technician) && filter.technician != "'null'")
            {
                FilterByUserId = string.Format("And (tech.TechnicianId in ({0}))", filter.technician);
                TechQuery = string.Format("\t\t\t(select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Add' and _tech.EquipmentId = tech.EquipmentId and _tech.TechnicianId = tech.TechnicianId) \r\n                        - (select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Release' and _tech.EquipmentId = tech.EquipmentId and _tech.TechnicianId = tech.TechnicianId) \r\n                        - (select isnull(sum(Quantity),0) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  =tech.EquipmentId  and (b.TechnicianId  = tech.TechnicianId or (b.TechnicianId = '22222222-2222-2222-2222-222222222221' and b.ReceivedBy = tech.TechnicianId)) \r\n                            and b.IsApprove = 0 and b.IsDecline = 0 )\r\n                        as Quantity,");
                TruckAmount = string.Format(" ((Select ISNULL(SUM(_eqp.SupplierCost),0) from Equipment _eqp where tech.EquipmentId=_eqp.EquipmentId And tech.TechnicianId in (tech.TechnicianId)))*\r\n\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t         \t\t\t((select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Add' and _tech.EquipmentId = tech.EquipmentId and _tech.TechnicianId = tech.TechnicianId) \r\n                        - (select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Release' and _tech.EquipmentId = tech.EquipmentId and _tech.TechnicianId = tech.TechnicianId) \r\n                        - (select isnull(sum(Quantity),0) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  =tech.EquipmentId  and (b.TechnicianId  = tech.TechnicianId or (b.TechnicianId = '22222222-2222-2222-2222-222222222221' and b.ReceivedBy = tech.TechnicianId)) \r\n                            and b.IsApprove = 0 and b.IsDecline = 0 )) AmountTruck,");
                //TechQuery = string.Format("((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId in ({0}))-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId in ({0}))) Quantity,", filter.technician);
                //TruckAmount = string.Format("((Select ISNULL(SUM(_eqp.SupplierCost),0) from Equipment _eqp where _inv.EquipmentId=_eqp.EquipmentId And _inv.TechnicianId in ({0})) * ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId in ({0}))-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId in ({0})))) AmountTruck,", filter.technician);
            }
            else
            {
                TechQuery = string.Format("\t\t\t(select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Add' and _tech.EquipmentId = tech.EquipmentId and _tech.TechnicianId = tech.TechnicianId) \r\n                        - (select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Release' and _tech.EquipmentId = tech.EquipmentId and _tech.TechnicianId = tech.TechnicianId) \r\n                        - (select isnull(sum(Quantity),0) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  =tech.EquipmentId  and (b.TechnicianId  = tech.TechnicianId or (b.TechnicianId = '22222222-2222-2222-2222-222222222221' and b.ReceivedBy = tech.TechnicianId)) \r\n                            and b.IsApprove = 0 and b.IsDecline = 0 )\r\n                        as Quantity,");
                TruckAmount = string.Format(" ((Select ISNULL(SUM(_eqp.SupplierCost),0) from Equipment _eqp where tech.EquipmentId=_eqp.EquipmentId And tech.TechnicianId in (tech.TechnicianId)))*\r\n\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t         \t\t\t((select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Add' and _tech.EquipmentId = tech.EquipmentId and _tech.TechnicianId = tech.TechnicianId) \r\n                        - (select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Release' and _tech.EquipmentId = tech.EquipmentId and _tech.TechnicianId = tech.TechnicianId) \r\n                        - (select isnull(sum(Quantity),0) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  =tech.EquipmentId  and (b.TechnicianId  = tech.TechnicianId or (b.TechnicianId = '22222222-2222-2222-2222-222222222221' and b.ReceivedBy = tech.TechnicianId)) \r\n                            and b.IsApprove = 0 and b.IsDecline = 0 )) AmountTruck,");
                //TechQuery = string.Format("((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId=_inv.TechnicianId)-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId=_inv.TechnicianId)) Quantity,");
                //TruckAmount = string.Format("((Select ISNULL(SUM(_eqp.SupplierCost),0) from Equipment _eqp where _inv.EquipmentId=_eqp.EquipmentId) * ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId=_inv.TechnicianId)-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId=_inv.TechnicianId))) AmountTruck,");
            }
            if (!string.IsNullOrWhiteSpace(filter.category) && filter.category != "'null'" && filter.category != "'undefined'" && filter.category != "null" && filter.category != "undefined")
            {
                filterByEquipmentType = string.Format("and _eqpType.Id in ({0})", filter.category);
            }
            if (!string.IsNullOrWhiteSpace(filter.manufact) && filter.manufact != "'null'" && filter.manufact != "'undefined'" && filter.manufact != "null" && filter.manufact != "undefined")
            {
                filterByEquipmentClass = string.Format("and manu.ManufacturerId in ({0})", filter.manufact);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filter.CompanyId,//0
                        filterColumntext,//1
                        filtertext,//2
                        filterByActiveStatus,//3
                        filterByEquipmentClass,//4
                        filterByEquipmentType,//5
                        filterByStockStatus,//6
                        FilterByUserId,//7
                        filter.UserId,//8
                        orderquery,//9
                        orderquery1,//10
                        orderquery2,//11
                        TechQuery,//12
                        TruckAmount,//13
                        OrderingId,//14
                        ReceivedDateFilte,
                        filter.Start,
                        filter.End,
                        DateQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {

                    AddParameter(cmd, pInt32("pageno", filter.PageNo));
                    AddParameter(cmd, pInt32("pagesize", filter.PageSize));
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", filter.SearchText)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetEquipmentListByFilterTechForReportOnlyTechnician(FilterEquipment filter)
        {
            string TechQuery = "";
            string TruckAmount = "";
            string DateQuery = "";
            if ((filter.Transferred_Date_From != null && filter.Transferred_Date_To != null && filter.Transferred_Date_From != new DateTime() && filter.Transferred_Date_To != new DateTime()))
            {
                DateQuery = string.Format("where TransferredDate between '{0}' and '{1}'", filter.Transferred_Date_From, filter.Transferred_Date_To);
            }

            string sqlQuery = @"
                                    declare @pagestart int
                                    declare @pageend int
                                    set @pagestart=(@pageno-1)* @pagesize 
                                    set @pageend = @pagesize

                                    
                                        ;WITH CTE AS (
                                          SELECT
                                        
                                        _eqp.*,
                                        _eqpClass.Name as EquipmentClass,
	                                    {12}
                                        {13}
                                        sup.Name as SupplierName,
                                        _eqpType.Name as Category,
                                        manu.Name as ManufacturerName,
                                        emp.FirstName + ' ' + emp.LastName as TechnicianName,
                                        emp2.FirstName+ ' ' + emp2.LastName as ReceivedBy,
                                        tech.LastUpdatedDate as TransferredDate,
                                        --tech.Quantity as TechQuantity
                                         tech.Id as _inv_Id,
                                          {1}
                                         --   INTO #CustomerData
                                       ROW_NUMBER() OVER (PARTITION BY tech.EquipmentId, tech.TechnicianId ORDER BY tech.LastUpdatedDate DESC) AS RowNum
                                          FROM InventoryTech tech 
                                          LEFT JOIN Equipment _eqp
                                            ON _eqp.EquipmentId = tech.EquipmentId
                                            LEFT JOIN EquipmentClass _eqpClass
		                                    ON _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                                            left join Supplier sup on sup.Id = _eqp.SupplierId
                                            LEFT JOIN EquipmentType _eqpType
		                                    ON _eqp.EquipmentTypeId = _eqpType.Id and _eqp.CompanyId = _eqpType.CompanyId		                                    
                                            left join Manufacturer manu
											ON manu.Id = _eqp.ManufacturerId
                                            left join employee emp on tech.TechnicianId = emp.UserId
                                            left join employee emp2 on tech.technicianId=emp2.UserId
                                             left join
                                                    UserPermission up ON up.UserId = emp.UserId
                                               left join
                                                    PermissionGroup pg ON pg.Id = up.PermissionGroupId
                                               left join
                                                    UserLogin ul ON emp.UserId = ul.UserId
                                                WHERE 
			                                    _eqp.CompanyId = '{0}' and  tech.technicianId not in ( '22222222-2222-2222-2222-222222222223','22222222-2222-2222-2222-222222222222','22222222-2222-2222-2222-222222222224',
	                               '22222222-2222-2222-2222-222222222225','22222222-2222-2222-2222-222222222226','22222222-2222-2222-2222-222222222231','22222222-2222-2222-2222-222222222232',
	                              '22222222-2222-2222-2222-222222222233')
                                             AND pg.Tag LIKE '%Technician%'
                                             AND emp.IsActive = 1
                                             AND emp.IsDeleted = 0
                                               -- AND _inv.IsReceived=1 AND _inv.IsApprove=1
                                                --AND ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId='{8}')-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId='{8}'))>0
                                                {7}
                                                {3}
                                                {4}
                                                {5}
                                                {6}
                                               --and _inv.ReceivedDate between  '{16}' and '{17}'
                                            -- {18}
                                     --           {9}
                                             
                                             )
                                             SELECT
                                               *
                                        INTO #CustomerData
                                          FROM CTE
                                           WHERE RowNum = 1 and  Quantity>0;

                                           SELECT * INTO #CustomerFilterData
                                           FROM #CustomerData
                                           -- {18}
                                           {2}

	                                       SELECT DISTINCT TOP (@pagesize) * into #TestTable
                                           FROM #CustomerFilterData _cfd
                                           where   {14} NOT IN(Select TOP (@pagestart)  {14} from #CustomerFilterData order by #CustomerFilterData.[TransferredDate]  desc )
                                           {10}
                                           
                                          
                                           select count(*) [TotalCount],
                                                SUM(Quantity) AS TotalQty,
                                            SUM(AmountTruck) AS TotalAmt
                                           from #CustomerFilterData;
                                    

                                           select * from #TestTable {9}
										   select sum(0) as TotalSupplierCost,
										   sum(Quantity) as TotalQuantity,
										   sum(AmountTruck) as TotalAmount from #TestTable

                                           DROP TABLE #CustomerData
                                           DROP TABLE #CustomerFilterData
                                           DROP TABLE #TestTable
										  ";

            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string FilterByUserId = "";
            string orderquery = "";
            string orderquery1 = "";
            string orderquery2 = "";
            string OrderingId = "_inv_Id"; //Id
            string ReceivedDateFilte = "";
            if (!string.IsNullOrWhiteSpace(filter.SearchText) && filter.SearchText != "-1" && filter.SearchText != "'null'" && filter.SearchText != "'undefined'" && filter.SearchText != "null" && filter.SearchText != "undefined")
            {
                if (filter.isTransferInventoryReport == true)
                {
                    filterColumntext = @",isNULL(_eqp.Name, '') +
                                isNULL(try_convert(nvarchar,_inv.Quantity),'')    FilterText  ";
                }
                else
                {
                    filterColumntext = @"isNULL(_eqp.Name, '') +
								isNULL(_eqp.SKU,'') + 
                                isNULL(emp.FirstName,'') + 
                                isNULL(emp.LastName,'') +
                                isNULL(try_convert(nvarchar,tech.Quantity),'') + 
								isNULL(try_convert(nvarchar,_eqp.SupplierCost),'')   FilterText,  ";

                }



                //filtertext = string.Format(" Where FilterText like '%{0}%' ", filter.SearchText);
                filtertext = string.Format(" where FilterText like @SearchText ");

            }

            if (filter.Transferred_Date_From != new DateTime() && filter.Transferred_Date_To != new DateTime())
            {
                ReceivedDateFilte = string.Format("where TransferredDate between  '{0}' and '{1}'", filter.Transferred_Date_From, filter.Transferred_Date_To);
            }
            else if (filter.Transferred_Date_From == new DateTime() && filter.Transferred_Date_To != new DateTime())
            {
                ReceivedDateFilte = string.Format("where TransferredDate <=  '{0}' ", filter.Transferred_Date_To);
            }
            else if (filter.Transferred_Date_From != new DateTime() && filter.Transferred_Date_To == new DateTime())
            {
                ReceivedDateFilte = string.Format("where TransferredDate >=  '{0}' ", filter.Transferred_Date_From);
            }


            #region Order
            if (!string.IsNullOrWhiteSpace(filter.order))
            {
                if (filter.order == "ascending/category")
                {
                    orderquery = "order by #TestTable.[Category] asc";
                    orderquery1 = "order by [Category] asc";
                }
                else if (filter.order == "descending/category")
                {
                    orderquery = "order by #TestTable.[Category] desc";
                    orderquery1 = "order by [Category] desc";
                }
                else if (filter.order == "ascending/manufacturer")
                {
                    orderquery = "order by #TestTable.ManufacturerName asc";
                    orderquery1 = "order by ManufacturerName asc";
                }
                else if (filter.order == "descending/manufacturer")
                {
                    orderquery = "order by #TestTable.ManufacturerName desc";
                    orderquery1 = "order by ManufacturerName desc";
                }
                else if (filter.order == "ascending/description")
                {
                    orderquery = "order by #TestTable.[Name] asc";
                    orderquery1 = "order by [Name] asc";
                }
                else if (filter.order == "descending/description")
                {
                    orderquery = "order by #TestTable.[Name] desc";
                    orderquery1 = "order by [Name] desc";
                }
                else if (filter.order == "ascending/SKU")
                {
                    orderquery = "order by #TestTable.[SKU] asc";
                    orderquery1 = "order by [SKU] asc";
                }
                else if (filter.order == "descending/SKU")
                {
                    orderquery = "order by #TestTable.[SKU] desc";
                    orderquery1 = "order by [SKU] desc";
                }
                else if (filter.order == "ascending/username")
                {
                    orderquery = "order by #TestTable.[TechnicianName] asc";
                    orderquery1 = "order by [TechnicianName] asc";
                }
                else if (filter.order == "descending/username")
                {
                    orderquery = "order by #TestTable.[TechnicianName] desc";
                    orderquery1 = "order by [TechnicianName] desc";
                }
                else if (filter.order == "ascending/vendorcost")
                {
                    orderquery = "order by #TestTable.[SupplierCost]  asc";
                    orderquery1 = "order by SupplierCost asc";
                }
                else if (filter.order == "descending/vendorcost")
                {
                    orderquery = "order by #TestTable.[SupplierCost]  desc";
                    orderquery1 = "order by SupplierCost desc";
                }
                else if (filter.order == "ascending/pieces")
                {
                    orderquery = "order by #TestTable.[TechQuantity]  asc";
                    orderquery1 = "order by TechQuantity asc";
                }
                else if (filter.order == "descending/pieces")
                {
                    orderquery = "order by #TestTable.[TechQuantity]  desc";
                    orderquery1 = "order by TechQuantity desc";
                }
                else if (filter.order == "ascending/amount")
                {
                    orderquery = "order by #TestTable.[AmountTruck]  asc";
                    orderquery1 = "order by AmountTruck asc";
                }
                else if (filter.order == "descending/amount")
                {
                    orderquery = "order by #TestTable.[AmountTruck]  desc";
                    orderquery1 = "order by AmountTruck desc";
                }
                else if (filter.order == "ascending/transferfrom")
                {
                    orderquery = "order by #TestTable.[ReceivedBy]  asc";
                    orderquery1 = "order by ReceivedBy asc";
                }
                else if (filter.order == "descending/transferfrom")
                {
                    orderquery = "order by #TestTable.[ReceivedBy]  desc";
                    orderquery1 = "order by ReceivedBy desc";
                }
                else if (filter.order == "ascending/date")
                {
                    orderquery = "order by #TestTable.[TransferredDate]  asc";
                    orderquery1 = "order by TransferredDate asc";
                }
                else if (filter.order == "descending/date")
                {
                    orderquery = "order by #TestTable.[TransferredDate]  desc";
                    orderquery1 = "order by TransferredDate desc";
                }

                else
                {
                    orderquery = "order by #TestTable.[TransferredDate]  desc";
                    orderquery1 = "order by TransferredDate desc";
                }


            }
            else
            {
                orderquery = "order by #TestTable.[TransferredDate] desc";
                orderquery1 = "order by TransferredDate desc";
            }
            #endregion
            if (!string.IsNullOrWhiteSpace(filter.technician) && filter.technician != "'null'")
            {
                FilterByUserId = string.Format("And (tech.TechnicianId in ({0}))", filter.technician);
                TechQuery = string.Format("\t\t\t(select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Add' and _tech.EquipmentId = tech.EquipmentId and _tech.TechnicianId = tech.TechnicianId) \r\n                        - (select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Release' and _tech.EquipmentId = tech.EquipmentId and _tech.TechnicianId = tech.TechnicianId) \r\n                        - (select isnull(sum(Quantity),0) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  =tech.EquipmentId  and (b.TechnicianId  = tech.TechnicianId or (b.TechnicianId = '22222222-2222-2222-2222-222222222221' and b.ReceivedBy = tech.TechnicianId)) \r\n                            and b.IsApprove = 0 and b.IsDecline = 0 )\r\n                        as Quantity,");
                TruckAmount = string.Format(" ((Select ISNULL(SUM(_eqp.SupplierCost),0) from Equipment _eqp where tech.EquipmentId=_eqp.EquipmentId And tech.TechnicianId in (tech.TechnicianId)))*\r\n\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t         \t\t\t((select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Add' and _tech.EquipmentId = tech.EquipmentId and _tech.TechnicianId = tech.TechnicianId) \r\n                        - (select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Release' and _tech.EquipmentId = tech.EquipmentId and _tech.TechnicianId = tech.TechnicianId) \r\n                        - (select isnull(sum(Quantity),0) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  =tech.EquipmentId  and (b.TechnicianId  = tech.TechnicianId or (b.TechnicianId = '22222222-2222-2222-2222-222222222221' and b.ReceivedBy = tech.TechnicianId)) \r\n                            and b.IsApprove = 0 and b.IsDecline = 0 )) AmountTruck,");
                //TechQuery = string.Format("((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId in ({0}))-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId in ({0}))) Quantity,", filter.technician);
                //TruckAmount = string.Format("((Select ISNULL(SUM(_eqp.SupplierCost),0) from Equipment _eqp where _inv.EquipmentId=_eqp.EquipmentId And _inv.TechnicianId in ({0})) * ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId in ({0}))-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId in ({0})))) AmountTruck,", filter.technician);
            }
            else
            {
                TechQuery = string.Format("\t\t\t(select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Add' and _tech.EquipmentId = tech.EquipmentId and _tech.TechnicianId = tech.TechnicianId) \r\n                        - (select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Release' and _tech.EquipmentId = tech.EquipmentId and _tech.TechnicianId = tech.TechnicianId) \r\n                        - (select isnull(sum(Quantity),0) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  =tech.EquipmentId  and (b.TechnicianId  = tech.TechnicianId or (b.TechnicianId = '22222222-2222-2222-2222-222222222221' and b.ReceivedBy = tech.TechnicianId)) \r\n                            and b.IsApprove = 0 and b.IsDecline = 0 )\r\n                        as Quantity,");
                TruckAmount = string.Format(" ((Select ISNULL(SUM(_eqp.SupplierCost),0) from Equipment _eqp where tech.EquipmentId=_eqp.EquipmentId And tech.TechnicianId in (tech.TechnicianId)))*\r\n\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t         \t\t\t((select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Add' and _tech.EquipmentId = tech.EquipmentId and _tech.TechnicianId = tech.TechnicianId) \r\n                        - (select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Release' and _tech.EquipmentId = tech.EquipmentId and _tech.TechnicianId = tech.TechnicianId) \r\n                        - (select isnull(sum(Quantity),0) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  =tech.EquipmentId  and (b.TechnicianId  = tech.TechnicianId or (b.TechnicianId = '22222222-2222-2222-2222-222222222221' and b.ReceivedBy = tech.TechnicianId)) \r\n                            and b.IsApprove = 0 and b.IsDecline = 0 )) AmountTruck,");
                //TechQuery = string.Format("((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId=_inv.TechnicianId)-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId=_inv.TechnicianId)) Quantity,");
                //TruckAmount = string.Format("((Select ISNULL(SUM(_eqp.SupplierCost),0) from Equipment _eqp where _inv.EquipmentId=_eqp.EquipmentId) * ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId=_inv.TechnicianId)-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId=_inv.TechnicianId))) AmountTruck,");
            }
            if (!string.IsNullOrWhiteSpace(filter.category) && filter.category != "'null'" && filter.category != "'undefined'" && filter.category != "null" && filter.category != "undefined")
            {
                filterByEquipmentType = string.Format("and _eqpType.Id in ({0})", filter.category);
            }
            if (!string.IsNullOrWhiteSpace(filter.manufact) && filter.manufact != "'null'" && filter.manufact != "'undefined'" && filter.manufact != "null" && filter.manufact != "undefined")
            {
                filterByEquipmentClass = string.Format("and manu.ManufacturerId in ({0})", filter.manufact);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filter.CompanyId,//0
                        filterColumntext,//1
                        filtertext,//2
                        filterByActiveStatus,//3
                        filterByEquipmentClass,//4
                        filterByEquipmentType,//5
                        filterByStockStatus,//6
                        FilterByUserId,//7
                        filter.UserId,//8
                        orderquery,//9
                        orderquery1,//10
                        orderquery2,//11
                        TechQuery,//12
                        TruckAmount,//13
                        OrderingId,//14
                        ReceivedDateFilte,
                        filter.Start,
                        filter.End,
                        DateQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {

                    AddParameter(cmd, pInt32("pageno", filter.PageNo));
                    AddParameter(cmd, pInt32("pagesize", filter.PageSize));
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", filter.SearchText)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataSet GetEquipmentListByFilterTechForPendingReport(FilterEquipment filter)
        {
            string TechQuery = "";
            string TruckAmount = "";
            string DateQuery = "";
            if ((filter.Transferred_Date_From != null && filter.Transferred_Date_To != null && filter.Transferred_Date_From != new DateTime() && filter.Transferred_Date_To != new DateTime()))
            {
                DateQuery = string.Format("and _inv.ReceivedDate between '{0}' and '{1}'", filter.Transferred_Date_From, filter.Transferred_Date_To);
            }

            string sqlQuery = @"
                                    declare @pagestart int
                                    declare @pageend int
                                    set @pagestart=(@pageno-1)* @pagesize 
                                    set @pageend = @pagesize

                                    SELECT 
                                        _eqp.*,
                                        _eqpClass.Name as EquipmentClass,
	                                    {12}
                                        {13}
                                        sup.Name as SupplierName,
                                        _eqpType.Name as Category,
                                        manu.Name as ManufacturerName,
                                        emp.FirstName + ' ' + emp.LastName as TechnicianName,
                                        emp2.FirstName+ ' ' + emp2.LastName as ReceivedBy,
                                        _inv.ReceivedDate as TransferredDate,
                                        _inv.Quantity as TechQuantity,_inv.Id as _inv_Id
                                          {1}
                                          INTO #CustomerData
                                          FROM AssignedInventoryTechReceived _inv
                                          LEFT JOIN Equipment _eqp
                                            ON _eqp.EquipmentId = _inv.EquipmentId
                                            LEFT JOIN EquipmentClass _eqpClass
		                                    ON _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                                            left join Supplier sup on sup.Id = _eqp.SupplierId
                                            LEFT JOIN EquipmentType _eqpType
		                                    ON _eqp.EquipmentTypeId = _eqpType.Id and _eqp.CompanyId = _eqpType.CompanyId		                                    
                                            left join Manufacturer manu
											ON manu.Id = _eqp.ManufacturerId
                                            left join employee emp on _inv.TechnicianId = emp.UserId
                                            left join employee emp2 on _inv.ReceivedBy=emp2.UserId
                                                WHERE 
			                                    _eqp.CompanyId = '{0}'
                                                AND _inv.IsReceived=0 AND _inv.IsApprove=1
                                                --AND ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId='{8}')-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId='{8}'))>0
                                                {7}
                                                {3}
                                                {4}
                                                {5}
                                                {6}
                                               --and _inv.ReceivedDate between  '{16}' and '{17}'
                                             {18}
                                     --           {9}
                                                
                                           SELECT * INTO #CustomerFilterData
                                           FROM #CustomerData
                                           {2}

	                                       SELECT DISTINCT TOP (@pagesize) * into #TestTable
                                           FROM #CustomerFilterData _cfd
                                           where   {14} NOT IN(Select TOP (@pagestart)  {14} from #CustomerFilterData order by #CustomerFilterData.[TransferredDate]  desc )
                                           {10}
                                           
                                          
                                           select count(*) [TotalCount]
                                           from #CustomerFilterData

                                           select * from #TestTable {9}
										   select sum(SupplierCost) as TotalSupplierCost,
										   sum(TechQuantity) as TotalQuantity,
										   sum(AmountTruck) as TotalAmount from #TestTable

                                           DROP TABLE #CustomerData
                                           DROP TABLE #CustomerFilterData
                                           DROP TABLE #TestTable
										  ";

            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string FilterByUserId = "";
            string orderquery = "";
            string orderquery1 = "";
            string orderquery2 = "";
            string OrderingId = "_inv_Id"; //Id
            string ReceivedDateFilte = "";
            if (!string.IsNullOrWhiteSpace(filter.SearchText) && filter.SearchText != "-1" && filter.SearchText != "'null'" && filter.SearchText != "'undefined'" && filter.SearchText != "null" && filter.SearchText != "undefined")
            {
                if (filter.isTransferInventoryReport == true)
                {
                    filterColumntext = @",isNULL(_eqp.Name, '') +
                                isNULL(try_convert(nvarchar,_inv.Quantity),'')    FilterText  ";
                }
                else
                {
                    filterColumntext = @",isNULL(_eqp.Name, '') +
								isNULL(_eqp.SKU,'') + 
                                isNULL(emp.FirstName,'') + 
                                isNULL(emp.LastName,'') +
                                isNULL(try_convert(nvarchar,_inv.Quantity),'') + 
								isNULL(try_convert(nvarchar,_eqp.SupplierCost),'')   FilterText  ";

                }



                //filtertext = string.Format(" Where FilterText like '%{0}%' ", filter.SearchText);
                filtertext = string.Format(" Where FilterText like @SearchText ");

            }

            if (filter.Transferred_Date_From != new DateTime() && filter.Transferred_Date_To != new DateTime())
            {
                ReceivedDateFilte = string.Format("and _inv.ReceivedDate between  '{0}' and '{1}'", filter.Transferred_Date_From, filter.Transferred_Date_To);
            }
            else if (filter.Transferred_Date_From == new DateTime() && filter.Transferred_Date_To != new DateTime())
            {
                ReceivedDateFilte = string.Format("and _inv.ReceivedDate <=  '{0}' ", filter.Transferred_Date_To);
            }
            else if (filter.Transferred_Date_From != new DateTime() && filter.Transferred_Date_To == new DateTime())
            {
                ReceivedDateFilte = string.Format("and _inv.ReceivedDate >=  '{0}' ", filter.Transferred_Date_From);
            }


            #region Order
            if (!string.IsNullOrWhiteSpace(filter.order))
            {
                if (filter.order == "ascending/category")
                {
                    orderquery = "order by #TestTable.[Category] asc";
                    orderquery1 = "order by [Category] asc";
                }
                else if (filter.order == "descending/category")
                {
                    orderquery = "order by #TestTable.[Category] desc";
                    orderquery1 = "order by [Category] desc";
                }
                else if (filter.order == "ascending/manufacturer")
                {
                    orderquery = "order by #TestTable.ManufacturerName asc";
                    orderquery1 = "order by ManufacturerName asc";
                }
                else if (filter.order == "descending/manufacturer")
                {
                    orderquery = "order by #TestTable.ManufacturerName desc";
                    orderquery1 = "order by ManufacturerName desc";
                }
                else if (filter.order == "ascending/description")
                {
                    orderquery = "order by #TestTable.[Name] asc";
                    orderquery1 = "order by [Name] asc";
                }
                else if (filter.order == "descending/description")
                {
                    orderquery = "order by #TestTable.[Name] desc";
                    orderquery1 = "order by [Name] desc";
                }
                else if (filter.order == "ascending/SKU")
                {
                    orderquery = "order by #TestTable.[SKU] asc";
                    orderquery1 = "order by [SKU] asc";
                }
                else if (filter.order == "descending/SKU")
                {
                    orderquery = "order by #TestTable.[SKU] desc";
                    orderquery1 = "order by [SKU] desc";
                }
                else if (filter.order == "ascending/username")
                {
                    orderquery = "order by #TestTable.[TechnicianName] asc";
                    orderquery1 = "order by [TechnicianName] asc";
                }
                else if (filter.order == "descending/username")
                {
                    orderquery = "order by #TestTable.[TechnicianName] desc";
                    orderquery1 = "order by [TechnicianName] desc";
                }
                else if (filter.order == "ascending/vendorcost")
                {
                    orderquery = "order by #TestTable.[SupplierCost]  asc";
                    orderquery1 = "order by SupplierCost asc";
                }
                else if (filter.order == "descending/vendorcost")
                {
                    orderquery = "order by #TestTable.[SupplierCost]  desc";
                    orderquery1 = "order by SupplierCost desc";
                }
                else if (filter.order == "ascending/pieces")
                {
                    orderquery = "order by #TestTable.[TechQuantity]  asc";
                    orderquery1 = "order by TechQuantity asc";
                }
                else if (filter.order == "descending/pieces")
                {
                    orderquery = "order by #TestTable.[TechQuantity]  desc";
                    orderquery1 = "order by TechQuantity desc";
                }
                else if (filter.order == "ascending/amount")
                {
                    orderquery = "order by #TestTable.[AmountTruck]  asc";
                    orderquery1 = "order by AmountTruck asc";
                }
                else if (filter.order == "descending/amount")
                {
                    orderquery = "order by #TestTable.[AmountTruck]  desc";
                    orderquery1 = "order by AmountTruck desc";
                }
                else if (filter.order == "ascending/transferfrom")
                {
                    orderquery = "order by #TestTable.[ReceivedBy]  asc";
                    orderquery1 = "order by ReceivedBy asc";
                }
                else if (filter.order == "descending/transferfrom")
                {
                    orderquery = "order by #TestTable.[ReceivedBy]  desc";
                    orderquery1 = "order by ReceivedBy desc";
                }
                else if (filter.order == "ascending/date")
                {
                    orderquery = "order by #TestTable.[TransferredDate]  asc";
                    orderquery1 = "order by TransferredDate asc";
                }
                else if (filter.order == "descending/date")
                {
                    orderquery = "order by #TestTable.[TransferredDate]  desc";
                    orderquery1 = "order by TransferredDate desc";
                }

                else
                {
                    orderquery = "order by #TestTable.[TransferredDate]  desc";
                    orderquery1 = "order by TransferredDate desc";
                }


            }
            else
            {
                orderquery = "order by #TestTable.[TransferredDate] desc";
                orderquery1 = "order by TransferredDate desc";
            }
            #endregion
            if (!string.IsNullOrWhiteSpace(filter.technician) && filter.technician != "'null'")
            {
                FilterByUserId = string.Format("And (_inv.TechnicianId in ({0}))", filter.technician);
                TechQuery = string.Format("((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId in ({0}))-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId in ({0}))) Quantity,", filter.technician);
                TruckAmount = string.Format("((Select ISNULL(SUM(_eqp.SupplierCost),0) from Equipment _eqp where _inv.EquipmentId=_eqp.EquipmentId And _inv.TechnicianId in ({0})) * ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId in ({0}))-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId in ({0})))) AmountTruck,", filter.technician);
            }
            else
            {
                TechQuery = string.Format("((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId=_inv.TechnicianId)-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId=_inv.TechnicianId)) Quantity,");
                TruckAmount = string.Format("((Select ISNULL(SUM(_eqp.SupplierCost),0) from Equipment _eqp where _inv.EquipmentId=_eqp.EquipmentId) * ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId=_inv.TechnicianId)-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId=_inv.TechnicianId))) AmountTruck,");
            }
            if (!string.IsNullOrWhiteSpace(filter.category) && filter.category != "'null'" && filter.category != "'undefined'" && filter.category != "null" && filter.category != "undefined")
            {
                filterByEquipmentType = string.Format("and _eqpType.Id in ({0})", filter.category);
            }
            if (!string.IsNullOrWhiteSpace(filter.manufact) && filter.manufact != "'null'" && filter.manufact != "'undefined'" && filter.manufact != "null" && filter.manufact != "undefined")
            {
                filterByEquipmentClass = string.Format("and manu.ManufacturerId in ({0})", filter.manufact);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filter.CompanyId,//0
                        filterColumntext,//1
                        filtertext,//2
                        filterByActiveStatus,//3
                        filterByEquipmentClass,//4
                        filterByEquipmentType,//5
                        filterByStockStatus,//6
                        FilterByUserId,//7
                        filter.UserId,//8
                        orderquery,//9
                        orderquery1,//10
                        orderquery2,//11
                        TechQuery,//12
                        TruckAmount,//13
                        OrderingId,//14
                        ReceivedDateFilte,
                        filter.Start,
                        filter.End,
                        DateQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {

                    AddParameter(cmd, pInt32("pageno", filter.PageNo));
                    AddParameter(cmd, pInt32("pagesize", filter.PageSize));
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", filter.SearchText)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetPendingListReportByFilterTech(FilterEquipment filter)
        {
            string TechQuery = "";
            string TruckAmount = "";
            string DateQuery = "";
            if ((filter.Transferred_Date_From != null && filter.Transferred_Date_To != null && filter.Transferred_Date_From != new DateTime() && filter.Transferred_Date_To != new DateTime()))
            {
                DateQuery = string.Format("and _inv.ReceivedDate between '{0}' and '{1}'", filter.Transferred_Date_From, filter.Transferred_Date_To);
            }
            string sqlQuery = @"";
            if (!string.IsNullOrWhiteSpace(filter.RepType) && filter.RepType == "Pending")
            {
                sqlQuery = @"

                                    SELECT _inv.ReceivedDate,
                                        _eqpType.Name as Category,
                                        manu.Name as Manufacturer,
                                        _eqp.Name as Description,
                                        _eqp.SKU,
                                        emp.FirstName + ' ' + emp.LastName as [To], 
                                        emp2.FirstName+ ' ' + emp2.LastName as [From],
                                        convert(date,_inv.CreatedDate) as CreatedDate,
                                        _inv.Quantity as Quantity
                                      
                                          FROM AssignedInventoryTechReceived _inv
                                          LEFT JOIN Equipment _eqp
                                            ON _eqp.EquipmentId = _inv.EquipmentId
                                            LEFT JOIN EquipmentClass _eqpClass
		                                    ON _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                                            left join Supplier sup on sup.Id = _eqp.SupplierId
                                            LEFT JOIN EquipmentType _eqpType
		                                    ON _eqp.EquipmentTypeId = _eqpType.Id and _eqp.CompanyId = _eqpType.CompanyId		                                    
                                            left join Manufacturer manu
											ON manu.Id = _eqp.ManufacturerId
                                            left join employee emp on _inv.TechnicianId = emp.UserId
                                            left join employee emp2 on _inv.ReceivedBy = emp2.UserId
                                                WHERE 
			                                    _eqp.CompanyId = '{0}'
                                                AND _inv.IsReceived=0 AND _inv.IsApprove=1
                                                --AND ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId='{8}')-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId='{8}'))>0
                                                {7}
                                                {2}
                                                {3}
                                                {4}
                                                {5}
                                                {6}
                                                {14}
                                                {9}
                                               --{15}
                                           ";
            }
            else
            {
                sqlQuery = @"
                                      SELECT _inv.ReceivedDate,
                                        emp.FirstName + ' ' + emp.LastName as TransferTo, 
                                        emp2.FirstName+ ' ' + emp2.LastName as TransferFrom,
                                        _inv.ReceivedDate as TransferredDate,
                                        _eqp.Name as Description,
                                        _eqpType.Name as Category,
                                        manu.Name as Manufacturer,
                                        _eqp.SKU,
                                        _inv.Quantity as Quantity
                                          FROM AssignedInventoryTechReceived _inv
                                          LEFT JOIN Equipment _eqp
                                            ON _eqp.EquipmentId = _inv.EquipmentId
                                            LEFT JOIN EquipmentClass _eqpClass
		                                    ON _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                                            left join Supplier sup on sup.Id = _eqp.SupplierId
                                            LEFT JOIN EquipmentType _eqpType
		                                    ON _eqp.EquipmentTypeId = _eqpType.Id and _eqp.CompanyId = _eqpType.CompanyId		                                    
                                            left join Manufacturer manu
											ON manu.Id = _eqp.ManufacturerId
                                            left join employee emp on _inv.TechnicianId = emp.UserId
                                                WHERE 
			                                    _eqp.CompanyId = '{0}'
                                                AND _inv.IsReceived=0 AND _inv.IsApprove=1
                                                --AND ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId='{8}')-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId='{8}'))>0
                                                {7}
                                                {2}
                                                {3}
                                                {4}
                                                {5}
                                                {6}
                                                {14}
                                                {9}
                                                --{15}
                                                
                                           ";
            }


            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string FilterByUserId = "";
            string orderquery = "";
            string orderquery1 = "";
            string orderquery2 = "";
            string ReceivedDateFilte = "";
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                filterColumntext = @",isNULL(_eqp.Name, '') +
								isNULL(_eqp.SKU,'') + 
                                isNULL(_eqpType.Name,'') + 
                                isNULL(sup.CompanyName,'') +
                                isNULL(manu.Name,'') + 
								isNULL(_eqpClass.Name,'')  FilterText  ";
                filtertext = string.Format("and (_eqp.Name  like '%{0}%' or _inv.Quantity  like '%{0}%')", filter.SearchText);
            }

            if (filter.Transferred_Date_From != new DateTime() && filter.Transferred_Date_To != new DateTime())
            {
                ReceivedDateFilte = string.Format("and _inv.ReceivedDate between  '{0}' and '{1}'", filter.Transferred_Date_From, filter.Transferred_Date_To);
            }
            else if (filter.Transferred_Date_From == new DateTime() && filter.Transferred_Date_To != new DateTime())
            {
                ReceivedDateFilte = string.Format("and _inv.ReceivedDate <=  '{0}' ", filter.Transferred_Date_To);
            }
            else if (filter.Transferred_Date_From != new DateTime() && filter.Transferred_Date_To == new DateTime())
            {
                ReceivedDateFilte = string.Format("and _inv.ReceivedDate >=  '{0}' ", filter.Transferred_Date_From);
            }



            if (!string.IsNullOrWhiteSpace(filter.order))
            {
                if (filter.order == "ascending/category")
                {
                    orderquery = "order by _eqpType.Name asc";
                    orderquery1 = "order by _cd.Category asc";
                    orderquery2 = "order by _cfd.Category asc";
                }
                else if (filter.order == "descending/category")
                {
                    orderquery = "order by _eqpType.Name desc";
                    orderquery1 = "order by _cd.Category desc";
                    orderquery2 = "order by _cfd.Category desc";
                }
                else if (filter.order == "ascending/monthlyfee")
                {
                    orderquery = "order by _eqp.Retail asc";
                    orderquery1 = "order by _cd.Retail asc";
                    orderquery2 = "order by _cfd.Retail asc";
                }
                else if (filter.order == "descending/monthlyfee")
                {
                    orderquery = "order by _eqp.Retail desc";
                    orderquery1 = "order by _cd.Retail desc";
                    orderquery2 = "order by _cfd.Retail desc";
                }
                else if (filter.order == "ascending/description")
                {
                    orderquery = "order by _eqp.Comments asc";
                    orderquery1 = "order by _cd.Comments asc";
                    orderquery2 = "order by _cfd.Comments asc";
                }
                else if (filter.order == "descending/description")
                {
                    orderquery = "order by _eqp.Comments desc";
                    orderquery1 = "order by _cd.Comments desc";
                    orderquery2 = "order by _cfd.Comments desc";
                }
                else if (filter.order == "ascending/manu")
                {
                    orderquery = "order by manu.Name asc";
                    orderquery1 = "order by _cd.ManufacturerName asc";
                    orderquery2 = "order by _cfd.ManufacturerName asc";
                }
                else if (filter.order == "descending/manu")
                {
                    orderquery = "order by manu.Name desc";
                    orderquery1 = "order by _cd.ManufacturerName desc";
                    orderquery2 = "order by _cfd.ManufacturerName desc";
                }
                else if (filter.order == "ascending/des")
                {
                    orderquery = "order by _eqp.Name asc";
                    orderquery1 = "order by _cd.Name asc";
                    orderquery2 = "order by _cfd.Name asc";
                }
                else if (filter.order == "descending/des")
                {
                    orderquery = "order by _eqp.Name desc";
                    orderquery1 = "order by _cd.Name desc";
                    orderquery2 = "order by _cfd.Name desc";
                }
                else if (filter.order == "ascending/sku")
                {
                    orderquery = "order by _eqp.SKU asc";
                    orderquery1 = "order by _cd.SKU asc";
                    orderquery2 = "order by _cfd.SKU asc";
                }
                else if (filter.order == "descending/sku")
                {
                    orderquery = "order by _eqp.SKU desc";
                    orderquery1 = "order by _cd.SKU desc";
                    orderquery2 = "order by _cfd.SKU desc";
                }
                else if (filter.order == "ascending/vendor")
                {
                    orderquery = "order by sup.CompanyName asc";
                    orderquery1 = "order by _cd.SupplierName asc";
                    orderquery2 = "order by _cfd.SupplierName asc";
                }
                else if (filter.order == "descending/vendor")
                {
                    orderquery = "order by sup.CompanyName desc";
                    orderquery1 = "order by _cd.SupplierName desc";
                    orderquery2 = "order by _cfd.SupplierName desc";
                }
                else if (filter.order == "ascending/vendorcost")
                {
                    orderquery = "order by _eqp.SupplierCost asc";
                    orderquery1 = "order by _cd.SupplierCost asc";
                    orderquery2 = "order by _cfd.SupplierCost asc";
                }
                else if (filter.order == "descending/vendorcost")
                {
                    orderquery = "order by _eqp.SupplierCost desc";
                    orderquery1 = "order by _cd.SupplierCost desc";
                    orderquery2 = "order by _cfd.SupplierCost desc";
                }
                else if (filter.order == "ascending/type")
                {
                    orderquery = "order by _eqp.Id asc";
                    orderquery1 = "order by _cd.Id asc";
                    orderquery2 = "order by _cfd.Id asc";
                }
                else if (filter.order == "descending/type")
                {
                    orderquery = "order by _eqp.Id desc";
                    orderquery1 = "order by _cd.Id desc";
                    orderquery2 = "order by _cfd.Id desc";
                }
                else if (filter.order == "ascending/qty")
                {
                    orderquery = "order by Quantity asc";
                    orderquery1 = "order by _cd.Quantity asc";
                    orderquery2 = "order by _cfd.Quantity asc";
                }
                else if (filter.order == "descending/qty")
                {
                    orderquery = "order by Quantity desc";
                    orderquery1 = "order by _cd.Quantity desc";
                    orderquery2 = "order by _cfd.Quantity desc";
                }
                else if (filter.order == "ascending/rack")
                {
                    orderquery = "order by _eqp.RackNo asc";
                    orderquery1 = "order by _cd.RackNo asc";
                    orderquery2 = "order by _cfd.RackNo asc";
                }
                else if (filter.order == "descending/rack")
                {
                    orderquery = "order by _eqp.RackNo desc";
                    orderquery1 = "order by _cd.RackNo desc";
                    orderquery2 = "order by _cfd.RackNo desc";
                }
            }
            else
            {
                orderquery = "order by _inv.ReceivedDate desc";
                orderquery1 = "order by _cd.Id desc";
                orderquery2 = "order by _cfd.Id desc";
            }
            if (!string.IsNullOrWhiteSpace(filter.technician) && filter.technician != "'null'" && filter.technician != "'undefined'" && filter.technician != "null" && filter.technician != "undefined")
            {
                FilterByUserId = string.Format("And (_inv.TechnicianId in ({0}) or _inv.ReceivedBy in ({0}))", filter.technician);
                TechQuery = string.Format("((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId in ({0}))-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId in ({0}))) Quantity,", filter.technician);
                TruckAmount = string.Format("((Select ISNULL(SUM(_eqp.SupplierCost),0) from Equipment _eqp where _inv.EquipmentId=_eqp.EquipmentId And _inv.TechnicianId in ({0})) * ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId in ({0}))-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId in ({0})))) Amount", filter.technician);
            }
            else
            {
                TechQuery = string.Format("((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId=_inv.TechnicianId)-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId=_inv.TechnicianId)) Quantity,");
                TruckAmount = string.Format("((Select ISNULL(SUM(_eqp.SupplierCost),0) from Equipment _eqp where _inv.EquipmentId=_eqp.EquipmentId) * ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId=_inv.TechnicianId)-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId=_inv.TechnicianId))) Amount");
            }
            if (!string.IsNullOrWhiteSpace(filter.category) && filter.category != "'null'" && filter.category != "'undefined'" && filter.category != "null" && filter.category != "undefined")
            {
                filterByEquipmentType = string.Format("and _eqpType.Id in ({0})", filter.category);
            }
            if (!string.IsNullOrWhiteSpace(filter.manufact) && filter.manufact != "'null'" && filter.manufact != "'undefined'" && filter.manufact != "null" && filter.manufact != "undefined")
            {
                filterByEquipmentClass = string.Format("and manu.ManufacturerId in ({0})", filter.manufact);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filter.CompanyId,//0
                        filterColumntext,//1
                        filtertext,//2
                        filterByActiveStatus,//3
                        filterByEquipmentClass,//4
                        filterByEquipmentType,//5
                        filterByStockStatus,//6
                        FilterByUserId,//7
                        filter.UserId,//8
                        orderquery,//9
                        orderquery1,//10
                        orderquery2,//11
                        TechQuery,//12
                        TruckAmount,//13
                        ReceivedDateFilte,
                        DateQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", filter.PageNo));
                    AddParameter(cmd, pInt32("pagesize", filter.PageSize));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetTranferListReportByFilterTech(FilterEquipment filter)
        {
            string TechQuery = "";
            string TruckAmount = "";
            string DateQuery = "";
            if ((filter.Transferred_Date_From != null && filter.Transferred_Date_To != null && filter.Transferred_Date_From != new DateTime() && filter.Transferred_Date_To != new DateTime()))
            {
                DateQuery = string.Format("and _inv.ReceivedDate between '{0}' and '{1}'", filter.Transferred_Date_From, filter.Transferred_Date_To);
            }
            string sqlQuery = @"";
            if (!string.IsNullOrWhiteSpace(filter.RepType) && filter.RepType == "Transfer")
            {
                sqlQuery = @"

                                    SELECT _inv.ReceivedDate,
                                        _eqpType.Name as Category,
                                        manu.Name as Manufacturer,
                                        _eqp.Name as Description,
                                        _eqp.SKU,
                                        emp.FirstName + ' ' + emp.LastName as TransferTo, 
                                        emp2.FirstName+ ' ' + emp2.LastName as TransferFrom,
                                        convert(date,_inv.ReceivedDate) as TransferredDate,
                                        _inv.Quantity as Quantity
                                      
                                          FROM AssignedInventoryTechReceived _inv
                                          LEFT JOIN Equipment _eqp
                                            ON _eqp.EquipmentId = _inv.EquipmentId
                                            LEFT JOIN EquipmentClass _eqpClass
		                                    ON _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                                            left join Supplier sup on sup.Id = _eqp.SupplierId
                                            LEFT JOIN EquipmentType _eqpType
		                                    ON _eqp.EquipmentTypeId = _eqpType.Id and _eqp.CompanyId = _eqpType.CompanyId		                                    
                                            left join Manufacturer manu
											ON manu.Id = _eqp.ManufacturerId
                                            left join employee emp on _inv.TechnicianId = emp.UserId
                                            left join employee emp2 on _inv.ReceivedBy = emp2.UserId
                                                WHERE 
			                                    _eqp.CompanyId = '{0}'
                                                AND _inv.IsReceived=1 AND _inv.IsApprove=1
                                                --AND ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId='{8}')-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId='{8}'))>0
                                                {7}
                                                {2}
                                                {3}
                                                {4}
                                                {5}
                                                {6}
                                                {14}
                                                {9}
                                               --{15}
                                           ";
            }
            else
            {
                sqlQuery = @"
                                      SELECT _inv.ReceivedDate,
                                        emp.FirstName + ' ' + emp.LastName as TransferTo, 
                                        emp2.FirstName+ ' ' + emp2.LastName as TransferFrom,
                                        _inv.ReceivedDate as TransferredDate,
                                        _eqp.Name as Description,
                                        _eqpType.Name as Category,
                                        manu.Name as Manufacturer,
                                        _eqp.SKU,
                                        _inv.Quantity as Quantity
                                          FROM AssignedInventoryTechReceived _inv
                                          LEFT JOIN Equipment _eqp
                                            ON _eqp.EquipmentId = _inv.EquipmentId
                                            LEFT JOIN EquipmentClass _eqpClass
		                                    ON _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                                            left join Supplier sup on sup.Id = _eqp.SupplierId
                                            LEFT JOIN EquipmentType _eqpType
		                                    ON _eqp.EquipmentTypeId = _eqpType.Id and _eqp.CompanyId = _eqpType.CompanyId		                                    
                                            left join Manufacturer manu
											ON manu.Id = _eqp.ManufacturerId
                                            left join employee emp on _inv.TechnicianId = emp.UserId
                                                WHERE 
			                                    _eqp.CompanyId = '{0}'
                                                AND _inv.IsReceived=1 AND _inv.IsApprove=1
                                                --AND ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId='{8}')-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId='{8}'))>0
                                                {7}
                                                {2}
                                                {3}
                                                {4}
                                                {5}
                                                {6}
                                                {14}
                                                {9}
                                                --{15}
                                                
                                           ";
            }


            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string FilterByUserId = "";
            string orderquery = "";
            string orderquery1 = "";
            string orderquery2 = "";
            string ReceivedDateFilte = "";
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                filterColumntext = @",isNULL(_eqp.Name, '') +
								isNULL(_eqp.SKU,'') + 
                                isNULL(_eqpType.Name,'') + 
                                isNULL(sup.CompanyName,'') +
                                isNULL(manu.Name,'') + 
								isNULL(_eqpClass.Name,'')  FilterText  ";
                filtertext = string.Format("and (_eqp.Name  like '%{0}%' or _inv.Quantity  like '%{0}%')", filter.SearchText);
            }

            if (filter.Transferred_Date_From != new DateTime() && filter.Transferred_Date_To != new DateTime())
            {
                ReceivedDateFilte = string.Format("and _inv.ReceivedDate between  '{0}' and '{1}'", filter.Transferred_Date_From, filter.Transferred_Date_To);
            }
            else if (filter.Transferred_Date_From == new DateTime() && filter.Transferred_Date_To != new DateTime())
            {
                ReceivedDateFilte = string.Format("and _inv.ReceivedDate <=  '{0}' ", filter.Transferred_Date_To);
            }
            else if (filter.Transferred_Date_From != new DateTime() && filter.Transferred_Date_To == new DateTime())
            {
                ReceivedDateFilte = string.Format("and _inv.ReceivedDate >=  '{0}' ", filter.Transferred_Date_From);
            }



            if (!string.IsNullOrWhiteSpace(filter.order))
            {
                if (filter.order == "ascending/category")
                {
                    orderquery = "order by _eqpType.Name asc";
                    orderquery1 = "order by _cd.Category asc";
                    orderquery2 = "order by _cfd.Category asc";
                }
                else if (filter.order == "descending/category")
                {
                    orderquery = "order by _eqpType.Name desc";
                    orderquery1 = "order by _cd.Category desc";
                    orderquery2 = "order by _cfd.Category desc";
                }
                else if (filter.order == "ascending/monthlyfee")
                {
                    orderquery = "order by _eqp.Retail asc";
                    orderquery1 = "order by _cd.Retail asc";
                    orderquery2 = "order by _cfd.Retail asc";
                }
                else if (filter.order == "descending/monthlyfee")
                {
                    orderquery = "order by _eqp.Retail desc";
                    orderquery1 = "order by _cd.Retail desc";
                    orderquery2 = "order by _cfd.Retail desc";
                }
                else if (filter.order == "ascending/description")
                {
                    orderquery = "order by _eqp.Comments asc";
                    orderquery1 = "order by _cd.Comments asc";
                    orderquery2 = "order by _cfd.Comments asc";
                }
                else if (filter.order == "descending/description")
                {
                    orderquery = "order by _eqp.Comments desc";
                    orderquery1 = "order by _cd.Comments desc";
                    orderquery2 = "order by _cfd.Comments desc";
                }
                else if (filter.order == "ascending/manu")
                {
                    orderquery = "order by manu.Name asc";
                    orderquery1 = "order by _cd.ManufacturerName asc";
                    orderquery2 = "order by _cfd.ManufacturerName asc";
                }
                else if (filter.order == "descending/manu")
                {
                    orderquery = "order by manu.Name desc";
                    orderquery1 = "order by _cd.ManufacturerName desc";
                    orderquery2 = "order by _cfd.ManufacturerName desc";
                }
                else if (filter.order == "ascending/des")
                {
                    orderquery = "order by _eqp.Name asc";
                    orderquery1 = "order by _cd.Name asc";
                    orderquery2 = "order by _cfd.Name asc";
                }
                else if (filter.order == "descending/des")
                {
                    orderquery = "order by _eqp.Name desc";
                    orderquery1 = "order by _cd.Name desc";
                    orderquery2 = "order by _cfd.Name desc";
                }
                else if (filter.order == "ascending/sku")
                {
                    orderquery = "order by _eqp.SKU asc";
                    orderquery1 = "order by _cd.SKU asc";
                    orderquery2 = "order by _cfd.SKU asc";
                }
                else if (filter.order == "descending/sku")
                {
                    orderquery = "order by _eqp.SKU desc";
                    orderquery1 = "order by _cd.SKU desc";
                    orderquery2 = "order by _cfd.SKU desc";
                }
                else if (filter.order == "ascending/vendor")
                {
                    orderquery = "order by sup.CompanyName asc";
                    orderquery1 = "order by _cd.SupplierName asc";
                    orderquery2 = "order by _cfd.SupplierName asc";
                }
                else if (filter.order == "descending/vendor")
                {
                    orderquery = "order by sup.CompanyName desc";
                    orderquery1 = "order by _cd.SupplierName desc";
                    orderquery2 = "order by _cfd.SupplierName desc";
                }
                else if (filter.order == "ascending/vendorcost")
                {
                    orderquery = "order by _eqp.SupplierCost asc";
                    orderquery1 = "order by _cd.SupplierCost asc";
                    orderquery2 = "order by _cfd.SupplierCost asc";
                }
                else if (filter.order == "descending/vendorcost")
                {
                    orderquery = "order by _eqp.SupplierCost desc";
                    orderquery1 = "order by _cd.SupplierCost desc";
                    orderquery2 = "order by _cfd.SupplierCost desc";
                }
                else if (filter.order == "ascending/type")
                {
                    orderquery = "order by _eqp.Id asc";
                    orderquery1 = "order by _cd.Id asc";
                    orderquery2 = "order by _cfd.Id asc";
                }
                else if (filter.order == "descending/type")
                {
                    orderquery = "order by _eqp.Id desc";
                    orderquery1 = "order by _cd.Id desc";
                    orderquery2 = "order by _cfd.Id desc";
                }
                else if (filter.order == "ascending/qty")
                {
                    orderquery = "order by Quantity asc";
                    orderquery1 = "order by _cd.Quantity asc";
                    orderquery2 = "order by _cfd.Quantity asc";
                }
                else if (filter.order == "descending/qty")
                {
                    orderquery = "order by Quantity desc";
                    orderquery1 = "order by _cd.Quantity desc";
                    orderquery2 = "order by _cfd.Quantity desc";
                }
                else if (filter.order == "ascending/rack")
                {
                    orderquery = "order by _eqp.RackNo asc";
                    orderquery1 = "order by _cd.RackNo asc";
                    orderquery2 = "order by _cfd.RackNo asc";
                }
                else if (filter.order == "descending/rack")
                {
                    orderquery = "order by _eqp.RackNo desc";
                    orderquery1 = "order by _cd.RackNo desc";
                    orderquery2 = "order by _cfd.RackNo desc";
                }
            }
            else
            {
                orderquery = "order by _inv.ReceivedDate desc";
                orderquery1 = "order by _cd.Id desc";
                orderquery2 = "order by _cfd.Id desc";
            }
            if (!string.IsNullOrWhiteSpace(filter.technician) && filter.technician != "'null'" && filter.technician != "'undefined'" && filter.technician != "null" && filter.technician != "undefined")
            {
                FilterByUserId = string.Format("And (_inv.TechnicianId in ({0}) or _inv.ReceivedBy in ({0}))", filter.technician);
                TechQuery = string.Format("((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId in ({0}))-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId in ({0}))) Quantity,", filter.technician);
                TruckAmount = string.Format("((Select ISNULL(SUM(_eqp.SupplierCost),0) from Equipment _eqp where _inv.EquipmentId=_eqp.EquipmentId And _inv.TechnicianId in ({0})) * ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId in ({0}))-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId in ({0})))) Amount", filter.technician);
            }
            else
            {
                TechQuery = string.Format("((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId=_inv.TechnicianId)-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId=_inv.TechnicianId)) Quantity,");
                TruckAmount = string.Format("((Select ISNULL(SUM(_eqp.SupplierCost),0) from Equipment _eqp where _inv.EquipmentId=_eqp.EquipmentId) * ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId=_inv.TechnicianId)-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId=_inv.TechnicianId))) Amount");
            }
            if (!string.IsNullOrWhiteSpace(filter.category) && filter.category != "'null'" && filter.category != "'undefined'" && filter.category != "null" && filter.category != "undefined")
            {
                filterByEquipmentType = string.Format("and _eqpType.Id in ({0})", filter.category);
            }
            if (!string.IsNullOrWhiteSpace(filter.manufact) && filter.manufact != "'null'" && filter.manufact != "'undefined'" && filter.manufact != "null" && filter.manufact != "undefined")
            {
                filterByEquipmentClass = string.Format("and manu.ManufacturerId in ({0})", filter.manufact);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filter.CompanyId,//0
                        filterColumntext,//1
                        filtertext,//2
                        filterByActiveStatus,//3
                        filterByEquipmentClass,//4
                        filterByEquipmentType,//5
                        filterByStockStatus,//6
                        FilterByUserId,//7
                        filter.UserId,//8
                        orderquery,//9
                        orderquery1,//10
                        orderquery2,//11
                        TechQuery,//12
                        TruckAmount,//13
                        ReceivedDateFilte,
                        DateQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", filter.PageNo));
                    AddParameter(cmd, pInt32("pagesize", filter.PageSize));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetEquipmentListReportByFilterTech(FilterEquipment filter)
        {
            string TechQuery = "";
            string TruckAmount = "";
            string sqlQuery = @"";
            string DateQuery = "";
            if ((filter.Transferred_Date_From != null && filter.Transferred_Date_To != null && filter.Transferred_Date_From != new DateTime() && filter.Transferred_Date_To != new DateTime()))
            {
                DateQuery = string.Format("and LastUpdatedDate between '{0}' and '{1}'", filter.Transferred_Date_From, filter.Transferred_Date_To);
            }
            if (!string.IsNullOrWhiteSpace(filter.RepType) && filter.RepType == "Transfer")
            {
                sqlQuery = @"

                                    SELECT 
                                        _inv.ReceivedDate,
                                        _eqpType.Name as Category,
                                        manu.Name as Manufacturer,
                                        _eqp.Name as Description,
                                        _eqp.SKU as SKU,
                                        emp.FirstName + ' ' + emp.LastName as [User Name],
                                        _inv.Quantity as [No Of Pieces],
                                        _eqp.SupplierCost as [Vendor Cost],
                                         {13}
                                          FROM AssignedInventoryTechReceived _inv
                                          LEFT JOIN Equipment _eqp
                                            ON _eqp.EquipmentId = _inv.EquipmentId
                                            LEFT JOIN EquipmentClass _eqpClass
		                                    ON _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                                            left join Supplier sup on sup.Id = _eqp.SupplierId
                                            LEFT JOIN EquipmentType _eqpType
		                                    ON _eqp.EquipmentTypeId = _eqpType.Id and _eqp.CompanyId = _eqpType.CompanyId		                                    
                                            left join Manufacturer manu
											ON manu.Id = _eqp.ManufacturerId
                                            left join employee emp on _inv.TechnicianId = emp.UserId
                                            left join employee emp2 on _inv.ReceivedBy = emp2.UserId
                                                WHERE 
			                                    _eqp.CompanyId = '{0}'
                                                AND _inv.IsReceived=1 AND _inv.IsApprove=1
                                                --AND ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId='{8}')-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId='{8}'))>0
                                        --       and _inv.ReceivedDate between  '{14}' and '{15}'
                                                  {16}
                                                {1}
                                                {7}
                                                {3}
                                                {4}
                                                {5}
                                                {6}
                                                {9}
                                                
                                           ";
            }
            else
            {
                sqlQuery = @"
                                  ;WITH CTE AS (
                                       SELECT 
                                        tech.LastUpdatedDate,
                                        _eqpType.Name as Category,
                                        manu.Name as Manufacturer,
                                        _eqp.Name as Description,
                                        _eqp.SKU as SKU,
                                        emp.FirstName + ' ' + emp.LastName as [User Name],
                                        ---_inv.Quantity as [No Of Pieces],
                       (select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Add' and _tech.EquipmentId = tech.EquipmentId and _tech.TechnicianId = tech.TechnicianId) 
                        - (select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Release' and _tech.EquipmentId = tech.EquipmentId and _tech.TechnicianId = tech.TechnicianId) 
                        - (select isnull(sum(Quantity),0) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  =tech.EquipmentId  and (b.TechnicianId  = tech.TechnicianId or (b.TechnicianId = '22222222-2222-2222-2222-222222222221' and b.ReceivedBy = tech.TechnicianId)) 
                            and b.IsApprove = 0 and b.IsDecline = 0 )
                        as [No Of Pieces],
                                        _eqp.SupplierCost as [Vendor Cost],
                                         {13}
                                                {1}

							        ROW_NUMBER() OVER (PARTITION BY tech.EquipmentId, tech.TechnicianId ORDER BY tech.LastUpdatedDate DESC) AS RowNum

                                          FROM inventorytech tech
                                          LEFT JOIN Equipment _eqp
                                            ON _eqp.EquipmentId = tech.EquipmentId
                                            LEFT JOIN EquipmentClass _eqpClass
		                                    ON _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                                            left join Supplier sup on sup.Id = _eqp.SupplierId
                                            LEFT JOIN EquipmentType _eqpType
		                                    ON _eqp.EquipmentTypeId = _eqpType.Id and _eqp.CompanyId = _eqpType.CompanyId		                                    
                                            left join Manufacturer manu
											ON manu.Id = _eqp.ManufacturerId
                                            left join employee emp on tech.TechnicianId = emp.UserId
                                                WHERE 
			                                    _eqp.CompanyId = '{0}' AND tech.TechnicianId not in ( '22222222-2222-2222-2222-222222222223','22222222-2222-2222-2222-222222222222','22222222-2222-2222-2222-222222222224',
	                               '22222222-2222-2222-2222-222222222225','22222222-2222-2222-2222-222222222226','22222222-2222-2222-2222-222222222231','22222222-2222-2222-2222-222222222232',
	                              '22222222-2222-2222-2222-222222222233')
                                                --AND _inv.IsReceived=1 AND _inv.IsApprove=1
                                                --AND ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId='{8}')-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId='{8}'))>0
                                --             and tech.LastUpdatedDate between  '{14}' and '{15}'
                                                         
                                                --  {16}
                                                {7}
                                                {3}
                                                {4}
                                                {5}
                                                {6}
                                                {9}
                                                 )
    SELECT
        LastUpdatedDate,
        Category,
        Manufacturer,
        Description,
        SKU,
        [User Name],
        [No Of Pieces],
        [Vendor Cost],
        AmountTruck
    FROM CTE
 WHERE RowNum = 1 and [No Of Pieces]>0
 --{16}
 {2}
     ORDER BY LastUpdatedDate DESC                                       
                                                      ";
            }


            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string FilterByUserId = "";
            string orderquery = "";
            string orderquery1 = "";
            string orderquery2 = "";

            //if (!string.IsNullOrWhiteSpace(filter.SearchText))
            //{


            //    //   filterColumntext = string.Format(@"and (_eqp.Name like '%{0}%'
            //    //or _eqp.SKU  like '%{0}%'
            //    //                        or emp.FirstName  like '%{0}%'
            //    //                        or emp.LastName like '%{0}%'
            //    //                        or _inv.Quantity like '%{0}%' 
            //    //or _eqp.SupplierCost like '%{0}%') ", filter.SearchText);
            //    filterColumntext = string.Format(@"and (_eqp.Name like @SearchText
								    // or _eqp.SKU  like @SearchText
            //                         or emp.FirstName  like @SearchText
            //                         or emp.LastName like @SearchText
            //                         or _inv.Quantity like @SearchText 
								    // or _eqp.SupplierCost like @SearchText) ", filter.SearchText);
            //    filtertext = string.Format(" Where FilterText like '%{0}%' ", filter.SearchText);
            //}
            if (!string.IsNullOrWhiteSpace(filter.SearchText) && filter.SearchText != "-1" && filter.SearchText != "'null'" && filter.SearchText != "'undefined'" && filter.SearchText != "null" && filter.SearchText != "undefined")
            {
                
                {
                    filterColumntext = @"isNULL(_eqp.Name, '') +
								isNULL(_eqp.SKU,'') + 
                                isNULL(emp.FirstName,'') + 
                                isNULL(emp.LastName,'') +
                                isNULL(try_convert(nvarchar,tech.Quantity),'') + 
								isNULL(try_convert(nvarchar,_eqp.SupplierCost),'')   FilterText,  ";

                }



                //filtertext = string.Format(" Where FilterText like '%{0}%' ", filter.SearchText);
                filtertext = string.Format(" and FilterText like @SearchText ");

            }
            if (!string.IsNullOrWhiteSpace(filter.order))
            {
                if (filter.order == "ascending/category")
                {
                    orderquery = "order by _eqpType.Name asc";
                    orderquery1 = "order by _cd.Category asc";
                    orderquery2 = "order by _cfd.Category asc";
                }
                else if (filter.order == "descending/category")
                {
                    orderquery = "order by _eqpType.Name desc";
                    orderquery1 = "order by _cd.Category desc";
                    orderquery2 = "order by _cfd.Category desc";
                }
                else if (filter.order == "ascending/monthlyfee")
                {
                    orderquery = "order by _eqp.Retail asc";
                    orderquery1 = "order by _cd.Retail asc";
                    orderquery2 = "order by _cfd.Retail asc";
                }
                else if (filter.order == "descending/monthlyfee")
                {
                    orderquery = "order by _eqp.Retail desc";
                    orderquery1 = "order by _cd.Retail desc";
                    orderquery2 = "order by _cfd.Retail desc";
                }
                else if (filter.order == "ascending/description")
                {
                    orderquery = "order by _eqp.Comments asc";
                    orderquery1 = "order by _cd.Comments asc";
                    orderquery2 = "order by _cfd.Comments asc";
                }
                else if (filter.order == "descending/description")
                {
                    orderquery = "order by _eqp.Comments desc";
                    orderquery1 = "order by _cd.Comments desc";
                    orderquery2 = "order by _cfd.Comments desc";
                }
                else if (filter.order == "ascending/manu")
                {
                    orderquery = "order by manu.Name asc";
                    orderquery1 = "order by _cd.ManufacturerName asc";
                    orderquery2 = "order by _cfd.ManufacturerName asc";
                }
                else if (filter.order == "descending/manu")
                {
                    orderquery = "order by manu.Name desc";
                    orderquery1 = "order by _cd.ManufacturerName desc";
                    orderquery2 = "order by _cfd.ManufacturerName desc";
                }
                else if (filter.order == "ascending/des")
                {
                    orderquery = "order by _eqp.Name asc";
                    orderquery1 = "order by _cd.Name asc";
                    orderquery2 = "order by _cfd.Name asc";
                }
                else if (filter.order == "descending/des")
                {
                    orderquery = "order by _eqp.Name desc";
                    orderquery1 = "order by _cd.Name desc";
                    orderquery2 = "order by _cfd.Name desc";
                }
                else if (filter.order == "ascending/sku")
                {
                    orderquery = "order by _eqp.SKU asc";
                    orderquery1 = "order by _cd.SKU asc";
                    orderquery2 = "order by _cfd.SKU asc";
                }
                else if (filter.order == "descending/sku")
                {
                    orderquery = "order by _eqp.SKU desc";
                    orderquery1 = "order by _cd.SKU desc";
                    orderquery2 = "order by _cfd.SKU desc";
                }
                else if (filter.order == "ascending/vendor")
                {
                    orderquery = "order by sup.CompanyName asc";
                    orderquery1 = "order by _cd.SupplierName asc";
                    orderquery2 = "order by _cfd.SupplierName asc";
                }
                else if (filter.order == "descending/vendor")
                {
                    orderquery = "order by sup.CompanyName desc";
                    orderquery1 = "order by _cd.SupplierName desc";
                    orderquery2 = "order by _cfd.SupplierName desc";
                }
                else if (filter.order == "ascending/vendorcost")
                {
                    orderquery = "order by _eqp.SupplierCost asc";
                    orderquery1 = "order by _cd.SupplierCost asc";
                    orderquery2 = "order by _cfd.SupplierCost asc";
                }
                else if (filter.order == "descending/vendorcost")
                {
                    orderquery = "order by _eqp.SupplierCost desc";
                    orderquery1 = "order by _cd.SupplierCost desc";
                    orderquery2 = "order by _cfd.SupplierCost desc";
                }
                else if (filter.order == "ascending/type")
                {
                    orderquery = "order by _eqp.Id asc";
                    orderquery1 = "order by _cd.Id asc";
                    orderquery2 = "order by _cfd.Id asc";
                }
                else if (filter.order == "descending/type")
                {
                    orderquery = "order by _eqp.Id desc";
                    orderquery1 = "order by _cd.Id desc";
                    orderquery2 = "order by _cfd.Id desc";
                }
                else if (filter.order == "ascending/qty")
                {
                    orderquery = "order by Quantity asc";
                    orderquery1 = "order by _cd.Quantity asc";
                    orderquery2 = "order by _cfd.Quantity asc";
                }
                else if (filter.order == "descending/qty")
                {
                    orderquery = "order by Quantity desc";
                    orderquery1 = "order by _cd.Quantity desc";
                    orderquery2 = "order by _cfd.Quantity desc";
                }
                else if (filter.order == "ascending/rack")
                {
                    orderquery = "order by _eqp.RackNo asc";
                    orderquery1 = "order by _cd.RackNo asc";
                    orderquery2 = "order by _cfd.RackNo asc";
                }
                else if (filter.order == "descending/rack")
                {
                    orderquery = "order by _eqp.RackNo desc";
                    orderquery1 = "order by _cd.RackNo desc";
                    orderquery2 = "order by _cfd.RackNo desc";
                }
            }
            else
            {
                orderquery = "--order by tech.LastUpdatedDate desc";
                orderquery1 = "order by _cd.Id desc";
                orderquery2 = "order by _cfd.Id desc";
            }
            if (!string.IsNullOrWhiteSpace(filter.technician) && filter.technician != "'null'")
            {
                FilterByUserId = string.Format("And tech.TechnicianId in ({0})", filter.technician);
                TechQuery = string.Format(" \t\t\t(select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Add' and _tech.EquipmentId = tech.EquipmentId and _tech.TechnicianId = tech.TechnicianId) \r\n                        - (select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Release' and _tech.EquipmentId = tech.EquipmentId and _tech.TechnicianId = tech.TechnicianId) \r\n                        - (select isnull(sum(Quantity),0) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  =tech.EquipmentId  and (b.TechnicianId  = tech.TechnicianId or (b.TechnicianId = '22222222-2222-2222-2222-222222222221' and b.ReceivedBy = tech.TechnicianId)) \r\n                            and b.IsApprove = 0 and b.IsDecline = 0 )\r\n                        as Quantity,");
                TruckAmount = string.Format(" ((Select ISNULL(SUM(_eqp.SupplierCost),0) from Equipment _eqp where tech.EquipmentId=_eqp.EquipmentId And tech.TechnicianId in (tech.TechnicianId)))*\r\n\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t         \t\t\t((select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Add' and _tech.EquipmentId = tech.EquipmentId and _tech.TechnicianId = tech.TechnicianId) \r\n                        - (select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Release' and _tech.EquipmentId = tech.EquipmentId and _tech.TechnicianId = tech.TechnicianId) \r\n                        - (select isnull(sum(Quantity),0) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  =tech.EquipmentId  and (b.TechnicianId  = tech.TechnicianId or (b.TechnicianId = '22222222-2222-2222-2222-222222222221' and b.ReceivedBy = tech.TechnicianId)) \r\n                            and b.IsApprove = 0 and b.IsDecline = 0 )) AmountTruck,");
                //TechQuery = string.Format("((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId in ({0}))-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId in ({0}))) Quantity,", filter.technician);
                //TruckAmount = string.Format("((Select ISNULL(SUM(_eqp.SupplierCost),0) from Equipment _eqp where _inv.EquipmentId=_eqp.EquipmentId And _inv.TechnicianId in ({0})) * ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId in ({0}))-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId in ({0})))) Amount", filter.technician);
            }
            else
            {

                TechQuery = string.Format("(select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Add' and _tech.EquipmentId = tech.EquipmentId and _tech.TechnicianId = tech.TechnicianId) \r\n                        - (select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Release' and _tech.EquipmentId = tech.EquipmentId and _tech.TechnicianId = tech.TechnicianId) \r\n                        - (select isnull(sum(Quantity),0) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  =tech.EquipmentId  and (b.TechnicianId  = tech.TechnicianId or (b.TechnicianId = '22222222-2222-2222-2222-222222222221' and b.ReceivedBy = tech.TechnicianId)) \r\n                            and b.IsApprove = 0 and b.IsDecline = 0 )\r\n                        as Quantity,");
                TruckAmount = string.Format(" ((Select ISNULL(SUM(_eqp.SupplierCost),0) from Equipment _eqp where tech.EquipmentId=_eqp.EquipmentId And tech.TechnicianId in (tech.TechnicianId)))*\r\n\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t         \t\t\t((select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Add' and _tech.EquipmentId = tech.EquipmentId and _tech.TechnicianId = tech.TechnicianId) \r\n                        - (select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Release' and _tech.EquipmentId = tech.EquipmentId and _tech.TechnicianId = tech.TechnicianId) \r\n                        - (select isnull(sum(Quantity),0) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  =tech.EquipmentId  and (b.TechnicianId  = tech.TechnicianId or (b.TechnicianId = '22222222-2222-2222-2222-222222222221' and b.ReceivedBy = tech.TechnicianId)) \r\n                            and b.IsApprove = 0 and b.IsDecline = 0 )) AmountTruck,");
                //TechQuery = string.Format("((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId=_inv.TechnicianId)-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId=_inv.TechnicianId)) Quantity,");
                //TruckAmount = string.Format("((Select ISNULL(SUM(_eqp.SupplierCost),0) from Equipment _eqp where _inv.EquipmentId=_eqp.EquipmentId) * ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId=_inv.TechnicianId)-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId=_inv.TechnicianId))) Amount");
            }
            if (!string.IsNullOrWhiteSpace(filter.category) && filter.category != "'null'" && filter.category != "'undefined'" && filter.category != "null" && filter.category != "undefined")
            {
                filterByEquipmentType = string.Format("and _eqpType.Id in ({0})", filter.category);
            }
            if (!string.IsNullOrWhiteSpace(filter.manufact) && filter.manufact != "'null'" && filter.manufact != "'undefined'" && filter.manufact != "undefined" && filter.manufact != "null")
            {
                filterByEquipmentClass = string.Format("and manu.ManufacturerId in ({0})", filter.manufact);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filter.CompanyId,
                        filterColumntext,
                        filtertext,
                        filterByActiveStatus,
                        filterByEquipmentClass,
                        filterByEquipmentType,
                        filterByStockStatus,
                        FilterByUserId,
                        filter.UserId,
                        orderquery,
                        orderquery1,
                        orderquery2,
                        TechQuery,
                        TruckAmount,
                        filter.Start,
                        filter.End,
                        DateQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", filter.PageNo));
                    AddParameter(cmd, pInt32("pagesize", filter.PageSize));
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", filter.SearchText)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetEquipmentListReportByFilterTechSummaryReport(FilterEquipment filter)
        {
            string TechQuery = "";
            string TruckAmount = "";
            string sqlQuery = @"";
            string DateQuery2 = "";
            if (filter.Start != null || filter.End != null)
            {
                DateQuery2 = string.Format("and itech.LastUpdatedDate between '{0}' and '{1}'", filter.Start, filter.End);

            }
            if (!string.IsNullOrWhiteSpace(filter.RepType) && filter.RepType == "Transfer")
            {
                sqlQuery = @"
                                    select convert(date, itech.LastUpdatedDate) as DateTransferred,'WareHouse' as Origin, emp.FirstName + ' ' + emp.LastName as UserName,
                                    sum(itech.Quantity) as NoOfUnit
                                    from InventoryTech itech
                                    left join Employee emp on emp.UserId = itech.TechnicianId 
                                    
                                                WHERE 
			                                    itech.CompanyId = '{0}'
                                                and itech.LastUpdatedDate is not null
                                                {14}
                                                --AND ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId='{8}')-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId='{8}'))>0
                                                {7}
                                                {3}
                                                {4}
                                                {5}
                                                {6}
                                                {15}
                                        group by convert(date, itech.LastUpdatedDate), emp.FirstName + ' ' + emp.LastName, itech.TechnicianId
                                         order by DateTransferred desc
                                           ";
            }
            else
            {
                sqlQuery = @"
                                   select convert(date, itech.LastUpdatedDate) as DateTransferred,'WareHouse' as Origin, emp.FirstName + ' ' + emp.LastName as UserName,
                                    sum(itech.Quantity) as NoOfUnit
                                    from InventoryTech itech
                                    left join Employee emp on emp.UserId = itech.TechnicianId 
                                    
                                                WHERE 
			                                    itech.CompanyId = '{0}'
                                                and itech.LastUpdatedDate is not null
                                                {14}
                                                --AND ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId='{8}')-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId='{8}'))>0
                                                {7}
                                                {3}
                                                {4}
                                                {5}
                                                {6}
                                                {15}
                                        group by convert(date, itech.LastUpdatedDate), emp.FirstName + ' ' + emp.LastName, itech.TechnicianId
                                            order by DateTransferred desc
                                           ";
            }


            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string FilterByUserId = "";
            string orderquery = "";
            string orderquery1 = "";
            string orderquery2 = "";
            string ReceivedDateFilte = "";

            if (!string.IsNullOrWhiteSpace(filter.SearchText) && filter.SearchText != "-1" && filter.SearchText != "'null'" && filter.SearchText != "'undefined'" && filter.SearchText != "null" && filter.SearchText != "undefined")
            {
                filterColumntext = @",isNULL(_eqp.Name, '') +
								isNULL(_eqp.SKU,'') + 
                                isNULL(_eqpType.Name,'') + 
                                isNULL(sup.CompanyName,'') +
                                isNULL(manu.Name,'') + 
								isNULL(_eqpClass.Name,'')  FilterText  ";
                filtertext = string.Format(" Where FilterText like '%{0}%' ", filter.SearchText);
            }

            if (filter.Transferred_Date_From != new DateTime() && filter.Transferred_Date_To != new DateTime())
            {
                filter.Transferred_Date_To = filter.Transferred_Date_To.AddHours(23).AddMinutes(59).AddSeconds(59);

                ReceivedDateFilte = string.Format("and itech.LastUpdatedDate between  '{0}' and '{1}'", filter.Transferred_Date_From, filter.Transferred_Date_To);
            }
            else if (filter.Transferred_Date_From == new DateTime() && filter.Transferred_Date_To != new DateTime())
            {
                filter.Transferred_Date_To = filter.Transferred_Date_To.AddHours(23).AddMinutes(59).AddSeconds(59);

                ReceivedDateFilte = string.Format("and itech.LastUpdatedDate <=  '{0}' ", filter.Transferred_Date_To);
            }
            else if (filter.Transferred_Date_From != new DateTime() && filter.Transferred_Date_To == new DateTime())
            {
                ReceivedDateFilte = string.Format("and itech.LastUpdatedDate >=  '{0}' ", filter.Transferred_Date_From);
            }


            if (!string.IsNullOrWhiteSpace(filter.order))
            {
                if (filter.order == "ascending/category")
                {
                    orderquery = "order by _eqpType.Name asc";
                    orderquery1 = "order by _cd.Category asc";
                    orderquery2 = "order by _cfd.Category asc";
                }
                else if (filter.order == "descending/category")
                {
                    orderquery = "order by _eqpType.Name desc";
                    orderquery1 = "order by _cd.Category desc";
                    orderquery2 = "order by _cfd.Category desc";
                }
                else if (filter.order == "ascending/monthlyfee")
                {
                    orderquery = "order by _eqp.Retail asc";
                    orderquery1 = "order by _cd.Retail asc";
                    orderquery2 = "order by _cfd.Retail asc";
                }
                else if (filter.order == "descending/monthlyfee")
                {
                    orderquery = "order by _eqp.Retail desc";
                    orderquery1 = "order by _cd.Retail desc";
                    orderquery2 = "order by _cfd.Retail desc";
                }
                else if (filter.order == "ascending/description")
                {
                    orderquery = "order by _eqp.Comments asc";
                    orderquery1 = "order by _cd.Comments asc";
                    orderquery2 = "order by _cfd.Comments asc";
                }
                else if (filter.order == "descending/description")
                {
                    orderquery = "order by _eqp.Comments desc";
                    orderquery1 = "order by _cd.Comments desc";
                    orderquery2 = "order by _cfd.Comments desc";
                }
                else if (filter.order == "ascending/manu")
                {
                    orderquery = "order by manu.Name asc";
                    orderquery1 = "order by _cd.ManufacturerName asc";
                    orderquery2 = "order by _cfd.ManufacturerName asc";
                }
                else if (filter.order == "descending/manu")
                {
                    orderquery = "order by manu.Name desc";
                    orderquery1 = "order by _cd.ManufacturerName desc";
                    orderquery2 = "order by _cfd.ManufacturerName desc";
                }
                else if (filter.order == "ascending/des")
                {
                    orderquery = "order by _eqp.Name asc";
                    orderquery1 = "order by _cd.Name asc";
                    orderquery2 = "order by _cfd.Name asc";
                }
                else if (filter.order == "descending/des")
                {
                    orderquery = "order by _eqp.Name desc";
                    orderquery1 = "order by _cd.Name desc";
                    orderquery2 = "order by _cfd.Name desc";
                }
                else if (filter.order == "ascending/sku")
                {
                    orderquery = "order by _eqp.SKU asc";
                    orderquery1 = "order by _cd.SKU asc";
                    orderquery2 = "order by _cfd.SKU asc";
                }
                else if (filter.order == "descending/sku")
                {
                    orderquery = "order by _eqp.SKU desc";
                    orderquery1 = "order by _cd.SKU desc";
                    orderquery2 = "order by _cfd.SKU desc";
                }
                else if (filter.order == "ascending/vendor")
                {
                    orderquery = "order by sup.CompanyName asc";
                    orderquery1 = "order by _cd.SupplierName asc";
                    orderquery2 = "order by _cfd.SupplierName asc";
                }
                else if (filter.order == "descending/vendor")
                {
                    orderquery = "order by sup.CompanyName desc";
                    orderquery1 = "order by _cd.SupplierName desc";
                    orderquery2 = "order by _cfd.SupplierName desc";
                }
                else if (filter.order == "ascending/vendorcost")
                {
                    orderquery = "order by _eqp.SupplierCost asc";
                    orderquery1 = "order by _cd.SupplierCost asc";
                    orderquery2 = "order by _cfd.SupplierCost asc";
                }
                else if (filter.order == "descending/vendorcost")
                {
                    orderquery = "order by _eqp.SupplierCost desc";
                    orderquery1 = "order by _cd.SupplierCost desc";
                    orderquery2 = "order by _cfd.SupplierCost desc";
                }
                else if (filter.order == "ascending/type")
                {
                    orderquery = "order by _eqp.Id asc";
                    orderquery1 = "order by _cd.Id asc";
                    orderquery2 = "order by _cfd.Id asc";
                }
                else if (filter.order == "descending/type")
                {
                    orderquery = "order by _eqp.Id desc";
                    orderquery1 = "order by _cd.Id desc";
                    orderquery2 = "order by _cfd.Id desc";
                }
                else if (filter.order == "ascending/qty")
                {
                    orderquery = "order by Quantity asc";
                    orderquery1 = "order by _cd.Quantity asc";
                    orderquery2 = "order by _cfd.Quantity asc";
                }
                else if (filter.order == "descending/qty")
                {
                    orderquery = "order by Quantity desc";
                    orderquery1 = "order by _cd.Quantity desc";
                    orderquery2 = "order by _cfd.Quantity desc";
                }
                else if (filter.order == "ascending/rack")
                {
                    orderquery = "order by _eqp.RackNo asc";
                    orderquery1 = "order by _cd.RackNo asc";
                    orderquery2 = "order by _cfd.RackNo asc";
                }
                else if (filter.order == "descending/rack")
                {
                    orderquery = "order by _eqp.RackNo desc";
                    orderquery1 = "order by _cd.RackNo desc";
                    orderquery2 = "order by _cfd.RackNo desc";
                }
            }
            else
            {
                orderquery = "order by _eqp.Id desc";
                orderquery1 = "order by _cd.Id desc";
                orderquery2 = "order by _cfd.Id desc";
            }
            if (!string.IsNullOrWhiteSpace(filter.technician) && filter.technician != "'null'")
            {
                FilterByUserId = string.Format("And itech.TechnicianId in ({0})", filter.technician);
                TechQuery = string.Format("((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId in ({0}))-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId in ({0}))) Quantity,", filter.technician);
                TruckAmount = string.Format("((Select ISNULL(SUM(_eqp.Retail),0) from Equipment _eqp where _inv.EquipmentId=_eqp.EquipmentId and Type='Add'  And _inv.TechnicianId in ({0}))-(Select ISNULL(SUM(_eqp.Retail),0) from Equipment _eqp where _inv.EquipmentId=_eqp.EquipmentId and Type='Release'  And _inv.TechnicianId in ({0}))) AmountTruck", filter.technician);
            }
            else
            {
                TechQuery = string.Format("((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId=_inv.TechnicianId)-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId=_inv.TechnicianId)) Quantity,");
                TruckAmount = string.Format("((Select ISNULL(SUM(_eqp.Retail),0) from Equipment _eqp where _inv.EquipmentId=_eqp.EquipmentId and Type='Add')-(Select ISNULL(SUM(_eqp.Retail),0) from Equipment _eqp where _inv.EquipmentId=_eqp.EquipmentId and Type='Release')) AmountTruck");
            }
            if (!string.IsNullOrWhiteSpace(filter.category) && filter.category != "null")
            {
                filterByEquipmentType = string.Format("and _eqpType.Id in ({0})", filter.category);
            }
            if (!string.IsNullOrWhiteSpace(filter.manufact) && filter.manufact != "'null'")
            {
                filterByEquipmentClass = string.Format("and manu.ManufacturerId in ({0})", filter.manufact);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filter.CompanyId,//0
                        filterColumntext,//1
                        filtertext,//2
                        filterByActiveStatus,//3
                        filterByEquipmentClass,//4
                        filterByEquipmentType,//5
                        filterByStockStatus,//6
                        FilterByUserId,//7
                        filter.UserId,//8
                        orderquery,//9
                        orderquery1,//10
                        orderquery2,//11
                        TechQuery,//12
                        TruckAmount,//13
                        ReceivedDateFilte,//14
                        DateQuery2);
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
        public DataTable GetEquipmentListReportByFilterTechSummary(FilterEquipment filter)
        {
            string TechQuery = "";
            string TruckAmount = "";
            string sqlQuery = @"";
            string DateQuery1 = "";
            string DateQuery2 = "";
            int pagestart = (filter.PageNo - 1) * filter.PageSize;
            int pageend = filter.PageSize;
            if (filter.Transferred_Date_From != new DateTime() || filter.Transferred_Date_To != new DateTime())
            {
                DateQuery1 = string.Format("and itech.LastUpdatedDate between '{0}' and '{1}'", filter.Transferred_Date_From, filter.Transferred_Date_To);

            }

            if (filter.Start != null || filter.End != null)
            {
                DateQuery2 = string.Format("and itech.LastUpdatedDate between '{0}' and '{1}'", filter.Start, filter.End);

            }
            if (!string.IsNullOrWhiteSpace(filter.RepType) && filter.RepType == "Transfer")
            {
                sqlQuery = @"
                                    select convert(date, itech.LastUpdatedDate) as TransferredDate, emp.FirstName + ' ' + emp.LastName as TechnicianName,
                                    sum(itech.Quantity) as Quantity, itech.TechnicianId
                                      into #SummaryData
                                    from InventoryTech itech
                                    left join Employee emp on emp.UserId = itech.TechnicianId 
                                    
                                                WHERE 
			                                    itech.CompanyId = '{0}'
                                                and itech.LastUpdatedDate is not null
                                                --and itech.LastUpdatedDate between '{15}' and '{16}'
                                                {14}
                                                --AND ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId='{8}')-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId='{8}'))>0
                                                {7}
                                                {3}
                                                {4}
                                                {5}
                                                {6}
                                                {15}
                                                  {16}
                                        group by convert(date, itech.LastUpdatedDate), emp.FirstName + ' ' + emp.LastName, itech.TechnicianId
                                        order by TransferredDate desc

	                            	 select * ,IDENTITY(INT, 1, 1) AS paginationid into #SummaryIdData from #SummaryData
								select top({19}) *  from #SummaryIdData
								where paginationid not in (Select TOP ({17}) paginationid from #SummaryIdData )
							    

								 
							
                                  

                                select Count(*) As TotalCount from #SummaryData
                         

								drop table #SummaryData
								drop table #SummaryIdData
                                           ";
            }
            else
            {
                sqlQuery = @"
                                    select convert(date, itech.LastUpdatedDate) as TransferredDate, emp.FirstName + ' ' + emp.LastName as TechnicianName,
                                    sum(itech.Quantity) as Quantity, itech.TechnicianId
                                    into #SummaryData
                                    from InventoryTech itech
                                    left join Employee emp on emp.UserId = itech.TechnicianId 
                                    
                                                WHERE 
			                                    itech.CompanyId = '{0}'
                                                and itech.LastUpdatedDate is not null
                                                    --and itech.LastUpdatedDate between '{15}' and '{16}'
                                                {14}
                                                --AND ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId='{8}')-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId='{8}'))>0
                                                {7}
                                                {3}
                                                {4}
                                                {5}
                                                {6}
                                                 {15}
                                                  {16}
                                        group by convert(date, itech.LastUpdatedDate), emp.FirstName + ' ' + emp.LastName, itech.TechnicianId
                                     	 order by TransferredDate desc
    	                   
	                            	 select *,IDENTITY(INT, 1, 1) AS paginationid into #SummaryIdData from #SummaryData
								select top({19}) *  from #SummaryIdData
								where paginationid not in (Select TOP ({17}) paginationid from #SummaryIdData )
							    

								 
							
                                  

                                select Count(*) As TotalCount from #SummaryData

								drop table #SummaryData
								drop table #SummaryIdData
                                           ";
            }


            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string FilterByUserId = "";
            string orderquery = "";
            string orderquery1 = "";
            string orderquery2 = "";
            string ReceivedDateFilte = "";

            if (!string.IsNullOrWhiteSpace(filter.SearchText) && filter.SearchText != "-1" && filter.SearchText != "'null'" && filter.SearchText != "'undefined'" && filter.SearchText != "null" && filter.SearchText != "undefined")
            {
                filterColumntext = @",isNULL(convert(date, itech.LastUpdatedDate), '') +
								isNULL(_eqp.SKU,'') + 
                                isNULL(_eqpType.Name,'') + 
                                isNULL(sup.CompanyName,'') +
                                isNULL(manu.Name,'') + 
								isNULL(_eqpClass.Name,'')  FilterText  ";
                filtertext = string.Format(" Where FilterText like '%{0}%' ", filter.SearchText);
            }
            if (filter.Transferred_Date_From != new DateTime() && filter.Transferred_Date_To != new DateTime())
            {
                ReceivedDateFilte = string.Format("and itech.LastUpdatedDate between  '{0}' and '{1}'", filter.Transferred_Date_From, filter.Transferred_Date_To);
            }
            else if (filter.Transferred_Date_From == new DateTime() && filter.Transferred_Date_To != new DateTime())
            {
                ReceivedDateFilte = string.Format("and itech.LastUpdatedDate <=  '{0}' ", filter.Transferred_Date_To);
            }
            else if (filter.Transferred_Date_From != new DateTime() && filter.Transferred_Date_To == new DateTime())
            {
                ReceivedDateFilte = string.Format("and itech.LastUpdatedDate >=  '{0}' ", filter.Transferred_Date_From);
            }



            if (!string.IsNullOrWhiteSpace(filter.order))
            {
                if (filter.order == "ascending/category")
                {
                    orderquery = "order by _eqpType.Name asc";
                    orderquery1 = "order by _cd.Category asc";
                    orderquery2 = "order by _cfd.Category asc";
                }
                else if (filter.order == "descending/category")
                {
                    orderquery = "order by _eqpType.Name desc";
                    orderquery1 = "order by _cd.Category desc";
                    orderquery2 = "order by _cfd.Category desc";
                }
                else if (filter.order == "ascending/monthlyfee")
                {
                    orderquery = "order by _eqp.Retail asc";
                    orderquery1 = "order by _cd.Retail asc";
                    orderquery2 = "order by _cfd.Retail asc";
                }
                else if (filter.order == "descending/monthlyfee")
                {
                    orderquery = "order by _eqp.Retail desc";
                    orderquery1 = "order by _cd.Retail desc";
                    orderquery2 = "order by _cfd.Retail desc";
                }
                else if (filter.order == "ascending/description")
                {
                    orderquery = "order by _eqp.Comments asc";
                    orderquery1 = "order by _cd.Comments asc";
                    orderquery2 = "order by _cfd.Comments asc";
                }
                else if (filter.order == "descending/description")
                {
                    orderquery = "order by _eqp.Comments desc";
                    orderquery1 = "order by _cd.Comments desc";
                    orderquery2 = "order by _cfd.Comments desc";
                }
                else if (filter.order == "ascending/manu")
                {
                    orderquery = "order by manu.Name asc";
                    orderquery1 = "order by _cd.ManufacturerName asc";
                    orderquery2 = "order by _cfd.ManufacturerName asc";
                }
                else if (filter.order == "descending/manu")
                {
                    orderquery = "order by manu.Name desc";
                    orderquery1 = "order by _cd.ManufacturerName desc";
                    orderquery2 = "order by _cfd.ManufacturerName desc";
                }
                else if (filter.order == "ascending/des")
                {
                    orderquery = "order by _eqp.Name asc";
                    orderquery1 = "order by _cd.Name asc";
                    orderquery2 = "order by _cfd.Name asc";
                }
                else if (filter.order == "descending/des")
                {
                    orderquery = "order by _eqp.Name desc";
                    orderquery1 = "order by _cd.Name desc";
                    orderquery2 = "order by _cfd.Name desc";
                }
                else if (filter.order == "ascending/sku")
                {
                    orderquery = "order by _eqp.SKU asc";
                    orderquery1 = "order by _cd.SKU asc";
                    orderquery2 = "order by _cfd.SKU asc";
                }
                else if (filter.order == "descending/sku")
                {
                    orderquery = "order by _eqp.SKU desc";
                    orderquery1 = "order by _cd.SKU desc";
                    orderquery2 = "order by _cfd.SKU desc";
                }
                else if (filter.order == "ascending/vendor")
                {
                    orderquery = "order by sup.CompanyName asc";
                    orderquery1 = "order by _cd.SupplierName asc";
                    orderquery2 = "order by _cfd.SupplierName asc";
                }
                else if (filter.order == "descending/vendor")
                {
                    orderquery = "order by sup.CompanyName desc";
                    orderquery1 = "order by _cd.SupplierName desc";
                    orderquery2 = "order by _cfd.SupplierName desc";
                }
                else if (filter.order == "ascending/vendorcost")
                {
                    orderquery = "order by _eqp.SupplierCost asc";
                    orderquery1 = "order by _cd.SupplierCost asc";
                    orderquery2 = "order by _cfd.SupplierCost asc";
                }
                else if (filter.order == "descending/vendorcost")
                {
                    orderquery = "order by _eqp.SupplierCost desc";
                    orderquery1 = "order by _cd.SupplierCost desc";
                    orderquery2 = "order by _cfd.SupplierCost desc";
                }
                else if (filter.order == "ascending/type")
                {
                    orderquery = "order by _eqp.Id asc";
                    orderquery1 = "order by _cd.Id asc";
                    orderquery2 = "order by _cfd.Id asc";
                }
                else if (filter.order == "descending/type")
                {
                    orderquery = "order by _eqp.Id desc";
                    orderquery1 = "order by _cd.Id desc";
                    orderquery2 = "order by _cfd.Id desc";
                }
                else if (filter.order == "ascending/qty")
                {
                    orderquery = "order by Quantity asc";
                    orderquery1 = "order by _cd.Quantity asc";
                    orderquery2 = "order by _cfd.Quantity asc";
                }
                else if (filter.order == "descending/qty")
                {
                    orderquery = "order by Quantity desc";
                    orderquery1 = "order by _cd.Quantity desc";
                    orderquery2 = "order by _cfd.Quantity desc";
                }
                else if (filter.order == "ascending/rack")
                {
                    orderquery = "order by _eqp.RackNo asc";
                    orderquery1 = "order by _cd.RackNo asc";
                    orderquery2 = "order by _cfd.RackNo asc";
                }
                else if (filter.order == "descending/rack")
                {
                    orderquery = "order by _eqp.RackNo desc";
                    orderquery1 = "order by _cd.RackNo desc";
                    orderquery2 = "order by _cfd.RackNo desc";
                }
            }
            else
            {
                orderquery = "order by _eqp.Id desc";
                orderquery1 = "order by _cd.Id desc";
                orderquery2 = "order by _cfd.Id desc";
            }
            if (!string.IsNullOrWhiteSpace(filter.technician) && filter.technician != "'null'")
            {
                FilterByUserId = string.Format("And itech.TechnicianId in ({0})", filter.technician);
                TechQuery = string.Format("((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId in ({0}))-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId in ({0}))) Quantity,", filter.technician);
                TruckAmount = string.Format("((Select ISNULL(SUM(_eqp.Retail),0) from Equipment _eqp where _inv.EquipmentId=_eqp.EquipmentId and Type='Add'  And _inv.TechnicianId in ({0}))-(Select ISNULL(SUM(_eqp.Retail),0) from Equipment _eqp where _inv.EquipmentId=_eqp.EquipmentId and Type='Release'  And _inv.TechnicianId in ({0}))) AmountTruck", filter.technician);
            }
            else
            {
                TechQuery = string.Format("((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId=_inv.TechnicianId)-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId=_inv.TechnicianId)) Quantity,");
                TruckAmount = string.Format("((Select ISNULL(SUM(_eqp.Retail),0) from Equipment _eqp where _inv.EquipmentId=_eqp.EquipmentId and Type='Add')-(Select ISNULL(SUM(_eqp.Retail),0) from Equipment _eqp where _inv.EquipmentId=_eqp.EquipmentId and Type='Release')) AmountTruck");
            }
            if (!string.IsNullOrWhiteSpace(filter.category) && filter.category != "null")
            {
                filterByEquipmentType = string.Format("and _eqpType.Id in ({0})", filter.category);
            }
            if (!string.IsNullOrWhiteSpace(filter.manufact) && filter.manufact != "'null'")
            {
                filterByEquipmentClass = string.Format("and manu.ManufacturerId in ({0})", filter.manufact);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filter.CompanyId,//0
                        filterColumntext,//1
                        filtertext,//2
                        filterByActiveStatus,//3
                        filterByEquipmentClass,//4
                        filterByEquipmentType,//5
                        filterByStockStatus,//6
                        FilterByUserId,//7
                        filter.UserId,//8
                        orderquery,//9
                        orderquery1,//10
                        orderquery2,//11
                        TechQuery,//12
                        TruckAmount,//13
                        ReceivedDateFilte,//14
                        DateQuery1,//15
                        DateQuery2,//16
                        pagestart,//17
                        pageend,//18
                        filter.PageSize);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", filter.PageNo));
                    AddParameter(cmd, pInt32("pagesize", filter.PageSize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetEquipmentListReportByFilterTechSummary1(FilterEquipment filter)
        {
            string TechQuery = "";
            string TruckAmount = "";
            string sqlQuery = @"";
            string DateQuery1 = "";
            string DateQuery2 = "";
            int pagestart = (filter.PageNo - 1) * filter.PageSize;
            int pageend = filter.PageSize;
            //if (filter.Transferred_Date_From != new DateTime() || filter.Transferred_Date_To != new DateTime())
            //{
            //    DateQuery1 = string.Format("and itech.LastUpdatedDate between '{0}' and '{1}'", filter.Transferred_Date_From, filter.Transferred_Date_To);

            //}

            if (filter.Start != null || filter.End != null)
            {
                DateQuery2 = string.Format("and itech.LastUpdatedDate between '{0}' and '{1}'", filter.Start, filter.End);

            }
            if (!string.IsNullOrWhiteSpace(filter.RepType) && filter.RepType == "Transfer")
            {
                sqlQuery = @"
                                    select convert(date, itech.LastUpdatedDate) as TransferredDate, emp.FirstName + ' ' + emp.LastName as TechnicianName,
                                    sum(itech.Quantity) as Quantity, itech.TechnicianId
                                      into #SummaryData
                                    from InventoryTech itech
                                    left join Employee emp on emp.UserId = itech.TechnicianId 
                                    
                                                WHERE 
			                                    itech.CompanyId = '{0}'
                                                and itech.LastUpdatedDate is not null
                                                --and itech.LastUpdatedDate between '{15}' and '{16}'
                                                {14}
                                                --AND ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId='{8}')-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId='{8}'))>0
                                                {7}
                                                {3}
                                                {4}
                                                {5}
                                                {6}
                                                {15}
                                                  {16}
                                        group by convert(date, itech.LastUpdatedDate), emp.FirstName + ' ' + emp.LastName, itech.TechnicianId
                                        order by TransferredDate desc

	                            	 select * ,IDENTITY(INT, 1, 1) AS paginationid into #SummaryIdData from #SummaryData
								select top({19}) * into #TestTable  from #SummaryIdData
								where paginationid not in (Select TOP ({17}) paginationid from #SummaryIdData )
							    								order by TransferredDate desc


								 
							
                                  

                                select Count(*) As TotalCount from #SummaryData

								select * from #TestTable {10}
								select sum(Quantity) as TotalQuantity from #TestTable

								drop table #SummaryData
								drop table #SummaryIdData
								drop table #TestTable
                                           ";
            }
            else
            {
                sqlQuery = @"
                                    select convert(date, itech.LastUpdatedDate) as TransferredDate, emp.FirstName + ' ' + emp.LastName as TechnicianName,
                                    sum(itech.Quantity) as Quantity, itech.TechnicianId
                                    into #SummaryData
                                    from InventoryTech itech
                                    left join Employee emp on emp.UserId = itech.TechnicianId 
                                    
                                                WHERE 
			                                    itech.CompanyId = '{0}'
                                                and itech.LastUpdatedDate is not null
                                                    --and itech.LastUpdatedDate between '{15}' and '{16}'
                                                {14}
                                                --AND ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId='{8}')-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId='{8}'))>0
                                                {7}
                                                {3}
                                                {4}
                                                {5}
                                                {6}
                                                 {15}
                                                  {16}
                                        group by convert(date, itech.LastUpdatedDate), emp.FirstName + ' ' + emp.LastName, itech.TechnicianId
                                     	 order by TransferredDate desc
    	                   
	                            	 select *,IDENTITY(INT, 1, 1) AS paginationid into #SummaryIdData from #SummaryData
								select top({19}) * into #TestTable  from #SummaryIdData
								where paginationid not in (Select TOP ({17}) paginationid from #SummaryIdData )
							    								order by TransferredDate desc


								 
							
                                  

                                select Count(*) As TotalCount from #SummaryData

								select * from #TestTable {10}
								select sum(Quantity) as TotalQuantity from #TestTable

								drop table #SummaryData
								drop table #SummaryIdData
								drop table #TestTable
                                           ";
            }


            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string FilterByUserId = "";
            string orderquery = "";
            string orderquery1 = "";
            string orderquery2 = "";
            string ReceivedDateFilte = "";

            if (!string.IsNullOrWhiteSpace(filter.SearchText) && filter.SearchText != "-1" && filter.SearchText != "'null'" && filter.SearchText != "'undefined'" && filter.SearchText != "null" && filter.SearchText != "undefined")
            {
                filterColumntext = @",isNULL(convert(date, itech.LastUpdatedDate), '') +
								isNULL(_eqp.SKU,'') + 
                                isNULL(_eqpType.Name,'') + 
                                isNULL(sup.CompanyName,'') +
                                isNULL(manu.Name,'') + 
								isNULL(_eqpClass.Name,'')  FilterText  ";
                filtertext = string.Format(" Where FilterText like '%{0}%' ", filter.SearchText);
            }
            if (filter.Transferred_Date_From != new DateTime() && filter.Transferred_Date_To != new DateTime())
            {
                filter.Transferred_Date_To = filter.Transferred_Date_To.AddHours(23).AddMinutes(59).AddSeconds(59);

                ReceivedDateFilte = string.Format("and itech.LastUpdatedDate between  '{0}' and '{1}'", filter.Transferred_Date_From, filter.Transferred_Date_To);
            }
            else if (filter.Transferred_Date_From == new DateTime() && filter.Transferred_Date_To != new DateTime())
            {
                filter.Transferred_Date_To = filter.Transferred_Date_To.AddHours(23).AddMinutes(59).AddSeconds(59);

                ReceivedDateFilte = string.Format("and itech.LastUpdatedDate <=  '{0}' ", filter.Transferred_Date_To);
            }
            else if (filter.Transferred_Date_From != new DateTime() && filter.Transferred_Date_To == new DateTime())
            {
                ReceivedDateFilte = string.Format("and itech.LastUpdatedDate >=  '{0}' ", filter.Transferred_Date_From);
            }


            #region Order
            if (!string.IsNullOrWhiteSpace(filter.order))
            {
                if (filter.order == "ascending/date")
                {
                    orderquery = "order by #TestTable.[TransferredDate] asc";
                    orderquery1 = "order by [TransferredDate] asc";
                }
                else if (filter.order == "descending/date")
                {
                    orderquery = "order by #TestTable.[TransferredDate] desc";
                    orderquery1 = "order by [TransferredDate] desc";
                }
                else if (filter.order == "ascending/origin")
                {
                    orderquery = "order by #TestTable.TransferredDate asc";
                    orderquery1 = "order by TransferredDate asc";
                }
                else if (filter.order == "descending/origin")
                {
                    orderquery = "order by #TestTable.TransferredDate desc";
                    orderquery1 = "order by TransferredDate desc";
                }
                else if (filter.order == "ascending/username")
                {
                    orderquery = "order by #TestTable.[TechnicianName] asc";
                    orderquery1 = "order by [TechnicianName] asc";
                }
                else if (filter.order == "descending/username")
                {
                    orderquery = "order by #TestTable.[TechnicianName] desc";
                    orderquery1 = "order by [TechnicianName] desc";
                }
                else if (filter.order == "ascending/noofunits")
                {
                    orderquery = "order by #TestTable.[Quantity] asc";
                    orderquery1 = "order by [Quantity] asc";
                }
                else if (filter.order == "descending/noofunits")
                {
                    orderquery = "order by #TestTable.[Quantity] desc";
                    orderquery1 = "order by [Quantity] desc";
                }


                else
                {
                    orderquery = "order by #TestTable.[TransferredDate]  desc";
                    orderquery1 = "order by TransferredDate desc";
                }


            }
            else
            {
                orderquery = "order by #TestTable.[TransferredDate] desc";
                orderquery1 = "order by TransferredDate desc";
            }
            #endregion
            if (!string.IsNullOrWhiteSpace(filter.technician) && filter.technician != "'null'")
            {
                FilterByUserId = string.Format("And itech.TechnicianId in ({0})", filter.technician);
                TechQuery = string.Format("((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId in ({0}))-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId in ({0}))) Quantity,", filter.technician);
                TruckAmount = string.Format("((Select ISNULL(SUM(_eqp.Retail),0) from Equipment _eqp where _inv.EquipmentId=_eqp.EquipmentId and Type='Add'  And _inv.TechnicianId in ({0}))-(Select ISNULL(SUM(_eqp.Retail),0) from Equipment _eqp where _inv.EquipmentId=_eqp.EquipmentId and Type='Release'  And _inv.TechnicianId in ({0}))) AmountTruck", filter.technician);
            }
            else
            {
                TechQuery = string.Format("((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId=_inv.TechnicianId)-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId=_inv.TechnicianId)) Quantity,");
                TruckAmount = string.Format("((Select ISNULL(SUM(_eqp.Retail),0) from Equipment _eqp where _inv.EquipmentId=_eqp.EquipmentId and Type='Add')-(Select ISNULL(SUM(_eqp.Retail),0) from Equipment _eqp where _inv.EquipmentId=_eqp.EquipmentId and Type='Release')) AmountTruck");
            }
            if (!string.IsNullOrWhiteSpace(filter.category) && filter.category != "null")
            {
                filterByEquipmentType = string.Format("and _eqpType.Id in ({0})", filter.category);
            }
            if (!string.IsNullOrWhiteSpace(filter.manufact) && filter.manufact != "'null'")
            {
                filterByEquipmentClass = string.Format("and manu.ManufacturerId in ({0})", filter.manufact);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filter.CompanyId,//0
                        filterColumntext,//1
                        filtertext,//2
                        filterByActiveStatus,//3
                        filterByEquipmentClass,//4
                        filterByEquipmentType,//5
                        filterByStockStatus,//6
                        FilterByUserId,//7
                        filter.UserId,//8
                        orderquery,//9
                        orderquery1,//10
                        orderquery2,//11
                        TechQuery,//12
                        TruckAmount,//13
                        ReceivedDateFilte,//14
                        DateQuery1,//15
                        DateQuery2,//16
                        pagestart,//17
                        pageend,//18
                        filter.PageSize);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", filter.PageNo));
                    AddParameter(cmd, pInt32("pagesize", filter.PageSize));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //public DataTable GetEqupmentListByComapnyId(Guid CompanyId)
        //{

        //    string sqlQuery = @"select 
        //                         _eqp.*,
        //                         _eqpClass.Name as EquipmentType,
        //                         _inv.Quantity as Quantity

        //                        from Equipment _eqp
        //                        left join Inventory _inv
        //                        on _eqp.CompanyId = _inv.CompanyId and _eqp.EquipmentId = _inv.EquipmentId
        //                        left join EquipmentClass _eqpClass
        //                        on _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
        //                        where _eqp.IsActive = 1 and _eqp.CompanyId = '{0}'";
        //    try
        //    {
        //        sqlQuery = string.Format(sqlQuery, CompanyId);
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

        //public DataTable GetEqupmentListByFilters(Guid CompanyId, int ActiveStatus, int EquipmentClass, int EquipmentCategory, int StockStatus)
        //{
        //    string sqlQuery = @"select 
        //        eqp.*,
        //        eqpClass.Name as EquipmentType,
        //        inv.Quantity as Quantity
        //        from Equipment eqp
        //        left join Inventory inv 
        //        on eqp.CompanyId = inv.CompanyId and eqp.EquipmentId = inv.EquipmentId
        //        left join EquipmentClass eqpclass
        //        on eqp.EquipmentClassId = eqpclass.Id and  eqp.CompanyId = eqpclass.CompanyId
        //        where eqp.CompanyId = '{0}'";
        //    string subquery = "";
        //    if (ActiveStatus != -1)
        //    {
        //        if (ActiveStatus == 1)
        //        {
        //            subquery = string.Format("and eqp.IsActive = 1");
        //            sqlQuery += subquery;
        //        }
        //        else
        //        {
        //            subquery = string.Format("and eqp.IsActive = 0");
        //            sqlQuery += subquery;
        //        }
        //    }
        //    if (EquipmentClass != -1)
        //    {
        //        subquery = string.Format("and eqp.EquipmentClassId = {0}", EquipmentClass);
        //        sqlQuery += subquery;
        //    }
        //    if (EquipmentCategory != -1)
        //    {
        //        subquery = string.Format("and eqp.EquipmentTypeId = {0}", EquipmentCategory);
        //        sqlQuery += subquery;
        //    }
        //    if (StockStatus != -1)
        //    {
        //        if (StockStatus == 1)
        //        {
        //            subquery = string.Format("and inv.Quantity between 1 and eqp.reorderpoint");
        //            sqlQuery += subquery;
        //        }
        //        else if (StockStatus == 0)
        //        {
        //            subquery = string.Format("and inv.Quantity = 0");
        //            sqlQuery += subquery;
        //        }
        //    }

        //    try
        //    {
        //        sqlQuery = string.Format(sqlQuery, CompanyId);
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

        public DataTable GetInventoryListByEquipmentIdAndCompanyId(Guid equipmentId, Guid companyId)
        {
            string sqlQuery = @"select invware.Id,invware.Description,invware.LastUpdatedDate,em.FirstName+' '+em.LastName as Name,invware.PurchaseOrderId,invware.Quantity,invware.Type
                                from InventoryWarehouse invware
                                LEFT JOIN Employee em on em.UserId=invware.LastUpdatedBy
                                where invware.CompanyId='{1}' and EquipmentId='{0}'
                                union
                                select invbr.Id,invbr.Description,invbr.LastUpdatedDate,em.FirstName+' '+em.LastName as Name ,invbr.PurchaseOrderId,invbr.Quantity,invbr.Type
                                from InventoryBranch invbr
                                LEFT JOIN Employee em on em.UserId=invbr.LastUpdatedBy
                                where invbr.CompanyId='{1}' and EquipmentId='{0}'
                                union
                                select invtech.Id,invtech.Description,invtech.LastUpdatedDate,em.FirstName+' '+em.LastName as Name ,invtech.PurchaseOrderId,invtech.Quantity,invtech.Type
                                from InventoryTech invtech
                                LEFT JOIN Employee em on em.UserId=invtech.LastUpdatedBy
                                where invtech.CompanyId='{1}' and EquipmentId='{0}'
                                order by LastUpdatedDate desc";
            try
            {
                sqlQuery = string.Format(sqlQuery, equipmentId, companyId);
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
        public DataTable GetTop50InventoryListByEquipmentIdAndCompanyId(Guid equipmentId, Guid companyId)
        {
            string sqlQuery = @"select invware.Id,invware.Description,invware.LastUpdatedDate,em.FirstName+' '+em.LastName as Name,invware.PurchaseOrderId,invware.Quantity,invware.Type
                                into #Eqtable from InventoryWarehouse invware
                                LEFT JOIN Employee em on em.UserId=invware.LastUpdatedBy
                                where invware.CompanyId='{1}' and EquipmentId='{0}'
                                union
                                select invbr.Id,invbr.Description,invbr.LastUpdatedDate,em.FirstName+' '+em.LastName as Name ,invbr.PurchaseOrderId,invbr.Quantity,invbr.Type
                                from InventoryBranch invbr
                                LEFT JOIN Employee em on em.UserId=invbr.LastUpdatedBy
                                where invbr.CompanyId='{1}' and EquipmentId='{0}'
                                union
                                select invtech.Id,invtech.Description,invtech.LastUpdatedDate,em.FirstName+' '+em.LastName as Name ,invtech.PurchaseOrderId,invtech.Quantity,invtech.Type
                                from InventoryTech invtech
                                LEFT JOIN Employee em on em.UserId=invtech.LastUpdatedBy
                                where invtech.CompanyId='{1}' and EquipmentId='{0}'
                                order by LastUpdatedDate desc
                               select top(50) * from #Eqtable
								drop table #Eqtable
";
            try
            {
                sqlQuery = string.Format(sqlQuery, equipmentId, companyId);
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
        public DataSet GetEquipmentListByCompanyId(Guid companyid, DateTime? start, DateTime? end, string category, string manufact, int pageno, int pagesize, string SearchText, string ProductTypeID, string primaryVendorID, string order)
        {
            string sqlQuery = @"";
            string DateQuery = "";
            string CategoryQuery = "";
            string ManuQuery = "";
            string SearchT = "";
            string primaryVendor = "";

            string ProductType = "";

            string orderquery = "";
            string orderquery1 = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/category")
                {
                    orderquery = "order by #equipmentfilter.[Category] asc";
                    orderquery1 = "order by [Category] asc";
                }
                else if (order == "descending/category")
                {
                    orderquery = "order by #equipmentfilter.[Category] desc";
                    orderquery1 = "order by [Category] desc";
                }
                else if (order == "ascending/manufacturer")
                {
                    orderquery = "order by #equipmentfilter.ManufacturerName asc";
                    orderquery1 = "order by ManufacturerName asc";
                }
                else if (order == "descending/manufacturer")
                {
                    orderquery = "order by #equipmentfilter.ManufacturerName desc";
                    orderquery1 = "order by ManufacturerName desc";
                }
                else if (order == "ascending/description")
                {
                    orderquery = "order by #equipmentfilter.[Name] asc";
                    orderquery1 = "order by [Name] asc";
                }
                else if (order == "descending/description")
                {
                    orderquery = "order by #equipmentfilter.[Name] desc";
                    orderquery1 = "order by [Name] desc";
                }
                else if (order == "ascending/SKU")
                {
                    orderquery = "order by #equipmentfilter.[SKU] asc";
                    orderquery1 = "order by [SKU] asc";
                }
                else if (order == "descending/SKU")
                {
                    orderquery = "order by #equipmentfilter.[SKU] desc";
                    orderquery1 = "order by [SKU] desc";
                }
                else if (order == "ascending/primaryvendor")
                {
                    orderquery = "order by #equipmentfilter.[SupplierName] asc";
                    orderquery1 = "order by [SupplierName] asc";
                }
                else if (order == "descending/primaryvendor")
                {
                    orderquery = "order by #equipmentfilter.[SupplierName] desc";
                    orderquery1 = "order by [SupplierName] desc";
                }
                else if (order == "ascending/vendorcost")
                {
                    orderquery = "order by #equipmentfilter.[VendorCost]  asc";
                    orderquery1 = "order by VendorCost asc";
                }
                else if (order == "descending/vendorcost")
                {
                    orderquery = "order by #equipmentfilter.[VendorCost]  desc";
                    orderquery1 = "order by VendorCost desc";
                }
                else if (order == "ascending/producttype")
                {
                    orderquery = "order by #equipmentfilter.[EquipmentClassId]  asc";
                    orderquery1 = "order by EquipmentClassId asc";
                }
                else if (order == "descending/producttype")
                {
                    orderquery = "order by #equipmentfilter.[EquipmentClassId]  desc";
                    orderquery1 = "order by EquipmentClassId desc";
                }
                else if (order == "ascending/onhand")
                {
                    orderquery = "order by #equipmentfilter.[Quantity]  asc";
                    orderquery1 = "order by Quantity asc";
                }
                else if (order == "descending/onhand")
                {
                    orderquery = "order by #equipmentfilter.[Quantity]  desc";
                    orderquery1 = "order by Quantity desc";
                }
                else if (order == "ascending/rackbinnumber")
                {
                    orderquery = "order by #equipmentfilter.[RackNo]  asc";
                    orderquery1 = "order by RackNo asc";
                }
                else if (order == "descending/rackbinnumber")
                {
                    orderquery = "order by #equipmentfilter.[RackNo]  desc";
                    orderquery1 = "order by RackNo desc";
                }
                else
                {
                    orderquery = "order by #equipmentfilter.[Id]  desc";
                    orderquery1 = "order by Id desc";
                }

            }
            else
            {
                orderquery = "order by #equipmentfilter.[Id] desc";
                orderquery1 = "order by Id desc";
            }
            #endregion
            if (start != new DateTime() || end != new DateTime())
            {
                DateQuery = string.Format("and _eqp.CreatedDate between '{0}' and '{1}'", start, end);

            }
            if (!string.IsNullOrWhiteSpace(category) && category != "-1" && category != "null")
            {
                CategoryQuery = string.Format("and _eqpType.Id in ({0})", category);
            }
            if (!string.IsNullOrWhiteSpace(manufact) && manufact != "-1" && manufact != "'null'")
            {
                ManuQuery = string.Format("and manu.ManufacturerId in ({0})", manufact);
            }

            if (!string.IsNullOrWhiteSpace(SearchText) && SearchText != "-1" && SearchText != "'null'" && SearchText != "'undefined'")
            {
                SearchT = string.Format("and (_eqp.Name like '%{0}%' or _eqp.SKU like '%{0}%' or _eqp.SupplierCost like '%{0}%' or ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=_eqp.EquipmentId and Type='Release')) like '%{0}%' or _eqp.RackNo like  '%{0}%')", SearchText);
            }

            if (!string.IsNullOrWhiteSpace(ProductTypeID) && ProductTypeID != "-1" && ProductTypeID != "'null'" && ProductTypeID != "'undefined'")
            {
                ProductType = string.Format("and _eqp.EquipmentClassId in ({0})", ProductTypeID);
            }
            if (!string.IsNullOrWhiteSpace(primaryVendorID) && primaryVendorID != "-1" && primaryVendorID != "'null'" && primaryVendorID != "'undefined'")
            {
                primaryVendor = string.Format("and sup.CompanyName in ({0})", primaryVendorID);
            }

            if (start.HasValue && end.HasValue)
            {
                sqlQuery = @"declare @pagestart int
                                declare @pageend int
                                declare @pageno int
                                declare @pagesize int
                                set @pageno={4}
                                set @pagesize ={5}

                                set @pagestart =(@pageno-1)* @pagesize
                                set @pageend = @pagesize
                                        SELECT 
                                        _eqp.*,
                                        _eqpClass.Name as EquipmentClass,
	                                    ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=_eqp.EquipmentId and Type='Release')) as Quantity,
                                         sup.CompanyName as SupplierName,
                                          _eqpType.Name as Category,
										  --_eqp.SupplierCost as VendorCost,
                                          eqpv.Cost as VendorCost,
										  manu.Name as ManufacturerName
                                            into #equipment
                                          FROM Equipment _eqp
                                            LEFT JOIN EquipmentClass _eqpClass
		                                    ON _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                                            left JOIN EquipmentVendor eqpv
											ON eqpv.EquipmentId = _eqp.EquipmentId
                                            and eqpv.IsPrimary=1
											 LEFT JOIN EquipmentType _eqpType
		                                    ON _eqp.EquipmentTypeId = _eqpType.Id and _eqp.CompanyId = _eqpType.CompanyId
                                            --left join Supplier sup on sup.Id = _eqp.SupplierId
                                            left join Supplier sup on sup.SupplierId = eqpv.SupplierId
											left join Manufacturer manu
											ON manu.Id = _eqp.ManufacturerId
		                                    WHERE _eqp.CompanyId = '{0}' and _eqp.IsActive=1 and _eqp.EquipmentClassId=1
                                            {1}
                                            {2}
                                            {3}
                                            {6}
                                            {7}
                                            {8}
                                            select * into #equipmentfilter from #equipment

											select top(@pagesize) * into #Testtable from #equipmentfilter where Id not in(select top(@pagestart) #equipmentfilter.Id from #equipmentfilter {9}) {10}

											select count(*) [TotalCount]
                                            from #equipmentfilter

                                            select * from #Testtable
											select sum(SupplierCost) as TotalCost, sum(Quantity) as TotalQuantity  from #TestTable
											
											drop table #equipmentfilter
											drop table #equipment
                                            drop table #Testtable";
                sqlQuery = string.Format(sqlQuery, companyid, DateQuery, CategoryQuery, ManuQuery, pageno, pagesize, ProductType, primaryVendor, SearchT, orderquery, orderquery1);
            }
            else
            {
                sqlQuery = @"declare @pagestart int
                                declare @pageend int
                                declare @pageno int
                                declare @pagesize int
                                set @pageno={3}
                                set @pagesize =(4)

                                set @pagestart =(@pageno-1)* @pagesize
                                set @pageend = @pagesize
                                        SELECT 
                                        _eqp.*,
                                        _eqpClass.Name as EquipmentClass,
	                                    ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=_eqp.EquipmentId and Type='Release')) as Quantity,
                                         sup.CompanyName as SupplierName,
                                          _eqpType.Name as Category,
										  --_eqp.SupplierCost as VendorCost,
                                          eqpv.Cost as VendorCost,
										  manu.Name as ManufacturerName
                                            into #equipment
                                          FROM Equipment _eqp
                                            LEFT JOIN EquipmentClass _eqpClass
		                                    ON _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                                            left JOIN EquipmentVendor eqpv
											ON eqpv.EquipmentId = _eqp.EquipmentId
                                            and eqpv.IsPrimary=1
											 LEFT JOIN EquipmentType _eqpType
		                                    ON _eqp.EquipmentTypeId = _eqpType.Id and _eqp.CompanyId = _eqpType.CompanyId
                                            --left join Supplier sup on sup.Id = _eqp.SupplierId
                                            left join Supplier sup on sup.SupplierId = eqpv.SupplierId
											left join Manufacturer manu
											ON manu.Id = _eqp.ManufacturerId
		                                    WHERE _eqp.CompanyId = '{0}' and _eqp.IsActive=1 and _eqp.EquipmentClassId=1
                                            {1}
                                            {2}
                                            {5}
                                            {6}
                                            {7}
                                            select * into #equipmentfilter from #equipment

											select top(@pagesize) * into #Testtable from #equipmentfilter where Id not in(select top(@pagestart) #equipmentfilter.Id from #equipmentfilter {8}) {9}

											select count(*) [TotalCount]
                                            from #equipmentfilter

											select * from #Testtable
											select sum(SupplierCost) as TotalCost, sum(Quantity) as TotalQuantity  from #TestTable

											drop table #equipmentfilter
											drop table #equipment
                                            drop table #Testtable";
                sqlQuery = string.Format(sqlQuery, companyid, CategoryQuery, ManuQuery, pageno, pagesize, ProductType, primaryVendor, SearchT, orderquery, orderquery1);
            }

            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    //AddParameter(cmd, pInt32("pageno", filter.PageNo));
                    //AddParameter(cmd, pInt32("pagesize", filter.PageSize));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetEquipmentListReportByCompanyId(Guid companyid, DateTime? start, DateTime? end, string category, string manufact, string SearchText, string ProductTypeID, string primaryVendorID)
        {
            string sqlQuery = @"";
            string DateQuery = "";
            string CategoryQuery = "";
            string ManuQuery = "";

            string SearchT = "";
            string primaryVendor = "";
            string ProductType = "";

            if (start != new DateTime() || end != new DateTime())
            {
                DateQuery = string.Format("and _eqp.CreatedDate between '{0}' and '{1}'", start, end);

            }
            if (!string.IsNullOrWhiteSpace(category) && category != "-1" && category != "null")
            {
                CategoryQuery = string.Format("and _eqpType.Id in ({0})", category);
            }
            if (!string.IsNullOrWhiteSpace(manufact) && manufact != "-1" && manufact != "'null'")
            {
                ManuQuery = string.Format("and manu.ManufacturerId in ({0})", manufact);
            }

            if (!string.IsNullOrWhiteSpace(SearchText) && SearchText != "-1" && SearchText != "'null'" && SearchText != "'undefined'")
            {
                SearchT = string.Format("and (_eqp.Name like '%{0}%' or _eqp.SKU like '%{0}%' or _eqp.SupplierCost like '%{0}%' or ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=_eqp.EquipmentId and Type='Release')) like '%{0}%' or _eqp.RackNo like  '%{0}%')", SearchText);
            }

            if (!string.IsNullOrWhiteSpace(ProductTypeID) && ProductTypeID != "-1" && ProductTypeID != "'null'" && ProductTypeID != "'undefined'")
            {
                ProductType = string.Format("and _eqp.EquipmentClassId in ({0})", ProductTypeID);
            }
            if (!string.IsNullOrWhiteSpace(primaryVendorID) && primaryVendorID != "-1" && primaryVendorID != "'null'" && primaryVendorID != "'undefined'")
            {
                primaryVendor = string.Format("and sup.CompanyName in ({0})", primaryVendorID);
            }






            if (start.HasValue && end.HasValue)
            {
                sqlQuery = @"
                                        SELECT 
                                         _eqpType.Name as Category, manu.Name as Manufacturer,_eqp.Name as Description,
                                         _eqp.SKU,  sup.CompanyName as [Primary Vendor], cast(_eqp.SupplierCost as decimal(20,2)) as [Primary Vendor Cost],CASE WHEN _eqp.EquipmentClassId = '1' THEN  'Equipment' WHEN _eqp.EquipmentClassId = '2' THEN  'Service' end [Product Type],
                                        ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=_eqp.EquipmentId and Type='Release')) as [On Hand Qty],
										_eqp.RackNo as Rack_BinNumber
	                                   
                                          FROM Equipment _eqp
                                            LEFT JOIN EquipmentClass _eqpClass
		                                    ON _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                                            left JOIN EquipmentVendor eqpv
											ON eqpv.EquipmentId = _eqp.EquipmentId
                                            and eqpv.IsPrimary=1
											 LEFT JOIN EquipmentType _eqpType
		                                    ON _eqp.EquipmentTypeId = _eqpType.Id and _eqp.CompanyId = _eqpType.CompanyId
                                            --left join Supplier sup on sup.Id = _eqp.SupplierId
                                            left join Supplier sup on sup.SupplierId = eqpv.SupplierId
											left join Manufacturer manu
											ON manu.Id = _eqp.ManufacturerId
		                                    WHERE _eqp.CompanyId = '{0}' and _eqp.IsActive=1 and _eqp.EquipmentClassId=1
                                            {1}
                                            {2}
                                            {3}
                                            {4}
                                            {5}
                                            {6}
                                            order by _eqp.Id desc";
                sqlQuery = string.Format(sqlQuery, companyid, DateQuery, CategoryQuery, ManuQuery, ProductType, primaryVendor, SearchT);
            }
            else
            {
                sqlQuery = @"
                                       SELECT 
                                         _eqpType.Name as Category, manu.Name as Manufacturer,_eqp.Name as Description,
                                         _eqp.SKU,  sup.CompanyName as [Primary Vendor],cast(_eqp.SupplierCost as decimal(20,2)) as [Primary Vendor Cost],CASE WHEN _eqp.EquipmentClassId = '1' THEN  'Equipment' WHEN _eqp.EquipmentClassId = '2' THEN  'Service' end [Product Type],
                                        ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=_eqp.EquipmentId and Type='Release')) as [On Hand Qty],
										_eqp.RackNo as Rack_BinNumber
                                          FROM Equipment _eqp
                                            LEFT JOIN EquipmentClass _eqpClass
		                                    ON _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                                            left JOIN EquipmentVendor eqpv
											ON eqpv.EquipmentId = _eqp.EquipmentId
                                            and eqpv.IsPrimary=1
											 LEFT JOIN EquipmentType _eqpType
		                                    ON _eqp.EquipmentTypeId = _eqpType.Id and _eqp.CompanyId = _eqpType.CompanyId
                                            --left join Supplier sup on sup.Id = _eqp.SupplierId
                                            left join Supplier sup on sup.SupplierId = eqpv.SupplierId
											left join Manufacturer manu
											ON manu.Id = _eqp.ManufacturerId
		                                    WHERE _eqp.CompanyId = '{0}' and _eqp.IsActive=1 and _eqp.EquipmentClassId=1
                                            {1}
                                            {2}
                                            {3}
                                            {4}
                                            {5}
                                            order by _eqp.Id desc";
                sqlQuery = string.Format(sqlQuery, companyid, CategoryQuery, ManuQuery, ProductType, primaryVendor, SearchT);
            }

            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    //AddParameter(cmd, pInt32("pageno", filter.PageNo));
                    //AddParameter(cmd, pInt32("pagesize", filter.PageSize));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetTransferInventoryForTechAndDateFilter(Guid techid, string transferfirstdate, string transferlastdate)
        {
            string sqlQuery = @"select itec.LastUpdatedDate, et.Name as Category, manu.Name as Manufacture
                                , eq.Name as EquipName, itec.[Description] as [Desc],
                                eq.SKU as Sku, emp.FirstName + ' ' + emp.LastName as Technician, itec.Quantity
                                from InventoryTech itec
                                left join Equipment eq on eq.EquipmentId = itec.EquipmentId
                                left join EquipmentType et on et.Id = eq.EquipmentTypeId
                                left join Manufacturer manu on manu.Id = eq.ManufacturerId
                                left join Employee emp on emp.UserId = itec.TechnicianId
                                where itec.TechnicianId='{0}' 
                                and itec.LastUpdatedDate between '{1}' and '{2}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, techid, transferfirstdate, transferlastdate);
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

        //where rec.TechnicianId = '{3}'
        //public DataSet GetAllTechReceiveByTechnicianId(TechReceiveFilter filter)
        public DataSet GetAllTechTransferActivity(TechReceiveFilter filter)
        {
            string sqlQuery = @"declare @pagestart int
                                declare @pageend int
                                declare @pageno int
                                declare @pagesize int
                                declare @CompanyId uniqueidentifier
                                set @CompanyId = '{0}'
                                set @pageno={1}
                                set @pagesize ={2}

                                set @pagestart =(@pageno-1)* @pagesize
                                set @pageend = @pagesize

                                -- Receive Inventory Tech

                                    select distinct rec.Id, rec.TechnicianId, rec.EquipmentId, rec.IsReceived, rec.ReceivedDate, rec.ReceivedBy, eq.Name as Name, rec.Quantity, rec.IsApprove, 'Transfer' as [Type], rec.IsDecline,
                                    (select _sentby.FirstName + ' ' + _sentby.LastName from Employee _sentby where _sentby.UserId = rec.TechnicianId) as SentByName,
                                    (select _recby.FirstName + ' ' + _recby.LastName from Employee _recby where _recby.UserId = rec.ReceivedBy) as ReceivedByName,
									isnull((select SUM(_tech.Quantity) from inventorytech _tech where _tech.[Type] = 'Add' and _tech.EquipmentId = rec.EquipmentId and _tech.TechnicianId = rec.ReceivedBy), 0) - isnull((select SUM(_tech.Quantity) from inventorytech _tech where _tech.[Type] = 'Release' and _tech.EquipmentId = rec.EquipmentId and _tech.TechnicianId = rec.ReceivedBy), 0) as TotalQuantity
                                    ,rec.CreatedDate,
                                    eq.SKU
                                    into #techreceive 
									from AssignedInventoryTechReceived rec
                                    left join Equipment eq on eq.EquipmentId = rec.EquipmentId
                                    
                                    where rec.IsReceived = 0
                                    and rec.IsApprove = 1
                                
                                        SELECT * INTO #techreceiverfilter
                                        FROM #techreceive
                                            
	                                    SELECT TOP (@pagesize)
                                        *
                                        FROM #techreceiverfilter
                                        where   Id NOT IN(Select TOP (@pagestart)  Id from #techreceive)
                                            
                                        select count(*) [TotalCount]
                                        from #techreceiverfilter

                                        DROP TABLE #techreceive
                                        DROP TABLE #techreceiverfilter
                                
                                -- Approve Inventory Tech

                                select distinct _rec.Id, _rec.TechnicianId, _rec.EquipmentId, _rec.IsReceived, _rec.ReceivedDate, _rec.ReceivedBy, _eq.Name as Name, _rec.Quantity, _rec.IsApprove, 'Approve' as [Type], _rec.IsDecline,
                                    (select _recby.FirstName + ' ' + _recby.LastName from Employee _recby where _recby.UserId = _rec.ReceivedBy) as ReceivedByName,
									isnull((select SUM(_tech.Quantity) from inventorytech _tech where _tech.[Type] = 'Add' and _tech.EquipmentId = _rec.EquipmentId and _tech.TechnicianId = _rec.TechnicianId), 0) - isnull((select SUM(_tech.Quantity) from inventorytech _tech where _tech.[Type] = 'Release' and _tech.EquipmentId = _rec.EquipmentId and _tech.TechnicianId = _rec.TechnicianId), 0) as TotalQuantity
                                    ,_rec.CreatedDate,
                                    _eq.SKU
                                    into #_techreceive 
									from AssignedInventoryTechReceived _rec
                                    left join Equipment _eq on _eq.EquipmentId = _rec.EquipmentId
                                    
                                    where _rec.IsReceived = 0
									and _rec.IsApprove = 0
                                
                                        SELECT * INTO #_techreceiverfilter
                                        FROM #_techreceive
                                            
	                                    SELECT TOP (@pagesize)
                                        *
                                        FROM #_techreceiverfilter
                                        where   Id NOT IN(Select TOP (@pagestart)  Id from #_techreceive)
                                            
                                        select count(*) [TotalCount]
                                        from #_techreceiverfilter

                                        DROP TABLE #_techreceive
                                        DROP TABLE #_techreceiverfilter";


            try
            {
                sqlQuery = string.Format(sqlQuery,
                    filter.CompanyId,//0
                    filter.PageNo,//1
                    filter.PageSize//2
                    , filter.EmployeeId//3
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

        #region Usage by Account Report
        public DataSet GetUsagebyAccountReportList(DateTime? Start, DateTime? End, int pageno, int pagesize, string searchtext, string order)
        {
            string DateQuery = "";
            string SearchText = "";
            if (Start != new DateTime() && End != new DateTime())
            {
                string StartDateQuery = Start.Value.ToString("yyyy-MM-dd 00:00:00.000");
                string EndDateQuery = End.Value.ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and cusapp.CreatedDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                SearchText = string.Format("and cus.CustomerNo = '{0}' or  Cus.FirstName +' '+ Cus.LastName like '%{0}%'", searchtext);
            }
            string orderquery = "";
            string orderquery1 = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/customer")
                {
                    orderquery = "order by #ed.[CustomerName] asc";
                    orderquery1 = "order by [CustomerName] asc";
                }
                else if (order == "descending/customer")
                {
                    orderquery = "order by #ed.[CustomerName] desc";
                    orderquery1 = "order by [CustomerName] desc";
                }
                else if (order == "ascending/csaccount")
                {
                    orderquery = "order by #ed.CustomerNo asc";
                    orderquery1 = "order by CustomerNo asc";
                }
                else if (order == "descending/csaccount")
                {
                    orderquery = "order by #ed.CustomerNo desc";
                    orderquery1 = "order by CustomerNo desc";
                }
                else if (order == "ascending/installedticket")
                {
                    orderquery = "order by #ed.[InstalledQuantity] asc";
                    orderquery1 = "order by [InstalledQuantity] asc";
                }
                else if (order == "descending/installedticket")
                {
                    orderquery = "order by #ed.[InstalledQuantity] desc";
                    orderquery1 = "order by [InstalledQuantity] desc";
                }
                else if (order == "ascending/serviceticket")
                {
                    orderquery = "order by #ed.[ServiceQuantity] asc";
                    orderquery1 = "order by [ServiceQuantity] asc";
                }
                else if (order == "descending/serviceticket")
                {
                    orderquery = "order by #ed.[ServiceQuantity] desc";
                    orderquery1 = "order by [ServiceQuantity] desc";
                }


                else
                {
                    orderquery = "order by #ed.[Id]  desc";
                    orderquery1 = "order by Id desc";
                }

            }
            else
            {
                orderquery = "order by #ed.[Id] desc";
                orderquery1 = "order by Id desc";
            }
            #endregion
            string sqlQuery = @"    DECLARE @Date nvarchar(50)
	                                DECLARE @pagestart int
	                                DECLARE @pageend int
	                                DECLARE @pageno int
	                                DECLARE @pagesize int

                                    SET @pageno = {0} --default 1
									SET @pagesize = {1} --default 10
                                    SET @pagestart=(@pageno-1)* @pagesize 
                                    SET @pageend = @pagesize

                                select DISTINCT 
                                    cus.Id
                                            ,cus.CustomerId
											,cus.CustomerNo as CustomerNo
											,cus.FirstName + ' ' + cus.LastName as CustomerName
											,(select  ISNULL(SUM(cusapp.Quantity),0) 
												from CustomerAppointmentEquipment cusapp 
												left join ticket tk on  cusapp.AppointmentId = tk.TicketId
												where tk.CustomerId = cus.CustomerId
												and cusapp.IsService = 0 {2})as InstalledQuantity
											,(select  ISNULL(SUM(cusapp.Quantity),0) 
												from CustomerAppointmentEquipment cusapp 
												left join ticket tk on  cusapp.AppointmentId = tk.TicketId
												where tk.CustomerId = cus.CustomerId
												and cusapp.IsService = 1 {2})as ServiceQuantity

									into #TempData


									from Customer cus
                                    left join Ticket tk on tk.CustomerId = cus.CustomerId
									left join CustomerAppointmentEquipment cusapp on cusapp.AppointmentId = tk.TicketId
											where cus.CustomerId in (select customerid from Ticket where TicketId in (select AppointmentId from CustomerAppointmentEquipment))
                                    {2}
                                    {3}
                                  select * into #TestTable from #TempData #ed where (InstalledQuantity > 0 or ServiceQuantity > 0)

									SELECT TOP (@pagesize) * into #TempTable
                                                                FROM #TestTable
                                                                where Id NOT IN(Select TOP (@pagestart) Id from #TestTable order by Id desc)
                                                                and (InstalledQuantity > 0 or ServiceQuantity > 0)
                                                                {5}
                                                                select count(Id) as [TotalCount] from #TestTable

																select * from #TempTable
																select sum(InstalledQuantity) as TotalInstalled,
																sum(ServiceQuantity) as TotalService from #TempTable

                                                                DROP TABLE #TempData
																Drop Table #TestTable
                                                                Drop Table #TempTable
";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        pageno,
                                        pagesize,
                                        DateQuery,
                                        SearchText,
                                        orderquery,
                                        orderquery1
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
        public DataSet GetUsagebyAccountReportEquipmentList(Guid CustomerId, DateTime? Start, DateTime? End)
        {
            string DateQuery = "";
            if (Start != new DateTime() && End != new DateTime())
            {
                string StartDateQuery = Start.Value.ToString("yyyy-MM-dd 00:00:00.000");
                string EndDateQuery = End.Value.ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and cusapp.CreatedDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }
            string sqlQuery = @"    	 select   cusapp.EquipName 
			                                      ,cusapp.Quantity
			                                      ,tk.Id as TicketIntId
		                                    from CustomerAppointmentEquipment cusapp 
		                                    left join ticket tk on  cusapp.AppointmentId = tk.TicketId
		                                    where tk.CustomerId = '{0}'
		                                    and cusapp.IsService = 0
                                            {1}";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        CustomerId,
                                        DateQuery
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
        public DataSet GetUsagebyAccountReportServiceEquipmentList(Guid CustomerId, DateTime? Start, DateTime? End)
        {
            string DateQuery = "";
            if (Start != new DateTime() && End != new DateTime())
            {
                string StartDateQuery = Start.Value.ToString("yyyy-MM-dd 00:00:00.000");
                string EndDateQuery = End.Value.ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and cusapp.CreatedDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }
            string sqlQuery = @"   select   cusapp.EquipName 
			                                      ,cusapp.Quantity
			                                      ,tk.Id as TicketIntId
		                                    from CustomerAppointmentEquipment cusapp 
		                                    left join ticket tk on  cusapp.AppointmentId = tk.TicketId
		                                    where tk.CustomerId = '{0}'
		                                    and cusapp.IsService = 1
                                            {1}";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        CustomerId,
                                        DateQuery
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
        public DataTable DownloadUsagebyAccountReportPartialList(DateTime? Start, DateTime? End, string searchtext)
        {
            string DateQuery = "";
            string SearchText = "";
            if (Start != new DateTime() && End != new DateTime())
            {
                string StartDateQuery = Start.Value.ToString("yyyy-MM-dd 00:00:00.000");
                string EndDateQuery = End.Value.ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and cusapp.CreatedDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                SearchText = string.Format("and cus.CustomerNo = '{0}' or  Cus.FirstName +' '+ Cus.LastName like '%{0}%'", searchtext);
            }
            string sqlQuery = @"select DISTINCT 
                                             cus.CustomerNo as [CS Account No]
											,cus.FirstName + ' ' + cus.LastName as [Customer Name]
											,(select  ISNULL(SUM(cusapp.Quantity),0) 
												from CustomerAppointmentEquipment cusapp 
												left join ticket tk on  cusapp.AppointmentId = tk.TicketId
												where tk.CustomerId = cus.CustomerId
												and cusapp.IsService = 0 {0} ) as [Inventory (Installed Ticket)]
											,(select  ISNULL(SUM(cusapp.Quantity),0) 
												from CustomerAppointmentEquipment cusapp 
												left join ticket tk on  cusapp.AppointmentId = tk.TicketId
												where tk.CustomerId = cus.CustomerId
												and cusapp.IsService = 1 {0} ) as [Inventory (Service Ticket)]

									into #TempData


									from Customer cus
                                    left join Ticket tk on tk.CustomerId = cus.CustomerId
									left join CustomerAppointmentEquipment cusapp on cusapp.AppointmentId = tk.TicketId
											where cus.CustomerId in (select customerid from Ticket where TicketId in (select AppointmentId from CustomerAppointmentEquipment))
                                   {0}
                                   {1}
									select * from #TempData
												where [Inventory (Installed Ticket)] > 0 or [Inventory (Service Ticket)] > 0
												Order by [Customer Name] asc
                                                                DROP TABLE #TempData";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        DateQuery,
                                        SearchText
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
        #endregion

        #region RMA Report
        public DataSet GetRMAEquipmentReport(DateTime? Start, DateTime? End, int pageno, int pagesize, string searchtext, string order)
        {
            string DateQuery = "";
            string SearchText = "";
            if (Start != new DateTime() && End != new DateTime())
            {
                DateQuery = string.Format("and equpret.LastUpdatedDate between '{0}' and '{1}'", Start, End);
            }

            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                SearchText = string.Format("and (equp.SKU = '{0}' or  equp.Name like '%{0}%')", searchtext);
            }
            string orderquery = "";
            string orderquery1 = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/equipment")
                {
                    orderquery = "order by #ed.[Name] asc";
                    orderquery1 = "order by [Name] asc";
                }
                else if (order == "descending/equipment")
                {
                    orderquery = "order by #ed.[Name] desc";
                    orderquery1 = "order by [Name] desc";
                }
                else if (order == "ascending/quantity")
                {
                    orderquery = "order by #ed.Quantity asc";
                    orderquery1 = "order by Quantity asc";
                }
                else if (order == "descending/quantity")
                {
                    orderquery = "order by #ed.Quantity desc";
                    orderquery1 = "order by Quantity desc";
                }
                else if (order == "ascending/description")
                {
                    orderquery = "order by #ed.[Description] asc";
                    orderquery1 = "order by [Description] asc";
                }
                else if (order == "descending/description")
                {
                    orderquery = "order by #ed.[Description] desc";
                    orderquery1 = "order by [Description] desc";
                }
                else if (order == "ascending/sku")
                {
                    orderquery = "order by #ed.[SKU] asc";
                    orderquery1 = "order by [SKU] asc";
                }
                else if (order == "descending/sku")
                {
                    orderquery = "order by #ed.[SKU] desc";
                    orderquery1 = "order by [SKU] desc";
                }
                else if (order == "ascending/createdby")
                {
                    orderquery = "order by #ed.[CreatedBy] asc";
                    orderquery1 = "order by [CreatedBy] asc";
                }
                else if (order == "descending/createdby")
                {
                    orderquery = "order by #ed.[CreatedBy] desc";
                    orderquery1 = "order by [CreatedBy] desc";
                }
                else if (order == "ascending/rma")
                {
                    orderquery = "order by #ed.[RMADate]  asc";
                    orderquery1 = "order by RMADate asc";
                }
                else if (order == "descending/rma")
                {
                    orderquery = "order by #ed.[RMADate]  desc";
                    orderquery1 = "order by RMADate desc";
                }
                else if (order == "ascending/amount")
                {
                    orderquery = "order by #ed.[Amount]  asc";
                    orderquery1 = "order by Amount asc";
                }
                else if (order == "descending/amount")
                {
                    orderquery = "order by #ed.[Amount]  desc";
                    orderquery1 = "order by Amount desc";
                }
                else
                {
                    orderquery = "order by #ed.[Id]  desc";
                    orderquery1 = "order by Id desc";
                }

            }
            else
            {
                orderquery = "order by #ed.[Id] desc";
                orderquery1 = "order by Id desc";
            }
            #endregion
            string sqlQuery = @"    DECLARE @Date nvarchar(50)
	                                DECLARE @pagestart int
	                                DECLARE @pageend int
	                                DECLARE @pageno int
	                                DECLARE @pagesize int

                                    SET @pageno = {0} --default 1
									SET @pagesize = {1} --default 10
                                    SET @pagestart=(@pageno-1)* @pagesize 
                                    SET @pageend = @pagesize

                                    select 
                                            equpret.Id
                                            ,equp.SKU
                                            ,equp.Name
                                            ,equpret.[Description]
                                            ,equpret.Quantity
                                            ,equp.Retail as Amount
                                            ,emp.Title +' '+ emp.FirstName +' '+ emp.LastName as CreatedBy
                                            ,equpret.LastUpdatedDate as RMADate

									into #TempData


									from EquipmentReturn equpret

                                    Left join Equipment equp on equp.EquipmentId = equpret.EquipmentId
                                    Left join Employee emp on emp.UserId = equpret.LastUpdatedBy
                                    where equpret.Id is not null
                                    {2}
                                    {3}
									SELECT TOP (@pagesize) #ed.* into #TestTable
                                                                FROM #TempData #ed
                                                                where Id NOT IN(Select TOP (@pagestart) Id from #TempData #ed {4})
                                                                {5}
                                                                select count(Id) as [TotalCount] from #TempData
																select * from #TestTable
																select sum(Quantity) as TotalQuantity,
																sum(Amount) as TotalAmount from #TestTable
                                                                DROP TABLE #TempData
																DROP TABLE #TestTable";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        pageno,
                                        pagesize,
                                        DateQuery,
                                        SearchText,
                                        orderquery,
                                        orderquery1
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
        public DataTable DownloadRMAReport(DateTime? Start, DateTime? End, string searchtext)
        {
            string DateQuery = "";
            string SearchText = "";
            if (Start != new DateTime() && End != new DateTime())
            {
                DateQuery = string.Format("and equpret.LastUpdatedDate between '{0}' and '{1}'", Start, End);
            }

            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                SearchText = string.Format("and equp.SKU = '{0}' or  equp.Name like '%{0}%'", searchtext);
            }
            string sqlQuery = @"select 
                                         
                                        equp.Name as EquipmentName
                                        ,equpret.[Description]
                                        ,equp.SKU
                                     
                                        ,emp.Title +' '+ emp.FirstName +' '+ emp.LastName as CreatedBy
                                        ,FORMAT(equpret.LastUpdatedDate,'MM/dd/yyyy') AS RMADate 
                                        ,equpret.Quantity
                                        ,equp.Retail as Amount


                                        from EquipmentReturn equpret

                                        Left join Equipment equp on equp.EquipmentId = equpret.EquipmentId
                                        Left join Employee emp on emp.UserId = equpret.LastUpdatedBy
                                        where equpret.Id is not null 
                                         {0}
                                         {1}
                                         Order by Name";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        DateQuery,
                                        SearchText
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
        #endregion

        #region Purchase Order Report
        public DataSet GetPurchaseOrderList(DateTime? Start, DateTime? End, int pageno, int pagesize, string searchtext, string StatusIDList, string order)
        {
            string DateQuery = "";
            string SearchText = "";
            if (StatusIDList == "null")
            {
                StatusIDList = StatusIDList.Substring(0, StatusIDList.Length - 4);


            }
            var array = StatusIDList.Split(",");
            string query = "";
            if (array != null)
            {
                foreach (var item in array)
                {
                    query += string.Format("'{0}',", item);
                }
                query = query.Remove(query.Length - 1, 1);
            }

            string filterquery = "";
            if (!string.IsNullOrWhiteSpace(query))
            {
                filterquery += string.Format("and POW.[Status] in ({0})", query);
            }
            if (Start != new DateTime() && End != new DateTime())
            {
                DateQuery = string.Format("and POW.CreatedDate between '{0}' and '{1}'", Start, End);
            }

            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                SearchText = string.Format("and POW.PurchaseOrderId = '{0}' or  Sup.CompanyName like '%{0}%'", searchtext);
            }
            string orderquery = "";
            string orderquery1 = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/orderid")
                {
                    orderquery = "order by #ed.[PurchaseOrderId] asc";
                    orderquery1 = "order by [PurchaseOrderId] asc";
                }
                else if (order == "descending/orderid")
                {
                    orderquery = "order by #ed.[PurchaseOrderId] desc";
                    orderquery1 = "order by [PurchaseOrderId] desc";
                }
                else if (order == "ascending/vendor")
                {
                    orderquery = "order by #ed.CompanyName asc";
                    orderquery1 = "order by CompanyName asc";
                }
                else if (order == "descending/vendor")
                {
                    orderquery = "order by #ed.CompanyName desc";
                    orderquery1 = "order by CompanyName desc";
                }
                else if (order == "ascending/createdby")
                {
                    orderquery = "order by #ed.[CreatdBy] asc";
                    orderquery1 = "order by [CreatdBy] asc";
                }
                else if (order == "descending/createdby")
                {
                    orderquery = "order by #ed.[CreatdBy] desc";
                    orderquery1 = "order by [CreatdBy] desc";
                }
                else if (order == "ascending/status")
                {
                    orderquery = "order by #ed.[Status] asc";
                    orderquery1 = "order by [Status] asc";
                }
                else if (order == "descending/status")
                {
                    orderquery = "order by #ed.[Status] desc";
                    orderquery1 = "order by [Status] desc";
                }
                else if (order == "ascending/amount")
                {
                    orderquery = "order by #ed.[TotalAmount] asc";
                    orderquery1 = "order by [TotalAmount] asc";
                }
                else if (order == "descending/amount")
                {
                    orderquery = "order by #ed.[TotalAmount] desc";
                    orderquery1 = "order by [TotalAmount] desc";
                }
                else if (order == "ascending/quantity")
                {
                    orderquery = "order by #ed.[Quantity]  asc";
                    orderquery1 = "order by Quantity asc";
                }
                else if (order == "descending/quantity")
                {
                    orderquery = "order by #ed.[Quantity]  desc";
                    orderquery1 = "order by Quantity desc";
                }

                else
                {
                    orderquery = "order by #ed.[Id]  desc";
                    orderquery1 = "order by Id desc";
                }

            }
            else
            {
                orderquery = "order by #ed.[Id] desc";
                orderquery1 = "order by Id desc";
            }
            #endregion
            string sqlQuery = @"    DECLARE @Date nvarchar(50)
	                                DECLARE @pagestart int
	                                DECLARE @pageend int
	                                DECLARE @pageno int
	                                DECLARE @pagesize int

                                    SET @pageno = {0} --default 1
									SET @pagesize = {1} --default 10
                                    SET @pagestart=(@pageno-1)* @pagesize 
                                    SET @pageend = @pagesize

                                    select
                                        POW.Id
                                        ,POW.PurchaseOrderId
                                        ,Sup.CompanyName
                                        ,POW.TotalAmount
                                        ,(select ISNULL(SUM(Quantity),0) from PurchaseOrderDetail where PurchaseOrderId = POW.PurchaseOrderId) as Quantity
                                        ,Emp.FirstName +' '+ Emp.LastName CreatdBy
                                        ,POW.[Status]

									into #TempData

									from PurchaseOrderWarehouse POW

                                    left join Supplier Sup on Sup.SupplierId = POW.SuplierId
                                    left join Employee Emp on Emp.UserId = POW.CreatedByUid
                                    where POW.[Status] != 'Init'
                                    {2}
                                    {3}
                                    {4}
									SELECT TOP (@pagesize) #ed.* into #TestTable
                                                                FROM #TempData #ed
                                                                where Id NOT IN(Select TOP (@pagestart) Id from #TempData #ed {5})
                                                                --Order by PurchaseOrderId desc
                                                                 {6}
                                                                select count(Id) as [TotalCount] from #TempData
                                                                select * from #TestTable
																select sum(TotalAmount) as TotalAmount, sum(Quantity) as TotalQuantity  from #TestTable
                                                                DROP TABLE #TempData
                                                                Drop TABLE #TestTable";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        pageno,
                                        pagesize,
                                        DateQuery,
                                        SearchText,
                                        filterquery,
                                        orderquery,
                                        orderquery1
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
        public DataSet GetPurchaseOrderPartialList(String Id, DateTime? start, DateTime? end)
        {
            string DateQuery = "";

            if (start != new DateTime() && end != new DateTime())
            {
                DateQuery = string.Format("and POD.CreatedDate between '{0}' and '{1}'", start, end);

            }

            string sqlQuery = @"select				
								     POD.EquipName
                                    ,POD.Quantity
                                    ,POD.UnitPrice
                                    ,POD.TotalPrice
                                    
                                    from PurchaseOrderDetail POD
                                    where POD.PurchaseOrderId = '{0}'
                                    {1}";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        Id,
                                        DateQuery
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
        public DataTable GetPurchaseOrderStatus()
        {


            string sqlQuery = @"select distinct
                                        
                                        POW.[Status]

								

									from PurchaseOrderWarehouse POW

                                    left join Supplier Sup on Sup.SupplierId = POW.SuplierId
                                    left join Employee Emp on Emp.UserId = POW.CreatedByUid
                                    where POW.[Status] != 'Init'
                                    ";

            try
            {
                sqlQuery = string.Format(sqlQuery

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
        public DataTable DownloadPurchaseOrderReport(DateTime? Start, DateTime? End, string searchtext, string StatusIDList)
        {
            string DateQuery = "";
            string SearchText = "";
            if (StatusIDList == "null")
            {
                StatusIDList = StatusIDList.Substring(0, StatusIDList.Length - 4);


            }
            var array = StatusIDList.Split(",");
            string query = "";
            if (array != null)
            {
                foreach (var item in array)
                {
                    query += string.Format("'{0}',", item);
                }
                query = query.Remove(query.Length - 1, 1);
            }

            string filterquery = "";
            if (!string.IsNullOrWhiteSpace(query))
            {
                filterquery += string.Format("and POW.[Status] in ({0})", query);
            }
            if (Start != new DateTime() && End != new DateTime())
            {
                DateQuery = string.Format("and POW.CreatedDate between '{0}' and '{1}'", Start, End);
            }

            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                SearchText = string.Format("and POW.PurchaseOrderId = '{0}' or  Sup.CompanyName like '%{0}%'", searchtext);
            }
            string sqlQuery = @"
                                    select
                                        POW.Id
                                        ,POW.PurchaseOrderId as [order Id]
                                        ,Sup.CompanyName as [Vendor Name]
										 ,Emp.FirstName +' '+ Emp.LastName CreatdBy
										 ,POW.[Status]
                                        ,POW.TotalAmount as [Amount]
                                        ,(select ISNULL(SUM(Quantity),0) from PurchaseOrderDetail where PurchaseOrderId = POW.PurchaseOrderId) as Quantity

          								from PurchaseOrderWarehouse POW

                                    left join Supplier Sup on Sup.SupplierId = POW.SuplierId
                                    left join Employee Emp on Emp.UserId = POW.CreatedByUid
                                    where POW.[Status] != 'Init'
                                         {0}
                                         {1}
                                         {2}
                                         Order by PurchaseOrderId desc";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        DateQuery,
                                        SearchText,
                                        filterquery
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
        #endregion

        #region ProductHistory
        public DataSet GetProductHistory(int pageno, int pagesize, string searchtext, Guid CompanyId, Guid UserId)
        {
            string SearchText1 = "";
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                SearchText1 = string.Format("and eq.[Name] like '{0}%'", searchtext);
            }
            string sqlQuery = @"    DECLARE @Date nvarchar(50)
	                                DECLARE @pagestart int
	                                DECLARE @pageend int
	                                DECLARE @pageno int
	                                DECLARE @pagesize int

                                    SET @pageno = {0} --default 1
									SET @pagesize = {1} --default 10
                                    SET @pagestart=(@pageno-1)* @pagesize 
                                    SET @pageend = @pagesize
                                    IF OBJECT_ID(N'tmpData', N'U') IS NOT NULL  
                                             Drop table tmpData
                                      IF OBJECT_ID(N'#_tempData', N'U') IS NOT NULL  
                                             Drop table #_tempData  
                                    
                                        SELECT * INTO tmpData from ( 
                                        select 
                                        invtech.Id,
                                        eq.Name,
                                        invtech.Description,
                                        invtech.LastUpdatedDate,
                                        invtech.Quantity,
                                        invtech.Type,
                                        tk.Id as TicketId
                                        from InventoryTech invtech
                                        LEFT JOIN Employee em on em.UserId=invtech.TechnicianId
                                        LEFT JOIN Equipment eq on eq.EquipmentId=invtech.EquipmentId
                                        LEFT JOIN CustomerAppointmentEquipment cae on cae.Id=invtech.CustomerAppointmentEquipmentId
										LEFT JOIN Ticket tk on tk.TicketId=cae.AppointmentId
                                        where invtech.CompanyId='{3}' and eq.EquipmentClassId = 1 and em.UserId = '{4}' {2}
                                    ) as #tmp

								    select * into #_tempData from tmpData
                                    
									SELECT TOP (@pagesize) *
	                                                FROM #_tempData #tmp2
	                                                where Id NOT IN(Select TOP (@pagestart) Id from tmpData Order by LastUpdatedDate desc)
	                                                Order by LastUpdatedDate desc
	                                                select count(Id) as [TotalCount] from tmpData
	                                                DROP TABLE tmpData
													DROP TABLE #_tempData";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        pageno,
                                        pagesize,
                                        SearchText1,
                                        CompanyId,
                                        UserId
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
        #endregion

        #region Digiture

        public DataSet GetDetailedHistoryListByFilters_with_WH(DetailedHistoryFilter filters)
        {
            //_inv.TechnicianId,
            string sqlQuery = @"
                                    SELECT
                                        _eqp.Id,
                                        _eqpType.Name as Category,
                                        _eqp.Name as Description,
                                        _eqp.EquipmentId,
                                            manu.Id as MfgId,
	                                        manu.Name as Manufacturer,
	                                        _eqp.SKU,
                                            
                                        ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where LastUpdatedDate < '{9}' AND invinner.EquipmentId=_eqp.EquipmentId and Type='Add' AND ({7}))-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where LastUpdatedDate < '{9}' AND invinner.EquipmentId=_eqp.EquipmentId and Type='Release' AND ({7}))) OpnBal,
                                        (Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release' AND LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description LIKE '%ticket%' AND ({7})) Added,
                                        (Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add' AND LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description LIKE '%ticket%' AND ({7})) Pulled,
                                        (Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release' AND LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description NOT LIKE '%ticket%' AND ({7})) TrfOut,
                                        (Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  AND LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description NOT LIKE '%ticket%' AND ({7})) TrfIn,
                                        (Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release' AND LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description LIKE '%warehouse%' ) WH_In,
                                        (Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add' AND LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description LIKE '%warehouse%' ) WH_Out,
                                        ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where LastUpdatedDate <= '{10}' AND invinner.EquipmentId=_eqp.EquipmentId and Type='Add' AND ({7}))-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where LastUpdatedDate <= '{10}' AND invinner.EquipmentId=_eqp.EquipmentId and Type='Release' AND ({7}))) OnHand,
                                        _eqp.RepCost
                                        ,isNULL(_eqp.Name, '') +
                                        isNULL(_eqp.SKU,'') + 
                                        isNULL(_eqpType.Name,'') + 
                                        isNULL(sup.CompanyName,'') +
                                        isNULL(manu.Name,'') + 
                                        isNULL(_eqpClass.Name,'')  FilterText
                                            INTO #CustomerData
                                            FROM InventoryTech _inv
                                            LEFT JOIN Equipment _eqp
                                            ON _eqp.CompanyId = _inv.CompanyId and _eqp.EquipmentId = _inv.EquipmentId
                                            LEFT JOIN EquipmentClass _eqpClass
	                                        ON _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                                            left join Supplier sup on sup.Id = _eqp.SupplierId
                                            LEFT JOIN EquipmentType _eqpType
	                                        ON _eqp.EquipmentTypeId = _eqpType.Id and _eqp.CompanyId = _eqpType.CompanyId		                                    
                                            left join Manufacturer manu
	                                        ON manu.Id = _eqp.ManufacturerId
                                                WHERE 
		                                        _eqp.CompanyId = '{0}'
                                                AND _eqp.IsActive = 1
                                                AND ({7})
                                                
                                            SELECT DISTINCT * INTO #CustomerFilterData1
                                            FROM #CustomerData
                                            where (OpnBal>0 OR Added>0 OR Pulled>0 OR TrfOut>0 OR TrfIn>0 OR OnHand>0)
                                            
                                            SELECT DISTINCT * INTO #CustomerFilterData
                                            FROM #CustomerFilterData1
                                            {11}
                                            {12}

	                                        SELECT
                                            Id,
                                            EquipmentId,
	                                        Description,
	                                        SKU,
                                            MfgId,
                                            Manufacturer,
	                                        OpnBal,
	                                        Added,
	                                        Pulled,
	                                        TrfOut,
	                                        TrfIn,
                                            WH_In,
                                            WH_Out,
	                                        OnHand,
	                                        ((OpnBal-Added+Pulled-TrfOut+TrfIn)-OnHand) Diff,
	                                        FilterText
	                                        FROM #CustomerFilterData _cfdL
                                            order by _cfdL.Manufacturer

                                            SELECT
                                            count(*)
	                                        FROM #CustomerFilterData _cfdC

                                            SELECT DISTINCT
                                            Id as Value,
                                            Description as Text 
                                            FROM #CustomerFilterData1 _cfdE

                                            SELECT DISTINCT
                                            MfgId as Value,
                                            Manufacturer as Text 
                                            FROM #CustomerFilterData1 _cfdM

                                            DROP TABLE #CustomerData
                                            DROP TABLE #CustomerFilterData
                                            DROP TABLE #CustomerFilterData1";

            //-- OFFSET {11} ROWS FETCH NEXT {12} ROWS ONLY
            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string orderquery = "order by _cfd.Id desc";
            string filter_MultipleTechs = "AND";
            string filter_MultipleEquipments = "WHERE (";
            string filter_MultipleManufactures = "OR (";

            foreach (var tech in filters.EmployeeIds)
            {
                filter_MultipleTechs += string.Format("OR TechnicianId='{0}' ", tech);
            }
            filter_MultipleTechs = filter_MultipleTechs.Replace("ANDOR", " ");

            foreach (var equip in filters.EquipmentIds)
            {
                filter_MultipleEquipments += string.Format("OR Id = '{0}' ", equip);
            }
            filter_MultipleEquipments = filter_MultipleEquipments.Replace("(OR", "( ");

            if (!filter_MultipleEquipments.Contains("undefined"))
            {
                filter_MultipleEquipments += " )";
            }
            else
            {
                filter_MultipleEquipments = " ";
            }

            foreach (var equip in filters.ManufacturerIds)
            {
                filter_MultipleManufactures += string.Format("OR Id = '{0}' ", equip);
            }
            filter_MultipleManufactures = filter_MultipleManufactures.Replace("(OR", "( ");

            if (!filter_MultipleManufactures.Contains("undefined"))
            {
                filter_MultipleManufactures += " )";
            }
            else
            {
                filter_MultipleManufactures = " ";
            }

            if (!string.IsNullOrWhiteSpace(filters.SearchText))
            {
                filterColumntext = @",isNULL(_eqp.Name, '') +
								isNULL(_eqp.SKU,'') + 
                                isNULL(_eqpType.Name,'') + 
                                isNULL(sup.CompanyName,'') +
                                isNULL(manu.Name,'') + 
								isNULL(_eqpClass.Name,'')  FilterText  ";
                filtertext = string.Format(" Where FilterText like '%{0}%' ", filters.SearchText);
            }

            filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");

            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filters.CompanyId,
                        filterColumntext,
                        filtertext,
                        filterByActiveStatus,
                        filterByEquipmentClass,
                        filterByEquipmentType,
                        filterByStockStatus,
                        filter_MultipleTechs,
                        orderquery,
                        filters.Start,
                        filters.End,
                        filter_MultipleEquipments,
                        filter_MultipleManufactures); //,
                                                      //(filters.PageNo - 1) * filters.PageSize,
                                                      //filters.PageSize);
                                                      //LogError(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "InventoryDataAccess-GetTechTransferListByFilters", Message = string.Format("{0} || {1}", "QLog", sqlQuery), TimeUtc = DateTime.Now });
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                _ErrorLogDataAccess.Insert(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "DA|Inventory|GetDetailedHistoryListByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                return null;
            }
        }

        public DataSet GetDetailedHistoryListByFilters(DetailedHistoryFilter filters)
        {
            //_inv.TechnicianId,
            string sqlQuery = @"
                                    SELECT
                                        _eqp.Id,
                                        _eqpType.Name as Category,
                                        _eqp.Name as Description,
                                        _eqp.EquipmentId,
                                            manu.Id as MfgId,
	                                        manu.Name as Manufacturer,
	                                        _eqp.SKU,
                                            
                                        ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where LastUpdatedDate < '{9}' AND invinner.EquipmentId=_eqp.EquipmentId and Type='Add' AND ({7}))-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where LastUpdatedDate < '{9}' AND invinner.EquipmentId=_eqp.EquipmentId and Type='Release' AND ({7}))) OpnBal,
                                        (Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release' AND LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description LIKE '%ticket%' AND ({7})) Added,
                                        (Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add' AND LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description LIKE '%ticket%' AND ({7})) Pulled,
                                        (Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release' AND LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description NOT LIKE '%ticket%' AND ({7})) TrfOut,
                                        (Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  AND LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description NOT LIKE '%ticket%' AND ({7})) TrfIn,
                                        ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where LastUpdatedDate <= '{10}' AND invinner.EquipmentId=_eqp.EquipmentId and Type='Add' AND ({7}))-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where LastUpdatedDate <= '{10}' AND invinner.EquipmentId=_eqp.EquipmentId and Type='Release' AND ({7}))  - (select isnull(sum(Quantity),0) from dbo.AssignedInventoryTechReceived b 
										 where b.EquipmentId  =_eqp.EquipmentId  
										 and (b.TechnicianId  = '{13}' or (b.TechnicianId = '22222222-2222-2222-2222-222222222221' and b.ReceivedBy = '{13}')) 
                                       and b.IsApprove = 0 and b.IsDecline = 0 )) OnHand,
                                        _eqp.RepCost
                                        ,isNULL(_eqp.Name, '') +
                                        isNULL(_eqp.SKU,'') + 
                                        isNULL(_eqpType.Name,'') + 
                                        isNULL(sup.CompanyName,'') +
                                        isNULL(manu.Name,'') + 
                                        isNULL(_eqpClass.Name,'')  FilterText
                                            INTO #CustomerData
                                            FROM InventoryTech _inv
                                            LEFT JOIN Equipment _eqp
                                            ON _eqp.CompanyId = _inv.CompanyId and _eqp.EquipmentId = _inv.EquipmentId
                                            LEFT JOIN EquipmentClass _eqpClass
	                                        ON _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                                            left join Supplier sup on sup.Id = _eqp.SupplierId
                                            LEFT JOIN EquipmentType _eqpType
	                                        ON _eqp.EquipmentTypeId = _eqpType.Id and _eqp.CompanyId = _eqpType.CompanyId		                                    
                                            left join Manufacturer manu
	                                        ON manu.Id = _eqp.ManufacturerId
                                                WHERE 
		                                        _eqp.CompanyId = '{0}'
                                                AND _eqp.IsActive = 1
                                                AND ({7})
                                                
                                            SELECT DISTINCT * INTO #CustomerFilterData1
                                            FROM #CustomerData
                                            where (OpnBal>0 OR Added>0 OR Pulled>0 OR TrfOut>0 OR TrfIn>0 OR OnHand>0)
                                            
                                            SELECT DISTINCT * INTO #CustomerFilterData
                                            FROM #CustomerFilterData1
                                            {11}
                                            {12}

	                                        SELECT
                                            Id,
                                            EquipmentId,
	                                        Description,
	                                        SKU,
                                            MfgId,
                                            Manufacturer,
	                                        OpnBal,
	                                        Added,
	                                        Pulled,
	                                        TrfOut,
	                                        TrfIn,
	                                        OnHand,
	                                        ((OpnBal-Added+Pulled-TrfOut+TrfIn)-OnHand) Diff,
	                                        FilterText
	                                        FROM #CustomerFilterData _cfdL
                                            order by _cfdL.Manufacturer

                                            SELECT
                                            count(*)
	                                        FROM #CustomerFilterData _cfdC

                                            SELECT DISTINCT
                                            Id as Value,
                                            Description as Text 
                                            FROM #CustomerFilterData1 _cfdE

                                            SELECT DISTINCT
                                            MfgId as Value,
                                            Manufacturer as Text 
                                            FROM #CustomerFilterData1 _cfdM

                                            DROP TABLE #CustomerData
                                            DROP TABLE #CustomerFilterData
                                            DROP TABLE #CustomerFilterData1";
            
            //-- OFFSET {11} ROWS FETCH NEXT {12} ROWS ONLY
            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string orderquery = "order by _cfd.Id desc";
            string filter_MultipleTechs = "AND";
            string filter_TechnicianId = "";
            string filter_MultipleEquipments = "WHERE (";
            string filter_MultipleManufactures = "OR (";

            foreach (var tech in filters.EmployeeIds)
            {
                filter_MultipleTechs += string.Format("OR TechnicianId='{0}' ", tech);
            }
            filter_MultipleTechs = filter_MultipleTechs.Replace("ANDOR"," ");
            foreach (var tech in filters.EmployeeIds)
            {
                filter_TechnicianId += string.Format("{0},", tech);
            }

            foreach (var equip in filters.EquipmentIds)
            {
                filter_MultipleEquipments += string.Format("OR Id = '{0}' ", equip);
            }
            filter_MultipleEquipments = filter_MultipleEquipments.Replace("(OR", "( ");

            if (!filter_MultipleEquipments.Contains("undefined"))
            {
                filter_MultipleEquipments += " )";
            }
            else
            {
                filter_MultipleEquipments = " ";
            }

            foreach (var equip in filters.ManufacturerIds)
            {
                filter_MultipleManufactures += string.Format("OR Id = '{0}' ", equip);
            }
            filter_MultipleManufactures = filter_MultipleManufactures.Replace("(OR", "( ");

            if (!filter_MultipleManufactures.Contains("undefined"))
            {
                filter_MultipleManufactures += " )";
            }
            else
            {
                filter_MultipleManufactures = " ";
            }

            if (!string.IsNullOrWhiteSpace(filters.SearchText))
            {
                filterColumntext = @",isNULL(_eqp.Name, '') +
								isNULL(_eqp.SKU,'') + 
                                isNULL(_eqpType.Name,'') + 
                                isNULL(sup.CompanyName,'') +
                                isNULL(manu.Name,'') + 
								isNULL(_eqpClass.Name,'')  FilterText  ";
                filtertext = string.Format(" Where FilterText like '%{0}%' ", filters.SearchText);
            }

            filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");
            filters.Start = !string.IsNullOrWhiteSpace(filters.Start)? Convert.ToDateTime(filters.Start).SetZeroHour().ToString(): filters.Start;
            filters.End = !string.IsNullOrWhiteSpace(filters.End) ? Convert.ToDateTime(filters.End).SetMaxHour().ToString() : filters.End;

            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filters.CompanyId,
                        filterColumntext,
                        filtertext,
                        filterByActiveStatus,
                        filterByEquipmentClass,
                        filterByEquipmentType,
                        filterByStockStatus,
                        filter_MultipleTechs,
                        orderquery,
                        filters.Start,
                        filters.End,
                        filter_MultipleEquipments,
                        filter_MultipleManufactures, //,
                         filter_TechnicianId);                               //(filters.PageNo - 1) * filters.PageSize,
                                                                    //filters.PageSize);
                                                                    //LogError(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "InventoryDataAccess-GetTechTransferListByFilters", Message = string.Format("{0} || {1}", "QLog", sqlQuery), TimeUtc = DateTime.Now });
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                _ErrorLogDataAccess.Insert(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "DA|Inventory|GetDetailedHistoryListByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                return null;
            }
        }

        public DataSet GetDetailedEquipmentTicketsByFilters(DetailedHistoryFilter filters)
        {
            string sqlQuery = @"
                                    select 
                                    invtech.LastUpdatedDate as Date,
                                    eq.Id,
                                    eq.EquipmentId,
                                    eq.Name,
                                    eq.SKU,
                                    invtech.Description,
                                    tk.Id as TicketNo,
                                    invtech.Type,
                                    CASE WHEN invtech.Type='Release'
	                                    THEN 0-invtech.Quantity
	                                    ELSE invtech.Quantity
                                    END as Quantity
                                    from InventoryTech invtech
                                    LEFT JOIN Employee em on em.UserId=invtech.TechnicianId
                                    LEFT JOIN Equipment eq on eq.EquipmentId=invtech.EquipmentId
                                    LEFT JOIN CustomerAppointmentEquipment cae on cae.Id=invtech.CustomerAppointmentEquipmentId
                                    LEFT JOIN Ticket tk on tk.TicketId=cae.AppointmentId
                                    where invtech.CompanyId='{0}' and eq.EquipmentClassId = 1 
                                    and ({7})
                                    and ({13})
                                    AND invtech.LastUpdatedDate BETWEEN '{9}' AND '{10}'
                                    AND Description LIKE '%ticket%'
                                    order by Date
                                    OFFSET {11} ROWS FETCH NEXT {12} ROWS ONLY

                                    select 
                                    COUNT(*) as Records
                                    from InventoryTech invtech
                                    LEFT JOIN Employee em on em.UserId=invtech.TechnicianId
                                    LEFT JOIN Equipment eq on eq.EquipmentId=invtech.EquipmentId
                                    LEFT JOIN CustomerAppointmentEquipment cae on cae.Id=invtech.CustomerAppointmentEquipmentId
                                    LEFT JOIN Ticket tk on tk.TicketId=cae.AppointmentId
                                    where invtech.CompanyId='{0}' and eq.EquipmentClassId = 1 
                                    and ({7})
                                    and ({13})
                                    AND invtech.LastUpdatedDate BETWEEN '{9}' AND '{10}'
                                    AND Description LIKE '%ticket%'";

            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string orderquery = "order by _cfd.Id desc";
            string filter_MultipleTechs = "AND";
            string filter_MultipleEquipments = "AND";

            foreach (var tech in filters.EmployeeIds)
            {
                filter_MultipleTechs += string.Format("OR em.UserId = '{0}' ", tech);
            }
            filter_MultipleTechs = filter_MultipleTechs.Replace("ANDOR", " ");

            foreach (var equip in filters.EquipmentIds)
            {
                filter_MultipleEquipments += string.Format("OR eq.EquipmentId = '{0}' ", equip);
            }
            filter_MultipleEquipments = filter_MultipleEquipments.Replace("ANDOR", " ");

            filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");
            filters.Start = !string.IsNullOrWhiteSpace(filters.Start) ? Convert.ToDateTime(filters.Start).SetZeroHour().ToString() : filters.Start;
            filters.End = !string.IsNullOrWhiteSpace(filters.End) ? Convert.ToDateTime(filters.End).SetMaxHour().ToString() : filters.End;

            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filters.CompanyId,
                        filterColumntext,
                        filtertext,
                        filterByActiveStatus,
                        filterByEquipmentClass,
                        filterByEquipmentType,
                        filterByStockStatus,
                        filter_MultipleTechs,
                        orderquery,
                        filters.Start,
                        filters.End,
                        (filters.PageNo-1)*filters.PageSize,
                        filters.PageSize,
                        filter_MultipleEquipments);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                //LogError(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "InventoryDataAccess-GetTechTransferListByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                _ErrorLogDataAccess.Insert(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "DA|Inventory|GetDetailedEquipmentTicketsByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                return null;
            }
        }

        public DataSet GetDetailedEquipmentTransfersByFiltersAIT(DetailedHistoryFilter filters)
        {

            string sqlQuery = @"

                select 
                (select te.FirstName + ' ' + te.LastName from Employee te where te.UserId=trf.TechnicianId) as Tech,
                (select te.FirstName + ' ' + te.LastName from Employee te where te.UserId=trf.ReceivedBy) as Recv,
                eq.Id,
                eq.Name,
                eq.SKU,
                trf.Quantity,
                trf.ReceivedDate
                from AssignedInventoryTechReceived trf LEFT JOIN Employee em on em.UserId=trf.TechnicianId LEFT JOIN Equipment eq on eq.EquipmentId=trf.EquipmentId
                where em.CompanyId='{0}' AND trf.ReceivedDate BETWEEN '{9}' AND '{10}' AND (trf.IsApprove=1 OR trf.IsReceived=1)
                and ( {7} )
                and ( {13} )
                union All
                select 
                (select te.FirstName + ' ' + te.LastName from Employee te where te.UserId=trf.TechnicianId) as Tech,
                (select te.FirstName + ' ' + te.LastName from Employee te where te.UserId=trf.ReceivedBy) as Recv,
                eq.Id,
                eq.Name,
                eq.SKU,
                trf.Quantity,
                trf.ReceivedDate
                from AssignedInventoryTechReceived trf LEFT JOIN Employee em on em.UserId=trf.ReceivedBy LEFT JOIN Equipment eq on eq.EquipmentId=trf.EquipmentId
                where em.CompanyId='{0}' AND trf.ReceivedDate BETWEEN '{9}' AND '{10}' AND (trf.IsApprove=1 OR trf.IsReceived=1)
                and ( {7} )
                and ( {13} )

                select SUM(cnt) as RowCnt FROM (
                select COUNT(*) as cnt
                from AssignedInventoryTechReceived trf LEFT JOIN Employee em on em.UserId=trf.TechnicianId LEFT JOIN Equipment eq on eq.EquipmentId=trf.EquipmentId
                where em.CompanyId='{0}' AND trf.ReceivedDate BETWEEN '{9}' AND '{10}' AND (trf.IsApprove=1 OR trf.IsReceived=1)
                and ( {7} )
                and ( {13} )
                union All
                select COUNT(*) as cnt
                from AssignedInventoryTechReceived trf LEFT JOIN Employee em on em.UserId=trf.ReceivedBy LEFT JOIN Equipment eq on eq.EquipmentId=trf.EquipmentId
                where em.CompanyId='{0}' AND trf.ReceivedDate BETWEEN '{9}' AND '{10}' AND (trf.IsApprove=1 OR trf.IsReceived=1)
                and ( {7} )
                and ( {13} )
                ) as tmpTable";



            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string orderquery = "order by _cfd.Id desc";
            string filter_MultipleTechs = "AND";
            string filter_MultipleEquipments = "AND";

            foreach (var tech in filters.EmployeeIds)
            {
                filter_MultipleTechs += string.Format("OR TechnicianId = '{0}' ", tech);
            }
            filter_MultipleTechs = filter_MultipleTechs.Replace("ANDOR", " ");

            foreach (var equip in filters.EquipmentIds)
            {
                filter_MultipleEquipments += string.Format("OR trf.EquipmentId= '{0}' ", equip);
            }
            filter_MultipleEquipments = filter_MultipleEquipments.Replace("ANDOR", " ");

            filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");

            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filters.CompanyId,
                        filterColumntext,
                        filtertext,
                        filterByActiveStatus,
                        filterByEquipmentClass,
                        filterByEquipmentType,
                        filterByStockStatus,
                        filter_MultipleTechs,
                        orderquery,
                        filters.Start,
                        filters.End,
                        (filters.PageNo - 1) * filters.PageSize,
                        filters.PageSize,
                        filter_MultipleEquipments);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                //LogError(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "InventoryDataAccess-GetTechTransferListByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                _ErrorLogDataAccess.Insert(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "DA|Inventory|GetDetailedEquipmentTransfersByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                return null;
            }
        }
        public DataSet GetDetailedEquipmentTransfersByFilters(DetailedHistoryFilter filters)
        {

            //string sqlQuery = @"

            //    Select *, 
            //    (select te.FirstName + ' ' + te.LastName from Employee te where te.UserId=invinner.TechnicianId) as Tech,
            //    eq.Name, eq.SKU, invinner.LastUpdatedDate 
            //    from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
            //    where ( {13} ) and invinner.Type='Release' 
            //    AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description NOT LIKE '%ticket%' AND ({7})
            //    union all
            //    Select *, 
            //    (select te.FirstName + ' ' + te.LastName from Employee te where te.UserId=invinner.TechnicianId) as Tech,
            //    eq.Name, eq.SKU, invinner.LastUpdatedDate 
            //    from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
            //    where ( {13} ) and invinner.Type='Add'  
            //    AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description NOT LIKE '%ticket%' AND ({7})

            //    select SUM(cnt) as RowCnt FROM (
            //    Select ISNULL(SUM(invinner.Quantity),0) as cnt from InventoryTech invinner where ( {13} ) and Type='Release' 
            //    AND LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description NOT LIKE '%ticket%' AND ({7})
            //    union all
            //    Select ISNULL(SUM(invinner.Quantity),0) as cnt from InventoryTech invinner where ( {13} ) and Type='Add'  
            //    AND LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description NOT LIKE '%ticket%' AND ({7})
            //    ) as tmpTable";

            //string sqlQuery = @"
                    
            //    select RCV.Quantity, RCV.EquipmentId, RCV.Name as Equipment, RCV.SKU, RCV.Type as [Add], 
            //    case when RCV.LocationId is not null and RCV.Type='Release' then (select te.UserName from Employee te where te.UserId=RCV.LocationId) else RCV.Tech end as RTech, 
            //    tmp.Type as Release, 
            //    case when tmp.LocationId is not null and tmp.Type='Add' then (select te.UserName from Employee te where te.UserId=tmp.LocationId) else tmp.Tech end as TTech, 
            //    DATEADD(MI, DATEDIFF(MI, GETUTCDATE(), GETDATE()), RCV.LastUpdatedDate) as RDate, 
            //    tmp.LastUpdatedDate as TDate, RCV.Description +' | '+ tmp.Description as Description 
            //    INTO #WHTrf
            //    from ( Select invinner.Quantity, invinner.TechnicianId,invinner.EquipmentId,invinner.Type,invinner.Description,
            //    (select te.FirstName + ' ' + te.LastName from Employee te where te.UserId=invinner.TechnicianId) as Tech,
            //    eq.Name, eq.SKU, invinner.LastUpdatedDate 
            //    from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
            //    where( {13} ) and invinner.Type='Add' 
            //    AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' 
            //    AND (Description NOT LIKE '%warehouse%' AND Description NOT LIKE '%ticket%' AND Description NOT LIKE '%purchase order%')
                
            //    ) as RCV
            //      full outer join 
            //    (
            //    Select invinner.Quantity, 
            //    invinner.TechnicianId,invinner.EquipmentId,invinner.Type,invinner.Description,
            //    (select te.FirstName + ' ' + te.LastName from Employee te where te.UserId=invinner.TechnicianId) as Tech,
            //    eq.Name, eq.SKU, invinner.LastUpdatedDate 
            //    from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
            //    where( {13} ) and invinner.Type='Release' 
            //    AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' 
            //    AND (Description NOT LIKE '%warehouse%' AND Description NOT LIKE '%ticket%')
                
            //    ) as tmp
            //    ON CONVERT(varchar,RCV.LastUpdatedDate,22)= CONVERT(varchar,tmp.LastUpdatedDate,22) and RCV.EquipmentId=tmp.EquipmentId AND RCV.Quantity=tmp.Quantity
            //    where {7}
            //    order by RCV.LastUpdatedDate desc

            //    Select invinner.Quantity, 
            //    invinner.EquipmentId,  eq.Name as Equipment, eq.SKU, invinner.Type as [Add], 
            //    (select te.FirstName + ' ' + te.LastName from Employee te where te.UserId=invinner.TechnicianId) as RTech, 'Release' as Release,
            //    'Warehouse' as TTech, DATEADD(MI, DATEDIFF(MI, GETUTCDATE(), GETDATE()), invinner.LastUpdatedDate) as RDate, 
            //    invinner.LastUpdatedDate TDate, invinner.Description
            //    from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
            //    where( {13} ) and invinner.Type='Add' 
            //    AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description LIKE '%warehouse%'
            //    AND ( {14} )
            //    union all
            //    Select invinner.Quantity, 
            //    invinner.EquipmentId,  eq.Name as Equipment, eq.SKU, 'Add' as [Add], 
            //     'Warehouse' as RTech, invinner.Type as Release, 
            //    (select te.FirstName + ' ' + te.LastName from Employee te where te.UserId=invinner.TechnicianId) as TTech, 
            //    DATEADD(MI, DATEDIFF(MI, GETUTCDATE(), GETDATE()), invinner.LastUpdatedDate) as RDate, 
            //    invinner.LastUpdatedDate TDate, invinner.Description
            //    from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
            //    where( {13} ) and invinner.Type='Release' 
            //    AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description LIKE '%warehouse%'
            //    AND ( {14} )
            //    union all 
            //    Select invinner.Quantity, 
            //    invinner.EquipmentId,  eq.Name as Equipment, eq.SKU, invinner.Type as [Add], 
            //    (select te.FirstName + ' ' + te.LastName from Employee te where te.UserId=invinner.TechnicianId) as RTech, 'Release' as Release,
            //    'Purchase Order' as TTech, DATEADD(MI, DATEDIFF(MI, GETUTCDATE(), GETDATE()), invinner.LastUpdatedDate) as RDate, 
            //    invinner.LastUpdatedDate TDate, invinner.Description
            //    from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
            //    where( {13} ) and invinner.Type='Add' 
            //    AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description LIKE '%purchase order%'
            //    AND ( {14} )
            //    union all
            //    select * from #WHTrf

            //    DROP TABLE #WHTrf

            //";

            //string sqlQuery = @"

            //    select RCV.Quantity, RCV.EquipmentId, RCV.Name as Equipment, RCV.SKU, RCV.Type as [Add], 
            //    case when RCV.LocationId is not null and RCV.Type='Release' then (select te.UserName from Employee te where te.UserId=RCV.LocationId) else RCV.Tech end as RTech, 
            //    tmp.Type as Release, 
            //    case when tmp.LocationId is not null and tmp.Type='Add' then (select te.UserName from Employee te where te.UserId=tmp.LocationId) else tmp.Tech end as TTech, 
            //    DATEADD(MI, DATEDIFF(MI, GETUTCDATE(), GETDATE()), RCV.LastUpdatedDate) as RDate, 
            //    tmp.LastUpdatedDate as TDate, RCV.Description +' | '+ tmp.Description as Description

            //    INTO #WHTrf
            //    from ( Select invWH.LocationId, invinner.Quantity, invinner.TechnicianId,invinner.EquipmentId,invinner.Type,invinner.Description,
            //    (select case when te.IsLocation=1 then te.UserName else te.FirstName + ' ' + te.LastName end from Employee te where te.UserId=invinner.TechnicianId) as Tech,
            //    eq.Name, eq.SKU, invinner.LastUpdatedDate 
            //    from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
            //    left join InventoryWarehouse invWH ON CONVERT(varchar,invinner.LastUpdatedDate,22)= CONVERT(varchar,invWH.LastUpdatedDate,22) 
            //    and invinner.EquipmentId=invWH.EquipmentId AND invinner.Quantity=invWH.Quantity
            //    where( {13} ) and (invinner.Type='Add' or invWH.Type='Add') 
            //    AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' 
            //    AND (invinner.Description NOT LIKE '%ticket%')

            //    ) as RCV
            //      full outer join 
            //    (
            //    Select invWH.LocationId, invinner.Quantity, 
            //    invinner.TechnicianId,invinner.EquipmentId,invinner.Type,invinner.Description,
            //    (select case when te.IsLocation=1 then te.UserName else te.FirstName + ' ' + te.LastName end from Employee te where te.UserId=invinner.TechnicianId) as Tech,
            //    eq.Name, eq.SKU, invinner.LastUpdatedDate 
            //    from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
            //    left join InventoryWarehouse invWH ON CONVERT(varchar,invinner.LastUpdatedDate,22)= CONVERT(varchar,invWH.LastUpdatedDate,22) 
            //    and invinner.EquipmentId=invWH.EquipmentId AND invinner.Quantity=invWH.Quantity
            //    where( {13} ) and (invinner.Type='Release' or invWH.Type='Release' ) 
            //    AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' 
            //    AND (invinner.Description NOT LIKE '%ticket%')

            //    ) as tmp
            //    ON CONVERT(varchar,RCV.LastUpdatedDate,22)= CONVERT(varchar,tmp.LastUpdatedDate,22) and RCV.EquipmentId=tmp.EquipmentId AND RCV.Quantity=tmp.Quantity
            //    where {7}
            //    order by RCV.LastUpdatedDate desc

            //    select * from #WHTrf

            //    DROP TABLE #WHTrf

            //";

            string sqlQuery = @"
                    
               

                    
                                          WITH RankedResults AS (
                                        SELECT
                                            assinITR.Quantity,
                                            assinITR.EquipmentId,
                                            eq.Name as Equipment,
                                            eq.SKU,
                                            'Add' as [Add],
                                            (SELECT CASE WHEN te.IsLocation=1 THEN te.UserName ELSE te.FirstName + ' ' + te.LastName END FROM Employee te WHERE te.UserId=assinITR.ReceivedBy) AS RTech,
                                            'Release' AS Release,
                                            COALESCE((SELECT CASE WHEN te.IsLocation=1 THEN te.UserName ELSE te.FirstName + ' ' + te.LastName END FROM Employee te WHERE te.UserId=assinITR.TechnicianId),'Purchase Order') AS TTech,
                                            assinITR.ReceivedDate AS RDate,
                                            assinITR.CreatedDate AS TDate,
                                            '' as Description,
                                          ROW_NUMBER() OVER (PARTITION BY assinITR.Quantity, assinITR.EquipmentId, eq.Name, eq.SKU, assinITR.ReceivedDate ORDER BY assinITR.CreatedDate) AS RowNum,
                                            assinITR.ReqSrc as ReqSrc,
										    assinITR.TechnicianId as TechnicianId
									   FROM
                                            AssignedInventoryTechReceived assinITR
                                        LEFT JOIN Equipment eq ON eq.EquipmentId = assinITR.EquipmentId
                                    
                                        WHERE
 						{13}
                                                                                     
                                            AND ReceivedDate BETWEEN '{9}' AND '{10}' 
                                            AND IsApprove=1
											
               --                                  
		                               and  ( {7} )
                                    )
                                    SELECT 
								--	TechnicianId,
								--	ReqSrc,
                                        Quantity,
                                        EquipmentId,
                                        Equipment,
                                        SKU,
                                        [Add],
                                        RTech,
                                        [Release],
                                        TTech,
                                        RDate,
                                        TDate,
                                        '' as Description
                                    FROM RankedResults

                                   WHERE RowNum = 1 
									
                                    ORDER BY RDate desc;

                

            

            

            ";

            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string orderquery = "order by _cfd.Id desc";
            string filter_MultipleTechs1 = "AND";
            string filter_MultipleTechs2 = "AND";
            string filter_MultipleEquipments = "AND";

            foreach (var tech in filters.EmployeeIds)
            {
                filter_MultipleTechs1 += string.Format("OR assinITR.TechnicianId = '{0}' OR assinITR.ReceivedBy= '{0}' ", tech);
                filter_MultipleTechs2 += string.Format("OR assinITR.TechnicianId = '{0}' ", tech);
            }
            filter_MultipleTechs1 = filter_MultipleTechs1.Replace("ANDOR", " ");
            filter_MultipleTechs2 = filter_MultipleTechs2.Replace("ANDOR", " ");


            foreach (var equip in filters.EquipmentIds)
            {
                filter_MultipleEquipments += string.Format("OR assinITR.EquipmentId= '{0}' ", equip);
            }
            filter_MultipleEquipments = filter_MultipleEquipments.Replace("ANDOR", " ");

            filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");
            filters.Start = !string.IsNullOrWhiteSpace(filters.Start) ? Convert.ToDateTime(filters.Start).SetZeroHour().ToString() : filters.Start;
            filters.End = !string.IsNullOrWhiteSpace(filters.End) ? Convert.ToDateTime(filters.End).SetMaxHour().ToString() : filters.End;

            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filters.CompanyId,
                        filterColumntext,
                        filtertext,
                        filterByActiveStatus,
                        filterByEquipmentClass,
                        filterByEquipmentType,
                        filterByStockStatus,
                        filter_MultipleTechs1,
                        orderquery,
                        filters.Start,
                        filters.End,
                        (filters.PageNo - 1) * filters.PageSize,
                        filters.PageSize,
                        filter_MultipleEquipments,
                        filter_MultipleTechs2);

               

                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                //LogError(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "InventoryDataAccess-GetTechTransferListByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                _ErrorLogDataAccess.Insert(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "DA|Inventory|GetDetailedEquipmentTransfersByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                return null;
            }
        }
        public DataSet GetDetailedEquipmentTransfersByFilters_old(DetailedHistoryFilter filters)
        {

            //string sqlQuery = @"

            //    Select *, 
            //    (select te.FirstName + ' ' + te.LastName from Employee te where te.UserId=invinner.TechnicianId) as Tech,
            //    eq.Name, eq.SKU, invinner.LastUpdatedDate 
            //    from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
            //    where ( {13} ) and invinner.Type='Release' 
            //    AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description NOT LIKE '%ticket%' AND ({7})
            //    union all
            //    Select *, 
            //    (select te.FirstName + ' ' + te.LastName from Employee te where te.UserId=invinner.TechnicianId) as Tech,
            //    eq.Name, eq.SKU, invinner.LastUpdatedDate 
            //    from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
            //    where ( {13} ) and invinner.Type='Add'  
            //    AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description NOT LIKE '%ticket%' AND ({7})

            //    select SUM(cnt) as RowCnt FROM (
            //    Select ISNULL(SUM(invinner.Quantity),0) as cnt from InventoryTech invinner where ( {13} ) and Type='Release' 
            //    AND LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description NOT LIKE '%ticket%' AND ({7})
            //    union all
            //    Select ISNULL(SUM(invinner.Quantity),0) as cnt from InventoryTech invinner where ( {13} ) and Type='Add'  
            //    AND LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description NOT LIKE '%ticket%' AND ({7})
            //    ) as tmpTable";

            //string sqlQuery = @"

            //    select RCV.Quantity, RCV.EquipmentId, RCV.Name as Equipment, RCV.SKU, RCV.Type as [Add], 
            //    case when RCV.LocationId is not null and RCV.Type='Release' then (select te.UserName from Employee te where te.UserId=RCV.LocationId) else RCV.Tech end as RTech, 
            //    tmp.Type as Release, 
            //    case when tmp.LocationId is not null and tmp.Type='Add' then (select te.UserName from Employee te where te.UserId=tmp.LocationId) else tmp.Tech end as TTech, 
            //    DATEADD(MI, DATEDIFF(MI, GETUTCDATE(), GETDATE()), RCV.LastUpdatedDate) as RDate, 
            //    tmp.LastUpdatedDate as TDate, RCV.Description +' | '+ tmp.Description as Description 
            //    INTO #WHTrf
            //    from ( Select invinner.Quantity, invinner.TechnicianId,invinner.EquipmentId,invinner.Type,invinner.Description,
            //    (select te.FirstName + ' ' + te.LastName from Employee te where te.UserId=invinner.TechnicianId) as Tech,
            //    eq.Name, eq.SKU, invinner.LastUpdatedDate 
            //    from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
            //    where( {13} ) and invinner.Type='Add' 
            //    AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' 
            //    AND (Description NOT LIKE '%warehouse%' AND Description NOT LIKE '%ticket%' AND Description NOT LIKE '%purchase order%')

            //    ) as RCV
            //      full outer join 
            //    (
            //    Select invinner.Quantity, 
            //    invinner.TechnicianId,invinner.EquipmentId,invinner.Type,invinner.Description,
            //    (select te.FirstName + ' ' + te.LastName from Employee te where te.UserId=invinner.TechnicianId) as Tech,
            //    eq.Name, eq.SKU, invinner.LastUpdatedDate 
            //    from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
            //    where( {13} ) and invinner.Type='Release' 
            //    AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' 
            //    AND (Description NOT LIKE '%warehouse%' AND Description NOT LIKE '%ticket%')

            //    ) as tmp
            //    ON CONVERT(varchar,RCV.LastUpdatedDate,22)= CONVERT(varchar,tmp.LastUpdatedDate,22) and RCV.EquipmentId=tmp.EquipmentId AND RCV.Quantity=tmp.Quantity
            //    where {7}
            //    order by RCV.LastUpdatedDate desc

            //    Select invinner.Quantity, 
            //    invinner.EquipmentId,  eq.Name as Equipment, eq.SKU, invinner.Type as [Add], 
            //    (select te.FirstName + ' ' + te.LastName from Employee te where te.UserId=invinner.TechnicianId) as RTech, 'Release' as Release,
            //    'Warehouse' as TTech, DATEADD(MI, DATEDIFF(MI, GETUTCDATE(), GETDATE()), invinner.LastUpdatedDate) as RDate, 
            //    invinner.LastUpdatedDate TDate, invinner.Description
            //    from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
            //    where( {13} ) and invinner.Type='Add' 
            //    AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description LIKE '%warehouse%'
            //    AND ( {14} )
            //    union all
            //    Select invinner.Quantity, 
            //    invinner.EquipmentId,  eq.Name as Equipment, eq.SKU, 'Add' as [Add], 
            //     'Warehouse' as RTech, invinner.Type as Release, 
            //    (select te.FirstName + ' ' + te.LastName from Employee te where te.UserId=invinner.TechnicianId) as TTech, 
            //    DATEADD(MI, DATEDIFF(MI, GETUTCDATE(), GETDATE()), invinner.LastUpdatedDate) as RDate, 
            //    invinner.LastUpdatedDate TDate, invinner.Description
            //    from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
            //    where( {13} ) and invinner.Type='Release' 
            //    AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description LIKE '%warehouse%'
            //    AND ( {14} )
            //    union all 
            //    Select invinner.Quantity, 
            //    invinner.EquipmentId,  eq.Name as Equipment, eq.SKU, invinner.Type as [Add], 
            //    (select te.FirstName + ' ' + te.LastName from Employee te where te.UserId=invinner.TechnicianId) as RTech, 'Release' as Release,
            //    'Purchase Order' as TTech, DATEADD(MI, DATEDIFF(MI, GETUTCDATE(), GETDATE()), invinner.LastUpdatedDate) as RDate, 
            //    invinner.LastUpdatedDate TDate, invinner.Description
            //    from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
            //    where( {13} ) and invinner.Type='Add' 
            //    AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description LIKE '%purchase order%'
            //    AND ( {14} )
            //    union all
            //    select * from #WHTrf

            //    DROP TABLE #WHTrf

            //";

            //string sqlQuery = @"

            //    select RCV.Quantity, RCV.EquipmentId, RCV.Name as Equipment, RCV.SKU, RCV.Type as [Add], 
            //    case when RCV.LocationId is not null and RCV.Type='Release' then (select te.UserName from Employee te where te.UserId=RCV.LocationId) else RCV.Tech end as RTech, 
            //    tmp.Type as Release, 
            //    case when tmp.LocationId is not null and tmp.Type='Add' then (select te.UserName from Employee te where te.UserId=tmp.LocationId) else tmp.Tech end as TTech, 
            //    DATEADD(MI, DATEDIFF(MI, GETUTCDATE(), GETDATE()), RCV.LastUpdatedDate) as RDate, 
            //    tmp.LastUpdatedDate as TDate, RCV.Description +' | '+ tmp.Description as Description

            //    INTO #WHTrf
            //    from ( Select invWH.LocationId, invinner.Quantity, invinner.TechnicianId,invinner.EquipmentId,invinner.Type,invinner.Description,
            //    (select case when te.IsLocation=1 then te.UserName else te.FirstName + ' ' + te.LastName end from Employee te where te.UserId=invinner.TechnicianId) as Tech,
            //    eq.Name, eq.SKU, invinner.LastUpdatedDate 
            //    from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
            //    left join InventoryWarehouse invWH ON CONVERT(varchar,invinner.LastUpdatedDate,22)= CONVERT(varchar,invWH.LastUpdatedDate,22) 
            //    and invinner.EquipmentId=invWH.EquipmentId AND invinner.Quantity=invWH.Quantity
            //    where( {13} ) and (invinner.Type='Add' or invWH.Type='Add') 
            //    AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' 
            //    AND (invinner.Description NOT LIKE '%ticket%')

            //    ) as RCV
            //      full outer join 
            //    (
            //    Select invWH.LocationId, invinner.Quantity, 
            //    invinner.TechnicianId,invinner.EquipmentId,invinner.Type,invinner.Description,
            //    (select case when te.IsLocation=1 then te.UserName else te.FirstName + ' ' + te.LastName end from Employee te where te.UserId=invinner.TechnicianId) as Tech,
            //    eq.Name, eq.SKU, invinner.LastUpdatedDate 
            //    from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
            //    left join InventoryWarehouse invWH ON CONVERT(varchar,invinner.LastUpdatedDate,22)= CONVERT(varchar,invWH.LastUpdatedDate,22) 
            //    and invinner.EquipmentId=invWH.EquipmentId AND invinner.Quantity=invWH.Quantity
            //    where( {13} ) and (invinner.Type='Release' or invWH.Type='Release' ) 
            //    AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' 
            //    AND (invinner.Description NOT LIKE '%ticket%')

            //    ) as tmp
            //    ON CONVERT(varchar,RCV.LastUpdatedDate,22)= CONVERT(varchar,tmp.LastUpdatedDate,22) and RCV.EquipmentId=tmp.EquipmentId AND RCV.Quantity=tmp.Quantity
            //    where {7}
            //    order by RCV.LastUpdatedDate desc

            //    select * from #WHTrf

            //    DROP TABLE #WHTrf

            //";

            string sqlQuery = @"
                    
               
                                        WITH DistinctRows AS (
                            SELECT DISTINCT
                                RCV.Quantity,
                                RCV.EquipmentId,
                                RCV.Name AS Equipment,
                                RCV.SKU,
                                RCV.Type AS [Add],
                                CASE
                                    WHEN RCV.LocationId IS NOT NULL AND RCV.Type = 'Release' THEN
                                        (SELECT te.UserName FROM Employee te WHERE te.UserId = RCV.LocationId)
                                    ELSE RCV.Tech
                                END AS RTech,
                                tmp.Type AS Release,
                                 CASE
                                    WHEN tmp.LocationId IS NOT NULL AND tmp.Type = 'Add' THEN
                                       COALESCE((SELECT te.UserName FROM Employee te WHERE te.UserId = tmp.LocationId), 'Purchase Order')
                                    ELSE COALESCE(tmp.Tech, 'Purchase Order')
                                END AS TTech,
                                DATEADD(MI, DATEDIFF(MI, GETUTCDATE(), GETDATE()), RCV.LastUpdatedDate) AS RDate,
                                tmp.LastUpdatedDate AS TDate,
                                RCV.Description + ' | ' + tmp.Description AS Description
                            FROM (
                                SELECT DISTINCT
                                    invWH.LocationId,
                                    invinner.Quantity,
                                    invinner.TechnicianId,
                                    invinner.EquipmentId,
                                    invinner.Type,
                                    invinner.Description,
                                    (
                                        SELECT
                                            CASE
                                                WHEN te.IsLocation = 1 THEN te.UserName
                                                ELSE te.FirstName + ' ' + te.LastName
                                            END
                                        FROM Employee te
                                        WHERE te.UserId = invinner.TechnicianId
                                    ) AS Tech,
                                    eq.Name,
                                    eq.SKU,
                                    invinner.LastUpdatedDate
                                FROM InventoryTech invinner
                                LEFT JOIN Equipment eq ON eq.EquipmentId = invinner.EquipmentId
                                LEFT JOIN InventoryWarehouse invWH ON CONVERT(VARCHAR, invinner.LastUpdatedDate, 22) = CONVERT(VARCHAR, invWH.LastUpdatedDate, 22)
                                    AND invinner.EquipmentId = invWH.EquipmentId AND invinner.Quantity = invWH.Quantity
                                WHERE
                                   ( {13})
                                    AND (invinner.Type = 'Add' OR invWH.Type = 'Add')
                                    AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' 
                                    AND (invinner.Description NOT LIKE '%ticket%')
                            ) AS RCV
                            FULL OUTER JOIN (
                                SELECT DISTINCT
                                    invWH.LocationId,
                                    invinner.Quantity,
                                    invinner.TechnicianId,
                                    invinner.EquipmentId,
                                    invinner.Type,
                                    invinner.Description,
                                    (
                                        SELECT
                                            CASE
                                                WHEN te.IsLocation = 1 THEN te.UserName
                                                ELSE te.FirstName + ' ' + te.LastName
                                            END
                                        FROM Employee te
                                        WHERE te.UserId = invinner.TechnicianId
                                    ) AS Tech,
                                    eq.Name,
                                    eq.SKU,
                                    invinner.LastUpdatedDate
                                FROM InventoryTech invinner
                                LEFT JOIN Equipment eq ON eq.EquipmentId = invinner.EquipmentId
                                LEFT JOIN InventoryWarehouse invWH ON CONVERT(VARCHAR, invinner.LastUpdatedDate, 22) = CONVERT(VARCHAR, invWH.LastUpdatedDate, 22)
                                    AND invinner.EquipmentId = invWH.EquipmentId AND invinner.Quantity = invWH.Quantity
                                WHERE
                                    ( {13} )
                                    AND (invinner.Type = 'Release' OR invWH.Type = 'Release')
                                    AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' 
                                    AND (invinner.Description NOT LIKE '%ticket%')
                            ) AS tmp ON CONVERT(VARCHAR, RCV.LastUpdatedDate, 22) = CONVERT(VARCHAR, tmp.LastUpdatedDate, 22)
                                AND RCV.EquipmentId = tmp.EquipmentId AND RCV.Quantity = tmp.Quantity
                            WHERE
                                {7}
                        )
                        SELECT *
                        FROM DistinctRows
                        ORDER BY RDate desc

            ";

            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string orderquery = "order by _cfd.Id desc";
            string filter_MultipleTechs1 = "AND";
            string filter_MultipleTechs2 = "AND";
            string filter_MultipleEquipments = "AND";

            foreach (var tech in filters.EmployeeIds)
            {
                filter_MultipleTechs1 += string.Format("OR RCV.TechnicianId = '{0}' OR tmp.TechnicianId= '{0}' ", tech);
                filter_MultipleTechs2 += string.Format("OR invinner.TechnicianId = '{0}' ", tech);
            }
            filter_MultipleTechs1 = filter_MultipleTechs1.Replace("ANDOR", " ");
            filter_MultipleTechs2 = filter_MultipleTechs2.Replace("ANDOR", " ");
            

            foreach (var equip in filters.EquipmentIds)
            {
                filter_MultipleEquipments += string.Format("OR invinner.EquipmentId= '{0}' ", equip);
            }
            filter_MultipleEquipments = filter_MultipleEquipments.Replace("ANDOR", " ");

            filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");
            filters.Start = !string.IsNullOrWhiteSpace(filters.Start) ? Convert.ToDateTime(filters.Start).SetZeroHour().ToString() : filters.Start;
            filters.End = !string.IsNullOrWhiteSpace(filters.End) ? Convert.ToDateTime(filters.End).SetMaxHour().ToString() : filters.End;

            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filters.CompanyId,
                        filterColumntext,
                        filtertext,
                        filterByActiveStatus,
                        filterByEquipmentClass,
                        filterByEquipmentType,
                        filterByStockStatus,
                        filter_MultipleTechs1,
                        orderquery,
                        filters.Start,
                        filters.End,
                        (filters.PageNo - 1) * filters.PageSize,
                        filters.PageSize,
                        filter_MultipleEquipments,
                        filter_MultipleTechs2);

                //sqlQuery = @"WITH DistinctRows AS (
                //            SELECT DISTINCT
                //                RCV.Quantity,
                //                RCV.EquipmentId,
                //                RCV.Name AS Equipment,
                //                RCV.SKU,
                //                RCV.Type AS [Add],
                //                CASE
                //                    WHEN RCV.LocationId IS NOT NULL AND RCV.Type = 'Release' THEN
                //                        (SELECT te.UserName FROM Employee te WHERE te.UserId = RCV.LocationId)
                //                    ELSE RCV.Tech
                //                END AS RTech,
                //                tmp.Type AS Release,
                //                CASE
                //                    WHEN tmp.LocationId IS NOT NULL AND tmp.Type = 'Add' THEN
                //                        (SELECT te.UserName FROM Employee te WHERE te.UserId = tmp.LocationId)
                //                    ELSE tmp.Tech
                //                END AS TTech,
                //                DATEADD(MI, DATEDIFF(MI, GETUTCDATE(), GETDATE()), RCV.LastUpdatedDate) AS RDate,
                //                tmp.LastUpdatedDate AS TDate,
                //                RCV.Description + ' | ' + tmp.Description AS Description
                //            FROM (
                //                SELECT DISTINCT
                //                    invWH.LocationId,
                //                    invinner.Quantity,
                //                    invinner.TechnicianId,
                //                    invinner.EquipmentId,
                //                    invinner.Type,
                //                    invinner.Description,
                //                    (
                //                        SELECT
                //                            CASE
                //                                WHEN te.IsLocation = 1 THEN te.UserName
                //                                ELSE te.FirstName + ' ' + te.LastName
                //                            END
                //                        FROM Employee te
                //                        WHERE te.UserId = invinner.TechnicianId
                //                    ) AS Tech,
                //                    eq.Name,
                //                    eq.SKU,
                //                    invinner.LastUpdatedDate
                //                FROM InventoryTech invinner
                //                LEFT JOIN Equipment eq ON eq.EquipmentId = invinner.EquipmentId
                //                LEFT JOIN InventoryWarehouse invWH ON CONVERT(VARCHAR, invinner.LastUpdatedDate, 22) = CONVERT(VARCHAR, invWH.LastUpdatedDate, 22)
                //                    AND invinner.EquipmentId = invWH.EquipmentId AND invinner.Quantity = invWH.Quantity
                //                WHERE
                //                    invinner.EquipmentId = 'a0093399-6139-4cd1-8e04-ad2ac112eef3'
                //                    AND (invinner.Type = 'Add' OR invWH.Type = 'Add')
                //                    AND invinner.LastUpdatedDate BETWEEN '11/01/2023 12:00:00 AM' AND '01/30/2024 11:59:59 PM'
                //                    AND (invinner.Description NOT LIKE '%ticket%')
                //            ) AS RCV
                //            FULL OUTER JOIN (
                //                SELECT DISTINCT
                //                    invWH.LocationId,
                //                    invinner.Quantity,
                //                    invinner.TechnicianId,
                //                    invinner.EquipmentId,
                //                    invinner.Type,
                //                    invinner.Description,
                //                    (
                //                        SELECT
                //                            CASE
                //                                WHEN te.IsLocation = 1 THEN te.UserName
                //                                ELSE te.FirstName + ' ' + te.LastName
                //                            END
                //                        FROM Employee te
                //                        WHERE te.UserId = invinner.TechnicianId
                //                    ) AS Tech,
                //                    eq.Name,
                //                    eq.SKU,
                //                    invinner.LastUpdatedDate
                //                FROM InventoryTech invinner
                //                LEFT JOIN Equipment eq ON eq.EquipmentId = invinner.EquipmentId
                //                LEFT JOIN InventoryWarehouse invWH ON CONVERT(VARCHAR, invinner.LastUpdatedDate, 22) = CONVERT(VARCHAR, invWH.LastUpdatedDate, 22)
                //                    AND invinner.EquipmentId = invWH.EquipmentId AND invinner.Quantity = invWH.Quantity
                //                WHERE
                //                    invinner.EquipmentId = 'a0093399-6139-4cd1-8e04-ad2ac112eef3'
                //                    AND (invinner.Type = 'Release' OR invWH.Type = 'Release')
                //                    AND invinner.LastUpdatedDate BETWEEN '11/01/2023 12:00:00 AM' AND '01/30/2024 11:59:59 PM'
                //                    AND (invinner.Description NOT LIKE '%ticket%')
                //            ) AS tmp ON CONVERT(VARCHAR, RCV.LastUpdatedDate, 22) = CONVERT(VARCHAR, tmp.LastUpdatedDate, 22)
                //                AND RCV.EquipmentId = tmp.EquipmentId AND RCV.Quantity = tmp.Quantity
                //            WHERE
                //                RCV.TechnicianId = 'a3f4b095-655c-42ee-9db1-73f85def54a8' OR tmp.TechnicianId = 'a3f4b095-655c-42ee-9db1-73f85def54a8'
                //        )
                //        SELECT *
                //        FROM DistinctRows
                //        ORDER BY RDate DESC;

                //            ";

                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd); 
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                //LogError(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "InventoryDataAccess-GetTechTransferListByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                _ErrorLogDataAccess.Insert(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "DA|Inventory|GetDetailedEquipmentTransfersByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                return null;
            }
        }
        public DataSet GetDetailedEquipmentTransfersByFiltersWH(DetailedHistoryFilter filters)
        {

            string sqlQuery = @"
                    
                Select invinner.Quantity, 
                invinner.EquipmentId,  eq.Name as Equipment, eq.SKU, invinner.Type as [Add], 
                (select te.FirstName + ' ' + te.LastName from Employee te where te.UserId=invinner.TechnicianId) as RTech, 'Release' as Release,
                'Warehouse' as TTech,invinner.LastUpdatedDate as RDate, 
                invinner.LastUpdatedDate TDate, invinner.Description
                from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
                where( {13} ) and invinner.Type='Add' 
                AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description LIKE '%warehouse%'
                
                union all
                Select invinner.Quantity, 
                invinner.EquipmentId,  eq.Name as Equipment, eq.SKU, 'Add' as [Add], 
                 'Warehouse' as RTech, invinner.Type as Release, 
                (select te.FirstName + ' ' + te.LastName from Employee te where te.UserId=invinner.TechnicianId) as TTech, 
                invinner.LastUpdatedDate as RDate, 
                invinner.LastUpdatedDate TDate, invinner.Description
                from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
                where( {13} ) and invinner.Type='Release' 
                AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description LIKE '%warehouse%'
                

            ";

            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string orderquery = "order by _cfd.Id desc";
            string filter_MultipleTechs = "AND";
            string filter_MultipleEquipments = "AND";

            foreach (var tech in filters.EmployeeIds)
            {
                filter_MultipleTechs += string.Format("OR invinner.TechnicianId = '{0}' ", tech);
            }
            filter_MultipleTechs = filter_MultipleTechs.Replace("ANDOR", " ");

            foreach (var equip in filters.EquipmentIds)
            {
                filter_MultipleEquipments += string.Format("OR invinner.EquipmentId= '{0}' ", equip);
            }
            filter_MultipleEquipments = filter_MultipleEquipments.Replace("ANDOR", " ");

            filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");

            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filters.CompanyId,
                        filterColumntext,
                        filtertext,
                        filterByActiveStatus,
                        filterByEquipmentClass,
                        filterByEquipmentType,
                        filterByStockStatus,
                        filter_MultipleTechs,
                        orderquery,
                        filters.Start,
                        filters.End,
                        (filters.PageNo - 1) * filters.PageSize,
                        filters.PageSize,
                        filter_MultipleEquipments);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                //LogError(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "InventoryDataAccess-GetTechTransferListByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                _ErrorLogDataAccess.Insert(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "DA|Inventory|GetDetailedEquipmentTransfersByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                return null;
            }
        }

        public DataSet GetTechTransferListByFilters(TechTransferFilter filters)
        {
            string sqlQuery = @"
                                -- Transfer Inventory Tech

                                    select rec.Id, rec.TechnicianId, rec.EquipmentId, rec.IsReceived, rec.ReceivedDate, rec.ReceivedBy, eq.Name as Name, rec.Quantity, rec.IsApprove, 'Transfer' as [Type], rec.IsDecline,
                                    (select case when _sentby.IsLocation=1 then _sentby.UserName else _sentby.FirstName + ' ' + _sentby.LastName end from Employee _sentby where _sentby.UserId = rec.TechnicianId) as SentByName,
                                    (select case when _recby.IsLocation=1 then _recby.UserName else _recby.FirstName + ' ' + _recby.LastName end from Employee _recby where _recby.UserId = rec.ReceivedBy) as ReceivedByName,
                                        ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=eq.EquipmentId and Type='Add')
										- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=eq.EquipmentId and Type='Release')
										- isnull((select sum(Quantity) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = eq.EquipmentId and b.IsApprove = 0 and b.IsDecline = 0 ),0)) as WHStock,
                                  -- (SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Add' AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId) - (SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Release' AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId) AS TotalQuantity,
                                      ABS(((SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Add' 
  AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId) - (SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Release'
  AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId)
-isnull((select sum(Quantity) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = eq.EquipmentId and b.IsApprove = 0 and b.IsDecline = 0 ),0))
 ) AS TotalQuantity,
                                   -- (select _sentby.UserId from Employee _sentby where _sentby.UserId = rec.TechnicianId) as SentById,
                                (select _sentby.IsLocation from Employee _sentby where _sentby.UserId = rec.TechnicianId) as IsLocation,

                                    rec.CreatedDate,
                                    eq.SKU
                                    INTO #TrfData
									from AssignedInventoryTechReceived rec
                                    left join Equipment eq on eq.EquipmentId = rec.EquipmentId
                                    where rec.TechnicianId='{7}'
                                    

                                    select Id, TechnicianId, EquipmentId, IsReceived, ReceivedBy, ReceivedDate, CreatedDate,
                                    Name, SKU, Quantity, ReceivedByName, TotalQuantity, IsApprove, Type, IsDecline, WHStock ,IsLocation from #TrfData
                                    {14}
                                    order by CreatedDate desc
                                    OFFSET {11} ROWS FETCH NEXT {13} ROWS ONLY

                                    select COUNT(*)
									from #TrfData
                                    {14}

                                    select DISTINCT ReceivedBy as Value, ReceivedByName as Text from #TrfData
                                    order by ReceivedByName  asc
                                
                                -- Receive Inventory Tech

                                select distinct rec.Id, rec.TechnicianId, rec.EquipmentId, rec.IsReceived, rec.ReceivedDate, rec.ReceivedBy, _eq.Name as Name, rec.Quantity, rec.IsApprove, 'Approve' as [Type], rec.IsDecline,
                                    (select case when _sentby.IsLocation=1 then _sentby.UserName else _sentby.FirstName + ' ' + _sentby.LastName end from Employee _sentby where _sentby.UserId = rec.TechnicianId) as SentByName,                                    
                                    (select case when recby.IsLocation=1 then recby.UserName else recby.FirstName + ' ' + recby.LastName end from Employee recby where recby.UserId = rec.ReceivedBy) as ReceivedByName,
                                   IIF(rec.TechnicianId = '22222222-2222-2222-2222-222222222222',(Select  ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eq.EquipmentId and Type='Add' AND invinner.LocationId = '22222222-2222-2222-2222-222222222222')
										- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=_eq.EquipmentId and Type='Release' AND invinner2.LocationId = '22222222-2222-2222-2222-222222222222')
										,
										
										(Select  ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eq.EquipmentId and Type='Add' AND invinner.LocationId <> '22222222-2222-2222-2222-222222222222')
									
										- isnull((select sum(Quantity) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = _eq.EquipmentId ),0)
										
										
										) as WHStock,
                                 ---  (SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Add' AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId) - (SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Release' AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId) AS TotalQuantity,
ABS (((SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Add' 
  AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId) - (SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Release'
  AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId)
-isnull((select sum(Quantity) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = _eq.EquipmentId and b.IsApprove = 0 and b.IsDecline = 0 ),0))
)  AS TotalQuantity,
                                    --(select _sentby.UserId from Employee _sentby where _sentby.UserId = rec.TechnicianId) as SentById,
                             (select _sentby.IsLocation from Employee _sentby where _sentby.UserId = rec.TechnicianId) as IsLocation,

                                    rec.CreatedDate,
                                    _eq.SKU,
                                    (CASE WHEN IsApprove!=1 AND IsDecline!=1 THEN 0 ELSE 1 END) as NeedAction
                                    INTO #RcvData
									from AssignedInventoryTechReceived rec
                                    left join Equipment _eq on _eq.EquipmentId = rec.EquipmentId
                                    where rec.ReceivedBy='{7}'
                                    

                                    select Id, TechnicianId, EquipmentId, IsReceived, ReceivedBy, ReceivedDate, CreatedDate,
                                    Name, SKU, Quantity, SentByName, TotalQuantity, IsApprove, Type, IsDecline , WHStock,IsLocation  from #RcvData
                                    {15}
                                    order by NeedAction, CreatedDate desc
                                    OFFSET {12} ROWS FETCH NEXT {13} ROWS ONLY 

                                    select COUNT(*)
									from #RcvData
                                    {15}

                                    select DISTINCT TechnicianId as Value, SentByName as Text from #RcvData
                                    order by SentByName asc
                                    
                                    DROP TABLE #TrfData
                                    DROP TABLE #RcvData
                                    ";
            //            string sqlQuery = @"
            //                                -- Transfer Inventory Tech

            //                                    select rec.Id, rec.TechnicianId, rec.EquipmentId, rec.IsReceived, rec.ReceivedDate, rec.ReceivedBy, eq.Name as Name, rec.Quantity, rec.IsApprove, 'Transfer' as [Type], rec.IsDecline,
            //                                    (select case when _sentby.IsLocation=1 then _sentby.UserName else _sentby.FirstName + ' ' + _sentby.LastName end from Employee _sentby where _sentby.UserId = rec.TechnicianId) as SentByName,
            //                                    (select case when _recby.IsLocation=1 then _recby.UserName else _recby.FirstName + ' ' + _recby.LastName end from Employee _recby where _recby.UserId = rec.ReceivedBy) as ReceivedByName,
            //                                        ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=eq.EquipmentId and Type='Add')
            //										- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=eq.EquipmentId and Type='Release')
            //										- isnull((select sum(Quantity) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = eq.EquipmentId and b.IsApprove = 0 and b.IsDecline = 0 ),0)) as WHStock,
            //                                  -- (SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Add' AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId) - (SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Release' AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId) AS TotalQuantity,
            //                                      ABS(((SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Add' 
            //  AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId) - (SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Release'
            //  AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId)
            //-isnull((select sum(Quantity) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = eq.EquipmentId and b.IsApprove = 0 and b.IsDecline = 0 ),0))
            // ) AS TotalQuantity,
            //                                   -- (select _sentby.UserId from Employee _sentby where _sentby.UserId = rec.TechnicianId) as SentById,
            //                                (select _sentby.IsLocation from Employee _sentby where _sentby.UserId = rec.TechnicianId) as IsLocation,

            //                                    rec.CreatedDate,
            //                                    eq.SKU
            //                                    INTO #TrfData
            //									from AssignedInventoryTechReceived rec
            //                                    left join Equipment eq on eq.EquipmentId = rec.EquipmentId
            //                                    where rec.TechnicianId='{7}'


            //                                    select Id, TechnicianId, EquipmentId, IsReceived, ReceivedBy, ReceivedDate, CreatedDate,
            //                                    Name, SKU, Quantity, ReceivedByName, TotalQuantity, IsApprove, Type, IsDecline, WHStock ,IsLocation from #TrfData
            //                                    {14}
            //                                    order by CreatedDate desc
            //                                    OFFSET {11} ROWS FETCH NEXT {13} ROWS ONLY

            //                                    select COUNT(*)
            //									from #TrfData
            //                                    {14}

            //                                    select DISTINCT ReceivedBy as Value, ReceivedByName as Text from #TrfData
            //                                    order by ReceivedByName  asc

            //                                -- Receive Inventory Tech

            //                                select distinct rec.Id, rec.TechnicianId, rec.EquipmentId, rec.IsReceived, rec.ReceivedDate, rec.ReceivedBy, _eq.Name as Name, rec.Quantity, rec.IsApprove, 'Approve' as [Type], rec.IsDecline,
            //                                    (select case when _sentby.IsLocation=1 then _sentby.UserName else _sentby.FirstName + ' ' + _sentby.LastName end from Employee _sentby where _sentby.UserId = rec.TechnicianId) as SentByName,                                    
            //                                    (select case when recby.IsLocation=1 then recby.UserName else recby.FirstName + ' ' + recby.LastName end from Employee recby where recby.UserId = rec.ReceivedBy) as ReceivedByName,
            //                                    ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eq.EquipmentId and Type='Add')
            //										- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=_eq.EquipmentId and Type='Release')
            //										- isnull((select sum(Quantity) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = _eq.EquipmentId and b.IsApprove = 0 and b.IsDecline = 0 ),0)) as WHStock,
            //                                 ---  (SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Add' AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId) - (SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Release' AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId) AS TotalQuantity,
            //ABS (((SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Add' 
            //  AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId) - (SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Release'
            //  AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId)
            //-isnull((select sum(Quantity) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = _eq.EquipmentId and b.IsApprove = 0 and b.IsDecline = 0 ),0))
            //)  AS TotalQuantity,
            //                                    --(select _sentby.UserId from Employee _sentby where _sentby.UserId = rec.TechnicianId) as SentById,
            //                             (select _sentby.IsLocation from Employee _sentby where _sentby.UserId = rec.TechnicianId) as IsLocation,

            //                                    rec.CreatedDate,
            //                                    _eq.SKU,
            //                                    (CASE WHEN IsApprove!=1 AND IsDecline!=1 THEN 0 ELSE 1 END) as NeedAction
            //                                    INTO #RcvData
            //									from AssignedInventoryTechReceived rec
            //                                    left join Equipment _eq on _eq.EquipmentId = rec.EquipmentId
            //                                    where rec.ReceivedBy='{7}'


            //                                    select Id, TechnicianId, EquipmentId, IsReceived, ReceivedBy, ReceivedDate, CreatedDate,
            //                                    Name, SKU, Quantity, SentByName, TotalQuantity, IsApprove, Type, IsDecline , WHStock,IsLocation  from #RcvData
            //                                    {15}
            //                                    order by NeedAction, CreatedDate desc
            //                                    OFFSET {12} ROWS FETCH NEXT {13} ROWS ONLY 

            //                                    select COUNT(*)
            //									from #RcvData
            //                                    {15}

            //                                    select DISTINCT TechnicianId as Value, SentByName as Text from #RcvData
            //                                    order by SentByName asc

            //                                    DROP TABLE #TrfData
            //                                    DROP TABLE #RcvData
            //                                    ";

            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string orderquery = "order by _cfd.Id desc";
            string filter_MultipleTechs = "WHERE (";
            string filter_MultipleRecvs = "WHERE (";

            if (filters.TTEmployeeIds != null)
            {
                foreach (var tech in filters.TTEmployeeIds)
                {
                    filter_MultipleTechs += string.Format("OR ReceivedBy='{0}' ", tech);
                }
                filter_MultipleTechs = filter_MultipleTechs.Replace("(OR", "( ");
                if (!filter_MultipleTechs.Contains("undefined"))
                {
                    filter_MultipleTechs += " )";
                }
                else
                {
                    filter_MultipleTechs = " ";
                }
            }
            else
            {
                filter_MultipleTechs = " ";
            }

            if (filters.RTEmployeeIds != null)
            {
                foreach (var tech in filters.RTEmployeeIds)
                {
                    filter_MultipleRecvs += string.Format("OR TechnicianId='{0}' ", tech);
                }
                filter_MultipleRecvs = filter_MultipleRecvs.Replace("(OR", "( ");
                if (!filter_MultipleRecvs.Contains("undefined"))
                {
                    filter_MultipleRecvs += " )";
                }
                else
                {
                    filter_MultipleRecvs = " ";
                }
            }
            else
            {
                filter_MultipleRecvs = " ";
            }

            filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");

            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filters.CompanyId,
                        filterColumntext,
                        filtertext,
                        filterByActiveStatus,
                        filterByEquipmentClass,
                        filterByEquipmentType,
                        filterByStockStatus,
                        filters.EmployeeId,
                        orderquery,
                        filters.Start,
                        filters.End,
                        (filters.PageNoTrf - 1) * filters.PageSize,
                        (filters.PageNoRcv - 1) * filters.PageSize,
                        filters.PageSize,
                        filter_MultipleTechs,
                        filter_MultipleRecvs);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                //LogError(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "InventoryDataAccess-GetTechTransferListByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                _ErrorLogDataAccess.Insert(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "DA|Inventory|GetTechTransferListByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                return null;
            }
        }
        
        public DataSet GetTechTransferLogListByFilters(TechTransferFilter filters)
        {
           
            string sqlQuery = @"
                                -- Transfer Inventory Tech

                                    select rec.Id, rec.TechnicianId, rec.EquipmentId, rec.IsReceived, rec.ReceivedDate, rec.ReceivedBy, eq.Name as Name, rec.Quantity, rec.IsApprove, 'Transfer' as [Type], rec.IsDecline, rec.ReqSrc,
                                    (select case when _sentby.IsLocation=1 then _sentby.UserName else _sentby.FirstName + ' ' + _sentby.LastName end from Employee _sentby where _sentby.UserId = rec.TechnicianId) as SentByName,
                                    (select case when _recby.IsLocation=1 then _recby.UserName else _recby.FirstName + ' ' + _recby.LastName end from Employee _recby where _recby.UserId = rec.ReceivedBy) as ReceivedByName,
                                    ISNULL((Select isnull(SUM(a.Quantity),0) from AssignedInventoryTechReceived a where a.EquipmentId = eq.EquipmentId 
									AND a.ReceivedBy = rec.ReceivedBy 
									and a.IsApprove = 1 
									) - (select isnull(SUM(a.Quantity),0) from AssignedInventoryTechReceived a 
									where  a.TechnicianId = rec.ReceivedBy 
									and a.EquipmentId = eq.EquipmentId 
									and a.IsApprove = 1
									)
									,0) AS WHStock,
                                     ABS(((SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Add' 
                                     AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId) - (SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Release'
                                     AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId)
                                   -isnull((select sum(Quantity) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = eq.EquipmentId and b.IsApprove = 0 and b.IsDecline = 0 ),0))
                                    ) AS TotalQuantity,
	                               --(select _sentby.UserId from Employee _sentby where _sentby.UserId = rec.TechnicianId) as SentById
                                    (select _sentby.IsLocation from Employee _sentby where _sentby.UserId = rec.TechnicianId) as IsLocation,
                                  --  (select  _sentby.FirstName + ' ' + _sentby.LastName from Employee _sentby where _sentby.UserId = rec.ClosedBy) as ClosedBy,
                                        (
                                            SELECT COALESCE(
                                                (SELECT _sentby.FirstName + ' ' + _sentby.LastName FROM Employee _sentby WHERE _sentby.UserId = rec.ClosedBy),
                                                CASE WHEN rec.ClosedBy IS NOT NULL THEN 'Live User' END
                                            )
                                        ) AS ClosedBy,
								   (select  _sentby.FirstName + ' ' + _sentby.LastName from Employee _sentby where _sentby.UserId = rec.CreatedBy) as CreatedBy,
                                   (CASE WHEN IsApprove!=1 AND IsDecline!=1 THEN 0 ELSE 1 END) as NeedAction,
                                    rec.CreatedDate,
                                    eq.SKU
                                    INTO #TrfData
									from AssignedInventoryTechReceived rec
                                    left join Equipment eq on eq.EquipmentId = rec.EquipmentId
                                    --where rec.TechnicianId='{7}'
                                    	
								    Delete from #TrfData where (SentByName is null OR SentByName = 'Purchase Order') AND ReceivedByName IN('Warehouse','Lost Bucket','X-Unused 02','X-Unused 01','X-Unused 03','RMA Bucket','Supplies Bucket','Correction Bucket') AND (TechnicianId = '00000000-0000-0000-0000-000000000000' Or TechnicianId = '22222222-2222-2222-2222-222222222221')
								   select distinct  Id, t.TechnicianId, t.EquipmentId, t.ReceivedBy, CreatedDate,
                                    Name, SKU, SentByName, Quantity, ReceivedByName, TotalQuantity, 
                                    CASE WHEN 
									t.ReceivedBy = '22222222-2222-2222-2222-222222222222'
									THEN  
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryWarehouse i WHERE i.EquipmentId = t.EquipmentId AND i.LocationId = '22222222-2222-2222-2222-222222222222' AND i.Type = 'Add') -
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryWarehouse i WHERE i.EquipmentId = t.EquipmentId AND i.LocationId = '22222222-2222-2222-2222-222222222222' AND i.Type = 'Release')
									
									WHEN 
									t.ReceivedBy
									IN( '22222222-2222-2222-2222-222222222223','22222222-2222-2222-2222-222222222224',
	                                    '22222222-2222-2222-2222-222222222225','22222222-2222-2222-2222-222222222226',
                                        '22222222-2222-2222-2222-222222222231','22222222-2222-2222-2222-222222222232',
	                                    '22222222-2222-2222-2222-222222222233')
									THEN
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryWarehouse i WHERE i.EquipmentId = t.EquipmentId AND i.LocationId = t.ReceivedBy AND i.Type = 'Add') -
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryWarehouse i WHERE i.EquipmentId = t.EquipmentId AND i.LocationId = t.ReceivedBy AND i.Type = 'Release')
									
									ELSE 
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryTech i WHERE i.EquipmentId = t.EquipmentId AND i.TechnicianId = t.ReceivedBy AND i.Type = 'Add') -
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryTech i WHERE i.EquipmentId = t.EquipmentId AND i.TechnicianId = t.ReceivedBy AND i.Type = 'Release')
									End As WHStock, 
                                    IsReceived, ReceivedDate, Type,  ReqSrc,IsLocation, CreatedBy,ClosedBy,t.IsApprove,  t.IsDecline,t.NeedAction from #TrfData t
                                    {14} {15}
                                    order by t.NeedAction, t.CreatedDate desc
                                    OFFSET {11} ROWS FETCH NEXT {13} ROWS ONLY

                                    select COUNT(*)
									from #TrfData t
                                    {14} {15}

                                    select DISTINCT TechnicianId as Value, SentByName as Text from #TrfData
                                    order by SentByName

                                    select DISTINCT ReceivedBy as Value, ReceivedByName as Text from #TrfData
                                    order by ReceivedByName asc
                                
                                -- Receive Inventory Tech

                                   select distinct rec.Id, rec.TechnicianId, rec.EquipmentId, rec.IsReceived, rec.ReceivedDate, rec.ReceivedBy, _eq.Name as Name, rec.Quantity, rec.IsApprove, 'Approve' as [Type], rec.IsDecline, rec.ReqSrc,
                                    (select case when _sentby.IsLocation=1 then _sentby.UserName else _sentby.FirstName + ' ' + _sentby.LastName end from Employee _sentby where _sentby.UserId = rec.TechnicianId) as SentByName,                                    
                                    (select case when recby.IsLocation=1 then recby.UserName else recby.FirstName + ' ' + recby.LastName end from Employee recby where recby.UserId = rec.ReceivedBy) as ReceivedByName,
                                   ISNULL((Select isnull(SUM(a.Quantity),0) from AssignedInventoryTechReceived a where a.EquipmentId = _eq.EquipmentId 
									AND a.ReceivedBy = rec.ReceivedBy 
									and a.IsApprove = 1 
									) - (select isnull(SUM(a.Quantity),0) from AssignedInventoryTechReceived a 
									where  a.TechnicianId = rec.ReceivedBy 
									and a.EquipmentId = _eq.EquipmentId 
									and a.IsApprove = 1
									)
									,0) AS WHStock,
                                   -- isnull((select SUM(_tech.Quantity) from InventoryWarehouse _tech where _tech.[Type] = 'Add' and _tech.EquipmentId = rec.EquipmentId ), 0) - isnull((select SUM(_tech.Quantity) from InventoryWarehouse _tech where _tech.[Type] = 'Release' and _tech.EquipmentId = rec.EquipmentId), 0) as WHStock
                                 ABS(((SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Add' 
                                 AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId) - (SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Release'
                                 AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId)
                                   -isnull((select sum(Quantity) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = _eq.EquipmentId and b.IsApprove = 0 and b.IsDecline = 0),0))
                                    )  AS TotalQuantity, 
	                              --(select _sentby.UserId from Employee _sentby where _sentby.UserId = rec.TechnicianId) as SentById
                                (select _sentby.IsLocation from Employee _sentby where _sentby.UserId = rec.TechnicianId) as IsLocation,
                                --(select  _sentby.FirstName + ' ' + _sentby.LastName from Employee _sentby where _sentby.UserId = rec.ClosedBy) as ClosedBy,
                                    (
                                        SELECT COALESCE(
                                            (SELECT _sentby.FirstName + ' ' + _sentby.LastName FROM Employee _sentby WHERE _sentby.UserId = rec.ClosedBy),
                                            CASE WHEN rec.ClosedBy IS NOT NULL THEN 'Live User' END
                                        )
                                    ) AS ClosedBy,
								   (select  _sentby.FirstName + ' ' + _sentby.LastName from Employee _sentby where _sentby.UserId = rec.CreatedBy) as CreatedBy,
                                    rec.CreatedDate,
                                    _eq.SKU,
									(CASE WHEN IsApprove!=1 AND IsDecline!=1 THEN 0 ELSE 1 END) as NeedAction
                                    INTO #RcvData
									from AssignedInventoryTechReceived rec
                                    left join Equipment _eq on _eq.EquipmentId = rec.EquipmentId
                                    --where rec.ReceivedBy='{7}'
                                    WHERE rec.ReceivedBy != '00000000-0000-0000-0000-000000000000' 
                                   GROUP BY rec.Id, rec.TechnicianId,
								   rec.EquipmentId,rec.IsReceived,rec.ReceivedDate,_eq.Name,rec.Quantity,rec.ReqSrc,rec.ClosedBy,
								   rec.CreatedBy,rec.CreatedDate,_eq.SKU,
								   rec.ReceivedBy, _eq.EquipmentId, rec.IsApprove, rec.IsDecline
                                    Delete from #RcvData where (SentByName is null OR SentByName = 'Purchase Order') AND ReceivedByName IN('Warehouse','Lost Bucket','X-Unused 02','X-Unused 01','X-Unused 03','RMA Bucket','Supplies Bucket','Correction Bucket') AND (TechnicianId = '00000000-0000-0000-0000-000000000000' Or TechnicianId = '22222222-2222-2222-2222-222222222221')
                                    select Id, TechnicianId, EquipmentId, ReceivedBy, ReceivedDate, CreatedDate,
                                    Name, SKU, SentByName, Quantity, ReceivedByName, TotalQuantity, 
                                    CASE WHEN 
									r.ReceivedBy = '22222222-2222-2222-2222-222222222222'
									THEN  
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryWarehouse i WHERE i.EquipmentId = r.EquipmentId AND i.LocationId = '22222222-2222-2222-2222-222222222222' AND i.Type = 'Add') -
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryWarehouse i WHERE i.EquipmentId = r.EquipmentId AND i.LocationId = '22222222-2222-2222-2222-222222222222' AND i.Type = 'Release')
									
									WHEN 
								    r.ReceivedBy
									IN( '22222222-2222-2222-2222-222222222223','22222222-2222-2222-2222-222222222224',
	                                    '22222222-2222-2222-2222-222222222225','22222222-2222-2222-2222-222222222226',
                                        '22222222-2222-2222-2222-222222222231','22222222-2222-2222-2222-222222222232',
	                                    '22222222-2222-2222-2222-222222222233')
									THEN
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryWarehouse i WHERE i.EquipmentId = r.EquipmentId AND i.LocationId = r.ReceivedBy AND i.Type = 'Add') -
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryWarehouse i WHERE i.EquipmentId = r.EquipmentId AND i.LocationId = r.ReceivedBy AND i.Type = 'Release')
									
									ELSE 
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryTech i WHERE i.EquipmentId = r.EquipmentId AND i.TechnicianId = r.ReceivedBy AND i.Type = 'Add') -
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryTech i WHERE i.EquipmentId = r.EquipmentId AND i.TechnicianId = r.ReceivedBy AND i.Type = 'Release')
									End As WHStock,
                                    IsReceived, IsApprove, Type, IsDecline,ReqSrc,IsLocation,ClosedBy,CreatedBy from #RcvData r
                                    {16} {17} {18}
                                    order by r.NeedAction, r.CreatedDate desc
                                    OFFSET {12} ROWS FETCH NEXT {13} ROWS ONLY 

                                    select COUNT(*)
									from #RcvData
                                    {16} {17} {18}

                                    select DISTINCT TechnicianId as Value, SentByName as Text from #RcvData
                                    order by SentByName

                                    select DISTINCT ReceivedBy as Value, ReceivedByName as Text from #RcvData
                                    order by ReceivedByName asc
                                    
                                    DROP TABLE #TrfData
                                    DROP TABLE #RcvData
                                    ";

            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string orderquery = "order by _cfd.Id desc";
            string filter_MultipleTechsFrom = "WHERE (";
            string filter_MultipleTechsTo = "AND (";
            string filter_MultipleRecvsFrom = "WHERE (";
            string filter_MultipleRecvsTo = "AND (";
            string DateQuery = "";
            if (filters.TFEmployeeIds != null)
            {
                foreach (var tech in filters.TFEmployeeIds)
                {
                    if (!string.IsNullOrEmpty(tech))
                    {
                        string[] techIds = tech.Split(',');

                        foreach (var techs in techIds)
                        {
                            if (techs.Contains("00000000-0000-0000-0000-000000000000"))
                            {
                                continue;
                            }
                            filter_MultipleTechsFrom += string.Format("OR t.TechnicianId='{0}' ", techs);
                        }
                    }
                    filter_MultipleTechsFrom = filter_MultipleTechsFrom.Replace("(OR", "( ");
                }
                if (!filter_MultipleTechsFrom.Contains("undefined"))
                {
                    filter_MultipleTechsFrom += " ) ";
                }
                else
                {
                    filter_MultipleTechsFrom = " ";
                }
              
            }
            else
            {
                filter_MultipleTechsFrom = " ";
                

            }


            if (filters.TTEmployeeIds != null)
            {
                foreach (var tech in filters.TTEmployeeIds)
                {

                    if (!string.IsNullOrEmpty(tech))
                    {
                        string[] techIds = tech.Split(',');

                        foreach (var techs in techIds)
                        {
                            if (techs.Contains("00000000-0000-0000-0000-000000000000"))
                            {
                                continue;
                            }
                            filter_MultipleTechsTo += string.Format("OR t.ReceivedBy='{0}' ", techs);
                        }
                    }
                    filter_MultipleTechsTo = filter_MultipleTechsTo.Replace("(OR", "( ");


                }
                if (!filter_MultipleTechsTo.Contains("undefined"))
                {
                    filter_MultipleTechsTo += " ) ";
                }
                else
                {
                    filter_MultipleTechsTo = " ";
                }
            }
            else
            {
                filter_MultipleTechsTo = " ";
            }

            if (filters.RFEmployeeIds != null)
            {
                foreach (var tech in filters.RFEmployeeIds)
                {
                    filter_MultipleRecvsFrom += string.Format("OR TechnicianId='{0}' ", tech);
                }
                filter_MultipleRecvsFrom = filter_MultipleRecvsFrom.Replace("(OR", "( ");
                if (!filter_MultipleRecvsFrom.Contains("undefined"))
                {
                    filter_MultipleRecvsFrom += " ) ";
                }
                else
                {
                    filter_MultipleRecvsFrom = " ";
                }
            }
            else
            {
                filter_MultipleRecvsFrom = " ";
            }

            if (filters.RTEmployeeIds != null)
            {
                foreach (var tech in filters.RTEmployeeIds)
                {
                    filter_MultipleRecvsTo += string.Format("OR ReceivedBy='{0}' ", tech);
                }
                filter_MultipleRecvsTo = filter_MultipleRecvsTo.Replace("(OR", "( ");
                if (!filter_MultipleRecvsTo.Contains("undefined"))
                {
                    filter_MultipleRecvsTo += " ) ";
                }
                else
                {
                    filter_MultipleRecvsTo = " ";
                }
            }
            else
            {
                filter_MultipleRecvsTo = " ";
            }
            
            filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");
            if(filters.GetReport)
            {
                if (filters.StartDate != new DateTime() && filters.EndDate != new DateTime())
                {
                    DateQuery = string.Format(" Where CreatedDate between '{0}' and '{1}'", filters.StartDate.ToString("yyyy-MM-dd hh:mm tt"), filters.EndDate.ToString("yyyy-MM-dd hh:mm tt"));
                }
            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filters.CompanyId,
                        filterColumntext,
                        filtertext,
                        filterByActiveStatus,
                        filterByEquipmentClass,
                        filterByEquipmentType,
                        filterByStockStatus,
                        filters.EmployeeId,
                        orderquery,
                        filters.Start,
                        filters.End,
                        (filters.PageNoTrf - 1) * filters.PageSize,
                        (filters.PageNoTrf - 1) * filters.PageSize,
                        //(filters.PageNoRcv - 1) * filters.PageSize,
                        filters.PageSize,
                        filter_MultipleTechsFrom,
                        filter_MultipleTechsTo,
                        filter_MultipleRecvsFrom,
                        filter_MultipleRecvsTo,
                        DateQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                //LogError(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "InventoryDataAccess-GetTechTransferListByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                _ErrorLogDataAccess.Insert(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "DA|Inventory|GetTechTransferListByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                return null;
            }
        }


        public DataTable GetTechTransferLogListByFiltersExport(TechTransferFilter filters)
        { 
            string sqlQuery = @"
                                select distinct rec.Id, rec.TechnicianId, rec.EquipmentId, rec.IsReceived, rec.ReceivedDate, rec.ReceivedBy, _eq.Name as Name, rec.Quantity, rec.IsApprove, 'Approve' as [Type], rec.IsDecline, rec.ReqSrc,
                                    (select case when _sentby.IsLocation=1 then _sentby.UserName else _sentby.FirstName + ' ' + _sentby.LastName end from Employee _sentby where _sentby.UserId = rec.TechnicianId) as SentByName,                                    
                                    (select case when recby.IsLocation=1 then recby.UserName else recby.FirstName + ' ' + recby.LastName end from Employee recby where recby.UserId = rec.ReceivedBy) as ReceivedByName,
                                   ISNULL((Select isnull(SUM(a.Quantity),0) from AssignedInventoryTechReceived a where a.EquipmentId = _eq.EquipmentId 
									AND a.ReceivedBy = rec.ReceivedBy 
									and a.IsApprove = 1 
									) - (select isnull(SUM(a.Quantity),0) from AssignedInventoryTechReceived a 
									where  a.TechnicianId = rec.ReceivedBy 
									and a.EquipmentId = _eq.EquipmentId 
									and a.IsApprove = 1
									)
									,0) AS WHStock,
                                   -- isnull((select SUM(_tech.Quantity) from InventoryWarehouse _tech where _tech.[Type] = 'Add' and _tech.EquipmentId = rec.EquipmentId ), 0) - isnull((select SUM(_tech.Quantity) from InventoryWarehouse _tech where _tech.[Type] = 'Release' and _tech.EquipmentId = rec.EquipmentId), 0) as WHStock
                                 ABS(((SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Add' 
                                 AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId) - (SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Release'
                                 AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId)
                                   -isnull((select sum(Quantity) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = _eq.EquipmentId and b.IsApprove = 0 and b.IsDecline = 0),0))
                                    )  AS TotalQuantity, 
	                              --(select _sentby.UserId from Employee _sentby where _sentby.UserId = rec.TechnicianId) as SentById
                                (select _sentby.IsLocation from Employee _sentby where _sentby.UserId = rec.TechnicianId) as IsLocation,
                                --(select  _sentby.FirstName + ' ' + _sentby.LastName from Employee _sentby where _sentby.UserId = rec.ClosedBy) as ClosedBy,
                                    (
                                        SELECT COALESCE(
                                            (SELECT _sentby.FirstName + ' ' + _sentby.LastName FROM Employee _sentby WHERE _sentby.UserId = rec.ClosedBy),
                                            CASE WHEN rec.ClosedBy IS NOT NULL THEN 'Live User' END
                                        )
                                    ) AS ClosedBy,
								   (select  _sentby.FirstName + ' ' + _sentby.LastName from Employee _sentby where _sentby.UserId = rec.CreatedBy) as CreatedBy,
                                    rec.CreatedDate,
                                    _eq.SKU,
									(CASE WHEN IsApprove!=1 AND IsDecline!=1 THEN 0 ELSE 1 END) as NeedAction
                                    INTO #RcvData
									from AssignedInventoryTechReceived rec
                                    left join Equipment _eq on _eq.EquipmentId = rec.EquipmentId
                                    --where rec.ReceivedBy='ac0ce890-bc5b-4c34-aab2-017af19bedf6'
                                    WHERE rec.ReceivedBy != '00000000-0000-0000-0000-000000000000' 
                                   GROUP BY rec.Id, rec.TechnicianId,
								   rec.EquipmentId,rec.IsReceived,rec.ReceivedDate,_eq.Name,rec.Quantity,rec.ReqSrc,rec.ClosedBy,
								   rec.CreatedBy,rec.CreatedDate,_eq.SKU,
								   rec.ReceivedBy, _eq.EquipmentId, rec.IsApprove, rec.IsDecline
                                    Delete from #RcvData where (SentByName is null OR SentByName = 'Purchase Order') AND ReceivedByName IN('Warehouse','Lost Bucket','X-Unused 02','X-Unused 01','X-Unused 03','RMA Bucket','Supplies Bucket','Correction Bucket') AND (TechnicianId = '00000000-0000-0000-0000-000000000000' Or TechnicianId = '22222222-2222-2222-2222-222222222221')
                                    select Id, TechnicianId, EquipmentId, ReceivedBy, ReceivedDate, CreatedDate,
                                    Name, SKU, SentByName, Quantity, ReceivedByName, TotalQuantity, 
                                    CASE WHEN 
									r.ReceivedBy = '22222222-2222-2222-2222-222222222222'
									THEN  
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryWarehouse i WHERE i.EquipmentId = r.EquipmentId AND i.LocationId = '22222222-2222-2222-2222-222222222222' AND i.Type = 'Add') -
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryWarehouse i WHERE i.EquipmentId = r.EquipmentId AND i.LocationId = '22222222-2222-2222-2222-222222222222' AND i.Type = 'Release')
									
									WHEN 
								    r.ReceivedBy
									IN( '22222222-2222-2222-2222-222222222223','22222222-2222-2222-2222-222222222224',
	                                    '22222222-2222-2222-2222-222222222225','22222222-2222-2222-2222-222222222226',
                                        '22222222-2222-2222-2222-222222222231','22222222-2222-2222-2222-222222222232',
	                                    '22222222-2222-2222-2222-222222222233')
									THEN
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryWarehouse i WHERE i.EquipmentId = r.EquipmentId AND i.LocationId = r.ReceivedBy AND i.Type = 'Add') -
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryWarehouse i WHERE i.EquipmentId = r.EquipmentId AND i.LocationId = r.ReceivedBy AND i.Type = 'Release')
									
									ELSE 
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryTech i WHERE i.EquipmentId = r.EquipmentId AND i.TechnicianId = r.ReceivedBy AND i.Type = 'Add') -
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryTech i WHERE i.EquipmentId = r.EquipmentId AND i.TechnicianId = r.ReceivedBy AND i.Type = 'Release')
									End As WHStock,
                                    IsReceived, IsApprove, Type, IsDecline,ReqSrc,IsLocation,ClosedBy,CreatedBy from #RcvData r
                                         {18} 
                                    order by r.NeedAction, r.CreatedDate desc
                                    OFFSET 0 ROWS FETCH NEXT 20000 ROWS ONLY 

                                    select COUNT(*)
									from #RcvData
                                         {18} 
										  
                                    DROP TABLE #RcvData
                                    ";

            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string orderquery = "order by _cfd.Id desc";
            string filter_MultipleTechsFrom = "WHERE (";
            string filter_MultipleTechsTo = "AND (";
            string filter_MultipleRecvsFrom = "WHERE (";
            string filter_MultipleRecvsTo = "AND (";
            string DateQuery = "";
            if (filters.TFEmployeeIds != null)
            {
                foreach (var tech in filters.TFEmployeeIds)
                {
                    if (!string.IsNullOrEmpty(tech))
                    {
                        string[] techIds = tech.Split(',');

                        foreach (var techs in techIds)
                        {
                            if (techs.Contains("00000000-0000-0000-0000-000000000000"))
                            {
                                continue;
                            }
                            filter_MultipleTechsFrom += string.Format("OR t.TechnicianId='{0}' ", techs);
                        }
                    }
                    filter_MultipleTechsFrom = filter_MultipleTechsFrom.Replace("(OR", "( ");
                }
                if (!filter_MultipleTechsFrom.Contains("undefined"))
                {
                    filter_MultipleTechsFrom += " ) ";
                }
                else
                {
                    filter_MultipleTechsFrom = " ";
                }

            }
            else
            {
                filter_MultipleTechsFrom = " ";


            }


            if (filters.TTEmployeeIds != null)
            {
                foreach (var tech in filters.TTEmployeeIds)
                {

                    if (!string.IsNullOrEmpty(tech))
                    {
                        string[] techIds = tech.Split(',');

                        foreach (var techs in techIds)
                        {
                            if (techs.Contains("00000000-0000-0000-0000-000000000000"))
                            {
                                continue;
                            }
                            filter_MultipleTechsTo += string.Format("OR t.ReceivedBy='{0}' ", techs);
                        }
                    }
                    filter_MultipleTechsTo = filter_MultipleTechsTo.Replace("(OR", "( ");


                }
                if (!filter_MultipleTechsTo.Contains("undefined"))
                {
                    filter_MultipleTechsTo += " ) ";
                }
                else
                {
                    filter_MultipleTechsTo = " ";
                }
            }
            else
            {
                filter_MultipleTechsTo = " ";
            }

            if (filters.RFEmployeeIds != null)
            {
                foreach (var tech in filters.RFEmployeeIds)
                {
                    filter_MultipleRecvsFrom += string.Format("OR TechnicianId='{0}' ", tech);
                }
                filter_MultipleRecvsFrom = filter_MultipleRecvsFrom.Replace("(OR", "( ");
                if (!filter_MultipleRecvsFrom.Contains("undefined"))
                {
                    filter_MultipleRecvsFrom += " ) ";
                }
                else
                {
                    filter_MultipleRecvsFrom = " ";
                }
            }
            else
            {
                filter_MultipleRecvsFrom = " ";
            }

            if (filters.RTEmployeeIds != null)
            {
                foreach (var tech in filters.RTEmployeeIds)
                {
                    filter_MultipleRecvsTo += string.Format("OR ReceivedBy='{0}' ", tech);
                }
                filter_MultipleRecvsTo = filter_MultipleRecvsTo.Replace("(OR", "( ");
                if (!filter_MultipleRecvsTo.Contains("undefined"))
                {
                    filter_MultipleRecvsTo += " ) ";
                }
                else
                {
                    filter_MultipleRecvsTo = " ";
                }
            }
            else
            {
                filter_MultipleRecvsTo = " ";
            }

            filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");
            if (filters.GetReport)
            {
                if (filters.StartDate != new DateTime() && filters.EndDate != new DateTime())
                {
                    DateQuery = string.Format(" Where CreatedDate between '{0}' and '{1}'", filters.StartDate.ToString("yyyy-MM-dd hh:mm tt"), filters.EndDate.ToString("yyyy-MM-dd hh:mm tt"));
                }
            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filters.CompanyId,
                        filterColumntext,
                        filtertext,
                        filterByActiveStatus,
                        filterByEquipmentClass,
                        filterByEquipmentType,
                        filterByStockStatus,
                        filters.EmployeeId,
                        orderquery,
                        filters.Start,
                        filters.End,
                        (filters.PageNoTrf - 1) * filters.PageSize,
                        (filters.PageNoTrf - 1) * filters.PageSize,
                        //(filters.PageNoRcv - 1) * filters.PageSize,
                        filters.PageSize,
                        filter_MultipleTechsFrom,
                        filter_MultipleTechsTo,
                        filter_MultipleRecvsFrom,
                        filter_MultipleRecvsTo,
                        DateQuery);
                        using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                        {
                            DataSet dsResult = GetDataSet(cmd);
                            return dsResult.Tables[0];
                        } 
            }
            catch (Exception ex)
            {
                //LogError(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "InventoryDataAccess-GetTechTransferListByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                _ErrorLogDataAccess.Insert(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "DA|Inventory|GetTechTransferListByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                return null;
            }
        }


        public DataSet GetTechTransferLogListByFiltersDate(TechTransferFilter filters, DateTime? start, DateTime? end)
        {
            string DateQuery = "";
            
            string sqlQuery = @"
                                -- Transfer Inventory Tech

                                    select rec.Id, rec.TechnicianId, rec.EquipmentId, rec.IsReceived, rec.ReceivedDate, rec.ReceivedBy, eq.Name as Name, rec.Quantity, rec.IsApprove, 'Transfer' as [Type], rec.IsDecline, rec.ReqSrc,
                                    (select case when _sentby.IsLocation=1 then _sentby.UserName else _sentby.FirstName + ' ' + _sentby.LastName end from Employee _sentby where _sentby.UserId = rec.TechnicianId) as SentByName,
                                    (select case when _recby.IsLocation=1 then _recby.UserName else _recby.FirstName + ' ' + _recby.LastName end from Employee _recby where _recby.UserId = rec.ReceivedBy) as ReceivedByName,
                                    ISNULL((Select isnull(SUM(a.Quantity),0) from AssignedInventoryTechReceived a where a.EquipmentId = eq.EquipmentId 
									AND a.ReceivedBy = rec.ReceivedBy 
									and a.IsApprove = 1 
									) - (select isnull(SUM(a.Quantity),0) from AssignedInventoryTechReceived a 
									where  a.TechnicianId = rec.ReceivedBy 
									and a.EquipmentId = eq.EquipmentId 
									and a.IsApprove = 1
									)
									,0) AS WHStock,
                                     ABS(((SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Add' 
                                     AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId) - (SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Release'
                                     AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId)
                                   -isnull((select sum(Quantity) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = eq.EquipmentId and b.IsApprove = 0 and b.IsDecline = 0 ),0))
                                    ) AS TotalQuantity,
	                               --(select _sentby.UserId from Employee _sentby where _sentby.UserId = rec.TechnicianId) as SentById
                                    (select _sentby.IsLocation from Employee _sentby where _sentby.UserId = rec.TechnicianId) as IsLocation,
                                  --  (select  _sentby.FirstName + ' ' + _sentby.LastName from Employee _sentby where _sentby.UserId = rec.ClosedBy) as ClosedBy,
                                        (
                                            SELECT COALESCE(
                                                (SELECT _sentby.FirstName + ' ' + _sentby.LastName FROM Employee _sentby WHERE _sentby.UserId = rec.ClosedBy),
                                                CASE WHEN rec.ClosedBy IS NOT NULL THEN 'Live User' END
                                            )
                                        ) AS ClosedBy,
								   (select  _sentby.FirstName + ' ' + _sentby.LastName from Employee _sentby where _sentby.UserId = rec.CreatedBy) as CreatedBy,
                                   (CASE WHEN IsApprove!=1 AND IsDecline!=1 THEN 0 ELSE 1 END) as NeedAction,
                                    rec.CreatedDate,
                                    eq.SKU
                                    INTO #TrfData
									from AssignedInventoryTechReceived rec
                                    left join Equipment eq on eq.EquipmentId = rec.EquipmentId
                                    --where rec.TechnicianId='{7}'
                                    	    Delete from #TrfData where (SentByName is null OR SentByName = 'Purchase Order') AND ReceivedByName IN('Warehouse','Lost Bucket','X-Unused 02','X-Unused 01','X-Unused 03','RMA Bucket','Supplies Bucket','Correction Bucket') AND (TechnicianId = '00000000-0000-0000-0000-000000000000' Or TechnicianId = '22222222-2222-2222-2222-222222222221')
								   select distinct  Id, t.TechnicianId, t.EquipmentId, t.ReceivedBy,
                                    Name, SKU, SentByName, Quantity, ReceivedByName, TotalQuantity, 
                                    CASE WHEN 
									t.ReceivedBy = '22222222-2222-2222-2222-222222222222'
									THEN  
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryWarehouse i WHERE i.EquipmentId = t.EquipmentId AND i.LocationId = '22222222-2222-2222-2222-222222222222' AND i.Type = 'Add') -
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryWarehouse i WHERE i.EquipmentId = t.EquipmentId AND i.LocationId = '22222222-2222-2222-2222-222222222222' AND i.Type = 'Release')
									
									WHEN 
									t.ReceivedBy
									IN( '22222222-2222-2222-2222-222222222223','22222222-2222-2222-2222-222222222224',
	                                    '22222222-2222-2222-2222-222222222225','22222222-2222-2222-2222-222222222226',
                                        '22222222-2222-2222-2222-222222222231','22222222-2222-2222-2222-222222222232',
	                                    '22222222-2222-2222-2222-222222222233')
									THEN
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryWarehouse i WHERE i.EquipmentId = t.EquipmentId AND i.LocationId = t.ReceivedBy AND i.Type = 'Add') -
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryWarehouse i WHERE i.EquipmentId = t.EquipmentId AND i.LocationId = t.ReceivedBy AND i.Type = 'Release')
									
									ELSE 
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryTech i WHERE i.EquipmentId = t.EquipmentId AND i.TechnicianId = t.ReceivedBy AND i.Type = 'Add') -
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryTech i WHERE i.EquipmentId = t.EquipmentId AND i.TechnicianId = t.ReceivedBy AND i.Type = 'Release')
									End As WHStock, 
                                    IsReceived,CreatedDate, ReceivedDate, Type,  ReqSrc,IsLocation, CreatedBy,ClosedBy,t.IsApprove,  t.IsDecline,t.NeedAction from #TrfData t 
                                    {14} {15}
                                    order by t.NeedAction, t.CreatedDate desc
                                    OFFSET {11} ROWS FETCH NEXT {13} ROWS ONLY

                                    select COUNT(*)
									from #TrfData t  
                                    {14} {15} 

                                    select DISTINCT TechnicianId as Value, SentByName as Text from #TrfData
                                    order by SentByName

                                    select DISTINCT ReceivedBy as Value, ReceivedByName as Text from #TrfData
                                    order by ReceivedByName asc
                                
                                -- Receive Inventory Tech

                                   select distinct rec.Id, rec.TechnicianId, rec.EquipmentId, rec.IsReceived, rec.ReceivedDate, rec.ReceivedBy, _eq.Name as Name, rec.Quantity, rec.IsApprove, 'Approve' as [Type], rec.IsDecline, rec.ReqSrc,
                                    (select case when _sentby.IsLocation=1 then _sentby.UserName else _sentby.FirstName + ' ' + _sentby.LastName end from Employee _sentby where _sentby.UserId = rec.TechnicianId) as SentByName,                                    
                                    (select case when recby.IsLocation=1 then recby.UserName else recby.FirstName + ' ' + recby.LastName end from Employee recby where recby.UserId = rec.ReceivedBy) as ReceivedByName,
                                   ISNULL((Select isnull(SUM(a.Quantity),0) from AssignedInventoryTechReceived a where a.EquipmentId = _eq.EquipmentId 
									AND a.ReceivedBy = rec.ReceivedBy 
									and a.IsApprove = 1 
									) - (select isnull(SUM(a.Quantity),0) from AssignedInventoryTechReceived a 
									where  a.TechnicianId = rec.ReceivedBy 
									and a.EquipmentId = _eq.EquipmentId 
									and a.IsApprove = 1
									)
									,0) AS WHStock,
                                   -- isnull((select SUM(_tech.Quantity) from InventoryWarehouse _tech where _tech.[Type] = 'Add' and _tech.EquipmentId = rec.EquipmentId ), 0) - isnull((select SUM(_tech.Quantity) from InventoryWarehouse _tech where _tech.[Type] = 'Release' and _tech.EquipmentId = rec.EquipmentId), 0) as WHStock
                                 ABS(((SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Add' 
                                 AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId) - (SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Release'
                                 AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId)
                                   -isnull((select sum(Quantity) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = _eq.EquipmentId and b.IsApprove = 0 and b.IsDecline = 0),0))
                                    )  AS TotalQuantity, 
	                              --(select _sentby.UserId from Employee _sentby where _sentby.UserId = rec.TechnicianId) as SentById
                                (select _sentby.IsLocation from Employee _sentby where _sentby.UserId = rec.TechnicianId) as IsLocation,
                                --(select  _sentby.FirstName + ' ' + _sentby.LastName from Employee _sentby where _sentby.UserId = rec.ClosedBy) as ClosedBy,
                                    (
                                        SELECT COALESCE(
                                            (SELECT _sentby.FirstName + ' ' + _sentby.LastName FROM Employee _sentby WHERE _sentby.UserId = rec.ClosedBy),
                                            CASE WHEN rec.ClosedBy IS NOT NULL THEN 'Live User' END
                                        )
                                    ) AS ClosedBy,
								   (select  _sentby.FirstName + ' ' + _sentby.LastName from Employee _sentby where _sentby.UserId = rec.CreatedBy) as CreatedBy,
                                    rec.CreatedDate,
                                    _eq.SKU,
									(CASE WHEN IsApprove!=1 AND IsDecline!=1 THEN 0 ELSE 1 END) as NeedAction
                                    INTO #RcvData
									from AssignedInventoryTechReceived rec
                                    left join Equipment _eq on _eq.EquipmentId = rec.EquipmentId
                                    --where rec.ReceivedBy='{7}'
                                    WHERE rec.ReceivedBy != '00000000-0000-0000-0000-000000000000' 
                                   GROUP BY rec.Id, rec.TechnicianId,
								   rec.EquipmentId,rec.IsReceived,rec.ReceivedDate,_eq.Name,rec.Quantity,rec.ReqSrc,rec.ClosedBy,
								   rec.CreatedBy,rec.CreatedDate,_eq.SKU,
								   rec.ReceivedBy, _eq.EquipmentId, rec.IsApprove, rec.IsDecline
                                    Delete from #RcvData where (SentByName is null OR SentByName = 'Purchase Order') AND ReceivedByName IN('Warehouse','Lost Bucket','X-Unused 02','X-Unused 01','X-Unused 03','RMA Bucket','Supplies Bucket','Correction Bucket') AND (TechnicianId = '00000000-0000-0000-0000-000000000000' Or TechnicianId = '22222222-2222-2222-2222-222222222221')
                                    select Id, TechnicianId, EquipmentId, ReceivedBy, ReceivedDate, CreatedDate,
                                    Name, SKU, SentByName, Quantity, ReceivedByName, TotalQuantity, 
                                    CASE WHEN 
									r.ReceivedBy = '22222222-2222-2222-2222-222222222222'
									THEN  
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryWarehouse i WHERE i.EquipmentId = r.EquipmentId AND i.LocationId = '22222222-2222-2222-2222-222222222222' AND i.Type = 'Add') -
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryWarehouse i WHERE i.EquipmentId = r.EquipmentId AND i.LocationId = '22222222-2222-2222-2222-222222222222' AND i.Type = 'Release')
									
									WHEN 
								    r.ReceivedBy
									IN( '22222222-2222-2222-2222-222222222223','22222222-2222-2222-2222-222222222224',
	                                    '22222222-2222-2222-2222-222222222225','22222222-2222-2222-2222-222222222226',
                                        '22222222-2222-2222-2222-222222222231','22222222-2222-2222-2222-222222222232',
	                                    '22222222-2222-2222-2222-222222222233')
									THEN
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryWarehouse i WHERE i.EquipmentId = r.EquipmentId AND i.LocationId = r.ReceivedBy AND i.Type = 'Add') -
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryWarehouse i WHERE i.EquipmentId = r.EquipmentId AND i.LocationId = r.ReceivedBy AND i.Type = 'Release')
									
									ELSE 
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryTech i WHERE i.EquipmentId = r.EquipmentId AND i.TechnicianId = r.ReceivedBy AND i.Type = 'Add') -
									(SELECT ISNULL(SUM(i.Quantity),0) from InventoryTech i WHERE i.EquipmentId = r.EquipmentId AND i.TechnicianId = r.ReceivedBy AND i.Type = 'Release')
									End As WHStock,
                                    IsReceived, IsApprove, Type, IsDecline,ReqSrc,IsLocation,ClosedBy,CreatedBy from #RcvData r 
                                    {16} {17} {18}
                                    order by r.NeedAction, r.CreatedDate desc
                                    OFFSET {12} ROWS FETCH NEXT {13} ROWS ONLY 

                                    select COUNT(*)
									from #RcvData  
                                    {16} {17}{18}

                                    select DISTINCT TechnicianId as Value, SentByName as Text from #RcvData
                                    order by SentByName

                                    select DISTINCT ReceivedBy as Value, ReceivedByName as Text from #RcvData
                                    order by ReceivedByName asc
                                    
                                    DROP TABLE #TrfData
                                    DROP TABLE #RcvData
                                    ";

            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string orderquery = "order by _cfd.Id desc";
            string filter_MultipleTechsFrom = "WHERE (";
            string filter_MultipleTechsTo = "AND (";
            string filter_MultipleRecvsFrom = "WHERE (";
            string filter_MultipleRecvsTo = "AND (";

            if (filters.TFEmployeeIds != null)
            {
                foreach (var tech in filters.TFEmployeeIds)
                {
                    if (!string.IsNullOrEmpty(tech))
                    {
                        string[] techIds = tech.Split(',');

                        foreach (var techs in techIds)
                        {
                            if (techs.Contains("00000000-0000-0000-0000-000000000000"))
                            {
                                continue;
                            }
                            filter_MultipleTechsFrom += string.Format("OR t.TechnicianId='{0}' ", techs);
                        }
                    }
                    filter_MultipleTechsFrom = filter_MultipleTechsFrom.Replace("(OR", "( ");
                    
                }
                if (!filter_MultipleTechsFrom.Contains("undefined"))
                {
                    filter_MultipleTechsFrom += " ) ";
                    if (start.HasValue && end.HasValue)
                    {
                        filter_MultipleTechsFrom += $" and CreatedDate between '{start.Value.ToString("yyyy-MM-dd hh:mm tt")}' and '{end.Value.ToString("yyyy-MM-dd hh:mm tt")}'";
                    }
                    
                }
                else
                {
                    filter_MultipleTechsFrom = " ";
                }
                if (start.HasValue && end.HasValue)
                {

                    DateQuery = string.Format("Where CreatedDate between '{0}' and '{1}'", start.Value.ToString("yyyy-MM-dd hh:mm tt"), end.Value.ToString("yyyy-MM-dd hh:mm tt"));

                }
            }
            else
            {
                filter_MultipleTechsFrom = " ";
                if (start.HasValue && end.HasValue)
                {
                    if (filters.PageNoTrf >= 1 &&
      !string.IsNullOrWhiteSpace(filters.RFEmployeeIds?.ToString()) &&
      !string.IsNullOrWhiteSpace(filters.RTEmployeeIds?.ToString()))
                    {
                        DateQuery = string.Format("and CreatedDate between '{0}' and '{1}'", start.Value.ToString("yyyy-MM-dd hh:mm tt"), end.Value.ToString("yyyy-MM-dd hh:mm tt"));

                    }
                    else
                    {
                        DateQuery = string.Format("Where CreatedDate between '{0}' and '{1}'", start.Value.ToString("yyyy-MM-dd hh:mm tt"), end.Value.ToString("yyyy-MM-dd hh:mm tt"));

                    }


                }
                }

            if (filters.TTEmployeeIds != null)
            {
                foreach (var tech in filters.TTEmployeeIds)
                {

                    if (!string.IsNullOrEmpty(tech))
                    {
                        string[] techIds = tech.Split(',');

                        foreach (var techs in techIds)
                        {
                            if (techs.Contains("00000000-0000-0000-0000-000000000000"))
                            {
                                continue;
                            }
                            filter_MultipleTechsTo += string.Format("OR t.ReceivedBy='{0}' ", techs);
                        }
                    }
                    filter_MultipleTechsTo = filter_MultipleTechsTo.Replace("(OR", "( ");
                    


                }
                if (!filter_MultipleTechsTo.Contains("undefined"))
                {
                    filter_MultipleTechsTo += " ) ";
                    if (start.HasValue && end.HasValue)
                    {
                        filter_MultipleTechsTo += $" and CreatedDate between '{start.Value.ToString("yyyy-MM-dd hh:mm tt")}' and '{end.Value.ToString("yyyy-MM-dd hh:mm tt")}'";
                    }

                    
                }
                else
                {
                    filter_MultipleTechsTo = " ";
                }
            }
            else
            {
                filter_MultipleTechsTo = " ";
            }

            if (filters.RFEmployeeIds != null)
            {
                foreach (var tech in filters.RFEmployeeIds)
                {
                    filter_MultipleRecvsFrom += string.Format("OR TechnicianId='{0}' ", tech);
                }
                filter_MultipleRecvsFrom = filter_MultipleRecvsFrom.Replace("(OR", "( ");
                

                if (!filter_MultipleRecvsFrom.Contains("undefined"))
                {
                    filter_MultipleRecvsFrom += " ) ";
                    if (start.HasValue && end.HasValue)
                    {
                        filter_MultipleRecvsFrom += $" and CreatedDate between '{start.Value.ToString("yyyy-MM-dd hh:mm tt")}' and '{end.Value.ToString("yyyy-MM-dd hh:mm tt")}'";
                    }

                }
                else
                {
                    filter_MultipleRecvsFrom = " ";
                }
            }
            else
            {
                filter_MultipleRecvsFrom = " ";
            }

            if (filters.RTEmployeeIds != null)
            {
                foreach (var tech in filters.RTEmployeeIds)
                {
                    filter_MultipleRecvsTo += string.Format("OR ReceivedBy='{0}' ", tech);
                }
                filter_MultipleRecvsTo = filter_MultipleRecvsTo.Replace("(OR", "( ");
                if (!filter_MultipleRecvsTo.Contains("undefined"))
                {
                    filter_MultipleRecvsTo += " ) ";

                }
                else
                {
                    filter_MultipleRecvsTo = " ";
                }
            }
            else
            {
                filter_MultipleRecvsTo = " ";
            }

            filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");

            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filters.CompanyId,
                        filterColumntext,
                        filtertext,
                        filterByActiveStatus,
                        filterByEquipmentClass,
                        filterByEquipmentType,
                        filterByStockStatus,
                        filters.EmployeeId,
                        orderquery,
                        filters.Start,
                        filters.End,
                        (filters.PageNoTrf - 1) * filters.PageSize,
                        (filters.PageNoTrf - 1) * filters.PageSize,
                        //(filters.PageNoRcv - 1) * filters.PageSize,
                        filters.PageSize,
                        filter_MultipleTechsFrom,
                        filter_MultipleTechsTo,
                        filter_MultipleRecvsFrom,
                        filter_MultipleRecvsTo,
                        DateQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                //LogError(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "InventoryDataAccess-GetTechTransferListByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                _ErrorLogDataAccess.Insert(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "DA|Inventory|GetTechTransferListByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                return null;
            }
        }

        public DataSet GetTechTransferLogListByFilters_Backup(TechTransferFilter filters)
        {
            string sqlQuery = @"

                                -- Transfer Inventory Tech

                                    select rec.Id, rec.TechnicianId, rec.EquipmentId, rec.IsReceived, rec.ReceivedDate, rec.ReceivedBy, eq.Name as Name, rec.Quantity, rec.IsApprove, 'Transfer' as [Type], rec.IsDecline, rec.ReqSrc,
                                    (select case when _sentby.IsLocation=1 then _sentby.UserName else _sentby.FirstName + ' ' + _sentby.LastName end from Employee _sentby where _sentby.UserId = rec.TechnicianId) as SentByName,
                                    (select case when _recby.IsLocation=1 then _recby.UserName else _recby.FirstName + ' ' + _recby.LastName end from Employee _recby where _recby.UserId = rec.ReceivedBy) as ReceivedByName,
                                    --CASE WHEN rec.TechnicianId='22222222-2222-2222-2222-222222222221' THEN 0 ELSE 
                                        --((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=eq.EquipmentId and Type='Add' and LocationId=rec.TechnicianId)
										-- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=eq.EquipmentId and Type='Release' and LocationId=rec.TechnicianId)
										-- isnull((select sum(Quantity) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = eq.EquipmentId and b.IsApprove = 0 and b.IsDecline = 0 ),0)) END as WHStock,
                                   -- isnull((select SUM(_tech.Quantity) from InventoryWarehouse _tech where _tech.[Type] = 'Add' and _tech.EquipmentId = rec.EquipmentId), 0) - isnull((select SUM(_tech.Quantity) from InventoryWarehouse _tech where _tech.[Type] = 'Release' and _tech.EquipmentId = rec.EquipmentId), 0) as WHStock,
ABS(((SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Add' 
  AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId) - (SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Release'
  AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId)
-isnull((select sum(Quantity) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = eq.EquipmentId and b.IsApprove = 0 and b.IsDecline = 0 ),0))
  ) AS TotalQuantity,
	                               --(select _sentby.UserId from Employee _sentby where _sentby.UserId = rec.TechnicianId) as SentById
                                    (select _sentby.IsLocation from Employee _sentby where _sentby.UserId = rec.TechnicianId) as IsLocation,
                                  --  (select  _sentby.FirstName + ' ' + _sentby.LastName from Employee _sentby where _sentby.UserId = rec.ClosedBy) as ClosedBy,
                                        (
                                            SELECT COALESCE(
                                                (SELECT _sentby.FirstName + ' ' + _sentby.LastName FROM Employee _sentby WHERE _sentby.UserId = rec.ClosedBy),
                                                CASE WHEN rec.ClosedBy IS NOT NULL THEN 'Live User' END
                                            )
                                        ) AS ClosedBy,
								   (select  _sentby.FirstName + ' ' + _sentby.LastName from Employee _sentby where _sentby.UserId = rec.CreatedBy) as CreatedBy,
                                   (CASE WHEN IsApprove!=1 AND IsDecline!=1 THEN 0 ELSE 1 END) as NeedAction,
                                    rec.CreatedDate,
                                    eq.SKU
                                    INTO #TrfData
									from AssignedInventoryTechReceived rec
                                    left join Equipment eq on eq.EquipmentId = rec.EquipmentId
                                    --where rec.TechnicianId='{7}'
                                    	select r.EquipmentId, ReceivedBy,r.IsApprove,r.IsDecline, SUM(Quantity) AS ReceivedQty INTO #rTbl from AssignedInventoryTechReceived r
                                    inner join
                                    Equipment eq on eq.EquipmentId = r.EquipmentId
                                   --Where r.IsApprove = 1
                                    Group by ReceivedBy,r.EquipmentId ,r.IsApprove,r.IsDecline 

                                   select s.EquipmentId, TechnicianId, SUM(Quantity) AS SentQty  INTO #sTbl from AssignedInventoryTechReceived s
                                   inner join
                                   Equipment eq on eq.EquipmentId = s.EquipmentId
                                   --Where s.IsDecline = 0
                                   Group by TechnicianId, s.EquipmentId
                                   
								   Select r.ReceivedBy,isnull(r.ReceivedQty,0) ReceivedQty, isnull(s.SentQty,0) SentQty,s.TechnicianId,r.EquipmentId, (isnull((r.ReceivedQty)-(s.SentQty),r.ReceivedQty)) AS OnHandQty 
								   , r.IsApprove,r.IsDecline
                                   into #WHStockTbl from #rTbl r  left join 
                                   #sTbl s on s.EquipmentId = r.EquipmentId AND r.ReceivedBy = s.TechnicianId
								   
								   select distinct  Id, t.TechnicianId, t.EquipmentId, t.ReceivedBy, CreatedDate,
                                    Name, SKU, SentByName, Quantity, ReceivedByName, TotalQuantity, CASE 
        WHEN (
            ISNULL(
                (
                    SELECT SUM(ReceivedQty) 
                    FROM #WHStockTbl t2 
                    WHERE t2.ReceivedBy = w.ReceivedBy 
                          AND t2.EquipmentId = w.EquipmentId 
                          AND t2.IsApprove = 1
                ) - (
                    SELECT SUM(SentQty) 
                    FROM #WHStockTbl t2 
                    WHERE t2.ReceivedBy = w.ReceivedBy 
                          AND t2.EquipmentId = w.EquipmentId 
                          AND t2.IsApprove = 1
                ) + (
                    SELECT ISNULL(SUM(Quantity),0) 
                    FROM #TrfData t2 
                    WHERE t2.TechnicianId = w.ReceivedBy 
                          AND t2.EquipmentId = w.EquipmentId 
                          AND t2.IsDecline = 1
                ),
                0
            ) < 0
        ) THEN 0
        ELSE ISNULL(
                (
                    SELECT SUM(ReceivedQty) 
                    FROM #WHStockTbl t2 
                    WHERE t2.ReceivedBy = w.ReceivedBy 
                          AND t2.EquipmentId = w.EquipmentId 
                          AND t2.IsApprove = 1
                ) - (
                    SELECT SUM(SentQty) 
                    FROM #WHStockTbl t2 
                    WHERE t2.ReceivedBy = w.ReceivedBy 
                          AND t2.EquipmentId = w.EquipmentId 
                          AND t2.IsApprove = 1
                ) + (
                    SELECT ISNULL(SUM(Quantity),0) 
                    FROM #TrfData t2 
                    WHERE t2.TechnicianId = w.ReceivedBy 
                          AND t2.EquipmentId = w.EquipmentId 
                          AND t2.IsDecline = 1
                ),
                0
            )
    END AS WHStock, IsReceived, ReceivedDate, Type,  ReqSrc,IsLocation, CreatedBy,ClosedBy,t.IsApprove,  t.IsDecline,t.NeedAction from #TrfData t
								    join #WHStockTbl w on w.EquipmentId = t.EquipmentId AND w.ReceivedBy = t.ReceivedBy
                                    {14} {15}
                                    order by t.NeedAction, t.CreatedDate desc
                                    OFFSET {12} ROWS FETCH NEXT {13} ROWS ONLY

                                    select COUNT(*)
									from #TrfData t
                                    {14} {15}

                                    select DISTINCT TechnicianId as Value, SentByName as Text from #TrfData
                                    order by SentByName

                                    select DISTINCT ReceivedBy as Value, ReceivedByName as Text from #TrfData
                                    order by ReceivedByName asc
                                
                                -- Receive Inventory Tech

                                select distinct rec.Id, rec.TechnicianId, rec.EquipmentId, rec.IsReceived, rec.ReceivedDate, rec.ReceivedBy, _eq.Name as Name, rec.Quantity, rec.IsApprove, 'Approve' as [Type], rec.IsDecline, rec.ReqSrc,
                                    (select case when _sentby.IsLocation=1 then _sentby.UserName else _sentby.FirstName + ' ' + _sentby.LastName end from Employee _sentby where _sentby.UserId = rec.TechnicianId) as SentByName,                                    
                                    (select case when recby.IsLocation=1 then recby.UserName else recby.FirstName + ' ' + recby.LastName end from Employee recby where recby.UserId = rec.ReceivedBy) as ReceivedByName,
                                    ISNULL(
        (SELECT SUM(Quantity) 
         FROM AssignedInventoryTechReceived 
         WHERE EquipmentId = _eq.EquipmentId 
           AND TechnicianId != '00000000-0000-0000-0000-000000000000' 
           AND ReceivedBy = rec.ReceivedBy), 0
    ) AS WHStock,

                                   -- isnull((select SUM(_tech.Quantity) from InventoryWarehouse _tech where _tech.[Type] = 'Add' and _tech.EquipmentId = rec.EquipmentId ), 0) - isnull((select SUM(_tech.Quantity) from InventoryWarehouse _tech where _tech.[Type] = 'Release' and _tech.EquipmentId = rec.EquipmentId), 0) as WHStock
ABS(((SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Add' 
  AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId) - (SELECT ISNULL(SUM(_tech.Quantity), 0) FROM inventorytech _tech WHERE _tech.[Type] = 'Release'
  AND _tech.EquipmentId = rec.EquipmentId AND _tech.TechnicianId = rec.TechnicianId)
-isnull((select sum(Quantity) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = _eq.EquipmentId and b.IsApprove = 0 and b.IsDecline = 0),0))
)  AS TotalQuantity, 
	                              --(select _sentby.UserId from Employee _sentby where _sentby.UserId = rec.TechnicianId) as SentById
                                (select _sentby.IsLocation from Employee _sentby where _sentby.UserId = rec.TechnicianId) as IsLocation,
                                --(select  _sentby.FirstName + ' ' + _sentby.LastName from Employee _sentby where _sentby.UserId = rec.ClosedBy) as ClosedBy,
                                    (
                                        SELECT COALESCE(
                                            (SELECT _sentby.FirstName + ' ' + _sentby.LastName FROM Employee _sentby WHERE _sentby.UserId = rec.ClosedBy),
                                            CASE WHEN rec.ClosedBy IS NOT NULL THEN 'Live User' END
                                        )
                                    ) AS ClosedBy,
								   (select  _sentby.FirstName + ' ' + _sentby.LastName from Employee _sentby where _sentby.UserId = rec.CreatedBy) as CreatedBy,
                                    rec.CreatedDate,
                                    _eq.SKU,
									(CASE WHEN IsApprove!=1 AND IsDecline!=1 THEN 0 ELSE 1 END) as NeedAction
                                    INTO #RcvData
									from AssignedInventoryTechReceived rec
                                    left join Equipment _eq on _eq.EquipmentId = rec.EquipmentId
                                    --where rec.ReceivedBy='{7}'
                                    WHERE rec.ReceivedBy != '00000000-0000-0000-0000-000000000000' 
                                   GROUP BY rec.Id, rec.TechnicianId,
								   rec.EquipmentId,rec.IsReceived,rec.ReceivedDate,_eq.Name,rec.Quantity,rec.ReqSrc,rec.ClosedBy,
								   rec.CreatedBy,rec.CreatedDate,_eq.SKU,
								   rec.ReceivedBy, _eq.EquipmentId, rec.IsApprove, rec.IsDecline
                                    

                                    select Id, TechnicianId, EquipmentId, ReceivedBy, ReceivedDate, CreatedDate,
                                    Name, SKU, SentByName, Quantity, ReceivedByName, TotalQuantity, WHStock, IsReceived, IsApprove, Type, IsDecline,ReqSrc,IsLocation,ClosedBy,CreatedBy from #RcvData
                                    {16} {17}
                                    order by NeedAction, CreatedDate desc
                                    OFFSET {12} ROWS FETCH NEXT {13} ROWS ONLY 

                                    select COUNT(*)
									from #RcvData
                                    {16} {17}

                                    select DISTINCT TechnicianId as Value, SentByName as Text from #RcvData
                                    order by SentByName

                                    select DISTINCT ReceivedBy as Value, ReceivedByName as Text from #RcvData
                                    order by ReceivedByName asc
                                    
                                    DROP TABLE #TrfData
                                    DROP TABLE #RcvData
	                                drop table #rTbl
                                    drop table #sTbl
									drop table #WHStockTbl
                                    ";

            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string orderquery = "order by _cfd.Id desc";
            string filter_MultipleTechsFrom = "WHERE (";
            string filter_MultipleTechsTo = "AND (";
            string filter_MultipleRecvsFrom = "WHERE (";
            string filter_MultipleRecvsTo = "AND (";

            if (filters.TFEmployeeIds != null)
            {
                foreach (var tech in filters.TFEmployeeIds)
                {
                    if (!string.IsNullOrEmpty(tech))
                    {
                        string[] techIds = tech.Split(',');

                        foreach (var techs in techIds)
                        {
                            if (techs.Contains("00000000-0000-0000-0000-000000000000"))
                            {
                                continue;
                            }
                            filter_MultipleTechsFrom += string.Format("OR t.TechnicianId='{0}' ", techs);
                        }
                    }
                    filter_MultipleTechsFrom = filter_MultipleTechsFrom.Replace("(OR", "( ");
                }
                if (!filter_MultipleTechsFrom.Contains("undefined"))
                {
                    filter_MultipleTechsFrom += " ) ";
                }
                else
                {
                    filter_MultipleTechsFrom = " ";
                }
            }
            else
            {
                filter_MultipleTechsFrom = " ";
            }

            if (filters.TTEmployeeIds != null)
            {
                foreach (var tech in filters.TTEmployeeIds)
                {

                    if (!string.IsNullOrEmpty(tech))
                    {
                        string[] techIds = tech.Split(',');

                        foreach (var techs in techIds)
                        {
                            if (techs.Contains("00000000-0000-0000-0000-000000000000"))
                            {
                                continue;
                            }
                            filter_MultipleTechsTo += string.Format("OR t.ReceivedBy='{0}' ", techs);
                        }
                    }
                    filter_MultipleTechsTo = filter_MultipleTechsTo.Replace("(OR", "( ");


                }
                if (!filter_MultipleTechsTo.Contains("undefined"))
                {
                    filter_MultipleTechsTo += " ) ";
                }
                else
                {
                    filter_MultipleTechsTo = " ";
                }
            }
            else
            {
                filter_MultipleTechsTo = " ";
            }

            if (filters.RFEmployeeIds != null)
            {
                foreach (var tech in filters.RFEmployeeIds)
                {
                    filter_MultipleRecvsFrom += string.Format("OR TechnicianId='{0}' ", tech);
                }
                filter_MultipleRecvsFrom = filter_MultipleRecvsFrom.Replace("(OR", "( ");
                if (!filter_MultipleRecvsFrom.Contains("undefined"))
                {
                    filter_MultipleRecvsFrom += " ) ";
                }
                else
                {
                    filter_MultipleRecvsFrom = " ";
                }
            }
            else
            {
                filter_MultipleRecvsFrom = " ";
            }

            if (filters.RTEmployeeIds != null)
            {
                foreach (var tech in filters.RTEmployeeIds)
                {
                    filter_MultipleRecvsTo += string.Format("OR ReceivedBy='{0}' ", tech);
                }
                filter_MultipleRecvsTo = filter_MultipleRecvsTo.Replace("(OR", "( ");
                if (!filter_MultipleRecvsTo.Contains("undefined"))
                {
                    filter_MultipleRecvsTo += " ) ";
                }
                else
                {
                    filter_MultipleRecvsTo = " ";
                }
            }
            else
            {
                filter_MultipleRecvsTo = " ";
            }

            filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");

            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filters.CompanyId,
                        filterColumntext,
                        filtertext,
                        filterByActiveStatus,
                        filterByEquipmentClass,
                        filterByEquipmentType,
                        filterByStockStatus,
                        filters.EmployeeId,
                        orderquery,
                        filters.Start,
                        filters.End,
                        (filters.PageNoTrf - 1) * filters.PageSize,
                        (filters.PageNoRcv - 1) * filters.PageSize,
                        filters.PageSize,
                        filter_MultipleTechsFrom,
                        filter_MultipleTechsTo,
                        filter_MultipleRecvsFrom,
                        filter_MultipleRecvsTo);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                //LogError(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "InventoryDataAccess-GetTechTransferListByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                _ErrorLogDataAccess.Insert(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "DA|Inventory|GetTechTransferListByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                return null;
            }
        }

        public DataSet GetAllAssignedInventory(AssignedInventoryTechReceived objassigntech)
        {
            string sqlQuery = @"

                                select * from AssignedInventoryTechReceived invtech
                                    where TechnicianId = '{0}' 
                                    and EquipmentId = '{1}' 
                                    AND Id={2}
                                    ";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                        objassigntech.TechnicianId,
                        objassigntech.EquipmentId,
                        objassigntech.Id);
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

        //TODO: discard this it working through the Base class
        //public long LogError(ErrorLog err)
        //{
        //    long Id = 0;
        //    string sqlQuery = @"

        //                        INSERT INTO [dbo].[ErrorLog]   
        //                        ( [ErrorId], [ErrorFor], [Message], [TimeUtc] )   
        //                        VALUES   
        //                        (  
        //                        '{0}',  
        //                        '{1}',  
        //                        '{2}',  
        //                        '{3}'
        //                        )  
        //                            ";
        //    try
        //    {
        //        err.Message = err.Message.Replace("'", "`");
        //        err.Message = Regex.Replace(err.Message, @"\s+", " ");
        //        sqlQuery = string.Format(sqlQuery,
        //                err.ErrorId, err.ErrorFor, err.Message, err.TimeUtc);
        //        using (SqlCommand cmd = GetSQLCommand(sqlQuery))
        //        {
        //            Id = InsertRecord(cmd);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return Id;
        //}

        public DataTable GetEqupmentListBySearchKeyAndCompanyIdTechnicianId(string key, Guid companyId, int MaxLoad, Guid technicianId, string ExistEquipment)
        {
            string sqlQuery = @"
                                Declare @SerchText nvarchar(100)
                                set @SerchText = '%{0}%'
                                select
                                DISTINCT
                                Top {2}
                                eq.EquipmentId
                                ,eq.SKU
                                ,eq.Barcode
		                        ,eq.Name as EquipmentName
		                        ,eq.Retail as RetailPrice
		                        ,eq.Reorderpoint
		                        ,(select sum(Quantity) from Inventory where EquipmentId = eq.EquipmentId)  
                                    as QuantityAvailable
		                        ,et.Name as EquipmentType
                                ,eq.Comments as EquipmentDescription
                                ,eq.Id
                                ,Round(ISNULL(ISNULL(ISNULL((select top(1) Cost from EquipmentVendor where IsPrimary =1 and equipmentid = eq.equipmentid) ,eq.SupplierCost),eq.Retail),0),2) as SupplierCost,
                                ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=eq.EquipmentId and Type='Add'  And invinner.TechnicianId='{3}')
                                -(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=eq.EquipmentId and Type='Release'  And invinner.TechnicianId='{3}')) QuantityOnHand

                                ,((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=eq.EquipmentId and Type='Add' And invinner.LocationId='{3}')
                                 - (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=eq.EquipmentId and Type='Release' And invinner2.LocationId='{3}')
                                -(select isnull(sum(Quantity),0) from dbo.AssignedInventoryTechReceived b where b.EquipmentId = eq.EquipmentId and b.TechnicianId  = '{3}' and b.IsApprove = 0 and b.IsDecline = 0)) as WarehouseQTY
                                ,mf.Name as ManufacturerName
                                ,eq.Point
                               ,ISNULL((select top(1) ev.Cost from EquipmentVendor ev  where ev.EquipmentId = eq.EquipmentId and IsPrimary = 1),0) as EquipmentVendorCost

                                from Equipment eq
								LEFT JOIN InventoryWarehouse itech on itech.EquipmentId=eq.EquipmentId
                                left join EquipmentType et
                                on et.Id= eq.EquipmentTypeId
                                left join Manufacturer mf
								on mf.Id=eq.ManufacturerId
                                where eq.EquipmentClassId=1 
                                and (eq.Name like @SerchText OR eq.SKU like @SerchText  OR eq.BarCode like @SerchText)
                                and eq.CompanyId ='{1}' and eq.IsActive=1
                                {4}";
            var EqpExist = "";
            //if (!string.IsNullOrEmpty(ExistEquipment))
            //{
            //    EqpExist = string.Format("AND eq.EquipmentId not in {0}", ExistEquipment);
            //}
            try
            {
                sqlQuery = string.Format(sqlQuery, key, companyId, MaxLoad, technicianId, EqpExist);
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

        public DataSet GetWHHistoryITechStockByFilters(DetailedHistoryFilter filters)
        {
            //_inv.TechnicianId,

            string sqlQuery = @"
                                    SELECT
                                        _eqp.Id,
                                        _eqpType.Name as Category,
                                        _eqp.Name as Description,
                                        _eqp.EquipmentId,
                                            manu.Id as MfgId,
	                                        manu.Name as Manufacturer,
	                                        _eqp.SKU,
                                            
                                        ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where LastUpdatedDate < '{9}' AND invinner.EquipmentId=_eqp.EquipmentId and Type='Add' AND ({7}))-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where LastUpdatedDate < '{9}' AND invinner.EquipmentId=_eqp.EquipmentId and Type='Release' AND ({7}))) OpnBal,
                                        (Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release' AND LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description LIKE '%ticket%' AND ({7})) Added,
                                        (Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add' AND LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description LIKE '%ticket%' AND ({7})) Pulled,
                                        (Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release' AND LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description LIKE '%warehouse%' ) WH_In,
                                        (Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add' AND LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description LIKE '%warehouse%' ) WH_Out,
                                        ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where LastUpdatedDate <= '{10}' AND invinner.EquipmentId=_eqp.EquipmentId and Type='Add' AND ({7}))-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where LastUpdatedDate <= '{10}' AND invinner.EquipmentId=_eqp.EquipmentId and Type='Release' AND ({7}))) OnHand,
                                        _eqp.RepCost
                                        ,isNULL(_eqp.Name, '') +
                                        isNULL(_eqp.SKU,'') + 
                                        isNULL(_eqpType.Name,'') + 
                                        isNULL(sup.CompanyName,'') +
                                        isNULL(manu.Name,'') + 
                                        isNULL(_eqpClass.Name,'')  FilterText
                                            INTO #CustomerData
                                            FROM InventoryTech _inv
                                            LEFT JOIN Equipment _eqp
                                            ON _eqp.CompanyId = _inv.CompanyId and _eqp.EquipmentId = _inv.EquipmentId
                                            LEFT JOIN EquipmentClass _eqpClass
	                                        ON _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                                            left join Supplier sup on sup.Id = _eqp.SupplierId
                                            LEFT JOIN EquipmentType _eqpType
	                                        ON _eqp.EquipmentTypeId = _eqpType.Id and _eqp.CompanyId = _eqpType.CompanyId		                                    
                                            left join Manufacturer manu
	                                        ON manu.Id = _eqp.ManufacturerId
                                                WHERE 
		                                        _eqp.CompanyId = '{0}'
                                                AND _eqp.IsActive = 1
                                                AND ({7})
                                                
                                            SELECT DISTINCT * INTO #CustomerFilterData1
                                            FROM #CustomerData
                                            where (OpnBal>0 OR Added>0 OR Pulled>0 OR TrfOut>0 OR TrfIn>0 OR OnHand>0)
                                            
                                            SELECT DISTINCT * INTO #CustomerFilterData
                                            FROM #CustomerFilterData1
                                            {11}
                                            {12}

	                                        SELECT
                                            Id,
                                            EquipmentId,
	                                        Description,
	                                        SKU,
                                            MfgId,
                                            Manufacturer,
	                                        OpnBal,
	                                        Added,
	                                        Pulled,
	                                        TrfOut,
	                                        TrfIn,
                                            WH_In,
                                            WH_Out,
	                                        OnHand,
	                                        ((OpnBal-Added+Pulled-TrfOut+TrfIn)-OnHand) Diff,
	                                        FilterText
	                                        FROM #CustomerFilterData _cfdL
                                            order by _cfdL.Manufacturer

                                            SELECT
                                            count(*)
	                                        FROM #CustomerFilterData _cfdC

                                            SELECT DISTINCT
                                            Id as Value,
                                            Description as Text 
                                            FROM #CustomerFilterData1 _cfdE

                                            SELECT DISTINCT
                                            MfgId as Value,
                                            Manufacturer as Text 
                                            FROM #CustomerFilterData1 _cfdM

                                            DROP TABLE #CustomerData
                                            DROP TABLE #CustomerFilterData
                                            DROP TABLE #CustomerFilterData1";

            //-- OFFSET {11} ROWS FETCH NEXT {12} ROWS ONLY
            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string orderquery = "order by _cfd.Id desc";
            string filter_MultipleTechs = "AND";
            string filter_MultipleEquipments = "WHERE (";
            string filter_MultipleManufactures = "OR (";

            foreach (var tech in filters.EmployeeIds)
            {
                filter_MultipleTechs += string.Format("OR TechnicianId='{0}' ", tech);
            }
            filter_MultipleTechs = filter_MultipleTechs.Replace("ANDOR", " ");

            foreach (var equip in filters.EquipmentIds)
            {
                filter_MultipleEquipments += string.Format("OR Id = '{0}' ", equip);
            }
            filter_MultipleEquipments = filter_MultipleEquipments.Replace("(OR", "( ");

            if (!filter_MultipleEquipments.Contains("undefined"))
            {
                filter_MultipleEquipments += " )";
            }
            else
            {
                filter_MultipleEquipments = " ";
            }

            foreach (var equip in filters.ManufacturerIds)
            {
                filter_MultipleManufactures += string.Format("OR Id = '{0}' ", equip);
            }
            filter_MultipleManufactures = filter_MultipleManufactures.Replace("(OR", "( ");

            if (!filter_MultipleManufactures.Contains("undefined"))
            {
                filter_MultipleManufactures += " )";
            }
            else
            {
                filter_MultipleManufactures = " ";
            }

            if (!string.IsNullOrWhiteSpace(filters.SearchText))
            {
                filterColumntext = @",isNULL(_eqp.Name, '') +
								isNULL(_eqp.SKU,'') + 
                                isNULL(_eqpType.Name,'') + 
                                isNULL(sup.CompanyName,'') +
                                isNULL(manu.Name,'') + 
								isNULL(_eqpClass.Name,'')  FilterText  ";
                filtertext = string.Format(" Where FilterText like '%{0}%' ", filters.SearchText);
            }

            filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");

            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filters.CompanyId,
                        filterColumntext,
                        filtertext,
                        filterByActiveStatus,
                        filterByEquipmentClass,
                        filterByEquipmentType,
                        filterByStockStatus,
                        filter_MultipleTechs,
                        orderquery,
                        filters.Start,
                        filters.End,
                        filter_MultipleEquipments,
                        filter_MultipleManufactures); //,
                                                      //(filters.PageNo - 1) * filters.PageSize,
                                                      //filters.PageSize);
                                                      //LogError(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "InventoryDataAccess-GetTechTransferListByFilters", Message = string.Format("{0} || {1}", "QLog", sqlQuery), TimeUtc = DateTime.Now });
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                _ErrorLogDataAccess.Insert(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "DA|Inventory|GetDetailedHistoryListByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                return null;
            }
        }
        public DataSet GetWHHistoryIWHStockListByFilters(DetailedHistoryFilter filters)
        {
            //_inv.TechnicianId,

            string sqlQuery = @"
                                    SELECT
                                        _eqp.Id,
                                        _eqpType.Name as Category,
                                        _eqp.Name as Description,
                                        _eqp.EquipmentId,
                                            manu.Id as MfgId,
	                                        manu.Name as Manufacturer,
	                                        _eqp.SKU,
                                            
                                        ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where LastUpdatedDate < '{9}' AND invinner.EquipmentId=_eqp.EquipmentId and Type='Add' AND ({7}))-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where LastUpdatedDate < '{9}' AND invinner.EquipmentId=_eqp.EquipmentId and Type='Release' AND ({7}))) OpnBal,
                                        (Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release' AND LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description LIKE '%Removed%' AND ({7})) Pulled,
                                        (Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add' AND LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description LIKE '%Added%' AND ({7})) Added,
                                        (Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release' AND LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description NOT LIKE '%ticket%' AND ({7})) WH_Out,
                                        (Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add' AND LastUpdatedDate BETWEEN '{9}' AND '{10}' AND Description NOT LIKE '%ticket%' AND ({7})) WH_In,
                                        ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where  {13} invinner.EquipmentId=_eqp.EquipmentId and Type='Add' AND ({7}))-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where LastUpdatedDate <= '{10}' AND invinner.EquipmentId=_eqp.EquipmentId and Type='Release' AND ({7}) )) OnHand,
                                    
                                        _eqp.RepCost
                                        ,isNULL(_eqp.Name, '') +
                                        isNULL(_eqp.SKU,'') + 
                                        isNULL(_eqpType.Name,'') + 
                                        isNULL(sup.CompanyName,'') +
                                        isNULL(manu.Name,'') + 
                                        isNULL(_eqpClass.Name,'')  FilterText
                                            INTO #CustomerData
                                            FROM InventoryWarehouse _inv
                                            LEFT JOIN Equipment _eqp
                                            ON _eqp.CompanyId = _inv.CompanyId and _eqp.EquipmentId = _inv.EquipmentId
                                            LEFT JOIN EquipmentClass _eqpClass
	                                        ON _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                                            left join Supplier sup on sup.Id = _eqp.SupplierId
                                            LEFT JOIN EquipmentType _eqpType
	                                        ON _eqp.EquipmentTypeId = _eqpType.Id and _eqp.CompanyId = _eqpType.CompanyId		                                    
                                            left join Manufacturer manu
	                                        ON manu.Id = _eqp.ManufacturerId
                                                WHERE 
		                                        _eqp.CompanyId = '{0}'
                                                AND _eqp.IsActive = 1
                                                   AND ({7})
                                                
                                                
                                            SELECT DISTINCT * INTO #CustomerFilterData1
                                            FROM #CustomerData
                                            where (OpnBal>0 OR Added>0 OR Pulled>0 OR WH_Out>0 OR WH_In>0 OR OnHand>0)
                                            
                                            SELECT DISTINCT * INTO #CustomerFilterData
                                            FROM #CustomerFilterData1
                                            {11}
                                            {12}

	                                        SELECT
                                            Id,
                                            EquipmentId,
	                                        Description,
	                                        SKU,
                                            MfgId,
                                            Manufacturer,
	                                        OpnBal,
	                                        Added,
	                                        Pulled,
                                            WH_In,
                                            WH_Out,
	                                        OnHand,
	                                        ((OpnBal-Added+Pulled-WH_Out+WH_In)-OnHand) Diff,
	                                        FilterText
	                                        FROM #CustomerFilterData _cfdL
                                            order by _cfdL.Manufacturer

                                            SELECT
                                            count(*)
	                                        FROM #CustomerFilterData _cfdC

                                            SELECT DISTINCT
                                            Id as Value,
                                            Description as Text 
                                            FROM #CustomerFilterData1 _cfdE

                                            SELECT DISTINCT
                                            MfgId as Value,
                                            Manufacturer as Text 
                                            FROM #CustomerFilterData1 _cfdM

                                            DROP TABLE #CustomerData
                                            DROP TABLE #CustomerFilterData
                                            DROP TABLE #CustomerFilterData1";

            //-- OFFSET {11} ROWS FETCH NEXT {12} ROWS ONLY
            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string orderquery = "order by _cfd.Id desc";
            string filter_MultipleTechs = "AND";
            string filter_MultipleEquipments = "WHERE (";
            string filter_MultipleManufactures = "AND (";

            foreach (var tech in filters.EmployeeIds)
            {
                filter_MultipleTechs += string.Format("OR LocationId='{0}' ", tech);
            }
            filter_MultipleTechs = filter_MultipleTechs.Replace("ANDOR", " ");

            foreach (var equip in filters.EquipmentIds)
            {
                filter_MultipleEquipments += string.Format("OR Id = '{0}' ", equip);
            }
            filter_MultipleEquipments = filter_MultipleEquipments.Replace("(OR", "( ");

            if (!filter_MultipleEquipments.Contains("undefined"))
            {
                filter_MultipleEquipments += " )";
            }
            else
            {
                filter_MultipleEquipments = " ";
            }

            foreach (var equip in filters.ManufacturerIds)
            {
                filter_MultipleManufactures += string.Format("OR MfgId = '{0}' ", equip);
            }
            filter_MultipleManufactures = filter_MultipleManufactures.Replace("(OR", "( ");

            if (!filter_MultipleManufactures.Contains("undefined"))
            {
                filter_MultipleManufactures += " )";
            }
            else
            {
                filter_MultipleManufactures = " ";
            }

            if (!string.IsNullOrWhiteSpace(filters.SearchText))
            {
                filterColumntext = @",isNULL(_eqp.Name, '') +
								isNULL(_eqp.SKU,'') + 
                                isNULL(_eqpType.Name,'') + 
                                isNULL(sup.CompanyName,'') +
                                isNULL(manu.Name,'') + 
								isNULL(_eqpClass.Name,'')  FilterText  ";
                filtertext = string.Format(" Where FilterText like '%{0}%' ", filters.SearchText);
            }

            filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");
            filters.Start = !string.IsNullOrWhiteSpace(filters.Start) ? Convert.ToDateTime(filters.Start).SetZeroHour().ToString() : filters.Start;
            filters.End = !string.IsNullOrWhiteSpace(filters.End) ? Convert.ToDateTime(filters.End).SetMaxHour().ToString() : filters.End;

            filters.Start = (filters.Start == null || filters.Start.Contains("01/01/0001"))? "" : filters.Start;
            filters.End = (filters.End == null || filters.End.Contains("01/01/0001")) ? "" : filters.End;
            
            string Datenull = (filters.End != "") ? "LastUpdatedDate <= ' " + filters.End + "' And " : filters.End;
            
            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filters.CompanyId,
                        filterColumntext,
                        filtertext,
                        filterByActiveStatus,
                        filterByEquipmentClass,
                        filterByEquipmentType,
                        filterByStockStatus,
                        filter_MultipleTechs,
                        orderquery,
                        filters.Start,
                        filters.End,
                        filter_MultipleEquipments,
                        filter_MultipleManufactures,Datenull); //,
                                                      //(filters.PageNo - 1) * filters.PageSize,
                                                      //filters.PageSize);
                                                      //LogError(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "InventoryDataAccess-GetTechTransferListByFilters", Message = string.Format("{0} || {1}", "QLog", sqlQuery), TimeUtc = DateTime.Now });
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                _ErrorLogDataAccess.Insert(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "DA|Inventory|GetDetailedHistoryListByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                return null;
            }
        }

        public DataSet GetWHHistoryPOListByFilters(DetailedHistoryFilter filters)
        {
            string sqlQuery = @"
                                --select
                                 --(
                                     --SELECT COUNT(*) 
                                     --FROM InventoryWarehouse invtech
                                     --LEFT JOIN PurchaseOrderWarehouse po ON po.PurchaseOrderId = invtech.PurchaseOrderId
                                     --LEFT JOIN Equipment eq ON eq.EquipmentId = invtech.EquipmentId
                                     --WHERE invtech.CompanyId = 'c7e72006-6edf-4b5a-8589-bfd1b2dae7ba'
                                       --and ({13})
                                       --AND eq.EquipmentId = invtech.EquipmentId
                                       --AND invtech.LastUpdatedDate BETWEEN '{9}' AND '{10}'
                                       --AND (invtech.Description NOT LIKE '%technician%' AND invtech.Description NOT LIKE '%Tech%' AND invtech.Description NOT LIKE '%WHWH%')) AS Records;



                                    select 
                                    invtech.LastUpdatedDate as Date,
                                    eq.Id,
                                    eq.EquipmentId,
                                    eq.Name,
                                    eq.SKU,
                                    invtech.Description,
                                    po.PurchaseOrderId as PONo,
                                    po.Id as POId,
                                    invtech.Type,
                                    CASE WHEN invtech.Type='Release'
	                                    THEN 0-invtech.Quantity
	                                    ELSE invtech.Quantity
                                    END as Quantity
                                    from InventoryWarehouse invtech
                                    LEFT JOIN PurchaseOrderWarehouse po on po.PurchaseOrderId=invtech.PurchaseOrderId
                                    LEFT JOIN Equipment eq on eq.EquipmentId=invtech.EquipmentId
                                    where invtech.CompanyId='{0}' and eq.EquipmentClassId = 1 
                                    and ({13})
                                    AND invtech.LastUpdatedDate BETWEEN '{9}' AND '{10}'
                                    AND (invtech.Description NOT LIKE '%technician%' AND invtech.Description NOT LIKE '%Tech%' and invtech.Description NOT LIKE '%WHWH%')
                                    order by Date
                                    OFFSET {11} ROWS FETCH NEXT {12} ROWS ONLY

                                    select 
                                    COUNT(*) as Records
                                    from InventoryWarehouse invtech
                                    LEFT JOIN PurchaseOrder po on po.PurchaseOrderId=invtech.PurchaseOrderId
                                    LEFT JOIN Equipment eq on eq.EquipmentId=invtech.EquipmentId
                                    where invtech.CompanyId='{0}' and eq.EquipmentClassId = 1 
                                    and ({13})
                                    AND invtech.LastUpdatedDate BETWEEN '{9}' AND '{10}'
                                    AND invtech.Description LIKE '%ticket%'";

            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string orderquery = "order by _cfd.Id desc";
            string filter_MultipleTechs = "AND";
            string filter_MultipleEquipments = "AND";

            foreach (var tech in filters.EmployeeIds)
            {
                filter_MultipleTechs += string.Format("OR em.UserId = '{0}' ", tech);
            }
            filter_MultipleTechs = filter_MultipleTechs.Replace("ANDOR", " ");

            foreach (var equip in filters.EquipmentIds)
            {
                filter_MultipleEquipments += string.Format("OR eq.EquipmentId = '{0}' ", equip);
            }
            filter_MultipleEquipments = filter_MultipleEquipments.Replace("ANDOR", " ");

            filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");
            filters.Start = !string.IsNullOrWhiteSpace(filters.Start) ? Convert.ToDateTime(filters.Start).SetZeroHour().ToString() : filters.Start;
            filters.End = !string.IsNullOrWhiteSpace(filters.End) ? Convert.ToDateTime(filters.End).SetMaxHour().ToString() : filters.End;

           
            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filters.CompanyId,
                        filterColumntext,
                        filtertext,
                        filterByActiveStatus,
                        filterByEquipmentClass,
                        filterByEquipmentType,
                        filterByStockStatus,
                        filter_MultipleTechs,
                        orderquery,
                        filters.Start,
                        filters.End,
                        (filters.PageNo - 1) * filters.PageSize,
                        filters.PageSize,
                        filter_MultipleEquipments);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                //LogError(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "InventoryDataAccess-GetTechTransferListByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                _ErrorLogDataAccess.Insert(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "DA|Inventory|GetWHHistoryPOListByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                return null;
            }
        }

        public DataSet GetWHHistoryPOListByFilters_old(DetailedHistoryFilter filters)
        {
            string sqlQuery = @"
                                select
                                 (SELECT COUNT(*) 
                                     FROM InventoryWarehouse invtech
                                     LEFT JOIN PurchaseOrderWarehouse po ON po.PurchaseOrderId = invtech.PurchaseOrderId
                                     LEFT JOIN Equipment eq ON eq.EquipmentId = invtech.EquipmentId
                                     WHERE invtech.CompanyId = 'c7e72006-6edf-4b5a-8589-bfd1b2dae7ba'
                                       and ({13})
                                       AND eq.EquipmentId = invtech.EquipmentId
                                       AND invtech.LastUpdatedDate BETWEEN '{9}' AND '{10}'
                                       AND (invtech.Description NOT LIKE '%technician%' AND invtech.Description NOT LIKE '%Tech%' AND invtech.Description NOT LIKE '%WHWH%')) AS Records;



                                    select 
                                    DATEADD(MI, DATEDIFF(MI, GETUTCDATE(), GETDATE()), invtech.LastUpdatedDate) as Date,
                                    eq.Id,
                                    eq.EquipmentId,
                                    eq.Name,
                                    eq.SKU,
                                    invtech.Description,
                                    po.PurchaseOrderId as PONo,
                                    po.Id as POId,
                                    invtech.Type,
                                    CASE WHEN invtech.Type='Release'
	                                    THEN 0-invtech.Quantity
	                                    ELSE invtech.Quantity
                                    END as Quantity
                                    from InventoryWarehouse invtech
                                    LEFT JOIN PurchaseOrderWarehouse po on po.PurchaseOrderId=invtech.PurchaseOrderId
                                    LEFT JOIN Equipment eq on eq.EquipmentId=invtech.EquipmentId
                                    where invtech.CompanyId='{0}' and eq.EquipmentClassId = 1 
                                    and ({13})
                                    AND invtech.LastUpdatedDate BETWEEN '{9}' AND '{10}'
                                    AND (invtech.Description NOT LIKE '%technician%' AND invtech.Description NOT LIKE '%Tech%' and invtech.Description NOT LIKE '%WHWH%')
                                    order by Date
                                    OFFSET {11} ROWS FETCH NEXT {12} ROWS ONLY

                                   -- select 
                                    --COUNT(*) as Records
                                    --from InventoryWarehouse invtech
                                    --LEFT JOIN PurchaseOrder po on po.PurchaseOrderId=invtech.PurchaseOrderId
                                    --LEFT JOIN Equipment eq on eq.EquipmentId=invtech.EquipmentId
                                    --where invtech.CompanyId='{0}' and eq.EquipmentClassId = 1 
                                    ---and ({13})
                                    --AND invtech.LastUpdatedDate BETWEEN '{9}' AND '{10}'
                                    --AND invtech.Description LIKE '%ticket%'";

            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string orderquery = "order by _cfd.Id desc";
            string filter_MultipleTechs = "AND";
            string filter_MultipleEquipments = "AND";

            foreach (var tech in filters.EmployeeIds)
            {
                filter_MultipleTechs += string.Format("OR em.UserId = '{0}' ", tech);
            }
            filter_MultipleTechs = filter_MultipleTechs.Replace("ANDOR", " ");

            foreach (var equip in filters.EquipmentIds)
            {
                filter_MultipleEquipments += string.Format("OR eq.EquipmentId = '{0}' ", equip);
            }
            filter_MultipleEquipments = filter_MultipleEquipments.Replace("ANDOR", " ");

            filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");
            filters.Start = !string.IsNullOrWhiteSpace(filters.Start) ? Convert.ToDateTime(filters.Start).SetZeroHour().ToString() : filters.Start;
            filters.End = !string.IsNullOrWhiteSpace(filters.End) ? Convert.ToDateTime(filters.End).SetMaxHour().ToString() : filters.End;


            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filters.CompanyId,
                        filterColumntext,
                        filtertext,
                        filterByActiveStatus,
                        filterByEquipmentClass,
                        filterByEquipmentType,
                        filterByStockStatus,
                        filter_MultipleTechs,
                        orderquery,
                        filters.Start,
                        filters.End,
                        (filters.PageNo - 1) * filters.PageSize,
                        filters.PageSize,
                        filter_MultipleEquipments);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                //LogError(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "InventoryDataAccess-GetTechTransferListByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                _ErrorLogDataAccess.Insert(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "DA|Inventory|GetWHHistoryPOListByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                return null;
            }
        }

        public DataSet GetWHHistoryTransfersListByFilters(DetailedHistoryFilter filters)
        {
            // old query


            //        string sqlQuery = @"

            //            select RCV.Quantity, RCV.EquipmentId, RCV.Name as Equipment, RCV.SKU, RCV.Type as [Add], RCV.Tech as RTech, tmp.Type as Release, 
            //            tmp.Tech  as TTech, DATEADD(MI, DATEDIFF(MI, GETUTCDATE(), GETDATE()), RCV.LastUpdatedDate) as RDate, 
            //            tmp.LastUpdatedDate as TDate, RCV.Description +' | '+ tmp.Description as Description 
            //            INTO #WHTrf
            //            from ( Select invinner.Quantity, invinner.TechnicianId,invinner.EquipmentId,invinner.Type,invinner.Description,
            //            (select case when te.IsLocation=1 then te.UserName else te.FirstName + ' ' + te.LastName end from Employee te where te.UserId=invinner.TechnicianId) as Tech,
            //            eq.Name, eq.SKU, invinner.LastUpdatedDate 
            //            from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
            //            where( {13} ) and invinner.Type='Add' 
            //            AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' 
            //            AND (Description NOT LIKE '%warehouse%' AND Description NOT LIKE '%WH%' AND Description NOT LIKE '%ticket%'  AND Description NOT  LIKE '%technician%')
            //            ) as RCV
            //              full outer join 
            //            (
            //            Select invinner.Quantity, 
            //            invinner.TechnicianId,invinner.EquipmentId,invinner.Type,invinner.Description,
            //            (select case when te.IsLocation=1 then te.UserName else te.FirstName + ' ' + te.LastName end from Employee te where te.UserId=invinner.TechnicianId) as Tech,
            //            eq.Name, eq.SKU, invinner.LastUpdatedDate 
            //            from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
            //            where( {13} ) and invinner.Type='Release' 
            //            AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' 
            //            AND (Description NOT LIKE '%warehouse%' AND Description NOT LIKE '%WH%' AND Description NOT LIKE '%ticket%' AND Description NOT  LIKE '%technician%')
            //            ) as tmp
            //            ON RCV.EquipmentId=tmp.EquipmentId
            //            order by RCV.LastUpdatedDate, tmp.LastUpdatedDate

            //            Select distinct invinner.Quantity, 
            //            invinner.EquipmentId,  eq.Name as Equipment, eq.SKU, invinner.Type as [Add], 
            //            (select case when te.IsLocation=1 then te.UserName else te.FirstName + ' ' + te.LastName end from Employee te where te.UserId=invinner.TechnicianId) as RTech, 'Release' as Release,
            //(select distinct case when te.IsLocation=1 then te.UserName else te.FirstName + ' ' + te.LastName end from Employee te where te.UserId=iw.LocationId) as TTech,
            //            DATEADD(MI, DATEDIFF(MI, GETUTCDATE(), GETDATE()), invinner.LastUpdatedDate) as RDate, 
            //            invinner.LastUpdatedDate TDate, invinner.Description
            //            from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
            //             JOIN InventoryWarehouse iw ON    eq.EquipmentId= iw.EquipmentId 
            //      join AssignedInventoryTechReceived assign on invinner.TechnicianId=assign.TechnicianId
            //            where( {13} ) and invinner.Type='Add'   and  iw.Type='Release'
            //            AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' AND (invinner.Description LIKE '%warehouse%' OR invinner.Description LIKE '%WH%')
            //            union all
            //            Select invinner.Quantity, 
            //            invinner.EquipmentId,  eq.Name as Equipment, eq.SKU, 'Add' as [Add], 
            //             'Warehouse' as RTech, invinner.Type as Release, 
            //            (select case when te.IsLocation=1 then te.UserName else te.FirstName + ' ' + te.LastName end from Employee te where te.UserId=invinner.TechnicianId) as TTech, 
            //            DATEADD(MI, DATEDIFF(MI, GETUTCDATE(), GETDATE()), invinner.LastUpdatedDate) as RDate, 
            //            invinner.LastUpdatedDate TDate, invinner.Description
            //            from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
            //            where( {13} ) and invinner.Type='Release' 
            //            AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' AND (Description LIKE '%warehouse%' OR Description LIKE '%WH%')
            //            union all
            //            select * from #WHTrf

            //            DROP TABLE #WHTrf

            //        ";

            string sqlQuery = @"


                                                  WITH RankedResults AS (
                                                SELECT
                                                    assinITR.Quantity,
                                                    assinITR.EquipmentId,
                                                    eq.Name as Equipment,
                                                    eq.SKU,
                                                    'Add' as [Add],
                                                    (SELECT CASE WHEN te.IsLocation=1 THEN te.UserName ELSE te.FirstName + ' ' + te.LastName END FROM Employee te WHERE te.UserId=assinITR.ReceivedBy) AS RTech,
                                                    'Release' AS Release,
                                                    COALESCE((SELECT CASE WHEN te.IsLocation=1 THEN te.UserName ELSE te.FirstName + ' ' + te.LastName END FROM Employee te WHERE te.UserId=assinITR.TechnicianId),'Purchase Order') AS TTech,
                                                    assinITR.ReceivedDate AS RDate,
                                                    assinITR.CreatedDate AS TDate,
                                                    '' as Description,
                                                 --   ROW_NUMBER() OVER (PARTITION BY assinITR.Quantity, assinITR.EquipmentId, eq.Name, eq.SKU, assinITR.ReceivedDate ORDER BY assinITR.CreatedDate) AS RowNum,
                                                    assinITR.ReqSrc as ReqSrc
        									   FROM
                                                    AssignedInventoryTechReceived assinITR
                                                LEFT JOIN Equipment eq ON eq.EquipmentId = assinITR.EquipmentId

                                                WHERE
                                                    {13} 

                                                    AND ReceivedDate BETWEEN '{9}' AND '{10}'
                                                    AND IsApprove=1
                                                    AND Quantity > 0
                       --                                   AND (
                       --                                             TRIM(assinITR.ReqSrc) like '%WHTT-Approve%'
                       --                                             OR TRIM(assinITR.ReqSrc) like '%TTWH-Approve%'
                       --                                             OR TRIM(assinITR.ReqSrc) like '%[SLTT-Approve]%'
                       --                                             OR TRIM(assinITR.ReqSrc) like '%[TTSL-Approve]%'
        															--or TRIM(assinITR.ReqSrc) like '%[TTSL-Approve]%'
                       --                                             OR TRIM(assinITR.ReqSrc) = ''
                       --                                         )
        		                                                AND (
        			                                                  TRIM(assinITR.ReqSrc) <> '[TT-Approve]'
        			                                                  and TRIM(assinITR.ReqSrc) <> '[PURORD-WHTT]'
        			                                             )
                                                            AND (
                                                                {14}
                                                            )
                                            )
                                            SELECT 
        									ReqSrc,
                                                Quantity,
                                                EquipmentId,
                                                Equipment,
                                                SKU,
                                                [Add],
                                                RTech,
                                                [Release],
                                                TTech,
                                                RDate,
                                                TDate,
                                                '' as Description
                                            FROM RankedResults

                                           -- WHERE RowNum = 1
                                            ORDER BY RDate desc;
                                           --OFFSET {11} ROWS FETCH NEXT {12} ROWS ONLY ;
        								  --SELECT COUNT(*) AS TotalCount 
        --FROM (
            --SELECT
               -- assinITR.Quantity,
                --assinITR.EquipmentId,
                --eq.Name as Equipment,
                --eq.SKU,
                --'Add' as [Add],
                --(SELECT CASE WHEN te.IsLocation=1 THEN te.UserName ELSE te.FirstName + ' ' + te.LastName END FROM Employee te WHERE te.UserId=assinITR.ReceivedBy) AS RTech,
                --'Release' AS Release,
                --COALESCE((SELECT CASE WHEN te.IsLocation=1 THEN te.UserName ELSE te.FirstName + ' ' + te.LastName END FROM Employee te WHERE te.UserId=assinITR.TechnicianId),'Purchase Order') AS TTech,
                --assinITR.ReceivedDate AS RDate,
                --assinITR.CreatedDate AS TDate,
                --'' as Description,
              --  assinITR.ReqSrc as ReqSrc
            --FROM
              --  AssignedInventoryTechReceived assinITR
            --LEFT JOIN Equipment eq ON eq.EquipmentId = assinITR.EquipmentId
            --WHERE
           --     {13}
         --AND ReceivedDate BETWEEN '{9}' AND '{10}'   
        --AND IsApprove = 1
               -- AND (
                   -- TRIM(assinITR.ReqSrc) <> '[TT-Approve]'
                   -- AND TRIM(assinITR.ReqSrc) <> '[PURORD-WHTT]'
               -- )
        --) AS CountedResults;






                    ";

            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string orderquery = "order by _cfd.Id desc";
            string filter_MultipleTechs = "AND";
            string filter_MultipleEquipments = "AND";

            foreach (var tech in filters.EmployeeIds)
            {
                filter_MultipleTechs += string.Format("OR invinner.TechnicianId = '{0}' ", tech);
            }
            filter_MultipleTechs = filter_MultipleTechs.Replace("ANDOR", " ");

            foreach (var equip in filters.EquipmentIds)
            {
                filter_MultipleEquipments += string.Format("OR assinITR.EquipmentId= '{0}' ", equip);
            }
            filter_MultipleEquipments = filter_MultipleEquipments.Replace("ANDOR", " ");
            string employeeCondition = string.Join(" OR ", filters.EmployeeIds.Select(id =>
                 $"assinITR.ReceivedBy = '{id}' OR assinITR.TechnicianId = '{id}'"
             ));

            filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");

            filters.Start = !string.IsNullOrWhiteSpace(filters.Start) ? Convert.ToDateTime(filters.Start).SetZeroHour().ToString() : filters.Start;
            filters.End = !string.IsNullOrWhiteSpace(filters.End) ? Convert.ToDateTime(filters.End).SetMaxHour().ToString() : filters.End;


            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filters.CompanyId,
                        filterColumntext,
                        filtertext,
                        filterByActiveStatus,
                        filterByEquipmentClass,
                        filterByEquipmentType,
                        filterByStockStatus,
                        filter_MultipleTechs,
                        orderquery,
                        filters.Start,
                        filters.End,
                        (filters.PageNoTransfers - 1) * filters.PageSize,
                        filters.PageSize,
                        filter_MultipleEquipments,
                       employeeCondition);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                //LogError(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "InventoryDataAccess-GetTechTransferListByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                _ErrorLogDataAccess.Insert(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "DA|Inventory|GetWHHistoryTransfersListByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                return null;
            }
        }
        public DataSet GetWHHistoryTransfersListByFilters_old(DetailedHistoryFilter filters)
        {
            // old query


            string sqlQuery = @"
                    
                select RCV.Quantity, RCV.EquipmentId, RCV.Name as Equipment, RCV.SKU, RCV.Type as [Add], RCV.Tech as RTech, tmp.Type as Release, 
                tmp.Tech  as TTech, DATEADD(MI,RCV.LastUpdatedDate as RDate, 
                tmp.LastUpdatedDate as TDate, RCV.Description +' | '+ tmp.Description as Description 
                INTO #WHTrf
                from ( Select invinner.Quantity, invinner.TechnicianId,invinner.EquipmentId,invinner.Type,invinner.Description,
                (select case when te.IsLocation=1 then te.UserName else te.FirstName + ' ' + te.LastName end from Employee te where te.UserId=invinner.TechnicianId) as Tech,
                eq.Name, eq.SKU, invinner.LastUpdatedDate 
                from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
                where( {13} ) and invinner.Type='Add' 
                AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' 
                AND (Description NOT LIKE '%warehouse%' AND Description NOT LIKE '%WH%' AND Description NOT LIKE '%ticket%'  AND Description NOT  LIKE '%technician%')
                ) as RCV
                  full outer join 
                (
                Select invinner.Quantity, 
                invinner.TechnicianId,invinner.EquipmentId,invinner.Type,invinner.Description,
                (select case when te.IsLocation=1 then te.UserName else te.FirstName + ' ' + te.LastName end from Employee te where te.UserId=invinner.TechnicianId) as Tech,
                eq.Name, eq.SKU, invinner.LastUpdatedDate 
                from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
                where( {13} ) and invinner.Type='Release' 
                AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' 
                AND (Description NOT LIKE '%warehouse%' AND Description NOT LIKE '%WH%' AND Description NOT LIKE '%ticket%' AND Description NOT  LIKE '%technician%')
                ) as tmp
                ON RCV.EquipmentId=tmp.EquipmentId
                order by RCV.LastUpdatedDate, tmp.LastUpdatedDate

                Select distinct invinner.Quantity, 
                invinner.EquipmentId,  eq.Name as Equipment, eq.SKU, invinner.Type as [Add], 
                (select case when te.IsLocation=1 then te.UserName else te.FirstName + ' ' + te.LastName end from Employee te where te.UserId=invinner.TechnicianId) as RTech, 'Release' as Release,
				(select distinct case when te.IsLocation=1 then te.UserName else te.FirstName + ' ' + te.LastName end from Employee te where te.UserId=iw.LocationId) as TTech,
                DATEADD(MI, DATEDIFF(MI, GETUTCDATE(), GETDATE()), invinner.LastUpdatedDate) as RDate, 
                invinner.LastUpdatedDate TDate, invinner.Description
                from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
                 JOIN InventoryWarehouse iw ON    eq.EquipmentId= iw.EquipmentId 
		        join AssignedInventoryTechReceived assign on invinner.TechnicianId=assign.TechnicianId
                where( {13} ) and invinner.Type='Add'   and  iw.Type='Release'
                AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' AND (invinner.Description LIKE '%warehouse%' OR invinner.Description LIKE '%WH%')
                union all
                Select invinner.Quantity, 
                invinner.EquipmentId,  eq.Name as Equipment, eq.SKU, 'Add' as [Add], 
                 'Warehouse' as RTech, invinner.Type as Release, 
                (select case when te.IsLocation=1 then te.UserName else te.FirstName + ' ' + te.LastName end from Employee te where te.UserId=invinner.TechnicianId) as TTech, 
                DATEADD(MI, DATEDIFF(MI, GETUTCDATE(), GETDATE()), invinner.LastUpdatedDate) as RDate, 
                invinner.LastUpdatedDate TDate, invinner.Description
                from InventoryTech invinner left join Equipment eq on eq.EquipmentId=invinner.EquipmentId 
                where( {13} ) and invinner.Type='Release' 
                AND invinner.LastUpdatedDate BETWEEN '{9}' AND '{10}' AND (Description LIKE '%warehouse%' OR Description LIKE '%WH%')
                union all
                select * from #WHTrf
                
                DROP TABLE #WHTrf

            ";

            string filtertext = "";
            string filterColumntext = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";
            string filterByStockStatus = "";
            string orderquery = "order by _cfd.Id desc";
            string filter_MultipleTechs = "AND";
            string filter_MultipleEquipments = "AND";

            foreach (var tech in filters.EmployeeIds)
            {
                filter_MultipleTechs += string.Format("OR invinner.TechnicianId = '{0}' ", tech);
            }
            filter_MultipleTechs = filter_MultipleTechs.Replace("ANDOR", " ");

            foreach (var equip in filters.EquipmentIds)
            {
                filter_MultipleEquipments += string.Format("OR invinner.EquipmentId= '{0}' ", equip);
            }
            filter_MultipleEquipments = filter_MultipleEquipments.Replace("ANDOR", " ");

            filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");
            filters.Start = !string.IsNullOrWhiteSpace(filters.Start) ? Convert.ToDateTime(filters.Start).SetZeroHour().ToString() : filters.Start;
            filters.End = !string.IsNullOrWhiteSpace(filters.End) ? Convert.ToDateTime(filters.End).SetMaxHour().ToString() : filters.End;

            try
            {
                sqlQuery = string.Format(sqlQuery,
                        filters.CompanyId,
                        filterColumntext,
                        filtertext,
                        filterByActiveStatus,
                        filterByEquipmentClass,
                        filterByEquipmentType,
                        filterByStockStatus,
                        filter_MultipleTechs,
                        orderquery,
                        filters.Start,
                        filters.End,
                        (filters.PageNo - 1) * filters.PageSize,
                        filters.PageSize,
                        filter_MultipleEquipments);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                //LogError(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "InventoryDataAccess-GetTechTransferListByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                _ErrorLogDataAccess.Insert(new Entities.ErrorLog() { ErrorId = new Guid(), ErrorFor = "DA|Inventory|GetWHHistoryTransfersListByFilters", Message = string.Format("{0} || {1}", ex.Message, sqlQuery), TimeUtc = DateTime.Now });
                return null;
            }
        }

        #endregion Digiture
    }
}
