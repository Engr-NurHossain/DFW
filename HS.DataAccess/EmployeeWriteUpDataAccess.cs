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
	public partial class EmployeeWriteUpDataAccess
	{
        public DataTable GetEmployeeWriteUpByUserId(Guid UserId)
        {
            string sqlQuery = @"select wp.*, 

                                emp.FirstName+' '+emp.LastName as SupervisorName 
                              

                                from EmployeeWriteUp wp
                               
                                left join Employee emp on emp.UserId = wp.Supervisor

                                where wp.UserId = '{0}'";


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
    }	
}
