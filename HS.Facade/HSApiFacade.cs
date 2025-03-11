using HS.DataAccess;
using HS.Entities;
using HS.Entities.Custom;
using HS.Entities.List;
using HS.Framework;
using HS.Framework.Utils;
using HS.SMS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace HS.Facade
{
    public class HSApiFacade : BaseFacade
    {
        private readonly EmployeeDataAccess _EmployeeDataAccess; 
        private readonly CustomerDataAccess _customerDataAccess;
        private readonly SMSHistoryDataAccess _SMSHistoryDataAccess;
        private readonly CustomerAppointmentEquipmentDataAccess _CustomerAppointmentEquipmentDataAccess; 
        private readonly CustomerPackageServiceDataAccess _CustomerPackageServiceDataAccess;
        private readonly CustomerExtendedDataAccess _CustomerExtendedDataAccess;
        private readonly InvoiceDataAccess _InvoiceDataAccess;
        private readonly UserLoginDataAccess _UserLoginDataAccess;
        private readonly CustomerCompanyDataAccess _CustomerCompanyDataAccess;
        private readonly CustomerNoteDataAccess _CustomerNoteDataAccess;
        private readonly ActivityDataAccess _ActivityDataAccess;
        private readonly NotificationDataAccess _NotificationDataAccess;
        private readonly NotificationUserDataAccess _NotificationUserDataAccess;
        private readonly CustomerFileDataAccess _CustomerFileDataAccess;
        private readonly CustomerInspectionDataAccess _CustomerInspectionDataAccess;
        private readonly CompanyDataAccess _CompanyDataAccess;
        private readonly UserCompanyDataAccess _UserCompanyDataAccess;
        private readonly InvoiceDetailDataAccess _InvoiceDetailDataAccess;
        private readonly LookupDataAccess _LookupDataAccess;
        private readonly GlobalSettingDataAccess _GlobalSettingDataAccess;
        private readonly CustomerSnapshotDataAccess _CustomerSnapshotDataAccess;
        private readonly CompanyBranchDataAccess _CompanyBranchDataAccess;
        private readonly EstimateImageDataAccess _EstimateImageDataAccess;
        private readonly EmailTemplateDataAccess _EmailTemplateDataAccess;
        private readonly EmailHistoryDataAccess _EmailHistoryDataAccess;
        private readonly OrganizationDataAccess _OrganizationDataAccess;
        private readonly LeadCorrespondenceDataAccess _LeadCorrespondenceDataAccess;
        private readonly EquipmentsFavouriteDataAccess _EquipmentsFavouriteDataAccess;
        private readonly EmployeeTimeClockDataAccess _employeeTimeClockDataAccess;
        private readonly UserCompanyDeviceDataAccess _UserCompanyDeviceDataAccess;
        private readonly ResturantOrderDataAccess _ResturantOrderDataAccess;
        private readonly WebsiteLocationDataAccess _WebsiteLocationDataAccess;
        private readonly PackageDataAccess _PackageDataAccess;
        private readonly ShortUrlDataAccess _ShortUrlDataAccess;
        private readonly BookingDataAccess _BookingDataAccess;
        private readonly BookingExtraItemDataAccess _BookingExtraItemDataAccess;
        private readonly BookingDetailsDataAccess _BookingDetailsDataAccess;
        private readonly ResturantOrderDetailDataAccess _ResturantOrderDetailDataAccess;
        private readonly CustomerRouteDataAccess _CustomerRouteDataAccess;
        private readonly CustomerCheckLogDataAccess _CustomerCheckLogDataAccess;
        private readonly TrackingNumberSettingDataAccess _TrackingNumberSettingDataAccess;
        private readonly CustomerPackageEqpDataAccess _CustomerPackageEqpDataAccess; 
        private readonly TicketDataAccess _TicketDataAccess;
        private readonly TicketUserDataAccess _TicketUserDataAccess;
        private readonly TicketReplyDataAccess _TicketReplyDataAccess;
        private readonly TicketFileDataAccess _TicketFileDataAccess;
        private readonly TicketBookingDetailsDataAccess _TicketBookingDetailsDataAccess;
        private readonly CustomerAppointmentDataAccess _CustomerAppointmentDataAccess;
        private readonly UserActivityDataAccess _UserActivityDataAccess;
        private readonly UserActivityCustomerDataAccess _UserActivityCustomerDataAccess;
        private readonly RestaurantCouponsDataAccess _RestaurantCouponsDataAccess;
        private readonly CustomSurveyDataAccess _CustomSurveyDataAccess;
        private readonly CustomSurveyUserDataAccess _CustomSurveyUserDataAccess; 
        private readonly TransactionDataAccess _TransactionDataAccess;
        private readonly TransactionQueueDataAccess _TransactionQueueDataAccess;
        private readonly TransactionHistoryDataAccess _TransactionHistoryDataAccess;
        private readonly PackageCustomerDataAccess _PackageCustomerDataAccess; 
        private readonly ContractAgreementTemplateDataAccess _ContractAgreementTemplateDataAccess; 
        private readonly CustomerSignatureDataAccess _CustomerSignatureDataAccess; 
        private readonly CustomerAgreementDataAccess _CustomerAgreementDataAccess; 
        private readonly CustomerProratedBillDataAccess _CustomerProratedBillDataAccess; 
        private readonly EstimatorPDFFilterDataAccess _EstimatorPDFFilterDataAccess;
        private readonly EstimatorDataAccess _EstimatorDataAccess; 
        private readonly EstimatorDetailDataAccess _EstimatorDetailDataAccess; 
        private readonly EstimatorServiceDataAccess _EstimatorServiceDataAccess; 
        private readonly ManufacturerDataAccess _ManufacturerDataAccess; 
        private readonly EquipmentFileDataAccess _EquipmentFileDataAccess; 
        private readonly EquipmentDataAccess _EquipmentDataAccess; 
        private readonly PaymentInfoCustomerDataAccess _PaymentInfoCustomerDataAccess;
        private readonly SmartInstallTypeDataAccess _SmartInstallTypeDataAccess; 
        private readonly SmartPackageEquipmentServiceDataAccess _SmartPackageEquipmentServiceDataAccess;
        private readonly PaymentInfoDataAccess _PaymentInfoDataAccess;
        private readonly EmergencyContactDataAccess _EmergencyContactDataAccess;
        private readonly PaymentProfileCustomerDataAccess _PaymentProfileCustomerDataAccess;
        private readonly AgreementAnswerDataAccess _AgreementAnswerDataAccess;
        private readonly CustomerAgreementTemplateDataAccess _CustomerAgreementTemplateDataAccess; 
        private readonly CustomerAddendumDataAccess _CustomerAddendumDataAccess;
        private readonly SMSTemplateDataAccess _SMSTemplateDataAccess;
        private readonly CustomerSecurityZonesDataAccess _CustomerSecurityZonesDataAccess;
        private readonly CustomerThirdPartyAgencyDataAccess _CustomerThirdPartyAgencyDataAccess;
        private readonly SmartPackageDataAccess _SmartPackageDataAccess;
        private readonly CustomerSystemNoDataAccess _CustomerSystemNoDataAccess;
        private readonly CustomerNoPrefixDataAccess _CustomerNoPrefixDataAccess;
        private readonly ThirdPartyCustomerDataAccess _ThirdPartyCustomerDataAccess;
        private readonly InventoryTechDataAccess _InventoryTechDataAccess;
        private readonly PayrollBrinksDataAccess _PayrollBrinksDataAccess;
        private readonly AlarmCustomerSelectedAddonDataAccess _AlarmCustomerSelectedAddonDataAccess;
        private readonly SetupAlarmDataAccess _SetupAlarmDataAccess;
        private readonly AlarmAddOnnsDataAccess _AlarmAddOnnsDataAccess;
        private readonly AlarmCustomerTerminationDataAccess _TerminationDataAccess;



        public List<Invoice> EqupmentList { get; private set; }
        public object ViewBag { get; private set; }
        public object User { get; private set; }
        public string ConnectionString = "";
        public string MasterConnectionString = "";
        public HSApiFacade(ClientContext clientContext) : base(clientContext)
        {
            
        }
        public HSApiFacade()
        {
            
        }
        public HSApiFacade(string connectionString,string MasterConnectionString = "")
        {
            this.ConnectionString = connectionString;
            this.MasterConnectionString = MasterConnectionString;
            if (_OrganizationDataAccess == null)
                _OrganizationDataAccess = new OrganizationDataAccess(MasterConnectionString);
            if (_EmployeeDataAccess == null)
                _EmployeeDataAccess = new EmployeeDataAccess(connectionString);
            if (_customerDataAccess == null)
                _customerDataAccess = new CustomerDataAccess(connectionString);
            if (_SMSHistoryDataAccess == null)
                _SMSHistoryDataAccess = new SMSHistoryDataAccess(connectionString);
            if (_CustomSurveyDataAccess == null)
                _CustomSurveyDataAccess = new CustomSurveyDataAccess(connectionString);
            if (_CustomerAppointmentEquipmentDataAccess == null)
                _CustomerAppointmentEquipmentDataAccess = new CustomerAppointmentEquipmentDataAccess(connectionString);
            if (_CustomerPackageServiceDataAccess == null)
                _CustomerPackageServiceDataAccess = new CustomerPackageServiceDataAccess(connectionString);
            if (_CustomerExtendedDataAccess == null)
                _CustomerExtendedDataAccess = new CustomerExtendedDataAccess(connectionString);
            if (_InvoiceDataAccess == null)
                _InvoiceDataAccess = new InvoiceDataAccess(connectionString);
            if (_UserLoginDataAccess == null)
                _UserLoginDataAccess = new UserLoginDataAccess(connectionString);
            if (_CustomerCompanyDataAccess == null)
                _CustomerCompanyDataAccess = new CustomerCompanyDataAccess(connectionString);
            if (_CustomerNoteDataAccess == null)
                _CustomerNoteDataAccess = new CustomerNoteDataAccess(connectionString);
            if (_ActivityDataAccess == null)
                _ActivityDataAccess = new ActivityDataAccess(connectionString);
            if (_NotificationDataAccess == null)
                _NotificationDataAccess = new NotificationDataAccess(connectionString);
            if (_NotificationUserDataAccess == null)
                _NotificationUserDataAccess = new NotificationUserDataAccess(connectionString);
            if (_CustomerFileDataAccess == null)
                _CustomerFileDataAccess = new CustomerFileDataAccess(connectionString);
            if (_CustomerInspectionDataAccess == null)
                _CustomerInspectionDataAccess = new CustomerInspectionDataAccess(connectionString);
            if (_CompanyDataAccess == null)
                _CompanyDataAccess = new CompanyDataAccess(connectionString);
            if (_UserCompanyDataAccess == null)
                _UserCompanyDataAccess = new UserCompanyDataAccess(connectionString);
            if (_InvoiceDetailDataAccess == null)
                _InvoiceDetailDataAccess = new InvoiceDetailDataAccess(connectionString);
            if (_LookupDataAccess == null)
                _LookupDataAccess = new LookupDataAccess(connectionString);
            if (_GlobalSettingDataAccess == null)
                _GlobalSettingDataAccess = new GlobalSettingDataAccess(connectionString);
            if (_CustomerSnapshotDataAccess == null)
                _CustomerSnapshotDataAccess = new CustomerSnapshotDataAccess(connectionString);
            if (_CompanyBranchDataAccess == null)
                _CompanyBranchDataAccess = new CompanyBranchDataAccess(connectionString);
            if (_EstimateImageDataAccess == null)
                _EstimateImageDataAccess = new EstimateImageDataAccess(connectionString);
            if (_EmailTemplateDataAccess == null)
                _EmailTemplateDataAccess = new EmailTemplateDataAccess(connectionString);
            if (_EmailHistoryDataAccess == null)
                _EmailHistoryDataAccess = new EmailHistoryDataAccess(connectionString);
            if (_LeadCorrespondenceDataAccess == null)
                _LeadCorrespondenceDataAccess = new LeadCorrespondenceDataAccess(connectionString);
            if (_EquipmentsFavouriteDataAccess == null)
                _EquipmentsFavouriteDataAccess = new EquipmentsFavouriteDataAccess(connectionString);
            if (_employeeTimeClockDataAccess == null)
                _employeeTimeClockDataAccess = new EmployeeTimeClockDataAccess(connectionString);
            if (_UserCompanyDeviceDataAccess == null)
                _UserCompanyDeviceDataAccess = new UserCompanyDeviceDataAccess(connectionString);
            if (_ResturantOrderDataAccess == null)
                _ResturantOrderDataAccess = new ResturantOrderDataAccess(connectionString);
            if (_WebsiteLocationDataAccess == null)
                _WebsiteLocationDataAccess = new WebsiteLocationDataAccess(connectionString);
            if (_PackageDataAccess == null)
                _PackageDataAccess = new PackageDataAccess(connectionString);
            if (_ShortUrlDataAccess == null)
                _ShortUrlDataAccess = new ShortUrlDataAccess(System.Configuration.ConfigurationManager.ConnectionStrings["hsCoonectionString"].ConnectionString);
            if (_BookingDataAccess == null)
                _BookingDataAccess = new BookingDataAccess(connectionString);
            if (_BookingExtraItemDataAccess == null)
                _BookingExtraItemDataAccess = new BookingExtraItemDataAccess(connectionString);
            if (_BookingDetailsDataAccess == null)
                _BookingDetailsDataAccess = new BookingDetailsDataAccess(connectionString);
            if (_ResturantOrderDetailDataAccess == null)
                _ResturantOrderDetailDataAccess = new ResturantOrderDetailDataAccess(connectionString);
            if (_CustomerRouteDataAccess == null)
                _CustomerRouteDataAccess = new CustomerRouteDataAccess(connectionString);
            if (_TicketDataAccess == null)
                _TicketDataAccess = new TicketDataAccess(connectionString);
            if (_CustomerAppointmentDataAccess == null)
                _CustomerAppointmentDataAccess = new CustomerAppointmentDataAccess(connectionString);
            if (_TicketReplyDataAccess == null)
                _TicketReplyDataAccess = new TicketReplyDataAccess(connectionString);
            if (_TicketFileDataAccess == null)
                _TicketFileDataAccess = new TicketFileDataAccess(connectionString);
            if (_TicketUserDataAccess == null)
                _TicketUserDataAccess = new TicketUserDataAccess(connectionString);
            if (_CustomerCheckLogDataAccess == null)
                _CustomerCheckLogDataAccess = new CustomerCheckLogDataAccess(connectionString);
            if (_TicketBookingDetailsDataAccess == null)
                _TicketBookingDetailsDataAccess = new TicketBookingDetailsDataAccess(connectionString);
            if (_CustomerPackageEqpDataAccess == null)
                _CustomerPackageEqpDataAccess = new CustomerPackageEqpDataAccess(connectionString);
            if (_TrackingNumberSettingDataAccess == null)
                _TrackingNumberSettingDataAccess = new TrackingNumberSettingDataAccess(connectionString);
            if (_RestaurantCouponsDataAccess == null)
                _RestaurantCouponsDataAccess = new RestaurantCouponsDataAccess(connectionString);
            if (_CustomSurveyUserDataAccess == null)
                _CustomSurveyUserDataAccess = new CustomSurveyUserDataAccess(connectionString);
            if (_TransactionDataAccess == null)
                _TransactionDataAccess = new TransactionDataAccess(connectionString);
            if (_TransactionQueueDataAccess == null)
                _TransactionQueueDataAccess = new TransactionQueueDataAccess(connectionString);
            if (_TransactionHistoryDataAccess == null)
                _TransactionHistoryDataAccess = new TransactionHistoryDataAccess(connectionString);
            if (_PackageCustomerDataAccess == null)
                _PackageCustomerDataAccess = new PackageCustomerDataAccess(connectionString);

            if (_ContractAgreementTemplateDataAccess == null)
                _ContractAgreementTemplateDataAccess = new ContractAgreementTemplateDataAccess(connectionString);
            if (_CustomerSignatureDataAccess == null)
                _CustomerSignatureDataAccess = new CustomerSignatureDataAccess(connectionString);
            if (_CustomerAgreementDataAccess == null)
                _CustomerAgreementDataAccess = new CustomerAgreementDataAccess(connectionString);
            if (_CustomerProratedBillDataAccess == null)
                _CustomerProratedBillDataAccess = new CustomerProratedBillDataAccess(connectionString);
            if (_EstimatorDataAccess == null)
                _EstimatorDataAccess = new EstimatorDataAccess(connectionString);

            if (_EstimatorPDFFilterDataAccess == null)
                _EstimatorPDFFilterDataAccess = new EstimatorPDFFilterDataAccess(connectionString);
            if (_EstimatorDetailDataAccess == null)
                _EstimatorDetailDataAccess = new EstimatorDetailDataAccess(connectionString);
            if (_EstimatorServiceDataAccess == null)
                _EstimatorServiceDataAccess = new EstimatorServiceDataAccess(connectionString);
            if (_ManufacturerDataAccess == null)
                _ManufacturerDataAccess = new ManufacturerDataAccess(connectionString);
            if (_EquipmentFileDataAccess == null)
                _EquipmentFileDataAccess = new EquipmentFileDataAccess(connectionString);
            if (_EquipmentDataAccess == null)
                _EquipmentDataAccess = new EquipmentDataAccess(connectionString);
            if (_PaymentInfoCustomerDataAccess == null)
                _PaymentInfoCustomerDataAccess = new PaymentInfoCustomerDataAccess(connectionString);
            if (_SmartInstallTypeDataAccess == null)
                _SmartInstallTypeDataAccess = new SmartInstallTypeDataAccess(connectionString);
            if (_SmartPackageEquipmentServiceDataAccess == null)
                _SmartPackageEquipmentServiceDataAccess = new SmartPackageEquipmentServiceDataAccess(connectionString);
            if (_PaymentInfoDataAccess == null)
                _PaymentInfoDataAccess = new PaymentInfoDataAccess(connectionString);
            if (_EmergencyContactDataAccess == null)
                _EmergencyContactDataAccess = new EmergencyContactDataAccess(connectionString);

            if (_PaymentProfileCustomerDataAccess == null)
                _PaymentProfileCustomerDataAccess = new PaymentProfileCustomerDataAccess(connectionString);
            if (_AgreementAnswerDataAccess == null)
                _AgreementAnswerDataAccess = new AgreementAnswerDataAccess(connectionString);
            if (_CustomerAgreementTemplateDataAccess == null)
                _CustomerAgreementTemplateDataAccess = new CustomerAgreementTemplateDataAccess(connectionString);
            if (_CustomerAddendumDataAccess == null)
                _CustomerAddendumDataAccess = new CustomerAddendumDataAccess(connectionString);

            if (_SMSTemplateDataAccess == null)
                _SMSTemplateDataAccess = new SMSTemplateDataAccess(connectionString);
            if (_CustomerSecurityZonesDataAccess == null)
                _CustomerSecurityZonesDataAccess = new CustomerSecurityZonesDataAccess(connectionString);
            if (_CustomerThirdPartyAgencyDataAccess == null)
                _CustomerThirdPartyAgencyDataAccess = new CustomerThirdPartyAgencyDataAccess(connectionString);
            if (_SmartPackageDataAccess == null)
                _SmartPackageDataAccess = new SmartPackageDataAccess(connectionString);
            if (_CustomerSystemNoDataAccess == null)
                _CustomerSystemNoDataAccess = new CustomerSystemNoDataAccess(connectionString);
            if (_CustomerNoPrefixDataAccess == null)
                _CustomerNoPrefixDataAccess = new CustomerNoPrefixDataAccess(connectionString);
            if (_ThirdPartyCustomerDataAccess == null)
                _ThirdPartyCustomerDataAccess = new ThirdPartyCustomerDataAccess(connectionString);
            if (_InventoryTechDataAccess == null)
                _InventoryTechDataAccess = new InventoryTechDataAccess(connectionString);
            if (_PayrollBrinksDataAccess == null)
                _PayrollBrinksDataAccess = new PayrollBrinksDataAccess(connectionString);
            if (_AlarmCustomerSelectedAddonDataAccess == null)
                _AlarmCustomerSelectedAddonDataAccess = new AlarmCustomerSelectedAddonDataAccess(connectionString);
            if (_SetupAlarmDataAccess == null)
                _SetupAlarmDataAccess = new SetupAlarmDataAccess(connectionString);
            if (_AlarmAddOnnsDataAccess == null)
                _AlarmAddOnnsDataAccess = new AlarmAddOnnsDataAccess(connectionString);
            if (_TerminationDataAccess == null)
                _TerminationDataAccess = new AlarmCustomerTerminationDataAccess(connectionString);
             
        }

     
        public Employee GetEmployeeByEmailAddress(string EmailAddress)
        {
            return _EmployeeDataAccess.GetByQuery(string.Format(" Email ='{0}' ", EmailAddress)).FirstOrDefault();
        }
        public CustomerCompany GetCustomerCompanyByCompanyIdAndCustomerId(Guid companyid, Guid customerid)
        {
            return _CustomerCompanyDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and CustomerId = '{1}'", companyid, customerid)).FirstOrDefault();
        }
        public string GetAgreementDocumentHeightByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;
            var globalsetting = new GlobalSetting();
            string AgreementDocumentHeight = RMRCacheKey.AgreementDocumentHeightLoad + CompanyId.ToString();
            if (System.Web.HttpRuntime.Cache[AgreementDocumentHeight] == null)
            {
                string DataKey = "AgreementDocumentHeight";
                globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    result = globalsetting.Value;
                    System.Web.HttpRuntime.Cache[AgreementDocumentHeight] = result;
                }
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[AgreementDocumentHeight];
            }
            return result;
        }
        public CustomerSignature GetFirstPageCustomerSignatureByCustomerId(Guid customerid)
        {
            return _CustomerSignatureDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and [Type] = 'First Page'", customerid)).FirstOrDefault();
        }
        public bool UpdateCustomerSignature(CustomerSignature cs)
        {
            return _CustomerSignatureDataAccess.Update(cs) > 0;
        }
        public CustomerSignature GetCommercialCustomerSignatureByCustomerId(Guid customerid)
        {
            return _CustomerSignatureDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and [Type] = 'Commercial'", customerid)).FirstOrDefault();
        }
        public CustomerCompany GetCustomerCompanyByCustomerId(int customerId)
        {
            Customer cus = _customerDataAccess.Get(customerId);
            if (cus == null)
                return null;
            return _CustomerCompanyDataAccess.GetByQuery(string.Format("[CustomerId]='{0}'", cus.CustomerId.ToString())).FirstOrDefault();
        }
        public bool CustomerIsInCompany(int id, Guid CompanyId)
        {
            DataTable dt = _customerDataAccess.GetAllCustomerByIdAndCompanyId(CompanyId, id);
            Customer CustomerList = new Customer();
            CustomerList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0
                            }).ToList().FirstOrDefault();
            return CustomerList != null;

        }
        public Customer GetCustomersById(int value)
        {
            //return _CustomerDataAccess.Get(value);

            return _customerDataAccess.GetCustomersByIdAPI(value);
        }
        public CustomerAgreement GetCustomerAgreementHistory(Guid CustomerId, string HistoryType)
        {
            return _CustomerAgreementDataAccess.GetByQuery(string.Format("CustomerId='{0}' and Type='{1}'", CustomerId, HistoryType)).FirstOrDefault();
        }
        public long InsertCustomerAgreement(CustomerAgreement ca)
        {
            return _CustomerAgreementDataAccess.Insert(ca);
        }
        public CustomerAgreement GetCustomerAgreementByComIdAndCusIsAndLoadAgreement(Guid comid, Guid cusid)
        {
            return _CustomerAgreementDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and CustomerId = '{1}' and Type = 'LoadAgreement'", comid, cusid)).FirstOrDefault();
        }
        public List<CityTax> GetCityTaxRate(Guid CustomerId, Guid CompanyId)
        {
            DataTable dt = _CompanyBranchDataAccess.GetCityTaxRate(CustomerId, CompanyId);
            List<CityTax> CityTaxList = new List<CityTax>();
            CityTaxList = (from DataRow dr in dt.Rows
                           select new CityTax()
                           {
                               //Name = dr["Name"].ToString(),
                               State = dr["State"].ToString(),
                               City = dr["City"].ToString(),
                               Rate = dr["Rate"] != DBNull.Value ? Convert.ToDouble(dr["Rate"]) : 0,
                           }).ToList();
            return CityTaxList;

        }
        public GlobalSetting GetSalesTax(Guid CompanyId, Guid CustomerId)
        {
            GlobalSetting gl = new GlobalSetting();
            gl = _GlobalSettingDataAccess.GetByQuery(string.Format("SearchKey = 'Sales Tax' and CompanyId = '{0}' and IsActive = 'true'", CompanyId)).FirstOrDefault();
            if (CustomerId != new Guid())
            {
                Customer cus = _customerDataAccess.GetIndividualCustomerByCustomerId(CustomerId);
                if (cus != null)
                {
                    if (cus.TaxExemption == "Yes")
                    {
                        gl.Value = "0.00";
                    }
                }
            }
            return gl;
        }
        public Estimator GetById(int value)
        {
            return _EstimatorDataAccess.Get(value);
        }
        public EstimatorPDFFilter GetEstimatorPdfFilterByComIdCusIdUserId(Guid comid, Guid userid, Guid cusid)
        {
            return _EstimatorPDFFilterDataAccess.GetByQuery(string.Format(" CompanyId = '{0}' and CreatedBy = '{1}' and CustomerId = '{2}'", comid, userid, cusid)).FirstOrDefault();
        }
        public List<EstimatorDetail> GetEstimatorDetailListByEstimatorId(string estimatorId)
        {
            return _EstimatorDetailDataAccess.GetByQuery(String.Format(" EstimatorId = '{0}'", estimatorId));
        }
        public List<EstimatorService> GetEstimatorServicesByEstimatorId(string estimatorId)
        {
            return _EstimatorServiceDataAccess.GetByQuery(string.Format(" EstimatorId='{0}'", estimatorId));
        }
        public CustomerProratedBill GetCusProratedBillByCustomerId(Guid customerId)
        {
            return _CustomerProratedBillDataAccess.GetByQuery(string.Format(" CustomerId = '{0}'", customerId)).LastOrDefault();
        }
        public Invoice GetByInvoiceId(string invoiceId)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format(" InvoiceId = '{0}'", invoiceId)).FirstOrDefault();
        }
        public bool DeleteAllSignatureByType(Guid customerId, string Type)
        {
            return _CustomerSignatureDataAccess.DeleteAllSignatureByType(customerId, Type);
        }

        public UserLogin GetUserLoginByUsername(string username)
        {
            return _UserLoginDataAccess.GetByQuery(string.Format(" UserName = '{0}' ", username)).FirstOrDefault();
        }

        public PermissionGroup GetEmployeeRoleByEmployeeIdAndCompanyId(Guid EmployeeId, Guid CompanyId)
        {
            DataTable dt = _EmployeeDataAccess.GetEmployeeRoleByEmployeeIdAndCompanyId(EmployeeId, CompanyId);
            PermissionGroup EmployeeList = new PermissionGroup();
            EmployeeList = (from DataRow dr in dt.Rows
                            select new PermissionGroup()
                            {
                                Name = dr["Name"].ToString(),
                                Tag = dr["Tag"].ToString()
                            }).FirstOrDefault();
            return EmployeeList;
        }
        public List<Partner> GetEmployeeByPartnerId(Guid SupervisorId)
        {
            DataTable dt = _EmployeeDataAccess.GetEmployeeByPartnerId(SupervisorId);
            List<Partner> PartnerList = new List<Partner>();
            PartnerList = (from DataRow dr in dt.Rows
                           select new Partner()
                           {
                               UserId = (Guid)dr["UserId"],
                               SupervisorId = new Guid(dr["SupervisorId"].ToString()),
                               FirstName = dr["FirstName"].ToString(),
                               LastName = dr["LastName"].ToString()
                           }).ToList();
            return PartnerList;
        }
        public CustomerListWithCountModel GetCustomerByFilter(CustomerFilter filter)
        {
            DataSet ds = _customerDataAccess.GetCustomerListByFilterAPI(filter);

            List<Customer> CustomerList = new List<Customer>();
            CustomerList = (from DataRow dr in ds.Tables[0].Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                DisplayName = dr["DisplayName"].ToString(),
                                CustomerNo = dr["CustomerNo"].ToString(),
                                Title = dr["Title"].ToString(),
                                SSN = dr["SSN"].ToString(),
                                CellNo = dr["CellNo"].ToString(),
                                Fax = dr["Fax"].ToString(),
                                CallingTime = dr["CallingTime"].ToString(),
                                Address2 = dr["Address2"].ToString(),
                                Country = dr["Country"].ToString(),
                                StreetPrevious = dr["StreetPrevious"].ToString(),
                                CityPrevious = dr["CityPrevious"].ToString(),
                                StatePrevious = dr["StatePrevious"].ToString(),
                                ZipCodePrevious = dr["ZipCodePrevious"].ToString(),
                                CountryPrevious = dr["CountryPrevious"].ToString(),
                                AccountNo = dr["AccountNo"].ToString(),
                                IsAlarmCom = dr["IsAlarmCom"] != DBNull.Value ? Convert.ToBoolean(dr["IsAlarmCom"]) : false,
                                CreditScore = dr["CreditScore"].ToString(),
                                CreditScoreValue = dr["CreditScoreValue"] != DBNull.Value ? Convert.ToInt32(dr["CreditScoreValue"]) : 0,
                                ContractTeam = dr["ContractTeam"].ToString(),
                                FundingCompany = dr["FundingCompany"].ToString(),
                                MonthlyMonitoringFee = dr["MonthlyMonitoringFee"].ToString(),
                                CellularBackup = dr["CellularBackup"] != DBNull.Value ? Convert.ToBoolean(dr["CellularBackup"]) : false,
                                CustomerFunded = dr["CustomerFunded"] != DBNull.Value ? Convert.ToBoolean(dr["CustomerFunded"]) : false,
                                Maintenance = dr["Maintenance"] != DBNull.Value ? Convert.ToBoolean(dr["Maintenance"]) : false,
                                Note = dr["Note"].ToString(),
                                SalesDate = dr["SalesDate"] != DBNull.Value ? Convert.ToDateTime(dr["SalesDate"]) : new DateTime(),
                                FollowUpDate = dr["FollowUpDate"] != DBNull.Value ? Convert.ToDateTime(dr["FollowUpDate"]) : new DateTime(),
                                InstallDate = dr["InstallDate"] != DBNull.Value ? Convert.ToDateTime(dr["InstallDate"]) : new DateTime(),
                                CutInDate = dr["CutInDate"] != DBNull.Value ? Convert.ToDateTime(dr["CutInDate"]) : new DateTime(),
                                Installer = dr["Installer"].ToString(),
                                Soldby = dr["Soldby"].ToString(),
                                FundingDate = dr["FundingDate"] != DBNull.Value ? Convert.ToDateTime(dr["FundingDate"]) : new DateTime(),
                                ReminderDate = dr["ReminderDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReminderDate"]) : new DateTime(),
                                QA1 = dr["QA1"].ToString(),
                                QA1Date = dr["QA1Date"] != DBNull.Value ? Convert.ToDateTime(dr["QA1Date"]) : new DateTime(),
                                QA2 = dr["QA2"].ToString(),
                                QA2Date = dr["QA2Date"] != DBNull.Value ? Convert.ToDateTime(dr["QA2Date"]) : new DateTime(),
                                BillAmount = dr["BillAmount"] != DBNull.Value ? Convert.ToDouble(dr["BillAmount"]) : 0.0,
                                PaymentMethod = dr["PaymentMethod"].ToString(),
                                BillCycle = dr["BillCycle"].ToString(),
                                BillDay = dr["BillDay"] != DBNull.Value ? Convert.ToInt32(dr["BillDay"]) : 0,
                                BillNotes = dr["BillNotes"].ToString(),
                                BillTax = dr["BillTax"] != DBNull.Value ? Convert.ToBoolean(dr["BillTax"]) : false,
                                BillOutStanding = dr["BillOutStanding"] != DBNull.Value ? Convert.ToDouble(dr["BillOutStanding"]) : 0.0,
                                ServiceDate = dr["ServiceDate"] != DBNull.Value ? Convert.ToDateTime(dr["ServiceDate"]) : new DateTime(),
                                Area = dr["Area"].ToString(),
                                Latlng = dr["Latlng"].ToString(),
                                SecondCustomerNo = dr["SecondCustomerNo"].ToString(),
                                AdditionalCustomerNo = dr["AdditionalCustomerNo"].ToString(),
                                IsTechCallPassed = dr["IsTechCallPassed"] != DBNull.Value ? Convert.ToBoolean(dr["IsTechCallPassed"]) : false,
                                IsDirect = dr["IsDirect"] != DBNull.Value ? Convert.ToBoolean(dr["IsDirect"]) : false,
                                AuthorizeRefId = dr["AuthorizeRefId"].ToString(),
                                AuthorizeCusProfileId = dr["AuthorizeCusProfileId"].ToString(),
                                AuthorizeCusPaymentProfileId = dr["AuthorizeCusPaymentProfileId"].ToString(),
                                AuthorizeDescription = dr["AuthorizeDescription"].ToString(),
                                IsRequiredCsvSync = dr["IsRequiredCsvSync"] != DBNull.Value ? Convert.ToBoolean(dr["IsRequiredCsvSync"]) : false,
                                Passcode = dr["Passcode"].ToString(),
                                ActivationFee = dr["ActivationFee"] != DBNull.Value ? Convert.ToDouble(dr["ActivationFee"]) : 0.0,
                                FirstBilling = dr["FirstBilling"] != DBNull.Value ? Convert.ToDateTime(dr["FirstBilling"]) : new DateTime(),
                                ActivationFeePaymentMethod = dr["ActivationFeePaymentMethod"].ToString(),
                                IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                                LastGeneratedInvoice = dr["LastGeneratedInvoice"] != DBNull.Value ? Convert.ToDateTime(dr["LastGeneratedInvoice"]) : new DateTime(),
                                Singature = dr["Singature"].ToString(),
                                CrossStreet = dr["CrossStreet"].ToString(),
                                DBA = dr["DBA"].ToString(),
                                AlarmRefId = dr["AlarmRefId"].ToString(),
                                TransunionRefId = dr["TransunionRefId"].ToString(),
                                MonitronicsRefId = dr["MonitronicsRefId"].ToString(),
                                CentralStationRefId = dr["CentralStationRefId"].ToString(),
                                CmsRefId = dr["CmsRefId"].ToString(),
                                PreferedEmail = dr["PreferedEmail"] != DBNull.Value ? Convert.ToBoolean(dr["PreferedEmail"]) : false,
                                PreferedSms = dr["PreferedSms"] != DBNull.Value ? Convert.ToBoolean(dr["PreferedSms"]) : false,
                                IsAgreement = dr["IsAgreement"] != DBNull.Value ? Convert.ToBoolean(dr["IsAgreement"]) : false,
                                IsFireAccount = dr["IsFireAccount"] != DBNull.Value ? Convert.ToBoolean(dr["IsFireAccount"]) : false,
                                CreatedByUid = (Guid)dr["CreatedByUid"],
                                CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                LastUpdatedByUid = (Guid)dr["LastUpdatedByUid"],
                                BusinessAccountType = dr["BusinessAccountType"].ToString(),
                                PhoneType = dr["PhoneType"].ToString(),
                                Carrier = dr["Carrier"].ToString(),
                                ReferringCustomer = (Guid)dr["ReferringCustomer"],
                                EsistingPanel = dr["EsistingPanel"].ToString(),
                                Ownership = dr["Ownership"].ToString(),
                                PurchasePrice = dr["PurchasePrice"] != DBNull.Value ? Convert.ToDouble(dr["PurchasePrice"]) : 0.0,
                                ContractValue = dr["ContractValue"].ToString(),
                                ChildOf = (Guid)dr["ChildOf"],
                                EmailVerified = dr["EmailVerified"] != DBNull.Value ? Convert.ToBoolean(dr["EmailVerified"]) : false,
                                HomeVerified = dr["HomeVerified"] != DBNull.Value ? Convert.ToBoolean(dr["HomeVerified"]) : false,
                                County = dr["County"].ToString(),
                                CustomerToken = dr["CustomerToken"].ToString(),
                                PaymentToken = dr["PaymentToken"].ToString(),
                                ScheduleToken = dr["ScheduleToken"].ToString(),
                                EstCloseDate = dr["EstCloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["EstCloseDate"]) : new DateTime(),
                                ProjectWalkDate = dr["ProjectWalkDate"] != DBNull.Value ? Convert.ToDateTime(dr["ProjectWalkDate"]) : new DateTime(),
                                BranchId = dr["BranchId"] != DBNull.Value ? Convert.ToInt32(dr["BranchId"]) : 0,
                                SubscriptionStatus = dr["SubscriptionStatus"].ToString(),
                                AnnualRevenue = dr["AnnualRevenue"] != DBNull.Value ? Convert.ToDouble(dr["AnnualRevenue"]) : 0.0,
                                Website = dr["Website"].ToString(),
                                Market = dr["Market"].ToString(),
                                Passengers = dr["Passengers"] != DBNull.Value ? Convert.ToInt32(dr["Passengers"]) : 0,
                                Budget = dr["Budget"] != DBNull.Value ? Convert.ToDouble(dr["Budget"]) : 0.0,
                                SmartSetUpStep = dr["SmartSetUpStep"] != DBNull.Value ? Convert.ToInt32(dr["SmartSetUpStep"]) : 0,
                                CustomerAccountType = dr["CustomerAccountType"].ToString(),
                                IsPrimaryPhoneVerified = dr["IsPrimaryPhoneVerified"] != DBNull.Value ? Convert.ToBoolean(dr["IsPrimaryPhoneVerified"]) : false,
                                IsSecondaryPhoneVerified = dr["IsSecondaryPhoneVerified"] != DBNull.Value ? Convert.ToBoolean(dr["IsSecondaryPhoneVerified"]) : false,
                                IsCellNoVerified = dr["IsCellNoVerified"] != DBNull.Value ? Convert.ToBoolean(dr["IsCellNoVerified"]) : false,
                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                MiddleName = dr["MiddleName"].ToString(),
                                BusinessName = dr["BusinessName"].ToString(),
                                Status = dr["Status"].ToString(),
                                Street = dr["Street"].ToString(),
                                City = dr["City"].ToString().UppercaseFirst(),
                                State = dr["State"].ToString(),
                                ZipCode = dr["ZipCode"].ToString(),
                                Address = dr["Address"].ToString(),
                                EmailAddress = dr["EmailAddress"].ToString(),
                                PrimaryPhone = dr["PrimaryPhone"].ToString(),
                                SecondaryPhone = dr["SecondaryPhone"].ToString(),
                                CustomerId = (Guid)dr["CustomerId"],
                                DateofBirth = dr["DateofBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DateofBirth"]) : new DateTime(),
                                JoinDate = dr["JoinDate"] != DBNull.Value ? Convert.ToDateTime(dr["JoinDate"]) : new DateTime(),
                                LeadSource = dr["LeadSource"].ToString(),
                                LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                                StreetType = dr["StreetType"].ToString(),
                                Appartment = dr["Appartment"].ToString(),
                                Type = dr["Type"].ToString(),
                                TechnicianName = dr["TechnicianName"].ToString(),
                                NameFile = dr["NameFile"].ToString(),
                                PersonSales = dr["PersonSales"].ToString(),
                                UnpaidInvoiceTotal = (dr["UnpaidInvoiceTotal"] != DBNull.Value ? Convert.ToDouble(dr["UnpaidInvoiceTotal"]) : 0.0),
                                CreatedByVal = dr["CreatedByVal"].ToString(),
                                DoNotCall = dr["DoNotCall"] != DBNull.Value ? Convert.ToDateTime(dr["DoNotCall"]) : new DateTime(),
                                PreferredContactMethod = dr["PreferredContactMethod"].ToString(),
                                StatusVal = dr["StatusVal"].ToString(),
                                MovingDate = dr["MovingDate"] != DBNull.Value ? Convert.ToDateTime(dr["MovingDate"]) : new DateTime(),
                                ContactedPerviously = dr["ContactedPerviously"].ToString() 
                                 
                            }).ToList();

            TotalCustomerCount TotalCustomer = new TotalCustomerCount();
            TotalCustomer = (from DataRow dr in ds.Tables[1].Rows
                             select new TotalCustomerCount()
                             {
                                 Counter = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                             }).FirstOrDefault();

            CustomerListWithCountModel CustomerResultModel = new CustomerListWithCountModel();
            CustomerResultModel.CustomerList = CustomerList;
            CustomerResultModel.TotalCustomerCount = TotalCustomer;

            return CustomerResultModel;
        }
     
        public int InsertCustomerPackageService(CustomerPackageService _CustomerPackageService)
        {
            return (int)_CustomerPackageServiceDataAccess.Insert(_CustomerPackageService);
        }
        public bool DeleteCustomerPackageEqpById(int Id)
        {
            return _CustomerPackageEqpDataAccess.Delete(Id) > 0;
        }
        public bool DeleteAllCustomerAppointmentEquipmentByAppointmentId(Guid AppointmentId)
        {
            var result = _CustomerAppointmentEquipmentDataAccess.DeleteAllCustomerAppointmentEquipmentByAppointmentId(AppointmentId);
            return result;
        }
        public bool DeleteCustomerAppoinmentEquipment(int id)
        {
            return _CustomerAppointmentEquipmentDataAccess.Delete(id) > 0;
        }
        public Customer GetCustomerByPhoneNumberAndEmail(string primaryPhone, string secondaryPhone, string cellNo, string email)
        {
            return _customerDataAccess.GetCustomerByPhoneNumberAndEmail(primaryPhone, secondaryPhone, cellNo,email);
        }
        public Customer GetCustomerByPhoneNumber(string CellNo)
        {
            return _customerDataAccess.GetByQuery(string.Format("CellNo = '{0}'", CellNo)).FirstOrDefault();
        }
     
        public List<CustomerPackageEqp> GetCustomerPackageEqpAPIById(int id)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetCustomerPackageEqpAPIById(id);

            List<CustomerPackageEqp> packageEqpList = new List<CustomerPackageEqp>();
            packageEqpList = (from DataRow dr in dt.Rows
                              select new CustomerPackageEqp()
                              {
                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                  AppointmentEquipmentIntId = dr["AppointmentEquipmentIntId"] != DBNull.Value ? Convert.ToInt32(dr["AppointmentEquipmentIntId"]) : 0,
                                  Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                  UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0,
                                  Total = dr["Total"] != DBNull.Value ? Convert.ToDouble(dr["Total"]) : 0,
                                  EquipmentId = dr["EquipmentId"] != DBNull.Value ? Guid.Parse(dr["EquipmentId"].ToString()) : Guid.Empty,
                                  CompanyId = (Guid)dr["CompanyId"] , 
                                  CustomerId = (Guid)dr["CustomerId"] ,
                                  PackageId = (Guid)dr["PackageId"]

                              }).ToList();

            return packageEqpList;
        }
        public List<CustomerPackageService> GetCustomerPackageServiceAPIById(int id)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetCustomerPackageServiceAPIById(id);

            List<CustomerPackageService> packageServiceList = new List<CustomerPackageService>();
            packageServiceList = (from DataRow dr in dt.Rows
                              select new CustomerPackageService()
                              {
                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                  AppointmentEquipmentIntId = dr["AppointmentEquipmentIntId"] != DBNull.Value ? Convert.ToInt32(dr["AppointmentEquipmentIntId"]) : 0,
                                  MonthlyRate = dr["MonthlyRate"] != DBNull.Value ? Convert.ToDouble(dr["MonthlyRate"]) : 0,
                                  Total = dr["Total"] != DBNull.Value ? Convert.ToDouble(dr["Total"]) : 0,
                                  EquipmentId = dr["EquipmentId"] != DBNull.Value ? Guid.Parse(dr["EquipmentId"].ToString()) : Guid.Empty,
                                  CompanyId = (Guid)dr["CompanyId"],
                                  CustomerId = (Guid)dr["CustomerId"],
                                  PackageId = (Guid)dr["PackageId"]
                              }).ToList();

            return packageServiceList;
        }
        public bool DeleteCustomerPackageServiceById(int Id)
        {
            return _CustomerPackageServiceDataAccess.Delete(Id) > 0;
        }
        public UserLogin GetUserLoginById(int userLoginId)
        {
            return _UserLoginDataAccess.Get(userLoginId);
        }

        public bool UpdateCustomerCompany(CustomerCompany customerCompany)
        {
            return _CustomerCompanyDataAccess.Update(customerCompany)>0;
        }

        public CustomerCompany GetCustomerCompanyByCustomerId(Guid customerId, Guid comid)
        {
            return _CustomerCompanyDataAccess.GetByQuery(string.Format(" CustomerId = '{0}' and CompanyId = '{1}'",customerId, comid)).FirstOrDefault();
        }

        public Employee GetEmployeeByUsername(string username)
        {
            return _EmployeeDataAccess.GetByQuery(string.Format(" UserName = '{0}' ",username)).FirstOrDefault();
        }

        public EmployeeTimeClock GetEmployeeTimeClockByUserId(Guid userid)
        {
            return _employeeTimeClockDataAccess.GetByQuery(string.Format(" UserId = '{0}' ", userid)).Last();
        }

        public EstimateListWithCountModelForAPI GetEstimateListByFilter(InvoiceFilter filter)
        {
            DataSet row = _InvoiceDataAccess.GetEstimateListByFilterAPI(filter);
            
            List<InvoiceAPI> EstimateList = new List<InvoiceAPI>();
            EstimateList = (from DataRow dr in row.Tables[0].Rows
                           select new InvoiceAPI()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               InvoiceId = dr["InvoiceId"].ToString(),
                               Description = dr["Description"].ToString(),
                               CreatedBy = dr["CreatedBy"].ToString(),
                               CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                               Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0.0,
                               TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0.0,
                               EstimateEqpCount = dr["EstimateEqpCount"] != DBNull.Value ? Convert.ToInt32(dr["EstimateEqpCount"]) : 0,
                               EstimateEqpQuantity = dr["EstimateEqpQuantity"] != DBNull.Value ? Convert.ToInt32(dr["EstimateEqpQuantity"]) : 0,
                               //DrawingImage = dr["DrawingImage"].ToString(),
                               //CameraImage = dr["CameraImage"].ToString(),
                               //SignaImage = dr["SignaImage"].ToString()
                           }).ToList();
           
            TotalEstimateCount TotalEstimate = new TotalEstimateCount();
            TotalEstimate = (from DataRow dr in row.Tables[1].Rows
                             select new TotalEstimateCount()
                             {
                                 Counter = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                             }).FirstOrDefault();

            EstimateListWithCountModelForAPI EstimateResultModel = new EstimateListWithCountModelForAPI();
            EstimateResultModel.EstimateList = EstimateList;
            EstimateResultModel.TotalEstimateCount = TotalEstimate;

            return EstimateResultModel;
        }

        public List<EquipmentSearchModel> GetEquipmentListByName(string key)
        {
            DataTable dt = _InvoiceDataAccess.GetEquipmentListBySearchKeyAPI(key, 10, "");
            List<EquipmentSearchModel> EquipmentList = new List<EquipmentSearchModel>();
            EquipmentList = (from DataRow dr in dt.Rows
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
                            SKU = dr["SKU"].ToString(),
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            ManufacturerName = dr["ManufacturerName"].ToString()
                        }).ToList();

            return EquipmentList;

        }
        public int AddUserActivity(UserActivity useractivity)
        {
            return (int)_UserActivityDataAccess.Insert(useractivity);
        }
        public int AddUserActivityCustomer(UserActivityCustomer useractivityCustomer)
        {
            return (int)_UserActivityCustomerDataAccess.Insert(useractivityCustomer);
        }
        public bool SendSurveyEmail(SendSurveyEmail email)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("NAME", email.Name);
                templateVars.Add("CompanyId", email.CompanyId);
                templateVars.Add("SenderName", email.SenderName);
                templateVars.Add("ShortLink", email.shortLink);
                templateVars.Add("CompanyName", email.CompanyName);
                templateVars.Add("Subject", email.Subject);
                templateVars.Add("Header", email.Header);
                if (SentEmail(templateVars, EmailTemplateKey.SendSurveyEmail.SurveyEmail, email.CompanyId, null))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }

            return false;
        }
        public TicketReply GetTicketReplyById(int value)
        {
            return _TicketReplyDataAccess.Get(value);
        }
        public bool UpdateTicketReply(TicketReply tr)
        {
            return _TicketReplyDataAccess.Update(tr) > 0;
        }
            public long InsertTicketReply(TicketReply tr)
        {
            return _TicketReplyDataAccess.Insert(tr);
        }
        public List<ContractAgreementTemplate> GetAllContractAgreeemntTemplate()
        {
            return _ContractAgreementTemplateDataAccess.GetAll();
        }

        public bool SendSMS(Guid CompanyId, Guid SendBy, string MessageBody, List<string> ReceiverNumberList)
        {
            return SendSMSPrivate(CompanyId, SendBy, "", MessageBody, ReceiverNumberList, false, string.Empty);
        }
        public bool SendSMS(Guid CompanyId, Guid SendBy, string MessageBody, List<string> ReceiverNumberList, bool IsSystemAutoSent, string FromName)
        {
            return SendSMSPrivate(CompanyId, SendBy, "", MessageBody, ReceiverNumberList, IsSystemAutoSent, FromName);
        }

        public bool SendSMSPrivate(Guid CompanyId, Guid SendBy, string TemplateKey, string MessageBody, List<string> ReceiverNumberList, bool IsSystemAutoSent, string FromName)
        {

            #region getting autth data
            GlobalSetting aid = _GlobalSettingDataAccess.GetByQuery(string.Format("SearchKey ='{1}' and CompanyId ='{0}'", CompanyId, "PlivoAuthId")).FirstOrDefault();
            if (aid == null)
            {
                return false;
            }
            GlobalSetting atok = _GlobalSettingDataAccess.GetByQuery(string.Format("SearchKey ='{1}' and CompanyId ='{0}'", CompanyId, "PlivoAuthToken")).FirstOrDefault();
            if (atok == null)
            {
                return false;
            }

            GlobalSetting SenderNum = _GlobalSettingDataAccess.GetByQuery(string.Format("SearchKey ='{1}' and CompanyId ='{0}'", CompanyId, "SMSSenderNumber")).FirstOrDefault();
            if (SenderNum == null)
            {
                return false;
            }
            #endregion

            string AuthId = aid.Value;//"MAZGIYZDHKOTY0MWYWMJ";
            string AuthToken = atok.Value;//"MjVkNzY4OWEwMzYzZDhhMGRhMTU2YTI4YjE3MTM4";
            string SenderNumber = SenderNum.Value;// "+18063020602";

            bool result = SMSManager.SendASms(ReceiverNumberList, SenderNumber, MessageBody, AuthId, AuthToken);

            if (result)
            {
                SMSHistory sh = new SMSHistory()
                {
                    FromMobileNo = SenderNumber,
                    IsRead = false,
                    LastUpdatedDate = DateTime.UtcNow,
                    SMSBodyContent = MessageBody,
                    SMSSentDate = DateTime.UtcNow,
                    TemplateKey = TemplateKey,
                    ReadCount = 0,
                    ToMobileNo = string.Join(",", ReceiverNumberList),
                    IsSystemAutoSent = IsSystemAutoSent,
                    FromName = FromName,
                    CompanyId = CompanyId,
                    CreatedBy = SendBy

                };
                _SMSHistoryDataAccess.Insert(sh);
            }
            return result;
        }
        public List<TicketReplyNoteCustomModel> GetJobNotesByTicketId(Guid ticketid)
        {
            DataTable dt = _TicketDataAccess.GetJobNotesByTicketId(ticketid);
            List<TicketReplyNoteCustomModel> model = new List<TicketReplyNoteCustomModel>();
            model = (from DataRow dr in dt.Rows
                     select new TicketReplyNoteCustomModel()
                     {
                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                         TicketId = (Guid)dr["TicketId"],
                         UserId = (Guid)dr["UserId"],
                         RepliedDate = dr["RepliedDate"] != DBNull.Value ? Convert.ToDateTime(dr["RepliedDate"]) : new DateTime(),
                         Message = dr["Message"].ToString(),
                         IsPrivate = dr["IsPrivate"] != DBNull.Value ? Convert.ToBoolean(dr["IsPrivate"]) : false,
                         IsOverview = dr["IsOverview"] != DBNull.Value ? Convert.ToBoolean(dr["IsOverview"]) : false,
                         UserName = dr["UserName"].ToString(),
                         ProfilePicture = dr["ProfilePicture"].ToString()
                     }).ToList();
            return model;
        }
        public List<Employee> GetAllEmployee(Guid Companyid)
        {
            return _EmployeeDataAccess.GetAllEmployee(Companyid);
        }
        public List<EmployeeCustomModel> GetEmployeeByCompanyIdAndPerGrpIdAPI(Guid CompanyId, string Tag, Guid userid, string PerGrpId)
        {
            DataTable dt = _EmployeeDataAccess.GetEmployeeByCompanyIdAndPerGrpIdAPI(CompanyId, Tag, userid, PerGrpId);
            List<EmployeeCustomModel> EmployeeList = new List<EmployeeCustomModel>();
            EmployeeList = (from DataRow dr in dt.Rows
                            select new EmployeeCustomModel()
                            {
                                UserId = new Guid(dr["UserId"].ToString()),
                                Name = dr["FirstName"].ToString() + " " + dr["LastName"].ToString(),
                                Selected = dr["SelectedUser"] != DBNull.Value ? Convert.ToBoolean(dr["SelectedUser"]) : false,
                            }).ToList();
            return EmployeeList;
        }
        public List<EmployeeCustomModel> GetEmployeeByCompanyIdAndTagAndTechnician(Guid CompanyId, string Tag, Guid userid)
        {
            DataTable dt = _EmployeeDataAccess.GetEmployeeByCompanyIdAndTagAndTechnician(CompanyId, Tag, userid);
            List<EmployeeCustomModel> EmployeeList = new List<EmployeeCustomModel>();
            EmployeeList = (from DataRow dr in dt.Rows
                            select new EmployeeCustomModel()
                            {
                                UserId = new Guid(dr["UserId"].ToString()),
                                Name = dr["FirstName"].ToString() + " " + dr["LastName"].ToString(),
                            }).ToList();
            return EmployeeList;
        }
        public string GetTicketAdditionalMemberOnlyTechnicianByCompanyId(Guid CompanyId)
        {
            string TicketAdditionalMemberOnlyTechnician = RMRCacheKey.TicketAdditionalMemberOnlyTechnician + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[TicketAdditionalMemberOnlyTechnician] == null)
            {
                string SearchKey = "TicketAdditionalMemberOnlyTechnician";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                result = globalsetting == null ? "" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[TicketAdditionalMemberOnlyTechnician] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[TicketAdditionalMemberOnlyTechnician];
            }
            return result;
        }
        public TicketBookingDetails GetItemInfoByTicketId(string BookingId, string Subject, int Id)
        {
            return _TicketBookingDetailsDataAccess.GetByQuery(string.Format("BookingId = '{0}' and ServiceType = '{1}' and Id = {2}", BookingId, Subject, Id)).FirstOrDefault();
        }
  

        public long InsertCustomerPackageEqp(CustomerPackageEqp _CustomerPackageEqp)
        {
            return _CustomerPackageEqpDataAccess.Insert(_CustomerPackageEqp);
        }
        public CustomerPackageEqp GetCustomerPackageEqpById(int Id)
        {
            return _CustomerPackageEqpDataAccess.Get(Id);
        }
        public bool UpdateCustomerPackageEqp(CustomerPackageEqp cpe)
        {
            return _CustomerPackageEqpDataAccess.Update(cpe) > 0;
        }

        public List<EquipmentSearchModel> GetEqupmentListBySearchKeyAndCompanyId(string key, Guid CompanyId, int MaxLoad, string ExistEquipment)
        {
            DataTable dt = _InvoiceDataAccess.GetEqupmentListBySearchKeyAndCompanyId(key, CompanyId, MaxLoad, ExistEquipment);
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
                            SKU = dr["SKU"].ToString(),
                            Barcode= dr["Barcode"].ToString(),
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            ManufacturerName = dr["ManufacturerName"].ToString(),
                            IsTaxable = dr["IsTaxable"] != DBNull.Value ? Convert.ToBoolean(dr["IsTaxable"]) : true,
                            EquipmentClassId = dr["EquipmentClassId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentClassId"]) : 0,
                            Equipmentvendorcost = dr["EquipmentVendorCost"] != DBNull.Value ? Convert.ToDouble(dr["EquipmentVendorCost"]) : 0.0
                        }).ToList();
            return NoteList;
        }
        public List<EquipmentSearchModel> GetEqupmentListBySearchKeyAndCompanyIdBarcode(string code, Guid CompanyId, int MaxLoad, string ExistEquipment)
        {
            DataTable dt = _InvoiceDataAccess.GetEqupmentListBySearchKeyAndCompanyIdBarcode(code, CompanyId, MaxLoad, ExistEquipment);
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
                            SKU = dr["SKU"].ToString(),
                            Barcode = dr["Barcode"].ToString(),
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            ManufacturerName = dr["ManufacturerName"].ToString(),
                            IsTaxable = dr["IsTaxable"] != DBNull.Value ? Convert.ToBoolean(dr["IsTaxable"]) : true,
                            EquipmentClassId = dr["EquipmentClassId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentClassId"]) : 0,
                            Equipmentvendorcost = dr["EquipmentVendorCost"] != DBNull.Value ? Convert.ToDouble(dr["EquipmentVendorCost"]) : 0.0
                        }).ToList();
            return NoteList;
        }
        public bool SendTicketCreatedNotificationEmail(TicketNotificationEmails email)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("CreatedByName", email.CreatedByName);
                templateVars.Add("CreatedForCustomerName", email.CreatedForCustomerName);
                templateVars.Add("TicketMessage", email.TicketMessage);
                templateVars.Add("TicketNumber", email.TicketNumber);
                templateVars.Add("Subject", email.Subject);
                templateVars.Add("BodyMessage", email.BodyMessage);
                templateVars.Add("TicketUrl", email.TicketUrl);
                templateVars.Add("Address", email.CustomerAddress);

                templateVars.Add("ScheduleOn", email.CompletionDate.ToString("MM/dd/yyyy"));


                templateVars.Add("StartTime", email.AppointmentStartTime);
                templateVars.Add("EndTime", email.AppointmentEndTime);

                if (SentEmail(templateVars, EmailTemplateKey.TicketEmailTemplates.SendTicketCreatedNotification, email.CompanyId, null))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }

            return false;
        }
        public List<TicketUser> GetTicketUserListByTicketId(Guid ticketId)
        {

            DataTable Dt = _TicketDataAccess.GetTicketUserListByTicketId(ticketId);

            List<TicketUser> TicketUsers = (from DataRow dr in Dt.Rows
                                            select new TicketUser()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                TiketId = (Guid)dr["TiketId"],
                                                UserId = (Guid)dr["UserId"],
                                                AddedBy = (Guid)dr["UserId"],
                                                AddedDate = dr["AddedDate"] != DBNull.Value ? Convert.ToDateTime(dr["AddedDate"]) : new DateTime(),
                                                IsPrimary = dr["IsPrimary"] != DBNull.Value ? Convert.ToBoolean(dr["IsPrimary"]) : false,
                                                NotificationOnly = dr["NotificationOnly"] != DBNull.Value ? Convert.ToBoolean(dr["NotificationOnly"]) : false,
                                                FullName = dr["FullName"].ToString()
                                            }).ToList();
            return TicketUsers;
        }
        public bool CloseTicketPermission(Guid uid, Guid comid)
        {
            return _customerDataAccess.ApiPermissionGroupMapByUserIdCompanyIdandPermissionName(uid, comid, "AppCustomerCloseTicketListShow");

        }
   
        public CustomSurvey GetCustomSurveyBySurveyId(Guid surveyId)
        {
            return _CustomSurveyDataAccess.GetByQuery(string.Format(" SurveyId = '{0}'", surveyId)).FirstOrDefault();
        }
        public Ticket GetTicketByTicketId(Guid ticketid)
        {
            return _TicketDataAccess.GetByQuery(string.Format("TicketId = '{0}'", ticketid)).FirstOrDefault();
        }
        public int InsertCustomSurveyUser(CustomSurveyUser item)
        {
            return (int)_CustomSurveyUserDataAccess.Insert(item);
        }
        private static string RandomString(int length)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            string Alphabet = "abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = Alphabet[rand.Next(Alphabet.Length)];
            }
            return new string(chars);

        }
        public List<Invoice> GetAllInvoicesByCustomerId(Guid companyid, Guid customerid)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and CustomerId = '{1}' and [Status] != 'Init' and ([Status] = 'Open' or [Status] = 'Partial') and IsEstimate = 0", companyid, customerid)).ToList();
        }

        public ShortUrl GetSortUrlByUrl(string fullurl, Guid? CustomerId)
        {
            ShortUrl model = _ShortUrlDataAccess.GetByQuery(string.Format(" Url='{0}'", fullurl)).FirstOrDefault();
            if (model == null)
            {
                string RandomCode = RandomString(7);

                while (_ShortUrlDataAccess.GetByQuery(string.Format(" Code = '{0}'", RandomCode)).FirstOrDefault() != null)
                {
                    RandomCode = RandomString(7);
                }

                model = new ShortUrl()
                {
                    Code = RandomCode,
                    Url = fullurl,
                    CustomerId = CustomerId.HasValue ? CustomerId.Value : new Guid()
                };
                model.Id = (int)_ShortUrlDataAccess.Insert(model);
                return model;
            }
            else
            {
                return model;
            }
        }
        public CustomSurvey GetFirstCustomSurvey()
        {
            return _CustomSurveyDataAccess.GetAll().FirstOrDefault();
        }
        public List<CustomSurvey> GetAllCustomSurvey()
        {
            return _CustomSurveyDataAccess.GetAll();
        }
        public Double GetInvoiceBalanceDueByCustomerId(Guid CustomerId)
        {
            double BalanceDue = 0.0;
            DataTable dt = _InvoiceDataAccess.GetInvoiceBalanceDueByCustomerId(CustomerId);
            if (dt.Rows.Count > 0)
            {
                BalanceDue = dt.Rows[0]["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dt.Rows[0]["BalanceDue"]) : 0.0;
            }
            return BalanceDue;
        }
        public JobsCustomModel GetJobSchedulesByTicketId(Guid ticketid)
        {
            DataSet ds = _customerDataAccess.GetJobSchedulesByTicketId(ticketid);
            JobsCustomModel model = new JobsCustomModel();
            model.TicketList = (from DataRow dr in ds.Tables[0].Rows
                                select new TicketCustomModel()
                                {
                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    TicketId = (Guid)dr["TicketId"],
                                    CompanyId = (Guid)dr["CompanyId"],
                                    CustomerId = (Guid)dr["CustomerId"],
                                    TicketType = dr["TicketType"].ToString(),
                                    Subject = dr["Subject"].ToString(),
                                    Message = dr["Message"].ToString(),
                                    CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                    CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                                    Status = dr["Status"].ToString(),
                                    CompletedDate = dr["CompletedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletedDate"]) : new DateTime(),
                                }).ToList();
            model.UserList = (from DataRow dr in ds.Tables[1].Rows
                              select new TicketUser()
                              {
                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                  TiketId = (Guid)dr["TiketId"],
                                  UserId = (Guid)dr["UserId"],
                                  AddedDate = dr["AddedDate"] != DBNull.Value ? Convert.ToDateTime(dr["AddedDate"]) : new DateTime(),
                                  IsPrimary = dr["IsPrimary"] != DBNull.Value ? Convert.ToBoolean(dr["IsPrimary"]) : false,
                                  NotificationOnly = dr["NotificationOnly"] != DBNull.Value ? Convert.ToBoolean(dr["NotificationOnly"]) : false,
                              }).ToList();
            model.ScheduleList = (from DataRow dr in ds.Tables[2].Rows
                                  select new CustomerAppointment()
                                  {
                                      Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                      AppointmentId = (Guid)dr["AppointmentId"],
                                      CompanyId = (Guid)dr["CompanyId"],
                                      CustomerId = (Guid)dr["CustomerId"],
                                      AppointmentType = dr["AppointmentType"].ToString(),
                                      AppointmentDate = dr["AppointmentDate"] != DBNull.Value ? Convert.ToDateTime(dr["AppointmentDate"]) : new DateTime(),
                                      AppointmentStartTime = dr["AppointmentStartTime"].ToString(),
                                      AppointmentEndTime = dr["AppointmentEndTime"].ToString(),
                                      Notes = dr["Notes"].ToString(),
                                      IsAllDay = dr["IsAllDay"] != DBNull.Value ? Convert.ToBoolean(dr["IsAllDay"]) : false,
                                      Status = dr["Status"] != DBNull.Value ? Convert.ToBoolean(dr["Status"]) : false,
                                      Address = dr["Address"].ToString(),
                                  }).ToList();
            model.AdditionalScheduleList = (from DataRow dr in ds.Tables[3].Rows
                                            select new AdditionalMembersAppointment()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                AppointmentId = (Guid)dr["AppointmentId"],
                                                CompanyId = (Guid)dr["CompanyId"],
                                                CustomerId = (Guid)dr["CustomerId"],
                                                AppointmentDate = dr["AppointmentDate"] != DBNull.Value ? Convert.ToDateTime(dr["AppointmentDate"]) : new DateTime(),
                                                AppointmentStartTime = dr["AppointmentStartTime"].ToString(),
                                                AppointmentEndTime = dr["AppointmentEndTime"].ToString(),
                                                IsAllDay = dr["IsAllDay"] != DBNull.Value ? Convert.ToBoolean(dr["IsAllDay"]) : false,
                                            }).ToList();
            return model;
        }
        public JobsCustomModel GetJobDetailByJobId(Guid ticketid)
        {
            DataSet ds = _customerDataAccess.GetJobDetailByJobId(ticketid);
            JobsCustomModel model = new JobsCustomModel();
            model.TicketList = (from DataRow dr in ds.Tables[0].Rows
                                select new TicketCustomModel()
                                {
                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    CustomerIntId = dr["CustomerIntId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIntId"]) : 0,
                                    TicketId = (Guid)dr["TicketId"],
                                    CompanyId = (Guid)dr["CompanyId"],
                                    CustomerId = (Guid)dr["CustomerId"],
                                    TicketType = dr["TicketType"].ToString(),
                                    Subject = dr["Subject"].ToString(),
                                    Message = dr["Message"].ToString(),
                                    CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                    CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                                    Status = dr["Status"].ToString(),
                                    CompletedDate = dr["CompletedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletedDate"]) : new DateTime(),
                                    HasInv = dr["HasInv"] != DBNull.Value ? Convert.ToBoolean(dr["HasInv"]) : false,
                                    HasNote = dr["HasNote"] != DBNull.Value ? Convert.ToBoolean(dr["HasNote"]) : false,
                                    HasLog = dr["HasLog"] != DBNull.Value ? Convert.ToBoolean(dr["HasLog"]) : false,
                                    IsClosed = dr["IsClosed"] != DBNull.Value ? Convert.ToBoolean(dr["IsClosed"]) : false,
                                    CustomerName = dr["CustomerName"].ToString(),
                                    AssignedPerson = dr["AssignedPerson"].ToString(),
                                    CreatedPerson = dr["CreatedPerson"].ToString(),
                                    AssignedStatus = dr["AssignedStatus"].ToString(),
                                    ProfileImage = dr["ProfileImage"].ToString(),
                                    Phone = dr["Phone"].ToString(),
                                    EmailAddress = dr["EmailAddress"].ToString(),
                                    Street = dr["Street"].ToString(),
                                    City = dr["City"].ToString(),
                                    State = dr["State"].ToString(),
                                    ZipCode = dr["ZipCode"].ToString(),
                                    IsDispatch = dr["IsDispatch"] != DBNull.Value ? Convert.ToBoolean(dr["IsDispatch"]) : false,
                                    HasBooking = dr["HasBooking"] != DBNull.Value ? Convert.ToBoolean(dr["HasBooking"]) : false,
                                    //HasExtraItem = dr["HasExtraItem"] != DBNull.Value ? Convert.ToBoolean(dr["HasExtraItem"]) : false,
                                    InvoiceBalanceDue = dr["InvoiceBalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["InvoiceBalanceDue"]) : 0,
                                    HasFile = dr["HasFile"] != DBNull.Value ? Convert.ToBoolean(dr["HasFile"]) : false,
                                }).ToList();
            return model;
        }
        public JobsCustomModel GetAllJobsDetailByFilter(Guid comid, string from, string to, Guid userid, string status, string type, bool permission)
        {
            DataSet ds = _customerDataAccess.GetAllJobsDetailByFilter(comid, from, to, userid, status, type, permission);
            JobsCustomModel model = new JobsCustomModel();
            model.TicketList = (from DataRow dr in ds.Tables[0].Rows
                                select new TicketCustomModel()
                                {
                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    TicketId = (Guid)dr["TicketId"],
                                    CompanyId = (Guid)dr["CompanyId"],
                                    CustomerId = (Guid)dr["CustomerId"],
                                    TicketType = dr["TicketType"].ToString(),
                                    Subject = dr["Subject"].ToString(),
                                    Message = dr["Message"].ToString(),
                                    CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                    CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                                    Status = dr["Status"].ToString(),
                                    CompletedDate = dr["CompletedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletedDate"]) : new DateTime(),
                                    CustomerName = dr["CustomerName"].ToString(),
                                    AssignedPerson = dr["AssignedPerson"].ToString(),
                                    CreatedPerson = dr["CreatedPerson"].ToString(),
                                    AssignedStatus = dr["AssignedStatus"].ToString(),
                                    Street = dr["Street"].ToString(),
                                    City = dr["City"].ToString(),
                                    State = dr["State"].ToString(),
                                    ZipCode = dr["ZipCode"].ToString(),
                                    AppointmentStartTime = dr["AppointmentStartTime"].ToString(),
                                    AppointmentEndTime = dr["AppointmentEndTime"].ToString(),
                                    CustomerIntId = dr["CustomerIntId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIntId"]) : 0
                                }).OrderByDescending(ticket => ticket.Id).ToList();
            return model;
        }
        public List<TransactionQueue> GetTransactionQueueCustomerId(Guid customerId, string starttime, string endtime, double amount)
        {
            DataTable dt = _TransactionDataAccess.GetTransactionQueueCustomerId(customerId, starttime, endtime, amount);
            List<TransactionQueue> TransactionQueueList = new List<TransactionQueue>();
            TransactionQueueList = (from DataRow dr in dt.Rows
                                    select new TransactionQueue()
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,

                                        Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                                    }).ToList();
            return TransactionQueueList;
        }
        public int InsertTransactionQueue(TransactionQueue transqueue)
        {
            transqueue.Amount = transqueue.Amount.DoubleRound(2);
            return (int)_TransactionQueueDataAccess.Insert(transqueue);
        }
        public Invoice GetInvoiceById(int value)
        {
            return _InvoiceDataAccess.Get(value);
        }
        public int InsertTransaction(Transaction transaction)
        {
            transaction.Amount = transaction.Amount.DoubleRound(2);
            return (int)_TransactionDataAccess.Insert(transaction);
        }
        public bool UpdatePaymentInvoice(Invoice invoice)
        {
            Invoice t = _InvoiceDataAccess.Get(invoice.Id);
            if (t != null && t.IsBill.HasValue && t.IsBill.Value)
            {
                return false;
            }
           
            invoice.Amount = invoice.Amount.DoubleRound(2);
            invoice.Tax = (invoice.Tax.HasValue ? invoice.Tax.Value : 0).DoubleRound(2);
            invoice.DiscountAmount = (invoice.DiscountAmount.HasValue ? invoice.DiscountAmount.Value : 0).DoubleRound(2);
            invoice.TotalAmount = (invoice.TotalAmount.HasValue ? invoice.TotalAmount.Value : 0).DoubleRound(2);
            invoice.ShippingCost = (invoice.ShippingCost.HasValue ? invoice.ShippingCost.Value : 0).DoubleRound(2);
            invoice.BalanceDue = (invoice.BalanceDue.HasValue ? invoice.BalanceDue.Value : 0).DoubleRound(2);
            invoice.Deposit = (invoice.Deposit.HasValue ? invoice.Deposit.Value : 0).DoubleRound(2);
            invoice.Balance = (invoice.Balance.HasValue ? invoice.Balance.Value : 0).DoubleRound(2);
            invoice.LateFee = (invoice.LateFee.HasValue ? invoice.LateFee.Value : 0).DoubleRound(2);
            invoice.LateAmount = (invoice.LateAmount.HasValue ? invoice.LateAmount.Value : 0).DoubleRound(2);
            invoice.MonitoringAmount = (invoice.MonitoringAmount.HasValue ? invoice.MonitoringAmount.Value : 0).DoubleRound(2);
           

            return _InvoiceDataAccess.Update(invoice) > 0;
        }
        public void InsertTransactionHistoryList(List<TransactionHistory> trhistory)
        {

            string TrHistoryInsertTemplate = "INSERT INTO [TransactionHistory] ( TransactionId, InvoiceId, Amout,Balance,ReceivedBy) VALUES ({0},{1},{2},{3},'{4}');";
            string sql = "";
            foreach (var item in trhistory)
            {
                sql += string.Format(TrHistoryInsertTemplate, item.TransactionId, item.InvoiceId, item.Amout.DoubleRound(2), item.Balance.DoubleRound(2), item.ReceivedBy) + Environment.NewLine;
            }
            _TransactionHistoryDataAccess.InsertTransactionHistoryList(sql);
        }

        public int InsertCustomerAppointmentEquipmentDetail(CustomerAppointmentEquipment CustomerAppointmentEquipment)
        {
            return (int)_CustomerAppointmentEquipmentDataAccess.Insert(CustomerAppointmentEquipment);
        }
        public CustomerAppointmentEquipment GetCustomerAppointmentEquipmentById(int value)
        {
            return _CustomerAppointmentEquipmentDataAccess.Get(value);
        }
        public bool UpdateCustomerAppoinmentEquipment(CustomerAppointmentEquipment cae)
        {
            return _CustomerAppointmentEquipmentDataAccess.Update(cae) > 0;
        }
        public bool UpdateCustomerPackageService(CustomerPackageService cps)
        {
            return _CustomerPackageServiceDataAccess.Update(cps) > 0;
        }
        public Customer GetCustomerByCustomerId(Guid customerId)
        {
            var query = "CustomerId='" + customerId +"'";
            return _customerDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public Estimator GetEstimatorByEstimatorId(string estimatorId)
        {
            return _EstimatorDataAccess.GetByQuery(string.Format(" EstimatorId = '{0}' ", estimatorId)).FirstOrDefault();
        }
        public List<SelectListItem> GetDropdownsByKey(string key, bool IncludeInActive = false)
        {
            List<Lookup> lookuplist = lookups(key, IncludeInActive);
            List<SelectListItem> selectListItems = lookuplist.OrderBy(x => x.DataOrder).Select(x =>
                new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString(),
                    Selected = (x.IsDefaultItem.HasValue && x.IsDefaultItem.Value)
                }).ToList();
            return selectListItems;
        }
        private List<Lookup> lookups(string key, bool IncludeInActive = false, bool ClearCache = false)
        {
            Guid comid = new Guid();
            #region Company Id
            if (System.Web.HttpContext.Current.Session[SessionKeys.CompanyId] != null)
            {
                comid = (Guid)System.Web.HttpContext.Current.Session[SessionKeys.CompanyId];
            }
            #endregion
            LocalizeFacade _localize = new LocalizeFacade();
            List<Lookup> resultLookup = new List<Lookup>();
            string currentLanguage = "en-US";
            if (HttpContext.Current.Request.Cookies["__Lng"] != null)
            {
                currentLanguage = HttpContext.Current.Request.Cookies["__Lng"].Value;
            }
            string cachekey = key + currentLanguage + comid.ToString();
            if (ClearCache)
            {
                System.Web.HttpRuntime.Cache.Remove(cachekey);
            }
            if (System.Web.HttpRuntime.Cache[cachekey] == null)
            {
                resultLookup = _LookupDataAccess.GetByQuery(string.Format(" DataKey = '{0}' and CompanyId = '{1}' order by DataOrder asc", key, comid));
                foreach (var lookup in resultLookup)
                {
                    lookup.DisplayText = _localize.GetResource(lookup.DisplayText);
                }
                //System.Web.HttpRuntime.Cache[cachekey] = resultLookup;
            }
            else
            {
                resultLookup = (List<Lookup>)System.Web.HttpRuntime.Cache[cachekey];
            }
            if (!IncludeInActive && resultLookup != null && resultLookup.Count() > 0)
            {
                return resultLookup.Where(x => x.IsActive == true).OrderBy(x => x.DataOrder).ToList();
                System.Web.HttpRuntime.Cache[cachekey] = resultLookup;
            }
            else
            {
                return resultLookup.OrderBy(x => x.DataOrder).ToList();
                System.Web.HttpRuntime.Cache[cachekey] = resultLookup;
            }
        }
        public Manufacturer GetManufacturerByManufacturerId(Guid manufacturerId)
        {
            return _ManufacturerDataAccess.GetByQuery(string.Format("ManufacturerId ='{0}'", manufacturerId)).FirstOrDefault();
        }
        public List<EquipmentFile> GetEquipmentFilesByEquipmentIdAndFileType(Guid equipmentId, string FileType)
        {
            return _EquipmentFileDataAccess.GetByQuery(string.Format("EquipmentId='{0}' AND FileType ='{1}' ", equipmentId, FileType));
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
        public List<Equipment> GetSmartEquipmentEstimatorListByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId, bool firstpage, int ticketid, string EstimatorId)
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
        public PaymentInfoCustomer GetPaymentInfoCustomerByCustomerIdAndPayForService(Guid cusid)
        {
            return _PaymentInfoCustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and Payfor = 'Service'", cusid)).FirstOrDefault();
        }
        public PackageCustomer GetPackageCustomerByCustomerIdandCompanyId(Guid CustomerId, Guid CompanyId)
        {
            return _PackageCustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}'", CustomerId, CompanyId)).FirstOrDefault();
        }
        public string SmartInstallTypeNameByInstallTypeId(int InstallTypeId)
        {
            string InstallTypeName = "";
            var InstallDetail = _SmartInstallTypeDataAccess.Get(InstallTypeId);
            if (InstallDetail != null)
            {
                InstallTypeName = InstallDetail.Name;
            }
            return InstallTypeName;
        }

        public List<PaymentInfo> GetAllPaymentInfoByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId)
        {
            DataTable dt = _PaymentInfoDataAccess.GetAllPaymentInfoByCustomerIdAndCompanyId(CustomerId, CompanyId);
            List<PaymentInfo> PaymentInfoList = new List<PaymentInfo>();
            PaymentInfoList = (from DataRow dr in dt.Rows
                               select new PaymentInfo()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   CompanyId = (Guid)dr["CompanyId"],
                                   AccountName = dr["AccountName"].ToString(),
                                   BankAccountType = dr["BankAccountType"].ToString(),
                                   RoutingNo = dr["RoutingNo"].ToString(),
                                   AcountNo = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["AcountNo"].ToString()),
                                   CardType = dr["CardType"].ToString(),
                                   CardNumber = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardNumber"].ToString()),
                                   CardExpireDate = dr["CardExpireDate"].ToString(),
                                   CardSecurityCode = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardSecurityCode"].ToString()),
                                   PayFor = dr["PayFor"].ToString(),
                                   Type = dr["Type"].ToString(),
                                   PaymentCustomerId = (Guid)dr["PaymentCustomerId"],
                                   CheckNo = dr["CheckNo"].ToString(),
                                   EcheckType = dr["EcheckType"].ToString(),
                                   BankName = dr["BankName"].ToString(),
                               }).ToList();
            return PaymentInfoList;
        }
        public List<PaymentInfo> GetLeadAgreementPaymentInfoByCustomerId(Guid CustomerId)
        {
            DataTable dt = _PaymentInfoDataAccess.GetLeadAgreementPaymentInfoByCustomerId(CustomerId);
            List<PaymentInfo> OldPaymentList = new List<PaymentInfo>();
            OldPaymentList = (from DataRow dr in dt.Rows
                              select new PaymentInfo()
                              {
                                  AccountName = dr["AccountName"].ToString(),
                                  BankAccountType = dr["BankAccountType"].ToString(),
                                  RoutingNo = dr["RoutingNo"].ToString(),
                                  AcountNo = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["AcountNo"].ToString()),
                                  CardType = dr["CardType"].ToString(),
                                  CardNumber = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardNumber"].ToString()),
                                  CardExpireDate = dr["CardExpireDate"].ToString(),
                                  CardSecurityCode = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardSecurityCode"].ToString()),
                                  Type = dr["Type"].ToString()
                              }).ToList();
            return OldPaymentList;
        }
        public CustomerSignature GetCustomerSignatureByReferenceIdcharCustomerIdType(Guid customerId, string referenceCharId, string type)
        {
            return _CustomerSignatureDataAccess.GetByQuery(string.Format("CustomerId ='{0}' and ReferenceIdnvarchar='{1}' and Type='{2}' order by Id desc", customerId, referenceCharId, type)).FirstOrDefault();
        }
        public List<PaymentInfoCustomer> GetAllPaymentInfoCustomerByCustomerId(Guid customerId)
        {
            return _PaymentInfoCustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerId)).ToList();
        }
        public Lookup GetLookupByKeyAndValueAndCompanyId(string dataKey, string dataValue, Guid comid)
        {
            return _LookupDataAccess.GetByQuery(string.Format(" datakey='{0}' and datavalue='{1}' and CompanyId = '{2}'", dataKey, dataValue, comid)).FirstOrDefault();
        }
        public string GetCustomerNameById(Guid customerId)
        {
            string CustomerName = "";
            string sql = string.Format("CustomerId='{0}'", customerId);
            var CustomerDetails = _customerDataAccess.GetByQuery(sql).FirstOrDefault();
            if (CustomerDetails != null)
            {
                CustomerName = CustomerDetails.FirstName + " " + CustomerDetails.LastName;
            }
            return CustomerName;
        }
        public string GetCompanyEmailLogoByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;

            var LogoUrl = _CompanyBranchDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and IsMainBranch = 'true'", CompanyId)).FirstOrDefault();

            if (LogoUrl != null)
            {
                if (!string.IsNullOrEmpty(LogoUrl.EmailLogo) && !string.IsNullOrWhiteSpace(LogoUrl.EmailLogo))
                {
                    string baseUrl = WebConfigurationManager.AppSettings["ShortSiteDomain"];
                    result = baseUrl + LogoUrl.EmailLogo;
                }
                else
                {
                    result = WebConfigurationManager.AppSettings["Logo.DefaultEmailLogo"];
                }
            }
            return result;
        }
        public List<CustomerAgreement> GetCustomerAgreementByCompanyIdAndCustomerId1(Guid companyid, Guid customerid)
        {
            return _CustomerAgreementDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and CustomerId = '{1}'", companyid, customerid)).ToList();
        }
        public List<AgreementAnswer> GetAllAgreementAnswerByCustomerId(Guid cusid)
        {
            return _AgreementAnswerDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", cusid)).ToList();
        }
        public CustomerAgreement GetIpAndUserAgentByCustomerIdAndCompanyId(Guid comid, Guid Customerid)
        {
            DataTable dt = _CustomerAgreementDataAccess.GetIpAndUserAgentByCustomerIdAndCompanyId(comid, Customerid);
            CustomerAgreement CopanyList = new CustomerAgreement();
            CopanyList = (from DataRow dr in dt.Rows
                          select new CustomerAgreement()
                          {
                              IP = dr["IP"].ToString(),
                              UserAgent = dr["UserAgent"].ToString()
                          }).FirstOrDefault();
            return CopanyList;
        }
        public string GetEmployeeNumByEmployeeId(Guid employeeid)
        {
            string EmapName = "";
            var result = _EmployeeDataAccess.GetByQuery(string.Format(" UserId = '{0}'", employeeid)).FirstOrDefault();
            if (result != null)
            {
                EmapName = result.FirstName + " " + result.LastName;
            }
            return EmapName;
        }
        public string GetCurrentCurrencyByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;
            var globalsetting = new GlobalSetting();
            string CurrentCurrency = RMRCacheKey.CurrentCurrency + CompanyId.ToString();

            if (System.Web.HttpRuntime.Cache[CurrentCurrency] == null)
            {
                string DataKey = "CurrentCurrency";
                globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    result = globalsetting.Value.ToString();
                }
                else
                {
                    result = "$";
                }

                System.Web.HttpRuntime.Cache[CurrentCurrency] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[CurrentCurrency];
            }
            return result;
        }
        #region
        public string FormatAmount(double? value)
        {
            string formatted = "0.00";
            if (value.HasValue)
            {
                formatted = string.Format(CultureInfo.InvariantCulture, "{0:N}", value);

            }
            return formatted;
        }
        public List<PaymentInfoCustomer> GetAllPaidPaymentInfoCustomer(Guid comid, Guid cusid)
        {
            DataTable dt = _PaymentInfoCustomerDataAccess.GetAllPaidPaymentInfoCustomer(comid, cusid);
            List<PaymentInfoCustomer> PaymentInfoList = new List<PaymentInfoCustomer>();
            PaymentInfoList = (from DataRow dr in dt.Rows
                               select new PaymentInfoCustomer()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   CompanyId = (Guid)dr["CompanyId"],
                                   Type = dr["Type"].ToString(),
                                   CustomerId = (Guid)dr["CustomerId"],
                                   Payfor = dr["Payfor"].ToString(),
                                   PaymentType = dr["PaymentType"].ToString()
                               }).ToList();
            return PaymentInfoList;
        }
        private string GetEstimatorLineItems(List<HS.Entities.EstimatorDetail> estimatorDetails, string Currency, HS.Entities.EstimatorPDFFilter filters)
        {
            string result = "";
            double TotalCatagoryCost = 0;
            foreach (HS.Entities.EstimatorDetail item in estimatorDetails)
            {
                TotalCatagoryCost += item.TotalPrice.HasValue ? item.TotalPrice.Value : 0.0;

                result += "<tr style='border-bottom:1px solid #ccc;'>"
              + "<td style='padding:5px 0px 5px 40px;'>";
                if (filters.IncludeImage.Value == true && item.EquipmentFile != null && !string.IsNullOrWhiteSpace(item.EquipmentFile.Filename))
                {
                    result += "<img src ='" +
                        item.EquipmentFile.Filename +
                        "' alt='Alternate Text' style='width:25px;height:25px'/>";
                }
                if (!string.IsNullOrWhiteSpace(item.PartDescription))
                {
                    result += "<span>"
                        + item.PartDescription +
                        "</span><br/>";
                }
                if (filters.IncludeManufacturer.Value == true)
                {
                    result += "<span>" +
                        item.Manufacturer +
                        "</span>";
                }
                result += "</td>" +
             "<td style='padding:5px 0px 5px 40px;'>";
                if (!string.IsNullOrWhiteSpace(item.PartNumber))
                {
                    result += "<b>" +
                        item.PartNumber
                        + "</b>";
                }
                if (filters.IncludeVariation.Value)
                {
                    result += "<span>" +
                        item.Variation
                        + "</span>";
                }
                result += "</td>"
             + "<td style='padding:5px 0px 5px 40px;'>";
                if (item.Qunatity != null)
                {
                    result += "<span>" +
                        item.Qunatity +
                        "</span><br/>";
                }
                result += "</td>";
                if (filters.IncludeCost.HasValue && filters.IncludeCost.Value == true)
                {
                    result += "<td style='padding:5px 0px 5px 40px;'>";
                    if (item.UnitCost != null)
                    {
                        result += "<span>" +
                            Currency + FormatAmount(item.UnitCost) +
                            "</span><br/>";
                    }
                    result += "</td>"
                + "<td style='padding:5px 0px 5px 40px;'>";
                    if (item.TotalCost != null)
                    {
                        result += "<span>" +
                            Currency + FormatAmount(item.TotalCost)
                            + "</span><br/>";
                    }
                    result += "</td>";
                }
                if (filters.IncludeProfit.Value || filters.IncludeMargin.Value)
                {
                    result += "<td style='padding:5px 0px 5px 40px;'>";
                    if (item.Profit != null)
                    {
                        result += "<span>" +
                            Currency + FormatAmount(item.Profit) +
                            "</span><br/>";
                    }
                    result += "</td>";
                }
                if (filters.IncludeOverhead.Value)
                {
                    result += "<td>";
                    if (item.Overhead != null)
                    {
                        result += "<span>" +
                            Currency + FormatAmount(item.Overhead) +
                            "</span><br/>";
                    }
                    result += "</td>";
                }
                if (filters.WithoutPricing.Value
                    || (filters.WithoutIndividualMaterialPricing.Value && item.CategoryVal != "Labor")
                    || (filters.WithoutIndividualLaborPricing.Value && item.CategoryVal == "Labor"))
                {

                }
                else
                {
                    result += "<td>";
                    if (item.TotalPrice != null)
                    {
                        result += "<span>" +
                            Currency + FormatAmount(item.TotalPrice / item.Qunatity)
                            + "</span><br/>";
                    }
                    result += "</td>"
                + "<td style='text-align:right;'>";
                    if (item.TotalPrice != null)
                    {
                        result += "<span>" +
                            Currency + FormatAmount(item.TotalPrice)
                            + "</span><br/>";
                    }
                    result += "</td>";
                }
                result += "</tr>";
            }

            if (!filters.WithoutPricing.Value && !filters.GroupedbyNone.Value)
            {
                result += "<tr>"
                + "<td style='padding:5px 10px 5px 40px; text-align:right;' colspan='10'>"
                + "<b>"
                + "<span>"
                + "Sub Total: "
                + Currency + FormatAmount(TotalCatagoryCost)
                + "</span></b>"
                + "</td>"
                + "</tr>";
                TotalCatagoryCost = 0;
            }
            return result;
        }

        public string MakeSmartAgreementPdf(InstallationAgreementModel agreementPdf, int? templateid)
        {
            string Body = "";
            string ContactName = "";
            string ContactPhone = "";
            string CenterSpace = "";
            string PhoneType = "";
            string ContactRelationship = "";
            string HasKey = "";
            string ContactNameHeader = "";
            string ContactPhoneHeader = "";
            string PhoneTypeHeader = "";
            string ContactRelationshipHeader = "";
            string EmergencyContactList = "";
            string EstimateEmergencyContactList = "";
            string EquipmentName = "";
            string ServiceName = "";
            string MonthlyRate = "";
            string DiscountRate = "";
            string Total = "";
            string UnitPrice = "";

            string CommercialEquipmentList = "";
            string CommercialServiceList = "";

            string InvoiceQTY = "";
            string InvoiceName = "";
            string InvoicePrice = "";
            string InvoiceSubTotal = "";
            double InvoiceTotalSubTotal = 0.0;
            double EstimateUpfrontTotalSubTotal = 0.0;
            double EstimateRecurringTotalSubTotal = 0.0;
            double InvoiceTotalSubTotalWithUpfront = 0.0;
            double InvoiceFinalTotal = 0.0;
            double EstimateSigningAmount = 0.0;
            double EstimateSigningAmountWithProrate = 0.0;
            string InvoiceList = "";
            string FinancedAmount = "";

            string ProductDFW = "";
            string QuantityDFW = "";
            string UnitPriceDFW = "";
            string DiscountUnitPriceDFW = "";
            string TotalDiscountDFW = "";
            string TotalPriceDFW = "";

            string EquipmentQuantity = "";
            string DiscountUnitPrice = "";
            string TotalEquipment = "";
            string EquipmentList = "";
            string EquipmentListRab = "";
            string EquipmentListDFW = "";
            string EquipmentListTable = "";
            string ServiceList = "";
            string ServiceListRab = "";
            string ServiceListDFW = "";
            string ServiceListOnit = "";
            string SmartPackageEquipmentServiceList = "";
            string ListAgreementAnswer = "";
            string divAnsval = "";
            string CustomerAgreement = "";
            string CustomerAgreementTable = "";
            string currentCurrency = "";
            string Upfronaddon = "";
            string Service = "";
            string UpFront = "";
            string Qty = "";
            string TotalDfw = "";
            string MonthlyService = "";
            string Type = "";
            string Monthly = "";
            string ServiceListTable = "";
            bool isServiceDetail = false;
            string CardOrAccNo = "";
            string AccountType = "";
            var SSN = "";
            var OriginalContactDateTemplate = "";
            string AgreementAccountMessage = "By signing here you agree to authorize the company or its agents or assigns to initiate monthly debit entries to your checking, savings,or {0} ({1}) for the Total Monthly Amount referenced in paragraph 2.2with the depository or credit card company named above.";
            string AgreementAccountMessageInvoice = "By signing here you agree to authorize the company or its agents or assignee to initiate monthly invoice at the address above.";
            string CardTypeMMR = "";
            string SubscribedMonthsText = "";
            string RenewalMonthsText = "";
            string NotARBEnabledServiceList = "";
            string LabourfeeDfw = "";
            string LabourFeeRmR = "";

            string EstimatorDetailList = "";
            string EstimateUpfrontChargeList = "";
            string EstimateRecurringChargeList = "";
            string EstimatorProduct = "";
            string EstimatorSKU = "";
            string EstimatorQTY = "";
            string EstimatorUCOST = "";
            string EstimatorTCOST = "";
            string EstimatorPROFIT = "";
            string EstimatorOVERHEAD = "";
            string EstimatorServiceList = "";
            string EstimatorSerSERVICE = "";
            string EstimatorSerQTY = "";
            string EstimatorSerUNITPRICE = "";
            string EstimatorSerTOTALPRICE = "";
            string StandardPlan = "";
            string PremiumPlan = "";

            string NonConfirmingFeeDivCommFire = string.Empty;
            string residentialSearchKey = "ResidentialTechFirstHourCost";
            GlobalSetting ResidentialTechFirstHourCost = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", agreementPdf.CompanyId, residentialSearchKey)).FirstOrDefault();

            if (ResidentialTechFirstHourCost != null)
            {
                string value = ResidentialTechFirstHourCost.Value;
                Console.WriteLine("The value for the key 'ResidentialTechFirstHourCost' is: " + value);
                agreementPdf.ResidentialTechFirstHourCost = value;
            }
            else
            {
                Console.WriteLine("No setting found for the specified key.");
            }
            string commercialSearchKey = "CommercialTechFirstHourCost";
            GlobalSetting commercialTechFirstHourCost = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", agreementPdf.CompanyId, commercialSearchKey)).FirstOrDefault();

            if (commercialTechFirstHourCost != null)
            {
                string value = commercialTechFirstHourCost.Value;
                Console.WriteLine("The value for the key 'CommercialTechFirstHourCost' is: " + value);
                agreementPdf.CommercialTechFirstHourCost = value;
            }
            else
            {
                Console.WriteLine("No setting found for the specified key.");
            }

            bool IsBBBConflict = false;
            if (agreementPdf.CustomerAgreement != null)
            {
                IsBBBConflict = agreementPdf.CustomerAgreement.Where(m => m.Type == "AgreementComplete").Count() > 0;
            }
            double AdvanceMonitoringFee = 0.0;
            double MonthlyServiceFee = 0.0;
            GlobalSetting glLabourFee = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", agreementPdf.CompanyId, "HasLabourFee")).FirstOrDefault();

            if (glLabourFee != null)
            {
                if (glLabourFee.Value == "true")
                {
                    LabourfeeDfw = string.Format(@"  <tr style='height:25px;'>
                        <td valign='middle' style='font-weight:bold; border:1px solid #000; text-align:right; padding-right:5px; background-color:#f3f3f3;'>+  Labor Fee</td>
                        <td style='border:1px solid #000;text-align: right;padding-right: 10px;'>{0}</td>
                    </tr>", currentCurrency + FormatAmount(agreementPdf.LabourFee));
                    LabourFeeRmR = string.Format(@" <div style='width:100%;float:left;margin-top:5px;border-top:1px solid;margin-bottom:5px'>
                                    <div style='width:80%;float:left;padding-left:10px;padding-top:5px'>
                                        <span>Labor Fee</span>
                                    </div>
                                    <div style='width:18%;float:left;text-align:right;padding-top:5px'>
                                        {0}
                                    </div>
                                </div>", agreementPdf.LabourFee);
                }
                else
                {
                    LabourfeeDfw = "";
                    LabourFeeRmR = "";
                }
            }

            if (!string.IsNullOrWhiteSpace(agreementPdf.SubscribedMonths) && agreementPdf.SubscribedMonths.ToLower() != "month to month")
            {
                if (agreementPdf.SubscribedMonths == "1")
                {
                    SubscribedMonthsText = "Month";
                }
                else
                {
                    SubscribedMonthsText = "Months";
                }
            }
            if (agreementPdf.RenewalMonths == 1)
            {
                RenewalMonthsText = "Month";
            }
            else
            {
                RenewalMonthsText = "Months";
            }
            var ArbEnabledServiceList = agreementPdf.ServiceList.Where(m => m.IsARBEnabled == true).ToList();
            DataTable dt = _PaymentInfoDataAccess.GetAllPaymentInfoByCustomerIdAndCompanyId(agreementPdf.CustomerAgreement.FirstOrDefault().CustomerId, new Guid(agreementPdf.CompanyId));
            List<PaymentInfo> PaymentInfoList = new List<PaymentInfo>();
            PaymentInfoList = (from DataRow dr in dt.Rows
                               select new PaymentInfo()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   CompanyId = (Guid)dr["CompanyId"],
                                   AccountName = dr["AccountName"].ToString(),
                                   BankAccountType = dr["BankAccountType"].ToString(),
                                   RoutingNo = dr["RoutingNo"].ToString(),
                                   AcountNo = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["AcountNo"].ToString()),
                                   CardType = dr["CardType"].ToString(),
                                   CardNumber = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardNumber"].ToString()),
                                   CardExpireDate = dr["CardExpireDate"].ToString(),
                                   CardSecurityCode = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardSecurityCode"].ToString()),
                                   PayFor = dr["PayFor"].ToString(),
                                   Type = dr["Type"].ToString(),
                                   PaymentCustomerId = (Guid)dr["PaymentCustomerId"],
                                   CheckNo = dr["CheckNo"].ToString(),
                                   EcheckType = dr["EcheckType"].ToString(),
                                   BankName = dr["BankName"].ToString(),
                               }).ToList();
            var MMRPaymentProfile = PaymentInfoList.Where(m => m.PayFor == "MMR").FirstOrDefault();
            if (MMRPaymentProfile != null)
            {
                CardTypeMMR = MMRPaymentProfile.CardType;
            }
            var CreditCardInfo = PaymentInfoList.Where(x => x.CardSecurityCode != "" && x.CardNumber != "" && x.CardExpireDate != "" && x.PayFor == "MMR").FirstOrDefault();
            var ACHInfo = PaymentInfoList.Where(x => x.BankAccountType != "" && x.EcheckType != "" && x.RoutingNo != "" && x.AcountNo != "" && x.PayFor == "MMR").FirstOrDefault();
            if (CreditCardInfo != null)
            {
                AccountType = "Credit card account no ";
                CardOrAccNo = string.Concat("".PadLeft(12, '*'), ((CreditCardInfo.CardNumber).Substring(((CreditCardInfo.CardNumber).Length - 4))));
            }
            else if (ACHInfo != null && ACHInfo.AcountNo.Length > 4)
            {
                AccountType = "ACH account no ";
                CardOrAccNo = ACHInfo.Id > 0 && ACHInfo.AcountNo != null ? string.Concat("".PadLeft(ACHInfo.AcountNo.Length - 4, '*'), ACHInfo.AcountNo.Substring(ACHInfo.AcountNo.Length - 4)) : "";
            }

            GlobalSetting gl = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", agreementPdf.CompanyId, "AgreementServiceDetails")).FirstOrDefault();

            if (gl != null)
            {
                if (gl.Value == "true")
                {
                    isServiceDetail = true;
                }
                else
                {
                    isServiceDetail = false;
                }
            }
            else
            {
                isServiceDetail = false;
            }

            if (agreementPdf.CurrentCurrency != null)
            {
                currentCurrency = agreementPdf.CurrentCurrency;
            }
            else
            {
                currentCurrency = "$";
            }
            agreementPdf.ACHDiscountAmount = 0;
            var objpayinfocus = _PaymentInfoCustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and Payfor = 'MMR'", agreementPdf.CustomerAgreement.FirstOrDefault().CustomerId)).FirstOrDefault();
            if (objpayinfocus != null)
            {
                var objpayprofile = _PaymentProfileCustomerDataAccess.GetByQuery(string.Format("PaymentInfoId = {0}", objpayinfocus.PaymentInfoId)).FirstOrDefault();
                if (objpayprofile != null && objpayprofile.Type.ToLower().IndexOf("ach_") > -1)
                {
                    var objglobal = _GlobalSettingDataAccess.GetByQuery(string.Format("SearchKey = 'ACHDiscount'")).FirstOrDefault();
                    if (objglobal != null)
                    {
                        agreementPdf.ACHDiscountAmount = Convert.ToDouble(objglobal.Value);
                    }
                }
            }
            try
            {
                Hashtable templateVars = new Hashtable();
                if (CardTypeMMR == "Invoice")
                {
                    templateVars.Add("AgreementAccountMessage", AgreementAccountMessageInvoice);
                }
                else
                {
                    AgreementAccountMessage = string.Format(AgreementAccountMessage, AccountType, CardOrAccNo);
                    templateVars.Add("AgreementAccountMessage", AgreementAccountMessage);
                }
                templateVars.Add("CardOrAccNo", CardOrAccNo);
                templateVars.Add("PaymentAccType", AccountType);
                templateVars.Add("CSIDNumber", agreementPdf.CSIDNumber);
                templateVars.Add("LeadSource", agreementPdf.LeadSource);
                templateVars.Add("FileId", !string.IsNullOrWhiteSpace(agreementPdf.FileId) ? agreementPdf.FileId : "");
                templateVars.Add("IPAddress", !string.IsNullOrWhiteSpace(agreementPdf.CusSignIP) ? agreementPdf.CusSignIP : "");
                if (agreementPdf.OriginalContactDate != new DateTime())
                {
                    OriginalContactDateTemplate = "<tr style='height: 30px;'><td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Original Contract Date</td><td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>" + agreementPdf.OriginalContactDate.ToString("MM/dd/yy") + "</td></tr>";
                }
                templateVars.Add("InstallDate", OriginalContactDateTemplate);
                templateVars.Add("EstimateInstallDate", DateTime.Now.DateFormat());

                templateVars.Add("AccountType", !string.IsNullOrWhiteSpace(agreementPdf.AccountType) && agreementPdf.AccountType != "-1" ? agreementPdf.AccountType : "");
                templateVars.Add("Referredby", agreementPdf.Referredby);
                if (!string.IsNullOrEmpty(agreementPdf.SocialSecurityNumber) && agreementPdf.SocialSecurityNumber.Length > 4)
                {
                    var FormateSSN = agreementPdf.SocialSecurityNumber.Substring(agreementPdf.SocialSecurityNumber.Length - 4);
                    SSN = String.Format("{0:xxx-xx-0000}", Convert.ToInt32(FormateSSN));
                }
                templateVars.Add("FirstName", agreementPdf.FirstName);
                templateVars.Add("LastName", agreementPdf.LastName);
                templateVars.Add("SocialSecurityNumber", SSN);
                templateVars.Add("Owner2ndPhone", Extentions.PhoneNumberFormatNew(agreementPdf.Owner2ndPhone));
                templateVars.Add("InstallAddress", agreementPdf.InstallAddress);
                templateVars.Add("InitialStreet", agreementPdf.InitialStreet);
                templateVars.Add("InitialCity", agreementPdf.InitialCity);
                templateVars.Add("InitialCountry", agreementPdf.InitialCountry);
                templateVars.Add("InitialState", agreementPdf.InitialState);
                templateVars.Add("InitialZip", agreementPdf.InitialZip);
                templateVars.Add("BillingStreet", agreementPdf.BillingStreet);
                templateVars.Add("BillingCity", agreementPdf.BillingCity);
                templateVars.Add("BillingCountry", agreementPdf.BillingCountry);
                templateVars.Add("BillingState", agreementPdf.BillingState);
                templateVars.Add("BillingZip", agreementPdf.BillingZip);
                templateVars.Add("InitialApt", agreementPdf.InitialApt);
                templateVars.Add("InstallTypeName", agreementPdf.InstallTypeName);
                templateVars.Add("DateOfAgreement", DateTime.Now.DateFormat());

                if (agreementPdf.InstallTypeName.ToLower() == "prewire" || agreementPdf.InstallTypeName.ToLower() == "upgrade")
                {
                    templateVars.Add("CustomerSignatureCondition", "<b>*PREWIRED OR UPGRADED SYSTEMS MUST Sign HERE</b><img src='" + agreementPdf.CustomerSignature + "' style='width:300px; height:100px;' />");
                }
                double collectedamount = 0.0;
                var CustomerPaymentInfo = GetAllPaidPaymentInfoCustomer(new Guid(agreementPdf.CompanyId), agreementPdf.CustomerAgreement.FirstOrDefault().CustomerId);
                if (agreementPdf.AdvanceServiceFeeTotal > 0)
                {
                    AdvanceMonitoringFee = agreementPdf.AdvanceServiceFeeTotal;
                }


                var ContractCreatedDateVal = agreementPdf.ContractCreatedDateVal;
                if (string.IsNullOrEmpty(ContractCreatedDateVal))
                {
                    ContractCreatedDateVal = DateTime.UtcNow.ToString("M/d/yyyy");
                }

                var upfrontamount = agreementPdf.UpfrontAddOnTotal;
                var EquipDiscount = 0.00;
                var SubTotalBeforeDiscount = 0.00;
                var EquipIsPcnt = false;
                var EqpActualDiscount = 0.00;
                var EqpDiscAmount = 0.00;
                if (agreementPdf.ACHDiscountAmount > 0)
                {

                    templateVars.Add("ACHDiscountAmount", "<tr style='background-color:#bfbfbf; color:#000; height: 30px;'><td style='text-align:center; border: 2px solid #000; border-right:1px solid #000; padding:0px 10px; font-size:15px; font-weight:bold;' colspan = 4>Discount</td><td style='text-align:center; border: 2px solid #000; border-left:1px solid #000;'><span>-</span>" + currentCurrency + FormatAmount(agreementPdf.ACHDiscountAmount) + "</td></tr>");
                    var serviceFeeTotal = agreementPdf.MonthlyServiceFeeTotal - agreementPdf.ACHDiscountAmount;

                    if (agreementPdf.IsUpfrontPromo == true)
                    {
                        upfrontamount = agreementPdf.UpfrontAddOnTotalPromo;
                    }

                    var activationamount = agreementPdf.ActivationFee;
                    var onetimeservicefee = 0.0;
                    if (agreementPdf.NotARBEnabledTotalPrice > 0)
                    {
                        onetimeservicefee = agreementPdf.NotARBEnabledTotalPrice;
                        //templateVars.Add("OneTimeServiceFee", "<tr style='height: 25px;'><td valign='middle' style='font-weight:bold; border: 1px solid #000; text-align:right; padding-right:5px; background-color:#f3f3f3;'>+  One Time Service Fee</td><td style='border: 1px solid #000;text-align: right;padding-right: 10px;'>" + currentCurrency + FormatAmount(onetimeservicefee) + "</td></tr>");
                    }
                    agreementPdf.NonConfirmingFee = agreementPdf.NonConfirmingFee;
                    if (agreementPdf.IsServicePromo != true)
                    {
                        MonthlyServiceFee = serviceFeeTotal;
                    }
                    var payCus = _PaymentInfoCustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and Payfor = 'Service'", agreementPdf.CustomerAgreement.FirstOrDefault().CustomerId)).FirstOrDefault();
                    if (payCus != null && payCus.ForMonths.HasValue && payCus.ForMonths > 1)
                    {
                        AdvanceMonitoringFee = MonthlyServiceFee * (payCus.ForMonths.Value - 1);
                    }
                    var subtotalamount = MonthlyServiceFee + AdvanceMonitoringFee + upfrontamount + activationamount + agreementPdf.NonConfirmingFee + onetimeservicefee;
                    double TaxableSubtotal = 0.0;

                    if (glLabourFee != null && glLabourFee.Value == "true")
                    {
                        subtotalamount = subtotalamount + agreementPdf.LabourFee;
                        TaxableSubtotal = subtotalamount - agreementPdf.LabourFee;
                    }
                    else
                    {
                        TaxableSubtotal = subtotalamount;
                    }
                    agreementPdf.TaxTotal = TaxableSubtotal * (agreementPdf.Tax / 100);
                    var duesignamount = subtotalamount + agreementPdf.TaxTotal;
                    if (agreementPdf.IsServicePromo != true)
                    {
                        MonthlyServiceFee = serviceFeeTotal;
                    }

                    ////  " Mayur " Discount change if ACH payment" :: Start
                    foreach (Equipment d in agreementPdf.EquipmentList)
                    {
                        if (d.DiscountInCurrency > 0.0)
                        {
                            EquipIsPcnt = false;
                            EquipDiscount = d.DiscountInCurrency;
                        }
                        else
                        {
                            EquipIsPcnt = true;
                            EquipDiscount = d.DiscountPercentage;
                        }
                        break;
                    }
                    if (EquipIsPcnt)
                    {
                        EqpDiscAmount = ((upfrontamount * EquipDiscount) / 100);
                    }
                    else
                    {
                        EqpDiscAmount = EquipDiscount;
                    }
                    EqpActualDiscount = EqpDiscAmount;
                    subtotalamount = MonthlyServiceFee + AdvanceMonitoringFee + upfrontamount + activationamount + agreementPdf.NonConfirmingFee + onetimeservicefee;
                    SubTotalBeforeDiscount = subtotalamount;
                    subtotalamount = subtotalamount - EqpActualDiscount;



                    TaxableSubtotal = 0.0;
                    if (glLabourFee != null && glLabourFee.Value == "true")
                    {
                        subtotalamount = subtotalamount + agreementPdf.LabourFee;
                        TaxableSubtotal = subtotalamount - agreementPdf.LabourFee;
                    }
                    else
                    {
                        TaxableSubtotal = subtotalamount;
                    }
                    agreementPdf.TaxTotal = TaxableSubtotal * (agreementPdf.Tax / 100);
                    duesignamount = subtotalamount + agreementPdf.TaxTotal;

                    //// Mayur :: New onetimeservice + services join showing in aggrement :: Start

                    var OnetimeServiceContent = "";
                    var OnetimeServiceItems = "";
                    var subtotal = 0.0;
                    var subtotal1 = 0.0;
                    var subtotal2 = 0.0;
                    var tax1 = 0.0;
                    var tax2 = 0.0;
                    var totaltax = 0.0;
                    var Finaltotal = 0.0;


                    if (!string.IsNullOrWhiteSpace(agreementPdf.ContractType) && agreementPdf.ContractType.ToLower() == "commercialfire")
                    {
                        if (agreementPdf.ServiceList.Count > 0)
                        {
                            foreach (var item in agreementPdf.ServiceList)
                            {
                                OnetimeServiceItems = OnetimeServiceItems +
                                "<tr style='border-bottom:1px solid #ccc'>" +
                                "<td style=\"padding:5px 0px 5px 20px\">" +
                                "<strong><label>" + item.Name + "</label></strong>" +
                                "</td>" +
                                "<td style=\"padding:5px;text-align:center\"><strong><label>" + currentCurrency + FormatAmount(item.Total) + "</label></strong></td>" +
                                "</tr>";

                                subtotal1 += subtotal + (double)item.Total;

                                tax1 += subtotal1 * (agreementPdf.Tax / 100);

                            }

                        }
                        if (agreementPdf.NotARBEnabledServiceList.Count > 0)
                        {
                            foreach (var item in agreementPdf.NotARBEnabledServiceList)
                            {
                                OnetimeServiceItems = OnetimeServiceItems +
                                "<tr style='border-bottom:1px solid #ccc'>" +
                                "<td style=\"padding:5px 0px 5px 20px\">" +
                                "<strong><label>" + " + One Time Service Fee - " + item.Name + "</label></strong>" +
                                "</td>" +
                                "<td style=\"padding:5px;text-align:center\"><strong><label>" + currentCurrency + FormatAmount(item.Total) + "</label></strong></td>" +
                                "</tr>";

                                subtotal2 += subtotal + (double)item.Total;
                                tax2 += subtotal2 * (agreementPdf.Tax / 100);



                            }
                        }

                        if (agreementPdf.NonConfirmingFee > 0)
                        {
                            NonConfirmingFeeDivCommFire = "<tr style='border-bottom:1px solid #ccc'><td style=\"padding:5px 0px 5px 20px\"><strong><label> + Non Conforming Fee </label></strong></td><td style=\"padding:5px;text-align:center\"><strong><label>{0}</label></strong></td></tr>";
                            NonConfirmingFeeDivCommFire = string.Format(NonConfirmingFeeDivCommFire, currentCurrency + FormatAmount(agreementPdf.NonConfirmingFee));
                            OnetimeServiceItems += NonConfirmingFeeDivCommFire;
                            subtotal2 += subtotal + agreementPdf.NonConfirmingFee;
                            tax2 = subtotal2 * (agreementPdf.Tax / 100);
                        }

                        subtotal = subtotal1 + subtotal2;
                        totaltax = tax1 + tax2;
                        Finaltotal = Finaltotal + subtotal + totaltax;
                        OnetimeServiceContent = "<table style=\"width:100%;float:left;border-collapse:collapse;margin-bottom:30px;margin-top:10px;\">" +
                           " <thead> " +
                           " <tr style = 'background-color:#4f90bb;color:white;/* width:100%; */border:1px solid #4f90bb' >" +
                           " <th style = 'width: 80%;text-align:left;padding:5px 0px 5px 20px;border-right:1px solid #fff' > SERVICE </th>" +
                           " <th style = 'width: 14%;padding:5px 0px;border-right:1px solid #fff;text-align:center' > PRICE </th></tr>" +
                           " </thead > <tbody>" +
                           OnetimeServiceItems +

                           /// subtotal
                           "<tr style = '/* border-bottom:1px solid #ccc; */' >" +
                           "<td style = 'border: 0px 0px 0px 5px;text-align: right;border: none;' ><strong><label> SubTotal </label></strong></td>" +
                           "<td style = 'padding:5px;text-align:center' ><strong><label>" + currentCurrency + FormatAmount(subtotal) + " </label></strong></td>" +
                           "</tr> " +

                           /// totalTax
                           "<tr style = '/* border-bottom:1px solid #ccc; */'>" +
                           "<td style = 'padding:0px 0px 0px 10px;text-align: right;' ><strong><label> Tax </label></strong></td> " +
                           "<td style = 'padding:0px;text-align:center' ><strong><label>" + currentCurrency + FormatAmount(totaltax) + " </label></strong></td>" +
                           "</tr> " +

                           // Total

                           "<tr style = '/* border-bottom:1px solid #ccc; */' >" +
                           "<td style = 'padding:0px 0px 0px 5px;text-align: right; ' ><strong><label> Total </label></strong></td>" +
                           "<td style = 'padding:5px;text-align:center' ><strong><label>" + currentCurrency + FormatAmount(Finaltotal) + "</label></strong></td>" +
                           "</tr>" +



                           " </tbody></table> ";

                        templateVars.Add("OneTimeServiceFee", OnetimeServiceContent);
                        templateVars.Remove("CommercialServiceList");
                    }
                    else
                    {
                        //if (agreementPdf.ServiceList.Count > 0)
                        //{
                        //    foreach (var item in agreementPdf.ServiceList)
                        //    {
                        //        OnetimeServiceItems = OnetimeServiceItems +
                        //        "<tr style='border-bottom:1px solid #ccc;'>" +
                        //        "<td style=\"padding:5px 0px 5px 20px\">" +
                        //        "<label>" + item.Name + "</label>" +
                        //        "</td>" +
                        //        "<td style='padding:5px;text-align:center'><label>" + currentCurrency + FormatAmount(item.Total) + "</label></td>" +
                        //        "</tr>";

                        //        subtotal1 = subtotal + (double)item.Total;

                        //        tax1 = subtotal1 * (agreementPdf.Tax / 100);

                        //    }

                        //}
                        //if (agreementPdf.NotARBEnabledServiceList.Count > 0)
                        //{
                        //    foreach (var item in agreementPdf.NotARBEnabledServiceList)
                        //    {
                        //        OnetimeServiceItems = OnetimeServiceItems +
                        //        "<tr style='border-bottom:1px solid #ccc'>" +
                        //        "<td style=\"padding:5px 0px 5px 20px\">" +
                        //        "<label>" + " + One Time Service Fee - " + item.Name + "</label>" +
                        //        "</td>" +
                        //        "<td style=\"padding:5px;text-align:center\"><label>" + currentCurrency + FormatAmount(item.Total) + "</label></td>" +
                        //        "</tr>";

                        //        subtotal2 = subtotal + (double)item.Total;
                        //        tax2 = subtotal2 * (agreementPdf.Tax / 100);



                        //    }
                        //}
                        //subtotal = subtotal1 + subtotal2;
                        //totaltax = tax1 + tax2;
                        //Finaltotal = Finaltotal + subtotal + totaltax;
                        //OnetimeServiceContent = "<table style=\"width:100%;float:left;border-collapse:collapse;margin-bottom:5px;margin-top:10px;font-size:12px\">" +
                        //   " <thead> " +
                        //   " <tr style = 'background-color:#000000;color:white;font-size:12px;/* width:100%; */border:1px solid #4f90bb' >" +
                        //   " <th style = 'width: 80%;text-align:left;padding:5px 0px 5px 20px;border-right:1px solid #fff' > SERVICE </th>" +
                        //   " <th style = 'width: 14%;padding:5px 0px;border-right:1px solid #fff;text-align:center' > PRICE </th></tr>" +
                        //   " </thead > <tbody>" +
                        //   OnetimeServiceItems +

                        //   /// subtotal
                        //   "<tr style = '/* border-bottom:1px solid #ccc; */' >" +
                        //   "<td style = 'padding: 0px 0px 0px 0px;text-align: right;border: none;' ><strong><label> SubTotal </label></strong></td>" +
                        //   "<td style = 'padding:0px;text-align:center' ><strong><label>" + currentCurrency + FormatAmount(subtotal) + " </label></strong></td>" +
                        //   "</tr> " +

                        //   /// totalTax
                        //   "<tr style = '/* border-bottom:1px solid #ccc; */'>" +
                        //   "<td style = 'padding:0px 0px 0px 0px;text-align: right;' ><strong><label> Tax </label></strong></td> " +
                        //   "<td style = 'padding:0px;text-align:center' ><strong><label>" + currentCurrency + FormatAmount(totaltax) + " </label></strong></td>" +
                        //   "</tr> " +

                        //   // Total

                        //   "<tr style = '/* border-bottom:1px solid #ccc; */' >" +
                        //   "<td style = 'padding:0px 0px 0px 0px;text-align: right; ' ><strong><label> Total </label></strong></td>" +
                        //   "<td style = 'padding:0px;text-align:center' ><strong><label>" + currentCurrency + FormatAmount(Finaltotal) + "</label></strong></td>" +
                        //   "</tr>" +



                        //   " </tbody></table> ";

                        //templateVars.Add("OneTimeServiceFee", OnetimeServiceContent);
                        //templateVars.Remove("CommercialServiceList");

                        onetimeservicefee = agreementPdf.NotARBEnabledTotalPrice;
                        templateVars.Add("OneTimeServiceFee", "<table style='width:100%; float:left; border-collapse:collapse; margin-bottom:30px; margin-top:10px;'><thead><tr style='background-color:#4f90bb;color:white;width:100%;border:1px solid #4f90bb'><th style='width:86%; text-align:left; padding:5px 0px 5px 20px; border-right:1px solid #fff'></th></tr></thead><tbody>"
                               + "<tr style='height:40px;'><td valign='middle' style='font-weight:bold; border: 1px solid #000; text-align:left; padding-left:15px; background-color:#f3f3f3;'>+One Time Service Fee</td><td style='border: 1px solid #000;text-align:center;padding-left:10px;'>"
                               + currentCurrency + FormatAmount(onetimeservicefee) + "</td></tr>"
                               + "</tbody></table>");
                    }
                    //// Mayur :: New onetimeservice + services join showing in aggrement :: End

                    ////  " Mayur " Discount change if ACH payment" :: End

                    templateVars.Add("MonthlyServiceFeeTotal", currentCurrency + FormatAmount(serviceFeeTotal));
                    templateVars.Add("MonthlyServiceFeeFinalTotal", currentCurrency + FormatAmount(MonthlyServiceFee));
                    templateVars.Add("UpfrontAddOnTotal", currentCurrency + FormatAmount(upfrontamount));
                    templateVars.Add("ActivationFee", currentCurrency + FormatAmount(activationamount));
                    templateVars.Add("LabourFeeDfw", LabourfeeDfw);
                    templateVars.Add("LabourFeeRmr", LabourFeeRmR);
                    ////  " Mayur " Discount change if ACH payment" :: Start
                    templateVars.Add("BeforeSubTotal", currentCurrency + FormatAmount(SubTotalBeforeDiscount));
                    templateVars.Add("EquipDiscount", currentCurrency + FormatAmount(EqpActualDiscount));
                    ////  " Mayur " Discount change if ACH payment" :: End
                    templateVars.Add("NewSubTotal", currentCurrency + FormatAmount(subtotalamount));
                    templateVars.Add("TotalDueAtSigning", currentCurrency + FormatAmount(duesignamount));

                    if (CustomerPaymentInfo != null && CustomerPaymentInfo.Count > 0)
                    {
                        foreach (var item in CustomerPaymentInfo)
                        {
                            if (item.Payfor.ToLower() == "activation fee" && (item.PaymentType.ToLower() == "ach" || item.PaymentType.ToLower() == "cc"))
                            {
                                collectedamount += activationamount;
                                collectedamount += agreementPdf.NonConfirmingFee;
                            }
                            if (item.Payfor.ToLower() == "service" && (item.PaymentType.ToLower() == "ach" || item.PaymentType.ToLower() == "cc"))
                            {
                                collectedamount += serviceFeeTotal;
                            }
                            if (item.Payfor.ToLower() == "equipment" && (item.PaymentType.ToLower() == "ach" || item.PaymentType.ToLower() == "cc"))
                            {
                                collectedamount += upfrontamount;
                            }
                        }
                        var taxcollectedtotal = collectedamount * (agreementPdf.Tax / 100);
                        collectedamount += taxcollectedtotal;
                    }
                    if (collectedamount > 0)
                    {
                        templateVars.Add("TotalCollectedAmount", "<tr style='height: 25px;'><td valign='middle' style='font-weight:bold; border: 1px solid #000; text-align:right; padding: 15px 5px;'>Collected</td><td style='border:1px solid #000; padding: 15px 5px;text-align: right;padding-right: 10px;'>" + currentCurrency + FormatAmount(collectedamount) + "</td></tr>");
                    }
                }
                else
                {
                    var serviceFeeTotal = agreementPdf.MonthlyServiceFeeTotal;

                    if (agreementPdf.IsUpfrontPromo == true)
                    {
                        upfrontamount = agreementPdf.UpfrontAddOnTotalPromo;
                    }
                    var activationamount = agreementPdf.ActivationFee;


                    //// Mayur :: New onetimeservice + services join showing in aggrement :: Start

                    var onetimeservicefee = 0.0;

                    var OnetimeServiceContent = "";
                    var OnetimeServiceItems = "";
                    var subtotal = 0.0;
                    var subtotal1 = 0.0;
                    var subtotal2 = 0.0;
                    var tax1 = 0.0;
                    var tax2 = 0.0;
                    var totaltax = 0.0;
                    var Finaltotal = 0.0;


                    if (!string.IsNullOrWhiteSpace(agreementPdf.ContractType) && agreementPdf.ContractType.ToLower() == "commercialfire")
                    {
                        if (agreementPdf.ServiceList.Count > 0)
                        {
                            foreach (var item in agreementPdf.ServiceList)
                            {
                                OnetimeServiceItems = OnetimeServiceItems +
                                "<tr style='border-bottom:1px solid #ccc'>" +
                                "<td style=\"padding:5px 0px 5px 20px\">" +
                                "<strong><label>" + item.Name + "</label></strong>" +
                                "</td>" +
                                "<td style=\"padding:5px;text-align:center\"><strong><label>" + currentCurrency + FormatAmount(item.Total) + "</label></strong></td>" +
                                "</tr>";

                                subtotal1 += subtotal + (double)item.Total;

                                tax1 += subtotal1 * (agreementPdf.Tax / 100);

                            }

                        }
                        if (agreementPdf.NotARBEnabledServiceList.Count > 0)
                        {
                            foreach (var item in agreementPdf.NotARBEnabledServiceList)
                            {
                                OnetimeServiceItems = OnetimeServiceItems +
                                "<tr style='border-bottom:1px solid #ccc'>" +
                                "<td style=\"padding:5px 0px 5px 20px\">" +
                                "<strong><label>" + " + One Time Service Fee - " + item.Name + "</label></strong>" +
                                "</td>" +
                                "<td style=\"padding:5px;text-align:center\"><strong><label>" + currentCurrency + FormatAmount(item.Total) + "</label></strong></td>" +
                                "</tr>";

                                subtotal2 += subtotal + (double)item.Total;
                                tax2 += subtotal2 * (agreementPdf.Tax / 100);



                            }
                        }

                        if (agreementPdf.NonConfirmingFee > 0)
                        {
                            NonConfirmingFeeDivCommFire = "<tr style='border-bottom:1px solid #ccc'><td style=\"padding:5px 0px 5px 20px\"><strong><label> + Non Conforming Fee </label></strong></td><td style=\"padding:5px;text-align:center\"><strong><label>{0}</label></strong></td></tr>";
                            NonConfirmingFeeDivCommFire = string.Format(NonConfirmingFeeDivCommFire, currentCurrency + FormatAmount(agreementPdf.NonConfirmingFee));
                            OnetimeServiceItems += NonConfirmingFeeDivCommFire;
                            subtotal2 += subtotal + agreementPdf.NonConfirmingFee;
                            tax2 = subtotal2 * (agreementPdf.Tax / 100);
                        }

                        subtotal = subtotal1 + subtotal2;
                        totaltax = tax1 + tax2;
                        Finaltotal = Finaltotal + subtotal + totaltax;
                        OnetimeServiceContent = "<table style=\"width:100%;float:left;border-collapse:collapse;margin-bottom:30px;margin-top:10px;\">" +

                           " <tr style = 'background-color:#4f90bb;color:white;/* width:100%; */border:1px solid #4f90bb' >" +
                           " <th style = 'width: 80%;text-align:left;padding:5px 0px 5px 20px;border-right:1px solid #fff' > SERVICE </th>" +
                           " <th style = 'width: 14%;padding:5px 0px;border-right:1px solid #fff;text-align:center' > PRICE </th></tr>" +
                           " <tbody>" +
                           OnetimeServiceItems +

                           /// subtotal
                           "<tr style = '/* border-bottom:1px solid #ccc; */' >" +
                           "<td style = 'border: 0px 0px 0px 5px;text-align: right;border: none;' ><strong><label> SubTotal </label></strong></td>" +
                           "<td style = 'padding:5px;text-align:center' ><strong><label>" + currentCurrency + FormatAmount(subtotal) + " </label></strong></td>" +
                           "</tr> " +

                           /// totalTax
                           "<tr style = '/* border-bottom:1px solid #ccc; */'>" +
                           "<td style = 'padding:0px 0px 0px 10px;text-align: right;' ><strong><label> Tax </label></strong></td> " +
                           "<td style = 'padding:0px;text-align:center' ><strong><label>" + currentCurrency + FormatAmount(totaltax) + " </label></strong></td>" +
                           "</tr> " +

                           // Total

                           "<tr style = '/* border-bottom:1px solid #ccc; */' >" +
                           "<td style = 'padding:0px 0px 0px 5px;text-align: right; ' ><strong><label> Total </label></strong></td>" +
                           "<td style = 'padding:5px;text-align:center' ><strong><label>" + currentCurrency + FormatAmount(Finaltotal) + "</label></strong></td>" +
                           "</tr>" +



                           " </tbody></table> ";

                        templateVars.Add("OneTimeServiceFee", OnetimeServiceContent);
                        templateVars.Remove("CommercialServiceList");
                    }
                    else
                    {
                        //if (agreementPdf.ServiceList.Count > 0)
                        //{
                        //    foreach (var item in agreementPdf.ServiceList)
                        //    {
                        //        OnetimeServiceItems = OnetimeServiceItems +
                        //        "<tr style='border-bottom:1px solid #ccc;'>" +
                        //        "<td style=\"padding:5px 0px 5px 20px\">" +
                        //        "<label>" + item.Name + "</label>" +
                        //        "</td>" +
                        //        "<td style='padding:5px;text-align:center'><label>" + currentCurrency + FormatAmount(item.Total) + "</label></td>" +
                        //        "</tr>";

                        //        subtotal1 = subtotal + (double)item.Total;

                        //        tax1 = subtotal1 * (agreementPdf.Tax / 100);

                        //    }

                        //}
                        //if (agreementPdf.NotARBEnabledServiceList.Count > 0)
                        //{
                        //    foreach (var item in agreementPdf.NotARBEnabledServiceList)
                        //    {
                        //        OnetimeServiceItems = OnetimeServiceItems +
                        //        "<tr style='border-bottom:1px solid #ccc'>" +
                        //        "<td style=\"padding:5px 0px 5px 20px\">" +
                        //        "<label>" + " + One Time Service Fee - " + item.Name + "</label>" +
                        //        "</td>" +
                        //        "<td style=\"padding:5px;text-align:center\"><label>" + currentCurrency + FormatAmount(item.Total) + "</label></td>" +
                        //        "</tr>";

                        //        subtotal2 = subtotal + (double)item.Total;
                        //        tax2 = subtotal2 * (agreementPdf.Tax / 100);



                        //    }
                        //}
                        //subtotal = subtotal1 + subtotal2;
                        //totaltax = tax1 + tax2;
                        //Finaltotal = Finaltotal + subtotal + totaltax;
                        //OnetimeServiceContent = "<table style=\"width:100%;float:left;border-collapse:collapse;margin-bottom:5px;margin-top:10px;font-size:12px\">" +
                        //   " <thead> " +
                        //   " <tr style = 'background-color:#000000;color:white;font-size:12px;/* width:100%; */border:1px solid #4f90bb' >" +
                        //   " <th style = 'width: 80%;text-align:left;padding:5px 0px 5px 20px;border-right:1px solid #fff' > SERVICE </th>" +
                        //   " <th style = 'width: 14%;padding:5px 0px;border-right:1px solid #fff;text-align:center' > PRICE </th></tr>" +
                        //   " </thead > <tbody>" +
                        //   OnetimeServiceItems +

                        //   /// subtotal
                        //   "<tr style = '/* border-bottom:1px solid #ccc; */' >" +
                        //   "<td style = 'padding: 0px 0px 0px 0px;text-align: right;border: none;' ><strong><label> SubTotal </label></strong></td>" +
                        //   "<td style = 'padding:0px;text-align:center' ><strong><label>" + currentCurrency + FormatAmount(subtotal) + " </label></strong></td>" +
                        //   "</tr> " +

                        //   /// totalTax
                        //   "<tr style = '/* border-bottom:1px solid #ccc; */'>" +
                        //   "<td style = 'padding:0px 0px 0px 0px;text-align: right;' ><strong><label> Tax </label></strong></td> " +
                        //   "<td style = 'padding:0px;text-align:center' ><strong><label>" + currentCurrency + FormatAmount(totaltax) + " </label></strong></td>" +
                        //   "</tr> " +

                        //   // Total

                        //   "<tr style = '/* border-bottom:1px solid #ccc; */' >" +
                        //   "<td style = 'padding:0px 0px 0px 0px;text-align: right; ' ><strong><label> Total </label></strong></td>" +
                        //   "<td style = 'padding:0px;text-align:center' ><strong><label>" + currentCurrency + FormatAmount(Finaltotal) + "</label></strong></td>" +
                        //   "</tr>" +



                        //   " </tbody></table> ";

                        //templateVars.Add("OneTimeServiceFee", OnetimeServiceContent);
                        //templateVars.Remove("CommercialServiceList");

                        onetimeservicefee = agreementPdf.NotARBEnabledTotalPrice;
                        templateVars.Add("OneTimeServiceFee", "<table style='width:100%; float:left; border-collapse:collapse; margin-bottom:30px; margin-top:10px;'><thead><tr style='background-color:#4f90bb;color:white;width:100%;border:1px solid #4f90bb'><th style='width:86%; text-align:left; padding:5px 0px 5px 20px; border-right:1px solid #fff'></th></tr></thead><tbody>"
                               + "<tr style='height:40px;'><td valign='middle' style='font-weight:bold; border: 1px solid #000; text-align:left; padding-left:15px; background-color:#f3f3f3;'>+One Time Service Fee</td><td style='border: 1px solid #000;text-align:center;padding-left:10px;'>"
                               + currentCurrency + FormatAmount(onetimeservicefee) + "</td></tr>"
                               + "</tbody></table>");
                    }
                    //// Mayur :: New onetimeservice + services join showing in aggrement :: End

                    ////////////////////////// old Onetimeservice templete
                    ///
                    // var onetimeservicefee = 0.0;
                    //if (agreementPdf.NotARBEnabledTotalPrice > 0)
                    //{
                    //    onetimeservicefee = agreementPdf.NotARBEnabledTotalPrice;
                    //    //templateVars.Add("OneTimeServiceFee", "< table style = 'width:100%'; float:left; border-collapse:collapse; margin-bottom:30px; margin-top:10px;' >< thead >< tr style = 'background-color:#4f90bb;color:white;width:100%;border:1px solid #4f90bb' >< th style = 'width:86%; text-align:left; padding:5px 0px 5px 20px; border-right:1px solid #fff' ></ th ></ tr ></ thead >< tbody > "
                    //    //     + "< tr style = 'height: 25px;' >< td valign = 'middle' style = 'font-weight:bold; border: 1px solid #000; text-align:right; padding-right:5px; background-color:#f3f3f3;' > +One Time Service Fee </ td >< td style = 'border: 1px solid #000;text-align: right;padding-right: 10px;' > " + currentCurrency + FormatAmount(onetimeservicefee) + "</ td ></ tr >
                    //    //     + "</ tbody ></ table > ");
                    //    templateVars.Add("OneTimeServiceFee", "<table style='width:100%; float:left; border-collapse:collapse; margin-bottom:30px; margin-top:10px;'><thead><tr style='background-color:#4f90bb;color:white;width:100%;border:1px solid #4f90bb'><th style='width:86%; text-align:left; padding:5px 0px 5px 20px; border-right:1px solid #fff'></th></tr></thead><tbody>"
                    //           + "<tr style='height:40px;'><td valign='middle' style='font-weight:bold; border: 1px solid #000; text-align:left; padding-left:15px; background-color:#f3f3f3;'>+One Time Service Fee</td><td style='border: 1px solid #000;text-align:center;padding-left:10px;'>"
                    //           + currentCurrency + FormatAmount(onetimeservicefee) + "</td></tr>"
                    //           + "</tbody></table>");

                    //}

                    /////////////////////

                    if (agreementPdf.IsServicePromo != true)
                    {
                        MonthlyServiceFee = serviceFeeTotal;
                    }
                    agreementPdf.NonConfirmingFee = agreementPdf.NonConfirmingFee;

                    foreach (Equipment d in agreementPdf.EquipmentList)
                    {
                        if (d.DiscountInCurrency > 0.0)
                        {
                            EquipIsPcnt = false;
                            EquipDiscount = d.DiscountInCurrency;
                        }
                        else
                        {
                            EquipIsPcnt = true;
                            EquipDiscount = d.DiscountPercentage;
                        }
                        break;
                    }
                    if (EquipIsPcnt)
                    {
                        EqpDiscAmount = ((upfrontamount * EquipDiscount) / 100);
                    }
                    else
                    {
                        EqpDiscAmount = EquipDiscount;
                    }
                    EqpActualDiscount = EqpDiscAmount;
                    var subtotalamount = MonthlyServiceFee + AdvanceMonitoringFee + upfrontamount + activationamount + agreementPdf.NonConfirmingFee + onetimeservicefee;
                    SubTotalBeforeDiscount = subtotalamount;
                    subtotalamount = subtotalamount - EqpActualDiscount;
                    var taxRate = agreementPdf.Tax / 100.0;
                    if (!string.IsNullOrWhiteSpace(agreementPdf.EstimatorId))
                    {
                        agreementPdf.TaxTotal = agreementPdf.TaxTotal;
                    }
                    else
                    {
                        var indivisualSubtotaltex =
                         Math.Round(MonthlyServiceFee * taxRate, 2) +
                         Math.Round(AdvanceMonitoringFee * taxRate, 2) +
                         Math.Round(upfrontamount * taxRate, 2) +
                         Math.Round(activationamount * taxRate, 2) +
                         Math.Round(agreementPdf.NonConfirmingFee * taxRate, 2) +
                         Math.Round(onetimeservicefee * taxRate, 2);
                        double TaxableSubtotal = 0.0;
                        if (glLabourFee != null && glLabourFee.Value == "true")
                        {
                            indivisualSubtotaltex = indivisualSubtotaltex + agreementPdf.LabourFee;
                            TaxableSubtotal = indivisualSubtotaltex - agreementPdf.LabourFee;
                        }
                        else
                        {
                            TaxableSubtotal = indivisualSubtotaltex;
                        }
                        agreementPdf.TaxTotal = TaxableSubtotal;
                    }


                    var duesignamount = subtotalamount + agreementPdf.TaxTotal;

                    templateVars.Add("MonthlyServiceFeeTotal", currentCurrency + FormatAmount(serviceFeeTotal));
                    templateVars.Add("MonthlyServiceFeeFinalTotal", currentCurrency + FormatAmount(MonthlyServiceFee));
                    templateVars.Add("UpfrontAddOnTotal", currentCurrency + FormatAmount(upfrontamount));
                    templateVars.Add("ActivationFee", currentCurrency + FormatAmount(activationamount));
                    templateVars.Add("LabourFeeDfw", LabourfeeDfw);
                    templateVars.Add("LabourFeeRmr", LabourFeeRmR);
                    templateVars.Add("BeforeSubTotal", currentCurrency + FormatAmount(SubTotalBeforeDiscount));
                    templateVars.Add("EquipDiscount", currentCurrency + FormatAmount(EqpActualDiscount));
                    templateVars.Add("NewSubTotal", currentCurrency + FormatAmount(subtotalamount));
                    templateVars.Add("TotalDueAtSigning", currentCurrency + FormatAmount(duesignamount));
                    if (CustomerPaymentInfo != null && CustomerPaymentInfo.Count > 0)
                    {
                        foreach (var item in CustomerPaymentInfo)
                        {
                            if (item.Payfor.ToLower() == "activation fee" && (item.PaymentType.ToLower() == "ach" || item.PaymentType.ToLower() == "cc"))
                            {
                                collectedamount += agreementPdf.ActivationFee;
                                collectedamount += agreementPdf.NonConfirmingFee;
                            }
                            if (item.Payfor.ToLower() == "service" && (item.PaymentType.ToLower() == "ach" || item.PaymentType.ToLower() == "cc"))
                            {
                                collectedamount += agreementPdf.MonthlyServiceFeeTotal;
                            }
                            if (item.Payfor.ToLower() == "equipment" && (item.PaymentType.ToLower() == "ach" || item.PaymentType.ToLower() == "cc"))
                            {
                                collectedamount += agreementPdf.UpfrontAddOnTotal;
                            }
                        }
                        var taxcollectedtotal = collectedamount * (agreementPdf.Tax / 100);
                        collectedamount += taxcollectedtotal;
                    }
                    if (collectedamount > 0)
                    {
                        templateVars.Add("TotalCollectedAmount", "<tr style='height: 25px;'><td valign='middle' style='font-weight:bold; border: 1px solid #000; text-align:right; padding: 15px 5px;'>Collected</td><td style='border:1px solid #000; padding: 15px 5px;text-align: right;padding-right: 10px;'>" + currentCurrency + FormatAmount(collectedamount) + "</td></tr>");
                    }
                }


                templateVars.Add("TotalMonthlyMonitoring", currentCurrency + FormatAmount(agreementPdf.TotalMonthlyMonitoring));

                templateVars.Add("UpfrontAddOnTotals", currentCurrency + FormatAmount(agreementPdf.UpfrontAddOnTotal));
                if (agreementPdf.IsUpfrontPromo == true)
                {
                    string upfrontpromo = string.Format(@"  <tr style='background-color:#f2f2f2; color:#000; height: 30px;'>
                        <td style='text-align:right;font-weight:bold; background-color:#fff;'></td>

                        <td colspan='4' style='text-align:right; border:2px solid #000; border-right:1px solid #000; padding:0px 10px; font-size:15px; font-weight:bold;'>
                            UPFRONT ADD-ON TOTAL(Promo Discount)
                        </td>
                        <td style='text-align:center; border:2px solid #000; border-left:1px solid #000;'>
                            {0}
                        </td>
                    </tr>", currentCurrency + FormatAmount(agreementPdf.UpfrontAddOnTotalPromo));
                    templateVars.Add("UpfrontAddOnTotalsPromo", upfrontpromo);
                }

                templateVars.Add("ResidentialTechFirstHourCost", currentCurrency + agreementPdf.ResidentialTechFirstHourCost);
                templateVars.Add("CommercialTechFirstHourCost", currentCurrency + agreementPdf.CommercialTechFirstHourCost);

                templateVars.Add("KazarLogo", agreementPdf.KazarLogo);
                templateVars.Add("CompanyName", agreementPdf.CompanyName);
                templateVars.Add("CompanyStreet", agreementPdf.CompanyStreet);
                templateVars.Add("CompanySate", agreementPdf.CompanySate);
                templateVars.Add("CompanyWebsite", agreementPdf.CompanyWebsite);
                templateVars.Add("CompanyPhone", agreementPdf.CompanyPhone);
                templateVars.Add("CustomerName", agreementPdf.OwnerName);
                templateVars.Add("ContractCreatedDateVal", ContractCreatedDateVal);
                templateVars.Add("BusinessName", agreementPdf.BusinessName);
                templateVars.Add("BusinessNameWithDBA", agreementPdf.BusinessName + (!string.IsNullOrWhiteSpace(agreementPdf.DoingBusinessAs) ? (" (DBA: " + agreementPdf.DoingBusinessAs + ")") : ""));
                templateVars.Add("EffectiveDate", agreementPdf.EffectiveDate);
                templateVars.Add("OwnerAddress", agreementPdf.OwnerAddress);
                templateVars.Add("OwnerEmail", agreementPdf.OwnerEmail);
                templateVars.Add("OwnerPhone", Extentions.PhoneNumberFormatNew(agreementPdf.OwnerPhone));
                templateVars.Add("CustomerSignature", agreementPdf.CustomerSignature);
                templateVars.Add("ContractCreatedDate", agreementPdf.ContractCreatedDateVal);
                //templateVars.Add("CustomerSignatureDate", agreementPdf.CustomerSignatureDate != null ? agreementPdf.CustomerSignatureDate.Value.ToShortDateString() : "");
                //templateVars.Add("CustomerSignatureDate", agreementPdf.CustomerSignatureStringDate);
                templateVars.Add("CustomerSignatureDate", agreementPdf.CustomerSignatureStringDateVal != null && agreementPdf.CustomerSignatureStringDateVal != new DateTime() ? agreementPdf.CustomerSignatureStringDateVal.ToString("M/dd/yy") : "");
                templateVars.Add("CustomerSignatureDateDay", !string.IsNullOrWhiteSpace(agreementPdf.CustomerSignatureStringDate) ? (Convert.ToDateTime(agreementPdf.CustomerSignatureStringDateVal)).DateFormat("day") : "_");
                templateVars.Add("CustomerSignatureDateMonth", !string.IsNullOrWhiteSpace(agreementPdf.CustomerSignatureStringDate) ? (Convert.ToDateTime(agreementPdf.CustomerSignatureStringDateVal)).DateFormat("monthName") : "_");
                templateVars.Add("CustomerSignatureDateYear", !string.IsNullOrWhiteSpace(agreementPdf.CustomerSignatureStringDate) ? (Convert.ToDateTime(agreementPdf.CustomerSignatureStringDateVal)).DateFormat("year") : "_");
                if (!string.IsNullOrWhiteSpace(agreementPdf.CustomerSignature))
                {
                    templateVars.Add("CompanySignature", agreementPdf.CompanySignature);
                    templateVars.Add("CompanySignatureDate", agreementPdf.CompanySignatureDate);
                }

                if (agreementPdf.ACHDiscountAmount > 0)
                {
                    var serviceFeeTotal = agreementPdf.MonthlyServiceFeeTotal - agreementPdf.ACHDiscountAmount;

                    templateVars.Add("MonthlyMonitoringFee", currentCurrency + FormatAmount(serviceFeeTotal));
                }
                else
                {
                    templateVars.Add("MonthlyMonitoringFee", currentCurrency + FormatAmount(agreementPdf.MonthlyServiceFeeTotal));
                }
                templateVars.Add("Tax", currentCurrency + FormatAmount(agreementPdf.TaxTotal));
                templateVars.Add("Total", currentCurrency + FormatAmount(agreementPdf.Total));
                templateVars.Add("CompanyLogo", agreementPdf.CompanyLogo);
                templateVars.Add("DateOfTransaction", agreementPdf.DateOfTransaction.ToString("MMMM dd yyyy"));

                if (templateid.HasValue && templateid.Value > 0)
                {
                    templateVars.Add("Day", DateTime.Now.DateFormat("day"));
                    templateVars.Add("Month", DateTime.Now.DateFormat("monthName"));
                    templateVars.Add("Year", DateTime.Now.DateFormat("year"));
                }
                else
                {
                    templateVars.Add("Date", agreementPdf.DateOfTransaction.ToString("dd"));
                    templateVars.Add("Month", agreementPdf.DateOfTransaction.ToString("MMMM"));
                    templateVars.Add("Year", agreementPdf.DateOfTransaction.ToString("yyyy"));
                }
                double SubscribedMonths = 0;
                double MonthlyMonitoringFee = 0;

                double.TryParse(agreementPdf.SubscribedMonths, out SubscribedMonths);
                double.TryParse(agreementPdf.MonthlyMonitoringFee, out MonthlyMonitoringFee);


                templateVars.Add("SubscribedMonths", string.Format("{0} {1}", agreementPdf.SubscribedMonthsInWord, SubscribedMonthsText));
                templateVars.Add("SubscribedMonthsUperCase", string.Format("{0} {1}", agreementPdf.SubscribedMonthsInWord.ToUpper(), SubscribedMonthsText.ToUpper()));
                templateVars.Add("RenewalMonths", string.Format("{0} {1}", agreementPdf.RenewalMonths, "month"));
                templateVars.Add("RenewalTerm", string.Format("{0}", agreementPdf.RenewalMonths));
                templateVars.Add("TotalPayments", currentCurrency + SubscribedMonths * MonthlyMonitoringFee);
                templateVars.Add("Subtotal", currentCurrency + FormatAmount(agreementPdf.Subtotal));
                templateVars.Add("RevisionDate", "<span style='float:right; margin-top:25px;'>Rev. " + DateTime.Now.ToString("MMM") + DateTime.Now.ToString("yyyy") + "</span>");
                if (agreementPdf.ACHDiscountAmount > 0)
                {
                    templateVars.Add("ServiceSubtotal", "<tr style='background-color:#bfbfbf; color:#000; height: 30px;'><td style='text-align:center; border: 2px solid #000; border-right:1px solid #000; padding:0px 10px; font-size:15px; font-weight:bold;' colspan = 4>SubTotal</td><td style='text-align:center; border: 2px solid #000; border-left:1px solid #000;'>" + currentCurrency + FormatAmount(agreementPdf.TotalMonthlyMonitoring) + "</td></tr>");
                }
                if (!string.IsNullOrWhiteSpace(agreementPdf.BusinessName) && !string.IsNullOrWhiteSpace(agreementPdf.DoingBusinessAs))
                {
                    if (templateid.HasValue && templateid.Value > 0)
                    {
                        templateVars.Add("OwnerName", agreementPdf.DisplayName);
                    }
                    else
                    {
                        templateVars.Add("OwnerName", "<td valign = 'top' style = 'padding-left:10px;' colspan = '3'>Customer Name<div>" + agreementPdf.OwnerName + "</div></td>");
                    }
                    templateVars.Add("OwnerBusinessName", "<td valign = 'top' style = 'padding-left:10px;' colspan = '2'>Business Name<div>" + agreementPdf.BusinessName + "</div></td>");
                    templateVars.Add("OwnerDBA", "<td valign = 'top' style = 'padding-left:10px;' colspan = '2'>Doing Business As<div>" + agreementPdf.DoingBusinessAs + "</div></td>");
                    templateVars.Add("CancellationOwnerName", "<td valign = 'top' colspan = '3'><div>" + agreementPdf.DispalyName + "</div></td>");
                    templateVars.Add("OwnerDisplayName", "<td valign = 'top' style = 'padding-left:10px;' colspan = '3'>Customer Name<div>" + agreementPdf.DispalyName + "</div></td>");
                }
                else if (!string.IsNullOrWhiteSpace(agreementPdf.BusinessName))
                {
                    if (templateid.HasValue && templateid.Value > 0)
                    {
                        templateVars.Add("OwnerName", agreementPdf.DisplayName);
                    }
                    else
                    {
                        templateVars.Add("OwnerName", "<td valign = 'top' style = 'padding-left:10px;' colspan = '5'>Customer Name<div>" + agreementPdf.OwnerName + "</div></td>");
                    }
                    templateVars.Add("OwnerDisplayName", "<td valign = 'top' style = 'padding-left:10px;' colspan = '3'>Customer Name<div>" + agreementPdf.DispalyName + "</div></td>");
                    templateVars.Add("OwnerBusinessName", "<td valign = 'top' style = 'padding-left:10px;' colspan = '2'>Business Name<div>" + agreementPdf.BusinessName + "</div></td>");
                    templateVars.Add("CancellationOwnerName", "<td valign = 'top' colspan = '3'><div>" + agreementPdf.DispalyName + "</div></td>");
                }
                else if (!string.IsNullOrWhiteSpace(agreementPdf.DoingBusinessAs))
                {
                    if (templateid.HasValue && templateid.Value > 0)
                    {
                        templateVars.Add("OwnerName", agreementPdf.DisplayName);
                    }
                    else
                    {
                        templateVars.Add("OwnerName", "<td valign = 'top' style = 'padding-left:10px;' colspan = '5'>Customer Name<div>" + agreementPdf.OwnerName + "</div></td>");
                    }
                    templateVars.Add("OwnerDisplayName", "<td valign = 'top' style = 'padding-left:10px;' colspan = '3'>Customer Name<div>" + agreementPdf.DispalyName + "</div></td>");
                    templateVars.Add("OwnerDBA", "<td valign = 'top' style = 'padding-left:10px;' colspan = '2'>Doing Business As<div>" + agreementPdf.DoingBusinessAs + "</div></td>");
                    templateVars.Add("CancellationOwnerName", "<td valign = 'top' colspan = '3'><div>" + agreementPdf.DispalyName + "</div></td>");
                }
                else
                {
                    if (templateid.HasValue && templateid.Value > 0)
                    {
                        templateVars.Add("OwnerName", agreementPdf.OwnerName);
                    }
                    else
                    {
                        templateVars.Add("OwnerName", "<td valign = 'top' style = 'padding-left:10px;' colspan = '7'>Customer Name<div>" + agreementPdf.OwnerName + "</div></td>");
                    }
                    templateVars.Add("OwnerDisplayName", "<td valign = 'top' style = 'padding-left:10px;' colspan = '3'>Customer Name<div>" + agreementPdf.DispalyName + "</div></td>");
                    templateVars.Add("CancellationOwnerName", "<td valign = 'top' colspan = '3'><div>" + agreementPdf.DispalyName + "</div></td>");
                }


                if (!string.IsNullOrWhiteSpace(agreementPdf.SalesRepresentative) && agreementPdf.SalesRepresentative != "-1")
                {
                    templateVars.Add("SalesRepresentative", agreementPdf.SalesRepresentative);
                }
                else
                {
                    templateVars.Add("SalesRepresentative", "");
                }
                var EmergencyContactHeader = "";
                if (agreementPdf.EmergencyContactList != null && agreementPdf.EmergencyContactList.Count > 0)
                {
                    ContactNameHeader = "<div style='width: 24%; float:left;text-align:center;border:1px solid #ccc;font-weight: bold;'>Name</div>";
                    ContactPhoneHeader = " <div style='width: 24%; float:left; text-align:center;border:1px solid #ccc;font-weight: bold;'>Phone No</div>";
                    PhoneTypeHeader = "<div style='width: 24%; float:left; text-align:center;border:1px solid #ccc;font-weight: bold;'>Phone Type</div>";
                    ContactRelationshipHeader = "<div style='width: 24%; float:left; text-align:center;border:1px solid #ccc;font-weight: bold;'>Relationship</div>";
                    EmergencyContactHeader += ContactNameHeader + ContactPhoneHeader + PhoneTypeHeader + PhoneType + ContactRelationshipHeader;
                    EstimateEmergencyContactList = "<table style='width:100%;float:left;border-collapse:collapse;'>" +
                        "<thead>" +
                        "<tr style='background-color:#2ca01c;width:100%;border:1px solid #ccc; color:#fff;'>" +
                        "<th style='width:70%;text-align:left;padding:5px 0px 5px 20px;border-right:1px solid #ccc'>" +
                        "NAME</th>" +
                        "<th style='width:15%;padding:5px 0px;border-right:1px solid #ccc;text-align:center; text-transform:uppercase;'>" +
                        "Relationship</th>" +
                        //"<th style='width:14%;padding:5px 0px;border-right:1px solid #ccc;text-align:center; text-transform:uppercase;'>" +
                        //"Has Key</th>" +
                        "<th style='width:15%;padding:5px 0px;border-right:1px solid #ccc;text-align:center; text-transform:uppercase;'>Phone Number" +
                        "</th></tr></thead><tbody>";
                }
                EmergencyContactList += EmergencyContactHeader;
                foreach (var item in agreementPdf.EmergencyContactList)
                {
                    ContactName = "<div style='width: 24%; float:left; text-align:center;border:1px solid #ccc;'>" + item.FirstName + " " + item.LastName + "</div>";
                    ContactPhone = " <div style='width: 24%; float:left; text-align:center;border:1px solid #ccc;'>" + Extentions.PhoneNumberFormatNew(item.Phone) + "</div>";
                    if (item.PhoneType != "-1" && !string.IsNullOrEmpty(item.PhoneType))
                    {
                        PhoneType = "<div style='width: 24%; float:left; text-align:center;border:1px solid #ccc;'>" + item.PhoneType + "</div>";
                    }
                    else
                    {
                        PhoneType = "<div style='width: 24%; float:left; text-align:center;border:1px solid #ccc;'>-</div>";
                    }
                    if (item.RelationShip != "-1" && !string.IsNullOrEmpty(item.RelationShip))
                    {
                        ContactRelationship = "<div style='width: 24%; float:left; text-align:center;border:1px solid #ccc;'>" + item.RelationShip + "</div>";
                    }
                    else
                    {
                        ContactRelationship = "<div style='width: 24%; float:left; text-align:center;border:1px solid #ccc;'>-</div>";
                    }
                    EmergencyContactList += ContactName + ContactPhone + CenterSpace + PhoneType + ContactRelationship + HasKey;
                    EstimateEmergencyContactList += "<tr style='border:1px solid #ccc'>" +
                        "<td style='padding:5px 0px 5px 20px;border:1px solid #ccc'>" +
                        item.FirstName + " " + item.LastName +
                        "</td>" +
                        "<td style='padding:5px;text-align:center; border:1px solid #ccc'>";
                    if (item.RelationShip != "-1" && !string.IsNullOrEmpty(item.RelationShip))
                    {
                        EstimateEmergencyContactList += item.RelationShip;
                    }
                    else
                    {
                        EstimateEmergencyContactList += "-";
                    }
                    EstimateEmergencyContactList += "</td>" +
                    //"<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                    //HasKey +
                    //"</td>" +
                    "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                    Extentions.PhoneNumberFormatNew(item.Phone) +
                    "</td></tr>";
                }
                if (agreementPdf.EmergencyContactList != null && agreementPdf.EmergencyContactList.Count > 0)
                {
                    EstimateEmergencyContactList += "</tbody></table>";
                }
                if (!string.IsNullOrWhiteSpace(agreementPdf.ContractType) && agreementPdf.ContractType.ToLower() == "commercialfire")
                {
                    CommercialEquipmentList += "<table style='width:100%; float:left; border-collapse:collapse; margin-bottom:30px; margin-top:10px;'>" +
                        "<thead>" +
                        "<tr style='background-color:#4f90bb;color:white;width:100%;border:1px solid #4f90bb'>" +
                        "<th style='width:67%;text-align:left;padding:5px 0px 5px 20px;border-right:1px solid #fff'>EQUIPMENT</th>" +
                        "<th style='width:5%;padding:5px 0px;border-right:1px solid #fff;text-align:center'>QTY</th>" +
                        "<th style='width:14%;padding:5px 0px;border-right:1px solid #fff;text-align:center'>UNIT PRICE</th>" +
                        "<th style='width:14%;padding:5px 0px;border-right:1px solid #fff;text-align:center'>TOTAL PRICE</th>" +
                        "</tr></thead><tbody>";
                }
                GlobalSetting glHidingUnitPrice = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", agreementPdf.CompanyId, "HideUnitPriceOnAgreement")).FirstOrDefault();
                foreach (var item in agreementPdf.EquipmentList)
                {
                    EquipmentName = "  <div style = 'width:39%;float:left;padding-left:10px;'><span> &nbsp;" + item.Name + " </span></div> ";
                    EquipmentQuantity = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + item.Quantity + " </span></div>";


                    DiscountUnitPrice = " <div style = 'width:15%;float:left;text-align:center'><span>" + currentCurrency + FormatAmount(item.DiscountUnitPricce) + " </span></div>";
                    TotalEquipment = "  <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.Total) + " </span></div>";
                    EquipmentList += " <div style='padding-top:5px;'>" + EquipmentName + EquipmentQuantity + UnitPrice + DiscountUnitPrice + TotalEquipment + "</div>";

                    if (item.IsEqpExist)
                    {
                        ProductDFW = "<td style='text-align:center; font-weight:bold; border: 1px solid #000; padding:0px 10px;'>" + item.Name + " (Existing Equipment)</td>";
                    }
                    else
                    {
                        ProductDFW = "<td style='text-align:center; font-weight:bold; border: 1px solid #000; padding:0px 10px;'>" + item.Name + "</td>";
                    }

                    QuantityDFW = "<td style='text-align:center; border:1px solid #000;'>" + item.Quantity + "</td>";

                    DiscountUnitPriceDFW = "<td style='text-align:center; border:1px solid #000;'>" + currentCurrency + FormatAmount(item.UnitPrice - item.DiscountUnitPricce) + "</td>";

                    TotalPriceDFW = "<td style='text-align:center; border:1px solid #000;'>" + currentCurrency + FormatAmount(item.Total) + "</td>";

                    UnitPriceDFW = "<td style='text-align:center; font-weight:bold; border:1px solid #000;'>" + currentCurrency + FormatAmount(item.UnitPrice) + "</td>";
                    TotalDiscountDFW = "<td style='text-align:center; border:1px solid #000;'>" + currentCurrency + FormatAmount(item.TotalDiscountUnitPrice) + "</td>";
                    if (glHidingUnitPrice != null && glHidingUnitPrice.Value == "true")
                    {
                        EquipmentListDFW += "<tr style='background - color:#f2f2f2; color:#000; height: 30px; border:2px solid #000;'>" + ProductDFW + QuantityDFW + DiscountUnitPriceDFW + TotalPriceDFW + "</tr>";
                    }
                    else
                    {

                        EquipmentListDFW += "<tr style='background - color:#f2f2f2; color:#000; height: 30px; border:2px solid #000;'>" + ProductDFW + QuantityDFW + UnitPriceDFW + DiscountUnitPriceDFW + TotalDiscountDFW + TotalPriceDFW + "</tr>";
                    }
                    if (!string.IsNullOrWhiteSpace(agreementPdf.ContractType) && agreementPdf.ContractType.ToLower() == "commercialfire")
                    {
                        CommercialEquipmentList += "<tr style='border-bottom:1px solid #ccc'>" +
                            "<td style='padding:5px 0px 5px 20px'>" +
                            "<strong>" +
                            "<label>" +
                            item.Name +
                            "</label>" +
                            "</strong></td>" +
                            "<td style='padding:5px;text-align:center'>" +
                            "<strong>" +
                            "<label>" +
                            item.Quantity +
                            "</label>" +
                            "</strong ></td>" +
                            "<td style='padding:5px;text-align:center'>" +
                            "<strong>" +
                            "<label>" +
                            currentCurrency + FormatAmount(item.UnitPrice) +
                            "</label>" +
                            "</strong></td>" +
                            "<td style='padding:5px;text-align:center'>" +
                            "<strong>" +
                            "<label>" +
                            currentCurrency + FormatAmount(item.Total) +
                            "</label></strong></td></tr> ";
                    }
                }
                if (!string.IsNullOrWhiteSpace(agreementPdf.ContractType) && agreementPdf.ContractType.ToLower() == "commercialfire")
                {

                    CommercialEquipmentList += "<tr style='font-weight:bold'>" +
                        "<td colspan='3' style='padding-top:5px;text-align:right;padding-right:10px'>Subtotal</td>" +
                        "<td style='padding-top:5px; text-align:center; padding-left:10px;'>" +
                        currentCurrency + FormatAmount(agreementPdf.UpfrontAddOnTotal) +
                        "</td></tr>"
                        + "<tr style = 'font-weight:bold' > " +
                        "<td colspan='3' style='padding-top:5px;text-align:right;padding-right:10px'>Discount</td>" +
                        "<td style='padding-top:5px; text-align:center; padding-left:10px;'>" +
                        currentCurrency + FormatAmount(EqpActualDiscount) +
                        "</td></tr><tr style='font-weight:bold'>" +
                        "<td colspan='3' style='padding-top:5px;text-align:right;padding-right:10px'>Subtotal</td>" +
                        "<td style='padding-top:5px; text-align:center; padding-left:10px;'>" +
                        currentCurrency + FormatAmount(agreementPdf.UpfrontAddOnTotal - EqpActualDiscount) +
                        "</td></tr>"
                        + "<tr style='font-weight:bold'>" +
                        "<td colspan='3' style='padding-top:5px;text-align:right;padding-right:10px'>Tax</td>" +
                        "<td style='padding-top:5px; text-align:center; padding-left:10px;'>" +
                        currentCurrency + FormatAmount(((agreementPdf.UpfrontAddOnTotal - EqpActualDiscount) / 100) * agreementPdf.Tax) +
                        "</td></tr>"
                        + "<tr style='font-weight:bold'>" +
                        "<td colspan='3' style='padding-top:5px;text-align:right;padding-right:10px'>Total</td>" +
                        "<td style='padding-top:5px; text-align:center; padding-left:10px;'>" +
                        currentCurrency + FormatAmount((agreementPdf.UpfrontAddOnTotal - EqpActualDiscount) + (((agreementPdf.UpfrontAddOnTotal - EqpActualDiscount) / 100) * agreementPdf.Tax)) +
                        "</td></tr></tbody></table>";
                }
                if (agreementPdf.EquipmentList.Count == 0)
                {
                    CommercialEquipmentList = "";
                }
                #region Onit Smart Set up
                EquipmentListTable = "<table style='width:100%; float:left; border-collapse:collapse; border-bottom:1px solid #ccc;'>" +
                    "<tr style='text-transform:uppercase; font-weight:bold; border-bottom:1px solid #ccc;'>" +
                    "<td style='width:75%; padding:5px;'>" +
                    "Equipment" +
                    "</td>" +
                    "<td style='width:5%; padding:5px; text-align:center;'>" +
                    "Qty" +
                    "</td>" +
                    "<td style='width:10%; padding:5px; text-align:right;'>" +
                    "Price" +
                    "</td>" +
                    "<td style='width:10%; padding:5px; text-align:right;'>" +
                    "Total" +
                    "</td></tr> ";
                #endregion
                foreach (var item in agreementPdf.EquipmentList)
                {
                    EquipmentName = "  <div style = 'width:30%;float:left;padding-left:10px;'><span> &nbsp;" + item.Name + " </span></div> ";
                    EquipmentQuantity = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + item.Quantity + " </span></div>";
                    UnitPrice = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.UnitPrice) + " </span></div>";
                    DiscountUnitPrice = " <div style = 'width:20%;float:left;text-align:center'><span>" + currentCurrency + FormatAmount(item.DiscountUnitPricce) + " </span></div>";
                    TotalEquipment = "  <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.Total) + " </span></div>";
                    EquipmentListRab += " <div style='padding-top:5px;'>" + EquipmentName + EquipmentQuantity + UnitPrice + DiscountUnitPrice + TotalEquipment + "</div>";
                    EquipmentListTable += "<tr>" +
                        "<td style='padding:5px;'>" +
                        item.Name +
                        "</td>" +
                        "<td style='padding:5px; text-align:center;'>" +
                        item.Quantity +
                        "</td>" +
                        "<td style='padding:5px; text-align:right'>" +
                        currentCurrency + FormatAmount(item.UnitPrice) +
                        "</td>" +
                        "<td style='padding:5px; text-align:right;'>" +
                        currentCurrency + FormatAmount(item.Total) +
                        "</td></tr>";
                }
                EquipmentListTable += "</table>";
                templateVars.Add("EquipmentListOnit", EquipmentListTable);
                if (isServiceDetail == true)
                {
                    ServiceListTable = " <tr style='background-color:#000; color:#fff; height: 30px; border:2px solid #000;'><td style='width:40%; text-align:center; font-weight:bold;'>MONTHLY SERVICE</td><td style='text-align:center; font-weight:bold;'>Type</td><td style='text-align:center; font-weight:bold;'> Monthly</td><td style='text-align:center; font-weight:bold;'> Qty</td><td style='text-align:center; font-weight:bold;'>Total $</td></tr>";
                }
                else
                {
                    ServiceListTable = " <tr style='background-color:#000; color:#fff; height: 30px; border:2px solid #000;'><td colspan='4' style='text-align:center; font-weight:bold;'>MONTHLY SERVICE</td></tr>";
                }
                if (!string.IsNullOrWhiteSpace(agreementPdf.ContractType) && agreementPdf.ContractType.ToLower() == "commercialfire")
                {
                    CommercialServiceList += "<table style='width:100%; float:left; border-collapse:collapse; margin-bottom:30px; margin-top:10px;'>"
                        + "<thead>"
                        + "<tr style='background-color:#4f90bb;color:white;width:100%;border:1px solid #4f90bb'>"
                        + "<th style='width:86%; text-align:left; padding:5px 0px 5px 20px; border-right:1px solid #fff'>SERVICE</th>"
                        + "</tr></thead><tbody>";
                }
                #region Onit Service
                ServiceListOnit = "<table style='width:100%; float:left; border-collapse:collapse; border-bottom:1px solid #ccc;'>" +
                    "<tr style='text-transform:uppercase; font-weight:bold; border-bottom:1px solid #ccc;'>" +
                    "<td style='width:80%; padding:5px;'>" +
                    "Service" +
                    "</td>" +
                    "<td style='width:10%; padding:5px; text-align:right;'>" +
                    "</td>" +
                    "<td style='width:10%; padding:5px; text-align:right;'>" +
                    "</td></tr> ";
                templateVars.Add("SubtotalOnit", currentCurrency + FormatAmount(MonthlyServiceFee + upfrontamount));
                templateVars.Add("TaxOnit", currentCurrency + FormatAmount((MonthlyServiceFee + upfrontamount) * (agreementPdf.Tax / 100)));
                templateVars.Add("TotalOnit", currentCurrency + FormatAmount(MonthlyServiceFee + upfrontamount + ((MonthlyServiceFee + upfrontamount) * (agreementPdf.Tax / 100))));
                #endregion
                foreach (var item in ArbEnabledServiceList)
                {
                    ServiceName = "  <div style = 'width:54%;float:left;padding-left:10px;'><span> &nbsp;" + item.Name + " </span></div> ";
                    MonthlyRate = "  <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.MonthlyRate) + " </span></div> ";
                    DiscountRate = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.DiscountRate) + " </span></div>";
                    Total = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.Total) + " </span></div>";
                    ServiceList += " <div style='padding-top:5px;'>" + ServiceName + MonthlyRate + DiscountRate + Total + "</div>";
                    ServiceListOnit += "<tr>" +
                        "<td style='padding:5px;'>" +
                        item.Name +
                        "</td>" +
                        "<td style='padding:5px; text-align:right;'>" +
                        currentCurrency + FormatAmount(item.MonthlyRate) +
                        "</td>" +
                        "<td style='padding:5px; text-align:right;'>" +
                        currentCurrency + FormatAmount(item.Total) +
                        "</td></tr> ";
                    if (isServiceDetail == true)
                    {
                        MonthlyService = "<td style='text-align:right;font-weight:bold; border:1px solid #000; padding:0px 10px;'>" + item.Name + "</td>";
                    }
                    else
                    {
                        MonthlyService = "<td colspan='4' style='text-align:center;font-weight:bold; border:1px solid #000; padding:0px 10px;'>" + item.Name + "</td>";
                    }
                    Type = "<td style='text-align:center; border:1px solid #000;'>" + item.Category + "</td>";
                    Monthly = "<td style='text-align:center; font-weight:bold; border:1px solid #000;'>" + currentCurrency + FormatAmount(item.Total) + "</td>";
                    Qty = "<td style='text-align:center; border:1px solid #000;'>" + item.Quantity + "</td>";
                    TotalDfw = "<td style='text-align:center; border:1px solid #000;'>" + FormatAmount(item.Total) + "</td>";
                    if (isServiceDetail == true)
                    {
                        ServiceListDFW += "<tr style='background-color:#bfbfbf; color:#000; height: 30px; border:2px solid #000;'>" + MonthlyService + Type + Monthly + Qty + TotalDfw + "</tr>";
                    }
                    else
                    {
                        ServiceListDFW += "<tr style='background-color:#bfbfbf; color:#000; height: 30px; border:2px solid #000;><td style='width:100%; text-align:center; font-weight:bold;'>" + MonthlyService + "</td></tr>";
                    }
                    if (!string.IsNullOrWhiteSpace(agreementPdf.ContractType) && agreementPdf.ContractType.ToLower() == "commercialfire")
                    {
                        CommercialServiceList += "<tr style='border-bottom:1px solid #ccc'>"
                            + "<td style='padding:5px 0px 5px 20px'>" +
                            "<strong>" +
                            "<label>" +
                             item.Name +
                            "</label>" +
                            "</strong></td>"
                            + "</tr>";
                    }
                }
                ServiceListOnit += "</table>";
                templateVars.Add("ServiceListOnit", ServiceListOnit);
                if (!string.IsNullOrWhiteSpace(agreementPdf.ContractType) && agreementPdf.ContractType.ToLower() == "commercialfire")
                {
                    CommercialServiceList += "<tr style='font-weight:bold'>"
                        + "<td style='padding-top:5px; text-align:right; padding-right:10px'>Subtotal</td >"
                        + "<td style='padding-top:5px; text-align:center; padding-left:10px;'>" +
                        currentCurrency + FormatAmount(agreementPdf.MonthlyServiceFeeTotal) +
                        "</td></tr>"
                        + "<tr style='font-weight:bold'>"
                        + "<td style='padding-top:5px; text-align:right; padding-right:10px'>Tax</td>"
                        + "<td style='padding-top:5px; text-align:center; padding-left:10px;'>" +
                        currentCurrency + FormatAmount((agreementPdf.MonthlyServiceFeeTotal / 100) * agreementPdf.Tax) +
                        "</td></tr>"
                        + "<tr style='font-weight:bold'>"
                        + "<td style='padding-top:5px; text-align:right; padding-right:10px'>Total</td >"
                        + "<td style='padding-top:5px; text-align:center; padding-left:10px;'>" +
                        currentCurrency + FormatAmount(agreementPdf.MonthlyServiceFeeTotal + ((agreementPdf.MonthlyServiceFeeTotal / 100) * agreementPdf.Tax)) +
                        "</td></tr></tbody></table>";
                }
                if (ArbEnabledServiceList.Count == 0)
                {
                    CommercialServiceList = "";
                }
                foreach (var item in ArbEnabledServiceList)
                {
                    ServiceName = "  <div style = 'width:35%;float:left;padding-left:10px;'><span> &nbsp;" + item.Name + " </span></div> ";
                    MonthlyRate = "  <div style = 'width:20%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.MonthlyRate) + " </span></div> ";
                    DiscountRate = " <div style = 'width:20%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.DiscountRate) + " </span></div>";
                    Total = " <div style = 'float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.Total) + " </span></div>";
                    ServiceListRab += " <div style='padding-top:5px;'>" + ServiceName + MonthlyRate + DiscountRate + Total + "</div>";
                }
                if (agreementPdf.ListAgreementAnswer != null && agreementPdf.ListAgreementAnswer.Count > 0)
                {
                    string ansval = "";
                    foreach (var item in agreementPdf.ListAgreementAnswer)
                    {
                        if (item.QuestionId == 1)
                        {
                            if (item.Answer == "true")
                            {
                                ansval = "Yes";
                            }
                            else
                            {
                                ansval = "No";
                            }
                            divAnsval = "<div style='word-wrap: break-word;'>Are you the homeowner? (" + ansval + ")</div>";
                        }
                        if (item.QuestionId == 2)
                        {
                            if (item.Answer == "true")
                            {
                                ansval = "Yes";
                            }
                            else
                            {
                                ansval = "No";
                            }
                            divAnsval = "<div style='word-wrap: break-word;'>Is your home new construction? (" + ansval + ")</div>";
                        }
                        if (item.QuestionId == 3)
                        {
                            if (item.Answer == "true")
                            {
                                ansval = "Yes";
                            }
                            else
                            {
                                ansval = "No";
                            }

                            divAnsval = " <div style='word-wrap: break-word;'> Are you under any contractual agreement/ obligation with any other company for monitoring services? (" + ansval + ")</div> ";
                        }
                        if (item.QuestionId == 4)
                        {
                            if (item.Answer == "true")
                            {
                                ansval = "Yes";
                            }
                            else
                            {
                                ansval = "No";
                            }
                            divAnsval = " <div style='word-wrap: break-word;'>  I understand that the Company or any representaive of the Company cannot be responsible for cancelling any services with my current security company(if applicable). (" + ansval + ")</div> ";
                        }
                        if (item.QuestionId == 5)
                        {
                            if (item.Answer == "true")
                            {
                                ansval = "Yes";
                            }
                            else
                            {
                                ansval = "No";
                            }
                            String month = "";
                            if (!string.IsNullOrWhiteSpace(agreementPdf.SubscribedMonths) && agreementPdf.SubscribedMonths.ToLower() != "month to month")
                            {
                                if (agreementPdf.SubscribedMonths == "1")
                                {
                                    month = "<span>month</span>";
                                }
                                else
                                {
                                    month = "<span>months</span>";
                                }
                            }
                            divAnsval = "<div style = 'word-wrap: break-word;'> I understand that I have signed an agreement to receive monitoring services for<Span style = 'text-decoration:underline;font-weight :600;'> &nbsp; &nbsp; &nbsp; &nbsp;" + agreementPdf.SubscribedMonths + " &nbsp; &nbsp; &nbsp; &nbsp;</span><span>" + month + "</span></div>";
                        }
                        ListAgreementAnswer += divAnsval;
                    }
                }
                else
                {
                    ListAgreementAnswer = "<div style='word-wrap: break-word;'>1. Are you the homeowner?</div><div style = 'word-wrap: break-word;' > 2.Is your home new construction?</div><div style = 'word-wrap: break-word;' >3.Are you under any contractual agreement / obligation with any other company for monitoring services?</div> <div style = 'word-wrap: break-word;' > 4.I understand that the Company or any representaive of the Company cannot be responsible for cancelling any services with my current security company(if applicable).</div><div style = 'word-wrap: break-word;' >5.I understand that I have signed an agreement to receive monitoring services for<span style = 'text-decoration:underline;font-weight :600;' > &nbsp; &nbsp; &nbsp; &nbsp;" + agreementPdf.SubscribedMonths + "&nbsp;&nbsp;&nbsp;&nbsp;</span> <span> month </span></div>";
                }
                if (agreementPdf.CustomerAgreement != null)
                {
                    foreach (var item in agreementPdf.CustomerAgreement)
                    {

                        if (item.Type == "LoadAgreement")
                        {
                            CustomerAgreement = "<span> Load: </span><span style = 'font-weight:600;'>" + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("MM/dd/yyyy") + "+ at+" + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("hh:mm tt") + " </span><br/>";
                        }
                        if (item.Type == "SignAgreement")
                        {
                            CustomerAgreement = "<span> Sign: </span><span style = 'font-weight:600;'>" + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("MM/dd/yyyy") + "+ at+" + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("hh:mm tt") + " </span><br/>";

                        }
                        if (item.Type == "SubmitAgreement")
                        {
                            CustomerAgreement = "<span> Submit: </span><span style = 'font-weight:600;'>" + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("MM/dd/yyyy") + "+ at+" + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("hh:mm tt") + " </span><br/>";
                        }
                    }
                    CustomerAgreementTable = "<table style='width:100%'><tr style='background-color:darkgray;'><th style='width:33%; text-align:center;'>IP</th><th style='width:33%; text-align:center;'>USER AGENT</th><th style='width:33%; text-align:center;'>TIMESTAMP</th> </tr><tr><td style='width:33%; text-align:center;'>" + agreementPdf.SingleCustomerAgreement.IP + "</td><td style='width:33%; text-align:center;'>" + agreementPdf.SingleCustomerAgreement.UserAgent + "</td><td style='width:33%; text-align:center;'>" + CustomerAgreement + " </td></tr>";
                }
                #region BBB Conflict
                if (IsBBBConflict)
                {
                    foreach (var item in agreementPdf.CustomerAgreement)
                    {

                        if (item.Type == "AgreementCreate")
                        {
                            templateVars.Add("AgreementCreatedDate", DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("MM/dd/yy"));
                            templateVars.Add("AgreementCreatedDateTime", DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("MM/dd/yy") + " at " + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("hh:mm tt"));
                        }
                        if (item.Type == "AgreementSend")
                        {
                            templateVars.Add("AgreementSendDateTime", DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("MM/dd/yy") + " at " + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("hh:mm tt"));
                        }
                        if (item.Type == "AgreementSign")
                        {
                            templateVars.Add("AgreementSignDate", DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("MM/dd/yy"));
                            templateVars.Add("AgreementSignDateTime", DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("MM/dd/yy") + " at " + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("hh:mm tt"));
                            templateVars.Add("AgreementSignIp", item.IP);
                        }
                        if (item.Type == "AgreementComplete")
                        {
                            templateVars.Add("AgreementCompletedDateTime", DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("MM/dd/yy") + " at " + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("hh:mm tt"));
                        }
                    }
                }
                #endregion
                templateVars.Add("EmergencyContactList", EmergencyContactList);
                templateVars.Add("EquipmentList", EquipmentList);
                templateVars.Add("ServiceList", ServiceList);
                templateVars.Add("EquipmentListRab", EquipmentListRab);
                templateVars.Add("EquipmentListDFW", EquipmentListDFW);
                templateVars.Add("ServiceListRab", ServiceListRab);
                templateVars.Add("ServiceListDFW", ServiceListDFW);
                templateVars.Add("ServiceListTable", ServiceListTable);
                templateVars.Add("ListAgreementAnswer", ListAgreementAnswer);
                templateVars.Add("CustomerAgreementTable", CustomerAgreementTable);
                templateVars.Add("Password", agreementPdf.Password);
                templateVars.Add("ListContactEmergency", agreementPdf.ListContactEmergency);
                templateVars.Add("ListPaymentInfo", agreementPdf.ListPaymentInfo);
                templateVars.Add("CommercialEquipmentList", CommercialEquipmentList);

                if (!(agreementPdf.ContractType.ToLower() == "commercialfire"))
                {
                    templateVars.Add("CommercialServiceList", CommercialServiceList);
                }
                #region Invoice List
                string invDelListGutter = "<table style='width:30%; float:left; border-collapse:collapse; margin-top:10px;'>" +
                    "<tr>" +
                    "<td style='width:70%;'>" +
                    "</td>" +
                    "<td style='width:30%;'>" +
                    "</td></tr>" +
                    "<tr>" +
                    "<td colspan='2' style='font-size:18px; text-align:center;'>" +
                    "DETAILS" +
                    "</td></tr>" +
                    "<tr>" +
                    "<td style='border:1px solid #ccc; padding:2px 5px;'>" +
                    "Product Name" +
                    "</td>" +
                    "<td style='border:1px solid #ccc; padding:2px 5px; text-align:center;'>" +
                    "Rate" +
                    "</td></tr>";
                if (!string.IsNullOrWhiteSpace(agreementPdf.InvoiceId) && agreementPdf.IsInvoice == true)
                {

                    templateVars.Add("EstimateId", agreementPdf.InvoiceId);
                    templateVars.Add("MonitoringAmount", currentCurrency + FormatAmount(agreementPdf.inv.MonitoringAmount.HasValue ? agreementPdf.inv.MonitoringAmount : 0.0));
                    templateVars.Add("MonitoringDescription", agreementPdf.inv.MonitoringDescription != "-1" ? agreementPdf.inv.MonitoringDescription : "");
                    if (!string.IsNullOrWhiteSpace(agreementPdf.inv.ContractTerm) && agreementPdf.inv.ContractTerm == "0")
                    {
                        templateVars.Add("ContractTerm", "Month to Month");
                    }
                    else if (!string.IsNullOrWhiteSpace(agreementPdf.inv.ContractTerm) && agreementPdf.inv.ContractTerm != "-1")
                    {
                        templateVars.Add("ContractTerm", Convert.ToInt32(agreementPdf.inv.ContractTerm) * 12);
                    }
                    else
                    {
                        templateVars.Add("ContractTerm", 0);
                    }
                    if (agreementPdf.InvoiceList != null)
                    {
                        foreach (var item in agreementPdf.InvoiceList)
                        {
                            InvoiceQTY = "<td style='text-align:right; border:1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + item.Quantity + "</td>";
                            InvoiceName = "<td style='text-align:center; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + item.EquipName + "</td>";
                            InvoicePrice = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(item.UnitPrice) + "</td>";
                            InvoiceSubTotal = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(item.TotalPrice) + "</td>";

                            InvoiceList += "<tr style=''>" + InvoiceQTY + InvoiceName + InvoicePrice + InvoiceSubTotal + "</tr>";
                            InvoiceTotalSubTotal += item.TotalPrice.Value;
                            invDelListGutter += "<tr>" +
                                "<td style='border:1px solid #ccc; padding:2px 5px;'>" +
                                item.EquipName +
                                "</td>" +
                                "<td style='border:1px solid #ccc; padding:2px 5px; text-align:center;'>" +
                                currentCurrency + FormatAmount(item.UnitPrice) +
                                "</td></tr>";
                        }
                    }
                    else
                    {
                        InvoiceQTY = "<td style='text-align:right; border:1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + 0 + "</td>";
                        InvoiceName = "<td style='text-align:center; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'> </td>";
                        InvoicePrice = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(0.0) + "</td>";
                        InvoiceSubTotal = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(0.0) + "</td>";

                        InvoiceList += "<tr style=''>" + InvoiceQTY + InvoiceName + InvoicePrice + InvoiceSubTotal + "</tr>";
                    }
                    InvoiceTotalSubTotalWithUpfront = InvoiceTotalSubTotal;

                    #region Estimate Upfront List
                    EstimateUpfrontChargeList = "<table style='width:100%;float:left;border-collapse:collapse;'>" +
                        "<thead><tr style='background-color:#2ca01c;width:100%;border:1px solid #ccc; color:#fff;'>" +
                        "<th style='width:75%;text-align:left;padding:5px 0px 5px 20px;border-right:1px solid #ccc'>" +
                        "PRODUCT" +
                        "</th>" +
                        "<th style='width:5%;padding:5px 0px;border-right:1px solid #ccc;text-align:center'>" +
                        "QTY</th>" +
                        "<th style='width:10%;padding:5px 0px;border-right:1px solid #ccc;text-align:center'>" +
                        "UNIT PRICE</th>" +
                        //"<th style='width:8%;padding:5px 0px;border-right:1px solid #ccc;text-align:center'>" +
                        //"DISCOUNT" +
                        //"</th>" +
                        "<th style='width:10%;padding:5px 0px;border-right:1px solid #ccc;text-align:center'>" +
                        "PRICE</th>" +
                        "</tr></thead><tbody>";
                    EstimateRecurringChargeList = "<table style='width:100%;float:left;border-collapse:collapse;'>" +
                        "<thead><tr style='background-color:#2ca01c;width:100%;border:1px solid #ccc; color:#fff;'>" +
                        "<th style='width:75%;text-align:left;padding:5px 0px 5px 20px;border-right:1px solid #ccc'>" +
                        "SERVICE</th>" +
                        "<th style='width:5%;padding:5px 0px;border-right:1px solid #ccc;text-align:center'> " +
                        "QTY " +
                        "</th><th style='width:10%;padding:5px 0px;border-right:1px solid #ccc;text-align:center'>" +
                        "UNIT PRICE" +
                        "</th><th style='width:10%;padding:5px 0px;border-right:1px solid #ccc;text-align:center'> " +
                        "PRICE " +
                        "</th></tr></thead><tbody>";
                    if (agreementPdf.InvoiceList != null)
                    {
                        foreach (var item in agreementPdf.InvoiceList)
                        {
                            if (item.EquipmentClassId == 1)
                            {
                                EstimateUpfrontChargeList += "<tr style='border:1px solid #ccc'>" +
                                "<td style='padding: 5px 0px 5px 20px; border: 1px solid #ccc'>" +
                                item.EquipName +
                                "</td>" +
                                "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                item.Quantity +
                                "</td>" +
                                "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                currentCurrency + FormatAmount(item.UnitPrice) +
                                "</td>" +
                                //"<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                //"116.99" +
                                //"</td>" +
                                "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                currentCurrency + FormatAmount(item.TotalPrice) +
                                "</td></tr>";
                                EstimateUpfrontTotalSubTotal += item.TotalPrice.Value;
                            }
                            else if (item.EquipmentClassId == 2)
                            {
                                EstimateRecurringChargeList += "<tr style='border:1px solid #ccc'>" +
                                    "<td style='padding:5px 0px 5px 20px;border:1px solid #ccc'>" +
                                    item.EquipName +
                                    "</td>" +
                                    "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                    item.Quantity +
                                    "</td>" +
                                    "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                    currentCurrency + FormatAmount(item.UnitPrice) +
                                    "</td>" +
                                    "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                    currentCurrency + FormatAmount(item.TotalPrice) +
                                    "</td></tr>";
                                EstimateRecurringTotalSubTotal += item.TotalPrice.Value;
                            }
                        }
                    }
                    else
                    {
                        EstimateRecurringChargeList += "<tr style='border:1px solid #ccc'><td style='padding:5px 0px 5px 20px;border:1px solid #ccc'>" +
                                    "" +
                                    "</td>" +
                                    "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                    1 +
                                    "</td>" +
                                    "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                    currentCurrency + FormatAmount(0.0) +
                                    "</td>" +
                                    "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                    currentCurrency + FormatAmount(0.0) +
                                    "</td></tr>";
                        EstimateRecurringTotalSubTotal += 0.0;
                    }
                    EstimateUpfrontChargeList += "<tr style='border:1px solid #ccc'>" +
                                "<td style='padding: 5px 0px 5px 20px; border: 1px solid #ccc'>" +
                                "Advance Monitoring Fee" +
                                "</td>" +
                                "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                1 +
                                "</td>" +
                                "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                currentCurrency + FormatAmount(AdvanceMonitoringFee) +
                                "</td>" +
                                //"<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                //"116.99" +
                                //"</td>" +
                                "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                currentCurrency + FormatAmount(AdvanceMonitoringFee) +
                                "</td></tr>";
                    EstimateUpfrontTotalSubTotal += AdvanceMonitoringFee;
                    EstimateUpfrontChargeList += "<tr style='border:1px solid #ccc'>" +
                                "<td style='padding: 5px 0px 5px 20px; border: 1px solid #ccc'>" +
                                "Non-Conforming Fee" +
                                "</td>" +
                                "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                1 +
                                "</td>" +
                                "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                currentCurrency + FormatAmount(agreementPdf.NonConfirmingFee) +
                                "</td>" +
                                //"<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                //"116.99" +
                                //"</td>" +
                                "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                currentCurrency + FormatAmount(agreementPdf.NonConfirmingFee) +
                                "</td></tr>";
                    EstimateUpfrontTotalSubTotal += agreementPdf.NonConfirmingFee;
                    EstimateUpfrontChargeList += "<tr style='border:1px solid #ccc'>" +
                                "<td style='padding: 5px 0px 5px 20px; border: 1px solid #ccc'>" +
                                "Activation Fee" +
                                "</td>" +
                                "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                1 +
                                "</td>" +
                                "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                currentCurrency + FormatAmount(agreementPdf.ActivationFee) +
                                "</td>" +
                                //"<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                //"116.99" +
                                //"</td>" +
                                "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                currentCurrency + FormatAmount(agreementPdf.ActivationFee) +
                                "</td></tr>";
                    EstimateUpfrontTotalSubTotal += agreementPdf.ActivationFee;
                    EstimateUpfrontChargeList += "<tr style=font-weight:bold'>" +
                        "<td colspan='2'>" +
                        "</td>" +
                        "<td style='padding:5px;text-align:right;padding-right:10px;background-color:#f8f8f8; border:1px solid #ccc;'>Subtotal</td>" +
                        "<td style='padding:5px; padding-left:10px; border:1px solid #ccc;text-align:center;'>" +
                        currentCurrency + FormatAmount(EstimateUpfrontTotalSubTotal) +
                        "</td></tr>" +
                        "<tr style='font-weight:bold'>" +
                        "<td colspan='2'></td>" +
                        "<td style='padding:5px;text-align:right;padding-right:10px;background-color:#f8f8f8; border:1px solid #ccc;'>" +
                        "Tax</td>" +
                        "<td style='padding:5px; padding-left:10px; border:1px solid #ccc;text-align:center;'>" +
                        currentCurrency + FormatAmount((EstimateUpfrontTotalSubTotal * agreementPdf.Tax) / 100) +
                        "</td></tr>" +
                        "<tr style='font-weight:bold'>" +
                        "<td colspan='2' ></td>" +
                        "<td style='padding:5px;text-align:right;padding-right:10px;background-color:#f8f8f8; border:1px solid #ccc;'>" +
                        "Total</td>" +
                        "<td style='padding:5px; padding-left:10px; border:1px solid #ccc;text-align:center;'>" +
                        currentCurrency + FormatAmount(EstimateUpfrontTotalSubTotal + ((EstimateUpfrontTotalSubTotal * agreementPdf.Tax) / 100)) +
                        "</td></tr></tbody></table>";
                    EstimateRecurringChargeList += "<tr style=font-weight:bold'>" +
                        "<td colspan='2'>" +
                        "</td>" +
                        "<td style='padding:5px;text-align:right;padding-right:10px;background-color:#f8f8f8; border:1px solid #ccc;'>Subtotal</td>" +
                        "<td style='padding:5px; padding-left:10px; border:1px solid #ccc;text-align:center;'>" +
                        currentCurrency + FormatAmount(EstimateRecurringTotalSubTotal) +
                        "</td></tr>" +
                        "<tr style='font-weight:bold'>" +
                        "<td colspan='2'></td>" +
                        "<td style='padding:5px;text-align:right;padding-right:10px;background-color:#f8f8f8; border:1px solid #ccc;'>" +
                        "Tax</td>" +
                        "<td style='padding:5px; padding-left:10px; border:1px solid #ccc;text-align:center;'>" +
                        currentCurrency + FormatAmount((EstimateRecurringTotalSubTotal * agreementPdf.Tax) / 100) +
                        "</td></tr>" +
                        "<tr style='font-weight:bold'>" +
                        "<td colspan='2' ></td>" +
                        "<td style='padding:5px;text-align:right;padding-right:10px;background-color:#f8f8f8; border:1px solid #ccc;'>" +
                        "Total</td>" +
                        "<td style='padding:5px; padding-left:10px; border:1px solid #ccc;text-align:center;'>" +
                        currentCurrency + FormatAmount(EstimateRecurringTotalSubTotal + ((EstimateRecurringTotalSubTotal * agreementPdf.Tax) / 100)) +
                        "</td></tr></tbody></table>";
                    templateVars.Add("EstimateUpfrontChargeList", EstimateUpfrontChargeList);
                    templateVars.Add("EstimateRecurringChargeList", EstimateRecurringChargeList);
                    templateVars.Add("EstimateEmergencyContactList", EstimateEmergencyContactList);
                    #endregion

                    if (!string.IsNullOrWhiteSpace(agreementPdf.inv.UpfrontMonth) && agreementPdf.inv.UpfrontMonth != "-1")
                    {
                        InvoiceQTY = "<td style='text-align:right; border:1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + agreementPdf.inv.UpfrontMonth + "</td>";
                        InvoiceName = "<td style='text-align:center; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + "Upfront month charge" + "</td>";
                        InvoicePrice = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(agreementPdf.inv.MonitoringAmount.HasValue ? agreementPdf.inv.MonitoringAmount : 0.0) + "</td>";
                        InvoiceSubTotal = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount((agreementPdf.inv.MonitoringAmount.HasValue ? agreementPdf.inv.MonitoringAmount : 0.0) * (Convert.ToInt32(agreementPdf.inv.UpfrontMonth))) + "</td>";

                        InvoiceList += "<tr style=''>" + InvoiceQTY + InvoiceName + InvoicePrice + InvoiceSubTotal + "</tr>";

                        InvoiceTotalSubTotalWithUpfront += Convert.ToDouble((agreementPdf.inv.MonitoringAmount.HasValue ? agreementPdf.inv.MonitoringAmount : 0.0) * (Convert.ToInt32(agreementPdf.inv.UpfrontMonth)));
                    }

                    InvoiceQTY = "<td style='text-align:right; border:1px solid #ebebeb; padding:10px; height:30px' valign='middle'>1</td>";
                    InvoiceName = "<td style='text-align:center; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + "Prorated amount" + "</td>";
                    InvoicePrice = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(agreementPdf.ProratedAmout) + "</td>";
                    InvoiceSubTotal = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(agreementPdf.ProratedAmout) + "</td>";

                    InvoiceList += "<tr style=''>" + InvoiceQTY + InvoiceName + InvoicePrice + InvoiceSubTotal + "</tr>";
                    InvoiceTotalSubTotalWithUpfront += agreementPdf.ProratedAmout;

                    InvoiceFinalTotal = agreementPdf.inv.TotalAmount.Value;
                    templateVars.Add("EstimateSubtotal", currentCurrency + FormatAmount(InvoiceTotalSubTotal));
                    if (agreementPdf.inv.EstimateTerm == "50UponAcceptance50UponCompletion")
                    {
                        EstimateSigningAmount = (InvoiceTotalSubTotalWithUpfront + (Math.Round(agreementPdf.inv.Tax.Value, 2))) / 2;
                        templateVars.Add("SigningAmount", currentCurrency + FormatAmount(EstimateSigningAmount));
                    }
                    else if (agreementPdf.inv.EstimateTerm == "DueonAcceptance")
                    {
                        EstimateSigningAmount = InvoiceTotalSubTotalWithUpfront + (Math.Round(agreementPdf.inv.Tax.Value, 2));
                        templateVars.Add("SigningAmount", currentCurrency + FormatAmount(EstimateSigningAmount));
                    }
                    else if (agreementPdf.inv.EstimateTerm == "DueUponCompletion")
                    {
                        templateVars.Add("SigningAmount", currentCurrency + FormatAmount(EstimateSigningAmount));
                    }

                    else
                    {
                        EstimateSigningAmount = InvoiceTotalSubTotalWithUpfront + (Math.Round(agreementPdf.inv.Tax.Value, 2));
                        templateVars.Add("SigningAmount", currentCurrency + FormatAmount(EstimateSigningAmount));
                    }
                    EstimateSigningAmountWithProrate = InvoiceTotalSubTotalWithUpfront + (Math.Round(agreementPdf.inv.Tax.Value, 2));
                    templateVars.Add("SigningAmountWithProrated", currentCurrency + FormatAmount(EstimateSigningAmountWithProrate));
                    InvoiceList += "<tr style=''><td colspan='2' style ='text-align:right; padding:10px;'></td ><td style ='text-align:right; padding:10px;'>Subtotal</td><td style ='text-align:right; padding:10px;font-weight:bold;'>" +
                                    currentCurrency + FormatAmount(InvoiceTotalSubTotalWithUpfront) +
                                "</td ></tr>";
                    InvoiceList += "<tr style=''><td colspan='2' style ='text-align:right; padding:10px;'></td ><td style ='text-align:right; padding:10px;'>" + agreementPdf.inv.TaxType + "</td><td style ='text-align:right; padding:10px;font-weight:bold;'>" +
                                    currentCurrency + FormatAmount(Math.Round(agreementPdf.inv.Tax.Value, 2)) +
                                "</td ></tr>";
                    InvoiceList += "<tr style='font-weight:bold;'><td colspan ='2' style='text-align:right; padding:10px;'></td><td style ='text-align:right; padding:10px;'>Estimate Total</td> <td style ='text-align:right; padding:10px;'>" +
                                    currentCurrency + FormatAmount(InvoiceTotalSubTotalWithUpfront + (Math.Round(agreementPdf.inv.Tax.Value, 2))) +
                                "</td></tr>";
                }
                else
                {
                    templateVars.Add("MonitoringAmount", currentCurrency + FormatAmount(Convert.ToDouble(agreementPdf.MonthlyMonitoringFee)));
                    templateVars.Add("ContractTerm", agreementPdf.SubscribedMonths);
                    if (agreementPdf.EquipmentList != null)
                    {
                        foreach (var item in agreementPdf.EquipmentList)
                        {
                            InvoiceQTY = "<td style='text-align:right; border:1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + item.Quantity + "</td>";
                            InvoiceName = "<td style='text-align:center; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + item.Name + "</td>";
                            InvoicePrice = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(item.UnitPrice) + "</td>";
                            InvoiceSubTotal = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(item.Total) + "</td>";

                            InvoiceList += "<tr style=''>" + InvoiceQTY + InvoiceName + InvoicePrice + InvoiceSubTotal + "</tr>";
                            InvoiceTotalSubTotal += item.Total;
                        }
                    }
                    else
                    {
                        InvoiceQTY = "<td style='text-align:right; border:1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + 0 + "</td>";
                        InvoiceName = "<td style='text-align:center; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'> </td>";
                        InvoicePrice = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(0.0) + "</td>";
                        InvoiceSubTotal = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(0.0) + "</td>";

                        InvoiceList += "<tr style=''>" + InvoiceQTY + InvoiceName + InvoicePrice + InvoiceSubTotal + "</tr>";
                    }
                    InvoiceFinalTotal = InvoiceTotalSubTotal + ((InvoiceTotalSubTotal * agreementPdf.Tax) / 100);
                    InvoiceList += "<tr style=''><td colspan='2' style ='text-align:right; padding:10px;'></td ><td style ='text-align:right; padding:10px;'>Subtotal</td><td style ='text-align:right; padding:10px;font-weight:bold;'>" +
                                    currentCurrency + FormatAmount(InvoiceTotalSubTotal) +
                                "</td ></tr>";
                    InvoiceList += "<tr style=''><td colspan='2' style ='text-align:right; padding:10px;'></td ><td style ='text-align:right; padding:10px;'>Tax(" + agreementPdf.Tax + "%)</td><td style ='text-align:right; padding:10px;font-weight:bold;'>" +
                                    currentCurrency + FormatAmount(((InvoiceTotalSubTotal * agreementPdf.Tax) / 100)) +
                                "</td ></tr>";
                    InvoiceList += "<tr style='font-weight:bold;'><td colspan ='2' style='text-align:right; padding:10px;'></td><td style ='text-align:right; padding:10px;'>Total</td> <td style ='text-align:right; padding:10px;'>" +
                                    currentCurrency + FormatAmount(InvoiceFinalTotal) +
                                "</td></tr>";
                    templateVars.Add("EstimateSubtotal", currentCurrency + FormatAmount(InvoiceTotalSubTotal));
                    EstimateSigningAmount = (InvoiceFinalTotal + (!string.IsNullOrWhiteSpace(agreementPdf.MonthlyMonitoringFee) ? Convert.ToDouble(agreementPdf.MonthlyMonitoringFee) : 0.0));
                    templateVars.Add("SigningAmount", currentCurrency + FormatAmount(EstimateSigningAmount));

                    EstimateSigningAmountWithProrate = (InvoiceFinalTotal + (!string.IsNullOrWhiteSpace(agreementPdf.MonthlyMonitoringFee) ? Convert.ToDouble(agreementPdf.MonthlyMonitoringFee) : 0.0) + agreementPdf.ProratedAmout);
                    templateVars.Add("SigningAmountWithProrated", currentCurrency + FormatAmount(EstimateSigningAmountWithProrate));

                }
                templateVars.Add("EstimateContractServiceList", InvoiceList);
                invDelListGutter += "</table>";
                templateVars.Add("GutterProductList", invDelListGutter);
                templateVars.Add("CtGutterLogo", string.Concat(AppConfig.SiteDomain, "/Content/img/ct_gutter_logo_pdf.png"));
                templateVars.Add("InvoiceDiagram", !string.IsNullOrWhiteSpace(agreementPdf.InvoiceDiagram) ? agreementPdf.InvoiceDiagram : "");
                #endregion

                #region Financed Amount
                if (agreementPdf.MonthlyFinanceRate > 0.0)
                {
                    FinancedAmount = "<table style='width:100%;float:left; border-collapse:collapse;'>" +
                        "<tbody>" +
                        "<tr>" +
                        "<td style='width:180px;border:2px solid #000;background-color:#f8f8f8;padding:3px;'>" +
                        "Monthly Finance Rate</td>" +
                        "<td style='border:2px solid #000; padding:3px;'>" +
                        currentCurrency + FormatAmount(agreementPdf.MonthlyFinanceRate) +
                        "</td></tr></tbody></table>";
                }
                if (agreementPdf.FinancedAmout > 0.0)
                {
                    FinancedAmount += "<table style='width:100%;float:left; border-collapse:collapse; margin-top:-2px;'>" +
                        "<tbody><tr><td style='width:180px;border:2px solid #000;background-color:#f8f8f8;padding:3px;'>" +
                        "Financed Amount</td>" +
                        "<td style='border:2px solid #000; padding:3px;'>" +
                        currentCurrency + FormatAmount(agreementPdf.FinancedAmout) +
                        "</td></tr></tbody></table>";
                }
                templateVars.Add("FinancedAmountInfo", FinancedAmount);
                #endregion

                #region Estimator Detail
                if (agreementPdf.createEst != null && agreementPdf.createEst.Estimator != null && agreementPdf.IsEstimator == true)
                {
                    templateVars.Add("EstimatorId", agreementPdf.createEst.Estimator.EstimatorId);
                    templateVars.Add("eSecurityLogo", agreementPdf.createEst.eSecurityLogo);
                    templateVars.Add("SpecializedLogo", agreementPdf.createEst.specializedLogo);
                    if (agreementPdf.userInfo != null)
                    {
                        templateVars.Add("UserName", agreementPdf.userInfo.FirstName + " " + agreementPdf.userInfo.LastName);
                        templateVars.Add("UserPhone", agreementPdf.userInfo.Phone);
                        templateVars.Add("UserEmail", agreementPdf.userInfo.Email);
                    }
                    if (agreementPdf.createEst.estimatorDetails != null)
                    {
                        EstimatorDetailList = "<table style='width:100%; float:left; border-collapse:collapse; margin-bottom:30px;'>"
                                           + "<thead>"
                                           + "<tr style='background-color:#4f90bb; color:white;width:100%; border:1px solid #4f90bb;'>"
                                           + "<th style='width:46%; text-align:left; padding:5px 0px 5px 20px; border-right:1px solid #fff;'>"
                                           + "PRODUCT</th>"
                                           + "<th style='width:10%; padding:5px 0px; border-right:1px solid #fff; text-align:center;'>SKU</th>"
                                           + "<th style='width:5%; padding:5px 0px; border-right:1px solid #fff; text-align:center;'>QTY</th>";
                        if (agreementPdf.createEst._EstimatorPDFFilter.IncludeCost.Value)
                        {
                            EstimatorDetailList += "<th style='width:8%; padding:5px 0px; border-right:1px solid #fff; text-align:center;'>U COST</th>"
                                           + "<th style='width:8%; padding:5px 0px; border-right:1px solid #fff; text-align:center;'>T COST</th>";
                        }
                        if (agreementPdf.createEst._EstimatorPDFFilter.IncludeProfit.Value)
                        {
                            EstimatorDetailList += "<th style='width:8%; padding:5px 0px; border-right:1px solid #fff; text-align:center;'>PROFIT</th>";
                        }
                        if (agreementPdf.createEst._EstimatorPDFFilter.IncludeOverhead.Value)
                        {
                            EstimatorDetailList += "<th style='width:8%; padding:5px 0px; border-right:1px solid #fff; text-align:center;'>OVERHEAD</th>";
                        }
                        if (!agreementPdf.createEst._EstimatorPDFFilter.WithoutPricing.Value)
                        {
                            if (!agreementPdf.createEst._EstimatorPDFFilter.WithoutIndividualMaterialPricing.Value || !agreementPdf.createEst._EstimatorPDFFilter.WithoutIndividualLaborPricing.Value)
                            {
                                EstimatorDetailList += "<th style='width:8%; padding:5px 0px; border-right:1px solid #fff; text-align:center;'>U PRICE</th>"
                                    + "<th style='width:10%;padding:5px 0px;'>TOTAL PRICE</th>";
                            }
                        }

                        EstimatorDetailList += "</tr></thead>"
                        + "<tbody>";
                        if (agreementPdf.createEst._EstimatorPDFFilter.GroupedbyCategory.Value)
                        {
                            List<string> CategoryList = agreementPdf.createEst.estimatorDetails.GroupBy(x => x.CategoryVal).Select(x => x.Key).ToList();
                            foreach (var Category in CategoryList)
                            {
                                EstimatorDetailList += "<tr style='border-bottom:1px solid #ccc;'>"
                                     + "<td style ='padding:5px 0px 5px 40px;'>" +
                                     "<strong>" +
                                     "<label>";
                                if (Category == "Category")
                                {
                                    EstimatorDetailList += "Category";
                                }
                                else
                                {
                                    EstimatorDetailList += "Uncategorized";
                                }
                                EstimatorDetailList += "</label></strong></td></tr>";
                                EstimatorDetailList += GetEstimatorLineItems(agreementPdf.createEst.estimatorDetails.Where(x => x.CategoryVal == Category).ToList(), currentCurrency, agreementPdf.createEst._EstimatorPDFFilter);
                            }
                            //foreach (var item in agreementPdf.createEst.estimatorDetails)
                            //{
                            //    EstimatorProduct = "<td style='padding:5px 0px 5px 20px;'><strong><label>" + item.PartDescription + "</label></strong></td>";
                            //    EstimatorSKU = "<td style='padding:5px; text-align:center;'><strong><label>" + item.PartNumber + "</label></strong></td>";
                            //    EstimatorQTY = "<td style='padding:5px; text-align:center; '><strong><label>" + item.Qunatity + "</label></strong></td>";
                            //    EstimatorUCOST = "<td style='padding:5px; text-align:center; '><strong><label>" + currentCurrency + FormatAmount(item.UnitCost) + "</label></strong></td>";
                            //    EstimatorTCOST = "<td style='padding:5px; text-align:center; '><strong><label>" + currentCurrency + FormatAmount(item.TotalCost) + "</label></strong></td>";
                            //    EstimatorPROFIT = "<td style='padding:5px; text-align:center; '><strong><label>" + currentCurrency + FormatAmount(item.Profit) + "</label></strong></td>";
                            //    EstimatorOVERHEAD = "<td style='padding:5px; text-align:center; '><strong><label>" + currentCurrency + FormatAmount(item.Overhead) + "</label></strong></td>";

                            //    EstimatorDetailList += "<tr style='border-bottom:1px solid #ccc;'>" + EstimatorProduct + EstimatorSKU + EstimatorQTY + EstimatorUCOST + EstimatorTCOST + EstimatorPROFIT + EstimatorOVERHEAD + "</tr>";
                            //}
                        }
                        else if (agreementPdf.createEst._EstimatorPDFFilter.GroupedbySupplier.Value)
                        {
                            List<string> SupplierList = agreementPdf.createEst.estimatorDetails.GroupBy(x => x.SupplierVal).Select(x => x.Key).ToList();
                            foreach (var Supplierval in SupplierList)
                            {
                                EstimatorDetailList += "<tr>"
                                    + "<td style='padding:5px 0px 5px 40px;'>"
                                    + "<strong>"
                                    + "<label>"
                                    + (Supplierval == "Supplier" ? "Others" : Supplierval)
                                    + "</label></strong></td>"
                                    + "</tr>";
                                EstimatorDetailList += GetEstimatorLineItems(agreementPdf.createEst.estimatorDetails.Where(x => x.SupplierVal == Supplierval).ToList(), currentCurrency, agreementPdf.createEst._EstimatorPDFFilter);

                            }
                        }
                        else if (agreementPdf.createEst._EstimatorPDFFilter.GroupedbyLabor.Value || agreementPdf.createEst._EstimatorPDFFilter.GroupedbyMaterial.Value)
                        {
                            var Items = agreementPdf.createEst.estimatorDetails.Where(x => x.CategoryVal == "Labor").ToList();
                            if (agreementPdf.createEst._EstimatorPDFFilter.GroupedbyMaterial.Value)
                            {
                                Items = agreementPdf.createEst.estimatorDetails.Where(x => x.CategoryVal != "Labor").ToList();
                            }
                            EstimatorDetailList += GetEstimatorLineItems(Items, currentCurrency, agreementPdf.createEst._EstimatorPDFFilter);
                        }
                        else if (agreementPdf.createEst._EstimatorPDFFilter.GroupedbyLaborAndMaterial.Value)
                        {
                            EstimatorDetailList += "<tr>"
                                + "<td style='padding:5px 0px 5px 40px;'>"
                                + "<strong>"
                                + "<label>" +
                                "Labor"
                                + "</label></strong></td>"
                                + "</tr>";
                            var Items = agreementPdf.createEst.estimatorDetails.Where(x => x.CategoryVal == "Labor").ToList();
                            EstimatorDetailList += GetEstimatorLineItems(Items, currentCurrency, agreementPdf.createEst._EstimatorPDFFilter);
                            EstimatorDetailList += "<tr>"
                                + "<td style='padding:5px 0px 5px 40px;'>"
                                + "<strong>"
                                + "<label>"
                                + "Material"
                                + "</label></strong></td>"
                                + "</tr>";
                            Items = agreementPdf.createEst.estimatorDetails.Where(x => x.CategoryVal != "Labor").ToList();
                            EstimatorDetailList += GetEstimatorLineItems(Items, currentCurrency, agreementPdf.createEst._EstimatorPDFFilter);
                        }
                        else
                        {
                            EstimatorDetailList += GetEstimatorLineItems(agreementPdf.createEst.estimatorDetails, currentCurrency, agreementPdf.createEst._EstimatorPDFFilter);
                        }
                        EstimatorDetailList += "</tbody></table> ";
                        EstimatorDetailList += "<table style='width:100%; float:left; border-collapse:collapse; margin-top:30px; margin-bottom:100px; border-top:1px solid #4f90bb;'>";
                        if (agreementPdf.createEst._EstimatorPDFFilter.IncludeCost.Value)
                        {
                            EstimatorDetailList += "<tr><td style='padding-top:5px; width:90%; text-align:right; padding-right:10px; '>Total Cost</td><td style='padding-top:5px; width:10%; text-align:right; font-weight:bold;'>"
                            + currentCurrency + FormatAmount(agreementPdf.createEst.Estimator.TotalCost)
                         + "</td></tr>";
                        }
                        if (agreementPdf.createEst._EstimatorPDFFilter.IncludeProfit.Value || agreementPdf.createEst._EstimatorPDFFilter.IncludeMargin.Value)
                        {
                            EstimatorDetailList += "<tr><td style='padding-top:5px; width:90%; text-align:right; padding-right:10px; '>Total Profit/Margin</td><td style='padding-top:5px; width:10%; text-align:right; font-weight:bold;'>"
                         + currentCurrency + FormatAmount(agreementPdf.createEst.Estimator.TotalProfitAmount)
                         + "</td></tr>";
                        }

                        if (agreementPdf.createEst._EstimatorPDFFilter.IncludeOverhead.Value)
                        {
                            EstimatorDetailList += "<tr><td style='padding-top:5px; width:90%; text-align:right; padding-right:10px; '>Total Overhead</td><td style='padding-top:5px; width:10%; text-align:right; font-weight:bold;'>"
                         + currentCurrency + FormatAmount(agreementPdf.createEst.Estimator.TotalOverheadCostAmount)
                         + "</td></tr>";
                        }
                        if (!agreementPdf.createEst._EstimatorPDFFilter.GroupedbyLabor.Value && !agreementPdf.createEst._EstimatorPDFFilter.GroupedbyMaterial.Value
                                && !agreementPdf.createEst._EstimatorPDFFilter.WithoutPricing.Value)
                        {
                            EstimatorDetailList += "<tr><td style='padding-top:5px; width:90%; text-align:right; padding-right:10px; '>Sub Total</td><td style='padding-top:5px; width:10%; text-align:right; font-weight:bold;'>"
                         + currentCurrency + FormatAmount(agreementPdf.createEst.Estimator.TotalPrice)
                         + "</td></tr>";
                            EstimatorDetailList += "<tr><td style='padding-top:5px; width:90%; text-align:right; padding-right:10px; '>Total Tax</td><td style='padding-top:5px; width:10%; text-align:right; font-weight:bold;'>"
                             + currentCurrency + FormatAmount(agreementPdf.createEst.Estimator.TaxAmount)
                             + "</td></tr>";
                            EstimatorDetailList += "<tr><td style='padding-top:5px; width:90%; text-align:right; padding-right:10px; '>Total Amount</td><td style='padding-top:5px; width:10%; text-align:right; font-weight:bold;'>"
                             + currentCurrency + FormatAmount(agreementPdf.createEst.Estimator.TotalPrice + agreementPdf.createEst.Estimator.TaxAmount)
                             + "</td></tr>";
                        }
                        EstimatorDetailList += "</table>";


                    }
                    templateVars.Add("EstimatorDetailList", EstimatorDetailList);
                    if (agreementPdf.createEst.estimatorServices != null && agreementPdf.createEst._EstimatorPDFFilter.IncludeService.Value)
                    {
                        EstimatorServiceList = "<div style='page-break-after: always; display: block; clear: both; '></div>"
                                       + "<table style='width:100%; float:left; border-collapse:collapse; margin-bottom:20px; '>"
                                             + "<thead>"
                                             + "<tr style='background-color:#4f90bb; color:white;width:100%; border:1px solid #4f90bb;' >"
                                             + "<th style='width:65%;text-align:left;padding:5px 0px 5px 20px; border-right:1px solid #fff;'>SERVICE</th>"
                                             + "<th style='width:5%; padding:5px 0px; border-right:1px solid #fff; text-align:center;'>QTY</th>"
                                             + "<th style='width:15%; padding:5px 0px; border-right:1px solid #fff; text-align:center;'>UNIT PRICE</th>"
                                             + "<th style='width:15%; padding:5px 0px; border-right:1px solid #fff; text-align:center;'>TOTAL PRICE</th>"
                                             + "</tr></thead >"
                                             + "<tbody>";
                        foreach (var item in agreementPdf.createEst.estimatorServices)
                        {
                            EstimatorSerSERVICE = "<td style='padding:5px 0px 5px 20px;'><strong><label>" + item.EquipmentName + "</label></strong></td>";
                            EstimatorSerQTY = "<td style='padding:5px; text-align:center; '><strong><label>" + item.Quantity + "</label></strong></td>";
                            EstimatorSerUNITPRICE = "<td style='padding:5px; text-align:center; '><strong><label>" + currentCurrency + FormatAmount(item.UnitPrice) + "</label></strong></td>";
                            EstimatorSerTOTALPRICE = "<td style='padding:5px; text-align:center; '><strong><label>" + currentCurrency + FormatAmount(item.Amount) + "</label></strong></td>";

                            EstimatorServiceList += "<tr style='border-bottom:1px solid #ccc;'>" + EstimatorSerSERVICE + EstimatorSerQTY + EstimatorSerUNITPRICE + EstimatorSerTOTALPRICE + "</tr>";
                        }
                        EstimatorServiceList += "</tbody></table>";
                        EstimatorServiceList += "<table style='width:100%; float:left; border-collapse:collapse; margin-top:30px; border-top:1px solid #4f90bb;'>";
                        EstimatorServiceList += "<tr><td style='padding-top:5px; width:90%; text-align:right; padding-right:10px;'> Sub Total </td><td style='padding-top:5px; width:10%; text-align:right; font-weight:bold; '> "
                                                         + currentCurrency + FormatAmount(agreementPdf.createEst.Estimator.ServiceTotalAmount)
                                         + "</td></tr>";
                        if (agreementPdf.createEst.Estimator.ShowServicePlan.HasValue && agreementPdf.createEst.Estimator.ShowServicePlan.Value)
                        {
                            EstimatorServiceList += "<tr><td style='padding-top:5px; width:90%; text-align:right; padding-right:10px;'>" + agreementPdf.createEst.Estimator.ServicePlanTypeName + "</td><td style='padding-top:5px; width:10%; text-align:right; font-weight:bold; '> "
                                                         + currentCurrency + FormatAmount(agreementPdf.createEst.Estimator.ServicePlanAmount)
                                         + "</td></tr>";
                            EstimatorServiceList += "<tr><td style='padding-top:5px; width:90%; text-align:right; padding-right:10px;'> Sub Total </td><td style='padding-top:5px; width:10%; text-align:right; font-weight:bold; '> "
                                                             + currentCurrency + FormatAmount(agreementPdf.createEst.Estimator.ServiceTotalAmount + agreementPdf.createEst.Estimator.ServicePlanAmount)
                                             + "</td></tr>";
                        }

                        EstimatorServiceList += "<tr><td style='padding-top:5px; width:90%; text-align:right; padding-right:10px;'>Total Tax</td><td style='padding-top:5px; width:10%; text-align:right; font-weight:bold; '> "
                                                         + currentCurrency + FormatAmount(agreementPdf.createEst.Estimator.ServiceTaxAmount)
                                         + "</td></tr>";
                        EstimatorServiceList += "<tr><td style='padding-top:5px; width:90%; text-align:right; padding-right:10px;'>Total Amount</td><td style='padding-top:5px; width:10%; text-align:right; font-weight:bold;'> "
                                                         + currentCurrency + FormatAmount(Math.Round(agreementPdf.createEst.Estimator.ServiceTotalAmount.Value, 2) + Math.Round(agreementPdf.createEst.Estimator.ServiceTaxAmount.Value, 2) + Math.Round(agreementPdf.createEst.Estimator.ServicePlanAmount.Value, 2))
                                         + "</td></tr>";
                        EstimatorServiceList += "</table>";
                    }
                    templateVars.Add("EstimatorServiceList", EstimatorServiceList);
                    string fullname = agreementPdf.FirstName + " " + agreementPdf.MiddleName + " " + agreementPdf.LastName;
                    String result = "";

                    // Traverse the string.  
                    bool v = true;
                    for (int i = 0; i < fullname.Length; i++)
                    {
                        // If it is space, set v as true.  
                        if (fullname[i] == ' ')
                        {
                            v = true;
                        }

                        // Else check if v is true or not.  
                        // If true, copy character in output  
                        // string and set v as false.  
                        else if (fullname[i] != ' ' && v == true)
                        {
                            result += (fullname[i]);
                            v = false;
                        }
                    }
                    templateVars.Add("CustomerInitName", result);
                    if (agreementPdf.createEst.Estimator.ServicePlanType == "0.0095")
                    {
                        PremiumPlan = "<table style='width:100%; float:left; border-collapse:collapse; '>" +
                            "<tr>" +
                            "<td style='padding:20px 0px 0px 0px; font-size:18px; font-weight:bold; text-transform:uppercase;' >" +
                            "PREMIUM SERVICE PLAN Terms" +
                            "<span style='font-size:12px; margin-left:10px;'> " +
                            "Adds $11.00 per month" +
                            "</span>" +
                            "</td>" +
                            "</tr>" +
                            "<tr>" +
                            "<td>(24x7, 365 days)" +
                            "<br/>" +
                            "EST Premium Service Plan covers labor and equipment costs. " +
                            "The service plan can cover all types of protection systems including intrusion alarms, fire alarms, " +
                            "camera systems and access control systems.This plan covers normal 'wear and tear', repair or replacement." +
                            "Repair or replacement of equipment damaged by the customer, acts of God or vandalism is not covered. " +
                            "Includes access to the EST Telephone Support(24x7)." +
                            "</td>" +
                            "</tr>" +
                            "</table> ";
                    }
                    if (agreementPdf.createEst.Estimator.ServicePlanType == "0.005")
                    {
                        StandardPlan = "<table style='width:100%; float:left; border-collapse: collapse; '>" +
                            "<tr>" +
                            "<td style ='padding:20px 0px 0px 0px; font-size:18px; font-weight:bold; text-transform:uppercase;' >" +
                            "Standard service plan terms: " +
                            "<span style='font-size:12px; margin-left:10px;'> " +
                            "Price as listed" +
                            "</span> " +
                            "</td>" +
                            "</tr>" +
                            "<tr>" +
                            "<td>" +
                            "(Monday - Friday, 8am - 4pm) EST Standard Service Plan covers labor and equipment cost during normal business hours." +
                            "The service plan can cover all types of protection systems including intrusion alarms, fire alarms, camera systems and access control systems including intrusion alarms, fire alarms, " +
                            "camera systems and access control systems." +
                            "This plan covers normal 'wear and tear', repair or replacement.Repair or replacement of equipment damaged by the customer, acts of God or vandalism is not covered." +
                            "Service labor rates for after hours work are not included and are based on current EST service labor rate schedule." +
                            "Includes access to the EST Telephone Support(24x7)." +
                            "</td>" +
                            "</tr>" +
                            "</table>";
                    }
                    templateVars.Add("PremiumPlan", PremiumPlan);
                    templateVars.Add("StandardPlan", StandardPlan);
                    templateVars.Add("EstimatorContractTerm", agreementPdf.createEst.EstimatorContractTerm);
                    templateVars.Add("EstimatorInstallTotal", currentCurrency + FormatAmount(agreementPdf.createEst.Estimator.TotalPrice + agreementPdf.createEst.Estimator.TaxAmount));
                    templateVars.Add("EstimatorMonthlyTotal", currentCurrency + FormatAmount(Math.Round(agreementPdf.createEst.Estimator.ServiceTotalAmount.Value, 2) + Math.Round(agreementPdf.createEst.Estimator.ServiceTaxAmount.Value, 2) + Math.Round(agreementPdf.createEst.Estimator.ServicePlanAmount.Value, 2)) + "(Included Tax)");
                }
                #endregion

                #region payment
                if (agreementPdf.PaymentDetails != null)
                {
                    if (agreementPdf.PaymentDetails.Type == "Credit Card")
                    {
                        var tr1 = "<tr style='height:25px;'>" +
                                            "<td style='font-weight:bold; width:140px; padding-left:10px;'>" +
                                                "Credit Card Type: " +
                                            "</td>" +
                                            "<td>" + agreementPdf.PaymentDetails.CardType + "</td>" +
                                            "<td style='font-weight:bold; width:140px; padding-left:10px;'>" +
                                                "Payment Type: " +
                                            "</td>" +
                                            "<td></td>" +
                                        "</tr>";
                        var tr2 = "<tr style='height: 25px; '>" +
                                               "<td style = 'font-weight:bold; padding-left:10px; ' >" +
                                                     "Account" +
                                                "</td>" +
                                                "<td style = 'border-bottom:1px solid #000;' >" + agreementPdf.PaymentDetails.CardNumber + "</td>" +
                                            "<td style = 'padding-left:10px; border-bottom: 1px solid #fff;' >" +
                                                 "Exp" +
                                             "</td >" +
                                             "<td style = 'border-bottom:1px solid #000;' >" + agreementPdf.PaymentDetails.CardExpireDate + "</td>" +
                                        "</tr >";
                        var tr3 = "<tr style='height: 25px; '>" +
                                               "<td style = 'font-weight:bold; padding-left:10px; ' >" +
                                                    " Name on Card" +
                                            "</td >" +
                                           " <td colspan = '3' style = 'border-bottom:1px solid #000;' >" + agreementPdf.PaymentDetails.AccountName + "</td>" +
                                           "</tr >";
                        var tr4 = "<tr style='height: 25px; '>" +
                                               "<td style = 'font-weight:bold; padding-left:10px; ' >" +
                                                     "Address on Card" +
                                            "</td >" +
                                            "<td colspan = '3' style = 'border-bottom:1px solid #000;' ></td>" +
                                        "</tr > ";
                        var CreditCardTable = "<table style='border-collapse:collapse; width:100%; float:left; table-layout:fixed; border: 2px solid #000; font-size:13px; border-bottom:0px;'>" + tr1 + tr2 + tr3 + tr4 + "</table>";
                        templateVars.Add("CreditCardTable", CreditCardTable);
                    }
                    if (agreementPdf.PaymentDetails.Type == "ACH")
                    {
                        var tr1 = "<tr style='height: 25px; '>" +
                                              "<td style = 'font-weight:bold; width:140px; padding-left:10px; ' >" +
                                                    "Bank Account Type:" +
                                            "</td>" +
                                            "<td>" + agreementPdf.PaymentDetails.BankAccountType + "</td>" +
                                        "</tr> ";
                        var tr2 = "<tr style='height: 25px; '>" +
                                              "<td style = 'font-weight:bold; padding-left:10px; ' >" +
                                                    "Bank Name" +
                                               "</td>" +
                                               "<td style = 'border-bottom:1px solid #000;' ></td>" +
                                        "</tr> ";
                        var tr3 = "<tr style='height: 25px; '>" +
                                              "<td style = 'font-weight:bold; padding-left:10px; ' >" +
                                                    "Account" +
                                               "</td >" +
                                               "<td style = 'border-bottom:1px solid #000;' >" + agreementPdf.PaymentDetails.AcountNo + "</td>" +
                                        "</tr>";
                        var tr4 = "<tr style='height: 25px; '>" +
                                              "<td style = 'font-weight:bold; padding-left:10px;' >" +
                                                   "Routing" +
                                               "</td>" +
                                               "<td style = 'border-bottom:1px solid #000;' >" + agreementPdf.PaymentDetails.RoutingNo + "</td>" +
                                        "</tr> ";
                        var BankAccountTable = "<table style='border-collapse:collapse; width:100%; float:left; table-layout:fixed;border:2px solid #000; font-size:13px; border-bottom:0px;'>" + tr1 + tr2 + tr3 + tr4 + "</table>";
                        templateVars.Add("BankAccountTable", BankAccountTable);
                    }
                }
                #endregion

                #region NonConformingFee
                var NonConfirmingFeeDivRab = "";
                var NonConfirmingFeeDivDFW = "";
                if (agreementPdf.NonConfirmingFee > 0)
                {
                    NonConfirmingFeeDivRab = "<div style='width:80%;float:left;padding-left:10px;padding-top:5px'><span>NON CONFORMING FEE</span></div>" +
                                         "<div style='width:18%;float:left;text-align:right;padding-top:5px'>{0}</div>";
                    NonConfirmingFeeDivRab = string.Format(NonConfirmingFeeDivRab, currentCurrency + FormatAmount(agreementPdf.NonConfirmingFee));

                    NonConfirmingFeeDivDFW = "<tr style='height:25px;'><td valign ='middle' style='font-weight:bold; border:1px solid #000; text-align:right; padding-right:5px; background-color:#f3f3f3;' > +Non Conforming Fee </td>" +
                                            "<td style='border:1px solid #000;text-align: right;padding-right: 10px;'>{0}</td></tr>";
                    NonConfirmingFeeDivDFW = string.Format(NonConfirmingFeeDivDFW, currentCurrency + FormatAmount(agreementPdf.NonConfirmingFee));
                    NonConfirmingFeeDivCommFire = "<tr style='border-bottom:1px solid #ccc'><td style=\"padding:5px 0px 5px 20px\"><strong><label> + Non Conforming Fee </label></strong></td><td style=\"padding:5px;text-align:center\"><strong><label>{0}</label></strong></td></tr>";
                    NonConfirmingFeeDivCommFire = string.Format(NonConfirmingFeeDivCommFire, currentCurrency + FormatAmount(agreementPdf.NonConfirmingFee));
                }
                templateVars.Add("NonConfirmingFeeDivRab", NonConfirmingFeeDivRab);
                templateVars.Add("NonConfirmingFeeDivDFW", NonConfirmingFeeDivDFW);
                templateVars.Add("NonConfirmingFeeDivCommFire", NonConfirmingFeeDivCommFire);

                #endregion
                #region Advance Monitoring Service Fee
                var AdvanceMonitoringFeeRab = "";
                var AdvanceMonitoringFeeDFW = "";
                if (agreementPdf.AdvanceServiceFeeTotal > 0)
                {
                    AdvanceMonitoringFeeRab = "<div style='width:80%;float:left;padding-left:10px;padding-top:5px'><span>ADVANCE MONITORING  FEE</span></div>" +
                                         "<div style='width:18%;float:left;text-align:right;padding-top:5px'>{0}</div>";
                    AdvanceMonitoringFeeRab = string.Format(AdvanceMonitoringFeeRab, currentCurrency + FormatAmount(AdvanceMonitoringFee));

                    AdvanceMonitoringFeeDFW = "<tr style='height:25px;'><td valign ='middle' style='font-weight:bold; border:1px solid #000; text-align:right; padding-right:5px; background-color:#f3f3f3;' > +Advance Monitoring Fee </td>" +
                                            "<td style='border:1px solid #000;text-align: right;padding-right: 10px;'>{0}</td></tr>";
                    AdvanceMonitoringFeeDFW = string.Format(AdvanceMonitoringFeeDFW, currentCurrency + FormatAmount(AdvanceMonitoringFee));
                }
                templateVars.Add("AdvanceMonitoringFeeRab", AdvanceMonitoringFeeRab);
                templateVars.Add("AdvanceMonitoringFeeDFW", AdvanceMonitoringFeeDFW);

                #endregion
                EmailParser parser = null;
                if (templateid.HasValue && templateid.Value > 0)
                {
                    CustomerAgreementTemplate agreementTemplate = _CustomerAgreementTemplateDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and Id = {1}", agreementPdf.CompanyId, templateid.Value)).FirstOrDefault();
                    parser = new EmailParser(agreementTemplate.BodyContent, templateVars, false);
                    MailMessage message = new MailMessage();
                    message.Body = parser.Parse();
                    Body = message.Body;
                }
                else if (agreementPdf.FirstPage == true)
                {
                    EmailTemplate agreementTemplate = _EmailTemplateDataAccess.GetEmailTemplateByKeyAndCompanyId(agreementPdf.CompanyId, EmailTemplateKey.AgreementFirstPage.SmartAgreementFirstPage);
                    parser = new EmailParser(System.Web.Hosting.HostingEnvironment.MapPath(agreementTemplate.BodyFile), templateVars, true);
                    MailMessage message = new MailMessage();
                    message.Body = parser.Parse();
                    Body = message.Body;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(agreementPdf.ContractType) && agreementPdf.ContractType.ToLower() == "commercial")
                    {
                        string TemplateKeyName = EmailTemplateKey.SmartAgreementCommercial.SmartAgreementCommercialRMR;
                        if (IsBBBConflict)
                        {
                            TemplateKeyName = EmailTemplateKey.SmartAgreementCommercial.SmartAgreementCommercialSignedRMR;
                        }
                        EmailTemplate agreementTemplate = _EmailTemplateDataAccess.GetEmailTemplateByKeyAndCompanyId(agreementPdf.CompanyId, TemplateKeyName);
                        parser = new EmailParser(System.Web.Hosting.HostingEnvironment.MapPath(agreementTemplate.BodyFile), templateVars, true);
                        MailMessage message = new MailMessage();
                        message.Body = parser.Parse();
                        Body = message.Body;
                    }
                    else if (!string.IsNullOrWhiteSpace(agreementPdf.ContractType) && agreementPdf.ContractType.ToLower() == "commercialfire")
                    {
                        string TemplateKeyName = EmailTemplateKey.SmartAgreementCommercialFire.SmartAgreementCommercialFireRMR;
                        if (IsBBBConflict)
                        {
                            TemplateKeyName = EmailTemplateKey.SmartAgreementCommercialFire.SmartAgreementCommercialFireSignedRMR;
                        }
                        EmailTemplate agreementTemplate = _EmailTemplateDataAccess.GetEmailTemplateByKeyAndCompanyId(agreementPdf.CompanyId, TemplateKeyName);
                        parser = new EmailParser(System.Web.Hosting.HostingEnvironment.MapPath(agreementTemplate.BodyFile), templateVars, true);
                        MailMessage message = new MailMessage();
                        message.Body = parser.Parse();
                        Body = message.Body;
                    }
                    else
                    {
                        string TemplateKeyName = EmailTemplateKey.SmartAgreement.SmartAgreementRMR;
                        if (IsBBBConflict)
                        {
                            TemplateKeyName = EmailTemplateKey.SmartAgreement.SmartAgreementSignedRMR;
                        }
                        EmailTemplate agreementTemplate = _EmailTemplateDataAccess.GetEmailTemplateByKeyAndCompanyId(agreementPdf.CompanyId, TemplateKeyName);
                        parser = new EmailParser(System.Web.Hosting.HostingEnvironment.MapPath(agreementTemplate.BodyFile), templateVars, true);
                        MailMessage message = new MailMessage();
                        message.Body = parser.Parse();
                        Body = message.Body;
                    }

                }
            }
            catch (Exception ex)
            {
                //Logger.AddElmah(ex);
            }

            return Body;
        }
        public List<Ticket> GetTicketListByCustomerIdAndCompanyId(Guid customerid, Guid companyid)
        {
            return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}'", customerid, companyid)).ToList();
        }
        public List<CustomerAppointmentEquipment> GetCustomerAppointmentEquipmentByAppointmentIdAndBilling(Guid appointmentid)
        {
            return _CustomerAppointmentEquipmentDataAccess.GetByQuery(string.Format("AppointmentId = '{0}' and IsBilling = 1 and IsCopied != 1 and IsService = 1", appointmentid)).ToList();
        }
        public string MakeCustomerAddendumPdf(CustomerAddendumModel cusAddendum)
        {
            string currentCurrency = "";
            string ServiceList = "";
            string AddedList = "";
            string RemovedList = "";
            string EquipmentList = "";
            string Body = "";
            double MonitoringFee = 0;
            double AdditionalMonthlyFee = 0;
            double MonthlyRate = 0;
            double Added = 0;
            double Removed = 0;
            double BillingTicketTotalAmount = 0;
            double NewTicketTotalAmount = 0;
            double DefaultServiceAmount = 0;
            string KazarAddendumQTY = "";
            string KazarAddendumName = "";
            string KazarAddendumPrice = "";
            string KazarAddendumSubTotal = "";
            double KazarAddendumTotalSubTotal = 0.0;
            double KazarAddendumFinalTotal = 0.0;
            string KazarAddendumList = "";
            if (cusAddendum.CurrentCurrency != null)
            {
                currentCurrency = cusAddendum.CurrentCurrency;
            }
            else
            {
                currentCurrency = "$";
            }
            var objtiklist = GetTicketListByCustomerIdAndCompanyId(cusAddendum.CustomerId, cusAddendum.CompanyId);
            if (objtiklist != null && objtiklist.Count > 0)
            {
                foreach (var tik in objtiklist)
                {
                    var objappeqp = GetCustomerAppointmentEquipmentByAppointmentIdAndBilling(tik.TicketId);
                    if (objappeqp != null && objappeqp.Count > 0)
                    {
                        foreach (var eqp in objappeqp)
                        {
                            BillingTicketTotalAmount += eqp.TotalPrice;
                        }
                    }
                }
            }
            if (cusAddendum.ServiceEqpList != null)
            {
                foreach (var item in cusAddendum.ServiceEqpList)
                {
                    ServiceList += "<tr><td>" + item.EquipmentServiceName + "</td><tr>";
                    if (item.IsDefaultService == false)
                    {
                        MonitoringFee += item.TotalPrice;
                        NewTicketTotalAmount += item.TotalPrice;
                    }
                }

                var ACHDiscountAmount = 0.0;
                var objpayinfocus = _PaymentInfoCustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and Payfor = 'MMR'", cusAddendum.CustomerId)).FirstOrDefault();
                if (objpayinfocus != null)
                {
                    var objpayprofile = _PaymentProfileCustomerDataAccess.GetByQuery(string.Format("PaymentInfoId = {0}", objpayinfocus.PaymentInfoId)).FirstOrDefault();
                    if (objpayprofile != null && objpayprofile.Type.ToLower().IndexOf("ach") > -1)
                    {
                        var objglobal = _GlobalSettingDataAccess.GetByQuery(string.Format("SearchKey = 'ACHDiscount'")).FirstOrDefault();
                        if (objglobal != null)
                        {
                            ACHDiscountAmount = Convert.ToDouble(objglobal.Value);
                        }
                    }
                }
                if (ACHDiscountAmount > 0)
                {
                    MonitoringFee = MonitoringFee - ACHDiscountAmount;
                    //NewTicketTotalAmount = NewTicketTotalAmount - ACHDiscountAmount;
                }
                MonitoringFee -= DefaultServiceAmount;
                if (BillingTicketTotalAmount > 0 && NewTicketTotalAmount > 0 && BillingTicketTotalAmount != NewTicketTotalAmount)
                {
                    if (NewTicketTotalAmount > BillingTicketTotalAmount)
                    {
                        Added = NewTicketTotalAmount - BillingTicketTotalAmount;
                    }
                    else
                    {
                        Removed = BillingTicketTotalAmount - NewTicketTotalAmount;
                    }
                }
                ServiceList = "<table style='width=100%';'border='1'>" + ServiceList + "</table>";
            }

            if (cusAddendum.EquipmentList != null)
            {
                foreach (var item in cusAddendum.EquipmentList)
                {
                    EquipmentList += "<tr><td>" + item.EquipmentServiceName + "</td><td>" + currentCurrency + item.TotalPrice.toFixed(2) + "</td><tr>";
                }
                EquipmentList = "<table style='width=100%';'border='1'>" + EquipmentList + "</table>";
            }
            try
            {
                Hashtable templateVars = new Hashtable();

                templateVars.Add("COMPANYLOGO", cusAddendum.CompanyLogo);
                templateVars.Add("KazarLogo", cusAddendum.KazarLogo);
                templateVars.Add("KazarLogoIcon", cusAddendum.KazarLogoIcon);
                templateVars.Add("COMPANYADDRESS", cusAddendum.CompanyAddress);

                templateVars.Add("COMPANYZIP", cusAddendum.CompanyZip);
                templateVars.Add("COMPANYCITY", cusAddendum.CompanyCity);
                templateVars.Add("COMPANYSTATE", cusAddendum.CompanyState);
                templateVars.Add("SERVICELIST", ServiceList);
                if (Added > 0)
                {
                    AddedList = "<table style='width=100%';'border='1'><tr><td>Added: " + currentCurrency + Added.toFixed(2) + "</td><tr></table>";
                    templateVars.Add("AddedList", AddedList);
                }
                if (Removed > 0)
                {
                    RemovedList = "<table style='width=100%';'border='1'><tr><td>Removed: " + currentCurrency + Removed.toFixed(2) + "</td><tr></table>";
                    templateVars.Add("RemovedList", RemovedList);
                }
                templateVars.Add("EQUIPMENTLIST", EquipmentList);
                if (cusAddendum.CompanyPhone != "")
                {
                    templateVars.Add("COMPANYPHONW", cusAddendum.CompanyPhone + " (Office)");
                }


                templateVars.Add("FIRSTNAME", cusAddendum.FirstName);
                templateVars.Add("LASTNAME", cusAddendum.LastName);
                templateVars.Add("CustomerStreet", cusAddendum.CustomerStreet);
                templateVars.Add("CustomerCity", cusAddendum.CustomerCity);
                templateVars.Add("CustomerState", cusAddendum.CustomerState);
                templateVars.Add("CustomerZip", cusAddendum.CustomerZip);
                templateVars.Add("INSTALLADDRESS", cusAddendum.InstallAddress);

                #region Kazar addendum Data
                templateVars.Add("WorkToBePerformed", cusAddendum.WorkToBePerformed);
                if (cusAddendum.ServiceEqpList != null)
                {
                    //templateVars.Add("MonthlyMonitoringFee", currentCurrency + FormatAmount(Convert.ToDouble(cusAddendum.ServiceEqpList.Where(x => x.EquipName == "Monthly Monitoring Rate").Select(x => x.TotalPrice).FirstOrDefault())));
                    foreach (var item in cusAddendum.ServiceEqpList)
                    {

                        if (item.IsBilling == true)
                        {
                            MonthlyRate += item.TotalPrice;
                        }
                        else
                        {
                            AdditionalMonthlyFee += item.TotalPrice;
                        }
                    }
                    templateVars.Add("AdditionalMonthlyFee", currentCurrency + FormatAmount(Convert.ToDouble(AdditionalMonthlyFee)));
                    templateVars.Add("MonthlyMonitoringFee", currentCurrency + FormatAmount(Convert.ToDouble(MonthlyRate)));
                    templateVars.Add("NewMonthlyTotal", currentCurrency + FormatAmount(Convert.ToDouble(AdditionalMonthlyFee + MonthlyRate)));
                }
                if (!string.IsNullOrWhiteSpace(cusAddendum.SalesRepresentative) && cusAddendum.SalesRepresentative != "-1")
                {
                    templateVars.Add("SalesRepresentative", cusAddendum.SalesRepresentative);
                }
                else
                {
                    templateVars.Add("SalesRepresentative", "");
                }

                if (cusAddendum.EquipmentList != null && cusAddendum.EquipmentList.Count() > 0)
                {
                    foreach (var item in cusAddendum.EquipmentList)
                    {
                        KazarAddendumQTY = "<td style='text-align:right; border:1px solid #ebebeb; padding:10px; height:30px; width:60px;' valign='middle'>" + item.Quantity + "</td>";
                        KazarAddendumName = "<td style='text-align:center; border: 1px solid #ebebeb; padding:10px; height:30px;  width:60%;' valign='middle'>" + item.EquipName + "</td>";
                        KazarAddendumPrice = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(item.UnitPrice) + "</td>";
                        KazarAddendumSubTotal = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(item.TotalPrice) + "</td>";

                        KazarAddendumList += "<tr style=''>" + KazarAddendumQTY + KazarAddendumName + KazarAddendumPrice + KazarAddendumSubTotal + "</tr>";
                        KazarAddendumTotalSubTotal += item.TotalPrice;
                    }
                }
                else
                {
                    KazarAddendumQTY = "<td style='text-align:right; border:1px solid #ebebeb; padding:10px; height:30px; width:60px;' valign='middle'>" + 0 + "</td>";
                    KazarAddendumName = "<td style='text-align:center; border: 1px solid #ebebeb; padding:10px; height:30px;  width:60%;' valign='middle'> </td>";
                    KazarAddendumPrice = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(0.0) + "</td>";
                    KazarAddendumSubTotal = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(0.0) + "</td>";

                    KazarAddendumList += "<tr style=''>" + KazarAddendumQTY + KazarAddendumName + KazarAddendumPrice + KazarAddendumSubTotal + "</tr>";
                }
                KazarAddendumFinalTotal = KazarAddendumTotalSubTotal + ((KazarAddendumTotalSubTotal * cusAddendum.Tax) / 100);
                KazarAddendumList += "<tr style=''><td colspan='2' style ='text-align:right; padding:10px;'></td ><td style ='text-align:right; padding:10px;'>Subtotal</td><td style ='text-align:right; padding:10px;font-weight:bold;'>" +
                                    currentCurrency + FormatAmount(KazarAddendumTotalSubTotal) +
                                "</td ></tr>";
                KazarAddendumList += "<tr style=''><td colspan='2' style ='text-align:right; padding:10px;'></td ><td style ='text-align:right; padding:10px;'>Tax(" + cusAddendum.Tax + "%)</td><td style ='text-align:right; padding:10px;font-weight:bold;'>" +
                                currentCurrency + FormatAmount(((KazarAddendumTotalSubTotal * cusAddendum.Tax) / 100)) +
                            "</td ></tr>";
                KazarAddendumList += "<tr style='font-weight:bold;'><td colspan ='2' style='text-align:right; padding:10px;'></td><td style ='text-align:right; padding:10px;'>Total</td> <td style ='text-align:right; padding:10px;'>" +
                                currentCurrency + FormatAmount(KazarAddendumFinalTotal) +
                            "</td></tr>";
                templateVars.Add("AddendumEquipmentList", KazarAddendumList);
                #endregion

                templateVars.Add("BUSINESSNAME", cusAddendum.BuisnessName);
                templateVars.Add("EMAILADDRESS", cusAddendum.EmailAddress);

                if (cusAddendum.CellPhone != "")
                {
                    templateVars.Add("CELLPHONE", cusAddendum.CellPhone + " (Cell Phone)");
                }
                if (cusAddendum.SitePhone != "")
                {
                    templateVars.Add("SITEPHONE", cusAddendum.SitePhone + " Primary Phone");
                }

                templateVars.Add("TICKETID", cusAddendum.TicketId);
                if (cusAddendum.ScheduleOn != new DateTime())
                {
                    templateVars.Add("SCHEDULEON", cusAddendum.ScheduleOn.ToString("MM/dd/yy"));
                }
                if (cusAddendum.AgreementSignDate != new DateTime())
                {
                    templateVars.Add("AgreementSignDate", cusAddendum.AgreementSignDate.ToString("M/dd/yy"));
                }
                if (cusAddendum.AddendumCreateDate != new DateTime())
                {
                    templateVars.Add("AddendumCreateDate", cusAddendum.AddendumCreateDate.ToString("M/dd/yy"));
                }
                templateVars.Add("RECURRINGAMOUNT", currentCurrency + String.Format("{0:0,0.00}", MonitoringFee));
                if (!string.IsNullOrWhiteSpace(cusAddendum.CustomerAddendumSignature))
                {
                    templateVars.Add("CustomerAddendumSignature", cusAddendum.CustomerAddendumSignature);
                    if (!string.IsNullOrWhiteSpace(cusAddendum.CompanySignature))
                    {
                        templateVars.Add("CompanySignature", cusAddendum.CompanySignature);
                        if (!string.IsNullOrWhiteSpace(cusAddendum.CompanySignatureDate))
                            templateVars.Add("CompanySignatureDate", cusAddendum.CompanySignatureDate);
                    }
                    if (!string.IsNullOrWhiteSpace(cusAddendum.CustomerAddendumStringSignatureDate))
                    {
                        templateVars.Add("CustomerAddendumSignatureDate", cusAddendum.CustomerAddendumStringSignatureDate);
                    }
                }

                //templateVars.Add("CustomerAddendumSignatureDate", cusAddendum.CustomerAddendumSignatureDate.UTCToClientTime().ToString("MM/dd/yyyy"));
                string TemplateKeyName = EmailTemplateKey.CostomerAddendumPdf.CustomerAddendum;

                EmailParser parser = null;
                EmailTemplate CustomerAddendumTemplate = _EmailTemplateDataAccess.GetEmailTemplateByKeyAndCompanyId(cusAddendum.CompanyId.ToString(), TemplateKeyName);


                parser = new EmailParser(System.Web.Hosting.HostingEnvironment.MapPath(CustomerAddendumTemplate.BodyFile), templateVars, true);
                MailMessage message = new MailMessage();
                message.Body = parser.Parse();
                Body = message.Body;



            }
            catch (Exception ex)
            {
                //Logger.AddElmah(ex);
            }

            return Body;
        }
        #endregion

        public bool SendAgrementSMS(SMSAgreement sms, Guid SendBy, Guid CompanyId, List<string> ReceiverNumberList, bool IsSystemAutoSent, string FromName)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("AgreementLINK", sms.ShortUrl);
                templateVars.Add("ToNumber", sms.ToNumber);
                templateVars.Add("CompanyName", sms.CompanyName);
                if (SendSMS(templateVars, SendBy, SMSTemplateKey.AgreementSMS.SendAgrementSms, CompanyId, ReceiverNumberList, IsSystemAutoSent, FromName))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }

            return false;
        }
        public bool SendSMS(Hashtable TemplateValue, Guid SendBy, string TemplateKey, Guid CompanyId, List<string> ReceiverNumberList, bool IsSystemAutoSent, string FromName)
        {
            SMSTemplate smsTemplate = _SMSTemplateDataAccess.GetByQuery($"TemplateKey='{TemplateKey}'").FirstOrDefault();
            MailMessage message = new MailMessage();
            #region Body
            if (smsTemplate.Body.IndexOf("##") > -1)
            {
                EmailParser BodyParser = new EmailParser(smsTemplate.Body, TemplateValue, false);
                message.Body = BodyParser.Parse();
            }
            else
            {
                message.Body = smsTemplate.Body;
            }
            #endregion
            return SendSMSPrivate(CompanyId, SendBy, TemplateKey, message.Body, ReceiverNumberList, IsSystemAutoSent, FromName);
        }
        public long InsertCorrespondence(LeadCorrespondence lc)
        {
            try
            {
                _ActivityDataAccess.Insert(new Activity()
                {
                    ActivityId = Guid.NewGuid(),
                    ActivityType = lc.Type,
                    AssignedTo = lc.SentBy,
                    AssociatedWith = lc.CustomerId,
                    AssociatedType = lc.AssociatedType,
                    Status = "Completed",
                    CreatedBy = lc.SentBy,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    Note = lc.BodyContent,
                    NotifyBy = "",
                });
            }
            catch (Exception) { }
            return _LeadCorrespondenceDataAccess.Insert(lc);
        }
        public CustomerSignature GetRecreateCustomerSignatureByCustomerId(Guid customerid)
        {
            return _CustomerSignatureDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and [Type] = 'Recreate'", customerid)).FirstOrDefault();
        }

        public bool EmailOnlyLeadsAggrement(LeadsAggrement email, Guid gCompanyId, string from)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("CustomerNum", email.CustomerNum);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("Body", email.BodyLink);
                templateVars.Add("CustomerId", email.CustomerId);
                templateVars.Add("EmployeeId", email.EmployeeId);
                List<Attachment> att = new List<Attachment>();
                att.Add(email.LeadsAggrementpdf);
                if (SentEmail(templateVars, EmailTemplateKey.mailtoLeadsAggrement.EmailtoLeadsAggrement, gCompanyId, att, from))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }

            return false;
        }

        public bool SentEmail(Hashtable TemplateValue, string TemplateKey, Guid CompanyId, List<Attachment> Attachments, string from = "")
        {

            #region Common Templates
            var SiteDomain = ConfigurationManager.AppSettings["SiteDomain"];
            var FacebookTemplate = "<a href='{0}'><img style='height:30px;' src='{1}/Content/Icons/Social/facebook_circle.png' /></a>";
            var InstagramTemplate = "<a href='{0}'><img style='height: 30px;' src='{1}/Content/Icons/Social/instagram_circle.png' /></a>";
            var YoutubeTemplate = "<a href='{0}'><img style='height:30px;' src='{1}/Content/Icons/Social/youtube_circle.png' /></a>";
            var FacebookLink = GetFacebookUrlByCompanyId(CompanyId);
            var InstagramLink = GetInstagramUrlByCompanyId(CompanyId);
            var YoutubeLink = GetYoutubeUrlByCompanyId(CompanyId);
            if (!string.IsNullOrEmpty(FacebookLink))
            {
                FacebookTemplate = string.Format(FacebookTemplate, FacebookLink, SiteDomain);
                TemplateValue.Add("FacebookDiv", FacebookTemplate);
            }
            if (!string.IsNullOrEmpty(InstagramLink))
            {
                InstagramTemplate = string.Format(InstagramTemplate, InstagramLink, SiteDomain);
                TemplateValue.Add("InstagramDiv", InstagramTemplate);
            }
            if (!string.IsNullOrEmpty(YoutubeLink))
            {
                YoutubeTemplate = string.Format(YoutubeTemplate, YoutubeLink, SiteDomain);
                TemplateValue.Add("YoutubeDiv", YoutubeTemplate);
            }

            if (TemplateValue["Logo"] == null)
            {
                TemplateValue.Add("Logo", GetEmailLogoByCompanyId(CompanyId));
            }
            if (TemplateValue["TeamNameSignature"] == null)
            {
                TemplateValue.Add("TeamNameSignature", GetTeamNameSignatureByCompanyId(CompanyId));
            }
            if (TemplateValue["CompanyNameAlt"] == null)
            {
                TemplateValue.Add("CompanyNameAlt", GetCompanyNameByCompanyId(CompanyId));
            }
            if (TemplateValue["CompanyInformation"] == null)
            {
                string Footer = GetFooterCompanyInformationByCompanyId(CompanyId);
                if (Footer.IndexOf("##Year##") > -1)
                {
                    Footer = Footer.Replace("##Year##", "2017-" + DateTime.Now.Year.ToString());
                }
                TemplateValue.Add("CompanyInformation", Footer);
            }
            #endregion

            EmailTemplate emailTemplate = _EmailTemplateDataAccess.GetByQuery("TemplateKey='" + TemplateKey + "'").FirstOrDefault();
            //if(emailTemplate == null)
            //{
            //    try
            //    {
            //        using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\ErrorReports\EmailError.txt"), true))
            //        {
            //            file.WriteLine(string.Format("{0} Template Not Found. Template Key: {1}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"), TemplateKey));

            //        }
            //    }
            //    catch (Exception) { }
            //}
            EmailParser parser = null;
            string toEmailAddress = "";
            string FromName = "";

            #region BodyFile And BodyContent
            if (emailTemplate == null || emailTemplate.Id == 0)
            {
                //Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception("Template not found for key :" + TemplateKey));
                return false;
            }
            if (!string.IsNullOrWhiteSpace(emailTemplate.BodyContent))
            {
                parser = new EmailParser(emailTemplate.BodyContent, TemplateValue, false);
            }
            else if (!string.IsNullOrWhiteSpace(emailTemplate.BodyFile))
            {
                parser = new EmailParser(System.Web.Hosting.HostingEnvironment.MapPath(emailTemplate.BodyFile), TemplateValue, true);
            }
            #endregion

            MailMessage message = new MailMessage();
            message.Body = parser.Parse();

            #region ToEmail
            if (emailTemplate.ToEmail.IndexOf("##") > -1)
            {
                EmailParser ToEmailParser = new EmailParser(emailTemplate.ToEmail, TemplateValue, false);
                string EmailAddress = ToEmailParser.Parse();
                if (EmailAddress.IsValidEmailAddress())
                {
                    message.To.Add(new MailAddress(EmailAddress));
                    toEmailAddress = message.To[0].ToString();
                }
                else
                {
                    EmailAddress = EmailAddress.Replace(" ", "");
                    toEmailAddress = EmailAddress;
                    if (EmailAddress.Split(',').Count() > 1)
                    {
                        string[] addList = EmailAddress.Split(',');
                        foreach (var item in addList)
                        {
                            if (item.IsValidEmailAddress())
                            {
                                message.To.Add(new MailAddress(item));
                            }
                            else if (item.Split('-').Count() > 0 && item.Split('-')[0].IsValidEmailAddress())
                            {
                                message.To.Add(new MailAddress(item.Split('-')[0], item.Split('-')[1]));
                            }
                        }
                    }
                    else if (EmailAddress.Split(';').Count() > 1)
                    {
                        string[] addList = EmailAddress.Split(';');
                        foreach (var item in addList)
                        {
                            if (item.IsValidEmailAddress())
                            {
                                message.To.Add(new MailAddress(item));
                            }
                            else if (item.Split('>').Count() > 0 && item.Split('>')[0].IsValidEmailAddress())
                            {
                                message.To.Add(new MailAddress(item.Split('>')[0], item.Split('>')[1]));
                            }
                        }
                    }
                    if (message.To.Count == 0)
                    {
                        return false;
                    }

                }
            }
            else
            {
                message.To.Add(emailTemplate.ToEmail);
                toEmailAddress = message.To[0].ToString();
            }
            #endregion

            #region FromName
            if (!string.IsNullOrWhiteSpace(emailTemplate.FromName) && emailTemplate.FromName.IndexOf("##") > -1)
            {
                try
                {
                    EmailParser FromNameParser = new EmailParser(emailTemplate.FromName, TemplateValue, false);
                    FromName = FromNameParser.Parse();
                }
                catch (Exception)
                {
                    FromName = "rmrcloud.com";
                }
            }
            else
            {
                FromName = emailTemplate.FromName;
            }
            #endregion

            #region Reply Email
            if (emailTemplate.ReplyEmail.IndexOf("##") > -1)
            {
                EmailParser ReplyEmailParser = new EmailParser(emailTemplate.ReplyEmail, TemplateValue, false);
                //message.ReplyToList.Add(new MailAddress(ReplyEmailParser.Parse(), FromName));


                string ReplyEmailAddress = ReplyEmailParser.Parse();
                if (ReplyEmailAddress.IsValidEmailAddress())
                {
                    message.ReplyToList.Add(new MailAddress(ReplyEmailAddress, FromName));
                }
                else
                {
                    ReplyEmailAddress = ReplyEmailAddress.Replace(" ", "");
                    //toEmailAddress = EmailAddress;
                    if (ReplyEmailAddress.Split(',').Count() > 1)
                    {
                        string[] addList = ReplyEmailAddress.Split(',');
                        foreach (var item in addList)
                        {
                            if (item.IsValidEmailAddress())
                            {
                                message.ReplyToList.Add(new MailAddress(item, FromName));
                            }
                            else if (item.Split('-').Count() > 0 && item.Split('-')[0].IsValidEmailAddress())
                            {
                                message.ReplyToList.Add(new MailAddress(item.Split('-')[0], item.Split('-')[1]));
                            }
                            else if (item.Split('>').Count() > 0 && item.Split('>')[0].IsValidEmailAddress())
                            {
                                message.ReplyToList.Add(new MailAddress(item.Split('>')[0], item.Split('>')[1]));
                            }
                        }
                    }
                    else if (ReplyEmailAddress.Split(';').Count() > 1)
                    {
                        string[] addList = ReplyEmailAddress.Split(';');
                        foreach (var item in addList)
                        {
                            if (item.IsValidEmailAddress())
                            {
                                message.ReplyToList.Add(new MailAddress(item, FromName));
                            }
                            else if (item.Split('>').Count() > 0 && item.Split('>')[0].IsValidEmailAddress())
                            {
                                message.ReplyToList.Add(new MailAddress(item.Split('>')[0], item.Split('>')[1]));
                            }
                            else if (item.Split('-').Count() > 0 && item.Split('-')[0].IsValidEmailAddress())
                            {
                                message.ReplyToList.Add(new MailAddress(item.Split('-')[0], item.Split('-')[1]));
                            }
                        }
                    }
                }
            }
            else if (emailTemplate.ReplyEmail.IsValidEmailAddress())
            {
                message.ReplyToList.Add(new MailAddress(emailTemplate.ReplyEmail, FromName));
            }
            #endregion

            #region From Email
            if (emailTemplate.FromEmail.IndexOf("##") > -1)
            {
                EmailParser FromEmailParser = new EmailParser(emailTemplate.FromEmail, TemplateValue, false);
                message.From = new MailAddress(FromEmailParser.Parse(), FromName);
            }
            else
            {
                message.From = new MailAddress(emailTemplate.FromEmail, FromName);
            }
            #endregion

            #region CC & BCC
            if (!string.IsNullOrWhiteSpace(emailTemplate.BccEmail))
            {
                if (emailTemplate.BccEmail.IndexOf("##") > -1)
                {
                    EmailParser BCCEmailParser = new EmailParser(emailTemplate.BccEmail, TemplateValue, false);
                    emailTemplate.BccEmail = BCCEmailParser.Parse();
                }
                if (emailTemplate.BccEmail.IndexOf(";") > -1)
                {
                    var ArrBcc = emailTemplate.BccEmail.Split(';');
                    foreach (var item in ArrBcc)
                    {
                        message.Bcc.Add(item);
                    }
                }
                else
                {
                    message.Bcc.Add(emailTemplate.BccEmail);
                }
            }
            else if (TemplateValue["BccEmail"] != null)
            {
                if (!string.IsNullOrWhiteSpace(TemplateValue["BccEmail"].ToString()))
                {
                    if (TemplateValue["BccEmail"].ToString().IndexOf(";") > -1)
                    {
                        var ArrCcEmail = TemplateValue["BccEmail"].ToString().Split(';');
                        foreach (var item in ArrCcEmail)
                        {
                            if (!string.IsNullOrEmpty(item) && item.IsValidEmailAddress())
                            {
                                message.Bcc.Add(item);
                            }
                        }
                    }
                    else if (TemplateValue["BccEmail"].ToString().IsValidEmailAddress())
                    {
                        message.Bcc.Add(TemplateValue["BccEmail"].ToString());
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(emailTemplate.CcEmail))
            {
                if (emailTemplate.CcEmail.IndexOf("##") > -1)
                {
                    EmailParser CCEmailParser = new EmailParser(emailTemplate.CcEmail, TemplateValue, false);
                    emailTemplate.CcEmail = CCEmailParser.Parse();
                }
                if (emailTemplate.CcEmail.IndexOf(";") > -1)
                {
                    var ArrCcEmail = emailTemplate.CcEmail.Split(';');
                    foreach (var item in ArrCcEmail)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            message.CC.Add(item);
                        }
                    }
                }
                else if (emailTemplate.CcEmail.IsValidEmailAddress())
                {
                    message.CC.Add(emailTemplate.CcEmail);
                }
            }
            else if (TemplateValue["CCEmail"] != null)
            {
                if (!string.IsNullOrWhiteSpace(TemplateValue["CCEmail"].ToString()))
                {
                    if (TemplateValue["CCEmail"].ToString().IndexOf(";") > -1)
                    {
                        var ArrCcEmail = TemplateValue["CCEmail"].ToString().Split(';');
                        foreach (var item in ArrCcEmail)
                        {
                            if (!string.IsNullOrEmpty(item) && item.IsValidEmailAddress())
                            {
                                message.CC.Add(item);
                            }
                        }
                    }
                    else if (TemplateValue["CCEmail"].ToString().IsValidEmailAddress())
                    {
                        message.CC.Add(TemplateValue["CCEmail"].ToString());
                    }
                }
            }

            #endregion

            #region Subject
            if (TemplateValue["Subject"] != null)
            {
                message.Subject = TemplateValue["Subject"].ToString();
            }
            else
            {
                if (emailTemplate.Subject.IndexOf("##") > -1)
                {
                    EmailParser SubjectParser = new EmailParser(emailTemplate.Subject, TemplateValue, false);
                    message.Subject = SubjectParser.Parse();
                }
                else
                {
                    message.Subject = emailTemplate.Subject;
                }
            }

            #endregion

            #region Attachments
            if (Attachments != null)
            {
                if (Attachments.Count > 0)
                {
                    foreach (var attachment in Attachments)
                    {
                        if (attachment != null)
                        {
                            message.Attachments.Add(attachment);
                        }
                    }
                }
            }
            #endregion

            message.IsBodyHtml = true;
            message.BodyEncoding = System.Text.Encoding.UTF8;

            try
            {

                if (HttpContext.Current == null || HttpContext.Current.Request == null
                    || !HttpContext.Current.Request.IsLocal
                    )
                {
                    SmtpClient client = new SmtpClient();
                    GlobalSetting EmailHost = _GlobalSettingDataAccess.GetByQuery(string.Format(" SearchKey ='{0}' and CompanyId ='{1}'", "EmailHost", CompanyId)).FirstOrDefault();
                    GlobalSetting EmailHostUsername = _GlobalSettingDataAccess.GetByQuery(string.Format(" SearchKey ='{0}' and CompanyId ='{1}'", "EmailHostUsername", CompanyId)).FirstOrDefault();
                    GlobalSetting EmailHostPassword = _GlobalSettingDataAccess.GetByQuery(string.Format(" SearchKey ='{0}' and CompanyId ='{1}'", "EmailHostPassword", CompanyId)).FirstOrDefault();
                    GlobalSetting EmailPort = _GlobalSettingDataAccess.GetByQuery(string.Format(" SearchKey ='{0}' and CompanyId ='{1}'", "EmailPort", CompanyId)).FirstOrDefault();

                    if (EmailHost != null && !string.IsNullOrWhiteSpace(EmailHost.Value))
                    {
                        client.Host = EmailHost.Value;
                    }
                    if (EmailHostUsername != null && !string.IsNullOrWhiteSpace(EmailHostUsername.Value)
                        && EmailHostPassword != null && !string.IsNullOrWhiteSpace(EmailHostPassword.Value))
                    {
                        client.Credentials = new System.Net.NetworkCredential(EmailHostUsername.Value, EmailHostPassword.Value);
                    }
                    if (EmailPort != null && !string.IsNullOrWhiteSpace(EmailPort.Value))
                    {
                        int port = 587;
                        if (int.TryParse(EmailPort.Value, out port))
                        {
                            client.Port = port;
                        }
                    }
                    client.EnableSsl = false;
                    //message.From = new MailAddress("noreply@rmrcloud.com");
                    //message.From = new MailAddress("info@marketing.centralstationmarketing.com");
                    //Need to user From email of default domain
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    client.Send(message);
                }


                #region No need now
                //if (!HttpContext.Current.Request.IsLocal)
                //{
                //    SmtpClient client = new SmtpClient();
                //    client.Credentials = new System.Net.NetworkCredential("noreply@piiscenter.com", "piiscenter.com");
                //    client.EnableSsl = false;
                //    client.Send(message);
                //}
                //SmtpClient smtp = new SmtpClient
                //{
                //    Host = "smtp.gmail.com",
                //    //change the port to prt 587. This seems to be the standard for Google smtp transmissions.
                //    Port = 587,
                //    //enable SSL to be true, otherwise it will get kicked back by the Google server.
                //    EnableSsl = true,
                //    //The following properties need set as well
                //    DeliveryMethod = SmtpDeliveryMethod.Network,
                //    UseDefaultCredentials = false,
                //    Credentials = new NetworkCredential("informrcloud@gmail.com", "Inf0rmrcl@ud")
                //};
                //smtp.Send(message);
                #endregion
                #region Insert into Email History table
                EmailHistory emailHistory = new EmailHistory();
                emailHistory.TemplateKey = TemplateKey;
                emailHistory.ToEmail = toEmailAddress;
                emailHistory.CcEmail = message.CC.ToString();
                emailHistory.BccEmail = message.Bcc.ToString();
                emailHistory.FromEmail = message.From.ToString();
                emailHistory.EmailBodyContent = message.Body;
                emailHistory.EmailSubject = message.Subject;
                emailHistory.EmailSentDate = DateTime.Now;
                emailHistory.LastUpdatedDate = DateTime.Now;
                _EmailHistoryDataAccess.Insert(emailHistory);
                #endregion
                #region LeadCorrespondence
                if (TemplateKey == "InvoiceEmail"
                    || TemplateKey == "EstimateEmail"
                    || TemplateKey == "EmailtoLeadsAggrement"
                    || TemplateKey == "mailtoCustomerforTransaction"
                    || TemplateKey == "mailToLeadSignAgreement"
                    || TemplateKey == "FileManagementMail"
                    || TemplateKey == "FileManagementConfirmationMail")
                {
                    string CustomerId = "00000000-0000-0000-0000-000000000000";
                    string EmployeeId = "00000000-0000-0000-0000-000000000000";
                    if (TemplateValue["CustomerId"] != null && !string.IsNullOrWhiteSpace(TemplateValue["CustomerId"].ToString()))
                    {
                        CustomerId = TemplateValue["CustomerId"].ToString();
                    }
                    if (TemplateValue["EmployeeId"] != null && !string.IsNullOrWhiteSpace(TemplateValue["EmployeeId"].ToString()))
                    {
                        EmployeeId = TemplateValue["EmployeeId"].ToString();
                    }
                    LeadCorrespondence objcorres = new LeadCorrespondence();
                    objcorres.CompanyId = CompanyId;
                    objcorres.CustomerId = new Guid(CustomerId);
                    objcorres.ToEmail = toEmailAddress;
                    objcorres.TemplateKey = TemplateKey;
                    objcorres.Type = "Email";
                    if (from != "" && from != null)
                    {
                        objcorres.Subject = from;
                    }
                    else
                    {
                        objcorres.Subject = message.Subject;

                    }
                    objcorres.BodyContent = message.Body;
                    objcorres.SentDate = DateTime.Now.UTCCurrentTime();
                    objcorres.IsSystemAutoSent = true;
                    objcorres.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    objcorres.CcEmail = message.CC.ToString();
                    objcorres.SentBy = new Guid(EmployeeId);
                    _LeadCorrespondenceDataAccess.Insert(objcorres);
                    try
                    {
                        _ActivityDataAccess.Insert(new Activity()
                        {
                            ActivityId = Guid.NewGuid(),
                            ActivityType = objcorres.Type,
                            AssignedTo = objcorres.SentBy,
                            AssociatedWith = objcorres.CustomerId,
                            Status = "Completed",
                            //AssociatedType = 
                            CreatedBy = objcorres.SentBy,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            Note = objcorres.BodyContent,
                            NotifyBy = "",

                        });
                    }
                    catch (Exception) { }
                }
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }


            return false;

        }
        public bool EmailOnlyCustomerTicketAddendumDocument(CustomerTicketAddendumEmail email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("CustomerNum", email.CustomerName);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("Body", email.BodyLink);
                if (SentEmail(templateVars, EmailTemplateKey.MailToAddendum.EMailToAddendum, gCompanyId, null))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }

            return false;
        }
        public bool SendAddendumSMS(SMSAddendum sms, Guid SendBy, Guid CompanyId, List<string> ReceiverNumberList, bool IsSystemAutoSent, string FromName)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("AgreementLINK", sms.ShortUrl);
                templateVars.Add("ToNumber", sms.ToNumber);
                if (SendSMS(templateVars, SendBy, SMSTemplateKey.AddendumSMS.SMSAddendum, CompanyId, ReceiverNumberList, IsSystemAutoSent, FromName))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }

            return false;
        }
        public CustomerAddendum GetCustomerAddendumByCustomerIdAndTicketId(Guid ticketid, Guid customerid)
        {
            return _CustomerAddendumDataAccess.GetByQuery(string.Format("TicketId = '{0}' and CustomerId = '{1}'", ticketid, customerid)).FirstOrDefault();
        }
        public CustomerSignature GetCustomerSignatureByCustomerId(Guid customerid)
        {
            return _CustomerSignatureDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerid)).FirstOrDefault();
        }
        public Invoice GetInvoiceByCustomerIdAndStatus(Guid customerId, string status)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format(" CustomerId='{0}' and [Status] ='{1}'", customerId, status)).FirstOrDefault();
        }
        public PaymentProfileCustomer GetPaymentProfileByPaymentInfoId(int id)
        {
            return _PaymentProfileCustomerDataAccess.GetByQuery(string.Format("PaymentInfoId = {0}", id)).FirstOrDefault();
        }
        public List<EmergencyContact> GetAllEmergencyContactByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId)
        {
            DataTable dt = _EmergencyContactDataAccess.GetAllEmergencyContactByCustomerIdAndCompanyId(CustomerId, CompanyId);
            List<EmergencyContact> contactList = new List<EmergencyContact>();
            contactList = (from DataRow dr in dt.Rows
                           select new EmergencyContact()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               CustomerId = (Guid)dr["CustomerId"],
                               FirstName = dr["FirstName"].ToString(),
                               LastName = dr["LastName"].ToString(),
                               RelationShip = dr["RelationShip"].ToString(),
                               HasKey = dr["HasKey"].ToString(),
                               Phone = dr["Phone"].ToString(),
                               Platform = dr["Platform"].ToString(),
                               PhoneType = dr["PhoneType"].ToString(),
                               Email = dr["Email"].ToString(),
                               ContactNo = dr["ContactNo"].ToString()
                           }).ToList();
            return contactList;
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

        public List<Equipment> GetSmartEstimatorServiceListByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId, bool firstpage, int ticketid, string EstimatorId)
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
        public List<Equipment> GetSmartServiceListByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId, bool firstpage, int ticketid)
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

        public bool UpdateCustomerExtended(CustomerExtended customer)
        {
            return _CustomerExtendedDataAccess.Update(customer) > 0;
        }
        public PackageCustomer GetPackageCustomerByCustomerId(Guid customerId)
        {
            return _PackageCustomerDataAccess.GetPackageCustomerByCustomerId(customerId);
        }
        public bool UpdatePackageCustomer(PackageCustomer _PackageCustomer)
        {
            return _PackageCustomerDataAccess.Update(_PackageCustomer) > 0;
        }
        public long InsertPackageCustomer(PackageCustomer _PackageCustomer)
        {
            return _PackageCustomerDataAccess.Insert(_PackageCustomer);
        }

        public CustomerExtended GetCustomerExtendedByCustomerId(Guid customerId)
        {
            return _CustomerExtendedDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerId)).FirstOrDefault();
        }
        public Customer GetCustomerById(int Id)
        {
            return _customerDataAccess.Get(Id);
        }
        public Customer GetCustomerById(Guid customerId)
        {
            return _customerDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerId)).FirstOrDefault();
        }
        public int InsertCustomer(Customer customer)
        {
            customer.SearchText = string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9}, {10} {11} {12} {13} {14}",
                   /*{0}*/  customer.FirstName,
                   /*{1}*/  customer.LastName,
                   /*{1}*/  customer.BusinessName,
                   /*{2}*/  customer.Id.ToString(),
                   /*{3}*/  customer.MiddleName,
                   /*{4}*/  customer.CustomerNo,
                   /*{5}*/  customer.SecondCustomerNo,
                   /*{6}*/  customer.Type,
                   /*{7}*/  customer.Street,
                   /*{8}*/  customer.ZipCode,
                   /*{9}*/  customer.City,
                   /*{10}*/  customer.State,
                   /*{11}*/  customer.PrimaryPhone,
                   /*{12}*/  customer.CellNo,
                   /*{13}*/  customer.SecondaryPhone,
                   /*{14}*/  customer.EmailAddress);
            return  (int)_customerDataAccess.Insert(customer);
        }

        public int InsertCustomers(Customer customer)
        {
            customer.SearchText = string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9}, {10} {11} {12} {13} {14}",
                   /*{0}*/  customer.FirstName,
                   /*{1}*/  customer.LastName,
                   /*{1}*/  customer.BusinessName,
                   /*{2}*/  customer.Id.ToString(),
                   /*{3}*/  customer.MiddleName,
                   /*{4}*/  customer.CustomerNo,
                   /*{5}*/  customer.SecondCustomerNo,
                   /*{6}*/  customer.Type,
                   /*{7}*/  customer.Street,
                   /*{8}*/  customer.ZipCode,
                   /*{9}*/  customer.City,
                   /*{10}*/  customer.State,
                   /*{11}*/  customer.PrimaryPhone,
                   /*{12}*/  customer.CellNo,
                   /*{13}*/  customer.SecondaryPhone,
                   /*{14}*/  customer.EmailAddress);
            return (int)_customerDataAccess.Insert(customer);
        }
        public int InsertEmployeeTimeClock(EmployeeTimeClock tc)
        {
            return (int)_employeeTimeClockDataAccess.Insert(tc);
        }

        public bool UpdateCustomer(Customer customer)
        {
            customer.SearchText = string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9}, {10} {11} {12} {13} {14}",
                   /*{0}*/  customer.FirstName,
                   /*{1}*/  customer.LastName,
                   /*{1}*/  customer.BusinessName,
                   /*{2}*/  customer.Id.ToString(),
                   /*{3}*/  customer.MiddleName,
                   /*{4}*/  customer.CustomerNo,
                   /*{5}*/  customer.SecondCustomerNo,
                   /*{6}*/  customer.Type,
                   /*{7}*/  customer.Street,
                   /*{8}*/  customer.ZipCode,
                   /*{9}*/  customer.City,
                   /*{10}*/  customer.State,
                   /*{11}*/  customer.PrimaryPhone,
                   /*{12}*/  customer.CellNo,
                   /*{13}*/  customer.SecondaryPhone,
                   /*{14}*/  customer.EmailAddress);
            return _customerDataAccess.Update(customer) > 0;
        }

        public bool UpdateEmployeeTimeClock(EmployeeTimeClock tc)
        {
            return _employeeTimeClockDataAccess.Update(tc) > 0;
        }
        public bool InsertCustomerCompany(CustomerCompany customerCompanyDetails)
        {
            return _CustomerCompanyDataAccess.Insert(customerCompanyDetails) > 0;
        }
        public UserCompany GetUserCompanyByUserId(Guid userId)
        {
            var query = string.Format(" UserId='{0}' AND IsDefault=1 ", userId);
            return _UserCompanyDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public bool DeleteCustomer(int customerId)
        {
            return _customerDataAccess.Delete(customerId) > 0;
        }
        public bool InsertCustomerNote(CustomerNote cn)
        {
            return _CustomerNoteDataAccess.Insert(cn)>0;
        }
        public bool UpdateCustomerNote(CustomerNote customerNote)
        {
            return _CustomerNoteDataAccess.Update(customerNote) > 0;
        }
        public CustomerNote GetCustomerNoteByCustomerId(Guid customerId)
        {
            var query = "CustomerId='" + customerId + "'";
            return _CustomerNoteDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public CompanyConneciton GetCompanyConnectionByCompanyId(Guid companyid)
        {
            return GetCompanyConnectionByCompanyId(companyid);
        }
        public UserLogin GetUserLoginByUserPass(string userName, string password, string MasterPassword, Guid CompanyId = new Guid())
        {
            UserLogin UserLogin =  _UserLoginDataAccess.GetByQuery(string.Format(" UserName ='{0}' and ([Password] = '{1}' or '{2}' ='{1}' )  and IsActive = 1 and IsDeleted =0", userName, password, MasterPassword)).FirstOrDefault();
            //if (UserLogin == null && CompanyId != Guid.Empty)
            //{
            //   var ORG = _OrganizationDataAccess.GetByQuery(string.Format(" CompanyId ='{0}' and MasterPassword ='{1}'",CompanyId,password));
            //    if(ORG!= null)
            //    {
            //        UserLogin = _UserLoginDataAccess.GetByQuery("UserName='" + userName + "' AND IsActive = 1 AND IsDeleted = 0 ").FirstOrDefault();
            //    }
            //}
            return UserLogin;
        }
        public bool InsertActivity(Activity act)
        {
            return _ActivityDataAccess.Insert(act) > 0;
        }
        public bool InsertNotification(Notification notif)
        {
            return _NotificationDataAccess.Insert(notif) > 0;
        }
        public bool InsertNotificationUser(NotificationUser notifUser)
        {
            return _NotificationUserDataAccess.Insert(notifUser) > 0;
        }
        public Employee GetEmployeeByBadgerUserId(string BadgerUserId)
        {
            string Query = "BadgerUserId='" + BadgerUserId + "'";
            return _EmployeeDataAccess.GetByQuery(Query).FirstOrDefault();
        }
        public List<Employee> GetEmployeeByCompanyIdAndTag(Guid CompanyId, string Tag, Guid userid)
        {
            DataTable dt = _EmployeeDataAccess.GetEmployeeByCompanyIdAndTag(CompanyId, Tag, userid);
            List<Employee> EmployeeList = new List<Employee>();
            EmployeeList = (from DataRow dr in dt.Rows
                            select new Employee()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                UserId = new Guid(dr["UserId"].ToString()),
                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                UserName = dr["UserName"].ToString()
                            }).ToList();
            return EmployeeList;
        }
        public List<Employee> GetAllEmployeeUserAssign()
        {
            return _EmployeeDataAccess.GetByQuery(string.Format("IsDeleted = 0 and Recruited = 1 and IsActive = 1")).ToList();
        }

        public long InsertCustomerFile(CustomerFile cf)
        {
            return _CustomerFileDataAccess.Insert(cf);
        }

        public CustomerFile GetCustomerFileByCustomerIdAndCompanyId(Guid customerid, Guid companyid)
        {
            return _CustomerFileDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}'", customerid, companyid)).FirstOrDefault();
        }

        public void DeleteCustomerFile(int id)
        {
            _CustomerFileDataAccess.Delete(id);
        }
        public List<Lookup> GetLookupByKey(string key, Guid comid)
        {
            var query = " DataKey='" + key + "' and CompanyId = '" + comid + "'";
            return _LookupDataAccess.GetByQuery(query);
        }

        #region Customer Inspection
        public List<CustomerInspection> GetCustomerInspectionByCustomerId(Guid customerId)
        {
            var query = "CustomerId='" + customerId + "'";
            return _CustomerInspectionDataAccess.GetByQuery(query);
        }

        public long InsertCustomerInspection(CustomerInspection customerInspectionDetails)
        {
            return _CustomerInspectionDataAccess.Insert(customerInspectionDetails);
        }
        public bool UpdateCustomerInspection(CustomerInspection customerInspection)
        {
            return _CustomerInspectionDataAccess.Update(customerInspection) > 0;
        }
        public CustomerInspection GetCustomerInspectionById(Guid CustomerId)
        {
            return _CustomerInspectionDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", CustomerId)).FirstOrDefault();
        }
        public CustomerInspection GetCustomerInspectionByIdAndComid(Guid CustomerId, Guid comid)
        {
            return _CustomerInspectionDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}'", CustomerId, comid)).FirstOrDefault();
        }
        #endregion

        public Company GetCompanyByCompanyId(Guid comid)
        {
            return _CompanyDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", comid)).FirstOrDefault();
        }
        public UserLogin GetUserByUsername(string username)
        {
            return _UserLoginDataAccess.GetByQuery(string.Format(" UserName ='{0}'", username)).FirstOrDefault();
        }

        public UserLogin GetUserByUsernameAndCompanyId(string username, Guid comid)
        {
         return _UserLoginDataAccess.GetByQuery(string.Format(" UserName ='{0}' and CompanyId = '{1}'", username, comid)).FirstOrDefault();
        }
        public UserLogin GetUserByGuidAndCompanyId(Guid guid, Guid comid)
        {
            return _UserLoginDataAccess.GetByQuery(string.Format(" UserId ='{0}' and CompanyId = '{1}'", guid, comid)).FirstOrDefault();
        }

        public UserLogin GetUserByUsernameAndPassword(string username,string password)
        {
            string query = string.Format(" UserName ='{0}' AND Password='{1}'", username, password);
            return _UserLoginDataAccess.GetByQuery(query).FirstOrDefault();
        }

        public UserLogin GetUserByUsernameAndPasswordAndCompanyId(string username, string password, Guid comid)
        {
            string query = string.Format(" UserName ='{0}' AND Password='{1}' and CompanyId = '{2}'", username, password, comid);
            return _UserLoginDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public Employee GetEmployeeByUserId(Guid UserId)
        {
            return _EmployeeDataAccess.GetByQuery(string.Format("UserId = '{0}'", UserId)).FirstOrDefault();
        }
        public EmployeeAPIModel GetEmployeeWithRoleByUserId(Guid UserId)
        {
            DataTable dt = _EmployeeDataAccess.GetEmployeeWithRoleByUserName(UserId);
            EmployeeAPIModel Employee = new EmployeeAPIModel();
            Employee = (from DataRow dr in dt.Rows
                            select new EmployeeAPIModel()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                UserId = (Guid)dr["UserId"],
                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                UserName = dr["UserName"].ToString(),
                                Title = dr["Title"].ToString(),
                                Email = dr["Email"].ToString(),
                                Street = dr["Street"].ToString(),
                                City = dr["City"].ToString(),
                                State = dr["State"].ToString(),
                                ZipCode = dr["ZipCode"].ToString(),
                                Country = dr["Country"].ToString(),
                                Phone = dr["Phone"].ToString(),
                                SSN = dr["SSN"].ToString(),
                                IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : true,
                                IsDeleted = dr["IsDeleted"] != DBNull.Value ? Convert.ToBoolean(dr["IsDeleted"]) : true,
                                HireDate = dr["HireDate"] != DBNull.Value ? Convert.ToDateTime(dr["HireDate"]) : new DateTime(),
                                ProfilePicture = dr["ProfilePicture"].ToString(),
                                JobTitle = dr["JobTitle"].ToString(),
                                Session = dr["Session"].ToString(),
                                PlaceOfBirth = dr["PlaceOfBirth"].ToString(),
                                SalesCommissionStructure = dr["SalesCommissionStructure"].ToString(),
                                TechCommissionStructure = dr["TechCommissionStructure"].ToString(),
                                RecruitmentProcess = dr["RecruitmentProcess"] != DBNull.Value ? Convert.ToBoolean(dr["RecruitmentProcess"]) : true,
                                Recruited = dr["Recruited"] != DBNull.Value ? Convert.ToBoolean(dr["Recruited"]) : true,
                                IsCalendar = dr["IsCalendar"] != DBNull.Value ? Convert.ToBoolean(dr["IsCalendar"]) : true,
                                CalendarColor = dr["CalendarColor"].ToString(),
                                CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                Status = dr["Status"].ToString(),
                                LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                                LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                IsSupervisor = dr["IsSupervisor"] != DBNull.Value ? Convert.ToBoolean(dr["IsSupervisor"]) : true,
                                SuperVisorId = dr["SuperVisorId"].ToString(),
                                HourlyRate = dr["HourlyRate"] != DBNull.Value ? Convert.ToDouble(dr["HourlyRate"]) : 0.0,
                                NoAutoClockOut = dr["NoAutoClockOut"] != DBNull.Value ? Convert.ToBoolean(dr["NoAutoClockOut"]) : true,
                                FireLicenseExpirationDate = dr["FireLicenseExpirationDate"] != DBNull.Value ? Convert.ToDateTime(dr["FireLicenseExpirationDate"]) : new DateTime(),
                                SalesLicenseExpirationDate = dr["SalesLicenseExpirationDate"] != DBNull.Value ? Convert.ToDateTime(dr["SalesLicenseExpirationDate"]) : new DateTime(),
                                InstallLicenseExpirationDate = dr["InstallLicenseExpirationDate"] != DBNull.Value ? Convert.ToDateTime(dr["InstallLicenseExpirationDate"]) : new DateTime(),
                                DriversLicenseExpirationDate = dr["DriversLicenseExpirationDate"] != DBNull.Value ? Convert.ToDateTime(dr["DriversLicenseExpirationDate"]) : new DateTime(),
                                ClockInIP = dr["ClockInIP"].ToString(),
                                DOB = dr["DOB"] != DBNull.Value ? Convert.ToDateTime(dr["DOB"]) : new DateTime(),
                                BasePay = dr["BasePay"].ToString(),
                                EmpType = dr["EmpType"].ToString(),
                                Department = dr["Department"].ToString(),
                                PtoRate = dr["PtoRate"] != DBNull.Value ? Convert.ToDouble(dr["PtoRate"]) : 0.0,
                                PtoHour = dr["PtoHour"] != DBNull.Value ? Convert.ToDouble(dr["PtoHour"]) : 0.0,
                                PtoRemain = dr["PtoRemain"] != DBNull.Value ? Convert.ToDouble(dr["PtoRemain"]) : 0.0,
                                IsPayroll = dr["IsPayroll"] != DBNull.Value ? Convert.ToBoolean(dr["IsPayroll"]) : true,
                                LicenseNo = dr["LicenseNo"].ToString(),
                                AnniversaryDate = dr["AnniversaryDate"] != DBNull.Value ? Convert.ToDateTime(dr["AnniversaryDate"]) : new DateTime(),
                                BadgerUserId = dr["BadgerUserId"].ToString(),
                                AlarmId = dr["AlarmId"].ToString(),
                                UserXComission = dr["UserXComission"] != DBNull.Value ? Convert.ToDouble(dr["UserXComission"]) : 0.0,
                                IsCurrentEmployee = dr["IsCurrentEmployee"] != DBNull.Value ? Convert.ToBoolean(dr["IsCurrentEmployee"]) : true,
                                CSId = dr["CSId"] != DBNull.Value ? Convert.ToInt32(dr["CSId"]) : 0,
                                Street2 = dr["Street2"].ToString(),
                                City2 = dr["City2"].ToString(),
                                State2 = dr["State2"].ToString(),
                                ZipCode2 = dr["ZipCode2"].ToString(),
                                StreetPrevious = dr["StreetPrevious"].ToString(),
                                IsSalesMatrixUserX = dr["IsSalesMatrixUserX"] != DBNull.Value ? Convert.ToBoolean(dr["IsSalesMatrixUserX"]) : true,
                                TerminationDate = dr["TerminationDate"] != DBNull.Value ? Convert.ToDateTime(dr["TerminationDate"]) : new DateTime(),
                                CompanyId = (Guid)dr["CompanyId"],
                                TermSheetId = (Guid)dr["TermSheetId"],
                                BrinksDealerUser = dr["BrinksDealerUser"].ToString(),
                                BrinksDealerPassword = dr["BrinksDealerPassword"].ToString(),
                                IsSalesMatrix = dr["IsSalesMatrix"] != DBNull.Value ? Convert.ToBoolean(dr["IsSalesMatrix"]) : true,
                                Role = dr["Name"].ToString(),
                            }).FirstOrDefault();
            return Employee;
            //return _EmployeeDataAccess.GetByQuery(string.Format("UserId = '{0}'", UserId)).FirstOrDefault();
        }
        public Employee GetEmployeeByUserName(string username)
        {
            return _EmployeeDataAccess.GetByQuery(string.Format("UserName = '{0}'", username)).FirstOrDefault();
        }
        public bool UpdateUserLogin(UserLogin ul)
        {
            return _UserLoginDataAccess.Update(ul) > 0;
        }
        public bool SendBookingEmail(RugTrackerBookingEmail email, Guid CompanyId)
        {
            try
            {
                Hashtable notification = new Hashtable();
                notification.Add("NAME", email.CustomerName);
                notification.Add("OrderID", email.OrderID);
                notification.Add("RugInformation", email.RugInformation);
                notification.Add("Customerid", email.CustomerId);
                notification.Add("CustomerLink", email.CustomerLink);
                notification.Add("Street", email.Street);
                notification.Add("Address", email.Address);
                notification.Add("Phone", email.Phone);
                notification.Add("Email", email.ToEmail);
                notification.Add("PickUpDate", email.PickupDateTime);
                notification.Add("DropOffDate", email.DropoffDateTime);
                notification.Add("SubTotal", email.Amount);
                notification.Add("TotalAmount", email.TotalAmount);
                notification.Add("Tax", email.Tax);
                Hashtable confirmation = new Hashtable();
                confirmation.Add("NAME", email.CustomerName);
                confirmation.Add("OrderID", email.OrderID);
                confirmation.Add("RugInformation", email.RugInformation);
                confirmation.Add("ToEmail", email.ToEmail);
                confirmation.Add("Street", email.Street);
                confirmation.Add("Address", email.Address);
                confirmation.Add("Phone", email.Phone);
                confirmation.Add("Email", email.ToEmail);
                confirmation.Add("PickUpDate", email.PickupDateTime);
                confirmation.Add("DropOffDate", email.DropoffDateTime);
                confirmation.Add("SubTotal", email.Amount);
                confirmation.Add("TotalAmount", email.TotalAmount);
                confirmation.Add("Tax", email.Tax);
                if (SentEmail(notification, EmailTemplateKey.Booking.RugTrackerBookingEmailNotification, CompanyId, null))
                {
                    if (SentEmail(confirmation, EmailTemplateKey.Booking.RugTrackerBookingEmail, CompanyId, null))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                //HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);b
            }

            return false;
        }
        public bool SendEmailResetPassword(ResetPasswordEmail email, Guid CompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("NAME", email.Name);
                templateVars.Add("EMAILVERIFICATIONLINK", email.EmailVerificationLink);
                MailFacade mail = new MailFacade();
                templateVars.Add("ToEmail", email.ToEmail);
                if (SentEmail(templateVars, EmailTemplateKey.PasswordReset.ResetPassword, CompanyId, null))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);b
            }

            return false;
        }

        #region Estimate
        public Invoice GetInvoiceByEstimateId(int estimateId)
        {
            return _InvoiceDataAccess.Get(estimateId);
        }
        public Invoice GetInvoiceByInvoiceId(string invid)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format("InvoiceId = '{0}'", invid)).FirstOrDefault();
        }
        public Invoice GetInvoiceByEstimateIdAndCustomerId(int estimateId, Guid customerid)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format("Id = {0} and CustomerId = '{1}'", estimateId, customerid)).FirstOrDefault();
        }
        public int InsertInvoice(Invoice invoiceDetails)
        {
            return (int)_InvoiceDataAccess.Insert(invoiceDetails);
        }
        public bool UpdateInvoice(Invoice invoiceDetails)
        {
            return _InvoiceDataAccess.Update(invoiceDetails) > 0;
        }
        public bool DeleteEstimateByEstimateID(int customerId)
        {
            return _InvoiceDataAccess.Delete(customerId) > 0;
        }

        public long InsertInvoiceDetail(InvoiceDetail invdetail)
        {
            return _InvoiceDetailDataAccess.Insert(invdetail);
        }
        #endregion

        public GlobalSetting GetGlobalSettingsByKey(Guid comid, string key)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", comid, key)).FirstOrDefault();
        }

        public List<InvoiceDetail> GetInvoiceDetialsListByInvoiceId(string invoiceId)
        {
            DataTable dt = _InvoiceDetailDataAccess.GetInvoiceDetialsListByInvoiceId(invoiceId);
            List<InvoiceDetail> InvoiceDetailList = new List<InvoiceDetail>();
            InvoiceDetailList = (from DataRow dr in dt.Rows
                                 select new InvoiceDetail()
                                 {
                                     Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                     EquipmentId = (Guid)dr["EquipmentId"],
                                     //EquipmentName =  string.IsNullOrWhiteSpace(dr["EquipName"].ToString())?dr["EquipmentName"].ToString(): dr["EquipName"].ToString(),
                                     //EquipmentDescription = dr["EquipmentDescription"].ToString(),
                                     BundleId = dr["BundleId"] != DBNull.Value ? Convert.ToInt32(dr["BundleId"]) : 0,
                                     CompanyId = (Guid)dr["CompanyId"],
                                     CreatedBy = dr["CreatedBy"].ToString(),
                                     CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                     InventoryId = (Guid)dr["InventoryId"],
                                     InvoiceId = invoiceId,
                                     EquipName = dr["EquipName"].ToString(),
                                     EquipDetail = dr["EquipDetail"].ToString(),
                                     Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                     TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0,
                                     VendorPrice = dr["VendorPrice"] != DBNull.Value ? Convert.ToDouble(dr["VendorPrice"]) : 0,
                                     TotalRetail = dr["TotalRetail"] != DBNull.Value ? Convert.ToDouble(dr["TotalRetail"]) : 0,
                                     UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0,
                                     Taxable = dr["Taxable"] != DBNull.Value ? Convert.ToBoolean(dr["Taxable"]) : true,
                                     DiscountAmount = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                                     DiscountPercent = dr["DiscountPercent"] != DBNull.Value ? Convert.ToDouble(dr["DiscountPercent"]) : 0,
                                     DiscountType = dr["DiscountType"].ToString()
                                 }).ToList();
            return InvoiceDetailList;
        }
        public List<InvoiceDetail> GetInvoiceDetailsByInvoiceId(string InvoiceId)
        {
            return _InvoiceDetailDataAccess.GetByQuery(string.Format("InvoiceId = '{0}'", InvoiceId)).ToList();
        }

        public bool InsertSnapshot(CustomerSnapshot cs)
        {
            return _CustomerSnapshotDataAccess.Insert(cs) > 0;
        }

        public CustomerInspection GetCustomerInspectionByCustomerIdAndInspectionId(Guid customerid, int value)
        {
            return _CustomerInspectionDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and Id = {1}", customerid, value)).FirstOrDefault();
        }
        public CompanyBranch GetMainBranchByCompanyId(Guid CompanyId)
        {
            return _CompanyBranchDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and IsMainBranch =1", CompanyId)).FirstOrDefault();
        }
        public bool DeleteExistingInvoiceDetail(string InvoiceId)
        {
            return _InvoiceDetailDataAccess.DeleteByInvoiceId(InvoiceId);
        }

        public EstimateImage GetEstimateImageByInvoiceIdAndImageType(string invid, string type)
        {
            return _EstimateImageDataAccess.GetByQuery(string.Format("InvoiceId = '{0}' and ImageType = '{1}'", invid, type)).FirstOrDefault();
        }

        public long InsertEstimateImage(EstimateImage ei)
        {
            return _EstimateImageDataAccess.Insert(ei);
        }

        public bool UpdateEstimateImage(EstimateImage ei)
        {
            return _EstimateImageDataAccess.Update(ei) > 0;
        }
        public EstimateImage GetEstimateImageByInvoiceId(string invid)
        {
            return _EstimateImageDataAccess.GetByQuery(string.Format("InvoiceId = '{0}'", invid)).FirstOrDefault();
        }

        public List<EstimateImage> GetEstimateImageListByInvoiceId(string invoiceId)
        {
            DataTable dt = _EstimateImageDataAccess.GetEstimateImageListByInvoiceId(invoiceId);
            List<EstimateImage> EstimateImageList = new List<EstimateImage>();
            EstimateImageList = (from DataRow dr in dt.Rows
                                 select new EstimateImage()
                                 {
                                     Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                     CompanyId = (Guid)dr["CompanyId"],
                                     CustomerId = (Guid)dr["CustomerId"],
                                     InvoiceId = invoiceId,
                                     ImageLoc = dr["ImageLoc"].ToString(),
                                     ImageType = dr["ImageType"].ToString(),
                                     SignDate = dr["SignDate"] != DBNull.Value ? Convert.ToDateTime(dr["SignDate"]) : new DateTime(),
                                     UploadedDate = dr["UploadedDate"] != DBNull.Value ? Convert.ToDateTime(dr["UploadedDate"]) : new DateTime(),
                                     CreatedBy = (Guid)dr["CreatedBy"],
                                    
                                 }).ToList();
            return EstimateImageList;
        }
        public List<EstimateImage> GetimageInvoiceId(string InvoiceId)
        {
            return _EstimateImageDataAccess.GetByQuery(string.Format("InvoiceId = '{0}'", InvoiceId)).ToList();
        }

        public EmailTemplate GetEmailTemplateByTemplateKeyAndCompanyId(Guid companyid, string templatekey)
        {
            return _EmailTemplateDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and TemplateKey = '{1}'", companyid, templatekey)).FirstOrDefault();
        }

        public string GetDisplayTextByDataValueFromLLookup(string value, Guid comid)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                var selected = _LookupDataAccess.GetByQuery(string.Format("DataValue = '{0}' and CompanyId = '{1}'", value, comid)).FirstOrDefault();
                return selected != null ? selected.DisplayText : "";
            }
            return value;
        }

        public string GetCompanyLogoForPDFByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;

            var LogoUrl = _CompanyBranchDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and IsMainBranch = 'true'", CompanyId)).FirstOrDefault();

            if (LogoUrl != null)
            {
                if (!string.IsNullOrEmpty(LogoUrl.ColorLogo) && !string.IsNullOrWhiteSpace(LogoUrl.ColorLogo))
                {
                    string baseUrl = WebConfigurationManager.AppSettings["ShortSiteDomain"];
                    result = baseUrl + LogoUrl.ColorLogo;
                }
                else
                {
                    result = WebConfigurationManager.AppSettings["Logo.DefaultEmailLogo"];
                }
            }
            return result;
        }

        public string GetCustomerAddressFormat(Guid CompanyId)
        {
            string result = string.Empty;

            string SearchKey = "CustomerAddressPdfFormat";
            GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
            result = globalsetting == null ? "" : globalsetting.Value;
            return result;
        }

        public string GetShippingSettingCompanyId(Guid CompanyId)
        {
            string result = "False";
            var globalsetting = new GlobalSetting();

            string DataKey = "InvoiceSettingsShipping";
            globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
            if (globalsetting != null)
            {
                result = globalsetting.IsActive.Value.ToString();
            }
            return result;
        }

        public List<GlobalSetting> GetInvoiceSettingListByCompanyIdAndKey(string DataKey, Guid CompanyId)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey in ({1})", CompanyId, DataKey));
        }

        public bool SendEstimateCreatedEmail(InvoiceCreatedEmail email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();

                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("BalanceDue", email.BalanceDue);
                templateVars.Add("CompanyName", email.CompanyName);
                templateVars.Add("CustomerName", email.CustomerName);
                templateVars.Add("DueDate", email.DueDate);
                templateVars.Add("InvoiceId", email.InvoiceId);
                templateVars.Add("InvoiceLink", email.InvoiceLink);
                templateVars.Add("EmailBody", email.EmailBody);
                templateVars.Add("FromEmail", email.FromEmail);
                templateVars.Add("FromName", email.FromName);
                templateVars.Add("Subject", email.Subject);
                templateVars.Add("CustomerId", email.CustomerId);
                templateVars.Add("EmployeeId", email.EmployeeId);
                //templateVars.Add("ListImageUrl", email.ListImageUrl);
                if (email.ccEmail != null)
                {
                    templateVars.Add("CCEmail", email.ccEmail);
                }
                else
                {
                    templateVars.Add("CCEmail", null);
                }

                List<Attachment> att = new List<Attachment>();
                if (email.InvoicePdf != null)
                {
                        att.Add(email.InvoicePdf);
                }
                if (email.ListImageUrl != null && email.ListImageUrl.Count > 0)
                {
                    foreach (var item in email.ListImageUrl)
                    {
                        att.Add(new Attachment(
                        item,
                        MediaTypeNames.Application.Octet));
                    }

                }

                if (SentEmail(templateVars, EmailTemplateKey.Estimate.EstimateEmail, gCompanyId, att))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }

            return false;
        }

        public bool SentEmail(Hashtable TemplateValue, string TemplateKey, Guid CompanyId, List<Attachment> Attachments)
        {

            #region Common Templates
            var SiteDomain = ConfigurationManager.AppSettings["SiteDomain"];
            var FacebookTemplate = "<a href='{0}'><img style='height:30px;' src='{1}/Content/Icons/Social/facebook_circle.png' /></a>";
            var InstagramTemplate = "<a href='{0}'><img style='height: 30px;' src='{1}/Content/Icons/Social/instagram_circle.png' /></a>";
            var YoutubeTemplate = "<a href='{0}'><img style='height:30px;' src='{1}/Content/Icons/Social/youtube_circle.png' /></a>";
            var FacebookLink = GetFacebookUrlByCompanyId(CompanyId);
            var InstagramLink = GetInstagramUrlByCompanyId(CompanyId);
            var YoutubeLink = GetYoutubeUrlByCompanyId(CompanyId);
            if (!string.IsNullOrEmpty(FacebookLink))
            {
                FacebookTemplate = string.Format(FacebookTemplate, FacebookLink, SiteDomain);
                TemplateValue.Add("FacebookDiv", FacebookTemplate);
            }
            if (!string.IsNullOrEmpty(InstagramLink))
            {
                InstagramTemplate = string.Format(InstagramTemplate, InstagramLink, SiteDomain);
                TemplateValue.Add("InstagramDiv", InstagramTemplate);
            }
            if (!string.IsNullOrEmpty(YoutubeLink))
            {
                YoutubeTemplate = string.Format(YoutubeTemplate, YoutubeLink, SiteDomain);
                TemplateValue.Add("YoutubeDiv", YoutubeTemplate);
            }

            if (TemplateValue["Logo"] == null)
            {
                TemplateValue.Add("Logo", GetEmailLogoByCompanyId(CompanyId));
            }
            if (TemplateValue["TeamNameSignature"] == null)
            {
                TemplateValue.Add("TeamNameSignature", GetTeamNameSignatureByCompanyId(CompanyId));
            }
            if (TemplateValue["CompanyNameAlt"] == null)
            {
                TemplateValue.Add("CompanyNameAlt", GetCompanyNameByCompanyId(CompanyId));
            }
            if (TemplateValue["CompanyInformation"] == null)
            {
                TemplateValue.Add("CompanyInformation", GetFooterCompanyInformationByCompanyId(CompanyId));
            }
            #endregion

            EmailTemplate emailTemplate = _EmailTemplateDataAccess.GetByQuery("TemplateKey='" + TemplateKey + "'").FirstOrDefault();
            //if(emailTemplate == null)
            //{
            //    try
            //    {
            //        using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\ErrorReports\EmailError.txt"), true))
            //        {
            //            file.WriteLine(string.Format("{0} Template Not Found. Template Key: {1}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"), TemplateKey));

            //        }
            //    }
            //    catch (Exception) { }
            //}
            EmailParser parser = null;
            string toEmailAddress = "";
            string FromName = "";

            #region BodyFile And BodyContent
            if (emailTemplate == null || emailTemplate.Id == 0)
            {
                //Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception("Template not found for key :" + TemplateKey));
                return false;
            }
            if (!string.IsNullOrWhiteSpace(emailTemplate.BodyContent))
            {
                parser = new EmailParser(emailTemplate.BodyContent, TemplateValue, false);
            }
            else if (!string.IsNullOrWhiteSpace(emailTemplate.BodyFile))
            {
                parser = new EmailParser(System.Web.Hosting.HostingEnvironment.MapPath(emailTemplate.BodyFile), TemplateValue, false);
                var abc = parser;
            }
            #endregion

            MailMessage message = new MailMessage();
            message.Body = parser.Parse();

            #region ToEmail
            if (emailTemplate.ToEmail.IndexOf("##") > -1)
            {
                EmailParser ToEmailParser = new EmailParser(emailTemplate.ToEmail, TemplateValue, false);
                string EmailAddress = ToEmailParser.Parse();
                if (EmailAddress.IsValidEmailAddress())
                {
                    message.To.Add(new MailAddress(EmailAddress));
                    toEmailAddress = message.To[0].ToString();
                }
                else
                {
                    EmailAddress = EmailAddress.Replace(" ", "");
                    toEmailAddress = EmailAddress;
                    if (EmailAddress.Split(',').Count() > 1)
                    {
                        string[] addList = EmailAddress.Split(',');
                        foreach (var item in addList)
                        {
                            if (item.IsValidEmailAddress())
                            {
                                message.To.Add(new MailAddress(item));
                            }
                            else if (item.Split('-').Count() > 0 && item.Split('-')[0].IsValidEmailAddress())
                            {
                                message.To.Add(new MailAddress(item.Split('-')[0], item.Split('-')[1]));
                            }
                        }
                    }
                    else if (EmailAddress.Split(';').Count() > 1)
                    {
                        string[] addList = EmailAddress.Split(';');
                        foreach (var item in addList)
                        {
                            if (item.IsValidEmailAddress())
                            {
                                message.To.Add(new MailAddress(item));
                            }
                            else if (item.Split('>').Count() > 0 && item.Split('>')[0].IsValidEmailAddress())
                            {
                                message.To.Add(new MailAddress(item.Split('>')[0], item.Split('>')[1]));
                            }
                        }
                    }
                    if (message.To.Count == 0)
                    {
                        return false;
                    }

                }
            }
            else
            {
                message.To.Add(emailTemplate.ToEmail);
                toEmailAddress = message.To[0].ToString();
            }
            #endregion

            #region FromName
            if (!string.IsNullOrWhiteSpace(emailTemplate.FromName) && emailTemplate.FromName.IndexOf("##") > -1)
            {
                try
                {
                    EmailParser FromNameParser = new EmailParser(emailTemplate.FromName, TemplateValue, false);
                    FromName = FromNameParser.Parse();
                }
                catch (Exception)
                {
                    FromName = "rmrcloud.com";
                }
            }
            else
            {
                FromName = emailTemplate.FromName;
            }
            #endregion

            #region Reply Email
            if (emailTemplate.ReplyEmail.IndexOf("##") > -1)
            {
                EmailParser ReplyEmailParser = new EmailParser(emailTemplate.ReplyEmail, TemplateValue, false);
                message.ReplyToList.Add(new MailAddress(ReplyEmailParser.Parse(), FromName));
            }
            else
            {
                message.ReplyToList.Add(new MailAddress(emailTemplate.ReplyEmail, FromName));
            }
            #endregion

            #region From Email
            if (emailTemplate.FromEmail.IndexOf("##") > -1)
            {
                EmailParser FromEmailParser = new EmailParser(emailTemplate.FromEmail, TemplateValue, false);
                message.From = new MailAddress(FromEmailParser.Parse(), FromName);
            }
            else
            {
                message.From = new MailAddress(emailTemplate.FromEmail, FromName);
            }
            #endregion

            #region CC & BCC
            if (!string.IsNullOrWhiteSpace(emailTemplate.BccEmail))
            {
                if (emailTemplate.BccEmail.IndexOf("##") > -1)
                {
                    EmailParser BCCEmailParser = new EmailParser(emailTemplate.BccEmail, TemplateValue, false);
                    emailTemplate.BccEmail = BCCEmailParser.Parse();
                }
                if (emailTemplate.BccEmail.IndexOf(";") > -1)
                {
                    var ArrBcc = emailTemplate.BccEmail.Split(';');
                    foreach (var item in ArrBcc)
                    {
                        message.Bcc.Add(item);
                    }
                }
                else
                {
                    message.Bcc.Add(emailTemplate.BccEmail);
                }
            }

            if (!string.IsNullOrWhiteSpace(emailTemplate.CcEmail))
            {
                if (emailTemplate.CcEmail.IndexOf("##") > -1)
                {
                    EmailParser CCEmailParser = new EmailParser(emailTemplate.CcEmail, TemplateValue, false);
                    emailTemplate.CcEmail = CCEmailParser.Parse();
                }
                if (emailTemplate.CcEmail.IndexOf(";") > -1)
                {
                    var ArrCcEmail = emailTemplate.CcEmail.Split(';');
                    foreach (var item in ArrCcEmail)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            message.CC.Add(item);
                        }
                    }
                }
                else if (emailTemplate.CcEmail.IsValidEmailAddress())
                {
                    message.CC.Add(emailTemplate.CcEmail);
                }
            }
            else if (TemplateValue["CCEmail"] != null)
            {
                if (!string.IsNullOrWhiteSpace(TemplateValue["CCEmail"].ToString()))
                {
                    if (TemplateValue["CCEmail"].ToString().IndexOf(";") > -1)
                    {
                        var ArrCcEmail = TemplateValue["CCEmail"].ToString().Split(';');
                        foreach (var item in ArrCcEmail)
                        {
                            if (!string.IsNullOrEmpty(item) && item.IsValidEmailAddress())
                            {
                                message.CC.Add(item);
                            }
                        }
                    }
                    else if (TemplateValue["CCEmail"].ToString().IsValidEmailAddress())
                    {
                        message.CC.Add(TemplateValue["CCEmail"].ToString());
                    }
                }
            }

            #endregion

            #region Subject
            if (emailTemplate.Subject.IndexOf("##") > -1)
            {
                EmailParser SubjectParser = new EmailParser(emailTemplate.Subject, TemplateValue, false);
                message.Subject = SubjectParser.Parse();
            }
            else
            {
                message.Subject = emailTemplate.Subject;
            }
            #endregion

            #region Attachments
            if (Attachments != null)
            {
                if (Attachments.Count > 0)
                {
                    foreach (var attachment in Attachments)
                    {
                        if (attachment != null)
                        {
                            message.Attachments.Add(attachment);
                        }
                    }
                }
            }
            #endregion

            message.IsBodyHtml = true;
            message.BodyEncoding = System.Text.Encoding.UTF8;

            try
            {

                SmtpClient client = new SmtpClient();
                GlobalSetting EmailHost = _GlobalSettingDataAccess.GetByQuery(string.Format(" SearchKey ='{0}' and CompanyId ='{1}'", "EmailHost", CompanyId)).FirstOrDefault();
                GlobalSetting EmailHostUsername = _GlobalSettingDataAccess.GetByQuery(string.Format(" SearchKey ='{0}' and CompanyId ='{1}'", "EmailHostUsername", CompanyId)).FirstOrDefault();
                GlobalSetting EmailHostPassword = _GlobalSettingDataAccess.GetByQuery(string.Format(" SearchKey ='{0}' and CompanyId ='{1}'", "EmailHostPassword", CompanyId)).FirstOrDefault();
                GlobalSetting EmailPort = _GlobalSettingDataAccess.GetByQuery(string.Format(" SearchKey ='{0}' and CompanyId ='{1}'", "EmailPort", CompanyId)).FirstOrDefault();

                if (EmailHost != null && !string.IsNullOrWhiteSpace(EmailHost.Value))
                {
                    client.Host = EmailHost.Value;
                }
                if (EmailHostUsername != null && !string.IsNullOrWhiteSpace(EmailHostUsername.Value)
                    && EmailHostPassword != null && !string.IsNullOrWhiteSpace(EmailHostPassword.Value))
                {
                    client.Credentials = new System.Net.NetworkCredential(EmailHostUsername.Value, EmailHostPassword.Value);
                }
                if (EmailPort != null && !string.IsNullOrWhiteSpace(EmailPort.Value))
                {
                    int port = 587;
                    if (int.TryParse(EmailPort.Value, out port))
                    {
                        client.Port = port;
                    }
                }
                client.EnableSsl = false;
                //message.From = new MailAddress("noreply@rmrcloud.com");
                //message.From = new MailAddress("info@marketing.centralstationmarketing.com");
                //Need to user From email of default domain
                client.Send(message);


                #region No need now
                //if (!HttpContext.Current.Request.IsLocal)
                //{
                //    SmtpClient client = new SmtpClient();
                //    client.Credentials = new System.Net.NetworkCredential("noreply@piiscenter.com", "piiscenter.com");
                //    client.EnableSsl = false;
                //    client.Send(message);
                //}
                //SmtpClient smtp = new SmtpClient
                //{
                //    Host = "smtp.gmail.com",
                //    //change the port to prt 587. This seems to be the standard for Google smtp transmissions.
                //    Port = 587,
                //    //enable SSL to be true, otherwise it will get kicked back by the Google server.
                //    EnableSsl = true,
                //    //The following properties need set as well
                //    DeliveryMethod = SmtpDeliveryMethod.Network,
                //    UseDefaultCredentials = false,
                //    Credentials = new NetworkCredential("informrcloud@gmail.com", "Inf0rmrcl@ud")
                //};
                //smtp.Send(message);
                #endregion
                #region Insert into Email History table
                EmailHistory emailHistory = new EmailHistory();
                emailHistory.TemplateKey = TemplateKey;
                emailHistory.ToEmail = toEmailAddress;
                emailHistory.CcEmail = message.CC.ToString();
                emailHistory.BccEmail = message.Bcc.ToString();
                emailHistory.FromEmail = message.From.ToString();
                emailHistory.EmailBodyContent = message.Body;
                emailHistory.EmailSubject = message.Subject;
                emailHistory.EmailSentDate = DateTime.Now;
                emailHistory.LastUpdatedDate = DateTime.Now;
                _EmailHistoryDataAccess.Insert(emailHistory);
                #endregion
                #region LeadCorrespondence
                if (TemplateKey == "InvoiceEmail"
                    || TemplateKey == "EstimateEmail"
                    || TemplateKey == "EmailtoLeadsAggrement"
                    || TemplateKey == "mailtoCustomerforTransaction"
                    || TemplateKey == "mailToLeadSignAgreement")
                {
                    string CustomerId = "00000000-0000-0000-0000-000000000000";
                    string EmployeeId = "00000000-0000-0000-0000-000000000000";
                    if (TemplateValue["CustomerId"] != null && !string.IsNullOrWhiteSpace(TemplateValue["CustomerId"].ToString()))
                    {
                        CustomerId = TemplateValue["CustomerId"].ToString();
                    }
                    if (TemplateValue["EmployeeId"] != null && !string.IsNullOrWhiteSpace(TemplateValue["EmployeeId"].ToString()))
                    {
                        EmployeeId = TemplateValue["EmployeeId"].ToString();
                    }
                    LeadCorrespondence objcorres = new LeadCorrespondence();
                    objcorres.CompanyId = CompanyId;
                    objcorres.CustomerId = new Guid(CustomerId);
                    objcorres.ToEmail = toEmailAddress;
                    objcorres.TemplateKey = TemplateKey;
                    objcorres.Type = "Email";
                    objcorres.Subject = message.Subject;
                    objcorres.BodyContent = message.Body;
                    objcorres.SentDate = DateTime.Now.UTCCurrentTime();
                    objcorres.IsSystemAutoSent = true;
                    objcorres.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    objcorres.CcEmail = message.CC.ToString();
                    objcorres.SentBy = new Guid(EmployeeId);
                    _LeadCorrespondenceDataAccess.Insert(objcorres);
                    try
                    {
                        _ActivityDataAccess.Insert(new Activity()
                        {
                            ActivityId = Guid.NewGuid(),
                            ActivityType = objcorres.Type,
                            AssignedTo = objcorres.SentBy,
                            AssociatedWith = objcorres.CustomerId,
                            Status = "Completed",
                            //AssociatedType = 
                            CreatedBy = objcorres.SentBy,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            Note = objcorres.BodyContent,
                            NotifyBy = "",

                        });
                    }
                    catch (Exception) { }
                }
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }


            return false;

        }

        #region Private 
        public string GetFacebookUrlByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;
            string key = RMRCacheKey.FacebookUrl + CompanyId.ToString();
            var globalsetting = new GlobalSetting();
            if (System.Web.HttpRuntime.Cache[key] == null)
            {
                string DataKey = "FaceBook";
                    globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                    result = globalsetting.Value;
                    System.Web.HttpRuntime.Cache[key] = result;    
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[key];
            }
            return result;
        }
        public string GetInstagramUrlByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;
            string key = RMRCacheKey.InstagramUrl + CompanyId.ToString();
            var globalsetting = new GlobalSetting();
            if (System.Web.HttpRuntime.Cache[key] == null)
            {
                string DataKey = "Instagram";
                    globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                    result = globalsetting != null ? globalsetting.Value : "";
                    System.Web.HttpRuntime.Cache[key] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[key];
            }
            return result;
        }
        public string GetYoutubeUrlByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;
            var globalsetting = new GlobalSetting();
            string Key = RMRCacheKey.YoutubeUrl + CompanyId.ToString();
            if (System.Web.HttpRuntime.Cache[Key] == null)
            {
                string DataKey = "Youtube";
                    globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                    result = globalsetting.Value;
                    System.Web.HttpRuntime.Cache[Key] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[Key];
            }
            return result;
        }
        public string GetTeamNameSignatureByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;
            string TeamNameSignature = RMRCacheKey.TeamNameSignatureLoad + CompanyId.ToString();
            var globalsetting = new GlobalSetting();
            if (System.Web.HttpRuntime.Cache[TeamNameSignature] == null)
            {
                string DataKey = "TeamNameSignature";
                    globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                    if (globalsetting == null)
                    {
                        return "";
                    }
                    result = globalsetting.Value;
                    System.Web.HttpRuntime.Cache[TeamNameSignature] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[TeamNameSignature];
            }
            return result;
        }
        public string GetFooterCompanyInformationByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;
            string FooterCompanyInformation = RMRCacheKey.FooterCompanyInformation + CompanyId.ToString();
            var globalsetting = new GlobalSetting();
            System.Web.HttpRuntime.Cache.Remove(FooterCompanyInformation);
            if (System.Web.HttpRuntime.Cache[FooterCompanyInformation] == null)
            {
                string DataKey = "FooterCompanyInformation";
                    globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                    if (globalsetting == null)
                    {
                        return "";
                    }
                    result = globalsetting.Value;
                    System.Web.HttpRuntime.Cache[FooterCompanyInformation] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[FooterCompanyInformation];
            }
            return result;
        }
        public string GetCompanyNameByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;
            Company comp = _CompanyDataAccess.GetByQuery(string.Format(" companyid  = '{0}'", CompanyId)).FirstOrDefault();
            if (comp != null)
            {
                result = comp.CompanyName;
            }
            return result;
        }
        public string GetEmailLogoByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;
            string key = RMRCacheKey.EmailLogoUrl + CompanyId.ToString();
            //var globalsetting = new GlobalSetting();
            if (System.Web.HttpRuntime.Cache[key] == null)
            {
                // string DataKey = "EmailLogo";
                //globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                //result = globalsetting.Value;
                CompanyBranch cb = _CompanyBranchDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and IsMainBranch =1", CompanyId)).FirstOrDefault();
                if (cb != null && !string.IsNullOrWhiteSpace(cb.EmailLogo))
                {
                    result = cb.EmailLogo;
                    result = AppConfig.ImageDomain + result;
                }
                else
                {
                    result = ConfigurationManager.AppSettings["Logo.DefaultEmailLogo"];
                }
                System.Web.HttpRuntime.Cache[key] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[key];
            }
            return result;
        }
        #endregion

        public string GetCompanyAddressFormat(Guid CompanyId)
        {
            string result = string.Empty;
            string SearchKey = "CompanyAddressPdfFormat";
            GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
            result = globalsetting == null ? "" : globalsetting.Value;
            return result;
        }

        public List<EstimateImage> GetAllEstimateImageByInvoiceId(string invid)
        {
            return _EstimateImageDataAccess.GetByQuery(string.Format("InvoiceId = '{0}'", invid)).ToList();
        }

        public List<Equipment> GetAllEquipmentsFavouriteByUserIdAndCompanyId(Guid comid, Guid userid)
        {
            DataTable dt = _EquipmentsFavouriteDataAccess.GetAllEquipmentsFavouriteByUserIdAndCompanyId(comid, userid);
            List<Equipment> EstimateImageList = new List<Equipment>();
            EstimateImageList = (from DataRow dr in dt.Rows
                                 select new Equipment()
                                 {
                                     Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                     EquipmentId = (Guid)dr["EquipmentId"],
                                     Name = dr["Name"].ToString(),
                                     SKU = dr["SKU"].ToString(),
                                     Description = dr["Comments"].ToString(),
                                     Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0
                                 }).ToList();
            return EstimateImageList;
        }

        public List<EmployeeTimeClock> GetLastClocksByUserIdAndTimePeriod(Guid userId, int pageno, int pagesize, DateTime StartDate, DateTime EndDate)
        {
            DataTable dt = _employeeTimeClockDataAccess.GetLastClocksByUserIdAndTimePeriodForApi(userId, pageno, pagesize, StartDate, EndDate);

            List<EmployeeTimeClock> buildList = new List<EmployeeTimeClock>();
            buildList = (from DataRow dr in dt.Rows
                         select new EmployeeTimeClock()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             UserId = (Guid)dr["UserId"],
                             ClockInTime = dr["ClockInTime"] != DBNull.Value ? Convert.ToDateTime(dr["ClockInTime"]) : new DateTime(),
                             ClockOutTime = dr["ClockOutTime"] != DBNull.Value ? Convert.ToDateTime(dr["ClockOutTime"]) : new DateTime(),
                             ClockInLat = dr["ClockInLat"].ToString(),
                             ClockInLng = dr["ClockInLng"].ToString(),
                             ClockOutLat = dr["ClockOutLat"].ToString(),
                             ClockOutLng = dr["ClockOutLng"].ToString(),
                             ClockInNote = dr["ClockInNote"].ToString(),
                             ClockOutNote = dr["ClockOutNote"].ToString(),
                             LastUpdatedName = dr["LastUpdatedName"].ToString(),
                             ClockInCreatedBy = (Guid)dr["ClockInCreatedBy"],
                             ClockOutCreatedBy = (Guid)dr["ClockOutCreatedBy"],
                             ClockedInSeconds = dr["ClockedInSeconds"] != DBNull.Value ? Convert.ToInt32(dr["ClockedInSeconds"]) : 0,
                             LastUpdateBy = (Guid)dr["LastUpdateBy"],
                             LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime()
                         }).ToList();
            return buildList;
        }
        public List<EmployeeTimeClock> GetLastClocksByUserId(Guid userId)
        {
            DataTable dt = _employeeTimeClockDataAccess.GetLastClocksByUserIdForApi(userId);

            List<EmployeeTimeClock> buildList = new List<EmployeeTimeClock>();
            buildList = (from DataRow dr in dt.Rows
                         select new EmployeeTimeClock()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             UserId = (Guid)dr["UserId"],
                             ClockInTime = dr["ClockInTime"] != DBNull.Value ? Convert.ToDateTime(dr["ClockInTime"]) : new DateTime(),
                             ClockOutTime = dr["ClockOutTime"] != DBNull.Value ? Convert.ToDateTime(dr["ClockOutTime"]) : new DateTime(),
                             ClockInLat = dr["ClockInLat"].ToString(),
                             ClockInLng = dr["ClockInLng"].ToString(),
                             ClockOutLat = dr["ClockOutLat"].ToString(),
                             ClockOutLng = dr["ClockOutLng"].ToString(),
                             ClockInNote = dr["ClockInNote"].ToString(),
                             ClockOutNote = dr["ClockOutNote"].ToString(),
                             LastUpdatedName = dr["LastUpdatedName"].ToString(),
                             ClockInCreatedBy = (Guid)dr["ClockInCreatedBy"],
                             ClockOutCreatedBy = (Guid)dr["ClockOutCreatedBy"],
                             ClockedInSeconds = dr["ClockedInSeconds"] != DBNull.Value ? Convert.ToInt32(dr["ClockedInSeconds"]) : 0,
                             LastUpdateBy = (Guid)dr["LastUpdateBy"],
                             LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime()
                         }).ToList();
            return buildList;
        }
        public int GetTimeClockHistoryCount(Guid userId, DateTime StartDate, DateTime EndDate)
        {
            DataTable dt = _employeeTimeClockDataAccess.GetTimeClockHistoryCount(userId, StartDate, EndDate);
            TotalCount TotalCount = new TotalCount();
            TotalCount = (from DataRow dr in dt.Rows
                          select new TotalCount()
                          {
                              CountTotal = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                          }).FirstOrDefault();
            return TotalCount.CountTotal;
        }
        public bool InsertUserCompanyDevice(UserCompanyDevice UserCompanyDeviceLog)
        {
            return _UserCompanyDeviceDataAccess.Insert(UserCompanyDeviceLog) > 0;
        }
        public UserCompanyDevice UserCompanyDeviceLog(Guid CompanyId, Guid UserId, string DeviceId)
        {
            string query = string.Format(" CompanyId ='{0}' AND UserId='{1}' AND DeviceId= '{2}'", CompanyId, UserId, DeviceId);
            return _UserCompanyDeviceDataAccess.GetByQuery(query).FirstOrDefault();
        }

        public UserCompanyDevice UserCompanyDeviceLogByDeviceId(string DeviceId)
        {
            string query = string.Format("DeviceId= '{0}'", DeviceId);
            return _UserCompanyDeviceDataAccess.GetByQuery(query).FirstOrDefault();
        }

        public bool UpdateUserCompanyDevice(UserCompanyDevice UserCompanyDeviceLog)
        {
            return _UserCompanyDeviceDataAccess.Update(UserCompanyDeviceLog) > 0;
        }

        public List<Customer> GetGlobalSearchCustomerAndLeadByKey(string key, Guid comid)
        {
            DataTable dt = _customerDataAccess.GetGlobalSearchCustomerAndLeadByKey(key, comid);
            List<Customer> EstimateImageList = new List<Customer>();
            EstimateImageList = (from DataRow dr in dt.Rows
                                 select new Customer()
                                 {
                                     Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                     CustomerNo = dr["CustomerNo"].ToString(),
                                     Title = dr["Title"].ToString(),
                                     SSN = dr["SSN"].ToString(),
                                     CellNo = dr["CellNo"].ToString(),
                                     Fax = dr["Fax"].ToString(),
                                     CallingTime = dr["CallingTime"].ToString(),
                                     Address2 = dr["Address2"].ToString(),
                                     Country = dr["Country"].ToString(),
                                     StreetPrevious = dr["StreetPrevious"].ToString(),
                                     CityPrevious = dr["CityPrevious"].ToString(),
                                     StatePrevious = dr["StatePrevious"].ToString(),
                                     ZipCodePrevious = dr["ZipCodePrevious"].ToString(),
                                     CountryPrevious = dr["CountryPrevious"].ToString(),
                                     AccountNo = dr["AccountNo"].ToString(),
                                     IsAlarmCom = dr["IsAlarmCom"] != DBNull.Value ? Convert.ToBoolean(dr["IsAlarmCom"]) : false,
                                     CreditScore = dr["CreditScore"].ToString(),
                                     ContractTeam = dr["ContractTeam"].ToString(),
                                     FundingCompany = dr["FundingCompany"].ToString(),
                                     MonthlyMonitoringFee = dr["MonthlyMonitoringFee"].ToString(),
                                     CellularBackup = dr["CellularBackup"] != DBNull.Value ? Convert.ToBoolean(dr["CellularBackup"]) : false,
                                     CustomerFunded = dr["CustomerFunded"] != DBNull.Value ? Convert.ToBoolean(dr["CustomerFunded"]) : false,
                                     Maintenance = dr["Maintenance"] != DBNull.Value ? Convert.ToBoolean(dr["Maintenance"]) : false,
                                     Note = dr["Note"].ToString(),
                                     SalesDate = dr["SalesDate"] != DBNull.Value ? Convert.ToDateTime(dr["SalesDate"]) : new DateTime(),
                                     FollowUpDate = dr["FollowUpDate"] != DBNull.Value ? Convert.ToDateTime(dr["FollowUpDate"]) : new DateTime(),
                                     InstallDate = dr["InstallDate"] != DBNull.Value ? Convert.ToDateTime(dr["InstallDate"]) : new DateTime(),
                                     CutInDate = dr["CutInDate"] != DBNull.Value ? Convert.ToDateTime(dr["CutInDate"]) : new DateTime(),
                                     Installer = dr["Installer"].ToString(),
                                     Soldby = dr["Soldby"].ToString(),
                                     FundingDate = dr["FundingDate"] != DBNull.Value ? Convert.ToDateTime(dr["FundingDate"]) : new DateTime(),
                                     ReminderDate = dr["ReminderDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReminderDate"]) : new DateTime(),
                                     QA1 = dr["QA1"].ToString(),
                                     QA1Date = dr["QA1Date"] != DBNull.Value ? Convert.ToDateTime(dr["QA1Date"]) : new DateTime(),
                                     QA2 = dr["QA2"].ToString(),
                                     QA2Date = dr["QA2Date"] != DBNull.Value ? Convert.ToDateTime(dr["QA2Date"]) : new DateTime(),
                                     BillAmount = dr["BillAmount"] != DBNull.Value ? Convert.ToDouble(dr["BillAmount"]) : 0.0,
                                     PaymentMethod = dr["PaymentMethod"].ToString(),
                                     BillCycle = dr["BillCycle"].ToString(),
                                     BillDay = dr["BillDay"] != DBNull.Value ? Convert.ToInt32(dr["BillDay"]) : 0,
                                     BillNotes = dr["BillNotes"].ToString(),
                                     BillTax = dr["BillTax"] != DBNull.Value ? Convert.ToBoolean(dr["BillTax"]) : false,
                                     BillOutStanding = dr["BillOutStanding"] != DBNull.Value ? Convert.ToDouble(dr["BillOutStanding"]) : 0.0,
                                     ServiceDate = dr["ServiceDate"] != DBNull.Value ? Convert.ToDateTime(dr["ServiceDate"]) : new DateTime(),
                                     Area = dr["Area"].ToString(),
                                     Latlng = dr["Latlng"].ToString(),
                                     SecondCustomerNo = dr["SecondCustomerNo"].ToString(),
                                     AdditionalCustomerNo = dr["AdditionalCustomerNo"].ToString(),
                                     IsTechCallPassed = dr["IsTechCallPassed"] != DBNull.Value ? Convert.ToBoolean(dr["IsTechCallPassed"]) : false,
                                     IsDirect = dr["IsDirect"] != DBNull.Value ? Convert.ToBoolean(dr["IsDirect"]) : false,
                                     AuthorizeRefId = dr["AuthorizeRefId"].ToString(),
                                     AuthorizeCusProfileId = dr["AuthorizeCusProfileId"].ToString(),
                                     AuthorizeCusPaymentProfileId = dr["AuthorizeCusPaymentProfileId"].ToString(),
                                     AuthorizeDescription = dr["AuthorizeDescription"].ToString(),
                                     IsRequiredCsvSync = dr["IsRequiredCsvSync"] != DBNull.Value ? Convert.ToBoolean(dr["IsRequiredCsvSync"]) : false,
                                     Passcode = dr["Passcode"].ToString(),
                                     ActivationFee = dr["ActivationFee"] != DBNull.Value ? Convert.ToDouble(dr["ActivationFee"]) : 0.0,
                                     FirstBilling = dr["FirstBilling"] != DBNull.Value ? Convert.ToDateTime(dr["FirstBilling"]) : new DateTime(),
                                     ActivationFeePaymentMethod = dr["ActivationFeePaymentMethod"].ToString(),
                                     IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                                     LastGeneratedInvoice = dr["LastGeneratedInvoice"] != DBNull.Value ? Convert.ToDateTime(dr["LastGeneratedInvoice"]) : new DateTime(),
                                     Singature = dr["Singature"].ToString(),
                                     CrossStreet = dr["CrossStreet"].ToString(),
                                     DBA = dr["DBA"].ToString(),
                                     AlarmRefId = dr["AlarmRefId"].ToString(),
                                     TransunionRefId = dr["TransunionRefId"].ToString(),
                                     MonitronicsRefId = dr["MonitronicsRefId"].ToString(),
                                     CentralStationRefId = dr["CentralStationRefId"].ToString(),
                                     CmsRefId = dr["CmsRefId"].ToString(),
                                     PreferedEmail = dr["PreferedEmail"] != DBNull.Value ? Convert.ToBoolean(dr["PreferedEmail"]) : false,
                                     PreferedSms = dr["PreferedSms"] != DBNull.Value ? Convert.ToBoolean(dr["PreferedSms"]) : false,
                                     IsAgreement = dr["IsAgreement"] != DBNull.Value ? Convert.ToBoolean(dr["IsAgreement"]) : false,
                                     IsFireAccount = dr["IsFireAccount"] != DBNull.Value ? Convert.ToBoolean(dr["IsFireAccount"]) : false,
                                     CreatedByUid = (Guid)dr["CreatedByUid"],
                                     CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                     LastUpdatedByUid = (Guid)dr["LastUpdatedByUid"],
                                     BusinessAccountType = dr["BusinessAccountType"].ToString(),
                                     PhoneType = dr["PhoneType"].ToString(),
                                     Carrier = dr["Carrier"].ToString(),
                                     ReferringCustomer = (Guid)dr["ReferringCustomer"],
                                     EsistingPanel = dr["EsistingPanel"].ToString(),
                                     Ownership = dr["Ownership"].ToString(),
                                     PurchasePrice = dr["PurchasePrice"] != DBNull.Value ? Convert.ToDouble(dr["PurchasePrice"]) : 0.0,
                                     ContractValue = dr["ContractValue"].ToString(),
                                     ChildOf = (Guid)dr["ChildOf"],
                                     EmailVerified = dr["EmailVerified"] != DBNull.Value ? Convert.ToBoolean(dr["EmailVerified"]) : false,
                                     HomeVerified = dr["HomeVerified"] != DBNull.Value ? Convert.ToBoolean(dr["HomeVerified"]) : false,
                                     County = dr["County"].ToString(),
                                     CustomerToken = dr["CustomerToken"].ToString(),
                                     PaymentToken = dr["PaymentToken"].ToString(),
                                     ScheduleToken = dr["ScheduleToken"].ToString(),
                                     EstCloseDate = dr["EstCloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["EstCloseDate"]) : new DateTime(),
                                     ProjectWalkDate = dr["ProjectWalkDate"] != DBNull.Value ? Convert.ToDateTime(dr["ProjectWalkDate"]) : new DateTime(),
                                     BranchId = dr["BranchId"] != DBNull.Value ? Convert.ToInt32(dr["BranchId"]) : 0,
                                     SubscriptionStatus = dr["SubscriptionStatus"].ToString(),
                                     AnnualRevenue = dr["AnnualRevenue"] != DBNull.Value ? Convert.ToDouble(dr["AnnualRevenue"]) : 0.0,
                                     Website = dr["Website"].ToString(),
                                     Market = dr["Market"].ToString(),
                                     Passengers = dr["Passengers"] != DBNull.Value ? Convert.ToInt32(dr["Passengers"]) : 0,
                                     Budget = dr["Budget"] != DBNull.Value ? Convert.ToDouble(dr["Budget"]) : 0.0,
                                     SmartSetUpStep = dr["SmartSetUpStep"] != DBNull.Value ? Convert.ToInt32(dr["SmartSetUpStep"]) : 0,
                                     CustomerAccountType = dr["CustomerAccountType"].ToString(),
                                     IsPrimaryPhoneVerified = dr["IsPrimaryPhoneVerified"] != DBNull.Value ? Convert.ToBoolean(dr["IsPrimaryPhoneVerified"]) : false,
                                     IsSecondaryPhoneVerified = dr["IsSecondaryPhoneVerified"] != DBNull.Value ? Convert.ToBoolean(dr["IsSecondaryPhoneVerified"]) : false,
                                     IsCellNoVerified = dr["IsCellNoVerified"] != DBNull.Value ? Convert.ToBoolean(dr["IsCellNoVerified"]) : false,
                                     FirstName = dr["FirstName"].ToString(),
                                     LastName = dr["LastName"].ToString(),
                                     MiddleName = dr["MiddleName"].ToString(),
                                     BusinessName = dr["BusinessName"].ToString(),
                                     Status = dr["Status"].ToString(),
                                     Street = dr["Street"].ToString(),
                                     City = dr["City"].ToString().UppercaseFirst(),
                                     State = dr["State"].ToString(),
                                     ZipCode = dr["ZipCode"].ToString(),
                                     Address = dr["Address"].ToString(),
                                     EmailAddress = dr["EmailAddress"].ToString(),
                                     PrimaryPhone = dr["PrimaryPhone"].ToString(),
                                     SecondaryPhone = dr["SecondaryPhone"].ToString(),
                                     CustomerId = (Guid)dr["CustomerId"],
                                     DateofBirth = dr["DateofBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DateofBirth"]) : new DateTime(),
                                     JoinDate = dr["JoinDate"] != DBNull.Value ? Convert.ToDateTime(dr["JoinDate"]) : new DateTime(),
                                     LeadSource = dr["LeadSource"].ToString(),
                                     LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                     LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                                     StreetType = dr["StreetType"].ToString(),
                                     Appartment = dr["Appartment"].ToString(),
                                     Type = dr["Type"].ToString(),
                                     CustomerType = dr["CustomerType"].ToString()
                                 }).ToList();
            return EstimateImageList;
        }

        public CustomerInspection GetCustomerInspectionByCustomerIdAndCompanyId(Guid customerid, Guid companyid)
        {
            return _CustomerInspectionDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and CustomerId = '{1}'", companyid, customerid)).FirstOrDefault();
        }
        public CustomerInspection GetCustomerInspectionValueByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId)
        {
            DataTable dt = _EmployeeDataAccess.GetCustomerInspactionByCustomerIdAndCompanyId(CustomerId, CompanyId);
            CustomerInspection InspectionList = new CustomerInspection();
            InspectionList = (from DataRow dr in dt.Rows
                            select new CustomerInspection()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                CustomerId = (Guid)dr["CustomerId"],
                                CompanyId =(Guid)dr["CompanyId"],
                                OutsideRelativeHumidity = dr["OutsideRelativeHumidity"] != DBNull.Value ? Convert.ToDouble(dr["OutsideRelativeHumidity"]) : 0.0,
                                OutsideTemperature = dr["OutsideTemperature"] != DBNull.Value ? Convert.ToDouble(dr["OutsideTemperature"]) : 0.0,
                                FirstFloorRelativeHumidity = dr["FirstFloorRelativeHumidity"] != DBNull.Value ? Convert.ToDouble(dr["FirstFloorRelativeHumidity"]) : 0.0,
                                FirstFloorTemperature = dr["FirstFloorTemperature"] != DBNull.Value ? Convert.ToDouble(dr["FirstFloorTemperature"]) : 0.0,
                                RelativeOther1 = dr["RelativeOther1"].ToString(),
                                RelativeOther2 = dr["RelativeOther2"].ToString(),
                                BasementRelativeHumidity = dr["BasementRelativeHumidity"] != DBNull.Value ? Convert.ToDouble(dr["BasementRelativeHumidity"]) : 0.0,
                                BasementTemperature = dr["BasementTemperature"] != DBNull.Value ? Convert.ToDouble(dr["BasementTemperature"]) : 0.0,
                                VisualBasementOther = dr["VisualBasementOther"].ToString(),
                                NoticedSmellsOrOdorsComment = dr["NoticedSmellsOrOdorsComment"].ToString(),
                                NoticedMoldOrMildewComment = dr["NoticedMoldOrMildewComment"].ToString(),
                                HomeSufferForrespiratoryComment = dr["HomeSufferForrespiratoryComment"].ToString(),
                                ChildrenPlayInBasementComment = dr["ChildrenPlayInBasementComment"].ToString(),
                                PetsGoInBasementComment = dr["PetsGoInBasementComment"].ToString(),
                                NoticedBugsOrRodentsComment = dr["NoticedBugsOrRodentsComment"].ToString(),
                                GetWaterComment = dr["GetWaterComment"].ToString(),
                                SeeCondensationPipesDrippingComment = dr["SeeCondensationPipesDrippingComment"].ToString(),
                                RepairsProblemsComment = dr["RepairsProblemsComment"].ToString(),
                                HomeTestForPastRadonComment = dr["HomeTestForPastRadonComment"].ToString(),
                                CustomerBasementOther = dr["CustomerBasementOther"].ToString(),
                                Drawing = dr["Drawing"].ToString(),
                                Notes = dr["Notes"].ToString(),
                                PMSignature = dr["PMSignature"].ToString(),
                                PMSignatureDate = dr["PMSignatureDate"] != DBNull.Value ? Convert.ToDateTime(dr["PMSignatureDate"]) : new DateTime(),
                                HomeOwnerSignature = dr["HomeOwnerSignature"].ToString(),
                                HomeOwnerSignatureDate = dr["HomeOwnerSignatureDate"] != DBNull.Value ? Convert.ToDateTime(dr["HomeOwnerSignatureDate"]) : new DateTime(),
                                CreatedBy = (Guid)dr["CreatedBy"],
                                CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                LastUpdatedBy = (Guid)dr["LastUpdatedBy"],
                                LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                InspectionPhoto = dr["InspectionPhoto"].ToString(),
                                CurrentOutsideConditions = dr["CurrentOutsideConditions"].ToString(),
                                Heat = dr["Heat"].ToString(),
                                Air = dr["Air"].ToString(),
                                BasementDehumidifier = dr["BasementDehumidifier"].ToString(),
                                StrGroundWaterRating = dr["GroundWaterRating"].ToString(),
                                StrIronBacteriaRating = dr["IronBacteriaRating"].ToString(),
                                StrCondensationRating = dr["CondensationRating"].ToString(),
                                StrWallCracksRating = dr["WallCracksRating"].ToString(),
                                StrFloorCracksRating = dr["FloorCracksRating"].ToString(),
                                LivingPlan = dr["LivingPlan"].ToString(),
                                // YesNo
                                GroundWater = dr["GroundWater"].ToString(),
                                IronBacteria = dr["IronBacteria"].ToString(),
                                Condensation = dr["IronBacteria"].ToString(),
                                WallCracks = dr["WallCracks"].ToString(),
                                FloorCracks = dr["FloorCracks"].ToString(),
                                ExistingSumpPump = dr["ExistingSumpPump"].ToString(),
                                ExistingDrainageSystem = dr["ExistingDrainageSystem"].ToString(),
                                ExistingRadonSystem = dr["ExistingRadonSystem"].ToString(),
                                DryerVentToCode = dr["DryerVentToCode"].ToString(),
                                FoundationType = dr["FoundationType"].ToString(),
                                Bulkhead = dr["Bulkhead"].ToString(),
                                NoticedSmellsOrOdors = dr["NoticedSmellsOrOdors"].ToString(),
                                NoticedMoldOrMildew = dr["NoticedMoldOrMildew"].ToString(),
                                HomeSufferForRespiratory = dr["HomeSufferForRespiratory"].ToString(),
                                ChildrenPlayInBasement = dr["ChildrenPlayInBasement"].ToString(),
                                PetsGoInBasement = dr["PetsGoInBasement"].ToString(),
                                NoticedBugsOrRodents = dr["NoticedBugsOrRodents"].ToString(),
                                GetWater = dr["GetWater"].ToString(),
                                SeeCondensationPipesDripping = dr["SeeCondensationPipesDripping"].ToString(),
                                RepairsProblems = dr["RepairsProblems"].ToString(),
                                SellPlaning = dr["SellPlaning"].ToString(),
                                HomeTestForPastRadon = dr["HomeTestForPastRadon"].ToString(),
                                BasementGoDown = dr["BasementGoDown"].ToString(),
                                RemoveWater = dr["RemoveWater"].ToString(),
                                PlansForBasementOnce = dr["PlansForBasementOnce"].ToString(),
                                LosePower = dr["LosePower"].ToString(),
                                LosePowerHowOften = dr["LosePowerHowOften"].ToString()
                            }).FirstOrDefault();
            return InspectionList;
        }
        public bool UpdateEmployee(Employee Employee)
        {
            return _EmployeeDataAccess.Update(Employee) > 0;
        }
        public EmployeeDashboardAPI GetEmployeeByEmployeeIdAndCompanyId(Guid EmployeeId, Guid CompanyId)
        {
            DataTable dt = _EmployeeDataAccess.GetEmployeeByEmployeeIdAndCompanyId(EmployeeId, CompanyId);
            EmployeeDashboardAPI EmployeeList = new EmployeeDashboardAPI();
            EmployeeList = (from DataRow dr in dt.Rows
                            select new EmployeeDashboardAPI()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                UserId = (Guid)dr["UserId"],
                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                CompanyName = dr["CompanyName"].ToString(),
                                ProfileImage = dr["ProfilePicture"].ToString()
                            }).FirstOrDefault();
            return EmployeeList;
        }
        public ClockinDetail GetEmployeeClockInDetailByEmployeeId(Guid EmployeeId, DateTime Start, DateTime End)
        {
            DataTable dt = _employeeTimeClockDataAccess.GetEmployeeClockInDetail(EmployeeId, Start,End);
            ClockinDetail ClockDetail = new ClockinDetail();
            ClockDetail = (from DataRow dr in dt.Rows
                            select new ClockinDetail()
                            {
                                Status =dr["ClockInStatus"].ToString(),
                                ClockInTime = dr["ClockIn"] != DBNull.Value ? Convert.ToDateTime(dr["ClockIn"]) : new DateTime(),
                                ClockOutTime = dr["ClockOut"] != DBNull.Value ? Convert.ToDateTime(dr["ClockOut"]) : new DateTime(),
                                WeeklyVisit = dr["TotalVisitWeekly"] != DBNull.Value ? Convert.ToInt32(dr["TotalVisitWeekly"]) : 0,
                                Job = dr["Job"].ToString(),
                                StartPayPeriod = dr["Start"].ToString(),
                                EndPayPeriod = dr["End"].ToString(),
                                RegularHour = dr["RegularHours"] != DBNull.Value ? Convert.ToDouble(dr["RegularHours"]) : 0.0,
                                OTOHours = dr["OTOHours"] != DBNull.Value ? Convert.ToDouble(dr["OTOHours"]) : 0.0,
                                PTOHours = dr["PTOHours"] != DBNull.Value ? Convert.ToDouble(dr["PTOHours"]) : 0.0,
                                TotalHours = dr["TotalHours"] != DBNull.Value ? Convert.ToDouble(dr["TotalHours"]) : 0.0,
                            }).FirstOrDefault();
            return ClockDetail;
        }
        #region comment
        //public List<EmployeeTimeClock> GetEmployeeTimeClockListByUserId(Guid userId)
        //{
        //    DataTable dt = _employeeTimeClockDataAccess.GetEmployeeTimeClockListByUserId(userId);
        //    List<EmployeeTimeClock> EmployeeList = new List<EmployeeTimeClock>();
        //    EmployeeList = (from DataRow dr in dt.Rows
        //                    select new EmployeeTimeClock()
        //                    {
        //                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
        //                        UserId = (Guid)dr["UserId"],
        //                        ClockInTime = dr["ClockInTime"] != DBNull.Value ? Convert.ToDateTime(dr["ClockInTime"]) : new DateTime(),
        //                        ClockOutTime = dr["ClockOutTime"] != DBNull.Value ? Convert.ToDateTime(dr["ClockOutTime"]) : new DateTime(),
        //                        ClockInLat = dr["ClockInLat"].ToString(),
        //                        ClockOutLat = dr["ClockInLat"].ToString(),
        //                        ClockInLng = dr["ClockInLng"].ToString(),
        //                        ClockOutLng = dr["ClockOutLng"].ToString(),
        //                        ClockInNote = dr["ClockInNote"].ToString(),
        //                        ClockOutNote = dr["ClockOutNote"].ToString(),
        //                        ClockInCreatedBy = (Guid)dr["ClockInCreatedBy"],
        //                        ClockOutCreatedBy = (Guid)dr["ClockOutCreatedBy"],
        //                        ClockedInSeconds = dr["ClockedInSeconds"] != DBNull.Value ? Convert.ToInt32(dr["ClockedInSeconds"]) : 0,
        //                        LastUpdateBy = (Guid)dr["LastUpdateBy"],
        //                        LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),

        //                    }).ToList();
        //    return EmployeeList;
        //}
        #endregion

        public List<RestaurantOrderCustomModel> GetAllResturantOrderList(Guid comid, int pageno, int pagesize, string searchtext, string order)
        {
            DataTable dt = _ResturantOrderDataAccess.GetRestaurentOrderList(comid, pageno, pagesize, searchtext, order, null, null, new Guid(), false, "", "");
            List<RestaurantOrderCustomModel> miList = new List<RestaurantOrderCustomModel>();
            miList = (from DataRow dr in dt.Rows
                      select new RestaurantOrderCustomModel()
                      {
                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                          CompanyId = (Guid)dr["CompanyId"],
                          OrderId = (Guid)dr["OrderId"],
                          CustomerId = (Guid)dr["CustomerId"],
                          Location = dr["Location"].ToString(),
                          OrderType = dr["OrderType"].ToString(),
                          Status = dr["Status"].ToString(),
                          PickUpTime = dr["PickUpTime"].ToString(),
                          ContactNo = dr["ContactNo"].ToString(),
                          Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,

                          // Amount = dr["Amount"] ,
                          Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,

                          OrderDate = dr["AcceptDate"] != DBNull.Value ? Convert.ToDateTime(dr["AcceptDate"]) : dr["OrderDate"] != DBNull.Value ? Convert.ToDateTime(dr["OrderDate"]) : new DateTime(),
                          CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                          LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                          CustomerName = dr["CustomerName"].ToString(),
                          CustomerStreet = dr["CustomerStreet"].ToString(),
                          CustomerCity = dr["CustomerCity"].ToString(),
                          CustomerState = dr["CustomerState"].ToString(),
                          CustomerZip = dr["CustomerZip"].ToString(),
                          TaxAmount = dr["TaxAmount"] != DBNull.Value ? Convert.ToDouble(dr["TaxAmount"]) : 0,
                          Notes = dr["Notes"].ToString(),
                          AcceptDate = dr["AcceptDate"] != DBNull.Value ? Convert.ToDateTime(dr["AcceptDate"]) : new DateTime(),
                          RejectDate = dr["RejectedDate"] != DBNull.Value ? Convert.ToDateTime(dr["RejectedDate"]) : new DateTime(),
                          rejectNote = dr["RejectedReason"].ToString(),
                          PaymentMethod = dr["PaymentMethod"].ToString(),
                      }).ToList();
            return miList;
        }

        public List<ResturantOrder> GetAllOrdersByCompanyId(Guid comid)
        {
            return _ResturantOrderDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", comid)).ToList();
        }

        public List<RestaurantOrderDetailCustomModel> GetAllResturantOrderDetailByOrderId(Guid orderid)
        {
            DataTable dt = _customerDataAccess.GetAllResturantOrderDetailByOrderId(orderid);
            List<RestaurantOrderDetailCustomModel> miList = new List<RestaurantOrderDetailCustomModel>();
            miList = (from DataRow dr in dt.Rows
                      select new RestaurantOrderDetailCustomModel()
                      {
                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                          CompanyId = (Guid)dr["CompanyId"],
                          OrderId = (Guid)dr["OrderId"],
                          ItemId = dr["ItemId"] != DBNull.Value ? Convert.ToInt32(dr["ItemId"]) : 0,
                          ItemName = dr["ItemName"].ToString(),
                          ItemPrice = dr["ItemPrice"] != DBNull.Value ? Convert.ToDouble(dr["ItemPrice"]) : 0.0,
                          ItemQty = dr["ItemQty"] != DBNull.Value ? Convert.ToInt32(dr["ItemQty"]) : 0,
                          CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                          LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                          Toppings = dr["Toppings"].ToString(),
                          SpecialInstruction = dr["SpecialInstruction"].ToString(),
                          IsStock = dr["IsStock"] != DBNull.Value ? Convert.ToBoolean(dr["IsStock"]) : false,
                          OrderDate = dr["AcceptDate"] != DBNull.Value ? Convert.ToDateTime(dr["AcceptDate"]) : dr["OrderDate"] != DBNull.Value ? Convert.ToDateTime(dr["OrderDate"]) : new DateTime(),
                          DiscountCode = dr["DiscountCode"].ToString(),
                          DiscountValue = dr["DiscountValue"] != DBNull.Value ? Convert.ToDouble(dr["DiscountValue"]) : 0,
                      }).ToList();
            return miList;
        }

        //public List<ResturantOrderDetail> GetAllResturantOrderItems()
        //{
        //    DataTable dt = _customerDataAccess.GetAllResturantOrderItems();
        //    List<ResturantOrderDetail> miList = new List<ResturantOrderDetail>();
        //    miList = (from DataRow dr in dt.Rows
        //              select new ResturantOrderDetail()
        //              {
        //                  ItemId = dr["ItemId"] != DBNull.Value ? Convert.ToInt32(dr["ItemId"]) : 0,
        //                  ItemName = dr["ItemName"].ToString(),
                         
        //              }).ToList();
        //    return miList;
        //}


        //public List<ResturantOrderDetail> GetResturantOrderItemsByItemId(int ItemId)
        //{
        //    DataTable dt = _customerDataAccess.GetResturantOrderItemsByItemId(ItemId);
        //    List<ResturantOrderDetail> miList = new List<ResturantOrderDetail>();
        //    miList = (from DataRow dr in dt.Rows
        //              select new ResturantOrderDetail()
        //              {
        //                  ItemId = dr["ItemId"] != DBNull.Value ? Convert.ToInt32(dr["ItemId"]) : 0,
        //                  ItemName = dr["ItemName"].ToString(),

        //              }).ToList();
        //    return miList;
        //}
        public List<SpaiderReport> GetAllReportData(Guid CompanyId)
        {
            DataTable dt = _customerDataAccess.GetAllReportData(CompanyId);
            List<SpaiderReport> miList = new List<SpaiderReport>();
            miList = (from DataRow dr in dt.Rows
                      select new SpaiderReport()
                      {
                          FoodName = dr["ItemName"].ToString(),
                          AcceptCount = dr["AcceptCount"] != DBNull.Value ? Convert.ToInt32(dr["AcceptCount"]) : 0,
                          PendingCount = dr["PendingCount"] != DBNull.Value ? Convert.ToInt32(dr["PendingCount"]) : 0,
                          RejectCount = dr["RejectCount"] != DBNull.Value ? Convert.ToInt32(dr["RejectCount"]) : 0,

                      }).ToList();
            return miList;
        }

        public List<LineChartReport> GetLineChartReport(Guid CompanyId,DateTime StartDate,DateTime EndDate)
        {
            DataTable dt = _customerDataAccess.GetAllLineChartReportData(CompanyId, StartDate, EndDate);
            List<LineChartReport> miList = new List<LineChartReport>();
            miList = (from DataRow dr in dt.Rows
                      select new LineChartReport()
                      {
                          OrderDate = dr["OrderDate"].ToString(),
                          AcceptCount = dr["AcceptCount"] != DBNull.Value ? Convert.ToInt32(dr["AcceptCount"]) : 0,
                          PendingCount = dr["PendingCount"] != DBNull.Value ? Convert.ToInt32(dr["PendingCount"]) : 0,
                          RejectCount = dr["RejectCount"] != DBNull.Value ? Convert.ToInt32(dr["RejectCount"]) : 0,

                      }).ToList();
            return miList;
        }
        public List<PieChart> GetAllPieReportData(Guid CompanyId)
        {
            DataTable dt = _customerDataAccess.GetAllPieReportData(CompanyId);
            List<PieChart> miList = new List<PieChart>();
            miList = (from DataRow dr in dt.Rows
                      select new PieChart()
                      {
                          FoodName = dr["ItemName"].ToString(),
                          Sale = dr["Sale"] != DBNull.Value ? Convert.ToInt32(dr["Sale"]) : 0,
                          

                      }).ToList();
            return miList;
        }
        public ResturantOrder GetResturantOrderByCompanyIdAndOrderId(Guid comid, Guid orderid)
        {
            return _ResturantOrderDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and OrderId = '{1}'", comid, orderid)).FirstOrDefault();
        }

        public bool UpdateResturantOrder(ResturantOrder order)
        {
            return _ResturantOrderDataAccess.Update(order) > 0;
        }

        public ResturantOrder GetRestaurantOrderByOrderId(Guid comid, Guid orderid)
        {
            return _ResturantOrderDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and OrderId = '{1}'", comid, orderid)).FirstOrDefault();
        }

        public WebsiteLocation GetWebsiteLocationByCompanyId(Guid comid)
        {
            return _WebsiteLocationDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", comid)).FirstOrDefault();
        }

        public List<Package> GetAllPackages()
        {
            return _PackageDataAccess.GetAll();
        }

        public ResturantOrderDetail GetOrderDetailById(int value)
        {
            return _ResturantOrderDetailDataAccess.Get(value);
        }

        public bool UpdateOrderDetail(ResturantOrderDetail rod)
        {
            return _ResturantOrderDetailDataAccess.Update(rod) > 0;
        }

        #region Rugtracker API
        public List<PackageListWithIncludeAndRate> GetPackageAndInclude()
        {
            DataSet ds = _BookingDataAccess.GetPackageAndIncludeList();

            List<PackageListWithIncludeAndRate> packageList = new List<PackageListWithIncludeAndRate>();
            packageList = (from DataRow dr in ds.Tables[0].Rows
                           select new PackageListWithIncludeAndRate()
                           {
                               Id = dr["PackageId"] != DBNull.Value ? Convert.ToInt32(dr["PackageId"]) : 0,
                               PackageName = dr["PackageName"].ToString(),
                               PackageRate = dr["PackageRate"] != DBNull.Value ? Convert.ToDouble(dr["PackageRate"]) : 0,

                           }).ToList();

            List<PackageListWithIncludedItem> packageIncludeList = new List<PackageListWithIncludedItem>();
            packageIncludeList = (from DataRow dr in ds.Tables[1].Rows
                                  select new PackageListWithIncludedItem()
                                  {
                                      PackId = dr["PackId"] != DBNull.Value ? Convert.ToInt32(dr["PackId"]) : 0,
                                      PackageInclude = dr["PackageInclude"].ToString(),
                                  }).ToList();

            foreach (var item in packageList)
            {
                item.IncludedPack = String.Join(", ", packageIncludeList.Where(x => x.PackId == item.Id).Select(x => x.PackageInclude));
            }

            return packageList;
        }
        public Booking GetByBookingId(string bookingId)
        {
            return _BookingDataAccess.GetByQuery(string.Format(" BookingId = '{0}'", bookingId)).FirstOrDefault();
        }
        public bool UpdateBooking(Booking booking)
        {
            return _BookingDataAccess.Update(booking) > 0;
        }
        public bool DeleteAllBookingExtraItemByBookingId(int id)
        {
            return _BookingExtraItemDataAccess.DeleteAllBookingExtraItemByBookingId(id);
        }
        public int InsertBookingExtraItem(BookingExtraItem item)
        {
            return (int)_BookingExtraItemDataAccess.Insert(item);
        }

        public int InsertBooking(Booking item)
        {
            return (int)_BookingDataAccess.Insert(item);
        }
       
        public List<CustomerSnapshot> GetCustomerSnapshotDetail(string des)
        {
            DataTable dt = _CustomerSnapshotDataAccess.GetCustomerSnapshotDetail(des);
            List<CustomerSnapshot> Responsiblelist = new List<CustomerSnapshot>();
            Responsiblelist = (from DataRow dr in dt.Rows
                               select new CustomerSnapshot()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   CustomerId = (Guid)dr["CustomerId"],
                                   CompanyId = (Guid)dr["CompanyId"],
                                   Description = dr["Description"].ToString(),
                                   Logdate = dr["Logdate"] != DBNull.Value ? Convert.ToDateTime(dr["Logdate"]) : new DateTime(),
                                   Updatedby = dr["Updatedby"].ToString(),
                                   Type = dr["Type"].ToString()
                               }).ToList();
            return Responsiblelist;
        }
        public List<AppTicketItemModel> GetJobBookingItemsByTicketId(Guid ticketid)
        {
            DataTable dt = _TicketDataAccess.GetJobBookingItemsByTicketId(ticketid);
            List<AppTicketItemModel> model = new List<AppTicketItemModel>();
            model = (from DataRow dr in dt.Rows
                     select new AppTicketItemModel()
                     {
                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                         PackageBaseRate = dr["BaseRate"] != DBNull.Value ? Convert.ToDouble(dr["BaseRate"]) : 0,
                         packageAdditionalRate = dr["AdditionalRate"] != DBNull.Value ? Convert.ToDouble(dr["AdditionalRate"]) : 0,
                         ServiceUnitPrice = dr["UnitValue"] != DBNull.Value ? Convert.ToDouble(dr["UnitValue"]) : 0,
                         AddOnsValue = dr["AdonsValue"] != DBNull.Value ? Convert.ToDouble(dr["AdonsValue"]) : 0,
                         ServiceValue = dr["Extras"].ToString(),
                         AddOnsText = dr["Adons"].ToString(),
                         PackageText = dr["Package"].ToString(),
                         ServiceText = dr["DisplayText"].ToString(),
                         SoldBy = dr["SoldBy"].ToString(),
                         Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                         Discount = dr["Discount"] != DBNull.Value ? Convert.ToDouble(dr["Discount"]) : 0,
                         Total = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0,
                         ServicePackageValue = dr["ExtraValue"] != DBNull.Value ? Convert.ToDouble(dr["ExtraValue"]) : 0,
                         Note = dr["Comments"].ToString(),
                         TicketId = dr["TicketId"].ToString(),
                         BookingId = dr["BookingId"].ToString(),
                         TicketType = dr["ServiceType"].ToString()
                     }).ToList();
            return model;
        }
        public int GetEquipmentSearchMaxLoad(Guid CompanyId)
        {
            string InvMsg = RMRCacheKey.EquipmentSearchMaxLoad + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[InvMsg] == null)
            {

                string SearchKey = "EquipmentSearchMaxLoad";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();

                result = globalsetting == null ? "40" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[InvMsg] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[InvMsg];
            }
            return Convert.ToInt32(result);
        }

        public Invoice GetInvoiceByTicketId(Guid ticketid)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format("TicketId = '{0}'", ticketid)).FirstOrDefault();
        }
        public GlobalSetting GetSalesTax(Guid CompanyId)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("SearchKey = 'Sales Tax' and CompanyId = '{0}' and IsActive = 'true'", CompanyId)).FirstOrDefault();
        }
        public List<TicketFile> GetTicketFilesByDetailsId(int id)
        {
            return _TicketFileDataAccess.GetByQuery(" TicketBookingDetailsId = " + id);
        }

        public Ticket GetTicketById(int TicketId)
        {
            DataSet ds = _TicketDataAccess.GetTicketById(TicketId);

            Ticket Tickets = (from DataRow dr in ds.Tables[0].Rows
                              select new Ticket()
                              {
                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                  CompanyId = (Guid)dr["CompanyId"],
                                  CustomerId = (Guid)dr["CustomerId"],
                                  TicketId = (Guid)dr["TicketId"],
                                  LastUpdatedBy = (Guid)dr["LastUpdatedBy"],
                                  CreatedBy = (Guid)dr["CreatedBy"],
                                  MiscName = dr["MiscName"].ToString(),
                                  MiscValue = dr["MiscValue"] != DBNull.Value ? Convert.ToDecimal(dr["MiscValue"]) : 0,
                                  Status = dr["Status"].ToString(),
                                  TicketType = dr["TicketType"].ToString(),
                                  AssignedTo = dr["AssignedTo"].ToString(),
                                  Priority = dr["Priority"].ToString(),
                                  Subject = dr["Subject"].ToString(),
                                  LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                  CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                  CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                                  CompletedDate = dr["CompletedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletedDate"]) : new DateTime(),
                                  Message = dr["Message"].ToString(),
                                  TicketTypeVal = dr["TicketTypeVal"].ToString(),
                                  StatusVal = dr["StatusVal"].ToString(),
                                  PriorityVal = dr["PriorityVal"].ToString(),
                                  CreatedByVal = dr["CreatedByVal"].ToString(),
                                  HasInvoice = dr["HasInvoice"] != DBNull.Value ? Convert.ToBoolean(dr["HasInvoice"]) : false,
                                  HasSurvey = dr["HasSurvey"] != DBNull.Value ? Convert.ToBoolean(dr["HasSurvey"]) : false,
                                  IsClosed = dr["IsClosed"] != DBNull.Value ? Convert.ToBoolean(dr["IsClosed"]) : false,
                                  IsAgreementTicket = dr["IsAgreementTicket"] != DBNull.Value ? Convert.ToBoolean(dr["IsAgreementTicket"]) : false,
                                  IsPayrollClosed = dr["IsPayrollClosed"] != DBNull.Value ? Convert.ToBoolean(dr["IsPayrollClosed"]) : false,
                                  Signature = dr["Signature"].ToString(),
                                  IsDispatch = dr["IsDispatch"] != DBNull.Value ? Convert.ToBoolean(dr["IsDispatch"]) : false,
                                  AssignedToId = (Guid)dr["AssignedToId"],
                                  ReferenceTicketId = dr["ReferenceTicketId"] != DBNull.Value ? Convert.ToInt32(dr["ReferenceTicketId"]) : 0,
                                  BookingId = dr["BookingId"].ToString(),
                                  Reason = dr["Reason"].ToString(),
                                  RackNo = dr["RackNo"].ToString(),
                                  Locations = dr["Locations"].ToString(),
                                  RescheduleTicketId = dr["RescheduleTicketId"] != DBNull.Value ? Convert.ToInt32(dr["RescheduleTicketId"]) : 0,
                                  TicketSignatureDate = dr["TicketSignatureDate"] != DBNull.Value ? Convert.ToDateTime(dr["TicketSignatureDate"]) : new DateTime(),
                              }).FirstOrDefault();



            //return _TicketDataAccess.Get(TicketId);
            return Tickets;
        }
        public List<CustomerAppointmentEquipmentApi> GetAllCustomerAppointmentEquipListByAppointmentId(Guid appointmentid)
        {
            DataTable dt = _TicketDataAccess.GetAllCustomerAppointmentEquipmentListByAppointmentId(appointmentid);
            List<CustomerAppointmentEquipmentApi> AppointmentEquipList = new List<CustomerAppointmentEquipmentApi>();
            AppointmentEquipList = (from DataRow dr in dt.Rows
                                    select new CustomerAppointmentEquipmentApi()
                                    {
                                        id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        equipmentGuid = (Guid)dr["EquipmentId"],
                                        quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                        unitCost = dr["RepCost"] != DBNull.Value ? Convert.ToDouble(dr["RepCost"]) : 0,
                                        unitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0,
                                        totalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0,
                                        name = dr["EquipName"].ToString(),
                                        barcode = dr["Barcode"].ToString(),
                                        onHand = dr["QuantityOnHand"] != DBNull.Value ? Convert.ToInt32(dr["QuantityOnHand"]) : 0,
                                        installed = dr["QuantityLeftEquipment"] != DBNull.Value ? Convert.ToInt32(dr["QuantityLeftEquipment"]) : 0,
                                        point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0,
                                        EquipmentClassId = dr["EquipmentClassId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentClassId"]) : 0,

                                    }).ToList();
            return AppointmentEquipList;
        }
        public List<CustomerAppointmentEquipment> GetAllCustomerAppointmentEquipListByAppointmentIdAPI(Guid appointmentid)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetAllCustomerAppointmentEquipmentListByAppointmentId(appointmentid);
            List<CustomerAppointmentEquipment> AppointmentEquipList = new List<CustomerAppointmentEquipment>();
            AppointmentEquipList = (from DataRow dr in dt.Rows
                                    select new CustomerAppointmentEquipment()
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        AppointmentId = (Guid)dr["AppointmentId"],
                                        EquipmentId = (Guid)dr["EquipmentId"],
                                        Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                        SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0,
                                        RepCost = dr["RepCost"] != DBNull.Value ? Convert.ToDouble(dr["RepCost"]) : 0,
                                        Retail = dr["Retail"] != DBNull.Value ? Convert.ToDouble(dr["Retail"]) : 0,
                                        UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0,
                                        OriginalUnitPrice = dr["OriginalUnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["OriginalUnitPrice"]) : 0,
                                        TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0,
                                        CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                        CreatedBy = dr["CreatedByName"].ToString(),
                                        CreatedByUid = dr["CreatedByUid"] != DBNull.Value ? (Guid)dr["CreatedByUid"] : Guid.Empty,
                                        EquipmentServiceName = dr["EquipmentName"].ToString(),
                                        EquipName = dr["EquipName"].ToString(),
                                        EquipDetail = dr["EquipDetail"].ToString(),
                                        FileDescription = dr["FileDescription"].ToString(),
                                        FileFullName = dr["FileFullName"].ToString(),
                                        FileName = dr["Filename"].ToString(),
                                        FileType = dr["FileType"].ToString(),
                                        SKU = dr["SKU"].ToString(),
                                        IsEquipmentRelease = dr["IsEquipmentRelease"] != DBNull.Value ? Convert.ToBoolean(dr["IsEquipmentRelease"]) : false,
                                        QuantityOnHand = dr["QuantityOnHand"] != DBNull.Value ? Convert.ToInt32(dr["QuantityOnHand"]) : 0,
                                        TechnicianId = dr["TechnicianId"] != DBNull.Value ? (Guid)dr["TechnicianId"] : new Guid(),
                                        EquipmentClassId = dr["EquipmentClassId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentClassId"]) : 0,
                                        IsService = dr["IsService"] != DBNull.Value ? Convert.ToBoolean(dr["IsService"]) : false,
                                        IsAgreementItem = dr["IsAgreementItem"] != DBNull.Value ? Convert.ToBoolean(dr["IsAgreementItem"]) : false,
                                        IsBaseItem = dr["IsBaseItem"] != DBNull.Value ? Convert.ToBoolean(dr["IsBaseItem"]) : false,
                                        IsBadInventory = dr["IsBadInventory"] != DBNull.Value ? Convert.ToBoolean(dr["IsBadInventory"]) : false,
                                        IsDefaultService = dr["IsDefaultService"] != DBNull.Value ? Convert.ToBoolean(dr["IsDefaultService"]) : false,
                                        IsCheckedEquipment = dr["IsCheckedEquipment"] != DBNull.Value ? Convert.ToBoolean(dr["IsCheckedEquipment"]) : false,
                                        QuantityLeftEquipment = dr["QuantityLeftEquipment"] != DBNull.Value ? Convert.ToInt32(dr["QuantityLeftEquipment"]) : 0,
                                        IsInvoiceCreate = dr["IsInvoiceCreate"] != DBNull.Value ? Convert.ToBoolean(dr["IsInvoiceCreate"]) : false,
                                        ReferenceInvoiceId = dr["ReferenceInvoiceId"].ToString(),
                                        ReferenceInvDetailId = dr["ReferenceInvDetailId"] != DBNull.Value ? Convert.ToInt32(dr["ReferenceInvDetailId"]) : 0,
                                        IsBilling = dr["IsBilling"] != DBNull.Value ? Convert.ToBoolean(dr["IsBilling"]) : false,
                                        IsCopied = dr["IsCopied"] != DBNull.Value ? Convert.ToBoolean(dr["IsCopied"]) : false,
                                        IsEquipmentExist = dr["IsEquipmentExist"] != DBNull.Value ? Convert.ToBoolean(dr["IsEquipmentExist"]) : false,
                                        Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0,
                                        TotalPoint = dr["TotalPoint"] != DBNull.Value ? Convert.ToDouble(dr["TotalPoint"]) : 0,
                                        Equipmentvendorcost = dr["EquipmentVendorCost"] != DBNull.Value ? Convert.ToDouble(dr["EquipmentVendorCost"]) : 0,
                                        IsARBEnabled = dr["IsARBEnabled"] != DBNull.Value ? Convert.ToBoolean(dr["IsARBEnabled"]) : false,
                                        IsBillingProcess = dr["IsBillingProcess"] != DBNull.Value ? Convert.ToBoolean(dr["IsBillingProcess"]) : false,

                                    }).ToList();
            return AppointmentEquipList;
        }

        public bool DeleteAdditionalMembersAndAppointment(Guid ticketid)
        {
            return _TicketDataAccess.DeleteAdditionalMembersByTicketId(ticketid);
        }
        public long InsertTicketUser(TicketUser tu)
        {
            return _TicketUserDataAccess.Insert(tu);
        }
        public CustomerAppointment GetCustomerAppointmentByTicketId(Guid ticketid)
        {
            return _CustomerAppointmentDataAccess.GetByQuery(string.Format("AppointmentId = '{0}'", ticketid)).FirstOrDefault();
        }
        public bool UpdateTicket(Ticket ticket)
        {
            
            return _TicketDataAccess.Update(ticket) > 0;
        }
        public bool UpdateCustomerAppointment(CustomerAppointment ca)
        {
            return _CustomerAppointmentDataAccess.Update(ca) > 0;
        }
        public TicketFile GetTicketFileById(int value)
        {
            return _TicketFileDataAccess.Get(value);
        }
        public bool DeleteTicketFileById(int value)
        {
            return _TicketFileDataAccess.Delete(value) > 0;
        }
        public bool UpdateTicketFile(TicketFile tf)
        {
            return _TicketFileDataAccess.Update(tf) > 0;
        }
        public long InsertTicketFile(TicketFile tf)
        {
            return _TicketFileDataAccess.Insert(tf);
        }
        public List<TicketFile> GetJobFilesByTicketId(Guid ticketid)
        {
            DataTable dt = _TicketDataAccess.GetJobFilesByTicketId(ticketid);
            List<TicketFile> model = new List<TicketFile>();
            model = (from DataRow dr in dt.Rows
                     select new TicketFile()
                     {
                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                         TicketId = (Guid)dr["TicketId"],
                         FileAddedBy = (Guid)dr["FileAddedBy"],
                         FileName = dr["FileName"].ToString(),
                         Filesize = dr["Filesize"] != DBNull.Value ? Convert.ToInt32(dr["Filesize"]) : 0,
                         FileLocation = !string.IsNullOrWhiteSpace(dr["FileLocation"].ToString()) && dr["FileLocation"].ToString().IndexOf("http") > -1 ? dr["FileLocation"].ToString() : !string.IsNullOrWhiteSpace(dr["FileLocation"].ToString()) ? AppConfig.ImageDomain + dr["FileLocation"].ToString() : dr["FileLocation"].ToString(),
                         Description = dr["Description"].ToString(),
                         FileAddedDate = dr["FileAddedDate"] != DBNull.Value ? Convert.ToDateTime(dr["FileAddedDate"]) : new DateTime(),
                         TicketBookingDetailsId = dr["TicketBookingDetailsId"] != DBNull.Value ? Convert.ToInt32(dr["TicketBookingDetailsId"]) : 0,
                     }).ToList();
            return model;
        }
        public TicketUser GetAssignedTicketUserByTicketId(Guid ticketid)
        {
            return _TicketUserDataAccess.GetByQuery(string.Format("TiketId = '{0}' and IsPrimary = 1", ticketid)).FirstOrDefault();
        }
        public bool UpdateTicketUser(TicketUser tu)
        {
            return _TicketUserDataAccess.Update(tu) > 0;
        }
        public bool DeleteNotifyingMembersByTicketId(Guid ticketid)
        {
            return _TicketDataAccess.DeleteNotifyingMembersByTicketId(ticketid);
        }
        public Employee GetEmployeeByEmployeeId(Guid EmployeeId)
        {
            return _EmployeeDataAccess.GetByQuery(string.Format("UserId = '{0}'", EmployeeId)).FirstOrDefault();
        }
        public bool DeleteAllBookingDetailsByBookingId(string bookingId)
        {
            return _BookingDataAccess.DeleteByBookingId(bookingId);
        }
        public int InsertBookingDetails(BookingDetails item)
        {
            return (int)_BookingDetailsDataAccess.Insert(item);
        }

        public Company GetCompanyByComapnyId(Guid companyId)
        {
            return _CompanyDataAccess.GetByQuery(string.Format(" CompanyId ='{0}'", companyId)).FirstOrDefault();
        }
        //public int SaveBookingPdfFile(string fileName, string BookingNo, Guid CustomerId, Guid CompanyId, bool Signed = false)
        //{

        //    CustomerFile cf = GetCustomerFileByDescriptionAndCustomerId(BookingNo + (Signed ? "_Signed" : ""), CustomerId);
        //    if (cf == null)
        //    {
        //        CustomerFile cuf = new CustomerFile()
        //        {
        //            CompanyId = CompanyId,
        //            CustomerId = CustomerId,
        //            FileDescription = BookingNo + (Signed ? "_Signed" : ""),
        //            FileFullName = BookingNo + (Signed ? "_Signed" : "") + ".pdf",
        //            Filename = fileName.IndexOf("/") == 0 ? fileName : "/" + fileName,
        //            IsActive = true,
        //            Uploadeddate = DateTime.UtcNow,
        //        };
        //        return (int)InsertCustomerFile(cuf);
        //    }
        //    else
        //    {
        //        cf.Filename = fileName.IndexOf("/") == 0 ? fileName : "/" + fileName;
        //        return UpdateCustomerFile(cf);
        //    }
        //}

        #endregion

        #region Geese Relief API
        public int InsertCustomerCheckLog(CustomerCheckLog CC)
        {
            return (int)_CustomerCheckLogDataAccess.Insert(CC);
        }
        public bool UpdateCustomerCheckLog(CustomerCheckLog CC)
        {
            return _CustomerCheckLogDataAccess.Update(CC) > 0;
        }
        public CustomerCheckLog GetCustomerCheckLogByCustomerId(Guid CustomerId, Guid UserId)
        {
            return _CustomerCheckLogDataAccess.GetByQuery(string.Format(" CustomerId = '{0}' and UserId = '{1}' order by Id desc ", CustomerId, UserId)).FirstOrDefault();
        }
        public bool UpdateCustomerRoute(CustomerRoute CR)
        {
            return _CustomerRouteDataAccess.Update(CR) > 0;
        }
        public CustomerRoute GetCustomerRouteByCustomerId(Guid CustomerId)
        {
            var query = "CustomerId='" + CustomerId + "'";
            return _CustomerRouteDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public List<CustomerGuidIdList> GetAllCustomerIdByRouteId(Guid RouteId)
        {
            DataTable dt = _CustomerRouteDataAccess.GetAllCustomerIdByRouteId(RouteId);
            List<CustomerGuidIdList> miList = new List<CustomerGuidIdList>();
            miList = (from DataRow dr in dt.Rows
                      select new CustomerGuidIdList()
                      {
                          CustomerId = (Guid)dr["CustomerId"]
                      }).ToList();
            return miList;
        }
        public List<CustomerGuidIdList> GetAllCustomerIdByUserId(Guid UserId)
        {
            DataTable dt = _CustomerRouteDataAccess.GetAllCustomerIdByUserId(UserId);
            List<CustomerGuidIdList> miList = new List<CustomerGuidIdList>();
            miList = (from DataRow dr in dt.Rows
                      select new CustomerGuidIdList()
                      {
                          CustomerId = (Guid)dr["CustomerId"]
                      }).ToList();
            return miList;
        }
        public RouteListModel GetAllCustomerRoutes(int PageNo, int PageSize, Guid UserId)
        {
            DataSet dsResult = _CustomerRouteDataAccess.GetAllCustomerRoutes(PageNo, PageSize, UserId);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];

            List<RouteList> RouteList = new List<RouteList>();
            RecordCount RecordCount = new RecordCount();
            RouteList = (from DataRow dr in dt.Rows
                             select new RouteList()
                             {
                                 RouteId = (Guid)dr["RouteId"],
                                 Name = dr["Name"].ToString(),
                                 LastVisit = dr["LastVisit"] != DBNull.Value ? Convert.ToDateTime(dr["LastVisit"]) : new DateTime(),
                                 Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0
                             }).ToList();

            RecordCount = (from DataRow dr in dt1.Rows
                                 select new RecordCount()
                                 {
                                     TotalRecord = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                 }).FirstOrDefault();
            RouteListModel RouteListModel = new RouteListModel();
            RouteListModel.RouteList = RouteList;
            RouteListModel.Total = RecordCount;
            return RouteListModel;

        }
        public GeeseCustomerDetailModel GetAllCustomerByRouteId(Guid CustomerId, int PageNo, int PageSize)
        {
            DataSet dsResult = _CustomerRouteDataAccess.GetAllCustomerByRouteId(CustomerId, PageNo, PageSize);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            DataTable dt2 = dsResult.Tables[2];

            GeeseCustomer GeeseCustomer = new GeeseCustomer();
            GeeseCustomer = (from DataRow dr in dt.Rows
                         select new GeeseCustomer()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             CustomerId = (Guid)dr["CustomerId"],
                             Name = dr["Name"].ToString(),
                             Email = dr["Email"].ToString(),
                             Phone = dr["Phone"].ToString(),
                             TotalMediaCount = dr["TotalMedia"] != DBNull.Value ? Convert.ToInt32(dr["TotalMedia"]) : 0,
                             TotalNoteCount = dr["TotalNote"] != DBNull.Value ? Convert.ToInt32(dr["TotalNote"]) : 0,
                             GeeseLead = dr["GeeseLead"].ToString(),
                             Street = dr["Street"].ToString(),
                             City = dr["City"].ToString(),
                             State = dr["State"].ToString(),
                             Zip = dr["ZipCode"].ToString(),
                             ProfilePicture = dr["ProfileImage"].ToString(),
                             GeeseCount = dr["GeeseCount"] != DBNull.Value ? Convert.ToInt32(dr["GeeseCount"]) : 0,
                             IsCheckedIn = dr["IsCheckIn"] != DBNull.Value ? Convert.ToBoolean(dr["IsCheckIn"]) : false,
                             LastCheckedInTime = dr["LastVisit"] != DBNull.Value ? Convert.ToDateTime(dr["LastVisit"]) : new DateTime(),
                             LastMedia = dr["LastMedia"].ToString(),
                             LastNote = dr["LastNote"].ToString()
                         }).FirstOrDefault();
            List<GeeseMedia> GeeseMediaList = new List<GeeseMedia>();
            GeeseMediaList = (from DataRow dr in dt1.Rows
                              select new GeeseMedia()
                              {
                                  CustomerId = (Guid)dr["CustomerId"],
                                  Url = dr["Url"].ToString(),
                                  Notes = dr["Note"].ToString(),
                                  UploadDate = dr["UploadedDate"] != DBNull.Value ? Convert.ToDateTime(dr["UploadedDate"]) : new DateTime(),
                                  Assigner = dr["Assigner"].ToString(),
                              }).ToList();
            List<GeeseNote> GeeseNoteList = new List<GeeseNote>();
            GeeseNoteList = (from DataRow dr in dt2.Rows
                             select new GeeseNote()
                             {
                                 CustomerId = (Guid)dr["CustomerId"],
                                 Notes = dr["Note"].ToString(),
                                 Date = dr["UploadedDate"] != DBNull.Value ? Convert.ToDateTime(dr["UploadedDate"]) : new DateTime(),
                                 Assigner = dr["Assigner"].ToString(),
                             }).ToList();
            GeeseCustomerDetailModel GeeseCustomerDetailModel = new GeeseCustomerDetailModel();
            GeeseCustomerDetailModel.Customers = GeeseCustomer;
            GeeseCustomerDetailModel.Media = GeeseMediaList;
            GeeseCustomerDetailModel.Note = GeeseNoteList;
            return GeeseCustomerDetailModel;

        }
        
        public GeeseCustomerDetailHistoryModel GetCustomerHistoryByCustomerId(Guid CustomerId, DateTime? StartDate, DateTime? EndDate)
        {
            DataSet dsResult = _CustomerRouteDataAccess.GetCustomerDetailsByCustomerId(CustomerId, StartDate, EndDate);
            DataTable dt = dsResult.Tables[0];

            GeeseCustomerDetailHistoryModel GeeseCustomerDetailHistory = new GeeseCustomerDetailHistoryModel();
            
            List<GeeseCustomerDetailHistory> GeeseCustomerList = new List<GeeseCustomerDetailHistory>();
            GeeseCustomerList = (from DataRow dr in dt.Rows
                                 select new GeeseCustomerDetailHistory()
                                 {
                                     Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                     CustomerId = (Guid)dr["CustomerId"],
                                     Name = dr["Name"].ToString(),
                                     UserName = dr["UserName"].ToString(),
                                     GeeseCount = dr["GeeseCount"] != DBNull.Value ? Convert.ToInt32(dr["GeeseCount"]) : 0,
                                     IsCheckedIn = dr["IsCheckIn"] != DBNull.Value ? Convert.ToBoolean(dr["IsCheckIn"]) : false,
                                     RouteId = (Guid)dr["RouteId"],
                                     LastCheckedInTime = dr["LastVisit"] != DBNull.Value ? Convert.ToDateTime(dr["LastVisit"]) : new DateTime(),
                                     CheckInTime = dr["CheckInTime"] != DBNull.Value ? Convert.ToDateTime(dr["CheckInTime"]) : new DateTime(),
                                     CheckOutTime = dr["CheckOutTime"] != DBNull.Value ? Convert.ToDateTime(dr["CheckOutTime"]) : new DateTime()
                                 }).ToList();
            GeeseCustomerDetailHistory.GeeseCustomerList = GeeseCustomerList;

            return GeeseCustomerDetailHistory;

        }
        #endregion

        public TrackingNumberSetting GetTrackingDetailsByTrackingPhone(string phone)
        {
            return _TrackingNumberSettingDataAccess.GetByQuery(string.Format("REPLACE(TrackingNumber,'-','') = '{0}'", phone.Replace("-", ""))).FirstOrDefault();
        }

        public RestaurantCoupons GetCouponsByCompanyIdandCode(Guid companyid, string code)
        {
            return _RestaurantCouponsDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and CouponCode = '{1}'", companyid, code)).FirstOrDefault();
        }

        #region Third Party Api
        public List<CustomerSecurityZones> GetAllCustomerSecurityZoneByCustomerId(Guid CustomerId, string Platform)
        {
            DataTable dt = _CustomerSecurityZonesDataAccess.GetAllCustomerSecurityZoneByCustomerId(CustomerId, Platform);
            List<CustomerSecurityZones> SecurityZones = new List<CustomerSecurityZones>();
            SecurityZones = (from DataRow dr in dt.Rows
                             select new CustomerSecurityZones()
                             {
                                 ID = dr["ID"] != DBNull.Value ? Convert.ToInt32(dr["ID"]) : 0,
                                 ZoneNumber = dr["ZoneNumber"].ToString(),
                                 EventCodeVal = dr["EventCodeVal"].ToString(),
                                 EventCode = dr["EventCode"].ToString(),
                                 Location = dr["Location"].ToString(),
                                 NmcEqpLoc = dr["NmcEqpLoc"].ToString(),
                                 NmcEqpType = dr["NmcEqpType"].ToString(),
                                 LocationVal = dr["LocationVal"].ToString(),
                                 CustomerId = (Guid)dr["CustomerId"],
                                 Platform = dr["Platform"].ToString(),
                                 EquipmentType = dr["EquipmentType"].ToString(),
                                 EquipmentTypeVal = dr["EquipmentTypeVal"].ToString(),
                                 ZoneComment = dr["ZoneComment"].ToString()

                             }).ToList();
            return SecurityZones;
        }
        public long DeleteCustomerSecurityZone(int id)
        {
            return _CustomerSecurityZonesDataAccess.Delete(id);
        }
        public long InsertCustomerSecurityZone(CustomerSecurityZones customerZones)
        {
            return _CustomerSecurityZonesDataAccess.Insert(customerZones);
        }
        public List<CustomerThirdPartyAgency> GetAllCustomerThirdPartyAgencyByCustomerId(Guid CustomerId, string Platform)
        {
            DataTable dt = _CustomerThirdPartyAgencyDataAccess.GetAllCustomerThirdPartyAgencyByCustomerId(CustomerId, Platform);
            List<CustomerThirdPartyAgency> ThirdPartyAgency = new List<CustomerThirdPartyAgency>();
            ThirdPartyAgency = (from DataRow dr in dt.Rows
                                select new CustomerThirdPartyAgency()
                                {
                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    CustomerId = (Guid)dr["CustomerId"],
                                    AgencyName = dr["AgencyName"].ToString(),
                                    Phone = dr["Phone"].ToString(),
                                    PermitNo = dr["PermitNo"].ToString(),
                                    PermType = dr["PermType"].ToString(),
                                    AgencyNo = dr["AgencyNo"].ToString(),
                                    Agencytype = dr["Agencytype"].ToString(),
                                    Platform = dr["Platform"].ToString(),
                                    AgencytypeVal = dr["AgencytypeVal"].ToString(),
                                    PermTypeVal = dr["PermTypeVal"].ToString(),

                                }).ToList();
            return ThirdPartyAgency;
        }
        public long InsertCustomerAgency(CustomerThirdPartyAgency thirdPartyAgency)
        {
            return _CustomerThirdPartyAgencyDataAccess.Insert(thirdPartyAgency);
        }
        public long DeleteCustomerThirdPartyAgency(int id)
        {
            return _CustomerThirdPartyAgencyDataAccess.Delete(id);
        }
        public List<EmergencyContact> GetAllEmergencyContactByCustomerId(Guid customerid)
        {

            DataTable dt = _EmergencyContactDataAccess.GetAllEmergencyContactByCustomerId(customerid);
            List<EmergencyContact> contactList = new List<EmergencyContact>();
            contactList = (from DataRow dr in dt.Rows
                           select new EmergencyContact()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               CustomerId = (Guid)dr["CustomerId"],
                               FirstName = dr["FirstName"].ToString(),
                               LastName = dr["LastName"].ToString(),
                               RelationShip = dr["RelationShip"].ToString(),
                               CompanyId = (Guid)dr["CompanyId"],
                               HasKey = dr["HasKey"].ToString(),
                               RelationShipVal = dr["RelationShipVal"].ToString(),
                               Phone = dr["Phone"].ToString(),
                               Platform = dr["Platform"].ToString(),
                               PhoneType = dr["PhoneType"].ToString(),
                               Email = dr["Email"].ToString(),
                               ContactNo = dr["ContactNo"].ToString()
                           }).ToList();
            return contactList;
        }
        public SmartPackage GetPackageByPackageIdAndCompanyId(Guid PackageId, Guid CompanyId)
        {
            return _SmartPackageDataAccess.GetByQuery(string.Format("PackageId = '{0}' and CompanyId = '{1}'", PackageId, CompanyId)).FirstOrDefault();
        }
        public List<CustomerSystemNo> GetAllOpenCustomerSystemNoByCompanyIdandPlatformPrifix(Guid CompanyId, string PlatformPrifix)
        {

            DataTable dt = _CustomerSystemNoDataAccess.GetAllOpenCustomerSystemNoByCompanyIdandPlatform(CompanyId, PlatformPrifix);

            List<CustomerSystemNo> CustomerList = new List<CustomerSystemNo>();
            CustomerList = (from DataRow dr in dt.Rows
                            select new CustomerSystemNo()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                CompanyId = (Guid)dr["CompanyId"],
                                CustomerNo = dr["CustomerNo"].ToString(),
                                IsUsed = Convert.ToBoolean(dr["IsUsed"]),
                                IsReserved = Convert.ToBoolean(dr["IsReserved"]),
                                GenerateDate = dr["GenerateDate"] != DBNull.Value ? Convert.ToDateTime(dr["GenerateDate"]) : new DateTime(),
                                ReserveDate = dr["ReserveDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReserveDate"]) : new DateTime(),
                                UsedDate = dr["UsedDate"] != DBNull.Value ? Convert.ToDateTime(dr["UsedDate"]) : new DateTime(),
                                CustomerId = dr["CustomerId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerId"]) : 0
                            }).ToList();

            return CustomerList;
        }
        public CustomerNoPrefix GetAllNumberPrefixByCentralstationName(Guid comid, string PlatformName)
        {
            return _CustomerNoPrefixDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and CentralstationName='{1}'", comid, PlatformName)).ToList().FirstOrDefault();

        }
        public List<Customer> IsCustomerUccExistCheck(string UccRefId)
        {
            DataTable dt = _customerDataAccess.IsCustomerUccExistCheck(UccRefId);
            List<Customer> viewList = new List<Customer>();
            viewList = (from DataRow dr in dt.Rows
                        select new Customer()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            CustomerId = dr["CustomerId"] != DBNull.Value ? (Guid)(dr["CustomerId"]) : new Guid(),
                            FirstName = dr["FirstName"].ToString(),
                            LastName = dr["LastName"].ToString()
                        }).ToList();
            return viewList;
        }
        public CustomerSystemNo GetCusSysNoByCustomerNo(string SystmeNo)
        {
            return _CustomerSystemNoDataAccess.GetByQuery(string.Format("CustomerNO = '{0}'", SystmeNo)).FirstOrDefault();
        }
        public bool UpdateCustomerSystemNo(CustomerSystemNo customerSystemNo)
        {
            return _CustomerSystemNoDataAccess.Update(customerSystemNo) > 0;
        }
        public GlobalSetting GetGlobalSettingsByOnlyKey(string searchKey)
        {
            GlobalSetting globalSetting = _GlobalSettingDataAccess.GetByQuery(string.Format("SearchKey = '{0}'", searchKey)).FirstOrDefault();
            if (globalSetting == null)
            {
                globalSetting = new GlobalSetting();
                globalSetting.Value = string.Empty;
            }
            return globalSetting;
        }
        public ThirdPartyCustomer GetThirdPartyCustomerByCustomerId(Guid CustomerId)
        {
            return _ThirdPartyCustomerDataAccess.GetByQuery(string.Format("CustomerId ='{0}'", CustomerId)).FirstOrDefault();
        }
        public long UpdateThirdPartyCustomer(ThirdPartyCustomer customer)
        {
            return _ThirdPartyCustomerDataAccess.Update(customer);
        }
        public long InsertThirdPartyCustomer(ThirdPartyCustomer customer)
        {
            return _ThirdPartyCustomerDataAccess.Insert(customer);
        }
        public bool DeleteEmergencyContactById(int id)
        {
            return _EmergencyContactDataAccess.Delete(id) > 0;
        }
        public long InsertEmergencyContact(EmergencyContact Emergency)
        {
            return _EmergencyContactDataAccess.Insert(Emergency);
        }
        public Ticket GetInstallationTicketByCustomerId(Guid customerid)
        {
            return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and TicketType = 'Installation'", customerid)).FirstOrDefault();
        }
        public List<Ticket> GetTicketListByCustomerIdAndCompanyIdAndNotCompleted(Guid customerid, Guid companyid)
        {
            return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}' and [Status] != 'Completed'", customerid, companyid)).ToList();
        }
        public List<CustomerAppointmentEquipment> GetAllCustomerAppointmentEquipmentByAppointmentIdAndIsBilling(Guid appid)
        {
            return _CustomerAppointmentEquipmentDataAccess.GetByQuery(string.Format("AppointmentId = '{0}' and IsBilling = 1", appid)).ToList();
        }
        public List<CustomerAppointmentEquipment> GetCustomerAppointmentEquipmentListByAppointmentId(Guid appointid)
        {
            return _CustomerAppointmentEquipmentDataAccess.GetByQuery(string.Format("AppointmentId = '{0}'", appointid)).ToList();
        }
        public List<CustomerAppointmentEquipment> GetAllCustomerAppointmentEquipmentByTicketId(Guid companyid, Guid TicketId)
        {
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetAllCustomerAppointmentEquipmentByTicketId(companyid, TicketId);
            List<CustomerAppointmentEquipment> CustomerAppointmentEquipmentList = new List<CustomerAppointmentEquipment>();
            CustomerAppointmentEquipmentList = (from DataRow dr in dt.Rows
                                                select new CustomerAppointmentEquipment()
                                                {
                                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                    AppointmentId = dr["AppointmentId"] != DBNull.Value ? (Guid)dr["AppointmentId"] : Guid.Empty,
                                                    EquipmentId = dr["AppointmentId"] != DBNull.Value ? (Guid)dr["EquipmentId"] : Guid.Empty,
                                                    Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                                    UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0,
                                                    OriginalUnitPrice = dr["OriginalUnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["OriginalUnitPrice"]) : 0,
                                                    TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0,
                                                    QuantityLeftEquipment = dr["QuantityLeftEquipment"] != DBNull.Value ? Convert.ToInt32(dr["QuantityLeftEquipment"]) : 0,
                                                    CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                                    CreatedBy = dr["CreatedBy"].ToString(),
                                                    EquipName = dr["EquipName"].ToString(),
                                                    EquipDetail = dr["EquipDetail"].ToString(),
                                                    IsEquipmentRelease = dr["IsEquipmentRelease"] != DBNull.Value ? Convert.ToBoolean(dr["IsEquipmentRelease"]) : false,
                                                    EquipmentClassId = dr["EquipmentClassId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentClassId"]) : 0,
                                                    IsService = dr["IsService"] != DBNull.Value ? Convert.ToBoolean(dr["IsService"]) : false,
                                                    CreatedByUid = dr["CreatedByUid"] != DBNull.Value ? (Guid)dr["CreatedByUid"] : Guid.Empty,
                                                    InstalledByUid = dr["InstalledByUid"] != DBNull.Value ? (Guid)dr["InstalledByUid"] : Guid.Empty,
                                                    IsAgreementItem = dr["IsAgreementItem"] != DBNull.Value ? Convert.ToBoolean(dr["IsAgreementItem"]) : false,
                                                    IsDefaultService = dr["IsDefaultService"] != DBNull.Value ? Convert.ToBoolean(dr["IsDefaultService"]) : false,
                                                    IsBaseItem = dr["IsBaseItem"] != DBNull.Value ? Convert.ToBoolean(dr["IsBaseItem"]) : false,
                                                    IsBadInventory = dr["IsBadInventory"] != DBNull.Value ? Convert.ToBoolean(dr["IsBadInventory"]) : false,
                                                    IsEquipmentExist = dr["IsEquipmentExist"] != DBNull.Value ? Convert.ToBoolean(dr["IsEquipmentExist"]) : false,
                                                    IsNonCommissionable = dr["IsNonCommissionable"] != DBNull.Value ? Convert.ToBoolean(dr["IsNonCommissionable"]) : false,
                                                    IsARBEnabled = dr["IsARBEnabled"] != DBNull.Value ? Convert.ToBoolean(dr["IsARBEnabled"]) : false
                                                }).ToList();
            return CustomerAppointmentEquipmentList;
        }
        public CustomerAppointmentEquipment GetAppoinmentEquipmentByAppoinmentIdAndEquipmentIdAndId(Guid appoinmentId, Guid equipmentId, int Id)
        {
            return _CustomerAppointmentEquipmentDataAccess.GetByQuery(string.Format("EquipmentId = '{0}' and AppointmentId ='{1}' and Id = {2}", equipmentId, appoinmentId, Id)).FirstOrDefault();
        }
        public TicketUser GetTicketUserByTicketIdAndPrimary(Guid ticketid)
        {
            return _TicketUserDataAccess.GetByQuery(string.Format("TiketId = '{0}' and NotificationOnly = 0 and IsPrimary = 1", ticketid)).FirstOrDefault();
        }
        public List<InventoryTech> GetAllInventoryTechByTechnicianIdAndEquipmentIdAndType(Guid techid, Guid equipid)
        {
            return _InventoryTechDataAccess.GetByQuery(string.Format("TechnicianId = '{0}' and EquipmentId = '{1}'", techid, equipid)).ToList();
        }
        
        public bool DeleteInventoryTechByCustomerAppointmentEquipmentIdAndType(int value)
        {
            return _InventoryTechDataAccess.DeleteInventoryTechByCustomerAppointmentEquipmentIdAndType(value);
        }
        public int InsertInventoryTech(InventoryTech inventoryTech)
        {
            return (int)_InventoryTechDataAccess.Insert(inventoryTech);
        }
        public PayrollBrinks GetPayrollBrinksByTicketId(Guid ticketId)
        {
            string query = string.Format("TicketId='{0}'", ticketId);
            return _PayrollBrinksDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public SalesPay GetSalesPayCalculationByTicketId(Guid ticketId)
        {
            DataTable dt = _EmployeeDataAccess.GetSalesPayCalculationByTicketId(ticketId);
            List<SalesPay> salesPay = new List<SalesPay>();
            salesPay = (from DataRow dr in dt.Rows
                        select new SalesPay()
                        {
                            TotalMultiple = dr["TotalMultiple"] != DBNull.Value ? Convert.ToDouble(dr["TotalMultiple"]) : 0,
                            Deductions = dr["Deductions"] != DBNull.Value ? Convert.ToDouble(dr["Deductions"]) : 0,
                            PassThrus = dr["PassThrus"] != DBNull.Value ? Convert.ToDouble(dr["PassThrus"]) : 0,
                            HoldBack = dr["HoldBack"] != DBNull.Value ? Convert.ToDouble(dr["HoldBack"]) : 0,
                            TermSheetId = (Guid)dr["TermSheetId"]
                        }).ToList();
            return salesPay.FirstOrDefault();
        }
        public bool UpdatePayrollBrinks(PayrollBrinks payrollBrinks)
        {
            return _PayrollBrinksDataAccess.Update(payrollBrinks) > 0;
        }
        public long InsertPayrollBrinks(PayrollBrinks payrollBrinks)
        {
            return _PayrollBrinksDataAccess.Insert(payrollBrinks);
        }
        public List<AlarmCustomerSelectedAddon> GetAllCutomerAlarmAddonsByCustomerId(Guid CustomerId)
        {
            return _AlarmCustomerSelectedAddonDataAccess.GetByQuery(string.Format("[CustomerId]='{0}'", CustomerId)).ToList();
        }
        public long DeleteCutomerAlarmAddons(int id)
        {
            return _AlarmCustomerSelectedAddonDataAccess.Delete(id);
        }
        public SetupAlarm GetSetupalarmByCustomerId(Guid customerId)
        {
            return _SetupAlarmDataAccess.GetByQuery(string.Format("CustomerId='{0}'", customerId)).FirstOrDefault();
        }
        public long DeleteSetupAlarm(int id)
        {
            return _SetupAlarmDataAccess.Delete(id);
        }
        #endregion
        #region Alarm Api
        public List<Customer> IsCustomerAlarmIdExistCheck(int AlarmRefId)
        {

            DataTable dt = _customerDataAccess.IsCustomerAlarmIdExistCheck(AlarmRefId);

            List<Customer> viewList = new List<Customer>();
            viewList = (from DataRow dr in dt.Rows
                        select new Customer()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            CustomerId = dr["CustomerId"] != DBNull.Value ? (Guid)(dr["CustomerId"]) : new Guid(),
                            FirstName = dr["FirstName"].ToString(),
                            LastName = dr["LastName"].ToString()
                        }).ToList();
            return viewList;
        }
        public AlarmAddOnns GeAddOnnsByName(string AddonName)
        {
            return _AlarmAddOnnsDataAccess.GetByQuery(string.Format("Name='{0}'", AddonName)).FirstOrDefault();
        }
        public long InsertCutomerAlarmAddons(AlarmCustomerSelectedAddon addon)
        {
            return _AlarmCustomerSelectedAddonDataAccess.Insert(addon);
        }
        public long InsertCustomerExtended(CustomerExtended customer)
        {
            return _CustomerExtendedDataAccess.Insert(customer);
        }
        public List<AlarmAddOnns> GetAllAddOnns()
        {
            return _AlarmAddOnnsDataAccess.GetAll();
        }
        public int InsertAlarmAddOnns(AlarmAddOnns addonns)
        {
            return (int)_AlarmAddOnnsDataAccess.Insert(addonns);
        }
        public List<CustomerSystemNo> GetAllReservedCustomerSystemNoByCustomerId(int CustomerId)
        {
            return _CustomerSystemNoDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and IsReserved = 1", CustomerId));
        }
        public int InsertSetupAlarm(SetupAlarm SetupAlarm)
        {
            return (int)_SetupAlarmDataAccess.Insert(SetupAlarm);
        }
        public bool UpdateSetupAlarm(SetupAlarm SetupAlarm)
        {
            return _SetupAlarmDataAccess.Update(SetupAlarm) > 0;
        }
        public Customer GetDirectCustomerByAlarmRefId(string CustomerId)
        {
            return _customerDataAccess.GetByQuery(string.Format("Alarmrefid ='{0}'", CustomerId)).FirstOrDefault(); ;
        }
        public CustomerSystemNo GetCustomerSystemNoObjectByNumberAndCompanyId(string CustomerNo, Guid CompanyId)
        {
            return _CustomerSystemNoDataAccess.GetByQuery(string.Format("CustomerNo='{0}' AND CompanyId = '{1}'", CustomerNo, CompanyId)).FirstOrDefault();
        }
        public long InsertCustomerTermination(AlarmCustomerTermination cusTerm)
        {
            return _TerminationDataAccess.Insert(cusTerm);
        }
        public List<AlarmCustomerTerminationViewModel> GetAlarmTerminationHistoryByCustomerId(Guid customerId)
        {
            DataTable dt = _TerminationDataAccess.GetAllAlarTerminationLogByCusId(customerId);
            List<AlarmCustomerTerminationViewModel> AlarmCustomerTerminationList = new List<AlarmCustomerTerminationViewModel>();
            AlarmCustomerTerminationList = (from DataRow dr in dt.Rows
                                            select new AlarmCustomerTerminationViewModel()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                AlarmId = dr["AlarmId"] != DBNull.Value ? Convert.ToInt32(dr["AlarmId"]) : 0,
                                                TerminationReason = dr["TerminationReason"].ToString(),
                                                TerminationBy = dr["TerminationBy"].ToString(),
                                                CustomerId = (Guid)dr["CustomerId"],
                                                TerminationDate = dr["TerminationDate"] != DBNull.Value ? Convert.ToDateTime(dr["TerminationDate"]) : new DateTime()
                                            }).ToList();
            return AlarmCustomerTerminationList;
        }
        #endregion
    }
}

