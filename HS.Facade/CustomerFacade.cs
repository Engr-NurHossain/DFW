using Forte;
using Forte.Entities;
using HS.DataAccess;
using HS.Entities;
using HS.Entities.List;
using HS.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Authentication;

namespace HS.Facade
{
    public class CustomerFacade : BaseFacade
    {
        MMRDataAccess _MMRDataAccess = null;
        ActivationFeeDataAccess _ActivationFeeDataAccess = null;
        CustomerDataAccess _CustomerDataAccess;
        LeadCorrespondenceDataAccess _LeadCorrespondenceDataAccess;
        KnowledgebaseFavouriteUserDataAccess _KnowledgebaseFavouriteUserDataAccess;
        CustomerCompanyDataAccess _CustomerCompanyDataAccess;
        public CustomerFacade(ClientContext clientContext)
            : base(clientContext)
        {
            if (_MMRDataAccess == null)
                _MMRDataAccess = (MMRDataAccess)_ClientContext[typeof(MMRDataAccess)];
            if (_ActivationFeeDataAccess == null)
                _ActivationFeeDataAccess = (ActivationFeeDataAccess)_ClientContext[typeof(ActivationFeeDataAccess)];
            if (_CustomerDataAccess == null)
                _CustomerDataAccess = (CustomerDataAccess)_ClientContext[typeof(CustomerDataAccess)];
            if (_LeadCorrespondenceDataAccess == null)
                _LeadCorrespondenceDataAccess = (LeadCorrespondenceDataAccess)_ClientContext[typeof(LeadCorrespondenceDataAccess)];
            if (_KnowledgebaseFavouriteUserDataAccess == null)
                _KnowledgebaseFavouriteUserDataAccess = (KnowledgebaseFavouriteUserDataAccess)_ClientContext[typeof(KnowledgebaseFavouriteUserDataAccess)];
            if (_CustomerCompanyDataAccess == null)
                _CustomerCompanyDataAccess = (CustomerCompanyDataAccess)_ClientContext[typeof(CustomerCompanyDataAccess)];
        }
        public CustomerFacade()
        {
            if (_MMRDataAccess == null)
                _MMRDataAccess = new MMRDataAccess();
            if (_ActivationFeeDataAccess == null)
                _ActivationFeeDataAccess = new ActivationFeeDataAccess();
            if (_CustomerDataAccess == null)
                _CustomerDataAccess = new CustomerDataAccess();
            if (_LeadCorrespondenceDataAccess == null)
                _LeadCorrespondenceDataAccess = new LeadCorrespondenceDataAccess();
            if (_KnowledgebaseFavouriteUserDataAccess == null)
                _KnowledgebaseFavouriteUserDataAccess = new KnowledgebaseFavouriteUserDataAccess();
            if (_CustomerCompanyDataAccess == null)
                _CustomerCompanyDataAccess = new CustomerCompanyDataAccess();
        }

        public CustomerFacade(string constr)
        {
            _CustomerDataAccess = new CustomerDataAccess(constr);
            _LeadCorrespondenceDataAccess = new LeadCorrespondenceDataAccess(constr);
            _CustomerCompanyDataAccess = new CustomerCompanyDataAccess(constr);
        }

        CustomerContactTrackDataAccess _CustomerContactTrackDataAccess
        {
            get
            {
                return (CustomerContactTrackDataAccess)_ClientContext[typeof(CustomerContactTrackDataAccess)];
            }
        }
        CustomerProratedBillDataAccess _CustomerProratedBillDataAccess
        {
            get
            {
                return (CustomerProratedBillDataAccess)_ClientContext[typeof(CustomerProratedBillDataAccess)];
            }
        }
        QA2ScriptDataAccess _QA2ScriptDataAccess
        {
            get
            {
                return (QA2ScriptDataAccess)_ClientContext[typeof(QA2ScriptDataAccess)];
            }
        }

        QAFinanceDataAccess _QAFinanceDataAccess
        {
            get
            {
                return (QAFinanceDataAccess)_ClientContext[typeof(QAFinanceDataAccess)];
            }
        }
        QA1ScriptDataAccess _QA1ScriptDataAccess
        {
            get
            {
                return (QA1ScriptDataAccess)_ClientContext[typeof(QA1ScriptDataAccess)];
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
        AlarmCustomerSelectedAddonDataAccess _AlarmCustomerSelectedAddonDataAccess
        {
            get
            {
                return (AlarmCustomerSelectedAddonDataAccess)_ClientContext[typeof(AlarmCustomerSelectedAddonDataAccess)];
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
        SMSHistoryDataAccess _SMSHistoryDataAccess
        {
            get
            {
                return (SMSHistoryDataAccess)_ClientContext[typeof(SMSHistoryDataAccess)];
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
        CustomerCancellationReasonDataAccess _CustomerCancellationReasonDataAccess
        {
            get
            {
                return (CustomerCancellationReasonDataAccess)_ClientContext[typeof(CustomerCancellationReasonDataAccess)];
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
        AgreementQuestionDataAccess _AgreementQuestionDataAccess
        {
            get
            {
                return (AgreementQuestionDataAccess)_ClientContext[typeof(AgreementQuestionDataAccess)];
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


        AlarmCustomerTerminationDataAccess _AlarmCustomerTerminationDataAccess
        {
            get
            {
                return (AlarmCustomerTerminationDataAccess)_ClientContext[typeof(AlarmCustomerTerminationDataAccess)];
            }
        }
        BrinksSignedInfoDataAccess _BrinksSignedInfoDataAccess
        {
            get
            {
                return (BrinksSignedInfoDataAccess)_ClientContext[typeof(BrinksSignedInfoDataAccess)];
            }
        }
        SecondaryCreditCheckContactDataAccess _SecondaryCreditCheckContactDataAccess
        {
            get
            {
                return (SecondaryCreditCheckContactDataAccess)_ClientContext[typeof(SecondaryCreditCheckContactDataAccess)];
            }
        }
        CustomerIsPcCreditApplicationDataAccess _CustomerIsPcCreditApplicationDataAccess
        {
            get
            {
                return (CustomerIsPcCreditApplicationDataAccess)_ClientContext[typeof(CustomerIsPcCreditApplicationDataAccess)];
            }
        }
        RecurringBillingScheduleDataAccess _RecurringBillingScheduleDataAccess
        {
            get
            {
                return (RecurringBillingScheduleDataAccess)_ClientContext[typeof(RecurringBillingScheduleDataAccess)];
            }
        }
        RecurringBillingScheduleItemsDataAccess _RecurringBillingScheduleItemsDataAccess
        {
            get
            {
                return (RecurringBillingScheduleItemsDataAccess)_ClientContext[typeof(RecurringBillingScheduleItemsDataAccess)];
            }
        }

        ThirdPartyAgenciesDataAccess _ThirdPartyAgenciesDataAccess
        {
            get
            {
                return (ThirdPartyAgenciesDataAccess)_ClientContext[typeof(ThirdPartyAgenciesDataAccess)];
            }
        }
        IndividualInstalledEquipmentDataAccess _IndividualInstalledEquipmentDataAccess
        {
            get
            {
                return (IndividualInstalledEquipmentDataAccess)_ClientContext[typeof(IndividualInstalledEquipmentDataAccess)];
            }
        }
        PowerPayFinanceDataAccess _PowerPayFinanceDataAccess
        {
            get
            {
                return (PowerPayFinanceDataAccess)_ClientContext[typeof(PowerPayFinanceDataAccess)];
            }
        }
        CustomerRouteDataAccess _CustomerRouteDataAccess
        {
            get
            {
                return (CustomerRouteDataAccess)_ClientContext[typeof(CustomerRouteDataAccess)];
            }
        }
        GeeseRouteDataAccess _GeeseRouteDataAccess
        {
            get
            {
                return (GeeseRouteDataAccess)_ClientContext[typeof(GeeseRouteDataAccess)];
            }
        }
        EmployeeRouteDataAccess _EmployeeRouteDataAccess
        {
            get
            {
                return (EmployeeRouteDataAccess)_ClientContext[typeof(EmployeeRouteDataAccess)];
            }
        }
        CustomerCheckLogDataAccess _CustomerCheckLogDataAccess
        {
            get
            {
                return (CustomerCheckLogDataAccess)_ClientContext[typeof(CustomerCheckLogDataAccess)];
            }
        }

        HomeOwnerHistoryDataAccess _HomeOwnerHistoryDataAccess
        {
            get
            {
                return (HomeOwnerHistoryDataAccess)_ClientContext[typeof(HomeOwnerHistoryDataAccess)];
            }
        }
        public CustomerIsPcCreditApplication GetCustomerIsPcAppById(int id)
        {
            return _CustomerIsPcCreditApplicationDataAccess.Get(id);
        }
        public PowerPayFinance GetCustomerPowerPayAppById(int id)
        {
            return _PowerPayFinanceDataAccess.Get(id);
        }
        public CustomerIsPcCreditApplication GetCustomerIsPcAppByCustomerId(Guid customerId)
        {
            return _CustomerIsPcCreditApplicationDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerId)).FirstOrDefault();
        }
        public PowerPayFinance GetCustomerPowerPayAppByCustomerId(Guid customerId)
        {
            return _PowerPayFinanceDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerId)).FirstOrDefault();
        }
        public long UpdateIsPcCreditApp(CustomerIsPcCreditApplication application)
        {
            return _CustomerIsPcCreditApplicationDataAccess.Update(application);
        }
        public long UpdatePowerPayCreditApp(PowerPayFinance application)
        {
            return _PowerPayFinanceDataAccess.Update(application);
        }
        public long InsertIsPcCreditApp(CustomerIsPcCreditApplication application)
        {
            return _CustomerIsPcCreditApplicationDataAccess.Insert(application);
        }
        public long InsertPowerPayCreditApp(PowerPayFinance application)
        {
            return _PowerPayFinanceDataAccess.Insert(application);
        }

        public long InsertHomeOwnerHistory(HomeOwnerHistory history)
        {
            return _HomeOwnerHistoryDataAccess.Insert(history);
        }
        public List<HomeOwnerHistory> GetHomeOwnerListBCustomerId(Guid customerId, Guid CompanyId)
        {

            //return _AlarmCustomerTerminationDataAccess.GetByQuery(string.Format("[CustomerId]='{0}'", cus.CustomerId.ToString())).ToList();
            DataTable dt = _HomeOwnerHistoryDataAccess.GetHomeOwnerListBCustomerId(customerId, CompanyId);
            List<HomeOwnerHistory> HomeOwnerList = new List<HomeOwnerHistory>();
            HomeOwnerList = (from DataRow dr in dt.Rows
                             select new HomeOwnerHistory()
                             {
                                 Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,


                                 RequestedByVal = dr["RequestedByVal"].ToString(),
                                 CustomerName = dr["CustomerName"].ToString(),
                                 OwnerAddress = dr["OwnerAddress"].ToString(),
                                 HomeOwnerName = dr["HomeOwnerName"].ToString(),
                                 CustomerId = (Guid)dr["CustomerId"],

                                 RequestedDate = dr["RequestedDate"] != DBNull.Value ? Convert.ToDateTime(dr["RequestedDate"]) : new DateTime(),


                             }).ToList();
            return HomeOwnerList;
        }

        public long DeleteHomeOwner(int id)
        {
            //return _AlarmCustomerTerminationDataAccess.GetByQuery(string.Format("[CustomerId]='{0}'", cus.CustomerId.ToString())).ToList();
            return _HomeOwnerHistoryDataAccess.Delete(id);
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

        public BrinksSignedInfo GetBrinksSignedInfoByCustomerId(Guid customerId)
        {
            return _BrinksSignedInfoDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerId)).FirstOrDefault();
        }
        public long UpdateBrinksSignedInfo(BrinksSignedInfo signedInfo)
        {
            return _BrinksSignedInfoDataAccess.Update(signedInfo);
        }
        public long InsertBrinksSingendInfo(BrinksSignedInfo signedInfo)
        {
            return _BrinksSignedInfoDataAccess.Insert(signedInfo);
        }
        public Customer GetCustomerByCentralStationRefId(int id)
        {
            return _CustomerDataAccess.GetByQuery(string.Format(" CentralStationRefId = '{0}' ", id)).FirstOrDefault();
        }
        public List<Customer> GetAllAuthorizeCustomer()
        {
            return _CustomerDataAccess.GetByQuery(" AuthorizeRefId != '' ");
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
        public CustomerCompany GetCustomerCompanyByCustomerId(Guid customerId)
        {
            return _CustomerCompanyDataAccess.GetByQuery(string.Format("[CustomerId]='{0}'", customerId)).FirstOrDefault();
        }
        public CustomerCompany GetCustomerCompanyByCustomerGuidId(Guid CustomerId)
        {
            return _CustomerCompanyDataAccess.GetByQuery(string.Format("[CustomerId]='{0}'", CustomerId)).FirstOrDefault();
        }

        public long InsertCutomerAlarmAddons(AlarmCustomerSelectedAddon addon)
        {
            return _AlarmCustomerSelectedAddonDataAccess.Insert(addon);
        }
        public long UpdateCutomerAlarmAddons(AlarmCustomerSelectedAddon addon)
        {
            return _AlarmCustomerSelectedAddonDataAccess.Update(addon);
        }
        public long DeleteCutomerAlarmAddons(int id)
        {
            return _AlarmCustomerSelectedAddonDataAccess.Delete(id);
        }
        public List<AlarmCustomerSelectedAddon> GetAllCutomerAlarmAddonsByCustomerId(Guid CustomerId)
        {
            return _AlarmCustomerSelectedAddonDataAccess.GetByQuery(string.Format("[CustomerId]='{0}'", CustomerId)).ToList();
        }
        public List<AlarmCustomerTermination> GetAlarmTerminationHistoryByCustomerId(Guid customerId)
        {

            //return _AlarmCustomerTerminationDataAccess.GetByQuery(string.Format("[CustomerId]='{0}'", cus.CustomerId.ToString())).ToList();
            DataTable dt = _AlarmCustomerTerminationDataAccess.GetAllAlarTerminationLogByCusId(customerId);
            List<AlarmCustomerTermination> AlarmCustomerTerminationList = new List<AlarmCustomerTermination>();
            AlarmCustomerTerminationList = (from DataRow dr in dt.Rows
                                            select new AlarmCustomerTermination()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                AlarmId = dr["AlarmId"] != DBNull.Value ? Convert.ToInt32(dr["AlarmId"]) : 0,

                                                TerminationReason = dr["TerminationReason"].ToString(),
                                                TerminationBy = dr["TerminationBy"].ToString(),
                                                CustomerId = (Guid)dr["CustomerId"],

                                                TerminationDate = dr["TerminationDate"] != DBNull.Value ? Convert.ToDateTime(dr["TerminationDate"]) : new DateTime(),


                                            }).ToList();
            return AlarmCustomerTerminationList;
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
        public DataTable GetAllLeadsReportByCompanyForHudsonOnly(Guid CompanyId, DateTime? Start, DateTime? End, string status, string market, string leads, string soldBy, string salesperson)
        {
            return _CustomerDataAccess.GetAllLeadsReportByCompanyForHudsonOnly(CompanyId, Start, End, status, market, leads, soldBy, salesperson);
        }
        public DataTable GetAllHudsonFollowupReportByCompany(Guid CompanyId, DateTime? Start, DateTime? End, string status, string market, string leads, string soldBy, string SearchText, string StatusIDList, string SalesopenerList, string LeadsourceIdList, string SalespersonList)
        {
            return _CustomerDataAccess.GetAllHudsonFollowupReportByCompany(CompanyId, Start, End, status, market, leads, soldBy, SearchText, StatusIDList, SalesopenerList, LeadsourceIdList, SalespersonList);
        }
        public DataTable GetAllFinancedDealsReportCompany(Guid CompanyId, DateTime? Start, DateTime? End, string status, string market, string leads, string soldBy, string SearchText, string FundingCompany, double? FinanceTerm, string StatusIDList)
        {
            return _CustomerDataAccess.GetAllFinancedDealsReportCompany(CompanyId, Start, End, status, market, leads, soldBy, SearchText, FundingCompany, FinanceTerm, StatusIDList);
        }
        public DataTable GetAllFinancedReportCompany(Guid CompanyId, DateTime? Start, DateTime? End, string status, string FinRep, string leads, string soldBy, string SearchText, string FundingCompany, double? FinanceTerm, string SalesRep, FilterReportModel filter)
        {
            return _CustomerDataAccess.GetAllFinancedReportCompany(CompanyId, Start, End, status, FinRep, leads, soldBy, SearchText, FundingCompany, FinanceTerm, SalesRep, filter);
        }
        public DataTable GetAllCustomerReportByCompany(Guid CompanyId, DateTime? Start, DateTime? End, string status, string soldBy)
        {
            return _CustomerDataAccess.GetAllCustomerReportByCompany(CompanyId, Start, End, status, soldBy);
        }
        public DataTable GetAllInactiveCustomerReportByCompany(Guid CompanyId, DateTime? Start, DateTime? End)
        {
            return _CustomerDataAccess.GetAllInactiveCustomerReportByCompany(CompanyId, Start, End);
        }
        public DataTable GetAllHudsonCustomerReportByCompany(Guid CompanyId, DateTime? Start, DateTime? End, List<string> Status, string market, string soldBy, string acctype, string servicetype)
        {
            return _CustomerDataAccess.GetAllHudsonCustomerReportByCompany(CompanyId, Start, End, Status, market, soldBy, acctype, servicetype);
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
        public List<Customer> GetAllTestAccountReportByCompanyCount(Guid CompanyId, DateTime? Start, DateTime? End, FilterReportModel filter)
        {
            //return _CustomerDataAccess.GetAllTransferCustomerReportByCompany(CompanyId, Start, End);
            DataTable dt = _CustomerDataAccess.GetAllTestAccountReportByCompany(CompanyId, Start, End, filter);
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
        public DataTable GetAllTestAccountReportByCompany(Guid CompanyId, DateTime? Start, DateTime? End, FilterReportModel filter)
        {
            return _CustomerDataAccess.GetAllTestAccountReportByCompany(CompanyId, Start, End, filter);
        }
        public DataTable GetAllTransferCustomerReportByCompany(Guid CompanyId, DateTime? Start, DateTime? End, FilterReportModel filter)
        {
            return _CustomerDataAccess.GetAllTransferCustomerReportByCompany(CompanyId, Start, End, filter);
        }
        public DataTable GetAllTicketReportByCompany(Guid CompanyId, DateTime? Start, DateTime? End, TicketFilter Filters, FilterReportModel filter)
        {
            return _CustomerDataAccess.GetAllTicketReportByCompany(CompanyId, Start, End, Filters, filter);
        }
        public DataTable GetAllTicketReportAppointmwntDateByCompany(Guid CompanyId, DateTime? Start, DateTime? End, TicketFilter Filters, FilterReportModel filter)
        {
            return _CustomerDataAccess.GetAllTicketReportAppointmentDateByCompany(CompanyId, Start, End, Filters, filter);
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
            //return _EmployeeDataAccess.GetTicketListInstallReportByFilter(CompanyId, Start, End, Filters);
            return _EmployeeDataAccess.DownloadIndividualInstalledEquipment(CompanyId, Start, End, Filters);
        }
        public DataTable GetTicketListAllReportByFilter(Guid CompanyId, DateTime? Start, DateTime? End, TicketFilter Filters)
        {
            return _EmployeeDataAccess.GetTicketListAllReportByFilter(CompanyId, Start, End, Filters);
        }

        public DataTable GetInstallationTrackerTicketListReportByFilter(Guid CompanyId, DateTime? Start, DateTime? End, TicketFilter Filters, FilterReportModel filter)
        {
            return _CustomerDataAccess.GetInstallationTrackerTicketReportByCompany(CompanyId, Start, End, Filters, filter);
        }
        public DataTable GetCSRActivityListReportByFilter(TicketFilter Filters, FilterReportModel filter)
        {
            return _CustomerDataAccess.GetCSRReportListByFilter(Filters, filter);
        }
        public DataTable GetServiceTrackerReportListByFilter(TicketFilter Filters, FilterReportModel filter)
        {
            return _CustomerDataAccess.GetServiceTrackerReportListByFilter(Filters, filter);
        }

        public DataTable GetTaskReportListByFilter(TicketFilter Filters, FilterReportModel filter, Guid comId, Guid empId, bool allTask)
        {
            return _CustomerDataAccess.GetTaskReportListByFilter(Filters, filter, comId, empId, allTask);
        }

        public DataTable GetTechnicianReportByFilter(Guid CompanyId, DateTime? Start, DateTime? End, EmployeeFilter Filters, FilterReportModel filter)
        {
            return _CustomerDataAccess.GetTechnicianReportByCompany(CompanyId, Start, End, Filters, filter);
        }
        public DataTable GetAllTicketReportByCompanyForGoBack(Guid CompanyId, DateTime? Start, DateTime? End, TicketFilter Filters, FilterReportModel filter)
        {
            return _CustomerDataAccess.GetAllTicketReportByCompanyForGoBack(CompanyId, Start, End, Filters, filter);
        }

        public List<Customer> GetForteSubscribedCustomerForSync()
        {
            return _CustomerDataAccess.GetByQuery(string.Format("ScheduleToken is not null and ScheduleToken != '' and  (BillCycle = '' or FirstBilling is null) "));
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

        public List<Customer> GetAllCustomerByCompany(Guid CompanyId, int pageno, int pagesize, List<string> Status, string market, string soldBy, string acctype, string servicetype)
        {
            DataTable dt = _CustomerDataAccess.GetAllCustomerByCompany(CompanyId, null, null, pageno, pagesize, Status, market, soldBy, acctype, servicetype);
            List<Customer> propertyList = new List<Customer>();
            propertyList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                CustomerNo = dr["CustomerNo"].ToString(),
                                EmailAddress = dr["EmailAddress"].ToString(),
                                Address = dr["Address"].ToString(),
                                Street = dr["Street"].ToString(),
                                City = dr["City"].ToString(),
                                ZipCode = dr["ZipCode"].ToString(),
                                State = dr["State"].ToString(),
                                BusinessName = dr["BusinessName"].ToString(),
                                CustomerId = (Guid)dr["CustomerId"],
                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                PrimaryPhone = dr["PrimaryPhone"].ToString(),
                                JoinDate = dr["JoinDate"] != DBNull.Value ? Convert.ToDateTime(dr["JoinDate"]) : new DateTime(),
                                Status = dr["Status"].ToString(),
                                IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                                CustomerType = dr["CustomerType"].ToString(),
                                AnnualRevenue = dr["AnnualRevenue"] != DBNull.Value ? Convert.ToDouble(dr["AnnualRevenue"]) : 0,
                                AccountType = dr["AccountType"].ToString(),
                                PersonSales = dr["PersonSales"].ToString(),
                                MarketVal = dr["MarketVal"].ToString(),
                                DBA = dr["DBA"].ToString(),
                                CancellationDate = dr["CancellationDate"] != DBNull.Value ? Convert.ToDateTime(dr["CancellationDate"]) : new DateTime()
                            }).ToList();
            return propertyList;
        }

        public List<Customer> GetAllCancelCustomerByCompany(Guid CompanyId, int pageno, int pagesize, string order, string SearchText)
        {
            DataTable dt = _CustomerDataAccess.GetAllCancelCustomerByCompany(CompanyId, null, null, pageno, pagesize, order, SearchText);
            List<Customer> propertyList = new List<Customer>();
            propertyList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                CancellationId = dr["CancellationId"] != DBNull.Value ? Convert.ToInt32(dr["CancellationId"]) : 0,
                                CustomerNo = dr["CustomerNo"].ToString(),
                                EmailAddress = dr["EmailAddress"].ToString(),
                                Address = dr["Address"].ToString(),
                                Street = dr["Street"].ToString(),
                                City = dr["City"].ToString(),
                                ZipCode = dr["ZipCode"].ToString(),
                                State = dr["State"].ToString(),
                                BusinessName = dr["BusinessName"].ToString(),
                                CustomerId = (Guid)dr["CustomerId"],
                                FirstName = dr["FirstName"].ToString(),
                                LastName = dr["LastName"].ToString(),
                                PrimaryPhone = dr["PrimaryPhone"].ToString(),
                                JoinDate = dr["JoinDate"] != DBNull.Value ? Convert.ToDateTime(dr["JoinDate"]) : new DateTime(),
                                Status = dr["Status"].ToString(),
                                IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                                //CustomerType = dr["CustomerType"].ToString(),
                                //AnnualRevenue = dr["AnnualRevenue"] != DBNull.Value ? Convert.ToDouble(dr["AnnualRevenue"]) : 0,
                                //AccountType = dr["AccountType"].ToString(),
                                //PersonSales = dr["PersonSales"].ToString(),
                                //MarketVal = dr["MarketVal"].ToString(),
                                DBA = dr["DBA"].ToString(),
                                CancellationReason = dr["CancellationReason"].ToString(),
                                CancellationDate = dr["CancellationDate"] != DBNull.Value ? Convert.ToDateTime(dr["CancellationDate"]) : new DateTime()
                            }).ToList();

            return propertyList;
        }

        public DataTable GetAllCustomerByCompanyCancelReport(Guid CompanyId, DateTime? startdate, DateTime? endtime, int pageno, int pagesize, List<string> Status, string market)
        {
            return _CustomerDataAccess.GetAllCustomerByCompanyCancelReport(CompanyId, startdate, endtime, pageno, pagesize, Status, market);
        }
        public DataTable GetAllCancelCustomerByCompanyCancelReport(Guid CompanyId, DateTime? startdate, DateTime? endtime, int pageno, int pagesize, string SearchText)
        {
            return _CustomerDataAccess.GetAllCancelCustomerByCompanyCancelReport(CompanyId, startdate, endtime, pageno, pagesize, SearchText);
        }

        public List<Customer> GetAllCustomerByCompanyCount(Guid CompanyId, DateTime? start, DateTime? end, List<string> Status, string market, string soldBy, string acctype, string servicetype)
        {
            DataTable dt = _CustomerDataAccess.GetAllCustomerByCompanyCount(CompanyId, start, end, Status, market, soldBy, acctype, servicetype);
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
        public List<Customer> GetAllCancelCustomerByCompanyCount(Guid CompanyId, DateTime? start, DateTime? end)
        {
            DataTable dt = _CustomerDataAccess.GetAllCancelCustomerByCompanyCount(CompanyId, start, end);
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
                                //AnnualRevenue = dr["AnnualRevenue"] != DBNull.Value ? Convert.ToDouble(dr["AnnualRevenue"]) : 0,
                                //AccountType = dr["AccountType"].ToString(),
                                //PersonSales = dr["PersonSales"].ToString(),
                                //MarketVal = dr["MarketVal"].ToString(),
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
        public CustomerListWithCountModel GetAllConvertedCustomerByCompany(Guid CompanyId, int pageno, int pagesize, FilterReportModel filter, string order)
        {
            DataSet ds = _CustomerDataAccess.GetAllConvertedCustomerByCompany(CompanyId, null, null, pageno, pagesize, filter, order);
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

        public bool UpdateBrinksFundingStatusByCustomerIdList(List<string> IdList)
        {
            return _CustomerDataAccess.UpdateBrinksFundingStatusByCustomerIdList(IdList);
        }
        public CustomerListWithCountModel GetAllLeadsByCompanyAndDates(Guid CompanyId, DateTime Start, DateTime End, int pageno, int pagesize, string status, string market, string leads, string soldBy, string order)
        {
            DataSet dsResult = _CustomerDataAccess.GetAllLeadsByCompany(CompanyId, Start, End, pageno, pagesize, status, market, leads, soldBy, order);
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
                                Passengers = dr["Passengers"] != DBNull.Value ? Convert.ToInt32(dr["Passengers"]) : 0,
                                Budget = dr["Budget"] != DBNull.Value ? Convert.ToDouble(dr["Budget"]) : 0,


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
        public CustomerListWithCountModel GetAllLeadsByCompanyAndDatesForHudsonOnly(Guid CompanyId, DateTime Start, DateTime End, int pageno, int pagesize, string status, string market, string leads, string soldBy, string order, string salesperson)
        {
            DataSet dsResult = _CustomerDataAccess.GetAllLeadsByCompanyForHudsonOnly(CompanyId, Start, End, pageno, pagesize, status, market, leads, soldBy, order, salesperson);
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
                                Passengers = dr["Passengers"] != DBNull.Value ? Convert.ToInt32(dr["Passengers"]) : 0,
                                Budget = dr["Budget"] != DBNull.Value ? Convert.ToDouble(dr["Budget"]) : 0,


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
        public CustomerListWithCountModel GetAllFollowupByCompanyAndDates(Guid CompanyId, DateTime Start, DateTime End, int pageno, int pagesize, string status, string market, string leads, string soldBy, string searchText, string StatusIDList, string SalesopenerList, string LeadsourceIdList, string SalespersonList, string order)
        {
            DataSet dsResult = _CustomerDataAccess.GetAllFollowupByCompany(CompanyId, Start, End, pageno, pagesize, status, market, leads, soldBy, searchText, StatusIDList, SalesopenerList, LeadsourceIdList, SalespersonList, order);
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

                                //Address = dr["Address"].ToString(),
                                BusinessName = dr["Name"].ToString(),
                                FirstName = dr["SalesOpenar"].ToString(),
                                PersonSales = dr["SalesPerson"].ToString(),
                                LeadSource = dr["LeadSource"].ToString(),
                                LeadSourceType = dr["LeadSourceType"].ToString(),
                                Status = dr["Status"].ToString(),
                                FollowUpDate = dr["FollowUpDate"] != DBNull.Value ? Convert.ToDateTime(dr["FollowUpDate"]) : new DateTime(),

                                //CreatedName = dr["CreatedName"].ToString()
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
            LeadList.searchText = searchText;
            LeadList.StatusIDList = StatusIDList;
            return LeadList;
        }
        public CustomerListWithCountModel GetAllFinanceDealsBycustomerIdandCompanyId(Guid CompanyId, DateTime Start, DateTime End, int pageno, int pagesize, string status, string market, string leads, string soldBy, string searchText, string FundingCompany, double? FinanceTerm, string StatusIDList, string order)
        {
            DataSet dsResult = _CustomerDataAccess.GetAllFinanceDealsByCompanyandCustomer(CompanyId, Start, End, pageno, pagesize, status, market, leads, soldBy, searchText, FundingCompany, FinanceTerm, StatusIDList, order);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            DataTable dt2 = dsResult.Tables[2];
            DataTable dt3 = dsResult.Tables[3];
            List<Customer> propertyList = new List<Customer>();
            TotalCustomerCount CustomerCount = new TotalCustomerCount();
            CustomerCount Cus = new CustomerCount();

            propertyList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                CustomerNo = dr["CustomerNo"].ToString(),

                                BusinessName = dr["Name"].ToString(),

                                FundingCompany = dr["FundingCompany"].ToString(),
                                FinancedAmount = dr["FinancedAmount"] != DBNull.Value ? Convert.ToDouble(dr["FinancedAmount"]) : 0,
                                //financedamount = dr["FinancedAmount"].ToString(),
                                FinancedTerm = dr["FinancedTerm"] != DBNull.Value ? Convert.ToDouble(dr["FinancedTerm"]) : 0,
                                TicketStatus = dr["Status"].ToString(),
                                //CreatedName = dr["CreatedName"].ToString()
                            }).ToList();


            CustomerCount = (from DataRow dr in dt2.Rows
                             select new TotalCustomerCount()
                             {
                                 Counter = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0

                             }).FirstOrDefault();
            Cus = (from DataRow dr in dt3.Rows
                   select new CustomerCount()
                   {
                       TotalCustomer = dr["CountCustomer"] != DBNull.Value ? Convert.ToInt32(dr["CountCustomer"]) : 0
                   }).FirstOrDefault();
            CustomerListWithCountModel LeadList = new CustomerListWithCountModel();
            LeadList.TotalAmountByPage = dsResult.Tables[1].Rows[0]["TotalAmountByPage"] != DBNull.Value ? Convert.ToDouble(dsResult.Tables[1].Rows[0]["TotalAmountByPage"]) : 0;
            LeadList.CustomerList = propertyList;
            LeadList.TotalCustomerCount = CustomerCount;
            LeadList.CustomerCount = Cus;
            LeadList.searchText = searchText;
            //   LeadList.StatusIDList = StatusIDList;
            return LeadList;


        }
        public CustomerListWithCountModel GetAllFinancedBycustomerIdandCompanyId(Guid CompanyId, DateTime Start, DateTime End, int pageno, int pagesize, string status, string FinRep, string leads, string soldBy, string searchText, string FundingCompany, double? FinanceTerm, string SalesRep, string order, FilterReportModel filter)
        {
            DataSet dsResult = _CustomerDataAccess.GetAllFinancedByCompanyandCustomer(CompanyId, Start, End, pageno, pagesize, status, FinRep, leads, soldBy, searchText, FundingCompany, FinanceTerm, SalesRep, order, filter);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            DataTable dt2 = dsResult.Tables[2];
            DataTable dt3 = dsResult.Tables[3];
            List<Customer> propertyList = new List<Customer>();
            TotalCustomerCount CustomerCount = new TotalCustomerCount();
            CustomerCount Cus = new CustomerCount();

            propertyList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                CustomerNo = dr["CustomerNo"].ToString(),
                                InstallDate = dr["InstallDate"] != DBNull.Value ? Convert.ToDateTime(dr["InstallDate"]) : new DateTime(),
                                SalesDate = dr["SalesDate"] != DBNull.Value ? Convert.ToDateTime(dr["SalesDate"]) : new DateTime(),
                                CreatedDate = dr["ScheduleDate"] != DBNull.Value ? Convert.ToDateTime(dr["ScheduleDate"]) : new DateTime(),
                                PersonSales = dr["SoldbyVal"].ToString(),
                                FinanceRepValue = dr["FinancerepVal"].ToString(),
                                BusinessName = dr["Name"].ToString(),


                                FinancedAmount = dr["FinancedAmount"] != DBNull.Value ? Convert.ToDouble(dr["FinancedAmount"]) : 0,

                            }).ToList();


            CustomerCount = (from DataRow dr in dt2.Rows
                             select new TotalCustomerCount()
                             {
                                 Counter = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0

                             }).FirstOrDefault();
            Cus = (from DataRow dr in dt3.Rows
                   select new CustomerCount()
                   {
                       TotalCustomer = dr["CountCustomer"] != DBNull.Value ? Convert.ToInt32(dr["CountCustomer"]) : 0
                   }).FirstOrDefault();
            CustomerListWithCountModel LeadList = new CustomerListWithCountModel();
            LeadList.TotalAmountByPage = dsResult.Tables[1].Rows[0]["TotalAmountByPage"] != DBNull.Value ? Convert.ToDouble(dsResult.Tables[1].Rows[0]["TotalAmountByPage"]) : 0;
            LeadList.CustomerList = propertyList;
            LeadList.TotalCustomerCount = CustomerCount;
            LeadList.CustomerCount = Cus;
            LeadList.searchText = searchText;
            //   LeadList.StatusIDList = StatusIDList;
            return LeadList;


        }
        public List<Customer> GetAllCustomerByCompanyAndDates(Guid CompanyId, DateTime Start, DateTime End, int pageno, int pagesize, List<string> Status, string market, string soldBy, string acctype, string servicttype)
        {
            DataTable dt = _CustomerDataAccess.GetAllCustomerByCompany(CompanyId, Start, End, pageno, pagesize, Status, market, soldBy, acctype, servicttype);
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

                                CustomerType = dr["CustomerType"].ToString(),
                                AnnualRevenue = dr["AnnualRevenue"] != DBNull.Value ? Convert.ToDouble(dr["AnnualRevenue"]) : 0,
                                AccountType = dr["AccountType"].ToString(),
                                PersonSales = dr["PersonSales"].ToString(),
                                MarketVal = dr["MarketVal"].ToString(),
                                DBA = dr["DBA"].ToString(),
                                CancellationDate = dr["CancellationDate"] != DBNull.Value ? Convert.ToDateTime(dr["CancellationDate"]) : new DateTime()
                            }).ToList();
            return propertyList;
        }

        public List<Customer> GetAllCancelCustomerByCompanyAndDates(Guid CompanyId, DateTime Start, DateTime End, int pageno, int pagesize, string order, string SearchText)
        {
            DataTable dt = _CustomerDataAccess.GetAllCancelCustomerByCompany(CompanyId, Start, End, pageno, pagesize, order, SearchText);
            List<Customer> propertyList = new List<Customer>();
            propertyList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                CancellationId = dr["CancellationId"] != DBNull.Value ? Convert.ToInt32(dr["CancellationId"]) : 0,
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

                                //CustomerType = dr["CustomerType"].ToString(),
                                //AnnualRevenue = dr["AnnualRevenue"] != DBNull.Value ? Convert.ToDouble(dr["AnnualRevenue"]) : 0,
                                //AccountType = dr["AccountType"].ToString(),
                                //PersonSales = dr["PersonSales"].ToString(),
                                //MarketVal = dr["MarketVal"].ToString(),
                                DBA = dr["DBA"].ToString(),
                                CancellationReason = dr["CancellationReason"].ToString(),
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
        public CustomerListWithCountModel GetAllConvertedCustomerByCompanyAndDates(Guid CompanyId, DateTime Start, DateTime End, int pageno, int pagesize, FilterReportModel filter, string order)
        {
            DataSet ds = _CustomerDataAccess.GetAllConvertedCustomerByCompany(CompanyId, Start, End, pageno, pagesize, filter, order);
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

        public DataTable GetCustomerReport(int[] IdList, string[] columnList, Guid CompanyId, string NumberPrefix, string acorin, Guid UserId, DateTime Start, DateTime End, List<Partner> Partners, bool isPermit, string EmployeeRole, string ReportFor, CustomerLiteFilter filter)
        {
            return _CustomerDataAccess.GetCustomerReport(IdList, columnList, CompanyId, NumberPrefix, acorin, UserId, Start, End, Partners, isPermit, EmployeeRole, ReportFor, filter);
        }
        public DataTable GetCustomerDatabaseReport(int[] IdList, string[] columnList, Guid CompanyId, string NumberPrefix, string acorin, Guid UserId, DateTime Start, DateTime End, string ReportFor)
        {
            return _CustomerDataAccess.GetCustomerDatabaseReport(IdList, columnList, CompanyId, NumberPrefix, acorin, UserId, Start, End, ReportFor);
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

        public List<Customer> GetAllCustomerByCompanyId(Guid CompanyId, DateTime? start, DateTime? end, int pageno, int pagesize, List<string> status, string market, string acctype, string servicetype)
        {
            DataTable dt = _CustomerDataAccess.GetAllCustomerByCompany(CompanyId, start, end, pageno, pagesize, status, market, null, acctype, servicetype);
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
                              Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                              EmailAddress = dr["EmailAddress"].ToString(),
                              Name = dr["Name"].ToString(),
                              BusinessName = dr["BusinessName"].ToString(),
                              PhoneNumber = dr["PhoneNumber"].ToString(),
                              Type = dr["Type"].ToString()
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

        public List<Customer> GetLeadsByKeyAndCompanyId(Guid CompanyId, string key, string emptag, string emprole, Guid empid, bool ispermit)
        {
            DataTable dt = _CustomerDataAccess.GetLeadssByKeyAndCompanyId(CompanyId, key, emptag, emprole, empid, ispermit);
            List<Customer> CustomerList = new List<Customer>();
            CustomerList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                AgemniId = dr["RefenrenceId"] != DBNull.Value ? Convert.ToInt32(dr["RefenrenceId"]) : 0,
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
                                LeadSourceVal = dr["LeadSourceVal"].ToString(),
                                LeadSourceParent = dr["LeadSourceParent"].ToString(),
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

        public CustomerTabCounts GetCustomerTabCountsByCustomerId(Guid customerId, string techid, Guid companyid, Guid user, bool orderpermit)
        {
            DataTable dt = _CustomerDataAccess.GetCustomerTabCountsByCustomerId(customerId, techid, companyid, user, orderpermit);
            List<CustomerTabCounts> CustomerTabCounts = new List<CustomerTabCounts>();
            CustomerTabCounts = (from DataRow dr in dt.Rows
                                 select new CustomerTabCounts()
                                 {
                                     CorrespondenceCount = dr["CorrespondenceCount"] != DBNull.Value ? Convert.ToInt32(dr["CorrespondenceCount"]) : 0,
                                     EstimateCount = dr["EstimateCount"] != DBNull.Value ? Convert.ToInt32(dr["EstimateCount"]) : 0,
                                     FilesCount = dr["FilesCount"] != DBNull.Value ? Convert.ToInt32(dr["FilesCount"]) : 0,
                                     FundingCount = dr["TotalFunding"] != DBNull.Value ? Convert.ToInt32(dr["TotalFunding"]) : 0,
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
                                     EstimatorCount = dr["EstimatorCount"] != DBNull.Value ? Convert.ToInt32(dr["EstimatorCount"]) : 0,
                                     OrderCount = dr["OrderCount"] != DBNull.Value ? Convert.ToInt32(dr["OrderCount"]) : 0,
                                     RecurringBillingCount = dr["RecurringBillingCount"] != DBNull.Value ? Convert.ToInt32(dr["RecurringBillingCount"]) : 0,
                                     TotalFundingCount = dr["TotalFunding"] != DBNull.Value ? Convert.ToInt32(dr["TotalFunding"]) : 0,
                                     ActiveFileStatusCount = dr["ActiveFileStatusCount"] != DBNull.Value ? Convert.ToInt32(dr["ActiveFileStatusCount"]) : 0,
                                     InActiveFileStatusCount = dr["InActiveFileStatusCount"] != DBNull.Value ? Convert.ToInt32(dr["InActiveFileStatusCount"]) : 0

                                 }).ToList();
            return CustomerTabCounts.FirstOrDefault();
        }

        public List<Customer> GetCustomersByKeyAndCompanyId(Guid CompanyId, string key, string emptag, string emprole, Guid empid, bool isPermit)
        {
            DataTable dt = _CustomerDataAccess.GetCustomersByKeyAndCompanyId(CompanyId, key, emptag, emprole, empid, isPermit);
            List<Customer> CustomerList = new List<Customer>();
            CustomerList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                AgemniId = dr["RefenrenceId"] != DBNull.Value ? Convert.ToInt32(dr["RefenrenceId"]) : 0,

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
            return _CustomerMigrationDataAccess.GetByQuery(string.Format(" [Platform] = '{0}'", PlatformName));
        }

        public CustomerMigration GetCustomerMigrationByReferenceId(string value)
        {
            return _CustomerMigrationDataAccess.GetByQuery(string.Format("RefenrenceId='{0}'", value)).FirstOrDefault();
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

        public EstimateStatusDetail GetAllEstimateStatusDetailByCustomerId(Guid Cusidval, bool? IsDeclinedAdded)
        {
            DataTable dt = _InvoiceDataAccess.GetAllEstimateStatusDetailByCustomerId(Cusidval, IsDeclinedAdded);
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
            if (CreditScore.IsSoftCheck == true)
            {
                if (CreditScore.BUREAU == "EFX")
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
            string BodyContent = "------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"ACCOUNT\"\r\n\r\n" + CreditScore.ACCOUNT + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"PASSWD\"\r\n\r\n" + CreditScore.PASSWD + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"PASS\"\r\n\r\n" + CreditScore.PASS + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"PROCESS\"\r\n\r\n" + CreditScore.PROCESS + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"BUREAU\"\r\n\r\n" + CreditScore.BUREAU + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"PRODUCT\"\r\n\r\n" + CreditScore.PRODUCT + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"NAME\"\r\n\r\n" + Name + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"SSN\"\r\n\r\n" + CreditScore.SSN + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"ADDRESS\"\r\n\r\n" + CreditScore.ADDRESS + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"CITY\"\r\n\r\n" + CreditScore.CITY + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"STATE\"\r\n\r\n" + CreditScore.STATE + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"ZIP\"\r\n\r\n" + CreditScore.ZIP + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW\r\nContent-Disposition: form-data; name=\"SELECTEDCODE\"\r\n\r\n" + CreditScore.SelectCode + "\r\n------WebKitFormBoundary7MA4YWxkTrZu0gW--";
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
        public QA2Script GetQA2ByCustomerIdAndId(int id, Guid customerid)
        {
            return _QA2ScriptDataAccess.GetByQuery(string.Format("Id ='{0}' and CustomerId ='{1}'", id, customerid)).FirstOrDefault();
        }
        public QAFinance GetQAFinanceByCustomerIdAndId(int id, Guid customerid)
        {
            return _QAFinanceDataAccess.GetByQuery(string.Format("Id ='{0}' and CustomerId ='{1}'", id, customerid)).FirstOrDefault();
        }
        public QA1Script GetQA1ByCustomerIdAndId(int id, Guid customerid)
        {
            return _QA1ScriptDataAccess.GetByQuery(string.Format("Id ='{0}' and CustomerId ='{1}'", id, customerid)).FirstOrDefault();
        }
        public List<QA2Script> GetAllQA2ByCustomerId(Guid customerid)
        {
            return _QA2ScriptDataAccess.GetByQuery(string.Format("CustomerId ='{0}'", customerid));
        }
        public List<QAFinance> GetAllQAFinanceByCustomerId(Guid customerid)
        {
            return _QAFinanceDataAccess.GetByQuery(string.Format("CustomerId ='{0}'", customerid));
        }
        public List<QA1Script> GetAllQA1ByCustomerId(Guid customerid)
        {
            return _QA1ScriptDataAccess.GetByQuery(string.Format("CustomerId ='{0}'", customerid));
        }
        public QA2Script GetQA2ById(int id)
        {
            return _QA2ScriptDataAccess.GetByQuery(string.Format("Id ='{0}'", id)).FirstOrDefault();
        }
        public QA1Script GetQA1ById(int id)
        {
            return _QA1ScriptDataAccess.GetByQuery(string.Format("Id ='{0}'", id)).FirstOrDefault();
        }
        public QAFinance GetQAFinanceById(int id)
        {
            return _QAFinanceDataAccess.GetByQuery(string.Format("Id ='{0}'", id)).FirstOrDefault();
        }
        public CustomerProratedBill GetCustomerLastProratedBillByCustomerId(Guid CustomerId)
        {
            return _CustomerProratedBillDataAccess.GetByQuery(string.Format("CustomerId ='{0}'", CustomerId)).LastOrDefault();
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
            return _CustomerDataAccess.GetCustomerByGuidId(CustomerId);
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
        public Customer GetDirectCustomerByAlarmRefId(string CustomerId)
        {
            return _CustomerDataAccess.GetByQuery(string.Format("Alarmrefid ='{0}'", CustomerId)).FirstOrDefault(); ;
        }
        public Customer GetCustomerByLeadId(int id)
        {
            return _CustomerDataAccess.GetByQuery(string.Format("Id ='{0}'", id)).FirstOrDefault();
        }
        public Company GetCompanyByCompanyId(Guid id)
        {
            return _CompanyDataAccess.GetByQuery(string.Format("CompanyId ='{0}'", id)).FirstOrDefault(); ;
        }

        public List<SecondaryCreditCheckContact> GetAllSecondaryCreditCheckContact()
        {
            return _SecondaryCreditCheckContactDataAccess.GetAll();
        }

        public SecondaryCreditCheckContact GetSecondaryCreditCheckContactById(int Id)
        {
            return _SecondaryCreditCheckContactDataAccess.Get(Id);
        }
        public List<SecondaryCreditCheckContact> GetAllSecondaryCreditCheckContactByCustomerId(Guid CustomerId)
        {
            return _SecondaryCreditCheckContactDataAccess.GetByQuery(string.Format("CustomerId ='{0}'", CustomerId)).ToList(); ;

        }

        public long DeleteSecondaryCreditCheckContact(int id)
        {
            return _SecondaryCreditCheckContactDataAccess.Delete(id);
        }

        public long InsertSecondaryCreditCheckContact(SecondaryCreditCheckContact contact)
        {
            return _SecondaryCreditCheckContactDataAccess.Insert(contact);
        }

        public long UpdateSecondaryCreditCheckContact(SecondaryCreditCheckContact contact)
        {
            return _SecondaryCreditCheckContactDataAccess.Update(contact);
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

        public bool CheckLeadOrCustomerByCustomerId(Guid CompanyId, Guid CustomerId)
        {
            DataTable dt = _CustomerDataAccess.CheckLeadOrCustomerByCustomerId(CompanyId, CustomerId);
            bool isLead = false;
            LeadOrCustomer customer = new LeadOrCustomer();
            customer = (from DataRow dr in dt.Rows
                        select new LeadOrCustomer()
                        {
                            IsLead = dr["IsCustomer"] != DBNull.Value ? Convert.ToBoolean(dr["IsCustomer"]) : false,
                        }).ToList().FirstOrDefault();
            if (customer != null)
            {
                isLead = customer.IsLead;
            }
            return isLead;

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
            customer.Id = (int)_CustomerDataAccess.Insert(customer);

            //customer.SearchText = string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9}, {10} {11} {12} {13} {14} {15}",
            //   /*{0}*/  customer.FirstName,
            //   /*{1}*/  customer.LastName,
            //   /*{1}*/  customer.BusinessName,
            //   /*{2}*/  customer.Id.ToString(),
            //   /*{3}*/  customer.MiddleName,
            //   /*{4}*/  customer.CustomerNo,
            //   /*{5}*/  customer.SecondCustomerNo,
            //   /*{6}*/  customer.Type,
            //   /*{7}*/  customer.Street,
            //   /*{8}*/  customer.ZipCode,
            //   /*{9}*/  customer.City,
            //   /*{10}*/  customer.State,
            //   /*{11}*/  customer.PrimaryPhone,
            //   /*{12}*/  customer.CellNo,
            //   /*{13}*/  customer.SecondaryPhone,
            //   /*{14}*/  customer.EmailAddress);

            //Update Search Text for newly added customer or lead
            UpdateCustomer(customer);

            return customer.Id;


        }
        public long InsertCustomerProratedBill(CustomerProratedBill cpb)
        {
            return _CustomerProratedBillDataAccess.Insert(cpb);
        }
        public long InsertCustomerExtended(CustomerExtended customer)
        {
            return _CustomerExtendedDataAccess.Insert(customer);
        }

        public bool UpdateBatchNumber(Guid customerId, string batchNumber)
        {
            return _CustomerDataAccess.UpdateBatchNumber(customerId, batchNumber);
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
        public long InsertThirdPartyAgencies(ThirdPartyAgencies agencies)
        {
            return _ThirdPartyAgenciesDataAccess.Insert(agencies);
        }
        public List<ThirdPartyAgencies> GetThirdpartyAgenciesByZipcode(string ZipCode)
        {
            return _ThirdPartyAgenciesDataAccess.GetByQuery(string.Format("ZipCode ='{0}' ", ZipCode)).ToList();
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

        public List<ZonesEquipmentTypeEventMapModel> GetEquipmentTypeEventMapByEventCode(string eventCode)
        {
            DataTable dt = _ZonesEquipmentTypeEventMapDataAccess.GetEquipmentTypeEventMapByEventCode(eventCode);
            List<ZonesEquipmentTypeEventMapModel> SecurityZones = new List<ZonesEquipmentTypeEventMapModel>();
            SecurityZones = (from DataRow dr in dt.Rows
                             select new ZonesEquipmentTypeEventMapModel()
                             {
                                 ID = dr["ID"] != DBNull.Value ? Convert.ToInt32(dr["ID"]) : 0,
                                 EquipmentTypeId = dr["EquipmentTypeId"].ToString(),
                                 EventId = dr["EventId"].ToString(),
                                 EquipTypeVal = dr["EquipTypeVal"].ToString(),


                             }).ToList();
            return SecurityZones;
            //return _ZonesEquipmentTypeEventMapDataAccess.GetByQuery(string.Format("EventId ='{0}'", eventCode)).ToList();
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
                                    AgencyNo = dr["AgencyNo"].ToString(),
                                    Agencytype = dr["Agencytype"].ToString(),
                                    Platform = dr["Platform"].ToString(),
                                    AgencytypeVal = dr["AgencytypeVal"].ToString(),
                                    PermTypeVal = dr["PermTypeVal"].ToString(),

                                }).ToList();
            return ThirdPartyAgency;



        }
        public long InsertAgemniCustomers(Customer customer)
        {
            customer.Id = (int)_CustomerDataAccess.Insert(customer);

            //customer.SearchText = string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9}, {10} {11} {12} {13} {14} {15}",
            //   /*{0}*/  customer.FirstName,
            //   /*{1}*/  customer.LastName,
            //   /*{1}*/  customer.BusinessName,
            //   /*{2}*/  customer.Id.ToString(),
            //   /*{3}*/  customer.MiddleName,
            //   /*{4}*/  customer.CustomerNo,
            //   /*{5}*/  customer.SecondCustomerNo,
            //   /*{6}*/  customer.Type,
            //   /*{7}*/  customer.Street,
            //   /*{8}*/  customer.ZipCode,
            //   /*{9}*/  customer.City,
            //   /*{10}*/  customer.State,
            //   /*{11}*/  customer.PrimaryPhone,
            //   /*{12}*/  customer.CellNo,
            //   /*{13}*/  customer.SecondaryPhone,
            //   /*{14}*/  customer.EmailAddress);

            //Update Search Text for newly added customer or lead
            UpdateCustomer(customer);

            return customer.Id;
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
        public CustomerMigration GetCustomerMigrationByReferenceId(int referenceId, string Platform)
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
            if (tickets.Count > 0)
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
        public long InsertAgreementQstn(AgreementQuestion agreementQuestion)
        {
            return _AgreementQuestionDataAccess.Insert(agreementQuestion);
        }
        public long UpdateCreditScoreGrade(CreditScoreGrade creditScoreGrade)
        {
            return _CreditScoreGradeDataAccess.Update(creditScoreGrade);
        }
        public long UpdateAgreementQstn(AgreementQuestion agreementQuestion)
        {
            return _AgreementQuestionDataAccess.Update(agreementQuestion);
        }
        public List<CreditScoreGrade> GetAllCreditScoreGrade()
        {
            return _CreditScoreGradeDataAccess.GetAll();
        }

        public List<AgreementQuestion> GetAllAgreementQstn()
        {
            return _AgreementQuestionDataAccess.GetAll();
        }
        public CreditScoreGrade GetCreditScoreGradeById(int Id)
        {
            return _CreditScoreGradeDataAccess.Get(Id);
        }
        public AgreementQuestion GetAgreementQuestionById(int Id)
        {
            return _AgreementQuestionDataAccess.Get(Id);
        }
        //public Customer GetCustomerCCpaymentInfo(Guid customerid){
        //     return _CustomerDataAccess.Get(customerid);
        // }
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

        public long DeleteAgreementQstn(int Id)
        {
            return _AgreementQuestionDataAccess.Delete(Id);
        }
        public long InsertCustomerTermination(AlarmCustomerTermination cusTerm)
        {
            return _TerminationDataAccess.Insert(cusTerm);
        }
        public long InsertQA2Script(QA2Script qa2)
        {
            return _QA2ScriptDataAccess.Insert(qa2);
        }
        public long InsertQA1Script(QA1Script qa1)
        {
            return _QA1ScriptDataAccess.Insert(qa1);
        }
        public long InsertQAFinance(QAFinance qaFinance)
        {
            return _QAFinanceDataAccess.Insert(qaFinance);
        }
        public LeadCorrespondence GetLeadCorrespondenceByCustomerId(Guid CustomerId)
        {
            return _LeadCorrespondenceDataAccess.GetByQuery(string.Format("CustomerId ='{0}'", CustomerId)).OrderByDescending(x => x.Id).ToList().FirstOrDefault();
        }
        public bool CheckCustomerQueueCancellationById(int id, Guid CustomerId)
        {
            LeadCorrespondence leadCorrespondence = _LeadCorrespondenceDataAccess.GetByQuery(string.Format("CustomerId ='{0}'", CustomerId)).OrderByDescending(x => x.Id).ToList().FirstOrDefault();
            bool result = false;
            CustomerCancellationQueue data = _CustomerCancellationQueueDataAccess.GetByQuery(string.Format("Id = {0} and CustomerId ='{1}' and IsActive = 1", id, CustomerId)).OrderByDescending(x => x.Id).ToList().FirstOrDefault();
            if (id > 0 && data != null && data.IsSigned.Value == false)
            {
                int Todays = DateTime.Now.Day;
                if (data.CreatedDate.HasValue)
                {
                    TimeSpan difference = DateTime.Now.Date - data.CreatedDate.Value.Date;
                    double difftotal = difference.TotalDays;
                    int Total = (int)Math.Truncate(difference.TotalDays);
                    if (data.ExpirationDays.HasValue)
                    {
                        if (Total > data.ExpirationDays.Value)
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                }
            }
            else
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
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;

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
        public CustomerCancellationQueue GetActiveCustomerCancellationQueueByCustomerId(Guid CustomerId)
        {
            return _CustomerCancellationQueueDataAccess.GetByQuery(string.Format("CustomerId ='{0}' and IsActive = 1", CustomerId)).OrderByDescending(x => x.Id).ToList().FirstOrDefault();
        }

        public CustomerCancellationQueue GetCustomerCancellationQueueById(int Id)
        {
            return _CustomerCancellationQueueDataAccess.Get(Id);
        }
        public long DeleteCustomerCancellationQueueById(int Id)
        {
            return _CustomerCancellationQueueDataAccess.Delete(Id);
        }
        public long InsertCustomerCancellationReason(CustomerCancellationReason customerCancellationReason)
        {
            return _CustomerCancellationReasonDataAccess.Insert(customerCancellationReason);
        }
        public List<CustomerCancellationReason> GetCustomerCancellationReasonList(Guid CustomerId)
        {
            string query = string.Format("CustomerId='{0}'", CustomerId);
            return _CustomerCancellationReasonDataAccess.GetByQuery(query).ToList();
        }
        public bool DeleteCustomerCancellationReasonByCustomerId(Guid CustomerId)
        {
            return _CustomerCancellationReasonDataAccess.DeleteCustomerCancellationReasonByCustomerId(CustomerId);
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
                                       Grade = dr["Grade"].ToString(),
                                       ReportPdfLink = dr["ReportPdfLink"].ToString(),
                                       RepontPdfName = dr["RepontPdfName"].ToString(),
                                       CustomerId = (Guid)dr["CustomerId"],
                                       CreatedBy = (Guid)dr["CreatedBy"],
                                       CreatedByVal = dr["CreatedByVal"].ToString(),
                                       Hit = dr["Hit"].ToString(),
                                       TransectionId = dr["TransectionId"].ToString(),
                                       CreditCheckDesc = dr["CreditCheckDesc"].ToString(),
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
            customer.Id = (int)_CustomerDataAccess.Insert(customer);

            //customer.SearchText = string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9}, {10} {11} {12} {13} {14} {15}",
            //   /*{0}*/  customer.FirstName,
            //   /*{1}*/  customer.LastName,
            //   /*{1}*/  customer.BusinessName,
            //   /*{2}*/  customer.Id.ToString(),
            //   /*{3}*/  customer.MiddleName,
            //   /*{4}*/  customer.CustomerNo,
            //   /*{5}*/  customer.SecondCustomerNo,
            //   /*{6}*/  customer.Type,
            //   /*{7}*/  customer.Street,
            //   /*{8}*/  customer.ZipCode,
            //   /*{9}*/  customer.City,
            //   /*{10}*/  customer.State,
            //   /*{11}*/  customer.PrimaryPhone,
            //   /*{12}*/  customer.CellNo,
            //   /*{13}*/  customer.SecondaryPhone,
            //   /*{14}*/  customer.EmailAddress);

            //Update Search Text for newly added customer or lead
            UpdateCustomer(customer);

            return customer.Id;
        }
        public bool UpdateCustomer(Customer customer)
        {
            customer.SearchText = string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9}, {10} {11} {12} {13} {14} {15} {16}",
                   /*{0}*/  customer.FirstName,
                   /*{1}*/  customer.LastName,
                   /*{2}*/  customer.BusinessName,
                   /*{3}*/  customer.Id.ToString(),
                   /*{4}*/  customer.MiddleName,
                   /*{5}*/  customer.CustomerNo,
                   /*{6}*/  customer.SecondCustomerNo,
                   /*{7}*/  customer.Type,
                   /*{8}*/  customer.Street,
                   /*{9}*/  customer.ZipCode,
                   /*{10}*/  customer.City,
                   /*{11}*/  customer.State,
                   /*{12}*/  customer.PrimaryPhone,
                   /*{13}*/  customer.CellNo,
                   /*{14}*/  customer.SecondaryPhone,
                   /*{15}*/  customer.EmailAddress,
                   /*{16}*/ customer.AgemniId.ToString());
            return _CustomerDataAccess.Update(customer) > 0;
        }
        public bool UpdateCustomerExtended(CustomerExtended customer)
        {
            return _CustomerExtendedDataAccess.Update(customer) > 0;
        }
        public bool UpdateQA2Script(QA2Script qa2)
        {
            return _QA2ScriptDataAccess.Update(qa2) > 0;
        }
        public bool UpdateQA1Script(QA1Script qa1)
        {
            return _QA1ScriptDataAccess.Update(qa1) > 0;
        }
        public bool UpdateQAFinance(QAFinance qaFinance)
        {
            return _QAFinanceDataAccess.Update(qaFinance) > 0;
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


        public List<CustomerFile> GetAllCustomerAWSFileByCustomerId(List<int> customerIDList, Guid companyID) /// Added by Mayur Rokade 19 May 2020
        {
            DataTable dt = _CustomerFileDataAccess.GetAllCustomerAWSFileCustomerId(customerIDList, companyID);

            List<CustomerFile> CustomerFileList = new List<CustomerFile>();
            CustomerFileList = (from DataRow dr in dt.Rows
                                select new CustomerFile()
                                {
                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    FileDescription = dr["FileDescription"].ToString(),
                                    Filename = dr["Filename"].ToString(),
                                    WMStatus = dr["WMStatus"].ToString(),
                                    AWSProcessStatus = dr["AWSProcessStatus"].ToString(),
                                    AWSUploadTS = dr["AWSUploadTS"] != DBNull.Value ? Convert.ToDateTime(dr["AWSUploadTS"]) : new DateTime(),
                                    CustomerIntId = (int)(dr["IntCustID"])
                                }).ToList();
            return CustomerFileList;
        }
        public List<CustomerIdList> GetCustomerReportByFilter(CustomerLiteFilter filter)
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

        public List<CustomerIdList> GetLeadReportByFilter(CustomerLiteFilter filter)
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
                                LeadSourceType = dr["LeadSourceType"].ToString(),
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
                                CreatedDateText = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]).ToString("MM/dd/yyyy") : "",
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
                                AppoinmentSetByValue = dr["AppoinmentSetByVal"].ToString(),
                                PlatformId = dr["PlatformId"].ToString(),
                                MapscoNo = dr["MapscoNo"].ToString(),
                                //ContractStartDate = dr["ContractStartDate"] != DBNull.Value ? Convert.ToDateTime(dr["ContractStartDate"]) : new DateTime(),
                                //RemainingContractTerm = dr["RemainingContractTerm"].ToString()
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
        public MarginReportCustom GetAllMarginReport(DateTime? start, DateTime? end, string searchtext, int pageno, int pagesize, string order)
        {
            MarginReportCustom Model = new MarginReportCustom();
            DataSet ds = _CustomerDataAccess.GetAllMarginReport(start, end, searchtext, pageno, pagesize, order);
            Model.CustomerModel = new List<Customer>();
            if (ds != null)
            {
                Model.CustomerModel = (from DataRow dr in ds.Tables[0].Rows
                                       select new Customer()
                                       {
                                           Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                           FirstName = dr["customerName"].ToString(),
                                           BusinessName = dr["BusinessName"].ToString(),
                                           StreetPrevious = dr["StreetPrevious"].ToString(),
                                           CityPrevious = dr["CityPrevious"].ToString(),
                                           StatePrevious = dr["StatePrevious"].ToString(),
                                           ZipCodePrevious = dr["ZipCodePrevious"].ToString(),
                                           Street = dr["Street"].ToString(),
                                           City = dr["City"].ToString(),
                                           State = dr["State"].ToString(),
                                           ZipCode = dr["ZipCode"].ToString(),
                                           CustomerNo = dr["CustomerNo"].ToString(),
                                           CSProvider = dr["CSProvider"].ToString(),
                                           ContractStartDate = dr["ContractStartDate"] != DBNull.Value ? Convert.ToDateTime(dr["ContractStartDate"]) : new DateTime(),
                                           CustomerSinceDate = dr["CustomerSince"] != DBNull.Value ? Convert.ToDateTime(dr["CustomerSince"]) : new DateTime(),
                                           CancellationDate = dr["CancellationDate"] != DBNull.Value ? Convert.ToDateTime(dr["CancellationDate"]) : new DateTime(),
                                           ContractTeam = dr["ContractTeam"].ToString(),
                                           RenewalTerm = dr["RenewalTerm"] != DBNull.Value ? Convert.ToInt32(dr["RenewalTerm"]) : 0,
                                           Type = dr["Type"].ToString(),
                                           CreditScore = dr["CreditScore"].ToString(),
                                           BillAmount = dr["BillAmount"] != DBNull.Value ? Convert.ToDouble(dr["BillAmount"]) : 0.0,
                                           ContractValue = dr["ContractValue"].ToString(),
                                           PurchasePrice = dr["PurchasePrice"] != DBNull.Value ? Convert.ToDouble(dr["PurchasePrice"]) : 0.0,
                                           Ownership = dr["Ownership"].ToString(),
                                           MonthlyBatch = dr["MonthlyBatch"] != DBNull.Value ? Convert.ToInt32(dr["MonthlyBatch"]) : 0,
                                           WeeklyBatch = dr["Batch"].ToString(),
                                           SalesPerson = dr["SalesPerson"].ToString(),
                                           PanelType = dr["PanelType"].ToString(),
                                           Replacement = dr["Replacement"] != DBNull.Value ? Convert.ToBoolean(dr["Replacement"]) : false,
                                           FCReplacement = dr["FCReplacement"] != DBNull.Value ? Convert.ToBoolean(dr["FCReplacement"]) : false,
                                           Transfer = dr["Transfer"] != DBNull.Value ? Convert.ToBoolean(dr["Transfer"]) : false,
                                           NOPOO = dr["NOPOO"] != DBNull.Value ? Convert.ToBoolean(dr["NOPOO"]) : false,
                                           CellSerialNo = dr["CellSerialNo"].ToString(),
                                           CellBackupCompany = dr["CellBackupCompany"].ToString()
                                       }).ToList();

                Model.TotalCount = ds.Tables[1].Rows[0]["Total"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["Total"]) : 0;

            }
            return Model;
        }

        public CustomerListWithCountModel GetCustomerByFilterAuditReccuring(CustomerFilter filter)
        {
            DataSet ds = _CustomerDataAccess.GetCustomerByFilterAuditReccuring(filter);

            List<Customer> CustomerList = new List<Customer>();
            CustomerList = (from DataRow dr in ds.Tables[0].Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                DisplayName = dr["DisplayName"].ToString(),
                                CustomerNo = dr["CustomerNo"].ToString(),
                                BillAdd1 = dr["BillAdd1"].ToString(),
                                BillCity = dr["BillCity"].ToString().UppercaseFirst(),
                                BillState = dr["BillState"].ToString(),
                                BillZip = dr["BillZip"].ToString(),
                                CardExpireDate = dr["CardExpireDate"].ToString(),
                                BankAccountName = dr["BankAccountName"].ToString(),
                                CardAccountName = dr["CardAccountName"].ToString(),
                                AutoBank = dr["AutoBank"].ToString(),
                                AutoCC = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["AutoCC"].ToString()),
                                BillingMethodType = dr["BillingMethodType"].ToString(),
                                RoutingNo = dr["RoutingNo"].ToString(),
                                BillEmailAddress = dr["BillEmailAddress"].ToString(),
                                RMRStartDate = dr["RMRStartDate"] != DBNull.Value ? Convert.ToDateTime(dr["RMRStartDate"]) : new DateTime(),
                                RMRLastBillDate = dr["RMRLastBillDate"] != DBNull.Value ? Convert.ToDateTime(dr["RMRLastBillDate"]) : new DateTime(),
                                RMRNextBillDate = dr["RMRNextBillDate"] != DBNull.Value ? Convert.ToDateTime(dr["RMRNextBillDate"]) : new DateTime(),
                                RMRProductName = dr["RMRProductName"].ToString(),
                                RMRAmount = dr["RMRAmount"] != DBNull.Value ? Convert.ToDouble(dr["RMRAmount"]) : 0,
                                RMRBillDay = dr["RMRBillDay"].ToString(),
                                RMRBillCycle = dr["RMRBillCycle"].ToString(),
                                RMRCycleStartDate = dr["RMRCycleStartDate"] != DBNull.Value ? Convert.ToDateTime(dr["RMRCycleStartDate"]) : new DateTime(),
                                RMREffectiveDate = dr["RMREffectiveDate"] != DBNull.Value ? Convert.ToDateTime(dr["RMREffectiveDate"]) : new DateTime(),

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
                                RenewalTerm = dr["RenewalTerm"] != DBNull.Value ? Convert.ToInt32(dr["RenewalTerm"]) : (int?)null,
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
                                LeadSourceType = dr["LeadSourceType"].ToString(),
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
                                CreatedDateText = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]).ToString("MM/dd/yyyy") : "",
                                LastUpdatedByUid = (Guid)dr["LastUpdatedByUid"],
                                BusinessAccountType = dr["CSBankAccountType"].ToString(),
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
                                AppoinmentSetByValue = dr["AppoinmentSetByVal"].ToString(),
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
        public CustomerMigration GetCustomerMigrationByCustomerId(Guid customerId)
        {
            return _CustomerMigrationDataAccess.GetByQuery(string.Format(" CustomerId = '{0}' ", customerId)).FirstOrDefault();
        }

        public CustomerTableWithCount GetCustomerByLiteFilter(CustomerLiteFilter filter)
        {
            bool isNew = true;
            DataSet ds = new DataSet();
            if (isNew)
            {
                ds = _CustomerDataAccess.GetCustomerListByLiteFilterNew(filter);
            }
            else
            {
                ds = _CustomerDataAccess.GetCustomerListByLiteFilter(filter);
            }


            TotalCustomerCount TotalCustomer = new TotalCustomerCount();
            TotalCustomer = (from DataRow dr in ds.Tables[1].Rows
                             select new TotalCustomerCount()
                             {
                                 Counter = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                             }).FirstOrDefault();

            var totalCountActive = ds != null && ds.Tables.Count > 1 ? ds.Tables[1] : null;



            CustomerTableWithCount customerTableCount = new CustomerTableWithCount();
            customerTableCount.CustomerList = ds.Tables[0];
            customerTableCount.TotalCustomerCount = TotalCustomer;

            List<CustomerHeaderMoneyBar> CustomerHeaderMoneyBarModel = new List<CustomerHeaderMoneyBar>();
            DataTable dtCount = ds.Tables.Count > 2 ? ds.Tables[2] : null;
            if (dtCount != null && dtCount.Rows.Count > 0)
                CustomerHeaderMoneyBarModel = (from DataRow dr in dtCount.Rows
                                               select new CustomerHeaderMoneyBar()
                                               {
                                                   CustomerCount = !string.IsNullOrWhiteSpace(dr["CustomerCount"].ToString()) ? (dr["CustomerCount"]).ToString() : "0",
                                                   TotalRMR = !string.IsNullOrWhiteSpace(dr["TotalRMR"].ToString()) ? (dr["TotalRMR"]).ToString() : "0",
                                                   TotalRMRCount = !string.IsNullOrWhiteSpace(dr["TotalRMRCount"].ToString()) ? (dr["TotalRMRCount"]).ToString() : "0",
                                                   DueAmount = !string.IsNullOrWhiteSpace(dr["DueAmount"].ToString()) ? (dr["DueAmount"]).ToString() : "0",
                                                   EstimateAmount = !string.IsNullOrWhiteSpace(dr["EstimateAmount"].ToString()) ? (dr["EstimateAmount"]).ToString() : "0",
                                                   OrderCount = dr["OrderCount"] != DBNull.Value ? Convert.ToInt32(dr["OrderCount"]) : 0,
                                                   OrderValue = dr["OrderValue"] != DBNull.Value ? Convert.ToDouble(dr["OrderValue"]) : 0,
                                               }).ToList();

            customerTableCount.CustomerHeaderMoneyBar = CustomerHeaderMoneyBarModel.FirstOrDefault();

            return customerTableCount;
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
                                LeadSourceType = dr["LeadSourceType"].ToString(),
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
                                AppoinmentSetByValue = dr["AppoinmentSetByVal"].ToString(),
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

        public CustomerNote GetCustomerNoteByCustomerIdAndIsPrimary(Guid customerid)
        {
            return _CustomerNoteDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and IsPrimaryNote = 1", customerid)).FirstOrDefault();
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
        public TaskModel GetAllTaskByFilter(TicketFilter Filters, FilterReportModel filter, string Start, string End, Guid ComId, Guid empId, bool allTask)
        {
            TaskModel Model = new TaskModel();
            DataSet ds = _CustomerDataAccess.GetTaskReportByFilter(Filters, filter, Start, End, ComId, empId, allTask);
            Model.TaskList = (from DataRow dr in ds.Tables[0].Rows
                              select new CustomerNote()
                              {
                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                  Notes = dr["Notes"].ToString(),
                                  NoteTypeValue = dr["NoteTypeValue"].ToString(),
                                  Color = dr["Color"].ToString(),
                                  ReminderDate = dr["ReminderDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReminderDate"]) : new DateTime(),
                                  ReminderEndDate = dr["ReminderEndDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReminderEndDate"]) : new DateTime(),
                                  CustomerId = (Guid)dr["CustomerId"],
                                  CompanyId = (Guid)dr["CompanyId"],
                                  CreatedByUid = (Guid)dr["CreatedByUid"],
                                  CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                  IsEmail = dr["IsEmail"] != DBNull.Value ? Convert.ToBoolean(dr["IsEmail"]) : false,
                                  IsText = dr["IsText"] != DBNull.Value ? Convert.ToBoolean(dr["IsText"]) : false,
                                  IsShedule = dr["IsShedule"] != DBNull.Value ? Convert.ToBoolean(dr["IsShedule"]) : false,
                                  IsFollowUp = dr["IsFollowUp"] != DBNull.Value ? Convert.ToBoolean(dr["IsFollowUp"]) : false,
                                  IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                                  IsClose = dr["IsClose"] != DBNull.Value ? Convert.ToBoolean(dr["IsClose"]) : false,
                                  IsAllDay = dr["IsAllDay"] != DBNull.Value ? Convert.ToBoolean(dr["IsAllDay"]) : false,
                                  IsPin = dr["IsPin"] != DBNull.Value ? Convert.ToBoolean(dr["IsPin"]) : false,
                                  CreatedBy = dr["CreatedBy"].ToString(),
                                  empName = dr["empName"].ToString(),
                                  AssignName = dr["AssignName"].ToString().TrimEnd(' ', ','),
                                  IsOverview = dr["IsOverview"] != DBNull.Value ? Convert.ToBoolean(dr["IsOverview"]) : false
                                  //TotalNoteCount = dr["TotalNoteCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalNoteCount"]) : 0
                              }).ToList();
            Model.TotalCount = ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;

            return Model;
            // return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerId));
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

        public LeadTabCountModel GetAllLeadTabCountByCompanyId(Guid companyID, bool ShowBookingCount, bool ShowEstimateCount)
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
        public int GetTotalUnassignedLeadsCount(Guid CompanyId, DateTime Start, DateTime End)
        {
            int count = 0;
            DataTable dt = _CustomerDataAccess.GetTotalUnassignedLeadsCount(CompanyId, Start, End);
            if (dt != null && dt.Rows.Count > 0)
            {
                count = dt.Rows[0]["Total"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Total"]) : 0;
            }
            return count;
        }
        public KnowledgebaseFavouriteUser GetFavoriteArticleByUserId(Guid UserId, int LeadId)
        {
            return _KnowledgebaseFavouriteUserDataAccess.GetByQuery(string.Format("UserId = '{0}' and KnowledgebaseId = {1}", UserId, LeadId)).FirstOrDefault();
        }
        public bool UpdateFavoriteUserArticle(KnowledgebaseFavouriteUser fav)
        {
            return _KnowledgebaseFavouriteUserDataAccess.Update(fav) > 0;
        }
        public long InsertKnowledgebaseFavouriteUser(KnowledgebaseFavouriteUser model)
        {
            return _KnowledgebaseFavouriteUserDataAccess.Insert(model);
        }
        public KnowledgebaseFavouriteUser GetKnowledgebaseFavouriteUser(int KnowledgeId, Guid UserId)
        {
            return _KnowledgebaseFavouriteUserDataAccess.GetByQuery(string.Format("KnowledgebaseId = {0} and UserId = '{1}' order by id desc", KnowledgeId, UserId)).FirstOrDefault();
        }
        public double GetLeadTotalRMRByCustomerId(Guid customerid)
        {
            string count = "0";
            double TotalRMR = 0;

            DataTable dt = _CustomerDataAccess.GetLeadTotalRMRByCustomerId(customerid);
            if (dt.Rows.Count > 0)
            {
                count = !string.IsNullOrWhiteSpace(dt.Rows[0]["TotalRMR"].ToString()) ? dt.Rows[0]["TotalRMR"].ToString() : "0";
            }
            double.TryParse(count, out TotalRMR);

            return TotalRMR;

        }
        public double GetCollectedAmountByCustomerId(Guid customerid)
        {
            string count = "0";
            double CollectedAmount = 0;

            DataTable dt = _CustomerDataAccess.GetCollectedAmountByCustomerId(customerid);
            if (dt.Rows.Count > 0)
            {
                count = !string.IsNullOrWhiteSpace(dt.Rows[0]["CollectedAmount"].ToString()) ? dt.Rows[0]["CollectedAmount"].ToString() : "0";
            }
            double.TryParse(count, out CollectedAmount);

            return CollectedAmount;

        }
        public Employee GetSalesGroupAndEmpNamBySoldby(Guid soldby)
        {
            //string salesGroup = "";
            ///double TotalRMR = 0;

            DataTable dt = _CustomerDataAccess.GetSalesGroupBySoldby(soldby);
            Employee employee = new Employee();
            employee = (from DataRow dr in dt.Rows
                        select new Employee()
                        {
                            EMPName = dr["SalesPerson"].ToString(),
                            PermissionGroupName = dr["SalesGroup"].ToString(),
                            Email = dr["Email"].ToString(),
                            Phone = dr["Phone"].ToString(),
                            UserName = dr["UserName"].ToString(),
                        }).FirstOrDefault();
            return employee;
            //double.TryParse(count, out TotalRMR);

            //return salesGroup;

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

        public string GetMonthlyFeeFromContractByCustomerId(Guid customerId, Guid comId)
        {
            string count = "0";
            DataTable dt = _CustomerDataAccess.GetMonthlyFeeFromContractByCustomerId(customerId, comId);
            if (dt.Rows.Count > 0)
            {
                count = dt.Rows[0]["MonthlyFee"].ToString();
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
                            PaymentMethodVal = dr["PaymentMethodVal"].ToString()
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
                           CusId = dr["CusId"] != DBNull.Value ? Convert.ToInt32(dr["CusId"]) : 0,
                           customerFirstName = dr["FirstName"].ToString(),
                           customerLastName = dr["LastName"].ToString(),
                           customerAddress = dr["CustomerAddress"].ToString(),
                           customerStreet = dr["CustomerStreet"].ToString(),
                           customerCity = dr["CustomerCity"].ToString(),
                           customerState = dr["CustomerState"].ToString(),
                           customerZipCode = dr["CustomerZipCode"].ToString(),
                           customerEmail = dr["CustomerEmailAddress"].ToString(),
                           customerPrimaryPhone = dr["PrimaryPhone"].ToString(),
                           customerSecondaryPhone = dr["SecondaryPhone"].ToString(),
                           companyAddress = dr["CompanyAddress"].ToString(),
                           companyName = dr["CompanyName"].ToString(),
                           companyLogo = dr["CompanyLogo"].ToString(),
                           companyCity = dr["CompanyCity"].ToString(),
                           companyStreet = dr["CompanyStreet"].ToString(),
                           companyState = dr["CompanyState"].ToString(),
                           companyZipCode = dr["CompanyZipCode"].ToString(),
                           companyEmail = dr["CompanyEmailAddress"].ToString(),
                           companyFax = dr["Fax"].ToString(),
                           companyPhone = dr["Phone"].ToString(),
                           companyWebsite = dr["Website"].ToString()
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
        //public DataTable GetAllLeadsByCompany(Guid CompanyId)
        //{
        //    return _CustomerDataAccess.GetAllleadByCompany(CompanyId);
        //}
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
        public List<Customer> IsCustomerBrinksExistCheck(string BrinksRefId)
        {

            DataTable dt = _CustomerDataAccess.IsCustomerBrinksExistCheck(BrinksRefId);

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
                            IsEqpExist = dr["IsEqpExist"] != DBNull.Value ? Convert.ToBoolean(dr["IsEqpExist"]) : false,
                            IsPackageEqp = dr["IsPackageEqp"] != DBNull.Value ? Convert.ToBoolean(dr["IsPackageEqp"]) : false,

                            DiscountInAmount = dr["DiscountInAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountInAmount"]) : 0.0,
                            DiscountPercent = dr["DiscountPercent"] != DBNull.Value ? Convert.ToDouble(dr["DiscountPercent"]) : 0.0


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
                    try
                    {
                        forteResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FortePaymentResponse>(finalResponse[index]);
                        response.ErrorMessage = forteResponse.response.response_desc;
                    }
                    catch (Exception)
                    {
                        response.Result = false;
                        response.ErrorMessage = response.Massege;
                    }
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
        public long UpdatePaymentProfileCustomer(PaymentProfileCustomer ppc)
        {
            return _PaymentProfileCustomerDataAccess.Update(ppc);
        }
        public List<PaymentProfileCustomer> GetAllPaymentProfileByCustomerId(Guid customerid, Guid companyid)
        {
            return _PaymentProfileCustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}'", customerid, companyid)).ToList();
        }
        public List<PaymentProfileCustomer> GetAllPaymentProfileByType(Guid customerid, Guid companyid, string type, bool AllowOnlyACHAndCC = false)
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
            model.EstimatorCount = ds.Tables[6].Rows[0][0] != DBNull.Value ? Convert.ToInt32(ds.Tables[6].Rows[0][0]) : 0;
            model.LogCount = ds.Tables[7].Rows[0][0] != DBNull.Value ? Convert.ToInt32(ds.Tables[7].Rows[0][0]) : 0;

            return model;
        }

        public CustomerCompany GetCustomerCompanyByCompanyIdAndCustomerId(Guid companyid, Guid customerid)
        {
            return _CustomerCompanyDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and CustomerId = '{1}'", companyid, customerid)).FirstOrDefault();
        }

        public DelinquentReportModel GetAllDelinquentCustomerByCompany(Guid companyId, DateTime? Start, DateTime? End, int pageno, int pagesize, string id, string searchtext, string unpaid, string order)
        {
            DataSet ds = _CustomerDataAccess.GetAllDelinquentCustomerByCompany(companyId, Start, End, pageno, pagesize, id, searchtext, unpaid, order);
            DataTable dt = ds.Tables[0];
            DataTable dt1 = ds.Tables[1];
            DataTable dt2 = ds.Tables[2];
            List<DelinquentCustomerModel> propertyList = new List<DelinquentCustomerModel>();
            DelinquentReportModel DelinquentReportModel = new DelinquentReportModel();
            UnpaiAmount UnpaiAmount = new UnpaiAmount();
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
            UnpaiAmount = (from DataRow dr in dt1.Rows
                           select new UnpaiAmount()
                           {
                               TotalUnpaid = dr["TotalUnpaid"] != DBNull.Value ? Convert.ToDouble(dr["TotalUnpaid"]) : 0.0
                           }).FirstOrDefault();
            int ToatalCustomer = 0;
            if (dt2 != null && dt.Rows.Count > 0)
                ToatalCustomer = dt2.Rows[0]["TotalCustomer"] != DBNull.Value ? Convert.ToInt32(dt2.Rows[0]["TotalCustomer"]) : 0;

            DelinquentReportModel.DelinquentCustomerModel = propertyList;
            DelinquentReportModel.UnpaidAmount = UnpaiAmount;
            DelinquentReportModel.ToatalCustomer = ToatalCustomer;

            return DelinquentReportModel;
        }
        public List<DelinquentTestCustomerModel> GetAllTestAccountByCompany(Guid companyId, DateTime? Start, DateTime? End, int pageno, int pagesize, FilterReportModel filter, string order)
        {
            DataTable dt = _CustomerDataAccess.GetAllTestAccountByCompany(companyId, Start, End, pageno, pagesize, filter, order);
            List<DelinquentTestCustomerModel> propertyList = new List<DelinquentTestCustomerModel>();
            propertyList = (from DataRow dr in dt.Rows
                            select new DelinquentTestCustomerModel()
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
                                Appartment = dr["Appartment"].ToString(),
                                UnlinkCustomer = dr["UnlinkCustomer"] != DBNull.Value ? Convert.ToBoolean(dr["UnlinkCustomer"]) : false,
                                TransferCustomerId = dr["TransferCustomerId"] != DBNull.Value ? Convert.ToInt32(dr["TransferCustomerId"]) : 0,
                                OldCustomer = dr["oldcustomer"].ToString()

                            }).ToList();
            return propertyList;
        }
        public List<DelinquentCustomerModel> GetAllTransferCustomerByCompany(Guid companyId, DateTime? Start, DateTime? End, int pageno, int pagesize, FilterReportModel filter, string order)
        {
            DataTable dt = _CustomerDataAccess.GetAllTransferCustomerByCompany(companyId, Start, End, pageno, pagesize, filter, order);
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
                                Appartment = dr["Appartment"].ToString(),
                                UnlinkCustomer = dr["UnlinkCustomer"] != DBNull.Value ? Convert.ToBoolean(dr["UnlinkCustomer"]) : false,
                                TransferCustomerId = dr["TransferCustomerId"] != DBNull.Value ? Convert.ToInt32(dr["TransferCustomerId"]) : 0,
                                OldCustomer = dr["oldcustomer"].ToString()

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
        public PackageCustomerModel GetSalesSummaryReportALLByCompanyId(Guid companyId, DateTime? start, DateTime? end, string searchtext, int pageno, int pagesize, string order)
        {
            PackageCustomerModel Model = new PackageCustomerModel();
            DataSet ds = _CustomerDataAccess.GetSalesSummaryReportALLByCompanyId(companyId, start, end, searchtext, pageno, pagesize, order);
            Model.packageCustomers = new List<PackageCustomer>();
            if (ds != null)
                Model.packageCustomers = (from DataRow dr in ds.Tables[1].Rows
                                          select new PackageCustomer()
                                          {
                                              Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                              CustomerNum = dr["CustomerNum"].ToString(),
                                              AdditionFee = dr["AdditionFee"] != DBNull.Value ? Convert.ToDouble(dr["AdditionFee"]) : 0,
                                              FirstMonths = dr["FirstMonths"] != DBNull.Value ? Convert.ToDouble(dr["FirstMonths"]) : 0,
                                              Tax = dr["Tax"] != DBNull.Value ? Convert.ToDouble(dr["Tax"]) : 0,
                                              SalesTax = dr["SalesTax"] != DBNull.Value ? Convert.ToDouble(dr["SalesTax"]) : 0,
                                              EquipmentAmount = dr["EquipmentAmount"] != DBNull.Value ? Convert.ToDouble(dr["EquipmentAmount"]) : 0,
                                              ServiceFee = dr["ServiceFee"] != DBNull.Value ? Convert.ToDouble(dr["ServiceFee"]) : 0,
                                              AdvancedMonitoring = dr["AdvancedMonitoring"] != DBNull.Value ? Convert.ToDouble(dr["AdvancedMonitoring"]) : 0,
                                          }).ToList();
            Model.Totalcount = ds.Tables[0].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["TotalCount"]) : 0;
            Model.TotalAdditionFee = ds.Tables[2].Rows[0]["TotalAdditionFee"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalAdditionFee"]) : 0.0;
            Model.TotalFirstMonth = ds.Tables[2].Rows[0]["TotalFirstMonth"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalFirstMonth"]) : 0.0;
            Model.TotalEquipmentAmount = ds.Tables[2].Rows[0]["TotalEquipmentAmount"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalEquipmentAmount"]) : 0.0;
            Model.TotalServiceFee = ds.Tables[2].Rows[0]["TotalServiceFee"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalServiceFee"]) : 0.0;
            Model.TotalAdvancedMonitoring = ds.Tables[2].Rows[0]["TotalAdvancedMonitoring"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalAdvancedMonitoring"]) : 0.0;
            Model.TotalTax = ds.Tables[2].Rows[0]["TotalTax"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalTax"]) : 0.0;
            Model.TotalSalesTax = ds.Tables[2].Rows[0]["TotalSalesTax"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalSalesTax"]) : 0.0;

            Model.SumTotalAdditionFee = ds.Tables[3].Rows[0]["TotalAdditionFee"] != DBNull.Value ? Convert.ToDouble(ds.Tables[3].Rows[0]["TotalAdditionFee"]) : 0.0;
            Model.SumTotalFirstMonth = ds.Tables[3].Rows[0]["TotalFirstMonth"] != DBNull.Value ? Convert.ToDouble(ds.Tables[3].Rows[0]["TotalFirstMonth"]) : 0.0;
            Model.SumTotalEquipmentAmount = ds.Tables[3].Rows[0]["TotalEquipmentAmount"] != DBNull.Value ? Convert.ToDouble(ds.Tables[3].Rows[0]["TotalEquipmentAmount"]) : 0.0;
            Model.SumTotalServiceFee = ds.Tables[3].Rows[0]["TotalServiceFee"] != DBNull.Value ? Convert.ToDouble(ds.Tables[3].Rows[0]["TotalServiceFee"]) : 0.0;
            Model.SumTotalAdvancedMonitoring = ds.Tables[3].Rows[0]["TotalAdvancedMonitoring"] != DBNull.Value ? Convert.ToDouble(ds.Tables[3].Rows[0]["TotalAdvancedMonitoring"]) : 0.0;
            Model.SumTotalTax = ds.Tables[3].Rows[0]["TotalTax"] != DBNull.Value ? Convert.ToDouble(ds.Tables[3].Rows[0]["TotalTax"]) : 0.0;
            Model.SumTotalSalesTax = ds.Tables[3].Rows[0]["TotalSalesTax"] != DBNull.Value ? Convert.ToDouble(ds.Tables[3].Rows[0]["TotalSalesTax"]) : 0.0;
            Model.SumTotalWoTax = ds.Tables[3].Rows[0]["TotalWoTax"] != DBNull.Value ? Convert.ToDouble(ds.Tables[3].Rows[0]["TotalWoTax"]) : 0.0;

            Model.pageno = pageno;
            Model.pagesize = pagesize;


            return Model;
            // return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerId));
        }
        public NewSalesCustomerModel GetNewSalesReportALLByInvoices(Guid companyId, DateTime? start, DateTime? end, string searchtext, int pageno, int pagesize, string order,
            List<string> SalesList, string SalesLocationList, string InvoiceTypeList, string LeadSourceList, string LeadSourceTypeList, DateTime? InstallFrom, DateTime? InstallTo)
        {
            NewSalesCustomerModel Model = new NewSalesCustomerModel();
            DataSet ds = _CustomerDataAccess.GetNewSalesReportALLByInvoices2(companyId, start, end, searchtext, pageno, pagesize, order, SalesList,
                SalesLocationList, InvoiceTypeList, LeadSourceList, LeadSourceTypeList, InstallFrom, InstallTo);
            Model.NewSalesCustomer = new List<NewSalesCustomer>();
            if (ds != null)
                Model.NewSalesCustomer = (from DataRow dr in ds.Tables[0].Rows
                                          select new NewSalesCustomer()
                                          {
                                              Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                              DisplayName = dr["DisplayName"].ToString(),
                                              CustomerNo = dr["CustomerNo"].ToString(),
                                              SalesDate = dr["SalesDate"] != DBNull.Value ? Convert.ToDateTime(dr["SalesDate"]) : new DateTime(),
                                              SalesPerson = dr["SalesPerson"].ToString(),
                                              TicketType = dr["TicketType"].ToString(),
                                              LeadStatus = dr["LeadStatus"].ToString(),
                                              LeadSource = dr["LeadSource"].ToString(),
                                              SalesLocation = dr["SalesLocation"].ToString(),
                                              Type = dr["Type"].ToString(),
                                              ActNonFee = dr["Actv"] != DBNull.Value ? Convert.ToDouble(dr["Actv"]) : 0,
                                              RMR = dr["RMR"] != DBNull.Value ? Convert.ToDouble(dr["RMR"]) : 0,
                                              EquipmentFee = dr["Equp"] != DBNull.Value ? Convert.ToDouble(dr["Equp"]) : 0,
                                              DiscAmt = dr["TotalDisc"] != DBNull.Value ? Convert.ToDouble(dr["TotalDisc"]) : 0,
                                              ServiceFee = dr["Serv"] != DBNull.Value ? Convert.ToDouble(dr["Serv"]) : 0,
                                              AdvancedMonitoring = dr["AdvM"] != DBNull.Value ? Convert.ToDouble(dr["AdvM"]) : 0,
                                              TotalTax = dr["Tax"] != DBNull.Value ? Convert.ToDouble(dr["Tax"]) : 0,
                                              TotalWoTax = dr["SubTotal"] != DBNull.Value ? Convert.ToDouble(dr["SubTotal"]) : 0,
                                              FinancedAmount = dr["FinancedAmount"] != DBNull.Value ? Convert.ToDouble(dr["FinancedAmount"]) : 0,
                                              TotalSales = dr["Total"] != DBNull.Value ? Convert.ToDouble(dr["Total"]) : 0,
                                              CollectedRMR = dr["CollectedRMR"] != DBNull.Value ? Convert.ToDouble(dr["CollectedRMR"]) : 0
                                          }).ToList();
            Model.Totalcount = ds.Tables[1].Rows[0]["CountTotal"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["CountTotal"]) : 0;
            Model.CustomerCount = ds.Tables[1].Rows[0]["CustomerCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["CustomerCount"]) : 0;
            Model.SumActNonFee = ds.Tables[1].Rows[0]["SumActNonFeeTotal"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumActNonFeeTotal"]) : 0.0;
            Model.SumRMRTotal = ds.Tables[1].Rows[0]["SumRMRTotal"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumRMRTotal"]) : 0.0;
            Model.SumEquipmentTotal = ds.Tables[1].Rows[0]["SumEquipmentFeeTotal"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumEquipmentFeeTotal"]) : 0.0;
            Model.SumServiceFeeTotal = ds.Tables[1].Rows[0]["SumServiceFeeTotal"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumServiceFeeTotal"]) : 0.0;
            Model.SumAdvanceMonitoringTotal = ds.Tables[1].Rows[0]["SumAdvancedMonitoringTotal"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumAdvancedMonitoringTotal"]) : 0.0;
            Model.SumTotalwoTax = ds.Tables[1].Rows[0]["SumTotalWoTax"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumTotalWoTax"]) : 0.0;
            Model.SumTotalTax = ds.Tables[1].Rows[0]["SumTaxTotal"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumTaxTotal"]) : 0.0;
            Model.SumTotalSales = ds.Tables[1].Rows[0]["SumTotalSales"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumTotalSales"]) : 0.0;
            Model.SumFinancedAmount = ds.Tables[1].Rows[0]["SumFinancedAmount"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumFinancedAmount"]) : 0.0;


            Model.TotalActNonFee = ds.Tables[2].Rows[0]["TotalActNonFee"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalActNonFee"]) : 0.0;
            Model.TotalRMR = ds.Tables[2].Rows[0]["TotalRMR"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalRMR"]) : 0.0;
            Model.TotalEquipmentFee = ds.Tables[2].Rows[0]["TotalEquipmentFee"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalEquipmentFee"]) : 0.0;
            Model.TotalDiscEquipFee = ds.Tables[2].Rows[0]["TotalDiscEquipFee"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalDiscEquipFee"]) : 0.0;
            Model.TotalServiceFee = ds.Tables[2].Rows[0]["TotalServiceFee"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalServiceFee"]) : 0.0;
            Model.TotalAdvancedMonitoring = ds.Tables[2].Rows[0]["TotalAdvancedMonitoring"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalAdvancedMonitoring"]) : 0.0;
            Model.TotalTotalTax = ds.Tables[2].Rows[0]["TotalTotalTax"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalTotalTax"]) : 0.0;
            Model.TotalWoTax = ds.Tables[2].Rows[0]["TotalWoTax"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalWoTax"]) : 0.0;
            Model.FinancedAmount = ds.Tables[2].Rows[0]["FinancedAmount"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["FinancedAmount"]) : 0.0;
            Model.TotalSales = ds.Tables[2].Rows[0]["TotalSales"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalSales"]) : 0.0;
            Model.TotalCollectedRMR = ds.Tables[2].Rows[0]["TotalCollectedRMR"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalCollectedRMR"]) : 0.0;
            Model.pageno = pageno;
            Model.pagesize = pagesize;

            return Model;
        }
        public NewSalesCustomerModel GetNewSalesReportALLByCompanyId(Guid companyId, DateTime? start, DateTime? end, string searchtext, int pageno, int pagesize, string order, List<string> SalesList)
        {
            NewSalesCustomerModel Model = new NewSalesCustomerModel();
            DataSet ds = _CustomerDataAccess.GetNewSalesReportALLByCompanyId(companyId, start, end, searchtext, pageno, pagesize, order, SalesList);
            Model.NewSalesCustomer = new List<NewSalesCustomer>();
            if (ds != null)
                Model.NewSalesCustomer = (from DataRow dr in ds.Tables[0].Rows
                                          select new NewSalesCustomer()
                                          {
                                              Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                              DisplayName = dr["DisplayName"].ToString(),
                                              CustomerNo = dr["CustomerNo"].ToString(),
                                              SalesDate = dr["SalesDate"] != DBNull.Value ? Convert.ToDateTime(dr["SalesDate"]) : new DateTime(),
                                              SalesPerson = dr["SalesPerson"].ToString(),
                                              TicketType = dr["TicketType"].ToString(),
                                              LeadStatus = dr["LeadStatus"].ToString(),
                                              LeadSource = dr["LeadSource"].ToString(),
                                              SalesLocation = dr["SalesLocation"].ToString(),
                                              Type = dr["Type"].ToString(),
                                              ActNonFee = dr["ActNonFee"] != DBNull.Value ? Convert.ToDouble(dr["ActNonFee"]) : 0,
                                              RMR = dr["RMR"] != DBNull.Value ? Convert.ToDouble(dr["RMR"]) : 0,
                                              EquipmentFee = dr["EquipmentFee"] != DBNull.Value ? Convert.ToDouble(dr["EquipmentFee"]) : 0,
                                              ServiceFee = dr["ServiceFee"] != DBNull.Value ? Convert.ToDouble(dr["ServiceFee"]) : 0,
                                              AdvancedMonitoring = dr["AdvancedMonitoring"] != DBNull.Value ? Convert.ToDouble(dr["AdvancedMonitoring"]) : 0,
                                              TotalTax = dr["TotalTax"] != DBNull.Value ? Convert.ToDouble(dr["TotalTax"]) : 0,
                                              FinancedAmount = dr["FinancedAmount"] != DBNull.Value ? Convert.ToDouble(dr["FinancedAmount"]) : 0,
                                              TotalSales = dr["TotalSales"] != DBNull.Value ? Convert.ToDouble(dr["TotalSales"]) : 0
                                          }).ToList();
            Model.Totalcount = ds.Tables[1].Rows[0]["CountTotal"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["CountTotal"]) : 0;
            Model.CustomerCount = ds.Tables[1].Rows[0]["CustomerCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["CustomerCount"]) : 0;
            Model.SumActNonFee = ds.Tables[1].Rows[0]["SumActNonFeeTotal"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumActNonFeeTotal"]) : 0.0;
            Model.SumRMRTotal = ds.Tables[1].Rows[0]["SumRMRTotal"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumRMRTotal"]) : 0.0;
            Model.SumEquipmentTotal = ds.Tables[1].Rows[0]["SumEquipmentFeeTotal"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumEquipmentFeeTotal"]) : 0.0;
            Model.SumServiceFeeTotal = ds.Tables[1].Rows[0]["SumServiceFeeTotal"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumServiceFeeTotal"]) : 0.0;
            Model.SumAdvanceMonitoringTotal = ds.Tables[1].Rows[0]["SumAdvancedMonitoringTotal"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumAdvancedMonitoringTotal"]) : 0.0;
            Model.SumTotalwoTax = ds.Tables[1].Rows[0]["TotalWoTax"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["TotalWoTax"]) : 0.0;
            Model.SumTotalTax = ds.Tables[1].Rows[0]["SumTaxTotal"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumTaxTotal"]) : 0.0;
            Model.SumTotalSales = ds.Tables[1].Rows[0]["SumTotalSales"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumTotalSales"]) : 0.0;
            Model.SumFinancedAmount = ds.Tables[1].Rows[0]["SumFinancedAmount"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumFinancedAmount"]) : 0.0;


            Model.TotalActNonFee = ds.Tables[2].Rows[0]["TotalActNonFee"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalActNonFee"]) : 0.0;
            Model.TotalRMR = ds.Tables[2].Rows[0]["TotalRMR"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalRMR"]) : 0.0;
            Model.TotalEquipmentFee = ds.Tables[2].Rows[0]["TotalEquipmentFee"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalEquipmentFee"]) : 0.0;
            Model.TotalServiceFee = ds.Tables[2].Rows[0]["TotalServiceFee"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalServiceFee"]) : 0.0;
            Model.TotalAdvancedMonitoring = ds.Tables[2].Rows[0]["TotalAdvancedMonitoring"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalAdvancedMonitoring"]) : 0.0;
            Model.TotalTotalTax = ds.Tables[2].Rows[0]["TotalTotalTax"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalTotalTax"]) : 0.0;
            Model.FinancedAmount = ds.Tables[2].Rows[0]["FinancedAmount"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["FinancedAmount"]) : 0.0;
            Model.TotalSales = ds.Tables[2].Rows[0]["TotalSales"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalSales"]) : 0.0;
            Model.pageno = pageno;
            Model.pagesize = pagesize;

            return Model;
        }

        public DataTable GetSalesSummaryReportsByCompanyId(Guid companyId, DateTime? start, DateTime? end, string searchtext)
        {
            return _CustomerDataAccess.GetSalesSummaryReportExportByCompanyId(companyId, start, end, searchtext);
        }
        public DataTable GetNewSalesReportsByCompanyId(Guid companyId, DateTime? start, DateTime? end, string searchtext, List<string> SalesList)
        {
            return _CustomerDataAccess.GetNewSalesReportExportByCompanyId(companyId, start, end, searchtext, SalesList);
        }

        public DataTable GetNewSalesReportsByInvoices1(Guid companyId, DateTime? start, DateTime? end, string searchtext, int pageno, int pagesize, string order,
            List<string> SalesList, string SalesLocationList, string InvoiceTypeList, string LeadSourceList, string LeadSourceTypeList, DateTime? InstallFrom, DateTime? InstallTo)
        {
            return _CustomerDataAccess.GetNewSalesReportALLByInvoices2(companyId, start, end, searchtext, pageno, pagesize, order, SalesList,
                SalesLocationList, InvoiceTypeList, LeadSourceList, LeadSourceTypeList, InstallFrom, InstallTo).Tables[0];
        }
        public DataTable GetVariableCostReportExportByCompanyId(SalesReportFilter salesReportFilter)
        {
            return _CustomerDataAccess.GetVariableCostReportExportByCompanyId(salesReportFilter);
        }
        public VariableCostCustomerModel GetVariableCostReportALLByCompanyId(SalesReportFilter salesReportFilter)
        {
            VariableCostCustomerModel Model = new VariableCostCustomerModel();
            DataSet ds = _CustomerDataAccess.GetVariableCostReportALLByCompanyId(salesReportFilter);
            Model.VariableCostCustomer = new List<VariableCostCustomer>();
            if (ds != null)
            {
                Model.VariableCostCustomer = (from DataRow dr in ds.Tables[0].Rows
                                              select new VariableCostCustomer()
                                              {
                                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                  DisplayName = dr["DisplayName"].ToString(),
                                                  CustomerNo = dr["CustomerNo"].ToString(),
                                                  SalesDate = dr["SalesDate"] != DBNull.Value ? Convert.ToDateTime(dr["SalesDate"]) : new DateTime(),
                                                  SalesPerson = dr["SalesPerson"].ToString(),
                                                  TicketType = dr["TicketType"].ToString(),
                                                  LeadStatus = dr["LeadStatus"].ToString(),
                                                  LeadSource = dr["LeadSource"].ToString(),
                                                  SalesLocation = dr["SalesLocation"].ToString(),
                                                  Type = dr["Type"].ToString(),
                                                  Revenue = dr["Revenue"] != DBNull.Value ? Convert.ToDouble(dr["Revenue"]) : 0,
                                                  RMR = dr["RMR"] != DBNull.Value ? Convert.ToDouble(dr["RMR"]) : 0,
                                                  EquipVendorCost = dr["EquipVendorCost"] != DBNull.Value ? Convert.ToDouble(dr["EquipVendorCost"]) : 0,
                                                  Labor = dr["Labor"] != DBNull.Value ? Convert.ToDouble(dr["Labor"]) : 0,
                                                  Comm = dr["Comm"] != DBNull.Value ? Convert.ToDouble(dr["Comm"]) : 0,
                                                  TtlCost = dr["TtlCost"] != DBNull.Value ? Convert.ToDouble(dr["TtlCost"]) : 0,
                                                  MISC = dr["MISC"] != DBNull.Value ? Convert.ToDouble(dr["MISC"]) : 0,
                                                  Net = dr["Net"] != DBNull.Value ? Convert.ToDouble(dr["Net"]) : 0,
                                                  CrMult = dr["CrMult"] != DBNull.Value ? Convert.ToDouble(dr["CrMult"]) : 0
                                              }).ToList();
                Model.Totalcount = ds.Tables[1].Rows[0]["Totalcount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["Totalcount"]) : 0;
                Model.SumCustomer = ds.Tables[1].Rows[0]["SumCustomer"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["SumCustomer"]) : 0;
                Model.SumRMR = ds.Tables[1].Rows[0]["SumRMR"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumRMR"]) : 0;
                Model.SumRevenue = ds.Tables[1].Rows[0]["SumRevenue"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumRevenue"]) : 0;
                Model.SumEquipCost = ds.Tables[1].Rows[0]["SumEquipCost"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumEquipCost"]) : 0;
                Model.SumLabor = ds.Tables[1].Rows[0]["SumLabor"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumLabor"]) : 0;
                Model.SumCommission = ds.Tables[1].Rows[0]["SumCommission"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumCommission"]) : 0;
                Model.SumMiscExp = ds.Tables[1].Rows[0]["SumMiscExp"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumMiscExp"]) : 0;
                Model.SumTotalCost = ds.Tables[1].Rows[0]["SumTotalCost"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumTotalCost"]) : 0;
                Model.SumAvgCost = ds.Tables[1].Rows[0]["SumAvgCost"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumAvgCost"]) : 0;
                Model.SumNet = ds.Tables[1].Rows[0]["SumNet"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumNet"]) : 0;
                Model.SumCrMul = ds.Tables[1].Rows[0]["SumCrMul"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumCrMul"]) : 0;

                Model.TotalRMR = ds.Tables[2].Rows[0]["TotalRMR"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalRMR"]) : 0.0;
                Model.TotalRevenue = ds.Tables[2].Rows[0]["TotalRevenue"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalRevenue"]) : 0.0;
                Model.TotalEquipVendorCost = ds.Tables[2].Rows[0]["TotalEquipVendorCost"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalEquipVendorCost"]) : 0.0;
                Model.TotalLabor = ds.Tables[2].Rows[0]["TotalLabor"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalLabor"]) : 0.0;
                Model.TotalComm = ds.Tables[2].Rows[0]["TotalComm"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalComm"]) : 0.0;
                Model.TotalMiscExp = ds.Tables[2].Rows[0]["TotalMiscExp"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalMiscExp"]) : 0.0;
                Model.TotalTtlCost = ds.Tables[2].Rows[0]["TotalTtlCost"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalTtlCost"]) : 0.0;
                Model.TotalNet = ds.Tables[2].Rows[0]["TotalNet"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalNet"]) : 0.0;
                Model.TotalCrMult = ds.Tables[2].Rows[0]["TotalCrMult"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalCrMult"]) : 0.0;
                Model.pageno = salesReportFilter.pageno;
                Model.pagesize = salesReportFilter.pagesize;
            }

            return Model;
        }
        public TechUpSalesModel GetTechUpSalesReportALLByCompanyId(Guid companyId, DateTime? start, DateTime? end, string searchtext, int pageno, int pagesize, string order, List<string> SalesList)
        {
            TechUpSalesModel Model = new TechUpSalesModel();
            DataSet ds = _EmployeeDataAccess.GetTechUpSalesReportALLByCompanyId(companyId, start, end, searchtext, pageno, pagesize, order, SalesList);
            Model.TechUpSales = new List<TechUpSales>();
            if (ds != null)
                Model.TechUpSales = (from DataRow dr in ds.Tables[0].Rows
                                     select new TechUpSales()
                                     {
                                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                         TechName = dr["TechName"].ToString(),
                                         RMR = dr["RMR"] != DBNull.Value ? Convert.ToDouble(dr["RMR"]) : 0,
                                         Commission = dr["Commission"] != DBNull.Value ? Convert.ToDouble(dr["Commission"]) : 0,
                                         EquipmentQty = dr["EquipmentQty"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentQty"]) : 0,
                                         EquipmentValue = dr["EquipmentValue"] != DBNull.Value ? Convert.ToDouble(dr["EquipmentValue"]) : 0,
                                         EquipmentCommission = dr["EquipmentCommission"] != DBNull.Value ? Convert.ToDouble(dr["EquipmentCommission"]) : 0,
                                     }).ToList();
            Model.Totalcount = ds.Tables[1].Rows[0]["CountTotal"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["CountTotal"]) : 0;
            Model.TotalRMR = ds.Tables[2].Rows[0]["TotalRMR"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalRMR"]) : 0.0;
            Model.TotalCommission = ds.Tables[2].Rows[0]["TotalCommission"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalCommission"]) : 0.0;
            Model.TotalEquipmentQty = ds.Tables[2].Rows[0]["TotalEquipmentQty"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalEquipmentQty"]) : 0.0;
            Model.TotalEquipmentValue = ds.Tables[2].Rows[0]["TotalEquipmentValue"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalEquipmentValue"]) : 0.0;
            Model.TotalEquipmentCommission = ds.Tables[2].Rows[0]["TotalEquipmentCommission"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalEquipmentCommission"]) : 0.0;
            Model.pageno = pageno;
            Model.pagesize = pagesize;

            return Model;
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

        public List<CustomerAddress> GetDiffrentAddressByCustomerId(Guid CustomerId)
        {
            DataTable dt = _CustomerAddressDataAccess.GetAllDiffrentAddressByCustomerId(CustomerId);
            List<CustomerAddress> addressList = new List<CustomerAddress>();
            addressList = (from DataRow dr in dt.Rows
                           select new CustomerAddress()
                           {

                               Street = dr["Street"].ToString(),
                               State = dr["State"].ToString(),
                               City = dr["City"].ToString(),
                               ZipCode = dr["ZipCode"].ToString(),

                           }).ToList();
            return addressList;
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

            EmployeeConversionReport EmployeeConversionReportFilter = new EmployeeConversionReport();
            if (ds != null)
            {
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
                EmployeeConversionReportFilter.ConversionReportList = CustomerList;
                EmployeeConversionReportFilter.PayrollTotalCount = PayrollTotalCount;
            }
            return EmployeeConversionReportFilter;
        }


        public CustomerCancellationQueueListWithCount GetAllCustomerCancellationQueue(DateTime StartDate, DateTime EndDate, int pageno, int pagesize, string reason, string contractSigned, string order, string employeereason, string effectivemindate, string effectivemaxdate, string name)
        {
            DataSet dsResult = _CustomerDataAccess.GetAllCustomerCancellationQueue(StartDate, EndDate, pageno, pagesize, reason, contractSigned, order, employeereason, effectivemindate, effectivemaxdate, name);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            DataTable dt2 = dsResult.Tables[2];

            CustomerCancellationQueueListWithCount model = new CustomerCancellationQueueListWithCount();

            model.CustomerCancellationQueueList = (from DataRow dr in dt1.Rows
                                                   select new CustomerCancellationQueue()
                                                   {
                                                       Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                       CustomerIdInt = dr["CustomerIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIdInt"]) : 0,
                                                       CancellationId = (Guid)dr["CancellationId"],
                                                       CustomerId = (Guid)dr["CustomerId"],
                                                       FirstName = dr["FirstName"].ToString(),
                                                       LastName = dr["LastName"].ToString(),
                                                       Address = dr["Address"].ToString(),
                                                       CustomerNo = dr["CustomerNo"].ToString(),
                                                       CustomerName = dr["CustomerName"].ToString(),
                                                       CreatedByVal = dr["CreatedByVal"].ToString(),
                                                       Reason = dr["CancellationReason"].ToString(),
                                                       EmployeeReason = dr["EmpreasonDisplay"].ToString(),
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
            model.TotalCustomerCancellationCount = (from DataRow dr in dt.Rows
                                                    select new TotalCustomerCancellationCount()
                                                    {
                                                        TotalCount = dr["TotalCustomer"] != DBNull.Value ? Convert.ToInt32(dr["TotalCustomer"]) : 0
                                                    }).FirstOrDefault();
            model.TotalUnpaidInvoiceAmount = (from DataRow dr in dt2.Rows
                                              select new TotalUnpaidInvoiceAmount()
                                              {
                                                  TotalUnpaidAmount = dr["TotalRemainingBalance"] != DBNull.Value ? Convert.ToDouble(dr["TotalRemainingBalance"]) : 0.0
                                              }).FirstOrDefault();
            return model;
        }

        public DataTable GetAllCustomerCancellationQueueExport(DateTime StartDate, DateTime EndDate, int pageno, int pagesize, string reason, string contractSigned, string employeereason, string effectivemindate, string effectivemaxdate, string name)
        {
            return _CustomerDataAccess.GetAllCustomerCancellationQueueExport(StartDate, EndDate, pageno, pagesize, reason, contractSigned, employeereason, effectivemindate, effectivemaxdate, name);
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
        public bool DeleteCustomerCreditById(int value)
        {
            return _CustomerCreditDataAccess.Delete(value) > 0;
        }

        public CustomerCredit GetCustomerCreditByTransactionId(int transId)
        {
            return _CustomerCreditDataAccess.GetByQuery(string.Format("TransactionId = '{0}'", transId)).FirstOrDefault();
        }
        public List<CustomerCredit> GetCustomerCreditByTransactionIdAndCustomerId(int transId, Guid CustomerId)
        {
            return _CustomerCreditDataAccess.GetByQuery(string.Format("TransactionId = '{0}' and CustomerId = '{1}'", transId, CustomerId)).ToList();
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
        public bool UpdateCustomerCredit(CustomerCredit cc)
        {
            return _CustomerCreditDataAccess.Update(cc) > 0;
        }
        public Customer GetCustomerByAdditionalCustomerNo(string addcusno)
        {
            return _CustomerDataAccess.GetByQuery(string.Format("AdditionalCustomerNo = '{0}'", addcusno)).FirstOrDefault();
        }

        public Customer GetTransferCustomerById(int value)
        {
            return _CustomerDataAccess.GetByQuery(string.Format("TransferCustomerId = {0}", value)).FirstOrDefault();
        }
        public List<ResturantOrder> GetAllResturantOrderList(Guid comid, int pageno, int pagesize, string searchtext, string order, string startdate, string enddate, Guid customerid, bool filter, string ordertype, string orderstatus)
        {
            DataTable dt = _ResturantOrderDataAccess.GetRestaurentOrderList(comid, pageno, pagesize, searchtext, order, startdate, enddate, customerid, filter, ordertype, orderstatus);
            List<ResturantOrder> miList = new List<ResturantOrder>();
            miList = (from DataRow dr in dt.Rows
                      select new ResturantOrder()
                      {
                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                          CustomerIntId = dr["CustomerIntId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIntId"]) : 0,
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

                          OrderDate = dr["OrderDate"] != DBNull.Value ? Convert.ToDateTime(dr["OrderDate"]) : new DateTime(),
                          CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                          LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                          CustomerName = dr["CustomerName"].ToString(),
                          IsViewed = dr["IsViewed"] != DBNull.Value ? Convert.ToBoolean(dr["IsViewed"]) : false,
                          IsDeleted = dr["IsDeleted"] != DBNull.Value ? Convert.ToBoolean(dr["IsDeleted"]) : false,
                          DiscountCode = dr["DiscountCode"].ToString(),
                          DiscountValue = dr["DiscountValue"] != DBNull.Value ? Convert.ToDouble(dr["DiscountValue"]) : 0,
                      }).ToList();
            return miList;
        }
        public List<ResturantOrder> GetAllResturantOrder(Guid comid)
        {
            return _ResturantOrderDataAccess.GetByQuery(string.Format("CompanyId = '{0}' order by Id desc", comid)).ToList();
        }

        public List<ResturantOrderDetail> GetAllResturantOrderDetailByOrderId(Guid orderid)
        {
            DataTable dt = _CustomerDataAccess.GetAllResturantOrderDetailByOrderId(orderid);
            List<ResturantOrderDetail> miList = new List<ResturantOrderDetail>();
            miList = (from DataRow dr in dt.Rows
                      select new ResturantOrderDetail()
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
                          ItemDescription = dr["ItemDescription"].ToString(),
                          IsStock = dr["IsStock"] != DBNull.Value ? Convert.ToBoolean(dr["IsStock"]) : false,
                          NetPrice = dr["NetPrice"] != DBNull.Value ? Convert.ToDouble(dr["NetPrice"]) : 0,
                      }).ToList();
            return miList;
        }

        public ResturantOrder GetResturantOrderById(int value)
        {
            return _ResturantOrderDataAccess.Get(value);
        }

        public Customer GetCustomerBySearch(string search)
        {
            return _CustomerDataAccess.GetByQuery(string.Format("EmailAddress = '{0}' or CellNo = '{0}'", search)).FirstOrDefault();
        }

        #region RWST Report
        public RWSTList GetRWSTList(DateTime? Start, DateTime? End, int pageno, int pagesize, string searchtext, string Status, string SalesPerson, string order)
        {
            DataSet dsResult = _CustomerDataAccess.GetRWSTList(Start, End, pageno, pagesize, searchtext, Status, SalesPerson, order);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            RWSTList Model = new RWSTList();
            List<RWSTList> RWSTDataList = new List<RWSTList>();
            CustomerCount TotalCustomer = new CustomerCount();
            if (dt != null)
                RWSTDataList = (from DataRow dr in dt.Rows
                                select new RWSTList()
                                {
                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    Name = dr["CustomerName"].ToString(),
                                    Address = dr["Address"].ToString(),
                                    Email = dr["EmailAddress"].ToString(),
                                    Phone = dr["PrimaryPhone"].ToString(),
                                    LeadSource = dr["LeadSource"].ToString(),
                                    Status = dr["Status"].ToString(),
                                    RWST1 = dr["RWST1"].ToString(),
                                    RWST2 = dr["RWST2"].ToString(),
                                    RWST3 = dr["RWST3"].ToString(),
                                    RWST4 = dr["RWST4"].ToString(),
                                    RWST5 = dr["RWST5"].ToString(),
                                    RWST6 = dr["RWST6"].ToString(),
                                    RWST7 = dr["RWST7"].ToString(),
                                    RWST8 = dr["RWST8"].ToString(),
                                    RWST9 = dr["RWST9"].ToString(),
                                    RWST10 = dr["RWST10"].ToString(),
                                    RWST11 = dr["RWST11"].ToString(),
                                    RWST12 = dr["RWST12"].ToString(),
                                    RWST13 = dr["RWST13"].ToString(),
                                    RWST14 = dr["RWST14"].ToString(),
                                    RWST15 = dr["RWST15"].ToString()
                                }).ToList();
            TotalCustomer = (from DataRow dr in dt1.Rows
                             select new CustomerCount()
                             {
                                 TotalCustomer = dr["TotalCustomer"] != DBNull.Value ? Convert.ToInt32(dr["TotalCustomer"]) : 0
                             }).FirstOrDefault();
            RWSTList RWSTList = new RWSTList();
            RWSTList.RWSTDataList = RWSTDataList;
            RWSTList.CustomerCount = TotalCustomer;
            return RWSTList;
        }
        public DataTable DownloadRWSReportData(DateTime? Start, DateTime? End, string searchtext, string Status, string SalesPerson)
        {
            DataTable dt = _CustomerDataAccess.DownloadPurchaseOrderReport(Start, End, searchtext, Status, SalesPerson);
            return dt;
        }
        #endregion

        public bool UpdateResturantOrder(ResturantOrder order)
        {
            return _ResturantOrderDataAccess.Update(order) > 0;
        }
        public CustomerListAgingWithCount GetCustomerListAgingWithCount(DateTime EndDate, string SearchText, int pageno, int pagesize, string reportFor, string order)
        {
            DataSet dsResult = _CustomerDataAccess.GetCustomerListAgingWithCount(EndDate, SearchText, pageno, pagesize, reportFor, order);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            DataTable dt2 = dsResult.Tables[2];

            CustomerListAgingWithCount model = new CustomerListAgingWithCount();

            model.CustomerListAgingList = (from DataRow dr in dt.Rows
                                           select new CustomerListAging()
                                           {
                                               CustomerIntId = dr["CustomerIntId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIntId"]) : 0,
                                               CustomerId = (Guid)dr["CustomerId"],
                                               CustomerName = dr["CustomerName"].ToString(),
                                               SalesPerson = dr["SalesPerson"].ToString(),
                                               SalesPersonId = dr["SalesPersonId"] != DBNull.Value ? Convert.ToInt32(dr["SalesPersonId"]) : 0,
                                               CurrentValue = dr["CurrentValue"] != DBNull.Value ? Convert.ToDouble(dr["CurrentValue"]) : 0.0,
                                               OneThirtyValue = dr["OneThirtyValue"] != DBNull.Value ? Convert.ToDouble(dr["OneThirtyValue"]) : 0.0,
                                               ThirtyOneSixtyValue = dr["ThirtyOneSixtyValue"] != DBNull.Value ? Convert.ToDouble(dr["ThirtyOneSixtyValue"]) : 0.0,
                                               SixtyOneNinetyValue = dr["SixtyOneNinetyValue"] != DBNull.Value ? Convert.ToDouble(dr["SixtyOneNinetyValue"]) : 0.0,
                                               NinetyPlusValue = dr["NinetyPlusValue"] != DBNull.Value ? Convert.ToDouble(dr["NinetyPlusValue"]) : 0.0,
                                               TotalValue = dr["TotalValue"] != DBNull.Value ? Convert.ToDouble(dr["TotalValue"]) : 0.0,
                                           }).ToList();
            model.TotalCurrentValue = dsResult.Tables[1].Rows[0]["TotalCurrentValue"] != DBNull.Value ? Convert.ToDouble(dsResult.Tables[1].Rows[0]["TotalCurrentValue"]) : 0;
            model.TotalOneThirtyValue = dsResult.Tables[1].Rows[0]["TotalOneThirtyValue"] != DBNull.Value ? Convert.ToDouble(dsResult.Tables[1].Rows[0]["TotalOneThirtyValue"]) : 0;
            model.TotalThirtyOneSixtyValue = dsResult.Tables[1].Rows[0]["TotalThirtyOneSixtyValue"] != DBNull.Value ? Convert.ToDouble(dsResult.Tables[1].Rows[0]["TotalThirtyOneSixtyValue"]) : 0;
            model.TotalSixtyOneNinetyValue = dsResult.Tables[1].Rows[0]["TotalSixtyOneNinetyValue"] != DBNull.Value ? Convert.ToDouble(dsResult.Tables[1].Rows[0]["TotalSixtyOneNinetyValue"]) : 0;
            model.TotalNinetyPlusValue = dsResult.Tables[1].Rows[0]["TotalNinetyPlusValue"] != DBNull.Value ? Convert.ToDouble(dsResult.Tables[1].Rows[0]["TotalNinetyPlusValue"]) : 0;
            model.TotalTotalValue = dsResult.Tables[1].Rows[0]["TotalTotalValue"] != DBNull.Value ? Convert.ToDouble(dsResult.Tables[1].Rows[0]["TotalTotalValue"]) : 0;


            model.TotalCustomerAgingCount = (from DataRow dr in dt2.Rows
                                             select new TotalCustomerAgingCount()
                                             {
                                                 TotalCount = dr["TotalEmployee"] != DBNull.Value ? Convert.ToInt32(dr["TotalEmployee"]) : 0
                                             }).FirstOrDefault();
            return model;
        }
        public List<Invoice> GetAgingInvoiceListByCustomerId(Guid CustomerId, string Type, DateTime? EndDate)
        {
            DataTable dt = _CustomerDataAccess.GetAgingInvoiceListByCustomerId(CustomerId, Type, EndDate);
            List<Invoice> iList = new List<Invoice>();
            iList = (from DataRow dr in dt.Rows
                     select new Invoice()
                     {
                         CustomerIntId = dr["CustomerIntId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIntId"]) : 0,
                         CustomerId = (Guid)dr["CustomerId"],
                         InvoiceId = dr["InvoiceId"].ToString(),
                         TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                         Day = dr["Day"] != DBNull.Value ? Convert.ToInt32(dr["Day"]) : 0,
                         CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                         DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : new DateTime()
                     }).ToList();
            return iList;
        }
        public DataTable GetCustomerListAgingExport(DateTime EndDate, string SearchText, int pageno, int pagesize, string reportFor)
        {
            return _CustomerDataAccess.GetCustomerListAgingExport(EndDate, SearchText, pageno, pagesize, reportFor);
        }
        public CustomerCreditCheck GetCustomerCreditCheckById(int value)
        {
            return _CustomerCreditCheckDataAccess.Get(value);
        }
        #region Vendor Account Report
        public CustomerVendorAccount GetVendorAccountList(DateTime? Start, DateTime? End, int pageno, int pagesize, string searchtext)
        {
            DataSet dsResult = _CustomerDataAccess.VendorAccountReportList(Start, End, pageno, pagesize, searchtext);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            CustomerVendorAccount Model = new CustomerVendorAccount();
            List<CustomerVendorAccount> CustomerVendorAccountList = new List<CustomerVendorAccount>();
            TotalCount Count = new TotalCount();
            if (dt != null)
                CustomerVendorAccountList = (from DataRow dr in dt.Rows
                                             select new CustomerVendorAccount()
                                             {
                                                 Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                 DBA = dr["DBA"].ToString(),
                                                 Street = dr["Street"].ToString(),
                                                 City = dr["City"].ToString(),
                                                 State = dr["State"].ToString(),
                                                 Zip = dr["ZipCode"].ToString(),
                                                 FirstName = dr["FirstName"].ToString(),
                                                 LastName = dr["LastName"].ToString(),
                                                 PrimaryPhone = dr["PrimaryPhone"].ToString(),
                                                 Market = dr["Market"].ToString(),
                                                 AccountType = dr["CustomerAccountType"].ToString(),
                                                 ServiceType = dr["Type"].ToString(),
                                                 Note = dr["Note"].ToString()

                                             }).ToList();
            Count = (from DataRow dr in dt1.Rows
                     select new TotalCount()
                     {
                         CountTotal = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                     }).FirstOrDefault();
            CustomerVendorAccount VendorAccount = new CustomerVendorAccount();
            VendorAccount.CustomerVendorAccountList = CustomerVendorAccountList;
            VendorAccount.Count = Count;
            return VendorAccount;
        }
        public DataTable GetVendorAccountListForDownload(DateTime? Start, DateTime? End, string searchtext)
        {
            DataTable dt = _CustomerDataAccess.GetVendorAccountReportListForDownload(Start, End, searchtext);
            return dt;
        }
        #endregion

        public List<Customerupdateoldandnew> GetCustomerAuditLog(int value, DateTime? Audittime)
        {
            string result = "";

            DataTable dtresult = _CustomerDataAccess.GetCustomerAuditLog(value, Audittime);
            List<Customerupdateoldandnew> customerupdatelist = new List<Customerupdateoldandnew>();
            customerupdatelist = (from DataRow dr in dtresult.Rows
                                  select new Customerupdateoldandnew()
                                  {
                                      oldemail = dr["oldEmail"].ToString(),
                                      newemail = dr["newEmail"].ToString(),
                                      oldssn = dr["oldSSN"].ToString(),
                                      newssn = dr["newSSN"].ToString(),
                                      oldtitle = dr["oldTitle"].ToString(),
                                      newtitle = dr["newTitle"].ToString(),
                                      oldcustomerno = dr["oldCustomerNo"].ToString(),
                                      newcustomerno = dr["newCustomerNo"].ToString(),
                                      oldfirstname = dr["oldFirstname"].ToString(),
                                      newfirstname = dr["newFirstname"].ToString(),
                                      oldlastname = dr["oldLastname"].ToString(),
                                      newlastname = dr["newLastname"].ToString(),
                                      oldtype = dr["oldType"].ToString(),
                                      newtype = dr["newType"].ToString(),
                                      oldbusinessname = dr["oldBusinessname"].ToString(),
                                      newbusinessname = dr["newBusinessname"].ToString(),
                                      olddateofbirth = dr["oldDateofBirth"] != DBNull.Value ? Convert.ToDateTime(dr["oldDateofBirth"]) : new DateTime(),
                                      newdateofbirth = dr["newDateofBirth"] != DBNull.Value ? Convert.ToDateTime(dr["newDateofBirth"]) : new DateTime(),
                                      oldprimaryphone = dr["oldPrimaryphone"].ToString(),
                                      newprimaryphone = dr["newPrimaryphone"].ToString(),
                                      oldsecondaryphone = dr["oldSecondaryPhone"].ToString(),
                                      newsecondaryphone = dr["newSecondaryPhone"].ToString(),
                                      oldcellno = dr["oldCellNo"].ToString(),
                                      newcellno = dr["newCellNo"].ToString(),
                                      oldfax = dr["oldFax"].ToString(),
                                      newfax = dr["newFax"].ToString(),

                                      oldcallingtime = dr["oldCallingTime"].ToString(),
                                      newcallingtime = dr["newCallingTime"].ToString(),
                                      oldaddress = dr["oldAddress"].ToString(),
                                      newaddress = dr["newAddress"].ToString(),
                                      oldstreet = dr["oldStreet"].ToString(),
                                      newstreet = dr["newStreet"].ToString(),
                                      oldcity = dr["oldCity"].ToString(),
                                      newcity = dr["newCity"].ToString(),
                                      oldstate = dr["oldState"].ToString(),
                                      newstate = dr["newState"].ToString(),
                                      oldzipcode = dr["oldZipcode"].ToString(),
                                      newzipcode = dr["newZipcode"].ToString(),
                                      oldcreditscore = dr["oldCreditscore"].ToString(),
                                      newcreditscore = dr["newCreditscore"].ToString(),
                                      oldcontactterm = dr["oldContactterm"].ToString(),
                                      newcontactterm = dr["newContactterm"].ToString(),
                                      oldfundingcompany = dr["oldFundingcompany"].ToString(),
                                      newfundingcompany = dr["newFundingcompany"].ToString(),
                                      oldmonthlymonitoringfee = dr["oldMonthlymonitoringfee"].ToString(),
                                      newmonthlymonitoringfee = dr["newMonthlymonitoringfee"].ToString(),
                                      oldcellularbackup = dr["oldCellularbackup"] != DBNull.Value ? Convert.ToBoolean(dr["oldCellularbackup"]) : false,
                                      newcellularbackup = dr["newCellularbackup"] != DBNull.Value ? Convert.ToBoolean(dr["newCellularbackup"]) : false,






                                      oldleadsource = dr["oldLeadSource"].ToString(),
                                      newleadsource = dr["newLeadSource"].ToString(),
                                      oldcustomerfunded = dr["oldCustomerfunded"] != DBNull.Value ? Convert.ToBoolean(dr["oldCustomerfunded"]) : false,
                                      newcustomerfunded = dr["newCustomerfunded"] != DBNull.Value ? Convert.ToBoolean(dr["newCustomerfunded"]) : false,
                                      oldmaintanence = dr["oldMaintanence"] != DBNull.Value ? Convert.ToBoolean(dr["oldMaintanence"]) : false,
                                      newmaintanence = dr["newMaintanence"] != DBNull.Value ? Convert.ToBoolean(dr["newMaintanence"]) : false,
                                      oldnote = dr["oldNote"].ToString(),
                                      newnote = dr["newNote"].ToString(),
                                      oldsalesdate = dr["oldSalesDate"] != DBNull.Value ? Convert.ToDateTime(dr["oldSalesDate"]) : new DateTime(),
                                      newsalesdate = dr["newSalesDate"] != DBNull.Value ? Convert.ToDateTime(dr["newSalesDate"]) : new DateTime(),
                                      oldinstalldate = dr["oldInstallDate"] != DBNull.Value ? Convert.ToDateTime(dr["oldInstallDate"]) : new DateTime(),
                                      newinstalldate = dr["newInstallDate"] != DBNull.Value ? Convert.ToDateTime(dr["newInstallDate"]) : new DateTime(),
                                      oldinstaller = dr["oldInstaller"].ToString(),
                                      newinstaller = dr["newInstaller"].ToString(),
                                      oldsoldby = dr["oldSoldby"].ToString(),
                                      newsoldby = dr["newSoldby"].ToString(),





                                      oldfundingdate = dr["oldFundingDate"] != DBNull.Value ? Convert.ToDateTime(dr["oldFundingDate"]) : new DateTime(),
                                      newfundingdate = dr["newFundingDate"] != DBNull.Value ? Convert.ToDateTime(dr["newFundingDate"]) : new DateTime(),
                                      oldjoindate = dr["oldJoinDate"] != DBNull.Value ? Convert.ToDateTime(dr["oldJoinDate"]) : new DateTime(),
                                      newjoindate = dr["newJoinDate"] != DBNull.Value ? Convert.ToDateTime(dr["newJoinDate"]) : new DateTime(),
                                      oldqa1 = dr["oldQA1"].ToString(),
                                      newqa1 = dr["newQA1"].ToString(),
                                      oldqa2 = dr["oldQA2"].ToString(),
                                      newqa2 = dr["newQA2"].ToString(),
                                      oldqa1date = dr["oldQA1Date"] != DBNull.Value ? Convert.ToDateTime(dr["oldQA1Date"]) : new DateTime(),
                                      newqa1date = dr["newQA1Date"] != DBNull.Value ? Convert.ToDateTime(dr["newQA1Date"]) : new DateTime(),
                                      oldqa2date = dr["oldQA2Date"] != DBNull.Value ? Convert.ToDateTime(dr["oldQA2Date"]) : new DateTime(),
                                      newqa2date = dr["newQA2Date"] != DBNull.Value ? Convert.ToDateTime(dr["newQA2Date"]) : new DateTime(),
                                      oldcustomerstatus = dr["oldStatus"].ToString(),
                                      newcustomerstatus = dr["newStatus"].ToString(),


                                      oldbillamount = dr["oldBillamount"] != DBNull.Value ? Convert.ToDouble(dr["oldBillamount"]) : 0.0,
                                      newbillamount = dr["newBillamount"] != DBNull.Value ? Convert.ToDouble(dr["newBillamount"]) : 0.0,
                                      oldpaymentmethod = dr["oldPaymentmethod"].ToString(),
                                      newpaymentmethod = dr["newPaymentmethod"].ToString(),
                                      oldbillcycle = dr["oldBillCycle"].ToString(),
                                      newbillcycle = dr["newBillCycle"].ToString(),
                                      oldbilltax = dr["oldBillTax"] != DBNull.Value ? Convert.ToBoolean(dr["oldBillTax"]) : false,
                                      newbilltax = dr["newBillTax"] != DBNull.Value ? Convert.ToBoolean(dr["newBillTax"]) : false,

                                      oldbilloutstanding = dr["oldBillOutstanding"] != DBNull.Value ? Convert.ToDouble(dr["oldBillOutstanding"]) : 0.0,
                                      newbilloutstanding = dr["newBillOutstanding"] != DBNull.Value ? Convert.ToDouble(dr["newBillOutstanding"]) : 0.0,
                                      oldisactive = dr["oldIsactive"] != DBNull.Value ? Convert.ToBoolean(dr["oldIsactive"]) : false,
                                      newisactive = dr["oldIsactive"] != DBNull.Value ? Convert.ToBoolean(dr["oldIsactive"]) : false,
                                      olddba = dr["oldDBA"].ToString(),
                                      newdba = dr["newDBA"].ToString(),
                                      oldbusinessaccounttype = dr["oldBusinessaccounttype"].ToString(),
                                      newbusinessaccounttype = dr["newBusinessaccounttype"].ToString(),
                                      //oldreferingcustomer = (Guid)dr["oldrefferingcustomer"],
                                      //newreferingcustomer = (Guid)dr["newrefferingcustomer"],
                                      oldownership = dr["oldownership"].ToString(),
                                      newownership = dr["newownership"].ToString(),
                                      oldpurchaseprice = dr["oldpurchaseprice"] != DBNull.Value ? Convert.ToDouble(dr["oldpurchaseprice"]) : 0.0,
                                      newpurchaseprice = dr["newpurchaseprice"] != DBNull.Value ? Convert.ToDouble(dr["newpurchaseprice"]) : 0.0,
                                      //oldchildof = (Guid)dr["oldchildof"],
                                      //newchildof = (Guid)dr["newchildof"],
                                      oldestimateclosedate = dr["oldestclosedate"] != DBNull.Value ? Convert.ToDateTime(dr["oldestclosedate"]) : new DateTime(),
                                      newestimateclosedate = dr["newestclosedate"] != DBNull.Value ? Convert.ToDateTime(dr["newestclosedate"]) : new DateTime(),





                                      olddonotcall = dr["olddonotcall"] != DBNull.Value ? Convert.ToDateTime(dr["olddonotcall"]) : new DateTime(),
                                      newdonotcall = dr["newdonotcall"] != DBNull.Value ? Convert.ToDateTime(dr["newdonotcall"]) : new DateTime(),

                                      oldcsprovider = dr["oldcsprovider"].ToString(),
                                      newcsprovider = dr["newcsprovider"].ToString(),
                                      //oldsoldby2 = (Guid)dr["oldSoldby2"],
                                      //newsoldby2 = (Guid)dr["newSoldby2"],
                                      //oldsoldby3 = (Guid)dr["oldSoldby3"],
                                      //newsoldby3 = (Guid)dr["newSoldby3"],
                                      oldmovingdate = dr["oldmovingdate"] != DBNull.Value ? Convert.ToDateTime(dr["oldmovingdate"]) : new DateTime(),
                                      newmovingdate = dr["newmovingdate"] != DBNull.Value ? Convert.ToDateTime(dr["newmovingdate"]) : new DateTime(),
                                      oldfollowupdate = dr["oldfollowupdate"] != DBNull.Value ? Convert.ToDateTime(dr["oldfollowupdate"]) : new DateTime(),
                                      newfollowupdate = dr["newfollowupdate"] != DBNull.Value ? Convert.ToDateTime(dr["newfollowupdate"]) : new DateTime(),
                                      oldbuyoutamountbyads = dr["oldbuyoutamountbyads"] != DBNull.Value ? Convert.ToDouble(dr["oldbuyoutamountbyads"]) : 0.0,
                                      newbuyoutamountbyads = dr["newbuyoutamountbyads"] != DBNull.Value ? Convert.ToDouble(dr["newbuyoutamountbyads"]) : 0.0,
                                      oldbuyoutamountbysalesrep = dr["oldbuyoutanountbysalesrep"] != DBNull.Value ? Convert.ToDouble(dr["oldbuyoutanountbysalesrep"]) : 0.0,





                                      newbuyoutamountbysalesrep = dr["newbuyoutanountbysalesrep"] != DBNull.Value ? Convert.ToDouble(dr["newbuyoutanountbysalesrep"]) : 0.0,
                                      oldfinancedterm = dr["oldfinancedterm"] != DBNull.Value ? Convert.ToDouble(dr["oldfinancedterm"]) : 0.0,
                                      newfinancedterm = dr["newfinancedterm"] != DBNull.Value ? Convert.ToDouble(dr["newfinancedterm"]) : 0.0,
                                      oldfinancedamount = dr["oldfinancedamount"] != DBNull.Value ? Convert.ToDouble(dr["oldfinancedamount"]) : 0.0,
                                      newfinancedamount = dr["newfinancedamount"] != DBNull.Value ? Convert.ToDouble(dr["newfinancedamount"]) : 0.0,
                                      oldsoldamount = dr["oldsoldamount"] != DBNull.Value ? Convert.ToDouble(dr["oldsoldamount"]) : 0.0,
                                      newsoldamount = dr["newsoldamount"] != DBNull.Value ? Convert.ToDouble(dr["newsoldamount"]) : 0.0,
                                      oldleadtype = dr["oldleadsourcetype"].ToString(),
                                      newleadtype = dr["newleadsourcetype"].ToString(),
                                      oldsoldbyval = dr["Oldsoldbyval"].ToString(),
                                      newsoldbyval = dr["Newsoldbyval"].ToString(),
                                      Newleadsourceval = dr["Newleadsourceval"].ToString(),
                                      Oldleadsourceval = dr["Oldleadsourceval"].ToString(),
                                      Newinstallerval = dr["Newinstallerval"].ToString(),
                                      Oldinstallerval = dr["Oldinstallerval"].ToString(),

                                  }).ToList();
            //if record 2 

            //Calculation 
            //Chagne happend or not 
            //result += @"Email Changed : '' -> 'rezakawser@gmail.com' ";
            //result += @"Phone Changed : '' -> 'rezakawser@gmail.com' ";
            //result += @"Name Changed : '' -> 'rezakawser@gmail.com' ";
            //result += @"Address Changed : '' -> 'rezakawser@gmail.com' ";

            // 




            // return result;
            return customerupdatelist;
        }

        public RecurringBillingSchedule GetByRecurringBillingID(int id)
        {
            return _RecurringBillingScheduleDataAccess.Get(id);
        }
        public RecurringBillingSchedule GetByRecurringBillingScheduleID(Guid ScheduleID)
        {
            return _RecurringBillingScheduleDataAccess.GetByQuery(string.Format("ScheduleId = '{0}'", ScheduleID)).FirstOrDefault();
        }
        public RecurringBillingSchedule GetRecurringBillingScheduleByInvoiceID(string InvoiceId)
        {
            return _RecurringBillingScheduleDataAccess.GetByQuery(string.Format("LastRMRInvoiceRefId = '{0}'", InvoiceId)).FirstOrDefault();
        }
        public RecurringBillingSchedule GetByRecurringBillingByCustomerId(Guid CustomerId)
        {
            return _RecurringBillingScheduleDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", CustomerId)).FirstOrDefault();
        }
        public List<RecurringBillingSchedule> GetByRecurringBillingListByCustomerId(Guid CustomerId)
        {
            return _RecurringBillingScheduleDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", CustomerId)).OrderBy(x => x.Id).ToList();
        }
        public List<RecurringBillingSchedule> GetRecurringBillingListByTempIds(string Ids)
        {
            return _RecurringBillingScheduleDataAccess.GetByQuery(string.Format("[Id] in ({0})", Ids)).ToList();
        }
        public List<RecurringBillingSchedule> GetAllRecurringBillingListByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId)
        {
            return _RecurringBillingScheduleDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}' and [Status] != 'Init'", CustomerId, CompanyId)).OrderBy(x => x.Id).ToList();
        }
        public int InsertRecurringBillingSchedule(RecurringBillingSchedule Schedule)
        {
            return (int)_RecurringBillingScheduleDataAccess.Insert(Schedule);
        }
        public bool UpdateRecurringBillingSchedule(RecurringBillingSchedule Schedule)
        {
            return _RecurringBillingScheduleDataAccess.Update(Schedule) > -1;
        }
        public int InsertRecurringBillingScheduleItems(RecurringBillingScheduleItems ScheduleItem)
        {
            return (int)_RecurringBillingScheduleItemsDataAccess.Insert(ScheduleItem);
        }
        public List<RecurringBillingSchedule> GetScheduleByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId, string SearchText, string Order)
        {
            DataTable dt = _RecurringBillingScheduleDataAccess.GetAllScheduleByCustomerIdAndCompanyId(CustomerId, CompanyId, SearchText, Order);
            List<RecurringBillingSchedule> ScheduleList = new List<RecurringBillingSchedule>();
            ScheduleList = (from DataRow dr in dt.Rows
                            select new RecurringBillingSchedule()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                ScheduleId = (Guid)dr["ScheduleId"],
                                TemplateName = dr["TemplateName"].ToString(),
                                Status = dr["Status"].ToString(),
                                BillCycle = dr["BillCycle"].ToString(),
                                StartDate = dr["StartDate"] != DBNull.Value ? Convert.ToDateTime(dr["StartDate"]) : new DateTime(),
                                EndDate = dr["EndDate"] != DBNull.Value ? Convert.ToDateTime(dr["EndDate"]) : new DateTime(),
                                PreviousDate = dr["PreviousDate"] != DBNull.Value ? Convert.ToDateTime(dr["PreviousDate"]) : new DateTime(),
                                PaymentCollectionDate = dr["PaymentCollectionDate"] != DBNull.Value ? Convert.ToDateTime(dr["PaymentCollectionDate"]) : new DateTime(),
                                TotalBillAmount = dr["TotalBillAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalBillAmount"]) : 0.0
                            }).ToList();
            return ScheduleList;
        }
        public DataTable GetRecurringBillingList(Guid CustomerId, Guid CompanyId, string SearchText)
        {
            return _RecurringBillingScheduleDataAccess.DownloadAllScheduleByCustomerIdAndCompanyId(CustomerId, CompanyId, SearchText);
        }
        public DataTable GetRMRInvoiceListByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId, string SearchText, DateTime? Start, DateTime? End)
        {
            return _RecurringBillingScheduleDataAccess.GetRMRInvoiceListForDownload(CustomerId, CompanyId, SearchText, null, Start, End);
        }
        public DataTable GetRMRHistoryListByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId, string SearchText, DateTime? Start, DateTime? End)
        {
            return _RecurringBillingScheduleDataAccess.GetRMRHistoryListForDownload(CustomerId, CompanyId, SearchText, null, Start, End);
        }
        public DataTable GetAllUserActivityForRMRCustomerListByCustomerIdExport(Guid CustomerId, Guid CompanyId, string SearchText, DateTime? Start, DateTime? End)
        {
            return _CustomerDataAccess.GetAllUserActivityForRMRCustomerListByCustomerIdExport(CustomerId, CompanyId, SearchText, null, Start, End);
        }
        public DataTable GetAllUserActivityForRMRCustomerListExport(Guid CompanyId, string SearchText, DateTime? Start, DateTime? End)
        {
            return _CustomerDataAccess.GetAllUserActivityForRMRCustomerListExport(CompanyId, SearchText, null, Start, End);
        }
        public DataTable GetAllRMRCreditListExport(Guid CompanyId, string SearchText, DateTime? Start, DateTime? End)
        {
            return _CustomerDataAccess.GetAllRMRCreditListExport(CompanyId, SearchText, null, Start, End);
        }
        public bool DeleteRecurringBillingScheduleItemsByScheduleId(Guid ScheduleId)
        {
            return _RecurringBillingScheduleItemsDataAccess.DeleteRecurringBillingScheduleItemsByScheduleId(ScheduleId);
        }
        public List<RecurringBillingScheduleItems> GetRecurringBillingScheduleItemsByScheduleId(Guid ScheduleId)
        {
            return _RecurringBillingScheduleItemsDataAccess.GetByQuery(String.Format(" ScheduleId = '{0}'", ScheduleId));
        }
        public bool CloneRecurringBilling(Guid oldScheduleId, Guid createdbyuid)
        {
            return _RecurringBillingScheduleDataAccess.CloneRecurringBilling(oldScheduleId, createdbyuid);
        }
        public bool DeleteRecurringBillingScheduleByScheduleId(Guid ScheduleId)
        {
            return _RecurringBillingScheduleDataAccess.DeleteRecurringBillingScheduleByScheduleId(ScheduleId);
        }
        public RecurringBillingModel GetSumOfRecurringBillingByCustomerId(Guid CustomerId)
        {
            DataSet dsResult = _RecurringBillingScheduleDataAccess.GetSumOfRecurringBillingByCustomerId(CustomerId);
            DataTable dt = dsResult.Tables[0];
            RecurringBillingModel model = new RecurringBillingModel();

            model = (from DataRow dr in dt.Rows
                     select new RecurringBillingModel()
                     {
                         MonitoringFee = dr["MonitoringFee"] != DBNull.Value ? Convert.ToDouble(dr["MonitoringFee"]) : 0.0,
                         BillingAmount = dr["BilligAmount"] != DBNull.Value ? Convert.ToDouble(dr["BilligAmount"]) : 0.0
                     }).FirstOrDefault();
            return model;
        }
        public RecurringBillingScheduleReportModel GetRecurringBilliingScheduleForReport(String SearchText, int? BillDay, String Interval, String BillingMethod, String BillingStatus, int PageNo, int PageSize, string Order)
        {
            DataSet dsResult = _RecurringBillingScheduleDataAccess.GetReurringBillingScheduleList(SearchText, BillDay, Interval, BillingMethod, BillingStatus, PageNo, PageSize, Order);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            //DataTable dt2 = dsResult.Tables[2];
            RecurringBillingScheduleReportModel Model = new RecurringBillingScheduleReportModel();
            List<RecurringBillingSchedule> RecurringBillingScheduleList = new List<RecurringBillingSchedule>();
            RecurringBillingScheduleCount Total = new RecurringBillingScheduleCount();
            RecurringBillingScheduleList = (from DataRow dr in dt.Rows
                                            select new RecurringBillingSchedule()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                UnpaidCount = dr["UnpaidCount"] != DBNull.Value ? Convert.ToInt32(dr["UnpaidCount"]) : 0,
                                                ScheduleId = (Guid)dr["ScheduleId"],
                                                TemplateName = dr["TemplateName"].ToString(),
                                                StartDate = dr["StartDate"] != DBNull.Value ? Convert.ToDateTime(dr["StartDate"]) : new DateTime(),
                                                TotalBillAmount = dr["TotalBillAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalBillAmount"]) : 0.0,
                                                Intervals = dr["Interval"].ToString(),
                                                Status = dr["Status"].ToString(),
                                                PaymentMethod = dr["PaymentMethod"].ToString(),
                                                LastInvoice = dr["LastInvoice"].ToString(),
                                                BillDay = dr["BillDate"] != DBNull.Value ? Convert.ToInt32(Convert.ToDateTime(dr["BillDate"]).ToString("dd")) : 0,
                                                CustomerIntId = dr["CusId"] != DBNull.Value ? Convert.ToInt32(dr["CusId"]) : 0,
                                                CustomerId = (Guid)dr["CustomerId"],
                                                CustomerName = dr["CustomerName"].ToString(),
                                                InvoiceIntId = dr["InvId"] != DBNull.Value ? Convert.ToInt32(dr["InvId"]) : 0,
                                                InvoiceId = dr["InvoiceId"].ToString(),
                                                InvoiceDate = dr["InvoiceDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceDate"]) : new DateTime(),
                                            }).ToList();
            Model.TemplateCount = dt1.Rows[0]["TemplateCount"] != DBNull.Value ? Convert.ToInt32(dt1.Rows[0]["TemplateCount"]) : 0;
            Model.TotalAmount = dt1.Rows[0]["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dt1.Rows[0]["TotalAmount"]) : 0;
            Model.ScheduleList = RecurringBillingScheduleList;
            return Model;
        }
        public DataTable GetReurringBillingScheduleListExportReport(string SearchText, int? BillDay, String Interval, String BillingMethod, String BillingStatus, string Order)
        {
            return _RecurringBillingScheduleDataAccess.GetReurringBillingScheduleListExportReport(SearchText, BillDay, Interval, BillingMethod, BillingStatus, Order);
        }
        public RecurringBillingScheduleReportModel GetRecurringBilliingScheduleForReport(string SearchText, string Cycle, string Method, DateTime? Start, DateTime? End, int PageNo, int PageSize, string Order)
        {
            DataSet dsResult = _CustomerDataAccess.GetRecurringBillingForReport(SearchText, Cycle, Method, Start, End, PageNo, PageSize, Order);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            DataTable dt2 = dsResult.Tables[2];
            RecurringBillingScheduleReportModel Model = new RecurringBillingScheduleReportModel();
            List<RecurringBillingSchedule> RecurringBillingScheduleList = new List<RecurringBillingSchedule>();
            RecurringBillingScheduleCount Total = new RecurringBillingScheduleCount();
            RecurringBillingScheduleList = (from DataRow dr in dt1.Rows
                                            select new RecurringBillingSchedule()
                                            {
                                                Id = dr["RBSID"] != DBNull.Value ? Convert.ToInt32(dr["RBSID"]) : 0,
                                                CustomerIntId = dr["CusId"] != DBNull.Value ? Convert.ToInt32(dr["CusId"]) : 0,
                                                CustomerName = dr["CustomerName"].ToString(),
                                                CustomerAddress = dr["Address"].ToString(),
                                                BillAmount = dr["RMRAmount"] != DBNull.Value ? Convert.ToDouble(dr["RMRAmount"]) : 0.0,
                                                PaymentMethod = dr["PaymentMethod"].ToString(),
                                                BillCycle = dr["BillCycle"].ToString(),
                                                StartDate = dr["BillingStartDate"] != DBNull.Value ? Convert.ToDateTime(dr["BillingStartDate"]) : new DateTime(),
                                                TaxAmount = dr["TaxAmount"] != DBNull.Value ? Convert.ToDouble(dr["TaxAmount"]) : 0.0,
                                                TotalBillAmount = dr["TotalBillAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalBillAmount"]) : 0.0
                                            }).ToList();
            Total = (from DataRow dr in dt.Rows
                     select new RecurringBillingScheduleCount()
                     {
                         TotalRecurringBillingSchedule = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                     }).FirstOrDefault();
            Model.TotalRMR = dt2.Rows[0]["TotalRMR"] != DBNull.Value ? Convert.ToDouble(dt2.Rows[0]["TotalRMR"]) : 0.0;
            Model.TotalTax = dt2.Rows[0]["TotalTax"] != DBNull.Value ? Convert.ToDouble(dsResult.Tables[2].Rows[0]["TotalTax"]) : 0.0;
            Model.TotalBilling = dt2.Rows[0]["TotalBill"] != DBNull.Value ? Convert.ToDouble(dt2.Rows[0]["TotalBill"]) : 0.0;
            Model.ScheduleList = RecurringBillingScheduleList;
            Model.Count = Total;
            return Model;
        }
        public DataTable DownloadRecurringBillingReport(string SearchText, string Cycle, string Method, DateTime? Start, DateTime? End, int PageNo, int PageSize, string Order)
        {
            DataTable dt = _CustomerDataAccess.DownloadRecurringBillingReport(SearchText, Cycle, Method, Start, End, PageNo, PageSize, Order);
            return dt;
        }
        public List<RMRInvoice> GetRMRInvoiceByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId, string SearchText, string Order, DateTime? Start, DateTime? End)
        {
            DataTable dt = _RecurringBillingScheduleDataAccess.GetRMRInvoiceListByCustomerIdAndCompanyId(CustomerId, CompanyId, SearchText, Order, Start, End);
            List<RMRInvoice> RMRInvoiceList = new List<RMRInvoice>();
            RMRInvoiceList = (from DataRow dr in dt.Rows
                              select new RMRInvoice()
                              {
                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                  InvoiceId = dr["InvoiceId"].ToString(),
                                  Date = dr["InvoiceDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceDate"]) : new DateTime(),
                                  AmountDue = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0.0,
                                  NetDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0.0,
                                  Status = dr["Status"].ToString(),
                              }).ToList();
            return RMRInvoiceList;
        }
        public List<RMRHistory> GetRMRHistoryByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId, string SearchText, string Order, DateTime? Start, DateTime? End)
        {
            DataTable dt = _RecurringBillingScheduleDataAccess.GetRMRHistoryListByCustomerIdAndCompanyId(CustomerId, CompanyId, SearchText, Order, Start, End);
            List<RMRHistory> RMRHistoryList = new List<RMRHistory>();
            RMRHistoryList = (from DataRow dr in dt.Rows
                              select new RMRHistory()
                              {
                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                  InvoiceId = dr["InvoiceId"].ToString(),
                                  InvoiceDate = dr["InvoiceDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceDate"]) : new DateTime(),
                                  PaymentDate = dr["PaymentDate"] != DBNull.Value ? Convert.ToDateTime(dr["PaymentDate"]) : new DateTime(),
                                  Amount = dr["Amout"] != DBNull.Value ? Convert.ToDouble(dr["Amout"]) : 0.0,
                                  CheckNo = dr["CheckNo"].ToString(),
                                  BatchCode = dr["BatchNumber"].ToString(),
                                  Funded = dr["Funded"].ToString(),
                                  Posted = dr["Posted"].ToString(),
                                  Method = dr["PaymentMethod"].ToString()
                              }).ToList();
            return RMRHistoryList;
        }
        public RMRInvoiceModel GetRMRInvoiceListByCompanyId(RMRFilter Filter)
        {
            DataSet dsResult = _RecurringBillingScheduleDataAccess.GetRMRInvoiceListByCompanyId(Filter);

            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            DataTable dt2 = dsResult.Tables[2];

            RMRInvoiceModel Model = new RMRInvoiceModel();
            Model.Total = dt.Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["TotalCount"]) : 0;

            List<RMRInvoice> RMRInvoiceList = new List<RMRInvoice>();
            RMRInvoiceList = (from DataRow dr in dt1.Rows
                              select new RMRInvoice()
                              {
                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                  InvoiceId = dr["InvoiceId"].ToString(),
                                  Name = dr["CustomerName"].ToString(),
                                  CustomerIntId = dr["CustomerIntId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIntId"]) : 0,
                                  Date = dr["InvoiceDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceDate"]) : new DateTime(),
                                  AmountDue = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0.0,
                                  NetDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0.0,
                                  Status = dr["Status"].ToString(),
                              }).ToList();
            RMRTotalModel RMRTotal = new RMRTotalModel();
            RMRTotal = (from DataRow dr in dt2.Rows
                        select new RMRTotalModel()
                        {
                            TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0.0,
                            TotalDue = dr["TotalBalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["TotalBalanceDue"]) : 0.0,
                        }).FirstOrDefault();

            Model.RMRInvoiceList = RMRInvoiceList;
            Model.RMRTotal = RMRTotal;
            return Model;
        }
        public DataTable DownloadRMRInvoiceListByCompanyId(RMRFilter Filter)
        {
            return _RecurringBillingScheduleDataAccess.DownloadRMRInvoiceListByCompanyId(Filter);
        }
        public RMRAuditModel GetRMRAuditList(RMRAuditFilter Filter)
        {
            RMRAuditModel Model = new RMRAuditModel();
            DataSet ds = _CustomerDataAccess.GetRMRAuditList(Filter);
            Model.RMRAudit = new List<RMRAudit>();
            if (ds != null)
                Model.RMRAudit = (from DataRow dr in ds.Tables[0].Rows
                                  select new RMRAudit()
                                  {
                                      Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                      DisplayName = dr["DisplayName"].ToString(),
                                      Address = dr["Address"].ToString(),
                                      Street = dr["Street"].ToString(),
                                      City = dr["City"].ToString(),
                                      State = dr["State"].ToString(),
                                      ZipCode = dr["ZipCode"].ToString(),
                                      Email = dr["EmailAddress"].ToString(),
                                      Phone = dr["PrimaryPhone"].ToString(),
                                      StreetPrevious = dr["StreetPrevious"].ToString(),
                                      CityPrevious = dr["CityPrevious"].ToString(),
                                      StatePrevious = dr["StatePrevious"].ToString(),
                                      ZipCodePrevious = dr["ZipCodePrevious"].ToString(),
                                      AddressPrevious = dr["AddressPrevious"].ToString(),
                                      InstallDate = dr["InstallDate"] != DBNull.Value ? Convert.ToDateTime(dr["InstallDate"]) : new DateTime(),
                                      BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0.0,
                                      Item = dr["EquipName"].ToString(),
                                      RMR = dr["RMR"] != DBNull.Value ? Convert.ToDouble(dr["RMR"]) : 0,
                                  }).ToList();
            Model.TotalRMR = ds.Tables[1].Rows[0]["TotalRMR"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalRMR"]) : 0;

            Model.Totalcount = ds.Tables[2].Rows[0]["Totalcount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["Totalcount"]) : 0;
            Model.CustomerTotalCount = ds.Tables[2].Rows[0]["CustomerTotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["CustomerTotalCount"]) : 0;
            Model.CustomerTotalRMR = ds.Tables[2].Rows[0]["CustomerTotalRMR"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["CustomerTotalRMR"]) : 0.0;

            Model.pageno = Filter.PageNo;
            Model.pagesize = Filter.PageSize;

            return Model;
        }
        //public DataTable DownloadRMRAuditListByCompanyId(RMRFilter Filter)
        //{
        //    DataTable dt=new DataTable()
        //    return dt;
        //}
        public RMRHistoryModel GetRMRHistoryListByCompanyId(RMRFilter Filter)
        {
            DataSet dsResult = _RecurringBillingScheduleDataAccess.GetRMRHistoryListByCompanyId(Filter);

            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            DataTable dt2 = dsResult.Tables[2];

            RMRHistoryModel Model = new RMRHistoryModel();
            Model.Total = dt.Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["TotalCount"]) : 0;

            List<RMRHistory> RMRHistoryList = new List<RMRHistory>();
            RMRHistoryList = (from DataRow dr in dt1.Rows
                              select new RMRHistory()
                              {
                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                  InvoiceId = dr["InvoiceId"].ToString(),
                                  Name = dr["CustomerName"].ToString(),
                                  CustomerIntId = dr["CustomerIntId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIntId"]) : 0,
                                  InvoiceDate = dr["InvoiceDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceDate"]) : new DateTime(),
                                  PaymentDate = dr["PaymentDate"] != DBNull.Value ? Convert.ToDateTime(dr["PaymentDate"]) : new DateTime(),
                                  Amount = dr["Amout"] != DBNull.Value ? Convert.ToDouble(dr["Amout"]) : 0.0,
                                  CheckNo = dr["CheckNo"].ToString(),
                                  BatchCode = dr["BatchNumber"].ToString(),
                                  Funded = dr["Funded"].ToString(),
                                  Posted = dr["Posted"].ToString(),
                                  Method = dr["PaymentMethod"].ToString()
                              }).ToList();
            RMRTotalModel RMRTotal = new RMRTotalModel();
            RMRTotal = (from DataRow dr in dt2.Rows
                        select new RMRTotalModel()
                        {
                            TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0.0
                        }).FirstOrDefault();

            Model.RMRHistoryList = RMRHistoryList;
            Model.RMRTotal = RMRTotal;
            return Model;
        }
        public DataTable DownloadRMRHistoryListByCompanyId(RMRFilter Filter)
        {
            return _RecurringBillingScheduleDataAccess.DownloadRMRHistoryListByCompanyId(Filter);
        }
        public List<ParentKey> GetParentSourceByDataKey(string DataKey)
        {
            DataTable dt = _CustomerDataAccess.GetParentKeyByDataKey(DataKey);
            List<ParentKey> ParentKeyList = new List<ParentKey>();
            ParentKeyList = (from DataRow dr in dt.Rows
                             select new ParentKey()
                             {
                                 Name = dr["ParentDatakey"].ToString()
                             }).ToList();
            return ParentKeyList;
        }
        public AllRecordsReportModel GetAllRecordsForReport(AllRecords filter)
        {
            DataSet dsResult = _CustomerDataAccess.GetAllRecordForReport(filter);

            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];

            List<AllRecordsModel> RecordList = new List<AllRecordsModel>();
            RecordCount Total = new RecordCount();
            RecordList = (from DataRow dr in dt.Rows
                          select new AllRecordsModel()
                          {
                              RMRId = dr["RMRId"] != DBNull.Value ? Convert.ToInt32(dr["RMRId"]) : 0,
                              Address = dr["Address"].ToString(),
                              LeadSourceParent = dr["LeadSourceParent"].ToString(),
                              LeadSourceType = dr["LeadSourceType"].ToString(),
                              LeadSource = dr["LeadSource"].ToString(),
                              LeadStatus = dr["LeadStatus"].ToString(),
                              CustomerName = dr["Name"].ToString(),
                              CustomerStatus = dr["CustomerStatus"].ToString(),
                              SalesLocation = dr["SalesLocation"].ToString(),
                              SalesPerson = dr["SalesPerson"].ToString(),
                              AppoinmentSetBy = dr["AppoinmentSetBy"].ToString(),
                              CSId = dr["CSId"].ToString(),
                              IsLead = dr["IsLead"] != DBNull.Value ? Convert.ToInt32(dr["IsLead"]) : 0,
                              CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime()

                          }).ToList();
            Total = (from DataRow dr in dt1.Rows
                     select new RecordCount()
                     {
                         TotalRecord = dr["TotalRecord"] != DBNull.Value ? Convert.ToInt32(dr["TotalRecord"]) : 0
                     }).FirstOrDefault();
            AllRecordsReportModel Model = new AllRecordsReportModel();
            Model.AllRecordsModelList = RecordList;
            Model.Total = Total;
            return Model;
        }
        public DataTable DownloadAllRecordsReport(AllRecords filter)
        {
            DataTable dt = _CustomerDataAccess.DownloadAllRecordsReport(filter);
            return dt;
        }
        public bool InsertInIndividualInstalledEquipment(Guid CompanyId)
        {
            return _IndividualInstalledEquipmentDataAccess.InsertInIndividualInstalledEquipment(CompanyId);
        }
        public MoveCustomerReportModel GetAllMoveCustomerRecordsForReport(MoveCustomer filter)
        {
            DataSet dsResult = _CustomerDataAccess.GetAllMoveCustomerRecordForReport(filter);

            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];

            List<MoveCustomerModel> MoveCustomerList = new List<MoveCustomerModel>();
            RecordCount Total = new RecordCount();
            MoveCustomerList = (from DataRow dr in dt.Rows
                                select new MoveCustomerModel()
                                {
                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    Address = dr["Address"].ToString(),
                                    CustomerId = (Guid)dr["CustomerId"],
                                    CustomerName = dr["CustomerName"].ToString(),
                                    Email = dr["Email"].ToString(),
                                    MoveDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                    Street = dr["Street"].ToString(),
                                    StreetType = dr["StreetType"].ToString(),
                                    Appartment = dr["Appartment"].ToString(),
                                    ZipCode = dr["ZipCode"].ToString(),
                                    City = dr["City"].ToString(),
                                    State = dr["State"].ToString(),
                                    IsLead = dr["IsLead"] != DBNull.Value ? Convert.ToInt32(dr["IsLead"]) : 0,
                                    UnlinkCustomer = (dr["UnlinkCustomer"] != DBNull.Value ? Convert.ToBoolean(dr["UnlinkCustomer"]) : false),
                                    OldCustomerId = dr["oldcustomerid"] != DBNull.Value ? Convert.ToInt32(dr["oldcustomerid"]) : 0,
                                    OldCustomerName = dr["oldcustomername"].ToString()


                                }).ToList();
            Total = (from DataRow dr in dt1.Rows
                     select new RecordCount()
                     {
                         TotalRecord = dr["TotalRecord"] != DBNull.Value ? Convert.ToInt32(dr["TotalRecord"]) : 0
                     }).FirstOrDefault();
            MoveCustomerReportModel Model = new MoveCustomerReportModel();
            Model.MoveCustomerModelList = MoveCustomerList;
            Model.Total = Total;
            return Model;
        }

        public DataTable DownloadAllMoveCustomerRecordsReport(MoveCustomer filter)
        {
            DataTable dt = _CustomerDataAccess.DownloadAllMoveCustomerRecord(filter);
            return dt;
        }

        public List<AppointmentEquipmentIdList> GetAllIndividualInstalledEquipmentIdById(int Id)
        {
            DataTable dt = _IndividualInstalledEquipmentDataAccess.GetAllIndividualInstalledEquipmentIdById(Id);
            List<AppointmentEquipmentIdList> miList = new List<AppointmentEquipmentIdList>();
            miList = (from DataRow dr in dt.Rows
                      select new AppointmentEquipmentIdList()
                      {
                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0
                      }).ToList();
            return miList;
        }
        public IndividualInstalledEquipment GetIndividualInstalledEquipmentById(int id)
        {
            return _IndividualInstalledEquipmentDataAccess.GetByQuery(string.Format(" Id = '{0}' ", id)).FirstOrDefault();
        }
        public bool UpdateIndividualInstalledEquipment(IndividualInstalledEquipment IE)
        {
            return _IndividualInstalledEquipmentDataAccess.Update(IE) > 0;
        }
        public IndividualInstalledEquipment GetIndividualInstalledEquipmentByTicketIdAndEquipmentId(int TicketId, Guid EquipmentId)
        {
            return _IndividualInstalledEquipmentDataAccess.GetByQuery(string.Format("TicketId ='{0}' and EquipmentId ='{1}'", TicketId, EquipmentId)).FirstOrDefault();
        }
        public IndividualInstalledEquipment GetIndividualInstalledEquipmentByTicketIdAndEquipmentIdAndAppointmentEquipmentId(int TicketId, Guid EquipmentId, int AppointmentEquipmentId)
        {
            return _IndividualInstalledEquipmentDataAccess.GetByQuery(string.Format("TicketId ='{0}' and EquipmentId ='{1}' and AppointmentEquipmentId='{2}'", TicketId, EquipmentId, AppointmentEquipmentId)).FirstOrDefault();
        }
        public bool DeleteIndividualInstalledEquipment(int TicketId, Guid EquipmentId)
        {
            return _IndividualInstalledEquipmentDataAccess.DeleteIndividualInstalledEquipmentByTicketIdAndEquipmentId(TicketId, EquipmentId);
        }
        public bool DeleteIndividualInstalledEquipmentById(int TicketId, Guid EquipmentId, int AppointmentEquipmentId)
        {
            return _IndividualInstalledEquipmentDataAccess.DeleteIndividualInstalledEquipmentByTicketIdAndEquipmentIdAndAppointmentEquipmentId(TicketId, EquipmentId, AppointmentEquipmentId);
        }
        public bool DeleteIndividualInstalledEquipmentById(int id)
        {
            return _IndividualInstalledEquipmentDataAccess.Delete(id) > 0;
        }
        public long InsertInIndividualInstalledEquipmentObj(IndividualInstalledEquipment Obj)
        {
            return _IndividualInstalledEquipmentDataAccess.Insert(Obj);
        }
        public NewSalesCustomerModel GetInstalledDealReportList(Guid companyId, DateTime? start, DateTime? end, string searchtext, int pageno, int pagesize, string order, List<string> SalesList, List<string> InstallerList)
        {
            NewSalesCustomerModel Model = new NewSalesCustomerModel();
            DataSet ds = _CustomerDataAccess.GetInstallerDealsReportALLByCompanyId(companyId, start, end, searchtext, pageno, pagesize, order, SalesList, InstallerList);
            Model.NewSalesCustomer = new List<NewSalesCustomer>();
            if (ds != null)
                Model.NewSalesCustomer = (from DataRow dr in ds.Tables[0].Rows
                                          select new NewSalesCustomer()
                                          {
                                              Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                              DisplayName = dr["DisplayName"].ToString(),
                                              CustomerNo = dr["CustomerNo"].ToString(),
                                              SalesDate = dr["InstalledDate"] != DBNull.Value ? Convert.ToDateTime(dr["InstalledDate"]) : new DateTime(),
                                              SalesPerson = dr["SalesPerson"].ToString(),
                                              Installer = dr["Installer"].ToString(),
                                              TicketType = dr["TicketType"].ToString(),
                                              LeadSource = dr["LeadSource"].ToString(),
                                              SalesLocation = dr["SalesLocation"].ToString(),
                                              Type = dr["Type"].ToString(),
                                              ActNonFee = dr["ActNonFee"] != DBNull.Value ? Convert.ToDouble(dr["ActNonFee"]) : 0,
                                              RMR = dr["RMR"] != DBNull.Value ? Convert.ToDouble(dr["RMR"]) : 0,
                                              EquipmentFee = dr["EquipmentFee"] != DBNull.Value ? Convert.ToDouble(dr["EquipmentFee"]) : 0,
                                              ServiceFee = dr["ServiceFee"] != DBNull.Value ? Convert.ToDouble(dr["ServiceFee"]) : 0,
                                              AdvancedMonitoring = dr["AdvancedMonitoring"] != DBNull.Value ? Convert.ToDouble(dr["AdvancedMonitoring"]) : 0,
                                              TotalTax = dr["TotalTax"] != DBNull.Value ? Convert.ToDouble(dr["TotalTax"]) : 0,
                                              FinancedAmount = dr["FinancedAmount"] != DBNull.Value ? Convert.ToDouble(dr["FinancedAmount"]) : 0,
                                              TotalSales = dr["TotalSales"] != DBNull.Value ? Convert.ToDouble(dr["TotalSales"]) : 0
                                          }).ToList();
            Model.Totalcount = ds.Tables[1].Rows[0]["CountTotal"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["CountTotal"]) : 0;
            Model.SumActNonFee = ds.Tables[1].Rows[0]["SumTotalActNonFee"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumTotalActNonFee"]) : 0.0;
            Model.SumRMRTotal = ds.Tables[1].Rows[0]["SumTotalRMR"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumTotalRMR"]) : 0.0;
            Model.SumEquipmentTotal = ds.Tables[1].Rows[0]["SumTotalEquipmentFee"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumTotalEquipmentFee"]) : 0.0;
            Model.SumServiceFeeTotal = ds.Tables[1].Rows[0]["SumTotalServiceFee"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumTotalServiceFee"]) : 0.0;
            Model.SumAdvanceMonitoringTotal = ds.Tables[1].Rows[0]["SumTotalAdvancedMonitoring"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumTotalAdvancedMonitoring"]) : 0.0;
            Model.SumTotalwoTax = ds.Tables[1].Rows[0]["SumTotalwoTax"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumTotalwoTax"]) : 0.0;
            Model.SumTotalTax = ds.Tables[1].Rows[0]["SumTotalTax"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumTotalTax"]) : 0.0;
            Model.SumTotalSales = ds.Tables[1].Rows[0]["SumTotalSales"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumTotalSales"]) : 0.0;
            Model.SumFinancedAmount = ds.Tables[1].Rows[0]["SumFinancedAmount"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["SumFinancedAmount"]) : 0.0;

            Model.TotalActNonFee = ds.Tables[2].Rows[0]["TotalActNonFee"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalActNonFee"]) : 0.0;
            Model.TotalRMR = ds.Tables[2].Rows[0]["TotalRMR"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalRMR"]) : 0.0;
            Model.TotalEquipmentFee = ds.Tables[2].Rows[0]["TotalEquipmentFee"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalEquipmentFee"]) : 0.0;
            Model.TotalServiceFee = ds.Tables[2].Rows[0]["TotalServiceFee"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalServiceFee"]) : 0.0;
            Model.TotalAdvancedMonitoring = ds.Tables[2].Rows[0]["TotalAdvancedMonitoring"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalAdvancedMonitoring"]) : 0.0;
            Model.TotalTotalTax = ds.Tables[2].Rows[0]["TotalTotalTax"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalTotalTax"]) : 0.0;
            Model.FinancedAmount = ds.Tables[2].Rows[0]["FinancedAmount"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["FinancedAmount"]) : 0.0;
            Model.TotalSales = ds.Tables[2].Rows[0]["TotalSales"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalSales"]) : 0.0;
            Model.pageno = pageno;
            Model.pagesize = pagesize;

            return Model;
        }

        public ServiceSalesModel GetServiceSalesReportList(DateTime? start, DateTime? end, string searchtext, string filtertext, int pageno, int pagesize)
        {
            ServiceSalesModel Model = new ServiceSalesModel();
            DataSet ds = _CustomerDataAccess.GetServiceSalesReportList(start, end, searchtext, filtertext, pageno, pagesize);
            Model.serviceSalesList = new List<ServiceSales>();
            if (ds != null)
                Model.serviceSalesList = (from DataRow dr in ds.Tables[0].Rows
                                          select new ServiceSales()
                                          {
                                              Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                              CustomerName = dr["CustomerName"].ToString(),
                                              Cellular = dr["Cellular"] != DBNull.Value ? Convert.ToInt32(dr["Cellular"]) : 0,
                                              Smart = dr["Smart"] != DBNull.Value ? Convert.ToInt32(dr["Smart"]) : 0,
                                              Standard = dr["Standard"] != DBNull.Value ? Convert.ToInt32(dr["Standard"]) : 0,
                                              PSP = dr["PSP"] != DBNull.Value ? Convert.ToInt32(dr["PSP"]) : 0,
                                              GSP = dr["GSP"] != DBNull.Value ? Convert.ToInt32(dr["GSP"]) : 0
                                          }).ToList();
            Model.Totalcount = ds.Tables[1].Rows[0]["Totalcount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["Totalcount"]) : 0;
            Model.SmartTotal = ds.Tables[1].Rows[0]["SmartTotal"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["SmartTotal"]) : 0;
            Model.StandardTotal = ds.Tables[1].Rows[0]["StandardTotal"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["StandardTotal"]) : 0;
            Model.PSPTotal = ds.Tables[1].Rows[0]["PSPTotal"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["PSPTotal"]) : 0;
            Model.GSPTotal = ds.Tables[1].Rows[0]["GSPTotal"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["GSPTotal"]) : 0;
            Model.CellularTotal = ds.Tables[1].Rows[0]["CellularTotal"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["CellularTotal"]) : 0;

            return Model;
        }
        public TaxCollection GetAllTaxCollectionReport(TaxCollectionFilter Filters)
        {
            TaxCollection Model = new TaxCollection();

            DataSet ds = _CustomerDataAccess.GetAllTaxCollectionReport(Filters);
            Model.TaxCollectionList = (from DataRow dr in ds.Tables[1].Rows
                                       select new TaxCollection()
                                       {
                                           Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                           CusName = dr["CustomerName"].ToString(),
                                           InvId = dr["InvoiceId"].ToString(),
                                           Tax = dr["Tax"] != DBNull.Value ? Convert.ToDouble(dr["Tax"]) : 0.0,
                                           CusId = dr["CusId"] != DBNull.Value ? Convert.ToInt32(dr["CusId"]) : 0,
                                           TransactionDate = dr["TransacationDate"] != DBNull.Value ? Convert.ToDateTime(dr["TransacationDate"]) : new DateTime(),

                                       }).ToList();
            Model.TotalCount = ds.Tables[0].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[0]["TotalCount"]) : 0;
            Model.TotalAmountByPage = ds.Tables[2].Rows[0]["TotalTax"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalTax"]) : 0.0;
            Model.PageNo = Filters.PageNo.Value;
            Model.PageSize = Filters.PageSize;
            Model.Searchtext = Filters.SearchText;
            Model.StartDate = Filters.StartDate;
            Model.EndDate = Filters.EndDate;
            return Model;
        }
        public DataTable GetInstalledDealReportsListForDownload(Guid companyId, DateTime? start, DateTime? end, string searchtext, List<string> SalesList, List<string> InstallerList)
        {
            return _CustomerDataAccess.GetInstalledDealsReportExport(companyId, start, end, searchtext, SalesList, InstallerList);
        }
        public DataTable TaxCollectionReportListForDownload(TaxCollectionFilter Filters)
        {
            DataSet ds = _CustomerDataAccess.GetAllTaxCollectionReport(Filters);

            return ds.Tables[0];
        }
        #region Recurring Billing

        public List<Customer> GetCustomerListForRecurringBillByCompanyId(Guid companyId)
        {
            return _CustomerDataAccess.GetCustomerListForRecurringBillingByCompanyId(companyId);
        }
        public List<RecurringBillingSchedule> GetRecurringBillingListByFilter(Guid companyId, string paymentfilter)
        {
            string strPaymentFilterquery = "";
            if (!string.IsNullOrEmpty(paymentfilter) && !string.IsNullOrWhiteSpace(paymentfilter))
            {
                if (paymentfilter == "CreditCard")
                {
                    strPaymentFilterquery = string.Format("AND PaymentMethod like 'CC_%'");
                }
                else if (paymentfilter == "ACH")
                {
                    strPaymentFilterquery = string.Format("AND PaymentMethod like 'ACH%'");
                }
                else
                {
                    strPaymentFilterquery = string.Format("AND PaymentMethod like 'Invoice'");
                }
            }
            return _RecurringBillingScheduleDataAccess.GetByQuery(string.Format(@" CompanyId = '{0}' AND Status in ('Active','FreeTrial') {1}", companyId, strPaymentFilterquery));
        }
        public RMROverviewModel GetRMROverview(Guid CustomerId, Guid CompanyId)
        {
            DataSet ds = _CustomerDataAccess.GetRMROverviewData(CustomerId, CompanyId);
            RMROverviewModel Model = new RMROverviewModel();
            Model = (from DataRow dr in ds.Tables[0].Rows
                     select new RMROverviewModel()
                     {
                         RMROverview = dr["RMR"] != DBNull.Value ? Convert.ToDouble(dr["RMR"]) : 0.0,
                         AmountDue = dr["AmountDue"] != DBNull.Value ? Convert.ToDouble(dr["AmountDue"]) : 0.0,
                         PaymentMethod = dr["PaymentMethod"].ToString(),
                         OpenInvoice = dr["OpenInvoice"] != DBNull.Value ? Convert.ToDouble(dr["OpenInvoice"]) : 0.0,
                         //DayPastDue = dr["DayPastDue"] != DBNull.Value ? Convert.ToInt32(dr["DayPastDue"]) : 0,
                         OpenCredit = dr["OpenCredit"] != DBNull.Value ? Convert.ToDouble(dr["OpenCredit"]) : 0.0,
                         BillOnDate = dr["BillOnDate"] != DBNull.Value ? Convert.ToInt32(dr["BillOnDate"]) : 0,
                         LastPayment = dr["LastPayment"] != DBNull.Value ? Convert.ToDateTime(dr["LastPayment"]) : new DateTime()
                     }).FirstOrDefault();

            return Model;
        }
        #endregion
        #region rmr log

        public UserActivityCustomerModel GetAllUserActivityForRMRCustomerListByCustomerId(int pageno, int pagesize, DateTime? startdate, DateTime? enddate, string searchtext, Guid CustomerGId, string order, string logstartdate, string logenddate)
        {

            DataSet ds = _CustomerDataAccess.GetAllUserActivityForRMRCustomerListByCustomerId(pageno, pagesize, startdate, enddate, searchtext, CustomerGId, order, logstartdate, logenddate);
            List<UserActivity> buildList = new List<UserActivity>();
            buildList = (from DataRow dr in ds.Tables[0].Rows
                         select new UserActivity()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             CustomerGId = (Guid)dr["UACCustomerId"],
                             CustomerName = dr["CustomerName"].ToString(),
                             ActivityId = (Guid)dr["ActivityId"],
                             PageUrl = dr["PageUrl"].ToString(),
                             ReferrerUrl = dr["ReferrerUrl"].ToString(),
                             Action = dr["Action"].ToString(),
                             ActionDisplyText = dr["ActionDisplyText"].ToString(),
                             UserIp = dr["UserIp"].ToString(),
                             UserAgent = dr["UserAgent"].ToString(),
                             UserName = dr["UserName"].ToString(),
                             UserId = (Guid)dr["UserId"],
                             StatsDate = dr["StatsDate"] != DBNull.Value ? Convert.ToDateTime(dr["StatsDate"]) : DateTime.Now,
                         }).ToList();

            SalesReportCountModel SalesReportCountModel = new SalesReportCountModel();
            SalesReportCountModel = (from DataRow dr in ds.Tables[1].Rows
                                     select new SalesReportCountModel()
                                     {
                                         TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                     }).FirstOrDefault();
            TotalSalesAmountModel TotalSalesAmountModel = new TotalSalesAmountModel();
            UserActivityCustomerModel SalesReportModel = new UserActivityCustomerModel();
            SalesReportModel.ListUserActivity = buildList;
            SalesReportModel.InvoiceReportCountModel = SalesReportCountModel;
            SalesReportModel.TotalInvoiceAmountModel = TotalSalesAmountModel;
            return SalesReportModel;
        }
        public UserActivityCustomerModel GetAllUserActivityForRMRCustomerList(int pageno, int pagesize, DateTime? startdate, DateTime? enddate, string searchtext, string order, string logstartdate, string logenddate)
        {

            DataSet ds = _CustomerDataAccess.GetAllUserActivityForRMRCustomerList(pageno, pagesize, startdate, enddate, searchtext, order, logstartdate, logenddate);
            List<UserActivity> buildList = new List<UserActivity>();
            buildList = (from DataRow dr in ds.Tables[0].Rows
                         select new UserActivity()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             CustomerGId = (Guid)dr["UACCustomerId"],
                             CustomerName = dr["CustomerName"].ToString(),
                             ActivityId = (Guid)dr["ActivityId"],
                             PageUrl = dr["PageUrl"].ToString(),
                             ReferrerUrl = dr["ReferrerUrl"].ToString(),
                             Action = dr["Action"].ToString(),
                             ActionDisplyText = dr["ActionDisplyText"].ToString(),
                             UserIp = dr["UserIp"].ToString(),
                             UserAgent = dr["UserAgent"].ToString(),
                             UserName = dr["UserName"].ToString(),
                             UserId = (Guid)dr["UserId"],
                             StatsDate = dr["StatsDate"] != DBNull.Value ? Convert.ToDateTime(dr["StatsDate"]) : DateTime.Now,
                             CustomerIntId = dr["CusIdInt"] != DBNull.Value ? Convert.ToInt32(dr["CusIdInt"]) : 0,
                             IsLead = (dr["IsLead"] != DBNull.Value ? Convert.ToBoolean(dr["IsLead"]) : false)


                         }).ToList();

            SalesReportCountModel SalesReportCountModel = new SalesReportCountModel();
            SalesReportCountModel = (from DataRow dr in ds.Tables[1].Rows
                                     select new SalesReportCountModel()
                                     {
                                         TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                     }).FirstOrDefault();
            TotalSalesAmountModel TotalSalesAmountModel = new TotalSalesAmountModel();
            UserActivityCustomerModel SalesReportModel = new UserActivityCustomerModel();
            SalesReportModel.ListUserActivity = buildList;
            SalesReportModel.InvoiceReportCountModel = SalesReportCountModel;
            SalesReportModel.TotalInvoiceAmountModel = TotalSalesAmountModel;
            return SalesReportModel;
        }

        #endregion

        public RMRCreditModel GetAllRMRCreditList(int pageno, int pagesize, DateTime? startdate, DateTime? enddate, string searchtext, string order, string logstartdate, string logenddate)
        {

            DataSet ds = _CustomerDataAccess.GetAllRMRCreditList(pageno, pagesize, startdate, enddate, searchtext, order, logstartdate, logenddate);
            List<CreditModel> buildList = new List<CreditModel>();
            buildList = (from DataRow dr in ds.Tables[0].Rows
                         select new CreditModel()
                         {
                             CusId = dr["CusId"] != DBNull.Value ? Convert.ToInt32(dr["CusId"]) : 0,
                             CreditAmount = dr["CreditAmount"] != DBNull.Value ? Convert.ToDouble(dr["CreditAmount"]) : 0.0,
                             CreditUsedAmount = dr["AmountUsed"] != DBNull.Value ? Convert.ToDouble(dr["AmountUsed"]) : 0.0,
                             //CreditAmountAvailable = dr["CreditAmountAvailable"] != DBNull.Value ? Convert.ToDouble(dr["CreditAmountAvailable"]) : 0.0,
                             CustomerName = dr["CustomerName"].ToString(),
                             //CreditUsedAmount = dr["CreditUsedAmount"] != DBNull.Value ? Convert.ToDouble(dr["CreditUsedAmount"]) : 0.0,
                             CustomerStatus = dr["CustomerStatus"].ToString(),
                             CreditIssueDate = dr["CreditIssueDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreditIssueDate"]) : DateTime.Now,
                             CreditReason = dr["CreditReason"].ToString(),
                             CreditUser = dr["CreditUser"].ToString(),
                             CustomerNo = dr["CustomerNo"].ToString(),

                         }).ToList();

            SalesReportCountModel SalesReportCountModel = new SalesReportCountModel();
            SalesReportCountModel = (from DataRow dr in ds.Tables[1].Rows
                                     select new SalesReportCountModel()
                                     {
                                         TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                     }).FirstOrDefault();

            RMRCreditModel ReportModel = new RMRCreditModel();
            ReportModel.RMRCreditList = buildList;
            ReportModel.TotalRMRCredit = SalesReportCountModel;
            return ReportModel;
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
        public long InsertGeeseRoute(GeeseRoute GR)
        {
            return _GeeseRouteDataAccess.Insert(GR);
        }
        public GeeseRoute GetRouteById(int id)
        {
            return _GeeseRouteDataAccess.Get(id);
        }
        public long UpdateRoute(GeeseRoute GR)
        {
            return _GeeseRouteDataAccess.Update(GR);
        }
        public List<GeeseRoute> GetRouteList()
        {
            DataTable dt = _GeeseRouteDataAccess.GetRouteList();
            List<GeeseRoute> RouteList = new List<GeeseRoute>();
            RouteList = (from DataRow dr in dt.Rows
                         select new GeeseRoute()
                         {
                             RouteId = (Guid)dr["RouteId"],
                             Name = dr["Name"].ToString()
                         }).ToList();
            return RouteList;
        }
        public GeeseRoute GetGeeseRouteByRouteId(Guid Id)
        {
            return _GeeseRouteDataAccess.GetByQuery(string.Format(" RouteId = '{0}' ", Id)).FirstOrDefault();
        }
        public GeeseRoute GetGeeseRouteByName(string Name)
        {
            return _GeeseRouteDataAccess.GetByQuery(string.Format("Name = '{0}' ", Name)).FirstOrDefault();
        }
        public long InsertCustomerRoute(CustomerRoute CR)
        {
            return _CustomerRouteDataAccess.Insert(CR);
        }
        public CustomerRoute GetCustomerRouteByCustomerId(Guid Id)
        {
            return _CustomerRouteDataAccess.GetByQuery(string.Format(" CustomerId = '{0}' ", Id)).FirstOrDefault();
        }
        public long DeleteCutomerRouteById(int id)
        {
            return _CustomerRouteDataAccess.Delete(id);
        }
        public RouteCustomerListModel GetCustomerByRouteId(Guid RouteId)
        {
            DataSet dsResult = _CustomerRouteDataAccess.GetCustomerListByRouteId(RouteId);
            DataTable dt = dsResult.Tables[0];

            List<RouteCustomerList> RouteCusList = new List<RouteCustomerList>();
            RouteCusList = (from DataRow dr in dt.Rows
                            select new RouteCustomerList()
                            {
                                RouteId = (Guid)dr["RouteId"],
                                Name = dr["Name"].ToString(),
                                CustomerName = dr["CustomerName"].ToString(),
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0
                            }).ToList();

            RouteCustomerListModel RCL = new RouteCustomerListModel();
            RCL.RouteCustomerList = RouteCusList;
            return RCL;
        }
        public bool DeleteRouteByRouteId(Guid RouteId)
        {
            return _GeeseRouteDataAccess.DeleteRouteByRouteId(RouteId);
        }
        public bool UpdateCustomerRouteByRouteId(Guid RouteId, string Name)
        {
            return _GeeseRouteDataAccess.UpdateCustomerRouteByRouteId(RouteId, Name);
        }
        public long InsertEmployeeRoute(EmployeeRoute ER)
        {
            return _EmployeeRouteDataAccess.Insert(ER);
        }
        public bool DeleteEmployeeRouteByUserId(Guid UserId)
        {
            return _EmployeeRouteDataAccess.DeleteEmployeeRouteByUserId(UserId);
        }
        public List<RouteList> GetAllEmployeeRouteByUserId(Guid UserId)
        {
            DataTable dt = _EmployeeRouteDataAccess.GetAllEmployeeRouteByUserId(UserId);
            List<RouteList> RouteList = new List<RouteList>();
            if (dt != null)
                RouteList = (from DataRow dr in dt.Rows
                             select new RouteList()
                             {
                                 RouteId = (Guid)dr["RouteId"],
                                 Name = dr["Name"].ToString(),
                             }).ToList();
            return RouteList;
        }
        public int GetTotalCustomerCountByCompanyId(Guid companyID)
        {
            int count = 0;
            DataTable dt = _CustomerDataAccess.GetTotalCustomerCountByCompanyId(companyID);
            if (dt.Rows.Count > 0)
            {
                count = dt.Rows[0]["TotalCustomer"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["TotalCustomer"]) : 0;
            }
            return count;
        }
        public int GetTotalRouteCountByUserId(Guid UserId)
        {
            int count = 0;
            DataTable dt = _GeeseRouteDataAccess.GetTotalRouteCountByUserId(UserId);
            if (dt != null && dt.Rows.Count > 0)
            {
                count = dt.Rows[0]["TotalRoute"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["TotalRoute"]) : 0;
            }
            return count;
        }
        public int GetTotalCheckInCountByUserId(Guid UserId, DateTime Today, DateTime EndDay)
        {
            int count = 0;
            DataTable dt = _CustomerCheckLogDataAccess.GetTotalCheckInCountByUserId(UserId, Today, EndDay);
            if (dt.Rows.Count > 0)
            {
                count = dt.Rows[0]["TotalCheckIn"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["TotalCheckIn"]) : 0;
            }
            return count;
        }
        public int GetTotalGeeseCount(Guid UserId, DateTime Today, DateTime EndDay)
        {
            int count = 0;
            DataTable dt = _CustomerCheckLogDataAccess.GetTotalGeeseCount(UserId, Today, EndDay);
            if (dt.Rows.Count > 0)
            {
                count = dt.Rows[0]["TotalGeese"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["TotalGeese"]) : 0;
            }
            return count;
        }
        public bool UpdateCustomerRoute(CustomerRoute CR)
        {
            return _CustomerRouteDataAccess.Update(CR) > 0;
        }
        #region History Report Section
        public CreditHistoryModel GetCreditHistoryListByCompanyId(CreditHistoryFilter Filter)
        {
            DataSet dsResult = _CustomerCreditCheckDataAccess.GetCreditHistoryListByCompanyId(Filter);

            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];

            CreditHistoryModel Model = new CreditHistoryModel();
            Model.Total = dt.Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["TotalCount"]) : 0;

            List<CreditHistory> CreditHistoryList = new List<CreditHistory>();
            CreditHistoryList = (from DataRow dr in dt1.Rows
                                 select new CreditHistory()
                                 {
                                     Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                     CustomerIntId = dr["CusIntId"] != DBNull.Value ? Convert.ToInt32(dr["CusIntId"]) : 0,
                                     Name = dr["CustomerName"].ToString(),
                                     Date = dr["CreditCheckDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreditCheckDate"]) : new DateTime(),
                                     Address = dr["Address"].ToString(),
                                     Bureau = dr["CreditBureau"].ToString(),
                                     Score = dr["Score"].ToString()
                                 }).ToList();

            Model.CreditHistoryList = CreditHistoryList;
            return Model;
        }
        public DataTable DownloadCreditHistoryListByCompanyId(CreditHistoryFilter Filter)
        {
            return _CustomerCreditCheckDataAccess.DownloadCreditHistoryListByCompanyId(Filter);
        }
        public SMSHistoryReportModel GetSMSHistoryListByCompanyId(SMSHistoryFilter Filter)
        {
            DataSet dsResult = _SMSHistoryDataAccess.GetSMSHistoryListByCompanyId(Filter);

            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];

            SMSHistoryReportModel Model = new SMSHistoryReportModel();
            Model.Total = dt.Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["TotalCount"]) : 0;

            List<SMSHistoryReport> SMSHistoryList = new List<SMSHistoryReport>();
            SMSHistoryList = (from DataRow dr in dt1.Rows
                              select new SMSHistoryReport()
                              {
                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                  ToMobile = dr["ToMobileNo"].ToString(),
                                  SentDate = dr["SMSSentDate"] != DBNull.Value ? Convert.ToDateTime(dr["SMSSentDate"]) : new DateTime(),
                                  FromMobile = dr["FromMobileNo"].ToString(),
                                  FromName = dr["FromName"].ToString()
                              }).ToList();

            Model.SMSHistoryList = SMSHistoryList;
            return Model;
        }
        public DataTable DownloadSMSHistoryListByCompanyId(SMSHistoryFilter Filter)
        {
            return _SMSHistoryDataAccess.DownloadSMSHistoryListByCompanyId(Filter);
        }
        #endregion

        #region Sales Person Report
        public SalesPersonReportModel GetSalesPersonListByCompanyId(SalesPersonFilter Filter)
        {
            DataSet dsResult = _CustomerDataAccess.GetSalesPersonListByCompanyId(Filter);

            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            DataTable dt2 = dsResult.Tables[2];

            SalesPersonReportModel Model = new SalesPersonReportModel();
            Model.Total = dt.Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["TotalCount"]) : 0;

            List<SalesPersonReport> SalesPersonList = new List<SalesPersonReport>();
            SalesPersonList = (from DataRow dr in dt1.Rows
                               select new SalesPersonReport()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   Name = dr["SalesPerson"].ToString(),
                                   //ConvertionDate = dr["ConvertionDate"] != DBNull.Value ? Convert.ToDateTime(dr["ConvertionDate"]) : new DateTime(),
                                   Sales = dr["Sales"] != DBNull.Value ? Convert.ToInt32(dr["Sales"]) : 0,
                                   Pending = dr["Pending"] != DBNull.Value ? Convert.ToInt32(dr["Pending"]) : 0,
                                   Completed = dr["Completed"] != DBNull.Value ? Convert.ToInt32(dr["Completed"]) : 0
                               }).ToList();
            Model.TotalSales = dt2.Rows[0]["TotalSales"] != DBNull.Value ? Convert.ToInt32(dt2.Rows[0]["TotalSales"]) : 0;
            Model.TotalPending = dt2.Rows[0]["TotalPending"] != DBNull.Value ? Convert.ToInt32(dt2.Rows[0]["TotalPending"]) : 0;
            Model.TotalCompleted = dt2.Rows[0]["TotalCompleted"] != DBNull.Value ? Convert.ToInt32(dt2.Rows[0]["TotalCompleted"]) : 0;
            Model.SalesPersonList = SalesPersonList;
            return Model;
        }
        public DataTable DownloadSalesPersonListByCompanyId(SalesPersonFilter Filter)
        {
            return _CustomerDataAccess.DownloadSalesPersonListByCompanyId(Filter);
        }
        #endregion
    }
}
