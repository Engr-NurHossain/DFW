using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class CustomSurveyUser 
	{
        public string UserName { get; set; }
        public string SurveyName { get; set; }
        public string AddedByName { get; set; }

    }

    public partial class DashboardReportDateFilter
    {
        public DateTime StartdateToday { get; set; }
        public DateTime EndDateToday { get; set; }

        public DateTime StartDateYesterday { get; set; }
        public DateTime EndDateYesterday { get; set; }
        public DateTime StartDateThisWeek { get; set; }
        public DateTime EndDateThisWeek { get; set; }

        public DateTime StartDateLastWeek { get; set; }
        public DateTime EndDateLastWeek { get; set; }
        public DateTime StartDateThisMonth { get; set; }
        public DateTime EndDateThisMonth { get; set; }

        public DateTime StartDateLastMonth { get; set; }
        public DateTime EndDateLastMonth { get; set; }
        public DateTime StartDateThisYear { get; set; }
        public DateTime EndDateThisYear { get; set; }

        public DateTime StartDateLastYear { get; set; }
        public DateTime EndDateLastYear { get; set; }
        public string EmpId { get; set; }
    }
    public partial class DashboardFundedReportFilter
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string EmployeeId { get; set; }
    }
    public partial class CustomerDashboardReport
    {
        public int Leads { get; set; }
        public int PriodLeads { get; set; }

        public double RMR { get; set; }
        public double PriorRMR { get; set; }
        public int GoodLeads { get; set; }
        public int PriodGoodLeads { get; set; }
        public int BadLeads { get; set; }
        public int PriodBadLeads { get; set; }
        public int FirstCall { get; set; }
        public int PriodFirstCall { get; set; }
        public double GoodLeadsPercentage { get; set; }
        public double PriodGoodLeadsPercentage { get; set; }
        public double BadLeadsPercentage { get; set; }
        public double PriodBadLeadsPercentage { get; set; }
        public int Sales { get; set; }
        public int PriodSales { get; set; }
        public double GoodSalesPercentage { get; set; }
        public double PriodGoodSalesPercentage { get; set; }
        public double OverallSalesPercentage { get; set; }
        public double PriodOverallSalesPercentage { get; set; }
        public double FirstCallPercentage { get; set; }
        public double PriodFirstCallPercentage { get; set; }
        public int AppointmentSet { get; set; }
        public int PriodAppointmentSet { get; set; }
        public double AptSetPercentage { get; set; }
        public double PriodAptSetPercentage { get; set; }


    }

    public partial class PackageDashboardReport
    {
        public int Sales { get; set; }
        public int PriorSales { get; set; }
        public int Cellular { get; set; }
        public int PriorCellular { get; set; }
        public double CellularPercentage { get; set; }
        public double PriorCellularPercentage { get; set; }
        public double Smart { get; set; }
        public int PriorSmart { get; set; }
        public double SmartPercentage { get; set; }
        public double PriorSmartPercentage { get; set; }

        public double Standard { get; set; }
        public int PriorStandard { get; set; }
        public double StandardPercentage { get; set; }
        public double PriorStandardPercentage { get; set; }

        public double PSP { get; set; }
        public int PriorPSP { get; set; }
        public double PSPPercentage { get; set; }
        public double PriorPSPPercentage { get; set; }

        public double GSP { get; set; }
        public int PriorGSP { get; set; }
        public double GSPPercentage { get; set; }
        public double PriorGSPPercentage { get; set; }
    }

    public partial class PaymentDashboardReport
    {
        public int Sales { get; set; }
        public int PriorSales { get; set; }
        public int TotalUpfront { get; set; }
        public int PriorTotalUpfront { get; set; }
        public int CreditCard { get; set; }
        public int PriorCreditCard { get; set; }
        public int Financed { get; set; }
        public int PriorFinanced { get; set; }

        public double TotalUpfrontAmount { get; set; }
        public double PriorTotalUpfrontAmount { get; set; }
        public double CreditCardAmount { get; set; }
        public double PriorCreditCardAmount { get; set; }
        public double FinancedAmount { get; set; }
        public double PriorFinancedAmount { get; set; }
        public double TotalUpfrontPercentage { get; set; }
        public double PriorTotalUpfrontPercentage { get; set; }
        public double CreditCardPercentage { get; set; }
        public double PriorCreditCardPercentage { get; set; }
        public double FinancedPercentage { get; set; }
        public double PriorFinancedPercentage { get; set; }

        
    }
    public partial class CustomerFundedDashboardReport
    {
        public int Sales { get; set; }
        public int Funded { get; set; }
        public int FundedPercentage { get; set; }
        public string UserX { get; set; }

    }
    public partial class CustomerFinancedReport
    {
        public double RMR { get; set; }
        public int CC { get; set; }
        public double CCAmount { get; set; }
        public double CCPercentage { get; set; }
        public double BadLeadsPercentage { get; set; }
        public int Financed { get; set; }
        public double FinancedPercentage { get; set; }
        public double FinancedAmount { get; set; }
        public double FirstCallPercentage { get; set; }
        public int Upfront { get; set; }
        public double UpfrontPercentage { get; set; }
        public double UpfrontAmount { get; set; }

    }

    public partial class DashboardReportModel
    {
        public CustomerDashboardReportModel reoportList { get; set; }
        public CustomerPackageDashboardReportModel packageReoportList { get; set; }
        public CustomerPaymentDashboardReportModel paymentReoportList { get; set; }
    }
    public partial class CustomerDashboardReportModel
    {
        public CustomerDashboardReport ReportToday { get; set; }
        public CustomerDashboardReport ReportThisWeek { get; set; }
        public CustomerDashboardReport ReportThisMonth { get; set; }
        public CustomerDashboardReport ReportThisYear { get; set; }

    }

    public partial class CustomerPackageDashboardReportModel
    {
        public PackageDashboardReport ReportToday { get; set; }
        public PackageDashboardReport ReportThisWeek { get; set; }
        public PackageDashboardReport ReportThisMonth { get; set; }
        public PackageDashboardReport ReportThisYear { get; set; }

    }
    public partial class CustomerPaymentDashboardReportModel
    {
        public PaymentDashboardReport ReportToday { get; set; }
        public PaymentDashboardReport ReportThisWeek { get; set; }
        public PaymentDashboardReport ReportThisMonth { get; set; }
        public PaymentDashboardReport ReportThisYear { get; set; }

    }
    public partial class CustomerFundedDashboardReportModel
    {
        public CustomerFundedDashboardReport ReportToday { get; set; }
        public CustomerFundedDashboardReport ReportThisWeek { get; set; }
        public CustomerFundedDashboardReport ReportThisMonth { get; set; }
        public CustomerFundedDashboardReport ReportThisYear { get; set; }

    }
    public partial class CustomerFinancedReportModel
    {
        public CustomerFinancedReport ReportToday { get; set; }
        public CustomerFinancedReport ReportThisWeek { get; set; }
        public CustomerFinancedReport ReportThisMonth { get; set; }
        public CustomerFinancedReport ReportThisYear { get; set; }

    }
}
