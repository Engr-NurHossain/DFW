using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;
using HS.Framework.Utils;

namespace HS.Entities
{
	public partial class FileTemplate 
	{
        public int DocumentFileManagementCount { get; set; }
    }
    public partial class FileTemplateWithCustomerInfo
    {
        public FileTemplate fileTemplate { get; set; }
        public CustomerAgreementTemplate cusAgreementTemplate { get; set; }
        public int fileTemplateId { get; set; }
        public int customerId { get; set; }
        public Company Company { get; set; }
        public string FileManagementCustomerSignature { get; set; }
        public string FileManagementCustomerSignatureDate { get; set; }
        public DateTime FileManagementCustomerSignatureDateVal { get; set; }
        public string CompanySignature { get; set; }
        public string CompanySignatureDate { get; set; }
        public string AdsLogo { get; set; }
        public string AuthorizedRepresentative { get; set; }
        public string iEateryLogo { get; set; }
        public string VisaLogo { get; set; }
        public string MastercardLogo { get; set; }
        public string DiscoverLogo { get; set; }
        public string AmericanExpressLogo { get; set; }
        public string BrinksLogo { get; set; }
        public string KazarLogo { get; set; }
        public string ContractTeam { get; set; }
        public string ContractTeamVal { get; set; }
        public string OnitSmartHome { get; set; }
        public string InstallationAddress { get; set; }
        public string CancellationDate { get; set; }
        public float? RemainingBalance { get; set; }
        public Customer customerInfo { get; set; }
        public Employee employeeInfo { get; set; }
        public PaymentInfo _paymentInfo { get; set; }
    }
}
