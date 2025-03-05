using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Entities
{
    public class CalEmployee
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
    public class CalTicketType
    {
        public string TicketType { get; set; }

    }
    public class CalAppointmentInfo
    {
        public Guid CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CellNo { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public Guid BookingId { get; set; }
        public Guid TicketId { get; set; }
        public string TicketType { get; set; }
        public string Status { get; set; }
        public Guid AppointmentId { get; set; }
        public string AppointmentType { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentStartTime { get; set; }
        public string AppointmentEndTime { get; set; }
        public bool IsAllDay { get; set; }
        public string Email { get; set; }
        public Guid UserId { get; set; }
        
    }

    public class CalListofTime
    {
        public List<CalListModel> TimeList { get; set; }
        public List<CalEmpModel> EmpList { get; set; }
        public List<string> TTypeList { get; set; }
        public string EventDate { get; set; }
    }
    public class CalListModel
    {
        public int SL { get; set; }
        public string TimeName { get; set; }
        public string DateName { get; set; }
        public int DayNumber { get; set; }
        public string WeekEnd { get; set; }
        public string WorkingTime { get; set; }
        public List<HolidayEmployeeInfoModel> Holidays { get; set; }
    }
    public class CalListModelList
    {
        public List<CalListModel> WeekList { get; set; }
        public int WeekCount { get; set; }
        public string MonthName { get; set; }
    }
    public class CalEmpModel
    {

        public string EmpName { get; set; }
        public string EmpGuidId { get; set; }
        public int? EmpIntId { get; set; }
        public string Empdate { get; set; }
        public int TaskCount { get; set; }
        public string HolidayStatus { get; set; }
        public int ErrorCount { get; set; }
        public string ErrorTitleStatus { get; set; }
        public string ErrorTicketIdEditList { get; set; }
        public string AvailablityTime { get; set; }
        public string GroupName { get; set; }
        public bool IsCompanyHoliday { get; set; }
    }

    public class CalAppointmentInfoList
    {
        public int SL { get; set; }
        public string CustomerId { get; set; }
        public string Name { get; set; }
        public string CellNo { get; set; }
        public string Address { get; set; }
        public string TicketId { get; set; }
        public string TicketType { get; set; }
        public string Status { get; set; }
        public string AppointmentId { get; set; }
        public string AppointmentType { get; set; }
        public string AppointmentDate { get; set; }
        public string AppointmentStartTime { get; set; }
        public string AppointmentEndTime { get; set; }
        public bool IsAllDay { get; set; }
        public bool IsCalled { get; set; }
        public string UserId { get; set; }
        public int? EmpId { get; set; }
        public int CustomerIntId { get; set; }
        public List<string> Time { get; set; }
        public int count { get; set; }
        public string DayStartTime { get; set; }
        public string DayEndTime { get; set; }
        public string StatusColor { get; set; }
        public string BGColor { get; set; }
        public string LeftIcon { get; set; }
        public string StreetAddress { get; set; }
        public string Border { get; set; }
        public string TitleString { get; set; }
        public int StartDivCount { get; set; }
        public int EndDivCount { get; set; }
        public string EmployeeName { get; set; }
        public string TicketTypeDisplayText { get; set; }
        public string BookingId { get; set; }
        public string AdditionalMember { get; set; }
        public string TicketAddress { get; set; }
        public string PopupDetails { get; set; }
        public string TopPadding { get; set; }
        public int OverLodding { get; set; }
        public string Subject { get; set; }
        public double TicketAmount { get; set; }

    }
    public class DailyInfoListModel
    {
        public List<CalAppointmentInfoList> AppList { get; set; }
        public List<CalEmpModel> EmpList { get; set; }
        public List<CalListModel> Timelist { get; set; }
        public CalEmpModel SystemUser { get; set; }
    }
    public class OverLoaddingModel
    {
        public int SL { get; set; }
        public int OverLoad { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int? EmpId { get; set; }
    }
    public class WeeklyInfoListModel
    {
        public List<CalAppointmentInfoList> AppList { get; set; }
        public List<CalEmpModel> EmpList { get; set; }
        public List<CalListModel> Timelist { get; set; }
        public List<DailyInfoShow> WeeklyInfo { get; set; }
        public List<DailyServiceInfoCount> WeeklyTotalInfo { get; set; }
        public TotalServiceInfoCountCollection ServiceCount { get; set; }
    }
    public class MonthlyInfoListModel
    {
        public List<CalListModelList> DayForCalendar { get; set; }
        public List<MonthlyGroupbyInfo> WeeklyTotalDataShow { get; set; }
        public List<DailyServiceInfoCountList> DayDataShow { get; set; }
        public List<WeeklyServiceInfoCount> TicketTypeList { get; set; }
    }
    public class ServiceCount
    {
        public int CountNumber { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDate { get; set; }
        public string EmpId { get; set; }

    }
    public class DailyInfoShow
    {        
        public string AppointmentDate { get; set; }
        public string DayName { get; set; }
        public string LeftIcon { get; set; }
        public string BGColor { get; set; }
        public string Border { get; set; }
        public List<DailyInfoList> dailyInfolist { get; set; }

    }
    public class DailyInfo
    {
        public string NameInfo { get; set; }
        public string AppointmentId { get; set; }
        public int CountNumber { get; set; }
        public string ServiceName { get; set; }
        public string EmpId { get; set; }
        public string AppDate { get; set; }
        public string PopUpString { get; set; }
    }
    public class DailyInfoList
    {
        public List<DailyInfo> dailyInfos { get; set; }
    }
    public class WeeklyServiceInfoCount
    {
        public string ServiceName { get; set; }
        public string Service { get; set; }        
        public int SCount { get; set; }
    }
    public class WeeklyServiceInfoCountList
    {
        public string Empid { get; set; }
        public int WeeklyTotal { get; set; }
        public int WeeklyAvgCount { get; set; }
        public List<WeeklyServiceInfoCount> WeeklyServiceTotal { get; set; }
    }
    public class TotalServiceInfoCountCollection
    {
        public List<WeeklyServiceInfoCountList> WeeklyServiceTotalList { get; set; }
        public List<DailyServiceInfoCountList> DailyServiceTotalList { get; set; }
        public List<EmpServiceInfoCount> EmpServiceTotal { get; set; }
        public List<string> TicketTypeList { get; set; }
    }
    public class DailyServiceInfoCount
    {
        public string ServiceName { get; set; }
        public string Service { get; set; }
        public int SCount { get; set; }
    }
    public class DailyServiceInfoCountList
    {
        public int DailyTotal { get; set; }
        public string AppDate { get; set; }
        public int DailyAvgCount { get; set; }
        public List<DailyServiceInfoCount> DailyServiceTotal { get; set; }
    }

    public class EmpServiceInfoCount
    {
        public string ServiceName { get; set; }
        public string Service { get; set; }
        public string Empid { get; set; }
        public string AppDate { get; set; }
        public int SCount { get; set; }
    }
    public class MonthlyGroupbyInfo
    {
        public int keyValue { get; set; }
        public List<MonthlyServiceInfoCount> MonthInfo { get; set; }
    }
    public class MonthlyServiceInfoCount
    {
        public int ServiceTotal { get; set; }
        public string ServiceName { get; set; }
        public string ServiceValue { get; set; }
        public int AvgCount { get; set; }
        public int Week { get; set; }

    }

    public class HolidayEmployeeInfoModel
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; } 
        public string EndTime { get; set; }
        public string Type { get; set; }
        public string PaidStatus { get; set; }
        public string Status { get; set; }
    }
    public class HolidayEmpModel
    {
        public string UserId { get; set; }
        public string StartDate { get; set; }
        public string Type { get; set; }
    }
    public class TimeNotMatchedTicketInfoCount
    {
        public string EmpId { get; set; }
        public string StrTitle { get; set; }
        public string StrEditList { get; set; }
        public int ErrorCount { get; set; }
    }
    public class MapSessionModel
    {
        public string SelectedDate { get; set; }
        public string ViewName { get; set; }
    }
    public class InActiveTicketsModel
    {
        public int TotalCount { get; set; }
        public bool IsActive { get; set; }
    }
    public enum DaysMonday
    {
        Monday = 1, Tuesday = 2, Wednesday = 3, Thursday = 4, Friday = 5, Saturday = 6, Sunday = 7 
    }
    public enum DaysSunday
    {
        Sunday = 1,  Monday = 2, Tuesday = 3, Wednesday = 4, Thursday = 5, Friday = 6, Saturday = 7
    }
    public enum DaysSaturday
    {
        Saturday = 1, Sunday = 2, Monday = 3, Tuesday = 4, Wednesday = 5, Thursday = 6, Friday = 7
    }

}
