using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.DataAccess
{
    public partial class CustomerAppointmentEquipmentDataAccess
    {
        public CustomerAppointmentEquipmentDataAccess(string ConStr) : base(ConStr) { }
        public DataTable GetAllCustomerAppointmentEquipmentListByAppointmentId(Guid AppointmentId)
        { 
            string sqlQuery = @"select						   
                               CAE.[Id]
                              ,CAE.[AppointmentId]
                              ,CAE.[EquipmentId]
                              ,CAE.[Quantity]
                              ,CAE.[IsEquipmentRelease]
                              ,ep.SupplierCost
							  ,ep.Retail
                              ,ep.RepCost
                              ,ep.SKU
                              ,ep.EquipmentClassId as EquipmentClassId
                              ,CAE.[UnitPrice]
                              ,CAE.[OriginalUnitPrice]
                              ,CAE.[TotalPrice]
                              ,CAE.[CreatedDate]
                              ,CAE.IsService
                              ,CAE.IsAgreementItem
                              ,CAE.[CreatedBy]
                              ,CAE.[CreatedByUid]
	                          ,CAE.[EquipName]
                              ,CAE.[EquipName] as EquipmentName
                              ,CAE.[EquipDetail]
                              ,CAE.IsDefaultService,
                               CAE.IsInvoiceCreate
                              ,CAE.ReferenceInvoiceId,
                              CAE.ReferenceInvDetailId
                                ,CAE.IsBilling
							  ,EF.[FileDescription]
							  ,EF.Filename
							  ,EF.FileFullName
							  ,EF.FileType
							  ,EF.EquipmentId
                              ,tu.UserId as TechnicianId
                              ,emp.FirstName + ' ' +emp.LastName as CreatedByName
                              ,CAE.IsBaseItem
                                ,CAE.IsBadInventory
                              ,CASE 
									WHEN tu.UserId='22222222-2222-2222-2222-222222222222' AND CAE.QuantityLeftEquipment=0 THEN 0
									WHEN tu.UserId='22222222-2222-2222-2222-222222222221' AND CAE.QuantityLeftEquipment=0 THEN 0
									ELSE 
                                    (SELECT 
                                    ISNULL(SUM(Quantity),0) AS TotalQty
                                    FROM
                                    (SELECT 
                                    ISNULL(SUM(CASE WHEN Type = 'ADD' THEN Quantity ELSE -Quantity END),0) AS Quantity
                                    FROM 
                                    InventoryTech
                                    WHERE 
                                    EquipmentId =ep.EquipmentId AND
                                    TechnicianId = tu.UserId
                                    GROUP BY 
                                    TechnicianId										  
                               HAVING 
                               SUM(CASE WHEN Type = 'ADD' THEN Quantity ELSE -Quantity END) >= 0) AS Qty)
                               - isnull((select sum(Quantity) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = ep.EquipmentId and b.IsApprove = 0 and b.IsDecline = 0 And b.TechnicianId=tu.UserId),0)END QuantityOnHand
                             ,ISNULL(IsCheckedEquipment, 0) as IsCheckedEquipment
                             ,CAE.QuantityLeftEquipment
                             ,CAE.IsCopied
                             ,CAE.isEquipmentExist
                             ,ep.Point as  Point
                             ,ISNULL((select top(1) ev.Cost from EquipmentVendor ev  where ev.EquipmentId = ep.EquipmentId and IsPrimary = 1),0) as EquipmentVendorCost
							 ,format(CAE.Quantity*ep.Point,'N2') as  TotalPoint
                             , CAE.IsBillingProcess
                             ,ep.IsARBEnabled
                             from CustomerAppointmentEquipment CAE
                             LEFT JOIN Equipment ep on ep.EquipmentId=CAE.EquipmentId
                       --      Left Join EquipmentVendor ev on ev.EquipmentId = CAE.EquipmentId
                             LEFT JOIN TicketUser tu on tu.TiketId=CAE.AppointmentId and tu.IsPrimary=1
                             LEFT JOIN Employee emp on CAE.CreatedByUid = emp.UserId
                            Left Join EquipmentFile EF on CAE.EquipmentId = EF.EquipmentId AND EF.FileType = '{1}'
                            where CAE.AppointmentId = '{0}' ORDER BY 
                            CAE.CreatedDate ASC,
                            CAE.Id ASC";
            try
            {
                sqlQuery = string.Format(sqlQuery, AppointmentId, "ProfilePicture");
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
        public DataTable NewGetAllCustomerAppointmentEquipmentListByAppointmentId(Guid AppointmentId, string order)
        {
            string orderquery = "";
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/equipname")
                {
                    orderquery = "Order by EquipName Asc";
                }
                else if (order == "descending/equipname")
                {
                    orderquery = "Order by EquipName Desc";
                }
                else if (order == "ascending/sku")
                {
                    orderquery = "Order by SKU Asc";
                }
                else if (order == "descending/sku")
                {
                    orderquery = "Order by SKU Desc";
                }
                else if (order == "ascending/equipdetail")
                {
                    orderquery = "Order by EquipDetail Asc";
                }
                else if (order == "descending/equipdetail")
                {
                    orderquery = "Order by EquipDetail Desc";
                }
                else if (order == "ascending/totalpoint")
                {
                    orderquery = "Order by TotalPoint Asc";
                }
                else if (order == "descending/totalpoint")
                {
                    orderquery = "Order by TotalPoint Desc";
                }
                else if (order == "ascending/unitprice")
                {
                    orderquery = "Order by UnitPrice Asc";
                }
                else if (order == "descending/unitprice")
                {
                    orderquery = "Order by UnitPrice Desc";
                }
                else if (order == "ascending/equipmentvendorcost")
                {
                    orderquery = "Order by EquipmentVendorCost Asc";
                }
                else if (order == "descending/equipmentvendorcost")
                {
                    orderquery = "Order by EquipmentVendorCost Desc";
                }
                else if (order == "ascending/quantity")
                {
                    orderquery = "Order by Quantity Asc";
                }
                else if (order == "descending/quantity")
                {
                    orderquery = "Order by Quantity Desc";
                }
                else if (order == "ascending/quantityleftequipment")
                {
                    orderquery = "Order by QuantityLeftEquipment Asc";
                }
                else if (order == "descending/quantityleftequipment")
                {
                    orderquery = "Order by QuantityLeftEquipment Desc";
                }
                else if (order == "ascending/quantityonHand")
                {
                    orderquery = "Order by QuantityOnHand Asc";
                }
                else if (order == "descending/quantityonHand")
                {
                    orderquery = "Order by QuantityOnHand Desc";
                }
                else
                {
                    orderquery = "Order by Id Desc";
                }
            }
            string sqlQuery = @"select						   
                               CAE.[Id]
                              ,CAE.[AppointmentId]
                              ,CAE.[EquipmentId]
                              ,CAE.[Quantity]
                              ,CAE.[IsEquipmentRelease]
                              ,ep.SupplierCost
							  ,ep.Retail
                              ,ep.RepCost
                              ,ep.SKU
                              ,ep.EquipmentClassId as EquipmentClassId
                              ,CAE.[UnitPrice]
                              ,CAE.[OriginalUnitPrice]
                              ,CAE.[TotalPrice]
                              ,CAE.[CreatedDate]
                              ,CAE.IsService
                              ,CAE.IsAgreementItem
                              ,CAE.[CreatedBy]
                              ,CAE.[CreatedByUid]
	                          ,CAE.[EquipName]
                              ,CAE.[EquipName] as EquipmentName
                              ,CAE.[EquipDetail]
                              ,CAE.IsDefaultService,
                               CAE.IsInvoiceCreate
                              ,CAE.ReferenceInvoiceId,
                              CAE.ReferenceInvDetailId
                                ,CAE.IsBilling
							  ,EF.[FileDescription]
							  ,EF.Filename
							  ,EF.FileFullName
							  ,EF.FileType
							  ,EF.EquipmentId
                              ,tu.UserId as TechnicianId
                              ,emp.FirstName + ' ' +emp.LastName as CreatedByName
                              ,CAE.IsBaseItem
                                ,CAE.IsBadInventory
                              ,CASE 
									WHEN tu.UserId='22222222-2222-2222-2222-222222222222' AND CAE.QuantityLeftEquipment=0 THEN 0
									WHEN tu.UserId='22222222-2222-2222-2222-222222222221' AND CAE.QuantityLeftEquipment=0 THEN 0
									ELSE 
                                    (SELECT 
                                    ISNULL(SUM(Quantity),0) AS TotalQty
                                    FROM
                                    (SELECT 
                                    ISNULL(SUM(CASE WHEN Type = 'ADD' THEN Quantity ELSE -Quantity END),0) AS Quantity
                                    FROM 
                                    InventoryTech
                                    WHERE 
                                    EquipmentId =ep.EquipmentId AND
                                    TechnicianId = tu.UserId
                                    GROUP BY 
                                    TechnicianId										  
                               HAVING 
                               SUM(CASE WHEN Type = 'ADD' THEN Quantity ELSE -Quantity END) >= 0) AS Qty)
                               - isnull((select sum(Quantity) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = ep.EquipmentId and b.IsApprove = 0 and b.IsDecline = 0 And b.TechnicianId=tu.UserId),0)END QuantityOnHand
                             ,ISNULL(IsCheckedEquipment, 0) as IsCheckedEquipment
                             ,CAE.QuantityLeftEquipment
                             ,CAE.IsCopied
                             ,CAE.isEquipmentExist
                             ,ep.Point as  Point
                             ,ISNULL((select top(1) ev.Cost from EquipmentVendor ev  where ev.EquipmentId = ep.EquipmentId and IsPrimary = 1),0) as EquipmentVendorCost
							 ,format(CAE.Quantity*ep.Point,'N2') as  TotalPoint
                             , CAE.IsBillingProcess
                             ,ep.IsARBEnabled
                             from CustomerAppointmentEquipment CAE
                             LEFT JOIN Equipment ep on ep.EquipmentId=CAE.EquipmentId
                       --      Left Join EquipmentVendor ev on ev.EquipmentId = CAE.EquipmentId
                             LEFT JOIN TicketUser tu on tu.TiketId=CAE.AppointmentId and tu.IsPrimary=1
                             LEFT JOIN Employee emp on CAE.CreatedByUid = emp.UserId
                            Left Join EquipmentFile EF on CAE.EquipmentId = EF.EquipmentId AND EF.FileType = '{1}'
                            where CAE.AppointmentId = '{0}' {2}";
            try
            {
                sqlQuery = string.Format(sqlQuery, AppointmentId, "ProfilePicture", orderquery);
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

        public DataTable GetAllCustomerAppointmentEquipmentListByAppointmentIdForCommission(Guid AppointmentId)
        {
            string sqlQuery = @"select						   
                               CAE.[Id]
                              ,CAE.[AppointmentId]
                              ,CAE.[EquipmentId]
                              ,CAE.[Quantity]
                              ,CAE.[IsEquipmentRelease]
                              ,CAE.[IsEquipmentExist]
                              ,ep.SupplierCost
							  ,ep.Retail
                              ,ep.RepCost
                              ,ep.SKU
                              ,ep.EquipmentClassId as EquipmentClassId
                              ,CAE.[UnitPrice]
                              ,CAE.[OriginalUnitPrice]
                              ,CAE.[TotalPrice]
                              ,CAE.[CreatedDate]
                              ,CAE.IsService
                              ,CAE.IsAgreementItem
                              ,CAE.[CreatedBy]
                              ,CAE.[CreatedByUid]
                              ,CAE.[InstalledByUid]
	                          ,CAE.[EquipName]
                              ,CAE.[EquipName] as EquipmentName
                              ,CAE.[EquipDetail]
                              ,CAE.IsDefaultService,
                               CAE.IsInvoiceCreate
                              ,CAE.ReferenceInvoiceId,
                              CAE.ReferenceInvDetailId
                                ,CAE.IsBilling
							  ,EF.[FileDescription]
							  ,EF.Filename
							  ,EF.FileFullName
							  ,EF.FileType
							  ,EF.EquipmentId
                              ,tu.UserId as TechnicianId
                              ,emp.FirstName + ' ' +emp.LastName as CreatedByName
                              ,CAE.IsBaseItem
                                ,CAE.IsBadInventory
                              ,(ISNULL((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=CAE.EquipmentId and Type='Add'  And invinner.TechnicianId=tu.UserId)-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=CAE.EquipmentId and Type='Release'  And invinner.TechnicianId=tu.UserId),0)) QuantityOnHand
                             ,ISNULL(IsCheckedEquipment, 0) as IsCheckedEquipment
                             ,CAE.QuantityLeftEquipment
                             ,CAE.IsCopied
                            ,ISNULL(ep.Point, 0) as Point
                             from CustomerAppointmentEquipment CAE
                             LEFT JOIN Equipment ep on ep.EquipmentId=CAE.EquipmentId
                             LEFT JOIN TicketUser tu on tu.TiketId=CAE.AppointmentId and tu.IsPrimary=1
                             LEFT JOIN Employee emp on CAE.CreatedByUid = emp.UserId
                            Left Join EquipmentFile EF on CAE.EquipmentId = EF.EquipmentId AND EF.FileType = '{1}'
                            where CAE.AppointmentId = '{0}' and IsNonCommissionable != 1";
            try
            {
                sqlQuery = string.Format(sqlQuery, AppointmentId, "ProfilePicture");
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
        public DataTable GetAllCustomerAppointmentEquipListByAppointmentIdForPoint(Guid AppointmentId)
        {
            string sqlQuery = @"select						   
                               CAE.[Id]
                              ,CAE.[AppointmentId]
                              ,CAE.[EquipmentId]
                              ,CAE.[Quantity]
                              ,CAE.[IsEquipmentRelease]
                              ,CAE.[IsEquipmentExist]
                              ,ep.SupplierCost
							  ,ep.Retail
                              ,ep.RepCost
                              ,ep.SKU
                              ,ep.EquipmentClassId as EquipmentClassId
                              ,CAE.[UnitPrice]
                              ,CAE.[OriginalUnitPrice]
                              ,CAE.[TotalPrice]
                              ,CAE.[CreatedDate]
                              ,CAE.IsService
                              ,CAE.IsAgreementItem
                              ,CAE.[CreatedBy]
                              ,CAE.[CreatedByUid]
                              ,CAE.[InstalledByUid]
	                          ,CAE.[EquipName]
                              ,CAE.[EquipName] as EquipmentName
                              ,CAE.[EquipDetail]
                              ,CAE.IsDefaultService,
                               CAE.IsInvoiceCreate
                              ,CAE.ReferenceInvoiceId,
                              CAE.ReferenceInvDetailId
                                ,CAE.IsBilling
							  ,EF.[FileDescription]
							  ,EF.Filename
							  ,EF.FileFullName
							  ,EF.FileType
							  ,EF.EquipmentId
                              ,tu.UserId as TechnicianId
                              ,emp.FirstName + ' ' +emp.LastName as CreatedByName
                              ,CAE.IsBaseItem
                                ,CAE.IsBadInventory
                              ,(ISNULL((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=CAE.EquipmentId and Type='Add'  And invinner.TechnicianId=tu.UserId)-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=CAE.EquipmentId and Type='Release'  And invinner.TechnicianId=tu.UserId),0)) QuantityOnHand
                             ,ISNULL(IsCheckedEquipment, 0) as IsCheckedEquipment
                             ,CAE.QuantityLeftEquipment
                             ,CAE.IsCopied
                            ,ISNULL(ep.Point, 0) as Point
                             from CustomerAppointmentEquipment CAE
                             LEFT JOIN Equipment ep on ep.EquipmentId=CAE.EquipmentId
                             LEFT JOIN TicketUser tu on tu.TiketId=CAE.AppointmentId and tu.IsPrimary=1
                             LEFT JOIN Employee emp on CAE.CreatedByUid = emp.UserId
                            Left Join EquipmentFile EF on CAE.EquipmentId = EF.EquipmentId AND EF.FileType = '{1}'
                            where CAE.AppointmentId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, AppointmentId, "ProfilePicture");
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
        public DataTable GetCustomerAppointmentEquipmentPointByAppointmentId(Guid AppointmentId)
        {
            string sqlQuery = @"select emp.FirstName +' '+emp.LastName as EmployeeName,
                                SUM(eqp.Point*cae.Quantity) as Point
                                from CustomerAppointmentEquipment cae
                                LEFT JOIN Employee emp on emp.UserId=cae.CreatedByUid
                                LEFT JOIN Equipment eqp on eqp.EquipmentId=cae.EquipmentId
                                where cae.AppointmentId='{0}'
                                and eqp.EquipmentClassId=1
                                Group by emp.FirstName,emp.LastName";
            try
            {
                sqlQuery = string.Format(sqlQuery, AppointmentId);
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
        public DataTable GetAllCustomerAppointmentEquipmentByCusId(Guid cusid)
        {
            string sqlQuery = @"select ca.* 
                                from CustomerAppointmentEquipment ca 
                                where ca.AppointmentId in (Select TicketId from Ticket where CustomerId='{0}')
                               ";

            try
            {
                sqlQuery = string.Format(sqlQuery, cusid);
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
        public DataTable GetCAEListByTicketIdUserId(Guid ticketId, Guid UserId, bool withNonCommission, string Type, int CommissionIntId)
        {
            string query = "";
            string queryTableName = "";
            if (Type == "Sales")
            {
                if (withNonCommission)
                {
                    query = string.Format(" AND CreatedByUid ='{0}'", UserId);
                }
                else
                {
                    query = string.Format(" AND CreatedByUid ='{0}' AND IsNonCommissionable != 1", UserId);
                }
                queryTableName = "SalesCommission";
            }
            else if (Type == "Tech")
            {
                if (withNonCommission)
                {
                    query = string.Format(" AND InstalledByUid ='{0}'", UserId);
                }
                else
                {
                    query = string.Format(" AND InstalledByUid ='{0}' AND IsNonCommissionable != 1", UserId);
                }
                queryTableName = "TechCommission";
            }
            string sqlQuery = @"select
                                eqp.Name as EquipName,
                                cae.Quantity,
                                cae.Quantity*eqp.Point as Point
                                from CustomerAppointmentEquipment cae
                                LEFT JOIN Equipment eqp on eqp.EquipmentId=cae.EquipmentId
                                where cae.IsService!=1 AND cae.AppointmentId='{0}' {1}
								UNION
								Select 
								'Adjustment' as EquipName,
								'1' as Quantity,
								sc.AdjustablePoint as Point
								From {3} sc
								where sc.Id={2}";
            try
            {
                sqlQuery = string.Format(sqlQuery, ticketId, query, CommissionIntId, queryTableName);
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
        public bool DeleteAllCustomerAppointmentEquipmentByAppointmentId(Guid AppointmentId)
        {
            string SqlQuery = @"Declare @AppoinmentId uniqueidentifier
                                set @AppoinmentId = '{0}'

                                delete from CustomerAppointmentEquipment 
                                where AppointmentId = @AppoinmentId and EquipmentId 
                                in ( select EquipmentId from Equipment where EquipmentId 
                                in  ( select EquipmentId from CustomerAppointmentEquipment 
                                where AppointmentId = @AppoinmentId And IsEquipmentRelease=0) and EquipmentClassId != 2)";
            SqlQuery = string.Format(SqlQuery, AppointmentId);
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
        public bool DeleteCustomerAppoinmentEquipmentByTicketId(Guid TicketId)
        {
            string SqlQuery = @"Declare @AppoinmentId uniqueidentifier
                                set @AppoinmentId = '{0}'

                                delete from CustomerAppointmentEquipment 
                                where AppointmentId = @AppoinmentId";
            SqlQuery = string.Format(SqlQuery, TicketId);
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
        public bool DeleteCustomerAppoinmentEquipmentByTicketIdEquipment(Guid TicketId)
        {
            string SqlQuery = @"Declare @AppoinmentId uniqueidentifier
                                set @AppoinmentId = '{0}'
 
                                delete cae from CustomerAppointmentEquipment cae
                                LEFT JOIN equipment eqp on eqp.EquipmentId=cae.EquipmentId
                                where cae.AppointmentId = @AppoinmentId and eqp.EquipmentClassId=1";
            SqlQuery = string.Format(SqlQuery, TicketId);
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
        public bool DeleteCustomerAppoinmentEquipmentByTicketIdService(Guid TicketId)
        {
            string SqlQuery = @"Declare @AppoinmentId uniqueidentifier
                                set @AppoinmentId = '{0}'
 
                                delete cae from CustomerAppointmentEquipment cae
                                LEFT JOIN equipment eqp on eqp.EquipmentId=cae.EquipmentId
                                where cae.AppointmentId = @AppoinmentId and eqp.EquipmentClassId=2";
            SqlQuery = string.Format(SqlQuery, TicketId);
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
        public bool DeleteAllCustomerAppointmentServiceByAppointmentId(Guid AppointmentId)
        {
            string SqlQuery = @"Declare @AppoinmentId uniqueidentifier
                                set @AppoinmentId = '{0}'

                                delete from CustomerAppointmentEquipment 
                                where AppointmentId = @AppoinmentId and EquipmentId 
                                in ( select EquipmentId from Equipment where EquipmentId 
                                in  ( select EquipmentId from CustomerAppointmentEquipment 
                                where AppointmentId = @AppoinmentId ) and EquipmentClassId = 2)";
            SqlQuery = string.Format(SqlQuery, AppointmentId);
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

        public DataTable GetAllLeadInstalledEquipmentsByAppointmentId(Guid AppointmentId)
        {
            string sqlQuery = @"select 
	                                _CustomerAppointmentEquipment.*,
                                    _Equipment.Name as EquipmentServiceName,
	                                _Equipment.Retail as EquipmentOldPrice
                                from CustomerAppointmentEquipment _CustomerAppointmentEquipment
	                                left join Equipment _Equipment
	                                on _CustomerAppointmentEquipment.EquipmentId = _Equipment.EquipmentId
                                where _CustomerAppointmentEquipment.AppointmentId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, AppointmentId);
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
        public bool UpdateSoldByCustomerAppointmentEqp(Guid AppointmentId, string CreatedByUid)
        {
            string sqlQuery = @"update CustomerAppointmentEquipment set CreatedByUid='{1}'
                                where AppointmentId='{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, AppointmentId, CreatedByUid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    ExecuteCommand(cmd);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }
        public bool UpdateInstalledByCustomerAppointmentEqp(Guid AppointmentId, Guid InstalledByUid)
        {
            string sqlQuery = @"update CustomerAppointmentEquipment set InstalledByUid='{1}'
                                where AppointmentId='{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, AppointmentId, InstalledByUid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    ExecuteCommand(cmd);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }
        public DataTable GetAllTicketEquipmentsDetailByCustomerIdAndCompanyId(Guid companyid, Guid customerid)
        {
            string sqlQuery = @"select 
                                        cae.EquipmentId
                                        ,cae.EquipName
                                        ,Sum(cae.QuantityLeftEquipment) as QuantityLeftEquipment
                                
                                from CustomerAppointmentEquipment cae

                                left join Ticket tik on tik.TicketId = cae.AppointmentId
                                left join Customer cus on cus.CustomerId = tik.CustomerId
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                where cus.CustomerId = '{1}'
                                and cc.CompanyId = '{0}'
                                and cc.IsLead = 0
                                and IsService = 0
                                and cae.QuantityLeftEquipment != 0
                                and cae.IsEquipmentRelease=1

								group by cae.EquipmentId,cae.EquipName";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, customerid);
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

        public DataTable GetAllTicketServicesDetailByCustomerIdAndCompanyId(Guid companyid, Guid customerid)
        {
            string sqlQuery = @"select 
                                        cae.EquipmentId
                                        ,cae.EquipName
                                        ,cae.TotalPrice
                                
                                from CustomerAppointmentEquipment cae

                                left join Ticket tik on tik.TicketId = cae.AppointmentId
                                left join Customer cus on cus.CustomerId = tik.CustomerId
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                left join Equipment eqp on eqp.EquipmentId=cae.EquipmentId
                                where cus.CustomerId = '{1}'
                                and cc.CompanyId = '{0}'
                                and cc.IsLead = 0
                                and cae.IsService = 1
								and eqp.IsArbEnabled=1

								group by cae.EquipmentId,cae.EquipName,cae.TotalPrice";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, customerid);
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

        public DataTable GetTicketEquipmentsDetailByCustomerIdAndCompanyIdAndEqpId(Guid companyid, Guid customerid, Guid EqpId)
        {
            string sqlQuery = @"select cae.*, tik.Id as TicketIntId from CustomerAppointmentEquipment cae
                                left join Ticket tik on tik.TicketId = cae.AppointmentId
                                left join Customer cus on cus.CustomerId = tik.CustomerId
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                where cus.CustomerId = '{1}'
                                and cc.CompanyId = '{0}'
                                and cae.EquipmentId = '{2}'
                                and cc.IsLead = 0
                                and IsService = 0
                                and cae.QuantityLeftEquipment != 0
                                and cae.IsEquipmentRelease=1";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, customerid, EqpId);
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

        public DataTable GetAllLeadInstalledEquipmentsByAppointmentIdCustomerPackageEqp(Guid CustomerId, Guid CompanyId)
        {
            string sqlQuery = @"select cpe.*
                                ,eq.Name as EquipmentServiceName
                                ,eq.SKU as SKU
                                ,eq.Point as Point   
                                from CustomerPackageEqp cpe
                                LEFT JOIN Equipment eq on eq.EquipmentId=cpe.EquipmentId
                                where cpe.CustomerId='{0}' and cpe.CompanyId='{1}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId, CompanyId);
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
        public DataTable GetCustomerPackageEqpAPIById(int id)
        {
            string sqlQuery = @"SELECT cpe.* 
                        FROM CustomerPackageEqp cpe
                        WHERE cpe.AppointmentEquipmentIntId = @Id";

            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                // Handle the exception or log it
                return null;
            }
        }
        public DataTable GetCustomerPackageServiceAPIById(int id)
        {
            string sqlQuery = @"SELECT cpe.* 
                        FROM CustomerPackageService cpe
                        WHERE cpe.AppointmentEquipmentIntId = @Id";

            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                // Handle the exception or log it
                return null;
            }
        }

        public DataTable GetAllLeadInstalledServiceByAppointmentIdCustomerPackageEqp(Guid CustomerId, Guid CompanyId)
        {
            string sqlQuery = @"select cpe.*,
                                eq.Name as EquipmentServiceName,
                                eq.IsARBEnabled,
                                ISNULL(manu.[Name],'') as Manufacturer,
                                ISNULL(loc.[Name],'') as [Location],
                                ISNULL(typ.[Name],'') as [Type],
                                ISNULL(model.[Name],'') as Model,
                                ISNULL(finish.[Name],'') as Finish,
                                ISNULL(capacity.[Name],'') as Capacity
                                from CustomerPackageService cpe
                                LEFT JOIN Equipment eq on eq.EquipmentId=cpe.EquipmentId

                                left join Manufacturer manu on cpe.ManufacturerId = manu.ManufacturerId
                                left join ServiceDetailInfo loc on loc.ServiceInfoId = cpe.LocationId 
                                    and loc.[Type] = 'Location' 
                                    and cpe.EquipmentId = loc.ServiceId
                                left join ServiceDetailInfo typ on typ.ServiceInfoId = cpe.TypeId 
                                    and typ.[Type] = 'Type' 
                                    and cpe.EquipmentId = typ.ServiceId
                                left join ServiceDetailInfo model on model.ServiceInfoId = cpe.ModelId 
                                    and model.[Type] = 'Model' 
                                    and cpe.EquipmentId = model.ServiceId
                                left join ServiceDetailInfo finish on finish.ServiceInfoId = cpe.FinishId 
                                    and finish.[Type] = 'Finish' 
                                    and cpe.EquipmentId = finish.ServiceId
                                left join ServiceDetailInfo capacity on capacity.ServiceInfoId = cpe.CapacityId 
                                    and capacity.[Type] = 'Capacity' 
                                    and cpe.EquipmentId = capacity.ServiceId

                                where cpe.CustomerId='{0}' and cpe.CompanyId='{1}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId, CompanyId);
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

        public DataTable GetAllLeadEquipmentDetailByLeadIdandCompanyId(Guid companyId, int id)
        {
            string sqlQuery = @"select eq.Name as LeadEquipmentName, cae.Quantity as LeadEquipmentQuantity, cae.TotalPrice as LeadEquipmentPrice
                                from CustomerAppointmentEquipment cae
                                join CustomerAppointment ca
                                on cae.AppointmentId = ca.AppointmentId
                                join customer cs
                                on cs.CustomerId = ca.CustomerId
                                join CustomerCompany cc
                                on cc.CustomerId = cs.CustomerId
                                join Equipment eq
                                on eq.EquipmentId = cae.EquipmentId
                                where cc.CompanyId = '{0}'
                                and cc.IsLead = 1
                                and cs.Id = {1}";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, id);
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

        public DataTable GetAllCustomerAppointmentEquipmentByTicketId(Guid companyId, Guid TicketId)
        {
            string sqlQuery = @"select
                                cae.*,
								eqp.EquipmentClassId,
								eqp.IsARBEnabled
                                from CustomerAppointmentEquipment cae
                                LEFT JOIN Equipment eqp on eqp.EquipmentId=cae.EquipmentId
                                LEFT JOIN CustomerAppointment ca on ca.AppointmentId=cae.AppointmentId
                                LEFT JOIN Ticket t on t.TicketId=ca.AppointmentId
                                where t.TicketId='{0}'
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, TicketId);
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

        public DataTable GetBillingCheckCustomerAppointmentEquipmentByEquipmentIdAndCustomerId(Guid customerid, Guid equipmentid)
        {
            string sqlQuery = @"select ISNULL(cae.IsBilling, 0) as IsBillingCheck, cae.EquipmentId as BillEquipmentId from CustomerAppointmentEquipment cae
                                left join Ticket tik on tik.TicketId = cae.AppointmentId
                                where cae.EquipmentId = '{1}'
                                and tik.CustomerId = '{0}'
                                and cae.IsBilling = 1";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerid, equipmentid);
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
        public DataTable GetCustomerTicketServiceByTicketId(Guid ticketId)
        {
            string sqlQuery = @"select SUM(cae.TotalPrice) as ServiceFee 
                                From CustomerAppointmentEquipment cae
								LEFT JOIN Equipment eqp on eqp.EquipmentId=cae.EquipmentId
                                where cae.AppointmentId='{0}' and cae.IsService=1 and eqp.IsARBEnabled=1";
            try
            {
                sqlQuery = string.Format(sqlQuery, ticketId);
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
        public DataTable GetCustomerServiceListByCustomerIdAndCompanyId(Guid customerid, Guid companyid)
        {
            string sqlQuery = @"select cae.* from CustomerAppointmentEquipment cae
                                left join Ticket ticket on ticket.TicketId = cae.AppointmentId
                                left join Customer customer on customer.CustomerId = ticket.CustomerId
                                where customer.CustomerId = '{0}'
                                and ticket.CompanyId = '{1}'
                                and cae.IsService = 1
                                and cae.IsBilling = 1
                                and ticket.[Status] = 'Completed'";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerid, companyid);
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

        public DataTable GetCustomerEquipmentListByCustomerIdAndCompanyId(Guid customerid, Guid companyid)
        {
            string sqlQuery = @"select cae.* from CustomerAppointmentEquipment cae
                                left join Ticket ticket on ticket.TicketId = cae.AppointmentId
                                left join Customer customer on customer.CustomerId = ticket.CustomerId
                                where customer.CustomerId = '{0}'
                                and ticket.CompanyId = '{1}'
                                and cae.IsService = 0
                                and cae.IsEquipmentRelease = 1";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerid, companyid);
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
        public DataTable GetEquipmentTypeByAppointmentIdAndEquipmentId(Guid AppointmentId, Guid EquipmentId)
        {
            string sqlQuery = @"select _et3.Name, _eq3.SKU
                                from  CustomerAppointmentEquipment _cae3
                                left join Equipment _eq3 on _eq3.EquipmentId = _cae3.EquipmentId
                                left join EquipmentType _et3 on _et3.Id = _eq3.EquipmentTypeId

                                where _cae3.EquipmentId = '{1}'
                                and _cae3.AppointmentId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, AppointmentId, EquipmentId);
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

        public DataTable GetManufacturerByAppointmentIdAndEquipmentId(Guid AppointmentId, Guid EquipmentId)
        {
            string sqlQuery = @"select manu3.Name
                                from  CustomerAppointmentEquipment _cae3
                                left join Equipment _eq3 on _eq3.EquipmentId = _cae3.EquipmentId
                                left join Manufacturer manu3 on manu3.Id = _eq3.ManufacturerId

                                where _cae3.EquipmentId = '{1}'
                                and _cae3.AppointmentId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, AppointmentId, EquipmentId);
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

        public DataTable GetInstallerTypeByAppointmentIdAndEquipmentId(Guid AppointmentId, Guid EquipmentId)
        {
            string sqlQuery = @"select _cae3.InstalledByUid, emp3.Firstname +' '+ emp3.LastName as Name
                                from  CustomerAppointmentEquipment _cae3
                                left join Employee emp3 on emp3.UserId = _cae3.InstalledByUid

                                where _cae3.EquipmentId = '{1}'
                                and _cae3.AppointmentId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, AppointmentId, EquipmentId);
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

        public DataTable GetCompanyCostByEquipmentId(Guid AppointmentId, Guid EquipmentId)
        {
            string sqlQuery = @"select EV.Cost
                                from  CustomerAppointmentEquipment _cae3
                                left join Equipment _eq3 on _eq3.EquipmentId = _cae3.EquipmentId
                                left join EquipmentVendor EV on EV.EquipmentId = _eq3.EquipmentId
                                where _cae3.EquipmentId = '{1}'
                                and _cae3.AppointmentId = '{0}'
                                and EV.IsPrimary = 1";
            try
            {
                sqlQuery = string.Format(sqlQuery, AppointmentId, EquipmentId);
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

        public DataTable GetEquipmentSKUAndPointByAppointmentIdAndEquipmentId(Guid AppointmentId, Guid EquipmentId)
        {
            string sqlQuery = @"select _eq3.SKU, _eq3.Point
                                from  CustomerAppointmentEquipment _cae3
                                left join Equipment _eq3 on _eq3.EquipmentId = _cae3.EquipmentId

                                where _cae3.EquipmentId = '{1}'
                                and _cae3.AppointmentId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, AppointmentId, EquipmentId);
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

        public DataTable GetCustomerByAppointmentId(Guid AppointmentId)
        {
            string sqlQuery = @"select Cus.FirstName + ' ' + Cus.LastName as Name, Cus.Id
                                from Ticket tk
                                left join customer Cus on Cus.CustomerId = tk.CustomerId
                                where tk.TicketId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, AppointmentId);
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

        public DataTable GetTicketTypeByAppointmentIdAndEquipmentId(Guid AppointmentId, Guid EquipmentId)
        {
            string sqlQuery = @"select lktype3.DisplayText as TicketType
                                from  CustomerAppointmentEquipment _cae3
                                left join ticket tk3 on _cae3.AppointmentId = tk3.TicketId
                                left join Lookup lktype3 on  lktype3.DataKey ='TicketType'
                                and lktype3.DataValue = tk3.TicketType

                                where _cae3.EquipmentId = '{1}'
                                and _cae3.AppointmentId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, AppointmentId, EquipmentId);
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

        public DataTable GetAttachmentsCountByAppointmentId(Guid AppointmentId)
        {
            string sqlQuery = @"select count(id) as TotalCount from TicketFile where TicketId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, AppointmentId);
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

        public DataTable GetRepliesCountByAppointmentId(Guid AppointmentId)
        {
            string sqlQuery = @"select
                                (select count(id) from TicketFile where TicketId = '{0}') +
	                                (select count(id) from TicketReply where TicketId = '{0}') as Total";
            try
            {
                sqlQuery = string.Format(sqlQuery, AppointmentId);
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
    }
}
