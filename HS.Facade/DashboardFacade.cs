using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Facade
{
    public class DashboardFacade : BaseFacade
    {
        public DashboardFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        InvoiceDataAccess _InvoiceDataAccess
        {
            get
            {
                return (InvoiceDataAccess)_ClientContext[typeof(InvoiceDataAccess)];
            }
        }
        CustomerDataAccess _CustomerDataAccess
        {
            get
            {
                return (CustomerDataAccess)_ClientContext[typeof(CustomerDataAccess)];
            }
        }
        CustomerNoteDataAccess _CustomerNoteDataAccess
        {
            get
            {
                return (CustomerNoteDataAccess)_ClientContext[typeof(CustomerNoteDataAccess)];
            }
        }

        public DashboardModel GetDashBoardData(Guid CompanyId, string tag, Guid empid, string firstdate, string lastdate, string previousfirstdate, string AccountOwnerId, bool orderpermit)
        {
            DataTable dt = new DataTable();
            bool isNew = false;
            if(isNew)
            {
                  dt = _InvoiceDataAccess.GetDashBoardDataNew(CompanyId, tag, empid, firstdate, lastdate, previousfirstdate, AccountOwnerId, orderpermit);
            }
            else
            {
                  dt = _InvoiceDataAccess.GetDashBoardData(CompanyId, tag, empid, firstdate, lastdate, previousfirstdate, AccountOwnerId, orderpermit);
            }
            //DataTable dt = _InvoiceDataAccess.GetDashBoardData(CompanyId, tag, empid, firstdate, lastdate, previousfirstdate, AccountOwnerId, orderpermit);
            List<DashboardModel> DashBoardModel = new List<DashboardModel>();
            if (dt != null)
                DashBoardModel = (from DataRow dr in dt.Rows
                                  select new DashboardModel()
                                  {
                                      EstimateCount = dr["EstimateCount"] != DBNull.Value ? Convert.ToInt32(dr["EstimateCount"]) : 0,
                                      InvoiceCount = dr["InvoiceCount"] != DBNull.Value ? Convert.ToInt32(dr["InvoiceCount"]) : 0,
                                      CountMMR = dr["CountMMR"] != DBNull.Value ? Convert.ToInt32(dr["CountMMR"]) : 0,
                                      FirstMonthLeadCount = dr["FirstMonthLeadCount"] != DBNull.Value ? Convert.ToInt32(dr["FirstMonthLeadCount"]) : 0,
                                      LastMonthLeadCount = dr["LastMonthLeadCount"] != DBNull.Value ? Convert.ToInt32(dr["LastMonthLeadCount"]) : 0,

                                      FirstMonthActivitiesCount = dr["FirstMonthActivitiesCount"] != DBNull.Value ? Convert.ToInt32(dr["FirstMonthActivitiesCount"]) : 0,
                                      LastMonthActivitiesCount = dr["LastMonthActivitiesCount"] != DBNull.Value ? Convert.ToInt32(dr["LastMonthActivitiesCount"]) : 0,
                                      FirstMonthOpportunitiesCount = dr["FirstMonthOpportunitiesCount"] != DBNull.Value ? Convert.ToInt32(dr["FirstMonthOpportunitiesCount"]) : 0,
                                      LastMonthOpportunitiesCount = dr["LastMonthOpportunitiesCount"] != DBNull.Value ? Convert.ToInt32(dr["LastMonthOpportunitiesCount"]) : 0,

                                      MMRCount = dr["MMRCount"] != DBNull.Value ? Convert.ToDouble(dr["MMRCount"]) : 0.0,
                                      FirstMonthCustomerCount = dr["FirstMonthCustomerCount"] != DBNull.Value ? Convert.ToInt32(dr["FirstMonthCustomerCount"]) : 0,
                                      LastMonthCustomerCount = dr["LastMonthCustomerCount"] != DBNull.Value ? Convert.ToInt32(dr["LastMonthCustomerCount"]) : 0,
                                      //TotalPaid = dr["TotalPaid"] != DBNull.Value ? Convert.ToDouble(dr["TotalPaid"]) : 0,
                                      TotalRevenue = dr["TotalRevenue"] != DBNull.Value ? Convert.ToDouble(dr["TotalRevenue"]) : 0,
                                      EstimateAmount = dr["EstimateAmount"] != DBNull.Value ? Convert.ToDouble(dr["EstimateAmount"]) : 0.0,
                                      InvoiceAmount = dr["InvoiceAmount"] != DBNull.Value ? Convert.ToDouble(dr["InvoiceAmount"]) : 0.0,
                                      UnpaidAmount = dr["UnpaidAmount"] != DBNull.Value ? Convert.ToDouble(dr["UnpaidAmount"]) : 0.0,
                                      UnpaidCount = dr["UnpaidCount"] != DBNull.Value ? Convert.ToInt32(dr["UnpaidCount"]) : 0,
                                      TotaltTransactions = dr["TotaltTransactions"] != DBNull.Value ? Convert.ToInt32(dr["TotaltTransactions"]) : 0,
                                      FirstMonthOrder = dr["FirstMonthOrder"] != DBNull.Value ? Convert.ToInt32(dr["FirstMonthOrder"]) : 0,
                                      LastMonthOrder = dr["LastMonthOrder"] != DBNull.Value ? Convert.ToInt32(dr["LastMonthOrder"]) : 0,
                                      FirstMonthRevenueCount = dr["FirstMonthRevenueCount"] != DBNull.Value ? Convert.ToDouble(dr["FirstMonthRevenueCount"]) : 0.0,
                                      LastMonthRevenueCount = dr["LastMonthRevenueCount"] != DBNull.Value ? Convert.ToDouble(dr["LastMonthRevenueCount"]) : 0.0,
                                  }).ToList();
            return DashBoardModel.FirstOrDefault();
        }
        public DashboardModelTechnician GetDashBoardDataTechnician(Guid CompanyId, Guid empid)
        {
            DataTable dt = _InvoiceDataAccess.GetDashBoardDataTechnician(CompanyId, empid);
            List<DashboardModelTechnician> DashBoardModel = new List<DashboardModelTechnician>();
            if (dt != null)
                DashBoardModel = (from DataRow dr in dt.Rows
                                  select new DashboardModelTechnician()
                                  {
                                      OpenInstallationTicket = dr["OpenInstallationTicket"] != DBNull.Value ? Convert.ToInt32(dr["OpenInstallationTicket"]) : 0,
                                      OpenServiceTicket = dr["OpenServiceTicket"] != DBNull.Value ? Convert.ToInt32(dr["OpenServiceTicket"]) : 0,
                                      ClosedInstallationTicket = dr["ClosedInstallationTicket"] != DBNull.Value ? Convert.ToInt32(dr["ClosedInstallationTicket"]) : 0,
                                      ClosedServiceTicket = dr["ClosedServiceTicket"] != DBNull.Value ? Convert.ToInt32(dr["ClosedServiceTicket"]) : 0,
                                      OpenPickTicket = dr["OpenPickTicket"] != DBNull.Value ? Convert.ToInt32(dr["OpenPickTicket"]) : 0,
                                      ClosedPickTicket = dr["ClosedPickTicket"] != DBNull.Value ? Convert.ToInt32(dr["ClosedPickTicket"]) : 0,
                                      OpenDropTicket = dr["OpenDropTicket"] != DBNull.Value ? Convert.ToInt32(dr["OpenDropTicket"]) : 0,
                                      ClosedDropTicket = dr["ClosedDropTicket"] != DBNull.Value ? Convert.ToInt32(dr["ClosedDropTicket"]) : 0,
                                  }).ToList();
            return DashBoardModel.FirstOrDefault();
        }

        public CustomerDashboardReportModel GetCustomerDashboardReprot(DashboardReportDateFilter filter)
        {
            DataSet ds = _CustomerDataAccess.GetCustomerDashboardReprot(filter);
            //DataSet ds = _CustomerDataAccess.GetCustomerDashboardReprotNew(filter);
            List<CustomerDashboardReport> reportToday = new List<CustomerDashboardReport>();
            List<CustomerDashboardReport> reportThisWeek = new List<CustomerDashboardReport>();
            List<CustomerDashboardReport> reportThisMonth = new List<CustomerDashboardReport>();
            List<CustomerDashboardReport> reportThisYear = new List<CustomerDashboardReport>();
            reportToday = (from DataRow dr in ds.Tables[0].Rows
                              select new CustomerDashboardReport()
                              {
                                 
                                  Leads = dr["Leads"] != DBNull.Value ? Convert.ToInt32(dr["Leads"]) : 0,
                                  PriodLeads = dr["PriodLeads"] != DBNull.Value ? Convert.ToInt32(dr["PriodLeads"]) : 0,
                                  RMR = dr["RMR"] != DBNull.Value ? Convert.ToDouble(dr["RMR"]) : 0.0,
                                  PriorRMR = dr["PriorRMR"] != DBNull.Value ? Convert.ToDouble(dr["PriorRMR"]) : 0.0,
                                  Sales = dr["Sales"] != DBNull.Value ? Convert.ToInt32(dr["Sales"]) : 0,
                                  PriodSales = dr["PriodSales"] != DBNull.Value ? Convert.ToInt32(dr["PriodSales"]) : 0,
                                  GoodLeads = dr["Good"] != DBNull.Value ? Convert.ToInt32(dr["Good"]) : 0,
                                  BadLeads = dr["Remove"] != DBNull.Value ? Convert.ToInt32(dr["Remove"]) : 0,
                                  PriodGoodLeads = dr["PriodGood"] != DBNull.Value ? Convert.ToInt32(dr["PriodGood"]) : 0,
                                  PriodBadLeads = dr["PriodRemove"] != DBNull.Value ? Convert.ToInt32(dr["PriodRemove"]) : 0,

                                  GoodLeadsPercentage = dr["GoodLeadsPercentage"] != DBNull.Value ? Convert.ToDouble(dr["GoodLeadsPercentage"]) : 0.0,
                                  PriodGoodLeadsPercentage = dr["PriodGoodLeadsPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriodGoodLeadsPercentage"]) : 0.0,
                                  BadLeadsPercentage = dr["BadLeadsPercentage"] != DBNull.Value ? Convert.ToDouble(dr["BadLeadsPercentage"]) : 0.0,
                                  GoodSalesPercentage = dr["GoodSalesPercentage"] != DBNull.Value ? Convert.ToDouble(dr["GoodSalesPercentage"]) : 0.0,
                                  OverallSalesPercentage = dr["OverallSalesPercentage"] != DBNull.Value ? Convert.ToDouble(dr["OverallSalesPercentage"]) : 0.0,
                                 
                                  PriodBadLeadsPercentage = dr["PriodBadLeadsPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriodBadLeadsPercentage"]) : 0.0,
                                  PriodGoodSalesPercentage = dr["PriodGoodSalesPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriodGoodSalesPercentage"]) : 0.0,
                                  PriodOverallSalesPercentage = dr["PriodOverallSalesPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriodOverallSalesPercentage"]) : 0.0,

                                  FirstCall = dr["FirstCall"] != DBNull.Value ? Convert.ToInt32(dr["FirstCall"]) : 0,
                                  PriodFirstCall = dr["PriodFirstCall"] != DBNull.Value ? Convert.ToInt32(dr["PriodFirstCall"]) : 0,
                                  AppointmentSet = dr["AptSet"] != DBNull.Value ? Convert.ToInt32(dr["AptSet"]) : 0,
                                  PriodAppointmentSet = dr["PriodAptSet"] != DBNull.Value ? Convert.ToInt32(dr["PriodAptSet"]) : 0,
                                  FirstCallPercentage = dr["FirstCallPercentage"] != DBNull.Value ? Convert.ToDouble(dr["FirstCallPercentage"]) : 0.0,
                                  PriodFirstCallPercentage = dr["PriodFirstCallPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriodFirstCallPercentage"]) : 0.0,
                                  AptSetPercentage = dr["AptSetPercentage"] != DBNull.Value ? Convert.ToDouble(dr["AptSetPercentage"]) : 0.0,
                                  PriodAptSetPercentage = dr["PriodAptSetPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriodAptSetPercentage"]) : 0.0

                              }).ToList();

            reportThisWeek = (from DataRow dr in ds.Tables[1].Rows
                           select new CustomerDashboardReport()
                           {

                               Leads = dr["Leads"] != DBNull.Value ? Convert.ToInt32(dr["Leads"]) : 0,
                               PriodLeads = dr["PriodLeads"] != DBNull.Value ? Convert.ToInt32(dr["PriodLeads"]) : 0,
                               Sales = dr["Sales"] != DBNull.Value ? Convert.ToInt32(dr["Sales"]) : 0,
                               PriodSales = dr["PriodSales"] != DBNull.Value ? Convert.ToInt32(dr["PriodSales"]) : 0,
                               GoodLeads = dr["Good"] != DBNull.Value ? Convert.ToInt32(dr["Good"]) : 0,
                               BadLeads = dr["Remove"] != DBNull.Value ? Convert.ToInt32(dr["Remove"]) : 0,
                               PriodGoodLeads = dr["PriodGood"] != DBNull.Value ? Convert.ToInt32(dr["PriodGood"]) : 0,
                               PriodBadLeads = dr["PriodRemove"] != DBNull.Value ? Convert.ToInt32(dr["PriodRemove"]) : 0,
                               RMR = dr["RMR"] != DBNull.Value ? Convert.ToDouble(dr["RMR"]) : 0.0,
                               PriorRMR = dr["PriorRMR"] != DBNull.Value ? Convert.ToDouble(dr["PriorRMR"]) : 0.0,

                               GoodLeadsPercentage = dr["GoodLeadsPercentage"] != DBNull.Value ? Convert.ToDouble(dr["GoodLeadsPercentage"]) : 0.0,
                               PriodGoodLeadsPercentage = dr["PriodGoodLeadsPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriodGoodLeadsPercentage"]) : 0.0,
                               BadLeadsPercentage = dr["BadLeadsPercentage"] != DBNull.Value ? Convert.ToDouble(dr["BadLeadsPercentage"]) : 0.0,
                               GoodSalesPercentage = dr["GoodSalesPercentage"] != DBNull.Value ? Convert.ToDouble(dr["GoodSalesPercentage"]) : 0.0,
                               OverallSalesPercentage = dr["OverallSalesPercentage"] != DBNull.Value ? Convert.ToDouble(dr["OverallSalesPercentage"]) : 0.0,

                               PriodBadLeadsPercentage = dr["PriodBadLeadsPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriodBadLeadsPercentage"]) : 0.0,
                               PriodGoodSalesPercentage = dr["PriodGoodSalesPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriodGoodSalesPercentage"]) : 0.0,
                               PriodOverallSalesPercentage = dr["PriodOverallSalesPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriodOverallSalesPercentage"]) : 0.0,

                               FirstCall = dr["FirstCall"] != DBNull.Value ? Convert.ToInt32(dr["FirstCall"]) : 0,
                               PriodFirstCall = dr["PriodFirstCall"] != DBNull.Value ? Convert.ToInt32(dr["PriodFirstCall"]) : 0,
                               AppointmentSet = dr["AptSet"] != DBNull.Value ? Convert.ToInt32(dr["AptSet"]) : 0,
                               PriodAppointmentSet = dr["PriodAptSet"] != DBNull.Value ? Convert.ToInt32(dr["PriodAptSet"]) : 0,
                               FirstCallPercentage = dr["FirstCallPercentage"] != DBNull.Value ? Convert.ToDouble(dr["FirstCallPercentage"]) : 0.0,
                               PriodFirstCallPercentage = dr["PriodFirstCallPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriodFirstCallPercentage"]) : 0.0,
                               AptSetPercentage = dr["AptSetPercentage"] != DBNull.Value ? Convert.ToDouble(dr["AptSetPercentage"]) : 0.0,
                               PriodAptSetPercentage = dr["PriodAptSetPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriodAptSetPercentage"]) : 0.0
                           }).ToList();

            reportThisMonth = (from DataRow dr in ds.Tables[2].Rows
                           select new CustomerDashboardReport()
                           {

                               Leads = dr["Leads"] != DBNull.Value ? Convert.ToInt32(dr["Leads"]) : 0,
                               PriodLeads = dr["PriodLeads"] != DBNull.Value ? Convert.ToInt32(dr["PriodLeads"]) : 0,
                               Sales = dr["Sales"] != DBNull.Value ? Convert.ToInt32(dr["Sales"]) : 0,
                               PriodSales = dr["PriodSales"] != DBNull.Value ? Convert.ToInt32(dr["PriodSales"]) : 0,
                               RMR = dr["RMR"] != DBNull.Value ? Convert.ToDouble(dr["RMR"]) : 0.0,
                               PriorRMR = dr["PriorRMR"] != DBNull.Value ? Convert.ToDouble(dr["PriorRMR"]) : 0.0,
                               GoodLeads = dr["Good"] != DBNull.Value ? Convert.ToInt32(dr["Good"]) : 0,
                               BadLeads = dr["Remove"] != DBNull.Value ? Convert.ToInt32(dr["Remove"]) : 0,
                               PriodGoodLeads = dr["PriodGood"] != DBNull.Value ? Convert.ToInt32(dr["PriodGood"]) : 0,
                               PriodBadLeads = dr["PriodRemove"] != DBNull.Value ? Convert.ToInt32(dr["PriodRemove"]) : 0,

                               GoodLeadsPercentage = dr["GoodLeadsPercentage"] != DBNull.Value ? Convert.ToDouble(dr["GoodLeadsPercentage"]) : 0.0,
                               PriodGoodLeadsPercentage = dr["PriodGoodLeadsPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriodGoodLeadsPercentage"]) : 0.0,
                               BadLeadsPercentage = dr["BadLeadsPercentage"] != DBNull.Value ? Convert.ToDouble(dr["BadLeadsPercentage"]) : 0.0,
                               GoodSalesPercentage = dr["GoodSalesPercentage"] != DBNull.Value ? Convert.ToDouble(dr["GoodSalesPercentage"]) : 0.0,
                               OverallSalesPercentage = dr["OverallSalesPercentage"] != DBNull.Value ? Convert.ToDouble(dr["OverallSalesPercentage"]) : 0.0,

                               PriodBadLeadsPercentage = dr["PriodBadLeadsPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriodBadLeadsPercentage"]) : 0.0,
                               PriodGoodSalesPercentage = dr["PriodGoodSalesPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriodGoodSalesPercentage"]) : 0.0,
                               PriodOverallSalesPercentage = dr["PriodOverallSalesPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriodOverallSalesPercentage"]) : 0.0,

                               FirstCall = dr["FirstCall"] != DBNull.Value ? Convert.ToInt32(dr["FirstCall"]) : 0,
                               PriodFirstCall = dr["PriodFirstCall"] != DBNull.Value ? Convert.ToInt32(dr["PriodFirstCall"]) : 0,
                               AppointmentSet = dr["AptSet"] != DBNull.Value ? Convert.ToInt32(dr["AptSet"]) : 0,
                               PriodAppointmentSet = dr["PriodAptSet"] != DBNull.Value ? Convert.ToInt32(dr["PriodAptSet"]) : 0,
                               FirstCallPercentage = dr["FirstCallPercentage"] != DBNull.Value ? Convert.ToDouble(dr["FirstCallPercentage"]) : 0.0,
                               PriodFirstCallPercentage = dr["PriodFirstCallPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriodFirstCallPercentage"]) : 0.0,
                               AptSetPercentage = dr["AptSetPercentage"] != DBNull.Value ? Convert.ToDouble(dr["AptSetPercentage"]) : 0.0,
                               PriodAptSetPercentage = dr["PriodAptSetPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriodAptSetPercentage"]) : 0.0
                           }).ToList();
            reportThisYear = (from DataRow dr in ds.Tables[3].Rows
                               select new CustomerDashboardReport()
                               {

                                   Leads = dr["Leads"] != DBNull.Value ? Convert.ToInt32(dr["Leads"]) : 0,
                                   PriodLeads = dr["PriodLeads"] != DBNull.Value ? Convert.ToInt32(dr["PriodLeads"]) : 0,
                                   Sales = dr["Sales"] != DBNull.Value ? Convert.ToInt32(dr["Sales"]) : 0,
                                   PriodSales = dr["PriodSales"] != DBNull.Value ? Convert.ToInt32(dr["PriodSales"]) : 0,
                                   GoodLeads = dr["Good"] != DBNull.Value ? Convert.ToInt32(dr["Good"]) : 0,
                                   BadLeads = dr["Remove"] != DBNull.Value ? Convert.ToInt32(dr["Remove"]) : 0,
                                   PriodGoodLeads = dr["PriodGood"] != DBNull.Value ? Convert.ToInt32(dr["PriodGood"]) : 0,
                                   PriodBadLeads = dr["PriodRemove"] != DBNull.Value ? Convert.ToInt32(dr["PriodRemove"]) : 0,
                                   RMR = dr["RMR"] != DBNull.Value ? Convert.ToDouble(dr["RMR"]) : 0.0,
                                   PriorRMR = dr["PriorRMR"] != DBNull.Value ? Convert.ToDouble(dr["PriorRMR"]) : 0.0,

                                   GoodLeadsPercentage = dr["GoodLeadsPercentage"] != DBNull.Value ? Convert.ToDouble(dr["GoodLeadsPercentage"]) : 0.0,
                                   PriodGoodLeadsPercentage = dr["PriodGoodLeadsPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriodGoodLeadsPercentage"]) : 0.0,
                                   BadLeadsPercentage = dr["BadLeadsPercentage"] != DBNull.Value ? Convert.ToDouble(dr["BadLeadsPercentage"]) : 0.0,
                                   GoodSalesPercentage = dr["GoodSalesPercentage"] != DBNull.Value ? Convert.ToDouble(dr["GoodSalesPercentage"]) : 0.0,
                                   OverallSalesPercentage = dr["OverallSalesPercentage"] != DBNull.Value ? Convert.ToDouble(dr["OverallSalesPercentage"]) : 0.0,

                                   PriodBadLeadsPercentage = dr["PriodBadLeadsPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriodBadLeadsPercentage"]) : 0.0,
                                   PriodGoodSalesPercentage = dr["PriodGoodSalesPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriodGoodSalesPercentage"]) : 0.0,
                                   PriodOverallSalesPercentage = dr["PriodOverallSalesPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriodOverallSalesPercentage"]) : 0.0,

                                   FirstCall = dr["FirstCall"] != DBNull.Value ? Convert.ToInt32(dr["FirstCall"]) : 0,
                                   PriodFirstCall = dr["PriodFirstCall"] != DBNull.Value ? Convert.ToInt32(dr["PriodFirstCall"]) : 0,
                                   AppointmentSet = dr["AptSet"] != DBNull.Value ? Convert.ToInt32(dr["AptSet"]) : 0,
                                   PriodAppointmentSet = dr["PriodAptSet"] != DBNull.Value ? Convert.ToInt32(dr["PriodAptSet"]) : 0,
                                   FirstCallPercentage = dr["FirstCallPercentage"] != DBNull.Value ? Convert.ToDouble(dr["FirstCallPercentage"]) : 0.0,
                                   PriodFirstCallPercentage = dr["PriodFirstCallPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriodFirstCallPercentage"]) : 0.0,
                                   AptSetPercentage = dr["AptSetPercentage"] != DBNull.Value ? Convert.ToDouble(dr["AptSetPercentage"]) : 0.0,
                                   PriodAptSetPercentage = dr["PriodAptSetPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriodAptSetPercentage"]) : 0.0
                               }).ToList();

            CustomerDashboardReportModel model = new CustomerDashboardReportModel();
            model.ReportToday = reportToday.FirstOrDefault();
            model.ReportThisWeek = reportThisWeek.FirstOrDefault();
            model.ReportThisMonth = reportThisMonth.FirstOrDefault();
            model.ReportThisYear = reportThisYear.FirstOrDefault();

            return model;
        }

        public CustomerFundedDashboardReport GetCustomerFundedReprot(DateTime StartDate, DateTime EndDate, string EmployeeId)
        {
            DataSet ds = _CustomerDataAccess.GetCustomerFundedDashboardReprot(StartDate, EndDate, EmployeeId);
            CustomerFundedDashboardReport fundedModel = new CustomerFundedDashboardReport();

            fundedModel = (from DataRow dr in ds.Tables[0].Rows
                           select new CustomerFundedDashboardReport()
                           {

                               Funded = dr["Funded"] != DBNull.Value ? Convert.ToInt32(dr["Funded"]) : 0,
                               FundedPercentage = dr["FundedPercentage"] != DBNull.Value ? Convert.ToInt32(dr["FundedPercentage"]) : 0,
                               Sales = dr["Sales"] != DBNull.Value ? Convert.ToInt32(dr["Sales"]) : 0,
                               UserX = dr["UserX"].ToString(),
                              
                           }).ToList().FirstOrDefault();

   

            return fundedModel;
        }

        public CustomerPackageDashboardReportModel GetPackageDashboardReprot(DashboardReportDateFilter filter)
        {
            DataSet ds = _CustomerDataAccess.GetPackageDashboardReprot(filter);
            List<PackageDashboardReport> reportToday = new List<PackageDashboardReport>();
            List<PackageDashboardReport> reportThisWeek = new List<PackageDashboardReport>();
            List<PackageDashboardReport> reportThisMonth = new List<PackageDashboardReport>();
            List<PackageDashboardReport> reportThisYear = new List<PackageDashboardReport>();
            reportToday = (from DataRow dr in ds.Tables[0].Rows
                           select new PackageDashboardReport()
                           {

                               Cellular = dr["Cellular"] != DBNull.Value ? Convert.ToInt32(dr["Cellular"]) : 0,
                               PriorCellular = dr["PriorCellular"] != DBNull.Value ? Convert.ToInt32(dr["PriorCellular"]) : 0,
                               Sales = dr["Sales"] != DBNull.Value ? Convert.ToInt32(dr["Sales"]) : 0,
                               PriorSales = dr["PriorSales"] != DBNull.Value ? Convert.ToInt32(dr["PriorSales"]) : 0,
                               CellularPercentage = dr["CellularPercentage"] != DBNull.Value ? Convert.ToDouble(dr["CellularPercentage"]) : 0.0,
                               PriorCellularPercentage = dr["PriorCellularPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorCellularPercentage"]) : 0,

                               Smart = dr["Smart"] != DBNull.Value ? Convert.ToInt32(dr["Smart"]) : 0,
                               PriorSmart = dr["PriorSmart"] != DBNull.Value ? Convert.ToInt32(dr["PriorSmart"]) : 0,
                               SmartPercentage = dr["SmartPercentage"] != DBNull.Value ? Convert.ToDouble(dr["SmartPercentage"]) : 0.0,
                               PriorSmartPercentage = dr["PriorSmartPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorSmartPercentage"]) : 0,

                               Standard = dr["Standard"] != DBNull.Value ? Convert.ToInt32(dr["Standard"]) : 0,
                               PriorStandard = dr["PriorStandard"] != DBNull.Value ? Convert.ToInt32(dr["PriorStandard"]) : 0,
                               StandardPercentage = dr["StandardPercentage"] != DBNull.Value ? Convert.ToDouble(dr["StandardPercentage"]) : 0.0,
                               PriorStandardPercentage = dr["PriorStandardPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorStandardPercentage"]) : 0,

                               PSP = dr["PSP"] != DBNull.Value ? Convert.ToInt32(dr["PSP"]) : 0,
                               PriorPSP = dr["PriorPSP"] != DBNull.Value ? Convert.ToInt32(dr["PriorPSP"]) : 0,
                               PSPPercentage = dr["PSPPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PSPPercentage"]) : 0.0,
                               PriorPSPPercentage = dr["PriorPSPPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorPSPPercentage"]) : 0,


                               GSP = dr["GSP"] != DBNull.Value ? Convert.ToInt32(dr["GSP"]) : 0,
                               PriorGSP = dr["PriorGSP"] != DBNull.Value ? Convert.ToInt32(dr["PriorGSP"]) : 0,
                               GSPPercentage = dr["GSPPercentage"] != DBNull.Value ? Convert.ToDouble(dr["GSPPercentage"]) : 0.0,
                               PriorGSPPercentage = dr["PriorGSPPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorGSPPercentage"]) : 0,


                           }).ToList();

            reportThisWeek = (from DataRow dr in ds.Tables[1].Rows
                              select new PackageDashboardReport()
                              {

                                  Cellular = dr["Cellular"] != DBNull.Value ? Convert.ToInt32(dr["Cellular"]) : 0,
                                  PriorCellular = dr["PriorCellular"] != DBNull.Value ? Convert.ToInt32(dr["PriorCellular"]) : 0,
                                  Sales = dr["Sales"] != DBNull.Value ? Convert.ToInt32(dr["Sales"]) : 0,
                                  PriorSales = dr["PriorSales"] != DBNull.Value ? Convert.ToInt32(dr["PriorSales"]) : 0,
                                  CellularPercentage = dr["CellularPercentage"] != DBNull.Value ? Convert.ToDouble(dr["CellularPercentage"]) : 0,
                                  PriorCellularPercentage = dr["PriorCellularPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorCellularPercentage"]) : 0,

                                  Smart = dr["Smart"] != DBNull.Value ? Convert.ToInt32(dr["Smart"]) : 0,
                                  PriorSmart = dr["PriorSmart"] != DBNull.Value ? Convert.ToInt32(dr["PriorSmart"]) : 0,
                                  SmartPercentage = dr["SmartPercentage"] != DBNull.Value ? Convert.ToDouble(dr["SmartPercentage"]) : 0.0,
                                  PriorSmartPercentage = dr["PriorSmartPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorSmartPercentage"]) : 0,
                                  
                                  PSP = dr["PSP"] != DBNull.Value ? Convert.ToInt32(dr["PSP"]) : 0,
                                  PriorPSP = dr["PriorPSP"] != DBNull.Value ? Convert.ToInt32(dr["PriorPSP"]) : 0,
                                  PSPPercentage = dr["PSPPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PSPPercentage"]) : 0.0,
                                  PriorPSPPercentage = dr["PriorPSPPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorPSPPercentage"]) : 0,

                                  Standard = dr["Standard"] != DBNull.Value ? Convert.ToInt32(dr["Standard"]) : 0,
                                  PriorStandard = dr["PriorStandard"] != DBNull.Value ? Convert.ToInt32(dr["PriorStandard"]) : 0,
                                  StandardPercentage = dr["StandardPercentage"] != DBNull.Value ? Convert.ToDouble(dr["StandardPercentage"]) : 0.0,
                                  PriorStandardPercentage = dr["PriorStandardPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorStandardPercentage"]) : 0,

                                  GSP = dr["GSP"] != DBNull.Value ? Convert.ToInt32(dr["GSP"]) : 0,
                                  PriorGSP = dr["PriorGSP"] != DBNull.Value ? Convert.ToInt32(dr["PriorGSP"]) : 0,
                                  GSPPercentage = dr["GSPPercentage"] != DBNull.Value ? Convert.ToDouble(dr["GSPPercentage"]) : 0.0,
                                  PriorGSPPercentage = dr["PriorGSPPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorGSPPercentage"]) : 0,
                              }).ToList();

            reportThisMonth = (from DataRow dr in ds.Tables[2].Rows
                               select new PackageDashboardReport()
                               {

                                   Cellular = dr["Cellular"] != DBNull.Value ? Convert.ToInt32(dr["Cellular"]) : 0,
                                   PriorCellular = dr["PriorCellular"] != DBNull.Value ? Convert.ToInt32(dr["PriorCellular"]) : 0,
                                   Sales = dr["Sales"] != DBNull.Value ? Convert.ToInt32(dr["Sales"]) : 0,
                                   PriorSales = dr["PriorSales"] != DBNull.Value ? Convert.ToInt32(dr["PriorSales"]) : 0,
                                   CellularPercentage = dr["CellularPercentage"] != DBNull.Value ? Convert.ToDouble(dr["CellularPercentage"]) : 0,
                                   PriorCellularPercentage = dr["PriorCellularPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorCellularPercentage"]) : 0,

                                   Smart = dr["Smart"] != DBNull.Value ? Convert.ToInt32(dr["Smart"]) : 0,
                                   PriorSmart = dr["PriorSmart"] != DBNull.Value ? Convert.ToInt32(dr["PriorSmart"]) : 0,
                                   SmartPercentage = dr["SmartPercentage"] != DBNull.Value ? Convert.ToDouble(dr["SmartPercentage"]) : 0.0,
                                   PriorSmartPercentage = dr["PriorSmartPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorSmartPercentage"]) : 0,

                                   PSP = dr["PSP"] != DBNull.Value ? Convert.ToInt32(dr["PSP"]) : 0,
                                   PriorPSP = dr["PriorPSP"] != DBNull.Value ? Convert.ToInt32(dr["PriorPSP"]) : 0,
                                   PSPPercentage = dr["PSPPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PSPPercentage"]) : 0.0,
                                   PriorPSPPercentage = dr["PriorPSPPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorPSPPercentage"]) : 0,

                                   Standard = dr["Standard"] != DBNull.Value ? Convert.ToInt32(dr["Standard"]) : 0,
                                   PriorStandard = dr["PriorStandard"] != DBNull.Value ? Convert.ToInt32(dr["PriorStandard"]) : 0,
                                   StandardPercentage = dr["StandardPercentage"] != DBNull.Value ? Convert.ToDouble(dr["StandardPercentage"]) : 0.0,
                                   PriorStandardPercentage = dr["PriorStandardPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorStandardPercentage"]) : 0,

                                   GSP = dr["GSP"] != DBNull.Value ? Convert.ToInt32(dr["GSP"]) : 0,
                                   PriorGSP = dr["PriorGSP"] != DBNull.Value ? Convert.ToInt32(dr["PriorGSP"]) : 0,
                                   GSPPercentage = dr["GSPPercentage"] != DBNull.Value ? Convert.ToDouble(dr["GSPPercentage"]) : 0.0,
                                   PriorGSPPercentage = dr["PriorGSPPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorGSPPercentage"]) : 0,
                               }).ToList();
            reportThisYear = (from DataRow dr in ds.Tables[3].Rows
                              select new PackageDashboardReport()
                              {

                                  Cellular = dr["Cellular"] != DBNull.Value ? Convert.ToInt32(dr["Cellular"]) : 0,
                                  PriorCellular = dr["PriorCellular"] != DBNull.Value ? Convert.ToInt32(dr["PriorCellular"]) : 0,
                                  Sales = dr["Sales"] != DBNull.Value ? Convert.ToInt32(dr["Sales"]) : 0,
                                  PriorSales = dr["PriorSales"] != DBNull.Value ? Convert.ToInt32(dr["PriorSales"]) : 0,
                                  CellularPercentage = dr["CellularPercentage"] != DBNull.Value ? Convert.ToDouble(dr["CellularPercentage"]) : 0,
                                  PriorCellularPercentage = dr["PriorCellularPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorCellularPercentage"]) : 0,

                                  Smart = dr["Smart"] != DBNull.Value ? Convert.ToInt32(dr["Smart"]) : 0,
                                  PriorSmart = dr["PriorSmart"] != DBNull.Value ? Convert.ToInt32(dr["PriorSmart"]) : 0,
                                  SmartPercentage = dr["SmartPercentage"] != DBNull.Value ? Convert.ToDouble(dr["SmartPercentage"]) : 0.0,
                                  PriorSmartPercentage = dr["PriorSmartPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorSmartPercentage"]) : 0,

                                  PSP = dr["PSP"] != DBNull.Value ? Convert.ToInt32(dr["PSP"]) : 0,
                                  PriorPSP = dr["PriorPSP"] != DBNull.Value ? Convert.ToInt32(dr["PriorPSP"]) : 0,
                                  PSPPercentage = dr["PSPPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PSPPercentage"]) : 0.0,
                                  PriorPSPPercentage = dr["PriorPSPPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorPSPPercentage"]) : 0,

                                  Standard = dr["Standard"] != DBNull.Value ? Convert.ToInt32(dr["Standard"]) : 0,
                                  PriorStandard = dr["PriorStandard"] != DBNull.Value ? Convert.ToInt32(dr["PriorStandard"]) : 0,
                                  StandardPercentage = dr["StandardPercentage"] != DBNull.Value ? Convert.ToDouble(dr["StandardPercentage"]) : 0.0,
                                  PriorStandardPercentage = dr["PriorStandardPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorStandardPercentage"]) : 0,

                                  GSP = dr["GSP"] != DBNull.Value ? Convert.ToInt32(dr["GSP"]) : 0,
                                  PriorGSP = dr["PriorGSP"] != DBNull.Value ? Convert.ToInt32(dr["PriorGSP"]) : 0,
                                  GSPPercentage = dr["GSPPercentage"] != DBNull.Value ? Convert.ToDouble(dr["GSPPercentage"]) : 0.0,
                                  PriorGSPPercentage = dr["PriorGSPPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorGSPPercentage"]) : 0,
                              }).ToList();

            CustomerPackageDashboardReportModel model = new CustomerPackageDashboardReportModel();
            model.ReportToday = reportToday.FirstOrDefault();
            model.ReportThisWeek = reportThisWeek.FirstOrDefault();
            model.ReportThisMonth = reportThisMonth.FirstOrDefault();
            model.ReportThisYear = reportThisYear.FirstOrDefault();

            return model;
        }

        public CustomerPaymentDashboardReportModel GetPaymentDashboardReprot(DashboardReportDateFilter filter)
        {
            DataSet ds = _CustomerDataAccess.GetPaymentDashboardReprot(filter);
            List<PaymentDashboardReport> reportToday = new List<PaymentDashboardReport>();
            List<PaymentDashboardReport> reportThisWeek = new List<PaymentDashboardReport>();
            List<PaymentDashboardReport> reportThisMonth = new List<PaymentDashboardReport>();
            List<PaymentDashboardReport> reportThisYear = new List<PaymentDashboardReport>();
            reportToday = (from DataRow dr in ds.Tables[0].Rows
                           select new PaymentDashboardReport()
                           {

                               TotalUpfront = dr["TotalUpfront"] != DBNull.Value ? Convert.ToInt32(dr["TotalUpfront"]) : 0,
                               PriorTotalUpfront = dr["PriorTotalUpfront"] != DBNull.Value ? Convert.ToInt32(dr["PriorTotalUpfront"]) : 0,
                               Sales = dr["Sales"] != DBNull.Value ? Convert.ToInt32(dr["Sales"]) : 0,
                               PriorSales = dr["PriorSales"] != DBNull.Value ? Convert.ToInt32(dr["PriorSales"]) : 0,
                               CreditCard = dr["CreditCard"] != DBNull.Value ? Convert.ToInt32(dr["CreditCard"]) : 0,
                               PriorCreditCard = dr["PriorCreditCard"] != DBNull.Value ? Convert.ToInt32(dr["PriorCreditCard"]) : 0,

                               Financed = dr["Financed"] != DBNull.Value ? Convert.ToInt32(dr["Financed"]) : 0,
                               PriorFinanced = dr["PriorFinanced"] != DBNull.Value ? Convert.ToInt32(dr["PriorFinanced"]) : 0,
                               CreditCardAmount = dr["CreditCardAmount"] != DBNull.Value ? Convert.ToDouble(dr["CreditCardAmount"]) : 0.0,
                                PriorCreditCardAmount= dr["PriorCreditCardAmount"] != DBNull.Value ? Convert.ToDouble(dr["PriorCreditCardAmount"]) : 0.0,

                               FinancedAmount = dr["FinancedAmount"] != DBNull.Value ? Convert.ToDouble(dr["FinancedAmount"]) : 0.0,
                               PriorFinancedAmount = dr["PriorFinancedAmount"] != DBNull.Value ? Convert.ToDouble(dr["PriorFinancedAmount"]) : 0.0,
                               TotalUpfrontAmount = dr["TotalUpfrontAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalUpfrontAmount"]) : 0.0,
                               PriorTotalUpfrontAmount = dr["PriorTotalUpfrontAmount"] != DBNull.Value ? Convert.ToDouble(dr["PriorTotalUpfrontAmount"]) : 0.0,

                               CreditCardPercentage = dr["CreditCardPercentage"] != DBNull.Value ? Convert.ToDouble(dr["CreditCardPercentage"]) : 0.0,
                               PriorCreditCardPercentage = dr["PriorCreditCardPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorCreditCardPercentage"]) : 0.0,
                               FinancedPercentage = dr["FinancedPercentage"] != DBNull.Value ? Convert.ToDouble(dr["FinancedPercentage"]) : 0.0,
                               PriorFinancedPercentage = dr["PriorFinancedPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorFinancedPercentage"]) : 0.0,


                               TotalUpfrontPercentage = dr["TotalUpfrontPercentage"] != DBNull.Value ? Convert.ToDouble(dr["TotalUpfrontPercentage"]) : 0.0,
                               PriorTotalUpfrontPercentage = dr["PriorTotalUpfrontPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorTotalUpfrontPercentage"]) : 0.0,
                             


                           }).ToList();

            reportThisWeek = (from DataRow dr in ds.Tables[1].Rows
                              select new PaymentDashboardReport()
                              {

                                  TotalUpfront = dr["TotalUpfront"] != DBNull.Value ? Convert.ToInt32(dr["TotalUpfront"]) : 0,
                                  PriorTotalUpfront = dr["PriorTotalUpfront"] != DBNull.Value ? Convert.ToInt32(dr["PriorTotalUpfront"]) : 0,
                                  Sales = dr["Sales"] != DBNull.Value ? Convert.ToInt32(dr["Sales"]) : 0,
                                  PriorSales = dr["PriorSales"] != DBNull.Value ? Convert.ToInt32(dr["PriorSales"]) : 0,
                                  CreditCard = dr["CreditCard"] != DBNull.Value ? Convert.ToInt32(dr["CreditCard"]) : 0,
                                  PriorCreditCard = dr["PriorCreditCard"] != DBNull.Value ? Convert.ToInt32(dr["PriorCreditCard"]) : 0,

                                  Financed = dr["Financed"] != DBNull.Value ? Convert.ToInt32(dr["Financed"]) : 0,
                                  PriorFinanced = dr["PriorFinanced"] != DBNull.Value ? Convert.ToInt32(dr["PriorFinanced"]) : 0,
                                  CreditCardAmount = dr["CreditCardAmount"] != DBNull.Value ? Convert.ToDouble(dr["CreditCardAmount"]) : 0.0,
                                  PriorCreditCardAmount = dr["PriorCreditCardAmount"] != DBNull.Value ? Convert.ToDouble(dr["PriorCreditCardAmount"]) : 0.0,

                                  FinancedAmount = dr["FinancedAmount"] != DBNull.Value ? Convert.ToDouble(dr["FinancedAmount"]) : 0.0,
                                  PriorFinancedAmount = dr["PriorFinancedAmount"] != DBNull.Value ? Convert.ToDouble(dr["PriorFinancedAmount"]) : 0.0,
                                  TotalUpfrontAmount = dr["TotalUpfrontAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalUpfrontAmount"]) : 0.0,
                                  PriorTotalUpfrontAmount = dr["PriorTotalUpfrontAmount"] != DBNull.Value ? Convert.ToDouble(dr["PriorTotalUpfrontAmount"]) : 0.0,

                                  CreditCardPercentage = dr["CreditCardPercentage"] != DBNull.Value ? Convert.ToDouble(dr["CreditCardPercentage"]) : 0.0,
                                  PriorCreditCardPercentage = dr["PriorCreditCardPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorCreditCardPercentage"]) : 0.0,
                                  FinancedPercentage = dr["FinancedPercentage"] != DBNull.Value ? Convert.ToDouble(dr["FinancedPercentage"]) : 0.0,
                                  PriorFinancedPercentage = dr["PriorFinancedPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorFinancedPercentage"]) : 0.0,


                                  TotalUpfrontPercentage = dr["TotalUpfrontPercentage"] != DBNull.Value ? Convert.ToDouble(dr["TotalUpfrontPercentage"]) : 0.0,
                                  PriorTotalUpfrontPercentage = dr["PriorTotalUpfrontPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorTotalUpfrontPercentage"]) : 0.0,
                              }).ToList();

            reportThisMonth = (from DataRow dr in ds.Tables[2].Rows
                               select new PaymentDashboardReport()
                               {

                                   TotalUpfront = dr["TotalUpfront"] != DBNull.Value ? Convert.ToInt32(dr["TotalUpfront"]) : 0,
                                   PriorTotalUpfront = dr["PriorTotalUpfront"] != DBNull.Value ? Convert.ToInt32(dr["PriorTotalUpfront"]) : 0,
                                   Sales = dr["Sales"] != DBNull.Value ? Convert.ToInt32(dr["Sales"]) : 0,
                                   PriorSales = dr["PriorSales"] != DBNull.Value ? Convert.ToInt32(dr["PriorSales"]) : 0,
                                   CreditCard = dr["CreditCard"] != DBNull.Value ? Convert.ToInt32(dr["CreditCard"]) : 0,
                                   PriorCreditCard = dr["PriorCreditCard"] != DBNull.Value ? Convert.ToInt32(dr["PriorCreditCard"]) : 0,

                                   Financed = dr["Financed"] != DBNull.Value ? Convert.ToInt32(dr["Financed"]) : 0,
                                   PriorFinanced = dr["PriorFinanced"] != DBNull.Value ? Convert.ToInt32(dr["PriorFinanced"]) : 0,
                                   CreditCardAmount = dr["CreditCardAmount"] != DBNull.Value ? Convert.ToDouble(dr["CreditCardAmount"]) : 0.0,
                                   PriorCreditCardAmount = dr["PriorCreditCardAmount"] != DBNull.Value ? Convert.ToDouble(dr["PriorCreditCardAmount"]) : 0.0,

                                   FinancedAmount = dr["FinancedAmount"] != DBNull.Value ? Convert.ToDouble(dr["FinancedAmount"]) : 0.0,
                                   PriorFinancedAmount = dr["PriorFinancedAmount"] != DBNull.Value ? Convert.ToDouble(dr["PriorFinancedAmount"]) : 0.0,
                                   TotalUpfrontAmount = dr["TotalUpfrontAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalUpfrontAmount"]) : 0.0,
                                   PriorTotalUpfrontAmount = dr["PriorTotalUpfrontAmount"] != DBNull.Value ? Convert.ToDouble(dr["PriorTotalUpfrontAmount"]) : 0.0,

                                   CreditCardPercentage = dr["CreditCardPercentage"] != DBNull.Value ? Convert.ToDouble(dr["CreditCardPercentage"]) : 0.0,
                                   PriorCreditCardPercentage = dr["PriorCreditCardPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorCreditCardPercentage"]) : 0.0,
                                   FinancedPercentage = dr["FinancedPercentage"] != DBNull.Value ? Convert.ToDouble(dr["FinancedPercentage"]) : 0.0,
                                   PriorFinancedPercentage = dr["PriorFinancedPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorFinancedPercentage"]) : 0.0,


                                   TotalUpfrontPercentage = dr["TotalUpfrontPercentage"] != DBNull.Value ? Convert.ToDouble(dr["TotalUpfrontPercentage"]) : 0.0,
                                   PriorTotalUpfrontPercentage = dr["PriorTotalUpfrontPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorTotalUpfrontPercentage"]) : 0.0,
                               }).ToList();
            reportThisYear = (from DataRow dr in ds.Tables[3].Rows
                              select new PaymentDashboardReport()
                              {

                                  TotalUpfront = dr["TotalUpfront"] != DBNull.Value ? Convert.ToInt32(dr["TotalUpfront"]) : 0,
                                  PriorTotalUpfront = dr["PriorTotalUpfront"] != DBNull.Value ? Convert.ToInt32(dr["PriorTotalUpfront"]) : 0,
                                  Sales = dr["Sales"] != DBNull.Value ? Convert.ToInt32(dr["Sales"]) : 0,
                                  PriorSales = dr["PriorSales"] != DBNull.Value ? Convert.ToInt32(dr["PriorSales"]) : 0,
                                  CreditCard = dr["CreditCard"] != DBNull.Value ? Convert.ToInt32(dr["CreditCard"]) : 0,
                                  PriorCreditCard = dr["PriorCreditCard"] != DBNull.Value ? Convert.ToInt32(dr["PriorCreditCard"]) : 0,

                                  Financed = dr["Financed"] != DBNull.Value ? Convert.ToInt32(dr["Financed"]) : 0,
                                  PriorFinanced = dr["PriorFinanced"] != DBNull.Value ? Convert.ToInt32(dr["PriorFinanced"]) : 0,
                                  CreditCardAmount = dr["CreditCardAmount"] != DBNull.Value ? Convert.ToDouble(dr["CreditCardAmount"]) : 0.0,
                                  PriorCreditCardAmount = dr["PriorCreditCardAmount"] != DBNull.Value ? Convert.ToDouble(dr["PriorCreditCardAmount"]) : 0.0,

                                  FinancedAmount = dr["FinancedAmount"] != DBNull.Value ? Convert.ToDouble(dr["FinancedAmount"]) : 0.0,
                                  PriorFinancedAmount = dr["PriorFinancedAmount"] != DBNull.Value ? Convert.ToDouble(dr["PriorFinancedAmount"]) : 0.0,
                                  TotalUpfrontAmount = dr["TotalUpfrontAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalUpfrontAmount"]) : 0.0,
                                  PriorTotalUpfrontAmount = dr["PriorTotalUpfrontAmount"] != DBNull.Value ? Convert.ToDouble(dr["PriorTotalUpfrontAmount"]) : 0.0,

                                  CreditCardPercentage = dr["CreditCardPercentage"] != DBNull.Value ? Convert.ToDouble(dr["CreditCardPercentage"]) : 0.0,
                                  PriorCreditCardPercentage = dr["PriorCreditCardPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorCreditCardPercentage"]) : 0.0,
                                  FinancedPercentage = dr["FinancedPercentage"] != DBNull.Value ? Convert.ToDouble(dr["FinancedPercentage"]) : 0.0,
                                  PriorFinancedPercentage = dr["PriorFinancedPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorFinancedPercentage"]) : 0.0,


                                  TotalUpfrontPercentage = dr["TotalUpfrontPercentage"] != DBNull.Value ? Convert.ToDouble(dr["TotalUpfrontPercentage"]) : 0.0,
                                  PriorTotalUpfrontPercentage = dr["PriorTotalUpfrontPercentage"] != DBNull.Value ? Convert.ToDouble(dr["PriorTotalUpfrontPercentage"]) : 0.0,
                              }).ToList();

            CustomerPaymentDashboardReportModel model = new CustomerPaymentDashboardReportModel();
            model.ReportToday = reportToday.FirstOrDefault();
            model.ReportThisWeek = reportThisWeek.FirstOrDefault();
            model.ReportThisMonth = reportThisMonth.FirstOrDefault();
            model.ReportThisYear = reportThisYear.FirstOrDefault();

            return model;
        }
        public CustomerFinancedReportModel GetCustomerFinancedReprot(DashboardReportDateFilter filter)
        {
            DataSet ds = _CustomerDataAccess.GetCustomerFinancedReprot(filter);
            List<CustomerFinancedReport> reportToday = new List<CustomerFinancedReport>();
            List<CustomerFinancedReport> reportThisWeek = new List<CustomerFinancedReport>();
            List<CustomerFinancedReport> reportThisMonth = new List<CustomerFinancedReport>();
            List<CustomerFinancedReport> reportThisYear = new List<CustomerFinancedReport>();
            reportToday = (from DataRow dr in ds.Tables[0].Rows
                           select new CustomerFinancedReport()
                           {
                               RMR = dr["RMR"] != DBNull.Value ? Convert.ToDouble(dr["RMR"]) : 0.0,
                               CC = dr["CC"] != DBNull.Value ? Convert.ToInt32(dr["CC"]) : 0,
                               CCAmount = dr["CCAmount"] != DBNull.Value ? Convert.ToDouble(dr["CCAmount"]) : 0.0,
                               CCPercentage = dr["CCPercentage"] != DBNull.Value ? Convert.ToDouble(dr["CCPercentage"]) : 0.0,

                               FinancedPercentage = dr["FinancedPercentage"] != DBNull.Value ? Convert.ToDouble(dr["FinancedPercentage"]) : 0.0,
                               Financed = dr["Financed"] != DBNull.Value ? Convert.ToInt32(dr["Financed"]) : 0,
                               FinancedAmount = dr["FinancedAmount"] != DBNull.Value ? Convert.ToDouble(dr["FinancedAmount"]) : 0.0,

                               Upfront = dr["Upfront"] != DBNull.Value ? Convert.ToInt32(dr["Upfront"]) : 0,
                               UpfrontPercentage = dr["UpfrontPercentage"] != DBNull.Value ? Convert.ToDouble(dr["UpfrontPercentage"]) : 0.0,
                               UpfrontAmount = dr["UpfrontAmount"] != DBNull.Value ? Convert.ToDouble(dr["UpfrontAmount"]) : 0.0

                           }).ToList();

            reportThisWeek = (from DataRow dr in ds.Tables[1].Rows
                              select new CustomerFinancedReport()
                              {

                                  RMR = dr["RMR"] != DBNull.Value ? Convert.ToDouble(dr["RMR"]) : 0.0,
                                  CC = dr["CC"] != DBNull.Value ? Convert.ToInt32(dr["CC"]) : 0,
                                  CCAmount = dr["CCAmount"] != DBNull.Value ? Convert.ToDouble(dr["CCAmount"]) : 0.0,
                                  CCPercentage = dr["CCPercentage"] != DBNull.Value ? Convert.ToDouble(dr["CCPercentage"]) : 0.0,

                                  FinancedPercentage = dr["FinancedPercentage"] != DBNull.Value ? Convert.ToDouble(dr["FinancedPercentage"]) : 0.0,
                                  Financed = dr["Financed"] != DBNull.Value ? Convert.ToInt32(dr["Financed"]) : 0,
                                  FinancedAmount = dr["FinancedAmount"] != DBNull.Value ? Convert.ToDouble(dr["FinancedAmount"]) : 0.0,

                                  Upfront = dr["Upfront"] != DBNull.Value ? Convert.ToInt32(dr["Upfront"]) : 0,
                                  UpfrontPercentage = dr["UpfrontPercentage"] != DBNull.Value ? Convert.ToDouble(dr["UpfrontPercentage"]) : 0.0,
                                  UpfrontAmount = dr["UpfrontAmount"] != DBNull.Value ? Convert.ToDouble(dr["UpfrontAmount"]) : 0.0
                              }).ToList();

            reportThisMonth = (from DataRow dr in ds.Tables[2].Rows
                               select new CustomerFinancedReport()
                               {

                                   RMR = dr["RMR"] != DBNull.Value ? Convert.ToDouble(dr["RMR"]) : 0.0,
                                   CC = dr["CC"] != DBNull.Value ? Convert.ToInt32(dr["CC"]) : 0,
                                   CCAmount = dr["CCAmount"] != DBNull.Value ? Convert.ToDouble(dr["CCAmount"]) : 0.0,
                                   CCPercentage = dr["CCPercentage"] != DBNull.Value ? Convert.ToDouble(dr["CCPercentage"]) : 0.0,

                                   FinancedPercentage = dr["FinancedPercentage"] != DBNull.Value ? Convert.ToDouble(dr["FinancedPercentage"]) : 0.0,
                                   Financed = dr["Financed"] != DBNull.Value ? Convert.ToInt32(dr["Financed"]) : 0,
                                   FinancedAmount = dr["FinancedAmount"] != DBNull.Value ? Convert.ToDouble(dr["FinancedAmount"]) : 0.0,

                                   Upfront = dr["Upfront"] != DBNull.Value ? Convert.ToInt32(dr["Upfront"]) : 0,
                                   UpfrontPercentage = dr["UpfrontPercentage"] != DBNull.Value ? Convert.ToDouble(dr["UpfrontPercentage"]) : 0.0,
                                   UpfrontAmount = dr["UpfrontAmount"] != DBNull.Value ? Convert.ToDouble(dr["UpfrontAmount"]) : 0.0
                               }).ToList();
            reportThisYear = (from DataRow dr in ds.Tables[3].Rows
                              select new CustomerFinancedReport()
                              {

                                  RMR = dr["RMR"] != DBNull.Value ? Convert.ToDouble(dr["RMR"]) : 0.0,
                                  CC = dr["CC"] != DBNull.Value ? Convert.ToInt32(dr["CC"]) : 0,
                                  CCAmount = dr["CCAmount"] != DBNull.Value ? Convert.ToDouble(dr["CCAmount"]) : 0.0,
                                  CCPercentage = dr["CCPercentage"] != DBNull.Value ? Convert.ToDouble(dr["CCPercentage"]) : 0.0,

                                  FinancedPercentage = dr["FinancedPercentage"] != DBNull.Value ? Convert.ToDouble(dr["FinancedPercentage"]) : 0.0,
                                  Financed = dr["Financed"] != DBNull.Value ? Convert.ToInt32(dr["Financed"]) : 0,
                                  FinancedAmount = dr["FinancedAmount"] != DBNull.Value ? Convert.ToDouble(dr["FinancedAmount"]) : 0.0,

                                  Upfront = dr["Upfront"] != DBNull.Value ? Convert.ToInt32(dr["Upfront"]) : 0,
                                  UpfrontPercentage = dr["UpfrontPercentage"] != DBNull.Value ? Convert.ToDouble(dr["UpfrontPercentage"]) : 0.0,
                                  UpfrontAmount = dr["UpfrontAmount"] != DBNull.Value ? Convert.ToDouble(dr["UpfrontAmount"]) : 0.0
                              }).ToList();

                                CustomerFinancedReportModel model = new CustomerFinancedReportModel();
                                model.ReportToday = reportToday.FirstOrDefault();
                                model.ReportThisWeek = reportThisWeek.FirstOrDefault();
                                model.ReportThisMonth = reportThisMonth.FirstOrDefault();
                                model.ReportThisYear = reportThisYear.FirstOrDefault();

            return model;
        }
        public DashboardBarModelTechnician GetDashBoardBarDataTechnician(Guid CompanyId, Guid empid)
        {
            DataSet dt = _InvoiceDataAccess.GetDashBoardBoardBarDataTechnician(CompanyId, empid);
            List<DashboardBarModelTechnician> DashBoardModel = new List<DashboardBarModelTechnician>();
            if (dt != null)
                DashBoardModel = (from DataRow dr in dt.Tables[0].Rows
                                  select new DashboardBarModelTechnician()
                                  {
                                      TotalCommissionSC = dr["TotalCommissionSC"] != DBNull.Value ? Convert.ToInt32(dr["TotalCommissionSC"]) : 0,
                                      TotalCommissionTC = dr["TotalCommissionTC"] != DBNull.Value ? Convert.ToInt32(dr["TotalCommissionTC"]) : 0,
                                      TotalCommissionAC = dr["TotalCommissionAC"] != DBNull.Value ? Convert.ToInt32(dr["TotalCommissionAC"]) : 0,
                                      TotalCommissionFC = dr["TotalCommissionFC"] != DBNull.Value ? Convert.ToInt32(dr["TotalCommissionFC"]) : 0,
                                      TotalCommissionRC = dr["TotalCommissionRC"] != DBNull.Value ? Convert.ToInt32(dr["TotalCommissionRC"]) : 0,
                                  }).ToList();
            DashBoardModel.FirstOrDefault().Total90GoBack = dt.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(dt.Tables[1].Rows[0]["TotalCount"]) : 0;

            return DashBoardModel.FirstOrDefault();
        }


        public AllCommissionTechnician GetLastweekspayListTechnician(Guid CompanyId, Guid empid)
        {
            DataSet dt = _InvoiceDataAccess.GetLastweekspayListTechnician(CompanyId, empid);
            AllCommissionTechnician DashBoardModel = new AllCommissionTechnician();
            if (dt != null)
                DashBoardModel.SalesCommission = (from DataRow dr in dt.Tables[0].Rows
                                  select new SalesCommission()
                                  {
                                      Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                      //ContactName = dr["ContactName"].ToString(),
                                      SalesCommissionId = (Guid)dr["SalesCommissionId"],
                                      TicketId = (Guid)dr["TicketId"],
                                      CustomerId = (Guid)dr["CustomerId"],
                                      UserId = (Guid)dr["UserId"],
                                      CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                                      RMRSold = dr["RMRSold"] != DBNull.Value ? Convert.ToDouble(dr["RMRSold"]) : 0.0,
                                      RMRCommission = dr["RMRCommission"] != DBNull.Value ? Convert.ToDouble(dr["RMRCommission"]) : 0.0,
                                      NoOfEquipment = dr["NoOfEquipment"] != DBNull.Value ? Convert.ToInt32(dr["NoOfEquipment"]) : 0,
                                      EquipmentCommission = dr["EquipmentCommission"] != DBNull.Value ? Convert.ToDouble(dr["EquipmentCommission"]) : 0.0,
                                      TotalCommission = dr["TotalCommission"] != DBNull.Value ? Convert.ToDouble(dr["TotalCommission"]) : 0.0,
                                      IsPaid = (dr["IsPaid"] != DBNull.Value ? Convert.ToBoolean(dr["IsPaid"]) : false),
                                      CreatedBy = (Guid)dr["CreatedBy"],
                                      CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                      Batch = dr["Batch"].ToString(),
                                      Adjustment = dr["Adjustment"] != DBNull.Value ? Convert.ToDouble(dr["Adjustment"]) : 0.0,

                                      RMRCommissionCalculation = dr["RMRCommissionCalculation"].ToString(),
                                      EquipmentCommissionCalculation = dr["EquipmentCommissionCalculation"].ToString(),

                                      PaidDate = dr["PaidDate"] != DBNull.Value ? Convert.ToDateTime(dr["PaidDate"]) : new DateTime()
                                  }).ToList();
            DashBoardModel.TechCommission = (from DataRow dr in dt.Tables[1].Rows
                                              select new TechCommission()
                                              {
                                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                  //ContactName = dr["ContactName"].ToString(),
                                                  TechCommissionId = (Guid)dr["TechCommissionId"],
                                                  TicketId = (Guid)dr["TicketId"],
                                                  CustomerId = (Guid)dr["CustomerId"],
                                                  UserId = (Guid)dr["UserId"],
                                                  CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                                                  BaseRMR = dr["BaseRMR"] != DBNull.Value ? Convert.ToDouble(dr["BaseRMR"]) : 0.0,
                                                  BaseRMRCommission = dr["BaseRMRCommission"] != DBNull.Value ? Convert.ToDouble(dr["BaseRMRCommission"]) : 0.0,
                                                  AddedRMR = dr["AddedRMR"] != DBNull.Value ? Convert.ToDouble(dr["AddedRMR"]) : 0.0,
                                                  AddedRMRCommission = dr["AddedRMRCommission"] != DBNull.Value ? Convert.ToDouble(dr["AddedRMRCommission"]) : 0.0,
                                                  TotalCommission = dr["TotalCommission"] != DBNull.Value ? Convert.ToDouble(dr["TotalCommission"]) : 0.0,
                                                  IsPaid = (dr["IsPaid"] != DBNull.Value ? Convert.ToBoolean(dr["IsPaid"]) : false),
                                                  CreatedBy = (Guid)dr["CreatedBy"],
                                                  CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                                  Batch = dr["Batch"].ToString(),
                                                  Adjustment = dr["Adjustment"] != DBNull.Value ? Convert.ToDouble(dr["Adjustment"]) : 0.0,

                                                  BaseRMRCommissionCalculation = dr["BaseRMRCommissionCalculation"].ToString(),
                                                  AddedRMRCommissionCalculation = dr["AddedRMRCommissionCalculation"].ToString(),
                                                  PaidDate = dr["PaidDate"] != DBNull.Value ? Convert.ToDateTime(dr["PaidDate"]) : new DateTime()
                                              }).ToList();
            DashBoardModel.AddMemberCommission = (from DataRow dr in dt.Tables[2].Rows
                                             select new AddMemberCommission()
                                             {
                                                 Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                 //ContactName = dr["ContactName"].ToString(),
                                                 AddMemberCommissionId = (Guid)dr["AddMemberCommissionId"],
                                                 TicketId = (Guid)dr["TicketId"],
                                                 CustomerId = (Guid)dr["CustomerId"],
                                                 UserId = (Guid)dr["UserId"],
                                                 CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                                                 Adjustment = dr["Adjustment"] != DBNull.Value ? Convert.ToDouble(dr["Adjustment"]) : 0.0,
                                                 Commission = dr["Commission"] != DBNull.Value ? Convert.ToDouble(dr["Commission"]) : 0.0,
                                                 IsPaid = (dr["IsPaid"] != DBNull.Value ? Convert.ToBoolean(dr["IsPaid"]) : false),
                                                 CreatedBy = (Guid)dr["CreatedBy"],
                                                 CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                                 Batch = dr["Batch"].ToString(),
                                                 CommissionCalculation = dr["CommissionCalculation"].ToString(),
                                                 PaidDate = dr["PaidDate"] != DBNull.Value ? Convert.ToDateTime(dr["PaidDate"]) : new DateTime()
                                             }).ToList();
            DashBoardModel.FollowUpCommission = (from DataRow dr in dt.Tables[3].Rows
                                             select new FollowUpCommission()
                                             {
                                                 Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                 //ContactName = dr["ContactName"].ToString(),
                                                 FollowUpCommissionId = (Guid)dr["FollowUpCommissionId"],
                                                 TicketId = (Guid)dr["TicketId"],
                                                 CustomerId = (Guid)dr["CustomerId"],
                                                 UserId = (Guid)dr["UserId"],
                                                 CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                                                 Adjustment = dr["Adjustment"] != DBNull.Value ? Convert.ToDouble(dr["Adjustment"]) : 0.0,
                                                 Commission = dr["Commission"] != DBNull.Value ? Convert.ToDouble(dr["Commission"]) : 0.0,
                                                 IsPaid = (dr["IsPaid"] != DBNull.Value ? Convert.ToBoolean(dr["IsPaid"]) : false),
                                                 CreatedBy = (Guid)dr["CreatedBy"],
                                                 CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                                 Batch = dr["Batch"].ToString(),
                                                 CommissionCalculation = dr["CommissionCalculation"].ToString(),
                                                 PaidDate = dr["PaidDate"] != DBNull.Value ? Convert.ToDateTime(dr["PaidDate"]) : new DateTime()
                                             }).ToList();
            DashBoardModel.RescheduleCommission = (from DataRow dr in dt.Tables[4].Rows
                                                 select new RescheduleCommission()
                                                 {
                                                     Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                     //ContactName = dr["ContactName"].ToString(),
                                                     RescheduleCommissionId = (Guid)dr["FollowUpCommissionId"],
                                                     TicketId = (Guid)dr["TicketId"],
                                                     CustomerId = (Guid)dr["CustomerId"],
                                                     UserId = (Guid)dr["UserId"],
                                                     CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                                                     Adjustment = dr["Adjustment"] != DBNull.Value ? Convert.ToDouble(dr["Adjustment"]) : 0.0,
                                                     Commission = dr["Commission"] != DBNull.Value ? Convert.ToDouble(dr["Commission"]) : 0.0,
                                                     IsPaid = (dr["IsPaid"] != DBNull.Value ? Convert.ToBoolean(dr["IsPaid"]) : false),
                                                     CreatedBy = (Guid)dr["CreatedBy"],
                                                     CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                                     Batch = dr["Batch"].ToString(),
                                                     CommissionCalculation = dr["CommissionCalculation"].ToString(),
                                                     PaidDate = dr["PaidDate"] != DBNull.Value ? Convert.ToDateTime(dr["PaidDate"]) : new DateTime()
                                                 }).ToList();

            return DashBoardModel;
        }


        public DashboardModelTechnician GetDashBoardDataTechnicianByDate(Guid CompanyId, Guid empid, string firstdate, string lastdate)
        {
            DataTable dt = _InvoiceDataAccess.GetDashBoardDataTechnicianByDate(CompanyId, empid, firstdate, lastdate);
            List<DashboardModelTechnician> DashBoardModel = new List<DashboardModelTechnician>();
            if (dt != null)
                DashBoardModel = (from DataRow dr in dt.Rows
                                  select new DashboardModelTechnician()
                                  {
                                      OpenInstallationTicket = dr["OpenInstallationTicket"] != DBNull.Value ? Convert.ToInt32(dr["OpenInstallationTicket"]) : 0,
                                      OpenServiceTicket = dr["OpenServiceTicket"] != DBNull.Value ? Convert.ToInt32(dr["OpenServiceTicket"]) : 0,
                                      ClosedInstallationTicket = dr["ClosedInstallationTicket"] != DBNull.Value ? Convert.ToInt32(dr["ClosedInstallationTicket"]) : 0,
                                      ClosedServiceTicket = dr["ClosedServiceTicket"] != DBNull.Value ? Convert.ToInt32(dr["ClosedServiceTicket"]) : 0,

                                  }).ToList();
            return DashBoardModel.FirstOrDefault();
        }

        public CustomerAppointmentEquipment GetDashBoardDataTechnicianUpsold(Guid userid)
        {
            
            DataTable dt = _InvoiceDataAccess.GetDashBoardDataTechnicianUpsold(userid);
            CustomerAppointmentEquipment model = new CustomerAppointmentEquipment();
            if (dt != null)
                model = (from DataRow dr in dt.Rows
                         select new CustomerAppointmentEquipment()
                         {
                             UpsoldEquipments = dr["UpsoldEquipments"] != DBNull.Value ? Convert.ToInt32(dr["UpsoldEquipments"]) : 0,
                             UpsoldServices = dr["UpsoldServices"] != DBNull.Value ? Convert.ToInt32(dr["UpsoldServices"]) : 0,
                             UpsoldEquipmentsTotalPrice = dr["UpsoldEquipmentsTotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["UpsoldEquipmentsTotalPrice"]) : 0.0,
                             UpsoldServicesTotalPrice = dr["UpsoldServicesTotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["UpsoldServicesTotalPrice"]) : 0.0,
                             UpsoldEquipmentsTotalQuantity = dr["UpsoldEquipmentsTotalQuantity"] != DBNull.Value ? Convert.ToDouble(dr["UpsoldEquipmentsTotalQuantity"]) : 0.0,
                             UpsoldServicesTotalQuantity = dr["UpsoldServicesTotalQuantity"] != DBNull.Value ? Convert.ToDouble(dr["UpsoldServicesTotalQuantity"]) : 0.0,
                             TotalUpsold = dr["TotalUpsold"] != DBNull.Value ? Convert.ToInt32(dr["TotalUpsold"]) : 0
                         }).FirstOrDefault();
            return model;
        }

        public CustomerAppointmentEquipment GetDashBoardDataTechnicianUpsoldByDate(Guid userid, string firstdate, string lastdate)
        {
            DataTable dt = _InvoiceDataAccess.GetDashBoardDataTechnicianUpsoldByDate(userid, firstdate, lastdate);
            CustomerAppointmentEquipment model = new CustomerAppointmentEquipment();
            if (dt != null)
                model = (from DataRow dr in dt.Rows
                         select new CustomerAppointmentEquipment()
                         {
                             UpsoldEquipments = dr["UpsoldEquipments"] != DBNull.Value ? Convert.ToInt32(dr["UpsoldEquipments"]) : 0,
                             UpsoldServices = dr["UpsoldServices"] != DBNull.Value ? Convert.ToInt32(dr["UpsoldServices"]) : 0,
                             UpsoldEquipmentsTotalPrice = dr["UpsoldEquipmentsTotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["UpsoldEquipmentsTotalPrice"]) : 0.0,
                             UpsoldServicesTotalPrice = dr["UpsoldServicesTotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["UpsoldServicesTotalPrice"]) : 0.0,
                             TotalUpsold = dr["TotalUpsold"] != DBNull.Value ? Convert.ToInt32(dr["TotalUpsold"]) : 0
                         }).FirstOrDefault();
            return model;
        }

        public List<DashBoardReminderFollowUpsModel> GetDashBoardReminderFollowUpsData(Guid CompanyId, string emptag, Guid empid)
        {
            DataTable dt = _CustomerNoteDataAccess.GetDashBoardReminderFollowUpsData(CompanyId, emptag, empid);
            List<DashBoardReminderFollowUpsModel> model = new List<DashBoardReminderFollowUpsModel>();
            model = (from DataRow dr in dt.Rows
                     select new DashBoardReminderFollowUpsModel()
                     {
                         CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                         ReminderDate = dr["ReminderDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReminderDate"]) : DateTime.Now,
                         ReminderType = dr["ReminderType"].ToString(),
                         Note = dr["Note"].ToString(),
                         CustomerName = dr["CustomerName"].ToString(),
                         UserType = dr["UserType"] != DBNull.Value ? Convert.ToBoolean(dr["UserType"]) : false,
                         Email = dr["Email"] != DBNull.Value ? Convert.ToBoolean(dr["Email"]) : false,
                         Phone = dr["Phone"] != DBNull.Value ? Convert.ToBoolean(dr["Phone"]) : false,
                         Customerid = dr["Customerid"] != DBNull.Value ? Convert.ToInt32(dr["Customerid"]) : 0,
                         noteid = dr["noteid"] != DBNull.Value ? Convert.ToInt32(dr["noteid"]) : 0,
                         BusinessName = dr["BusinessName"].ToString(),
                         AssignUser = dr["AssignUser"].ToString().TrimEnd(' ', ',')
                     }).ToList();
            return model;
        }
        public List<DashBoardReminderFollowUpsModel> GetDashBoardCurrentUserReminderFollowUpsData(Guid CompanyId, Guid empid, string selctactive)
        {
            DataTable dt = _CustomerNoteDataAccess.GetDashBoardCurrentUserReminderFollowUpsData(CompanyId, empid, selctactive);
            List<DashBoardReminderFollowUpsModel> model = new List<DashBoardReminderFollowUpsModel>();
            model = (from DataRow dr in dt.Rows
                     select new DashBoardReminderFollowUpsModel()
                     {
                         CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                         ReminderDate = dr["ReminderDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReminderDate"]) : DateTime.Now,
                         ReminderEndDate = dr["ReminderEndDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReminderEndDate"]) : DateTime.Now,
                         ReminderType = dr["ReminderType"].ToString(),
                         Note = dr["Note"].ToString(),
                         CustomerName = dr["CustomerName"].ToString(),
                         UserType = dr["UserType"] != DBNull.Value ? Convert.ToBoolean(dr["UserType"]) : false,
                         Email = dr["Email"] != DBNull.Value ? Convert.ToBoolean(dr["Email"]) : false,
                         Phone = dr["Phone"] != DBNull.Value ? Convert.ToBoolean(dr["Phone"]) : false,
                         Customerid = dr["Customerid"] != DBNull.Value ? Convert.ToInt32(dr["Customerid"]) : 0,
                         noteid = dr["noteid"] != DBNull.Value ? Convert.ToInt32(dr["noteid"]) : 0,
                         BusinessName = dr["BusinessName"].ToString(),
                         AssignUser = dr["AssignUser"].ToString().TrimEnd(' ', ',')
                     }).ToList();
            return model;
        }

    }
}
