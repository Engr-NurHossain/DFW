using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;
using NLog;
using NLog.Filters;
using static HS.Entities.Custom.NMCTestAccountResponse;

namespace HS.DataAccess
{
    public partial class InvoiceDataAccess
    {
        //private Client _Client;
        //protected Logger logger = LogManager.GetCurrentClassLogger();

        public InvoiceDataAccess() { }
        public InvoiceDataAccess(string ConnectionString) : base(ConnectionString) { }

        public DataTable GetDashboardSalesAreaChartData(Guid CompanyId, DateTime Start, DateTime End, string labelvalue, string tag, Guid empid)
        {
            string sqlQuery = @"select 
	                                ISNULL(sum(inv.TotalAmount),0) as TotalSaleAmount,
	                                dateadd(DAY,0, datediff(day,0, inv.CreatedDate))as SaleDate , 
	                                --ISNULL( sum(ind.Quantity),0) as SaleQuantity
                                    0 as SaleQuantity
                                from Invoice inv
	                                --left join InvoiceDetail ind
	                                --on inv.InvoiceId = ind.InvoiceId
                                where inv.CompanyId = '{0}'
                                    {1}
                                    {2}
                                group by dateadd(DAY,0, datediff(day,0, inv.CreatedDate))
                                order by dateadd(DAY,0, datediff(day,0, inv.CreatedDate)) asc
                                ";


            string DateRange = "";
            string IsSales = "";
            if (Start != null && End != null)
            {
                DateRange = string.Format("and inv.CreatedDate between '{0}' and '{1}'", Start, End);
            }
            if (!string.IsNullOrWhiteSpace(tag) && tag.ToLower().IndexOf("admin") == -1)
            {
                IsSales = string.Format("and (inv.LastUpdatedByUid = '{0}' or inv.CreatedByUid = '{0}')", empid);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, CompanyId, DateRange, IsSales);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, sqlQuery);
                return null;
            }
        }


        public bool ReseedInvoiceTable()
        {
            string SqlQuery = @"Delete from InvoiceDetail 
                                Delete from Invoice
                                DBCC CHECKIDENT('InvoiceDetail', RESEED, 0) 
                                DBCC CHECKIDENT('Invoice', RESEED, 0) 
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
                logger.Error(ex, SqlQuery);
                return false;
            }
            return true;
        }
        public DataTable GetEqupmentListBySearchKeyAndCompanyId(string key, Guid companyId, int MaxLoad, string ExistEquipment)
        {
            string sqlQuery = @"
                                --Declare @SerchText nvarchar(100)
                                --set @SerchText = '{0}%'
                                select
                                Top {2}
                                eq.EquipmentId
                                ,eq.EquipmentClassId
		                        ,eq.Name as EquipmentName
                                ,eq.IsTaxable
		                        ,eq.Retail as RetailPrice
                                ,eq.SKU
                                ,eq.Barcode
		                        ,eq.Reorderpoint
                                ,eq.EquipmentTypeId 
                                ,eq.SupplierId 
		                        ,(select sum(Quantity) from Inventory where EquipmentId = eq.EquipmentId)  
                                    as QuantityAvailable
		                        ,et.Name as EquipmentType
                                ,eq.Comments as EquipmentDescription
                                ,eq.Id
                                ,(case WHEN eq.SupplierCost IS NULL
								then
								    eq.Retail 
								else  
									eq.SupplierCost
								end) as SupplierCost
                                ,mf.Name as ManufacturerName 
				                ,ISNULL((select top(1) ev.Cost from EquipmentVendor ev  where ev.EquipmentId = eq.EquipmentId and IsPrimary = 1),0) as EquipmentVendorCost

                                from Equipment eq
                                left join Manufacturer mf
								on mf.Id=eq.ManufacturerId
                                left join EquipmentType et
                                on et.Id= eq.EquipmentTypeId
                                where (eq.Name like @SerchText OR eq.SKU like @SerchText) 
                                and eq.CompanyId ='{1}' and eq.IsActive = 1 
                                {3}";
            var EqpExist = "";
            if (!string.IsNullOrEmpty(ExistEquipment))
            {
                EqpExist = string.Format("AND eq.EquipmentId not in {0}", ExistEquipment);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, key.Replace("'", "'"), companyId, MaxLoad, EqpExist);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    cmd.Parameters.Add(new SqlParameter("SerchText", "%" + key + "%"));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, sqlQuery);
                return null;
            }
        }
        public DataTable GetEqupmentListBySearchKeyAndCompanyIdBarcode(string code, Guid companyId, int MaxLoad, string ExistEquipment)
        {
            string sqlQuery = @"
                                --Declare @SerchText nvarchar(100)
                                --set @SerchText = '{0}%'
                                select
                                Top {2}
                                eq.EquipmentId
                                ,eq.EquipmentClassId
		                        ,eq.Name as EquipmentName
                                ,eq.IsTaxable
		                        ,eq.Retail as RetailPrice
                                ,eq.SKU
                                ,eq.Barcode
		                        ,eq.Reorderpoint
                                ,eq.EquipmentTypeId 
                                ,eq.SupplierId 
		                        ,(select sum(Quantity) from Inventory where EquipmentId = eq.EquipmentId)  
                                    as QuantityAvailable
		                        ,et.Name as EquipmentType
                                ,eq.Comments as EquipmentDescription
                                ,eq.Id
                                ,(case WHEN eq.SupplierCost IS NULL
								then
								    eq.Retail 
								else  
									eq.SupplierCost
								end) as SupplierCost
                                ,mf.Name as ManufacturerName 
				                ,ISNULL((select top(1) ev.Cost from EquipmentVendor ev  where ev.EquipmentId = eq.EquipmentId and IsPrimary = 1),0) as EquipmentVendorCost

                                from Equipment eq
                                left join Manufacturer mf
								on mf.Id=eq.ManufacturerId
                                left join EquipmentType et
                                on et.Id= eq.EquipmentTypeId
                                  where eq.Barcode = @SerchText 
                                and eq.CompanyId ='{1}' and eq.IsActive = 1 
                                {3}";
            var EqpExist = "";
            if (!string.IsNullOrEmpty(ExistEquipment))
            {
                EqpExist = string.Format("AND eq.EquipmentId not in {0}", ExistEquipment);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, code.Replace("'", "'"), companyId, MaxLoad, EqpExist);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                   
                    cmd.Parameters.Add(new SqlParameter("@SerchText", code));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, sqlQuery);
                return null;
            }
        }

        public DataTable GetEquipmentListBySearchKeyAPI(string key, int MaxLoad, string ExistEquipment)
        {
            string sqlQuery = @"
                                Declare @SerchText nvarchar(100)
                                set @SerchText = '%{0}%'
                                select
                                Top {1}
                                eq.EquipmentId
		                        ,eq.Name as EquipmentName
		                        ,eq.Retail as RetailPrice
                                ,eq.SKU
		                        ,eq.Reorderpoint
		                        ,(select sum(Quantity) from Inventory where EquipmentId = eq.EquipmentId)  
                                    as QuantityAvailable
		                        ,et.Name as EquipmentType
                                ,eq.Comments as EquipmentDescription
                                ,eq.Id
                                ,(case WHEN eq.SupplierCost IS NULL
								then
								    eq.Retail 
								else  
									eq.SupplierCost
								end) as SupplierCost
                                ,mf.Name as ManufacturerName 
                                from Equipment eq
                                left join Manufacturer mf
								on mf.Id=eq.ManufacturerId
                                left join EquipmentType et
                                on et.Id= eq.EquipmentTypeId
                                where (eq.Name like @SerchText)";
            var EqpExist = "";
            if (!string.IsNullOrEmpty(ExistEquipment))
            {
                EqpExist = string.Format("AND eq.EquipmentId not in {2}", ExistEquipment);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, key, MaxLoad, EqpExist);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, sqlQuery);
                return null;
            }
        }



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
                                -(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=eq.EquipmentId and Type='Release'  And invinner.TechnicianId='{3}')
                                -(select isnull(sum(Quantity),0) from dbo.AssignedInventoryTechReceived b where b.EquipmentId  = eq.EquipmentId and b.TechnicianId  = '{3}' and b.IsApprove = 0 and b.IsDecline = 0)
                                ) QuantityOnHand
                                ,((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner 
                                 where invinner.EquipmentId=eq.EquipmentId and Type='Add')
                                 - (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 
                                 where invinner2.EquipmentId=eq.EquipmentId and Type='Release')) as WarehouseQTY
                                ,mf.Name as ManufacturerName
                                ,eq.Point
                               ,ISNULL((select top(1) ev.Cost from EquipmentVendor ev  where ev.EquipmentId = eq.EquipmentId and IsPrimary = 1),0) as EquipmentVendorCost

                                from Equipment eq
								LEFT JOIN InventoryTech itech on itech.EquipmentId=eq.EquipmentId
                                left join EquipmentType et
                                on et.Id= eq.EquipmentTypeId
                                left join Manufacturer mf
								on mf.Id=eq.ManufacturerId
                                where eq.EquipmentClassId=1 
                                and (eq.Name like @SerchText OR eq.SKU like @SerchText OR eq.BarCode like @SerchText)  
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
        public DataTable GetOnlyEqupmentListBySearchKeyAndCompanyId(string key, Guid companyId, int MaxLoad, string ExistEquipment)
        {
            string sqlQuery = @" 
                                Declare @SerchText nvarchar(100)
                                set @SerchText = '%{0}%'
                                select
                                Top {2}
                                eq.EquipmentId
		                        ,eq.Name as EquipmentName
		                        ,round(eq.Retail,2) as RetailPrice
		                        ,eq.Reorderpoint
                                ,iif((EM.SKU is null OR EM.SKU = '') AND (EM.Variation is null OR EM.Variation = ''), eq.SKU, iif(EM.SKU is null OR EM.SKU = '', EM.Variation, EM.SKU +' - '+ EM.Variation)) as SKU
		                        ,(select sum(Quantity) from Inventory where EquipmentId = eq.EquipmentId)  
                                    as QuantityAvailable
		                        ,et.Name as EquipmentType
                                ,eq.Comments as EquipmentDescription
                                ,eq.Id
                                ,Round(ISNULL(ISNULL(ISNULL((select top(1) Cost from EquipmentVendor where IsPrimary =1 and equipmentid = eq.equipmentid) ,eq.SupplierCost),eq.Retail),0),2) as SupplierCost
                                ,mf.Name as ManufacturerName
                                --,iif(EM.SKU is null OR EM.SKU = '', EM.Variation, EM.SKU +' - '+ EM.Variation) as ManufacturerDetail
                                from Equipment eq
                                left join EquipmentType et
                                on et.Id= eq.EquipmentTypeId
                                left join Manufacturer mf
								on mf.Id=eq.ManufacturerId
                                left join EquipmentManufacturer EM
                                on EM.EquipmentId = eq.EquipmentId
                                where eq.EquipmentClassId=1 
                                and eq.IsActive = 1 
                                and (eq.Name like @SerchText or eq.SKU like @SerchText) 
                                and eq.CompanyId ='{1}'  {3}
                                order by eq.Name asc";
            var EqpExist = "";
            if (!string.IsNullOrEmpty(ExistEquipment))
            {
                EqpExist = string.Format("AND eq.EquipmentId not in {0}", ExistEquipment);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, key.Replace("'", "''"), companyId, MaxLoad, EqpExist);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, sqlQuery);
                return null;
            }
        }
        public DataTable GetEqupmentListByTypeAndSearchKeyAndCompanyId(string key, Guid companyId, int MaxLoad, string ExistEquipment, string EqpTypeId)
        {
            string sqlQuery = @" 
                                Declare @SerchText nvarchar(100)
                                set @SerchText = '%{0}%'
                                select
                                Top {2}
                                eq.EquipmentId
		                        ,eq.Name as EquipmentName
		                        ,round(eq.Retail,2) as RetailPrice
		                        ,eq.Reorderpoint
                                ,eq.SKU
		                        ,(select sum(Quantity) from Inventory where EquipmentId = eq.EquipmentId)  
                                    as QuantityAvailable
		                        ,et.Name as EquipmentType
                                ,eq.Comments as EquipmentDescription
                                ,eq.Id
                                ,Round(ISNULL(ISNULL(ISNULL((select top(1) Cost from EquipmentVendor where IsPrimary =1 and equipmentid = eq.equipmentid) ,eq.SupplierCost),eq.Retail),0),2) as SupplierCost
                                ,mf.Name as ManufacturerName
                                from Equipment eq
                                left join EquipmentType et
                                on et.Id= eq.EquipmentTypeId
                                left join Manufacturer mf
								on mf.Id=eq.ManufacturerId
                                where eq.IsActive = 1  
                                and eq.EquipmentTypeId = '{4}'
                                and (eq.Name like @SerchText or eq.SKU like @SerchText) 
                                and eq.CompanyId ='{1}' 
                                 {3}
                                order by eq.Name asc";
            var EqpExist = "";
            if (!string.IsNullOrEmpty(ExistEquipment))
            {
                EqpExist = string.Format("AND eq.EquipmentId not in {0}", ExistEquipment);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, key.Replace("'", "''"), companyId, MaxLoad, EqpExist, EqpTypeId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, sqlQuery);
                return null;
            }
        }
        public DataTable GetOnlyEqupmentListByTypeAndSearchKeyAndCompanyId(string key, Guid companyId, int MaxLoad, string ExistEquipment, string EqpTypeId)
        {
            string sqlQuery = @" 
                                Declare @SerchText nvarchar(100)
                                set @SerchText = '%{0}%'
                                select
                                Top {2}
                                eq.EquipmentId
		                        ,eq.Name as EquipmentName
		                        ,round(eq.Retail,2) as RetailPrice
		                        ,eq.Reorderpoint
                                ,eq.SKU
		                        ,(select sum(Quantity) from Inventory where EquipmentId = eq.EquipmentId)  
                                    as QuantityAvailable
		                        ,et.Name as EquipmentType
                                ,eq.Comments as EquipmentDescription
                                ,eq.Id
                                ,Round(ISNULL(ISNULL(ISNULL((select top(1) Cost from EquipmentVendor where IsPrimary =1 and equipmentid = eq.equipmentid) ,eq.SupplierCost),eq.Retail),0),2) as SupplierCost
                                ,mf.Name as ManufacturerName
                                from Equipment eq
                                left join EquipmentType et
                                on et.Id= eq.EquipmentTypeId
                                left join Manufacturer mf
								on mf.Id=eq.ManufacturerId
                                where eq.EquipmentClassId=1 
                                and eq.IsActive = 1  
                                and eq.EquipmentTypeId = '{4}'
                                and (eq.Name like @SerchText or eq.SKU like @SerchText) 
                                and eq.CompanyId ='{1}' 
                                 {3}
                                order by eq.Name asc";
            var EqpExist = "";
            if (!string.IsNullOrEmpty(ExistEquipment))
            {
                EqpExist = string.Format("AND eq.EquipmentId not in {0}", ExistEquipment);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, key.Replace("'", "''"), companyId, MaxLoad, EqpExist, EqpTypeId);
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
        public DataTable GetOnlyServiceListBySearchKeyAndCompanyId(string key, Guid companyId, int MaxLoad, string ExistEquipment, string ServiceEquipment)
        {
            string sqlQuery = @" select
                                Top {2}
                                eq.EquipmentId
		                        ,eq.Name as EquipmentName
		                        ,eq.Retail as RetailPrice
		                        ,eq.Reorderpoint
		                        ,(select sum(Quantity) from Inventory where EquipmentId = eq.EquipmentId)  
                                    as QuantityAvailable
		                        ,et.Name as EquipmentType
                                ,eq.Comments as EquipmentDescription
                                ,eq.Id
                                ,eq.IsARBEnabled
                                ,ISNULL(eq.IsTaxable,0) as IsTaxable
                                ,(case WHEN eq.SupplierCost IS NULL
								then
								    eq.Retail 
								else  
									eq.SupplierCost
								end) as SupplierCost,
                                mf.Name as ManufacturerName
                                from Equipment eq
                                left join EquipmentType et
                                on et.Id= eq.EquipmentTypeId
                                LEFT JOIN Manufacturer mf on mf.Id=eq.ManufacturerId
                                where eq.EquipmentClassId=2 and eq.Name like '%{0}%' and eq.CompanyId ='{1}'  {3} {4}
                                order by eq.Name asc";
            try
            {
                var EqpExist = "";
                var Eqpservice = "";
                if (!string.IsNullOrEmpty(ExistEquipment))
                {
                    EqpExist = string.Format("AND eq.EquipmentId not in {0}", ExistEquipment);
                }
                if (!string.IsNullOrEmpty(ServiceEquipment) && ServiceEquipment == "Service")
                {
                    Eqpservice = string.Format("AND eq.IsARBEnabled = 1");
                }
                else {
                    Eqpservice = string.Format("AND eq.IsARBEnabled = 0");
                }
                sqlQuery = string.Format(sqlQuery, key, companyId, MaxLoad, EqpExist, Eqpservice);
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
        public DataTable GetOnlyRMRServiceListBySearchKeyAndCompanyId(string key, Guid companyId, int MaxLoad, string ExistEquipment)
        {
            string sqlQuery = @" select
                                Top {2}
                                eq.EquipmentId
		                        ,eq.Name as EquipmentName
		                        ,eq.Retail as RetailPrice
		                        ,eq.Reorderpoint
		                        ,(select sum(Quantity) from Inventory where EquipmentId = eq.EquipmentId)  
                                    as QuantityAvailable
		                        ,et.Name as EquipmentType
                                ,eq.Comments as EquipmentDescription
                                ,eq.Id
                                ,ISNULL(eq.IsTaxable,0) as IsTaxable
                                ,(case WHEN eq.SupplierCost IS NULL
								then
								    eq.Retail 
								else  
									eq.SupplierCost
								end) as SupplierCost,
                                mf.Name as ManufacturerName
                                from Equipment eq
                                left join EquipmentType et
                                on et.Id= eq.EquipmentTypeId
                                LEFT JOIN Manufacturer mf on mf.Id=eq.ManufacturerId
                                where eq.EquipmentClassId=2 and eq.isARBEnabled = 1 and eq.Name like '%{0}%' and eq.CompanyId ='{1}'  {3}
                                order by eq.Name asc";
            try
            {
                var EqpExist = "";
                if (!string.IsNullOrEmpty(ExistEquipment))
                {
                    EqpExist = string.Format("AND eq.EquipmentId not in {0}", ExistEquipment);
                }
                sqlQuery = string.Format(sqlQuery, key, companyId, MaxLoad, EqpExist);
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
        public DataTable GetOnlyServiceListByTypeAndSearchKeyAndCompanyId(string key, Guid companyId, int MaxLoad, string ExistEquipment, string EqpTypeId)
        {
            string sqlQuery = @" select
                                Top {2}
                                eq.EquipmentId
		                        ,eq.Name as EquipmentName
		                        ,eq.Retail as RetailPrice
		                        ,eq.Reorderpoint
		                        ,(select sum(Quantity) from Inventory where EquipmentId = eq.EquipmentId)  
                                    as QuantityAvailable
		                        ,et.Name as EquipmentType
                                ,eq.Comments as EquipmentDescription
                                ,eq.Id
                                ,ISNULL(eq.IsTaxable,0) as IsTaxable
                                ,(case WHEN eq.SupplierCost IS NULL
								then
								    eq.Retail 
								else  
									eq.SupplierCost
								end) as SupplierCost,
                                mf.Name as ManufacturerName
                                from Equipment eq
                                left join EquipmentType et
                                on et.Id= eq.EquipmentTypeId
                                LEFT JOIN Manufacturer mf on mf.Id=eq.ManufacturerId
                                where eq.EquipmentClassId=2 and eq.EquipmentTypeId = '{4}' and eq.Name like '%{0}%' and eq.CompanyId ='{1}'  {3}
                                order by eq.Name asc";
            try
            {
                var EqpExist = "";
                if (!string.IsNullOrEmpty(ExistEquipment))
                {
                    EqpExist = string.Format("AND eq.EquipmentId not in {0}", ExistEquipment);
                }
                sqlQuery = string.Format(sqlQuery, key, companyId, MaxLoad, EqpExist, EqpTypeId);
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
        public DataTable GetPaymentHistoryByInvoiceId(int InvoiceId)
        {
            string sqlQuery = @"select
                                th.Amout as Amount,
                                pi.CardNumber as numberCard,
                                t.CheckNo as chkno,
                                t.PaymentMethod as method
                                into #TransactionHistory from TransactionHistory th
                                LEFT JOIN [Transaction] t on t.Id=th.TransactionId
                                LEFT JOIN PaymentInfo pi on pi.Id=t.PaymentInfoId
                                where th.InvoiceId={0}

								(select * from #TransactionHistory group by Amount,numberCard,chkno,method)

								drop table #TransactionHistory";
            try
            {
                sqlQuery = string.Format(sqlQuery, InvoiceId);
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

        #region InvoiceList
        public DataTable GetAllInvoiceByCompanyIdAndCustomerId(Guid companyid, Guid customerid, string InvoiceType, CustomerFilter filter, bool IsDeclined)
        {

            string paidSql = "";
            string declinedSql = "";

            string subquery = "";
            string subquery1 = "";
            string searchText = "";
            var strStartDate = "";
            var strEndDate = "";
            var dateRange = "";
            string unpaidquery = "";
            string declinequery = "";
            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                searchText = string.Format("and inv.InvoiceId like '%{0}%'", filter.SearchText);
            }
            string sqlQuery = @"declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)* @pagesize 
                                set @pageend = @pagesize
                                select inv.*,
								(select top 1 AddedDate from InvoiceNote where InvoiceId=inv.Id order by  addedDate desc) InvoiceNoteAddedDate,
                                (select top 1 Note from InvoiceNote where InvoiceId=inv.Id order by  addedDate desc) NotesInvoice,
								(select top 1 added.FirstName + ' ' + added.LastName from InvoiceNote left join Employee added on added.UserId = AddedBy where InvoiceId=inv.Id order by  addedDate desc) NoteInvoiceAddedBy,
                                (select top 1 AddedDate from CustomerAgreement where InvoiceId = inv.InvoiceId and CompanyId = '{0}' order by id desc) as CustomerViewedTime,
                                (select top 1 Type from CustomerAgreement where InvoiceId = inv.InvoiceId and CompanyId = '{0}' order by id desc) as CustomerViewedType,
                                (select top 1 EquipDetail from InvoiceDetail where InvoiceId = inv.InvoiceId order by EquipDetail) InvoiceEquipDes,
                                cus.EmailAddress as CustomerMailAddress,
                                emp.FirstName + ' ' + emp.LastName as UserNum,
                                iif(inv.[Status] != 'Paid', iif(datediff(day, convert(date, inv.DueDate), convert(date, getdate())) < 0, 0, datediff(day, convert(date, inv.DueDate), convert(date, getdate()))), 0) as AgingDate
                                 INTO #CustomerInvoice
                                 from Invoice inv
                                 join customer cus
								 on cus.CustomerId = inv.CustomerId
                                left join Employee emp on emp.UserId = inv.CreatedByUid
                                 where inv.CompanyId = '{0}'
                                {6}{7}
                                 and inv.CustomerId = '{1}'
								 and inv.IsEstimate = 0
								 and inv.Status != 'init' {2} {3} {8} {9}
                                order by inv.CreatedDate desc
                                
								SELECT TOP (@pagesize)
                                  *
                                FROM #CustomerInvoice
                                where Id NOT IN(Select TOP (@pagestart) Id from #CustomerInvoice {4})
                                {5}
								drop table #CustomerInvoice
                                ";

            #region Order
            if (!string.IsNullOrWhiteSpace(filter.order))
            {
                if (filter.order == "ascending/createdon")
                {
                    subquery = "order by [CreatedDate] asc";
                    subquery1 = "order by [CreatedDate] asc";
                }
                else if (filter.order == "descending/createdon")
                {
                    subquery = "order by [CreatedDate] desc";
                    subquery1 = "order by [CreatedDate] desc";
                }
                else if (filter.order == "ascending/invoiceno")
                {
                    subquery = "order by [InvoiceId] asc";
                    subquery1 = "order by [InvoiceId] asc";
                }
                else if (filter.order == "descending/invoiceno")
                {
                    subquery = "order by [InvoiceId] desc";
                    subquery1 = "order by [InvoiceId] desc";
                }
                else if (filter.order == "ascending/description")
                {
                    subquery = "order by [Description] asc";
                    subquery1 = "order by [Description] asc";
                }
                else if (filter.order == "descending/description")
                {
                    subquery = "order by [Description] desc";
                    subquery1 = "order by [Description] desc";
                }
                else if (filter.order == "ascending/description")
                {
                    subquery = "order by [Description] asc";
                    subquery1 = "order by [Description] asc";
                }
                else if (filter.order == "descending/description")
                {
                    subquery = "order by [Description] desc";
                    subquery1 = "order by [Description] desc";
                }
                else if (filter.order == "ascending/duedate")
                {
                    subquery = "order by [DueDate] asc";
                    subquery1 = "order by [DueDate] desc";
                }
                else if (filter.order == "descending/duedate")
                {
                    subquery = "order by [DueDate] asc";
                    subquery1 = "order by [DueDate] asc";
                }
                else if (filter.order == "ascending/total")
                {
                    subquery = "order by [TotalAmount] asc";
                    subquery1 = "order by [TotalAmount] asc";
                }
                else if (filter.order == "descending/total")
                {
                    subquery = "order by [TotalAmount] desc";
                    subquery1 = "order by [TotalAmount] desc";
                }
                else if (filter.order == "ascending/balance")
                {
                    subquery = "order by [Balance] asc";
                    subquery1 = "order by [Balance] asc";
                }
                else if (filter.order == "descending/balance")
                {
                    subquery = "order by [Balance] desc";
                    subquery1 = "order by [Balance] desc";
                }
                else if (filter.order == "ascending/status")
                {
                    subquery = "order by [Status]  asc";
                    subquery1 = "order by Status asc";
                }
                else if (filter.order == "descending/status")
                {
                    subquery = "order by [Status]  desc";
                    subquery1 = "order by Status desc";
                }
                else if (filter.order == "ascending/lastnoteadded")
                {
                    subquery = "order by [InvoiceNoteAddedDate]  asc";
                    subquery1 = "order by InvoiceNoteAddedDate asc";
                }
                else if (filter.order == "descending/lastnoteadded")
                {
                    subquery = "order by [InvoiceNoteAddedDate]  desc";
                    subquery1 = "order by InvoiceNoteAddedDate desc";
                }

            }
            else
            {
                subquery = "order by [Id] desc";
                subquery1 = "order by Id desc";
            }
            #endregion
            if (!string.IsNullOrWhiteSpace(InvoiceType) && InvoiceType.ToLower() == "paid")
            {
                paidSql = "and inv.Status = 'Paid'";
            }
            //else if (!string.IsNullOrWhiteSpace(InvoiceType) && InvoiceType.ToLower() == "declined")
            //{
            //    declinequery = "and (inv.Status = 'Declined' or inv.Status = 'Rolled Over')";
            //}
            else if (!string.IsNullOrWhiteSpace(InvoiceType) && (InvoiceType.ToLower() == "rolled over" || InvoiceType.ToLower() == "cancelled"))
            {
                if (IsDeclined == false)
                {
                    paidSql = "and (inv.Status='Rolled Over' or inv.Status='Cancelled' or inv.Status='Cancel' or inv.Status='Declined' )";
                }
                else
                {
                    paidSql = "and (inv.Status='Rolled Over' or inv.Status='Cancelled' or inv.Status='Cancel')";
                }
            }
            else if ((!string.IsNullOrWhiteSpace(InvoiceType) && InvoiceType.ToLower() != "all") || string.IsNullOrWhiteSpace(InvoiceType))
            {
                if (IsDeclined == false)
                {
                    paidSql = "and inv.Status != 'Paid' and inv.Status!='Cancelled' and inv.Status!='Rolled Over' and inv.Status!='Cancel' and inv.Status!='Declined' ";
                }
                else
                {
                    paidSql = "and inv.Status != 'Paid' and inv.Status!='Cancelled' and inv.Status!='Rolled Over' and inv.Status!='Cancel'";
                }
                //declinedSql = "and inv.Status != 'Declined'";
                //unpaidquery = "and inv.Status != 'Rolled Over'";
            }
            if (filter.StrStartDate != new DateTime())
            {
                strStartDate = filter.StrStartDate.SetZeroHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss");
                strEndDate = filter.StrEndDate.SetMaxHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss");
                dateRange = string.Format("and inv.CreatedDate between '{0}' and '{1}'", strStartDate, strEndDate);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid
                    , customerid
                    , paidSql
                    , declinedSql
                    , subquery
                    , subquery1
                    , dateRange
                    , searchText, unpaidquery, declinequery);
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

        public DataSet GetAllInvoicesByfilter(AllInvoicesFilter filter, string BillicycleIdList, string InvoicestatusIdList)
        {
            string subquery = "";
            string datequery = "";
            string searchText = "";
            string BillingCycle = "";
            string statusquery = "";
            if (BillicycleIdList == "null")
            {
                BillicycleIdList = BillicycleIdList.Substring(0, BillicycleIdList.Length - 4);


            }
            if (InvoicestatusIdList == "null")
            {
                InvoicestatusIdList = InvoicestatusIdList.Substring(0, InvoicestatusIdList.Length - 4);


            }
            var array = BillicycleIdList.Split(",");
            string query = "";
            if (array != null)
            {
                foreach (var item in array)
                {
                    query += string.Format("'{0}',", item);
                }
                query = query.Remove(query.Length - 1, 1);
            }
            var array1 = InvoicestatusIdList.Split(",");
            string query1 = "";
            if (array1 != null)
            {
                foreach (var item in array1)
                {
                    query1 += string.Format("'{0}',", item);
                }
                query1 = query1.Remove(query1.Length - 1, 1);
            }
            if (filter.StartDate != new DateTime() && filter.EndDate != new DateTime())
            {
                //2018-07-30 12:50:07.463
                datequery = string.Format("and inv.CreatedDate between '{0}' and '{1}'", filter.StartDate.ToString("yyyy-MM-dd HH:mm:ss"), filter.EndDate.ToString("yyyy-MM-dd  HH:mm:ss"));
            }
            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                searchText = string.Format(@"and (cus.FirstName + ' '+cus.LastName like '%{0}%' or cus.BusinessName like '%{0}%'
                                            or inv.InvoiceId like '%{0}%' 
                                            or inv.TotalAmount like '%{0}%'  
                                            or inv.[description] like '%{0}%'
                                            or cus.AuthorizeRefId like '%{0}%')", filter.SearchText.Replace("'", "''"));
            }
            //if (!string.IsNullOrEmpty(filter.BillingCycle) && filter.BillingCycle != "-1")
            //{
            //    BillingCycle = string.Format("and BillingCycle = '{0}'", filter.BillingCycle);
            //}
            //if (!string.IsNullOrWhiteSpace(filter.Status) && filter.Status != "-1")
            //{
            //    statusquery = string.Format("and inv.[Status] = '{0}'", filter.Status);
            //}
            if (!string.IsNullOrWhiteSpace(query1))
            {
                statusquery = string.Format("and lktype.DataValue in ({0})", query1);
            }
            if (!string.IsNullOrWhiteSpace(query))
            {
                BillingCycle = string.Format("and BillingCycle in ({0})", query);
            }

            string sqlQuery = @" DECLARE @pagestart int
                            DECLARE @pageend int
                            DECLARE @pageno int
                            DECLARE @pagesize int

                            DECLARE @SearchText nvarchar(50)
                            DECLARE @SearchBy nvarchar(50)

                            Declare @PaymentMethod nvarchar(50)
                            SET @PaymentMethod = '{0}'

                            SET @SearchText = '%%'
                            SET @SearchBy = '%%'
                            SET @pageno = {1} --default 1
                            SET @pagesize = {2} --default 10 

                            SET @pagestart=(@pageno-1)* @pagesize 
                            SET @pageend = @pagesize

                            select inv.Id, inv.InvoiceId
                            ,inv.TotalAmount
                            ,inv.BillingCycle
                            ,convert(date,inv.CreatedDate) as CreatedDate
                            ,inv.InvoiceDate
                            ,inv.[Description]
                            ,inv.[Status]
                            ,inv.BalanceDue
                            ,cus.Id as CustomerIntId
                            ,(select top(1) CardTransactionId from [Transaction] where id in 
	                            (select TransactionId from TransactionHistory where InvoiceId = inv.Id) and CardTransactionId != '' ) as [TransactionId]
                            
                            ,cus.FirstName + ' '+cus.LastName as CustomerName
                            ,cus.BusinessName
                            ,cus.AuthorizeRefId 
							into #InvoiceData
                            from Invoice inv
                            left join Customer cus on cus.CustomerId = inv.CustomerId
							left join LookUp lktype on lktype.DisplayText = inv.Status  and lktype.DataKey='InvoiceStatusForSales' 

                            where inv.CreatedBy = 'System' 
                            and inv.InvoiceFor =  @PaymentMethod
                            and inv.[Status] != 'Init' 
                            and inv.IsEstimate = 0
							{4}{5}{6}{7}
                            --and BillingCycle != 'Monthly' --,Annual,Semi-Annual,Quarterly
                            --and (inv.InvoiceId like @SearchText 
                            --or cus.FirstName + ' ' +cus.LastName like @SearchText
                            --or inv.[Description] like @SearchText)

							select * into #invoiceDatafilter
							from #InvoiceData

                            SELECT TOP (@pagesize) * into #Testtable FROM #invoiceDatafilter
                            where Id NOT IN(Select TOP (@pagestart) Id from #InvoiceData {3}) 
                            {3}
                          

                            select *  from #Testtable
				            select sum(TotalAmount) as TotalAmountByPage from #TestTable 
                           
                            select Id
                            ,CONVERT(float,MonthlyMonitoringFee) as MonthlyMonitoringFee
                            ,BillCycle
                            ,SubscriptionStatus
                            into #CustomerData  from customer 
                                where IsActive =1 
                                    and PaymentMethod = 'Invoice' 
                                    --and BillAmount > 0
                                    and (MonthlyMonitoringFee is not null and CONVERT(float, MonthlyMonitoringFee) > 0)
                                    and (BillCycle is not null and BillCycle != '' and BillCycle !='-1')


                            select count(Id) as  [TotalCount]  
                            ,(select count (Id) from #CustomerData) as TotalCustomer
                            ,(select Sum (MonthlyMonitoringFee) from #CustomerData) as TotalMMR 
                            ,(select count (Id) from #CustomerData where BillCycle ='Monthly') as MonthlyCustomer
                            ,(select Sum (MonthlyMonitoringFee) from #CustomerData where BillCycle ='Monthly') as MonthlyMMR
                            ,(select count (Id) from #invoiceDatafilter where Status = 'Open') as InActiveCustomer
                            ,(select Sum (MonthlyMonitoringFee) from #CustomerData where SubscriptionStatus != 'active') as InActiveCustomerMMR
                            ,(SELECT  STUFF((SELECT  ',' + convert(nvarchar(50), Id)
							FROM #invoiceDatafilter #inv
							WHERE  #inv.Id = Id
							FOR XML PATH('')), 1, 1, '')
							) AS InvoiceIdList
                            from  #invoiceDatafilter

                            drop table #InvoiceData
                            drop table #invoiceDatafilter
                            drop table #CustomerData
                            drop table #Testtable
                            ";


            #region Order
            if (!string.IsNullOrWhiteSpace(filter.order))
            {
                if (filter.order == "ascending/customername")
                {
                    subquery = "order by CustomerName asc";

                }
                else if (filter.order == "descending/customername")
                {
                    subquery = "order by customername desc";

                }
                else if (filter.order == "ascending/invoiceno")
                {
                    subquery = "order by InvoiceId asc";

                }
                else if (filter.order == "descending/invoiceno")
                {
                    subquery = "order by InvoiceId desc";

                }
                else if (filter.order == "ascending/billamount")
                {
                    subquery = "order by TotalAmount asc";

                }
                else if (filter.order == "descending/billamount")
                {
                    subquery = "order by TotalAmount desc";

                }
                else if (filter.order == "ascending/description")
                {
                    subquery = "order by description asc";

                }
                else if (filter.order == "descending/description")
                {
                    subquery = "order by description desc";

                }
                else if (filter.order == "ascending/createddate")
                {
                    subquery = "order by CreatedDate asc";

                }
                else if (filter.order == "descending/createddate")
                {
                    subquery = "order by CreatedDate desc";

                }
                else if (filter.order == "ascending/status")
                {
                    subquery = "order by Status asc";

                }
                else if (filter.order == "descending/status")
                {
                    subquery = "order by Status desc";

                }



            }
            else
            {
                subquery = "order by CreatedDate desc";

            }
            #endregion

            try
            {
                sqlQuery = string.Format(sqlQuery,
                    filter.InvoiceFor,//0 
                    filter.PageNo, //1
                    filter.PageSize,//2
                    subquery,//3
                    datequery,//4
                    searchText, //5
                    BillingCycle,
                    statusquery//6
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

        public DataSet GetEstimateListByFilterAPI(InvoiceFilter filter)
        {
            string sqlQuery = @"DECLARE @pagestart int
                            DECLARE @pageend int
                            DECLARE @pageno int
                            DECLARE @pagesize int

                            SET @pageno = {0} --default 1
                            SET @pagesize = {1} --default 10 

                            SET @pagestart=(@pageno-1)* @pagesize 
                            SET @pageend = @pagesize

                            select distinct inv.Id
							,inv.InvoiceId
							,inv.Description
							,inv.CreatedBy
							,inv.CreatedDate
                            ,inv.Amount
                            ,inv.TotalAmount
                            ,(Select COUNT(invDet.Id) from InvoiceDetail invDet where invDet.InvoiceId=inv.InvoiceId) as EstimateEqpCount
                            ,(Select SUM(invDet.Quantity) from InvoiceDetail invDet where invDet.InvoiceId=inv.InvoiceId) as EstimateEqpQuantity
							into #InvoiceData
                            from Invoice inv
                            left join Customer cus on cus.CustomerId = inv.CustomerId
							where inv.IsEstimate=1 and cus.Id={2} and inv.[Status] != 'Init'
                            
                            SELECT TOP (@pagesize) * FROM #InvoiceData
                            where Id NOT IN(Select TOP (@pagestart) Id from #InvoiceData #inv order by #inv.Id desc)
                            order by Id desc
                            
                            select COUNT(*) as TotalCount from #InvoiceData

                            drop table #InvoiceData";
            try
            {
                sqlQuery = string.Format(sqlQuery,
                    filter.PageNo, //0
                    filter.PageSize,//1
                    filter.CustomerIntId//2
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
        public DataTable GetAllSalesInvoiceIdForARB()
        {
            string sqlQuery = @"Declare @PaymentMethod nvarchar(50)
                            SET @PaymentMethod = 'SystemGenerated'

                            select * into #InvoiceData 
                            from (select inv.InvoiceId
                            ,inv.TotalAmount
                            ,inv.BillingCycle
                            ,inv.CreatedDate
                            ,inv.InvoiceDate
                            ,inv.[Description]
                            ,inv.[Status]
                            ,inv.BalanceDue
                            ,(select top(1) CardTransactionId from [Transaction] where id in 
	                            (select TransactionId from TransactionHistory where InvoiceId = inv.Id) and CardTransactionId != '' ) as [TransactionId]
                            ,cus.Id
                            ,cus.FirstName + ' '+cus.LastName as CustomerName
                            ,cus.BusinessName
                            ,cus.AuthorizeRefId from Invoice inv
                            left join Customer cus on cus.CustomerId = inv.CustomerId

                            where inv.CreatedBy = 'System' 
                            and inv.InvoiceFor =  @PaymentMethod
                            and inv.[Status] != 'Init' 
                            and inv.IsEstimate = 0

                            ) InvoiceTable

                                SELECT TOP (5000) Id FROM #InvoiceData
                            where    InvoiceId NOT IN(Select TOP (0) InvoiceId from #InvoiceData order by CreatedDate desc) 
                            order by CreatedDate desc

                            drop table #InvoiceData";

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

        public DataSet GetAllARBInvoicesByfilter(AllInvoicesFilter filter, string BillicycleIdList, string InvoicestatusIdList)
        {
            string subquery = "";
            string datequery = "";
            string searchText = "";
            string BillingCycle = "";
            string statusquery = "";
            if (BillicycleIdList == "null")
            {
                BillicycleIdList = BillicycleIdList.Substring(0, BillicycleIdList.Length - 4);


            }
            if (InvoicestatusIdList == "null")
            {
                InvoicestatusIdList = InvoicestatusIdList.Substring(0, InvoicestatusIdList.Length - 4);


            }
            var array = BillicycleIdList.Split(",");
            string query = "";
            if (array != null)
            {
                foreach (var item in array)
                {
                    query += string.Format("'{0}',", item);
                }
                query = query.Remove(query.Length - 1, 1);
            }
            var array1 = InvoicestatusIdList.Split(",");
            string query1 = "";
            if (array1 != null)
            {
                foreach (var item in array1)
                {
                    query1 += string.Format("'{0}',", item);
                }
                query1 = query1.Remove(query1.Length - 1, 1);
            }
            if (filter.StartDate != new DateTime() && filter.EndDate != new DateTime())
            {
                //2018-07-30 12:50:07.463
                datequery = string.Format("(CreatedDate between '{0}' and '{1}') and ", filter.StartDate.ToString("yyyy-MM-dd HH:mm:ss"), filter.EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                searchText = string.Format(@"(CustomerName like '%{0}%' or BusinessName like '%{0}%'
                                            or InvoiceId like '%{0}%' 
                                            or TotalAmount like '%{0}%'  
                                            or description like '%{0}%' 
                                            or TransactionId like '%{0}%'  
                                            or AuthorizeRefId like '%{0}%') and ", filter.SearchText.Replace("'", "''"));
            }
            //if (!string.IsNullOrEmpty(filter.BillingCycle) && filter.BillingCycle != "-1")
            //{
            //    BillingCycle = string.Format("BillingCycle = '{0}' and ", filter.BillingCycle);
            //}
            if (!string.IsNullOrWhiteSpace(query))
            {
                BillingCycle = string.Format("BillingCycle in ({0}) and", query);
            }
            if (!string.IsNullOrWhiteSpace(query1))
            {
                statusquery = string.Format("and lktype.DataValue in ({0})", query1);
            }
            //if (!string.IsNullOrWhiteSpace(filter.Status) && filter.Status != "-1")
            //{
            //    statusquery = string.Format("and inv.[Status] = '{0}'", filter.Status);
            //}
            string sqlQuery = @" DECLARE @pagestart int
                            DECLARE @pageend int
                            DECLARE @pageno int
                            DECLARE @pagesize int

                            DECLARE @SearchText nvarchar(50)
                            DECLARE @SearchBy nvarchar(50)

                            Declare @PaymentMethod nvarchar(50)
                            SET @PaymentMethod = '{0}'

                            SET @SearchText = '%%'
                            SET @SearchBy = '%%'
                            SET @pageno = {1} --default 1
                            SET @pagesize = {2} --default 10 

                            SET @pagestart=(@pageno-1)* @pagesize 
                            SET @pageend = @pagesize

                            select * into #InvoiceData 
                            from (select inv.InvoiceId
                             ,inv.Id as InvId
                            ,inv.TotalAmount
                            ,inv.BillingCycle
                            ,inv.CreatedDate
                            ,inv.InvoiceDate
                            ,inv.[Description]
                            ,inv.BalanceDue
                            ,inv.[Status]
                            ,(select top(1) CardTransactionId from [Transaction] where id in 
	                            (select TransactionId from TransactionHistory where InvoiceId = inv.Id) and CardTransactionId != '' ) as [TransactionId]
                            ,cus.Id
                            ,cus.FirstName + ' '+cus.LastName as CustomerName
                            ,cus.BusinessName
                            ,cus.CustomerId
                            ,cus.AuthorizeRefId from Invoice inv
                            left join Customer cus on cus.CustomerId = inv.CustomerId
                            left join LookUp lktype on lktype.DisplayText = inv.Status  and lktype.DataKey='InvoiceStatusForSales'
                            
                            where inv.CreatedBy = 'System' 
                           and cus.PaymentMethod =  @PaymentMethod
                            and inv.[Status] != 'Init' 
                            and inv.IsEstimate = 0
                            {7}
                            --and BillingCycle != 'Monthly' --,Annual,Semi-Annual,Quarterly
                            and (inv.InvoiceId like @SearchText 
                            or cus.FirstName + ' ' +cus.LastName like @SearchText
                            or inv.[Description] like @SearchText )
                            ) InvoiceTable

                            SELECT TOP (@pagesize) * into #Testtable FROM #InvoiceData
                            where {4}{5}{6} InvoiceId NOT IN(Select TOP (@pagestart) InvoiceId from #InvoiceData {3}) 
                            {3}

                            select *  from #Testtable
				            select sum(TotalAmount) as TotalAmountByPage from #TestTable 
 

                            select Id
                            ,CONVERT(float,MonthlyMonitoringFee) as MonthlyMonitoringFee
                            ,BillCycle
                            ,SubscriptionStatus
                            into #CustomerData  from customer where IsActive =1 and PaymentMethod = @PaymentMethod and AuthorizeRefId !=''


                            select count(Id) as  [TotalCount]  
                            ,(select count (Id) from #CustomerData) as TotalCustomer
                            ,(select Sum (MonthlyMonitoringFee) from #CustomerData) as TotalMMR 
                            ,(select count (Id) from #CustomerData where BillCycle ='Monthly') as MonthlyCustomer
                            ,(select Sum (MonthlyMonitoringFee) from #CustomerData where BillCycle ='Monthly') as MonthlyMMR
                            ,(select count (Id) from #CustomerData where SubscriptionStatus != 'active') as InActiveCustomer
                            ,(select Sum (MonthlyMonitoringFee) from #CustomerData where SubscriptionStatus != 'active') as InActiveCustomerMMR
                            from  #InvoiceData
                            where
                            {4}{5}{6} /*Where Condition for paging*/
                            1 = 1

                            drop table #InvoiceData
                            drop table #CustomerData
                            drop table #TestTable
                            ";


            #region Order
            if (!string.IsNullOrWhiteSpace(filter.SortOrder))
            {
                if (filter.SortOrder == "ascending/customername")
                {
                    subquery = "order by CustomerName asc";

                }
                else if (filter.SortOrder == "descending/customername")
                {
                    subquery = "order by customername desc";

                }
                else if (filter.SortOrder == "ascending/invoiceno")
                {
                    subquery = "order by InvoiceId asc";

                }
                else if (filter.SortOrder == "descending/invoiceno")
                {
                    subquery = "order by InvoiceId desc";

                }
                else if (filter.SortOrder == "ascending/billamount")
                {
                    subquery = "order by TotalAmount asc";

                }
                else if (filter.SortOrder == "descending/billamount")
                {
                    subquery = "order by TotalAmount desc";

                }
                else if (filter.SortOrder == "ascending/description")
                {
                    subquery = "order by description asc";

                }
                else if (filter.SortOrder == "descending/description")
                {
                    subquery = "order by description desc";

                }
                else if (filter.SortOrder == "ascending/settlementdate")
                {
                    subquery = "order by CreatedDate asc";

                }
                else if (filter.SortOrder == "descending/settlementdate")
                {
                    subquery = "order by CreatedDate desc";

                }
                else if (filter.SortOrder == "ascending/transactionid")
                {
                    subquery = "order by TransactionId asc";

                }
                else if (filter.SortOrder == "descending/transactionid")
                {
                    subquery = "order by TransactionId desc";

                }



            }
            else
            {
                subquery = "order by CreatedDate desc";

            }
            #endregion

            try
            {
                sqlQuery = string.Format(sqlQuery,
                    filter.InvoiceFor,//0 
                    filter.PageNo, //1
                    filter.PageSize,//2
                    subquery,//3
                    datequery,//4
                    searchText, //5
                    BillingCycle,
                    statusquery//6
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
        public DataTable GetAllInvoice1ByCompanyIdAndCustomerId(Guid companyid, Guid customerid, string InvoiceType, bool IsDeclined)
        {

            string paidSql = "and inv.Status != 'Paid' and inv.Status != 'Declined' and inv.Status != 'Rolled Over'";
            string sqlQuery = @"
                                select inv.*,
								(select top 1 AddedDate from InvoiceNote where InvoiceId=inv.Id order by  addedDate desc) InvoiceNoteAddedDate,
                                (select top 1 Note from InvoiceNote where InvoiceId=inv.Id order by  addedDate desc) NotesInvoice,
								(select top 1 added.FirstName + ' ' + added.LastName from InvoiceNote left join Employee added on added.UserId = AddedBy where InvoiceId=inv.Id order by  addedDate desc) NoteInvoiceAddedBy,
                                (select top 1 AddedDate from CustomerAgreement where InvoiceId = inv.InvoiceId and CompanyId = '{0}' order by id desc) as CustomerViewedTime,
                                (select top 1 EquipDetail from InvoiceDetail where InvoiceId = inv.InvoiceId order by EquipDetail) InvoiceEquipDes,
                                cus.EmailAddress as CustomerMailAddress
                                 
                                 from Invoice inv
                                 join customer cus
								 on cus.CustomerId = inv.CustomerId
                                 where inv.CompanyId = '{0}'
                                 and inv.CustomerId = '{1}'
								 and inv.IsEstimate = 0
								 and inv.Status != 'init' {2}
                                
                                order by inv.CreatedDate desc
                                ";
            if (!string.IsNullOrWhiteSpace(InvoiceType) && InvoiceType.ToLower() == "paid")
            {
                paidSql = "and inv.Status = 'Paid'";
            }
            else if (!string.IsNullOrWhiteSpace(InvoiceType) && (InvoiceType.ToLower() == "rolled over" || InvoiceType.ToLower() == "cancelled"))
            {
                if (IsDeclined == false)
                {
                    paidSql = "and (inv.Status='Rolled Over' or inv.Status='Cancelled' or inv.Status='Cancel' or inv.Status='Declined' )";
                }
                else
                {
                    paidSql = "and (inv.Status='Rolled Over' or inv.Status='Cancelled' or inv.Status='Cancel')";
                }
            }
            else if ((!string.IsNullOrWhiteSpace(InvoiceType) && InvoiceType.ToLower() != "all") || string.IsNullOrWhiteSpace(InvoiceType))
            {
                if (IsDeclined == false)
                {
                    paidSql = "and inv.Status != 'Paid' and inv.Status!='Cancelled' and inv.Status!='Rolled Over' and inv.Status!='Cancel' and inv.Status!='Declined' ";
                }
                else
                {
                    paidSql = "and inv.Status != 'Paid' and inv.Status!='Cancelled' and inv.Status!='Rolled Over' and inv.Status!='Cancel'";
                }
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, customerid, paidSql);
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
        public DataTable GetDashBoardDataNew(Guid CompanyId, string tag, Guid empid, string firstdate, string lastdate, string previousfirstdate, string AccountOwnerId, bool orderpermit)
        {
            string firstdateCustomer = firstdate;
            string lastdateCustomer = lastdate;
            string previousfirstdateCustomer = previousfirstdate;
            string previouslastdateCustomer = "";

            string previouslastdate = "";
            string orderQuery = ",0 as FirstMonthOrder,0 as LastMonthOrder";
            string orderRevenueQuery = ",0 as FirstMonthRevenueCount,0 as LastMonthRevenueCount";
            if (orderpermit == true)
            {
                if (!string.IsNullOrWhiteSpace(firstdate) && !string.IsNullOrWhiteSpace(lastdate) && !string.IsNullOrWhiteSpace(previousfirstdate))
                {
                    var orderfirstdate = DateTime.Now;
                    var orderlastdate = DateTime.Now;
                    var orderpreviousfirstdate = DateTime.Now;
                    var orderpreviouslastdate = DateTime.Now;
                    DateTime FirstDate = firstdate.ToDateTime();
                    if (FirstDate != new DateTime())
                    {
                        orderfirstdate = FirstDate.SetClientZeroHourToUTC();
                    }
                    DateTime LastDate = lastdate.ToDateTime();
                    if (LastDate != new DateTime())
                    {
                        orderlastdate = LastDate.SetClientMaxHourToUTC();
                    }
                    DateTime PreviousFirstDate = previousfirstdate.ToDateTime();
                    if (PreviousFirstDate != new DateTime())
                    {
                        orderpreviousfirstdate = PreviousFirstDate.SetClientZeroHourToUTC();
                        orderpreviouslastdate = FirstDate.SetClientMaxHourToUTC();
                    }
                    orderQuery = ",(select COUNT(*) from ResturantOrder where CompanyId = @CompanyId and CreatedDate between (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + orderfirstdate + @"')) and (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + orderlastdate + @"')) and ([Status] = 'Pickedup' or [Status] = 'Delivered')) as FirstMonthOrder, (select COUNT(*) from ResturantOrder where CompanyId = @CompanyId and CreatedDate between (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + orderpreviousfirstdate + @"')) and (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + orderpreviouslastdate + @"')) and ([Status] = 'Pickedup' or [Status] = 'Delivered')) as LastMonthOrder";
                    orderRevenueQuery = ",(select SUM(Amount) from ResturantOrder where CompanyId = @CompanyId and CreatedDate between (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + orderfirstdate + @"')) and (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + orderlastdate + @"')) and ([Status] = 'Pickedup' or [Status] = 'Delivered')) as FirstMonthRevenueCount, (select SUM(Amount) from ResturantOrder where CompanyId = @CompanyId and CreatedDate between (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + orderpreviousfirstdate + @"')) and (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + orderpreviouslastdate + @"')) and ([Status] = 'Pickedup' or [Status] = 'Delivered')) as LastMonthRevenueCount";
                }

            }
            string sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                Set @CompanyId = '{0}'

                                DECLARE @EmployeeId uniqueidentifier
                                Set @EmployeeId = '{2}'

                                DECLARE @firstdate nvarchar(50)
                                Set @firstdate = '{5}'

                                DECLARE @lastdate nvarchar(50)
                                Set @lastdate = '{6}'

                                DECLARE @previousfirstdate nvarchar(50)
                                Set @previousfirstdate = '{7}'

                                DECLARE @previouslastdate nvarchar(50)
                                Set @previouslastdate = '{8}'

                                DECLARE @firstdatecustomer nvarchar(50)
                                Set @firstdatecustomer = '{9}'

                                DECLARE @lastdatecustomer nvarchar(50)
                                Set @lastdatecustomer = '{10}'

                                DECLARE @previousfirstdatecustomer nvarchar(50)
                                Set @previousfirstdatecustomer = '{11}'

                                DECLARE @previouslastdatecustomer nvarchar(50)
                                Set @previouslastdatecustomer = '{12}'

                                select c.CustomerStatus, c.JoinDate, c.Id,c.CustomerId, C.CreatedDate, c.MonthlyMonitoringFee, c.IsActive,c.TransferCustomerId,c.MoveCustomerId,c.SalesDate,c.Soldby,c.Installer,c.QA1,c.QA2 into #customer from  Customer c
                                Left Join CustomerCompany cc on cc.CustomerId = c.CustomerId
                                where cc.CompanyId = @CompanyId AND c.IsActive = 1 and c.JoinDate is not null
                                {3}
                                select
                                {4},
                                (select ISNULL(sum(inv.BalanceDue),0) from #Customer2 cs6
                                left join Invoice inv
								on inv.CustomerId = cs6.CustomerId
                                where inv.IsEstimate = 0
                                and inv.BalanceDue is not null
                                and inv.IsEstimate = 0
                                and inv.Status != 'Cancel') UnpaidAmount,
                                (select ISNULL(count(inv.Id), 0) from #Customer2 cs7
                                left join Invoice inv
								on inv.CustomerId = cs7.CustomerId
                                where inv.IsEstimate = 0
                                and inv.BalanceDue is not null
                                and inv.IsEstimate = 0
                                and inv.Status != 'Cancel'
                                and inv.Status != 'Paid') UnpaidCount
                                ,
                                ((select isnull(sum(trans.Amount), 0) from #Customer2 cs8
								left join [Transaction] trans
								on trans.CustomerId = cs8.CustomerId
                                ) +
                                (select ISNULL(sum(pr.Amount), 0) from #Customer2 cs9
								left join PaymentRevenue pr
								on pr.CustomerId = cs9.CustomerId
                                )) TotalRevenue,
                                (select count(trans.Id) from #Customer2 cs10
								left join [Transaction] trans
								on trans.CustomerId = cs10.CustomerId
                                ) TotaltTransactions
                                
                                into #FinalCustomer
                                from #Customer2 cs2

								select * from #FinalCustomer
								group by FirstMonthLeadCount, LastMonthLeadCount, FirstMonthCustomerCount, LastMonthCustomerCount, 
                                FirstMonthActivitiesCount,LastMonthActivitiesCount,
		                        FirstMonthOpportunitiesCount,LastMonthOpportunitiesCount,
                                InvoiceAmount, EstimateAmount, UnpaidAmount, UnpaidCount,MMRCount, TotalRevenue, TotaltTransactions, InvoiceCount, EstimateCount, CountMMR, FirstMonthOrder, LastMonthOrder, FirstMonthRevenueCount, LastMonthRevenueCount

                                drop table #Customer2
                                drop table #customer
                                drop table #FinalCustomer";
            string subquery = "";
            string datequery = "";
            string AccountOwnerIdFilter = "";
            string AssignToIdFilter = "";
            string SoldByIdFilter = "";
            if (!string.IsNullOrEmpty(AccountOwnerId))
            {

                AccountOwnerIdFilter = string.Format(" and AccountOwner = '{0}' ", AccountOwnerId);
                AssignToIdFilter = string.Format(" and AssignedTo = '{0}' ", AccountOwnerId);
                SoldByIdFilter = string.Format(" and SoldBy = '{0}' ", AccountOwnerId);
            }

            if (!string.IsNullOrWhiteSpace(tag) && tag.ToLower().IndexOf("admin") == -1)
            {
                subquery = @"select cs.* into #Customer2 from #customer cs
                             left join Employee emp on CONVERT(nvarchar(50), emp.UserId) = cs.Soldby AND emp.UserId = @EmployeeId
                             left join Employee emp1 on CONVERT(nvarchar(50), emp1.UserId) = cs.Installer AND emp1.UserId = @EmployeeId
                             left join Employee emp2 on CONVERT(nvarchar(50), emp2.UserId) = cs.QA1 AND emp2.UserId = @EmployeeId
                             left join Employee emp3 on CONVERT(nvarchar(50), emp3.UserId) = cs.QA2 AND emp3.UserId = @EmployeeId
                             where emp.UserId IS NOT NULL OR emp1.UserId  IS NOT NULL OR emp2.UserId  IS NOT NULL OR  emp3.UserId   IS NOT NULL";
            }
            else
            {
                subquery = @"select cs.* into #Customer2 from #customer cs"; ;
            }
            if (!string.IsNullOrWhiteSpace(firstdate) && !string.IsNullOrWhiteSpace(lastdate) && !string.IsNullOrWhiteSpace(previousfirstdate))
            {
                //2017-05-22 00:00:00.000
                #region Customer
                //DateTime FDateCustomer = firstdate.ToDateTime().SetZeroHour();
                DateTime FDateCustomer = firstdate.ToDateTime().SetClientZeroHourToUTC();
                DateTime PreviousLDateCustomer = firstdate.ToDateTime().SetClientMaxHourToUTC(); //firstdate.ToDateTime().SetMaxHour();
                if (FDateCustomer != new DateTime())
                {
                    //firstdateCustomer = FDateCustomer.ToString("yyyy-MM-dd HH:mm:ss");
                    //firstdateCustomer = FDateCustomer.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                    firstdateCustomer = FDateCustomer.ToString("yyyy-MM-dd HH:mm:ss");
                }
                //DateTime LDateCustomer = lastdateCustomer.ToDateTime().SetMaxHour();
                DateTime LDateCustomer = lastdateCustomer.ToDateTime().SetClientMaxHourToUTC();
                if (LDateCustomer != new DateTime())
                {
                    //lastdateCustomer = LDateCustomer.ToString("yyyy-MM-dd HH:mm:ss");
                    lastdateCustomer = LDateCustomer.ToString("yyyy-MM-dd HH:mm:ss");
                }
                //DateTime PreviousFDateCustomer = previousfirstdateCustomer.ToDateTime().SetZeroHour();
                DateTime PreviousFDateCustomer = previousfirstdateCustomer.ToDateTime().SetClientMaxHourToUTC();
                 
                if (PreviousFDateCustomer != new DateTime())
                {
                    //previousfirstdateCustomer = PreviousFDateCustomer.ToString("yyyy-MM-dd HH:mm:ss");
                    previousfirstdateCustomer = PreviousFDateCustomer.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (PreviousLDateCustomer != new DateTime())
                {
                    //previouslastdateCustomer = PreviousLDateCustomer.ToString("yyyy-MM-dd HH:mm:ss");
                    previouslastdateCustomer = PreviousLDateCustomer.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                }
                #endregion
                #region General
                DateTime FDate = firstdate.ToDateTime().SetClientZeroHourToUTC();
                DateTime PreviousLDate = firstdate.ToDateTime().SetClientMaxHourToUTC();

                if (FDate != new DateTime())
                {
                    firstdate = FDate.ToString("yyyy-MM-dd HH:mm:ss");
                }
                DateTime LDate = lastdate.ToDateTime().SetClientMaxHourToUTC();
                if (LDate != new DateTime())
                {
                    lastdate = LDate.ToString("yyyy-MM-dd HH:mm:ss");
                }
                DateTime PreviousFDate = previousfirstdate.ToDateTime().SetClientZeroHourToUTC();
                if (PreviousFDate != new DateTime())
                {
                    previousfirstdate = PreviousFDate.ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (PreviousLDate != new DateTime())
                {
                    //previouslastdate = PreviousLDate.ToString("yyyy-MM-dd HH:mm:ss");
                    previouslastdate = PreviousLDate.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
                }
                #endregion


                datequery = @"(Select Count(lead.Id) from #Customer2 lead
                                Left JOin CustomerCompany cc on cc.CustomerId = lead.CustomerId
                                where cc.IsLead = 1
								and lead.CreatedDate between @firstdate and @lastdate
								) FirstMonthLeadCount,
                                (Select Count(lead.Id) from #Customer2 lead
                                Left JOin CustomerCompany cc on cc.CustomerId = lead.CustomerId
                                where cc.IsLead = 1
								and lead.CreatedDate between @previousfirstdate and @previouslastdate) LastMonthLeadCount,
                                (select COUNT(cus.Id) from #Customer2 cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId and cc.CompanyId = @CompanyId
                                where cc.IsLead = 0 and cus.IsActive=1 and    cus.CustomerStatus in ('6','42') and  cus.JoinDate is not null and cc.ConvertionDate
								 between @firstdatecustomer and @lastdatecustomer {3}) FirstMonthCustomerCount,
								(select COUNT(cus.Id) from #Customer2 cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId and cc.CompanyId = @CompanyId
                                where cc.IsLead = 0 and cus.IsActive=1 and 
								    cus.CustomerStatus in ('6','42') and  cus.JoinDate is not null and cc.ConvertionDate between @previousfirstdatecustomer and @previouslastdatecustomer {3}) LastMonthCustomerCount,

                                (select count(id) from Activity where CreatedDate between @firstdate and @lastdate {2} ) as FirstMonthActivitiesCount,
	                            (select count(id) from Activity where CreatedDate between @previousfirstdate and @previouslastdate {2} ) as LastMonthActivitiesCount,

	                            (select count(id) from Opportunity where CreatedDate between @firstdate and @lastdate {1} ) as FirstMonthOpportunitiesCount,
	                            (select count(id) from Opportunity where CreatedDate between @previousfirstdate and @previouslastdate {1} ) as LastMonthOpportunitiesCount,
               

                                (select ISNULL(sum(inv.TotalAmount),0) from #Customer2 cs4
								left join Invoice inv
								on inv.CustomerId = cs4.CustomerId
                                where inv.IsEstimate = 0
                                and inv.CreatedDate between @firstdate and @lastdate) InvoiceAmount,
                                (select COUNT(*) from Invoice inv
								left join #Customer2 cs4 on cs4.CustomerId = inv.CustomerId
                                where inv.IsEstimate = 0
                                and inv.CreatedDate between @firstdate and @lastdate
                                {0}) InvoiceCount,
                                (select ISNULL(sum(inv.TotalAmount),0) from #Customer2 cs5
                                left join Invoice inv
								on inv.CustomerId = cs5.CustomerId
                                where inv.IsEstimate = 1
                                and inv.CreatedDate between @firstdate and @lastdate) EstimateAmount,
                                (select COUNT(*) from Invoice inv
								left join #Customer2 cs4 on cs4.CustomerId = inv.CustomerId
                                where inv.IsEstimate = 1
                                and inv.CreatedDate between @firstdate and @lastdate
                                {0}) EstimateCount,
                                (select sum(CONVERT(float, cs3.MonthlyMonitoringFee)) from #Customer2 cs3
                                where cs3.MonthlyMonitoringFee != '-1'
                                and cs3.MonthlyMonitoringFee is not null
                                and cs3.IsActive = 1
                                and cs3.CreatedDate between @firstdate and @lastdate) MMRCount,
                                (select COUNT(*) from #Customer2 cs3
                                where cs3.MonthlyMonitoringFee != '-1'
                                and cs3.MonthlyMonitoringFee is not null
								and cs3.MonthlyMonitoringFee != ''
                                and cs3.IsActive = 1
                                and cs3.SalesDate between @firstdate and @lastdate) CountMMR
                                {4}
                                {5}";
            }
            else
            {
                DateTime ThisMOnth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                var StartingMOnth = ThisMOnth.SetClientZeroHourToUTC();
                var EndingMonth = ThisMOnth.AddMonths(1).SetClientMaxHourToUTC();
                var StartingMOnthCustomer = ThisMOnth.SetZeroHour();
                var EndingMonthCustomer = ThisMOnth.AddMonths(1).SetZeroHour();

                var StartingLastMOnth = ThisMOnth.AddMonths(-1).SetClientZeroHourToUTC();
                var EndingLastMonth = ThisMOnth.SetClientMaxHourToUTC();
                var StartingLastMOnthCustomer = ThisMOnth.AddMonths(-1).SetZeroHour();
                var EndingLastMonthCustomer = ThisMOnth.SetZeroHour();

                //month(lead.CreatedDate) = DATEPART(MONTH, DATEADD(MONTH, 0, GETDATE())
                if (orderpermit == true)
                {
                    orderQuery = ",(select COUNT(*) from ResturantOrder where CompanyId = @CompanyId and CreatedDate between (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + StartingMOnth + @"')) and (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + EndingMonth + @"')) and ([Status] = 'Pickedup' or [Status] = 'Delivered')) as FirstMonthOrder, (select COUNT(*) from ResturantOrder where CompanyId = @CompanyId and CreatedDate between (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + StartingLastMOnth + @"')) and (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + EndingLastMonth + @"')) and ([Status] = 'Pickedup' or [Status] = 'Delivered')) as LastMonthOrder";
                    orderRevenueQuery = ",(select SUM(Amount) from ResturantOrder where CompanyId = @CompanyId and CreatedDate between (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + StartingMOnth + @"')) and (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + EndingMonth + @"')) and ([Status] = 'Pickedup' or [Status] = 'Delivered')) as FirstMonthRevenueCount, (select SUM(Amount) from ResturantOrder where CompanyId = @CompanyId and CreatedDate between (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + StartingLastMOnth + @"')) and (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + EndingLastMonth + @"')) and ([Status] = 'Pickedup' or [Status] = 'Delivered')) as LastMonthRevenueCount";
                }
                //        datequery = @"(Select Count(lead.Id) from #Customer2 lead
                //                        Left JOin CustomerCompany cc on cc.CustomerId = lead.CustomerId
                //                        where cc.IsLead = 1
                //and lead.CreatedDate between '" + StartingMOnth + @"' and '" + EndingMonth + @"' ) FirstMonthLeadCount,
                //                        (Select Count(lead.Id) from #Customer2 lead
                //                        Left JOin CustomerCompany cc on cc.CustomerId = lead.CustomerId
                //                        where cc.IsLead = 1
                //and lead.CreatedDate between '" + StartingLastMOnth + @"' and '" + EndingLastMonth + @"' ) LastMonthLeadCount,
                //                        (select COUNT(cus.Id) from #Customer2 cus
                //                        left join CustomerCompany cc on cc.CustomerId = cus.CustomerId and cc.CompanyId = @CompanyId
                //                        where cc.IsLead = 0 and cus.IsActive=1 and cus.TransferCustomerId IS NULL and (cus.MoveCustomerId='00000000-0000-0000-0000-000000000000' or cus.MoveCustomerId IS NULL)
                //and cus.SalesDate between '" + StartingMOnthCustomer + @"' and '" + EndingMonthCustomer + @"' {3}) FirstMonthCustomerCount,
                //(select COUNT(cus.Id) from #Customer2 cus
                //                        left join CustomerCompany cc on cc.CustomerId = cus.CustomerId and cc.CompanyId = @CompanyId
                //                        where cc.IsLead = 0 and cus.IsActive=1 and cus.TransferCustomerId IS NULL and (cus.MoveCustomerId='00000000-0000-0000-0000-000000000000' or cus.MoveCustomerId IS NULL)
                //and cus.SalesDate between '" + StartingLastMOnthCustomer + @"' and '" + EndingLastMonthCustomer + @"' {3}) LastMonthCustomerCount,


                //                        (select count(id) from Activity where CreatedDate between '" + StartingMOnth + @"' and '" + EndingMonth + @"' {2} ) as FirstMonthActivitiesCount,
                //                  (select count(id) from Activity where CreatedDate between '" + StartingLastMOnth + @"' and '" + EndingLastMonth + @"' {2}) as LastMonthActivitiesCount,

                //                  (select count(id) from Opportunity where CreatedDate between '" + StartingMOnth + @"' and '" + EndingMonth + @"' {1}) as FirstMonthOpportunitiesCount,
                //                  (select count(id) from Opportunity where CreatedDate between '" + StartingLastMOnth + @"' and '" + EndingLastMonth + @"' {1}) as LastMonthOpportunitiesCount,


                //                        (select ISNULL(sum(inv.TotalAmount),0) from #Customer2 cs4
                //left join Invoice inv
                //on inv.CustomerId = cs4.CustomerId
                //                        where inv.IsEstimate = 0
                //) InvoiceAmount,
                //                        (select COUNT(*) from Invoice inv
                //left join #Customer2 cs4 on cs4.CustomerId = inv.CustomerId
                //                        where inv.IsEstimate = 0
                //                        {0}
                //) InvoiceCount,
                //                        (select ISNULL(sum(inv.TotalAmount),0) from #Customer2 cs5
                //                        left join Invoice inv
                //on inv.CustomerId = cs5.CustomerId
                //                        where inv.IsEstimate = 1) EstimateAmount,
                //                        (select COUNT(*) from Invoice inv
                //left join #Customer2 cs4 on cs4.CustomerId = inv.CustomerId
                //                        where inv.IsEstimate = 1
                //                        {0}
                //                        ) EstimateCount,
                //                        (select sum(CONVERT(float, cs3.MonthlyMonitoringFee)) from #Customer2 cs3
                //                        where cs3.MonthlyMonitoringFee != '-1'
                //                        and cs3.MonthlyMonitoringFee is not null
                //                        and cs3.IsActive = 1) MMRCount,
                //                        (select COUNT(*) from #Customer2 cs3
                //                        where cs3.MonthlyMonitoringFee != '-1'
                //                        and cs3.MonthlyMonitoringFee is not null
                //and cs3.MonthlyMonitoringFee != ''
                //                        and cs3.IsActive = 1) CountMMR
                //                        {4}
                //                        {5}";

                datequery = @"(Select Count(lead.Id) from #Customer2 lead
                                Left JOin CustomerCompany cc on cc.CustomerId = lead.CustomerId
                                where cc.IsLead = 1
								and lead.CreatedDate between '" + StartingMOnth + @"' and '" + EndingMonth + @"' ) FirstMonthLeadCount,
                                (Select Count(lead.Id) from #Customer2 lead
                                Left JOin CustomerCompany cc on cc.CustomerId = lead.CustomerId
                                where cc.IsLead = 1
								and lead.CreatedDate between '" + StartingLastMOnth + @"' and '" + EndingLastMonth + @"' ) LastMonthLeadCount,
                                (select COUNT(cus.Id) from #Customer2 cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId and cc.CompanyId = @CompanyId
                                where cc.IsLead = 0 and cus.IsActive=1 and   cus.IsActive=1
								and   cus.CustomerStatus in ('6','42') and  cus.JoinDate is not null and cc.ConvertionDate between '" + StartingMOnthCustomer + @"' and '" + EndingMonthCustomer + @"' {3}) FirstMonthCustomerCount,
								(select COUNT(cus.Id) from #Customer2 cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId and cc.CompanyId = @CompanyId
                                where cc.IsLead = 0 and cus.IsActive=1 and cus.CustomerStatus in ('6','42') and  cus.JoinDate is not null
								and cc.ConvertionDate between '" + StartingLastMOnthCustomer + @"' and '" + EndingLastMonthCustomer + @"' {3}) LastMonthCustomerCount,
                                

                                (select count(id) from Activity where CreatedDate between '" + StartingMOnth + @"' and '" + EndingMonth + @"' {2} ) as FirstMonthActivitiesCount,
		                        (select count(id) from Activity where CreatedDate between '" + StartingLastMOnth + @"' and '" + EndingLastMonth + @"' {2}) as LastMonthActivitiesCount,

		                        (select count(id) from Opportunity where CreatedDate between '" + StartingMOnth + @"' and '" + EndingMonth + @"' {1}) as FirstMonthOpportunitiesCount,
		                        (select count(id) from Opportunity where CreatedDate between '" + StartingLastMOnth + @"' and '" + EndingLastMonth + @"' {1}) as LastMonthOpportunitiesCount,

 
                                (select ISNULL(sum(inv.TotalAmount),0) from #Customer2 cs4
								left join Invoice inv
								on inv.CustomerId = cs4.CustomerId
                                where inv.IsEstimate = 0
								) InvoiceAmount,
                                (select COUNT(*) from Invoice inv
								left join #Customer2 cs4 on cs4.CustomerId = inv.CustomerId
                                where inv.IsEstimate = 0
                                {0}
								) InvoiceCount,
                                (select ISNULL(sum(inv.TotalAmount),0) from #Customer2 cs5
                                left join Invoice inv
								on inv.CustomerId = cs5.CustomerId
                                where inv.IsEstimate = 1) EstimateAmount,
                                (select COUNT(*) from Invoice inv
								left join #Customer2 cs4 on cs4.CustomerId = inv.CustomerId
                                where inv.IsEstimate = 1
                                {0}
                                ) EstimateCount,
                                (select sum(CONVERT(float, cs3.MonthlyMonitoringFee)) from #Customer2 cs3
                                where cs3.MonthlyMonitoringFee != '-1'
                                and cs3.MonthlyMonitoringFee is not null
                                and cs3.IsActive = 1) MMRCount,
                                (select COUNT(*) from #Customer2 cs3
                                where cs3.MonthlyMonitoringFee != '-1'
                                and cs3.MonthlyMonitoringFee is not null
								and cs3.MonthlyMonitoringFee != ''
                                and cs3.IsActive = 1) CountMMR
                                {4}
                                {5}";

            }
            string IsSalesPerson = "";
            if (!string.IsNullOrWhiteSpace(tag) && tag.ToLower().IndexOf("admin") == -1)
            {
                IsSalesPerson = @"and inv.LastUpdatedByUid = @EmployeeId";
                datequery = string.Format(datequery, IsSalesPerson, AccountOwnerIdFilter, AssignToIdFilter, SoldByIdFilter, orderQuery, orderRevenueQuery);
            }
            else
            {
                IsSalesPerson = @"";
                datequery = string.Format(datequery, IsSalesPerson, AccountOwnerIdFilter, AssignToIdFilter, SoldByIdFilter, orderQuery, orderRevenueQuery);
            }
            try
            {
                //previouslastdate = previouslastdate.ToDateTime().AddHours(-24).ToString("yyyy-MM-dd HH:mm:ss");
                sqlQuery = string.Format(sqlQuery, CompanyId, tag, empid, subquery, datequery, firstdate, lastdate, previousfirstdate, previouslastdate, firstdateCustomer, lastdateCustomer, previousfirstdateCustomer, previouslastdateCustomer);
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

        public DataTable GetDashBoardData(Guid CompanyId, string tag, Guid empid, string firstdate, string lastdate, string previousfirstdate, string AccountOwnerId, bool orderpermit)
        {
            string firstdateCustomer = firstdate;
            string lastdateCustomer = lastdate;
            string previousfirstdateCustomer = previousfirstdate;
            string previouslastdateCustomer = "";

            string previouslastdate = "";
            string orderQuery = ",0 as FirstMonthOrder,0 as LastMonthOrder";
            string orderRevenueQuery = ",0 as FirstMonthRevenueCount,0 as LastMonthRevenueCount";
            if (orderpermit == true)
            {
                if (!string.IsNullOrWhiteSpace(firstdate) && !string.IsNullOrWhiteSpace(lastdate) && !string.IsNullOrWhiteSpace(previousfirstdate))
                {
                    var orderfirstdate = DateTime.Now;
                    var orderlastdate = DateTime.Now;
                    var orderpreviousfirstdate = DateTime.Now;
                    var orderpreviouslastdate = DateTime.Now;
                    DateTime FirstDate = firstdate.ToDateTime();
                    if (FirstDate != new DateTime())
                    {
                        orderfirstdate = FirstDate.SetClientZeroHourToUTC();
                    }
                    DateTime LastDate = lastdate.ToDateTime();
                    if (LastDate != new DateTime())
                    {
                        orderlastdate = LastDate.SetClientMaxHourToUTC();
                    }
                    DateTime PreviousFirstDate = previousfirstdate.ToDateTime();
                    if (PreviousFirstDate != new DateTime())
                    {
                        orderpreviousfirstdate = PreviousFirstDate.SetClientZeroHourToUTC();
                        orderpreviouslastdate = FirstDate.SetClientMaxHourToUTC();
                    }
                    orderQuery = ",(select COUNT(*) from ResturantOrder where CompanyId = @CompanyId and CreatedDate between (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + orderfirstdate + @"')) and (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + orderlastdate + @"')) and ([Status] = 'Pickedup' or [Status] = 'Delivered')) as FirstMonthOrder, (select COUNT(*) from ResturantOrder where CompanyId = @CompanyId and CreatedDate between (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + orderpreviousfirstdate + @"')) and (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + orderpreviouslastdate + @"')) and ([Status] = 'Pickedup' or [Status] = 'Delivered')) as LastMonthOrder";
                    orderRevenueQuery = ",(select SUM(Amount) from ResturantOrder where CompanyId = @CompanyId and CreatedDate between (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + orderfirstdate + @"')) and (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + orderlastdate + @"')) and ([Status] = 'Pickedup' or [Status] = 'Delivered')) as FirstMonthRevenueCount, (select SUM(Amount) from ResturantOrder where CompanyId = @CompanyId and CreatedDate between (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + orderpreviousfirstdate + @"')) and (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + orderpreviouslastdate + @"')) and ([Status] = 'Pickedup' or [Status] = 'Delivered')) as LastMonthRevenueCount";
                }

            }
            string sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                Set @CompanyId = '{0}'

                                DECLARE @EmployeeId uniqueidentifier
                                Set @EmployeeId = '{2}'

                                DECLARE @firstdate nvarchar(50)
                                Set @firstdate = '{5}'

                                DECLARE @lastdate nvarchar(50)
                                Set @lastdate = '{6}'

                                DECLARE @previousfirstdate nvarchar(50)
                                Set @previousfirstdate = '{7}'

                                DECLARE @previouslastdate nvarchar(50)
                                Set @previouslastdate = '{8}'

                                DECLARE @firstdatecustomer nvarchar(50)
                                Set @firstdatecustomer = '{9}'

                                DECLARE @lastdatecustomer nvarchar(50)
                                Set @lastdatecustomer = '{10}'

                                DECLARE @previousfirstdatecustomer nvarchar(50)
                                Set @previousfirstdatecustomer = '{11}'

                                DECLARE @previouslastdatecustomer nvarchar(50)
                                Set @previouslastdatecustomer = '{12}'

                                select c.Id,c.CustomerId, C.CreatedDate, c.MonthlyMonitoringFee, c.IsActive,c.TransferCustomerId,c.MoveCustomerId,c.SalesDate, c.Soldby, c.Installer, c.QA1, c.QA2 into #customer from  Customer c
                                Left Join CustomerCompany cc on cc.CustomerId = c.CustomerId
                                where cc.CompanyId = @CompanyId AND c.IsActive = 1 and c.JoinDate is not null
                                {3}
                                select
                                {4},
                                (select ISNULL(sum(inv.BalanceDue),0) from #Customer2 cs6
                                left join Invoice inv
								on inv.CustomerId = cs6.CustomerId
                                where inv.IsEstimate = 0
                                and inv.BalanceDue is not null
                                and inv.IsEstimate = 0
                                and inv.Status != 'Cancel') UnpaidAmount,
                                (select ISNULL(count(inv.Id), 0) from #Customer2 cs7
                                left join Invoice inv
								on inv.CustomerId = cs7.CustomerId
                                where inv.IsEstimate = 0
                                and inv.BalanceDue is not null
                                and inv.IsEstimate = 0
                                and inv.Status != 'Cancel'
                                and inv.Status != 'Paid') UnpaidCount
                                ,
                                ((select isnull(sum(trans.Amount), 0) from #Customer2 cs8
								left join [Transaction] trans
								on trans.CustomerId = cs8.CustomerId
                                ) +
                                (select ISNULL(sum(pr.Amount), 0) from #Customer2 cs9
								left join PaymentRevenue pr
								on pr.CustomerId = cs9.CustomerId
                                )) TotalRevenue,
                                (select count(trans.Id) from #Customer2 cs10
								left join [Transaction] trans
								on trans.CustomerId = cs10.CustomerId
                                ) TotaltTransactions
                                
                                into #FinalCustomer
                                from #Customer2 cs2

								select * from #FinalCustomer
								group by FirstMonthLeadCount, LastMonthLeadCount, FirstMonthCustomerCount, LastMonthCustomerCount, 
                                FirstMonthActivitiesCount,LastMonthActivitiesCount,
		                        FirstMonthOpportunitiesCount,LastMonthOpportunitiesCount,
                                InvoiceAmount, EstimateAmount, UnpaidAmount, UnpaidCount,MMRCount, TotalRevenue, TotaltTransactions, InvoiceCount, EstimateCount, CountMMR, FirstMonthOrder, LastMonthOrder, FirstMonthRevenueCount, LastMonthRevenueCount

                                drop table #Customer2
                                drop table #customer
                                drop table #FinalCustomer";
            string subquery = "";
            string datequery = "";
            string AccountOwnerIdFilter = "";
            string AssignToIdFilter = "";
            string SoldByIdFilter = "";
            if (!string.IsNullOrEmpty(AccountOwnerId))
            {

                AccountOwnerIdFilter = string.Format(" and AccountOwner = '{0}' ", AccountOwnerId);
                AssignToIdFilter = string.Format(" and AssignedTo = '{0}' ", AccountOwnerId);
                SoldByIdFilter = string.Format(" and SoldBy = '{0}' ", AccountOwnerId);
            }

            if (!string.IsNullOrWhiteSpace(tag) && tag.ToLower().IndexOf("admin") == -1)
            {
                subquery = @"select cs.* into #Customer2 from #customer cs
                             left join Employee emp on CONVERT(nvarchar(50), emp.UserId) = cs.Soldby AND emp.UserId = @EmployeeId
                             left join Employee emp1 on CONVERT(nvarchar(50), emp1.UserId) = cs.Installer AND emp1.UserId = @EmployeeId
                             left join Employee emp2 on CONVERT(nvarchar(50), emp2.UserId) = cs.QA1 AND emp2.UserId = @EmployeeId
                             left join Employee emp3 on CONVERT(nvarchar(50), emp3.UserId) = cs.QA2 AND emp3.UserId = @EmployeeId
                             where emp.UserId IS NOT NULL OR emp1.UserId  IS NOT NULL OR emp2.UserId  IS NOT NULL OR  emp3.UserId   IS NOT NULL";
            }
            else
            {
                subquery = @"select cs.* into #Customer2 from #customer cs"; ;
            }
            if (!string.IsNullOrWhiteSpace(firstdate) && !string.IsNullOrWhiteSpace(lastdate) && !string.IsNullOrWhiteSpace(previousfirstdate))
            {
                //2017-05-22 00:00:00.000
                #region Customer
                DateTime FDateCustomer = firstdate.ToDateTime().SetZeroHour();
                DateTime PreviousLDateCustomer = firstdate.ToDateTime().SetMaxHour();
                if (FDateCustomer != new DateTime())
                {
                    firstdateCustomer = FDateCustomer.ToString("yyyy-MM-dd HH:mm:ss");
                }
                DateTime LDateCustomer = lastdateCustomer.ToDateTime().SetMaxHour();
                if (LDateCustomer != new DateTime())
                {
                    lastdateCustomer = LDateCustomer.ToString("yyyy-MM-dd HH:mm:ss");
                }
                DateTime PreviousFDateCustomer = previousfirstdateCustomer.ToDateTime().SetZeroHour();
                if (PreviousFDateCustomer != new DateTime())
                {
                    previousfirstdateCustomer = PreviousFDateCustomer.ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (PreviousLDateCustomer != new DateTime())
                {
                    previouslastdateCustomer = PreviousLDateCustomer.ToString("yyyy-MM-dd HH:mm:ss");
                }
                #endregion
                #region General
                DateTime FDate = firstdate.ToDateTime().SetClientZeroHourToUTC();
                DateTime PreviousLDate = firstdate.ToDateTime().SetClientMaxHourToUTC();

                if (FDate != new DateTime())
                {
                    firstdate = FDate.ToString("yyyy-MM-dd HH:mm:ss");
                }
                DateTime LDate = lastdate.ToDateTime().SetClientMaxHourToUTC();
                if (LDate != new DateTime())
                {
                    lastdate = LDate.ToString("yyyy-MM-dd HH:mm:ss");
                }
                DateTime PreviousFDate = previousfirstdate.ToDateTime().SetClientZeroHourToUTC();
                if (PreviousFDate != new DateTime())
                {
                    previousfirstdate = PreviousFDate.ToString("yyyy-MM-dd HH:mm:ss");
                }
                if (PreviousLDate != new DateTime())
                {
                    previouslastdate = PreviousLDate.ToString("yyyy-MM-dd HH:mm:ss");
                }
                #endregion


                datequery = @"(Select Count(lead.Id) from #Customer2 lead
                                Left JOin CustomerCompany cc on cc.CustomerId = lead.CustomerId
                                where cc.IsLead = 1
								and lead.CreatedDate between @firstdate and @lastdate
								) FirstMonthLeadCount,
                                (Select Count(lead.Id) from #Customer2 lead
                                Left JOin CustomerCompany cc on cc.CustomerId = lead.CustomerId
                                where cc.IsLead = 1
								and lead.CreatedDate between @previousfirstdate and @previouslastdate) LastMonthLeadCount,
                                (select COUNT(cus.Id) from #Customer2 cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId and cc.CompanyId = @CompanyId
                                where cc.IsLead = 0 and cus.IsActive=1 and cus.TransferCustomerId IS NULL and (cus.MoveCustomerId='00000000-0000-0000-0000-000000000000' or cus.MoveCustomerId IS NULL)
								and cus.SalesDate between @firstdatecustomer and @lastdatecustomer {3}) FirstMonthCustomerCount,
								(select COUNT(cus.Id) from #Customer2 cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId and cc.CompanyId = @CompanyId
                                where cc.IsLead = 0 and cus.IsActive=1 and cus.TransferCustomerId IS NULL and (cus.MoveCustomerId='00000000-0000-0000-0000-000000000000' or cus.MoveCustomerId IS NULL)
								and cus.SalesDate between @previousfirstdatecustomer and @previouslastdatecustomer {3}) LastMonthCustomerCount,

                                (select count(id) from Activity where CreatedDate between @firstdate and @lastdate {2} ) as FirstMonthActivitiesCount,
	                            (select count(id) from Activity where CreatedDate between @previousfirstdate and @previouslastdate {2} ) as LastMonthActivitiesCount,

	                            (select count(id) from Opportunity where CreatedDate between @firstdate and @lastdate {1} ) as FirstMonthOpportunitiesCount,
	                            (select count(id) from Opportunity where CreatedDate between @previousfirstdate and @previouslastdate {1} ) as LastMonthOpportunitiesCount,
               

                                (select ISNULL(sum(inv.TotalAmount),0) from #Customer2 cs4
								left join Invoice inv
								on inv.CustomerId = cs4.CustomerId
                                where inv.IsEstimate = 0
                                and inv.CreatedDate between @firstdate and @lastdate) InvoiceAmount,
                                (select COUNT(*) from Invoice inv
								left join #Customer2 cs4 on cs4.CustomerId = inv.CustomerId
                                where inv.IsEstimate = 0
                                and inv.CreatedDate between @firstdate and @lastdate
                                {0}) InvoiceCount,
                                (select ISNULL(sum(inv.TotalAmount),0) from #Customer2 cs5
                                left join Invoice inv
								on inv.CustomerId = cs5.CustomerId
                                where inv.IsEstimate = 1
                                and inv.CreatedDate between @firstdate and @lastdate) EstimateAmount,
                                (select COUNT(*) from Invoice inv
								left join #Customer2 cs4 on cs4.CustomerId = inv.CustomerId
                                where inv.IsEstimate = 1
                                and inv.CreatedDate between @firstdate and @lastdate
                                {0}) EstimateCount,
                                (select sum(CONVERT(float, cs3.MonthlyMonitoringFee)) from #Customer2 cs3
                                where cs3.MonthlyMonitoringFee != '-1'
                                and cs3.MonthlyMonitoringFee is not null
                                and cs3.IsActive = 1
                                and cs3.CreatedDate between @firstdate and @lastdate) MMRCount,
                                (select COUNT(*) from #Customer2 cs3
                                where cs3.MonthlyMonitoringFee != '-1'
                                and cs3.MonthlyMonitoringFee is not null
								and cs3.MonthlyMonitoringFee != ''
                                and cs3.IsActive = 1
                                and cs3.SalesDate between @firstdate and @lastdate) CountMMR
                                {4}
                                {5}";
            }
            else
            {
                DateTime ThisMOnth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                var StartingMOnth = ThisMOnth.SetClientZeroHourToUTC();
                var EndingMonth = ThisMOnth.AddMonths(1).SetClientMaxHourToUTC();
                var StartingMOnthCustomer = ThisMOnth.SetZeroHour();
                var EndingMonthCustomer = ThisMOnth.AddMonths(1).SetZeroHour();

                var StartingLastMOnth = ThisMOnth.AddMonths(-1).SetClientZeroHourToUTC();
                var EndingLastMonth = ThisMOnth.SetClientMaxHourToUTC();
                var StartingLastMOnthCustomer = ThisMOnth.AddMonths(-1).SetZeroHour();
                var EndingLastMonthCustomer = ThisMOnth.SetZeroHour();

                //month(lead.CreatedDate) = DATEPART(MONTH, DATEADD(MONTH, 0, GETDATE())
                if (orderpermit == true)
                {
                    orderQuery = ",(select COUNT(*) from ResturantOrder where CompanyId = @CompanyId and CreatedDate between (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + StartingMOnth + @"')) and (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + EndingMonth + @"')) and ([Status] = 'Pickedup' or [Status] = 'Delivered')) as FirstMonthOrder, (select COUNT(*) from ResturantOrder where CompanyId = @CompanyId and CreatedDate between (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + StartingLastMOnth + @"')) and (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + EndingLastMonth + @"')) and ([Status] = 'Pickedup' or [Status] = 'Delivered')) as LastMonthOrder";
                    orderRevenueQuery = ",(select SUM(Amount) from ResturantOrder where CompanyId = @CompanyId and CreatedDate between (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + StartingMOnth + @"')) and (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + EndingMonth + @"')) and ([Status] = 'Pickedup' or [Status] = 'Delivered')) as FirstMonthRevenueCount, (select SUM(Amount) from ResturantOrder where CompanyId = @CompanyId and CreatedDate between (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + StartingLastMOnth + @"')) and (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '" + EndingLastMonth + @"')) and ([Status] = 'Pickedup' or [Status] = 'Delivered')) as LastMonthRevenueCount";
                }
                datequery = @"(Select Count(lead.Id) from #Customer2 lead
                                Left JOin CustomerCompany cc on cc.CustomerId = lead.CustomerId
                                where cc.IsLead = 1
								and lead.CreatedDate between '" + StartingMOnth + @"' and '" + EndingMonth + @"' ) FirstMonthLeadCount,
                                (Select Count(lead.Id) from #Customer2 lead
                                Left JOin CustomerCompany cc on cc.CustomerId = lead.CustomerId
                                where cc.IsLead = 1
								and lead.CreatedDate between '" + StartingLastMOnth + @"' and '" + EndingLastMonth + @"' ) LastMonthLeadCount,
                                (select COUNT(cus.Id) from #Customer2 cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId and cc.CompanyId = @CompanyId
                                where cc.IsLead = 0 and cus.IsActive=1 and cus.TransferCustomerId IS NULL and (cus.MoveCustomerId='00000000-0000-0000-0000-000000000000' or cus.MoveCustomerId IS NULL)
								and cus.SalesDate between '" + StartingMOnthCustomer + @"' and '" + EndingMonthCustomer + @"' {3}) FirstMonthCustomerCount,
								(select COUNT(cus.Id) from #Customer2 cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId and cc.CompanyId = @CompanyId
                                where cc.IsLead = 0 and cus.IsActive=1 and cus.TransferCustomerId IS NULL and (cus.MoveCustomerId='00000000-0000-0000-0000-000000000000' or cus.MoveCustomerId IS NULL)
								and cus.SalesDate between '" + StartingLastMOnthCustomer + @"' and '" + EndingLastMonthCustomer + @"' {3}) LastMonthCustomerCount,
                                

                                (select count(id) from Activity where CreatedDate between '" + StartingMOnth + @"' and '" + EndingMonth + @"' {2} ) as FirstMonthActivitiesCount,
		                        (select count(id) from Activity where CreatedDate between '" + StartingLastMOnth + @"' and '" + EndingLastMonth + @"' {2}) as LastMonthActivitiesCount,

		                        (select count(id) from Opportunity where CreatedDate between '" + StartingMOnth + @"' and '" + EndingMonth + @"' {1}) as FirstMonthOpportunitiesCount,
		                        (select count(id) from Opportunity where CreatedDate between '" + StartingLastMOnth + @"' and '" + EndingLastMonth + @"' {1}) as LastMonthOpportunitiesCount,

 
                                (select ISNULL(sum(inv.TotalAmount),0) from #Customer2 cs4
								left join Invoice inv
								on inv.CustomerId = cs4.CustomerId
                                where inv.IsEstimate = 0
								) InvoiceAmount,
                                (select COUNT(*) from Invoice inv
								left join #Customer2 cs4 on cs4.CustomerId = inv.CustomerId
                                where inv.IsEstimate = 0
                                {0}
								) InvoiceCount,
                                (select ISNULL(sum(inv.TotalAmount),0) from #Customer2 cs5
                                left join Invoice inv
								on inv.CustomerId = cs5.CustomerId
                                where inv.IsEstimate = 1) EstimateAmount,
                                (select COUNT(*) from Invoice inv
								left join #Customer2 cs4 on cs4.CustomerId = inv.CustomerId
                                where inv.IsEstimate = 1
                                {0}
                                ) EstimateCount,
                                (select sum(CONVERT(float, cs3.MonthlyMonitoringFee)) from #Customer2 cs3
                                where cs3.MonthlyMonitoringFee != '-1'
                                and cs3.MonthlyMonitoringFee is not null
                                and cs3.IsActive = 1) MMRCount,
                                (select COUNT(*) from #Customer2 cs3
                                where cs3.MonthlyMonitoringFee != '-1'
                                and cs3.MonthlyMonitoringFee is not null
								and cs3.MonthlyMonitoringFee != ''
                                and cs3.IsActive = 1) CountMMR
                                {4}
                                {5}";
            }
            string IsSalesPerson = "";
            if (!string.IsNullOrWhiteSpace(tag) && tag.ToLower().IndexOf("admin") == -1)
            {
                IsSalesPerson = @"and inv.LastUpdatedByUid = @EmployeeId";
                datequery = string.Format(datequery, IsSalesPerson, AccountOwnerIdFilter, AssignToIdFilter, SoldByIdFilter, orderQuery, orderRevenueQuery);
            }
            else
            {
                IsSalesPerson = @"";
                datequery = string.Format(datequery, IsSalesPerson, AccountOwnerIdFilter, AssignToIdFilter, SoldByIdFilter, orderQuery, orderRevenueQuery);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, CompanyId, tag, empid, subquery, datequery, firstdate, lastdate, previousfirstdate, previouslastdate, firstdateCustomer, lastdateCustomer, previousfirstdateCustomer, previouslastdateCustomer);
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

        public DataTable GetDashBoardDataTechnician(Guid CompanyId, Guid empid)
        {
            string sqlQuery = @"select Distinct
                                (select COUNT(tu.UserId)
                                from TicketUser tu
                                LEFT JOIN Ticket t on t.TicketId=tu.TiketId
                                where tu.UserId='{0}'
                                and t.CompanyId='{1}'
                                and (t.TicketType like '%Install%' or t.TicketType='Pick Up' or t.TicketType='Drop Off') and  (t.Status!='Completed' and t.Status != 'Closed')) as OpenInstallationTicket,
                                (select COUNT(tu.UserId)
                                from TicketUser tu
                                LEFT JOIN Ticket t on t.TicketId=tu.TiketId
                                where tu.UserId='{0}'
                                and t.CompanyId='{1}'
                                and t.TicketType='Installation' and  (t.Status='Completed' or t.Status='Closed')) as ClosedInstallationTicket,
                                (select COUNT(tu.UserId)
                                from TicketUser tu
                                LEFT JOIN Ticket t on t.TicketId=tu.TiketId
                                where tu.UserId='{0}'
                                and t.CompanyId='{1}'
                                and t.TicketType='Service'  and  (t.Status!='Completed' and t.Status != 'Closed')) as OpenServiceTicket,
                                (select COUNT(tu.UserId)
                                from TicketUser tu
                                LEFT JOIN Ticket t on t.TicketId=tu.TiketId
                                where tu.UserId='{0}'
                                and t.CompanyId='{1}'
                                and t.TicketType='Service' and  (t.Status='Completed' or t.Status='Closed')) as ClosedServiceTicket,
                                (select COUNT(tu.UserId)
                                from TicketUser tu
                                LEFT JOIN Ticket t on t.TicketId=tu.TiketId
                                where tu.UserId='{0}'
                                and t.CompanyId='{1}'
                                and t.TicketType='Pick Up' and  (t.Status!='Completed' and t.Status != 'Closed')) as OpenPickTicket,
                                (select COUNT(tu.UserId)
                                from TicketUser tu
                                LEFT JOIN Ticket t on t.TicketId=tu.TiketId
                                where tu.UserId='{0}'
                                and t.CompanyId='{1}'
                                and t.TicketType='Pick Up' and  (t.Status='Completed' or t.Status='Closed')) as ClosedPickTicket,
                                (select COUNT(tu.UserId)
                                from TicketUser tu
                                LEFT JOIN Ticket t on t.TicketId=tu.TiketId
                                where tu.UserId='{0}'
                                and t.CompanyId='{1}'
                                and t.TicketType='Drop Off' and  (t.Status!='Completed' and t.Status != 'Closed')) as OpenDropTicket,
                                (select COUNT(tu.UserId)
                                from TicketUser tu
                                LEFT JOIN Ticket t on t.TicketId=tu.TiketId
                                where tu.UserId='{0}'
                                and t.CompanyId='{1}'
                                and t.TicketType='Drop Off' and  (t.Status='Completed' or t.Status='Closed')) as ClosedDropTicket
                                from Ticket
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, empid, CompanyId);
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


        public DataSet GetDashBoardBoardBarDataTechnician(Guid CompanyId, Guid empid)
        {
            TicketFilter TicketFilters = new TicketFilter();
            if (TicketFilters.StartDate == new DateTime())
            {
                TicketFilters.StartDate = DateTime.Today.AddDays(-90).Date;
            }
            if (TicketFilters.EndDate == new DateTime())
            {
                TicketFilters.EndDate = DateTime.Today.Date;
            }
            if (!TicketFilters.PageNo.HasValue || TicketFilters.PageNo.Value < 1)
            {
                TicketFilters.PageNo = 1;
            }
            TicketFilters.CompanyId = CompanyId;
            TicketFilters.UserId = empid;
            TicketFilters.PageSize = 20;
            TicketFilters.ReportTabType = "GoBack";

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
                ReportQuery = string.Format("and countticket > 1");
                ReportColQuery = string.Format("(select count(tik.Id) from ticket tik where convert(date, tik.CreatedDate) between '{0}' and '{1}' and tik.CustomerId=cs.CustomerId) as CountTicket", TicketFilters.StartDate, TicketFilters.EndDate);
                ReportCountQuery = string.Format("select  count(Id) as [TotalCount] from #TicketData where countticket > 1 and AssignedToUserId = '<UserId>{0}</UserId>'", TicketFilters.UserId);
            }
            else
            {
                ReportColQuery = string.Format("'' as CountTicket");
                ReportCountQuery = string.Format("select  count(Id) as [TotalCount] from #TicketData");
            }
            #region TicketStatus
            if (!string.IsNullOrWhiteSpace(TicketFilters.TicketStatus)
                && TicketFilters.TicketStatus != "-1" && TicketFilters.TicketStatus != "null")
            {
                ticketStatusQuery = string.Format("and tk.[Status] in ('{0}')", TicketFilters.TicketStatus);
            }
            #endregion

            #region Assigned
            if (TicketFilters.UserId != Guid.Empty)
            {
                assignedQuery = string.Format("and tuser.UserId in ('{0}')", TicketFilters.UserId);
            }
            #endregion


            #region TicketType
            if (!string.IsNullOrWhiteSpace(TicketFilters.TicketType)
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
            if (!string.IsNullOrWhiteSpace(TicketFilters.MyTicket))
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
		                                select tk.Id
                      
                                        --,(select CAST(firstname + ' '+LastName + ', ' AS VARCHAR(200))  from Employee  where UserId in (select UserId from TicketUser tulist where tulist.TiketId = tk.TicketId and IsPrimary = 1) FOR XML PATH ('') ) as AssignedTo
                                        ,(select UserId  from Employee  where UserId in (select UserId from TicketUser tulist where tulist.TiketId = tk.TicketId and IsPrimary = 1) FOR XML PATH ('') ) as AssignedToUserId
                                        --,(select CAST(firstname + ' '+LastName + ', ' AS VARCHAR(200))  from Employee  where UserId in (select UserId from TicketUser tualist where tualist.TiketId = tk.TicketId and IsPrimary = 0) FOR XML PATH ('') ) as AdditionalMembers
                                        ,{16}   
                                        from Ticket tk
                                        LEFT JOIN Customer cs on cs.CustomerId=tk.CustomerId
                                        left join TicketUser tuser on tuser.TiketId = tk.TicketId and tuser.IsPrimary = 1
                                        left join Lookup lktype on  lktype.DataKey ='TicketType'  
                                        and lktype.DataValue = tk.TicketType
                                        left join CustomerAppointment CA on  CA.AppointmentId = tk.TicketId
                                        left join Lookup lkStartTime on lkStartTime.DataKey = 'Arrival'
                                        and lkStartTime.DataValue = CA.AppointmentStartTime
   
		                                where tk.CompanyId = @CompanyId 
                                        {5}
                                        {6}
                                        {7}
                                        {8}
                                        {9}
                                        {10}
                                        {13}
                                        {14}
                                        
	                                ) a  
	                            {17}
                                DROP TABLE #TicketData
                                    ";
            DayOfWeek weekStart = DayOfWeek.Monday; // or Sunday, or whenever
            DateTime startingDate = DateTime.Today;

            while (startingDate.DayOfWeek != weekStart)
                startingDate = startingDate.AddDays(-1);

            DateTime previousWeekStart = startingDate.AddDays(-7);
            DateTime previousWeekEnd = startingDate.AddDays(0);

            string sqlQuery2 = @"select Distinct
                                (select SUM(TotalCommission) from SalesCommission 
								where UserId='{0}'  
								and CreatedDate < '{2}' and CreatedDate >= '{3}')
								as TotalCommissionSC,

                                (select SUM(TotalCommission) from TechCommission 
								where UserId='{0}' 
								and CreatedDate < '{2}' and CreatedDate >= '{3}')
								as TotalCommissionTC,
                                
								(select SUM(Commission) from AddMemberCommission 
								where UserId='{0}' 
								and CreatedDate < '{2}' and CreatedDate >= '{3}') 
								as TotalCommissionAC,
                                
								(select SUM(Commission) From FollowUpCommission 
								where UserId='{0}' 
								and CreatedDate < '{2}' and CreatedDate >= '{3}') 
								as TotalCommissionFC,
                                
								(select SUM(Commission) from RescheduleCommission 
								where UserId='{0}' 
								and CreatedDate < '{2}' and CreatedDate >= '{3}')
								as TotalCommissionRC
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
                                        ReportCountQuery,//17
                                        TicketFilters.UserId
                                        );



                sqlQuery2 = string.Format(sqlQuery2, empid, CompanyId, previousWeekEnd, previousWeekStart);

                sqlQuery2 = sqlQuery2 + sqlQuery;
                using (SqlCommand cmd = GetSQLCommand(sqlQuery2))
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

        public bool CancellInvoiceByBookingId(string bookingId)
        {
            string SqlQuery = @"update Invoice set Status= 'Cancelled' 
                                where Status = 'Open' and BookingId = '{0}'
                                Update Invoice set BookingId = '' where BookingId = '{0}'";

            SqlQuery = string.Format(SqlQuery, bookingId);
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

        public DataSet GetLastweekspayListTechnician(Guid CompanyId, Guid empid)
        {

            DayOfWeek weekStart = DayOfWeek.Monday; // or Sunday, or whenever
            DateTime startingDate = DateTime.Today;

            while (startingDate.DayOfWeek != weekStart)
                startingDate = startingDate.AddDays(-1);

            DateTime previousWeekStart = startingDate.AddDays(-7);
            DateTime previousWeekEnd = startingDate.AddDays(0);

            string sqlQuery = @"select * from SalesCommission 
								where UserId='{0}'  
								and CreatedDate < '{2}' and CreatedDate >= '{3}'

                                select * from TechCommission 
								where UserId='{0}' 
								and CreatedDate < '{2}' and CreatedDate >= '{3}'
                                
								select * from AddMemberCommission 
								where UserId='{0}' 
								and CreatedDate < '{2}' and CreatedDate >= '{3}'
                                
								select * From FollowUpCommission 
								where UserId='{0}' 
								and CreatedDate < '{2}' and CreatedDate >= '{3}'
                                
								select * from RescheduleCommission 
								where UserId='{0}' 
								and CreatedDate < '{2}' and CreatedDate >= '{3}'
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, empid, CompanyId, previousWeekEnd, previousWeekStart);
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


        public DataTable GetDashBoardDataTechnicianByDate(Guid CompanyId, Guid empid, string firstdate, string lastdate)
        {
            DateTime FDate = firstdate.ToDateTime().ClientToUTCTime();
            if (FDate != new DateTime())
            {
                firstdate = FDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            DateTime LDate = lastdate.ToDateTime().SetMaxHour().ClientToUTCTime();
            if (LDate != new DateTime())
            {
                lastdate = LDate.ToString("yyyy-MM-dd HH:mm:ss");
            }

            string sqlQuery = @"select Distinct
                                (select COUNT(tu.UserId)
                                from TicketUser tu
                                LEFT JOIN Ticket t on t.TicketId=tu.TiketId
                                where tu.UserId='{0}'
                                and t.CompanyId='{1}'
                                and t.CreatedDate >= '{2}' and t.CompletionDate <= '{3}'
                                and (t.TicketType like '%Install%' or t.TicketType='Pick Up' or t.TicketType='Drop Off') and t.Status!='Completed' and t.Status != 'Closed') as OpenInstallationTicket,
                                (select COUNT(tu.UserId)
                                from TicketUser tu
                                LEFT JOIN Ticket t on t.TicketId=tu.TiketId
                                where tu.UserId='{0}'
                                and t.CompanyId='{1}'
                                and t.LastUpdatedDate >= '{2}' and t.CompletionDate <= '{3}'
                                and (t.TicketType like '%Install%' or t.TicketType='Pick Up' or t.TicketType='Drop Off') and t.Status='Completed' and t.Status != 'Closed') as ClosedInstallationTicket,
                                (select COUNT(tu.UserId)
                                from TicketUser tu
                                LEFT JOIN Ticket t on t.TicketId=tu.TiketId
                                where tu.UserId='{0}'
                                and t.CompanyId='{1}'
                                and t.CreatedDate >= '{2}' and t.CompletionDate <= '{3}'
                                and t.TicketType='Service' and t.Status!='Completed' and t.Status != 'Closed') as OpenServiceTicket,
                                (select COUNT(tu.UserId)
                                from TicketUser tu
                                LEFT JOIN Ticket t on t.TicketId=tu.TiketId
                                where tu.UserId='{0}'
                                and t.CompanyId='{1}'
                                and t.LastUpdatedDate >= '{2}' and t.CompletionDate <= '{3}'
                                and (t.TicketType not like '%Install%') and t.Status='Completed' and t.Status != 'Closed') as ClosedServiceTicket
                                from Ticket
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, empid, CompanyId, firstdate, lastdate);
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

        public DataTable GetDashBoardDataTechnicianUpsold(Guid userid)
        {
            var StartDate = DateTime.Now.AddDays(-30).ToString("MM/dd/yyyy");
            var EndDate = DateTime.Now.ToString("MM/dd/yyyy");
            string sqlQuery = @"select	(select COUNT(*) from CustomerAppointmentEquipment cae left join Equipment Equ 
								on cae.EquipmentId = Equ.EquipmentId where cae.IsService = 0 and cae.CreatedByUid = '{0}' and cae.IsAgreementItem ='0' and Equ.IsUpsold='1' and cae.CreatedDate between '{1}' and '{2}') as UpsoldEquipments,
                                (select SUM(TotalPrice) from CustomerAppointmentEquipment cae left join Equipment Equ 
								on cae.EquipmentId = Equ.EquipmentId where cae.IsService = 0 and cae.CreatedByUid = '{0}' and cae.IsAgreementItem ='0' and Equ.IsUpsold='1' and cae.CreatedDate between '{1}' and '{2}') as UpsoldEquipmentsTotalPrice,
                                (select SUM(Quantity) from CustomerAppointmentEquipment cae left join Equipment Equ 
								on cae.EquipmentId = Equ.EquipmentId where cae.IsService = 0 and cae.CreatedByUid = '{0}' and cae.IsAgreementItem ='0' and Equ.IsUpsold='1' and cae.CreatedDate between '{1}' and '{2}') as UpsoldEquipmentsTotalQuantity,
                                (select COUNT(*) from CustomerAppointmentEquipment cae where IsService = 1 and CreatedByUid = '{0}' and cae.CreatedDate between '{1}' and '{2}') as UpsoldServices,
                                (select SUM(TotalPrice) from CustomerAppointmentEquipment cae where IsService = 1 and CreatedByUid = '{0}' and cae.CreatedDate between '{1}' and '{2}') as UpsoldServicesTotalPrice,
                                (select SUM(Quantity) from CustomerAppointmentEquipment cae where IsService = 1 and CreatedByUid = '{0}' and cae.CreatedDate between '{1}' and '{2}') as UpsoldServicesTotalQuantity,
                                (select COUNT(cae.Id)) as TotalUpsold
                                from CustomerAppointmentEquipment cae
                                left join Employee emp on emp.UserId = cae.CreatedByUid
                                where emp.UserId = '{0}' and cae.CreatedDate between '{1}' and '{2}'
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, userid, StartDate, EndDate);
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

        public DataTable GetDashBoardDataTechnicianUpsoldByDate(Guid userid, string firstdate, string lastdate)
        {
            string sqlQuery = @"select	(select COUNT(*) from CustomerAppointmentEquipment cae left join Equipment Equ 
								on cae.EquipmentId = Equ.EquipmentId where cae.IsService = 0 and cae.CreatedByUid = '{0}' and cae.IsAgreementItem ='0' and Equ.IsUpsold='1' and cae.CreatedDate >= '{1}' and cae.CreatedDate <= '{2}') as UpsoldEquipments,
                                (select SUM(TotalPrice) from CustomerAppointmentEquipment cae left join Equipment Equ 
								on cae.EquipmentId = Equ.EquipmentId where cae.IsService = 0 and cae.CreatedByUid = '{0}' and cae.IsAgreementItem ='0' and Equ.IsUpsold='1' and cae.CreatedDate >= '{1}' and cae.CreatedDate <= '{2}') as UpsoldEquipmentsTotalPrice,
                                (select COUNT(*) from CustomerAppointmentEquipment where IsService = 1 and  CreatedByUid = '{0}' and CreatedDate >= '{1}' and CreatedDate <= '{2}') as UpsoldServices,
                                (select SUM(TotalPrice) from CustomerAppointmentEquipment where IsService = 1 and  CreatedByUid = '{0}' and CreatedDate >= '{1}' and CreatedDate <= '{2}') as UpsoldServicesTotalPrice,
                                (select COUNT(cae.Id)) as TotalUpsold
                                from CustomerAppointmentEquipment cae 
                                left join Employee emp on emp.UserId = cae.CreatedByUid
                                where emp.UserId = '{0}' and cae.CreatedDate >= '{1}' and cae.CreatedDate <= '{2}'
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, userid, firstdate, lastdate);
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

        public List<Invoice> GetUnpaidInvoiceDetailListByCustomerId(Guid customerId)
        {
            string sqlQuery = @"select * from Invoice 
                                where 
                                --CreatedBy not in ('SystemGenerated','System','Automated') and 
                                DueDate < GETDATE()
                                and BalanceDue > 0
                                and IsEstimate = 0
                                and [Status] not in('Cancelled','Rolled Over','Init','Paid')
                                and CustomerId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return GetList(cmd, -1);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<Invoice> GetUnpaidRecurringBillingInvoiceListByCustomerId(Guid customerId, Guid ComId)
        {
            string sqlQuery = @"select * from Invoice 
                               where IsARBInvoice = 1
                            --    and DueDate < GETDATE()
                                and BalanceDue > 0
                                and IsEstimate = 0
                                and [Status] not in('Cancelled','Rolled Over','Init','Paid')
                                and CustomerId = '{0}'
                                and CompanyId = '{1}'
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerId, ComId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return GetList(cmd, -1);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<Invoice> GetUnpaidOthersInvoiceListByCustomerId(Guid customerId, Guid ComId)
        {
            string sqlQuery = @"select * from Invoice 
                                where IsARBInvoice != 1
                            --    and DueDate < GETDATE()
                                and BalanceDue > 0
                                and IsEstimate = 0
                                and [Status] not in('Cancelled','Rolled Over','Init','Paid')
                                and CustomerId = '{0}'
                                and CompanyId = '{1}'
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerId, ComId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return GetList(cmd, -1);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<Invoice> GetInvoiceByTransactionId(string transactionId)
        {
            string sqlQuery = @"select inv.*
                                ,tr.CardTransactionId
                                ,tr.Id
                                from [Transaction] tr
                                left join TransactionHistory trh on tr.Id = trh.TransactionId
                                left join invoice inv on trh.InvoiceId = inv.Id 
                                where tr.CardTransactionId !=''
                                and tr.CardTransactionId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, transactionId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return GetList(cmd, -1);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<Invoice> GetInvoiceListByCardTransactionIdList(string transactionIds)
        {
            string sqlQuery = @"select inv.*
                                ,tr.CardTransactionId
                                ,tr.Id
                                from [Transaction] tr
                                left join TransactionHistory trh on tr.Id = trh.TransactionId
                                left join invoice inv on trh.InvoiceId = inv.Id 
                                where tr.CardTransactionId !=''
                                and tr.CardTransactionId in ({0})";
            try
            {
                sqlQuery = string.Format(sqlQuery, transactionIds);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return GetList(cmd, -1);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Invoice> GetInvoiceByAddedDateAndCustomerId(Guid customerId, DateTime lastGeneratedInvoice, bool AutomatedBilling)
        {
            //Invoice will be created for all customer
            string automatedBillingSql = "and (invoicefor ='ACH' or InvoiceFor = 'Credit Card')";
            if (!AutomatedBilling)
            {
                automatedBillingSql = "";
            }
            string sqlQuery = @"select * from Invoice
                                where CustomerId = '{0}'
                                {1}
                                and CreatedBy in('System','Automated')
                                and CreatedDate ='{2}'
                                and [Status] != 'init'
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerId, automatedBillingSql, lastGeneratedInvoice.ToString("yyyy-MM-dd HH:mm:ss"));
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return GetList(cmd, 10);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #region Report Invoice
        public DataTable GetAllInvoiceReportByCompanyId(Guid companyId, string Start, string End, string Status, string Type)
        {
            string sqlQuery = @"Declare @CompanyId uniqueidentifier
                                set @CompanyId ='{0}' 
                                select inv.InvoiceId 
                                ,cus.Title+' '+cus.FirstName+' '+cus.LastName as [Customer Name]
                                ,inv.TotalAmount as [Total Amount]
                                ,inv.BalanceDue as [Balance Due]
                                ,inv.CreatedDate as [Created Date]
                                from Invoice inv
                                left join Customer cus
                                on cus.CustomerId = inv.CustomerId
                                left join [Transaction] trans
								on trans.ReferenceNo = inv.InvoiceId
                                where inv.CompanyId = @CompanyId
                                and IsEstimate =0
                                ";
            string subQuery = "";
            if (!string.IsNullOrWhiteSpace(Start) && !string.IsNullOrWhiteSpace(End))
            {
                subQuery = string.Format(" and inv.Status != 'Init' and inv.CreatedDate between '{0}' and '{1}'", Start, End);
                sqlQuery += subQuery;
            }
            if (!string.IsNullOrWhiteSpace(Status) && Status != "-1")
            {
                if (Status == "Due")
                {
                    subQuery = string.Format(" and inv.Status = 'Open' and inv.DueDate < '{0}'", DateTime.Now);
                    sqlQuery += subQuery;
                }
                else
                {
                    subQuery = string.Format(" and inv.Status = '{0}'", Status);
                    sqlQuery += subQuery;
                }

            }
            if (!string.IsNullOrWhiteSpace(Type) && Type != "-1")
            {
                subQuery = string.Format(" and trans.PaymentMethod = '{0}'", Type);
                sqlQuery += subQuery;
            }
            if (string.IsNullOrWhiteSpace(Start) && string.IsNullOrWhiteSpace(End) && string.IsNullOrWhiteSpace(Status) && Status == "-1" && string.IsNullOrWhiteSpace(Type) && Type == "-1")
            {
                subQuery = string.Format(" and inv.Status != 'Init'");
                sqlQuery += subQuery;
            }
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

        //This method will call from scheduler
        public List<Invoice> GetExpiringEstimateListByCompanyIdAndDay(Guid CompanyId, int Day)
        {
            //2017-08-25 00:00:00.000
            DateTime today = DateTime.Now.UTCCurrentTime().AddDays(Day).SetMaxHour();
            string date = today.ToString("yyyy-MM-dd 23:mm:ss.000");
            string sqlQuery = @"select * from Invoice where IsEstimate = 1 
                                and Status ='sent to customer'
                                and DueDate ='{0}'
                                and CompanyId = '{1}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, date, CompanyId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    return GetList(cmd, -1);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
        public DataTable GetEstimateListByCompanyIdAndDates(Guid companyId, DateTime start, DateTime end)
        {
            string sqlQuery = @"select * from Invoice 
                                where  CompanyId = '{0}'
                                and [Status] !='Init'
                                and IsEstimate =1
                                and CreatedDate between {1}  
                                and {1}";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, start, end);
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
        public DataTable GetInvoiceReport(int[] IdList, string[] Colmuns, Guid CompanyId)
        {
            DataTable dt = new DataTable();
            string InIdListFilter = "";
            if (IdList != null && IdList.Length > 0)
            {
                string Ids = "";
                foreach (int id in IdList)
                {
                    Ids += id + ",";
                }
                Ids += "0";
                InIdListFilter = "And eq.Id in(" + Ids + ")";
            }
            string ColumnList = "";
            if (Colmuns != null && Colmuns.Length > 0)
            {
                foreach (string column in Colmuns)
                {
                    if (column == "Created On")
                    {
                        ColumnList += "eq.CreatedDate";
                    }
                    else if (column == "Invoice No")
                    {
                        ColumnList += ",eq.InvoiceId";
                    }
                    else if (column == "Description")
                    {
                        ColumnList += ",eq.Description";
                    }
                    else if (column == "Due Date")
                    {
                        ColumnList += ",eq.DueDate";
                    }
                    else if (column == "Total")
                    {
                        ColumnList += ",eq.TotalAmount";
                    }
                    else if (column == "Balance")
                    {
                        ColumnList += ",eq.BalanceDue";
                    }
                    else if (column == "Status")
                    {
                        ColumnList += ",eq.Status";
                    }

                }
            }
            string sqlQuery = @"
                            select {0} from Invoice eq
                           
                            where eq.CompanyId = '{1}'
                            {2}
                            ";
            try
            {
                sqlQuery = string.Format(sqlQuery, ColumnList, CompanyId, InIdListFilter);
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
            //return dt;
        }

        public DataTable GetTicketReport(Guid customerid, string[] Colmuns, Guid CompanyId)
        {
            DataTable dt = new DataTable();
            string ColumnList = "";
            if (Colmuns != null && Colmuns.Length > 0)
            {
                foreach (string column in Colmuns)
                {
                    if (column == "TicketId")
                    {
                        ColumnList += "tk.Id";
                    }
                    else if (column == "CustomerName")
                    {
                        ColumnList += ", cus.FirstName + ' ' + cus.LastName as CustomerName";
                    }
                    else if (column == "TicketType")
                    {
                        ColumnList += ",tk.TicketType";
                    }
                    else if (column == "Description")
                    {
                        ColumnList += ",tk.Message";
                    }
                    else if (column == "CreatedBy")
                    {
                        ColumnList += ", createdemp.FirstName + ' ' + createdemp.LastName as CreatedBy";
                    }
                    else if (column == "CreatedDate")
                    {
                        ColumnList += ", tk.CreatedDate";
                    }
                    else if (column == "Assigned")
                    {
                        ColumnList += ", assignemp.FirstName + ' ' + assignemp.LastName as Assigned";
                    }
                    else if (column == "ScheduleOn")
                    {
                        ColumnList += ", tk.CompletionDate";
                    }
                    else if (column == "Status")
                    {
                        ColumnList += ",tk.Status";
                    }

                }
            }
            string sqlQuery = @"
                            select {0} from Ticket tk
                            left join Customer cus on cus.CustomerId = tk.CustomerId
							left join Employee createdemp on createdemp.UserId = tk.CreatedBy
							left join TicketUser tu on tu.TiketId = tk.TicketId and tu.IsPrimary = 1
							left join Employee assignemp on assignemp.UserId = tu.UserId
                            where tk.CompanyId = '{1}'
                            and tk.CustomerId = '{2}'
                            ";
            try
            {
                sqlQuery = string.Format(sqlQuery, ColumnList, CompanyId, customerid);
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
            //return dt;
        }

        public DataTable GetEstimateReport(int[] IdList, string[] Colmuns, Guid CompanyId)
        {
            DataTable dt = new DataTable();
            string InIdListFilter = "";
            if (IdList != null && IdList.Length > 0)
            {
                string Ids = "";
                foreach (int id in IdList)
                {
                    Ids += id + ",";
                }
                Ids += "0";
                InIdListFilter = "And _Estimate.Id in(" + Ids + ")";
            }
            string ColumnList = "";
            if (Colmuns != null && Colmuns.Length > 0)
            {
                foreach (string column in Colmuns)
                {
                    if (column == "Estimate")
                    {
                        ColumnList += "_Estimate.InvoiceId";
                    }
                    else if (column == "Status")
                    {
                        ColumnList += ",_Estimate.Status";
                    }
                    else if (column == "User")
                    {
                        ColumnList += ",emp.FirstName + ' ' + emp.LastName as Name";
                    }
                    else if (column == "Date")
                    {
                        ColumnList += ",convert(date,_Estimate.LastUpdatedDate) as [Last Updated Date]";
                    }
                    else if (column == "Total")
                    {
                        ColumnList += ",_Estimate.TotalAmount";
                    }

                    else if (column == "LastNoteAdded")
                    {
                        ColumnList += ",convert(date,(select top 1 AddedDate from InvoiceNote where InvoiceId=_Estimate.Id order by  addedDate desc)) as NoteAddedDate";
                    }
                    else if (column == "Status")
                    {
                        ColumnList += ",eq.Status";
                    }


                }
            }
            string sqlQuery = @"
                            select {0} from Invoice _Estimate
                            left join Customer _Customer 
                            on _Estimate.CustomerId = _Customer.CustomerId
                            left join Employee emp
                            on emp.UserId = _Estimate.CreatedByUid
                            where _Estimate.CompanyId = '{1}'
                            {2}
                            ";
            try
            {
                sqlQuery = string.Format(sqlQuery, ColumnList, CompanyId, InIdListFilter);
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
            //return dt;
        }
        public DataTable GetAllDueInvoiceIdByCustomerId(Guid CustomerId, Guid CompanyId)
        {
            string sqlQuery = @"select invoice.InvoiceId
                                from Invoice invoice 
                                where invoice.CustomerId = '{0}'
                                and invoice.CompanyId = '{1}' 
                                and invoice.Status = 'Due'
                                and invoice.IsEstimate = 0";
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

        public DataTable GetInvoiceListByCompanyIdAndDates(Guid companyId, DateTime start, DateTime end)
        {
            string sqlQuery = @"select inv.Id
                                ,inv.CustomerId
                                ,inv.CompanyId
                                ,inv.CreatedBy
                                ,inv.BillingAddress
                                ,inv.Amount
                                ,inv.TotalAmount
                                ,inv.CreatedDate
                                ,inv.DiscountAmount
                                ,inv.DueDate
                                ,inv.InvoiceId
                                ,inv.IsEstimate
                                ,inv.[Status]
                                ,inv.Tax
                                ,cus.Title
                                ,cus.FirstName
                                ,cus.LastName 
                                from Invoice inv
                                left join Customer cus on cus.CustomerId = inv.CustomerId
                                where  inv.CompanyId = '{0}'
                                and inv.[Status] !='Init'
                                and inv.IsEstimate =0
                                and CreatedDate between '{1}'  
                                and '{2}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, start.ToString("yyyy-MM-dd HH:mm:ss.fff"), end.ToString("yyyy-MM-dd HH:mm:ss.fff"));
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

        public DataTable GetAllEstimateStatus(Guid CompanyId)
        {
            string sqlQuery = @"select (SELECT SUM([TotalAmount]) FROM Invoice WHERE [IsEstimate]=1) EstimateAmount, 
                                (SELECT SUM([TotalAmount]) FROM Invoice WHERE [Status]='Due') DueAmount, 
                                ISNULL((SELECT SUM([TotalAmount]) FROM Invoice WHERE [Status]='Paid'),0) PaidAmount
                                ";
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

        public List<Invoice> RabDataMigrationNewInvoiceList()
        {
            string Sql = "select * from Invoice where id > 6521";
            try
            {
                using (SqlCommand cmd = GetSQLCommand(Sql))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return GetList(cmd, -1);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllEstimateReportByCompanyId(Guid CompanyId, DateTime? Start, DateTime? End)
        {

            string sqlQuery = @"declare @CompanyId  uniqueidentifier
                                set @CompanyId = '{0}'
                                select inv.InvoiceId as [Estimate Id]
                                , cus.Title +' '+cus.FirstName+' '+cus.LastName as [Customer Name]
                                , inv.TotalAmount as [Total Amount]
                                , inv.CreatedDate as [Created Date]
                                from Invoice inv
                                left join Customer cus 
	                                on cus.CustomerId = inv.CustomerId

                                where inv.CompanyId = @CompanyId
                                and inv.IsEstimate = 1 
                                ";

            if (Start.HasValue && End.HasValue)
            {
                sqlQuery += @"and inv.CreatedDate between '{1}' 
                                and '{2}'";

                sqlQuery = string.Format(sqlQuery, CompanyId, Start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), End.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            }
            else
            {
                sqlQuery = string.Format(sqlQuery, CompanyId);
            }
            try
            {

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
        public DataTable GetAllEstimateByCompanyId(Guid companyId, DateTime? Start, DateTime? End)
        {
            string sqlQuery = @" Declare @CompanyId uniqueidentifier
                                set @CompanyId ='{0}' 
                                select inv.*
                                ,cu.Title
                                ,cu.FirstName
                                ,cu.LastName from Invoice inv 
                                left join Customer cu 
                                on cu.CustomerId = inv.CustomerId
                                where inv.CompanyId=@CompanyId
                                and inv.Status != 'Init'
                                and inv.IsEstimate = 1 
                                ";

            if (Start.HasValue & End.HasValue)
            {
                sqlQuery += @"and inv.CreatedDate between '{1}' and '{2}'";
                sqlQuery = string.Format(sqlQuery, companyId, Start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), End.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            else
            {
                sqlQuery = string.Format(sqlQuery, companyId);
            }
            try
            {

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
        public DataTable GetAllExportEstimateSentByCompanyId(Guid companyId, DateTime? Start, DateTime? End, string SearchText,string order)
        {
            string searchquery = "";
            string datequery = ""; 
            string orderquery1 = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(order) && order != "undefined")
            {

                if (order == "ascending/customername")
                { 
                    orderquery1 = "order by [Customer Name] asc";
                }
                else if (order == "descending/customername")
                { 
                    orderquery1 = "order by [Customer Name] desc";
                }
                else if (order == "ascending/estimateid")
                { 
                    orderquery1 = "order by [Estimate Id] asc";
                }
                else if (order == "descending/estimateid")
                { 
                    orderquery1 = "order by [Estimate Id] desc";
                }
                else if (order == "ascending/sentdate")
                { 
                    orderquery1 = "order by [Sent Date]  asc";
                }
                else if (order == "descending/sentdate")
                { 
                    orderquery1 = "order by [Sent Date]  desc";
                }


            }
            else
            { 
                orderquery1 = "order by [Estimate Id] desc";
            }
            #endregion
            string sqlQuery = @"   Declare @CompanyId uniqueidentifier
                                    set @CompanyId ='{0}' 
                                    select Distinct 
                                    cu.FirstName + ' ' + cu.LastName As [Customer Name]
                                    ,cu.Id as [Customer Id]
									,est.EstimatorId as [Estimate Id]
									,FORMAT(DATEADD(MI, 360, est.LastUpdatedDate),'M/d/yy') As [Sent Date]  
                                    from Estimator est 
                                    left join Customer cu on cu.CustomerId = est.CustomerId
                                    where est.CompanyId=@CompanyId
                                    and est.Status != 'Init'
                                    and est.Status = 'Sent To Customer'
                                    and cu.IsActive = 1 {1} {2} {3}
                                ";
            if (!string.IsNullOrWhiteSpace(SearchText) && SearchText != "undefined")
            {

                searchquery += string.Format("and (cu.FirstName like '%{0}%' or cu.LastName like '%{0}%' or cu.FirstName + ' ' + cu.LastName like '%{0}%' or est.EstimatorId like '%{0}%')", SearchText);
            }
            if (Start.HasValue && Start.Value != new DateTime() && End.HasValue && End.Value != new DateTime())
            {
                datequery += string.Format("and est.LastUpdatedDate between '{0}' and '{1}'", Start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), End.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, datequery, searchquery, orderquery1);
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

        public DataSet GetAllEstimateSentByCompanyId(Guid companyId, DateTime? Start, DateTime? End,string SearchText,string order,int pageno,int pagesize)
        {
            string searchquery = "";
            string datequery = "";
            string orderquery = ""; 
            #region Order
            if (!string.IsNullOrWhiteSpace(order) && order != "undefined")
            {

                if (order == "ascending/customername")
                {
                    orderquery = "order by CustomerName asc"; 
                }
                else if (order == "descending/customername")
                {
                    orderquery = "order by CustomerName desc"; 
                }
                else if (order == "ascending/estimateid")
                {
                    orderquery = "order by EstimatorId asc"; 
                }
                else if (order == "descending/estimateid")
                {
                    orderquery = "order by EstimatorId desc"; 
                }
                else if (order == "ascending/sentdate")
                {
                    orderquery = "order by LastUpdatedDate asc"; 
                }
                else if (order == "descending/sentdate")
                {
                    orderquery = "order by LastUpdatedDate desc"; 
                } 
            }
            else
            {
                orderquery = "order by EstimatorId desc"; 
            }
            #endregion
            string sqlQuery = @"    Declare @CompanyId uniqueidentifier
                                    Declare @pagestart int
                                    Declare @pageend int
                                    set @pagestart=(@pageno-1)* @pagesize 
                                    set @pageend = @pagesize 
                                    set @CompanyId ='{0}' 
                                    select Distinct 
                                    cu.FirstName + ' ' + cu.LastName As [CustomerName]
                                    ,cu.Id as [CustomerIntId]
									,est.EstimatorId as [EstimatorId]
									,est.LastUpdatedDate As [LastUpdatedDate] 
									,est.Id 
									,est.Status
                                    into #sentestimator from Estimator est 
                                    left join Customer cu on cu.CustomerId = est.CustomerId
                                    where est.CompanyId=@CompanyId
                                    and est.Status != 'Init'
                                    and est.Status = 'Sent To Customer'
                                    and cu.IsActive = 1 {1} {2} 

                                    select top(@pagesize) *  from #sentestimator
                                    where Id not in (Select TOP (@pagestart)  Id from #sentestimator {3})
                                    {3} 
 
								    Select  count(Id) as [TotalCount] from #sentestimator

								    Drop Table #sentestimator  ";
            if(!string.IsNullOrWhiteSpace(SearchText) && SearchText != "undefined")
            {
                
                searchquery += string.Format("and (cu.FirstName like '%{0}%' or cu.LastName like '%{0}%' or cu.FirstName + ' ' + cu.LastName like '%{0}%' or est.EstimatorId like '%{0}%')",SearchText);
            }
            if (Start.HasValue && Start.Value != new DateTime() && End.HasValue && End.Value != new DateTime())
            {
                datequery += string.Format("and est.LastUpdatedDate between '{0}' and '{1}'", Start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), End.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, datequery, searchquery, orderquery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                    //DataSet dsResult = GetDataSet(cmd);
                    //return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetAllEstimateStatusDetailByCustomerId(Guid Cusidval, bool? IsDeclinedAdded)
        {
            string DeclinedAddQuery = "";
            if (IsDeclinedAdded != null && IsDeclinedAdded == false)
            {
                DeclinedAddQuery = " and [Status] != 'Declined'";
            }
            string sqlQuery = @"Declare @WorkOrderPayment float
                                Declare @InvoicePayments float
                                DECLARE @CustomerId uniqueidentifier;
                                set @CustomerId = '{0}';                              

                               set @InvoicePayments = (select ISNULL ((select sum(th.Amout) from [TransactionHistory] th   left join [Transaction] tr on th.TransactionId = tr.Id
                                left join Invoice inv on inv.Id = th.InvoiceId
                                left join Customer cus on cus.CustomerId = inv.CustomerId
                                --left join Employee emp on emp.UserId = 'ac0ce890-bc5b-4c34-aab2-017af19bedf6'
                                left join Employee receivedBy on receivedBy.UserId = th.ReceivedBy
                                    and receivedBy.UserId !='00000000-0000-0000-0000-000000000000'

                                where  tr.CustomerId = @CustomerId),0))
                                set @WorkOrderPayment = (select ISNULL((select  sum(cad.CollectedAmount) from CustomerAppointment ca 
                                left join CustomerAppointmentDetail cad
                                on ca.AppointmentId = cad.AppointmentId
                                where AppointmentType ='WorkOrder' 
                                and CustomerId =@CustomerId),0) )
 
                                select (
                                SELECT sum([BalanceDue])  from Invoice where [Status]!='Paid' AND IsEstimate=0 and CustomerId =@CustomerId and [Status] != 'Init'
								and [Status] != 'Cancelled' and [Status] != 'Cancel' and [Status] != 'Declined' and [Status] != 'Rolled Over') EstimateAmountDetail,
                                (SELECT SUM([BalanceDue]) FROM Invoice WHERE [Status]='Open' AND IsEstimate=0 and CustomerId = @CustomerId and ([DueDate] <= getdate() or [DueDate] is null) ) DueAmountDetail,
                                (SELECT @WorkOrderPayment +@InvoicePayments) PaidAmountDetail,
                                (SELECT ISNULL(SUM(amount),0.00) from CustomerCredit where CustomerId = @CustomerId and (IsDeleted != 1 or IsDeleted is null)) as CustomerCredit,
                               -- ((select cast(ISNULL(SUM(amount),0.00) as decimal(10,2)) from CustomerCredit where CustomerId = @CustomerId AND Type = 'Credit') - (select cast(ISNULL(SUM(amount),0.00) as decimal(10,2)) from CustomerCredit where CustomerId = @CustomerId AND Type = 'Debit')) as CustomerCredit, 
                                (SELECT ISNULL((SELECT SUM([BalanceDue]) FROM Invoice WHERE IsEstimate=0 and CustomerId =@CustomerId and [Status] != 'Init'
								and [Status] != 'Cancelled' and [Status] != 'Cancel' and [Status] != 'Rolled Over' {1}),0)) UnpaidAmount";
            try
            {
                sqlQuery = string.Format(sqlQuery, Cusidval, DeclinedAddQuery);
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

        public DataTable GetInvoiceByKeyAndCompanyId(Guid companyId, string key, string emptag, Guid empid)
        {

            string sqlQuery = @"select top 30 inv.*
                                ,cus.Id as CustomerIntId
                                ,CASE
                                    WHEN cus.[Type]='Commercial' and cus.BusinessName != '' and cus.BusinessName is not null
		                                THEN cus.BusinessName 
                                    ELSE cus.FirstName +' '+ cus.LastName
                                END as CustomerName

                                from invoice inv
                                left join Customer cus on cus.CustomerId = inv.CustomerId
                                where inv.[Status] != 'init' 
                                and inv.IsEstimate = 0 
                                and inv.CompanyId = '{0}' 
                                and inv.InvoiceId like @SearchText /*'%{1}%'*/
                                {2}
                                ";


            string subquery = "";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, key, subquery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", key)));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetAllReceivePaymentsByCompanyIdAndFilter(Guid companyId, int pageNo, int pageSize, string searchBy, string searchText, string order, DateTime StartDate, DateTime EndDate)
        {
            string orderquery = "";
            string orderquery1 = "";
            string DateFilter = "";
            if (StartDate != new DateTime() && EndDate != new DateTime())
            {
                StartDate = StartDate.SetZeroHour().ClientToUTCTime();
                EndDate = EndDate.SetMaxHour().ClientToUTCTime();
                DateFilter = string.Format("and inv.CreatedDate between '{0}' and '{1}'", StartDate, EndDate);
            }
            string sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int
                                --DECLARE @SearchText nvarchar(50)
                                DECLARE @SearchBy nvarchar(50)

                                --SET @SearchText = '%{0}%'
                                SET @SearchBy = '%{1}%'
                                SET @pageno = {2} --default 1
                                SET @pagesize = {3} --default 10
                                SET @CompanyId = '{4}'

                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

 
                                select * into #InvoiceData 
                                    from (select 0 as Id 
	                                , inv.Id as InvoiceId,inv.CreatedDate as TransacationDate
	                                ,'Invoice' as [Type],InvoiceId InvoiceIdStr
	                                ,DueDate as InvoiceDueDate 
	                                ,BalanceDue as Balance
	                                ,TotalAmount as Amount
	                                ,inv.[Status]
	                                ,inv.CustomerId 
	                                ,cus.FirstName
	                                ,cus.LastName,
                                    cus.Id as CustomerIdValue,
                                    cus.BusinessName as CustomerBussinessName
	                                , CompanyId from invoice inv
	                                left join Customer cus on cus.CustomerId = inv.CustomerId

                                where IsBill =0 and IsEstimate =0 and inv.[Status] !='Init'
	                                and CompanyId = @CompanyId
                                    and cus.IsActive = 1
	                                and BalanceDue>0 {7}) a 

                                   select top(@pagesize) * into #Testtable from #InvoiceData
								    where InvoiceId not in (Select TOP (@pagestart)  InvoiceId from #InvoiceData {5})
                                    and (InvoiceIdStr like @SearchText or FirstName + ' '+ LastName like @SearchText)
	                                {6}
                                  
                                    select *  from #Testtable

							        select cast( sum(Balance) as decimal(10,2)) as TotalBalanceByPage,
						
									cast(sum(Amount)as decimal(10,2)) as TotalAmountByPage from #TestTable 

                                  --  SELECT TOP (@pagesize) * FROM #InvoiceData

                                 --  where   InvoiceId NOT IN(Select TOP (@pagestart) InvoiceId from #InvoiceData {5}) 
	                                --and (InvoiceIdStr like @SearchText or FirstName + ' '+ LastName like @SearchText)
	                                --{6}

                                    select CustomerId into #Customers from #InvoiceData group by CustomerId
	                             
	                                select sum(Balance) as TotalBalance
		                                ,sum(Amount) as TotalAmount
                                      ,(select count (CustomerId) from #Customers) as CustomerCount
		                             ,(select count(*)  from #InvoiceData) as [TotalCount]
		                             --,count (InvoiceId)  as [TotalCount]

                                  

		                                from  #InvoiceData
	                                      --where ((InvoiceId like @SearchText or FirstName + ' '+ LastName like @SearchText))

                                DROP TABLE #InvoiceData
                                DROP TABLE #Customers
                                DROP TABLE #Testtable
                                    ";
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {

                if (order == "ascending/date")
                {
                    orderquery = "order by #InvoiceData.[TransacationDate] asc";
                    orderquery1 = "order by [TransacationDate] asc";
                }
                else if (order == "descending/date")
                {
                    orderquery = "order by #InvoiceData.[TransacationDate] desc";
                    orderquery1 = "order by [TransacationDate] desc";
                }
                if (order == "ascending/customername")
                {
                    orderquery = "order by #InvoiceData.[CustomerBussinessName] asc";
                    orderquery1 = "order by [CustomerBussinessName] asc";
                }
                else if (order == "descending/customername")
                {
                    orderquery = "order by #InvoiceData.[CustomerBussinessName] desc";
                    orderquery1 = "order by [CustomerBussinessName] desc";
                }
                else if (order == "ascending/invoiceno")
                {
                    orderquery = "order by #InvoiceData.[InvoiceId] asc";
                    orderquery1 = "order by [InvoiceId] asc";
                }
                else if (order == "descending/invoiceno")
                {
                    orderquery = "order by #InvoiceData.[InvoiceId] desc";
                    orderquery1 = "order by [InvoiceId] desc";
                }
                else if (order == "ascending/duedate")
                {
                    orderquery = "order by #InvoiceData.[InvoiceDueDate] asc";
                    orderquery1 = "order by [InvoiceDueDate] asc";
                }
                else if (order == "descending/duedate")
                {
                    orderquery = "order by #InvoiceData.[InvoiceDueDate] desc";
                    orderquery1 = "order by [InvoiceDueDate] desc";
                }
                else if (order == "ascending/type")
                {
                    orderquery = "order by #InvoiceData.[Type] asc";
                    orderquery1 = "order by [Type] asc";
                }
                else if (order == "descending/type")
                {
                    orderquery = "order by #InvoiceData.[Type] desc";
                    orderquery1 = "order by [Type] desc";
                }
                else if (order == "ascending/balance")
                {
                    orderquery = "order by #InvoiceData.[Balance] asc";
                    orderquery1 = "order by [Balance] asc";
                }
                else if (order == "descending/balance")
                {
                    orderquery = "order by #InvoiceData.[Balance] desc";
                    orderquery1 = "order by [Balance] desc";
                }
                else if (order == "ascending/total")
                {
                    orderquery = "order by #InvoiceData.[Amount] asc";
                    orderquery1 = "order by [Amount] desc";
                }
                else if (order == "descending/total")
                {
                    orderquery = "order by #InvoiceData.[Amount] asc";
                    orderquery1 = "order by [Amount] asc";
                }
                else if (order == "ascending/status")
                {
                    orderquery = "order by #InvoiceData.[Status] asc";
                    orderquery1 = "order by [Status] asc";
                }
                else if (order == "descending/status")
                {
                    orderquery = "order by #InvoiceData.[Status] desc";
                    orderquery1 = "order by [Status] desc";
                }


            }
            else
            {
                orderquery = "order by #InvoiceData.[Id] desc";
                orderquery1 = "order by Id desc";
            }
            #endregion

            try
            {
                sqlQuery = string.Format(sqlQuery, searchText, searchBy, pageNo, pageSize, companyId, orderquery, orderquery1, DateFilter);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", searchText)));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable DownloadAllReceivePaymentsByCompanyIdAndFilter(Guid companyId, int? pageNo, int pageSize, string searchBy, string searchText, string order, DateTime StartDate, DateTime EndDate)
        {
            string orderquery = "";
            string orderquery1 = "";
            string DateFilter = "";
            if (StartDate != new DateTime() && EndDate != new DateTime())
            {
                StartDate = StartDate.SetZeroHour().ClientToUTCTime();
                EndDate = EndDate.SetMaxHour().ClientToUTCTime();
                DateFilter = string.Format("and inv.CreatedDate between '{0}' and '{1}'", StartDate, EndDate);
            }
            string sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                --DECLARE @SearchText nvarchar(50)
                                DECLARE @SearchBy nvarchar(50)

                                --SET @SearchText = '%{0}%'
                                SET @SearchBy = '%{1}%'
                                SET @CompanyId = '{4}'

 
                                select * into #InvoiceData 
                                    from (select
									FORMAT(inv.CreatedDate,'MM/dd/yyyy') as [Date]
									,CASE WHEN (cus.BusinessName = '' or cus.BusinessName is null) THEN  cus.FirstName +' '+ cus.LastName WHEN (cus.BusinessName != '' or cus.BusinessName is not null) THEN  cus.BusinessName end [Customer Name]
	                                ,InvoiceId as [Invoice No.]
	                                ,FORMAT(DueDate,'MM/dd/yyyy') as [Due Date]
									,cast( BalanceDue as decimal(10,2)) as Balance
									,cast( TotalAmount as decimal(10,2)) as Total
									,CASE WHEN inv.[Status] = 'Open' THEN  'Due' WHEN inv.[Status] != 'Open' THEN  inv.[Status] end [Status]
                                    from invoice inv
	                                left join Customer cus on cus.CustomerId = inv.CustomerId

                                where IsBill =0 and IsEstimate =0 and inv.[Status] !='Init'
	                                and CompanyId = @CompanyId
                                    and cus.IsActive = 1
	                                and BalanceDue>0 {7}
                                    and (InvoiceId like @SearchText or FirstName + ' '+ LastName like @SearchText)
--{6}
) a 

                                   select  * into #Testtable from #InvoiceData
                                    --where InvoiceId is not null {5}
								    --where InvoiceId not in (Select TOP (@pagestart)  InvoiceId from #InvoiceData {5})
                                    
	                                --{6} top(@pagesize)
                                  
                                    select *  from #Testtable


                                    --select CustomerId into #Customers from #InvoiceData group by CustomerId
	                             

                                DROP TABLE #InvoiceData
                                --DROP TABLE #Customers
                                DROP TABLE #Testtable
                                    ";
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {

                if (order == "ascending/date")
                {
                    orderquery = "order by #InvoiceData.[TransacationDate] asc";
                    orderquery1 = "order by [TransacationDate] asc";
                }
                else if (order == "descending/date")
                {
                    orderquery = "order by #InvoiceData.[TransacationDate] desc";
                    orderquery1 = "order by [TransacationDate] desc";
                }
                if (order == "ascending/customername")
                {
                    orderquery = "order by #InvoiceData.[CustomerBussinessName] asc";
                    orderquery1 = "order by [CustomerBussinessName] asc";
                }
                else if (order == "descending/customername")
                {
                    orderquery = "order by #InvoiceData.[CustomerBussinessName] desc";
                    orderquery1 = "order by [CustomerBussinessName] desc";
                }
                else if (order == "ascending/invoiceno")
                {
                    orderquery = "order by #InvoiceData.[InvoiceId] asc";
                    orderquery1 = "order by [InvoiceId] asc";
                }
                else if (order == "descending/invoiceno")
                {
                    orderquery = "order by #InvoiceData.[InvoiceId] desc";
                    orderquery1 = "order by [InvoiceId] desc";
                }
                else if (order == "ascending/duedate")
                {
                    orderquery = "order by #InvoiceData.[InvoiceDueDate] asc";
                    orderquery1 = "order by [InvoiceDueDate] asc";
                }
                else if (order == "descending/duedate")
                {
                    orderquery = "order by #InvoiceData.[InvoiceDueDate] desc";
                    orderquery1 = "order by [InvoiceDueDate] desc";
                }
                else if (order == "ascending/type")
                {
                    orderquery = "order by #InvoiceData.[Type] asc";
                    orderquery1 = "order by [Type] asc";
                }
                else if (order == "descending/type")
                {
                    orderquery = "order by #InvoiceData.[Type] desc";
                    orderquery1 = "order by [Type] desc";
                }
                else if (order == "ascending/balance")
                {
                    orderquery = "order by #InvoiceData.[Balance] asc";
                    orderquery1 = "order by [Balance] asc";
                }
                else if (order == "descending/balance")
                {
                    orderquery = "order by #InvoiceData.[Balance] desc";
                    orderquery1 = "order by [Balance] desc";
                }
                else if (order == "ascending/total")
                {
                    orderquery = "order by #InvoiceData.[Amount] asc";
                    orderquery1 = "order by [Amount] desc";
                }
                else if (order == "descending/total")
                {
                    orderquery = "order by #InvoiceData.[Amount] asc";
                    orderquery1 = "order by [Amount] asc";
                }
                else if (order == "ascending/status")
                {
                    orderquery = "order by #InvoiceData.[Status] asc";
                    orderquery1 = "order by [Status] asc";
                }
                else if (order == "descending/status")
                {
                    orderquery = "order by #InvoiceData.[Status] desc";
                    orderquery1 = "order by [Status] desc";
                }


            }
            else
            {
                orderquery = "order by #InvoiceData.[Id] desc";
                orderquery1 = "order by Id desc";
            }
            #endregion

            try
            {
                sqlQuery = string.Format(sqlQuery, searchText, searchBy, pageNo, pageSize, companyId, orderquery, orderquery1, DateFilter);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", searchText)));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable GetTotalRevenueByCustomerIdandCompanyId(Guid CustomerId, Guid CompanyId)
        {
            string sqlQuery = @"DECLARE @CustomerId uniqueidentifier;
                                set @CustomerId = '{0}';
                                select * into #tempinv from Invoice 
                                select ((select SUM(TotalAmount) from #tempinv where [IsEstimate]=1 and CustomerId=@CustomerId)   
                                - (select SUM(TotalAmount) from #tempinv where [Status]='Due' and CustomerId=@CustomerId)) TotalRevenue  
                                drop table #tempinv";
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

        public DataTable GetAllInvoiceByCompanyId(Guid companyId, string StartDate, string EndDate, string Status, string Type)
        {
            string sqlQuery = @"select inv.*,cu.Title,cu.FirstName,cu.LastName from Invoice inv 
                                left join Customer cu 
                                on cu.CustomerId = inv.CustomerId
                                left join [Transaction] trans
								on trans.ReferenceNo = inv.InvoiceId
                                where inv.CompanyId='{0}'
                                and inv.IsEstimate = 0";
            string subQuery = "";
            //string DateStart = StartDate.ToString("yyyy-MM-dd");
            //string DateEnd = EndDate.Value.ToString("yyyy-MM-dd");
            if (!string.IsNullOrWhiteSpace(StartDate) && !string.IsNullOrWhiteSpace(EndDate))
            {
                subQuery = string.Format(" and inv.Status != 'Init' and inv.CreatedDate between '{0}' and '{1}'", StartDate, EndDate);
                sqlQuery += subQuery;
            }
            if (!string.IsNullOrWhiteSpace(Status) && Status != "-1")
            {
                if (Status == "Due")
                {
                    subQuery = string.Format(" and inv.Status = 'Open' and inv.DueDate < '{0}'", DateTime.Now);
                    sqlQuery += subQuery;
                }
                else
                {
                    subQuery = string.Format(" and inv.Status = '{0}'", Status);
                    sqlQuery += subQuery;
                }

            }
            if (!string.IsNullOrWhiteSpace(Type) && Type != "-1")
            {
                subQuery = string.Format(" and trans.PaymentMethod = '{0}'", Type);
                sqlQuery += subQuery;
            }
            if (string.IsNullOrWhiteSpace(StartDate) && string.IsNullOrWhiteSpace(EndDate) && string.IsNullOrWhiteSpace(Status) && Status == "-1" && string.IsNullOrWhiteSpace(Type) && Type == "-1")
            {
                subQuery = string.Format(" and inv.Status != 'Init'");
                sqlQuery += subQuery;
            }
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

        public DataTable GetAllAutogeneratedUnpaidInvoiceByCompanyIdAndInvoiceFor(AllInvoicesFilter filter)
        {
            string InvoiceForSql = "";
            string CustomerFilterSql = "";
            string sortbySql = "";
            string StatusQuery = "Declined";

            if (!string.IsNullOrWhiteSpace(filter.InvoiceFor) && (filter.InvoiceFor.ToLower() == "systemgenerated"))
            {
                StatusQuery = "Open";
            }
            if (!string.IsNullOrWhiteSpace(filter.InvoiceFor) && (filter.InvoiceFor.ToLower() == "ach" || filter.InvoiceFor.ToLower() == "credit card"))
            {
                InvoiceForSql = string.Format("and inv.InvoiceFor ='{0}' ", filter.InvoiceFor);
                CustomerFilterSql = string.Format(@"and cu.paymentmethod='{0}' 
                                                    --and cu.AuthorizeRefId != '' ", filter.InvoiceFor);
            }
            else if (!string.IsNullOrWhiteSpace(filter.InvoiceFor) && filter.InvoiceFor.ToLower() == "systemgenerated")
            {
                InvoiceForSql = @"and inv.InvoiceFor in ('{0}' ,'{1}') ";
                InvoiceForSql = string.Format(InvoiceForSql, "SystemGenerated", "Invoice");
                CustomerFilterSql = string.Format("and cu.paymentmethod='{0}' ", "Invoice");
            }
            else
            {
                InvoiceForSql = @" and (inv.InvoiceFor !='ach')
                                  and inv.InvoiceFor != 'credit card'
                                  and inv.InvoiceFor != 'SystemGenerated'
                                  and inv.InvoiceFor != 'Invoice' ";

                CustomerFilterSql = @" and (cu.PaymentMethod !='ach' or (cu.PaymentMethod ='ach' and cu.AuthorizeRefId = '') )
                                      and ( cu.PaymentMethod != 'credit card' or (cu.PaymentMethod ='credit card' and cu.AuthorizeRefId = '') )
                                      and cu.PaymentMethod != 'Invoice' "
                                      ;
            }
            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                CustomerFilterSql += string.Format(" and inv.Status='{0}' ", filter.Status);
            }
            if (filter.PaymentFilter == "Cleared")
            {
                CustomerFilterSql += "and dbo.IsUnpaidUser(cu.BillDay,cu.FirstBilling,cu.LastGeneratedInvoice,cu.BillCycle)=0";
            }
            else if (filter.PaymentFilter == "Pending")
            {
                CustomerFilterSql += "and dbo.IsUnpaidUser(cu.BillDay,cu.FirstBilling,cu.LastGeneratedInvoice,cu.BillCycle)=1";
            }
            if (filter.IsTax == "true")
            {
                //InvoiceForSql += string.Format("and (inv.TaxType !='Non-Tax' and inv.TaxType !='') ");//tax will now check for customer
                CustomerFilterSql += "and cu.BillTax = 1 ";
            }
            else if (filter.IsTax == "false")
            {
                //InvoiceForSql += string.Format("and (inv.TaxType ='Non-Tax' or inv.TaxType ='') ");//tax will now check for customer
                CustomerFilterSql += "and cu.BillTax = 0 ";
            }

            if (!string.IsNullOrWhiteSpace(filter.BillingCycle) && filter.BillingCycle != "-1")
            {
                CustomerFilterSql += string.Format(@"and cu.BillCycle is not null 
                                                and cu.BillCycle = '{0}'", filter.BillingCycle);
            }
            if (filter.BillingDay > 0 && filter.BillingDay < 28)
            {
                CustomerFilterSql += string.Format(@"and cu.BillDay is not null
	                                             and cu.BillDay ='{0}'", filter.BillingDay);
            }
            string sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                DECLARE @SearchText nvarchar(50)
                                DECLARE @SearchBy nvarchar(50)

                                SET @SearchText = '%{3}%'
                                SET @SearchBy = '%{4}%'
                                SET @CompanyId = '{0}'


                                select * into #InvoiceData 
                                    from (
                                    
                                    select inv.*
                                    ,cu.Id as CusId
                                    from Invoice inv
                                    left join customer cu 
										on cu.CustomerId = inv.CustomerId
                                    left join CustomerCompany cc
										on cc.CustomerId = cu.CustomerId 
									 
	                                and inv.CreatedDate = cu.LastGeneratedInvoice
	                                and inv.Status != 'Init'
                                    and inv.InvoiceFor Is not null
                                     and inv.CreatedBy in ('System','Automated')
	                                /*Invoice Filters*/
                                    {5}
	                                where cc.CompanyId = @CompanyId
                                    and cu.IsActive=1
                                    and cc.IsLead=0
	                                /*CustomerFilters*/
                                    {6}
                                    ) a

                                SELECT *FROM #InvoiceData ORDER BY CusId DESC
                                    {7}
                                DROP TABLE #InvoiceData";
            sqlQuery = string.Format(sqlQuery,
                filter.CompanyId, //0
                filter.PageNo, //1
                filter.PageSize,//2
                filter.SearchText,//3
                filter.SearchBy,//4
                InvoiceForSql,//5
                CustomerFilterSql,//6
                sortbySql,//7
                StatusQuery//8
                );
            try
            {

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

        public DataSet GetAllInvoiceByCompanyIdAndFilter(AllInvoicesFilter filter)
        {

            /*Previously this query was selecting all invoices without payment method ach or credit card
             * now it will select only payment method of SystemGenerated and created by system
             * string InvoiceForSql = @"and inv.Status != 'Init'
                                    and (inv.InvoiceFor != 'ach' 
                                    and inv.InvoiceFor !='credit card' 
                                    or inv.InvoiceFor Is null)";*/
            string InvoiceForSql = "";
            string CustomerFilterSql = "";
            string sortbySql = "";
            string sortbySql1 = "";
            string StatusQuery = "Declined";

            if (!string.IsNullOrWhiteSpace(filter.InvoiceFor) && (filter.InvoiceFor.ToLower() == "systemgenerated"))
            {
                StatusQuery = "Open";
            }
            if (!string.IsNullOrWhiteSpace(filter.InvoiceFor) && (filter.InvoiceFor.ToLower() == "ach" || filter.InvoiceFor.ToLower() == "credit card"))
            {
                InvoiceForSql = string.Format("and inv.InvoiceFor ='{0}' ", filter.InvoiceFor);
                CustomerFilterSql = string.Format(@"and cu.paymentmethod='{0}'", filter.InvoiceFor);
            }
            else if (!string.IsNullOrWhiteSpace(filter.InvoiceFor) && filter.InvoiceFor.ToLower() == "systemgenerated")
            {
                InvoiceForSql = @"and inv.InvoiceFor in ('{0}' ,'{1}') ";
                InvoiceForSql = string.Format(InvoiceForSql, "SystemGenerated", "Invoice");
                CustomerFilterSql = string.Format("and cu.paymentmethod='{0}' ", "Invoice");
            }
            else
            {
                InvoiceForSql = @" and (inv.InvoiceFor !='ach')
                                  and inv.InvoiceFor != 'credit card'
                                  and inv.InvoiceFor != 'SystemGenerated'
                                  and inv.InvoiceFor != 'Invoice' ";

                CustomerFilterSql = @" and (cu.PaymentMethod !='ach' or (cu.PaymentMethod ='ach' and cu.AuthorizeRefId = '') )
                                      and ( cu.PaymentMethod != 'credit card' or (cu.PaymentMethod ='credit card' and cu.AuthorizeRefId = '') )
                                      and cu.PaymentMethod != 'Invoice' "
                                      ;
            }
            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                CustomerFilterSql += string.Format(" and inv.Status='{0}' ", filter.Status);
            }
            if (filter.PaymentFilter == "Cleared")
            {
                CustomerFilterSql += "and dbo.IsUnpaidUser(cu.BillDay,cu.FirstBilling,cu.LastGeneratedInvoice,cu.BillCycle)=0";
            }
            else if (filter.PaymentFilter == "Pending")
            {
                CustomerFilterSql += "and dbo.IsUnpaidUser(cu.BillDay,cu.FirstBilling,cu.LastGeneratedInvoice,cu.BillCycle)=1";
            }
            if (filter.IsTax == "true")
            {
                //InvoiceForSql += string.Format("and (inv.TaxType !='Non-Tax' and inv.TaxType !='') ");//tax will now check for customer
                CustomerFilterSql += "and cu.BillTax = 1 ";
            }
            else if (filter.IsTax == "false")
            {
                //InvoiceForSql += string.Format("and (inv.TaxType ='Non-Tax' or inv.TaxType ='') ");//tax will now check for customer
                CustomerFilterSql += "and cu.BillTax = 0 ";
            }

            if (!string.IsNullOrWhiteSpace(filter.BillingCycle) && filter.BillingCycle != "-1")
            {
                CustomerFilterSql += string.Format(@"and cu.BillCycle is not null 
                                                and cu.BillCycle = '{0}'", filter.BillingCycle);
            }
            if (filter.BillingDay > 0 && filter.BillingDay < 28)
            {
                CustomerFilterSql += string.Format(@"and cu.BillDay is not null
	                                             and cu.BillDay ='{0}'", filter.BillingDay);
            }

            #region Order
            if (!string.IsNullOrWhiteSpace(filter.order))
            {
                if (filter.order == "ascending/customername")
                {
                    sortbySql = "order by #InvoiceData.[Id] asc";
                    sortbySql1 = "order by [Id] asc";
                }
                else if (filter.order == "descending/customername")
                {
                    sortbySql = "order by #InvoiceData.[Id] desc";
                    sortbySql1 = "order by [Id] desc";
                }
                else if (filter.order == "ascending/billamount")
                {
                    sortbySql = "order by #InvoiceData.[TotalAmount] asc";
                    sortbySql1 = "order by [TotalAmount] asc";
                }
                else if (filter.order == "descending/billamount")
                {
                    sortbySql = "order by #InvoiceData.[TotalAmount] desc";
                    sortbySql1 = "order by [TotalAmount] desc";
                }
                else if (filter.order == "ascending/billcycle")
                {
                    sortbySql = "order by #InvoiceData.[BillingCycle] asc";
                    sortbySql1 = "order by [BillingCycle] asc";
                }
                else if (filter.order == "descending/billcycle")
                {
                    sortbySql = "order by #InvoiceData.[BillingCycle] desc";
                    sortbySql1 = "order by [BillingCycle] desc";
                }
                else if (filter.order == "ascending/settlementdate")
                {
                    sortbySql = "order by #InvoiceData.[Description] asc";
                    sortbySql1 = "order by [Description] asc";
                }
                else if (filter.order == "descending/settlementdate")
                {
                    sortbySql = "order by #InvoiceData.[Description] desc";
                    sortbySql1 = "order by [Description] desc";
                }
                else if (filter.order == "ascending/billeddate")
                {
                    sortbySql = "order by #InvoiceData.[DueDate] asc";
                    sortbySql1 = "order by [DueDate] desc";
                }
                else if (filter.order == "descending/billeddate")
                {
                    sortbySql = "order by #InvoiceData.[DueDate] asc";
                    sortbySql1 = "order by [DueDate] asc";
                }
                else if (filter.order == "ascending/duedate")
                {
                    sortbySql = "order by #InvoiceData.[TotalAmount] asc";
                    sortbySql1 = "order by [TotalAmount] asc";
                }
                else if (filter.order == "descending/duedate")
                {
                    sortbySql = "order by #InvoiceData.[TotalAmount] desc";
                    sortbySql1 = "order by [TotalAmount] desc";
                }
                else if (filter.order == "ascending/currentbillamount")
                {
                    sortbySql = "order by #InvoiceData.[CurrentBilledAmount] asc";
                    sortbySql1 = "order by [CurrentBilledAmount] asc";
                }
                else if (filter.order == "descending/currentbillamount")
                {
                    sortbySql = "order by #InvoiceData.[CurrentBilledAmount] desc";
                    sortbySql1 = "order by [CurrentBilledAmount] desc";
                }
                else if (filter.order == "ascending/pastdueamount")
                {
                    sortbySql = "order by #InvoiceData.[PastDueAmount]  asc";
                    sortbySql1 = "order by PastDueAmount asc";
                }
                else if (filter.order == "descending/pastdueamount")
                {
                    sortbySql = "order by #InvoiceData.[PastDueAmount]  desc";
                    sortbySql1 = "order by PastDueAmount desc";
                }
                else if (filter.order == "ascending/declineddate")
                {
                    sortbySql = "order by #InvoiceData.[ReturnedDate]  asc";
                    sortbySql1 = "order by ReturnedDate asc";
                }
                else if (filter.order == "descending/declineddate")
                {
                    sortbySql = "order by #InvoiceData.[ReturnedDate]  desc";
                    sortbySql1 = "order by ReturnedDate desc";
                }

                else if (filter.order == "ascending/returneddate")
                {
                    // sortbySql = "order by #InvoiceData.[ReturnedDate]  asc";
                    //sortbySql1 = "order by ReturnedDate asc";
                }
                else if (filter.order == "descending/returneddate")
                {
                    //sortbySql = "order by #InvoiceData.[ReturnedDate]  desc";
                    //sortbySql1 = "order by ReturnedDate desc";
                }

                else if (filter.order == "ascending/totalrev")
                {
                    // sortbySql = "order by #InvoiceData.[InvoiceNoteAddedDate]  asc";
                    // sortbySql1 = "order by InvoiceNoteAddedDate asc";
                }
                else if (filter.order == "descending/totalrev")
                {
                    // sortbySql = "order by #InvoiceData.[InvoiceNoteAddedDate]  desc";
                    //sortbySql1 = "order by InvoiceNoteAddedDate desc";
                }

                else if (filter.order == "ascending/acctno")
                {
                    sortbySql = "order by #InvoiceData.[CustomerNo]  asc";
                    sortbySql1 = "order by CustomerNo asc";
                }
                else if (filter.order == "descending/acctno")
                {
                    sortbySql = "order by #InvoiceData.[CustomerNo]  desc";
                    sortbySql1 = "order by CustomerNo desc";
                }



                else if (filter.order == "ascending/createddate")
                {
                    sortbySql = "order by #InvoiceData.[CreatedDate]  asc";
                    sortbySql1 = "order by CreatedDate asc";
                }
                else if (filter.order == "descending/createddate")
                {
                    sortbySql = "order by #InvoiceData.[CreatedDate]  desc";
                    sortbySql1 = "order by CreatedDate desc";
                }



            }
            else
            {
                sortbySql = "order by #InvoiceData.[Id] desc";
                sortbySql1 = "order by Id desc";
            }
            #endregion

            string sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int
                                DECLARE @SearchText nvarchar(50)
                                DECLARE @SearchBy nvarchar(50)

                                SET @SearchText = '%{3}%'
                                SET @SearchBy = '%{4}%'
                                SET @pageno = {1} --default 1
                                SET @pagesize = {2} --default 10
                                SET @CompanyId = '{0}'

                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

                                select * into #InvoiceData 
                                    from (
                                    
                                    select cu.Id
                                    ,cu.CustomerNo
	                                ,inv.BalanceDue
	                                ,inv.CreatedBy
	                                ,inv.Amount
	                                ,inv.TotalAmount
	                                ,cc.CompanyId
	                                ,cu.CustomerId
	                                ,inv.CreatedDate
	                                ,cu.Title
	                                ,cu.FirstName
	                                ,cu.LastName
	                                ,inv.DiscountAmount
	                                ,inv.DiscountCode
	                                ,inv.DueDate
	                                ,inv.InvoiceId
	                                ,inv.IsEstimate
	                                ,inv.Status
	                                ,inv.Tax
	                                ,cu.AuthorizeRefId as AuthRefId
	                                ,cu.BusinessName as CustomerBussinessName
	                                ,cu.BillCycle as BillingCycle
	                                ,inv.LateFee
	                                ,inv.LateAmount	
	                                ,inv.InvoiceFor
	                                ,cu.PaymentMethod
                                    ,cu.FirstBilling
									,cu.BillDay
                                    ,ISNULL(NULLIF(cu.MonthlyMonitoringFee,''),0) as MonthlyMonitoringFee
                                    ,(select sum(TotalAmount) from Invoice invinner
									where cu.CustomerId = invinner.CustomerId 
									and (invinner.Status='Open' or invinner.Status='sent to customer')
									and invinner.DueDate < GETDATE()
									) PastDueAmount,
									(select TotalAmount from Invoice invcrnt
									where cu.CustomerId = invcrnt.CustomerId
                                    and inv.Invoiceid=invcrnt.Invoiceid
									and (invcrnt.Status='Open')
									and invcrnt.DueDate >= GETDATE()
									) CurrentBilledAmount,
                                    cu.CreatedDate as CustomerCreatedDate,
                                    dectr.ReturnedDate
                                    from customer cu
                                    left join DeclinedTransactions dectr
										on dectr.CustomerId = cu.CustomerId 
                                    left join CustomerCompany cc
										on cc.CustomerId = cu.CustomerId 
                                    left join Invoice inv 
										on cu.CustomerId = inv.CustomerId
	                                and inv.CreatedDate = cu.LastGeneratedInvoice
	                                and inv.Status != 'Init'
                                    and inv.InvoiceFor Is not null
                                     and inv.CreatedBy in ('System','Automated')
	                                /*Invoice Filters*/
                                    {5}
	                                where cc.CompanyId = @CompanyId
                                    and cu.IsActive=1
                                    and cc.IsLead=0
	                                /*CustomerFilters*/
                                    {6}
                                    ) a

                                SELECT TOP (@pagesize) * FROM #InvoiceData
                                    where   Id NOT IN(Select TOP (@pagestart) Id from #InvoiceData {7}) 
	                                and (InvoiceId like @SearchText or FirstName + ' '+ LastName like @SearchText or AuthRefId like @SearchText )
                                    {9}

	                                 select 
										(select count(*) from #InvoiceData 
										where (InvoiceId like @SearchText or FirstName + ' '+ LastName like @SearchText or AuthRefId like @SearchText ))
									    as TotalCustomerAll,
										(select sum(CONVERT(float, MonthlyMonitoringFee)) from #InvoiceData
										where (InvoiceId like @SearchText or FirstName + ' '+ LastName like @SearchText or AuthRefId like @SearchText ))
									    as TotalAmountAll,
										(select count(*) from #InvoiceData
										 where BillingCycle='Monthly'
										 and (InvoiceId like @SearchText or FirstName + ' '+ LastName like @SearchText or AuthRefId like @SearchText )
										 ) as MonthlyCustomer,
										(select sum(Amount) from #InvoiceData 
										where BillingCycle='Monthly'
										and (InvoiceId like @SearchText or FirstName + ' '+ LastName like @SearchText or AuthRefId like @SearchText )
										 ) as MonthlyAmount,
										 (select count(*) from #InvoiceData
										 where Status='{8}'
										 and (InvoiceId like @SearchText or FirstName + ' '+ LastName like @SearchText or AuthRefId like @SearchText )
										 ) as  ReturnedCustomer,
										(select sum(Amount) from #InvoiceData 
										where Status='{8}'
										and (InvoiceId like @SearchText or FirstName + ' '+ LastName like @SearchText or AuthRefId like @SearchText )
										 ) as  ReturnedCustomerAmount,
		                                (select count(*)  from #InvoiceData
	                                where ((InvoiceId like @SearchText or FirstName + ' '+ LastName like @SearchText or AuthRefId like @SearchText))) as [TotalCount]
		                                ,sum(BalanceDue) as TotalBalance
		                                ,sum(TotalAmount) as TotalAmount
										from  #InvoiceData
                                DROP TABLE #InvoiceData";
            sqlQuery = string.Format(sqlQuery,
                filter.CompanyId, //0
                filter.PageNo, //1
                filter.PageSize,//2
                filter.SearchText,//3
                filter.SearchBy,//4
                InvoiceForSql,//5
                CustomerFilterSql,//6
                sortbySql,//7
                StatusQuery,//8
                sortbySql1//9
                );
            try
            {

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


        public DataSet GetAllInvoiceByCompanyIdForReport(AllInvoicesFilter filter)
        {
            string InvoiceForSql = "";
            string CustomerFilterSql = "";
            string sortbySql = "";
            string StatusQuery = "Declined";

            if (!string.IsNullOrWhiteSpace(filter.InvoiceFor) && (filter.InvoiceFor.ToLower() == "systemgenerated"))
            {
                StatusQuery = "Open";
            }
            if (!string.IsNullOrWhiteSpace(filter.InvoiceFor) && (filter.InvoiceFor.ToLower() == "ach" || filter.InvoiceFor.ToLower() == "credit card"))
            {
                InvoiceForSql = string.Format("and inv.InvoiceFor ='{0}' ", filter.InvoiceFor);
                CustomerFilterSql = string.Format(@"and cu.paymentmethod='{0}' 
                                                    --and cu.AuthorizeRefId != '' ", filter.InvoiceFor);
            }
            else if (!string.IsNullOrWhiteSpace(filter.InvoiceFor) && filter.InvoiceFor.ToLower() == "systemgenerated")
            {
                InvoiceForSql = @"and inv.InvoiceFor in ('{0}' ,'{1}') ";
                InvoiceForSql = string.Format(InvoiceForSql, "SystemGenerated", "Invoice");
                CustomerFilterSql = string.Format("and cu.paymentmethod='{0}' ", "Invoice");
            }
            else
            {
                InvoiceForSql = @" and (inv.InvoiceFor !='ach')
                                  and inv.InvoiceFor != 'credit card'
                                  and inv.InvoiceFor != 'SystemGenerated'
                                  and inv.InvoiceFor != 'Invoice' ";

                CustomerFilterSql = @" and (cu.PaymentMethod !='ach' or (cu.PaymentMethod ='ach' and cu.AuthorizeRefId = '') )
                                      and ( cu.PaymentMethod != 'credit card' or (cu.PaymentMethod ='credit card' and cu.AuthorizeRefId = '') )
                                      and cu.PaymentMethod != 'Invoice' "
                                      ;
            }

            if (filter.PaymentFilter == "Cleared")
            {
                CustomerFilterSql += "and dbo.IsUnpaidUser(cu.BillDay,cu.FirstBilling,cu.LastGeneratedInvoice,cu.BillCycle)=0";
            }
            else if (filter.PaymentFilter == "Pending")
            {
                CustomerFilterSql += "and dbo.IsUnpaidUser(cu.BillDay,cu.FirstBilling,cu.LastGeneratedInvoice,cu.BillCycle)=1";
            }
            if (filter.IsTax == "true")
            {
                //InvoiceForSql += string.Format("and (inv.TaxType !='Non-Tax' and inv.TaxType !='') ");//tax will now check for customer
                CustomerFilterSql += "and cu.BillTax = 1 ";
            }
            else if (filter.IsTax == "false")
            {
                //InvoiceForSql += string.Format("and (inv.TaxType ='Non-Tax' or inv.TaxType ='') ");//tax will now check for customer
                CustomerFilterSql += "and cu.BillTax = 0 ";
            }

            if (!string.IsNullOrWhiteSpace(filter.BillingCycle) && filter.BillingCycle != "-1")
            {
                CustomerFilterSql += string.Format(@"and cu.BillCycle is not null 
                                                and cu.BillCycle = '{0}'", filter.BillingCycle);
            }
            if (filter.BillingDay > 0 && filter.BillingDay < 28)
            {
                CustomerFilterSql += string.Format(@"and cu.BillDay is not null
	                                             and cu.BillDay ='{0}'", filter.BillingDay);
            }
            #region Sorting
            if (!string.IsNullOrWhiteSpace(filter.SortBy) && filter.SortBy != "-1")
            {
                if (filter.SortOrder.ToLower() != "asc")
                {
                    filter.SortOrder = "desc";
                }

                if (filter.SortBy.ToLower() == "invoiceid")
                {
                    sortbySql = string.Concat("ORDER BY InvoiceId ", filter.SortOrder);
                }
                else if (filter.SortBy.ToLower() == "customername")
                {
                    sortbySql = string.Concat("ORDER BY FirstName +' '+LastName ", filter.SortOrder);
                }
                else if (filter.SortBy.ToLower() == "billcycle")
                {
                    sortbySql = string.Concat("ORDER BY BillingCycle ", filter.SortOrder);
                }
                else if (filter.SortBy.ToLower() == "billamount")
                {
                    sortbySql = string.Concat("ORDER BY Amount ", filter.SortOrder);
                }
                else if (filter.SortBy.ToLower() == "pastdueamount")
                {
                    sortbySql = string.Concat("ORDER BY BalanceDue ", filter.SortOrder);
                }

                else if (filter.SortBy.ToLower() == "totaldue")
                {
                    sortbySql = string.Concat("ORDER BY TotalAmount ", filter.SortOrder);
                }

                else if (filter.SortBy.ToLower() == "duedate")
                {
                    sortbySql = string.Concat("ORDER BY DueDate ", filter.SortOrder);
                }
                else if (filter.SortBy.ToLower() == "billday")
                {
                    sortbySql = string.Concat("ORDER BY BillDay ", filter.SortOrder);
                }
                else if (filter.SortBy.ToLower() == "subscriptionstartday")
                {
                    sortbySql = string.Concat("ORDER BY FirstBilling ", filter.SortOrder);
                }

            }
            #endregion


            string sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                DECLARE @SearchText nvarchar(50)
                                DECLARE @SearchBy nvarchar(50)

                                SET @SearchText = '%{3}%'
                                SET @SearchBy = '%{4}%'
                                SET @CompanyId = '{0}'


                                select * into #InvoiceData 
                                    from (
                                    
                                    select cu.Id
	                                ,inv.BalanceDue
	                                ,inv.CreatedBy
	                                ,inv.Amount
	                                ,inv.TotalAmount
	                                ,cc.CompanyId
	                                ,cu.CustomerId
	                                ,inv.CreatedDate
	                                ,cu.Title
	                                ,cu.FirstName
	                                ,cu.LastName
	                                ,inv.DiscountAmount
	                                ,inv.DiscountCode
	                                ,inv.DueDate
	                                ,inv.InvoiceId
	                                ,inv.IsEstimate
	                                ,inv.Status
	                                ,inv.Tax
	                                ,cu.AuthorizeRefId as AuthRefId
	                                ,cu.BusinessName as CustomerBussinessName
	                                ,cu.BillCycle as BillingCycle
	                                ,inv.LateFee
	                                ,inv.LateAmount	
	                                ,inv.InvoiceFor
	                                ,cu.PaymentMethod
                                    ,cu.FirstBilling
									,cu.BillDay
                                    ,ISNULL(NULLIF(cu.MonthlyMonitoringFee,''),0) as MonthlyMonitoringFee
                                    from customer cu
 
                                    left join CustomerCompany cc
										on cc.CustomerId = cu.CustomerId 
                                    left join Invoice inv 
										on cu.CustomerId = inv.CustomerId
									 
	                                and inv.CreatedDate = cu.LastGeneratedInvoice
	                                and inv.Status != 'Init'
                                    and inv.InvoiceFor Is not null
                                     and inv.CreatedBy in ('System','Automated')
	                                /*Invoice Filters*/
                                    {5}
	                                where cc.CompanyId = @CompanyId
                                    and cu.IsActive=1
                                    and cc.IsLead=0
	                                /*CustomerFilters*/
                                    {6}
                                    ) a

                                SELECT *FROM #InvoiceData
                                    where (InvoiceId like @SearchText or FirstName + ' '+ LastName like @SearchText or AuthRefId like @SearchText )
	                                --ORDER BY Id DESC
                                    {7}
                                DROP TABLE #InvoiceData";
            sqlQuery = string.Format(sqlQuery,
                filter.CompanyId, //0
                filter.PageNo, //1
                filter.PageSize,//2
                filter.SearchText,//3
                filter.SearchBy,//4
                InvoiceForSql,//5
                CustomerFilterSql,//6
                sortbySql,//7
                StatusQuery//8
                );
            try
            {

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

        public DataTable GetTotalInvoiceByCompanyId(AllInvoicesFilter filter)
        {

            /*Previously this query was selecting all invoices without payment method ach or credit card
             * now it will select only payment method of SystemGenerated and created by system
             * string InvoiceForSql = @"and inv.Status != 'Init'
                                    and (inv.InvoiceFor != 'ach' 
                                    and inv.InvoiceFor !='credit card' 
                                    or inv.InvoiceFor Is null)";*/
            string InvoiceForSql = "";
            string datequery = "";
            if (filter.StartDate != new DateTime() && filter.EndDate != new DateTime())
            {
                //2018-07-30 12:50:07.463
                datequery = string.Format("and inv.CreatedDate between '{0}' and '{1}'", filter.StartDate.ToString("yyyy-MM-dd HH:mm:ss"), filter.EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (!string.IsNullOrWhiteSpace(filter.InvoiceFor) && (filter.InvoiceFor.ToLower() == "ach" || filter.InvoiceFor.ToLower() == "credit card"))
            {
                InvoiceForSql = string.Format("and inv.InvoiceFor ='{0}' ", filter.InvoiceFor);

            }
            else if (!string.IsNullOrWhiteSpace(filter.InvoiceFor) && filter.InvoiceFor.ToLower() == "systemgenerated")
            {
                InvoiceForSql = @"and inv.InvoiceFor in ('{0}' ,'{1}') ";
                InvoiceForSql = string.Format(InvoiceForSql, "SystemGenerated", "Invoice");

            }
            else
            {
                InvoiceForSql = @" and (inv.InvoiceFor !='ach')
                                  and inv.InvoiceFor != 'credit card'
                                  and inv.InvoiceFor != 'SystemGenerated'
                                  and inv.InvoiceFor != 'Invoice' ";


                ;
            }
            string sqlQuery = @"select
	                               sum(inv.TotalAmount) as TotalAmount,COUNT(cu.Id) as TotalCustomer
                                    from customer cu
 
                                    left join CustomerCompany cc
										on cc.CustomerId = cu.CustomerId 
                                    left join Invoice inv 
										on cu.CustomerId = inv.CustomerId
									 
	                                and inv.CreatedDate = cu.LastGeneratedInvoice
	                                and inv.Status != 'Init'
                                    and inv.InvoiceFor Is not null
                                     and inv.CreatedBy in ('System','Automated')
	                                /*Invoice Filters*/
                                    {5}
	                                where cc.CompanyId = '{0}'
                                    and cu.IsActive=1
                                    and cc.IsLead=0
                                    {6}
	                              
                                    ";
            sqlQuery = string.Format(sqlQuery,
                filter.CompanyId, //0
                filter.PageNo, //1
                filter.PageSize,//2
                filter.SearchText,//3
                filter.SearchBy,//4
                InvoiceForSql,//5
                datequery//6

                );
            try
            {

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
        public DataTable GetAllInvoicesByfilterDownload(AllInvoicesFilter filter, string BillicycleIdList, string InvoicestatusIdList)
        {
            string subquery = "";
            string datequery = "";
            string searchText = "";
            string BillingCycle = "";
            string statusquery = "";
            if (BillicycleIdList == "null")
            {
                BillicycleIdList = BillicycleIdList.Substring(0, BillicycleIdList.Length - 4);


            }
            if (InvoicestatusIdList == "null")
            {
                InvoicestatusIdList = InvoicestatusIdList.Substring(0, InvoicestatusIdList.Length - 4);


            }
            var array = BillicycleIdList.Split(",");
            string query = "";
            if (array != null)
            {
                foreach (var item in array)
                {
                    query += string.Format("'{0}',", item);
                }
                query = query.Remove(query.Length - 1, 1);
            }
            var array1 = InvoicestatusIdList.Split(",");
            string query1 = "";
            if (array1 != null)
            {
                foreach (var item in array1)
                {
                    query1 += string.Format("'{0}',", item);
                }
                query1 = query1.Remove(query1.Length - 1, 1);
            }
            if (filter.StartDate != new DateTime() && filter.EndDate != new DateTime())
            {
                //2018-07-30 12:50:07.463
                datequery = string.Format("and inv.CreatedDate between '{0}' and '{1}'", filter.StartDate.ToString("yyyy-MM-dd HH:mm:ss"), filter.EndDate.ToString("yyyy-MM-dd  HH:mm:ss"));
            }
            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                searchText = string.Format(@"and (cus.FirstName + ' '+cus.LastName like '%{0}%' or cus.BusinessName like '%{0}%'
                                            or inv.InvoiceId like '%{0}%' 
                                            or inv.TotalAmount like '%{0}%'  
                                            or inv.[description] like '%{0}%'
                                            or cus.AuthorizeRefId like '%{0}%')", filter.SearchText.Replace("'", "''"));
            }
            //if (!string.IsNullOrEmpty(filter.BillingCycle) && filter.BillingCycle != "-1")
            //{
            //    BillingCycle = string.Format("and BillingCycle = '{0}'", filter.BillingCycle);
            //}
            //if (!string.IsNullOrWhiteSpace(filter.Status) && filter.Status != "-1")
            //{
            //    statusquery = string.Format("and inv.[Status] = '{0}'", filter.Status);
            //}
            if (!string.IsNullOrWhiteSpace(query1))
            {
                statusquery = string.Format("and lktype.DataValue in ({0})", query1);
            }
            if (!string.IsNullOrWhiteSpace(query))
            {
                BillingCycle = string.Format("and BillingCycle in ({0})", query);
            }

            string sqlQuery = @" DECLARE @pagestart int
                            DECLARE @pageend int
                            DECLARE @pageno int
                            DECLARE @pagesize int

                            DECLARE @SearchText nvarchar(50)
                            DECLARE @SearchBy nvarchar(50)

                            Declare @PaymentMethod nvarchar(50)
                            SET @PaymentMethod = '{0}'

                            SET @SearchText = '%%'
                            SET @SearchBy = '%%'
                            SET @pageno = {1} --default 1
                            SET @pagesize = {2} --default 10 

                            SET @pagestart=(@pageno-1)* @pagesize 
                            SET @pageend = @pagesize

                             select inv.Id,
							cus.Id as [Customer Id],
							cus.FirstName + ' '+cus.LastName as [Customer Name],
				            inv.InvoiceId,

							inv.TotalAmount [Billed Amount],
							convert(date,inv.CreatedDate) [Settlement Date]
                            ,inv.[Description]

                            ,(select top(1) CardTransactionId from [Transaction] where id in 
	                            (select TransactionId from TransactionHistory where InvoiceId = inv.Id) and CardTransactionId != '' ) as [TransactionId]
                               ,inv.[Status]

							into #InvoiceData
                            from Invoice inv
                            left join Customer cus on cus.CustomerId = inv.CustomerId
							left join LookUp lktype on lktype.DisplayText = inv.Status  and lktype.DataKey='InvoiceStatusForSales' 

                            where inv.CreatedBy = 'System' 
                            and inv.InvoiceFor =  @PaymentMethod
                            and inv.[Status] != 'Init' 
                            and inv.IsEstimate = 0
							{4}{5}{6}{7}
                            --and BillingCycle != 'Monthly' --,Annual,Semi-Annual,Quarterly
                            --and (inv.InvoiceId like @SearchText 
                            --or cus.FirstName + ' ' +cus.LastName like @SearchText
                            --or inv.[Description] like @SearchText)

							select * into #invoiceDatafilter
							from #InvoiceData

                            SELECT TOP (@pagesize) * into #Testtable FROM #invoiceDatafilter
                            where Id NOT IN(Select TOP (@pagestart) Id from #InvoiceData {3}) 
                            {3}
                          

                            select *  from #Testtable
				            select sum([Billed Amount]) as TotalAmountByPage from #TestTable 
                           
                            select Id
                            ,CONVERT(float,MonthlyMonitoringFee) as MonthlyMonitoringFee
                            ,BillCycle
                            ,SubscriptionStatus
                            into #CustomerData  from customer 
                                where IsActive =1 
                                    and PaymentMethod = 'Invoice' 
                                    --and BillAmount > 0
                                    and (MonthlyMonitoringFee is not null and CONVERT(float, MonthlyMonitoringFee) > 0)
                                    and (BillCycle is not null and BillCycle != '' and BillCycle !='-1')


                            select count(Id) as  [TotalCount]  
                            ,(select count (Id) from #CustomerData) as TotalCustomer
                            ,(select Sum (MonthlyMonitoringFee) from #CustomerData) as TotalMMR 
                            ,(select count (Id) from #CustomerData where BillCycle ='Monthly') as MonthlyCustomer
                            ,(select Sum (MonthlyMonitoringFee) from #CustomerData where BillCycle ='Monthly') as MonthlyMMR
                            ,(select count (Id) from #invoiceDatafilter where Status = 'Open') as InActiveCustomer
                            ,(select Sum (MonthlyMonitoringFee) from #CustomerData where SubscriptionStatus != 'active') as InActiveCustomerMMR
                            ,(SELECT  STUFF((SELECT  ',' + convert(nvarchar(50), Id)
							FROM #invoiceDatafilter #inv
							WHERE  #inv.Id = Id
							FOR XML PATH('')), 1, 1, '')
							) AS InvoiceIdList
                            from  #invoiceDatafilter

                            drop table #InvoiceData
                            drop table #invoiceDatafilter
                            drop table #CustomerData
                            drop table #Testtable
                            ";


            #region Order
            if (!string.IsNullOrWhiteSpace(filter.order))
            {
                if (filter.order == "ascending/customername")
                {
                    subquery = "order by CustomerName asc";

                }
                else if (filter.order == "descending/customername")
                {
                    subquery = "order by customername desc";

                }
                else if (filter.order == "ascending/invoiceno")
                {
                    subquery = "order by InvoiceId asc";

                }
                else if (filter.order == "descending/invoiceno")
                {
                    subquery = "order by InvoiceId desc";

                }
                else if (filter.order == "ascending/billamount")
                {
                    subquery = "order by TotalAmount asc";

                }
                else if (filter.order == "descending/billamount")
                {
                    subquery = "order by TotalAmount desc";

                }
                else if (filter.order == "ascending/description")
                {
                    subquery = "order by description asc";

                }
                else if (filter.order == "descending/description")
                {
                    subquery = "order by description desc";

                }
                else if (filter.order == "ascending/createddate")
                {
                    subquery = "order by [Settlement Date] asc";

                }
                else if (filter.order == "descending/createddate")
                {
                    subquery = "order by [Settlement Date] desc";

                }
                else if (filter.order == "ascending/status")
                {
                    subquery = "order by Status asc";

                }
                else if (filter.order == "descending/status")
                {
                    subquery = "order by Status desc";

                }



            }
            else
            {
                subquery = "order by [Settlement Date] desc";

            }
            #endregion

            try
            {
                sqlQuery = string.Format(sqlQuery,
                    filter.InvoiceFor,//0 
                    filter.PageNo, //1
                    filter.PageSize,//2
                    subquery,//3
                    datequery,//4
                    searchText, //5
                    BillingCycle,
                    statusquery//6
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
        public DataTable GetAllReceivePaymentsByCompanyId(Guid companyId)
        {
            string sqlQuery = @"select inv.*,cu.Title,cu.FirstName,cu.LastName from Invoice inv 
                                left join Customer cu 
                                on cu.CustomerId = inv.CustomerId
                                where inv.CompanyId='{0}'
                                and inv.Status != 'Init'
                                and inv.IsEstimate = 0
                                and inv.BalanceDue > 0
                                ";
            sqlQuery = string.Format(sqlQuery, companyId);

            try
            {

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

        public DataTable GetAllEstimateListByCustomerIdAndCompanyId(Guid CustomerId, Guid companyId, EstimateFilter filter)
        {
            var strStartDate = "";
            var strEndDate = "";
            var dateRange = "";
            string searchSql = "";
            string subquery = "";
            if (!string.IsNullOrWhiteSpace(filter.SearchText) && filter.SearchText != "undefined" && filter.SearchText != "null")
            {
                searchSql = string.Format(" and _Estimate.InvoiceId like '%{0}%'", filter.SearchText);
            }

            string sqlQuery = @"Declare @CustomerId uniqueidentifier
                                Declare @CompanyId uniqueidentifier
                                set @CustomerId ='{0}' 
                                set @CompanyId = '{1}'

                                select _Customer.FirstName+' '+_Customer.MiddleName +' '+_Customer.LastName CustomerName
                                ,_Estimate.*
                                ,emp.FirstName + ' ' + emp.LastName as UserNum
                                ,(select top 1 AddedDate from InvoiceNote where InvoiceId=_Estimate.Id order by  addedDate desc) as NoteAddedDate
                                ,(select top 1 AddedDate from CustomerAgreement where InvoiceId = _Estimate.InvoiceId and CompanyId = @CompanyId order by id desc) as CustomerViewedTime,
                                (select top 1 Type from CustomerAgreement where InvoiceId = _Estimate.InvoiceId and CompanyId = @CompanyId order by id desc) as CustomerViewedType,
                                (select top 1 Note from InvoiceNote where InvoiceId=_Estimate.Id order by  addedDate desc) NotesInvoice,
								(select top 1 added.FirstName + ' ' + added.LastName from InvoiceNote left join Employee added on added.UserId = AddedBy where InvoiceId=_Estimate.Id order by  addedDate desc) NoteInvoiceAddedBy
                                from Invoice _Estimate
                                left join Customer _Customer 
                                on _Estimate.CustomerId = _Customer.CustomerId
                                left join Employee emp
                                on emp.UserId = _Estimate.CreatedByUid
								 
                                where _Estimate.CompanyId =  @CompanyId
                                and _Estimate.CustomerId = @CustomerId
                                and _Estimate.Status != 'Init'
                                {4}
                                and _Estimate.IsEstimate = 1
                                {2}{3}
                                order by _Estimate.Id Desc ";

            if (filter.StrStartDate.ToString("yyyy-MM-dd HH:mm:ss") != "0001-01-01 00:00:00")
            {
                strStartDate = filter.StrStartDate.SetZeroHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss");
                strEndDate = filter.StrEndDate.SetMaxHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss");
                dateRange = string.Format("and _Estimate.InvoiceDate between '{0}' and '{1}'", strStartDate, strEndDate);
            }
            if (!string.IsNullOrWhiteSpace(filter.estimateStatus))
            {
                if (filter.estimateStatus == "Open")
                {
                    subquery = string.Format("and _Estimate.Status != 'Completed'");
                }
                else if (filter.estimateStatus == "Completed")
                {
                    subquery = string.Format("and _Estimate.Status = 'Completed'");
                }
            }
            //else
            //{
            //    subquery = string.Format("and _Estimate.Status = 'Open'");
            //}
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId, companyId, searchSql, dateRange, subquery);
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
        public DataTable GetAllEstimatorListByCustomerIdAndCompanyId(Guid CustomerId, Guid companyId, EstimateFilter filter)
        {
            var strStartDate = "";
            var strEndDate = "";
            var dateRange = "";
            string searchSql = "";
            string subquery = "";
            if (!string.IsNullOrWhiteSpace(filter.SearchText) && filter.SearchText != "undefined" && filter.SearchText != "null")
            {
                searchSql = string.Format(" and _Estimate.EstimatorId like '%{0}%'", filter.SearchText);
            }

            string sqlQuery = @"Declare @CustomerId uniqueidentifier
                                Declare @CompanyId uniqueidentifier
                                set @CustomerId ='{0}' 
                                set @CompanyId = '{1}'

                                select _Customer.FirstName+' '+_Customer.MiddleName +' '+_Customer.LastName CustomerName
                                ,_Estimate.*
                                ,emp.FirstName + ' ' + emp.LastName as UserNum
                                ,(select top 1 AddedDate from InvoiceNote where InvoiceId=_Estimate.Id order by  addedDate desc) as NoteAddedDate
                                ,(select top 1 AddedDate from CustomerAgreement where InvoiceId = _Estimate.EstimatorId and CompanyId = @CompanyId order by id desc) as CustomerViewedTime,
                                (select top 1 Type from CustomerAgreement where InvoiceId = _Estimate.EstimatorId and CompanyId = @CompanyId order by id desc) as CustomerViewedType,
                                (select top 1 Note from InvoiceNote where InvoiceId=_Estimate.Id order by  addedDate desc) NotesInvoice,
								(select top 1 added.FirstName + ' ' + added.LastName from InvoiceNote left join Employee added on added.UserId = AddedBy where InvoiceId=_Estimate.Id order by  addedDate desc) NoteInvoiceAddedBy
                                from Estimator _Estimate
                                left join Customer _Customer 
                                on _Estimate.CustomerId = _Customer.CustomerId
                                left join Employee emp
                                on emp.UserId = _Estimate.CreatedBy
								 
                                where 
                                 _Estimate.CustomerId = @CustomerId
                                and _Estimate.Status != 'Init'
                                {4}
                                
                                {2}{3}
                                order by _Estimate.Id Desc ";

            if (filter.StrStartDate.ToString("yyyy-MM-dd HH:mm:ss") != "0001-01-01 00:00:00")
            {
                strStartDate = filter.StrStartDate.SetZeroHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss");
                strEndDate = filter.StrEndDate.SetMaxHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss");
                dateRange = string.Format("and _Estimate.CreatedDate between '{0}' and '{1}'", strStartDate, strEndDate);
            }
            if (!string.IsNullOrWhiteSpace(filter.estimateStatus))
            {
                if (filter.estimateStatus == "Open")
                {
                    subquery = string.Format("and _Estimate.Status != 'Completed'");
                }
                else if (filter.estimateStatus == "Completed")
                {
                    subquery = string.Format("and _Estimate.Status = 'Completed'");
                }
            }
            //else
            //{
            //    subquery = string.Format("and _Estimate.Status = 'Open'");
            //}
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId, companyId, searchSql, dateRange, subquery);
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

        public DataTable GetEstimateByCompnayIdAndKey(Guid companyId, Guid employeeid, string key, string emptag, Guid empid)
        {
            string sqlQuery = @" select cus.FirstName+' '+cus.MiddleName +' '+cus.LastName CustomerName
                                ,_Estimate.*,
                                emp.FirstName + ' ' + emp.LastName as UserNum,
                                (select top 1 AddedDate from InvoiceNote where InvoiceId=_Estimate.Id order by  addedDate desc) NoteAddedDate
                                from Invoice _Estimate
                                left join Customer cus
                                on _Estimate.CustomerId = cus.CustomerId
                                join Employee emp
								on emp.UserId = '{1}'
                                where _Estimate.CompanyId = '{0}'
                                and _Estimate.Status != 'Init'
                                and _Estimate.IsEstimate = 1
                                and InvoiceId like @SearchText /*'%{2}%'*/ 
                                {3}";
            string subquery = "";
            if (!string.IsNullOrWhiteSpace(emptag) && emptag.ToLower().IndexOf("admin") == -1)
            {
                subquery = string.Format("and _Estimate.LastUpdatedByUid = '{0}'", empid);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, employeeid, key, subquery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", key)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetCustomerListBySearchKeyAndCompanyId(string key, Guid companyId, int MaxLoad)
        {
            string sqlQuery = @" select 
                                Top {2}
                                cus.CustomerId,
                                cus.[Address],
                                cus.Address2,
                                cus.Street,
                                cus.StreetPrevious as Street1,
                                cus.City,
                                cus.CityPrevious as City1,
                                cus.[State],
                                cus.StatePrevious as State1,
                                cus.ZipCode,
                                cus.ZipCodePrevious as ZipCode1,
                                cus.FirstName,
                                cus.LastName,
                                cus.EmailAddress,
                                cus.BusinessName,
                                cus.Type

                                 from CustomerCompany cc left join Customer cus
                                on cus.CustomerId = cc.CustomerId 
                                where cc.CompanyId='{1}'
                                and( cus.FirstName + ' '+ cus.LastName like '%{0}%'
                                OR cus.BusinessName like '%{0}%')
                                and cus.IsActive =1
                                and cc.IsLead =0";
            try
            {
                sqlQuery = string.Format(sqlQuery, key, companyId, MaxLoad);
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

        public DataTable GetLeadListBySearchKeyAndCompanyId(string key, Guid companyId, int MaxLoad)
        {
            string sqlQuery = @" select 
                                Top {2}
                                cus.CustomerId,
                                cus.[Address],
                                cus.Address2,
                                cus.Street,
                                cus.StreetPrevious as Street1,
                                cus.City,
                                cus.CityPrevious as City1,
                                cus.[State],
                                cus.StatePrevious as State1,
                                cus.ZipCode,
                                cus.ZipCodePrevious as ZipCode1,
                                cus.FirstName,
                                cus.LastName,
                                cus.EmailAddress,
                                cus.BusinessName,
                                cus.Type

                                 from CustomerCompany cc left join Customer cus
                                on cus.CustomerId = cc.CustomerId 
                                where cc.CompanyId='{1}'
                                and( cus.FirstName + ' '+ cus.LastName like '%{0}%'
                                OR cus.BusinessName like '%{0}%')
                                and cus.IsActive =1
                                and cc.IsLead =1";
            try
            {
                sqlQuery = string.Format(sqlQuery, key, companyId, MaxLoad);
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

        public bool DeleteEstimateConvertToOrder(string invId)
        {
            string SqlQuery = @"delete from invoice 
                                where InvoiceId = '{0}'";
            SqlQuery = string.Format(SqlQuery, invId);
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
        public bool UpdateInvoiceTicketIdByCustomerId(Guid TicketId, Guid CustomerId)
        {
            string SqlQuery = @"Update Invoice 
                                set TicketId='{0}' 
                                where CustomerId='{1}'";

            SqlQuery = string.Format(SqlQuery, TicketId, CustomerId);
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
        public DataTable GetRecentInvoiceUnpaidByCompanyIdAndCustomerId(Guid companyId, Guid customerid)
        {
            string sqlQuery = @"select inv.InvoiceId, inv.CreatedDate, inv.Id
                                from Invoice inv
                                left join Customer cus
                                on cus.CustomerId = inv.CustomerId
                                where inv.CompanyId = '{0}'
                                and cus.CustomerId = '{1}'
                                and (inv.Status = 'Open'
                                or inv.Status = 'Partial')
                                and inv.IsEstimate = 0
                                order by inv.InvoiceId desc";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, customerid);
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

        public DataTable GetInvoiceBalanceDueByCustomerId(Guid customerid)
        {
            string sqlQuery = @"select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                From Invoice where CustomerId='{0}'
                                and (Status='Open' or Status='Partial')";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerid);
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

        public DataTable GetAllRecurringBillingUppaidInvoiceByFilter(AllCustomerFilter filter)
        {
            var strStartDatequery = "";
            var strEndDatequery = "";
            string strorderbyquery = "";
            string searchTextbyquery = "";
            string customerIdbyquery = "";
            string PaymentText = "";

            string sqlQuery = @"


                                SELECT * into #tempTable FROM 
                                (
                                select cus.Id as CustomerIntId, cus.CustomerId, CASE WHEN cus.DBA != null or cus.DBA != '' THEN cus.DBA WHEN cus.BusinessName != null or cus.BusinessName != '' THEN cus.BusinessName ELSE cus.FirstName +' '+ cus.LastName END as CustomerName, cus.EmailAddress
                                ,inv.Id as InvoiceIntId, inv.InvoiceId, inv.Amount, inv.InvoiceDate, ISNULL((inv.BalanceDue),0.00) as BalanceDue, inv.DueDate, ISNULL((inv.Tax),0.00) as Tax, inv.Status, cc.CompanyId, inv.InvoiceFor, ISNULL((inv.DiscountAmount),0.00) as DiscountAmount                                
								,(select ISNULL(SUM(iv.BalanceDue),0.00) from Invoice iv where iv.CustomerId = inv.CustomerId and iv.IsARBInvoice = 1  AND iv.BalanceDue > 0 AND iv.IsEstimate = 0 AND iv.[Status] not in('Cancelled', 'Rolled Over', 'Init', 'Paid')) as NetTotalBalance
                                ,ROW_NUMBER() OVER ( PARTITION BY cus.CustomerId ORDER BY inv.InvoiceId DESC ) AS [ROW NUMBER]
                                from CustomerCompany cc join Customer cus on cc.CustomerId = cus.CustomerId join 
                                Invoice inv ON cus.CustomerId = inv.CustomerId
                                where cc.CompanyId = '{0}' {1} {2} {3} and inv.IsARBInvoice = 1 AND inv.BalanceDue > 0 AND inv.IsEstimate = 0 AND inv.[Status] not in('Cancelled', 'Rolled Over', 'Init', 'Paid')
                                 ) groups
                                WHERE groups.[ROW NUMBER] = 1
                                ORDER BY groups.CustomerName asc
                                
                                -- Customer and Invoice Data
                                select t.*, (t.NetTotalBalance - t.BalanceDue) as PastDue, CASE WHEN t.Status = 'Open' THEN ((t.NetTotalBalance - t.BalanceDue) + t.Amount) ELSE IIF((((t.NetTotalBalance - t.BalanceDue) + (t.BalanceDue - t.Tax)) > t.BalanceDue ),((t.NetTotalBalance - t.BalanceDue) + (t.BalanceDue - t.Tax)),((t.NetTotalBalance - t.BalanceDue) + t.BalanceDue)) END as TotalDue, CASE WHEN t.Status = 'Open' THEN ((t.NetTotalBalance - t.BalanceDue) + (t.Amount + t.Tax - t.DiscountAmount)) ELSE t.NetTotalBalance END as SubTotalWithTax  from #tempTable t
								where  {4} {5} 

                                drop table #tempTable


";
            DateTime chkDate = new DateTime();
            if (filter.StartDate != chkDate)
            {
                string strStartDate = filter.StartDate.SetClientZeroHourToUTC().ToString();
                strStartDatequery = string.Format("and inv.InvoiceDate >= '{0}'", strStartDate);
            }
            if (filter.EndDate != chkDate)
            {
                string strEndDate = filter.EndDate.SetClientMaxHourToUTC().ToString();
                strEndDatequery = string.Format("and inv.InvoiceDate <= '{0}'", strEndDate);
            }
            if (!string.IsNullOrEmpty(filter.SearchText) && !string.IsNullOrWhiteSpace(filter.SearchText))
            {
                searchTextbyquery = string.Format("and (cus.FirstName like '%{0}%' or cus.LastName like '%{0}%' or cus.BusinessName like '%{0}%' or cus.DBA like '%{0}%' or cus.Id like '%{0}%' or cus.CustomerNo like '%{0}%' or inv.InvoiceId like '%{0}%' or inv.Id like '%{0}%')", filter.SearchText);
            }
            if (!string.IsNullOrEmpty(filter.Paymentmethod) && !string.IsNullOrWhiteSpace(filter.Paymentmethod))
            {
                if (filter.Paymentmethod == "CreditCard")
                {

                    PaymentText = string.Format("t.InvoiceFor = 'Credit Card'");
                }
                else if (filter.Paymentmethod == "ACH")
                {
                    PaymentText = string.Format("t.InvoiceFor = 'ACH'");
                }
                else
                {
                    PaymentText = string.Format("t.InvoiceFor != 'ACH' and t.InvoiceFor != 'Credit Card'");
                }
            }
            else
            {
                PaymentText = string.Format("t.InvoiceFor = 'Credit Card'");
            }
            if (!string.IsNullOrWhiteSpace(filter.Order) && !string.IsNullOrEmpty(filter.Order))
            {
                if (filter.Order == "ascending/customername")
                {
                    strorderbyquery = "order by t.CustomerName asc";
                }
                else if (filter.Order == "descending/customername")
                {
                    strorderbyquery = "order by t.CustomerName desc";
                }
                //else if (filter.Order == "ascending/billingmethod")
                //{
                //    strorderbyquery = "order by #Inv.PaymentType asc";
                //}
                //else if (filter.Order == "descending/billingmethod")
                //{
                //    strorderbyquery = "order by #Inv.PaymentType desc";
                //}
                else if (filter.Order == "ascending/invoicedate")
                {
                    strorderbyquery = "order by t.InvoiceDate asc";
                }
                else if (filter.Order == "descending/invoicedate")
                {
                    strorderbyquery = "order by t.InvoiceDate desc";
                }
                else if (filter.Order == "ascending/duedate")
                {
                    strorderbyquery = "order by t.DueDate asc";
                }
                else if (filter.Order == "descending/duedate")
                {
                    strorderbyquery = "order by t.DueDate desc";
                }
                else if (filter.Order == "ascending/amount")
                {
                    strorderbyquery = "order by t.Amount asc";
                }
                else if (filter.Order == "descending/amount")
                {
                    strorderbyquery = "order by t.Amount desc";
                }
                else if (filter.Order == "ascending/totalduewithtax")
                {
                    strorderbyquery = "order by SubTotalWithTax asc";
                }
                else if (filter.Order == "descending/totalduewithtax")
                {
                    strorderbyquery = "order by SubTotalWithTax desc";
                }
                else if (filter.Order == "ascending/pastdueamount")
                {
                    strorderbyquery = "order by PastDue asc";
                }
                else if (filter.Order == "descending/pastdueamount")
                {
                    strorderbyquery = "order by PastDue desc";
                }
                else if (filter.Order == "ascending/totaldue")
                {
                    strorderbyquery = "order by TotalDue asc";
                }
                else if (filter.Order == "descending/totaldue")
                {
                    strorderbyquery = "order by TotalDue desc";
                }
            }
            else
            {
                strorderbyquery = "order by t.CustomerIntId asc";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, filter.CompanyId, searchTextbyquery, strStartDatequery, strEndDatequery, PaymentText, strorderbyquery);
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
        public DataTable GetallTabCountsDetailsByCustomerId(Guid customerId, Guid companyid)
        {
            string subquery = "";

            string sqlQuery = @"Declare @CustomerId uniqueidentifier
                                Declare @CompanyId uniqueidentifier
                                set @CustomerId = '{0}'
                                set @CompanyId = '{2}'

                                select (select count(id) from Invoice where IsEstimate = 0 
                                and Status !='Init' 
                                and CustomerId =@CustomerId 
                                and [Status] != 'paid' and [Status] != 'Decline' and [Status] != 'Rolled Over' and [Status] != 'Cancelled' and [Status] != 'Cancel' 
                                and CompanyId = @CompanyId) as InvoiceOpenCount,

                                (select count(id) from Invoice where IsEstimate = 0                         
                                and CustomerId =@CustomerId 
                                and [Status] = 'paid' 
                                and CompanyId = @CompanyId) as InvoicePaidCount,

                               (select count(id) from Invoice where IsEstimate = 0                         
                                and CustomerId =@CustomerId 
                                and [Status] = 'Rolled Over' 
                                and CompanyId = @CompanyId) as InvoiceRolledOverCount,

                                (select count(id) from Invoice where IsEstimate = 1
                                and [Status] !='Init' and [Status] != 'Completed' and CustomerId =@CustomerId and CompanyId = @CompanyId) as EstimateOpenCount,

                                (select count(id) from Invoice where IsEstimate = 1
                                 and  [Status] = 'Completed' and CustomerId =@CustomerId and CompanyId = @CompanyId) as EstimateCompletedCount,

                                 (select count(id) from CustomerFile where  
                                 CustomerId =@CustomerId and IsActive = 1 and CompanyId = @CompanyId) as FilesActiveCount,

                                 (select count(id) from CustomerFile where  
                                 CustomerId =@CustomerId and IsActive = 0 and CompanyId = @CompanyId) as FilesInActiveCount,

                                 (select count(id) from Estimator where  
                                 CustomerId =@CustomerId and CompanyId = @CompanyId  and [Status] !='Init' and [Status] != 'Completed' ) as EstimatorCount,

                                 (select count(id) from CustomerNote where  
                                 CustomerId =@CustomerId and CompanyId = @CompanyId) as NotesCount,

                                  (select count(id) from LeadCorrespondence where  
                                 CustomerId =@CustomerId and CompanyId = @CompanyId) as CorrespondenceCount,

                                  (select count(id) from CustomerFile where  
                                 CustomerId =@CustomerId and IsActive = 1 and CompanyId = @CompanyId) as FilesCount,

                                 (select count(th.Id) from [TransactionHistory] th
								 left join [Transaction] tr  on tr.Id  = th.TransactionId where  
                                 tr.CustomerId =@CustomerId and th.Amout!=0) as FundingCount ,

                                 (select count(th.Id) from TransactionExpense th
								 where CustomerId =@CustomerId) as ExpenseCount 
                                
                                
";
            //CustomerCredit
            try
            {
                sqlQuery = string.Format(sqlQuery, customerId, subquery, companyid);
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

        #region RMR Unpaid Invoice Information for Cash/Cheque Payments
        public DataTable DownloadUnpaidRMRInvoiceExcelData(string CustomerIdList, string InvoiceType, string PaymentMethod, Guid CompanyId)
        {
            string sqlQuery = "";
            string InvoiceIdQuery = "";
            string CustomerQuery = "";
            string PaymentMethodQuery = "";
            string StrInvoiceType = "All";
            string StrPaymentMethod = "All";
            //string cusList = "";
            //if (CustomerIdList != null && CustomerIdList.Count > 0)
            //{
            //    for (int i = 0; i < CustomerIdList.Count; i++)
            //    {
            //        if (i == 0) { cusList = CustomerIdList[i].ToString(); }
            //        else { cusList += "," +CustomerIdList[i].ToString(); }
            //    }
            //}

            //if (InvoiceType == "SelectedInvoice")
            //{
            //    if (IdList.Length > 1)
            //    {
            //        InvoiceIdQuery = string.Format("AND inv.InvoiceId in ({0})", InvoiceIdList);
            //    }
            //    else
            //    {
            //        InvoiceIdQuery = string.Format("AND inv.InvoiceId = {0}", InvoiceIdList);
            //    }
            //    StrInvoiceType = "Selected Id";
            //}
            CustomerQuery = string.Format("AND cus.Id in ({0}) ", CustomerIdList);
            if (InvoiceType == "RMR")
            {
                InvoiceIdQuery = string.Format("AND inv.IsARBInvoice = 1");
                StrInvoiceType = "RMR";
            }
            else if (InvoiceType == "Others")
            {
                InvoiceIdQuery = string.Format("AND inv.IsARBInvoice != 1");
                StrInvoiceType = "Others";
            }
            if (PaymentMethod == "Invoice")
            {
                PaymentMethodQuery = string.Format("AND inv.InvoiceFor != 'Credit Card' AND inv.InvoiceFor != 'ACH'");
                StrPaymentMethod = "Invoice";
            }
            else if (PaymentMethod == "CC")
            {
                PaymentMethodQuery = string.Format("AND inv.InvoiceFor = 'Credit Card'");
                StrPaymentMethod = "Credit Card";
            }
            else if (PaymentMethod == "ACH")
            {
                PaymentMethodQuery = string.Format("AND inv.InvoiceFor = 'ACH' ");
                StrPaymentMethod = "ACH";
            }
            //    if (InvoiceType == "SelectedInvoice")
            //    {
            //        sqlQuery = @"select cus.Id as [Customer Id]
            //, CASE WHEN cus.DBA != null  and cus.DBA != '' THEN cus.DBA WHEN cus.BusinessName != null  and cus.BusinessName != '' THEN cus.BusinessName  ELSE cus.FirstName +' '+ cus.LastName END AS [Customer Name]
            //, inv.InvoiceId as [Invoice Number]
            //, cast(inv.BalanceDue as decimal(10,2)) as [Total Due Amount]
            //, '' as [Check No]
            //, '' as [Collected Amount] 
            //                        , '{2}' as [Invoice For]
            //                        , '{3}' as [Payment Method]
            //                        , com.CompanyName as [Company Name]
            //from Invoice inv join Customer cus on cus.CustomerId = inv.CustomerId
            //                        join Company com on com.CompanyId = inv.CompanyId
            //where inv.CompanyId = '{1}' AND inv.BalanceDue > 0 AND inv.IsEstimate = 0 AND inv.[Status] not in('Cancelled', 'Rolled Over', 'Init', 'Paid')
            //                        {0}
            //                        {4}
            //Order by [Customer Name] asc";
            //    }
            //    else
            if (!string.IsNullOrWhiteSpace(CustomerIdList) && !string.IsNullOrEmpty(CustomerIdList))
            {
                sqlQuery = @"select cus.Id as [Customer Id],  cus.CustomerId 
          , CASE WHEN cus.DBA != null  and cus.DBA != '' THEN cus.DBA WHEN cus.BusinessName != null  and cus.BusinessName != '' THEN cus.BusinessName  ELSE cus.FirstName +' '+ cus.LastName END AS [Customer Name]
          ,STUFF((
          SELECT ',' + inv.InvoiceId
          FROM Invoice inv
          WHERE inv.CustomerId = cus.CustomerId AND inv.BalanceDue > 0 AND inv.IsEstimate = 0 AND inv.[Status] not in('Cancelled', 'Rolled Over', 'Init', 'Paid')
          {0}  {4} {5}
          FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, '') as [Invoice Number]
		  , '' as [Check No]
		  , '' as [Collected Amount] 
          , '{2}' as [Invoice For]
          , '{3}' as [Payment Method]
          , com.CompanyName as [Company Name]
		  into #InvoiceData
          from Company com join CustomerCompany cc on cc.CompanyId=com.CompanyId
		  join Customer cus on cus.CustomerId = cc.CustomerId where Com.CompanyId = '{1}' {5}  

		  Select [Customer Id], [Customer Name], [Invoice Number]
		  , CAST (( SELECT  SUM(inv.BalanceDue) FROM Invoice inv
          WHERE inv.CustomerId = ind.CustomerId AND inv.BalanceDue > 0 AND inv.IsEstimate = 0 AND inv.[Status] not in('Cancelled', 'Rolled Over', 'Init', 'Paid')
           {0}  {4}
          ) as decimal(10,2)) as [Total Due Amount] 
		  ,[Check No]
		  ,[Collected Amount] 
		  ,[Invoice For]
		  ,[Payment Method]
		  ,[Company Name]
		  From #InvoiceData ind where ind.[Invoice Number] is not null
           Order by [Customer Name] asc

		  DROP TABLE #InvoiceData

          ";
            }

            try
            {
                sqlQuery = string.Format(sqlQuery, InvoiceIdQuery, CompanyId, StrInvoiceType, StrPaymentMethod, PaymentMethodQuery, CustomerQuery);
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

        #region Invoice Statement
        public DataSet GetAllForInvoiceStatementByCustomerIntIdList(List<int> CustomerIdList, string StatementFor)
        {
            string CustomerIdQuery = "";
            string InvoiceTypeQuery = "";
            string InvoiceTypeQuery2 = "";
            string InvoiceTypeQuery3 = "";
            string CustomerCreditQuery = "";
            string sqlQuery = @"
                                SELECT * into #tempTable FROM 
                                (
                                select cus.Id as CustomerIntId, cus.CustomerId, CASE WHEN cus.DBA != null or cus.DBA != '' THEN cus.DBA WHEN cus.BusinessName != null or cus.BusinessName != '' THEN cus.BusinessName ELSE cus.FirstName +' '+ cus.LastName END as CustomerName, cus.Street, (cus.City+', '+ cus.State+' '+ cus.ZipCode) as CustomerAddress, cus.EmailAddress, cus.InstallDate, CASE WHEN cus.CellNo != null or cus.CellNo != '' THEN cus.CellNo WHEN cus.PrimaryPhone != null or cus.PrimaryPhone != '' THEN cus.PrimaryPhone ELSE cus.BillingPhone END as PhoneNumber
                                ,inv.Id as InvoiceIntId, inv.InvoiceId, inv.Amount, inv.TotalAmount, inv.InvoiceDate, inv.InvoiceEmailAddress, inv.BalanceDue, inv.DueDate, inv.Tax, inv.IsARBInvoice, inv.DiscountAmount, inv.TicketId, inv.Status, inv.CompanyId, inv.InvoiceFor, inv.Description
                                ,(select ISNULL(SUM(cc.amount),0.00) from CustomerCredit cc where cc.CustomerId = cus.CustomerId and (cc.IsDeleted != 1 or cc.IsDeleted is null) {3}) as CreditBalance
                                ,(select ISNULL(SUM(iv.BalanceDue),0.00) from Invoice iv where iv.CustomerId = inv.CustomerId {2} AND iv.BalanceDue > 0 AND iv.IsEstimate = 0 AND iv.[Status] not in('Cancelled', 'Rolled Over', 'Init', 'Paid')) as NetTotalBalance
                                ,ROW_NUMBER() OVER ( PARTITION BY cus.CustomerId ORDER BY inv.InvoiceId DESC ) AS [ROW NUMBER]
                                from Customer cus join 
                                Invoice inv ON cus.CustomerId = inv.CustomerId
                                where cus.Id in ({0}) {1} AND inv.BalanceDue > 0 AND inv.IsEstimate = 0 AND inv.[Status] not in('Cancelled', 'Rolled Over', 'Init', 'Paid')
                                 ) groups
                                WHERE groups.[ROW NUMBER] = 1
                                ORDER BY groups.CustomerName asc
                                
                                -- Customer and Invoice Data
                                select * from #tempTable
                                
                                -- Invoice Details List For New Invoice
                                select InvoiceId, EquipDetail, EquipName, Quantity, UnitPrice, TotalPrice
                                from InvoiceDetail where InvoiceId in (select InvoiceId from #tempTable group by InvoiceId)
                                
                                -- Due Open Invoice List
                                select i.Id, i.CustomerId, i.InvoiceId, i.Amount, i.BalanceDue,i.TotalAmount, i.DueDate, i.InvoiceDate, i.Tax, i.DiscountAmount, i.Status, i.InvoiceFor, i.Description
                                from invoice i where i.CustomerId in (select CustomerId from #tempTable group by CustomerId)
                                and i.InvoiceId not in((select InvoiceId from #tempTable group by InvoiceId)) {4} AND i.BalanceDue > 0 AND i.IsEstimate = 0 AND i.[Status] not in('Cancelled', 'Rolled Over', 'Init', 'Paid')
                                
                                drop table #tempTable
                                ";

            if (CustomerIdList != null && CustomerIdList.Count > 0)
            {
                //CustomerIdQuery = CustomerIdList.ToString();
                foreach (int id in CustomerIdList)
                {
                    if (string.IsNullOrEmpty(CustomerIdQuery))
                    {
                        CustomerIdQuery = id.ToString();
                    }
                    else
                    {
                        CustomerIdQuery = CustomerIdQuery + "," + id.ToString();
                    }
                    //CustomerIdQuery = string.Join(", ", id.ToString());
                }
                if (StatementFor == "RMR")
                {
                    InvoiceTypeQuery = string.Format("and inv.IsARBInvoice = 1");
                    InvoiceTypeQuery2 = string.Format("and iv.IsARBInvoice = 1");
                    InvoiceTypeQuery3 = string.Format("and i.IsARBInvoice = 1");
                    CustomerCreditQuery = string.Format("and cc.IsRMRCredit = 1");
                }
                else if (StatementFor == "Others")
                {
                    InvoiceTypeQuery = string.Format("and (inv.IsARBInvoice != 1 or inv.IsARBInvoice is null)");
                    InvoiceTypeQuery2 = string.Format("and (iv.IsARBInvoice != 1 or iv.IsARBInvoice is null)");
                    InvoiceTypeQuery3 = string.Format("and (i.IsARBInvoice != 1 or i.IsARBInvoice is null)");
                    CustomerCreditQuery = string.Format("and (cc.IsRMRCredit != 1 or cc.IsRMRCredit is null)");
                }
                try
                {
                    sqlQuery = string.Format(sqlQuery, CustomerIdQuery, InvoiceTypeQuery, InvoiceTypeQuery2, CustomerCreditQuery, InvoiceTypeQuery3);
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
            return null;
        }

        public DataSet GetAllPaidForInvoiceStatementByInvoiceIdList(string InvoiceIdList)
        {
            string InvoiceTypeQuery = "";
            string InvoiceTypeQuery2 = "";
            string sqlQuery = @"
                                SELECT * into #tempTable FROM 
                                (
                                select cus.Id as CustomerIntId, cus.CustomerId, CASE WHEN cus.DBA != null or cus.DBA != '' THEN cus.DBA WHEN cus.BusinessName != null or cus.BusinessName != '' THEN cus.BusinessName ELSE cus.FirstName +' '+ cus.LastName END as CustomerName, cus.Street, (cus.City+', '+ cus.State+' '+ cus.ZipCode) as CustomerAddress, cus.EmailAddress, cus.InstallDate, CASE WHEN cus.CellNo != null or cus.CellNo != '' THEN cus.CellNo WHEN cus.PrimaryPhone != null or cus.PrimaryPhone != '' THEN cus.PrimaryPhone ELSE cus.BillingPhone END as PhoneNumber
                                ,inv.Id as InvoiceIntId, inv.InvoiceId, inv.Amount, inv.TotalAmount, inv.InvoiceDate, inv.InvoiceEmailAddress, inv.BalanceDue, inv.DueDate, inv.Tax, inv.IsARBInvoice, inv.DiscountAmount, inv.TicketId, inv.Status, inv.CompanyId, inv.InvoiceFor, inv.Description
                                ,0.00 as CreditBalance
                                ,0.00 as NetTotalBalance
                                ,ROW_NUMBER() OVER ( PARTITION BY cus.CustomerId ORDER BY inv.InvoiceId DESC ) AS [ROW NUMBER]
                                from Invoice inv join 
								Customer cus ON cus.CustomerId = inv.CustomerId
                                where inv.InvoiceId = '{0}'								
                                 ) groups
                                WHERE groups.[ROW NUMBER] = 1
                                ORDER BY groups.CustomerName asc
                                
                                -- Customer and Invoice Data
                                select * from #tempTable
                                
                                -- Invoice Details List For New Invoice
                                select InvoiceId, EquipDetail, EquipName, Quantity, UnitPrice, TotalPrice
                                from InvoiceDetail where InvoiceId = '{0}'	
                                
                                -- Due Open Invoice List
                                {1}
                                
                                drop table #tempTable
                                ";

            if (!string.IsNullOrWhiteSpace(InvoiceIdList) && !string.IsNullOrEmpty(InvoiceIdList))
            {
                string[] ArrInvoiceIdList = InvoiceIdList.Split(',');
                if (ArrInvoiceIdList.Length > 0)
                {
                    string StrList = "";
                    for (int i = 0; i < ArrInvoiceIdList.Length; i++)
                    {
                        if (i == 0) { InvoiceTypeQuery = ArrInvoiceIdList[i]; }
                        else
                        {

                            if (string.IsNullOrWhiteSpace(StrList) || string.IsNullOrEmpty(StrList))
                            {
                                StrList = string.Format("'{0}'", ArrInvoiceIdList[i]);
                            }
                            else if (!string.IsNullOrWhiteSpace(StrList) && !string.IsNullOrEmpty(StrList))
                            {
                                StrList += string.Format(",'{0}'", ArrInvoiceIdList[i]);
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(StrList) && !string.IsNullOrEmpty(StrList))
                    {
                        InvoiceTypeQuery2 = string.Format(" select i.Id, i.CustomerId, i.InvoiceId, i.Amount, i.BalanceDue,i.TotalAmount, i.DueDate, i.InvoiceDate, i.Tax, i.DiscountAmount, i.Status, i.InvoiceFor, i.Description from invoice i where i.InvoiceId in ({0})", StrList);
                    }
                }
                try
                {
                    sqlQuery = string.Format(sqlQuery, InvoiceTypeQuery, InvoiceTypeQuery2);
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
            return null;
        }

        public DataTable GetTotalDueAmountForInvoiceStatement(Guid customerId, string type)
        {
            string RMRQuery = "";
            if (type.ToLower() == "rmr") { RMRQuery = string.Format("and IsARBInvoice = 1"); }
            else if (type.ToLower() == "others") { RMRQuery = string.Format("and (IsARBInvoice != 1 or IsARBInvoice is null)"); }
            string sqlQuery = @"
                                select ISNULL(SUM(BalanceDue),0.00) as TotalDue, ISNULL(SUM(TotalAmount),0.00) as TotalAmount
                                from Invoice where CustomerId = '{0}' {1} and BalanceDue > 0 and IsEstimate = 0 and [Status] not in('Cancelled', 'Rolled Over', 'Init', 'Paid')
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerId, RMRQuery);
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
