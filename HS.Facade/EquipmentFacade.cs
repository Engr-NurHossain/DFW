using HS.DataAccess;
using HS.Framework;
using HS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace HS.Facade
{
    public class EquipmentFacade : BaseFacade
    {
        public EquipmentFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        EquipmentDataAccess _EquipmentDataAccess
        {
            get
            {
                return (EquipmentDataAccess)_ClientContext[typeof(EquipmentDataAccess)];
            }
        }
        EquipmentClassDataAccess _EquipmentClassDataAccess
        {
            get
            {
                return (EquipmentClassDataAccess)_ClientContext[typeof(EquipmentClassDataAccess)];
            }
        }
        EquipmentVendorDataAccess _EquipmentVendorDataAccess
        {
            get
            {
                return (EquipmentVendorDataAccess)_ClientContext[typeof(EquipmentVendorDataAccess)];
            }
        }
        EquipmentManufacturerDataAccess _EquipmentManufacturerDataAccess
        {
            get
            {
                return (EquipmentManufacturerDataAccess)_ClientContext[typeof(EquipmentManufacturerDataAccess)];
            }
        }
        ServiceEquipmentDataAccess _ServiceEquipmentDataAccess
        {
            get
            {
                return (ServiceEquipmentDataAccess)_ClientContext[typeof(ServiceEquipmentDataAccess)];
            }
        }
        EquipmentTechnicianReorderPointDataAccess _EquipmentTechnicianReorderPointDataAccess
        {
            get
            {
                return (EquipmentTechnicianReorderPointDataAccess)_ClientContext[typeof(EquipmentTechnicianReorderPointDataAccess)];
            }
        }
        CustomerPackageServiceDataAccess _CustomerPackageServiceDataAccess
        {
            get
            {
                return (CustomerPackageServiceDataAccess)_ClientContext[typeof(CustomerPackageServiceDataAccess)];
            }
        }
        EquipmentTypeDataAccess _EquipmentTypeDataAccess
        {
            get
            {
                return (EquipmentTypeDataAccess)_ClientContext[typeof(EquipmentTypeDataAccess)];
            }
        }
        ManufacturerDataAccess _ManufacturerDataAccess
        {
            get
            {
                return (ManufacturerDataAccess)_ClientContext[typeof(ManufacturerDataAccess)];
            }
        }

        TicketDataAccess _TicketDataAccess
        {
            get
            {
                return (TicketDataAccess)_ClientContext[typeof(TicketDataAccess)];
            }
        }
        EquipmentReturnDataAccess _EquipmentReturnDataAccess
        {
            get
            {
                return (EquipmentReturnDataAccess)_ClientContext[typeof(EquipmentReturnDataAccess)];
            }
        }

        InventoryTechDataAccess _InventoryTechDataAccess
        {
            get
            {
                return (InventoryTechDataAccess)_ClientContext[typeof(InventoryTechDataAccess)];
            }
        }
        

        public Equipment GetEquipmentById(int EqId)
        {
            return _EquipmentDataAccess.GetEquipmentById(EqId);

            #region Prev 
            //Equipment EquipmentServiceList = (from DataRow dr in dt.Rows
            //                        select new Equipment()
            //                        {
            //                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
            //                            EquipmentId = (Guid)dr["EquipmentId"],
            //                            CompanyId = (Guid)dr["CompanyId"],
            //                            Name = dr["Name"].ToString(),
            //                            SKU = dr["SKU"].ToString(),

            //                            ManufacturerId = dr["ManufacturerId"] != DBNull.Value ? Convert.ToInt32(dr["ManufacturerId"]) : 0,
            //                            ManufacturerName = dr["ManufacturerName"].ToString(),

            //                            SupplierId = dr["SupplierId"] != DBNull.Value ? Convert.ToInt32(dr["SupplierId"]) : 0,
            //                            SupplierName = dr["SupplierName"].ToString(),
            //                            SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0.0,

            //                            EquipmentTypeId = dr["EquipmentTypeId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentTypeId"]) : 0,
            //                            EquipmentType = dr["EquipmentType"].ToString(),

            //                            EquipmentClassId = dr["EquipmentClassId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentClassId"]) : 0,
            //                            EquipmentClass = dr["EquipmentClass"].ToString(),

            //                            Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0.0,
            //                            Cost = dr["Cost"] != DBNull.Value ? Convert.ToDouble(dr["Cost"]) : 0.0,
            //                            Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0.0,
            //                            EqOrder = dr["EqOrder"] != DBNull.Value ? Convert.ToInt32(dr["EqOrder"]) : 0,
            //                            Service = dr["Service"].ToString(),
            //                            Comments = dr["Comments"].ToString(),
            //                            ProfilePicture = dr["ProfilePicture"].ToString(),
            //                            AsOfDate = dr["AsOfDate"] != DBNull.Value ? Convert.ToDateTime(dr["AsOfDate"]) : new DateTime(),
            //                            reorderpoint = dr["reorderpoint"] != DBNull.Value ? Convert.ToInt32(dr["reorderpoint"]) : 0,
            //                        }).FirstOrDefault();
            //return EquipmentServiceList; 
            #endregion
        }

        public List<EquipmentOptions> GetEquipmentOptionsByKeyAndType(string key, string type)
        {
            return _EquipmentDataAccess.GetEquipmentOptionsByKeyAndType(key, type);
        }
        public InventoryTech NewGetAllInventoryTechByTechnicianIdAndEquipmentIdAndType(Guid techid, Guid equipid)
        {
            return _InventoryTechDataAccess.GetByQuery(string.Format("TechnicianId = '{0}' and EquipmentId = '{1}'", techid, equipid)).FirstOrDefault();
        }
        public Equipment GetEquipmentByEquipmentId(Guid EquipmentId)
        {
            return _EquipmentDataAccess.GetEquipmentById(0, EquipmentId);
        }
        public Equipment GetEquipmentByEquipmentName(string eqpName)
        {
            return _EquipmentDataAccess.GetByQuery(string.Format("Name = '{0}'", eqpName)).FirstOrDefault();
        }
        public Equipment GetEquipmentByIdAndCompanyId(int id, Guid CompanyId)
        {
            return _EquipmentDataAccess.GetByQuery(string.Format("Id = '{0}' and CompanyId='{1}'", id, CompanyId)).FirstOrDefault();
        }

        public Equipment GetEquipmentByEquipmentIdAndCompanyId(Guid EquipmentId, Guid CompanyId)
        {
            return _EquipmentDataAccess.GetByQuery(string.Format("EquipmentId = '{0}' and CompanyId='{1}'", EquipmentId, CompanyId)).FirstOrDefault();
        }
        public long InsertEquipment(Equipment equipment)
        {
            return _EquipmentDataAccess.Insert(equipment);
        }
        public long InsertEquipmentVendor(EquipmentVendor equipment)
        {
            return _EquipmentVendorDataAccess.Insert(equipment);
        }
        public long InsertEquipmentManufacturer(EquipmentManufacturer equipment)
        {
            if(equipment!= null && equipment.IsPrimary == true)
            {
                _EquipmentManufacturerDataAccess.UpdateEquipmentManufacturerSetIsPrimaryFalse(equipment.EquipmentId);
            }
            return _EquipmentManufacturerDataAccess.Insert(equipment);
        }
        public List<EquipmentVendor> GetAllEquipmentVendorByEquipmentId(Guid EquipmentId)
        {
            DataTable dt = _EquipmentDataAccess.GetAllEquipmentVendorByEquipmentId(EquipmentId);
            List<EquipmentVendor> EquipmentServiceList = new List<EquipmentVendor>();
            if (dt != null)
                EquipmentServiceList = (from DataRow dr in dt.Rows
                                        select new EquipmentVendor()
                                        {

                                            Name = dr["Name"].ToString(),
                                            SKU = dr["SKU"].ToString(),

                                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                            Cost = dr["Cost"] != DBNull.Value ? Convert.ToDouble(dr["Cost"]) : 0.0,
                                            IsPrimary = dr["IsPrimary"] != DBNull.Value ? Convert.ToBoolean(dr["IsPrimary"]) : false,
                                        }).ToList();
            return EquipmentServiceList;
        }

        public List<Manufacturer> GetManufacturersByEquipmentId(Guid equipmentId)
        {
            return _ManufacturerDataAccess.GetManufacturersByEquipmentId(equipmentId);
        }

        public List<EquipmentManufacturer> GetAllEquipmentManufacturerByEquipmentId(Guid EquipmentId)
        {
            DataTable dt = _EquipmentDataAccess.GetAllEquipmentManufacturerByEquipmentId(EquipmentId);
            List<EquipmentManufacturer> EquipmentServiceList = new List<EquipmentManufacturer>();
            if (dt != null)
                EquipmentServiceList = (from DataRow dr in dt.Rows
                                        select new EquipmentManufacturer()
                                        {

                                            Name = dr["Name"].ToString(),
                                            SKU = dr["SKU"].ToString(),
                                            Variation = dr["Variation"].ToString(),
                                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                            
                                            IsPrimary = dr["IsPrimary"] != DBNull.Value ? Convert.ToBoolean(dr["IsPrimary"]) : false,
                                        }).ToList();
            return EquipmentServiceList;
        }
        public List<ServiceEquipment> GetEquipmentServiceListByServiceId(Guid equipmentId)
        {
            return _ServiceEquipmentDataAccess.GetEquipmentServiceListByServiceId(equipmentId);
        }

        public EquipmentVendor GetEquipmentVendorById(int id)
        {
            return _EquipmentVendorDataAccess.Get(id);
        }
        public EquipmentManufacturer GetEquipmentManufacturerById(int id)
        {
            return _EquipmentManufacturerDataAccess.Get(id);
        }
        public List<EquipmentVendor> GetEquipmentVendorByEquipmentId(Guid EquipmentId)
        {
            var query = "EquipmentId='" + EquipmentId + "'";
            return _EquipmentVendorDataAccess.GetByQuery(query);
        }
        public List<EquipmentManufacturer> GetEquipmentManufacturerByEquipmentId(Guid EquipmentId)
        {
            var query = "EquipmentId='" + EquipmentId + "'";
            return _EquipmentManufacturerDataAccess.GetByQuery(query);
        }
        
        public EquipmentVendor GetEquipmentVendorById(Guid EquipmentId, Guid SupplierId)
        {
            var query = "EquipmentId='" + EquipmentId + "' And SupplierId='" + SupplierId + "'";
            return _EquipmentVendorDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public bool UpdateEquipmentVendorSetIsPrimaryFalse(Guid equipmentId)
        {
            return _EquipmentVendorDataAccess.UpdateEquipmentVendorSetIsPrimaryFalse(equipmentId);
        }
        public bool UpdateEquipmentManufacturerSetIsPrimaryFalse(Guid equipmentId)
        {
            return _EquipmentManufacturerDataAccess.UpdateEquipmentManufacturerSetIsPrimaryFalse(equipmentId);
        }
        public bool UpdateEquipment(Equipment equipment)
        {
            return _EquipmentDataAccess.Update(equipment) > 0;
        }

        public bool UpdateEquipmentReturn(EquipmentReturn EquipmentReturn)
        {
            return _EquipmentReturnDataAccess.Update(EquipmentReturn) > 0;
        }

        public bool UpdateEquipmentVendor(EquipmentVendor equipment)
        {
            return _EquipmentVendorDataAccess.Update(equipment) > 0;
        }
        public bool UpdateEquipmentManufacturer(EquipmentManufacturer equipment)
        {
            return _EquipmentManufacturerDataAccess.Update(equipment) > 0;
        }
        public List<Equipment> GetAllEquipmentServiceByCompanyId(Guid CompanyId)
        {
            DataTable dt = _EquipmentDataAccess.GetAllEquipmentServiceByCompany(CompanyId);
            List<Equipment> EquipmentServiceList = new List<Equipment>();
            EquipmentServiceList = (from DataRow dr in dt.Rows
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
                                        AsOfDate = dr["AsOfDate"] != DBNull.Value ? Convert.ToDateTime(dr["AsOfDate"]) : new DateTime(),
                                        reorderpoint = dr["reorderpoint"] != DBNull.Value ? Convert.ToInt32(dr["reorderpoint"]) : 0,
                                    }).ToList();
            return EquipmentServiceList;
        }
        public List<CustomerAppointmentEquipment> GetCAEquipmentListByCompanyIdTechnicianId(MassRestockFilter massFilter)
        {
            DataTable dt = _EquipmentDataAccess.GetEquipmentListDataTechnicianUpsold(massFilter);
            List<CustomerAppointmentEquipment> EquipmentList = new List<CustomerAppointmentEquipment>();
            if(dt != null)
            {
                EquipmentList = (from DataRow dr in dt.Rows
                                 select new CustomerAppointmentEquipment()
                                 {
                                     Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                     FileName = dr["Name"].ToString(),
                                     AppointmentId = (Guid)dr["AppointmentId"],
                                     Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                     UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0.0,
                                     TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0.0,
                                     CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                     CreatedBy = dr["CreatedBy"].ToString(),
                                     EquipName = dr["EquipName"].ToString(),
                                     EquipDetail = dr["EquipDetail"].ToString(),
                                     IsEquipmentRelease = dr["IsEquipmentRelease"] != DBNull.Value ? Convert.ToBoolean(dr["IsEquipmentRelease"]) : false,
                                     IsService = dr["IsService"] != DBNull.Value ? Convert.ToBoolean(dr["IsService"]) : false,
                                     CreatedByUid = dr["CreatedByUid"] != DBNull.Value ? (Guid)dr["CreatedByUid"] : new Guid(),
                                     IsAgreementItem = dr["IsAgreementItem"] != DBNull.Value ? Convert.ToBoolean(dr["IsAgreementItem"]) : false,
                                     IsBaseItem = dr["IsBaseItem"] != DBNull.Value ? Convert.ToBoolean(dr["IsBaseItem"]) : false,
                                     IsBadInventory = dr["IsBadInventory"] != DBNull.Value ? Convert.ToBoolean(dr["IsBadInventory"]) : false,
                                     IsDefaultService = dr["IsDefaultService"] != DBNull.Value ? Convert.ToBoolean(dr["IsDefaultService"]) : false,
                                     IsCheckedEquipment = dr["IsCheckedEquipment"] != DBNull.Value ? Convert.ToBoolean(dr["IsCheckedEquipment"]) : false,
                                     IsTransfered = dr["IsTransfered"] != DBNull.Value ? Convert.ToBoolean(dr["IsTransfered"]) : false,
                                     QuantityLeftEquipment = dr["QuantityLeftEquipment"] != DBNull.Value ? Convert.ToInt32(dr["QuantityLeftEquipment"]) : 0,
                                 }).ToList();
            }
            
            return EquipmentList;
        }

        public List<CustomerAppointmentEquipment> GetCAUpsoldListByCompanyIdTechnicianId(MassRestockFilter massFilter)
        {
            DataTable dt = _EquipmentDataAccess.GetUpsoldListDataTechnicianUpsold(massFilter);
            List<CustomerAppointmentEquipment> EquipmentList = new List<CustomerAppointmentEquipment>();
            if (dt != null)
            {
                EquipmentList = (from DataRow dr in dt.Rows
                                 select new CustomerAppointmentEquipment()
                                 {
                                     Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                     FileName = dr["Name"].ToString(),
                                     AppointmentId = (Guid)dr["AppointmentId"],
                                     Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                     UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0.0,
                                     TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0.0,
                                     CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                     CreatedBy = dr["CreatedBy"].ToString(),
                                     EquipName = dr["EquipName"].ToString(),
                                     EquipDetail = dr["EquipDetail"].ToString(),
                                     IsEquipmentRelease = dr["IsEquipmentRelease"] != DBNull.Value ? Convert.ToBoolean(dr["IsEquipmentRelease"]) : false,
                                     IsService = dr["IsService"] != DBNull.Value ? Convert.ToBoolean(dr["IsService"]) : false,
                                     CreatedByUid = dr["CreatedByUid"] != DBNull.Value ? (Guid)dr["CreatedByUid"] : new Guid(),
                                     IsAgreementItem = dr["IsAgreementItem"] != DBNull.Value ? Convert.ToBoolean(dr["IsAgreementItem"]) : false,
                                     IsBaseItem = dr["IsBaseItem"] != DBNull.Value ? Convert.ToBoolean(dr["IsBaseItem"]) : false,
                                     IsBadInventory = dr["IsBadInventory"] != DBNull.Value ? Convert.ToBoolean(dr["IsBadInventory"]) : false,
                                     IsDefaultService = dr["IsDefaultService"] != DBNull.Value ? Convert.ToBoolean(dr["IsDefaultService"]) : false,
                                     IsCheckedEquipment = dr["IsCheckedEquipment"] != DBNull.Value ? Convert.ToBoolean(dr["IsCheckedEquipment"]) : false,
                                     IsTransfered = dr["IsTransfered"] != DBNull.Value ? Convert.ToBoolean(dr["IsTransfered"]) : false,
                                     QuantityLeftEquipment = dr["QuantityLeftEquipment"] != DBNull.Value ? Convert.ToInt32(dr["QuantityLeftEquipment"]) : 0,
                                 }).ToList();
            }

            return EquipmentList;
        }

        public TicketListModel GetAllTicketReportByFilter(TicketFilter Filters)
        {
            TicketListModel Model = new TicketListModel();
            DataSet ds = _EquipmentDataAccess.GetTicketListByFilter(Filters);
            Model.Tickets = (from DataRow dr in ds.Tables[0].Rows
                             select new Ticket()
                             {
                                 Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                 CompanyId = (Guid)dr["CompanyId"],
                                 CustomerName = dr["CustomerName"].ToString(),
                                 CustomerId = (Guid)dr["CustomerId"],
                                 TicketId = (Guid)dr["TicketId"],
                                 LastUpdatedBy = (Guid)dr["LastUpdatedBy"],
                                 CreatedBy = (Guid)dr["TicketId"],
                                 Status = dr["Status"].ToString(),
                                 TicketType = dr["TicketType"].ToString(),
                                 AssignedTo = dr["AssignedTo"].ToString().TrimEnd(' ', ','),
                                 AdditionalMembers = dr["AdditionalMembers"].ToString().TrimEnd(' ', ','),
                                 Priority = dr["Priority"].ToString(),
                                 Subject = dr["Subject"].ToString(),
                                 Message = dr["Message"].ToString(),
                                 LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                 CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                 CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                                 AttachmentsCount = dr["AttachmentsCount"] != DBNull.Value ? Convert.ToInt32(dr["AttachmentsCount"]) : 0,
                                 RepliesCount = dr["RepliesCount"] != DBNull.Value ? Convert.ToInt32(dr["RepliesCount"]) : 0,
                                 AppointmentStartTimeVal = dr["AppointmentStartTimeVal"].ToString(),
                                 AppointmentStartTime = dr["AppointmentStartTime"].ToString(),
                                 AppointmentEndTime = dr["AppointmentStartTime"].ToString(),
                                 AppointmentEndTimeVal = dr["AppointmentEndTimeVal"].ToString(),
                                 TicketTypeVal = dr["TicketTypeVal"].ToString(),
                                 StatusVal = dr["StatusVal"].ToString(),
                                 PriorityVal = dr["PriorityVal"].ToString(),
                                 CreatedByVal = dr["CreatedByVal"].ToString(),
                                 ExceedQuantity = dr["ExceedQuantity"] != DBNull.Value ? Convert.ToInt32(dr["ExceedQuantity"]) : 0,
                                 IsClosed = dr["IsClosed"] != DBNull.Value ? Convert.ToBoolean(dr["IsClosed"]) : false,
                                 CusIdInt = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                                 CusBusinessName = dr["CusBusinessName"].ToString(),
                                 CusSalesPerson = dr["CusSalesPerson"].ToString(),
                                 CusInstaller = dr["CusInstaller"].ToString(),
                                 RMRAmount = dr["RMRAmount"] != DBNull.Value ? Convert.ToDouble(dr["RMRAmount"]) : 0.0,
                                 PrevTicketType = dr["PrevTicketType"].ToString(),
                                 PrevAppointmentDate = dr["PrevAppointmentDate"] != DBNull.Value ? Convert.ToDateTime(dr["PrevAppointmentDate"]) : new DateTime(),
                                 PrevTechnician = dr["PrevTechnician"].ToString(),
                                 CusSalesLoc = dr["CusSalesLoc"].ToString()
                             }).ToList();
            Model.TotalCount = ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;
            Model.PageNo = Filters.PageNo.Value;
            Model.PageSize = Filters.PageSize;
            Model.Searchtext = Filters.SearchText;
            Model.TicketType = Filters.TicketType;
            Model.TicketStatus = Filters.TicketStatus;
            Model.Assigned = Filters.Assigned;
            Model.StartDate = Filters.StartDate;
            Model.EndDate = Filters.EndDate;
            return Model;
            // return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerId));
        }

        public MassRestockModel GetEquipmentListByCompanyIdTechnicianId(MassRestockFilter massFilter)
        {
            DataSet dsResult = _EquipmentDataAccess.GetEquipmentListByCompanyIdTechnicianId(massFilter);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            MassRestockModel Model = new MassRestockModel();
            Model.MassRestockList = (from DataRow dr in dt.Rows
                             select new MassRestock()
                             {
                                 EquipmentId = dr["EquipmentId"] != DBNull.Value ? (Guid)dr["EquipmentId"] : new Guid(),
                                 TechnicianId = dr["TechnicianId"] != DBNull.Value ? (Guid)dr["TechnicianId"] : new Guid(),
                                 Name = dr["Name"].ToString(),
                                 ManufacturerName = dr["ManufacturerName"].ToString(),
                                 SKU = dr["SKU"].ToString(),
                                 Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                 ReorderPoint = dr["ReorderPoint"] != DBNull.Value ? Convert.ToInt32(dr["ReorderPoint"]) : 0,
                                 WHReorderPoint = dr["WHReorderPoint"] != DBNull.Value ? Convert.ToInt32(dr["WHReorderPoint"]) : 0,
                                 Have = dr["Have"] != DBNull.Value ? Convert.ToInt32(dr["Have"]) : 0,
                                 New = 0
                             }).ToList();
            Model.TotalQty = dt1.Rows[0]["TotalQty"] != DBNull.Value ? Convert.ToInt32(dt1.Rows[0]["TotalQty"]) : 0;
            Model.TotalPoint = dt1.Rows[0]["TotalPoint"] != DBNull.Value ? Convert.ToInt32(dt1.Rows[0]["TotalPoint"]) : 0;
            Model.TotalHave = dt1.Rows[0]["TotalHave"] != DBNull.Value ? Convert.ToInt32(dt1.Rows[0]["TotalHave"]) : 0;
            Model.TotalWHPoint= dt1.Rows[0]["TotalWHPoint"] != DBNull.Value ? Convert.ToInt32(dt1.Rows[0]["TotalWHPoint"]) : 0;
            return Model;
        }

        public MassRestockModel GetEquipmentListByCompanyIdTechnicianId_v2(MassRestockFilter massFilter)
        {
            DataSet dsResult = _EquipmentDataAccess.GetEquipmentListByCompanyIdTechnicianId(massFilter);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            MassRestockModel Model = new MassRestockModel();
            Model.MassRestockList = (from DataRow dr in dt.Rows
                                     select new MassRestock()
                                     {
                                         EquipmentId = dr["EquipmentId"] != DBNull.Value ? (Guid)dr["EquipmentId"] : new Guid(),
                                         TechnicianId = dr["TechnicianId"] != DBNull.Value ? (Guid)dr["TechnicianId"] : new Guid(),
                                         Name = dr["Name"].ToString(),
                                         ManufacturerName = dr["ManufacturerName"].ToString(),
                                         SKU = dr["SKU"].ToString(),
                                         Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                         ReorderPoint = dr["ReorderPoint"] != DBNull.Value ? Convert.ToInt32(dr["ReorderPoint"]) : 0,
                                         WHReorderPoint = dr["WHReorderPoint"] != DBNull.Value ? Convert.ToInt32(dr["WHReorderPoint"]) : 0,
                                         Have = dr["Have"] != DBNull.Value ? Convert.ToInt32(dr["Have"]) : 0,
                                         New = 0
                                     }).OrderBy(x => x.Name).ToList();
            Model.TotalQty = dt1.Rows[0]["TotalQty"] != DBNull.Value ? Convert.ToInt32(dt1.Rows[0]["TotalQty"]) : 0;
            Model.TotalPoint = dt1.Rows[0]["TotalPoint"] != DBNull.Value ? Convert.ToInt32(dt1.Rows[0]["TotalPoint"]) : 0;
            Model.TotalHave = dt1.Rows[0]["TotalHave"] != DBNull.Value ? Convert.ToInt32(dt1.Rows[0]["TotalHave"]) : 0;
            Model.TotalWHPoint = dt1.Rows[0]["TotalWHPoint"] != DBNull.Value ? Convert.ToInt32(dt1.Rows[0]["TotalWHPoint"]) : 0;
            return Model;
        }


        public List<MassRestock> GetEquipmentListByCompanyIdTechnicianIdReorderPoint(MassRestockFilter massFilter)
        {
            DataTable dt = _EquipmentDataAccess.GetEquipmentListByCompanyIdTechnicianIdReorderPoint(massFilter);
            List<MassRestock> EquipmentList = new List<MassRestock>();
            EquipmentList = (from DataRow dr in dt.Rows
                             select new MassRestock()
                             {
                                 EquipmentId = dr["EquipmentId"] != DBNull.Value ? (Guid)dr["EquipmentId"] : new Guid(),
                                 TechnicianId = dr["TechnicianId"] != DBNull.Value ? (Guid)dr["TechnicianId"] : new Guid(),
                                 Name = dr["Name"].ToString(),
                                 ManufacturerName = dr["ManufacturerName"].ToString(),
                                 SKU = dr["SKU"].ToString(),
                                 Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                 ReorderPoint = dr["ReorderPoint"] != DBNull.Value ? Convert.ToInt32(dr["ReorderPoint"]) : 0,
                                 Have = dr["Have"] != DBNull.Value ? Convert.ToInt32(dr["Have"]) : 0,
                                 New = 0
                             }).ToList();
            return EquipmentList;
        }
        public List<MassPO> GetMassPOEquipmentListByCompanyId(Guid CompanyId)
        {
            DataTable dt = _EquipmentDataAccess.GetMassPOEquipmentListByCompanyId(CompanyId);
            List<MassPO> EquipmentList = new List<MassPO>();
            EquipmentList = (from DataRow dr in dt.Rows
                             select new MassPO()
                             {
                                 DemandOrderId = dr["DemandOrderId"].ToString(),
                                 TechDemandOrderId = dr["TechDemandOrderId"].ToString(),
                                 EquipmentId = dr["EquipmentId"] != DBNull.Value ? (Guid)dr["EquipmentId"] : new Guid(),
                                 Name = dr["Name"].ToString(),
                                 ManufacturerName = dr["ManufacturerName"].ToString(),
                                 SKU = dr["SKU"].ToString(),
                                 PrimaryVendor = dr["PrimaryVendor"].ToString(),
                                 SupplierId = dr["SupplierId"] != DBNull.Value ? (Guid)dr["SupplierId"] : new Guid(),
                                 Price = dr["Price"] != DBNull.Value ? Convert.ToDouble(dr["Price"]) : 0.0,
                                 Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                             }).ToList();
            return EquipmentList;
        }
        public List<EquipmentTechnicianReorderPoint> GetEqpTechRP()
        {
            DataTable dt = _EquipmentTechnicianReorderPointDataAccess.GetEqpTechRP();
            List<EquipmentTechnicianReorderPoint> GetEqpTechRPList = new List<EquipmentTechnicianReorderPoint>();
            GetEqpTechRPList = (from DataRow dr in dt.Rows
                                select new EquipmentTechnicianReorderPoint()
                                {
                                    EquipmentId = (Guid)dr["EquipmentId"],
                                    TechnicianId = (Guid)dr["TechnicianId"],
                                    ReorderPoint = dr["ReorderPoint"] != DBNull.Value ? Convert.ToInt32(dr["ReorderPoint"]) : 0,
                                    Have = dr["Have"] != DBNull.Value ? Convert.ToInt32(dr["Have"]) : 0,
                                    New = 0,
                                }).ToList();
            return GetEqpTechRPList;
        }

        public List<Equipment> GetAllEquipmentByCompanyIdAndClassId(Guid CompanyId, int ClassId)
        {
            return _EquipmentDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and EquipmentClassId = {1} ", CompanyId, ClassId));
        }

        public bool ConvertActiveEquipmentService(int equipmentServiceId)
        {
            bool IsSuccess = false;
            var objEquipmentService = _EquipmentDataAccess.GetByQuery(string.Format("Id = '{0}'", equipmentServiceId)).FirstOrDefault();
            objEquipmentService.IsActive = false;
            return IsSuccess = _EquipmentDataAccess.Update(objEquipmentService) > 0;
        }

        public List<Equipment> GetEquipmentByOptions(string location, string type, string model, string finish, string capacity,Guid ManufacturerId)
        {

            return _EquipmentDataAccess.GetEquipmentByOptions(location, type, model, finish, capacity, ManufacturerId);
            
            //return _EquipmentDataAccess.GetByQuery(string.Format(" [Location] ='{0}' and [Type] = '{1}' and Model = '{2}' and Finish = '{3}' and Capacity = '{4}'"
            //    , location
            //    ,type
            //    ,model
            //    ,finish
            //    ,capacity));
        }
        public List<Equipment> GetIncludeEstimateEquipment()
        {
            DataTable dt = _EquipmentDataAccess.GetIncludeEstimateEquipment();
            List<Equipment> equipment = new List<Equipment>();
            equipment = (from DataRow dr in dt.Rows
                                 select new Equipment()
                                 {
                                     Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                     EquipmentId = (Guid)dr["EquipmentId"],
                                     EquipmentClassId = dr["EquipmentClassId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentClassId"]) : 0,
                                     Name = dr["Name"].ToString(),
                                     Description = dr["Comments"].ToString(),
                                     Quantity = 1,
                                     Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0.0,
                                     Total = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0.0





                                 }).ToList();
            return equipment;


        }
        public bool ConvertInactiveEquipmentService(int equipmentServiceId)
        {
            bool IsSuccess = false;
            var objEquipmentService = _EquipmentDataAccess.GetByQuery(string.Format("Id = '{0}'", equipmentServiceId)).FirstOrDefault();
            objEquipmentService.IsActive = true;
            return IsSuccess = _EquipmentDataAccess.Update(objEquipmentService) > 0;
        }
        public bool ConvertInActiveEquipmentService(int equipmentServiceId)
        {
            bool IsSuccess = false;
            var objEquipmentService = _EquipmentDataAccess.GetByQuery(string.Format("Id = '{0}'", equipmentServiceId)).FirstOrDefault();
            objEquipmentService.IsActive = true;
            return IsSuccess = _EquipmentDataAccess.Update(objEquipmentService) > 0;
        }
        public List<EquipmentClass> GetAllEquipmentClassByCompanyId(Guid CompanyId)
        {
            return _EquipmentClassDataAccess.GetByQuery(string.Format("CompanyId= '{0}'", CompanyId));
        }
        public string GetEquipmentServiceNameByEquipmentId(Guid EquipmentId)
        {
            return _EquipmentDataAccess.GetByQuery(string.Format("EquipmentId = '{0}'", EquipmentId)).FirstOrDefault().Name;
        }
        public bool DeleteEquipment(int id)
        {
            return _EquipmentDataAccess.Delete(id) > 0;
        }
        public bool DeleteEquipmentVendorById(int id)
        {
            return _EquipmentVendorDataAccess.Delete(id) > 0;
        }
        public bool DeleteEquipmentManufacturerById(int id)
        {
            return _EquipmentManufacturerDataAccess.Delete(id) > 0;
        }
        public Equipment GetEquipmentServiceCommentsByEquipmentId(Guid EquipmentId)
        {
            return _EquipmentDataAccess.GetByQuery(string.Format("EquipmentId = '{0}'", EquipmentId)).FirstOrDefault();
        }

        public List<Equipment> GetEquipmentListByCustomerIdAndCompanyId(int leadId, Guid CompanyId)
        {
            DataTable dt = _EquipmentDataAccess.GetEquipmentListByCustomerIdAndCompanyId(leadId, CompanyId);
            List<Equipment> EquipmentServiceList = new List<Equipment>();
            EquipmentServiceList = (from DataRow dr in dt.Rows
                                    select new Equipment()
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        EquipmentId = (Guid)dr["EquipmentId"],
                                        CompanyId = (Guid)dr["CompanyId"],
                                        Name = dr["Name"].ToString(),
                                        SKU = dr["SKU"].ToString(),
                                        //ManufacturerId = dr["ManufacturerId"] != DBNull.Value ? Convert.ToInt32(dr["ManufacturerId"]) : 0,
                                        //SupplierId = dr["SupplierId"] != DBNull.Value ? Convert.ToInt32(dr["SupplierId"]) : 0,
                                        //EquipmentTypeId = dr["EquipmentTypeId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentTypeId"]) : 0,
                                        //EquipmentClassId = dr["EquipmentClassId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentClassId"]) : 0,
                                        Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0.0,
                                        //SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0.0,
                                        //Cost = dr["Cost"] != DBNull.Value ? Convert.ToDouble(dr["Cost"]) : 0.0,
                                        Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0.0,
                                        //EqOrder = dr["EqOrder"] != DBNull.Value ? Convert.ToInt32(dr["EqOrder"]) : 0,
                                        //Service = dr["Service"].ToString(),
                                        //AsOfDate = dr["AsOfDate"] != DBNull.Value ? Convert.ToDateTime(dr["AsOfDate"]) : new DateTime(),
                                        //reorderpoint = dr["reorderpoint"] != DBNull.Value ? Convert.ToInt32(dr["reorderpoint"]) : 0,
                                        TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0.0,
                                        UnitPriceAppointmentEquipment = dr["UnitPriceAppointmentEquipment"] != DBNull.Value ? Convert.ToDouble(dr["UnitPriceAppointmentEquipment"]) : 0.0,
                                        QuantityAppointmentEquipment = dr["QuantityAppointmentEquipment"] != DBNull.Value ? Convert.ToDouble(dr["QuantityAppointmentEquipment"]) : 0.0,
                                    }).ToList();
            return EquipmentServiceList;
        }

        public List<Equipment> GetSmartServiceListByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId, bool firstpage, int ticketid)
        {
            Guid ticketguidid = new Guid();
            if(ticketid > 0)
            {
                var objticket = _TicketDataAccess.GetByQuery(string.Format("Id = {0}", ticketid)).FirstOrDefault();
                if(objticket != null)
                {
                    ticketguidid = objticket.TicketId;
                }
            }
            DataTable dt = _EquipmentDataAccess.GetSmartServiceListByCustomerIdAndCompanyId(CustomerId, CompanyId, firstpage, ticketguidid);
            List<Equipment> EquipmentServiceList = new List<Equipment>();
            EquipmentServiceList = (from DataRow dr in dt.Rows
                                    select new Equipment()
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        EquipmentId = dr["EquipmentId"] != DBNull.Value ? Guid.Parse(dr["EquipmentId"].ToString()) : Guid.Empty,
                                        CompanyId = dr["CompanyId"] != DBNull.Value ? Guid.Parse(dr["CompanyId"].ToString()) : Guid.Empty,
                                        Name = dr["Name"].ToString(),
                                        SKU = dr["SKU"].ToString(),
                                        Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0.0,
                                        Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0.0,
                                        MonthlyRate = dr["MonthlyRate"] != DBNull.Value ? Convert.ToInt32(dr["MonthlyRate"]) : 0,
                                        DiscountRate = dr["DiscountRate"] != DBNull.Value ? Convert.ToDouble(dr["DiscountRate"]) : 0.0,
                                        Total = dr["Total"] != DBNull.Value ? Convert.ToDouble(dr["Total"]) : 0.0,
                                        IsARBEnabled = dr["IsARBEnabled"] != DBNull.Value ? Convert.ToBoolean(dr["IsARBEnabled"]) : false
                                    }).ToList();
            return EquipmentServiceList;
        }
        public List<Equipment> GetSmartEstimatorServiceListByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId, bool firstpage, int ticketid,string EstimatorId)
        {
            Guid ticketguidid = new Guid();
            if (ticketid > 0)
            {
                var objticket = _TicketDataAccess.GetByQuery(string.Format("Id = {0}", ticketid)).FirstOrDefault();
                if (objticket != null)
                {
                    ticketguidid = objticket.TicketId;
                }
            }
            DataTable dt = _EquipmentDataAccess.GetSmartEstimatorServiceListByCustomerIdAndCompanyId(CustomerId, CompanyId, firstpage, ticketguidid, EstimatorId);
            List<Equipment> EquipmentServiceList = new List<Equipment>();
            EquipmentServiceList = (from DataRow dr in dt.Rows
                                    select new Equipment()
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        EquipmentId = dr["EquipmentId"] != DBNull.Value ? Guid.Parse(dr["EquipmentId"].ToString()) : Guid.Empty,
                                        CompanyId = dr["CompanyId"] != DBNull.Value ? Guid.Parse(dr["CompanyId"].ToString()) : Guid.Empty,
                                        Name = dr["Name"].ToString(),
                                        //SKU = dr["SKU"].ToString(),
                                        Point = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0.0,
                                        Retail = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0.0,
                                        MonthlyRate = dr["TotalAmount"] != DBNull.Value ? Convert.ToInt32(dr["TotalAmount"]) : 0,
                                        //DiscountRate = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0.0,
                                        TotalestimatorTaxAmount = dr["TotalestimatorTaxAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalestimatorTaxAmount"]) : 0.0,
                                        Total = dr["Total"] != DBNull.Value ? Convert.ToDouble(dr["Total"]) : 0.0,
                                        IsTaxable = dr["IsTaxable"] != DBNull.Value ? Convert.ToBoolean(dr["IsTaxable"]) : false,
                                        IsARBEnabled = dr["IsARBEnabled"] != DBNull.Value ? Convert.ToBoolean(dr["IsARBEnabled"]) : false
                                    }).ToList();
            return EquipmentServiceList;
        }
        public List<Estimator> GetEstimatorSmartServiceListByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId, bool firstpage, int ticketid,int EstimatorId)
        {
            Guid ticketguidid = new Guid();
            if (ticketid > 0)
            {
                var objticket = _TicketDataAccess.GetByQuery(string.Format("Id = {0}", ticketid)).FirstOrDefault();
                if (objticket != null)
                {
                    ticketguidid = objticket.TicketId;
                }
            }
            DataTable dt = _EquipmentDataAccess.GetEstimatorSmartServiceListByCustomerIdAndCompanyId(CustomerId, CompanyId, firstpage, ticketguidid, EstimatorId);
            List<Estimator> EquipmentServiceList = new List<Estimator>();
            EquipmentServiceList = (from DataRow dr in dt.Rows
                                    select new Estimator()
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        CustomerId = dr["CustomerId"] != DBNull.Value ? Guid.Parse(dr["CustomerId"].ToString()) : Guid.Empty,//(Guid)dr["CustomerId"],
                                        CompanyId = dr["CompanyId"] != DBNull.Value ? Guid.Parse(dr["CompanyId"].ToString()) : Guid.Empty,//(Guid)dr["CompanyId"],
                                        Status = dr["Status"].ToString(),
                                        EstimatorId = dr["EstimatorId"].ToString(),
                                        EmailAddress = dr["EmailAddress"].ToString(),
                                        TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0.0,
                                        TotalEstimateServiceAmount = dr["TotalEstimateServiceAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalEstimateServiceAmount"]) : 0.0,
                                        ServiceTaxAmount = dr["ServiceTaxAmount"] != DBNull.Value ? Convert.ToInt32(dr["ServiceTaxAmount"]) : 0,
                                        ServiceTotalAmount = dr["ServiceTotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["ServiceTotalAmount"]) : 0.0,
                                         
                                    }).ToList();
            return EquipmentServiceList;
        }
        public List<Equipment> GetNotARBEnabledSmartServiceListFromService(Guid CustomerId, Guid CompanyId)
        {
            DataTable dt = _EquipmentDataAccess.GetNotARBEnabledSmartServiceListFromService(CustomerId, CompanyId);
            List<Equipment> EquipmentServiceList = new List<Equipment>();
            EquipmentServiceList = (from DataRow dr in dt.Rows
                                    select new Equipment()
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        EquipmentId = (Guid)dr["EquipmentId"],
                                        CompanyId = (Guid)dr["CompanyId"],
                                        Name = dr["Name"].ToString(),
                                        SKU = dr["SKU"].ToString(),
                                        Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0.0,
                                        Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0.0,
                                        MonthlyRate = dr["MonthlyRate"] != DBNull.Value ? Convert.ToInt32(dr["MonthlyRate"]) : 0,
                                        DiscountRate = dr["DiscountRate"] != DBNull.Value ? Convert.ToDouble(dr["DiscountRate"]) : 0.0,
                                        Total = dr["Total"] != DBNull.Value ? Convert.ToDouble(dr["Total"]) : 0.0,
                                        IsARBEnabled = dr["IsARBEnabled"] != DBNull.Value ? Convert.ToBoolean(dr["IsARBEnabled"]) : false
                                    }).ToList();
            return EquipmentServiceList;
        }
        public List<Equipment> GetNotARBEnabledSmartServiceListByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId, bool firstpage, int ticketid)
        {
            Guid ticketguidid = new Guid();
            if (ticketid > 0)
            {
                var objticket = _TicketDataAccess.GetByQuery(string.Format("Id = {0}", ticketid)).FirstOrDefault();
                if (objticket != null)
                {
                    ticketguidid = objticket.TicketId;
                }
            }
            DataTable dt = _EquipmentDataAccess.GetNotARBEnabledSmartServiceListByCustomerIdAndCompanyId(CustomerId, CompanyId, firstpage, ticketguidid);
            List<Equipment> EquipmentServiceList = new List<Equipment>();
            EquipmentServiceList = (from DataRow dr in dt.Rows
                                    select new Equipment()
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        EquipmentId = (Guid)dr["EquipmentId"],
                                        CompanyId = (Guid)dr["CompanyId"],
                                        Name = dr["Name"].ToString(),
                                        SKU = dr["SKU"].ToString(),
                                        Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0.0,
                                        Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0.0,
                                        MonthlyRate = dr["MonthlyRate"] != DBNull.Value ? Convert.ToInt32(dr["MonthlyRate"]) : 0,
                                        DiscountRate = dr["DiscountRate"] != DBNull.Value ? Convert.ToDouble(dr["DiscountRate"]) : 0.0,
                                        Total = dr["Total"] != DBNull.Value ? Convert.ToDouble(dr["Total"]) : 0.0,
                                        IsARBEnabled = dr["IsARBEnabled"] != DBNull.Value ? Convert.ToBoolean(dr["IsARBEnabled"]) : false
                                    }).ToList();
            return EquipmentServiceList;
        }
        public List<Equipment> GetSmartEquipmentEstimatorListByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId, bool firstpage, int ticketid,string EstimatorId)
        {
            Guid ticketguidid = new Guid();
            if (ticketid > 0)
            {
                var objticket = _TicketDataAccess.GetByQuery(string.Format("Id = {0}", ticketid)).FirstOrDefault();
                if (objticket != null)
                {
                    ticketguidid = objticket.TicketId;
                }
            }
            DataTable dt = _EquipmentDataAccess.GetSmartEquipmentEstimatorListByCustomerIdAndCompanyId(CustomerId, CompanyId, firstpage, ticketguidid, EstimatorId);
            List<Equipment> EquipmentServiceList = new List<Equipment>();
            EquipmentServiceList = (from DataRow dr in dt.Rows
                                    select new Equipment()
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        EquipmentId = dr["EquipmentId"] != DBNull.Value ? Guid.Parse(dr["EquipmentId"].ToString()) : Guid.Empty,
                                        CompanyId = dr["CompanyId"] != DBNull.Value ? Guid.Parse(dr["CompanyId"].ToString()) : Guid.Empty,
                                        Name = dr["Name"].ToString(), 
                                        SKU = dr["SKU"].ToString(),
                                        //Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0.0,
                                        //Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0.0,
                                        Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                        UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0.0,
                                        ActivationFee = dr["ActivationFee"] != DBNull.Value ? Convert.ToDouble(dr["ActivationFee"]) : 0.0,
                                        //DiscountUnitPricce = dr["TotalUnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalUnitPrice"]) : 0.0,
                                        Total = dr["TotalPriceValue"] != DBNull.Value ? Convert.ToDouble(dr["TotalPriceValue"]) : 0.0,
                                        //IsTransfered = dr["IsTransfered"] != DBNull.Value ? Convert.ToBoolean(dr["IsTransfered"]) : false,
                                        //IsEqpExist = dr["IsEqpExist"] != DBNull.Value ? Convert.ToBoolean(dr["IsEqpExist"]) : false,
                                        //TotalDiscountUnitPrice = dr["TotalDiscountUnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalDiscountUnitPrice"]) : 0.0,
                                        //DiscountPercentage = dr["DiscountPercent"] != DBNull.Value ? Convert.ToDouble(dr["DiscountPercent"]) : 0.0,
                                        //DiscountInCurrency = dr["DiscountInAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountInAmount"]) : 0.0,

                                    }).ToList();
            return EquipmentServiceList;
        }
        public List<Equipment> GetSmartEquipmentListByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId, bool firstpage, int ticketid)
        {
            Guid ticketguidid = new Guid();
            if (ticketid > 0)
            {
                var objticket = _TicketDataAccess.GetByQuery(string.Format("Id = {0}", ticketid)).FirstOrDefault();
                if (objticket != null)
                {
                    ticketguidid = objticket.TicketId;
                }
            }
            DataTable dt = _EquipmentDataAccess.GetSmartEquipmentListByCustomerIdAndCompanyId(CustomerId, CompanyId, firstpage, ticketguidid);
            List<Equipment> EquipmentServiceList = new List<Equipment>();
            EquipmentServiceList = (from DataRow dr in dt.Rows
                                    select new Equipment()
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        EquipmentId = (Guid)dr["EquipmentId"],
                                        CompanyId = (Guid)dr["CompanyId"],
                                        Name = dr["Name"].ToString(),
                                        SKU = dr["SKU"].ToString(),
                                        Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0.0,
                                        Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0.0,
                                        Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                        UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0.0,
                                        DiscountUnitPricce = dr["DiscountUnitPricce"] != DBNull.Value ? Convert.ToDouble(dr["DiscountUnitPricce"]) : 0.0,
                                        Total = dr["Total"] != DBNull.Value ? Convert.ToDouble(dr["Total"]) : 0.0,
                                        IsTransfered = dr["IsTransfered"] != DBNull.Value ? Convert.ToBoolean(dr["IsTransfered"]) : false,
                                        IsEqpExist = dr["IsEqpExist"] != DBNull.Value ? Convert.ToBoolean(dr["IsEqpExist"]) : false,
                                        TotalDiscountUnitPrice = dr["TotalDiscountUnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalDiscountUnitPrice"]) : 0.0,
                                        DiscountPercentage = dr["DiscountPercent"] != DBNull.Value ? Convert.ToDouble(dr["DiscountPercent"]) : 0.0,
                                        DiscountInCurrency = dr["DiscountInAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountInAmount"]) : 0.0,

                                    }).ToList();
            return EquipmentServiceList;
        }

        //public bool CheckEquipmentExistOrNot(Guid EquipmentId,Guid CompanyId) {
        //    var result = false;
        //    result = _EquipmentDataAccess.GetByQuery(string.Format("EquipmentId = '{0}' and CompanyId = '{1}'", EquipmentId, CompanyId)).FirstOrDefault().Id > 0;
        //    return result;
        //}
        public List<Equipment> GetAllEquipmentsByCompanyId(Guid CompanyId)
        {
            DataTable dt = _EquipmentDataAccess.GetAllEquipmentsByCompanyId(CompanyId);
            List<Equipment> EquipmentList = new List<Equipment>();
            EquipmentList = (from DataRow dr in dt.Rows
                             select new Equipment()
                             {
                                 EquipmentId = (Guid)dr["EquipmentId"],
                                 Name = dr["Name"].ToString(),
                                 EquipmentClass = dr["EquipmentClass"].ToString(),
                                 Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0.0
                             }).ToList();
            return EquipmentList;
        }

        public List<Equipment> GetAllEquipmentsForOptByCompanyId(Guid CompanyId, int id)
        {
            DataTable dt = _EquipmentDataAccess.GetAllEquipmentsForOptByCompanyId(CompanyId, id);
            List<Equipment> EquipmentList = new List<Equipment>();
            EquipmentList = (from DataRow dr in dt.Rows
                             select new Equipment()
                             {
                                 EquipmentId = (Guid)dr["EquipmentId"],
                                 Name = dr["Name"].ToString(),
                                 ManufacturerName = dr["ManufacturerName"].ToString(),
                                 EquipmentClass = dr["EquipmentClass"].ToString(),
                                 Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0.0
                             }).ToList();
            return EquipmentList;
        }

        public DataTable GetAllEquipmentsForOptByCompanyId(Guid CompanyId, int id, string query)
        {
            DataTable dt = _EquipmentDataAccess.GetAllEquipmentsForOptByCompanyId(CompanyId, id, query);
            return dt;
        }

        public DataTable GetAllEquipmentsForOptByCompanyId(Guid CompanyId, Guid id, string query, int equipmentClassId)
        {
            DataTable dt = _EquipmentDataAccess.GetAllEquipmentsForOptByCompanyId(CompanyId, id, query, equipmentClassId);
            return dt;
        }

        public DataTable GetAllTechEquipmentsForOptByCompanyId(Guid CompanyId, Guid id, string query, int equipmentClassId, Guid techid)
        {
            DataTable dt = _EquipmentDataAccess.GetAllTechEquipmentsForOptByCompanyId(CompanyId, id, query, equipmentClassId, techid);
            return dt;
        }

        public List<Equipment> GetAllEquipmentIdByCompanyId(Guid comid)
        {
            return _EquipmentDataAccess.GetAllEquipmentIdByCompanyId(comid);
        }
        public List<Equipment> GetAllPendingPurchaseOrderListByCompanyId(Guid CompanyId)
        {
            return _EquipmentDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and POOrder = 0", CompanyId)).ToList();
        }

        public void DeleteProduct(int id)
        {
            _EquipmentDataAccess.Delete(id);
        }

        public List<Equipment> GetAllLowsStockProductsByComapnyId(Guid value)
        {
            return _EquipmentDataAccess.GetByQuery(string.Format(""));
        }
        public List<ServiceEquipment> GetServiceEquipmentByServiceIdAndCompanyId(Guid CompanyId, Guid ServiceId)
        {
            return _ServiceEquipmentDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and ServiceId = '{1}'", CompanyId, ServiceId)).ToList();
        }
        public Equipment GetDefaultService(string ServiceName)
        {
            var query = string.Format("Name='{0}' and EquipmentClassId=2",ServiceName);
            return _EquipmentDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public List<Equipment> GetAllService()
        {
            var query = "EquipmentClassId=2";
            return _EquipmentDataAccess.GetByQuery(query);
        }
        public int InsertServiceEquipment(ServiceEquipment item)
        {
            return (int)_ServiceEquipmentDataAccess.Insert(item);
        }

        public bool DeleteServiceEquipmentsByServiceId(Guid equipmentId)
        {
            return _ServiceEquipmentDataAccess.DeleteServiceEquipmentsByServiceId(equipmentId);
        }

        public LeadReportServiceAndEquipmentModel GetAllSmartLeadServicesAndEquipmentsByCustomerId(Guid customerid)
        {
            DataSet ds = _EquipmentDataAccess.GetAllSmartLeadServicesAndEquipmentsByCustomerId(customerid);
            LeadReportServiceAndEquipmentModel model = new LeadReportServiceAndEquipmentModel();
            model.ListCustomerPackageService = (from DataRow dr in ds.Tables[0].Rows
                                                select new CustomerPackageService()
                                                {
                                                    EquipmentName = dr["EquipmentName"].ToString(),
                                                    MonthlyRate = dr["MonthlyRate"] != DBNull.Value ? Convert.ToDouble(dr["MonthlyRate"]) : 0.0,
                                                    DiscountRate = dr["DiscountRate"] != DBNull.Value ? Convert.ToDouble(dr["DiscountRate"]) : 0.0,
                                                    Total = dr["Total"] != DBNull.Value ? Convert.ToDouble(dr["Total"]) : 0.0
                                                }).ToList();
            model.ListCustomerPackageEqp = (from DataRow dr in ds.Tables[1].Rows
                                                select new CustomerPackageEqp()
                                                {
                                                    EquipmentName = dr["EquipmentName"].ToString(),
                                                    Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                                    UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0.0,
                                                    Total = dr["Total"] != DBNull.Value ? Convert.ToDouble(dr["Total"]) : 0.0
                                                }).ToList();
            model.Customer = (from DataRow dr in ds.Tables[2].Rows
                              select new Customer()
                              {
                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                  FirstName = dr["FirstName"].ToString(),
                                  LastName = dr["LastName"].ToString(),
                                  BusinessName = dr["BusinessName"].ToString()
                              }).FirstOrDefault();
            return model;
        }

        public List<Manufacturer> GetAllManufacturer()
        {
            return _ManufacturerDataAccess.GetAll();
        }

        public List<EquipmentType> GetAllEquipmentType()
        {
            return _EquipmentTypeDataAccess.GetAll();
        }

        public List<InventoryTech> GetInventoryTechByEquipmentId(Guid equipid)
        {
            DataTable dt = _EquipmentDataAccess.GetInventoryTechByEquipmentId(equipid);
            List<InventoryTech> EquipmentServiceList = new List<InventoryTech>();
            if (dt != null)
                EquipmentServiceList = (from DataRow dr in dt.Rows
                                        select new InventoryTech()
                                        {
                                            empName = dr["empName"].ToString(),
                                            TechnicianId = (Guid)dr["TechnicianId"],
                                            Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                            EquipmentId = (Guid)dr["EquipmentId"]
                                        }).ToList();
            return EquipmentServiceList;
        }

        public List<InventoryTech> GetInventoryLocByEquipmentId(Guid equipid)
        {
            DataTable dt = _EquipmentDataAccess.GetInventoryLocByEquipmentId(equipid);
            List<InventoryTech> EquipmentServiceList = new List<InventoryTech>();
            if (dt != null)
                EquipmentServiceList = (from DataRow dr in dt.Rows
                                        select new InventoryTech()
                                        {
                                            empName = dr["UserName"].ToString(),
                                            TechnicianId = (Guid)dr["LocationId"],
                                            Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                            EquipmentId = (Guid)dr["EquipmentId"]
                                        }).ToList();
            return EquipmentServiceList;
        }

        public List<Equipment> GetWarrentyEquipmentListByEquipmentId(Guid equipid)
        {
            return _EquipmentDataAccess.GetByQuery(string.Format("EquipmentId = '{0}' and IsWarrenty = 1", equipid)).ToList();
        }

        public long InsertEquipmentReturn(EquipmentReturn eqr)
        {
            return _EquipmentReturnDataAccess.Insert(eqr);
        }

        public EquipmentReturn GetEquipmentReturnByCustomerIdAndTechIdAndEquipmentId(Guid cusid, Guid techid, Guid eqpid)
        {
            return _EquipmentReturnDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and TechnicianId = '{1}' and EquipmentId = '{2}'", cusid, techid, eqpid)).FirstOrDefault();
        }

        public List<EquipmentVendor> GetAllEquipmentVendor()
        {
            return _EquipmentVendorDataAccess.GetAll();
        }

        public DataTable GetEquipmentListByCompanyIdTechnicianIdForReport(Guid TechnicianId,bool IsShowAll)
        {
            DataTable dt = _EquipmentDataAccess.GetEquipmentListByCompanyIdTechnicianIdForReport(TechnicianId, IsShowAll);
            return dt;
        }

        public DataTable GetClusterEquipmentListByCompanyIdTechnicianIdForReport(string TechnicianId)
        {
            DataTable dt = _EquipmentDataAccess.GetClusterEquipmentListByCompanyIdTechnicianIdForReport(TechnicianId);
            return dt;
        }
        #region Inventory Count Report
        public InventoryCount GetInventoryCountReport(DateTime? start, DateTime? end, int pageno, int pagesize, string searchtext,string order)
        {
            DataSet dsResult = _EquipmentDataAccess.GetInventoryCountReport(start, end, pageno, pagesize, searchtext, order);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            DataTable dt2 = dsResult.Tables[2];
            InventoryCount Model = new InventoryCount();
            List<InventoryCount> InventoryList = new List<InventoryCount>();
            Count TotalCount = new Count();
            TotalInventoryCount TotalInventoryCount = new TotalInventoryCount();
            if (dt != null)
                InventoryList = (from DataRow dr in dt1.Rows
                                            select new InventoryCount()
                                      {
                                          EquipmentId = (Guid)dr["EquipmentId"],
                                          Name = dr["Name"].ToString(),
                                          SKU = dr["SKU"].ToString(),
                                          QuantityOnStartDate = dr["QuantityOnStartDate"] != DBNull.Value ? Convert.ToInt32(dr["QuantityOnStartDate"]) : 0,
                                          QuantityOnEndDate = dr["QuantityOnEndDate"] != DBNull.Value ? Convert.ToInt32(dr["QuantityOnEndDate"]) : 0,
                                          Used = dr["Used"] != DBNull.Value ? Convert.ToInt32(dr["Used"]) : 0,
                                          Purchase = dr["Purchase"] != DBNull.Value ? Convert.ToInt32(dr["Purchase"]) : 0,
                                          RMA = dr["RMA"] != DBNull.Value ? Convert.ToInt32(dr["RMA"]) : 0
                                            }).ToList();

            TotalCount = (from DataRow dr in dt.Rows
                   select new Count()
                   {
                       TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                   }).FirstOrDefault();
            TotalInventoryCount = (from DataRow dr in dt2.Rows
                          select new TotalInventoryCount()
                          {
                              TotalOnHandStartDate = dr["TotalOnHandStartDate"] != DBNull.Value ? Convert.ToInt32(dr["TotalOnHandStartDate"]) : 0,
                              TotalOnHandEndDate = dr["TotalOnHandEndDate"] != DBNull.Value ? Convert.ToInt32(dr["TotalOnHandEndDate"]) : 0,
                              TotalPurchase = dr["TotalPurchase"] != DBNull.Value ? Convert.ToInt32(dr["TotalPurchase"]) : 0,
                              TotalRMA = dr["TotalRMA"] != DBNull.Value ? Convert.ToInt32(dr["TotalRMA"]) : 0,
                              TotalUsed = dr["TotalUsed"] != DBNull.Value ? Convert.ToInt32(dr["TotalUsed"]) : 0
                          }).FirstOrDefault();
            InventoryCount EquCount = new InventoryCount();
            EquCount.InventoryCountList = InventoryList;
            EquCount.Count = TotalCount;
            EquCount.TotalInventoryCount = TotalInventoryCount;
            return EquCount;
        }

        public EquipmentManufacturer GetEquipmentManufacturerByEquipmentIdAndManufacturerId(Guid equipmentId, Guid manufacturerId)
        {
            return _EquipmentManufacturerDataAccess.GetByQuery(string.Format("EquipmentId = '{0}' and ManufacturerId ='{1}'",equipmentId, manufacturerId)).FirstOrDefault();
        }
        public List<EquipmentManufacturer> GetEquipmentManufacturerListByEquipmentIdAndManufacturerId(Guid equipmentId, Guid manufacturerId)
        {
            return _EquipmentManufacturerDataAccess.GetByQuery(string.Format("EquipmentId = '{0}' and ManufacturerId ='{1}'", equipmentId, manufacturerId));
        }
        
        public InventoryCount GetInventoryCountOnStartDate(DateTime? start, Guid Id)
        {
            DataSet dsResult = _EquipmentDataAccess.GetInventoryCountOnStartDate(start, Id);
            DataTable dt = dsResult.Tables[0];
            InventoryCount Model = new InventoryCount();
            List<InventoryCount> InventoryList = new List<InventoryCount>();
            Count TotalCount = new Count();

            InventoryList = (from DataRow dr in dt.Rows
                             select new InventoryCount()
                             {
                                 Name = dr["Name"].ToString(),
                                 QuantityOnStartDate = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0
                             }).ToList();
            InventoryCount EquCount = new InventoryCount();
            EquCount.InventoryCountList = InventoryList;
            return EquCount;
        }
        public InventoryCount GetInventoryCountOnEndDate(DateTime? end, Guid Id)
        {
            DataSet dsResult = _EquipmentDataAccess.GetInventoryCountOnEndDate(end, Id);
            DataTable dt = dsResult.Tables[0];
            InventoryCount Model = new InventoryCount();
            List<InventoryCount> InventoryList = new List<InventoryCount>();
            Count TotalCount = new Count();

            InventoryList = (from DataRow dr in dt.Rows
                             select new InventoryCount()
                             {
                                 Name = dr["Name"].ToString(),
                                 QuantityOnEndDate = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0
                             }).ToList();
            InventoryCount EquCount = new InventoryCount();
            EquCount.InventoryCountList = InventoryList;
            return EquCount;
        }
        public UsedInventoryCount UsedInventoryCountDetails(Guid Id, DateTime? start, DateTime? end)
        {
            DataSet dsResult = _EquipmentDataAccess.UsedInventoryCountDetails(Id, start, end);
            DataTable dt = dsResult.Tables[0];
            UsedInventoryCount Model = new UsedInventoryCount();
            List<UsedInventoryCount> UsedInventoryList = new List<UsedInventoryCount>();
            if(dt != null)
            UsedInventoryList = (from DataRow dr in dt.Rows
                             select new UsedInventoryCount()
                             {
                                 Name = dr["Name"].ToString(),
                                 CustomerId = dr["CustomerId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerId"]) : 0,
                                 TicketId = dr["TicketId"] != DBNull.Value ? Convert.ToInt32(dr["TicketId"]) : 0,
                                 Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0

                             }).ToList();
            UsedInventoryCount EquCount = new UsedInventoryCount();
            EquCount.UsedInventoryCountList = UsedInventoryList;
            return EquCount;
        }
        public PurchaseInventoryCount PurchaseInventoryCountDetails(Guid Id, DateTime? start, DateTime? end)
        {
            DataSet dsResult = _EquipmentDataAccess.PurchaseInventoryCountDetails(Id, start, end);
            DataTable dt = dsResult.Tables[0];
            PurchaseInventoryCount Model = new PurchaseInventoryCount();
            List<PurchaseInventoryCount> PurchaseInventoryCountList = new List<PurchaseInventoryCount>();

            PurchaseInventoryCountList = (from DataRow dr in dt.Rows
                                          select new PurchaseInventoryCount()
                                 {
                                     Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                     PurchaseOrderId = dr["PurchaseOrderId"].ToString(),
                                     CreatedByUser = dr["CreatedBy"].ToString(),
                                     Date = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                     Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0

                                          }).ToList();
            PurchaseInventoryCount EquCount = new PurchaseInventoryCount();
            EquCount.PurchaseInventoryCountList = PurchaseInventoryCountList;
            return EquCount;
        }
        public RMADetails RMADetailsOfInventoryCount(Guid Id, DateTime? start, DateTime? end)
        {
            DataSet dsResult = _EquipmentDataAccess.RMADetailsOfInventoryCount(Id, start, end);
            DataTable dt = dsResult.Tables[0];
            RMADetails Model = new RMADetails();
            List<RMADetails> RMADetailsList = new List<RMADetails>();
            if (dt != null)
                RMADetailsList = (from DataRow dr in dt.Rows
                                     select new RMADetails()
                                     {
                                         Name = dr["Name"].ToString(),
                                         CustomerId = dr["CustomerId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerId"]) : 0,
                                         Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0

                                     }).ToList();
            RMADetails RMA = new RMADetails();
            RMA.RMADetailsList = RMADetailsList;
            return RMA;
        }
        public DataTable GetInventoryCountReportForDownload(DateTime? start, DateTime? end, string searchtext)
        {
            DataTable dt = _EquipmentDataAccess.GetInventoryCountReportForDownload(start,end,searchtext);
            return dt;
        }

        public Equipment GetEquipmentBySKU(string SKU)
        {
            return _EquipmentDataAccess.GetByQuery(string.Format(" SKU = '{0}'",SKU)).FirstOrDefault();
        }

        public void DeleteEquipmentVendorByEquipmentId(Guid equipmentId)
        {
            _EquipmentVendorDataAccess.DeleteEquipmentVendorByEquipmentId(equipmentId);
        }
        #endregion

        public EquipmentVendor GetEquipmentVendorByEquipmentIdAndSupplierId(Guid EquipmentId, Guid SupplierId)
        {
            var query = "EquipmentId='" + EquipmentId + "' And SupplierId='" + SupplierId + "'";
            return _EquipmentVendorDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public EquipmentVendor GetEquipmentVendorByEquipmentIdAndIsPrimary(Guid EquipmentId)
        {
            return _EquipmentVendorDataAccess.GetByQuery(string.Format("EquipmentId = '{0}' and IsPrimary =1", EquipmentId)).FirstOrDefault();
        }
        public Manufacturer GetManufacturerByManufacturerId(Guid manufacturerId)
        {
            return _ManufacturerDataAccess.GetByQuery(string.Format("ManufacturerId ='{0}'",manufacturerId)).FirstOrDefault();
        }
    }
}
