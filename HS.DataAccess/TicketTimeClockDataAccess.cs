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
	public partial class TicketTimeClockDataAccess
	{
        public TicketTimeClock GetLastClockInByUserIdAndTicketId(Guid UserId, Guid TicketId)
        {
            string sqlQuery = @"select top(1) * from TicketTimeClock 
                                where TicketId = '{1}'
                                and UserId ='{0}' and Type = 'start'
                                order by [Id] desc";
            sqlQuery = string.Format(sqlQuery, UserId, TicketId);

            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    return GetObject(cmd);
                }

            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable GetLastClockOutByUserIdAndTicketId(Guid UserId, Guid TicketId)
        {
            string sqlQuery = @"select * from TicketTimeClock 
                                where TicketId = '{1}'
                                and UserId ='{0}' and Type = 'end'
                                order by [Id] desc";
            sqlQuery = string.Format(sqlQuery, UserId, TicketId);

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
        public TicketTimeClock GetLastTicketTimeClockByTicketId(Guid ticketId)
        {
            string sqlQuery = @"select top(1) * from TicketTimeClock 
                                where TicketId = '{0}' order by id desc";
            sqlQuery = string.Format(sqlQuery,  ticketId);

            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    return GetObject(cmd);
                }

            }
            catch (Exception)
            {
                return null;
            }
        }
    }	
}
