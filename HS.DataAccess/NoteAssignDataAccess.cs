using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.DataAccess
{
    public partial class NoteAssignDataAccess
    {
        public NoteAssignDataAccess(string ConnectionString):base(ConnectionString){}
        public NoteAssignDataAccess() { }
        public bool ReseedNoteAssignTable()
        {
            string SqlQuery = @"
                                Delete from NoteAssign
                                DBCC CHECKIDENT('NoteAssign', RESEED, 0) 
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
        public bool DeleteAllAssignNoteByNoteId(int noteid)
        {
            string sqlQuery = @"delete
                                from NoteAssign
                                where NoteId='{0}'";

            try
            {
                sqlQuery = string.Format(sqlQuery, noteid);

                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    return ExecuteCommand(cmd) > 0;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public DataTable GetAllEmployeeNameeByEmployeeId(Guid employeeid)
        {
            string sqlQuery = @"select emp.FirstName+' '+emp.LastName as AssignName
                                from NoteAssign na
                                inner join Employee emp
                                on na.EmployeeId=emp.UserId
                                where emp.UserId='{0}'
                                ";

            try
            {
                sqlQuery = string.Format(sqlQuery, employeeid);
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
        public DataTable GetDisplayTextByDataValue(string Datavalue)
        {
            string sqlQuery = @"select * from lookup where DataKey = 'TicketScheduleTime' and DataValue = '{0}'
                                ";

            try
            {
                sqlQuery = string.Format(sqlQuery, Datavalue);
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
        public DataTable GetAllAssignByCustomerIdAndCompanyId(Guid customerId, Guid companyId)
        {
            string sqlQuery = @"
                                select na.*,emp.FirstName + ' '+emp.LastName as AssignName 
                                from NoteAssign na
                                left join CustomerNote cusn on cusn.Id = na.NoteId
                                left join Employee emp on na.EmployeeId = emp.UserId
                                where cusn.CompanyId = '{1}'
                                and cusn.CustomerId ='{0}'
                                ";

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

        public DataTable GetAllAssignCustomerNoteListByNoteId(int NoteId)
        {
            string sqlQuery = @"Select * from NoteAssign 
                                where NoteAssign.NoteId = '{0}'";

            try
            {
                sqlQuery = string.Format(sqlQuery, NoteId);
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

        public bool CheckAndDeleteOldAssignedEmployee(int NoteId)
        {
            string sqlQuery = @"DELETE FROM NoteAssign
                    WHERE NoteId = {0};";
            try
            {
                sqlQuery = string.Format(sqlQuery, NoteId);

                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    return ExecuteCommand(cmd) > 0;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
