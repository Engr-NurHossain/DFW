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
	public partial class ServiceCallCommissionDataAccess
	{
        public DataTable GetServiceCallCommissionByTicketId(Guid ticketId, Guid CommissionUserId)
        {
            string subQUery = "";
            string sqlQuery = @"select 
                                sc.Id,
                                sc.UserId,
                                em.FirstName+' '+em.LastName Technician,
                                sc.CommissionCalculation,
                                sc.Commission,
                                sc.ServiceCallCommissionId,
                                sc.Adjustment
                                from ServiceCallCommission sc
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
        public bool DeleteServiceCallCommissionByTicketId(Guid ticketId)
        {
            string SqlQuery = @"delete from ServiceCallCommission where TicketId ='{0}' and IsManual != 1";
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

        public int GetLastServiceCallBatchNo()
        {
            string sqlQuery = @"SELECT * INTO #ASD FROM
                                (select TRY_PARSE([Value] as int) as Value 
	                                from GlobalSetting 
	                                where SearchKey = 'InitialBatchNO'
                                UNION
	                                SELECT TRY_PARSE([Batch] as int) as Value  
	                                FROM ServiceCallCommission 
                                )  A 
                                SELECT MAX(Value)+1 AS Value FROM #ASD 
                                DROP TABLE #ASD ";
            try
            {
                sqlQuery = string.Format(sqlQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return int.Parse(dsResult.Tables[0].Rows[0]["Value"].ToString());
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }	
}
