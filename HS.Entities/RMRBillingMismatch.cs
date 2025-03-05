using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class RMRBillingMismatch 
	{
        public int CustomerIntId { set; get; }
        public string CustomerName { set; get; }
        public string ResolvedByName { set; get; }
    }
    public class RMRBillingMismatchModel
    {        
        public List<RMRBillingMismatch> RMRBillingMismatchList { set; get; }
        public Count TotalCount { set; get; }

        public double TotalAmountByPage { get; set; }
    }

    public class ARBUnpaidInvoiceGenerateList
    {
        public int Id { get; set; }
        public int CustomerIntId { get; set; }
        public string CustomerGuidId { get; set; }
        public string CustomerName { set; get; }
        public string InvoiceId { get; set; }
        public string EmailAddress { set; get; }
        public string BillingMethod { set; get; }
        public int BillingMethodId { set; get; }
        public DateTime InvoiceDate { set; get; }
        public DateTime DueDate { set; get; }
        public double Amount { set; get; }
        public double PastDueAmount { set; get; }
        public double TotalAmount { set; get; }
        public double TotalTaxAmount { set; get; }
        public double RMRTaxAmount { set; get; }
        public double SubTotalAmountWithTax { set; get; }
    }
}
