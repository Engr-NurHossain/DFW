using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class Transaction 
	{
		public DateTime InvoiceDueDate { set; get; }
        public int InvoiceId { set; get; }
        public double Balance { set; get; }
        public string InvoiceIdStr { set; get; }
        public string CustomerName { set; get; }
        public string Description { set; get; }
        public string InvoiceNo { set; get; }
        public string TransactionId { set; get; }
        public string AuthRefId { set; get; }
        public string CustomerBussinessName { get; set; }
        public string TransactionUserName { get; set; }

        public int invId  { get; set; }
        public int CustomerIdValue { get; set; }
        public double RefundAmount { get; set; }
        public string PaymentMethodVal { get; set; }
    }
    public class TransactionPdfModel
    {
        
        public string CustomerName { set; get; }
        public string BusinessName { set; get; }
        public string InvoiceId { set; get; }
        public DateTime PaymentDate { set; get; }
        public string PaymentMethod { set; get; }
        public double PaymentAmount { set; get; }
        public double InvoiceTotal { set; get; }
        public double InvoicePreviousBalance { set; get; }
        public double InvoiceBalance { set; get; }
        public string TransactionId { set; get; }
        public string CheckNo { set; get; }



        public Guid CompanyId { set; get; }
        public string CompanyName { set; get; }
        public string CompanyAddressFormat { set; get; }
        public string CompanyLogo { set; get; }
        public string AmountInWord { set; get; }
    }
}
