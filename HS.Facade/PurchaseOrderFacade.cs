using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Facade
{
    public class PurchaseOrderFacade : BaseFacade
    {
        public PurchaseOrderFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        PurchaseOrderDataAccess _PurchaseOrderDataAccess
        {
            get
            {
                return (PurchaseOrderDataAccess)_ClientContext[typeof(PurchaseOrderDataAccess)];
            }
        }
        PurchaseOrderDetailDataAccess _PurchaseOrderDetailDataAccess
        {
            get
            {
                return (PurchaseOrderDetailDataAccess)_ClientContext[typeof(PurchaseOrderDetailDataAccess)];
            }
        }
        PurchaseOrderWarehouseDataAccess _PurchaseOrderWarehouseDataAccess
        {
            get
            {
                return (PurchaseOrderWarehouseDataAccess)_ClientContext[typeof(PurchaseOrderWarehouseDataAccess)];
            }
        }
        PurchaseOrderBranchDataAccess _PurchaseOrderBranchDataAccess
        {
            get
            {
                return (PurchaseOrderBranchDataAccess)_ClientContext[typeof(PurchaseOrderBranchDataAccess)];
            }
        }
        PurchaseOrderTechDataAccess _PurchaseOrderTechDataAccess
        {
            get
            {
                return (PurchaseOrderTechDataAccess)_ClientContext[typeof(PurchaseOrderTechDataAccess)];
            }
        }
        PurchaseOrderBranchReceivedDataAccess _PurchaseOrderBranchReceivedDataAccess
        {
            get
            {
                return (PurchaseOrderBranchReceivedDataAccess)_ClientContext[typeof(PurchaseOrderBranchReceivedDataAccess)];
            }
        }
        PurchaseOrderTechReceivedDataAccess _PurchaseOrderTechReceivedDataAccess
        {
            get
            {
                return (PurchaseOrderTechReceivedDataAccess)_ClientContext[typeof(PurchaseOrderTechReceivedDataAccess)];
            }
        }
        EquipmentReturnDataAccess EquipmentReturnDataAccess
        {
            get
            {
                return (EquipmentReturnDataAccess)_ClientContext[typeof(EquipmentReturnDataAccess)];
            }
        }
        EquipmentReturnNoteDataAccess EquipmentReturnNoteDataAccess
        {
            get
            {
                return (EquipmentReturnNoteDataAccess)_ClientContext[typeof(EquipmentReturnNoteDataAccess)];
            }
        }
        EquipmentReturnVendorDataAccess EquipmentReturnVendorDataAccess
        {
            get
            {
                return (EquipmentReturnVendorDataAccess)_ClientContext[typeof(EquipmentReturnVendorDataAccess)];
            }
        }
        public int InsertPurchaseOrder(PurchaseOrder PurchaseOrder)
        {
            return (int)_PurchaseOrderDataAccess.Insert(PurchaseOrder);
        }
        public int InsertPurchaseOrderDetail(PurchaseOrderDetail PurchaseOrderDetail)
        {
            return (int)_PurchaseOrderDetailDataAccess.Insert(PurchaseOrderDetail);
        }

        public long CheckMassRestockOrderDetail(Guid EquipmentId, Guid TechnicianId)
        {
            return (long)_PurchaseOrderDetailDataAccess.CheckMassRestockOrderDetail(EquipmentId, TechnicianId);
        }

        public PurchaseOrder GetPurchaseOrderById(int value)
        {
            return _PurchaseOrderDataAccess.Get(value);
        }
        public PurchaseOrderWarehouse GetPurchaseOrderWarehouseById(int value)
        {
            return _PurchaseOrderWarehouseDataAccess.Get(value);
        }
        public PurchaseOrderTech GetPurchaseOrderTechById(int value)
        {
            return _PurchaseOrderTechDataAccess.Get(value);
        }
        public PurchaseOrderWarehouse GetPurchaseOrderByPurchaseOrderId(string purchaseOrderId)
        {
            return _PurchaseOrderWarehouseDataAccess.GetPurchaseOrderByPurchaseOrderId(purchaseOrderId);
        }
        public PurchaseOrderWarehouse GetPurchaseOrderWarehouseByPurchaseOrderId(string purchaseOrderId)
        {
            var purchaseOrderDetails = _PurchaseOrderWarehouseDataAccess.GetByQuery(string.Format("PurchaseOrderId = '{0}'", purchaseOrderId)).FirstOrDefault();
            return purchaseOrderDetails;
        }
        public PurchaseOrderTech GetPurchaseOrderTechByPurchaseOrderId(string demandOrderId)
        {
            return _PurchaseOrderTechDataAccess.GetByQuery(string.Format("DemandOrderId = '{0}'", demandOrderId)).FirstOrDefault();
        }
        public List<PurchaseOrderDetail> GetPurchaseOrderDetailListByPurchaseOrderId(string purchaseOrderId)
        {
            //return _PurchaseOrderDetailDataAccess.GetByQuery(string.Format("PurchaseOrderId = '{0}'", purchaseOrderId));
            return _PurchaseOrderDetailDataAccess.GetPurchaseOrderDetailListByPurchaseOrderId(purchaseOrderId);
        }
        public List<PurchaseOrderDetail> GetReceivePOHistoryByPurchaseOrderId(int Id)
        {
            DataTable dt = _PurchaseOrderDetailDataAccess.GetReceivePOHistoryByPurchaseOrderId(Id);
            List<PurchaseOrderDetail> PurchaseOrderDetailList = new List<PurchaseOrderDetail>();
            PurchaseOrderDetailList = (from DataRow dr in dt.Rows
                                       select new PurchaseOrderDetail()
                                       {
                                           EquipName = dr["EquipName"].ToString(),
                                           RecieveQty = dr["RecieveQty"] != DBNull.Value ? Convert.ToInt32(dr["RecieveQty"]) : 0,
                                           ReceiveBy = dr["ReceiveBy"].ToString(),
                                           ReceiveFor = dr["ReceiveFor"].ToString(),
                                           RecieveDate = dr["RecieveDate"] != DBNull.Value ? Convert.ToDateTime(dr["RecieveDate"]) : new DateTime()
                                       }).ToList();
            return PurchaseOrderDetailList;
        }
        public PurchaseOrderDetail GetPurchaseOrderDetailByPurchaseOrderId(string POID, Guid EquipmentId)
        {
            string query = string.Format("PurchaseOrderId='{0}' And EquipmentId='{1}'", POID, EquipmentId);
            return _PurchaseOrderDetailDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public bool UpdatePurchaseOrder(PurchaseOrder purchaseOrder)
        {
            return _PurchaseOrderDataAccess.Update(purchaseOrder) > 0;
        }

        public bool DeleteAllPurchaseOrderDetailByPurchaseOrderId(string purchaseOrderId)
        {
            return _PurchaseOrderDetailDataAccess.DeleteAllPurchaseOrderDetailByPurchaseOrderId(purchaseOrderId);
        }

        public bool UpdatePurchaseOrderDetail(PurchaseOrderDetail item)
        {
            return _PurchaseOrderDetailDataAccess.Update(item) > 0;
        }

        public List<PurchaseOrderBranch> GetAllPurchaseOrderBranch()
        {
            return _PurchaseOrderBranchDataAccess.GetAll();
        }
        public PurchaseOrderDetail GetEquipmentByPurchaseOrderId(string PurchaseOrderId)
        {
            return _PurchaseOrderDetailDataAccess.GetByQuery(string.Format("PurchaseOrderId = '{0}'", PurchaseOrderId)).FirstOrDefault();
        }
        public DataTable GetPurchaseOrderExportListByFilters(PurchaseOrderFilter filters, DateTime? start, DateTime? end)
        {
            DataTable dt = _PurchaseOrderDataAccess.GetPurchaseOrderExportListByFilters(filters, start, end);

            if (dt != null && dt.Columns.Contains("Received For"))
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["Received For"] != DBNull.Value && row["Received For"].ToString() == "System User")
                    {
                        row["Received For"] = "Warehouse";
                    }
                }
            }
        
            return dt;
        }
      

        public POListModel GetPurchaseOrderListByFilters(PurchaseOrderFilter filters, DateTime? start, DateTime? end)
        {
            DataSet ds = _PurchaseOrderDataAccess.GetPurchaseOrderListByFilters(filters,start,end);
            POListModel Model = new POListModel();
            Model.PurchaseOrderWarehouseList = new List<PurchaseOrderWarehouse>();
            if (ds != null)
                Model.PurchaseOrderWarehouseList = (from DataRow dr in ds.Tables[0].Rows
                                                    select new PurchaseOrderWarehouse()
                                                    {
                                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                        PurchaseOrderId = dr["PurchaseOrderId"].ToString(),
                                                        Status = dr["Status"].ToString(),
                                                        Name = dr["Name"].ToString(),
                                                        VendorName = dr["VendorName"].ToString(),
                                                        CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                                                        TotalAmount = dr["TotalOrderPriceSum"] != DBNull.Value ? Convert.ToDouble(dr["TotalOrderPriceSum"]) : 0,
                                                        Description = dr["Description"].ToString(),
                                                        TechnicianName = dr["TechnicianName"].ToString() == "System User" ? "Warehouse" : dr["TechnicianName"].ToString()

                                                    }).ToList();

            Model.TotalCount = ds != null && ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;
            Model.Searchtext = filters.Searchtext;
            Model.PageNo = filters.PageNo;
            Model.PageSize = filters.PageSize;
            return Model;
        }

        public List<PurchaseOrderWarehouse> GetPurchaseOrderWarehouseByEstimatorId(string estimatorId)
        {
            return _PurchaseOrderWarehouseDataAccess.GetByQuery(string.Format("EstimatorId = '{0}' and Status !='Init' ", estimatorId)) ;
        }

        public BIListModel GetBadInventoryListByFilters(BadInventoryFilter filters, Guid techid)
        {
            DataSet ds = _PurchaseOrderDataAccess.GetBadInventoryListFilters(filters, techid);
            BIListModel Model = new BIListModel();
            Model.TotalCount = ds != null && ds.Tables[0].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["TotalCount"]) : 0;
            Model.EquipmentReturnList = new List<EquipmentReturn>();
            if (ds != null)
                Model.EquipmentReturnList = (from DataRow dr in ds.Tables[1].Rows
                                             select new EquipmentReturn()
                                             {
                                                 Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                 CustomerId = new Guid(dr["CustomerId"].ToString()),
                                                 EquipmentId = new Guid(dr["EquipmentId"].ToString()),
                                                 ReturnId = new Guid(dr["EquipmentId"].ToString()),
                                                 TechnicianId = new Guid(dr["TechnicianId"].ToString()),
                                                 CompanyId = new Guid(dr["CompanyId"].ToString()),
                                                 Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                                 InvoiceNo = dr["InvoiceNo"].ToString(),
                                                 Status = dr["Status"].ToString(),
                                                 PurchaseDate = dr["PurchaseDate"] != DBNull.Value ? Convert.ToDateTime(dr["PurchaseDate"]) : new DateTime(),
                                                 WanrantyAvailable = dr["WanrantyAvailable"] != DBNull.Value ? Convert.ToBoolean(dr["WanrantyAvailable"]) : false,
                                                 Description = dr["Description"].ToString(),
                                                 LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : DateTime.Now,
                                                 LastUpdatedBy = new Guid(dr["LastUpdatedBy"].ToString()),
                                                 CustomerName = dr["CustomerName"].ToString(),
                                                 EquipmentName = dr["EquipmentName"].ToString(),
                                                 TechnicianName = dr["TechnicianName"].ToString(),
                                                 CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0
                                             }).ToList();
            
            Model.TotalQuantity = ds != null && ds.Tables[2].Rows[0]["TotalQuantity"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["TotalQuantity"]) : 0;
            Model.Searchtext = filters.Searchtext;
            Model.PageNo = filters.PageNo;
            Model.PageSize = filters.PageSize;
            return Model;
        }

        public DataTable GetBadInventoryReportListFilters(BadInventoryFilter filters)
        {
            return _PurchaseOrderDataAccess.GetBadInventoryReportListFilters(filters);
        }

        public int InsertPurchaseOrderTechReceived(PurchaseOrderTechReceived techrec)
        {
            return (int)_PurchaseOrderTechReceivedDataAccess.Insert(techrec);
        }

        public int InsertPurchaseOrderBranchReceived(PurchaseOrderBranchReceived rec)
        {
            return (int)_PurchaseOrderBranchReceivedDataAccess.Insert(rec);
        }

        public POListModel GetPurchaseOrderListByFiltersBranch(PurchaseOrderFilter filters)
        {
            DataSet ds = _PurchaseOrderDataAccess.GetPurchaseOrderListByFiltersBranch(filters);
            POListModel Model = new POListModel();
            Model.PurchaseOrderBranchList = new List<PurchaseOrderBranch>();
            if (ds != null)
                Model.PurchaseOrderBranchList = (from DataRow dr in ds.Tables[0].Rows
                                                 select new PurchaseOrderBranch()
                                                 {
                                                     Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                     TicketId = dr["TicketId"] != DBNull.Value ? Convert.ToInt32(dr["TicketId"]) : 0,
                                                     DOTId = dr["DOTId"] != DBNull.Value ? Convert.ToInt32(dr["DOTId"]) : 0,
                                                     Status = dr["Status"].ToString(),
                                                     DemandOrderId = dr["DemandOrderId"].ToString(),
                                                     TechName = dr["TechName"].ToString(),
                                                     Email = dr["Email"].ToString(),
                                                     CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now
                                                 }).ToList();
            Model.TotalCount = ds != null && ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;
            Model.Searchtext = filters.Searchtext;
            Model.PageNo = filters.PageNo;
            Model.PageSize = filters.PageSize;
            return Model;
        }

        public POListModel GetPurchaseOrderListByFiltersTech(PurchaseOrderFilter filters)
        {
            DataSet ds = _PurchaseOrderDataAccess.GetPurchaseOrderListByFiltersTech(filters);
            POListModel Model = new POListModel();
            Model.PurchaseOrderTechList = new List<PurchaseOrderTech>();
            if (ds != null)
                Model.PurchaseOrderTechList = (from DataRow dr in ds.Tables[0].Rows
                                               select new PurchaseOrderTech()
                                               {
                                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                   TicketId = dr["TicketId"] != DBNull.Value ? Convert.ToInt32(dr["TicketId"]) : 0,
                                                   Status = dr["Status"].ToString(),
                                                   DemandOrderId = dr["DemandOrderId"].ToString(),
                                                   CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now
                                               }).ToList();
            Model.TotalCount = ds != null && ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;
            Model.Searchtext = filters.Searchtext;
            Model.PageNo = filters.PageNo;
            Model.PageSize = filters.PageSize;
            return Model;
        }
        public List<CustomerAppointmentEquipment> GetRequestOrderListByFilter(DateTime? startDate, DateTime? dateTime, Guid userId)
        {
            DataSet ds = _PurchaseOrderDataAccess.GetRequestOrderListFilter(startDate, dateTime, userId);
            List<CustomerAppointmentEquipment> Model = new List<CustomerAppointmentEquipment>();
            if (ds != null)
                Model = (from DataRow dr in ds.Tables[0].Rows
                         select new CustomerAppointmentEquipment()
                         {
                             //Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             EquipmentId = (Guid)dr["EquipmentId"],
                             Quantity = dr["QTYNeeded"] != DBNull.Value ? Convert.ToInt32(dr["QTYNeeded"]) : 0,
                             WarehouseQTY = dr["WarehouseQTY"] != DBNull.Value ? Convert.ToInt32(dr["WarehouseQTY"]) : 0,
                             QTYOnHand = dr["QTYOnHand"] != DBNull.Value ? Convert.ToInt32(dr["QTYOnHand"]) : 0,
                             QTYPending= dr["QTYPending"] != DBNull.Value ? Convert.ToInt32(dr["QTYPending"]) : 0,
                             ProductSKU = dr["ProductSKU"].ToString(),
                             ManufacturerName = dr["ManufacturerName"].ToString(),
                             TicketsId=dr["TicketsId"].ToString(),
                             EquipmentName = dr["EquipmentName"].ToString()
                         }).ToList();
            return Model;
        }

        public CustomerAppointmentEquipmentListModel GetCompletedInventoryListByFilter(int? pageno, int? pagesize, DateTime startDate, DateTime endDate, Guid CompanyId, string SearchText, string order)
        {
            CustomerAppointmentEquipmentListModel Model = new CustomerAppointmentEquipmentListModel();
            DataSet ds = _PurchaseOrderDataAccess.GetCompletedInventoryListFilter(pageno, pagesize, startDate, endDate, CompanyId, SearchText, order);
            Model.CustomerAppointmentEquipments = (from DataRow dr in ds.Tables[0].Rows
                                                   select new CustomerAppointmentEquipment()
                                                   {
                                                       Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                       EquipmentId = (Guid)dr["EquipmentId"],
                                                       //Quantity = dr["QTYNeeded"] != DBNull.Value ? Convert.ToInt32(dr["QTYNeeded"]) : 0,
                                                       //WarehouseQTY = dr["WarehouseQTY"] != DBNull.Value ? Convert.ToInt32(dr["WarehouseQTY"]) : 0,
                                                       QTYOnHand = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                                       technician = dr["technician"] != DBNull.Value ? Convert.ToInt32(dr["technician"]) : 0,
                                                       TotalEq = dr["TotalEq"] != DBNull.Value ? Convert.ToInt32(dr["TotalEq"]) : 0,
                                                       QtyOnInstall= dr["QTYUsededInstall"] != DBNull.Value ? Convert.ToInt32(dr["QTYUsededInstall"]) : 0,
                                                       CostQtyOnInstall= dr["CostQTYUsededInstall"] != DBNull.Value ? Convert.ToDouble(dr["CostQTYUsededInstall"]) : 0.0,
                                                       QtyOnService= dr["QTYUsededService"] != DBNull.Value ? Convert.ToInt32(dr["QTYUsededService"]) : 0,
                                                       CostQtyOnService= dr["CostQTYUsededService"] != DBNull.Value ? Convert.ToDouble(dr["CostQTYUsededService"]) : 0.0,
                                                       ProductSKU = dr["SKU"].ToString(),
                                                       OrderingQuantity= dr["QTYOrdered"] != DBNull.Value ? Convert.ToInt32(dr["QTYOrdered"]) : 0,
                                                       CostQTYOrder= dr["CostQTYOrdered"] != DBNull.Value ? Convert.ToDouble(dr["CostQTYOrdered"]) : 0.0,
                                                       //ManufacturerName = dr["ManufacturerName"].ToString(),
                                                       ProductType = dr["Category"].ToString(),
                                                       CostPerItem= dr["VendorCost"] != DBNull.Value ? Convert.ToDouble(dr["VendorCost"]) : 0.0,
                                                       //CostQuantityNeeded = dr["CostQuantityNeeded"] != DBNull.Value ? Convert.ToDouble(dr["CostQuantityNeeded"]) : 0.00,
                                                       QTYPending= dr["QTYPending"] != DBNull.Value ? Convert.ToInt32(dr["QTYPending"]) : 0,
                                                       CurrentInventoryValue= dr["CurrentInventoryValue"] != DBNull.Value ? Convert.ToDouble(dr["CurrentInventoryValue"]) : 0.0,
                                                       EquipmentName = dr["Name"].ToString()
                                                   }).ToList();
            Model.TotalCount = ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;
            Model.PageNo = pageno.Value;
            Model.PageSize = pagesize.Value;
            Model.Searchtext = SearchText;
            Model.StartDate = startDate;
            Model.EndDate = endDate;
            return Model;
        }
        public DataTable GetCompletedInventoryListByFilterReport(Guid CompanyId, DateTime? Start, DateTime? End, string SearchText)
        {
            return _PurchaseOrderDataAccess.GetCompletedInventoryListFilterReport(CompanyId, Start, End, SearchText);
        }
        public List<PurchaseOrderTechReceived> GetPurchaseOrderTechReceivedListByTech(PurchaseOrderFilter filters)
        {
            DataSet ds = _PurchaseOrderTechReceivedDataAccess.GetPurchaseOrderTechReceivedListByTech(filters);
            List<PurchaseOrderTechReceived> Model = new List<PurchaseOrderTechReceived>();
            if (ds != null)
                Model = (from DataRow dr in ds.Tables[0].Rows
                         select new PurchaseOrderTechReceived()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             BranchDemandOrderId = dr["BranchDemandOrderId"].ToString(),
                             EquipmentId = (Guid)dr["EquipmentId"],
                             EquipName = dr["EquipName"].ToString(),
                             Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                             PurchaseOrderTechId = dr["PurchaseOrderTechId"] != DBNull.Value ? Convert.ToInt32(dr["PurchaseOrderTechId"]) : 0,
                             IsReceived = dr["IsReceived"] != DBNull.Value ? Convert.ToBoolean(dr["IsReceived"]) : false,
                             ReceivedDate = dr["ReceivedDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReceivedDate"]) : DateTime.Now
                         }).ToList();
            return Model;
        }
        public List<PurchaseOrderTechReceived> GetPurchaseOrderTechReceivedListByDOId(string DOId)
        {
            string query = string.Format("BranchDemandOrderId='" + DOId + "'");
            return _PurchaseOrderTechReceivedDataAccess.GetByQuery(query);
        }
        public int InsertPurchaseOrderWarehouse(PurchaseOrderWarehouse PurchaseOrderWarehouse)
        {
            return (int)_PurchaseOrderWarehouseDataAccess.Insert(PurchaseOrderWarehouse);
        }
        public bool UpdatePurchaseOrderWarehouse(PurchaseOrderWarehouse purchaseOrderWarehouse)
        {
            return _PurchaseOrderWarehouseDataAccess.Update(purchaseOrderWarehouse) > 0;
        }
        public PurchaseOrderWarehouse GetPurchaseOrderWarehouseByPOId(string poId)
        {
            string query = "PurchaseOrderId='" + poId + "'";
            return _PurchaseOrderWarehouseDataAccess.GetByQuery(query).FirstOrDefault();
        }

        public int InsertPurchaseOrderBranch(PurchaseOrderBranch PurchaseOrderBranch)
        {
            return (int)_PurchaseOrderBranchDataAccess.Insert(PurchaseOrderBranch);
        }
        public bool UpdatePurchaseOrderBranch(PurchaseOrderBranch purchaseOrderBranch)
        {
            return _PurchaseOrderBranchDataAccess.Update(purchaseOrderBranch) > 0;
        }
        public PurchaseOrderBranch GetPurchaseOrderBranchByPOId(string poId)
        {
            string query = "DemandOrderId='" + poId + "'";
            return _PurchaseOrderBranchDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public PurchaseOrderBranch GetPurchaseOrderBranchByTechPOId(string TechpoId)
        {
            string query = "TechDemandOrderId='" + TechpoId + "'";
            return _PurchaseOrderBranchDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public bool DeletePurchaseOrderBranchById(int id)
        {
            return _PurchaseOrderBranchDataAccess.Delete(id) > 0;
        }
        public bool DeletePurchaseOrderWareById(int id)
        {
            return _PurchaseOrderWarehouseDataAccess.Delete(id) > 0;
        }

        public int InsertPurchaseOrderTech(PurchaseOrderTech PurchaseOrderTech)
        {
            return (int)_PurchaseOrderTechDataAccess.Insert(PurchaseOrderTech);
        }
        public bool UpdatePurchaseOrderTech(PurchaseOrderTech purchaseOrderTech)
        {
            return _PurchaseOrderTechDataAccess.Update(purchaseOrderTech) > 0;
        }
        public PurchaseOrderTech GetPurchaseOrderTechByPOId(string poId)
        {
            string query = "DemandOrderId='" + poId + "'";
            return _PurchaseOrderTechDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public PurchaseOrderTechReceived PurchaseOrderTechReceivedByDOIDEqp(string DOID, Guid EquipmentId)
        {
            string query = "BranchDemandOrderId='" + DOID + "' And EquipmentId='" + EquipmentId + "'";
            return _PurchaseOrderTechReceivedDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public bool UpdatePurchaseOrderBranchReceived(PurchaseOrderBranchReceived purchaseOrderBranchReceived)
        {
            return _PurchaseOrderBranchReceivedDataAccess.Update(purchaseOrderBranchReceived) > 0;
        }
        public PurchaseOrderBranchReceived PurchaseOrderBranchReceivedByDOIDEqp(string DOID, Guid EquipmentId)
        {
            string query = "BranchDemandOrderId='" + DOID + "' And EquipmentId='" + EquipmentId + "'";
            return _PurchaseOrderBranchReceivedDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public bool UpdatePurchaseOrderTechReceived(PurchaseOrderTechReceived purchaseOrderTechReceived)
        {
            return _PurchaseOrderTechReceivedDataAccess.Update(purchaseOrderTechReceived) > 0;
        }
        public PurchaseOrderTechReceived GetPurchaseOrderTechReceivedById(int id)
        {
            return _PurchaseOrderTechReceivedDataAccess.Get(id);
        }
        public List<EstimatorId> GetEstimatorIdListOfPurchaseOrder()
        {
            DataTable dt = _PurchaseOrderDataAccess.GetEstimatorIdListOfPurchaseOrder();
            List<EstimatorId> EstimatorIdList = new List<EstimatorId>();
            EstimatorIdList = (from DataRow dr in dt.Rows
                              select new EstimatorId()
                              {
                                  Id = dr["EstimatorId"].ToString()
                              }).ToList();
            return EstimatorIdList;
        }
        #region BadInventory

        public DataTable GETSKU()  
        {
            DataTable dt = _PurchaseOrderDataAccess.GetSKU();

            return dt;
        }
        public EquipmentReturn GetEquipmentReturnById(int id)
        {
            return EquipmentReturnDataAccess.Get(id);
        }
        public bool InsertEquipmentReturn(EquipmentReturn equipmentReturn)
        {
            return EquipmentReturnDataAccess.Insert(equipmentReturn) > 0;
        }
        public bool UpdateEquipmentReturn(EquipmentReturn equipmentReturn)
        {
            return EquipmentReturnDataAccess.Update(equipmentReturn) > 0;
        }
        public bool DeleteEquipmentReturn(int Id)
        {
            return EquipmentReturnDataAccess.Delete(Id) > 0;
        }

        public bool InsertEquipmentReturnNote(EquipmentReturnNote equipmentReturnNote)
        {
            return EquipmentReturnNoteDataAccess.Insert(equipmentReturnNote) > 0;
        }
        public bool UpdateEquipmentReturnNote(EquipmentReturnNote equipmentReturnNote)
        {
            return EquipmentReturnNoteDataAccess.Update(equipmentReturnNote) > 0;
        }
        public bool DeleteEquipmentReturnNote(int Id)
        {
            return EquipmentReturnNoteDataAccess.Delete(Id) > 0;
        }
        public EquipmentReturnNote GetByReturnId(Guid ReturnId)
        {
            return EquipmentReturnNoteDataAccess.GetByQuery(string.Format("ReturnId = '{0}'", ReturnId)).FirstOrDefault();
        }

        public bool InsertEquipmentReturnVendor(EquipmentReturnVendor equipmentReturnVendor)
        {
            return EquipmentReturnVendorDataAccess.Insert(equipmentReturnVendor) > 0;
        }
        public bool UpdateEquipmentReturnVendor(EquipmentReturnVendor equipmentReturnVendor)
        {
            return EquipmentReturnVendorDataAccess.Update(equipmentReturnVendor) > 0;
        }
        public bool DeleteEquipmentReturnVendor(int Id)
        {
            return EquipmentReturnVendorDataAccess.Delete(Id) > 0;
        }
        public EquipmentReturnVendor GetVendorByReturnId(Guid ReturnId)
        {
            return EquipmentReturnVendorDataAccess.GetByQuery(string.Format("ReturnId = '{0}'", ReturnId)).FirstOrDefault();
        }
        #endregion

        #region PO Report
        public POListModel GetCreatedPurchaseOrderListByFilters(PurchaseOrderFilter filters, DateTime? start, DateTime? end)
        {
            DataSet ds = _PurchaseOrderDataAccess.GetCreatedPurchaseOrderListByFilters(filters, start, end);
            POListModel Model = new POListModel();
            Model.POReportList = new List<POReport>();
            if (ds != null)
                Model.POReportList = (from DataRow dr in ds.Tables[1].Rows
                                      select new POReport()
                                      {
                                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                          PurchaseOrderId = dr["PONumber"].ToString(),
                                          Status = dr["Status"].ToString(),
                                          CreatedBy = dr["CreatedBy"].ToString(),
                                          Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                          PoAmount = dr["PoAmount"] != DBNull.Value ? Convert.ToDouble(dr["PoAmount"]) : 0,
                                          CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime()
                                      }).ToList();
            Model.TotalCount = ds.Tables[0].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["TotalCount"]) : 0;
            Model.TotalQuantity = ds.Tables[2].Rows[0]["TotalQuantity"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["TotalQuantity"]) : 0;
            Model.TotalAmount = ds.Tables[2].Rows[0]["TotalAmount"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalAmount"]) : 0.0;
            Model.Searchtext = filters.Searchtext;
            Model.PageNo = filters.PageNo;
            Model.PageSize = filters.PageSize;
            return Model;
        }



        /// mayur :: new function 
      
   
        public POListModel GetPurchaseOrderNewListByFilters(PurchaseOrderFilter filters, DateTime? start, DateTime? end, string category, string Vendor,string manufeteture ,string serchtext ,string  SKU) // new po report
        {
            DataSet ds = _PurchaseOrderDataAccess.GetPurchaseOrderNewListByFilters(filters, start, end, category, Vendor, manufeteture , serchtext ,SKU );
            POListModel Model = new POListModel();
            DataTable dt = ds.Tables[3];
            Model.POReportNewList = new List<PONewReport>();
            Model.equipmentSKUs = new List<SKU>();
            if (ds != null)
                Model.POReportNewList = (from DataRow dr in ds.Tables[1].Rows
                                      select new PONewReport()
                                      {
                                          Id = dr["ID"] != DBNull.Value ? Convert.ToInt32(dr["ID"]) : 0,
                                          PurchaseOrderId = dr["PurchaseOrderId"].ToString(),
                                          OrderDate = dr["OrderDate"] != DBNull.Value ? Convert.ToDateTime(dr["OrderDate"]) : new DateTime(),
                                          Vender = dr["Vendor"].ToString(),
                                          Category = dr["Category"].ToString(),
                                          Manufacturer = dr["Manufacturer"].ToString(),
                                          Description = dr["Description"].ToString(),
                                          SKU = dr["SKU"].ToString(),
                                          Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                          UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0,
                                          TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0,
                                          Status = dr["Status"].ToString(),
                                          EquipName= dr["EquipName"].ToString()
                                      }).ToList();

            Model.TotalCount = ds.Tables[0].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["TotalCount"]) : 0;
            Model.TotalQuantity = ds.Tables[2].Rows[0]["TotalQuantity"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["TotalQuantity"]) : 0;
            Model.TotalAmount = ds.Tables[2].Rows[0]["TotalAmount"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalAmount"]) : 0.0;
          
            Model.Searchtext = filters.Searchtext;
            Model.PageNo = filters.PageNo;
            Model.PageSize = filters.PageSize;
            if (ds != null)
                Model.equipmentSKUs = (from DataRow dr in ds.Tables[3].Rows
                                       select new SKU()
                                       {
                                           text = dr["text"].ToString(),
                                           value= dr["value"].ToString(),
                                       }).ToList();

            return Model;
        }

        public POListModel GetPurchaseOrderNewListPOReportInventory(PurchaseOrderFilter filters, DateTime? start, DateTime? end, string category, string Vendor, string manufeteture, string serchtext, string SKU) // new po report
        {
            DataSet ds = _PurchaseOrderDataAccess.GetPurchaseOrderNewListPOReportInventory(filters, start, end, category, Vendor, manufeteture, serchtext, SKU);
            POListModel Model = new POListModel();
            DataTable dt = ds.Tables[3];
            Model.POReportNewList = new List<PONewReport>();
            Model.equipmentSKUs = new List<SKU>();
            if (ds != null)
                Model.POReportNewList = (from DataRow dr in ds.Tables[1].Rows
                                         select new PONewReport()
                                         {
                                             Id = dr["ID"] != DBNull.Value ? Convert.ToInt32(dr["ID"]) : 0,
                                             PurchaseOrderId = dr["PurchaseOrderId"].ToString(),
                                             OrderDate = dr["OrderDate"] != DBNull.Value ? Convert.ToDateTime(dr["OrderDate"]) : new DateTime(),
                                             CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                             Vender = dr["Vendor"].ToString(),
                                             Category = dr["Category"].ToString(),
                                             Manufacturer = dr["Manufacturer"].ToString(),
                                             Description = dr["Description"].ToString(),
                                             SKU = dr["SKU"].ToString(),
                                             Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                             RecieveQty = dr["RecieveQty"] != DBNull.Value ? Convert.ToInt32(dr["RecieveQty"]) : 0,
                                             UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0,
                                             TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0,
                                             Status = dr["Status"].ToString(),
                                             EquipName = dr["EquipName"].ToString(),
                                             Po_Description = dr["PO Description"].ToString(),
                                             TechnicianName = dr["TechnicianName"].ToString() == "System User" ? "Warehouse" : dr["TechnicianName"].ToString()
                                         }).ToList();

            Model.TotalCount = ds.Tables[0].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["TotalCount"]) : 0;
            Model.TotalQuantity = ds.Tables[2].Rows[0]["TotalQuantity"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["TotalQuantity"]) : 0;
            Model.TotalAmount = ds.Tables[2].Rows[0]["TotalAmount"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalAmount"]) : 0.0;

            Model.Searchtext = filters.Searchtext;
            Model.PageNo = filters.PageNo;
            Model.PageSize = filters.PageSize;
            if (ds != null)
                Model.equipmentSKUs = (from DataRow dr in ds.Tables[3].Rows
                                       select new SKU()
                                       {
                                           text = dr["text"].ToString(),
                                           value = dr["value"].ToString(),
                                       }).ToList();

            return Model;
        }


        public POListModel GetReceivedPurchaseOrderListByFilters(PurchaseOrderFilter filters, DateTime? start, DateTime? end)
        {
            DataSet ds = _PurchaseOrderDataAccess.GetReceivedPurchaseOrderListByFilters(filters, start, end);
            POListModel Model = new POListModel();
            Model.POReportList = new List<POReport>();
            if (ds != null)
                Model.POReportList = (from DataRow dr in ds.Tables[1].Rows
                                      select new POReport()
                                      {
                                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                          PurchaseOrderId = dr["PONumber"].ToString(),
                                          Status = dr["Status"].ToString(),
                                          CreatedBy = dr["CreatedBy"].ToString(),
                                          ReceivedBy = dr["ReceivedBy"].ToString(),
                                          Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                          PoAmount = dr["PoAmount"] != DBNull.Value ? Convert.ToDouble(dr["PoAmount"]) : 0,
                                          CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                          ReceivedDate = dr["RecieveDate"] != DBNull.Value ? Convert.ToDateTime(dr["RecieveDate"]) : new DateTime()

                                      }).ToList();
            Model.TotalCount = ds.Tables[0].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["TotalCount"]) : 0;
            Model.TotalQuantity = ds.Tables[2].Rows[0]["TotalQuantity"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["TotalQuantity"]) : 0;
            Model.TotalAmount = ds.Tables[2].Rows[0]["TotalAmount"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalAmount"]) : 0.0;
            Model.Searchtext = filters.Searchtext;
            Model.PageNo = filters.PageNo;
            Model.PageSize = filters.PageSize;
            return Model;
        }

        public DataTable GetCreatedPurchaseOrderListByFiltersForReportnew(PurchaseOrderFilter filters, DateTime? Start, DateTime? End,string serchtext,string SKU, string Category, string Manufeturelist, string supplier)
        {
            DataTable dt = _PurchaseOrderDataAccess.GetPurchaseOrderNewListByFiltersdownload(filters, Start, End , serchtext, SKU, Category, Manufeturelist, supplier);
            return dt;
        }
        public DataTable GetCreatedPurchaseOrderListPOReportInventoryDownload(PurchaseOrderFilter filters, DateTime? Start, DateTime? End, string serchtext, string SKU, string Category, string Manufeturelist, string supplier)
        {
            DataTable dt = _PurchaseOrderDataAccess.GetPurchaseOrderNewPOReportInventoryDownload(filters, Start, End, serchtext, SKU, Category, Manufeturelist, supplier);
            
            return dt;
        }
        public DataTable GetCreatedPurchaseOrderListByFiltersForReport(PurchaseOrderFilter filters, DateTime? Start, DateTime? End)
        {
            DataTable dt = _PurchaseOrderDataAccess.GetCreatedPurchaseOrderListByFiltersForReport(filters, Start, End);
            return dt;
        }

        public DataTable GetReceivedPurchaseOrderListByFiltersForReport(PurchaseOrderFilter filters, DateTime? Start, DateTime? End)
        {
            DataTable dt = _PurchaseOrderDataAccess.GetReceivedPurchaseOrderListByFiltersForReport(filters, Start, End);
            return dt;
        }

        #endregion
    }
}
