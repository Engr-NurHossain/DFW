using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HS.Entities
{
    /*
     * 
     * 1.Use [DisplayName("xx")] for column names
     * 2.Use [Browsable(false)] to not show the item in excel sheet
     * 3.Use (List<Obj>).ListToDataTable() to get DataTable for that list (using HS.Framework)
     * 
     */

    public class IsPcFinanceApplyModel
    {
        public int MerchantId { get; set; }
        public double AmountRequested { get; set; }
        public string OptionCode { get; set; }
        public bool? ProdSecuritySystem { get; set; }
        public string ProdMiscDescription { get; set; }
        public string ApplicantLastName { get; set; }
        public string ApplicantFirstName { get; set; }
        public string ApplicantMiddleName { get; set; }
        public string ApplicantNameSuffix { get; set; }
        public DateTime ApplicantDOB { get; set; }
        public string ApplicantSSN { get; set; }
        public string ApplicantHomePhone { get; set; }
        public string ApplicantCellPhone { get; set; }

        public string ApplicantEmailAddress { get; set; }
        public string ApplicantDriversLicense { get; set; }
        public string ApplicantStreet { get; set; }
        public string ApplicantCity { get; set; }
        public string ApplicantState { get; set; }
        public string ApplicantZipCode { get; set; }

        public string ApplicantCountry { get; set; }
        public string ApplicantYearsAtAddress { get; set; }
        public string ApplicantMonthsAtAddress { get; set; }
        public double? MortgagePayment { get; set; }
        public string MortgageHolder { get; set; }
        public double ?MortgageBalance { get; set; }

        public string BankName { get; set; }
        public string BankAcctType { get; set; }
        public string ApplicantPrevStreet { get; set; }
        public string ApplicantPrevCity { get; set; }
        public string ApplicantPrevState { get; set; }
        public string ApplicantPrevZipCode { get; set; }
        public string ApplicantPrevCountry { get; set; }

        public string ApplicantPrevYearsAtAddress { get; set; }

        public string ApplicantPrevMonthsAtAddress { get; set; }
        public string ApplicantEmployer { get; set; }
        public string ApplicantYearsEmployed { get; set; }
        public string ApplicantMonthsEmployed { get; set; }
        public string ApplicantEmployerPhone { get; set; }
        public string ApplicantPosition { get; set; }
        public double? ApplicantGrossAnnualIncome { get; set; }

        public double? ApplicantOtherIncome { get; set; }
        public string ApplicantOtherIncomeSource { get; set; }
        public string SelectedAssignedUserId { get; set; }
        public string CoapplicantLastName { get; set; }
        public string CoapplicantFirstName { get; set; }
        public string CoapplicantMiddleName { get; set; }
        public string CoapplicantNameSuffix { get; set; }
        public DateTime? CoapplicantDOB { get; set; }
        public string CoapplicantSSN { get; set; }
        public string CoapplicantHomePhone { get; set; }
        public string CpapplicantCellPhone { get; set; }
        public string CoapplicantDriversLicense { get; set; }
        public string CoapplicantEmailAddress { get; set; }
        public string CoapplicantStreet { get; set; }
        public string CoapplicantCity { get; set; }
        public string CoapplicantState { get; set; }
        public string CoapplicantZipCode { get; set; }
        public string CoapplicantCountry { get; set; }
        public string CoapplicantYearsAtAddress { get; set; }
        public string CoapplicantMonthsAtAddress { get; set; }
        public string CoapplicantEmployer { get; set; }
        public string CoapplicantYearsEmployed { get; set; }
        public string CoapplicantMonthsEmployed { get; set; }
        public string CoapplicantEmployerPhone { get; set; }
        public string CoapplicantPosition { get; set; }
        public double? CoapplicantGrossAnnualIncome { get; set; }
        public double? CoapplicantOtherIncome { get; set; }
        public string CoapplicantOtherIncomeSource { get; set; }


    }
    public class IsPcResponseModel
    {
        public string ApplicationID { get; set; }
        public string Decision { get; set; }
        public string CurrentStatus { get; set; }
        public string HasRequirementsForESign { get; set; }
        public List<string> ValidationErrors { get; set; }
        public string ErrorMessage { get; set; }

    }
    public class GenericSubmitAppResponse
    {
   
        public string Errors { get; set; }

        public string Success { get; set; }

        public string ApplicationNumber { get; set; }
   
    }

    public class IsPcStatusModel
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string MerchantId { get; set; }
        public string ApplicationID { get; set; }
        public string Decision { get; set; }
        public string CurrentStatus { get; set; }
        public string FundingOnHoldMessage { get; set; }
        public List<string> ProcessingMessages { get; set; }
        public List<string> AnalystNotes { get; set; }
    }

  

    [XmlRoot(ElementName = "PrimaryApplicant")]
    public class PrimaryApplicant
    {
        [XmlElement(ElementName = "Firstname")]
        public string Firstname { get; set; }
        [XmlElement(ElementName = "Lastname")]
        public string Lastname { get; set; }
        [XmlElement(ElementName = "DOB")]
        public string DOB { get; set; }
        [XmlElement(ElementName = "Phone")]
        public string Phone { get; set; }
        [XmlElement(ElementName = "Email")]
        public string Email { get; set; }
        [XmlElement(ElementName = "SSN")]
        public string SSN { get; set; }
        [XmlElement(ElementName = "US_Citizen")]
        public string US_Citizen { get; set; }
        [XmlElement(ElementName = "PPAddressPB")]
        public string PPAddressPB { get; set; }
        [XmlElement(ElementName = "AddressStreetNumber")]
        public string AddressStreetNumber { get; set; }
        [XmlElement(ElementName = "AddressStreetName")]
        public string AddressStreetName { get; set; }
        [XmlElement(ElementName = "AddressStreetType")]
        public string AddressStreetType { get; set; }
        [XmlElement(ElementName = "POBox")]
        public string POBox { get; set; }
        [XmlElement(ElementName = "City")]
        public string City { get; set; }
        [XmlElement(ElementName = "State")]
        public string State { get; set; }
        [XmlElement(ElementName = "Zip")]
        public string Zip { get; set; }
        [XmlElement(ElementName = "AddressHouseType")]
        public string AddressHouseType { get; set; }
        [XmlElement(ElementName = "ActiveMilitary")]
        public string ActiveMilitary { get; set; }
        [XmlElement(ElementName = "DriversLicense")]
        public string DriversLicense { get; set; }
        [XmlElement(ElementName = "DriversLicenseState")]
        public string DriversLicenseState { get; set; }
        [XmlElement(ElementName = "AnnualIncome")]
        public string AnnualIncome { get; set; }
        [XmlElement(ElementName = "IncomeFrequency")]
        public string IncomeFrequency { get; set; }
        [XmlElement(ElementName = "EmployerOccupation")]
        public string EmployerOccupation { get; set; }
        [XmlElement(ElementName = "EmployerName")]
        public string EmployerName { get; set; }
        [XmlElement(ElementName = "EmployerZip")]
        public string EmployerZip { get; set; }
        [XmlElement(ElementName = "Employment_Type")]
        public string Employment_Type { get; set; }
        [XmlElement(ElementName = "Employer_Years")]
        public string Employer_Years { get; set; }
    }

    [XmlRoot(ElementName = "CoApplicant")]
    public class CoApplicant
    {
        [XmlElement(ElementName = "Firstname")]
        public string Firstname { get; set; }
        [XmlElement(ElementName = "Lastname")]
        public string Lastname { get; set; }
        [XmlElement(ElementName = "DOB")]
        public string DOB { get; set; }
        [XmlElement(ElementName = "Phone")]
        public string Phone { get; set; }
        [XmlElement(ElementName = "Email")]
        public string Email { get; set; }
        [XmlElement(ElementName = "SSN")]
        public string SSN { get; set; }
        [XmlElement(ElementName = "US_Citizen")]
        public string US_Citizen { get; set; }
        [XmlElement(ElementName = "PPAddressCB")]
        public string PPAddressCB { get; set; }
        [XmlElement(ElementName = "AddressStreetNumber")]
        public string AddressStreetNumber { get; set; }
        [XmlElement(ElementName = "AddressStreetName")]
        public string AddressStreetName { get; set; }
        [XmlElement(ElementName = "AddressStreetType")]
        public string AddressStreetType { get; set; }
        [XmlElement(ElementName = "POBox")]
        public string POBox { get; set; }
        [XmlElement(ElementName = "City")]
        public string City { get; set; }
        [XmlElement(ElementName = "State")]
        public string State { get; set; }
        [XmlElement(ElementName = "Zip")]
        public string Zip { get; set; }
        [XmlElement(ElementName = "AddressHouseType")]
        public string AddressHouseType { get; set; }
        [XmlElement(ElementName = "ActiveMilitary")]
        public string ActiveMilitary { get; set; }
        [XmlElement(ElementName = "DriversLicense")]
        public string DriversLicense { get; set; }
        [XmlElement(ElementName = "DriversLicenseState")]
        public string DriversLicenseState { get; set; }
        [XmlElement(ElementName = "AnnualIncome")]
        public string AnnualIncome { get; set; }
        [XmlElement(ElementName = "IncomeFrequency")]
        public string IncomeFrequency { get; set; }
        [XmlElement(ElementName = "EmployerOccupation")]
        public string EmployerOccupation { get; set; }
        [XmlElement(ElementName = "EmployerName")]
        public string EmployerName { get; set; }
        [XmlElement(ElementName = "EmployerZip")]
        public string EmployerZip { get; set; }
        [XmlElement(ElementName = "Employment_Type")]
        public string Employment_Type { get; set; }
        [XmlElement(ElementName = "Employer_Years")]
        public string Employer_Years { get; set; }
    }

    [XmlRoot(ElementName = "loanapplication")]
    public class Loanapplication
    {
        [XmlElement(ElementName = "DefiSourceSystemId")]
        public string DefiSourceSystemId { get; set; }
        [XmlElement(ElementName = "DefiDealerId")]
        public string DefiDealerId { get; set; }
        [XmlElement(ElementName = "RequestedLoanAmount")]
        public string RequestedLoanAmount { get; set; }
        [XmlElement(ElementName = "ApplicationAffiliate")]
        public string ApplicationAffiliate { get; set; }
        [XmlElement(ElementName = "ith_email")]
        public string Ith_email { get; set; }
        [XmlElement(ElementName = "IHRMobileNumber")]
        public string IHRMobileNumber { get; set; }
        [XmlElement(ElementName = "PrimaryApplicant")]
        public PrimaryApplicant PrimaryApplicant { get; set; }
        [XmlElement(ElementName = "CoApplicant")]
        public CoApplicant CoApplicant { get; set; }
    }

   
    public class OptionCodeModel
    {
        public string FinancePlanLoanType { get; set; }
        public int OptionCodeID { get; set; }
        public string OptionCode { get; set; }
        public string OptionCodeDescription { get; set; }
    }
    public class IsPcOptionModel
    {
        public int MerchId { get; set; }
        public List<OptionCodeModel> OptionCodeList { get; set; }
        public List<string> Errors { get; set; }
    }

    public class ServiceOptionInput
    {
        public int MerchantId { get; set; }
      
    }
    public class ReportModels
    {
    }

    public class CancelQueue
    {
        public string CsNumber { get; set; }
        public DateTime CancelDate { get; set; }
    }
  
    public class CancelQueueModels
    {
        public List<CancelQueue> CancelQueueList { get; set; }
        public Count TotalCount { get; set; }
    }
    public class BrinksReportCustomer
    {
        public string Account { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime FundingDate { get; set; }
        public double GrossFundingAmmount { get; set; }
    }
    public class BrinksCustomerModels
    {
        public List<BrinksReportCustomer> CustomerList { get; set; }
        public Count TotalCount { get; set; }
    }

    public class FundingVerification
    {
        public DateTime FundingDate { get; set; }
        public string CsNumber { get; set; }
        public string FinanceCompany { get; set; }
        public string PlanCode { get; set; }
        public string NewMMR { get; set; }
        public string LoanAmount { get; set; }
        public string PayOut { get; set; }
    }
    public class FundingVerificationModels
    {
        public List<FundingVerification> FundingVerificationList { get; set; }
        public Count TotalCount { get; set; }
    }
    public class EmployeeTimeClockReportModel
    {
        [DisplayName("Employee Name")]
        public string EmployeeName { set; get; }
        [DisplayName("Clock In/Out")]
        public string Type { set; get; }
        public DateTime Time { set; get; }
        public string Note { set; get; } 
        public int ClockedInMinutes { set; get; }
    }
}
