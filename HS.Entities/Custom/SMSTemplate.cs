using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Entities.Custom
{
    public class SMSAgreement
    {
        public string ShortUrl { get; set; }
        public string CompanyName { get; set; }
        public List<string> ToNumber { get; set; }
    }
    public class SMSFile
    {
        public string ShortUrl { get; set; }
        public bool IsFileWithoutCustomerSign { get; set; }
        public string CompanyName { get; set; }
        public List<string> ToNumber { get; set; }
    }
    public class InvoiceSms
    {
        public string Message { get; set; }
       
    }
    public class CustomerInfoSms
    {
        public string Message { get; set; }

    }
    public class EstimatorSms
    {
        public string Message { get; set; }

    }
    public class ReminderSms
    {
        public string Message { get; set; }
        public string CustomerName { get; set; }
        public string CompanyName { get; set; }
        public int CusId { get; set; }
        public string AttnBy { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string NoteType { get; set; }
    }
    public class RequisitionSms
    {
        public string Message { get; set; }

    }
    public class PurchaseOrderSms
    {
        public string Message { get; set; }
    }
    public class LeadsEstimateAgree
    {
        public string EnvoiceNo { get; set; }
    }
    public class SalesPersonSms
    {
        public string Message { get; set; }

    }
    public class ActivityNotificationSMS
    {
        public string Message { get; set; }
        public List<string> ReceiverNumber { get; set; }
    }
    public class SMSAddendum
    {
        public string ShortUrl { get; set; }
        public List<string> ToNumber { get; set; }
    }
}
