using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Linq;
using System.Collections.Generic;

namespace HS.DataAccess
{
    public partial class EquipmentDataAccess
    {
        public EquipmentDataAccess(string ConStr) : base(ConStr) { }
        public EquipmentDataAccess() { }
        public bool ReseedEquipmentTable()
        {
            string SqlQuery = @"Delete from Equipment  
                                DBCC CHECKIDENT('Equipment', RESEED, 0)
                                ";
            try
            {
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
                {

                    ExecuteCommand(cmd);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public DataTable GetAllEquipmentServiceByCompany(Guid companyId)
        {
            string sqlQuery = @"select * from Equipment _Equipment
                                where _Equipment.CompanyId = '{0}'
                                and _Equipment.IsActive = 1";

            try
            {
                sqlQuery = string.Format(sqlQuery, companyId);
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

        public DataSet GetTicketListByFilter(TicketFilter TicketFilters)
        {
            string sqlQuery = @"";
            string searchQuery = "";
            string myTicketQuery = "";
            string ticketStatusQuery = "";
            string ticketTypeQuery = "";
            string assignedQuery = "";
            string CreatedByMeQuery = "";
            string subquery = "";
            string subquery1 = "";
            string CreatedDateQuery = "";
            string ReportTypeQuery = "";
            string ReportQuery = "";
            string ReportColQuery = "";
            string ReportCountQuery = "";
            if (!string.IsNullOrWhiteSpace(TicketFilters.SearchText))
            {
                searchQuery = @"and (CONVERT(nvarchar(11), tk.CompletionDate, 101) like @SearchText
								or tk.[Status] like @SearchText or tk.[TicketType] like @SearchText or tk.Id like @SearchText or cs.FirstName + ' ' + cs.LastName like @SearchText)";
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.ReportTabType) && TicketFilters.ReportTabType == "GoBack")
            {
                ReportTypeQuery = string.Format("and convert(date, tk.CreatedDate) between '{0}' and '{1}'", TicketFilters.StartDate, TicketFilters.EndDate);
                ReportQuery = string.Format("and countticket > 1 and AssignedToUserId = '<UserId>{0}</UserId>'", TicketFilters.UserId);
                ReportColQuery = string.Format("(select count(tik.Id) from ticket tik where convert(date, tik.CreatedDate) between '{0}' and '{1}' and tik.CustomerId=cs.CustomerId) as CountTicket", TicketFilters.StartDate, TicketFilters.EndDate);
                ReportCountQuery = string.Format("select  count(Id) as [TotalCount] from #TicketData where countticket > 1 and AssignedToUserId = '<UserId>{0}</UserId>'", TicketFilters.UserId);
            }
            else
            {
                ReportColQuery = string.Format("'' as CountTicket");
                ReportCountQuery = string.Format("select  count(Id) as [TotalCount] from #TicketData");
            }
            #region TicketStatus
            if (!string.IsNullOrWhiteSpace(TicketFilters.TicketStatus) && TicketFilters.TicketStatus != "undefined"
                && TicketFilters.TicketStatus != "-1" && TicketFilters.TicketStatus != "null")
            {
                ticketStatusQuery = string.Format("and tk.[Status] in ('{0}')", TicketFilters.TicketStatus);
            }
            #endregion

            #region Assigned
            if (!string.IsNullOrWhiteSpace(TicketFilters.AssignedUserTicket) && TicketFilters.AssignedUserTicket != "undefined" && TicketFilters.AssignedUserTicket != "-1" && TicketFilters.AssignedUserTicket != "null" && TicketFilters.AssignedUserTicket != new Guid().ToString())
            {
                assignedQuery = string.Format("and tuser.UserId in ('{0}')", TicketFilters.AssignedUserTicket);
            }
            #endregion


            #region TicketType
            if (!string.IsNullOrWhiteSpace(TicketFilters.TicketType) && TicketFilters.TicketType != "undefined"
                && TicketFilters.TicketType != "-1" && TicketFilters.TicketType != "null")
            {
                ticketTypeQuery = string.Format("and tk.TicketType in ('{0}')", TicketFilters.TicketType);
            }
            #endregion
            #region CreatedDateQuery
            if (TicketFilters.StartDate != new DateTime() && TicketFilters.EndDate != new DateTime())
            {
                var StartDate = TicketFilters.StartDate.SetZeroHour().UTCToClientTime();
                var EndDate = TicketFilters.EndDate.SetMaxHour().UTCToClientTime();
                CreatedDateQuery = string.Format("and tk.CreatedDate between '{0}' and '{1}'", StartDate, EndDate);
            }
            #endregion
            #region MyTicket
            if (!string.IsNullOrWhiteSpace(TicketFilters.MyTicket) && TicketFilters.MyTicket != "undefined")
            {
                if (TicketFilters.MyTicket == "Created")
                {
                    CreatedByMeQuery = string.Format("and tk.CreatedByUid = '{0}'", TicketFilters.UserId);

                }
                else if (TicketFilters.MyTicket == "Assigned")
                {
                    myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(tk.TicketId,'{0}') = 1 ", TicketFilters.UserId);
                }
                else if (TicketFilters.MyTicket == "Both")
                {
                    CreatedByMeQuery = string.Format("and tk.CreatedByUid = '{0}'", TicketFilters.UserId);
                    myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(tk.TicketId,'{0}') = 1 ", TicketFilters.UserId);
                }
                else if (TicketFilters.MyTicket == "None")
                {
                    CreatedByMeQuery = string.Format("and tk.CreatedByUid != '{0}'", TicketFilters.UserId);
                    myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(tk.TicketId,'{0}') = 0 ", TicketFilters.UserId);

                }
            }

            #endregion
            #region Order
            if (!string.IsNullOrWhiteSpace(TicketFilters.order))
            {
                if (TicketFilters.order == "ascending/ticketid")
                {
                    subquery = "order by #TicketData.[Id] asc";
                    subquery1 = "order by [Id] asc";
                }
                else if (TicketFilters.order == "descending/ticketid")
                {
                    subquery = "order by #TicketData.[Id] desc";
                    subquery1 = "order by [Id] desc";
                }
                if (TicketFilters.order == "ascending/customername")
                {
                    subquery = "order by #TicketData.CustomerName asc";
                    subquery1 = "order by CustomerName asc";
                }
                else if (TicketFilters.order == "descending/customername")
                {
                    subquery = "order by #TicketData.CustomerName desc";
                    subquery1 = "order by CustomerName desc";
                }
                else if (TicketFilters.order == "ascending/tickettype")
                {
                    subquery = "order by #TicketData.[TicketType] asc";
                    subquery1 = "order by [TicketType] asc";
                }
                else if (TicketFilters.order == "descending/tickettype")
                {
                    subquery = "order by #TicketData.[TicketType] desc";
                    subquery1 = "order by [TicketType] desc";
                }
                else if (TicketFilters.order == "ascending/assignto")
                {
                    subquery = "order by #TicketData.[AssignedTo] asc";
                    subquery1 = "order by [AssignedTo] asc";
                }
                else if (TicketFilters.order == "descending/assignto")
                {
                    subquery = "order by #TicketData.[AssignedTo] desc";
                    subquery1 = "order by [AssignedTo] desc";
                }
                else if (TicketFilters.order == "ascending/scheduledon")
                {
                    subquery = "order by #TicketData.[CompletionDate] asc";
                    subquery1 = "order by [CompletionDate] asc";
                }
                else if (TicketFilters.order == "descending/scheduledon")
                {
                    subquery = "order by #TicketData.[CompletionDate] desc";
                    subquery1 = "order by [CompletionDate] desc";
                }
                else if (TicketFilters.order == "ascending/status")
                {
                    subquery = "order by #TicketData.[Status]  asc";
                    subquery1 = "order by Status asc";
                }
                else if (TicketFilters.order == "descending/status")
                {
                    subquery = "order by #TicketData.[Status]  desc";
                    subquery1 = "order by Status desc";
                }

            }
            else
            {
                subquery = "order by #TicketData.[Id] desc";
                subquery1 = "order by Id desc";
            }
            #endregion
            sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                DECLARE @CustomerId uniqueidentifier
                                DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int
                                DECLARE @SearchText nvarchar(50) 

                                SET @SearchText = '%{0}%' 
                                SET @pageno = {1} --default 1
                                SET @pagesize = {2} --default 10
                                SET @CompanyId = '{3}' --97BCF758-A482-47EB-82B8-F88BF12293FF
                                SET @CustomerId = '{4}'

                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

 
                                select * into #TicketData 
                                    from (--TicketTypeVal
		                                select tk.*
                                        ,cs.FirstName +' '+cs.LastName as CustomerName
                                        ,(select count(id) from TicketFile where TicketId = tk.TicketId) as AttachmentsCount
                                        ,(select UserId  from Employee  where UserId in (select UserId from TicketUser tulist where tulist.TiketId = tk.TicketId and IsPrimary = 1) FOR XML PATH ('') ) as AssignedToUserId
                                        ,(select count(id) from TicketFile where TicketId = tk.TicketId)
		                                        + (select count(id) from TicketReply where TicketId = tk.TicketId) as RepliesCount
                                        ,lktype.DisplayText as TicketTypeVal
                                        ,lkstatus.DisplayText as StatusVal
                                        ,lkpriority.DisplayText as PriorityVal
                                        ,emp.FirstName + ' '+emp.LastName as CreatedByVal
                                        ,(select CAST(firstname + ' '+LastName + ', ' AS VARCHAR(200))  from Employee  where UserId in (select UserId from TicketUser tulist where tulist.TiketId = tk.TicketId and IsPrimary = 1) FOR XML PATH ('') ) as AssignedTo
                                        ,(select CAST(firstname + ' '+LastName + ', ' AS VARCHAR(200))  from Employee  where UserId in (select UserId from TicketUser tualist where tualist.TiketId = tk.TicketId and IsPrimary = 0) FOR XML PATH ('') ) as AdditionalMembers
	                                    ,lkStartTime.DisplayText as AppointmentStartTimeVal
                                        ,CA.AppointmentStartTime as AppointmentStartTime
                                        ,lkEndTime.DisplayText as AppointmentEndTimeVal
                                        ,CA.AppointmentEndTime as AppointmentEndTime
                                        ,(select COUNT(cae.Id)
                                        
										from CustomerAppointmentEquipment cae
										LEFT JOIN Ticket t on t.TicketId=cae.AppointmentId
										LEFT JOIN TicketUser tu on tu.TiketId=t.TicketId and tu.IsPrimary=1
										where cae.AppointmentId=CA.AppointmentId
                                        AND cae.IsEquipmentRelease=0
										AND cae.Quantity>(ISNULL((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=CAE.EquipmentId and Type='Add'  And invinner.TechnicianId=tu.UserId)-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=CAE.EquipmentId and Type='Release'  And invinner.TechnicianId=tu.UserId),0))) as ExceedQuantity,
                                        cs.Id as CusIdInt,
                                        isnull(cs.BusinessName, '') as CusBusinessName,
                                        isnull(sales.FirstName + ' ' + sales.LastName, '') as CusSalesPerson,
                                        isnull(installer.FirstName + ' ' + installer.LastName, '') as CusInstaller,
                                        iif(cs.MonthlyMonitoringFee != '' and cs.MonthlyMonitoringFee is not null, cs.MonthlyMonitoringFee, '0.00') as RMRAmount,
                                        {16}
                                        ,LAG(lktype.DisplayText) OVER (ORDER BY tk.Id) as PrevTicketType
										,LAG(tk.CompletionDate) OVER (ORDER BY tk.Id) as PrevAppointmentDate
										,LAG(emp.FirstName + ' '+emp.LastName) OVER (ORDER BY tk.Id) as PrevTechnician,
                                        lksalesloc.DisplayText as CusSalesLoc   
                                        from Ticket tk
                                        LEFT JOIN Customer cs on cs.CustomerId=tk.CustomerId
                                        left join TicketUser tuser on tuser.TiketId = tk.TicketId and tuser.IsPrimary = 1
                                        left join Lookup lktype on  lktype.DataKey ='TicketType'  
                                        and lktype.DataValue = tk.TicketType

                                        left join CustomerAppointment CA on  CA.AppointmentId = tk.TicketId

                                        left join Lookup lkStartTime on lkStartTime.DataKey = 'Arrival'
                                        and lkStartTime.DataValue = CA.AppointmentStartTime

                                        left join Lookup lkEndTime on lkEndTime.DataKey = 'Arrival'
                                        and lkEndTime.DataValue = CA.AppointmentEndTime

                                        left join Lookup lkstatus on  lkstatus.DataKey ='TicketStatus'  
                                        and lkstatus.DataValue = tk.[Status]

                                        left join Lookup lkpriority on  lkpriority.DataKey ='TicketPriority'  
                                        and lkpriority.DataValue = tk.[Priority]

                                        left join Lookup lksalesloc on  lksalesloc.DataKey ='CommissionType'  
                                        and lksalesloc.DataValue = iif(cs.SalesLocation != '-1', cs.SalesLocation, null)

                                        left join Employee emp on tk.CreatedBy = emp.UserId
                                        left join Employee sales on CONVERT(nvarchar(50), sales.UserId) = cs.Soldby
                                        left join Employee installer on CONVERT(nvarchar(50), installer.UserId) = cs.Soldby
                                        
		                                where tk.CompanyId = @CompanyId
                                        and iif(cs.FirstName +' '+cs.LastName is null, cs.BusinessName, cs.DBA) is not null
                                        {5}
                                        {6}
                                        {7}
                                        {8}
                                        {9}
                                        {10}
                                        {13}
                                        {14}
                                        
	                                ) a 

                                SELECT TOP (@pagesize) * FROM #TicketData
                                    where   Id NOT IN(Select TOP (@pagestart) Id from #TicketData {11}) 
                                    {15}
                                     {12}
	                                --and (InvoiceIdStr like @SearchText or FirstName + ' '+ LastName like @SearchText)
	                           

	                            {17}
                                DROP TABLE #TicketData
                                    ";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        TicketFilters.SearchText,//0
                                        TicketFilters.PageNo,  //1
                                        TicketFilters.PageSize, //2
                                        TicketFilters.CompanyId, //3
                                        TicketFilters.CustomerId, //4
                                        searchQuery,//5
                                        ticketStatusQuery,//6
                                        ticketTypeQuery,//7
                                        assignedQuery,//8
                                        CreatedByMeQuery,//9
                                        myTicketQuery,//10
                                        subquery,//11
                                        subquery1,//12
                                        CreatedDateQuery,//13,
                                        ReportTypeQuery,//14,
                                        ReportQuery,//15,
                                        ReportColQuery,//16
                                        ReportCountQuery//17
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

        public DataTable GetEqupmentListBySearchKeySupplierIdAndCategory(string key, int MaxLoad, Guid? SupplierId, int? CategoryId)
        {
            string sqlQuery = @"
                                select 
                                TOP {0}
                                eq.Id as Id,
                                eq.EquipmentId,
                                eq.[Name] as EquipmentName,
                                eq.Retail as RetailPrice,
                                {3}
                                et.[Name] as EquipmentType,
                                eq.Note as EquipmentDescription,
                                eq.Point as Point,
                                ISNULL((select top(1) SKU from EquipmentManufacturer where EquipmentId = eq.EquipmentId and IsPrimary = 1),eq.SKU) as SKU,
								ISNULL((select top(1) ManufacturerId from EquipmentManufacturer where EquipmentId = eq.EquipmentId and IsPrimary = 1),'00000000-0000-0000-0000-000000000000') as ManufacturerId,
                                eq.EquipmentTypeId as EquipmentTypeId,
                                {4}
                                eq.Unit,
                                eq.OverheadRate,
                                eq.ProfitRate

                                from Equipment eq
                                left join EquipmentType et
                                on eq.EquipmentTypeId = et.Id
                                where (eq.Name like @SerchText OR eq.SKU like @SerchText) 
                                and eq.IsActive = 1
                                and eq.EquipmentClassId != 2
                                {1}
                                {2}
                                ";

            #region Supplier Filter
            string SupplierFilter = "";
            string SupplierCostSubQuery = "(select top(1) Cost from EquipmentVendor where EquipmentId = eq.EquipmentId and IsPrimary = 1) as SupplierCost,";
            string SupplierIdSubQuery = "ISNULL((select top(1) SupplierId from EquipmentVendor where EquipmentId = eq.EquipmentId and IsPrimary = 1),'00000000-0000-0000-0000-000000000000') as SupplierId,";

            if (SupplierId != null && SupplierId != Guid.Empty)
            {
                SupplierFilter = "AND EquipmentId in (select equipmentid from EquipmentVendor where SupplierId = '{0}')";
                SupplierFilter = string.Format(SupplierFilter, SupplierId);
                SupplierCostSubQuery = "(select top(1) Cost from EquipmentVendor where EquipmentId = eq.EquipmentId and SupplierId = '{0}' ) as SupplierCost,";
                SupplierCostSubQuery = string.Format(SupplierCostSubQuery, SupplierId);
                SupplierIdSubQuery = string.Format(" CONVERT(uniqueidentifier,'{0}') as SupplierId,", SupplierId);
            }
            #endregion

            #region Category Filter
            string CategoryFilter = "";
            if (CategoryId != null && CategoryId > -1)
            {
                CategoryFilter = string.Format("AND eq.EquipmentTypeId = {0}", CategoryId);
            }
            #endregion

            try
            {
                sqlQuery = string.Format(sqlQuery, MaxLoad, SupplierFilter, CategoryFilter, SupplierCostSubQuery, SupplierIdSubQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    cmd.Parameters.Add(new SqlParameter("SerchText", "%" + key + "%"));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<EquipmentOptions> GetEquipmentOptionsByKeyAndType(string key, string type)
        {
            string sqlQuery = @"select * into #InvoiceOptions from (select [{0}] as OptionName, '00000000-0000-0000-0000-000000000000' as EquipmentId from Equipment
                                where [{0}] != ''
                                Union
                                select [Name] as OptionName, '00000000-0000-0000-0000-000000000000' as EquipmentId from ServiceDetailInfo where [Type] = '{0}'
                                and [Name] != '')a 
                                select * from #InvoiceOptions where OptionName like '%{1}%' 
                                drop table #InvoiceOptions
                                ";

            try
            {
                sqlQuery = string.Format(sqlQuery, type, key);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);

                    List<EquipmentOptions> EquipmentServiceList = new List<EquipmentOptions>();
                    if (dsResult.Tables[0] != null)
                        EquipmentServiceList = (from DataRow dr in dsResult.Tables[0].Rows
                                                select new EquipmentOptions()
                                                {
                                                    EquipmentId = Guid.Empty,//(Guid)dr["EquipmentId"],
                                                    OptionName = dr["OptionName"].ToString()
                                                }).ToList();

                    return EquipmentServiceList;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public DataTable GetAllEquipmentVendorByEquipmentId(Guid EquipmentId)
        {
            string sqlQuery = @"select ev.Id, 
                                ev.Cost
                                , IIF(sp.[Name] is null or sp.[Name] = '',sp.CompanyName,sp.[Name]) as [Name] 
                                , ev.IsPrimary
                                ,ev.SKU
                                from EquipmentVendor ev

                                left join Supplier sp on sp.SupplierId = ev.SupplierId 
                                where  ev.EquipmentId = '{0}'
                                order by IsPrimary desc
                                ";

            try
            {
                sqlQuery = string.Format(sqlQuery, EquipmentId);
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
        public DataTable GetAllEquipmentManufacturerByEquipmentId(Guid EquipmentId)
        {
            string sqlQuery = @"select ev.Id, 
                                ev.Cost
                                , mn.[Name] as [Name] 
                                , ev.IsPrimary
                                ,ev.SKU
                                ,ev.Variation

                                from EquipmentManufacturer ev

                                left join Manufacturer mn on mn.ManufacturerId = ev.ManufacturerId 
                                where  ev.EquipmentId = '{0}'
                                order by IsPrimary desc
                                ";

            try
            {
                sqlQuery = string.Format(sqlQuery, EquipmentId);
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
        public DataTable GetEquipmentListByCustomerIdAndCompanyId(int leadId, Guid companyId)
        {
            string sqlQuery = @"declare @AppoinmentId uniqueidentifier
                                declare @CompanyId uniqueidentifier
                                set @CompanyId = '{1}'
                                set @AppoinmentId = (
                                select AppointmentId from CustomerAppointment 
	                                where CustomerId = (select CustomerId from Customer where id = {0}) 
	                                and CompanyId = @CompanyId)

                                select eq.*
								, cae.UnitPrice as UnitPriceAppointmentEquipment 
								, cae.TotalPrice 
								, cae.Quantity as QuantityAppointmentEquipment
								from Equipment eq
								left join CustomerAppointmentEquipment cae 
								on cae.EquipmentId = eq.EquipmentId
								where cae.AppointmentId = @AppoinmentId
								and eq.CompanyId = @CompanyId 
                                order by cae.Id
                                ";

            try
            {
                sqlQuery = string.Format(sqlQuery, leadId, companyId);
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
        public DataSet GetEquipmentListByCompanyIdTechnicianId(MassRestockFilter massFilter)
        {
            //    string sqlQuery = @"select
            //                        eqp.EquipmentId,
            //                        etrp.TechnicianId,
            //                        eqp.Name,
            //                        mf.Name as ManufacturerName,
            //                        eqp.SKU,
            //                        ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=eqp.EquipmentId and Type='Add')
            //                        - (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=eqp.EquipmentId and Type='Release')
            //                        - isnull((select sum(Quantity) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = eqp.EquipmentId and b.TechnicianId  = '22222222-2222-2222-2222-222222222222' 
            //                        and b.IsApprove = 0 and b.IsDecline = 0),0)) as Quantity,
            //                        ISNULL(etrp.ReorderPoint,0) ReorderPoint,
            //                        ISNULL(eqp.WarehouseReorder,0) WHReorderPoint,
            //                        (Select (Select ISNULL(SUM(it.Quantity),0) from InventoryTech it where it.TechnicianId='{0}' and it.EquipmentId=eqp.EquipmentId and it.Type='Add')
            //                        - (Select ISNULL(SUM(it.Quantity),0) from InventoryTech it where it.TechnicianId='{0}' and it.EquipmentId=eqp.EquipmentId and it.Type='Release')
            //                        - (select isnull(sum(Quantity),0) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  =eqp.EquipmentId and b.TechnicianId  = '{0}' 
            //                            and b.IsApprove = 0 and b.IsDecline = 0 )) Have
            //   into #TestTable
            //                        from Equipment eqp
            //                        left join Manufacturer mf on mf.Id=eqp.ManufacturerId
            //Left join EquipmentTechnicianReorderPoint etrp on etrp.EquipmentId=eqp.EquipmentId AND etrp.TechnicianId='{0}'
            //where eqp.EquipmentClassId=1 AND eqp.IsActive=1 {2} 

            //                        select * from #TestTable {1} 


            //                        select sum(Quantity) as TotalQty
            //                        , sum(ReorderPoint) as TotalPoint
            //                            ,sum(WHReorderPoint) as TotalWHPoint
            //                        , sum(Have) as TotalHave


            //                        from #TestTable

            //drop table #TestTable";
            string sqlQuery = @"select
                                eqp.EquipmentId,
                                etrp.TechnicianId,
                                eqp.Name,
                                mf.Name as ManufacturerName,
                                eqp.SKU,
                                 ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=eqp.EquipmentId and Type='Add' AND invinner.LocationId = '22222222-2222-2222-2222-222222222222')
                                - (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=eqp.EquipmentId and Type='Release' AND invinner2.LocationId = '22222222-2222-2222-2222-222222222222')
                                -(select ISNull(Sum(b.Quantity),0) from AssignedInventoryTechReceived b where b.EquipmentId = eqp.EquipmentId AND b.TechnicianId =
	                                       '22222222-2222-2222-2222-222222222222' And b.IsApprove = 0 and b.IsDecline = 0)
                                ) as Quantity,
                                ISNULL(etrp.ReorderPoint,0) ReorderPoint,
                                ISNULL(eqp.WarehouseReorder,0) WHReorderPoint,
                                (Select (Select ISNULL(SUM(it.Quantity),0) from InventoryTech it where it.TechnicianId='{0}' and it.EquipmentId=eqp.EquipmentId and it.Type='Add')
                                - (Select ISNULL(SUM(it.Quantity),0) from InventoryTech it where it.TechnicianId='{0}' and it.EquipmentId=eqp.EquipmentId and it.Type='Release')
                                - (select isnull(sum(Quantity),0) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  =eqp.EquipmentId and b.TechnicianId  = '{0}' 
                                    and b.IsApprove = 0 and b.IsDecline = 0 )) Have
							    into #TestTable
                                from Equipment eqp
                                left join Manufacturer mf on mf.Id=eqp.ManufacturerId
								Left join EquipmentTechnicianReorderPoint etrp on etrp.EquipmentId=eqp.EquipmentId AND etrp.TechnicianId='{0}'
								where eqp.EquipmentClassId=1 AND eqp.IsActive=1 {2} 

                                select * from #TestTable {1} 


                                select sum(Quantity) as TotalQty
                                , sum(ReorderPoint) as TotalPoint
                                    ,sum(WHReorderPoint) as TotalWHPoint
                                , sum(Have) as TotalHave


                                from #TestTable

								drop table #TestTable";

            var ShowAllQuery = "";
            if (!massFilter.ShowAll)
            {
                ShowAllQuery = string.Format(" And etrp.TechnicianId='{0}'", massFilter.TechnicianId);
            }
            string OrderBy = "";
            if (!string.IsNullOrEmpty(massFilter.Order))
            {
                if (massFilter.Order == "ascending/des")
                {
                    OrderBy = "order by Name asc";
                }
                else if (massFilter.Order == "descending/des")
                {
                    OrderBy = "order by Name desc";
                }
                else if (massFilter.Order == "ascending/man")
                {
                    OrderBy = "order by ManufacturerName asc";
                }
                else if (massFilter.Order == "descending/man")
                {
                    OrderBy = "order by ManufacturerName desc";
                }
                else if (massFilter.Order == "ascending/sku")
                {
                    OrderBy = "order by SKU asc";
                }
                else if (massFilter.Order == "descending/sku")
                {
                    OrderBy = "order by SKU desc";
                }
                else if (massFilter.Order == "ascending/sku")
                {
                    OrderBy = "order by SKU asc";
                }
                else if (massFilter.Order == "descending/sku")
                {
                    OrderBy = "order by SKU desc";
                }
                else if (massFilter.Order == "ascending/qty")
                {
                    OrderBy = "order by Quantity asc";
                }
                else if (massFilter.Order == "descending/qty")
                {
                    OrderBy = "order by Quantity desc";
                }
                else if (massFilter.Order == "ascending/reorder")
                {
                    OrderBy = "order by ReorderPoint asc";
                }
                else if (massFilter.Order == "ascending/whreorder")
                {
                    OrderBy = "order by WarehouseReorder asc";
                }
                else if (massFilter.Order == "descending/reorder")
                {
                    OrderBy = "order by ReorderPoint desc";
                }
                else if (massFilter.Order == "descending/whreorder")
                {
                    OrderBy = "order by WarehouseReorder desc";
                }
                else if (massFilter.Order == "ascending/have")
                {
                    OrderBy = "order by Have asc";
                }
                else if (massFilter.Order == "descending/have")
                {
                    OrderBy = "order by Have desc";
                }
            }
            else
            {
                OrderBy = " order by Name";
            }

            try
            {
                sqlQuery = string.Format(sqlQuery, massFilter.TechnicianId, OrderBy, ShowAllQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable GetEquipmentListByCompanyIdTechnicianIdForReport(Guid TechnicianId, bool IsShowAll)
        {
            var ShowAllQuery = "";
            if (!IsShowAll)
            {
                ShowAllQuery = " And etrp.ReorderPoint>0";
            }
            string sqlQuery = @"select
                                emp.FirstName + ' ' + emp.LastName as TechnicianName,
                                eqp.Name as Description,
                                mf.Name as Manufacturer,
                                eqp.SKU,
                                ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=eqp.EquipmentId and Type='Add')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=eqp.EquipmentId and Type='Release')) as OnHand,
                                ISNULL(etrp.ReorderPoint,0) ReorderPoint,
                                (Select ISNULL((Select ISNULL(SUM(it.Quantity),0) from InventoryTech it where it.TechnicianId=etrp.TechnicianId and it.EquipmentId=etrp.EquipmentId and it.Type='Add')-(Select ISNULL(SUM(it.Quantity),0) from InventoryTech it where it.TechnicianId=etrp.TechnicianId and it.EquipmentId=etrp.EquipmentId and it.Type='Release'),0)) Truck
							    from Equipment eqp
                                left join Manufacturer mf on mf.Id=eqp.ManufacturerId
								Left join EquipmentTechnicianReorderPoint etrp on etrp.EquipmentId=eqp.EquipmentId and etrp.TechnicianId='{0}'
								left join Employee emp on emp.UserId = etrp.TechnicianId
								where eqp.EquipmentClassId=1 AND eqp.IsActive=1 {1}";

            try
            {
                sqlQuery = string.Format(sqlQuery, TechnicianId, ShowAllQuery);
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
        public DataTable GetClusterEquipmentListByCompanyIdTechnicianIdForReport(string TechnicianId)
        {
            TechnicianId = TechnicianId.Substring(0, TechnicianId.Length - 1);

            string technicianIdquery = "";
            var array = TechnicianId.Split(",");
            string query = "";
            if (array != null)
            {
                foreach (var item in array)
                {
                    query += string.Format("'{0}',", item);
                }
                query = query.Remove(query.Length - 1, 1);
            }
            if (!string.IsNullOrWhiteSpace(query))
            {
                technicianIdquery += string.Format("and etrp.TechnicianId in ({0})", query);
            }
            string sqlQuery = @"select
                                emp.FirstName + ' ' + emp.LastName as TechnicianName,
                                eqp.Name as Description,
                                mf.Name as Manufacturer,
                                eqp.SKU,
                                ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=eqp.EquipmentId and Type='Add')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=eqp.EquipmentId and Type='Release')) as OnHand,
                                ISNULL(etrp.ReorderPoint,0) ReorderPoint,
                                (Select ISNULL((Select ISNULL(SUM(it.Quantity),0) from InventoryTech it where it.TechnicianId=etrp.TechnicianId and it.EquipmentId=etrp.EquipmentId and it.Type='Add')-(Select ISNULL(SUM(it.Quantity),0) from InventoryTech it where it.TechnicianId=etrp.TechnicianId and it.EquipmentId=etrp.EquipmentId and it.Type='Release'),0)) Truck
							    from Equipment eqp
                                left join Manufacturer mf on mf.Id=eqp.ManufacturerId
								Left join EquipmentTechnicianReorderPoint etrp on etrp.EquipmentId=eqp.EquipmentId
								left join Employee emp on emp.UserId = etrp.TechnicianId
								where emp.Id > 0 {1}";


            try
            {
                sqlQuery = string.Format(sqlQuery, TechnicianId, technicianIdquery);
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

        public DataTable GetEquipmentListDataTechnicianUpsold(MassRestockFilter massFilter)
        {
            string sqlQuery = "";
            if (massFilter.Searchtext == "service")
            {
                sqlQuery = @"select *,Equ.Name from CustomerAppointmentEquipment cae
                                left join Equipment Equ 
								on cae.EquipmentId = Equ.EquipmentId where cae.IsService=1 and cae.CreatedByUid = '{0}' {1}";

            }
            else
            {
                sqlQuery = @"select *,Equ.Name from CustomerAppointmentEquipment cae
                                left join Equipment Equ 
								on cae.EquipmentId = Equ.EquipmentId where cae.IsService = 0 and cae.CreatedByUid = '{0}' and cae.IsAgreementItem ='0' and Equ.IsUpsold='1' {1}";

            }

            var ShowAllQuery = "";
            if (!massFilter.ShowAll)
            {
                ShowAllQuery = "AND etrp.ReorderPoint >0";
            }
            string OrderBy = "";
            if (!string.IsNullOrEmpty(massFilter.Order))
            {
                if (massFilter.Order == "ascending/quantity")
                {
                    OrderBy = "order by cae.Quantity asc";
                }
                else if (massFilter.Order == "descending/quantity")
                {
                    OrderBy = "order by cae.Quantity desc";
                }
                else if (massFilter.Order == "ascending/unitprice")
                {
                    OrderBy = "order by cae.UnitPrice asc";
                }
                else if (massFilter.Order == "descending/unitprice")
                {
                    OrderBy = "order by cae.UnitPrice desc";
                }
                else if (massFilter.Order == "ascending/equipmentname")
                {
                    OrderBy = "order by cae.EquipName asc";
                }
                else if (massFilter.Order == "descending/equipmentname")
                {
                    OrderBy = "order by cae.EquipName desc";
                }
                else if (massFilter.Order == "ascending/totalprice")
                {
                    OrderBy = "order by cae.TotalPrice asc";
                }
                else if (massFilter.Order == "descending/totalprice")
                {
                    OrderBy = "order by cae.TotalPrice desc";
                }
            }


            try
            {
                sqlQuery = string.Format(sqlQuery, massFilter.TechnicianId, OrderBy, ShowAllQuery);
                //sqlQuery = string.Format(sqlQuery, userid);
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

        public DataTable GetUpsoldListDataTechnicianUpsold(MassRestockFilter massFilter)
        {
            string sqlQuery = "";
            sqlQuery = @"select *,Equ.Name from CustomerAppointmentEquipment cae
                                left join Equipment Equ 
								on cae.EquipmentId = Equ.EquipmentId where (cae.IsService=1 and cae.CreatedByUid = '{0}' {2}) or
                                (cae.IsService = 0 and cae.CreatedByUid = '{0}' and cae.IsAgreementItem ='0' and Equ.IsUpsold='1' {2}) 
                                  {1} ";

            var DateQuery = "";
            if (!string.IsNullOrWhiteSpace(massFilter.Searchtext) &&
                (massFilter.Searchtext == "Daily"
                || massFilter.Searchtext == "Weekly"
                || massFilter.Searchtext == "Monthly"
                || massFilter.Searchtext == "Yearly")
                )
            {
                DateQuery = "and cae.CreatedDate >= '{0}' and cae.CreatedDate <= '{1}'";
                DateQuery = string.Format(DateQuery, massFilter.FDate, massFilter.LDate);

            }
            string OrderBy = "";
            if (!string.IsNullOrEmpty(massFilter.Order))
            {
                if (massFilter.Order == "ascending/quantity")
                {
                    OrderBy = "order by cae.Quantity asc";
                }
                else if (massFilter.Order == "descending/quantity")
                {
                    OrderBy = "order by cae.Quantity desc";
                }
                else if (massFilter.Order == "ascending/unitprice")
                {
                    OrderBy = "order by cae.UnitPrice asc";
                }
                else if (massFilter.Order == "descending/unitprice")
                {
                    OrderBy = "order by cae.UnitPrice desc";
                }
                else if (massFilter.Order == "ascending/equipmentname")
                {
                    OrderBy = "order by cae.EquipName asc";
                }
                else if (massFilter.Order == "descending/equipmentname")
                {
                    OrderBy = "order by cae.EquipName desc";
                }
                else if (massFilter.Order == "ascending/totalprice")
                {
                    OrderBy = "order by cae.TotalPrice asc";
                }
                else if (massFilter.Order == "descending/totalprice")
                {
                    OrderBy = "order by cae.TotalPrice desc";
                }
            }


            try
            {
                sqlQuery = string.Format(sqlQuery, massFilter.TechnicianId, OrderBy, DateQuery);
                //sqlQuery = string.Format(sqlQuery, userid);
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



        public DataTable GetEquipmentListByCompanyIdTechnicianIdReorderPoint(MassRestockFilter massFilter)
        {
            string sqlQuery = @"select
                                eqp.EquipmentId,
                                etrp.TechnicianId,
                                eqp.Name,
                                mf.Name as ManufacturerName,
                                eqp.SKU,
                                ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=eqp.EquipmentId and Type='Add')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=eqp.EquipmentId and Type='Release')) as Quantity,
                                ISNULL(etrp.ReorderPoint,0) ReorderPoint,
                                (Select ISNULL((Select ISNULL(SUM(it.Quantity),0) from InventoryTech it where it.TechnicianId=etrp.TechnicianId and it.EquipmentId=etrp.EquipmentId and it.Type='Add')-(Select ISNULL(SUM(it.Quantity),0) from InventoryTech it where it.TechnicianId=etrp.TechnicianId and it.EquipmentId=etrp.EquipmentId and it.Type='Release'),0)) Have
							    from Equipment eqp
                                left join Manufacturer mf on mf.Id=eqp.ManufacturerId
								Left join EquipmentTechnicianReorderPoint etrp on etrp.EquipmentId=eqp.EquipmentId and etrp.TechnicianId='{1}'
								where eqp.EquipmentClassId=1 AND eqp.IsActive=1 AND eqp.CompanyId='{0}' {3} {2}";

            var ShowAllQuery = "";
            if (!massFilter.ShowAll)
            {
                ShowAllQuery = "AND ISNULL(etrp.ReorderPoint,0)>0";
            }
            string OrderBy = "";
            if (!string.IsNullOrEmpty(massFilter.Order))
            {
                if (massFilter.Order == "ascending/des")
                {
                    OrderBy = "order by eqp.Name asc";
                }
                else if (massFilter.Order == "descending/des")
                {
                    OrderBy = "order by eqp.Name desc";
                }
                else if (massFilter.Order == "ascending/man")
                {
                    OrderBy = "order by ManufacturerName asc";
                }
                else if (massFilter.Order == "descending/des")
                {
                    OrderBy = "order by ManufacturerName desc";
                }
                else if (massFilter.Order == "ascending/sku")
                {
                    OrderBy = "order by SKU asc";
                }
                else if (massFilter.Order == "descending/sku")
                {
                    OrderBy = "order by SKU desc";
                }
                else if (massFilter.Order == "ascending/sku")
                {
                    OrderBy = "order by SKU asc";
                }
                else if (massFilter.Order == "descending/sku")
                {
                    OrderBy = "order by SKU desc";
                }
                else if (massFilter.Order == "ascending/qty")
                {
                    OrderBy = "order by Quantity asc";
                }
                else if (massFilter.Order == "descending/qty")
                {
                    OrderBy = "order by Quantity desc";
                }
                else if (massFilter.Order == "ascending/reorder")
                {
                    OrderBy = "order by ReorderPoint asc";
                }
                else if (massFilter.Order == "descending/reorder")
                {
                    OrderBy = "order by ReorderPoint desc";
                }
                else if (massFilter.Order == "ascending/have")
                {
                    OrderBy = "order by Have asc";
                }
                else if (massFilter.Order == "descending/have")
                {
                    OrderBy = "order by Have desc";
                }
            }

            try
            {
                sqlQuery = string.Format(sqlQuery, massFilter.CompanyId, massFilter.TechnicianId, OrderBy, ShowAllQuery);
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
        public DataTable GetMassPOEquipmentListByCompanyId(Guid companyId)
        {
            string sqlQuery = @"select
                                pob.DemandOrderId,
								pob.TechDemandOrderId,
                                eqp.EquipmentId,
                                eqp.Name,
                                mf.Name as ManufacturerName,
                                eqp.SKU,
                                ev.Cost as Price,
                                sp.CompanyName as PrimaryVendor,
                                sp.SupplierId,
                                pod.Quantity
                                from PurchaseOrderBranch pob
                                LEFT JOIN PurchaseOrderDetail pod on pod.PurchaseOrderId=pob.DemandOrderId
                                Left join Equipment eqp on eqp.EquipmentId=pod.EquipmentId
                                left join Manufacturer mf on mf.Id=eqp.ManufacturerId
                                left join EquipmentVendor ev on ev.EquipmentId=eqp.EquipmentId and ev.IsPrimary=1
                                left join Supplier sp on sp.SupplierId=ev.SupplierId
                                where eqp.EquipmentClassId=1 AND eqp.IsActive=1 AND pob.IsBulkPo=1 AND pod.BulkStatus=0 AND eqp.CompanyId='{0}' ";

            try
            {
                sqlQuery = string.Format(sqlQuery, companyId);
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
        public DataTable GetSmartEquipmentEstimatorListByCustomerIdAndCompanyId(Guid CustomerId, Guid companyId, bool firstpage, Guid ticketid,string EstimatorId)
        {
            string subquery = "";
            //if (ticketid != new Guid())
            //{
            //    subquery = string.Format("and cpe.AppointmentIntId = (select Id from Ticket where TicketId = '{0}')", ticketid);
            //}
            string sqlQuery = @"";
            sqlQuery = @"Select est.*,est.CustomerId
								,estdtl.PartDescription As Name
								,estdtl.IsTaxable
                                ,estdtl.Qunatity As Quantity
                                ,estdtl.UnitCost AS UnitPrice 
                                ,ISNULL((estdtl.UnitCost * estdtl.Qunatity), 0) as TotalUnitPrice
                                ,estdtl.TotalCost As TotalCost
                                ,estdtl.TotalPrice As TotalPriceValue
                                ,estdtl.Overhead
                                ,estdtl.OverheadRate 
                                ,estdtl.EquipmentId 
                                ,est.ActivationFee
                                ,eq.SKU
                                from Estimator est
                                Left join EstimatorDetail estdtl on est.EstimatorId = estdtl.EstimatorId
                                Left join Equipment eq on estdtl.EquipmentId = eq.EquipmentId
                                where  est.CompanyId='{0}' AND est.CustomerId='{1}'
                                and est.EstimatorId = '{2}'
                                 ";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, CustomerId,EstimatorId);
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
        public DataTable GetSmartEquipmentListByCustomerIdAndCompanyId(Guid CustomerId, Guid companyId, bool firstpage, Guid ticketid)
        {
            string subquery = "";
            if (ticketid != new Guid())
            {
                subquery = string.Format("and cpe.AppointmentIntId = (select Id from Ticket where TicketId = '{0}')", ticketid);
            }
            string sqlQuery = @"";
            sqlQuery = @"select 
                                eq.*
                                ,cpe.CustomerId
                                ,cpe.Quantity
                                ,cpe.UnitPrice
                                ,cpe.DiscountUnitPricce
                                ,ISNULL((cpe.DiscountUnitPricce * cpe.Quantity), 0) as TotalDiscountUnitPrice
                                ,cpe.Total
                                ,cpe.IsTransfered,
                                cpe.IsEqpExist,
                    cpe.DiscountPercent,
                    cpe.DiscountInAmount
                                from CustomerPackageEqp cpe
                                LEFT JOIN Equipment eq on eq.EquipmentId=cpe.EquipmentId
                                where cpe.CompanyId='{0}' AND cpe.CustomerId='{1}'
                                and eq.EquipmentId is not null
                                {2}  ORDER BY 
                                (SELECT MAX(CreatedDate) 
                                FROM CustomerAppointmentEquipment 
                                WHERE Id = cpe.AppointmentEquipmentIntId) ASC";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, CustomerId, subquery);
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

        public DataTable GetSmartEstimatorServiceListByCustomerIdAndCompanyId(Guid CustomerId, Guid companyId, bool firstpage, Guid ticketid,string EstimatorId)
        {
            //string subquery = "";
            //if (ticketid != new Guid())
            //{
            //    subquery = string.Format("and cpe.AppointmentIntId = (select Id from Ticket where TicketId = '{0}')", ticketid);
            //}
            string sqlQuery = @"";
            sqlQuery = @"Select est.*,est.CustomerId
								,estservice.EquipmentName As Name
								,estservice.IsTaxable
                                ,estservice.Quantity As Quantity
                                ,estservice.UnitPrice AS UnitPrice 
                                ,ISNULL((estservice.UnitPrice * estservice.Quantity), 0) as TotalAmount
                                ,estservice.Amount As Total  
                                ,estservice.EquipmentId 
                                ,equ.IsARBEnabled
                                ,ISNULL(est.TaxAmount + est.ServiceTaxAmount,0) As TotalestimatorTaxAmount
								 from Estimator est
								Left join EstimatorService estservice on est.EstimatorId = estservice.EstimatorId
                                Left join Equipment equ on equ.EquipmentId = estservice.EquipmentId
                                where  est.CompanyId='{0}' AND est.CustomerId='{1}'
                                and est.EstimatorId = '{2}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, CustomerId, EstimatorId);
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
        public DataTable GetSmartServiceListByCustomerIdAndCompanyId(Guid CustomerId, Guid companyId, bool firstpage, Guid ticketid)
        {
            string subquery = "";
            if (ticketid != new Guid())
            {
                subquery = string.Format("and cpe.AppointmentIntId = (select Id from Ticket where TicketId = '{0}')", ticketid);
            }
            string sqlQuery = @"";
            sqlQuery = @"select distinct
                                eq.*
                                ,cpe.CustomerId
                                ,cpe.MonthlyRate
                                ,cpe.DiscountRate
                                ,cpe.Total
                                from CustomerPackageService cpe
                                LEFT JOIN Equipment eq on eq.EquipmentId=cpe.EquipmentId
                                where cpe.CompanyId='{0}' AND cpe.CustomerId='{1}'
                                and eq.EquipmentId is not null
                                and eq.IsARBEnabled = 1
                                {2}";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, CustomerId, subquery);
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
        public DataTable GetEstimatorSmartServiceListByCustomerIdAndCompanyId(Guid CustomerId, Guid companyId, bool firstpage, Guid ticketid,int EstimatorId)
        {
            string subquery = "";
            if (ticketid != new Guid())
            {
                subquery = string.Format("and cpe.AppointmentIntId = (select Id from Ticket where TicketId = '{0}')", ticketid);
            }
            string sqlQuery = @"";
            sqlQuery = @"select distinct
                                Est.*,(Est.ServiceTotalAmount + Est.ServiceTaxAmount) As TotalEstimateServiceAmount
                                 
                                from Estimator Est
                                where Est.CompanyId='{0}'
								AND Est.CustomerId='{1}'
                                and Est.Id = {2} 
                                 ";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, CustomerId, EstimatorId);
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
        public DataTable GetSmartServiceListByCustomerIdAndCompanyId_v2(Guid CustomerId, Guid companyId, bool firstpage, Guid ticketid)
        {
            string subquery = "";
            if (ticketid != new Guid())
            {
                subquery = string.Format("and cpe.AppointmentIntId = (select Id from Ticket where TicketId = '{0}')", ticketid);
            }
            string sqlQuery = @"";
            sqlQuery = @"select distinct
                                eq.*
                                ,cpe.CustomerId
                                ,cpe.MonthlyRate
                                ,cpe.DiscountRate
                                ,cpe.Total
                                from CustomerPackageService cpe
                                LEFT JOIN Equipment eq on eq.EquipmentId=cpe.EquipmentId
                                where cpe.CompanyId='{0}' AND cpe.CustomerId='{1}'
                                and eq.EquipmentId is not null
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, CustomerId, subquery);
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
        public DataTable GetNotARBEnabledSmartServiceListFromService(Guid CustomerId, Guid companyId)
        {
            string sqlQuery = @"";
            sqlQuery = @"select distinct
                                eq.*
                                ,cpe.CustomerId
                                ,cpe.MonthlyRate
                                ,cpe.DiscountRate
                                ,cpe.Total
                                from CustomerPackageService cpe
                                LEFT JOIN Equipment eq on eq.EquipmentId=cpe.EquipmentId
                                where cpe.CompanyId='{0}' AND cpe.CustomerId='{1}'
                                and eq.EquipmentId is not null
                                and eq.IsARBEnabled = 0";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, CustomerId);
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
        public DataTable GetNotARBEnabledSmartServiceListByCustomerIdAndCompanyId(Guid CustomerId, Guid companyId, bool firstpage, Guid ticketid)
        {
            string subquery = "";
            if (ticketid != new Guid())
            {
                subquery = string.Format("and cae.AppointmentId = '{0}'", ticketid);
            }
            string sqlQuery = @"select distinct
                                eq.*
                                ,ticket.CustomerId
                                ,cae.UnitPrice as MonthlyRate
                                ,isnull(cps.DiscountRate, 0) as DiscountRate
                                ,cae.TotalPrice as [Total]
                                from CustomerAppointmentEquipment cae
                                left join Ticket ticket on ticket.TicketId = cae.AppointmentId
                                LEFT JOIN Equipment eq on eq.EquipmentId = cae.EquipmentId
                                left join CustomerPackageService cps on cps.EquipmentId = cae.EquipmentId and cps.CustomerId = ticket.CustomerId
                                where ticket.CompanyId='{0}' AND ticket.CustomerId='{1}' and cae.IsService = 1 {2}
                                and eq.EquipmentId is not null
                                and eq.IsARBEnabled = 0
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, CustomerId, subquery);
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

        public DataTable GetAllEquipmentsByCompanyId(Guid companyId)
        {
            string sqlQuery = @"select
	                                eqp.EquipmentId,
	                                eqp.Name,
	                                ec.Name as EquipmentClass,
                                    eqp.Retail
                                from Equipment eqp
	                                left join EquipmentClass ec
	                                on eqp.EquipmentClassId = ec.Id and eqp.CompanyId = ec.CompanyId
                                where eqp.CompanyId = '{0}'
                                order by ec.Id";

            try
            {
                sqlQuery = string.Format(sqlQuery, companyId);
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

        public DataTable GetAllEquipmentsForOptByCompanyId(Guid companyId, int id)
        {
            string sqlQuery = @"select
	                                eqp.EquipmentId,
	                                eqp.Name,
									pd.*,
									pin.*,
									pop.*,
	                                ec.Name as EquipmentClass,
                                    eqp.Retail,
									man.Name as ManufacturerName
                                from Equipment eqp
									left join Manufacturer man on eqp.ManufacturerId = man.Id
	                                left join EquipmentClass ec
	                                on eqp.EquipmentClassId = ec.Id and eqp.CompanyId = ec.CompanyId
									Left Join PackageDevice pd on pd.EquipmentId = eqp.EquipmentId 
									Left Join PackageInclude pin on pin.EquipmentId = eqp.EquipmentId 
									Left Join PackageOptional pop on pop.EquipmentId = eqp.EquipmentId
                                    Left Join EquipmentType etype on etype.Id = eqp.EquipmentTypeId
                                where eqp.CompanyId = '{0}' AND (eqp.Name Like '%%' OR eqp.SKU Like '%%' OR man.Name Like '%%' OR etype.Name Like '%%') AND (pd.EquipmentId is  null 
								AND pop.EquipmentId is  null
								AND pin.EquipmentId is null)
                                order by ec.Id";

            try
            {
                sqlQuery = string.Format(sqlQuery, companyId);
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

        public DataTable GetAllEquipmentsForOptByCompanyId(Guid companyId, int id, string query)
        {
            string sqlQuery = @"
								select
	                                eqp.EquipmentId,
	                                eqp.Name,
									pd.*,
									pin.*,
									pop.*,
	                                ec.Name as EquipmentClass,
                                    eqp.Retail,
									man.Name as ManufacturerName
                                from Equipment eqp
									left join Manufacturer man on eqp.ManufacturerId = man.Id
	                                left join EquipmentClass ec
	                                on eqp.EquipmentClassId = ec.Id and eqp.CompanyId = ec.CompanyId
									Left Join PackageDevice pd on pd.EquipmentId = eqp.EquipmentId AND pd.PackageId='{1}'
									Left Join PackageInclude pin on pin.EquipmentId = eqp.EquipmentId AND pin.PackageId='{1}'
									Left Join PackageOptional pop on pop.EquipmentId = eqp.EquipmentId AND pop.PackageId='{1}'
                                    Left Join EquipmentType etype on etype.Id = eqp.EquipmentTypeId
                                where eqp.CompanyId = '{0}' AND (eqp.Name Like '%{2}%' OR eqp.SKU Like '%{2}%' OR man.Name Like '%{2}%' OR etype.Name Like '%{2}%')
                                AND (pd.EquipmentId is  null 
								AND pop.EquipmentId is  null
								AND pin.EquipmentId is null)
                                order by ec.Id";

            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, id, query);
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

        public DataTable GetAllEquipmentsForOptByCompanyId(Guid companyId, Guid id, string query, int equipmentClassId)
        {
            string sqlQuery = @"select
	                                eqp.EquipmentId,
	                                eqp.Name,
	                                ec.Name as EquipmentClass,
                                    eqp.Retail,
									man.Name as ManufacturerName
                                from Equipment eqp
	                                left join EquipmentClass ec
	                                on eqp.EquipmentClassId = ec.Id and eqp.CompanyId = ec.CompanyId
									left join Manufacturer man
									on eqp.ManufacturerId = man.Id
                                    Left Join EquipmentType etype 
                                    on etype.Id = eqp.EquipmentTypeId
                                where eqp.CompanyId = '{0}' AND 
                                (eqp.Name Like '%{1}%' OR eqp.SKU Like '%{1}%' OR man.Name Like '%{1}%' OR etype.Name Like '%{1}%')
								AND eqp.EquipmentClassId='{2}'
                                order by ec.Id";

            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, query, equipmentClassId);
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

        public DataTable GetAllTechEquipmentsForOptByCompanyId(Guid companyId, Guid id, string query, int equipmentClassId, Guid techid)
        {
            string sqlQuery = @"select
	                                eqp.EquipmentId,
	                                eqp.Name,
	                                ec.Name as EquipmentClass,
                                    eqp.Retail,
									man.Name as ManufacturerName
                                from InventoryTech tech
								left join Equipment eqp on eqp.EquipmentId = tech.EquipmentId
	                                left join EquipmentClass ec
	                                on eqp.EquipmentClassId = ec.Id and eqp.CompanyId = ec.CompanyId
									left join Manufacturer man
									on eqp.ManufacturerId = man.Id
                                    Left Join EquipmentType etype 
                                    on etype.Id = eqp.EquipmentTypeId
                                where eqp.CompanyId = '{0}' 
								AND (eqp.Name Like '%{1}%' OR eqp.SKU Like '%{1}%' OR man.Name Like '%{1}%' OR etype.Name Like '%{1}%')
								AND eqp.EquipmentClassId='{2}'
								and ((select SUM(Quantity) from InventoryTech where [Type] = 'Add' and tech.TechnicianId = '{3}') - (select SUM(Quantity) from InventoryTech where [Type] = 'Release' and tech.TechnicianId = '{3}')) > 0
                                group by eqp.EquipmentId,eqp.Name,ec.Name,eqp.Retail,man.Name
								";

            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, query, equipmentClassId, techid);
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

        public List<Equipment> GetAllEquipmentIdByCompanyId(Guid companyId)
        {
            string sqlQuery = @"select
	                                eqp.*
                                from Equipment eqp
                                where eqp.CompanyId = '{0}'";

            try
            {
                sqlQuery = string.Format(sqlQuery, companyId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    return GetList(cmd, -1);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public Equipment GetEquipmentById(int eqId, Guid EquipmentId = new Guid())
        {
            string sqlQuery = @"select * into #POData from(
                                select eq.* 
                                , manu.Name as ManufacturerName
                                , sup.CompanyName as SupplierName
                                , eqType.Name as EquipmentType
                                , eqClass.Name as EquipmentClass
                                , (select top(1) [filename] from EquipmentFile 
									where EquipmentId = eq.EquipmentId and FileType ='ProfilePicture') as ProfilePicture
                                , ISNULL((select sum(inv.quantity) from InventoryWarehouse inv where inv.EquipmentId = eq.EquipmentId 
								AND inv.Type = 'Add' AND inv.LocationId = '22222222-2222-2222-2222-222222222222') -
								(select sum(inv.quantity) from InventoryWarehouse inv where inv.EquipmentId = eq.EquipmentId 
								AND inv.Type = 'Release' AND inv.LocationId = '22222222-2222-2222-2222-222222222222')
								,0) as QtyOnHand
                                from equipment eq

                                left join Supplier sup on sup.id = eq.SupplierId 
	                        --and eq.SupplierId is not null and eq.SupplierId >0
                                left join EquipmentType eqType on eqType.id = eq.EquipmentTypeId 
	                                and eq.EquipmentTypeId is not null and eq.EquipmentTypeId >0
                                left join EquipmentClass eqClass on eqClass.Id = eq.EquipmentClassId 
	                                and eq.EquipmentClassId is not null and eq.EquipmentClassId >0
                                left join Manufacturer manu on eq.ManufacturerId = manu.Id
	                                and eq.ManufacturerId is not null and  eq.ManufacturerId >0 {0}) as POD
									select *from #POData

								SELECT sup.CompanyName as SupplierName, sup.Id, ev.Cost,ev.IsPrimary,ev.SKU as VendorSKU from #POData _podata
								left join Equipmentvendor ev on ev.EquipmentId = _podata.EquipmentId
								left join Supplier sup on sup.SupplierId = ev.SupplierId 

         
	                            SELECT manu.Id, em.IsPrimary as ManuIsPrimary,em.SKU as ManuSKU,manu.Name as ManufacturerName from #POData _podata
								left join EquipmentManufacturer em on em.EquipmentId = _podata.EquipmentId
								left join Manufacturer manu on manu.ManufacturerId = em.ManufacturerId

                               
                                DROP TABLE #POData";
            try
            {

                if (EquipmentId != new Guid())
                {
                    sqlQuery = string.Format(sqlQuery, "where eq.EquipmentId ='{0}'");
                }
                else if (eqId > 0)
                {
                    sqlQuery = string.Format(sqlQuery, "where eq.Id ='{1}'");
                }
                else
                {
                    return null;
                }
                sqlQuery = string.Format(sqlQuery, EquipmentId, eqId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    Equipment equipmentObject = new Equipment();
                    DataSet dsResult = GetDataSet(cmd);
                    if (dsResult != null)
                        equipmentObject.EquipmentVendorList = (from DataRow dr in dsResult.Tables[1].Rows
                                                               select new EquipmentVendor()
                                                               {
                                                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                                   SupplierName = dr["SupplierName"].ToString(),
                                                                   SKU = dr["VendorSKU"].ToString(),

                                                                   Cost = dr["Cost"] != DBNull.Value ? Convert.ToDouble(dr["Cost"]) : 0,
                                                                   IsPrimary = dr["IsPrimary"] != DBNull.Value ? Convert.ToBoolean(dr["IsPrimary"]) : false
                                                               }).ToList();

                    equipmentObject.EquipmentManufacturerList = (from DataRow dr in dsResult.Tables[2].Rows
                                                                 select new EquipmentManufacturer()
                                                                 {
                                                                     Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                                     ManufacturerName = dr["ManufacturerName"].ToString(),
                                                                     SKU = dr["ManuSKU"].ToString(),

                                                                     //Cost = dr["Cost"] != DBNull.Value ? Convert.ToDouble(dr["Cost"]) : 0,
                                                                     IsPrimary = dr["ManuIsPrimary"] != DBNull.Value ? Convert.ToBoolean(dr["ManuIsPrimary"]) : false
                                                                 }).ToList();


                    SqlDataReader reader;
                    long rows = SelectRecords(cmd, out reader);
                    using (reader)
                    {
                        if (reader.Read())
                        {
                            FillObject(equipmentObject, reader);
                            equipmentObject.ManufacturerName = reader["ManufacturerName"].ToString();
                            equipmentObject.Tag = reader["Tag"].ToString();
                            equipmentObject.SupplierName = reader["SupplierName"].ToString();
                            equipmentObject.EquipmentType = reader["EquipmentType"].ToString();
                            equipmentObject.EquipmentClass = reader["EquipmentClass"].ToString();
                            equipmentObject.ProfilePicture = reader["ProfilePicture"].ToString();
                            int QtyOnHand = 0;
                            int.TryParse(reader["QtyOnHand"].ToString(), out QtyOnHand);
                            equipmentObject.QtyOnHand = QtyOnHand;

                            return equipmentObject;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Equipment> GetEquipmentByOptions(string location, string type, string model, string finish, string capacity, Guid ManufacturerId)
        {
            string ManuSql = "";

            string sqlQuery = @"Select * from Equipment 
                                where [Location] ='{0}' 
                                and [Type] = '{1}' 
                                and Model = '{2}' 
                                and Finish = '{3}' 
                                and Capacity = '{4}'
                                {5} ";
            if (ManufacturerId != Guid.Empty)
            {
                ManuSql = string.Format(@"and ManufacturerId = (select top(1) id from Manufacturer where ManufacturerId = '{0}')", ManufacturerId);
            }

            try
            {
                sqlQuery = string.Format(sqlQuery
                                        , location
                                        , type
                                        , model
                                        , finish
                                        , capacity
                                        , ManuSql);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    return GetList(cmd, -1);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataSet GetAllSmartLeadServicesAndEquipmentsByCustomerId(Guid customerid)
        {
            string sqlQuery = @"select cps.MonthlyRate, eq.Name as EquipmentName, cps.DiscountRate, cps.Total
                                from CustomerPackageService cps
                                left join Equipment eq on eq.EquipmentId = cps.EquipmentId
                                where CustomerId = '{0}'

                                select eq.Name as EquipmentName, cpe.Quantity, cpe.UnitPrice, cpe.Total, cpe.DiscountUnitPricce
                                from CustomerPackageEqp cpe
                                left join Equipment eq on eq.EquipmentId = cpe.EquipmentId
                                where customerid = '{0}'
                                select * from Customer where CustomerId = '{0}'";

            try
            {
                sqlQuery = string.Format(sqlQuery, customerid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable GetInventoryTechByEquipmentId(Guid equipid)
        {
            string sqlQuery = @"select (select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Add' and _tech.EquipmentId = '{0}' and _tech.TechnicianId = tech.TechnicianId) 
                        - (select isnull(SUM(_tech.Quantity),0) from inventorytech _tech where _tech.[Type] = 'Release' and _tech.EquipmentId = '{0}' and _tech.TechnicianId = tech.TechnicianId) 
                        - (select isnull(sum(Quantity),0) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = '{0}' and (b.TechnicianId  = tech.TechnicianId or (b.TechnicianId = '22222222-2222-2222-2222-222222222221' and b.ReceivedBy = tech.TechnicianId)) 
                            and b.IsApprove = 0 and b.IsDecline = 0 )
                        as Quantity,
                        tech.TechnicianId, emp.FirstName + ' ' + emp.LastName as empName, tech.EquipmentId from inventorytech tech
                                left join Employee emp on emp.UserId = tech.TechnicianId
                                where tech.EquipmentId = '{0}'
                                and Quantity > 0
                                and tech.[Type] = 'Add'
                                and tech.TechnicianId not in ( '22222222-2222-2222-2222-222222222223','22222222-2222-2222-2222-222222222222','22222222-2222-2222-2222-222222222224',
	                               '22222222-2222-2222-2222-222222222225','22222222-2222-2222-2222-222222222226','22222222-2222-2222-2222-222222222231','22222222-2222-2222-2222-222222222232',
	                              '22222222-2222-2222-2222-222222222233')
								group by emp.FirstName + ' ' + emp.LastName, tech.TechnicianId, tech.EquipmentId
                                order by emp.FirstName + ' ' + emp.LastName";

            try
            {
                sqlQuery = string.Format(sqlQuery, equipid);
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

        public DataTable GetInventoryLocByEquipmentId(Guid equipid)
        {
            string sqlQuery = @"select (select isnull(SUM(_tech.Quantity),0) from InventoryWarehouse _tech where _tech.[Type] = 'Add' and _tech.EquipmentId = '{0}' and _tech.LocationId = tech.LocationId) 
                        - (select isnull(SUM(_tech.Quantity),0) from InventoryWarehouse _tech where _tech.[Type] = 'Release' and _tech.EquipmentId = '{0}' and _tech.LocationId = tech.LocationId) 
                        - (select isnull(sum(Quantity),0) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = '{0}' and (b.TechnicianId  = tech.LocationId or (b.TechnicianId = '22222222-2222-2222-2222-222222222221' and b.ReceivedBy = tech.LocationId)) 
                            and b.IsApprove = 0 and b.IsDecline = 0 )
                        as Quantity,
                        tech.LocationId, emp.UserName, tech.EquipmentId from InventoryWarehouse tech
                                left join Employee emp on emp.UserId = tech.LocationId
                                where tech.EquipmentId = '{0}'
                                and Quantity > 0
                                and tech.[Type] = 'Add'
                                and tech.LocationId in ('22222222-2222-2222-2222-222222222223','22222222-2222-2222-2222-222222222224',
                                '22222222-2222-2222-2222-222222222225','22222222-2222-2222-2222-222222222226','22222222-2222-2222-2222-222222222231',
                                '22222222-2222-2222-2222-222222222232')
								group by emp.UserName, tech.LocationId, tech.EquipmentId
                                order by emp.UserName";

            try
            {
                sqlQuery = string.Format(sqlQuery, equipid);
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
        public DataTable GetIncludeEstimateEquipment()
        {

            string sqlQuery = @"SELECT * from Equipment where IsIncludeEstimate = 1";
            try
            {
                sqlQuery = string.Format(sqlQuery);
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

        #region Inventory Count Report
        public DataSet GetInventoryCountReport(DateTime? start, DateTime? end, int pageno, int pagesize, string searchtext, string order)
        {
            string SearchText = "";
            string StartDateQuery = "";
            string EndDateQuery = "";
            string DateQuery = "";
            string RMADateQuery = "";
            string FilterDateQuery = "";
            string orderquery = "";
            string orderquery1 = "";
            if (end.ToString() == ("1/1/0001 11:59:59 PM"))
            {
                end = ("1/1/0001 12:00:00 AM").ToDateTime();
            }
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
                else if (order == "ascending/sku")
                {
                    orderquery = "order by #ed.SKU asc";
                    orderquery1 = "order by SKU asc";
                }
                else if (order == "descending/sku")
                {
                    orderquery = "order by #ed.SKU desc";
                    orderquery1 = "order by SKU desc";
                }
                else if (order == "ascending/onhandstart")
                {
                    orderquery = "order by #ed.[QuantityOnStartDate] asc";
                    orderquery1 = "order by [QuantityOnStartDate] asc";
                }
                else if (order == "descending/onhandstart")
                {
                    orderquery = "order by #ed.[QuantityOnStartDate] desc";
                    orderquery1 = "order by [QuantityOnStartDate] desc";
                }
                else if (order == "ascending/onhandend")
                {
                    orderquery = "order by #ed.[QuantityOnEndDate] asc";
                    orderquery1 = "order by [QuantityOnEndDate] asc";
                }
                else if (order == "descending/onhandend")
                {
                    orderquery = "order by #ed.[QuantityOnEndDate] desc";
                    orderquery1 = "order by [QuantityOnEndDate] desc";
                }
                else if (order == "ascending/used")
                {
                    orderquery = "order by #ed.[Used] asc";
                    orderquery1 = "order by [Used] asc";
                }
                else if (order == "descending/used")
                {
                    orderquery = "order by #ed.[Used] desc";
                    orderquery1 = "order by [Used] desc";
                }
                else if (order == "ascending/purchase")
                {
                    orderquery = "order by #ed.[Purchase] asc";
                    orderquery1 = "order by [Purchase] asc";
                }
                else if (order == "descending/purchase")
                {
                    orderquery = "order by #ed.[Purchase] desc";
                    orderquery1 = "order by [Purchase] desc";
                }
                else if (order == "ascending/rma")
                {
                    orderquery = "order by #ed.[RMA] asc";
                    orderquery1 = "order by [RMA] asc";
                }
                else if (order == "descending/rma")
                {
                    orderquery = "order by #ed.[RMA] desc";
                    orderquery1 = "order by [RMA] desc";
                }
                else
                {
                    orderquery = "order by #ed.[SKU]  desc";
                    orderquery1 = "order by SKU desc";
                }

            }
            else
            {
                orderquery = "order by #ed.[SKU] desc";
                orderquery1 = "order by SKU desc";
            }
            #endregion
            if (start != new DateTime())
            {
                StartDateQuery = string.Format("and invinner.LastUpdatedDate <= '{0}'", start);

            }
            if (end != new DateTime() && end.HasValue)
            {
                EndDateQuery = string.Format("and invinner2.LastUpdatedDate <= '{0}'", end);

            }
            if (start != new DateTime() && end != new DateTime())
            {
                DateQuery = string.Format("and invinner.LastUpdatedDate between '{0}' and '{1}'", start, end);

            }
            if (start != new DateTime() || end != new DateTime())
            {
                RMADateQuery = string.Format("and EquRe.PurchaseDate between '{0}' and '{1}'", start, end);

            }
            if (start != new DateTime() || end != new DateTime())
            {
                FilterDateQuery = string.Format("and Eq.CreatedDate between '{0}' and '{1}'", start, end);

            }
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                SearchText = string.Format("and SKU like '%{0}%' or  Name like '%{0}%'", searchtext);
            }

            string sqlQuery = @"	DECLARE @Date nvarchar(50)
	                                DECLARE @pagestart int
	                                DECLARE @pageend int
	                                DECLARE @pageno int
	                                DECLARE @pagesize int

                                    SET @pageno = {0} --default 1
									SET @pagesize = {1} --default 10
                                    SET @pagestart=(@pageno-1)* @pagesize 
                                    SET @pageend = @pagesize

                                select DISTINCT Eq.SKU
                                ,Eq.Name
                                ,Eq.EquipmentId
                                ,(((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=Eq.EquipmentId and Type='Add' {2})
                                - (Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=Eq.EquipmentId and Type='Release' {2}))
                                +
								((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=Eq.EquipmentId and Type='Add' {2})
                                - (Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=Eq.EquipmentId and Type='Release' {2}))
                                ) as QuantityOnStartDate

                                ,(((Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=Eq.EquipmentId and Type='Add' {3})
                                - (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=Eq.EquipmentId and Type='Release' {3}))
                                +
								((Select ISNULL(SUM(invinner2.Quantity),0) from InventoryTech invinner2 where invinner2.EquipmentId=Eq.EquipmentId and Type='Add' {3})
                                - (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryTech invinner2 where invinner2.EquipmentId=Eq.EquipmentId and Type='Release' {3}))
                                ) as QuantityOnEndDate
                                

                                ,(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=Eq.EquipmentId and Type='Release' {4}) as Used 

                                ,(Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=Eq.EquipmentId and Type='Add' {4}) as Purchase

                                
                                ,(Select ISNULL(SUM(EquRe.Quantity),0) from Equipmentreturn EquRe where EquRe.EquipmentId=Eq.EquipmentId {6}) as RMA

                                into #EquipmentData

                                 from Equipment Eq
                                left join InventoryWarehouse IW on Eq.EquipmentId = IW.EquipmentId
                                where Eq.EquipmentClassId = 1
                                
                                {5}
                                {7}
                                select *into EquipmentDataFilter
								From #EquipmentData
								where QuantityOnStartDate > 0 or QuantityOnEndDate > 0 or Used > 0 or Purchase > 0 or RMA > 0

                                SELECT TOP (@pagesize) #ed.* into #TestTable
                                                                FROM EquipmentDataFilter #ed
                                                                where EquipmentId NOT IN(Select TOP (@pagestart) EquipmentId from EquipmentDataFilter #ed {8})
                                                                {9}
                                                                select count(EquipmentId) as [TotalCount] from EquipmentDataFilter

																select * from #TestTable
																select sum(QuantityOnStartDate) as TotalOnHandStartDate
																,sum(QuantityOnEndDate) as TotalOnHandEndDate
																,sum(Used) as TotalUsed
																,sum(Purchase) as TotalPurchase
																,sum(RMA) as TotalRMA
																from #TestTable

                                                                DROP TABLE #EquipmentData
																DROP TABLE EquipmentDataFilter
																Drop Table #TestTable";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        pageno,
                                        pagesize,
                                        StartDateQuery,
                                        EndDateQuery,
                                        DateQuery,
                                        SearchText,
                                        RMADateQuery,
                                        FilterDateQuery,
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
        public DataSet GetInventoryCountOnStartDate(DateTime? start, Guid Id)
        {
            string StartDateQuery = "";
            if (start != new DateTime())
            {
                StartDateQuery = string.Format("and LastUpdatedDate <= '{0}'", start);
            }
            string sqlQuery = @"	select TechnicianId into #TechTemp From InventoryTech
	                                where EquipmentId = '{1}'
	                                group by TechnicianId 


	                                select FirstName + ' ' +LastName as Name ,
	                                (Isnull((select SUM(Quantity) from InventoryTech where EquipmentId = '{1}' and Type = 'ADD' and TechnicianId = emp.userid {0}), 0)-
	                                Isnull((select SUM(Quantity) from InventoryTech where EquipmentId = '{1}' and Type = 'Release' and TechnicianId = emp.userid {0}),0) ) as Quantity
	
	                                from  Employee emp  
	                                where userid in (select TechnicianId from #TechTemp) 

	                                UNION

	                                select DISTINCT
					
								    'Warehouse' as Name
								    ,((Select ISNULL(SUM(Quantity),0) from InventoryWarehouse where EquipmentId=Eq.EquipmentId and Type='Add' {0})
                                    - (Select ISNULL(SUM(Quantity),0) from InventoryWarehouse where EquipmentId=Eq.EquipmentId and Type='Release' {0})) as Quantity
                                 
								     from Equipment Eq
                                    left join InventoryWarehouse IW on Eq.EquipmentId = IW.EquipmentId
                                    where Eq.EquipmentClassId = 1
								    and Eq.EquipmentId = '{1}'


	                                drop table #TechTemp";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        StartDateQuery,
                                        Id
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
        public DataSet GetInventoryCountOnEndDate(DateTime? end, Guid Id)
        {
            string EndDateQuery = "";
            if (end != new DateTime())
            {
                EndDateQuery = string.Format("and LastUpdatedDate <= '{0}'", end);
            }
            string sqlQuery = @"select TechnicianId into #TechTemp From InventoryTech
	                                where EquipmentId = '{1}'
	                                group by TechnicianId 


	                                select FirstName + ' ' +LastName as Name ,
	                                (Isnull((select SUM(Quantity) from InventoryTech where EquipmentId = '{1}' and Type = 'ADD' and TechnicianId = emp.userid {0}), 0)-
	                                Isnull((select SUM(Quantity) from InventoryTech where EquipmentId = '{1}' and Type = 'Release' and TechnicianId = emp.userid {0}),0) ) as Quantity
	
	                                from  Employee emp  
	                                where userid in (select TechnicianId from #TechTemp) 

	                                UNION

	                                select DISTINCT
					
								    'Warehouse' as Name
								    ,((Select ISNULL(SUM(Quantity),0) from InventoryWarehouse invinner where EquipmentId=Eq.EquipmentId and Type='Add' {0})
                                    - (Select ISNULL(SUM(Quantity),0) from InventoryWarehouse invinner where EquipmentId=Eq.EquipmentId and Type='Release' {0})) as Quantity
                                 
								     from Equipment Eq
                                    left join InventoryWarehouse IW on Eq.EquipmentId = IW.EquipmentId
                                    where Eq.EquipmentClassId = 1
								    and Eq.EquipmentId = '{1}'


	                                drop table #TechTemp";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        EndDateQuery,
                                        Id
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
        public DataSet UsedInventoryCountDetails(Guid Id, DateTime? start, DateTime? end)
        {
            string DateQuery = "";

            if (start != new DateTime() && end != new DateTime())
            {
                DateQuery = string.Format("and CusApp.CreatedDate between '{0}' and '{1}'", start, end);

            }

            string sqlQuery = @"select				
								cus.FirstName +' '+ cus.LastName as Name
								,cus.Id as CustomerId
								,Tk.Id as TicketId
								,CusApp.Quantity as Quantity
									from Equipment Eq
								left join CustomerAppointmentEquipment CusApp on Eq.EquipmentId = CusApp.EquipmentId
								left join Ticket Tk on Tk.TicketId = CusApp.AppointmentId
								left join Customer cus on cus.CustomerId = Tk.CustomerId
								where Eq.EquipmentClassId = 1
								and Eq.EquipmentId = '{0}'
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
        public DataSet PurchaseInventoryCountDetails(Guid Id, DateTime? start, DateTime? end)
        {
            string DateQuery = "";

            if (start != new DateTime() || end != new DateTime())
            {
                DateQuery = string.Format("and IW.LastUpdatedDate between '{0}' and '{1}'", start, end);

            }

            string sqlQuery = @"select 
		                                 POW.Id
		                                ,IW.PurchaseOrderId
                                        ,emp.FirstName + ' '+ emp.LastName as CreatedBy
		                                ,IW.LastUpdatedDate
		                                ,IW.Quantity 
		                                        from InventoryWarehouse IW
		                                    left join Employee emp on emp.UserId= IW.LastUpdatedBy
                                            left join PurchaseOrderWarehouse POW on POW.PurchaseOrderId = IW.PurchaseOrderId
		                                    where IW.Type='Add'
		                                    and IW.EquipmentId = '{0}'
		                                    {1}
                                        Order by PurchaseOrderId desc";

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
        public DataSet RMADetailsOfInventoryCount(Guid Id, DateTime? start, DateTime? end)
        {
            string DateQuery = "";

            if (start != new DateTime() || end != new DateTime())
            {
                DateQuery = string.Format("and EquRe.PurchaseDate between '{0}' and '{1}'", start, end);

            }

            string sqlQuery = @"select
                                            cus.FirstName +' '+ cus.LastName as Name
								            ,cus.Id as CustomerId
								            ,sum(EquRe.Quantity) as Quantity
									            from Equipmentreturn EquRe
								            left join Customer cus on EquRe.CustomerId = cus.CustomerId
								            where  EquRe.EquipmentId = '{0}'
                                            {1}
								            group by cus.FirstName +' '+ cus.LastName, cus.Id
								            order by cus.Id desc";

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
        public DataTable GetInventoryCountReportForDownload(DateTime? start, DateTime? end, string searchtext)
        {
            string SearchText = "";
            string StartDateQuery = "";
            string EndDateQuery = "";
            string DateQuery = "";
            string RMADateQuery = "";
            string FilterDateQuery = "";
            if (end.ToString() == ("1/1/0001 11:59:59 PM"))
            {
                end = ("1/1/0001 12:00:00 AM").ToDateTime();
            }
            if (start != new DateTime())
            {
                StartDateQuery = string.Format("and invinner.LastUpdatedDate <= '{0}'", start);

            }
            if (end != new DateTime())
            {
                EndDateQuery = string.Format("and invinner2.LastUpdatedDate <= '{0}'", end);

            }
            if (start != new DateTime() && end != new DateTime())
            {
                DateQuery = string.Format("and invinner.LastUpdatedDate between '{0}' and '{1}'", start, end);

            }
            if (start != new DateTime() || end != new DateTime())
            {
                RMADateQuery = string.Format("and EquRe.PurchaseDate between '{0}' and '{1}'", start, end);

            }
            if (start != new DateTime() || end != new DateTime())
            {
                FilterDateQuery = string.Format("and Eq.CreatedDate between '{0}' and '{1}'", start, end);

            }
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                SearchText = string.Format("and SKU like '%{0}%' or  Name like '%{0}%'", searchtext);
            }
            string Start = "";
            if (start != null && start != new DateTime())
            {
                Start = "On Hand on " + start.Value.ToString("MM/dd/yyyy");
            }
            else
            {
                Start = "On Hand on ";
            }

            string End = "";
            if (end != null && end != new DateTime())
            {
                End = "On Hand_on " + end.Value.ToString("MM/dd/yyyy");
            }
            else
            {
                End = "On Hand_on ";
            }
            string sqlQuery = @"
                                DECLARE @Start nvarchar(50)
                                DECLARE @End nvarchar(50) 

                                SET @Start = '{0}'
                                SET @End = '{1}'

                                select DISTINCT Eq.SKU
                                ,Eq.Name
                                 , Eq.EquipmentId
                                ,(((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=Eq.EquipmentId and Type='Add' {2})
                                - (Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=Eq.EquipmentId and Type='Release' {2}))
                                +
								((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=Eq.EquipmentId and Type='Add' {2})
                                - (Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=Eq.EquipmentId and Type='Release' {2}))
                                ) as [On Hand on]

                                ,(((Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=Eq.EquipmentId and Type='Add' {3})
                                - (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=Eq.EquipmentId and Type='Release' {3}))
                                +
								((Select ISNULL(SUM(invinner2.Quantity),0) from InventoryTech invinner2 where invinner2.EquipmentId=Eq.EquipmentId and Type='Add' {3})
                                - (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryTech invinner2 where invinner2.EquipmentId=Eq.EquipmentId and Type='Release' {3}))
                                ) as [On Hand_on]
                                

                                ,(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=Eq.EquipmentId and Type='Release' {4}) as Used 

                                ,(Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=Eq.EquipmentId and Type='Add' {4}) as Purchase

								,(Select ISNULL(SUM(EquRe.Quantity),0) from Equipmentreturn EquRe where EquRe.EquipmentId=Eq.EquipmentId {6}) as RMA

                                into #EquipmentData 

                                 from Equipment Eq
                                left join InventoryWarehouse IW on Eq.EquipmentId = IW.EquipmentId
                                where Eq.EquipmentClassId = 1
								{5}
                                {7}
                                select * from #EquipmentData
                                where [On Hand on] > 0 or [On Hand_on] > 0 or Used > 0 or Purchase > 0 or RMA > 0
								Order by SKU asc

                                DROP TABLE #EquipmentData";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        Start,
                                        End,
                                        StartDateQuery,
                                        EndDateQuery,
                                        DateQuery,
                                        SearchText,
                                        RMADateQuery,
                                        FilterDateQuery
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

    }
}
