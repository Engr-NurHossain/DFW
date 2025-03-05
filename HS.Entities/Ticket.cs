using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
    public partial class Ticket
    {
        public string StatusVal { set; get; }
        public string PriorityVal { set; get; }
        public string ResignByVal { set; get; }
        public string CreatedByVal { set; get; }
        public string CreatedDateVal { set; get; }
        public string AssignedTo { set; get; }
        public string AdditionalMembers { set; get; }
        public string TicketTypeVal { set; get; }
        public int AttachmentsCount { set; get; }
        public int RepliesCount { set; get; }
        public string AppointmentStartTime { set; get; }
        public string AppointmentStartTimeVal { set; get; }
        public string AppointmentEndTime { set; get; }
        public string AppointmentEndTimeVal { set; get; }
        public int ExceedQuantity { get; set; }
        public int TicketReplyCount { get; set; }
        public string TicketUserName { get; set; }
        public string CustomerName { get; set; }
        public DateTime SalesDate { get; set; }
        public DateTime CustomerAgreementSignature { get; set; }
        public DateTime InstallDate { get; set; }
        public int CusIdInt { get; set; }
        public double TotalPoint { get; set; }
        public string CusBusinessName { get; set; }
        public string CusSalesPerson { get; set; }
        public string CusInstaller { get; set; }
        public double RMRAmount { get; set; }
        public int AppointmentEquipmentId { get; set; }
        public string Category { get; set; }
        public string Manufacturer { get; set; }
        public string Description { get; set; }
        public string EmpUser { get; set; }
        public string SKU { get; set; }
        public string CustomerQA1 { get; set; }
        public string CustomerQA2 { get; set; }
        public string OwnerShip { get; set; }
        public string PrevTicketType { get; set; }
        public DateTime PrevAppointmentDate { get; set; }
        public DateTime AccountOnlineDate { get; set; }
        public string WhoPlacedOnline { get; set; }
        public string PrevTechnician { get; set; }
        public string CusSalesLoc { get; set; }
        public double CompanyCost { get; set; }
        public double CustomerCost { get; set; }
        public Guid AssignedToId { get; set; }
        public string ReferenceTicketList { get; set; }
        public int CustomerAddressId { get; set; }
        public string AdditionalMembersUserId { get; set; }
        public List<AdditionalMember> AdditionalMemberList { get; set; }
        public bool IsTicketPayment { get; set; }
        public string CreatedUser { get; set; }
        public string AssignUser { get; set; }
        public string CustomerNo { get; set; }
        public string LeadSource { get; set; }
        public bool AssignedToIsReschedulePay { get; set; }
        public double BookingInvoiceAmount { set; get; }
        public double TotalCollectedAmount { set; get; }
        public string Address { get; set; }
        public string RegisteredVal { get; set; }
        public int Quantity { get; set; }
        public int InstalledQuantity { get; set; }
        public string BusinessName { get; set; }
        public string PrimaryPhone { get; set; }

        public int PrevTicketId { get; set; }
        public string SoldBy { get; set; }
    }
    public class TicketListModel
    {
        public List<Ticket> Tickets { set; get; }
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
        public double TotalRMR { set; get; }
        public int TotalQuantity { get; set; }
        public double TotalPoint { get; set; }
        public double TotalCompanyCost { get; set; }
        public double TotalCustomerCost { get; set; }
        public int TotalInstalledQuantity { get; set; }
        public double PointSum { get; set; }

        public double TotalAmountByPage { get; set; }

        public string CustomerStatus { set; get; }





    }


    public class CSRActivity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CancelledAccount { get; set; }
        public int CreatedAccount { get; set; }
        public int AccountPlaced { get; set; }
        public int ContractSent { get; set; }
        public int InstallScheduled { get; set; }
        public int ServiceScheduled { get; set; }
    }

    public class SeviceTracker
    {
        public int Id { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int TicketId { get; set; }
        public string InstallerTechnician { get; set; }
        public string ServiceTechnician { get; set; }
        public string ServiceType { get; set; }
        public string Reason { get; set; }
        public DateTime ScheduledDate { get; set; }
        public DateTime TechOnsiteDate { get; set; }
    }

    public class CSRActivityModel
    {
        public List<CSRActivity> CSRActivityList{get; set; }
        public int TotalCount { get; set; }

        public int TotalCancelledAccount { get; set; }

        public int TotalCreatedAccount { get; set; }


        public int TotalContractSent { get; set; }

        public int TotalAccountPlaced { get; set; }

        public int TotalInstallScheduled { get; set; }

        public int TotalServicesScheduled { get; set; }

    }

    public class ServiceTrackerModel
    {
        public List<SeviceTracker> SeviceTrackerList { get; set; }
        public int TotalCount { get; set; }
    }
    public class TaskModel
    {
        public List<CustomerNote> TaskList { get; set; }
        public int TotalCount { get; set; }
    }
    public class AdditionalMemberAppointmentModel
    {
        public Guid TicketId { set; get; }
        public DateTime? AppointmentDate { set; get; }
        public string UserList { get; set; }
        public Guid CustomerId { get; set; }
    }
    public class CreateTicketModel
    {
        public List<CustomerNote> CustomerNotes { set; get; }
        public PackageCustomer PackageCustomermodel { get; set; }
        public Ticket Ticket { set; get; }
        public List<TicketUser> TicketAssignedUserList { set; get; }
        public List<TicketUser> TicketUserList { set; get; }
        public List<TicketUser> NotifyingUserList { set; get; }
        public List<TicketReply> TicketReplyList { set; get; }
        public int TicketDefaultTimeDuration { set; get; }
        public CustomerAppointment CustomerAppointment { get; set; }
        public Customer Customer { set; get; }
        public List<CustomerAppointmentEquipment> CustomerAppointmentEquipmentList { set; get; }
        public List<CustomerAppointmentEquipmentPoint> CustomerAppointmentEquipmentPointList { set; get; }
        public List<CustomerAppointmentEquipment> CustomerAppointmentServiceList { set; get; }
        public List<AdditionalMembersAppointment> memberAppointment { get; set; }
        //For Printing Pdf Only
        public Company Company { set; get; }
        public string CompanyAddressFormat { set; get; }
        public string CustomerAddressFormat { set; get; }
        public string ProfilePicture { get; set; }
        public string SoldBy { get; set; }

        public List<SalesCommission> SalesCommissionList { get; set; }
        public List<TechCommission> TechCommissionList { get; set; }
        public List<AddMemberCommission> AddMemberCommissionList { get; set; }
        public List<FinRepCommission> FinRepCommissionList { get; set; }
        public List<ServiceCallCommission> ServiceCallCommissionList { get; set; }
        public List<FollowUpCommission> FollowUpCommissionList { get; set; }
        public List<RescheduleCommission> RescheduleCommissionList { get; set; }
        public double InvoiceBalanceDue { get; set; }
        public string SearchType { get; set; }
        public List<Ticket> RefTicketTable { get; set; }
        public List<TicketBookingDetails> TicketBookingDetails { set; get; }
        public List<TicketBookingExtraItem> TicketBookingExtraItem { set; get; }
        public bool ShowNoteFirst { set; get; }
        public List<BillingCheckModel> ListBillingCheckModel { get; set; }
        public List<Lookup> RugCondtions { set; get; }

        public CustomerAddress PickupAddress { set; get; }
        public CustomerAddress DropofAddress { set; get; }
        public CustomerAddress ServiceAddress { set; get; }
    }
    public class TicketFilter
    {
        public Guid CustomerId { set; get; }
        public Guid CompanyId { set; get; }
        public Guid UserId { set; get; }
        public string SearchText { set; get; }
        public string TicketType { set; get; }
        public string[] TicketStatusArr { get; set; }
        public string TicketStatus { set; get; }
        public string OwnerShip { set; get; }
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

        public string technician { get; set; }
        public string[] technicianlist { get; set; }

        public string salesperson { get; set; }
        public string seletActive { get; set; }
        public string EquipmentStatus { get; set; }
        public string AllEquipment { get; set; }
        public bool Generate { get; set; }

        public string CustomerStatus { set; get; }



    }
    public class AssignTicketFilter
    {
        public Guid? UserId { get; set; }
        public string TicketStatus { get; set; }
        public string TicketType { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string IsInstallation { get; set; }
        public List<Lookup> TaskLookupList { get; set; }
        public List<Lookup> JobLookupList { get; set; } 
        public string ScheduleMinDate { get; set; }

        public string ScheduleMaxDate { get; set; }

 
    }
    public class TicketSchedule
    {
        public Guid TicketId { set; get; }
        public string StartTime { set; get; }
        public string EndTime { set; get; }
        public string EmployeeName { set; get; }
    }
    public class TicketSummary
    {
        public int CusIdInt { set; get; }
        public int TicketIntId { set; get; }
        public int PageNo { set; get; }
        public int PageSize { set; get; }
        public string Searchtext { set; get; }
        public int ExceedQuantity { get; set; }
        public string CustomerNo { set; get; }
        public bool IsClosed { set; get; }
        public string TicketTypeVal { set; get; }
    }

    public class PRReport
    {
        public List<PRReport> PRReportList { set; get; }
        public int TicketId { set; get; }
        public int CustomerId { set; get; }
        public string CustomerNo { set; get; }
        public string TicketType { set; get; }
        public string TicketStatus { set; get; }
        public string Name { set; get; }
        public string EquipmentNames { set; get; }
        public double TotalPayments { set; get; }
        public ListCount Count { set; get; }
        public TotalCost TotalCost { get; set; }
    }
    public class ListCount
    {
        public int Count { get; set; }
    }
    public class TotalCost
    {
        public double EquipmentCost { get; set; }
        public double Amount { get; set; }
    }
    public class InstalledTicket
    {
        public ListCount Count { set; get; }
        public List<InstalledTicket> InstalledTicketList { set; get; }
        public int TicketId { set; get; }
        public int RMAAccountNo { set; get; }
        public string CustomerNo { set; get; }
        public string OpenerName { set; get; }
        public string SoldBy { set; get; }
        public string SoldBy2 { set; get; }
        public string SoldBy3 { set; get; }
        public string SoldBy4 { set; get; }
        public string CustomerName { set; get; }
        public DateTime InstallDate { get; set; }
        public string Address { set; get; }
        public string MonthlyMonitoringFee { set; get; }
        public string EquipmentNames { set; get; }
        public string EquipmentPoint { set; get; }
        public double EquipmentCost { set; get; }
        public double TotalRevenue { set; get; }
		public string ContractTerm { set; get; }
		public string PaymentMethod { set; get; }

        public string PaymentMethodVal { set; get; }

        public string Technician { set; get; }
        public string AccountStatus { set; get; }
		public string CustomerType { set; get; }
        public DateTime ServiceShedule { set; get; }
		public string FundedStatus { set; get; }
        public string CreditScore { set; get; }
        public int NumnberOfPayment { set; get; }
        public string UserGroup { set; get; }
        public TotalCost TotalCost { set; get; }
    }
}
