using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Entities
{
    public partial class CustomerDraft
    {
        public string CreatedByVal { set; get; }
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
        public bool IsMonitronics { get; set; }
        public bool IsCentral { get; set; }

        // public CustomInitialLeadPackageModel CustomInitialLeadPackageModelItems { get; set; }
        public string BillMethod { get; set; }
        public List<QaAnswer> QAList { get; set; }
        public List<QaAnswer> QAList1 { get; set; }
        public string LeadName { get; set; }

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
        public CustomerSystemInfo CustomerSystemInfo { get; set; }
        public CustomerSpouse CustomerSpouse { get; set; }
        public LeadCorrespondence LeadCorrespondence { get; set; }
        public int TotalActiveCustomer { get; set; }
        public int TotalRMRCustomer { get; set; }
        public CustomerTabCounts CustomerTabCounts { set; get; }
        public double UnpaidInvoiceTotal { get; set; }
        public List<ReferingCustomer> ReferingCustomerList { set; get; }
        public Dictionary<string, string> EquipmentDetailList { set; get; }

        public double CreditBalance { set; get; }

        //TicketAdd
        public Guid[] AssignedTo { get; set; }
        public DateTime CompletionDate { get; set; }
        public Ticket Ticket { get; set; }
        public int PaymentIncress { get; set; }
    }
}
