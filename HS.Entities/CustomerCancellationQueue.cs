using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
    public partial class CustomerCancellationQueue
    {
        public DateTime ExpireDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public bool CustomerIsActive { get; set; }
        public int CustomerIdInt { get; set; }
        public string OtherReason { get; set; }
        public string CreatedByVal { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }

        public List<string> CacellationReasonList { get; set; }
    }
    public class TotalCustomerCancellationCount
    {
        public int TotalCount { get; set; }
    }
    public class TotalUnpaidInvoiceAmount
    {
        public double TotalUnpaidAmount { get; set; }
    }
    public partial class CustomerCancellationQueueListWithCount
    {
        public List<CustomerCancellationQueue> CustomerCancellationQueueList { get; set; }
        public TotalCustomerCancellationCount TotalCustomerCancellationCount { get; set; }
        public TotalUnpaidInvoiceAmount TotalUnpaidInvoiceAmount { get; set; }
    }

    public partial class CustomerCancellationList
    {
        public Guid CustomerId { get; set; }
        public bool IsInvoiceOff { get; set; }
        public bool IsBillingOff { get; set; }
        public bool IsAlarmOff { get; set; }
    }
    public partial class CustomerCancellationLookup
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
    public partial class CustomerCancellationQueueAggrement
    {
        public string CustomerName { get; set; }
        public string CompanyLogo { get; set; }
        public string CompanyName { get; set; }
        public string CompanyStreet { get; set; }
        public string CompanyState { get; set; }
        public string CompanyCity { get; set; }
        public string CompanyWebsite { get; set; }
        public string CompanyZipcode { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyEmail { get; set; }
        public List<CustomerCancellationLookup> CancellationReason { get; set; }
        public string CustomerAddress { get; set; }
        public double RemainingBalance { get; set; }
        public DateTime CancellationDate { get; set; }
        public string Reason { get; set; }
        public bool IsSigned { get; set; }
        public string SignedImg { get; set; }
        public Guid CompanyId { get; set; }
        public string Note { get; set; }
        public List<CustomerCancellationReason> CustomerCancellationReasonList { get; set; }
    }
}
