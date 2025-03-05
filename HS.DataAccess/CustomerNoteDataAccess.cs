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
	public partial class CustomerNoteDataAccess
	{
        public CustomerNoteDataAccess(string ConnectionString):base(ConnectionString)
        {

        }
        public CustomerNoteDataAccess() { }

        public List<CustomerNote> GetTodaysReminders()
        {
            string SqlQuery = @"select * from CustomerNote cn 
                                where ReminderDate between '{0}' and '{1}'and IsShedule = 1
                                ";
            try
            {
                SqlQuery = string.Format(SqlQuery, DateTime.Now.SetZeroHour().ToString("yyyy-MM-dd HH:mm"), DateTime.Now.SetMaxHour().ToString("yyyy-MM-dd HH:mm"));
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
                {

                    return GetList(cmd,-1);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool ReseedCustomerNoteTable()
        {
            string SqlQuery = @"
                                Delete from CustomerNote
                                DBCC CHECKIDENT('CustomerNote', RESEED, 0) 
                                ";
            try
            {
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
                {

                    ExecuteCommand(cmd);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public DataTable GetNoteAssignEmployeeIdByNoteId(int noteid)
        {
            string sqlQuery = @"select na.EmployeeId
                                from NoteAssign na
                                where na.NoteId='{0}'
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, noteid);
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

        public DataTable GetNotesByCustomerCompany(Guid companyID, Guid customerID)
        {
            string sqlQuery = @"select CN.* ,
                                lkNType.AlterDisplayText as Color,
                                lkNType.DisplayText as NoteTypeValue,
                                from CustomerNote CN
                                left join CustomerCompany CC 
                                ON CN.CustomerId = CC.CustomerId 
                                LEFT JOIN Lookup lkNType on lkNType.DataValue=CN.NoteType and lkNType.DataKey='NoteType' and CN.NoteType!='-1'
                                WHERE CC.IsLead = 1 AND CN.IsShedule = 0 
                                AND CN.CompanyId = '{0}' 
                                and CN.CustomerId='{1}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyID, customerID);
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

        public DataTable GetLeadNotesByCustomerIdandCompanyId(Guid companyID, Guid customerID)
        {
            string sqlQuery = @"Select CN.*,
                                lkNType.AlterDisplayText as Color,
                                lkNType.DisplayText as NoteTypeValue,
                                NA.EmployeeId as EmployeeId
                                from CustomerNote CN
                                left join CustomerCompany CC
                                ON CN.CustomerId = CC.CustomerId AND CN.CompanyId = CC.CompanyId
                                left join NoteAssign NA
                                on CN.Id = NA.NoteId
                                LEFT JOIN Lookup lkNType on lkNType.DataValue=CN.NoteType and lkNType.DataKey='NoteType' and CN.NoteType!='-1'
                                Where CC.IsLead = 1 AND CN.IsShedule = 0 AND CN.CompanyId = '{0}'
                                and CN.CustomerId = '{1}'
	                            order by IsPin desc,CreatedDate desc";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyID, customerID);
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

        public DataTable GetReminderScheduleByReminderDateAndEmployeeId(string datetime, Guid empid, string enddate)
        {
            string sqlQuery = @"select cn.Notes, cn.Id from CustomerNote cn
                                left join NoteAssign na on na.NoteId = cn.Id
                                where (cn.ReminderDate between '{0}' and '{2}'
                                or cn.ReminderEndDate between '{0}' and '{2}')
                                and na.EmployeeId = '{1}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, datetime, empid, enddate);
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

        public DataTable GetReminderSchedule1ByReminderDateAndEmployeeId(string datetime, Guid empid, string enddate)
        {
            string sqlQuery = @"select cn.Notes, cn.Id from CustomerNote cn
                                left join NoteAssign na on na.NoteId = cn.Id
                                where cn.ReminderEndDate >= '{0}'
								and cn.ReminderEndDate <= '{2}'
                                and na.EmployeeId = '{1}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, datetime, empid, enddate);
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

        public DataTable GetAllNotesByCustomerNoteId(int id)
        {
            string sqlQuery = @"select CN.*,
                                lkNType.AlterDisplayText as Color,
                                lkNType.DisplayText as NoteTypeValue,
                                EMP.FirstName+' '+EMP.LastName AS AssignName,
                                 EMP.UserId as EmployeeId
 
                                from CustomerNote CN 
                                left join NoteAssign NA 
                                ON CN.Id = NA.NoteId 
                                left join Employee EMP
                                ON EMP.UserId = NA.EmployeeId
                                LEFT JOIN Lookup lkNType on lkNType.DataValue=CN.NoteType and lkNType.DataKey='NoteType' and CN.NoteType!='-1'
                                WHERE CN.Id = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, id);
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
        public DataTable GetTopNotesByCustomerNoteId(int id)
        {
            string sqlQuery = @"select * from CustomerNote 
                                WHERE Id = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, id);
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
        public DataTable GetNoteidByCustomerNoteId()
        {
            string sqlQuery = @"select na.NoteId
                                from NoteAssign na
                                left join CustomerNote note
                                on note.Id = na.NoteId";
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
        //Not in use anymore
        public DataTable GetAllCustomerNoteByCustomerId(Guid customerId, Guid companyId)
        {

            string sqlQuery = @"Select CN.*,
                                lkNType.AlterDisplayText as Color,
                                lkNType.DisplayText as NoteTypeValue,
                                emp.FirstName + ' '+emp.LastName as empName,
                                (select CAST(FirstName + ' '+LastName + ', ' AS VARCHAR(200))  from Employee  where UserId in (select na.EmployeeId from NoteAssign na where CN.Id = na.NoteId) FOR XML PATH ('')) as AssignName
                                into #ReminderData
                                from CustomerNote CN
                                LEFT JOIN Lookup lkNType on lkNType.DataValue=CN.NoteType and lkNType.DataKey='NoteType' and CN.NoteType!='-1'
                                left join Employee emp 
                                on emp.UserId = CN.CreatedByUid
                                --left join Employee emp1 
                                --on emp1.UserId = na.EmployeeId
                                Where CN.CustomerId = '{0}' 
                                AND CN.CompanyId='{1}'
                                and (CN.IsOverview = 1 or (CN.IsShedule = 1 and CN.IsFollowUp = 1))
                                select * from #ReminderData order by OrderBy asc
                                drop table #ReminderData   ";

            try
            {
                sqlQuery = string.Format(sqlQuery, customerId, companyId);
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
        public DataTable GetAssignedNotesListByCustomerId(Guid customerId, Guid companyId)
        {

            string sqlQuery = @"Select CN.*,
                                lkNType.AlterDisplayText as Color,
                                lkNType.DisplayText as NoteTypeValue,
                                emp.FirstName + ' '+emp.LastName as empName,
                                (select CAST(FirstName + ' '+LastName + ', ' AS VARCHAR(200))  from Employee  where UserId in (select na.EmployeeId from NoteAssign na where CN.Id = na.NoteId) FOR XML PATH ('')) as AssignName
                                into #ReminderData
                                from CustomerNote CN
                                LEFT JOIN Lookup lkNType on lkNType.DataValue=CN.NoteType and lkNType.DataKey='NoteType' and CN.NoteType!='-1'
                                left join Employee emp 
                                on emp.UserId = CN.CreatedByUid
                                --left join Employee emp1 
                                --on emp1.UserId = na.EmployeeId
                                Where CN.CustomerId = '{0}' 
                                AND CN.CompanyId='{1}'
                                and cn.IsActive = 1
                                select * from #ReminderData order by OrderBy asc
                                drop table #ReminderData   ";

            try
            {
                sqlQuery = string.Format(sqlQuery, customerId, companyId);
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
        public DataTable GetAllCustomerNotesByCustomerId(Guid customerId, Guid companyId, int pageno, int pagesize, DateTime? StartDate, DateTime? EndDate, string SearchText)
        {
            string SearchQuery = "";
            string DateFilterQuery = "";
            if(!string.IsNullOrEmpty(SearchText))
            {
                SearchQuery = string.Format(" and CN.Notes like '%{0}%'",SearchText);
            }
            if((StartDate != null && StartDate != new DateTime()) && (EndDate != null && EndDate != new DateTime()))
            {
                var StartDateValue = StartDate.Value.SetZeroHour().ClientToUTCTime().ToString("MM/dd/yy");
                var EndDateValue = EndDate.Value.SetMaxHour().ClientToUTCTime().ToString("MM/dd/yy");
                DateFilterQuery = string.Format(" and CN.CreatedDate between '{0}' and '{1}'", StartDateValue, EndDateValue);
            }
        
            string sqlQuery = @"declare @pagestart int
                                declare @pageend int
                                set @pagestart=({2}-1)* {3}
                                set @pageend = {3}

                                Select distinct CN.*,
                                lkNType.AlterDisplayText as Color,
                                lkNType.DisplayText as NoteTypeValue,
                                (select count(tn.Id) from TaskNote tn where tn.TaskId=CN.Id) as ReplyCount,
                                emp.FirstName + ' '+emp.LastName as empName,
                                (select CAST(FirstName + ' '+LastName + ', ' AS VARCHAR(200))  from Employee  where UserId in (select na.EmployeeId from NoteAssign na where CN.Id = na.NoteId) FOR XML PATH ('')) as AssignName
                                ,(select COUNT(*) from CustomerNote CN where CustomerId = '{0}' and CompanyId='{1}' {4} {5}) as TotalNoteCount
                                into #ReminderData
                                from CustomerNote CN
                                LEFT JOIN Lookup lkNType on lkNType.DataValue=CN.NoteType and lkNType.DataKey='NoteType' and CN.NoteType!='-1'
                                left join Employee emp 
                                on emp.UserId = CN.CreatedByUid
                                --left join Employee emp1 
                                --on emp1.UserId = na.EmployeeId
                                Where CN.CustomerId = '{0}' 
                                AND CN.CompanyId='{1}'
                                {4} {5}
                                
                                select * into #ReminderDataFilter from #ReminderData
								
								select TOP({3}) * from #ReminderDataFilter where Id not in (select TOP(@pagestart) Id from #ReminderData #rd order by #rd.IsPin desc, #rd.CreatedDate desc)
								 order by IsPin desc, CreatedDate desc
								 
                                drop table #ReminderData   
								drop table #ReminderDataFilter";

            try
            {
                sqlQuery = string.Format(sqlQuery, customerId, companyId, pageno, pagesize, SearchQuery,DateFilterQuery);
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
        //Not in use anymore

        public DataTable GetAllTaskNoteByTaskIdAndCompanyId(int TaskId, Guid ComId)
        {
            string sqlQuery = @"   
                    select tNote.*, 
                    case when emp.firstname != ''
                    then  emp.firstname + ' ' +emp.lastname 
                    else
                    cus.firstname +' '+ cus.lastname
                    end as AddedByText
                    from tasknote tNote 
                    left join employee emp 
                    on emp.UserId = tNote.AddedBy
                    left join Customer cus
                    on cus.CustomerId = tNote.AddedBy
                    where tNote.companyid = '{0}'
                    and tNote.taskid = {1}";
            try
            {
                sqlQuery = string.Format(sqlQuery, ComId, TaskId);
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

        public DataTable GetAllCustomerNoteByCustomerCompany(Guid companyId)
        {
            string sqlQuery = @"select cnote.*
                                from CustomerNote cnote
                                left join CustomerCompany cus
                                on cnote.CustomerId=cus.CustomerId
                                and cus.CompanyId='{0}'
                                where cus.IsLead=1";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId);
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


        public DataTable GetAllLeadsFollowUpByCompanyId(Guid companyId,Guid CustomerId)
        {
            string sqlQuery = @"select CN.* , (select CAST(FirstName + ' '+LastName + ', ' AS VARCHAR(200))  from Employee  where UserId in (select na.EmployeeId from NoteAssign na where CN.Id = na.NoteId) FOR XML PATH ('')) as AssignName,
                                emp1.FirstName + ' ' + emp1.LastName as empName,
								lkNType.AlterDisplayText as Color,
                                lkNType.DisplayText as NoteTypeValue
                                from CustomerNote CN
                                left join CustomerCompany CC 
                                ON CN.CustomerId = CC.CustomerId
								left join NoteAssign na on na.NoteId = CN.Id
								left join Employee emp on emp.UserId = na.EmployeeId
                                left join Employee emp1 on emp1.UserId = CN.CreatedByUid
                                LEFT JOIN Lookup lkNType on lkNType.DataValue=CN.NoteType and lkNType.DataKey='NoteType'  and CN.NoteType!='-1'                                
                                WHERE CC.IsLead = 1 AND CN.CompanyId = '{0}' AND CN.CustomerId = '{1}'
                                group by CN.Id, CN.CompanyId, CN.CreatedBy,CN.CreatedByUid,CN.CreatedDate,CN.CustomerId,CN.IsActive,CN.IsAllDay,CN.IsClose,CN.IsEmail,CN.IsFollowUp,CN.IsShedule,CN.IsText,CN.Notes,CN.ReminderDate,CN.ReminderEndDate,CN.NoteType,lkNType.AlterDisplayText,lkNType.DisplayText,emp1.FirstName + ' ' + emp1.LastName,CN.IsPin, CN.IsOverview, CN.OrderBy
                                order by IsPin desc, CreatedDate desc
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery,companyId,CustomerId);
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
        //This function is Not in use 
        public DataTable GetAllCustomerFollowUpByCompanyId(Guid companyId,Guid CustomerId)
        {
            string sqlQuery = @"select CN.*
                                from CustomerNote CN
                                left join CustomerCompany CC
                                on CN.CustomerId = CC.CustomerId
                                WHERE CC.IsLead = 0 
                                AND CN.IsShedule = 1 
                                AND CN.CompanyId = '{0}' 
                                AND CN.CustomerId ='{1}'";
         
            try
            {
                sqlQuery = string.Format(sqlQuery,companyId,CustomerId);
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
        //This function is Not in use 

        public DataTable GetDashBoardReminderFollowUpsData(Guid companyId, string emptag, Guid empid)
        {
            string sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                            Set @CompanyId = '{0}'
	                    
                            select  
                            cn.Notes as Note,
                            cn.ReminderDate as ReminderDate,
                            cn.CreatedDate as CreatedDate,
                            cus.FirstName + ' '+cus.MiddleName +' ' + cus.LastName as CustomerName,
                            cus.BusinessName as BusinessName,
                            'Reminder' as ReminderType, cc.IsLead as UserType, cn.IsEmail as Email, cn.IsText as Phone, cus.Id as Customerid, cn.Id as noteid,
                             ISNULL(  emp.FirstName + ' '+emp.LastName, '')     as AssignUser

                            from CustomerNote cn
                            left join Customer cus on cus.CustomerId = cn.CustomerId
                            left join CustomerCompany cc on cus.CustomerId = cc.CustomerId
                            left join NoteAssign na on na.NoteId = cn.Id
                            left join Employee emp on emp.UserId = na.EmployeeId
                             Where cc.CompanyId=@CompanyId AND cn.CustomerId is not null 
	                    and cn.IsShedule = 1
                        and cn.IsClose = 0   {1}
						 order by cn.ReminderDate asc";
            string subquery = "";
            if(!string.IsNullOrWhiteSpace(emptag) && emptag.ToLower().IndexOf("admin") == -1)
            {
                subquery = string.Format(" AND emp.UserId in ('{0}')", empid);
            }
            
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, subquery);
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

        public DataTable GetDashBoardCurrentUserReminderFollowUpsData(Guid companyId, Guid empid, string seletActive)
        {
            string sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                            Set @CompanyId = '{0}'
	                    
                            select  
                            cn.Notes as Note,
                            cn.ReminderDate as ReminderDate,
                            cn.ReminderEndDate,
                            cn.CreatedDate as CreatedDate,
                            cus.FirstName + ' '+cus.MiddleName +' ' + cus.LastName as CustomerName,
                            cus.BusinessName as BusinessName,
                            'Reminder' as ReminderType, cc.IsLead as UserType, cn.IsEmail as Email, cn.IsText as Phone, cus.Id as Customerid, cn.Id as noteid,
                             ISNULL(  emp.FirstName + ' '+emp.LastName, '')     as AssignUser

                            from CustomerNote cn
                            left join Customer cus on cus.CustomerId = cn.CustomerId
                            left join CustomerCompany cc on cus.CustomerId = cc.CustomerId
                            left join NoteAssign na on na.NoteId = cn.Id
                            left join Employee emp on emp.UserId = na.EmployeeId
                             Where cc.CompanyId=@CompanyId AND cn.CustomerId is not null 
	                    {2}
                        and emp.UserId='{1}'
						 order by cn.ReminderDate asc";
            string subquery = "";
            if (!string.IsNullOrWhiteSpace(seletActive))
            {
                subquery = string.Format(" AND cn.IsShedule in ({0})", seletActive);
            }
            else
            {
                subquery = string.Format(" AND cn.IsShedule = 1");
            }

            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, empid, subquery);
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

        public DataTable GetCountCustomerNoteByCompanyId(Guid companyId, string startdate, string enddate)
        {
            string sqlQuery = @"select COUNT(*) ReminderCount from CustomerNote 
                                where CompanyId = '{0}'
                                and (ReminderDate > '{1}' and  ReminderDate < '{2}'
                                or ReminderEndDate > '{1}' and  ReminderEndDate < '{2}')";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, startdate, enddate);
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
