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
	public partial class EmployeeLeadSourceDataAccess
	{
        public bool DeleteEmployeeLeadSourceByUserId(Guid userId)
        {
            string sqlQuery = @"delete from EmployeeLeadSource where EmployeeId = '{0}'";
            sqlQuery = string.Format(sqlQuery, userId);
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
    }	
}
