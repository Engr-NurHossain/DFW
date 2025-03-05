using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace HS.Facade
{
    public class TimeClockFacade:BaseFacade
    {
        TimeClockDataAccess _TimeClockDataAccess = null;

        public TimeClockFacade(ClientContext clientContext)
            : base(clientContext)
        {
            if (_TimeClockDataAccess == null)
                _TimeClockDataAccess = (TimeClockDataAccess)_ClientContext[typeof(TimeClockDataAccess)];

        }
        public TimeClockFacade(string ConStr)
        {
            if (_TimeClockDataAccess == null)
                _TimeClockDataAccess = new TimeClockDataAccess(ConStr);
        }

        public List<TimeClock> GetAllTimeClock()
        {
            return _TimeClockDataAccess.GetAll();
        }


        public int InsertTimeClock(TimeClock TC)
        {
            return (int)_TimeClockDataAccess.Insert(TC);
        }
        public List<TimeClock> GetLastClocksByUserIdAndTimePeriod(Guid userId, DateTime StartDate, DateTime EndDate, string order)
        {
            return _TimeClockDataAccess.GetLastClocksByUserIdAndTimePeriod(userId, StartDate, EndDate, order);
        }
        public TimeClockFilterModel GetLastClocksByUserIdAndTimePeriod(Guid userId,DateTime StartDate,DateTime EndDate, string order, int pageno, int pagesize)
        {
            DataSet ds = _TimeClockDataAccess.GetLastClocksByUserIdAndTimePeriod(userId, StartDate, EndDate, order, pageno, pagesize);

            List<TimeClock> buildList = new List<TimeClock>();
            buildList = (from DataRow dr in ds.Tables[0].Rows
                         select new TimeClock()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             UserId = (Guid)dr["UserId"],
                             Time = dr["Time"] != DBNull.Value ? Convert.ToDateTime(dr["Time"]) : new DateTime(),
                             Type = dr["Type"].ToString(),
                             Lat = dr["Lat"].ToString(),
                             Lng = dr["Lng"].ToString(),
                             Note = dr["Note"].ToString(),
                             LastUpdatedName = dr["LastUpdatedName"].ToString(),
                             CreatedBy = (Guid)dr["CreatedBy"],
                             ClockedInMinutes = dr["ClockedInMinutes"] != DBNull.Value ? Convert.ToInt32(dr["ClockedInMinutes"]) : 0,
                             LastUpdateBy = (Guid)dr["LastUpdateBy"],
                             LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime()
                         }).ToList();

            TotalCount TotalCount = new TotalCount();
            TotalCount = (from DataRow dr in ds.Tables[1].Rows
                                 select new TotalCount()
                                 {
                                     CountTotal = dr["CountTotal"] != DBNull.Value ? Convert.ToInt32(dr["CountTotal"]) : 0
                                 }).FirstOrDefault();
            TimeClockFilterModel TimeClockFilterModel = new TimeClockFilterModel();
            //TimeClockFilterModel.ListTimeClock = buildList;
            TimeClockFilterModel.TotalCount = TotalCount;
            return TimeClockFilterModel;
        }

        public List<TimeClock> GetAllTimeClockByUserIdANDClockInTime(Guid userId, DateTime ClockInTime)
        {
            return _TimeClockDataAccess.GetByQuery(string.Format("UserId = '{0}' AND Time >= '{1}'", userId, ClockInTime));
        }
        public List<TimeClock> GetAllTimeClockByUserIdANDClockOutTime(Guid userId, DateTime ClockInTime)
        {
            return _TimeClockDataAccess.GetByQuery(string.Format("UserId = '{0}' AND Time <= '{1}'", userId, ClockInTime));
        }
        
        //public List<EmployeeTimeClockReportModel> GetEmployeeTimeClockReportByUserId(Guid userId,DateTime StartDate,DateTime EndDate)
        //{
        //    DataTable dt = _TimeClockDataAccess.GetEmployeeTimeClockReportByUserId(userId, StartDate,EndDate);
        //    List<EmployeeTimeClockReportModel> model = (from DataRow dr in dt.Rows
        //        select new EmployeeTimeClockReportModel()
        //        {
        //            EmployeeName = dr["EmployeeName"].ToString(), 
        //            Note = dr["Note"].ToString(),
        //            Time = dr["Time"] != DBNull.Value ? Convert.ToDateTime(dr["Time"]) : new DateTime(),
        //            Type = dr["Type"].ToString(),
        //            ClockedInMinutes = dr["ClockedInMinutes"] != DBNull.Value ? Convert.ToInt32(dr["ClockedInMinutes"]) : 0
        //        }).ToList();
        //    return model;

        //}

        public TimeClockViewModel GetAllUsersInOutreportByFilter(PayrollFilterModel filter)
        {
            TimeClockViewModel model = new TimeClockViewModel();

            DataSet ds =  _TimeClockDataAccess.GetAllUsersInOutreportByFilter(filter); 
            model.TimeClockList = (from DataRow dr in ds.Tables[0].Rows
                                   select new TimeClock()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0, 
                               CreatedBy = (Guid)dr["CreatedBy"],
                               Lat = dr["Lat"].ToString(),
                               Lng = dr["Lng"].ToString(),
                               EmployeeName = dr["EmployeeName"].ToString(),
                               UserId = (Guid)dr["UserId"],
                               Note = dr["Note"].ToString(),
                               Time = dr["Time"] != DBNull.Value ? Convert.ToDateTime(dr["Time"]) : new DateTime(),
                               Type = dr["Type"].ToString(),
                               ClockedInMinutes = dr["ClockedInMinutes"] != DBNull.Value ? Convert.ToInt32(dr["ClockedInMinutes"]) : 0,
                           }).ToList();

            model.TotalCount = ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;

            model.StartDate = filter.StartDate;
            model.EndDate = filter.EndDate;
            model.PageNo = filter.PageNo.Value;
            model.PageSize = filter.PageSize.Value;


            return model;
        }

        public List<EmployeeTimeClock> GetAllEmploployeeTimeClockReport(string SupervisorId, DateTime StartDate, DateTime EndDate)
        {
            DataTable dt = _TimeClockDataAccess.GetAllEmploployeeTimeClockReport(SupervisorId, StartDate, EndDate);
            List<EmployeeTimeClock> asd = (from DataRow dr in dt.Rows
                                           select new EmployeeTimeClock()
                                           {
                                           
                                               ClockInTime = dr["ClockInTime"] != DBNull.Value ? Convert.ToDateTime(dr["ClockInTime"]) : new DateTime(),
                                               ClockOutTime = dr["ClockOutTime"] != DBNull.Value ? Convert.ToDateTime(dr["ClockOutTime"]) : new DateTime(),
                                               ClockInNote = dr["ClockInNote"].ToString(),
                                               ClockOutNote = dr["ClockOutNote"].ToString(),
                                               ClockedInSeconds = dr["ClockedInSeconds"] != DBNull.Value ? Convert.ToInt32(dr["ClockedInSeconds"]) : 0,
                                               LastUpdatedName = dr["LastUpdatedName"].ToString()
                                           }).ToList();
            return asd;
        }

        public List<TimeClock> GetAllEmploployeeTimeClockReport(Guid UserId)
        {
            return _TimeClockDataAccess.GetByQuery(string.Format("UserId = '{0}'", UserId));
        }
        public List<EmpPtoType> GetAllPtoReport(Guid SupervisorId, DateTime StartDate, DateTime EndDate, DateTime FilterStartDate, DateTime FilterEndDate)
        {
            DataTable dt = _TimeClockDataAccess.GetAllPtoReport(SupervisorId, StartDate, EndDate, FilterStartDate, FilterEndDate);
            List<EmpPtoType> asd = (from DataRow dr in dt.Rows
                                   select new EmpPtoType()
                                   {

                                      // Time = dr["Time"] != DBNull.Value ? Convert.ToDateTime(dr["Time"]) : new DateTime(),
                                       UserId = new Guid(dr["UserId"].ToString()),
                                       Type = dr["EmployeeName"].ToString()

                                   }).ToList();
            return asd;
        }
        public EmpPayrollFilter GetAllPayrollReport(Guid SupervisorId,DateTime FilterStartDate,DateTime FilterEndDate, string order, int pageno, int pagesize,Guid UserId,string CurrentEmployee)
        {
            DataSet ds = _TimeClockDataAccess.GetAllPayrollReport(SupervisorId,FilterStartDate,FilterEndDate, order, pageno, pagesize, UserId,CurrentEmployee);
            List<EmpPayrollReport> buildList = new List<EmpPayrollReport>();
            buildList = (from DataRow dr in ds.Tables[0].Rows
                         select new EmpPayrollReport()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             EmpName = dr["EmpName"].ToString(),
                             RegularHours = dr["RegularHours"].ToString(),
                             OTOHours = Math.Round(dr["OTOHours"] != DBNull.Value ? Convert.ToDouble(dr["OTOHours"]) : 0.0,2),
                             PTOHours = dr["PTOHours"].ToString(),
                             UserId = (Guid)dr["UserId"]
                         }).ToList();

            PayrollTotalCount PayrollTotalCount = new PayrollTotalCount();
            PayrollTotalCount = (from DataRow dr in ds.Tables[1].Rows
                          select new PayrollTotalCount()
                          {
                              CountTotal = dr["CountTotal"] != DBNull.Value ? Convert.ToInt32(dr["CountTotal"]) : 0
                          }).FirstOrDefault();
            EmpPayrollFilter EmpPayrollFilter = new EmpPayrollFilter();
            EmpPayrollFilter.ListEmpPayrollReport = buildList;
            EmpPayrollFilter.PayrollTotalCount = PayrollTotalCount;
            return EmpPayrollFilter;
        }

        public EmpPayrollFilters GetAllPayrollReportForReports(Guid SupervisorId, DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, Guid userid,string CurrentEmployee)
        {
            DataSet ds = _TimeClockDataAccess.GetAllPayrollReportForReports(SupervisorId, FilterStartDate, FilterEndDate, order, pageno, pagesize, userid, CurrentEmployee);
            List<EmpPayrollReports> buildList = new List<EmpPayrollReports>();
            buildList = (from DataRow dr in ds.Tables[0].Rows
                         select new EmpPayrollReports()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             UserId = dr["UserId"] != DBNull.Value ? Convert.ToInt32(dr["UserId"]) : 0,
                             EmpName = dr["EmpName"].ToString(),
                             RegularHours = dr["RegularHours"].ToString(),
                             OTOHours = Math.Round(dr["OTOHours"] != DBNull.Value ? Convert.ToDouble(dr["OTOHours"]) : 0,2),
                             PTOHours = dr["PTOHours"].ToString(),
                             HourlyRate = Math.Round(dr["HourlyRate"] != DBNull.Value ? Convert.ToDouble(dr["HourlyRate"]) : 0, 2),
                             TotalPay = Math.Round(dr["totalpay"] != DBNull.Value ? Convert.ToDouble(dr["totalpay"]) : 0, 2),
                         }).ToList();

            PayrollTotalCount PayrollTotalCount = new PayrollTotalCount();
            PayrollTotalCount = (from DataRow dr in ds.Tables[1].Rows
                                 select new PayrollTotalCount()
                                 {
                                     CountTotal = dr["CountTotal"] != DBNull.Value ? Convert.ToInt32(dr["CountTotal"]) : 0
                                 }).FirstOrDefault();
            EmpPayrollFilters EmpPayrollFilter = new EmpPayrollFilters();
            EmpPayrollFilter.ListEmpPayrollReport = buildList;
            EmpPayrollFilter.PayrollTotalCount = PayrollTotalCount;
            return EmpPayrollFilter;
        }

        public List<AutoClockOutModel> GetAutoClockOutList()
        {
            DataTable dt = _TimeClockDataAccess.GetAutoClockOutList();
            List<AutoClockOutModel> asd = (from DataRow dr in dt.Rows
                select new AutoClockOutModel()
                { 
                    ClockedInOutTime = dr["ClockedInOutTime"] != DBNull.Value ? Convert.ToDateTime(dr["ClockedInOutTime"]) : new DateTime(),
                    ClockedInOutStatus = dr["ClockedInOutStatus"].ToString(),
                    EmpName = dr["EmpName"].ToString(),
                    UserId = (Guid)dr["UserId"],
                }).ToList();
            return asd;
        }

        public TimeClock GetTimeClockById(int Id)
        {
            return _TimeClockDataAccess.Get(Id);
        }

        public bool UpdateTimeClock(TimeClock clockAdd)
        {
            return _TimeClockDataAccess.Update(clockAdd) > 0;
        }
    }
}
