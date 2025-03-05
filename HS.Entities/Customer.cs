using HS.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace HS.Entities
{

    public partial class Customer
    {
        public string EstimatorId { get; set; }
        public CustomerExtended CustomerExtended { set; get; }
        public RecurringBillingModel RecurringBillingModel { set; get; }
        public string FinanceRepValue { get; set; }
        public int AgemniId { get; set; }
        public string AppoinmentSetByValue { get; set; }
        public string ResignedByValue { get; set; }
        public string DisplayName { set; get; }
        public string CreatedByVal { set; get; }
        public string SelesLocationVal { get; set; }
        public string FullName { set; get; }
        public Guid RouteId { get; set; }
        // public string financedterm { get; set; }
        //public string financedamount { get; set; }
        public string CustomerAccountTypeVal { set; get; }
        public string AccessGivenToVal { get; set; }
        public List<CustomerSystemNo> systemNo { get; set; }
        public int CustomerSystemId { get; set; }
        public Guid CustomerSystemCustomerId { get; set; }
        public Guid CustomerSystemCompanyId { get; set; }
        public double CustomerTotalRevenue { get; set; }
        public CustomerNote CustomerNoteNew { get; set; }
        //for pagination
        public int TotalCount { get; set; }
        public string customerName { get; set; }
        public string[] SalesPersonListLeadEmailArray { get; set; }
        public string InstallerName { get; set; }
        public string SellerName { get; set; }
        public string QualityAssurance1 { get; set; }
        public string QualityAssurance2 { get; set; }
        public string MarketVal { get; set; }
        public bool IsMonitronics { get; set; }
        public bool IsCentral { get; set; }
        public bool IsLead { get; set; }
        // public CustomInitialLeadPackageModel CustomInitialLeadPackageModelItems { get; set; }
        public string BillMethod { get; set; }
        public List<QaAnswer> QAList { get; set; }
        public List<QaAnswer> QAList1 { get; set; }
        public string LeadName { get; set; }
        public string StatusVal { get; set; }
        //for IsQA
        public bool IsQA1Done { get; set; }
        public bool IsQA2Done { get; set; }

        //for QATech GlobalSettings
        public bool QA1GlobalSettings { get; set; }
        public bool TechCallGlobalSettings { get; set; }
        public bool QA2GlobalSettings { get; set; }
        public string InstallerId { get; set; }
        public List<CustomerView> ListCustomerView { get; set; }
        public string NameFile { get; set; }
        public string TechnicianName { get; set; }
        public string PersonSales { get; set; }
        public string EMPNUM { get; set; }
        public List<Invoice> Invoice { get; set; }
        public CustomerCancel CustomerCancel { get; set; }
        public List<EmergencyContact> ListEmergency { get; set; }
        public string PayAccountName { get; set; }
        public string PayCardNumber { get; set; }
        public string PayCardExpireDate { get; set; }
        public string PayBAccountType { get; set; }
        public string PayAccountNo { get; set; }
        public string PayRoutingNo { get; set; }
        public CustomerSystemInfo CustomerSystemInfo { get; set; }
        public CustomerSpouse CustomerSpouse { get; set; }
        public LeadCorrespondence LeadCorrespondence { get; set; }
        public int TotalActiveCustomer { get; set; }
        public int TotalRMRCustomer { get; set; }
        public CustomerTabCounts CustomerTabCounts { set; get; }
        public double UnpaidInvoiceTotal { get; set; }
        public List<ReferingCustomer> ReferingCustomerList { set; get; }
        public Customer ChildCustomer { get; set; }
        public List<ChildCustomer> ChildCustomerList { set; get; }
        public List<CustomerAppointmentEquipment> EquipmentDetailList { set; get; }
        public List<CustomerAppointmentEquipment> TicketServiceDetailList { set; get; }
        public List<InvoiceDetail> ServiceDetailList { set; get; }
        public CustomerMigration CustomerMigration { set; get; }
        public double CreditBalance { set; get; }

        public int EqpmentCount { get; set; }
        public int MonthlyBatch { get; set; }
        public string WeeklyBatch { get; set; }
        public int EmgContactCount { get; set; }
        //TicketAdd
        public Guid[] AssignedTo { get; set; }
        public DateTime CompletionDate { get; set; }
        public DateTime CustomerSinceDate { get; set; }
        public Ticket Ticket { get; set; }
        //CustomerBranch
        public CompanyBranch CompanyBranch { set; get; }
        public string LeadSourceVal { set; get; }
        public string CustomerStatusVal { set; get; }
        public string LeadSourceParentVal { set; get; }
        public string InstallType { set; get; }
        public string BranchName { get; set; }
        public string LeadSiteType { get; set; }
        public LeadDetailTabCountModel LeadDetailTabCountModel { get; set; }
        public string RefCustomer { get; set; }
        public string chCustomer { get; set; }
        public DateTime ConvertionDate { get; set; }
        public string ConvertionType { get; set; }
        public double TotalSales { get; set; }
        public double TotalRMR { get; set; }
        public double TotalTax { get; set; }
        public double SalesAfterTax { get; set; }
        public double TotalPaid { get; set; }
        public double TotalUnpaid { get; set; }
        public int AutomaticCustomerCount { get; set; }
        public string InvoiceBillingType { get; set; }
        public double CustomerRMR { get; set; }
        public double CustomerBillAmount { get; set; }
        public int ACHSubs { get; set; }
        public double ACHRMR { get; set; }
        public double ACHAmount { get; set; }
        public int CCSubs { get; set; }
        public double CCRMR { get; set; }
        public double CCAmount { get; set; }
        public int InvoiceSubs { get; set; }
        public double InvoiceRMR { get; set; }
        public double InvoiceAmount { get; set; }
        public CustomerCreditCheck CreditCheck { get; set; }

        public PackageCustomer PackageCustomer { get; set; }
        public string AccountType { get; set; }
        public string CreatedName { get; set; }
        public string MethodPayment { get; set; }
        public string TransferCustomerName { get; set; }
        public string DuplicateCustomerName { set; get; }
        public int? DuplicateCustomerId { set; get; }
        public DateTime CancellationDate { set; get; }
        public string SoldBy2Text { set; get; }
        public string SoldBy3Text { set; get; }
        // public Guid SalesPerson4 { set; get; }
        //public string FinanceCompany { set; get; }
        public string TaxExemptionVal { get; set; }
        public string AppointmentSetVal { get; set; }
        public string Name { get; set; }
        public string CreditGrade { get; set; }
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLogo { get; set; }
        public string MovedCustomerName { get; set; }
        public int MovedCustomerId { get; set; }

        public string TicketStatus { get; set; }
        public string CSAgreement { get; set; }
        public int MovedCustomerIdFrom { get; set; }
        public List<string> CustomerAccountTypeList { get; set; }
        public string CustomerType { get; set; }
        private string _StrFirstBilling { set; get; }
        public string StrFirstBilling
        {
            get { return _StrFirstBilling; }
            set
            {
                _StrFirstBilling = value;
                this.FirstBilling = value.ToDateTime();
            }
        }
        public string TransferredCustomerName { get; set; }
        public int TransferredCustomerId { get; set; }
        public int ParentId { get; set; }
        //public DateTime ContractStartDate { get; set; }
        //public string RemainingContractTerm { get; set; }
        public List<CustomerDetailTimestamp> CustomerDetailTimestampList { get; set; }

        public string Password { get; set; }
        public string CreatedDateText { get; set; }
        public OrderSummeryDataModel OrderSummeryDataModel { get; set; }

        public string LeadSourceParent { get; set; }
        public int CancellationId { get; set; }
        public string RouteName { get; set; }
        public int MediaCout { get; set; }
        public int NoteCount { get; set; }

        public string PaymentMethodVal { get; set; }

        public string PhoneNumberVal { get; set; }

        public RecurringBillingSchedule RMR { get; set; }
        public string BusinessAccountTypeVal { get; set; }
        public string CancellationReason { get; set; }
        public string BillAdd1 { get; set; }
        public string BillCity { get; set; }
        public string BillState { get; set; }
        public string BillZip { get; set; }
        public string BillEmailAddress { get; set; }
        public DateTime RMRStartDate { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime RMRLastBillDate { get; set; }
        public DateTime RMRNextBillDate { get; set; }
        public string RemainingContractTerm { get; set; }
        public string RMRProductName { get; set; }
        public string CardExpireDate { get; set; }
        public string BankAccountName { get; set; }
        public string CardAccountName { get; set; }
        public string AutoBank { get; set; }
        public string AutoCC { get; set; }
        public string BillingMethodType { get; set; }
        public string RoutingNo { get; set; }
        public string RMRBillDay { get; set; }
        public string RMRBillCycle { get; set; }
        public string SalesPerson { get; set; }
        public string PanelType { get; set; }
        public double RMRAmount { get; set; }
        public DateTime RMRCycleStartDate { get; set; }
    }

    public partial class LeadOrCustomer
    {
        public bool IsLead { get; set; }
    }
    public class CustomerInfoWithCompany
    {
        public int CusId { get; set; }
        public string customerFirstName { get; set; }
        public string customerLastName { get; set; }
        public string customerAddress { get; set; }
        public string customerStreet { get; set; }
        public string customerCity { get; set; }
        public string customerState { get; set; }
        public string customerZipCode { get; set; }
        public string customerPrimaryPhone { get; set; }
        public string customerSecondaryPhone { get; set; }
        public string customerEmail { get; set; }
        public string companyName { get; set; }
        public string companyAddress { get; set; }
        public string companyStreet { get; set; }
        public string companyCity { get; set; }
        public string companyState { get; set; }
        public string companyZipCode { get; set; }
        public string companyPhone { get; set; }
        public string companyFax { get; set; }
        public string companyEmail { get; set; }
        public string companyLogo { get; set; }
        public string companyWebsite { get; set; }
    }
    public class CustomerCreditScoreReport
    {
        public string ContentBody { get; set; }
    }
    public class ReferingCustomer
    {
        public int Id { set; get; }
        public string AccountType { set; get; }
        public string CustomerName { set; get; }
        public string BusinessName { set; get; }
    }
    public class CustomerListAgingWithCount
    {
        public List<CustomerListAging> CustomerListAgingList { get; set; }
        public TotalCustomerAgingCount TotalCustomerAgingCount { get; set; }

        public double TotalCurrentValue { get; set; }

        public double TotalOneThirtyValue { get; set; }

        public double TotalThirtyOneSixtyValue { get; set; }

        public double TotalSixtyOneNinetyValue { get; set; }

        public double TotalNinetyPlusValue { get; set; }

        public double TotalTotalValue { get; set; }


    }
    public class TotalCustomerAgingCount
    {
        public int TotalCount { get; set; }
    }
    public class CustomerListAging
    {
        public int CustomerIntId { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string SalesPerson { get; set; }
        public int SalesPersonId { get; set; }
        public double CurrentValue { get; set; }
        public double OneThirtyValue { get; set; }
        public double ThirtyOneSixtyValue { get; set; }
        public double SixtyOneNinetyValue { get; set; }
        public double NinetyPlusValue { get; set; }
        public double TotalValue { get; set; }
    }
    public class CustomerCreditScore
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SSN { get; set; }
        public string ZIP { get; set; }
        public string ADDRESS { get; set; }
        public string STATE { get; set; }
        public string CITY { get; set; }
        public string ACCOUNT { get; set; }
        public string PASSWD { get; set; }
        public string PASS { get; set; }
        public DateTime DOB { get; set; }
        public string PROCESS { get; set; }
        public string BUREAU { get; set; }
        public string PRODUCT { get; set; }
        public Guid CustomerId { get; set; }
        public string SelectCode { get; set; }
        public bool IsSoftCheck { get; set; }
        public int? ContactId { get; set; }
    }
    public class ChildCustomer
    {
        public int Id { set; get; }
        public string AccountType { set; get; }
        public string CustomerName { set; get; }
        public string BusinessName { set; get; }

    }
    public class CustomerIdList
    {
        public int customerId { get; set; }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string BusinessName { get; set; }
        public string Status { get; set; }
        public string EMPNUM { get; set; }
        public string Type { get; set; }
        public DateTime JoinDate { get; set; }
        public string Address { get; set; }
        public string PrimaryPhone { get; set; }
        public string LeadSource { get; set; }
        public string FilterText { get; set; }
    }

    public class InvoiceIdList
    {
        public int CreatedDate { get; set; }
        public int Id { get; set; }
        public string Discription { get; set; }
        public string DueDate { get; set; }
        public string Total { get; set; }
        public string Balance { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
    }
    public class GETCustomerDataResponse
    {
        public string Hours { get; set; }
        public string Brand { get; set; }
        public bool ExistingCustomer { get; set; }
        public string CustomerNumber { get; set; }
        public string AddressStreet { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string AddressZip { get; set; }
        public string MainPhone { get; set; }
        public string SecondaryPhone { get; set; }

    }
    public class GETCustomerLookUpResponse
    {
        public bool ExistingAppointment { get; set; }
        public string ServiceArea { get; set; }
        public string AppointmentDay { get; set; }
        public string AppointmentMonth { get; set; }
        public int AppointmentDate { get; set; }
        public string AppointmentTimeframe { get; set; }
        public string Token { get; set; }
    }
    public class GETCustomerSchedulerResponse
    {
        public bool Success { get; set; }
    }
    public class GETCustomerDateValidatorResponse
    {
        public bool ValidDate { get; set; }
    }
    public class CustomerTabCounts
    {
        public int BookingCount { get; set; }
        public int InvoiceCount { set; get; }
        public int EstimateCount { set; get; }
        public int OrderCount { set; get; }
        public int FundingCount { set; get; }
        public int WorkOrderCount { set; get; }
        public int ServiceOrderCount { set; get; }
        public int NotesCount { set; get; }
        public int CorrespondenceCount { set; get; }
        public int FilesCount { set; get; }
        public int DocumentFileCount { set; get; }
        public int ScheduleCount { set; get; }
        public int TicketsCount { set; get; }
        public string CustomerCredit { set; get; }
        public int ActivityCustomer { get; set; }
        public int OpportunityCustomer { get; set; }
        public int ContactCustomer { get; set; }
        public bool IsVip { get; set; }
        public int LogCount { get; set; }
        public int RecurringBillingCount { get; set; }
        public int EstimatorCount { set; get; }

        public int TotalFundingCount { set; get; }

        public int? ActiveFileStatusCount { set; get; }
        public int? InActiveFileStatusCount { set; get; }


    }

    public partial class EstimateStatus
    {
        public double EstimateAmount { set; get; }
        public double DueAmount { set; get; }
        public double PaidAmount { set; get; }
    }
    public partial class UploadedFileName
    {
        public string CustomerFileName { set; get; }
        public string CustomerFileDescription { set; get; }
    }
    public class CustomerSubscriptionStatus
    {
        public string status { set; get; }
    }
    public class CustomerPaymentMethod
    {
        public string Paymentmethod { get; set; }
    }
    public class CustomerLiteFilter
    {
        public string Industry { get; set; }
        public DateTime UpdatedFrom { get; set; }
        public DateTime UpdatedTo { get; set; }
        public string from { get; set; }
        public string IsAssigned { get; set; }
        public bool HideNotInterestedLead { get; set; }
        public string User { get; set; }
        public string Status { get; set; }
        public string CustomerAccountTypeText { get; set; }
        public bool ShowNotesInGrid { get; set; }
        public bool ShowNotesInGridCSM { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsDownload { get; set; }
        public bool MyLeads { get; set; }
        public int HourstoCount { get; set; }
        public string Installer { get; set; }
        public string CreatedBy { get; set; }
        public string searchid { get; set; }
        public int PageSize { get; set; }
        public int PageNo { get; set; }
        public string SearchText { get; set; }
        public string SQLNameCondition { get; set; }
        public bool isLead { get; set; }
        public DateTime FirstDate { get; set; }
        public DateTime LastDate { get; set; }
        public string IdText { get; set; }
        public string FirstNameText { get; set; }
        public string LastNameText { get; set; }
        public string BusinessNameText { get; set; }
        public string EmailText { get; set; }
        public string StreetText { get; set; }
        public string LeadSourceText { get; set; }
        public string SalesPersonText { get; set; }
        public string JoinDateText { get; set; }
        public string StatusText { get; set; }
        public string ActiveStatusText { get; set; }
        public string DisplayNameText { get; set; }
        public string FollowupDateText { get; set; }
        public string CustomerNoText { get; set; }
        public string LeadStatusText { get; set; }
        public string CustomerTypeText { get; set; }
        public string PrimaryPhoneText { get; set; }
        public string SecondaryPhoneText { get; set; }
        public string CellNoText { get; set; }
        public string AccountNoText { get; set; }
        public string DbaText { get; set; }
        public string BranchidText { get; set; }
        public string SalesLocationText { get; set; }
        public string PlatformIdText { get; set; }


        public string EmployeeRole { get; set; }
        public string UserRole { get; set; }
        public string EmployeeId { get; set; }
        public Guid CompanyId { get; set; }
        public List<Partner> Partners { get; set; }
        public bool isPermit { get; set; }
        public string SettingOrderBy { get; set; }

        public bool OrderPermission { get; set; }

        public string MonitoringCompany { set; get; }
        public string SortBy { set; get; }

        public string SortOrder { set; get; }

        public bool TotalRMR { get; set; }
        public bool TotalOpenInvoice { get; set; }

        public bool TotalDueInvoice { get; set; }

        public string Others { get; set; }

        public string Branch { get; set; }

        public Nullable<bool> IsActive { set; get; }

        public string SalesDate { set; get; }

        public string InstallationDate { set; get; }

        public string PaymentMethod { set; get; }

        public string Source { get; set; }

        public Guid SoldById { get; set; }
        public string BusinessAccountTypeText { get; set; }
        public string RouteName { get; set; }
        public int GeeseCount { get; set; }
    }
    public class CustomerFilter
    {

        public Guid CompanyId { get; set; }
        public string Source { get; set; }

        #region No longer in use in customer search 
        public string User { get; set; }
        public DateTime FirstDate { get; set; }
        public DateTime LastDate { get; set; }
        #endregion

        public int PageSize { get; set; }
        public int PageNo { get; set; }
        public string SearchText { get; set; }
        public string EmployeeRole { get; set; }
        public string PaymentMethod { set; get; }
        public Nullable<bool> IsActive { set; get; }
        public string EmployeeId { get; set; }
        public string order { get; set; }
        public string SettingOrderBy { get; set; }
        public string TechnicianId { set; get; }
        public string SalesPersonId { set; get; }
        public string InstallationDate { set; get; }
        public string SalesDate { set; get; }
        public string FollowUpDate { set; get; }
        public string MonitoringCompany { set; get; }
        public string UserRole { get; set; }
        public string SortBy { set; get; }
        public string SortOrder { set; get; }
        public bool TotalRMR { get; set; }
        public bool TotalOpenInvoice { get; set; }
        public bool TotalDueInvoice { get; set; }
        public string Status { get; set; }
        public string Branch { get; set; }
        public string Package { get; set; }
        public DateTime StrStartDate { get; set; }
        public DateTime StrEndDate { get; set; }

        public string Others { get; set; }
        public Guid SoldById { get; set; }
        public bool LeadEstimate { get; set; }
        public bool leadthismonth { get; set; }
        public bool leadlastmonth { get; set; }
        public bool LeadBooking { get; set; }
        public List<Partner> Partners { set; get; }
        public bool isPermit { get; set; }
        public bool isLead { set; get; }

        /*start new filter customer object*/
        public string displayname { get; set; }
        public string customerno { get; set; }
        public string customerintid { get; set; }
        public string title { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string childof { get; set; }
        public string passcode { get; set; }
        public string customertype { get; set; }
        public string businessname { get; set; }
        public string dba { get; set; }
        public string street { get; set; }
        public string zipcode { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string county { get; set; }
        public string country { get; set; }
        public string homeowner { get; set; }
        public string leadsource { get; set; }
        public string leadsourcetype { get; set; }
        public string referringcustomer { get; set; }
        public string emailaddress { get; set; }
        public string primaryphone { get; set; }
        public string cellno { get; set; }
        public string secondaryphone { get; set; }
        public string carrier { get; set; }
        public string phonetype { get; set; }
        public string besttimetocall { get; set; }
        public string preferredcontactmethod { get; set; }
        public string dateofbirth { get; set; }
        public string ssn { get; set; }
        public string callingtime { get; set; }
        public string address { get; set; }
        public string address2 { get; set; }
        public string streetprevious { get; set; }
        public string cityprevious { get; set; }
        public string stateprevious { get; set; }
        public string zipcodeprevious { get; set; }
        public string countryprevious { get; set; }
        public string accountno { get; set; }
        public string creditscore { get; set; }
        public string creditscorevalue { get; set; }
        public string contractteam { get; set; }
        public string fundingcompany { get; set; }
        public string joindate { get; set; }
        public string note { get; set; }
        public string salesdatecus { get; set; }
        public string installdate { get; set; }
        public string cutindate { get; set; }
        public string installer { get; set; }
        public string fundingdate { get; set; }
        public string middlename { get; set; }
        public string reminderdate { get; set; }
        public string qa1 { get; set; }
        public string qa1date { get; set; }
        public string qa2 { get; set; }
        public string qa2date { get; set; }
        public string billamount { get; set; }
        public string paymentmethodcus { get; set; }
        public string billcycle { get; set; }
        public string billday { get; set; }
        public string billnotes { get; set; }
        public string billtax { get; set; }
        public string billoutstanding { get; set; }
        public string servicedate { get; set; }
        public string area { get; set; }
        public string streettype { get; set; }
        public string apartment { get; set; }
        public string secondcustomerno { get; set; }
        public string additionalcustomerno { get; set; }
        public string activationfee { get; set; }
        public string firstbilling { get; set; }
        public string isactivestatus { get; set; }
        public string mmr { get; set; }
        public string crossstreet { get; set; }
        public string businessaccounttype { get; set; }
        public string esistingpanel { get; set; }
        public string estclosedate { get; set; }
        public string projectwalkdate { get; set; }
        public string annualrevenue { get; set; }
        public string website { get; set; }
        public string market { get; set; }
        public string passengers { get; set; }
        public string budget { get; set; }
        public string customeraccounttype { get; set; }
        public string csprovider { get; set; }
        public string ownership { get; set; }
        public string purchaseprice { get; set; }
        public string contractvalue { get; set; }
        public string accessgivento { get; set; }
        public string soldby { get; set; }
        public string saleslocation { get; set; }
        public string leadstatus { get; set; }
        public string donotcall { get; set; }
        public string customerstatus { get; set; }
        public string duplicatecustomer { get; set; }
        public string movingdate { get; set; }
        public string contractedpreviously { get; set; }
        public string installedstatus { get; set; }
        public string acquiredform { get; set; }
        public string followupdatecus { get; set; }
        public string buyoutamountbyads { get; set; }
        public string buyoutamountbysalesrep { get; set; }
        public string financedterm { get; set; }
        public string financedamount { get; set; }
        public string levels { get; set; }
        public string soldamount { get; set; }
        public string taxexemption { get; set; }
        public string appointmentset { get; set; }
        public string customerfunded { get; set; }
        public string maintenance { get; set; }
        public string isalarmcom { get; set; }
        public string isagreement { get; set; }
        public string isfireaccount { get; set; }
        public string branchid { get; set; }
        public string PlatformId { get; set; }
        public string MapscoNo { get; set; }
        /*end new filter customer object*/
    }
    public class EstimateFilter
    {
        public string SearchText { get; set; }
        public DateTime StrStartDate { get; set; }
        public DateTime StrEndDate { get; set; }
        public string estimateStatus { get; set; }
    }
    public class EstimatorFilter
    {
        public Guid? UserId { get; set; }
        public string SearchText { get; set; }
        public DateTime StrStartDate { get; set; }
        public DateTime StrEndDate { get; set; }
        public string estimateStatus { get; set; }
    }
    public class TotalCustomerCount
    {
        public int Counter { get; set; }
        public int TotalAmount { get; set; }
        public int Closing { get; set; }

    }
    public class JobFileCustomModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileLocation { get; set; }
        public string Description { get; set; }
        public int BookingId { get; set; }
    }
    public class AppTicketItemModel
    {
        public int Id { get; set; }
        public string TicketId { get; set; }
        public string SoldBy { get; set; }
        public string TicketType { get; set; }
        public string ServiceText { get; set; }
        public string ServiceValue { get; set; }
        public double ServiceUnitPrice { get; set; }
        public string PackageText { get; set; }
        public double ServicePackageValue { get; set; }
        public double PackageBaseRate { get; set; }
        public double packageAdditionalRate { get; set; }
        public string AddOnsText { get; set; }
        public double AddOnsValue { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }
        public string BookingId { get; set; }
        public double Discount { get; set; }
        public double Total { get; set; }
    }
    public class CustomerAppointmentEquipmentApi
    {

        public int id { set; get; }
        public Guid equipmentGuid { set; get; }
        public int onHand { get; set; }
        public int quantity { get; set; }
        public double unitPrice { get; set; }
        public double unitCost { get; set; }
        public double point { get; set; }
        public string name { get; set; }
        public int installed { get; set; }
        public double totalPrice { get; set; }
        public int EquipmentClassId { get; set; }
        public string barcode { get; set; }


    }
    public class SimplifiedCustomerAppointmentEquipmentApi
    {
        public int id { get; set; }
        public Guid serviceGuid { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public double unitCost { get; set; }
        public double unitPrice { get; set; }
        public double totalPrice { get; set; }
    }
    public class CustomerAppointmentEquipmentApiResponse
    {
        public int id { get; set; }
        public Guid equipmentGuid { get; set; }
        public string name { get; set; }
        public string barcode { get; set; }
        public double point { get; set; }
        public int onHand { get; set; }
        public int installed { get; set; }
        public double unitCost { get; set; }
        public double unitPrice { get; set; }
        public int quantity { get; set; }
        public double totalPrice { get; set; }
    }

    public class AttachBookingItemsModel
    {
        public List<AppTicketItemModel> ListBookingDetailsCustomModel { get; set; }
        public List<CustomerAppointmentEquipmentApi> ListCustomerAppointmentEquipment { get; set; }
        public List<JobFileCustomModel> ListRugImages { get; set; }
        public string DiscountType { get; set; }
        public double DiscountAmount { get; set; }
        public double TotalAmount { get; set; }
        public double MinValue { get; set; }
        public double OutofServiceFee { get; set; }
        public double PortableAmount { get; set; }
        public double AdjustmentValue { get; set; }
        public bool IsCallAhead { get; set; }
        public bool IsBillToCustomer { get; set; }
        public bool IsPaid { get; set; }
        public double TaxPercentage { get; set; }
        public double TotalTax { get; set; }
        public string BookingId { get; set; }
        public string InvoiceId { get; set; }
        public double MissingAmount { get; set; }
        public string MissingNotes { get; set; }
        public int? ReworkTicket { get; set; }
    }
    public class CustomerCount
    {
        public int TotalCustomer { get; set; }
    }
    public class TicketReplyNoteCustomModel
    {
        public int Id { get; set; }
        public Guid TicketId { get; set; }
        public Guid UserId { get; set; }
        public DateTime RepliedDate { get; set; }
        public string Message { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsOverview { get; set; }
        public string UserName { get; set; }
        public string ProfilePicture { get; set; }
    }
    public class EmployeeCustomModel
    {
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public bool Selected { get; set; }
    }

    public class TicketCustomModel
    {
        public int Id { get; set; }
        public Guid TicketId { get; set; }
        public Guid CompanyId { get; set; }
        public Guid CustomerId { get; set; }
        public string TicketType { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public string Status { get; set; }
        public DateTime CompletedDate { get; set; }
        public bool HasInv { get; set; }
        public bool HasNote { get; set; }
        public bool IsClosed { get; set; }
        public bool HasLog { get; set; }
        public string CustomerName { get; set; }
        public string AssignedPerson { get; set; }
        public string CreatedPerson { get; set; }
        public string AssignedStatus { get; set; }
        public string ProfileImage { get; set; }
        public string Phone { get; set; }
        public string EmailAddress { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public bool IsDispatch { get; set; }
        public bool HasBooking { get; set; }
        public bool HasExtraItem { get; set; }
        public double InvoiceBalanceDue { get; set; }
        public bool HasFile { get; set; }
        public string AppointmentStartTime { get; set; }
        public string AppointmentEndTime { get; set; }
        public int CustomerIntId { get; set; }
    }

    public class JobsCustomModel
    {
        public List<TicketCustomModel> TicketList { get; set; }
        public List<InvoiceDetail> InvDetailList { get; set; }
        public List<TicketUser> UserList { get; set; }
        public List<CustomerAppointment> ScheduleList { get; set; }
        public List<AdditionalMembersAppointment> AdditionalScheduleList { get; set; }
    }

    public class CustomerListWithCountModel
    {
        public List<Customer> CustomerList { get; set; }
        public TotalCustomerCount TotalCustomerCount { get; set; }
        public CustomerCount CustomerCount { get; set; }
        public string UserTag { get; set; }

        public string searchText { get; set; }

        public string StatusIDList { get; set; }
        public LeadTabCount LeadTabCount { get; set; }
        public LeadTabEstimateCount LeadTabEstimateCount { get; set; }
        public LeadTabBookingCount LeadTabBookingCount { get; set; }
        public LeadTabThisMonthCount LeadTabThisMonthCount { get; set; }
        public LeadTabLastMonthCount LeadTabLastMonthCount { get; set; }
        public LeadTabEstimateAmount LeadTabEstimateAmount { get; set; }
        public LeadTabBookingAmount LeadTabBookingAmount { get; set; }
        public List<CustomerIdList> TotalFilteredCusId { get; set; }


        public double TotalAmountByPage { get; set; }
    }
    public class LeadHeaderStatusBar
    {
        public int NewLeadCount { get; set; }
        public int ContactedCount { get; set; }
        public int QuotedCount { get; set; }
        public int PresentedCount { get; set; }
        public int SoldCount { get; set; }
        public int DeadCount { get; set; }
        public int PartialCount { get; set; }
        public int AllLead { get; set; }
        public int SubmittedCount { get; set; }
        //public List<Lookup> Statuslist { get; set; }
    }
    public class CustomerTableWithCount
    {
        public LeadHeaderStatusBar LeadHeaderStatusBar { get; set; }
        public DataTable CustomerList { get; set; }
        public string CustomerListString { get; set; }
        public TotalCustomerCount TotalCustomerCount { get; set; }
        public CustomerCount CustomerCount { get; set; }
        public CustomerHeaderMoneyBar CustomerHeaderMoneyBar { get; set; }


    }
    public partial class EstimateStatusDetail
    {
        public double EstimateAmountDetail { set; get; }
        public double DueAmountDetail { set; get; }
        public double PaidAmountDetail { set; get; }
        public double UnpaidAmount { get; set; }
        public double CustomerCredit { set; get; }
    }
    public class Customerupdateoldandnew
    {
        public string oldtitle { get; set; }
        public string newtitle { get; set; }

        public string oldnote { get; set; }
        public string newnote { get; set; }
        public string oldbusinessname { get; set; }
        public string newbusinessname { get; set; }

        public string oldfirstname { get; set; }
        public string newfirstname { get; set; }

        public string oldlastname { get; set; }
        public string newlastname { get; set; }

        public string oldcustomerno { get; set; }
        public string newcustomerno { get; set; }

        public string oldtype { get; set; }
        public string newtype { get; set; }

        public DateTime? olddateofbirth { get; set; }
        public DateTime? newdateofbirth { get; set; }

        public string oldprimaryphone { get; set; }
        public string newprimaryphone { get; set; }

        public string oldsecondaryphone { get; set; }
        public string newsecondaryphone { get; set; }

        public string oldcellno { get; set; }
        public string newcellno { get; set; }

        public string oldfax { get; set; }
        public string newfax { get; set; }

        public string oldcallingtime { get; set; }
        public string newcallingtime { get; set; }

        public string oldaddress { get; set; }
        public string newaddress { get; set; }

        public string oldaddress2 { get; set; }
        public string newaddress2 { get; set; }

        public string oldstreet { get; set; }
        public string newstreet { get; set; }

        public string oldcity { get; set; }
        public string newcity { get; set; }

        public string oldstate { get; set; }
        public string newstate { get; set; }

        public string oldzipcode { get; set; }
        public string newzipcode { get; set; }

        public string oldcountry { get; set; }
        public string newcountry { get; set; }

        public string oldleadsource { get; set; }
        public string newleadsource { get; set; }

        public DateTime? olddonotcall { get; set; }
        public DateTime? newdonotcall { get; set; }


        public Guid? oldreferingcustomer { get; set; }
        public Guid? newreferingcustomer { get; set; }

        public Guid? oldchildof { get; set; }
        public Guid? newchildof { get; set; }

        public string oldalternateid { get; set; }
        public string newalternateid { get; set; }
        public DateTime? oldestimateclosedate { get; set; }
        public DateTime? newestimateclosedate { get; set; }

        public DateTime? oldinspectiondate { get; set; }
        public DateTime? newinspectiondate { get; set; }

        public DateTime? oldjoindate { get; set; }
        public DateTime? newjoindate { get; set; }
        public string oldcsprovider { get; set; }
        public string newcsprovider { get; set; }

        public string oldbranch { get; set; }
        public string newbranch { get; set; }



        public string oldownership { get; set; }
        public string newownership { get; set; }

        public string oldsoldby { get; set; }
        public string newsoldby { get; set; }

        public string oldsaleslocation { get; set; }
        public string newsaleslocation { get; set; }

        public string oldleadstatus { get; set; }
        public string newleadstatus { get; set; }

        public string oldcustomerstatus { get; set; }
        public string newcustomerstatus { get; set; }

        public Guid? oldsoldby2 { get; set; }
        public Guid? newsoldby2 { get; set; }

        public string oldfinancerep { get; set; }
        public string newfinancerep { get; set; }

        public DateTime? oldmovingdate { get; set; }
        public DateTime? newmovingdate { get; set; }

        public string oldcontactedpreviously { get; set; }
        public string newcontactedpreviously { get; set; }

        public string oldinstalledstatus { get; set; }
        public string newinstalledstatus { get; set; }

        public string oldpaymentmethod { get; set; }
        public string newpaymentmethod { get; set; }

        public bool oldbilltax { get; set; }
        public bool newbilltax { get; set; }

        public bool oldisactive { get; set; }
        public bool newisactive { get; set; }

        public string olddba { get; set; }
        public string newdba { get; set; }

        public string oldbusinessaccounttype { get; set; }
        public string newbusinessaccounttype { get; set; }

        public string oldacquiredfrom { get; set; }
        public string newacquiredfrom { get; set; }

        public DateTime? oldfollowupdate { get; set; }
        public DateTime? newfollowupdate { get; set; }

        public double? oldbuyoutamountbyads { get; set; }
        public double? newbuyoutamountbyads { get; set; }

        public double? oldbuyoutamountbysalesrep { get; set; }
        public double? newbuyoutamountbysalesrep { get; set; }

        public double? oldfinancedterm { get; set; }
        public double? newfinancedterm { get; set; }

        public double? oldfinancedamount { get; set; }
        public double? newfinancedamount { get; set; }

        public double? oldsoldamount { get; set; }
        public double? newsoldamount { get; set; }

        public string oldcsagreement { get; set; }
        public string newcsagreement { get; set; }

        public bool oldisfinanced { get; set; }
        public bool newisfinanced { get; set; }

        public string oldssn { get; set; }
        public string newssn { get; set; }

        public Guid? oldsoldby3 { get; set; }
        public Guid? newsoldby3 { get; set; }

        public string oldmonthlymonitoringfee { get; set; }
        public string newmonthlymonitoringfee { get; set; }


        public string oldpaneltype { get; set; }
        public string newpaneltype { get; set; }


        public string oldleadtype { get; set; }
        public string newleadtype { get; set; }


        public bool oldcellularbackup { get; set; }
        public bool newcellularbackup { get; set; }

        public DateTime? oldsalesdate { get; set; }
        public DateTime? newsalesdate { get; set; }

        public DateTime? oldinstalldate { get; set; }
        public DateTime? newinstalldate { get; set; }

        public DateTime? oldrepsassigndate { get; set; }
        public DateTime? newrepsassigndate { get; set; }

        public DateTime? oldfundingdate { get; set; }
        public DateTime? newfundingdate { get; set; }

        public DateTime? oldqa1date { get; set; }
        public DateTime? newqa1date { get; set; }

        public DateTime? oldqa2date { get; set; }
        public DateTime? newqa2date { get; set; }
        public string newemail { get; set; }

        public string oldemail { get; set; }

        public string newinstaller { get; set; }

        public string oldinstaller { get; set; }

        public string newqa2 { get; set; }

        public string oldqa2 { get; set; }

        public string newqa1 { get; set; }

        public string oldqa1 { get; set; }


        public string newcreditgrade { get; set; }

        public string oldcreditgrade { get; set; }
        public string newcreditscore { get; set; }

        public string oldcreditscore { get; set; }

        public string newcontactterm { get; set; }

        public string oldcontactterm { get; set; }

        public string newfundingcompany { get; set; }

        public string oldfundingcompany { get; set; }

        public bool newcustomerfunded { get; set; }

        public bool oldcustomerfunded { get; set; }

        public bool newmaintanence { get; set; }

        public bool oldmaintanence { get; set; }

        public double? newbillamount { get; set; }

        public double? oldbillamount { get; set; }

        public string newbillcycle { get; set; }

        public string oldbillcycle { get; set; }

        public double? newbilloutstanding { get; set; }

        public double? oldbilloutstanding { get; set; }

        public double? newmonitoringfee { get; set; }

        public double? oldmonitoringfee { get; set; }

        public double? newpurchaseprice { get; set; }

        public double? oldpurchaseprice { get; set; }

        public string newcontactvalue { get; set; }

        public string oldcontactvalue { get; set; }


        public DateTime? oldstartdayfirstbilling { get; set; }
        public DateTime? newstartdayfirstbilling { get; set; }

        public string oldbillday { get; set; }

        public string newbillday { get; set; }

        public string oldpaymentprofile { get; set; }

        public string newpaymentprofile { get; set; }

        public bool oldtax { get; set; }

        public string newtax { get; set; }

        public string oldsoldbyval { get; set; }

        public string newsoldbyval { get; set; }

        public string Newleadsourceval { get; set; }

        public string Oldleadsourceval { get; set; }

        public string Newinstallerval { get; set; }

        public string Oldinstallerval { get; set; }



    }
    public class CustomerAddendumModel
    {
        public string CompanyLogo { get; set; }
        public string KazarLogo { get; set; }
        public string KazarLogoIcon { get; set; }
        public string CompanySignature { get; set; }
        public string CompanySignatureDate { get; set; }
        public Guid CompanyId { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyCity { get; set; }
        public double Tax { set; get; }
        public string CompanyState { get; set; }
        public string CompanyZip { get; set; }
        public string CompanyPhone { get; set; }
        public string WorkToBePerformed { get; set; }
        public DateTime AgreementSignDate { get; set; }
        public DateTime AddendumCreateDate { get; set; }
        public string SalesRepresentative { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BuisnessName { get; set; }
        public string CustomerStreet { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerState { get; set; }
        public string CustomerZip { get; set; }
        public string InstallAddress { get; set; }
        public string CellPhone { get; set; }
        public string SitePhone { get; set; }
        public string EmailAddress { get; set; }

        public int TicketId { get; set; }
        public DateTime ScheduleOn { get; set; }
        public string RecurringAmount { get; set; }
        public string CurrentCurrency { get; set; }

        public List<CustomerAppointmentEquipment> ServiceEqpList { get; set; }
        public List<CustomerAppointmentEquipment> EquipmentList { get; set; }
        public Guid CustomerId { get; set; }
        public Guid TicketGuidId { get; set; }
        public string CustomerAddendumSignature { get; set; }
        public DateTime CustomerAddendumSignatureDate { get; set; }
        public string CustomerAddendumStringSignatureDate { get; set; }
    }
    public class CustomerDetailsTabCount
    {
        public int OpenEstimateCount { set; get; }

        public int CompletedEstimateCount { set; get; }

        public int OpenInvoiceCount { set; get; }

        public int PaidInvoiceCount { set; get; }

        public int RolledOverInvoiceCount { set; get; }

        public int ActiveFilesCount { set; get; }

        public int InActiveFilesCount { set; get; }

        public int FundingCount { set; get; }

        public int ExpenseCount { set; get; }




    }
    public class CustomerHeaderMoneyBar
    {
        public string CustomerCount { set; get; }
        public string TotalRMR { set; get; }
        public string TotalRMRCount { set; get; }
        public string EstimateAmount { get; set; }
        public string DueAmount { set; get; }

        public int OrderCount { get; set; }
        public double OrderValue { get; set; }

    }

    public class AgemnitCustomer
    {
        public string CustomerId { get; set; }
        public string ContactStatusId { get; set; }
        public string BillingAddress { get; set; }
        public string BillingZipCode { get; set; }
        public string Email1 { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }

        public string AlternateFirstName { get; set; }
        public string AlternateLastName { get; set; }
        public string AlternateBirthDate { get; set; }
        public string AlternatePhone1 { get; set; }
        public string AlternatePhone1TypeId { get; set; }
        public string AlternateEmail { get; set; }
    }

    public class AgemnitCustomerList
    {
        public List<AgemnitCustomer> Data { get; set; }
        public string StatusCode { get; set; }
    }

    public class AgemniCustomerList
    {
        public List<AgemniCustomer> Data { get; set; }
        public string StatusCode { get; set; }
    }
    public class AgemniCustomer
    {
        public string CustomerId { get; set; }
        public string VendorId { get; set; }
        public string SalesPersonId { get; set; }
        public string LeadPersonID { get; set; }
        public string PromotionId { get; set; }
        public string ReferralId { get; set; }
        public string SalesLocationId { get; set; }
        public string CreatedById { get; set; }

        public string JobTypeId { get; set; }
        public string Phone1TypeId { get; set; }
        public string BestTimeToCallId { get; set; }
        public string MiscPhone1TypeId { get; set; }

        public string MiscPhone2TypeId { get; set; }
        public string InstallAddressTypeId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BusinessName { get; set; }
        public string Phone1 { get; set; }
        public string Email { get; set; }
        public string InstallAddress { get; set; }
        public string InstallCity { get; set; }
        public string InstallZipCode { get; set; }
        public string InstallState { get; set; }

        public string InstallCounty { get; set; }
        public string IsHomeOwner { get; set; }
        public string MiscPhone1 { get; set; }
        public string MiscPhone2 { get; set; }

        public string BillingContact { get; set; }
        public string BillingFirstName { get; set; }
        public string BillingPhone { get; set; }
        public string BillingLastName { get; set; }
        public string BillingEmail { get; set; }
        public string BillingCounty { get; set; }

        public string MyProperty { get; set; }
        public string BirthDate { get; set; }
        public string CreateDate { get; set; }
        public string LastOpenedDate { get; set; }
        public string CancelDate { get; set; }

        public string DisconnectServiceDate { get; set; }
        public string SoldDate { get; set; }
        public string DoNotCallDate { get; set; }
        public string ChangedDate { get; set; }
        public string CloseDate { get; set; }


    }

    public class AgemniJobTypeList
    {
        public List<AgemniJobType> Data { get; set; }
        public string StatusCode { get; set; }
    }
    public class AgemniJobType
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string IsActive { get; set; }
        public string SortOrder { get; set; }
    }

    public class AgemniPhoneTypeList
    {
        public List<AgemniPhoneType> Data { get; set; }
        public string StatusCode { get; set; }
    }
    public class AgemniPhoneType
    {
        public string phonetypeID { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

    }
    public class AgemniBestTimeToCallList
    {
        public List<AgemniBestTimeToCall> Data { get; set; }
        public string StatusCode { get; set; }
    }
    public class AgemniBestTimeToCall
    {
        public string Name { get; set; }
        public string SortOrder { get; set; }
        public string IsActive { get; set; }
        public string Id { get; set; }
        public string Removed { get; set; }

    }
    public class AgemniLeadSource
    {
        public string AdvertisementID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }


    }
    public class AgemniLeadSourceList
    {
        public List<AgemniLeadSource> Data { get; set; }

        public string StatusCode { get; set; }
    }

    public class AgemniSalesLocation
    {
        public string LocationID { get; set; }
        public string Name { get; set; }



    }
    public class AgemniSalesLocationList
    {
        public List<AgemniSalesLocation> Data { get; set; }

        public string StatusCode { get; set; }
    }
    public class AgemniEmployeeList
    {
        public List<AgemniEmployee> Data { get; set; }
        public string StatusCode { get; set; }
    }
    public class AgemniEmployee
    {
        public string UserID { get; set; }
        public string login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }

        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone1 { get; set; }


        public string Phone2 { get; set; }
        public string Email { get; set; }
        public string Active { get; set; }
        public string ssnumber { get; set; }
        public string password { get; set; }

        public string hireDate { get; set; }
        public string HourlyRate { get; set; }
        public string BirthDate { get; set; }
        public string UpdatedDate { get; set; }
        public string OriginDate { get; set; }
        public string TerminationDate { get; set; }

    }
    public class CountData
    {
        public int TotalCount { get; set; }
    }
    public class TotalDataCount
    {
        public CountData Data { get; set; }
        public string StatusCode { get; set; }
    }
    public class AgemniNoteList
    {
        public List<AgemniNote> Data { get; set; }
        public string StatusCode { get; set; }
    }

    public class AgemniNote
    {
        public string NoteID { get; set; }
        public string CustomerID { get; set; }
        public string WorkOrderId { get; set; }
        public string InvoiceID { get; set; }
        public string NoteText { get; set; }
        public string notetype { get; set; }
        public string ParentNoteId { get; set; }
        public string NoteDate { get; set; }

        public string UserID { get; set; }
        public string AppointmentID { get; set; }

        public string ShowOnContract { get; set; }
        public string ProgrammingQuoteID { get; set; }

        public string PanelID { get; set; }
        public string NoteHtml { get; set; }


        public string UpdatedDate { get; set; }
        public string Id { get; set; }

        public string Removed { get; set; }

    }

    public class AgemniWorkOrderList
    {
        public List<AgemniWorkOrder> Data { get; set; }
        public string StatusCode { get; set; }
    }
    public class AgemniWorkOrder
    {
        public string Id { get; set; }
        public string InvoiceId { get; set; }
        public string CustomerId { get; set; }
        public string WorkOrderType { get; set; }
        public string TypeId { get; set; }
        public string StatusId { get; set; }
        public string LocationId { get; set; }
        public string InstallerId { get; set; }
        public string InstallDate { get; set; }
        public string PanelType { get; set; }
        public string TakeOverType { get; set; }

        public string CommType { get; set; }
        public string ReceiverPhone { get; set; }
        public string MonitoringAccount { get; set; }
        public string AlarmAccount { get; set; }
        public string CellModemSerial { get; set; }
        public string AbortCode { get; set; }
        public string MasterPin { get; set; }
        public string FirstName1 { get; set; }
        public string LastName1 { get; set; }
        public string Phone1 { get; set; }
        public string Phone1Type { get; set; }
        public string ECV1 { get; set; }

        public string FirstName2 { get; set; }
        public string LastName2 { get; set; }
        public string Phone2 { get; set; }
        public string Phone2Type { get; set; }
        public string ECV2 { get; set; }


        public string FirstName3 { get; set; }
        public string LastName3 { get; set; }
        public string Phone3 { get; set; }
        public string Phone3Type { get; set; }
        public string ECV3 { get; set; }
        public string TestDateTime { get; set; }
        public string TestDuration { get; set; }

        public string CustomFields { get; set; }
        public string AlarmDesiredLogin { get; set; }
        public string AlarmDesiredPassword { get; set; }
        public string CellPlanId { get; set; }
        public string PackageId { get; set; }
        public string SummaryOfService { get; set; }
        public string ProvisioningAccount { get; set; }

        public string WarrantyEffectiveDate { get; set; }
        public string WarrantyField { get; set; }
        public string WorkOrderPriorityId { get; set; }
        public string WorkOrderStateId { get; set; }
        public string WOName { get; set; }
        public string TotalBillableUnits { get; set; }
        public string AgemniId { get; set; }
        public string WarrantyEffectiveDateTo { get; set; }

        public string PaymentTerm { get; set; }
        public string WODueDate { get; set; }

    }
    public class RWSTList
    {
        public List<RWSTList> RWSTDataList { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string LeadSource { get; set; }
        public string Status { get; set; }
        public string RWST1 { get; set; }
        public string BatchNumber { get; set; }
        public string FinanceRep { get; set; }
        public string RWST2 { get; set; }
        public string RWST3 { get; set; }
        public string RWST4 { get; set; }
        public string RWST5 { get; set; }
        public string RWST6 { get; set; }
        public string RWST7 { get; set; }
        public string RWST8 { get; set; }
        public string RWST9 { get; set; }
        public string RWST10 { get; set; }
        public string RWST11 { get; set; }
        public string RWST12 { get; set; }
        public string RWST13 { get; set; }
        public string RWST14 { get; set; }
        public string RWST15 { get; set; }
        public CustomerCount CustomerCount { get; set; }
    }
    public class CustomerDashbordAPI
    {
        public int Id { get; set; }
        public Guid CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string ImageURL { get; set; }
    }
    public class CustomerDetailTimestamp
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public DateTime VisitedDate { get; set; }
        public CustomerCount Count { get; set; }
        public List<CustomerDetailTimestamp> CustomerDetailTimestampList { get; set; }
    }
    public class Paging
    {
        public int PageCount { get; set; }
        public int CurrentNumber { get; set; }
        public int PageNumber { get; set; }
        public int OutOfNumber { get; set; }
    }
    public class CustomerPaging : Paging
    {
        public int PageSize { get; set; }
        public bool IsLead { get; set; }
        public List<SelectListItem> PageList { get; set; }
    }
    public class CustomerVendorAccount
    {
        public int Id { get; set; }
        public string DBA { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PrimaryPhone { get; set; }
        public string Market { get; set; }
        public string AccountType { get; set; }
        public string ServiceType { get; set; }
        public string Note { get; set; }
        public List<CustomerVendorAccount> CustomerVendorAccountList { get; set; }
        public TotalCount Count { get; set; }

    }
    public class RecurringBillingModel
    {
        public double MonitoringFee { get; set; }
        public double BillingAmount { get; set; }
    }
}
