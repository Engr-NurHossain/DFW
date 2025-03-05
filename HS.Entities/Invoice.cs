using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class Invoice 
	{
        public String ParentInvoiceRef { set; get; }
        public int CustomerIntId { set; get; }
		public string CustomerName { set; get; }
        public string CustomerNo { set; get; }
        public string InvoiceMessage { set; get; }
        public double AmountReceived { set; get; }
        public double PayableAmount { set; get; }
        public string AuthRefId { set; get; }
        public string CustomerBussinessName { get; set; }
        public string UserNum { get; set; }
        public DateTime NoteAddedDate { get; set; }
        public List<InvoiceNote> NoteInvoice { get; set; }
        public DateTime InvoiceNoteAddedDate { get; set; }
        public string InvoiceEquipDes { get; set; }
        public List<InvoiceDetail> InvoiceListDetail { get; set; }
        public string CustomerMailAddress { get; set; }
        public DateTime? CustomerViewedTime { set; get; }
        public string NotesInvoice { get; set; }
        public string NoteInvoiceAddedBy { get; set; }
        public string SalesLocationName { get; set; }
        public string CustomerViewedType { get; set; }
        public double Total { get; set; }
        public double PastDueAmount { get; set; }
        public double CurrentBilledAmount { get; set; }
        
        public string ccEmail { get; set; }
        public DateTime CustomerCreatedDate { get; set; }
        public DateTime ReturnedDate { get; set; }
        #region Pdf company info
        //For Pdf Head Company Address
        public string CompanyInfo { get; set; }
        #endregion
        #region Pdf User Stamp
        public List<CustomerAgreement> CustomerAgreement { set; get; }
        #endregion
        public int LeadEstimateCount { get; set; }
        public int AgingDate { get; set; }
        public string PaymentMethod { get; set; }
        public int EstimateEqpCount { get; set; }
        public DateTime TransacationDate { get; set; }
        public int Day { get; set; }
    }

    public class InvoiceAPI
    {
        public int Id { set; get; }
        public string InvoiceId { set; get; }
        public string Description { set; get; }
        public string CreatedBy { set; get; }
        public DateTime CreatedDate { set; get; }
        public double Amount { set; get; }
        public double TotalAmount { set; get; }
        public int EstimateEqpCount { set; get; }
        public string DrawingImage { get; set; }
        public string CameraImage { get; set; }
        public string SignaImage { get; set; }
        public int EstimateEqpQuantity { set; get; }
        public string PDFUrl { get; set; }
    }
    public class EstimateListWithCountModelForAPI
    {
        public List<InvoiceAPI> EstimateList { get; set; }
        public TotalEstimateCount TotalEstimateCount { get; set; }
        
    }
    public class EstimateListWithCountModel
    {
        public List<Invoice> EstimateList { get; set; }
        public TotalEstimateCount TotalEstimateCount { get; set; }
    }
    public class TotalEstimateCount
    {
        public int Counter { get; set; }
        public int TotalAmount { get; set; }
    }
    public class InvoiceFilter
    {
        public int PageSize { get; set; }
        public int PageNo { get; set; }
        public int CustomerIntId { get; set; }
        
    }
    public class TaxCollection
    {
        public List<TaxCollection> TaxCollectionList { set; get; }

        public int Id { get; set; }

        public string InvId { get; set; }

        public int CusId { get; set; }

        public double Tax { get; set; }

        public string CusName { get; set; }

        public DateTime TransactionDate { get; set; }

        public string Searchtext { set; get; }
        public int PageNo { set; get; }
        public int PageSize { set; get; }
        public int TotalCount { set; get; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }  
        public double TotalAmountByPage { get; set; }

    }
    public class TaxCollectionFilter
    {
        public Guid CustomerId { set; get; }
        public Guid CompanyId { set; get; }
        public string order { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? PageNo { set; get; }
        public int PageSize { set; get; }
        public string SearchText { get; set; }

        public string viewtype { get; set; }

        public int CusId { get; set; }

        public string InvoiceId { get; set; }     
    }

    public class GeneratePdfInvoiceStatementModel
    {
        public string CustomerId { set; get; }
        public string CompanyId { set; get; }
        public string TicketId { set; get; }
        public int CustomerIntId { get; set; }
        public int InvoiceIntId { get; set; }
        public DateTime InstallDate { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public double Amount { get; set; }
        public double TotalAmount { get; set; }
        public double BalanceDue { get; set; }
        public double Tax { get; set; }
        public double DiscountAmount { get; set; }
        public double CreditBalance { get; set; }
        public double NetTotalBalance { get; set; }
        public string CustomerName { get; set; }
        public string Street { get; set; }
        public string CustomerAddress { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string InvoiceId { get; set; }
        public string Status { get; set; }
        public string InvoiceFor { get; set; }
        public string Description { get; set; }
        public double NetDueAmount { get; set; }
    }
    public class GeneratePdfInvoiceDetailsStatementModel
    {
        public int Quantity { set; get; }
        public string InvoiceId { get; set; }
        public string EquipDetail { get; set; }
        public string EquipName { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }
    }
    public class GeneratePdfOtherDueOpenInvoiceStatementModel
    {
        public int InvoiceIntId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public double Amount { get; set; }
        public double TotalAmount { get; set; }
        public double BalanceDue { get; set; }
        public double Tax { get; set; }
        public double DiscountAmount { get; set; }
        public string CustomerId { set; get; }
        public string InvoiceId { get; set; }
        public string Status { get; set; }
        public string InvoiceFor { get; set; }
        public string Description { get; set; }
    }
    public class GeneratePdfInvoiceStatementModelList
    {
        public GeneratePdfInvoiceStatementModel InvoiceStatement { get; set; }
        public List<GeneratePdfInvoiceDetailsStatementModel> InvoiceDetailsList { get; set; }
        public List<GeneratePdfOtherDueOpenInvoiceStatementModel> DueOpenInvoiceList { get; set; }
    }
    public class InvoiceStatementEmailSendModel
    {
        public string Id { set; get; }
        public string CustomerId { set; get; }
        public string StatementFor { get; set; }
        public string EmailDescription { get; set; }
        public string EmailSubject { get; set; }
        public string CCEmail { get; set; }
        public string ToEmail { get; set; }
    }
}
