using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;
using System.Linq;

using System.Linq;
using System.Web;
using System.Runtime.InteropServices.ComTypes;
using NLog.Filters;

namespace HS.DataAccess
{
    public partial class EmployeeDataAccess
    {
        
        public EmployeeDataAccess(string ConnectionString) : base(ConnectionString)
        {
            _EmployeeDataAccess = new ErrorLogDataAccess(ConnectionString);
        }
        public EmployeeDataAccess()
        {
            _EmployeeDataAccess = new ErrorLogDataAccess();
        }
        /// <summary>
        /// Retrieves all Employee objects by query String
        /// </summary>
        /// <returns>A list of Employee objects</returns>
        public EmployeeList GetByQuery(String query, bool ResetDb)
        {
            using (SqlCommand cmd = GetSPCommand(GETEMPLOYEEBYQUERY, ResetDb))
            {
                AddParameter(cmd, pNVarChar("Query", 4000, query));
                return GetList(cmd, ALL_AVAILABLE_RECORDS); ;
            }
        }

        public List<Employee> GetAllEmployee(Guid companyid)
        {
            string sqlQuery = @" select emp.* from  Employee emp
                                 left join UserCompany uc
                                 on uc.UserId = emp.UserId
                                 left join UserLogin ul 
	                                on ul.UserId = uc.UserId and ul.CompanyId = uc.CompanyId
                                 left join UserPermission up 
	                                on up.UserId = ul.UserId
                                left join PermissionGroup pg 
	                                on pg.Id = up.PermissionGroupId
                                 where Uc.CompanyId ='{0}'
                                 and emp.IsDeleted = 0
                                and emp.IsActive = 1
                                and pg.Name is not null
                                and emp.IsCurrentEmployee = 1
                                and Recruited = 1
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    //return dsResult.Tables[0];
                    return GetList(cmd, -1);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Employee> GetTransferLocations(Guid companyid)
        {
            string sqlQuery = @" select emp.* from  Employee emp
                                left join UserCompany uc on uc.UserId = emp.UserId
                                where emp.IsDeleted = 0  and emp.IsActive = 1 --and pg.Name is not null 
                                and emp.IsLocation = 1 and emp.UserId <> '22222222-2222-2222-2222-222222222221'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    //return dsResult.Tables[0];
                    return GetList(cmd, -1);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<Employee> GetAllEmployeeByCompanyIdForFinanced(Guid companyid)
        {
            string sqlQuery = @" select emp.* from  Employee emp
                                 left join UserCompany uc
                                 on uc.UserId = emp.UserId
                                 left join UserLogin ul 
	                                on ul.UserId = uc.UserId and ul.CompanyId = uc.CompanyId
  
                                 where Uc.CompanyId ='{0}'
                                 and emp.IsDeleted = 0
                              
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    //return dsResult.Tables[0];
                    return GetList(cmd, -1);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<Employee> GetAllEmployeeByTicketUserIsprimary(Guid companyid)
        {
            string sqlQuery = @" select distinct emp.* from  Employee emp
                                 left join UserCompany uc
                                 
                                 on uc.UserId = emp.UserId
                                 left join ticketuser t on emp.UserId = t.UserId 
                                 where Uc.CompanyId ='{0}'
                                 and emp.IsDeleted = 0
                                and emp.IsActive = 1
                                and Recruited = 1
                               and t.IsPrimary = 1
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    //return dsResult.Tables[0];
                    return GetList(cmd, -1);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<Employee> GetAllEmployeeByTicketAssigned(Guid companyid, Guid userid)
        {
            string sqlQuery = @" select emp.* from  Employee emp
                                 left join UserCompany uc
                                 on uc.UserId = emp.UserId
                                 where Uc.CompanyId ='{0}'
                                 and emp.IsDeleted = 'false'
                                 and emp.UserId != '{1}'
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, userid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    //return dsResult.Tables[0];
                    return GetList(cmd, -1);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetAllEmployeeWithArticle(Guid CompanyId, DateTime? start, DateTime? end, string searchtext, int pageno, int pagesize, string order)
        {
            string sqldate = "";
            string sqldate2 = "";
            string sqlorderby = " order by [Name] asc";
            string sqlsearch = "";

            if (start != new DateTime() && end != new DateTime())
            {
                sqldate = string.Format(" and ka.AssignedDate between '{0}' and '{1}'", start.Value.ToString("yyyy/MM/dd HH:mm"), end.Value.ToString("yyyy/MM/dd HH:mm"));
            }
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                sqlsearch = string.Format(" and emp.FirstName +' '+ emp.LastName Like '%{0}%'", searchtext);
            }
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "descending/phrase")
                {
                    sqlorderby = " order by [Name] desc";
                }
                else if (order == "ascending/count")
                {
                    sqlorderby = " order by Artical asc , [Name] asc";
                }
                else if (order == "descending/count")
                {
                    sqlorderby = " order by Artical desc , [Name] asc";
                }
            }
            string sqlQuery = @"DECLARE @pagestart int
	                            DECLARE @pageend int
	                            DECLARE @pageno int
	                            DECLARE @pagesize int


                                SET @pageno = {0}
							    SET @pagesize = {1}
                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

								select emp.Id, emp.FirstName +' '+ emp.LastName as [Name], emp.UserId,
								(select count(*) from KnowledgebaseAccountability ka
								left join Knowledgebase kn on kn.Id = ka.KnowledgebaseId
								where ka.AssignedUser = emp.UserId and kn.IsDeleted = 0 {5}) as Artical,
                                (select count(*) from KnowledgebaseAccountability ka
								left join Knowledgebase kn on kn.Id = ka.KnowledgebaseId
								where ka.AssignedUser = emp.UserId and kn.IsDeleted = 0 and ka.EndDate is null and ka.IsRead = 0  {5}) as TotalUnread,
                                (select count(*) from KnowledgebaseAccountability ka
								left join Knowledgebase kn on kn.Id = ka.KnowledgebaseId
								where ka.AssignedUser = emp.UserId and kn.IsDeleted = 0 and ka.EndDate is not null and ka.IsRead = 1  {5}) as TotalCompleted
								into #temptable 
								from  Employee emp
								left join UserCompany uc on uc.UserId = emp.UserId
								left join UserLogin ul on ul.UserId = uc.UserId and ul.CompanyId = uc.CompanyId
								left join UserPermission up on up.UserId = ul.UserId
								left join PermissionGroup pg on pg.Id = up.PermissionGroupId
								where Uc.CompanyId ='{2}'
								and emp.IsDeleted = 0
								and emp.IsActive = 1
								and pg.Name is not null
								and emp.IsCurrentEmployee = 1
                                {3}
								select top(@pagesize) * from #temptable
                                where Id not in (Select TOP (@pagestart)  Id from #temptable #ft {4}) and Artical > 0
								{4}
                                select COUNT(*) Total from #temptable where Artical > 0

								drop table #temptable";
            try
            {
                sqlQuery = string.Format(sqlQuery, pageno, pagesize, CompanyId, sqlsearch, sqlorderby, sqldate);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetAllEmployeeWithArticleForDownload(Guid CompanyId, DateTime? start, DateTime? end, string searchtext, int pageno, int pagesize, string order)
        {
            string sqldate = "";
            string sqldate2 = "";
            string sqlorderby = " order by [Assigned Users] asc";
            string sqlsearch = "";

            if (start != new DateTime() && end != new DateTime())
            {
                sqldate = string.Format(" and ka.AssignedDate between '{0}' and '{1}'", start.Value.ToString("yyyy/MM/dd HH:mm"), end.Value.ToString("yyyy/MM/dd HH:mm"));
            }
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                sqlsearch = string.Format(" and emp.FirstName +' '+ emp.LastName Like '%{0}%'", searchtext);
            }
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "descending/phrase")
                {
                    sqlorderby = " order by [Assigned Users] desc";
                }
                else if (order == "ascending/count")
                {
                    sqlorderby = " order by Artical asc , [Assigned Users] asc";
                }
                else if (order == "descending/count")
                {
                    sqlorderby = " order by Artical desc , [Assigned Users] asc";
                }
            }
            string sqlQuery = @"DECLARE @pagestart int
	                            DECLARE @pageend int
	                            DECLARE @pageno int
	                            DECLARE @pagesize int
                                Declare @TotalArticles decimal(18,2) = 0
								Declare @TotalUnRead decimal(18,2) = 0
								Declare @TotalCompleted decimal(18,2) = 0

                                SET @pageno = {0}
							    SET @pagesize = {1}
                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

								select emp.FirstName +' '+ emp.LastName as [Assigned Users],
								(select count(*) from KnowledgebaseAccountability ka
								left join Knowledgebase kn on kn.Id = ka.KnowledgebaseId
								where ka.AssignedUser = emp.UserId and kn.IsDeleted = 0 {5}) as Articles,
                                (select count(*) from KnowledgebaseAccountability ka
								left join Knowledgebase kn on kn.Id = ka.KnowledgebaseId
								where ka.AssignedUser = emp.UserId and kn.IsDeleted = 0 and ka.EndDate is null and ka.IsRead = 0  {5}) as TotalUnread,
								(select count(*) from KnowledgebaseAccountability ka
								left join Knowledgebase kn on kn.Id = ka.KnowledgebaseId
								where ka.AssignedUser = emp.UserId and kn.IsDeleted = 0 and ka.EndDate is not null and ka.IsRead = 1 {5}) as TotalCompleted
								into #temptable from  Employee emp
								left join UserCompany uc on uc.UserId = emp.UserId
								left join UserLogin ul on ul.UserId = uc.UserId and ul.CompanyId = uc.CompanyId
								left join UserPermission up on up.UserId = ul.UserId
								left join PermissionGroup pg on pg.Id = up.PermissionGroupId
								where Uc.CompanyId ='{2}'
								and emp.IsDeleted = 0
								and emp.IsActive = 1
								and pg.Name is not null
								and emp.IsCurrentEmployee = 1 
                                {3}
                                Set @TotalArticles = (Select Sum(Articles) from #temptable)
								Set @TotalUnRead = (Select Sum(TotalUnread) from #temptable)
								Set @TotalCompleted = (Select Sum(TotalCompleted) from #temptable)
								Insert Into #temptable([Assigned Users],Articles,TotalUnread,TotalCompleted)
								Values('Total',@TotalArticles,@TotalUnRead,@TotalCompleted)
                                select * from #temptable where Articles > 0
								{4}
                                drop table #temptable";
            try
            {
                sqlQuery = string.Format(sqlQuery, pageno, pagesize, CompanyId, sqlsearch, sqlorderby, sqldate);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetAccountabilityHistoryList(DateTime start, DateTime end, Guid UserId, string Order)
        {
            string sqlQuery = @"select 
                                kn.Id,
                                kn.Title,
                                ka.AssignedDate,
                                ka.EndDate,
                                emp.FirstName +' '+ emp.LastName as [AssignedBy] 
								into #temptable 
								from KnowledgebaseAccountability ka
                                left join Knowledgebase kn on kn.Id = ka.KnowledgebaseId
                                left join Employee emp on emp.UserId = ka.AssignedBy
								Where ka.AssignedUser ='{0}' and kn.IsDeleted = 0
								{1}
					            select * from #temptable
                                {2}
                                select COUNT(*) Total from #temptable 

								drop table #temptable";

            string sqldate = "";
            string sqlorderby = " order by Title asc";
            string sqlsearch = "";

            if (start != new DateTime() && end != new DateTime())
            {
                sqldate = string.Format(" and ka.AssignedDate between '{0}' and '{1}'", start.ToString("yyyy/MM/dd HH:mm"), end.ToString("yyyy/MM/dd HH:mm"));
            }
            if (!string.IsNullOrWhiteSpace(Order))
            {
                if (Order == "descending/articles")
                {
                    sqlorderby = " order by Title desc";
                }
                else if (Order == "ascending/assignedon")
                {
                    sqlorderby = " order by AssignedDate asc ";
                }
                else if (Order == "descending/assignedon")
                {
                    sqlorderby = " order by AssignedDate desc";
                }
                else if (Order == "ascending/readon")
                {
                    sqlorderby = " order by EndDate asc ";
                }
                else if (Order == "descending/readon")
                {
                    sqlorderby = " order by EndDate desc";
                }
                else if (Order == "ascending/assignedby")
                {
                    sqlorderby = " order by [AssignedBy] asc ";
                }
                else if (Order == "descending/assignedby")
                {
                    sqlorderby = " order by [AssignedBy] desc";
                }
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, UserId, sqldate, sqlorderby);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    return GetDataSet(cmd);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetAccountabilityHistoryUnreadList(DateTime start, DateTime end, Guid UserId, string Order)
        {
            string sqlQuery = @"select 
                                kn.Id,
                                kn.Title,
                                ka.AssignedDate,
                                ka.EndDate,
                                emp.FirstName +' '+ emp.LastName as [AssignedBy] 
								into #temptable 
								from KnowledgebaseAccountability ka
                                left join Knowledgebase kn on kn.Id = ka.KnowledgebaseId
                                left join Employee emp on emp.UserId = ka.AssignedBy
								Where ka.AssignedUser ='{0}' and kn.IsDeleted = 0 and ka.EndDate is null and ka.IsRead = 0
								{1}
					            select * from #temptable
                                {2}
                                select COUNT(*) Total from #temptable 

								drop table #temptable";

            string sqldate = "";
            string sqlorderby = " order by Title asc";
            string sqlsearch = "";

            if (start != new DateTime() && end != new DateTime())
            {
                sqldate = string.Format(" and ka.AssignedDate between '{0}' and '{1}'", start.ToString("yyyy/MM/dd HH:mm"), end.ToString("yyyy/MM/dd HH:mm"));
            }
            if (!string.IsNullOrWhiteSpace(Order))
            {
                if (Order == "descending/articles")
                {
                    sqlorderby = " order by Title desc";
                }
                else if (Order == "ascending/assignedon")
                {
                    sqlorderby = " order by AssignedDate asc ";
                }
                else if (Order == "descending/assignedon")
                {
                    sqlorderby = " order by AssignedDate desc";
                }
                else if (Order == "ascending/readon")
                {
                    sqlorderby = " order by EndDate asc ";
                }
                else if (Order == "descending/readon")
                {
                    sqlorderby = " order by EndDate desc";
                }
                else if (Order == "ascending/assignedby")
                {
                    sqlorderby = " order by [AssignedBy] asc ";
                }
                else if (Order == "descending/assignedby")
                {
                    sqlorderby = " order by [AssignedBy] desc";
                }
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, UserId, sqldate, sqlorderby);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    return GetDataSet(cmd);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetAccountabilityHistoryCompletedList(DateTime start, DateTime end, Guid UserId, string Order)
        {
            string sqlQuery = @"select 
                                kn.Id,
                                kn.Title,
                                ka.AssignedDate,
                                ka.EndDate,
                                emp.FirstName +' '+ emp.LastName as [AssignedBy] 
								into #temptable 
								from KnowledgebaseAccountability ka
                                left join Knowledgebase kn on kn.Id = ka.KnowledgebaseId
                                left join Employee emp on emp.UserId = ka.AssignedBy
								Where ka.AssignedUser ='{0}' and kn.IsDeleted = 0 and ka.EndDate is not null and ka.IsRead = 1
								{1}
					            select * from #temptable
                                {2}
                                select COUNT(*) Total from #temptable 

								drop table #temptable";

            string sqldate = "";
            string sqlorderby = " order by Title asc";
            string sqlsearch = "";

            if (start != new DateTime() && end != new DateTime())
            {
                sqldate = string.Format(" and ka.AssignedDate between '{0}' and '{1}'", start.ToString("yyyy/MM/dd HH:mm"), end.ToString("yyyy/MM/dd HH:mm"));
            }
            if (!string.IsNullOrWhiteSpace(Order))
            {
                if (Order == "descending/articles")
                {
                    sqlorderby = " order by Title desc";
                }
                else if (Order == "ascending/assignedon")
                {
                    sqlorderby = " order by AssignedDate asc ";
                }
                else if (Order == "descending/assignedon")
                {
                    sqlorderby = " order by AssignedDate desc";
                }
                else if (Order == "ascending/readon")
                {
                    sqlorderby = " order by EndDate asc ";
                }
                else if (Order == "descending/readon")
                {
                    sqlorderby = " order by EndDate desc";
                }
                else if (Order == "ascending/assignedby")
                {
                    sqlorderby = " order by [AssignedBy] asc ";
                }
                else if (Order == "descending/assignedby")
                {
                    sqlorderby = " order by [AssignedBy] desc";
                }
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, UserId, sqldate, sqlorderby);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    return GetDataSet(cmd);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetAllEmployeeByBirthDate(Guid companyid, DateTime StartDate, DateTime EndDate)
        {
            string sqlQuery = @" select emp.* , ul.Id as UserLoginId from  Employee emp
                                 left join UserCompany uc
                                 on uc.UserId = emp.UserId
								 left join UserLogin ul on ul.UserId = emp.UserId
                                 where Uc.CompanyId ='{0}' 
                                 and DATEADD( Year, DATEPART( Year, GETDATE()) - DATEPART( Year, emp.DOB), emp.DOB)
								 BETWEEN CONVERT( DATE, '{1}') AND CONVERT( DATE, '{2}')
                                 and emp.IsActive = 'true' and emp.IsDeleted = 'false'
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, StartDate, EndDate);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<Employee> GetAllEmployeeByHirehDate(DateTime FromDate,DateTime EndDate)
        {
            string sqlQuery = @"Select emp.* from Employee emp
                                Where emp.HireDate is not null and emp.HireDate != '' 
                                and emp.PayType is not null and  emp.PayType != '' 
                                and emp.IsActive = 1                              
							    and emp.IsCurrentEmployee = 1 
                                and emp.UserId not in (select ptolog.UserId from EmployeePTOHourLog 
                                ptolog where ptolog.FromDate = '{0}' and ptolog.EndDate = '{1}') 
                                 ";
            try
            {
                sqlQuery = string.Format(sqlQuery,FromDate,EndDate);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    return GetList(cmd, -1);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetAllEmployeeTimeClockByPaytype(DateTime FromDate, DateTime EndDate)
        {
            string sqlQuery = @"select SUM(ClockedinSeconds) TotalWorkingSeconds, UserId  from EmployeeTimeClock                                where ClockInTime >= '{0}' and ClockInTime <= '{1}'                                 Group by UserId";
            try
            {
                sqlQuery = string.Format(sqlQuery, FromDate, EndDate);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetAllEmployeeTimeClockBySalaryPaytype(DateTime FromDate, DateTime EndDate)
        {
            string sqlQuery = @"select SUM(ClockedinSeconds) TotalWorkingSeconds, UserId  from EmployeeTimeClock                                where ClockInTime >= '{0}' and ClockInTime <= '{1}'                                 Group by UserId";
            try
            {
                sqlQuery = string.Format(sqlQuery, FromDate, EndDate);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetEmployeePtohour(Guid UserId)
        {
            string sqlQuery = @"Select Sum(PTOHour) As TotalPTOHour from EmployeePTOHourLog Where UserId = '{0}' ";
            try
            {
                sqlQuery = string.Format(sqlQuery, UserId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetEmployeePtoHourByUserId(Guid UserId)
        {
            string sqlQuery = @"Select IsNull(Sum(Minute),0) As TotalMinute from Pto Where UserId = '{0}' and Status = 'Accepted' and Payable = 1 ";
            try
            {
                sqlQuery = string.Format(sqlQuery, UserId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetEmployeeAccrualPtoHourByUserId(Guid UserId)
        {
            string sqlQuery = @"Select IsNull(Sum(Minute),0) As TotalMinute from Pto Where UserId = '{0}' and Status = 'Accepted'";
            try
            {
                sqlQuery = string.Format(sqlQuery, UserId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetAllEmployeeByAnniversaryDate(Guid companyid, DateTime StartDate, DateTime EndDate)
        {
            string sqlQuery = @" select emp.* , ul.Id as UserLoginId,  (YEAR(getdate()) - YEAR(emp.HireDate)) as AnniversaryYears from  Employee emp
                                 left join UserCompany uc
                                 on uc.UserId = emp.UserId
								 left join UserLogin ul on ul.UserId = emp.UserId
                                 where Uc.CompanyId ='{0}'
                                 and DATEADD( Year, DATEPART( Year, GETDATE()) - DATEPART( Year, emp.HireDate), emp.HireDate)
								 BETWEEN CONVERT( DATE, '{1}') AND CONVERT( DATE, '{2}')
                                 AND YEAR(emp.HireDate) != YEAR(getdate())
                                 and emp.IsActive = 'true' and emp.IsDeleted = 'false'
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, StartDate, EndDate);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                    //return GetList(cmd, -1);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Employee> GetAllEmployeeByCompanyId(Guid companyid, string userid)
        {
            string subquery = "";
            string sqlQuery = @" select emp.*,
                                emp.FirstName + ' ' + emp.LastName as EmpName,
                                pg.Name as PermissionGroupName
                                    from UserCompany uc
                                    left join UserLogin ul 
	                                on ul.UserId = uc.UserId
                                    left join UserPermission up 
	                                on up.UserId = ul.UserId
                                left join PermissionGroup pg 
	                                on pg.Id = up.PermissionGroupId
                                Left Join Employee emp
	                                on emp.UserId = ul.UserId
                                where ul.Id is not null 
	                                and pg.Name is not null
                                    and emp.FirstName is not null 
	                                and emp.LastName is not null
                                    and emp.Recruited =1 
                                    and uc.CompanyId ='{0}' 
                                    and emp.IsActive = 1
                                    and emp.IsDeleted = 0
                                    {1}";
            if (!string.IsNullOrWhiteSpace(userid) && userid != new Guid().ToString())
            {
                subquery = string.Format("and emp.UserId = '{0}'", userid);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, subquery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    SqlDataReader reader;
                    long result = SelectRecords(cmd, out reader);
                    EmployeeList list = new EmployeeList();

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            Employee employeeObject = new Employee();
                            FillObject(employeeObject, reader);
                            employeeObject.EMPName = reader["EmpName"].ToString();
                            employeeObject.PermissionGroupName = reader["PermissionGroupName"].ToString();
                            list.Add(employeeObject);
                        }
                        reader.Close();
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Employee> GetAllCalendarEmployeeByCompanyId(Guid companyid, string userid)
        {
            string subquery = "";
            string sqlQuery = @" select emp.*,
                                emp.FirstName + ' ' + emp.LastName as EmpName,
                                pg.Name as PermissionGroupName
                                    from UserCompany uc
                                    left join UserLogin ul 
	                                on ul.UserId = uc.UserId
                                    left join UserPermission up 
	                                on up.UserId = ul.UserId
                                left join PermissionGroup pg 
	                                on pg.Id = up.PermissionGroupId
                                Left Join Employee emp
	                                on emp.UserId = ul.UserId
                                where ul.Id is not null 
	                                and pg.Name is not null
                                    and emp.FirstName is not null 
	                                and emp.LastName is not null
                                    and emp.Recruited =1 
                                    and uc.CompanyId ='{0}' 
                                    and emp.IsActive = 1
                                    and emp.IsDeleted = 0
                                    and emp.IsCalendar = 1
                                    {1}";
            if (!string.IsNullOrWhiteSpace(userid) && userid != new Guid().ToString())
            {
                subquery = string.Format("and emp.UserId = '{0}'", userid);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, subquery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    SqlDataReader reader;
                    long result = SelectRecords(cmd, out reader);
                    EmployeeList list = new EmployeeList();

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            Employee employeeObject = new Employee();
                            FillObject(employeeObject, reader);
                            employeeObject.EMPName = reader["EmpName"].ToString();
                            employeeObject.PermissionGroupName = reader["PermissionGroupName"].ToString();
                            list.Add(employeeObject);
                        }
                        reader.Close();
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Employee> GetAllEmployeeByCompanyIdAndKey(Guid companId, string key)
        {
            string sqlQuery = @" declare @SearchText nvarchar(100)

                                set @SearchText = '%{1}%'
                                select emp.*,
                                emp.FirstName + ' ' + emp.LastName as EmpName,
                                pg.Name as PermissionGroupName
                                    from UserCompany uc
                                    left join UserLogin ul 
	                                on ul.UserId = uc.UserId
                                    left join UserPermission up 
	                                on up.UserId = ul.UserId
                                left join PermissionGroup pg 
	                                on pg.Id = up.PermissionGroupId
                                Left Join Employee emp
	                                on emp.UserId = ul.UserId
                                where ul.Id is not null 
	                                and pg.Name is not null
                                    and emp.FirstName is not null 
	                                and emp.LastName is not null
                                    and emp.Recruited =1 
                                    and emp.FirstName+' '+emp.LastName like @SearchText
                                    and uc.CompanyId ='{0}'
                                    and emp.IsActive = 1 and emp.IsDeleted = 0";
            try
            {
                sqlQuery = string.Format(sqlQuery, companId, key);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    SqlDataReader reader;
                    long result = SelectRecords(cmd, out reader);
                    EmployeeList list = new EmployeeList();

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            Employee employeeObject = new Employee();
                            FillObject(employeeObject, reader);
                            employeeObject.EMPName = reader["EmpName"].ToString();
                            employeeObject.PermissionGroupName = reader["PermissionGroupName"].ToString();
                            list.Add(employeeObject);
                        }
                        reader.Close();
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetEmployeeByuserIdandCompanyId(Guid companyid, Guid userid)
        {
            string sqlQuery = @"select *,ul.Id as UserLoginId from employee emp
                                left join UserLogin ul on ul.UserId = emp.UserId
                                left join UserCompany uc on uc.Userid = ul.UserId
                                where emp.UserId = '{1}'
                                and uc.CompanyId = '{0}'
                                ";
            sqlQuery = string.Format(sqlQuery, companyid, userid);
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

        public DataTable GetNewEmployeeByuserIdandCompanyId(Guid userid)
        {
            string sqlQuery = @"select emp.* from Employee emp
                                Where emp.UserId = '{0}'
                                ";
            sqlQuery = string.Format(sqlQuery, userid);
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
        public Employee GetEmployeeByUserLoginId(int userId)
        {
            string sqlQuery = @"select emp.* from Employee emp
                                left join UserLogin ul on emp.UserId = ul.UserId
                                where ul.id = {0}";
            try
            {
                sqlQuery = string.Format(sqlQuery, userId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    return GetObject(cmd);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public Employee GetNewEmployeeByUserLoginId(Guid userId)
        {
            string sqlQuery = @"select emp.PasswordUpdateDays from Employee emp
                                where emp.UserId = {0}";
            try
            {
                sqlQuery = string.Format(sqlQuery, userId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    return GetObject(cmd);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetAllEmployeeNameByEmployeeId(Guid companyid)
        {
            string sqlQuery = @"select cn.*,
                                emp.FirstName as EmployeeName
                                from CustomerNote cn
                                left join Employee emp
                                on emp.UserId='{0}'
                                where cn.CompanyId='{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllQAEmployee(Guid companyId)
        {
            string sqlQuery = @"select 
	                                _Employee.FirstName ,
	                                _Employee.LastName,
	                                _Employee.UserName
                                 from
                                Employee _Employee
                                left join 
                                UserPermission _Up
                                on _Employee.UserId = _up.UserId
                                left join PermissionGroup _Pg
                                on _Up.PermissionGroupId = _Pg.Id
                                left join UserCompany _Uc
                                on _Employee.UserId = _Uc.UserId
                                where _Pg.Name = 'QA' and
                                _Uc.CompanyId = '{0}'
                                 and _Employee.IsActive = 1 and _Employee.IsDeleted = 0";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllEmployeeByEmpIdList(string EmpIdList)
        {
            string EmpIdQuery = "";
            if (!string.IsNullOrEmpty(EmpIdList))
            {
                EmpIdQuery = string.Format(" UserId in ({0})", EmpIdList);
            }
            string sqlQuery = @"select  *
                                from
                                Employee  where
                                {0}";
            try
            {
                sqlQuery = string.Format(sqlQuery, EmpIdQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<Employee> GetEmployeeListBySupervisorId(Guid userId)
        {
            string sqlQuery = @"select * from employee where SuperVisorId = '{0}' and IsActive = 'true' and IsDeleted = 'false'";
            sqlQuery = string.Format(sqlQuery, userId);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return GetList(cmd, -1);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public List<Employee> GetTimeClockEmployeeListBySupervisorId(Guid userId)
        {
            string sqlQuery = @"select emp.* from EmployeeTimeClockSuperVisor etcs
                                left join Employee emp on etcs.UserId = emp.UserId
                                where etcs.SuperVisorId = '{0}'
                                and emp.IsActive = 1
                                and emp.IsDeleted = 0";
            sqlQuery = string.Format(sqlQuery, userId);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return GetList(cmd, -1);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Employee> GetEmployeeListByUserTag(Guid companyid, Guid userid)
        {
            string sqlQuery = @"select * from employee emp
                                left join UserLogin ul on ul.UserId = emp.UserId
                                left join UserCompany uc on uc.Userid = ul.UserId
                                where emp.UserId != '{1}'
                                and uc.CompanyId = '{0}' and emp.IsActive = 1 and emp.IsDeleted = 0 and emp.Recruited = 1
                                ";
            sqlQuery = string.Format(sqlQuery, companyid, userid);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return GetList(cmd, -1);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Employee> GetCurrentEmployeeListByCompanyId(Guid companyid)
        {
            string sqlQuery = @"select * from employee emp
                                left join UserLogin ul on ul.UserId = emp.UserId
                                left join UserCompany uc on uc.Userid = ul.UserId
                                where uc.CompanyId = '{0}' 
                                and emp.IsActive = 1 and emp.IsDeleted = 0 and emp.Recruited = 1 and emp.IsCurrentEmployee=1
                                ";
            sqlQuery = string.Format(sqlQuery, companyid);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return GetList(cmd, -1);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable GetAllWorkPersonEmployee(Guid companyid)
        {
            string sqlQuery = @"select _emp.UserId, _emp.FirstName, _emp.LastName, _emp.UserName
                                from 
                                 Employee _emp 
                                 left join UserPermission _up
                                     on _emp.UserId = _up.UserId
                                 left join PermissionGroup _pg
                                     on _up.PermissionGroupId = _pg.Id
                                 left join UserCompany _uc
                                     on _uc.UserId = _up.UserId
                                    left join UserLogin _ul 
                                      on _ul.UserId = _emp.UserId
		                              and _uc.CompanyId = '{0}'
                                     and _pg.Name is not null
                                     and _pg.Name = 'Technician'
                                    and _ul.IsActive = 'true'
                                    and _ul.IsDeleted = 'false'
                                    and _emp.IsActive = 'true'
                                    and _emp.IsDeleted = 'false'
									group by _emp.UserId, _emp.FirstName, _emp.LastName, _emp.UserName";
            sqlQuery = string.Format(sqlQuery, companyid);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetAllServiceOrderEmployee()
        {
            string sqlQuery = @"select*
                from Employee emp 
                where
                 emp.IsSupervisor = 1 and emp.IsActive = 'true' and emp.IsDeleted = 'false'";
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetEmployee()
        {
            string sqlQuery = @"select distinct emp.* from Employee emp
                                
                                   left join Customer cus on cus.SoldBy2 = emp.UserId
                                where cus.FollowUpDate != ''
                                ";
            sqlQuery = string.Format(sqlQuery);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable Getfollowupsalesperson()
        {
            string sqlQuery = @"select distinct emp.* from Employee emp
                                
                                   left join Customer cus on cus.Soldby1 = emp.UserId
                                where cus.FollowUpDate != ''
                                ";
            sqlQuery = string.Format(sqlQuery);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetSalesPerSon()
        {
            string sqlQuery = @"select distinct emp.* from Employee emp
                                
                                   
                                ";
            sqlQuery = string.Format(sqlQuery);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetSalesPerSonCustomer()
        {
            string sqlQuery = @"select distinct cus.* from Customer cus
                                
                                   
                                ";
            sqlQuery = string.Format(sqlQuery);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetEmployeeByCompanyIdAndTag(Guid companyId, string tag, Guid userid, string tag2 = "")

        {

            string sqlQuery = @"
                                select emp.*, emp.Street+' '+emp.ZipCode+' '+emp.City+', '+emp.State as [Address] from Employee emp 
                                left join UserCompany uc
	                                on emp.UserId = uc.UserId
                                left join UserPermission up 
	                                on up.UserId = emp.UserId
                                left join PermissionGroup pg
	                                on pg.Id = up.PermissionGroupId
                                left join UserLogin ul 
	                                on emp.UserId = ul.UserId
                                where up.PermissionGroupId is not null 
                                and up.PermissionGroupId > 0
                                --and ul.IsActive = 1
                                and ul.IsDeleted = 0
                                and uc.CompanyId = '{0}'
                                and emp.IsActive = 1
                                and emp.IsDeleted = 0
                                {1}
                                {2}
                                order by emp.FirstName
                                ";
            string tagQuery = string.Format("and pg.Tag like '%{0}%'", tag);
            if (!string.IsNullOrEmpty(tag2))
            {
                tagQuery = string.Format("and (pg.Tag like '%{0}%' or pg.Tag like '%{1}%')", tag, tag2);
            }
            string subquery = "";
            if (userid != null && userid != new Guid())
            {
                subquery = string.Format("and emp.UserId = '{0}'", userid);
            }
            sqlQuery = string.Format(sqlQuery, companyId, tagQuery, subquery);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
           public DataTable GetEmployeeByCompanyIdAndTagTechnician(Guid companyId, string tag, Guid userid, string tag2 = "")

        {
            string sqlQuery = @"
                                  WITH CTE AS (
                                    SELECT 
                                        emp.UserId, 
                                        emp.FirstName,
                                        emp.LastName,
                                        emp.Street + ' ' + emp.ZipCode + ' ' + emp.City + ', ' + emp.State AS [Address], 
                                        (
                                            SELECT ISNULL(SUM(_tech.Quantity), 0)
                                            FROM inventorytech _tech
                                            WHERE _tech.[Type] = 'Add' AND _tech.EquipmentId = tech.EquipmentId AND _tech.TechnicianId = tech.TechnicianId
                                        ) - (
                                            SELECT ISNULL(SUM(_tech.Quantity), 0)
                                            FROM inventorytech _tech
                                            WHERE _tech.[Type] = 'Release' AND _tech.EquipmentId = tech.EquipmentId AND _tech.TechnicianId = tech.TechnicianId
                                        ) - (
                                            SELECT ISNULL(SUM(b.Quantity), 0)
                                            FROM dbo.AssignedInventoryTechReceived b
                                            WHERE b.EquipmentId = tech.EquipmentId AND (b.TechnicianId = tech.TechnicianId OR (b.TechnicianId = '22222222-2222-2222-2222-222222222221' AND b.ReceivedBy = tech.TechnicianId))
                                                AND b.IsApprove = 0 AND b.IsDecline = 0
                                        ) AS Quantity,
                                        ROW_NUMBER() OVER (PARTITION BY tech.EquipmentId, tech.TechnicianId ORDER BY MAX(tech.LastUpdatedDate) DESC) AS RowNum,
                                        MAX(tech.LastUpdatedDate) AS LastUpdatedDate,
                                        tech.EquipmentId,
                                        tech.TechnicianId 
                                    FROM 
                                        Employee emp 
                                    LEFT JOIN 
                                        UserCompany uc ON emp.UserId = uc.UserId
                                    LEFT JOIN 
                                        UserPermission up ON up.UserId = emp.UserId
                                    LEFT JOIN 
                                        PermissionGroup pg ON pg.Id = up.PermissionGroupId
                                    LEFT JOIN 
                                        UserLogin ul ON emp.UserId = ul.UserId

                                    LEFT JOIN 
                                        InventoryTech tech ON tech.TechnicianId = emp.UserId
                                    WHERE 
                                        up.PermissionGroupId IS NOT NULL 
                                        AND up.PermissionGroupId > 0
                                        AND ul.IsDeleted = 0
 
                                        AND ul.CompanyId = '{0}'
                                        AND emp.IsActive = 1
                                        AND emp.IsDeleted = 0
                                        AND tech.TechnicianId NOT IN (
                                            '22222222-2222-2222-2222-222222222223',
                                            '22222222-2222-2222-2222-222222222222',
                                            '22222222-2222-2222-2222-222222222224',
                                            '22222222-2222-2222-2222-222222222225',
                                            '22222222-2222-2222-2222-222222222226',
                                            '22222222-2222-2222-2222-222222222231',
                                            '22222222-2222-2222-2222-222222222232',
                                            '22222222-2222-2222-2222-222222222233'
                                        )
                                            {1}
                                            {2}
                                        GROUP BY
                                            emp.UserId, 
                                            emp.FirstName,
                                            emp.LastName,
                                            emp.Street,
                                            emp.ZipCode,
                                            emp.City,
                                            emp.State,
                                            tech.EquipmentId,
                                            tech.TechnicianId
                                    )
                                   SELECT 
                                    CTE.*, 
                                    COUNT(*) OVER(PARTITION BY CTE.TechnicianId) AS TechnicianCount
                                FROM CTE
                                WHERE RowNum = 1 AND Quantity > 0
                                ORDER BY FirstName;
                                ";

            string tagQuery = string.Format("and pg.Tag like '%{0}%'", tag);

            if (!string.IsNullOrEmpty(tag2))
            {
                tagQuery = string.Format("and (pg.Tag like '%{0}%' or pg.Tag like '%{1}%')", tag, tag2);
            }
            string subquery = "";
            if (userid != null && userid != new Guid())
            {
                subquery = string.Format("and emp.UserId = '{0}'", userid);
            }
            sqlQuery = string.Format(sqlQuery, companyId, tagQuery, subquery);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetEmployeeByCompanyIdAndPerGrpId(Guid companyId, string tag, Guid userid, string PerGrpId)

        {

            string sqlQuery = @"
                                select emp.*, emp.Street+' '+emp.ZipCode+' '+emp.City+', '+emp.State as [Address] from Employee emp 
                                left join UserCompany uc
	                                on emp.UserId = uc.UserId
                                left join UserPermission up 
	                                on up.UserId = emp.UserId
                                left join PermissionGroup pg
	                                on pg.Id = up.PermissionGroupId
                                left join UserLogin ul 
	                                on emp.UserId = ul.UserId
                                where up.PermissionGroupId is not null 
                                and up.PermissionGroupId > 0
                                --and ul.IsActive = 1
                                and ul.IsDeleted = 0
                                and uc.CompanyId = '{0}'
                                and emp.IsActive = 1
                                and emp.IsDeleted = 0
                                and emp.Recruited = 1 
                                and emp.IsCurrentEmployee = 1
                                {1}
                                {2}
                                
                                ";
            string tagQuery = string.Format("and pg.Tag like '%{0}%'", tag);
            if (!string.IsNullOrEmpty(PerGrpId))
            {
                tagQuery = string.Format("and pg.Id in ({0})", PerGrpId);
            }
            string subquery = "";
            if (userid != null && userid != new Guid())
            {
                subquery = string.Format("and emp.UserId = '{0}'", userid);
            }
            sqlQuery = string.Format(sqlQuery, companyId, tagQuery, subquery);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllEmployeeByCompanyId(Guid companyId)

        {

            string sqlQuery = @"
                                select emp.*
                                from Employee emp
                                where emp.IsActive = 1
                                and emp.IsDeleted = 0
                                and emp.CompanyId='{0}'
                                ";

            sqlQuery = string.Format(sqlQuery, companyId);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetTechtransferEmployeeByCompanyIdAndTag(Guid companyId, string searchtext, string tag, Guid userid, string tag2 = "")

        {
            string searchquery = "";
            if (!string.IsNullOrEmpty(searchtext))
            {
                searchquery = string.Format("and (emp.FirstName+' '+emp.LastName like '%{0}%' )", searchtext);
            }
            string sqlQuery = @"
                                select emp.*, emp.Street+' '+emp.ZipCode+' '+emp.City+', '+emp.State as [Address] from Employee emp 
                                left join UserCompany uc
	                                on emp.UserId = uc.UserId
                                left join UserPermission up 
	                                on up.UserId = emp.UserId
                                left join PermissionGroup pg
	                                on pg.Id = up.PermissionGroupId
                                left join UserLogin ul 
	                                on emp.UserId = ul.UserId
                                where up.PermissionGroupId is not null 
                                and up.PermissionGroupId > 0
                                --and ul.IsActive = 1
                                and ul.IsDeleted = 0
                                and uc.CompanyId = '{0}'
                                and emp.IsActive = 1
                                and emp.IsDeleted = 0
                                {1}
                                {2}
                                 {3}
								order by emp.FirstName + emp.LastName

                                
                                ";
            string tagQuery = string.Format("and pg.Tag like '%{0}%'", tag);
            if (!string.IsNullOrEmpty(tag2))
            {
                tagQuery = string.Format("and (pg.Tag like '%{0}%' or pg.Tag like '%{1}%')", tag, tag2);
            }
            string subquery = "";
            //if (userid != null && userid != new Guid())
            //{
            //    subquery = string.Format("and emp.UserId = '{0}'", userid);
            //}
            sqlQuery = string.Format(sqlQuery, companyId, tagQuery, subquery, searchquery);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetEmployeeByCompanyIdAndTagAndSearch(Guid companyId, string searchtext, string tag, Guid userid, string tag2 = "")
        {
            string searchquery = "";
            if (!string.IsNullOrEmpty(searchtext))
            {
                searchquery = string.Format("and (emp.FirstName+' '+emp.LastName like '%{0}%' )", searchtext);
            }

            string sqlQuery = @"
                                select emp.*, emp.Street+' '+emp.ZipCode+' '+emp.City+', '+emp.State as [Address] from Employee emp 
                                left join UserCompany uc
	                                on emp.UserId = uc.UserId
                                left join UserPermission up 
	                                on up.UserId = emp.UserId
                                left join PermissionGroup pg
	                                on pg.Id = up.PermissionGroupId
                                left join UserLogin ul 
	                                on emp.UserId = ul.UserId
                                where up.PermissionGroupId is not null 
                                and up.PermissionGroupId > 0
                                --and ul.IsActive = 1
                                and ul.IsDeleted = 0
                                and uc.CompanyId = '{0}'
                                and emp.IsActive = 1
                                and emp.IsDeleted = 0
                                {1}
                                {2}
                                {3}
								order by emp.FirstName + emp.LastName
                                ";
            string tagQuery = string.Format("and pg.Tag like '%{0}%'", tag);
            if (!string.IsNullOrEmpty(tag2))
            {
                tagQuery = string.Format("and (pg.Tag like '%{0}%' or pg.Tag like '%{1}%')", tag, tag2);
            }
            string subquery = "";
            if (userid != null && userid != new Guid())
            {
                subquery = string.Format("and emp.UserId = '{0}'", userid);
            }
            sqlQuery = string.Format(sqlQuery, companyId, tagQuery, subquery, searchquery);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetTechnicianByCompanyIdAndTagAndSearch(Guid companyId, DateTime? start, DateTime? end, int pageno, int pagesize, string searchtext, string order, string tag)
        {
            string sqlQuery = @"";
            string AppointmentDateQuery = "";

            string searchquery = "";
            if (!string.IsNullOrEmpty(searchtext))
            {
                searchquery = string.Format("and (emp.FirstName+' '+emp.LastName like '%{0}%' )", searchtext);
            }

            string tagQuery = string.Format("and pg.Tag like '%{0}%'", tag);
            #region AppointmentDateQuery
            if (start != null && start != new DateTime() && end != null && end != new DateTime())
            {
                var StartDate = start.Value.SetZeroHour();
                var EndDate = end.Value.SetMaxHour();
                AppointmentDateQuery = string.Format("and emp.CreatedDate between '{0}' and '{1}'", StartDate, EndDate);
            }
            #endregion

            sqlQuery = @"DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int

                                SET @pageno = {2} --default 1
                                SET @pagesize = {3} --default 10

                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize
                                
                                select emp.Id,emp.UserId,emp.FirstName, emp.LastName, emp.UserName 
                                into #employeedata from Employee emp 
                                left join UserCompany uc
	                                on emp.UserId = uc.UserId
                                left join UserPermission up 
	                                on up.UserId = emp.UserId
                                left join PermissionGroup pg
	                                on pg.Id = up.PermissionGroupId
                                left join UserLogin ul 
	                                on emp.UserId = ul.UserId
                                where up.PermissionGroupId is not null 
                                and up.PermissionGroupId > 0
                                --and ul.IsActive = 1
                                and ul.IsDeleted = 0
                                and uc.CompanyId = '{1}'
                                and emp.IsActive = 1
                                and emp.IsDeleted = 0
                                {0}
                                {4}
                                {5}
								order by emp.FirstName + emp.LastName 
                               
                                select * into #employeeIddata from #employeedata
								select top(@pagesize) * into #Testtable from #employeeIddata 
								where Id not in (Select TOP (@pagestart) Id from #employeedata #e order by #e.FirstName + #e.LastName )
                                order by FirstName + LastName

                                select * from #Testtable
                              
                                  
                                select  count(Id) as [TotalCount] from #employeeIddata
                      

						  drop table #employeedata
						  drop table #EmployeeIdData
                          drop table #Testtable ";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        searchquery,//0
                                        companyId,  //1
                                        pageno, //2
                                        pagesize, //3
                                        AppointmentDateQuery,//4
                                        tagQuery//5
                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetInventoryTechTruckReportForDownload(Guid ComId, DateTime? start, DateTime? end, string searchtext, string tag)
        {
            string AppointmentDateQuery = "";

            string searchquery = "";
            if (!string.IsNullOrEmpty(searchtext))
            {
                searchquery = string.Format("and (emp.FirstName+' '+emp.LastName like '%{0}%' )", searchtext);
            }

            string tagQuery = string.Format("and pg.Tag like '%{0}%'", tag);
            #region AppointmentDateQuery
            if (start != null && start != new DateTime() && end != null && end != new DateTime())
            {
                var StartDate = start.Value.SetZeroHour();
                var EndDate = end.Value.SetMaxHour();
                AppointmentDateQuery = string.Format("and emp.CreatedDate between '{0}' and '{1}'", StartDate, EndDate);
            }
            #endregion

            string sqlQuery = @"                            
                                select emp.UserId,emp.FirstName + ' ' + emp.LastName as EmpName 
                                into #tempEmp 
                                from Employee emp 
                                left join UserCompany uc
	                                on emp.UserId = uc.UserId
                                left join UserPermission up 
	                                on up.UserId = emp.UserId
                                left join PermissionGroup pg
	                                on pg.Id = up.PermissionGroupId
                                left join UserLogin ul 
	                                on emp.UserId = ul.UserId
                                where up.PermissionGroupId is not null 
                                and up.PermissionGroupId > 0
                                --and ul.IsActive = 1
                                and ul.IsDeleted = 0
                                and uc.CompanyId = '{1}'
                                and emp.IsActive = 1
                                and emp.IsDeleted = 0
                                {0}
                                {2}
                                {3}
								order by EmpName
                                 
                                select EquipmentId, TechnicianId
                                into #tempTable
                                from InventoryTech
                                Group by EquipmentId, TechnicianId

                                 DECLARE
                                 @columns NVARCHAR(MAX) = '',
                                 @sql NVARCHAR(MAX) = '';

                                -- select the category names
                                 SELECT
                                 @columns+=QUOTENAME(eqp.Name + '(' + convert(nvarchar(MAX), eqp.Id) + ')') + ','
                                 FROM
                                 Equipment eqp where eqp.EquipmentClassId=1
                                 ORDER BY
                                 Name;

                            -- remove the last comma
                                SET @columns = LEFT(@columns, LEN(@columns) - 1);

                        -- construct dynamic SQL
                               SET @sql ='
                               SELECT * into 
                               ##resulttechtrack FROM
                               (
	                           SELECT
	                           (eqp.Name + ''('' + convert(nvarchar(MAX), eqp.Id) + '')'') as Name,
	                           (EmpName) as [Technician Name],
                               UserId,
	                           iif((isnull(
                              (select ISNULL(sum(quantity),0) from InventoryTech _it 
                              WHERE _it.EquipmentId = eqp.EquipmentId
                              AND _it.TechnicianId = emp.UserId 
                              AND _it.[Type] = ''Add''), 0) - isnull(
                              (select ISNULL(sum(quantity) ,0) from InventoryTech _it 
                              WHERE _it.EquipmentId = eqp.EquipmentId 
                              AND _it.TechnicianId = emp.UserId and _it.[Type] = ''Release''), 0)) < 0, 0, isnull(
                              (select ISNULL(sum(quantity) ,0) from InventoryTech _it
                               WHERE _it.EquipmentId = eqp.EquipmentId 
                               AND _it.TechnicianId = emp.UserId and _it.[Type] = ''Add''), 0) - isnull(
                              (select ISNULL(sum(quantity) ,0) from InventoryTech _it 
                               WHERE _it.EquipmentId = eqp.EquipmentId 
                               AND _it.TechnicianId = emp.UserId and _it.[Type] = ''Release''), 0)) as OnHand
	                           FROM
		                       #tempEmp emp

		                       left join #tempTable it on it.TechnicianId=emp.UserId
		                       left join Equipment eqp on eqp.EquipmentId=it.EquipmentId
		                       where eqp.EquipmentClassId=1
		 
                               ) t
                               PIVOT(
	                             sum(OnHand)
	                           FOR [Name] IN ('+ @columns +')
                                ) AS pivot_table;';

                           -- execute the dynamic SQL
                              EXECUTE sp_executesql @sql

                              Declare @userid uniqueidentifier
							    Declare @technane nvarchar(500)

							  While (Select Count(*)
                              From #tempEmp where UserId NOT IN (select UserId from ##resulttechtrack)) > 0
                              Begin
							  

                              Select Top 1 @userid = UserId, @technane = EmpName From #tempEmp where UserId NOT IN (select UserId from ##resulttechtrack)

                              INSERT INTO ##resulttechtrack ([Technician Name], [UserId]) VALUES (@technane ,@userid )

                              Delete FROM #tempEmp Where UserId = @userid

                              End 
							  
							  DECLARE db_cursor CURSOR FOR select [Name],Id from Equipment where EquipmentClassId =1
							DECLARE @Name VARCHAR(256);
							DECLARE @Id int;
							OPEN db_cursor;
							FETCH NEXT FROM db_cursor INTO @Name,@Id ;
							WHILE @@FETCH_STATUS = 0  
							BEGIN  
									set @Name = '['+@Name + '('+CAST(@Id AS nvarchar(50))+')]';
									print @Name
									set @sql = 'update ##resulttechtrack set '+@Name +'= 0 where '+@Name+' is null;';
									EXECUTE sp_executesql @sql

									FETCH NEXT FROM db_cursor INTO @Name,@Id;
							END;
							CLOSE db_cursor;
							DEALLOCATE db_cursor;
							  
						    select * from ##resulttechtrack order by [Technician Name]


							  drop table  ##resulttechtrack
                              drop table #tempTable
                              drop table #tempEmp";

            //    string sqlQuery = @"
            //                        DECLARE @Start nvarchar(50)
            //                        DECLARE @End nvarchar(50) 

            //                        SET @Start = '{0}'
            //                        SET @End = '{1}'

            //                        select DISTINCT Eq.SKU
            //                        ,Eq.Name
            //                        ,(((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=Eq.EquipmentId and Type='Add' {2})
            //                        - (Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=Eq.EquipmentId and Type='Release' {2}))
            //                        +
            //((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=Eq.EquipmentId and Type='Add' {2})
            //                        - (Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=Eq.EquipmentId and Type='Release' {2}))
            //                        ) as '{0}'

            //                        ,(((Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=Eq.EquipmentId and Type='Add' {3})
            //                        - (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=Eq.EquipmentId and Type='Release' {3}))
            //                        +
            //((Select ISNULL(SUM(invinner2.Quantity),0) from InventoryTech invinner2 where invinner2.EquipmentId=Eq.EquipmentId and Type='Add' {3})
            //                        - (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryTech invinner2 where invinner2.EquipmentId=Eq.EquipmentId and Type='Release' {3}))
            //                        ) as '{1}'


            //                        ,(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=Eq.EquipmentId and Type='Release' {4}) as Used 

            //                        ,(Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=Eq.EquipmentId and Type='Add' {4}) as Purchase

            //,(Select ISNULL(SUM(EquRe.Quantity),0) from Equipmentreturn EquRe where EquRe.EquipmentId=Eq.EquipmentId {6}) as RMA

            //                        into #EquipmentData 

            //                         from Equipment Eq
            //                        left join InventoryWarehouse IW on Eq.EquipmentId = IW.EquipmentId
            //                        where Eq.EquipmentClassId = 1
            //{5}
            //                        {7}
            //                        select * from #EquipmentData
            //                        where REPLACE('{0}', ' ', '') != NULL or REPLACE('{1}', ' ', '') != NULL or Used > 0 or Purchase > 0 or RMA > 0
            //Order by SKU asc

            //                        DROP TABLE #EquipmentData";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        searchquery,//0
                                        ComId,  //1
                                        AppointmentDateQuery,//2
                                        tagQuery//3
                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetInventoryTechUsedReportForDownload(Guid ComId, DateTime? start, DateTime? end, string searchtext, string tag)
        {
            string AppointmentDateQuery = "";

            string searchquery = "";
            if (!string.IsNullOrEmpty(searchtext))
            {
                searchquery = string.Format("and (emp.FirstName+' '+emp.LastName like '%{0}%' )", searchtext);
            }

            string tagQuery = string.Format("and pg.Tag like '%{0}%'", tag);
            #region AppointmentDateQuery
            if (start != null && start != new DateTime() && end != null && end != new DateTime())
            {
                var StartDate = start.Value.SetZeroHour();
                var EndDate = end.Value.SetMaxHour();
                AppointmentDateQuery = string.Format("and emp.CreatedDate between '{0}' and '{1}'", StartDate, EndDate);
            }
            #endregion

            string sqlQuery = @"                            
                                select emp.UserId,emp.FirstName + ' ' + emp.LastName as EmpName 
                                into #tempEmp 
                                from Employee emp 
                                left join UserCompany uc
	                                on emp.UserId = uc.UserId
                                left join UserPermission up 
	                                on up.UserId = emp.UserId
                                left join PermissionGroup pg
	                                on pg.Id = up.PermissionGroupId
                                left join UserLogin ul 
	                                on emp.UserId = ul.UserId
                                where up.PermissionGroupId is not null 
                                and up.PermissionGroupId > 0
                                --and ul.IsActive = 1
                                and ul.IsDeleted = 0
                                and uc.CompanyId = '{1}'
                                and emp.IsActive = 1
                                and emp.IsDeleted = 0
                                {0}
                                {2}
                                {3}
								order by EmpName
                                 
                                select EquipmentId, TechnicianId
                                into #tempTable
                                from InventoryTech
                                Group by EquipmentId, TechnicianId

                                 DECLARE
                                 @columns NVARCHAR(MAX) = '',
                                 @sql NVARCHAR(MAX) = '';

                                -- select the category names
                                 SELECT
                                 @columns+=QUOTENAME(eqp.Name + '(' + convert(nvarchar(MAX), eqp.Id) + ')') + ','
                                 FROM
                                 Equipment eqp where eqp.EquipmentClassId=1
                                 ORDER BY
                                 Name;

                            -- remove the last comma
                                SET @columns = LEFT(@columns, LEN(@columns) - 1);

                        -- construct dynamic SQL
                               SET @sql ='
                               SELECT * into 
                               ##resulttechused FROM
                               (
	                           SELECT
	                           (eqp.Name + ''('' + convert(nvarchar(MAX), eqp.Id) + '')'') as Name,
	                           (EmpName) as [Technician Name],
                               UserId,
	                           (isnull(
                               (select ISNULL(sum(quantity),0) from InventoryTech _it 
                               where _it.EquipmentId = eqp.EquipmentId  
                               and _it.TechnicianId = emp.UserId and _it.[Type] = ''Release''), 0)) as Used
	                           FROM
		                       #tempEmp emp

		                       left join #tempTable it on it.TechnicianId=emp.UserId
		                       left join Equipment eqp on eqp.EquipmentId=it.EquipmentId
		                       where eqp.EquipmentClassId=1
		 
                               ) t
                               PIVOT(
	                             sum(Used)
	                           FOR [Name] IN ('+ @columns +')
                                ) AS pivot_table;';

                           -- execute the dynamic SQL
                              EXECUTE sp_executesql @sql

                              Declare @userid uniqueidentifier
							  Declare @technane nvarchar(500)

							  While (Select Count(*)
                              From #tempEmp where UserId NOT IN (select UserId from ##resulttechused)) > 0
                              Begin
							  

                              Select Top 1 @userid = UserId, @technane = EmpName From #tempEmp where UserId NOT IN (select UserId from ##resulttechused)

                              INSERT INTO ##resulttechused ([Technician Name], [UserId]) VALUES (@technane ,@userid )

                              Delete FROM #tempEmp Where UserId = @userid

                              End 
							  
							  DECLARE db_cursor CURSOR FOR select [Name],Id from Equipment where EquipmentClassId =1
							DECLARE @Name VARCHAR(256);
							DECLARE @Id int;
							OPEN db_cursor;
							FETCH NEXT FROM db_cursor INTO @Name,@Id ;
							WHILE @@FETCH_STATUS = 0  
							BEGIN  
									set @Name = '['+@Name + '('+CAST(@Id AS nvarchar(50))+')]';
									print @Name
									set @sql = 'update ##resulttechused set '+@Name +'= 0 where '+@Name+' is null;';
									EXECUTE sp_executesql @sql

									FETCH NEXT FROM db_cursor INTO @Name,@Id;
							END;
							CLOSE db_cursor;
							DEALLOCATE db_cursor;
							  
						    select * from ##resulttechused order by [Technician Name]

                              drop table #tempTable
                              drop table #tempEmp";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        searchquery,//0
                                        ComId,  //1
                                        AppointmentDateQuery,//2
                                        tagQuery//3
                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetInventoryTechOrderReportForDownload(Guid ComId, DateTime? start, DateTime? end, string searchtext, string tag)
        {
            string AppointmentDateQuery = "";

            string searchquery = "";
            if (!string.IsNullOrEmpty(searchtext))
            {
                searchquery = string.Format("and (emp.FirstName+' '+emp.LastName like '%{0}%' )", searchtext);
            }

            string tagQuery = string.Format("and pg.Tag like '%{0}%'", tag);
            #region AppointmentDateQuery
            if (start != null && start != new DateTime() && end != null && end != new DateTime())
            {
                var StartDate = start.Value.SetZeroHour();
                var EndDate = end.Value.SetMaxHour();
                AppointmentDateQuery = string.Format("and emp.CreatedDate between '{0}' and '{1}'", StartDate, EndDate);
            }
            #endregion

            string sqlQuery = @"                            
                                select emp.UserId,emp.FirstName + ' ' + emp.LastName as EmpName 
                                into #tempEmp 
                                from Employee emp 
                                left join UserCompany uc
	                                on emp.UserId = uc.UserId
                                left join UserPermission up 
	                                on up.UserId = emp.UserId
                                left join PermissionGroup pg
	                                on pg.Id = up.PermissionGroupId
                                left join UserLogin ul 
	                                on emp.UserId = ul.UserId
                                where up.PermissionGroupId is not null 
                                and up.PermissionGroupId > 0
                                --and ul.IsActive = 1
                                and ul.IsDeleted = 0
                                and uc.CompanyId = '{1}'
                                and emp.IsActive = 1
                                and emp.IsDeleted = 0
                                {0}
                                {2}
                                {3}
								order by EmpName
                                 
                                select EquipmentId, TechnicianId
                                into #tempTable
                                from InventoryTech
                                Group by EquipmentId, TechnicianId

                                 DECLARE
                                 @columns NVARCHAR(MAX) = '',
                                 @sql NVARCHAR(MAX) = '';

                                -- select the category names
                                 SELECT
                                 @columns+=QUOTENAME(eqp.Name + '(' + convert(nvarchar(MAX), eqp.Id) + ')') + ','
                                 FROM
                                 Equipment eqp where eqp.EquipmentClassId=1
                                 ORDER BY
                                 Name;

                            -- remove the last comma
                                SET @columns = LEFT(@columns, LEN(@columns) - 1);

                        -- construct dynamic SQL
                               SET @sql ='
                               SELECT * into 
                               ##resulttechorder FROM
                               (
	                           SELECT
	                           (eqp.Name + ''('' + convert(nvarchar(MAX), eqp.Id) + '')'') as Name,
	                           (EmpName) as [Technician Name],
                               UserId,
	                           (select ISNULL(sum(caeTemp.Quantity),0) from CustomerAppointmentEquipment caeTemp 
                               left join TicketUser _tu on _tu.TiketId=caeTemp.AppointmentId 
                               left join Ticket _tic on _tic.TicketId=_tu.TiketId 
                               where caeTemp.EquipmentId=eqp.EquipmentId and _tu.UserId =emp.UserId ) as Ordered
	                           FROM
		                       #tempEmp emp

		                       left join #tempTable it on it.TechnicianId=emp.UserId
		                       left join Equipment eqp on eqp.EquipmentId=it.EquipmentId
		                       where eqp.EquipmentClassId=1
		 
                               ) t
                               PIVOT(
	                             sum(Ordered)
	                           FOR [Name] IN ('+ @columns +')
                                ) AS pivot_table;';

                           -- execute the dynamic SQL
                              EXECUTE sp_executesql @sql

                              Declare @userid uniqueidentifier
							  Declare @technane nvarchar(500)

							  While (Select Count(*)
                              From #tempEmp where UserId NOT IN (select UserId from ##resulttechorder)) > 0
                              Begin
							  

                              Select Top 1 @userid = UserId, @technane = EmpName From #tempEmp where UserId NOT IN (select UserId from ##resulttechorder)

                              INSERT INTO ##resulttechorder ([Technician Name], [UserId]) VALUES (@technane ,@userid )

                              Delete FROM #tempEmp Where UserId = @userid

                              End 
							  
							  DECLARE db_cursor CURSOR FOR select [Name],Id from Equipment where EquipmentClassId =1
							DECLARE @Name VARCHAR(256);
							DECLARE @Id int;
							OPEN db_cursor;
							FETCH NEXT FROM db_cursor INTO @Name,@Id ;
							WHILE @@FETCH_STATUS = 0  
							BEGIN  
									set @Name = '['+@Name + '('+CAST(@Id AS nvarchar(50))+')]';
									print @Name
									set @sql = 'update ##resulttechorder set '+@Name +'= 0 where '+@Name+' is null;';
									EXECUTE sp_executesql @sql

									FETCH NEXT FROM db_cursor INTO @Name,@Id;
							END;
							CLOSE db_cursor;
							DEALLOCATE db_cursor;
							  
						    select * from ##resulttechorder order by [Technician Name]

                              drop table #tempTable
                              drop table #tempEmp";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        searchquery,//0
                                        ComId,  //1
                                        AppointmentDateQuery,//2
                                        tagQuery//3
                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetEmployeeByCompanyIdAndPerGrpIdAPI(Guid companyId, string tag, Guid userid, string PerGrpId)

        {

            string sqlQuery = @"
                                select emp.*, emp.Street+' '+emp.ZipCode+' '+emp.City+', '+emp.State as [Address] 
                                ,iif((select COUNT(*) from Employee _emp where _emp.UserId = emp.UserId and _emp.UserId = '{3}') > 0, 1, 0) as SelectedUser
                                from Employee emp 
                                left join UserCompany uc
	                                on emp.UserId = uc.UserId
                                left join UserPermission up 
	                                on up.UserId = emp.UserId
                                left join PermissionGroup pg
	                                on pg.Id = up.PermissionGroupId
                                left join UserLogin ul 
	                                on emp.UserId = ul.UserId
                                where up.PermissionGroupId is not null 
                                and up.PermissionGroupId > 0
                                --and ul.IsActive = 1
                                and ul.IsDeleted = 0
                                and uc.CompanyId = '{0}'
                                and emp.IsActive = 1
                                and emp.IsDeleted = 0
                                and emp.Recruited = 1 
                                and emp.IsCurrentEmployee = 1
                                {1}
                                {2}
                                
                                ";
            string tagQuery = string.Format("and pg.Tag like '%{0}%'", tag);
            if (!string.IsNullOrEmpty(PerGrpId))
            {
                tagQuery = string.Format("and pg.Id in ({0})", PerGrpId);
            }
            string subquery = "";
            //if (userid != null && userid != new Guid())
            //{
            //    subquery = string.Format("and emp.UserId = '{0}'", userid);
            //}
            sqlQuery = string.Format(sqlQuery, companyId, tagQuery, subquery, userid);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetEmployeeByCompanyIdAndTagAndTechnician(Guid companyId, string tag, Guid userid)
        {
            string sqlQuery = @"
                                select emp.* from Employee emp 
                                left join UserCompany uc
	                                on emp.UserId = uc.UserId
                                left join UserPermission up 
	                                on up.UserId = emp.UserId
                                left join PermissionGroup pg
	                                on pg.Id = up.PermissionGroupId
                                left join UserLogin ul 
	                                on emp.UserId = ul.UserId
                                where up.PermissionGroupId is not null 
                                and up.PermissionGroupId > 0
                                and pg.Tag like '%{1}%'
                                and ul.IsActive = 1
                                and ul.IsDeleted = 0
                                and uc.CompanyId = '{0}'
                                and emp.IsActive = 1
                                and emp.IsDeleted = 0
                                and emp.IsCurrentEmployee = 1
                                {2}
                                ";
            string subquery = "";
            if (userid != null && userid != new Guid())
            {
                subquery = string.Format("and emp.UserId != '{0}'", userid);
            }
            sqlQuery = string.Format(sqlQuery, companyId, tag, subquery);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Employee> GetALLEmployeeByCompanyIdAndIsRecruted(Guid companyId)
        {
            string sqlQuery = @"select emp.* from Employee emp 
                                left join UserCompany uc
	                                on emp.UserId = uc.UserId
                                left join UserLogin ul 
	                                on emp.UserId = ul.UserId
                                where ul.IsActive = 1
                                and ul.IsDeleted = 0
                                and uc.CompanyId = '{0}'
								and emp.Recruited = 1
                                and emp.IsActive = 'true'
                                and emp.IsDeleted = 'false'";
            sqlQuery = string.Format(sqlQuery, companyId);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    return GetList(cmd, -1);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllInstallerEmployeeByCompnayId(Guid companyId)
        {
            string sqlQuery = @"select emp.UserId, emp.FirstName, emp.LastName
                                from 
                                 Employee emp 
                                 left join UserPermission up
                                     on emp.UserId = up.UserId
                                 left join PermissionGroup pg
                                     on up.PermissionGroupId = pg.Id
                                 left join UserCompany uc
                                     on uc.UserId = up.UserId
                                    left join UserLogin ul 
                                on ul.UserId = emp.UserId
                                where 
                                 uc.CompanyId = '{0}'
                                 and pg.Name is not null
                                 and pg.Name = 'Technician'
                                    and ul.IsActive = 'true'
                                and ul.IsDeleted = 'false'
                                and emp.IsActive = 'true'
                                and emp.IsDeleted = 'false'";
            sqlQuery = string.Format(sqlQuery, companyId);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //[Shariful-20-9-19]
        public DataTable GetEmployeeByPartnerId(Guid SupervisorId)
        {
            string sqlQuery = @"select *  into #Employee  from Employee 
                                update #Employee set SuperVisorId = '00000000-0000-0000-0000-000000000000' 
                                where SuperVisorId = '-1' or SuperVisorId = '' or SuperVisorId is null

                                DECLARE @UserId uniqueidentifier = '{0}'
                                ;WITH cte AS 
                                (
	                                SELECT a.UserId,   SupervisorId, a.FirstName,a.LastName
	                                FROM #Employee a
	                                WHERE UserId = @UserId
	                                UNION ALL
	                                SELECT a.UserId, a.SupervisorId, a.FirstName,a.LastName
	                                FROM #Employee a JOIN cte c ON convert(uniqueidentifier,a.SuperVisorId)  = c.UserId
                                )
                                SELECT SupervisorId, UserId, FirstName,LastName
                                FROM cte
                                option (maxrecursion 0) 

                                drop table #Employee";

            sqlQuery = string.Format(sqlQuery, SupervisorId);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //[~Shariful-20-9-19]

        public DataTable GetEmployeeRoleByEmployeeIdAndCompanyId(Guid EmployeeId, Guid CompanyId)
        {
            string sqlQuery = @"select _pg.Name, _pg.Tag
                                from 
	                                Employee _emp 
	                                left join UserPermission _up
	                                on _emp.UserId = _up.UserId
	                                left join PermissionGroup _pg
	                                on _up.PermissionGroupId = _pg.Id
	                                left join UserCompany _uc
	                                on _uc.UserId = _up.UserId
                                where 
	                                _emp.UserId = '{0}' 
	                                and _uc.CompanyId = '{1}'";

            sqlQuery = string.Format(sqlQuery, EmployeeId, CompanyId);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool DeleteEmployeeByUserId(Guid UserId)
        {
            string sqlQuery = @"delete from Employee where UserId = '{0}'";
            sqlQuery = string.Format(sqlQuery, UserId);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool DeleteEmployeeByUsername(string username)
        {
            string sqlQuery = @"delete from Employee where UserName = '{0}'";
            sqlQuery = string.Format(sqlQuery, username);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public DataTable CheckTechnicianIsInCompany(Guid EmployeeId, Guid CompanyId)
        {
            string sqlQuery = @"select 
                                    _emp.*
                                from 
                                    Employee _emp 
	                                left join UserCompany _uc
		                                on _emp.UserId = _uc.UserId
		                        where 
                                    _emp.UserId = '{0}'
                                    and _uc.CompanyId = '{1}'
		                            and _uc.IsDefault = 'true'
		                            and _emp.IsActive = 'true'";

            sqlQuery = string.Format(sqlQuery, EmployeeId, CompanyId);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllPayrollReport(Guid CompanyId, DateTime StartDate, DateTime EndDate)
        {

            string DateValueSet = "";
            string DateCheckForEmpComm = "";
            string DateCheckFroTimeClock = "";

            string sqlQuery = @"Declare @StartDate datetime
                                Declare @EndDate datetime
                                {0}
                                select emp.UserId,
                                emp.FirstName,
                                emp.LastName, 
                                IsNULL((select SUM(amount) from EmployeeCommission 
	                                where UserId = emp.UserId 
		                                {1} ),0) as TotalCommission,

                                IsNULL((select  round((sum(Cast(ClockedInMinutes as float))/3600),2 )  from TimeClock 
	                                where UserId = emp.UserId 
		                                {2}),0)  as [RegularHours],
                                ISNULL(emp.HourlyRate,0) as HourlyRate
                                from Employee emp
                                ";
            string subquery = "";
            if (StartDate != new DateTime() && EndDate != new DateTime())
            {
                DateValueSet = string.Format(@"set @StartDate = '{0}'; set @EndDate = '{1}'; ", StartDate.ToString("yyyy-MM-dd HH:mm:ss"), EndDate.ToString("yyyy-MM-dd HH:mm:ss"));
                DateCheckForEmpComm = " and CreatedDate between @StartDate and @EndDate ";
                DateCheckFroTimeClock = " and [Time] between @StartDate and @EndDate";
            }
            sqlQuery = string.Format(sqlQuery, DateValueSet, DateCheckForEmpComm, DateCheckFroTimeClock);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataSet GetAllEmpPayrollReport(Guid SupervisorId, DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize)
        {
            string DateFilter2 = "";
            string DateFilter1 = "";
            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime())
            {

                FilterStartDate = FilterStartDate.SetZeroHour().ClientToUTCTime();
                FilterEndDate = FilterEndDate.SetMaxHour().ClientToUTCTime();

                DateFilter2 = string.Format("and [ClockInTime] between '{0}' and '{1}'"
                    , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
                DateFilter1 = string.Format("and StartDate between '{0}' and '{1}' or EndDate between '{0}' and '{1}'"
                    , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            string sqlQuery = @"
                                declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                select emp.Id,emp.UserId, emp.FirstName,emp.LastName,emp.HourlyRate,
                                (select  round((sum(Cast(ClockedInSeconds as float))/3600),2 )  from EmployeeTimeClock where UserId = emp.UserId {0})  as [RegularHours],
                                (select ( round((sum(Cast(ClockedInSeconds as float))/3600),2 ) - 40 )  from EmployeeTimeClock where UserId = emp.UserId {0}) as [OTOHours],
                                (select round((sum(Cast(Minute as float))/60),2 )  from Pto where UserId = emp.UserId {4}) as [PTOHours]
                                into #emp
                                from Employee emp
                                left join EmployeeTimeClock tm on tm.UserId = emp.UserId
                                LEFT JOIN PTO pt on pt.UserId=emp.UserId and pt.Status='accepted' 
                                where emp.IsPayroll =1 	group by emp.FirstName,emp.LastName,emp.UserId, emp.Id,emp.HourlyRate

                                select * into #empfilter
								from #emp

								select top(@pagesize)
								* from #empfilter
								where Id not in(select top(@pagestart) Id from #emp #e {2})
                                {3}
								select count(*) CountTotal
                                from #empfilter
                                
								drop table #emp
								drop table #empfilter";

            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(order) && order != "undefined" && order != "null")
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
                sqlQuery = string.Format(sqlQuery, DateFilter2, order, subquery, subquery1, DateFilter1);
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
        public DataSet GetPayrollBrinksBySalesPersonId(DateTime FilterStartDate, DateTime FilterEndDate, int pageno, int pagesize, string SearchText, Guid UserId, string PayrollBrinksStatus, string PayrollBrinksFunding)
        {
            string SearchQuery = "";
            string DateFilter1 = "";
            string StatusQuery = "";
            string FundingFilterQuery = "";
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }
            if (!string.IsNullOrEmpty(SearchText))
            {
                SearchQuery = string.Format(" and (emp.FirstName like '%{0}%' or emp.LastName like '%{0}%' or emp.FirstName +' '+emp.LastName like '%{0}%')", SearchText);
            }
            if (!string.IsNullOrEmpty(PayrollBrinksStatus))
            {
                StatusQuery = string.Format(" and pb.FundingStatus in ({0})", PayrollBrinksStatus);
            }
            if (!string.IsNullOrEmpty(PayrollBrinksFunding) && PayrollBrinksFunding != "'null'")
            {
                if (PayrollBrinksFunding == "Funded")
                {
                    FundingFilterQuery = " and pb.IsPaid=1";
                }
                else if (PayrollBrinksFunding == "NotFunded")
                {
                    FundingFilterQuery = " and (pb.IsPaid Is NULL or pb.IsPaid=0)";
                }

            }
            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime())
            {
                FilterStartDate = FilterStartDate.SetZeroHour();
                FilterEndDate = FilterEndDate.SetMaxHour();

                DateFilter1 = string.Format("and cus.InstallDate between '{0}' and '{1}'"
                    , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            string sqlQuery = @"declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                select * into #SaleCom from (
								select
                                pb.Id,
                                cus.Id as CustomerIntId,
								pb.SalesPersonId,
								ISNULL(NULLIF(cus.CustomerNo,''),0) as AccountId,
                                {0} as CustomerName,
                                cusExt.BrinksFundingStatus,
								cusExt.FinanceFundingStatus,
								CASE 
								WHEN CHARINDEX('ACH',ppc.Type) > 0 THEN 'ACH'
								WHEN CHARINDEX('CC',ppc.Type) > 0 THEN 'CC'
								WHEN CHARINDEX('Invoice',ppc.Type) > 0 THEN 'Invoice'
								ELSE ''
								END as PaymentMethodForRMR,
								cus.CreditScoreValue,
								cus.Type,
								lp.DisplayText as ContractTerm,
								CASE 
								WHEN cusExt.IsFinanced=1 THEN 'Finance'
								ELSE 'Traditional'
								END as PayType,
								ISNULL(pb.MMR,0) as MMR,
								ISNULL(cusExt.MonthlyFinanceRate,0) as FinanceMonthlyPayment,
								ISNULL(pb.MMR+cusExt.MonthlyFinanceRate,0) as TotalMonthly,
								pb.Multiple,
								pb.GrossPay,
								ISNULL(Pb.HoldBack,0) as HoldBack,
								ISNULL(pb.Deductions,0) as Deductions,
								ISNULL(pb.Adjustments,0) as Adjustments,
								ISNULL(pb.NetPay,0) as NetPay,
                                pb.FundingStatus,
                                (SELECT STRING_AGG(eqp.Name, ', ') FROM CustomerAppointmentEquipment cae
								LEFT JOIN Equipment eqp on eqp.EquipmentId=cae.EquipmentId
								where cae.IsService != 1 and cae.AppointmentId=pb.TicketId
								) as EquipmentList,
								(ISNULL((select
								ppt.Amount
								from CustomerExtended ce
								LEFT JOIN PayrollPassThrus ppt on ppt.PassThrus=ce.AlarmBasicPackage
								Where ppt.TermSheetId=emp.TermSheetId AND ce.CustomerId=cus.CustomerId),0) +
								ISNULL((select SUM(ppt.aMount) Total from AlarmCustomerSelectedAddon ad
								left join  PayrollPassThrus ppt on ppt.PassThrus = ad.AddonType
								where ppt.TermSheetId=emp.TermSheetId AND ad.CustomerId=cus.CustomerId),0)
								) as PassThrus,
                                csg.Grade as CreditGrade
								From PayrollBrinks pb
								LEFT JOIN Customer cus on cus.CustomerId=pb.CustomerId
								LEFT JOIN CustomerExtended cusExt on cusExt.CustomerId=pb.CustomerId
								LEFT JOIN Employee emp on emp.UserId=pb.SalesPersonId
                                LEFT JOIN Ticket tk on tk.TicketId=pb.TicketId

								LEFT JOIN PaymentInfoCustomer pic on pic.CustomerId=cus.CustomerId and pic.Payfor='MMR'
								LEFT JOIN PaymentProfileCustomer ppc on ppc.PaymentInfoId=pic.PaymentInfoId

								LEFT JOIN Lookup lp on lp.DataValue=cus.ContractTeam and lp.DataKey='ContractTerm' and lp.DataValue!='-1'
                                LEFT JOIN CreditScoreGrade csg on cus.CreditScore between csg.MinScore and csg.MaxScore                                
                                where pb.NetPay>0 and tk.Status='Completed' and pb.SalesPersonId='{1}' {2} {3} {4} {5}
								) d

								select top(@pagesize)
								* into #10SaleCom from #SaleCom
								where Id not in(select top(@pagestart) Id from #SaleCom)

								select * from #10SaleCom

								select count(*) CountTotal
                                from #SaleCom

								select 
								SUM(MMR) as TotalTotalRMR,
								SUM(GrossPay) as TotalGrossPay,
								SUM(Deductions) as TotalDeductions,
								SUM(Adjustments) as TotalAdjustments,
								SUM(NetPay) as TotalNetPay
								from #10SaleCom

								drop table #SaleCom
                                drop table #10SaleCom";

            try
            {
                sqlQuery = string.Format(sqlQuery, NameSql, UserId, DateFilter1, StatusQuery, FundingFilterQuery, SearchQuery);
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
        public DataSet DownLoadPayrollBrinksBySalesPersonId(DateTime FilterStartDate, DateTime FilterEndDate, int pageno, int pagesize, string SearchText, Guid UserId, string PayrollBrinksStatus, string PayrollBrinksFunding)
        {
            string DateFilter1 = "";
            string SearchQuery = "";
            string FilterQuery = "";
            string StatusFilterQuery = "";
            string FundingFilterQuery = "";

            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }

            if (!string.IsNullOrEmpty(SearchText))
            {
                SearchQuery = string.Format(" and (emp.FirstName like '%{0}%' or emp.LastName like '%{0}%' or emp.FirstName +' '+emp.LastName like '%{0}%')", SearchText);
            }
            if (!string.IsNullOrEmpty(PayrollBrinksStatus) && PayrollBrinksStatus != "'null'")
            {
                StatusFilterQuery = string.Format(" and pb.FundingStatus in ({0})", PayrollBrinksStatus);
            }
            if (!string.IsNullOrEmpty(PayrollBrinksFunding) && PayrollBrinksFunding != "'null'")
            {
                if (PayrollBrinksFunding == "Funded")
                {
                    FundingFilterQuery = " and pb.IsPaid=1";
                }
                else if (PayrollBrinksFunding == "NotFunded")
                {
                    FundingFilterQuery = " and (pb.IsPaid Is NULL or pb.IsPaid=0)";
                }

            }
            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime())
            {

                FilterStartDate = FilterStartDate.SetZeroHour();
                FilterEndDate = FilterEndDate.SetMaxHour();


                DateFilter1 = string.Format("and cus.InstallDate between '{0}' and '{1}'"
                    , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            string sqlQuery = @"select
								ISNULL(NULLIF(cus.CustomerNo,''),0) as AccountId,
                                {0} as CustomerName,
                                cusExt.BrinksFundingStatus,
								cusExt.FinanceFundingStatus,
								CASE 
								WHEN CHARINDEX('ACH',ppc.Type) > 0 THEN 'ACH'
								WHEN CHARINDEX('CC',ppc.Type) > 0 THEN 'CC'
								WHEN CHARINDEX('Invoice',ppc.Type) > 0 THEN 'Invoice'
								ELSE ''
								END as PaymentMethodForRMR,
								cus.CreditScoreValue,
								cus.Type,
								lp.DisplayText as ContractTerm,
								CASE 
								WHEN cusExt.IsFinanced=1 THEN 'Finance'
								ELSE 'Traditional'
								END as PayType,
								Format(ISNULL(pb.MMR,0),'N2') as TotalRMR,
								Format(ISNULL(cusExt.MonthlyFinanceRate,0),'N2') as FinanceMonthlyPayment,
								Format(ISNULL(pb.MMR+cusExt.MonthlyFinanceRate,0),'N2') as TotalMonthly,
								Format(ISNULL(pb.Multiple,0),'N2') as Multiple,
								Format(ISNULL(pb.GrossPay,0),'N2') as GrossPay,
								Format(ISNULL(Pb.HoldBack,0),'N2') as HoldBack,
								Format(ISNULL(pb.Deductions,0),'N2') as Deductions,
								Format(ISNULL(pb.Adjustments,0),'N2') as Adjustments,
								Format(ISNULL(pb.NetPay,0),'N2') as NetPay
								From PayrollBrinks pb
								LEFT JOIN Customer cus on cus.CustomerId=pb.CustomerId
								LEFT JOIN CustomerExtended cusExt on cusExt.CustomerId=pb.CustomerId
								LEFT JOIN Employee emp on emp.UserId=pb.SalesPersonId
                                LEFT JOIN Ticket tk on tk.TicketId=pb.TicketId

								LEFT JOIN PaymentInfoCustomer pic on pic.CustomerId=cus.CustomerId and pic.Payfor='MMR'
								LEFT JOIN PaymentProfileCustomer ppc on ppc.PaymentInfoId=pic.PaymentInfoId

								LEFT JOIN Lookup lp on lp.DataValue=cus.ContractTeam and lp.DataKey='ContractTerm' and lp.DataValue!='-1'
                                where pb.NetPay>0 and tk.Status='Completed' and pb.SalesPersonId='{1}' {2} {3} {4} {5} {6}";

            try
            {
                sqlQuery = string.Format(sqlQuery, NameSql, UserId, SearchQuery, DateFilter1, FilterQuery, StatusFilterQuery, FundingFilterQuery);
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
        public DataTable GetSalesPayCalculationByTicketId(Guid ticketId)
        {
            string sqlQuery = @"Select
                                emp.TermSheetId,
								ISNULL(pbm.Amount,0) +
								CASE 
	                            WHEN CHARINDEX('ACH',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='ACH')
	                            WHEN CHARINDEX('CC',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='CC')
	                            WHEN CHARINDEX('Invoice',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='Invoice')
                                ELSE 0
								END +
								ISNULL(pmpb.Point,0) +
								ISNULL(pcr.Point,0) +
								ISNULL(pct.Point,0) +
								ISNULL(pal.Point,0) as TotalMultiple,

								ISNULL((ISNULL((select 
								SUM(eqp.RepCost*cae.Quantity)
								from CustomerAppointmentEquipment cae
								LEFT JOIN Equipment eqp on eqp.EquipmentId=cae.EquipmentId
								LEFT JOIN Ticket tk on tk.TicketId=cae.AppointmentId
								where tk.TicketId='{0}' and cae.IsService=0),0) -
								ISNULL((Select SUM(inv.TotalAmount)
								from Invoice inv
								where inv.CustomerId=cus.CustomerId 
								AND (inv.InvoiceFor='ActivationNonConforming' Or inv.InvoiceFor='Service' Or inv.InvoiceFor='Equipment')
								AND inv.Status='Paid'),0)) 
								+
								ISNULL((Select Amount from PayrollInstallationFee pif where 
								(select SUM(eqp.RepCost*cae.Quantity)
								from CustomerAppointmentEquipment cae
								LEFT JOIN Equipment eqp on eqp.EquipmentId=cae.EquipmentId
								LEFT JOIN Ticket tk on tk.TicketId=cae.AppointmentId
								where tk.TicketId='{0}' and cae.IsService=0)
								Between pif.InstallationFeeMin and pif.InstallationFeeMax
								and pif.TermSheetId=emp.TermSheetId
								),0)
								+
								Case 
								when phb.Type='Percentage' THEN
								(cus.MonthlyMonitoringFee * (ISNULL(pbm.Amount,0) +
								CASE 
	                            WHEN CHARINDEX('ACH',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='ACH')
	                            WHEN CHARINDEX('CC',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='CC')
	                            WHEN CHARINDEX('Invoice',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='Invoice')
                                ELSE 0
								END +
								ISNULL(pmpb.Point,0) +
								ISNULL(pcr.Point,0) +
								ISNULL(pct.Point,0) +
								ISNULL(pal.Point,0)) * phb.Percentage)/100
								when phb.Type='Multiple' THEN
								cus.MonthlyMonitoringFee * (ISNULL(pbm.Amount,0) +
								CASE 
	                            WHEN CHARINDEX('ACH',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='ACH')
	                            WHEN CHARINDEX('CC',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='CC')
	                            WHEN CHARINDEX('Invoice',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='Invoice')
                                ELSE 0
								END +
								ISNULL(pmpb.Point,0) +
								ISNULL(pcr.Point,0) +
								ISNULL(pct.Point,0) +
								ISNULL(pal.Point,0)) * phb.Percentage
								ELSE 0
								END 
								+
								CASE
								WHEN cus.ContractTeam='Month to Month'
								THEN
									Case 
									when phbmtm.Type='Percentage' THEN
									(cus.MonthlyMonitoringFee * (ISNULL(pbm.Amount,0) +
									CASE 
									WHEN CHARINDEX('ACH',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='ACH')
									WHEN CHARINDEX('CC',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='CC')
									WHEN CHARINDEX('Invoice',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='Invoice')
									ELSE 0
									END +
									ISNULL(pmpb.Point,0) +
									ISNULL(pcr.Point,0) +
									ISNULL(pct.Point,0) +
									ISNULL(pal.Point,0)) * phbmtm.Percentage)/100
									when phb.Type='Multiple' THEN
									cus.MonthlyMonitoringFee * (ISNULL(pbm.Amount,0) +
									CASE 
									WHEN CHARINDEX('ACH',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='ACH')
									WHEN CHARINDEX('CC',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='CC')
									WHEN CHARINDEX('Invoice',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='Invoice')
									ELSE 0
									END +
									ISNULL(pmpb.Point,0) +
									ISNULL(pcr.Point,0) +
									ISNULL(pct.Point,0) +
									ISNULL(pal.Point,0)) * phbmtm.Percentage
									ELSE 0
									END
								WHEN cus.ContractTeam!='Month to Month'
								THEN
									Case 
									when phbaot.Type='Percentage' THEN
									(cus.MonthlyMonitoringFee * (ISNULL(pbm.Amount,0) +
									CASE 
									WHEN CHARINDEX('ACH',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='ACH')
									WHEN CHARINDEX('CC',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='CC')
									WHEN CHARINDEX('Invoice',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='Invoice')
									ELSE 0
									END +
									ISNULL(pmpb.Point,0) +
									ISNULL(pcr.Point,0) +
									ISNULL(pct.Point,0) +
									ISNULL(pal.Point,0)) * phbaot.Percentage)/100
									when phb.Type='Multiple' THEN
									cus.MonthlyMonitoringFee * (ISNULL(pbm.Amount,0) +
									CASE 
									WHEN CHARINDEX('ACH',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='ACH')
									WHEN CHARINDEX('CC',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='CC')
									WHEN CHARINDEX('Invoice',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='Invoice')
									ELSE 0
									END +
									ISNULL(pmpb.Point,0) +
									ISNULL(pcr.Point,0) +
									ISNULL(pct.Point,0) +
									ISNULL(pal.Point,0)) * phbaot.Percentage
									ELSE 0
									END
								ELSE 0
								END,0)
								AS Deductions,
                                (ISNULL((select
								ppt.Amount
								from CustomerExtended ce
								LEFT JOIN PayrollPassThrus ppt on ppt.PassThrus=ce.AlarmBasicPackage
								Where ppt.TermSheetId=emp.TermSheetId AND ce.CustomerId=cus.CustomerId),0) +
								ISNULL((select SUM(ppt.aMount) Total from AlarmCustomerSelectedAddon ad
								left join  PayrollPassThrus ppt on ppt.PassThrus = ad.AddonType
								where ppt.TermSheetId=emp.TermSheetId AND ad.CustomerId=cus.CustomerId),0)
								) as PassThrus,

								ISNULL(
								Case 
								when phb.Type='Percentage' THEN
								(cus.MonthlyMonitoringFee * (ISNULL(pbm.Amount,0) +
								CASE 
	                            WHEN CHARINDEX('ACH',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='ACH')
	                            WHEN CHARINDEX('CC',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='CC')
	                            WHEN CHARINDEX('Invoice',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='Invoice')
                                ELSE 0
								END 
								+
								ISNULL(pmpb.Point,0) +
								ISNULL(pcr.Point,0) +
								ISNULL(pct.Point,0) +
								ISNULL(pal.Point,0)) * phb.Percentage)/100
								when phb.Type='Multiple' THEN
								cus.MonthlyMonitoringFee * (ISNULL(pbm.Amount,0) +
								CASE 
	                            WHEN CHARINDEX('ACH',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='ACH')
	                            WHEN CHARINDEX('CC',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='CC')
	                            WHEN CHARINDEX('Invoice',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='Invoice')
                                ELSE 0
								END +
								ISNULL(pmpb.Point,0) +
								ISNULL(pcr.Point,0) +
								ISNULL(pct.Point,0) +
								ISNULL(pal.Point,0)) * phb.Percentage
								ELSE 0
								END 
								+
								CASE
								WHEN cus.ContractTeam='Month to Month'
								THEN
									Case 
									when phbmtm.Type='Percentage' THEN
									(cus.MonthlyMonitoringFee * (ISNULL(pbm.Amount,0) +
									CASE 
									WHEN CHARINDEX('ACH',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='ACH')
									WHEN CHARINDEX('CC',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='CC')
									WHEN CHARINDEX('Invoice',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='Invoice')
									ELSE 0
									END +
									ISNULL(pmpb.Point,0) +
									ISNULL(pcr.Point,0) +
									ISNULL(pct.Point,0) +
									ISNULL(pal.Point,0)) * phbmtm.Percentage)/100
									when phb.Type='Multiple' THEN
									cus.MonthlyMonitoringFee * (ISNULL(pbm.Amount,0) +
									CASE 
									WHEN CHARINDEX('ACH',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='ACH')
									WHEN CHARINDEX('CC',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='CC')
									WHEN CHARINDEX('Invoice',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='Invoice')
									ELSE 0
									END +
									ISNULL(pmpb.Point,0) +
									ISNULL(pcr.Point,0) +
									ISNULL(pct.Point,0) +
									ISNULL(pal.Point,0)) * phbmtm.Percentage
									ELSE 0
									END
								WHEN cus.ContractTeam!='Month to Month'
								THEN
									Case 
									when phbaot.Type='Percentage' THEN
									(cus.MonthlyMonitoringFee * (ISNULL(pbm.Amount,0) +
									CASE 
									WHEN CHARINDEX('ACH',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='ACH')
									WHEN CHARINDEX('CC',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='CC')
									WHEN CHARINDEX('Invoice',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='Invoice')
									ELSE 0
									END +
									ISNULL(pmpb.Point,0) +
									ISNULL(pcr.Point,0) +
									ISNULL(pct.Point,0) +
									ISNULL(pal.Point,0)) * phbaot.Percentage)/100
									when phb.Type='Multiple' THEN
									cus.MonthlyMonitoringFee * (ISNULL(pbm.Amount,0) +
									CASE 
									WHEN CHARINDEX('ACH',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='ACH')
									WHEN CHARINDEX('CC',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='CC')
									WHEN CHARINDEX('Invoice',ppc.Type) > 0 THEN (Select Point from PayrollCustomerBillingMethod where TermSheetId=emp.TermSheetId and BillingMethod='Invoice')
									ELSE 0
									END +
									ISNULL(pmpb.Point,0) +
									ISNULL(pcr.Point,0) +
									ISNULL(pct.Point,0) +
									ISNULL(pal.Point,0)) * phbaot.Percentage
									ELSE 0
									END
								ELSE 0
								END,0)
								AS HoldBack
                                From Ticket tk
                                LEFT JOIN Customer cus on cus.CustomerId=tk.CustomerId
                                LEFT JOIN Employee emp on emp.UserId=cus.Soldby
								LEFT JOIN PayrollBaseMultiple pbm on pbm.TermSheetId=emp.TermSheetId

								LEFT JOIN PaymentInfoCustomer pic on pic.CustomerId=cus.CustomerId and pic.Payfor='MMR'
								LEFT JOIN PaymentProfileCustomer ppc on ppc.PaymentInfoId=pic.PaymentInfoId

								LEFT JOIN PayrollMonthlyProductionBonus pmpb on pmpb.TermSheetId=emp.TermSheetId and
								(Select COUNT(Id) from Customer cusinner where cusinner.Soldby1=emp.UserId) Between pmpb.MonthlyProductionBonusMin and pmpb.MonthlyProductionBonusMax

								LEFT JOIN PayrollCreditRating pcr on pcr.TermSheetId=emp.TermSheetId and
								cus.CreditScoreValue Between pcr.MinCredit and pcr.MaxCredit

								LEFT JOIN PayrollCustomerType pct on pct.TermSheetId=emp.TermSheetId and pct.CustomerType=cus.Type

								LEFT JOIN PayrollAgreementLength pal on pal.TermSheetId=emp.TermSheetId and pal.AgreementLength=cus.ContractTeam

								LEFT JOIN PayrollHoldBack phb on phb.TermSheetId=emp.TermSheetId and phb.HoldBack='ServiceFee'
								LEFT JOIN PayrollHoldBack phbmtm on phbmtm.TermSheetId=emp.TermSheetId and phbmtm.HoldBack='MonthToMonth'
								LEFT JOIN PayrollHoldBack phbaot on phbaot.TermSheetId=emp.TermSheetId and phb.HoldBack='AllotherTerms'

                                where tk.TicketId='{0}'";

            try
            {
                sqlQuery = string.Format(sqlQuery, ticketId);
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
        public DataSet GetDedudctionDetailsByPayrollBrinksId(int PayrollBrinksId)
        {
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }

            string sqlQuery = @"Select
			pb.Id,
			pb.Deductions,
            cus.Id as CustomerIntId,
			{0} as CustomerName,
			(ISNULL((select 
			SUM(eqp.RepCost*cae.Quantity)
			from CustomerAppointmentEquipment cae
			LEFT JOIN Equipment eqp on eqp.EquipmentId=cae.EquipmentId
			LEFT JOIN Ticket tk on tk.TicketId=cae.AppointmentId
			where tk.TicketId=pb.TicketId and cae.IsService=0),0)) as EquipmentRepCost,

			(ISNULL((Select SUM(inv.TotalAmount)
			from Invoice inv
			where inv.CustomerId=cus.CustomerId 
			AND (inv.InvoiceFor='ActivationNonConforming' Or inv.InvoiceFor='Service' Or inv.InvoiceFor='Equipment')
			AND inv.Status='Paid'),0)) as UpFrontCollect,

			(ISNULL((select 
			SUM(eqp.RepCost*cae.Quantity)
			from CustomerAppointmentEquipment cae
			LEFT JOIN Equipment eqp on eqp.EquipmentId=cae.EquipmentId
			LEFT JOIN Ticket tk on tk.TicketId=cae.AppointmentId
			where tk.TicketId=pb.TicketId and cae.IsService=0),0) -
			ISNULL((Select SUM(inv.TotalAmount)
			from Invoice inv
			where inv.CustomerId=cus.CustomerId 
			AND (inv.InvoiceFor='ActivationNonConforming' Or inv.InvoiceFor='Service' Or inv.InvoiceFor='Equipment')
			AND inv.Status='Paid'),0)) as EquipmentAdjustment,

			ISNULL((Select Amount from PayrollInstallationFee pif where 
			(select SUM(eqp.RepCost*cae.Quantity)
			from CustomerAppointmentEquipment cae
			LEFT JOIN Equipment eqp on eqp.EquipmentId=cae.EquipmentId
			LEFT JOIN Ticket tk on tk.TicketId=cae.AppointmentId
			where tk.TicketId=pb.TicketId and cae.IsService=0)
			Between pif.InstallationFeeMin and pif.InstallationFeeMax
			and pif.TermSheetId=emp.TermSheetId
			),0) as InstallationFee,
			pb.HoldBack,
		    pb.PassThrus as TotalPassThrus,
            pb.MMR,
            pb.Multiple,
			(ISNULL((select
			ppt.Amount
			from CustomerExtended ce
			LEFT JOIN PayrollPassThrus ppt on ppt.PassThrus=ce.AlarmBasicPackage
			Where ppt.TermSheetId=emp.TermSheetId AND ce.CustomerId=cus.CustomerId),0) +
			ISNULL((select SUM(ppt.aMount) Total from AlarmCustomerSelectedAddon ad
			left join  PayrollPassThrus ppt on ppt.PassThrus = ad.AddonType
			where ppt.TermSheetId=emp.TermSheetId AND ad.CustomerId=cus.CustomerId),0)
			) as PassThrus
			from PayrollBrinks pb
			LEFT JOIN Customer cus on cus.CustomerId=pb.CustomerId
			LEFT JOIN Employee emp on emp.UserId=cus.Soldby
			LEFT JOIN PayrollBaseMultiple pbm on pbm.TermSheetId=emp.TermSheetId

			LEFT JOIN PaymentInfoCustomer pic on pic.CustomerId=cus.CustomerId and pic.Payfor='MMR'
			LEFT JOIN PaymentProfileCustomer ppc on ppc.PaymentInfoId=pic.PaymentInfoId

			LEFT JOIN PayrollMonthlyProductionBonus pmpb on pmpb.TermSheetId=emp.TermSheetId and
			(Select COUNT(Id) from Customer cusinner where cusinner.Soldby1=emp.UserId) Between pmpb.MonthlyProductionBonusMin and pmpb.MonthlyProductionBonusMax

			LEFT JOIN PayrollCreditRating pcr on pcr.TermSheetId=emp.TermSheetId and
			cus.CreditScoreValue Between pcr.MinCredit and pcr.MaxCredit

			LEFT JOIN PayrollCustomerType pct on pct.TermSheetId=emp.TermSheetId and pct.CustomerType=cus.Type

			LEFT JOIN PayrollAgreementLength pal on pal.TermSheetId=emp.TermSheetId and pal.AgreementLength=cus.ContractTeam

			LEFT JOIN PayrollHoldBack phb on phb.TermSheetId=emp.TermSheetId and phb.HoldBack='ServiceFee'
			LEFT JOIN PayrollHoldBack phbmtm on phbmtm.TermSheetId=emp.TermSheetId and phbmtm.HoldBack='MonthToMonth'
			LEFT JOIN PayrollHoldBack phbaot on phbaot.TermSheetId=emp.TermSheetId and phb.HoldBack='AllotherTerms'
			where pb.Id={1}

            select
			eqp.Name EquipmentName,
			ISNULL(eqp.RepCost*cae.Quantity,0) as Cost
			from CustomerAppointmentEquipment cae
			LEFT JOIN Equipment eqp on eqp.EquipmentId=cae.EquipmentId
			LEFT JOIN Ticket tk on tk.TicketId=cae.AppointmentId
			LEFT JOIN PayrollBrinks pb on pb.TicketId=tk.TicketId
			where pb.Id={1} and eqp.EquipmentClassId=1";

            try
            {
                sqlQuery = string.Format(sqlQuery, NameSql, PayrollBrinksId);
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
        public DataSet GetAllPayrollBrinks(DateTime FilterStartDate, DateTime FilterEndDate, int pageno, int pagesize, string SearchText, string SalesPersonList, string order, string PayrollBrinksStatus, string PayrollBrinksFunding)
        {
            string DateFilter1 = "";
            string SearchQuery = "";
            string FilterQuery = "";
            string StatusFilterQuery = "";
            string FundingFilterQuery = "";
            string orderquery = "";
            string orderquery1 = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/salesperson")
                {
                    orderquery = "order by #cd.[SalesPerson] asc";
                    orderquery1 = "order by [SalesPerson] asc";
                }
                else if (order == "descending/salesperson")
                {
                    orderquery = "order by #cd.[SalesPerson] desc";
                    orderquery1 = "order by [SalesPerson] desc";
                }
                else if (order == "ascending/totalrmr")
                {
                    orderquery = "order by #cd.MMR asc";
                    orderquery1 = "order by MMR asc";
                }
                else if (order == "descending/totalrmr")
                {
                    orderquery = "order by #cd.MMR desc";
                    orderquery1 = "order by MMR desc";
                }
                else if (order == "ascending/grosspay")
                {
                    orderquery = "order by #cd.[GrossPay] asc";
                    orderquery1 = "order by [GrossPay] asc";
                }
                else if (order == "descending/grosspay")
                {
                    orderquery = "order by #cd.[GrossPay] desc";
                    orderquery1 = "order by [GrossPay] desc";
                }
                else if (order == "ascending/deductions")
                {
                    orderquery = "order by #cd.[Deductions] asc";
                    orderquery1 = "order by [Deductions] asc";
                }
                else if (order == "descending/deductions")
                {
                    orderquery = "order by #cd.[Deductions] desc";
                    orderquery1 = "order by [Deductions] desc";
                }
                else if (order == "ascending/adjustments")
                {
                    orderquery = "order by #cd.[Adjustment] asc";
                    orderquery1 = "order by [Adjustment] asc";
                }
                else if (order == "descending/adjustments")
                {
                    orderquery = "order by #cd.[Adjustment] desc";
                    orderquery1 = "order by [Adjustment] desc";
                }
                else if (order == "ascending/netpay")
                {
                    orderquery = "order by #cd.[NetPay] asc";
                    orderquery1 = "order by [NetPay] asc";
                }
                else if (order == "descending/netpay")
                {
                    orderquery = "order by #cd.[NetPay] desc";
                    orderquery1 = "order by [NetPay] desc";
                }

                else
                {
                    orderquery = "order by #cd.[Id]  desc";
                    orderquery1 = "order by Id desc";
                }

            }
            else
            {
                orderquery = "order by #cd.[Id] desc";
                orderquery1 = "order by Id desc";
            }
            #endregion
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }

            if (!string.IsNullOrEmpty(SearchText))
            {
                SearchQuery = string.Format(" and (emp.FirstName like '%{0}%' or emp.LastName like '%{0}%' or emp.FirstName +' '+emp.LastName like '%{0}%')", SearchText);
            }

            if (!string.IsNullOrEmpty(SalesPersonList) && SalesPersonList != "'null'")
            {
                FilterQuery = string.Format(" and pb.SalesPersonId in ({0})", SalesPersonList);
            }

            if (!string.IsNullOrEmpty(PayrollBrinksStatus) && PayrollBrinksStatus != "'null'")
            {
                StatusFilterQuery = string.Format(" and pb.FundingStatus in ({0})", PayrollBrinksStatus);
            }
            if (!string.IsNullOrEmpty(PayrollBrinksFunding) && PayrollBrinksFunding != "'null'")
            {
                if (PayrollBrinksFunding == "Funded")
                {
                    FundingFilterQuery = " and pb.IsPaid=1";
                }
                else if (PayrollBrinksFunding == "NotFunded")
                {
                    FundingFilterQuery = " and (pb.IsPaid Is NULL or pb.IsPaid=0)";
                }

            }
            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime())
            {

                FilterStartDate = FilterStartDate.SetZeroHour();
                FilterEndDate = FilterEndDate.SetMaxHour();


                DateFilter1 = string.Format("and cus.InstallDate between '{0}' and '{1}'"
                    , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            string sqlQuery = @"declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                select * into #SaleCom from (Select
								MAX(pb.Id) as Id,
                                emp.FirstName +' '+emp.LastName as SalesPerson,
                                MAX(emp.UserId) as SalesPersonId,
								SUM(pb.MMR) as MMR,
								SUM(pb.GrossPay) as GrossPay,
								SUM(pb.Deductions) as Deductions,
								SUM(pb.Adjustments) as Adjustment,
								SUM(pb.NetPay) as NetPay,
								MAX(pt.Name) as TermSheetName,
                                MAX(pt.TermSheetId) as TermSheetId,
                               ISNULL(SUM(pb.HoldBack),0) as HoldBack,
								ISNULL(SUM(pb.PassThrus),0) as PassThrus
                                From PayrollBrinks pb
                                LEFT JOIN Customer cus on cus.CustomerId=pb.CustomerId
                                LEFT JOIN Employee emp on emp.UserId=pb.SalesPersonId
								LEFT JOIN PayrollTermSheet pt on pt.TermSheetId=emp.TermSheetId
                                LEFT JOIN Ticket tk on tk.TicketId=pb.TicketId
                                where pb.NetPay>0 and tk.Status='Completed' {0} {1} {2} {5} {6}
								group by emp.FirstName,emp.LastName
								) d

								select top(@pagesize)
								* into #10SaleCom from #SaleCom
								where Id not in(select top(@pagestart) Id from #SaleCom #cd {3})
                                {4}

								select * from #10SaleCom

								select count(*) CountTotal
                                from #SaleCom

								select 
								SUM(MMR) as TotalTotalRMR,
								SUM(GrossPay) as TotalGrossPay,
                                SUM(HoldBack) as TotalHoldBack,
                                SUM(PassThrus) as TotalPassThru,
								SUM(Deductions) as TotalDeductions,
								SUM(Adjustment) as TotalAdjustments,
								SUM(NetPay) as TotalNetPay
								from #10SaleCom

								drop table #SaleCom
                                drop table #10SaleCom";

            try
            {
                sqlQuery = string.Format(sqlQuery, SearchQuery, DateFilter1, FilterQuery, orderquery, orderquery1, StatusFilterQuery, FundingFilterQuery);
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
        public DataSet DownLoadAllPayrollBrinks(DateTime FilterStartDate, DateTime FilterEndDate, int pageno, int pagesize, string SearchText, string SalesPersonList, string PayrollBrinksStatus, string PayrollBrinksFunding)
        {
            string DateFilter1 = "";
            string SearchQuery = "";
            string FilterQuery = "";
            string StatusFilterQuery = "";
            string FundingFilterQuery = "";
            string orderquery = "";
            string orderquery1 = "";

            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }

            if (!string.IsNullOrEmpty(SearchText))
            {
                SearchQuery = string.Format(" and (emp.FirstName like '%{0}%' or emp.LastName like '%{0}%' or emp.FirstName +' '+emp.LastName like '%{0}%')", SearchText);
            }

            if (!string.IsNullOrEmpty(SalesPersonList) && SalesPersonList != "'null'")
            {
                FilterQuery = string.Format(" and pb.SalesPersonId in ({0})", SalesPersonList);
            }

            if (!string.IsNullOrEmpty(PayrollBrinksStatus) && PayrollBrinksStatus != "'null'")
            {
                StatusFilterQuery = string.Format(" and pb.FundingStatus in ({0})", PayrollBrinksStatus);
            }
            if (!string.IsNullOrEmpty(PayrollBrinksFunding) && PayrollBrinksFunding != "'null'")
            {
                if (PayrollBrinksFunding == "Funded")
                {
                    FundingFilterQuery = " and pb.IsPaid=1";
                }
                else if (PayrollBrinksFunding == "NotFunded")
                {
                    FundingFilterQuery = " and (pb.IsPaid Is NULL or pb.IsPaid=0)";
                }

            }
            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime())
            {

                FilterStartDate = FilterStartDate.SetZeroHour();
                FilterEndDate = FilterEndDate.SetMaxHour();


                DateFilter1 = string.Format("and cus.InstallDate between '{0}' and '{1}'"
                    , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            string sqlQuery = @"Select
                                emp.FirstName +' '+emp.LastName as SalesPerson,
								Format(ISNULL(SUM(pb.MMR),0),'N2') as TotalRMR,
								Format(ISNULL(SUM(pb.GrossPay),0),'N2') as GrossPay,
                                Format(ISNULL(SUM(pb.HoldBack),0),'N2') as HoldBack,
								Format(ISNULL(SUM(pb.PassThrus),0),'N2') as PassThrus,
								Format(ISNULL(SUM(pb.Deductions),0),'N2') as Deductions,
								Format(ISNULL(SUM(pb.Adjustments),0),'N2') as Adjustment,
								Format(ISNULL(SUM(pb.NetPay),0),'N2') as NetPay
                                From PayrollBrinks pb
                                LEFT JOIN Customer cus on cus.CustomerId=pb.CustomerId
                                LEFT JOIN Employee emp on emp.UserId=pb.SalesPersonId
								LEFT JOIN PayrollTermSheet pt on pt.TermSheetId=emp.TermSheetId
                                LEFT JOIN Ticket tk on tk.TicketId=pb.TicketId
                                where pb.NetPay>0 and tk.Status='Completed' {0} {1} {2} {3} {4}
								group by emp.FirstName,emp.LastName";

            try
            {
                sqlQuery = string.Format(sqlQuery, SearchQuery, DateFilter1, FilterQuery, StatusFilterQuery, FundingFilterQuery);
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

        public DataSet GetAllSalesReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, bool IsPaid, Guid UserId, string SearchText, string SalesPersonList)
        {
            string DateFilter2 = "";
            string DateFilter1 = "";
            string IsPaidFilter = "";
            string techPayrollFilter = "";
            string SearchQuery = "";
            string FilterQuery = "";

            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
                //NameSql = NameSql.Replace("cus.", "cu.");
            }

            if (!string.IsNullOrEmpty(SearchText))
            {
                SearchQuery = string.Format(" (tkId like @SearchText or  CustomerName like @SearchText or SalesPerson like @SearchText or BalanceDue like @SearchText) and ");
            }

            if (!string.IsNullOrEmpty(SalesPersonList) && SalesPersonList != "'null'")
            {

                FilterQuery = string.Format(" UserId in ({0}) and ", SalesPersonList);
            }

            if (UserId != new Guid())
            {
                techPayrollFilter = string.Format(" and sc.UserId = '{0}'", UserId);
            }
            if (IsPaid)
            {
                IsPaidFilter = "where sc.IsPaid = 1";
            }
            else
            {
                IsPaidFilter = "where (sc.IsPaid = 0 or sc.IsPaid is null)";
            }
            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime())
            {

                FilterStartDate = FilterStartDate.SetZeroHour();
                FilterEndDate = FilterEndDate.SetMaxHour();


                DateFilter1 = string.Format("and Sc.CreatedDate between '{0}' and '{1}'"
                    , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            string sqlQuery = @"
                                declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                 select * into #SaleCom from (
                                select 
                                sc.*,
                                tk.Id as tkId,
                                cus.Id as CustomerIdValue,
                                {7} as CustomerName,
                                emp.FirstName +' '+emp.LastName as SalesPerson,
								(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                From Invoice where CustomerId=cus.CustomerId
                                and (Status='Open' or Status='Partial')) as BalanceDue
						
                                from SalesCommission Sc
                                left join customer cus on cus.CustomerId = sc.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on   emp.UserId =  sc.UserId
                                left join Ticket tk on tk.TicketId = sc.TicketId
                                {5}{4}{6} and tk.IsClosed = 1 and ce.IsTestAccount != 1
                                  ) d	

                                select * into #SalesCommissionFilter
								from #SaleCom  

								select top(@pagesize)
								* into #TotalCount from #SalesCommissionFilter
								where {8} {9}  Id not in(select top(@pagestart) Id from #SalesCommissionFilter #e {2})
                                {3}
								select count(*) CountTotal
                                from #SalesCommissionFilter
                                where {8} {9} Id > 0

								select * from #TotalCount
								select sum(Adjustment) as TotalAdjustment
								,sum(TotalCommission) as SumCommission
								,sum(TotalPoint) as SumPoint
								,sum(OriginalPoint) as SumCommissionablePoint
								,sum(BalanceDue) as SumUnpaid
								,sum(EquipmentCommission) as SumOverage
								from #TotalCount

								drop table #SaleCom
								drop table #SalesCommissionFilter
								drop table #TotalCount";

            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(order) && order != "undefined" && order != "null")
            {
                if (order == "ascending/rmr")
                {
                    subquery = "order by #e.RMRSold asc";
                    subquery1 = "order by RMRSold asc";
                }
                else if (order == "descending/rmr")
                {
                    subquery = "order by #e.RMRSold desc";
                    subquery1 = "order by RMRSold desc";
                }
                else if (order == "ascending/ticketid")
                {
                    subquery = "order by #e.Id asc";
                    subquery1 = "order by Id asc";
                }
                else if (order == "descending/ticketid")
                {
                    subquery = "order by #e.Id desc";
                    subquery1 = "order by Id desc";
                }
                else if (order == "ascending/equipment")
                {
                    subquery = "order by #e.NoOfEquipment asc";
                    subquery1 = "order by NoOfEquipment asc";
                }
                else if (order == "descending/equipment")
                {
                    subquery = "order by #e.NoOfEquipment desc";
                    subquery1 = "order by NoOfEquipment desc";
                }
                else if (order == "ascending/adjustment")
                {
                    subquery = "order by #e.Adjustment asc";
                    subquery1 = "order by Adjustment asc";
                }
                else if (order == "descending/adjustment")
                {
                    subquery = "order by #e.Adjustment desc";
                    subquery1 = "order by Adjustment desc";
                }
                else if (order == "ascending/commission")
                {
                    subquery = "order by #e.TotalCommission asc";
                    subquery1 = "order by TotalCommission asc";
                }
                else if (order == "descending/commission")
                {
                    subquery = "order by #e.TotalCommission desc";
                    subquery1 = "order by TotalCommission desc";
                }
                else if (order == "ascending/totalpoint")
                {
                    subquery = "order by #e.TotalPoint asc";
                    subquery1 = "order by TotalPoint asc";
                }
                else if (order == "descending/totalpoint")
                {
                    subquery = "order by #e.TotalPoint desc";
                    subquery1 = "order by TotalPoint desc";
                }
                else if (order == "ascending/commissionablepoints")
                {
                    subquery = "order by #e.OriginalPoint asc";
                    subquery1 = "order by OriginalPoint asc";
                }
                else if (order == "descending/commissionablepoints")
                {
                    subquery = "order by #e.OriginalPoint desc";
                    subquery1 = "order by OriginalPoint desc";
                }
                else if (order == "ascending/unpaidbalance")
                {
                    subquery = "order by #e.BalanceDue asc";
                    subquery1 = "order by BalanceDue asc";
                }
                else if (order == "descending/unpaidbalance")
                {
                    subquery = "order by #e.BalanceDue desc";
                    subquery1 = "order by BalanceDue desc";
                }
                else if (order == "ascending/overage")
                {
                    subquery = "order by #e.EquipmentCommission asc";
                    subquery1 = "order by EquipmentCommission asc";
                }
                else if (order == "descending/overage")
                {
                    subquery = "order by #e.EquipmentCommission desc";
                    subquery1 = "order by EquipmentCommission desc";
                }
                else if (order == "ascending/batch")
                {
                    subquery = "order by #e.Batch asc";
                    subquery1 = "order by Batch asc";
                }
                else if (order == "descending/batch")
                {
                    subquery = "order by #e.Batch desc";
                    subquery1 = "order by Batch desc";
                }
            }
            else
            {
                subquery = "order by #e.tkId desc";
                subquery1 = "order by tkId desc";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, DateFilter2, order, subquery, subquery1, DateFilter1, IsPaidFilter, techPayrollFilter, NameSql, SearchQuery, FilterQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", SearchText)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }

            }
            catch (Exception ee)
            {
                return null;
            }

        }

        public DataSet GetDownLoadAllSalesReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, bool IsPaid, Guid UserId, string SearchText, string SalesPersonList)
        {
            string DateFilter2 = "";
            string DateFilter1 = "";
            string IsPaidFilter = "";
            string techPayrollFilter = "";
            string SearchQuery = "";
            string FilterQuery = "";
            string Batch = "";

            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
                //NameSql = NameSql.Replace("cus.", "cu.");
            }

            if (!string.IsNullOrEmpty(SearchText))
            {
                SearchQuery = string.Format(" ([Ticket ID] like @SearchText or  [Customer Name] like @SearchText or [Sales Person] like @SearchText or [Unpaid Balance] like @SearchText) and ");
            }

            if (!string.IsNullOrEmpty(SalesPersonList) && SalesPersonList != "'null'")
            {

                FilterQuery = string.Format(" UserId in ({0}) and ", SalesPersonList);
            }

            if (UserId != new Guid())
            {
                techPayrollFilter = string.Format(" and sc.UserId = '{0}'", UserId);
            }
            if (IsPaid)
            {
                IsPaidFilter = "where sc.IsPaid = 1";
                Batch = ", [Batch]";
            }
            else
            {
                IsPaidFilter = "where (sc.IsPaid = 0 or sc.IsPaid is null)";
            }
            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime())
            {

                FilterStartDate = FilterStartDate.SetZeroHour();
                FilterEndDate = FilterEndDate.SetMaxHour();


                DateFilter1 = string.Format("and Sc.CreatedDate between '{0}' and '{1}'"
                    , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            string sqlQuery = @"
                                declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                 select * into #SaleCom from (
                                select 
                                tk.Id as [Ticket ID],
                                {7} as [Customer Name],
                                cus.CustomerNo as [CS ID],

                                emp.FirstName +' '+emp.LastName as [Sales Person],
                                TRY_CONVERT(date,sc.CompletionDate) as [Completion Date],
                                cast(sc.RMRSold as decimal(12,2)) as [RMR Sold],
                                cast(sc.RMRCommission as decimal(12,2)) as [RMR Comm.],
                                sc.NoOfEquipment as [# Of Equip.],
                                cast(sc.EquipmentCommission as decimal(12,2)) as [Equip Comm.],
                                sc.Adjustment as [Adjustment],
                                cast(sc.TotalCommission as decimal(12,2))  as [Commission],
                                sc.UserId as [UserId],
								sc.Id as [Id],
								sc.Batch as [Batch],
                                ISNULL(sc.TotalPoint,0) as TotalPoint,
                                ISNULL(sc.OriginalPoint,0) as CommissionablePoints,
                                
								 (select cast(ISNULL(SUM(BalanceDue),0)  as decimal(12,2)) [Unpaid Balance]
                                From Invoice where CustomerId=cus.CustomerId
                                and (Status='Open' or Status='Partial')) as [Unpaid Balance]
						
                                from SalesCommission Sc
                                left join customer cus on cus.CustomerId = sc.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on   emp.UserId =  sc.UserId
                                left join Ticket tk on tk.TicketId = sc.TicketId
                                {5}{4}{6} and tk.IsClosed = 1 and ce.IsTestAccount != 1
                                  ) d	

                                select * into #SalesCommissionFilter
								from #SaleCom

								select 
								[Ticket ID],
                                [Customer Name],
                                [CS ID],
                                [Sales Person],
                                [Completion Date],
                                [RMR Sold],
                                [RMR Comm.],
                                [# Of Equip.],
                                [Equip Comm.],
                                [Adjustment],
                                [Commission],
                                [TotalPoint],
								[CommissionablePoints],
                                [Unpaid Balance],
                                [Equip Comm.] as Overage
                                {10}


                                from #SalesCommissionFilter
								where {8} {9}  Id not in(select top(@pagestart) Id from #SalesCommissionFilter #e {2})
                                {3}
								select count(*) CountTotal
                                from #SalesCommissionFilter
                                where {8} {9} Id > 0

								drop table #SaleCom
								drop table #SalesCommissionFilter";

            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(order) && order != "undefined" && order != "null")
            {
                if (order == "ascending/customername")
                {
                    subquery = "order by #e.[Customer Name] asc";
                    subquery1 = "order by [Customer Name] asc";
                }
                else if (order == "descending/customername")
                {
                    subquery = "order by #e.[Customer Name] desc";
                    subquery1 = "order by [Customer Name] desc";
                }
                else if (order == "ascending/ticketid")
                {
                    subquery = "order by #e.Id asc";
                    subquery1 = "order by Id asc";
                }
                else if (order == "descending/ticketid")
                {
                    subquery = "order by #e.Id desc";
                    subquery1 = "order by Id desc";
                }
                else if (order == "ascending/userassigned")
                {
                    subquery = "order by #e.UserAssigned asc";
                    subquery1 = "order by UserAssigned asc";
                }
                else if (order == "descending/userassigned")
                {
                    subquery = "order by #e.UserAssigned desc";
                    subquery1 = "order by UserAssigned desc";
                }
                else if (order == "ascending/completiondate")
                {
                    subquery = "order by #e.[Completion Date] asc";
                    subquery1 = "order by [Completion Date] asc";
                }
                else if (order == "descending/completiondate")
                {
                    subquery = "order by #e.[Completion Date] desc";
                    subquery1 = "order by [Completion Date] desc";
                }
            }
            else
            {
                subquery = "order by #e.[Ticket ID] desc";
                subquery1 = "order by [Ticket ID] desc";
            }

            try
            {
                sqlQuery = string.Format(sqlQuery, DateFilter2, order, subquery, subquery1, DateFilter1, IsPaidFilter, techPayrollFilter, NameSql, SearchQuery, FilterQuery, Batch);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", SearchText)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }

            }
            catch (Exception ee)
            {
                return null;
            }

        }

        public DataSet GetTicketListByFilter(TicketFilter TicketFilters, FilterReportModel filter)
        {
            string sqlQuery = @"";
            string searchQuery = "";
            string myTicketQuery = "";
            string ticketStatusQuery = "";
            string ticketTypeQuery = "";
            string assignedQuery = "";
            string CreatedByMeQuery = "";
            string subquery = "";
            string subquery1 = "";
            string CreatedDateQuery = "";
            string AppointmentDateQuery = "";
            string ReportTypeQuery = "";
            string ReportQuery = "";
            string ReportColQuery = "";
            string ReportCountQuery = "";
            string filterquery = "";
            string ReportAgeQuery = "";
            string gobacksearchquery = "";
            string searchQueryupdated = "";
            if (!string.IsNullOrWhiteSpace(TicketFilters.SearchText))
            {
                searchQueryupdated = string.Format("and ticket.id like '%{0}%' or cus.id like '%{0}%'", TicketFilters.SearchText);
            }
            //if (!string.IsNullOrWhiteSpace(TicketFilters.SearchText))
            //{
            //    searchQuery = @" where CustomerName like @SearchText";
            //}
            if (!string.IsNullOrWhiteSpace(TicketFilters.ReportTabType) && TicketFilters.ReportTabType == "GoBack")
            {
                //gobacksearchquery = string.Format("and ticket.Id like '{0}' or cus.Id like '{0}' or firstname+' '+lastname like '{0}'", TicketFilters.SearchText);
                //        and(select count(tik.Id) from ticket tik where convert(date, tik.CreatedDate) between convert(date, dateadd(day, -90, getdate() + 1)) and convert(date, getdate() + 1) and tik.CustomerId = cus.CustomerId) > 1

                //    ReportTypeQuery = string.Format("and convert(date, ticket.CreatedDate) between '{0}' and '{1}'", TicketFilters.StartDate, TicketFilters.EndDate);
                ReportTypeQuery = string.Format("and convert(date, ticket.CreatedDate) between convert(date, dateadd(day, -90, getdate() + 1)) and convert(date, getdate() + 1)");

                //ReportQuery = string.Format("and CountTicket > 1");
                ReportColQuery = string.Format("(select count(tik.Id) from ticket tik where convert(date, tik.CreatedDate) between '{0}' and '{1}' and tik.CustomerId=cus.CustomerId) as CountTicket", TicketFilters.StartDate, TicketFilters.EndDate);
                ReportCountQuery = string.Format("select  count(paginationid) as [TotalCount] from #TicketDataFilter");
                //ReportAgeQuery = string.Format("and (select count(tik.Id) from ticket tik where convert(date, tik.CreatedDate) between '{0}' and '{1}' and tik.CustomerId=cus.CustomerId) > 1", TicketFilters.StartDate, TicketFilters.EndDate);
                ReportAgeQuery = string.Format("and (select count(tik.Id) from ticket tik where convert(date, tik.CreatedDate) between convert(date, dateadd(day, -90, getdate() + 1)) and convert(date, getdate() + 1) and tik.CustomerId=cus.CustomerId) > 1", TicketFilters.StartDate, TicketFilters.EndDate);

            }
            else
            {
                ReportColQuery = string.Format("'' as CountTicket");
                ReportCountQuery = string.Format("select  count(Id) as [TotalCount] from #TicketIdData");
            }
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }

            #region TicketStatus
            if (!string.IsNullOrWhiteSpace(TicketFilters.TicketStatus)
                && TicketFilters.TicketStatus != "-1" && TicketFilters.TicketStatus != "null")
            {
                ticketStatusQuery = string.Format("and ticket.[Status] in ('{0}')", HttpUtility.UrlDecode(TicketFilters.TicketStatus));
            }
            #endregion

            //#region Assigned
            //if (!string.IsNullOrWhiteSpace(TicketFilters.AssignedUserTicket) && TicketFilters.AssignedUserTicket != "-1" && TicketFilters.AssignedUserTicket != "null" && TicketFilters.AssignedUserTicket != new Guid().ToString())
            //{
            //    assignedQuery = string.Format("and tuser.UserId in ('{0}')", TicketFilters.AssignedUserTicket);
            //}
            //#endregion


            #region TicketType
            if (!string.IsNullOrWhiteSpace(TicketFilters.TicketType)
                && TicketFilters.TicketType != "-1" && TicketFilters.TicketType != "null")
            {
                ticketTypeQuery = string.Format("and ticket.TicketType in ('{0}')", HttpUtility.UrlDecode(TicketFilters.TicketType));
            }
            #endregion
            //#region CreatedDateQuery
            //if (TicketFilters.StartDate != new DateTime() && TicketFilters.EndDate != new DateTime())
            //{
            //    var StartDate = TicketFilters.StartDate.SetZeroHour().UTCToClientTime();
            //    var EndDate = TicketFilters.EndDate.SetMaxHour().UTCToClientTime();
            //    CreatedDateQuery = string.Format("and ticket.CreatedDate between '{0}' and '{1}'", StartDate, EndDate);
            //}
            //#endregion
            #region AppointmentDateQuery
            if (TicketFilters.StartDate != new DateTime() && TicketFilters.EndDate != new DateTime())
            {
                var StartDate = TicketFilters.StartDate.ToString("yyyy-MM-dd 00:00:00.000");
                var EndDate = TicketFilters.EndDate.ToString("yyyy-MM-dd 23:59:59.000");
                AppointmentDateQuery = string.Format("and ticket.CompletionDate between '{0}' and '{1}'", StartDate, EndDate);
            }
            #endregion
            #region MyTicket
            if (!string.IsNullOrWhiteSpace(TicketFilters.MyTicket))
            {
                if (TicketFilters.MyTicket == "Created")
                {
                    CreatedByMeQuery = string.Format("and ticket.CreatedByUid = '{0}'", TicketFilters.UserId);

                }
                else if (TicketFilters.MyTicket == "Assigned")
                {
                    myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(ticket.TicketId,'{0}') = 1 ", TicketFilters.UserId);
                }
                else if (TicketFilters.MyTicket == "Both")
                {
                    CreatedByMeQuery = string.Format("and ticket.CreatedByUid = '{0}'", TicketFilters.UserId);
                    myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(ticket.TicketId,'{0}') = 1 ", TicketFilters.UserId);
                }
                else if (TicketFilters.MyTicket == "None")
                {
                    CreatedByMeQuery = string.Format("and ticket.CreatedByUid != '{0}'", TicketFilters.UserId);
                    myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(ticket.TicketId,'{0}') = 0 ", TicketFilters.UserId);

                }
            }

            #endregion
            #region Order
            if (!string.IsNullOrWhiteSpace(TicketFilters.order))
            {
                if (TicketFilters.order == "ascending/ticketid")
                {
                    subquery = "order by #tdf.[Id] asc";
                    subquery1 = "order by [Id] asc";
                }
                else if (TicketFilters.order == "descending/ticketid")
                {
                    subquery = "order by #tdf.[Id] desc";
                    subquery1 = "order by [Id] desc";
                }
                else if (TicketFilters.order == "ascending/customername")
                {
                    subquery = "order by #tdf.CustomerName asc";
                    subquery1 = "order by CustomerName asc";
                }
                else if (TicketFilters.order == "descending/customername")
                {
                    subquery = "order by #tdf.CustomerName desc";
                    subquery1 = "order by CustomerName desc";
                }
                else if (TicketFilters.order == "ascending/tickettype")
                {
                    subquery = "order by #tdf.[TicketTypeVal] asc";
                    subquery1 = "order by [TicketTypeVal] asc";
                }
                else if (TicketFilters.order == "descending/tickettype")
                {
                    subquery = "order by #tdf.[TicketTypeVal] desc";
                    subquery1 = "order by [TicketTypeVal] desc";
                }
                else if (TicketFilters.order == "ascending/appointmentdate")
                {
                    subquery = "order by #tdf.[CompletionDate] asc";
                    subquery1 = "order by [CompletionDate] asc";
                }
                else if (TicketFilters.order == "descending/appointmentdate")
                {
                    subquery = "order by #tdf.[CompletionDate] desc";
                    subquery1 = "order by [CompletionDate] desc";
                }
                else if (TicketFilters.order == "ascending/technician")
                {
                    subquery = "order by #tdf.[AssignedTo] asc";
                    subquery1 = "order by [AssignedTo] asc";
                }
                else if (TicketFilters.order == "descending/technician")
                {
                    subquery = "order by #tdf.[AssignedTo] desc";
                    subquery1 = "order by [AssignedTo] desc";
                }
                else if (TicketFilters.order == "ascending/installdate")
                {
                    subquery = "order by #tdf.[InstallDate]  asc";
                    subquery1 = "order by InstallDate asc";
                }
                else if (TicketFilters.order == "descending/installdate")
                {
                    subquery = "order by #tdf.[InstallDate]  desc";
                    subquery1 = "order by InstallDate desc";
                }
                else if (TicketFilters.order == "ascending/leadsource")
                {
                    subquery = "order by #tdf.[LeadSource]  asc";
                    subquery1 = "order by LeadSource asc";
                }
                else if (TicketFilters.order == "descending/leadsource")
                {
                    subquery = "order by #tdf.[LeadSource]  desc";
                    subquery1 = "order by LeadSource desc";
                }
                else if (TicketFilters.order == "ascending/saleslocation")
                {
                    subquery = "order by #tdf.[CusSalesLoc]  asc";
                    subquery1 = "order by CusSalesLoc asc";
                }
                else if (TicketFilters.order == "descending/saleslocation")
                {
                    subquery = "order by #tdf.[CusSalesLoc]  desc";
                    subquery1 = "order by CusSalesLoc desc";
                }
                else if (TicketFilters.order == "ascending/salesperson")
                {
                    subquery = "order by #tdf.[CusSalesPerson]  asc";
                    subquery1 = "order by CusSalesPerson asc";
                }
                else if (TicketFilters.order == "descending/salesperson")
                {
                    subquery = "order by #tdf.[CusSalesPerson]  desc";
                    subquery1 = "order by CusSalesPerson desc";
                }
                else if (TicketFilters.order == "ascending/RMR")
                {
                    subquery = "order by #tdf.[RMRAmount]  asc";
                    subquery1 = "order by RMRAmount asc";
                }
                else if (TicketFilters.order == "descending/RMR")
                {
                    subquery = "order by #tdf.[RMRAmount]  desc";
                    subquery1 = "order by RMRAmount desc";
                }

                else
                {
                    subquery = "order by #tdf.[Id]  desc";
                    subquery1 = "order by Id desc";
                }

            }
            else
            {
                subquery = "order by #tdf.[Id] desc";
                subquery1 = "order by Id desc";
            }
            #endregion

            #region Filter Query
            if (!string.IsNullOrWhiteSpace(filter.id))
            {
                filterquery += string.Format("and ticket.Id = '{0}'", filter.id);
            }
            if (!string.IsNullOrWhiteSpace(filter.cusid))
            {
                filterquery += string.Format("and cus.Id = '{0}'", filter.cusid);
            }
            if (!string.IsNullOrWhiteSpace(filter.user) && filter.user != "-1")
            {
                filterquery += string.Format("and cus.Soldby = '{0}'", filter.user);
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.AssignedUserTicket) && TicketFilters.AssignedUserTicket != "null")
            {
                filterquery += string.Format("and tuser.UserId in ('{0}')", TicketFilters.AssignedUserTicket);
            }
            if (!string.IsNullOrWhiteSpace(filter.convertmindate) && !string.IsNullOrWhiteSpace(filter.convertmaxdate))
            {
                var datemin = Convert.ToDateTime(filter.convertmindate);
                var date = Convert.ToDateTime(filter.convertmaxdate);
                filterquery += string.Format("and ticket.CompletionDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.convertmindate))
            {
                var date = Convert.ToDateTime(filter.convertmindate);
                filterquery += string.Format("and ticket.CompletionDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.convertmaxdate))
            {
                var date = Convert.ToDateTime(filter.convertmaxdate);
                filterquery += string.Format("and ticket.CompletionDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }

            if (!string.IsNullOrWhiteSpace(filter.createmindate) && !string.IsNullOrWhiteSpace(filter.createmaxdate))
            {
                DateTime datemin = Convert.ToDateTime(filter.createmindate);
                DateTime date = Convert.ToDateTime(filter.createmaxdate);
                filterquery += string.Format("and cus.SalesDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.createmindate))
            {
                var date = Convert.ToDateTime(filter.createmindate);
                filterquery += string.Format("and cus.SalesDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.createmaxdate))
            {
                var date = Convert.ToDateTime(filter.createmaxdate);
                filterquery += string.Format("and cus.SalesDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }

            if (!string.IsNullOrWhiteSpace(filter.transfermindate) && !string.IsNullOrWhiteSpace(filter.transfermaxdate))
            {
                var datemin = Convert.ToDateTime(filter.transfermindate);
                var date = Convert.ToDateTime(filter.transfermaxdate);
                filterquery += string.Format("and cus.InstallDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.transfermindate))
            {
                var date = Convert.ToDateTime(filter.transfermindate);
                filterquery += string.Format("and cus.InstallDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.transfermaxdate))
            {
                var date = Convert.ToDateTime(filter.transfermaxdate);
                filterquery += string.Format("and cus.InstallDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }


            if (!string.IsNullOrWhiteSpace(filter.ticketcreateddatemin) && !string.IsNullOrWhiteSpace(filter.ticketcreateddatemax))
            {
                var datemin = Convert.ToDateTime(filter.ticketcreateddatemin);
                var date = Convert.ToDateTime(filter.ticketcreateddatemax);
                filterquery += string.Format("and ticket.CreatedDate between '{0}' and '{1}'", datemin.SetZeroHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss.fff"), date.SetMaxHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.ticketcreateddatemin))
            {
                var date = Convert.ToDateTime(filter.ticketcreateddatemin);
                filterquery += string.Format("and ticket.CreatedDate between '{0}' and '{1}'", date.SetZeroHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss.fff"), date.SetMaxHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.ticketcreateddatemax))
            {
                var date = Convert.ToDateTime(filter.ticketcreateddatemax);
                filterquery += string.Format("and ticket.CreatedDate between '{0}' and '{1}'", date.SetZeroHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss.fff"), date.SetMaxHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            #endregion
            sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                DECLARE @CustomerId uniqueidentifier
                                DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int
                                DECLARE @SearchText nvarchar(50) 

                                SET @SearchText = '%{0}%' 
                                SET @pageno = {1} --default 1
                                SET @pagesize = {2} --default 10
                                SET @CompanyId = '{3}' --97BCF758-A482-47EB-82B8-F88BF12293FF
                                SET @CustomerId = '{4}'

                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

 
                                select ticket.Id,
                                ticket.BookingId, 
                                ticket.TicketId, 
                                ticket.CustomerId, 
                                ticket.TicketType, 
                                ticket.[Status], 
                                ticket.[Priority], 
                                ticket.CreatedBy, 
                                ticket.CompanyId, 
                                ticket.CompletionDate, 
								cus.Id as cusid,
                                tuser.UserId as UserId,
                                ticket.CreatedDate into #TicketIdData from Ticket ticket
		                        left join TicketUser tuser on tuser.TiketId = ticket.TicketId
                                left join Customer cus on cus.CustomerId = ticket.CustomerId
                                where ticket.CompanyId = @CompanyId
                                {6}
                                        {7}
                                        {8}
                                        {9}
                                        {10}
                                        {13}
                                        {14}
                                        {19}
                                        {20}
		                                select 
                                       -- top (@pageno*@pagesize) 

                                        ticket.*,
                                        CASE 
	                                        WHEN (cus.DBA = '' or cus.DBA IS NULL) AND  (cus.BusinessName = '' or cus.BusinessName IS NULL) THEN cus.FirstName +' '+cus.LastName
	                                        WHEN (cus.DBA = '' or cus.DBA IS NULL)  THEN cus.BusinessName
	                                        ELSE  cus.DBA
                                        END as CustomerName
                                        ,cus.SalesDate
										,cus.InstallDate
                                        ,(select count(id) from TicketFile where TicketId = ticket.TicketId) as AttachmentsCount
                                        ,(select count(id) from TicketFile where TicketId = ticket.TicketId)
		                                        + (select count(id) from TicketReply where TicketId = ticket.TicketId) as RepliesCount
                                        ,lktype.DisplayText as TicketTypeVal
                                        ,lkstatus.DisplayText as StatusVal
                                        ,lkpriority.DisplayText as PriorityVal
                                        ,emp.FirstName + ' '+emp.LastName as CreatedByVal
                                        ,(select CAST(firstname + ' '+LastName + ', ' AS VARCHAR(200))  from Employee  where UserId in (select UserId from TicketUser tulist where tulist.TiketId = ticket.TicketId and IsPrimary = 1) FOR XML PATH ('') ) as AssignedTo
                                        --,(select CAST(firstname + ' '+LastName + ', ' AS VARCHAR(200))  from Employee  where UserId in (select UserId from TicketUser tualist where tualist.TiketId = ticket.TicketId and IsPrimary = 0) FOR XML PATH ('') ) as AdditionalMembers
	                                    --,lkStartTime.DisplayText as AppointmentStartTimeVal
                                        --,CA.AppointmentStartTime as AppointmentStartTime
                                        --,lkEndTime.DisplayText as AppointmentEndTimeVal
                                        --,CA.AppointmentEndTime as AppointmentEndTime
                                        --,(select COUNT(cae.Id)
                                        
										--from CustomerAppointmentEquipment cae
										--LEFT JOIN Ticket t on t.TicketId=cae.AppointmentId
										--LEFT JOIN TicketUser tu on tu.TiketId=t.TicketId and tu.IsPrimary=1
										--where cae.AppointmentId=CA.AppointmentId
                                        --AND cae.IsEquipmentRelease=0
										--AND cae.Quantity>(ISNULL((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=CAE.EquipmentId and Type='Add'  And invinner.TechnicianId=tu.UserId)-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=CAE.EquipmentId and Type='Release'  And invinner.TechnicianId=tu.UserId),0))) as ExceedQuantity,
                                        ,cus.Id as CusIdInt
                                        ,(select sum(TotalAmount) from Invoice 
										    where bookingId = ticket.BookingId and bookingId != '' 
										        and (Status = 'Open' or Status = 'Partial' or Status ='Paid')) as BookingInvoiceAmount
                                        ,isnull(cus.BusinessName, '') as CusBusinessName,
                                        isnull(sales.FirstName + ' ' + sales.LastName, '') as CusSalesPerson,
                                        isnull(installer.FirstName + ' ' + installer.LastName, '') as CusInstaller,
                                        isnull((select SUM(cae.TotalPrice) from CustomerAppointmentEquipment cae
											LEFT JOIN Ticket tk on tk.TicketId=cae.AppointmentId
											LEFT JOIN equipment eqp on eqp.EquipmentId=cae.EquipmentId
											where tk.CustomerId=cus.CustomerId
											and cae.IsService=1 and (cae.IsDefaultService is NULL or cae.IsDefaultService=0) 
											and (cae.IsCopied is NULL or cae.IsCopied=0) and eqp.IsArbEnabled=1),0) as RMRAmount,
                                        {16}
                                        --,LAG(lktype.DisplayText) OVER (ORDER BY ticket.Id) as PrevTicketType
										--,LAG(ticket.CompletionDate) OVER (ORDER BY ticket.Id) as PrevAppointmentDate
										--,LAG(emp.FirstName + ' '+emp.LastName) OVER (ORDER BY ticket.Id) as PrevTechnician
                                        ,lksalesloc.DisplayText as CusSalesLoc
										,lkleadsource.DisplayText as LeadSource 
										into #TicketData
                                        from #TicketIdData ticket
                                        LEFT JOIN Customer cus on cus.CustomerId=ticket.CustomerId
                                        left join TicketUser tuser on tuser.TiketId = ticket.TicketId and tuser.IsPrimary = 1
                                        left join Lookup lktype on  lktype.DataKey ='TicketType'  
                                        and lktype.DataValue = ticket.TicketType

                                        left join CustomerAppointment CA on  CA.AppointmentId = ticket.TicketId

                                        left join Lookup lkStartTime on lkStartTime.DataKey = 'Arrival'
                                        and lkStartTime.DataValue = CA.AppointmentStartTime

                                        left join Lookup lkEndTime on lkEndTime.DataKey = 'Arrival'
                                        and lkEndTime.DataValue = CA.AppointmentEndTime

                                        left join Lookup lkstatus on  lkstatus.DataKey ='TicketStatus'  
                                        and lkstatus.DataValue = ticket.[Status]

                                        left join Lookup lkpriority on  lkpriority.DataKey ='TicketPriority'  
                                        and lkpriority.DataValue = ticket.[Priority]

                                        left join Lookup lksalesloc on  lksalesloc.DataKey ='CommissionType'  
                                        and lksalesloc.DataValue = iif(cus.SalesLocation != '-1', cus.SalesLocation, null)

										left join Lookup lkleadsource on  lkleadsource.DataKey ='LeadSource'  
                                        and lkleadsource.DataValue = iif(cus.LeadSource != '-1', cus.LeadSource, null)

                                        left join Employee emp on emp.UserId = ticket.CreatedBy
                                        left join Employee sales on CONVERT(nvarchar(50), sales.UserId) = cus.Soldby
                                        left join Employee installer on CONVERT(nvarchar(50), installer.UserId) = cus.Soldby
                                        
		                                where ticket.CompanyId = @CompanyId
                                        and iif(cus.FirstName +' '+cus.LastName is null, cus.BusinessName, cus.DBA) is not null
                                        {6}
                                        {7}
                                        {8}
                                        {9}
                                        {10}
                                        {13}
                                        {14}
                                        {19}
                                         {22}
                                        order by ticket.Id desc
select *,IDENTITY(INT, 1, 1) AS paginationid into #TicketDataFilter from #TicketData{5}

                                SELECT TOP (@pagesize) #tdf.*, LAG(#tdf.TicketTypeVal) OVER (ORDER BY #tdf.Id) as PrevTicketType
                                , LAG(#tdf.CompletionDate) OVER (ORDER BY #tdf.Id) as PrevAppointmentDate
								 , LAG(#tdf.CreatedByVal) OVER (ORDER BY #tdf.Id) as PrevTechnician into #TestTable
                                FROM #TicketDataFilter #tdf
                                    where paginationid NOT IN(Select TOP (@pagestart) paginationid from #TicketDataFilter  #tdf {11}) 
                                    -- {15}
                                     {12}
	                           select  count(Id) as [TotalCount] from #TicketData where Id>0 and Id is not null and cusid>0 and cusid is not null and Id like '%%' or cusid like '%%'

	                            select * from #TestTable
								select sum(RMRAmount) as TotalRMR from #TestTable
                                
                                DROP TABLE #TicketData
								DROP TABLE #TicketDataFilter
								drop table #TicketIdData
								drop table #TestTable
                                    ";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        TicketFilters.SearchText,//0
                                        TicketFilters.PageNo,  //1
                                        TicketFilters.PageSize, //2
                                        TicketFilters.CompanyId, //3
                                        TicketFilters.CustomerId, //4
                                        searchQuery,//5
                                        ticketStatusQuery,//6
                                        ticketTypeQuery,//7
                                        assignedQuery,//8
                                        CreatedByMeQuery,//9
                                        myTicketQuery,//10
                                        subquery,//11
                                        subquery1,//12
                                                  //CreatedDateQuery,//13,
                                        AppointmentDateQuery,//13
                                        ReportTypeQuery,//14,
                                        ReportQuery,//15,
                                        ReportColQuery,//16
                                        ReportCountQuery,//17
                                        NameSql,
                                        filterquery,
                                        ReportAgeQuery,
                                        gobacksearchquery,
                                        searchQueryupdated

                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetAllAppointmentDateReport(TicketFilter TicketFilters, FilterReportModel filter)
        {
            string sqlQuery = @"";
            string searchQuery = "";
            string myTicketQuery = "";
            string ticketStatusQuery = "";
            string ticketTypeQuery = "";
            string assignedQuery = "";
            string CreatedByMeQuery = "";
            string subqueryforpaging1 = "";
            string subqueryforpaging2 = "";
            string CreatedDateQuery = "";
            string AppointmentDateQuery = "";
            string ReportTypeQuery = "";
            string ReportQuery = "";
            string ReportColQuery = "";
            string ReportCountQuery = "";
            string filterquery = "";
            string ReportAgeQuery = "";
            string gobacksearchquery = "";
            string searchQueryupdated = "";
            string orderquery = "";
            string orderquery1 = "";
            string excelquery = "";
            string customerstatusquery = "";
            int? pagestart = (TicketFilters.PageNo - 1) * TicketFilters.PageSize;
            int? pageend = TicketFilters.PageSize;
            if (!string.IsNullOrWhiteSpace(TicketFilters.SearchText))
            {
                searchQueryupdated = string.Format("and ticket.id like '%{0}%' or cus.id like '%{0}%'", TicketFilters.SearchText);
            }

            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }

            #region TicketStatus
            if (!string.IsNullOrWhiteSpace(TicketFilters.TicketStatus)
                && TicketFilters.TicketStatus != "-1" && TicketFilters.TicketStatus != "null")
            {
                ticketStatusQuery = string.Format("and ticket.[Status] in ('{0}')", HttpUtility.UrlDecode(TicketFilters.TicketStatus));
            }
            #endregion

            #region CustomerStatus
            if (!string.IsNullOrWhiteSpace(TicketFilters.CustomerStatus)
                && TicketFilters.CustomerStatus != "-1" && TicketFilters.CustomerStatus != "null")
            {
                customerstatusquery = string.Format("and lkcusstatus.DataValue in ('{0}')", HttpUtility.UrlDecode(TicketFilters.CustomerStatus));
            }
            #endregion


            #region TicketType
            if (!string.IsNullOrWhiteSpace(TicketFilters.TicketType)
                && TicketFilters.TicketType != "-1" && TicketFilters.TicketType != "null")
            {
                ticketTypeQuery = string.Format("and ticket.TicketType in ('{0}')", HttpUtility.UrlDecode(TicketFilters.TicketType));
            }
            #endregion

            #region AppointmentDateQuery
            if (TicketFilters.StartDate != new DateTime() && TicketFilters.EndDate != new DateTime())
            {
                var StartDate = TicketFilters.StartDate.ToString("yyyy-MM-dd 00:00:00.000");
                var EndDate = TicketFilters.EndDate.ToString("yyyy-MM-dd 23:59:59.000");
                AppointmentDateQuery = string.Format("and ticket.CompletionDate between '{0}' and '{1}'", StartDate, EndDate);
            }
            #endregion

            #region Order
            if (!string.IsNullOrWhiteSpace(TicketFilters.order))
            {
                if (TicketFilters.order == "ascending/ticketid")
                {
                    orderquery = "order by #tdf.[Id] asc";
                    orderquery1 = "order by [Id] asc";
                }
                else if (TicketFilters.order == "descending/ticketid")
                {
                    orderquery = "order by #tdf.[Id] desc";
                    orderquery1 = "order by [Id] desc";
                }
                else if (TicketFilters.order == "ascending/customername")
                {
                    orderquery = "order by #tdf.CustomerName asc";
                    orderquery1 = "order by CustomerName asc";
                }
                else if (TicketFilters.order == "descending/customername")
                {
                    orderquery = "order by #tdf.CustomerName desc";
                    orderquery1 = "order by CustomerName desc";
                }
                else if (TicketFilters.order == "ascending/tickettype")
                {
                    orderquery = "order by #tdf.[TicketTypeVal] asc";
                    orderquery1 = "order by [TicketTypeVal] asc";
                }
                else if (TicketFilters.order == "descending/tickettype")
                {
                    orderquery = "order by #tdf.[TicketTypeVal] desc";
                    orderquery1 = "order by [TicketTypeVal] desc";
                }
                else if (TicketFilters.order == "ascending/appointmentdate")
                {
                    orderquery = "order by #tdf.[CompletionDate] asc";
                    orderquery1 = "order by [CompletionDate] asc";
                }
                else if (TicketFilters.order == "descending/appointmentdate")
                {
                    orderquery = "order by #tdf.[CompletionDate] desc";
                    orderquery1 = "order by [CompletionDate] desc";
                }
                else if (TicketFilters.order == "ascending/technician")
                {
                    orderquery = "order by #tdf.[AssignedTo] asc";
                    orderquery1 = "order by [AssignedTo] asc";
                }
                else if (TicketFilters.order == "descending/technician")
                {
                    orderquery = "order by #tdf.[AssignedTo] desc";
                    orderquery1 = "order by [AssignedTo] desc";
                }
                else if (TicketFilters.order == "ascending/installdate")
                {
                    orderquery = "order by #tdf.[InstallDate]  asc";
                    orderquery1 = "order by InstallDate asc";
                }
                else if (TicketFilters.order == "descending/installdate")
                {
                    orderquery = "order by #tdf.[InstallDate]  desc";
                    orderquery1 = "order by InstallDate desc";
                }
                else if (TicketFilters.order == "ascending/leadsource")
                {
                    orderquery = "order by #tdf.[LeadSource]  asc";
                    orderquery1 = "order by LeadSource asc";
                }
                else if (TicketFilters.order == "descending/leadsource")
                {
                    orderquery = "order by #tdf.[LeadSource]  desc";
                    orderquery1 = "order by LeadSource desc";
                }
                else if (TicketFilters.order == "ascending/saleslocation")
                {
                    orderquery = "order by #tdf.[CusSalesLoc]  asc";
                    orderquery1 = "order by CusSalesLoc asc";
                }
                else if (TicketFilters.order == "descending/saleslocation")
                {
                    orderquery = "order by #tdf.[CusSalesLoc]  desc";
                    orderquery1 = "order by CusSalesLoc desc";
                }
                else if (TicketFilters.order == "ascending/salesperson")
                {
                    orderquery = "order by #tdf.[CusSalesPerson]  asc";
                    orderquery1 = "order by CusSalesPerson asc";
                }
                else if (TicketFilters.order == "descending/salesperson")
                {
                    orderquery = "order by #tdf.[CusSalesPerson]  desc";
                    orderquery1 = "order by CusSalesPerson desc";
                }
                else if (TicketFilters.order == "ascending/RMR")
                {
                    orderquery = "order by #tdf.[RMRAmount]  asc";
                    orderquery1 = "order by RMRAmount asc";
                }
                else if (TicketFilters.order == "descending/RMR")
                {
                    orderquery = "order by #tdf.[RMRAmount]  desc";
                    orderquery1 = "order by RMRAmount desc";
                }

                else
                {
                    orderquery = "order by #tdf.[Id]  desc";
                    orderquery1 = "order by Id desc";
                }

            }
            else
            {
                orderquery = "order by #tdf.[Id] desc";
                orderquery1 = "order by Id desc";
            }
            #endregion

            #region Filter Query
            if (!string.IsNullOrWhiteSpace(filter.id))
            {
                filterquery += string.Format("and ticket.Id = '{0}'", filter.id);
            }
            if (!string.IsNullOrWhiteSpace(filter.cusid))
            {
                filterquery += string.Format("and cus.Id = '{0}'", filter.cusid);
            }
            if (!string.IsNullOrWhiteSpace(filter.user) && filter.user != "-1")
            {
                filterquery += string.Format("and cus.Soldby = '{0}'", filter.user);
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.AssignedUserTicket) && TicketFilters.AssignedUserTicket != "null")
            {
                filterquery += string.Format("and tuser.UserId in ('{0}')", TicketFilters.AssignedUserTicket);
            }
            if (!string.IsNullOrWhiteSpace(filter.convertmindate) && !string.IsNullOrWhiteSpace(filter.convertmaxdate))
            {
                var datemin = Convert.ToDateTime(filter.convertmindate);
                var date = Convert.ToDateTime(filter.convertmaxdate);
                filterquery += string.Format("and ticket.CompletionDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.convertmindate))
            {
                var date = Convert.ToDateTime(filter.convertmindate);
                filterquery += string.Format("and ticket.CompletionDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.convertmaxdate))
            {
                var date = Convert.ToDateTime(filter.convertmaxdate);
                filterquery += string.Format("and ticket.CompletionDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }

            if (!string.IsNullOrWhiteSpace(filter.createmindate) && !string.IsNullOrWhiteSpace(filter.createmaxdate))
            {
                DateTime datemin = Convert.ToDateTime(filter.createmindate);
                DateTime date = Convert.ToDateTime(filter.createmaxdate);
                filterquery += string.Format("and cus.SalesDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.createmindate))
            {
                var date = Convert.ToDateTime(filter.createmindate);
                filterquery += string.Format("and cus.SalesDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.createmaxdate))
            {
                var date = Convert.ToDateTime(filter.createmaxdate);
                filterquery += string.Format("and cus.SalesDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }

            if (!string.IsNullOrWhiteSpace(filter.transfermindate) && !string.IsNullOrWhiteSpace(filter.transfermaxdate))
            {
                var datemin = Convert.ToDateTime(filter.transfermindate);
                var date = Convert.ToDateTime(filter.transfermaxdate);
                filterquery += string.Format("and cus.InstallDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.transfermindate))
            {
                var date = Convert.ToDateTime(filter.transfermindate);
                filterquery += string.Format("and cus.InstallDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.transfermaxdate))
            {
                var date = Convert.ToDateTime(filter.transfermaxdate);
                filterquery += string.Format("and cus.InstallDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }


            if (!string.IsNullOrWhiteSpace(filter.ticketcreateddatemin) && !string.IsNullOrWhiteSpace(filter.ticketcreateddatemax))
            {
                var datemin = Convert.ToDateTime(filter.ticketcreateddatemin);
                var date = Convert.ToDateTime(filter.ticketcreateddatemax);
                filterquery += string.Format("and ticket.CreatedDate between '{0}' and '{1}'", datemin.SetZeroHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss.fff"), date.SetMaxHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.ticketcreateddatemin))
            {
                var date = Convert.ToDateTime(filter.ticketcreateddatemin);
                filterquery += string.Format("and ticket.CreatedDate between '{0}' and '{1}'", date.SetZeroHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss.fff"), date.SetMaxHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.ticketcreateddatemax))
            {
                var date = Convert.ToDateTime(filter.ticketcreateddatemax);
                filterquery += string.Format("and ticket.CreatedDate between '{0}' and '{1}'", date.SetZeroHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss.fff"), date.SetMaxHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            #endregion
            #region excel
            if (!string.IsNullOrWhiteSpace(filter.viewtype)
              && filter.viewtype == "excel")
            {
                excelquery = string.Format(" select Id as [Ticket Id], cusid as [Customer Id], CusNameOnly as [Customer Name], CustomerStatusVal as [Customer Status],CusBusinessName as [Business Name], LeadSource as [Lead Source], TicketTypeVal as [Ticket Type], CusSalesLoc as [Sales Location]" +
                    ",CusSalesPerson as [Sales Person], ResignByVal as [Resigned By] ,AssignedTo as [Installer] , cast(RMRAmount as decimal(12, 2)) as RMR ,StatusVal as [Ticket Status] , convert(date,CompletionDate) as [Appointment Date] , convert(date, SalesDate) as [Sales Date]" +
                    ", convert(date, InstallDate) as [Install Date] , convert(date, CreatedDate) as [Created Date]  from #TicketData   order by Id desc ");






            }
            #endregion
            #region paging
            if (!string.IsNullOrWhiteSpace(filter.viewtype)
              && filter.viewtype == "webview")
            {
                subqueryforpaging1 = string.Format(" select *,IDENTITY(INT, 1, 1) AS paginationid into #TicketDataFilter from #TicketData {0} " +
                    "SELECT TOP ({1}) #tdf.* into #TestTable FROM #TicketDataFilter #tdf where paginationid NOT IN(Select TOP ({2}) paginationid from #TicketDataFilter #tdf {3} ) {4} " +
                    "select  count(Id) as [TotalCount] from #TicketData where Id>0 and Id is not null and cusid>0 and cusid is not null and Id like '%%' or cusid like '%%'" +
                    "  select * from #TestTable " +
                    "select sum(CAST(RMRAmount as float)) as TotalRMR from #TestTable" +
                    " DROP TABLE #TicketDataFilter " +
                    "drop table #TestTable", searchQuery, TicketFilters.PageSize, pagestart, orderquery, orderquery1);
            }
            #endregion
            sqlQuery = @"   

 
                                select ticket.Id,
                                ticket.BookingId, 
                                ticket.TicketId, 
                                ticket.CustomerId, 
                                ticket.TicketType, 
                                ticket.[Status], 
                                ticket.[Priority], 
                                ticket.CreatedBy, 
                                ticket.CompanyId, 
                                ticket.CompletionDate, 
								cus.Id as cusid,
                                tuser.UserId as UserId,
                                cusext.ResignedBy,
                                ticket.CreatedDate into #TicketIdData from Ticket ticket
		                        left join TicketUser tuser on tuser.TiketId = ticket.TicketId
                                left join Customer cus on cus.CustomerId = ticket.CustomerId
                                left join CustomerExtended cusext on cusext.CustomerId = cus.CustomerId
                                where ticket.CompanyId = '{3}'
								and tuser.IsPrimary = 1
                                and cusext.IsTestAccount != 1

                                {6}
                                        {7}
                                        {8}                                       
                                        {13}
                                        {14}
                                        {19}
                                        {20}
		                                select 
                       

                                        ticket.*,
                                        CASE 
	                                        WHEN (cus.DBA = '' or cus.DBA IS NULL) AND  (cus.BusinessName = '' or cus.BusinessName IS NULL) THEN cus.FirstName +' '+cus.LastName
	                                        WHEN (cus.DBA = '' or cus.DBA IS NULL)  THEN cus.BusinessName
	                                        ELSE  cus.DBA
                                        END as CustomerName
                                        , cus.FirstName +' '+cus.LastName as CusNameOnly
                                        ,cus.SalesDate
										,cus.InstallDate
                                        ,(select count(id) from TicketFile where TicketId = ticket.TicketId) as AttachmentsCount
                                        ,(select count(id) from TicketFile where TicketId = ticket.TicketId)
		                                        + (select count(id) from TicketReply where TicketId = ticket.TicketId) as RepliesCount
                                        ,lktype.DisplayText as TicketTypeVal
                                        ,lkstatus.DisplayText as StatusVal
                                        ,lkpriority.DisplayText as PriorityVal
										,lkcusstatus.DisplayText as CustomerStatusVal
                                        ,emp.FirstName + ' '+emp.LastName as CreatedByVal
                                        ,(select CAST(firstname + ' '+LastName + ', ' AS VARCHAR(200))  from Employee  where UserId in (select UserId from TicketUser tulist where tulist.TiketId = ticket.TicketId and IsPrimary = 1) FOR XML PATH ('') ) as AssignedTo
                              
                                        ,cus.Id as CusIdInt
                                        ,(select sum(TotalAmount) from Invoice 
										    where bookingId = ticket.BookingId and bookingId != '' 
										        and (Status = 'Open' or Status = 'Partial' or Status ='Paid')) as BookingInvoiceAmount
                                        ,isnull(cus.BusinessName, '') as CusBusinessName,
                                        isnull(sales.FirstName + ' ' + sales.LastName, '') as CusSalesPerson,
                                        isnull(installer.FirstName + ' ' + installer.LastName, '') as CusInstaller,
                                         ISNULL(NULLIF(cus.MonthlyMonitoringFee, ''), 0) as RMRAmount,
                               
                                          LAG(lktype.DisplayText) OVER (ORDER BY ticket.Id) as PrevTicketType
										  ,LAG(ticket.CompletionDate) OVER (ORDER BY ticket.Id) as PrevAppointmentDate
										  ,LAG(emp.FirstName + ' '+emp.LastName) OVER (ORDER BY ticket.Id) as PrevTechnician
                                        ,lksalesloc.DisplayText as CusSalesLoc
										,lkleadsource.DisplayText as LeadSource 
                                        ,isnull(empResignedBy.FirstName + ' ' + empResignedBy.LastName, '') as ResignByVal
										into #TicketData
                                        from #TicketIdData ticket
                                        LEFT JOIN Customer cus on cus.CustomerId=ticket.CustomerId
                                        left join CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                        left join TicketUser tuser on tuser.TiketId = ticket.TicketId and tuser.IsPrimary = 1
                                        left join Lookup lktype on  lktype.DataKey ='TicketType'  
                                        and lktype.DataValue = ticket.TicketType

                                        left join CustomerAppointment CA on  CA.AppointmentId = ticket.TicketId

                                        left join Lookup lkStartTime on lkStartTime.DataKey = 'Arrival'
                                        and lkStartTime.DataValue = CA.AppointmentStartTime

                                        left join Lookup lkEndTime on lkEndTime.DataKey = 'Arrival'
                                        and lkEndTime.DataValue = CA.AppointmentEndTime

                                        left join Lookup lkstatus on  lkstatus.DataKey ='TicketStatus'  
                                        and lkstatus.DataValue = ticket.[Status]

                                        left join Lookup lkpriority on  lkpriority.DataKey ='TicketPriority'  
                                        and lkpriority.DataValue = ticket.[Priority]

                                        left join Lookup lksalesloc on  lksalesloc.DataKey ='CommissionType'  
                                        and lksalesloc.DataValue = iif(cus.SalesLocation != '-1', cus.SalesLocation, null)

										left join Lookup lkleadsource on  lkleadsource.DataKey ='LeadSource'  
                                        and lkleadsource.DataValue = iif(cus.LeadSource != '-1', cus.LeadSource, null)

			                            left join Lookup lkcusstatus on  lkcusstatus.DataKey ='CustomerStatus1'  
                                        and lkcusstatus.DataValue = cus.CustomerStatus

                                        left join Employee emp on emp.UserId = ticket.CreatedBy
                                        left join Employee sales on CONVERT(nvarchar(50), sales.UserId) = cus.Soldby
                                        left join Employee installer on CONVERT(nvarchar(50), installer.UserId) = cus.Soldby
                                         left join Employee empResignedBy on empResignedBy.UserId = ticket.ResignedBy

		                                where ticket.CompanyId = '{3}'
                                        and ce.IsTestAccount != 1
                                        and iif(cus.FirstName +' '+cus.LastName is null, cus.BusinessName, cus.DBA) is not null
								        and tuser.IsPrimary = 1

                                        {6}
                                        {7}
                                        {8}  
                                        {27}
                                        {13}
                                        {14}
                                        {19}
                                         {22}
                                        order by ticket.Id desc
                                          {25}
                                          {11}

                           
                                
                                DROP TABLE #TicketData
								drop table #TicketIdData
					
                                    ";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        TicketFilters.SearchText,//0
                                        TicketFilters.PageNo,  //1
                                        TicketFilters.PageSize, //2
                                        TicketFilters.CompanyId, //3
                                        TicketFilters.CustomerId, //4
                                        searchQuery,//5
                                        ticketStatusQuery,//6
                                        ticketTypeQuery,//7
                                        assignedQuery,//8
                                        CreatedByMeQuery,//9
                                        myTicketQuery,//10
                                        subqueryforpaging1,//11
                                        subqueryforpaging2,//12
                                                           //CreatedDateQuery,//13,
                                        AppointmentDateQuery,//13
                                        ReportTypeQuery,//14,
                                        ReportQuery,//15,
                                        ReportColQuery,//16
                                        ReportCountQuery,//17
                                        NameSql,
                                        filterquery,
                                        ReportAgeQuery,
                                        gobacksearchquery,
                                        searchQueryupdated,
                                        orderquery,//23
                                        orderquery1,//24
                                        excelquery,//25
                                        pagestart, //26
                                        customerstatusquery//27


                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetAllDateReferenceReport(TicketFilter TicketFilters, FilterReportModel filter)
        {
            string sqlQuery = @"";
            string searchQuery = "";
            string myTicketQuery = "";
            string ticketStatusQuery = "";
            string ticketTypeQuery = "";
            string assignedQuery = "";
            string CreatedByMeQuery = "";
            string subqueryforpaging1 = "";
            string subqueryforpaging2 = "";
            string CreatedDateQuery = "";
            string DateReferenceQuery = "";
            string ReportTypeQuery = "";
            string ReportQuery = "";
            string ReportColQuery = "";
            string ReportCountQuery = "";
            string filterquery = "";
            string ReportAgeQuery = "";
            string gobacksearchquery = "";
            string searchQueryupdated = "";
            string orderquery = "";
            string orderquery1 = "";
            string excelquery = "";
            int? pagestart = (TicketFilters.PageNo - 1) * TicketFilters.PageSize;
            int? pageend = TicketFilters.PageSize;
            if (!string.IsNullOrWhiteSpace(TicketFilters.SearchText))
            {
                searchQueryupdated = string.Format("and ticket.id like '%{0}%' or cus.id like '%{0}%'", TicketFilters.SearchText);
            }

            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }

            #region TicketStatus
            if (!string.IsNullOrWhiteSpace(TicketFilters.TicketStatus)
                && TicketFilters.TicketStatus != "-1" && TicketFilters.TicketStatus != "null")
            {
                ticketStatusQuery = string.Format("and ticket.[Status] in ('{0}')", HttpUtility.UrlDecode(TicketFilters.TicketStatus));
            }
            #endregion




            #region TicketType
            if (!string.IsNullOrWhiteSpace(TicketFilters.TicketType)
                && TicketFilters.TicketType != "-1" && TicketFilters.TicketType != "null")
            {
                ticketTypeQuery = string.Format("and ticket.TicketType in ('{0}')", HttpUtility.UrlDecode(TicketFilters.TicketType));
            }
            #endregion

            #region DateReferenceQuery
            if (TicketFilters.StartDate != new DateTime() && TicketFilters.EndDate != new DateTime())
            {
                var StartDate = TicketFilters.StartDate.ToString("yyyy-MM-dd 00:00:00.000");
                var EndDate = TicketFilters.EndDate.ToString("yyyy-MM-dd 23:59:59.000");
                DateReferenceQuery = string.Format("and ticket.CompletionDate between '{0}' and '{1}'", StartDate, EndDate);
            }
            #endregion

            #region Order
            if (!string.IsNullOrWhiteSpace(TicketFilters.order))
            {
                if (TicketFilters.order == "ascending/ticketid")
                {
                    orderquery = "order by #tdf.[Id] asc";
                    orderquery1 = "order by [Id] asc";
                }
                else if (TicketFilters.order == "descending/ticketid")
                {
                    orderquery = "order by #tdf.[Id] desc";
                    orderquery1 = "order by [Id] desc";
                }
                else if (TicketFilters.order == "ascending/customername")
                {
                    orderquery = "order by #tdf.CustomerName asc";
                    orderquery1 = "order by CustomerName asc";
                }
                else if (TicketFilters.order == "descending/customername")
                {
                    orderquery = "order by #tdf.CustomerName desc";
                    orderquery1 = "order by CustomerName desc";
                }
                else if (TicketFilters.order == "ascending/tickettype")
                {
                    orderquery = "order by #tdf.[TicketTypeVal] asc";
                    orderquery1 = "order by [TicketTypeVal] asc";
                }
                else if (TicketFilters.order == "descending/tickettype")
                {
                    orderquery = "order by #tdf.[TicketTypeVal] desc";
                    orderquery1 = "order by [TicketTypeVal] desc";
                }
                else if (TicketFilters.order == "ascending/appointmentdate")
                {
                    orderquery = "order by #tdf.[CompletionDate] asc";
                    orderquery1 = "order by [CompletionDate] asc";
                }
                else if (TicketFilters.order == "descending/appointmentdate")
                {
                    orderquery = "order by #tdf.[CompletionDate] desc";
                    orderquery1 = "order by [CompletionDate] desc";
                }
                else if (TicketFilters.order == "ascending/technician")
                {
                    orderquery = "order by #tdf.[AssignedTo] asc";
                    orderquery1 = "order by [AssignedTo] asc";
                }
                else if (TicketFilters.order == "descending/technician")
                {
                    orderquery = "order by #tdf.[AssignedTo] desc";
                    orderquery1 = "order by [AssignedTo] desc";
                }
                else if (TicketFilters.order == "ascending/installdate")
                {
                    orderquery = "order by #tdf.[InstallDate]  asc";
                    orderquery1 = "order by InstallDate asc";
                }
                else if (TicketFilters.order == "descending/installdate")
                {
                    orderquery = "order by #tdf.[InstallDate]  desc";
                    orderquery1 = "order by InstallDate desc";
                }
                else if (TicketFilters.order == "ascending/leadsource")
                {
                    orderquery = "order by #tdf.[LeadSource]  asc";
                    orderquery1 = "order by LeadSource asc";
                }
                else if (TicketFilters.order == "descending/leadsource")
                {
                    orderquery = "order by #tdf.[LeadSource]  desc";
                    orderquery1 = "order by LeadSource desc";
                }
                else if (TicketFilters.order == "ascending/saleslocation")
                {
                    orderquery = "order by #tdf.[CusSalesLoc]  asc";
                    orderquery1 = "order by CusSalesLoc asc";
                }
                else if (TicketFilters.order == "descending/saleslocation")
                {
                    orderquery = "order by #tdf.[CusSalesLoc]  desc";
                    orderquery1 = "order by CusSalesLoc desc";
                }
                else if (TicketFilters.order == "ascending/salesperson")
                {
                    orderquery = "order by #tdf.[CusSalesPerson]  asc";
                    orderquery1 = "order by CusSalesPerson asc";
                }
                else if (TicketFilters.order == "descending/salesperson")
                {
                    orderquery = "order by #tdf.[CusSalesPerson]  desc";
                    orderquery1 = "order by CusSalesPerson desc";
                }
                else if (TicketFilters.order == "ascending/RMR")
                {
                    orderquery = "order by #tdf.[RMRAmount]  asc";
                    orderquery1 = "order by RMRAmount asc";
                }
                else if (TicketFilters.order == "descending/RMR")
                {
                    orderquery = "order by #tdf.[RMRAmount]  desc";
                    orderquery1 = "order by RMRAmount desc";
                }

                else
                {
                    orderquery = "order by #tdf.[Id]  desc";
                    orderquery1 = "order by Id desc";
                }

            }
            else
            {
                orderquery = "order by #tdf.[Id] desc";
                orderquery1 = "order by Id desc";
            }
            #endregion

            #region Filter Query
            if (!string.IsNullOrWhiteSpace(filter.id))
            {
                filterquery += string.Format("and ticket.Id = '{0}'", filter.id);
            }
            if (!string.IsNullOrWhiteSpace(filter.cusid))
            {
                filterquery += string.Format("and cus.Id = '{0}'", filter.cusid);
            }
            if (!string.IsNullOrWhiteSpace(filter.user) && filter.user != "-1")
            {
                filterquery += string.Format("and cus.Soldby = '{0}'", filter.user);
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.AssignedUserTicket) && TicketFilters.AssignedUserTicket != "null")
            {
                filterquery += string.Format("and tuser.UserId in ('{0}')", TicketFilters.AssignedUserTicket);
            }
            if (!string.IsNullOrWhiteSpace(filter.convertmindate) && !string.IsNullOrWhiteSpace(filter.convertmaxdate))
            {
                var datemin = Convert.ToDateTime(filter.convertmindate);
                var date = Convert.ToDateTime(filter.convertmaxdate);
                filterquery += string.Format("and ticket.CompletionDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.convertmindate))
            {
                var date = Convert.ToDateTime(filter.convertmindate);
                filterquery += string.Format("and ticket.CompletionDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.convertmaxdate))
            {
                var date = Convert.ToDateTime(filter.convertmaxdate);
                filterquery += string.Format("and ticket.CompletionDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }

            if (!string.IsNullOrWhiteSpace(filter.createmindate) && !string.IsNullOrWhiteSpace(filter.createmaxdate))
            {
                DateTime datemin = Convert.ToDateTime(filter.createmindate);
                DateTime date = Convert.ToDateTime(filter.createmaxdate);
                filterquery += string.Format("and cus.SalesDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.createmindate))
            {
                var date = Convert.ToDateTime(filter.createmindate);
                filterquery += string.Format("and cus.SalesDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.createmaxdate))
            {
                var date = Convert.ToDateTime(filter.createmaxdate);
                filterquery += string.Format("and cus.SalesDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }

            if (!string.IsNullOrWhiteSpace(filter.transfermindate) && !string.IsNullOrWhiteSpace(filter.transfermaxdate))
            {
                var datemin = Convert.ToDateTime(filter.transfermindate);
                var date = Convert.ToDateTime(filter.transfermaxdate);
                filterquery += string.Format("and cus.InstallDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.transfermindate))
            {
                var date = Convert.ToDateTime(filter.transfermindate);
                filterquery += string.Format("and cus.InstallDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.transfermaxdate))
            {
                var date = Convert.ToDateTime(filter.transfermaxdate);
                filterquery += string.Format("and cus.InstallDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }


            if (!string.IsNullOrWhiteSpace(filter.ticketcreateddatemin) && !string.IsNullOrWhiteSpace(filter.ticketcreateddatemax))
            {
                var datemin = Convert.ToDateTime(filter.ticketcreateddatemin);
                var date = Convert.ToDateTime(filter.ticketcreateddatemax);
                filterquery += string.Format("and ticket.CreatedDate between '{0}' and '{1}'", datemin.SetZeroHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss.fff"), date.SetMaxHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.ticketcreateddatemin))
            {
                var date = Convert.ToDateTime(filter.ticketcreateddatemin);
                filterquery += string.Format("and ticket.CreatedDate between '{0}' and '{1}'", date.SetZeroHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss.fff"), date.SetMaxHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.ticketcreateddatemax))
            {
                var date = Convert.ToDateTime(filter.ticketcreateddatemax);
                filterquery += string.Format("and ticket.CreatedDate between '{0}' and '{1}'", date.SetZeroHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss.fff"), date.SetMaxHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            #endregion

            #region excel
            if (!string.IsNullOrWhiteSpace(filter.viewtype)
              && filter.viewtype == "excel")
            {
                excelquery = string.Format(" select Id as [Ticket Id], cusid as [Customer Id], CusNameOnly as [Customer Name], CusBusinessName as [Business Name], LeadSource as [Lead Source], TicketTypeVal as [Ticket Type], CusSalesLoc as [Sales Location]" +
                    ",CusSalesPerson as [Sales Person], ResignByVal as [Resigned By] ,AssignedTo as [Installer] , cast(RMRAmount as decimal(12, 2)) as RMR ,StatusVal as [Ticket Status] , convert(date,CompletionDate) as [Appointment Date] , convert(date, SalesDate) as [Sales Date]" +
                    ", convert(date, InstallDate) as [Install Date] , convert(date, CreatedDate) as [Created Date]  from #TicketData   order by Id desc ");






            }
            #endregion
            #region paging
            if (!string.IsNullOrWhiteSpace(filter.viewtype)
              && filter.viewtype == "webview")
            {
                subqueryforpaging1 = string.Format(" select *,IDENTITY(INT, 1, 1) AS paginationid into #TicketDataFilter from #TicketData {0} " +
                    "SELECT TOP ({1}) #tdf.* into #TestTable FROM #TicketDataFilter #tdf where paginationid NOT IN(Select TOP ({2}) paginationid from #TicketDataFilter #tdf {3} ) {4} " +
                    "select  count(Id) as [TotalCount] from #TicketData where Id>0 and Id is not null and cusid>0 and cusid is not null and Id like '%%' or cusid like '%%'" +
                    "  select * from #TestTable " +
                    "select sum(CAST(RMRAmount as float)) as TotalRMR from #TestTable" +
                    " DROP TABLE #TicketDataFilter " +
                    "drop table #TestTable", searchQuery, TicketFilters.PageSize, pagestart, orderquery, orderquery1);
            }
            #endregion
            sqlQuery = @"   

 
                                select ticket.Id,
                                ticket.BookingId, 
                                ticket.TicketId, 
                                ticket.CustomerId, 
                                ticket.TicketType, 
                                ticket.[Status], 
                                ticket.[Priority], 
                                ticket.CreatedBy, 
                                ticket.CompanyId, 
                                ticket.CompletionDate, 
								cus.Id as cusid,
                                tuser.UserId as UserId,
                                cusext.ResignedBy,
                                ticket.CreatedDate into #TicketIdData from Ticket ticket
		                        left join TicketUser tuser on tuser.TiketId = ticket.TicketId
                                left join Customer cus on cus.CustomerId = ticket.CustomerId
                                left join CustomerExtended cusext on cusext.CustomerId = cus.CustomerId
                                where ticket.CompanyId = '{3}'
								and tuser.IsPrimary = 1
                                and cusext.IsTestAccount != 1

                                {6}
                                        {7}
                                        {8}                                       
                                        {13}
                                        {14}
                                        {19}
                                        {20}
		                                select 
                       

                                        ticket.*,
                                        CASE 
	                                        WHEN (cus.DBA = '' or cus.DBA IS NULL) AND  (cus.BusinessName = '' or cus.BusinessName IS NULL) THEN cus.FirstName +' '+cus.LastName
	                                        WHEN (cus.DBA = '' or cus.DBA IS NULL)  THEN cus.BusinessName
	                                        ELSE  cus.DBA
                                        END as CustomerName
                                        , cus.FirstName +' '+cus.LastName as CusNameOnly

                                        ,cus.SalesDate
										,cus.InstallDate
                                        ,(select count(id) from TicketFile where TicketId = ticket.TicketId) as AttachmentsCount
                                        ,(select count(id) from TicketFile where TicketId = ticket.TicketId)
		                                        + (select count(id) from TicketReply where TicketId = ticket.TicketId) as RepliesCount
                                        ,lktype.DisplayText as TicketTypeVal
                                        ,lkstatus.DisplayText as StatusVal
                                        ,lkpriority.DisplayText as PriorityVal
                                        ,emp.FirstName + ' '+emp.LastName as CreatedByVal
                                        ,(select CAST(firstname + ' '+LastName + ', ' AS VARCHAR(200))  from Employee  where UserId in (select UserId from TicketUser tulist where tulist.TiketId = ticket.TicketId and IsPrimary = 1) FOR XML PATH ('') ) as AssignedTo
                              
                                        ,cus.Id as CusIdInt
                                        ,(select sum(TotalAmount) from Invoice 
										    where bookingId = ticket.BookingId and bookingId != '' 
										        and (Status = 'Open' or Status = 'Partial' or Status ='Paid')) as BookingInvoiceAmount
                                        ,isnull(cus.BusinessName, '') as CusBusinessName,
                                        isnull(sales.FirstName + ' ' + sales.LastName, '') as CusSalesPerson,
                                        isnull(installer.FirstName + ' ' + installer.LastName, '') as CusInstaller,
                                        ISNULL(NULLIF(cus.MonthlyMonitoringFee, ''), 0) as RMRAmount,
                               
                                          LAG(lktype.DisplayText) OVER (ORDER BY ticket.Id) as PrevTicketType
										  ,LAG(ticket.CompletionDate) OVER (ORDER BY ticket.Id) as PrevAppointmentDate
										  ,LAG(emp.FirstName + ' '+emp.LastName) OVER (ORDER BY ticket.Id) as PrevTechnician
                                        ,lksalesloc.DisplayText as CusSalesLoc
										,lkleadsource.DisplayText as LeadSource
                                        ,isnull(empResignedBy.FirstName + ' ' + empResignedBy.LastName, '') as ResignByVal
										into #TicketData
                                        from #TicketIdData ticket
                                        LEFT JOIN Customer cus on cus.CustomerId=ticket.CustomerId
                                        left join CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                        left join TicketUser tuser on tuser.TiketId = ticket.TicketId and tuser.IsPrimary = 1
                                        left join Lookup lktype on  lktype.DataKey ='TicketType'  
                                        and lktype.DataValue = ticket.TicketType

                                        left join CustomerAppointment CA on  CA.AppointmentId = ticket.TicketId

                                        left join Lookup lkStartTime on lkStartTime.DataKey = 'Arrival'
                                        and lkStartTime.DataValue = CA.AppointmentStartTime

                                        left join Lookup lkEndTime on lkEndTime.DataKey = 'Arrival'
                                        and lkEndTime.DataValue = CA.AppointmentEndTime

                                        left join Lookup lkstatus on  lkstatus.DataKey ='TicketStatus'  
                                        and lkstatus.DataValue = ticket.[Status]

                                        left join Lookup lkpriority on  lkpriority.DataKey ='TicketPriority'  
                                        and lkpriority.DataValue = ticket.[Priority]

                                        left join Lookup lksalesloc on  lksalesloc.DataKey ='CommissionType'  
                                        and lksalesloc.DataValue = iif(cus.SalesLocation != '-1', cus.SalesLocation, null)

										left join Lookup lkleadsource on  lkleadsource.DataKey ='LeadSource'  
                                        and lkleadsource.DataValue = iif(cus.LeadSource != '-1', cus.LeadSource, null)

                                        left join Employee emp on emp.UserId = ticket.CreatedBy
                                        left join Employee sales on CONVERT(nvarchar(50), sales.UserId) = cus.Soldby
                                        left join Employee installer on CONVERT(nvarchar(50), installer.UserId) = cus.Soldby
                                        left join Employee empResignedBy on empResignedBy.UserId = ticket.ResignedBy

		                                where ticket.CompanyId = '{3}'
                                        and ce.IsTestAccount != 1
                                        and iif(cus.FirstName +' '+cus.LastName is null, cus.BusinessName, cus.DBA) is not null
								        and tuser.IsPrimary = 1

                                        {6}
                                        {7}
                                        {8}                               
                                        {13}
                                        {14}
                                        {19}
                                         {22}
                                        order by ticket.Id desc
                                          {25}
                                          {11}

                           
                                
                                DROP TABLE #TicketData
								drop table #TicketIdData
					
                                    ";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        TicketFilters.SearchText,//0
                                        TicketFilters.PageNo,  //1
                                        TicketFilters.PageSize, //2
                                        TicketFilters.CompanyId, //3
                                        TicketFilters.CustomerId, //4
                                        searchQuery,//5
                                        ticketStatusQuery,//6
                                        ticketTypeQuery,//7
                                        assignedQuery,//8
                                        CreatedByMeQuery,//9
                                        myTicketQuery,//10
                                        subqueryforpaging1,//11
                                        subqueryforpaging2,//12
                                                           //CreatedDateQuery,//13,
                                        DateReferenceQuery,//13
                                        ReportTypeQuery,//14,
                                        ReportQuery,//15,
                                        ReportColQuery,//16
                                        ReportCountQuery,//17
                                        NameSql,
                                        filterquery,
                                        ReportAgeQuery,
                                        gobacksearchquery,
                                        searchQueryupdated,
                                        orderquery,//23
                                        orderquery1,//24
                                        excelquery,//25
                                        pagestart //26



                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetAllGoBackReport(TicketFilter TicketFilters, FilterReportModel filter)
        {
            string sqlQuery = @"";
            string searchQuery = "";
            string myTicketQuery = "";
            string ticketStatusQuery = "";
            string ticketTypeQuery = "";
            string assignedQuery = "";
            string CreatedByMeQuery = "";
            string subqueryforpaging1 = "";
            string subqueryforpaging2 = "";
            string CreatedDateQuery = "";
            string GoBackQuery = "";
            string ReportTypeQuery = "";
            string ReportQuery = "";
            string ReportColQuery = "";
            string ReportCountQuery = "";
            string filterquery = "";
            string ReportAgeQuery = "";
            string gobacksearchquery = "";
            string searchQueryupdated = "";
            string orderquery = "";
            string orderquery1 = "";
            string excelquery = "";
            int? pagestart = (TicketFilters.PageNo - 1) * TicketFilters.PageSize;
            int? pageend = TicketFilters.PageSize;
            if (!string.IsNullOrWhiteSpace(TicketFilters.SearchText))
            {
                searchQueryupdated = string.Format("and ticket.id like '%{0}%' or cus.id like '%{0}%'", TicketFilters.SearchText);
            }

            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }

            #region TicketStatus
            if (!string.IsNullOrWhiteSpace(TicketFilters.TicketStatus)
                && TicketFilters.TicketStatus != "-1" && TicketFilters.TicketStatus != "null")
            {
                ticketStatusQuery = string.Format("and ticket.[Status] in ('{0}')", HttpUtility.UrlDecode(TicketFilters.TicketStatus));
            }
            #endregion

            #region TicketType
            if (!string.IsNullOrWhiteSpace(TicketFilters.TicketType) && TicketFilters.TicketType != "-1" && TicketFilters.TicketType != "null")
            {
                ticketTypeQuery = string.Format("and ticket.TicketType in ('{0}')", HttpUtility.UrlDecode(TicketFilters.TicketType));
            }
            #endregion

            #region GoBackQuery
            if (TicketFilters.StartDate != new DateTime() && TicketFilters.EndDate != new DateTime())
            {
                var StartDate = TicketFilters.StartDate.ToString("yyyy-MM-dd 00:00:00.000");
                var EndDate = TicketFilters.EndDate.ToString("yyyy-MM-dd 23:59:59.000");
                GoBackQuery = string.Format("and ticket.CompletionDate between '{0}' and '{1}'", StartDate, EndDate);
            }
            #endregion

            #region Order
            if (!string.IsNullOrWhiteSpace(TicketFilters.order))
            {
                if (TicketFilters.order == "ascending/ticketid")
                {
                    orderquery = "order by #tdf.[Id] asc";
                    orderquery1 = "order by [Id] asc";
                }
                else if (TicketFilters.order == "descending/ticketid")
                {
                    orderquery = "order by #tdf.[Id] desc";
                    orderquery1 = "order by [Id] desc";
                }
                else if (TicketFilters.order == "ascending/customername")
                {
                    orderquery = "order by #tdf.CustomerName asc";
                    orderquery1 = "order by CustomerName asc";
                }
                else if (TicketFilters.order == "descending/customername")
                {
                    orderquery = "order by #tdf.CustomerName desc";
                    orderquery1 = "order by CustomerName desc";
                }
                else if (TicketFilters.order == "ascending/tickettype")
                {
                    orderquery = "order by #tdf.[TicketTypeVal] asc";
                    orderquery1 = "order by [TicketTypeVal] asc";
                }
                else if (TicketFilters.order == "descending/tickettype")
                {
                    orderquery = "order by #tdf.[TicketTypeVal] desc";
                    orderquery1 = "order by [TicketTypeVal] desc";
                }
                else if (TicketFilters.order == "ascending/appointmentdate")
                {
                    orderquery = "order by #tdf.[CompletionDate] asc";
                    orderquery1 = "order by [CompletionDate] asc";
                }
                else if (TicketFilters.order == "descending/appointmentdate")
                {
                    orderquery = "order by #tdf.[CompletionDate] desc";
                    orderquery1 = "order by [CompletionDate] desc";
                }
                else if (TicketFilters.order == "ascending/technician")
                {
                    orderquery = "order by #tdf.[AssignedTo] asc";
                    orderquery1 = "order by [AssignedTo] asc";
                }
                else if (TicketFilters.order == "descending/technician")
                {
                    orderquery = "order by #tdf.[AssignedTo] desc";
                    orderquery1 = "order by [AssignedTo] desc";
                }
                else if (TicketFilters.order == "ascending/installdate")
                {
                    orderquery = "order by #tdf.[InstallDate]  asc";
                    orderquery1 = "order by InstallDate asc";
                }
                else if (TicketFilters.order == "descending/installdate")
                {
                    orderquery = "order by #tdf.[InstallDate]  desc";
                    orderquery1 = "order by InstallDate desc";
                }
                else if (TicketFilters.order == "ascending/leadsource")
                {
                    orderquery = "order by #tdf.[LeadSource]  asc";
                    orderquery1 = "order by LeadSource asc";
                }
                else if (TicketFilters.order == "descending/leadsource")
                {
                    orderquery = "order by #tdf.[LeadSource]  desc";
                    orderquery1 = "order by LeadSource desc";
                }
                else if (TicketFilters.order == "ascending/saleslocation")
                {
                    orderquery = "order by #tdf.[CusSalesLoc]  asc";
                    orderquery1 = "order by CusSalesLoc asc";
                }
                else if (TicketFilters.order == "descending/saleslocation")
                {
                    orderquery = "order by #tdf.[CusSalesLoc]  desc";
                    orderquery1 = "order by CusSalesLoc desc";
                }
                else if (TicketFilters.order == "ascending/salesperson")
                {
                    orderquery = "order by #tdf.[CusSalesPerson]  asc";
                    orderquery1 = "order by CusSalesPerson asc";
                }
                else if (TicketFilters.order == "descending/salesperson")
                {
                    orderquery = "order by #tdf.[CusSalesPerson]  desc";
                    orderquery1 = "order by CusSalesPerson desc";
                }
                else if (TicketFilters.order == "ascending/RMR")
                {
                    orderquery = "order by #tdf.[RMRAmount]  asc";
                    orderquery1 = "order by RMRAmount asc";
                }
                else if (TicketFilters.order == "descending/RMR")
                {
                    orderquery = "order by #tdf.[RMRAmount]  desc";
                    orderquery1 = "order by RMRAmount desc";
                }

                else
                {
                    orderquery = "order by #tdf.[Id]  desc";
                    orderquery1 = "order by Id desc";
                }

            }
            else
            {
                orderquery = "order by #tdf.[Id] desc";
                orderquery1 = "order by Id desc";
            }
            #endregion

            #region Filter Query
            if (!string.IsNullOrWhiteSpace(filter.id))
            {
                filterquery += string.Format("and ticket.Id = '{0}'", filter.id);
            }
            if (!string.IsNullOrWhiteSpace(filter.cusid))
            {
                filterquery += string.Format("and cus.Id = '{0}'", filter.cusid);
            }
            if (!string.IsNullOrWhiteSpace(filter.user) && filter.user != "-1")
            {
                filterquery += string.Format("and cus.Soldby = '{0}'", filter.user);
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.AssignedUserTicket) && TicketFilters.AssignedUserTicket != "null")
            {
                filterquery += string.Format("and tuser.UserId in ('{0}')", TicketFilters.AssignedUserTicket);
            }
            if (!string.IsNullOrWhiteSpace(filter.convertmindate) && !string.IsNullOrWhiteSpace(filter.convertmaxdate))
            {
                var datemin = Convert.ToDateTime(filter.convertmindate);
                var date = Convert.ToDateTime(filter.convertmaxdate);
                filterquery += string.Format("and ticket.CompletionDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.convertmindate))
            {
                var date = Convert.ToDateTime(filter.convertmindate);
                filterquery += string.Format("and ticket.CompletionDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.convertmaxdate))
            {
                var date = Convert.ToDateTime(filter.convertmaxdate);
                filterquery += string.Format("and ticket.CompletionDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }

            if (!string.IsNullOrWhiteSpace(filter.createmindate) && !string.IsNullOrWhiteSpace(filter.createmaxdate))
            {
                DateTime datemin = Convert.ToDateTime(filter.createmindate);
                DateTime date = Convert.ToDateTime(filter.createmaxdate);
                filterquery += string.Format("and cus.SalesDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.createmindate))
            {
                var date = Convert.ToDateTime(filter.createmindate);
                filterquery += string.Format("and cus.SalesDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.createmaxdate))
            {
                var date = Convert.ToDateTime(filter.createmaxdate);
                filterquery += string.Format("and cus.SalesDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }

            if (!string.IsNullOrWhiteSpace(filter.transfermindate) && !string.IsNullOrWhiteSpace(filter.transfermaxdate))
            {
                var datemin = Convert.ToDateTime(filter.transfermindate);
                var date = Convert.ToDateTime(filter.transfermaxdate);
                filterquery += string.Format("and cus.InstallDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.transfermindate))
            {
                var date = Convert.ToDateTime(filter.transfermindate);
                filterquery += string.Format("and cus.InstallDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.transfermaxdate))
            {
                var date = Convert.ToDateTime(filter.transfermaxdate);
                filterquery += string.Format("and cus.InstallDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            }

            #endregion
            #region excel
            if (!string.IsNullOrWhiteSpace(filter.viewtype)
             && filter.viewtype == "excel")
            {
                excelquery = string.Format(" select Id [Ticket Id],CusIdInt [CustomerId],CusNameOnly [Customer Name],CusBusinessName [Business Name],TicketTypeVal [Ticket Type], PrevTicketType [Prev Ticket Type] , convert(date,PrevAppointmentDate) [Prev AppointmentDate],convert(date,CompletionDate) [Appointment Date]" +
                    ",PrevTechnician, PrevTicketId [Prev Ticket Id], StatusVal [Ticket Status],Technician,convert(date,SalesDate) [Sales Date], convert(date,InstallDate) [InstallDate]" +
                    ", [Follow up From], [Reschedule From]  from #TicketData where convert(date, PrevAppointmentDate) between DATEADD(day, -90, CAST(CompletionDate AS DATE)) and  DATEADD(day, 1, CAST(CompletionDate AS DATE)) and PrevAgemnaiStatus = 0  order by Id desc ");






            }
            #endregion
            #region paging
            if (!string.IsNullOrWhiteSpace(filter.viewtype)
              && filter.viewtype == "webview")
            {
                subqueryforpaging1 = string.Format(" select *,IDENTITY(INT, 1, 1) AS paginationid into #TicketDataFilter from #TicketData {0} where convert(date, PrevAppointmentDate) between DATEADD(day, -90, CAST(CompletionDate AS DATE)) and  DATEADD(day, 1, CAST(CompletionDate AS DATE)) and PrevAgemnaiStatus = 0" +
                    "SELECT TOP ({1}) #tdf.* into #TestTable FROM #TicketDataFilter #tdf where paginationid NOT IN(Select TOP ({2}) paginationid from #TicketDataFilter #tdf {3} ) {4} " +
                    "select  count(Id) as [TotalCount] from #TicketDataFilter where Id>0 and Id is not null and cusid>0 and cusid is not null and Id like '%%' or cusid like '%%'" +
                    "  select * from #TestTable " +
                    "select sum(RMRAmount) as TotalRMR from #TestTable" +
                    " DROP TABLE #TicketDataFilter " +
                    "drop table #TestTable", searchQuery, TicketFilters.PageSize, pagestart, orderquery, orderquery1);
            }
            #endregion
            sqlQuery = @"   

 
                                select ticket.Id,
                                ticket.BookingId, 
                                ticket.TicketId, 
                                ticket.CustomerId, 
                                ticket.TicketType, 
                                ticket.[Status], 
                                ticket.[Priority], 
                                ticket.CreatedBy, 
                                ticket.CompanyId, 
                                ticket.CompletionDate, 
								cus.Id as cusid,
                                tuser.UserId as UserId,
                                ticket.IsImportedTicket,
	              iif(ticket.ReferenceTicketId is not null and ticket.ReferenceTicketId > 0, ticket.ReferenceTicketId, '') as [Follow up From],
				  iif(ticket.RescheduleTicketId is not null and ticket.RescheduleTicketId > 0, ticket.RescheduleTicketId, '') as [Reschedule From],
                                ticket.CreatedDate into #TicketIdData from Ticket ticket
		                        left join TicketUser tuser on tuser.TiketId = ticket.TicketId
                                left join Customer cus on cus.CustomerId = ticket.CustomerId
                                left join CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                where ticket.CompanyId = '{3}'
								and tuser.IsPrimary = 1
                                and ce.IsTestAccount != 1
							    and ticket.IsImportedTicket = 0

                                --and convert(date, ticket.CompletionDate) between convert(date, dateadd(day, -90, getdate() + 1)) and convert(date, getdate() + 1)
                                {6}
                                        {7}
                                        {8}                                       
                                        {13}
                                        {19}
                                        {20}
		                                select 
                       

                                        ticket.*,
                                        CASE 
	                                        WHEN (cus.DBA = '' or cus.DBA IS NULL) AND  (cus.BusinessName = '' or cus.BusinessName IS NULL) THEN cus.FirstName +' '+cus.LastName
	                                        WHEN (cus.DBA = '' or cus.DBA IS NULL)  THEN cus.BusinessName
	                                        ELSE  cus.DBA
                                        END as CustomerName
                                        , cus.FirstName +' '+cus.LastName as CusNameOnly

                                        ,cus.SalesDate
										,cus.InstallDate
                                        ,(select count(id) from TicketFile where TicketId = ticket.TicketId) as AttachmentsCount
                                        ,(select count(id) from TicketFile where TicketId = ticket.TicketId)
		                                        + (select count(id) from TicketReply where TicketId = ticket.TicketId) as RepliesCount
                                        ,lktype.DisplayText as TicketTypeVal
                                        ,lkstatus.DisplayText as StatusVal
                                        ,lkpriority.DisplayText as PriorityVal
                                        ,emp.FirstName + ' '+emp.LastName as CreatedByVal
                                        ,(select CAST(firstname + ' '+LastName + ', ' AS VARCHAR(200))  from Employee  where UserId in (select UserId from TicketUser tulist where tulist.TiketId = ticket.TicketId and IsPrimary = 1) FOR XML PATH ('') ) as AssignedTo
                              
                                        ,cus.Id as CusIdInt
                                        ,(select sum(TotalAmount) from Invoice 
										    where bookingId = ticket.BookingId and bookingId != '' 
										        and (Status = 'Open' or Status = 'Partial' or Status ='Paid')) as BookingInvoiceAmount
                                        ,isnull(cus.BusinessName, '') as CusBusinessName,
                                        isnull(sales.FirstName + ' ' + sales.LastName, '') as CusSalesPerson,
                                        isnull(installer.FirstName + ' ' + installer.LastName, '') as CusInstaller,
                                        isnull((select SUM(cae.TotalPrice) from CustomerAppointmentEquipment cae
											LEFT JOIN Ticket tk on tk.TicketId=cae.AppointmentId
											LEFT JOIN equipment eqp on eqp.EquipmentId=cae.EquipmentId
											where tk.CustomerId=cus.CustomerId
											and cae.IsService=1 and (cae.IsDefaultService is NULL or cae.IsDefaultService=0) 
											and (cae.IsCopied is NULL or cae.IsCopied=0) and eqp.IsArbEnabled=1),0) as RMRAmount,
                               
                                       (select top(1) lktype.DisplayText from LookUp lktype left join Ticket tk on  lktype.DataKey ='TicketType'  
                                        and lktype.DataValue = tk.TicketType  where tk.CustomerId = cus.CustomerId and tk.Id < ticket.Id order by tk.Id desc) as PrevTicketType
				                      , (select top(1) tk.IsImportedTicket  from Ticket tk where tk.CustomerId = cus.CustomerId and tk.Id < ticket.Id order by tk.Id desc) as PrevAgemnaiStatus
                     				 , (select top(1) tk.Id  from Ticket tk where tk.CustomerId = cus.CustomerId and tk.Id < ticket.Id order by tk.Id desc) as PrevTicketId

							          , (select top(1) tk.CompletionDate  from Ticket tk where tk.CustomerId = cus.CustomerId and tk.Id < ticket.Id order by tk.Id desc) as PrevAppointmentDate
				                      ,(select empl.FirstName + ' ' + empl.LastName from Employee empl where empl.UserId in (select UserId from TicketUser tulist where tulist.TiketId = (select top(1) tk.TicketId  from Ticket tk where tk.CustomerId = cus.CustomerId and tk.Id < ticket.Id order by tk.Id desc) and IsPrimary = 1) FOR XML PATH ('')) as PrevTechnician
                                        ,(select CAST(firstname + ' '+LastName + ' ' AS VARCHAR(200))  from Employee  where UserId in (select UserId from TicketUser tulist where tulist.TiketId = ticket.TicketId and IsPrimary = 1) FOR XML PATH ('') ) as Technician

                                        ,lksalesloc.DisplayText as CusSalesLoc
										,lkleadsource.DisplayText as LeadSource 
										into #TicketData
                                        from #TicketIdData ticket
                                        LEFT JOIN Customer cus on cus.CustomerId=ticket.CustomerId
                                        left join CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                        left join TicketUser tuser on tuser.TiketId = ticket.TicketId and tuser.IsPrimary = 1
                                        left join Lookup lktype on  lktype.DataKey ='TicketType'  
                                        and lktype.DataValue = ticket.TicketType

                                        left join CustomerAppointment CA on  CA.AppointmentId = ticket.TicketId

                                        left join Lookup lkStartTime on lkStartTime.DataKey = 'Arrival'
                                        and lkStartTime.DataValue = CA.AppointmentStartTime

                                        left join Lookup lkEndTime on lkEndTime.DataKey = 'Arrival'
                                        and lkEndTime.DataValue = CA.AppointmentEndTime

                                        left join Lookup lkstatus on  lkstatus.DataKey ='TicketStatus'  
                                        and lkstatus.DataValue = ticket.[Status]

                                        left join Lookup lkpriority on  lkpriority.DataKey ='TicketPriority'  
                                        and lkpriority.DataValue = ticket.[Priority]

                                        left join Lookup lksalesloc on  lksalesloc.DataKey ='CommissionType'  
                                        and lksalesloc.DataValue = iif(cus.SalesLocation != '-1', cus.SalesLocation, null)

										left join Lookup lkleadsource on  lkleadsource.DataKey ='LeadSource'  
                                        and lkleadsource.DataValue = iif(cus.LeadSource != '-1', cus.LeadSource, null)

                                        left join Employee emp on emp.UserId = ticket.CreatedBy
                                        left join Employee sales on CONVERT(nvarchar(50), sales.UserId) = cus.Soldby
                                        left join Employee installer on CONVERT(nvarchar(50), installer.UserId) = cus.Soldby
                                        
		                                where ticket.CompanyId = '{3}'
                                        and ce.IsTestAccount != 1
                                        and iif(cus.FirstName +' '+cus.LastName is null, cus.BusinessName, cus.DBA) is not null
                                        --and convert(date, ticket.CompletionDate) between convert(date, dateadd(day, -90, getdate() + 1)) and convert(date, getdate() + 1)
								        and tuser.IsPrimary = 1
										and ticket.IsImportedTicket = 0

                                        {6}
                                        {7}
                                        {8}                               
                                        {13}
                                        {19}
                                         {22}
                                        order by ticket.Id desc
                                          {25}
                                          {11}

                           
                                
                                DROP TABLE #TicketData
								drop table #TicketIdData
					
                                    ";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        TicketFilters.SearchText,//0
                                        TicketFilters.PageNo,  //1
                                        TicketFilters.PageSize, //2
                                        TicketFilters.CompanyId, //3
                                        TicketFilters.CustomerId, //4
                                        searchQuery,//5
                                        ticketStatusQuery,//6
                                        ticketTypeQuery,//7
                                        assignedQuery,//8
                                        CreatedByMeQuery,//9
                                        myTicketQuery,//10
                                        subqueryforpaging1,//11
                                        subqueryforpaging2,//12
                                                           //CreatedDateQuery,//13,
                                        GoBackQuery,//13
                                        ReportTypeQuery,//14,
                                        ReportQuery,//15,
                                        ReportColQuery,//16
                                        ReportCountQuery,//17
                                        NameSql,
                                        filterquery,
                                        ReportAgeQuery,
                                        gobacksearchquery,
                                        searchQueryupdated,
                                        orderquery,//23
                                        orderquery1,//24
                                        excelquery,//25
                                        pagestart //26



                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    //DataSet dsResult = GetDataSet(cmd);
                    DataSet dsResult = GetDataSet(cmd,90);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return null;
            }
        }
        public DataSet GetTechnicianListByFilter(EmployeeFilter TicketFilters, FilterReportModel filter)
        {
            string sqlQuery = @"";
            string searchQuery = "";
            string myTicketQuery = "";
            string ticketStatusQuery = "";
            string ticketTypeQuery = "";
            string assignedQuery = "";
            string CreatedByMeQuery = "";
            string subquery = "";
            string subquery1 = "";
            string CreatedDateQuery = "";
            string AppointmentDateQuery = "";
            string ReportTypeQuery = "";
            string ReportQuery = "";
            string ReportColQuery = "";
            string ReportCountQuery = "";
            string filterquerycity = "";
            string filterquerystate = "";

            string ReportAgeQuery = "";
            if (TicketFilters.City == "null")
            {
                TicketFilters.City = TicketFilters.City.Substring(0, TicketFilters.City.Length - 4);

            }
            if (TicketFilters.State == "null")
            {
                TicketFilters.State = TicketFilters.State.Substring(0, TicketFilters.State.Length - 4);

            }

            if (!string.IsNullOrWhiteSpace(TicketFilters.City))
            {
                filterquerycity += string.Format("and lk.DataValue in ('{0}')", TicketFilters.City);
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.State))
            {
                filterquerystate += string.Format("and employee.State in ('{0}')", TicketFilters.State);
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.SearchText))
            {
                searchQuery = @" and ( FirstName  + ' ' + LastName ) like @SearchText ";
            }

            ReportCountQuery = string.Format("select  count(Id) as [TotalCount] from #employeedata");


            #region AppointmentDateQuery
            if (TicketFilters.StartDate != new DateTime() && TicketFilters.EndDate != new DateTime())
            {
                var StartDate = TicketFilters.StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                var EndDate = TicketFilters.EndDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                AppointmentDateQuery = string.Format("and employee.CreatedDate between '{0}' and '{1}'", StartDate, EndDate);
            }
            #endregion
            #region MyTicket
            //if (!string.IsNullOrWhiteSpace(TicketFilters.MyTicket))
            //{
            //    if (TicketFilters.MyTicket == "Created")
            //    {
            //        CreatedByMeQuery = string.Format("and ticket.CreatedByUid = '{0}'", TicketFilters.UserId);

            //    }
            //    else if (TicketFilters.MyTicket == "Assigned")
            //    {
            //        myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(ticket.TicketId,'{0}') = 1 ", TicketFilters.UserId);
            //    }
            //    else if (TicketFilters.MyTicket == "Both")
            //    {
            //        CreatedByMeQuery = string.Format("and ticket.CreatedByUid = '{0}'", TicketFilters.UserId);
            //        myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(ticket.TicketId,'{0}') = 1 ", TicketFilters.UserId);
            //    }
            //    else if (TicketFilters.MyTicket == "None")
            //    {
            //        CreatedByMeQuery = string.Format("and ticket.CreatedByUid != '{0}'", TicketFilters.UserId);
            //        myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(ticket.TicketId,'{0}') = 0 ", TicketFilters.UserId);

            //    }
            //}

            #endregion
            #region Order
            if (!string.IsNullOrWhiteSpace(TicketFilters.order))
            {
                if (TicketFilters.order == "ascending/technician")
                {
                    subquery = "order by #e.[Name] asc";
                    subquery1 = "order by [Name] asc";
                }
                else if (TicketFilters.order == "descending/technician")
                {
                    subquery = "order by #e.[Name] desc";
                    subquery1 = "order by [Name] desc";
                }
                else if (TicketFilters.order == "ascending/city")
                {
                    subquery = "order by #e.City asc";
                    subquery1 = "order by City asc";
                }
                else if (TicketFilters.order == "descending/city")
                {
                    subquery = "order by #e.City desc";
                    subquery1 = "order by City desc";
                }
                else if (TicketFilters.order == "ascending/state")
                {
                    subquery = "order by #e.[State] asc";
                    subquery1 = "order by [State] asc";
                }
                else if (TicketFilters.order == "descending/state")
                {
                    subquery = "order by #e.[State] desc";
                    subquery1 = "order by [State] desc";
                }
                else if (TicketFilters.order == "ascending/instlschedule")
                {
                    subquery = "order by #e.[InstallationsScheduled] asc";
                    subquery1 = "order by [InstallationsScheduled] asc";
                }
                else if (TicketFilters.order == "descending/instlschedule")
                {
                    subquery = "order by #e.[InstallationsScheduled] desc";
                    subquery1 = "order by [InstallationsScheduled] desc";
                }
                else if (TicketFilters.order == "ascending/instlcomplete")
                {
                    subquery = "order by #e.[Installationscomplete] asc";
                    subquery1 = "order by [Installationscomplete] asc";
                }
                else if (TicketFilters.order == "descending/instlcomplete")
                {
                    subquery = "order by #e.[Installationscomplete] desc";
                    subquery1 = "order by [Installationscomplete] desc";
                }
                else if (TicketFilters.order == "ascending/serviceschedule")
                {
                    subquery = "order by #e.[servicesscheduled]  asc";
                    subquery1 = "order by servicesscheduled asc";
                }
                else if (TicketFilters.order == "descending/serviceschedule")
                {
                    subquery = "order by #e.[servicesscheduled]  desc";
                    subquery1 = "order by servicesscheduled desc";
                }
                else if (TicketFilters.order == "descending/servicescomplete")
                {
                    subquery = "order by #e.[servicescomplete]  desc";
                    subquery1 = "order by servicescomplete desc";
                }
                else if (TicketFilters.order == "descending/servicescomplete")
                {
                    subquery = "order by #e.[servicescomplete]  desc";
                    subquery1 = "order by servicescomplete desc";
                }
                else
                {
                    subquery = "order by #e.[Id]  desc";
                    subquery1 = "order by Id desc";
                }

            }
            else
            {
                subquery = "order by #e.[Id] desc";
                subquery1 = "order by Id desc";
            }
            #endregion


            sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                DECLARE @CustomerId uniqueidentifier
                                DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int
                                DECLARE @SearchText nvarchar(50) 

                                SET @SearchText = '%{0}%' 
                                SET @pageno = {1} --default 1
                                SET @pagesize = {2} --default 10
                                SET @CompanyId = '{3}' --97BCF758-A482-47EB-82B8-F88BF12293FF
                                SET @CustomerId = '{4}'

                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

 
                                 select distinct employee.Id,
								(employee.FirstName+' '+employee.LastName) as Name,
								--lk.DisplayText as City,
								--lkstate.DisplayText as State,
                                employee.City,
								employee.State,
                                
                           (select COUNT(_TICKET.Id) from Ticket _TICKET left join TicketUser _ticketuser on _ticketuser.TiketId = _TICKET.TicketId where _TICKET.TicketType='Installation' and _TICKET.Status ='created' and _ticketuser.UserId = employee.UserId and _ticketuser.NotificationOnly = 0) as InstallationsScheduled,
						   (select COUNT(_TICKET.Id) from Ticket _TICKET left join TicketUser _ticketuser on _ticketuser.TiketId = _TICKET.TicketId where _TICKET.TicketType='Installation' and _TICKET.Status ='Completed' and _ticketuser.UserId = employee.UserId and _ticketuser.NotificationOnly = 0) as Installationscomplete,
						   (select COUNT(_TICKET.Id) from Ticket _TICKET left join TicketUser _ticketuser on _ticketuser.TiketId = _TICKET.TicketId where _TICKET.TicketType ='service' and _TICKET.Status ='created' and _ticketuser.UserId = employee.UserId and _ticketuser.NotificationOnly = 0) as servicesscheduled,
						   (select COUNT(_TICKET.Id) from Ticket _TICKET left join TicketUser _ticketuser on _ticketuser.TiketId = _TICKET.TicketId where _TICKET.TicketType ='service' and _TICKET.Status ='Completed' and _ticketuser.UserId = employee.UserId and _ticketuser.NotificationOnly = 0) as servicescomplete
                           
                           
                             into #employeedata from Employee employee
                                left join UserPermission up 
	                                on up.UserId = employee.UserId
                                left join PermissionGroup pg 
	                                on pg.Id = up.PermissionGroupId
                                --left join TicketUser ticketuser on employee.UserId = ticketuser.UserId
								--left join Ticket ticket on ticketuser.TiketId = ticket.TicketId
                                left join LookUp lk on lk.DataValue = employee.City  and lk.DataKey='USACity' 
                                --left join LookUp lkstate on lkstate.DataValue = employee.State  and lkstate.DataKey='StateList' 
                                where employee.CompanyId = @CompanyId and employee.IsCurrentEmployee=1 and pg.Id =5   {5} {13} {18} {19}
								order by employee.Id desc 
                               
                                select * into #employeeIddata from #employeedata
								select top(@pagesize) * into #Testtable from #employeeIddata 
								where Id not in (Select TOP (@pagestart) Id from #employeedata #e {11} )
                                {12}

                                select * from #Testtable
                                select sum(InstallationsScheduled) as TotalInstallationsScheduled,sum(Installationscomplete) as TotalInstallationscomplete, sum(servicesscheduled) as Totalservicesscheduled, sum(servicescomplete) as Totalservicescomplete from #Testtable
                              
                                  
                                  select  count(Id) as [TotalCount] from #employeeIddata
                      

						  drop table #employeedata
						  drop table #EmployeeIdData
                          drop table #Testtable ";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        TicketFilters.SearchText,//0
                                        TicketFilters.PageNo,  //1
                                        TicketFilters.PageSize, //2
                                        TicketFilters.CompanyId, //3
                                        TicketFilters.CustomerId, //4
                                        searchQuery,//5
                                        ticketStatusQuery,//6
                                        ticketTypeQuery,//7
                                        assignedQuery,//8
                                        CreatedByMeQuery,//9
                                        myTicketQuery,//10
                                        subquery,//11
                                        subquery1,//12
                                                  //CreatedDateQuery,//13,
                                        AppointmentDateQuery,//13
                                        ReportTypeQuery,//14,
                                        ReportQuery,//15,
                                        ReportColQuery,//16
                                        ReportCountQuery,//17

                                        filterquerycity,//18
                                        filterquerystate,//19
                                        ReportAgeQuery
                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetInstallationTicketListByFilter(TicketFilter TicketFilters, FilterReportModel filter)
        {
            string sqlQuery = @"";
            string searchQuery = "";
            string myTicketQuery = "";
            string ticketStatusQuery = "";
            string ticketTypeQuery = "";
            string assignedQuery = "";
            string CreatedByMeQuery = "";
            string subquery = "";
            string subquery1 = "";
            string CreatedDateQuery = "";
            string AppointmentDateQuery = "";
            string ReportTypeQuery = "";
            string ReportQuery = "";
            string ReportColQuery = "";
            string ReportCountQuery = "";
            string filterquery = "";
            string ReportAgeQuery = "";
            string filterquerysalesperson = "";
            string filterquerystatus = "";
            string filterqueryownership = "";

            if (TicketFilters.TicketStatus == "null")
            {
                TicketFilters.TicketStatus = TicketFilters.TicketStatus.Substring(0, TicketFilters.TicketStatus.Length - 4);

            }
            if (TicketFilters.OwnerShip == "null")
            {
                TicketFilters.OwnerShip = TicketFilters.OwnerShip.Substring(0, TicketFilters.OwnerShip.Length - 4);

            }
            var array = TicketFilters.TicketStatus.Split(",");
            string query = "";
            if (array != null)
            {
                foreach (var item in array)
                {
                    query += string.Format("'{0}',", item);
                }
                query = query.Remove(query.Length - 1, 1);
            }
            var arrayOwnership = TicketFilters.OwnerShip.Split(",");
            string queryOwnership = "";
            if (arrayOwnership != null)
            {
                foreach (var item in arrayOwnership)
                {
                    queryOwnership += string.Format("'{0}',", item);
                }
                queryOwnership = queryOwnership.Remove(queryOwnership.Length - 1, 1);
            }
            if (TicketFilters.salesperson == "null")
            {
                TicketFilters.salesperson = TicketFilters.salesperson.Substring(0, TicketFilters.salesperson.Length - 4);

            }


            if (!string.IsNullOrWhiteSpace(TicketFilters.salesperson))
            {
                filterquerysalesperson += string.Format("and sales.FirstName + ' ' + sales.LastName in ('{0}')", TicketFilters.salesperson);
            }
            if (!string.IsNullOrWhiteSpace(query) && query != "null")
            {
                filterquerystatus += string.Format("and ticket.Status in ({0})", query);
            }
            if (!string.IsNullOrWhiteSpace(queryOwnership) && queryOwnership != "null")
            {
                filterqueryownership += string.Format("and cus.Ownership in ({0})", queryOwnership);
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.SearchText))
            {
                searchQuery = @" where CustomerName like @SearchText or Status like @SearchText or CusIdInt like @SearchText";
            }
            //if (!string.IsNullOrWhiteSpace(TicketFilters.ReportTabType) && TicketFilters.ReportTabType == "GoBack")
            //{
            //    ReportTypeQuery = string.Format("and convert(date, ticket.CreatedDate) between '{0}' and '{1}'", TicketFilters.StartDate, TicketFilters.EndDate);
            //    ReportQuery = string.Format("and CountTicket > 1");
            //    ReportColQuery = string.Format("(select count(tik.Id) from ticket tik where convert(date, tik.CreatedDate) between '{0}' and '{1}' and tik.CustomerId=cus.CustomerId) as CountTicket", TicketFilters.StartDate, TicketFilters.EndDate);
            //    ReportCountQuery = string.Format("select  count(Id) as [TotalCount] from #TicketIdData");
            //    ReportAgeQuery = string.Format("and (select count(tik.Id) from ticket tik where convert(date, tik.CreatedDate) between '{0}' and '{1}' and tik.CustomerId=cus.CustomerId) > 1", TicketFilters.StartDate, TicketFilters.EndDate);
            //}
            //else
            //{
            ReportColQuery = string.Format("'' as CountTicket");
            ReportCountQuery = string.Format("select  count(Id) as [TotalCount] from #TicketDataFilter");
            //}
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }

            //#region TicketStatus
            //if (!string.IsNullOrWhiteSpace(TicketFilters.TicketStatus)
            //    && TicketFilters.TicketStatus != "-1" && TicketFilters.TicketStatus != "null")
            //{
            //    ticketStatusQuery = string.Format("and ticket.[Status] in ('{0}')", TicketFilters.TicketStatus);
            //}
            //#endregion

            //#region Assigned
            //if (!string.IsNullOrWhiteSpace(TicketFilters.AssignedUserTicket) && TicketFilters.AssignedUserTicket != "-1" && TicketFilters.AssignedUserTicket != "null" && TicketFilters.AssignedUserTicket != new Guid().ToString())
            //{
            //    assignedQuery = string.Format("and tuser.UserId in ('{0}')", TicketFilters.AssignedUserTicket);
            //}
            //#endregion


            //#region TicketType
            //if (!string.IsNullOrWhiteSpace(TicketFilters.TicketType)
            //    && TicketFilters.TicketType != "-1" && TicketFilters.TicketType != "null")
            //{
            //    ticketTypeQuery = string.Format("and ticket.TicketType in ('{0}')", TicketFilters.TicketType);
            //}
            //#endregion
            //#region CreatedDateQuery
            //if (TicketFilters.StartDate != new DateTime() && TicketFilters.EndDate != new DateTime())
            //{
            //    var StartDate = TicketFilters.StartDate.SetZeroHour().UTCToClientTime();
            //    var EndDate = TicketFilters.EndDate.SetMaxHour().UTCToClientTime();
            //    CreatedDateQuery = string.Format("and ticket.CreatedDate between '{0}' and '{1}'", StartDate, EndDate);
            //}
            //#endregion
            #region AppointmentDateQuery
            if (TicketFilters.StartDate != new DateTime() && TicketFilters.EndDate != new DateTime())
            {
                var StartDate = TicketFilters.StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                var EndDate = TicketFilters.EndDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                AppointmentDateQuery = string.Format("and ticket.CompletionDate between '{0}' and '{1}'", StartDate, EndDate);
            }
            #endregion
            #region MyTicket
            if (!string.IsNullOrWhiteSpace(TicketFilters.MyTicket))
            {
                if (TicketFilters.MyTicket == "Created")
                {
                    CreatedByMeQuery = string.Format("and ticket.CreatedByUid = '{0}'", TicketFilters.UserId);

                }
                else if (TicketFilters.MyTicket == "Assigned")
                {
                    myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(ticket.TicketId,'{0}') = 1 ", TicketFilters.UserId);
                }
                else if (TicketFilters.MyTicket == "Both")
                {
                    CreatedByMeQuery = string.Format("and ticket.CreatedByUid = '{0}'", TicketFilters.UserId);
                    myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(ticket.TicketId,'{0}') = 1 ", TicketFilters.UserId);
                }
                else if (TicketFilters.MyTicket == "None")
                {
                    CreatedByMeQuery = string.Format("and ticket.CreatedByUid != '{0}'", TicketFilters.UserId);
                    myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(ticket.TicketId,'{0}') = 0 ", TicketFilters.UserId);

                }
            }

            #endregion
            #region Order
            if (!string.IsNullOrWhiteSpace(TicketFilters.order))
            {
                if (TicketFilters.order == "ascending/customername")
                {
                    subquery = "order by #TicketData.[CustomerName] asc";
                    subquery1 = "order by [CustomerName] asc";
                }
                else if (TicketFilters.order == "descending/customername")
                {
                    subquery = "order by #TicketData.[CustomerName] desc";
                    subquery1 = "order by [CustomerName] desc";
                }
                else if (TicketFilters.order == "ascending/qa")
                {
                    subquery = "order by #TicketData.QA1 asc";
                    subquery1 = "order by QA1 asc";
                }
                else if (TicketFilters.order == "descending/qa")
                {
                    subquery = "order by #TicketData.QA1 desc";
                    subquery1 = "order by QA1 desc";
                }

                else if (TicketFilters.order == "ascending/salesperson")
                {
                    subquery = "order by #TicketData.[CusSalesPerson] asc";
                    subquery1 = "order by [CusSalesPerson] asc";
                }
                else if (TicketFilters.order == "descending/salesperson")
                {
                    subquery = "order by #TicketData.[CusSalesPerson] desc";
                    subquery1 = "order by [CusSalesPerson] desc";
                }
                else if (TicketFilters.order == "ascending/technician")
                {
                    subquery = "order by #TicketData.[AssignedTo] asc";
                    subquery1 = "order by [AssignedTo] asc";
                }
                else if (TicketFilters.order == "descending/technician")
                {
                    subquery = "order by #TicketData.[AssignedTo] desc";
                    subquery1 = "order by [AssignedTo] desc";
                }
                else if (TicketFilters.order == "ascending/install")
                {
                    subquery = "order by #TicketData.[CompletionDate] asc";
                    subquery1 = "order by [CompletionDate] asc";
                }
                else if (TicketFilters.order == "descending/install")
                {
                    subquery = "order by #TicketData.[CompletionDate] desc";
                    subquery1 = "order by [CompletionDate] desc";
                }
                else if (TicketFilters.order == "ascending/onsite")
                {
                    subquery = "order by #TicketData.[TechOnsiteDate] asc";
                    subquery1 = "order by [TechOnsiteDate] asc";
                }
                else if (TicketFilters.order == "descending/onsite")
                {
                    subquery = "order by #TicketData.[TechOnsiteDate] desc";
                    subquery1 = "order by [TechOnsiteDate] desc";
                }
                else if (TicketFilters.order == "ascending/signed")
                {
                    subquery = "order by #TicketData.[CustomerSignatureDate] asc";
                    subquery1 = "order by [CustomerSignatureDate] asc";
                }
                else if (TicketFilters.order == "descending/signed")
                {
                    subquery = "order by #TicketData.[CustomerSignatureDate] desc";
                    subquery1 = "order by [CustomerSignatureDate] desc";
                }
                else if (TicketFilters.order == "ascending/registered")
                {
                    subquery = "order by #TicketData.[Registered] asc";
                    subquery1 = "order by [Registered] asc";
                }
                else if (TicketFilters.order == "descending/registered")
                {
                    subquery = "order by #TicketData.[Registered] desc";
                    subquery1 = "order by [Registered] desc";
                }
                else if (TicketFilters.order == "ascending/accountonline")
                {
                    subquery = "order by #TicketData.[AccountOnlineDate] asc";
                    subquery1 = "order by [AccountOnlineDate] asc";
                }
                else if (TicketFilters.order == "descending/accountonline")
                {
                    subquery = "order by #TicketData.[AccountOnlineDate] desc";
                    subquery1 = "order by [AccountOnlineDate] desc";
                }
                else if (TicketFilters.order == "ascending/total")
                {
                    subquery = "order by #TicketData.[TotalCollectedAmount] asc";
                    subquery1 = "order by [TotalCollectedAmount] asc";
                }
                else if (TicketFilters.order == "descending/total")
                {
                    subquery = "order by #TicketData.[TotalCollectedAmount] desc";
                    subquery1 = "order by [TotalCollectedAmount] desc";
                }
                else if (TicketFilters.order == "ascending/leadsource")
                {
                    subquery = "order by #TicketData.[LeadSourceVal] asc";
                    subquery1 = "order by [LeadSourceVal] asc";
                }
                else if (TicketFilters.order == "descending/leadsource")
                {
                    subquery = "order by #TicketData.[LeadSourceVal] desc";
                    subquery1 = "order by [LeadSourceVal] desc";
                }
                else
                {
                    subquery = "order by #TicketData.[Id]  desc";
                    subquery1 = "order by Id desc";
                }

            }
            else
            {
                subquery = "order by #TicketData.[Id] desc";
                subquery1 = "order by Id desc";
            }
            #endregion

            //#region Filter Query
            //if (!string.IsNullOrWhiteSpace(filter.id))
            //{
            //    filterquery += string.Format("and ticket.Id = '{0}'", filter.id);
            //}
            //if (!string.IsNullOrWhiteSpace(filter.cusid))
            //{
            //    filterquery += string.Format("and cus.Id = '{0}'", filter.cusid);
            //}
            //if (!string.IsNullOrWhiteSpace(filter.user) && filter.user != "-1")
            //{
            //    filterquery += string.Format("and cus.Soldby = '{0}'", filter.user);
            //}
            //if (!string.IsNullOrWhiteSpace(filter.convertmindate) && !string.IsNullOrWhiteSpace(filter.convertmaxdate))
            //{
            //    var datemin = Convert.ToDateTime(filter.convertmindate);
            //    var date = Convert.ToDateTime(filter.convertmaxdate);
            //    filterquery += string.Format("and ticket.CompletionDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            //}
            //else if (!string.IsNullOrWhiteSpace(filter.convertmindate))
            //{
            //    var date = Convert.ToDateTime(filter.convertmindate);
            //    filterquery += string.Format("and ticket.CompletionDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            //}
            //else if (!string.IsNullOrWhiteSpace(filter.convertmaxdate))
            //{
            //    var date = Convert.ToDateTime(filter.convertmaxdate);
            //    filterquery += string.Format("and ticket.CompletionDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            //}
            //if (!string.IsNullOrWhiteSpace(filter.createmindate) && !string.IsNullOrWhiteSpace(filter.createmaxdate))
            //{
            //    var datemin = Convert.ToDateTime(filter.createmindate);
            //    var date = Convert.ToDateTime(filter.createmaxdate);
            //    filterquery += string.Format("and cus.SalesDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            //}
            //else if (!string.IsNullOrWhiteSpace(filter.createmindate))
            //{
            //    var date = Convert.ToDateTime(filter.createmindate);
            //    filterquery += string.Format("and cus.SalesDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            //}
            //else if (!string.IsNullOrWhiteSpace(filter.createmaxdate))
            //{
            //    var date = Convert.ToDateTime(filter.createmaxdate);
            //    filterquery += string.Format("and cus.SalesDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            //}
            //if (!string.IsNullOrWhiteSpace(filter.transfermindate) && !string.IsNullOrWhiteSpace(filter.transfermaxdate))
            //{
            //    var datemin = Convert.ToDateTime(filter.transfermindate);
            //    var date = Convert.ToDateTime(filter.transfermaxdate);
            //    filterquery += string.Format("and cus.InstallDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            //}
            //else if (!string.IsNullOrWhiteSpace(filter.transfermindate))
            //{
            //    var date = Convert.ToDateTime(filter.transfermindate);
            //    filterquery += string.Format("and cus.InstallDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            //}
            //else if (!string.IsNullOrWhiteSpace(filter.transfermaxdate))
            //{
            //    var date = Convert.ToDateTime(filter.transfermaxdate);
            //    filterquery += string.Format("and cus.InstallDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.000"));
            //}
            //#endregion
            sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                DECLARE @CustomerId uniqueidentifier
                                DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int
                                DECLARE @SearchText nvarchar(50) 

                                SET @SearchText = '%{0}%' 
                                SET @pageno = {1} --default 1
                                SET @pagesize = {2} --default 10
                                SET @CompanyId = '{3}' --97BCF758-A482-47EB-82B8-F88BF12293FF
                                SET @CustomerId = '{4}'

                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

 
                                select ticket.Id,
                                ticket.BookingId, 
                                ticket.TicketId, 
                                ticket.CustomerId, 
                                ticket.TicketType, 
                                ticket.[Status], 
                                --ticket.[Priority], 
                                ticket.CreatedBy, 
                                ticket.CompanyId, 
                                ticket.CompletionDate, 
                                ticket.TechOnsiteDate,
                                ticket.CompletedDate,
                                ticket.CreatedDate into #TicketIdData from Ticket ticket
                                left join Customer cus on cus.CustomerId = ticket.CustomerId
                                where ticket.CompanyId = @CompanyId and ticket.TicketType='Installation'
                                        {6}
                                        {7}
                                        {10}
		                                select  ticket.*,
                                        CASE 
	                                        WHEN (cus.DBA = '' or cus.DBA IS NULL) AND  (cus.BusinessName = '' or cus.BusinessName IS NULL) THEN cus.FirstName +' '+cus.LastName
	                                        WHEN (cus.DBA = '' or cus.DBA IS NULL)  THEN cus.BusinessName
	                                        ELSE  cus.DBA
                                        END as CustomerName
                                        ,iif((select count(qa1.Id) from QA1Script qa1 where qa1.CustomerId=ticket.CustomerId and qa1.IsCompleted=1) > 0 , 'Yes' , 'No') as QA1
										,iif((select count(qa2.Id) from QA2Script qa2 where qa2.CustomerId=ticket.CustomerId and qa2.IsCompleted=1) > 0 , 'Yes' , 'No') as QA2
                                        ,iif((cus.AlarmRefId != ' ' and cus.AlarmRefId is not null) or (cus.BrinksRefId != ' ' and cus.BrinksRefId is not null) or (cus.UCCRefId != ' ' and cus.UCCRefId is not null), 'Yes', 'No') as Registered
                                        --,cus.SalesDate
										,cus.InstallDate
                                        --,(select count(id) from TicketFile where TicketId = ticket.TicketId) as AttachmentsCount
                                        --,(select count(id) from TicketFile where TicketId = ticket.TicketId)
		                                        --+ (select count(id) from TicketReply where TicketId = ticket.TicketId) as RepliesCount
                                        --,lktype.DisplayText as TicketTypeVal
                                        --,lkstatus.DisplayText as StatusVal
                                        --,lkpriority.DisplayText as PriorityVal
                                        ,emp.FirstName + ' '+emp.LastName as CreatedByVal
                                        ,(select CAST(firstname + ' '+LastName + ', ' AS VARCHAR(200))  from Employee  where UserId in (select UserId from TicketUser tulist where tulist.TiketId = ticket.TicketId and IsPrimary = 1) FOR XML PATH ('') ) as AssignedTo
                                        --,(select CAST(firstname + ' '+LastName + ', ' AS VARCHAR(200))  from Employee  where UserId in (select UserId from TicketUser tualist where tualist.TiketId = ticket.TicketId and IsPrimary = 0) FOR XML PATH ('') ) as AdditionalMembers
	                                    --,lkStartTime.DisplayText as AppointmentStartTimeVal
                                        --,CA.AppointmentStartTime as AppointmentStartTime
                                        --,lkEndTime.DisplayText as AppointmentEndTimeVal
                                        --,CA.AppointmentEndTime as AppointmentEndTime
                                        --,(select COUNT(cae.Id)
                                        
										--from CustomerAppointmentEquipment cae
										--LEFT JOIN Ticket t on t.TicketId=cae.AppointmentId
										--LEFT JOIN TicketUser tu on tu.TiketId=t.TicketId and tu.IsPrimary=1
										--where cae.AppointmentId=CA.AppointmentId
                                        --AND cae.IsEquipmentRelease=0
										--AND cae.Quantity>(ISNULL((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=CAE.EquipmentId and Type='Add'  And invinner.TechnicianId=tu.UserId)-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=CAE.EquipmentId and Type='Release'  And invinner.TechnicianId=tu.UserId),0))) as ExceedQuantity,
                                        ,cus.Id as CusIdInt
                                        , iif(lk.DataValue='-1',' ',lk.DisplayText) as TicketStatus
                                        , iif(lkownership.DataValue = '-1', ' ', lkownership.DisplayText) as OwnerShip
										,iif(lksource.DataValue='-1',' ',lksource.DisplayText) as LeadSourceVal

                                        ,tpc.AccountOnlineDate
                                        ,iif((select top(1) cs.CreatedDate from CustomerSignature cs where cs.CustomerId=ticket.CustomerId and cs.Type='Agreement File') !=null , (select top(1) cs.CreatedDate from CustomerSignature cs where cs.CustomerId=ticket.CustomerId and cs.Type='Agreement File'), cus.CustomerSignatureDate) as CustomerSignatureDate
                                        ,(select sum(TotalAmount) from Invoice 
										    where CustomerId = ticket.CustomerId 
										        and (Status ='Paid')) as TotalCollectedAmount
                                        ,isnull(cus.BusinessName, '') as CusBusinessName,
                                        isnull(sales.FirstName + ' ' + sales.LastName, '') as CusSalesPerson,
                                        isnull(whoplaced.FirstName + ' ' + whoplaced.LastName, '') as WhoPlaced,
                                        isnull(installer.FirstName + ' ' + installer.LastName, '') as CusInstaller,
                                        --isnull((select SUM(cae.TotalPrice) from CustomerAppointmentEquipment cae
											--LEFT JOIN Ticket tk on tk.TicketId=cae.AppointmentId
											--LEFT JOIN equipment eqp on eqp.EquipmentId=cae.EquipmentId
											--where tk.CustomerId=cus.CustomerId
											--and cae.IsService=1 and (cae.IsDefaultService is NULL or cae.IsDefaultService=0) 
											--and (cae.IsCopied is NULL or cae.IsCopied=0) and eqp.IsArbEnabled=1),0) as RMRAmount,
                                        {11}
                                        --,LAG(lktype.DisplayText) OVER (ORDER BY ticket.Id) as PrevTicketType
										--,LAG(ticket.CompletionDate) OVER (ORDER BY ticket.Id) as PrevAppointmentDate
										--,LAG(emp.FirstName + ' '+emp.LastName) OVER (ORDER BY ticket.Id) as PrevTechnician
                                        --,lksalesloc.DisplayText as CusSalesLoc
										--,lkleadsource.DisplayText as LeadSource 
										into #TicketData
                                        from  ticket
                                        LEFT JOIN Customer cus on cus.CustomerId=ticket.CustomerId
                                        left join TicketUser tuser on tuser.TiketId = ticket.TicketId and tuser.IsPrimary = 1
                                        --left join Lookup lktype on  lktype.DataKey ='TicketType'  
                                        --and lktype.DataValue = ticket.TicketType

                                        --left join CustomerAppointment CA on  CA.AppointmentId = ticket.TicketId
                                          --left join BrinksCustomer bc on bc.CustomerId=ticket.CustomerId
                                          left join ThirdPartyCustomer tpc on tpc.CustomerId=ticket.CustomerId

                                        --left join Lookup lkStartTime on lkStartTime.DataKey = 'Arrival'
                                        --and lkStartTime.DataValue = CA.AppointmentStartTime

                                        --left join Lookup lkEndTime on lkEndTime.DataKey = 'Arrival'
                                        --and lkEndTime.DataValue = CA.AppointmentEndTime

                                        --left join Lookup lkstatus on  lkstatus.DataKey ='TicketStatus'  
                                        --and lkstatus.DataValue = ticket.[Status]

                                        --left join Lookup lkpriority on  lkpriority.DataKey ='TicketPriority'  
                                        --and lkpriority.DataValue = ticket.[Priority]

                                        --left join Lookup lksalesloc on  lksalesloc.DataKey ='CommissionType'  
                                        --and lksalesloc.DataValue = iif(cus.SalesLocation != '-1', cus.SalesLocation, null)

										--left join Lookup lkleadsource on  lkleadsource.DataKey ='LeadSource'  
                                        --and lkleadsource.DataValue = iif(cus.LeadSource != '-1', cus.LeadSource, null)

                                        left join Employee emp on emp.UserId = ticket.CreatedBy
                                        left join Employee whoplaced on whoplaced.UserId=tpc.CreatedBy
                                        left join LookUp lk on lk.DataValue = ticket.Status  and lk.DataKey='TicketStatus'
                                        left join LookUp lkownership on lkownership.DataValue = cus.Ownership and lkownership.DataKey='OwnerShip'
                                        left join Employee sales on CONVERT(nvarchar(50), sales.UserId) = cus.Soldby
                                        left join Employee installer on CONVERT(nvarchar(50), installer.UserId) = cus.Soldby
                                        left join Lookup lksource on lksource.DataValue = cus.LeadSource and lksource.DataKey = 'LeadSource'

		                                where ticket.CompanyId = @CompanyId
                                        and iif(cus.FirstName +' '+cus.LastName is null, cus.BusinessName, cus.DBA) is not null
                                        and ticket.TicketType='Installation'
                                        {6}
                                        {7}
                                        {10}
                                          {14}
                                           {15}
                                           {16}
                                        --order by  ticket.CompletedDate desc,ticket.Id desc
                               select *  into #TicketDataFilter from #TicketData {5}

                                SELECT TOP (@pagesize) #tdf.* --, LAG(#tdf.TicketTypeVal) OVER (ORDER BY #tdf.Id) as PrevTicketType
                                --, LAG(#tdf.CompletionDate) OVER (ORDER BY #tdf.Id) as PrevAppointmentDate
								 --, LAG(#tdf.CreatedByVal) OVER (ORDER BY #tdf.Id) as PrevTechnician
                                into #Testtable FROM #TicketDataFilter #tdf
                                    where Id NOT IN(Select TOP (@pagestart) Id from #TicketData {8}) 
                                     {9}
	                                 select * from #Testtable 
									 select sum(TotalCollectedAmount) as TotalAmountByPage from #TestTable
	                                 {12}
                                
                                DROP TABLE #TicketData
								DROP TABLE #TicketDataFilter
								drop table #TicketIdData
                                drop table #Testtable
                                    ";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        TicketFilters.SearchText,//0
                                        TicketFilters.PageNo,  //1
                                        TicketFilters.PageSize, //2
                                        TicketFilters.CompanyId, //3
                                        TicketFilters.CustomerId, //4
                                        searchQuery,//5
                                                    //ticketStatusQuery,//6
                                                    //ticketTypeQuery,//7
                                                    //assignedQuery,//8
                                        CreatedByMeQuery,//6
                                        myTicketQuery,//7
                                        subquery,// 8
                                        subquery1,// 9
                                        AppointmentDateQuery,// 10
                                                             //ReportTypeQuery,//14,
                                                             //ReportQuery,//15,
                                        ReportColQuery,// 11
                                        ReportCountQuery,// 12
                                        NameSql,//13
                                        filterquerysalesperson,//14
                                        filterquerystatus, //15
                                        filterqueryownership //16
                                                             //filterquery,
                                                             //ReportAgeQuery
                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataSet GetCSRReportListByFilter(TicketFilter TicketFilters, FilterReportModel filter)
        {
            string sqlQuery = @"";
            string searchQuery = @"";
            string DateQuery = @"";

            if (!string.IsNullOrWhiteSpace(TicketFilters.SearchText))
            {
                searchQuery = string.Format(" and Name like '%{0}%'", TicketFilters.SearchText);
            }
            string orderquery = "";
            string orderquery1 = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(TicketFilters.order))
            {
                if (TicketFilters.order == "ascending/customername")
                {
                    orderquery = "order by #e.[Name] asc";
                    orderquery1 = "order by [Name] asc";
                }
                else if (TicketFilters.order == "descending/customername")
                {
                    orderquery = "order by #e.[Name] desc";
                    orderquery1 = "order by [Name] desc";
                }
                else if (TicketFilters.order == "ascending/cancelledaccount")
                {
                    orderquery = "order by #e.CancelledAccount asc";
                    orderquery1 = "order by CancelledAccount asc";
                }
                else if (TicketFilters.order == "descending/cancelledaccount")
                {
                    orderquery = "order by #e.CancelledAccount desc";
                    orderquery1 = "order by CancelledAccount desc";
                }
                else if (TicketFilters.order == "ascending/createdaccount")
                {
                    orderquery = "order by #e.[CreatedAccount] asc";
                    orderquery1 = "order by [CreatedAccount] asc";
                }
                else if (TicketFilters.order == "descending/createdaccount")
                {
                    orderquery = "order by #e.[CreatedAccount] desc";
                    orderquery1 = "order by [CreatedAccount] desc";
                }
                else if (TicketFilters.order == "ascending/accountplacedonline")
                {
                    orderquery = "order by #e.[AccountPlaced] asc";
                    orderquery1 = "order by [AccountPlaced] asc";
                }
                else if (TicketFilters.order == "descending/accountplacedonline")
                {
                    orderquery = "order by #e.[AccountPlaced] desc";
                    orderquery1 = "order by [AccountPlaced] desc";
                }
                else if (TicketFilters.order == "ascending/contractsent")
                {
                    orderquery = "order by #e.[ContractSent] asc";
                    orderquery1 = "order by [ContractSent] asc";
                }
                else if (TicketFilters.order == "descending/contractsent")
                {
                    orderquery = "order by #e.[ContractSent] desc";
                    orderquery1 = "order by [ContractSent] desc";
                }
                else if (TicketFilters.order == "ascending/installedscheduled")
                {
                    orderquery = "order by #e.[InstallScheduled] asc";
                    orderquery1 = "order by [InstallScheduled] asc";
                }
                else if (TicketFilters.order == "descending/installedscheduled")
                {
                    orderquery = "order by #e.[InstallScheduled] desc";
                    orderquery1 = "order by [InstallScheduled] desc";
                }
                else if (TicketFilters.order == "ascending/servicescheduled")
                {
                    orderquery = "order by #e.[ServicesScheduled] asc";
                    orderquery1 = "order by [ServicesScheduled] asc";
                }
                else if (TicketFilters.order == "descending/servicescheduled")
                {
                    orderquery = "order by #e.[ServicesScheduled] desc";
                    orderquery1 = "order by [ServicesScheduled] desc";
                }

                else
                {
                    orderquery = "order by #e.[Id]  desc";
                    orderquery1 = "order by Id desc";
                }

            }
            else
            {
                orderquery = "order by #e.[Id] desc";
                orderquery1 = "order by Id desc";
            }
            #endregion

            #region AppointmentDateQuery
            if (TicketFilters.StartDate != new DateTime() && TicketFilters.EndDate != new DateTime())
            {
                var StartDate = TicketFilters.StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                var EndDate = TicketFilters.EndDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                DateQuery = string.Format(" and CreatedDate between '{0}' and '{1}'", StartDate, EndDate);
            }
            #endregion


            sqlQuery = @"declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                 select * into #CsrData from (
                                select  emp.Id,emp.FirstName+' '+emp.LastName as Name,emp.CreatedDate,
                                (select Count(*) from customercancel where EmployeeId = emp.UserId) as [CancelledAccount],
                                (select Count(*) from customer where CreatedByUid = emp.UserId) as [CreatedAccount],
                                (select Count(*) from CustomerExtended where ContractSentBy = emp.UserId) as [ContractSent],
                                (select Count(*) from ThirdPartyCustomer where CreatedBy = emp.UserId) as [AccountPlaced],
                                (select Count(*) from Ticket where TicketType = 'Installation' and  CreatedBy = emp.UserId) as [InstallScheduled],
                                (select Count(*) from Ticket where TicketType = 'Service' and  CreatedBy = emp.UserId) as [ServicesScheduled]
                                from employee emp 
                                  ) d	
                                 
                                select * into #CsrDataFilter
								from #CsrData

								select top(@pagesize)
								* into #Testtable from #CsrDataFilter
								where Id not in(select top(@pagestart) Id from #CsrDataFilter #e {2}) {0}{1}
                                 {3}
                               
                                select * from #Testtable
                                select sum(CancelledAccount) as TotalCancelledAccount,sum(CreatedAccount) as TotalCreatedAccount,sum(ContractSent) as TotalContractSent,
                                sum(AccountPlaced) as TotalAccountPlaced,sum(InstallScheduled) as TotalInstallScheduled,sum(ServicesScheduled) as TotalServicesScheduled from #Testtable
								select count(*) TotalCount
                                from #CsrData
                                where Id>0
                                   {0}{1}
                                
								drop table #CsrData
								drop table #CsrDataFilter
                                drop table #Testtable
                                ";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                       searchQuery,
                                       DateQuery,
                                       orderquery,
                                       orderquery1

                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", TicketFilters.PageNo));
                    AddParameter(cmd, pInt32("pagesize", TicketFilters.PageSize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetServiceTrackerReportByFilter(TicketFilter TicketFilters, FilterReportModel filter, string Start, string End)
        {
            string sqlQuery = @"";
            string sqlQuery1 = @"";

            string searchQuery = @"";
            string DateQuery = @"";
            string filterqueryinstallertechnician = "";
            string orderquery = "";
            string orderquery1 = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(TicketFilters.order))
            {
                if (TicketFilters.order == "ascending/ticketid")
                {
                    orderquery = "order by #e.[TicketId] asc";
                    orderquery1 = "order by [TicketId] asc";
                }
                else if (TicketFilters.order == "descending/ticketid")
                {
                    orderquery = "order by #e.[TicketId] desc";
                    orderquery1 = "order by [TicketId] desc";
                }
                else if (TicketFilters.order == "ascending/customername")
                {
                    orderquery = "order by #e.CustomerName asc";
                    orderquery1 = "order by CustomerName asc";
                }
                else if (TicketFilters.order == "descending/customername")
                {
                    orderquery = "order by #e.CustomerName desc";
                    orderquery1 = "order by CustomerName desc";
                }
                else if (TicketFilters.order == "ascending/technician")
                {
                    orderquery = "order by #e.[InstallerTechnician] asc";
                    orderquery1 = "order by [InstallerTechnician] asc";
                }
                else if (TicketFilters.order == "descending/technician")
                {
                    orderquery = "order by #e.[InstallerTechnician] desc";
                    orderquery1 = "order by [InstallerTechnician] desc";
                }
                else if (TicketFilters.order == "ascending/servicetech")
                {
                    orderquery = "order by #e.[ServiceTechnician] asc";
                    orderquery1 = "order by [ServiceTechnician] asc";
                }
                else if (TicketFilters.order == "descending/servicetech")
                {
                    orderquery = "order by #e.[ServiceTechnician] desc";
                    orderquery1 = "order by [ServiceTechnician] desc";
                }
                else if (TicketFilters.order == "ascending/service")
                {
                    orderquery = "order by #e.[ServiceType] asc";
                    orderquery1 = "order by [ServiceType] asc";
                }
                else if (TicketFilters.order == "descending/service")
                {
                    orderquery = "order by #e.[ServiceType] desc";
                    orderquery1 = "order by [ServiceType] desc";
                }
                else if (TicketFilters.order == "ascending/reason")
                {
                    orderquery = "order by #e.[Reason] asc";
                    orderquery1 = "order by [Reason] asc";
                }
                else if (TicketFilters.order == "descending/reason")
                {
                    orderquery = "order by #e.[Reason] desc";
                    orderquery1 = "order by [Reason] desc";
                }
                else if (TicketFilters.order == "ascending/schedule")
                {
                    orderquery = "order by #e.[ScheduledServiceDate] asc";
                    orderquery1 = "order by [ScheduledServiceDate] asc";
                }
                else if (TicketFilters.order == "descending/schedule")
                {
                    orderquery = "order by #e.[ScheduledServiceDate] desc";
                    orderquery1 = "order by [ScheduledServiceDate] desc";
                }
                else if (TicketFilters.order == "ascending/onsite")
                {
                    orderquery = "order by #e.[TechOnsiteDate] asc";
                    orderquery1 = "order by [TechOnsiteDate] asc";
                }
                else if (TicketFilters.order == "descending/onsite")
                {
                    orderquery = "order by #e.[TechOnsiteDate] desc";
                    orderquery1 = "order by [TechOnsiteDate] desc";
                }
                else
                {
                    orderquery = "order by #e.[TicketId]  desc";
                    orderquery1 = "order by TicketId desc";
                }

            }
            else
            {
                orderquery = "order by #e.[TicketId] desc";
                orderquery1 = "order by TicketId desc";
            }
            #endregion

            if (TicketFilters.salesperson == "null")
            {
                TicketFilters.salesperson = TicketFilters.salesperson.Substring(0, TicketFilters.salesperson.Length - 4);

            }

            if (!string.IsNullOrWhiteSpace(TicketFilters.salesperson))
            {
                filterqueryinstallertechnician += string.Format("and InstallerTechnician in ('{0}')", TicketFilters.salesperson);
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.SearchText))
            {
                searchQuery = string.Format(" and (CustomerName like '%{0}%' or TicketId like '%{0}%')", TicketFilters.SearchText);
            }

            //DateQuery = string.Format(" and ScheduledServiceDate between '{0}' and '{1}'", Start, End);
            //#region AppointmentDateQuery
            if (TicketFilters.StartDate != new DateTime() && TicketFilters.EndDate != new DateTime())
            {
                var StartDate = TicketFilters.StartDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                var EndDate = TicketFilters.EndDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                DateQuery = string.Format(" and ScheduledServiceDate between '{0}' and '{1}'", StartDate, EndDate);
            }

            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
                //NameSql = NameSql.Replace("cus.", "cu.");
            }

            sqlQuery = @"declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                 select * into #ServiceData from (
                                select {2} as CustomerName,cus.Id, cus.CustomerId,tk.Id  as TicketId,tk.CreatedDate,(select top(1) emp.FirstName+' '+emp.LastName from customer cu
								left join ticket tk on tk.CustomerId = cu.CustomerId
								left join TicketUser tu on tu.TiketId = tk.TicketId and tk.TicketType = 'Installation' and tu.IsPrimary = 1
								left join Employee emp on emp.UserId = tu.UserId
								 where cu.CustomerId = cus.CustomerId and tu.Id > 0) as InstallerTechnician,empService.firstname+''+empService.LastName as ServiceTechnician,
                                tk.TicketType as ServiceType,tk.Reason,tk.TechOnsiteDate,tk.CompletionDate as ScheduledServiceDate  from customer cus
                                left join ticket tk on tk.CustomerId = cus.customerId
                              
                                left join ticketuser tuService on tuService.TiketId = tk.TicketId and tk.TicketType != 'Installation' and tuService.IsPrimary = 1
                           
                                left join employee empService on empService.UserId = tuService.UserId
                                where tk.TicketId is not null 
                                  ) d	

                                select *,IDENTITY(INT, 1, 1) AS paginationid into #ServiceDataFilter
								from #ServiceData
                                where Id>0 {3}
								select top(@pagesize)
								* from #ServiceDataFilter
								where paginationid not in(select top(@pagestart) paginationid from #ServiceDataFilter #e {4}) {0}{1}
                                 {5}

								select count(*) TotalCount
                                from #ServiceData
                                where Id>0 {3}{1} {0}
								drop table #ServiceData
								drop table #ServiceDataFilter";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                       searchQuery,
                                       DateQuery,
                                       NameSql,
                                       filterqueryinstallertechnician,
                                       orderquery,
                                       orderquery1
                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", TicketFilters.PageNo));
                    AddParameter(cmd, pInt32("pagesize", TicketFilters.PageSize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetTicketListInstallByFilter(TicketFilter TicketFilters)
        {
            string sqlQuery = @"";
            string searchQuery = "";
            string searchQuery2 = "";
            string searchQuery3 = "";
            string searchQuery4 = "";
            string myTicketQuery = "";
            string ticketStatusQuery = "";
            string ticketStatusQuery2 = "";
            string ticketStatusQuery3 = "";
            string ticketStatusQuery4 = "";
            string ticketTypeQuery = "";
            string ticketTypeQuery2 = "";
            string ticketTypeQuery3 = "";
            string ticketTypeQuery4 = "";
            string assignedQuery = "";
            string CreatedByMeQuery = "";
            string subquery = "";
            string subquery1 = "";
            string CreatedDateQuery = "";
            string CreatedDateQuery2 = "";
            string CreatedDateQuery3 = "";
            string CreatedDateQuery4 = "";
            string ReportTypeQuery = "";
            string ReportQuery = "";
            string ReportColQuery = "";
            string ReportCountQuery = "";
            string CategoryQuery = "";
            string CategoryQuery2 = "";
            string CategoryQuery3 = "";
            string CategoryQuery4 = "";
            string ManuQuery = "";
            string ManuQuery2 = "";
            string ManuQuery3 = "";
            string ManuQuery4 = "";
            string TechnicianQuery = "";
            string TechnicianQuery2 = "";
            string TechnicianQuery3 = "";
            string TechnicianQuery4 = "";
            //string EquipmentStatus = "";
            //string EquipmentStatus2 = "";
            if (!string.IsNullOrWhiteSpace(TicketFilters.SearchText))
            {
                searchQuery = @"and (CONVERT(nvarchar(11), tk.CompletionDate, 101) like @SearchText
								or tk.[Status] like @SearchText or tk.[TicketType] like @SearchText or tk.Id like @SearchText or (select FirstName + LastName from Customer where CustomerId = tk.CustomerId) like @SearchText or (select Id from Customer where CustomerId = tk.CustomerId) like @SearchText)";
                searchQuery2 = @"and (CONVERT(nvarchar(11), tk2.CompletionDate, 101) like @SearchText
								or tk2.[Status] like @SearchText or tk2.[TicketType] like @SearchText or tk2.Id like @SearchText or (select FirstName + LastName from Customer where CustomerId = tk2.CustomerId) like @SearchText or (select Id from Customer where CustomerId = tk2.CustomerId) like @SearchText)";
                searchQuery3 = @"and (CONVERT(nvarchar(11), tk3.CompletionDate, 101) like @SearchText
								or tk3.[Status] like @SearchText or tk3.[TicketType] like @SearchText or tk3.Id like @SearchText or (select FirstName + LastName from Customer where CustomerId = tk3.CustomerId) like @SearchText or (select Id from Customer where CustomerId = tk3.CustomerId) like @SearchText)";
                searchQuery4 = @"and (CONVERT(nvarchar(11), tk4.CompletionDate, 101) like @SearchText
								or tk4.[Status] like @SearchText or tk4.[TicketType] like @SearchText or tk4.Id like @SearchText or (select FirstName + LastName from Customer where CustomerId = tk4.CustomerId) like @SearchText or (select Id from Customer where CustomerId = tk4.CustomerId) like @SearchText)";
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.ReportTabType) && TicketFilters.ReportTabType == "GoBack")
            {
                ReportTypeQuery = string.Format("and convert(date, tk.CreatedDate) between '{0}' and '{1}'", TicketFilters.StartDate, TicketFilters.EndDate);
                //ReportQuery = string.Format("and countticket > 1");
                //ReportColQuery = string.Format("(select count(tik.Id) from ticket tik where convert(date, tik.CreatedDate) between '{0}' and '{1}' and tik.CustomerId=cs.CustomerId) as CountTicket", TicketFilters.StartDate, TicketFilters.EndDate);
                //ReportCountQuery = string.Format("select  count(Id) as [TotalCount] from #TicketData where countticket > 1");
            }
            else
            {
                ReportColQuery = string.Format("'' as CountTicket");
                ReportCountQuery = string.Format("select  count(Id) as [TotalCount] from #TicketData");
            }
            #region TicketStatus
            if (!string.IsNullOrWhiteSpace(TicketFilters.TicketStatus)
                && TicketFilters.TicketStatus != "-1" && TicketFilters.TicketStatus != "null")
            {
                ticketStatusQuery = string.Format("and tk.[Status] in ('{0}')", TicketFilters.TicketStatus);
                ticketStatusQuery2 = string.Format("and tk2.[Status] in ('{0}')", TicketFilters.TicketStatus);
                ticketStatusQuery3 = string.Format("and tk3.[Status] in ('{0}')", TicketFilters.TicketStatus);
                ticketStatusQuery4 = string.Format("and tk4.[Status] in ('{0}')", TicketFilters.TicketStatus);
            }
            #endregion

            #region Assigned
            if (!string.IsNullOrWhiteSpace(TicketFilters.AssignedUserTicket) && TicketFilters.AssignedUserTicket != "-1" && TicketFilters.AssignedUserTicket != "null")
            {
                assignedQuery = string.Format("and tuser.UserId in ('{0}')", TicketFilters.AssignedUserTicket);
            }
            #endregion


            #region TicketType
            if (!string.IsNullOrWhiteSpace(TicketFilters.TicketType)
                && TicketFilters.TicketType != "-1" && TicketFilters.TicketType != "null")
            {
                ticketTypeQuery = string.Format("and tk.TicketType in ('{0}')", TicketFilters.TicketType);
                ticketTypeQuery2 = string.Format("and tk2.TicketType in ('{0}')", TicketFilters.TicketType);
                ticketTypeQuery3 = string.Format("and tk3.TicketType in ('{0}')", TicketFilters.TicketType);
                ticketTypeQuery4 = string.Format("and tk4.TicketType in ('{0}')", TicketFilters.TicketType);
            }
            #endregion
            #region Technician
            //if (!string.IsNullOrWhiteSpace(TicketFilters.technician)
            //    && TicketFilters.technician != "-1" && TicketFilters.technician != "null")
            if (TicketFilters.technicianlist != null && TicketFilters.technicianlist[0] != "null"
                && TicketFilters.technicianlist[0] != "" && TicketFilters.technicianlist[0] != "-1")
            {
                string Ids = "";
                foreach (string id in TicketFilters.technicianlist)
                {
                    Ids += string.Format("'{0}',", id);
                }
                TechnicianQuery = "and emp.UserId in (" + Ids.TrimEnd(',') + ")";
                TechnicianQuery2 = "and emp2.UserId in (" + Ids.TrimEnd(',') + ")";
                TechnicianQuery3 = "and emp3.UserId in (" + Ids.TrimEnd(',') + ")";
                TechnicianQuery4 = "and emp4.UserId in (" + Ids.TrimEnd(',') + ")";

                //TechnicianQuery = string.Format("and emp.FirstName + ' ' + emp.LastName in ({0})", TicketFilters.technicianlist);
            }
            #endregion
            #region CreatedDateQuery
            if (TicketFilters.StartDate != new DateTime() && TicketFilters.EndDate != new DateTime())
            {
                string StartDate = TicketFilters.StartDate.ToString("yyyy-MM-dd 00:00:00.000"); //("yyyy-MM-dd HH:mm:ss:fff")
                string EndDate = TicketFilters.EndDate.ToString("yyyy-MM-dd 23:59:59.999"); //("yyyy-MM-dd HH:mm:ss:fff")

                //var StartDate = TicketFilters.StartDate.SetZeroHour().UTCToClientTime();
                //var EndDate = TicketFilters.EndDate.SetMaxHour().UTCToClientTime();
                CreatedDateQuery = string.Format("and tk.CreatedDate between '{0}' and '{1}'", StartDate, EndDate);
                CreatedDateQuery2 = string.Format("and tk2.CreatedDate between '{0}' and '{1}'", StartDate, EndDate);
                CreatedDateQuery3 = string.Format("and tk3.CreatedDate between '{0}' and '{1}'", StartDate, EndDate);
                CreatedDateQuery4 = string.Format("and tk4.CreatedDate between '{0}' and '{1}'", StartDate, EndDate);
            }
            #endregion
            #region Installed Status
            //if (!string.IsNullOrWhiteSpace(TicketFilters.EquipmentStatus)
            //    && TicketFilters.EquipmentStatus != "-1" && TicketFilters.EquipmentStatus == "Installed")
            //{
            //    EquipmentStatus = string.Format("and _cae.QuantityLeftEquipment > 0", TicketFilters.EquipmentStatus);
            //}
            //if (!string.IsNullOrWhiteSpace(TicketFilters.EquipmentStatus)
            //    && TicketFilters.EquipmentStatus != "-1" && TicketFilters.EquipmentStatus == "NotInstalled")
            //{
            //    EquipmentStatus2 = string.Format("and (_cae.QuantityLeftEquipment = 0 or _cae.QuantityLeftEquipment is null) ", TicketFilters.EquipmentStatus);
            //}
            #endregion
            #region MyTicket
            if (!string.IsNullOrWhiteSpace(TicketFilters.MyTicket))
            {
                if (TicketFilters.MyTicket == "Created")
                {
                    CreatedByMeQuery = string.Format("and tk.CreatedByUid = '{0}'", TicketFilters.UserId);

                }
                else if (TicketFilters.MyTicket == "Assigned")
                {
                    myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(tk.TicketId,'{0}') = 1 ", TicketFilters.UserId);
                }
                else if (TicketFilters.MyTicket == "Both")
                {
                    CreatedByMeQuery = string.Format("and tk.CreatedByUid = '{0}'", TicketFilters.UserId);
                    myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(tk.TicketId,'{0}') = 1 ", TicketFilters.UserId);
                }
                else if (TicketFilters.MyTicket == "None")
                {
                    CreatedByMeQuery = string.Format("and tk.CreatedByUid != '{0}'", TicketFilters.UserId);
                    myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(tk.TicketId,'{0}') = 0 ", TicketFilters.UserId);

                }
            }

            #endregion
            #region Order
            if (!string.IsNullOrWhiteSpace(TicketFilters.order))
            {
                if (TicketFilters.order == "ascending/ticketid")
                {
                    subquery = "order by #MyList.[Id] asc";
                    subquery1 = "order by [Id] asc";
                }
                else if (TicketFilters.order == "descending/ticketid")
                {
                    subquery = "order by #MyList.[Id] desc";
                    subquery1 = "order by [Id] desc";
                }
                else if (TicketFilters.order == "ascending/customername")
                {
                    subquery = "order by #MyList.CustomerName asc";
                    subquery1 = "order by CustomerName asc";
                }
                else if (TicketFilters.order == "descending/customername")
                {
                    subquery = "order by #MyList.CustomerName desc";
                    subquery1 = "order by CustomerName desc";
                }
                else if (TicketFilters.order == "ascending/technician")
                {
                    subquery = "order by #MyList.[EmpUser] asc";
                    subquery1 = "order by [EmpUser] asc";
                }
                else if (TicketFilters.order == "descending/technician")
                {
                    subquery = "order by #MyList.[EmpUser] desc";
                    subquery1 = "order by [EmpUser] desc";
                }
                else if (TicketFilters.order == "ascending/SKU")
                {
                    subquery = "order by #MyList.[SKU] asc";
                    subquery1 = "order by [SKU] asc";
                }
                else if (TicketFilters.order == "descending/SKU")
                {
                    subquery = "order by #MyList.[SKU] desc";
                    subquery1 = "order by [SKU] desc";
                }
                else if (TicketFilters.order == "ascending/appointmentdate")
                {
                    subquery = "order by #MyList.[CompletionDate] asc";
                    subquery1 = "order by [CompletionDate] asc";
                }
                else if (TicketFilters.order == "descending/appointmentdate")
                {
                    subquery = "order by #MyList.[CompletionDate] desc";
                    subquery1 = "order by [CompletionDate] desc";
                }
                else if (TicketFilters.order == "ascending/ticketstatus")
                {
                    subquery = "order by #MyList.[Status]  asc";
                    subquery1 = "order by Status asc";
                }
                else if (TicketFilters.order == "descending/ticketstatus")
                {
                    subquery = "order by #MyList.[Status]  desc";
                    subquery1 = "order by Status desc";
                }
                else if (TicketFilters.order == "ascending/installed")
                {
                    subquery = "order by #MyList.[InstalledEquipment]  asc";
                    subquery1 = "order by InstalledEquipment asc";
                }
                else if (TicketFilters.order == "descending/installed")
                {
                    subquery = "order by #MyList.[InstalledEquipment]  desc";
                    subquery1 = "order by InstalledEquipment desc";
                }
                else if (TicketFilters.order == "ascending/points")
                {
                    subquery = "order by #MyList.[TotalPoint]  asc";
                    subquery1 = "order by TotalPoint asc";
                }
                else if (TicketFilters.order == "descending/points")
                {
                    subquery = "order by #MyList.[TotalPoint]  desc";
                    subquery1 = "order by TotalPoint desc";
                }
                else if (TicketFilters.order == "ascending/description")
                {
                    subquery = "order by #MyList.[Description]  asc";
                    subquery1 = "order by Description asc";
                }
                else if (TicketFilters.order == "descending/description")
                {
                    subquery = "order by #MyList.[Description]  desc";
                    subquery1 = "order by Description desc";
                }
                else if (TicketFilters.order == "ascending/cost")
                {
                    subquery = "order by #MyList.[CustomerCost]  asc";
                    subquery1 = "order by CustomerCost asc";
                }
                else if (TicketFilters.order == "descending/cost")
                {
                    subquery = "order by #MyList.[CustomerCost]  desc";
                    subquery1 = "order by CustomerCost desc";
                }
                else
                {
                    subquery = "order by #MyList.[Id]  desc";
                    subquery1 = "order by Id desc";
                }
            }
            else
            {
                subquery = "order by #MyList.[Id] desc";
                subquery1 = "order by Id desc";
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.category) && TicketFilters.category != "-1" && TicketFilters.category != "null" && TicketFilters.category != "undefined")
            {
                CategoryQuery = string.Format("and _et.Id in ({0})", TicketFilters.category);
                CategoryQuery2 = string.Format("and _et2.Id in ({0})", TicketFilters.category);
                CategoryQuery3 = string.Format("and _et3.Id in ({0})", TicketFilters.category);
                CategoryQuery4 = string.Format("and _et4.Id in ({0})", TicketFilters.category);
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.manufact) && TicketFilters.manufact != "'-1'" && TicketFilters.manufact != "'null'" && TicketFilters.manufact != "undefined")
            {
                ManuQuery = string.Format("and manu.ManufacturerId in ({0})", TicketFilters.manufact);
                ManuQuery2 = string.Format("and manu2.ManufacturerId in ({0})", TicketFilters.manufact);
                ManuQuery3 = string.Format("and manu3.ManufacturerId in ({0})", TicketFilters.manufact);
                ManuQuery4 = string.Format("and manu4.ManufacturerId in ({0})", TicketFilters.manufact);
            }
            #endregion
            sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                DECLARE @CustomerId uniqueidentifier
                                DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int
                                DECLARE @SearchText nvarchar(50) 

                                SET @SearchText = '%{0}%' 
                                SET @pageno = {1} --default 1
                                SET @pagesize = {2} --default 10
                                SET @CompanyId = '{3}' --97BCF758-A482-47EB-82B8-F88BF12293FF
                                SET @CustomerId = '{4}'

                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

 
DECLARE @MyList Table(AppointmentEquipmentId int, Category nvarchar(50),Manufacturer nvarchar(50), [Description] nvarchar(50), TicketType nvarchar(50),
EmpUser varchar(50), TicketId int, RepliesCount int, AttachmentsCount int, CusIdInt int, CustomerName nvarchar(50), CompletionDate datetime,
SKU nvarchar(50),TotalPoint float, IsClosed bit, CompanyCost float, CustomerCost float, Quantity int, InstalledEquipment int, Qty int, [Status] nvarchar(50))

DECLARE @IdCount int, @qty int, @i int, @j int, @getAppointmentEquipmentId int, @getCategory nvarchar(50), @getManufacturer nvarchar(50),@getDescription nvarchar(50),
 @getTicketType nvarchar(50), @getEmpUser nvarchar(50),  @getRepliesCount int, @getAttachmentsCount int, @getCusIdInt int,
 @getCustomerName nvarchar(50), @getCompletionDate datetime, @getSKU nvarchar(50), @getTotalPoint float, @getIsClosed bit, @getCompanyCost float,
 @getCustomerCost float, @getQuantity int, @getInstalledEquipment int, @getTicketId int, @getQty int, @getStatus nvarchar(50)

	set @i = 1;
	set @j = 1;

	select @IdCount = count(_cae.Id) from  CustomerAppointmentEquipment _cae
										left join ticket tk on _cae.AppointmentId = tk.TicketId
										left join Equipment _eq on _eq.EquipmentId = _cae.EquipmentId
										left join EquipmentType _et on _et.Id = _eq.EquipmentTypeId
										left join Manufacturer manu on manu.Id = _eq.ManufacturerId
										left join TicketUser tu on tu.TiketId = tk.TicketId
										left join Employee emp on emp.UserId = tu.UserId
                                        left join Lookup lktype on  lktype.DataKey ='TicketType'
                                        left join Customer cus on cus.CustomerId = tk.CustomerId  
                                        and lktype.DataValue = tk.TicketType
										where tk.CompanyId = @CompanyId
                                        and _eq.EquipmentClassId=1
										and cus.id is not null
										and _cae.QuantityLeftEquipment > 0
										{5}
										{6}
										{7}
										{20}
										{13}
										{18}
										{19}
	Select 
      Row_Number() Over (Order By ca.Id DESC) As RowNum
     ,ca.Id,Quantity,AppointmentId,ca.EquipmentId,QuantityLeftEquipment,UnitPrice
    into #CustomerAppointmentEquipment From CustomerAppointmentEquipment ca
	left join ticket tk2 on ca.AppointmentId = tk2.TicketId
										left join Equipment _eq2 on _eq2.EquipmentId = ca.EquipmentId
										left join EquipmentType _et2 on _et2.Id = _eq2.EquipmentTypeId
										left join Manufacturer manu2 on manu2.Id = _eq2.ManufacturerId
										left join TicketUser tu2 on tu2.TiketId = tk2.TicketId
										left join Employee emp2 on emp2.UserId = tu2.UserId
                                        left join Lookup lktype2 on  lktype2.DataKey ='TicketType'
                                        left join Customer cus2 on cus2.CustomerId = tk2.CustomerId  
                                        and lktype2.DataValue = tk2.TicketType
										where tk2.CompanyId = @CompanyId
                                        and _eq2.EquipmentClassId=1
										and cus2.id is not null
										and ca.QuantityLeftEquipment > 0
										{21}
										{24}
										{27}
										{30}
										{33}
										{36}
										{39}

	WHILE @i <= @IdCount
	BEGIN
		select @qty = _cae4.QuantityLeftEquipment from  #CustomerAppointmentEquipment _cae4
										left join ticket tk4 on _cae4.AppointmentId = tk4.TicketId
										left join Equipment _eq4 on _eq4.EquipmentId = _cae4.EquipmentId
										left join EquipmentType _et4 on _et4.Id = _eq4.EquipmentTypeId
										left join Manufacturer manu4 on manu4.Id = _eq4.ManufacturerId
										left join TicketUser tu4 on tu4.TiketId = tk4.TicketId
										left join Employee emp4 on emp4.UserId = tu4.UserId
                                        left join Lookup lktype4 on  lktype4.DataKey ='TicketType'
                                        left join Customer cus4 on cus4.CustomerId = tk4.CustomerId  
                                        and lktype4.DataValue = tk4.TicketType
										where tk4.CompanyId = @CompanyId
                                        and _eq4.EquipmentClassId=1
										and cus4.id is not null
										and _cae4.QuantityLeftEquipment > 0
										and _cae4.RowNum = @i
										{23}
										{26}
										{29}
										{32}
										{35}
										{38}
										{41}
		WHILE @j <= @qty
		BEGIN
			select @getAppointmentEquipmentId = _cae3.Id,
				   @getCategory = _et3.Name,
				   @getManufacturer = manu3.Name,
				   @getDescription = _eq3.Name,
				   @getTicketType = lktype3.DisplayText,
				   @getEmpUser = emp3.FirstName + ' ' + emp3.LastName,
				   @getRepliesCount = (select count(id) from TicketFile where TicketId = tk3.TicketId)
		                                        + (select count(id) from TicketReply where TicketId = tk3.TicketId),
				   @getStatus = tk3.[Status],
				   @getTicketId = tk3.Id,
				   @getAttachmentsCount = (select count(id) from TicketFile where TicketId = tk3.TicketId),
				   @getCusIdInt = cus3.Id,
				   @getCustomerName = cus3.FirstName + ' ' + cus3.LastName,
				   @getCompletionDate = tk3.CompletionDate,
				   @getSKU = _eq3.SKU,
				   @getTotalPoint = Format(_eq3.Point*_cae3.QuantityLeftEquipment,'N2'),
				   @getIsClosed = tk3.IsClosed,
				   @getCompanyCost = (select Cost from EquipmentVendor where EquipmentId = _eq3.EquipmentId and IsPrimary = 1),
				   @getCustomerCost = _cae3.UnitPrice,
				   @getQuantity = _cae3.Quantity,
				   @getInstalledEquipment =_cae3.QuantityLeftEquipment,
				   @getQty = 1
				   from  #CustomerAppointmentEquipment _cae3
				   left join ticket tk3 on _cae3.AppointmentId = tk3.TicketId
										left join Equipment _eq3 on _eq3.EquipmentId = _cae3.EquipmentId
										left join EquipmentType _et3 on _et3.Id = _eq3.EquipmentTypeId
										left join Manufacturer manu3 on manu3.Id = _eq3.ManufacturerId
										left join TicketUser tu3 on tu3.TiketId = tk3.TicketId
										left join Employee emp3 on emp3.UserId = tu3.UserId
                                        left join Lookup lktype3 on  lktype3.DataKey ='TicketType'
                                        left join Customer cus3 on cus3.CustomerId = tk3.CustomerId  
                                        and lktype3.DataValue = tk3.TicketType
										where tk3.CompanyId = @CompanyId
                                        and _eq3.EquipmentClassId=1
										and cus3.Id is not null
										and _cae3.QuantityLeftEquipment > 0
										and _cae3.RowNum = @i
										{22}
										{25}
										{28}
										{31}
										{34}
										{37}
										{40}
			insert INTO @MyList values (@getAppointmentEquipmentId, @getCategory, @getManufacturer, @getDescription, @getTicketType, @getEmpUser, @getTicketId, @getRepliesCount,
			 @getAttachmentsCount, @getCusIdInt, @getCustomerName, @getCompletionDate, @getSKU, @getTotalPoint, @getIsClosed,
			@getCompanyCost, @getCustomerCost, @getQuantity, @getInstalledEquipment,@getQty, @getStatus)

			SET @j += 1;
		END

		SET @i += 1;
		set @j = 1;
END

Select 
      Row_Number() Over (Order By AppointmentEquipmentId DESC) As Id
     ,*
    into #MyList From @MyList

SELECT TOP (@pagesize) * into #TotalCount FROM #MyList
                                    where Id NOT IN(Select TOP (@pagestart) Id from #MyList order by Id asc) 
                                    --{11}
                                      order by TicketId desc

	                            select  count(*) as [TotalCount] from @MyList

                                select * from #TotalCount
								select sum(Qty) as TotalQuantity
								,sum(TotalPoint) as PointSum
								,sum(CustomerCost) as TotalCustomerCost
								,sum(CompanyCost) as TotalCompanyCost
								from #TotalCount

							DROP TABLE #CustomerAppointmentEquipment
							DROP TABLE #MyList
							DROP TABLE #TotalCount
                                    
                                    ";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        TicketFilters.SearchText,//0
                                        TicketFilters.PageNo,  //1
                                        TicketFilters.PageSize, //2
                                        TicketFilters.CompanyId, //3
                                        TicketFilters.CustomerId, //4
                                        searchQuery,//5
                                        ticketStatusQuery,//6
                                        ticketTypeQuery,//7
                                        assignedQuery,//8
                                        CreatedByMeQuery,//9
                                        myTicketQuery,//10
                                        subquery,//11
                                        subquery1,//12
                                        CreatedDateQuery,//13,
                                        ReportTypeQuery,//14,
                                        ReportQuery,//15,
                                        ReportColQuery,//16
                                        ReportCountQuery,//17
                                        CategoryQuery,//18
                                        ManuQuery,//19
                                        TechnicianQuery,//20
                                        searchQuery2, //21
                                        searchQuery3, //22
                                        searchQuery4, //23
                                        ticketStatusQuery2, // 24
                                        ticketStatusQuery3, //25
                                        ticketStatusQuery4, // 26
                                        ticketTypeQuery2, //27
                                        ticketTypeQuery3, //28
                                        ticketTypeQuery4, //29
                                        TechnicianQuery2, //30
                                        TechnicianQuery3,//31
                                        TechnicianQuery4, //32
                                        CreatedDateQuery2, //33
                                        CreatedDateQuery3, //34
                                        CreatedDateQuery4, //35
                                        CategoryQuery2, //36
                                        CategoryQuery3, //37
                                        CategoryQuery4, //38
                                        ManuQuery2, //39
                                        ManuQuery3, //40
                                        ManuQuery4 //41
                                        );

                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetIndividualInstalledEquipment(TicketFilter TicketFilters)
        {
            string sqlQuery = @"";
            string searchQuery = "";
            string myTicketQuery = "";
            string ticketStatusQuery = ""; ;
            string ticketTypeQuery = "";
            string assignedQuery = "";
            string CreatedByMeQuery = "";
            string subquery = "";
            string subquery1 = "";
            string CreatedDateQuery = "";
            string ReportTypeQuery = "";
            string ReportQuery = "";
            string ReportColQuery = "";
            string ReportCountQuery = "";
            string CategoryQuery = "";
            string ManuQuery = "";
            string TechnicianQuery = "";
            if (!string.IsNullOrWhiteSpace(TicketFilters.SearchText))
            {
                searchQuery = @"and (CONVERT(nvarchar(11), tk.CompletionDate, 101) like @SearchText
								or tk.[Status] like @SearchText or tk.[TicketType] like @SearchText or tk.Id like @SearchText or (select FirstName + LastName from Customer where CustomerId = tk.CustomerId) like @SearchText or (select Id from Customer where CustomerId = tk.CustomerId) like @SearchText)";
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.ReportTabType) && TicketFilters.ReportTabType == "GoBack")
            {
                ReportTypeQuery = string.Format("and convert(date, tk.CreatedDate) between '{0}' and '{1}'", TicketFilters.StartDate, TicketFilters.EndDate);
            }
            else
            {
                ReportColQuery = string.Format("'' as CountTicket");
                ReportCountQuery = string.Format("select  count(AppointmentEquipmentId) as [TotalCount] from #TestTable");
            }
            #region TicketStatus
            if (!string.IsNullOrWhiteSpace(TicketFilters.TicketStatus)
                && TicketFilters.TicketStatus != "-1" && TicketFilters.TicketStatus != "null")
            {
                ticketStatusQuery = string.Format("and tk.[Status] in ('{0}')", TicketFilters.TicketStatus);
            }
            #endregion
            #region Assigned
            if (!string.IsNullOrWhiteSpace(TicketFilters.AssignedUserTicket) && TicketFilters.AssignedUserTicket != "-1" && TicketFilters.AssignedUserTicket != "null" && TicketFilters.AssignedUserTicket != "undefined")
            {
                assignedQuery = string.Format("and tuser.UserId in ('{0}')", TicketFilters.AssignedUserTicket);
            }
            #endregion
            #region TicketType
            if (!string.IsNullOrWhiteSpace(TicketFilters.TicketType)
                && TicketFilters.TicketType != "-1" && TicketFilters.TicketType != "null")
            {
                ticketTypeQuery = string.Format("and tk.TicketType in ('{0}')", TicketFilters.TicketType);
            }
            #endregion
            #region Technician
            if (TicketFilters.technicianlist != null && TicketFilters.technicianlist[0] != "null"
                && TicketFilters.technicianlist[0] != "" && TicketFilters.technicianlist[0] != "-1")
            {
                string Ids = "";
                foreach (string id in TicketFilters.technicianlist)
                {
                    Ids += string.Format("'{0}',", id);
                }
                TechnicianQuery = "and emp.UserId in (" + Ids.TrimEnd(',') + ")";
            }
            #endregion
            #region CreatedDateQuery
            if (TicketFilters.StartDate != new DateTime() && TicketFilters.EndDate != new DateTime())
            {
                var StartDate = TicketFilters.StartDate.SetZeroHour();
                var EndDate = TicketFilters.EndDate.SetMaxHour();
                //string StartDate = TicketFilters.StartDate.ToString("yyyy-MM-dd"); // 00:00:00.000
                //string EndDate = TicketFilters.EndDate.ToString("yyyy-MM-dd"); // 23:59:59.999
                CreatedDateQuery = string.Format("and IE.CompletionDate between '{0}' and '{1}'", StartDate, EndDate);

            }
            #endregion
            #region MyTicket
            if (!string.IsNullOrWhiteSpace(TicketFilters.MyTicket))
            {
                if (TicketFilters.MyTicket == "Created")
                {
                    CreatedByMeQuery = string.Format("and tk.CreatedByUid = '{0}'", TicketFilters.UserId);

                }
                else if (TicketFilters.MyTicket == "Assigned")
                {
                    myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(tk.TicketId,'{0}') = 1 ", TicketFilters.UserId);
                }
                else if (TicketFilters.MyTicket == "Both")
                {
                    CreatedByMeQuery = string.Format("and tk.CreatedByUid = '{0}'", TicketFilters.UserId);
                    myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(tk.TicketId,'{0}') = 1 ", TicketFilters.UserId);
                }
                else if (TicketFilters.MyTicket == "None")
                {
                    CreatedByMeQuery = string.Format("and tk.CreatedByUid != '{0}'", TicketFilters.UserId);
                    myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(tk.TicketId,'{0}') = 0 ", TicketFilters.UserId);

                }
            }

            #endregion
            #region Order
            if (!string.IsNullOrWhiteSpace(TicketFilters.order))
            {
                if (TicketFilters.order == "ascending/ticketid")
                {
                    subquery = "order by TicketId asc";
                    subquery1 = "order by TicketId asc";
                }
                else if (TicketFilters.order == "descending/ticketid")
                {
                    subquery = "order by CustomerName desc";
                    subquery1 = "order by CustomerName desc";
                }
                else if (TicketFilters.order == "ascending/customername")
                {
                    subquery = "order by CustomerName asc";
                    subquery1 = "order by CustomerName asc";
                }
                else if (TicketFilters.order == "descending/customername")
                {
                    subquery = "order by CustomerName desc";
                    subquery1 = "order by CustomerName desc";
                }
                else if (TicketFilters.order == "ascending/technician")
                {
                    subquery = "order by EmpUser asc";
                    subquery1 = "order by EmpUser asc";
                }
                else if (TicketFilters.order == "descending/technician")
                {
                    subquery = "order by EmpUser desc";
                    subquery1 = "order by EmpUser desc";
                }
                else if (TicketFilters.order == "ascending/SoldBy")
                {
                    subquery = "order by Soldby asc";
                    subquery1 = "order by Soldby asc";
                }
                else if (TicketFilters.order == "descending/SoldBy")
                {
                    subquery = "order by Soldby desc";
                    subquery1 = "order by Soldby desc";
                }
                else if (TicketFilters.order == "ascending/appointmentdate")
                {
                    subquery = "order by CompletionDate asc";
                    subquery1 = "order by CompletionDate asc";
                }
                else if (TicketFilters.order == "descending/appointmentdate")
                {
                    subquery = "order by CompletionDate desc";
                    subquery1 = "order by CompletionDate desc";
                }
                //else if (TicketFilters.order == "ascending/ticketstatus")
                //{
                //    subquery = "order by #MyList.[Status]  asc";
                //    subquery1 = "order by Status asc";
                //}
                //else if (TicketFilters.order == "descending/ticketstatus")
                //{
                //    subquery = "order by #MyList.[Status]  desc";
                //    subquery1 = "order by Status desc";
                //}
                else if (TicketFilters.order == "ascending/installed")
                {
                    subquery = "order by InstalledEquipment  asc";
                    subquery1 = "order by InstalledEquipment asc";
                }
                else if (TicketFilters.order == "descending/installed")
                {
                    subquery = "order by InstalledEquipment  desc";
                    subquery1 = "order by InstalledEquipment desc";
                }
                else if (TicketFilters.order == "ascending/points")
                {
                    subquery = "order by TotalPoint  asc";
                    subquery1 = "order by TotalPoint asc";
                }
                else if (TicketFilters.order == "descending/points")
                {
                    subquery = "order by TotalPoint  desc";
                    subquery1 = "order by TotalPoint desc";
                }
                else if (TicketFilters.order == "ascending/description")
                {
                    subquery = "order by Description  asc";
                    subquery1 = "order by Description asc";
                }
                else if (TicketFilters.order == "descending/description")
                {
                    subquery = "order by Description  desc";
                    subquery1 = "order by Description desc";
                }
                else if (TicketFilters.order == "ascending/cost")
                {
                    subquery = "order by CustomerCost  asc";
                    subquery1 = "order by CustomerCost asc";
                }
                else if (TicketFilters.order == "descending/cost")
                {
                    subquery = "order by CustomerCost desc";
                    subquery1 = "order by CustomerCost desc";
                }
                else
                {
                    subquery = "order by [AppointmentEquipmentId]  desc";
                    subquery1 = "order by TicketId desc";
                }
            }
            else
            {
                subquery = "order by [AppointmentEquipmentId] desc";
                subquery1 = "order by TicketId desc";
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.category) && TicketFilters.category != "-1" && TicketFilters.category != "null" && TicketFilters.category != "undefined")
            {
                CategoryQuery = string.Format("and _et.Id in ({0})", TicketFilters.category);
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.manufact) && TicketFilters.manufact != "'-1'" && TicketFilters.manufact != "'null'" && TicketFilters.manufact != "undefined")
            {
                ManuQuery = string.Format("and manu.ManufacturerId in ({0})", TicketFilters.manufact);
            }
            #endregion
            sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                DECLARE @CustomerId uniqueidentifier
                                DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int
                                --DECLARE @SearchText nvarchar(50) 

                                --SET @SearchText = '%{0}%' 
                                SET @pageno = {1} --default 1
                                SET @pagesize = {2} --default 10
                                SET @CompanyId = '{3}' --97BCF758-A482-47EB-82B8-F88BF12293FF
                                SET @CustomerId = '{4}'

                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

                                    select
									 IE.AppointmentEquipmentId
									,IE.Category
									,IE.Manufacturer
									,IE.[Description]
									,IE.TicketType
									,emp2.FirstName +' '+ emp2.LastName as EmpUser
                                    ,soldby.FirstName +' '+ soldby.LastName as Soldby
									,IE.TicketId
									,IE.RepliesCount
									,IE.AttachmentsCount
									,IE.CusIdInt
									,IE.CustomerName
									,IE.CompletionDate
									,IE.SKU
									,IE.TotalPoint
									,IE.IsClosed
                                 , ISNULL(ROUND((SELECT TOP(1) ev.Cost FROM EquipmentVendor ev WHERE ev.EquipmentId = _eq.EquipmentId AND IsPrimary = 1), 2), 0) AS [CompanyCost]
									,IE.CustomerCost
									,IE.Quantity
									,IE.InstalledEquipment
									,IE.Qty
									,IE.[Status]
                                    ,{16}
									into #TestTable
									from IndividualInstalledEquipment IE
									left join CustomerAppointmentEquipment ca on ca.Id = IE.AppointmentEquipmentId
									left join Customer Cus on Cus.Id = IE.CusIdInt
                                    left join CustomerExtended ce on ce.CustomerId=Cus.CustomerId
									left join Ticket tk on tk.Id = IE.TicketId
									--left join TicketUser tu on tu.TiketId = tk.TicketId
									--left join Employee emp on emp.UserId = tu.UserId
                                    left join Employee emp on emp.UserId = CA.InstalledByUid
                                    left join Employee emp2 on emp2.UserId = IE.InstalledByUid
                                    left join Employee soldby on soldby.UserId = CA.CreatedByUid
									left join Equipment _eq on _eq.EquipmentId = ca.EquipmentId
									left join EquipmentType _et on _et.Id = _eq.EquipmentTypeId
									left join Manufacturer manu on manu.Id = _eq.ManufacturerId
									where tk.CompanyId = @CompanyId
                                    and ce.IsTestAccount != 1
                                    {5}
                                    {6}
                                        {7}
                                        {8}
                                        {9}
                                        {10}
                                        {13}
                                        {14}
                                        {15}
                                        {18}
                                        {19}
                                        {20}
                                        
                                        
									SELECT TOP (@pagesize) #ed.* into #SumTable
                                                                FROM #TestTable #ed
                                                                where AppointmentEquipmentId NOT IN(Select TOP (@pagestart) AppointmentEquipmentId from #TestTable #ed {11})
                                                                {12}
                                                                {17}
                                                                --select count(AppointmentEquipmentId) as [TotalRecord] from #TestTable
                                                                select * from #SumTable

																select sum(Qty) as TotalQuantity
																,sum(TotalPoint) as PointSum
																,sum(CustomerCost) as TotalCustomerCost
																,sum(CompanyCost) as TotalCompanyCost
																from #SumTable
															
                                                                DROP TABLE #TestTable
																DROP TABLE #SumTable";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        TicketFilters.SearchText,//0
                                        TicketFilters.PageNo,  //1
                                        TicketFilters.PageSize, //2
                                        TicketFilters.CompanyId, //3
                                        TicketFilters.CustomerId, //4
                                        searchQuery,//5
                                        ticketStatusQuery,//6
                                        ticketTypeQuery,//7
                                        assignedQuery,//8
                                        CreatedByMeQuery,//9
                                        myTicketQuery,//10
                                        subquery,//11
                                        subquery1,//12
                                        CreatedDateQuery,//13,
                                        ReportTypeQuery,//14,
                                        ReportQuery,//15,
                                        ReportColQuery,//16
                                        ReportCountQuery,//17
                                        CategoryQuery,//18
                                        ManuQuery,//19
                                        TechnicianQuery//20
                                        );

                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", TicketFilters.SearchText)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable DownloadIndividualInstalledEquipment(Guid CompanyId, DateTime? Start, DateTime? End, TicketFilter TicketFilters)
        {
            string sqlQuery = @"";
            string searchQuery = "";
            string myTicketQuery = "";
            string ticketStatusQuery = ""; ;
            string ticketTypeQuery = "";
            string assignedQuery = "";
            string CreatedByMeQuery = "";
            string subquery = "";
            string subquery1 = "";
            string CreatedDateQuery = "";
            string ReportTypeQuery = "";
            string ReportQuery = "";
            string ReportColQuery = "";
            string ReportCountQuery = "";
            string CategoryQuery = "";
            string ManuQuery = "";
            string TechnicianQuery = "";
            if (!string.IsNullOrWhiteSpace(TicketFilters.SearchText))
            {
                searchQuery = @"and (CONVERT(nvarchar(11), tk.CompletionDate, 101) like @SearchText
								or tk.[Status] like @SearchText or tk.[TicketType] like @SearchText or tk.Id like @SearchText or (select FirstName + LastName from Customer where CustomerId = tk.CustomerId) like @SearchText or (select Id from Customer where CustomerId = tk.CustomerId) like @SearchText)";
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.ReportTabType) && TicketFilters.ReportTabType == "GoBack")
            {
                ReportTypeQuery = string.Format("and convert(date, tk.CreatedDate) between '{0}' and '{1}'", TicketFilters.StartDate, TicketFilters.EndDate);
            }
            else
            {
                ReportColQuery = string.Format("'' as CountTicket");
                ReportCountQuery = string.Format("select  count(AppointmentEquipmentId) as [TotalCount] from #TestTable");
            }
            #region TicketStatus
            if (!string.IsNullOrWhiteSpace(TicketFilters.TicketStatus)
                && TicketFilters.TicketStatus != "-1" && TicketFilters.TicketStatus != "null")
            {
                ticketStatusQuery = string.Format("and tk.[Status] in ('{0}')", TicketFilters.TicketStatus);
            }
            #endregion
            #region Assigned
            if (!string.IsNullOrWhiteSpace(TicketFilters.AssignedUserTicket) && TicketFilters.AssignedUserTicket != "-1" && TicketFilters.AssignedUserTicket != "null" && TicketFilters.AssignedUserTicket != "undefined")
            {
                assignedQuery = string.Format("and tuser.UserId in ('{0}')", TicketFilters.AssignedUserTicket);
            }
            #endregion
            #region TicketType
            if (!string.IsNullOrWhiteSpace(TicketFilters.TicketType)
                && TicketFilters.TicketType != "-1" && TicketFilters.TicketType != "null")
            {
                ticketTypeQuery = string.Format("and tk.TicketType in ('{0}')", TicketFilters.TicketType);
            }
            #endregion
            #region Technician
            if (TicketFilters.technicianlist != null && TicketFilters.technicianlist[0] != "null"
                && TicketFilters.technicianlist[0] != "" && TicketFilters.technicianlist[0] != "-1")
            {
                string[] splittechnicianlist = TicketFilters.technicianlist[0].Split(',');
                string Ids = "";
                foreach (string id in splittechnicianlist)
                {
                    Ids += string.Format("'{0}',", id);
                }
                TechnicianQuery = "and emp.UserId in (" + Ids.TrimEnd(',') + ")";
            }
            #endregion
            #region CreatedDateQuery
            if (Start != new DateTime() && End != new DateTime()
                && Start != null && End != null)
            {
                //string StartDate = Start.Value.ToString("yyyy-MM-dd"); // 00:00:00.000
                //string EndDate = End.Value.ToString("yyyy-MM-dd"); // 23:59:59.999
                var StartDate = Start.Value;
                var EndDate = End.Value;
                CreatedDateQuery = string.Format("and IE.CompletionDate between '{0}' and '{1}'", StartDate, EndDate);

            }
            #endregion
            #region MyTicket
            if (!string.IsNullOrWhiteSpace(TicketFilters.MyTicket))
            {
                if (TicketFilters.MyTicket == "Created")
                {
                    CreatedByMeQuery = string.Format("and tk.CreatedByUid = '{0}'", TicketFilters.UserId);

                }
                else if (TicketFilters.MyTicket == "Assigned")
                {
                    myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(tk.TicketId,'{0}') = 1 ", TicketFilters.UserId);
                }
                else if (TicketFilters.MyTicket == "Both")
                {
                    CreatedByMeQuery = string.Format("and tk.CreatedByUid = '{0}'", TicketFilters.UserId);
                    myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(tk.TicketId,'{0}') = 1 ", TicketFilters.UserId);
                }
                else if (TicketFilters.MyTicket == "None")
                {
                    CreatedByMeQuery = string.Format("and tk.CreatedByUid != '{0}'", TicketFilters.UserId);
                    myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(tk.TicketId,'{0}') = 0 ", TicketFilters.UserId);

                }
            }

            #endregion
            #region Order
            if (!string.IsNullOrWhiteSpace(TicketFilters.order))
            {
                if (TicketFilters.order == "ascending/ticketid")
                {
                    subquery = "order by #MyList.[Id] asc";
                    subquery1 = "order by [Id] asc";
                }
                else if (TicketFilters.order == "descending/ticketid")
                {
                    subquery = "order by #MyList.[Id] desc";
                    subquery1 = "order by [Id] desc";
                }
                else if (TicketFilters.order == "ascending/customername")
                {
                    subquery = "order by #MyList.CustomerName asc";
                    subquery1 = "order by CustomerName asc";
                }
                else if (TicketFilters.order == "descending/customername")
                {
                    subquery = "order by #MyList.CustomerName desc";
                    subquery1 = "order by CustomerName desc";
                }
                else if (TicketFilters.order == "ascending/technician")
                {
                    subquery = "order by #MyList.[EmpUser] asc";
                    subquery1 = "order by [EmpUser] asc";
                }
                else if (TicketFilters.order == "descending/technician")
                {
                    subquery = "order by #MyList.[EmpUser] desc";
                    subquery1 = "order by [EmpUser] desc";
                }
                else if (TicketFilters.order == "ascending/SKU")
                {
                    subquery = "order by #MyList.[SKU] asc";
                    subquery1 = "order by [SKU] asc";
                }
                else if (TicketFilters.order == "descending/SKU")
                {
                    subquery = "order by #MyList.[SKU] desc";
                    subquery1 = "order by [SKU] desc";
                }
                else if (TicketFilters.order == "ascending/appointmentdate")
                {
                    subquery = "order by #MyList.[CompletionDate] asc";
                    subquery1 = "order by [CompletionDate] asc";
                }
                else if (TicketFilters.order == "descending/appointmentdate")
                {
                    subquery = "order by #MyList.[CompletionDate] desc";
                    subquery1 = "order by [CompletionDate] desc";
                }
                else if (TicketFilters.order == "ascending/ticketstatus")
                {
                    subquery = "order by #MyList.[Status]  asc";
                    subquery1 = "order by Status asc";
                }
                else if (TicketFilters.order == "descending/ticketstatus")
                {
                    subquery = "order by #MyList.[Status]  desc";
                    subquery1 = "order by Status desc";
                }
                else if (TicketFilters.order == "ascending/installed")
                {
                    subquery = "order by #MyList.[InstalledEquipment]  asc";
                    subquery1 = "order by InstalledEquipment asc";
                }
                else if (TicketFilters.order == "descending/installed")
                {
                    subquery = "order by #MyList.[InstalledEquipment]  desc";
                    subquery1 = "order by InstalledEquipment desc";
                }
                else if (TicketFilters.order == "ascending/points")
                {
                    subquery = "order by #MyList.[TotalPoint]  asc";
                    subquery1 = "order by TotalPoint asc";
                }
                else if (TicketFilters.order == "descending/points")
                {
                    subquery = "order by #MyList.[TotalPoint]  desc";
                    subquery1 = "order by TotalPoint desc";
                }
                else if (TicketFilters.order == "ascending/description")
                {
                    subquery = "order by #MyList.[Description]  asc";
                    subquery1 = "order by Description asc";
                }
                else if (TicketFilters.order == "descending/description")
                {
                    subquery = "order by #MyList.[Description]  desc";
                    subquery1 = "order by Description desc";
                }
                else if (TicketFilters.order == "ascending/cost")
                {
                    subquery = "order by #MyList.[CustomerCost]  asc";
                    subquery1 = "order by CustomerCost asc";
                }
                else if (TicketFilters.order == "descending/cost")
                {
                    subquery = "order by #MyList.[CustomerCost]  desc";
                    subquery1 = "order by CustomerCost desc";
                }
                else
                {
                    subquery = "order by [Ticket Id]  desc";
                    subquery1 = "order by [Ticket Id] desc";
                }
            }
            else
            {
                subquery = "order by [Ticket Id] desc";
                subquery1 = "order by [Ticket Id] desc";
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.category) && TicketFilters.category != "-1" && TicketFilters.category != "null" && TicketFilters.category != "undefined")
            {
                CategoryQuery = string.Format("and _et.Id in ({0})", TicketFilters.category);
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.manufact) && TicketFilters.manufact != "'-1'" && TicketFilters.manufact != "'null'" && TicketFilters.manufact != "undefined")
            {
                ManuQuery = string.Format("and manu.ManufacturerId in ({0})", TicketFilters.manufact);
            }
            #endregion
            sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                DECLARE @CustomerId uniqueidentifier
                                DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int
                                --DECLARE @SearchText nvarchar(50) 

                                --SET @SearchText = '%{0}%' 
                                SET @pageno = 1 --default 1
                                SET @pagesize = 10 --default 10
                                SET @CompanyId = '{3}' --97BCF758-A482-47EB-82B8-F88BF12293FF
                                SET @CustomerId = '{4}'

                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

                                    select
                                     IE.TicketId as [Ticket Id]
                                    ,IE.TicketType as [Ticket Type]
                                    ,IE.[Status] as [Ticket Status]
                                    ,IE.CusIdInt as [Customer Id]
									,IE.CustomerName as [Customer Name]
                                    ,FORMAT(IE.CompletionDate,'MM/dd/yyyy') as [Appt. Date]
                                    ,emp2.FirstName +' '+ emp2.LastName as [Technician]
                                    ,soldby.FirstName +' '+ soldby.LastName as [Sold By]
                                    ,IE.SKU
									,IE.[Description]
                                    ,IE.Qty as [Installed]
									,cast(IE.TotalPoint as decimal(12,2)) as [Points]
                                   , ISNULL(ROUND((SELECT TOP(1) ev.Cost FROM EquipmentVendor ev WHERE ev.EquipmentId = _eq.EquipmentId AND IsPrimary = 1), 2), 0) AS [Company Cost]
									,cast(IE.CustomerCost as decimal(12,2)) as [Customer Cost]
									into #TestTable
									from IndividualInstalledEquipment IE
									left join CustomerAppointmentEquipment ca on ca.Id = IE.AppointmentEquipmentId
									left join Customer Cus on Cus.Id = IE.CusIdInt
                                    left join CustomerExtended ce on ce.CustomerId=Cus.CustomerId
									left join Ticket tk on tk.Id = IE.TicketId
									--left join TicketUser tu on tu.TiketId = tk.TicketId
									--left join Employee emp on emp.UserId = tu.UserId
                                    left join Employee emp on emp.UserId = CA.InstalledByUid
                                    left join Employee emp2 on emp2.UserId = IE.InstalledByUid
                                    left join Employee soldby on soldby.UserId = ca.CreatedByUid
									left join Equipment _eq on _eq.EquipmentId = ca.EquipmentId
                                   -- left join EquipmentVendor EV on EV.EquipmentId = _eq.EquipmentId
									left join EquipmentType _et on _et.Id = _eq.EquipmentTypeId
									left join Manufacturer manu on manu.Id = _eq.ManufacturerId
									where tk.CompanyId = @CompanyId and ce.IsTestAccount != 1
                                    {5}
                                    {6}
                                        {7}
                                        {8}
                                        {9}
                                        {10}
                                        {13}
                                        {14}
                                        {15}
                                        {18}
                                        {19}
                                        {20}
                                        
                                        
									SELECT #ed.*
                                                                FROM #TestTable #ed
                                                                --where AppointmentEquipmentId NOT IN(Select TOP (@pagestart) AppointmentEquipmentId from #TestTable #ed {11})
                                                                {12}
                                                                DROP TABLE #TestTable";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        TicketFilters.SearchText,//0
                                        TicketFilters.PageNo,  //1
                                        TicketFilters.PageSize, //2
                                        CompanyId, //3
                                        TicketFilters.CustomerId, //4
                                        searchQuery,//5
                                        ticketStatusQuery,//6
                                        ticketTypeQuery,//7
                                        assignedQuery,//8
                                        CreatedByMeQuery,//9
                                        myTicketQuery,//10
                                        subquery,//11
                                        subquery1,//12
                                        CreatedDateQuery,//13,
                                        ReportTypeQuery,//14,
                                        ReportQuery,//15,
                                        ReportColQuery,//16
                                        ReportCountQuery,//17
                                        CategoryQuery,//18
                                        ManuQuery,//19
                                        TechnicianQuery//20
                                        );

                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", TicketFilters.SearchText)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataSet GetTicketListAllByFilter(TicketFilter TicketFilters)
        {
            string sqlQuery = @"";
            string searchQuery = "";
            string myTicketQuery = "";
            string ticketStatusQuery = "";
            string ticketTypeQuery = "";
            string assignedQuery = "";
            string CreatedByMeQuery = "";
            string subquery = "";
            string subquery1 = "";
            string CreatedDateQuery = "";
            string ReportTypeQuery = "";
            string ReportQuery = "";
            string ReportColQuery = "";
            string ReportCountQuery = "";
            string CategoryQuery = "";
            string ManuQuery = "";
            string TechnicianQuery = "";
            string EquipmentStatus = "";
            string EquipmentStatus2 = "";
            if (!string.IsNullOrWhiteSpace(TicketFilters.SearchText))
            {
                searchQuery = @"and (CONVERT(nvarchar(11), tk.CompletionDate, 101) like @SearchText
								or tk.[Status] like @SearchText or tk.[TicketType] like @SearchText or tk.Id like @SearchText or (select FirstName + LastName from Customer where CustomerId = tk.CustomerId) like @SearchText or (select Id from Customer where CustomerId = tk.CustomerId) like @SearchText)";
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.ReportTabType) && TicketFilters.ReportTabType == "GoBack")
            {
                ReportTypeQuery = string.Format("and convert(date, tk.CreatedDate) between '{0}' and '{1}'", TicketFilters.StartDate, TicketFilters.EndDate);
                ReportQuery = string.Format("and countticket > 1");
                ReportColQuery = string.Format("(select count(tik.Id) from ticket tik where convert(date, tik.CreatedDate) between '{0}' and '{1}' and tik.CustomerId=cs.CustomerId) as CountTicket", TicketFilters.StartDate, TicketFilters.EndDate);
                ReportCountQuery = string.Format("select  count(Id) as [TotalCount] from #TicketData where countticket > 1");
            }
            else
            {
                ReportColQuery = string.Format("'' as CountTicket");
                ReportCountQuery = string.Format("select  count(Id) as [TotalCount] from #TicketData");
            }
            #region TicketStatus
            if (!string.IsNullOrWhiteSpace(TicketFilters.TicketStatus)
                && TicketFilters.TicketStatus != "-1" && TicketFilters.TicketStatus != "null")
            {
                ticketStatusQuery = string.Format("and tk.[Status] in ('{0}')", TicketFilters.TicketStatus);
            }
            #endregion

            #region Assigned
            if (!string.IsNullOrWhiteSpace(TicketFilters.AssignedUserTicket) && TicketFilters.AssignedUserTicket != "-1" && TicketFilters.AssignedUserTicket != "null")
            {
                assignedQuery = string.Format("and tuser.UserId in ('{0}')", TicketFilters.AssignedUserTicket);
            }
            #endregion


            #region TicketType
            if (!string.IsNullOrWhiteSpace(TicketFilters.TicketType)
                && TicketFilters.TicketType != "-1" && TicketFilters.TicketType != "null")
            {
                ticketTypeQuery = string.Format("and tk.TicketType in ('{0}')", TicketFilters.TicketType);
            }
            #endregion
            #region Technician
            //if (!string.IsNullOrWhiteSpace(TicketFilters.technician)
            //    && TicketFilters.technician != "-1" && TicketFilters.technician != "null")
            if (TicketFilters.technicianlist != null && TicketFilters.technicianlist[0] != "null"
                && TicketFilters.technicianlist[0] != "" && TicketFilters.technicianlist[0] != "-1")
            {
                string Ids = "";
                foreach (string id in TicketFilters.technicianlist)
                {
                    Ids += string.Format("'{0}',", id);
                }

                TechnicianQuery = "and emp.UserId in (" + Ids.TrimEnd(',') + ")";
                //TechnicianQuery = string.Format("and emp.FirstName + ' ' + emp.LastName in ({0})", TicketFilters.technicianlist);
            }
            #endregion
            #region CreatedDateQuery
            if (TicketFilters.StartDate != new DateTime() && TicketFilters.EndDate != new DateTime())
            {
                //string StartDate = TicketFilters.StartDate.ToString("yyyy-MM-dd 00:00:00.000"); //("yyyy-MM-dd HH:mm:ss:fff")
                //string EndDate = TicketFilters.EndDate.ToString("yyyy-MM-dd 23:59:59.999"); //("yyyy-MM-dd HH:mm:ss:fff")

                var StartDate = TicketFilters.StartDate.SetZeroHour();
                var EndDate = TicketFilters.EndDate.SetMaxHour();
                CreatedDateQuery = string.Format("and tk.CompletionDate between '{0}' and '{1}'", StartDate, EndDate);
            }
            #endregion
            #region Installed Status
            if (!string.IsNullOrWhiteSpace(TicketFilters.EquipmentStatus)
                && TicketFilters.EquipmentStatus != "-1" && TicketFilters.EquipmentStatus == "Installed")
            {
                EquipmentStatus = string.Format("and _cae.QuantityLeftEquipment > 0", TicketFilters.EquipmentStatus);
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.EquipmentStatus)
                && TicketFilters.EquipmentStatus != "-1" && TicketFilters.EquipmentStatus == "NotInstalled")
            {
                EquipmentStatus2 = string.Format("and (_cae.QuantityLeftEquipment = 0 or _cae.QuantityLeftEquipment is null) ", TicketFilters.EquipmentStatus);
            }
            #endregion
            #region MyTicket
            if (!string.IsNullOrWhiteSpace(TicketFilters.MyTicket))
            {
                if (TicketFilters.MyTicket == "Created")
                {
                    CreatedByMeQuery = string.Format("and tk.CreatedByUid = '{0}'", TicketFilters.UserId);

                }
                else if (TicketFilters.MyTicket == "Assigned")
                {
                    myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(tk.TicketId,'{0}') = 1 ", TicketFilters.UserId);
                }
                else if (TicketFilters.MyTicket == "Both")
                {
                    CreatedByMeQuery = string.Format("and tk.CreatedByUid = '{0}'", TicketFilters.UserId);
                    myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(tk.TicketId,'{0}') = 1 ", TicketFilters.UserId);
                }
                else if (TicketFilters.MyTicket == "None")
                {
                    CreatedByMeQuery = string.Format("and tk.CreatedByUid != '{0}'", TicketFilters.UserId);
                    myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(tk.TicketId,'{0}') = 0 ", TicketFilters.UserId);

                }
            }

            #endregion
            #region Order
            if (!string.IsNullOrWhiteSpace(TicketFilters.order))
            {
                if (TicketFilters.order == "ascending/ticketid")
                {
                    subquery = "order by #ticket_filter.[Id] asc";
                    subquery1 = "order by [Id] asc";
                }
                else if (TicketFilters.order == "descending/ticketid")
                {
                    subquery = "order by #ticket_filter.[Id] desc";
                    subquery1 = "order by [Id] desc";
                }
                else if (TicketFilters.order == "ascending/customername")
                {
                    subquery = "order by #ticket_filter.CustomerName asc";
                    subquery1 = "order by CustomerName asc";
                }
                else if (TicketFilters.order == "descending/customername")
                {
                    subquery = "order by #ticket_filter.CustomerName desc";
                    subquery1 = "order by CustomerName desc";
                }
                else if (TicketFilters.order == "ascending/Quantity")
                {
                    subquery = "order by #ticket_filter.[Quantity] asc";
                    subquery1 = "order by [Quantity] asc";
                }
                else if (TicketFilters.order == "descending/Quantity")
                {
                    subquery = "order by #ticket_filter.[Quantity] desc";
                    subquery1 = "order by [Quantity] desc";
                }
                else if (TicketFilters.order == "ascending/Installed")
                {
                    subquery = "order by #ticket_filter.[InstalledEquipment] asc";
                    subquery1 = "order by [InstalledEquipment] asc";
                }
                else if (TicketFilters.order == "descending/Installed")
                {
                    subquery = "order by #ticket_filter.[InstalledEquipment] desc";
                    subquery1 = "order by [InstalledEquipment] desc";
                }
                else if (TicketFilters.order == "ascending/appointmentdate")
                {
                    subquery = "order by #ticket_filter.[CompletionDate] asc";
                    subquery1 = "order by [CompletionDate] asc";
                }
                else if (TicketFilters.order == "descending/appointmentdate")
                {
                    subquery = "order by #ticket_filter.[CompletionDate] desc";
                    subquery1 = "order by [CompletionDate] desc";
                }
                else if (TicketFilters.order == "ascending/status")
                {
                    subquery = "order by #ticket_filter.[Status]  asc";
                    subquery1 = "order by Status asc";
                }
                else if (TicketFilters.order == "descending/status")
                {
                    subquery = "order by #ticket_filter.[Status]  desc";
                    subquery1 = "order by Status desc";
                }
                else if (TicketFilters.order == "ascending/technician")
                {
                    subquery = "order by #ticket_filter.[EmpUser]  asc";
                    subquery1 = "order by EmpUser asc";
                }
                else if (TicketFilters.order == "descending/technician")
                {
                    subquery = "order by #ticket_filter.[EmpUser]  desc";
                    subquery1 = "order by EmpUser desc";
                }
                else if (TicketFilters.order == "ascending/SKU")
                {
                    subquery = "order by #ticket_filter.[SKU]  asc";
                    subquery1 = "order by SKU asc";
                }
                else if (TicketFilters.order == "descending/SKU")
                {
                    subquery = "order by #ticket_filter.[SKU]  desc";
                    subquery1 = "order by SKU desc";
                }
                else if (TicketFilters.order == "ascending/Points")
                {
                    subquery = "order by #ticket_filter.[TotalPoint]  asc";
                    subquery1 = "order by TotalPoint asc";
                }
                else if (TicketFilters.order == "descending/Points")
                {
                    subquery = "order by #ticket_filter.[TotalPoint]  desc";
                    subquery1 = "order by TotalPoint desc";
                }
                else if (TicketFilters.order == "ascending/Description")
                {
                    subquery = "order by #ticket_filter.[Description]  asc";
                    subquery1 = "order by Description asc";
                }
                else if (TicketFilters.order == "descending/Description")
                {
                    subquery = "order by #ticket_filter.[Description]  desc";
                    subquery1 = "order by Description desc";
                }
                else if (TicketFilters.order == "ascending/Cost")
                {
                    subquery = "order by #ticket_filter.[CustomerCost]  asc";
                    subquery1 = "order by CustomerCost asc";
                }
                else if (TicketFilters.order == "descending/Cost")
                {
                    subquery = "order by #ticket_filter.[CustomerCost]  desc";
                    subquery1 = "order by CustomerCost desc";
                }
                else
                {
                    subquery = "order by #ticket_filter.[Id]  desc";
                    subquery1 = "order by Id desc";
                }
            }
            else
            {
                subquery = "order by #ticket_filter.[Id] desc";
                subquery1 = "order by Id desc";
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.category) && TicketFilters.category != "-1" && TicketFilters.category != "null" && TicketFilters.category != "undefined")
            {
                CategoryQuery = string.Format("and _et.Id in ({0})", TicketFilters.category);
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.manufact) && TicketFilters.manufact != "'-1'" && TicketFilters.manufact != "'null'" && TicketFilters.manufact != "undefined")
            {
                ManuQuery = string.Format("and manu.ManufacturerId in ({0})", TicketFilters.manufact);
            }
            #endregion
            sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                DECLARE @CustomerId uniqueidentifier
                                DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int
                               -- DECLARE @SearchText nvarchar(50) 

                                --SET @SearchText = '%{0}%' 
                                SET @pageno = {1} --default 1
                                SET @pagesize = {2} --default 10
                                SET @CompanyId = '{3}' --97BCF758-A482-47EB-82B8-F88BF12293FF
                                SET @CustomerId = '{4}'

                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

 
                                select distinct _cae.Id as AppointmentEquipmentId,
										_et.Name as Category,
										manu.Name as Manufacturer ,
										  _eq.Name as [Description],
										  lktype.DisplayText as TicketType,
										  emp.FirstName + ' ' + emp.LastName as EmpUser,
                                        tk.Id, tk.Status,
                                        (select count(id) from TicketFile where TicketId = tk.TicketId)
		                                        + (select count(id) from TicketReply where TicketId = tk.TicketId) as RepliesCount,
                                        (select count(id) from TicketFile where TicketId = tk.TicketId) as AttachmentsCount,
                                        cus.Id as CusIdInt,
                                        cus.FirstName + ' ' + cus.LastName   as CustomerName,
                                        tk.CompletionDate,
                                        _eq.SKU as SKU
                                        ,Format(_eq.Point*_cae.Quantity,'N2') as TotalPoint
                                        ,tk.IsClosed
                                        ,(select Cost from EquipmentVendor where EquipmentId = _eq.EquipmentId and IsPrimary = 1)*_cae.Quantity as CompanyCost
                                        ,_cae.UnitPrice*_cae.Quantity as CustomerCost
                                        ,_cae.Quantity
										,_cae.QuantityLeftEquipment as InstalledEquipment
                                        into #TicketData
                                        from CustomerAppointmentEquipment _cae
										left join ticket tk on _cae.AppointmentId = tk.TicketId
										left join Equipment _eq on _eq.EquipmentId = _cae.EquipmentId
										left join EquipmentType _et on _et.Id = _eq.EquipmentTypeId
										left join Manufacturer manu on manu.Id = _eq.ManufacturerId
										--left join TicketUser tu on tu.TiketId = tk.TicketId
										left join Employee emp on emp.UserId = _cae.InstalledByUid
                                        left join Lookup lktype on  lktype.DataKey ='TicketType'
                                        left join Customer cus on cus.CustomerId = tk.CustomerId  
                                        left join CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                        and lktype.DataValue = tk.TicketType
		                                where tk.CompanyId = @CompanyId and ce.IsTestAccount != 1
                                        and _eq.EquipmentClassId=1
                                        {5}
                                        {6}
                                        {7}
                                        {18}
                                        {19}
                                        {20}
                                        {21}
                                        {22}
                                        {13}
                                 --       {11}
                                        --{12}
										select * into #ticket_filter from #TicketData where CusIdInt is not null

                                SELECT TOP (@pagesize) * into #TestTable FROM #ticket_filter
                                    where AppointmentEquipmentId NOT IN(Select TOP (@pagestart) AppointmentEquipmentId from #TicketData order by #TicketData.Id desc) 
                                    order by Id desc
                                --{11}
                              --       order by AppointmentEquipmentId desc
	                           

	                            select  count(*) as [TotalCount] from #ticket_filter
								select * from #TestTable
								select sum(Quantity) as TotalQuantity 
                                ,sum(InstalledEquipment) as TotalInstalledQuantity
								,sum( cast(TotalPoint as decimal(12,2))) as TotalPoint
								,sum(CompanyCost) as TotalCompanyCost
								,sum(CustomerCost) as TotalCustomerCost
								from #TestTable

                                DROP TABLE #TicketData
								DROP TABLE #ticket_filter
								DROP TABLE #TestTable
                                    ";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        TicketFilters.SearchText,//0
                                        TicketFilters.PageNo,  //1
                                        TicketFilters.PageSize, //2
                                        TicketFilters.CompanyId, //3
                                        TicketFilters.CustomerId, //4
                                        searchQuery,//5
                                        ticketStatusQuery,//6
                                        ticketTypeQuery,//7
                                        assignedQuery,//8
                                        CreatedByMeQuery,//9
                                        myTicketQuery,//10
                                        subquery,//11
                                        subquery1,//12
                                        CreatedDateQuery,//13,
                                        ReportTypeQuery,//14,
                                        ReportQuery,//15,
                                        ReportColQuery,//16
                                        ReportCountQuery,//17
                                        CategoryQuery,//18
                                        ManuQuery,//19
                                        TechnicianQuery,//20
                                        EquipmentStatus,//21
                                        EquipmentStatus2
                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", TicketFilters.SearchText)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetTechnicianList(TicketFilter TicketFilters)
        {
            string sqlQuery = @"";



            sqlQuery = @"
 
                                select distinct 
									
										  emp.FirstName + ' ' + emp.LastName as EmpUser
                                      
                                        from CustomerAppointmentEquipment _cae
										left join ticket tk on _cae.AppointmentId = tk.TicketId
										left join Equipment _eq on _eq.EquipmentId = _cae.EquipmentId
										left join EquipmentType _et on _et.Id = _eq.EquipmentTypeId
										left join Manufacturer manu on manu.Id = _eq.ManufacturerId
										left join Employee emp on emp.UserId = tk.CreatedBy
                                        left join Lookup lktype on  lktype.DataKey ='TicketType'
                                        --left join Customer cus on cus.CustomerId = tk.CustomerId  
                                       
		                                where tk.CompanyId = '{0}'
                                        and _eq.EquipmentClassId=1
                                    
                                  
							
                                    ";

            try
            {
                sqlQuery = string.Format(sqlQuery, TicketFilters.CompanyId

                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public DataTable GetTicketListInstallReportByFilter(Guid CompanyId, DateTime? Start, DateTime? End, TicketFilter TicketFilters)
        {
            string sqlQuery = @"";
            string searchQuery = "";
            string searchQuery2 = "";
            string searchQuery3 = "";
            string searchQuery4 = "";
            string myTicketQuery = "";
            string ticketStatusQuery = "";
            string ticketStatusQuery2 = "";
            string ticketStatusQuery3 = "";
            string ticketStatusQuery4 = "";
            string ticketTypeQuery = "";
            string ticketTypeQuery2 = "";
            string ticketTypeQuery3 = "";
            string ticketTypeQuery4 = "";
            string assignedQuery = "";
            string CreatedByMeQuery = "";
            string subquery = "";
            string subquery1 = "";
            string CreatedDateQuery = "";
            string CreatedDateQuery2 = "";
            string CreatedDateQuery3 = "";
            string CreatedDateQuery4 = "";
            string ReportTypeQuery = "";
            string ReportQuery = "";
            string ReportColQuery = "";
            string ReportCountQuery = "";
            string CategoryQuery = "";
            string CategoryQuery2 = "";
            string CategoryQuery3 = "";
            string CategoryQuery4 = "";
            string ManuQuery = "";
            string ManuQuery2 = "";
            string ManuQuery3 = "";
            string ManuQuery4 = "";
            string sdate = "";
            string qatechnician = "";
            string qatechnician2 = "";
            string qatechnician3 = "";
            string qatechnician4 = "";
            string edate = "";
            //string EquipmentStatus = "";
            //string EquipmentStatus2 = "";
            //string Quantity = "";
            //string InstalledQuantity = "";
            if (!string.IsNullOrWhiteSpace(TicketFilters.SearchText))
            {
                searchQuery = @"and (CONVERT(nvarchar(11), tk.CompletionDate, 101) like @SearchText
								or tk.[Status] like @SearchText or tk.[TicketType] like @SearchText or tk.Id like @SearchText or (select FirstName + LastName from Customer where CustomerId = tk.CustomerId) like @SearchText or (select Id from Customer where CustomerId = tk.CustomerId) like @SearchText)";
                searchQuery2 = @"and (CONVERT(nvarchar(11), tk2.CompletionDate, 101) like @SearchText
								or tk2.[Status] like @SearchText or tk2.[TicketType] like @SearchText or tk2.Id like @SearchText or (select FirstName + LastName from Customer where CustomerId = tk2.CustomerId) like @SearchText or (select Id from Customer where CustomerId = tk2.CustomerId) like @SearchText)";
                searchQuery3 = @"and (CONVERT(nvarchar(11), tk3.CompletionDate, 101) like @SearchText
								or tk3.[Status] like @SearchText or tk3.[TicketType] like @SearchText or tk3.Id like @SearchText or (select FirstName + LastName from Customer where CustomerId = tk3.CustomerId) like @SearchText or (select Id from Customer where CustomerId = tk3.CustomerId) like @SearchText)";
                searchQuery4 = @"and (CONVERT(nvarchar(11), tk4.CompletionDate, 101) like @SearchText
								or tk4.[Status] like @SearchText or tk4.[TicketType] like @SearchText or tk4.Id like @SearchText or (select FirstName + LastName from Customer where CustomerId = tk4.CustomerId) like @SearchText or (select Id from Customer where CustomerId = tk4.CustomerId) like @SearchText)";
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.ReportTabType) && TicketFilters.ReportTabType == "GoBack")
            {
                ReportTypeQuery = string.Format("and convert(date, tk.CreatedDate) between '{0}' and '{1}'", TicketFilters.StartDate, TicketFilters.EndDate);
                ReportQuery = string.Format("and countticket > 1");
                ReportColQuery = string.Format("(select count(tik.Id) from ticket tik where convert(date, tik.CreatedDate) between '{0}' and '{1}' and tik.CustomerId=cs.CustomerId) as CountTicket", TicketFilters.StartDate, TicketFilters.EndDate);
                ReportCountQuery = string.Format("select  count(Id) as [TotalCount] from #TicketData where countticket > 1");
            }
            else
            {
                ReportColQuery = string.Format("'' as CountTicket");
                ReportCountQuery = string.Format("select  count(Id) as [TotalCount] from #TicketData");
            }
            #region TicketStatus
            if (!string.IsNullOrWhiteSpace(TicketFilters.TicketStatus)
                && TicketFilters.TicketStatus != "-1" && TicketFilters.TicketStatus != "null")
            {
                ticketStatusQuery = string.Format("and tk.[Status] in ('{0}')", TicketFilters.TicketStatus);
                ticketStatusQuery2 = string.Format("and tk2.[Status] in ('{0}')", TicketFilters.TicketStatus);
                ticketStatusQuery3 = string.Format("and tk3.[Status] in ('{0}')", TicketFilters.TicketStatus);
                ticketStatusQuery4 = string.Format("and tk4.[Status] in ('{0}')", TicketFilters.TicketStatus);
            }

            //if (!string.IsNullOrWhiteSpace(TicketFilters.technician) && TicketFilters.technician != "-1" && TicketFilters.technician != "null" && TicketFilters.technician != "'null'")
            //{
            //    qatechnician = string.Format("and emp.FirstName + ' ' + emp.LastName in ({0})", TicketFilters.technician);
            //}
            #endregion
            #region Technician
            //if (!string.IsNullOrWhiteSpace(TicketFilters.technician)
            //    && TicketFilters.technician != "-1" && TicketFilters.technician != "null")
            if (TicketFilters.technicianlist != null && TicketFilters.technicianlist[0] != "null"
                && TicketFilters.technicianlist[0] != "" && TicketFilters.technicianlist[0] != "-1")
            {
                string[] splittechnicianlist = TicketFilters.technicianlist[0].Split(',');
                string Ids = "";
                foreach (string id in splittechnicianlist)
                {
                    Ids += string.Format("'{0}',", id);
                }
                qatechnician = "and emp.UserId in (" + Ids.TrimEnd(',') + ")";
                qatechnician2 = "and emp2.UserId in (" + Ids.TrimEnd(',') + ")";
                qatechnician3 = "and emp3.UserId in (" + Ids.TrimEnd(',') + ")";
                qatechnician4 = "and emp4.UserId in (" + Ids.TrimEnd(',') + ")";
            }
            #endregion
            #region Assigned
            if (!string.IsNullOrWhiteSpace(TicketFilters.AssignedUserTicket) && TicketFilters.AssignedUserTicket != "-1" && TicketFilters.AssignedUserTicket != "null")
            {
                assignedQuery = string.Format("and tuser.UserId in ('{0}')", TicketFilters.AssignedUserTicket);
            }
            #endregion


            #region TicketType
            if (!string.IsNullOrWhiteSpace(TicketFilters.TicketType)
                && TicketFilters.TicketType != "-1" && TicketFilters.TicketType != "null")
            {
                ticketTypeQuery = string.Format("and tk.TicketType in ('{0}')", TicketFilters.TicketType);
                ticketTypeQuery2 = string.Format("and tk2.TicketType in ('{0}')", TicketFilters.TicketType);
                ticketTypeQuery3 = string.Format("and tk3.TicketType in ('{0}')", TicketFilters.TicketType);
                ticketTypeQuery4 = string.Format("and tk4.TicketType in ('{0}')", TicketFilters.TicketType);
            }
            #endregion
            #region CreatedDateQuery
            if (TicketFilters.StartDate != new DateTime() && TicketFilters.EndDate != new DateTime())
            {
                var StartDate = TicketFilters.StartDate.SetZeroHour().UTCToClientTime();
                var EndDate = TicketFilters.EndDate.SetMaxHour().UTCToClientTime();
                CreatedDateQuery = string.Format("and tk.CreatedDate between '{0}' and '{1}'", StartDate, EndDate);
                CreatedDateQuery2 = string.Format("and tk2.CreatedDate between '{0}' and '{1}'", StartDate, EndDate);
                CreatedDateQuery3 = string.Format("and tk3.CreatedDate between '{0}' and '{1}'", StartDate, EndDate);
                CreatedDateQuery4 = string.Format("and tk4.CreatedDate between '{0}' and '{1}'", StartDate, EndDate);
            }
            #endregion
            #region MyTicket
            if (!string.IsNullOrWhiteSpace(TicketFilters.MyTicket))
            {
                if (TicketFilters.MyTicket == "Created")
                {
                    CreatedByMeQuery = string.Format("and tk.CreatedByUid = '{0}'", TicketFilters.UserId);

                }
                else if (TicketFilters.MyTicket == "Assigned")
                {
                    myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(tk.TicketId,'{0}') = 1 ", TicketFilters.UserId);
                }
                else if (TicketFilters.MyTicket == "Both")
                {
                    CreatedByMeQuery = string.Format("and tk.CreatedByUid = '{0}'", TicketFilters.UserId);
                    myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(tk.TicketId,'{0}') = 1 ", TicketFilters.UserId);
                }
                else if (TicketFilters.MyTicket == "None")
                {
                    CreatedByMeQuery = string.Format("and tk.CreatedByUid != '{0}'", TicketFilters.UserId);
                    myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(tk.TicketId,'{0}') = 0 ", TicketFilters.UserId);

                }
            }

            #endregion
            #region Installed Status
            //if (!string.IsNullOrWhiteSpace(TicketFilters.EquipmentStatus)
            //    && TicketFilters.EquipmentStatus != "-1" && TicketFilters.EquipmentStatus == "Installed")
            //{
            //    EquipmentStatus = string.Format("and _cae.QuantityLeftEquipment > 0", TicketFilters.EquipmentStatus);
            //}
            //if (!string.IsNullOrWhiteSpace(TicketFilters.EquipmentStatus)
            //    && TicketFilters.EquipmentStatus != "-1" && TicketFilters.EquipmentStatus == "NotInstalled")
            //{
            //    EquipmentStatus2 = string.Format("and (_cae.QuantityLeftEquipment = 0 or _cae.QuantityLeftEquipment is null) ", TicketFilters.EquipmentStatus);
            //}
            #endregion
            #region All Equipment
            //if (!string.IsNullOrWhiteSpace(TicketFilters.AllEquipment)
            //    && TicketFilters.AllEquipment == "True")
            //{
            //    Quantity = string.Format(",case when _cae.Quantity = '-1' Then ' ' else _cae.Quantity end as Quantity");
            //    InstalledQuantity = string.Format(",case when _cae.QuantityLeftEquipment = '-1' Then ' ' else _cae.QuantityLeftEquipment end as [Installed]");
            //}
            #endregion
            #region Order
            if (!string.IsNullOrWhiteSpace(TicketFilters.order))
            {
                if (TicketFilters.order == "ascending/ticketid")
                {
                    subquery = "order by #TicketData.[Id] asc";
                    subquery1 = "order by [Id] asc";
                }
                else if (TicketFilters.order == "descending/ticketid")
                {
                    subquery = "order by #TicketData.[Id] desc";
                    subquery1 = "order by [Id] desc";
                }
                if (TicketFilters.order == "ascending/customername")
                {
                    subquery = "order by #TicketData.CustomerName asc";
                    subquery1 = "order by CustomerName asc";
                }
                else if (TicketFilters.order == "descending/customername")
                {
                    subquery = "order by #TicketData.CustomerName desc";
                    subquery1 = "order by CustomerName desc";
                }
                else if (TicketFilters.order == "ascending/tickettype")
                {
                    subquery = "order by #TicketData.[TicketType] asc";
                    subquery1 = "order by [TicketType] asc";
                }
                else if (TicketFilters.order == "descending/tickettype")
                {
                    subquery = "order by #TicketData.[TicketType] desc";
                    subquery1 = "order by [TicketType] desc";
                }
                else if (TicketFilters.order == "ascending/assignto")
                {
                    subquery = "order by #TicketData.[AssignedTo] asc";
                    subquery1 = "order by [AssignedTo] asc";
                }
                else if (TicketFilters.order == "descending/assignto")
                {
                    subquery = "order by #TicketData.[AssignedTo] desc";
                    subquery1 = "order by [AssignedTo] desc";
                }
                else if (TicketFilters.order == "ascending/scheduledon")
                {
                    subquery = "order by #TicketData.[CompletionDate] asc";
                    subquery1 = "order by [CompletionDate] asc";
                }
                else if (TicketFilters.order == "descending/scheduledon")
                {
                    subquery = "order by #TicketData.[CompletionDate] desc";
                    subquery1 = "order by [CompletionDate] desc";
                }
                else if (TicketFilters.order == "ascending/status")
                {
                    subquery = "order by #TicketData.[Status]  asc";
                    subquery1 = "order by Status asc";
                }
                else if (TicketFilters.order == "descending/status")
                {
                    subquery = "order by #TicketData.[Status]  desc";
                    subquery1 = "order by Status desc";
                }
                else
                {
                    subquery = "order by #TicketData.[Id]  desc";
                    subquery1 = "order by Id desc";
                }
            }
            else
            {
                subquery = "order by #TicketData.[Id] desc";
                subquery1 = "order by Id desc";
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.category) && TicketFilters.category != "-1" && TicketFilters.category != "null" && TicketFilters.category != "undefined")
            {
                CategoryQuery = string.Format("and _et.Id in ({0})", TicketFilters.category);
                CategoryQuery2 = string.Format("and _et2.Id in ({0})", TicketFilters.category);
                CategoryQuery3 = string.Format("and _et3.Id in ({0})", TicketFilters.category);
                CategoryQuery4 = string.Format("and _et4.Id in ({0})", TicketFilters.category);
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.manufact) && TicketFilters.manufact != "'-1'" && TicketFilters.manufact != "'null'" && TicketFilters.manufact != "undefined")
            {
                ManuQuery = string.Format("and manu.ManufacturerId in ({0})", TicketFilters.manufact);
                ManuQuery2 = string.Format("and manu2.ManufacturerId in ({0})", TicketFilters.manufact);
                ManuQuery3 = string.Format("and manu3.ManufacturerId in ({0})", TicketFilters.manufact);
                ManuQuery4 = string.Format("and manu4.ManufacturerId in ({0})", TicketFilters.manufact);
            }
            #endregion
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
                //NameSql = NameSql.Replace("cus.", "cu.");
            }

            if (Start.HasValue && End.HasValue)
            {
                sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                               DECLARE @SearchText nvarchar(50) 

                                SET @SearchText = '%{0}%' 
                                SET @CompanyId = '{3}'--'97BCF758-A482-47EB-82B8-F88BF12293FF' 

DECLARE @MyList Table(TicketId int, TicketType nvarchar(50), [Status] nvarchar(50), CustomerId int, CustomerName nvarchar(50), AppointmentDate datetime, Technician varchar(50),
SKU nvarchar(50), Installed int, Point float, [Description] nvarchar(50),CompanyCost float, CustomerCost float)

DECLARE @IdCount int, @qty int, @i int, @j int, @getDescription nvarchar(50),
 @getTicketType nvarchar(50), @getEmpUser nvarchar(50), @getCusIdInt int,
 @getCustomerName nvarchar(50), @getCompletionDate datetime, @getSKU nvarchar(50), @getTotalPoint float, @getCompanyCost float,
 @getCustomerCost float, @getTicketId int, @getQty int, @getStatus nvarchar(50)

	set @i = 1;
	set @j = 1;

	select @IdCount = count(_cae.Id) from  CustomerAppointmentEquipment _cae
										left join ticket tk on _cae.AppointmentId = tk.TicketId
										left join Equipment _eq on _eq.EquipmentId = _cae.EquipmentId
										left join EquipmentType _et on _et.Id = _eq.EquipmentTypeId
										left join Manufacturer manu on manu.Id = _eq.ManufacturerId
										left join TicketUser tu on tu.TiketId = tk.TicketId
										left join Employee emp on emp.UserId = tu.UserId
                                        left join Lookup lktype on  lktype.DataKey ='TicketType'
                                        left join Customer cus on cus.CustomerId = tk.CustomerId  
                                        and lktype.DataValue = tk.TicketType
										where tk.CompanyId = @CompanyId
                                        and _eq.EquipmentClassId=1
										and cus.id is not null
										and _cae.QuantityLeftEquipment > 0
										{5}
										{6}
										{7}
										{18}
										{19}
										{23}
										and tk.CreatedDate between '{20}' and '{21}' 
	Select 
      Row_Number() Over (Order By ca.Id DESC) As RowNum
     ,ca.Id,Quantity,AppointmentId,ca.EquipmentId,QuantityLeftEquipment,UnitPrice
    into #CustomerAppointmentEquipment From CustomerAppointmentEquipment ca
	left join ticket tk2 on ca.AppointmentId = tk2.TicketId
										left join Equipment _eq2 on _eq2.EquipmentId = ca.EquipmentId
										left join EquipmentType _et2 on _et2.Id = _eq2.EquipmentTypeId
										left join Manufacturer manu2 on manu2.Id = _eq2.ManufacturerId
										left join TicketUser tu2 on tu2.TiketId = tk2.TicketId
										left join Employee emp2 on emp2.UserId = tu2.UserId
                                        left join Lookup lktype2 on  lktype2.DataKey ='TicketType'
                                        left join Customer cus2 on cus2.CustomerId = tk2.CustomerId  
                                        and lktype2.DataValue = tk2.TicketType
										where tk2.CompanyId = @CompanyId
                                        and _eq2.EquipmentClassId=1
										and cus2.id is not null
										and ca.QuantityLeftEquipment > 0
										{24}
										{27}
										{30}
										{33}
										{36}
										{39}
										and tk2.CreatedDate between '{20}' and '{21}' 
										
	WHILE @i <= @IdCount
	BEGIN
		select @qty = _cae4.QuantityLeftEquipment from  #CustomerAppointmentEquipment _cae4
										left join ticket tk4 on _cae4.AppointmentId = tk4.TicketId
										left join Equipment _eq4 on _eq4.EquipmentId = _cae4.EquipmentId
										left join EquipmentType _et4 on _et4.Id = _eq4.EquipmentTypeId
										left join Manufacturer manu4 on manu4.Id = _eq4.ManufacturerId
										left join TicketUser tu4 on tu4.TiketId = tk4.TicketId
										left join Employee emp4 on emp4.UserId = tu4.UserId
                                        left join Lookup lktype4 on  lktype4.DataKey ='TicketType'
                                        left join Customer cus4 on cus4.CustomerId = tk4.CustomerId  
                                        and lktype4.DataValue = tk4.TicketType
										where tk4.CompanyId = @CompanyId
                                        and _eq4.EquipmentClassId=1
										and cus4.id is not null
										and _cae4.QuantityLeftEquipment > 0
										and _cae4.RowNum = @i
										{26}
										{29}
										{32}
										{35}
										{38}
										{41}
										and tk4.CreatedDate between '{20}' and '{21}' 
		WHILE @j <= @qty
		BEGIN
			select 
			
				   @getTicketId = tk3.Id,
				   @getTicketType = lktype3.DisplayText,
				   @getDescription = _eq3.Name,
				   @getEmpUser = emp3.FirstName + ' ' + emp3.LastName,
				   @getStatus = tk3.[Status],
				   @getCusIdInt = cus3.Id,
				   @getCustomerName = cus3.FirstName + ' ' + cus3.LastName,
				   @getCompletionDate = tk3.CompletionDate,
				   @getSKU = _eq3.SKU,
				   @getTotalPoint = Format(_eq3.Point,'N2'),
				   @getCompanyCost = (select Cost from EquipmentVendor where EquipmentId = _eq3.EquipmentId and IsPrimary = 1),
				   @getCustomerCost = _cae3.UnitPrice,
				   @getQty = 1
				   from  #CustomerAppointmentEquipment _cae3
				   left join ticket tk3 on _cae3.AppointmentId = tk3.TicketId
										left join Equipment _eq3 on _eq3.EquipmentId = _cae3.EquipmentId
										left join EquipmentType _et3 on _et3.Id = _eq3.EquipmentTypeId
										left join Manufacturer manu3 on manu3.Id = _eq3.ManufacturerId
										left join TicketUser tu3 on tu3.TiketId = tk3.TicketId
										left join Employee emp3 on emp3.UserId = tu3.UserId
                                        left join Lookup lktype3 on  lktype3.DataKey ='TicketType'
                                        left join Customer cus3 on cus3.CustomerId = tk3.CustomerId  
                                        and lktype3.DataValue = tk3.TicketType
										where tk3.CompanyId = @CompanyId
                                        and _eq3.EquipmentClassId=1
										and cus3.Id is not null
										and _cae3.QuantityLeftEquipment > 0
										and _cae3.RowNum = @i
                                        {25}
										{28}
										{31}
										{34}
										{37}
										{40}
										and tk3.CreatedDate between '{20}' and '{21}'

			insert INTO @MyList values (@getTicketId, @getTicketType, @getStatus, @getCusIdInt,@getCustomerName, @getCompletionDate, @getEmpUser,
			   @getSKU, @getQty, @getTotalPoint, @getDescription,@getCompanyCost, @getCustomerCost) 

			SET @j += 1;
		END

		SET @i += 1;
		set @j = 1;
END

select * from @MyList order by TicketId desc
drop table #CustomerAppointmentEquipment
                                    ";

                sdate = Start.Value.ToString("yyyy-MM-dd 00:00:00.000");
                edate = End.Value.ToString("yyyy-MM-dd 23:59:59.999");
            }
            else
            {
                sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                               DECLARE @SearchText nvarchar(50) 

                                SET @SearchText = '%{0}%' 
                                SET @CompanyId =  '{3}' --'97BCF758-A482-47EB-82B8-F88BF12293FF'

DECLARE @MyList Table(TicketId int, TicketType nvarchar(50), [Status] nvarchar(50), CustomerId int, CustomerName nvarchar(50), AppointmentDate datetime, Technician varchar(50),
SKU nvarchar(50), Installed int, Point float, [Description] nvarchar(50),CompanyCost float, CustomerCost float)

DECLARE @IdCount int, @qty int, @i int, @j int, @getDescription nvarchar(50),
 @getTicketType nvarchar(50), @getEmpUser nvarchar(50), @getCusIdInt int,
 @getCustomerName nvarchar(50), @getCompletionDate datetime, @getSKU nvarchar(50), @getTotalPoint float, @getCompanyCost float,
 @getCustomerCost float, @getTicketId int, @getQty int, @getStatus nvarchar(50)

	set @i = 1;
	set @j = 1;

	select @IdCount = count(_cae.Id) from  CustomerAppointmentEquipment _cae
										left join ticket tk on _cae.AppointmentId = tk.TicketId
										left join Equipment _eq on _eq.EquipmentId = _cae.EquipmentId
										left join EquipmentType _et on _et.Id = _eq.EquipmentTypeId
										left join Manufacturer manu on manu.Id = _eq.ManufacturerId
										left join TicketUser tu on tu.TiketId = tk.TicketId
										left join Employee emp on emp.UserId = tu.UserId
                                        left join Lookup lktype on  lktype.DataKey ='TicketType'
                                        left join Customer cus on cus.CustomerId = tk.CustomerId  
                                        and lktype.DataValue = tk.TicketType
										where tk.CompanyId = @CompanyId
                                        and _eq.EquipmentClassId=1
										and cus.id is not null
										and _cae.QuantityLeftEquipment > 0
										{5}
										{6}
										{7}
										{18}
										{19}
										{23}
	Select 
      Row_Number() Over (Order By ca.Id DESC) As RowNum
     ,ca.Id,Quantity,AppointmentId,ca.EquipmentId,QuantityLeftEquipment,UnitPrice
    into #CustomerAppointmentEquipment From CustomerAppointmentEquipment ca
	left join ticket tk2 on ca.AppointmentId = tk2.TicketId
										left join Equipment _eq2 on _eq2.EquipmentId = ca.EquipmentId
										left join EquipmentType _et2 on _et2.Id = _eq2.EquipmentTypeId
										left join Manufacturer manu2 on manu2.Id = _eq2.ManufacturerId
										left join Employee emp2 on emp2.UserId = tk2.CreatedBy
                                        left join Lookup lktype2 on  lktype2.DataKey ='TicketType'
                                        left join Customer cus2 on cus2.CustomerId = tk2.CustomerId  
                                        and lktype2.DataValue = tk2.TicketType
										where tk2.CompanyId = @CompanyId
                                        and _eq2.EquipmentClassId=1
										and cus2.id is not null
										and ca.QuantityLeftEquipment > 0
										{24}
										{27}
										{30}
										{33}
										{36}
										{39}										
	WHILE @i <= @IdCount
	BEGIN
		select @qty = _cae4.QuantityLeftEquipment from  #CustomerAppointmentEquipment _cae4
										left join ticket tk4 on _cae4.AppointmentId = tk4.TicketId
										left join Equipment _eq4 on _eq4.EquipmentId = _cae4.EquipmentId
										left join EquipmentType _et4 on _et4.Id = _eq4.EquipmentTypeId
										left join Manufacturer manu4 on manu4.Id = _eq4.ManufacturerId
										left join TicketUser tu4 on tu4.TiketId = tk4.TicketId
										left join Employee emp4 on emp4.UserId = tu4.UserId
                                        left join Lookup lktype4 on  lktype4.DataKey ='TicketType'
                                        left join Customer cus4 on cus4.CustomerId = tk4.CustomerId  
                                        and lktype4.DataValue = tk4.TicketType
										where tk4.CompanyId = @CompanyId
                                        and _eq4.EquipmentClassId=1
										and cus4.id is not null
										and _cae4.QuantityLeftEquipment > 0
										and _cae4.RowNum = @i
										{26}
										{29}
										{32}
										{35}
										{38}
										{41}
		WHILE @j <= @qty
		BEGIN
			select 
			
				   @getTicketId = tk3.Id,
				   @getTicketType = lktype3.DisplayText,
				   @getDescription = _eq3.Name,
				   @getEmpUser = emp3.FirstName + ' ' + emp3.LastName,
				   @getStatus = tk3.[Status],
				   @getCusIdInt = cus3.Id,
				   @getCustomerName = cus3.FirstName + ' ' + cus3.LastName,
				   @getCompletionDate = tk3.CompletionDate,
				   @getSKU = _eq3.SKU,
				   @getTotalPoint = Format(_eq3.Point,'N2'),
				   @getCompanyCost = (select Cost from EquipmentVendor where EquipmentId = _eq3.EquipmentId and IsPrimary = 1),
				   @getCustomerCost = _cae3.UnitPrice,
				   @getQty = 1
				   from  #CustomerAppointmentEquipment _cae3
				   left join ticket tk3 on _cae3.AppointmentId = tk3.TicketId
										left join Equipment _eq3 on _eq3.EquipmentId = _cae3.EquipmentId
										left join EquipmentType _et3 on _et3.Id = _eq3.EquipmentTypeId
										left join Manufacturer manu3 on manu3.Id = _eq3.ManufacturerId
										left join TicketUser tu3 on tu3.TiketId = tk3.TicketId
										left join Employee emp3 on emp3.UserId = tu3.UserId
                                        left join Lookup lktype3 on  lktype3.DataKey ='TicketType'
                                        left join Customer cus3 on cus3.CustomerId = tk3.CustomerId  
                                        and lktype3.DataValue = tk3.TicketType
										where tk3.CompanyId = @CompanyId
                                        and _eq3.EquipmentClassId=1
										and cus3.Id is not null
										and _cae3.QuantityLeftEquipment > 0
										and _cae3.RowNum = @i
										{25}
										{28}
										{31}
										{34}
										{37}
										{40}

			insert INTO @MyList values (@getTicketId, @getTicketType, @getStatus, @getCusIdInt,@getCustomerName, @getCompletionDate, @getEmpUser,
			   @getSKU, @getQty, @getTotalPoint, @getDescription,@getCompanyCost, @getCustomerCost) 

			SET @j += 1;
		END

		SET @i += 1;
		set @j = 1;
END

select * from @MyList order by TicketId desc
drop table #CustomerAppointmentEquipment
                                    
                                    ";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        TicketFilters.SearchText,//0
                                        TicketFilters.PageNo,  //1
                                        TicketFilters.PageSize, //2
                                        CompanyId, //3
                                        TicketFilters.CustomerId, //4
                                        searchQuery,//5
                                        ticketStatusQuery,//6
                                        ticketTypeQuery,//7
                                        assignedQuery,//8
                                        CreatedByMeQuery,//9
                                        myTicketQuery,//10
                                        subquery,//11
                                        subquery1,//12
                                        CreatedDateQuery,//13,
                                        ReportTypeQuery,//14,
                                        ReportQuery,//15,
                                        ReportColQuery,//16
                                        ReportCountQuery,//17
                                        CategoryQuery,//18
                                        ManuQuery,//19
                                        sdate,//20
                                        edate,//21
                                        NameSql,//22
                                        qatechnician,//23
                                        searchQuery2, //24
                                        searchQuery3, //25
                                        searchQuery4, //26
                                        ticketStatusQuery2, //27
                                        ticketStatusQuery3, //28
                                        ticketStatusQuery4, //29
                                        ticketTypeQuery2,//30
                                        ticketTypeQuery3, //31
                                        ticketTypeQuery4, //32
                                        CategoryQuery2,// 33
                                        CategoryQuery3, //34
                                        CategoryQuery4, //35
                                        ManuQuery2, //36
                                        ManuQuery3, //37
                                        ManuQuery4, //38
                                        qatechnician2, //39
                                        qatechnician3, //40
                                        qatechnician4 //41

                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetTicketListAllReportByFilter(Guid CompanyId, DateTime? Start, DateTime? End, TicketFilter TicketFilters)
        {
            string sqlQuery = @"";
            string searchQuery = "";
            string myTicketQuery = "";
            string ticketStatusQuery = "";
            string ticketTypeQuery = "";
            string assignedQuery = "";
            string CreatedByMeQuery = "";
            string subquery = "";
            string subquery1 = "";
            string CreatedDateQuery = "";
            string ReportTypeQuery = "";
            string ReportQuery = "";
            string ReportColQuery = "";
            string ReportCountQuery = "";
            string CategoryQuery = "";
            string ManuQuery = "";
            string sdate = "";
            string qatechnician = "";
            string edate = "";
            string EquipmentStatus = "";
            string EquipmentStatus2 = "";
            //string Quantity = "";
            //string InstalledQuantity = "";
            if (!string.IsNullOrWhiteSpace(TicketFilters.SearchText))
            {
                searchQuery = @"and (CONVERT(nvarchar(11), tk.CompletionDate, 101) like @SearchText
								or tk.[Status] like @SearchText or tk.[TicketType] like @SearchText or tk.Id like @SearchText or (select FirstName + LastName from Customer where CustomerId = tk.CustomerId) like @SearchText or (select Id from Customer where CustomerId = tk.CustomerId) like @SearchText)";
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.ReportTabType) && TicketFilters.ReportTabType == "GoBack")
            {
                ReportTypeQuery = string.Format("and convert(date, tk.CreatedDate) between '{0}' and '{1}'", TicketFilters.StartDate, TicketFilters.EndDate);
                ReportQuery = string.Format("and countticket > 1");
                ReportColQuery = string.Format("(select count(tik.Id) from ticket tik where convert(date, tik.CreatedDate) between '{0}' and '{1}' and tik.CustomerId=cs.CustomerId) as CountTicket", TicketFilters.StartDate, TicketFilters.EndDate);
                ReportCountQuery = string.Format("select  count(Id) as [TotalCount] from #TicketData where countticket > 1");
            }
            else
            {
                ReportColQuery = string.Format("'' as CountTicket");
                ReportCountQuery = string.Format("select  count(Id) as [TotalCount] from #TicketData");
            }
            #region TicketStatus
            if (!string.IsNullOrWhiteSpace(TicketFilters.TicketStatus)
                && TicketFilters.TicketStatus != "-1" && TicketFilters.TicketStatus != "null")
            {
                ticketStatusQuery = string.Format("and tk.[Status] in ('{0}')", TicketFilters.TicketStatus);
            }

            //if (!string.IsNullOrWhiteSpace(TicketFilters.technician) && TicketFilters.technician != "-1" && TicketFilters.technician != "null" && TicketFilters.technician != "'null'")
            //{
            //    qatechnician = string.Format("and emp.FirstName + ' ' + emp.LastName in ({0})", TicketFilters.technician);
            //}
            #endregion
            #region Technician
            //if (!string.IsNullOrWhiteSpace(TicketFilters.technician)
            //    && TicketFilters.technician != "-1" && TicketFilters.technician != "null")
            if (TicketFilters.technicianlist != null && TicketFilters.technicianlist[0] != "null"
                && TicketFilters.technicianlist[0] != "" && TicketFilters.technicianlist[0] != "-1")
            {
                string[] splittechnicianlist = TicketFilters.technicianlist[0].Split(',');
                string Ids = "";
                foreach (string id in splittechnicianlist) // technicianlist
                {
                    Ids += string.Format("'{0}',", id);
                }
                qatechnician = "and emp.UserId in (" + Ids.TrimEnd(',') + ")";

                //TechnicianQuery = string.Format("and emp.FirstName + ' ' + emp.LastName in ({0})", TicketFilters.technicianlist);
            }
            #endregion
            #region Assigned
            if (!string.IsNullOrWhiteSpace(TicketFilters.AssignedUserTicket) && TicketFilters.AssignedUserTicket != "-1" && TicketFilters.AssignedUserTicket != "null")
            {
                assignedQuery = string.Format("and tuser.UserId in ('{0}')", TicketFilters.AssignedUserTicket);
            }
            #endregion


            #region TicketType
            if (!string.IsNullOrWhiteSpace(TicketFilters.TicketType)
                && TicketFilters.TicketType != "-1" && TicketFilters.TicketType != "null")
            {
                ticketTypeQuery = string.Format("and tk.TicketType in ('{0}')", TicketFilters.TicketType);
            }
            #endregion
            #region CreatedDateQuery
            if (TicketFilters.StartDate != new DateTime() && TicketFilters.EndDate != new DateTime())
            {
                //var StartDate = TicketFilters.StartDate.SetZeroHour();
                //var EndDate = TicketFilters.EndDate.SetMaxHour();
                var StartDate = Start.Value;
                var EndDate = End.Value;
                CreatedDateQuery = string.Format("and tk.CompletionDate between '{0}' and '{1}'", StartDate, EndDate);
            }
            #endregion
            #region MyTicket
            if (!string.IsNullOrWhiteSpace(TicketFilters.MyTicket))
            {
                if (TicketFilters.MyTicket == "Created")
                {
                    CreatedByMeQuery = string.Format("and tk.CreatedByUid = '{0}'", TicketFilters.UserId);

                }
                else if (TicketFilters.MyTicket == "Assigned")
                {
                    myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(tk.TicketId,'{0}') = 1 ", TicketFilters.UserId);
                }
                else if (TicketFilters.MyTicket == "Both")
                {
                    CreatedByMeQuery = string.Format("and tk.CreatedByUid = '{0}'", TicketFilters.UserId);
                    myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(tk.TicketId,'{0}') = 1 ", TicketFilters.UserId);
                }
                else if (TicketFilters.MyTicket == "None")
                {
                    CreatedByMeQuery = string.Format("and tk.CreatedByUid != '{0}'", TicketFilters.UserId);
                    myTicketQuery = string.Format("and dbo.CheckTicktAssignedUser(tk.TicketId,'{0}') = 0 ", TicketFilters.UserId);

                }
            }

            #endregion
            #region Installed Status
            if (!string.IsNullOrWhiteSpace(TicketFilters.EquipmentStatus)
                && TicketFilters.EquipmentStatus != "-1" && TicketFilters.EquipmentStatus == "Installed")
            {
                EquipmentStatus = string.Format("and _cae.QuantityLeftEquipment > 0", TicketFilters.EquipmentStatus);
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.EquipmentStatus)
                && TicketFilters.EquipmentStatus != "-1" && TicketFilters.EquipmentStatus == "NotInstalled")
            {
                EquipmentStatus2 = string.Format("and (_cae.QuantityLeftEquipment = 0 or _cae.QuantityLeftEquipment is null) ", TicketFilters.EquipmentStatus);
            }
            #endregion
            #region All Equipment
            //if (!string.IsNullOrWhiteSpace(TicketFilters.AllEquipment)
            //    && TicketFilters.AllEquipment == "True")
            //{
            //    Quantity = string.Format(",case when _cae.Quantity = '-1' Then ' ' else _cae.Quantity end as Quantity");
            //    InstalledQuantity = string.Format(",case when _cae.QuantityLeftEquipment = '-1' Then ' ' else _cae.QuantityLeftEquipment end as [Installed]");
            //}
            #endregion
            #region Order
            if (!string.IsNullOrWhiteSpace(TicketFilters.order))
            {
                if (TicketFilters.order == "ascending/ticketid")
                {
                    subquery = "order by #TicketData.[Id] asc";
                    subquery1 = "order by [Id] asc";
                }
                else if (TicketFilters.order == "descending/ticketid")
                {
                    subquery = "order by #TicketData.[Id] desc";
                    subquery1 = "order by [Id] desc";
                }
                if (TicketFilters.order == "ascending/customername")
                {
                    subquery = "order by #TicketData.CustomerName asc";
                    subquery1 = "order by CustomerName asc";
                }
                else if (TicketFilters.order == "descending/customername")
                {
                    subquery = "order by #TicketData.CustomerName desc";
                    subquery1 = "order by CustomerName desc";
                }
                else if (TicketFilters.order == "ascending/tickettype")
                {
                    subquery = "order by #TicketData.[TicketType] asc";
                    subquery1 = "order by [TicketType] asc";
                }
                else if (TicketFilters.order == "descending/tickettype")
                {
                    subquery = "order by #TicketData.[TicketType] desc";
                    subquery1 = "order by [TicketType] desc";
                }
                else if (TicketFilters.order == "ascending/assignto")
                {
                    subquery = "order by #TicketData.[AssignedTo] asc";
                    subquery1 = "order by [AssignedTo] asc";
                }
                else if (TicketFilters.order == "descending/assignto")
                {
                    subquery = "order by #TicketData.[AssignedTo] desc";
                    subquery1 = "order by [AssignedTo] desc";
                }
                else if (TicketFilters.order == "ascending/scheduledon")
                {
                    subquery = "order by #TicketData.[CompletionDate] asc";
                    subquery1 = "order by [CompletionDate] asc";
                }
                else if (TicketFilters.order == "descending/scheduledon")
                {
                    subquery = "order by #TicketData.[CompletionDate] desc";
                    subquery1 = "order by [CompletionDate] desc";
                }
                else if (TicketFilters.order == "ascending/status")
                {
                    subquery = "order by #TicketData.[Status]  asc";
                    subquery1 = "order by Status asc";
                }
                else if (TicketFilters.order == "descending/status")
                {
                    subquery = "order by #TicketData.[Status]  desc";
                    subquery1 = "order by Status desc";
                }
                else
                {
                    subquery = "order by #TicketData.[Id]  desc";
                    subquery1 = "order by Id desc";
                }
            }
            else
            {
                subquery = "order by #TicketData.[Id] desc";
                subquery1 = "order by Id desc";
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.category) && TicketFilters.category != "-1" && TicketFilters.category != "null" && TicketFilters.category != "undefined")
            {
                CategoryQuery = string.Format("and _et.Id in ({0})", TicketFilters.category);
            }
            if (!string.IsNullOrWhiteSpace(TicketFilters.manufact) && TicketFilters.manufact != "'-1'" && TicketFilters.manufact != "'null'" && TicketFilters.manufact != "undefined")
            {
                ManuQuery = string.Format("and manu.ManufacturerId in ({0})", TicketFilters.manufact);
            }
            #endregion
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
                //NameSql = NameSql.Replace("cus.", "cu.");
            }

            if (Start.HasValue && End.HasValue)
            {
                sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                              -- DECLARE @SearchText nvarchar(50) 

                                --SET @SearchText = '%{0}%' 
                                        SET @CompanyId = '{3}' --97BCF758-A482-47EB-82B8-F88BF12293FF
                                        select distinct tk.Id as TicketId,lktype.DisplayText as TicketType,tk.Status as [Status],
                                   
                             
                                        cus.Id as [Customer Id],
                                        cus.FirstName + ' ' + cus.LastName as [Customer Name],
                                        convert(date,tk.CompletionDate)  as [Appointment Date],
                                        emp.FirstName + ' ' + emp.LastName as Technician,
                                        _eq.SKU as SKU
                                        ,case when _cae.Quantity = '-1' Then ' ' else _cae.Quantity end as Quantity
                                        ,case when _cae.QuantityLeftEquipment = '-1' Then ' ' else _cae.QuantityLeftEquipment end as [Installed]
                                        ,Format(_eq.Point*_cae.Quantity, 'N2') as [Points], _eq.Name as [Description]
                                        ,cast((select Cost from EquipmentVendor where EquipmentId = _eq.EquipmentId and IsPrimary = 1)*_cae.Quantity as decimal(10,2)) as [Company Cost]
                                        --,FORMAT(_eq.SupplierCost, 'N2') as CompanyCost
                                        --,FORMAT(_cae.UnitPrice, 'N2') as CustomerCost
                                        ,(_cae.UnitPrice*_cae.Quantity) as CustomerCost                                        
                                       --,iif(tk.ReferenceTicketId is not null and tk.ReferenceTicketId > 0, tk.ReferenceTicketId, '') as [Follow up From]
                                        --,iif(tk.RescheduleTicketId is not null and tk.RescheduleTicketId > 0, tk.RescheduleTicketId, '') as [Reschedule From]
                                        from CustomerAppointmentEquipment _cae
										left join ticket tk on _cae.AppointmentId = tk.TicketId
										left join Equipment _eq on _eq.EquipmentId = _cae.EquipmentId
										left join EquipmentType _et on _et.Id = _eq.EquipmentTypeId
										left join Manufacturer manu on manu.Id = _eq.ManufacturerId
                                        left join Lookup lktype on  lktype.DataKey ='TicketType'  
                                        and lktype.DataValue = tk.TicketType
										left join TicketUser tu on tu.TiketId = tk.TicketId
										left join Employee emp on emp.UserId = _cae.InstalledByUid
                                        left join Customer cus on cus.CustomerId = tk.CustomerId
                                        left join CustomerExtended ce on ce.CustomerId=cus.CustomerId
		                                where tk.CompanyId = @CompanyId and ce.IsTestAccount != 1
                                        and cus.Id is not null
                                        and _eq.EquipmentClassId=1
                                           {5}
                                        {6}
                                        {7}
                                        {18}
                                        {19}
                                        {23}
                                        {24}
                                        {25}
                                        {13}
                                   and tk.CompletionDate between '{20}' and '{21}' 
                                        order by TicketId desc
                                    ";

                sdate = Convert.ToDateTime(Start).SetZeroHour().ToString();  //Start.Value.ToString("yyyy-MM-dd 00:00:00.000");
                edate = Convert.ToDateTime(End).SetMaxHour().ToString();  //End.Value.ToString("yyyy-MM-dd 23:59:59.999");
            }
            else
            {
                sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                         --DECLARE @SearchText nvarchar(50) 

                                --SET @SearchText = '%{0}%' 
                                        SET @CompanyId = '{3}' --97BCF758-A482-47EB-82B8-F88BF12293FF
                                        select distinct tk.Id as TicketId,lktype.DisplayText as TicketType,tk.Status as [Status],
									    cus.Id as [Customer Id],
                                        cus.FirstName + ' ' + cus.LastName   as [Customer Name],
                                        convert(date,tk.CompletionDate)  as [Appointment Date],
										emp.FirstName + ' ' + emp.LastName as Technician,
									    _eq.SKU as SKU
                                        ,case when _cae.Quantity = '-1' Then ' ' else _cae.Quantity end as Quantity
                                        ,case when _cae.QuantityLeftEquipment = '-1' Then ' ' else _cae.QuantityLeftEquipment end as [Installed]
                                        ,Format(_eq.Point*_cae.Quantity, 'N2') as [Points], _eq.Name as [Description]
                                        ,cast((select Cost from EquipmentVendor where EquipmentId = _eq.EquipmentId and IsPrimary = 1)*_cae.Quantity as decimal(10,2)) as [Company Cost]
                                        --,FORMAT(_eq.SupplierCost, 'N2') as CompanyCost
                                        ,(_cae.UnitPrice*_cae.Quantity) as CustomerCost
                                        --,iif(tk.ReferenceTicketId is not null and tk.ReferenceTicketId > 0, tk.ReferenceTicketId, '') as [Follow up From]
                                        --,iif(tk.RescheduleTicketId is not null and tk.RescheduleTicketId > 0, tk.RescheduleTicketId, '') as [Reschedule From]
                                        from CustomerAppointmentEquipment _cae
										left join ticket tk on _cae.AppointmentId = tk.TicketId
										left join Equipment _eq on _eq.EquipmentId = _cae.EquipmentId
										left join EquipmentType _et on _et.Id = _eq.EquipmentTypeId
										left join Manufacturer manu on manu.Id = _eq.ManufacturerId
										left join TicketUser tu on tu.TiketId = tk.TicketId
										left join Employee emp on emp.UserId = _cae.InstalledByUid
                                        left join Customer cus on cus.CustomerId = tk.CustomerId
                                        left join CustomerExtended ce on ce.CustomerId=cus.CustomerId
	                                    left join Lookup lktype on  lktype.DataKey ='TicketType'
                                        and lktype.DataValue = tk.TicketType
		                                where tk.CompanyId = @CompanyId and ce.IsTestAccount != 1
                                        and cus.Id is not null
                                        --and _eq.SKU != ' ' 
                                        and _eq.EquipmentClassId=1
                                        {5}
                                        {6}
                                        {7}
                                        {18}
                                        {19}
                                        {23}
                                        {24}
                                        {25}
                                        {13}
                                        order by TicketId desc
                                    ";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        TicketFilters.SearchText,//0
                                        TicketFilters.PageNo,  //1
                                        TicketFilters.PageSize, //2
                                        CompanyId, //3
                                        TicketFilters.CustomerId, //4
                                        searchQuery,//5
                                        ticketStatusQuery,//6
                                        ticketTypeQuery,//7
                                        assignedQuery,//8
                                        CreatedByMeQuery,//9
                                        myTicketQuery,//10
                                        subquery,//11
                                        subquery1,//12
                                        CreatedDateQuery,//13,
                                        ReportTypeQuery,//14,
                                        ReportQuery,//15,
                                        ReportColQuery,//16
                                        ReportCountQuery,//17
                                        CategoryQuery,//18
                                        ManuQuery,//19
                                        sdate,//20
                                        edate,//21
                                        NameSql,//22
                                       qatechnician,//23
                                       EquipmentStatus,//24
                                       EquipmentStatus2 //25
                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", TicketFilters.SearchText)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetAllTicketReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize)
        {
            string DateFilter2 = "";
            string DateFilter1 = "";
            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime())
            {

                FilterStartDate = FilterStartDate.SetZeroHour().ClientToUTCTime();
                FilterEndDate = FilterEndDate.SetMaxHour().ClientToUTCTime();

                DateFilter2 = string.Format("and [Time] between '{0}' and '{1}'"
                    , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
                DateFilter1 = string.Format("and StartDate between '{0}' and '{1}' or EndDate between '{0}' and '{1}'"
                    , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            string sqlQuery = @"
                                declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                 select * into #tic from (
                                select 
                                sc.*,
                                tk.Id as tkId,
                                cus.FirstName +' '+cus.LastName as CustomerName,
                                emp.FirstName +' '+emp.LastName as SalesPerson,
								(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                From Invoice where CustomerId=cus.CustomerId
                                and (Status='Open' or Status='Partial')) as BalanceDue
						
                                from Ticket tk
                                left join customer cus on cus.CustomerId = sc.CustomerId
                                left join employee emp on   emp.UserId =  sc.UserId
                                left join Ticket tk on tk.TicketId = sc.TicketId
                                  ) d	

                                select * into #SalesCommissionFilter
								from #SaleCom

								select top(@pagesize)
								* from #SalesCommissionFilter
								where Id not in(select top(@pagestart) Id from #SalesCommissionFilter #e {2})
                                {3}
								select count(*) CountTotal
                                from #SalesCommissionFilter
                                
								drop table #SaleCom
								drop table #SalesCommissionFilter";

            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/customername")
                {
                    subquery = "order by #e.CustomerName asc";
                    subquery1 = "order by CustomerName asc";
                }
                else if (order == "descending/customername")
                {
                    subquery = "order by #e.CustomerName desc";
                    subquery1 = "order by CustomerName desc";
                }
                else if (order == "ascending/ticketid")
                {
                    subquery = "order by #e.Id asc";
                    subquery1 = "order by Id asc";
                }
                else if (order == "descending/ticketid")
                {
                    subquery = "order by #e.Id desc";
                    subquery1 = "order by Id desc";
                }
                else if (order == "ascending/userassigned")
                {
                    subquery = "order by #e.UserAssigned asc";
                    subquery1 = "order by UserAssigned asc";
                }
                else if (order == "descending/userassigned")
                {
                    subquery = "order by #e.UserAssigned desc";
                    subquery1 = "order by UserAssigned desc";
                }
                else if (order == "ascending/completiondate")
                {
                    subquery = "order by #e.CompletionDate asc";
                    subquery1 = "order by CompletionDate asc";
                }
                else if (order == "descending/completiondate")
                {
                    subquery = "order by #e.CompletionDate desc";
                    subquery1 = "order by CompletionDate desc";
                }
            }
            else
            {
                subquery = "order by #e.CompletionDate desc";
                subquery1 = "order by CompletionDate desc";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, DateFilter2, order, subquery, subquery1, DateFilter1);
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

        public DataSet GetAllTechReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, bool IsPaid, Guid UserId, string SearchText, string TechPersonList)
        {
            string DateFilter2 = "";
            string DateFilter1 = "";
            string IsPaidQuery = "";
            string techPayrollFilter = "";
            string SearchQuery = "";
            string FilterQuery = "";

            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
                //NameSql = NameSql.Replace("cus.", "cu.");
            }

            if (!string.IsNullOrEmpty(SearchText))
            {
                SearchQuery = string.Format(" (TicketIdValue like @SearchText or  CustomerName like @SearchText or Technician like @SearchText or BalanceDue like @SearchText) and ");
            }

            if (!string.IsNullOrEmpty(TechPersonList) && TechPersonList != "'null'")
            {

                FilterQuery = string.Format(" UserId in ({0}) and ", TechPersonList);
            }
            if (UserId != new Guid())
            {
                techPayrollFilter = string.Format(" and Tc.UserId = '{0}'", UserId);
            }
            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime())
            {

                FilterStartDate = FilterStartDate.SetZeroHour();
                FilterEndDate = FilterEndDate.SetMaxHour();
                DateFilter1 = string.Format("and Tc.CreatedDate between '{0}' and '{1}'"
                    , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (IsPaid)
            {
                IsPaidQuery = "where Tc.IsPaid = 1";
            }
            else
            {
                IsPaidQuery = "where (Tc.IsPaid = 0 or Tc.IsPaid is null)";
            }
            string sqlQuery = @"
                                declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                 select * into #TechCom from (select
                                  Tc.*, 
                                  tk.Id as TicketIdValue,
								 {7} as CustomerName,
							     cus.Id as CustomerIdValue,
								 emp.FirstName +' '+emp.LastName as Technician,
								(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                From Invoice where CustomerId=cus.CustomerId
                                and (Status='Open' or Status='Partial')) as BalanceDue
 
                                from TechCommission Tc
                                left join Ticket tk on tk.TicketId = Tc.TicketId
                                left join customer cus on cus.CustomerId = tk.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = Tc.UserId
                                {5}{4}{6}  and tk.IsClosed = 1 and ce.IsTestAccount != 1                    
                                ) d	

                                select * into #TechComFilter
								from #TechCom

								select top(@pagesize)
								* into #TotalCount from #TechComFilter
								where {8} {9} Id not in(select top(@pagestart) Id from #TechComFilter #e {2})
                                {3}
								select count(*) CountTotal
                                from #TechComFilter
                                where {8} {9} Id > 0

								select * from #TotalCount
								select sum(Adjustment) as TotalAdjustment
								,sum(TotalCommission) as SumCommission
								,sum(TotalPoint) as SumPoint
								,sum(OriginalPoint) as SumCommissionablePoint
								,sum(BalanceDue) as SumUnpaid
								from #TotalCount

								drop table #TechCom
								drop table #TechComFilter
								drop table #TotalCount";

            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/basermr")
                {
                    subquery = "order by #e.BaseRMR asc";
                    subquery1 = "order by BaseRMR asc";
                }
                else if (order == "descending/basermr")
                {
                    subquery = "order by #e.BaseRMR desc";
                    subquery1 = "order by BaseRMR desc";
                }
                else if (order == "ascending/ticketid")
                {
                    subquery = "order by #e.Id asc";
                    subquery1 = "order by Id asc";
                }
                else if (order == "descending/ticketid")
                {
                    subquery = "order by #e.Id desc";
                    subquery1 = "order by Id desc";
                }
                else if (order == "ascending/addedrmr")
                {
                    subquery = "order by #e.AddedRMR asc";
                    subquery1 = "order by AddedRMR asc";
                }
                else if (order == "descending/addedrmr")
                {
                    subquery = "order by #e.AddedRMR desc";
                    subquery1 = "order by AddedRMR desc";
                }
                else if (order == "ascending/adjustment")
                {
                    subquery = "order by #e.Adjustment asc";
                    subquery1 = "order by Adjustment asc";
                }
                else if (order == "descending/adjustment")
                {
                    subquery = "order by #e.Adjustment desc";
                    subquery1 = "order by Adjustment desc";
                }
                else if (order == "ascending/comission")
                {
                    subquery = "order by #e.TotalCommission asc";
                    subquery1 = "order by TotalCommission asc";
                }
                else if (order == "descending/comission")
                {
                    subquery = "order by #e.TotalCommission desc";
                    subquery1 = "order by TotalCommission desc";
                }
                else if (order == "ascending/totalpoint")
                {
                    subquery = "order by #e.TotalPoint asc";
                    subquery1 = "order by TotalPoint asc";
                }
                else if (order == "descending/totalpoint")
                {
                    subquery = "order by #e.TotalPoint desc";
                    subquery1 = "order by TotalPoint desc";
                }
                else if (order == "ascending/commissionablepoints")
                {
                    subquery = "order by #e.OriginalPoint asc";
                    subquery1 = "order by OriginalPoint asc";
                }
                else if (order == "descending/commissionablepoints")
                {
                    subquery = "order by #e.OriginalPoint desc";
                    subquery1 = "order by OriginalPoint desc";
                }
                else if (order == "ascending/unpaidbalance")
                {
                    subquery = "order by #e.BalanceDue asc";
                    subquery1 = "order by BalanceDue asc";
                }
                else if (order == "descending/unpaidbalance")
                {
                    subquery = "order by #e.BalanceDue desc";
                    subquery1 = "order by BalanceDue desc";
                }
                else if (order == "ascending/batch")
                {
                    subquery = "order by #e.Batch asc";
                    subquery1 = "order by Batch asc";
                }
                else if (order == "descending/batch")
                {
                    subquery = "order by #e.Batch desc";
                    subquery1 = "order by Batch desc";
                }
            }
            else
            {
                subquery = "order by #e.TicketIdValue desc";
                subquery1 = "order by TicketIdValue desc";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, DateFilter2, order, subquery, subquery1, DateFilter1, IsPaidQuery, techPayrollFilter, NameSql, SearchQuery, FilterQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {

                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", SearchText)));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }

            }
            catch (Exception ee)
            {
                return null;
            }

        }

        public DataSet GetDownLoadAllTechReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, bool IsPaid, Guid UserId, string SearchText, string TechPersonList)
        {
            string DateFilter2 = "";
            string DateFilter1 = "";
            string IsPaidQuery = "";
            string techPayrollFilter = "";
            string SearchQuery = "";
            string FilterQuery = "";
            string Batch = "";

            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
                //NameSql = NameSql.Replace("cus.", "cu.");
            }

            if (!string.IsNullOrEmpty(SearchText))
            {
                SearchQuery = string.Format(" ([Ticket Id] like @SearchText or  [Customer Name] like @SearchText or [Technician] like @SearchText or [Unpaid Balance] like @SearchText) and ");
            }

            if (!string.IsNullOrEmpty(TechPersonList) && TechPersonList != "'null'")
            {

                FilterQuery = string.Format(" UserId in ({0}) and ", TechPersonList);
            }
            if (UserId != new Guid())
            {
                techPayrollFilter = string.Format(" and Tc.UserId = '{0}'", UserId);
            }
            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime())
            {

                FilterStartDate = FilterStartDate.SetZeroHour();
                FilterEndDate = FilterEndDate.SetMaxHour();
                DateFilter1 = string.Format("and Tc.CreatedDate between '{0}' and '{1}'"
                    , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (IsPaid)
            {
                IsPaidQuery = "where Tc.IsPaid = 1";
                Batch = ", [Batch]";
            }
            else
            {
                IsPaidQuery = "where (Tc.IsPaid = 0 or Tc.IsPaid is null)";
            }
            string sqlQuery = @"
                                declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                 select * into #TechCom from (select
                                  tk.Id as [Ticket Id],
								 {7} as [Customer Name],
                                 cus.CustomerNo as [CS ID],
							     cus.Id as CustomerIdValue,
								 emp.FirstName +' '+emp.LastName as [Technician],
                                 TRY_CONVERT(date,Tc.CompletionDate) as [Completion Date],
                                 cast(Tc.BaseRMR as decimal(12,2)) as [Base RMR],
                                 cast(Tc.BaseRMRCommission as decimal(12,2))  as [Base RMR Comm.],
                                 cast(Tc.AddedRMR as decimal(12,2))  as [Added RMR],
                                 cast(Tc.AddedRMRCommission as decimal(12,2))  as [Added RMR Comm.],
                                 cast(Tc.Adjustment as decimal(12,2))  as [Adjustment],
                                 cast(Tc.TotalCommission as decimal(12,2)) as [Commission],
                                 ISNULL(Tc.TotalPoint,0) as TotalPoint,
                                ISNULL(Tc.OriginalPoint,0) as CommissionablePoints,
                                 
								(select cast(ISNULL(SUM(BalanceDue),0)  as decimal(12,2)) [Unpaid Balance] 
                                From Invoice where CustomerId=cus.CustomerId
                                and (Status='Open' or Status='Partial')) as [Unpaid Balance],
                                Tc.Id,
                                Tc.UserId,Tc.Batch as [Batch]
 
                                from TechCommission Tc
                                left join Ticket tk on tk.TicketId = Tc.TicketId
                                left join customer cus on cus.CustomerId = tk.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = Tc.UserId
                                {5}{4}{6} and tk.IsClosed = 1  and ce.IsTestAccount != 1                      
                                ) d	

                                select * into #TechComFilter
								from #TechCom

								select 
								[Ticket Id],
								[Customer Name],
                                [CS ID],
								[Technician],
                                [Completion Date],
                                [Base RMR],
                                [Base RMR Comm.],
                                [Added RMR],
                                [Added RMR Comm.],
                                [Adjustment],
                                [Commission],
                                [TotalPoint],
								[CommissionablePoints],
                                [Unpaid Balance] {10}
                                from #TechComFilter
								where {8} {9} Id not in(select top(@pagestart) Id from #TechComFilter #e {2})
                                {3}
								select count(*) CountTotal
                                from #TechComFilter
                                where {8} {9} Id > 0
								drop table #TechCom
								drop table #TechComFilter";

            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/customername")
                {
                    subquery = "order by #e.[Customer Name] asc";
                    subquery1 = "order by [Customer Name] asc";
                }
                else if (order == "descending/customername")
                {
                    subquery = "order by #e.[Customer Name] desc";
                    subquery1 = "order by [Customer Name] desc";
                }
                else if (order == "ascending/ticketid")
                {
                    subquery = "order by #e.Id asc";
                    subquery1 = "order by Id asc";
                }
                else if (order == "descending/ticketid")
                {
                    subquery = "order by #e.Id desc";
                    subquery1 = "order by Id desc";
                }
                else if (order == "ascending/userassigned")
                {
                    subquery = "order by #e.UserAssigned asc";
                    subquery1 = "order by UserAssigned asc";
                }
                else if (order == "descending/userassigned")
                {
                    subquery = "order by #e.UserAssigned desc";
                    subquery1 = "order by UserAssigned desc";
                }
                else if (order == "ascending/completiondate")
                {
                    subquery = "order by #e.[Completion Date] asc";
                    subquery1 = "order by [Completion Date] asc";
                }
                else if (order == "descending/completiondate")
                {
                    subquery = "order by #e.[Completion Date] desc";
                    subquery1 = "order by [Completion Date] desc";
                }
            }
            else
            {
                subquery = "order by #e.[Ticket Id] desc";
                subquery1 = "order by [Ticket Id] desc";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, DateFilter2, order, subquery, subquery1, DateFilter1, IsPaidQuery, techPayrollFilter, NameSql, SearchQuery, FilterQuery, Batch);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", SearchText)));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }

            }
            catch (Exception ee)
            {
                return null;
            }

        }


        public DataSet GetSalesReportByCompanyId(Guid companyid, int pageno, int pagesize, DateTime? startdate, DateTime? enddate, string searchtext, string invostatus, string Order)
        {

            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }

            string sqlQuery = @"";
            string subquery = "";
            string statusquery = "";
            string orderquery = "";
            string orderquery1 = "";

            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                //subquery = string.Format("and cus.BusinessName like '%{0}%' or cus.DBA like '%{0}%' or cus.FirstName + ' ' + cus.LastName like '%{0}%' or cus.FirstName like '%{0}%' or cus.LastName like '%{0}%'", searchtext);
                subquery = "and ((cus.SearchText like @SearchText) OR (cus.DBA like @SearchText))";
            }
            #region Order
            if (!string.IsNullOrWhiteSpace(Order))
            {
                if (Order == "ascending/customername")
                {
                    orderquery = "order by #CustomerDataFilter.[DisplayName] asc";
                    orderquery1 = "order by [DisplayName] asc";
                }
                else if (Order == "descending/customername")
                {
                    orderquery = "order by #CustomerDataFilter.[DisplayName] desc";
                    orderquery1 = "order by [DisplayName] desc";
                }
                else if (Order == "ascending/totalsales")
                {
                    orderquery = "order by #CustomerDataFilter.TotalSales asc";
                    orderquery1 = "order by TotalSales asc";
                }
                else if (Order == "descending/totalsales")
                {
                    orderquery = "order by #CustomerDataFilter.TotalSales desc";
                    orderquery1 = "order by TotalSales desc";
                }
                else if (Order == "ascending/totaltax")
                {
                    orderquery = "order by #CustomerDataFilter.[TotalTax] asc";
                    orderquery1 = "order by [TotalTax] asc";
                }
                else if (Order == "descending/totaltax")
                {
                    orderquery = "order by #CustomerDataFilter.[TotalTax] desc";
                    orderquery1 = "order by [TotalTax] desc";
                }
                else if (Order == "ascending/salesaftertax")
                {
                    orderquery = "order by #CustomerDataFilter.[SalesAfterTax] asc";
                    orderquery1 = "order by [SalesAfterTax] asc";
                }
                else if (Order == "descending/salesaftertax")
                {
                    orderquery = "order by #CustomerDataFilter.[SalesAfterTax] desc";
                    orderquery1 = "order by [SalesAfterTax] desc";
                }
                else if (Order == "ascending/totalpaid")
                {
                    orderquery = "order by #CustomerDataFilter.[TotalPaid] asc";
                    orderquery1 = "order by [TotalPaid] asc";
                }
                else if (Order == "descending/totalpaid")
                {
                    orderquery = "order by #CustomerDataFilter.[TotalPaid] desc";
                    orderquery1 = "order by [TotalPaid] desc";
                }
                else if (Order == "ascending/totalunpaid")
                {
                    orderquery = "order by #CustomerDataFilter.[TotalUnpaid]  asc";
                    orderquery1 = "order by TotalUnpaid asc";
                }
                else if (Order == "descending/totalunpaid")
                {
                    orderquery = "order by #CustomerDataFilter.[TotalUnpaid]  desc";
                    orderquery1 = "order by TotalUnpaid desc";
                }


                else
                {
                    orderquery = "order by #CustomerDataFilter.[Id]  desc";
                    orderquery1 = "order by Id desc";
                }

            }
            else
            {
                orderquery = "order by #CustomerDataFilter.[Id] desc";
                orderquery1 = "order by Id desc";
            }
            #endregion
            //if (!string.IsNullOrWhiteSpace(invostatus) && invostatus != "'null'")
            //{
            //    statusquery = string.Format("and invo.[Status] in ({0})", invostatus);
            //}
            if (startdate.HasValue && enddate.HasValue && startdate.Value != new DateTime() && enddate.Value != new DateTime())
            {
                sqlQuery = @"DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int

                                SET @pageno = {1}
                                SET @pagesize = {2}
                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

                                select distinct cus.Id, cus.CustomerId, cus.FirstName, cus.LastName,{7} as [DisplayName],
                                ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0)
								+ ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Open') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0)
                                + ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Partial') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0)
                                as TotalSales,

                                ISNULL((select SUM(cps.Total) from CustomerPackageService cps where cps.CustomerId = cus.CustomerId), 0) as TotalRMR,

                                ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000' and inv.TaxType != 'Non-Tax'), 0)
								+ ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Open') and inv.CompanyId != '00000000-0000-0000-0000-000000000000' and inv.TaxType != 'Non-Tax'), 0)
                                + ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Partial') and inv.CompanyId != '00000000-0000-0000-0000-000000000000' and inv.TaxType != 'Non-Tax'), 0))
								* (select Value from globalsetting where SearchKey = 'Sales Tax') / 100) as TotalTax,

                                --ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) + ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) * (select Value from globalsetting where SearchKey = 'Sales Tax')) / 100)
								--+ ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Open') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) + ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Open') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) * (select Value from globalsetting where SearchKey = 'Sales Tax')) / 100)
                                --+ ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Partial') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) + ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Partial') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) * (select Value from globalsetting where SearchKey = 'Sales Tax')) / 100)
								--as SalesAfterTax,

                                ISNULL((select SUM(inv.TotalAmount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0)
								+ ISNULL((select SUM(inv.TotalAmount - inv.BalanceDue) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Partial' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0)
								as TotalPaid,

                                (ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) + ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) * (select Value from globalsetting where SearchKey = 'Sales Tax')) / 100)
								+ ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Open') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) + ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Open') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) * (select Value from globalsetting where SearchKey = 'Sales Tax')) / 100)
                                + ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Partial') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) + ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Partial') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) * (select Value from globalsetting where SearchKey = 'Sales Tax')) / 100))
                                - ((ISNULL((select SUM(inv.TotalAmount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0)
								+ ISNULL((select SUM(inv.TotalAmount - inv.BalanceDue) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Partial' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0))) as TotalUnpaid

                                into #CustomerData from Customer cus
                                left join CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                left join Invoice invo on invo.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}'
                                and cc.IsLead = 0
                                and ce.IsTestAccount != 1
                                and invo.InvoiceDate between '{3}' and '{4}'
                                {5}
                                {6}

                               	select Id,CustomerId,FirstName,LastName,DisplayName,TotalSales,TotalRMR,TotalTax,
							    (TotalSales+TotalTax) as [SalesAfterTax],
								TotalPaid,
								(TotalSales+TotalTax)-TotalPaid as [TotalUnpaid]
								into #CustomerDataFilter from #CustomerData

                                SELECT TOP(@pagesize) * FROM #CustomerDataFilter where Id NOT IN(Select TOP (@pagestart) #cus.Id from #CustomerData #cus )
                                -- order by Id desc
                                {8}

                                select COUNT(*) as TotalCount from #CustomerDataFilter

                                select SUM(TotalSales) as TotalSalesAmount,(sum(TotalSales)/COUNT(*)) as [AveSaleswoTax],sum(TotalTax) as SumTotalTax,sum(SalesAfterTax) as SumSalesAfterTax,(sum(SalesAfterTax)/Count(*)) as AveSalesWTax,sum(TotalPaid) as SumTotalPaid ,sum(TotalUnpaid) as SumTotalUnpaid from #CustomerDataFilter

                                drop table #CustomerData
                                drop table #CustomerDataFilter";
                sqlQuery = string.Format(sqlQuery, companyid, pageno, pagesize, startdate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), enddate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), subquery, statusquery, NameSql, orderquery);
            }
            else
            {
                sqlQuery = @"DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int

                                SET @pageno = {1}
                                SET @pagesize = {2}
                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

                                select distinct cus.Id, cus.CustomerId, cus.FirstName, cus.LastName,{5} as [DisplayName],
                                ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0)
								+ ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Open') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0)
                                + ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Partial') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0)
                                as TotalSales,

                                ISNULL((select SUM(cps.Total) from CustomerPackageService cps where cps.CustomerId = cus.CustomerId), 0) as TotalRMR,

                                ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000' and inv.TaxType != 'Non-Tax'), 0)
								+ ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Open') and inv.CompanyId != '00000000-0000-0000-0000-000000000000' and inv.TaxType != 'Non-Tax'), 0)
                                + ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Partial') and inv.CompanyId != '00000000-0000-0000-0000-000000000000' and inv.TaxType != 'Non-Tax'), 0))
								* (select Value from globalsetting where SearchKey = 'Sales Tax') / 100) as TotalTax,

                                --ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) + ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) * (select Value from globalsetting where SearchKey = 'Sales Tax')) / 100)
								--+ ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Open') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) + ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Open') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) * (select Value from globalsetting where SearchKey = 'Sales Tax')) / 100)
                               -- + ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Partial') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) + ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Partial') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) * (select Value from globalsetting where SearchKey = 'Sales Tax')) / 100)
								--as SalesAfterTax,

                                ISNULL((select SUM(inv.TotalAmount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0)
								+ ISNULL((select SUM(inv.TotalAmount - inv.BalanceDue) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Partial' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0)
								as TotalPaid,

                                (ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) + ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) * (select Value from globalsetting where SearchKey = 'Sales Tax')) / 100)
								+ ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Open') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) + ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Open') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) * (select Value from globalsetting where SearchKey = 'Sales Tax')) / 100)
                                + ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Partial') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) + ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Partial') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) * (select Value from globalsetting where SearchKey = 'Sales Tax')) / 100))
                                - ((ISNULL((select SUM(inv.TotalAmount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0)
								+ ISNULL((select SUM(inv.TotalAmount - inv.BalanceDue) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Partial' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0))) as TotalUnpaid

                                into #CustomerData from Customer cus
                                left join CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                left join Invoice invo on invo.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}'
                                and cc.IsLead = 0
                                and ce.IsTestAccount != 1
                                {3}
                                {4}

                                
							    select Id,CustomerId,FirstName,LastName,DisplayName,TotalSales,TotalRMR,TotalTax,
							    (TotalSales+TotalTax) as [SalesAfterTax],
								TotalPaid,
								(TotalSales+TotalTax)-TotalPaid as [TotalUnpaid]
								into #CustomerDataFilter from #CustomerData

                                SELECT TOP(@pagesize) * FROM #CustomerDataFilter where Id NOT IN(Select TOP (@pagestart) #cus.Id from #CustomerData #cus)
                                -- order by Id desc
                                {6}


                                select COUNT(*) as TotalCount from #CustomerDataFilter

                                select SUM(TotalSales) as TotalSalesAmount,(sum(TotalSales)/COUNT(*)) as [AveSaleswoTax],sum(TotalTax) as SumTotalTax,sum(SalesAfterTax) as SumSalesAfterTax,(sum(SalesAfterTax)/Count(*)) as AveSalesWTax,sum(TotalPaid) as SumTotalPaid ,sum(TotalUnpaid) as SumTotalUnpaid from #CustomerDataFilter

                                drop table #CustomerData
                                drop table #CustomerDataFilter";
                sqlQuery = string.Format(sqlQuery, companyid, pageno, pagesize, subquery, statusquery, NameSql, orderquery);
            }

            try
            {
                sqlQuery = string.Format(sqlQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", searchtext)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetInvoiceReportByCompanyId(Guid companyid, int pageno, int pagesize, DateTime? startdate, DateTime? enddate, string searchtext, string invostatus, string order, FilterReportModel filter)
        {
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }
            string sqlQuery = @"";
            string subquery = "";
            string statusquery = "";
            string datequery = "";
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                subquery = ("and cus.BusinessName like @SearchText or cus.DBA like @SearchText or cus.FirstName + ' ' + cus.LastName like @SearchText or invo.InvoiceId like @SearchText or invo.InvoiceDate like @SearchText  or invo.DueDate like @SearchText or invo.TotalAmount like @SearchText or invo.BalanceDue like @SearchText");

                //subquery = string.Format("and cus.BusinessName like '%{0}%' or cus.DBA like '%{0}%' or cus.FirstName + ' ' + cus.LastName like '%{0}%' or invo.InvoiceId like '%{0}%' or invo.InvoiceDate like '%{0}%'  or invo.DueDate like '%{0}%' or invo.TotalAmount like '%{0}%' or invo.BalanceDue like '%{0}%'", searchtext);
            }
            if (!string.IsNullOrWhiteSpace(invostatus) && invostatus != "'null'")
            {
                statusquery = string.Format("", invostatus);
            }
            if (!string.IsNullOrWhiteSpace(filter.convertmindate) && !string.IsNullOrWhiteSpace(filter.convertmaxdate) && filter.convertmindate != "undefined" && filter.convertmaxdate != "undefined")
            {
                var date = Convert.ToDateTime(filter.convertmaxdate);
                var datemin = Convert.ToDateTime(filter.convertmindate);
                datequery = string.Format("and invo.InvoiceDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.convertmindate) && filter.convertmindate != "undefined")
            {
                var date = Convert.ToDateTime(filter.convertmindate);
                datequery = string.Format("and invo.InvoiceDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.convertmaxdate) && filter.convertmaxdate != "undefined")
            {
                var date = Convert.ToDateTime(filter.convertmaxdate);
                datequery = string.Format("and invo.InvoiceDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            if (!string.IsNullOrWhiteSpace(filter.createmindate) && !string.IsNullOrWhiteSpace(filter.createmaxdate) && filter.createmaxdate != "undefined" && filter.createmindate != "undefined")
            {
                var date = Convert.ToDateTime(filter.createmaxdate);
                var datemin = Convert.ToDateTime(filter.createmindate);
                datequery = string.Format("and invo.DueDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.createmindate) && filter.createmindate != "undefined")
            {
                var date = Convert.ToDateTime(filter.createmindate);
                datequery = string.Format("and invo.DueDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.createmaxdate) && filter.createmaxdate != "undefined")
            {
                var date = Convert.ToDateTime(filter.createmaxdate);
                datequery = string.Format("and invo.DueDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            String ordersubquery = "";
            String ordersubquery1 = "";
            if (!string.IsNullOrWhiteSpace(order) && order != "undefined" && order != "null")
            {
                if (order == "ascending/invoicedate")
                {
                    ordersubquery = "order by #CustomerDataFilter.InvoiceDate asc";
                    ordersubquery1 = "order by InvoiceDate asc";
                }
                else if (order == "descending/invoicedate")
                {
                    ordersubquery = "order by #CustomerDataFilter.InvoiceDate desc";
                    ordersubquery1 = "order by InvoiceDate desc";
                }
                else if (order == "ascending/firstname")
                {
                    ordersubquery = "order by #CustomerDataFilter.DisplayName asc";
                    ordersubquery1 = "order by DisplayName asc";
                }
                else if (order == "descending/firstname")
                {
                    ordersubquery = "order by #CustomerDataFilter.DisplayName desc";
                    ordersubquery1 = "order by DisplayName desc";
                }

                else if (order == "ascending/invoiceid")
                {
                    ordersubquery = "order by #CustomerDataFilter.InvoiceId asc";
                    ordersubquery1 = "order by InvoiceId asc";
                }
                else if (order == "descending/invoiceid")
                {
                    ordersubquery = "order by #CustomerDataFilter.InvoiceId desc";
                    ordersubquery1 = "order by InvoiceId desc";
                }

                else if (order == "ascending/duedate")
                {
                    ordersubquery = "order by #CustomerDataFilter.DueDate asc";
                    ordersubquery1 = "order by DueDate asc";
                }
                else if (order == "descending/duedate")
                {
                    ordersubquery = "order by #CustomerDataFilter.DueDate desc";
                    ordersubquery1 = "order by DueDate desc";
                }

                else if (order == "ascending/totalamount")
                {
                    ordersubquery = "order by #CustomerDataFilter.TotalAmount asc";
                    ordersubquery1 = "order by TotalAmount asc";
                }
                else if (order == "descending/totalamount")
                {
                    ordersubquery = "order by #CustomerDataFilter.TotalAmount desc";
                    ordersubquery1 = "order by TotalAmount desc";
                }

                else if (order == "ascending/amount")
                {
                    ordersubquery = "order by #CustomerDataFilter.Amount asc";
                    ordersubquery1 = "order by Amount asc";
                }
                else if (order == "descending/amount")
                {
                    ordersubquery = "order by #CustomerDataFilter.Amount desc";
                    ordersubquery1 = "order by Amount desc";
                }

                else if (order == "ascending/tax")
                {
                    ordersubquery = "order by #CustomerDataFilter.Tax asc";
                    ordersubquery1 = "order by Tax asc";
                }
                else if (order == "descending/tax")
                {
                    ordersubquery = "order by #CustomerDataFilter.Tax desc";
                    ordersubquery1 = "order by Tax desc";
                }

                else if (order == "ascending/balancedue")
                {
                    ordersubquery = "order by #CustomerDataFilter.BalanceDue asc";
                    ordersubquery1 = "order by BalanceDue asc";
                }
                else if (order == "descending/balancedue")
                {
                    ordersubquery = "order by #CustomerDataFilter.BalanceDue desc";
                    ordersubquery1 = "order by BalanceDue desc";
                }
            }
            else
            {
                ordersubquery = "order by #CustomerDataFilter.Id desc";
                ordersubquery1 = "order by Id desc";
            }



            if (startdate.HasValue && enddate.HasValue && startdate.Value != new DateTime() && enddate.Value != new DateTime())
            {
                sqlQuery = @"DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int

                                SET @pageno = {1}
                                SET @pagesize = {2}
                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

                                 select distinct invo.InvoiceId,invo.InvoiceDate,invo.DueDate,invo.TotalAmount,invo.Id,invo.BalanceDue, invo.Amount,invo.Tax, 
								cus.CustomerId,{9} as DisplayName,
								ISNULL((invo.TotalAmount - invo.BalanceDue), 0) as PaidBalance,cus.Id as CustomerIntId

                                into #CustomerData from Customer cus
                                left join CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                left join Invoice invo on invo.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}' and invo.Status != 'Init' and invo.Status !='Cancelled' and  invo.Status !='Rolled Over'
                                and cc.IsLead = 0 and invo.IsEstimate = 0
                                and invo.InvoiceDate between '{3}' and '{4}'
                                and ce.IsTestAccount != 1
                                {5}
                                {6}
                                {10}

                                select * into #CustomerDataFilter from #CustomerData

                                SELECT TOP(@pagesize) * FROM #CustomerDataFilter where Id NOT IN(Select TOP (@pagestart) #Cus.Id from #CustomerData #Cus order by #Cus.Id desc)
                                {7}

                                select COUNT(*) as TotalCount from #CustomerDataFilter

                                select count(distinct CustomerIntId)as CustomerCount,count(InvoiceId)as InvoiceCount,sum(Amount)as SumTotalAmt, (SUM(TotalAmount)/count(InvoiceId))as AveInvAmnt,sum(Tax)as SumTotalTax,SUM(TotalAmount) as TotalSalesAmount,SUM(BalanceDue) as TotalDueAmount from #CustomerDataFilter


                                drop table #CustomerData
                                drop table #CustomerDataFilter";
                sqlQuery = string.Format(sqlQuery, companyid, pageno, pagesize, startdate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), enddate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), subquery, statusquery, ordersubquery, ordersubquery1, NameSql, datequery);
            }
            else
            {
                sqlQuery = @"DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int

                                SET @pageno = {1}
                                SET @pagesize = {2}
                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

                                select distinct invo.InvoiceId,invo.InvoiceDate,invo.DueDate,invo.TotalAmount,invo.Id,invo.BalanceDue, invo.Amount,invo.Tax, 
								cus.CustomerId,{7} as DisplayName,
								ISNULL((invo.TotalAmount - invo.BalanceDue), 0) as PaidBalance,cus.Id as CustomerIntId

                                into #CustomerData from Customer cus
                                left join CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                left join Invoice invo on invo.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}' and invo.Status != 'Init' and invo.Status !='Cancelled' and  invo.Status !='Rolled Over'
                                and cc.IsLead = 0 and invo.IsEstimate = 0
                                and ce.IsTestAccount != 1
                                {3}
                                {4}
                                {8}
                                select * into #CustomerDataFilter from #CustomerData

                                SELECT TOP(@pagesize) * FROM #CustomerDataFilter where Id NOT IN(Select TOP (@pagestart) #Cus.Id from #CustomerData #Cus order by #Cus.Id desc)
                                {5}

                                select COUNT(Id) as TotalCount from #CustomerDataFilter

                                select count(distinct CustomerIntId)as CustomerCount,count(distinct InvoiceId)as InvoiceCount,sum(Amount)as SumTotalAmt, (SUM(TotalAmount)/count(distinct InvoiceId))as AveInvAmnt,sum(Tax)as SumTotalTax,SUM(TotalAmount) as TotalSalesAmount,SUM(BalanceDue) as TotalDueAmount from #CustomerDataFilter

                                drop table #CustomerData
                                drop table #CustomerDataFilter";
                sqlQuery = string.Format(sqlQuery, companyid, pageno, pagesize, subquery, statusquery, ordersubquery, ordersubquery1, NameSql, datequery);
            }

            try
            {
                sqlQuery = string.Format(sqlQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", searchtext)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetEstimateReportByCompanyId(Guid companyid, int pageno, int pagesize, DateTime? startdate, DateTime? enddate, string searchtext, string invostatus, string order, FilterReportModel filter)
        {
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }
            string sqlQuery = @"";
            string subquery = "";
            string statusquery = "";
            string datequery = "";
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                subquery = string.Format("and cus.BusinessName like '%{0}%' or cus.DBA like '%{0}%' or cus.FirstName + ' ' + cus.LastName like '%{0}%' or invo.InvoiceId like '%{0}%' or invo.InvoiceDate like '%{0}%'  or invo.DueDate like '%{0}%' or invo.TotalAmount like '%{0}%' or invo.BalanceDue like '%{0}%'", searchtext);
            }
            if (!string.IsNullOrWhiteSpace(invostatus) && invostatus != "'null'" && invostatus != "null")
            {
                statusquery = string.Format("and invo.Status in ({0})", invostatus);
            }
            //if (!string.IsNullOrWhiteSpace(filter.convertmindate) && !string.IsNullOrWhiteSpace(filter.convertmaxdate) && filter.convertmaxdate!="undefined")
            //{
            //    var date = Convert.ToDateTime(filter.convertmaxdate);
            //    var datemin = Convert.ToDateTime(filter.convertmindate);
            //    datequery = string.Format("and invo.InvoiceDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            //}
            //else if (!string.IsNullOrWhiteSpace(filter.convertmindate) && filter.convertmindate!= "undefined")
            //{
            //    var date = Convert.ToDateTime(filter.convertmindate);
            //    datequery = string.Format("and invo.InvoiceDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            //}
            //else if (!string.IsNullOrWhiteSpace(filter.convertmaxdate) && filter.convertmaxdate!= "undefined")
            //{
            //    var date = Convert.ToDateTime(filter.convertmaxdate);
            //    datequery = string.Format("and invo.InvoiceDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            //}
            //if (!string.IsNullOrWhiteSpace(filter.createmindate) && !string.IsNullOrWhiteSpace(filter.createmaxdate))
            //{
            //    var date = Convert.ToDateTime(filter.createmaxdate);
            //    var datemin = Convert.ToDateTime(filter.createmindate);
            //    datequery = string.Format("and invo.DueDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            //}
            //else if (!string.IsNullOrWhiteSpace(filter.createmindate))
            //{
            //    var date = Convert.ToDateTime(filter.createmindate);
            //    datequery = string.Format("and invo.DueDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            //}
            //else if (!string.IsNullOrWhiteSpace(filter.createmaxdate))
            //{
            //    var date = Convert.ToDateTime(filter.createmaxdate);
            //    datequery = string.Format("and invo.DueDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            //}
            String ordersubquery = "";
            String ordersubquery1 = "";
            if (!string.IsNullOrWhiteSpace(order) && order != "undefined" && order != "null")
            {
                if (order == "ascending/invoicedate")
                {
                    ordersubquery = "order by #CustomerDataFilter.InvoiceDate asc";
                    ordersubquery1 = "order by InvoiceDate asc";
                }
                else if (order == "descending/invoicedate")
                {
                    ordersubquery = "order by #CustomerDataFilter.InvoiceDate desc";
                    ordersubquery1 = "order by InvoiceDate desc";
                }
                else if (order == "ascending/firstname")
                {
                    ordersubquery = "order by #CustomerDataFilter.DisplayName asc";
                    ordersubquery1 = "order by DisplayName asc";
                }
                else if (order == "descending/firstname")
                {
                    ordersubquery = "order by #CustomerDataFilter.DisplayName desc";
                    ordersubquery1 = "order by DisplayName desc";
                }

                else if (order == "ascending/invoiceid")
                {
                    ordersubquery = "order by #CustomerDataFilter.InvoiceId asc";
                    ordersubquery1 = "order by InvoiceId asc";
                }
                else if (order == "descending/invoiceid")
                {
                    ordersubquery = "order by #CustomerDataFilter.InvoiceId desc";
                    ordersubquery1 = "order by InvoiceId desc";
                }

                else if (order == "ascending/duedate")
                {
                    ordersubquery = "order by #CustomerDataFilter.DueDate asc";
                    ordersubquery1 = "order by DueDate asc";
                }
                else if (order == "descending/duedate")
                {
                    ordersubquery = "order by #CustomerDataFilter.DueDate desc";
                    ordersubquery1 = "order by DueDate desc";
                }

                else if (order == "ascending/totalamount")
                {
                    ordersubquery = "order by #CustomerDataFilter.TotalAmount asc";
                    ordersubquery1 = "order by TotalAmount asc";
                }
                else if (order == "descending/totalamount")
                {
                    ordersubquery = "order by #CustomerDataFilter.TotalAmount desc";
                    ordersubquery1 = "order by TotalAmount desc";
                }

                else if (order == "ascending/amount")
                {
                    ordersubquery = "order by #CustomerDataFilter.Amount asc";
                    ordersubquery1 = "order by Amount asc";
                }
                else if (order == "descending/amount")
                {
                    ordersubquery = "order by #CustomerDataFilter.Amount desc";
                    ordersubquery1 = "order by Amount desc";
                }

                else if (order == "ascending/tax")
                {
                    ordersubquery = "order by #CustomerDataFilter.Tax asc";
                    ordersubquery1 = "order by Tax asc";
                }
                else if (order == "descending/tax")
                {
                    ordersubquery = "order by #CustomerDataFilter.Tax desc";
                    ordersubquery1 = "order by Tax desc";
                }

                else if (order == "ascending/balancedue")
                {
                    ordersubquery = "order by #CustomerDataFilter.BalanceDue asc";
                    ordersubquery1 = "order by BalanceDue asc";
                }
                else if (order == "descending/balancedue")
                {
                    ordersubquery = "order by #CustomerDataFilter.BalanceDue desc";
                    ordersubquery1 = "order by BalanceDue desc";
                }
                else
                {
                    ordersubquery = "order by #CustomerDataFilter.CreatedDate desc";
                    ordersubquery1 = "order by CreatedDate desc";
                }
            }
            else
            {
                ordersubquery = "order by #CustomerDataFilter.CreatedDate desc";
                ordersubquery1 = "order by CreatedDate desc";
            }



            if (startdate.HasValue && enddate.HasValue && startdate.Value != new DateTime() && enddate.Value != new DateTime())
            {
                sqlQuery = @"DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int

                                SET @pageno = {1}
                                SET @pagesize = {2}
                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

                                 select distinct invo.InvoiceId,invo.InvoiceDate,invo.CreatedDate, invo.Status,invo.DueDate,invo.TotalAmount,invo.Id,invo.BalanceDue, invo.Amount,invo.Tax, 
								cus.CustomerId,{9} as DisplayName,
								ISNULL((invo.TotalAmount - invo.BalanceDue), 0) as PaidBalance,cus.Id as CustomerIntId

                                into #CustomerData from Customer cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                left join Invoice invo on invo.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}' and invo.Status != 'Init' and invo.Status !='Cancelled' and  invo.Status !='Rolled Over'
                                and invo.IsEstimate = 1
                                and invo.CreatedDate between '{3}' and '{4}'
                                {5}
                                {6}
                                {10}

                                select * into #CustomerDataFilter from #CustomerData

                                SELECT TOP(@pagesize) * FROM #CustomerDataFilter where Id NOT IN(Select TOP (@pagestart) #Cus.Id from #CustomerData #Cus order by #Cus.CreatedDate desc)
                                {7}

                                select COUNT(*) as TotalCount from #CustomerDataFilter

                                select SUM(TotalAmount) as TotalSalesAmount,SUM(BalanceDue) as TotalDueAmount from #CustomerDataFilter

                                drop table #CustomerData
                                drop table #CustomerDataFilter";
                sqlQuery = string.Format(sqlQuery, companyid, pageno, pagesize, startdate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), enddate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), subquery, statusquery, ordersubquery, ordersubquery1, NameSql, datequery);
            }
            else
            {
                sqlQuery = @"DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int

                                SET @pageno = {1}
                                SET @pagesize = {2}
                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

                                select distinct invo.InvoiceId,invo.InvoiceDate,invo.CreatedDate, invo.Status,invo.DueDate,invo.TotalAmount,invo.Id,invo.BalanceDue, invo.Amount,invo.Tax, 
								cus.CustomerId,{7} as DisplayName,
								ISNULL((invo.TotalAmount - invo.BalanceDue), 0) as PaidBalance,cus.Id as CustomerIntId

                                into #CustomerData from Customer cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                left join Invoice invo on invo.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}' and invo.Status != 'Init' and invo.Status !='Cancelled' and  invo.Status !='Rolled Over'
                                and invo.IsEstimate = 1
                                {3}
                                {4}
                                {8}
                                select * into #CustomerDataFilter from #CustomerData

                                SELECT TOP(@pagesize) * FROM #CustomerDataFilter where Id NOT IN(Select TOP (@pagestart) #Cus.Id from #CustomerData #Cus order by #Cus.CreatedDate desc)
                                {5}

                                select COUNT(Id) as TotalCount from #CustomerDataFilter

                                select SUM(TotalAmount) as TotalSalesAmount,SUM(BalanceDue) as TotalDueAmount from #CustomerDataFilter

                                drop table #CustomerData
                                drop table #CustomerDataFilter";
                sqlQuery = string.Format(sqlQuery, companyid, pageno, pagesize, subquery, statusquery, ordersubquery, ordersubquery1, NameSql, datequery);
            }

            try
            {
                sqlQuery = string.Format(sqlQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataSet GetBookingSalesReportByCompanyId(Guid companyid, int pageno, int pagesize, DateTime? startdate, DateTime? enddate, string searchtext, string source, string order)
        {
            string sqlQuery = @"";
            string subquery = "";
            string subquery1 = "";
            string setext = "";
            string statusquery = "";
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                setext = string.Format("and (Bking.BookingId like '%{0}%' or FirstName like '%{0}%' or Bking.Message like '%{0}%' or invo.PaymentType like '%{0}%' or invo.InvoiceId like '%{0}%')", searchtext);
            }
            string tempQuery = "";
            if (!string.IsNullOrWhiteSpace(source) && source != "'null'" && source != "null" && source != "'undefined'")
            {
                string[] TempSource = source.Split(',');
                if (source.Contains("Online") && TempSource.Length == 1)
                {
                    tempQuery += " and Bking.BookingSource = 'Online'";
                }
                else if (source.Contains("System") && TempSource.Length == 1)
                {
                    tempQuery += " and (Bking.BookingSource is null or Bking.BookingSource != 'Online')";
                }
            }
            //if (!string.IsNullOrWhiteSpace(invostatus) && invostatus != "'null'" && invostatus != "'undefined'")
            //{
            //    statusquery = string.Format("and invo.[Status] in ({0})", invostatus);
            //}

            sqlQuery = @"DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int

                                SET @pageno = {1}
                                SET @pagesize = {2}
                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

                                select distinct cus.Id,cus.Id as CustomerIntId, cus.CustomerId, cus.FirstName + ' ' + cus.LastName as FirstName,cus.PrimaryPhone,cus.SecondaryPhone,cus.EmailAddress,
								cus.Address,cus.Street,cus.City,cus.State,cus.ZipCode,
								Bking.Id as BookingIntId,Bking.BookingId,Bking.BookingSource,Bking.TotalAmount,Bking.Status,Bking.Message,
                                (STUFF((
										SELECT ', '  + BkingD.RugType + ' ' 
										+ CASE WHEN BkingD.Length is not null 
											THEN CONVERT(nvarchar(50),BkingD.Length) + '''' ELSE '' END  
										+ CASE WHEN BkingD.LengthInch is not null 
											THEN CONVERT(nvarchar(50),BkingD.LengthInch) + '@""' + ' by ' ELSE '' END 
                                        + CASE WHEN BkingD.Width is not null

                                            THEN CONVERT(nvarchar(50), BkingD.Width) +'''' ELSE '' END
                                        + CASE WHEN BkingD.WidthInch is not null

                                            THEN CONVERT(nvarchar(50), BkingD.WidthInch) +'@""' ELSE '' END
                                        + CASE WHEN BkingD.Radius is not null

                                            THEN CONVERT(nvarchar(50), BkingD.Radius)  +'''' ELSE '' END
                                        + CASE WHEN BkingD.RadiusInch is not null

                                            THEN CONVERT(nvarchar(50), BkingD.RadiusInch) +'@""' ELSE '' END
                                        FROM BookingDetails BkingD where BkingD.BookingId = Bking.BookingId

                                        FOR XML PATH('')
										), 1, 2, '')
									) AS RugType,
                                    (STUFF((
										SELECT ', ' + 'ID:'    + '{5}' +
                                        '- ' + TicT.TicketType + ' '
										FROM Ticket  TicT where TicT.BookingId = Bking.BookingId
										FOR XML PATH('')
										), 1, 2, '')
									) AS TicketType,
								invo.InvoiceId,invo.Id as InvoiceIntId,invo.BalanceDue,invo.PaymentType,invo.TotalAmount As InTotalAmount,
                                (invo.TotalAmount - invo.BalanceDue) as AmountPaid

                              
                                into #CustomerData from Booking Bking
								left join Customer cus on Bking.CustomerId = cus.CustomerId
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
								--left join BookingDetails BkingD on BkingD.BookingId = Bking.BookingId
								--left join Ticket TicT on TicT.BookingId = Bking.BookingId
                                left join Invoice invo on invo.BookingId = Bking.BookingId
                                where cc.CompanyId = '{0}'
                                and Bking.Status='Approved'
                                {8}
                                {7}
                                {4}

                                select * into #CustomerDataFilter from #CustomerData

                                SELECT TOP(@pagesize) * FROM #CustomerDataFilter where Id NOT IN(Select TOP (@pagestart) #cus.Id from #CustomerData #cus {3})
                                {6}

                                select COUNT(*) as TotalCount from #CustomerDataFilter

                                drop table #CustomerData
                                drop table #CustomerDataFilter";

            if (!string.IsNullOrWhiteSpace(order) && order != "undefined" && order != "null")
            {
                if (order == "ascending/bookingid")
                {
                    subquery = "order by #cus.BookingId asc";
                    subquery1 = "order by BookingId asc";
                }
                else if (order == "descending/bookingid")
                {
                    subquery = "order by #cus.BookingId desc";
                    subquery1 = "order by BookingId desc";
                }
                else if (order == "ascending/customerid")
                {
                    subquery = "order by #cus.FirstName asc";
                    subquery1 = "order by FirstName asc";
                }
                else if (order == "descending/customerid")
                {
                    subquery = "order by #cus.FirstName desc";
                    subquery1 = "order by FirstName desc";
                }
                else if (order == "ascending/invoiceid")
                {
                    subquery = "order by #cus.InvoiceIntId asc";
                    subquery1 = "order by InvoiceIntId asc";
                }
                else if (order == "descending/invoiceid")
                {
                    subquery = "order by #cus.InvoiceIntId desc";
                    subquery1 = "order by InvoiceIntId desc";
                }

                else if (order == "ascending/paymentmethod")
                {
                    subquery = "order by #cus.PaymentType asc";
                    subquery1 = "order by PaymentType asc";
                }
                else if (order == "descending/paymentmethod")
                {
                    subquery = "order by #cus.PaymentType desc";
                    subquery1 = "order by PaymentType desc";
                }

                else if (order == "ascending/rugtype")
                {
                    subquery = "order by #cus.RugType asc";
                    subquery1 = "order by RugType asc";
                }
                else if (order == "descending/rugtype")
                {
                    subquery = "order by #cus.RugType desc";
                    subquery1 = "order by RugType desc";
                }


                else if (order == "ascending/amountpaid")
                {
                    subquery = "order by #cus.AmountPaid asc";
                    subquery1 = "order by AmountPaid asc";
                }
                else if (order == "descending/amountpaid")
                {
                    subquery = "order by #cus.AmountPaid desc";
                    subquery1 = "order by AmountPaid desc";
                }


                else if (order == "ascending/balancedue")
                {
                    subquery = "order by #cus.BalanceDue asc";
                    subquery1 = "order by BalanceDue asc";
                }
                else if (order == "descending/balancedue")
                {
                    subquery = "order by #cus.BalanceDue desc";
                    subquery1 = "order by BalanceDue desc";
                }


                else if (order == "ascending/tickettype")
                {
                    subquery = "order by #cus.TicketType asc";
                    subquery1 = "order by TicketType asc";
                }
                else if (order == "descending/tickettype")
                {
                    subquery = "order by #cus.TicketType desc";
                    subquery1 = "order by TicketType desc";
                }

                else if (order == "ascending/message")
                {
                    subquery = "order by #cus.Message asc";
                    subquery1 = "order by Message asc";
                }
                else if (order == "descending/message")
                {
                    subquery = "order by #cus.Message desc";
                    subquery1 = "order by Message desc";
                }
            }
            else
            {
                subquery = "order by #cus.Id desc";
                subquery1 = "order by Id desc";
            }

            string dateFilter = "";
            if (startdate.HasValue && enddate.HasValue && startdate.Value != new DateTime() && enddate.Value != new DateTime())
            {
                dateFilter = string.Format(@"and Bking.CreatedDate >= '{0}' and Bking.CreatedDate <= '{1}'", startdate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), enddate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            string OpenTicketById = string.Format(@"<a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenTicketById('{0}')"">'{0}'</a>", " + CONVERT(nvarchar(50), TicT.Id) +");
            sqlQuery = string.Format(sqlQuery, companyid, pageno, pagesize, subquery, tempQuery, OpenTicketById, subquery1, setext, dateFilter);
            try
            {
                sqlQuery = string.Format(sqlQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetCollectionSalesReportByCompanyId(Guid companyid, int pageno, int pagesize, DateTime? startdate, DateTime? enddate, string searchtext, int salesCommission, string invostatus, string order, FilterReportModel filter)
        {
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }
            string sqlQuery = @"";
            string subquery = "";
            string subquery1 = "";
            string setext = "";
            string statusquery = "";
        
            string datequery = "";
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                setext = "and (cus.BusinessName like @SearchText or cus.DBA like @SearchText or cus.FirstName + ' ' + cus.LastName like @SearchText or invo.InvoiceId like @SearchText or invo.InvoiceDate like @SearchText  or invo.DueDate like @SearchText or invo.TotalAmount like @SearchText or invo.BalanceDue like @SearchText)";
            }
            if (!string.IsNullOrWhiteSpace(invostatus) && invostatus != "'null'" && invostatus != "'undefined'")
            {
                statusquery = string.Format("and invo.[Status] in ({0})", invostatus);
            }
            if (salesCommission > 0) 
            {
                statusquery = string.Format("and cus.SalesLocation = {0}", salesCommission);
            }
            if (!string.IsNullOrWhiteSpace(filter.convertmaxdate) && !string.IsNullOrWhiteSpace(filter.convertmindate))
            {
                var datemin = Convert.ToDateTime(filter.convertmindate);
                var date = Convert.ToDateTime(filter.convertmaxdate);
                datequery += string.Format("and invo.InvoiceDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.convertmindate))
            {
                var date = Convert.ToDateTime(filter.convertmindate);
                datequery += string.Format("and invo.InvoiceDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.convertmaxdate))
            {
                var date = Convert.ToDateTime(filter.convertmaxdate);
                datequery += string.Format("and invo.InvoiceDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            if (!string.IsNullOrWhiteSpace(filter.createmaxdate) && !string.IsNullOrWhiteSpace(filter.createmindate))
            {
                var datemin = Convert.ToDateTime(filter.createmindate);
                var date = Convert.ToDateTime(filter.createmaxdate);
                datequery += string.Format("and invo.DueDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.createmindate))
            {
                var date = Convert.ToDateTime(filter.createmindate);
                datequery += string.Format("and invo.DueDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.createmaxdate))
            {
                var date = Convert.ToDateTime(filter.createmaxdate);
                datequery += string.Format("and invo.DueDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }

            if (!string.IsNullOrWhiteSpace(filter.collectionmaxdate) && !string.IsNullOrWhiteSpace(filter.collectionmindate))
            {
                var datemin = Convert.ToDateTime(filter.collectionmindate);
                var date = Convert.ToDateTime(filter.collectionmaxdate);
                datequery += string.Format(" and tr.TransacationDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.collectionmindate))
            {
                var date = Convert.ToDateTime(filter.collectionmindate);
                datequery += string.Format(" and tr.TransacationDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.collectionmaxdate))
            {
                var date = Convert.ToDateTime(filter.collectionmaxdate);
                datequery += string.Format(" and tr.TransacationDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            sqlQuery = @"DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int

                                SET @pageno = {1}
                                SET @pagesize = {2}
                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

                                 select invo.InvoiceId,invo.InvoiceDate,invo.DueDate,invo.TotalAmount,invo.Id,invo.BalanceDue,lksalesloc.DisplayText as SalesLocationName, invo.Amount,invo.Tax,
								cus.CustomerId,{9} as FirstName,
								--ISNULL((invo.TotalAmount - invo.BalanceDue), 0) as PaidBalance,
                                ISNULL(th.Amout,0) as PaidBalance,
                                (STUFF((
										SELECT ', '  +  Transa.PaymentMethod + ' '
										FROM TransactionHistory  TranH
										left join [Transaction] Transa on TranH.TransactionId = Transa.Id
										where TranH.InvoiceId = invo.Id
										
										FOR XML PATH('')
										), 1, 2, '')
									) AS PaymentMethod,cus.Id as CustomerIntId,
                                tr.TransacationDate
                                into #CustomerData from Customer cus
                                left join CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                left join Invoice invo on invo.CustomerId = cus.CustomerId
                                left join Lookup lksalesloc on  lksalesloc.DataKey ='CommissionType'  
                                and lksalesloc.DataValue = iif(cus.SalesLocation != '-1', cus.SalesLocation, null)
                                LEFT JOIN [TransactionHistory] th on th.InVoiceId=invo.Id
								LEFT JOIN [Transaction] tr on tr.Id=th.TransactionId
                                where cc.CompanyId = '{0}' and (invo.Status ='Paid' or  invo.Status ='Partial')
                                and cc.IsLead = 0 and invo.IsEstimate = 0
                                and ce.IsTestAccount != 1
                       
                                {8}
                                {7}
                                {4}
                                {10}
                                select * into #CustomerDataFilter from #CustomerData

                                SELECT TOP(@pagesize) * into #TestTable FROM #CustomerDataFilter where Id NOT IN(Select TOP (@pagestart) #cus.Id from #CustomerData #cus {3})
                                {6}

                                select COUNT(Id) as TotalCount from #CustomerDataFilter
                                select count(distinct CustomerIntId) as CountCustomer,count(distinct InvoiceId) as CountInvoice,(sum(TotalAmount)/count(distinct InvoiceId)) as AveInvoiceAmt ,sum(Amount) as TotalAmount,sum(Tax) as TotalTax,SUM(TotalAmount) as TotalSalesAmount,sum(BalanceDue) as TotalOpenBalance,SUM(PaidBalance) as TotalDueAmount from #CustomerDataFilter

								select * from #TestTable
								select sum(TotalAmount) as TotalInvoicesAmount
								,sum(BalanceDue) as TotalOpenBalance
								,sum(Tax) as TotalTax
								,sum(Amount) as TotalAmount
								,sum(PaidBalance) as TotalCollected
								from #TestTable

                                drop table #CustomerData
                                drop table #CustomerDataFilter
								drop table #TestTable";

            if (!string.IsNullOrWhiteSpace(order) && order != "undefined" && order != "null")
            {
                if (order == "ascending/invoicedate")
                {
                    subquery = "order by #cus.InvoiceDate asc";
                    subquery1 = "order by InvoiceDate asc";
                }
                else if (order == "descending/invoicedate")
                {
                    subquery = "order by #cus.InvoiceDate desc";
                    subquery1 = "order by InvoiceDate desc";
                }
                else if (order == "ascending/firstname")
                {
                    subquery = "order by #cus.FirstName asc";
                    subquery1 = "order by FirstName asc";
                }
                else if (order == "descending/firstname")
                {
                    subquery = "order by #cus.FirstName desc";
                    subquery1 = "order by FirstName desc";
                }

                else if (order == "ascending/invoiceid")
                {
                    subquery = "order by #cus.InvoiceId asc";
                    subquery1 = "order by InvoiceId asc";
                }
                else if (order == "descending/invoiceid")
                {
                    subquery = "order by #cus.InvoiceId desc";
                    subquery1 = "order by InvoiceId desc";
                }

                else if (order == "ascending/duedate")
                {
                    subquery = "order by #cus.DueDate asc";
                    subquery1 = "order by DueDate asc";
                }
                else if (order == "descending/duedate")
                {
                    subquery = "order by #cus.DueDate desc";
                    subquery1 = "order by DueDate desc";
                }

                else if (order == "ascending/totalamount")
                {
                    subquery = "order by #cus.TotalAmount asc";
                    subquery1 = "order by TotalAmount asc";
                }
                else if (order == "descending/totalamount")
                {
                    subquery = "order by #cus.TotalAmount desc";
                    subquery1 = "order by TotalAmount desc";
                }

                else if (order == "ascending/amount")
                {
                    subquery = "order by #cus.Amount asc";
                    subquery1 = "order by Amount asc";
                }
                else if (order == "descending/amount")
                {
                    subquery = "order by #cus.Amount desc";
                    subquery1 = "order by Amount desc";
                }

                else if (order == "ascending/tax")
                {
                    subquery = "order by #cus.Tax asc";
                    subquery1 = "order by Tax asc";
                }
                else if (order == "descending/tax")
                {
                    subquery = "order by #cus.Tax desc";
                    subquery1 = "order by Tax desc";
                }
                else if (order == "ascending/balancedue")
                {
                    subquery = "order by #cus.BalanceDue asc";
                    subquery1 = "order by BalanceDue asc";
                }
                else if (order == "descending/balancedue")
                {
                    subquery = "order by #cus.BalanceDue desc";
                    subquery1 = "order by BalanceDue desc";
                }

                else if (order == "ascending/paymentmethod")
                {
                    subquery = "order by #cus.PaymentMethod asc";
                    subquery1 = "order by PaymentMethod asc";
                }
                else if (order == "descending/paymentmethod")
                {
                    subquery = "order by #cus.PaymentMethod desc";
                    subquery1 = "order by PaymentMethod desc";
                }

                else if (order == "ascending/paidbalance")
                {
                    subquery = "order by #cus.PaidBalance asc";
                    subquery1 = "order by PaidBalance asc";
                }
                else if (order == "descending/paidbalance")
                {
                    subquery = "order by #cus.PaidBalance desc";
                    subquery1 = "order by PaidBalance desc";
                }
                else if (order == "ascending/collectiondate")
                {
                    subquery = "order by #cus.TransacationDate asc";
                    subquery1 = "order by TransacationDate asc";
                }
                else if (order == "descending/collectiondate")
                {
                    subquery = "order by #cus.TransacationDate desc";
                    subquery1 = "order by TransacationDate desc";
                }
            }
            else
            {
                subquery = "order by #cus.Id desc";
                subquery1 = "order by Id desc";
            }

            string dateFilter = "";
            if (startdate.HasValue && enddate.HasValue && startdate.Value != new DateTime() && enddate.Value != new DateTime())
            {
                dateFilter = string.Format(@"and invo.InvoiceDate >= '{0}' and invo.InvoiceDate <= '{1}'", startdate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), enddate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            string OpenTicketById = string.Format(@"<a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenTicketById('{0}')"">'{0}'</a>", " + CONVERT(nvarchar(50), TicT.Id) +");
            sqlQuery = string.Format(sqlQuery, companyid, pageno, pagesize, subquery, statusquery, OpenTicketById, subquery1, setext, dateFilter, NameSql, datequery);
            try
            {
                sqlQuery = string.Format(sqlQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", searchtext)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public DataSet GetPartnerReportByCompanyId(Guid companyid, int pageno, int pagesize, DateTime? startdate, DateTime? enddate, string searchtext, string order, string eIdList)
        {
            string sqlQuery = @"";
            string subquery = "";
            string subquery1 = "";
            string setext = "";

            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                setext = string.Format("where (CreatedByEmpName like '%{0}%' or FirstName like '%{0}%' or Street like '%{0}%' or Status like '%{0}%' or CreatedDate like '%{0}%' or CustomerTotalRevenue like '%{0}%')", searchtext);
            }


            sqlQuery = @"DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int

                                SET @pageno = {1}
                                SET @pagesize = {2}
                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

                                select distinct cus.Id, cus.FirstName + ' ' + cus.LastName as FirstName,cus.CustomerId,cus.PrimaryPhone,cus.SecondaryPhone,cus.EmailAddress,
								cus.Address,cus.Street,cus.City,cus.State,cus.ZipCode,cus.Status,cc.IsLead, cus.CreatedDate,cus.CreatedByUid,
                                (select SUM(Trans.Amount) from [Transaction] Trans where Trans.CustomerId = cus.CustomerId) as CustomerTotalRevenue,
								(select emp.FirstName + ' ' + emp.LastName  from Employee emp where emp.UserId = cus.CreatedByUid) as CreatedByEmpName
                                into #CustomerData from Customer cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}'
                                and cus.Soldby in ({7})
                                
                                {6}
                                
                                

                                select * into #CustomerDataFilter from #CustomerData {5}

                                SELECT TOP(@pagesize) * FROM #CustomerDataFilter where Id NOT IN(Select TOP (@pagestart) #cus.Id from #CustomerData #cus {3})
                                {4}

                                select COUNT(Id) as TotalCount from #CustomerDataFilter

                                drop table #CustomerData
                                drop table #CustomerDataFilter";

            if (!string.IsNullOrWhiteSpace(order) && order != "undefined" && order != "null")
            {
                if (order == "ascending/islead")
                {
                    subquery = "order by #cus.IsLead asc";
                    subquery1 = "order by IsLead asc";
                }
                else if (order == "descending/islead")
                {
                    subquery = "order by #cus.IsLead desc";
                    subquery1 = "order by IsLead desc";
                }


                else if (order == "ascending/firstname")
                {
                    subquery = "order by #cus.FirstName asc";
                    subquery1 = "order by FirstName asc";
                }
                else if (order == "descending/firstname")
                {
                    subquery = "order by #cus.FirstName desc";
                    subquery1 = "order by FirstName desc";
                }


                else if (order == "ascending/street")
                {
                    subquery = "order by #cus.Street asc";
                    subquery1 = "order by Street asc";
                }
                else if (order == "descending/street")
                {
                    subquery = "order by #cus.Street desc";
                    subquery1 = "order by Street desc";
                }

                else if (order == "ascending/createddate")
                {
                    subquery = "order by #cus.CreatedDate asc";
                    subquery1 = "order by CreatedDate asc";
                }
                else if (order == "descending/createddate")
                {
                    subquery = "order by #cus.CreatedDate desc";
                    subquery1 = "order by CreatedDate desc";
                }

                else if (order == "ascending/createdbyempname")
                {
                    subquery = "order by #cus.CreatedByEmpName asc";
                    subquery1 = "order by CreatedByEmpName asc";
                }
                else if (order == "descending/createdbyempname")
                {
                    subquery = "order by #cus.CreatedByEmpName desc";
                    subquery1 = "order by CreatedByEmpName desc";
                }


                else if (order == "ascending/status")
                {
                    subquery = "order by #cus.Status asc";
                    subquery1 = "order by Status asc";
                }
                else if (order == "descending/status")
                {
                    subquery = "order by #cus.Status desc";
                    subquery1 = "order by Status desc";
                }


                else if (order == "ascending/customertotalrevenue")
                {
                    subquery = "order by #cus.CustomerTotalRevenue asc";
                    subquery1 = "order by CustomerTotalRevenue asc";
                }
                else if (order == "descending/customertotalrevenue")
                {
                    subquery = "order by #cus.CustomerTotalRevenue desc";
                    subquery1 = "order by CustomerTotalRevenue desc";
                }




            }
            else
            {
                subquery = "order by #cus.Id desc";
                subquery1 = "order by Id desc";
            }

            string dateFilter = "";
            if (startdate.HasValue && enddate.HasValue && startdate.Value != new DateTime() && enddate.Value != new DateTime())
            {
                dateFilter = string.Format(@"and cus.CreatedDate >= '{0}' and cus.CreatedDate <= '{1}'", startdate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), enddate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }

            sqlQuery = string.Format(sqlQuery, companyid, pageno, pagesize, subquery, subquery1, setext, dateFilter, eIdList);
            try
            {
                sqlQuery = string.Format(sqlQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetPartnerReportBarByCompanyId(Guid companyid, DateTime? startdate, DateTime? enddate, string eIdList)
        {

            string sqlQuery = @"select distinct (
                                select  COUNT(cus.Id)
                                 from Customer cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}'
                                and cus.Soldby in ({2})
                                and cc.IsLead = 0 {1}) as Customer,

								(select  COUNT(cus.Id)
                                 from Customer cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}'
                                and cus.Soldby in ({2})
                                and cc.IsLead != 0  {1}) as Lead,
								( select  SUM(Trans.Amount) 
                                 from Customer cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
								left join [Transaction] Trans on Trans.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}'
                                and cus.Soldby in ({2}) {1} ) as TotalRevanew,

								(select  SUM(TRY_PARSE(cus.MonthlyMonitoringFee as int))
                                 from Customer cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}'
                                and cus.Soldby in ({2}) {1} ) as MonthlyMonitoringFee,

                                (select  COUNT(cus.Id)
                                 from Customer cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}'
                                and cus.Soldby in ({2})
                                and (cus.InstalledStatus ='Scheduled' or cus.InstalledStatus ='Rescheduled') {1}) as Scheduled,
                                
                                (select  COUNT(cus.Id)
                                 from Customer cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}'
                                and cus.Soldby in ({2})
                                and (cus.InstalledStatus ='Installed') {1}) as Installed
                                    ";

            string dateFilter = "";
            if (startdate.HasValue && enddate.HasValue && startdate.Value != new DateTime() && enddate.Value != new DateTime())
            {
                dateFilter = string.Format(@"and cus.CreatedDate >= '{0}' and cus.CreatedDate <= '{1}'", startdate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), enddate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }

            sqlQuery = string.Format(sqlQuery, companyid, dateFilter, eIdList);
            try
            {
                sqlQuery = string.Format(sqlQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetLeadSourceReportBarByCompanyId(Guid companyid, DateTime? startdate, DateTime? enddate, string eIdList)
        {

            string sqlQuery = @"select distinct (
                                select  COUNT(cus.Id)
                                 from Customer cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}'
                                and cus.LeadSource in ({2})
                                and cc.IsLead = 0 {1}) as Customer,

								(select  COUNT(cus.Id)
                                 from Customer cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}'
                                and cus.LeadSource in ({2})
                                and cc.IsLead != 0  {1}) as Lead,
								( select  SUM(Trans.Amount) 
                                 from Customer cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
								left join [Transaction] Trans on Trans.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}'
                                and cus.LeadSource in ({2}) {1} ) as TotalRevanew,

								(select  SUM(TRY_PARSE(cus.MonthlyMonitoringFee as int))
                                 from Customer cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}'
                                and cus.LeadSource in ({2}) {1} ) as MonthlyMonitoringFee,

                                (select  COUNT(cus.Id)
                                 from Customer cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}'
                                and cus.LeadSource in ({2})
                                and (cus.InstalledStatus ='Scheduled' or cus.InstalledStatus ='Rescheduled') {1}) as Scheduled,
                                
                                (select  COUNT(cus.Id)
                                 from Customer cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}'
                                and cus.LeadSource in ({2})
                                and (cus.InstalledStatus ='Installed') {1}) as Installed
                                    ";

            string dateFilter = "";
            if (startdate.HasValue && enddate.HasValue && startdate.Value != new DateTime() && enddate.Value != new DateTime())
            {
                dateFilter = string.Format(@"and cus.CreatedDate >= '{0}' and cus.CreatedDate <= '{1}'", startdate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), enddate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }

            sqlQuery = string.Format(sqlQuery, companyid, dateFilter, eIdList);
            try
            {
                sqlQuery = string.Format(sqlQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataSet GetLeadSourceReportByCompanyId(Guid companyid, int pageno, int pagesize, DateTime? startdate, DateTime? enddate, string searchtext, string order, string eIdList)
        {
            string sqlQuery = @"";
            string subquery = "";
            string subquery1 = "";
            string setext = "";

            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                setext = string.Format("where (CreatedByEmpName like @SearchText or FirstName like @SearchText or Street like @SearchText or Status like @SearchText or CreatedDate like @SearchText or CustomerTotalRevenue like @SearchText)");
            }


            sqlQuery = @"DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int

                                SET @pageno = {1}
                                SET @pagesize = {2}
                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

                                select distinct cus.Id, cus.FirstName + ' ' + cus.LastName as FirstName,cus.CustomerId,cus.PrimaryPhone,cus.SecondaryPhone,cus.EmailAddress,
								cus.Address,cus.Street,cus.City,cus.State,cus.ZipCode,cus.Status,cc.IsLead, cus.CreatedDate,cus.CreatedByUid,
                                (select SUM(Trans.Amount) from [Transaction] Trans where Trans.CustomerId = cus.CustomerId) as CustomerTotalRevenue,
								(select emp.FirstName + ' ' + emp.LastName  from Employee emp where emp.UserId = cus.CreatedByUid) as CreatedByEmpName
                                into #CustomerData from Customer cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}'
                                and cus.LeadSource in ({7})
                                
                                {6}
                                
                                

                                select * into #CustomerDataFilter from #CustomerData {5}

                                SELECT TOP(@pagesize) * FROM #CustomerDataFilter where Id NOT IN(Select TOP (@pagestart) #cus.Id from #CustomerData #cus {3})
                                {4}

                                select COUNT(Id) as TotalCount from #CustomerDataFilter

                                drop table #CustomerData
                                drop table #CustomerDataFilter";

            if (!string.IsNullOrWhiteSpace(order) && order != "undefined" && order != "null")
            {
                if (order == "ascending/islead")
                {
                    subquery = "order by #cus.IsLead asc";
                    subquery1 = "order by IsLead asc";
                }
                else if (order == "descending/islead")
                {
                    subquery = "order by #cus.IsLead desc";
                    subquery1 = "order by IsLead desc";
                }


                else if (order == "ascending/firstname")
                {
                    subquery = "order by #cus.FirstName asc";
                    subquery1 = "order by FirstName asc";
                }
                else if (order == "descending/firstname")
                {
                    subquery = "order by #cus.FirstName desc";
                    subquery1 = "order by FirstName desc";
                }


                else if (order == "ascending/street")
                {
                    subquery = "order by #cus.Street asc";
                    subquery1 = "order by Street asc";
                }
                else if (order == "descending/street")
                {
                    subquery = "order by #cus.Street desc";
                    subquery1 = "order by Street desc";
                }

                else if (order == "ascending/createddate")
                {
                    subquery = "order by #cus.CreatedDate asc";
                    subquery1 = "order by CreatedDate asc";
                }
                else if (order == "descending/createddate")
                {
                    subquery = "order by #cus.CreatedDate desc";
                    subquery1 = "order by CreatedDate desc";
                }

                else if (order == "ascending/createdbyempname")
                {
                    subquery = "order by #cus.CreatedByEmpName asc";
                    subquery1 = "order by CreatedByEmpName asc";
                }
                else if (order == "descending/createdbyempname")
                {
                    subquery = "order by #cus.CreatedByEmpName desc";
                    subquery1 = "order by CreatedByEmpName desc";
                }


                else if (order == "ascending/status")
                {
                    subquery = "order by #cus.Status asc";
                    subquery1 = "order by Status asc";
                }
                else if (order == "descending/status")
                {
                    subquery = "order by #cus.Status desc";
                    subquery1 = "order by Status desc";
                }


                else if (order == "ascending/customertotalrevenue")
                {
                    subquery = "order by #cus.CustomerTotalRevenue asc";
                    subquery1 = "order by CustomerTotalRevenue asc";
                }
                else if (order == "descending/customertotalrevenue")
                {
                    subquery = "order by #cus.CustomerTotalRevenue desc";
                    subquery1 = "order by CustomerTotalRevenue desc";
                }




            }
            else
            {
                subquery = "order by #cus.Id desc";
                subquery1 = "order by Id desc";
            }

            string dateFilter = "";
            if (startdate.HasValue && enddate.HasValue && startdate.Value != new DateTime() && enddate.Value != new DateTime())
            {
                dateFilter = string.Format(@"and cus.CreatedDate >= '{0}' and cus.CreatedDate <= '{1}'", startdate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), enddate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }

            sqlQuery = string.Format(sqlQuery, companyid, pageno, pagesize, subquery, subquery1, setext, dateFilter, eIdList);
            try
            {
                sqlQuery = string.Format(sqlQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", searchtext)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public DataTable GetSalesReportExportByCompanyId(Guid companyid, DateTime? startdate, DateTime? enddate, string searchtext, string invostatus)
        {
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }
            string sqlQuery = @"";
            string subquery = "";
            string statusquery = "";
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                //subquery = string.Format("and cus.BusinessName like '%{0}%' or cus.DBA like '%{0}%' or cus.FirstName + ' ' + cus.LastName like '%{0}%' or cus.FirstName like '%{0}%' or cus.LastName like '%{0}%'", searchtext);
                subquery = "and ((cus.SearchText like @SearchText) OR (cus.DBA like @SearchText))";
            }
            //if (!string.IsNullOrWhiteSpace(invostatus) && invostatus != "'null'")
            //{
            //    statusquery = string.Format("and invo.[Status] in ({0})", invostatus);
            //}
            if (startdate.HasValue && enddate.HasValue && startdate.Value != new DateTime() && enddate.Value != new DateTime())
            {
                sqlQuery = @"declare @SalesTotal nvarchar(MAX)
                              declare @RMRTotal nvarchar(MAX)

                             declare @TaxTotal nvarchar(MAX)
                             declare @TaxSalesTotal nvarchar(MAX)
                             declare @PaidTotal nvarchar(MAX)
                             declare @UnpaidTotal nvarchar(MAX)
                                select distinct cus.Id,{5} as [Customers],
                                ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0)
								+ ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Open') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0)
                                + ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Partial') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0)
                                as [Sales],
								ISNULL((select SUM(cps.Total) from CustomerPackageService cps where cps.CustomerId = cus.CustomerId), 0) as [Total RMR],


                                ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0)
								+ ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Open') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0)
                                + ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Partial') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0))
								* (select Value from globalsetting where SearchKey = 'Sales Tax') / 100) as [Tax],

                                ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) + ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) * (select Value from globalsetting where SearchKey = 'Sales Tax')) / 100)
								+ ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Open') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) + ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Open') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) * (select Value from globalsetting where SearchKey = 'Sales Tax')) / 100)
                                + ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Partial') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) + ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Partial') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) * (select Value from globalsetting where SearchKey = 'Sales Tax')) / 100)
								as [Total],

                                ISNULL((select SUM(inv.TotalAmount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0)
								+ ISNULL((select SUM(inv.TotalAmount - inv.BalanceDue) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Partial' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) as [Paid],

                                (ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) + ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) * (select Value from globalsetting where SearchKey = 'Sales Tax')) / 100)
								+ ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Open') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) + ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Open') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) * (select Value from globalsetting where SearchKey = 'Sales Tax')) / 100)
                                + ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Partial') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) + ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Partial') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) * (select Value from globalsetting where SearchKey = 'Sales Tax')) / 100))
                                - ((ISNULL((select SUM(inv.TotalAmount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0)
								+ ISNULL((select SUM(inv.TotalAmount - inv.BalanceDue) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Partial' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0))) as [Unpaid]

                                into #CustomerData from Customer cus
                                left join CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                left join Invoice invo on invo.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}'
                                and cc.IsLead = 0
                                and ce.IsTestAccount != 1
                                and invo.InvoiceDate between '{1}' and '{2}'
                                {3}
                                {4}

                                select * into #CustomerDataFilter from #CustomerData

                                set @SalesTotal = ISNULL(Convert(DECIMAL(18,2),(Select SUM([Sales])from #CustomerDataFilter)),0)
								set @RMRTotal = ISNULL(Convert(DECIMAL(18,2),(Select SUM([Total RMR])from #CustomerDataFilter)),0)

								set @TaxTotal = ISNULL(Convert(DECIMAL(18,2),(Select SUM([Tax])from #CustomerDataFilter)),0)
								set @TaxSalesTotal = ISNULL(Convert(DECIMAL(18,2),(Select SUM([Total])from #CustomerDataFilter)),0)
								set @PaidTotal = ISNULL(Convert(DECIMAL(18,2),(Select SUM([Paid])from #CustomerDataFilter)),0)
								set @UnpaidTotal = ISNULL(Convert(DECIMAL(18,2),(Select SUM([Unpaid])from #CustomerDataFilter)),0)

								insert into #CustomerDataFilter (Id, [Customers], [Sales] ,[Total RMR],[Tax], [Total], [Paid], [Unpaid]) 
								values(0, 'Total', @SalesTotal,@RMRTotal, @TaxTotal, @TaxSalesTotal, @PaidTotal, @UnpaidTotal)

                                SELECT * FROM #CustomerDataFilter
                                order by Id desc

                                drop table #CustomerData
                                drop table #CustomerDataFilter";
                sqlQuery = string.Format(sqlQuery, companyid, startdate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), enddate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), subquery, statusquery, NameSql);
            }
            else
            {
                sqlQuery = @" declare @SalesTotal nvarchar(MAX)
                              declare @RMRTotal nvarchar(MAX)

                              declare @TaxTotal nvarchar(MAX)
                              declare @TaxSalesTotal nvarchar(MAX)
                              declare @PaidTotal nvarchar(MAX)
                              declare @UnpaidTotal nvarchar(MAX)
                                select distinct cus.Id,{3} as [Customers],
                                ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0)
								+ ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Open') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0)
                                + ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Partial') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0)
                                as [Sales],

								ISNULL((select SUM(cps.Total) from CustomerPackageService cps where cps.CustomerId = cus.CustomerId), 0) as [Total RMR],

                                ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0)
								+ ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Open') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0)
                                + ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Partial') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0))
								* (select Value from globalsetting where SearchKey = 'Sales Tax') / 100) as [Tax],

                                ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) + ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) * (select Value from globalsetting where SearchKey = 'Sales Tax')) / 100)
								+ ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Open') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) + ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Open') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) * (select Value from globalsetting where SearchKey = 'Sales Tax')) / 100)
                                + ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Partial') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) + ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Partial') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) * (select Value from globalsetting where SearchKey = 'Sales Tax')) / 100)
								as [Total],

                                ISNULL((select SUM(inv.TotalAmount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0)
								+ ISNULL((select SUM(inv.TotalAmount - inv.BalanceDue) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Partial' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) as [Paid],

                                (ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) + ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) * (select Value from globalsetting where SearchKey = 'Sales Tax')) / 100)
								+ ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Open') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) + ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Open') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) * (select Value from globalsetting where SearchKey = 'Sales Tax')) / 100)
                                + ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Partial') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) + ((ISNULL((select SUM(inv.Amount) from Invoice inv where inv.CustomerId = cus.CustomerId and (inv.[Status] = 'Partial') and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0) * (select Value from globalsetting where SearchKey = 'Sales Tax')) / 100))
                                - ((ISNULL((select SUM(inv.TotalAmount) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Paid' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0)
								+ ISNULL((select SUM(inv.TotalAmount - inv.BalanceDue) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] = 'Partial' and inv.CompanyId != '00000000-0000-0000-0000-000000000000'), 0))) as [Unpaid]

                                into #CustomerData from Customer cus
                                left join CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                left join Invoice invo on invo.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}'
                                and cc.IsLead = 0
                                and ce.IsTestAccount != 1
                                {1}
                                {2}

                                select * into #CustomerDataFilter from #CustomerData

                                set @SalesTotal = ISNULL(Convert(DECIMAL(18,2),(Select SUM([Sales])from #CustomerDataFilter)),0)
								 set @RMRTotal = ISNULL(Convert(DECIMAL(18,2),(Select SUM([Total RMR])from #CustomerDataFilter)),0)

								set @TaxTotal = ISNULL(Convert(DECIMAL(18,2),(Select SUM([Tax])from #CustomerDataFilter)),0)
								set @TaxSalesTotal = ISNULL(Convert(DECIMAL(18,2),(Select SUM([Total])from #CustomerDataFilter)),0)
								set @PaidTotal = ISNULL(Convert(DECIMAL(18,2),(Select SUM([Paid])from #CustomerDataFilter)),0)
								set @UnpaidTotal = ISNULL(Convert(DECIMAL(18,2),(Select SUM([Unpaid])from #CustomerDataFilter)),0)
								 
								insert into #CustomerDataFilter (Id, [Customers], [Sales],[Total RMR], [Tax], [Total], [Paid], [Unpaid]) 
								values(0, 'Total', @SalesTotal,@RMRTotal, @TaxTotal, @TaxSalesTotal, @PaidTotal, @UnpaidTotal)

                                SELECT * FROM #CustomerDataFilter
                                order by Id desc

                                drop table #CustomerData
                                drop table #CustomerDataFilter";
                sqlQuery = string.Format(sqlQuery, companyid, subquery, statusquery, NameSql);
            }

            try
            {
                sqlQuery = string.Format(sqlQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", searchtext)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetInvoiceListReportExportByCompanyId(Guid companyid, DateTime? startdate, DateTime? enddate, string searchtext, string invostatus, string order, FilterReportModel filter)
        {
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }
            string sqlQuery = @"";
            string subquery = "";
            string statusquery = "";
            string datequery = "";
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                subquery = ("and cus.BusinessName like @SearchText or cus.DBA like @SearchText or cus.FirstName + ' ' + cus.LastName like @SearchText or invo.InvoiceId like @SearchText or invo.InvoiceDate like @SearchText  or invo.DueDate like @SearchText or invo.TotalAmount like @SearchText or invo.BalanceDue like @SearchText");
            }
            if (!string.IsNullOrWhiteSpace(invostatus) && invostatus != "'null'")
            {
                statusquery = string.Format("", invostatus);
            }
            if (!string.IsNullOrWhiteSpace(filter.convertmindate) && !string.IsNullOrWhiteSpace(filter.convertmaxdate))
            {
                var date = Convert.ToDateTime(filter.convertmaxdate);
                var datemin = Convert.ToDateTime(filter.convertmindate);
                datequery = string.Format("and invo.InvoiceDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.convertmindate))
            {
                var date = Convert.ToDateTime(filter.convertmindate);
                datequery = string.Format("and invo.InvoiceDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.convertmaxdate))
            {
                var date = Convert.ToDateTime(filter.convertmaxdate);
                datequery = string.Format("and invo.InvoiceDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            if (!string.IsNullOrWhiteSpace(filter.createmindate) && !string.IsNullOrWhiteSpace(filter.createmaxdate))
            {
                var date = Convert.ToDateTime(filter.createmaxdate);
                var datemin = Convert.ToDateTime(filter.createmindate);
                datequery = string.Format("and invo.DueDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.createmindate))
            {
                var date = Convert.ToDateTime(filter.createmindate);
                datequery = string.Format("and invo.DueDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.createmaxdate))
            {
                var date = Convert.ToDateTime(filter.createmaxdate);
                datequery = string.Format("and invo.DueDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            String ordersubquery1 = "";
            if (!string.IsNullOrWhiteSpace(order) && order != "undefined" && order != "null")
            {
                if (order == "ascending/invoicedate")
                {
                    ordersubquery1 = "order by [Invoice Date] asc";
                }
                else if (order == "descending/invoicedate")
                {
                    ordersubquery1 = "order by [Invoice Date] desc";
                }

                else if (order == "ascending/invoiceid")
                {
                    ordersubquery1 = "order by [Invoices] asc";
                }
                else if (order == "descending/invoiceid")
                {
                    ordersubquery1 = "order by [Invoices] desc";
                }

                else if (order == "ascending/duedate")
                {
                    ordersubquery1 = "order by [Due Date] asc";
                }
                else if (order == "descending/duedate")
                {
                    ordersubquery1 = "order by [Due Date] desc";
                }

                else if (order == "ascending/totalamount")
                {
                    ordersubquery1 = "order by [Invoice Amount] asc";
                }
                else if (order == "descending/totalamount")
                {
                    ordersubquery1 = "order by [Invoice Amount] desc";
                }

                else if (order == "ascending/amount")
                {
                    ordersubquery1 = "order by invo.Amount asc";
                }
                else if (order == "descending/amount")
                {
                    ordersubquery1 = "order by invo.Amount desc";
                }

                else if (order == "ascending/tax")
                {
                    ordersubquery1 = "order by invo.Tax asc";
                }
                else if (order == "descending/tax")
                {
                    ordersubquery1 = "order by invo.Tax desc";
                }

                else if (order == "ascending/balancedue")
                {
                    ordersubquery1 = "order by [Open Balance] asc";
                }
                else if (order == "descending/balancedue")
                {
                    ordersubquery1 = "order by [Open Balance] desc";
                }
            }
            else
            {
                ordersubquery1 = "order by [Invoice Date] desc,[Invoices] desc,[Customer Id] desc";
            }



            if (startdate.HasValue && enddate.HasValue && startdate.Value != new DateTime() && enddate.Value != new DateTime())
            {
                sqlQuery = @"declare @TotalAmount nvarchar(MAX)
                             declare @TotalTax nvarchar(MAX)
                             declare @TotalInvoiceAmount nvarchar(MAX)
                             declare @TotalBalanceDue nvarchar(MAX)
                                select distinct cus.Id as [Customer Id],{6} as [Customers],invo.InvoiceId as [Invoices],convert(date,invo.InvoiceDate) as [Invoice Date],convert(date,invo.DueDate) as [Due Date],invo.Amount as Amount,invo.Tax as Tax,invo.TotalAmount as [Invoice Amount],invo.BalanceDue as [Open Balance],

                                ISNULL((select top 1 CONVERT(DECIMAL(18,2),trn.Amount,0) from [TransactionHistory] thist left join [Transaction] trn on thist.TransactionId=trn.id 
	                                where thist.InvoiceId=invo.Id order by trn.TransacationDate desc),0) LastPaymentAmount,                                
                                (select top 1 trn.TransacationDate from [TransactionHistory] thist left join [Transaction] trn on thist.TransactionId=trn.id 
	                                where thist.InvoiceId=invo.Id order by trn.TransacationDate desc) LastPaymentDate,
                                (select top 1 DisplayText from Lookup Where DataKey='CommissionType' and DataValue=cus.SalesLocation) SalesLocation,
                                CASE 
									WHEN (SELECT TOP 1 DisplayText FROM Lookup WHERE DataKey='InvoiceForList' AND DataValue=invo.InvoiceFor) = 'Select One' THEN ''
									ELSE (SELECT TOP 1 DisplayText FROM Lookup WHERE DataKey='InvoiceForList' AND DataValue=invo.InvoiceFor)
								END AS InvoiceFor 
                                --invo.InvoiceFor

                                into #customerdata from Customer cus
                                left join CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                left join Invoice invo on invo.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}' and invo.Status != 'Init' and invo.Status !='Cancelled' and  invo.Status !='Rolled Over'
                                and cc.IsLead = 0 and invo.IsEstimate = 0
                                and ce.IsTestAccount != 1
                                and invo.InvoiceDate between '{1}' and '{2}'
                                {3}
                                {4}
                                {7}
                                set @TotalAmount = (select (CONVERT(DECIMAL(18,2),SUM(Amount))) from #customerdata)
			                    set @TotalTax = (select (CONVERT(DECIMAL(18,2),SUM(Tax))) from #customerdata)
			                    set @TotalInvoiceAmount = (select (CONVERT(DECIMAL(18,2),SUM([Invoice Amount]))) from #customerdata)
			                    set @TotalBalanceDue = (select (CONVERT(DECIMAL(18,2),SUM([Open Balance]))) from #customerdata)

								insert into #customerdata ([Customer Id], [Invoice Date], [Customers], [Invoices], [Due Date], Amount, Tax, [Invoice Amount], [Open Balance],[LastPaymentAmount]) 
								values(0, NULL, 'Total', '', NULL, @TotalAmount, @TotalTax, @TotalInvoiceAmount, @TotalBalanceDue, 0)

                                select * from #customerdata
                                
                                {5}


								drop table #customerdata";
                sqlQuery = string.Format(sqlQuery, companyid, startdate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), enddate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), subquery, statusquery, ordersubquery1, NameSql, datequery);
            }
            else
            {
                sqlQuery = @"declare @TotalAmount nvarchar(MAX)
                             declare @TotalTax nvarchar(MAX)
                             declare @TotalInvoiceAmount nvarchar(MAX)
                             declare @TotalBalanceDue nvarchar(MAX)
                                select distinct cus.Id as [Customer Id],{4} as [Customers],invo.InvoiceId as [Invoices],convert(date,invo.InvoiceDate) as [Invoice Date],convert(date,invo.DueDate) as [Due Date],invo.Amount as Amount,invo.Tax as Tax,invo.TotalAmount as [Invoice Amount],invo.BalanceDue as [Open Balance],

                                    ISNULL((select top 1 CONVERT(DECIMAL(18,2),trn.Amount,0) from [TransactionHistory] thist left join [Transaction] trn on thist.TransactionId=trn.id 
	                                where thist.InvoiceId=invo.Id order by trn.TransacationDate desc),0) LastPaymentAmount,                                
                                    (select top 1 trn.TransacationDate from [TransactionHistory] thist left join [Transaction] trn on thist.TransactionId=trn.id 
	                                where thist.InvoiceId=invo.Id order by trn.TransacationDate desc) LastPaymentDate,
                                (select top 1 DisplayText from Lookup Where DataKey='CommissionType' and DataValue=cus.SalesLocation) SalesLocation,
                                CASE 
									WHEN (SELECT TOP 1 DisplayText FROM Lookup WHERE DataKey='InvoiceForList' AND DataValue=invo.InvoiceFor) = 'Select One' THEN ''
									ELSE (SELECT TOP 1 DisplayText FROM Lookup WHERE DataKey='InvoiceForList' AND DataValue=invo.InvoiceFor)
								END AS InvoiceFor
                                --invo.InvoiceFor

                                into #customerdata from Customer cus
                                left join CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                left join Invoice invo on invo.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}' and invo.Status != 'Init' and invo.Status !='Cancelled' and  invo.Status !='Rolled Over'
                                and cc.IsLead = 0 and invo.IsEstimate = 0
                                and ce.IsTestAccount != 1
                                {1}
                                {2}
                                {5}
                                    set @TotalAmount = (select (CONVERT(DECIMAL(18,2),SUM(Amount))) from #customerdata)
			                        set @TotalTax = (select (CONVERT(DECIMAL(18,2),SUM(Tax))) from #customerdata)
			                        set @TotalInvoiceAmount = (select (CONVERT(DECIMAL(18,2),SUM([Invoice Amount]))) from #customerdata)
			                        set @TotalBalanceDue = (select (CONVERT(DECIMAL(18,2),SUM([Open Balance]))) from #customerdata)

								insert into #customerdata ([Customer Id], [Invoice Date], [Customers], [Invoices], [Due Date], Amount, Tax, [Invoice Amount], [Open Balance], [LastPaymentAmount]) 
								values(0, NULL, 'Total', '', NULL, @TotalAmount, @TotalTax, @TotalInvoiceAmount, @TotalBalanceDue, 0)

                                select * from #customerdata
                                
                                {3}


								drop table #customerdata";
                sqlQuery = string.Format(sqlQuery, companyid, subquery, statusquery, ordersubquery1, NameSql, datequery);
            }

            try
            {
                sqlQuery = string.Format(sqlQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", searchtext)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetEstimateListReportExportByCompanyId(Guid companyid, DateTime? startdate, DateTime? enddate, string searchtext, string invostatus, string order, FilterReportModel filter)
        {
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }
            string sqlQuery = @"";
            string subquery = "";
            string statusquery = "";
            string datequery = "";
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                subquery = string.Format("and cus.BusinessName like '%{0}%' or cus.DBA like '%{0}%' or cus.FirstName + ' ' + cus.LastName like '%{0}%' or invo.InvoiceId like '%{0}%' or invo.InvoiceDate like '%{0}%'  or invo.DueDate like '%{0}%' or invo.TotalAmount like '%{0}%' or invo.BalanceDue like '%{0}%'", searchtext);
            }
            if (!string.IsNullOrWhiteSpace(invostatus) && invostatus != "'null'" && invostatus != "null")
            {
                statusquery = string.Format("and invo.Status in ({0})", invostatus);
            }
            //if (!string.IsNullOrWhiteSpace(filter.convertmindate) && !string.IsNullOrWhiteSpace(filter.convertmaxdate))
            //{
            //    var date = Convert.ToDateTime(filter.convertmaxdate);
            //    var datemin = Convert.ToDateTime(filter.convertmindate);
            //    datequery = string.Format("and invo.InvoiceDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            //}
            //else if (!string.IsNullOrWhiteSpace(filter.convertmindate))
            //{
            //    var date = Convert.ToDateTime(filter.convertmindate);
            //    datequery = string.Format("and invo.InvoiceDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            //}
            //else if (!string.IsNullOrWhiteSpace(filter.convertmaxdate))
            //{
            //    var date = Convert.ToDateTime(filter.convertmaxdate);
            //    datequery = string.Format("and invo.InvoiceDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            //}
            //if (!string.IsNullOrWhiteSpace(filter.createmindate) && !string.IsNullOrWhiteSpace(filter.createmaxdate))
            //{
            //    var date = Convert.ToDateTime(filter.createmaxdate);
            //    var datemin = Convert.ToDateTime(filter.createmindate);
            //    datequery = string.Format("and invo.DueDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            //}
            //else if (!string.IsNullOrWhiteSpace(filter.createmindate))
            //{
            //    var date = Convert.ToDateTime(filter.createmindate);
            //    datequery = string.Format("and invo.DueDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            //}
            //else if (!string.IsNullOrWhiteSpace(filter.createmaxdate))
            //{
            //    var date = Convert.ToDateTime(filter.createmaxdate);
            //    datequery = string.Format("and invo.DueDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            //}
            String ordersubquery1 = "";
            if (!string.IsNullOrWhiteSpace(order) && order != "undefined" && order != "null")
            {
                if (order == "ascending/invoicedate")
                {
                    ordersubquery1 = "order by [Invoice Date] asc";
                }
                else if (order == "descending/invoicedate")
                {
                    ordersubquery1 = "order by [Invoice Date] desc";
                }

                else if (order == "ascending/invoiceid")
                {
                    ordersubquery1 = "order by [Invoice Number] asc";
                }
                else if (order == "descending/invoiceid")
                {
                    ordersubquery1 = "order by [Invoice Number] desc";
                }

                else if (order == "ascending/duedate")
                {
                    ordersubquery1 = "order by [Due Date] asc";
                }
                else if (order == "descending/duedate")
                {
                    ordersubquery1 = "order by [Due Date] desc";
                }

                else if (order == "ascending/totalamount")
                {
                    ordersubquery1 = "order by [Invoice Amount] asc";
                }
                else if (order == "descending/totalamount")
                {
                    ordersubquery1 = "order by [Invoice Amount] desc";
                }

                else if (order == "ascending/amount")
                {
                    ordersubquery1 = "order by invo.Amount asc";
                }
                else if (order == "descending/amount")
                {
                    ordersubquery1 = "order by invo.Amount desc";
                }

                else if (order == "ascending/tax")
                {
                    ordersubquery1 = "order by invo.Tax asc";
                }
                else if (order == "descending/tax")
                {
                    ordersubquery1 = "order by invo.Tax desc";
                }

                else if (order == "ascending/balancedue")
                {
                    ordersubquery1 = "order by [Open Balance] asc";
                }
                else if (order == "descending/balancedue")
                {
                    ordersubquery1 = "order by [Open Balance] desc";
                }
                else
                {
                    ordersubquery1 = "order by [Created On] desc";
                }
            }
            else
            {
                ordersubquery1 = "order by [Created On] desc";
            }



            if (startdate.HasValue && enddate.HasValue && startdate.Value != new DateTime() && enddate.Value != new DateTime())
            {
                sqlQuery = @"declare @TotalInvoiceAmount nvarchar(MAX)
                                select distinct cus.Id as [Customer Id],{6} as [Customer Name], invo.Status,convert(date,invo.CreatedDate) as [Created On],invo.InvoiceId as [Estimate Number],format(invo.TotalAmount,'N2') as [Estimate Amount]

                                into #customerdata from Customer cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                left join Invoice invo on invo.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}' and invo.Status != 'Init' and invo.Status !='Cancelled' and  invo.Status !='Rolled Over'
                                and invo.IsEstimate = 1
                                and invo.CreatedDate between '{1}' and '{2}'
                                {3}
                                {4}
                                {7}
                                
								set @TotalInvoiceAmount = (select FORMAT(SUM(CONVERT(DECIMAL(18,2),replace([Estimate Amount], ',', ''))), 'N2') from #customerdata)


								insert into #customerdata ([Customer Id], [Status], [Created On], [Customer Name], [Estimate Number], [Estimate Amount]) 
								values(0, NULL,NULL , 'Total', NULL, @TotalInvoiceAmount)

                                select * from #customerdata
                                
                                {5}


								drop table #customerdata";
                sqlQuery = string.Format(sqlQuery, companyid, startdate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), enddate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), subquery, statusquery, ordersubquery1, NameSql, datequery);
            }
            else
            {
                sqlQuery = @"declare @TotalInvoiceAmount nvarchar(MAX)
select distinct cus.Id as [Customer Id],{4} as [Customer Name], invo.Status,convert(date,invo.CreatedDate) as [Created On],invo.InvoiceId as [Estimate Number],format(invo.TotalAmount,'N2') as [Estimate Amount]

                                into #customerdata from Customer cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                left join Invoice invo on invo.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}' and invo.Status != 'Init' and invo.Status !='Cancelled' and  invo.Status !='Rolled Over'
                                and invo.IsEstimate = 1
                                {1}
                                {2}
                                {5}
                                
								set @TotalInvoiceAmount = (select FORMAT(SUM(CONVERT(DECIMAL(18,2),replace([Estimate Amount], ',', ''))), 'N2') from #customerdata)
						

								insert into #customerdata ([Customer Id], [Status], [Created On], [Customer Name], [Estimate Number], [Estimate Amount]) 
								values(0, NULL,NULL , 'Total', NULL, @TotalInvoiceAmount)

                                select * from #customerdata
                                
                                {3}


								drop table #customerdata";
                sqlQuery = string.Format(sqlQuery, companyid, subquery, statusquery, ordersubquery1, NameSql, datequery);
            }

            try
            {
                sqlQuery = string.Format(sqlQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetTechEquipmentListReportExportByCompanyId(Guid companyid, DateTime? startdate, DateTime? enddate, string searchtext, Guid TechnicianId, int ActiveStatus, int EquipmentClass, int EquipmentCategory)
        {
            string sqlQuery = @"";
            string subquery = "";
            string filterByActiveStatus = "";
            string filterByEquipmentClass = "";
            string filterByEquipmentType = "";


            if (EquipmentCategory != -1)
            {
                filterByEquipmentType = string.Format("AND _eqp.EquipmentTypeId = '{0}'", EquipmentCategory);
            }

            if (EquipmentClass != -1)
            {
                filterByEquipmentClass = string.Format("AND _eqp.EquipmentClassId = '{0}'", EquipmentClass);
            }
            if (ActiveStatus == -1)
            {
                filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");
            }
            else
            {
                if (ActiveStatus == 1)
                {
                    filterByActiveStatus = string.Format("AND _eqp.IsActive = 1");
                }
                if (ActiveStatus == 0)
                {
                    filterByActiveStatus = string.Format("AND _eqp.IsActive = 0");
                }

            }
            if (!string.IsNullOrWhiteSpace(searchtext) && searchtext != "undefined")
            {
                subquery = string.Format("and (_eqp.Name like '%{0}%' or _eqpType.Name like '%{0}%' or _eqpClass.Name like '%{0}%' or manu.Name like '%{0}%' or sup.Name like '%{0}%')", searchtext);
            }
            if (startdate.HasValue && enddate.HasValue && startdate.Value != new DateTime() && enddate.Value != new DateTime())
            {
                sqlQuery = @"   SELECT DISTINCT
                                        _eqp.Name,_eqpType.Name as Category,_eqp.RepCost,
                                        _eqpClass.Name as EquipmentClass,
										 ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId='{4}')-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId='{4}')) Quantity,
										manu.Name as ManufacturerName,
	                                    sup.Name as SupplierName,_eqp.SKU,
										case 
											when _eqp.EquipmentClassId = 1 then 'Equipment'
											when _eqp.EquipmentClassId = 2 then 'Service'
											else '-'
										end as Type
                                        
                                          FROM InventoryTech _inv
                                          LEFT JOIN Equipment _eqp
                                            ON _eqp.CompanyId = _inv.CompanyId and _eqp.EquipmentId = _inv.EquipmentId
                                            LEFT JOIN EquipmentClass _eqpClass
		                                    ON _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                                            left join Supplier sup on sup.Id = _eqp.SupplierId
                                            LEFT JOIN EquipmentType _eqpType
		                                    ON _eqp.EquipmentTypeId = _eqpType.Id and _eqp.CompanyId = _eqpType.CompanyId		                                    
                                            left join Manufacturer manu
											ON manu.Id = _eqp.ManufacturerId
                                                WHERE 
			                                    _eqp.CompanyId = '{0}'
                                                AND ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId='{4}')-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId='{4}'))>0
                                                 And _inv.TechnicianId='{4}'
                                                AND _eqp.IsActive = 1
                                and _eqp.CreatedDate between '{1}' and '{2}'
                                {3}
                                {5}
                                {6}
                                {7}

                                ";
                sqlQuery = string.Format(sqlQuery, companyid, startdate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), enddate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), subquery, TechnicianId, filterByActiveStatus, filterByEquipmentClass, filterByEquipmentType);
            }
            else
            {
                sqlQuery = @"  SELECT DISTINCT
                                        _eqp.Name,_eqpType.Name as Category,_eqp.RepCost,
                                        _eqpClass.Name as EquipmentClass,
										 ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId='{2}')-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId='{2}')) Quantity,
										manu.Name as ManufacturerName,
	                                    sup.Name as SupplierName,_eqp.SKU,
										case 
											when _eqp.EquipmentClassId = 1 then 'Equipment'
											when _eqp.EquipmentClassId = 2 then 'Service'
											else '-'
										end as Type
                                        
                                          FROM InventoryTech _inv
                                          LEFT JOIN Equipment _eqp
                                            ON _eqp.CompanyId = _inv.CompanyId and _eqp.EquipmentId = _inv.EquipmentId
                                            LEFT JOIN EquipmentClass _eqpClass
		                                    ON _eqp.EquipmentClassId = _eqpClass.Id and _eqp.CompanyId = _eqpClass.CompanyId
                                            left join Supplier sup on sup.Id = _eqp.SupplierId
                                            LEFT JOIN EquipmentType _eqpType
		                                    ON _eqp.EquipmentTypeId = _eqpType.Id and _eqp.CompanyId = _eqpType.CompanyId		                                    
                                            left join Manufacturer manu
											ON manu.Id = _eqp.ManufacturerId
                                                WHERE 
			                                    _eqp.CompanyId = '{0}'
                                                AND ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Add'  And invinner.TechnicianId='{2}')-(Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=_eqp.EquipmentId and Type='Release'  And invinner.TechnicianId='{2}'))>0
                                                 And _inv.TechnicianId='{2}'
                                                AND _eqp.IsActive = 1
                                {1}
                                {3}
                                {4}
                                {5}
                                ";
                sqlQuery = string.Format(sqlQuery, companyid, subquery, TechnicianId, filterByActiveStatus, filterByEquipmentClass, filterByEquipmentType);
            }

            try
            {
                sqlQuery = string.Format(sqlQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public DataTable GetBookingSalesReportExportByCompanyId(Guid companyid, DateTime? startdate, DateTime? enddate, string searchtext, string source)
        {
            string sqlQuery = @"";
            string subquery = "";
            string statusquery = "";
            string tempQuery = "";
            if (!string.IsNullOrWhiteSpace(source) && source != "'null'" && source != "null" && source != "'undefined'")
            {
                string[] TempSource = source.Split(',');
                if (source.Contains("Online") && TempSource.Length == 1)
                {
                    tempQuery += " and Bking.BookingSource = 'Online'";
                }
                else if (source.Contains("System") && TempSource.Length == 1)
                {
                    tempQuery += " and (Bking.BookingSource is null or Bking.BookingSource != 'Online')";
                }
            }
            //if (!string.IsNullOrWhiteSpace(invostatus) && invostatus != "'null'")
            //{
            //    statusquery = string.Format("and invo.[Status] in ({0})", invostatus);
            //}
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                subquery = string.Format("and (Bking.BookingId like '%{0}%' or FirstName like '%{0}%' or Bking.Message like '%{0}%' or invo.PaymentType like '%{0}%' or invo.InvoiceId like '%{0}%')", searchtext);
            }

            sqlQuery = @"   select  Bking.BookingId,cus.FirstName + ' ' + cus.LastName as CustomerName,iif(Bking.BookingSource = 'Online', 'Online', 'System Generated') as [Source],cus.PrimaryPhone,cus.SecondaryPhone,cus.EmailAddress,
								cus.Street,cus.City,cus.State,cus.ZipCode,invo.InvoiceId,invo.PaymentType as PaymentMethod,
								(STUFF((
										SELECT ', '  + BkingD.RugType + ' ' 
										+ CASE WHEN BkingD.Length is not null 
											THEN CONVERT(nvarchar(50),BkingD.Length) + '''' ELSE '' END  
										+ CASE WHEN BkingD.LengthInch is not null 
											THEN CONVERT(nvarchar(50),BkingD.LengthInch) + '{1}' + ' by ' ELSE '' END 
                                        + CASE WHEN BkingD.Width is not null

                                            THEN CONVERT(nvarchar(50), BkingD.Width) +'''' ELSE '' END
                                        + CASE WHEN BkingD.WidthInch is not null

                                            THEN CONVERT(nvarchar(50), BkingD.WidthInch) +'{1}' ELSE '' END
                                        + CASE WHEN BkingD.Radius is not null

                                            THEN CONVERT(nvarchar(50), BkingD.Radius)  +'''' ELSE '' END
                                        + CASE WHEN BkingD.RadiusInch is not null

                                            THEN CONVERT(nvarchar(50), BkingD.RadiusInch) +'{1}' ELSE '' END
                                        FROM BookingDetails BkingD where BkingD.BookingId = Bking.BookingId

                                        FOR XML PATH('')
										), 1, 2, '')
									) AS RugType, (invo.TotalAmount - invo.BalanceDue) as AmountPaid, invo.BalanceDue,
                                (STUFF((
                                        SELECT ', ' + 'ID:' + CONVERT(nvarchar(50), TicT.Id) +
                                        '- ' + TicT.TicketType + ' '

                                        FROM Ticket  TicT where TicT.BookingId = Bking.BookingId

                                        FOR XML PATH('')
                                        ), 1, 2, '')
									) AS TicketID, Bking.Message


                                  from Booking Bking
								left join Customer cus on Bking.CustomerId = cus.CustomerId
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
								--left join BookingDetails BkingD on BkingD.BookingId = Bking.BookingId
								--left join Ticket TicT on TicT.BookingId = Bking.BookingId
                                left join Invoice invo on invo.BookingId = Bking.BookingId
                                where cc.CompanyId = '{0}'
                                and Bking.Status='Approved'
                                {8}
                                {7}
                                {3}
                                {9}
                                ";
            //sqlQuery = string.Format(sqlQuery, companyid, subquery, statusquery);
            string OpenTicketById = string.Format(@"<a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenTicketById('{0}')"">'{0}'</a>", " + CONVERT(nvarchar(50), TicT.Id) +");
            string setext = "";
            string subquery1 = "";
            string pageno = "";
            string pagesize = "";

            string dateFilter = "";
            if (startdate.HasValue && enddate.HasValue && startdate.Value != new DateTime() && enddate.Value != new DateTime())
            {
                dateFilter = string.Format(@"and Bking.CreatedDate >= '{0}' and Bking.CreatedDate <= '{1}'", startdate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), enddate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }

            sqlQuery = string.Format(sqlQuery, companyid, '"', pagesize, subquery, statusquery, OpenTicketById, subquery1, setext, dateFilter, tempQuery);
            try
            {
                sqlQuery = string.Format(sqlQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetCollectionReportExportByCompanyId(Guid companyid, DateTime? startdate, DateTime? enddate, string searchtext,int salesCommission, string invostatus, string order, FilterReportModel filter)
        {
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }
            string sqlQuery = @"";
            string subquery1 = "";
            string setext = "";
            string statusquery = "";
            string datequery = "";
            string methodquery = "";
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                setext = "and (cus.BusinessName like @SearchText or cus.DBA like @SearchText or cus.FirstName + ' ' + cus.LastName like @SearchText or invo.InvoiceId like @SearchText or invo.InvoiceDate like @SearchText  or invo.DueDate like @SearchText or invo.TotalAmount like @SearchText or invo.BalanceDue like @SearchText)";
            }
            if (!string.IsNullOrWhiteSpace(invostatus) && invostatus != "'null'" && invostatus != "'undefined'")
            {
                statusquery = string.Format("and invo.[Status] in ({0})", invostatus);
            }
            if (salesCommission > 0)
            {
                statusquery = string.Format("and cus.SalesLocation = {0}", salesCommission);
            }
            if (!string.IsNullOrWhiteSpace(filter.convertmaxdate) && !string.IsNullOrWhiteSpace(filter.convertmindate))
            {
                var datemin = Convert.ToDateTime(filter.convertmindate);
                var date = Convert.ToDateTime(filter.convertmaxdate);
                datequery += string.Format("and invo.InvoiceDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.convertmindate))
            {
                var date = Convert.ToDateTime(filter.convertmindate);
                datequery += string.Format("and invo.InvoiceDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.convertmaxdate))
            {
                var date = Convert.ToDateTime(filter.convertmaxdate);
                datequery += string.Format("and invo.InvoiceDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            if (!string.IsNullOrWhiteSpace(filter.createmaxdate) && !string.IsNullOrWhiteSpace(filter.createmindate))
            {
                var datemin = Convert.ToDateTime(filter.createmindate);
                var date = Convert.ToDateTime(filter.createmaxdate);
                datequery += string.Format("and invo.DueDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.createmindate))
            {
                var date = Convert.ToDateTime(filter.createmindate);
                datequery += string.Format("and invo.DueDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.createmaxdate))
            {
                var date = Convert.ToDateTime(filter.createmaxdate);
                datequery += string.Format("and invo.DueDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }

            if (!string.IsNullOrWhiteSpace(filter.collectionmaxdate) && !string.IsNullOrWhiteSpace(filter.collectionmindate))
            {
                var datemin = Convert.ToDateTime(filter.collectionmindate);
                var date = Convert.ToDateTime(filter.collectionmaxdate);
                datequery += string.Format(" and tr.TransacationDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.collectionmindate))
            {
                var date = Convert.ToDateTime(filter.collectionmindate);
                datequery += string.Format(" and tr.TransacationDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.collectionmaxdate))
            {
                var date = Convert.ToDateTime(filter.collectionmaxdate);
                datequery += string.Format(" and tr.TransacationDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            sqlQuery = @"select cus.Id as [Customer Id],convert(date,invo.InvoiceDate) as [Invoice Date], {6} as [Customer Name],invo.InvoiceId as [Invoice Number], lksalesloc.DisplayText as [Sales Location], convert(date,invo.DueDate) as [Due Date],invo.Amount as Amount,invo.Tax as Tax,invo.TotalAmount as [Invoice Amount],                                (STUFF((
										SELECT ', '  +  Transa.PaymentMethod + ' '
										FROM TransactionHistory  TranH
										left join [Transaction] Transa on TranH.TransactionId = Transa.Id
										where TranH.InvoiceId = invo.Id
										
										FOR XML PATH('')
										), 1, 2, '')
									) AS [Payment Method],invo.BalanceDue as [Open Balance],
                                ISNULL(th.Amout,0) as [Amount Collected],
                                --ISNULL((invo.TotalAmount - invo.BalanceDue), 0) as [Amount Collected],
                                convert(date,tr.TransacationDate) as [Collection Date]
                                from Customer cus
                                left join CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                left join Invoice invo on invo.CustomerId = cus.CustomerId
                                left join Lookup lksalesloc on  lksalesloc.DataKey ='CommissionType'  
                                and lksalesloc.DataValue = iif(cus.SalesLocation != '-1', cus.SalesLocation, null)       
                               LEFT JOIN [TransactionHistory] th on th.InVoiceId=invo.Id
								LEFT JOIN [Transaction] tr on tr.Id=th.TransactionId
                                where cc.CompanyId = '{0}' and (invo.Status ='Paid' or  invo.Status ='Partial')
                                and cc.IsLead = 0 and invo.IsEstimate = 0
                                and ce.IsTestAccount != 1
                                {5}
                                {4}
                                {1}
                                {7}
                                {3}";

            if (!string.IsNullOrWhiteSpace(order) && order != "undefined" && order != "null")
            {
                if (order == "ascending/invoicedate")
                {
                    subquery1 = "order by [Invoice Date] asc";
                }
                else if (order == "descending/invoicedate")
                {
                    subquery1 = "order by [Invoice Date] desc";
                }
                else if (order == "ascending/firstname")
                {
                    subquery1 = "order by cus.FirstName asc";
                }
                else if (order == "descending/firstname")
                {
                    subquery1 = "order by cus.FirstName desc";
                }

                else if (order == "ascending/invoiceid")
                {
                    subquery1 = "order by [Invoice Number] asc";
                }
                else if (order == "descending/invoiceid")
                {
                    subquery1 = "order by [Invoice Number] desc";
                }

                else if (order == "ascending/duedate")
                {
                    subquery1 = "order by [Due Date] asc";
                }
                else if (order == "descending/duedate")
                {
                    subquery1 = "order by [Due Date] desc";
                }

                else if (order == "ascending/totalamount")
                {
                    subquery1 = "order by [Invoice Amount] asc";
                }
                else if (order == "descending/totalamount")
                {
                    subquery1 = "order by [Invoice Amount] desc";
                }

                else if (order == "ascending/amount")
                {
                    subquery1 = "order by invo.Amount asc";
                }
                else if (order == "descending/amount")
                {
                    subquery1 = "order by invo.Amount desc";
                }

                else if (order == "ascending/tax")
                {
                    subquery1 = "order by invo.Tax asc";
                }
                else if (order == "descending/tax")
                {
                    subquery1 = "order by invo.Tax desc";
                }
                else if (order == "ascending/balancedue")
                {
                    subquery1 = "order by [Open Balance] asc";
                }
                else if (order == "descending/balancedue")
                {
                    subquery1 = "order by [Open Balance] desc";
                }

                else if (order == "ascending/paymentmethod")
                {
                    subquery1 = "order by [Payment Method] asc";
                }
                else if (order == "descending/paymentmethod")
                {
                    subquery1 = "order by [Payment Method] desc";
                }

                else if (order == "ascending/paidbalance")
                {
                    subquery1 = "order by [Amount Collected] asc";
                }
                else if (order == "descending/paidbalance")
                {
                    subquery1 = "order by [Amount Collected] desc";
                }
            }
            else
            {
                subquery1 = "order by [Invoice Date] desc,[Invoice Number] desc,cus.Id desc";
            }

            string dateFilter = "";
            if (startdate.HasValue && enddate.HasValue && startdate.Value != new DateTime() && enddate.Value != new DateTime())
            {
                dateFilter = string.Format(@"and invo.InvoiceDate >= '{0}' and invo.InvoiceDate <= '{1}'", startdate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), enddate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            string OpenTicketById = string.Format(@"<a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenTicketById('{0}')"">'{0}'</a>", " + CONVERT(nvarchar(50), TicT.Id) +");
            sqlQuery = string.Format(sqlQuery, companyid, statusquery, OpenTicketById, subquery1, setext, dateFilter, NameSql, datequery);
            try
            {
                sqlQuery = string.Format(sqlQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", searchtext)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable GetPartnerReportExportByCompanyId(Guid companyid, DateTime? startdate, DateTime? enddate, string eiDList)
        {
            string sqlQuery = @"";



            sqlQuery = @"   select  (case when cc.IsLead = 1 then 'Lead' else 'Customer' end) as Type, cus.FirstName + ' ' + cus.LastName as FullName,cus.PrimaryPhone,cus.SecondaryPhone,cus.EmailAddress,
                                 cus.Street,cus.City,cus.State,cus.ZipCode,cus.Status,cus.CreatedDate,cus.InstalledStatus,
								(select emp.FirstName + ' ' + emp.LastName  from Employee emp where emp.UserId = cus.CreatedByUid) as CreatedByEmpName
                                from Customer cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}'
                                and cus.Soldby in ({1})
                                
                                {2}
                                ";

            string dateFilter = "";
            if (startdate.HasValue && enddate.HasValue && startdate.Value != new DateTime() && enddate.Value != new DateTime())
            {
                dateFilter = string.Format(@"and cus.CreatedDate >= '{0}' and cus.CreatedDate <= '{1}'", startdate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), enddate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }


            sqlQuery = string.Format(sqlQuery, companyid, eiDList, dateFilter);
            try
            {
                sqlQuery = string.Format(sqlQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable GetLeadSourceReportExportByCompanyId(Guid companyid, DateTime? startdate, DateTime? enddate, string eiDList)
        {
            string sqlQuery = @"";



            sqlQuery = @"   select  (case when cc.IsLead = 1 then 'Lead' else 'Customer' end) as Type, cus.FirstName + ' ' + cus.LastName as FullName,cus.PrimaryPhone,cus.SecondaryPhone,cus.EmailAddress,
                                 cus.Street,cus.City,cus.State,cus.ZipCode,cus.Status,cus.CreatedDate,cus.InstalledStatus,
								(select emp.FirstName + ' ' + emp.LastName  from Employee emp where emp.UserId = cus.CreatedByUid) as CreatedByEmpName
                                from Customer cus
                                left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
                                where cc.CompanyId = '{0}'
                                and cus.LeadSource in ({1})
                                
                                {2}
                                ";

            string dateFilter = "";
            if (startdate.HasValue && enddate.HasValue && startdate.Value != new DateTime() && enddate.Value != new DateTime())
            {
                dateFilter = string.Format(@"and cus.CreatedDate >= '{0}' and cus.CreatedDate <= '{1}'", startdate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), enddate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }


            sqlQuery = string.Format(sqlQuery, companyid, eiDList, dateFilter);
            try
            {
                sqlQuery = string.Format(sqlQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }




        public DataTable GetCustomerPackageServiceByCustomerId(Guid customerid)
        {
            string sqlQuery = @"select eq.Name as EquipmentServiceName, cps.MonthlyRate, cps.Total, cps.DiscountRate from customerpackageservice cps
                                left join Equipment eq on eq.EquipmentId = cps.EquipmentId
                                left join Package pa on pa.PackageId = cps.packageId
                                where cps.CustomerId = '{0}'";

            try
            {
                sqlQuery = string.Format(sqlQuery, customerid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetCustomerPackageEqpByCustomerId(Guid customerid)
        {
            string sqlQuery = @"select eq.Name as EquipmentServiceName, cpe.UnitPrice, cpe.DiscountPckage, cpe.Total from CustomerPackageEqp cpe
                                left join Equipment eq on eq.EquipmentId = cpe.EquipmentId
                                where cpe.CustomerId = '{0}'";

            try
            {
                sqlQuery = string.Format(sqlQuery, customerid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool DeleteAllTimeClockSupervisor(Guid userlist)
        {
            string sqlQuery = @"delete from EmployeeTimeClockSupervisor where UserId = '{0}'";

            try
            {
                sqlQuery = string.Format(sqlQuery, userlist);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public DataSet GetFirsCallCloseMatrixWithCount(DateTime? StartDate, DateTime? EndDate, int pageno, int pagesize, string order)
        {
            string sqlQuery = @"";
            string createddateSubQuery = "";
            string joindateSubQuery = "";
            string orderquery = "";
            string orderquery1 = "";
            if (StartDate.HasValue && EndDate.HasValue && StartDate != new DateTime() && EndDate != new DateTime())
            {
                createddateSubQuery = string.Format(" and cus.CreatedDate between '{0}' and '{1}'", StartDate.Value, EndDate.Value);
                joindateSubQuery = string.Format(" and cus.SalesDate between '{0}' and '{1}'", StartDate.Value, EndDate.Value);
            }

            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/salesperson")
                {
                    orderquery = "order by #cd.[EmployeeName] asc";
                    orderquery1 = "order by [EmployeeName] asc";
                }
                else if (order == "descending/salesperson")
                {
                    orderquery = "order by #cd.[EmployeeName] desc";
                    orderquery1 = "order by [EmployeeName] desc";
                }
                else if (order == "ascending/leads")
                {
                    orderquery = "order by #cd.NoOfLeads asc";
                    orderquery1 = "order by NoOfLeads asc";
                }
                else if (order == "descending/leads")
                {
                    orderquery = "order by #cd.NoOfLeads desc";
                    orderquery1 = "order by NoOfLeads desc";
                }
                else if (order == "ascending/closed")
                {
                    orderquery = "order by #cd.[Closing] asc";
                    orderquery1 = "order by [Closing] asc";
                }
                else if (order == "descending/closed")
                {
                    orderquery = "order by #cd.[Closing] desc";
                    orderquery1 = "order by [Closing] desc";
                }
                else if (order == "ascending/closing")
                {
                    orderquery = "order by #cd.[Percentage] asc";
                    orderquery1 = "order by [Percentage] asc";
                }
                else if (order == "descending/closing")
                {
                    orderquery = "order by #cd.[Percentage] desc";
                    orderquery1 = "order by [Percentage] desc";
                }
                else if (order == "ascending/userx")
                {
                    orderquery = "order by #cd.[UserX] asc";
                    orderquery1 = "order by [UserX] asc";
                }
                else if (order == "descending/userx")
                {
                    orderquery = "order by #cd.[UserX] desc";
                    orderquery1 = "order by [UserX] desc";
                }



                else
                {
                    orderquery = "order by #cd.[Id]  desc";
                    orderquery1 = "order by Id desc";
                }

            }
            else
            {
                orderquery = "order by #cd.[Id] desc";
                orderquery1 = "order by Id desc";
            }
            #endregion
            sqlQuery = @"declare @pagestart int
                        declare @pageend int
                        set @pagestart=(@pageno-1)* @pagesize 
                        set @pageend = @pagesize

                        select cus.Id, cus.CustomerId, cus.Soldby1,cus.Status, CE.AppoinmentSetBy,ccclosing.IsLead,cus.SalesDate,CE.CreatedDay,cus.CreatedDate
						into #TempCustomer
						from customer cus 
						left join CustomerExtended CE on CE.Customerid = cus.Customerid
						LEFT JOIN CustomerCompany ccclosing on ccclosing.CustomerId=cus.CustomerId
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=cus.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText !='' and CE.IsTestAccount != 1
						
						select #tc.* into #TempCustomerBad
						From #TempCustomer #tc
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=#tc.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText = 'Bad' 

						select #tc.* into #TempCustomerGood
						From #TempCustomer #tc
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=#tc.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText = 'Good' 

                        select emp.UserId  into #TempEmployee From Employee emp
                        left join UserPermission up on up.UserId = emp.UserId
                        left join UserLogin u on u.UserId=up.UserId
                        left join PermissionGroup pg on pg.Id = up.PermissionGroupId
                        where emp.IsSalesMatrix=1 --and emp.IsCurrentEmployee=1

						select 
						emp.Id,u.Id as UserLoginId,emp.UserId as EmpId,
						emp.FirstName +' '+ emp.LastName as EmployeeName,

						(Select COUNT(cus.Id) from #TempCustomer cus
						where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId) {0}) TotalLeads,

						(Select COUNT(cus.Id) from #TempCustomerBad cus
						where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId) {0}) BadLeads,

						(Select COUNT(cus.Id) from #TempCustomerGood cus
						where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId) {0}) GoodLeads,

						(Select COUNT(cus.Id) from #TempCustomer cus 
						where (cus.Soldby1=emp.UserId) and CAST(cus.CreatedDay as date) = CAST(cus.SalesDate as date) and cus.IsLead=0  {1}) Closing,

						ISNULL(((Select COUNT(cus.Id) from #TempCustomer cus 
						where cus.Soldby1=emp.UserId and CAST(cus.CreatedDay as date) = CAST(cus.SalesDate as date) and cus.IsLead=0  {1}) * 100.0 /
						 NULLIF((Select COUNT(cus.Id) from #TempCustomerGood cus 
						 where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId ) {0}), 0)),0) as Percentage,

						(Select UserX from SalesMatrix sm where sm.Type='FirstCallCosingPecentage' and 
						ISNULL((Select COUNT(cus.Id) from #TempCustomer cus
						 where cus.Soldby1=emp.UserId and CAST(cus.CreatedDay as date) = CAST(cus.SalesDate as date) and cus.IsLead=0  {1})* 100.0 /
						(NULLIF((Select COUNT(cus.Id) from #TempCustomerGood cus
						where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId ) {0}), 0)),0) between sm.Min and sm.Max
						) as UserX
						 into #employeeData From Employee emp
						left join UserPermission up on up.UserId = emp.UserId
                        left join UserLogin u on u.UserId=up.UserId
						left join PermissionGroup pg on pg.Id = up.PermissionGroupId
						where emp.IsSalesMatrix=1 

                        select empdata.* into #employeeDataFinal
						from #employeeData empdata
						where empdata.TotalLeads>0
                        
                        select top(@pagesize) * from #employeeDataFinal
                        where Id not in (Select TOP (@pagestart)  Id from #employeeDataFinal #cd {2})
                        --order by Id desc

                        {3}
                        select COUNT(*) TotalEmployee from #employeeDataFinal

                        --select 
						--(Select COUNT(cus.Id) from #TempCustomer cus
						--where (cus.SoldBy1 in (select * from #TempEmployee) or cus.AppoinmentSetBy in (select * from #TempEmployee)) {0}) as TotalTotalLeads,
						--(Select COUNT(cus.Id) from #TempCustomerBad cus
						--where (cus.SoldBy1 in (select * from #TempEmployee) or cus.AppoinmentSetBy in (select * from #TempEmployee)) {0}) as TotalBadLeads,
                        --(Select COUNT(cus.Id) from #TempCustomerGood cus
						--where (cus.SoldBy1 in (select * from #TempEmployee) or cus.AppoinmentSetBy in (select * from #TempEmployee)) {0}) as TotalGoodLeads,
                        --(Select COUNT(cus.Id) from #TempCustomer cus 
						--where (cus.SoldBy1 in (select * from #TempEmployee)) and CAST(cus.CreatedDay as date) = CAST(cus.SalesDate as date) and cus.IsLead=0  {1}) as TotalClosing,
						--AVG(Percentage) as AvgPercentage,
						--AVG(UserX) as AvgUserX
						--from #employeeDataFinal

                        drop table #employeeData
                        drop table #employeeDataFinal
						drop table #TempCustomer
                        drop table #TempCustomerBad
						drop table #TempCustomerGood";
            try
            {
                sqlQuery = string.Format(sqlQuery, createddateSubQuery, joindateSubQuery, orderquery, orderquery1);
                //_EmployeeDataAccess.Insert(new ErrorLog() { ErrorId = Guid.NewGuid(), ErrorFor = "DA|Employee|GetFirsCallCloseMatrixWithCount", Message = string.Format("{0} | {1} | {2} | {3} | {4}", StartDate, EndDate, pageno, order, sqlQuery), TimeUtc=DateTime.Now });
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetFirsCallCloseMatrixWithCount(string Start, string End, int pageno, int pagesize, string order)
        {
            string sqlQuery = @"";
            string createddateSubQuery = "";
            string joindateSubQuery = "";
            string orderquery = "";
            string orderquery1 = "";
            if (!string.IsNullOrWhiteSpace(Start) && !string.IsNullOrWhiteSpace(End) && Start != "undefined" && End != "undefined")
            {
                createddateSubQuery = string.Format(" and cus.CreatedDay between '{0}' and '{1}'", Start, End);
                joindateSubQuery = string.Format(" and cus.SalesDate between '{0}' and '{1}'", Start, End);
            }

            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/salesperson")
                {
                    orderquery = "order by #cd.[EmployeeName] asc";
                    orderquery1 = "order by [EmployeeName] asc";
                }
                else if (order == "descending/salesperson")
                {
                    orderquery = "order by #cd.[EmployeeName] desc";
                    orderquery1 = "order by [EmployeeName] desc";
                }
                else if (order == "ascending/leads")
                {
                    orderquery = "order by #cd.NoOfLeads asc";
                    orderquery1 = "order by NoOfLeads asc";
                }
                else if (order == "descending/leads")
                {
                    orderquery = "order by #cd.NoOfLeads desc";
                    orderquery1 = "order by NoOfLeads desc";
                }
                else if (order == "ascending/closed")
                {
                    orderquery = "order by #cd.[Closing] asc";
                    orderquery1 = "order by [Closing] asc";
                }
                else if (order == "descending/closed")
                {
                    orderquery = "order by #cd.[Closing] desc";
                    orderquery1 = "order by [Closing] desc";
                }
                else if (order == "ascending/closing")
                {
                    orderquery = "order by #cd.[Percentage] asc";
                    orderquery1 = "order by [Percentage] asc";
                }
                else if (order == "descending/closing")
                {
                    orderquery = "order by #cd.[Percentage] desc";
                    orderquery1 = "order by [Percentage] desc";
                }
                else if (order == "ascending/userx")
                {
                    orderquery = "order by #cd.[UserX] asc";
                    orderquery1 = "order by [UserX] asc";
                }
                else if (order == "descending/userx")
                {
                    orderquery = "order by #cd.[UserX] desc";
                    orderquery1 = "order by [UserX] desc";
                }



                else
                {
                    orderquery = "order by #cd.[Id]  desc";
                    orderquery1 = "order by Id desc";
                }

            }
            else
            {
                orderquery = "order by #cd.[Id] desc";
                orderquery1 = "order by Id desc";
            }
            #endregion
            sqlQuery = @"declare @pagestart int
                        declare @pageend int
                        set @pagestart=(@pageno-1)* @pagesize 
                        set @pageend = @pagesize

                        select cus.Id, cus.CustomerId, cus.Soldby1,cus.Status, CE.AppoinmentSetBy,ccclosing.IsLead,cus.SalesDate,CE.CreatedDay
						into #TempCustomer
						from customer cus 
						left join CustomerExtended CE on CE.Customerid = cus.Customerid
						LEFT JOIN CustomerCompany ccclosing on ccclosing.CustomerId=cus.CustomerId
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=cus.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText !='' and CE.IsTestAccount != 1
						
						select #tc.* into #TempCustomerBad
						From #TempCustomer #tc
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=#tc.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText = 'Bad' 

						select #tc.* into #TempCustomerGood
						From #TempCustomer #tc
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=#tc.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText = 'Good' 

						select 
						emp.Id,u.Id as UserLoginId,emp.UserId as EmpId,
						emp.FirstName +' '+ emp.LastName as EmployeeName,

						(Select COUNT(cus.Id) from #TempCustomer cus
						where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId) {0}) TotalLeads,

						(Select COUNT(cus.Id) from #TempCustomerBad cus
						where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId) {0}) BadLeads,

						(Select COUNT(cus.Id) from #TempCustomerGood cus
						where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId) {0}) GoodLeads,

						(Select COUNT(cus.Id) from #TempCustomer cus 
						where (cus.Soldby1=emp.UserId) and CAST(cus.CreatedDay as date) = CAST(cus.SalesDate as date) and cus.IsLead=0  {1}) Closing,

						ISNULL(((Select COUNT(cus.Id) from #TempCustomer cus 
						where cus.Soldby1=emp.UserId and CAST(cus.CreatedDay as date) = CAST(cus.SalesDate as date) and cus.IsLead=0  {1}) * 100.0 /
						 NULLIF((Select COUNT(cus.Id) from #TempCustomerGood cus 
						 where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId ) {0}), 0)),0) as Percentage,

						(Select UserX from SalesMatrix sm where sm.Type='FirstCallCosingPecentage' and 
						ISNULL((Select COUNT(cus.Id) from #TempCustomer cus
						 where cus.Soldby1=emp.UserId and CAST(cus.CreatedDay as date) = CAST(cus.SalesDate as date) and cus.IsLead=0  {1})* 100.0 /
						(NULLIF((Select COUNT(cus.Id) from #TempCustomerGood cus
						where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId ) {0}), 0)),0) between sm.Min and sm.Max
						) as UserX
						 into #employeeData From Employee emp
						left join UserPermission up on up.UserId = emp.UserId
                        left join UserLogin u on u.UserId=up.UserId
						left join PermissionGroup pg on pg.Id = up.PermissionGroupId
						where emp.IsSalesMatrix=1 

                        select empdata.* into #employeeDataFinal
						from #employeeData empdata
						where empdata.TotalLeads>0
                        
                        select top(@pagesize) * from #employeeDataFinal
                        where Id not in (Select TOP (@pagestart)  Id from #employeeDataFinal #cd {2})
                        --order by Id desc

                        {3}
                        select COUNT(*) TotalEmployee from #employeeDataFinal

                        select 
						SUM(TotalLeads) as TotalTotalLeads,
						SUM(BadLeads) as TotalBadLeads,
                        SUM(GoodLeads) as TotalGoodLeads,
                        SUM(Closing) as TotalClosing,
						AVG(Percentage) as AvgPercentage,
						AVG(UserX) as AvgUserX
						from #employeeDataFinal

                        drop table #employeeData
                        drop table #employeeDataFinal
						drop table #TempCustomer
                        drop table #TempCustomerBad
						drop table #TempCustomerGood";
            try
            {
                sqlQuery = string.Format(sqlQuery, createddateSubQuery, joindateSubQuery, orderquery, orderquery1);
                //_EmployeeDataAccess.Insert(new ErrorLog() { ErrorId = Guid.NewGuid(), ErrorFor = "DA|Employee|GetFirsCallCloseMatrixWithCount", Message = string.Format("{0} | {1} | {2} | {3} | {4}", Start, End, pageno, order, sqlQuery), TimeUtc = DateTime.Now });
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetAllEmployeeForReportDownload(DateTime? StartDate, DateTime? EndDate, string Search, string[] DeptFilter, string[] StatusFilter, string InsuranceFilter)
        {
            string sqlQuery = "";
            string createddateSubQuery = "";
            string searchQuery = "";
            string deptQuery = "";
            string statusQuery = "";
            string insuranceQuery = "";

            if (StartDate.HasValue && EndDate.HasValue && StartDate != new DateTime() && EndDate != new DateTime())
            {
                createddateSubQuery = string.Format(" and emp.CreatedDate between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());

            }
            if (!string.IsNullOrEmpty(Search))
            {
                searchQuery = string.Format(" and (emp.FirstName like '%{0}%' or emp.LastName like '%{0}%' or emp.FirstName+' '+emp.LastName like '%{0}%' or emp.Email like '%{0}%') ", Search);
            }
            if (DeptFilter != null && DeptFilter.Count() > 0 && DeptFilter[0] != "null")
            {
                var deptItem = "(";
                foreach (var item in DeptFilter[0].Split(','))
                {
                    deptItem += string.Format("'{0}',", item);
                }
                deptItem = deptItem.Remove(deptItem.Length - 1, 1);
                deptItem += ")";
                deptQuery += string.Format(" and emp.Department in {0}", deptItem);
            }
            if (!string.IsNullOrEmpty(InsuranceFilter))
            {
                if (InsuranceFilter == "Yes")
                {
                    insuranceQuery = " and Insurance = 'Yes'";
                }
                else if (InsuranceFilter == "No")
                {
                    insuranceQuery = " and Insurance = 'No'";
                }

            }
            if (StatusFilter != null && StatusFilter.Count() > 0)
            {
                foreach (var item in StatusFilter)
                {
                    if (item == "1")
                    {
                        statusQuery += "and (emp.IsActive = 1 and emp.IsCurrentEmployee = 1)";
                    }
                    else if (item == "0")
                    {
                        if (!string.IsNullOrEmpty(statusQuery))
                        {
                            statusQuery += " or emp.IsActive = 0";
                        }
                        else
                        {
                            statusQuery += " and emp.IsActive = 0";
                        }

                    }
                    else if (item == "All")
                    {
                        statusQuery = "";
                    }

                }
            }
            sqlQuery = @"select emp.Id,FirstName+' '+LastName [Name],Email,Street [Address Street1],Street2 [Address Street2],City,State,ZipCode,
           Case when Department != '-1' then Department else '' End as Department,CONVERT(date, DOB) [Birthday],CONVERT(date, HireDate) [Hire Date],round(emp.PtoRate,4)  [Pto Accual Rate],
                       (select  Case when count(Id) > 0 then 'Yes' Else 'No' End as Insurance from EmployeeInsurance where UserId = emp.userid and Type != '') Insurance,
                        (select  top(1) CONVERT(date, EligibleFrom) from EmployeeInsurance where UserId = emp.userid) [Insurance Eligible],CONVERT(date, empev.LastEvaluationDate) [Last Evaluation],CONVERT(date,emp.InstallLicenseExpirationDate) [Install License Exp.]
                        ,CONVERT(date, emp.DriversLicenseExpirationDate)  [Driver License Exp.],(select sum(Amount) from EmployeeOccurences where UserId = emp.UserId  And OccurenceDate > DATEADD(year, -1, GetDate())) as Occurence,
                        CONVERT(date, emp.FireLicenseExpirationDate) [Fire License Exp.] into #tempEmployee from employee emp
                        left join EmployeeEvaluation empev on empev.UserId = emp.userid
                        where 1=1  {0} {1} {2} {3}
                        select  * into #tempEmployeeFilter from #tempEmployee where 1=1 {4}
                        Select * from #tempEmployeeFilter order by Id desc
                        drop table #tempEmployee
                        drop table #tempEmployeeFilter";
            try
            {
                sqlQuery = string.Format(sqlQuery, createddateSubQuery, searchQuery, deptQuery, statusQuery, insuranceQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetAllEmployeeInsuranceForReportDownload(DateTime? StartDate, DateTime? EndDate, string Search, string[] DeptFilter, string[] StatusFilter, string InsuranceFilter)
        {
            string sqlQuery = "";
            string createddateSubQuery = "";
            string searchQuery = "";
            string deptQuery = "";
            string statusQuery = "";
            string insuranceQuery = "";

            if (StartDate.HasValue && EndDate.HasValue && StartDate != new DateTime() && EndDate != new DateTime())
            {
                createddateSubQuery = string.Format(" and emp.CreatedDate between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());

            }
            if (!string.IsNullOrEmpty(Search))
            {
                searchQuery = string.Format(" and (emp.FirstName like '%{0}%' or emp.LastName like '%{0}%' or emp.FirstName+' '+emp.LastName like '%{0}%' or emp.Email like '%{0}%') ", Search);
            }
            if (DeptFilter != null && DeptFilter.Count() > 0 && DeptFilter[0] != "null")
            {
                var deptItem = "(";
                foreach (var item in DeptFilter[0].Split(','))
                {
                    deptItem += string.Format("'{0}',", item);
                }
                deptItem = deptItem.Remove(deptItem.Length - 1, 1);
                deptItem += ")";
                deptQuery += string.Format(" and emp.Department in {0}", deptItem);
            }
            if (!string.IsNullOrEmpty(InsuranceFilter))
            {
                if (InsuranceFilter == "Yes")
                {
                    insuranceQuery = " and Insurance = 'Yes'";
                }
                else if (InsuranceFilter == "No")
                {
                    insuranceQuery = " and Insurance = 'No'";
                }

            }
            if (StatusFilter != null && StatusFilter.Count() > 0)
            {
                foreach (var item in StatusFilter)
                {
                    if (item == "1")
                    {
                        statusQuery += "and (emp.IsActive = 1 and emp.IsCurrentEmployee = 1)";
                    }
                    else if (item == "0")
                    {
                        if (!string.IsNullOrEmpty(statusQuery))
                        {
                            statusQuery += " or emp.IsActive = 0";
                        }
                        else
                        {
                            statusQuery += " and emp.IsActive = 0";
                        }

                    }
                    else if (item == "All")
                    {
                        statusQuery = "";
                    }

                }
            }
            sqlQuery = @"select emp.Id,FirstName+' '+LastName [Name],Email,Street [Address Street1],Street2 [Address Street2],City,State,ZipCode,
           Case When Department != '-1' then Department Else '' End as Department,CONVERT(date, DOB) [Birthday],CONVERT(date, HireDate)  [Hire Date],round(emp.PtoRate,4) [Pto Accual Rate],
                       (select  Case when count(Id) > 0 then 'Yes' Else 'No' End as Insurance from EmployeeInsurance where UserId = emp.userid and Type != '') Insurance,

                        (select Case when count(Id) > 0 then 'Yes' Else 'No' End as IsMedical from employeeInsurance empins where empins.type = 'Medical' and empins.UserId = emp.UserId) [Medical],
						(select Case When empins.Subtype != '-1' then empins.Subtype Else '' End as MedicalPlan from employeeInsurance empins where empins.type = 'Medical' and empins.UserId = emp.UserId) [Medical Plan],
						(select Case When empins.RateType != '-1' then empins.RateType Else '' End as MedicalType from employeeInsurance empins where empins.type = 'Medical' and empins.UserId = emp.UserId) [Medical Type],
						(select empins.InsuranceRate from employeeInsurance empins where empins.type = 'Medical' and empins.UserId = emp.UserId) [Medical Amount],

                        (select Case when count(Id) > 0 then 'Yes' Else 'No' End as IsDental from employeeInsurance empins where empins.type = 'Dental' and empins.UserId = emp.UserId) [Dental],
                        (select Case When empins.Subtype != '-1' then empins.Subtype Else '' End as DentalPlan from employeeInsurance empins where empins.type = 'Dental' and empins.UserId = emp.UserId) [Dental Plan],
						(select Case When empins.RateType != '-1' then empins.RateType Else '' End as DentalType from employeeInsurance empins where empins.type = 'Dental' and empins.UserId = emp.UserId) [Dental Type],
						(select empins.InsuranceRate from employeeInsurance empins where empins.type = 'Dental' and empins.UserId = emp.UserId) [Dental Amount],

                        (select Case when count(Id) > 0 then 'Yes' Else 'No' End as IsVision from employeeInsurance empins where empins.type = 'Vision' and empins.UserId = emp.UserId) [Vision],
                        (select Case When empins.RateType != '-1' then empins.RateType Else '' End as VisionType from employeeInsurance empins where empins.type = 'Vision' and empins.UserId = emp.UserId) [Vision Type],
						(select empins.InsuranceRate from employeeInsurance empins where empins.type = 'Vision' and empins.UserId = emp.UserId) [Vision Amount],

                        (select Case when count(Id) > 0 then 'Yes' Else 'No' End as IsVoluntaryLife from employeeInsurance empins where empins.type = 'VoluntaryLife' and empins.UserId = emp.UserId) [IsVoluntaryLife],
						(select empins.InsuranceRate from employeeInsurance empins where empins.type = 'VoluntaryLife' and empins.UserId = emp.UserId) [VoluntaryLifeAmount],

                        (select Case when count(Id) > 0 then 'Yes' Else 'No' End as IsSTD from employeeInsurance empins where empins.type = 'ShortTermDisability' and empins.UserId = emp.UserId) [IsSTD],
                        (select empins.InsuranceRate from employeeInsurance empins where empins.type = 'ShortTermDisability' and empins.UserId = emp.UserId) [STDAmount],

                        (select Case when count(Id) > 0 then 'Yes' Else 'No' End as IsLTD from employeeInsurance empins where empins.type = 'LongTermDisability' and empins.UserId = emp.UserId) [IsLTD],
	                    (select empins.InsuranceRate from employeeInsurance empins where empins.type = 'LongTermDisability' and empins.UserId = emp.UserId) [LTDAmount]
   
                        into #tempInsurance from employee emp
                        left join EmployeeEvaluation empev on empev.UserId = emp.userid
                        where 1=1   {0} {1} {2} {3}
                           			
                        select  * into #tempInsuranceFilter from #tempInsurance where 1=1 {4}
					
                       	select * from #tempInsuranceFilter order by Id desc
                        drop table #tempInsurance
                        drop table #tempInsuranceFilter

					";
            try
            {
                sqlQuery = string.Format(sqlQuery, createddateSubQuery, searchQuery, deptQuery, statusQuery, insuranceQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetAllEmployeeForReport(DateTime? StartDate, DateTime? EndDate, string Search, string[] DeptFilter, string[] StatusFilter, string InsuranceFilter, int pageno, int pagesize, string order)
        {
            string sqlQuery = "";
            string createddateSubQuery = "";
            string searchQuery = "";
            string deptQuery = "";
            string statusQuery = "";
            string insuranceQuery = "";
            string orderquery = "";
            string orderquery1 = "";
            if (StartDate.HasValue && EndDate.HasValue && StartDate != new DateTime() && EndDate != new DateTime())
            {
                createddateSubQuery = string.Format(" and emp.CreatedDate between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());

            }
            if (!string.IsNullOrEmpty(Search))
            {
                searchQuery = string.Format(" and (emp.FirstName like '%{0}%' or emp.LastName like '%{0}%' or emp.FirstName+' '+emp.LastName like '%{0}%' or emp.Email like '%{0}%') ", Search);
            }
            if (DeptFilter != null && DeptFilter.Count() > 0 && DeptFilter[0] != "null")
            {
                var deptItem = "(";
                foreach (var item in DeptFilter[0].Split(','))
                {
                    deptItem += string.Format("'{0}',", item);
                }
                deptItem = deptItem.Remove(deptItem.Length - 1, 1);
                deptItem += ")";
                deptQuery += string.Format(" and emp.Department in {0}", deptItem);
            }
            if (!string.IsNullOrEmpty(InsuranceFilter))
            {
                if (InsuranceFilter == "Yes")
                {
                    insuranceQuery = " and Insurance = 'Yes'";
                }
                else if (InsuranceFilter == "No")
                {
                    insuranceQuery = " and Insurance = 'No'";
                }

            }
            if (StatusFilter != null && StatusFilter.Count() > 0)
            {
                foreach (var item in StatusFilter)
                {
                    if (item == "1")
                    {
                        statusQuery += "and (emp.IsActive = 1 and emp.IsCurrentEmployee = 1)";
                    }
                    else if (item == "0")
                    {
                        if (!string.IsNullOrEmpty(statusQuery))
                        {
                            statusQuery += " or emp.IsActive = 0";
                        }
                        else
                        {
                            statusQuery += " and emp.IsActive = 0";
                        }

                    }
                    else if (item == "All")
                    {
                        statusQuery = "";
                    }

                }
            }
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/name")
                {
                    orderquery = "order by #cd.[FirstName] asc";
                    orderquery1 = "order by [FirstName] asc";
                }
                else if (order == "descending/name")
                {
                    orderquery = "order by #cd.[FirstName] desc";
                    orderquery1 = "order by [FirstName] desc";
                }
                else if (order == "ascending/adress")
                {
                    orderquery = "order by #cd.City asc";
                    orderquery1 = "order by City asc";
                }
                else if (order == "descending/adress")
                {
                    orderquery = "order by #cd.City desc";
                    orderquery1 = "order by City desc";
                }
                else if (order == "ascending/hiredate")
                {
                    orderquery = "order by #cd.[HireDate] asc";
                    orderquery1 = "order by [HireDate] asc";
                }
                else if (order == "descending/hiredate")
                {
                    orderquery = "order by #cd.[HireDate] desc";
                    orderquery1 = "order by [HireDate] desc";
                }
                else if (order == "ascending/insurance")
                {
                    orderquery = "order by #cd.[Insurance] asc";
                    orderquery1 = "order by [Insurance] asc";
                }
                else if (order == "descending/insurance")
                {
                    orderquery = "order by #cd.[Insurance] desc";
                    orderquery1 = "order by [Insurance] desc";
                }
                else if (order == "ascending/insuranceeligable")
                {
                    orderquery = "order by #cd.[EligibleFrom] asc";
                    orderquery1 = "order by [EligibleFrom] asc";
                }
                else if (order == "descending/insuranceeligable")
                {
                    orderquery = "order by #cd.[EligibleFrom] desc";
                    orderquery1 = "order by [EligibleFrom] desc";
                }
                else if (order == "ascending/lastevaluation")
                {
                    orderquery = "order by #cd.[LastEvaluationDate] asc";
                    orderquery1 = "order by [LastEvaluationDate] asc";
                }
                else if (order == "descending/lastevaluation")
                {
                    orderquery = "order by #cd.[LastEvaluationDate] desc";
                    orderquery1 = "order by [LastEvaluationDate] desc";
                }
                else if (order == "ascending/nextevaluation")
                {
                    orderquery = "order by #cd.[NextEvaluationDate] asc";
                    orderquery1 = "order by [NextEvaluationDate] asc";
                }
                else if (order == "descending/nextevaluation")
                {
                    orderquery = "order by #cd.[NextEvaluationDate] desc";
                    orderquery1 = "order by [NextEvaluationDate] desc";
                }

                else if (order == "ascending/medical")
                {
                    orderquery = "order by #cd.[IsMedical] asc";
                    orderquery1 = "order by [IsMedical] asc";
                }
                else if (order == "descending/medical")
                {
                    orderquery = "order by #cd.[IsMedical] desc";
                    orderquery1 = "order by [IsMedical] desc";
                }

                else if (order == "ascending/voluntary")
                {
                    orderquery = "order by #cd.[IsVoluntaryLife] asc";
                    orderquery1 = "order by [IsVoluntaryLife] asc";
                }
                else if (order == "descending/voluntary")
                {
                    orderquery = "order by #cd.[IsVoluntaryLife] desc";
                    orderquery1 = "order by [IsVoluntaryLife] desc";
                }

                else
                {
                    orderquery = "order by #cd.[Id]  desc";
                    orderquery1 = "order by Id desc";
                }

            }
            else
            {
                orderquery = "order by #cd.[Id] desc";
                orderquery1 = "order by Id desc";
            }
            #endregion
            sqlQuery = @"declare @pagestart int
                        declare @pageend int
                        set @pagestart=(@pageno-1)* @pagesize 
                        set @pageend = @pagesize

                        select emp.Id,ul.Id as UserIntId,FirstName,LastName,Email,Street,Street2,City,State,ZipCode,Department,DOB,HireDate,emp.PtoRate
                        ,empev.LastEvaluationDate,empev.NextEvaluationDate,emp.InstallLicenseExpirationDate
                        ,emp.DriversLicenseExpirationDate,(select sum(Amount) from EmployeeOccurences where UserId = emp.UserId) as Occurence,
                        emp.FireLicenseExpirationDate,

                        (select Case when count(Id) > 0 then 'Yes' Else 'No' End as IsMedical from employeeInsurance empins where empins.type = 'Medical' and empins.UserId = emp.UserId) [IsMedical],
                        (select Case when count(Id) > 0 then 'Yes' Else 'No' End as IsDental from employeeInsurance empins where empins.type = 'Dental' and empins.UserId = emp.UserId) [IsDental],
                        (select Case when count(Id) > 0 then 'Yes' Else 'No' End as IsVision from employeeInsurance empins where empins.type = 'Vision' and empins.UserId = emp.UserId) [IsVision],
                        (select Case when count(Id) > 0 then 'Yes' Else 'No' End as IsVoluntaryLife from employeeInsurance empins where empins.type = 'VoluntaryLife' and empins.UserId = emp.UserId) [IsVoluntaryLife],
                        (select Case when count(Id) > 0 then 'Yes' Else 'No' End as IsSTD from employeeInsurance empins where empins.type = 'ShortTermDisability' and empins.UserId = emp.UserId) [IsSTD],
                        (select Case when count(Id) > 0 then 'Yes' Else 'No' End as IsLTD from employeeInsurance empins where empins.type = 'LongTermDisability' and empins.UserId = emp.UserId) [IsLTD],

                        (select  Case when count(Id) > 0 then 'Yes' Else 'No' End as Insurance from EmployeeInsurance where UserId = emp.userid and Type != '') Insurance,
                        (select  top(1) EligibleFrom from EmployeeInsurance where UserId = emp.userid) EligibleFrom,
						(select empins.Subtype from employeeInsurance empins where empins.type = 'Medical' and empins.UserId = emp.UserId) [MedicalPlan],
						(select empins.RateType from employeeInsurance empins where empins.type = 'Medical' and empins.UserId = emp.UserId) [MedicalType],
						(select empins.InsuranceRate from employeeInsurance empins where empins.type = 'Medical' and empins.UserId = emp.UserId) [MedicalAmount],



						(select empins.Subtype from employeeInsurance empins where empins.type = 'Dental' and empins.UserId = emp.UserId) [DentalPlan],
						(select empins.RateType from employeeInsurance empins where empins.type = 'Dental' and empins.UserId = emp.UserId) [DentalType],
						(select empins.InsuranceRate from employeeInsurance empins where empins.type = 'Dental' and empins.UserId = emp.UserId) [DentalAmount],

						(select empins.RateType from employeeInsurance empins where empins.type = 'Vision' and empins.UserId = emp.UserId) [VisionType],
						(select empins.InsuranceRate from employeeInsurance empins where empins.type = 'Vision' and empins.UserId = emp.UserId) [VisionAmount],

						(select empins.InsuranceRate from employeeInsurance empins where empins.type = 'VoluntaryLife' and empins.UserId = emp.UserId) [VoluntaryLifeAmount],

						(select empins.InsuranceRate from employeeInsurance empins where empins.type = 'ShortTermDisability' and empins.UserId = emp.UserId) [STDAmount],

						(select empins.InsuranceRate from employeeInsurance empins where empins.type = 'LongTermDisability' and empins.UserId = emp.UserId) [LTDAmount]
                    

                       into #tempEmployee from employee emp
                       left join EmployeeEvaluation empev on empev.UserId = emp.userid
                       left join UserLogin ul on ul.UserId = emp.UserId
                        where 1=1 {0} {1} {2} {3} 
                         select top(@pagesize) * from #tempEmployee
                        where Id not in (Select TOP (@pagestart)  Id from #tempEmployee #cd {5}) {6} {4}
                        --order by Id desc
                    
                        select COUNT(*) TotalEmployee from #tempEmployee where 1=1 {4}
                        drop table #tempEmployee";
            try
            {
                sqlQuery = string.Format(sqlQuery, createddateSubQuery, searchQuery, deptQuery, statusQuery, insuranceQuery, orderquery, orderquery1);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetFirsCallCloseDownload(DateTime? StartDate, DateTime? EndDate)
        {
            string sqlQuery = @"";
            string createddateSubQuery = "";
            string joindateSubQuery = "";
            string orderquery = "";
            string orderquery1 = "";
            if (StartDate.HasValue && EndDate.HasValue && StartDate != new DateTime() && EndDate != new DateTime())
            {
                createddateSubQuery = string.Format(" and cus.CreatedDate between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
                joindateSubQuery = string.Format(" and cus.SalesDate between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
            }


            sqlQuery = @"

                       

                        select cus.Id, cus.CustomerId, cus.Soldby1,cus.Status, CE.AppoinmentSetBy,ccclosing.IsLead,cus.SalesDate,CE.CreatedDay,cus.CreatedDate
						into #TempCustomer
						from customer cus 
						left join CustomerExtended CE on CE.Customerid = cus.Customerid
						LEFT JOIN CustomerCompany ccclosing on ccclosing.CustomerId=cus.CustomerId
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=cus.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText !='' and CE.IsTestAccount != 1
						
						select #tc.* into #TempCustomerBad
						From #TempCustomer #tc
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=#tc.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText = 'Bad' 

						select #tc.* into #TempCustomerGood
						From #TempCustomer #tc
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=#tc.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText = 'Good' 

						select 
						emp.Id,
						emp.FirstName +' '+ emp.LastName as [Sales Person],

						(Select COUNT(cus.Id) from #TempCustomer cus
						where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId)  {0}) TotalLeads,

						(Select COUNT(cus.Id) from #TempCustomerBad cus
						where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId)  {0}) Removed,

						(Select COUNT(cus.Id) from #TempCustomerGood cus
						where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId)  {0}) GoodLeads,

						(Select COUNT(cus.Id) from #TempCustomer cus 
						where (cus.Soldby1=emp.UserId) and CAST(cus.CreatedDay as date) = CAST(cus.SalesDate as date) and cus.IsLead=0  {1}) [First Call Close],

						ISNULL(((Select COUNT(cus.Id) from #TempCustomer cus 
						where cus.Soldby1=emp.UserId and CAST(cus.CreatedDay as date) = CAST(cus.SalesDate as date) and cus.IsLead=0   {1}) * 100.0 /
						 NULLIF((Select COUNT(cus.Id) from #TempCustomerGood cus 
						 where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId )  {0}), 0)),0) as [Closing],

						(Select UserX from SalesMatrix sm where sm.Type='FirstCallCosingPecentage' and 
						ISNULL((Select COUNT(cus.Id) from #TempCustomer cus
						 where cus.Soldby1=emp.UserId and CAST(cus.CreatedDay as date) = CAST(cus.SalesDate as date) and cus.IsLead=0   {1})* 100.0 /
						(NULLIF((Select COUNT(cus.Id) from #TempCustomerGood cus
						where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId )  {0}), 0)),0) between sm.Min and sm.Max
						) as UserX
						 into #employeeData From Employee emp
						left join UserPermission up on up.UserId = emp.UserId
                        left join UserLogin u on u.UserId=up.UserId
						left join PermissionGroup pg on pg.Id = up.PermissionGroupId
						where emp.IsSalesMatrix=1 
                        
                        select empdata.* into #employeeDataFinal
						from #employeeData empdata
						where empdata.TotalLeads>0

                        select  [Sales Person],TotalLeads,Removed,GoodLeads,[First Call Close],cast(Closing as decimal(10,2)) as [Closing %],cast(UserX as decimal(10,2)) as [UserX] from #employeeDataFinal
                  
                        order by Id desc
                        
                        select COUNT(*) TotalEmployee from #employeeDataFinal

                        drop table #employeeData
                        drop table #employeeDataFinal
						drop table #TempCustomer
						
                        drop table #TempCustomerBad
						drop table #TempCustomerGood";
            try
            {
                sqlQuery = string.Format(sqlQuery, createddateSubQuery, joindateSubQuery, orderquery, orderquery1);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetFirsCallCloseCustomerDownload(DateTime? StartDate, DateTime? EndDate, Guid EmployeeId, string from)
        {
            string sqlQuery = @"";
            string createddateSubQuery = "";
            string joindateSubQuery = "";
            if (StartDate.HasValue && EndDate.HasValue && StartDate != new DateTime() && EndDate != new DateTime())
            {
                createddateSubQuery = string.Format(" and ce.CreatedDay between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
                joindateSubQuery = string.Format(" and cus.SalesDate between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
            }
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }
            var LeadQuery = " and ccclosing.IsLead=0";

            var ConvertQuery = "";
            var SoldByQuery = "";
            if (from == "AppointmentCustomer" || from == "ApptSet")
            {
                SoldByQuery = string.Format(" lkLStatus.AlterDisplayText!='' and ce.AppoinmentSetBy = '{0}'", EmployeeId);
                LeadQuery = "";
                //createddateSubQuery = joindateSubQuery;
            }
            else if (from == "AssignTo")
            {
                SoldByQuery = string.Format("lkLStatus.AlterDisplayText!='Bad' and cus.Soldby1 = CONVERT(nvarchar(50), '{0}')", EmployeeId);
                LeadQuery = "";
            }

            else
            {
                SoldByQuery = string.Format("lkLStatus.AlterDisplayText!='Bad' and (cus.Soldby = CONVERT(nvarchar(50), '{0}') or ce.AppoinmentSetBy='{0}')", EmployeeId);
            }
            if (from == "FirstCallClose")
            {
                SoldByQuery = string.Format("(cus.Soldby1 = CONVERT(nvarchar(50), '{0}')) and lkLStatus.AlterDisplayText !=''", EmployeeId);

                LeadQuery = "and  CAST(ce.CreatedDay as date) = CAST(cus.SalesDate as date) and ccclosing.IsLead=0";
                createddateSubQuery = joindateSubQuery;
            }
            if (from == "OverallClose")
            {
                SoldByQuery = string.Format("(cus.Soldby1 = CONVERT(nvarchar(50), '{0}')) and lkLStatus.AlterDisplayText !=''", EmployeeId);
                createddateSubQuery = joindateSubQuery;
            }
            if (from == "TotalLead")
            {
                SoldByQuery = string.Format("(ce.AppoinmentSetBy = '{0}' or cus.Soldby1 = CONVERT(nvarchar(50), '{0}')) and lkLStatus.AlterDisplayText !=''", EmployeeId);
                LeadQuery = "";
            }
            if (from == "BadLead")
            {
                SoldByQuery = string.Format("(ce.AppoinmentSetBy = '{0}' or cus.Soldby1 = CONVERT(nvarchar(50), '{0}')) and lkLStatus.AlterDisplayText ='Bad'", EmployeeId);
                LeadQuery = "";
            }
            if (from == "GoodLead")
            {
                SoldByQuery = string.Format("(ce.AppoinmentSetBy = '{0}' or cus.Soldby1 = CONVERT(nvarchar(50), '{0}')) and lkLStatus.AlterDisplayText ='Good'", EmployeeId);
                LeadQuery = "";
            }
            else if (from == "CustomerFunded")
            {
                SoldByQuery = string.Format("(cus.Soldby1 = CONVERT(nvarchar(50), '{0}')) and lkLStatus.AlterDisplayText !=''", EmployeeId);

                LeadQuery = " and ccclosing.IsLead=0";
                if (StartDate.HasValue && EndDate.HasValue && StartDate != new DateTime() && EndDate != new DateTime())
                {
                    joindateSubQuery = string.Format(" and cus.CustomerFundedDate between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
                }
                createddateSubQuery = joindateSubQuery;
                ConvertQuery = " and cus.CustomerFunded=1";
            }
            sqlQuery = @"
                        Select cus.Id,{0} as [Customer Name], cus.CustomerNo as [Cs Number], lk.DisplayText as [Lead Source],lkLStatus.DisplayText as [Lead Status], iif(cus.AppoinmentSet != '' and cus.AppoinmentSet != '-1',cus.AppoinmentSet,'No') as [Appointment Set], iif(convert(date,cus.CreatedDate) is not null and convert(date,cus.CreatedDate) != '' and convert(date,ccclosing.ConvertionDate) is not null and convert(date,ccclosing.ConvertionDate) != '' and convert(date,cus.CreatedDate) = convert(date,ccclosing.ConvertionDate), 'Yes', 'No') as [First Call Close], iif(cus.MonthlyMonitoringFee is not null and cus.MonthlyMonitoringFee != '',cus.MonthlyMonitoringFee, '0.00') as [RMR] ,convert(date,cus.SalesDate) as [Sale Date], convert(date,cus.CreatedDate) as [Created Date]
                        into #CusData from Customer cus
						LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                        LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=cus.Status and lkLStatus.DataKey='LeadStatus'
						LEFT JOIN CustomerCompany ccclosing on ccclosing.CustomerId=cus.CustomerId
                        left join Lookup  lk on lk.DataValue = cus.LeadSource and lk.DataKey = 'LeadSource'
						where ce.IsTestAccount != 1 and {3} {2}{1}{4}
					    select * from #CusData
                        order by Id desc
                        drop table #CusData";
            try
            {
                sqlQuery = string.Format(sqlQuery, NameSql, LeadQuery, ConvertQuery, SoldByQuery, createddateSubQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    //AddParameter(cmd, pInt32("pageno", pageno));
                    //AddParameter(cmd, pInt32("pagesize", pagesize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetFirsCallCustomerData(DateTime? StartDate, DateTime? EndDate, Guid EmployeeId, int pageno, int pagesize)
        {
            string sqlQuery = @"";
            string createddateSubQuery = "";
            string joindateSubQuery = "";
            if (StartDate.HasValue && EndDate.HasValue && StartDate != new DateTime() && EndDate != new DateTime())
            {
                createddateSubQuery = string.Format(" and cus.CreatedDay between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
                joindateSubQuery = string.Format(" and cus.SalesDate between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
            }
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }

            sqlQuery = @"declare @pagestart int
                        declare @pageend int
                        set @pagestart=(@pageno-1)* @pagesize 
                        set @pageend = @pagesize

						
                        Select cus.Id,{0} as CustomerName,cus.SalesDate,cus.CustomerNo,lk.DisplayText as LeadSourceVal,ce.CreatedDay,cus.AppoinmentSet,cus.MonthlyMonitoringFee 
                        into #CusData from Customer cus
                        LEFT JOIN CustomerCompany ccclosing on ccclosing.CustomerId=cus.CustomerId
                        LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=cus.Status and lkLStatus.DataKey='LeadStatus'
	                    left join customerextended ce on ce.customerid = cus.customerid
                        left join Lookup lk on lk.DataValue = cus.LeadSource and lk.DataKey = 'LeadSource'
                        where ce.IsTestAccount != 1 and lkLStatus.AlterDisplayText!='Bad' and (cus.Soldby=CONVERT(nvarchar(50), '{1}') or ce.AppoinmentSetBy = '{1}'){2}
					    select top(@pagesize) * from #CusData
                        where Id not in (Select TOP (@pagestart)  Id from #CusData #cd order by #cd.Id desc)
                        order by Id desc
                        select COUNT(*) TotalCustomer from #CusData
                        drop table #CusData";
            try
            {
                sqlQuery = string.Format(sqlQuery, NameSql, EmployeeId, createddateSubQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetFirsCallCustomerDataDownload(DateTime? StartDate, DateTime? EndDate, Guid EmployeeId)
        {
            string sqlQuery = @"";
            string createddateSubQuery = "";
            string joindateSubQuery = "";
            if (StartDate.HasValue && EndDate.HasValue && StartDate != new DateTime() && EndDate != new DateTime())
            {
                createddateSubQuery = string.Format(" and cus.CreatedDay between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
                joindateSubQuery = string.Format(" and cus.SalesDate between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
            }
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }

            sqlQuery = @"
                        Select cus.Id,{0} as [Customer Name], cus.CustomerNo as [Cs Number], lk.DisplayText as [Lead Source], iif(cus.AppoinmentSet != '' and cus.AppoinmentSet != '-1',cus.AppoinmentSet,'No') as [Appointment Set], iif(convert(date,cus.CreatedDate) is not null and convert(date,cus.CreatedDate) != '' and convert(date,ccclosing.ConvertionDate) is not null and convert(date,ccclosing.ConvertionDate) != '' and convert(date,cus.CreatedDate) = convert(date,ccclosing.ConvertionDate), 'Yes', 'No') as [First Call Close], iif(cus.MonthlyMonitoringFee is not null and cus.MonthlyMonitoringFee != '',cus.MonthlyMonitoringFee, '0.00') as [RMR] ,convert(date,cus.SalesDate) as [Sale Date], convert(date,cus.CreatedDate) as [Created Date]
                        into #CusData from Customer cus
                        LEFT JOIN CustomerCompany ccclosing on ccclosing.CustomerId=cus.CustomerId
                        LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=cus.Status and lkLStatus.DataKey='LeadStatus'
	                    left join customerextended ce on ce.customerid = cus.customerid
                        left join Lookup lk on lk.DataValue = cus.LeadSource and lk.DataKey = 'LeadSource'
                        where ce.IsTestAccount != 1 and lkLStatus.AlterDisplayText!='Bad' and (cus.Soldby=CONVERT(nvarchar(50), '{1}') or ce.AppoinmentSetBy = '{1}'){2}
					    select * from #CusData
                        order by Id desc
                        drop table #CusData";
            try
            {
                sqlQuery = string.Format(sqlQuery, NameSql, EmployeeId, createddateSubQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    //AddParameter(cmd, pInt32("pageno", pageno));
                    //AddParameter(cmd, pInt32("pagesize", pagesize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetFirsCallCloseCustomerData(DateTime? StartDate, DateTime? EndDate, Guid EmployeeId, int pageno, int pagesize, string from)
        {
            string sqlQuery = @"";
            string createddateSubQuery = "";
            string joindateSubQuery = "";
            if (StartDate.HasValue && EndDate.HasValue && StartDate != new DateTime() && EndDate != new DateTime())
            {
                createddateSubQuery = string.Format(" and ce.CreatedDay between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
                joindateSubQuery = string.Format(" and cus.SalesDate between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
            }
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }
            var LeadQuery = " and ccclosing.IsLead=0";

            var ConvertQuery = "";
            var SoldByQuery = "";
            if (from == "AppointmentCustomer" || from == "ApptSet")
            {
                SoldByQuery = string.Format(" lkLStatus.AlterDisplayText!='' and ce.AppoinmentSetBy = '{0}'", EmployeeId);
                LeadQuery = "";
                //createddateSubQuery = joindateSubQuery;
            }
            else if (from == "AssignTo")
            {
                SoldByQuery = string.Format("lkLStatus.AlterDisplayText!='Bad' and cus.Soldby1 = CONVERT(nvarchar(50), '{0}')", EmployeeId);
                LeadQuery = "";
            }

            if (from == "FirstCallClose")
            {
                SoldByQuery = string.Format("(cus.Soldby1 = CONVERT(nvarchar(50), '{0}')) and lkLStatus.AlterDisplayText !=''", EmployeeId);

                LeadQuery = "and  CAST(ce.CreatedDay as date) = CAST(cus.SalesDate as date) and ccclosing.IsLead=0";
                createddateSubQuery = joindateSubQuery;
            }
            if (from == "OverallClose")
            {
                SoldByQuery = string.Format("(cus.Soldby1 = CONVERT(nvarchar(50), '{0}')) and lkLStatus.AlterDisplayText !=''", EmployeeId);
                createddateSubQuery = joindateSubQuery;
            }
            if (from == "TotalLead")
            {
                SoldByQuery = string.Format("(ce.AppoinmentSetBy = '{0}' or cus.Soldby1 = CONVERT(nvarchar(50), '{0}')) and lkLStatus.AlterDisplayText !=''", EmployeeId);
                LeadQuery = "";
            }
            if (from == "BadLead")
            {
                SoldByQuery = string.Format("(ce.AppoinmentSetBy = '{0}' or cus.Soldby1 = CONVERT(nvarchar(50), '{0}')) and lkLStatus.AlterDisplayText ='Bad'", EmployeeId);
                LeadQuery = "";
            }
            if (from == "GoodLead")
            {
                SoldByQuery = string.Format("(ce.AppoinmentSetBy = '{0}' or cus.Soldby1 = CONVERT(nvarchar(50), '{0}')) and lkLStatus.AlterDisplayText ='Good'", EmployeeId);
                LeadQuery = "";
            }
            else if (from == "CustomerFunded")
            {
                SoldByQuery = string.Format("(cus.Soldby1 = CONVERT(nvarchar(50), '{0}')) and lkLStatus.AlterDisplayText !=''", EmployeeId);

                LeadQuery = " and ccclosing.IsLead=0";
                if (StartDate.HasValue && EndDate.HasValue && StartDate != new DateTime() && EndDate != new DateTime())
                {
                    joindateSubQuery = string.Format(" and cus.CustomerFundedDate between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
                }
                createddateSubQuery = joindateSubQuery;
                ConvertQuery = " and cus.CustomerFunded=1";
            }
            sqlQuery = @"declare @pagestart int
                        declare @pageend int
                        set @pagestart=(@pageno-1)* @pagesize 
                        set @pageend = @pagesize

						
                        Select cus.Id,{0} as CustomerName,cus.SalesDate,cus.CustomerFundedDate,cus.CustomerNo,lk.DisplayText as LeadSourceVal,lkLStatus.DisplayText as LeadStatusVal,ce.CreatedDay,cus.AppoinmentSet,cus.MonthlyMonitoringFee
                        into #CusData from Customer cus
						LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                        LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=cus.Status and lkLStatus.DataKey='LeadStatus'
						LEFT JOIN CustomerCompany ccclosing on ccclosing.CustomerId=cus.CustomerId
                        left join Lookup  lk on lk.DataValue = cus.LeadSource and lk.DataKey = 'LeadSource'
						where ce.IsTestAccount != 1 and {3} {2}{1}{4}
					    select top(@pagesize) * from #CusData
                        where Id not in (Select TOP (@pagestart)  Id from #CusData #cd order by #cd.Id desc)
                        order by Id desc
                        select COUNT(*) TotalCustomer from #CusData
                        drop table #CusData";
            try
            {
                sqlQuery = string.Format(sqlQuery, NameSql, LeadQuery, ConvertQuery, SoldByQuery, createddateSubQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataSet GetSoldFundedMatrixCustomerData(DateTime? StartDate, DateTime? EndDate, Guid EmployeeId, int pageno, int pagesize, string from)
        {
            string sqlQuery = @"";
            string createddateSubQuery = "";
            string joindateSubQuery = "";
            if (StartDate.HasValue && EndDate.HasValue && StartDate != new DateTime() && EndDate != new DateTime())
            {
                createddateSubQuery = string.Format(" and ex.CreatedDay between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
                joindateSubQuery = string.Format(" and cus.SalesDate between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
            }
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }
            var LeadQuery = "";
            var appQuery = "";
            if (from == "Appointment")
            {
                LeadQuery = " and ccclosing.IsLead=1 and lkLStatus.AlterDisplayText ='Good'";
                appQuery = string.Format(" or ex.AppoinmentSetBy='{0}'", EmployeeId);
            }
            else
            {
                LeadQuery = "  and ccclosing.IsLead=0 and lkLStatus.AlterDisplayText!=''";
                createddateSubQuery = joindateSubQuery;

            }

            sqlQuery = @"declare @pagestart int
                        declare @pageend int
                        set @pagestart=(@pageno-1)* @pagesize 
                        set @pageend = @pagesize

						
                        Select cus.Id,{0} as CustomerName,cus.SalesDate,cus.CustomerNo,lk.DisplayText as LeadSourceVal,lkLStatus.DisplayText as LeadStatusVal,ex.CreatedDay,cus.AppoinmentSet,cus.MonthlyMonitoringFee 
                        into #CusData from Customer cus
                         LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=cus.Status and lkLStatus.DataKey='LeadStatus'
                        LEFT JOIN CustomerCompany ccclosing on ccclosing.CustomerId=cus.CustomerId
                        left join Lookup lk on lk.DataValue = cus.LeadSource and lk.DataKey = 'LeadSource'
						left join CustomerExtended ex on ex.CustomerId = cus.CustomerId
                        where ex.IsTestAccount != 1 and (cus.Soldby=CONVERT(nvarchar(50), '{1}') {3}) {2}{4}
					    select top(@pagesize) * from #CusData
                        where Id not in (Select TOP (@pagestart)  Id from #CusData #cd order by #cd.Id desc)
                        order by Id desc
                        select COUNT(*) TotalCustomer from #CusData
                        drop table #CusData";
            try
            {
                sqlQuery = string.Format(sqlQuery, NameSql, EmployeeId, LeadQuery, appQuery, createddateSubQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetSoldFundedMatrixCustomerDataDownload(DateTime? StartDate, DateTime? EndDate, Guid EmployeeId, string from)
        {
            string sqlQuery = @"";
            string createddateSubQuery = "";
            string joindateSubQuery = "";
            if (StartDate.HasValue && EndDate.HasValue && StartDate != new DateTime() && EndDate != new DateTime())
            {
                createddateSubQuery = string.Format(" and ex.CreatedDay between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
                joindateSubQuery = string.Format(" and cus.SalesDate between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
            }
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }
            var LeadQuery = "";
            var appQuery = "";
            if (from == "Appointment")
            {
                LeadQuery = " and ccclosing.IsLead=1 and lkLStatus.AlterDisplayText ='Good'";
                appQuery = string.Format(" or ex.AppoinmentSetBy='{0}'", EmployeeId);
            }
            else
            {
                LeadQuery = "  and ccclosing.IsLead=0 and lkLStatus.AlterDisplayText!=''";
                if (from != "SoldFunded")
                {
                    createddateSubQuery = joindateSubQuery;
                }
            }

            sqlQuery = @"
                        Select cus.Id,{0} as [Customer Name], cus.CustomerNo as [Cs Number], lk.DisplayText as [Lead Source],lkLStatus.DisplayText as [Lead Status], iif(cus.AppoinmentSet != '' and cus.AppoinmentSet != '-1',cus.AppoinmentSet,'No') as [Appointment Set], iif(convert(date,cus.CreatedDate) is not null and convert(date,cus.CreatedDate) != '' and convert(date,ccclosing.ConvertionDate) is not null and convert(date,ccclosing.ConvertionDate) != '' and convert(date,cus.CreatedDate) = convert(date,ccclosing.ConvertionDate), 'Yes', 'No') as [First Call Close], iif(cus.MonthlyMonitoringFee is not null and cus.MonthlyMonitoringFee != '',cus.MonthlyMonitoringFee, '0.00') as [RMR] ,convert(date,cus.SalesDate) as [Sale Date], convert(date,cus.CreatedDate) as [Created Date] 
                        into #CusData from Customer cus
                         LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=cus.Status and lkLStatus.DataKey='LeadStatus'
                        LEFT JOIN CustomerCompany ccclosing on ccclosing.CustomerId=cus.CustomerId
                         left join Lookup lk on lk.DataValue = cus.LeadSource and lk.DataKey = 'LeadSource'
						left join CustomerExtended ex on ex.CustomerId = cus.CustomerId
                        where ex.IsTestAccount != 1 and lkLStatus.AlterDisplayText!='' and (cus.Soldby=CONVERT(nvarchar(50), '{1}') {3}) {2}{4}
					    select * from #CusData
                        order by Id desc
                        drop table #CusData";
            try
            {
                sqlQuery = string.Format(sqlQuery, NameSql, EmployeeId, LeadQuery, appQuery, createddateSubQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    //AddParameter(cmd, pInt32("pageno", pageno));
                    //AddParameter(cmd, pInt32("pagesize", pagesize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetOverAllCloseMatrixWithCountNew(DateTime? StartDate, DateTime? EndDate, int pageno, int pagesize, string order)
        {
            string sqlQuery = @"";
            string createddateSubQuery = "";
            string joindateSubQuery = "";
            string orderquery = "";
            string orderquery1 = "";
            if (StartDate.HasValue && EndDate.HasValue && StartDate != new DateTime() && EndDate != new DateTime())
            {
                //createddateSubQuery = string.Format(" and cus.CreatedDay between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
                //joindateSubQuery = string.Format(" and cus.SalesDate between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());

                createddateSubQuery = string.Format(" and cus.createdDate between '{0}' and '{1}'", StartDate.Value.SetClientZeroHourToUTC().ToString("yyyy-MM-dd HH:mm:ss"), EndDate.Value.SetClientMaxHourToUTC().ToString("yyyy-MM-dd HH:mm:ss"));
                joindateSubQuery = string.Format(" and cus.SalesDate between '{0}' and '{1}'", StartDate.Value.SetClientZeroHourToUTC().ToString("yyyy-MM-dd HH:mm:ss"), EndDate.Value.SetClientMaxHourToUTC().ToString("yyyy-MM-dd HH:mm:ss"));

                 


            }
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/salesperson")
                {
                    orderquery = "order by #cd.[EmployeeName] asc";
                    orderquery1 = "order by [EmployeeName] asc";
                }
                else if (order == "descending/salesperson")
                {
                    orderquery = "order by #cd.[EmployeeName] desc";
                    orderquery1 = "order by [EmployeeName] desc";
                }
                else if (order == "ascending/leads")
                {
                    orderquery = "order by #cd.NoOfLeads asc";
                    orderquery1 = "order by NoOfLeads asc";
                }
                else if (order == "descending/leads")
                {
                    orderquery = "order by #cd.NoOfLeads desc";
                    orderquery1 = "order by NoOfLeads desc";
                }
                else if (order == "ascending/closed")
                {
                    orderquery = "order by #cd.[Closing] asc";
                    orderquery1 = "order by [Closing] asc";
                }
                else if (order == "descending/closed")
                {
                    orderquery = "order by #cd.[Closing] desc";
                    orderquery1 = "order by [Closing] desc";
                }
                else if (order == "ascending/closing")
                {
                    orderquery = "order by #cd.[Percentage] asc";
                    orderquery1 = "order by [Percentage] asc";
                }
                else if (order == "descending/closing")
                {
                    orderquery = "order by #cd.[Percentage] desc";
                    orderquery1 = "order by [Percentage] desc";
                }
                else if (order == "ascending/userx")
                {
                    orderquery = "order by #cd.[UserX] asc";
                    orderquery1 = "order by [UserX] asc";
                }
                else if (order == "descending/userx")
                {
                    orderquery = "order by #cd.[UserX] desc";
                    orderquery1 = "order by [UserX] desc";
                }



                else
                {
                    orderquery = "order by #cd.[Id]  desc";
                    orderquery1 = "order by Id desc";
                }

            }
            else
            {
                orderquery = "order by #cd.[Id] desc";
                orderquery1 = "order by Id desc";
            }
            #endregion
            //      sqlQuery = @"declare @pagestart int
            //                  declare @pageend int
            //                  set @pagestart=(@pageno-1)* @pagesize 
            //                  set @pageend = @pagesize

            //                select cus.Id, cus.CustomerId, cus.Soldby1,cus.Status, CE.AppoinmentSetBy,ccclosing.IsLead,CE.CreatedDay,cus.SalesDate
            //into #TempCustomer
            //from customer cus 
            //left join CustomerExtended CE on CE.Customerid = cus.Customerid
            //LEFT JOIN CustomerCompany ccclosing on ccclosing.CustomerId=cus.CustomerId  and ccclosing.IsActive=1
            //LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=cus.Status and lkLStatus.DataKey='LeadStatus'
            //where cus.IsActive = 1 and cus.JoinDate is not null

            //select #tc.* into #TempCustomerBad
            //From #TempCustomer #tc
            //LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=#tc.Status and lkLStatus.DataKey='LeadStatus'
            //where lkLStatus.AlterDisplayText = 'Bad' 

            //select #tc.* into #TempCustomerGood
            //From #TempCustomer #tc
            //LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=#tc.Status and lkLStatus.DataKey='LeadStatus'
            //where lkLStatus.AlterDisplayText = 'Good' 

            //select 
            //emp.Id,u.Id as UserLoginId,emp.UserId as EmpId,

            //emp.FirstName +' '+ emp.LastName as EmployeeName,

            //                  (Select COUNT(cus.Id) from #TempCustomer cus
            //where    cus.IsLead=1 and (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId) {0}) TotalLeads,

            //(Select COUNT(cus.Id) from #TempCustomerBad cus
            //where    cus.IsLead=1 and (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId) {0}) BadLeads,

            //(Select COUNT(cus.Id) from #TempCustomerGood cus
            //where    cus.IsLead=1 and (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId) {0}) GoodLeads,

            //(Select COUNT(cus.Id) from #TempCustomer cus 
            //where (cus.Soldby1=emp.UserId) and cus.IsLead=0 {1}) Closing,

            //ISNULL(((Select COUNT(cus.Id) from #TempCustomer cus 
            //where (cus.Soldby1=emp.UserId) and cus.IsLead=0 {1})* 100.0 /
            // NULLIF((Select COUNT(cus.Id) from #TempCustomerGood cus
            //                  where cus.IsLead=1 and (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId ) {0}), 0)),0) as Percentage,

            //(Select UserX from SalesMatrix sm where sm.Type='OverallClosingPercentage' and 
            //ISNULL((Select COUNT(cus.Id) from #TempCustomer cus 
            //where (cus.Soldby1=emp.UserId) and cus.IsLead=0  {1})* 100.0 /
            // (NULLIF((Select COUNT(Id) from #TempCustomerGood cus
            //                  where cus.IsLead=1 and (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId ) {0}), 0)),0) between sm.Min and sm.Max
            //) as UserX

            // into #employeeData From Employee emp
            //left join UserPermission up on up.UserId = emp.UserId
            //                  left join UserLogin u on u.UserId=up.UserId
            //left join PermissionGroup pg on pg.Id = up.PermissionGroupId


            //                  select top(@pagesize) * from #employeeData
            //                  where Id not in (Select TOP (@pagestart)  Id from #employeeData #cd {2})
            //                  --order by Id desc
            //                  {3}
            //                  select COUNT(*) TotalEmployee from #employeeData

            //                  select 
            //SUM(TotalLeads) as TotalTotalLeads,
            //SUM(BadLeads) as TotalBadLeads,
            //                  SUM(GoodLeads) as TotalGoodLeads,
            //                  SUM(Closing) as TotalClosing,
            //AVG(Percentage) as AvgPercentage,
            //AVG(UserX) as AvgUserX
            //from #employeeData

            //drop table #employeeData
            //drop table #TempCustomer
            //drop table #TempCustomerBad
            //drop table #TempCustomerGood";
            sqlQuery = @"declare @pagestart int
                        declare @pageend int
                        set @pagestart=(@pageno-1)* @pagesize 
                        set @pageend = @pagesize

                      select cus.Id, cus.CustomerId, cus.Soldby1,cus.Status, CE.AppoinmentSetBy,ccclosing.IsLead,CE.CreatedDay,cus.createdDate,cus.SalesDate
						into #TempCustomer
						from customer cus 
						left join CustomerExtended CE on CE.Customerid = cus.Customerid
						LEFT JOIN CustomerCompany ccclosing on ccclosing.CustomerId=cus.CustomerId  and ccclosing.IsActive=1
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=cus.Status and lkLStatus.DataKey='LeadStatus'
						where cus.IsActive = 1 and cus.JoinDate is not null
						
						select #tc.* into #TempCustomerBad
						From #TempCustomer #tc
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=#tc.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText = 'Bad' 

						select #tc.* into #TempCustomerGood
						From #TempCustomer #tc
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=#tc.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText = 'Good' 

						select 
						emp.Id,u.Id as UserLoginId,emp.UserId as EmpId,

						emp.FirstName +' '+ emp.LastName as EmployeeName,

                        (Select COUNT(cus.Id) from #TempCustomer cus
						where    cus.IsLead=1 and (cus.Soldby1=emp.UserId ) {0}) TotalLeads,

						(Select COUNT(cus.Id) from #TempCustomerBad cus
						where    cus.IsLead=1 and (cus.Soldby1=emp.UserId ) {0}) BadLeads,

						(Select COUNT(cus.Id) from #TempCustomerGood cus
						where    cus.IsLead=1 and (cus.Soldby1=emp.UserId ) {0}) GoodLeads,

						(Select COUNT(cus.Id) from #TempCustomer cus 
						where (cus.Soldby1=emp.UserId) and cus.IsLead=0 {1}) Closing,

						ISNULL(((Select COUNT(cus.Id) from #TempCustomer cus 
						where (cus.Soldby1=emp.UserId) and cus.IsLead=0 {1})* 100.0 /
						 NULLIF((Select COUNT(cus.Id) from #TempCustomerGood cus
                        where cus.IsLead=1 and (cus.Soldby1=emp.UserId  ) {0}), 0)),0) as Percentage,

						(Select UserX from SalesMatrix sm where sm.Type='OverallClosingPercentage' and 
						ISNULL((Select COUNT(cus.Id) from #TempCustomer cus 
						where (cus.Soldby1=emp.UserId) and cus.IsLead=0  {1})* 100.0 /
					  (NULLIF((Select COUNT(Id) from #TempCustomerGood cus
                        where cus.IsLead=1 and (cus.Soldby1=emp.UserId ) {0}), 0)),0) between sm.Min and sm.Max
						) as UserX

						 into #employeeData From Employee emp
						left join UserPermission up on up.UserId = emp.UserId
                        left join UserLogin u on u.UserId=up.UserId
						left join PermissionGroup pg on pg.Id = up.PermissionGroupId
						
                        
                        select top(@pagesize) * from #employeeData
                        where Id not in (Select TOP (@pagestart)  Id from #employeeData #cd {2})
                        --order by Id desc
                        {3}
                        select COUNT(*) TotalEmployee from #employeeData

                        select 
						SUM(TotalLeads) as TotalTotalLeads,
						SUM(BadLeads) as TotalBadLeads,
                        SUM(GoodLeads) as TotalGoodLeads,
                        SUM(Closing) as TotalClosing,
						AVG(Percentage) as AvgPercentage,
						AVG(UserX) as AvgUserX
						from #employeeData

						drop table #employeeData
						drop table #TempCustomer
						drop table #TempCustomerBad
						drop table #TempCustomerGood";


            try
            {
                sqlQuery = string.Format(sqlQuery, createddateSubQuery, joindateSubQuery, orderquery, orderquery1);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetOverAllCloseMatrixWithCount(DateTime? StartDate, DateTime? EndDate, int pageno, int pagesize, string order)
        {
            string sqlQuery = @"";
            string createddateSubQuery = "";
            string joindateSubQuery = "";
            string orderquery = "";
            string orderquery1 = "";
            if (StartDate.HasValue && EndDate.HasValue && StartDate != new DateTime() && EndDate != new DateTime())
            {
                createddateSubQuery = string.Format(" and cus.CreatedDate between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
                joindateSubQuery = string.Format(" and cus.SalesDate between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
            }
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/salesperson")
                {
                    orderquery = "order by #cd.[EmployeeName] asc";
                    orderquery1 = "order by [EmployeeName] asc";
                }
                else if (order == "descending/salesperson")
                {
                    orderquery = "order by #cd.[EmployeeName] desc";
                    orderquery1 = "order by [EmployeeName] desc";
                }
                else if (order == "ascending/leads")
                {
                    orderquery = "order by #cd.NoOfLeads asc";
                    orderquery1 = "order by NoOfLeads asc";
                }
                else if (order == "descending/leads")
                {
                    orderquery = "order by #cd.NoOfLeads desc";
                    orderquery1 = "order by NoOfLeads desc";
                }
                else if (order == "ascending/closed")
                {
                    orderquery = "order by #cd.[Closing] asc";
                    orderquery1 = "order by [Closing] asc";
                }
                else if (order == "descending/closed")
                {
                    orderquery = "order by #cd.[Closing] desc";
                    orderquery1 = "order by [Closing] desc";
                }
                else if (order == "ascending/closing")
                {
                    orderquery = "order by #cd.[Percentage] asc";
                    orderquery1 = "order by [Percentage] asc";
                }
                else if (order == "descending/closing")
                {
                    orderquery = "order by #cd.[Percentage] desc";
                    orderquery1 = "order by [Percentage] desc";
                }
                else if (order == "ascending/userx")
                {
                    orderquery = "order by #cd.[UserX] asc";
                    orderquery1 = "order by [UserX] asc";
                }
                else if (order == "descending/userx")
                {
                    orderquery = "order by #cd.[UserX] desc";
                    orderquery1 = "order by [UserX] desc";
                }



                else
                {
                    orderquery = "order by #cd.[Id]  desc";
                    orderquery1 = "order by Id desc";
                }

            }
            else
            {
                orderquery = "order by #cd.[Id] desc";
                orderquery1 = "order by Id desc";
            }
            #endregion
            sqlQuery = @"declare @pagestart int
                        declare @pageend int
                        set @pagestart=(@pageno-1)* @pagesize 
                        set @pageend = @pagesize

                      select cus.Id, cus.CustomerId, cus.Soldby1,cus.Status, CE.AppoinmentSetBy,ccclosing.IsLead,CE.CreatedDay,cus.CreatedDate,cus.SalesDate
						into #TempCustomer
						from customer cus 
						left join CustomerExtended CE on CE.Customerid = cus.Customerid
						LEFT JOIN CustomerCompany ccclosing on ccclosing.CustomerId=cus.CustomerId
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=cus.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText !='' and CE.IsTestAccount != 1
						
						select #tc.* into #TempCustomerBad
						From #TempCustomer #tc
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=#tc.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText = 'Bad' 

						select #tc.* into #TempCustomerGood
						From #TempCustomer #tc
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=#tc.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText = 'Good' 

						select 
						emp.Id,u.Id as UserLoginId,emp.UserId as EmpId,

						emp.FirstName +' '+ emp.LastName as EmployeeName,

                        (Select COUNT(cus.Id) from #TempCustomer cus
						where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId) {0}) TotalLeads,

						(Select COUNT(cus.Id) from #TempCustomerBad cus
						where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId) {0}) BadLeads,

						(Select COUNT(cus.Id) from #TempCustomerGood cus
						where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId) {0}) GoodLeads,

						(Select COUNT(cus.Id) from #TempCustomer cus 
						where (cus.Soldby1=emp.UserId) {1}) Closing,

						ISNULL(((Select COUNT(cus.Id) from #TempCustomer cus 
						where (cus.Soldby1=emp.UserId) and cus.IsLead=0 {1})* 100.0 /
						 NULLIF((Select COUNT(cus.Id) from #TempCustomerGood cus
                        where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId ) {0}), 0)),0) as Percentage,

						(Select UserX from SalesMatrix sm where sm.Type='OverallClosingPercentage' and 
						ISNULL((Select COUNT(cus.Id) from #TempCustomer cus 
						where (cus.Soldby1=emp.UserId) and cus.IsLead=0  {1})* 100.0 /
					  (NULLIF((Select COUNT(Id) from #TempCustomerGood cus
                        where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId ) {0}), 0)),0) between sm.Min and sm.Max
						) as UserX

						 into #employeeData1 From Employee emp
						left join UserPermission up on up.UserId = emp.UserId
                        left join UserLogin u on u.UserId=up.UserId
						left join PermissionGroup pg on pg.Id = up.PermissionGroupId
						where emp.IsSalesMatrix=1 

                        select empdata.* into #employeeData
						from #employeeData1 empdata
						where empdata.TotalLeads>0
                        
                        select top(@pagesize) * from #employeeData
                        where Id not in (Select TOP (@pagestart)  Id from #employeeData #cd {2})
                        --order by Id desc
                        {3}
                        select COUNT(*) TotalEmployee from #employeeData

                        --select 
						--SUM(TotalLeads) as TotalTotalLeads,
						--SUM(BadLeads) as TotalBadLeads,
                       -- SUM(GoodLeads) as TotalGoodLeads,
                        --SUM(Closing) as TotalClosing,
						--AVG(Percentage) as AvgPercentage,
						--AVG(UserX) as AvgUserX
						--from #employeeData

						drop table #employeeData
						drop table #TempCustomer
						drop table #TempCustomerBad
						drop table #TempCustomerGood";
            try
            {
                sqlQuery = string.Format(sqlQuery, createddateSubQuery, joindateSubQuery, orderquery, orderquery1);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetDownloadOverAllClose(DateTime? StartDate, DateTime? EndDate)
        {
            string sqlQuery = @"";
            string createddateSubQuery = "";
            string joindateSubQuery = "";
            string orderquery = "";
            string orderquery1 = "";
            if (StartDate.HasValue && EndDate.HasValue && StartDate != new DateTime() && EndDate != new DateTime())
            {
                createddateSubQuery = string.Format(" and cus.CreatedDate between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
                joindateSubQuery = string.Format(" and cus.SalesDate between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
            }

            sqlQuery = @"

                        select cus.Id, cus.CustomerId, cus.Soldby1,cus.Status, CE.AppoinmentSetBy,ccclosing.IsLead,CE.CreatedDay,cus.CreatedDate,cus.SalesDate
						into #TempCustomer
						from customer cus 
						left join CustomerExtended CE on CE.Customerid = cus.Customerid
						LEFT JOIN CustomerCompany ccclosing on ccclosing.CustomerId=cus.CustomerId
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=cus.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText !='' and CE.IsTestAccount != 1
						
						select #tc.* into #TempCustomerBad
						From #TempCustomer #tc
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=#tc.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText = 'Bad' 

						select #tc.* into #TempCustomerGood
						From #TempCustomer #tc
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=#tc.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText = 'Good' 

						select 
						emp.Id,u.Id as UserLoginId,emp.UserId as EmpId,

						emp.FirstName +' '+ emp.LastName as [Sales Person],

                        (Select COUNT(cus.Id) from #TempCustomer cus
						where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId)  {0}) TotalLeads,

						(Select COUNT(cus.Id) from #TempCustomerBad cus
						where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId)  {0}) Removed,

						(Select COUNT(cus.Id) from #TempCustomerGood cus
						where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId)  {0}) GoodLeads,

					    (Select COUNT(cus.Id) from #TempCustomer cus 
						where (cus.Soldby1=emp.UserId) {1}) [Overall Close],

						ISNULL(((Select COUNT(cus.Id) from #TempCustomer cus 
						where (cus.Soldby1=emp.UserId) and cus.IsLead=0  {1})* 100.0 /
						 NULLIF((Select COUNT(cus.Id) from #TempCustomerGood cus
                        where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId )  {0}), 0)),0) as [Closing],

						(Select UserX from SalesMatrix sm where sm.Type='OverallClosingPercentage' and 
						ISNULL((Select COUNT(cus.Id) from #TempCustomer cus 
						where (cus.Soldby1=emp.UserId) and cus.IsLead=0   {1})* 100.0 /
					  (NULLIF((Select COUNT(Id) from #TempCustomerGood cus
                        where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId )  {0}), 0)),0) between sm.Min and sm.Max
						) as UserX

						 into #employeeData1 From Employee emp
						left join UserPermission up on up.UserId = emp.UserId
                        left join UserLogin u on u.UserId=up.UserId
						left join PermissionGroup pg on pg.Id = up.PermissionGroupId
						where emp.IsSalesMatrix=1 
                        --and emp.IsCurrentEmployee=1
                        
                        select empdata.* into #employeeData
						from #employeeData1 empdata
						where empdata.TotalLeads>0

                        select  [Sales Person],TotalLeads,Removed,GoodLeads,[Overall Close],cast(Closing as decimal(10,2)) as [Closing %],cast(UserX as decimal(10,2)) as [UserX] from #employeeData
                     
                        order by Id desc
                  
                        select COUNT(*) TotalEmployee from #employeeData

                        drop table #employeeData
                        drop table #TempCustomer
						drop table #TempCustomerBad
                        drop table #TempCustomerGood";
            try
            {
                sqlQuery = string.Format(sqlQuery, createddateSubQuery, joindateSubQuery, orderquery, orderquery1);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetSoldToFundedMatrixWithCount(DateTime? StartDate, DateTime? EndDate, int pageno, int pagesize, string order)
        {
            string sqlQuery = @"";
            string createddateSubQuery = "";
            string joindateSubQuery = "";
            string orderquery = "";
            string orderquery1 = "";
            if (StartDate.HasValue && EndDate.HasValue && StartDate != new DateTime() && EndDate != new DateTime())
            {
                createddateSubQuery = string.Format(" and cus.SalesDate between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
                joindateSubQuery = string.Format(" and cus.CustomerFundedDate between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
            }
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/salesperson")
                {
                    orderquery = "order by #cd.[EmployeeName] asc";
                    orderquery1 = "order by [EmployeeName] asc";
                }
                else if (order == "descending/salesperson")
                {
                    orderquery = "order by #cd.[EmployeeName] desc";
                    orderquery1 = "order by [EmployeeName] desc";
                }
                else if (order == "ascending/sales")
                {
                    orderquery = "order by #cd.NoOfLeads asc";
                    orderquery1 = "order by NoOfLeads asc";
                }
                else if (order == "descending/sales")
                {
                    orderquery = "order by #cd.NoOfLeads desc";
                    orderquery1 = "order by NoOfLeads desc";
                }
                else if (order == "ascending/funded")
                {
                    orderquery = "order by #cd.[CustomerFunded] asc";
                    orderquery1 = "order by [CustomerFunded] asc";
                }
                else if (order == "descending/funded")
                {
                    orderquery = "order by #cd.[CustomerFunded] desc";
                    orderquery1 = "order by [CustomerFunded] desc";
                }
                else if (order == "ascending/percentage")
                {
                    orderquery = "order by #cd.[Percentage] asc";
                    orderquery1 = "order by [Percentage] asc";
                }
                else if (order == "descending/percentage")
                {
                    orderquery = "order by #cd.[Percentage] desc";
                    orderquery1 = "order by [Percentage] desc";
                }
                else if (order == "ascending/userx")
                {
                    orderquery = "order by #cd.[UserX] asc";
                    orderquery1 = "order by [UserX] asc";
                }
                else if (order == "descending/userx")
                {
                    orderquery = "order by #cd.[UserX] desc";
                    orderquery1 = "order by [UserX] desc";
                }



                else
                {
                    orderquery = "order by #cd.[Id]  desc";
                    orderquery1 = "order by Id desc";
                }

            }
            else
            {
                orderquery = "order by #cd.[Id] desc";
                orderquery1 = "order by Id desc";
            }
            #endregion
            sqlQuery = @"declare @pagestart int
                        declare @pageend int
                        set @pagestart=(@pageno-1)* @pagesize 
                        set @pageend = @pagesize

                        select cus.Id, cus.CustomerId, cus.Soldby1,cus.Status, CE.AppoinmentSetBy,ccclosing.IsLead,cus.SalesDate,cus.CustomerFunded,cus.CustomerFundedDate
						into #TempCustomer
						from customer cus 
						left join CustomerExtended CE on CE.Customerid = cus.Customerid
						LEFT JOIN CustomerCompany ccclosing on ccclosing.CustomerId=cus.CustomerId
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=cus.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText !='' and CE.IsTestAccount != 1

						select 
						emp.Id,u.Id as UserLoginId,
						emp.FirstName +' '+ emp.LastName as EmployeeName,emp.UserId as EmpId,

						(Select COUNT(cus.Id) from #TempCustomer cus 
                        where cus.Soldby1=emp.UserId and cus.IsLead=0 and cus.IsLead=0 {0}) NoOfLeads,

						(Select COUNT(cus.Id) from #TempCustomer cus
						where cus.Soldby1=emp.UserId and cus.IsLead=0 and cus.CustomerFunded=1 {1}) CustomerFunded,

						ISNULL(((Select COUNT(cus.Id) from #TempCustomer cus
						where cus.Soldby1=emp.UserId and cus.IsLead=0 and cus.CustomerFunded=1 {1})* 100.0 /
						 NULLIF((Select COUNT(cus.Id) from #TempCustomer cus
                        where cus.Soldby1=emp.UserId and cus.IsLead=0 {0}), 0)),0) as Percentage,

						(Select UserX from SalesMatrix sm where sm.Type='Soldtofunded' and 
						ISNULL((Select COUNT(cus.Id) from #TempCustomer cus 
						where cus.Soldby1=emp.UserId and cus.IsLead=0 and cus.CustomerFunded=1 {1})* 100.0 /
					   (NULLIF((Select COUNT(cus.Id) from #TempCustomer cus 
                        where cus.Soldby1=emp.UserId and cus.IsLead=0 {0}), 0)),0) between sm.Min and sm.Max
						) as UserX

						 into #employeeData1 From Employee emp
						left join UserPermission up on up.UserId = emp.UserId
                        left join UserLogin u on u.UserId=up.UserId
						left join PermissionGroup pg on pg.Id = up.PermissionGroupId
						where emp.IsSalesMatrix=1
                        
                        select empdata.* into #employeeData
						from #employeeData1 empdata
						where empdata.NoOfLeads>0

                        select top(@pagesize) * from #employeeData
                        where Id not in (Select TOP (@pagestart)  Id from #employeeData #cd {2})
                        {3}

                        select COUNT(*) TotalEmployee from #employeeData

                        select 
						SUM(NoOfLeads) as TotalTotalSales,
						SUM(CustomerFunded) as TotalCustomerFunded,
						AVG(Percentage) as AvgPercentage,
						AVG(UserX) as AvgUserX
						from #employeeData

                        drop table #employeeData
						drop table #TempCustomer";
            try
            {
                sqlQuery = string.Format(sqlQuery, createddateSubQuery, joindateSubQuery, orderquery, orderquery1);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetDownloadSoldToFunded(DateTime? StartDate, DateTime? EndDate)
        {
            string sqlQuery = @"";
            string createddateSubQuery = "";
            string joindateSubQuery = "";
            string orderquery = "";
            string orderquery1 = "";
            if (StartDate.HasValue && EndDate.HasValue && StartDate != new DateTime() && EndDate != new DateTime())
            {
                createddateSubQuery = string.Format(" and cus.SalesDate between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
                joindateSubQuery = string.Format(" and cus.CustomerFundedDate between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
            }

            sqlQuery = @"

                        select cus.Id, cus.CustomerId, cus.Soldby1,cus.Status, CE.AppoinmentSetBy,ccclosing.IsLead,cus.SalesDate,cus.CustomerFunded,cus.CustomerFundedDate
						into #TempCustomer
						from customer cus 
						left join CustomerExtended CE on CE.Customerid = cus.Customerid
						LEFT JOIN CustomerCompany ccclosing on ccclosing.CustomerId=cus.CustomerId
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=cus.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText!='' and CE.IsTestAccount != 1

						select 
						emp.Id,
                        --u.Id as UserLoginId,
						emp.FirstName +' '+ emp.LastName as [Sales Person],
                        --emp.UserId as EmpId,

						(Select COUNT(cus.Id) from #TempCustomer cus 
                        where cus.Soldby1=emp.UserId and cus.IsLead=0 and cus.IsLead=0 {0}) [Sales],

						(Select COUNT(cus.Id) from #TempCustomer cus
						where cus.Soldby1=emp.UserId and cus.IsLead=0 and cus.CustomerFunded=1 {1}) Funded,

						 cast(ISNULL(((Select COUNT(cus.Id) from #TempCustomer cus
						where cus.Soldby1=emp.UserId and cus.IsLead=0 and cus.CustomerFunded=1 {1})* 100.0 /
						 NULLIF((Select COUNT(cus.Id) from #TempCustomer cus
                        where cus.Soldby1=emp.UserId and cus.IsLead=0 {0}), 0)),0)as decimal(10,2)) as [Funded %],

						convert(numeric(10,2),(Select UserX from SalesMatrix sm where sm.Type='Soldtofunded' and 
						ISNULL((Select COUNT(cus.Id) from #TempCustomer cus 
						where cus.Soldby1=emp.UserId and cus.IsLead=0 and cus.CustomerFunded=1 {1})* 100.0 /
					   (NULLIF((Select COUNT(cus.Id) from #TempCustomer cus 
                        where cus.Soldby1=emp.UserId and cus.IsLead=0 {0}), 0)),0) between sm.Min and sm.Max
						)) as UserX

						 into #employeeData1 From Employee emp
						left join UserPermission up on up.UserId = emp.UserId
                        left join UserLogin u on u.UserId=up.UserId
						left join PermissionGroup pg on pg.Id = up.PermissionGroupId
						where emp.IsSalesMatrix=1 and emp.IsCurrentEmployee=1

                        select empdata.* into #employeeData
						from #employeeData1 empdata
						where empdata.Sales>0
                        
                        select * from #employeeData
                       
                        order by Id desc

                        select COUNT(*) TotalEmployee from #employeeData

                        drop table #employeeData
						drop table #TempCustomer";
            try
            {
                sqlQuery = string.Format(sqlQuery, createddateSubQuery, joindateSubQuery, orderquery, orderquery1);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetNumberOfSalesMatrixWithCount(DateTime? StartDate, DateTime? EndDate, int pageno, int pagesize, string order)
        {
            string sqlQuery = @"";
            string createddateSubQuery = "";
            string joindateSubQuery = "";
            string orderquery = "";
            string orderquery1 = "";
            if (StartDate.HasValue && EndDate.HasValue && StartDate != new DateTime() && EndDate != new DateTime())
            {
                createddateSubQuery = string.Format(" and cus.CreatedDay between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
                joindateSubQuery = string.Format(" and cus.SalesDate between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
            }
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/salesperson")
                {
                    orderquery = "order by #cd.[EmployeeName] asc";
                    orderquery1 = "order by [EmployeeName] asc";
                }
                else if (order == "descending/salesperson")
                {
                    orderquery = "order by #cd.[EmployeeName] desc";
                    orderquery1 = "order by [EmployeeName] desc";
                }
                else if (order == "ascending/sales")
                {
                    orderquery = "order by #cd.Closing asc";
                    orderquery1 = "order by Closing asc";
                }
                else if (order == "descending/sales")
                {
                    orderquery = "order by #cd.Closing desc";
                    orderquery1 = "order by Closing desc";
                }

                else if (order == "ascending/userx")
                {
                    orderquery = "order by #cd.[UserX] asc";
                    orderquery1 = "order by [UserX] asc";
                }
                else if (order == "descending/userx")
                {
                    orderquery = "order by #cd.[UserX] desc";
                    orderquery1 = "order by [UserX] desc";
                }



                else
                {
                    orderquery = "order by #cd.[Id]  desc";
                    orderquery1 = "order by Id desc";
                }

            }
            else
            {
                orderquery = "order by #cd.[Id] desc";
                orderquery1 = "order by Id desc";
            }
            #endregion
            sqlQuery = @"declare @pagestart int
                        declare @pageend int
                        set @pagestart=(@pageno-1)* @pagesize 
                        set @pageend = @pagesize

                        select cus.Id, cus.CustomerId, cus.Soldby1,cus.Status, CE.AppoinmentSetBy,ccclosing.IsLead,CE.CreatedDay,cus.SalesDate
						into #TempCustomer
						from customer cus 
						left join CustomerExtended CE on CE.Customerid = cus.Customerid
						LEFT JOIN CustomerCompany ccclosing on ccclosing.CustomerId=cus.CustomerId
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=cus.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText !='' and CE.IsTestAccount != 1

						Select 
						emp.Id,u.Id as UserLoginId,emp.UserId as EmpId,

						emp.FirstName +' '+ emp.LastName as EmployeeName,

						(Select COUNT(cus.Id) from #TempCustomer cus 
						where cus.Soldby1=emp.UserId and cus.IsLead=0 {1}) Closing,

						(Select UserX from SalesMatrix sm where sm.Type='NumberofSales' and 
						(Select COUNT(cus.Id) from #TempCustomer cus 
						where cus.Soldby1=emp.UserId and cus.IsLead=0 {1}) between sm.Min and sm.Max
						) as UserX

						 into #employeeData1 From Employee emp
						left join UserPermission up on up.UserId = emp.UserId
                        left join UserLogin u on u.UserId=up.UserId
						left join PermissionGroup pg on pg.Id = up.PermissionGroupId
						where emp.IsSalesMatrix=1 
                        
                        select empdata.* into #employeeData
						from #employeeData1 empdata
						where empdata.Closing>0

                        select top(@pagesize) * from #employeeData
                        where Id not in (Select TOP (@pagestart)  Id from #employeeData #cd {2})
                 --       order by Id desc
                         {3}

                        select COUNT(*) TotalEmployee from #employeeData

                        select 
						SUM(Closing) as TotalTotalSales,
						AVG(UserX) as AvgUserX
						from #employeeData

                        drop table #employeeData
                        drop table #TempCustomer";
            try
            {
                sqlQuery = string.Format(sqlQuery, createddateSubQuery, joindateSubQuery, orderquery, orderquery1);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetDownloadNumberOfSales(DateTime? StartDate, DateTime? EndDate)
        {
            string sqlQuery = @"";
            string createddateSubQuery = "";
            string joindateSubQuery = "";
            string orderquery = "";
            string orderquery1 = "";
            if (StartDate.HasValue && EndDate.HasValue && StartDate != new DateTime() && EndDate != new DateTime())
            {
                createddateSubQuery = string.Format(" and cus.CreatedDay between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
                joindateSubQuery = string.Format(" and cus.SalesDate between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
            }

            sqlQuery = @"

                        select cus.Id, cus.CustomerId, cus.Soldby1,cus.Status, CE.AppoinmentSetBy,ccclosing.IsLead,CE.CreatedDay,cus.SalesDate
						into #TempCustomer
						from customer cus 
						left join CustomerExtended CE on CE.Customerid = cus.Customerid
						LEFT JOIN CustomerCompany ccclosing on ccclosing.CustomerId=cus.CustomerId
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=cus.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText!='' and CE.IsTestAccount != 1

						Select 
						emp.Id,
                        --u.Id as UserLoginId,emp.UserId as EmpId,
						emp.FirstName +' '+ emp.LastName as [Sales Person],

						(Select COUNT(cus.Id) from #TempCustomer cus 
						where cus.Soldby1=emp.UserId and cus.IsLead=0 {1}) Sales,

						convert(numeric(10,2),(Select UserX from SalesMatrix sm where sm.Type='NumberofSales' and 
						(Select COUNT(cus.Id) from #TempCustomer cus 
						where cus.Soldby1=emp.UserId and cus.IsLead=0 {1}) between sm.Min and sm.Max
						)) as UserX

						 into #employeeData From Employee emp
						left join UserPermission up on up.UserId = emp.UserId
                        left join UserLogin u on u.UserId=up.UserId
						left join PermissionGroup pg on pg.Id = up.PermissionGroupId
						where emp.IsSalesMatrix=1 and emp.IsCurrentEmployee=1
                        
                        select * from #employeeData
               
                         order by Id desc
                         

                        select COUNT(*) TotalEmployee from #employeeData

                        drop table #employeeData
                        drop table #TempCustomer";
            try
            {
                sqlQuery = string.Format(sqlQuery, createddateSubQuery, joindateSubQuery, orderquery, orderquery1);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetAppointmentSetMatrixWithCount(DateTime? StartDate, DateTime? EndDate, int pageno, int pagesize, string order)
        {
            string sqlQuery = @"";
            string createddateSubQuery = "";
            string joindateSubQuery = "";
            string orderquery = "";
            string orderquery1 = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/salesperson")
                {
                    orderquery = "order by #cd.[EmployeeName] asc";
                    orderquery1 = "order by [EmployeeName] asc";
                }
                else if (order == "descending/salesperson")
                {
                    orderquery = "order by #cd.[EmployeeName] desc";
                    orderquery1 = "order by [EmployeeName] desc";
                }
                else if (order == "ascending/leads")
                {
                    orderquery = "order by #cd.NoOfLeads asc";
                    orderquery1 = "order by NoOfLeads asc";
                }
                else if (order == "descending/leads")
                {
                    orderquery = "order by #cd.NoOfLeads desc";
                    orderquery1 = "order by NoOfLeads desc";
                }

                else if (order == "ascending/percentage")
                {
                    orderquery = "order by #cd.[Percentage] asc";
                    orderquery1 = "order by [Percentage] asc";
                }
                else if (order == "descending/percentage")
                {
                    orderquery = "order by #cd.[Percentage] desc";
                    orderquery1 = "order by [Percentage] desc";
                }
                else if (order == "ascending/apptsetby")
                {
                    orderquery = "order by #cd.AppoinmentSet asc";
                    orderquery1 = "order by AppoinmentSet asc";
                }
                else if (order == "descending/apptsetby")
                {
                    orderquery = "order by #cd.AppoinmentSet desc";
                    orderquery1 = "order by AppoinmentSet desc";
                }

                else if (order == "ascending/userx")
                {
                    orderquery = "order by #cd.[UserX] asc";
                    orderquery1 = "order by [UserX] asc";
                }
                else if (order == "descending/userx")
                {
                    orderquery = "order by #cd.[UserX] desc";
                    orderquery1 = "order by [UserX] desc";
                }


                else
                {
                    orderquery = "order by #cd.[Id]  desc";
                    orderquery1 = "order by Id desc";
                }

            }
            else
            {
                orderquery = "order by #cd.[Id] desc";
                orderquery1 = "order by Id desc";
            }
            #endregion
            if (StartDate.HasValue && EndDate.HasValue && StartDate != new DateTime() && EndDate != new DateTime())
            {
                createddateSubQuery = string.Format(" and cus.CreatedDay between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
                joindateSubQuery = string.Format(" and cus.SalesDate between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
            }
            sqlQuery = @"declare @pagestart int
                        declare @pageend int
                        set @pagestart=(@pageno-1)* @pagesize 
                        set @pageend = @pagesize

                       select cus.Id, cus.CustomerId, cus.Soldby1,cus.Status, CE.AppoinmentSetBy,ccclosing.IsLead,CE.CreatedDay,cus.SalesDate,cus.CustomerFunded
						into #TempCustomer
						from customer cus 
						left join CustomerExtended CE on CE.Customerid = cus.Customerid
						LEFT JOIN CustomerCompany ccclosing on ccclosing.CustomerId=cus.CustomerId
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=cus.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText !='' and ccclosing.IsLead=1 and CE.IsTestAccount != 1

                        select cus.Id, cus.CustomerId, cus.Soldby1,cus.Status, CE.AppoinmentSetBy,ccclosing.IsLead,CE.CreatedDay,cus.SalesDate,cus.CustomerFunded
						into #TempRealCustomer
						from customer cus 
						left join CustomerExtended CE on CE.Customerid = cus.Customerid
						LEFT JOIN CustomerCompany ccclosing on ccclosing.CustomerId=cus.CustomerId
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=cus.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText !=''
						
						select #tc.* into #TempCustomerBad
						From #TempCustomer #tc
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=#tc.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText = 'Bad' 

						select #tc.* into #TempCustomerGood
						From #TempCustomer #tc
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=#tc.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText = 'Good' 
						
                        select 
						emp.Id,u.Id as UserLoginId,emp.UserId as EmpId,
						emp.FirstName +' '+ emp.LastName as EmployeeName,

						(Select COUNT(cus.Id) from #TempCustomerGood cus 
                        where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId ) {0}) NoOfLeads,

						(Select COUNT(cus.Id) from #TempRealCustomer cus
						where cus.AppoinmentSetBy=emp.UserId {0}) AppoinmentSet,


						ISNULL(((Select COUNT(cus.Id) from #TempRealCustomer cus
						where cus.AppoinmentSetBy=emp.UserId {0})* 100.0 /
						 NULLIF((Select COUNT(cus.Id) from #TempCustomerGood cus
                        where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId) {0}), 0)),0) as Percentage,


						(Select UserX from SalesMatrix sm where sm.Type='AppointmentSetPercentage' and 
						ISNULL((Select COUNT(cus.Id) from #TempRealCustomer cus 
						where cus.AppoinmentSetBy=emp.UserId {0})* 100.0 /
					  (NULLIF((Select COUNT(cus.Id) from #TempCustomerGood cus
                        where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId) {0}), 0)),0) between sm.Min and sm.Max
						) as UserX

						 into #employeeData1 From Employee emp
						left join UserPermission up on up.UserId = emp.UserId
                        left join UserLogin u on u.UserId=up.UserId
						left join PermissionGroup pg on pg.Id = up.PermissionGroupId
						where emp.IsSalesMatrix=1 and emp.IsCurrentEmployee=1

                        select empdata.* into #employeeData
						from #employeeData1 empdata
						where empdata.NoOfLeads>0
                        
                        select top(@pagesize) * from #employeeData
                        where Id not in (Select TOP (@pagestart)  Id from #employeeData #cd {2})
                  --      order by Id desc
                        {3}
                        select COUNT(*) TotalEmployee from #employeeData

                        select 
						SUM(NoOfLeads) as TotalTotalLeads,
						SUM(AppoinmentSet) as TotalAppoinmentSet,
						AVG(Percentage) as AvgPercentage,
						AVG(UserX) as AvgUserX
						from #employeeData

                        drop table #employeeData
                        drop table #TempCustomer
						drop table #TempCustomerBad
						drop table #TempCustomerGood";
            try
            {
                sqlQuery = string.Format(sqlQuery, createddateSubQuery, joindateSubQuery, orderquery, orderquery1);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetDownLoadAppointmentSet(DateTime? StartDate, DateTime? EndDate)
        {
            string sqlQuery = @"";
            string createddateSubQuery = "";
            string joindateSubQuery = "";
            string orderquery = "";
            string orderquery1 = "";

            if (StartDate.HasValue && EndDate.HasValue && StartDate != new DateTime() && EndDate != new DateTime())
            {
                createddateSubQuery = string.Format(" and cus.CreatedDay between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
                joindateSubQuery = string.Format(" and cus.SalesDate between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
            }
            sqlQuery = @"

                        

                       select cus.Id, cus.CustomerId, cus.Soldby1,cus.Status, CE.AppoinmentSetBy,ccclosing.IsLead,CE.CreatedDay,cus.SalesDate,cus.CustomerFunded
						into #TempCustomer
						from customer cus 
						left join CustomerExtended CE on CE.Customerid = cus.Customerid
						LEFT JOIN CustomerCompany ccclosing on ccclosing.CustomerId=cus.CustomerId
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=cus.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText !='' and ccclosing.IsLead=1 and CE.IsTestAccount != 1

                        select cus.Id, cus.CustomerId, cus.Soldby1,cus.Status, CE.AppoinmentSetBy,ccclosing.IsLead,CE.CreatedDay,cus.SalesDate,cus.CustomerFunded
						into #TempRealCustomer
						from customer cus 
						left join CustomerExtended CE on CE.Customerid = cus.Customerid
						LEFT JOIN CustomerCompany ccclosing on ccclosing.CustomerId=cus.CustomerId
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=cus.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText !=''
						
						select #tc.* into #TempCustomerBad
						From #TempCustomer #tc
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=#tc.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText = 'Bad' 

						select #tc.* into #TempCustomerGood
						From #TempCustomer #tc
						LEFT JOIN Lookup lkLStatus on lkLStatus.DataValue=#tc.Status and lkLStatus.DataKey='LeadStatus'
						where lkLStatus.AlterDisplayText = 'Good' 
						
                        select 
						emp.Id,u.Id as UserLoginId,emp.UserId as EmpId,
						emp.FirstName +' '+ emp.LastName as [Sales Person],

						(Select COUNT(cus.Id) from #TempCustomerGood cus 
                        where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId ) {0} ) Leads,

						(Select COUNT(cus.Id) from #TempRealCustomer cus
						where cus.AppoinmentSetBy=emp.UserId {0}) [Appt Set By],


						ISNULL(((Select COUNT(cus.Id) from #TempRealCustomer cus
						where cus.AppoinmentSetBy=emp.UserId {0})* 100.0 /
						 NULLIF((Select COUNT(cus.Id) from #TempCustomerGood cus
                        where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId) {0}), 0)),0) as [Closing],


						(Select UserX from SalesMatrix sm where sm.Type='AppointmentSetPercentage' and 
						ISNULL((Select COUNT(cus.Id) from #TempRealCustomer cus 
						where cus.AppoinmentSetBy=emp.UserId {0})* 100.0 /
					  (NULLIF((Select COUNT(cus.Id) from #TempCustomerGood cus
                        where (cus.Soldby1=emp.UserId or cus.AppoinmentSetBy=emp.UserId) {0}), 0)),0) between sm.Min and sm.Max
						) as UserX

						 into #employeeData1 From Employee emp
						left join UserPermission up on up.UserId = emp.UserId
                        left join UserLogin u on u.UserId=up.UserId
						left join PermissionGroup pg on pg.Id = up.PermissionGroupId
						where emp.IsSalesMatrix=1 

                        select empdata.* into #employeeData
						from #employeeData1 empdata
						where empdata.Leads>0
                        
                        select [Sales Person],Leads,[Appt Set By],cast(Closing as decimal(10,2)) as [Closing %],cast(UserX as decimal(10,2)) as [UserX] from #employeeData
                
                         order by Id desc
                        
                        select COUNT(*) TotalEmployee from #employeeData

                        drop table #employeeData
                        drop table #TempCustomer
                        drop table #TempRealCustomer
                        drop table #TempCustomerBad
                        drop table #TempCustomerGood";
            try
            {
                sqlQuery = string.Format(sqlQuery, createddateSubQuery, joindateSubQuery, orderquery, orderquery1);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetAllInsideSalesEmployee()
        {
            string sqlQuery = "";
            sqlQuery = @"select emp.Id, emp.UserId
                        from Employee emp
                        left join UserPermission up on up.UserId = emp.UserId
                        left join PermissionGroup pg on pg.Id = up.PermissionGroupId
                        where pg.Name='Inside Sales' and emp.IsCurrentEmployee=1";
            try
            {
                sqlQuery = string.Format(sqlQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetAllUserXByUserId(DateTime? StartDate, DateTime? EndDate, Guid userId)
        {
            string sqlQuery = "";
            string createddateSubQuery = "";
            string joindateSubQuery = "";
            if (StartDate.HasValue && EndDate.HasValue && StartDate != new DateTime() && EndDate != new DateTime())
            {
                createddateSubQuery = string.Format(" and cus.CreatedDate between '{0}' and '{1}'", StartDate.Value.SetClientZeroHourToUTC(), EndDate.Value.SetClientMaxHourToUTC());
                joindateSubQuery = string.Format(" and cus.SalesDate between '{0}' and '{1}'", StartDate.Value.SetZeroHour(), EndDate.Value.SetMaxHour());
            }

            sqlQuery = @"select 
						(Select UserX from SalesMatrix sm where sm.Type='FirstCallCosingPecentage' and 
						ISNULL((Select COUNT(cus.Id) from Customer cus
						LEFT JOIN CustomerCompany ccclosing on ccclosing.CustomerId=cus.CustomerId
						 where cus.Soldby=CONVERT(nvarchar(50), emp.UserId) and CAST(cus.SalesDate as date) = CAST(ccclosing.ConvertionDate as date) and ccclosing.IsLead=0  {1})* 100.0 /
						(NULLIF((Select COUNT(Id) from Customer cus where cus.Soldby=CONVERT(nvarchar(50), emp.UserId)  {0}), 0)),0) between sm.Min and sm.Max
						) as FirstCallUserX,

						(Select UserX from SalesMatrix sm where sm.Type='OverallClosingPercentage' and 
						ISNULL((Select COUNT(cus.Id) from Customer cus 
						LEFT JOIN CustomerCompany ccclosing on ccclosing.CustomerId=cus.CustomerId
						where cus.Soldby=CONVERT(nvarchar(50), emp.UserId) and ccclosing.IsLead=0 {1})* 100.0 /
					    (NULLIF((Select COUNT(Id) from Customer cus where cus.Soldby=CONVERT(nvarchar(50), emp.UserId) {0}), 0)),0) between sm.Min and sm.Max
						) as OverallUserX,

						(Select UserX from SalesMatrix sm where sm.Type='Soldtofunded' and 
						ISNULL((Select COUNT(cus.Id) from Customer cus 
						where cus.Soldby=CONVERT(nvarchar(50), emp.UserId) and cus.CustomerFunded=1 {1})* 100.0 /
					  (NULLIF((Select COUNT(Id) from Customer cus where cus.Soldby=CONVERT(nvarchar(50), emp.UserId) {0}), 0)),0) between sm.Min and sm.Max
						) as SoldtofundedUserX,

						(Select UserX from SalesMatrix sm where sm.Type='NumberofSales' and 
						(Select COUNT(cus.Id) from Customer cus 
						LEFT JOIN CustomerCompany ccclosing on ccclosing.CustomerId=cus.CustomerId
						where cus.Soldby=CONVERT(nvarchar(50), emp.UserId) and ccclosing.IsLead=0 {1}) between sm.Min and sm.Max
						) as NumberofSalesUserX,

						(Select UserX from SalesMatrix sm where sm.Type='AppointmentSetPercentage' and 
						ISNULL((Select COUNT(cus.Id) from Customer cus 
						where cus.Soldby=CONVERT(nvarchar(50), emp.UserId) and cus.AppoinmentSet='Yes' {1})* 100.0 /
					  (NULLIF((Select COUNT(Id) from Customer cus where cus.Soldby=CONVERT(nvarchar(50), emp.UserId) {0}), 0)),0) between sm.Min and sm.Max
						) as AppointmentSetUserX

						 into #employeeData From Employee emp
						 where emp.UserId='{2}'

						 select *from #employeeData

						    drop table #employeeData";
            try
            {
                sqlQuery = string.Format(sqlQuery, createddateSubQuery, joindateSubQuery, userId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<string> GetAssignedEmployeeList(List<string> User)
        {
            string queryOwnership = "";
            if (User != null && User.Count() > 0)
            {
                foreach (var item in User)
                {
                    queryOwnership += string.Format("'{0}',", item);
                }
                queryOwnership = queryOwnership.Remove(queryOwnership.Length - 1, 1);
            }

            string sqlQuery = @"Select emp.FirstName +' '+ emp.LastName as EmpName from Employee emp where emp.UserId in ({0})";
            sqlQuery = string.Format(sqlQuery, queryOwnership);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return (from DataRow dr in dsResult.Tables[0].Rows select dr["EmpName"].ToString()).ToList();

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetEmployeeListCalendarSchedule(Guid empid)
        {
            string sqlQuery = @"select iif((select (select CHARINDEX('technician', Tag) from PermissionGroup _pgroup where _pgroup.Id = _up.PermissionGroupId) from UserPermission _up where _up.PermissionGroupId is not null and _up.UserId = emp.Userid) > 0, emp.FirstName + ' ' + emp.LastName + ' (T)', iif(emp.UserId = '22222222-2222-2222-2222-222222222222', emp.FirstName + ' ' + emp.LastName + ' (T)', emp.FirstName + ' ' + emp.LastName)) as ResourceName, emp.UserId from Employee emp where emp.IsCalendar is not null and emp.IsCalendar = 1 and emp.Recruited = 1 and emp.IsActive = 1 and emp.IsDeleted = 0 {0}";
            string subquery = "";
            if (empid != new Guid())
            {
                subquery = string.Format("and emp.UserId = '{0}'", empid);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, subquery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetEmployeeListCustomCalendar(Guid empid)
        {
            string sqlQuery = @"select iif((select (select CHARINDEX('technician', Tag) from PermissionGroup _pgroup where _pgroup.Id = _up.PermissionGroupId) from UserPermission _up where _up.PermissionGroupId is not null and _up.UserId = emp.Userid) > 0, emp.FirstName + ' ' + emp.LastName, iif(emp.UserId = '22222222-2222-2222-2222-222222222222', emp.FirstName + ' ' + emp.LastName, emp.FirstName + ' ' + emp.LastName)) as ResourceName, emp.UserId from Employee emp where emp.IsCalendar is not null and emp.IsCalendar = 1 and emp.Recruited = 1 and emp.IsActive = 1 and emp.IsDeleted = 0 {0}";
            string subquery = "";
            if (empid != new Guid())
            {
                subquery = string.Format("and emp.UserId = '{0}'", empid);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, subquery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetUpsellTechnicianList(Guid userid, string startdate, string enddate, string searchtxt)
        {
            string subquery = "";
            string txtquery = "";
            if (userid != new Guid())
            {
                subquery = string.Format("and emp.UserId = '{0}'", userid);
            }
            if (!string.IsNullOrWhiteSpace(searchtxt))
            {
                txtquery = string.Format("and (emp.FirstName like '%{0}%' or emp.LastName like '%{0}%' or emp.FirstName + ' ' + emp.LastName like '%{0}%')", searchtxt);
            }
            string sqlQuery = @"declare @StartDate nvarchar(50)
                                declare @EndDate nvarchar(50)

                                declare @pageno int = 1
                                declare @pagesize int = 50
                                declare @pagestart int = (@pageno - 1) * @pagesize


                                set @StartDate='{1}'
                                set @EndDate='{2}'

                                Create table #EmployeeData (EmpDay nvarchar(50), EmployeeName nvarchar(250), EmpId int, UserId uniqueidentifier, AddedRMR float, ServiceName nvarchar(MAX), PiecesSold int, CollectedAmount float)

                                while (Replace(@StartDate, '-', '') <= Replace(@EndDate, '-', ''))
                                begin
                                 insert into #EmployeeData(EmpDay, EmployeeName, EmpId, UserId, AddedRMR, ServiceName, PiecesSold, CollectedAmount) 
                                 select convert(date, @StartDate), emp.FirstName + ' ' + emp.LastName, emp.Id, emp.UserId
								 ,ISNULL((select SUM(cae.TotalPrice) from TicketUser tu
								 left join CustomerAppointmentEquipment cae on cae.AppointmentId = tu.TiketId and cae.IsDefaultService = 0 and cae.IsCopied = 0 and cae.IsService = 1
								 where tu.UserId = emp.UserId and tu.IsPrimary = 1 and cae.CreatedDate between @StartDate + ' 00:00:00.000' and @StartDate + ' 23:59:59.999'), 0)
								 ,STUFF((SELECT ', ' + CAST(cae.EquipName AS nvarchar(MAX)) [text()]
								 FROM TicketUser tu
								 left join CustomerAppointmentEquipment cae on cae.AppointmentId = tu.TiketId and cae.IsDefaultService = 0 and cae.IsCopied = 0 and cae.IsService = 1
								 where tu.UserId = emp.UserId and tu.IsPrimary = 1 and cae.CreatedDate between @StartDate + ' 00:00:00.000' and @StartDate + ' 23:59:59.999'
								 FOR XML PATH(''), TYPE)
								.value('.','NVARCHAR(MAX)'),1,2,' ')
								,ISNULL((select SUM(cae.Quantity) from TicketUser tu
								 left join CustomerAppointmentEquipment cae on cae.AppointmentId = tu.TiketId and cae.IsDefaultService = 0 and cae.IsCopied = 0 and cae.IsService = 0
								 where tu.UserId = emp.UserId and tu.IsPrimary = 1 and cae.CreatedDate between @StartDate + ' 00:00:00.000' and @StartDate + ' 23:59:59.999'), 0)
								 ,ISNULL((select SUM(cae.TotalPrice) from TicketUser tu
								 left join CustomerAppointmentEquipment cae on cae.AppointmentId = tu.TiketId and cae.IsDefaultService = 0 and cae.IsCopied = 0 and cae.IsService = 0
								 left join InvoiceDetail detail on detail.EquipmentId = cae.EquipmentId
								 left join Invoice inv on inv.InvoiceId = detail.InvoiceId
								 where tu.UserId = emp.UserId and tu.IsPrimary = 1 and detail.CreatedDate between @StartDate + ' 00:00:00.000' and @StartDate + ' 23:59:59.999' and inv.[Status] = 'Paid'
								 and cae.ReferenceInvoiceId != ''), 0)
								 from Employee emp
                                 left join UserPermission up on up.UserId = emp.UserId
                                 left join PermissionGroup pg on pg.Id = up.PermissionGroupId 
                                 where emp.Recruited = 1 and emp.IsDeleted = 0 and emp.IsActive = 1
                                 and charindex('technician', pg.Tag) > 0
                                {3}
                                {4}
                                 set @StartDate = DATEADD(DAY, 1, CONVERT(date, @StartDate))
                                end

                                select * into #EmployeeDataFilter from #EmployeeData

                                select * into #Total from #EmployeeDataFilter where AddedRMR > 0 and PiecesSold > 0 and CollectedAmount > 0
                                order by EmpDay asc

                                select count(*) as TotalCount from #EmployeeDataFilter

                                select * from #Total
								select sum(PiecesSold) as TotalSold
								,sum(CollectedAmount) as TotalAmount
								,sum(AddedRMR) as TotalRMR
								from #Total

                                drop table #EmployeeData
                                drop table #EmployeeDataFilter
								drop table #Total";
            try
            {
                sqlQuery = string.Format(sqlQuery, userid, startdate, enddate, subquery, txtquery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetEmployeeAccrualPtoAndApprovePtohour(Guid userid)
        {
            string subquery = "";
            string txtquery = "";  
            string sqlQuery = @"Declare @userid uniqueidentifier 
                                declare @Start Datetime;                                declare @End Datetime;                                Set @userid = '{0}'                                                                                                 select @Start = Min(FromDate), @End = Max(EndDate)                                from EmployeePTOHourLog pthour                                where pthour.UserId = @userid
                                Declare @TotalPTOHour float 
                                Declare @TotalApproveHour float 
                                                                  Set @TotalPTOHour =  (Select  Isnull(Sum(pthour.PTOHour),0)                                  from EmployeePTOHourLog pthour                                Where UserId = @userid and FromDate >= @Start and EndDate <= @End)                                                                Set @TotalApproveHour  = (Select Isnull(Sum(Pto.Minute),0)/60 from Pto                                Where UserId = @userid                                and LastUpdatedDate >= @Start and LastUpdatedDate <= @End                                and Status = 'Accepted')  
                                
                                Select @TotalPTOHour As TotalPTOHour,@TotalApproveHour As TotalApproveHour, @TotalPTOHour - @TotalApproveHour As TotalRemaining";
            try
            {
                sqlQuery = string.Format(sqlQuery, userid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetEmployeeAccrualPTOList(PayrollFilterModel filter)
        {
            string subquery = ""; 
            string DateFilterQuery = "";
            if (filter.StartDate != new DateTime() && filter.EndDate != new DateTime())
            {
                DateFilterQuery = string.Format(" and pthour.FromDate >= '{0}' and EndDate <= '{1}'", filter.StartDate.ToString("yyyy-MM-dd"), filter.EndDate.ToString("yyyy-MM-dd"));
            }
            if (!string.IsNullOrEmpty(filter.CurrentEmployee) && filter.CurrentEmployee != "00000000-0000-0000-0000-000000000000")
            {
                subquery = string.Format(" and emp.UserId = '{0}'", filter.CurrentEmployee);
            }

            string sqlQuery = @"select IsNull(SUM(pthour.PTOHour),0) As TotalPTOHour, UserId into #tempuser from EmployeePTOHourLog pthour                                where 1=1 {1}                                and pthour.UserId in (select userid from Employee emp where emp.IsActive = 1                                and emp.IsCurrentEmployee = 1                                and (emp.HireDate is not null and emp.HireDate <> '')                                 and (emp.PayType is not null and emp.PayType != '' and emp.PayType != '-1'))                                Group by UserId 
                                Select tmpus.*,emp.Id,emp.HireDate,emp.PayType, emp.FirstName + ' ' + emp.LastName As EmployeeName                                from #tempuser tmpus                                 left join Employee emp on tmpus.UserId = emp.UserId                                Where 1=1  {0}                                 Order by EmployeeName Asc                                  Drop table #tempuser";
            try
            {
                sqlQuery = string.Format(sqlQuery,subquery, DateFilterQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetUpsellTechnicianListExport(Guid userid, string startdate, string enddate, string searchtxt)
        {
            string subquery = "";
            string txtquery = "";
            if (userid != new Guid())
            {
                subquery = string.Format("and emp.UserId = '{0}'", userid);
            }
            if (!string.IsNullOrWhiteSpace(searchtxt))
            {
                txtquery = string.Format("and (emp.FirstName like '%{0}%' or emp.LastName like '%{0}%' or emp.FirstName + ' ' + emp.LastName like '%{0}%')", searchtxt);
            }
            string sqlQuery = @"declare @StartDate nvarchar(50)
                                declare @EndDate nvarchar(50)

                                declare @pageno int = 1
                                declare @pagesize int = 50
                                declare @pagestart int = (@pageno - 1) * @pagesize


                                set @StartDate='{1}'
                                set @EndDate='{2}'

                                Create table #EmployeeData ([Day] nvarchar(50), Tech nvarchar(250), [Added RMR] float, Service nvarchar(MAX), [RMR Comm.] float, [Pieces Sold] int, [$ Collected] float, [Piece Comm.] float)

                                while (Replace(@StartDate, '-', '') <= Replace(@EndDate, '-', ''))
                                begin
                                 insert into #EmployeeData([Day], Tech, [Added RMR], Service, [RMR Comm.], [Pieces Sold], [$ Collected], [Piece Comm.]) 
                                 select convert(date, @StartDate), emp.FirstName + ' ' + emp.LastName
								 ,cast(ISNULL((select SUM(cae.TotalPrice) from TicketUser tu
								 left join CustomerAppointmentEquipment cae on cae.AppointmentId = tu.TiketId and cae.IsDefaultService = 0 and cae.IsCopied = 0 and cae.IsService = 1
								 where tu.UserId = emp.UserId and tu.IsPrimary = 1 and cae.CreatedDate between @StartDate + ' 00:00:00.000' and @StartDate + ' 23:59:59.999'), 0) as decimal(10,2))
								 ,STUFF((SELECT ', ' + CAST(cae.EquipName AS nvarchar(MAX)) [text()]
								 FROM TicketUser tu
								 left join CustomerAppointmentEquipment cae on cae.AppointmentId = tu.TiketId and cae.IsDefaultService = 0 and cae.IsCopied = 0 and cae.IsService = 1
								 where tu.UserId = emp.UserId and tu.IsPrimary = 1 and cae.CreatedDate between @StartDate + ' 00:00:00.000' and @StartDate + ' 23:59:59.999'
								 FOR XML PATH(''), TYPE)
								.value('.','NVARCHAR(MAX)'),1,2,' ')
								,0
								,ISNULL((select SUM(cae.Quantity) from TicketUser tu
								 left join CustomerAppointmentEquipment cae on cae.AppointmentId = tu.TiketId and cae.IsDefaultService = 0 and cae.IsCopied = 0 and cae.IsService = 0
								 where tu.UserId = emp.UserId and tu.IsPrimary = 1 and cae.CreatedDate between @StartDate + ' 00:00:00.000' and @StartDate + ' 23:59:59.999'), 0)
								 ,cast(ISNULL((select SUM(cae.TotalPrice) from TicketUser tu
								 left join CustomerAppointmentEquipment cae on cae.AppointmentId = tu.TiketId and cae.IsDefaultService = 0 and cae.IsCopied = 0 and cae.IsService = 0
								 left join InvoiceDetail detail on detail.EquipmentId = cae.EquipmentId
								 left join Invoice inv on inv.InvoiceId = detail.InvoiceId
								 where tu.UserId = emp.UserId and tu.IsPrimary = 1 and detail.CreatedDate between @StartDate + ' 00:00:00.000' and @StartDate + ' 23:59:59.999' and inv.[Status] = 'Paid'
								 and cae.ReferenceInvoiceId != ''), 0) as decimal(10,2))
								 ,0
								 from Employee emp
                                 left join UserPermission up on up.UserId = emp.UserId
                                 left join PermissionGroup pg on pg.Id = up.PermissionGroupId 
                                 where emp.Recruited = 1 and emp.IsDeleted = 0 and emp.IsActive = 1
                                 and charindex('technician', pg.Tag) > 0
                                {3}{4}
                                
                                 set @StartDate = DATEADD(DAY, 1, CONVERT(date, @StartDate))
                                end


                                select * into #EmployeeDataFilter from #EmployeeData

                                select * from #EmployeeDataFilter where [Added RMR] > 0 and [Pieces Sold] > 0 and [$ Collected] > 0
                                order by [Day] asc

                                select count(*) as TotalCount from #EmployeeDataFilter

                                drop table #EmployeeData
                                drop table #EmployeeDataFilter";
            try
            {
                sqlQuery = string.Format(sqlQuery, userid, startdate, enddate, subquery, txtquery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetEmployeeByEmployeeIdAndCompanyId(Guid EmployeeId, Guid CompanyId)
        {
            string sqlQuery = @"Select 
	                                    emp.Id
	                                    ,emp.UserId
	                                    ,emp.FirstName
	                                    ,emp.LastName
	                                    ,com.CompanyName
                                        ,emp.ProfilePicture

                                     from Employee emp
                                    left join Company com on com.CompanyId = emp.CompanyId
                                    where 
	                                     emp.UserId = '{0}' 
	                                     and com.CompanyId = '{1}'";

            sqlQuery = string.Format(sqlQuery, EmployeeId, CompanyId);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetCustomerInspactionByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId)
        {
            string sqlQuery = @"select ci.Id, ci.CompanyId,ci.CustomerId
                                ,ci.OutsideRelativeHumidity 
                                ,ci.OutsideTemperature
                                ,ci.FirstFloorRelativeHumidity
                                ,ci.FirstFloorTemperature
                                ,ci.RelativeOther1
                                ,ci.RelativeOther2
                                ,ci.BasementRelativeHumidity
                                ,ci.BasementTemperature
                                ,ci.VisualBasementOther
                                ,ci.NoticedSmellsOrOdorsComment
                                ,ci.NoticedMoldOrMildewComment 
                                ,ci.HomeSufferForrespiratoryComment
                                ,ci.ChildrenPlayInBasementComment
                                ,ci.PetsGoInBasementComment
                                ,ci.NoticedBugsOrRodentsComment
                                ,ci.GetWaterComment
                                ,ci.SeeCondensationPipesDrippingComment 
                                ,ci.RepairsProblemsComment
                                ,ci.HomeTestForPastRadonComment
                                ,ci.CustomerBasementOther
                                ,ci.Drawing
                                ,ci.Notes
                                ,ci.PMSignature
                                ,ci.PMSignatureDate
                                ,ci.HomeOwnerSignature
                                ,ci.HomeOwnerSignatureDate
                                ,ci.CreatedBy
                                ,ci.CreatedDate
                                ,ci.LastUpdatedBy
                                ,ci.LastUpdatedDate
                                ,ci.InspectionPhoto 
								,CASE WHEN (lkCurrentOutSideConditions.DataValue = '-1' or lkCurrentOutSideConditions.DataValue ='' or lkCurrentOutSideConditions.DataValue is null) THEN  '-' WHEN lkCurrentOutSideConditions.DataValue != '-1' THEN  lkCurrentOutSideConditions.DisplayText end CurrentOutsideConditions
								,CASE WHEN (lkHeat.DataValue = '-1' or lkHeat.DataValue = '' or lkHeat.DataValue is null) THEN  '-' WHEN lkHeat.DataValue != '-1' THEN  lkHeat.DisplayText end Heat
								,CASE WHEN (lkAir.DataValue = '-1' or lkAir.DataValue = '' or lkAir.DataValue is null) THEN  '-' WHEN lkAir.DataValue != '-1' THEN  lkAir.DisplayText end Air
                                ,CASE WHEN (lkBasementDehumidifier.DataValue = '-1' or lkBasementDehumidifier.DataValue = '' or lkBasementDehumidifier.DataValue is null) THEN  '-' WHEN lkBasementDehumidifier.DataValue != '-1' THEN  lkBasementDehumidifier.DisplayText end BasementDehumidifier
								--Rating
                                ,CASE WHEN (lkGroundWaterRating.DataValue = '-1' or lkGroundWaterRating.DataValue = '' or lkGroundWaterRating.DataValue is null) THEN  '-' WHEN lkGroundWaterRating.DataValue != '-1' THEN  lkGroundWaterRating.DisplayText end GroundWaterRating
                                ,CASE WHEN (lkIronBacteriaRating.DataValue = '-1' or lkIronBacteriaRating.DataValue = '' or lkIronBacteriaRating.DataValue is null) THEN  '-' WHEN lkIronBacteriaRating.DataValue != '-1' THEN  lkIronBacteriaRating.DisplayText end IronBacteriaRating
								,CASE WHEN (lkCondensationRating.DataValue = '-1' or lkCondensationRating.DataValue = '' or lkCondensationRating.DataValue is null) THEN  '-' WHEN lkCondensationRating.DataValue != '-1' THEN  lkCondensationRating.DisplayText end CondensationRating
								,CASE WHEN (lkWallCracksRating.DataValue = '-1' or lkWallCracksRating.DataValue = '' or lkWallCracksRating.DataValue is null) THEN  '-' WHEN lkWallCracksRating.DataValue != '-1' THEN  lkWallCracksRating.DisplayText end WallCracksRating
								,CASE WHEN (lkFloorCracksRating.DataValue = '-1' or lkFloorCracksRating.DataValue = '' or lkFloorCracksRating.DataValue is null) THEN  '-' WHEN lkFloorCracksRating.DataValue != '-1' THEN  lkFloorCracksRating.DisplayText end FloorCracksRating
								,CASE WHEN (lkLivingPlan.DataValue = '-1' or lkLivingPlan.DataValue = '' or lkLivingPlan.DataValue is null) THEN  '-' WHEN lkLivingPlan.DataValue != '-1' THEN  lkLivingPlan.DisplayText end LivingPlan
                                --YesNo
								,CASE WHEN (lkGroundWater.DataValue = '-1' or lkGroundWater.DataValue = '' or lkGroundWater.DataValue is null) THEN  '-' WHEN lkGroundWater.DataValue != '-1' THEN  lkGroundWater.DisplayText end GroundWater
								,CASE WHEN (lkIronBacteria.DataValue = '-1' or lkIronBacteria.DataValue = '' or lkIronBacteria.DataValue is null) THEN  '-' WHEN lkIronBacteria.DataValue != '-1' THEN  lkIronBacteria.DisplayText end IronBacteria
								,CASE WHEN (lkCondensation.DataValue = '-1' or lkCondensation.DataValue = '' or lkCondensation.DataValue is null) THEN  '-' WHEN lkCondensation.DataValue != '-1' THEN  lkCondensation.DisplayText end Condensation
								,CASE WHEN (lkWallCracks.DataValue = '-1' or lkWallCracks.DataValue = '' or lkWallCracks.DataValue is null) THEN  '-' WHEN lkWallCracks.DataValue != '-1' THEN  lkWallCracks.DisplayText end WallCracks
								,CASE WHEN (lkFloorCracks.DataValue = '-1' or lkFloorCracks.DataValue = '' or lkFloorCracks.DataValue is null) THEN  '-' WHEN lkFloorCracks.DataValue != '-1' THEN  lkFloorCracks.DisplayText end FloorCracks
								,CASE WHEN (lkExistingSumpPump.DataValue = '-1' or lkExistingSumpPump.DataValue = '' or lkExistingSumpPump.DataValue is null) THEN  '-' WHEN lkExistingSumpPump.DataValue != '-1' THEN  lkExistingSumpPump.DisplayText end ExistingSumpPump
								,CASE WHEN (lkExistingDrainageSystem.DataValue = '-1' or lkExistingDrainageSystem.DataValue = '' or lkExistingDrainageSystem.DataValue is null) THEN  '-' WHEN lkExistingDrainageSystem.DataValue != '-1' THEN  lkExistingDrainageSystem.DisplayText end ExistingDrainageSystem
								,CASE WHEN (lkExistingRadonSystem.DataValue = '-1' or lkExistingRadonSystem.DataValue = '' or lkExistingRadonSystem.DataValue is null) THEN  '-' WHEN lkExistingRadonSystem.DataValue != '-1' THEN  lkExistingRadonSystem.DisplayText end ExistingRadonSystem
								,CASE WHEN (lkDryerVentToCode.DataValue = '-1' or lkDryerVentToCode.DataValue = '' or lkDryerVentToCode.DataValue is null) THEN  '-' WHEN lkDryerVentToCode.DataValue != '-1' THEN  lkDryerVentToCode.DisplayText end DryerVentToCode
								,CASE WHEN (lkFoundationType.DataValue = '-1' or lkFoundationType.DataValue = '' or lkFoundationType.DataValue is null) THEN  '-' WHEN lkFoundationType.DataValue != '-1' THEN  lkFoundationType.DisplayText end FoundationType
								,CASE WHEN (lkBulkhead.DataValue = '-1' or lkBulkhead.DataValue = '' or lkBulkhead.DataValue is null) THEN  '-' WHEN lkBulkhead.DataValue != '-1' THEN  lkBulkhead.DisplayText end Bulkhead
								,CASE WHEN (lkNoticedSmellsOrOdors.DataValue = '-1' or lkNoticedSmellsOrOdors.DataValue = '' or lkNoticedSmellsOrOdors.DataValue is null) THEN  '-' WHEN lkNoticedSmellsOrOdors.DataValue != '-1' THEN  lkNoticedSmellsOrOdors.DisplayText end NoticedSmellsOrOdors
								,CASE WHEN (lkNoticedMoldOrMildew.DataValue = '-1' or lkNoticedMoldOrMildew.DataValue = '' or lkNoticedMoldOrMildew.DataValue is null) THEN  '-' WHEN lkNoticedMoldOrMildew.DataValue != '-1' THEN  lkNoticedMoldOrMildew.DisplayText end NoticedMoldOrMildew
								,CASE WHEN (lkHomeSufferForRespiratory.DataValue = '-1' or lkHomeSufferForRespiratory.DataValue = '' or lkHomeSufferForRespiratory.DataValue is null) THEN  '-' WHEN lkHomeSufferForRespiratory.DataValue != '-1' THEN  lkHomeSufferForRespiratory.DisplayText end HomeSufferForRespiratory
								,CASE WHEN (lkChildrenPlayInBasement.DataValue = '-1' or lkChildrenPlayInBasement.DataValue = '' or lkChildrenPlayInBasement.DataValue is null) THEN  '-' WHEN lkChildrenPlayInBasement.DataValue != '-1' THEN  lkChildrenPlayInBasement.DisplayText end ChildrenPlayInBasement
								,CASE WHEN (lkPetsGoInBasement.DataValue = '-1' or lkPetsGoInBasement.DataValue = '' or lkPetsGoInBasement.DataValue is null) THEN  '-' WHEN lkPetsGoInBasement.DataValue != '-1' THEN  lkPetsGoInBasement.DisplayText end PetsGoInBasement
								,CASE WHEN (lkNoticedBugsOrRodents.DataValue = '-1' or lkNoticedBugsOrRodents.DataValue = '' or lkNoticedBugsOrRodents.DataValue is null) THEN  '-' WHEN lkNoticedBugsOrRodents.DataValue != '-1' THEN  lkNoticedBugsOrRodents.DisplayText end NoticedBugsOrRodents
								,CASE WHEN (lkGetWater.DataValue = '-1' or lkGetWater.DataValue = '' or lkGetWater.DataValue is null) THEN  '-' WHEN lkGetWater.DataValue != '-1' THEN  lkGetWater.DisplayText end GetWater
								,CASE WHEN (lkSeeCondensationPipesDripping.DataValue = '-1' or lkSeeCondensationPipesDripping.DataValue = '' or lkSeeCondensationPipesDripping.DataValue is null) THEN  '-' WHEN lkSeeCondensationPipesDripping.DataValue != '-1' THEN  lkSeeCondensationPipesDripping.DisplayText end SeeCondensationPipesDripping
								,CASE WHEN (lkRepairsProblems.DataValue = '-1' or lkRepairsProblems.DataValue = '' or lkRepairsProblems.DataValue is null) THEN  '-' WHEN lkRepairsProblems.DataValue != '-1' THEN  lkRepairsProblems.DisplayText end RepairsProblems
								,CASE WHEN (lkSellPlaning.DataValue = '-1' or lkSellPlaning.DataValue = '' or lkSellPlaning.DataValue is null) THEN  '-' WHEN lkSellPlaning.DataValue != '-1' THEN  lkSellPlaning.DisplayText end SellPlaning
								,CASE WHEN (lkHomeTestForPastRadon.DataValue = '-1' or lkHomeTestForPastRadon.DataValue = '' or lkHomeTestForPastRadon.DataValue is null) THEN  '-' WHEN lkHomeTestForPastRadon.DataValue != '-1' THEN  lkHomeTestForPastRadon.DisplayText end HomeTestForPastRadon
								--,CASE WHEN lkFoundationType2.DataValue = '-1' THEN  '-' WHEN lkFoundationType2.DataValue != '-1' THEN  lkFoundationType2.DisplayText end FoundationType2
                                
                                ,CASE WHEN (lkBasementGoDown.DataValue = '-1' or lkBasementGoDown.DataValue = '' or lkBasementGoDown.DataValue is null) THEN  '-' WHEN lkBasementGoDown.DataValue != '-1' THEN  lkBasementGoDown.DisplayText end BasementGoDown
                                ,CASE WHEN (lkRemoveWater.DataValue = '-1' or lkRemoveWater.DataValue ='' or lkRemoveWater.DataValue is null) THEN  '-' WHEN lkRemoveWater.DataValue != '-1' THEN  lkRemoveWater.DisplayText end RemoveWater
                                ,CASE WHEN (lkPlansForBasementOnce.DataValue = '-1' or lkPlansForBasementOnce.DataValue = '' or lkPlansForBasementOnce.DataValue is null) THEN  '-' WHEN lkPlansForBasementOnce.DataValue != '-1' THEN  lkPlansForBasementOnce.DisplayText end PlansForBasementOnce
								,CASE WHEN (lkLosePower.DataValue = '-1' or lkLosePower.DataValue = '' or lkLosePower.DataValue is null) THEN  '-' WHEN lkLosePower.DataValue != '-1' THEN  lkLosePower.DisplayText end LosePower
								,CASE WHEN (lkLosePowerHowOften.DataValue = '-1' or lkLosePowerHowOften.DataValue = '' or lkLosePowerHowOften.DataValue is null) THEN  '-' WHEN lkLosePowerHowOften.DataValue != '-1' THEN  lkLosePowerHowOften.DisplayText end LosePowerHowOften

                                from CustomerInspection ci
                                left join [Lookup] lkCurrentOutSideConditions on lkCurrentOutSideConditions.DataKey = 'CurrentOutsideConditions' and ci.CurrentOutsideConditions = lkCurrentOutSideConditions.DataValue
                                left join [Lookup] lkHeat on lkHeat.DataKey = 'Heat' and ci.Heat = lkHeat.DataValue
                                left join [Lookup] lkAir on lkAir.DataKey = 'Air' and ci.Heat = lkAir.DataValue
                                left join [Lookup] lkBasementDehumidifier on lkBasementDehumidifier.DataKey = 'BasementDehumidifier' and ci.BasementDehumidifier = lkBasementDehumidifier.DataValue

                                left join [Lookup] lkGroundWaterRating on lkGroundWaterRating.DataKey = 'Rating' and ci.GroundWaterRating = lkGroundWaterRating.DataValue
								left join [Lookup] lkIronBacteriaRating on lkIronBacteriaRating.DataKey = 'Rating' and ci.IronBacteriaRating = lkIronBacteriaRating.DataValue
								left join [Lookup] lkCondensationRating on lkCondensationRating.DataKey = 'Rating' and ci.CondensationRating = lkCondensationRating.DataValue
								left join [Lookup] lkWallCracksRating on lkWallCracksRating.DataKey = 'Rating' and ci.WallCracksRating = lkWallCracksRating.DataValue
								left join [Lookup] lkFloorCracksRating on lkFloorCracksRating.DataKey = 'Rating' and ci.FloorCracksRating = lkFloorCracksRating.DataValue
								left join [Lookup] lkLivingPlan on lkLivingPlan.DataKey = 'Rating' and ci.LivingPlan = lkLivingPlan.DataValue

								left join [Lookup] lkGroundWater on lkGroundWater.DataKey = 'YesNo' and ci.GroundWater = lkGroundWater.DataValue
								left join [Lookup] lkIronBacteria on lkIronBacteria.DataKey = 'YesNo' and ci.IronBacteria = lkIronBacteria.DataValue
								left join [Lookup] lkCondensation on lkCondensation.DataKey = 'YesNo' and ci.Condensation = lkCondensation.DataValue
								left join [Lookup] lkWallCracks on lkWallCracks.DataKey = 'YesNo' and ci.WallCracks = lkWallCracks.DataValue
								left join [Lookup] lkFloorCracks on lkFloorCracks.DataKey = 'YesNo' and ci.FloorCracks = lkFloorCracks.DataValue
								left join [Lookup] lkExistingSumpPump on lkExistingSumpPump.DataKey = 'YesNo' and ci.ExistingSumpPump = lkExistingSumpPump.DataValue
								left join [Lookup] lkExistingDrainageSystem on lkExistingDrainageSystem.DataKey = 'YesNo' and ci.ExistingDrainageSystem = lkExistingDrainageSystem.DataValue
								left join [Lookup] lkExistingRadonSystem on lkExistingRadonSystem.DataKey = 'YesNo' and ci.ExistingRadonSystem = lkExistingRadonSystem.DataValue
								left join [Lookup] lkDryerVentToCode on lkDryerVentToCode.DataKey = 'YesNo' and ci.DryerVentToCode = lkDryerVentToCode.DataValue
								left join [Lookup] lkFoundationType on lkFoundationType.DataKey = 'YesNo' and ci.FoundationType = lkFoundationType.DataValue
								left join [Lookup] lkBulkhead on lkBulkhead.DataKey = 'YesNo' and ci.Bulkhead = lkBulkhead.DataValue
								left join [Lookup] lkNoticedSmellsOrOdors on lkNoticedSmellsOrOdors.DataKey = 'YesNo' and ci.NoticedSmellsOrOdors = lkNoticedSmellsOrOdors.DataValue
								left join [Lookup] lkNoticedMoldOrMildew on lkNoticedMoldOrMildew.DataKey = 'YesNo' and ci.NoticedMoldOrMildew = lkNoticedMoldOrMildew.DataValue
								left join [Lookup] lkHomeSufferForRespiratory on lkHomeSufferForRespiratory.DataKey = 'YesNo' and ci.HomeSufferForRespiratory = lkHomeSufferForRespiratory.DataValue
								left join [Lookup] lkChildrenPlayInBasement on lkChildrenPlayInBasement.DataKey = 'YesNo' and ci.ChildrenPlayInBasement = lkChildrenPlayInBasement.DataValue
								left join [Lookup] lkPetsGoInBasement on lkPetsGoInBasement.DataKey = 'YesNo' and ci.PetsGoInBasement = lkPetsGoInBasement.DataValue
								left join [Lookup] lkNoticedBugsOrRodents on lkNoticedBugsOrRodents.DataKey = 'YesNo' and ci.NoticedBugsOrRodents = lkNoticedBugsOrRodents.DataValue
								left join [Lookup] lkGetWater on lkGetWater.DataKey = 'YesNo' and ci.GetWater = lkGetWater.DataValue
								left join [Lookup] lkSeeCondensationPipesDripping on lkSeeCondensationPipesDripping.DataKey = 'YesNo' and ci.SeeCondensationPipesDripping = lkSeeCondensationPipesDripping.DataValue
								left join [Lookup] lkRepairsProblems on lkRepairsProblems.DataKey = 'YesNo' and ci.RepairsProblems = lkRepairsProblems.DataValue
								left join [Lookup] lkSellPlaning on lkSellPlaning.DataKey = 'YesNo' and ci.SellPlaning = lkSellPlaning.DataValue
								left join [Lookup] lkHomeTestForPastRadon on lkHomeTestForPastRadon.DataKey = 'YesNo' and ci.HomeTestForPastRadon = lkHomeTestForPastRadon.DataValue
                                --left join [Lookup] lkFoundationType2 on lkFoundationType2.DataKey = 'FoundationType' and ci.FoundationType = lkFoundationType2.DataValue
                                left join [Lookup] lkBasementGoDown on lkBasementGoDown.DataKey = 'GoDownBasement' and ci.BasementGoDown = lkBasementGoDown.DataValue
                                left join [Lookup] lkRemoveWater on lkRemoveWater.DataKey = 'RemoveWater' and ci.RemoveWater = lkRemoveWater.DataValue
                                left join [Lookup] lkPlansForBasementOnce on lkPlansForBasementOnce.DataKey = 'BasementPlans' and ci.PlansForBasementOnce = lkPlansForBasementOnce.DataValue
                                left join [Lookup] lkLosePower on lkLosePower.DataKey = 'LosePower' and ci.LosePower = lkLosePower.DataValue
								left join [Lookup] lkLosePowerHowOften on lkLosePowerHowOften.DataKey = 'LosePower' and ci.LosePowerHowOften = lkLosePowerHowOften.DataValue

                                where ci.CompanyId = '{0}' and ci.CustomerId = '{1}'";

            sqlQuery = string.Format(sqlQuery, CompanyId, CustomerId);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetTotalEmployeeCountByCompanyId(Guid CompanyId)
        {
            string sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                Set @CompanyId = '{0}'
                                select count(*) as TotalEmployee from Employee Emp
                                Left Join UserPermission UP on Up.UserId = Emp.UserId
                                Left Join PermissionGroup PG on PG.Id = Up.PermissionGroupId
                                Where PG.Tag = 'Employee' and Emp.CompanyId = @CompanyId";
            try
            {
                sqlQuery = string.Format(sqlQuery, CompanyId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetEmployeeWithRoleByUserName(Guid UserId)
        {
            string sqlQuery = @"DECLARE @UserId uniqueidentifier
                                Set @UserId = '{0}'
                                select distinct Emp.*,PG.Name from Employee Emp
                                Left Join UserPermission UP on Up.UserId = Emp.UserId
                                Left Join PermissionGroup PG on PG.Id = Up.PermissionGroupId
                                Where Emp.UserId = @UserId";
            try
            {
                sqlQuery = string.Format(sqlQuery, UserId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetTechUpSalesReportALLByCompanyId(Guid companyid, DateTime? start, DateTime? end, string searchtext, int pageno, int pagesize, string order, List<string> SalesList)
        {
            string SalesPersonQuery = "";
            if (SalesList != null && SalesList.Count > 0 && SalesList[0] != "null")
            {
                string Ids = "";
                foreach (string id in SalesList)
                {
                    Ids += string.Format("'{0}',", id);
                }

                SalesPersonQuery = "and emp.UserId in (" + Ids.TrimEnd(',') + ")";
            }
            //string subquery = "";
            //if (start.HasValue && start.Value != new DateTime() && end.HasValue && end.Value != new DateTime())
            //{
            //    subquery = string.Format("and cus.SalesDate between '{0}' and '{1}'", start.Value.ToString("yyyy-MM-dd 00:00:00.000"), end.Value.ToString("yyyy-MM-dd 23:59:59.000"));
            //}
            //string searchquery = "";
            //if (!string.IsNullOrWhiteSpace(searchtext))
            //{
            //    searchquery += "and CHARINDEX(@SearchText, cus.SearchText) > 0";
            //}

            string sqlQuery = @"declare @pagestart int
                                declare @pageend int
                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

                                select * into #SaleCom from (Select
								emp.Id,
								emp.FirstName +' '+emp.LastName as TechName,
								0 as RMR,
								0 as Commission,
								0 as EquipmentQty,
								0 as EquipmentValue,
								0 as EquipmentCommission
								from Employee emp
								LEFT JOIN UserPermission up on up.UserId=emp.UserId
								LEFT JOIN PermissionGroup pg on pg.Id=up.PermissionGroupId
								where (pg.Name like '%Technician%' or pg.Name like '%Installer%') {0}
								) d

								Select *into #tempSaleCom from #SaleCom

								select top(@pagesize)
								* into #10SaleCom from #tempSaleCom
								where Id not in(select top(@pagestart) Id from #tempSaleCom #cd order by TechName desc)
                                order by TechName desc

								select * from #10SaleCom

								select count(*) CountTotal
                                from #tempSaleCom

								select 
								 sum(RMR) as TotalRMR
								,sum(Commission) as TotalCommission
								,sum(EquipmentQty) as TotalEquipmentQty
								,sum(EquipmentValue) as TotalEquipmentValue
								,sum(EquipmentCommission) as TotalEquipmentCommission
								from #10SaleCom

								drop table #SaleCom
								drop table #tempSaleCom
                                drop table #10SaleCom";
            try
            {
                sqlQuery = string.Format(sqlQuery, SalesPersonQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
