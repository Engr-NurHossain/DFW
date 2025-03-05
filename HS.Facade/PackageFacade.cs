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
    public class PackageFacade : BaseFacade
    {
        public PackageFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        public PackageFacade()
        {

        }

        PackageSystemDataAccess _PackageSystemDataAccess
        {
            get
            {
                return (PackageSystemDataAccess)_ClientContext[typeof(PackageSystemDataAccess)];
            }
        }
        MMRRangeDataAccess _MMRRangeDataAccess
        {
            get
            {
                return (MMRRangeDataAccess)_ClientContext[typeof(MMRRangeDataAccess)];
            }
        }
        PackageSystemCustomerDataAccess _PackageSystemCustomerDataAccess
        {
            get
            {
                return (PackageSystemCustomerDataAccess)_ClientContext[typeof(PackageSystemCustomerDataAccess)];
            }
        }
        PackageDeviceDataAccess _PackageDeviceDataAccess
        {
            get
            {
                return (PackageDeviceDataAccess)_ClientContext[typeof(PackageDeviceDataAccess)];
            }
        }
        PackageIncludeDataAccess _PackageIncludeDataAccess
        {
            get
            {
                return (PackageIncludeDataAccess)_ClientContext[typeof(PackageIncludeDataAccess)];
            }
        }
        PackageOptionalDataAccess _PackageOptionalDataAccess
        {
            get
            {
                return (PackageOptionalDataAccess)_ClientContext[typeof(PackageOptionalDataAccess)];
            }
        }
        PackageDataAccess _PackageDataAccess
        {
            get
            {
                return (PackageDataAccess)_ClientContext[typeof(PackageDataAccess)];
            }
        }
        PackageCustomerDataAccess _PackageCustomerDataAccess
        {
            get
            {
                return (PackageCustomerDataAccess)_ClientContext[typeof(PackageCustomerDataAccess)];
            }
        }
        PackageDetailCustomerDataAccess _PackageDetailCustomerDataAccess
        {
            get
            {
                return (PackageDetailCustomerDataAccess)_ClientContext[typeof(PackageDetailCustomerDataAccess)];
            }
        }
        PackageSystemInstallTypeDataAccess _PackageSystemInstallTypeDataAccess
        {
            get
            {
                return (PackageSystemInstallTypeDataAccess)_ClientContext[typeof(PackageSystemInstallTypeDataAccess)];
            }
        }
        CustomerPackageEqpDataAccess _CustomerPackageEqpDataAccess
        {
            get
            {
                return (CustomerPackageEqpDataAccess)_ClientContext[typeof(CustomerPackageEqpDataAccess)];
            }
        }
        CustomerPackageServiceDataAccess _CustomerPackageServiceDataAccess
        {
            get
            {
                return (CustomerPackageServiceDataAccess)_ClientContext[typeof(CustomerPackageServiceDataAccess)];
            }
        }
        SmartSystemTypeDataAccess _SmartSystemTypeDataAccess
        {
            get
            {
                return (SmartSystemTypeDataAccess)_ClientContext[typeof(SmartSystemTypeDataAccess)];
            }
        }
        SmartPackageDataAccess _SmartPackageDataAccess
        {
            get
            {
                return (SmartPackageDataAccess)_ClientContext[typeof(SmartPackageDataAccess)];
            }
        }
        SmartPackageEquipmentServiceDataAccess _SmartPackageEquipmentServiceDataAccess
        {
            get
            {
                return (SmartPackageEquipmentServiceDataAccess)_ClientContext[typeof(SmartPackageEquipmentServiceDataAccess)];
            }
        }
        SmartPackageSystemInstallTypeMapDataAccess _SmartPackageSystemInstallTypeMapDataAccess
        {
            get
            {
                return (SmartPackageSystemInstallTypeMapDataAccess)_ClientContext[typeof(SmartPackageSystemInstallTypeMapDataAccess)];
            }
        }
        SmartInstallTypeDataAccess _SmartInstallTypeDataAccess
        {
            get
            {
                return (SmartInstallTypeDataAccess)_ClientContext[typeof(SmartInstallTypeDataAccess)];
            }
        }
        SystemTypeManufacturerMapDataAccess _SystemTypeManufacturerMapDataAccess
        {
            get
            {
                return (SystemTypeManufacturerMapDataAccess)_ClientContext[typeof(SystemTypeManufacturerMapDataAccess)];
            }
        }
        public List<PackageSystem> GetAllPackageSystemListByCompanyId(Guid CompanyId)
        {
            return _PackageSystemDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", CompanyId)).ToList();
        }
        public List<Package> GetAllPackageListByCompanyId(Guid CompanyId)
        {
            var PackageList= _PackageDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", CompanyId)).ToList();
            return PackageList;
        }
        public List<Package> GetAllPackageListByCompanyIdAndInstallType(Guid CompanyId, string installType)
        {
            return _PackageDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and InstallType='{1}'", CompanyId, installType)).ToList();
        }
        public List<SmartSystemType> GetAllSmartSystemType(Guid CompanyId)
        {
            return _SmartSystemTypeDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", CompanyId));
        }
        public List<SmartInstallType> GetInstallTypeByCompanyIdAndSystemId(Guid CompanyId, int SystemId)
        {
            DataTable dt = _PackageSystemInstallTypeDataAccess.GetAllInstallTypeBySystemId(CompanyId, SystemId);
            List<SmartInstallType> InstallTypeList = new List<SmartInstallType>();
            InstallTypeList = (from DataRow dr in dt.Rows
                                          select new SmartInstallType()
                                          {
                                              Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                              Name = dr["Name"].ToString(),
                                              SystemId= dr["SystemId"] != DBNull.Value ? Convert.ToInt32(dr["SystemId"]) : 0,
                                          }).ToList();
            return InstallTypeList;
        }
        public List<Manufacturer> GetManufacturerBySystemId(int SystemId)
        {
            DataTable dt = _SystemTypeManufacturerMapDataAccess.GetAllManufacturerBySystemId(SystemId);
            List<Manufacturer> ManufacturerList = new List<Manufacturer>();
            ManufacturerList = (from DataRow dr in dt.Rows
                               select new Manufacturer()
                               {
                                   Name = dr["Name"].ToString(),
                                   ManufacturerId = dr["ManufacturerId"] != DBNull.Value ? (Guid)(dr["ManufacturerId"]) : Guid.Empty,
                               }).ToList();
            return ManufacturerList;
        }
        public string SmartInstallTypeNameByInstallTypeId(int InstallTypeId)
        {
            string InstallTypeName = "";
            var InstallDetail = _SmartInstallTypeDataAccess.Get(InstallTypeId);
            if(InstallDetail!=null)
            {
                InstallTypeName = InstallDetail.Name;
            }
            return InstallTypeName;
        }
        public List<SmartPackage> GetAllPackageListByCompanyIdSystemIdAndInstallTypeId(Guid CompanyId, int SystemId,int InstallTypeId,Guid? ManufacturerId)
        {
            DataTable dt = _SmartPackageSystemInstallTypeMapDataAccess.GetAllPackageByCompanySystemInstall(CompanyId, SystemId, InstallTypeId, ManufacturerId);
            List<SmartPackage> PackageTypeList = new List<SmartPackage>();
            PackageTypeList = (from DataRow dr in dt.Rows
                               select new SmartPackage()
                               {
                                   PackageId = dr["PackageId"] != DBNull.Value ? (Guid)(dr["PackageId"]) : new Guid(),
                                   PackageName = dr["PackageName"].ToString(),
                                   ActivationFee = dr["ActivationFee"] != DBNull.Value ? Convert.ToDouble(dr["ActivationFee"]) : 0.0,
                                   UserType = dr["UserType"].ToString(),
                                   PackageCode = dr["PackageCode"].ToString(),
                                   MinCredit = dr["MinCredit"] != DBNull.Value ? Convert.ToDouble(dr["MinCredit"]) : 0.0
                               }).ToList();
            return PackageTypeList;
        }
        public List<SmartPackageEquipmentService> GetAllSmartPackageEquipmentServiceByPackageIdAndCompanyId(Guid PackageId, Guid CompanyId)
        {
            DataTable dt = _SmartPackageEquipmentServiceDataAccess.GetAllSmartPackageEquipmentServiceByPackageIdAndCompanyId(PackageId, CompanyId);
            List<SmartPackageEquipmentService> PackageDeviceEquipmentList = new List<SmartPackageEquipmentService>();
            PackageDeviceEquipmentList = (from DataRow dr in dt.Rows
                                          select new SmartPackageEquipmentService()
                                          {
                                              Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                              PackageId = dr["PackageId"] != DBNull.Value ? (Guid)(dr["PackageId"]) : new Guid(),
                                              IsFree = dr["IsFree"] != DBNull.Value ? Convert.ToBoolean(dr["IsFree"]) : false,
                                              EptNo = dr["EptNo"] != DBNull.Value ? Convert.ToInt32(dr["EptNo"]) : 0,
                                              Type = dr["Type"].ToString(),
                                              EquipmentName = dr["EquipmentName"].ToString(),
                                              EquipmentId = dr["EquipmentId"] != DBNull.Value ? (Guid)(dr["EquipmentId"]) : new Guid(),
                                              SmartPackageEquipmentServiceId = dr["SmartPackageEquipmentServiceId"] != DBNull.Value ? (Guid)(dr["SmartPackageEquipmentServiceId"]) : new Guid(),
                                              Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0,
                                              PackageName = dr["PackageName"].ToString(),
                                              EquipmentMaxLimit = dr["EquipmentMaxLimit"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentMaxLimit"]) : 0,
                                              Price = dr["Price"] != DBNull.Value ? Convert.ToDouble(dr["Price"]) : 0.0
                                          }).ToList();
            return PackageDeviceEquipmentList;
        }
        public List<SmartPackageEquipmentService> GetAllSmartPackageIncludeEquipmentByPackageIdAndCompanyId(Guid PackageId, Guid CompanyId)
        {
            DataTable dt = _SmartPackageEquipmentServiceDataAccess.GetAllSmartPackageIncludeEquipmentByPackageIdAndCompanyId(PackageId, CompanyId);
            List<SmartPackageEquipmentService> PackageDeviceEquipmentList = new List<SmartPackageEquipmentService>();
            PackageDeviceEquipmentList = (from DataRow dr in dt.Rows
                                          select new SmartPackageEquipmentService()
                                          {
                                              PackageId = dr["PackageId"] != DBNull.Value ? (Guid)(dr["PackageId"]) : new Guid(),
                                              IsFree = dr["IsFree"] != DBNull.Value ? Convert.ToBoolean(dr["IsFree"]) : false,
                                              EptNo = dr["EptNo"] != DBNull.Value ? Convert.ToInt32(dr["EptNo"]) : 0,
                                              Type = dr["Type"].ToString(),
                                              EquipmentName = dr["EquipmentName"].ToString(),
                                              EquipmentId = dr["EquipmentId"] != DBNull.Value ? (Guid)(dr["EquipmentId"]) : new Guid(),
                                              Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0,
                                              PackageName = dr["PackageName"].ToString(),
                                              EquipmentMaxLimit = dr["EquipmentMaxLimit"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentMaxLimit"]) : 0,
                                              Price = dr["Price"] != DBNull.Value ? Convert.ToDouble(dr["Price"]) : 0.0
                                          }).ToList();
            return PackageDeviceEquipmentList;
        }
        public List<LeadPackageEuipment> GetAllPackageDeviceEquipmentListByPackageIdAndCompanyIdAndCustomerId(int PackageId, Guid CompanyId, Guid CustomerId)
        {
            DataTable dt = _PackageDataAccess.GetAllPackageDeviceEquipmentListByPackageIdAndCompanyIdAndCustomerId(PackageId, CompanyId, CustomerId);
            List<LeadPackageEuipment> PackageDeviceEquipmentList = new List<LeadPackageEuipment>();
            PackageDeviceEquipmentList = (from DataRow dr in dt.Rows
                                          select new LeadPackageEuipment()
                                          {
                                              PackageId = dr["PackageId"] != DBNull.Value ? Convert.ToInt32(dr["PackageId"]) : 0,
                                              EquipmentName = dr["EquipmentName"].ToString(),
                                              PackageName = dr["PackageName"].ToString(),
                                              PackageEquipmentid = dr["PackageEquipmentid"] != DBNull.Value ? (Guid)(dr["PackageEquipmentid"]) : new Guid(),
                                              PackageEquipmentType = dr["PackageEquipmentType"].ToString(),
                                              EquipmentCost = dr["EquipmentCost"] != DBNull.Value ? Convert.ToDouble(dr["EquipmentCost"]) : 0,
                                              EquipmentIsFreeFlag = dr["EquipmentIsFreeFlag"] != DBNull.Value ? Convert.ToBoolean(dr["EquipmentIsFreeFlag"]) : false,
                                              PackageEqpId = dr["PackageEqpId"] != DBNull.Value ? Convert.ToInt32(dr["PackageEqpId"]) : 0,
                                              IsSelected = dr["IsSelected"] != DBNull.Value ? Convert.ToInt32(dr["IsSelected"]) : 0,
                                              NumOfEquipment = dr["NumOfEquipment"] != DBNull.Value ? Convert.ToInt32(dr["NumOfEquipment"]) : 0
                                          }).ToList();
            return PackageDeviceEquipmentList;
        }
        public List<LeadPackageEuipment> GetAllPackageIncludeEquipmentListByPackageIdAndCompanyId(int PackageId, Guid CompanyId)
        {
            DataTable dt = _PackageDataAccess.GetAllPackageIncludeEquipmentListByPackageIdAndCompanyId(PackageId, CompanyId);
            List<LeadPackageEuipment> PackageDeviceEquipmentList = new List<LeadPackageEuipment>();
            PackageDeviceEquipmentList = (from DataRow dr in dt.Rows
                                          select new LeadPackageEuipment()
                                          {
                                              PackageId = dr["PackageId"] != DBNull.Value ? Convert.ToInt32(dr["PackageId"]) : 0,
                                              EquipmentName = dr["EquipmentName"].ToString(),
                                              PackageName = dr["PackageName"].ToString(),
                                              PackageEquipmentid = dr["PackageEquipmentid"] != DBNull.Value ? (Guid)(dr["PackageEquipmentid"]) : new Guid(),
                                              PackageEquipmentType = dr["PackageEquipmentType"].ToString(),
                                              EquipmentCost = dr["EquipmentCost"] != DBNull.Value ? Convert.ToDouble(dr["EquipmentCost"]) : 0,
                                              PackageEqpId = dr["PackageEqpId"] != DBNull.Value ? Convert.ToInt32(dr["PackageEqpId"]) : 0,
                                              NumOfEquipment = dr["NumOfEquipment"] != DBNull.Value ? Convert.ToInt32(dr["NumOfEquipment"]) : 0,
                                              OrderBy = dr["OrderBy"] != DBNull.Value ? Convert.ToInt32(dr["OrderBy"]) : 0,
                                          }).ToList();
            return PackageDeviceEquipmentList;
        }
        public List<LeadPackageEuipment> GetAllPackageOptionalEquipmentListByPackageIdAndCompanyIdAndCustomerId(int PackageId, Guid CompanyId, Guid CustomerId)
        {
            DataTable dt = _PackageDataAccess.GetAllPackageOptionalEquipmentListByPackageIdAndCompanyIdAndCustomerId(PackageId, CompanyId, CustomerId);
            List<LeadPackageEuipment> PackageDeviceEquipmentList = new List<LeadPackageEuipment>();
            PackageDeviceEquipmentList = (from DataRow dr in dt.Rows
                                          select new LeadPackageEuipment()
                                          {
                                              PackageId = dr["PackageId"] != DBNull.Value ? Convert.ToInt32(dr["PackageId"]) : 0,
                                              EquipmentName = dr["EquipmentName"].ToString(),
                                              PackageName = dr["PackageName"].ToString(),
                                              PackageEquipmentid = dr["PackageEquipmentid"] != DBNull.Value ? (Guid)(dr["PackageEquipmentid"]) : new Guid(),
                                              PackageEquipmentType = dr["PackageEquipmentType"].ToString(),
                                              EquipmentCost = dr["EquipmentCost"] != DBNull.Value ? Convert.ToDouble(dr["EquipmentCost"]) : 0.0,
                                              EquipmentIsFreeFlag = dr["EquipmentIsFreeFlag"] != DBNull.Value ? Convert.ToBoolean(dr["EquipmentIsFreeFlag"]) : false,
                                              PackageEqpId = dr["PackageEqpId"] != DBNull.Value ? Convert.ToInt32(dr["PackageEqpId"]) : 0,
                                              IsSelected = dr["IsSelected"] != DBNull.Value ? Convert.ToInt32(dr["IsSelected"]) : 0,
                                              NumofOptional = dr["NumofOptional"] != DBNull.Value ? Convert.ToInt32(dr["NumofOptional"]) : 0
                                          }).ToList();
            return PackageDeviceEquipmentList;
        }
  
        public CustomerPackageService GetCustomerPackageServiceById(int id)
        {
            DataTable dt = _CustomerPackageServiceDataAccess.GetCustomerPackageServiceById(id);

            List<CustomerPackageService> viewList = new List<CustomerPackageService>();
            viewList = (from DataRow dr in dt.Rows
                        select new CustomerPackageService()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            CompanyId = dr["CompanyId"] != DBNull.Value ? (Guid)(dr["CompanyId"]) : new Guid(),
                            CustomerId = dr["CustomerId"] != DBNull.Value ? (Guid)(dr["CustomerId"]) : new Guid(),
                            PackageId = dr["PackageId"] != DBNull.Value ? (Guid)(dr["PackageId"]) : new Guid(),
                            EquipmentId = dr["EquipmentId"] != DBNull.Value ? (Guid)(dr["EquipmentId"]) : new Guid(),
                            UnitPrice = dr["MonthlyRate"] != DBNull.Value ? Convert.ToDouble(dr["MonthlyRate"]) : 0.0,
                            DiscountUnitPricce = dr["DiscountRate"] != DBNull.Value ? Convert.ToDouble(dr["DiscountRate"]) : 0.0,
                            Total = dr["Total"] != DBNull.Value ? Convert.ToDouble(dr["Total"]) : 0.0,
                            EquipmentServiceName = dr["EquipmentServiceName"].ToString(),
                            ManufacturerId = dr["ManufacturerId"] != DBNull.Value ? (Guid)(dr["ManufacturerId"]) : Guid.Empty, 
                            Manufacturer = dr["Manufacturer"].ToString(),
                            Location = dr["Location"].ToString(),
                            Type = dr["Type"].ToString(),
                            Model = dr["Model"].ToString(),
                            Finish = dr["Finish"].ToString(),
                            Capacity = dr["Capacity"].ToString(),
                        }).ToList();
            return viewList.FirstOrDefault();
        }

        public long InsertPackageSystemCustomer(PackageSystemCustomer DBPackageSystemCustomer)
        {
            return _PackageSystemCustomerDataAccess.Insert(DBPackageSystemCustomer);
        }
        public int GetPackageOptionEqpMaxLimitByPackageIdAndCompanyId(int PackageId, Guid CompanyId)
        {
            int result = 0;
            var EqpMaxLimitObj = _PackageDataAccess.GetByQuery(string.Format("Id='{0}' and CompanyId = '{1}'", PackageId, CompanyId)).FirstOrDefault();
            if (EqpMaxLimitObj != null)
            {
                result = _PackageDataAccess.GetByQuery(string.Format("Id='{0}' and CompanyId = '{1}'", PackageId, CompanyId)).FirstOrDefault().OptionEqpMaxLimit.Value;
            }
            return result;
        }
        public int GetPackageOptionEqpMaxLimitBySmartPackageIdAndCompanyId(Guid PackageId, Guid CompanyId)
        {
            int result = 0;
            var EqpMaxLimitObj = _SmartPackageDataAccess.GetByQuery(string.Format("PackageId='{0}' and CompanyId = '{1}'", PackageId, CompanyId)).FirstOrDefault();
            if (EqpMaxLimitObj != null)
            {
                result = EqpMaxLimitObj.EquipmentMaxLimit;
            }
            return result;
        }
        public PackageSystemCustomer GetPackageSystemCustomerByCustomerIdandCompanyId(Guid CustomerId, Guid CompanyId)
        {
            return _PackageSystemCustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}'", CustomerId, CompanyId)).FirstOrDefault();
        }
        public long InsertCustomerPackageEqp(CustomerPackageEqp _CustomerPackageEqp)
        {
            return _CustomerPackageEqpDataAccess.Insert(_CustomerPackageEqp);
        }
        public int InsertCustomerPackageService(CustomerPackageService _CustomerPackageService)
        {
            return (int) _CustomerPackageServiceDataAccess.Insert(_CustomerPackageService);
        }
        public List<CustomerPackageEqp> GetCustomerPackageEqpListbyCustomerId(Guid CompanyId, Guid CustomerId)
        {
            string Query = "CompanyId='" + CompanyId + "'and CustomerId='" + CustomerId + "'";
            return _CustomerPackageEqpDataAccess.GetByQuery(Query);
        }
        public bool DeleteCustomerPackageEqpByCompanyIdCustomerId(Guid CompanyId,Guid CustomerId)
        {
            var result = _CustomerPackageEqpDataAccess.DeleteCustomerPackageEqpByCompanyIdCustomerId(CompanyId,CustomerId);
            return result;
        }
        public bool DeleteAllCustomerPackageEqpByCompanyIdCustomerId(Guid CompanyId, Guid CustomerId)
        {
            var result = _CustomerPackageEqpDataAccess.DeleteAllCustomerPackageEqpByCompanyIdCustomerId(CompanyId, CustomerId);
            return result;
        }
        public bool DeleteCustomerPackageServiceByCompanyIdCustomerId(Guid CompanyId, Guid CustomerId)
        {
            var result = _CustomerPackageServiceDataAccess.DeleteCustomerPackageServiceByCompanyIdCustomerId(CompanyId, CustomerId);
            return result;
        }
        #region Only Package Related EqpService
        public bool DeleteOnlyCustomerPackageEqpByCompanyIdCustomerId(Guid CompanyId, Guid CustomerId)
        {
            var result = _CustomerPackageEqpDataAccess.DeleteOnlyCustomerPackageEqpByCompanyIdCustomerId(CompanyId, CustomerId);
            return result;
        }
        public bool DeleteOnlyCustomerPackageServiceByCompanyIdCustomerId(Guid CompanyId, Guid CustomerId)
        {
            var result = _CustomerPackageServiceDataAccess.DeleteOnlyCustomerPackageServiceByCompanyIdCustomerId(CompanyId, CustomerId);
            return result;
        }
        #endregion
        public long InsertPackageCustomer(PackageCustomer _PackageCustomer)
        {
            return _PackageCustomerDataAccess.Insert(_PackageCustomer);
        }
        public long InsertPackageDetailCustomer(PackageDetailCustomer _PackageDetailCustomer)
        {
            return _PackageDetailCustomerDataAccess.Insert(_PackageDetailCustomer);
        }
        public bool UpdatePackageSystemCustomer(PackageSystemCustomer _PackageSystemCustomer)
        {
            return _PackageSystemCustomerDataAccess.Update(_PackageSystemCustomer) > 0;
        }
        public PackageCustomer GetPackageCustomerByCustomerIdandCompanyId(Guid CustomerId, Guid CompanyId)
        {
            return _PackageCustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}'", CustomerId, CompanyId)).FirstOrDefault();
        }
        public bool UpdatePackageCustomer(PackageCustomer _PackageCustomer)
        {
            return _PackageCustomerDataAccess.Update(_PackageCustomer) > 0;
        }
        public List<PackageDetailCustomer> GetAllPackageDetailCustomerByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId)
        {
            return _PackageDetailCustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId ='{1}'", CustomerId, CompanyId)).ToList();
        }
        public bool DeleteAllPackageDetailCustomerByCustomerIdAndComapnyId(Guid CustomerId, Guid CompanyId)
        {
            var result = _PackageDetailCustomerDataAccess.DeleteAllPackageDetailCustomerByCustomerIdAndComapnyId(CustomerId, CompanyId);
            return result;
        }
        public bool DeletePackageDetailCustomerById(int Id)
        {
            return _PackageDetailCustomerDataAccess.Delete(Id) > 0;
        }
        public bool DeleteCustomerPackageEqpById(int Id)
        {
            return _CustomerPackageEqpDataAccess.Delete(Id) > 0;
        }
        public CustomerPackageEqp GetCustomerPackageEqpById(int Id)
        {
            return _CustomerPackageEqpDataAccess.Get(Id);
        }
        public bool DeleteCustomerPackageServiceById(int Id)
        {
            return _CustomerPackageServiceDataAccess.Delete(Id) > 0;
        }
        public List<Package> GetAllPackageByCompanyId(Guid CompanyId)
        {
            DataTable dt = _PackageDataAccess.GetAllPackageByCompanyId(CompanyId);
            List<Package> PackageList = new List<Package>();
            PackageList = (from DataRow dr in dt.Rows
                           select new Package()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               Name = dr["Name"].ToString(),
                               OptionEqpMaxLimit = dr["OptionEqpMaxLimit"] != DBNull.Value ? Convert.ToInt32(dr["OptionEqpMaxLimit"]) : 0,
                               CompanyId = (Guid)dr["CompanyId"],
                               PackageId = (Guid)dr["PackageId"],
                               MMRMax = dr["MMRMax"] != DBNull.Value ? Convert.ToDouble(dr["MMRMax"]) : 0.0,
                               MMRMin = dr["MMRMin"] != DBNull.Value ? Convert.ToDouble(dr["MMRMin"]) : 0.0,
                               Rate = dr["Rate"] != DBNull.Value ? Convert.ToDouble(dr["Rate"]) : 0.0,
                           }).ToList();
            return PackageList;
        }
        public Package GetPackageByIdAndCompanyId(int Id, Guid CompanyId)
        {
            return _PackageDataAccess.GetByQuery(string.Format("Id = '{0}' and CompanyId = '{1}'", Id, CompanyId)).FirstOrDefault();
        }
        public Package GetPackageByPackageIdAndCompanyId(Guid PackageId, Guid CompanyId)
        {
            return _PackageDataAccess.GetByQuery(string.Format("PackageId = '{0}' and CompanyId = '{1}'", PackageId, CompanyId)).FirstOrDefault();
        }
        public SmartPackage GetSmartPackageByIdAndCompanyId(Guid PackageId, Guid CompanyId)
        {
            return _SmartPackageDataAccess.GetByQuery(string.Format("PackageId = '{0}' and CompanyId = '{1}'", PackageId, CompanyId)).FirstOrDefault();
        }
        public PackageInclude GetPackageIncludeByIdAndCompanyId(int Id, Guid CompanyId)
        {
            return _PackageIncludeDataAccess.GetByQuery(string.Format("Id = '{0}' and CompanyId = '{1}'", Id, CompanyId)).FirstOrDefault();
        }
        public PackageDevice GetPackageDeviceByIdAndCompanyId(int Id, Guid CompanyId)
        {
            return _PackageDeviceDataAccess.GetByQuery(string.Format("Id = '{0}' and CompanyId = '{1}'", Id, CompanyId)).FirstOrDefault();
        }

        public PackageDevice GetPackageDeviceByPackageIdAndCompanyIdAndEquipmentId(int PackageId, Guid CompanyId, Guid EquipmentId)
        {
            return _PackageDeviceDataAccess.GetByQuery(string.Format("PackageId = '{0}' and CompanyId = '{1}' and EquipmentId='{2}'", PackageId, CompanyId, EquipmentId)).FirstOrDefault();
        }
        public PackageOptional GetPackageOptionalByPackageIdAndCompanyIdAndEquipmentId(int PackageId, Guid CompanyId, Guid EquipmentId)
        {
            return _PackageOptionalDataAccess.GetByQuery(string.Format("PackageId = '{0}' and CompanyId = '{1}' and EquipmentId='{2}'", PackageId, CompanyId, EquipmentId)).FirstOrDefault();
        }
        public PackageInclude GetPackageIncludeByPackageIdAndCompanyIdAndEquipmentId(int PackageId, Guid CompanyId, Guid EquipmentId)
        {
            return _PackageIncludeDataAccess.GetByQuery(string.Format("PackageId = '{0}' and CompanyId = '{1}' and EquipmentId='{2}'", PackageId, CompanyId, EquipmentId)).FirstOrDefault();
        }
        public PackageOptional GetPackageOptionalByIdAndCompanyId(int Id, Guid CompanyId)
        {
            return _PackageOptionalDataAccess.GetByQuery(string.Format("Id = '{0}' and CompanyId = '{1}'", Id, CompanyId)).FirstOrDefault();
        }
        public long InsertPackage(Package _Package)
        {
            return _PackageDataAccess.Insert(_Package);
        }
        public long InsertSmartPackage(SmartPackage _Package)
        {
            return _SmartPackageDataAccess.Insert(_Package);
        }
        public bool UpdatePackage(Package _Package)
        {
            return _PackageDataAccess.Update(_Package) > 0;
        }
        public bool DeletePackage(int Id)
        {
            return _PackageDataAccess.Delete(Id) > 0;
        }
        public List<PackageInclude> GetAllPackageIncludeProducts(Guid CompanyId)
        {
            DataTable dt = _PackageDataAccess.GetAllPackageIncludeProductsByCompanyId(CompanyId);

            List<PackageInclude> PackageDeviceEquipmentList = new List<PackageInclude>();
            PackageDeviceEquipmentList = (from DataRow dr in dt.Rows
                                          select new PackageInclude()
                                          {
                                              Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                              CompanyId = dr["CompanyId"] != DBNull.Value ? (Guid)(dr["CompanyId"]) : new Guid(),
                                              PackageId = dr["PackageId"] != DBNull.Value ? Convert.ToInt32(dr["PackageId"]) : 0,
                                              EquipmentId = dr["EquipmentId"] != DBNull.Value ? (Guid)(dr["EquipmentId"]) : new Guid(),
                                              IsFree = dr["IsFree"] != DBNull.Value ? Convert.ToBoolean(dr["IsFree"]) : false,
                                              IncludedEquipmentName = dr["IncludedEquipmentName"].ToString(),
                                              IncludedPackageName = dr["IncludedPackageName"].ToString(),
                                              EptNo = dr["EptNo"] != DBNull.Value ? Convert.ToInt32(dr["EptNo"]) : 0,
                                          }).ToList();
            return PackageDeviceEquipmentList;

        }
        public List<PackageDevice> GetAllPackageDeviceProducts(Guid CompanyId)
        {
            DataTable dt = _PackageDataAccess.GetAllPackageDeviceProductsByCompanyId(CompanyId);

            List<PackageDevice> PackageDeviceEquipmentList = new List<PackageDevice>();
            PackageDeviceEquipmentList = (from DataRow dr in dt.Rows
                                          select new PackageDevice()
                                          {
                                              Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                              CompanyId = dr["CompanyId"] != DBNull.Value ? (Guid)(dr["CompanyId"]) : new Guid(),
                                              PackageId = dr["PackageId"] != DBNull.Value ? Convert.ToInt32(dr["PackageId"]) : 0,
                                              EquipmentId = dr["EquipmentId"] != DBNull.Value ? (Guid)(dr["EquipmentId"]) : new Guid(),
                                              IsFree = dr["IsFree"] != DBNull.Value ? Convert.ToBoolean(dr["IsFree"]) : false,
                                              IncludedEquipmentName = dr["IncludedEquipmentName"].ToString(),
                                              IncludedPackageName = dr["IncludedPackageName"].ToString(),
                                              EptNo = dr["EptNo"] != DBNull.Value ? Convert.ToInt32(dr["EptNo"]) : 0,
                                          }).ToList();
            return PackageDeviceEquipmentList;

        }
        public List<PackageOptional> GetAllPackageOptionalProducts(Guid CompanyId)
        {
            DataTable dt = _PackageDataAccess.GetAllPackageOptionalProductsByCompanyId(CompanyId);

            List<PackageOptional> PackageDeviceEquipmentList = new List<PackageOptional>();
            PackageDeviceEquipmentList = (from DataRow dr in dt.Rows
                                          select new PackageOptional()
                                          {
                                              Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                              CompanyId = dr["CompanyId"] != DBNull.Value ? (Guid)(dr["CompanyId"]) : new Guid(),
                                              PackageId = dr["PackageId"] != DBNull.Value ? Convert.ToInt32(dr["PackageId"]) : 0,
                                              EquipmentId = dr["EquipmentId"] != DBNull.Value ? (Guid)(dr["EquipmentId"]) : new Guid(),
                                              IsFree = dr["IsFree"] != DBNull.Value ? Convert.ToBoolean(dr["IsFree"]) : false,
                                              IncludedEquipmentName = dr["IncludedEquipmentName"].ToString(),
                                              IncludedPackageName = dr["IncludedPackageName"].ToString(),
                                          }).ToList();
            return PackageDeviceEquipmentList;

        }

        public long InsertPackageInclude(PackageInclude _PackageInclude)
        {
            return _PackageIncludeDataAccess.Insert(_PackageInclude);
        }
        public bool UpdatePackageInclude(PackageInclude _PackageInclude)
        {
            return _PackageIncludeDataAccess.Update(_PackageInclude) > 0;
        }
        public bool DeletePackageInclude(int Id)
        {
            return _PackageIncludeDataAccess.Delete(Id) > 0;
        }

        public long InsertPackageDevice(PackageDevice _PackageDevice)
        {
            return _PackageDeviceDataAccess.Insert(_PackageDevice);
        }
        public bool UpdatePackageDevice(PackageDevice _PackageDevice)
        {
            return _PackageDeviceDataAccess.Update(_PackageDevice) > 0;
        }
        public bool DeletePackageDevice(int Id)
        {
            return _PackageDeviceDataAccess.Delete(Id) > 0;
        }

        public long InsertPackageOptional(PackageOptional _PackageOptional)
        {
            return _PackageOptionalDataAccess.Insert(_PackageOptional);
        }
        public bool UpdatePackageOptional(PackageOptional _PackageOptional)
        {
            return _PackageOptionalDataAccess.Update(_PackageOptional) > 0;
        }
        public bool DeletePackageOptional(int Id)
        {
            return _PackageOptionalDataAccess.Delete(Id) > 0;
        }

        public List<LeadPackageDetail> GetAllLeadPackageDetailByLeadIdandCompanyId(Guid companyid, int id)
        {
            DataTable dt = _PackageDataAccess.GetAllLeadPackageDetailByLeadIdandCompanyId(companyid, id);
            List<LeadPackageDetail> Pdetail = new List<LeadPackageDetail>();
            Pdetail = (from DataRow dr in dt.Rows
                       select new LeadPackageDetail()
                       {
                           PackageName = dr["PackageName"].ToString(),
                           PackageInstallType = dr["PackageInstallType"].ToString(),
                           PanelName = dr["PanelName"].ToString()
                       }).ToList();
            return Pdetail;
        }
        public List<LeadPackageDetail> GetAllSmartLeadPackageDetailByLeadIdandCompanyId(Guid companyid, int id)
        {
            DataTable dt = _PackageDataAccess.GetAllSmartLeadPackageDetailByLeadIdandCompanyId(companyid, id);
            List<LeadPackageDetail> Pdetail = new List<LeadPackageDetail>();
            Pdetail = (from DataRow dr in dt.Rows
                       select new LeadPackageDetail()
                       {
                           PackageName = dr["PackageName"].ToString(),
                           PackageInstallType = dr["SmartInstallTypeName"].ToString(),
                           SmartSystemTypeName = dr["SmartSystemTypeName"].ToString()
                       }).ToList();
            return Pdetail;
        }

        public MMRRange GetMMrRangeByPackageId(Guid PackageId)
        {
            return _MMRRangeDataAccess.GetByQuery(string.Format("PackageId = '{0}'", PackageId)).FirstOrDefault();
        }
        public MMRRange GetMMrByPackageId(int PackageId)
        {
            return _MMRRangeDataAccess.GetByQuery(string.Format("PackageId = '{0}'", PackageId)).FirstOrDefault();
        }
        public long InsertMMrRange(MMRRange mr)
        {
            return _MMRRangeDataAccess.Insert(mr);
        }

        public bool UpdateMMRRange(MMRRange mr)
        {
            return _MMRRangeDataAccess.Update(mr) > 0;
        }

        public bool DeleteMMrRangeByPackageId(Guid companyid, Guid packageId)
        {
            return _PackageDataAccess.DeleteMMrRangeByPackageId(companyid, packageId);
        }

        public List<PackageInclude> GetPackageIncludeListByCustomerId(Guid customerId)
        {
            return _PackageIncludeDataAccess.GetPackageIncludeListByCustomerId(customerId);
        }

        public PackageCustomer GetPackageCustomerByCustomerId(Guid customerId)
        {
            return _PackageCustomerDataAccess.GetPackageCustomerByCustomerId(customerId);
        }

        public bool DeleteCustomerPackageEqpByCustomerIdServiceIdAndPackageId(Guid customerId, Guid equipmentId, Guid packageId)
        {
            return _CustomerPackageEqpDataAccess.DeleteCustomerPackageEqpByCustomerIdServiceIdAndPackageId(customerId,equipmentId,packageId);
        }

        public PackageCustomer GetWarrentyPackageCustomerByCustomerIdAndPackageId(Guid packageid, Guid customerid)
        {
            return _PackageCustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and PackageId = '{1}'", customerid, packageid)).FirstOrDefault();
        }

        public CustomerPackageService GetPackageCustomerServiceByCustomerIdAndPackageIdAndEquipmentId(Guid packageid, Guid customerid, Guid eqpid)
        {
            return _CustomerPackageServiceDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and PackageId = '{1}' and EquipmentId = '{2}'", customerid, packageid, eqpid)).FirstOrDefault();
        }

        public CustomerPackageEqp GetPackageCustomerEqpByCustomerIdAndPackageIdAndEquipmentId(Guid packageid, Guid customerid, Guid eqpid)
        {
            return _CustomerPackageEqpDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and PackageId = '{1}' and EquipmentId = '{2}'", customerid, packageid, eqpid)).FirstOrDefault();
        }

        public CustomerPackageEqp GetPackageCustomerEqpByCustomerIdAndEquipmentId(Guid customerid, Guid eqpid)
        {
            return _CustomerPackageEqpDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and EquipmentId = '{1}'", customerid, eqpid)).FirstOrDefault();
        }

        public CustomerPackageEqp GetPackageCustomerEqpByCustomerIdAndEquipmentIdAndAppointmentEqpId(Guid customerid, Guid eqpid, int appointeqpid)
        {
            return _CustomerPackageEqpDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and EquipmentId = '{1}' and AppointmentEquipmentIntId = {2}", customerid, eqpid, appointeqpid)).FirstOrDefault();
        }

        public CustomerPackageService GetPackageCustomerServiceByCustomerIdAndEquipmentId(Guid customerid, Guid eqpid)
        {
            return _CustomerPackageServiceDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and EquipmentId = '{1}'", customerid, eqpid)).FirstOrDefault();
        }

        public CustomerPackageService GetPackageCustomerServiceByCustomerIdAndEquipmentIdAndAppointmentEqpId(Guid customerid, Guid eqpid, int appointeqpid)
        {
            return _CustomerPackageServiceDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and EquipmentId = '{1}' and AppointmentEquipmentIntId = {2}", customerid, eqpid, appointeqpid)).FirstOrDefault();
        }

        public List<CustomerPackageService> GetCustomerPackageServiceByCustomerId(Guid customerid)
        {
            //return _CustomerPackageServiceDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerid)).ToList();
            DataTable dt = _CustomerPackageEqpDataAccess.GetCustomerPackageServiceByCustomerId(customerid);
            List<CustomerPackageService> cusEqp = new List<CustomerPackageService>();
            cusEqp = (from DataRow dr in dt.Rows
                      select new CustomerPackageService()
                      {
                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                          CompanyId = dr["CompanyId"] != DBNull.Value ? (Guid)(dr["CompanyId"]) : new Guid(),
                          CustomerId = dr["CustomerId"] != DBNull.Value ? (Guid)(dr["CustomerId"]) : new Guid(),
                          EquipmentId = dr["EquipmentId"] != DBNull.Value ? (Guid)(dr["EquipmentId"]) : new Guid(),
                          PackageId = dr["PackageId"] != DBNull.Value ? (Guid)(dr["PackageId"]) : new Guid(),

                          EquipmentName = dr["EquipmentName"].ToString(),
                          MonthlyRate = dr["MonthlyRate"] != DBNull.Value ? Convert.ToDouble(dr["MonthlyRate"]) : 0,
                          DiscountRate = dr["DiscountRate"] != DBNull.Value ? Convert.ToDouble(dr["DiscountRate"]) : 0,
                          Total = dr["Total"] != DBNull.Value ? Convert.ToDouble(dr["Total"]) : 0,

                      }).ToList();
            return cusEqp;
        }

        public List<CustomerPackageEqp> GetCustomerPackageEqpByCustomerId(Guid customerid)
        {
           // return _CustomerPackageEqpDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerid)).ToList();
            DataTable dt = _CustomerPackageEqpDataAccess.GetAllCustomerPackageEqpByCustomerId(customerid);
            List<CustomerPackageEqp> cusEqp = new List<CustomerPackageEqp>();
            cusEqp = (from DataRow dr in dt.Rows
                       select new CustomerPackageEqp()
                       {
                           Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                           CompanyId = dr["CompanyId"] != DBNull.Value ? (Guid)(dr["CompanyId"]) : new Guid(),
                           CustomerId = dr["CustomerId"] != DBNull.Value ? (Guid)(dr["CustomerId"]) : new Guid(),
                           EquipmentId = dr["EquipmentId"] != DBNull.Value ? (Guid)(dr["EquipmentId"]) : new Guid(),
                           PackageId = dr["PackageId"] != DBNull.Value ? (Guid)(dr["PackageId"]) : new Guid(),
                           ServiceId = dr["ServiceId"] != DBNull.Value ? (Guid)(dr["ServiceId"]) : new Guid(),

                           IsIncluded = dr["IsIncluded"] != DBNull.Value ? Convert.ToBoolean(dr["IsIncluded"]) : false,
                           IsDevice = dr["IsDevice"] != DBNull.Value ? Convert.ToBoolean(dr["IsDevice"]) : false,
                           //IsDeletable = dr["IsDeletable"] != DBNull.Value ? Convert.ToBoolean(dr["IsDeletable"]) : false,
                           IsOptionalEqp = dr["IsOptionalEqp"] != DBNull.Value ? Convert.ToBoolean(dr["IsOptionalEqp"]) : false,
                           IsServiceEquipment = dr["IsServiceEquipment"] != DBNull.Value ? Convert.ToBoolean(dr["IsServiceEquipment"]) : false,
                           IsEqpExist = dr["IsEqpExist"] != DBNull.Value ? Convert.ToBoolean(dr["IsEqpExist"]) : false,
                           IsPackageEqp = dr["IsPackageEqp"] != DBNull.Value ? Convert.ToBoolean(dr["IsPackageEqp"]) : false,
                           IsTransfered = dr["IsTransfered"] != DBNull.Value ? Convert.ToBoolean(dr["IsTransfered"]) : false,
                            
                           EquipmentName = dr["EquipmentName"].ToString(),
                           Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0,
                           DiscountPckage = dr["DiscountPckage"] != DBNull.Value ? Convert.ToDouble(dr["DiscountPckage"]) : 0,
                           Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                           UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0,
                           Total = dr["Total"] != DBNull.Value ? Convert.ToDouble(dr["Total"]) : 0,
                           DiscountUnitPricce = dr["DiscountUnitPricce"] != DBNull.Value ? Convert.ToDouble(dr["DiscountUnitPricce"]) : 0,
                         
                       }).ToList();
            return cusEqp;
        }

        public CustomerPackageService GetPackageServiceByCustomerIdAndAppointmentIdAndPackageIdAndEquipmentId(Guid customerid, int appointmentid, Guid packageid, Guid equipmentid)
        {
            return _CustomerPackageServiceDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and AppointmentIntId = {1} and PackageId = '{2}' and EquipmentId = '{3}'", customerid, appointmentid, packageid, equipmentid)).FirstOrDefault();
        }

        public CustomerPackageEqp GetPackageEqpByCustomerIdAndAppointmentIdAndPackageIdAndEquipmentId(Guid customerid, int appointmentid, Guid packageid, Guid equipmentid)
        {
            return _CustomerPackageEqpDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and AppointmentIntId = {1} and PackageId = '{2}' and EquipmentId = '{3}'", customerid, appointmentid, packageid, equipmentid)).FirstOrDefault();
        }

        public CustomerPackageEqp GetPackageEqpByCustomerIdAndAppointmentIdAndPackageIdAndEquipmentIdAndEqpIntId(Guid customerid, int appointmentid, Guid packageid, Guid equipmentid, int appeqpid)
        {
            return _CustomerPackageEqpDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and AppointmentIntId = {1} and PackageId = '{2}' and EquipmentId = '{3}' and AppointmentEquipmentIntId = {4}", customerid, appointmentid, packageid, equipmentid, appeqpid)).FirstOrDefault();
        }

        public List<CustomerPackageService> GetAllPackageServiceByCustomerIdAndCompanyId(Guid customerid, Guid companyid)
        {
            return _CustomerPackageServiceDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}'", customerid, companyid)).ToList();
        }

        public List<CustomerPackageEqp> GetAllPackageEqpByCustomerIdAndCompanyId(Guid customerid, Guid companyid)
        {
            return _CustomerPackageEqpDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}'", customerid, companyid)).ToList();
        }
    }
}
