using Forte;
using Forte.Entities;
using HS.Framework;
using HS.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using static Forte.ForteTransaction;

namespace HS.Entities
{
    public class CustomModels
    {
    }
    //public class KnowledgebaseWeblink
    //{
    //    public string RelatedArticalTitle { get; set; }

    //}
    //public class KnowledgeImage
    //{
    //    public string Location { set; get; }
    //    public string Description { set; get; }
    //    public double Size { get; set; }
    //}
    //public  class Knowledgebase
    //{
    //    //public int Id { get; set; }
    //    //public bool IsDocumentLibrary { get; set; }
    //    public int AttachmentCount { get; set; }
    //    public string UpadtedBy { get; set; }
    //    public string Comments { get; set; }
    //    public List<KnowledgebaseWeblink> KnowledgeWeblinkList { get; set; }
    //    public List<KnowledgebaseWeblink> RelatedArticleList { get; set; }
    //    public List<KnowledgeImage> Images { get; set; }
    //    public List<EstimateImage> SavedImages { get; set; }
    //    public List<string> TagsStr { get; set; }
    //    public string FlagByName { get; set; }
    //    public List<Knowledgebase> SearchedKnowledgebase { get; set; }
    //    public List<Knowledgebase> DeletedKnowledgebaseList { get; set; }
    //    public List<KnowledgeBaseFlagUserCustom> ListKnowledgeBaseFlagUser { get; set; }
    //    public int TotalCount { get; set; }
    //    public List<int> UserGroups { get; set; }
    //    public bool IsDefault { get; set; }
    //    public List<string> StrUserGroups { get; set; }
    //    public KnowledgebaseAccountability IsAssigned { get; set; }
    //    public string AssignTo { get; set; }
    //    public DateTime DueDate { get; set; }
    //    public DateTime EndDate { get; set; }
    //}
    //public class KnowledgeBaseFlagUserCustom
    //{
    //    public string Name { get; set; }
    //    public string Comment { get; set; }
    //    public DateTime Date { get; set; }
    //    public string DateC { get; set; }
    //}
    //public  class KnowledgeSearchedKeyword
    //{

    //}
    //public class KnowledgebaseListModel
    //{
    //    public List<Knowledgebase> KnowledgebaseList { get; set; }
    //    public Guid CompanyId { get; set; }
    //    public int TotalCount { get; set; }
    //    public int DeletedCount { get; set; }
    //    public int TotalKnFlagCount { get; set; }
    //}
    //public class KnowledgebaseHomeModel
    //{
    //    public List<Knowledgebase> RecentViewedlist { get; set; }
    //    public List<Knowledgebase> MostViewedlist { get; set; }
    //    public List<KnowledgeSearchedKeyword> KnowledgeSearchedKeywordList { get; set; }
    //    public List<RMRTag> RMRTagList { get; set; }
    //    public int FlaggedCount { get; set; }
    //}
    //public partial class KnowledgebaseAccountability
    //{
    //    public string AssignedByUserName { set; get; }
    //    public string Title { set; get; }
    //}
    //public class AccessedKnowledgebase
    //{
    //    public string Title { get; set; }
    //    public int Id { get; set; }
    //    public bool IsDefault { get; set; }
    //}

    public class ImageUrl
    {
        public string url { set; get; }
        public string caption { set; get; }
    }
    public class KnowledgeImage
    {
        public string Location { set; get; }
        public string Description { set; get; }
        public double Size { get; set; }
    }
    public class RoutingNumberAPIResponse
    {
        public string name { set; get; }
        public string rn { set; get; }
    }
    public class RugFile
    {
        public string Location { set; get; }
        public string Description { set; get; }
    }
    public class HolidayReturnModel
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public float Hours { get; set; }
        public int Days { get; set; }
        public string WeekEnd { get; set; }
    }
    public class UploadStatus
    {
        public string filepath { get; set; }
        public string filefullpath { get; set; }
        public string filename { get; set; }
        public double size { get; set; }
    }
    public class FinishedJobReport
    {
        public int Id { set; get; }//TicketId
        public int CustomerId { set; get; }
        public string CustomerName { set; get; }
        public string BookingId { set; get; }
        public int BookingIntId { set; get; }
        public string Address { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public double Discount { set; get; }
        public double TotalPrice { set; get; }
    }
    public class FinishedJobReportModel
    {
        public List<FinishedJobReport> FinishedJobReportList { set; get; }
        public PayrollTotalCount PayrollTotalCount { set; get; }


    }
    public class UserDetails
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public bool IsRead { get; set; }
    }
    public class AssignedUserModel
    {
        public List<UserDetails> Users { get; set; }
        public List<Guid> UserIds { get; set; }
    }
    public class UnassignedLeadCount
    {
        public int TotalLeads { get; set; }
    }
    public class AccountabilityReportModel
    {
        public List<AccountabilityReport> List { get; set; }
        public int TotalCount { get; set; }
    }
    public class AccountabilityHistoryReportModel
    {
        public List<AccountabilityHistory> List { get; set; }
        public int TotalCount { get; set; }
    }
    public class AccountabilityHistory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string AssignedBy { get; set; }
    }
    public class AccountabilityReport
    {
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public int Artical { get; set; }
        public int TotalUnread { get; set; }
        public int TotalCompleted { get; set; }
    }
    public class KnowledgebaseReport
    {
        public string Searchkey { get; set; }
        public int Count { get; set; }
    }
    public class KnowledgebaseReportModel
    {
        public List<KnowledgebaseReport> List { get; set; }
        public int TotalCount { get; set; }
    }
    public class KnowledgebaseSearchedHistory
    {
        public string SearchBy { get; set; }
        public DateTime SearchDate { get; set; }
    }
    public class KnowledgebaseSearchedHistoryModel
    {
        public List<KnowledgebaseSearchedHistory> List { get; set; }
        public int TotalCount { get; set; }
    }
    public class AccessedKnowledgebaseHistory
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsDocumentLibrary { get; set; }
        public int Count { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class AccessedKnowledgebaseHistoryModel
    {
        public List<AccessedKnowledgebaseHistory> List { get; set; }
        public int TotalCount { get; set; }
    }
    public class AccessedHistory
    {
        public string AccessedBy { get; set; }
        public DateTime AccessedDate { get; set; }
    }
    public class AccessedHistoryModel
    {
        public List<AccessedHistory> List { get; set; }
        public int TotalCount { get; set; }
    }

    public class PackageSummaryModel
    {
        public string CustomerName { set; get; }
        public int CustomerId { set; get; }
        public string BookingId { set; get; }
        public int BookingIntId { set; get; }
        public string PackageName { set; get; }
        public int RugQty { set; get; }
        public string RugShape { set; get; }
        public double Value { set; get; }
        public bool IsPaid { set; get; }
    }

    #region Base Filter Model
    public class BaseFilterModel
    {
        private string _StrStartDate { set; get; }
        private string _StrEndDate { set; get; }
        public string StrStartDate
        {
            get { return _StrStartDate; }
            set
            {
                _StrStartDate = value;
                this.StartDate = value.ToDateTime();
            }
        }
        public string StrEndDate
        {
            get { return _StrEndDate; }
            set
            {
                _StrEndDate = value;
                this.EndDate = value.ToDateTime();
            }
        }
        public DateTime? StartDate { set; get; }
        public DateTime? EndDate { set; get; }
        public string SearchText { set; get; }
        public int PageNO { set; get; }
        public int PageSize { set; get; }
    }
    #endregion
    public class FinishedJobFilter : BaseFilterModel
    {
        public string order { set; get; }
        public bool? IsDownload { get; set; }
    }

    public class PackageSummaryFilter : BaseFilterModel
    {

    }

    public class CommonJesonresponse
    {
        public bool result { set; get; }
        public string message { set; get; }
    }
    public class AddTicketModel
    {
        public Ticket Ticket { set; get; }
        public Guid[] Assigned { set; get; }
        public Guid[] UserList { set; get; }
        public Guid[] NotifyingUserList { set; get; }
        public String[] ReasonList { set; get; }
        public string amount { set; get; }
        public string desc { set; get; }
        public CustomerAppointment CustomerAppointment { set; get; }
        //List<string> AddedEquipmentList, List<string> RemovedEquipmentList, 
        public bool? NotifyCustomer { set; get; }
        public string EquipmentType { set; get; }
        public bool? IsStatusChange { set; get; }
        public bool? IsDispatch { set; get; }
        #region For Booking Items
        public string BookingId { set; get; }
        public List<TicketBookingDetails> TicketBookingDetails { set; get; }
        public List<TicketBookingExtraItem> TicketBookingExtraItems { set; get; }
        public bool? RecreateInvoice { set; get; }
        #endregion
        public string Lat { get; set; }
        public string Lng { get; set; }
    }
    public class ReceivePaymentResponse
    {
        public ReceivePaymentResponse()
        {
            this.TransactionSuccess = false;
        }
        public bool TransactionSuccess { set; get; }
        public string TransactionId { set; get; }
        public string Message { set; get; }
    }
    public class EquipmentOptions
    {
        public string OptionName { set; get; }
        public Guid EquipmentId { set; get; }
    }
    public class SmartSetupRcvPaymentResponse
    {
        public SmartSetupRcvPaymentResponse()
        {
            this.FinalMesage = "";
            this.PaymentReceived = false;
            this.Result = false;
            this.Caution = false;
        }
        public string FinalMesage { set; get; }
        public bool PaymentReceived { set; get; }
        public bool Result { set; get; }
        public bool Caution { set; get; }

    }

    #region ForteTransection
    public class ResourceSpecific
    {
        public string location_id { get; set; }
        public string customer_token { get; set; }
    }

    public class SearchCriteria
    {
        public int page_size { get; set; }
        public int page_index { get; set; }
        public string home_organization_id { get; set; }
        public ResourceSpecific resource_specific { get; set; }
    }

    public class PhysicalAddress
    {
        public string street_line1 { get; set; }
        public string locality { get; set; }
        public string region { get; set; }
        public string postal_code { get; set; }
    }

    public class BillingAddress
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string company_name { get; set; }
        public PhysicalAddress physical_address { get; set; }
    }

    public class Card
    {
        public string name_on_card { get; set; }
        public string last_4_account_number { get; set; }
        public string masked_account_number { get; set; }
        public int expire_month { get; set; }
        public int expire_year { get; set; }
        public string card_type { get; set; }
    }

    public class Response
    {
        public string response_code { get; set; }
        public string authorization_code { get; set; }
    }

    public class Links
    {
        public string disputes { get; set; }
        public string settlements { get; set; }
        public string self { get; set; }
    }

    public class ResultTransection
    {
        public string transaction_id { get; set; }
        public string organization_id { get; set; }
        public string location_id { get; set; }
        public string customer_token { get; set; }
        public string status { get; set; }
        public string action { get; set; }
        public double authorization_amount { get; set; }
        public string authorization_code { get; set; }
        public string entered_by { get; set; }
        public DateTime received_date { get; set; }
        public BillingAddress billing_address { get; set; }
        public Card card { get; set; }
        public Response response { get; set; }
        public Links links { get; set; }
    }

    public class Response2
    {
        public string environment { get; set; }
        public string response_desc { get; set; }
    }

    public class Links2
    {
        public string self { get; set; }
    }

    public class ForteTransectionResponse
    {
        public int number_results { get; set; }
        public SearchCriteria search_criteria { get; set; }
        public List<ResultTransection> results { get; set; }
        public Response2 response { get; set; }
        public Links2 links { get; set; }
    }
    public class ForteScheduleResponse
    {
        public int number_results { get; set; }
        public SearchCriteria search_criteria { get; set; }
        //public List<ForteScheduleV3> results { get; set; }
        public Response2 response { get; set; }
        public Links2 links { get; set; }
        public string schedule_id { set; get; }
        public string location_id { set; get; }
        public string customer_token { set; get; }
        public string paymethod_token { set; get; }
        public string action { set; get; }
        public int schedule_quantity { set; get; }
        public string schedule_frequency { set; get; }
        public float schedule_amount { set; get; }
        public string schedule_service_fee_amount { set; get; }
        public string schedule_authorization_amount { set; get; }
        public DateTime schedule_created_date { set; get; }
        public DateTime schedule_start_date { set; get; }
        public string schedule_status { set; get; }
        public string item_description { set; get; }
        public string reference_id { set; get; }
        public string order_number { set; get; }
        public ScheduleSummary schedule_summary { set; get; }
    }
    #endregion
    public class FortePaymentResponse
    {
        public string location_id { get; set; }
        public string action { get; set; }
        public string authorization_amount { get; set; }
        public string entered_by { get; set; }
        public ForteResponse response { get; set; }
        public CardClass card { get; set; }
        public billing_addressClass billing_address { get; set; }
        public Array players { get; set; }
        public string transaction_id { set; get; }
        public string paymethod_token { set; get; }
        public string customer_token { set; get; }
        public string schedule_id { set; get; }

    }

    public class ErrorResponse
    {
        public string environment { get; set; }
        public string response_desc { get; set; }
    }

    public class ForteErrorResoponse
    {
        public ErrorResponse response { get; set; }
    }
    public class UserProfile
    {
        public UserLogin UserLogin { set; get; }
        public Employee Employee { set; get; }
    }
    public class ResetLoginUser
    {
        public int UserLoginId { get; set; }
        public UserLogin UserLogin { set; get; }
        public Employee Employee { set; get; }
    }
    public class CustomerLeadGraph
    {
        public int CustomerCount { set; get; }
        public int LeadCount { set; get; }
        public string Month { set; get; }
    }
    public class TechScheduleProfile
    {
        public Employee Employee { get; set; }
        public TechSchedule TechSchedule { get; set; }
    }
    public class CustomInvoiceModel
    {
        public Invoice Invoice { get; set; }
        public InvoiceDetail InvoiceDetail { get; set; }
        public Equipment Equipment { get; set; }
    }


    public class EquipmentSearchModel
    {
        public Guid EquipmentId { set; get; }
        public string EquipmentName { set; get; }
        public double RetailPrice { set; get; }
        public double SupplierCost { set; get; }
        public int Reorderpoint { set; get; }
        public int QuantityAvailable { set; get; }
        public string EquipmentType { set; get; }
        public string EquipmentDescription { set; get; }
        public int Id { get; set; }
        public int EquipmentClassId { get; set; }
        public string ManufacturerName { set; get; }
        public string SKU { set; get; }
        public string Barcode { set; get; }
        public int QuantityOnHand { get; set; }
        public int WareHouseQuantity { get; set; }
        public bool IsTaxable { get; set; }
        public double Point { set; get; }
        public double Equipmentvendorcost { get; set; }

    }
    public class EquipmentSearchModelEstimator
    {
        public Guid EquipmentId { set; get; }
        public string EquipmentName { set; get; }
        public double RetailPrice { set; get; }
        public double SupplierCost { set; get; }
        public string EquipmentType { set; get; }
        public int Id { get; set; }
        public string EquipmentDescription { set; get; }
        public double Point { set; get; }
        public string SKU { set; get; }
        public int EquipmentTypeId { set; get; }
        public Guid SupplierId { set; get; }
        public Guid ManufacturerId { set; get; }
        public string Unit { set; get; }
        public double OverheadRate { set; get; }
        public double ProfitRate { set; get; }
    }
    public class CustomerSearchModel
    {
        public Guid CustomerId { set; get; }
        public string Address { set; get; }
        public string Address1 { set; get; }
        public string Street { set; get; }
        public string City { set; get; }
        public string State { set; get; }
        public string ZipCode { set; get; }
        public string Street1 { set; get; }
        public string City1 { set; get; }
        public string State1 { set; get; }
        public string ZipCode1 { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string EmailAddress { set; get; }
        public string BusinessName { set; get; }
        public string Type { get; set; }
    }
    public class GlobalSearchModel
    {
        public string Name { set; get; }
        public string BusinessName { set; get; }
        public string PhoneNumber { set; get; }
        public string EmailAddress { set; get; }
        public string Type { set; get; }
        public int Id { set; get; }
    }
    public class GlobalSearchViewModel
    {
        public List<Customer> Customers { set; get; }
        public List<Customer> Leads { set; get; }
        public List<Invoice> Invoices { set; get; }
        public List<Invoice> Estimates { set; get; }
        public List<Contact> Contacts { set; get; }
        public List<Opportunity> Opportunities { set; get; }
        public List<Ticket> Tickets { get; set; }
    }
    public class CustomInventoryEquipmentModel
    {
        public int EuipmentIntId { get; set; }
        public int InventoryIntId { get; set; }
        public Guid EquipmentId { get; set; }
        public Guid CompanyId { get; set; }
        public Double EquipmentCost { get; set; }
        public Double InventoryCost { get; set; }
        public DateTime EquipmentAsOfDate { get; set; }
        public int InventoryQuantity { get; set; }
        public string ccEmail { get; set; }
        //public Inventory Inventory { get; set;}
        //public Equipment Equipment { get; set; }
    }
    public class CoverLetter
    {
        public string Logo { get; set; }
        public string Address { get; set; }
        public string BillingAddress { get; set; }
        public string ProjectAddress { get; set; }
        public string EstimatorId { get; set; }
        public string CustomerName { get; set; }
        public string SalesGuy { get; set; }
        public string CompanyName { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
    }
    public class PaymentHistory
    {
        public Double Amount { get; set; }
        public string numberCard { get; set; }
        public string chkno { get; set; }
        public string method { get; set; }
        public double RefundAmount { get; set; }
    }
    public class CreateEstimator
    {
        public string EstEstimatorId { get; set; }
        public Estimator Estimator { set; get; }
        public int ContractId { set; get; }
        public List<EstimatorDetail> estimatorDetails { set; get; }
        public List<EstimatorService> estimatorServices { set; get; }
        public List<EstimatorService> estimatorOneTimeServices { set; get; }
        public List<EstimatorNote> EstimatorNotes { set; get; }
        public ContractAgreementTemplate contractAgreemntTemplt { set; get; }
        public CustomerAgreementTemplate cusAgreemntTemplt { get; set; }
        public EstimatorSetting EstimatorSetting { get; set; }
        public string EstimatorContractTerm { get; set; }
        public Customer Customer { set; get; }
        public Ticket Ticket { get; set; }
        public Company Company { set; get; }
        public string CreatePO { set; get; }
        public List<InvoiceDetail> InvoiceDetailList { set; get; }
        public double SubTotal { set; get; }
        public double ServiceTax { set; get; }
        public double ServiceSubTotal { get; set; }
        public double TotalServiceAmount { get; set; }
        public string PhoneNo { get; set; }
        #region CustomerInfo
        public string CustomerName { set; get; }
        public string CusBussinessName { get; set; }
        public string CustomerNo { set; get; }
        public string CustomerInfo { get; set; }
        public string CusType { get; set; }
        public string CustomerStreetInfo { get; set; }
        public string CustomerStreet { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerCity { set; get; }
        public string CustomerState { set; get; }
        public string CustomerZipCode { set; get; }
        public string Soldby { set; get; }
        #endregion
        #region Company Info
        public string CompanyEmail { set; get; }
        public string CompanyName { set; get; }
        public string CompanyAddress { set; get; }
        public string CompanyWebsite { set; get; }
        public string companyStreetInfo { get; set; }
        public string CompanyStreet { get; set; }
        public string CompanyLogo { get; set; }
        public string eSecurityLogo { get; set; }
        public string specializedLogo { get; set; }
        public string CompanyCity { get; set; }
        public string CompanyState { get; set; }
        public string CompanyZip { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyInfo { get; set; }
        #endregion
        public bool ShowEstimatorShippingAddress { get; set; }
        public bool ShowEstimatorContractTerm { get; set; }
        //public EstimatorPDFFilters EstimatorPDFFilters { set; get; }
        public EstimatorPDFFilter _EstimatorPDFFilter { set; get; }
        public List<PurchaseOrderWarehouse> PurchaseOrderWarehouse { set; get; }
        public int[] ChildEstimator { get; set; }
        public bool PrintEstimator { get; set; }

    }
    public class EstimatorPDFFilters
    {
        public EstimatorPDFFilters()
        {
            //this.ShowEverything = false;
            this.IncludeProfit = false;
            this.GroupedbyCategory = false;
            this.IncludeMargin = false;
            this.GroupedbySupplier = false;
            this.IncludeOverhead = false;
            this.IncludeCost = false;
            this.GroupedbyLabor = false;
            this.WithoutPricing = false;
            this.GroupedbyMaterial = false;
            this.GroupedbyLaborAndMaterial = false;
            this.WithoutIndividualMaterialPricing = false;
            this.WithoutIndividualLaborPricing = false;
            this.IncludeImages = false;
            this.IncludePDFs = false;
            this.IncludeService = false;
            this.IncludeManufacturers = false;
            this.IncludeVariation = false;
        }
        public bool GroupedbyNone { set; get; }
        public bool GroupedbyCategory { set; get; }
        public bool GroupedbySupplier { set; get; }
        public bool GroupedbyLabor { set; get; }
        public bool GroupedbyMaterial { set; get; }
        public bool GroupedbyLaborAndMaterial { set; get; }


        //public bool ShowEverything{set; get;}
        public bool IncludeCost { set; get; }
        public bool IncludeProfit { set; get; }
        public bool IncludeMargin { set; get; }
        public bool IncludeOverhead { set; get; }
        public bool WithoutPricing { set; get; }
        public bool WithoutIndividualMaterialPricing { set; get; }
        public bool WithoutIndividualLaborPricing { set; get; }
        public bool IncludeImages { set; get; }
        public bool IncludePDFs { set; get; }
        public bool IncludeService { get; set; }
        public bool IncludeManufacturers { get; set; }
        public bool IncludeVariation { get; set; }
    }
    public class CreateInvoice
    {
        public List<PaymentHistory> PaymentHistoryList { get; set; }
        public Invoice Invoice { set; get; }
        public List<InvoiceDetail> InvoiceDetailList { set; get; }
        public List<InvoiceDetail> InvoiceDetailServiceList { get; set; }

        public string EmailAddress { set; get; }
        public string CustomerSSN { get; set; }
        public DateTime CustomerDOB { get; set; }
        public string CustomerDrivingLicense { get; set; }
        public double SubTotal { set; get; }
        public double Discount { set; get; }
        public InvoiceSetting InvoiceSetting { get; set; }
        public List<InvoiceNote> InvoiceNotes { set; get; }
        public Transaction Transaction { set; get; }
        public string PhoneNum { get; set; }
        //public string ccEmail { get; set; }
        //public string EstimateShipping { get; set; }
        //public string InvoiceShipping { get; set; }
        public string EmailDescription { get; set; }
        public string InvoiceEquipmentName { get; set; }
        public string InvoiceEquipmentDescription { get; set; }
        public string EmailSubject { get; set; }
        public string AmountInWord { set; get; }
        public List<EstimateImage> EstimateImage { get; set; }
        public string JobNo { get; set; }
        public bool ShowInvoiceShippingAddress { get; set; }
        public bool ShowInvoiceStaticBox { get; set; }
        public bool ShowCode3InvoiceStaticBox { get; set; }
        public bool ShowInvoiceCompanyAddress { get; set; }
        public bool ShowPaymentAddressForSendInvoice { get; set; }
        public string PaymentAddress { get; set; }
        public bool ShowEstimateMonitoringAmount { get; set; }
        public bool ShowEstimateOldButton { get; set; }
        public bool ShowEstimateContractTerm { get; set; }
        public bool ShowOnitWaterTreatment { get; set; }

        public bool ShowGutterEquipmentImage { get; set; }
        public bool ShowThompsonEstimateText { get; set; }
        public bool ShowEstimateMonitoringDescription { get; set; }
        public bool ShowEstimateDefaultLineItem { get; set; }
        public List<GlobalSetting> PrintSettings { get; set; }

        #region CustomerInfo
        public string CustomerName { set; get; }
        public string CusBussinessName { get; set; }
        public string CustomerNo { set; get; }
        public string CustomerInfo { get; set; }
        public string CusType { get; set; }
        public string CustomerStreetInfo { get; set; }
        public string CustomerStreet { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerCity { set; get; }
        public string CustomerState { set; get; }
        public string CustomerZipCode { set; get; }
        #endregion

        #region Company Info
        public string CompanyEmail { set; get; }
        public string CompanyName { set; get; }
        public string CompanyAddress { set; get; }
        public string CompanyWebsite { set; get; }
        public string companyStreetInfo { get; set; }
        public string CompanyStreet { get; set; }
        public string CompanyLogo { get; set; }
        public string CompanyCity { get; set; }
        public string CompanyState { get; set; }
        public string CompanyZip { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyInfo { get; set; }
        #endregion

        public string ConStr { get; set; }
        public string CurrencyType { get; set; }
        public CustomerInspection CustomerInspection { get; set; }
        public Company Company { get; set; }
        public Customer Customer { get; set; }
        public string DescriptionDropdown { get; set; }
        public CreateInvoice()
        {
            Invoice = new Invoice()
            {
                Amount = 0,
                BalanceDue = 0,
                Balance = 0,
                Deposit = 0,
                Tax = 0,
                LateFee = 0,
                LateAmount = 0,
                Status = "Init",
                CreatedDate = DateTime.Now,
                InvoiceFor = "Others",
                InvoiceDate = DateTime.Now,
                DueDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now,
                PaymentType = "-1"
            };
            InvoiceSetting = new InvoiceSetting();
            InvoiceDetailList = new List<InvoiceDetail>();
        }
    }
    public class CreateCustomerAppoinmentEquipment
    {
        public string POId { get; set; }
        public List<CustomerAppointmentEquipment> CusAppoinmentEquipmentList { get; set; }
        public string FullfillmentDate { get; set; }
        public string PickupShipped { get; set; }
        public Guid SupplierId { get; set; }
        public Guid TechnicianId { get; set; }
        public Supplier SupplierModel { get; set; }
        public Employee TechnicianModel { get; set; }
        public string EmailAddress { set; get; }
        public string PhoneNum { get; set; }
        public string EmailDescription { get; set; }
        public string EmailSubject { get; set; }
        public string ShippingAddress { get; set; }
        public bool ShowShippingAddress { get; set; }
        public string SupplierInfo { get; set; }
        public string EmployeeInfo { get; set; }
        public bool SendMail { get; set; }

        #region CustomerInfo
        public string CustomerName { set; get; }
        public string CusBussinessName { get; set; }
        public string CustomerNo { set; get; }
        public string CustomerInfo { get; set; }
        public string CusType { get; set; }
        public string CustomerStreetInfo { get; set; }
        public string CustomerStreet { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerCity { set; get; }
        public string CustomerState { set; get; }
        public string CustomerZipCode { set; get; }
        #endregion

        #region Company Info
        public string CompanyEmail { set; get; }
        public string CompanyName { set; get; }
        public string CompanyAddress { set; get; }
        public string CompanyWebsite { set; get; }
        public string companyStreetInfo { get; set; }
        public string CompanyStreet { get; set; }
        public string CompanyLogo { get; set; }
        public string CompanyCity { get; set; }
        public string CompanyState { get; set; }
        public string CompanyZip { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyInfo { get; set; }
        #endregion
    }
    /// <summary>
    /// CompanyId CustomerGId PaymentMethod required
    /// </summary>
    public class ReceivePaymentAPIModel
    {
        public Guid customerGuid { set; get; }
        public Guid CompanyId { set; get; }
        public string CompanyName { set; get; }
        public int customerId { set; get; }
        public string CustomerNo { set; get; }
        public string CustomerName { set; get; }
        public DateTime PaymentDate { set; get; }
        public string paymentMethod { set; get; }
        public string reference { set; get; }
        public string AuthorizeSubscriptionId { set; get; }
        public string CustomerProfileId { set; get; }
        public string CustomerPaymentProfileId { set; get; }
        public string DepositTo { set; get; }
        public string EmailAddress { set; get; }
        public double amount { set; get; }
        public double GeneralCreditAmount { set; get; }
        public double RMRCreditAmount { set; get; }
        public List<OutStandingTransactions> transactions { set; get; }
        public bool SendEmail { set; get; }
        public cardInfo card { set; get; }
        public achInfo ach { set; get; }

        public OthersInfo OthersInfo { set; get; }

        public string Description { set; get; }
        public string InvoiceList { set; get; }
        //public Customer Customer { set; get; }
    }
    public class cardInfo : BillingAddressAuthorize
    {
        public string CardNumber { set; get; }
        public string ExpiredDate { set; get; }
        public string NameOnCard { set; get; }
        public string SecurityCode { set; get; }
        public string CheckNo { set; get; }

        public double Amount { set; get; }
        public string FirstName { set; get; }
        public string Lastname { set; get; }
        public string InvoiceNo { set; get; }
        public string Description { set; get; }
        public string CustomerId { set; get; }
        public string EmailAddress { set; get; }
        public string CardType { set; get; }
        public int PaymentProfileId { set; get; }
    }
    public class achInfo : BillingAddressAuthorize
    {
        public string RoutingNo { set; get; }
        public string AccountNo { set; get; }
        public string AccountType { set; get; }
        public string ECheckType { set; get; }
        public string BankName { set; get; }
        public string AccountName { set; get; }

        public double Amount { set; get; }
        public string FirstName { set; get; }
        public string Lastname { set; get; }
        public string InvoiceNo { set; get; }
        public string Description { set; get; }
        public string CustomerId { set; get; }
        public string EmailAddress { set; get; }
        public int PaymentProfileId { set; get; }
    }

    public class ReceivePaymentModel
    {
        public Guid CustomerGId { set; get; }
        public Guid CompanyId { set; get; }
        public string CompanyName { set; get; }
        public int CustomerId { set; get; }
        public string CustomerNo { set; get; }
        public string CustomerName { set; get; }
        public DateTime PaymentDate { set; get; }
        public string PaymentMethod { set; get; }
        public string RefNo { set; get; }
        public string AuthorizeSubscriptionId { set; get; }
        public string CustomerProfileId { set; get; }
        public string CustomerPaymentProfileId { set; get; }
        public string DepositTo { set; get; }
        public string EmailAddress { set; get; }
        public double AmoutReceived { set; get; }
        public double GeneralCreditAmount { set; get; }
        public double RMRCreditAmount { set; get; }
        public List<OutStandingTransactions> Transactions { set; get; }
        public bool SendEmail { set; get; }
        public CardInfo CardInfo { set; get; }
        public ACHInfo ACHInfo { set; get; }

        public OthersInfo OthersInfo { set; get; }

        public string Description { set; get; }
        public string InvoiceList { set; get; }
        //public Customer Customer { set; get; }
    }

    public class ReceiveFortePaymentModel
    {
        public string expire_year { set; get; }
        public string card_number { set; get; }
        public string api_login_id { set; get; }
        public string expire_month { set; get; }
        public string cvv { set; get; }
        public string account_number { set; get; }
        public string routing_number { set; get; }
        public string account_type { set; get; }

    }

    public class BillingAddressAuthorize
    {
        public string Company { set; get; }
        public string Phone { set; get; }
        public string BillingAddress { set; get; }
        public string City { set; get; }
        public string State { set; get; }
        public string Zipcode { set; get; }
    }
    public class OthersInfo
    {
        public string PaymentMethodOthers { set; get; }
        public string ConfirmationNumber { set; get; }
    }

    public class CardInfo : BillingAddressAuthorize
    {
        public string CardNumber { set; get; }
        public string ExpiredDate { set; get; }
        public string NameOnCard { set; get; }
        public string SecurityCode { set; get; }
        public string CheckNo { set; get; }

        public double Amount { set; get; }
        public string FirstName { set; get; }
        public string Lastname { set; get; }
        public string InvoiceNo { set; get; }
        public string Description { set; get; }
        public string CustomerId { set; get; }
        public string EmailAddress { set; get; }
        public string CardType { set; get; }
        public int PaymentProfileId { set; get; }
    }
    public class ACHInfo : BillingAddressAuthorize
    {
        public string RoutingNo { set; get; }
        public string AccountNo { set; get; }
        public string AccountType { set; get; }
        public string ECheckType { set; get; }
        public string BankName { set; get; }
        public string AccountName { set; get; }

        public double Amount { set; get; }
        public string FirstName { set; get; }
        public string Lastname { set; get; }
        public string InvoiceNo { set; get; }
        public string Description { set; get; }
        public string CustomerId { set; get; }
        public string EmailAddress { set; get; }
        public int PaymentProfileId { set; get; }
    }
    public class OutStandingTransactions
    {
        public int Id { set; get; }
        public string Description { set; get; }
        public string JobName { set; get; }
        public string InvoiceId { set; get; }
        public string POId { set; get; }
        public DateTime DueDate { set; get; }
        public double OriginalAmount { set; get; }
        public double OpenBalance { set; get; }
        public double Payment { set; get; }
        public DateTime CreatedDate { set; get; }
    }

    public class IspcAccountInfo
    {
        public string ApplicationId { get; set; }
        public string StateOfInstallation { get; set; }
        public string MyProperty { get; set; }
        public string ApplicantEmailAddress { get; set; }
        public string ApplicantDriversLicense { get; set; }
        public string ApplicantEmployer { get; set; }
        public string ApplicantHomePhone { get; set; }
        public string ApplicantCellPhone { get; set; }
        public string CoApplicantEmailAddress { get; set; }
        public string CoApplicantDriversLicense { get; set; }
        public string CoApplicantEmployer { get; set; }
        public string CoApplicantHomePhone { get; set; }
        public string CoApplicantCellPhone { get; set; }
        public double PaymentAmount { get; set; }
        public string AccountNumber { get; set; }
        public string RoutingNumber { get; set; }
        public string BillingMethod { get; set; }
        public string DayToBill { get; set; }
        public string SigningApplicant { get; set; }
        public bool SignatureRequired { get; set; }

    }
    public class IspcESigningResponse
    {
        public int ApplicationId { get; set; }
        public string Status { get; set; }
        public List<string> MissingEsignFields { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class MakePayment
    {
        public int SupplierId { set; get; }
        public DateTime PaymentDate { set; get; }
        public string PaymentMethod { set; get; }
        public string RefNo { set; get; }
        public string EmailAddress { set; get; }
        public double AmountDue { set; get; }
        public List<OutStandingTransactions> Transactions { set; get; }
        public PaymentInfo PaymentInfo { set; get; }
    }
    public class ShowBillModel
    {
        public List<ShowBillModel> ShowBillModelList { get; set; }
        public int Id { set; get; }
        public int PaymentId { set; get; }
        public string BillNo { get; set; }
        public string JobName { get; set; }
        public double TotalAmount { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { set; get; }
        public double OpenBalance { set; get; }
        public string PaymentStatus { set; get; }
        public string Type { set; get; }
        public string SupplierName { set; get; }
        public string PaymentMethod { set; get; }
        public List<CustomerBillAccoutType> CustomerBillAccoutType { get; set; }
        public int SupplierId { get; set; }
        public Guid EMPID { get; set; }
        public string InvoiceId { get; set; }
        public string PurchaseOrderId { get; set; }
        public string Notes { get; set; }

        public double TTAmount { get; set; }

        public double TotalOpenBalance { get; set; }


    }

    public class CustomerBillAccoutType
    {
        public int CustomerBillID { set; get; }
        public string Type { set; get; }
    }
    public class CheckPayment
    {
        public int PaymentId { set; get; }
        public string SupplierName { set; get; }
        public string BillId { set; get; }
        public double Amount { set; get; }
    }
    public class MakePaymentModel
    {
        //public Guid CustomerGId { set; get; }
        // public int CustomerId { set; get; }
        public PaymentInfo PaymentInfo { set; get; }
        public DateTime PaymentDate { set; get; }
        public string PaymentMethod { set; get; }
        public string RefNo { set; get; }
        public string DepositTo { set; get; }
        //public string EmailAddress { set; get; }
        public double AmoutReceived { set; get; }
        public List<OutStandingTransactions> Transactions { set; get; }
        public string CompanyAddress { set; get; }
        public string CompanyName { set; get; }
        public string CompanyEmail { get; set; }

        //public bool SendEmail { set; get; }
    }
    public class StockStatus
    {
        public int LowStockQuantity { get; set; }
        public int OutOfStockQuantity { get; set; }
        public List<Equipment> LowStockEquipmentList { get; set; }
        public List<Equipment> OutOfStockEquipmentList { get; set; }
    }
    [DataContract]
    public class ReportModel
    {
        public string Name { set; get; }
        public string Value { set; get; }
    }
    public class AddCustomerAppointment
    {
        public string ServiceOrderCustomerName { get; set; }
        public string ServiceOrderCustomerEmail { get; set; }
        public string ServiceOrderCustomerPhone { get; set; }
        public string ServiceOrderCustomerAddress { get; set; }
        public string ServiceOrderEmployeeName { get; set; }
        public string ServiceOrderCompanyAddress { get; set; }
        public string ServiceOrderCompanyEmail { get; set; }
        public string ServiceOrderCompanyName { get; set; }
        public double ServiceOrderTotal { get; set; }
        //public string ServiceOrderEquipmentServiceName {get;set;}
        public CustomerAppointmentEquipment CustomerAppointmentEquipment { set; get; }
        public CustomerAppointment CustomerAppointment { get; set; }
        public List<CustomerAppointmentEquipment> CustomerAppointmentEquipmentList { set; get; }
        public string TotalServiceOrderPrice { get; set; }
        public string ServiceOrderTaxAmount { get; set; }
        public string ServiceOrderSubTotalAmount { get; set; }
        public List<ServiceOrderPdfDetail> ServiceOrderPdfDetail { get; set; }
        public string PriceServiceOrderTotal { get; set; }
        public string CompanyLogo { get; set; }
        public string ServiceZone1 { get; set; }
        public string ServiceZone2 { get; set; }
        public string ServiceZone3 { get; set; }
        public string ServiceZone4 { get; set; }
        public string ServiceZone5 { get; set; }
        public string ServiceZone6 { get; set; }
        public string ServiceZone7 { get; set; }
        public string ServiceZone8 { get; set; }
        public string ServiceZone9 { get; set; }
        public string ServiceNote { get; set; }
        public string CityName { get; set; }
        public string StreetName { get; set; }
        public string StateName { get; set; }
        public string ZipName { get; set; }
        public List<CustomerAppointmentTechnician> ListCustomerAppointmentTechnician { get; set; }
    }
    public class AddCustomerAppointmentWorkOrder
    {

        public string WorkOrderCustomerName { get; set; }
        public string WorkOrderCustomerEmail { get; set; }
        public string WorkOrderCustomerPhone { get; set; }
        public string WorkOrderCustomerAddress { get; set; }
        public string WorkOrderCustomerSteet { get; set; }
        public string WorkOrderCustomerCity { get; set; }
        public string WorkOrderCustomerZipCode { get; set; }
        public string WorkOrderCustomerState { get; set; }
        public string WorkOrderEmployeeName { get; set; }
        public string WorkOrderCompanyAddress { get; set; }
        public string WorkOrderCompanyEmail { get; set; }
        public string WorkOrderCompanyName { get; set; }
        public string WorkOrderInstallType { get; set; }
        public double WorkOrderTotal { get; set; }
        public string PriceWorkOrderTotal { get; set; }
        public CustomerAppointmentEquipment CustomerAppointmentEquipment { set; get; }
        public CustomerAppointment CustomerAppointment { get; set; }
        public List<CustomerAppointmentEquipment> CustomerAppointmentEquipmentList { set; get; }
        public CustomerAppointmentDetail CustomerAppointmentDetail { get; set; }
        public List<CustomerAppointmentDetail> CustomerAppointmentDetailList { set; get; }
        public List<WorkOrderPdfDetail> WorkOrderPdfDetail { get; set; }
        public string WorkOrderTaxPercent { get; set; }
        public string WorkOrderTaxAmount { get; set; }
        public string WorkOrderSubTotalAmount { get; set; }
        public string CompanyLogo { get; set; }
        public List<CustomerAppointmentTechnician> ListCustomerAppointmentTechnician { get; set; }
    }

    public class AddLeadEquipment
    {
        public CustomerAppointmentEquipment LeadCustomerAppointmentEquipment { set; get; }
        public CustomerAppointment LeadCustomerAppointment { get; set; }
        public List<CustomerAppointmentEquipment> LeadCustomerAppointmentEquipmentList { set; get; }
        public List<CustomerAppointmentEquipment> PackageEquipmentsList { get; set; }
        public List<PackageDetailCustomer> packageDetailCustomer { get; set; }
    }
    public class AddSmartLeadEquipment
    {
        public CustomerPackageEqp LeadCustomerAppointmentEquipment { set; get; }
        public CustomerAppointment LeadCustomerAppointment { get; set; }
        public List<CustomerPackageEqp> LeadCustomerAppointmentEquipmentList { set; get; }
        public List<CustomerPackageEqp> PackageEquipmentsList { get; set; }
        public List<CustomerPackageService> PackageServiceList { set; get; }
        public List<PackageDetailCustomer> packageDetailCustomer { get; set; }
    }
    public class SmartSetupSummary
    {
        public SmartSetupSummary()
        {
            PackageIsPaid = false;
            EquipmentIsPaid = false;
            ServiceIsPaid = false;
            ServiceForMonths = 1;
        }

        public Customer Customer { set; get; }
        public PackageCustomer PackageCustomer { set; get; }
        public List<CustomerPackageService> CustomerPackageServiceList { set; get; }
        public List<CustomerPackageService> CustomerPackageOneTimeServiceList { set; get; }
        public List<CustomerPackageEqp> CustomerPackageEqpList { set; get; }
        public List<EmergencyContact> EmergencyContact { get; set; }
        public List<PaymentProfileCustomer> PaymentProfileCustomer { get; set; }

        #region Package
        public int PackagePaymentInfoId { set; get; }
        public bool PackageIsPaid { set; get; }
        public string PackageInvoice { set; get; }
        public string PackageComment { set; get; }
        #endregion
        #region Equipment
        public int EquipmentPaymentInfoId { set; get; }
        public bool EquipmentIsPaid { set; get; }
        public string EquipmentInvoice { set; get; }
        public string EquipmentComment { set; get; }
        public string DiscountType { get; set; }
        public double? DiscountAmount { get; set; }
        public double? Discountpercent { get; set; }



        #endregion
        #region Service
        public int ServicePaymentInfoId { set; get; }

        public int OneTimePaymentInfoId { set; get; }
        public bool ServiceIsPaid { set; get; }
        public int ServiceForMonths { set; get; }
        public string ServiceInvoice { set; get; }
        public string ServiceComment { set; get; }
        #endregion

        public int MMRPaymentInfoId { set; get; }

        public int NonConfirmingPaymentInfoId { set; get; }
        public bool NonConfirmingIsPaid { set; get; }
        public string NonConfirmingInvoice { set; get; }
        public bool IsNonConfirming { get; set; }
        public string ACHDiscountVal { get; set; }
    }
    public class SmartSummaryPayments
    {
        public Guid CustomerId { set; get; }
        public int AdvancedPaymentMonths { set; get; }
        public int PaymentInfoCustomerPackage { set; get; }
        public int PaymentInfoCustomerService { set; get; }
        public int PaymentInfoCustomerEquipment { set; get; }


        // "MAYUR" : for updatting discount in invoice while capture payment ::start : 
        public string PaymentInfoDiscountType { set; get; }
        public double PaymentInfoDiscountAmount { set; get; }
        public double PaymentInfoDiscountQty { set; get; }
        public double PaymentInfoDiscountPercentage { set; get; }
        public double PaymentInfoFinalAmount { set; get; }
        public double PaymentInfoTax { set; get; }

        // "MAYUR" : for updatting discount in invoice while capture payment ::start

    }

    public class TotalCalculatePayAmount
    {
        public double TotalAmount { get; set; }
        public double CollectToday { get; set; }
        public double CollectAch { get; set; }
        public double CollectCC { get; set; }
        public double CollectCash { get; set; }
        public double CollectCheck { get; set; }
        public string CurrentCurrency { get; set; }
    }
    public class CustomerAppointmentDetailWorkOrder
    {
        public string WorkOrderInstallType { get; set; }
        public string WorkOrderCollectedAmount { get; set; }
        public List<CustomerAppointmentDetail> CustomerAppointmentDetailList { set; get; }
        public CustomerAppointmentDetail CustomerAppointmentDetail { set; get; }

        public CustomerAppointment CustomerAppointment { get; set; }
    }

    public class CreatePdfWorkOrder
    {
        public CustomerAppointment CustomerAppointment { get; set; }
        public CustomerAppointmentEquipment CustomerAppointmentEquipment { get; set; }
        public List<CustomerAppointmentEquipment> CustomerAppointmentEquipmentList { set; get; }
        public string CompanyEmail { get; set; }
        public string CompanyName { get; set; }
        public bool CreatePdf { get; set; }
    }
    public class appointID
    {
        public CustomerAppointment CustomerAppointment { get; set; }
        public Guid appID { get; set; }
        public Guid custID { get; set; }
    }

    public class NewBillingOfSuppliers
    {
        public Supplier SupplierObject { get; set; }
        public SupplierBill SupplierBillObject { get; set; }
        public SupplierBillDetail SupplierBillDetailObject { get; set; }
    }
    public class NewBillingOfCustomer
    {
        public Customer CustomerObject { get; set; }
        public CustomerBill CustomerBillObject { get; set; }

    }
    public class LeadPackageEuipment
    {
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public Guid PackageEquipmentid { get; set; }
        public string EquipmentName { get; set; }
        public double EquipmentCost { get; set; }
        public string PackageEquipmentType { get; set; }
        public bool EquipmentIsFreeFlag { get; set; }
        public int PackageEqpId { get; set; }
        public int IsSelected { get; set; }
        public bool IsChecked { get; set; }
        public int NumOfEquipment { get; set; }
        public int NumofOptional { get; set; }
        public int OrderBy { get; set; }
    }
    public class LeadPackageModelEquipmentList
    {
        public List<LeadPackageEuipment> PackageDeviceEquipmentList { get; set; }
        public List<LeadPackageEuipment> PackageIncludeEquipmentList { get; set; }
        public List<LeadPackageEuipment> PackageOptionalEquipmentList { get; set; }
        public int PackageMaxDeviceEquipmentLimit { get; set; }
        public MMRRange PackageMMRRange { get; set; }
    }
    public class LeadSmartPackageModelEquipmentList
    {
        public List<SmartPackageEquipmentService> PackageDeviceEquipmentList { get; set; }
        public List<SmartPackageEquipmentService> PackageIncludeEquipmentList { get; set; }
        public List<SmartPackageEquipmentService> PackageOptionalEquipmentList { get; set; }
        public List<SmartPackageEquipmentService> PackageServiceList { get; set; }

        public List<CustomerPackageEqp> SelectedDeviceEqpList { set; get; }
        public List<CustomerPackageEqp> SelectedOptionalEqpList { set; get; }
        public List<CustomerPackageEqp> SelectedServiceEqpList { set; get; }

        public int PackageMaxDeviceEquipmentLimit { get; set; }
        public MMRRange PackageMMRRange { get; set; }
        public SmartPackage SmartPackageDetails { get; set; }
        public PackageCustomer PackageCustomerDetails { get; set; }
    }
    public class AddLeadPackage
    {
        public List<LeadSelectedEquipments> EquipmentList { get; set; }
        public List<CustomPackageDetailCustomerProducts> PackageCustomerEquipmentsList { get; set; }
        public Guid PackageId { get; set; }
        public int PackageIdInt { get; set; }
        public string InstallType { get; set; }
        public int SystemTypeId { get; set; }
        public int LeadId { get; set; }
    }
    public class AddSmartLeadPackage
    {
        public List<CustomerPackageService> ServiceList { get; set; }
        public List<LeadSelectedEquipments> EquipmentList { get; set; }
        public List<CustomPackageDetailCustomerProducts> PackageCustomerEquipmentsList { get; set; }
        public List<SmartPackageEquipmentServiceEquipment> SmartPackageEquipmentServiceEquipmentList { set; get; }
        public PackageCustomer PackageCustomerDetails { get; set; }
        public Guid PackageId { get; set; }
        public Guid ManufacturerId { get; set; }
        public int SmartInstallTypeId { get; set; }
        public int SmartSystemTypeId { get; set; }
        public int LeadId { get; set; }
        public Guid TicketId { get; set; }
    }
    public class LeadSelectedEquipments
    {
        public Guid SelectedEquipmentId { get; set; }
        public double SelectedEquipmentPrice { get; set; }
        public bool SelectedEquipmentIsFree { get; set; }
        public int NumOfEquipments { get; set; }
        public bool IsIncluded { get; set; }
        public bool IsDevice { get; set; }
        public bool IsOptionalEqp { get; set; }
    }
    public class CustomInitialLeadPackageModel
    {
        public string InstallType { get; set; }
        public int PackageSystemType { get; set; }
        public int PackageType { get; set; }
        public int ServiceType { get; set; }
        public CustomerSystemInfo CustomerSystemInfo { get; set; }
    }
    public class ListedPaymentInfo
    {
        public List<PaymentInfo> ListPayment { get; set; }
    }
    public class CustomPackageDetailCustomerProducts
    {
        public string Type { get; set; }
        public bool IsIncluded { get; set; }
        public int PackageEqpId { get; set; }
    }
    public class AddLeadAddedEquipments
    {
        public int EquipmentLeadId { get; set; }
        public List<LeadAddedEquipments> AddedEquipmetList { get; set; }
    }
    public class LeadAddedEquipments
    {
        public Guid EquipmentId { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
        public float TotalPrice { get; set; }
    }
    public class AddSmartLeadAddedEquipments
    {
        public int EquipmentLeadId { get; set; }
        public List<SmartLeadAddedEquipments> AddedEquipmetList { get; set; }
    }
    public class SmartLeadAddedEquipments
    {
        public int Id { get; set; }
        public Guid EquipmentId { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
        public float DiscountUnitPricce { get; set; }
        public float DiscountPckage { get; set; }
        public float Total { get; set; }
        public Guid ServiceId { set; get; }
        public bool IsPackageEqp { get; set; }
        public bool IsIncluded { get; set; }
        public bool IsDevice { get; set; }
        public bool IsOptionalEqp { set; get; }
    }
    public class QaAnswerListItem
    {
        public Guid LeadCustomerId { get; set; }
        public int Leadid { get; set; }
        public List<ListOfQaAnswers> QaAnswersList { get; set; }
    }
    public class ListOfQaAnswers
    {
        public int SelectedQuesid { get; set; }
        public string SelectedAnswer { get; set; }
    }

    public class ListQuestionAnswer
    {
        public List<QaAnswer> ListQa1Answer { get; set; }
        public List<QaQuestion> ListQa1Question { get; set; }
        public List<QaQuestion> ListQa2Question { get; set; }
        public List<QaAnswer> ListQa2Answer { get; set; }
        public List<QaQuestion> ListQaQuesNotQaAnswer { get; set; }
        public Customer QACustomer { get; set; }
    }
    public class CustomScheduleCaneldar
    {
        public string EventType { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public string EventAssignedEmployee { get; set; }
        public string EventCreator { get; set; }
        public string EventName { get; set; }
        public string inedtity { get; set; }
        public string EventCustomerId { get; set; }
        public int EventLeadId { get; set; }
    }

    public class LeadTechScheduleCalendar
    {
        public string EventStartTime { get; set; }
        public string EventEndTime { get; set; }
        public string EventTime { get; set; }
        public DateTime EventDate { get; set; }
        public string EventName { get; set; }
        public int EventId { get; set; }
        public int EventCustomer { get; set; }

    }

    public class PreScheduleCalendarList
    {
        public string EventTitle { get; set; }
        public string EventStartDate { get; set; }
        public string EventEndDate { get; set; }
        public string EventType { get; set; }
        public int EventLeadId { get; set; }
        public Guid EventCusId { get; set; }
        public string EventCustomId { get; set; }
        public Guid EventAppid { get; set; }
        public string EventPermissionName { get; set; }
        public string EventResourceName { get; set; }
        public bool EventIsCalendar { get; set; }
        public string EventColor { get; set; }
        public int EventId { get; set; }
        public string EventCustomerName { get; set; }
        public bool EventAllDay { get; set; }
    }
    public class PreCustomerNote
    {
        public string EventTitle { get; set; }
        public string EventStartDate { get; set; }
        public string EventEndDate { get; set; }
        public string EventType { get; set; }
        public int EventLeadId { get; set; }
        public string EventCusId { get; set; }
        public string EventCustomId { get; set; }
        public string EventAppid { get; set; }
        public string EventPermissionName { get; set; }
        public string EventResourceName { get; set; }
        public bool EventIsCalendar { get; set; }
        public string EventColor { get; set; }
        public int EventId { get; set; }
        public string EventCustomerName { get; set; }
        public bool EventAllDay { get; set; }
        public bool EventIsLead { get; set; }
        public string EventStreet { get; set; }
        public string EventLocate { get; set; }
        public string EventDate { get; set; }
        public string EventCalendarCount { get; set; }
        public string EventTicketId { get; set; }
        public string EventStatus { get; set; }
        public string HoverTitle { get; set; }
        public string EventDisplayType { get; set; }
        public string EventDBA { get; set; }
        public string EventBookingId { get; set; }
        public string EventAdditionalMember { get; set; }
        public int EventRescheduleId { get; set; }
        public string EventBusinessName { get; set; }
        public string EventLocationFlag { get; set; }
        public string EventLatitude { get; set; }
        public string EventLongitude { get; set; }
        public int EventCustomerIntId { get; set; }
        public string EventEmployeeColor { get; set; }
        public string EventStatusColor { get; set; }
    }

    public class EventCount
    {
        public int EventTotalCount { get; set; }
    }

    public class EventFilterCount
    {
        public int EventCounter { get; set; }
    }

    public class ScheduleCalendarList
    {
        public List<PreScheduleCalendarList> ListTechnicianSchedule { get; set; }
        public List<PreCustomerNote> ListFollowUpSchedule { get; set; }
        public EventCount EventCount { get; set; }
        public EventFilterCount EventFilterCount { get; set; }
        public List<Employee> ListEmployeeCalendar { get; set; }
        public List<Employee> ListUserIdHaveEvent { get; set; }
    }
    public class AllReturnsFilter
    {
        public string SearchText { set; get; }
        public int PageNo { set; get; }
        public int PageSize { set; get; }
        public string OrderBy { set; get; }
        public string PaymentType { set; get; }
        public Guid? CompanyId { set; get; }
        public string Order { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class AllCustomerFilter
    {
        public string SearchText { set; get; }
        public int PageNo { set; get; }
        public int PageSize { set; get; }
        public string OrderBy { set; get; }
        public Guid? CompanyId { set; get; }
        public string Status { get; set; }
        public string Order { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Paymentmethod { get; set; }

    }

    public class InvoiceSetting
    {
        public bool ServiceSetting { get; set; }
        public bool ShippingSetting { get; set; }
        public bool DiscountSetting { get; set; }
        public bool DepositSetting { get; set; }
        public bool VendorPriceSetting { get; set; }
        public bool ShowEstimateServiceSetting { get; set; }
        public bool ShowEstimateTaxSetting { get; set; }


    }

    public class EstimatorSetting
    {
        public bool ShippingSetting { get; set; }
        public bool DiscountSetting { get; set; }
        public bool DepositSetting { get; set; }
        public bool VendorPriceSetting { get; set; }
        public bool ShowServicePlan { get; set; }
        public bool ShowService { get; set; }
        public float ServicePlanRate { get; set; }
    }
    public class CreateCustomerInvoice
    {
        public Invoice Invoice { set; get; }
        public List<Invoice> InvoiceList { set; get; }
        public string CustomerName { set; get; }
        public string CustomerEmailAddress { set; get; }
        public string CustomerContactNumber { set; get; }
        public string CompanyEmail { set; get; }
        public string CompanyName { set; get; }
        public string CompanyLogo { set; get; }
        public string CompanyAddress { set; get; }
        public string InvoiceId { set; get; }
        public string CompanyPhone { get; set; }
        public Guid CustomerId { set; get; }
        public string ShortUrl { set; get; }
        public string EmailBody { set; get; }
        public string SMSBody { set; get; }
        public EmailTemplate EmailTemplate { get; set; }
    }

    public class CreateCustomerEstimator
    {
        public Estimator Estimator { set; get; }
        public List<Estimator> EstimatorList { set; get; }
        public string CustomerName { set; get; }
        public string CustomerEmailAddress { set; get; }
        public string CustomerContactNumber { set; get; }
        public string CompanyEmail { set; get; }
        public string CompanyName { set; get; }
        public string CompanyLogo { set; get; }
        public string CompanyAddress { set; get; }
        public string EstimatorId { set; get; }
        public string CompanyPhone { get; set; }
        public Guid CustomerId { set; get; }
        public string ShortUrl { set; get; }
        public string EmailBody { set; get; }
        public string SMSBody { set; get; }
        public EmailTemplate EmailTemplate { get; set; }
    }
    public class CreateCustomerRequisition
    {
        public CreateCustomerAppoinmentEquipment customerAppoinmentEquipment { set; get; }
        public List<CreateCustomerAppoinmentEquipment> customerAppoinmentEquipmentList { set; get; }
        public string CustomerName { set; get; }
        public string SendEmailAddress { set; get; }
        public string CcSendEmailAddress { get; set; }
        public string SendContactNumber { get; set; }
        public string CustomerContactNumber { set; get; }
        public string CompanyEmail { set; get; }
        public string CompanyName { set; get; }
        public string CompanyLogo { set; get; }
        public string CompanyAddress { set; get; }
        public string InvoiceId { set; get; }
        public string CompanyPhone { get; set; }
        public Guid CustomerId { set; get; }
        public string ShortUrl { set; get; }
        public string EmailBody { set; get; }
        public string SMSBody { set; get; }
        public EmailTemplate EmailTemplate { get; set; }
    }

    public class CreateVendorBill
    {
        public Bill Bill { set; get; }
        public List<BillDetail> BillDetailList { set; get; }
        public string File { set; get; }
        //public string EmailAddress { set; get; }
        public string CompanyEmail { set; get; }
        public string CompanyName { set; get; }
        public string CompanyAddress { set; get; }
        //public string CustomerName { set; get; }
        //public double SubTotal { set; get; }
        //public double Discount { set; get; }
        //  public InvoiceSetting InvoiceSetting { get; set; }
        //  public List<InvoiceNote> InvoiceNotes { set; get; }
        public string ExpenseType { get; set; }
    }
    public class CreateCustomerShareInfo
    {
        public Customer Customer { set; get; }

        public CustomerExtended CustomerExtended { set; get; }
        public string CustomerName { set; get; }
        public string CustomerEmailAddress { set; get; }
        public string CustomerContactNumber { set; get; }
        public string CompanyEmail { set; get; }
        public string CompanyName { set; get; }
        public string CompanyLogo { set; get; }
        public string CompanyAddress { set; get; }
        public string InvoiceId { set; get; }
        public string CompanyPhone { get; set; }

        public string CustomerAddress { get; set; }

        public Guid CustomerId { set; get; }
        public string ShortUrl { set; get; }
        public string EmailBody { set; get; }
        public string SMSBody { set; get; }
        public EmailTemplate EmailTemplate { get; set; }
    }
    public class CusEquipmentList
    {
        public string EquipmentName { get; set; }
        public double EquipmentQuantity { get; set; }
        public double EquipmentUnitPrice { get; set; }
        public double EquipmentTotalPrice { get; set; }
        public double YourPrice { get; set; }
    }

    public class TechCallAddEquipment
    {
        public Guid CustomerID { get; set; }
        public List<LeadAddedEquipments> AddedEquipmetList { get; set; }
    }
    public class CheckModel
    {
        public string Amount { set; get; }
        public string Name { set; get; }
        public string AmountInWord { set; get; }
        public DateTime? CheckDate { set; get; }
        public string Memo { set; get; }
    }
    public class InstallationAgreementModel
    {
        public string EstimatorId { get; set; }
        public double TotalPrice { get; set; }
        public double TotalEstimateServiceAmount { get; set; }
        public string AccountType { get; set; }
        public string ContractType { get; set; }
        public int CSIDNumber { get; set; }
        public string KazarLogo { get; set; }
        public string InstallDate { get; set; }
        public string Referredby { get; set; }
        public string Owner2ndPhone { get; set; }
        public string OwnerCellPhone { get; set; }
        public string DisplayName { get; set; }
        public string SocialSecurityNumber { get; set; }
        public string InitialStreet { get; set; }
        public string InitialCity { get; set; }
        public string InitialCountry { get; set; }
        public string InitialState { get; set; }
        public string InitialZip { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string BillingZip { get; set; }
        public string BillingCountry { get; set; }
        public string InstallTypeName { get; set; }
        public List<SmartPackageEquipmentService> SmartPackageEquipmentServiceList { set; get; }
        public double UpfrontAddOnTotal { set; get; }
        public double UpfrontAddOnTotalPromo { set; get; }
        public bool IsUpfrontPromo { set; get; }
        public double MonthlyServiceFeeTotal { set; get; }
        public bool IsServicePromo { get; set; }
        public double TotalMonthlyMonitoring { set; get; }
        public double ProratedAmout { get; set; }
        public double FinancedAmout { get; set; }
        public double MonthlyFinanceRate { get; set; }
        public double NewSubTotal { set; get; }
        public double TotalDueAtSigning { set; get; }
        public PaymentInfo PaymentDetails { set; get; }
        public CreateEstimator createEst { get; set; }
        public Employee userInfo { get; set; }
        public bool IsEstimator { get; set; }
        public string InvoiceDiagram { get; set; }
        public string CompanyName { set; get; }
        public string CompanySignature { get; set; }
        public string CompanySignatureDate { get; set; }
        public string CompanyId { set; get; }
        public string CurrentCurrency { set; get; }
        public string CompanyStreet { set; get; }
        public string CompanySate { set; get; }
        public string CompanyWebsite { set; get; }
        public string CompanyLogo { set; get; }
        public string OwnerName { set; get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string OwnerAddress { set; get; }
        public string BillingAddress { set; get; }
        public string InstallAddress { set; get; }
        public string OwnerEmail { set; get; }
        public string OwnerPhone { set; get; }
        public List<EmergencyContact> EmergencyContactList { set; get; }
        public List<Equipment> EquipmentList { set; get; }
        public List<InvoiceDetail> InvoiceList { get; set; }
        public Invoice inv { get; set; }
        public List<Equipment> ServiceList { set; get; }
        public double Subtotal { set; get; }
        public string FileId { get; set; }
        public string CusSignIP { get; set; }
        public double Tax { set; get; }
        public double TaxTotal { set; get; }
        public double Total { set; get; }
        public string SubscribedMonths { set; get; }
        public string SubscribedMonthsInWord { set; get; }
        public int RenewalMonths { set; get; }
        public double ActivationFee { get; set; }
        public double LabourFee { get; set; }
        public DateTime DateOfTransaction { get; set; }
        public string BusinessName { set; get; }
        public DateTime EffectiveDate { set; get; }
        public string MonthlyMonitoringFee { set; get; }
        public string ResidentialTechFirstHourCost { set; get; }
        public string CommercialTechFirstHourCost { set; get; }
        public string CustomerSignature { get; set; }
        public string CustomerSignatureStringDate { get; set; }
        public string ContractCreatedDateVal { get; set; }
        public DateTime CustomerSignatureStringDateVal { get; set; }
        public Nullable<DateTime> CustomerSignatureDate { get; set; }
        public List<CustomerAgreement> CustomerAgreement { get; set; }
        public CustomerAgreement SingleCustomerAgreement { get; set; }
        public string Password { get; set; }
        public int EContractId { get; set; }
        public List<AgreementAnswer> ListAgreementAnswer { get; set; }
        public string SalesRepresentative { get; set; }
        public double TotalPayments { get; set; }
        public string LeadSource { get; set; }
        public bool IsNonConfirming { get; set; }
        public double NonConfirmingFee { get; set; }
        public string InitialApt { get; set; }
        public string ListContactEmergency { get; set; }
        public string BillingStreet { get; set; }
        public string ListPaymentInfo { get; set; }
        public double ACHDiscountAmount { get; set; }
        public DateTime OriginalContactDate { get; set; }
        public string DoingBusinessAs { get; set; }
        public string DispalyName { get; set; }
        public string CompanyPhone { get; set; }
        public bool FirstPage { get; set; }
        public bool Commercial { get; set; }
        public bool Recreate { get; set; }
        public bool IsInvoice { get; set; }
        public string InvoiceId { get; set; }
        public List<Equipment> NotARBEnabledServiceList { get; set; }
        public double NotARBEnabledTotalPrice { get; set; }
        public double AdvanceServiceFeeTotal { get; set; }
    }
    public class DashboardModel
    {
        public int FirstMonthLeadCount { set; get; }
        public int LastMonthLeadCount { set; get; }

        public int FirstMonthOrderCount { set; get; }
        public int LastMonthOrderCount { set; get; }

        public double FirstMonthRevenueCount { set; get; }
        public double LastMonthRevenueCount { set; get; }

        public int FirstMonthAvgTicketCount { set; get; }
        public int LastMonthAvgTicketCount { set; get; }

        public int FirstMonthCustomerCount { set; get; }
        public int LastMonthCustomerCount { set; get; }

        public int FirstMonthOpportunitiesCount { set; get; }
        public int LastMonthOpportunitiesCount { set; get; }

        public int FirstMonthActivitiesCount { set; get; }
        public int LastMonthActivitiesCount { set; get; }

        public int EstimateCount { set; get; }
        public int InvoiceCount { set; get; }

        public double TotalPaid { set; get; }
        public double TotalRevenue { set; get; }

        public double MMRCount { set; get; }
        public double EstimateAmount { set; get; }

        public double InvoiceAmount { set; get; }
        public double UnpaidAmount { set; get; }

        public int UnpaidCount { set; get; }
        public int TotaltTransactions { set; get; }

        public string EmployeeTag { get; set; }
        public int CountMMR { get; set; }

        public int FirstMonthOrder { get; set; }
        public int LastMonthOrder { get; set; }
    }
    public class DashboardModelTechnician
    {
        public int OpenInstallationTicket { set; get; }
        public int OpenServiceTicket { set; get; }
        public int ClosedInstallationTicket { set; get; }
        public int ClosedServiceTicket { set; get; }
        public int OpenPickTicket { get; set; }
        public int ClosedPickTicket { get; set; }
        public int OpenDropTicket { get; set; }
        public int ClosedDropTicket { get; set; }
        public CustomerAppointmentEquipment CustomerAppointmentEquipment { get; set; }
        public DashboardBarModelTechnician DashboardBarModelTechnician { get; set; }
    }

    public class DashboardBarModelTechnician
    {
        public int TotalCommissionSC { set; get; }
        public int TotalCommissionTC { set; get; }
        public int TotalCommissionAC { set; get; }
        public int TotalCommissionFC { set; get; }
        public int TotalCommissionRC { get; set; }
        public int AllTotalCommission { get; set; }
        public int Total90GoBack { get; set; }
    }


    public class AllCommissionTechnician
    {
        public List<SalesCommission> SalesCommission { set; get; }
        public List<TechCommission> TechCommission { set; get; }
        public List<AddMemberCommission> AddMemberCommission { set; get; }
        public List<FollowUpCommission> FollowUpCommission { set; get; }
        public List<RescheduleCommission> RescheduleCommission { get; set; }
    }

    public class DashBoardReminderFollowUpsModel
    {
        public string Note { set; get; }
        public DateTime ReminderDate { set; get; }
        public DateTime ReminderEndDate { get; set; }
        public DateTime CreatedDate { set; get; }
        public string ReminderType { set; get; }
        public string CustomerName { set; get; }
        public bool UserType { get; set; }
        public bool Email { get; set; }
        public bool Phone { get; set; }
        public int Customerid { get; set; }
        public int noteid { get; set; }
        public string BusinessName { get; set; }
        public string AssignUser { get; set; }
    }

    #region DataBase Decentralization
    public class CompanyConneciton
    {
        public Guid CompanyId { set; get; }
        public string ConnectionString { set; get; }
        public string MasterPassword { set; get; }
        public string UserName { get; set; }
        public string CompanyName { get; set; }
    }

    #endregion


    #region Custom model for technician inventory 
    public class TechnicianInventoryCustomModel
    {
        public Employee EmployeeModel { get; set; }
        public List<TechnicianInventory> TechnicianInventoryListModel { get; set; }
    }
    #endregion

    public class LeadServiceSetupCustomModel
    {
        public Customer CustomerModel { get; set; }
        public PaymentInfo PaymentInfo { get; set; }
        public List<PaymentInfo> PaymentInfoList { set; get; }
        public Nullable<double> ActivationFee { get; set; }
        public Nullable<double> ServiceFee { get; set; }
        public Nullable<double> EquipmentFee { get; set; }
        public Nullable<double> SubTotalFee { get; set; }
        public Nullable<double> Tax { get; set; }
        public Nullable<double> TotalFee { get; set; }
    }
    public class SmartAgreementFinalizeModel
    {
        public LeadPackageDetail PackageCustomerModel { get; set; }
        public List<CustomerPackageEqp> CustomerPackageEqpModel { get; set; }
        public List<CustomerPackageService> CustomerPackageServiceModel { set; get; }
        public Customer CustomerModel { get; set; }
        public List<EmergencyContact> EmergencyContactModel { set; get; }
        public List<PaymentInfo> PaymentInfoModel { set; get; }
    }
    public class AllSalesInfoModel
    {
        public List<Transaction> TransactionList { set; get; }
        public int TotalOpenEstimates { set; get; }
        public double TotalOpenEstimatesAmount { set; get; }
        public double TotalRevenue { set; get; }
        public double AccountsReceivable { set; get; }
        public double TotalAmount { set; get; }
        public double TotalBalance { set; get; }
        public int TotalCount { set; get; }
        public int CustomerCount { set; get; }
        public string SearchBy { set; get; }
        public string SearchText { set; get; }
        public DateTime FromDate { get; set; }
        public DateTime Todate { get; set; }
        public string InvoiceStatus { set; get; }

        public double TotalBalanceByPage { get; set; }

        public double TotalAmountByPage { get; set; }

    }
    public class AllInvoicesModel
    {
        public List<Invoice> InvoiceList { set; get; }
        public double TotalAmount { set; get; }
        public double TotalBalance { set; get; }
        public int TotalCount { set; get; }
        public string SearchBy { set; get; }
        public string SearchText { set; get; }
        public string TransactionId { set; get; }
        public string BillingCycle { set; get; }
        public int BillingDay { set; get; }
        public string IsTax { set; get; }
        public string PaymentFilter { set; get; }
        public string InvoiceFor { set; get; }

        public int TotalCustomerAll { set; get; }
        public double TotalAmountAll { set; get; }
        public int MonthlyCustomer { set; get; }
        public double MonthlyAmount { set; get; }
        public int ReturnedCustomer { set; get; }
        public double ReturnedCustomerAmount { set; get; }
        public int CreditCardExpiringNumber { set; get; }
        public string Status { get; set; }
    }

    public class WorkOrderPdfDetail
    {
        public string WorkOrderEquipmentName { get; set; }
        public int WorkOrderQuantity { get; set; }
        public float WorkOrderUnitPrice { get; set; }
        public float WorkOrderTotalPrice { get; set; }
    }
    public class ServiceOrderPdfDetail
    {
        public string ServiceOrderEquipmentName { get; set; }
        public int ServiceOrderQuantity { get; set; }
        public float ServiceOrderUnitPrice { get; set; }
        public float ServiceOrderTotalPrice { get; set; }
    }
    public class DashboardSalesAreaChart
    {
        public Double TotalSaleAmount { set; get; }
        public DateTime SaleDate { set; get; }
        public int SaleQuantity { set; get; }
    }
    public class CustomerTabModel
    {
        public int CustomerId { set; get; }
        public string CustomerName { set; get; }
        public string CustomerGuid { set; get; }
    }
    public class ContactTabModel
    {
        public int ContactId { set; get; }
        public string ContactName { set; get; }
        public string ContactGuid { set; get; }
    }
    public class OpportunityTabModel
    {
        public int OpportunityId { set; get; }
        public string OpportunityName { set; get; }
        public string OpportunityGuid { set; get; }
    }
    public class DashboardServiceBoardListViewModel
    {
        public DateTime AppointmentDate { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerMiddleName { get; set; }
        public string CustomerLastName { get; set; }
        public string EmployeeName { get; set; }
        public string AppointmentTime { get; set; }
        public int CustomerIntId { get; set; }
        public Guid CustomerGuidId { get; set; }
        public Guid ServiceOrderId { get; set; }
    }
    public class DashboardInstallationListViewModel
    {
        public DateTime AppointmentDate { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerMiddleName { get; set; }
        public string CustomerLastName { get; set; }
        public string EmployeeName { get; set; }
        public string AppointmentTime { get; set; }
        public int CustomerIntId { get; set; }
        public Guid CustomerGuidId { get; set; }
        public Guid ServiceOrderId { get; set; }
    }

    public class VendorBillAmountPanel
    {
        public double VendorBillOverDue { get; set; }
        public double VendorBillOpen { get; set; }
        public double VendorBillPaid { get; set; }
    }
    public class VendorBillFilter
    {
        public int? UnitPerPage { get; set; }
        public int? PageNumber { get; set; }
        public string Order { get; set; }
        public string SearchText { get; set; }
        public string filterText { get; set; }
        public string ExpensedBy { get; set; }
        public string ExpenseType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string BillStatus { get; set; }
    }
    public class ExpenseListFilter
    {
        public int? UnitPerPage { get; set; }
        public int? PageNumber { get; set; }
        public string Order { get; set; }
        public string SearchText { get; set; }
        public string filterText { get; set; }
    }
    public class LeadPackageDetail
    {
        public string PackageName { get; set; }
        public string PackageInstallType { get; set; }
        public string PanelName { get; set; }
        public string ContactName { get; set; }
        public string ContactRelation { get; set; }
        public string ContactPhone { get; set; }
        public string ContactHaskey { get; set; }
        public string SmartSystemTypeName { get; set; }
    }

    public class LeadEmergencyDetail
    {
        public string ContactName { get; set; }
        public string ContactRelation { get; set; }
        public string ContactPhone { get; set; }
        public string ContactHaskey { get; set; }
    }

    public class LeadEquipmentDetail
    {
        public string LeadEquipmentName { get; set; }
        public string LeadEquipmentQuantity { get; set; }
        public string LeadEquipmentPrice { get; set; }
    }
    public class SalesARBInvoices
    {
        public List<ARBInvoiceModel> InvoiceList { set; get; }
        public List<ARBInvoiceDownLoadModel> ARBInvoiceDownLoadModelList { set; get; }


        public ARBSummary Summary { set; get; }

        public int? PageNo { set; get; }
        public int PageSize { set; get; }
        public string SearchText { set; get; }
        public string SearchBy { set; get; }
        public string SortBy { set; get; }
        public string SortOrder { set; get; }
        public string order { get; set; }
        public string InvoiceFor { set; get; }
        public string BillingCycle { set; get; }
        public DateTime InvStarDate { set; get; }
        public DateTime InvEndDate { set; get; }

        public double TotalAmountByPage { get; set; }
    }
    public class ARBInvoiceModel
    {
        public int Id { set; get; }

        public int InvId { get; set; }
        public string CustomerName { set; get; }
        public string BusinessName { set; get; }
        public string AuthorizeRefId { set; get; }
        public string InvoiceId { set; get; }
        public double TotalAmount { set; get; }
        public double BalanceDue { set; get; }
        public string Status { set; get; }
        public string TransactionId { set; get; }
        public string BillingCycle { set; get; }
        public DateTime CreatedDate { set; get; }
        public DateTime InvoiceDate { set; get; }
        public string Description { set; get; }
        public int CustomerIntId { get; set; }
        public Guid CustomerId { get; set; }
    }

    public class ARBInvoiceDownLoadModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { set; get; }
        public string InvoiceId { set; get; }
        public double Billed_Amount { set; get; }
        public string Description { set; get; }
        public DateTime SettlementDate { set; get; }
        public string TransactionId { set; get; }
        public string Status { set; get; }


        //public int Id { set; get; }

        //public string BusinessName { set; get; }
        //public string AuthorizeRefId { set; get; }


        //public double BalanceDue { set; get; }


        //public string BillingCycle { set; get; }

        //public DateTime InvoiceDate { set; get; }


    }

    public class ARBSummary
    {
        public int TotalCount { set; get; }
        public int TotalCustomer { set; get; }
        public double TotalMMR { set; get; }
        public int MonthlyCustomer { set; get; }
        public double MonthlyMMR { set; get; }
        public int InActiveCustomer { set; get; }
        public double InActiveCustomerMMR { set; get; }
        public string InvoiceIdList { get; set; }

    }

    public class AllInvoicesFilter
    {
        public Guid CompanyId { set; get; }
        public int? PageNo { set; get; }
        public int PageSize { set; get; }
        public string InvoiceFor { set; get; }
        public string SearchText { set; get; }
        public string SearchBy { set; get; }
        public string BillingCycle { set; get; }
        public int? BillingDay { set; get; }
        public string IsTax { set; get; }
        public string PaymentFilter { set; get; }
        public string SortBy { set; get; }
        public string SortOrder { set; get; }
        public string Status { set; get; }
        public string order { get; set; }


        public string InvoiceStartDate
        {
            get { return _StrStartDate; }
            set
            {
                _StrStartDate = value;
                this.StartDate = value.ToDateTime();
            }
        }
        public string InvoiceEndDate
        {
            get { return _StrEndDate; }
            set
            {
                _StrEndDate = value;
                this.EndDate = value.ToDateTime();
            }
        }
        public string StrSearchWeek { set; get; }

        private string _StrStartDate { set; get; }
        private string _StrEndDate { set; get; }

        public DateTime StartDate { set; get; }
        public DateTime EndDate { get; set; }
    }
    public class AchReturn
    {
        public DateTime StartDate { set; get; }
        public DateTime EndDate { get; set; }
    }
    public class ExpenseSummary
    {
        public double OpenBill { set; get; }
        public double Paid { set; get; }
        public double OverDue { set; get; }
    }
    public class PayrollReport
    {
        public string EmpName { set; get; }
        public Guid UserId { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public double TotalCommission { set; get; }
        public double RegularHours { set; get; }
        public double HourlyRate { set; get; }


    }
    public class SaleCommisionReport
    {
        public string CustomerName { set; get; }
        public int TicketId { set; get; }
        public string UserAssigned { set; get; }
        public DateTime CompletionDate { get; set; }
        public double TotalRMR { set; get; }
        public double TotalCollected { set; get; }
        public double TotalCommission { set; get; }
    }
    public class TechCommisionReport
    {
        public string CustomerName { set; get; }
        public int TicketIdValue { set; get; }
        public string Technician { set; get; }
        public DateTime CompletionDate { get; set; }
        public double TotalRMR { set; get; }
        public double TotalCollected { set; get; }
        public double TotalCommission { set; get; }

    }
    public class EmployeePayrollReport
    {
        public List<PayrollReport> PayrollReportList { get; set; }

        public PayrollTotalCount PayrollTotalCount { get; set; }
    }

    public class EmployeeJobsReport
    {
        public List<Booking> JobReportList { get; set; }
        public TotalOnlineJobsCount TotalOnlineJobsCount { get; set; }
        public TotalSystemJobsCount TotalSystemJobsCount { get; set; }
        public PayrollTotalCount PayrollTotalCount { get; set; }
        public TotalCost TotalAmount { get; set; }
    }
    public class TotalOnlineJobsCount
    {
        public int TotalOnlineBooking { get; set; }
    }
    public class TotalSystemJobsCount
    {
        public int TotalSystemBooking { get; set; }
    }
    public class EmployeeConversionReport
    {
        public List<Customer> ConversionReportList { get; set; }

        public PayrollTotalCount PayrollTotalCount { get; set; }
    }
    public class SalesPay
    {
        public int Id { get; set; }
        public int CustomerIntId { get; set; }
        public int AccountId { get; set; }
        public string CustomerName { get; set; }
        public string BrinksFundingStatus { get; set; }
        public string FinanceFundingStatus { get; set; }
        public string PaymentMethodForRMR { get; set; }
        public double CreditScoreValue { get; set; }
        public string CreditGrade { get; set; }
        public string Type { get; set; }
        public string ContractTerm { get; set; }
        public string PayType { get; set; }
        public int SalesPersonIntId { get; set; }
        public Guid SalesPersonId { get; set; }
        public string SalesPerson { get; set; }
        public int TicketIdInt { get; set; }
        public double TotalRMR { get; set; }
        public double FinanceMonthlyPayment { get; set; }
        public double TotalMonthly { get; set; }
        public double TotalMultiple { get; set; }
        public double GrossPay { get; set; }
        public double HoldBack { get; set; }
        public double Deductions { get; set; }
        public double Adjustment { get; set; }
        public double NetPay { get; set; }
        public string TermSheetName { get; set; }
        public Guid TermSheetId { get; set; }
        public double PassThrus { get; set; }
        public string FundingStatus { get; set; }
        public string EquipmentList { get; set; }
    }
    public class TotalPayrollSum
    {
        public double TotalAdjustment { get; set; }
        public double TotalCommission { get; set; }
        public double TotalPoint { get; set; }
        public double TotalCommissionablePoints { get; set; }
        public double TotalUnpaidBalance { get; set; }
        public double TotalOverage { get; set; }
        public double TotalSalesCommission { get; set; }
        public double TotalTechCommission { get; set; }
        public double TotalAddCommission { get; set; }
        public double TotalFinRepCommission { get; set; }
        public double TotalCallCommission { get; set; }
        public double TotalFollowUpCommission { get; set; }
        public double TotalRescheduleCommission { get; set; }
    }
    public class EmpSalesPayReport
    {
        public string SalesPersonName { get; set; }
        public Guid SalesPersonId { get; set; }
        public List<SalesPay> SalesPayList { get; set; }
        public PayrollTotalCount PayrollTotalCount { get; set; }
        public double TotalTotalRMR { get; set; }
        public double TotalGrossPay { get; set; }
        public double TotalHoldBack { get; set; }
        public double TotalPassThru { get; set; }
        public double TotalDeductions { get; set; }
        public double TotalAdjustments { get; set; }
        public double TotalNetPay { get; set; }
    }
    public partial class NewSalesCustomer
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string TicketType { get; set; }
        public string CustomerNo { get; set; }
        public string SalesPerson { get; set; }
        public string Installer { get; set; }
        public string LeadSource { get; set; }
        public string SalesLocation { get; set; }
        public string Type { get; set; }
        public string LeadStatus { get; set; }
        public double ActNonFee { get; set; }
        public double RMR { get; set; }
        public double CollectedRMR { get; set; }
        public double EquipmentFee { get; set; }
        public double DiscAmt { get; set; }

        public double ServiceFee { get; set; }
        public double AdvancedMonitoring { get; set; }
        public double TotalTax { get; set; }
        public double FinancedAmount { get; set; }
        public double TotalWoTax { get; set; }
        public double TotalSales { get; set; }
        public DateTime SalesDate { get; set; }
    }
    public class NewSalesCustomerModel
    {
        public List<NewSalesCustomer> NewSalesCustomer { get; set; }
        public int Totalcount { get; set; }
        public int pageno { get; set; }
        public int pagesize { get; set; }
        public double TotalActNonFee { get; set; }
        public double TotalRMR { get; set; }
        public double TotalEquipmentFee { get; set; }
        public double TotalDiscEquipFee { get; set; }
        public double TotalServiceFee { get; set; }
        public double TotalAdvancedMonitoring { get; set; }
        public double TotalTotalTax { get; set; }
        public double TotalWoTax { get; set; }
        public double FinancedAmount { get; set; }
        public double TotalSales { get; set; }
        public double TotalCollectedRMR { get; set; }


        public int CustomerCount { get; set; }
        public double SumActNonFee { get; set; }
        public double SumRMRTotal { get; set; }
        public double SumEquipmentTotal { get; set; }
        public double SumServiceFeeTotal { get; set; }
        public double SumAdvanceMonitoringTotal { get; set; }

        public double SumTotalwoTax { get; set; }

        public double SumTotalTax { get; set; }
        public double SumTotalSales { get; set; }

        public double SumFinancedAmount { get; set; }




    }
    public class ServiceSalesModel
    {
        public List<ServiceSales> serviceSalesList { get; set; }
        public int Totalcount { get; set; }
        public int SmartTotal { get; set; }
        public int StandardTotal { get; set; }
        public int LandLineTotal { get; set; }
        public int CellularTotal { get; set; }
        public int GSPTotal { get; set; }
        public int PSPTotal { get; set; }
    }
    public class ServiceSales
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int Smart { get; set; }
        public int Standard { get; set; }
        public int LandLine { get; set; }
        public int Cellular { get; set; }
        public int GSP { get; set; }
        public int PSP { get; set; }


    }
    public partial class RMRAudit
    {
        public int Id { get; set; }
        public string Ownership { get; set; }
        public string Branch { get; set; }
        public string OldCustomerId { get; set; }
        public string CustomerNo { get; set; }
        public string DisplayName { get; set; }
        public string Address { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string StreetPrevious { get; set; }
        public string CityPrevious { get; set; }
        public string StatePrevious { get; set; }
        public string ZipCodePrevious { get; set; }
        public string AddressPrevious { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public double BalanceDue { get; set; }
        public DateTime InstallDate { get; set; }
        public string BusinessAccountType { get; set; }
        public string Item { get; set; }
        public double RMR { get; set; }
        public string BillCycle { get; set; }
        public DateTime LastBillDate { get; set; }
        public DateTime NextBillDate { get; set; }
        public DateTime StartBillDate { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime WarrantyStartDate { get; set; }
        public int InitialTerm { get; set; }
        public int RenewalTerm { get; set; }
        public string Panel { get; set; }
        public string AutoBank { get; set; }
        public string Description { get; set; }
        public string BankProcessDay { get; set; }
        public string BillType { get; set; }
        public string AutoCC { get; set; }
        public string ExpiresOn { get; set; }

    }
    public class RMRAuditModel
    {
        public List<RMRAudit> RMRAudit { get; set; }
        public int Totalcount { get; set; }
        public int TotalRMR { get; set; }
        public int pageno { get; set; }
        public int pagesize { get; set; }
        public double CustomerTotalRMR { get; set; }
        public int CustomerTotalCount { get; set; }
    }
    public partial class VariableCostCustomer
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string TicketType { get; set; }
        public string CustomerNo { get; set; }
        public string SalesPerson { get; set; }
        public string Installer { get; set; }
        public string LeadSource { get; set; }
        public string SalesLocation { get; set; }
        public string Type { get; set; }
        public string LeadStatus { get; set; }
        public double RMR { get; set; }
        public double Revenue { get; set; }
        public double EquipVendorCost { get; set; }
        public double Labor { get; set; }
        public double Comm { get; set; }
        public double TtlCost { get; set; }
        public double Net { get; set; }
        public double CrMult { get; set; }
        public double MISC { get; set; }
        public DateTime SalesDate { get; set; }
    }
    public class VariableCostCustomerModel
    {
        public List<VariableCostCustomer> VariableCostCustomer { get; set; }
        public int Totalcount { get; set; }
        public int pageno { get; set; }
        public int pagesize { get; set; }
        public double TotalRMR { get; set; }
        public double TotalRevenue { get; set; }
        public double TotalEquipVendorCost { get; set; }
        public double TotalLabor { get; set; }
        public double TotalComm { get; set; }
        public double TotalMiscExp { get; set; }
        public double TotalTtlCost { get; set; }
        public double TotalNet { get; set; }
        public double TotalCrMult { get; set; }
        public int SumCustomer { get; set; }
        public double SumRMR { get; set; }
        public double SumRevenue { get; set; }
        public double SumEquipCost { get; set; }
        public double SumLabor { get; set; }
        public double SumCommission { get; set; }
        public double SumMiscExp { get; set; }
        public double SumTotalCost { get; set; }
        public double SumAvgCost { get; set; }
        public double SumNet { get; set; }
        public double SumCrMul { get; set; }
    }
    public partial class TechUpSales
    {
        public int Id { get; set; }
        public string TechName { get; set; }
        public double RMR { get; set; }
        public double Commission { get; set; }
        public int EquipmentQty { get; set; }
        public double EquipmentValue { get; set; }
        public double EquipmentCommission { get; set; }
    }
    public class TechUpSalesModel
    {
        public List<TechUpSales> TechUpSales { get; set; }
        public int Totalcount { get; set; }
        public int pageno { get; set; }
        public int pagesize { get; set; }
        public double TotalRMR { get; set; }
        public double TotalCommission { get; set; }
        public double TotalEquipmentQty { get; set; }
        public double TotalEquipmentValue { get; set; }
        public double TotalEquipmentCommission { get; set; }
    }
    public partial class InsideCommission
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime PayrollDate { get; set; }
        public int Batch { get; set; }
        public string Rep { get; set; }
        public string DisplayName { get; set; }
        public string CustomerName { get; set; }
        public int CustomerIntId { get; set; }
        public double RMR { get; set; }
        public double Activation { get; set; }
        public double Equipment { get; set; }
        public double Total { get; set; }
        public double Comm { get; set; }
        public string FinanceRep { get; set; }
        public double FinanceRepComm { get; set; }
    }
    public class InsideCommissionModel
    {
        public List<InsideCommission> InsideCommission { get; set; }
        public int Totalcount { get; set; }
        public int pageno { get; set; }
        public int pagesize { get; set; }
        public double TotalRMR { get; set; }
        public double TotalActivation { get; set; }
        public double TotalEquipment { get; set; }
        public double TotalTotal { get; set; }
        public double TotalComm { get; set; }
        public double TotalFinanceRepComm { get; set; }
        public double SumCustomer { get; set; }
        public double SumRMR { get; set; }
        public double SumActivation { get; set; }
        public double SumEquipmentFee { get; set; }
        public double SumTotal { get; set; }
        public double SumComm { get; set; }
        public double SumFinRepCommission { get; set; }
    }
    public class EmpSaleCommisionReport
    {
        public List<SalesCommission> PayrollReportList { get; set; }
        public TotalPayrollSum TotalSalesCount { get; set; }
        public PayrollTotalCount PayrollTotalCount { get; set; }
    }
    public class EmpTicketReport
    {
        public List<Ticket> TicketReportList { get; set; }

        public PayrollTotalCount TicketTotalCount { get; set; }
    }
    public class EmpTechCommisionReport
    {
        public List<TechCommission> PayrollReportList { get; set; }
        public TotalPayrollSum TotalTechCount { get; set; }
        public PayrollTotalCount PayrollTotalCount { get; set; }
    }
    public class MemberCommisionReport
    {
        public List<AddMemberCommission> PayrollReportList { get; set; }
        public TotalPayrollSum TotalMemberCommisionCount { get; set; }
        public PayrollTotalCount PayrollTotalCount { get; set; }
    }
    public class FundedCommisionCluster
    {
        public int TicketId { get; set; }
        public string CustomerName { get; set; }
        public DateTime CompletionDate { get; set; }
        public double SalesTotalCommission { get; set; }
        public double TechTotalCommission { get; set; }
        public double AddCommission { get; set; }
        public double FinRepCommission { get; set; }
        public double CallCommission { get; set; }
        public double FollowUpCommission { get; set; }
        public double RescheduleCommission { get; set; }
        public string TicketType { get; set; }
        public double BalanceDue { get; set; }
    }
    public class FundedCommision
    {
        public int TicketId { get; set; }
        public int Id { get; set; }
        public int CustomerIdValue { get; set; }
        public string CustomerName { get; set; }
        public int SalesId { get; set; }
        public int TechId { get; set; }
        public int AddMemberId { get; set; }
        public int FinRepId { get; set; }
        public int ServiceCallId { get; set; }
        public int FollowUpId { get; set; }
        public int RescheduleId { get; set; }
        public double Adjustment { get; set; }
        public string Batch { get; set; }
        public bool? IsPaid { get; set; }
        public double SalesTotalCommission { get; set; }
        public double TechTotalCommission { get; set; }
        public double AddCommission { get; set; }
        public double FinRepCommission { get; set; }
        public double CallCommission { get; set; }
        public double FollowUpCommission { get; set; }
        public double RescheduleCommission { get; set; }
        public DateTime CompletionDate { get; set; }
        public string Technician { get; set; }

        public string Type { get; set; }
        public DateTime PaidDate { get; set; }
        public DateTime PayrollDate { get; set; }
        public double BalanceDue { get; set; }
        public string TicketType { get; set; }
        public double OriginalPoint { get; set; }

    }
    public class TotalAmount
    {
        public double Amount { get; set; }
    }
    public class ServiceCallCommisionReport
    {
        public List<ServiceCallCommission> PayrollReportList { get; set; }
        public TotalPayrollSum TotalServiceCallCount { get; set; }
        public PayrollTotalCount PayrollTotalCount { get; set; }
    }
    public class FollowUpCommisionReport
    {
        public List<FollowUpCommission> PayrollReportList { get; set; }
        public TotalPayrollSum TotalFollowUp { get; set; }
        public PayrollTotalCount PayrollTotalCount { get; set; }
    }
    public class RescheduleCommisionReport
    {
        public List<RescheduleCommission> PayrollReportList { get; set; }
        public TotalPayrollSum TotalRescheduleCommision { get; set; }
        public PayrollTotalCount PayrollTotalCount { get; set; }
    }
    public class FundedCommisionReportCluster
    {
        public List<FundedCommisionCluster> PayrollReportCluster { get; set; }
        public PayrollTotalCount PayrollTotalCount { get; set; }
    }
    public class FundedCommisionReport
    {
        public List<FundedCommision> PayrollReportList { get; set; }
        public TotalPayrollSum TotalFundedCommision { get; set; }
        public PayrollTotalCount PayrollTotalCount { get; set; }
    }
    public class AdjustmentFundingReport
    {
        public List<AdjustmentFunding> PayrollReportList { get; set; }
        public TotalAmount TotalAmount { get; set; }
        public PayrollTotalCount PayrollTotalCount { get; set; }
    }
    public class FinalCustomerSetupData
    {
        public List<PackageCustomer> ListPackageCustomer { get; set; }
        public List<CustomerAppointmentEquipment> ListCustomerAppointmentEquipment { get; set; }
        public List<PaymentInfoCustomer> ListPaymentInfoCustomer { get; set; }
        public List<EmergencyContact> ListEmergencyContact { get; set; }
    }

    public class ListAgreementAnswer
    {
        public int SelectedQues { get; set; }
        public string SelectedAns { get; set; }
    }
    public class InvoiceSessionModel
    {
        public string InvoiceId { set; get; }
        public string FileName { set; get; }
        public int FileID { set; get; }

    }

    public class EstimatorSessionModel
    {
        public string EstimatorId { set; get; }
        public string FileName { set; get; }
    }
    public class RequisitionSessionModel
    {
        public string UserId { set; get; }
        public string FileName { set; get; }
    }
    public class MailCorrespondenceData
    {
        public List<LeadCorrespondence> MailCorrespondenceList { get; set; }
        public List<LeadCorrespondence> MailEmployeeList { get; set; }
    }
    public class InvoiceEmailSentResponse
    {
        public string FileLocation { set; get; }
        public bool IsSent { set; get; }
    }
    public class EstimatorEmailSentResponse
    {
        public string FileLocation { set; get; }
        public bool IsSent { set; get; }
    }
    public class CustomerSystemNumberWithModel
    {
        public List<CustomerSystemNo> ListCustomerSystemNo { get; set; }
        public TotalCustomerSystemNoCount TotalCustomerSystemNoCount { get; set; }
    }
    public class TotalCustomerSystemNoCount
    {
        public int Counter { get; set; }
        public int TotalAmount { get; set; }
    }
    public class RequestAdmin
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class PayrollFilterModel
    {
        public PayrollFilterModel()
        {
            this.StartDate = new DateTime();
            this.EndDate = new DateTime();
        }
        public bool? IsPaid { set; get; }
        public string PtoStatus { get; set; }
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
        public string Start { get; set; }
        public string End { get; set; }
        public string BookingSource { get; set; }
        public int? PageNo { set; get; }
        public int? PageSize { set; get; }
        public string FilterText { get; set; }
        public string UserGroup { get; set; }
        public List<string> TicketType { get; set; }
        public int TicketId { get; set; }
        public string StrStartDate
        {
            get
            {
                return _StrStartDate;
            }
            set
            {
                _StrStartDate = value;
                this.StartDate = value.ToDateTime();
            }
        }
        public string StrEndDate
        {
            get
            {
                return _StrEndDate;
            }
            set
            {
                _StrEndDate = value;
                this.EndDate = value.ToDateTime();
            }
        }
        public string StrSearchWeek { set; get; }

        private string _StrStartDate { set; get; }
        private string _StrEndDate { set; get; }
        public string order { get; set; }
        public int pageno1 { get; set; }
        public int pagesize1 { get; set; }
        public string SearchText { get; set; }
        public string SalesPerson { get; set; }
        public string TechPersonList { get; set; }
        public string MemberPersonList { get; set; }
        public string ServicePersonList { get; set; }
        public string FollowUpPersonList { get; set; }
        public string RescheduleTech { get; set; }

        public Guid UserId { get; set; }
        public bool GetReport { get; set; }
        public string CurrentEmployee { get; set; }
        public string PayrollBrinksStatus { get; set; }
        public string PayrollBrinksFunding { get; set; }
    }

    public class LeadFormGeneration
    {
        public Customer Customer { get; set; }
        public List<FormGenerator> ListFormGenerator { get; set; }
    }

    public class AutoClockOutModel
    {
        public string EmpName { set; get; }
        public Guid UserId { set; get; }
        public string ClockedInOutStatus { set; get; }
        public DateTime ClockedInOutTime { set; get; }

    }
    public class LocalizeFilterModel
    {
        public Guid CompanyId { set; get; }
        public int LangId { set; get; }
        public string SearchText { set; get; }
        public int PageNo { set; get; }
        public int PageSize { set; get; }
    }
    public class LeadTabCountModel
    {
        public LeadTabCount LeadTabCount { get; set; }
        public LeadTabEstimateCount LeadTabEstimateCount { get; set; }
        public LeadTabThisMonthCount LeadTabThisMonthCount { get; set; }
        public LeadTabLastMonthCount LeadTabLastMonthCount { get; set; }
        public LeadTabEstimateAmount LeadTabEstimateAmount { get; set; }
        public LeadTabBookingCount LeadTabBookingCount { set; get; }
        public LeadTabBookingAmount LeadTabBookingAmount { set; get; }
    }

    public class LeadTabCount
    {
        public int LeadCount { get; set; }
    }
    public class LeadTabEstimateCount
    {
        public int EstimateCount { get; set; }
    }
    public class LeadTabBookingCount
    {
        public int BookingCount { get; set; }
    }
    public class LeadTabThisMonthCount
    {
        public int LeadThisMonthCount { get; set; }
    }
    public class LeadTabLastMonthCount
    {
        public int LeadLastMonthCount { get; set; }
    }
    public class LeadTabEstimateAmount
    {
        public double LeadEstimateAmount { get; set; }
    }
    public class LeadTabBookingAmount
    {
        public double LeadBookingAmount { get; set; }
    }

    public class TicketCounter
    {
        public int TicketAppCounter { get; set; }
    }

    public class TicketCounter1
    {
        public int TicketAppCounter1 { get; set; }
    }

    public class TicketCounterUser
    {
        public int TicketUserCounter { get; set; }
    }

    public class ReminderCounter
    {
        public int ReminderCount { get; set; }
    }
    public class CustomerChangeField
    {
        public Customer Customer { get; set; }
        public CustomerDraft CustomerDraft { get; set; }
        public CustomerSystemInfo customerSysinfo { get; set; }
        public CustomerSystemInfoDraft customerSysinfoDraft { get; set; }
        public EmergencyContact emContact { get; set; }
        public EmergencyContactDraft emContactDraft { get; set; }
    }

    public class LeadDetailTabCountModel
    {
        public Invoice LeadEstimate { get; set; }
        public CustomerNote LeadNote { get; set; }
        public LeadCorrespondence LeadCorrespondence { get; set; }
        public CustomerFile LeadFile { get; set; }
        public Booking Booking { get; set; }
        public int EstimatorCount { get; set; }
        public int TicketCount { set; get; }

        public int LogCount { set; get; }

    }

    public class CustomerLeadGridModel
    {
        public int CustomerGridId { get; set; }
        public int LeadGridId { get; set; }
        public Guid ComId { get; set; }
        public string SColumn { get; set; }
        public string CustomerColumnGroup { get; set; }
        public string LeadColumnGroup { get; set; }
        public int CustomerGroupOrder { get; set; }
        public int LeadGroupOrder { get; set; }
        public int ByOrder { get; set; }
        public bool ActivateColumn { get; set; }
        public bool CustomerGridActive { get; set; }
        public bool LeadGridActive { get; set; }
        public bool CustomerFormActive { get; set; }
        public bool LeadFormActive { get; set; }
        public string CustomerKey { get; set; }
        public string LeadKey { get; set; }
        public bool IsCustomerFilter { get; set; }
        public bool IsLeadFilter { get; set; }
        public bool IsCustomerRequired { get; set; }
        public bool IsLeadRequired { get; set; }
        public bool IsCustomerLabel { get; set; }
        public bool IsLeadLabel { get; set; }
    }

    public class RMRTagCustomModel
    {
        public string value { get; set; }
        public string label { get; set; }
    }

    public class CustomerLeadDetailGridModel
    {
        public int CustomerDetailId { get; set; }
        public int LeadDetailId { get; set; }
        public string DetailKey { get; set; }
        public string CustomerDetailColumn { get; set; }
        public string LeadDetailColumn { get; set; }
        public int CustomerDetailOrder { get; set; }
        public int LeadDetailOrder { get; set; }
        public bool DetailActive { get; set; }
        public bool CustomerDetailForm { get; set; }
        public bool LeadDetailForm { get; set; }
    }

    public class LeadReportServiceAndEquipmentModel
    {
        public List<CustomerPackageService> ListCustomerPackageService { get; set; }
        public List<CustomerPackageEqp> ListCustomerPackageEqp { get; set; }
        public Customer Customer { get; set; }
    }
    public class DelinquentTestCustomerModel
    {
        public int Id { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public double Unpaid { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string StreetType { get; set; }
        public string Appartment { get; set; }
        public bool UnlinkCustomer { get; set; }

        public int TransferCustomerId { get; set; }

        public string OldCustomer { get; set; }


    }
    public class DelinquentCustomerModel
    {
        public int Id { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public double Unpaid { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string StreetType { get; set; }
        public string Appartment { get; set; }
        public bool UnlinkCustomer { get; set; }

        public int TransferCustomerId { get; set; }

        public string OldCustomer { get; set; }


    }
    public class UnpaiAmount
    {
        public double TotalUnpaid { get; set; }
    }
    public class DelinquentReportModel
    {
        public List<DelinquentCustomerModel> DelinquentCustomerModel { get; set; }
        public UnpaiAmount UnpaidAmount { get; set; }
        public int ToatalCustomer { get; set; }
    }
    public class GoBackTicketModel
    {
        public int TikId { get; set; }
        public int CountTicket { get; set; }
        public int CusId { get; set; }
        public string CustomerName { get; set; }
        public string TikType { get; set; }
        public string AssignPerson { get; set; }
        public DateTime ScheduleOn { get; set; }
        public string TikStatus { get; set; }
    }

    public class SalesReportModel
    {
        public List<Customer> ListCustomer { get; set; }
        public SalesReportCountModel SalesReportCountModel { get; set; }
        public TotalSalesAmountModel TotalSalesAmountModel { get; set; }
    }

    public class TotalInvoiceAmount
    {
        public double TotalAmount { get; set; }
        public double TotalTax { get; set; }
        public double TotalInvoicesAmount { get; set; }
        public double TotalOpenBalance { get; set; }
        public double TotalCollected { get; set; }
    }

    public class InvoiceReportModel
    {
        public List<Invoice> ListInvoice { get; set; }
        public SalesReportCountModel InvoiceReportCountModel { get; set; }
        public TotalSalesAmountModel TotalInvoiceAmountModel { get; set; }
        public TotalInvoiceAmount TotalInvoiceAmount { get; set; }
    }

    public class BookingSalesReportModel
    {
        public Invoice Invoice { get; set; }
        public List<Ticket> ListTicket { get; set; }
        public Customer Customer { get; set; }
        public List<Booking> ListBooking { get; set; }
        public List<BookingDetails> ListBookingDetails { get; set; }
        public SalesReportCountModel SalesReportCountModel { get; set; }

    }

    public class BookingReportModel
    {
        public int Id { get; set; }
        public int CustomerIntId { get; set; }
        public Guid CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PrimaryPhone { get; set; }
        public string SecondaryPhone { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        public int BookingIntId { get; set; }
        public string BookingId { get; set; }
        public double TotalAmount { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string RugType { get; set; }
        public double Length { get; set; }
        public double LengthInch { get; set; }
        public double Width { get; set; }
        public double WidthInch { get; set; }
        public double Radius { get; set; }
        public double RadiusInch { get; set; }

        public int TicketId { get; set; }
        public string TicketType { get; set; }


        public int InvoiceIntId { get; set; }
        public string InvoiceId { get; set; }
        public string PaymentMethod { get; set; }
        public string BookingSource { get; set; }
        public double BalanceDue { get; set; }
        public double AmountPaid { get; set; }
        public double InTotalAmount { get; set; }
        public SalesReportCountModel SalesReportCountModel { get; set; }
    }


    public class SalesReportCountModel
    {
        public int TotalCount { get; set; }
    }

    public class TotalSalesAmountModel
    {
        public double TotalSalesAmount { get; set; }
        public double TotalDueAmount { get; set; }

        public double AveSaleswoTax { get; set; }

        public double TotalTax { get; set; }

        public double SalesAfterTax { get; set; }
        public double AveSaleswTax { get; set; }
        public double TotalPaid { get; set; }
        public double TotalUnpaid { get; set; }

        public int CustomerCount { get; set; }

        public int InvoiceCount { get; set; }

        public double TotalAmount { get; set; }

        public double AveInvoiceAmount { get; set; }



    }


    public class PartnerReportBarModel
    {
        public int CustomerCount { get; set; }
        public int LeadCount { get; set; }
        public double TotalRevanew { get; set; }
        public double MonthlyMonitoringFee { get; set; }
        public int ScheduledCount { get; set; }
        public int InstalledCount { get; set; }
    }
    public class LeadSourceReportBarModel
    {
        public int CustomerCount { get; set; }
        public int LeadCount { get; set; }
        public double TotalRevanew { get; set; }
        public double MonthlyMonitoringFee { get; set; }
        public int ScheduledCount { get; set; }
        public int InstalledCount { get; set; }
    }





    public class RecurringBillingCustomerModel
    {
        public List<Customer> ListCus { get; set; }
        public TotalAutomaticCustomerCountACHModel TotalAutomaticCustomerCountACHModel { get; set; }
        public TotalAutomaticCustomerCountCCModel TotalAutomaticCustomerCountCCModel { get; set; }
        public TotalAutomaticCustomerCountInvoiceModel TotalAutomaticCustomerCountInvoiceModel { get; set; }
        public TotalCustomerRMRACHModel TotalCustomerRMRACHModel { get; set; }
        public TotalCustomerRMRCCModel TotalCustomerRMRCCModel { get; set; }
        public TotalCustomerRMRInvoiceModel TotalCustomerRMRInvoiceModel { get; set; }
        public TotalCustomerBillAmountACHModel TotalCustomerBillAmountACHModel { get; set; }
        public TotalCustomerBillAmountCCModel TotalCustomerBillAmountCCModel { get; set; }
        public TotalCustomerBillAmountInvoiceModel TotalCustomerBillAmountInvoiceModel { get; set; }
        public TotalCustomerCountModel TotalCustomerCountModel { get; set; }
        public TotalRMRCountModel TotalRMRCountModel { get; set; }
        public TotalBillAmountCountModel TotalBillAmountCountModel { get; set; }
    }

    public class TotalAutomaticCustomerCountACHModel
    {
        public int TotalAutomaticCustomerCountACH { get; set; }
    }

    public class TotalAutomaticCustomerCountCCModel
    {
        public int TotalAutomaticCustomerCountCC { get; set; }
    }

    public class TotalAutomaticCustomerCountInvoiceModel
    {
        public int TotalAutomaticCustomerCountInvoice { get; set; }
    }

    public class TotalCustomerRMRACHModel
    {
        public double TotalCustomerRMRACH { get; set; }
    }

    public class TotalCustomerRMRCCModel
    {
        public double TotalCustomerRMRCC { get; set; }
    }

    public class TotalCustomerRMRInvoiceModel
    {
        public double TotalCustomerRMRInvoice { get; set; }
    }

    public class TotalCustomerBillAmountACHModel
    {
        public double TotalCustomerBillAmountACH { get; set; }
    }

    public class TotalCustomerBillAmountCCModel
    {
        public double TotalCustomerBillAmountCC { get; set; }
    }

    public class TotalCustomerBillAmountInvoiceModel
    {
        public double TotalCustomerBillAmountInvoice { get; set; }
    }

    public class TotalCustomerCountModel
    {
        public int TotalCustomerCount { get; set; }
    }

    public class TotalRMRCountModel
    {
        public double TotalRMRCount { get; set; }
    }

    public class TotalBillAmountCountModel
    {
        public double TotalBillAmountCount { get; set; }
    }

    public class TicketNotificationModel
    {
        public List<Lookup> TicketStatusList { get; set; }
        public List<Lookup> TicketTypeList { get; set; }
        public List<TicketCustomerNotification> ListNotification { get; set; }
    }

    public class StatusImageSetting
    {
        public string StatusTicket { get; set; }
        public TicketStatusImageSetting TicketStatusImageSetting { get; set; }
    }

    public class TicketStatusImageModel
    {
        public string status { get; set; }
        public string image { get; set; }
    }

    public class BadInventoryUserAssignModel
    {
        public Ticket Ticket { get; set; }
        public TicketUser TicketUser { get; set; }
        public CustomerAppointment CustomerAppointment { get; set; }
        public CustomerAppointmentEquipment CustomerAppointmentEquipment { get; set; }
        public InventoryTech InventoryTech { get; set; }
    }

    public class PackageEquipmentServiceModel
    {
        public List<SmartPackageEquipmentService> ListSmartPackageEquipmentService { get; set; }
        public PackageEquipmentServiceTotalCountModel PackageEquipmentServiceTotalCountModel { get; set; }
    }

    public class PackageEquipmentServiceTotalCountModel
    {
        public int TotalCount { get; set; }
    }

    public class SaveUserInfoModel
    {
        public int empid { set; get; }
        public string fnum { set; get; }
        public string lnum { set; get; }
        public string username { set; get; }
        public string email { set; get; }
        public string street { set; get; }
        public string city { set; get; }
        public string state { set; get; }
        public string zip { set; get; }
        public string phn { set; get; }
        public string ssn { set; get; }
        public string password { set; get; }
        public string hire { set; get; }
        public string place { set; get; }
        public string job { set; get; }
        public string session { set; get; }
        public string sales { set; get; }
        public string tech { set; get; }
        public string Status { set; get; }
        public bool iscalendar { set; get; }
        public string color { set; get; }
        public string ClockInIp { set; get; }
        public bool IsSupervisor { set; get; }
        public bool NoAutoClockout { set; get; }
        public string SuperVisorId { set; get; }
        public string DriversLicenseExpirationDate { set; get; }
        public string FireLicenseExpirationDate { set; get; }
        public string SalesLicenseExpirationDate { set; get; }
        public string InstallLicenseExpirationDate { set; get; }
        public bool ispayroll { set; get; }
        public bool IsSalesMatrix { set; get; }
        public string LicenseNo { set; get; }
        public int? BranchId { set; get; }
        public string BadgerUserId { set; get; }
        public string AlarmId { set; get; }
        public List<string> employeetimeclocksupervisor { set; get; }
        public bool currentemp { set; get; }
        public List<string> LeadSources { set; get; }

        public string street2 { set; get; }
        public string city2 { set; get; }
        public string state2 { set; get; }
        public string zip2 { set; get; }
        public string StreetPrevious { set; get; }
        public string BrinksDelearUser { get; set; }
        public string BrinksDelearPassword { get; set; }
        public string RouteList { get; set; }
        public int PasswordUpdateDays { get; set; }

    }

    public class BillingCheckModel
    {
        public bool IsBillingCheck { get; set; }
        public Guid BillEquipmentId { get; set; }
    }

    public class FilterReportModel
    {
        public string id { get; set; }
        public string searchtext { get; set; }
        public string convertmaxdate { get; set; }
        public string convertmindate { get; set; }
        public string user { get; set; }
        public string leadsource { get; set; }
        public string type { get; set; }
        public string createmaxdate { get; set; }
        public string createmindate { get; set; }
        public string transfermaxdate { get; set; }
        public string transfermindate { get; set; }
        public string paymentmethod { get; set; }
        public string cusid { get; set; }
        public string collectionmaxdate { get; set; }
        public string collectionmindate { get; set; }

        public string ticketcreateddatemax { get; set; }
        public string ticketcreateddatemin { get; set; }

        public string installmindate { get; set; }
        public string installmaxdate { get; set; }

        public string soldmindate { get; set; }
        public string soldmaxdate { get; set; }

        public string schedulemindate { get; set; }
        public string schedulemaxdate { get; set; }

        public string viewtype { get; set; }

        public string ticketcompletiondatemax { get; set; }
        public string ticketcompletiondatemin { get; set; }

    }
    public class EstimateModel
    {
        public string CustomerName { get; set; }
        public Int32 Id { get; set; }
        public Int32 CustomerIntId { get; set; }
        public string EstimatorId { get; set; }
        public string Status { get; set; }
        public DateTime SendDate { get; set; }
    }
    public class UpsellUserModel
    {
        public string EmpDay { get; set; }
        public string EmployeeName { get; set; }
        public int EmpId { get; set; }
        public Guid UserId { get; set; }
        public double AddedRMR { get; set; }
        public string ServiceName { get; set; }
        public int PiecesSold { get; set; }
        public double CollectedAmount { get; set; }
    }
    public class TotalUpSell
    {
        public int Total { get; set; }
    }
    public class SumOfUpsell
    {
        public double TotalRMR { get; set; }
        public double TotalAmount { get; set; }
        public double TotalSold { get; set; }
    }
    public class AccrualPtoHourModel
    {
        public double TotalPtoHour { get; set; }
    }
    public class PtoApproveModel
    {
        public double TotalApproveHour { get; set; }
    }
    public class UpsellUserReportModel
    {
        public List<UpsellUserModel> UpsellUserModelList { get; set; }
        public TotalUpSell TotalUpSell { get; set; }
        public SumOfUpsell SumOfUpsell { get; set; }

    }
    public class EstimateReportModel
    {
        public List<EstimateModel> EstimateModelList { get; set; } 
        public int TotalCount { get; set; }
    }
    public class EmployeeAccrualPtoAndApprovePtohourModel
    {
        public List<EmployeePTOHourLog> EmployeePTOHourLogList { get; set; }
        public List<EmployeePTOHourLog> schedulerList { get; set; }
        public List<EmployeePTOHourLog> approveLogList { get; set; }
        public AccrualPtoHourModel TotalPto { get; set; }
        public PtoApproveModel TotalApprove { get; set; }

    }
    public class RUGTicketAgreementModel
    {
        public string CompanyLogo { get; set; }
        public string Text { get; set; }
        public string Signature { get; set; }
        public string CompanyInfo { get; set; }
        public string SubmitInfo { get; set; }
        public string Text2 { get; set; }
        public string CustomerName { set; get; }
        public string SignDate { set; get; }

    }

    public class AllMenuItemModel
    {
        public List<RestMenu> MenuList { get; set; }
        public List<MenuDetail> MenuDetailList { get; set; }
        public List<MenuItem> MenuItemList { get; set; }
        public List<MenuItemDetail> MenuItemDetailList { get; set; }
        public List<Category> CategoryList { get; set; }
        public List<CategoryDetail> CategoryDetailList { get; set; }
        public List<Topping> ToppingList { get; set; }
        public List<ToppingCategory> ToppingCategoryList { get; set; }
    }
    public class SendAgreementPopup
    {
        public int TicketId { get; set; }
        public int CustomerId { get; set; }
        public string VerbalPassword { get; set; }
        public DateTime? OrginalContractDate { get; set; }
        public double? NonConfirmingFee { get; set; }
        public double? ActivationFee { get; set; }
        public string ContractTerm { get; set; }
        public string ContractType { get; set; }
        public int RenewalTerm { get; set; }
    }
    public class SendEmailTicketAssignModel
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Name { get; set; }
    }

    public class CustomerServiceEqpBillingModel
    {
        public Customer CustomerModel { get; set; }
        public PackageCustomer PackageCustomerModel { get; set; }
        public List<CustomerPackageService> CustomerPackageServiceModel { get; set; }
        public List<CustomerPackageEqp> CustomerPackageEqpModel { get; set; }
        public List<CustomerAppointmentEquipment> ListCustomerService { get; set; }
        public List<CustomerAppointmentEquipment> ListCustomerEquipment { get; set; }
    }

    public class AgreementQuestionAnswerPDFModel
    {
        public Customer Customer { get; set; }
        public List<AgreementQuestion> AgreementQuestion { get; set; }
        public List<AgreementAnswer> AgreementAnswer { get; set; }
    }

    public class ResturantOrderCustomModel
    {
        public List<ResturantOrderDetail> ListOrderDetail { get; set; }
        public ResturantOrder ResturantOrder { get; set; }

        public Customer Customer { get; set; }
        public WebsiteLocation WebsiteLocation { get; set; }
        public RestaurantCoupons RestaurantCoupons { get; set; }
    }

    public class CreateResturantModel
    {
        public string ResturantName { get; set; }
        public string UrlSlug { get; set; }
        public string WebsiteUrl { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Phone { get; set; }
        public string DomainName { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string CompanyImage { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public bool SentMail { get; set; }
    }

    public class SendEmailToCreateRestaurant
    {
        public string ToEmail { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string Name { get; set; }
        public string CopyrightYear { get; set; }
    }

    public class ItemReportsModel
    {
        public int TotalActive { get; set; }
        public int TotalItems { get; set; }
        public double AveragePrice { get; set; }
    }

    public class RestaurantOrderCustomModel
    {
        public int Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public string Location { get; set; }
        public string OrderType { get; set; }
        public string Status { get; set; }
        public string PickUpTime { get; set; }
        public string ContactNo { get; set; }
        public int Quantity { get; set; }
        public double Amount { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerStreet { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerState { get; set; }
        public string CustomerZip { get; set; }
        public double TaxAmount { get; set; }
        public string Notes { get; set; }
        public DateTime AcceptDate { get; set; }
        public DateTime RejectDate { get; set; }
        public string rejectNote { get; set; }
        public string PaymentMethod { get; set; }
    }

    public class RestaurantOrderDetailCustomModel
    {
        public int Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid OrderId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public double ItemPrice { get; set; }
        public int ItemQty { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string Toppings { get; set; }
        public string SpecialInstruction { get; set; }
        public bool IsStock { get; set; }
        public DateTime OrderDate { get; set; }
        public string DiscountCode { get; set; }
        public double DiscountValue { get; set; }
    }
    public class SpaiderReport
    {
        public string FoodName { get; set; }
        public int AcceptCount { get; set; }

        public int RejectCount { get; set; }
        public int PendingCount { get; set; }

    }

    public class LineChartReport
    {
        public string OrderDate { get; set; }
        public int AcceptCount { get; set; }

        public int RejectCount { get; set; }
        public int PendingCount { get; set; }

    }
    public class PieChart
    {
        public string FoodName { get; set; }
        public int Sale { get; set; }
    }
    public class PieReport
    {
        public int TotalSale { get; set; }

        public List<PieChart> PieChart { get; set; }

    }
    public class IeateryCategoryCustomModel
    {
        public int id { get; set; }
        public int orderid { get; set; }
    }

    public class EmailToOrderPlace
    {
        public string ToEmail { get; set; }
        public string Body { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string OrderNo { get; set; }
        public string TotalAmount { get; set; }
        public string RestaurantPhone { get; set; }
        public string RestaurantLogo { get; set; }
    }

    public class OrderSummeryDataModel
    {
        public string OrderType { get; set; }
        public int QuantityCount { get; set; }
        public int InProgressCount { get; set; }
        public int RejectedCount { get; set; }
        public int CompletedCount { get; set; }
        public double AverageOrder { get; set; }
        public double TotalRev { get; set; }
        public int CancellationCount { get; set; }
    }
    public class AllRecords
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Order { get; set; }
        public string SearchText { get; set; }
        public string LeadSource { get; set; }
        public string LeadStatus { get; set; }
        public string SalesPerson { get; set; }
        public string CustomerStatus { get; set; }
        public string SalesLocation { get; set; }
        public string IsLead { get; set; }
        public string ParentSource { get; set; }
        public string LeadSourceType { get; set; }
    }
    public class AllRecordsModel
    {
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public int RMRId { get; set; }
        public string CSId { get; set; }
        public string LeadSourceParent { get; set; }
        public string LeadSource { get; set; }
        public string LeadSourceType { get; set; }
        public string LeadStatus { get; set; }
        public string CustomerStatus { get; set; }
        public string SalesPerson { get; set; }
        public string SalesLocation { get; set; }
        public string AppoinmentSetBy { get; set; }
        public int IsLead { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class RecordCount
    {
        public int TotalRecord { get; set; }
    }
    public class AllRecordsReportModel
    {
        public List<AllRecordsModel> AllRecordsModelList { get; set; }
        public RecordCount Total { get; set; }
    }
    public class CustomCalendarAllRecords
    {
        public int EventLeadId { get; set; }
        public string EventTicketId { get; set; }
        public string EventAppointmentId { get; set; }
        public string EventStartDate { get; set; }
        public string EventEndDate { get; set; }
        public string EventType { get; set; }
        public string EventDisplayType { get; set; }
        public string EventColor { get; set; }
        public int CustomerIntId { get; set; }
        public string EventCustomerId { get; set; }
        public string EmployeeName { get; set; }
        public string EventEmployeeGuidId { get; set; }
        public int EmployeeIntId { get; set; }
        public string EventCustomerName { get; set; }
        public bool EventAllDay { get; set; }
        public bool IsCalled { get; set; }
        public string EventPhone { get; set; }
        public string EventStreet { get; set; }
        public string EventLocate { get; set; }
        public string EventDate { get; set; }
        public string EventStatus { get; set; }
        public string EventSubject { get; set; }
        public string EventIcon { get; set; }
        public string StatusColor { get; set; }
        public string EmpColor { get; set; }
        public string BookingId { get; set; }
        public string Subject { get; set; }
        public string EventAdditionalMember { get; set; }
        public string EventTicketAddress { get; set; }
        public double TicketAmount { get; set; }

    }
    public class CustomCalendarGlobalRecords
    {
        public string SearchKey { get; set; }
        public string Value { get; set; }
        public string OptionalValue { get; set; }
        public bool IsActive { get; set; }
        public string IsBottom { get; set; }
    }
    public class CustomCalendarEmployees
    {
        public int Id { get; set; }
        public int Rank { get; set; }
        public string UserId { get; set; }
        public string ResourceName { get; set; }
        public string GroupName { get; set; }
    }
    public class CustomCalendarTicketTypes
    {
        public int Id { get; set; }
        public string DisplayText { get; set; }
        public string DataValue { get; set; }
        public string AlterDisplayText { get; set; }
        public bool IsDefaultItem { get; set; }
    }
    public class CustomCalendarTicketStatus
    {
        public string TicketStatus { get; set; }
        public string Filename { get; set; }
        public string TicketStatusColor { get; set; }
    }
    public class CustomCalendarScheduleCalendarList
    {
        public List<CustomCalendarTicketStatus> CalendarTicketStatus { get; set; }
        public List<CustomCalendarGlobalRecords> CalendarGlobalRecords { get; set; }
        public List<CustomCalendarTicketTypes> CalendarViewTicketType { get; set; }
        public List<CustomCalendarEmployees> CalendarEmployeeList { get; set; }
    }
    public class CustomCalendarAllTaskList
    {
        public List<CustomCalendarAllRecords> CalendarTaskList { get; set; }
        public List<CustomCalendarEmployees> CalendarEmployeeList { get; set; }
        public int DeactiveTicketCount { get; set; }
    }

    public class MoveCustomer
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Order { get; set; }
        public string SearchText { get; set; }
        public int Id { get; set; }
        public string MoveMaxDate { get; set; }
        public string MoveMinDate { get; set; }

    }
    public class MoveCustomerModel
    {
        public string CustomerName { get; set; }
        public int Id { get; set; }
        public Guid CustomerId { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime MoveDate { get; set; }
        public string Street { get; set; }
        public string StreetType { get; set; }
        public string Appartment { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int IsLead { get; set; }
        public bool UnlinkCustomer { get; set; }
        public int OldCustomerId { get; set; }

        public string OldCustomerName { get; set; }
    }
    public class MoveCustomerReportModel
    {
        public List<MoveCustomerModel> MoveCustomerModelList { get; set; }
        public RecordCount Total { get; set; }

    }
    #region Geese Relief API
    public class RouteCustomerList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid RouteId { get; set; }
        public string CustomerName { get; set; }
    }
    public class RouteCustomerListModel
    {
        public List<RouteCustomerList> RouteCustomerList { get; set; }
    }
    public class RouteList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid RouteId { get; set; }
        public DateTime LastVisit { get; set; }
    }
    public class RouteListModel
    {
        public List<RouteList> RouteList { get; set; }
        public RecordCount Total { get; set; }
    }
    public class GeeseMedia
    {
        public Guid CustomerId { get; set; }
        public string Url { get; set; }
        public DateTime UploadDate { get; set; }
        public string Notes { get; set; }
        public string Assigner { get; set; }
        public string Address { get; set; }
    }
    public class GeeseNote
    {
        public Guid CustomerId { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
        public string Assigner { get; set; }
    }
    public class GeeseMediaNoteDetails
    {
        public List<GeeseMedia> GeeseMediaList { get; set; }
        public List<GeeseNote> GeeseNoteList { get; set; }
    }
    public class GeeseCustomer
    {
        public int Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid RouteId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int TotalMediaCount { get; set; }
        public int TotalNoteCount { get; set; }
        public string GeeseLead { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string ProfilePicture { get; set; }
        public int GeeseCount { get; set; }
        public bool IsCheckedIn { get; set; }
        public DateTime LastCheckedInTime { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
        public string LastNote { get; set; }
        public string LastMedia { get; set; }
    }

    public class GeeseCustomerDetailModel
    {
        public GeeseCustomer Customers { get; set; }
        public List<GeeseMedia> Media { get; set; }
        public List<GeeseNote> Note { get; set; }
    }
    public class CustomerGuidIdList
    {
        public Guid CustomerId { get; set; }
    }
    public class GeeseCustomerDetailHistory
    {
        public int Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid RouteId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string GeeseLead { get; set; }
        public int GeeseCount { get; set; }
        public bool IsCheckedIn { get; set; }
        public DateTime LastCheckedInTime { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
    }
    public class GeeseCustomerDetailHistoryModel
    {
        public List<GeeseCustomerDetailHistory> GeeseCustomerList { get; set; }
    }
    #endregion

    public class AppointmentEquipmentIdList
    {
        public int Id { get; set; }
    }

    public class RootObject
    {
        public List<IeateryResult> results { get; set; }
        public string status { get; set; }
    }

    public class IeateryResult
    {
        public string formatted_address { get; set; }
        public geometry geometry { get; set; }
        public string place_id { get; set; }
        public List<string> types { get; set; }
    }
    public class TicketType
    {
        public string Type { get; set; }
    }
    public class InstallerModel
    {
        public Guid Id { get; set; }
        public string Name { set; get; }
    }
    public class EquipmentSKU
    {
        public double Point { get; set; }
        public string SKU { get; set; }
    }
    public class SKU
    {
        public string text { get; set; }
        public string value { get; set; }
    }
    public class CompanyCost
    {
        public double Cost { get; set; }
    }

    public class TrackingNumberRecordingModel
    {
        public string MediaFile { get; set; }
        public string ReceivedDate { get; set; }
        public string PhoneNumber { get; set; }
        public string CallerId { get; set; }
        public string Status { get; set; }
    }
    public class RoutIdList
    {
        public Guid RouteId { get; set; }
    }
    public class GeeseActivityLogModel
    {
        public int TotalCoustomer { get; set; }
        public int TotalEmployee { get; set; }
        public int TotalRoute { get; set; }
        public int TotalGeese { get; set; }
        public int TotalCheckIn { get; set; }
    }
    public class ParentKey
    {
        public string Name { get; set; }
    }
    public class RMROverviewModel
    {
        public double RMROverview { get; set; }
        public double AmountDue { get; set; }
        public string PaymentMethod { set; get; }
        public double OpenInvoice { set; get; }
        public int DayPastDue { set; get; }
        public double OpenCredit { get; set; }
        public int BillOnDate { get; set; }
        public DateTime LastPayment { get; set; }
    }
    public class RMRTotalModel
    {
        public double TotalAmount { get; set; }
        public double TotalDue { get; set; }
    }
    public class RMRInvoice
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CustomerIntId { get; set; }
        public string InvoiceId { get; set; }
        public DateTime Date { get; set; }
        public double AmountDue { get; set; }
        public double NetDue { get; set; }
        public string Status { get; set; }
    }
    public class RMRInvoiceModel
    {
        public List<RMRInvoice> RMRInvoiceList { get; set; }
        public RMRTotalModel RMRTotal { get; set; }
        public int Total { get; set; }
    }
    public class RMRHistory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CustomerIntId { get; set; }
        public string InvoiceId { get; set; }
        public string Method { get; set; }
        public string CheckNo { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime InvoiceDate { get; set; }
        public double Amount { get; set; }
        public string BatchCode { get; set; }
        public string Funded { get; set; }
        public string Posted { get; set; }
    }
    public class RMRHistoryModel
    {
        public List<RMRHistory> RMRHistoryList { get; set; }
        public RMRTotalModel RMRTotal { get; set; }
        public int Total { get; set; }
    }
    public class RMRFilter
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string SearchText { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string Order { get; set; }
        public bool? GetReport { get; set; }
        public Guid CompanyId { get; set; }
    }
    public class CreditHistoryFilter
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string SearchText { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string Order { get; set; }
        public bool? GetReport { get; set; }
        public Guid CompanyId { get; set; }
    }
    public class CreditHistory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CustomerIntId { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }
        public string Bureau { get; set; }
        public string Score { get; set; }
    }
    public class CreditHistoryModel
    {
        public List<CreditHistory> CreditHistoryList { get; set; }
        public int Total { get; set; }
    }
    public class RMRAuditFilter
    {
        public DateTime? AsOfDate { get; set; }
        public string SearchText { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string Order { get; set; }
        public bool? GetReport { get; set; }
        public Guid CompanyId { get; set; }

    }
    public class SMSHistoryFilter
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string SearchText { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string Order { get; set; }
        public bool? GetReport { get; set; }
        public Guid CompanyId { get; set; }
    }
    public class SMSHistoryReport
    {
        public int Id { get; set; }
        public string ToMobile { get; set; }
        public string FromMobile { get; set; }
        public DateTime SentDate { get; set; }
        public string FromName { get; set; }
    }
    public class SMSHistoryReportModel
    {
        public List<SMSHistoryReport> SMSHistoryList { get; set; }
        public int Total { get; set; }
    }
    public class EstimatorId
    {
        public string Id { get; set; }
    }
    public class SalesPersonFilter
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string SearchText { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string Order { get; set; }
        public bool? GetReport { get; set; }
        public Guid CompanyId { get; set; }
    }
    public class SalesPersonReport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Sales { get; set; }
        public int Pending { get; set; }
        public int Completed { get; set; }
        public DateTime ConvertionDate { get; set; }
    }
    public class SalesPersonReportModel
    {
        public List<SalesPersonReport> SalesPersonList { get; set; }
        public int Total { get; set; }
        public int TotalSales { get; set; }
        public int TotalPending { get; set; }
        public int TotalCompleted { get; set; }
    }

    public class LoginCustomModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class SalesReportFilter
    {
        public Guid CompanyId { get; set; }
        public DateTime? StartDate { set; get; }
        public DateTime? EndDate { set; get; }
        public bool? GetReport { get; set; }
        public string searchtext { get; set; }
        public int pageno { get; set; }
        public int pagesize { get; set; }
        public string order { get; set; }
        public string SalesPerson { get; set; }
        public string TicketType { get; set; }
        public string LeadSource { get; set; }
        public string CustomerStatus { get; set; }
        public List<string> SalesPersonList { get; set; }
        public List<string> TicketTypeList { get; set; }
        public List<string> LeadSourceList { get; set; }
        public List<string> CustomerStatusList { get; set; }
        public DateTime? ScheduledOnMin { get; set; }
        public DateTime? ScheduledOnMax { get; set; }
        public DateTime? CompletedMin { get; set; }
        public DateTime? CompletedMax { get; set; }
        public DateTime? InstallMin { get; set; }
        public DateTime? InstallMax { get; set; }
        public int BatchMin { get; set; }
        public int BatchMax { get; set; }
    }
    public class InvoicePaymentDate
    {
        public DateTime PaymentDate { get; set; }
    }
    public class SetupAlarmModel
    {
        public string PropertyType { set; get; }
        public Guid CompanyId { set; get; }
        public Guid CustomerId { set; get; }
        public string DealerCustomer { set; get; }
        public string InsStreet { set; get; }
        public string InsZip { set; get; }
        public string InsState { set; get; }
        public string InsCity { set; get; }
        public string Phone { set; get; }
        public string Emailaddress { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string CentralStationAccountNo { set; get; }
        public string CentralStationForwardingOption { set; get; }
        public string CentralStationRecieverNumber { set; get; }
        public string CentralStationName { set; get; }
        public bool PhoneLinePresent { set; get; }
        public bool IgnoreLowCoverageError { set; get; }
        public string LoginName { set; get; }
        public string LoginPassword { set; get; }
        public string PanelType { set; get; }
        public string PanelVersion { set; get; }
        public string Culture { set; get; }
        public string CustomerStatus { set; get; }
        public string ModelSerialNumber { set; get; }
        public int? PackageId { set; get; }

        public string City { set; get; }
        public string Street { set; get; }
        public string State { set; get; }
        public string Zip { set; get; }

        public DateTime? InstallationDate { set; get; }
        public string InstallerUserName { set; get; }
        public string Network { set; get; }

        public string AlarmRefId { set; get; }
        public string Action { set; get; }

        public string AuthUser { set; get; }
        public string AuthPass { set; get; }
        public string SalesRepName { set; get; }

        public bool SameInsAddress { get; set; }
        public string CompanyName { get; set; }
        public string[] ForwardedEvents { get; set; }
        public int?[] Adonitem { get; set; }

        public string CustomerType { get; set; }
        public bool? IsContractSigned { get; set; }
    }
    public class MarginReportCustom
    {
        public List<Customer> CustomerModel { get; set; }
        public int TotalCount { get; set; }
    }
}