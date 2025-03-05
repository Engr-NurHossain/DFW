using HS.DataAccess;
using HS.Framework;
using HS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using HS.Entities.List;
using Forte;
using Forte.Entities;
using RestSharp;
using System.Security.Authentication;
using System.Net;

namespace HS.Facade
{
    public class CustomerFacade : BaseFacade
    {
        MMRDataAccess _MMRDataAccess = null;
        ActivationFeeDataAccess _ActivationFeeDataAccess = null;
        public CustomerFacade(ClientContext clientContext)
            : base(clientContext)
        {
            if (_MMRDataAccess == null)
                _MMRDataAccess = (MMRDataAccess)_ClientContext[typeof(MMRDataAccess)];
            if (_ActivationFeeDataAccess == null)
                _ActivationFeeDataAccess = (ActivationFeeDataAccess)_ClientContext[typeof(ActivationFeeDataAccess)];
        }
        public CustomerFacade()
        {
            if (_MMRDataAccess == null)
                _MMRDataAccess = new MMRDataAccess();
            if (_ActivationFeeDataAccess == null)
                _ActivationFeeDataAccess = new ActivationFeeDataAccess();
        }
        CustomerContactTrackDataAccess _CustomerContactTrackDataAccess
        {
            get
            {
                return (CustomerContactTrackDataAccess)_ClientContext[typeof(CustomerContactTrackDataAccess)];
            }
        }
        CustomerDataAccess _CustomerDataAccess
        {
            get
            {
                return (CustomerDataAccess)_ClientContext[typeof(CustomerDataAccess)];
            }
        }
        CustomerSecurityZonesDataAccess _CustomerSecurityZonesDataAccess
        {
            get
            {
                return (CustomerSecurityZonesDataAccess)_ClientContext[typeof(CustomerSecurityZonesDataAccess)];
            }
        }
        CustomerThirdPartyAgencyDataAccess _CustomerThirdPartyAgencyDataAccess
        {
            get
            {
                return (CustomerThirdPartyAgencyDataAccess)_ClientContext[typeof(CustomerThirdPartyAgencyDataAccess)];
            }
        }
        ThirdPartyEmergencyContactDataAccess _ThirdPartyEmergencyContactDataAccess
        {
            get
            {
                return (ThirdPartyEmergencyContactDataAccess)_ClientContext[typeof(ThirdPartyEmergencyContactDataAccess)];
            }
        }
        ZonesEquipmentLocationDataAccess _ZonesEquipmentLocationDataAccess
        {
            get
            {
                return (ZonesEquipmentLocationDataAccess)_ClientContext[typeof(ZonesEquipmentLocationDataAccess)];
            }
        }
        ZonesEquipmentTypeDataAccess _ZonesEquipmentTypeDataAccess
        {
            get
            {
                return (ZonesEquipmentTypeDataAccess)_ClientContext[typeof(ZonesEquipmentTypeDataAccess)];
            }
        }
        ZonesEquipmentTypeEventMapDataAccess _ZonesEquipmentTypeEventMapDataAccess
        {
            get
            {
                return (ZonesEquipmentTypeEventMapDataAccess)_ClientContext[typeof(ZonesEquipmentTypeEventMapDataAccess)];
            }
        }
        AlarmCustomerTerminationDataAccess _TerminationDataAccess
        {
            get
            {
                return (AlarmCustomerTerminationDataAccess)_ClientContext[typeof(AlarmCustomerTerminationDataAccess)];
            }
        }
        CustomerAddressDataAccess _CustomerAddressDataAccess
        {
            get
            {
                return (CustomerAddressDataAccess)_ClientContext[typeof(CustomerAddressDataAccess)];
            }
        }
        EmergencyContactDataAccess _EmergencyContactDataAccess
        {
            get
            {
                return (EmergencyContactDataAccess)_ClientContext[typeof(EmergencyContactDataAccess)];
            }
        }
        CompanyDataAccess _CompanyDataAccess
        {
            get
            {
                return (CompanyDataAccess)_ClientContext[typeof(CompanyDataAccess)];
            }
        }

        CustomerCreditCheckDataAccess _CustomerCreditCheckDataAccess
        {
            get
            {
                return (CustomerCreditCheckDataAccess)_ClientContext[typeof(CustomerCreditCheckDataAccess)];
            }
        }

        CustomerCompanyDataAccess _CustomerCompanyDataAccess
        {
            get
            {
                return (CustomerCompanyDataAccess)_ClientContext[typeof(CustomerCompanyDataAccess)];
            }
        }
        InvoiceDataAccess _InvoiceDataAccess
        {
            get
            {
                return (InvoiceDataAccess)_ClientContext[typeof(InvoiceDataAccess)];
            }
        }

        CustomerFileDataAccess _CustomerFileDataAccess
        {
            get
            {
                return (CustomerFileDataAccess)_ClientContext[typeof(CustomerFileDataAccess)];
            }
        }
        EquipmentDataAccess _EquipmentDataAccess
        {
            get
            {
                return (EquipmentDataAccess)_ClientContext[typeof(EquipmentDataAccess)];
            }
        }
        PaymentProfileCustomerDataAccess _PaymentProfileCustomerDataAccess
        {
            get
            {
                return (PaymentProfileCustomerDataAccess)_ClientContext[typeof(PaymentProfileCustomerDataAccess)];
            }
        }

        CustomerExtendedDataAccess _CustomerExtendedDataAccess
        {
            get
            {
                return (CustomerExtendedDataAccess)_ClientContext[typeof(CustomerExtendedDataAccess)];
            }
        }

        ResturantOrderDetailDataAccess _ResturantOrderDetailDataAccess
        {
            get
            {
                return (ResturantOrderDetailDataAccess)_ClientContext[typeof(ResturantOrderDetailDataAccess)];
            }
        }

        public CustomerContactTrack GetCustomerContactTrackByPlatformId(int id)
        {
            return _CustomerContactTrackDataAccess.GetByQuery(string.Format(" PlatformId = '{0}' ", id)).FirstOrDefault();
        }

        CustomerNoteDataAccess _CustomerNoteDataAccess
        {
            get
            {
                return (CustomerNoteDataAccess)_ClientContext[typeof(CustomerNoteDataAccess)];
            }
        }

        CustomerCancellationQueueDataAccess _CustomerCancellationQueueDataAccess
        {
            get
            {
                return (CustomerCancellationQueueDataAccess)_ClientContext[typeof(CustomerCancellationQueueDataAccess)];
            }
        }
        CustomerSystemInfoDataAccess _CustomerSystemInfoDataAccess
        {
            get
            {
                return (CustomerSystemInfoDataAccess)_ClientContext[typeof(CustomerSystemInfoDataAccess)];
            }
        }
        CustomerApiSettingDataAccess _CustomerApiSettingDataAccess
        {
            get
            {
                return (CustomerApiSettingDataAccess)_ClientContext[typeof(CustomerApiSettingDataAccess)];
            }
        }
        CustomerViewDataAccess _CustomerViewDataAccess
        {
            get
            {
                return (CustomerViewDataAccess)_ClientContext[typeof(CustomerViewDataAccess)];
            }
        }
        CustomerSystemNoDataAccess _CustomerSystemNoDataAccess
        {
            get
            {
                return (CustomerSystemNoDataAccess)_ClientContext[typeof(CustomerSystemNoDataAccess)];
            }
        }

        CustomerNoPrefixDataAccess _CustomerNoPrefixDataAccess
        {
            get
            {
                return (CustomerNoPrefixDataAccess)_ClientContext[typeof(CustomerNoPrefixDataAccess)];
            }
        }
        CustomerAppointmentEquipmentDataAccess _CustomerAppointmentEquipmentDataAccess
        {
            get
            {
                return (CustomerAppointmentEquipmentDataAccess)_ClientContext[typeof(CustomerAppointmentEquipmentDataAccess)];
            }
        }
        ServiceFeeDataAccess _ServiceFeeDataAccess
        {
            get
            {
                return (ServiceFeeDataAccess)_ClientContext[typeof(ServiceFeeDataAccess)];
            }
        }
        AccountHolderDataAccess _AccountHolderDataAccess
        {
            get
            {
                return (AccountHolderDataAccess)_ClientContext[typeof(AccountHolderDataAccess)];
            }
        }
        CustomerExistingItemDataAccess _CustomerExistingItemDataAccess
        {
            get
            {
                return (CustomerExistingItemDataAccess)_ClientContext[typeof(CustomerExistingItemDataAccess)];
            }
        }
        GlobalSettingDataAccess _GlobalSettingDataAccess
        {
            get
            {
                return new GlobalSettingDataAccess();
            }
        }
        QaAnswerDataAccess _QaAnswerDataAccess
        {
            get
            {
                return (QaAnswerDataAccess)_ClientContext[typeof(QaAnswerDataAccess)];
            }
        }
        CustomerCancelDataAccess _CustomerCancelDataAccess
        {
            get
            {
                return (CustomerCancelDataAccess)_ClientContext[typeof(CustomerCancelDataAccess)];
            }
        }
        CustomerSpouseDataAccess _CustomerSpouseDataAccess
        {
            get
            {
                return (CustomerSpouseDataAccess)_ClientContext[typeof(CustomerSpouseDataAccess)];
            }
        }

        CustomerVaultDataAccess _CustomerVaultDataAccess
        {
            get
            {
                return (CustomerVaultDataAccess)_ClientContext[typeof(CustomerVaultDataAccess)];
            }
        }

        AnnouncementDataAccess _AnnouncementDataAccess
        {
            get
            {
                return (AnnouncementDataAccess)_ClientContext[typeof(AnnouncementDataAccess)];
            }
        }
        EmployeeDataAccess _EmployeeDataAccess
        {
            get
            {
                return (EmployeeDataAccess)_ClientContext[typeof(EmployeeDataAccess)];
            }
        }
        CustomerCreditDataAccess _CustomerCreditDataAccess
        {
            get
            {
                return (CustomerCreditDataAccess)_ClientContext[typeof(CustomerCreditDataAccess)];
            }
        }
        CustomerAddendumDataAccess _CustomerAddendumDataAccess
        {
            get
            {
                return (CustomerAddendumDataAccess)_ClientContext[typeof(CustomerAddendumDataAccess)];
            }
        }
        CustomerMigrationDataAccess _CustomerMigrationDataAccess
        {
            get
            {
                return (CustomerMigrationDataAccess)_ClientContext[typeof(CustomerMigrationDataAccess)];
            }
        }

        CreditScoreGradeDataAccess _CreditScoreGradeDataAccess
        {
            get
            {
                return (CreditScoreGradeDataAccess)_ClientContext[typeof(CreditScoreGradeDataAccess)];
            }
        }

        ThirdPartyCustomerDataAccess _ThirdPartyCustomerDataAccess
        {
            get
            {
                return (ThirdPartyCustomerDataAccess)_ClientContext[typeof(ThirdPartyCustomerDataAccess)];
            }
        }
        ResturantOrderDataAccess _ResturantOrderDataAccess
        {
            get
            {
                return (ResturantOrderDataAccess)_ClientContext[typeof(ResturantOrderDataAccess)];
            }
        }
        public CustomerVaultList GetCustomerVaultList()
        {
            return _CustomerVaultDataAccess.GetAll();
        }
        public Customer GetCustomerById(int customerId)
        {
            return _CustomerDataAccess.Get(customerId);
        }
        public Customer GetCustomerById(Guid customerId)
        {
            return _CustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerId)).FirstOrDefault();
        }
        public CustomerExtended GetCustomerExtendedByCustomerId(Guid customerId)
        {
            return _CustomerExtendedDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerId)).FirstOrDefault();
        }
        public Customer GetCustomerByCentralStationRefId(int id)
        {
            return _CustomerDataAccess.GetByQuery(string.Format(" CentralStationRefId = '{0}' ", id)).FirstOrDefault();
        }

        public Customer GetCustomerByPhoneNoOrEmail(string phoneNo, string email)
        {
            string phoneSearchSql = "";
            string emailSearchSql = "";
            string sql = "";

            if (!string.IsNullOrWhiteSpace(phoneNo))
            {

                if (phoneNo.Length == 12)
                {
                    string TempphoneNo = phoneNo.Substring(2, phoneNo.Length - 2);

                    long PhoneNumber = 0;
                    if (long.TryParse(TempphoneNo, out PhoneNumber))
                    {
                        phoneNo = String.Format("{0:###-###-####}", PhoneNumber);
                    }
                }
                phoneSearchSql = string.Format(" PrimaryPhone = '{0}' or SecondaryPhone = '{0}' or CellNo = '{0}' ", phoneNo);
            }
            if (!string.IsNullOrWhiteSpace(email))
            {
                emailSearchSql = string.Format(" EmailAddress = '{0}'", email);
            }

            if (phoneSearchSql != "" && emailSearchSql != "")
            {
                sql = phoneSearchSql + " or " + emailSearchSql;
            }
            else if (phoneSearchSql == "" && emailSearchSql != "")
            {
                sql = emailSearchSql;
            }
            else if (phoneSearchSql != "" && emailSearchSql == "")
            {
                sql = phoneSearchSql;
            }
            else
            {
                return null;
            }
            return _CustomerDataAccess.GetByQuery(sql).FirstOrDefault();
        }

        public string GetCustomerNameById(Guid customerId)
        {
            string CustomerName = "";
            string sql = string.Format("CustomerId='{0}'", customerId);
            var CustomerDetails = _CustomerDataAccess.GetByQuery(sql).FirstOrDefault();
            if (CustomerDetails != null)
            {
                CustomerName = CustomerDetails.FirstName + " " + CustomerDetails.LastName;
            }
            return CustomerName;
        }
        public CustomerCompany GetCustomerCompanyByCustomerId(int customerId)
        {
            Customer cus = _CustomerDataAccess.Get(customerId);
            if (cus == null)
                return null;
            return _CustomerCompanyDataAccess.GetByQuery(string.Format("[CustomerId]='{0}'", cus.CustomerId.ToString())).FirstOrDefault();
        }
        public List<Customer> GetAllCustomers()
        {
            return _CustomerDataAccess.GetAll();
        }
        public List<CustomerExistingItem> GetAllCustomerExistingItem()
        {
            return _CustomerExistingItemDataAccess.GetAll();
        }


        #region Responsible person

        /*public Customer GetAllCustomerResponsibleByCustomerId(Guid customerid)
        {
            DataTable dt = _CustomerDataAccess.GetAllCustomerResponsibleByCustomerId(customerid);
            Customer Responsiblelist = new Customer();
            Responsiblelist = (from DataRow dr in dt.Rows
                               select new Customer()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   ResponsiblePerson1 = dr["ResponsiblePerson1"].ToString(),
                                   ResponsiblePerson2 = dr["ResponsiblePerson2"].ToString(),
                                   ResponsiblePerson3 = dr["ResponsiblePerson3"].ToString(),
                                   ResponsiblePerson4 = dr["ResponsiblePerson4"].ToString(),
                                   ResponsiblePersonContact1 = dr["ResponsiblePersonContact1"].ToString(),
                                   ResponsiblePersonContact2 = dr["ResponsiblePersonContact2"].ToString(),
                                   ResponsiblePersonContact3 = dr["ResponsiblePersonContact3"].ToString(),
                                   ResponsiblePersonContact4 = dr["ResponsiblePersonContact4"].ToString(),
                                   ResponsiblePersonEmail1 = dr["ResponsiblePersonEmail1"].ToString(),
                                   ResponsiblePersonEmail2 = dr["ResponsiblePersonEmail2"].ToString(),
                                   ResponsiblePersonEmail3 = dr["ResponsiblePersonEmail3"].ToString(),
                                   ResponsiblePersonEmail4 = dr["ResponsiblePersonEmail4"].ToString(),
                               }).FirstOrDefault();
            return Responsiblelist;
        }*/
        #endregion

        public Customer GetCustomersById(int value)
        {
            //return _CustomerDataAccess.Get(value);

            return _CustomerDataAccess.GetCustomersById(value);
        }
        public Customer GetCustomersByIdAndSoldBy(int value, Guid SoldBy, string EmployeeRole, string userRole)
        {
            //return _CustomerDataAccess.Get(value);

            return _CustomerDataAccess.GetCustomersByIdAndSoldBy(value, SoldBy, EmployeeRole, userRole);
        }
        public DataTable GetAllLeadsReportByCompany(Guid CompanyId, DateTime? Start, DateTime? End, string soldBy)
        {
            return _CustomerDataAccess.GetAllLeadsReportByCompany(CompanyId, Start, End, soldBy);
        }
        public DataTable GetAllHudsonLeadsReportByCompany(Guid CompanyId, DateTime? Start, DateTime? End, string status, string market, string leads, string soldBy)
        {
            return _CustomerDataAccess.GetAllHudsonLeadsReportByCompany(CompanyId, Start, End, status, market, leads, soldBy);
        }
        public DataTable GetAllCustomerReportByCompany(Guid CompanyId, DateTime? Start, DateTime? End)
        {
            return _CustomerDataAccess.GetAllCustomerReportByCompany(CompanyId, Start, End);
        }
        public DataTable GetAllInactiveCustomerReportByCompany(Guid CompanyId, DateTime? Start, DateTime? End)
        {
            return _CustomerDataAccess.GetAllInactiveCustomerReportByCompany(CompanyId, Start, End);
        }
        public DataTable GetAllHudsonCustomerReportByCompany(Guid CompanyId, DateTime? Start, DateTime? End, List<string> Status, string market, string soldBy)
        {
            return _CustomerDataAccess.GetAllHudsonCustomerReportByCompany(CompanyId, Start, End, Status, market, soldBy);
        }
        public DataTable GetAllConvertedCustomerReportByCompany(Guid CompanyId, DateTime? Start, DateTime? End, FilterReportModel filter)
        {
            return _CustomerDataAccess.GetAllConvertCustomerReportByCompany(CompanyId, Start, End, filter);
        }
        public DataTable GetAllDelinquentCustomerReportByCompany(Guid CompanyId, DateTime? Start, DateTime? End, string id, string searchtext, string unpaid)
        {
            return _CustomerDataAccess.GetAllDelinquentCustomerReportByCompany(CompanyId, Start, End, id, searchtext, unpaid);
        }
        public List<Invoice> GetAllDelinquentCustomerReportByCompanyCount(Guid CompanyId, DateTime? Start, DateTime? End, string id, string searchtext, string unpaid)
        {
            DataTable dt = _CustomerDataAccess.GetAllDelinquentCustomerReportByCompany(CompanyId, Start, End, id, searchtext, unpaid);
            List<Invoice> propertyList = new List<Invoice>();
            propertyList = (from DataRow dr in dt.Rows
                            select new Invoice()
                            {
                                //Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                //ContactName = dr["ContactName"].ToString(),
                                //CustomerNo = dr["CustomerNo"].ToString(),
                                //EmailAddress = dr["EmailAddress"].ToString(),
                                //Address = dr["Address"].ToString(),
                                CustomerName = dr["Customer Name"].ToString()
                                //CallingTime = dr["CallingTime"].ToString(),
                                //CellNo = dr["CellNo"].ToString(),
                                //CellularBackup = (dr["CellularBackup"] != DBNull.Value ? Convert.ToBoolean(dr["CellularBackup"]) : false),
                                //City = dr["City"].ToString(),
                                //ContractTeam = dr["ContractTeam"].ToString(),
                                //Country = dr["Country"].ToString(),
                                //CreditScore = dr["CreditScore"].ToString(),
                                //CustomerFunded = (dr["CustomerFunded"] != DBNull.Value ? Convert.ToBoolean(dr["CustomerFunded"]) : false),
                                //CustomerId = (Guid)dr["CustomerId"],
                                //Fax = dr["Fax"].ToString(),
                                //FirstName = dr["FirstName"].ToString(),
                                //FundingCompany = dr["FundingCompany"].ToString(),
                                //IsAlarmCom = dr["IsAlarmCom"] != DBNull.Value ? Convert.ToBoolean(dr["IsAlarmCom"]) : false,
                                //LastName = dr["LastName"].ToString(),
                                //LeadSource = dr["LeadSource"].ToString(),
                                //Maintenance = dr["Maintenance"] != DBNull.Value ? Convert.ToBoolean(dr["Maintenance"]) : false,
                                //MonthlyMonitoringFee = dr["MonthlyMonitoringFee"].ToString(),
                                //Note = dr["Note"].ToString(),
                                //PrimaryPhone = dr["PrimaryPhone"].ToString(),
                                //SecondaryPhone = dr["SecondaryPhone"].ToString(),
                                //SSN = dr["SSN"].ToString(),
                                //State = dr["State"].ToString(),
                                //Street = dr["Street"].ToString(),
                                //JoinDate = dr["JoinDate"] != DBNull.Value ? Convert.ToDateTime(dr["JoinDate"]) : new DateTime(),
                                //Title = dr["Title"].ToString(),
                                //Type = dr["Type"].ToString(),
                                //ZipCode = dr["ZipCode"].ToString(),
                                //DateofBirth = dr["DateofBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DateofBirth"]) : new DateTime(),
                                //MiddleName = dr["MiddleName"].ToString(),
                                //LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                                //LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime()
                                //Status = dr["Status"].ToString(),
                                //IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                                //Street = dr["Street"].ToString(),
                                //City = dr["City"].ToString(),
                                //State = dr["State"].ToString(),
                                //ZipCode = dr["ZipCode"].ToString(),
                                //AnnualRevenue = dr["AnnualRevenue"] != DBNull.Value ? Convert.ToDouble(dr["AnnualRevenue"]) : 0,
                                //AccountType = dr["AccountType"].ToString(),
                                //PersonSales = dr["PersonSales"].ToString(),
                                //MarketVal = dr["MarketVal"].ToString(),
                                //DBA = dr["DBA"].ToString()
                            }).ToList();
            return propertyList;
        }
        public List<Customer> GetAllTransferCustomerReportByCompanyCount(Guid CompanyId, DateTime? Start, DateTime? End, FilterReportModel filter)
        {
            //return _CustomerDataAccess.GetAllTransferCustomerReportByCompany(CompanyId, Start, End);
            DataTable dt = _CustomerDataAccess.GetAllTransferCustomerReportByCompany(CompanyId, Start, End, filter);
            List<Customer> propertyList = new List<Customer>();
            propertyList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                //ContactName = dr["ContactName"].ToString(),
                                //CustomerNo = dr["CustomerNo"].ToString(),
                                EmailAddress = dr["Email"].ToString(),
                                Address = dr["Address"].ToString(),
                                FullName = dr["Customer Name"].ToString(),
                                //CallingTime = dr["CallingTime"].ToString(),
                                //CellNo = dr["CellNo"].ToString(),
                                //CellularBackup = (dr["CellularBackup"] != DBNull.Value ? Convert.ToBoolean(dr["CellularBackup"]) : false),
                                //City = dr["City"].ToString(),
                                //ContractTeam = dr["ContractTeam"].ToString(),
                                //Country = dr["Country"].ToString(),
                                //CreditScore = dr["CreditScore"].ToString(),
                                //CustomerFunded = (dr["CustomerFunded"] != DBNull.Value ? Convert.ToBoolean(dr["CustomerFunded"]) : false),
                                //CustomerId = (Guid)dr["CustomerId"],
                                //Fax = dr["Fax"].ToString(),
                                //FirstName = dr["FirstName"].ToString(),
                                //FundingCompany = dr["FundingCompany"].ToString(),
                                //IsAlarmCom = dr["IsAlarmCom"] != DBNull.Value ? Convert.ToBoolean(dr["IsAlarmCom"]) : false,
                                //LastName = dr["LastName"].ToString(),
                                //LeadSource = dr["LeadSource"].ToString(),
                                //Maintenance = dr["Maintenance"] != DBNull.Value ? Convert.ToBoolean(dr["Maintenance"]) : false,
                                //MonthlyMonitoringFee = dr["MonthlyMonitoringFee"].ToString(),
                                //Note = dr["Note"].ToString(),
                                PrimaryPhone = dr["Phone No"].ToString()
                                //SecondaryPhone = dr["SecondaryPhone"].ToString(),
                                //SSN = dr["SSN"].ToString(),
                                //State = dr["State"].ToString(),
                                //Street = dr["Street"].ToString(),
                                //JoinDate = dr["JoinDate"] != DBNull.Value ? Convert.ToDateTime(dr["JoinDate"]) : new DateTime(),
                                //Title = dr["Title"].ToString(),
                                //Type = dr["Type"].ToString(),
                                //ZipCode = dr["ZipCode"].ToString(),
                                //DateofBirth = dr["DateofBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DateofBirth"]) : new DateTime(),
                                //MiddleName = dr["MiddleName"].ToString(),
                                //LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                                //LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime()
                                //Status = dr["Status"].ToString(),
                                //IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                                //Street = dr["Street"].ToString(),
                                //City = dr["City"].ToString(),
                                //State = dr["State"].ToString(),
                                //ZipCode = dr["ZipCode"].ToString(),
                                //AnnualRevenue = dr["AnnualRevenue"] != DBNull.Value ? Convert.ToDouble(dr["AnnualRevenue"]) : 0,
                                //AccountType = dr["AccountType"].ToString(),
                                //PersonSales = dr["PersonSales"].ToString(),
                                //MarketVal = dr["MarketVal"].ToString(),
                                //DBA = dr["DBA"].ToString()
                            }).ToList();
            return propertyList;
        }
        public DataTable GetAllTransferCustomerReportByCompany(Guid CompanyId, DateTime? Start, DateTime? End, FilterReportModel filter)
        {
            return _CustomerDataAccess.GetAllTransferCustomerReportByCompany(CompanyId, Start, End, filter);
        }
        public DataTable GetAllTicketReportByCompany(Guid CompanyId, DateTime? Start, DateTime? End, TicketFilter Filters, FilterReportModel filter)
        {
            return _CustomerDataAccess.GetAllTicketReportByCompany(CompanyId, Start, End, Filters, filter);
        }

        public DataTable GetAllTechCommissionReport(DateTime? Start, DateTime? End)
        {
            return _CustomerDataAccess.GetAllTechCommissionReport(Start, End);
        }

        public DataTable GetAllAdditionalMemberCommissionReport(DateTime? Start, DateTime? End)
        {
            return _CustomerDataAccess.GetAllAdditionalMemberCommissionReport(Start, End);
        }
        public DataTable GetAllServiceCallCommissionReport(DateTime? Start, DateTime? End)
        {
            return _CustomerDataAccess.GetAllServiceCallCommissionReport(Start, End);
        }
        public DataTable GetAllRescheduleCommissionReport(DateTime? Start, DateTime? End)
        {
            return _CustomerDataAccess.GetAllRescheduleCommissionReport(Start, End);
        }

        public DataTable GetAllFollowupCommissionReport(DateTime? Start, DateTime? End)
        {
            return _CustomerDataAccess.GetAllFollowUpCommissionReport(Start, End);
        }
        public DataTable GetAllSalesCommissionReport(DateTime? Start, DateTime? End)
        {
            return _CustomerDataAccess.GetAllSalesCommissionReport(Start, End);
        }

        public DataTable GetAllTimeClockReport(DateTime? Start, DateTime? End)
        {
            return _CustomerDataAccess.GetAllTimeClockReport(Start, End);
        }

        public DataTable GetAllTicketReportByCompanyForAppointmentDate(Guid CompanyId, DateTime? Start, DateTime? End, TicketFilter Filters, FilterReportModel filter)
        {
            return _CustomerDataAccess.GetAllTicketReportByCompanyForAppointmentDate(CompanyId, Start, End, Filters, filter);
        }

        public DataTable GetTicketListInstallReportByFilter(Guid CompanyId, DateTime? Start, DateTime? End, TicketFilter Filters)
        {
            return _EmployeeDataAccess.GetTicketListInstallReportByFilter(CompanyId, Start, End, Filters);
        }

        public DataTable GetAllTicketReportByCompanyForGoBack(Guid CompanyId, DateTime? Start, DateTime? End, TicketFilter Filters, FilterReportModel filter)
        {
            return _CustomerDataAccess.GetAllTicketReportByCompanyForGoBack(CompanyId, Start, End, Filters, filter);
        }

        #region Leads Report
        //public List<Customer> GetAllLeadsByCompany(Guid CompanyId)
        //{
        //    DataTable dt = _CustomerDataAccess.GetAllLeadsByCompany(CompanyId, null, null);
        //    List<Customer> propertyList = new List<Customer>();
        //    propertyList = (from DataRow dr in dt.Rows
        //                    select new Customer()
        //                    {
        //                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
        //                        //ContactName = dr["ContactName"].ToString(),
        //                        AccountNo = dr["AccountNo"].ToString(),
        //                        EmailAddress = dr["EmailAddress"].ToString(),
        //                        Address = dr["Address"].ToString(),
        //                        BusinessName = dr["BusinessName"].ToString(),
        //                        CallingTime = dr["CallingTime"].ToString(),
        //                        CellNo = dr["CellNo"].ToString(),
        //                        CellularBackup = (dr["CellularBackup"] != DBNull.Value ? Convert.ToBoolean(dr["CellularBackup"]) : false),
        //                        City = dr["City"].ToString(),
        //                        ContractTeam = dr["ContractTeam"].ToString(),
        //                        Country = dr["Country"].ToString(),
        //                        CreditScore = dr["CreditScore"].ToString(),
        //                        CustomerFunded = (dr["CustomerFunded"] != DBNull.Value ? Convert.ToBoolean(dr["CustomerFunded"]) : false),
        //                        CustomerId = (Guid)dr["CustomerId"],
        //                        Fax = dr["Fax"].ToString(),
        //                        FirstName = dr["FirstName"].ToString(),
        //                        FundingCompany = dr["FundingCompany"].ToString(),
        //                        IsAlarmCom = dr["IsAlarmCom"] != DBNull.Value ? Convert.ToBoolean(dr["IsAlarmCom"]) : false,
        //                        LastName = dr["LastName"].ToString(),
        //                        LeadSource = dr["LeadSource"].ToString(),
        //                        Maintenance = dr["Maintenance"] != DBNull.Value ? Convert.ToBoolean(dr["Maintenance"]) : false,
        //                        MonthlyMonitoringFee = dr["MonthlyMonitoringFee"].ToString(),
        //                        Note = dr["Note"].ToString(),
        //                        PrimaryPhone = dr["PrimaryPhone"].ToString(),
        //                        SecondaryPhone = dr["SecondaryPhone"].ToString(),
        //                        SSN = dr["SSN"].ToString(),
        //                        State = dr["State"].ToString(),
        //                        Street = dr["Street"].ToString(),
        //                        JoinDate = dr["JoinDate"] != DBNull.Value ? Convert.ToDateTime(dr["JoinDate"]) : new DateTime(),
        //                        Title = dr["Title"].ToString(),
        //                        Type = dr["Type"].ToString(),
        //                        ZipCode = dr["ZipCode"].ToString(),
        //                        DateofBirth = dr["DateofBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DateofBirth"]) : new DateTime(),
        //                        MiddleName = dr["MiddleName"].ToString(),
        //                        LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
        //                        LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime()
        //                    }).ToList();
        //    return propertyList;
        //}

        public List<Customer> GetAllCustomerByCompany(Guid CompanyId, int pageno, int pagesize, List<string> Status, string market, string soldBy)
        {
            DataTable dt = _CustomerDataAccess.GetAllCustomerByCompany(CompanyId, null, null, pageno, pagesize, Status, market, soldBy);
            List<Customer> propertyList = new List<Customer>();
            propertyList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                CustomerNo = dr["CustomerNo"].ToString(),
                                EmailAddress = dr["EmailAddress"].ToString(),
                                Address = dr["Address"].ToString(),
                                Street=dr["Street"].ToString(),
                                City=dr["City"].ToString(),
                                ZipCode=dr["ZipCode"].ToString(),
                                State=dr["State"].ToString(),
                                BusinessName = dr["BusinessName"].ToString(),
                                CustomerId = (Guid)dr["CustomerId"],
                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                PrimaryPhone = dr["PrimaryPhone"].ToString(),
                                JoinDate = dr["JoinDate"] != DBNull.Value ? Convert.ToDateTime(dr["JoinDate"]) : new DateTime(),
                                Status = dr["Status"].ToString(),
                                IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                                
                                AnnualRevenue = dr["AnnualRevenue"] != DBNull.Value ? Convert.ToDouble(dr["AnnualRevenue"]) : 0,
                                AccountType = dr["AccountType"].ToString(),
                                PersonSales = dr["PersonSales"].ToString(),
                                MarketVal = dr["MarketVal"].ToString(),
                                DBA = dr["DBA"].ToString(),
                                CancellationDate = dr["CancellationDate"] != DBNull.Value ? Convert.ToDateTime(dr["CancellationDate"]) : new DateTime()
                            }).ToList();
            return propertyList;
        }

        public DataTable GetAllCustomerByCompanyCancelReport(Guid CompanyId, DateTime? startdate, DateTime? endtime, int pageno, int pagesize, List<string> Status, string market)
        {
            return _CustomerDataAccess.GetAllCustomerByCompanyCancelReport(CompanyId, startdate, endtime, pageno, pagesize, Status, market);
        }

        public List<Customer> GetAllCustomerByCompanyCount(Guid CompanyId, DateTime? start, DateTime? end, List<string> Status, string market, string soldBy)
        {
            DataTable dt = _CustomerDataAccess.GetAllCustomerByCompanyCount(CompanyId, start, end, Status, market, soldBy);
            List<Customer> propertyList = new List<Customer>();
            propertyList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                //ContactName = dr["ContactName"].ToString(),
                                CustomerNo = dr["CustomerNo"].ToString(),
                                EmailAddress = dr["EmailAddress"].ToString(),
                                Address = dr["Address"].ToString(),
                                BusinessName = dr["BusinessName"].ToString(),
                                //CallingTime = dr["CallingTime"].ToString(),
                                //CellNo = dr["CellNo"].ToString(),
                                //CellularBackup = (dr["CellularBackup"] != DBNull.Value ? Convert.ToBoolean(dr["CellularBackup"]) : false),
                                //City = dr["City"].ToString(),
                                //ContractTeam = dr["ContractTeam"].ToString(),
                                //Country = dr["Country"].ToString(),
                                //CreditScore = dr["CreditScore"].ToString(),
                                //CustomerFunded = (dr["CustomerFunded"] != DBNull.Value ? Convert.ToBoolean(dr["CustomerFunded"]) : false),
                                CustomerId = (Guid)dr["CustomerId"],
                                //Fax = dr["Fax"].ToString(),
                                FirstName = dr["FirstName"].ToString(),
                                //FundingCompany = dr["FundingCompany"].ToString(),
                                //IsAlarmCom = dr["IsAlarmCom"] != DBNull.Value ? Convert.ToBoolean(dr["IsAlarmCom"]) : false,
                                LastName = dr["LastName"].ToString(),
                                //LeadSource = dr["LeadSource"].ToString(),
                                //Maintenance = dr["Maintenance"] != DBNull.Value ? Convert.ToBoolean(dr["Maintenance"]) : false,
                                //MonthlyMonitoringFee = dr["MonthlyMonitoringFee"].ToString(),
                                //Note = dr["Note"].ToString(),
                                PrimaryPhone = dr["PrimaryPhone"].ToString(),
                                //SecondaryPhone = dr["SecondaryPhone"].ToString(),
                                //SSN = dr["SSN"].ToString(),
                                //State = dr["State"].ToString(),
                                //Street = dr["Street"].ToString(),
                                JoinDate = dr["JoinDate"] != DBNull.Value ? Convert.ToDateTime(dr["JoinDate"]) : new DateTime(),
                                //Title = dr["Title"].ToString(),
                                //Type = dr["Type"].ToString(),
                                //ZipCode = dr["ZipCode"].ToString(),
                                //DateofBirth = dr["DateofBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DateofBirth"]) : new DateTime(),
                                //MiddleName = dr["MiddleName"].ToString(),
                                //LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                                //LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime()
                                Status = dr["Status"].ToString(),
                                IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                                Street = dr["Street"].ToString(),
                                City = dr["City"].ToString(),
                                State = dr["State"].ToString(),
                                ZipCode = dr["ZipCode"].ToString(),
                                AnnualRevenue = dr["AnnualRevenue"] != DBNull.Value ? Convert.ToDouble(dr["AnnualRevenue"]) : 0,
                                AccountType = dr["AccountType"].ToString(),
                                PersonSales = dr["PersonSales"].ToString(),
                                MarketVal = dr["MarketVal"].ToString(),
                                DBA = dr["DBA"].ToString()
                            }).ToList();
            return propertyList;
        }
        public List<Customer> GetAllInactiveCustomerByCompanyCount(Guid CompanyId, DateTime? start, DateTime? end)
        {
            DataTable dt = _CustomerDataAccess.GetAllInactiveCustomerByCompanyCount(CompanyId, start, end);
            List<Customer> propertyList = new List<Customer>();
            propertyList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                //ContactName = dr["ContactName"].ToString(),
                                CustomerNo = dr["CustomerNo"].ToString(),
                                EmailAddress = dr["EmailAddress"].ToString(),
                                Address = dr["Address"].ToString(),
                                BusinessName = dr["BusinessName"].ToString(),
                                //CallingTime = dr["CallingTime"].ToString(),
                                //CellNo = dr["CellNo"].ToString(),
                                //CellularBackup = (dr["CellularBackup"] != DBNull.Value ? Convert.ToBoolean(dr["CellularBackup"]) : false),
                                //City = dr["City"].ToString(),
                                //ContractTeam = dr["ContractTeam"].ToString(),
                                //Country = dr["Country"].ToString(),
                                //CreditScore = dr["CreditScore"].ToString(),
                                //CustomerFunded = (dr["CustomerFunded"] != DBNull.Value ? Convert.ToBoolean(dr["CustomerFunded"]) : false),
                                CustomerId = (Guid)dr["CustomerId"],
                                //Fax = dr["Fax"].ToString(),
                                FirstName = dr["FirstName"].ToString(),
                                //FundingCompany = dr["FundingCompany"].ToString(),
                                //IsAlarmCom = dr["IsAlarmCom"] != DBNull.Value ? Convert.ToBoolean(dr["IsAlarmCom"]) : false,
                                LastName = dr["LastName"].ToString(),
                                //LeadSource = dr["LeadSource"].ToString(),
                                //Maintenance = dr["Maintenance"] != DBNull.Value ? Convert.ToBoolean(dr["Maintenance"]) : false,
                                //MonthlyMonitoringFee = dr["MonthlyMonitoringFee"].ToString(),
                                //Note = dr["Note"].ToString(),
                                PrimaryPhone = dr["PrimaryPhone"].ToString(),
                                //SecondaryPhone = dr["SecondaryPhone"].ToString(),
                                //SSN = dr["SSN"].ToString(),
                                //State = dr["State"].ToString(),
                                //Street = dr["Street"].ToString(),
                                JoinDate = dr["JoinDate"] != DBNull.Value ? Convert.ToDateTime(dr["JoinDate"]) : new DateTime(),
                                //Title = dr["Title"].ToString(),
                                //Type = dr["Type"].ToString(),
                                //ZipCode = dr["ZipCode"].ToString(),
                                //DateofBirth = dr["DateofBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DateofBirth"]) : new DateTime(),
                                //MiddleName = dr["MiddleName"].ToString(),
                                //LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                                //LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime()
                                //Status = dr["Status"].ToString(),
                                IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                                Street = dr["Street"].ToString(),
                                City = dr["City"].ToString(),
                                State = dr["State"].ToString(),
                                ZipCode = dr["ZipCode"].ToString(),
                                AnnualRevenue = dr["AnnualRevenue"] != DBNull.Value ? Convert.ToDouble(dr["AnnualRevenue"]) : 0,
                                //AccountType = dr["AccountType"].ToString(),
                                //PersonSales = dr["PersonSales"].ToString(),
                                //MarketVal = dr["MarketVal"].ToString(),
                                DBA = dr["DBA"].ToString()
                            }).ToList();
            return propertyList;
        }
        public CustomerListWithCountModel GetAllConvertedCustomerByCompany(Guid CompanyId, int pageno, int pagesize, FilterReportModel filter)
        {
            DataSet ds = _CustomerDataAccess.GetAllConvertedCustomerByCompany(CompanyId, null, null, pageno, pagesize, filter);
            CustomerListWithCountModel model = new CustomerListWithCountModel();
            model.CustomerList = (from DataRow dr in ds.Tables[0].Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                Name = dr["Name"].ToString(),
                                EmailAddress = dr["EmailAddress"].ToString(),
                                ConvertionDate = dr["ConvertionDate"] != DBNull.Value ? Convert.ToDateTime(dr["ConvertionDate"]) : new DateTime(),
                                ConvertionType = dr["ConvertionType"].ToString(),
                                CustomerId = (Guid)dr["CustomerId"],
                                CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                CreatedByVal = dr["CreatedByVal"].ToString(),
                                LeadSourceVal = dr["LeadSourceVal"].ToString()
                            }).ToList();
            model.TotalCustomerCount = (from DataRow dr in ds.Tables[1].Rows
                             select new TotalCustomerCount()
                             {
                                 Counter = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                             }).FirstOrDefault();
            return model;
        }
        public List<Customer> GetCustomerListBySubscriptionIdList(string cCSubscriptionList)
        {
            return _CustomerDataAccess.GetCustomerListBySubscriptionIdList(cCSubscriptionList);
        }

        public List<Customer> GetCustomerListByCustomerIdList(string CustomerIdList)
        {
            DataTable dt = _CustomerDataAccess.GetCustomerListByCustomerIdList(CustomerIdList);
            List<Customer> propertyList = new List<Customer>();
            propertyList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                //ContactName = dr["ContactName"].ToString(),

                                BusinessName = dr["BusinessName"].ToString(),
                                customerName = dr["Name"].ToString(),
                                CustomerId = (Guid)dr["CustomerId"],

                                FirstName = dr["FirstName"].ToString(),

                                LastName = dr["LastName"].ToString(),
                                IsLead = dr["IsLead"] != DBNull.Value ? Convert.ToBoolean(dr["IsLead"]) : false,

                            }).ToList();
            return propertyList;
        }
        public CustomerListWithCountModel GetAllLeadsByCompanyAndDates(Guid CompanyId, DateTime Start, DateTime End, int pageno, int pagesize, string status, string market, string leads, string soldBy)
        {
            DataSet dsResult = _CustomerDataAccess.GetAllLeadsByCompany(CompanyId, Start, End, pageno, pagesize, status, market, leads, soldBy);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            DataTable dt2 = dsResult.Tables[2];
            List<Customer> propertyList = new List<Customer>();
            TotalCustomerCount CustomerCount = new TotalCustomerCount();
            CustomerCount Cus = new CustomerCount();

            propertyList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                //ContactName = dr["ContactName"].ToString(),
                                //AccountNo = dr["AccountNo"].ToString(),
                                EmailAddress = dr["EmailAddress"].ToString(),
                                Address = dr["Address"].ToString(),
                                BusinessName = dr["BusinessName"].ToString(),
                                CallingTime = dr["CallingTime"].ToString(),
                                CellNo = dr["CellNo"].ToString(),
                                CellularBackup = (dr["CellularBackup"] != DBNull.Value ? Convert.ToBoolean(dr["CellularBackup"]) : false),
                                City = dr["City"].ToString(),
                                ContractTeam = dr["ContractTeam"].ToString(),
                                Country = dr["Country"].ToString(),
                                CreditScore = dr["CreditScore"].ToString(),
                                CustomerFunded = (dr["CustomerFunded"] != DBNull.Value ? Convert.ToBoolean(dr["CustomerFunded"]) : false),
                                CustomerId = (Guid)dr["CustomerId"],
                                Fax = dr["Fax"].ToString(),
                                FirstName = dr["FirstName"].ToString(),
                                FundingCompany = dr["FundingCompany"].ToString(),
                                IsAlarmCom = dr["IsAlarmCom"] != DBNull.Value ? Convert.ToBoolean(dr["IsAlarmCom"]) : false,
                                IsLead = dr["IsLead"] != DBNull.Value ? Convert.ToBoolean(dr["IsLead"]) : false,
                                LastName = dr["LastName"].ToString(),
                                LeadSource = dr["LeadSource"].ToString(),
                                Maintenance = dr["Maintenance"] != DBNull.Value ? Convert.ToBoolean(dr["Maintenance"]) : false,
                                MonthlyMonitoringFee = dr["MonthlyMonitoringFee"].ToString(),
                                Note = dr["Note"].ToString(),
                                PrimaryPhone = dr["PrimaryPhone"].ToString(),
                                SecondaryPhone = dr["SecondaryPhone"].ToString(),
                                SSN = dr["SSN"].ToString(),
                                State = dr["State"].ToString(),
                                Street = dr["Street"].ToString(),

                                LeadSourceVal = dr["LeadSourceVal"].ToString(),
                                StatusVal = dr["StatusVal"].ToString(),
                                Status = dr["Status"].ToString(),

                                JoinDate = dr["JoinDate"] != DBNull.Value ? Convert.ToDateTime(dr["JoinDate"]) : new DateTime(),
                                Title = dr["Title"].ToString(),
                                Type = dr["Type"].ToString(),
                                ZipCode = dr["ZipCode"].ToString(),
                                DateofBirth = dr["DateofBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DateofBirth"]) : new DateTime(),
                                MiddleName = dr["MiddleName"].ToString(),
                                LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                                LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                DBA = dr["DBA"].ToString(),
                                MarketVal = dr["MarketVal"].ToString(),
                                PersonSales = dr["PersonSales"].ToString(),
                                CreatedName = dr["CreatedName"].ToString()
                            }).ToList();


            CustomerCount = (from DataRow dr in dt1.Rows
                             select new TotalCustomerCount()
                             {
                                 Counter = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0

                             }).FirstOrDefault();
            Cus = (from DataRow dr in dt2.Rows
                   select new CustomerCount()
                   {
                       TotalCustomer = dr["CountCustomer"] != DBNull.Value ? Convert.ToInt32(dr["CountCustomer"]) : 0
                   }).FirstOrDefault();

            CustomerListWithCountModel LeadList = new CustomerListWithCountModel();
            LeadList.CustomerList = propertyList;
            LeadList.TotalCustomerCount = CustomerCount;
            LeadList.CustomerCount = Cus;
            return LeadList;
        }

        public List<Customer> GetAllCustomerByCompanyAndDates(Guid CompanyId, DateTime Start, DateTime End, int pageno, int pagesize, List<string> Status, string market, string soldBy)
        {
            DataTable dt = _CustomerDataAccess.GetAllCustomerByCompany(CompanyId, Start, End, pageno, pagesize, Status, market, soldBy);
            List<Customer> propertyList = new List<Customer>();
            propertyList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                //ContactName = dr["ContactName"].ToString(),
                                CustomerNo = dr["CustomerNo"].ToString(),
                                EmailAddress = dr["EmailAddress"].ToString(),
                                Address = dr["Address"].ToString(),
                                Street = dr["Street"].ToString(),
                                City = dr["City"].ToString(),
                                ZipCode = dr["ZipCode"].ToString(),
                                State = dr["State"].ToString(),
                                BusinessName = dr["BusinessName"].ToString(),
                                //CallingTime = dr["CallingTime"].ToString(),
                                //CellNo = dr["CellNo"].ToString(),
                                //CellularBackup = (dr["CellularBackup"] != DBNull.Value ? Convert.ToBoolean(dr["CellularBackup"]) : false),
                                //City = dr["City"].ToString(),
                                //ContractTeam = dr["ContractTeam"].ToString(),
                                //Country = dr["Country"].ToString(),
                                //CreditScore = dr["CreditScore"].ToString(),
                                //CustomerFunded = (dr["CustomerFunded"] != DBNull.Value ? Convert.ToBoolean(dr["CustomerFunded"]) : false),
                                CustomerId = (Guid)dr["CustomerId"],
                                //Fax = dr["Fax"].ToString(),
                                FirstName = dr["FirstName"].ToString(),
                                //FundingCompany = dr["FundingCompany"].ToString(),
                                //IsAlarmCom = dr["IsAlarmCom"] != DBNull.Value ? Convert.ToBoolean(dr["IsAlarmCom"]) : false,
                                LastName = dr["LastName"].ToString(),
                                PrimaryPhone = dr["PrimaryPhone"].ToString(),
                                //LeadSource = dr["LeadSource"].ToString(),
                                //Maintenance = dr["Maintenance"] != DBNull.Value ? Convert.ToBoolean(dr["Maintenance"]) : false,
                                //MonthlyMonitoringFee = dr["MonthlyMonitoringFee"].ToString(),
                                //Note = dr["Note"].ToString(),
                                //PrimaryPhone = dr["PrimaryPhone"].ToString(),
                                //SecondaryPhone = dr["SecondaryPhone"].ToString(),
                                //SSN = dr["SSN"].ToString(),
                                //State = dr["State"].ToString(),
                                //Street = dr["Street"].ToString(),
                                JoinDate = dr["JoinDate"] != DBNull.Value ? Convert.ToDateTime(dr["JoinDate"]) : new DateTime(),
                                //Title = dr["Title"].ToString(),
                                //Type = dr["Type"].ToString(),
                                //ZipCode = dr["ZipCode"].ToString(),
                                //DateofBirth = dr["DateofBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DateofBirth"]) : new DateTime(),
                                //MiddleName = dr["MiddleName"].ToString(),
                                //LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                                //LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                Status = dr["Status"].ToString(),
                                IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                                
                                AnnualRevenue = dr["AnnualRevenue"] != DBNull.Value ? Convert.ToDouble(dr["AnnualRevenue"]) : 0,
                                AccountType = dr["AccountType"].ToString(),
                                PersonSales = dr["PersonSales"].ToString(),
                                MarketVal = dr["MarketVal"].ToString(),
                                DBA = dr["DBA"].ToString(),
                                CancellationDate = dr["CancellationDate"] != DBNull.Value ? Convert.ToDateTime(dr["CancellationDate"]) : new DateTime()
                            }).ToList();
            return propertyList;
        }
        public List<Customer> GetAllInactiveCustomerByCompanyAndDates(Guid CompanyId, DateTime? Start, DateTime? End, int pageno, int pagesize)
        {
            DataTable dt = _CustomerDataAccess.GetAllInactiveCustomerByCompany(CompanyId, Start, End, pageno, pagesize);
            List<Customer> propertyList = new List<Customer>();
            propertyList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                EmailAddress = dr["EmailAddress"].ToString(),
                                Address = dr["Address"].ToString(),
                                Name = dr["Name"].ToString(),
                                CustomerId = (Guid)dr["CustomerId"],
                                PrimaryPhone = dr["PrimaryPhone"].ToString()
                            }).ToList();
            return propertyList;
        }
        #endregion
        public CustomerListWithCountModel GetAllConvertedCustomerByCompanyAndDates(Guid CompanyId, DateTime Start, DateTime End, int pageno, int pagesize, FilterReportModel filter)
        {
            DataSet ds = _CustomerDataAccess.GetAllConvertedCustomerByCompany(CompanyId, Start, End, pageno, pagesize, filter);
            CustomerListWithCountModel model = new CustomerListWithCountModel();
            model.CustomerList = (from DataRow dr in ds.Tables[0].Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                Name = dr["Name"].ToString(),
                                EmailAddress = dr["EmailAddress"].ToString(),
                                ConvertionDate = dr["ConvertionDate"] != DBNull.Value ? Convert.ToDateTime(dr["ConvertionDate"]) : new DateTime(),
                                ConvertionType = dr["ConvertionType"].ToString(),
                                CustomerId = (Guid)dr["CustomerId"],
                                CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                CreatedByVal = dr["CreatedByVal"].ToString(),
                                LeadSourceVal = dr["LeadSourceVal"].ToString()
                            }).ToList();
            model.TotalCustomerCount = (from DataRow dr in ds.Tables[1].Rows
                             select new TotalCustomerCount()
                             {
                                 Counter = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                             }).FirstOrDefault();
            return model;
        }

        public DataTable GetCustomerReport(int[] IdList, string[] columnList, Guid CompanyId, string NumberPrefix, string acorin)
        {
            return _CustomerDataAccess.GetCustomerReport(IdList, columnList, CompanyId, NumberPrefix, acorin);
        }
        public DataTable GetCustomerDatabaseReport(int[] IdList, Guid CompanyId)
        {
            return _CustomerDataAccess.GetCustomerDatabaseReport(IdList, CompanyId);
        }
        public List<Customer> GetCustomerPdf(int[] IdList, Guid CompanyId, string NumberPrefix, string acorin)
        {
            DataTable dt = _CustomerDataAccess.GetCustomerPdf(IdList, CompanyId, NumberPrefix, acorin);
            List<Customer> CustomerList = new List<Customer>();

            CustomerList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                //ContactName = dr["ContactName"].ToString(),
                                AccountNo = dr["AccountNo"].ToString(),
                                EmailAddress = dr["EmailAddress"].ToString(),
                                Address = dr["Address"].ToString(),
                                Address2 = dr["Address2"].ToString(),
                                BusinessName = dr["BusinessName"].ToString(),
                                //CallingTime = dr["NewCallingTime"].ToString(),
                                CellNo = dr["CellNo"].ToString(),
                                CellularBackup = (dr["CellularBackup"] != DBNull.Value ? Convert.ToBoolean(dr["CellularBackup"]) : false),
                                City = dr["City"].ToString(),
                                ContractTeam = dr["ContractTeam"].ToString(),
                                Country = dr["Country"].ToString(),
                                CreditScore = dr["CreditScore"].ToString(),
                                CustomerFunded = (dr["CustomerFunded"] != DBNull.Value ? Convert.ToBoolean(dr["CustomerFunded"]) : false),
                                CustomerId = (Guid)dr["CustomerId"],
                                Fax = dr["Fax"].ToString(),
                                FirstName = dr["FirstName"].ToString(),
                                FundingCompany = dr["FundingCompany"].ToString(),
                                IsAlarmCom = dr["IsAlarmCom"] != DBNull.Value ? Convert.ToBoolean(dr["IsAlarmCom"]) : false,
                                LastName = dr["LastName"].ToString(),
                                LeadSource = dr["LeadSource"].ToString(),
                                Maintenance = dr["Maintenance"] != DBNull.Value ? Convert.ToBoolean(dr["Maintenance"]) : false,
                                MonthlyMonitoringFee = dr["MonthlyMonitoringFee"].ToString(),
                                Note = dr["Note"].ToString(),
                                PrimaryPhone = dr["PrimaryPhone"].ToString(),
                                SecondaryPhone = dr["SecondaryPhone"].ToString(),
                                SSN = dr["SSN"].ToString(),
                                StreetPrevious = dr["StreetPrevious"].ToString(),
                                CityPrevious = dr["CityPrevious"].ToString(),
                                StatePrevious = dr["StatePrevious"].ToString(),
                                ZipCodePrevious = dr["ZipCodePrevious"].ToString(),
                                State = dr["State"].ToString(),
                                Street = dr["Street"].ToString(),
                                Title = dr["Title"].ToString(),
                                Type = dr["Type"].ToString(),
                                ZipCode = dr["ZipCode"].ToString(),
                                Singature = dr["Singature"].ToString(),
                                DateofBirth = dr["DateofBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DateofBirth"]) : new DateTime(),
                            }).ToList();
            return CustomerList;
        }
        public DataTable GetEquipmentReport(int[] IdList, string[] columnList, Guid CompanyId)
        {
            return _CustomerDataAccess.GetEquipmentReport(IdList, columnList, CompanyId);
        }
        public List<Customer> GetSubscribedAllCustomer(bool IsActiveCheck)
        {

            string Query = " AuthorizeRefId is not null and AuthorizeRefId !='' and Id >= 522479 ";
            if (IsActiveCheck)
            {
                Query += " and IsActive = 1";
            }
            return _CustomerDataAccess.GetByQuery(Query);
        }
        public List<Customer> GetAllCustomersByCompanyId(Guid CompanyId)
        {
            DataTable dt = _CustomerDataAccess.GetAllCustomersByCompany(CompanyId);
            List<Customer> CustomerList = new List<Customer>();

            CustomerList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                //ContactName = dr["ContactName"].ToString(),
                                AccountNo = dr["AccountNo"].ToString(),
                                EmailAddress = dr["EmailAddress"].ToString(),
                                Address = dr["Address"].ToString(),
                                Address2 = dr["Address2"].ToString(),
                                BusinessName = dr["BusinessName"].ToString(),
                                //CallingTime = dr["NewCallingTime"].ToString(),
                                CellNo = dr["CellNo"].ToString(),
                                CellularBackup = (dr["CellularBackup"] != DBNull.Value ? Convert.ToBoolean(dr["CellularBackup"]) : false),
                                City = dr["City"].ToString(),
                                ContractTeam = dr["ContractTeam"].ToString(),
                                Country = dr["Country"].ToString(),
                                CreditScore = dr["CreditScore"].ToString(),
                                CustomerFunded = (dr["CustomerFunded"] != DBNull.Value ? Convert.ToBoolean(dr["CustomerFunded"]) : false),
                                CustomerId = (Guid)dr["CustomerId"],
                                Fax = dr["Fax"].ToString(),
                                FirstName = dr["FirstName"].ToString(),
                                FundingCompany = dr["FundingCompany"].ToString(),
                                IsAlarmCom = dr["IsAlarmCom"] != DBNull.Value ? Convert.ToBoolean(dr["IsAlarmCom"]) : false,
                                LastName = dr["LastName"].ToString(),
                                LeadSource = dr["LeadSource"].ToString(),
                                Maintenance = dr["Maintenance"] != DBNull.Value ? Convert.ToBoolean(dr["Maintenance"]) : false,
                                MonthlyMonitoringFee = dr["MonthlyMonitoringFee"].ToString(),
                                Note = dr["Note"].ToString(),
                                PrimaryPhone = dr["PrimaryPhone"].ToString(),
                                SecondaryPhone = dr["SecondaryPhone"].ToString(),
                                SSN = dr["SSN"].ToString(),
                                StreetPrevious = dr["StreetPrevious"].ToString(),
                                CityPrevious = dr["CityPrevious"].ToString(),
                                StatePrevious = dr["StatePrevious"].ToString(),
                                ZipCodePrevious = dr["ZipCodePrevious"].ToString(),
                                State = dr["State"].ToString(),
                                Street = dr["Street"].ToString(),
                                Title = dr["Title"].ToString(),
                                Type = dr["Type"].ToString(),
                                ZipCode = dr["ZipCode"].ToString(),
                                Singature = dr["Singature"].ToString(),
                                DateofBirth = dr["DateofBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DateofBirth"]) : new DateTime(),
                            }).ToList();
            return CustomerList;
        }

        public List<Customer> GetAllCustomerByCompanyId(Guid CompanyId, DateTime? start, DateTime? end, int pageno, int pagesize, List<string> status, string market)
        {
            DataTable dt = _CustomerDataAccess.GetAllCustomerByCompany(CompanyId, start, end, pageno, pagesize, status, market, null);
            List<Customer> CustomerList = new List<Customer>();

            CustomerList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                //ContactName = dr["ContactName"].ToString(),
                                AccountNo = dr["AccountNo"].ToString(),
                                EmailAddress = dr["EmailAddress"].ToString(),
                                Address = dr["Address"].ToString(),
                                Address2 = dr["Address2"].ToString(),
                                BusinessName = dr["BusinessName"].ToString(),
                                //CallingTime = dr["NewCallingTime"].ToString(),
                                CellNo = dr["CellNo"].ToString(),
                                CellularBackup = (dr["CellularBackup"] != DBNull.Value ? Convert.ToBoolean(dr["CellularBackup"]) : false),
                                City = dr["City"].ToString(),
                                ContractTeam = dr["ContractTeam"].ToString(),
                                Country = dr["Country"].ToString(),
                                CreditScore = dr["CreditScore"].ToString(),
                                CustomerFunded = (dr["CustomerFunded"] != DBNull.Value ? Convert.ToBoolean(dr["CustomerFunded"]) : false),
                                CustomerId = (Guid)dr["CustomerId"],
                                Fax = dr["Fax"].ToString(),
                                FirstName = dr["FirstName"].ToString(),
                                FundingCompany = dr["FundingCompany"].ToString(),
                                IsAlarmCom = dr["IsAlarmCom"] != DBNull.Value ? Convert.ToBoolean(dr["IsAlarmCom"]) : false,
                                LastName = dr["LastName"].ToString(),
                                LeadSource = dr["LeadSource"].ToString(),
                                Maintenance = dr["Maintenance"] != DBNull.Value ? Convert.ToBoolean(dr["Maintenance"]) : false,
                                MonthlyMonitoringFee = dr["MonthlyMonitoringFee"].ToString(),
                                Note = dr["Note"].ToString(),
                                PrimaryPhone = dr["PrimaryPhone"].ToString(),
                                SecondaryPhone = dr["SecondaryPhone"].ToString(),
                                SSN = dr["SSN"].ToString(),
                                StreetPrevious = dr["StreetPrevious"].ToString(),
                                CityPrevious = dr["CityPrevious"].ToString(),
                                StatePrevious = dr["StatePrevious"].ToString(),
                                ZipCodePrevious = dr["ZipCodePrevious"].ToString(),
                                State = dr["State"].ToString(),
                                Street = dr["Street"].ToString(),
                                Title = dr["Title"].ToString(),
                                Type = dr["Type"].ToString(),
                                ZipCode = dr["ZipCode"].ToString(),
                                Singature = dr["Singature"].ToString(),
                                DateofBirth = dr["DateofBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DateofBirth"]) : new DateTime(),
                            }).ToList();
            return CustomerList;
        }

        public List<GlobalSearchModel> GetGlobalSearchByKeyAndCompanyId(string key, Guid CompanyId, string emptag, string emprole, Guid empid, string Currency, bool IsContactPermitted, bool IsOpportunityPermitted)
        {
            DataTable dt = _CustomerDataAccess.GetGlobalSearchByKeyAndCompanyId(key, CompanyId, emptag, emprole, empid, Currency, IsContactPermitted, IsOpportunityPermitted);
            List<GlobalSearchModel> SearchList = new List<GlobalSearchModel>();
          SearchList = (from DataRow dr in dt.Rows
                          select new GlobalSearchModel()
                          {
                              EmailAddress = dr["EmailAddress"].ToString(),
                              Name = dr["Name"].ToString(),
                              BusinessName = dr["BusinessName"].ToString(),
                              PhoneNumber = dr["PhoneNumber"].ToString(),
                              Type = dr["Type"].ToString(),
                          }).ToList();
            return SearchList;
        }
        public List<CustomerExistingItem> GetAllExistingItemByCustomerId(Guid CustomerId)
        {
            DataTable dt = _CustomerDataAccess.GetAllExistingItemByCustomerId(CustomerId);
            List<CustomerExistingItem> ExistingItemList = new List<CustomerExistingItem>();
            ExistingItemList = (from DataRow dr in dt.Rows
                                select new CustomerExistingItem()
                                {
                                    ItemName = dr["ItemName"].ToString(),
                                    Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    CustomerId = (Guid)dr["CustomerId"],
                                }).ToList();
            return ExistingItemList;
        }

        public List<Customer> GetLeadsByKeyAndCompanyId(Guid CompanyId, string key, string emptag, string emprole, Guid empid,bool ispermit)
        {
            DataTable dt = _CustomerDataAccess.GetLeadssByKeyAndCompanyId(CompanyId, key, emptag, emprole, empid, ispermit);
            List<Customer> CustomerList = new List<Customer>();
            CustomerList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                //ContactName = dr["ContactName"].ToString(),
                                AccountNo = dr["AccountNo"].ToString(),
                                EmailAddress = dr["EmailAddress"].ToString(),
                                Address = dr["Address"].ToString(),
                                BusinessName = dr["BusinessName"].ToString(),
                                CallingTime = dr["CallingTime"].ToString(),
                                CellNo = dr["CellNo"].ToString(),
                                CellularBackup = (dr["CellularBackup"] != DBNull.Value ? Convert.ToBoolean(dr["CellularBackup"]) : false),
                                City = dr["City"].ToString(),
                                ContractTeam = dr["ContractTeam"].ToString(),
                                Country = dr["Country"].ToString(),
                                CreditScore = dr["CreditScore"].ToString(),
                                CustomerFunded = (dr["CustomerFunded"] != DBNull.Value ? Convert.ToBoolean(dr["CustomerFunded"]) : false),
                                CustomerId = (Guid)dr["CustomerId"],
                                Fax = dr["Fax"].ToString(),
                                FirstName = dr["FirstName"].ToString(),
                                FundingCompany = dr["FundingCompany"].ToString(),
                                IsAlarmCom = dr["IsAlarmCom"] != DBNull.Value ? Convert.ToBoolean(dr["IsAlarmCom"]) : false,
                                LastName = dr["LastName"].ToString(),
                                LeadSource = dr["LeadSource"].ToString(),
                                Maintenance = dr["Maintenance"] != DBNull.Value ? Convert.ToBoolean(dr["Maintenance"]) : false,
                                MonthlyMonitoringFee = dr["MonthlyMonitoringFee"].ToString(),
                                Note = dr["Note"].ToString(),
                                PrimaryPhone = dr["PrimaryPhone"].ToString(),
                                SecondaryPhone = dr["SecondaryPhone"].ToString(),
                                SSN = dr["SSN"].ToString(),
                                State = dr["State"].ToString(),
                                Street = dr["Street"].ToString(),
                                Title = dr["Title"].ToString(),
                                Type = dr["Type"].ToString(),
                                ZipCode = dr["ZipCode"].ToString(),
                                Singature = dr["Singature"].ToString(),
                                DateofBirth = dr["DateofBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DateofBirth"]) : new DateTime(),
                                IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                                StreetType = dr["StreetType"].ToString(),
                                Appartment = dr["Appartment"].ToString(),
                                EMPNUM = dr["EMPNUM"].ToString(),
                                LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime()
                            }).ToList();
            return CustomerList;
        }

        public CustomerTabCounts GetCustomerTabCountsByCustomerId(Guid customerId, string techid, Guid companyid, Guid user)
        {
            DataTable dt = _CustomerDataAccess.GetCustomerTabCountsByCustomerId(customerId, techid, companyid, user);
            List<CustomerTabCounts> CustomerTabCounts = new List<CustomerTabCounts>();
            CustomerTabCounts = (from DataRow dr in dt.Rows
                                 select new CustomerTabCounts()
                                 {
                                     CorrespondenceCount = dr["CorrespondenceCount"] != DBNull.Value ? Convert.ToInt32(dr["CorrespondenceCount"]) : 0,
                                     EstimateCount = dr["EstimateCount"] != DBNull.Value ? Convert.ToInt32(dr["EstimateCount"]) : 0,
                                     FilesCount = dr["FilesCount"] != DBNull.Value ? Convert.ToInt32(dr["FilesCount"]) : 0,
                                     FundingCount = dr["FundingCount"] != DBNull.Value ? Convert.ToInt32(dr["FundingCount"]) : 0,
                                     InvoiceCount = dr["InvoiceCount"] != DBNull.Value ? Convert.ToInt32(dr["InvoiceCount"]) : 0,
                                     NotesCount = dr["NotesCount"] != DBNull.Value ? Convert.ToInt32(dr["NotesCount"]) : 0,
                                     ScheduleCount = dr["ScheduleCount"] != DBNull.Value ? Convert.ToInt32(dr["ScheduleCount"]) : 0,
                                     ServiceOrderCount = dr["ServiceOrderCount"] != DBNull.Value ? Convert.ToInt32(dr["ServiceOrderCount"]) : 0,
                                     WorkOrderCount = dr["WorkOrderCount"] != DBNull.Value ? Convert.ToInt32(dr["WorkOrderCount"]) : 0,
                                     TicketsCount = dr["TicketsCount"] != DBNull.Value ? Convert.ToInt32(dr["TicketsCount"]) : 0,
                                     CustomerCredit = string.Format("{0:0,0.00}", (dr["CustomerCredit"] != DBNull.Value ? Convert.ToDouble(dr["CustomerCredit"]) : 0)),
                                     ActivityCustomer = dr["ActivityCustomer"] != DBNull.Value ? Convert.ToInt32(dr["ActivityCustomer"]) : 0,
                                     OpportunityCustomer = dr["OpportunityCustomer"] != DBNull.Value ? Convert.ToInt32(dr["OpportunityCustomer"]) : 0,
                                     ContactCustomer = dr["ContactCustomer"] != DBNull.Value ? Convert.ToInt32(dr["ContactCustomer"]) : 0,
                                     BookingCount = dr["BookingCount"] != DBNull.Value ? Convert.ToInt32(dr["BookingCount"]) : 0,
                                     LogCount = dr["LogCount"] != DBNull.Value ? Convert.ToInt32(dr["LogCount"]) : 0,
                                 }).ToList();
            return CustomerTabCounts.FirstOrDefault();
        }

        public List<Customer> GetCustomersByKeyAndCompanyId(Guid CompanyId, string key, string emptag, string emprole, Guid empid,bool isPermit)
        {
            DataTable dt = _CustomerDataAccess.GetCustomersByKeyAndCompanyId(CompanyId, key, emptag, emprole, empid, isPermit);
            List<Customer> CustomerList = new List<Customer>();
            CustomerList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                //ContactName = dr["ContactName"].ToString(),
                                AccountNo = dr["AccountNo"].ToString(),
                                EmailAddress = dr["EmailAddress"].ToString(),
                                Address = dr["Address"].ToString(),
                                BusinessName = dr["BusinessName"].ToString(),
                                CallingTime = dr["CallingTime"].ToString(),
                                CellNo = dr["CellNo"].ToString(),
                                CellularBackup = (dr["CellularBackup"] != DBNull.Value ? Convert.ToBoolean(dr["CellularBackup"]) : false),
                                City = dr["City"].ToString(),
                                ContractTeam = dr["ContractTeam"].ToString(),
                                Country = dr["Country"].ToString(),
                                CreditScore = dr["CreditScore"].ToString(),
                                CustomerFunded = (dr["CustomerFunded"] != DBNull.Value ? Convert.ToBoolean(dr["CustomerFunded"]) : false),
                                CustomerId = (Guid)dr["CustomerId"],
                                Fax = dr["Fax"].ToString(),
                                FirstName = dr["FirstName"].ToString(),
                                FundingCompany = dr["FundingCompany"].ToString(),
                                IsAlarmCom = dr["IsAlarmCom"] != DBNull.Value ? Convert.ToBoolean(dr["IsAlarmCom"]) : false,
                                LastName = dr["LastName"].ToString(),
                                LeadSource = dr["LeadSource"].ToString(),
                                Maintenance = dr["Maintenance"] != DBNull.Value ? Convert.ToBoolean(dr["Maintenance"]) : false,
                                MonthlyMonitoringFee = dr["MonthlyMonitoringFee"].ToString(),
                                Note = dr["Note"].ToString(),
                                PrimaryPhone = dr["PrimaryPhone"].ToString(),
                                SecondaryPhone = dr["SecondaryPhone"].ToString(),
                                SSN = dr["SSN"].ToString(),
                                State = dr["State"].ToString(),
                                Street = dr["Street"].ToString(),
                                Title = dr["Title"].ToString(),
                                Type = dr["Type"].ToString(),
                                ZipCode = dr["ZipCode"].ToString(),
                                DateofBirth = dr["DateofBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DateofBirth"]) : new DateTime(),
                                IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                                InstallDate = dr["InstallDate"] != DBNull.Value ? Convert.ToDateTime(dr["InstallDate"]) : new DateTime(),
                                FirstBilling = dr["FirstBilling"] != DBNull.Value ? Convert.ToDateTime(dr["FirstBilling"]) : new DateTime(),
                                CustomerNo = dr["CustomerNo"].ToString(),
                                Soldby = dr["Soldby"].ToString(),
                                PaymentMethod = dr["PaymentMethod"].ToString(),
                                TechnicianName = dr["TechnicianName"].ToString(),
                                PersonSales = dr["PersonSales"].ToString(),
                                IsAgreement = dr["IsAgreement"] != DBNull.Value ? Convert.ToBoolean(dr["IsAgreement"]) : false,
                                UnpaidInvoiceTotal = dr["UnpaidInvoiceTotal"] != DBNull.Value ? Convert.ToDouble(dr["UnpaidInvoiceTotal"]) : 0.0,
                                JoinDate = dr["JoinDate"] != DBNull.Value ? Convert.ToDateTime(dr["JoinDate"]) : new DateTime(),
                            }).ToList();
            return CustomerList;
        }

        public List<CustomerMigration> GetAllCustomerMigration()
        {
            return _CustomerMigrationDataAccess.GetAll();
        }
        public List<CustomerMigration> GetAllCustomerMigration(string PlatformName)
        {
            return _CustomerMigrationDataAccess.GetByQuery(string.Format(" [Platform] = '{0}'",PlatformName));
        }

        public CustomerMigration GetCustomerMigrationByReferenceId(string value)
        {
            return _CustomerMigrationDataAccess.GetByQuery(string.Format("RefenrenceId='{0}'",value)).FirstOrDefault();
        }

        public List<CustomerSearchModel> GetCustomerListBySearchKeyAndCompanyId(string key, Guid CompanyId, int MaxLoad)
        {
            DataTable dt = _InvoiceDataAccess.GetCustomerListBySearchKeyAndCompanyId(key, CompanyId, MaxLoad);
            List<CustomerSearchModel> CustomerSearchList = new List<CustomerSearchModel>();
            CustomerSearchList = dt.AsEnumerable().Select(dataRow => new CustomerSearchModel
            {
                CustomerId = dataRow.Field<Guid>("CustomerId"),
                Address = dataRow.Field<string>("Address"),
                Address1 = dataRow.Field<string>("Address2"),
                Street = dataRow.Field<string>("Street"),
                Street1 = dataRow.Field<string>("Street1"),
                City = dataRow.Field<string>("City"),
                City1 = dataRow.Field<string>("City1"),
                State = dataRow.Field<string>("State"),
                State1 = dataRow.Field<string>("State1"),
                ZipCode = dataRow.Field<string>("ZipCode"),
                ZipCode1 = dataRow.Field<string>("ZipCode1"),
                FirstName = dataRow.Field<string>("FirstName"),
                LastName = dataRow.Field<string>("LastName"),
                EmailAddress = dataRow.Field<string>("EmailAddress"),
                BusinessName = dataRow.Field<string>("BusinessName"),
                Type = dataRow.Field<string>("Type")
            }).ToList();
            return CustomerSearchList;
        }

        public Customer GetCustomerByCustomerNo(string customerNo)
        {
            return _CustomerDataAccess.GetByQuery(string.Format(" CustomerNo ='{0}'", customerNo)).FirstOrDefault();
        }

        public List<CustomerSearchModel> GetLeadListBySearchKeyAndCompanyId(string key, Guid CompanyId, int MaxLoad)
        {
            DataTable dt = _InvoiceDataAccess.GetLeadListBySearchKeyAndCompanyId(key, CompanyId, MaxLoad);
            List<CustomerSearchModel> CustomerSearchList = new List<CustomerSearchModel>();
            CustomerSearchList = dt.AsEnumerable().Select(dataRow => new CustomerSearchModel
            {
                CustomerId = dataRow.Field<Guid>("CustomerId"),
                Address = dataRow.Field<string>("Address"),
                Address1 = dataRow.Field<string>("Address2"),
                Street = dataRow.Field<string>("Street"),
                Street1 = dataRow.Field<string>("Street1"),
                City = dataRow.Field<string>("City"),
                City1 = dataRow.Field<string>("City1"),
                State = dataRow.Field<string>("State"),
                State1 = dataRow.Field<string>("State1"),
                ZipCode = dataRow.Field<string>("ZipCode"),
                ZipCode1 = dataRow.Field<string>("ZipCode1"),
                FirstName = dataRow.Field<string>("FirstName"),
                LastName = dataRow.Field<string>("LastName"),
                EmailAddress = dataRow.Field<string>("EmailAddress"),
                BusinessName = dataRow.Field<string>("BusinessName"),
                Type = dataRow.Field<string>("Type")
            }).ToList();
            return CustomerSearchList;
        }

        public List<EstimateStatus> GetAllEstimateStatusByCompanyId(Guid CompanyId)
        {
            DataTable dt = _InvoiceDataAccess.GetAllEstimateStatus(CompanyId);
            List<EstimateStatus> EstimateStatusList = new List<EstimateStatus>();
            EstimateStatusList = (from DataRow dr in dt.Rows
                                  select new EstimateStatus()
                                  {
                                      EstimateAmount = dr["EstimateAmount"] != DBNull.Value ? Convert.ToDouble(dr["EstimateAmount"]) : 0,
                                      DueAmount = dr["DueAmount"] != DBNull.Value ? Convert.ToDouble(dr["DueAmount"]) : 0,
                                      PaidAmount = dr["PaidAmount"] != DBNull.Value ? Convert.ToDouble(dr["PaidAmount"]) : 0,
                                  }).ToList();
            return EstimateStatusList;
        }

        public EstimateStatusDetail GetAllEstimateStatusDetailByCustomerId(Guid Cusidval)
        {
            DataTable dt = _InvoiceDataAccess.GetAllEstimateStatusDetailByCustomerId(Cusidval);
            List<EstimateStatusDetail> EstimateStatusDetailList = new List<EstimateStatusDetail>();
            EstimateStatusDetailList = (from DataRow dr in dt.Rows
                                        select new EstimateStatusDetail()
                                        {
                                            EstimateAmountDetail = dr["EstimateAmountDetail"] != DBNull.Value ? Convert.ToDouble(dr["EstimateAmountDetail"]) : 0,
                                            DueAmountDetail = dr["DueAmountDetail"] != DBNull.Value ? Convert.ToDouble(dr["DueAmountDetail"]) : 0,
                                            PaidAmountDetail = dr["PaidAmountDetail"] != DBNull.Value ? Convert.ToDouble(dr["PaidAmountDetail"]) : 0,
                                            UnpaidAmount = dr["UnpaidAmount"] != DBNull.Value ? Convert.ToDouble(dr["UnpaidAmount"]) : 0,
                                            CustomerCredit = dr["CustomerCredit"] != DBNull.Value ? Convert.ToDouble(dr["CustomerCredit"]) : 0,
                                        }).ToList();
            return EstimateStatusDetailList.FirstOrDefault();
        }
        public IRestResponse GetCreditReportResponse(CustomerCreditScore CreditScore, Guid CompanyId)
        {
            var CreditScoreInProduction = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = 'CreditScoreInProduction'", CompanyId)).FirstOrDefault();
            string url = "";
            if (CreditScoreInProduction != null && CreditScoreInProduction.Value.ToLower() == "true")
            {
                url = "https://api.creditsystem.com/";
            }
            else
            {
                url = "https://api.testdatasolutions.com/";
            }

            string Name = CreditScore.FirstName + " " + CreditScore.LastName;
            CreditScore.ACCOUNT = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = 'CreditScoreAccount'", CompanyId)).FirstOrDefault().Value;
            CreditScore.PASSWD = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = 'CreditScorePassword'", CompanyId)).FirstOrDefault().Value;
            CreditScore.PASS = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = 'CreditScorePass'", CompanyId)).FirstOrDefault().Value;
            CreditScore.PROCESS = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = 'CreditScoreProcess'", CompanyId)).FirstOrDefault().Value;
            CreditScore.PRODUCT = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = 'CreditScoreProduct'", CompanyId)).FirstOrDefault().Value;
            //CreditScore.BUREAU = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = 'CreditScoreBureau'", CompanyId)).FirstOrDefault().Value;
            if(CreditScore.IsSoftCheck == true)
            {
                if(CreditScore.BUREAU == "EFX")
                {
                    CreditScore.SelectCode = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = 'EFXSelectCodeSoft'", CompanyId)).FirstOrDefault().Value;
                }
                else
                {
                    CreditScore.SelectCode = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = 'TUSelectCodeSoft'", CompanyId)).FirstOrDefault().Value;
                }
            }
            else
            {
                if (CreditScore.BUREAU == "EFX")
                {
                    CreditScore.SelectCode = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = 'EFXSelectCodeHard'", CompanyId)).FirstOrDefault().Value;
                }
                else
                {
                    CreditScore.SelectCode = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = 'TUSelectCodeHard'", CompanyId)).FirstOrDefault().Value;
                }
               
            }
            //string Content = string.Format("------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"ACCOUNT\"\r\n\r\n{0}\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"PASSWD\"\r\n\r\n{1}\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"PASS\"\r\n\r\n{2}\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"PROCESS\"\r\n\r\n{3}\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"BUREAU\"\r\n\r\n{4}\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"PRODUCT\"\r\n\r\n{5}\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"NAME\"\r\n\r\n{6}\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"SSN\"\r\n\r\n{7}\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"ADDRESS\"\r\n\r\n{8}\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"CITY\"\r\n\r\n{9}\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"STATE\"\r\n\r\n{10}\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"ZIP\"\r\n\r\n{11}\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"SELECTEDCODE\"\r\n\r\n{12}\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--", CreditScore.ACCOUNT, CreditScore.PASSWD, CreditScore.PASS, CreditScore.PROCESS, CreditScore.BUREAU, CreditScore.PRODUCT, Name, CreditScore.SSN, CreditScore.ADDRESS, CreditScore.CITY, CreditScore.STATE, CreditScore.ZIP);
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
            const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
            ServicePointManager.SecurityProtocol = Tls12;
            string BodyContent = "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"ACCOUNT\"\r\n\r\n" + CreditScore.ACCOUNT + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"PASSWD\"\r\n\r\n" + CreditScore.PASSWD + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"PASS\"\r\n\r\n" + CreditScore.PASS + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"PROCESS\"\r\n\r\n" + CreditScore.PROCESS + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"BUREAU\"\r\n\r\n" + CreditScore.BUREAU + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"PRODUCT\"\r\n\r\n" + CreditScore.PRODUCT + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"NAME\"\r\n\r\n" + Name + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"SSN\"\r\n\r\n" + CreditScore.SSN + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"ADDRESS\"\r\n\r\n" + CreditScore.ADDRESS + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"CITY\"\r\n\r\n" + CreditScore.CITY + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"STATE\"\r\n\r\n" + CreditScore.STATE + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"ZIP\"\r\n\r\n" + CreditScore.ZIP + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"SELECTEDCODE\"\r\n\r\n"+CreditScore.SelectCode+"\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--";
            request.AddHeader("postman-token", "ee0be9f9-72be-ca18-ee60-78c989280310");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("accept", "application/json");
            request.AddHeader("content-type", "multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW");
            // request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW," + Content, ParameterType.RequestBody);

            request.AddParameter("multipart/form-data; boundary=----WebKitFormBoundary7MA4YWxkTrZu0gW", BodyContent, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            return response;
        }


        public Customer GetById(int id)
        {
            return _CustomerDataAccess.Get(id);
        }

        public Customer GetCustomerByCustomerId(Guid CustomerId)
        {
            return _CustomerDataAccess.GetByQuery(string.Format("CustomerId ='{0}'", CustomerId)).FirstOrDefault();
        }

        public Customer GetCustomerByMovedCustomerIdAndType(Guid CustomerId)
        {
            DataTable dt = _CustomerDataAccess.GetCustomerByMovedCustomerIdAndType(CustomerId);
            Customer CustomerList = new Customer();
            CustomerList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                BusinessName = dr["BusinessName"].ToString(),
                                DBA = dr["DBA"].ToString(),
                                IsLead = dr["IsLead"] != DBNull.Value ? Convert.ToBoolean(dr["IsLead"]) : false,
                            }).ToList().FirstOrDefault();
            return CustomerList;
        }

        public ThirdPartyCustomer GetThirdPartyCustomerByCustomerId(Guid CustomerId)
        {
            return _ThirdPartyCustomerDataAccess.GetByQuery(string.Format("CustomerId ='{0}'", CustomerId)).FirstOrDefault();
        }
        public Customer GetCustomerByCustomerGuidId(Guid CustomerId)
        {
            return _CustomerDataAccess.GetCustomerByGuidId(CustomerId) ;
        }
        public string IsCustomerInCustomerContactByCustomerId(Guid CustomerId)
        {
            CustomerContactTrack cus = _CustomerContactTrackDataAccess.GetByQuery(string.Format("CustomerId ='{0}'", CustomerId)).FirstOrDefault();
            if (cus != null)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }
        public Customer GetDirectCustomerByCustomerId(Guid CustomerId)
        {
            return _CustomerDataAccess.GetByQuery(string.Format("CustomerId ='{0}' and IsDirect = 1", CustomerId)).FirstOrDefault(); ;
        }

        public Customer GetCustomerByLeadId(int id)
        {
            return _CustomerDataAccess.GetByQuery(string.Format("Id ='{0}'", id)).FirstOrDefault();
        }
        public Company GetCompanyByCompanyId(Guid id)
        {
            return _CompanyDataAccess.GetByQuery(string.Format("CompanyId ='{0}'", id)).FirstOrDefault(); ;
        }
        public bool CustomerIsInCompany(int id, Guid CompanyId)
        {
            DataTable dt = _CustomerDataAccess.GetAllCustomerByIdAndCompanyId(CompanyId, id);
            Customer CustomerList = new Customer();
            CustomerList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0
                            }).ToList().FirstOrDefault();
            return CustomerList != null;

        }
        public bool CustomerIsInCompany(Guid CustomerId, Guid CompanyId)
        {
            DataTable dt = _CustomerDataAccess.GetAllCustomerByIdAndCompanyId(CompanyId, CustomerId);
            Customer CustomerList = new Customer();
            CustomerList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0
                            }).ToList().FirstOrDefault();
            return CustomerList != null;

        }


        public bool LeadIsInCompany(int id, Guid CompanyId)
        {
            DataTable dt = _CustomerDataAccess.GetAllLeadByIdAndCompanyId(CompanyId, id);
            Customer CustomerList = new Customer();
            CustomerList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0
                            }).ToList().FirstOrDefault();
            return CustomerList != null;

        }

        public bool CustomerIsInCompanySalesPartial(Guid id, Guid CompanyId)
        {
            DataTable dt = _CustomerDataAccess.GetAllCustomerByIdAndCompanyIdSalesPartial(CompanyId, id);
            Customer CustomerList = new Customer();
            CustomerList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0
                            }).ToList().FirstOrDefault();
            return CustomerList != null;

        }
        public Company GetCompanyByCustomerId(Guid Customerid)
        {
            DataTable dt = _CustomerDataAccess.GetCompanyByCustomerId(Customerid);
            Company CopanyList = new Company();
            CopanyList = (from DataRow dr in dt.Rows
                          select new Company()
                          {

                              CompanyName = dr["CompanyName"].ToString()
                          }).ToList().FirstOrDefault();
            return CopanyList;

        }

        public bool UpdateCustomerCompany(CustomerCompany cc)
        {
            return _CustomerCompanyDataAccess.Update(cc) > 0;
        }
        public long InsertCustomer(Customer customer)
        {
            return _CustomerDataAccess.Insert(customer);
        }
        public long InsertCustomerExtended(CustomerExtended customer)
        {
            return _CustomerExtendedDataAccess.Insert(customer);
        }
        

        public long InsertAgemniCustomer(Customer customer)
        {
            return _CustomerDataAccess.InsertAgemniCustomer(customer);
        }

        public long InsertThirdPartyCustomer(ThirdPartyCustomer customer)
        {
            return _ThirdPartyCustomerDataAccess.Insert(customer);
        }
     
        public long UpdateThirdPartyCustomer(ThirdPartyCustomer customer)
        {
            return _ThirdPartyCustomerDataAccess.Update(customer);
        }
        public long InsertCustomerSecurityZone(CustomerSecurityZones customerZones)
        {
            return _CustomerSecurityZonesDataAccess.Insert(customerZones);
        }
        public long UpdateCustomerSecurityZone(CustomerSecurityZones customerZones)
        {
            return _CustomerSecurityZonesDataAccess.Update(customerZones);
        }
        public long InsertCustomerAgency(CustomerThirdPartyAgency thirdPartyAgency)
        {
            return _CustomerThirdPartyAgencyDataAccess.Insert(thirdPartyAgency);
        }
        public long InsertThirdpartyCustomerEmgContact(ThirdPartyEmergencyContact emergencyContact)
        {
            return _ThirdPartyEmergencyContactDataAccess.Insert(emergencyContact);
        }
        public long UpdateThirdpartyCustomerEmgContact(ThirdPartyEmergencyContact emergencyContact)
        {
            return _ThirdPartyEmergencyContactDataAccess.Update(emergencyContact);
        }
        public long InsertZoneEquipmentLocation(ZonesEquipmentLocation ZonesLocation)
        {
            return _ZonesEquipmentLocationDataAccess.Insert(ZonesLocation);
        }
        public List<ZonesEquipmentLocation> GetAllZoneEquipmentLocation()
        {
            return _ZonesEquipmentLocationDataAccess.GetAll();
        }
        public long InsertZoneEquipmentType(ZonesEquipmentType ZonesEquipmentType)
        {
            return _ZonesEquipmentTypeDataAccess.Insert(ZonesEquipmentType);
        }
        public List<ZonesEquipmentType> GetAllZoneEquipmentType()
        {
            return _ZonesEquipmentTypeDataAccess.GetAll();
        }

        public long InsertZoneEquipmentTypeEventMap(ZonesEquipmentTypeEventMap ZonesEquipmentTypeEventMap)
        {
            return _ZonesEquipmentTypeEventMapDataAccess.Insert(ZonesEquipmentTypeEventMap);
        }

        public List<ZonesEquipmentTypeEventMap> GetEquipmentTypeEventMapByEventCode(string eventCode)
        {
            return _ZonesEquipmentTypeEventMapDataAccess.GetByQuery(string.Format("EventId ='{0}'", eventCode)).ToList();
        }
        public long DeleteCustomerSecurityZone(int id)
        {
            return _CustomerSecurityZonesDataAccess.Delete(id);
        }
        public long DeleteCustomerThirdPartyEmergencyContact(int id)
        {
            return _ThirdPartyEmergencyContactDataAccess.Delete(id);
        }
        public long DeleteCustomerThirdPartyAgency(int id)
        {
            return _CustomerThirdPartyAgencyDataAccess.Delete(id);
        }
        public ThirdPartyEmergencyContact GetCustomerThirdPartyEmergencyContact(int id)
        {
            return _ThirdPartyEmergencyContactDataAccess.Get(id);
        }
        public List<CustomerSecurityZones> GetAllCustomerSecurityZoneByCustomerId(Guid CustomerId,string Platform)
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
                           LocationVal = dr["LocationVal"].ToString(),
                           CustomerId = (Guid)dr["CustomerId"],
                           Platform = dr["Platform"].ToString(),
                           EquipmentType = dr["EquipmentType"].ToString(),
                           EquipmentTypeVal = dr["EquipmentTypeVal"].ToString(),
                           ZoneComment = dr["ZoneComment"].ToString()

                       }).ToList();
            return SecurityZones;



        }
        public CustomerSecurityZones GetCustomerSecurityZoneById(int id)
        {
            return _CustomerSecurityZonesDataAccess.Get(id);
        }
        public List<ThirdPartyEmergencyContact> GetAllThirdPartyCustomerByCustomerId(Guid CustomerId, string Platform)
        {
            return _ThirdPartyEmergencyContactDataAccess.GetByQuery(string.Format("CustomerId ='{0}' and Platform ={1}", CustomerId, Platform)).ToList();

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
                                 AgencyNo= dr["AgencyNo"].ToString(),
                                 Agencytype = dr["Agencytype"].ToString(),
                                 Platform = dr["Platform"].ToString(),
                                 AgencytypeVal = dr["AgencytypeVal"].ToString(),
                                 PermTypeVal = dr["PermTypeVal"].ToString(),

                             }).ToList();
            return ThirdPartyAgency;
           
         

        }
        public long InsertAgemniCustomers(Customer customer)
        {
            return _CustomerDataAccess.Insert(customer);
        }

        public long InsertCustomerMigration(CustomerMigration customer)
        {
            return _CustomerMigrationDataAccess.Insert(customer);
        }

        public CustomerMigration GetAgemniCustomerByReferenceId(int id)
        {
           
            return _CustomerMigrationDataAccess.GetByQuery(string.Format("refenrenceId ='{0}' ", id)).ToList().FirstOrDefault();
        }
        public CustomerMigration GetCustomerMigrationByReferenceId(int referenceId)
        {
            return _CustomerMigrationDataAccess.GetByQuery(string.Format(" RefenrenceId ='{0}' ", referenceId)).ToList().FirstOrDefault();
        }
        public CustomerMigration GetCustomerMigrationByReferenceId(int referenceId,string Platform)
        {
            string SqlQuery = string.Format(" RefenrenceId ='{0}' and [Platform] ='{1}' ", referenceId, Platform);
            return _CustomerMigrationDataAccess.GetByQuery(SqlQuery).FirstOrDefault();
        }
        public long UpdateCustomerMigration(CustomerMigration CusMigration)
        {

            return _CustomerMigrationDataAccess.Update(CusMigration);
        }
        public CustomerMigration GetAgemniCustomerByCustomerId(Guid CustomerId)
        {

            return _CustomerMigrationDataAccess.GetByQuery(string.Format("CustomerId ='{0}' ", CustomerId)).ToList().FirstOrDefault();
        }
        public bool IsSameWorkOrder(string Notes)
        {
            DataTable dt = _CustomerMigrationDataAccess.IsSameWorkOrder(Notes);
            List<Ticket> tickets = new List<Ticket>();
            tickets = (from DataRow dr in dt.Rows
                          select new Ticket()
                          {
                              Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0

                          }).ToList();
            if(tickets.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
       
            // return _CustomerMigrationDataAccess.GetByQuery(string.Format("CustomerId ='{0}' ", CustomerId)).ToList().FirstOrDefault();
        }
        public long InsertCreditScoreGrade(CreditScoreGrade creditScoreGrade)
        {
            return _CreditScoreGradeDataAccess.Insert(creditScoreGrade);
        }
        public long UpdateCreditScoreGrade(CreditScoreGrade creditScoreGrade)
        {
            return _CreditScoreGradeDataAccess.Update(creditScoreGrade);
        }

        public List<CreditScoreGrade> GetAllCreditScoreGrade()
        {
            return _CreditScoreGradeDataAccess.GetAll();
        }
        public CreditScoreGrade GetCreditScoreGradeById(int Id)
        {
            return _CreditScoreGradeDataAccess.Get(Id);
        }

        public CreditScoreGrade GetCreditScoreGradeByScoreRange(int Score)
        {
            DataTable dt = _CreditScoreGradeDataAccess.GetCreditScoreGradeByScoreRange(Score);
            CreditScoreGrade scoreGrade = new CreditScoreGrade();
            scoreGrade = (from DataRow dr in dt.Rows
                          select new CreditScoreGrade()
                          {
                              ID = dr["ID"] != DBNull.Value ? Convert.ToInt32(dr["ID"]) : 0,
                              CreditGradeId = (Guid)dr["CreditGradeId"],
                          }).ToList().FirstOrDefault();
            return scoreGrade;

        }




        public long DeleteCreditScoreGrade(int Id)
        {
            return _CreditScoreGradeDataAccess.Delete(Id);
        }


        public long InsertCustomerTermination(AlarmCustomerTermination cusTerm)
        {
            return _TerminationDataAccess.Insert(cusTerm);
        }
        public bool CheckCustomerQueueCancellationById(int id)
        {
            DataTable dt = _CustomerDataAccess.CheckCustomerQueueCancellationById(id);
            CustomerCancellationQueue CustomerCancellationQueue = new CustomerCancellationQueue();
            CustomerCancellationQueue = (from DataRow dr in dt.Rows
                                         select new CustomerCancellationQueue()
                                         {

                                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0
                                         }).ToList().FirstOrDefault();
            if (CustomerCancellationQueue != null)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        public List<CustomerCancellationQueue> GetActiveCustomerQueueCancellationByCustomerId(Guid CustomerId)
        {
            return _CustomerCancellationQueueDataAccess.GetByQuery(string.Format("IsActive = 1 and CustomerId ='{0}' ", CustomerId)).ToList();

        }
        public long InsertCustomerCancellationQueue(CustomerCancellationQueue customerCancellation)
        {
            return _CustomerCancellationQueueDataAccess.Insert(customerCancellation);
        }
        public long UpdateCustomerCancellationQueue(CustomerCancellationQueue customerCancellation)
        {
            return _CustomerCancellationQueueDataAccess.Update(customerCancellation);
        }
        public CustomerCancellationQueue GetCustomerCancellationQueueByCustomerId(Guid CustomerId)
        {
            return _CustomerCancellationQueueDataAccess.GetByQuery(string.Format("CustomerId ='{0}'", CustomerId)).OrderByDescending(x => x.Id).ToList().FirstOrDefault();
        }

        public CustomerCancellationQueue GetCustomerCancellationQueueById(int Id)
        {
            return _CustomerCancellationQueueDataAccess.Get(Id);
        }
        public long DeleteCustomerCancellationQueueById(int Id)
        {
            return _CustomerCancellationQueueDataAccess.Delete(Id);
        }
        public long InsertCustomerContactTrack(CustomerContactTrack CustomerContactTrack)
        {
            return _CustomerContactTrackDataAccess.Insert(CustomerContactTrack);
        }
        public CustomerContactTrack GetCustomerContactTrackByPlatformIdAndCustomerId(int PlatformId, Guid CustomerId)
        {
            return _CustomerContactTrackDataAccess.GetByQuery(string.Format("PlatformId ='{0}' and CustomerId ='{1}' ", PlatformId, CustomerId)).FirstOrDefault();
        }
        public List<CustomerContactTrack> GetCustomerContactTrackByCustomerId(Guid CustomerId)
        {
            return _CustomerContactTrackDataAccess.GetByQuery(string.Format("CustomerId ='{0}' ", CustomerId));
        }



        public long InsertCustomerCreditCheck(CustomerCreditCheck customerCreditCheck)
        {
            return _CustomerCreditCheckDataAccess.Insert(customerCreditCheck);
        }
        public List<CustomerCreditCheck> GetAllCustomerCreditCheckByCustomerId(Guid CustomerId)
        {
            DataTable dt = _CustomerCreditCheckDataAccess.GetAllCustomerCreditCheckByCustomerId(CustomerId);
            List<CustomerCreditCheck> CustomerCreditCheck = new List<CustomerCreditCheck>();
            CustomerCreditCheck = (from DataRow dr in dt.Rows
                                   select new CustomerCreditCheck()
                                   {
                                       FirstName = dr["FirstName"].ToString(),
                                       LastName = dr["LastName"].ToString(),
                                       Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                       Score = dr["Score"].ToString(),
                                       Grade=dr["Grade"].ToString(),
                                       ReportPdfLink = dr["ReportPdfLink"].ToString(),
                                       RepontPdfName = dr["RepontPdfName"].ToString(),
                                       CustomerId = (Guid)dr["CustomerId"],
                                       CreatedBy = (Guid)dr["CreatedBy"],
                                       CreatedByVal = dr["CreatedByVal"].ToString(),
                                       Hit = dr["Hit"].ToString(),
                                       CreditCheckDate = dr["CreditCheckDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreditCheckDate"]) : new DateTime(),
                                   }).ToList();
            return CustomerCreditCheck;

        }
        public long InsertCustomerAnnouncement(Announcement announcement)
        {
            return _AnnouncementDataAccess.Insert(announcement);
        }
        public long InsertCustomerExistingItem(CustomerExistingItem item)
        {
            return _CustomerExistingItemDataAccess.Insert(item);
        }
        public int InsertCustomerAndReturnId(Customer customer)
        {
            return (int)_CustomerDataAccess.Insert(customer);
        }
        public bool UpdateCustomer(Customer customer)
        {
            return _CustomerDataAccess.Update(customer) > 0;
        }
        public bool UpdateCustomerExtended(CustomerExtended customer)
        {
            return _CustomerExtendedDataAccess.Update(customer) > 0;
        }
        public bool UpdateCustomerChange(Guid CustomerId, string ColumnName, string NewValue, DateTime LastUpdatedDate, Guid LastUpdatedByUid, string LastUpdatedBy)
        {
            return _CustomerDataAccess.UpdateCustomerChange(CustomerId, ColumnName, NewValue, LastUpdatedDate, LastUpdatedByUid, LastUpdatedBy);
        }

        public bool UpdateCustomerChangeSysinfo(Guid CustomerId, string ColumnName, string NewValue)
        {
            return _CustomerSystemInfoDataAccess.UpdateCustomerSysinfo(CustomerId, ColumnName, NewValue);
        }

        public bool DeleteCustomer(int customerId)
        {
            return _CustomerDataAccess.Delete(customerId) > 0;
        }
        public long InsertCustomerCompany(CustomerCompany cc)
        {
            return _CustomerCompanyDataAccess.Insert(cc);
        }
        public bool DeleteCustomerAndCustomerCompanyByIdAndCompanyId(Guid CustomerId, Guid CompanyId)
        {
            return _CustomerDataAccess.DeleteCustomerAndCustomerCompanyByIdAndCompanyId(CustomerId, CompanyId);
        }

        public CustomerCompany GetCustomerCompanyByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId)
        {
            return _CustomerCompanyDataAccess.GetByQuery(string.Format("CustomerId ='{0}' and CompanyId ='{1}'", CustomerId, CompanyId)).FirstOrDefault();
        }
        public bool ConvertLeadToCustomer(Guid CustomerId, Guid CompanyId, string ConvertionType)
        {
            CustomerCompany objCustomerCompany = new CustomerCompany();
            objCustomerCompany = _CustomerCompanyDataAccess.GetByQuery(string.Format("CustomerId ='{0}' and CompanyId ='{1}'", CustomerId, CompanyId)).FirstOrDefault();
            if (objCustomerCompany.IsLead == true)
            {
                objCustomerCompany.IsLead = false;
            }
            objCustomerCompany.ConvertionDate = DateTime.Now.UTCCurrentTime();
            objCustomerCompany.ConvertionType = ConvertionType;
            return _CustomerCompanyDataAccess.Update(objCustomerCompany) > 0;

        }

        public Customer GetCustoemrBySubscriptionId(string subscriptionId)
        {
            return _CustomerDataAccess.GetByQuery(string.Format("AuthorizeRefId = '{0}'", subscriptionId)).FirstOrDefault();
        }
        public Customer GetCustoemrByScheduleToken(string subscriptionId)
        {
            return _CustomerDataAccess.GetByQuery(string.Format("ScheduleToken = '{0}'", subscriptionId)).FirstOrDefault();
        }
        public Customer GetCustomerByCustomerId(int customerId)
        {
            return _CustomerDataAccess.GetCustomersById(customerId);
        }
        public Customer GetEmployeeByEmployeeId(Guid CustomerId)
        {
            return _CustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", CustomerId)).FirstOrDefault();
        }
        public List<CustomerFile> GetAllFileNameByCustomerIdAndComapanyId(Guid CustomerId, Guid ComapanyId)
        {
            return _CustomerFileDataAccess.GetByQuery(string.Format(" CompanyId = '{0}' and  CustomerId = '{1}' and IsActive = 1", ComapanyId, CustomerId));
        }
        public List<CustomerFile> GetAllCustomerFileNameByCustomerId(Guid customerID, Guid companyID)
        {
            DataTable dt = _CustomerFileDataAccess.GetAllCustomerFileNameByCustomerId(customerID, companyID);
            List<CustomerFile> CustomerFileName = new List<CustomerFile>();
            CustomerFileName = (from DataRow dr in dt.Rows
                                select new CustomerFile()
                                {
                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    FileDescription = dr["FileDescription"].ToString(),
                                    Filename = dr["Filename"].ToString(),
                                    CustomerId = (Guid)dr["CustomerId"],
                                    CompanyId = (Guid)dr["CompanyId"],
                                    Uploadeddate = dr["Uploadeddate"] != DBNull.Value ? Convert.ToDateTime(dr["Uploadeddate"]) : new DateTime(),
                                }).ToList();
            return CustomerFileName;
        }
        public List<CustomerIdList> GetCustomerReportByFilter(CustomerFilter filter)
        {
            DataSet ds = _CustomerDataAccess.GetCustomerReportByFilter(filter);

            List<CustomerIdList> CustomerList = new List<CustomerIdList>();
            CustomerList = (from DataRow dr in ds.Tables[0].Rows
                            select new CustomerIdList()
                            {
                                customerId = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,

                            }).ToList();


            return CustomerList;
        }

        public List<CustomerIdList> GetLeadReportByFilter(CustomerFilter filter)
        {
            DataSet ds = _CustomerDataAccess.GetLeadReportByFilter(filter);

            List<CustomerIdList> CustomerList = new List<CustomerIdList>();
            CustomerList = (from DataRow dr in ds.Tables[0].Rows
                            select new CustomerIdList()
                            {
                                customerId = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,

                            }).ToList();


            return CustomerList;
        }


        public CustomerListWithCountModel GetCustomerByFilter(CustomerFilter filter)
        {
            DataSet ds = _CustomerDataAccess.GetCustomerListByFilter(filter);

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
                                LeadSourceType=dr["LeadSourceType"].ToString(),
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
                                ContactedPerviously = dr["ContactedPerviously"].ToString(),
                                InstalledStatus = dr["InstalledStatus"].ToString(),
                                AcquiredFrom = dr["AcquiredFrom"].ToString(),
                                BuyoutAmountByADS = dr["BuyoutAmountByADS"] != DBNull.Value ? Convert.ToDouble(dr["BuyoutAmountByADS"]) : 0,
                                BuyoutAmountBySalesRep = dr["BuyoutAmountBySalesRep"] != DBNull.Value ? Convert.ToDouble(dr["BuyoutAmountBySalesRep"]) : 0,
                                FinancedTerm = dr["FinancedTerm"] != DBNull.Value ? Convert.ToDouble(dr["FinancedTerm"]) : 0,
                                FinancedAmount = dr["FinancedAmount"] != DBNull.Value ? Convert.ToDouble(dr["FinancedAmount"]) : 0,
                                Levels = dr["Levels"] != DBNull.Value ? Convert.ToDouble(dr["Levels"]) : 0,
                                SoldAmount = dr["SoldAmount"] != DBNull.Value ? Convert.ToDouble(dr["SoldAmount"]) : 0,
                                AgreementEmail = dr["AgreementEmail"].ToString(),
                                AgreementPhoneNo = dr["AgreementPhoneNo"].ToString(),
                                TaxExemption = dr["TaxExemption"].ToString(),
                                AppoinmentSet = dr["AppoinmentSet"].ToString(),
                                PlatformId = dr["PlatformId"].ToString(),
                                MapscoNo = dr["MapscoNo"].ToString(),
                                ContractStartDate = dr["ContractStartDate"] != DBNull.Value ? Convert.ToDateTime(dr["ContractStartDate"]) : new DateTime(),
                                RemainingContractTerm = dr["RemainingContractTerm"].ToString()
                            }).ToList();

            TotalCustomerCount TotalCustomer = new TotalCustomerCount();
            TotalCustomer = (from DataRow dr in ds.Tables[1].Rows
                             select new TotalCustomerCount()
                             {
                                 Counter = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                             }).FirstOrDefault();

            var totalCountActive = ds != null && ds.Tables.Count > 1 ? ds.Tables[1] : null;



            CustomerListWithCountModel CustomerResultModel = new CustomerListWithCountModel();
            CustomerResultModel.CustomerList = CustomerList;
            CustomerResultModel.TotalCustomerCount = TotalCustomer;


            return CustomerResultModel;
        }

        public CustomerListWithCountModel GetLeadsByFilter(CustomerFilter filter)
        {
            CustomerListWithCountModel CustomerResultModel = new CustomerListWithCountModel();
            //DataSet ds = _CustomerDataAccess.GetCustomerListByFilter(filter);
            DataSet ds = _CustomerDataAccess.GetLeadListByFilterNew(filter);
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
                                //Installer = dr["Installer"].ToString(),
                                Soldby = dr["Soldby"].ToString(),
                                FundingDate = dr["FundingDate"] != DBNull.Value ? Convert.ToDateTime(dr["FundingDate"]) : new DateTime(),
                                ReminderDate = dr["ReminderDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReminderDate"]) : new DateTime(),
                                //QA1 = dr["QA1"].ToString(),
                                QA1Date = dr["QA1Date"] != DBNull.Value ? Convert.ToDateTime(dr["QA1Date"]) : new DateTime(),
                                //QA2 = dr["QA2"].ToString(),
                                QA2Date = dr["QA2Date"] != DBNull.Value ? Convert.ToDateTime(dr["QA2Date"]) : new DateTime(),
                                BillAmount = dr["BillAmount"] != DBNull.Value ? Convert.ToDouble(dr["BillAmount"]) : 0.0,
                                PaymentMethod = dr["PaymentMethod"].ToString(),
                                BillCycle = dr["BillCycle"].ToString(),
                                LeadSourceType=dr["LeadSourceType"].ToString(),
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
                                EMPNUM = dr["EMPNUM"].ToString(),
                                Type = dr["Type"].ToString(),
                                InstallType = dr["InstallType"].ToString(),
                                BranchName = dr["BranchName"].ToString(),
                                DoNotCall = dr["DoNotCall"] != DBNull.Value ? Convert.ToDateTime(dr["DoNotCall"]) : new DateTime(),
                                PreferredContactMethod = dr["PreferredContactMethod"].ToString(),
                                LeadSourceVal = dr["LeadSourceVal"].ToString(),
                                StatusVal = dr["Status"].ToString(),
                                MovingDate = dr["MovingDate"] != DBNull.Value ? Convert.ToDateTime(dr["MovingDate"]) : new DateTime(),
                                ContactedPerviously = dr["ContactedPerviously"].ToString(),
                                InstalledStatus = dr["InstalledStatus"].ToString(),
                                AcquiredFrom = dr["AcquiredFrom"].ToString(),
                                BuyoutAmountByADS = dr["BuyoutAmountByADS"] != DBNull.Value ? Convert.ToDouble(dr["BuyoutAmountByADS"]) : 0,
                                BuyoutAmountBySalesRep = dr["BuyoutAmountBySalesRep"] != DBNull.Value ? Convert.ToDouble(dr["BuyoutAmountBySalesRep"]) : 0,
                                FinancedTerm = dr["FinancedTerm"] != DBNull.Value ? Convert.ToDouble(dr["FinancedTerm"]) : 0,
                                FinancedAmount = dr["FinancedAmount"] != DBNull.Value ? Convert.ToDouble(dr["FinancedAmount"]) : 0,
                                Levels = dr["Levels"] != DBNull.Value ? Convert.ToDouble(dr["Levels"]) : 0,
                                SoldAmount = dr["SoldAmount"] != DBNull.Value ? Convert.ToDouble(dr["SoldAmount"]) : 0,
                                AgreementEmail = dr["AgreementEmail"].ToString(),
                                AgreementPhoneNo = dr["AgreementPhoneNo"].ToString(),
                                TaxExemption = dr["TaxExemption"].ToString(),
                                AppoinmentSet = dr["AppoinmentSet"].ToString(),
                                PlatformId = dr["PlatformId"].ToString(),
                                MapscoNo = dr["MapscoNo"].ToString()
                            }).ToList();
            TotalCustomerCount TotalCustomer = new TotalCustomerCount();
            TotalCustomer = (from DataRow dr in ds.Tables[1].Rows
                             select new TotalCustomerCount()
                             {
                                 Counter = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                             }).FirstOrDefault();
            CustomerResultModel.LeadTabCount = (from DataRow dr in ds.Tables[1].Rows
                                                select new LeadTabCount()
                                                {
                                                    LeadCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0,
                                                }).FirstOrDefault();
            #region Not in use
            /*CustomerResultModel.LeadTabEstimateCount = (from DataRow dr in ds.Tables[2].Rows
                                                        select new LeadTabEstimateCount()
                                                        {
                                                            EstimateCount = dr["EstimateCount"] != DBNull.Value ? Convert.ToInt32(dr["EstimateCount"]) : 0,
                                                        }).FirstOrDefault();
            CustomerResultModel.LeadTabEstimateAmount = (from DataRow dr in ds.Tables[2].Rows
                                                         select new LeadTabEstimateAmount()
                                                         {
                                                             LeadEstimateAmount = dr["LeadEstimateAmount"] != DBNull.Value ? Convert.ToDouble(dr["LeadEstimateAmount"]) : 0,
                                                         }).FirstOrDefault();

            CustomerResultModel.LeadTabThisMonthCount = (from DataRow dr in ds.Tables[3].Rows
                                                         select new LeadTabThisMonthCount()
                                                         {
                                                             LeadThisMonthCount = dr["LeadThisMonthCount"] != DBNull.Value ? Convert.ToInt32(dr["LeadThisMonthCount"]) : 0,
                                                         }).FirstOrDefault();
            CustomerResultModel.LeadTabLastMonthCount = (from DataRow dr in ds.Tables[4].Rows
                                                         select new LeadTabLastMonthCount()
                                                         {
                                                             LeadLastMonthCount = dr["LeadLastMonthCount"] != DBNull.Value ? Convert.ToInt32(dr["LeadLastMonthCount"]) : 0,
                                                         }).FirstOrDefault();
            
            CustomerResultModel.LeadTabBookingCount = (from DataRow dr in ds.Tables[5].Rows
                                                       select new LeadTabBookingCount()
                                                       {
                                                           BookingCount = dr["BookingCount"] != DBNull.Value ? Convert.ToInt32(dr["BookingCount"]) : 0,
                                                       }).FirstOrDefault();
            CustomerResultModel.LeadTabBookingAmount = (from DataRow dr in ds.Tables[5].Rows
                                                        select new LeadTabBookingAmount()
                                                        {
                                                            LeadBookingAmount = dr["LeadBookingAmount"] != DBNull.Value ? Convert.ToDouble(dr["LeadBookingAmount"]) : 0,
                                                        }).FirstOrDefault();

    */
            #endregion

            var totalCount = ds != null && ds.Tables.Count > 1 ? ds.Tables[1] : null;
            CustomerResultModel.CustomerList = CustomerList;
            CustomerResultModel.TotalCustomerCount = TotalCustomer;
            return CustomerResultModel;
        }

        public List<Customer> GetAllCustomerListByCompanyId(Guid CompanyId)
        {
            DataTable dt = _CustomerDataAccess.GetAllCustomerListByCompany(CompanyId);
            List<Customer> CustomerList = new List<Customer>();
            CustomerList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                //ContactName = dr["ContactName"].ToString(),
                                AccountNo = dr["AccountNo"].ToString(),
                                CustomerNo = dr["CustomerNo"].ToString(),
                                EmailAddress = dr["EmailAddress"].ToString(),
                                Address = dr["Address"].ToString(),
                                BusinessName = dr["BusinessName"].ToString(),
                                CallingTime = dr["CallingTime"].ToString(),
                                CellNo = dr["CellNo"].ToString(),
                                CellularBackup = (dr["CellularBackup"] != DBNull.Value ? Convert.ToBoolean(dr["CellularBackup"]) : false),
                                City = dr["City"].ToString(),
                                ContractTeam = dr["ContractTeam"].ToString(),
                                Country = dr["Country"].ToString(),
                                CreditScore = dr["CreditScore"].ToString(),
                                CustomerFunded = (dr["CustomerFunded"] != DBNull.Value ? Convert.ToBoolean(dr["CustomerFunded"]) : false),
                                CustomerId = (Guid)dr["CustomerId"],
                                Fax = dr["Fax"].ToString(),
                                FirstName = dr["FirstName"].ToString(),
                                FundingCompany = dr["FundingCompany"].ToString(),
                                IsAlarmCom = dr["IsAlarmCom"] != DBNull.Value ? Convert.ToBoolean(dr["IsAlarmCom"]) : false,
                                LastName = dr["LastName"].ToString(),
                                LeadSource = dr["LeadSource"].ToString(),
                                Maintenance = dr["Maintenance"] != DBNull.Value ? Convert.ToBoolean(dr["Maintenance"]) : false,
                                MonthlyMonitoringFee = dr["MonthlyMonitoringFee"].ToString(),
                                Note = dr["Note"].ToString(),
                                PrimaryPhone = dr["PrimaryPhone"].ToString(),
                                SecondaryPhone = dr["SecondaryPhone"].ToString(),
                                SSN = dr["SSN"].ToString(),
                                State = dr["State"].ToString(),
                                Street = dr["Street"].ToString(),
                                Title = dr["Title"].ToString(),
                                Type = dr["Type"].ToString(),
                                ZipCode = dr["ZipCode"].ToString(),
                                DateofBirth = dr["DateofBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DateofBirth"]) : new DateTime(),
                                LastUpdatedBy = dr["LastUpdatedBy"].ToString()
                            }).ToList();
            return CustomerList;
        }
        public List<Customer> GetAllUserListByCompanyId(Guid companyId)
        {
            return _CustomerDataAccess.GetByQuery(string.Format(" CompanyId = '{0}'", companyId));
        }

        public List<Customer> GetAllLeadListByFiltering(Guid companyId, string UserList, string SourceList, string firstdate, string lastdate)
        {
            DataTable dt = _CustomerDataAccess.GetAllLeadListByFiltering(companyId, UserList, SourceList, firstdate, lastdate);
            List<Customer> leadList = new List<Customer>();
            leadList = (from DataRow dr in dt.Rows
                        select new Customer()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            //ContactName = dr["ContactName"].ToString(),
                            AccountNo = dr["AccountNo"].ToString(),
                            EmailAddress = dr["EmailAddress"].ToString(),
                            Address = dr["Address"].ToString(),
                            BusinessName = dr["BusinessName"].ToString(),
                            CallingTime = dr["CallingTime"].ToString(),
                            CellNo = dr["CellNo"].ToString(),
                            CellularBackup = (dr["CellularBackup"] != DBNull.Value ? Convert.ToBoolean(dr["CellularBackup"]) : false),
                            City = dr["City"].ToString(),
                            ContractTeam = dr["ContractTeam"].ToString(),
                            Country = dr["Country"].ToString(),
                            CreditScore = dr["CreditScore"].ToString(),
                            CustomerFunded = (dr["CustomerFunded"] != DBNull.Value ? Convert.ToBoolean(dr["CustomerFunded"]) : false),
                            CustomerId = (Guid)dr["CustomerId"],
                            Fax = dr["Fax"].ToString(),
                            FirstName = dr["FirstName"].ToString(),
                            FundingCompany = dr["FundingCompany"].ToString(),
                            IsAlarmCom = dr["IsAlarmCom"] != DBNull.Value ? Convert.ToBoolean(dr["IsAlarmCom"]) : false,
                            LastName = dr["LastName"].ToString(),
                            LeadSource = dr["LeadSource"].ToString(),
                            Maintenance = dr["Maintenance"] != DBNull.Value ? Convert.ToBoolean(dr["Maintenance"]) : false,
                            MonthlyMonitoringFee = dr["MonthlyMonitoringFee"].ToString(),
                            Note = dr["Note"].ToString(),
                            PrimaryPhone = dr["PrimaryPhone"].ToString(),
                            SecondaryPhone = dr["SecondaryPhone"].ToString(),
                            SSN = dr["SSN"].ToString(),
                            State = dr["State"].ToString(),
                            Street = dr["Street"].ToString(),
                            Title = dr["Title"].ToString(),
                            Type = dr["Type"].ToString(),
                            ZipCode = dr["ZipCode"].ToString(),
                            DateofBirth = dr["DateofBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DateofBirth"]) : new DateTime(),
                            MiddleName = dr["MiddleName"].ToString(),
                            LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                            LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime()
                        }).ToList();
            return leadList;
        }

        public List<Customer> GetAllCustomerListByFiltering(Guid companyId, string UserList, string SourceList, string firstdate, string lastdate)
        {
            DataTable dt = _CustomerDataAccess.GetAllCustomerListByFiltering(companyId, UserList, SourceList, firstdate, lastdate);
            List<Customer> customerList = new List<Customer>();
            customerList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                //ContactName = dr["ContactName"].ToString(),
                                AccountNo = dr["AccountNo"].ToString(),
                                CustomerNo = dr["CustomerNo"].ToString(),
                                EmailAddress = dr["EmailAddress"].ToString(),
                                Address = dr["Address"].ToString(),
                                BusinessName = dr["BusinessName"].ToString(),
                                CallingTime = dr["CallingTime"].ToString(),
                                CellNo = dr["CellNo"].ToString(),
                                CellularBackup = (dr["CellularBackup"] != DBNull.Value ? Convert.ToBoolean(dr["CellularBackup"]) : false),
                                City = dr["City"].ToString(),
                                ContractTeam = dr["ContractTeam"].ToString(),
                                Country = dr["Country"].ToString(),
                                CreditScore = dr["CreditScore"].ToString(),
                                CustomerFunded = (dr["CustomerFunded"] != DBNull.Value ? Convert.ToBoolean(dr["CustomerFunded"]) : false),
                                CustomerId = (Guid)dr["CustomerId"],
                                Fax = dr["Fax"].ToString(),
                                FirstName = dr["FirstName"].ToString(),
                                FundingCompany = dr["FundingCompany"].ToString(),
                                IsAlarmCom = dr["IsAlarmCom"] != DBNull.Value ? Convert.ToBoolean(dr["IsAlarmCom"]) : false,
                                LastName = dr["LastName"].ToString(),
                                LeadSource = dr["LeadSource"].ToString(),
                                Maintenance = dr["Maintenance"] != DBNull.Value ? Convert.ToBoolean(dr["Maintenance"]) : false,
                                MonthlyMonitoringFee = dr["MonthlyMonitoringFee"].ToString(),
                                Note = dr["Note"].ToString(),
                                PrimaryPhone = dr["PrimaryPhone"].ToString(),
                                SecondaryPhone = dr["SecondaryPhone"].ToString(),
                                SSN = dr["SSN"].ToString(),
                                State = dr["State"].ToString(),
                                Street = dr["Street"].ToString(),
                                Title = dr["Title"].ToString(),
                                Type = dr["Type"].ToString(),
                                ZipCode = dr["ZipCode"].ToString(),
                                DateofBirth = dr["DateofBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DateofBirth"]) : new DateTime(),
                                MiddleName = dr["MiddleName"].ToString(),
                                LastUpdatedBy = dr["LastUpdatedBy"].ToString(),
                                LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime()
                            }).ToList();
            return customerList;
        }

        public Customer GetInfoByCustomerId(Guid customerid)
        {
            return _CustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerid)).FirstOrDefault();
        }

        public List<CustomerNote> GetAllCustomerNoteByCustomerCompany(Guid companyId)
        {
            DataTable dt = _CustomerNoteDataAccess.GetAllCustomerNoteByCustomerCompany(companyId);
            List<CustomerNote> leadList = new List<CustomerNote>();
            leadList = (from DataRow dr in dt.Rows
                        select new CustomerNote()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            Notes = dr["Notes"].ToString(),
                            ReminderDate = dr["ReminderDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReminderDate"]) : new DateTime(),
                            CustomerId = (Guid)dr["CustomerId"],
                            CompanyId = (Guid)dr["CompanyId"],
                            CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                        }).ToList();
            return leadList;
        }
        public Customer GetAllCustomerByCustomerIdAndCompanyId(Guid companyid)
        {
            return _CustomerDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", companyid)).FirstOrDefault();
        }

        public List<CustomerNote> GetAllLeadsFollowUpByCompanyId(Guid CompanyId, Guid CustomerId)
        {
            DataTable dt = _CustomerNoteDataAccess.GetAllLeadsFollowUpByCompanyId(CompanyId, CustomerId);
            List<CustomerNote> leadList = new List<CustomerNote>();
            leadList = (from DataRow dr in dt.Rows
                        select new CustomerNote()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            Notes = dr["Notes"].ToString(),
                            Color = dr["Color"].ToString(),
                            NoteTypeValue = dr["NoteTypeValue"].ToString(),
                            ReminderDate = dr["ReminderDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReminderDate"]) : new DateTime(),
                            CustomerId = (Guid)dr["CustomerId"],
                            CompanyId = (Guid)dr["CompanyId"],
                            CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                            IsEmail = dr["IsEmail"] != DBNull.Value ? Convert.ToBoolean(dr["IsEmail"]) : false,
                            IsShedule = dr["IsShedule"] != DBNull.Value ? Convert.ToBoolean(dr["IsShedule"]) : false,
                            IsText = dr["IsText"] != DBNull.Value ? Convert.ToBoolean(dr["IsText"]) : false,
                            AssignName = dr["AssignName"].ToString().TrimEnd(' ', ','),
                            CreatedBy = dr["CreatedBy"].ToString(),
                            CreatedByUid = (Guid)dr["CreatedByUid"],
                            empName = dr["empName"].ToString(),
                            IsPin = dr["IsPin"] != DBNull.Value ? Convert.ToBoolean(dr["IsPin"]) : false,
                            IsOverview = dr["IsOverview"] != DBNull.Value ? Convert.ToBoolean(dr["IsOverview"]) : false,
                            OrderBy = dr["OrderBy"] != DBNull.Value ? Convert.ToInt32(dr["OrderBy"]) : 0,
                        }).ToList();
            return leadList;
        }

        public List<CustomerNote> GetAllCustomerFollowUpByCompanyId(Guid CompanyId, Guid CustomerId)
        {
            //DataTable dt = _CustomerNoteDataAccess.GetAllCustomerFollowUpByCompanyId(CompanyId, CustomerId);
            //List<CustomerNote> leadList = new List<CustomerNote>();
            //leadList = (from DataRow dr in dt.Rows
            //            select new CustomerNote()
            //            {
            //                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
            //                Notes = dr["Notes"].ToString(),
            //                ReminderDate = dr["ReminderDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReminderDate"]) : new DateTime(),
            //                CustomerId = (Guid)dr["CustomerId"],
            //                CompanyId = (Guid)dr["CompanyId"],
            //                CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
            //                IsEmail = dr["IsEmail"] != DBNull.Value ? Convert.ToBoolean(dr["IsEmail"]) : false,
            //                IsShedule = dr["IsShedule"] != DBNull.Value ? Convert.ToBoolean(dr["IsShedule"]) : false,
            //                IsText = dr["IsText"] != DBNull.Value ? Convert.ToBoolean(dr["IsText"]) : false
            //            }).ToList();
            //return leadList;

            return _CustomerNoteDataAccess.GetByQuery(string.Format(@"  IsShedule = 1 
                                                                        AND CompanyId = '{0}'
                                                                        AND CustomerId = '{1}'", CompanyId, CustomerId));


        }

        public List<Customer> GetAllLeadsNotConvertedLeadToCustomer()
        {
            CustomerDataAccess dataCustomer = new CustomerDataAccess();
            DataTable dt = dataCustomer.GetAllLeadsNotConvertedLeadToCustomer();
            List<Customer> ConvertLead = new List<Customer>();

            ConvertLead = (from DataRow dr in dt.Rows
                           select new Customer()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               //ContactName = dr["ContactName"].ToString(),
                               AccountNo = dr["AccountNo"].ToString(),
                               EmailAddress = dr["EmailAddress"].ToString(),
                               Address = dr["Address"].ToString(),
                               BusinessName = dr["BusinessName"].ToString(),
                               CallingTime = dr["CallingTime"].ToString(),
                               CellNo = dr["CellNo"].ToString(),
                               CellularBackup = (dr["CellularBackup"] != DBNull.Value ? Convert.ToBoolean(dr["CellularBackup"]) : false),
                               City = dr["City"].ToString(),
                               ContractTeam = dr["ContractTeam"].ToString(),
                               Country = dr["Country"].ToString(),
                               CreditScore = dr["CreditScore"].ToString(),
                               CustomerFunded = (dr["CustomerFunded"] != DBNull.Value ? Convert.ToBoolean(dr["CustomerFunded"]) : false),
                               CustomerId = (Guid)dr["CustomerId"],
                               Fax = dr["Fax"].ToString(),
                               FirstName = dr["FirstName"].ToString(),
                               FundingCompany = dr["FundingCompany"].ToString(),
                               IsAlarmCom = dr["IsAlarmCom"] != DBNull.Value ? Convert.ToBoolean(dr["IsAlarmCom"]) : false,
                               LastName = dr["LastName"].ToString(),
                               LeadSource = dr["LeadSource"].ToString(),
                               Maintenance = dr["Maintenance"] != DBNull.Value ? Convert.ToBoolean(dr["Maintenance"]) : false,
                               MonthlyMonitoringFee = dr["MonthlyMonitoringFee"].ToString(),
                               Note = dr["Note"].ToString(),
                               PrimaryPhone = dr["PrimaryPhone"].ToString(),
                               SecondaryPhone = dr["SecondaryPhone"].ToString(),
                               SSN = dr["SSN"].ToString(),
                               State = dr["State"].ToString(),
                               Street = dr["Street"].ToString(),
                               Title = dr["Title"].ToString(),
                               Type = dr["Type"].ToString(),
                               ZipCode = dr["ZipCode"].ToString(),
                               DateofBirth = dr["DateofBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DateofBirth"]) : new DateTime(),
                               JoinDate = dr["JoinDate"] != DBNull.Value ? Convert.ToDateTime(dr["JoinDate"]) : new DateTime(),
                               ReminderDate = dr["ReminderDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReminderDate"]) : new DateTime(),
                           }).ToList();
            return ConvertLead;
        }

        public List<Customer> GetAllCustomerNotSetCustomerBilling()
        {
            CustomerDataAccess dataCustomer = new CustomerDataAccess();
            DataTable dt = dataCustomer.GetAllCustomerNotSetCustomerBilling();
            List<Customer> NotSetCustomerBill = new List<Customer>();
            NotSetCustomerBill = (from DataRow dr in dt.Rows
                                  select new Customer()
                                  {
                                      Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                      //ContactName = dr["ContactName"].ToString(),
                                      AccountNo = dr["AccountNo"].ToString(),
                                      EmailAddress = dr["EmailAddress"].ToString(),
                                      Address = dr["Address"].ToString(),
                                      BusinessName = dr["BusinessName"].ToString(),
                                      CallingTime = dr["CallingTime"].ToString(),
                                      CellNo = dr["CellNo"].ToString(),
                                      CellularBackup = (dr["CellularBackup"] != DBNull.Value ? Convert.ToBoolean(dr["CellularBackup"]) : false),
                                      City = dr["City"].ToString(),
                                      ContractTeam = dr["ContractTeam"].ToString(),
                                      Country = dr["Country"].ToString(),
                                      CreditScore = dr["CreditScore"].ToString(),
                                      CustomerFunded = (dr["CustomerFunded"] != DBNull.Value ? Convert.ToBoolean(dr["CustomerFunded"]) : false),
                                      CustomerId = (Guid)dr["CustomerId"],
                                      Fax = dr["Fax"].ToString(),
                                      FirstName = dr["FirstName"].ToString(),
                                      FundingCompany = dr["FundingCompany"].ToString(),
                                      IsAlarmCom = dr["IsAlarmCom"] != DBNull.Value ? Convert.ToBoolean(dr["IsAlarmCom"]) : false,
                                      LastName = dr["LastName"].ToString(),
                                      LeadSource = dr["LeadSource"].ToString(),
                                      Maintenance = dr["Maintenance"] != DBNull.Value ? Convert.ToBoolean(dr["Maintenance"]) : false,
                                      MonthlyMonitoringFee = dr["MonthlyMonitoringFee"].ToString(),
                                      Note = dr["Note"].ToString(),
                                      PrimaryPhone = dr["PrimaryPhone"].ToString(),
                                      SecondaryPhone = dr["SecondaryPhone"].ToString(),
                                      SSN = dr["SSN"].ToString(),
                                      State = dr["State"].ToString(),
                                      Street = dr["Street"].ToString(),
                                      Title = dr["Title"].ToString(),
                                      Type = dr["Type"].ToString(),
                                      ZipCode = dr["ZipCode"].ToString(),
                                      DateofBirth = dr["DateofBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DateofBirth"]) : new DateTime(),
                                      JoinDate = dr["JoinDate"] != DBNull.Value ? Convert.ToDateTime(dr["JoinDate"]) : new DateTime(),
                                      ReminderDate = dr["ReminderDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReminderDate"]) : new DateTime(),
                                  }).ToList();
            return NotSetCustomerBill;
        }

        public string GetActiveCustomerCount(Guid companyID)
        {
            string count = "";
            DataTable dt = _CustomerDataAccess.GetAllCustomerCountByCompanyId(companyID);
            count = dt.Rows[0]["Count"].ToString();
            return count;
        }

        public LeadTabCountModel GetAllLeadTabCountByCompanyId(Guid companyID,bool ShowBookingCount,bool ShowEstimateCount)
        {
            DataSet ds = _CustomerDataAccess.GetAllLeadTabCountByCompanyId(companyID, ShowBookingCount, ShowEstimateCount);
            LeadTabCountModel model = new LeadTabCountModel();
            model.LeadTabCount = (from DataRow dr in ds.Tables[0].Rows
                                  select new LeadTabCount()
                                  {
                                      LeadCount = dr["LeadCount"] != DBNull.Value ? Convert.ToInt32(dr["LeadCount"]) : 0,
                                  }).FirstOrDefault(); 

            model.LeadTabThisMonthCount = (from DataRow dr in ds.Tables[2].Rows
                                           select new LeadTabThisMonthCount()
                                           {
                                               LeadThisMonthCount = dr["LeadThisMonthCount"] != DBNull.Value ? Convert.ToInt32(dr["LeadThisMonthCount"]) : 0,
                                           }).FirstOrDefault();
            model.LeadTabLastMonthCount = (from DataRow dr in ds.Tables[3].Rows
                                           select new LeadTabLastMonthCount()
                                           {
                                               LeadLastMonthCount = dr["LeadLastMonthCount"] != DBNull.Value ? Convert.ToInt32(dr["LeadLastMonthCount"]) : 0,
                                           }).FirstOrDefault();
            if (ShowEstimateCount)
            {
                model.LeadTabEstimateCount = (from DataRow dr in ds.Tables[1].Rows
                                              select new LeadTabEstimateCount()
                                              {
                                                  EstimateCount = dr["EstimateCount"] != DBNull.Value ? Convert.ToInt32(dr["EstimateCount"]) : 0,
                                              }).FirstOrDefault();
                model.LeadTabEstimateAmount = (from DataRow dr in ds.Tables[1].Rows
                                               select new LeadTabEstimateAmount()
                                               {
                                                   LeadEstimateAmount = dr["LeadEstimateAmount"] != DBNull.Value ? Convert.ToDouble(dr["LeadEstimateAmount"]) : 0,
                                               }).FirstOrDefault();
            }

            if (ShowBookingCount)
            {
                model.LeadTabBookingCount = (from DataRow dr in ds.Tables[4].Rows
                                             select new LeadTabBookingCount()
                                             {
                                                 BookingCount = dr["BookingCount"] != DBNull.Value ? Convert.ToInt32(dr["BookingCount"]) : 0,
                                             }).FirstOrDefault();
                model.LeadTabBookingAmount = (from DataRow dr in ds.Tables[4].Rows
                                              select new LeadTabBookingAmount()
                                              {
                                                  LeadBookingAmount = dr["LeadBookingAmount"] != DBNull.Value ? Convert.ToDouble(dr["LeadBookingAmount"]) : 0,
                                              }).FirstOrDefault();
            }
           
            return model;
        }

        public double GetTotalRMRByCompanyId(Guid companyID, string tag, Guid empid)
        {
            string count = "0";
            double TotalRMR = 0;

            DataTable dt = _CustomerDataAccess.GetTotalRMRByCompanyId(companyID, tag, empid);
            if (dt.Rows.Count > 0)
            {
                count = dt.Rows[0]["TotalRMR"].ToString();
            }
            double.TryParse(count, out TotalRMR);

            return TotalRMR;

        }

        public string GetTotalRMRCountByCompanyId(Guid companyID, string tag, Guid empid)
        {
            string count = "0";
            DataTable dt = _CustomerDataAccess.GetTotalRMRCountByCompanyId(companyID, tag, empid);
            if (dt.Rows.Count > 0)
            {
                count = dt.Rows[0]["RmrCount"].ToString();
            }
            return count;
        }


        public CustomerHeaderMoneyBar GetAllCustomerCountTotalRMREstimateAmountByCompanyId(Guid companyID, string tag, Guid empid, bool ispermit)
        {
            DataTable dt = _CustomerDataAccess.GetAllCustomerCountTotalRMREstimateAmountByCompanyId(companyID, tag, empid, ispermit);
            List<CustomerHeaderMoneyBar> CustomerHeaderMoneyBarModel = new List<CustomerHeaderMoneyBar>();
            if (dt != null)
                CustomerHeaderMoneyBarModel = (from DataRow dr in dt.Rows
                                               select new CustomerHeaderMoneyBar()
                                               {
                                                   CustomerCount = !string.IsNullOrWhiteSpace(dr["CustomerCount"].ToString()) ? (dr["CustomerCount"]).ToString() : "0",
                                                   TotalRMR = !string.IsNullOrWhiteSpace(dr["TotalRMR"].ToString()) ? (dr["TotalRMR"]).ToString() : "0",
                                                   TotalRMRCount = !string.IsNullOrWhiteSpace(dr["TotalRMRCount"].ToString()) ? (dr["TotalRMRCount"]).ToString() : "0",
                                                   DueAmount = !string.IsNullOrWhiteSpace(dr["DueAmount"].ToString()) ? (dr["DueAmount"]).ToString() : "0",
                                                   EstimateAmount = !string.IsNullOrWhiteSpace(dr["EstimateAmount"].ToString()) ? (dr["EstimateAmount"]).ToString() : "0"
                                               }).ToList();
            return CustomerHeaderMoneyBarModel.FirstOrDefault();
        }



        public string GetCustomerSystemInfoIdByCompanyId(Guid Companyid, Guid Customerid)
        {
            string count = "";
            DataTable dt = _CustomerSystemInfoDataAccess.GetCustomerSystemInfoIdByCompanyId(Companyid, Customerid);
            //Id = dt.Rows["Id"] != DBNull.Value ? Convert.ToInt32(dt.Rows["Id"]) : 0;

            if (dt.Rows.Count > 0)
            {
                count = dt.Rows[0]["Id"].ToString();
            }
            return count;
        }
        public string GetCustomerSystemInfoLastUpdateDateByCompanyIdandCustomerId(Guid Companyid, Guid Customerid)
        {
            string Dcount = "";
            DataTable dt = _CustomerSystemInfoDataAccess.GetCustomerSystemInfoLastUpdateDateByCompanyIdandCustomerId(Companyid, Customerid);
            //Id = dt.Rows["Id"] != DBNull.Value ? Convert.ToInt32(dt.Rows["Id"]) : 0;

            if (dt.Rows.Count > 0)
            {
                Dcount = dt.Rows[0]["IsLead"].ToString();
            }
            return Dcount;
        }
        public string GetTotalRevenueByCustomerIdandCompanyId(Guid customerId, Guid companyId)
        {
            string Revenueinvoice = "";
            DataTable dt = _InvoiceDataAccess.GetTotalRevenueByCustomerIdandCompanyId(customerId, companyId);
            Revenueinvoice = dt.Rows[0]["TotalRevenue"].ToString();
            return Revenueinvoice;
        }
        public CustomerApiSetting GetCustomerApiAlarmIdByCompanyIdandCustomerId(Guid Companyid, Guid Customerid)
        {
            DataTable dt = _CustomerApiSettingDataAccess.GetCustomerApiAlarmIdByCompanyIdandCustomerId(Companyid, Customerid);
            CustomerApiSetting AlarmList = new CustomerApiSetting();
            AlarmList = (from DataRow dr in dt.Rows
                         select new CustomerApiSetting()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             CompanyId = (Guid)dr["CompanyId"],
                             CustomerId = (Guid)dr["CustomerId"],
                             Url = dr["Url"].ToString(),
                             UserName = dr["UserName"].ToString(),
                             Password = dr["Password"].ToString()
                         }).ToList().FirstOrDefault();
            return AlarmList;
        }

        public CustomerApiSetting GetCustomerApiMonitronicsIdByCompanyIdandCustomerId(Guid Companyid, Guid Customerid)
        {
            DataTable dt = _CustomerApiSettingDataAccess.GetCustomerApiMonitronicsIdByCompanyIdandCustomerId(Companyid, Customerid);
            CustomerApiSetting MoniList = new CustomerApiSetting();
            MoniList = (from DataRow dr in dt.Rows
                        select new CustomerApiSetting()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            CompanyId = (Guid)dr["CompanyId"],
                            CustomerId = (Guid)dr["CustomerId"],
                            Url = dr["Url"].ToString(),
                            UserName = dr["UserName"].ToString(),
                            Password = dr["Password"].ToString()
                        }).ToList().FirstOrDefault();
            return MoniList;
        }

        public CustomerApiSetting GetCustomerApiCentralIdByCompanyIdandCustomerId(Guid Companyid, Guid Customerid)
        {
            DataTable dt = _CustomerApiSettingDataAccess.GetCustomerApiCentralIdByCompanyIdandCustomerId(Companyid, Customerid);
            CustomerApiSetting CentralList = new CustomerApiSetting();
            CentralList = (from DataRow dr in dt.Rows
                           select new CustomerApiSetting()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               CompanyId = (Guid)dr["CompanyId"],
                               CustomerId = (Guid)dr["CustomerId"],
                               Url = dr["Url"].ToString(),
                               UserName = dr["UserName"].ToString(),
                               Password = dr["Password"].ToString()
                           }).ToList().FirstOrDefault();
            return CentralList;
        }
        public List<CustomerView> GetCustomerViewListByCompanyIdandCustomerId(Guid companyID, Guid customerID)
        {
            DataTable dt = _CustomerApiSettingDataAccess.GetCustomerApiCentralIdByCompanyIdandCustomerId(companyID, customerID);
            List<CustomerView> viewList = new List<CustomerView>();
            viewList = (from DataRow dr in dt.Rows
                        select new CustomerView()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            CompanyId = (Guid)dr["CompanyId"],
                            CustomerId = (Guid)dr["CustomerId"],
                            CustomerViewName = dr["CustomerViewName"].ToString(),
                            LastVistited = dr["LastVistited"] != DBNull.Value ? Convert.ToDateTime(dr["LastVistited"]) : new DateTime()
                        }).ToList();
            return viewList;
        }
        public List<CustomerSystemNo> GetAllCustomerNUmberByCompanyId(Guid companyID, String KeyVal)
        {
            DataTable dt = _CustomerSystemNoDataAccess.GetAllCustomerNUmberByCompanyId(companyID, KeyVal);
            List<CustomerSystemNo> viewList = new List<CustomerSystemNo>();
            viewList = (from DataRow dr in dt.Rows
                        select new CustomerSystemNo()
                        {
                            //Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            //CompanyId = (Guid)dr["CompanyId"],
                            CustomerNo = dr["CustomerNo"].ToString(),
                            //IsUsed = dr["IsUsed"] != DBNull.Value ? Convert.ToBoolean(dr["IsUsed"]) : false,
                            //IsReserved = dr["IsReserved"] != DBNull.Value ? Convert.ToBoolean(dr["IsReserved"]) : false,
                            //GenerateDate = dr["GenerateDate"] != DBNull.Value ? Convert.ToDateTime(dr["GenerateDate"]) : new DateTime(),
                            //ReserveDate = dr["ReserveDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReserveDate"]) : new DateTime(),
                            //UsedDate = dr["UsedDate"] != DBNull.Value ? Convert.ToDateTime(dr["UsedDate"]) : new DateTime(),
                        }).ToList();
            return viewList;
        }
        public Customer GetDuplicateCustomer(string type, string value, int CustomerId)
        {
            DataTable dt = _CustomerDataAccess.GetDuplicateCustomer(type, value, CustomerId);
            Customer customer = new Customer();
            customer = (from DataRow dr in dt.Rows
                        select new Customer()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            FullName = dr["Name"].ToString()
                        }).FirstOrDefault();
            return customer;
        }
        public Customer GetAllCustomerBillInfoByCompanyId(Guid CompanyID, Guid CustomerID)
        {
            DataTable dt = _CustomerDataAccess.GetAllCustomerBillInfoByCompanyId(CompanyID, CustomerID);
            Customer BillList = new Customer();
            BillList = (from DataRow dr in dt.Rows
                        select new Customer()
                        {
                            Id = dr["CusID"] != DBNull.Value ? Convert.ToInt32(dr["CusID"]) : 0,
                            BillAmount = dr["BillAmount"] != DBNull.Value ? Convert.ToDouble(dr["BillAmount"]) : 0,
                            PaymentMethod = dr["PaymentMethod"].ToString(),
                            BillCycle = dr["BillCycle"].ToString(),
                            BillDay = dr["BillDay"] != DBNull.Value ? Convert.ToInt32(dr["BillDay"]) : 0,
                            BillNotes = dr["BillNotes"].ToString(),
                            FirstBilling = dr["FirstBilling"] != DBNull.Value ? Convert.ToDateTime(dr["FirstBilling"]) : new DateTime(),
                            ContractTeam = dr["ContractTeam"].ToString(),
                            CreditScore = dr["CreditScore"].ToString(),
                            CustomerFunded = dr["CustomerFunded"] != DBNull.Value ? Convert.ToBoolean(dr["CustomerFunded"]) : false,
                            MonthlyMonitoringFee = dr["MonthlyMonitoringFee"].ToString(),
                            FundingCompany = dr["FundingCompany"].ToString(),
                            Maintenance = dr["Maintenance"] != DBNull.Value ? Convert.ToBoolean(dr["Maintenance"]) : false,
                            BillOutStanding = dr["BillOutStanding"] != DBNull.Value ? Convert.ToDouble(dr["BillOutStanding"]) : 0.0,
                            BillTax = dr["BillTax"] != DBNull.Value ? Convert.ToBoolean(dr["BillTax"]) : false,
                            PayAccountName = dr["PayAccountName"].ToString(),
                            PayCardNumber = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["PayCardNumber"].ToString()),
                            PayCardExpireDate = dr["PayCardExpireDate"].ToString(),
                            PayBAccountType = dr["PayBAccountType"].ToString(),
                            PayAccountNo = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["PayAccountNo"].ToString()),
                            PayRoutingNo = dr["PayRoutingNo"].ToString(),
                            AuthorizeRefId = dr["AuthorizeRefId"].ToString(),
                            SubscriptionStatus = dr["SubscriptionStatus"].ToString(),
                        }).FirstOrDefault();
            return BillList;
        }
        //[Shariful-26-9-19]
        public CustomerInfoWithCompany GetCustomerInfoWithCompanyByCustomerId(Guid Customerid)
        {
            DataTable dt = _CustomerDataAccess.GetCustomerInfoWithCompanyByCustomerId(Customerid);
            CustomerInfoWithCompany CusInsp = new CustomerInfoWithCompany();
            CusInsp = (from DataRow dr in dt.Rows
                       select new CustomerInfoWithCompany()
                       {
                           CusId= dr["CusId"] != DBNull.Value ? Convert.ToInt32(dr["CusId"]) : 0,
                           customerFirstName =dr["FirstName"].ToString(),
                           customerLastName= dr["LastName"].ToString(),
                           customerAddress= dr["CustomerAddress"].ToString(),
                           customerStreet= dr["CustomerStreet"].ToString(),
                           customerCity= dr["CustomerCity"].ToString(),
                           customerState= dr["CustomerState"].ToString(),
                           customerZipCode= dr["CustomerZipCode"].ToString(),
                           customerEmail= dr["CustomerEmailAddress"].ToString(),
                           customerPrimaryPhone= dr["PrimaryPhone"].ToString(),
                           customerSecondaryPhone= dr["SecondaryPhone"].ToString(),
                           companyAddress= dr["CompanyAddress"].ToString(),
                           companyName = dr["CompanyName"].ToString(),
                           companyLogo= dr["CompanyLogo"].ToString(),
                           companyCity= dr["CompanyCity"].ToString(),
                           companyStreet= dr["CompanyStreet"].ToString(),
                           companyState= dr["CompanyState"].ToString(),
                           companyZipCode= dr["CompanyZipCode"].ToString(),
                           companyEmail= dr["CompanyEmailAddress"].ToString(),
                           companyFax= dr["Fax"].ToString(),
                           companyPhone= dr["Phone"].ToString(),
                           companyWebsite= dr["Website"].ToString()
                       }).ToList().FirstOrDefault();
            return CusInsp;

        }
        //[~Shariful-26-9-19]
        public List<MMR> GetLeadMMRValueListByCompanyId(Guid CompanyId, int max, int min)
        {
            DataTable dt = _CustomerDataAccess.GetMMRValueListByCompanyId(CompanyId, max, min);
            List<MMR> MMRList = new List<MMR>();
            MMRList = (from DataRow dr in dt.Rows
                       select new MMR()
                       {
                           Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                           CompanyId = (Guid)dr["CompanyId"],
                           Name = dr["Name"].ToString(),
                           Value = dr["Value"] != DBNull.Value ? Convert.ToDouble(dr["Value"]) : 0.0
                       }).ToList();
            return MMRList;
        }

        public List<MMR> GetMMRValueListByCompanyId(Guid CompanyId)
        {
            return _MMRDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", CompanyId)).ToList();
        }
        public List<ActivationFee> GetActivationFeeByCompanyId(Guid CompanyId)
        {
            return _ActivationFeeDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", CompanyId)).ToList();
        }
        public List<CustomerNoPrefix> GetAllCustomerSystemNoPrefixByCompanyId(Guid CustomerId)
        {
            return _CustomerNoPrefixDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", CustomerId)).ToList();
        }
        public List<Announcement> GetAllAnnouncementByCurrentDate()
        {
            string query = "GetDate() between StartTime and EndTime";
            return _AnnouncementDataAccess.GetByQuery(query);
        }
        public List<Announcement> GetAllAnnouncement()
        {
            return _AnnouncementDataAccess.GetAll();
        }
        public Announcement GetAnnouncementById(int id)
        {
            return _AnnouncementDataAccess.Get(id);
        }
        public long UpdateAnnouncement(Announcement announcement)
        {
            return _AnnouncementDataAccess.Update(announcement);
        }
        public long DeleteAnnouncement(int Id)
        {
            return _AnnouncementDataAccess.Delete(Id);
        }
        public CustomerNoPrefix GetCustomerNoPrefixById(int id)
        {
            return _CustomerNoPrefixDataAccess.Get(id);
        }
        public bool CheckNoPrefixExistUsingCompanyIdAndName(Guid CustomerId, string Name)
        {
            var result = _CustomerNoPrefixDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and Name ='{1}'", CustomerId, Name)).FirstOrDefault();
            if (result != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool InsertCustomerNoPrefix(CustomerNoPrefix customerNoPrefix)
        {
            return _CustomerNoPrefixDataAccess.Insert(customerNoPrefix) > 0;
        }
        public bool UpdateCustomerNoPrefix(CustomerNoPrefix customerNoPrefix)
        {
            return _CustomerNoPrefixDataAccess.Update(customerNoPrefix) > 0;
        }
        public bool DeleteCustomerNoPrefix(int Id)
        {
            return _CustomerNoPrefixDataAccess.Delete(Id) > 0;
        }

        public List<Customer> GetAllCustomerWithSystemNo()
        {
            return _CustomerDataAccess.GetAllCustomerWithSystemNo();
        }
        public List<Customer> GetAllCustomerByCompanyId(Guid companyid)
        {
            return _CustomerDataAccess.GetAllCustomerByCompanyId(companyid);
        }
        public Customer GetAllCustomerByCustomerId(Guid customerid)
        {
            return _CustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerid)).FirstOrDefault();
        }
        public List<Customer> GetAllLeadByCompanyId(Guid companyid)
        {
            return _CustomerDataAccess.GetAllLeadByCompanyId(companyid);
        }
        public List<Customer> GetAllLeadByCompanyIdByCustomerStatus(Guid companyid)
        {
            return _CustomerDataAccess.GetAllLeadByCompanyIdByCustomerStatus(companyid);
        }
        public List<Customer> GetAllResposiblePersonByCompanyIdandCustomerId(Guid customerid)
        {
            return _CustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerid));
        }

        public Customer GetAllAccountActivityByCustomerId(Guid customerid)
        {
            DataTable dt = _CustomerDataAccess.GetAllAccountActivityByCustomerId(customerid);
            Customer ActivityList = new Customer();
            ActivityList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                IsFireAccount = dr["IsFireAccount"] != DBNull.Value ? Convert.ToBoolean(dr["IsFireAccount"]) : false,
                                SalesDate = dr["SalesDate"] != DBNull.Value ? Convert.ToDateTime(dr["SalesDate"]) : new DateTime(),
                                InstallDate = dr["InstallDate"] != DBNull.Value ? Convert.ToDateTime(dr["InstallDate"]) : new DateTime(),
                                CutInDate = dr["CutInDate"] != DBNull.Value ? Convert.ToDateTime(dr["CutInDate"]) : new DateTime(),
                                Installer = dr["Installer"].ToString(),
                                Soldby = dr["Soldby"].ToString(),
                                FundingDate = dr["FundingDate"] != DBNull.Value ? Convert.ToDateTime(dr["FundingDate"]) : new DateTime(),
                                QA1 = dr["QA1"].ToString(),
                                QA2 = dr["QA2"].ToString(),
                                QA1Date = dr["QA1Date"] != DBNull.Value ? Convert.ToDateTime(dr["QA1Date"]) : new DateTime(),
                                QA2Date = dr["QA2Date"] != DBNull.Value ? Convert.ToDateTime(dr["QA2Date"]) : new DateTime(),
                                InstallerName = dr["InstallerName"].ToString(),
                                SellerName = dr["SellerName"].ToString(),
                                QualityAssurance1 = dr["QualityAssurance1"].ToString(),
                                QualityAssurance2 = dr["QualityAssurance2"].ToString(),
                                CreatedByVal = dr["CreatedByVal"].ToString(),
                                JoinDate = dr["JoinDate"] != DBNull.Value ? Convert.ToDateTime(dr["JoinDate"]) : new DateTime()
                            }).ToList().FirstOrDefault();
            return ActivityList;
        }

        public List<Customer> GetAllCustomer()
        {
            return _CustomerDataAccess.GetAll();
        }

        public List<CustomerAppointmentEquipment> IsLeadAppointmentEquipmentExistCheck(Guid AppointmentId)
        {

            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetAllLeadInstalledEquipmentsByAppointmentId(AppointmentId);

            List<CustomerAppointmentEquipment> viewList = new List<CustomerAppointmentEquipment>();
            viewList = (from DataRow dr in dt.Rows
                        select new CustomerAppointmentEquipment()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            AppointmentId = dr["AppointmentId"] != DBNull.Value ? (Guid)(dr["AppointmentId"]) : new Guid(),
                            EquipmentId = dr["EquipmentId"] != DBNull.Value ? (Guid)(dr["EquipmentId"]) : new Guid(),
                            Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                            UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0.0,
                            TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0.0,
                            CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                            CreatedBy = dr["CreatedBy"].ToString(),
                            EquipmentOldPrice = dr["EquipmentOldPrice"] != DBNull.Value ? Convert.ToDouble(dr["EquipmentOldPrice"]) : 0.0,
                            EquipmentServiceName = dr["EquipmentServiceName"].ToString(),
                        }).ToList();
            return viewList;


            //return _CustomerAppointmentEquipmentDataAccess.GetByQuery(string.Format("AppointmentId = '{0}'", AppointmentId));
        }

        public List<Customer> IsCustomerAlarmIdExistCheck(int AlarmRefId)
        {

            DataTable dt = _CustomerDataAccess.IsCustomerAlarmIdExistCheck(AlarmRefId);

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
            //return _CustomerAppointmentEquipmentDataAccess.GetByQuery(string.Format("AppointmentId = '{0}'", AppointmentId));
        }

        public List<Customer> IsCustomerUccExistCheck(string UccRefId)
        {

            DataTable dt = _CustomerDataAccess.IsCustomerUccExistCheck(UccRefId);

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
            //return _CustomerAppointmentEquipmentDataAccess.GetByQuery(string.Format("AppointmentId = '{0}'", AppointmentId));
        }

        public List<CustomerPackageService> IsLeadAppointmentServiceExistCheckCustomerPackageEqp(Guid CustomerId, Guid CompanyId)
        {

            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetAllLeadInstalledServiceByAppointmentIdCustomerPackageEqp(CustomerId, CompanyId);

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
                            Manufacturer = dr["Manufacturer"].ToString(),
                            Location = dr["Location"].ToString(),
                            Type = dr["Type"].ToString(),
                            Model = dr["Model"].ToString(),
                            Finish = dr["Finish"].ToString(),
                            Capacity = dr["Capacity"].ToString(),
                            IsARBEnabled = dr["IsARBEnabled"] != DBNull.Value ? Convert.ToBoolean(dr["IsARBEnabled"]) : false,
                            IsNonCommissionable = dr["IsNonCommissionable"] != DBNull.Value ? Convert.ToBoolean(dr["IsNonCommissionable"]) : false,
                            IsInvoice = dr["IsInvoice"] != DBNull.Value ? Convert.ToBoolean(dr["IsInvoice"]) : false,
                        }).ToList();
            return viewList;

        }

        public List<CustomerPackageEqp> IsLeadAppointmentEquipmentExistCheckCustomerPackageEqp(Guid CustomerId, Guid CompanyId)
        {

            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetAllLeadInstalledEquipmentsByAppointmentIdCustomerPackageEqp(CustomerId, CompanyId);

            List<CustomerPackageEqp> viewList = new List<CustomerPackageEqp>();
            viewList = (from DataRow dr in dt.Rows
                        select new CustomerPackageEqp()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            CompanyId = dr["CompanyId"] != DBNull.Value ? (Guid)(dr["CompanyId"]) : new Guid(),
                            CustomerId = dr["CustomerId"] != DBNull.Value ? (Guid)(dr["CustomerId"]) : new Guid(),
                            PackageId = dr["PackageId"] != DBNull.Value ? (Guid)(dr["PackageId"]) : new Guid(),
                            EquipmentId = dr["EquipmentId"] != DBNull.Value ? (Guid)(dr["EquipmentId"]) : new Guid(),
                            IsIncluded = dr["IsIncluded"] != DBNull.Value ? Convert.ToBoolean(dr["IsIncluded"]) : false,
                            IsDevice = dr["IsDevice"] != DBNull.Value ? Convert.ToBoolean(dr["IsDevice"]) : false,
                            IsOptionalEqp = dr["IsOptionalEqp"] != DBNull.Value ? Convert.ToBoolean(dr["IsOptionalEqp"]) : false,
                            Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                            UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0.0,
                            DiscountUnitPricce = dr["DiscountUnitPricce"] != DBNull.Value ? Convert.ToDouble(dr["DiscountUnitPricce"]) : 0.0,
                            DiscountPckage = dr["DiscountPckage"] != DBNull.Value ? Convert.ToDouble(dr["DiscountPckage"]) : 0.0,
                            Total = dr["Total"] != DBNull.Value ? Convert.ToDouble(dr["Total"]) : 0.0,
                            EquipmentServiceName = dr["EquipmentServiceName"].ToString(),
                            SKU = dr["SKU"].ToString(),
                            Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0,
                            ServiceId = dr["ServiceId"] != DBNull.Value ? (Guid)(dr["ServiceId"]) : new Guid(),
                            IsNonCommissionable = dr["IsNonCommissionable"] != DBNull.Value ? Convert.ToBoolean(dr["IsNonCommissionable"]) : false,
                            IsInvoice = dr["IsInvoice"] != DBNull.Value ? Convert.ToBoolean(dr["IsInvoice"]) : false,
                        }).ToList();
            return viewList;


            //return _CustomerAppointmentEquipmentDataAccess.GetByQuery(string.Format("AppointmentId = '{0}'", AppointmentId));
        }

        public List<ServiceFee> GetAllServiceFeeValueByCompanyId(Guid Companyid)
        {
            return _ServiceFeeDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", Companyid));
        }

        public List<ActivationFee> GetAllActivationFeeValueByCompanyId(Guid Companyid)
        {
            return _ActivationFeeDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", Companyid));
        }

        public List<AccountHolder> GetAllAccountHolderValueByCompanyId(Guid Companyid)
        {
            return _AccountHolderDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", Companyid));
        }

        public Customer GetLeadIdByCompanyIdAndCustomerId(Guid companyid, int id)
        {
            DataTable dt = _CustomerDataAccess.GetLeadIdByCompanyIdAndCustomerId(companyid, id);
            Customer ActivityList = new Customer();
            ActivityList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                //ContactName = dr["ContactName"].ToString(),
                                AccountNo = dr["AccountNo"].ToString(),
                                EmailAddress = dr["EmailAddress"].ToString(),
                                Address = dr["Address"].ToString(),
                                BusinessName = dr["BusinessName"].ToString(),
                                CallingTime = dr["CallingTime"].ToString(),
                                CellNo = dr["CellNo"].ToString(),
                                CellularBackup = (dr["CellularBackup"] != DBNull.Value ? Convert.ToBoolean(dr["CellularBackup"]) : false),
                                City = dr["City"].ToString(),
                                ContractTeam = dr["ContractTeam"].ToString(),
                                Country = dr["Country"].ToString(),
                                CreditScore = dr["CreditScore"].ToString(),
                                CustomerFunded = (dr["CustomerFunded"] != DBNull.Value ? Convert.ToBoolean(dr["CustomerFunded"]) : false),
                                CustomerId = (Guid)dr["CustomerId"],
                                Fax = dr["Fax"].ToString(),
                                FirstName = dr["FirstName"].ToString(),
                                FundingCompany = dr["FundingCompany"].ToString(),
                                IsAlarmCom = dr["IsAlarmCom"] != DBNull.Value ? Convert.ToBoolean(dr["IsAlarmCom"]) : false,
                                LastName = dr["LastName"].ToString(),
                                LeadSource = dr["LeadSource"].ToString(),
                                Maintenance = dr["Maintenance"] != DBNull.Value ? Convert.ToBoolean(dr["Maintenance"]) : false,
                                MonthlyMonitoringFee = dr["MonthlyMonitoringFee"].ToString(),
                                Note = dr["Note"].ToString(),
                                PrimaryPhone = dr["PrimaryPhone"].ToString(),
                                SecondaryPhone = dr["SecondaryPhone"].ToString(),
                                SSN = dr["SSN"].ToString(),
                                State = dr["State"].ToString(),
                                Street = dr["Street"].ToString(),
                                Title = dr["Title"].ToString(),
                                Type = dr["Type"].ToString(),
                                ZipCode = dr["ZipCode"].ToString(),
                                DateofBirth = dr["DateofBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DateofBirth"]) : new DateTime(),
                                JoinDate = dr["JoinDate"] != DBNull.Value ? Convert.ToDateTime(dr["JoinDate"]) : new DateTime(),
                                ReminderDate = dr["ReminderDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReminderDate"]) : new DateTime(),
                            }).ToList().FirstOrDefault();
            return ActivityList;
        }

        public Customer GetLeadByPaymentinfoID(int id)
        {
            DataTable dt = _CustomerDataAccess.GetLeadByPaymentinfoID(id);
            Customer ActivityList = new Customer();
            ActivityList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                //ContactName = dr["ContactName"].ToString(),
                                AccountNo = dr["AccountNo"].ToString(),
                                EmailAddress = dr["EmailAddress"].ToString(),
                                Address = dr["Address"].ToString(),
                                BusinessName = dr["BusinessName"].ToString(),
                                CallingTime = dr["CallingTime"].ToString(),
                                CellNo = dr["CellNo"].ToString(),
                                CellularBackup = (dr["CellularBackup"] != DBNull.Value ? Convert.ToBoolean(dr["CellularBackup"]) : false),
                                City = dr["City"].ToString(),
                                ContractTeam = dr["ContractTeam"].ToString(),
                                Country = dr["Country"].ToString(),
                                CreditScore = dr["CreditScore"].ToString(),
                                CustomerFunded = (dr["CustomerFunded"] != DBNull.Value ? Convert.ToBoolean(dr["CustomerFunded"]) : false),
                                CustomerId = (Guid)dr["CustomerId"],
                                Fax = dr["Fax"].ToString(),
                                FirstName = dr["FirstName"].ToString(),
                                FundingCompany = dr["FundingCompany"].ToString(),
                                IsAlarmCom = dr["IsAlarmCom"] != DBNull.Value ? Convert.ToBoolean(dr["IsAlarmCom"]) : false,
                                LastName = dr["LastName"].ToString(),
                                LeadSource = dr["LeadSource"].ToString(),
                                Maintenance = dr["Maintenance"] != DBNull.Value ? Convert.ToBoolean(dr["Maintenance"]) : false,
                                MonthlyMonitoringFee = dr["MonthlyMonitoringFee"].ToString(),
                                Note = dr["Note"].ToString(),
                                PrimaryPhone = dr["PrimaryPhone"].ToString(),
                                SecondaryPhone = dr["SecondaryPhone"].ToString(),
                                SSN = dr["SSN"].ToString(),
                                State = dr["State"].ToString(),
                                Street = dr["Street"].ToString(),
                                Title = dr["Title"].ToString(),
                                Type = dr["Type"].ToString(),
                                ZipCode = dr["ZipCode"].ToString(),
                                DateofBirth = dr["DateofBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DateofBirth"]) : new DateTime(),
                                JoinDate = dr["JoinDate"] != DBNull.Value ? Convert.ToDateTime(dr["JoinDate"]) : new DateTime(),
                                ReminderDate = dr["ReminderDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReminderDate"]) : new DateTime(),
                                BillMethod = dr["BillMethod"].ToString()
                            }).ToList().FirstOrDefault();
            return ActivityList;
        }

        public Customer GetLeadNameByLeadId(int id)
        {
            DataTable dt = _CustomerDataAccess.GetLeadNameByLeadId(id);
            Customer Leadnum = new Customer();
            Leadnum = (from DataRow dr in dt.Rows
                       select new Customer()
                       {
                           LeadName = dr["LeadName"].ToString()
                       }).FirstOrDefault();
            return Leadnum;
        }

        public string GetCustomerQA1StatusByCompanyIdAndCustomerId(Guid Companyid, Guid CustomerId)
        {
            string result = "";
            DataTable dt = _CustomerDataAccess.GetCustomerQA1StatusByCompanyIdAndCustomerId(Companyid, CustomerId);
            result = dt.Rows[0]["QA1COunt"].ToString();
            return result;
        }

        public string GetQA1PercentageValueByCompanyId(Guid Companyid)
        {
            string result = "";
            DataTable dt = _CustomerDataAccess.GetQA1PercentageValueByCompanyId(Companyid);
            result = dt.Rows[0]["GlobalSettingPercentage"].ToString();
            return result;
        }

        public bool GetCustomerQA2StatusByCompanyIdAndCustomerId(Guid Companyid, Guid CustomerId)
        {
            bool result = false;
            DataTable dt = _CustomerDataAccess.GetCustomerQA2StatusByCompanyIdAndCustomerId(Companyid, CustomerId);
            result = Convert.ToInt32(dt.Rows[0]["QA2COunt"]) > 0;
            return result;
        }

        public bool GetQATechCallValueBycompanyId(Guid companyid, Guid customerid)
        {
            bool res = false;
            DataTable dt = _GlobalSettingDataAccess.GetQATechCallValueBycompanyId(companyid, customerid);
            if (dt != null)
                res = Convert.ToBoolean(dt.Rows[0]["Techval"]);
            return res;
        }

        public Customer GetEmployeeIdByCompanyIdandCustomerId(Guid companyid, int customerid)
        {
            DataTable dt = _CustomerDataAccess.GetEmployeeIdByCompanyIdandCustomerId(companyid, customerid);
            Customer Leadnum = new Customer();
            Leadnum = (from DataRow dr in dt.Rows
                       select new Customer()
                       {
                           InstallerId = dr["InstallerId"].ToString()
                       }).FirstOrDefault();
            return Leadnum;
        }

        public Customer GetLeadIdForCustomerSignatureByCustomerId(Guid cusid)
        {
            return _CustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", cusid)).FirstOrDefault();
        }

        public CustomerSystemInfo GetZoneInfoByCustomerId(Guid customerid)
        {
            return _CustomerSystemInfoDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerid)).FirstOrDefault();
        }
        public bool EmergencyContactExists(Guid customerId, Guid CompanyId, string contactNumber, int Id)
        {
            string sqlstr = "CustomerId = '{0}' and CompanyId = '{1}' and Phone = '{2}'";
            if (Id > 0)
            {
                sqlstr += " and Id != {3}";
            }

            return _EmergencyContactDataAccess.GetByQuery(string.Format(sqlstr, customerId, CompanyId, contactNumber, Id)).Count() > 0;
        }

        public bool GetLeadSetupDetailByLeadId(Guid id)
        {
            return _CustomerCompanyDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", id)).FirstOrDefault().IsLead;
        }

        public long InsertCustomerCancel(CustomerCancel cc)
        {
            return _CustomerCancelDataAccess.Insert(cc);
        }

        public bool UpdateCustomerCancel(CustomerCancel cc)
        {
            return _CustomerCancelDataAccess.Update(cc) > 0;
        }

        public CustomerCancel GetCustomerCancelByCompanyIdCustomerIdAndEmployeeId(Guid comid, Guid cusid, Guid empid)
        {
            return _CustomerCancelDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and CustomerId = '{1}' and EmployeeId = '{2}'", comid, cusid, empid)).FirstOrDefault();
        }

        public CustomerCancel GetCustomerCancelDateByCustomerIdAndCompanyId(Guid companyid, Guid customerid)
        {
            DataTable dt = _CustomerCancelDataAccess.GetCustomerCancelDateByCustomerIdAndCompanyId(companyid, customerid);
            List<CustomerCancel> viewList = new List<CustomerCancel>();
            viewList = (from DataRow dr in dt.Rows
                        select new CustomerCancel()
                        {
                            CancelDatet = dr["CancelDatet"] != DBNull.Value ? Convert.ToDateTime(dr["CancelDatet"]) : new DateTime(),
                            CancelReason = dr["CancelReason"].ToString(),
                            EmpName = dr["EmpName"].ToString()
                        }).ToList();
            return viewList.FirstOrDefault();
        }

        public FinalCustomerSetupData GetAllFinalCustomerSetupDataByCompanyIdAndCustomerId(Guid companyid, Guid customerid)
        {
            DataSet ds = _CustomerDataAccess.GetAllFinalCustomerSetupDataByCompanyIdAndCustomerId(companyid, customerid);
            FinalCustomerSetupData model = new FinalCustomerSetupData();
            model.ListPackageCustomer = (from DataRow dr in ds.Tables[0].Rows
                                         select new PackageCustomer()
                                         {
                                             PackageCustomerId = dr["PackageCustomerId"] != DBNull.Value ? Convert.ToInt32(dr["PackageCustomerId"]) : 0
                                         }).ToList();
            model.ListCustomerAppointmentEquipment = (from DataRow dr in ds.Tables[1].Rows
                                                      select new CustomerAppointmentEquipment()
                                                      {
                                                          AppointmentEquipmentId = dr["AppointmentEquipmentId"] != DBNull.Value ? Convert.ToInt32(dr["AppointmentEquipmentId"]) : 0
                                                      }).ToList();
            model.ListPaymentInfoCustomer = (from DataRow dr in ds.Tables[2].Rows
                                             select new PaymentInfoCustomer()
                                             {
                                                 PaymentCustomerId = dr["PaymentCustomerId"] != DBNull.Value ? Convert.ToInt32(dr["PaymentCustomerId"]) : 0
                                             }).ToList();
            model.ListEmergencyContact = (from DataRow dr in ds.Tables[3].Rows
                                          select new EmergencyContact()
                                          {
                                              EmergencyId = dr["EmergencyId"] != DBNull.Value ? Convert.ToInt32(dr["EmergencyId"]) : 0
                                          }).ToList();
            return model;
        }

        public FinalCustomerSetupData GetAllFinalSmartCustomerSetupDataByCompanyIdAndCustomerId(Guid companyid, Guid customerid)
        {
            DataSet ds = _CustomerDataAccess.GetAllFinalSmartCustomerSetupDataByCompanyIdAndCustomerId(companyid, customerid);
            FinalCustomerSetupData model = new FinalCustomerSetupData();
            model.ListPackageCustomer = (from DataRow dr in ds.Tables[0].Rows
                                         select new PackageCustomer()
                                         {
                                             PackageCustomerId = dr["PackageCustomerId"] != DBNull.Value ? Convert.ToInt32(dr["PackageCustomerId"]) : 0
                                         }).ToList();
            model.ListCustomerAppointmentEquipment = (from DataRow dr in ds.Tables[1].Rows
                                                      select new CustomerAppointmentEquipment()
                                                      {
                                                          AppointmentEquipmentId = dr["AppointmentEquipmentId"] != DBNull.Value ? Convert.ToInt32(dr["AppointmentEquipmentId"]) : 0
                                                      }).ToList();
            model.ListPaymentInfoCustomer = (from DataRow dr in ds.Tables[2].Rows
                                             select new PaymentInfoCustomer()
                                             {
                                                 PaymentCustomerId = dr["PaymentCustomerId"] != DBNull.Value ? Convert.ToInt32(dr["PaymentCustomerId"]) : 0,
                                                 PaymentFor = dr["PaymentFor"].ToString()
                                             }).ToList();
            model.ListEmergencyContact = (from DataRow dr in ds.Tables[3].Rows
                                          select new EmergencyContact()
                                          {
                                              EmergencyId = dr["EmergencyId"] != DBNull.Value ? Convert.ToInt32(dr["EmergencyId"]) : 0
                                          }).ToList();
            return model;
        }

        public CustomerSpouse GetSpouseById(int value)
        {
            return _CustomerSpouseDataAccess.Get(value);
        }
        public CustomerSpouse GetSpouseByCustomerIdAndComapnyId(Guid cusid, Guid comid)
        {
            return _CustomerSpouseDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}'", cusid, comid)).FirstOrDefault();
        }
        public void DeleteSpouseById(int id)
        {
            _CustomerSpouseDataAccess.Delete(id);
        }
        public long InsertCustomerSpouse(CustomerSpouse cs)
        {
            return _CustomerSpouseDataAccess.Insert(cs);
        }

        public bool UpdateCustomerSpouse(CustomerSpouse cs)
        {
            return _CustomerSpouseDataAccess.Update(cs) > 0;
        }
        public void DeleteSpouse(int id)
        {
            _CustomerSpouseDataAccess.Delete(id);
        }

        public List<ReferingCustomer> GetReferringCustomerListByCustomerId(Guid customerId)
        {
            return _CustomerDataAccess.GetReferringCustomerListByCustomerId(customerId);
        }
        public List<ChildCustomer> GetChildCustomerListByCustomerId(Guid customerId)
        {
            return _CustomerDataAccess.GetChildCustomerListByCustomerId(customerId);
        }
        public ForteCustomerCreate GetCustomerTokenForForte(ForteCustomer forteCust, ForteCustomerService forteCustomer)
        {


            try
            {
                ForteCustomerCreate response = new ForteCustomerCreate();
                response = forteCustomer.Create(forteCust);
                if (response.Massege.Contains("Successfully"))
                {
                    var finalResponse = response.Massege.Split('|');
                    int index = finalResponse.Length - 1;
                    FortePaymentResponse forteResponse = new FortePaymentResponse();
                    forteResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FortePaymentResponse>(finalResponse[index]);
                    response.CustomerToken = forteResponse.customer_token;
                    response.Result = true;

                }
                else
                {
                    var finalResponse = response.Massege.Split('#');
                    int index = finalResponse.Length - 1;
                    FortePaymentResponse forteResponse = new FortePaymentResponse();
                    forteResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FortePaymentResponse>(finalResponse[index]);
                    response.ErrorMessage = forteResponse.response.response_desc;
                    response.Result = false;

                }
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public ForteCustomerCreate UpdateCustomerForForte(UpdateCustomerWithPaymentToken forteCust, ForteCustomerService forteCustomer)
        {


            try
            {
                ForteCustomerCreate response = new ForteCustomerCreate();
                response = forteCustomer.Update(forteCust);
                if (response.Massege.Contains("Successfully"))
                {
                    var finalResponse = response.Massege.Split('|');
                    int index = finalResponse.Length - 1;
                    FortePaymentResponse forteResponse = new FortePaymentResponse();
                    forteResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FortePaymentResponse>(finalResponse[index]);
                    response.PaymentToken = forteResponse.paymethod_token;
                    response.Result = true;

                }
                else
                {
                    var finalResponse = response.Massege.Split('#');
                    int index = finalResponse.Length - 1;
                    FortePaymentResponse forteResponse = new FortePaymentResponse();
                    forteResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FortePaymentResponse>(finalResponse[index]);
                    response.ErrorMessage = forteResponse.response.response_desc;
                    response.Result = false;

                }
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public FortePaymentCreate GetPaymentTokenForForte(FortePaymethod fortePay, FortePaymethodsService fortePayService)
        {

            try
            {
                FortePaymentCreate response = fortePayService.Create(fortePay);
                if (response.Massege.Contains("Successfully"))
                {
                    var finalResponse = response.Massege.Split('|');
                    int index = finalResponse.Length - 1;
                    FortePaymentResponse forteResponse = new FortePaymentResponse();
                    forteResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FortePaymentResponse>(finalResponse[index]);
                    response.PaymentToken = forteResponse.paymethod_token;
                    response.Result = true;
                }
                else
                {
                    var finalResponse = response.Massege.Split('#');
                    int index = finalResponse.Length - 1;
                    FortePaymentResponse forteResponse = new FortePaymentResponse();
                    forteResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FortePaymentResponse>(finalResponse[index]);
                    response.ErrorMessage = forteResponse.response.response_desc;
                    response.Result = false;
                }
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public FortePaymentCreate UpdatePaymentTokenForForte(FortePaymethod fortePay, FortePaymethodsService fortePayService)
        {

            string[] responseCustomerToken;
            string[] responselist;

            try
            {
                FortePaymentCreate response = fortePayService.Update(fortePay);
                if (response.Massege.Contains("Successfully"))
                {
                    responselist = response.Massege.Split(',');
                    responseCustomerToken = responselist[0].Split(':');
                    response.PaymentToken = responseCustomerToken[2].Trim('"');
                    response.Result = true;
                }
                else
                {
                    response.Result = false;
                }
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public ForteResponseDetails MakeScheduleForCustomer(ForteSchedule forteSchedule, ForteScheduleService forteScheduleService)
        {
            try
            {
                ForteResponseDetails response = new ForteResponseDetails();
                response = forteScheduleService.Create(forteSchedule);
                if (response.Massege.Contains("Successfully"))
                {
                    var finalResponse = response.Massege.Split('|');
                    int index = finalResponse.Length - 1;
                    FortePaymentResponse forteResponse = new FortePaymentResponse();
                    forteResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FortePaymentResponse>(finalResponse[index]);
                    response.ScheduleToken = forteResponse.schedule_id;
                    response.Massege = "Subscribed to forte successfully.";
                    response.Result = true;

                }
                else
                {
                    var finalResponse = response.Massege.Split('#');
                    int index = finalResponse.Length - 1;
                    FortePaymentResponse forteResponse = new FortePaymentResponse();
                    forteResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FortePaymentResponse>(finalResponse[index]);
                    response.ErrorMessage = forteResponse.response.response_desc;
                    response.Result = false;
                }
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public ForteResponseDetails UpdateScheduleForCustomer(ForteSchedule forteSchedule, ForteScheduleService forteScheduleService)
        {
            try
            {
                ForteResponseDetails response = new ForteResponseDetails();
                response = forteScheduleService.Update(forteSchedule);
                if (response.Massege.Contains("Successfully"))
                {
                    var finalResponse = response.Massege.Split('|');
                    int index = finalResponse.Length - 1;
                    FortePaymentResponse forteResponse = new FortePaymentResponse();
                    forteResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FortePaymentResponse>(finalResponse[index]);

                    response.ScheduleToken = forteResponse.schedule_id;
                    response.Massege = "Customer forte subscription updated successfully.";
                    response.Result = true;

                }
                else
                {
                    var finalResponse = response.Massege.Split('#');
                    int index = finalResponse.Length - 1;
                    FortePaymentResponse forteResponse = new FortePaymentResponse();
                    forteResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FortePaymentResponse>(finalResponse[index]);
                    response.ErrorMessage = forteResponse.response.response_desc;
                    response.Result = false;
                }
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public bool DeleteCustomerSchedule(ForteScheduleService forteScheduleService)
        {
            try
            {

                string response = forteScheduleService.Delete();
                if (response.Contains("Successfully"))
                {
                    return true;

                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public long InsertPaymentProfileCustomer(PaymentProfileCustomer ppc)
        {
            return _PaymentProfileCustomerDataAccess.Insert(ppc);
        }

        public List<PaymentProfileCustomer> GetAllPaymentProfileByCustomerId(Guid customerid, Guid companyid)
        {
            return _PaymentProfileCustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}'", customerid, companyid)).ToList();
        }
        public List<PaymentProfileCustomer> GetAllPaymentProfileByType(Guid customerid, Guid companyid, string type,bool AllowOnlyACHAndCC =false)
        {
            DataTable dt = _PaymentProfileCustomerDataAccess.GetAllPaymentProfileByType(companyid, customerid, type, AllowOnlyACHAndCC);
            List<PaymentProfileCustomer> PaymentProfileCustomerList = new List<PaymentProfileCustomer>();
            PaymentProfileCustomerList = (from DataRow dr in dt.Rows
                                          select new PaymentProfileCustomer()
                                          {
                                              PaymentInfoId = dr["PaymentInfoId"] != DBNull.Value ? Convert.ToInt32(dr["PaymentInfoId"]) : 0,
                                              Type = dr["Type"].ToString()
                                          }).ToList();
            return PaymentProfileCustomerList;
        }

        public PaymentProfileCustomer GetProfileByPaymentInfoId(int infoid)
        {
            return _PaymentProfileCustomerDataAccess.GetByQuery(string.Format("PaymentInfoId = {0}", infoid)).FirstOrDefault();
        }

        public LeadDetailTabCountModel GetLeadDetailTabCount(Guid companyid, Guid customerid)
        {
            DataSet ds = _CustomerDataAccess.GetLeadDetailTabCount(companyid, customerid);

            LeadDetailTabCountModel model = new LeadDetailTabCountModel();
            model.LeadEstimate = (from DataRow dr in ds.Tables[0].Rows
                                  select new Invoice()
                                  {
                                      LeadEstimateCount = dr["LeadEstimateCount"] != DBNull.Value ? Convert.ToInt32(dr["LeadEstimateCount"]) : 0,
                                  }).FirstOrDefault();
            model.LeadNote = (from DataRow dr in ds.Tables[1].Rows
                              select new CustomerNote()
                              {
                                  LeadNoteCount = dr["LeadNoteCount"] != DBNull.Value ? Convert.ToInt32(dr["LeadNoteCount"]) : 0,
                              }).FirstOrDefault();
            model.LeadCorrespondence = (from DataRow dr in ds.Tables[2].Rows
                                        select new LeadCorrespondence()
                                        {
                                            LeadCorresCount = dr["LeadCorresCount"] != DBNull.Value ? Convert.ToInt32(dr["LeadCorresCount"]) : 0,
                                        }).FirstOrDefault();
            model.LeadFile = (from DataRow dr in ds.Tables[3].Rows
                              select new CustomerFile()
                              {
                                  LeadFileCount = dr["LeadFileCount"] != DBNull.Value ? Convert.ToInt32(dr["LeadFileCount"]) : 0,
                              }).FirstOrDefault();
            model.Booking = (from DataRow dr in ds.Tables[4].Rows
                             select new Booking()
                             {
                                 BookingCount = dr["BookingCount"] != DBNull.Value ? Convert.ToInt32(dr["BookingCount"]) : 0,
                             }).FirstOrDefault();

            model.TicketCount = ds.Tables[5].Rows[0][0] != DBNull.Value ? Convert.ToInt32(ds.Tables[5].Rows[0][0]) : 0;

            return model;
        }

        public CustomerCompany GetCustomerCompanyByCompanyIdAndCustomerId(Guid companyid, Guid customerid)
        {
            return _CustomerCompanyDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and CustomerId = '{1}'", companyid, customerid)).FirstOrDefault();
        }

        public List<DelinquentCustomerModel> GetAllDelinquentCustomerByCompany(Guid companyId, DateTime? Start, DateTime? End, int pageno, int pagesize, string id, string searchtext, string unpaid)
        {
            DataTable dt = _CustomerDataAccess.GetAllDelinquentCustomerByCompany(companyId, Start, End, pageno, pagesize, id, searchtext, unpaid);
            List<DelinquentCustomerModel> propertyList = new List<DelinquentCustomerModel>();
            propertyList = (from DataRow dr in dt.Rows
                            select new DelinquentCustomerModel()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                CustomerId = (Guid)dr["CustomerId"],
                                CustomerName = dr["CustomerName"].ToString(),
                                PhoneNo = dr["PhoneNo"].ToString(),
                                Email = dr["Email"].ToString(),
                                Unpaid = dr["Unpaid"] != DBNull.Value ? Convert.ToDouble(dr["Unpaid"]) : 0.0,
                                Address = dr["Address"].ToString(),
                                Street = dr["Street"].ToString(),
                                ZipCode = dr["ZipCode"].ToString(),
                                City = dr["City"].ToString(),
                                State = dr["State"].ToString(),
                                StreetType = dr["StreetType"].ToString(),
                                Appartment = dr["Appartment"].ToString()
                                
                            }).ToList();
            return propertyList;
        }

        public List<DelinquentCustomerModel> GetAllTransferCustomerByCompany(Guid companyId, DateTime? Start, DateTime? End, int pageno, int pagesize, FilterReportModel filter)
        {
            DataTable dt = _CustomerDataAccess.GetAllTransferCustomerByCompany(companyId, Start, End, pageno, pagesize, filter);
            List<DelinquentCustomerModel> propertyList = new List<DelinquentCustomerModel>();
            propertyList = (from DataRow dr in dt.Rows
                            select new DelinquentCustomerModel()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                CustomerId = (Guid)dr["CustomerId"],
                                CustomerName = dr["CustomerName"].ToString(),
                                PhoneNo = dr["PhoneNo"].ToString(),
                                Email = dr["Email"].ToString(),
                                Address = dr["Address"].ToString(),
                                CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                Street = dr["Street"].ToString(),
                                ZipCode = dr["ZipCode"].ToString(),
                                City = dr["City"].ToString(),
                                State = dr["State"].ToString(),
                                StreetType = dr["StreetType"].ToString(),
                                Appartment = dr["Appartment"].ToString()
                            }).ToList();
            return propertyList;
        }

        public RecurringBillingCustomerModel GetCustomerRecurringBillingByCompanyId(Guid companyid, DateTime? start, DateTime? end, string billingmindate, string billingmaxdate)
        {
            DataSet ds = _CustomerDataAccess.GetCustomerRecurringBillingByCompanyId(companyid, start, end, billingmindate, billingmaxdate);
            RecurringBillingCustomerModel model = new RecurringBillingCustomerModel();
            model.ListCus = (from DataRow dr in ds.Tables[0].Rows
                             select new Customer()
                             {
                                 FirstBilling = dr["FirstBilling"] != DBNull.Value ? Convert.ToDateTime(dr["FirstBilling"]) : new DateTime(),
                                 ACHSubs = dr["ACHSubs"] != DBNull.Value ? Convert.ToInt32(dr["ACHSubs"]) : 0,
                                 ACHRMR = dr["ACHRMR"] != DBNull.Value ? Convert.ToDouble(dr["ACHRMR"]) : 0,
                                 ACHAmount = dr["ACHAmount"] != DBNull.Value ? Convert.ToDouble(dr["ACHAmount"]) : 0,
                                 CCSubs = dr["CCSubs"] != DBNull.Value ? Convert.ToInt32(dr["CCSubs"]) : 0,
                                 CCRMR = dr["CCRMR"] != DBNull.Value ? Convert.ToDouble(dr["CCRMR"]) : 0,
                                 CCAmount = dr["CCAmount"] != DBNull.Value ? Convert.ToDouble(dr["CCAmount"]) : 0,
                                 InvoiceSubs = dr["InvoiceSubs"] != DBNull.Value ? Convert.ToInt32(dr["InvoiceSubs"]) : 0,
                                 InvoiceRMR = dr["InvoiceRMR"] != DBNull.Value ? Convert.ToDouble(dr["InvoiceRMR"]) : 0,
                                 InvoiceAmount = dr["InvoiceAmount"] != DBNull.Value ? Convert.ToDouble(dr["InvoiceAmount"]) : 0,
                             }).ToList();
            model.TotalAutomaticCustomerCountACHModel = (from DataRow dr in ds.Tables[1].Rows
                                                         select new TotalAutomaticCustomerCountACHModel()
                                                         {
                                                             TotalAutomaticCustomerCountACH = dr["TotalAutomaticCustomerCountACH"] != DBNull.Value ? Convert.ToInt32(dr["TotalAutomaticCustomerCountACH"]) : 0
                                                         }).FirstOrDefault();
            model.TotalAutomaticCustomerCountCCModel = (from DataRow dr in ds.Tables[2].Rows
                                                        select new TotalAutomaticCustomerCountCCModel()
                                                        {
                                                            TotalAutomaticCustomerCountCC = dr["TotalAutomaticCustomerCountCC"] != DBNull.Value ? Convert.ToInt32(dr["TotalAutomaticCustomerCountCC"]) : 0
                                                        }).FirstOrDefault();
            model.TotalAutomaticCustomerCountInvoiceModel = (from DataRow dr in ds.Tables[3].Rows
                                                             select new TotalAutomaticCustomerCountInvoiceModel()
                                                             {
                                                                 TotalAutomaticCustomerCountInvoice = dr["TotalAutomaticCustomerCountInvoice"] != DBNull.Value ? Convert.ToInt32(dr["TotalAutomaticCustomerCountInvoice"]) : 0
                                                             }).FirstOrDefault();
            model.TotalCustomerRMRACHModel = (from DataRow dr in ds.Tables[4].Rows
                                              select new TotalCustomerRMRACHModel()
                                              {
                                                  TotalCustomerRMRACH = dr["TotalCustomerRMRACH"] != DBNull.Value ? Convert.ToDouble(dr["TotalCustomerRMRACH"]) : 0
                                              }).FirstOrDefault();
            model.TotalCustomerRMRCCModel = (from DataRow dr in ds.Tables[5].Rows
                                             select new TotalCustomerRMRCCModel()
                                             {
                                                 TotalCustomerRMRCC = dr["TotalCustomerRMRCC"] != DBNull.Value ? Convert.ToDouble(dr["TotalCustomerRMRCC"]) : 0
                                             }).FirstOrDefault();
            model.TotalCustomerRMRInvoiceModel = (from DataRow dr in ds.Tables[6].Rows
                                                  select new TotalCustomerRMRInvoiceModel()
                                                  {
                                                      TotalCustomerRMRInvoice = dr["TotalCustomerRMRInvoice"] != DBNull.Value ? Convert.ToDouble(dr["TotalCustomerRMRInvoice"]) : 0
                                                  }).FirstOrDefault();
            model.TotalCustomerBillAmountACHModel = (from DataRow dr in ds.Tables[7].Rows
                                                     select new TotalCustomerBillAmountACHModel()
                                                     {
                                                         TotalCustomerBillAmountACH = dr["TotalCustomerBillAmountACH"] != DBNull.Value ? Convert.ToDouble(dr["TotalCustomerBillAmountACH"]) : 0
                                                     }).FirstOrDefault();
            model.TotalCustomerBillAmountCCModel = (from DataRow dr in ds.Tables[8].Rows
                                                    select new TotalCustomerBillAmountCCModel()
                                                    {
                                                        TotalCustomerBillAmountCC = dr["TotalCustomerBillAmountCC"] != DBNull.Value ? Convert.ToDouble(dr["TotalCustomerBillAmountCC"]) : 0
                                                    }).FirstOrDefault();
            model.TotalCustomerBillAmountInvoiceModel = (from DataRow dr in ds.Tables[9].Rows
                                                         select new TotalCustomerBillAmountInvoiceModel()
                                                         {
                                                             TotalCustomerBillAmountInvoice = dr["TotalCustomerBillAmountInvoice"] != DBNull.Value ? Convert.ToDouble(dr["TotalCustomerBillAmountInvoice"]) : 0
                                                         }).FirstOrDefault();
            model.TotalCustomerCountModel = (from DataRow dr in ds.Tables[10].Rows
                                             select new TotalCustomerCountModel()
                                             {
                                                 TotalCustomerCount = dr["TotalCustomerCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCustomerCount"]) : 0
                                             }).FirstOrDefault();
            model.TotalRMRCountModel = (from DataRow dr in ds.Tables[11].Rows
                                        select new TotalRMRCountModel()
                                        {
                                            TotalRMRCount = dr["TotalRMRCount"] != DBNull.Value ? Convert.ToDouble(dr["TotalRMRCount"]) : 0
                                        }).FirstOrDefault();
            model.TotalBillAmountCountModel = (from DataRow dr in ds.Tables[12].Rows
                                               select new TotalBillAmountCountModel()
                                               {
                                                   TotalBillAmountCount = dr["TotalBillAmountCount"] != DBNull.Value ? Convert.ToDouble(dr["TotalBillAmountCount"]) : 0
                                               }).FirstOrDefault();
            return model;
        }

        public List<PackageCustomer> GetSalesSummaryReportByCompanyId(Guid companyId, DateTime? start, DateTime? end, string searchtext)
        {
            DataTable dt = _CustomerDataAccess.GetSalesSummaryReportByCompanyId(companyId, start, end, searchtext);
            List<PackageCustomer> propertyList = new List<PackageCustomer>();
            propertyList = (from DataRow dr in dt.Rows
                            select new PackageCustomer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                CustomerNum = dr["CustomerNum"].ToString(),
                                AdditionFee = dr["AdditionFee"] != DBNull.Value ? Convert.ToDouble(dr["AdditionFee"]) : 0,
                                FirstMonths = dr["FirstMonths"] != DBNull.Value ? Convert.ToDouble(dr["FirstMonths"]) : 0,
                                Tax = dr["Tax"] != DBNull.Value ? Convert.ToDouble(dr["Tax"]) : 0,
                                SalesTax = dr["SalesTax"] != DBNull.Value ? Convert.ToDouble(dr["SalesTax"]) : 0,
                                SummaryStatus = dr["SummaryStatus"].ToString(),
                                EquipmentAmount = dr["EquipmentAmount"] != DBNull.Value ? Convert.ToDouble(dr["EquipmentAmount"]) : 0,
                                ServiceFee = dr["ServiceFee"] != DBNull.Value ? Convert.ToDouble(dr["ServiceFee"]) : 0,
                                AdvancedMonitoring = dr["AdvancedMonitoring"] != DBNull.Value ? Convert.ToDouble(dr["AdvancedMonitoring"]) : 0,
                            }).ToList();
            return propertyList;
        }

        public DataTable GetSalesSummaryReportsByCompanyId(Guid companyId, DateTime? start, DateTime? end, string searchtext)
        {
            return _CustomerDataAccess.GetSalesSummaryReportExportByCompanyId(companyId, start, end, searchtext);
        }

        public List<Customer> GetRecurringBillingDetailsByCompanyId(Guid companyId, string maxdate, string mindate, string searchtext, string methodtype)
        {
            DataTable dt = _CustomerDataAccess.GetRecurringBillingDetailsByCompanyId(companyId, maxdate, mindate, searchtext, methodtype);
            List<Customer> propertyList = new List<Customer>();
            propertyList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                TotalSales = dr["TotalSales"] != DBNull.Value ? Convert.ToDouble(dr["TotalSales"]) : 0,
                                TotalTax = dr["TotalTax"] != DBNull.Value ? Convert.ToDouble(dr["TotalTax"]) : 0,
                                SalesAfterTax = dr["SalesAfterTax"] != DBNull.Value ? Convert.ToDouble(dr["SalesAfterTax"]) : 0,
                                AuthorizeRefId = dr["AuthorizeRefId"].ToString(),
                                MethodPayment = dr["MethodPayment"].ToString()
                            }).ToList();
            return propertyList;
        }

        public DataTable GetCustomerRecurringBillingReportByCompanyId(Guid comid, DateTime? start, DateTime? end, string mindate, string maxdate)
        {
            return _CustomerDataAccess.GetCustomerRecurringBillingReportByCompanyId(comid, start, end, mindate, maxdate);
        }

        public List<CustomerCancel> GetAllCustomerCancellLogByCustomerId(Guid customerId)
        {
            DataTable dt = _CustomerCancelDataAccess.GetAllCustomerCancellLogByCustomerId(customerId);
            List<CustomerCancel> propertyList = new List<CustomerCancel>();
            propertyList = (from DataRow dr in dt.Rows
                            select new CustomerCancel()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                CustomerId = (Guid)dr["CustomerId"],
                                CancelReason = dr["CancelReason"].ToString(),
                                CompanyId = (Guid)dr["CompanyId"],
                                EmployeeId = (Guid)dr["EmployeeId"],
                                EmpName = dr["EmpName"].ToString(),
                                IsActivated = dr["IsActivated"] != DBNull.Value ? Convert.ToBoolean(dr["IsActivated"]) : false,
                                CancelDatet = dr["CancelDatet"] != DBNull.Value ? Convert.ToDateTime(dr["CancelDatet"]) : new DateTime()
                            }).ToList();
            return propertyList;
        }


        public CustomerAddress GetCustomerAddesssById(int Id)
        {
            return _CustomerAddressDataAccess.Get(Id);
        }

        public CustomerAddress GetCustomerAddressByCustomerIdRefIdAddressType(Guid customerid, string RefId, string AddressType)
        {
            return _CustomerAddressDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and RefId = '{1}' and AddressType = '{2}'", customerid, RefId, AddressType)).FirstOrDefault();
        }
        public int InsertCustomerAddress(CustomerAddress customerAddress)
        {
            return (int)_CustomerAddressDataAccess.Insert(customerAddress);
        }
        public bool UpdateCustomerAddress(CustomerAddress CustomerAddress)
        {
            return _CustomerAddressDataAccess.Update(CustomerAddress) > 0;
        }

        //Get All Conversion List For Report

        public EmployeeConversionReport GetAllConversionListForReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, bool IsPaid)
        {
            DataSet ds = _CustomerDataAccess.GetAllConversionReport(FilterStartDate, FilterEndDate, order, pageno, pagesize, IsPaid);


            List<Customer> CustomerList = new List<Customer>();
            CustomerList = (from DataRow dr in ds.Tables[0].Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                CustomerId = (Guid)dr["CustomerId"],
                                FirstName = dr["CustomerName"].ToString(),
                                SSN = dr["SSN"].ToString(),
                                Type = dr["Type"].ToString(),
                                BusinessName = dr["BusinessName"].ToString(),
                                DateofBirth = dr["DateofBirth"] != DBNull.Value ? Convert.ToDateTime(dr["DateofBirth"]) : DateTime.Now,
                                PrimaryPhone = dr["PrimaryPhone"].ToString(),
                                SecondaryPhone = dr["SecondaryPhone"].ToString(),
                                CellNo = dr["CellNo"].ToString(),
                                Fax = dr["Fax"].ToString(),
                                EmailAddress = dr["EmailAddress"].ToString(),
                                Address = dr["Address"].ToString(),
                                Street = dr["Street"].ToString(),
                                City = dr["City"].ToString(),
                                State = dr["State"].ToString(),
                                ZipCode = dr["ZipCode"].ToString(),
                                Note = dr["Note"].ToString(),
                                Status = dr["Status"].ToString(),
                                ActivationFee = dr["ActivationFee"] != DBNull.Value ? Convert.ToDouble(dr["ActivationFee"]) : 0,
                                CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                                ConvertionDate = dr["ConvertionDate"] != DBNull.Value ? Convert.ToDateTime(dr["ConvertionDate"]) : DateTime.Now,

                                //Type = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                                //TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                                //CompanyId = (Guid)dr["CompanyId"],
                                //CustomerId = (Guid)dr["CustomerId"],
                                //CustomerIntId = dr["CustomerIntId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIntId"]) : 0,
                                //CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                                //CustomerName = dr["CustomerName"].ToString(),

                                //BookingId = dr["BookingId"].ToString(),
                                //Status = dr["Status"].ToString(),


                                //PickUpDate = dr["PickUpDate"] != DBNull.Value ? Convert.ToDateTime(dr["PickUpDate"]) : DateTime.Now,


                            }).ToList();

            PayrollTotalCount PayrollTotalCount = new PayrollTotalCount();
            PayrollTotalCount = (from DataRow dr in ds.Tables[1].Rows
                                 select new PayrollTotalCount()
                                 {
                                     CountTotal = dr["CountTotal"] != DBNull.Value ? Convert.ToInt32(dr["CountTotal"]) : 0
                                 }).FirstOrDefault();
            EmployeeConversionReport EmployeeConversionReportFilter = new EmployeeConversionReport();
            EmployeeConversionReportFilter.ConversionReportList = CustomerList;
            EmployeeConversionReportFilter.PayrollTotalCount = PayrollTotalCount;
            return EmployeeConversionReportFilter;
        }


        public CustomerCancellationQueueListWithCount GetAllCustomerCancellationQueue(DateTime StartDate, DateTime EndDate, int pageno, int pagesize, string reason, string contractSigned)
        {
            DataSet dsResult = _CustomerDataAccess.GetAllCustomerCancellationQueue(StartDate, EndDate, pageno, pagesize, reason, contractSigned);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];

            CustomerCancellationQueueListWithCount model = new CustomerCancellationQueueListWithCount();

            model.CustomerCancellationQueueList = (from DataRow dr in dt.Rows
                                                   select new CustomerCancellationQueue()
                                                   {
                                                       Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                       CustomerIdInt = dr["CustomerIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIdInt"]) : 0,
                                                       CancellationId = (Guid)dr["CancellationId"],
                                                       CustomerId = (Guid)dr["CustomerId"],
                                                       FirstName = dr["FirstName"].ToString(),
                                                       LastName = dr["LastName"].ToString(),
                                                       Address = dr["Address"].ToString(),
                                                       CreatedByVal = dr["CreatedByVal"].ToString(),
                                                       Reason = dr["ReasonDisplay"].ToString(),
                                                       Note = dr["Note"].ToString(),
                                                       RemainingBalance = dr["RemainingBalance"] != DBNull.Value ? Convert.ToDouble(dr["RemainingBalance"]) : 0,
                                                       CancellationDate = dr["CancellationDate"] != DBNull.Value ? Convert.ToDateTime(dr["CancellationDate"]) : new DateTime(),
                                                       CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                                       IsSigned = dr["IsSigned"] != DBNull.Value ? Convert.ToBoolean(dr["IsSigned"]) : false,
                                                       IsAlarmOff = dr["IsAlarmOff"] != DBNull.Value ? Convert.ToBoolean(dr["IsAlarmOff"]) : false,
                                                       IsInvoiceOff = dr["IsInvoiceOff"] != DBNull.Value ? Convert.ToBoolean(dr["IsInvoiceOff"]) : false,
                                                       IsBillingOff = dr["IsBillingOff"] != DBNull.Value ? Convert.ToBoolean(dr["IsBillingOff"]) : false,
                                                       IsCancelled = dr["IsCancelled"] != DBNull.Value ? Convert.ToBoolean(dr["IsCancelled"]) : false,
                                                       CustomerIsActive = dr["CustomerIsActive"] != DBNull.Value ? Convert.ToBoolean(dr["CustomerIsActive"]) : false
                                                   }).ToList();
            model.TotalCustomerCancellationCount = (from DataRow dr in dt1.Rows
                                                    select new TotalCustomerCancellationCount()
                                                    {
                                                        TotalCount = dr["TotalCustomer"] != DBNull.Value ? Convert.ToInt32(dr["TotalCustomer"]) : 0
                                                    }).FirstOrDefault();
            return model;
        }

        public DataTable GetAllCustomerCancellationQueueExport(DateTime StartDate, DateTime EndDate, int pageno, int pagesize, string reason, string contractSigned)
        {
            return _CustomerDataAccess.GetAllCustomerCancellationQueueExport(StartDate, EndDate, pageno, pagesize, reason, contractSigned);
        }

        public List<CustomerView> GetAllCustomerViewByCustomerId(Guid value, DateTime visitdate)
        {
            return _CustomerViewDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", value)).ToList();
        }

        public bool DeleteCustomerViewById(int value)
        {
            return _CustomerViewDataAccess.Delete(value) > 0;
        }

        public bool InsertCustomerCredit(CustomerCredit cc)
        {
            return _CustomerCreditDataAccess.Insert(cc) > 0;
        }

        public long InsertCustomerAddendum(CustomerAddendum ca)
        {
            return _CustomerAddendumDataAccess.Insert(ca);
        }

        public CustomerAddendum GetCustomerAddendumByCustomerIdAndTicketId(Guid ticketid, Guid customerid)
        {
            return _CustomerAddendumDataAccess.GetByQuery(string.Format("TicketId = '{0}' and CustomerId = '{1}'", ticketid, customerid)).FirstOrDefault();
        }

        public void DeleteCustomerAddendum(int value)
        {
            _CustomerAddendumDataAccess.Delete(value);
        }

        public CustomerCredit GetCustomerCreditById(int value)
        {
            return _CustomerCreditDataAccess.Get(value);
        }

        public Customer GetCustomerByAdditionalCustomerNo(string addcusno)
        {
            return _CustomerDataAccess.GetByQuery(string.Format("AdditionalCustomerNo = '{0}'", addcusno)).FirstOrDefault();
        }

        public Customer GetTransferCustomerById(int value)
        {
            return _CustomerDataAccess.GetByQuery(string.Format("TransferCustomerId = {0}", value)).FirstOrDefault();
        }

        public List<ResturantOrder> GetAllResturantOrder(Guid comid)
        {
            return _ResturantOrderDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", comid)).ToList();
        }

        public List<ResturantOrderDetail> GetAllResturantOrderDetailByOrderId(Guid orderid)
        {
            return _ResturantOrderDetailDataAccess.GetByQuery(string.Format("OrderId = '{0}'", orderid)).ToList();
        }

        public ResturantOrder GetResturantOrderById(int value)
        {
            return _ResturantOrderDataAccess.Get(value);
        }

        public Customer GetCustomerBySearch(string search)
        {
            return _CustomerDataAccess.GetByQuery(string.Format("EmailAddress = '{0}' or CellNo = '{0}'", search)).FirstOrDefault();
        }
    }
}
