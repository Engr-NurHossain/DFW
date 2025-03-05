using HS.DataAccess;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HS.Entities;
using System.Data;
using System.Web;

namespace HS.Facade
{
    public class SchedulerFacade : BaseFacade
    {
        UserOrganizationDataAccess _UserOrganizationDataAccessMaster = null;
        OrganizationDataAccess _OrganizationDataAccess = null;
        CustomerDataAccess _CustomerDataAccess = null;
        InvoiceDataAccess _InvoiceDataAccess = null;
        InvoiceDetailDataAccess _InvoiceDetailDataAccess = null;
        GlobalSettingDataAccess _GlobalSettingDataAccess = null;
        TransactionDataAccess _TransactionDataAccess = null;
        TransactionHistoryDataAccess _TransactionHistoryDataAccess = null;
        CustomerNoteDataAccess _CustomerNoteDataAccess = null;
        NoteAssignDataAccess _NoteAssignDataAccess = null;
        EmployeeDataAccess _EmployeeDataAccess = null;
        UserLoginDataAccess _UserLoginDataAccess = null;
        UserCompanyDataAccess _UserCompanyDataAccess = null;
        CompanyDataAccess _CompanyDataAccess = null;
        CustomerAgreementDataAccess _CustomerAgreementDataAccess = null;
        NotificationDataAccess _NotificationDataAccess = null;
        NotificationUserDataAccess _NotificationUserDataAccess = null;
        CustomerCompanyDataAccess _CustomerCompanyDataAccess = null;
        CustomerContactTrackDataAccess _CustomerContactTrackDataAccess = null;
        DeclinedTransactionsDataAccess _DeclinedTransactionsDataAccess = null;
        ActivityDataAccess _ActivityDataAccess = null;
        LookupDataAccess _LookupDataAccess = null;
        CustomerAppointmentDataAccess _CustomerAppointmentDataAccess = null;
        TicketDataAccess _TicketDataAccess = null;
        TicketUserDataAccess _TicketUserDataAccess = null; 
        CustomerMigrationDataAccess _CustomerMigrationDataAccess = null;
        RMRBillingMismatchDataAccess _RMRBillingMismatchDataAccess = null;
        WebsiteLocationDataAccess _WebsiteLocationDataAccess = null;
        ResturantOrderDataAccess _ResturantOrderDataAccess = null;
        RecurringBillingScheduleDataAccess _RecurringBillingScheduleDataAccess = null;
        RecurringBillingScheduleItemsDataAccess _RecurringBillingScheduleItemsDataAccess = null;
        TrackingNumberRecordedDataAccess _TrackingNumberRecordedDataAccess = null;
        TrackingNumberSettingDataAccess _TrackingNumberSettingDataAccess = null;
        CustomerCreditDataAccess _CustomerCreditDataAccess = null;
        //ErrorLogDataAccess _ErrorLogDataAccess = null;
        RestaurantCouponsDataAccess _RestaurantCouponsDataAccess = null;
        ResturantSystemSettingDataAccess _ResturantSystemSettingDataAccess = null;
        EmployeeEvaluationDataAccess _EmployeeEvaluationDataAccess = null;
        EmployeePTOHourLogDataAccess _EmployeePTOHourLogDataAccess = null;
        EmployeePtoAccrualRateDataAccess _EmployeePtoAccrualRateDataAccess = null;
        public SchedulerFacade(ClientContext clientContext)
            : base(clientContext)
        {
            if (_UserOrganizationDataAccessMaster == null)
                _UserOrganizationDataAccessMaster = new UserOrganizationDataAccess(clientContext, System.Configuration.ConfigurationManager.ConnectionStrings["hsCoonectionString"].ConnectionString);
            if (_OrganizationDataAccess == null)
                _OrganizationDataAccess = new OrganizationDataAccess(clientContext, System.Configuration.ConfigurationManager.ConnectionStrings["hsCoonectionString"].ConnectionString);
        }
        public SchedulerFacade()
        {
            if (_UserOrganizationDataAccessMaster == null)
                _UserOrganizationDataAccessMaster = new UserOrganizationDataAccess(System.Configuration.ConfigurationManager.ConnectionStrings["hsCoonectionString"].ConnectionString);
            if (_OrganizationDataAccess == null)
                _OrganizationDataAccess = new OrganizationDataAccess(System.Configuration.ConfigurationManager.ConnectionStrings["hsCoonectionString"].ConnectionString);
        }
        public SchedulerFacade(string ConnectionStr)
        {
            //if (_ErrorLogDataAccess == null)
            //    _ErrorLogDataAccess = new ErrorLogDataAccess(ConnectionStr);
            if (_CustomerDataAccess == null)
                _CustomerDataAccess = new CustomerDataAccess(ConnectionStr);
            if (_InvoiceDataAccess == null)
                _InvoiceDataAccess = new InvoiceDataAccess(ConnectionStr);
            if (_InvoiceDetailDataAccess == null)
                _InvoiceDetailDataAccess = new InvoiceDetailDataAccess(ConnectionStr);
            if (_GlobalSettingDataAccess == null)
                _GlobalSettingDataAccess = new GlobalSettingDataAccess(ConnectionStr);
            if (_TransactionDataAccess == null)
                _TransactionDataAccess = new TransactionDataAccess(ConnectionStr);
            if (_TransactionHistoryDataAccess == null)
                _TransactionHistoryDataAccess = new TransactionHistoryDataAccess(ConnectionStr);
            if (_CustomerNoteDataAccess == null)
                _CustomerNoteDataAccess = new CustomerNoteDataAccess(ConnectionStr);
            if (_NoteAssignDataAccess == null)
                _NoteAssignDataAccess = new NoteAssignDataAccess(ConnectionStr);
            if (_EmployeeDataAccess == null)
                _EmployeeDataAccess = new EmployeeDataAccess(ConnectionStr);
            if (_UserLoginDataAccess == null)
                _UserLoginDataAccess = new UserLoginDataAccess(ConnectionStr);
            if (_UserCompanyDataAccess == null)
                _UserCompanyDataAccess = new UserCompanyDataAccess(ConnectionStr);
            if (_CompanyDataAccess == null)
                _CompanyDataAccess = new CompanyDataAccess(ConnectionStr);
            if (_CustomerAgreementDataAccess == null)
                _CustomerAgreementDataAccess = new CustomerAgreementDataAccess(ConnectionStr);
            if (_NotificationDataAccess == null)
                _NotificationDataAccess = new NotificationDataAccess(ConnectionStr);
            if (_NotificationUserDataAccess == null)
                _NotificationUserDataAccess = new NotificationUserDataAccess(ConnectionStr);
            if (_CustomerCompanyDataAccess == null)
                _CustomerCompanyDataAccess = new CustomerCompanyDataAccess(ConnectionStr);
            if (_CustomerContactTrackDataAccess == null)
                _CustomerContactTrackDataAccess = new CustomerContactTrackDataAccess(ConnectionStr);
            if (_DeclinedTransactionsDataAccess==null)
                _DeclinedTransactionsDataAccess = new DeclinedTransactionsDataAccess(ConnectionStr);
            if (_ActivityDataAccess == null)
                _ActivityDataAccess = new ActivityDataAccess(ConnectionStr);
            if (_LookupDataAccess == null)
                _LookupDataAccess = new LookupDataAccess(ConnectionStr);
            if (_CustomerAppointmentDataAccess == null)
                _CustomerAppointmentDataAccess = new CustomerAppointmentDataAccess(ConnectionStr);
            if (_TicketDataAccess == null)
                _TicketDataAccess = new TicketDataAccess(ConnectionStr);
            if (_TicketUserDataAccess == null)
                _TicketUserDataAccess = new TicketUserDataAccess(ConnectionStr);
            if (_CustomerMigrationDataAccess == null)
                _CustomerMigrationDataAccess = new CustomerMigrationDataAccess(ConnectionStr);
            if (_RMRBillingMismatchDataAccess == null)
                _RMRBillingMismatchDataAccess = new RMRBillingMismatchDataAccess(ConnectionStr);
            if (_WebsiteLocationDataAccess == null)
                _WebsiteLocationDataAccess = new WebsiteLocationDataAccess(ConnectionStr);
            if (_ResturantOrderDataAccess == null)
                _ResturantOrderDataAccess = new ResturantOrderDataAccess(ConnectionStr);
            if (_RecurringBillingScheduleDataAccess == null)
                _RecurringBillingScheduleDataAccess = new RecurringBillingScheduleDataAccess(ConnectionStr);
            if (_RecurringBillingScheduleItemsDataAccess == null)
                _RecurringBillingScheduleItemsDataAccess = new RecurringBillingScheduleItemsDataAccess(ConnectionStr);
            if (_TrackingNumberRecordedDataAccess == null)
                _TrackingNumberRecordedDataAccess = new TrackingNumberRecordedDataAccess(ConnectionStr);
            if (_TrackingNumberSettingDataAccess == null)
                _TrackingNumberSettingDataAccess = new TrackingNumberSettingDataAccess(ConnectionStr);
            if (_RestaurantCouponsDataAccess == null)
                _RestaurantCouponsDataAccess = new RestaurantCouponsDataAccess(ConnectionStr);
            if (_CustomerCreditDataAccess == null)
                _CustomerCreditDataAccess = new CustomerCreditDataAccess(ConnectionStr);
            if (_ResturantSystemSettingDataAccess == null)
                _ResturantSystemSettingDataAccess = new ResturantSystemSettingDataAccess(ConnectionStr);            
            
            if (_EmployeeEvaluationDataAccess == null)
                _EmployeeEvaluationDataAccess = new EmployeeEvaluationDataAccess(ConnectionStr);
            if (_EmployeePTOHourLogDataAccess == null)
                _EmployeePTOHourLogDataAccess = new EmployeePTOHourLogDataAccess(ConnectionStr);
            if (_EmployeePtoAccrualRateDataAccess == null)
                _EmployeePtoAccrualRateDataAccess = new EmployeePtoAccrualRateDataAccess(ConnectionStr);
        }
        public List<Customer> GetACHAndCCSubscribedCustomer()
        {
            return _CustomerDataAccess.GetACHAndCCSubscribedCustomer();
        }

        public List<Organization> GetAllOrganizations()
        {
            return _OrganizationDataAccess.GetAll();
        }

        public List<Customer> GetSubscribedCustomerList(Guid CompanyId)
        {
            return _CustomerDataAccess.GetSubscribedCustomerList(CompanyId); 
        }
        public List<Customer> GetCustomerListForRecurringBillingByCompanyId(Guid CompanyId)
        {
            return _CustomerDataAccess.GetCustomerListForRecurringBillingByCompanyId(CompanyId);
        }
        public List<CustomerCancellationQueue> GetAllCancelledRequestedCustomer()
        {
            DataTable dt = _CustomerDataAccess.GetAllCancellRequestedCustomer();
            List<CustomerCancellationQueue> RequestedCustomerList = new List<CustomerCancellationQueue>();

            RequestedCustomerList = (from DataRow dr in dt.Rows
                                       select new CustomerCancellationQueue()
                                       {
                                           Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                           CustomerId = (Guid)dr["CustomerId"],
                                           CancellationDate = dr["CancellationDate"] != DBNull.Value ? Convert.ToDateTime(dr["CancellationDate"]) : new DateTime(),
                                       }).ToList();
            return RequestedCustomerList;
        }
        public List<Invoice> GetExpiringEstimateList(Guid CompanyId,int day)
        {
            return _InvoiceDataAccess.GetExpiringEstimateListByCompanyIdAndDay(CompanyId,day);
        }

        public List<Activity> GetExpiringActivityList()
        {
            return _ActivityDataAccess.GetExpiringActivityListByDay();
        }

        public List<Activity> GetExpiringActivityListByDueDate(string FromDate, string ToDate)
        {
            return _ActivityDataAccess.GetByQuery(string.Format("DueDate between '{0}' and '{1}'", FromDate, ToDate));
        }

        public GlobalSetting GetGlobalsettingBySearchKeyAndCompanyId(string SearchKey, Guid CompanyId)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format(" SearchKey ='{0}' and IsActive =1 and CompanyId = '{1}'", SearchKey, CompanyId)).FirstOrDefault();
        }

        public int InsertTransactionHistory(TransactionHistory trhs)
        {
            return (int)_TransactionHistoryDataAccess.Insert(trhs);
        }

        public int InsertTransaction(Transaction tr)
        {
            return (int)_TransactionDataAccess.Insert(tr);
        }

        public Customer GetCustomerBySubscriptionId(int subscriptionId)
        {
            return _CustomerDataAccess.GetByQuery(string.Format("AuthorizeRefId ='{0}'", subscriptionId)).FirstOrDefault();
        }
     
        public Transaction GetTransactionByTransactionId(string transactionId)
        {
            return _TransactionDataAccess.GetByQuery(string.Format("CardTransactionId ='{0}'", transactionId)).FirstOrDefault();
        }
        public string GetAuthAPILoginIdByCompanyId(Guid companyId, bool IsAch)
        {
            string SearchKey = "AuthAPILoginId";
            if (IsAch)
            {
                SearchKey = "AuthAPILoginIdACH";
            }
            string result = string.Empty;
            GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", companyId, SearchKey)).FirstOrDefault();
            if (globalsetting != null)
            {
                result = globalsetting.Value;
            }
            return result;
        }

        public string GetAuthTransactionKeyByCompanyId(Guid companyId, bool IsAch)
        {
            string SearchKey = "AuthTransactionKey";
            if (IsAch)
            {
                SearchKey = "AuthTransactionKeyACH";
            }
            string result = string.Empty;
            GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", companyId, SearchKey)).FirstOrDefault();
            if (globalsetting != null)
            {
                result = globalsetting.Value;
            }
            return result;
        }

        public List<Customer> GetARBSubscribedCustomerList()
        {
            return _CustomerDataAccess.GetARBSubscribedCustomerList(); 
        }
        public List<Customer> GetForteSubscribedCustomerList(Guid CompanyId)
        {
            return _CustomerDataAccess.GetForteSubscribedCustomerList(CompanyId);
        }

        public string GetForteAPILoginIdByCompanyId(Guid CompanyId, bool IsACH)
        {
            string result = string.Empty;
            string SearchKey = "ForteAPILoginId";
            string ApiLogin = RMRCacheKey.ForteAPILoginId + CompanyId.ToString();

            if (System.Web.HttpRuntime.Cache[ApiLogin] == null)
            {
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    result = globalsetting.Value;
                    System.Web.HttpRuntime.Cache[ApiLogin] = result;
                }
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[ApiLogin];
            }
            return result;
        }

        public CustomerContactTrack GetCustomerContactTrackByPlatformId(int id)
        {
            return _CustomerContactTrackDataAccess.GetByQuery(string.Format(" PlatformId = '{0}' ", id)).FirstOrDefault();
        }

        public string GetForteTransactionKeyByCompanyId(Guid CompanyId, bool IsAch)
        {
            string ForteKey = RMRCacheKey.ForteTransactionKey + CompanyId.ToString();
            string SearchKey = "ForteTransactionKey";
            string result = string.Empty;


            if (System.Web.HttpRuntime.Cache[ForteKey] == null)
            {
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    result = globalsetting.Value;
                    System.Web.HttpRuntime.Cache[ForteKey] = result;
                }
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[ForteKey];
            }
            return result;
        }

        public string GetForteOrganizationIdByCompanyId(Guid CompanyId, bool IsAch)
        {
            string ForteKey = RMRCacheKey.ForteOrganizationId + CompanyId.ToString();
            string SearchKey = "ForteOrganizationId";
            string result = string.Empty;


            if (System.Web.HttpRuntime.Cache[ForteKey] == null)
            {
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    result = globalsetting.Value;
                    System.Web.HttpRuntime.Cache[ForteKey] = result;
                }
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[ForteKey];
            }
            return result;
        } 
        public string GetForteLocationIdByCompanyId(Guid CompanyId, bool IsAch)
        {
            string ForteKey = RMRCacheKey.ForteLocationId + CompanyId.ToString();
            string SearchKey = "ForteLocationId";
            string result = string.Empty;


            if (System.Web.HttpRuntime.Cache[ForteKey] == null)
            {
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    result = globalsetting.Value;
                    System.Web.HttpRuntime.Cache[ForteKey] = result;
                }
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[ForteKey];
            }
            return result;
        }

        public string GetForteAuthAccountIdByCompanyId(Guid CompanyId, bool IsAch)
        {
            string ForteKey = RMRCacheKey.ForteAuthAccountId + CompanyId.ToString();
            string SearchKey = "ForteAuthAccountId";
            string result = string.Empty;


            if (System.Web.HttpRuntime.Cache[ForteKey] == null)
            {
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    result = globalsetting.Value;
                    System.Web.HttpRuntime.Cache[ForteKey] = result;
                }
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[ForteKey];
            }
            return result;
        }

        public List<Customer> GetSubscribedAllCustomer(bool IsActiveCheck)
        {
            string Query = " AuthorizeRefId is not null and AuthorizeRefId !='' and LEN(AuthorizeRefId) >6 and LEN(AuthorizeRefId) < 11 ";
            if (IsActiveCheck)
            {
                Query += " and IsActive = 1";
            }
            return _CustomerDataAccess.GetByQuery(Query);
        }

        public Invoice GetInvoiceByAddedDateAndCustomerId(Guid customerId, DateTime lastGeneratedInvoice, bool AutomatedBilling)
        {
            List<Invoice> InvoiceList = _InvoiceDataAccess.GetInvoiceByAddedDateAndCustomerId(customerId, lastGeneratedInvoice, AutomatedBilling);
             
            return InvoiceList.FirstOrDefault();
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
        public List<Lookup> GetLookupByKey(string key, bool IncludeInActive = false)
        {
            List<Lookup> resultLookup = new List<Lookup>();
            string currentLanguage = "en-US";
            //if (HttpContext.Current.Request.Cookies["__Lng"] != null)
            //{
            //    currentLanguage = HttpContext.Current.Request.Cookies["__Lng"].Value;
            //}
            string cachekey = key + currentLanguage;
            System.Web.HttpRuntime.Cache.Remove(cachekey);
            if (System.Web.HttpRuntime.Cache[cachekey] == null)
            {
                resultLookup = _LookupDataAccess.GetByQuery(string.Format(" DataKey = '{0}'  order by DataOrder ", key));
                foreach (var lookup in resultLookup)
                {
                    lookup.DisplayText = lookup.DisplayText;
                }
                System.Web.HttpRuntime.Cache[cachekey] = resultLookup;
            }
            else
            {
                resultLookup = (List<Lookup>)System.Web.HttpRuntime.Cache[cachekey];
            }
            if (!IncludeInActive && resultLookup != null && resultLookup.Count() > 0)
            {
                return resultLookup.Where(x => x.IsActive == true).ToList();
            }
            else
            {
                return resultLookup;
            }
        }

        public long InsertCustomer(Customer customer)
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
            return _CustomerDataAccess.Insert(customer);
        }
        public long InsertCustomerCompany(CustomerCompany cc)
        {
            return _CustomerCompanyDataAccess.Insert(cc);
        }

        public long InsertCustomerContactTrack(CustomerContactTrack CustomerContactTrack)
        {
            return _CustomerContactTrackDataAccess.Insert(CustomerContactTrack);
        }
        public CustomerContactTrack GetCustomerContactTrackByPlatformIdAndCustomerId(int PlatformId, Guid CustomerId)
        {
            return _CustomerContactTrackDataAccess.GetByQuery(string.Format("PlatformId ='{0}' and CustomerId ='{1}' ", PlatformId, CustomerId)).FirstOrDefault();
        }

     


        public int InsertInvoice(Invoice inv)
        {
            return (int)_InvoiceDataAccess.Insert(inv);
        }

        public bool UpdateInvoice(Invoice inv)
        {
            return _InvoiceDataAccess.Update(inv) > 0;
        }

        public int InsertInvoiceDetails(InvoiceDetail invDet)
        {
            if (!invDet.Taxable.HasValue)
            {
                invDet.Taxable = true;
            }
            return (int)_InvoiceDetailDataAccess.Insert(invDet);
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
            return _CustomerDataAccess.Update(customer) > 0;
        }

        public string GetSalesTax(Guid CompanyId)
        {
            GlobalSetting gobset = _GlobalSettingDataAccess.GetByQuery(string.Format("SearchKey = 'Sales Tax' and CompanyId = '{0}' and IsActive = 'true'", CompanyId)).FirstOrDefault();
            if (gobset != null)
            {
                return gobset.Value;
            }

            return "0";
        }

        public List<Customer> GetBilledCustomer(int InvoicePullingDays)
        {
            return _CustomerDataAccess.GetBilledCustomer(InvoicePullingDays);
        }

        public string GetCustomerAddressFormat(Guid CompanyId)
        {
            string CustomerAddressPdfFormat = RMRCacheKey.CustomerAddressPdfFormat + CompanyId.ToString();

            string result = string.Empty;
            string SearchKey = "CustomerAddressPdfFormat";
            GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
            result = globalsetting == null ? "" : globalsetting.Value;
            return result;
        }

        public List<Invoice> GetARBInvoicesByCustomerId(Guid customerId, int InvoicePullingDays = 5)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format(@" CustomerId = '{0}' 
                                                                AND Status !='Paid' AND Status != 'Declined'
                                                                AND CreatedDate > '{1}'
                                                                AND IsARBInvoice = 1
                                                                ", customerId, DateTime.Today.AddDays((InvoicePullingDays * -1)).ToString("yyyy-MM-dd 00:00:00")));
        }

        public int GetRetryTrasactionCheck(Guid CompanyId)
        {
            GlobalSetting gobset = _GlobalSettingDataAccess.GetByQuery(string.Format("SearchKey = 'RetryTrasactionCheck' and CompanyId = '{0}' and IsActive = 'true'", CompanyId)).FirstOrDefault();
            if (gobset != null && !string.IsNullOrWhiteSpace(gobset.Value))
            {
                return Convert.ToInt32(gobset.Value);
            }

            return 3;
        }

        public List<CustomerNote> GetAllCustomerNoteByCompanyIdAndIsSchedule(Guid CompanyId, string TodaysDate, string TomorrowDate, string ConStr)
        {
            return _CustomerNoteDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and IsShedule = 'true' and ReminderDate between '{1}' and '{2}'", CompanyId, TodaysDate, TomorrowDate));
        }

        public Customer GetCustomerByCustomerId(Guid cusid)
        {
            return _CustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", cusid)).FirstOrDefault();
        }

        public NoteAssign GetNoteAssignByNoteId(int id)
        {
            return _NoteAssignDataAccess.GetByQuery(string.Format("NoteId = '{0}'", id)).FirstOrDefault();
        }

        public List<NoteAssign> GetNoteAssignListByNoteId(int id)
        {
            return _NoteAssignDataAccess.GetByQuery(string.Format(" NoteId = '{0}'", id)).ToList();
        }

        public NoteAssign GetNoteAssignById(int id)
        {
            return _NoteAssignDataAccess.GetByQuery(string.Format("Id = '{0}'", id)).FirstOrDefault();
        }

        public Employee GetEmployeeByEmployeeId(Guid empid)
        {
            return _EmployeeDataAccess.GetByQuery(string.Format("UserId = '{0}'", empid)).FirstOrDefault();
        }

        public List<CustomerNote> GetCustomerNotesById(int value)
        {
            return _CustomerNoteDataAccess.GetByQuery(string.Format("Id = '{0}'", value));
        }

        public Company GetCompanyByCompanyId(Guid companyId)
        {
            return _CompanyDataAccess.GetByQuery(string.Format("CompanyId='{0}'",companyId)).FirstOrDefault();
        }

        public int InsertCustomerAgreement(CustomerAgreement objagree)
        {
            return (int) _CustomerAgreementDataAccess.Insert(objagree);
        }

        public List<CustomerNote> GetTodaysReminders()
        {
            return _CustomerNoteDataAccess.GetTodaysReminders();
        }

        public List<NoteAssign> GetAllAssignedUsersByCustomerNoteId(int Id)
        {
            return _NoteAssignDataAccess.GetByQuery(string.Format("NoteId = '{0}'", Id));
        }

        public int InsertNotification(Notification notification)
        {
            return (int)_NotificationDataAccess.Insert(notification);
        }

        public int InsertNotificationUser(NotificationUser nu)
        {
            return (int)_NotificationUserDataAccess.Insert(nu);
        }

        public CustomerCompany GetCustomerCompanyByCustomerId(Guid customerId)
        {
            return _CustomerCompanyDataAccess.GetByQuery(string.Format("CustomerId = '{0}'",customerId)).FirstOrDefault();
        }

        public int InsertDeclinedTransaction(DeclinedTransactions dt)
        {
            return (int) _DeclinedTransactionsDataAccess.Insert(dt);
        }

        public List<InvoiceDetail> GetUnpaidInvoiceDetailListByCustomerId(Guid customerId)
        {
            return _InvoiceDetailDataAccess.GetUnpaidInvoiceDetailListByCustomerId(customerId);
        }

        public List<Invoice> GetUnpaidInvoiceListByCustomerId(Guid customerId)
        {
            return _InvoiceDataAccess.GetUnpaidInvoiceDetailListByCustomerId(customerId);
        }

        public DeclinedTransactions GetDeclinedTransactionByTransactionId(string transId)
        {
            return _DeclinedTransactionsDataAccess.GetByQuery(string.Format(" TransactionId = '{0}'",transId)).FirstOrDefault();
        }

        public List<CustomerAppointment> GetCustomerAppointmentListByDateSchedule(string date, Guid comid)
        {
            DataTable dt = _CustomerAppointmentDataAccess.GetCustomerAppointmentListByDateSchedule(date, comid);
            List<CustomerAppointment> CustomerAppointmentList = new List<CustomerAppointment>();

            CustomerAppointmentList = (from DataRow dr in dt.Rows
                                       select new CustomerAppointment()
                                       {
                                           Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                           AppointmentId = (Guid)dr["AppointmentId"],
                                           CustomerName = dr["CustomerName"].ToString()
                                       }).ToList();
            return CustomerAppointmentList;
        }

        public List<Ticket> GetTicketListByTicketTypeAndCompanyId(Guid companyid)
        {
            return _TicketDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and TicketType = 'Service' and ([Status] != 'Completed' or IsClosed = 0)", companyid)).ToList();
        }

        public bool UpdateTicket(Ticket ticket)
        {
            return _TicketDataAccess.Update(ticket) > 0;
        }

        public CustomerAppointment GetAppointmentByAppointmentId(Guid appointmentid)
        {
            return _CustomerAppointmentDataAccess.GetByQuery(string.Format("AppointmentId = '{0}'", appointmentid)).FirstOrDefault();
        }

        public bool UpdateAppointment(CustomerAppointment appointment)
        {
            return _CustomerAppointmentDataAccess.Update(appointment) > 0;
        }

        public GlobalSetting GetGlobalSettingByKey(string key)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("SearchKey = '{0}'", key)).FirstOrDefault();
        }

        public int InsertRMRBillingMismatch(RMRBillingMismatch mismatch)
        {
            return (int)_RMRBillingMismatchDataAccess.Insert(mismatch);
        }

        public int InsertCustomerNote(CustomerNote note)
        {
            return (int)_CustomerNoteDataAccess.Insert(note); 
        }

        public RMRBillingMismatch GetRMRBillingMismatchByTransactionID(string transaction_id)
        {
            return _RMRBillingMismatchDataAccess.GetByQuery(string.Format(" TransactionId = '{0}'", transaction_id)).FirstOrDefault();
        }

        public Employee GetemployeeByFirstNameAndLastNameOrCSID(string userName,int CSID)
        {
            return _EmployeeDataAccess.GetByQuery(string.Format(" FirstName + ' '+ LastName = '{0}' OR CSId = {1} ", userName, CSID)).FirstOrDefault();
        }

        public bool UpdateEmployee(Employee emp)
        {
            return _EmployeeDataAccess.Update(emp)>0;
        }

        public int InsertTicket(Ticket ticket)
        {
            return (int)_TicketDataAccess.Insert(ticket); 
        }

        public int InsertTicketUser(TicketUser ticketUser)
        {
            return (int)_TicketUserDataAccess.Insert(ticketUser);
        }
        public int InsertCustomerAppointment(CustomerAppointment CustomerAppointment)
        {
            return (int)_CustomerAppointmentDataAccess.Insert(CustomerAppointment);
        }

        public int InsertCustomerMigration(CustomerMigration cm)
        {
            return (int)_CustomerMigrationDataAccess.Insert(cm);
        }

        public Employee GetEmployeeByEmailAddress(string userEmail)
        {
            return _EmployeeDataAccess.GetByQuery(string.Format(" Email = '{0}' ", userEmail)).FirstOrDefault();
        }

        public int InsertEmployee(Employee newEmp)
        {
            return (int)_EmployeeDataAccess.Insert(newEmp);
        }

        public int InsertUserLogin(UserLogin ul)
        {
            return (int)_UserLoginDataAccess.Insert(ul);
        }

        public int InsertUserCompany(UserCompany uc)
        {
            return (int)_UserCompanyDataAccess.Insert(uc);
        }

        public int InsertUserOrganization(UserOrganization userOrganization)
        {
            UserOrganization TemUo = _UserOrganizationDataAccessMaster.GetByQuery(string.Format("UserName = '{0}' and CompanyId ='{1}'", userOrganization.UserName, userOrganization.CompanyId)).FirstOrDefault();
            if (TemUo != null)
            {
                return TemUo.Id;
            }

            if (userOrganization.IsActive)
            {
                List<UserOrganization> TemUoList = _UserOrganizationDataAccessMaster.GetByQuery(string.Format(" UserName = '{0}'", userOrganization.UserName));
                if (TemUo != null && TemUoList.Count > 0)
                {
                    foreach (var item in TemUoList)
                    {
                        item.IsActive = false;
                        _UserOrganizationDataAccessMaster.Update(item);
                    }
                }
            }
            return (int)_UserOrganizationDataAccessMaster.Insert(userOrganization);
        }
        //public int InsertErrorLog(ErrorLog log)
        //{
        //    return (int)_ErrorLogDataAccess.Insert(log);
        //}
        #region UserX Calculation
        public List<Employee> GetAllInsideSalesEmployee()
        {
            DataTable dt = _EmployeeDataAccess.GetAllInsideSalesEmployee();
            List<Employee> EmployeeList = new List<Employee>();

            EmployeeList = (from DataRow dr in dt.Rows
                                     select new Employee()
                                     {
                                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                         UserId = (Guid)dr["UserId"],
                                     }).ToList();
            return EmployeeList;
        }
        public AllUserX GetAllUserXByUserId(DateTime StartDate, DateTime EndDate, Guid userId)
        {
            DataTable dt = _EmployeeDataAccess.GetAllUserXByUserId(StartDate, EndDate, userId);
            List<AllUserX> AllUserXList = new List<AllUserX>();

            AllUserXList = (from DataRow dr in dt.Rows
                            select new AllUserX()
                            {
                                FirstCallUserX = dr["FirstCallUserX"] != DBNull.Value ? Convert.ToDouble(dr["FirstCallUserX"]) : 0.0,
                                OverallUserX = dr["OverallUserX"] != DBNull.Value ? Convert.ToDouble(dr["OverallUserX"]) : 0.0,
                                SoldtofundedUserX = dr["SoldtofundedUserX"] != DBNull.Value ? Convert.ToDouble(dr["SoldtofundedUserX"]) : 0.0,
                                NumberofSalesUserX = dr["NumberofSalesUserX"] != DBNull.Value ? Convert.ToDouble(dr["NumberofSalesUserX"]) : 0.0,
                                AppointmentSetUserX = dr["AppointmentSetUserX"] != DBNull.Value ? Convert.ToDouble(dr["AppointmentSetUserX"]) : 0.0,
                            }).ToList();
            return AllUserXList.FirstOrDefault();
        }

        public CustomerNote GetCustomerNoteByThirdPartyId(int noteID)
        {
            return _CustomerNoteDataAccess.GetByQuery(string.Format("ThirdPartyId ='{0}'", noteID)).FirstOrDefault();
        }

        public Invoice GetInvoiceByTransactionId(string transId)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format(" TransactionId = '{0}'", transId)).FirstOrDefault();
        }

        public Invoice GetLatestARBInvoiceByCustomerId(Guid customerId)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format(@"IsARBInvoice = 1 and CustomerId = '{0}' and TransactionId = ''
                                                    order by id desc", customerId)).FirstOrDefault();
        }
        #endregion

        public WebsiteLocation GetWebsiteLocationByCompanyId(Guid comid)
        {
            return _WebsiteLocationDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", comid)).FirstOrDefault();
        }

        public List<ResturantOrder> GetAllOrderByExpirationTime(string exptime)
        {
            return _ResturantOrderDataAccess.GetByQuery(string.Format("[Status] = 'Pending' and OrderDate <= '{0}'", exptime)).ToList();
        }

        public bool UpdateOrder(ResturantOrder ord)
        {
            return _ResturantOrderDataAccess.Update(ord) > 0;
        }

        #region ForRecurringBilling
        public List<RecurringBillingSchedule> GetRecurringBillingByCustomerIdAndCompanyId( Guid companyId)
        {
            return _RecurringBillingScheduleDataAccess.GetByQuery(string.Format(@" CompanyId = '{0}' AND Status in ('Active','FreeTrial')", companyId));
        }
        public List<RecurringBillingScheduleItems> GetRecurringBillingItemsByScheduleId(Guid scheduleId)
        {
            return _RecurringBillingScheduleItemsDataAccess.GetByQuery(string.Format(@" ScheduleId = '{0}'", scheduleId));
        }
        public bool UpdateRecurringBilling(RecurringBillingSchedule obj)
        {
            return _RecurringBillingScheduleDataAccess.Update(obj) > 0;
        }
        public List<InvoiceDetail> GetUnpaidRecurringBillingInvoiceDetailsListByCustomerId(Guid customerId)
        {
            return _InvoiceDetailDataAccess.GetUnpaidRecurringBillingInvoiceDetailsListByCustomerId(customerId);
        }
        public List<InvoiceDetail> GetUnpaidOthersInvoiceDetailsListByCustomerId(Guid customerId)
        {
            return _InvoiceDetailDataAccess.GetUnpaidOthersBillingInvoiceDetailsListByCustomerId(customerId);
        }
        public List<Invoice> GetUnpaidRecurringBillingInvoiceListByCustomerId(Guid customerId, Guid ComId)
        {
            return _InvoiceDataAccess.GetUnpaidRecurringBillingInvoiceListByCustomerId(customerId, ComId);
        }
        public List<Invoice> GetUnpaidOthersInvoiceListByCustomerId(Guid customerId, Guid ComId)
        {
            return _InvoiceDataAccess.GetUnpaidOthersInvoiceListByCustomerId(customerId, ComId);
        }
        #endregion

        public List<TrackingNumberRecorded> GetAllTrackingNumberRecordedByCompanyId()
        {
            return _TrackingNumberRecordedDataAccess.GetAll();
        }

        public Customer GetCustomerByCellNumber(string number)
        {
            return _CustomerDataAccess.GetByQuery(string.Format("REPLACE(CellNo,'-','') = '{0}'", number.Replace("-", ""))).FirstOrDefault();
        }

        public int InsertTrackingNumberRecorded(TrackingNumberRecorded tnr)
        {
            return (int)_TrackingNumberRecordedDataAccess.Insert(tnr);
        }

        public TrackingNumberSetting GetTrackingNumberSettingByNumber(string number)
        {
            return _TrackingNumberSettingDataAccess.GetByQuery(string.Format("(REPLACE(TrackingNumber,'-','') = '{0}' or REPLACE(ForwardingNumber,'-','') = '{0}')", number.Replace("-", ""))).FirstOrDefault();
        }

        public bool UpdateTrackingNumberRecorded(TrackingNumberRecorded tnr)
        {
            return _TrackingNumberRecordedDataAccess.Update(tnr) > 0;
        }

        public List<RestaurantCoupons> GetAllRestaurantCouponsByCompanyIdAndEndDate(Guid comid, string date)
        {
            return _RestaurantCouponsDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and EndDate <= '{1}'", comid, date)).ToList();
        }

        public bool UpdateCoupons(RestaurantCoupons rc)
        {
            return _RestaurantCouponsDataAccess.Update(rc) > 0;
        }
        public int InsertCustomerCredit(CustomerCredit cc)
        {
            return (int)_CustomerCreditDataAccess.Insert(cc);
        }

        public ResturantSystemSetting GetSystemSettingByCompanyId(Guid comid)
        {
            return _ResturantSystemSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", comid)).FirstOrDefault();
        }

        public List<ResturantOrder> GetAllAcceptedOrdersByCompanyId(Guid comid)
        {
            return _ResturantOrderDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and [Status] = 'Accepted'", comid)).ToList();
        }

        public List<NoteAssign> GetAllNoteAssignByNoteIdList(List<int> lists)
        {
            return _NoteAssignDataAccess.GetByQuery(String.Format("NoteId in ({0})", string.Join("," ,lists)));
        }

        public List<Employee> GetAllEmplyeeByEmployeeIdList(List<Guid> userIdList)
        {
            return _EmployeeDataAccess.GetByQuery(String.Format("UserId in ('{0}')", string.Join("','", userIdList)));
        }
        public List<EmployeeEvaluation> GetAllEmplyeeEvaluationList()
        {
            return _EmployeeEvaluationDataAccess.GetAll();
        }
        public List<Customer> GetAllCustomerByCustomerIdList(IEnumerable<Guid> CustomerIdList)
        {
            return _CustomerDataAccess.GetByQuery(String.Format("CustomerId in ('{0}')", string.Join("','", CustomerIdList)));
        }
        public List<Invoice> GetARBInvoiceByCustomerIdAndDate(Guid customerId, DateTime today)
        {
            string SQL = "IsARBInvoice = 1 and InvoiceDate = '{0}' and CustomerId = '{1}'"; 
            return _InvoiceDataAccess.GetByQuery(string.Format(SQL,today.ToString("yyyy-MM-dd"), customerId)) ;
        }
        public Invoice GetInvoiceByInvoiceId(string InvoiceId)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format(" InvoiceId = '{0}'", InvoiceId)).FirstOrDefault();
        }

        #region Employee PTO Hours Calculation
        public List<Employee> GetAllEmployeeByHireDate(DateTime FromDate, DateTime EndDate)
        {
            return _EmployeeDataAccess.GetAllEmployeeByHirehDate(FromDate, EndDate);
        }
        public int InsertEmployeePTOHourLog(EmployeePTOHourLog model)
        {
            return (int)_EmployeePTOHourLogDataAccess.Insert(model);
        }
        public List<EmployeePTOHourLog> GetEmployeeTotalPtoHour(Guid UserId)
        {
            DataTable dt = _EmployeeDataAccess.GetEmployeePtohour(UserId);
            List<EmployeePTOHourLog> Emplist = new List<EmployeePTOHourLog>();
            Emplist = (from DataRow dr in dt.Rows
                       select new EmployeePTOHourLog()
                       {
                           TotalPTOHour = dr["TotalPTOHour"] != DBNull.Value ? Convert.ToDouble(dr["TotalPTOHour"]) : 0,
                       }).ToList();
            return Emplist;
        }
        public List<Pto> GetEmployeeAccrualTotalPtoHour(Guid UserId)
        {
            DataTable dt = _EmployeeDataAccess.GetEmployeePtoHourByUserId(UserId);
            List<Pto> Emplist = new List<Pto>();
            Emplist = (from DataRow dr in dt.Rows
                       select new Pto()
                       {
                           TotalMinute = dr["TotalMinute"] != DBNull.Value ? Convert.ToDouble(dr["TotalMinute"]) : 0,
                       }).ToList();
            return Emplist;
        }
        public List<EmployeePTOHourLog> GetAllEmployeeTimeClockByPaytype(DateTime FromDate, DateTime EndDate)
        {
            DataTable dt = _EmployeeDataAccess.GetAllEmployeeTimeClockByPaytype(FromDate, EndDate);
            List<EmployeePTOHourLog> Emplist = new List<EmployeePTOHourLog>();
            Emplist = (from DataRow dr in dt.Rows
                       select new EmployeePTOHourLog()
                       {
                           TotalWorkingSeconds = dr["TotalWorkingSeconds"] != DBNull.Value ? Convert.ToDouble(dr["TotalWorkingSeconds"]) : 0,
                           UserId = (Guid)dr["UserId"],
                       }).ToList();
            return Emplist;
        }
        public List<EmployeePTOHourLog> GetAllEmployeeTimeClockBySalaryPaytype(DateTime FromDate, DateTime EndDate)
        {
            DataTable dt = _EmployeeDataAccess.GetAllEmployeeTimeClockBySalaryPaytype(FromDate, EndDate);
            List<EmployeePTOHourLog> Emplist = new List<EmployeePTOHourLog>();
            Emplist = (from DataRow dr in dt.Rows
                       select new EmployeePTOHourLog()
                       {
                           TotalWorkingSeconds = dr["TotalWorkingSeconds"] != DBNull.Value ? Convert.ToDouble(dr["TotalWorkingSeconds"]) : 0,
                           UserId = (Guid)dr["UserId"],
                       }).ToList();
            return Emplist;
        }
        public EmployeePtoAccrualRate GetEmployeePtoAccrualRate(int Maxvalue, string Paytype)
        {
            string query = string.Format(" {0} BETWEEN MinimumDay AND MaximumDay and PayType = '{1}'", Maxvalue, Paytype);
            return _EmployeePtoAccrualRateDataAccess.GetByQuery(query).FirstOrDefault();
        }
        #endregion
    }
}
