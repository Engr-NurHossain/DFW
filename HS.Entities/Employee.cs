using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
    public partial class Employee
    {
        public double PTOUnassigned { get; set; }
        public string EMPName { get; set; }
        public string AssignName { get; set; }
        public string PermissionGroupName { set; get; }
        public int UserLoginId { get; set; }
        public string ResourceName { get; set; }
        public string Address { get; set; }
        public string EmployeeAddress { get; set; }
        public string CompanyName { get; set; }
        public int AnniversaryYears { get; set; }
        public int InstallationsScheduled { get; set; }

        public int Installationscomplete { get; set; }

        public int servicesscheduled { get; set; }

        public int servicescomplete { get; set; }
        public string Insurance { get; set; }
        public DateTime EligibleFrom { get; set; }
        public double Occurence { get; set; }
        public string MedicalPlan { get; set; }
        public string MedicalType { get; set; }
        public double MedicalAmount { get; set; }
        public string DentalPlan { get; set; }
        public string DentalType { get; set; }
        public double DentalAmount { get; set; }

        public int TechnicianCount { get; set; }

        public string VisionType { get; set; }
        public double VisionAmount { get; set; }

        public double VoluntaryLifeAmount { get; set; }
        public double STDAmount { get; set; }
        public double LTDAmount { get; set; }

        public DateTime LastEvaluationDate { get; set; }
        public DateTime NextEvaluationDate { get; set; }

        public string IsMedical { get; set; }
        public string IsDental { get; set; }
        public string IsVision { get; set; }
        public string IsVoluntaryLife { get; set; }
        public string IsSTD { get; set; }
        public string IsLTD { get; set; }
        public int UserIntId { get; set; }



    }
    public class EmployeeDashboardAPI
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string CompanyName { get; set; }
        public string ProfileImage { get; set; }
        //public List<EmployeeTimeClock> ListEmployeeTimeClock { get; set; }
    }
    public class ClockinDetail
    {
        public string Status { get; set; }
        public DateTime ClockInTime { get; set; }
        public DateTime ClockOutTime { get; set; }
        public int WeeklyVisit { get; set; }
        public string Job { get; set; }
        public string StartPayPeriod { get; set; }
        public string EndPayPeriod { get; set; }
        public double RegularHour { get; set; }
        public double OTOHours { get; set; }
        public double PTOHours { get; set; }
        public double TotalHours { get; set; }
    }
    public class EmployeeFilter
    {
        public Guid CustomerId { set; get; }
        public Guid CompanyId { set; get; }
        public Guid UserId { set; get; }

        public string Name { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int InstallationSchedule { get; set; }
        public int InstallationComplete { get; set; }

        public int ServiceSchedule { get; set; }

        public int ServiceComplete { get; set; }


        public string SearchText { set; get; }
        public string TicketType { set; get; }
        public string[] TicketStatusArr { get; set; }
        public string TicketStatus { set; get; }
        public Guid Assigned { set; get; }
        public string MyTicket { set; get; }
        public string order { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? PageNo { set; get; }
        public int PageSize { set; get; }
        public string ReportTabType { get; set; }
        public string AssignedUserTicket { get; set; }
        public string category { get; set; }
        public string manufact { get; set; }
    }
    public class EmployeeListModel
    {
        public List<Ticket> Tickets { set; get; }
        public List<Employee> Employees { set; get; }

        public List<TicketUser> TicketUsers { set; get; }

        public int TotalInstallationsScheduled { get; set; }

        public int TotalInstallationscomplete { get; set; }

        public int Totalservicesscheduled { get; set; }

        public int Totalservicescomplete { get; set; }


        public string Searchtext { set; get; }
        public int PageNo { set; get; }
        public int PageSize { set; get; }
        public int TotalCount { set; get; }
        public List<Customer> CustomarList { set; get; }
        public List<TicketSummary> TicketSummaryList { set; get; }
        public string TicketStatus { set; get; }
        public string TicketType { set; get; }
        public string MyTicket { set; get; }
        public Guid Assigned { set; get; }
        public List<GoBackTicketModel> ListGoBackTicketModel { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CustomerNo { set; get; }
        public string BookingId { set; get; }

    }
    public partial class EmployeePartial
    {
        public Guid UserId { get; set; }
        public DateTime DOB { get; set; }
        public DateTime HireDate { get; set; }
        public string PayType { get; set; }
        public DateTime AnniversaryDate { get; set; }
        public string BasePay { get; set; }
        public double HourlyRate { get; set; }
        public string EmpType { get; set; }
        public string Department { get; set; }
        public float PtoRate { get; set; }
        public float PtoHour { get; set; }
        public float PtoRemain { get; set; }
        public string SalesCommissionStructure { get; set; }
        public string Session { get; set; }
        public double UserXComission { get; set; }
        public bool IsSalesMatrixUserX { get; set; }
        public Guid TermSheetId { get; set; }
    }
    //[Shariful-20-9-19]
    public partial class Partner
    {
        public Guid UserId { get; set; }
        public Guid SupervisorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    //[~Shariful-20-9-19]
    public class TotalEmployeeCount
    {
        public int TotalCount { get; set; }
    }
    public partial class EmployeeListMatrix
    {
        public int Id { get; set; }
        public int UserLoginId { get; set; }
        public Guid EmpId { get; set; }
        public string EmployeeName { get; set; }
        public int NoOfLeads { get; set; }
        public int TotalLeads { get; set; }
        public int BadLeads { get; set; }
        public int GoodLeads { get; set; }
        public int AssignedTo { get; set; }
        public int ApptSetBy { get; set; }
        public int Closing { get; set; }
        public int CustomerFunded { get; set; }
        public int AppoinmentSet { get; set; }
        public double Percentage { get; set; }
        public double UserX { get; set; }
    }
    public partial class CustomerData
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string LeadSource { get; set; }
        public string RMR { get; set; }
        public DateTime SaleDate { get; set; }
        public string AppoinmentSet { get; set; }
        public string LeadStatusVal { get; set; }
        public string CSNumber { get; set; }
        public DateTime CreatedDay { get; set; }

    }
    public class TotalSalesMatrix
    {
        public int TotalLeads { get; set; }
        public int TotalClosing { get; set; }
    }
    public partial class EmployeeListMatrixWithCount
    {
        public List<EmployeeListMatrix> EmployeeListMatrixList { get; set; }
        public TotalEmployeeCount TotalEmployeeCount { get; set; }
        public TotalSalesMatrix TotalSalesMatrix { get; set; }
        public int TotalTotalLeads { get; set; }
        public int TotalBadLeads { get; set; }
        public int TotalGoodLeads { get; set; }
        public int TotalClosing { get; set; }
        public int TotalTotalSales { get; set; }
        public int TotalTotalFunded { get; set; }
        public int TotalCustomerFunded { get; set; }
        public int TotalAppoinmentSet { get; set; }
        public double AvgPercentage { get; set; }
        public double AvgUserX { get; set; }
    }
    public partial class EmployeeListMatrixCustomerModel
    {
        public List<CustomerData> EmployeeListMatrixCustomerList { get; set; }
        public TotalEmployeeCount TotalCustomerCount { get; set; }
    }
    public partial class AllUserX
    {
        public double FirstCallUserX { get; set; }
        public double OverallUserX { get; set; }
        public double SoldtofundedUserX { get; set; }
        public double NumberofSalesUserX { get; set; }
        public double AppointmentSetUserX { get; set; }
    }
    public partial class EmployeeAPIModel
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string SSN { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime HireDate { get; set; }
        public string ProfilePicture { get; set; }
        public string JobTitle { get; set; }
        public string Session { get; set; }
        public string PlaceOfBirth { get; set; }
        public string SalesCommissionStructure { get; set; }
        public string TechCommissionStructure { get; set; }
        public bool RecruitmentProcess { get; set; }
        public bool Recruited { get; set; }
        public bool IsCalendar { get; set; }
        public string CalendarColor { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public bool IsSupervisor { get; set; }
        public string SuperVisorId { get; set; }
        public double HourlyRate { get; set; }
        public bool NoAutoClockOut { get; set; }
        public DateTime FireLicenseExpirationDate { get; set; }
        public DateTime SalesLicenseExpirationDate { get; set; }
        public DateTime InstallLicenseExpirationDate { get; set; }
        public DateTime DriversLicenseExpirationDate { get; set; }
        public string ClockInIP { get; set; }
        public DateTime DOB { get; set; }
        public string BasePay { get; set; }
        public string EmpType { get; set; }
        public string Department { get; set; }
        public double PtoRate { get; set; }
        public double PtoHour { get; set; }
        public double PtoRemain { get; set; }
        public bool IsPayroll { get; set; }
        public string LicenseNo { get; set; }
        public DateTime AnniversaryDate { get; set; }
        public string BadgerUserId { get; set; }
        public string AlarmId { get; set; }
        public double UserXComission { get; set; }
        public bool IsCurrentEmployee { get; set; }
        public int CSId { get; set; }
        public string Street2 { get; set; }
        public string City2 { get; set; }
        public string State2 { get; set; }
        public string ZipCode2 { get; set; }
        public string StreetPrevious { get; set; }
        public bool IsSalesMatrixUserX { get; set; }
        public DateTime TerminationDate { get; set; }
        public Guid CompanyId { get; set; }
        public Guid TermSheetId { get; set; }
        public string BrinksDealerUser { get; set; }
        public string BrinksDealerPassword { get; set; }
        public bool IsSalesMatrix { get; set; }
        public string Role { get; set; }
    }

    public partial class EmployeeListWithCustomerModel
    {
        public List<Employee> EmployeeList { get; set; }
        public TotalEmployeeCount TotalEmployeeCount { get; set; }
    }

}
