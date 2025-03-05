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
	public partial class CustomerCheckLogDataAccess
	{
        public CustomerCheckLogDataAccess(string ConnectionString) : base(ConnectionString)
		{

        }
        public DataTable GetTotalCheckInCountByUserId(Guid UserId, DateTime Today, DateTime EndDay)
        {
            string AddQuery = "";
            if (UserId != new Guid())
            {
                AddQuery = string.Format("and CL.UserId = '{0}'", UserId);
            }

            string sqlQuery = @"select count(*) as TotalCheckIn from CustomerCheckLog CL
                                Where CheckInTime between '{0}' and '{1}'
                                {2}";

            try
            {
                sqlQuery = string.Format(sqlQuery, EndDay, Today, AddQuery);
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
        public DataTable GetTotalGeeseCount(Guid UserId, DateTime Today, DateTime EndDay)
        {
            string AddQuery = "";
            if (UserId != new Guid())
            {
                AddQuery = string.Format("and CL.UserId = '{0}'", UserId);
            }

            string sqlQuery = @"select sum(GeeseCount) as TotalGeese from CustomerCheckLog CL
                                Where CheckInTime between '{0}' and '{1}'
                                {2}";

            try
            {
                sqlQuery = string.Format(sqlQuery, EndDay, Today, AddQuery);
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
