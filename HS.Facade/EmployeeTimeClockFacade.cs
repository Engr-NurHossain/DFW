using HS.DataAccess;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Facade
{
   public class EmployeeTimeClockFacade : BaseFacade
    {
        EmployeeTimeClockDataAccess _EmployeeTimeClockDataAccess = null;
        public EmployeeTimeClockFacade(ClientContext clientContext)
            : base(clientContext)
        {
            if (_EmployeeTimeClockDataAccess == null)
                _EmployeeTimeClockDataAccess = (EmployeeTimeClockDataAccess)_ClientContext[typeof(EmployeeTimeClockDataAccess)];
        }
     
        public EmployeeTimeClockFacade()
        {
          
        }
        public EmployeeTimeClockFacade(string ConStr)
        {
            if (_EmployeeTimeClockDataAccess == null)
                _EmployeeTimeClockDataAccess = new EmployeeTimeClockDataAccess(ConStr);
        }
        
        public int InsertEmployeeTimeClock(EmployeeTimeClock TC)
        {
            return (int)_EmployeeTimeClockDataAccess.Insert(TC);
        }
        public EmployeeTimeClock GetEmployeeLastTimeClockByUserId(Guid UserId)
        {
            var Query = string.Format("UserId='{0}' order by ClockInTime desc", UserId);
            var EmployeeTimeClock= _EmployeeTimeClockDataAccess.GetByQuery(Query).FirstOrDefault();
            return EmployeeTimeClock;
        }
        public bool UpdateEmployeeTimeClock(EmployeeTimeClock TC)
        {
            return _EmployeeTimeClockDataAccess.Update(TC)>0;
        }
        public TimeClockFilterModel GetLastClocksByUserIdAndTimePeriod(string userId, DateTime StartDate, DateTime EndDate, string order, int pageno, int pagesize)
        {
            DataSet ds = _EmployeeTimeClockDataAccess.GetLastClocksByUserIdAndTimePeriod(userId, StartDate, EndDate, order, pageno, pagesize);

            List<EmployeeTimeClock> buildList = new List<EmployeeTimeClock>();
            buildList = (from DataRow dr in ds.Tables[0].Rows
                         select new EmployeeTimeClock()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             UserId = (Guid)dr["UserId"],
                             ClockInTime = dr["ClockInTime"] != DBNull.Value ? Convert.ToDateTime(dr["ClockInTime"]) : new DateTime(),
                             ClockOutTime = dr["ClockOutTime"] != DBNull.Value ? Convert.ToDateTime(dr["ClockOutTime"]) : new DateTime(),
                             ClockInLat = dr["ClockInLat"].ToString(),
                             ClockInLng = dr["ClockInLng"].ToString(),
                             ClockOutLat = dr["ClockOutLat"].ToString(),
                             ClockOutLng = dr["ClockOutLng"].ToString(),
                             ClockInNote = dr["ClockInNote"].ToString(),
                             ClockOutNote = dr["ClockOutNote"].ToString(),
                             LastUpdatedName = dr["LastUpdatedName"].ToString(),
                             ClockInCreatedBy = (Guid)dr["ClockInCreatedBy"],
                             ClockOutCreatedBy = (Guid)dr["ClockOutCreatedBy"],
                             ClockedInSeconds = dr["ClockedInSeconds"] != DBNull.Value ? Convert.ToInt32(dr["ClockedInSeconds"]) : 0,
                             LastUpdateBy = (Guid)dr["LastUpdateBy"],
                             LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime()
                         }).ToList();

            TotalCount TotalCount = new TotalCount();
            TotalCount = (from DataRow dr in ds.Tables[1].Rows
                          select new TotalCount()
                          {
                              CountTotal = dr["CountTotal"] != DBNull.Value ? Convert.ToInt32(dr["CountTotal"]) : 0
                          }).FirstOrDefault();

            TotalCount AllTotalClockedInSeconds = new TotalCount();
            AllTotalClockedInSeconds = (from DataRow dr in ds.Tables[2].Rows
                          select new TotalCount()
                          {
                              CountTotal = dr["AllTotalClockedInSeconds"] != DBNull.Value ? Convert.ToInt32(dr["AllTotalClockedInSeconds"]) : 0
                          }).FirstOrDefault();
            RegularHourSummary regularHour = new RegularHourSummary();
            regularHour = (from DataRow dr in ds.Tables[3].Rows
                                        select new RegularHourSummary()
                                        {
                                            RegularHour = dr["RegularHour"] != DBNull.Value ? Convert.ToDouble(dr["RegularHour"]) : 0.0,
                                        }).FirstOrDefault();
            PtoHourSummary ptoHour = new PtoHourSummary();
            ptoHour = (from DataRow dr in ds.Tables[4].Rows
                           select new PtoHourSummary()
                           {
                               PtoHour = dr["PtoHour"] != DBNull.Value ? Convert.ToDouble(dr["PtoHour"]) : 0.0,
                           }).FirstOrDefault();
            TimeClockFilterModel TimeClockFilterModel = new TimeClockFilterModel();
            TimeClockFilterModel.ListTimeClock = buildList;
            TimeClockFilterModel.TotalCount = TotalCount;
            TimeClockFilterModel.AllTotalClockedInSeconds = AllTotalClockedInSeconds;
            TimeClockFilterModel.RegularHour = regularHour;
            TimeClockFilterModel.PtoHour = ptoHour;
            return TimeClockFilterModel;
        }

        public EmployeeAccrualPtoAndApprovePtohourModel GetLastClocksAccrualPTOByUserIdAndTimePeriod(string userId, DateTime StartDate, DateTime EndDate)
        {
            EmployeeAccrualPtoAndApprovePtohourModel model = new EmployeeAccrualPtoAndApprovePtohourModel();    
            DataSet ds = _EmployeeTimeClockDataAccess.EmployeeTimeClockListAccrualPtoByUserId(userId, StartDate, EndDate);

            List<EmployeePTOHourLog> PTOHourLog = new List<EmployeePTOHourLog>();
            model.EmployeePTOHourLogList = (from DataRow dr in ds.Tables[0].Rows
                         select new EmployeePTOHourLog()
                         {
                             EmployeeName = dr["EmployeeName"].ToString(), 
                             Totalearned = dr["Totalearned"] != DBNull.Value ? Convert.ToDouble(dr["Totalearned"]) : 0.0,
                             TotalUsed = dr["TotalPtoUsedHour"] != DBNull.Value ? Convert.ToDouble(dr["TotalPtoUsedHour"]) : 0.0,
                             TotalAvailable = dr["TotalAvailable"] != DBNull.Value ? Convert.ToDouble(dr["TotalAvailable"]) : 0.0,
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             HireDate = dr["HireDate"] != DBNull.Value ? Convert.ToDateTime(dr["HireDate"]) : new DateTime(),
                             PayType = dr["PayType"].ToString(),
                             UserId = (Guid)dr["UserId"],

                         }).ToList();

            model.schedulerList = (from DataRow dr in ds.Tables[1].Rows
                                            select new EmployeePTOHourLog()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                UserId = (Guid)dr["UserId"],
                                                PTOHour = dr["PTOHour"] != DBNull.Value ? Convert.ToDouble(dr["PTOHour"]) : 0.0,
                                                FromDate = dr["FromDate"] != DBNull.Value ? Convert.ToDateTime(dr["FromDate"]) : new DateTime(),
                                                EndDate = dr["EndDate"] != DBNull.Value ? Convert.ToDateTime(dr["EndDate"]) : new DateTime(),
                                                CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                                LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                                WorkingHours = dr["WorkingHours"] != DBNull.Value ? Convert.ToDouble(dr["WorkingHours"]) : 0.0,
                                                TotalUsed = dr["Used"] != DBNull.Value ? Convert.ToDouble(dr["Used"]) : 0.0,
                                                Balance = dr["TotalBalance"] != DBNull.Value ? Convert.ToDouble(dr["TotalBalance"]) : 0.0, 
                                            }).ToList();
            model.approveLogList = (from DataRow dr in ds.Tables[2].Rows
                                             select new EmployeePTOHourLog()
                                             {
                                                 TotalRequestedHour = dr["TotalHour"] != DBNull.Value ? Convert.ToDouble(dr["TotalHour"]) : 0.0,
                                                 CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                                 LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                                 Status = dr["Status"].ToString(),
                                             }).ToList(); 
            return model;
        }

        public EmployeeTimeClock GetEmployeeTimeClockById(int Id)
        {
            return _EmployeeTimeClockDataAccess.Get(Id);
        }

        public List<EmployeeTimeClock> GetAutoClockOutList()
        {
            DataTable dt = _EmployeeTimeClockDataAccess.GetAutoClockOutList();
            List<EmployeeTimeClock> asd = (from DataRow dr in dt.Rows
                                           select new EmployeeTimeClock()
                                           {
                                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                               ClockInTime = dr["ClockInTime"] != DBNull.Value ? Convert.ToDateTime(dr["ClockInTime"]) : new DateTime(),
                                               ClockOutTime = dr["ClockInTime"] != DBNull.Value ? Convert.ToDateTime(dr["ClockInTime"]) : new DateTime(),
                                               ClockInNote = dr["ClockInNote"].ToString(),
                                               ClockInCreatedBy = (Guid)dr["ClockInCreatedBy"],
                                               ClockInLat = dr["ClockInLat"].ToString(),
                                               ClockInLng = dr["ClockInLng"].ToString(),
                                     
                                               UserId = (Guid)dr["UserId"],
                                           }).ToList();
            return asd;
        }

        public bool DeleteEmployeeTimeClock(int value)
        {
            return _EmployeeTimeClockDataAccess.Delete(value) > 0;
        }

        public List<EmployeeTimeClock> GetAllEmployeeTimeClockListByDateFilter(DateTime indate, DateTime outdate, Guid userid)
        {
            return _EmployeeTimeClockDataAccess.GetByQuery(string.Format("((ClockInTime between '{0}' and '{1}' or ClockOutTime between '{0}' and '{1}') or ('{0}'  between ClockInTime and ClockOutTime or '{1}'  between ClockInTime and ClockOutTime)) and UserId = '{2}'", indate, outdate, userid)).ToList();
        }
        public List<EmployeeTimeClock> GetAllEmployeeTimeClockListByDateFilterAndId(DateTime indate, DateTime outdate, Guid userid,int id)
        {
            string indd = indate.ToString("yyyy-MM-dd HH:mm:ss:fff");
            string outdd = outdate.ToString("yyyy-MM-dd HH:mm:ss:fff");
            return _EmployeeTimeClockDataAccess.GetByQuery(string.Format("((ClockInTime between '{0}' and '{1}' or ClockOutTime between '{0}' and '{1}') or ('{0}'  between ClockInTime and ClockOutTime or '{1}'  between ClockInTime and ClockOutTime)) and UserId = '{2}' and Id != {3}", indate.ToString("yyyy-MM-dd HH:mm:ss:fff"), outdate.ToString("yyyy-MM-dd HH:mm:ss:fff"), userid, id)).ToList();
        }
    }
}
