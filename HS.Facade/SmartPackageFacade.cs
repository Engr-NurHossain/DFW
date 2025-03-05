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
    public class SmartPackageFacade : BaseFacade
    {
        public SmartPackageFacade(ClientContext clientContext) : base(clientContext) { }
        SmartPackageDataAccess SmartPackageDataAccess
        {
            get
            {
                return (SmartPackageDataAccess)_ClientContext[typeof(SmartPackageDataAccess)];
            }
        }

        SmartSystemTypeDataAccess SmartSystemTypeDataAccess
        {
            get
            {
                return (SmartSystemTypeDataAccess)_ClientContext[typeof(SmartSystemTypeDataAccess)];
            }
        }
        PackageCommissionDataAccess PackageCommissionDataAccess
        {
            get
            {
                return (PackageCommissionDataAccess)_ClientContext[typeof(PackageCommissionDataAccess)];
            }
        }
        SalesComissionDataAccess SalesComissionDataAccess
        {
            get
            {
                return (SalesComissionDataAccess)_ClientContext[typeof(SalesComissionDataAccess)];
            }
        }

        SmartSystemInstallTypeDataAccess SmartSystemInstallTypeDataAccess
        {
            get
            {
                return (SmartSystemInstallTypeDataAccess)_ClientContext[typeof(SmartSystemInstallTypeDataAccess)];
            }
        }

        SmartInstallTypeDataAccess SmartInstallTypeDataAccess
        {
            get
            {
                return (SmartInstallTypeDataAccess)_ClientContext[typeof(SmartInstallTypeDataAccess)];
            }
        }

        SmartPackageEquipmentServiceDataAccess SmartPackageEquipmentServiceDataAccess
        {
            get
            {
                return (SmartPackageEquipmentServiceDataAccess)_ClientContext[typeof(SmartPackageEquipmentServiceDataAccess)];
            }
        }

        SmartPackageSystemInstallTypeMapDataAccess SmartPackageSystemInstallTypeMapDataAccess
        {
            get
            {
                return (SmartPackageSystemInstallTypeMapDataAccess)_ClientContext[typeof(SmartPackageSystemInstallTypeMapDataAccess)];
            }
        }

        SmartPackageDataAccess _SmartPackageDataAccess
        {
            get
            {
                return (SmartPackageDataAccess)_ClientContext[typeof(SmartPackageDataAccess)];
            }
        }

        CustomerPackageServiceDataAccess _CustomerPackageServiceDataAccess
        {
            get
            {
                return (CustomerPackageServiceDataAccess)_ClientContext[typeof(CustomerPackageServiceDataAccess)];
            }
        }

        CustomerPackageEqpDataAccess _CustomerPackageEqpDataAccess
        {
            get
            {
                return (CustomerPackageEqpDataAccess)_ClientContext[typeof(CustomerPackageEqpDataAccess)];
            }
        }

        SmartPackageEquipmentServiceEquipmentDataAccess _SmartPackageEquipmentServiceEquipmentDataAccess
        {
            get
            {
                return (SmartPackageEquipmentServiceEquipmentDataAccess)_ClientContext[typeof(SmartPackageEquipmentServiceEquipmentDataAccess)];
            }
        }
        SystemTypeManufacturerMapDataAccess _SystemTypeManufacturerMapDataAccess
        {
            get
            {
                return (SystemTypeManufacturerMapDataAccess)_ClientContext[typeof(SystemTypeManufacturerMapDataAccess)];
            }
        }
        SystemTypeServiceMapDataAccess _SystemTypeServiceMapDataAccess
        {
            get
            {
                return (SystemTypeServiceMapDataAccess)_ClientContext[typeof(SystemTypeServiceMapDataAccess)];
            }
        }
        SmartPackageEquipmentServiceDataAccess _SmartPackageEquipmentServiceDataAccess
        {
            get
            {
                return (SmartPackageEquipmentServiceDataAccess)_ClientContext[typeof(SmartPackageEquipmentServiceDataAccess)];
            }
        }
        #region SmartSystemType
        public SmartSystemType GetSmartSystemTypeById(int Id)
        {
            return SmartSystemTypeDataAccess.Get(Id);
        }
        public List<SmartSystemType> GetAllSmartSystemType(Guid CompanyId)
        {
            return SmartSystemTypeDataAccess.GetByQuery(String.Format("CompanyId = '{0}'", CompanyId));
        }

        public bool UpdateSystemType(SmartSystemType smartSystemType)
        {
            return SmartSystemTypeDataAccess.Update(smartSystemType) > 0;
        }

        public bool InsertSystemType(SmartSystemType smartSystemType)
        {
            return SmartSystemTypeDataAccess.Insert(smartSystemType) > 0;
        }
        public bool DeleteSystemType(int Id)
        {
            return SmartSystemTypeDataAccess.Delete(Id) > 0;
        }
        public List<SmartInstallType> LoadInstallType(int SystemTypeId, Guid CompanyId)
        {
            DataTable dt = SmartInstallTypeDataAccess.GetAllInstallTypeByCompanyIdSystemId(SystemTypeId, CompanyId);

            List<SmartInstallType> SmartInstallType = new List<SmartInstallType>();
            SmartInstallType = (from DataRow dr in dt.Rows
                                select new SmartInstallType()
                                {
                                    Id = dr["InstallTypeId"] != DBNull.Value ? Convert.ToInt32(dr["InstallTypeId"]) : 0,
                                    CompanyId = dr["CompanyId"] != DBNull.Value ? (Guid)(dr["CompanyId"]) : new Guid(),
                                    Name = dr["Name"].ToString(),
                                }).ToList();
            return SmartInstallType;
        }
        #endregion

        #region SmartInstallType
        public List<SmartInstallType> GetAllSmartInstallType(Guid CompanyId)
        {
            return SmartInstallTypeDataAccess.GetByQuery(String.Format("CompanyId = '{0}'", CompanyId));
        }
        public SmartInstallType GetSmartInstallTypeByID(int Id)
        {
            return SmartInstallTypeDataAccess.Get(Id);
        }

        public bool UpdateInstallType(SmartInstallType smartInstallType)
        {
            return SmartInstallTypeDataAccess.Update(smartInstallType) > 0;
        }

        public bool InsertInstallType(SmartInstallType smartInstallType)
        {
            return SmartInstallTypeDataAccess.Insert(smartInstallType) > 0;
        }
        public bool DeleteInstallType(int smartInstallType)
        {
            return SmartInstallTypeDataAccess.Delete(smartInstallType) > 0;
        }
        #endregion

        #region SmartSystemInstallType
        public List<SmartSystemInstallType> GetAllSmartSystemInstallTypeByCompanyId(Guid CompanyId)
        {
            DataTable dt = SmartSystemInstallTypeDataAccess.GetAllSmartSystemInstallTypeByCompanyId(CompanyId);

            List<SmartSystemInstallType> SmartSystemInstallType = new List<SmartSystemInstallType>();
            SmartSystemInstallType = (from DataRow dr in dt.Rows
                                      select new SmartSystemInstallType()
                                      {
                                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                          CompanyId = dr["CompanyId"] != DBNull.Value ? (Guid)(dr["CompanyId"]) : new Guid(),
                                          SystemId = dr["SystemId"] != DBNull.Value ? Convert.ToInt32(dr["SystemId"]) : 0,
                                          InstallTypeId = dr["InstallTypeId"] != DBNull.Value ? Convert.ToInt32(dr["InstallTypeId"]) : 0,
                                          SystemType = dr["SystemType"].ToString(),
                                          InstallType = dr["InstallType"].ToString(),
                                      }).ToList();
            return SmartSystemInstallType;
        }
        public bool InsertSystemInstallType(SmartSystemInstallType smartSystemType)
        {
            return SmartSystemInstallTypeDataAccess.Insert(smartSystemType) > 0;
        }

        public bool DeleteSystemInstallType(int smartSystemType)
        {
            return SmartSystemInstallTypeDataAccess.Delete(smartSystemType) > 0;
        }

        #endregion

        #region SmartPackage
        public SmartPackage GetPackageByIdAndCompanyId(int Id, Guid CompanyId)
        {
            return SmartPackageDataAccess.GetByQuery(string.Format("Id = '{0}' and CompanyId = '{1}'", Id, CompanyId)).FirstOrDefault();
        }
        public SmartPackage GetPackageById(int Id)
        {
            return SmartPackageDataAccess.Get(Id);
        }
        public bool InsertPackage(SmartPackage SmartPackage)
        {
            return SmartPackageDataAccess.Insert(SmartPackage) > 0;
        }
        public bool UpdatePackage(SmartPackage SmartPackage)
        {
            return SmartPackageDataAccess.Update(SmartPackage) > 0;
        }
        public bool DeletePackage(int Id)
        {
            return SmartPackageDataAccess.Delete(Id) > 0;
        }
        public List<SmartPackage> GetAllSmartPackageByCompanyId(Guid CompanyId)
        {
            DataTable dt = SmartPackageDataAccess.GetAllSmartPackageByCompanyId(CompanyId);

            List<SmartPackage> SmartPackageList = new List<SmartPackage>();
            SmartPackageList = (from DataRow dr in dt.Rows
                                select new SmartPackage()
                                {
                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    CompanyId = dr["CompanyId"] != DBNull.Value ? (Guid)(dr["CompanyId"]) : new Guid(),
                                    PackageId = dr["PackageId"] != DBNull.Value ? (Guid)(dr["PackageId"]) : new Guid(),
                                    SmartInstallTypeId = dr["SmartInstallTypeId"] != DBNull.Value ? Convert.ToInt32(dr["SmartInstallTypeId"]) : 0,
                                    SmartSystemTypeId = dr["SmartSystemTypeId"] != DBNull.Value ? Convert.ToInt32(dr["SmartSystemTypeId"]) : 0,
                                    EquipmentMaxLimit = dr["EquipmentMaxLimit"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentMaxLimit"]) : 0,
                                    SystemType = dr["SystemType"].ToString(),
                                    InstallType = dr["InstallType"].ToString(),
                                    PackageName = dr["PackageName"].ToString(),
                                    ActivationFee = dr["ActivationFee"] != DBNull.Value ? Convert.ToDouble(dr["ActivationFee"]) : 0,
                                    IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                                    IsPromo = dr["IsPromo"] != DBNull.Value ? Convert.ToBoolean(dr["IsPromo"]) : false,
                                    TotalRMR = dr["TotalRMR"] != DBNull.Value ? Convert.ToDouble(dr["TotalRMR"]) : 0,
                                    LastUpdatedName = dr["UserName"].ToString(),
                                    LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                    EndDate = dr["EndDate"] != DBNull.Value ? Convert.ToDateTime(dr["EndDate"]) : new DateTime(),
                                    StartDate = dr["StartDate"] != DBNull.Value ? Convert.ToDateTime(dr["StartDate"]) : new DateTime(),
                                    PackageCode = dr["PackageCode"].ToString(),
                                    UserType = dr["UserType"].ToString(),
                                    ConformingFee = dr["ConformingFee"] != DBNull.Value ? Convert.ToDouble(dr["ConformingFee"]) : 0,
                                    PackageType = dr["PackageType"].ToString(),
                                    MinCredit= dr["MinCredit"] != DBNull.Value ? Convert.ToDouble(dr["MinCredit"]) : 0,
                                    ManufacturerId= dr["ManufacturerId"] != DBNull.Value ? (Guid)(dr["ManufacturerId"]) : new Guid(),
                                    NonConforming = dr["NonConforming"] != DBNull.Value ? Convert.ToBoolean(dr["NonConforming"]) : false,
                                    CustomerNumber = dr["CustomerNumber"].ToString()
                                }).ToList();
            return SmartPackageList;
        }

        public PackageModel GetAllSmartPackageByFilter(SmartPackageFilter filter)
        {
            DataSet dsResult = SmartPackageDataAccess.GetAllSmartPackageByFilter(filter);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];

            List<SmartPackage> SmartPackageList = new List<SmartPackage>();
            PackageCount TotalPackage = new PackageCount();
            SmartPackageList = (from DataRow dr in dt.Rows
                                select new SmartPackage()
                                {
                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    CompanyId = dr["CompanyId"] != DBNull.Value ? (Guid)(dr["CompanyId"]) : new Guid(),
                                    PackageId = dr["PackageId"] != DBNull.Value ? (Guid)(dr["PackageId"]) : new Guid(),
                                    SmartInstallTypeId = dr["SmartInstallTypeId"] != DBNull.Value ? Convert.ToInt32(dr["SmartInstallTypeId"]) : 0,
                                    SmartSystemTypeId = dr["SmartSystemTypeId"] != DBNull.Value ? Convert.ToInt32(dr["SmartSystemTypeId"]) : 0,
                                    EquipmentMaxLimit = dr["EquipmentMaxLimit"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentMaxLimit"]) : 0,
                                    SystemType = dr["SystemType"].ToString(),
                                    InstallType = dr["InstallType"].ToString(),
                                    PackageName = dr["PackageName"].ToString(),
                                    ActivationFee = dr["ActivationFee"] != DBNull.Value ? Convert.ToDouble(dr["ActivationFee"]) : 0,
                                    IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                                    IsPromo = dr["IsPromo"] != DBNull.Value ? Convert.ToBoolean(dr["IsPromo"]) : false,
                                    TotalRMR = dr["TotalRMR"] != DBNull.Value ? Convert.ToDouble(dr["TotalRMR"]) : 0,
                                    LastUpdatedName = dr["UserName"].ToString(),
                                    LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                    EndDate = dr["EndDate"] != DBNull.Value ? Convert.ToDateTime(dr["EndDate"]) : new DateTime(),
                                    StartDate = dr["StartDate"] != DBNull.Value ? Convert.ToDateTime(dr["StartDate"]) : new DateTime(),
                                    PackageCode = dr["PackageCode"].ToString(),
                                    UserType = dr["UserType"].ToString(),
                                    ConformingFee = dr["ConformingFee"] != DBNull.Value ? Convert.ToDouble(dr["ConformingFee"]) : 0,
                                    PackageType = dr["PackageType"].ToString(),
                                }).ToList();
            TotalPackage = (from DataRow dr in dt1.Rows
                            select new PackageCount()
                            {
                                TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                            }).FirstOrDefault();

            PackageModel pm = new PackageModel();
            pm.PackageList = SmartPackageList;
            pm.TotalCount = TotalPackage;
            return pm;
        }



        public ExistSmartPackage GetAllDuplicateSmartPackagesCountByPackageName(string PackageName)
        {
            DataTable dt = SmartPackageDataAccess.GetAllDuplicateSmartPackagesByPackageName(PackageName);

            List<ExistSmartPackage> SmartPackageList = new List<ExistSmartPackage>();
            SmartPackageList = (from DataRow dr in dt.Rows
                                select new ExistSmartPackage()
                                {
                                    ExistCount = dr["ExistCount"] != DBNull.Value ? Convert.ToInt32(dr["ExistCount"]) : 0,


                                }).ToList();
            return SmartPackageList.FirstOrDefault();
        }
        public SmartPackage GetPackageByPackageIdAndCompanyId(Guid PackageId, Guid CompanyId)
        {
            return SmartPackageDataAccess.GetByQuery(string.Format("PackageId = '{0}' and CompanyId = '{1}'", PackageId, CompanyId)).FirstOrDefault();
        }

        public SmartPackage GetSmartPackageByPackageId(Guid PackageId)
        {
            return SmartPackageDataAccess.GetByQuery(string.Format("PackageId = '{0}'", PackageId)).FirstOrDefault();
        }
        public SmartPackage GetSmartPackageById(int Id)
        {
            return SmartPackageDataAccess.Get(Id);
        }
        #endregion

        #region SmartPackageEquipmentService
        public List<SmartPackageEquipmentService> GetAllSmartPackageEquipmentServiceByPackageId(Guid PackageId)
        {
            return SmartPackageEquipmentServiceDataAccess.GetByQuery(string.Format("PackageId = '{0}'", PackageId));
        }
        public List<SmartPackageEquipmentService> GetAllSmartPackageEquipmentService(Guid CompanyId, string Type)
        {
            DataTable dt = SmartPackageEquipmentServiceDataAccess.GetAllSmartPackageEquipmentServiceByTypeAndCompanyId(CompanyId, Type);

            List<SmartPackageEquipmentService> SmartPackageEquipmentService = new List<SmartPackageEquipmentService>();
            SmartPackageEquipmentService = (from DataRow dr in dt.Rows
                                            select new SmartPackageEquipmentService()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                EptNo = dr["EptNo"] != DBNull.Value ? Convert.ToInt32(dr["EptNo"]) : 0,
                                                Price = dr["Price"] != DBNull.Value ? Convert.ToDouble(dr["Price"]) : 0,
                                                OriginalPrice = dr["OriginalPrice"] != DBNull.Value ? Convert.ToDouble(dr["OriginalPrice"]) : 0,
                                                CompanyId = dr["CompanyId"] != DBNull.Value ? (Guid)(dr["CompanyId"]) : new Guid(),
                                                PackageId = dr["PackageId"] != DBNull.Value ? (Guid)(dr["PackageId"]) : new Guid(),
                                                EquipmentId = dr["EquipmentId"] != DBNull.Value ? (Guid)(dr["EquipmentId"]) : new Guid(),
                                                SmartPackageEquipmentServiceId = dr["SmartPackageEquipmentServiceId"] != DBNull.Value ? (Guid)(dr["SmartPackageEquipmentServiceId"]) : Guid.Empty,
                                                IsFree = dr["IsFree"] != DBNull.Value ? Convert.ToBoolean(dr["IsFree"]) : false,
                                                EquipmentName = dr["EquipmentName"].ToString(),
                                                PackageName = dr["PackageName"].ToString(),
                                                Status = dr["Status"] != DBNull.Value ? Convert.ToBoolean(dr["Status"]) : false,
                                                LastUpdatedName = dr["UserName"].ToString(),
                                                ManufacturerName = dr["ManufacturerName"].ToString(),
                                                SKU = dr["SKU"].ToString(),
                                                LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]).ClientToUTCTime() : new DateTime(),
                                            }).ToList();
            return SmartPackageEquipmentService;

        }

        public PackageEquipmentServiceModel GetAllSmartPackageEquipmentServiceListByTypeAndCompanyId(Guid CompanyId, string Type, int pageno, int pagesize, string status)
        {
            DataSet ds = SmartPackageEquipmentServiceDataAccess.GetAllSmartPackageEquipmentServiceListByTypeAndCompanyId(CompanyId, Type, pageno, pagesize, status);

            PackageEquipmentServiceModel model = new PackageEquipmentServiceModel();
            model.ListSmartPackageEquipmentService = (from DataRow dr in ds.Tables[0].Rows
                                                      select new SmartPackageEquipmentService()
                                                      {
                                                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                          EptNo = dr["EptNo"] != DBNull.Value ? Convert.ToInt32(dr["EptNo"]) : 0,
                                                          Price = dr["Price"] != DBNull.Value ? Convert.ToDouble(dr["Price"]) : 0,
                                                          CompanyId = dr["CompanyId"] != DBNull.Value ? (Guid)(dr["CompanyId"]) : new Guid(),
                                                          PackageId = dr["PackageId"] != DBNull.Value ? (Guid)(dr["PackageId"]) : new Guid(),
                                                          EquipmentId = dr["EquipmentId"] != DBNull.Value ? (Guid)(dr["EquipmentId"]) : new Guid(),
                                                          SmartPackageEquipmentServiceId = dr["SmartPackageEquipmentServiceId"] != DBNull.Value ? (Guid)(dr["SmartPackageEquipmentServiceId"]) : Guid.Empty,
                                                          IsFree = dr["IsFree"] != DBNull.Value ? Convert.ToBoolean(dr["IsFree"]) : false,
                                                          EquipmentName = dr["EquipmentName"].ToString(),
                                                          PackageName = dr["PackageName"].ToString(),
                                                          Status = dr["Status"] != DBNull.Value ? Convert.ToBoolean(dr["Status"]) : false,
                                                          LastUpdatedName = dr["UserName"].ToString(),
                                                          ManufacturerName = dr["ManufacturerName"].ToString(),
                                                          LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]).ClientToUTCTime() : new DateTime(),
                                                      }).ToList();
            model.PackageEquipmentServiceTotalCountModel = (from DataRow dr in ds.Tables[1].Rows
                                                            select new PackageEquipmentServiceTotalCountModel()
                                                            {
                                                                TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0,
                                                            }).FirstOrDefault();
            return model;

        }

        public SmartPackageEquipmentService GetSmartPackageEquipmentServiceById(int Id)
        {
            return SmartPackageEquipmentServiceDataAccess.Get(Id);
        }
        public bool UpdateSmartPackageEquipmentService(SmartPackageEquipmentService SmartPackageEquipmentService)
        {
            return SmartPackageEquipmentServiceDataAccess.Update(SmartPackageEquipmentService) > 0;
        }
        public bool InsertSmartPackageEquipmentService(SmartPackageEquipmentService SmartPackageEquipmentService)
        {
            return SmartPackageEquipmentServiceDataAccess.Insert(SmartPackageEquipmentService) > 0;
        }

        public bool DeleteSmartPackageEquipmentService(int id)
        {
            return SmartPackageEquipmentServiceDataAccess.Delete(id) > 0;
        }
        public bool DeleteSmartPackageEquipmentServiceByPackageId(Guid PackageId)
        {
            List<SmartPackageEquipmentService> model = SmartPackageEquipmentServiceDataAccess.GetByQuery(string.Format("PackageId='{0}'", PackageId));
            foreach (var item in model)
            {
                SmartPackageEquipmentServiceDataAccess.Delete(item.Id);
            }
            return true;
        }
        #endregion

        #region SmartPackageSystemInstallTypeMap

        public bool InsertSmartPackageSystemInstallTypeMap(SmartPackageSystemInstallTypeMap model)
        {
            return SmartPackageSystemInstallTypeMapDataAccess.Insert(model) > 0;
        }
        public bool UpdateSmartPackageSystemInstallTypeMap(SmartPackageSystemInstallTypeMap model)
        {
            return SmartPackageSystemInstallTypeMapDataAccess.Update(model) > 0;
        }
        public bool DeleteSmartPackageSystemInstallTypeMap(int Id)
        {
            return SmartPackageSystemInstallTypeMapDataAccess.Delete(Id) > 0;
        }
        public SmartPackageSystemInstallTypeMap GetSmartPackageSystemInstallTypeMapByPackageId(Guid Id)
        {
            return SmartPackageSystemInstallTypeMapDataAccess.GetByQuery(string.Format("PackageId='{0}'", Id)).FirstOrDefault();
        }

        public bool DeleteSmartPackageSystemInstallTypeMapBySystemId(int Id)
        {
            var model = SmartPackageSystemInstallTypeMapDataAccess.GetByQuery(string.Format("SmartSystemTypeId = '{0}'", Id));
            foreach (var item in model)
            {
                SmartPackageSystemInstallTypeMapDataAccess.Delete(item.Id);
            }
            return true;
        }
        #endregion

        #region SalesCommision
        public SalesComission GetSalesCommisionBypackageSalesType(Guid CompanyId, Guid packageId, string SalesLocation, string LeadType)
        {
            return SalesComissionDataAccess.GetByQuery(string.Format("CompanyId='{0}' AND PackageServiceId='{1}' AND SalesLocation='{2}' AND LeadType='{3}'", CompanyId, packageId, SalesLocation, LeadType)).FirstOrDefault();
        }
        public List<SalesComission> GetAllSalesCommisionBypackageId(Guid packageId, Guid CompanyId)
        {
            return SalesComissionDataAccess.GetByQuery(string.Format("PackageServiceId='{0}' AND CompanyId='{1}'", packageId, CompanyId));
        }
        public List<SalesComission> GetAllSalesCommisionByCompanyId(Guid CompanyId)
        {
            return SalesComissionDataAccess.GetByQuery(string.Format("CompanyId='{0}'", CompanyId));
        }

        public bool UpdateSalesCommission(SalesComission SalesComission)
        {
            return SalesComissionDataAccess.Update(SalesComission) > 0;
        }
        public bool InsertSalesCommission(SalesComission SalesComission)
        {
            return SalesComissionDataAccess.Insert(SalesComission) > 0;
        }
        public bool DeleteSalesCommission(int id)
        {
            return SalesComissionDataAccess.Delete(id) > 0;
        }

        public List<SmartPackageEquipmentServiceEquipment> GetSmartPackageEquipmentServiceEquipmentBySmartPackageEquipmentServiceId(Guid smartPackageEquipmentServiceId)
        {
            DataTable dt = _SmartPackageEquipmentServiceEquipmentDataAccess.GetSmartPackageEquipmentServiceEquipmentBySmartPackageEquipmentServiceId(smartPackageEquipmentServiceId);
            List<SmartPackageEquipmentServiceEquipment> SmartInstallType = new List<SmartPackageEquipmentServiceEquipment>();
            SmartInstallType = (from DataRow dr in dt.Rows
                                select new SmartPackageEquipmentServiceEquipment()
                                {
                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    EquipmentId = dr["EquipmentId"] != DBNull.Value ? (Guid)(dr["EquipmentId"]) : new Guid(),
                                    SmartPackageEquipmentServiceId = dr["SmartPackageEquipmentServiceId"] != DBNull.Value ? (Guid)(dr["SmartPackageEquipmentServiceId"]) : new Guid(),
                                    Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 1,
                                    EquipmentName = dr["EquipmentName"].ToString(),
                                    EquipmentPrice = dr["EquipmentPrice"] != DBNull.Value ? Convert.ToDouble(dr["EquipmentPrice"]) : 0,
                                }).ToList();
            return SmartInstallType;

        }

        public SmartPackageEquipmentService GetSmartPackageEquipmentServiceByPackageIdEquipmentId(Guid PackageId, Guid EquipmentId)
        {
            string sql = string.Format("PackageId='{0}' AND EquipmentId='{1}'", PackageId, EquipmentId);
            return _SmartPackageEquipmentServiceDataAccess.GetByQuery(sql).FirstOrDefault();

        }

        public bool DeleteSmartPackageEquipmentServiceBySmartPackageEquipmentServiceId(Guid smartPackageEquipmentServiceId)
        {
            return _SmartPackageEquipmentServiceEquipmentDataAccess.DeleteSmartPackageEquipmentServiceBySmartPackageEquipmentServiceId(smartPackageEquipmentServiceId);
        }

        public bool InsertSmartPackageEquipmentServiceEquipment(SmartPackageEquipmentServiceEquipment item)
        {
            return _SmartPackageEquipmentServiceEquipmentDataAccess.Insert(item) > 0;
        }
        #endregion

        #region PackageCommission
        public List<PackageCommission> GetAllPackageComission()
        {
            //return PackageCommissionDataAccess.GetAll();
            DataTable dt = PackageCommissionDataAccess.GetAllPackageComission();

            List<PackageCommission> PackageCommission = new List<PackageCommission>();
            PackageCommission = (from DataRow dr in dt.Rows
                                 select new PackageCommission()
                                 {
                                     Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                     TypeVal = dr["TypeVal"].ToString(),
                                     PackageTypeVal = dr["PackageTypeVal"].ToString(),
                                     LeadTypeVal = dr["LeadTypeVal"].ToString(),
                                     CommissionTypeVal = dr["CommissionTypeVal"].ToString(),
                                     Commission = dr["Commission"] != DBNull.Value ? Convert.ToDouble(dr["Commission"]) : 0,

                                 }).ToList();
            return PackageCommission;
        }
        public List<PackageCommission> GetAllPackageComissionByLeadTypeandPackageType(string Type, string LeadType, string PackageType)
        {

            return PackageCommissionDataAccess.GetByQuery(string.Format("Type ='{0}' and PackageType = '{1}' and  LeadType = '{2}'", Type, PackageType, LeadType));
        }
        public PackageCommission GetPackageCommissionByAll(string type, string leadType, string packageType, string commissionType)
        {
            string query = "";
            if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(leadType) && !string.IsNullOrEmpty(packageType) && !string.IsNullOrEmpty(commissionType))
            {
                query = string.Format("Type='{0}' AND LeadType='{1}' AND PackageType='{2}' AND CommissionType='{3}'", type, leadType, packageType, commissionType);
            }
            else if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(leadType) && !string.IsNullOrEmpty(packageType))
            {
                query = string.Format("Type='{0}' AND LeadType='{1}' AND PackageType='{2}'", type, leadType, packageType);
            }
            else if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(leadType))
            {
                query = string.Format("Type='{0}' AND LeadType='{1}'", type, leadType);
            }
            else if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(commissionType))
            {
                query = string.Format("Type='{0}' AND CommissionType='{1}'", type, commissionType);
            }
            else if (!string.IsNullOrEmpty(type))
            {
                query = string.Format("Type='{0}'", type);
            }
            return PackageCommissionDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public PackageCommission GetPackageCommissionById(int Id)
        {
            return PackageCommissionDataAccess.Get(Id);
        }
        public bool UpdatePackageCommission(PackageCommission packagecommission)
        {
            return PackageCommissionDataAccess.Update(packagecommission) > 0;
        }
        public bool InsertPackageCommission(PackageCommission packCom)
        {
            return PackageCommissionDataAccess.Insert(packCom) > 0;
        }
        #endregion

        #region SystemTypeManufacturerMap
        public List<SystemTypeManufacturerMap> GetAllSystemTypeManufacturerMap()
        {
            DataTable dt = _SystemTypeManufacturerMapDataAccess.GetAllSystemTypeManufacturerMap();

            List<SystemTypeManufacturerMap> systemTypeManufacturerMap = new List<SystemTypeManufacturerMap>();
            systemTypeManufacturerMap = (from DataRow dr in dt.Rows
                                         select new SystemTypeManufacturerMap()
                                         {
                                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                             SystemId = dr["SystemId"] != DBNull.Value ? Convert.ToInt32(dr["SystemId"]) : 0,
                                             ManufacturerId = dr["ManufacturerId"] != DBNull.Value ? (Guid)(dr["ManufacturerId"]) : new Guid(),
                                             SystemType = dr["SystemType"].ToString(),
                                             ManufacturerName = dr["ManufacturerName"].ToString(),
                                         }).ToList();
            return systemTypeManufacturerMap;
        }
        public bool InsertSystemTypeManufacturerMap(SystemTypeManufacturerMap model)
        {
            return _SystemTypeManufacturerMapDataAccess.Insert(model) > 0;
        }

        public bool DeleteSystemTypeManufacturerMap(int smartSystemTypeManId)
        {
            return _SystemTypeManufacturerMapDataAccess.Delete(smartSystemTypeManId) > 0;
        }

        #endregion

        #region SystemTypeServiceMap
        public List<SystemTypeServiceMap> GetAllServiceForSmartSetUp()
        {
            DataTable dt = _SystemTypeServiceMapDataAccess.GetAllServiceForSmartSetUp();

            List<SystemTypeServiceMap> systemTypeServiceMap = new List<SystemTypeServiceMap>();
            systemTypeServiceMap = (from DataRow dr in dt.Rows
                                    select new SystemTypeServiceMap()
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        EquipmentId = dr["EquipmentId"] != DBNull.Value ? (Guid)(dr["EquipmentId"]) : new Guid(),
                                        ServiceName = dr["ServiceName"].ToString(),
                                        Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0.0
                                    }).ToList();
            return systemTypeServiceMap;
        }
        public List<SystemTypeServiceMap> GetAllSystemTypeServiceMap()
        {
            DataTable dt = _SystemTypeServiceMapDataAccess.GetAllSystemTypeServiceMap();

            List<SystemTypeServiceMap> systemTypeServiceMap = new List<SystemTypeServiceMap>();
            systemTypeServiceMap = (from DataRow dr in dt.Rows
                                    select new SystemTypeServiceMap()
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        SystemTypeId = dr["SystemTypeId"] != DBNull.Value ? Convert.ToInt32(dr["SystemTypeId"]) : 0,
                                        EquipmentId = dr["EquipmentId"] != DBNull.Value ? (Guid)(dr["EquipmentId"]) : new Guid(),
                                        PackageId = dr["PackageId"] != DBNull.Value ? (Guid)(dr["PackageId"]) : new Guid(),
                                        SystemType = dr["SystemType"].ToString(),
                                        PackageName = dr["PackageName"].ToString(),
                                        ServiceName = dr["ServiceName"].ToString(),
                                        Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0.0
                                    }).ToList();
            return systemTypeServiceMap;
        }
        public bool InsertSystemTypeServiceMap(SystemTypeServiceMap model)
        {
            return _SystemTypeServiceMapDataAccess.Insert(model) > 0;
        }

        public bool DeleteSystemTypeServiceMap(int smartSystemTypeServiceId)
        {
            return _SystemTypeServiceMapDataAccess.Delete(smartSystemTypeServiceId) > 0;
        }

        #endregion

        #region AddPrefix


        #endregion

        public CustomerPackageService GetCustomerPackageServiceById(int value)
        {
            return _CustomerPackageServiceDataAccess.Get(value);
        }

        public bool UpdateCustomerPackageService(CustomerPackageService cps)
        {
            return _CustomerPackageServiceDataAccess.Update(cps) > 0;
        }

        public CustomerPackageEqp GetCustomerPackageEqpById(int value)
        {
            return _CustomerPackageEqpDataAccess.Get(value);
        }
        public List<CustomerPackageEqp> GetCustomerPackageEqpByCustomerIdServiceId(Guid CustomerId, Guid ServiceId)
        {
            string query = string.Format("CustomerId='{0}' And ServiceId='{1}'", CustomerId, ServiceId);
            return _CustomerPackageEqpDataAccess.GetByQuery(query);
        }
        public bool UpdateCustomerPackageEqp(CustomerPackageEqp cpe)
        {
            return _CustomerPackageEqpDataAccess.Update(cpe) > 0;
        }

        public List<CustomerPackageEqp> GetAllPackageEqpListByCustomerIdAndCompanyIdAndAppointmentId(Guid customerid, Guid companyid, int appointmentId)
        {
            return _CustomerPackageEqpDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}' and AppointmentIntId = {2}", customerid, companyid, appointmentId)).ToList();
        }

        public List<CustomerPackageService> GetAllPackageServiceListByCustomerIdAndCompanyIdAndAppointmentId(Guid customerid, Guid companyid, int appointmentId)
        {
            return _CustomerPackageServiceDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}' and AppointmentIntId = {2}", customerid, companyid, appointmentId)).ToList();
        }

        public SmartPackageSystemInstallTypeMap GetSmartPackageBySystemTypeIdAndInstallIdAndManufacturerId(int typeid, int installtypeid, Guid manuid, string usertype)
        {
            DataTable dt = _SmartPackageDataAccess.GetSmartPackageBySystemTypeIdAndInstallIdAndManufacturerId(typeid, installtypeid, manuid, usertype);

            SmartPackageSystemInstallTypeMap SmartPackageSystemInstallTypeMap = new SmartPackageSystemInstallTypeMap();
            SmartPackageSystemInstallTypeMap = (from DataRow dr in dt.Rows
                                            select new SmartPackageSystemInstallTypeMap()
                                            {
                                                PackageId = (Guid)dr["PackageId"]
                                            }).FirstOrDefault();
            return SmartPackageSystemInstallTypeMap;
        }
    }
}
