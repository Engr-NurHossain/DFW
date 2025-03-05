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
	public partial class FinRepCommissionDataAccess
	{
        public DataTable GetFinRepCommissionByTicketId(Guid ticketId,Guid CommissionUserId)
        {
            string subQUery = "";
            string sqlQuery = @"select 
                                fr.Id,
                                fr.UserId,
                                em.FirstName+' '+em.LastName FinanceRep,
                                fr.CommissionCalculation,
                                fr.Commission,
                                fr.FinRepCommissionId,
                                fr.Adjustment,
                                fr.OriginalPoint,
                                fr.AdjustablePoint,
                                fr.OriginalPoint,
                                fr.AdjustablePoint
                                from FinRepCommission fr
                                LEFT JOIN Employee em on em.UserId=fr.UserId
                                Where fr.TicketId='{0}' {1}";
            if (CommissionUserId != Guid.Empty)
            {
                subQUery = string.Format(" and fr.UserId='{0}'", CommissionUserId);
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
    }	
}
