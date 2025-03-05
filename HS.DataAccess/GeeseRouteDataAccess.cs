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
	public partial class GeeseRouteDataAccess
	{
        public DataTable GetRouteList()
        {
            string sqlQuery = @"select Name, RouteId from GeeseRoute";

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
        public bool DeleteRouteByRouteId(Guid RouteId)
        {
            string SqlQuery = @"delete from GeeseRoute where RouteId ='{0}'

                                delete from CustomerRoute where RouteId = '{0}'";
            SqlQuery = string.Format(SqlQuery, RouteId);
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

        public bool UpdateCustomerRouteByRouteId(Guid RouteId, string Name)
        {
            string SqlQuery = @"Update CustomerRoute set Name ='{0}' where RouteId ='{1}'";
            SqlQuery = string.Format(SqlQuery, Name, RouteId);
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
        public DataTable GetTotalRouteCountByUserId(Guid UserId)
        {
            string sqlQuery = "";
            if(UserId != new Guid())
            {
                sqlQuery = string.Format("select count(*) as TotalRoute from GeeseRoute GR Left Join EmployeeRoute ER on ER.RouteId = GR.RouteId Where ER.UserId = '{0}'", UserId);
            }
            else
            {
                 sqlQuery = @"select count(*) as TotalRoute from GeeseRoute GR";
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
    }	
}
