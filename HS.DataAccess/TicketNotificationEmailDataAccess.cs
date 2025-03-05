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
	public partial class TicketNotificationEmailDataAccess
	{
        public DataTable GetAllTicketNotificationEmailList()
        {
            string sqlQuery = @"select tn.*,lk.DisplayText as TicketStatusVal from ticketnotificationemail tn
                                left join Lookup lk on lk.DataValue = tn.TicketStatus and lk.DataKey = 'TicketStatus'";
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

        public DataTable GetTicketNotificationEmailByTicketStatus(string ticketStatus)
        {
            string sqlQuery = @"select tn.*,lk.DisplayText as TicketStatusVal from ticketnotificationemail tn
                                left join Lookup lk on lk.DataValue = tn.TicketStatus and lk.DataKey = 'TicketStatus'
                                where tn.TicketStatus = '{0}'
                                    ";
            try
            {
                sqlQuery = string.Format(sqlQuery, ticketStatus);
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
