using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.DataAccess
{
    public partial class TimeClockDataAccess
    {
        public TimeClockDataAccess(string ConStr) : base(ConStr) { }
        public List<TimeClock> GetLastClocksByUserIdAndTimePeriod(Guid userId, DateTime StartDate, DateTime EndDate, string order)
        {
            string sqlQuery = @"select tc.*
                                , emp.FirstName +' '+emp.LastName as LastUpdatedName from TimeClock tc 
                                left join Employee emp on emp.UserId = tc.LastUpdateBy
                                where tc.UserId = '{0}'
                                and Time between '{1}' and '{2}'
                                {4}";
            string subquery = "";
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/timeclockdate")
                {
                    subquery = "order by [Time] asc";
                }
                else if (order == "descending/timeclockdate")
                {
                    subquery = "order by [Time] desc";
                }
                else if (order == "ascending/time")
                {
                    subquery = "order by [Time] asc";
                }
                else if (order == "descending/time")
                {
                    subquery = "order by [Time] desc";
                }
                else if (order == "ascending/clockinout")
                {
                    subquery = "order by [Type] asc";
                }
                else if (order == "descending/clockinout")
                {
                    subquery = "order by [Type] desc";
                }
                else if (order == "ascending/timespent")
                {
                    subquery = "order by ClockedInMinutes asc";
                }
                else if (order == "descending/timespent")
                {
                    subquery = "order by ClockedInMinutes desc";
                }
                else if (order == "ascending/note")
                {
                    subquery = "order by Note asc";
                }
                else if (order == "descending/note")
                {
                    subquery = "order by Note desc";
                }
            }
            else
            {
                subquery = "order by Id desc";
            }
            sqlQuery = string.Format(sqlQuery
                , userId
                , StartDate.ToString("yyyy-MM-dd HH:mm:ss")
                , EndDate.ToString("yyyy-MM-dd HH:mm:ss"), order, subquery);

            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    SqlDataReader reader;
                    long result = SelectRecords(cmd, out reader);

                    TimeClockList list = new TimeClockList();

                    using (reader)
                    {
                        // Read rows until end of result or number of rows specified is reached
                        while (reader.Read())
                        {
                            TimeClock timeClockObject = new TimeClock();
                            FillObject(timeClockObject, reader);
                            timeClockObject.LastUpdatedName = reader["LastUpdatedName"].ToString();
                            list.Add(timeClockObject);
                        }
                        reader.Close();
                    }

                    return list;
                }

            }
            catch (Exception ee)
            {
                return null;
            }

        }
        public DataSet GetLastClocksByUserIdAndTimePeriod(Guid userId, DateTime StartDate, DateTime EndDate, string order, int pageno, int pagesize)
        {
            string sqlQuery = @"declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize
								
								select tc.*, emp.FirstName+' ' + emp.LastName as LastUpdatedName into #TimeClock 
                                
                                from TimeClock tc
                                left join Employee emp on emp.UserId = tc.LastUpdateBy
                                where tc.UserId ='{0}'
                                and tc.[Time] between '{1}' and '{2}'

								select * into #TimeClockfilter
								from #TimeClock

								select top(@pagesize)
								* from #TimeClockfilter
								where Id not in(select top(@pagestart) Id from #TimeClock #tc {4})
                                {7}
								select count(*) CountTotal
                                from #TimeClockfilter
                                
								drop table #TimeClock
								drop table #TimeClockfilter";
            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/timeclockdate")
                {
                    subquery = "order by #tc.[Time] asc";
                    subquery1 = "order by [Time] asc";
                }
                else if (order == "descending/timeclockdate")
                {
                    subquery = "order by #tc.[Time] desc";
                    subquery1 = "order by [Time] desc";
                }
                else if (order == "ascending/time")
                {
                    subquery = "order by #tc.[Time] asc";
                    subquery1 = "order by [Time] asc";
                }
                else if (order == "descending/time")
                {
                    subquery = "order by #tc.[Time] desc";
                    subquery1 = "order by [Time] desc";
                }
                else if (order == "ascending/clockinout")
                {
                    subquery = "order by #tc.[Type] asc";
                    subquery1 = "order by [Type] asc";
                }
                else if (order == "descending/clockinout")
                {
                    subquery = "order by #tc.[Type] desc";
                    subquery1 = "order by [Type] desc";
                }
                else if (order == "ascending/timespent")
                {
                    subquery = "order by #tc.ClockedInMinutes asc";
                    subquery1 = "order by ClockedInMinutes asc";
                }
                else if (order == "descending/timespent")
                {
                    subquery = "order by #tc.ClockedInMinutes desc";
                    subquery1 = "order by ClockedInMinutes desc";
                }
                else if (order == "ascending/note")
                {
                    subquery = "order by #tc.Note asc";
                    subquery1 = "order by Note asc";
                }
                else if (order == "descending/note")
                {
                    subquery = "order by #tc.Note desc";
                    subquery1 = "order by Note desc";
                }
            }
            else
            {
                subquery = "order by #tc.Id desc";
                subquery1 = "order by Id desc";
            }
            sqlQuery = string.Format(sqlQuery
                , userId
                , StartDate.ToString("yyyy-MM-dd HH:mm:ss")
                , EndDate.ToString("yyyy-MM-dd HH:mm:ss"), order, subquery, pageno, pagesize, subquery1);

            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }

            }
            catch (Exception ee)
            {
                return null;
            }

        }
        public DataTable GetAutoClockOutList()
        {
            string sqlQuery = @"select emp.FirstName +' ' +emp.LastName as EmpName
                                ,emp.UserId
                                , (select top(1) [Type] from TimeClock where UserId  = emp.UserId order by [Time] desc) as ClockedInOutStatus 
                                , (select top(1) [Time] from TimeClock where UserId  = emp.UserId order by [Time] desc) as ClockedInOutTime
                                into #TempData
                                from Employee emp
                                where emp.NoAutoClockOut is null or emp.NoAutoClockOut = 0

                                select * from #TempData 
                                where ClockedInOutStatus is not null 
                                and ClockedInOutTime is not null 
                                and ClockedInOutStatus = 'Clock In'

                                drop table #TempData";
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet ds = GetDataSet(cmd);
                    return ds.Tables[0];
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public DataSet GetAllUsersInOutreportByFilter(PayrollFilterModel filter)
        {
            #region Previous Query
            //select TC.*, emp.FirstName + ' ' + emp.LastName as EmployeeName
            //                    from TimeClock TC
            //                    left
            //                    join Employee emp on emp.UserId = tc.UserId
            //                    where Time between FORMAT(GETDATE() - 7, 'yyyy-MM-dd 00:00:00') and FORMAT(getdate() + 2, 'yyyy-MM-dd 00:00:00')
            //                    order by[Time] desc,
            //                    TC.UserId desc
            #endregion
            string sqlQuery = @"
                                DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int

                                SET @pageno ={0} --default 1
                                SET @pagesize = {1} --default 10
                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

                                select * into #PayrollData from (
                                select TC.*, emp.FirstName + ' ' + emp.LastName as EmployeeName
	                                from TimeClock TC
                                    left join Employee emp on emp.UserId = tc.UserId
                                    {2}
    
                                )a
                                SELECT TOP (@pagesize) * FROM #PayrollData
	                                where   Id NOT IN(Select TOP (@pagestart) Id from #PayrollData order by [Time] desc)
	                                order by [Time] Desc

                                select  count(Id) as [TotalCount] from #PayrollData 
                                drop table #PayrollData 
                               ";
            string FilterQuery = "";
            if (filter.StartDate != null && filter.StartDate != new DateTime()
                && filter.EndDate != null && filter.EndDate != new DateTime())
            {
                filter.StartDate = filter.StartDate.SetZeroHour().ClientToUTCTime();
                filter.EndDate = filter.EndDate.SetMaxHour().ClientToUTCTime();

                FilterQuery = string.Format("where [Time] between '{0}' and '{1}'"
                    , filter.StartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , filter.EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else if (filter.StartDate != null && filter.StartDate != new DateTime())
            {
                filter.StartDate = filter.StartDate.SetZeroHour().ClientToUTCTime();
                FilterQuery = string.Format("where [Time] > '{0}'"
                   , filter.StartDate.ToString("yyyy-MM-dd HH:mm:ss"));

            }
            else if (filter.EndDate != null && filter.EndDate != new DateTime())
            {
                filter.EndDate = filter.EndDate.SetMaxHour().ClientToUTCTime();
                FilterQuery = string.Format("where [Time] < '{0}'"
                   , filter.StartDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, filter.PageNo.Value, filter.PageSize.Value, FilterQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    return GetDataSet(cmd);
                }

            }
            catch (Exception)
            {
                return null;
            }

        }
        public DataSet GetAllPayrollReport(Guid SupervisorId, DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, Guid UserId, string CurrentEmployee)
        {
            #region Previous Query
            //select TC.*, emp.FirstName + ' ' + emp.LastName as EmployeeName
            //                    from TimeClock TC
            //                    left
            //                    join Employee emp on emp.UserId = tc.UserId
            //                    where Time between FORMAT(GETDATE() - 7, 'yyyy-MM-dd 00:00:00') and FORMAT(getdate() + 2, 'yyyy-MM-dd 00:00:00')
            //                    order by[Time] desc,
            //                    TC.UserId desc
            #endregion

            string DateFilter2 = "";
            string DateFilter1 = "";
            string RoleFilter = "";
            string CurrentEmployeeFilter = "";
            if (!string.IsNullOrEmpty(CurrentEmployee))
            {
                if (CurrentEmployee == "1")
                {
                    CurrentEmployeeFilter = " And emp.IsCurrentEmployee=1";
                }
                else if (CurrentEmployee == "0")
                {
                    CurrentEmployeeFilter = " And emp.IsCurrentEmployee!=1";
                }
            }
            if (UserId != new Guid())
            {
                RoleFilter = string.Format(" and emp.UserId= '{0}'", UserId);
            }
            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime())
            {
                DateFilter2 = string.Format("and (ClockInTime between '{0}' and '{1}' or ClockOutTime between '{0}' and '{1}')"
                    , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss:fff")
                    , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                DateFilter1 = string.Format("and (StartDate between '{0}' and '{1}' or EndDate between '{0}' and '{1}')"
                     , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss:fff")
                    , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            }
            string sqlQuery = @"
                                declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                select emp.Id, emp.FirstName + ' '+emp.LastName as EmpName, emp.UserId,
                                
                                (select  round((sum(Cast(ClockedInSeconds as float))/3600),2 )  from EmployeeTimeClock where UserId = emp.UserId {0})  as [RegularHours],
                                (select ( round((sum(Cast(ClockedInSeconds as float))/3600),2 ) - 40 )  from EmployeeTimeClock where UserId = emp.UserId {0}) as [OTOHours],
                                (select round((sum(Cast(Minute as float))/60),2 )  from Pto where UserId = emp.UserId and Payable = 1  {4}) as [PTOHours]
                                into #emp
                                from Employee emp
                                left join EmployeeTimeClock tm on tm.UserId = emp.UserId
                                LEFT JOIN PTO pt on pt.UserId=emp.UserId and pt.Status='accepted' 
                                where emp.IsPayroll =1 and emp.IsActive=1   and emp.IsCurrentEmployee = 1 and emp.IsDeleted = 0 {6} {7}
                                group by emp.FirstName,emp.LastName,emp.UserId, emp.Id

                                select * into #empfilter
								from #emp

								select 
								* from #empfilter
								where Id not in(select top(@pagestart) Id from #emp #e {2})
                                {3}
								select count(*) CountTotal
                                from #empfilter
                                
								drop table #emp
								drop table #empfilter";

            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/name")
                {
                    subquery = "order by #e.EmpName asc";
                    subquery1 = "order by EmpName asc";
                }
                else if (order == "descending/name")
                {
                    subquery = "order by #e.EmpName desc";
                    subquery1 = "order by EmpName desc";
                }
                else if (order == "ascending/regular")
                {
                    subquery = "order by #e.[RegularHours] asc";
                    subquery1 = "order by [RegularHours] asc";
                }
                else if (order == "descending/regular")
                {
                    subquery = "order by #e.[RegularHours] desc";
                    subquery1 = "order by [RegularHours] desc";
                }
                else if (order == "ascending/ot")
                {
                    subquery = "order by #e.[OTOHours] asc";
                    subquery1 = "order by [OTOHours] asc";
                }
                else if (order == "descending/ot")
                {
                    subquery = "order by #e.[OTOHours] desc";
                    subquery1 = "order by [OTOHours] desc";
                }
                else if (order == "ascending/pto")
                {
                    subquery = "order by #e.[PTOHours] asc";
                    subquery1 = "order by [PTOHours] asc";
                }
                else if (order == "descending/pto")
                {
                    subquery = "order by #e.[PTOHours] desc";
                    subquery1 = "order by [PTOHours] desc";
                }
            }
            else
            {
                subquery = "order by #e.Id desc";
                subquery1 = "order by Id desc";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, DateFilter2, order, subquery, subquery1, DateFilter1, SupervisorId, RoleFilter, CurrentEmployeeFilter);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }

            }
            catch (Exception ee)
            {
                return null;
            }

        }

        public DataSet GetAllPayrollReportForReports(Guid SupervisorId, DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, Guid userid, string CurrentEmployee)
        {
            #region Previous Query
            //select TC.*, emp.FirstName + ' ' + emp.LastName as EmployeeName
            //                    from TimeClock TC
            //                    left
            //                    join Employee emp on emp.UserId = tc.UserId
            //                    where Time between FORMAT(GETDATE() - 7, 'yyyy-MM-dd 00:00:00') and FORMAT(getdate() + 2, 'yyyy-MM-dd 00:00:00')
            //                    order by[Time] desc,
            //                    TC.UserId desc
            #endregion
            string rolequery = "";
            string CurrentEmployeeFilter = "";
            if (!string.IsNullOrEmpty(CurrentEmployee))
            {
                if (CurrentEmployee == "1")
                {
                    CurrentEmployeeFilter = " And emp.IsCurrentEmployee=1";
                }
                else if (CurrentEmployee == "0")
                {
                    CurrentEmployeeFilter = " And emp.IsCurrentEmployee!=1";
                }
            }
            if (userid != new Guid())
            {
                rolequery = string.Format("and emp.UserId = '{0}'", userid);
            }
            string DateFilter2 = "";
            string DateFilter1 = "";
            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime())
            {

                DateFilter2 = string.Format("and (ClockInTime between '{0}' and '{1}' or ClockOutTime between '{0}' and '{1}')"
                    , FilterStartDate.SetZeroHour()
                    , FilterEndDate.SetMaxHour());
                DateFilter1 = string.Format("and (StartDate between '{0}' and '{1}' or EndDate between '{0}' and '{1}')"
                    , FilterStartDate.SetZeroHour()
                    , FilterEndDate.SetMaxHour());
            }
            string sqlQuery = @"
                                declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                select emp.Id, ul.Id as UserId, emp.FirstName + ' '+emp.LastName as EmpName,
                                
                                (select  iif(round((sum(Cast(ClockedInSeconds as float))/3600),2) > 40, 40, round((sum(Cast(ClockedInSeconds as float))/3600),2))  from EmployeeTimeClock where UserId = emp.UserId {0})  as [RegularHours],
                                (select ( round((sum(Cast(ClockedInSeconds as float))/3600),2 ) - 40 )  from EmployeeTimeClock where UserId = emp.UserId {0}) as [OTOHours],
                                (select round((sum(Cast(Minute as float))/60),2 )  from Pto where UserId = emp.UserId  and Payable =1  {4}) as [PTOHours]
                                ,emp.HourlyRate
                                into #emp
                                from Employee emp
                                left join EmployeeTimeClock tm on tm.UserId = emp.UserId
                                LEFT JOIN PTO pt on pt.UserId=emp.UserId and pt.Status='accepted' 
                                Left Join UserLogin ul on ul.UserId = emp.UserId
                                where emp.IsPayroll =1 and emp.IsActive=1  and emp.IsCurrentEmployee = 1 and emp.IsDeleted = 0 {6} {7}
                                group by emp.FirstName,emp.LastName, emp.Id, emp.UserId, emp.HourlyRate, ul.Id

                                select * into #empfilter
								from #emp

								select top(@pagesize)
								*,(RegularHours*HourlyRate) as totalpay from #empfilter
								where Id not in(select top(@pagestart) Id from #emp #e {2})
                                {3}
								select count(*) CountTotal
                                from #empfilter
                                
								drop table #emp
								drop table #empfilter";

            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/name")
                {
                    subquery = "order by #e.EmpName asc";
                    subquery1 = "order by EmpName asc";
                }
                else if (order == "descending/name")
                {
                    subquery = "order by #e.EmpName desc";
                    subquery1 = "order by EmpName desc";
                }
                else if (order == "ascending/regular")
                {
                    subquery = "order by #e.[RegularHours] asc";
                    subquery1 = "order by [RegularHours] asc";
                }
                else if (order == "descending/regular")
                {
                    subquery = "order by #e.[RegularHours] desc";
                    subquery1 = "order by [RegularHours] desc";
                }
                else if (order == "ascending/ot")
                {
                    subquery = "order by #e.[OTOHours] asc";
                    subquery1 = "order by [OTOHours] asc";
                }
                else if (order == "descending/ot")
                {
                    subquery = "order by #e.[OTOHours] desc";
                    subquery1 = "order by [OTOHours] desc";
                }
                else if (order == "ascending/pto")
                {
                    subquery = "order by #e.[PTOHours] asc";
                    subquery1 = "order by [PTOHours] asc";
                }
                else if (order == "descending/pto")
                {
                    subquery = "order by #e.[PTOHours] desc";
                    subquery1 = "order by [PTOHours] desc";
                }
            }
            else
            {
                subquery = "order by #e.Id desc";
                subquery1 = "order by Id desc";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, DateFilter2, order, subquery, subquery1, DateFilter1, SupervisorId, rolequery,CurrentEmployeeFilter);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }

            }
            catch (Exception ee)
            {
                return null;
            }

        }

        public DataTable GetEmployeeTimeClockReportByUserId(Guid userId, DateTime StartDate, DateTime EndDate, DateTime FilterStartDate, DateTime FilterEndDate)
        {
            string DateFilter = "";
            if (StartDate != new DateTime() && EndDate != new DateTime())
            {
                DateFilter = string.Format("and tc.[Time] between '{0}' and '{1}'"
                    , StartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            string DateFilter2 = "";
            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime())
            {
                DateFilter2 = string.Format("and tc.[Time] between '{0}' and '{1}'"
                    , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            string sqlQuery = @"select  emp.FirstName + ' '+emp.LastName as [EmployeeName]
                                ,tc.[Type]  
                                --,CONVERT(varchar, tc.[Time], 109) as [Time]
                                , tc.[Time]
                                ,tc.Note
                                ,tc.ClockedInMinutes
                                
                                from TimeClock tc
                                left join Employee emp on tc.UserId = emp.UserId
                                where tc.UserId = '{0}'
                                {1}{2}
                                order by [Time] desc";

            sqlQuery = string.Format(sqlQuery, userId, DateFilter, DateFilter2);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable GetAllEmploployeeTimeClockReport(string SupervisorId, DateTime StartDate, DateTime EndDate)
        {
            string DateFilter = "";
            string subquery = "";
            if (!string.IsNullOrWhiteSpace(SupervisorId) && SupervisorId != "null")
            {
                subquery = string.Format("and UserId in ('{0}')", SupervisorId);
            }
            if (StartDate != new DateTime() && EndDate != new DateTime())
            {
                DateFilter = string.Format("(ClockInTime between '{0}' and '{1}' or ClockOutTime between '{0}' and '{1}')"
                    , StartDate.ToString("yyyy-MM-dd")
                    , EndDate.ToString("yyyy-MM-dd"));
            }

            string sqlQuery = @"select timec.*, (select emp.FirstName+' ' + emp.LastName from employee emp where emp.UserId = timec.UserId) as LastUpdatedName 
                                from EmployeeTimeClock timec
                                where {1}
                                {2}
                                --order by [Time] desc
                                ";
            sqlQuery = string.Format(sqlQuery, SupervisorId, DateFilter, subquery);




            //string sqlQuery = @"select emp.FirstName + ' '+emp.LastName as [Employee],
            //                    (select  sum(ClockedInMinutes)/60 from TimeClock where UserId = emp.UserId)  as [Regular Hours],
            //                    (select (sum(ClockedInMinutes)/60 - 40 ) from TimeClock where UserId = emp.UserId) as [OT Hours]
            //                    from Employee emp
            //                    where emp.SuperVisorId = '{0}'";
            //sqlQuery = string.Format(sqlQuery, SupervisorId);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }

            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable GetAllPtoReport(Guid SupervisorId, DateTime StartDate, DateTime EndDate, DateTime FilterStartDate, DateTime FilterEndDate)
        {
            string DateFilter = "";
            if (StartDate != new DateTime() && EndDate != new DateTime())
            {
                DateFilter = string.Format("TC.[Time] between '{0}' and '{1}'"
                    , StartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            string DateFilter2 = "";
            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime())
            {
                DateFilter2 = string.Format("and TC.[Time] between '{0}' and '{1}'"
                    , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            string sqlQuery = @"select TC.Type,emp.UserId, emp.FirstName + ' ' + emp.LastName as EmployeeName,emp.PtoHour,emp.PtoRate
                                from TimeClock TC
                                left
                                join Employee emp on emp.UserId = tc.UserId
                                where tc.UserId in (select UserId from Employee where SuperVisorId = '{0}') and
                                {1}{2}
                                order by TC.[Time] desc";
            sqlQuery = string.Format(sqlQuery, SupervisorId, DateFilter, DateFilter2);




            //string sqlQuery = @"select emp.FirstName + ' '+emp.LastName as [Employee],
            //                    (select  sum(ClockedInMinutes)/60 from TimeClock where UserId = emp.UserId)  as [Regular Hours],
            //                    (select (sum(ClockedInMinutes)/60 - 40 ) from TimeClock where UserId = emp.UserId) as [OT Hours]
            //                    from Employee emp
            //                    where emp.SuperVisorId = '{0}'";
            //sqlQuery = string.Format(sqlQuery, SupervisorId);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }

            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
