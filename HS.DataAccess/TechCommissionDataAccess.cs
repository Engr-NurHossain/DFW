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
	public partial class TechCommissionDataAccess
	{
        public DataTable GetTechCommissionByTicketId(Guid ticketId, Guid CommissionUserId)
        {
            string subQUery = "";
            string sqlQuery = @"select 
                                sc.Id,
                                em.FirstName+' '+em.LastName Technician,
                                sc.BaseRMR,
                                sc.BaseRMRCommission,
                                sc.BaseRMRCommissionCalculation,
                                sc.AddedRMR,
                                sc.AddedRMRCommission,
                                sc.AddedRMRCommissionCalculation,
                                sc.TotalCommission,
                                sc.TechCommissionId,
                                sc.Adjustment,
                                sc.OriginalPoint,
                                sc.AdjustablePoint
                                from TechCommission sc
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
        public bool DeleteExtraTechCommission(Guid ticketId, string userId)
        {
            string SqlQuery = @"delete from TechCommission where TicketId='{0}' and UserId not in {1} and BaseRMR is Not NULL And AddedRMR is Not NULL";
            SqlQuery = string.Format(SqlQuery, ticketId, userId);
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
