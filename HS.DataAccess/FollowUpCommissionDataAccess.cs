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
	public partial class FollowUpCommissionDataAccess
	{
        public DataTable GetFollowUpCommissionByTicketId(Guid ticketId, Guid CommissionUserId)
        {
            string subQUery = "";
            string sqlQuery = @"select 
                                sc.Id,
                                sc.UserId,
                                em.FirstName+' '+em.LastName Technician,
                                sc.CommissionCalculation,
                                sc.Commission,
                                sc.FollowUpCommissionId,
                                sc.Adjustment
                                from FollowUpCommission sc
                                LEFT JOIN Employee em on em.UserId=sc.UserId
                                Where sc.TicketId='{0}' {1}";
            if (CommissionUserId != Guid.Empty)
            {
                subQUery = string.Format(" and sc.UserId='{0}'", CommissionUserId);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, ticketId, subQUery);
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
        public bool DeleteFollowUpCommissionByTicketId(Guid ticketId)
        {
            string SqlQuery = @"delete from FollowUpCommission where TicketId ='{0}' and IsManual != 1";
            SqlQuery = string.Format(SqlQuery, ticketId);
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
    }	
}
