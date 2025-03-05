using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class UserActivity 
	{
        public int UserActivityCustomerIntId { get; set; }
        public Guid CustomerGId { get; set; }
        public string CustomerName { get; set; }

        public int CustomerIntId { get; set; }

        public bool IsLead { get; set; }
    }
    public partial class CreditModel
    {
        public int CusId { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerStatus { get; set; }

        public string CreditUser { get; set; }

        public DateTime CreditIssueDate { get; set; }
        public double CreditAmount { get; set; }
        public double CreditUsedAmount { get; set; }
        public double CreditAmountAvailable { get; set; }
        public string CreditReason { get; set; }
    }

    public partial class UserActivityCustomerModel
    {
       
        public List<UserActivity> ListUserActivity { get; set; }
        public SalesReportCountModel InvoiceReportCountModel { get; set; }
        public TotalSalesAmountModel TotalInvoiceAmountModel { get; set; }
        
    }
    public partial class RMRCreditModel
    {

        public List<CreditModel> RMRCreditList { get; set; }
        public SalesReportCountModel TotalRMRCredit { get; set; }
    }

}
