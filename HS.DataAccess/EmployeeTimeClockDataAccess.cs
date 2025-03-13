using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using NLog.Filters;

namespace HS.DataAccess
{
	public partial class EmployeeTimeClockDataAccess
	{
        public DataSet GetLastClocksByUserIdAndTimePeriod(string userId, DateTime StartDate, DateTime EndDate, string order, int pageno, int pagesize)
        {
            string usersubquery = "";
            string empquery = "";
            string DateFilter1 = "";
            if (!string.IsNullOrWhiteSpace(userId) && userId != "null")
            {
                usersubquery = string.Format("and tc.UserId in ('{0}')", userId);
                empquery = string.Format("left join Employee emp on emp.UserId in ('{0}')", userId);
            }
            if (StartDate != new DateTime() && EndDate != new DateTime())
            {

                DateFilter1 = string.Format("and (tc.ClockInTime between '{0}' and '{1}' or tc.ClockOutTime between '{0}' and '{1}')"
                    , StartDate.ToString("yyyy-MM-dd HH:mm:ss:fff")
                    , EndDate.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            }
            string sqlQuery = @"declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize
								
								select tc.*, (select emp.FirstName+' ' + emp.LastName from employee emp where emp.UserId = tc.UserId) as LastUpdatedName into #TimeClock 
                                
                                from EmployeeTimeClock tc
                                
                                where tc.Id> 0 
                                --(tc.ClockInTime between '{1}' and '{2}' or tc.ClockOutTime between '{1}' and '{2}')
                                {10}
                                {8}
								select * into #TimeClockfilter
								from #TimeClock

								select top(@pagesize)
								* from #TimeClockfilter
								where Id not in(select top(@pagestart) Id from #TimeClock #tc {4})
                                {7}
								select count(*) CountTotal
                                from #TimeClockfilter
                                select sum(ClockedInSeconds) as AllTotalClockedInSeconds
								from #TimeClockfilter
								drop table #TimeClock
								drop table #TimeClockfilter

                                (select  round((sum(Cast(tc.ClockedInSeconds as float))/3600),2 ) as RegularHour   from EmployeeTimeClock tc where UserId in ('{0}') {10}) 
                                
                                (select round((sum(Cast(Minute as float))/60),2 ) as PtoHour from Pto where UserId in ('{0}') and Payable = 1  and (StartDate between '{1}' and '{2}' or EndDate between '{1}' and '{2}')) 


                             ";
            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/timeclockdate")
                {
                    subquery = "order by #tc.[ClockInTime] asc";
                    subquery1 = "order by [ClockInTime] asc";
                }
                else if (order == "descending/timeclockdate")
                {
                    subquery = "order by #tc.[ClockInTime] desc";
                    subquery1 = "order by [ClockInTime] desc";
                }
                else if (order == "ascending/clockintime")
                {
                    subquery = "order by #tc.[ClockInTime] asc";
                    subquery1 = "order by [ClockInTime] asc";
                }
                else if (order == "descending/clockintime")
                {
                    subquery = "order by #tc.[ClockInTime] desc";
                    subquery1 = "order by [ClockInTime] desc";
                }
                else if (order == "ascending/clockouttime")
                {
                    subquery = "order by #tc.[ClockOutTime] asc";
                    subquery1 = "order by [ClockOutTime] asc";
                }
                else if (order == "descending/clockouttime")
                {
                    subquery = "order by #tc.[ClockOutTime] desc";
                    subquery1 = "order by [ClockOutTime] desc";
                }
                else if (order == "ascending/timespent")
                {
                    subquery = "order by #tc.ClockedInSeconds asc";
                    subquery1 = "order by ClockedInSeconds asc";
                }
                else if (order == "descending/timespent")
                {
                    subquery = "order by #tc.ClockedInSeconds desc";
                    subquery1 = "order by ClockedInSeconds desc";
                }
                else if (order == "ascending/clockinnote")
                {
                    subquery = "order by #tc.ClockInNote asc";
                    subquery1 = "order by ClockInNote asc";
                }
                else if (order == "descending/clockinnote")
                {
                    subquery = "order by #tc.ClockInNote desc";
                    subquery1 = "order by ClockInNote desc";
                }
                else if (order == "ascending/clockoutnote")
                {
                    subquery = "order by #tc.ClockOutNote asc";
                    subquery1 = "order by ClockOutNote asc";
                }
                else if (order == "descending/clockoutnote")
                {
                    subquery = "order by #tc.ClockOutNote desc";
                    subquery1 = "order by ClockOutNote desc";
                }
                else
                {
                    subquery = "order by #tc.ClockInTime asc";
                    subquery1 = "order by ClockInTime asc";
                }
            }
            else
            {
                subquery = "order by #tc.ClockInTime asc";
                subquery1 = "order by ClockInTime asc";
            }
        
            sqlQuery = string.Format(sqlQuery
                , userId
                , StartDate.ToString("MM/dd/yyyy")
                , EndDate.ToString("MM/dd/yyyy"), order, subquery, pageno, pagesize, subquery1, usersubquery, empquery, DateFilter1);

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
        public DataSet EmployeeTimeClockListAccrualPtoByUserId(string userId, DateTime StartDate, DateTime EndDate)
        {
              
            string PtoDatefilterNew = "";
            string AccrualDatefilter = ""; 
            if (StartDate != new DateTime() && EndDate != new DateTime())
            { 
                AccrualDatefilter = string.Format(" and pthour.FromDate >= @FromDate and pthour.EndDate <= @EndDate ");
                PtoDatefilterNew = string.Format(" set @FromDate ='{0}' set @EndDate ='{1}' ", StartDate.ToString("yyyy-MM-dd"), EndDate.ToString("yyyy-MM-dd")); 
            } 
            else
            {
                PtoDatefilterNew = string.Format(" set @FromDate = null   set @EndDate = null"); 
            }
            //if (!string.IsNullOrEmpty(userId) && userId != "00000000-0000-0000-0000-000000000000")
            //{
            //    subquery = string.Format(" and emp.UserId = '{0}'", userId);
            //}
            string sqlQuery = @"DECLARE @userid UNIQUEIDENTIFIER;
                                DECLARE @FromDate DATETIME;
                                DECLARE @EndDate DATETIME;

                                SET @userid = '{0}';
                                {1} 

                                DECLARE @Balance FLOAT = 0;
                                DECLARE @PreviousBalance FLOAT = 0;
                                DECLARE @PTOHour FLOAT, @Used FLOAT;
                                DECLARE @RowId INT = 1;
                                DECLARE @TotalRows INT;

 
                                DECLARE @Start DATETIME;
                                DECLARE @End DATETIME;

                                SELECT @Start = MIN(FromDate), @End = MAX(EndDate)
                                FROM EmployeePTOHourLog pthour
                                WHERE UserId = @userid {2} 

 
                                IF @FromDate IS NULL 
                                    SET @FromDate = @Start;
                                ELSE 
                                    SET @FromDate = @FromDate; 

                                IF @EndDate IS NULL 
                                    SET @EndDate = @End;
                                ELSE 
                                    SET @EndDate = @EndDate; 

 
                                DECLARE @Results TABLE (StartPto DATETIME, EndPto DATETIME, TotalHours DECIMAL(18, 2));

                                DECLARE @StartPto DATETIME = @FromDate;
                                DECLARE @EndPto DATETIME;
  
                                    Select emp.FirstName + ' ' + emp.LastName As EmployeeName                                ,emp.Id,emp.HireDate,emp.PayType,pthour.UserId                                ,Sum(pthour.PTOHour) Totalearned                                ,IsNull(dbo.PTOUsedHours(@userid,@Start,@End),0) As TotalPtoUsedHour                                ,(Sum(pthour.PTOHour) - IsNull(dbo.PTOUsedHours(@userid,@Start,@End),0)) As TotalAvailable                                                                from EmployeePTOHourLog pthour                                left join Employee emp on pthour.UserId = emp.UserId                                where pthour.UserId = @userid {2}                                   group by pthour.UserId,emp.FirstName,emp.LastName,emp.Id,emp.HireDate,emp.PayType


                                WHILE @StartPto <= @EndDate
                                BEGIN
     
                                    SET @EndPto = DATEADD(DAY, 6, @StartPto);
                                    IF @EndPto > @EndDate
                                        SET @EndPto = @EndDate;
                                    INSERT INTO @Results (StartPto, EndPto, TotalHours)
                                    SELECT 
                                        @StartPto, 
                                        @EndPto, 
                                        SUM(ISNULL(CAST(Pto.Minute AS DECIMAL(18, 2)) / 60.0, 0))
                                    FROM Pto
                                    WHERE Status = 'Accepted' 
                                        AND UserId = @userid
                                        AND LastUpdatedDate >= @StartPto 
                                        AND LastUpdatedDate <= @EndPto;  
                                    SET @StartPto = DATEADD(DAY, 7, @StartPto);
                                END;

                                SELECT 
                                    ROW_NUMBER() OVER (ORDER BY pthour.CreatedDate) AS RowId,  
                                    pthour.Id,
                                    pthour.UserId,
                                    pthour.PTOHour AS PTOHour,  
                                    pthour.FromDate,
                                    pthour.EndDate,
                                    pthour.CreatedDate,
                                    pthour.LastUpdatedDate,
                                    ISNULL(CAST(pthour.WorkingHours AS DECIMAL(18,2)),0) AS WorkingHours, 
                                    ISNULL(r.TotalHours, 0) AS Used,   
                                    CAST(0 AS FLOAT) AS TotalBalance  
                                INTO #tempbalance
                                FROM EmployeePTOHourLog pthour
                                LEFT JOIN @Results r
                                    ON pthour.FromDate <= r.EndPto AND pthour.EndDate >= r.StartPto 
                                WHERE pthour.UserId = @userid  {2}  

                                SELECT @TotalRows = COUNT(*) FROM #tempbalance;

 
                                WHILE @RowId <= @TotalRows 
                                BEGIN  
                                    SELECT   
                                        @PTOHour = PTOHour,   
                                        @Used = Used  
                                    FROM #tempbalance  
                                    WHERE RowId = @RowId;

    
                                    SET @PreviousBalance = @Balance;
                                    SET @Balance = CAST(@PreviousBalance + @PTOHour - @Used AS FLOAT);

  
                                    UPDATE #tempbalance  
                                    SET TotalBalance = @Balance  
                                    WHERE RowId = @RowId;
 
                                    SET @RowId = @RowId + 1;
                                END;
 
                                SELECT 
                                    Id, 
                                    UserId, 
                                    PTOHour, 
                                    FromDate, 
                                    EndDate, 
                                    CreatedDate, 
                                    LastUpdatedDate, 
                                    WorkingHours, 
                                    Used, 
                                    TotalBalance 
                                FROM #tempbalance;

 
                                SELECT 
                                    CreatedDate,
                                    LastUpdatedDate,
                                    Status,
                                    ISNULL(Minute / 60, 0) AS TotalHour 
                                FROM Pto 
                                WHERE UserId = @userid  
                                    AND LastUpdatedDate BETWEEN @FromDate AND @EndDate 
                                    AND Status IN ('Accepted', 'Rejected');

                                DROP TABLE #tempbalance "; 
           
            sqlQuery = string.Format(sqlQuery,userId, PtoDatefilterNew,AccrualDatefilter);

            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                { 
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ee)
            {
                return null;
            }

        }

        public DataTable GetLastClocksByUserIdAndTimePeriodForApi(Guid userId, int pageno, int pagesize, DateTime StartDate, DateTime EndDate)
        {
            string DateQuery = "";

            if (StartDate != new DateTime() && EndDate != new DateTime() )
            {
                string StartDateQuery = StartDate.ToString("yyyy-MM-dd HH:mm:ss:fff");
                string EndDateQuery = EndDate.ToString("yyyy-MM-dd HH:mm:ss:fff");
                
                DateQuery = string.Format("and tc.ClockInTime between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }

            string sqlQuery = @"declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize
								
								select tc.*, (select emp.FirstName+' ' + emp.LastName from employee emp where emp.UserId = tc.UserId) as LastUpdatedName into #TimeClock 
                                
                                from EmployeeTimeClock tc
                                
                                where tc.UserId in ('{0}') {1} order by tc.ClockInTime
								select * into #TimeClockfilter
								from #TimeClock

								select top(@pagesize)
								* from #TimeClockfilter
								where Id not in(select top(@pagestart) Id from #TimeClock #tc order by Id desc)
								order by Id desc

								drop table #TimeClock
								drop table #TimeClockfilter";
           
            sqlQuery = string.Format(sqlQuery
                , userId
                , DateQuery
                , pageno
                ,pagesize);

            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }

            }
            catch (Exception ee)
            {
                return null;
            }

        }
        public DataTable GetLastClocksByUserIdForApi(Guid userId)
        {
            string sqlQuery = @"
        select 
            tc.*, 
            (select emp.FirstName + ' ' + emp.LastName 
             from employee emp 
             where emp.UserId = tc.UserId) as LastUpdatedName
        from EmployeeTimeClock tc
        where tc.UserId = @userId
        order by tc.ClockInTime";

            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pGuid("userId", userId));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


        public DataTable GetTimeClockHistoryCount(Guid userId, DateTime StartDate, DateTime EndDate)
        {
            string DateQuery = "";
            if (StartDate != new DateTime() && EndDate != new DateTime())
            {
                string StartDateQuery = StartDate.ToString("yyyy-MM-dd HH:mm:ss:fff");
                string EndDateQuery = EndDate.ToString("yyyy-MM-dd HH:mm:ss:fff");

                DateQuery = string.Format("and tc.ClockInTime between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }

            string sqlQuery = @"select count(tc.Id) TotalCount
                                from EmployeeTimeClock tc
                                where tc.UserId in ('{0}') {1}";

            sqlQuery = string.Format(sqlQuery
                , userId, DateQuery);

            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }

            }
            catch (Exception ee)
            {
                return null;
            }
        }

        public DataTable GetAutoClockOutList()
        {
            string sqlQuery = @"select * from EmployeeTimeClock where ClockOutTime is null";
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
        #region Comment
        //public DataTable GetEmployeeTimeClockListByUserId(Guid userId)
        //{
        //    string sqlQuery = @"select * from EmployeeTimeClock 
        //                        where UserId = '{0}'";

        //    sqlQuery = string.Format(sqlQuery
        //        , userId);

        //    try
        //    {
        //        using (SqlCommand cmd = GetSQLCommand(sqlQuery))
        //        {

        //            DataSet dsResult = GetDataSet(cmd);
        //            return dsResult.Tables[0];
        //        }

        //    }
        //    catch (Exception ee)
        //    {
        //        return null;
        //    }
        //}
        #endregion
        public DataTable GetEmployeeClockInDetail(Guid userId, DateTime StartDate, DateTime EndDate)
        {
            string DateQuery = "";
            string PtoDateQuery = "";
            if (StartDate != new DateTime() && EndDate != new DateTime())
            {
                string StartDateQuery = StartDate.ToString("yyyy-MM-dd 00:00:00:000");
                string EndDateQuery = EndDate.ToString("yyyy-MM-dd 23:59:59:999");

                DateQuery = string.Format("and (ClockInTime between '{0}' and '{1}' or ClockOutTime between '{0}' and '{1}')", StartDateQuery, EndDateQuery);
                PtoDateQuery = string.Format("and (StartDate between '{0}' and '{1}' or EndDate between '{0}' and '{1}')", StartDateQuery, EndDateQuery);
            }
            string PeriodStart = StartDate.ToString("yyyy-MM-dd");
            string PeriodEnd = EndDate.ToString("yyyy-MM-dd");
            string sqlQuery = @"select distinct 

                                case when (select top(1) ClockOutTime from EmployeeTimeClock where UserId = '{0}'
                                {1} order by id desc) is null then 'True' else 'False' end as ClockInStatus

                                ,(select top(1) ClockInTime from EmployeeTimeClock where UserId = '{0}'
                                {1} order by id desc) as ClockIn

                                ,(select top(1) ClockOutTime from EmployeeTimeClock where UserId = '{0}'
                                {1} order by id desc) as ClockOut

                                ,count(*) as TotalVisitWeekly

                                ,(select top(1) ClockInNote from EmployeeTimeClock where UserId = '{0}'
                                {1} order by id desc) as Job

                                ,'{2}' as Start

                                ,'{3}' as [End]
                                
                                ,(select  round((sum(Cast(ClockedInSeconds as float))/3600),2 )  from EmployeeTimeClock where UserId = '{0}'
								{1}) as [RegularHours]

                                ,(select ( round((sum(Cast(ClockedInSeconds as float))/3600),2 ) - 40 )  from EmployeeTimeClock where UserId = '{0}'
								{1}) as [OTOHours]

                                ,(select round((sum(Cast(Minute as float))/60),2 )  from Pto where UserId = '{0}'
								{4}) as [PTOHours]

                                ,(select  round((sum(Cast(ClockedInSeconds as float))/3600),2 )  from EmployeeTimeClock where UserId = '{0}'
								{1}) 

								+ (select ( round((sum(Cast(ClockedInSeconds as float))/3600),2 ) - 40 )  from EmployeeTimeClock where UserId = '{0}'
								{1})

								+ isnull((select round((sum(Cast(Minute as float))/60),2 )  from Pto where UserId = '{0}'
								{4}),0) as [TotalHours]


                                from EmployeeTimeClock tk
                                LEFT JOIN PTO pt on pt.UserId = tk.UserId and pt.Status='accepted'  
                                where tk.UserId = '{0}'
                                {1}";

            sqlQuery = string.Format(sqlQuery
                , userId, DateQuery, PeriodStart, PeriodEnd, PtoDateQuery);

            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }

            }
            catch (Exception ee)
            {
                return null;
            }
        }
    }
}
