using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HS.Facade
{
    public class InventoryFacade : BaseFacade
    {
        public InventoryFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        EquipmentClassDataAccess _EquipmentClassDataAccess
        {
            get
            {
                return (EquipmentClassDataAccess)_ClientContext[typeof(EquipmentClassDataAccess)];
            }
        }
        EquipmentDataAccess _EquipmentDataAccess
        {
            get
            {
                return (EquipmentDataAccess)_ClientContext[typeof(EquipmentDataAccess)];
            }
        }
        InventoryDataAccess _InventoryDataAccess
        {
            get
            {
                return (InventoryDataAccess)_ClientContext[typeof(InventoryDataAccess)];
            }
        }
        TechnicianInventoryDataAccess _TechnicianInventoryDataAccess
        {
            get
            {
                return (TechnicianInventoryDataAccess)_ClientContext[typeof(TechnicianInventoryDataAccess)];
            }
        }
        PurchaseOrderDataAccess _PurchaseOrderDataAccess
        {
            get
            {
                return (PurchaseOrderDataAccess)_ClientContext[typeof(PurchaseOrderDataAccess)];
            }
        }
        InventoryWarehouseDataAccess _InventoryWarehouseDataAccess
        {
            get
            {
                return (InventoryWarehouseDataAccess)_ClientContext[typeof(InventoryWarehouseDataAccess)];
            }
        }
        InventoryBranchDataAccess _InventoryBranchDataAccess
        {
            get
            {
                return (InventoryBranchDataAccess)_ClientContext[typeof(InventoryBranchDataAccess)];
            }
        }
        InventoryTechDataAccess _InventoryTechDataAccess
        {
            get
            {
                return (InventoryTechDataAccess)_ClientContext[typeof(InventoryTechDataAccess)];
            }
        }
        ServiceDetailInfoViewDataAccess _ServiceDetailInfoViewDataAccess
        {
            get
            {
                return (ServiceDetailInfoViewDataAccess)_ClientContext[typeof(ServiceDetailInfoViewDataAccess)];
            }
        }

        ServiceDetailInfoDataAccess _ServiceDetailInfoDataAccess
        {
            get
            {
                return (ServiceDetailInfoDataAccess)_ClientContext[typeof(ServiceDetailInfoDataAccess)];
            }
        }
        ServiceMapDataAccess _ServiceMapDataAccess
        {
            get
            {
                return (ServiceMapDataAccess)_ClientContext[typeof(ServiceMapDataAccess)];
            }
        }
        EquipmentTechnicianReorderPointDataAccess _EquipmentTechnicianReorderPointDataAccess
        {
            get
            {
                return (EquipmentTechnicianReorderPointDataAccess)_ClientContext[typeof(EquipmentTechnicianReorderPointDataAccess)];
            }
        }
        EquipmentReturnDataAccess _EquipmentReturnDataAccess
        {
            get
            {
                return (EquipmentReturnDataAccess)_ClientContext[typeof(EquipmentReturnDataAccess)];
            }
        }
        AssignedInventoryTechReceivedDataAccess _AssignedInventoryTechReceivedDataAccess
        {
            get
            {
                return (AssignedInventoryTechReceivedDataAccess)_ClientContext[typeof(AssignedInventoryTechReceivedDataAccess)];
            }
        }
        EquipmentsFavouriteDataAccess _EquipmentsFavouriteDataAccess
        {
            get
            {
                return (EquipmentsFavouriteDataAccess)_ClientContext[typeof(EquipmentsFavouriteDataAccess)];
            }
        }

        ErrorLogDataAccess _ErrorLogDataAccess
        {
            get
            {
                return (ErrorLogDataAccess)_ClientContext[typeof(ErrorLogDataAccess)];
            }
        }

        public EquipmentClass GetById(int value)
        {
            return _EquipmentClassDataAccess.Get(value);
        }

        public EquipmentReturn GetBadInventoryById(int value)
        {
            return _EquipmentReturnDataAccess.Get(value);
        }

        public List<EquipmentClass> GetAllEquipmentClass()
        {
            return _EquipmentClassDataAccess.GetAll();
        }
        public List<EquipmentClass> GetAllEquipmentClassByCompanyId(Guid companyId)
        {
            return _EquipmentClassDataAccess.GetByQuery(string.Format(" CompanyId = '{0}'", companyId));
        }
        public List<ServiceDetailInfo> GetAllServiceDetailinfoByServiceId(Guid ServiceInfoId)
        {
            return _ServiceDetailInfoDataAccess.GetByQuery(string.Format(" ServiceId = '{0}'", ServiceInfoId));
        }
        public bool UpdateEquipmentClass(EquipmentClass eq)
        {
            return _EquipmentClassDataAccess.Update(eq) > 0;
        }
      
        public long InsertEquipmentClass(EquipmentClass eq)
        {
            return _EquipmentClassDataAccess.Insert(eq);
        }
        public long InsertServiceDetailInfo(ServiceDetailInfo serviceInfo)
        {
            return _ServiceDetailInfoDataAccess.Insert(serviceInfo);
        }
        public EquipmentClass GetEquipmentClassById(int value)
        {
            return _EquipmentClassDataAccess.Get(value);
        }
        public Equipment GetEquipmentId(int value)
        {
            return _EquipmentDataAccess.Get(value);
        }

        public EquipmentListWithCountModel GetEquipmentByFilter(FilterEquipment filter)
        {
            DataSet ds = _InventoryDataAccess.GetEquipmentListByFilter(filter);

            List<Equipment> CustomerList = new List<Equipment>();
            if (ds != null)
                CustomerList = (from DataRow dr in ds.Tables[0].Rows
                                select new Equipment()
                                {
                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    EquipmentId = (Guid)dr["EquipmentId"],
                                    CompanyId = (Guid)dr["CompanyId"],
                                    Name = dr["Name"].ToString(),
                                    SKU = dr["SKU"].ToString(),
                                    RepCost = dr["RepCost"] != DBNull.Value ? Convert.ToInt32(dr["RepCost"]) : 0,
                                    ManufacturerId = dr["ManufacturerId"] != DBNull.Value ? Convert.ToInt32(dr["ManufacturerId"]) : 0,
                                    SupplierId = dr["SupplierId"] != DBNull.Value ? Convert.ToInt32(dr["SupplierId"]) : 0,
                                    EquipmentTypeId = dr["EquipmentTypeId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentTypeId"]) : 0,
                                    EquipmentClassId = dr["EquipmentClassId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentClassId"]) : 0,
                                    Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0.0,
                                    //SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0.0,
                                    SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0.0,
                                    Cost = dr["Cost"] != DBNull.Value ? Convert.ToDouble(dr["Cost"]) : 0.0,
                                    Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0.0,
                                    EqOrder = dr["EqOrder"] != DBNull.Value ? Convert.ToInt32(dr["EqOrder"]) : 0,
                                    Service = dr["Service"].ToString(),
                                    AsOfDate = dr["AsOfDate"] != DBNull.Value ? Convert.ToDateTime(dr["AsOfDate"]) : DateTime.Now,
                                    reorderpoint = dr["reorderpoint"] != DBNull.Value ? Convert.ToInt32(dr["reorderpoint"]) : 0,
                                    IsActive = Convert.ToBoolean(dr["IsActive"]),
                                    Comments = dr["Comments"].ToString(),
                                    CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                                    LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : DateTime.Now,
                                    LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                                    EquipmentClass = dr["EquipmentClass"].ToString(),
                                    QtyOnHand = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                    InQueue = dr["InQueue"] != DBNull.Value ? Convert.ToInt32(dr["InQueue"]) : 0,
                                    LocQoH = dr["LocQoH"] != DBNull.Value ? Convert.ToInt32(dr["LocQoH"]) : 0,
                                    SupplierName = dr["SupplierName"].ToString(),
                                    ManufacturerName = dr["ManufacturerName"].ToString(),
                                    Category = dr["Category"].ToString(),
                                    RackNo = dr["RackNo"].ToString(),
                                    technician = dr["technician"] != DBNull.Value ? Convert.ToInt32(dr["technician"]) : 0,
                                    TotalEq = dr["TotalEq"] != DBNull.Value ? Convert.ToInt32(dr["TotalEq"]) : 0,
                                    FIFO = dr["FIFO"] != DBNull.Value ? Convert.ToDouble(dr["FIFO"]) : 0,
                                }).ToList();

            TotalEquipmentCount TotalCustomer = new TotalEquipmentCount();
            if (ds != null)
                TotalCustomer = (from DataRow dr in ds.Tables[1].Rows
                                 select new TotalEquipmentCount()
                                 {
                                     Counter = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                 }).FirstOrDefault();

            var totalCount = ds != null && ds.Tables.Count > 1 ? ds.Tables[1] : null;


            EquipmentListWithCountModel CustomerResultModel = new EquipmentListWithCountModel();
            CustomerResultModel.EquipmentList = CustomerList;
            CustomerResultModel.TotalEquipmentCount = TotalCustomer;

            return CustomerResultModel;
        }

        public EquipmentListWithCountModel GetEquipmentByFilterBranch(FilterEquipment filter)
        {
            DataSet ds = _InventoryDataAccess.GetEquipmentListByFilterBranch(filter);

            List<Equipment> CustomerList = new List<Equipment>();
            if (ds != null)
                CustomerList = (from DataRow dr in ds.Tables[0].Rows
                                select new Equipment()
                                {
                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    EquipmentId = (Guid)dr["EquipmentId"],
                                    CompanyId = (Guid)dr["CompanyId"],
                                    Name = dr["Name"].ToString(),
                                    SKU = dr["SKU"].ToString(),
                                    ManufacturerId = dr["ManufacturerId"] != DBNull.Value ? Convert.ToInt32(dr["ManufacturerId"]) : 0,
                                    SupplierId = dr["SupplierId"] != DBNull.Value ? Convert.ToInt32(dr["SupplierId"]) : 0,
                                    EquipmentTypeId = dr["EquipmentTypeId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentTypeId"]) : 0,
                                    EquipmentClassId = dr["EquipmentClassId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentClassId"]) : 0,
                                    Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0.0,
                                    SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0.0,
                                    Cost = dr["Cost"] != DBNull.Value ? Convert.ToDouble(dr["Cost"]) : 0.0,
                                    Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0.0,
                                    EqOrder = dr["EqOrder"] != DBNull.Value ? Convert.ToInt32(dr["EqOrder"]) : 0,
                                    Service = dr["Service"].ToString(),
                                    AsOfDate = dr["AsOfDate"] != DBNull.Value ? Convert.ToDateTime(dr["AsOfDate"]) : DateTime.Now,
                                    reorderpoint = dr["reorderpoint"] != DBNull.Value ? Convert.ToInt32(dr["reorderpoint"]) : 0,
                                    IsActive = Convert.ToBoolean(dr["IsActive"]),
                                    Comments = dr["Comments"].ToString(),
                                    CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                                    LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : DateTime.Now,
                                    LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                                    EquipmentClass = dr["EquipmentClass"].ToString(),
                                    QtyOnHand = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                    SupplierName = dr["SupplierName"].ToString()
                                }).ToList();

            TotalEquipmentCount TotalCustomer = new TotalEquipmentCount();
            if (ds != null)
                TotalCustomer = (from DataRow dr in ds.Tables[1].Rows
                                 select new TotalEquipmentCount()
                                 {
                                     Counter = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                 }).FirstOrDefault();

            var totalCount = ds != null && ds.Tables.Count > 1 ? ds.Tables[1] : null;


            EquipmentListWithCountModel CustomerResultModel = new EquipmentListWithCountModel();
            CustomerResultModel.EquipmentList = CustomerList;
            CustomerResultModel.TotalEquipmentCount = TotalCustomer;

            return CustomerResultModel;
        }
        public DataTable GetEquipmentByFilterTechDownload(FilterEquipment filter)
        {
            return _InventoryDataAccess.GetEquipmentByFilterTechDownload(filter);
        }
        public EquipmentListWithCountModel GetEquipmentByFilterTech(FilterEquipment filter)
        {
            DataSet ds = _InventoryDataAccess.GetEquipmentListByFilterTech(filter);

            List<Equipment> CustomerList = new List<Equipment>();
            if (ds != null)
                CustomerList = (from DataRow dr in ds.Tables[0].Rows
                                select new Equipment()
                                {
                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    EquipmentId = (Guid)dr["EquipmentId"],
                                    CompanyId = (Guid)dr["CompanyId"],
                                    Name = dr["Name"].ToString(),
                                    SKU = dr["SKU"].ToString(),
                                    RepCost = dr["RepCost"] != DBNull.Value ? Convert.ToInt32(dr["RepCost"]) : 0,
                                    ManufacturerId = dr["ManufacturerId"] != DBNull.Value ? Convert.ToInt32(dr["ManufacturerId"]) : 0,
                                    SupplierId = dr["SupplierId"] != DBNull.Value ? Convert.ToInt32(dr["SupplierId"]) : 0,
                                    EquipmentTypeId = dr["EquipmentTypeId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentTypeId"]) : 0,
                                    EquipmentClassId = dr["EquipmentClassId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentClassId"]) : 0,
                                    Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0.0,
                                    SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0.0,
                                    Cost = dr["Cost"] != DBNull.Value ? Convert.ToDouble(dr["Cost"]) : 0.0,
                                    Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0.0,
                                    EqOrder = dr["EqOrder"] != DBNull.Value ? Convert.ToInt32(dr["EqOrder"]) : 0,
                                    Service = dr["Service"].ToString(),
                                    AsOfDate = dr["AsOfDate"] != DBNull.Value ? Convert.ToDateTime(dr["AsOfDate"]) : DateTime.Now,
                                    reorderpoint = dr["reorderpoint"] != DBNull.Value ? Convert.ToInt32(dr["reorderpoint"]) : 0,
                                    IsActive = Convert.ToBoolean(dr["IsActive"]),
                                    Comments = dr["Comments"].ToString(),
                                    CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                                    LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : DateTime.Now,
                                    LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                                    EquipmentClass = dr["EquipmentClass"].ToString(),
                                    QtyOnHand = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                    SupplierName = dr["SupplierName"].ToString(),
                                    ManufacturerName = dr["ManufacturerName"].ToString(),
                                    Category = dr["Category"].ToString()
                                }).ToList();

            TotalEquipmentCount TotalCustomer = new TotalEquipmentCount();
            if (ds != null)
                TotalCustomer = (from DataRow dr in ds.Tables[1].Rows
                                 select new TotalEquipmentCount()
                                 {
                                     Counter = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                 }).FirstOrDefault();

            var totalCount = ds != null && ds.Tables.Count > 1 ? ds.Tables[1] : null;


            EquipmentListWithCountModel CustomerResultModel = new EquipmentListWithCountModel();
            CustomerResultModel.EquipmentList = CustomerList;
            CustomerResultModel.TotalEquipmentCount = TotalCustomer;

            return CustomerResultModel;
        }
        public EquipmentListWithCountModel GetEquipmentListByFilterTechForPendingReport(FilterEquipment filter)
        {
            DataSet ds = _InventoryDataAccess.GetEquipmentListByFilterTechForPendingReport(filter);

            List<Equipment> CustomerList = new List<Equipment>();
            if (ds != null)
                CustomerList = (from DataRow dr in ds.Tables[1].Rows
                                select new Equipment()
                                {
                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    EquipmentId = (Guid)dr["EquipmentId"],
                                    CompanyId = (Guid)dr["CompanyId"],
                                    Name = dr["Name"].ToString(),
                                    SKU = dr["SKU"].ToString(),
                                    ManufacturerId = dr["ManufacturerId"] != DBNull.Value ? Convert.ToInt32(dr["ManufacturerId"]) : 0,
                                    SupplierId = dr["SupplierId"] != DBNull.Value ? Convert.ToInt32(dr["SupplierId"]) : 0,
                                    EquipmentTypeId = dr["EquipmentTypeId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentTypeId"]) : 0,
                                    EquipmentClassId = dr["EquipmentClassId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentClassId"]) : 0,
                                    Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0.0,
                                    SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0.0,
                                    Cost = dr["Cost"] != DBNull.Value ? Convert.ToDouble(dr["Cost"]) : 0.0,
                                    Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0.0,
                                    EqOrder = dr["EqOrder"] != DBNull.Value ? Convert.ToInt32(dr["EqOrder"]) : 0,
                                    Service = dr["Service"].ToString(),
                                    AsOfDate = dr["AsOfDate"] != DBNull.Value ? Convert.ToDateTime(dr["AsOfDate"]) : DateTime.Now,
                                    reorderpoint = dr["reorderpoint"] != DBNull.Value ? Convert.ToInt32(dr["reorderpoint"]) : 0,
                                    IsActive = Convert.ToBoolean(dr["IsActive"]),
                                    Comments = dr["Comments"].ToString(),
                                    CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                                    LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : DateTime.Now,
                                    LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                                    EquipmentClass = dr["EquipmentClass"].ToString(),
                                    QtyOnHand = dr["TechQuantity"] != DBNull.Value ? Convert.ToInt32(dr["TechQuantity"]) : 0,
                                    SupplierName = dr["SupplierName"].ToString(),
                                    ManufacturerName = dr["ManufacturerName"].ToString(),
                                    Category = dr["Category"].ToString(),
                                    TechnicianName = dr["TechnicianName"].ToString(),
                                    ReceivedBy = dr["ReceivedBy"].ToString(),
                                    AmountTruck = dr["AmountTruck"] != DBNull.Value ? Convert.ToDouble(dr["AmountTruck"]) : 0,
                                    TransferredDate = dr["TransferredDate"] != DBNull.Value ? Convert.ToDateTime(dr["TransferredDate"]) : DateTime.Now
                                }).ToList();

            TotalEquipmentCount TotalCustomer = new TotalEquipmentCount();
            if (ds != null)
                TotalCustomer = (from DataRow dr in ds.Tables[0].Rows
                                 select new TotalEquipmentCount()
                                 {
                                     Counter = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                 }).FirstOrDefault();

            var totalCount = ds != null && ds.Tables.Count > 1 ? ds.Tables[0] : null;

            TotalTruckInvenorty TruckInvenorty = new TotalTruckInvenorty();
            if (ds != null)
                TruckInvenorty = (from DataRow dr in ds.Tables[2].Rows
                                  select new TotalTruckInvenorty()
                                  {
                                      TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                                      TotalSupplierCost = dr["TotalSupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["TotalSupplierCost"]) : 0.0,
                                      TotalQuantity = dr["TotalQuantity"] != DBNull.Value ? Convert.ToInt32(dr["TotalQuantity"]) : 0
                                  }).FirstOrDefault();

            var TotalTruckInvenorty = ds != null && ds.Tables.Count > 1 ? ds.Tables[2] : null;


            EquipmentListWithCountModel CustomerResultModel = new EquipmentListWithCountModel();
            CustomerResultModel.EquipmentList = CustomerList;
            CustomerResultModel.TotalEquipmentCount = TotalCustomer;
            CustomerResultModel.TotalTruckInvenorty = TruckInvenorty;

            return CustomerResultModel;
        }

        public EquipmentListWithCountModel GetEquipmentListByFilterTechForReportOnly(FilterEquipment filter)
        {
            DataSet ds = _InventoryDataAccess.GetEquipmentListByFilterTechForReportOnly(filter);

            List<Equipment> CustomerList = new List<Equipment>();
            if (ds != null)
                CustomerList = (from DataRow dr in ds.Tables[1].Rows
                                select new Equipment()
                                {
                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    EquipmentId = (Guid)dr["EquipmentId"],
                                    CompanyId = (Guid)dr["CompanyId"],
                                    Name = dr["Name"].ToString(),
                                    SKU = dr["SKU"].ToString(),
                                    ManufacturerId = dr["ManufacturerId"] != DBNull.Value ? Convert.ToInt32(dr["ManufacturerId"]) : 0,
                                    SupplierId = dr["SupplierId"] != DBNull.Value ? Convert.ToInt32(dr["SupplierId"]) : 0,
                                    EquipmentTypeId = dr["EquipmentTypeId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentTypeId"]) : 0,
                                    EquipmentClassId = dr["EquipmentClassId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentClassId"]) : 0,
                                    Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0.0,
                                    SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0.0,
                                    Cost = dr["Cost"] != DBNull.Value ? Convert.ToDouble(dr["Cost"]) : 0.0,
                                    Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0.0,
                                    EqOrder = dr["EqOrder"] != DBNull.Value ? Convert.ToInt32(dr["EqOrder"]) : 0,
                                    Service = dr["Service"].ToString(),
                                    AsOfDate = dr["AsOfDate"] != DBNull.Value ? Convert.ToDateTime(dr["AsOfDate"]) : DateTime.Now,
                                    reorderpoint = dr["reorderpoint"] != DBNull.Value ? Convert.ToInt32(dr["reorderpoint"]) : 0,
                                    IsActive = Convert.ToBoolean(dr["IsActive"]),
                                    Comments = dr["Comments"].ToString(),
                                    CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                                    LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : DateTime.Now,
                                    LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                                    EquipmentClass = dr["EquipmentClass"].ToString(),
                                    QtyOnHand = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                    SupplierName = dr["SupplierName"].ToString(),
                                    ManufacturerName = dr["ManufacturerName"].ToString(),
                                    Category = dr["Category"].ToString(),
                                    TechnicianName = dr["TechnicianName"].ToString(),
                                    ReceivedBy = dr["ReceivedBy"].ToString(),
                                    AmountTruck = dr["AmountTruck"] != DBNull.Value ? Convert.ToDouble(dr["AmountTruck"]) : 0,
                                    TransferredDate = dr["TransferredDate"] != DBNull.Value ? Convert.ToDateTime(dr["TransferredDate"]) : DateTime.Now
                                }).ToList();

            int TotalQty = ds.Tables[0].Rows
                       .Cast<DataRow>()
                       .Sum(dr => dr.Field<int?>("TotalQty") ?? 0);

            TotalEquipmentCount TotalCustomer = new TotalEquipmentCount();
            if (ds != null)
                TotalCustomer = (from DataRow dr in ds.Tables[0].Rows
                                 select new TotalEquipmentCount()
                                 {
                                     Counter = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                 }).FirstOrDefault();

            var totalCount = ds != null && ds.Tables.Count > 1 ? ds.Tables[0] : null;

            TotalTruckInvenorty TruckInvenorty = new TotalTruckInvenorty();
            if (ds != null)
                TruckInvenorty = (from DataRow dr in ds.Tables[2].Rows
                                 select new TotalTruckInvenorty()
                                 {
                                     TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                                     TotalSupplierCost = dr["TotalSupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["TotalSupplierCost"]) : 0.0,
                                     TotalQuantity = dr["TotalQuantity"] != DBNull.Value ? Convert.ToInt32(dr["TotalQuantity"]) : 0
                                 }).FirstOrDefault();

            var TotalTruckInvenorty  = ds != null && ds.Tables.Count > 1 ? ds.Tables[2] : null;


            EquipmentListWithCountModel CustomerResultModel = new EquipmentListWithCountModel();
            CustomerResultModel.EquipmentList = CustomerList;
            CustomerResultModel.TotalEquipmentCount = TotalCustomer;
            CustomerResultModel.TotalTruckInvenorty = TruckInvenorty;
            CustomerResultModel.TotalQty = TotalQty;

            return CustomerResultModel;
        }
        public EquipmentListWithCountModel GetEquipmentListByFilterTechForReportOnlyTechnician(FilterEquipment filter)
        {
            DataSet ds = _InventoryDataAccess.GetEquipmentListByFilterTechForReportOnlyTechnician(filter);

            List<Equipment> CustomerList = new List<Equipment>();
            if (ds != null)
                CustomerList = (from DataRow dr in ds.Tables[1].Rows
                                select new Equipment()
                                {
                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    EquipmentId = (Guid)dr["EquipmentId"],
                                    CompanyId = (Guid)dr["CompanyId"],
                                    Name = dr["Name"].ToString(),
                                    SKU = dr["SKU"].ToString(),
                                    ManufacturerId = dr["ManufacturerId"] != DBNull.Value ? Convert.ToInt32(dr["ManufacturerId"]) : 0,
                                    SupplierId = dr["SupplierId"] != DBNull.Value ? Convert.ToInt32(dr["SupplierId"]) : 0,
                                    EquipmentTypeId = dr["EquipmentTypeId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentTypeId"]) : 0,
                                    EquipmentClassId = dr["EquipmentClassId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentClassId"]) : 0,
                                    Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0.0,
                                    SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0.0,
                                    Cost = dr["Cost"] != DBNull.Value ? Convert.ToDouble(dr["Cost"]) : 0.0,
                                    Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0.0,
                                    EqOrder = dr["EqOrder"] != DBNull.Value ? Convert.ToInt32(dr["EqOrder"]) : 0,
                                    Service = dr["Service"].ToString(),
                                    AsOfDate = dr["AsOfDate"] != DBNull.Value ? Convert.ToDateTime(dr["AsOfDate"]) : DateTime.Now,
                                    reorderpoint = dr["reorderpoint"] != DBNull.Value ? Convert.ToInt32(dr["reorderpoint"]) : 0,
                                    IsActive = Convert.ToBoolean(dr["IsActive"]),
                                    Comments = dr["Comments"].ToString(),
                                    CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                                    LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : DateTime.Now,
                                    LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                                    EquipmentClass = dr["EquipmentClass"].ToString(),
                                    QtyOnHand = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                    SupplierName = dr["SupplierName"].ToString(),
                                    ManufacturerName = dr["ManufacturerName"].ToString(),
                                    Category = dr["Category"].ToString(),
                                    TechnicianName = dr["TechnicianName"].ToString(),
                                    ReceivedBy = dr["ReceivedBy"].ToString(),
                                    AmountTruck = dr["AmountTruck"] != DBNull.Value ? Convert.ToDouble(dr["AmountTruck"]) : 0,
                                    TransferredDate = dr["TransferredDate"] != DBNull.Value ? Convert.ToDateTime(dr["TransferredDate"]) : DateTime.Now
                                }).ToList();
            double TotalAmt = ds.Tables[0].Rows
                       .Cast<DataRow>()
                       .Sum(dr => dr.Field<double?>("TotalAmt") ?? 0);

            int TotalQty = ds.Tables[0].Rows
                     .Cast<DataRow>()
                     .Sum(dr => dr.Field<int?>("TotalQty") ?? 0);

            TotalEquipmentCount TotalCustomer = new TotalEquipmentCount();
            if (ds != null)
                TotalCustomer = (from DataRow dr in ds.Tables[0].Rows
                                 select new TotalEquipmentCount()
                                 {
                                     Counter = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                 }).FirstOrDefault();

            var totalCount = ds != null && ds.Tables.Count > 1 ? ds.Tables[0] : null;

            TotalTruckInvenorty TruckInvenorty = new TotalTruckInvenorty();
            if (ds != null)
                TruckInvenorty = (from DataRow dr in ds.Tables[2].Rows
                                  select new TotalTruckInvenorty()
                                  {
                                      TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                                      TotalSupplierCost = dr["TotalSupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["TotalSupplierCost"]) : 0.0,
                                      TotalQuantity = dr["TotalQuantity"] != DBNull.Value ? Convert.ToInt32(dr["TotalQuantity"]) : 0
                                  }).FirstOrDefault();

            var TotalTruckInvenorty = ds != null && ds.Tables.Count > 1 ? ds.Tables[2] : null;


            EquipmentListWithCountModel CustomerResultModel = new EquipmentListWithCountModel();
            CustomerResultModel.EquipmentList = CustomerList;
            CustomerResultModel.TotalEquipmentCount = TotalCustomer;
            CustomerResultModel.TotalTruckInvenorty = TruckInvenorty;
            CustomerResultModel.TotalAmt = TotalAmt;
            CustomerResultModel.TotalQty = TotalQty;

            return CustomerResultModel;
        }

        public List<ServiceMap> GetServiceMapListByServiceMap(ServiceMap serviceMap)
        {
            return _ServiceMapDataAccess.GetServiceMapListByServiceMap(serviceMap);
        }

        public EquipmentListWithCountModel GetProductListByFilter(FilterEquipment filter,DateTime? StartDate,DateTime? EndDate)
        {
            //DataSet ds = _InventoryDataAccess.GetEquipmentListByFilter(filter);
            DataSet ds = _InventoryDataAccess.GetProductListByFilter(filter, StartDate, EndDate);

            List<Equipment> CustomerList = new List<Equipment>();
            CustomerList = (from DataRow dr in ds.Tables[0].Rows
                            select new Equipment()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                EquipmentId = (Guid)dr["EquipmentId"],
                                CompanyId = (Guid)dr["CompanyId"],
                                Name = dr["Name"].ToString(),
                                SKU = dr["SKU"].ToString(),
                                ManufacturerId = dr["ManufacturerId"] != DBNull.Value ? Convert.ToInt32(dr["ManufacturerId"]) : 0,
                                SupplierId = dr["SupplierId"] != DBNull.Value ? Convert.ToInt32(dr["SupplierId"]) : 0,
                                EquipmentTypeId = dr["EquipmentTypeId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentTypeId"]) : 0,
                                EquipmentClassId = dr["EquipmentClassId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentClassId"]) : 0,
                                Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0.0,
                                SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0.0,
                                Cost = dr["Cost"] != DBNull.Value ? Convert.ToDouble(dr["Cost"]) : 0.0,
                                Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0.0,
                                EqOrder = dr["EqOrder"] != DBNull.Value ? Convert.ToInt32(dr["EqOrder"]) : 0,
                                Service = dr["Service"].ToString(),
                                AsOfDate = dr["AsOfDate"] != DBNull.Value ? Convert.ToDateTime(dr["AsOfDate"]) : DateTime.Now,
                                reorderpoint = dr["reorderpoint"] != DBNull.Value ? Convert.ToInt32(dr["reorderpoint"]) : 0,
                                IsActive = Convert.ToBoolean(dr["IsActive"]),
                                Comments = dr["Comments"].ToString(),
                                CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                                LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : DateTime.Now,
                                LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                                EquipmentClass = dr["EquipmentClass"].ToString(),
                                //QtyOnHand = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                ManufacturerName = dr["ManufacturerName"].ToString(),
                            }).ToList();

            TotalEquipmentCount TotalCustomer = new TotalEquipmentCount();
            TotalCustomer = (from DataRow dr in ds.Tables[2].Rows
                             select new TotalEquipmentCount()
                             {
                                 Counter = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                             }).FirstOrDefault();

            var totalCount = ds != null && ds.Tables.Count > 1 ? ds.Tables[2] : null;


            EquipmentListWithCountModel CustomerResultModel = new EquipmentListWithCountModel();
            CustomerResultModel.TotalSupplierCost = ds.Tables[1].Rows[0]["TotalSupplierCost"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["TotalSupplierCost"]) : 0;
            CustomerResultModel.TotalCost = ds.Tables[1].Rows[0]["TotalCost"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["TotalCost"]) : 0;
            CustomerResultModel.TotalRetail = ds.Tables[1].Rows[0]["TotalRetail"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["TotalRetail"]) : 0;
            CustomerResultModel.TotalPoint = ds.Tables[1].Rows[0]["TotalPoint"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalPoint"]) : 0;

            CustomerResultModel.EquipmentList = CustomerList;
            CustomerResultModel.TotalEquipmentCount = TotalCustomer;

            return CustomerResultModel;
        }
        public DataTable DownloadGetProductListByFilter(FilterEquipment filter, DateTime? StartDate, DateTime? EndDate)
        {
            return _InventoryDataAccess.DownloadProductListByFilter(filter, StartDate, EndDate);
        }

        //public List<Equipment> GetAllEquipmentsByCompanyId(Guid companyId)
        //{
        //    DataTable dt = _InventoryDataAccess.GetEqupmentListByComapnyId(companyId);
        //    List<Equipment> EquipmentList = new List<Equipment>();
        //    EquipmentList = (from DataRow dr in dt.Rows
        //                     select new Equipment()
        //                     {
        //                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
        //                         EquipmentId = (Guid)dr["EquipmentId"],
        //                         CompanyId = (Guid)dr["CompanyId"],
        //                         Name = dr["Name"].ToString(),
        //                         SKU = dr["SKU"].ToString(),
        //                         ManufacturerId = dr["ManufacturerId"] != DBNull.Value ? Convert.ToInt32(dr["ManufacturerId"]) : 0,
        //                         SupplierId = dr["SupplierId"] != DBNull.Value ? Convert.ToInt32(dr["SupplierId"]) : 0,
        //                         EquipmentTypeId = dr["EquipmentTypeId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentTypeId"]) : 0,
        //                         EquipmentClassId = dr["EquipmentClassId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentClassId"]) : 0,
        //                         Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0.0,
        //                         SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0.0,
        //                         Cost = dr["Cost"] != DBNull.Value ? Convert.ToDouble(dr["Cost"]) : 0.0,
        //                         Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0.0,
        //                         EqOrder = dr["EqOrder"] != DBNull.Value ? Convert.ToInt32(dr["EqOrder"]) : 0,
        //                         Service = dr["Service"].ToString(),
        //                         AsOfDate = dr["AsOfDate"] != DBNull.Value ? Convert.ToDateTime(dr["AsOfDate"]):DateTime.Now,
        //                         reorderpoint = dr["reorderpoint"] != DBNull.Value ? Convert.ToInt32(dr["reorderpoint"]) : 0,
        //                         IsActive = Convert.ToBoolean(dr["IsActive"]),
        //                         Comments = dr["Comments"].ToString(),
        //                         CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
        //                         LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : DateTime.Now,
        //                         LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
        //                         EquipmentType = dr["EquipmentType"].ToString(),
        //                         QtyOnHand = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0
        //                }).ToList();

        //    return EquipmentList;
        //}
        public StockStatus GetStockStatusByCompanyId(Guid CompanyId)
        {
            int LowStockCount = 0;
            int OutOfStock = 0;
            DataTable dtLowStock = _InventoryDataAccess.GetLowStockQuantityStatus(CompanyId);
            DataTable dtOutOfStock = _InventoryDataAccess.GetOutOfStockQuantityStatus(CompanyId);
            LowStockCount = dtLowStock.Rows.Count;
            OutOfStock = dtOutOfStock.Rows.Count;

            List<Equipment> LowStockEquipmentList = new List<Equipment>();
            LowStockEquipmentList = (from DataRow dr in dtLowStock.Rows
                                     select new Equipment()
                                     {
                                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                         EquipmentId = (Guid)dr["EquipmentId"],
                                         CompanyId = (Guid)dr["CompanyId"],
                                         Name = dr["Name"].ToString(),
                                         SKU = dr["SKU"].ToString(),
                                         ManufacturerId = dr["ManufacturerId"] != DBNull.Value ? Convert.ToInt32(dr["ManufacturerId"]) : 0,
                                         SupplierId = dr["SupplierId"] != DBNull.Value ? Convert.ToInt32(dr["SupplierId"]) : 0,
                                         EquipmentTypeId = dr["EquipmentTypeId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentTypeId"]) : 0,
                                         EquipmentClassId = dr["EquipmentClassId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentClassId"]) : 0,
                                         Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0.0,
                                         SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0.0,
                                         Cost = dr["Cost"] != DBNull.Value ? Convert.ToDouble(dr["Cost"]) : 0.0,
                                         Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0.0,
                                         EqOrder = dr["EqOrder"] != DBNull.Value ? Convert.ToInt32(dr["EqOrder"]) : 0,
                                         Service = dr["Service"].ToString(),
                                         AsOfDate = dr["AsOfDate"] != DBNull.Value ? Convert.ToDateTime(dr["AsOfDate"]) : DateTime.Now,
                                         reorderpoint = dr["reorderpoint"] != DBNull.Value ? Convert.ToInt32(dr["reorderpoint"]) : 0,
                                         IsActive = Convert.ToBoolean(dr["IsActive"]),
                                         Comments = dr["Comments"].ToString(),
                                         CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                                         LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : DateTime.Now,
                                         LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                                         EquipmentClass = dr["EquipmentClass"].ToString(),
                                         QtyOnHand = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0
                                     }).ToList();

            //DataTable dtOutOfStock = _InventoryDataAccess.GetOutOfStockQuantityStatus(CompanyId);

            List<Equipment> OutOfStockEquipmentList = new List<Equipment>();
            OutOfStockEquipmentList = (from DataRow dr in dtOutOfStock.Rows
                                       select new Equipment()
                                       {
                                           Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                           EquipmentId = (Guid)dr["EquipmentId"],
                                           CompanyId = (Guid)dr["CompanyId"],
                                           Name = dr["Name"].ToString(),
                                           SKU = dr["SKU"].ToString(),
                                           ManufacturerId = dr["ManufacturerId"] != DBNull.Value ? Convert.ToInt32(dr["ManufacturerId"]) : 0,
                                           SupplierId = dr["SupplierId"] != DBNull.Value ? Convert.ToInt32(dr["SupplierId"]) : 0,
                                           EquipmentTypeId = dr["EquipmentTypeId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentTypeId"]) : 0,
                                           EquipmentClassId = dr["EquipmentClassId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentClassId"]) : 0,
                                           Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0.0,
                                           SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0.0,
                                           Cost = dr["Cost"] != DBNull.Value ? Convert.ToDouble(dr["Cost"]) : 0.0,
                                           Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0.0,
                                           EqOrder = dr["EqOrder"] != DBNull.Value ? Convert.ToInt32(dr["EqOrder"]) : 0,
                                           Service = dr["Service"].ToString(),
                                           AsOfDate = dr["AsOfDate"] != DBNull.Value ? Convert.ToDateTime(dr["AsOfDate"]) : DateTime.Now,
                                           reorderpoint = dr["reorderpoint"] != DBNull.Value ? Convert.ToInt32(dr["reorderpoint"]) : 0,
                                           IsActive = Convert.ToBoolean(dr["IsActive"]),
                                           Comments = dr["Comments"].ToString(),
                                           CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                                           LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : DateTime.Now,
                                           LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                                           EquipmentClass = dr["EquipmentClass"].ToString(),
                                           QtyOnHand = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0
                                       }).ToList();

            LowStockCount = LowStockEquipmentList.Count();
            OutOfStock = OutOfStockEquipmentList.Count();

            StockStatus ObjStockStatus = new StockStatus();
            ObjStockStatus.LowStockQuantity = LowStockCount;
            ObjStockStatus.OutOfStockQuantity = OutOfStock;

            return ObjStockStatus;
        }
        public bool DeleteEquipmentClass(int ClassId)
        {
            return _EquipmentClassDataAccess.Delete(ClassId) > 0;
        }
        public long InsertInventory(Inventory inventory)
        {
            return _InventoryDataAccess.Insert(inventory);
        }
        public long InsertInventoryPO(InventoryAssignedPO inventoryPO)
        {
            return _InventoryDataAccess.InsertInventoryPO(inventoryPO);
        }
        public bool DeleteEquipment(Guid inventoryId, int equipmentId)
        {
            var iSExist = _InventoryDataAccess.GetByQuery(string.Format("EquipmentId = '{0}'", inventoryId)).FirstOrDefault();
            if (iSExist == null)
            {
                return _EquipmentDataAccess.Delete(equipmentId) > 0;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteInventoryByEquipmentId(Guid EquipmentId)
        {
            return _InventoryDataAccess.DeleteInventoryByEquipmentId(EquipmentId);
        }

        public Inventory GetInventoryProductByProductIdAndCompanyId(Guid productId, Guid CompanyId)
        {
            var Result = _InventoryDataAccess.GetByQuery(string.Format("EquipmentId = '{0}' and CompanyId = '{1}'", productId, CompanyId)).FirstOrDefault();
            Inventory InventoryEquipmentService = new Inventory();
            if (Result != null)
            {

                InventoryEquipmentService.Id = Result.Id;
                InventoryEquipmentService.InventoryId = Result.InventoryId;
                InventoryEquipmentService.EquipmentId = Result.EquipmentId;
                InventoryEquipmentService.CompanyId = Result.CompanyId;
                InventoryEquipmentService.Quantity = Result.Quantity;
                InventoryEquipmentService.Type = Result.Type;
                InventoryEquipmentService.SupplierCost = Result.SupplierCost;
                InventoryEquipmentService.Cost = Result.Cost;
                InventoryEquipmentService.Retail = Result.Retail;
                InventoryEquipmentService.CreatedDate = Result.CreatedDate;
                InventoryEquipmentService.CreatedBy = Result.CreatedBy;

                var CustomEquipmentServiceNameFromEquipmentDataAccess = _EquipmentDataAccess.GetByQuery(string.Format("EquipmentId = '{0}' and CompanyId = '{1}'", productId, CompanyId)).FirstOrDefault();
                InventoryEquipmentService.EquipmentServiceName = CustomEquipmentServiceNameFromEquipmentDataAccess.Name;
            }
            return InventoryEquipmentService;
        }
        public bool UpdateInventoryByInventoryObject(Inventory inventory)
        {
            var result = _InventoryDataAccess.Update(inventory) > 0;
            return result;
        }
        public bool UpdateEquipmentByEquipmentObject(Equipment equipment)
        {
            var result = _EquipmentDataAccess.Update(equipment) > 0;
            return result;
        }
        public CustomInventoryEquipmentModel GetCustomInventoryEquipmentByEquipmentIdAndCompanyId(Guid equipmentId, Guid companyId)
        {
            DataTable dt = _InventoryDataAccess.GetCustomInventoryEquipmentByEquipmentIdAndCompanyId(equipmentId, companyId);
            List<CustomInventoryEquipmentModel> EquipmentInventoryList = new List<CustomInventoryEquipmentModel>();
            EquipmentInventoryList = (from DataRow dr in dt.Rows
                                      select new CustomInventoryEquipmentModel()
                                      {
                                          InventoryIntId = dr["EquipmentIntId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentIntId"]) : 0,
                                          EuipmentIntId = dr["InventoryIntId"] != DBNull.Value ? Convert.ToInt32(dr["InventoryIntId"]) : 0,
                                          EquipmentId = (Guid)dr["EquipmentId"],
                                          CompanyId = (Guid)dr["CompanyId"],
                                          EquipmentCost = dr["EquipmentCost"] != DBNull.Value ? Convert.ToDouble(dr["EquipmentCost"]) : 0.0,
                                          InventoryCost = dr["InventoryCost"] != DBNull.Value ? Convert.ToDouble(dr["InventoryCost"]) : 0.0,
                                          EquipmentAsOfDate = dr["EquipmentAsOfDate"] != DBNull.Value ? Convert.ToDateTime(dr["EquipmentAsOfDate"]) : DateTime.Now,
                                          InventoryQuantity = dr["InventoryQuantity"] != DBNull.Value ? Convert.ToInt32(dr["InventoryQuantity"]) : 0,
                                      }).ToList();

            var EquipmentInventoryArr = EquipmentInventoryList.ToArray();

            return EquipmentInventoryArr[0];
        }

        public List<InventoryTech> GetEquipmentListReportByFilterTechSummary(FilterEquipment filter)
        {
            DataTable dt = _InventoryDataAccess.GetEquipmentListReportByFilterTechSummary(filter);


            List<InventoryTech> EquipmentInventoryList = new List<InventoryTech>();
            EquipmentInventoryList = (from DataRow dr in dt.Rows
                                      select new InventoryTech()
                                      {
                                          Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                          TransferredDate = dr["TransferredDate"] != DBNull.Value ? Convert.ToDateTime(dr["TransferredDate"]) : new DateTime(),
                                          TechnicianName = dr["TechnicianName"].ToString(),
                                          TechnicianId = (Guid)dr["TechnicianId"]


        }).ToList();

            return EquipmentInventoryList;
        }
        public InventoryTechModel GetEquipmentListReportByFilterTechSummary1(FilterEquipment filter)
        {
            //DataTable dt = _InventoryDataAccess.GetEquipmentListReportByFilterTechSummary(filter);
            DataSet ds = _InventoryDataAccess.GetEquipmentListReportByFilterTechSummary1(filter);

            InventoryTechModel Model = new InventoryTechModel();
            Model.InventoryTech = new List<InventoryTech>();
            if (ds != null)
                Model.InventoryTech = (from DataRow dr in ds.Tables[1].Rows
                                             select new InventoryTech()
                                             {
                                                 Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                                 TransferredDate = dr["TransferredDate"] != DBNull.Value ? Convert.ToDateTime(dr["TransferredDate"]) : new DateTime(),
                                                 TechnicianName = dr["TechnicianName"].ToString(),
                                                 TechnicianId = (Guid)dr["TechnicianId"]
                                             }).ToList();
            Model.TotalCount = ds != null && ds.Tables[0].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["TotalCount"]) : 0;
            Model.TotalQuantity = ds != null && ds.Tables[2].Rows[0]["TotalQuantity"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["TotalQuantity"]) : 0;
            Model.PageNo = filter.PageNo;
            Model.PageSize = filter.PageSize;
            return Model;
        }
        public DataTable GetEquipmentListByFilterTechSummaryReport(FilterEquipment filter)
        {
            return _InventoryDataAccess.GetEquipmentListReportByFilterTechSummaryReport(filter);
        }

        public List<InventoryWarehouse> GetCustomInventoryWarehouseEquipmentByEquipmentIdAndCompanyId(Guid equipmentId, Guid companyId)
        {
            DataTable dt = _InventoryDataAccess.GetCustomInventoryWarehouseEquipmentByEquipmentIdAndCompanyId(equipmentId, companyId);
            List<InventoryWarehouse> EquipmentInventoryList = new List<InventoryWarehouse>();
            EquipmentInventoryList = (from DataRow dr in dt.Rows
                                      select new InventoryWarehouse()
                                      {
                                          EquipmentId = (Guid)dr["EquipmentId"],
                                          Name = dr["Name"].ToString(),
                                          Quantity = dr["InventoryQuantity"] != DBNull.Value ? Convert.ToInt32(dr["InventoryQuantity"]) : 0,
                                          LocationId = (Guid)dr["LocationId"]
                                      }).ToList();

            return EquipmentInventoryList;
        }

        public List<InventoryWarehouse> GetCustomInventoryLocationEquipmentByEquipmentIdAndCompanyId(Guid equipmentId, Guid companyId, Guid LocationId)
        {
            if(LocationId==null) LocationId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            DataTable dt = _InventoryDataAccess.GetCustomInventoryLocationEquipmentByEquipmentIdAndCompanyId(equipmentId, companyId, LocationId);
            List<InventoryWarehouse> EquipmentInventoryList = new List<InventoryWarehouse>();
            EquipmentInventoryList = (from DataRow dr in dt.Rows
                                      select new InventoryWarehouse()
                                      {
                                          EquipmentId = (Guid)dr["EquipmentId"],
                                          Name = dr["Name"].ToString(),
                                          Quantity = dr["InventoryQuantity"] != DBNull.Value ? Convert.ToInt32(dr["InventoryQuantity"]) : 0,
                                          LocationId = (Guid)dr["LocationId"]
                                      }).ToList();

            return EquipmentInventoryList;
        }
        public Inventory GetInventoryByEquipmentIdAndCompanyId(Guid EquipmentId, Guid CompanyId)
        {
            return _InventoryDataAccess.GetByQuery(string.Format("EquipmentId = '{0}' and CompanyId='{1}' ", EquipmentId, CompanyId)).FirstOrDefault();
        }

        public Inventory GetInventoryEquipmentQuantityAmountByEquipmentIdAndCompanyId(Guid EquipmentId, Guid CompanyId)
        {
            return _InventoryDataAccess.GetByQuery(string.Format("EquipmentId = '{0}' and CompanyId='{1}' ", EquipmentId, CompanyId)).FirstOrDefault();
        }

        //public List<Equipment> GetAllEquipmentsByFiltering(Guid companyId, int ActiveStatus, int EquipmentClass, int EquipmentCategory, int StockStatus)
        //{
        //    DataTable dt = _InventoryDataAccess.GetEqupmentListByFilters(companyId,ActiveStatus,EquipmentClass,EquipmentCategory,StockStatus);
        //    List<Equipment> EquipmentList = new List<Equipment>();
        //    EquipmentList = (from DataRow dr in dt.Rows
        //                     select new Equipment()
        //                     {
        //                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
        //                         EquipmentId = (Guid)dr["EquipmentId"],
        //                         CompanyId = (Guid)dr["CompanyId"],
        //                         Name = dr["Name"].ToString(),
        //                         SKU = dr["SKU"].ToString(),
        //                         ManufacturerId = dr["ManufacturerId"] != DBNull.Value ? Convert.ToInt32(dr["ManufacturerId"]) : 0,
        //                         SupplierId = dr["SupplierId"] != DBNull.Value ? Convert.ToInt32(dr["SupplierId"]) : 0,
        //                         EquipmentTypeId = dr["EquipmentTypeId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentTypeId"]) : 0,
        //                         EquipmentClassId = dr["EquipmentClassId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentClassId"]) : 0,
        //                         Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0.0,
        //                         SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0.0,
        //                         Cost = dr["Cost"] != DBNull.Value ? Convert.ToDouble(dr["Cost"]) : 0.0,
        //                         Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0.0,
        //                         EqOrder = dr["EqOrder"] != DBNull.Value ? Convert.ToInt32(dr["EqOrder"]) : 0,
        //                         Service = dr["Service"].ToString(),
        //                         AsOfDate = dr["AsOfDate"] != DBNull.Value ? Convert.ToDateTime(dr["AsOfDate"]) : DateTime.Now,
        //                         reorderpoint = dr["reorderpoint"] != DBNull.Value ? Convert.ToInt32(dr["reorderpoint"]) : 0,
        //                         IsActive = Convert.ToBoolean(dr["IsActive"]),
        //                         Comments = dr["Comments"].ToString(),
        //                         CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
        //                         LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : DateTime.Now,
        //                         LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
        //                         EquipmentType = dr["EquipmentType"].ToString(),
        //                         QtyOnHand = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0
        //                     }).ToList();

        //    return EquipmentList;
        //}

        public List<TechnicianInventory> GetAllTechnicianInventoryByEmployeeIdAndCompanyId(Guid EmployeeId, Guid CompanyId)
        {
            DataTable dt = _TechnicianInventoryDataAccess.GetAllTechnicianInventoryByEmployeeIdAndCompanyId(EmployeeId, CompanyId);
            List<TechnicianInventory> TechEquipmentList = new List<TechnicianInventory>();
            TechEquipmentList = (from DataRow dr in dt.Rows
                                 select new TechnicianInventory()
                                 {
                                     Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                     CompanyId = dr["CompanyId"] != DBNull.Value ? (Guid)dr["CompanyId"] : new Guid(),
                                     TechnicianId = dr["TechnicianId"] != DBNull.Value ? (Guid)dr["TechnicianId"] : new Guid(),
                                     EquipmentId = dr["EquipmentId"] != DBNull.Value ? (Guid)dr["EquipmentId"] : new Guid(),
                                     Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                     LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : DateTime.Now,
                                     LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                                     EquipmentName = dr["EquipmentName"].ToString(),
                                     EquipmentType = dr["EquipmentType"].ToString()
                                 }).ToList();

            return TechEquipmentList;
        }

        public TechnicianInventory GetTechnicianInventoryByIdAndCompanyId(int Id, Guid CompanyId, Guid TechnicianId)
        {
            return _TechnicianInventoryDataAccess.GetByQuery(string.Format("Id = '{0}' and CompanyId = '{1}' and TechnicianId ='{2}'", Id, CompanyId, TechnicianId)).FirstOrDefault();
        }
        public long InsertTechnicianInventoryEquipment(TechnicianInventory _TechnicianInventory)
        {
            return _TechnicianInventoryDataAccess.Insert(_TechnicianInventory);
        }
        public bool UpdateTechnicianInventoryEquipment(TechnicianInventory _TechnicianInventory)
        {
            return _TechnicianInventoryDataAccess.Update(_TechnicianInventory) > 0;
        }

        public long InsertPO(PurchaseOrder po)
        {
            return _PurchaseOrderDataAccess.Insert(po);
        }
        public bool UpdatePO(PurchaseOrder po)
        {
            return _PurchaseOrderDataAccess.Update(po) > 0;
        }

        public List<InventoryHistory> GetInventoryListByEquipmentIdAndCompanyId(Guid equipmentId, Guid CompanyId)
        {
            DataTable dt = _InventoryDataAccess.GetInventoryListByEquipmentIdAndCompanyId(equipmentId, CompanyId);
            List<InventoryHistory> InventoryList = new List<InventoryHistory>();
            InventoryList = (from DataRow dr in dt.Rows
                             select new InventoryHistory()
                             {
                                 Description = dr["Description"].ToString(),
                                 LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                 Name = dr["Name"].ToString(),
                                 PurchaseOrderId = dr["PurchaseOrderId"].ToString(),
                                 Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                 Type = dr["Type"].ToString()
                             }).ToList();

            return InventoryList;
        }
        public List<InventoryHistory> GetTop50InventoryListByEquipmentIdAndCompanyId(Guid equipmentId, Guid CompanyId)
        {
            DataTable dt = _InventoryDataAccess.GetTop50InventoryListByEquipmentIdAndCompanyId(equipmentId, CompanyId);
            List<InventoryHistory> InventoryList = new List<InventoryHistory>();
            InventoryList = (from DataRow dr in dt.Rows
                             select new InventoryHistory()
                             {
                                 Description = dr["Description"].ToString(),
                                 LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                 Name = dr["Name"].ToString(),
                                 PurchaseOrderId = dr["PurchaseOrderId"].ToString(),
                                 Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                 Type = dr["Type"].ToString()
                             }).ToList();

            return InventoryList;
        }
        public InventoryHistoryModel GetProductHistory(int pageno, int pagesize, string searchtext, Guid CompanyId, Guid UserId)
        {
            DataSet ds = _InventoryDataAccess.GetProductHistory(pageno, pagesize, searchtext, CompanyId, UserId);
            List<InventoryHistory> InventoryHistoryList = new List<InventoryHistory>();
            InventoryHistoryList = (from DataRow dr in ds.Tables[0].Rows
                            select new InventoryHistory()
                            {
                                EquipmentName = dr["Name"].ToString(),
                                Description = dr["Description"].ToString(),
                                LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                Type = dr["Type"].ToString(),
                                TicketId = dr["TicketId"] != DBNull.Value ? Convert.ToInt32(dr["TicketId"]) : 0
                            }).ToList();
            ProductCount TotalProduct = new ProductCount();
            TotalProduct = (from DataRow dr in ds.Tables[1].Rows
                             select new ProductCount()
                             {
                                 TotalProduct = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                             }).FirstOrDefault();

            var totalCount = ds != null && ds.Tables.Count > 1 ? ds.Tables[1] : null;


            InventoryHistoryModel ProductResultModel = new InventoryHistoryModel();
            ProductResultModel.InventoryHistoryList = InventoryHistoryList;
            ProductResultModel.TotalProductCount = TotalProduct;

            return ProductResultModel;
        }

        public int InsertInventoryWareHouse(InventoryWarehouse inventoryWare)
        {
            return (int)_InventoryWarehouseDataAccess.Insert(inventoryWare);
        }
        public InventoryWarehouse GetInventoryWareByPOId(string poId)
        {
            string query = "PurchaseOrderId='" + poId + "'";
            return _InventoryWarehouseDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public bool DeleteInventoryWareHouseById(int Id)
        {
            return _InventoryWarehouseDataAccess.Delete(Id) > 0;
        }
        public int InsertInventoryBranch(InventoryBranch inventoryBranch)
        {
            return (int)_InventoryBranchDataAccess.Insert(inventoryBranch);
        }
        public InventoryBranch GetInventoryBranchByPOId(string poId)
        {
            string query = "PurchaseOrderId='" + poId + "'";
            return _InventoryBranchDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public bool DeleteInventoryBranchById(int Id)
        {
            return _InventoryBranchDataAccess.Delete(Id) > 0;
        }
        public int InsertInventoryTech(InventoryTech inventoryTech)
        {
            return (int)_InventoryTechDataAccess.Insert(inventoryTech);
        }
        public InventoryTech GetInventoryTechhByPOId(string poId)
        {
            string query = "PurchaseOrderId='" + poId + "'";
            return _InventoryTechDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public int inventoryWareAvailableCount(Guid equipmentId, Guid companyId)
        {
            var Count = 0;
            DataTable dt = _InventoryWarehouseDataAccess.inventoryWareAvailableCount(equipmentId, companyId);
            if (dt != null && dt.Rows.Count > 0)
            {
                Count = dt.Rows[0]["Quantity"] != null ? Convert.ToInt32(dt.Rows[0]["Quantity"]) : 0;
            }
            return Count;
        }
        public int InventoryTechAvailableCount(Guid TechnicianId, Guid EquipmentId)
        {
            var Count = 0;
            DataTable dt = _InventoryTechDataAccess.InventoryTechAvailableCount(TechnicianId, EquipmentId);
            if (dt != null && dt.Rows.Count > 0)
            {
                Count = dt.Rows[0]["Quantity"] != null ? Convert.ToInt32(dt.Rows[0]["Quantity"]) : 0;
            }
            return Count;
        }
        public int AlreadyReleaseCountByCAEId(int CAEId)
        {
            var Count = 0;
            DataTable dt = _InventoryTechDataAccess.AlreadyReleaseCountByCAEId(CAEId);
            if (dt != null && dt.Rows.Count > 0)
            {
                Count = dt.Rows[0]["Quantity"] != null ? Convert.ToInt32(dt.Rows[0]["Quantity"]) : 0;
            }
            return Count;
        }
        public ServiceDetailInfoView GetServiceDetailInfoViewByServiceId(Guid equipmentId)
        {
            return _ServiceDetailInfoViewDataAccess.GetByQuery(string.Format("ServiceId='{0}'", equipmentId)).FirstOrDefault();
        }

        public int InsertServiceDetailInfoView(ServiceDetailInfoView model)
        {
            return (int)_ServiceDetailInfoViewDataAccess.Insert(model);
        }
        public int UpdateServiceDetailInfoView(ServiceDetailInfoView model)
        {
            return (int)_ServiceDetailInfoViewDataAccess.Update(model);
        }

        public List<ServiceDetailInfo> GetServiceInfoByServiceIdAndType(Guid ServiceId, string Type)
        {
            return _ServiceDetailInfoDataAccess.GetByQuery(string.Format(" ServiceId ='{0}' and Type='{1}'", ServiceId, Type));
        }

        public List<ServiceMap> GetServiceModelListByServiceId(Guid equipmentId)
        {
            return _ServiceMapDataAccess.GetServiceModelListByServiceId(equipmentId);
        }

        public int InsertServiceMap(ServiceMap model)
        {
            return (int)_ServiceMapDataAccess.Insert(model);
        }

        public ServiceMap GetServiceMapByServiceMap(ServiceMap model)
        {
            string GetSer = string.Format("CapacityId='{0}' and FinishId='{1}' and LocationId='{2}' and ModelId='{3}' and ServiceId='{4}' and TypeId='{5}'  and ManufacturerId = '{6}'"
                , model.CapacityId, model.FinishId, model.LocationId, model.ModelId, model.ServiceId, model.TypeId, model.ManufacturerId);
            return _ServiceMapDataAccess.GetByQuery(GetSer).FirstOrDefault();
        }

        public bool DeleteServiceMapByServiceId(Guid serviceId)
        {
            return _ServiceMapDataAccess.DeleteServiceMapByServiceId(serviceId);
        }
        public bool InsertEquipmentTechnicianReorderPoint(EquipmentTechnicianReorderPoint reorderPoint)
        {
            return _EquipmentTechnicianReorderPointDataAccess.Insert(reorderPoint) > 0;
        }
        public bool DeleteEquipmentTechnicianReorderPoint(Guid TechnicianId)
        {
            return _EquipmentTechnicianReorderPointDataAccess.DeleteEquipmentTechnicianReorderPoint(TechnicianId);
        }

        public ServiceDetailInfo GetServiceDetailInfoByServiceInfoId(Guid serviceInfoId)
        {
            return _ServiceDetailInfoDataAccess.GetByQuery(string.Format("ServiceInfoId  = '{0}'", serviceInfoId)).FirstOrDefault();
        }

        public bool DeleteServiceDetailInfoById(int id)
        {
            return _ServiceDetailInfoDataAccess.Delete(id) > 0;
        }

        public bool DeleteServiceMapByServiceInfoId(Guid serviceInfoId)
        {
            return _ServiceMapDataAccess.DeleteServiceMapByServiceInfoId(serviceInfoId);
        }

        public ServiceMap GetserviceMapById(int id)
        {
            return _ServiceMapDataAccess.Get(id);
        }

        public bool DeleteServiceMapById(int id)
        {
            return _ServiceMapDataAccess.Delete(id) > 0;
        }

        public EquipmentListWithCountModel GetEquipmentListByCompanyId(Guid companyid, DateTime? start, DateTime? end, string category, string manufact, int pageno, int pagesize, string SearchText, string ProductTypeID, string primaryVendorID,string order)
        {
            DataSet ds = _InventoryDataAccess.GetEquipmentListByCompanyId(companyid, start.Value, end.Value, category, manufact, pageno, pagesize, SearchText, ProductTypeID, primaryVendorID,order);
            List<Equipment> CustomerList = new List<Equipment>();
            TotalEquipmentCount TotalCustomer = new TotalEquipmentCount();
            TotalCustomer = (from DataRow dr in ds.Tables[0].Rows
                             select new TotalEquipmentCount()
                             {
                                 Counter = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                             }).FirstOrDefault();

            var totalCount = ds != null && ds.Tables.Count > 1 ? ds.Tables[1] : null;
            CustomerList = (from DataRow dr in ds.Tables[1].Rows
                            select new Equipment()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                EquipmentId = (Guid)dr["EquipmentId"],
                                CompanyId = (Guid)dr["CompanyId"],
                                Name = dr["Name"].ToString(),
                                SKU = dr["SKU"].ToString(),
                                ManufacturerId = dr["ManufacturerId"] != DBNull.Value ? Convert.ToInt32(dr["ManufacturerId"]) : 0,
                                SupplierId = dr["SupplierId"] != DBNull.Value ? Convert.ToInt32(dr["SupplierId"]) : 0,
                                EquipmentTypeId = dr["EquipmentTypeId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentTypeId"]) : 0,
                                EquipmentClassId = dr["EquipmentClassId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentClassId"]) : 0,
                                Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0.0,
                                //SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0.0,
                                SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0.0,
                                Cost = dr["Cost"] != DBNull.Value ? Convert.ToDouble(dr["Cost"]) : 0.0,
                                Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0.0,
                                EqOrder = dr["EqOrder"] != DBNull.Value ? Convert.ToInt32(dr["EqOrder"]) : 0,
                                Service = dr["Service"].ToString(),
                                AsOfDate = dr["AsOfDate"] != DBNull.Value ? Convert.ToDateTime(dr["AsOfDate"]) : DateTime.Now,
                                reorderpoint = dr["reorderpoint"] != DBNull.Value ? Convert.ToInt32(dr["reorderpoint"]) : 0,
                                IsActive = Convert.ToBoolean(dr["IsActive"]),
                                Comments = dr["Comments"].ToString(),
                                CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                                LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : DateTime.Now,
                                LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                                EquipmentClass = dr["EquipmentClass"].ToString(),
                                QtyOnHand = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                SupplierName = dr["SupplierName"].ToString(),
                                ManufacturerName = dr["ManufacturerName"].ToString(),
                                Category = dr["Category"].ToString(),
                                RackNo = dr["RackNo"].ToString()
                            }).ToList();
            
            TotalVendorCost VendorCost = new TotalVendorCost();
            VendorCost = (from DataRow dr in ds.Tables[2].Rows
                          select new TotalVendorCost()
                          {
                              VendorCost = dr["TotalCost"] != DBNull.Value ? Convert.ToDouble(dr["TotalCost"]) : 0.0,
                              TotalQuantity = dr["TotalQuantity"] != DBNull.Value ? Convert.ToDouble(dr["TotalQuantity"]) : 0.0
                          }).FirstOrDefault();


            EquipmentListWithCountModel CustomerResultModel = new EquipmentListWithCountModel();
            CustomerResultModel.EquipmentList = CustomerList;
            CustomerResultModel.TotalEquipmentCount = TotalCustomer;
            CustomerResultModel.TotalVendorCost = VendorCost;

            return CustomerResultModel;
        }

        public DataTable GetGetEquipmentReportByCompanyId(Guid companyid, DateTime? start, DateTime? end, string category, string manufact, string SearchText, string ProductTypeID, string primaryVendorID)
        {
            return _InventoryDataAccess.GetEquipmentListReportByCompanyId(companyid, start.Value, end.Value, category, manufact, SearchText, ProductTypeID, primaryVendorID);
        }

        public DataTable GetEquipmentListReportByFilterTech(FilterEquipment filter)
        {
            return _InventoryDataAccess.GetEquipmentListReportByFilterTech(filter);
        }
        public DataTable GetTranferListReportByFilterTech(FilterEquipment filter)
        {
            return _InventoryDataAccess.GetTranferListReportByFilterTech(filter);
        }
        public DataTable GetPendingListReportByFilterTech(FilterEquipment filter)
        {
            return _InventoryDataAccess.GetPendingListReportByFilterTech(filter);
        }
        public List<InventoryTech> GetTransferInventoryForTechAndDateFilter(Guid techid, string transferfirstdate, string transferlastdate)
        {
            DataTable dt = _InventoryDataAccess.GetTransferInventoryForTechAndDateFilter(techid, transferfirstdate, transferlastdate);
            List<InventoryTech> InventoryList = new List<InventoryTech>();
            InventoryList = (from DataRow dr in dt.Rows
                             select new InventoryTech()
                             {
                                 LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                 Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                 Category = dr["Category"].ToString(),
                                 Manufacture = dr["Manufacture"].ToString(),
                                 EquipName = dr["EquipName"].ToString(),
                                 Desc = dr["Desc"].ToString(),
                                 Sku = dr["Sku"].ToString(),
                                 Technician = dr["Technician"].ToString(),

                             }).ToList();

            return InventoryList;
        }

        public InventoryTech GetInventoryTechByTechnicianIdAndEquipmentId(Guid techid, Guid equipid)
        {
            return _InventoryTechDataAccess.GetByQuery(string.Format("TechnicianId = '{0}' and EquipmentId = '{1}'", techid, equipid)).FirstOrDefault();
        }

        public List<InventoryTech> GetAllInventoryTechByTechnicianIdAndEquipmentIdAndType(Guid techid, Guid equipid)
        {
            return _InventoryTechDataAccess.GetByQuery(string.Format("TechnicianId = '{0}' and EquipmentId = '{1}'", techid, equipid)).ToList();
        }

        public bool UpdateInventoryTech(InventoryTech tech)
        {
            return _InventoryTechDataAccess.Update(tech) > 0;
        }

        public InventoryWarehouse GetInventoryWareHouseByEquipmentIdAndType(Guid equpid)
        {
            return _InventoryWarehouseDataAccess.GetByQuery(string.Format("EquipmentId = '{0}' and Type = 'Add'", equpid)).FirstOrDefault();
        }

        public bool UpdateWareHouse(InventoryWarehouse ware)
        {
            return _InventoryWarehouseDataAccess.Update(ware) > 0;
        }

        public AssignedInventoryTechReceived GetTechReceiveByEquipmentIdAndTechnicianIdAndReceived(Guid eqpid, Guid techid)
        {
            return _AssignedInventoryTechReceivedDataAccess.GetByQuery(string.Format("TechnicianId = '{0}' and EquipmentId = '{1}' and IsReceived = 0 and IsDecline = 0", techid, eqpid)).FirstOrDefault();
        }

        public bool UpdateTechReceive(AssignedInventoryTechReceived tr)
        {
            return _AssignedInventoryTechReceivedDataAccess.Update(tr) > 0;
        }

        public long InsertTechReceive(AssignedInventoryTechReceived tr)
        {
            return _AssignedInventoryTechReceivedDataAccess.Insert(tr);
        }

        public TechReceiveListModel GetAllTechReceiveByTechnicianId(TechReceiveFilter filter)
        {
            //
            //DataSet ds = _InventoryDataAccess.GetAllTechReceiveByTechnicianId(filter);
            DataSet ds = _InventoryDataAccess.GetAllTechTransferActivity(filter);
            TechReceiveListModel Model = new TechReceiveListModel();
            if (ds != null)
                Model.ListAssignedInventoryTechReceived = (from DataRow dr in ds.Tables[0].Rows
                                                           select new AssignedInventoryTechReceived()
                                                           {
                                                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                               TechnicianId = (Guid)dr["TechnicianId"],
                                                               EquipmentId = (Guid)dr["EquipmentId"],
                                                               IsReceived = dr["IsReceived"] != DBNull.Value ? Convert.ToBoolean(dr["IsReceived"]) : false,
                                                               ReceivedBy = (Guid)dr["ReceivedBy"],
                                                               ReceivedDate = dr["ReceivedDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReceivedDate"]) : new DateTime(),
                                                               CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                                               Name = dr["Name"].ToString(),
                                                               SKU = dr["SKU"].ToString(),
                                                               Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                                               ReceivedByName = dr["ReceivedByName"].ToString(),
                                                               TotalQuantity = dr["TotalQuantity"] != DBNull.Value ? Convert.ToInt32(dr["TotalQuantity"]) : 0,
                                                               IsApprove = dr["IsApprove"] != DBNull.Value ? Convert.ToBoolean(dr["IsApprove"]) : false,
                                                               Type = dr["Type"].ToString(),
                                                               IsDecline = dr["IsDecline"] != DBNull.Value ? Convert.ToBoolean(dr["IsDecline"]) : false,
                                                           }).ToList();
            Model.ListAssignedInventoryTechApprove = (from DataRow dr in ds.Tables[2].Rows
                                                      select new AssignedInventoryTechReceived()
                                                      {
                                                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                          TechnicianId = (Guid)dr["TechnicianId"],
                                                          EquipmentId = (Guid)dr["EquipmentId"],
                                                          IsReceived = dr["IsReceived"] != DBNull.Value ? Convert.ToBoolean(dr["IsReceived"]) : false,
                                                          ReceivedBy = (Guid)dr["ReceivedBy"],
                                                          ReceivedDate = dr["ReceivedDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReceivedDate"]) : new DateTime(),
                                                          CreatedDate = dr["ReceivedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                                          Name = dr["Name"].ToString(),
                                                          SKU = dr["SKU"].ToString(),
                                                          Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                                          ReceivedByName = dr["ReceivedByName"].ToString(),
                                                          TotalQuantity = dr["TotalQuantity"] != DBNull.Value ? Convert.ToInt32(dr["TotalQuantity"]) : 0,
                                                          IsApprove = dr["IsApprove"] != DBNull.Value ? Convert.ToBoolean(dr["IsApprove"]) : false,
                                                          Type = dr["Type"].ToString(),
                                                          IsDecline = dr["IsDecline"] != DBNull.Value ? Convert.ToBoolean(dr["IsDecline"]) : false,
                                                      }).ToList();
            Model.TotalCountTrf = ds != null && ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;
            Model.TotalCountRcv = ds != null && ds.Tables[3].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[3].Rows[0]["TotalCount"]) : 0;
            Model.Searchtext = filter.Searchtext;
            Model.PageNo = filter.PageNo;
            Model.PageSize = filter.PageSize;
            return Model;
        }

        public List<AssignedInventoryTechReceived> GetTechReceiveByEquipmentIdAndReceived(Guid eqpid)
        {
            return _AssignedInventoryTechReceivedDataAccess.GetByQuery(string.Format("EquipmentId = '{0}' and IsReceived = 0 and IsDecline = 0", eqpid)).ToList();
        }

        public AssignedInventoryTechReceived GetTechReceiveByEquipmentIdAndTechnicianIdAndReceivedAndApprove(Guid eqpid, Guid techid)
        {
            return _AssignedInventoryTechReceivedDataAccess.GetByQuery(string.Format("EquipmentId = '{0}' and TechnicianId = '{1}' and IsReceived = 0 and IsApprove = 0 and IsDecline = 0", eqpid, techid)).FirstOrDefault();
        }

        public void DeleteInventoryTechReceive(int id)
        {
            _AssignedInventoryTechReceivedDataAccess.Delete(id);
        }

        public AssignedInventoryTechReceived GetTechReceiveById(int value)
        {
            return _AssignedInventoryTechReceivedDataAccess.Get(value);
        }

        public AssignedInventoryTechReceived GetTechReceiveByEquipmentIdAndTechnicianIdAndReceivedAndIsApprove(Guid eqpid, Guid techid, int qty)
        {
            return _AssignedInventoryTechReceivedDataAccess.GetByQuery(string.Format("EquipmentId = '{0}' and TechnicianId = '{1}' and IsReceived = 0 and IsApprove = 1 and Quantity = {2} and IsDecline = 0", eqpid, techid, qty)).FirstOrDefault();
        }

        public bool DeleteInventoryTechByCustomerAppointmentEquipmentIdAndType(int value)
        {
            return _InventoryTechDataAccess.DeleteInventoryTechByCustomerAppointmentEquipmentIdAndType(value);
        }

        public List<EquipmentsFavourite> GetAllEquipmentsFavouriteByEquipmentId(Guid eqpid, Guid comid)
        {
            return _EquipmentsFavouriteDataAccess.GetByQuery(string.Format("EquipmentId = '{0}' and CompanyId = '{1}'", eqpid, comid)).ToList();
        }

        public int InsertEquipmentsFavourite(EquipmentsFavourite ef)
        {
            return (int)_EquipmentsFavouriteDataAccess.Insert(ef);
        }

        #region Usage by Account
        public UsagebyAccount GetUsagebyAccountReportList(DateTime? Start, DateTime? End, int pageno, int pagesize, string searchtext,string order)
        {
            DataSet dsResult = _InventoryDataAccess.GetUsagebyAccountReportList(Start, End, pageno, pagesize, searchtext,order);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            DataTable dt2 = dsResult.Tables[2];

            UsagebyAccount Model = new UsagebyAccount();
            List<UsagebyAccount> UsagebyAccountList = new List<UsagebyAccount>();
            TotalEquipmentCount Count = new TotalEquipmentCount();
            UsagebyAccountQuantity TotalQuantity = new UsagebyAccountQuantity();
            if (dt != null)
                UsagebyAccountList = (from DataRow dr in dt1.Rows
                                      select new UsagebyAccount()
                                      {
                                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                          CustomerId = (Guid)dr["CustomerId"],
                                          CustomerNo = dr["CustomerNo"].ToString(),
                                          CustomerName = dr["CustomerName"].ToString(),
                                          InstalledQuantity = dr["InstalledQuantity"] != DBNull.Value ? Convert.ToInt32(dr["InstalledQuantity"]) : 0,
                                          ServiceQuantity = dr["ServiceQuantity"] != DBNull.Value ? Convert.ToInt32(dr["ServiceQuantity"]) : 0,

                                      }).ToList();
            Count = (from DataRow dr in dt.Rows
                     select new TotalEquipmentCount()
                     {
                         Counter = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                     }).FirstOrDefault();
            TotalQuantity = (from DataRow dr in dt2.Rows
                             select new UsagebyAccountQuantity()
                             {
                                 TotalInstalled = dr["TotalInstalled"] != DBNull.Value ? Convert.ToInt32(dr["TotalInstalled"]) : 0,
                                 TotalService = dr["TotalService"] != DBNull.Value ? Convert.ToInt32(dr["TotalService"]) : 0
                             }).FirstOrDefault();
            UsagebyAccount UsagebyAccount = new UsagebyAccount();
            UsagebyAccount.UsagebyAccountList = UsagebyAccountList;
            UsagebyAccount.Count = Count;
            UsagebyAccount.TotalCount = TotalQuantity;
            return UsagebyAccount;
        }
        public UsageEquipmentList GetUsagebyAccountReportEquipmentList(Guid CustomerId, DateTime? Start, DateTime? End)
        {
            DataSet dsResult = _InventoryDataAccess.GetUsagebyAccountReportEquipmentList(CustomerId, Start, End);
            DataTable dt = dsResult.Tables[0];
            UsageEquipmentList Model = new UsageEquipmentList();
            List<UsageEquipmentList> UsageEquipmentList = new List<UsageEquipmentList>();
            if (dt != null)
                UsageEquipmentList = (from DataRow dr in dt.Rows
                                      select new UsageEquipmentList()
                                      {
                                          EquipName = dr["EquipName"].ToString(),
                                          TicketIntId = dr["TicketIntId"] != DBNull.Value ? Convert.ToInt32(dr["TicketIntId"]) : 0,
                                          Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,

                                      }).ToList();
            UsageEquipmentList EquipmentList = new UsageEquipmentList();
            EquipmentList.UsageEquipmentPartialList = UsageEquipmentList;
            return EquipmentList;
        }
        public UsageEquipmentList GetUsagebyAccountReportServiceEquipmentList(Guid CustomerId, DateTime? Start, DateTime? End)
        {
            DataSet dsResult = _InventoryDataAccess.GetUsagebyAccountReportServiceEquipmentList(CustomerId, Start, End);
            DataTable dt = dsResult.Tables[0];
            UsageEquipmentList Model = new UsageEquipmentList();
            List<UsageEquipmentList> UsageEquipmentList = new List<UsageEquipmentList>();
            if (dt != null)
                UsageEquipmentList = (from DataRow dr in dt.Rows
                                      select new UsageEquipmentList()
                                      {
                                          EquipName = dr["EquipName"].ToString(),
                                          TicketIntId = dr["TicketIntId"] != DBNull.Value ? Convert.ToInt32(dr["TicketIntId"]) : 0,
                                          Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,

                                      }).ToList();
            UsageEquipmentList EquipmentList = new UsageEquipmentList();
            EquipmentList.UsageEquipmentPartialList = UsageEquipmentList;
            return EquipmentList;
        }
        public DataTable DownloadUsagebyAccountReportPartialList(DateTime? Start, DateTime? End, string searchtext)
        {
            DataTable dt = _InventoryDataAccess.DownloadUsagebyAccountReportPartialList(Start, End, searchtext);
            return dt;
        }
        #endregion

        #region RMA Report
        public RMAEquipment GetRMAEquipmentList(DateTime? Start, DateTime? End, int pageno, int pagesize, string searchtext, string order)
        {
            DataSet dsResult = _InventoryDataAccess.GetRMAEquipmentReport(Start, End, pageno, pagesize, searchtext,order);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            DataTable dt2 = dsResult.Tables[2];
            RMAEquipment Model = new RMAEquipment();
            List<RMAEquipment> RMAEquipmentList = new List<RMAEquipment>();
            PurchaseOrderCount TotalRMA = new PurchaseOrderCount();
            TotalEquipmentCount Count = new TotalEquipmentCount();
            if (dt != null)
                RMAEquipmentList = (from DataRow dr in dt1.Rows
                                      select new RMAEquipment()
                                      {
                                          //Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                          SKU = dr["SKU"].ToString(),
                                          EquipmentName = dr["Name"].ToString(),
                                          Description = dr["Description"].ToString(),
                                          Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                          AmountDouble = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                                          Createdby = dr["Createdby"].ToString(),
                                          RMADate = dr["RMADate"] != DBNull.Value ? Convert.ToDateTime(dr["RMADate"]) : new DateTime()
                                      }).ToList();
            Count = (from DataRow dr in dt.Rows
                     select new TotalEquipmentCount()
                     {
                         Counter = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                     }).FirstOrDefault();
            TotalRMA = (from DataRow dr in dt2.Rows
                     select new PurchaseOrderCount()
                     {
                         TotalQuantity = dr["TotalQuantity"] != DBNull.Value ? Convert.ToInt32(dr["TotalQuantity"]) : 0,
                         TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0.0
                     }).FirstOrDefault();
            RMAEquipment RMAEquipment = new RMAEquipment();
            RMAEquipment.RMAEquipmentList = RMAEquipmentList;
            RMAEquipment.Count = Count;
            RMAEquipment.TotalRMA = TotalRMA;
            return RMAEquipment;
        }
        public DataTable DownloadRMAReport(DateTime? Start, DateTime? End, string searchtext)
        {
            DataTable dt = _InventoryDataAccess.DownloadRMAReport(Start, End, searchtext);
            return dt;
        }
        #endregion

        #region Purchase Order Report
        public PurchaseOrderReportList GetPurchaseOrderList(DateTime? Start, DateTime? End, int pageno, int pagesize, string searchtext,string StatusIDList,string order)
        {
            DataSet dsResult = _InventoryDataAccess.GetPurchaseOrderList(Start, End, pageno, pagesize, searchtext, StatusIDList, order);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            DataTable dt2 = dsResult.Tables[2];
            PurchaseOrderReportList Model = new PurchaseOrderReportList();
            List<PurchaseOrderReportList> PurchaseOrderReportList = new List<PurchaseOrderReportList>();
            TotalEquipmentCount Count = new TotalEquipmentCount();
            PurchaseOrderCount TotalCount = new PurchaseOrderCount();
            Count = (from DataRow dr in dt.Rows
                     select new TotalEquipmentCount()
                     {
                         Counter = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                     }).FirstOrDefault();
            if (dt != null)
                PurchaseOrderReportList = (from DataRow dr in dt1.Rows
                                    select new PurchaseOrderReportList()
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        PurchaseOrderId = dr["PurchaseOrderId"].ToString(),
                                        SupplierName = dr["CompanyName"].ToString(),
                                        TotalAmountDouble = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                                        Status = dr["Status"].ToString(),
                                        Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                        CreatedBy = dr["CreatdBy"].ToString()
                                    }).ToList();
            TotalCount = (from DataRow dr in dt2.Rows
                                 select new PurchaseOrderCount()
                                 {
                                     TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0.0,
                                     TotalQuantity = dr["TotalQuantity"] != DBNull.Value ? Convert.ToInt32(dr["TotalQuantity"]) : 0,
                                 }).FirstOrDefault();

            PurchaseOrderReportList PurchaseOrderList = new PurchaseOrderReportList();
            PurchaseOrderList.PurchaseOrderList = PurchaseOrderReportList;
            PurchaseOrderList.Count = Count;
            PurchaseOrderList.TotalCount = TotalCount;
            return PurchaseOrderList;
        }
        public PurchaseOrderItemList GetPurchaseOrderItemList(string Id, DateTime? Start, DateTime? End)
        {
            DataSet dsResult = _InventoryDataAccess.GetPurchaseOrderPartialList(Id, Start, End);
            DataTable dt = dsResult.Tables[0];
            PurchaseOrderItemList Model = new PurchaseOrderItemList();
            List<PurchaseOrderItemList> PurchaseOrderItem = new List<PurchaseOrderItemList>();
            if (dt != null)
                PurchaseOrderItem = (from DataRow dr in dt.Rows
                                      select new PurchaseOrderItemList()
                                      {
                                          EquipName = dr["EquipName"].ToString(),
                                          Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                          UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToInt32(dr["UnitPrice"]) : 0,
                                          TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToInt32(dr["TotalPrice"]) : 0,
                                      }).ToList();
            PurchaseOrderItemList PurchaseOrderItemList = new PurchaseOrderItemList();
            PurchaseOrderItemList.PurchaseOrderItem = PurchaseOrderItem;
            return PurchaseOrderItemList;
        }
        public DataTable DownloadPurchaseOrderReport(DateTime? Start, DateTime? End, string searchtext,string StatusIDList)
        {
            DataTable dt = _InventoryDataAccess.DownloadPurchaseOrderReport(Start, End, searchtext, StatusIDList);
            return dt;
        }
        //public PurchaseOrderReportList GetPurchaseOrderStatus()
        //{
        //    DataSet dsResult = _InventoryDataAccess.GetPurchaseOrderStatus();
        //    DataTable dt = dsResult.Tables[0];

        //    PurchaseOrderReportList Model = new PurchaseOrderReportList();
        //    List<PurchaseOrderReportList> PurchaseOrderReportList = new List<PurchaseOrderReportList>();

        //    if (dt != null)
        //        PurchaseOrderReportList = (from DataRow dr in dt.Rows
        //                                   select new PurchaseOrderReportList()
        //                                   {
        //                                       Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                            
        //                                       Status = dr["Status"].ToString(),
                                               
        //                                   }).ToList();

        //    PurchaseOrderReportList PurchaseOrderList = new PurchaseOrderReportList();
        //    PurchaseOrderList.PurchaseOrderList = PurchaseOrderReportList;
   
        //    return PurchaseOrderList;
        //}
        public List<PurchaseOrderWarehouse> GetPurchaseOrderStatus()
        {
            DataTable dt = _InventoryDataAccess.GetPurchaseOrderStatus();

            //DataSet dsResult = _InventoryDataAccess.GetPurchaseOrderStatus();
            List<PurchaseOrderWarehouse> POWList = new List<PurchaseOrderWarehouse>();
            POWList = (from DataRow dr in dt.Rows
                            select new PurchaseOrderWarehouse()
                            {
                                Status = dr["Status"].ToString(),

                            }).ToList();
            return POWList;
        }
        #endregion
        public DataTable FilterEquipmentsListDownload(FilterEquipment filter)
        {
            DataTable dt = _InventoryDataAccess.GetEquipmentListForDownload(filter);
            return dt;
        }

        public Equipment GetEquipmentByName(string EquipmentName)
        {
            return _EquipmentDataAccess.GetByQuery(string.Format("Name = '{0}'", EquipmentName)).FirstOrDefault();
        }

        #region Digiture

        public DetailedHistoryVM GetDetailedHistoryListByFilters(DetailedHistoryFilter filters)
        {
            DataSet ds = _InventoryDataAccess.GetDetailedHistoryListByFilters(filters);
            DetailedHistoryVM Model = new DetailedHistoryVM();
            Model.Items = new List<DetailedHistoryItem>();

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                Model.Items = (from DataRow dr in ds.Tables[0].Rows
                                    select new DetailedHistoryItem()
                                    {
                                        EquipmentId = dr["EquipmentId"].ToString(),
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        EquipmentName = dr["Description"].ToString(),
                                        SKU = dr["SKU"].ToString(),
                                        MfgId = dr["MfgId"] != DBNull.Value ? Convert.ToInt32(dr["MfgId"]) : 0,
                                        Manufacturer = dr["Manufacturer"].ToString(),
                                        Start = dr["OpnBal"] != DBNull.Value? Convert.ToInt32(dr["OpnBal"]) : 0,
                                        AddToTicket = dr["Added"] != DBNull.Value? Convert.ToInt32(dr["Added"]) : 0,
                                        PulledFromTicket = dr["Pulled"] != DBNull.Value? Convert.ToInt32(dr["Pulled"]) : 0,
                                        TransferredOut = dr["TrfOut"] != DBNull.Value? Convert.ToInt32(dr["TrfOut"]) : 0,
                                        TransferredIn = dr["TrfIn"] != DBNull.Value? Convert.ToInt32(dr["TrfIn"]) : 0,
                                        OnHand = dr["OnHand"] != DBNull.Value? Convert.ToInt32(dr["OnHand"]) : 0,
                                    }).ToList();
            Model.ItemsCount = ds.Tables[1].Rows.Count > 0 ? Convert.ToInt32(ds.Tables[1].Rows[0][0]) : 0;
            
            Model.EquipmentsList= ds.Tables[2].ToList<GenericDropDownFilter>();
            Model.ManufacturersList = ds.Tables[3].ToList<GenericDropDownFilter>();

            //Model.TotalCount = ds != null && ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;
            //Model.Searchtext = filters.Searchtext;
            //Model.PageNo = filters.PageNo;
            //Model.PageSize = filters.PageSize;
            return Model;
        }

        public DataSet GetDetailedHistoryListByFiltersR(DetailedHistoryFilter filters)
        {
            DataSet ds = _InventoryDataAccess.GetDetailedHistoryListByFilters(filters);
            //ds.Tables[0].Columns.Remove("columnName");
            ds.Tables[0].Columns.RemoveAt(0);
            ds.Tables[0].Columns.RemoveAt(0);
            ds.Tables[0].Columns[0].ColumnName = "Description";
            ds.Tables[0].Columns.RemoveAt(2);
            ds.Tables[0].Columns[3].ColumnName = "Start";
            ds.Tables[0].Columns[4].ColumnName = "AddToTicket";
            ds.Tables[0].Columns[5].ColumnName = "PulledFromTicket";
            ds.Tables[0].Columns[6].ColumnName = "TransferredOut";
            ds.Tables[0].Columns[7].ColumnName = "TransferredIn";
            ds.Tables[0].Columns.RemoveAt(9);
            ds.Tables[0].Columns.RemoveAt(9);
            return ds;
        }

        public DetailedEquipmentVM GetDetailedEquipmentListByFilters(DetailedHistoryFilter filters)
        {
            DataSet ds = _InventoryDataAccess.GetDetailedEquipmentTicketsByFilters(filters);
            DataTable ticketsDT = ds.Tables[0];
            
            DetailedEquipmentVM Model = new DetailedEquipmentVM();
            Model.Tickets = new List<DetailedEquipmentTicketListItem>();
            Model.TicketsCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
            if (ticketsDT != null)
                Model.Tickets = (from DataRow dr in ticketsDT.Rows
                                  select new DetailedEquipmentTicketListItem()
                                  {
                                      TicketId = dr["TicketNo"] != DBNull.Value ? Convert.ToInt32(dr["TicketNo"]) : 0,
                                      Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                      EquipmentId = dr["EquipmentId"].ToString(),
                                      EquipmentName = dr["Name"].ToString(),
                                      SKU = dr["SKU"].ToString(),
                                      Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                      Date = dr["Date"].ToString()
                                  }).ToList();
            
            //ds = _InventoryDataAccess.GetDetailedEquipmentTransfersByFilters(filters);
            ds = _InventoryDataAccess.GetDetailedEquipmentTransfersByFilters(filters);
            DataTable ticketsTR = ds.Tables[0];
            Model.Transfers = new List<DetailedEquipmentTransferListItem>();
            //Model.TransfersCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
            Model.TransfersCount = Model.Transfers.Count;
            if (ticketsTR != null)
                Model.Transfers = (from DataRow dr in ticketsTR.Rows
                                 select new DetailedEquipmentTransferListItem()
                                 {
                                     TechName = dr["TTech"].ToString() + " ➡️ " + dr["RTech"].ToString(),
                                     //EquipmentId = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                     EquipmentName = dr["Equipment"].ToString(),
                                     SKU = dr["SKU"].ToString(),
                                     Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                     Date = dr["RDate"].ToString(),
                                     _ReceivedDate= dr["RDate"] != DBNull.Value ? Convert.ToDateTime(dr["RDate"]) : new DateTime(),
                                     _CreatedDate  = dr["TDate"] != DBNull.Value ? Convert.ToDateTime(dr["TDate"]) : new DateTime(),

                                 }).ToList();

            /// Mayur :: added comment since new query fetch warehouse details directly no need to check employee id contains wharehouse
            //if (filters.EmployeeIds.Contains("22222222-2222-2222-2222-222222222222"))
            //{
            //    ds = _InventoryDataAccess.GetDetailedEquipmentTransfersByFiltersWH(filters);
            //    ticketsTR = ds.Tables[0];
            //    //Model.TransfersCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
            //    Model.TransfersCount += ticketsTR.Rows.Count;
            //    if (ticketsTR != null)
            //    {
            //        List<DetailedEquipmentTransferListItem> TransfersWH = new List<DetailedEquipmentTransferListItem>();
            //        TransfersWH = (from DataRow dr in ticketsTR.Rows
            //                           select new DetailedEquipmentTransferListItem()
            //                           {
            //                               TechName = dr["TTech"].ToString() + " ➡️ " + dr["RTech"].ToString(),
            //                               //EquipmentId = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
            //                               EquipmentName = dr["Equipment"].ToString(),
            //                               SKU = dr["SKU"].ToString(),
            //                               Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
            //                               _ReceivedDate = dr["RDate"] != DBNull.Value ? Convert.ToDateTime(dr["RDate"]) : new DateTime(),
            //                               _CreatedDate = dr["TDate"] != DBNull.Value ? Convert.ToDateTime(dr["TDate"]) : new DateTime(),
            //                               Date = dr["RDate"].ToString()
            //                           }).ToList();
            //        Model.Transfers.AddRange(TransfersWH);
            //    }

            //}
            /// Mayur :: added comment since new query fetch warehouse details directly no need to check employee id contains wharehouse


            //Model.TotalCount = ds != null && ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;
            //Model.Searchtext = filters.Searchtext;
            //Model.PageNo = filters.PageNo;
            //Model.PageSize = filters.PageSize;
            return Model;
        }

        public DataSet GetTechTransferListByFiltersR(TechTransferFilter filter)
        {
            TechReceiveListModel Model = new TechReceiveListModel();
            DataSet ds = new DataSet();
            try
            {
                ds = _InventoryDataAccess.GetTechTransferListByFilters(filter);

                ds.Tables.Remove("Default1");
                ds.Tables.Remove("Default2");
                ds.Tables.Remove("Default4");
                ds.Tables.Remove("Default5");

                ds.Tables[0].Columns.RemoveAt(0);
                ds.Tables[0].Columns.RemoveAt(0);
                ds.Tables[0].Columns.RemoveAt(0);
                ds.Tables[0].Columns.RemoveAt(0);
                ds.Tables[0].Columns.RemoveAt(0);
                ds.Tables[0].Columns[1].ColumnName = "RequestOn";
                ds.Tables[0].Columns[2].ColumnName = "Description";
                ds.Tables[0].Columns[5].ColumnName = "TransferTo";
                ds.Tables[0].Columns[6].ColumnName = "OnHand";
                ds.Tables[0].Columns[7].ColumnName = "Approved";
                ds.Tables[0].Columns[9].ColumnName = "Declined";
                ds.Tables[0].Columns.RemoveAt(10);
                ds.Tables[0].Columns.RemoveAt(10);

                ds.Tables[1].Columns.RemoveAt(0);
                ds.Tables[1].Columns.RemoveAt(0);
                ds.Tables[1].Columns.RemoveAt(0);
                ds.Tables[1].Columns.RemoveAt(0);
                ds.Tables[1].Columns.RemoveAt(0);
                ds.Tables[1].Columns[1].ColumnName = "RequestOn";
                ds.Tables[1].Columns[2].ColumnName = "Description";
                ds.Tables[1].Columns[5].ColumnName = "TransferFrom";
                ds.Tables[1].Columns[6].ColumnName = "OnHand";
                ds.Tables[1].Columns[7].ColumnName = "Approved";
                ds.Tables[1].Columns[9].ColumnName = "Declined";


                //ds.Tables[0].Columns.RemoveAt(0);
                //ds.Tables[0].Columns[0].ColumnName = "Description";
                //ds.Tables[0].Columns.RemoveAt(2);
                //ds.Tables[0].Columns[3].ColumnName = "Start";
                //ds.Tables[0].Columns[4].ColumnName = "AddToTicket";
                //ds.Tables[0].Columns[5].ColumnName = "PulledFromTicket";
                //ds.Tables[0].Columns[6].ColumnName = "TransferredOut";
                //ds.Tables[0].Columns[7].ColumnName = "TransferredIn";
                //ds.Tables[0].Columns.RemoveAt(9);
                //ds.Tables[0].Columns.RemoveAt(9);

                //if (ds != null)
                //{
                //    Model.ListAssignedInventoryTechApprove = (from DataRow dr in ds.Tables[0].Rows
                //                                              select new AssignedInventoryTechReceived()
                //                                              {
                //                                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                //                                                  TechnicianId = (Guid)dr["TechnicianId"],
                //                                                  EquipmentId = (Guid)dr["EquipmentId"],
                //                                                  IsReceived = dr["IsReceived"] != DBNull.Value ? Convert.ToBoolean(dr["IsReceived"]) : false,
                //                                                  ReceivedBy = (Guid)dr["ReceivedBy"],
                //                                                  ReceivedDate = dr["ReceivedDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReceivedDate"]) : new DateTime(),
                //                                                  CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                //                                                  Name = dr["Name"].ToString(),
                //                                                  SKU = dr["SKU"].ToString(),
                //                                                  Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                //                                                  ReceivedByName = dr["ReceivedByName"].ToString(),
                //                                                  TotalQuantity = dr["TotalQuantity"] != DBNull.Value ? Convert.ToInt32(dr["TotalQuantity"]) : 0,
                //                                                  IsApprove = dr["IsApprove"] != DBNull.Value ? Convert.ToBoolean(dr["IsApprove"]) : false,
                //                                                  Type = dr["Type"].ToString(),
                //                                                  IsDecline = dr["IsDecline"] != DBNull.Value ? Convert.ToBoolean(dr["IsDecline"]) : false,

                //                                              }).ToList();
                //    Model.ListAssignedInventoryTechReceived = (from DataRow dr in ds.Tables[3].Rows
                //                                               select new AssignedInventoryTechReceived()
                //                                               {
                //                                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                //                                                   TechnicianId = (Guid)dr["TechnicianId"],
                //                                                   EquipmentId = (Guid)dr["EquipmentId"],
                //                                                   IsReceived = dr["IsReceived"] != DBNull.Value ? Convert.ToBoolean(dr["IsReceived"]) : false,
                //                                                   ReceivedBy = (Guid)dr["ReceivedBy"],
                //                                                   ReceivedDate = dr["ReceivedDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReceivedDate"]) : new DateTime(),
                //                                                   CreatedDate = dr["ReceivedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                //                                                   Name = dr["Name"].ToString(),
                //                                                   SKU = dr["SKU"].ToString(),
                //                                                   Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                //                                                   ReceivedByName = dr["SentByName"].ToString(),
                //                                                   TotalQuantity = dr["TotalQuantity"] != DBNull.Value ? Convert.ToInt32(dr["TotalQuantity"]) : 0,
                //                                                   IsApprove = dr["IsApprove"] != DBNull.Value ? Convert.ToBoolean(dr["IsApprove"]) : false,
                //                                                   Type = dr["Type"].ToString(),
                //                                                   IsDecline = dr["IsDecline"] != DBNull.Value ? Convert.ToBoolean(dr["IsDecline"]) : false,
                //                                               }).ToList();

                //    Model.TotalCountTrf = ds.Tables[1].Rows[0][0] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0][0]) : 0;
                //    Model.TotalCountRcv = ds.Tables[4].Rows[0][0] != DBNull.Value ? Convert.ToInt32(ds.Tables[3].Rows[0][0]) : 0;

                //    Model.TechTrfList = ds.Tables[2].ToList<GenericDropDownFilter>();
                //    Model.TechRcvList = ds.Tables[5].ToList<GenericDropDownFilter>();
                //}



                //Model.Searchtext = filter.Searchtext;
                //Model.PageNo = filter.PageNoTrf;
                //Model.PageNoRcv = filter.PageNoRcv;
                //Model.PageSize = filter.PageSize;
            }
            catch (Exception ex)
            {
                new ErrorFacade(_ClientContext).InsertErrorLog(new ErrorLog() { ErrorId = new Guid(), ErrorFor = "InventoryFacade-GetTechTransferListByFilters", Message = ex.Message, TimeUtc = DateTime.Now });
            }
            return ds;
        }

        public TransferRequestVM GetTechTransferListByFilters_old(DetailedHistoryFilter filters)
        {
            //DataSet ds = _PurchaseOrderDataAccess.GetPurchaseOrderListByFilters(filters);
            DataSet ds = _InventoryDataAccess.GetDetailedHistoryListByFilters(filters);
            //DataTable dt = _InventoryDataAccess.GetDetailedHistoryListByFilters(filters);
            TransferRequestVM Model = new TransferRequestVM();
            Model.Items = new List<TransferRequest>();

            if (ds.Tables[0] != null)
                Model.Items = (from DataRow dr in ds.Tables[0].Rows
                                  select new TransferRequest()
                                  {
                                      TransferRequestId = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                      EquipmentName = dr["Description"].ToString(),
                                      SKU = dr["SKU"].ToString(),
                                      TransferFrom = dr["TransferFrom"].ToString(),
                                      Qty = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                      OnHand = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                      TransferDate = dr["TransferDate"] != DBNull.Value ? Convert.ToDateTime(dr["TransferDate"]) : DateTime.Now,
                                     
                                  }).ToList();
            Model.ItemsCount = ds.Tables[1].Rows[0][0] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0][0]) : 0;
            //Model.TotalCount = ds != null && ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;
            //Model.Searchtext = filters.Searchtext;
            //Model.PageNo = filters.PageNo;
            //Model.PageSize = filters.PageSize;
            return Model;
        }

        public TechReceiveListModel GetTechTransferListByFilters(TechTransferFilter filter)
        {
            TechReceiveListModel Model = new TechReceiveListModel();
            try
            {
                DataSet ds = _InventoryDataAccess.GetTechTransferListByFilters(filter);
                if (ds != null)
                {
                    Model.ListAssignedInventoryTechApprove = (from DataRow dr in ds.Tables[0].Rows
                                                              select new AssignedInventoryTechReceived()
                                                              {
                                                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                                  TechnicianId = (Guid)dr["TechnicianId"],
                                                                  EquipmentId = (Guid)dr["EquipmentId"],
                                                                  IsReceived = dr["IsReceived"] != DBNull.Value ? Convert.ToBoolean(dr["IsReceived"]) : false,
                                                                  ReceivedBy = (Guid)dr["ReceivedBy"],
                                                                  ReceivedDate = dr["ReceivedDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReceivedDate"]) : new DateTime(),
                                                                  CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                                                  Name = dr["Name"].ToString(),
                                                                  SKU = dr["SKU"].ToString(),
                                                                  Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                                                  ReceivedByName = dr["ReceivedByName"].ToString(),
                                                                  TotalQuantity = (dr["IsLocation"] != DBNull.Value && Convert.ToBoolean(dr["IsLocation"])) ? Convert.ToInt32(dr["WHStock"]) : Convert.ToInt32(dr["TotalQuantity"]),
                                                                  IsApprove = dr["IsApprove"] != DBNull.Value ? Convert.ToBoolean(dr["IsApprove"]) : false,
                                                                  Type = dr["Type"].ToString(),
                                                                  IsDecline = dr["IsDecline"] != DBNull.Value ? Convert.ToBoolean(dr["IsDecline"]) : false,

                                                              }).ToList();
                    Model.ListAssignedInventoryTechReceived = (from DataRow dr in ds.Tables[3].Rows
                                                               select new AssignedInventoryTechReceived()
                                                               {
                                                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                                   TechnicianId = (Guid)dr["TechnicianId"],
                                                                   EquipmentId = (Guid)dr["EquipmentId"],
                                                                   IsReceived = dr["IsReceived"] != DBNull.Value ? Convert.ToBoolean(dr["IsReceived"]) : false,
                                                                   ReceivedBy = (Guid)dr["ReceivedBy"],
                                                                   ReceivedDate = dr["ReceivedDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReceivedDate"]) : new DateTime(),
                                                                   CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                                                   Name = dr["Name"].ToString(),
                                                                   SKU = dr["SKU"].ToString(),
                                                                   Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                                                   ReceivedByName = dr["SentByName"].ToString(),
                                                                   TotalQuantity = (dr["IsLocation"] != DBNull.Value && Convert.ToBoolean(dr["IsLocation"])) ? Convert.ToInt32(dr["WHStock"]) : Convert.ToInt32(dr["TotalQuantity"]),
                                                                   IsApprove = dr["IsApprove"] != DBNull.Value ? Convert.ToBoolean(dr["IsApprove"]) : false,
                                                                   Type = dr["Type"].ToString(),
                                                                   IsDecline = dr["IsDecline"] != DBNull.Value ? Convert.ToBoolean(dr["IsDecline"]) : false,
                                                               }).ToList();

                    Model.TotalCountTrf = ds.Tables[1].Rows[0][0] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0][0]) : 0;
                    Model.TotalCountRcv = ds.Tables[4].Rows[0][0] != DBNull.Value ? Convert.ToInt32(ds.Tables[4].Rows[0][0]) : 0;


                    Model.TechTrfFromList = ds.Tables[2].ToList<GenericDropDownFilter>();
                    Model.TechRcvFromList = ds.Tables[5].ToList<GenericDropDownFilter>();
                }

                

                Model.Searchtext = filter.Searchtext;
                Model.PageNo = filter.PageNoTrf;
                Model.PageNoRcv = filter.PageNoRcv;
                Model.PageSize = filter.PageSize;
            }
            catch (Exception ex)
            {
                new ErrorFacade(_ClientContext).InsertErrorLog(new ErrorLog() { ErrorId = new Guid(), ErrorFor = "InventoryFacade-GetTechTransferListByFilters", Message = ex.Message, TimeUtc = DateTime.Now });
            }
            return Model;
        }
    
        public TechReceiveListModel GetTechTransferLogListByFilters(TechTransferFilter filter)
        {
            TechReceiveListModel Model = new TechReceiveListModel();
            try
            {
                DataSet ds = _InventoryDataAccess.GetTechTransferLogListByFilters(filter);
                if (ds != null)
                {
                    Model.ListAssignedInventoryTechApprove = (from DataRow dr in ds.Tables[0].Rows
                                                              select new AssignedInventoryTechReceived()
                                                              {
                                                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                                  TechnicianId = dr["TechnicianId"] != DBNull.Value ? (Guid)dr["TechnicianId"] : Guid.Empty,
                                                                  EquipmentId = dr["EquipmentId"] != DBNull.Value ? (Guid)dr["EquipmentId"] : Guid.Empty,
                                                                  IsReceived = dr["IsReceived"] != DBNull.Value ? Convert.ToBoolean(dr["IsReceived"]) : false,
                                                                  ReceivedBy = dr["ReceivedBy"] != DBNull.Value ? (Guid)dr["ReceivedBy"] : Guid.Empty,
                                                                  ReceivedDate = dr["ReceivedDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReceivedDate"]) : DateTime.MinValue,
                                                                  CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.MinValue,
                                                                  Name = dr["Name"].ToString(),
                                                                  SKU = dr["SKU"].ToString(),
                                                                  Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                                                  TransferByName = dr["SentByName"] != DBNull.Value ? dr["SentByName"].ToString() : "Purchase Order",
                                                                  ReceivedByName = dr["ReceivedByName"].ToString(),
                                                                  TotalQuantity = dr["WHStock"] != DBNull.Value ? Convert.ToInt32(dr["WHStock"]) : 0,
                                                                  IsApprove = dr["IsApprove"] != DBNull.Value ? Convert.ToBoolean(dr["IsApprove"]) : false,
                                                                  Type = dr["Type"].ToString(),
                                                                  IsDecline = dr["IsDecline"] != DBNull.Value ? Convert.ToBoolean(dr["IsDecline"]) : false,
                                                                  created_BY = dr["CreatedBy"] != DBNull.Value ? Convert.ToString(dr["CreatedBy"]) : null,
                                                                  Closed_By = dr["ClosedBy"] != DBNull.Value ? Convert.ToString(dr["ClosedBy"]) : null,
                                                              }).ToList();

                    Model.ListAssignedInventoryTechReceived = (from DataRow dr in ds.Tables[4].Rows
                                                               select new AssignedInventoryTechReceived()
                                                               {
                                                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                                   TechnicianId = (Guid)dr["TechnicianId"],
                                                                   EquipmentId = (Guid)dr["EquipmentId"],
                                                                   IsReceived = dr["IsReceived"] != DBNull.Value ? Convert.ToBoolean(dr["IsReceived"]) : false,
                                                                   ReceivedBy = (Guid)dr["ReceivedBy"],
                                                                   ReceivedDate = dr["ReceivedDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReceivedDate"]) : DateTime.MinValue,
                                                                   CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.MinValue,
                                                                   Name = dr["Name"].ToString(),
                                                                   SKU = dr["SKU"].ToString(),
                                                                   Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                                                   TransferByName = dr["SentByName"] != DBNull.Value ? dr["SentByName"].ToString() : "Purchase Order",
                                                                   ReceivedByName = dr["ReceivedByName"].ToString(),
                                                                   TotalQuantity = dr["WHStock"] != DBNull.Value ? Convert.ToInt32(dr["WHStock"]) : 0 ,
                                                                   IsApprove = dr["IsApprove"] != DBNull.Value ? Convert.ToBoolean(dr["IsApprove"]) : false,
                                                                   Type = dr["Type"].ToString(),
                                                                   IsDecline = dr["IsDecline"] != DBNull.Value ? Convert.ToBoolean(dr["IsDecline"]) : false,
                                                                   ReqSrc = dr["ReqSrc"] != DBNull.Value ? Convert.ToString(dr["ReqSrc"]) : null,
                                                                   created_BY = dr["CreatedBy"] != DBNull.Value ? Convert.ToString(dr["CreatedBy"]) : null,
                                                                   Closed_By = dr["ClosedBy"] != DBNull.Value ? Convert.ToString(dr["ClosedBy"]) : null,

                                                               }).ToList();

                    Model.TotalCountTrf = ds.Tables[1].Rows[0][0] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0][0]) : 0;
                    Model.TotalCountRcv = ds.Tables[5].Rows[0][0] != DBNull.Value ? Convert.ToInt32(ds.Tables[5].Rows[0][0]) : 0;

                    Model.TechTrfFromList = ds.Tables[2].ToList<GenericDropDownFilter>();
                    Model.TechTrfToList = ds.Tables[3].ToList<GenericDropDownFilter>();
                    Model.TechRcvFromList = ds.Tables[6].ToList<GenericDropDownFilter>();
                    Model.TechRcvToList = ds.Tables[7].ToList<GenericDropDownFilter>();
                }



                Model.Searchtext = filter.Searchtext;
                Model.PageNo = filter.PageNoTrf;
                Model.PageNoRcv = filter.PageNoRcv;
                Model.PageSize = filter.PageSize;
            }
            catch (Exception ex)
            {
                new ErrorFacade(_ClientContext).InsertErrorLog(new ErrorLog() { ErrorId = new Guid(), ErrorFor = "InventoryFacade-GetTechTransferListByFilters", Message = ex.Message, TimeUtc = DateTime.Now });
            }
            return Model;
        }
        public TechReceiveListModel GetTechTransferLogListByFiltersDate(TechTransferFilter filter, DateTime? start, DateTime? end)
        {
            TechReceiveListModel Model = new TechReceiveListModel();
            try
            {
                DataSet ds = _InventoryDataAccess.GetTechTransferLogListByFiltersDate(filter, start, end);
                if (ds != null)
                {
                    Model.ListAssignedInventoryTechApprove = (from DataRow dr in ds.Tables[0].Rows
                                                              select new AssignedInventoryTechReceived()
                                                              {
                                                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                                  TechnicianId = dr["TechnicianId"] != DBNull.Value ? (Guid)dr["TechnicianId"] : Guid.Empty,
                                                                  EquipmentId = dr["EquipmentId"] != DBNull.Value ? (Guid)dr["EquipmentId"] : Guid.Empty,
                                                                  IsReceived = dr["IsReceived"] != DBNull.Value ? Convert.ToBoolean(dr["IsReceived"]) : false,
                                                                  ReceivedBy = dr["ReceivedBy"] != DBNull.Value ? (Guid)dr["ReceivedBy"] : Guid.Empty,
                                                                  ReceivedDate = dr["ReceivedDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReceivedDate"]) : DateTime.MinValue,
                                                                  CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.MinValue,
                                                                  Name = dr["Name"].ToString(),
                                                                  SKU = dr["SKU"].ToString(),
                                                                  Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                                                  TransferByName = dr["SentByName"] != DBNull.Value ? dr["SentByName"].ToString() : "Purchase Order",
                                                                  ReceivedByName = dr["ReceivedByName"].ToString(),
                                                                  TotalQuantity = dr["WHStock"] != DBNull.Value ? Convert.ToInt32(dr["WHStock"]) : 0,
                                                                  IsApprove = dr["IsApprove"] != DBNull.Value ? Convert.ToBoolean(dr["IsApprove"]) : false,
                                                                  Type = dr["Type"].ToString(),
                                                                  IsDecline = dr["IsDecline"] != DBNull.Value ? Convert.ToBoolean(dr["IsDecline"]) : false,
                                                                  created_BY = dr["CreatedBy"] != DBNull.Value ? Convert.ToString(dr["CreatedBy"]) : null,
                                                                  Closed_By = dr["ClosedBy"] != DBNull.Value ? Convert.ToString(dr["ClosedBy"]) : null,
                                                              }).ToList();

                    Model.ListAssignedInventoryTechReceived = (from DataRow dr in ds.Tables[4].Rows
                                                               select new AssignedInventoryTechReceived()
                                                               {
                                                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                                   TechnicianId = (Guid)dr["TechnicianId"],
                                                                   EquipmentId = (Guid)dr["EquipmentId"],
                                                                   IsReceived = dr["IsReceived"] != DBNull.Value ? Convert.ToBoolean(dr["IsReceived"]) : false,
                                                                   ReceivedBy = (Guid)dr["ReceivedBy"],
                                                                   ReceivedDate = dr["ReceivedDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReceivedDate"]) : DateTime.MinValue,
                                                                   CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.MinValue,
                                                                   Name = dr["Name"].ToString(),
                                                                   SKU = dr["SKU"].ToString(),
                                                                   Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                                                   TransferByName = dr["SentByName"] != DBNull.Value ? dr["SentByName"].ToString() : "Purchase Order",
                                                                   ReceivedByName = dr["ReceivedByName"].ToString(),
                                                                   TotalQuantity = dr["WHStock"] != DBNull.Value ? Convert.ToInt32(dr["WHStock"]) : 0,
                                                                   IsApprove = dr["IsApprove"] != DBNull.Value ? Convert.ToBoolean(dr["IsApprove"]) : false,
                                                                   Type = dr["Type"].ToString(),
                                                                   IsDecline = dr["IsDecline"] != DBNull.Value ? Convert.ToBoolean(dr["IsDecline"]) : false,
                                                                   ReqSrc = dr["ReqSrc"] != DBNull.Value ? Convert.ToString(dr["ReqSrc"]) : null,
                                                                   created_BY = dr["CreatedBy"] != DBNull.Value ? Convert.ToString(dr["CreatedBy"]) : null,
                                                                   Closed_By = dr["ClosedBy"] != DBNull.Value ? Convert.ToString(dr["ClosedBy"]) : null,

                                                               }).ToList();

                    Model.TotalCountTrf = ds.Tables[1].Rows[0][0] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0][0]) : 0;
                    Model.TotalCountRcv = ds.Tables[5].Rows[0][0] != DBNull.Value ? Convert.ToInt32(ds.Tables[5].Rows[0][0]) : 0;

                    Model.TechTrfFromList = ds.Tables[2].ToList<GenericDropDownFilter>();
                    Model.TechTrfToList = ds.Tables[3].ToList<GenericDropDownFilter>();
                    Model.TechRcvFromList = ds.Tables[6].ToList<GenericDropDownFilter>();
                    Model.TechRcvToList = ds.Tables[7].ToList<GenericDropDownFilter>();
                }



                Model.Searchtext = filter.Searchtext;
                Model.PageNo = filter.PageNoTrf;
                Model.PageNoRcv = filter.PageNoRcv;
                Model.PageSize = filter.PageSize;
            }
            catch (Exception ex)
            {
                new ErrorFacade(_ClientContext).InsertErrorLog(new ErrorLog() { ErrorId = new Guid(), ErrorFor = "InventoryFacade-GetTechTransferListByFilters", Message = ex.Message, TimeUtc = DateTime.Now });
            }
            return Model;
        }
        private T GetValueOrDefault<T>(DataRow dr, string columnName)
        {
            return dr[columnName] != DBNull.Value ? (T)dr[columnName] : default(T);
        }

        public DataSet GetTechTransferLogListByFiltersR(TechTransferFilter filter)
        {
            TechReceiveListModel Model = new TechReceiveListModel();
            DataSet ds = new DataSet();
            try
            {
                filter.PageSize = 20000;
                filter.PageNoTrf = 1;
                filter.PageNoRcv = 1;
                ds = _InventoryDataAccess.GetTechTransferLogListByFilters(filter);
                if (ds!=null)
                {
                    ds.Tables[0].Columns.RemoveAt(0);
                    ds.Tables[0].Columns.RemoveAt(0);
                    ds.Tables[0].Columns.RemoveAt(0);
                    ds.Tables[0].Columns.RemoveAt(0);
                    ds.Tables[0].Columns.RemoveAt(0);
                    ds.Tables[0].Columns[0].ColumnName = "Discription";
                    ds.Tables[0].Columns[1].ColumnName = "SKU";
                    ds.Tables[0].Columns[2].ColumnName = "Transfer From";
                    ds.Tables[0].Columns[3].ColumnName = " QTY ";
                    ds.Tables[0].Columns[4].ColumnName = "Transfer To";
                    ds.Tables[0].Columns.RemoveAt(5);
                    ds.Tables[0].Columns[5].ColumnName = "On Hand";
                    ds.Tables[0].Columns.RemoveAt(6);
                    //ds.Tables[0].Columns.RemoveAt(6);

                    ds.Tables[0].Columns[6].ColumnName = "Transfer Date";
                    ds.Tables[0].Columns.RemoveAt(7);
                    ds.Tables[0].Columns.RemoveAt(7);
                    ds.Tables[0].Columns.RemoveAt(7);
                    //ds.Tables[0].Columns.RemoveAt(7);

                    ds.Tables[0].Columns[7].ColumnName = "Request By";
                    ds.Tables[0].Columns[8].ColumnName = "Accepted By";

                    //ds.Tables[0].Columns.RemoveAt(9);
                    //ds.Tables[0].Columns.RemoveAt(9);
                    //ds.Tables[0].Columns.RemoveAt(9);
                    //ds.Tables[0].Columns.RemoveAt(9);


                    ds.Tables[0].Columns.Add("Status", typeof(string));

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        bool isApprove = row.Field<bool>("IsApprove");
                        bool isDecline = row.Field<bool>("IsDecline");

                        if (isDecline)
                        {
                            row["Status"] = "Declined";
                        }
                        else if (!isApprove && !isDecline)
                        {
                            row["Status"] = "In Process";
                        }
                        else
                        {
                            row["Status"] = "Accepted";
                        }
                    }






                }
            }
            catch (Exception ex)
            {
                new ErrorFacade(_ClientContext).InsertErrorLog(new ErrorLog() { ErrorId = new Guid(), ErrorFor = "InventoryFacade-GetTechTransferListByFilters", Message = ex.Message, TimeUtc = DateTime.Now });
            }
            return ds;
        }
        public DataSet GetTechTransferLogListByFiltersRDate(TechTransferFilter filter, DateTime? start, DateTime? end)
        {
            TechReceiveListModel Model = new TechReceiveListModel();
            DataSet ds = new DataSet();
            try
            {
                filter.PageSize = 20000;
                filter.PageNoTrf = 1;
                filter.PageNoRcv = 1;
                ds = _InventoryDataAccess.GetTechTransferLogListByFiltersDate(filter, start, end);
                if (ds != null)
                {
                    ds.Tables[0].Columns.RemoveAt(0);
                    ds.Tables[0].Columns.RemoveAt(0);
                    ds.Tables[0].Columns.RemoveAt(0);
                    ds.Tables[0].Columns.RemoveAt(0);
                    //ds.Tables[0].Columns.RemoveAt(0);
                    ds.Tables[0].Columns[0].ColumnName = "Discription";
                    ds.Tables[0].Columns[1].ColumnName = "SKU";
                    ds.Tables[0].Columns[2].ColumnName = "Transfer From";
                    ds.Tables[0].Columns[3].ColumnName = " QTY ";
                    ds.Tables[0].Columns[4].ColumnName = "Transfer To";
                    ds.Tables[0].Columns.RemoveAt(5);
                    ds.Tables[0].Columns[5].ColumnName = "On Hand";
                    ds.Tables[0].Columns.RemoveAt(6);
                    //ds.Tables[0].Columns.RemoveAt(6);

                    ds.Tables[0].Columns[6].ColumnName = "Transfer Date";
                    ds.Tables[0].Columns[7].ColumnName = "Accepted Date";
                    //ds.Tables[0].Columns.RemoveAt(7);

                    ds.Tables[0].Columns.RemoveAt(8);
                    ds.Tables[0].Columns.RemoveAt(8);
                    ds.Tables[0].Columns.RemoveAt(8);

                    ds.Tables[0].Columns[8].ColumnName = "Request By";
                    ds.Tables[0].Columns[9].ColumnName = "Accepted By";

                    //ds.Tables[0].Columns.RemoveAt(9);
                    //ds.Tables[0].Columns.RemoveAt(9);
                    //ds.Tables[0].Columns.RemoveAt(9);
                    //ds.Tables[0].Columns.RemoveAt(9);


                    ds.Tables[0].Columns.Add("Status", typeof(string));
                    ds.Tables[0].Columns.Add("Transfer Time", typeof(string));
                    ds.Tables[0].Columns.Add("Accepted Time", typeof(string));
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        bool isApprove = row.Field<bool>("IsApprove");
                        bool isDecline = row.Field<bool>("IsDecline");

                        if (isDecline)
                        {
                            row["Status"] = "Declined";
                        }
                        else if (!isApprove && !isDecline)
                        {
                            row["Status"] = "In Process";
                        }
                        else
                        {
                            row["Status"] = "Accepted";
                        }
                        if (row["Transfer Date"] != DBNull.Value)
                        {
                            DateTime transferDate = row.Field<DateTime>("Transfer Date");
                            row["Transfer Date"] = transferDate.ToString("M/d/yyyy");
                            row["Transfer Time"] = transferDate.ToString("h:mm tt");
                        }

                        if (row["Accepted Date"] != DBNull.Value)
                        {
                            DateTime acceptedDate = row.Field<DateTime>("Accepted Date");
                            row["Accepted Date"] = acceptedDate.ToString("M/d/yyyy");
                            row["Accepted Time"] = acceptedDate.ToString("h:mm tt");
                        }

                    }
                    ds.Tables[0].Columns["Transfer Time"].SetOrdinal(ds.Tables[0].Columns["Transfer Date"].Ordinal + 1);
                    ds.Tables[0].Columns["Accepted Time"].SetOrdinal(ds.Tables[0].Columns["Accepted Date"].Ordinal + 1);





                }
            }
            catch (Exception ex)
            {
                new ErrorFacade(_ClientContext).InsertErrorLog(new ErrorLog() { ErrorId = new Guid(), ErrorFor = "InventoryFacade-GetTechTransferListByFilters", Message = ex.Message, TimeUtc = DateTime.Now });
            }
            return ds;
        }
        public long InsertTechTransfer(TechTransferRequest tr, Guid CreatedBy)
        {
            long result = 0;
            foreach (var item in tr.Items)
            {
                item.CreatedBy=CreatedBy;
                item.CreatedDate= DateTime.Now;

                logger.WithProperty("tags", $"Trasfer Equipment created datetime {DateTime.Now}")
                           .WithProperty("params", JsonConvert.SerializeObject(tr))
                           .Trace($"Equipment History by {CreatedBy}.");

                //item.CreatedDate= DateTime.Now.UTCCurrentTime();

                result =_AssignedInventoryTechReceivedDataAccess.Insert(item);
            }

            return result;
            //return _AssignedInventoryTechReceivedDataAccess.InsertTechTransfer(tr);
        }
        public long InsertTechTransferwithoutapprove(TechTransferRequest tr, Guid CreatedBy)
        {
            long result = 0;
            foreach (var item in tr.Items)
            {
                item.CreatedBy = CreatedBy;
                item.ClosedBy = CreatedBy;
                item.ReceivedDate = DateTime.Now;
                item.CreatedDate = DateTime.Now;

                logger.WithProperty("tags", $"Trasfer Equipment created datetime {DateTime.Now}")
                           .WithProperty("receivedDate", $"ReceivedDate: {item.ReceivedDate}")
                           .WithProperty("closedBy", $"ClosedBy: {item.ClosedBy}")
                           .WithProperty("params", JsonConvert.SerializeObject(tr))
                           .Trace($"Equipment History by {CreatedBy}.");

                //item.CreatedDate= DateTime.Now.UTCCurrentTime();

                result = _AssignedInventoryTechReceivedDataAccess.Insert(item);
                var re = _AssignedInventoryTechReceivedDataAccess.Update_DG(item);
            }

            return result;
            //return _AssignedInventoryTechReceivedDataAccess.InsertTechTransfer(tr);
        }
        public AssignedInventoryTechReceived GetAllAssignedInventory(AssignedInventoryTechReceived objassigntech)
        {
            DataSet ds = _InventoryDataAccess.GetAllAssignedInventory(objassigntech);

            if (ds.Tables[0] != null)
            {
                objassigntech.Id = ds.Tables[0].Rows[0]["Id"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["Id"]) : 0;
                objassigntech.TechnicianId = ds.Tables[0].Rows[0]["TechnicianId"] != DBNull.Value ? new Guid(ds.Tables[0].Rows[0]["TechnicianId"].ToString()) : new Guid();
                objassigntech.EquipmentId = ds.Tables[0].Rows[0]["EquipmentId"] != DBNull.Value ? new Guid(ds.Tables[0].Rows[0]["EquipmentId"].ToString()) : new Guid();
                objassigntech.ReceivedBy = ds.Tables[0].Rows[0]["ReceivedBy"] != DBNull.Value ? new Guid(ds.Tables[0].Rows[0]["ReceivedBy"].ToString()) : new Guid();
                objassigntech.Quantity = ds.Tables[0].Rows[0]["Quantity"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["Quantity"]) : 0;
                objassigntech.IsApprove = ds.Tables[0].Rows[0]["IsApprove"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["IsApprove"]) : false;
                objassigntech.IsDecline = ds.Tables[0].Rows[0]["IsDecline"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["IsDecline"]) : false;
                objassigntech.IsReceived = ds.Tables[0].Rows[0]["IsReceived"] != DBNull.Value ? Convert.ToBoolean(ds.Tables[0].Rows[0]["IsReceived"]) : false;
                objassigntech.ReqSrc = ds.Tables[0].Rows[0]["ReqSrc"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[0]["ReqSrc"]) : null;
            }
            return objassigntech;
        }

        public bool UpdateTechReceive_DG(AssignedInventoryTechReceived tr)
        {
            return _AssignedInventoryTechReceivedDataAccess.Update_DG(tr) > 0;
        }

        public List<EquipmentSearchModel> GetEqupmentListBySearchKeyAndCompanyIdTechnicianId(string key, Guid CompanyId, int MaxLoad, Guid technicianId, string ExistEquipment)
        {
            DataTable dt = _InventoryDataAccess.GetEqupmentListBySearchKeyAndCompanyIdTechnicianId(key, CompanyId, MaxLoad, technicianId, ExistEquipment);
            List<EquipmentSearchModel> NoteList = new List<EquipmentSearchModel>();
            NoteList = (from DataRow dr in dt.Rows
                        select new EquipmentSearchModel()
                        {
                            EquipmentId = (Guid)dr["EquipmentId"],
                            EquipmentName = dr["EquipmentName"].ToString(),
                            QuantityAvailable = dr["QuantityAvailable"] != DBNull.Value ? Convert.ToInt32(dr["QuantityAvailable"]) : 0,
                            Reorderpoint = dr["Reorderpoint"] != DBNull.Value ? Convert.ToInt32(dr["Reorderpoint"]) : 0,
                            RetailPrice = dr["RetailPrice"] != DBNull.Value ? Convert.ToDouble(dr["RetailPrice"]) : 0.0,
                            SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0.0,
                            EquipmentType = dr["EquipmentType"].ToString(),
                            EquipmentDescription = dr["EquipmentDescription"].ToString(),
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            //QuantityOnHand = dr["QuantityOnHand"] != DBNull.Value ? Convert.ToInt32(dr["QuantityOnHand"]) : 0,
                            QuantityOnHand = dr["WarehouseQTY"] != DBNull.Value ? Convert.ToInt32(dr["WarehouseQTY"]) : 0,
                            WareHouseQuantity = dr["WarehouseQTY"] != DBNull.Value ? Convert.ToInt32(dr["WarehouseQTY"]) : 0,
                            ManufacturerName = dr["ManufacturerName"].ToString(),
                            SKU = dr["SKU"].ToString(),
                            Barcode = dr["Barcode"].ToString(),
                            Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0.0,
                          
                            Equipmentvendorcost = dr["EquipmentVendorCost"] != DBNull.Value ? Convert.ToDouble(dr["EquipmentVendorCost"]) : 0.0
                        }).ToList();
            return NoteList;
        }


        public DetailedHistoryVM GetWHHistoryListByFilters(DetailedHistoryFilter filters)
        {
            DataSet ds = _InventoryDataAccess.GetWHHistoryIWHStockListByFilters(filters);
            DetailedHistoryVM Model = new DetailedHistoryVM();
            Model.Items = new List<DetailedHistoryItem>();

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                Model.Items = (from DataRow dr in ds.Tables[0].Rows
                               select new DetailedHistoryItem()
                               {
                                   EquipmentId = dr["EquipmentId"].ToString(),
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   EquipmentName = dr["Description"].ToString(),
                                   SKU = dr["SKU"].ToString(),
                                   MfgId = dr["MfgId"] != DBNull.Value ? Convert.ToInt32(dr["MfgId"]) : 0,
                                   Manufacturer = dr["Manufacturer"].ToString(),
                                   Start = dr["OpnBal"] != DBNull.Value ? Convert.ToInt32(dr["OpnBal"]) : 0,
                                   AddToTicket = dr["Added"] != DBNull.Value ? Convert.ToInt32(dr["Added"]) : 0,
                                   PulledFromTicket = dr["Pulled"] != DBNull.Value ? Convert.ToInt32(dr["Pulled"]) : 0,
                                   WH_In = dr["WH_In"] != DBNull.Value ? Convert.ToInt32(dr["WH_In"]) : 0,
                                   WH_Out = dr["WH_Out"] != DBNull.Value ? Convert.ToInt32(dr["WH_Out"]) : 0,
                                   OnHand = dr["OnHand"] != DBNull.Value ? Convert.ToInt32(dr["OnHand"]) : 0,
                               }).ToList();
            Model.ItemsCount = ds.Tables[1].Rows.Count > 0 ? Convert.ToInt32(ds.Tables[1].Rows[0][0]) : 0;

            Model.EquipmentsList = ds.Tables[2].ToList<GenericDropDownFilter>();
            Model.ManufacturersList = ds.Tables[3].ToList<GenericDropDownFilter>();

            //Model.TotalCount = ds != null && ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;
            //Model.Searchtext = filters.Searchtext;
            //Model.PageNo = filters.PageNo;
            //Model.PageSize = filters.PageSize;
            return Model;
        }


        public DataSet GetWHHistoryListByFiltersR(DetailedHistoryFilter filters)
        {
            DataSet ds = _InventoryDataAccess.GetWHHistoryIWHStockListByFilters(filters);
            //ds.Tables[0].Columns.Remove("columnName");


            ds.Tables[0].Columns.RemoveAt(0);
            ds.Tables[0].Columns.RemoveAt(0);
            ds.Tables[0].Columns[0].ColumnName = "Description";
            ds.Tables[0].Columns.RemoveAt(2);
            ds.Tables[0].Columns[3].ColumnName = "Start";
            ds.Tables[0].Columns[4].ColumnName = "Purchase";
            ds.Tables[0].Columns[5].ColumnName = "Returns";
            ds.Tables[0].Columns[6].ColumnName = "Transfer In";
            ds.Tables[0].Columns[7].ColumnName = "Transfer Out";
            ds.Tables[0].Columns.RemoveAt(9);
            ds.Tables[0].Columns.RemoveAt(9);

            //DetailedHistoryVM Model = new DetailedHistoryVM();
            //Model.Items = new List<DetailedHistoryItem>();

            //if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            //    Model.Items = (from DataRow dr in ds.Tables[0].Rows
            //                   select new DetailedHistoryItem()
            //                   {
            //                       EquipmentId = dr["EquipmentId"].ToString(),
            //                       Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
            //                       EquipmentName = dr["Description"].ToString(),
            //                       SKU = dr["SKU"].ToString(),
            //                       MfgId = dr["MfgId"] != DBNull.Value ? Convert.ToInt32(dr["MfgId"]) : 0,
            //                       Manufacturer = dr["Manufacturer"].ToString(),
            //                       Start = dr["OpnBal"] != DBNull.Value ? Convert.ToInt32(dr["OpnBal"]) : 0,
            //                       AddToTicket = dr["Added"] != DBNull.Value ? Convert.ToInt32(dr["Added"]) : 0,
            //                       PulledFromTicket = dr["Pulled"] != DBNull.Value ? Convert.ToInt32(dr["Pulled"]) : 0,
            //                       WH_In = dr["WH_In"] != DBNull.Value ? Convert.ToInt32(dr["WH_In"]) : 0,
            //                       WH_Out = dr["WH_Out"] != DBNull.Value ? Convert.ToInt32(dr["WH_Out"]) : 0,
            //                       OnHand = dr["OnHand"] != DBNull.Value ? Convert.ToInt32(dr["OnHand"]) : 0,
            //                   }).ToList();
            //Model.ItemsCount = ds.Tables[1].Rows.Count > 0 ? Convert.ToInt32(ds.Tables[1].Rows[0][0]) : 0;

            //Model.EquipmentsList = ds.Tables[2].ToList<GenericDropDownFilter>();
            //Model.ManufacturersList = ds.Tables[3].ToList<GenericDropDownFilter>();

                       
            
            
            return ds;

        }
        public DetailedEquipmentVM GetWHHistoryDetailedListByFilters(DetailedHistoryFilter filters)
        {
            DataSet ds = _InventoryDataAccess.GetWHHistoryPOListByFilters(filters);
            DataTable ticketsDT = ds.Tables[0];

            DetailedEquipmentVM Model = new DetailedEquipmentVM();
            Model.Tickets = new List<DetailedEquipmentTicketListItem>();
            Model.TicketsCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
            if (ticketsDT != null)
                Model.Tickets = (from DataRow dr in ticketsDT.Rows
                                 select new DetailedEquipmentTicketListItem()
                                 {
                                     
                                     Id = dr["POId"] != DBNull.Value ? Convert.ToInt32(dr["POId"]) : 0,
                                     EquipmentId = dr["PONo"].ToString(),
                                     EquipmentName = dr["Name"].ToString(),
                                     SKU = dr["SKU"].ToString(),
                                     Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                     Date = dr["Date"].ToString()
                                 }).ToList();

            //ds = _InventoryDataAccess.GetDetailedEquipmentTransfersByFilters(filters);
            ds = _InventoryDataAccess.GetWHHistoryTransfersListByFilters(filters);
            DataTable ticketsTR = ds.Tables[0];
            Model.Transfers = new List<DetailedEquipmentTransferListItem>();
            //Model.TransfersCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
            Model.TransfersCount = Model.Transfers.Count;
            if (ticketsTR != null)
                Model.Transfers = (from DataRow dr in ticketsTR.Rows
                                   select new DetailedEquipmentTransferListItem()
                                   {
                                       TechName = dr["TTech"].ToString() + " ➡️ " + dr["RTech"].ToString(),
                                       //EquipmentId = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                       EquipmentName = dr["Equipment"].ToString(),
                                       SKU = dr["SKU"].ToString(),
                                       Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                       Date = dr["RDate"].ToString()
                                   }).ToList();

            //if (filters.EmployeeIds.Contains("22222222-2222-2222-2222-222222222222"))
            //{
            //    ds = _InventoryDataAccess.GetDetailedEquipmentTransfersByFiltersWH(filters);
            //    ticketsTR = ds.Tables[0];
            //    //Model.TransfersCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
            //    Model.TransfersCount += ticketsTR.Rows.Count;
            //    if (ticketsTR != null)
            //    {
            //        List<DetailedEquipmentTransferListItem> TransfersWH = new List<DetailedEquipmentTransferListItem>();
            //        TransfersWH = (from DataRow dr in ticketsTR.Rows
            //                       select new DetailedEquipmentTransferListItem()
            //                       {
            //                           TechName = dr["TTech"].ToString() + " ➡️ " + dr["RTech"].ToString(),
            //                           //EquipmentId = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
            //                           EquipmentName = dr["Equipment"].ToString(),
            //                           SKU = dr["SKU"].ToString(),
            //                           Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
            //                           Date = dr["RDate"].ToString()
            //                       }).ToList();
            //        Model.Transfers.AddRange(TransfersWH);
            //    }

            //}



            //Model.TotalCount = ds != null && ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;
            //Model.Searchtext = filters.Searchtext;
            //Model.PageNo = filters.PageNo;
            //Model.PageSize = filters.PageSize;
            return Model;
        }

        #endregion Digiture
    }
}
