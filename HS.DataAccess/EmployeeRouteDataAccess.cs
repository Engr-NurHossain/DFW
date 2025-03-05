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
	public partial class EmployeeRouteDataAccess
	{
        public bool DeleteEmployeeRouteByUserId(Guid UserId)
        {
            string SqlQuery = @"delete from EmployeeRoute where UserId ='{0}'";
            SqlQuery = string.Format(SqlQuery, UserId);
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
        public DataTable GetAllEmployeeRouteByUserId(Guid UserId)
        {
            string sqlQuery = @"select GR.Name, ER.RouteId
                                    from EmployeeRoute ER
                                    Left join GeeseRoute GR on GR.RouteId = ER.RouteId
	                                where ER.UserId = '{0}'";
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
