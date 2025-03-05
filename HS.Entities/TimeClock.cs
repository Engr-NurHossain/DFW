using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Office.Interop.Excel;
using System.ComponentModel;

namespace HS.Entities
{
	public partial class TimeClock 
	{
		public string EmployeeName { set; get; }
        public string TimeSpend { get; set; }
   
        private string _StrTime { set; get; }
        public string StrTime { get { return _StrTime; }
            set
            {
                _StrTime = value;
                this.Time = value.ToDateTime();
            }
        }
        public string LastUpdatedName { get; set; }
    }

    public class TimeClockViewModel
    {
        public List<TimeClock> TimeClockList { set; get; }
        public int PageNo { set; get; }
        public int PageSize { set; get; }
        public int TotalCount { set; get; }
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
    }
    public class TimeClockReport
    {
        public string Name { get; set; }
        public string ClockInDate { set; get; }
        public string ClockInTime { set; get; }
        public string ClockInNote { set; get; }
        public string ClockOutDate { set; get; }
        public string ClockOuTime { set; get; }
        public string ClockOutNote { set; get; }
        public string TimeSpent { set; get; }
        public string TotalHours { get; set; }
    }
    
    public class EmpPayrollReport
    {
        public int Id { get; set; }
        public string EmpName { set; get; }
        public string RegularHours { set; get; }
        public double OTOHours { set; get; }
        public string PTOHours { set; get; }
        public string HourlyRate { get; set; }
        public Guid UserId { get; set; }
    }

    public class EmpPayrollFilter
    {
        public List<EmpPayrollReport> ListEmpPayrollReport { get; set; }
        public PayrollTotalCount PayrollTotalCount { get; set; }
        public List<Employee> ListEmployeePayroll { get; set; }
    }

    public class EmpPayrollReports
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string EmpName { set; get; }
        public string RegularHours { set; get; }
        public double OTOHours { set; get; }
        public string PTOHours { set; get; }
        public double HourlyRate { get; set; }
        public double TotalPay { get; set; }
    }

    public class EmpPayrollFilters
    {
        public List<EmpPayrollReports> ListEmpPayrollReport { get; set; }
        public PayrollTotalCount PayrollTotalCount { get; set; }
        public List<Employee> ListEmployeePayroll { get; set; }
    }

    public class PayrollTotalCount
    {
        public int CountTotal { get; set; }
    }

    public class EmpPtoType
    {
        public Guid UserId { set; get; }
        public string Type { get; set; }
    }
    public class EmpPtoReport
    {
        public string User { set; get; }
        [DisplayName("Total Time")]
        public string TotalTime { set; get; }
        [DisplayName("Approved PTO Hours")]
        public string PtoHours { set; get; }
        [DisplayName("PTO Rate")]
        public string PtoRates { set; get; }
    }

    public class TimeClockFilterModel
    {
        public List<EmployeeTimeClock> ListTimeClock { get; set; }
        public TotalCount TotalCount { get; set; }
        public TotalCount AllTotalClockedInSeconds { get; set; }
        public RegularHourSummary RegularHour { get; set; }
        public PtoHourSummary PtoHour { get; set; }
    }

    public class TotalCount
    {
        public int CountTotal { get; set; }
    }
    public class RegularHourSummary
    {
        public double RegularHour { get; set; }
    }
    public class PtoHourSummary
    {
        public double PtoHour { get; set; }
    }

}
