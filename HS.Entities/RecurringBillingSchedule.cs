using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class RecurringBillingSchedule 
	{
        public string LastInvoice { get; set; }
        public string Intervals { get; set; }
        public string CustomerName { get; set; }
        public int CustomerIntId { get; set; }
        public int UnpaidCount { get; set; }
        public int InvoiceIntId { get; set; }
        public string InvoiceId { get; set; }
        public string CustomerAddress { get; set; }
        public DateTime BillDate { get; set; } 
        public int BillDay { get; set; } 
        public DateTime InvoiceDate { get; set; } 
        public bool ShowBillDate { get; set; }
        public bool ShowLineItem { get; set; }
        public bool ShowDayInAdvance { get; set; }
        public bool ShowAddNewLineItemButton { get; set; }
    }
    public class RecurringBillingScheduleCount
    {
        public int TotalRecurringBillingSchedule { get; set; }
    }
    public partial class ReurringBillingScheduleModel
    {
        public RecurringBillingSchedule Schedule { set; get; }
        public List<RecurringBillingScheduleItems> ScheduleItems { get; set; }
        public Customer Customer { set; get; }
        public Company Company { set; get; }
        public int BillDay { get; set; }
        public int BillingMethod { get; set; }
        public double SalesTax { get; set; }
    }
    public partial class RecurringBillingScheduleReportModel
    {
        public List<RecurringBillingSchedule> ScheduleList { set; get; }
        public RecurringBillingScheduleCount Count { set; get; }
        public double TotalRMR { get; set; }
        public int TemplateCount { get; set; }
        public double TotalAmount { get; set; }
        public double TotalTax { get; set; }
        public double TotalBilling { get; set; }
    }
    public partial class RecurringBillingInvoiceCreateNotificationEmailInfo
    {
        public string ToEmail { get; set; }
        public string CCEmail { get; set; }
        public string BCCEmail { get; set; }
        public string EmailSujbect { get; set; }
        public string EmailBody { get; set; }
        public int InvoiceIntId { get; set; }
    }
    public class RecurringBillingTempSettings
    {
        public bool DaysInAdvanceSetting { get; set; }
        public bool eInvoiceSettings { get; set; }
        public bool PaperlessBillsSettings { get; set; }
        public bool eReceiptSettings { get; set; }
        public bool RMRUnpaidBillsSettings { get; set; }
        public bool OtherUnpaidBillsSettings { get; set; }
        public bool LineItemsSettings { get; set; }
        public bool AddNewLineItemsButtonSettings { get; set; }
        public bool BillDay { get; set; }
    }
    public class RecurringBillingPaymentInfoModel
    {
        public int PaymentId { get; set; }
        public string PaymentMethod { get; set; }
    }

    public class RMRCollectPaymentInfoFromFileModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string InvoiceNumber { get; set; }
        public double TotalDueAmount { get; set; }
        public string ChequeNumber { get; set; }
        public string PaymentPethod { get; set; }
        public double CollectedAmount { get; set; }
        public string InvoiceType { get; set; }
        public string CompanyName { get; set; }
        public Guid CompanyId { get; set; }
    }
}
