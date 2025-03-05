using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Web;

namespace HS.DataAccess
{
	public partial class EmployeeOperationsDataAccess
	{
        public EmployeeOperationsDataAccess(string ConnectionString) : base(ConnectionString)
        {
             
        }
        public EmployeeOperationsDataAccess()
        {
             
        }
        public DataSet GetAllTechReliabilityByFilter(DateTime StartDate, DateTime EndDate, float workinghours, string weekquery, string search, string order, bool IsDownload)
        {
            string sqlQuery = @" select emp.FirstName +' '+ emp.LastName as Technician,
                                 (ISNULL(dbo.Func_PromisedAvailibilityCalculation(emp.UserId, {0}, '{1}', '{2}', @WeeklyText),0) / 60) as [Promised Availibility],
                                 
                                 cast((cast((select ISNULL(sum([Minute]),0) from Pto    
                                    where UserId = emp.UserId and [Status] = 'Accepted'
                                    and (StartDate <= '{2}' and EndDate >= '{1}')) as decimal(12,2)) / 60) as decimal(12,2)) as [Taken Off]
                                 into #tempreliability1
                                 from Employee emp where emp.IsActive = 1 and emp.IsDeleted = 0 and emp.IsCurrentEmployee = 1
                                 --select *, ([Promised Availibility] - [Taken Off]) as [Actual Avilibility], (case when [Promised Availibility] > 0 then ((([Promised Availibility] - [Taken Off]) / [Promised Availibility]) * 100) else 0 end) as Reliability into #tempreliability from #tempreliability1   
                                   select *, ([Promised Availibility] - [Taken Off]) as [Actual Avilibility], cast(case when [Promised Availibility] > 0 then ((([Promised Availibility] - [Taken Off]) / [Promised Availibility]) * 100) else 0 end as decimal(18,2)) as Reliability into #tempreliability from #tempreliability1   
                                   {5}
                                    drop table #tempreliability
                                    drop table #tempreliability1
                                ";
            string sqlorderby = " order by Technician asc";
            if (!string.IsNullOrWhiteSpace(order) && order != "null")
            {
                if (order == "descending/technician")
                {
                    sqlorderby = " order by Technician desc";
                }
                else if (order == "descending/promised")
                {
                    sqlorderby = " order by [Promised Availibility] desc ";
                }
                else if (order == "ascending/promised")
                {
                    sqlorderby = " order by [Promised Availibility] asc";
                }
                else if (order == "descending/takenoff")
                {
                    sqlorderby = " order by [Taken Off] desc";
                }
                else if (order == "ascending/takenoff")
                {
                    sqlorderby = " order by [Taken Off] asc";
                }
                else if (order == "descending/actualavilibility")
                {
                    sqlorderby = " order by [Actual Avilibility] desc";
                }
                else if (order == "ascending/actualavilibility")
                {
                    sqlorderby = " order by [Actual Avilibility] asc";
                }
                else if (order == "descending/reliability")
                {
                    sqlorderby = " order by Reliability desc ";
                }
                else if (order == "ascending/reliability")
                {
                    sqlorderby = " order by Reliability asc";
                }
            }
            string sqlsearch = "";
            search = HttpUtility.UrlDecode(search);
            if (!string.IsNullOrWhiteSpace(search))
            {
                sqlsearch = string.Format(" where Technician like '%{0}%'", search);
            }
            string subquery = string.Format("select * from #tempreliability {0} {1}", sqlsearch, sqlorderby);
            if (IsDownload)
            {
                subquery = string.Format(@"
                               select *, identity(int,1,1) as Id into #tempreliability2 from #tempreliability {0} {1}
                                declare @tempcount int
                                declare @tempability decimal(10,2)
                                declare @tempschedule decimal(10,2)
                                declare @tempnothour  decimal(10,2)
                                declare @temppercent  decimal(10,2)
                                set @tempcount = (select Isnull(count(Technician),0) from #tempreliability2)
                                set @tempschedule = (select Isnull(SUM([Taken Off]),0) from #tempreliability2)
                                set @tempability = (select Isnull(SUM([Promised Availibility]),0) from #tempreliability2)
                                set @tempnothour = (select Isnull(SUM([Actual Avilibility]),0) from #tempreliability2)
                                set @temppercent = (select case when Isnull(SUM([Promised Availibility]),0) > 0 then ((Isnull(SUM([Actual Avilibility]),0) / Isnull(SUM([Promised Availibility]),0)) * 100) else 0.00 end from #tempreliability2)
                                
                                SET IDENTITY_INSERT #tempreliability2 ON
                                insert into #tempreliability2 ([Technician],[Promised Availibility],[Taken Off],[Actual Avilibility],[Reliability],[Id]) 
                                values('Total: '+cast(@tempcount as nvarchar(10)),@tempability,@tempschedule, @tempnothour, @temppercent, @tempcount + 1)
                                select [Technician],[Promised Availibility],[Taken Off],[Actual Avilibility],[Reliability] from #tempreliability2 order by Id asc
                                drop table #tempreliability2
                               ", sqlsearch, sqlorderby);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, workinghours, StartDate, EndDate, sqlsearch, sqlorderby, subquery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("WeeklyText", weekquery));
                    return GetDataSet(cmd);

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetAllTechAvailabilityByFilter(DateTime StartDate, DateTime EndDate, float workinghours, string weekquery, string search, string order, bool IsDownload)
        {
            string sqlQuery = @" 
                                 select emp.FirstName +' '+ emp.LastName as Technician,
                                cast((((ISNULL(dbo.Func_PromisedAvailibilityCalculation(emp.UserId, {0}, '{1}', '{2}', @WeeklyText),0) -
                                (select ISNULL(sum([Minute]),0) from Pto   where UserId = emp.UserId and [Status] = 'Accepted'
                                and (StartDate <= '{2}' and EndDate >= '{1}')))) / 60) as Decimal(10,2))  as [Tech Available],
                                
                                cast((select ISNULL(SUM(
                                (PARSE(Replace(case when ca.AppointmentEndTime != '-1' then ca.AppointmentEndTime else '0' end,':','') as int) - PARSE(Replace(case when ca.AppointmentStartTime != '-1' then ca.AppointmentStartTime else '0' end,':','') as int) )/100 *60 
                                +(PARSE(Replace(case when ca.AppointmentEndTime != '-1' then ca.AppointmentEndTime else '0' end,':','') as decimal(12,2)) - PARSE(Replace(case when ca.AppointmentStartTime != '-1' then ca.AppointmentStartTime else '0' end,':','') as decimal(12,2)) )%100),0 ) / 60
                                
                                from CustomerAppointment ca
                                left join TicketUser tu on tu.TiketId =ca.AppointmentId
                                left join Ticket ticket on ticket.TicketId = tu.TiketId
                                left Join TicketStatusImageSetting imgset on imgset.TicketStatus = ticket.[Status]
                                                            	
                                where tu.NotificationOnly = 0
                                and ticket.[Status] NOT LIKE '%cancel%' and ticket.[Status] NOT LIKE '%lost%' and ticket.[Status] NOT LIKE '%no%'
                                and ticket.TicketId is not null
                                and tu.UserId = emp.UserId
                                and ca.AppointmentDate between '{1}' and '{2}' 
                                ) as Decimal(10,2)) as [Scheduled]
                                into #tempability
                                from Employee emp where emp.IsActive = 1 and emp.IsDeleted = 0 and emp.IsCurrentEmployee = 1
                                
                                select *, cast((case when [Tech Available] > 0 then ([Tech Available] - [Scheduled]) else 0 end) as Decimal(10,2)) as [Hours Not Used], cast((case when [Tech Available] > 0 then (([Scheduled] / [Tech Available]) * 100) else 0 end ) as Decimal(10,2)) as [Usability] into #tempability2 from #tempability
                                
                                {5}
                                
                                drop table #tempability
                                drop table #tempability2

                                ";
            string sqlorderby = " order by Technician asc";
            if (!string.IsNullOrWhiteSpace(order) && order != "null")
            {
                if (order == "descending/technician")
                {
                    sqlorderby = " order by Technician desc";
                }
                else if (order == "descending/available")
                {
                    sqlorderby = " order by [Tech Available] desc ";
                }
                else if (order == "ascending/available")
                {
                    sqlorderby = " order by [Tech Available] asc";
                }
                else if (order == "descending/scheduled")
                {
                    sqlorderby = " order by [Scheduled] desc";
                }
                else if (order == "ascending/scheduled")
                {
                    sqlorderby = " order by [Scheduled] asc";
                }
                else if (order == "descending/hours")
                {
                    sqlorderby = " order by [Hours Not Used] desc";
                }
                else if (order == "ascending/hours")
                {
                    sqlorderby = " order by [Hours Not Used] asc";
                }
                else if (order == "descending/usability")
                {
                    sqlorderby = " order by [Usability] desc ";
                }
                else if (order == "ascending/usability")
                {
                    sqlorderby = " order by [Usability] asc";
                }
            }

            string sqlsearch = "";
            search = HttpUtility.UrlDecode(search);
            if (!string.IsNullOrWhiteSpace(search))
            {
                sqlsearch = string.Format(" where Technician like '%{0}%'", search);
            }
            string subquery = string.Format("select * from #tempability2 {0} {1}", sqlsearch, sqlorderby);
            if (IsDownload)
            {
                subquery = string.Format(@"
                               select *, identity(int,1,1) as Id into #tempability3 from #tempability2  {0} {1}
                               
                               declare @tempcount int
                               declare @tempability decimal(10,2)
                               declare @tempschedule decimal(10,2)
                               declare @tempnothour  decimal(10,2)
                               declare @temppercent  decimal(10,2)
                               set @tempcount = (select Isnull(count(Technician),0) from #tempability3)
                               set @tempschedule = (select Isnull(SUM([Scheduled]),0) from #tempability3)
                               set @tempability = (select Isnull(SUM([Tech Available]),0) from #tempability3)
                               set @tempnothour = (select Isnull(SUM([Hours Not Used]),0) from #tempability3)
                               set @temppercent = (select case when Isnull(SUM([Tech Available]),0) > 0 then ((Isnull(SUM([Scheduled]),0) / Isnull(SUM([Tech Available]),0)) * 100) else 0.00 end from #tempability3)
                               
                               SET IDENTITY_INSERT #tempability3 ON
                               insert into #tempability3 ([Technician],[Tech Available],[Scheduled],[Hours Not Used],[Usability],[Id]) 
                               values('Total: '+cast(@tempcount as nvarchar(10)),@tempability,@tempschedule, @tempnothour, @temppercent, @tempcount + 1)
                               select [Technician],[Tech Available],[Scheduled],[Hours Not Used],[Usability] from #tempability3 order by Id asc
                                drop table #tempability3
                               ", sqlsearch, sqlorderby);
            }

            try
            {
                sqlQuery = string.Format(sqlQuery, workinghours, StartDate, EndDate, sqlsearch, sqlorderby, subquery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("WeeklyText", weekquery));
                    return GetDataSet(cmd);

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public double GetTotalPromiseHours(DateTime Start, DateTime End, float hours, string weeks, Guid user)
        {
            string sqlQuery = @"select ISNULL(dbo.Func_PromisedAvailibilityCalculation('{3}', {0}, '{1}', '{2}', @WeeklyText),0) as [Promised Availibility]";
            try
            {
                sqlQuery = string.Format(sqlQuery, hours, Start, End, user);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("WeeklyText", weeks));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0].Rows[0]["Promised Availibility"] != DBNull.Value ? (Convert.ToDouble(dsResult.Tables[0].Rows[0]["Promised Availibility"]) / 60) : 0.0;
                }
            }
            catch (Exception ex)
            {
                return 0.00;
            }
        }
        public DataSet GetEmployeeQualificationDataByUser(DateTime Start, DateTime End, float hours, string weeks, Guid user)
        {
            string sqlQuery = @"
                                select (select count(csu2.Id) from CustomSurveyUser csu2
								join Ticket tk on tk.TicketId = csu2.ReferenceId
								join TicketUser tu on  tu.TiketId = tk.ticketid and tu.IsPrimary = 1
								where tu.UserId=emp.UserId and tk.CompletionDate between '{1}' and '{2}') as Surveys

                                ,(select count(*) 
								from CustomSurveyUserAnswers  csua
								join CustomSurveyUser csu on csu.SurveyUserId = csua.SurveyUserId
								join Ticket tk on tk.ticketid = csu.referenceId
								join ticketuser tu on tu.tiketId = tk.ticketid and tu.IsPrimary = 1
								where tu.UserId = emp.UserId and tk.CompletionDate between '{1}' and '{2}'
								and TRY_PARSE(csua.Answer AS int) > 8) as [High]
								
								,(select count(*) 
								from CustomSurveyUserAnswers  csua
								join CustomSurveyUser csu on csu.SurveyUserId = csua.SurveyUserId
								join Ticket tk on tk.ticketid = csu.referenceId
								join ticketuser tu on tu.tiketId = tk.ticketid and tu.IsPrimary = 1
								where tu.UserId = emp.UserId and tk.CompletionDate between '{1}' and '{2}'
								and TRY_PARSE(csua.Answer AS int) < 3) as [Low],

								(ISNULL(dbo.Func_PromisedAvailibilityCalculation(emp.UserId, {0}, '{1}', '{2}', @WeeklyText),0) / 60) as [Promised Availibility],
                                 
                                 cast((cast((select ISNULL(sum([Minute]),0) from Pto    
                                    where UserId = emp.UserId and [Status] = 'Accepted'
                                    and (StartDate <= '{2}' and EndDate >= '{1}')) as decimal(12,2)) / 60) as decimal(12,2)) as [Taken Off],

									(select count(*) from EmployeeOperations where EmployeeId = emp.UserId and SelectedDate between '{1}' and '{2}' and IsDayOff = 0 and OperationStartTime != '' and OperationEndTime != '' and [DayName]='Saturday') as SaterDayCount
								
								into #temptable33 from Employee emp where emp.UserId = '{3}'
                                
								select 
							IIF(CASE
								    WHEN Surveys > 0
									THEN ((CAST([High] AS float)*100) + (CAST([Low] AS float)* -100))/CAST(Surveys AS float)  
								    ELSE CAST(0 AS float)
								END >= 80, 
								CASE
								    WHEN Surveys > 0
									THEN ((CAST([High] AS float)*100) + (CAST([Low] AS float)* -100))/CAST(Surveys AS float)  
								    ELSE CAST(0 AS float)
								END,
								0) as EmpQuality,
								(case when [Promised Availibility] > 0 then ((([Promised Availibility] - [Taken Off]) / [Promised Availibility]) * 100) else 0 end) as EmpReliability,
								
								IIF( ([Promised Availibility] - [Taken Off]) >= 180, 10, 0) as EmpFullSchedule,
								IIF( ([Promised Availibility] - [Taken Off]) >= 180 and SaterDayCount > 0, 5, 0) as EmpSaturdayBonus
								from #temptable33
								drop table #temptable33

                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, hours, Start, End, user);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("WeeklyText", weeks));
                    return GetDataSet(cmd);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }	
}