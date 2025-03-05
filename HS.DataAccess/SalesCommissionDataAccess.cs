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
    public partial class SalesCommissionDataAccess
    {
        public bool UpdatePayrollClusterFunding(string ticketIdJoin, int batch, DateTime PaidDate)
        {
            string sqlQuery = @"Declare @batch nvarchar(50)
                                set @batch='{0}'
                                Declare @paiddate datetime
                                set @paiddate='{1}'

                                --Sales Commission
                                Update SalesCommission
                                Set IsPaid=1,
                                Batch=CASE
                                When Ce.Batch IS NULL or ce.Batch = '' THEN @batch
                                ELSE ce.Batch
                                END,
                                PaidDate=@paiddate
                                from SalesCommission sc
                                LEFT JOIN ticket tk on tk.TicketId=sc.TicketId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=tk.CustomerId
                                Where tk.Id in({2}) and sc.IsPaid=0

                                --Tech Commission
                                Update TechCommission
                                Set IsPaid=1,
                                Batch=CASE
                                When Ce.Batch IS NULL or ce.Batch = '' THEN @batch
                                ELSE ce.Batch
                                END,
                                PaidDate=@paiddate
                                from TechCommission tc
                                LEFT JOIN ticket tk on tk.TicketId=tc.TicketId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=tk.CustomerId
                                Where tk.Id in({2}) and tc.IsPaid=0

                                --AddMember Commission
                                Update AddMemberCommission
                                Set IsPaid=1,
                                Batch=CASE
                                When Ce.Batch IS NULL or ce.Batch = '' THEN @batch
                                ELSE ce.Batch
                                END,
                                PaidDate=@paiddate
                                from AddMemberCommission amc
                                LEFT JOIN ticket tk on tk.TicketId=amc.TicketId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=tk.CustomerId
                                Where tk.Id in({2}) and amc.IsPaid=0

                                --FinRep Commission
                                Update FinRepCommission
                                Set IsPaid=1,
                                Batch=CASE
                                When Ce.Batch IS NULL or ce.Batch = '' THEN @batch
                                ELSE ce.Batch
                                END,
                                PaidDate=@paiddate
                                from FinRepCommission frc
                                LEFT JOIN ticket tk on tk.TicketId=frc.TicketId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=tk.CustomerId
                                Where tk.Id in({2}) and frc.IsPaid=0

                                --ServiceCall Commission
                                Update ServiceCallCommission
                                Set IsPaid=1,
                                Batch=CASE
                                When Ce.Batch IS NULL or ce.Batch = '' THEN @batch
                                ELSE ce.Batch
                                END,
                                PaidDate=@paiddate
                                from ServiceCallCommission scc
                                LEFT JOIN ticket tk on tk.TicketId=scc.TicketId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=tk.CustomerId
                                Where tk.Id in({2}) and scc.IsPaid=0

                                --FollowUp Commission
                                Update FollowUpCommission
                                Set IsPaid=1,
                                Batch=CASE
                                When Ce.Batch IS NULL or ce.Batch = '' THEN @batch
                                ELSE ce.Batch
                                END,
                                PaidDate=@paiddate
                                from FollowUpCommission fuc
                                LEFT JOIN ticket tk on tk.TicketId=fuc.TicketId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=tk.CustomerId
                                Where tk.Id in({2}) and fuc.IsPaid=0

                                --Reschedule Commission
                                Update RescheduleCommission
                                Set IsPaid=1,
                                Batch=CASE
                                When Ce.Batch IS NULL or ce.Batch = '' THEN @batch
                                ELSE ce.Batch
                                END,
                                PaidDate=@paiddate
                                from RescheduleCommission resc
                                LEFT JOIN ticket tk on tk.TicketId=resc.TicketId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=tk.CustomerId
                                Where tk.Id in({2}) and resc.IsPaid=0

                                --CustomerExtended Commission
                                Update CustomerExtended
                                Set Batch=CASE
                                When Ce.Batch IS NULL or ce.Batch = '' THEN @batch
                                ELSE ce.Batch
                                END
                                from CustomerExtended ce
                                LEFT JOIN Ticket tk on tk.CustomerId=ce.CustomerId
                                Where (tk.TicketType='Installation' or tk.TicketType='Fire') and tk.Id in({2})

                                --Customer Commission
                                Update Customer
                                Set CustomerFunded=1, CustomerFundedDate='{3}'
                                from Customer cus
                                LEFT JOIN ticket tk on tk.CustomerId=cus.CustomerId
                                Where (tk.TicketType='Installation' or tk.TicketType='Fire') and tk.Id in({2})
                                ";

            try
            {
                sqlQuery = string.Format(sqlQuery, batch, PaidDate, ticketIdJoin, DateTime.Now);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();

                    return true;
                }

            }
            catch (Exception ee)
            {
                return false;
            }

        }
        public bool UpdatePayrollSingleFunding(string idSales, string idTech, string idAddMember, string idFinRep, string idServiceCall, string idFollowUp, string idReshcedule, string idAdjustmentFunding, int batch, DateTime PaidDate)
        {
            if (string.IsNullOrEmpty(idSales))
            {
                idSales = "0";
            }
            if (string.IsNullOrEmpty(idTech))
            {
                idTech = "0";
            }
            if (string.IsNullOrEmpty(idAddMember))
            {
                idAddMember = "0";
            }
            if (string.IsNullOrEmpty(idFinRep))
            {
                idFinRep = "0";
            }
            if (string.IsNullOrEmpty(idServiceCall))
            {
                idServiceCall = "0";
            }
            if (string.IsNullOrEmpty(idFollowUp))
            {
                idFollowUp = "0";
            }
            if (string.IsNullOrEmpty(idReshcedule))
            {
                idReshcedule = "0";
            }
            if (string.IsNullOrEmpty(idAdjustmentFunding))
            {
                idAdjustmentFunding = "0";
            }
            string sqlQuery = @"Declare @batch nvarchar(50)
                                set @batch='{0}'
                                Declare @paiddate datetime
                                set @paiddate='{1}'

                                --Sales Commission
                                Update SalesCommission
                                Set IsPaid=1,
                                Batch=CASE
                                When Ce.Batch IS NULL or ce.Batch = '' THEN @batch
                                ELSE ce.Batch
                                END,
                                PaidDate=@paiddate
                                from SalesCommission sc
                                LEFT JOIN ticket tk on tk.TicketId=sc.TicketId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=tk.CustomerId
                                Where sc.Id in({2}) and sc.IsPaid=0

                                --Tech Commission
                                Update TechCommission
                                Set IsPaid=1,
                                Batch=CASE
                                When Ce.Batch IS NULL or ce.Batch = '' THEN @batch
                                ELSE ce.Batch
                                END,
                                PaidDate=@paiddate
                                from TechCommission tc
                                LEFT JOIN ticket tk on tk.TicketId=tc.TicketId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=tk.CustomerId
                                Where tc.Id in({3}) and tc.IsPaid=0

                                --AddMember Commission
                                Update AddMemberCommission
                                Set IsPaid=1,
                                Batch=CASE
                                When Ce.Batch IS NULL or ce.Batch = '' THEN @batch
                                ELSE ce.Batch
                                END,
                                PaidDate=@paiddate
                                from AddMemberCommission amc
                                LEFT JOIN ticket tk on tk.TicketId=amc.TicketId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=tk.CustomerId
                                Where amc.Id in({4}) and amc.IsPaid=0

                                --FinRep Commission
                                Update FinRepCommission
                                Set IsPaid=1,
                                Batch=CASE
                                When Ce.Batch IS NULL or ce.Batch = '' THEN @batch
                                ELSE ce.Batch
                                END,
                                PaidDate=@paiddate
                                from FinRepCommission frc
                                LEFT JOIN ticket tk on tk.TicketId=frc.TicketId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=tk.CustomerId
                                Where frc.Id in({9}) and frc.IsPaid=0

                                --ServiceCall Commission
                                Update ServiceCallCommission
                                Set IsPaid=1,
                                Batch=CASE
                                When Ce.Batch IS NULL or ce.Batch = '' THEN @batch
                                ELSE ce.Batch
                                END,
                                PaidDate=@paiddate
                                from ServiceCallCommission scc
                                LEFT JOIN ticket tk on tk.TicketId=scc.TicketId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=tk.CustomerId
                                Where scc.Id in({5}) and scc.IsPaid=0

                                --FollowUp Commission
                                Update FollowUpCommission
                                Set IsPaid=1,
                                Batch=CASE
                                When Ce.Batch IS NULL or ce.Batch = '' THEN @batch
                                ELSE ce.Batch
                                END,
                                PaidDate=@paiddate
                                from FollowUpCommission fuc
                                LEFT JOIN ticket tk on tk.TicketId=fuc.TicketId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=tk.CustomerId
                                Where fuc.Id in({6}) and fuc.IsPaid=0

                                --Reschedule Commission
                                Update RescheduleCommission
                                Set IsPaid=1,
                                Batch=CASE
                                When Ce.Batch IS NULL or ce.Batch = '' THEN @batch
                                ELSE ce.Batch
                                END,
                                PaidDate=@paiddate
                                from RescheduleCommission resc
                                LEFT JOIN ticket tk on tk.TicketId=resc.TicketId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=tk.CustomerId
                                Where resc.Id in({7}) and resc.IsPaid=0

                                --AdjustmentFunding Commission
                                Update AdjustmentFunding
                                Set IsPaid=1,
                                Batch=@batch,
                                PaidDate=@paiddate
                                from AdjustmentFunding afc
                                Where afc.Id in({8}) and afc.IsPaid=0

                                --CustomerExtended Commission
                                Update CustomerExtended
                                Set Batch=CASE
                                When Ce.Batch IS NULL or ce.Batch = '' THEN @batch
                                ELSE ce.Batch
                                END
                                from CustomerExtended ce
                                LEFT JOIN Ticket tk on tk.CustomerId=ce.CustomerId
								LEFT JOIN SalesCommission sc on sc.TicketId=tk.TicketId
                                Where (tk.TicketType='Installation' or tk.TicketType='Fire') and sc.Id in({2})
								
								Update CustomerExtended
                                Set Batch=CASE
                                When Ce.Batch IS NULL or ce.Batch = '' THEN @batch
                                ELSE ce.Batch
                                END
                                from CustomerExtended ce
                                LEFT JOIN Ticket tk on tk.CustomerId=ce.CustomerId
								LEFT JOIN TechCommission tc on tc.TicketId=tk.TicketId
                                Where (tk.TicketType='Installation' or tk.TicketType='Fire') and tc.Id in({3})

								Update CustomerExtended
                                Set Batch=CASE
                                When Ce.Batch IS NULL or ce.Batch = '' THEN @batch
                                ELSE ce.Batch
                                END
                                from CustomerExtended ce
                                LEFT JOIN Ticket tk on tk.CustomerId=ce.CustomerId
								LEFT JOIN AddMemberCommission amc on amc.TicketId=tk.TicketId
                                Where (tk.TicketType='Installation' or tk.TicketType='Fire') and amc.Id in({4})

                                Update CustomerExtended
                                Set Batch=CASE
                                When Ce.Batch IS NULL or ce.Batch = '' THEN @batch
                                ELSE ce.Batch
                                END
                                from CustomerExtended ce
                                LEFT JOIN Ticket tk on tk.CustomerId=ce.CustomerId
								LEFT JOIN FinRepCommission frc on frc.TicketId=tk.TicketId
                                Where (tk.TicketType='Installation' or tk.TicketType='Fire') and frc.Id in({9})

								Update CustomerExtended
                                Set Batch=CASE
                                When Ce.Batch IS NULL or ce.Batch = '' THEN @batch
                                ELSE ce.Batch
                                END
                                from CustomerExtended ce
                                LEFT JOIN Ticket tk on tk.CustomerId=ce.CustomerId
								LEFT JOIN ServiceCallCommission scc on scc.TicketId=tk.TicketId
                                Where (tk.TicketType='Installation' or tk.TicketType='Fire') and scc.Id in({5})

								Update CustomerExtended
                                Set Batch=CASE
                                When Ce.Batch IS NULL or ce.Batch = '' THEN @batch
                                ELSE ce.Batch
                                END
                                from CustomerExtended ce
                                LEFT JOIN Ticket tk on tk.CustomerId=ce.CustomerId
								LEFT JOIN FollowUpCommission fuc on fuc.TicketId=tk.TicketId
                                Where (tk.TicketType='Installation' or tk.TicketType='Fire') and fuc.Id in({6})

								Update CustomerExtended
                                Set Batch=CASE
                                When Ce.Batch IS NULL or ce.Batch = '' THEN @batch
                                ELSE ce.Batch
                                END
                                from CustomerExtended ce
                                LEFT JOIN Ticket tk on tk.CustomerId=ce.CustomerId
								LEFT JOIN RescheduleCommission rc on rc.TicketId=tk.TicketId
                                Where (tk.TicketType='Installation' or tk.TicketType='Fire') and rc.Id in({7})

                                --Customer Commission
                                Update Customer
                                Set CustomerFunded=1, CustomerFundedDate='{10}'
                                from Customer cus
                                LEFT JOIN ticket tk on tk.CustomerId=cus.CustomerId
                                LEFT JOIN SalesCommission sc on sc.TicketId=tk.TicketId
                                Where (tk.TicketType='Installation' or tk.TicketType='Fire') and sc.Id in({2})

								Update Customer
                                Set CustomerFunded=1, CustomerFundedDate='{10}'
                                from Customer cus
                                LEFT JOIN ticket tk on tk.CustomerId=cus.CustomerId
								LEFT JOIN TechCommission tc on tc.TicketId=tk.TicketId
                                Where (tk.TicketType='Installation' or tk.TicketType='Fire') and tc.Id in({3})

								Update Customer
                                Set CustomerFunded=1, CustomerFundedDate='{10}'
                                from Customer cus
                                LEFT JOIN ticket tk on tk.CustomerId=cus.CustomerId
								LEFT JOIN AddMemberCommission amc on amc.TicketId=tk.TicketId
                                Where (tk.TicketType='Installation' or tk.TicketType='Fire') and amc.Id in({4})

                                Update Customer
                                Set CustomerFunded=1, CustomerFundedDate='{10}'
                                from Customer cus
                                LEFT JOIN ticket tk on tk.CustomerId=cus.CustomerId
								LEFT JOIN FinRepCommission frc on frc.TicketId=tk.TicketId
                                Where (tk.TicketType='Installation' or tk.TicketType='Fire') and frc.Id in({9})

								Update Customer
                                Set CustomerFunded=1, CustomerFundedDate='{10}'
                                from Customer cus
                                LEFT JOIN ticket tk on tk.CustomerId=cus.CustomerId
								LEFT JOIN ServiceCallCommission scc on scc.TicketId=tk.TicketId
                                Where (tk.TicketType='Installation' or tk.TicketType='Fire') and scc.Id in({5})

								Update Customer
                                Set CustomerFunded=1, CustomerFundedDate='{10}'
                                from Customer cus
                                LEFT JOIN ticket tk on tk.CustomerId=cus.CustomerId
								LEFT JOIN FollowUpCommission fuc on fuc.TicketId=tk.TicketId
                                Where (tk.TicketType='Installation' or tk.TicketType='Fire') and fuc.Id in({6})

								Update Customer
                                Set CustomerFunded=1, CustomerFundedDate='{10}'
                                from Customer cus
                                LEFT JOIN ticket tk on tk.CustomerId=cus.CustomerId
                                LEFT JOIN RescheduleCommission rc on rc.TicketId=tk.TicketId
                                Where (tk.TicketType='Installation' or tk.TicketType='Fire') and rc.Id in({7})
                                ";

            try
            {
                sqlQuery = string.Format(sqlQuery, batch, PaidDate, idSales, idTech, idAddMember, idServiceCall, idFollowUp, idReshcedule, idAdjustmentFunding, idFinRep, DateTime.Now);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();

                    return true;
                }

            }
            catch (Exception ee)
            {
                return false;
            }

        }
        public DataTable GetSalesCommissionByTicketId(Guid ticketId, Guid CommissionUserId)
        {
            string subQUery = "";
            string sqlQuery = @"select 
                                sc.Id,
                                em.FirstName+' '+em.LastName SalesPerson,
                                sc.RMRSold,
                                sc.RMRCommission,
								sc.RMRCommissionCalculation,
                                sc.NoOfEquipment,
                                sc.EquipmentCommission,
								sc.EquipmentCommissionCalculation,
                                sc.TotalCommission,
                                sc.Adjustment,
                                sc.SalesCommissionId,
                                sc.OriginalPoint,
								sc.AdjustablePoint
                                from SalesCommission sc
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

        public int GetLastBatchNo()
        {
            string sqlQuery = @"SELECT * INTO #ASD FROM
                                (select TRY_PARSE([Value] as int) as Value 
	                                from GlobalSetting 
	                                where SearchKey = 'InitialBatchNO'
                                UNION
	                                SELECT TRY_PARSE([Batch] as int) as Value  
	                                FROM TechCommission
                                UNION
	                                SELECT TRY_PARSE([Batch] as int) as Value  
	                                FROM SalesCommission
                                UNION
	                                SELECT TRY_PARSE([Batch] as int) as Value  
	                                FROM AddMemberCommission
                                UNION
	                                SELECT TRY_PARSE([Batch] as int) as Value  
	                                FROM ServiceCallCommission
                                UNION
	                                SELECT TRY_PARSE([Batch] as int) as Value  
	                                FROM FollowUpCommission
                                UNION
	                                SELECT TRY_PARSE([Batch] as int) as Value  
	                                FROM RescheduleCommission
                                UNION
	                                SELECT Batch as Value  
	                                FROM AdjustmentFunding
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

        public int GetLastTechBatchNo()
        {
            string sqlQuery = @"SELECT * INTO #ASD FROM
                                (select TRY_PARSE([Value] as int) as Value 
	                                from GlobalSetting 
	                                where SearchKey = 'InitialBatchNO'
                                UNION
	                                SELECT TRY_PARSE([Batch] as int) as Value  
	                                FROM TechCommission 
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

        public int GetLastSalesBatchNo()
        {
            string sqlQuery = @"SELECT * INTO #ASD FROM
                                (select TRY_PARSE([Value] as int) as Value 
	                                from GlobalSetting 
	                                where SearchKey = 'InitialBatchNO' 
                                UNION
	                                SELECT TRY_PARSE([Batch] as int) as Value  
	                                FROM SalesCommission
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

        public bool DeleteAllCommissionByCustomerIdAndTicketId(Guid customerId, Guid ticketId)
        {
            string SqlQuery = @"delete from SalesCommission 
                                where TicketId = '{0}' and CustomerId = '{1}'

                                delete from TechCommission
                                where TicketId = '{0}' and CustomerId = '{1}'

                                delete from RescheduleCommission 
                                where TicketId = '{0}' and CustomerId = '{1}'

                                delete from ServiceCallCommission
                                where TicketId = '{0}' and CustomerId = '{1}'

                                delete from FollowUpCommission 
                                where TicketId = '{0}' and CustomerId = '{1}'

                                delete from AddMemberCommission
                                where TicketId = '{0}' and CustomerId = '{1}' ";
            try
            {
                SqlQuery = string.Format(SqlQuery, ticketId, customerId);

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
        public bool DeleteExtraSalesCommission(Guid ticketId, string subquery)
        {
            string SqlQuery = @"delete from SalesCommission where TicketId='{0}' and UserId not in ({1}) and IsPermanent!=1 and RMRSold is Not NULL And NoOfEquipment is Not NULL";
            SqlQuery = string.Format(SqlQuery, ticketId, subquery);
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
        public bool DeleteSalesCommissionByTicketId(Guid ticketId)
        {
            string SqlQuery = @"delete from SalesCommission where TicketId='{0}'";
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
