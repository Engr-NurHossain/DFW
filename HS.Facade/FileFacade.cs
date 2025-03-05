//[Shariful-30-9-19]
using HS.DataAccess;
using HS.Entities;
using HS.Entities.Custom;
using HS.Framework;
using HS.Framework.Utils;
using HS.SMS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HS.Facade
{
    public class FileFacade : BaseFacade
    {
        FileTemplateDataAccess _FileTemplateDataAccess = null;
        GlobalSettingDataAccess _GlobalSettingDataAccess = null;
        ContractAgreementTemplateDataAccess _ContractAgreementTemplateDataAccess = null;
        CustomerAgreementTemplateDataAccess _CustomerAgreementTemplateDataAccess = null;

        public FileFacade(ClientContext clientContext)
            : base(clientContext)
        {
            if (_FileTemplateDataAccess == null)
                _FileTemplateDataAccess = (FileTemplateDataAccess)_ClientContext[typeof(FileTemplateDataAccess)];
            if (_GlobalSettingDataAccess == null)
                _GlobalSettingDataAccess = (GlobalSettingDataAccess)_ClientContext[typeof(GlobalSettingDataAccess)];
            if (_ContractAgreementTemplateDataAccess == null)
                _ContractAgreementTemplateDataAccess = (ContractAgreementTemplateDataAccess)_ClientContext[typeof(ContractAgreementTemplateDataAccess)];
            if (_CustomerAgreementTemplateDataAccess == null)
                _CustomerAgreementTemplateDataAccess = (CustomerAgreementTemplateDataAccess)_ClientContext[typeof(CustomerAgreementTemplateDataAccess)];
        }
        public FileFacade(string ConStr)
        {
            if (_FileTemplateDataAccess == null)
                _FileTemplateDataAccess = new FileTemplateDataAccess(ConStr);
            if (_GlobalSettingDataAccess == null)
                _GlobalSettingDataAccess = new GlobalSettingDataAccess(ConStr);
            if (_ContractAgreementTemplateDataAccess == null)
                _ContractAgreementTemplateDataAccess = new ContractAgreementTemplateDataAccess(ConStr);
            if (_CustomerAgreementTemplateDataAccess == null)
                _CustomerAgreementTemplateDataAccess = new CustomerAgreementTemplateDataAccess(ConStr);
        }
        PaymentInfoDataAccess _PaymentInfoDataAccess
        {
            get
            {
                return (PaymentInfoDataAccess)_ClientContext[typeof(PaymentInfoDataAccess)];
            }
        }
        PaymentInfoCustomerDataAccess _PaymentInfoCustomerDataAccess
        {
            get
            {
                return (PaymentInfoCustomerDataAccess)_ClientContext[typeof(PaymentInfoCustomerDataAccess)];
            }
        }

        PaymentProfileCustomerDataAccess _PaymentProfileCustomerDataAccess
        {
            get
            {
                return (PaymentProfileCustomerDataAccess)_ClientContext[typeof(PaymentProfileCustomerDataAccess)];
            }
        }
        public FileTemplate GetFileTemplateById(int id)
        {
            return _FileTemplateDataAccess.Get(id);
        }
        public FileTemplate GetFileTemplateByName(string fName)
        {
            return _FileTemplateDataAccess.GetByQuery(string.Format("FileName='{0}'", fName)).FirstOrDefault();
        }
        public List<FileTemplate> GetAllTemplate()
        {
            return _FileTemplateDataAccess.GetAll();
        }
        public bool DeleteFileManagementFile(int id)
        {
            return _FileTemplateDataAccess.Delete(id) > 0;
        }
        public List<FileTemplate> GetAllTemplateForDropdown()
        {
            DataTable dt = _FileTemplateDataAccess.GetAllTemplateForDropdown();
            List<FileTemplate> FileTemplateList = new List<FileTemplate>();

            FileTemplateList = (from DataRow dr in dt.Rows
                                select new FileTemplate()
                                {
                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    FileName = dr["FileName"].ToString(),
                                }).ToList();



            return FileTemplateList;
        }
        public List<FileTemplate> GetTemplateWithoutPermissionForDropdown()
        {
            DataTable dt = _FileTemplateDataAccess.GetTemplateWithoutPermissionForDropdown();
            List<FileTemplate> FileTemplateList = new List<FileTemplate>();

            FileTemplateList = (from DataRow dr in dt.Rows
                                select new FileTemplate()
                                {
                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    FileName = dr["FileName"].ToString(),
                                }).ToList();



            return FileTemplateList;
        }
    
        public  string Format(string Num)
        {
            string FormatedPhoneNo = "";
            if (Num.Length == 10)
            {
                FormatedPhoneNo = string.Format("({0}) {1}-{2}", Num.Substring(0, 3), Num.Substring(3, 3), Num.Substring(6));
            }
            else
            {
                FormatedPhoneNo = Num;
            }
            return string.IsNullOrWhiteSpace(FormatedPhoneNo) ? "" : FormatedPhoneNo;
        }
        
        public string MakeFileTemplatePdf(FileTemplateWithCustomerInfo ft)
        {
            string Body = "";
            try
            {
                Hashtable templateVars = new Hashtable();
                if (ft.cusAgreementTemplate.Name == "3 Day ROR - NOC")
                {
                    templateVars.Add("CompanyLogo", ft.Company.CompanyLogo);
                    templateVars.Add("CompanyStreet", ft.Company.Street);
                    templateVars.Add("CompanyCity", ft.Company.City);
                    templateVars.Add("CompanyState", ft.Company.State);
                    templateVars.Add("CompanyZipCode", ft.Company.ZipCode);
                    templateVars.Add("FirstName", ft.customerInfo.FirstName);
                    templateVars.Add("LastName", ft.customerInfo.LastName);
                    templateVars.Add("BusinessName", ft.customerInfo.BusinessName);
                    templateVars.Add("DisplayName", ft.customerInfo.DisplayName);
                    templateVars.Add("InstallAddress", ft.InstallationAddress.ToString());
                    templateVars.Add("CustomerSignatureDate", ft.FileManagementCustomerSignatureDateVal != null && ft.FileManagementCustomerSignatureDateVal != new DateTime() ? ft.FileManagementCustomerSignatureDateVal.ToString("M/dd/yy") : "");
                    if (!string.IsNullOrWhiteSpace(ft.FileManagementCustomerSignature) && ft.FileManagementCustomerSignature != "")
                    {
                        templateVars.Add("CustomerSignature", ft.FileManagementCustomerSignature);
                    }
                    //templateVars.Add("InstallAddressCity", ft.customerInfo.City);
                    //templateVars.Add("InstallAddressState", ft.customerInfo.State);
                    //templateVars.Add("InstallAddressZip", ft.customerInfo.ZipCode);
                    templateVars.Add("TicketPrimaryAppointmentStartDate", ft.customerInfo.JoinDate.Value.ToString("MM/dd/yy"));
                }
                else if (ft.cusAgreementTemplate.Name == "Cancellation")
                {
                    templateVars.Add("CompanyLogo", ft.Company.CompanyLogo);
                    templateVars.Add("GenerationDate", ft.customerInfo.CreatedDate.ToString("MM/dd/yy"));
                    templateVars.Add("FirstName", ft.customerInfo.FirstName);
                    templateVars.Add("LastName", ft.customerInfo.LastName);
                    templateVars.Add("DisplayName", ft.customerInfo.DisplayName);
                    templateVars.Add("InstallAddress", ft.InstallationAddress);
                    templateVars.Add("TermDate", ft.CancellationDate);
                    templateVars.Add("RemainingBalance", ft.RemainingBalance);
                    templateVars.Add("CustomerSignatureDate", ft.FileManagementCustomerSignatureDateVal != null && ft.FileManagementCustomerSignatureDateVal != new DateTime() ? ft.FileManagementCustomerSignatureDateVal.ToString("M/dd/yy") : "");
                    if (!string.IsNullOrWhiteSpace(ft.FileManagementCustomerSignature) && ft.FileManagementCustomerSignature != "")
                    {
                        templateVars.Add("CustomerSignature", ft.FileManagementCustomerSignature);
                    }
                }
                else if (ft.cusAgreementTemplate.Name == "Emergency Notification")
                {
                    templateVars.Add("CompanyLogo", ft.Company.CompanyLogo);
                    templateVars.Add("GenerationDate", ft.customerInfo.CreatedDate.ToString("MM/dd/yy"));
                    templateVars.Add("CompanyStreet", ft.Company.Street);
                    templateVars.Add("CompanyCity", ft.Company.City);
                    templateVars.Add("CompanyState", ft.Company.State);
                    templateVars.Add("CompanyZipCode", ft.Company.ZipCode);
                    templateVars.Add("FirstName", ft.customerInfo.FirstName);
                    templateVars.Add("LastName", ft.customerInfo.LastName);
                    templateVars.Add("DisplayName", ft.customerInfo.DisplayName);
                    templateVars.Add("MonitoringAbortCode", ft.customerInfo.Passcode);
                    templateVars.Add("InstallAddress", ft.InstallationAddress);
                    templateVars.Add("CustomerSignatureDate", ft.FileManagementCustomerSignatureDateVal != null && ft.FileManagementCustomerSignatureDateVal != new DateTime() ? ft.FileManagementCustomerSignatureDateVal.ToString("M/dd/yy") : "");
                    if (!string.IsNullOrWhiteSpace(ft.FileManagementCustomerSignature) && ft.FileManagementCustomerSignature != "")
                    {
                        templateVars.Add("CustomerSignature", ft.FileManagementCustomerSignature);
                    }
                    //templateVars.Add("InstallAddressCity", ft.customerInfo.City);
                    //templateVars.Add("InstallAddressState", ft.customerInfo.State);
                    //templateVars.Add("InstallAddressZip", ft.customerInfo.ZipCode);
                    //templateVars.Add("MonitoringEc1Name", ft.);
                    //templateVars.Add("MonitoringEc1Phone", ft.);
                    //templateVars.Add("MonitoringEc1PhoneType", ft.);
                    //templateVars.Add("MonitoringEc2Name", ft);
                    //templateVars.Add("MonitoringEc2Phone", ft.);
                    //templateVars.Add("MonitoringEc2PhoneType", ft.);
                    //templateVars.Add("MonitoringEc3Name", ft);
                    //templateVars.Add("MonitoringEc3Phone", ft.);
                    //templateVars.Add("MonitoringEc3PhoneType", ft.);
                }
                else if (ft.cusAgreementTemplate.Name == "Installation Completion Checklist")
                {
                    templateVars.Add("CompanyLogo", ft.Company.CompanyLogo);
                    templateVars.Add("FirstName", ft.customerInfo.FirstName);
                    templateVars.Add("LastName", ft.customerInfo.LastName);
                    templateVars.Add("DisplayName", ft.customerInfo.DisplayName);
                    templateVars.Add("ContactNumber", ft.customerInfo.PrimaryPhone);
                    templateVars.Add("TicketPrimaryAppointmentStartDate", ft.customerInfo.JoinDate.Value.ToString("MM/dd/yy"));
                    templateVars.Add("CustomerSignatureDate", ft.FileManagementCustomerSignatureDateVal != null && ft.FileManagementCustomerSignatureDateVal != new DateTime() ? ft.FileManagementCustomerSignatureDateVal.ToString("M/dd/yy") : "");
                    if (!string.IsNullOrWhiteSpace(ft.FileManagementCustomerSignature) && ft.FileManagementCustomerSignature != "")
                    {
                        templateVars.Add("CustomerSignature", ft.FileManagementCustomerSignature);
                    }
                    //templateVars.Add("TicketSalesRepName", ft.);
                    //templateVars.Add("TicketLeadTech", ft.);
                    //templateVars.Add("WoActualAllDetailsWithExtPrice", ft.);
                }
                else if (ft.cusAgreementTemplate.Name == "Insurance Certificate")
                {
                    templateVars.Add("CompanyLogo", ft.Company.CompanyLogo);
                    templateVars.Add("FirstName", ft.customerInfo.FirstName);
                    templateVars.Add("LastName", ft.customerInfo.LastName);
                    templateVars.Add("DisplayName", ft.customerInfo.DisplayName);
                    templateVars.Add("InstallAddress", ft.InstallationAddress);
                    templateVars.Add("CompanySignature", ft.CompanySignature);
                    templateVars.Add("GenerationDate", ft.customerInfo.CreatedDate.ToString("MM/dd/yy"));
                    templateVars.Add("PrimaryApptComplete", ft.customerInfo.JoinDate.Value.ToString("MM/dd/yy"));
                    templateVars.Add("CustomerSignatureDate", ft.FileManagementCustomerSignatureDateVal != null && ft.FileManagementCustomerSignatureDateVal != new DateTime() ? ft.FileManagementCustomerSignatureDateVal.ToString("M/dd/yy") : "");
                    if (!string.IsNullOrWhiteSpace(ft.FileManagementCustomerSignature) && ft.FileManagementCustomerSignature != "")
                    {
                        templateVars.Add("CustomerSignature", ft.FileManagementCustomerSignature);
                    }
                    //templateVars.Add("SalesRepName", ft.);
                }
                else if (ft.cusAgreementTemplate.Name == "Service Call Completion Checklist")
                {
                    templateVars.Add("CompanyLogo", ft.Company.CompanyLogo);
                    templateVars.Add("FirstName", ft.customerInfo.FirstName);
                    templateVars.Add("LastName", ft.customerInfo.LastName);
                    templateVars.Add("DisplayName", ft.customerInfo.DisplayName);
                    templateVars.Add("ContactNumber", ft.customerInfo.PrimaryPhone);
                    templateVars.Add("TicketPrimaryAppointmentStartDate", ft.customerInfo.JoinDate.Value.ToString("MM/dd/yy"));
                    templateVars.Add("CustomerSignatureDate", ft.FileManagementCustomerSignatureDateVal != null && ft.FileManagementCustomerSignatureDateVal != new DateTime() ? ft.FileManagementCustomerSignatureDateVal.ToString("M/dd/yy") : "");
                    if (!string.IsNullOrWhiteSpace(ft.FileManagementCustomerSignature) && ft.FileManagementCustomerSignature != "")
                    {
                        templateVars.Add("CustomerSignature", ft.FileManagementCustomerSignature);
                    }
                    //templateVars.Add("TicketSalesRepName", ft.);
                    //templateVars.Add("TicketLeadTech", ft.);
                    //templateVars.Add("WoActualSummaryWithTotalCustomerPrice", ft.);
                }
                else if (ft.cusAgreementTemplate.Name == "Service Suspension")
                {
                    templateVars.Add("CompanyLogo", ft.Company.CompanyLogo);
                    templateVars.Add("CompanyAddress", ft.Company.Address);
                    //templateVars.Add("CompanyCity", ft.Company.City);
                    //templateVars.Add("CompanyState", ft.Company.State);
                    //templateVars.Add("CompanyZip", ft.Company.ZipCode);
                    templateVars.Add("CompanyPhone", ft.Company.Phone);
                    templateVars.Add("FirstName", ft.customerInfo.FirstName);
                    templateVars.Add("LastName", ft.customerInfo.LastName);
                    templateVars.Add("DisplayName", ft.customerInfo.DisplayName);
                    templateVars.Add("BusinessName", ft.customerInfo.BusinessName);
                    templateVars.Add("InstallAddress", ft.InstallationAddress);
                    //templateVars.Add("InstallAddressCity", ft.customerInfo.City);
                    //templateVars.Add("installaddressstate", ft.customerInfo.State);
                    //templateVars.Add("installaddresszip", ft.customerInfo.ZipCode);
                    templateVars.Add("Phone1", ft.customerInfo.PrimaryPhone);
                    templateVars.Add("Phone1Type", ft.customerInfo.PhoneType);
                    templateVars.Add("Phone2", ft.customerInfo.SecondaryPhone);
                    templateVars.Add("Phone2Type", ft.customerInfo.PhoneType);
                    templateVars.Add("Email", ft.customerInfo.EmailAddress);
                    templateVars.Add("CustomerSignatureDate", ft.FileManagementCustomerSignatureDateVal != null && ft.FileManagementCustomerSignatureDateVal != new DateTime() ? ft.FileManagementCustomerSignatureDateVal.ToString("M/dd/yy") : "");
                    if (!string.IsNullOrWhiteSpace(ft.FileManagementCustomerSignature) && ft.FileManagementCustomerSignature != "")
                    {
                        templateVars.Add("CustomerSignature", ft.FileManagementCustomerSignature);
                    }
                    //templateVars.Add("RMRAmount", ft.customerInfo.);
                }
                else if (ft.cusAgreementTemplate.Name == "Commercial Multipurpose Addendum to Agreement")
                {
                    templateVars.Add("CompanyLogo", ft.Company.CompanyLogo);
                    templateVars.Add("FirstName", ft.customerInfo.FirstName);
                    templateVars.Add("LastName", ft.customerInfo.LastName);
                    templateVars.Add("DisplayName", ft.customerInfo.DisplayName);
                    //templateVars.Add("ContactNumber", ft.customerInfo.PrimaryPhone);
                    //templateVars.Add("WorkOrderPrimaryAppointmentStartDate", ft.customerInfo.JoinDate.Value.ToString("MM/dd/yy"));
                    templateVars.Add("DateOfAgreement", DateTime.Now.DateFormat());
                    //templateVars.Add("Date", DateTime.Now.ToString("dd"));
                    //templateVars.Add("Month", DateTime.Now.ToString("MMMM"));
                    //templateVars.Add("Year", DateTime.Now.ToString("yyyy"));
                    templateVars.Add("CustomerSignatureDate", ft.FileManagementCustomerSignatureDateVal != null && ft.FileManagementCustomerSignatureDateVal != new DateTime() ? ft.FileManagementCustomerSignatureDateVal.ToString("M/dd/yy") : "");
                    if (!string.IsNullOrWhiteSpace(ft.FileManagementCustomerSignature) && ft.FileManagementCustomerSignature != "")
                    {
                        templateVars.Add("CustomerSignature", ft.FileManagementCustomerSignature);
                    }
                    //templateVars.Add("WorkOrderSalesRepName", ft.);
                    //templateVars.Add("WorkOrderLeadTech", ft.);
                    //templateVars.Add("WoActualAllDetailsWithExtPrice", ft.);
                }
                else if (ft.cusAgreementTemplate.Name == "DSS Commercial Contract")
                {
                    templateVars.Add("CompanyLogo", ft.Company.CompanyLogo);
                    templateVars.Add("FirstName", ft.customerInfo.FirstName);
                    templateVars.Add("LastName", ft.customerInfo.LastName);
                    templateVars.Add("DisplayName", ft.customerInfo.DisplayName);
                    templateVars.Add("InstallAddress", ft.InstallationAddress);
                    templateVars.Add("InstallAddressCity", ft.customerInfo.City);
                    //templateVars.Add("installaddressstate", ft.customerInfo.State);
                    templateVars.Add("InstallAddressZip", ft.customerInfo.ZipCode);
                    templateVars.Add("ContactNumber", ft.customerInfo.PrimaryPhone);
                    templateVars.Add("Phone2", ft.customerInfo.SecondaryPhone);
                    templateVars.Add("Email", ft.customerInfo.EmailAddress);
                    //templateVars.Add("WorkOrderPrimaryAppointmentStartDate", ft.customerInfo.JoinDate.Value.ToString("MM/dd/yy"));
                    //templateVars.Add("DateOfAgreement", DateTime.Now.ToString("MM/dd/yyyy"));
                    templateVars.Add("Day", DateTime.Now.DateFormat("day"));
                    templateVars.Add("Month", DateTime.Now.DateFormat("monthName"));
                    templateVars.Add("Year", DateTime.Now.DateFormat("year"));
                    templateVars.Add("CustomerSignatureDate", ft.FileManagementCustomerSignatureDateVal != null && ft.FileManagementCustomerSignatureDateVal != new DateTime() ? ft.FileManagementCustomerSignatureDateVal.ToString("M/dd/yy") : "");
                    if (!string.IsNullOrWhiteSpace(ft.FileManagementCustomerSignature) && ft.FileManagementCustomerSignature != "")
                    {
                        templateVars.Add("CustomerSignature", ft.FileManagementCustomerSignature);
                    }
                    //templateVars.Add("WorkOrderSalesRepName", ft.);
                    //templateVars.Add("WorkOrderLeadTech", ft.);
                    //templateVars.Add("WoActualAllDetailsWithExtPrice", ft.);
                }
                else if (ft.cusAgreementTemplate.Name == "DSS Residential Contract")
                {
                    templateVars.Add("CompanyLogo", ft.Company.CompanyLogo);
                    templateVars.Add("FirstName", ft.customerInfo.FirstName);
                    templateVars.Add("LastName", ft.customerInfo.LastName);
                    templateVars.Add("DisplayName", ft.customerInfo.DisplayName);
                    templateVars.Add("InstallAddress", ft.InstallationAddress);
                    //templateVars.Add("InstallAddressCity", ft.customerInfo.City);
                    //templateVars.Add("installaddressstate", ft.customerInfo.State);
                    //templateVars.Add("installaddresszip", ft.customerInfo.ZipCode);
                    templateVars.Add("ContactNumber", ft.customerInfo.PrimaryPhone);
                    //templateVars.Add("Phone2", ft.customerInfo.SecondaryPhone);
                    //templateVars.Add("Email", ft.customerInfo.EmailAddress);
                    //templateVars.Add("WorkOrderPrimaryAppointmentStartDate", ft.customerInfo.JoinDate.Value.ToString("MM/dd/yy"));
                    //templateVars.Add("DateOfAgreement", DateTime.Now.ToString("MM/dd/yyyy"));
                    templateVars.Add("Day", DateTime.Now.DateFormat("day"));
                    templateVars.Add("Month", DateTime.Now.DateFormat("monthName"));
                    templateVars.Add("Year", DateTime.Now.DateFormat("year"));
                    templateVars.Add("CustomerSignatureDate", ft.FileManagementCustomerSignatureDateVal != null && ft.FileManagementCustomerSignatureDateVal != new DateTime() ? ft.FileManagementCustomerSignatureDateVal.ToString("M/dd/yy") : "");
                    if (!string.IsNullOrWhiteSpace(ft.FileManagementCustomerSignature) && ft.FileManagementCustomerSignature != "")
                    {
                        templateVars.Add("CustomerSignature", ft.FileManagementCustomerSignature);
                    }
                    //templateVars.Add("WorkOrderSalesRepName", ft.);
                    //templateVars.Add("WorkOrderLeadTech", ft.);
                    //templateVars.Add("WoActualAllDetailsWithExtPrice", ft.);
                }
                else if (ft.cusAgreementTemplate.Name == "Residential  Multipurpose Addendum to Agreement")
                {
                    templateVars.Add("CompanyLogo", ft.Company.CompanyLogo);
                    templateVars.Add("FirstName", ft.customerInfo.FirstName);
                    templateVars.Add("LastName", ft.customerInfo.LastName);
                    templateVars.Add("DisplayName", ft.customerInfo.DisplayName);
                    //templateVars.Add("InstallAddress", ft.InstallationAddress);
                    //templateVars.Add("InstallAddressCity", ft.customerInfo.City);
                    //templateVars.Add("installaddressstate", ft.customerInfo.State);
                    //templateVars.Add("installaddresszip", ft.customerInfo.ZipCode);
                    //templateVars.Add("ContactNumber", ft.customerInfo.PrimaryPhone);
                    //templateVars.Add("Phone2", ft.customerInfo.SecondaryPhone);
                    //templateVars.Add("Email", ft.customerInfo.EmailAddress);
                    //templateVars.Add("WorkOrderPrimaryAppointmentStartDate", ft.customerInfo.JoinDate.Value.ToString("MM/dd/yy"));
                    templateVars.Add("DateOfAgreement", DateTime.Now.DateFormat());
                    //templateVars.Add("Day", DateTime.Now.ToString("dd"));
                    //templateVars.Add("Month", DateTime.Now.ToString("MMMM"));
                    //templateVars.Add("Year", DateTime.Now.ToString("yyyy"));
                    templateVars.Add("CustomerSignatureDate", ft.FileManagementCustomerSignatureDateVal != null && ft.FileManagementCustomerSignatureDateVal != new DateTime() ? ft.FileManagementCustomerSignatureDateVal.ToString("M/dd/yy") : "");
                    if (!string.IsNullOrWhiteSpace(ft.FileManagementCustomerSignature) && ft.FileManagementCustomerSignature != "")
                    {
                        templateVars.Add("CustomerSignature", ft.FileManagementCustomerSignature);
                    }
                    //templateVars.Add("WorkOrderSalesRepName", ft.);
                    //templateVars.Add("WorkOrderLeadTech", ft.);
                    //templateVars.Add("WoActualAllDetailsWithExtPrice", ft.);
                }
                else if (ft.cusAgreementTemplate.Name == "ISPC Invoice Barry Pyron asd")
                {
                    templateVars.Add("CompanyLogo", ft.Company.CompanyLogo);
                    templateVars.Add("FirstName", ft.customerInfo.FirstName);
                    templateVars.Add("LastName", ft.customerInfo.LastName);
                    templateVars.Add("DisplayName", ft.customerInfo.DisplayName);
                    templateVars.Add("InstallAddress", ft.InstallationAddress);
                    templateVars.Add("ContactNumber", ft.customerInfo.PrimaryPhone);
                    templateVars.Add("AdsLogo", ft.AdsLogo);
                    templateVars.Add("BrinksLogo", ft.BrinksLogo);
                    //templateVars.Add("Day", DateTime.Now.ToString("dd"));
                    //templateVars.Add("Month", DateTime.Now.ToString("MMMM"));
                    //templateVars.Add("Year", DateTime.Now.ToString("yyyy"));
                    templateVars.Add("CustomerSignatureDate", ft.FileManagementCustomerSignatureDateVal != null && ft.FileManagementCustomerSignatureDateVal != new DateTime() ? ft.FileManagementCustomerSignatureDateVal.ToString("M/dd/yy") : "");
                    if (!string.IsNullOrWhiteSpace(ft.FileManagementCustomerSignature) && ft.FileManagementCustomerSignature != "")
                    {
                        templateVars.Add("CustomerSignature", ft.FileManagementCustomerSignature);
                    }
                }
                else if (ft.cusAgreementTemplate.Name == "Finance Terms")
                {
                    templateVars.Add("DisplayName", ft.customerInfo.DisplayName);
                    templateVars.Add("OnitSmartHome", ft.OnitSmartHome);
                    templateVars.Add("CustomerSignatureDate", ft.FileManagementCustomerSignatureDateVal != null && ft.FileManagementCustomerSignatureDateVal != new DateTime() ? ft.FileManagementCustomerSignatureDateVal.ToString("M/dd/yy") : "");
                    if (!string.IsNullOrWhiteSpace(ft.FileManagementCustomerSignature) && ft.FileManagementCustomerSignature != "")
                    {
                        templateVars.Add("CustomerSignature", ft.FileManagementCustomerSignature);
                    }
                }    
                else if (ft.cusAgreementTemplate.Name == "OrderRequestForm")
                {
                    templateVars.Add("CompanyLogo", ft.Company.CompanyLogo);
                    templateVars.Add("CustomerName", ft.customerInfo.DisplayName);

                    templateVars.Add("ContactNumber", !string.IsNullOrEmpty(ft.customerInfo.PrimaryPhone)? Format(ft.customerInfo.PrimaryPhone): Format(ft.customerInfo.SecondaryPhone));
                    templateVars.Add("CustomerSignatureDate", ft.FileManagementCustomerSignatureDateVal != null && ft.FileManagementCustomerSignatureDateVal != new DateTime() ? ft.FileManagementCustomerSignatureDateVal.ToString("M/dd/yy") : "");
                    if (!string.IsNullOrWhiteSpace(ft.FileManagementCustomerSignature) && ft.FileManagementCustomerSignature != "")
                    {
                        templateVars.Add("CustomerSignature", ft.FileManagementCustomerSignature);
                    }
                }

                else if (ft.cusAgreementTemplate.Name == "Multiple Term")
                {
                    templateVars.Add("DisplayName", ft.customerInfo.DisplayName);
                    templateVars.Add("OnitSmartHome", ft.OnitSmartHome);
                    templateVars.Add("CustomerSignatureDate", ft.FileManagementCustomerSignatureDateVal != null && ft.FileManagementCustomerSignatureDateVal != new DateTime() ? ft.FileManagementCustomerSignatureDateVal.ToString("M/dd/yy") : "");
                    if (!string.IsNullOrWhiteSpace(ft.FileManagementCustomerSignature) && ft.FileManagementCustomerSignature != "")
                    {
                        templateVars.Add("CustomerSignature", ft.FileManagementCustomerSignature);
                    }
                }
                else if (ft.cusAgreementTemplate.Name == "Residential Security Sale Agmt")
                {
                    templateVars.Add("DisplayName", ft.customerInfo.DisplayName);
                    templateVars.Add("InstallAddress", ft.InstallationAddress.ToString());
                    templateVars.Add("PhoneNumber", ft.customerInfo.PrimaryPhone);
                    templateVars.Add("DateOfAgreement", DateTime.Now.DateFormat());
                    templateVars.Add("CellNo", ft.customerInfo.CellNo);
                    templateVars.Add("Email", ft.customerInfo.EmailAddress);
                    templateVars.Add("CompanySignature", ft.CompanySignature);
                    templateVars.Add("CompanySignatureDate", ft.CompanySignatureDate);
                    templateVars.Add("CustomerSignatureDate", ft.FileManagementCustomerSignatureDateVal != null && ft.FileManagementCustomerSignatureDateVal != new DateTime() ? ft.FileManagementCustomerSignatureDateVal.ToString("M/dd/yy") : "");
                    if (!string.IsNullOrWhiteSpace(ft.FileManagementCustomerSignature) && ft.FileManagementCustomerSignature != "")
                    {
                        templateVars.Add("CustomerSignature", ft.FileManagementCustomerSignature);
                    }
                }
                else if (ft.cusAgreementTemplate.Name == "Commercial Security Sale Agmt")
                {
                    templateVars.Add("DisplayName", ft.customerInfo.DisplayName);
                    templateVars.Add("InstallAddress", ft.InstallationAddress.ToString());
                    templateVars.Add("PhoneNumber", ft.customerInfo.PrimaryPhone);
                    templateVars.Add("DateOfAgreement", DateTime.Now.DateFormat());
                    templateVars.Add("CellNo", ft.customerInfo.CellNo);
                    templateVars.Add("Email", ft.customerInfo.EmailAddress);
                    templateVars.Add("CompanySignature", ft.CompanySignature);
                    templateVars.Add("CompanySignatureDate", ft.CompanySignatureDate);
                    templateVars.Add("CustomerSignatureDate", ft.FileManagementCustomerSignatureDateVal != null && ft.FileManagementCustomerSignatureDateVal != new DateTime() ? ft.FileManagementCustomerSignatureDateVal.ToString("M/dd/yy") : "");
                    if (!string.IsNullOrWhiteSpace(ft.FileManagementCustomerSignature) && ft.FileManagementCustomerSignature != "")
                    {
                        templateVars.Add("CustomerSignature", ft.FileManagementCustomerSignature);
                    }
                }
                else if (ft.cusAgreementTemplate.Name == "Kazar Commercial Agreement")
                {
                    templateVars.Add("DisplayName", ft.customerInfo.DisplayName);
                    templateVars.Add("FirstName", ft.customerInfo.FirstName);
                    templateVars.Add("kazarLogo", ft.KazarLogo);
                    templateVars.Add("LastName", ft.customerInfo.LastName);
                    templateVars.Add("InstallAddress", ft.InstallationAddress.ToString());
                    templateVars.Add("InstallAddressCity", ft.customerInfo.City);
                    templateVars.Add("InstallAddressState", ft.customerInfo.State);
                    templateVars.Add("InstallAddressZip", ft.customerInfo.ZipCode);
                    templateVars.Add("InstallAddressStreet",ft.customerInfo.Street);
                    templateVars.Add("PhoneNumber", ft.customerInfo.PrimaryPhone);
                    templateVars.Add("DateOfAgreement", DateTime.Now.DateFormat());
                    templateVars.Add("CellNo", ft.customerInfo.CellNo);
                    templateVars.Add("Email", ft.customerInfo.EmailAddress);
                    templateVars.Add("SalesRepresentative",ft.employeeInfo.FirstName+" "+ft.employeeInfo.LastName);
                    templateVars.Add("CompanySignature", ft.CompanySignature);
                    templateVars.Add("CompanySignatureDate", ft.CompanySignatureDate);
                    templateVars.Add("CustomerSignatureDate", ft.FileManagementCustomerSignatureDateVal != null && ft.FileManagementCustomerSignatureDateVal != new DateTime() ? ft.FileManagementCustomerSignatureDateVal.ToString("M/dd/yy") : "");
                    if (!string.IsNullOrWhiteSpace(ft.FileManagementCustomerSignature) && ft.FileManagementCustomerSignature != "")
                    {
                        templateVars.Add("CustomerSignature", ft.FileManagementCustomerSignature);
                    }
                }
                else if (ft.cusAgreementTemplate.Name == "Kazar Residential Alarm Agreement")
                {
                    templateVars.Add("DisplayName", ft.customerInfo.DisplayName);
                    templateVars.Add("FirstName", ft.customerInfo.FirstName);
                    templateVars.Add("kazarLogo", ft.KazarLogo);
                    templateVars.Add("LastName", ft.customerInfo.LastName);
                    templateVars.Add("InstallAddress", ft.InstallationAddress.ToString());
                    templateVars.Add("InstallAddressCity", ft.customerInfo.City);
                    templateVars.Add("InstallAddressState", ft.customerInfo.State);
                    templateVars.Add("InstallAddressZip", ft.customerInfo.ZipCode);
                    templateVars.Add("InstallAddressStreet", ft.customerInfo.Street);
                    templateVars.Add("PhoneNumber", ft.customerInfo.PrimaryPhone);
                    templateVars.Add("DateOfAgreement", DateTime.Now.DateFormat());
                    templateVars.Add("CellNo", ft.customerInfo.CellNo);
                    templateVars.Add("Email", ft.customerInfo.EmailAddress);
                    templateVars.Add("SalesRepresentative", ft.employeeInfo.FirstName + " " + ft.employeeInfo.LastName);
                    templateVars.Add("CompanySignature", ft.CompanySignature);
                    templateVars.Add("CompanySignatureDate", ft.CompanySignatureDate);
                    templateVars.Add("CustomerSignatureDate", ft.FileManagementCustomerSignatureDateVal != null && ft.FileManagementCustomerSignatureDateVal != new DateTime() ? ft.FileManagementCustomerSignatureDateVal.ToString("M/dd/yy") : "");
                    if (!string.IsNullOrWhiteSpace(ft.FileManagementCustomerSignature) && ft.FileManagementCustomerSignature != "")
                    {
                        templateVars.Add("CustomerSignature", ft.FileManagementCustomerSignature);
                    }
                }
                else if (ft.cusAgreementTemplate.Name == "Code-3 Agreement")
                {
                    templateVars.Add("CompanyLogo", ft.Company.CompanyLogo);
                    templateVars.Add("FirstName", ft.customerInfo.FirstName);
                    templateVars.Add("LastName", ft.customerInfo.LastName);
                    templateVars.Add("BusinessName",ft.customerInfo.BusinessName);
                    templateVars.Add("DisplayName", ft.customerInfo.DisplayName);
                    templateVars.Add("ActivationFee", ft.customerInfo.ActivationFee !=null? Math.Round(ft.customerInfo.ActivationFee.Value,2):0);
                    templateVars.Add("InstallAddress", ft.InstallationAddress);
                    templateVars.Add("BillingAddress",!string.IsNullOrWhiteSpace(ft.customerInfo.Address2)?ft.customerInfo.Address2: ft.InstallationAddress);
                    templateVars.Add("ContactNumber", ft.customerInfo.PrimaryPhone);
                    templateVars.Add("Email", ft.customerInfo.EmailAddress);
                    templateVars.Add("ContractTerm",ft.ContractTeam);
                    templateVars.Add("SubscribedMonths", ft.ContractTeamVal);
                    templateVars.Add("SalesRepresentative", ft.employeeInfo.FirstName + " " + ft.employeeInfo.LastName);
                    templateVars.Add("Day", DateTime.Now.DateFormat("day"));
                    templateVars.Add("Month", DateTime.Now.DateFormat("monthName"));
                    templateVars.Add("Year", DateTime.Now.ToString("yy"));
                    templateVars.Add("DateOfAgreement", DateTime.Now.DateFormat());
                    //templateVars.Add("Day", DateTime.Now.ToString("dd"));
                    //templateVars.Add("Month", DateTime.Now.ToString("MMMM"));
                    //templateVars.Add("Year", DateTime.Now.ToString("yyyy"));
                    templateVars.Add("CustomerSignatureDate", ft.FileManagementCustomerSignatureDateVal != null && ft.FileManagementCustomerSignatureDateVal != new DateTime() ? ft.FileManagementCustomerSignatureDateVal.ToString("M/dd/yy") : "");
                    if (!string.IsNullOrWhiteSpace(ft.FileManagementCustomerSignature) && ft.FileManagementCustomerSignature != "")
                    {
                        templateVars.Add("CustomerSignature", ft.FileManagementCustomerSignature);
                    }
                }

                else if (ft.cusAgreementTemplate.Name == "iEatery Service Agreement")
                {
                    templateVars.Add("iEateryLogo", ft.iEateryLogo);
                    templateVars.Add("VisaLogo", ft.VisaLogo);
                    templateVars.Add("MasterCardLogo", ft.MastercardLogo);
                    templateVars.Add("DiscoverLogo", ft.DiscoverLogo);
                    templateVars.Add("AmericanExpressLogo", ft.AmericanExpressLogo);
                    templateVars.Add("BusinessName", ft.customerInfo.BusinessName);
                    templateVars.Add("CustomerTitle", ft.customerInfo.Title);
                    templateVars.Add("InstallAddress", ft.InstallationAddress);
                    templateVars.Add("ContactNumber", ft.customerInfo.PrimaryPhone);
                    templateVars.Add("AuthorizedRepresentative", ft.AuthorizedRepresentative);
                    templateVars.Add("Email", ft.customerInfo.EmailAddress);
                    templateVars.Add("Day", DateTime.Now.DateFormat("day"));
                    templateVars.Add("Month", DateTime.Now.DateFormat("monthName"));
                    templateVars.Add("Year", DateTime.Now.DateFormat("year"));
                    templateVars.Add("CustomerSignatureDate", ft.FileManagementCustomerSignatureDateVal != null && ft.FileManagementCustomerSignatureDateVal != new DateTime() ? ft.FileManagementCustomerSignatureDateVal.ToString("M/dd/yy") : "");
                    if (!string.IsNullOrWhiteSpace(ft.FileManagementCustomerSignature) && ft.FileManagementCustomerSignature != "")
                    {
                        templateVars.Add("CustomerSignature", ft.FileManagementCustomerSignature);
                    }
                    if(ft._paymentInfo!=null)
                    {

                        string AccountName = ft._paymentInfo.AccountName;
                        string[] arrAccName = AccountName.Split(' ');
                        string accFirstName = AccountName.Split(' ').First();
                        string accLastName = "";
                        if (arrAccName.Length > 1)
                        {
                            accLastName = AccountName.Split(' ').Last();
                        }
                        templateVars.Add("ACHLastName", accLastName);
                        templateVars.Add("ACHFirstName", accFirstName);
                        if (!string.IsNullOrWhiteSpace(ft._paymentInfo.AcountNo))
                        {
                            
                            templateVars.Add("ACHRTNo", ft._paymentInfo.RoutingNo);
                            templateVars.Add("BankOrCCNo", ft._paymentInfo.AcountNo);     
                        }
                        else
                        {
                            templateVars.Add("BankOrCCNo",!string.IsNullOrWhiteSpace(ft._paymentInfo.CardNumber)? "********"+ft._paymentInfo.CardNumber.Substring(Math.Max(0, ft._paymentInfo.CardNumber.Length-4)):"");
                            templateVars.Add("ExpiredDate", ft._paymentInfo.CardExpireDate);
                            templateVars.Add("CVV", !string.IsNullOrWhiteSpace(ft._paymentInfo.CardSecurityCode)? "**" + ft._paymentInfo.CardSecurityCode.Substring(Math.Max(0, ft._paymentInfo.CardSecurityCode.Length-1)):"");
                            if(ft._paymentInfo.CardType== "Visa")
                            {
                                templateVars.Add("IsVisa", "<input type='checkbox' checked />");
                            }
                            else
                            {
                                templateVars.Add("IsVisa", "<input type='checkbox' />");
                            }
                            if(ft._paymentInfo.CardType== "Mastercard")
                            {
                                templateVars.Add("IsMasterCard", "<input type='checkbox' checked />");
                            }
                            else
                            {
                                templateVars.Add("IsMasterCard", "<input type='checkbox' />");
                            }
                            if (ft._paymentInfo.CardType == "AMEX")
                            {
                                templateVars.Add("IsAmericanExpress", "<input type='checkbox' checked />");
                            }
                            else
                            {
                                templateVars.Add("IsAmericanExpress", "<input type='checkbox' />");
                            }
                            if (ft._paymentInfo.CardType == "Discover")
                            {
                                templateVars.Add("IsDiscover", "<input type='checkbox' checked />");
                            }
                            else
                            {
                                templateVars.Add("IsDiscover", "<input type='checkbox' />");
                            }
                        }
                        
                    }
                    else
                    {
                        templateVars.Add("IsVisa", "<input type='checkbox' />");
                        templateVars.Add("IsMasterCard", "<input type='checkbox' />");
                        templateVars.Add("IsAmericanExpress", "<input type='checkbox' />");
                        templateVars.Add("IsDiscover", "<input type='checkbox' />");
                    }
                }

                EmailParser parser = null;
                //EmailTemplate agreementTemplate = _EmailTemplateDataAccess.GetEmailTemplateByKeyAndCompanyId(agreementPdf.CompanyId, EmailTemplateKey.SmartAgreement.SmartAgreementRMR);


                parser = new EmailParser(ft.cusAgreementTemplate.BodyContent, templateVars, false);
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
        #region Customer Addendum
        public string MakeCustomerAddendumPdf(CustomerAddendumModel cusAddendum, int? templateid)
        {
            string currentCurrency = "";
            string ServiceList = "";
            string AddedList = "";
            string RemovedList = "";
            string EquipmentList = "";
            string Body = "";
            double MonitoringFee = 0;
            double Added = 0;
            double Removed = 0;
            double BillingTicketTotalAmount = 0;
            double NewTicketTotalAmount = 0;
            double DefaultServiceAmount = 0;
            if (cusAddendum.CurrentCurrency != null)
            {
                currentCurrency = cusAddendum.CurrentCurrency;
            }
            else
            {
                currentCurrency = "$";
            }
            //var objtiklist = GetTicketListByCustomerIdAndCompanyId(cusAddendum.CustomerId, cusAddendum.CompanyId);
            //if (objtiklist != null && objtiklist.Count > 0)
            //{
            //    foreach (var tik in objtiklist)
            //    {
            //        var objappeqp = GetCustomerAppointmentEquipmentByAppointmentIdAndBilling(tik.TicketId);
            //        if (objappeqp != null && objappeqp.Count > 0)
            //        {
            //            foreach (var eqp in objappeqp)
            //            {
            //                BillingTicketTotalAmount += eqp.TotalPrice;
            //            }
            //        }
            //    }
            //}
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
                templateVars.Add("INSTALLADDRESS", cusAddendum.InstallAddress);

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
                templateVars.Add("RECURRINGAMOUNT", currentCurrency +FormatAmount(MonitoringFee));
                if (!string.IsNullOrWhiteSpace(cusAddendum.CustomerAddendumSignature))
                {
                    templateVars.Add("CustomerAddendumSignature", cusAddendum.CustomerAddendumSignature);
                    if (!string.IsNullOrWhiteSpace(cusAddendum.CompanySignature))
                    {
                        templateVars.Add("CompanySignature", cusAddendum.CompanySignature);
                        if (!string.IsNullOrWhiteSpace(cusAddendum.CompanySignatureDate))
                            templateVars.Add("CompanySignatureDate", cusAddendum.CompanySignatureDate);
                    }
                    //if (!string.IsNullOrWhiteSpace(cusAddendum.CustomerAddendumStringSignatureDate))
                    //{
                    //    templateVars.Add("CustomerAddendumSignatureDate", cusAddendum.CustomerAddendumStringSignatureDate);
                    //}
                    templateVars.Add("CustomerAddendumSignatureDate", cusAddendum.CustomerAddendumSignatureDate != null && cusAddendum.CustomerAddendumSignatureDate != new DateTime() ? cusAddendum.CustomerAddendumSignatureDate.ToString("M/dd/yy hh:mm tt") : "");

                }


                EmailParser parser = null;

                CustomerAgreementTemplate agreementTemplate = _CustomerAgreementTemplateDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and Id = {1}", cusAddendum.CompanyId, templateid.Value)).FirstOrDefault();
                parser = new EmailParser(agreementTemplate.BodyContent, templateVars, false);
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

        public string FormatAmount(double? value)
        {
            string formatted = "0.00";
            if (value.HasValue)
            {
                formatted = string.Format(CultureInfo.InvariantCulture, "{0:N}", value);

            }
            return formatted;
        }
        #region Smart Agreement
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

        public string MakeSmartAgreementPdf(InstallationAgreementModel agreementPdf, int? templateid)
        {
            string Body = "";
            string ContactName = "";
            string ContactPhone = "";
            string CenterSpace = "";
            string PhoneType = "";
            string ContactRelationship = "";
            string HasKey = "";
            string EmergencyContactList = "";
            string ContactNameHeader = "";
            string ContactPhoneHeader = "";
            string PhoneTypeHeader = "";
            string ContactRelationshipHeader = "";
            string EquipmentName = "";
            string ServiceName = "";
            string MonthlyRate = "";
            string DiscountRate = "";
            string Total = "";
            string UnitPrice = "";
            string FinancedAmount = "";

            string CommercialEquipmentList = "";
            string CommercialServiceList = "";

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
            string ServiceList = "";
            string ServiceListRab = "";
            string ServiceListDFW = "";
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
            double AdvanceMonitoringFee = 0.0;
            double MonthlyServiceFee = 0.0;

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
                if (objpayprofile != null && objpayprofile.Type.ToLower().IndexOf("ach") > -1)
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
                //templateVars.Add("CSIDNumber", agreementPdf.CSIDNumber);
                if (agreementPdf.OriginalContactDate != new DateTime())
                {
                    OriginalContactDateTemplate = "<tr style='height: 30px;'><td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Original Contract Date</td><td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>" + agreementPdf.OriginalContactDate.ToString("MM/dd/yy") + "</td></tr>";
                }
                templateVars.Add("InstallDate", OriginalContactDateTemplate);
                templateVars.Add("AccountType", agreementPdf.AccountType);
                templateVars.Add("Referredby", agreementPdf.Referredby);
                if (!string.IsNullOrEmpty(agreementPdf.SocialSecurityNumber) && agreementPdf.SocialSecurityNumber.Length > 4)
                {
                    var FormateSSN = agreementPdf.SocialSecurityNumber.Substring(agreementPdf.SocialSecurityNumber.Length - 4);
                    SSN = String.Format("{0:xxx-xx-0000}", Convert.ToInt32(FormateSSN));
                }
                templateVars.Add("SocialSecurityNumber", SSN);
                templateVars.Add("Owner2ndPhone",Extentions.PhoneNumberFormatNew(agreementPdf.Owner2ndPhone));
                templateVars.Add("OwnerCellPhone", Extentions.PhoneNumberFormatNew(agreementPdf.OwnerCellPhone));
                templateVars.Add("InstallAddress", agreementPdf.InstallAddress);
                templateVars.Add("InitialCity", agreementPdf.InitialCity);
                templateVars.Add("InitialCountry", agreementPdf.InitialCountry);
                templateVars.Add("InitialState", agreementPdf.InitialState);
                templateVars.Add("InitialZip", agreementPdf.InitialZip);
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
                var upfrontamount = agreementPdf.UpfrontAddOnTotal;
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
                        templateVars.Add("OneTimeServiceFee", "<tr style='height: 25px;'><td valign='middle' style='font-weight:bold; border: 1px solid #000; text-align:right; padding-right:5px; background-color:#f3f3f3;'>+  One Time Service Fee</td><td style='border: 1px solid #000;text-align: right;padding-right: 10px;'>" + currentCurrency + FormatAmount(onetimeservicefee) + "</td></tr>");
                    }
                    agreementPdf.NonConfirmingFee = agreementPdf.NonConfirmingFee;
                    if (agreementPdf.IsServicePromo != true)
                    {
                        MonthlyServiceFee = serviceFeeTotal;
                    }
                    var subtotalamount = MonthlyServiceFee + AdvanceMonitoringFee+ upfrontamount + activationamount + agreementPdf.NonConfirmingFee + onetimeservicefee;
                    agreementPdf.TaxTotal = subtotalamount * (agreementPdf.Tax / 100);
                    var duesignamount = subtotalamount + agreementPdf.TaxTotal;
                    templateVars.Add("MonthlyServiceFeeTotal", currentCurrency + FormatAmount(serviceFeeTotal));
                    templateVars.Add("MonthlyServiceFeeFinalTotal", currentCurrency + FormatAmount(MonthlyServiceFee));
                    templateVars.Add("UpfrontAddOnTotal", currentCurrency + FormatAmount(upfrontamount));
                    templateVars.Add("ActivationFee", currentCurrency + FormatAmount(activationamount));
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
                    var onetimeservicefee = 0.0;
                    var serviceFeeTotal = agreementPdf.MonthlyServiceFeeTotal;
                    if (agreementPdf.IsUpfrontPromo == true)
                    {
                        upfrontamount = agreementPdf.UpfrontAddOnTotalPromo;
                    }
                    var activationamount = agreementPdf.ActivationFee;
                    if (agreementPdf.NotARBEnabledTotalPrice > 0)
                    {
                        onetimeservicefee = agreementPdf.NotARBEnabledTotalPrice;
                        templateVars.Add("OneTimeServiceFee", "<tr style='height: 25px;'><td valign='middle' style='font-weight:bold; border: 1px solid #000; text-align:right; padding-right:5px; background-color:#f3f3f3;'>+  One Time Service Fee</td><td style='border: 1px solid #000;text-align: right;padding-right: 10px;'>" + currentCurrency + FormatAmount(onetimeservicefee) + "</td></tr>");
                    }
                    if (agreementPdf.IsServicePromo != true)
                    {
                        MonthlyServiceFee = serviceFeeTotal;
                    }
                    var subtotalamount = MonthlyServiceFee + AdvanceMonitoringFee + upfrontamount + activationamount + agreementPdf.NonConfirmingFee + onetimeservicefee;
                    agreementPdf.TaxTotal = subtotalamount * (agreementPdf.Tax / 100);
                    var duesignamount = subtotalamount + agreementPdf.TaxTotal;

                    templateVars.Add("MonthlyServiceFeeTotal", currentCurrency + FormatAmount(agreementPdf.MonthlyServiceFeeTotal));
                    templateVars.Add("MonthlyServiceFeeFinalTotal", currentCurrency + FormatAmount(MonthlyServiceFee));
                    templateVars.Add("UpfrontAddOnTotal", currentCurrency + FormatAmount(upfrontamount));
                    templateVars.Add("ActivationFee", currentCurrency + FormatAmount(agreementPdf.ActivationFee));
                    templateVars.Add("NewSubTotal", currentCurrency + FormatAmount(agreementPdf.NewSubTotal + AdvanceMonitoringFee));
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
                        var taxcollectedtotal = (collectedamount+ AdvanceMonitoringFee) * (agreementPdf.Tax / 100);
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
                templateVars.Add("CompanyName", agreementPdf.CompanyName);
                templateVars.Add("CompanyStreet", agreementPdf.CompanyStreet);
                templateVars.Add("CompanySate", agreementPdf.CompanySate);
                templateVars.Add("CompanyWebsite", agreementPdf.CompanyWebsite);
                templateVars.Add("CompanyPhone", agreementPdf.CompanyPhone);
                templateVars.Add("BusinessName", agreementPdf.BusinessName);
                templateVars.Add("CustomerName", agreementPdf.OwnerName);
                templateVars.Add("EffectiveDate", agreementPdf.EffectiveDate);
                templateVars.Add("OwnerAddress", agreementPdf.OwnerAddress);
                templateVars.Add("OwnerBillingAddress", agreementPdf.BillingAddress);
                templateVars.Add("OwnerEmail", agreementPdf.OwnerEmail);
                templateVars.Add("OwnerPhone", Extentions.PhoneNumberFormatNew(agreementPdf.OwnerPhone));
                templateVars.Add("ContractTerm", agreementPdf.SubscribedMonths);
                templateVars.Add("RenewalTerm", agreementPdf.RenewalMonths);
                templateVars.Add("ContractCreatedDate", agreementPdf.ContractCreatedDateVal);
                if (!string.IsNullOrWhiteSpace(agreementPdf.CustomerSignature))
                {
                    templateVars.Add("CustomerSignature", agreementPdf.CustomerSignature);
                    templateVars.Add("CustomerSignatureDate", agreementPdf.CustomerSignatureStringDateVal != null && agreementPdf.CustomerSignatureStringDateVal != new DateTime() ? agreementPdf.CustomerSignatureStringDateVal.ToString("M/dd/yy") : "");
                    //if (!string.IsNullOrWhiteSpace(agreementPdf.CustomerSignatureStringDate))
                    //{
                    //    templateVars.Add("CustomerSignatureDate", agreementPdf.CustomerSignatureStringDate);
                    //}
                    if (!string.IsNullOrWhiteSpace(agreementPdf.CompanySignature))
                    {
                        templateVars.Add("CompanySignature", agreementPdf.CompanySignature);
                        if (!string.IsNullOrWhiteSpace(agreementPdf.CompanySignatureDate))
                        {
                            templateVars.Add("CompanySignatureDate", agreementPdf.CompanySignatureDate);
                        }
                    }
                }

                if (agreementPdf.ACHDiscountAmount > 0)
                {
                    var serviceFeeTotal = agreementPdf.MonthlyServiceFeeTotal - agreementPdf.ACHDiscountAmount;
                    templateVars.Add("MonthlyMonitoringFee", currentCurrency + FormatAmount(serviceFeeTotal));
                }
                else
                {
                    templateVars.Add("MonthlyMonitoringFee", currentCurrency + FormatAmount(Convert.ToDouble(agreementPdf.MonthlyMonitoringFee)));
                }
                templateVars.Add("Tax", currentCurrency + FormatAmount(agreementPdf.TaxTotal));
                templateVars.Add("Total", currentCurrency + FormatAmount(agreementPdf.Total));
                templateVars.Add("CompanyLogo", agreementPdf.CompanyLogo);
                templateVars.Add("DateOfTransaction", agreementPdf.DateOfTransaction.ToString("MMMM dd yyyy"));

                //if (templateid.HasValue && templateid.Value > 0)
                //{
                //    templateVars.Add("Day", DateTime.Now.DateFormat("day"));
                //    templateVars.Add("Month", DateTime.Now.DateFormat("monthName"));
                //    templateVars.Add("Year", DateTime.Now.DateFormat("year"));
                //}
                //else
                //{
                templateVars.Add("Date", agreementPdf.DateOfTransaction.ToString("dd"));
                templateVars.Add("Month", agreementPdf.DateOfTransaction.ToString("MMMM"));
                templateVars.Add("Year", agreementPdf.DateOfTransaction.ToString("yyyy"));
                //}
                double SubscribedMonths = 0;
                double MonthlyMonitoringFee = 0;

                double.TryParse(agreementPdf.SubscribedMonths, out SubscribedMonths);
                double.TryParse(agreementPdf.MonthlyMonitoringFee, out MonthlyMonitoringFee);
                templateVars.Add("SubscribedMonths", string.Format("{0} {1}", agreementPdf.SubscribedMonthsInWord, SubscribedMonthsText));
                templateVars.Add("SubscribedMonthsUperCase", string.Format("{0} {1}", agreementPdf.SubscribedMonthsInWord.ToUpper(), SubscribedMonthsText.ToUpper()));
                templateVars.Add("RenewalMonths", string.Format("{0} {1}", agreementPdf.RenewalMonths, "month"));
                templateVars.Add("TotalPayments", currentCurrency + SubscribedMonths * MonthlyMonitoringFee);
                templateVars.Add("Subtotal", currentCurrency + FormatAmount(agreementPdf.Subtotal));
                templateVars.Add("RevisionDate", "<span style='float:right; margin-top:25px;'>Rev. " + DateTime.Now.ToString("MMM") + DateTime.Now.ToString("yyyy") + "</span>");
                if (agreementPdf.ACHDiscountAmount > 0)
                {
                    templateVars.Add("ServiceSubtotal", "<tr style='background-color:#bfbfbf; color:#000; height: 30px;'><td style='text-align:center; border: 2px solid #000; border-right:1px solid #000; padding:0px 10px; font-size:15px; font-weight:bold;' colspan = 4>SubTotal</td><td style='text-align:center; border: 2px solid #000; border-left:1px solid #000;'>" + currentCurrency + FormatAmount(agreementPdf.TotalMonthlyMonitoring) + "</td></tr>");
                }
                if (!string.IsNullOrWhiteSpace(agreementPdf.BusinessName) && !string.IsNullOrWhiteSpace(agreementPdf.DoingBusinessAs))
                {
                    //if (templateid.HasValue && templateid.Value > 0)
                    //{
                    //    templateVars.Add("OwnerName", agreementPdf.DisplayName);
                    //}
                    //else
                    //{
                    templateVars.Add("OwnerName", @"<div style = 'padding-left:10px; width:42%; float:left;'>Customer Name<div>" + agreementPdf.OwnerName + "</div></div>");
                    //}
                    templateVars.Add("OwnerBusinessName", @"<div style = 'padding-left:10px; width:27%; float:left;'>Business Name<div>" + agreementPdf.BusinessName + "</div></div>");
                    templateVars.Add("OwnerDBA", @"<div style = 'padding-left:10px; width:27%; float:left;'>Doing Business As<div>" + agreementPdf.DoingBusinessAs + "</div></div>");
                    templateVars.Add("CancellationOwnerName", agreementPdf.DispalyName);
                    templateVars.Add("OwnerDisplayName", @"<td valign = 'top' style = 'padding-left:10px;' colspan = '3'>Customer Name<div>" + agreementPdf.DispalyName + "</div></td>");
                }
                else if (!string.IsNullOrWhiteSpace(agreementPdf.BusinessName))
                {
                    //if (templateid.HasValue && templateid.Value > 0)
                    //{
                    //    templateVars.Add("OwnerName", agreementPdf.DisplayName);
                    //}
                    //else
                    //{
                    templateVars.Add("OwnerName", @"<div style = 'padding-left:10px; width:42%; float:left;'>Customer Name<div>" + agreementPdf.OwnerName + "</div></div>");
                    //templateVars.Add("OwnerName", @"<td valign = 'top' style = 'padding-left:10px;' colspan = '5'>Customer Name<div>" + agreementPdf.OwnerName + "</div></td>");
                    //}
                    templateVars.Add("OwnerDisplayName", @"<td valign = 'top' style = 'padding-left:10px;' colspan = '3'>Customer Name<div>" + agreementPdf.DispalyName + "</div></td>");
                    templateVars.Add("OwnerBusinessName", @"<div style = 'padding-left:10px; width:27%; float:left;'>Business Name<div>" + agreementPdf.BusinessName + "</div></div>");
                    //templateVars.Add("OwnerBusinessName", @"<td valign = 'top' style = 'padding-left:10px;' colspan = '2'>Business Name<div>" + agreementPdf.BusinessName + "</div></td>");
                    templateVars.Add("CancellationOwnerName", agreementPdf.DispalyName);
                }
                else if (!string.IsNullOrWhiteSpace(agreementPdf.DoingBusinessAs))
                {
                    //if (templateid.HasValue && templateid.Value > 0)
                    //{
                    //    templateVars.Add("OwnerName", agreementPdf.DisplayName);
                    //}
                    //else
                    //{
                    templateVars.Add("OwnerName", @"<div style = 'padding-left:10px; width:42%; float:left;'>Customer Name<div>" + agreementPdf.OwnerName + "</div></div>");
                    //templateVars.Add("OwnerName", @"<td valign = 'top' style = 'padding-left:10px;' colspan = '5'>Customer Name<div>" + agreementPdf.OwnerName + "</div></td>");
                    //}
                    templateVars.Add("OwnerDisplayName", @"<td valign = 'top' style = 'padding-left:10px;' colspan = '3'>Customer Name<div>" + agreementPdf.DispalyName + "</div></td>");
                    templateVars.Add("OwnerDBA", @"<div style = 'padding-left:10px; width:27%; float:left;'>Doing Business As<div>" + agreementPdf.DoingBusinessAs + "</div></div>");
                    //templateVars.Add("OwnerDBA", @"<td valign = 'top' style = 'padding-left:10px;' colspan = '2'>Doing Business As<div>" + agreementPdf.DoingBusinessAs + "</div></td>");
                    templateVars.Add("CancellationOwnerName", agreementPdf.DispalyName);
                }
                else
                {
                    //if (templateid.HasValue && templateid.Value > 0)
                    //{
                    //    templateVars.Add("OwnerName", agreementPdf.OwnerName);
                    //}
                    //else
                    //{
                    templateVars.Add("OwnerName", @"<div style = 'padding-left:10px; width:42%; float:left;'>Customer Name<div>" + agreementPdf.OwnerName + "</div></div>");
                    //templateVars.Add("OwnerName", @"<td valign = 'top' style = 'padding-left:10px;' colspan = '7'>Customer Name<div>" + agreementPdf.OwnerName + "</div></td>");
                    //}
                    templateVars.Add("OwnerDisplayName", @"<td valign = 'top' style = 'padding-left:10px;' colspan = '3'>Customer Name<div>" + agreementPdf.DispalyName + "</div></td>");
                    templateVars.Add("CancellationOwnerName", agreementPdf.DispalyName);
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
                }
                foreach (var item in agreementPdf.SmartPackageEquipmentServiceList)
                {
                    SmartPackageEquipmentServiceList += "<div style = 'width:50%; float:left; text-align:left;box-sizing:border-box;'>" + item.EquipmentName + "</div>";
                }
                templateVars.Add("SmartPackageEquipmentServiceList", SmartPackageEquipmentServiceList);
                CommercialEquipmentList += "<table style='width:100%; float:left; border-collapse:collapse; margin-bottom:30px; margin-top:10px;'>" +
                        "<thead>" +
                        "<tr style='background-color:#4f90bb;color:white;width:100%;border:1px solid #4f90bb'>" +
                        "<th style='width:67%;text-align:left;padding:5px 0px 5px 20px;border-right:1px solid #fff'>EQUIPMENT</th>" +
                        "<th style='width:5%;padding:5px 0px;border-right:1px solid #fff;text-align:center'>QTY</th>" +
                        "<th style='width:14%;padding:5px 0px;border-right:1px solid #fff;text-align:center'>UNIT PRICE</th>" +
                        "<th style='width:14%;padding:5px 0px;border-right:1px solid #fff;text-align:center'>TOTAL PRICE</th>" +
                        "</tr></thead><tbody>";
                GlobalSetting glHidingUnitPrice = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", agreementPdf.CompanyId, "HideUnitPriceOnAgreement")).FirstOrDefault();
                foreach (var item in agreementPdf.EquipmentList)
                {
                    EquipmentName = "  <div style = 'width:39%;float:left;padding-left:10px;'><span> &nbsp;" + item.Name + " </span></div> ";
                    EquipmentQuantity = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + item.Quantity + " </span></div>";
                    UnitPrice = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency +FormatAmount(item.UnitPrice) + " </span></div>";
                    DiscountUnitPrice = " <div style = 'width:15%;float:left;text-align:center'><span>" + currentCurrency + FormatAmount(item.DiscountUnitPricce) + " </span></div>";
                    TotalEquipment = "  <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.Total) + " </span></div>";
                    EquipmentList += " <div style='padding-top:5px;'>" + EquipmentName + EquipmentQuantity + UnitPrice + DiscountUnitPrice + TotalEquipment + "</div>";

                    if (item.IsEqpExist)
                    {
                        ProductDFW = "<td style='text-align:center; font-weight:bold; border: 1px solid #000; padding:0px 10px;'>" + item.Name + " (Existing Equipment)</ td >";
                    }
                    else
                    {
                        ProductDFW = "<td style='text-align:center; font-weight:bold; border: 1px solid #000; padding:0px 10px;'>" + item.Name + "</ td >";
                    }

                    QuantityDFW = "<td style='text-align:center; border:1px solid #000;'>" + item.Quantity + "</ td >";
                    UnitPriceDFW = "<td style='text-align:center; font-weight:bold; border:1px solid #000;'>" + currentCurrency + FormatAmount(item.UnitPrice) + "</ td >";
                    DiscountUnitPriceDFW = "<td style='text-align:center; border:1px solid #000;'>" + currentCurrency + FormatAmount(item.DiscountUnitPricce) + "</ td >";
                    TotalDiscountDFW = "<td style='text-align:center; border:1px solid #000;'>" + currentCurrency + FormatAmount(item.DiscountUnitPricce * item.Quantity) + "</ td >";
                    TotalPriceDFW = "<td style='text-align:center; border:1px solid #000;'>" + currentCurrency + FormatAmount(item.Total) + "</ td >";
                    if (glHidingUnitPrice != null && glHidingUnitPrice.Value == "true")
                    {
                        EquipmentListDFW += "<tr style='background - color:#f2f2f2; color:#000; height: 30px; border:2px solid #000;'>" + ProductDFW + QuantityDFW + DiscountUnitPriceDFW + TotalPriceDFW + "</tr>";
                    }
                    else
                    {

                        EquipmentListDFW += "<tr style='background - color:#f2f2f2; color:#000; height: 30px; border:2px solid #000;'>" + ProductDFW + QuantityDFW + UnitPriceDFW + DiscountUnitPriceDFW + TotalDiscountDFW + TotalPriceDFW + "</tr>";
                    }
                    //EquipmentListDFW += "<tr style='background - color:#f2f2f2; color:#000; height: 30px; border:2px solid #000;'>" + ProductDFW + QuantityDFW + UnitPriceDFW + DiscountUnitPriceDFW + TotalDiscountDFW + TotalPriceDFW + "</tr>";

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
                CommercialEquipmentList += "<tr style='font-weight:bold'>" +
                        "<td colspan='3' style='padding-top:5px;text-align:right;padding-right:10px'>Subtotal</td>" +
                        "<td style='padding-top:5px; text-align:center; padding-left:10px;'>" +
                        currentCurrency + FormatAmount(agreementPdf.UpfrontAddOnTotal) +
                        "</td></tr>"
                        + "<tr style='font-weight:bold'>" +
                        "<td colspan='3' style='padding-top:5px;text-align:right;padding-right:10px'>Tax</td>" +
                        "<td style='padding-top:5px; text-align:center; padding-left:10px;'>" +
                        currentCurrency + FormatAmount((agreementPdf.UpfrontAddOnTotal / 100) * agreementPdf.Tax) +
                        "</td></tr>"
                        + "<tr style='font-weight:bold'>" +
                        "<td colspan='3' style='padding-top:5px;text-align:right;padding-right:10px'>Total</td>" +
                        "<td style='padding-top:5px; text-align:center; padding-left:10px;'>" +
                        currentCurrency + FormatAmount(agreementPdf.UpfrontAddOnTotal + ((agreementPdf.UpfrontAddOnTotal / 100) * agreementPdf.Tax)) +
                        "</td></tr></tbody></table>";
                if(agreementPdf.EquipmentList.Count == 0)
                {
                    CommercialEquipmentList = "";
                }
                foreach (var item in agreementPdf.EquipmentList)
                {
                    EquipmentName = "  <div style = 'width:30%;float:left;padding-left:10px;'><span> &nbsp;" + item.Name + " </span></div> ";
                    EquipmentQuantity = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + item.Quantity + " </span></div>";
                    UnitPrice = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.UnitPrice) + " </span></div>";
                    DiscountUnitPrice = " <div style = 'width:20%;float:left;text-align:center'><span>" + currentCurrency + FormatAmount(item.DiscountUnitPricce) + " </span></div>";
                    TotalEquipment = "  <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.Total) + " </span></div>";
                    EquipmentListRab += " <div style='padding-top:5px;'>" + EquipmentName + EquipmentQuantity + UnitPrice + DiscountUnitPrice + TotalEquipment + "</div>";
                }
                if (isServiceDetail == true)
                {
                    ServiceListTable = " <tr style='background-color:#000; color:#fff; height: 30px; border:2px solid #000;'><td style='width:40%; text-align:center; font-weight:bold;'>MONTHLY SERVICE</td><td style='text-align:center; font-weight:bold;'>Type</td><td style='text-align:center; font-weight:bold;'> Monthly</td><td style='text-align:center; font-weight:bold;'> Qty</td><td style='text-align:center; font-weight:bold;'>Total $</td></tr>";
                }
                else
                {
                    ServiceListTable = " <tr style='background-color:#000; color:#fff; height: 30px; border:2px solid #000;'><td colspan='4' style='text-align:center; font-weight:bold;'>MONTHLY SERVICE</td></tr>";
                }
                CommercialServiceList += "<table style='width:100%; float:left; border-collapse:collapse; margin-bottom:30px; margin-top:10px;'>"
                        + "<thead>"
                        + "<tr style='background-color:#4f90bb;color:white;width:100%;border:1px solid #4f90bb'>"
                        + "<th style='width:86%; text-align:left; padding:5px 0px 5px 20px; border-right:1px solid #fff'>SERVICE</th>"
                        //+ "<th style='width:14%; padding:5px 0px; border-right:1px solid #fff;text-align:center'>PRICE</th>"
                        + "</tr></thead><tbody>";
                foreach (var item in agreementPdf.ServiceList)
                {
                    ServiceName = "  <div style = 'width:54%;float:left;padding-left:10px;'><span> &nbsp;" + item.Name + " </span></div> ";
                    MonthlyRate = "  <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.MonthlyRate) + " </span></div> ";
                    DiscountRate = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.DiscountRate)+ " </span></div>";
                    Total = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.Total)+ " </span></div>";
                    ServiceList += " <div style='padding-top:5px;'>" + ServiceName + MonthlyRate + DiscountRate + Total + "</div>";
                    if (isServiceDetail == true)
                    {
                        MonthlyService = "<td style='text-align:right;font-weight:bold; border:1px solid #000; padding:0px 10px;'>" + item.Name + "</ td >";
                    }
                    else
                    {
                        MonthlyService = "<td colspan='4' style='text-align:center;font-weight:bold; border:1px solid #000; padding:0px 10px;'>" + item.Name + "</ td >";
                    }
                    Type = "<td style='text-align:center; border:1px solid #000;'>" + item.Category + "</ td >";
                    Monthly = "<td style='text-align:center; font-weight:bold; border:1px solid #000;'>" + currentCurrency +FormatAmount(item.Total) + "</ td >";
                    Qty = "<td style='text-align:center; border:1px solid #000;'>" + item.Quantity + "</ td >";
                    TotalDfw = "<td style='text-align:center; border:1px solid #000;'>" + FormatAmount(item.Total) + "</ td >";
                    if (isServiceDetail == true)
                    {
                        ServiceListDFW += "<tr style='background-color:#bfbfbf; color:#000; height: 30px; border:2px solid #000;'>" + MonthlyService + Type + Monthly + Qty + TotalDfw + "</tr>";
                    }
                    else
                    {
                        ServiceListDFW += "<tr style='background-color:#bfbfbf; color:#000; height: 30px; border:2px solid #000;><td style='width:100%; text-align:center; font-weight:bold;'>" + MonthlyService + "</td></tr>";
                    }
                    CommercialServiceList += "<tr style='border-bottom:1px solid #ccc'>"
                            + "<td style='padding:5px 0px 5px 20px'>" +
                            "<strong>" +
                            "<label>" +
                             item.Name +
                            "</label>" +
                            "</strong></td>"
                            //+ "<td style='padding:5px; text-align:center'>" +
                            //"<strong>" +
                            //"<label>" +
                            //currentCurrency + FormatAmount(item.Total) +
                            //"</label></strong></td></tr>"
                            + "</tr>";
                }
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
                if(agreementPdf.ServiceList.Count == 0)
                {
                    CommercialServiceList = "";
                }
                foreach (var item in agreementPdf.ServiceList)
                {
                    ServiceName = "  <div style = 'width:35%;float:left;padding-left:10px;'><span> &nbsp;" + item.Name + " </span></div> ";
                    MonthlyRate = "  <div style = 'width:20%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.MonthlyRate) + " </span></div> ";
                    DiscountRate = " <div style = 'width:20%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.DiscountRate)  + " </span></div>";
                    Total = " <div style = 'float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.Total)  + " </span></div>";
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
                            if(!string.IsNullOrWhiteSpace(agreementPdf.SubscribedMonths) && agreementPdf.SubscribedMonths.ToLower() != "month to month")
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

                templateVars.Add("ResidentialTechFirstHourCost", currentCurrency + agreementPdf.ResidentialTechFirstHourCost);
                templateVars.Add("CommercialTechFirstHourCost", currentCurrency + agreementPdf.CommercialTechFirstHourCost);

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
                templateVars.Add("CommercialServiceList", CommercialServiceList);
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
                                            "<td>" + agreementPdf.PaymentDetails.BankAccountType + "</td >" +
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
                    NonConfirmingFeeDivRab = string.Format(NonConfirmingFeeDivRab, currentCurrency +FormatAmount(agreementPdf.NonConfirmingFee));

                    NonConfirmingFeeDivDFW = "<tr style='height:25px;'><td valign ='middle' style='font-weight:bold; border:1px solid #000; text-align:right; padding-right:5px; background-color:#f3f3f3;' > +Non Conforming Fee </ td >" +
                                            "<td style='border:1px solid #000;text-align: right;padding-right: 10px;'>{0}</td></ tr >";
                    NonConfirmingFeeDivDFW = string.Format(NonConfirmingFeeDivDFW, currentCurrency + FormatAmount(agreementPdf.NonConfirmingFee));
                }
                templateVars.Add("NonConfirmingFeeDivRab", NonConfirmingFeeDivRab);
                templateVars.Add("NonConfirmingFeeDivDFW", NonConfirmingFeeDivDFW);
                templateVars.Add("NonConfirmingFeeDivADS", NonConfirmingFeeDivDFW);

                #endregion
                #region Advance Monitoring Service Fee
                var AdvanceMonitoringFeeRab = "";
                var AdvanceMonitoringFeeDFW = "";
                if (agreementPdf.AdvanceServiceFeeTotal > 0)
                {
                    AdvanceMonitoringFeeRab = "<div style='width:80%;float:left;padding-left:10px;padding-top:5px'><span>ADVANCE MONITORING FEE</span></div>" +
                                         "<div style='width:18%;float:left;text-align:right;padding-top:5px'>{0}</div>";
                    AdvanceMonitoringFeeRab = string.Format(AdvanceMonitoringFeeRab, currentCurrency + FormatAmount(agreementPdf.AdvanceServiceFeeTotal));

                    AdvanceMonitoringFeeDFW = "<tr style='height:25px;'><td valign ='middle' style='font-weight:bold; border:1px solid #000; text-align:right; padding-right:5px; background-color:#f3f3f3;' > +Advance Monitoring Fee </td>" +
                                            "<td style='border:1px solid #000;text-align: right;padding-right: 10px;'>{0}</td></tr>";
                    AdvanceMonitoringFeeDFW = string.Format(AdvanceMonitoringFeeDFW, currentCurrency + FormatAmount(agreementPdf.AdvanceServiceFeeTotal));
                }
                templateVars.Add("AdvanceMonitoringFeeRab", AdvanceMonitoringFeeRab);
                templateVars.Add("AdvanceMonitoringFeeDFW", AdvanceMonitoringFeeDFW);

                #endregion
                EmailParser parser = null;
                //if (templateid.HasValue && templateid.Value > 0)
                //{
                CustomerAgreementTemplate agreementTemplate = _CustomerAgreementTemplateDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and Id = {1}", agreementPdf.CompanyId, templateid.Value)).FirstOrDefault();
                parser = new EmailParser(agreementTemplate.BodyContent, templateVars, false);
                MailMessage message = new MailMessage();
                message.Body = parser.Parse();
                Body = message.Body;
                //}
                //else
                //{
                //    EmailTemplate agreementTemplate = _EmailTemplateDataAccess.GetEmailTemplateByKeyAndCompanyId(agreementPdf.CompanyId, EmailTemplateKey.SmartAgreement.SmartAgreementRMR);
                //    parser = new EmailParser(HttpContext.Current.Server.MapPath(agreementTemplate.BodyFile), templateVars, true);
                //    MailMessage message = new MailMessage();
                //    message.Body = parser.Parse();
                //    Body = message.Body;
                //}
            }
            catch (Exception ex)
            {
                //Logger.AddElmah(ex);
            }

            return Body;
        }
        #endregion

        //public List<FileTemplate> GetAllFileTemplateListByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId)
        //{
        //    DataTable dt = _FileTemplateDataAccess.GetAllFileTemplateListByCustomerIdAndCompanyId(CustomerId, CompanyId);
        //    List<FileTemplate> FileTemplateList = new List<FileTemplate>();
        //    FileTemplateList = (from DataRow dr in dt.Rows
        //                   select new FileTemplate()
        //                   {
        //                       Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
        //                       CreatedBy = (Guid)dr["CreatedBy"],
        //                       BillingAddress = dr["BillingAddress"].ToString(),
        //                       Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
        //                       TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
        //                       CompanyId = (Guid)dr["CompanyId"],
        //                       CustomerId = (Guid)dr["CustomerId"],
        //                       CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
        //                       CustomerName = dr["CustomerName"].ToString(),
        //                       DiscountAmount = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
        //                       DiscountCode = dr["DiscountCode"].ToString(),
        //                       BookingId = dr["BookingId"].ToString(),
        //                       Status = dr["Status"].ToString(),
        //                       UserNum = dr["UserNum"].ToString(),
        //                       TaxType = dr["TaxType"].ToString(),
        //                       PickUpDate = dr["PickUpDate"] != DBNull.Value ? Convert.ToDateTime(dr["PickUpDate"]) : DateTime.Now,
        //                       Tax = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
        //                       LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : DateTime.Now,
        //                       CustomerViewedTime = dr["CustomerViewedTime"] != DBNull.Value ? Convert.ToDateTime(dr["CustomerViewedTime"]) : new DateTime(),
        //                       CustomerViewedType = dr["CustomerViewedType"].ToString(),
        //                   }).ToList();

        //    return FileTemplateList;
        //}
        public bool InsertFileTemplate(FileTemplate ft)
        {
            return _FileTemplateDataAccess.Insert(ft) > 0;
        }
        public bool UpdateFileTemplate(FileTemplate ft)
        {
            return _FileTemplateDataAccess.Update(ft) > 0;
        }
        public bool InsertAgreementTemplate(ContractAgreementTemplate ft)
        {
            return _ContractAgreementTemplateDataAccess.Insert(ft) > 0;
        }
        public long InsertCustomerAgreementTemplate(CustomerAgreementTemplate ft)
        {
            return _CustomerAgreementTemplateDataAccess.Insert(ft);
        }
        public bool UpdateAgreementTemplate(ContractAgreementTemplate ft)
        {
            return _ContractAgreementTemplateDataAccess.Update(ft) > 0;
        }
        public bool UpdateCustomerAgreementTemplate(CustomerAgreementTemplate ft)
        {
            return _CustomerAgreementTemplateDataAccess.Update(ft) > 0;
        }
        public List<ContractAgreementTemplate> GetAllContractAgreeemntTemplate()
        {
            return _ContractAgreementTemplateDataAccess.GetAll();
        }

        public ContractAgreementTemplate GetAgreementTemplateById(int value)
        {
            return _ContractAgreementTemplateDataAccess.Get(value);
        }
        public CustomerAgreementTemplate GetCustomerAgreementTemplateById(int value)
        {
            return _CustomerAgreementTemplateDataAccess.Get(value);
        }
        public CustomerAgreementTemplate GetCustomerAgreementTemplateByReferenceTemplateId(int value, Guid cusId, bool isFile)
        {
            return _CustomerAgreementTemplateDataAccess.GetByQuery(string.Format("ReferenceTemplateId ='{0}' and CustomerId='{1}' and IsFileTemplate='{2}'", value, cusId, isFile)).OrderByDescending(x => x.Id).FirstOrDefault();
        }
    }
}
//[~Shariful-30-9-19]