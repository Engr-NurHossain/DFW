using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;


using System.Linq;
namespace HS.DataAccess
{
    public partial class AddMemberCommissionDataAccess
    {
        public DataTable GetAddMemberCommissionByTicketId(Guid ticketId, Guid CommissionUserId)
        {
            string subQUery = "";
            string sqlQuery = @"select 
                                sc.Id,
                                sc.UserId,
                                em.FirstName+' '+em.LastName Technician,
                                sc.CommissionCalculation,
                                sc.Commission,
                                sc.AddMemberCommissionId,
                                sc.Adjustment,
                                sc.OriginalPoint,
                                sc.AdjustablePoint,
                                sc.OriginalPoint,
                                sc.AdjustablePoint
                                from AddMemberCommission sc
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


        public int GetLastMemberBatchNo()
        {
            string sqlQuery = @"SELECT * INTO #ASD FROM
                                (select TRY_PARSE([Value] as int) as Value 
	                                from GlobalSetting 
	                                where SearchKey = 'InitialBatchNO'
                                UNION
	                                SELECT TRY_PARSE([Batch] as int) as Value  
	                                FROM AddMemberCommission 
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
        public DataSet GetAllMemberCommReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, bool IsPaid, Guid UserId, string SearchText, string MemberList)
        {
            string DateFilter2 = "";
            string DateFilter1 = "";
            string IsPaidQuery = "";
            string techPayrollFilter = "";
            string SearchQuery = "";
            string FilterQuery = "";
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
                //NameSql = NameSql.Replace("cus.", "cu.");
            }

            if (!string.IsNullOrEmpty(SearchText))
            {
                SearchQuery = string.Format(" (TicketIdValue like @SearchText or  CustomerName like @SearchText or Technician like @SearchText or BalanceDue like @SearchText) and ");
            }

            if (!string.IsNullOrEmpty(MemberList) && MemberList != "'null'")
            {

                FilterQuery = string.Format(" UserId in ({0}) and ", MemberList);
            }


            if (UserId != new Guid())
            {
                techPayrollFilter = string.Format(" and mc.UserId = '{0}'", UserId);
            }
            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime())
            {

                FilterStartDate = FilterStartDate.SetZeroHour();
                FilterEndDate = FilterEndDate.SetMaxHour();
                DateFilter1 = string.Format("and mc.CreatedDate between '{0}' and '{1}'"
                    , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (IsPaid)
            {
                IsPaidQuery = "where mc.IsPaid = 1";
            }
            else
            {
                IsPaidQuery = "where (mc.IsPaid = 0 or mc.IsPaid is null)  ";
            }
            string sqlQuery = @"
                                declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                 select * into #MemberCom  from (select
                                  mc.*, 
                                  tk.Id as TicketIdValue,
								 {7} as CustomerName,
							     cus.Id as CustomerIdValue,
								 emp.FirstName +' '+emp.LastName as Technician,
								(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                From Invoice where CustomerId=cus.CustomerId
                                and (Status='Open' or Status='Partial')) as BalanceDue
 
                                from AddMemberCommission mc
                                left join Ticket tk on tk.TicketId = mc.TicketId
                                left join customer cus on cus.CustomerId = tk.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = mc.UserId
                                {5}{4}{6}  and tk.IsClosed = 1 and ce.IsTestAccount != 1
                                ) d	

                                 select * into #MemberComFilter
								from #MemberCom
                               

								select top(@pagesize)
								* into #TotalCount from #MemberComFilter
								where {8} {9} Id not in(select top(@pagestart) Id from #MemberComFilter #e {2})
                                {3}
								select count(*) CountTotal
                                from #MemberComFilter
                                where {8} {9} Id>0

								select * from #TotalCount
								select sum(Adjustment) as TotalAdjustment
								,sum(Commission) as SumCommission
								,sum(BalanceDue) as SumUnpaid
								from #TotalCount

								drop table #MemberCom
								drop table #MemberComFilter
								drop table #TotalCount";

            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/adjustment")
                {
                    subquery = "order by #e.Adjustment asc";
                    subquery1 = "order by Adjustment asc";
                }
                else if (order == "descending/adjustment")
                {
                    subquery = "order by #e.Adjustment desc";
                    subquery1 = "order by Adjustment desc";
                }
                else if (order == "ascending/ticketid")
                {
                    subquery = "order by #e.Id asc";
                    subquery1 = "order by Id asc";
                }
                else if (order == "descending/ticketid")
                {
                    subquery = "order by #e.Id desc";
                    subquery1 = "order by Id desc";
                }
                else if (order == "ascending/totalcommission")
                {
                    subquery = "order by #e.Commission asc";
                    subquery1 = "order by Commission asc";
                }
                else if (order == "descending/totalcommission")
                {
                    subquery = "order by #e.Commission desc";
                    subquery1 = "order by Commission desc";
                }
                else if (order == "ascending/totalcollected")
                {
                    subquery = "order by #e.BalanceDue asc";
                    subquery1 = "order by BalanceDue asc";
                }
                else if (order == "descending/totalcollected")
                {
                    subquery = "order by #e.BalanceDue desc";
                    subquery1 = "order by BalanceDue desc";
                }
                else if (order == "ascending/batch")
                {
                    subquery = "order by #e.Batch asc";
                    subquery1 = "order by Batch asc";
                }
                else if (order == "descending/batch")
                {
                    subquery = "order by #e.Batch desc";
                    subquery1 = "order by Batch desc";
                }
            }
            else
            {
                subquery = "order by #e.Id desc";
                subquery1 = "order by Id desc";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, DateFilter2, order, subquery, subquery1, DateFilter1, IsPaidQuery, techPayrollFilter, NameSql, SearchQuery, FilterQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", SearchText)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }

            }
            catch (Exception ee)
            {
                return null;
            }

        }

        public DataSet GetDownLoadAllMemberCommReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, bool IsPaid, Guid UserId, string SearchText, string MemberList)
        {
            string DateFilter2 = "";
            string DateFilter1 = "";
            string IsPaidQuery = "";
            string techPayrollFilter = "";
            string SearchQuery = "";
            string FilterQuery = "";
            string Batch = "";
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
                //NameSql = NameSql.Replace("cus.", "cu.");
            }

            if (!string.IsNullOrEmpty(SearchText))
            {
                SearchQuery = string.Format(" ([Ticket Id] like @SearchText or  [Customer Name] like @SearchText or [Technician] like @SearchText or [Unpaid Balance] like @SearchText) and ");
            }

            if (!string.IsNullOrEmpty(MemberList) && MemberList != "'null'")
            {

                FilterQuery = string.Format(" UserId in ({0}) and ", MemberList);
            }


            if (UserId != new Guid())
            {
                techPayrollFilter = string.Format(" and mc.UserId = '{0}'", UserId);
            }
            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime())
            {

                FilterStartDate = FilterStartDate.SetZeroHour();
                FilterEndDate = FilterEndDate.SetMaxHour();
                DateFilter1 = string.Format("and mc.CreatedDate between '{0}' and '{1}'"
                    , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (IsPaid)
            {
                IsPaidQuery = "where mc.IsPaid = 1";
                Batch = ", [Batch]";
            }
            else
            {
                IsPaidQuery = "where (mc.IsPaid = 0 or mc.IsPaid is null)  ";
            }
            string sqlQuery = @"
                                declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                 select * into #MemberCom  from (select
                                  tk.Id as [Ticket Id],
								 {7} as [Customer Name],
                                 cus.CustomerNo as [CS ID],
                                 emp.FirstName +' '+emp.LastName as [Technician],
                                 format(mc.CompletionDate,'M/d/yy') as  [Completion Date],
                                 mc.Adjustment as [Adjustment],
                                 mc.Commission as [Commission], 
								 
								(select cast(ISNULL(SUM(BalanceDue),0)  as decimal(12,2)) [Unpaid Balance] 
                                From Invoice where CustomerId=cus.CustomerId
                                and (Status='Open' or Status='Partial')) as [Unpaid Balance],
                                mc.Id,mc.UserId,mc.Batch as [Batch]
 
                                from AddMemberCommission mc
                                left join Ticket tk on tk.TicketId = mc.TicketId
                                left join customer cus on cus.CustomerId = tk.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = mc.UserId
                                {5}{4}{6}   and tk.IsClosed = 1 and ce.IsTestAccount != 1             
                                ) d	

                                 select * into #MemberComFilter
								from #MemberCom
                               

								select 
								[Ticket Id],
								[Customer Name],
                                [CS ID],
                                [Technician],
                                [Completion Date],
                                [Adjustment],
                                [Commission],
                                [Unpaid Balance] {10}
                                from #MemberComFilter
								where {8} {9} Id not in(select top(@pagestart) Id from #MemberComFilter #e {2})
                                {3}
								select count(*) CountTotal
                                from #MemberComFilter
                                where {8} {9} Id>0
								drop table #MemberCom
								drop table #MemberComFilter";

            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/customername")
                {
                    subquery = "order by #e.[Customer Name] asc";
                    subquery1 = "order by [Customer Name] asc";
                }
                else if (order == "descending/customername")
                {
                    subquery = "order by #e.[Customer Name] desc";
                    subquery1 = "order by [Customer Name] desc";
                }
                else if (order == "ascending/ticketid")
                {
                    subquery = "order by #e.Id asc";
                    subquery1 = "order by Id asc";
                }
                else if (order == "descending/ticketid")
                {
                    subquery = "order by #e.Id desc";
                    subquery1 = "order by Id desc";
                }
                else if (order == "ascending/userassigned")
                {
                    subquery = "order by #e.UserAssigned asc";
                    subquery1 = "order by UserAssigned asc";
                }
                else if (order == "descending/userassigned")
                {
                    subquery = "order by #e.UserAssigned desc";
                    subquery1 = "order by UserAssigned desc";
                }
                else if (order == "ascending/completiondate")
                {
                    subquery = "order by #e.[Completion Date] asc";
                    subquery1 = "order by [Completion Date] asc";
                }
                else if (order == "descending/completiondate")
                {
                    subquery = "order by #e.[Completion Date] desc";
                    subquery1 = "order by [Completion Date] desc";
                }
            }
            else
            {
                subquery = "order by #e.[Ticket Id] desc";
                subquery1 = "order by Id desc";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, DateFilter2, order, subquery, subquery1, DateFilter1, IsPaidQuery, techPayrollFilter, NameSql, SearchQuery, FilterQuery, Batch);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", SearchText)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }

            }
            catch (Exception ee)
            {
                return null;
            }

        }

        public DataSet GetDownLoadServiceCallCommReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, bool IsPaid, Guid UserId, string SearchText, string MemberList)
        {
            string DateFilter2 = "";
            string DateFilter1 = "";
            string IsPaidQuery = "";
            string techPayrollFilter = "";
            string SearchQuery = "";
            string FilterQuery = "";
            string Batch = "";
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
                //NameSql = NameSql.Replace("cus.", "cu.");
            }

            if (!string.IsNullOrEmpty(SearchText))
            {
                SearchQuery = string.Format(" ([Ticket Id] like @SearchText or  [Customer Name] like @SearchText or [Technician] like @SearchText or [Unpaid Balance] like @SearchText) and ");
            }

            if (!string.IsNullOrEmpty(MemberList) && MemberList != "'null'")
            {

                FilterQuery = string.Format(" UserId in ({0}) and ", MemberList);
            }


            if (UserId != new Guid())
            {
                techPayrollFilter = string.Format(" and mc.UserId = '{0}'", UserId);
            }
            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime())
            {

                FilterStartDate = FilterStartDate.SetClientZeroHourToUTC(); //SetZeroHour();
                FilterEndDate = FilterEndDate.SetClientMaxHourToUTC(); //SetMaxHour();
                DateFilter1 = string.Format("and mc.CreatedDate between '{0}' and '{1}'"
                    , FilterStartDate
                    , FilterEndDate); //.ToString("yyyy-MM-dd HH:mm:ss")
            }
            if (IsPaid)
            {
                IsPaidQuery = "where mc.IsPaid = 1";
                Batch = ", [Batch]";
            }
            else
            {
                IsPaidQuery = "where (mc.IsPaid = 0 or mc.IsPaid is null)  ";
            }
            string sqlQuery = @"
                                 declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                 select * into #ServiceeCom  from (select
                                  tk.Id as [Ticket Id],
								 {7} as [Customer Name],
                                 cus.CustomerNo as [CS ID],
                                 emp.FirstName +' '+emp.LastName as [Technician],
                                 TRY_CONVERT(date,mc.CompletionDate) as  [Completion Date],
                                 mc.Adjustment as [Adjustment],
                                 mc.Commission as [Commission], 
								 
								(select cast(ISNULL(SUM(BalanceDue),0)  as decimal(12,2)) [Unpaid Balance] 
                                From Invoice where CustomerId=cus.CustomerId
                                and (Status='Open' or Status='Partial')) as [Unpaid Balance],
                                mc.Id,mc.UserId,mc.Batch as [Batch]
 
                                from ServiceCallCommission mc
                                left join Ticket tk on tk.TicketId = mc.TicketId
                                left join customer cus on cus.CustomerId = tk.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = mc.UserId
                                {5}{4}{6}  and tk.IsClosed = 1 and ce.IsTestAccount != 1    
                                ) d	

                                 select * into #ServiceCallComFilter
								from #ServiceeCom


								select 
								[Ticket Id],
								[Customer Name],
                                [CS ID],
                                [Technician],
                                [Completion Date],
                                [Adjustment],
                                [Commission],
                                [Unpaid Balance] {10}
                                from #ServiceCallComFilter
								where {8} {9} Id not in(select top(@pagestart) Id from #ServiceCallComFilter #e {2})
                                {3}
								select count(*) CountTotal
                                from #ServiceCallComFilter
                                where {8} {9} Id>0
								drop table #ServiceeCom
								drop table #ServiceCallComFilter";

            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/customername")
                {
                    subquery = "order by #e.[Customer Name] asc";
                    subquery1 = "order by [Customer Name] asc";
                }
                else if (order == "descending/customername")
                {
                    subquery = "order by #e.[Customer Name] desc";
                    subquery1 = "order by [Customer Name] desc";
                }
                else if (order == "ascending/ticketid")
                {
                    subquery = "order by #e.Id asc";
                    subquery1 = "order by Id asc";
                }
                else if (order == "descending/ticketid")
                {
                    subquery = "order by #e.Id desc";
                    subquery1 = "order by Id desc";
                }
                else if (order == "ascending/userassigned")
                {
                    subquery = "order by #e.UserAssigned asc";
                    subquery1 = "order by UserAssigned asc";
                }
                else if (order == "descending/userassigned")
                {
                    subquery = "order by #e.UserAssigned desc";
                    subquery1 = "order by UserAssigned desc";
                }
                else if (order == "ascending/completiondate")
                {
                    subquery = "order by #e.[Completion Date] asc";
                    subquery1 = "order by [Completion Date] asc";
                }
                else if (order == "descending/completiondate")
                {
                    subquery = "order by #e.[Completion Date] desc";
                    subquery1 = "order by [Completion Date] desc";
                }
            }
            else
            {
                subquery = "order by #e.[Ticket Id] desc";
                subquery1 = "order by [Ticket Id] desc";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, DateFilter2, order, subquery, subquery1, DateFilter1, IsPaidQuery, techPayrollFilter, NameSql, SearchQuery, FilterQuery, Batch);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", SearchText)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }

            }
            catch (Exception ee)
            {
                return null;
            }

        }

        public DataSet GetDownLoadFollowUpCommReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, bool IsPaid, Guid UserId, string SearchText, string MemberList)
        {
            string DateFilter2 = "";
            string DateFilter1 = "";
            string IsPaidQuery = "";
            string techPayrollFilter = "";
            string SearchQuery = "";
            string FilterQuery = "";
            string Batch = "";
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
                //NameSql = NameSql.Replace("cus.", "cu.");
            }

            if (!string.IsNullOrEmpty(SearchText))
            {
                SearchQuery = string.Format(" ([Ticket Id] like @SearchText or  [Customer Name] like @SearchText or [Technician] like @SearchText or [Unpaid Balance] like @SearchText) and ");
            }

            if (!string.IsNullOrEmpty(MemberList) && MemberList != "'null'")
            {

                FilterQuery = string.Format(" UserId in ({0}) and ", MemberList);
            }


            if (UserId != new Guid())
            {
                techPayrollFilter = string.Format(" and mc.UserId = '{0}'", UserId);
            }
            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime())
            {

                FilterStartDate = FilterStartDate.SetClientZeroHourToUTC(); //SetZeroHour();
                FilterEndDate = FilterEndDate.SetClientMaxHourToUTC(); //SetMaxHour();
                DateFilter1 = string.Format("and mc.CreatedDate between '{0}' and '{1}'"
                    , FilterStartDate
                    , FilterEndDate); //.ToString("yyyy-MM-dd HH:mm:ss")
            }
            if (IsPaid)
            {
                IsPaidQuery = "where mc.IsPaid = 1";
                Batch = ", [Batch]";
            }
            else
            {
                IsPaidQuery = "where (mc.IsPaid = 0 or mc.IsPaid is null)  ";
            }
            string sqlQuery = @"
                                 declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                 select * into #FollowUpCom  from (select
                                  tk.Id as [Ticket Id],
								 {7} as [Customer Name],
                                 cus.CustomerNo as [CS ID],
                                 emp.FirstName +' '+emp.LastName as [Technician],
                                 TRY_CONVERT(date,mc.CompletionDate) as  [Completion Date],
                                 mc.Adjustment as [Adjustment],
                                 mc.Commission as [Commission], 
								 
								(select cast(ISNULL(SUM(BalanceDue),0)  as decimal(12,2)) [Unpaid Balance] 
                                From Invoice where CustomerId=cus.CustomerId
                                and (Status='Open' or Status='Partial')) as [Unpaid Balance],
                                mc.Id,mc.UserId,mc.Batch as [Batch]
 
                                
                                from FollowUpCommission mc
                                left join Ticket tk on tk.TicketId = mc.TicketId
                                left join customer cus on cus.CustomerId = tk.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = mc.UserId
                                {5}{4}{6}  and tk.IsClosed = 1 and ce.IsTestAccount != 1     
                                ) d	

                                 select * into #FollowUpComFilter
								from #FollowUpCom


								select 
								[Ticket Id],
								[Customer Name],
                                [CS ID],
                                [Technician],
                                [Completion Date],
                                [Adjustment],
                                [Commission],
                                [Unpaid Balance] {10}
                                from #FollowUpComFilter
								where {8} {9} Id not in(select top(@pagestart) Id from #FollowUpComFilter #e {2})
                                {3}
								select count(*) CountTotal
                                from #FollowUpComFilter
                                where {8} {9} Id>0
								drop table #FollowUpCom
								drop table #FollowUpComFilter";

            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/customername")
                {
                    subquery = "order by #e.[Customer Name] asc";
                    subquery1 = "order by [Customer Name] asc";
                }
                else if (order == "descending/customername")
                {
                    subquery = "order by #e.[Customer Name] desc";
                    subquery1 = "order by [Customer Name] desc";
                }
                else if (order == "ascending/ticketid")
                {
                    subquery = "order by #e.Id asc";
                    subquery1 = "order by Id asc";
                }
                else if (order == "descending/ticketid")
                {
                    subquery = "order by #e.Id desc";
                    subquery1 = "order by Id desc";
                }
                else if (order == "ascending/userassigned")
                {
                    subquery = "order by #e.UserAssigned asc";
                    subquery1 = "order by UserAssigned asc";
                }
                else if (order == "descending/userassigned")
                {
                    subquery = "order by #e.UserAssigned desc";
                    subquery1 = "order by UserAssigned desc";
                }
                else if (order == "ascending/completiondate")
                {
                    subquery = "order by #e.[Completion Date] asc";
                    subquery1 = "order by [Completion Date] asc";
                }
                else if (order == "descending/completiondate")
                {
                    subquery = "order by #e.[Completion Date] desc";
                    subquery1 = "order by [Completion Date] desc";
                }
            }
            else
            {
                subquery = "order by #e.[Ticket Id] desc";
                subquery1 = "order by [Ticket Id] desc";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, DateFilter2, order, subquery, subquery1, DateFilter1, IsPaidQuery, techPayrollFilter, NameSql, SearchQuery, FilterQuery, Batch);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", SearchText)));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }

            }
            catch (Exception ee)
            {
                return null;
            }

        }
        public DataSet GetDownLoadRescheduleCommReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, bool IsPaid, Guid UserId, string SearchText, string MemberList)
        {
            string DateFilter2 = "";
            string DateFilter1 = "";
            string IsPaidQuery = "";
            string techPayrollFilter = "";
            string SearchQuery = "";
            string FilterQuery = "";
            string Batch = "";
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
                //NameSql = NameSql.Replace("cus.", "cu.");
            }

            if (!string.IsNullOrEmpty(SearchText))
            {
                SearchQuery = string.Format(" ([Ticket Id] like @SearchText or  [Customer Name] like @SearchText or [Technician] like @SearchText or [Unpaid Balance] like @SearchText) and ");
            }

            if (!string.IsNullOrEmpty(MemberList) && MemberList != "'null'")
            {

                FilterQuery = string.Format(" UserId in ({0}) and ", MemberList);
            }


            if (UserId != new Guid())
            {
                techPayrollFilter = string.Format(" and mc.UserId = '{0}'", UserId);
            }
            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime())
            {

                FilterStartDate = FilterStartDate.SetClientZeroHourToUTC(); //SetZeroHour();
                FilterEndDate = FilterEndDate.SetClientMaxHourToUTC();  //SetMaxHour();
                DateFilter1 = string.Format("and mc.CreatedDate between '{0}' and '{1}'"
                    , FilterStartDate
                    , FilterEndDate); //.ToString("yyyy-MM-dd HH:mm:ss")
            }
            if (IsPaid)
            {
                IsPaidQuery = "where mc.IsPaid = 1";
                Batch = ", [Batch]";
            }
            else
            {
                IsPaidQuery = "where (mc.IsPaid = 0 or mc.IsPaid is null)  ";
            }
            string sqlQuery = @"
                                 declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                 select * into #RescheduleComm  from (select
                                  tk.Id as [Ticket Id],
								 {7} as [Customer Name],
                                  cus.CustomerNo as [CS ID],
                                 emp.FirstName +' '+emp.LastName as [Technician],
                                 TRY_CONVERT(date,mc.CompletionDate) as  [Completion Date],
                                 mc.Adjustment as [Adjustment],
                                 mc.Commission as [Commission], 
								 
								(select cast(ISNULL(SUM(BalanceDue),0)  as decimal(12,2)) [Unpaid Balance] 
                                From Invoice where CustomerId=cus.CustomerId
                                and (Status='Open' or Status='Partial')) as [Unpaid Balance],
                                mc.Id,mc.UserId,mc.Batch as [Batch]
 
                                
                                from RescheduleCommission mc
                                left join Ticket tk on tk.TicketId = mc.TicketId
                                left join customer cus on cus.CustomerId = tk.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = mc.UserId
                                {5}{4}{6}  and tk.IsClosed = 1  and ce.IsTestAccount != 1    
                                ) d	

                                 select * into #RescheduleCommFilter
								from #RescheduleComm


								select 
								[Ticket Id],
								[Customer Name],
                                [CS ID],
                                [Technician],
                                [Completion Date],
                                [Adjustment],
                                [Commission],
                                [Unpaid Balance] {10}
                                from #RescheduleCommFilter
								where {8} {9} Id not in(select top(@pagestart) Id from #RescheduleCommFilter #e {2})
                                {3}
								select count(*) CountTotal
                                from #RescheduleCommFilter
                                where {8} {9} Id>0
								drop table #RescheduleComm
								drop table #RescheduleCommFilter";

            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/customername")
                {
                    subquery = "order by #e.[Customer Name] asc";
                    subquery1 = "order by [Customer Name] asc";
                }
                else if (order == "descending/customername")
                {
                    subquery = "order by #e.[Customer Name] desc";
                    subquery1 = "order by [Customer Name] desc";
                }
                else if (order == "ascending/ticketid")
                {
                    subquery = "order by #e.Id asc";
                    subquery1 = "order by Id asc";
                }
                else if (order == "descending/ticketid")
                {
                    subquery = "order by #e.Id desc";
                    subquery1 = "order by Id desc";
                }
                else if (order == "ascending/userassigned")
                {
                    subquery = "order by #e.UserAssigned asc";
                    subquery1 = "order by UserAssigned asc";
                }
                else if (order == "descending/userassigned")
                {
                    subquery = "order by #e.UserAssigned desc";
                    subquery1 = "order by UserAssigned desc";
                }
                else if (order == "ascending/completiondate")
                {
                    subquery = "order by #e.[Completion Date] asc";
                    subquery1 = "order by [Completion Date] asc";
                }
                else if (order == "descending/completiondate")
                {
                    subquery = "order by #e.[Completion Date] desc";
                    subquery1 = "order by [Completion Date] desc";
                }
            }
            else
            {
                subquery = "order by #e.[Ticket Id] desc";
                subquery1 = "order by [Ticket Id] desc";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, DateFilter2, order, subquery, subquery1, DateFilter1, IsPaidQuery, techPayrollFilter, NameSql, SearchQuery, FilterQuery, Batch);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", SearchText)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }

            }
            catch (Exception ee)
            {
                return null;
            }

        }

        public DataSet GetAllServiceCallReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, bool IsPaid, Guid UserId, string SearchText, string ServicePersonList)
        {
            string DateFilter2 = "";
            string DateFilter1 = "";
            string IsPaidQuery = "";
            string techPayrollFilter = "";
            string SearchQuery = "";
            string FilterQuery = "";
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
                //NameSql = NameSql.Replace("cus.", "cu.");
            }
            if (!string.IsNullOrEmpty(SearchText))
            {
                SearchQuery = string.Format(" (TicketIdValue like @SearchText or  CustomerName like @SearchText or Technician like @SearchText or BalanceDue like @SearchText) and ");
            }

            if (!string.IsNullOrEmpty(ServicePersonList) && ServicePersonList != "'null'")
            {

                FilterQuery = string.Format(" UserId in ({0}) and ", ServicePersonList);
            }


            if (UserId != new Guid())
            {
                techPayrollFilter = string.Format(" and mc.UserId = '{0}'", UserId);
            }
            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime())
            {

                FilterStartDate = FilterStartDate.SetClientZeroHourToUTC(); //SetZeroHour();
                FilterEndDate = FilterEndDate.SetClientMaxHourToUTC();  //SetMaxHour();
                DateFilter1 = string.Format("and mc.CreatedDate between '{0}' and '{1}'"
                    , FilterStartDate
                    , FilterEndDate); //ToString("yyyy-MM-dd HH:mm:ss")
            }
            if (IsPaid)
            {
                IsPaidQuery = "where mc.IsPaid = 1";
            }
            else
            {
                IsPaidQuery = "where (mc.IsPaid = 0 or mc.IsPaid is null) ";
            }
            string sqlQuery = @"
                                declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                 select * into #ServiceeCom  from (select
                                  mc.*, 
                                  tk.Id as TicketIdValue,
								 {7} as CustomerName,
							     cus.Id as CustomerIdValue,
								 emp.FirstName +' '+emp.LastName as Technician,
								(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                From Invoice where CustomerId=cus.CustomerId
                                and (Status='Open' or Status='Partial')) as BalanceDue
 
                                from ServiceCallCommission mc
                                left join Ticket tk on tk.TicketId = mc.TicketId
                                left join customer cus on cus.CustomerId = tk.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = mc.UserId
                                {5}{4}{6}  and tk.IsClosed = 1  and ce.IsTestAccount != 1
                                ) d	

                                 select * into #ServiceCallComFilter
								from #ServiceeCom


								select top(@pagesize)
								* into #TotalCount from #ServiceCallComFilter
								where {8} {9} Id not in(select top(@pagestart) Id from #ServiceCallComFilter #e {2})
                                {3}
								select count(*) CountTotal
                                from #ServiceCallComFilter
                                where  {8} {9} Id>0

								select * from #TotalCount
								select sum(Adjustment) as TotalAdjustment
								,sum(Commission) as SumCommission
								,sum(BalanceDue) as SumUnpaid
								from #TotalCount

								drop table #ServiceeCom
								drop table #ServiceCallComFilter
								drop table #TotalCount";

            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/adjustment")
                {
                    subquery = "order by #e.Adjustment asc";
                    subquery1 = "order by Adjustment asc";
                }
                else if (order == "descending/adjustment")
                {
                    subquery = "order by #e.Adjustment desc";
                    subquery1 = "order by Adjustment desc";
                }
                else if (order == "ascending/ticketid")
                {
                    subquery = "order by #e.Id asc";
                    subquery1 = "order by Id asc";
                }
                else if (order == "descending/ticketid")
                {
                    subquery = "order by #e.Id desc";
                    subquery1 = "order by Id desc";
                }
                else if (order == "ascending/totalcommission")
                {
                    subquery = "order by #e.Commission asc";
                    subquery1 = "order by Commission asc";
                }
                else if (order == "descending/totalcommission")
                {
                    subquery = "order by #e.Commission desc";
                    subquery1 = "order by Commission desc";
                }
                else if (order == "ascending/totalcollected")
                {
                    subquery = "order by #e.BalanceDue asc";
                    subquery1 = "order by BalanceDue asc";
                }
                else if (order == "descending/totalcollected")
                {
                    subquery = "order by #e.BalanceDue desc";
                    subquery1 = "order by BalanceDue desc";
                }
                else if (order == "ascending/batch")
                {
                    subquery = "order by #e.Batch asc";
                    subquery1 = "order by Batch asc";
                }
                else if (order == "descending/batch")
                {
                    subquery = "order by #e.Batch desc";
                    subquery1 = "order by Batch desc";
                }
            }
            else
            {
                subquery = "order by #e.Id desc";
                subquery1 = "order by Id desc";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, DateFilter2, order, subquery, subquery1, DateFilter1, IsPaidQuery, techPayrollFilter, NameSql, SearchQuery, FilterQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", SearchText)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }

            }
            catch (Exception ee)
            {
                return null;
            }

        }

        public DataSet GetAllFollowUpReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, bool IsPaid, Guid UserId, string SearchText, string FollowUpPersonList)
        {
            string DateFilter2 = "";
            string DateFilter1 = "";
            string IsPaidQuery = "";
            string techPayrollFilter = "";
            string FilterQuery = "";
            string SearchQuery = "";
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
                //NameSql = NameSql.Replace("cus.", "cu.");
            }
            if (!string.IsNullOrEmpty(SearchText))
            {
                SearchQuery = string.Format(" (TicketIdValue like @SearchText or  CustomerName like @SearchText or Technician like @SearchText or BalanceDue like @SearchText) and ");
            }


            if (UserId != new Guid())
            {
                techPayrollFilter = string.Format(" and mc.UserId = '{0}'", UserId);
            }

            if (!string.IsNullOrEmpty(FollowUpPersonList) && FollowUpPersonList != "'null'")
            {

                FilterQuery = string.Format(" UserId in ({0}) and ", FollowUpPersonList);
            }


            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime())
            {

                FilterStartDate = FilterStartDate.SetClientZeroHourToUTC(); //SetZeroHour();
                FilterEndDate = FilterEndDate.SetClientMaxHourToUTC(); //SetMaxHour();
                DateFilter1 = string.Format("and mc.CreatedDate between '{0}' and '{1}'"
                    , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (IsPaid)
            {
                IsPaidQuery = "where mc.IsPaid = 1";
            }
            else
            {
                IsPaidQuery = "where (mc.IsPaid = 0 or mc.IsPaid is null) ";
            }
            string sqlQuery = @"
                                declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                 select * into #FollowUPCom  from (select
                                  mc.*, 
                                  tk.Id as TicketIdValue,
								 {7} as CustomerName,
							     cus.Id as CustomerIdValue,
								 emp.FirstName +' '+emp.LastName as Technician,
								(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                From Invoice where CustomerId=cus.CustomerId
                                and (Status='Open' or Status='Partial')) as BalanceDue
 
                                from FollowUpCommission mc
                                left join Ticket tk on tk.TicketId = mc.TicketId
                                left join customer cus on cus.CustomerId = tk.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = mc.UserId
                                {5}{4}{6}   and tk.IsClosed = 1 and ce.IsTestAccount != 1               
                                ) d	

                                 select * into #FollowUpComFilter
								from #FollowUpCom


								select top(@pagesize)
								* into #TotalCount from #FollowUpComFilter
								where {8} {9}  Id not in(select top(@pagestart) Id from #FollowUpComFilter #e {2})
                                {3}
								select count(*) CountTotal
                                from #FollowUpComFilter
                                where {8} {9}  Id>0

								select * from #TotalCount
								select sum(Adjustment) as TotalAdjustment
								,sum(Commission) as SumCommission
								,sum(BalanceDue) as SumUnpaid
								from #TotalCount

								drop table #FollowUpCom
								drop table #FollowUpComFilter
								drop table #TotalCount";

            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/customername")
                {
                    subquery = "order by #e.CustomerName asc";
                    subquery1 = "order by CustomerName asc";
                }
                else if (order == "descending/customername")
                {
                    subquery = "order by #e.CustomerName desc";
                    subquery1 = "order by CustomerName desc";
                }
                else if (order == "ascending/ticketid")
                {
                    subquery = "order by #e.Id asc";
                    subquery1 = "order by Id asc";
                }
                else if (order == "descending/ticketid")
                {
                    subquery = "order by #e.Id desc";
                    subquery1 = "order by Id desc";
                }
                else if (order == "ascending/userassigned")
                {
                    subquery = "order by #e.UserAssigned asc";
                    subquery1 = "order by UserAssigned asc";
                }
                else if (order == "descending/userassigned")
                {
                    subquery = "order by #e.UserAssigned desc";
                    subquery1 = "order by UserAssigned desc";
                }
                else if (order == "ascending/completiondate")
                {
                    subquery = "order by #e.CompletionDate asc";
                    subquery1 = "order by CompletionDate asc";
                }
                else if (order == "descending/completiondate")
                {
                    subquery = "order by #e.CompletionDate desc";
                    subquery1 = "order by CompletionDate desc";
                }
            }
            else
            {
                subquery = "order by #e.TicketIdValue desc";
                subquery1 = "order by TicketIdValue desc";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, DateFilter2, order, subquery, subquery1, DateFilter1, IsPaidQuery, techPayrollFilter, NameSql, SearchQuery, FilterQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", SearchText)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }

            }
            catch (Exception ee)
            {
                return null;
            }

        }

        public DataSet GetAllRescheduleReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, bool IsPaid, Guid UserId, string SearchText, string RescheduleTech)
        {
            string DateFilter2 = "";
            string DateFilter1 = "";
            string IsPaidQuery = "";
            string techPayrollFilter = "";
            string SearchQuery = "";
            string FilterQuery = "";
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
                //NameSql = NameSql.Replace("cus.", "cu.");
            }
            if (!string.IsNullOrEmpty(SearchText))
            {
                SearchQuery = string.Format(" (TicketIdValue like @SearchText or  CustomerName like @SearchText or Technician like @SearchText or BalanceDue like @SearchText) and ");
            }

            if (!string.IsNullOrEmpty(RescheduleTech) && RescheduleTech != "'null'")
            {

                FilterQuery = string.Format(" UserId in ({0}) and ", RescheduleTech);
            }
            if (UserId != new Guid())
            {
                techPayrollFilter = string.Format(" and mc.UserId= '{0}' ", UserId);
            }
            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime())
            {

                FilterStartDate = FilterStartDate.SetClientZeroHourToUTC(); //SetZeroHour();
                FilterEndDate = FilterEndDate.SetClientMaxHourToUTC(); //SetMaxHour();
                DateFilter1 = string.Format("and mc.CreatedDate between '{0}' and '{1}'"
                    , FilterStartDate
                    , FilterEndDate); //.ToString("yyyy-MM-dd HH:mm:ss")
            }
            if (IsPaid)
            {
                IsPaidQuery = "where mc.IsPaid = 1";
            }
            else
            {
                IsPaidQuery = "where (mc.IsPaid = 0 or mc.IsPaid is null)";
            }
            string sqlQuery = @"
                                declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                 select * into #RescheduleCom  from (select
                                  mc.*, 
                                  tk.Id as TicketIdValue,
								 {7} as CustomerName,
							     cus.Id as CustomerIdValue,
								 emp.FirstName +' '+emp.LastName as Technician,
								(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                From Invoice where CustomerId=cus.CustomerId
                                and (Status='Open' or Status='Partial')) as BalanceDue
 
                                from RescheduleCommission mc
                                left join Ticket tk on tk.TicketId = mc.TicketId
                                left join customer cus on cus.CustomerId = tk.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = mc.UserId
                                {5}{4}{6} and tk.IsClosed = 1 and ce.IsTestAccount != 1
                                ) d	

                                 select * into #RescheduleComFilter
								from #RescheduleCom
                         

								select top(@pagesize)
								* into #TotalCount from #RescheduleComFilter
								where {8} {9} Id not in(select top(@pagestart) Id from #RescheduleComFilter #e {2})
                                {3}
								select count(*) CountTotal
                                from #RescheduleComFilter
                                where {8} {9}  Id>0


								select * from #TotalCount
								select sum(Adjustment) as TotalAdjustment
								,sum(Commission) as SumCommission
								,sum(BalanceDue) as SumUnpaid
								from #TotalCount

								drop table #RescheduleCom
								drop table #RescheduleComFilter
								drop table #TotalCount";

            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/customername")
                {
                    subquery = "order by #e.CustomerName asc";
                    subquery1 = "order by CustomerName asc";
                }
                else if (order == "descending/customername")
                {
                    subquery = "order by #e.CustomerName desc";
                    subquery1 = "order by CustomerName desc";
                }
                else if (order == "ascending/ticketid")
                {
                    subquery = "order by #e.Id asc";
                    subquery1 = "order by Id asc";
                }
                else if (order == "descending/ticketid")
                {
                    subquery = "order by #e.Id desc";
                    subquery1 = "order by Id desc";
                }
                else if (order == "ascending/userassigned")
                {
                    subquery = "order by #e.UserAssigned asc";
                    subquery1 = "order by UserAssigned asc";
                }
                else if (order == "descending/userassigned")
                {
                    subquery = "order by #e.UserAssigned desc";
                    subquery1 = "order by UserAssigned desc";
                }
                else if (order == "ascending/completiondate")
                {
                    subquery = "order by #e.CompletionDate asc";
                    subquery1 = "order by CompletionDate asc";
                }
                else if (order == "descending/completiondate")
                {
                    subquery = "order by #e.CompletionDate desc";
                    subquery1 = "order by CompletionDate desc";
                }
            }
            else
            {
                subquery = "order by #e.TicketIdValue desc";
                subquery1 = "order by TicketIdValue desc";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, DateFilter2, order, subquery, subquery1, DateFilter1, IsPaidQuery, techPayrollFilter, NameSql, SearchQuery, FilterQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", SearchText)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }

            }
            catch (Exception ee)
            {
                return null;
            }

        }
        public DataSet GetAllFundedReportCluster(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, string SearchText, bool IsPaid, string FilterText, string UserGroup, string TicketType)
        {
            var IsPaidSalesQuery = "";
            var IsPaidTechQuery = "";
            var IsPaidMemberQuery = "";
            var IsPaidFinRepQuery = "";
            var IsPaidCallQuery = "";
            var IsPaidFollowUpQuery = "";
            var IsPaidRescheduleQuery = "";
            string DateFilterSale = "";
            string DateFilterTech = "";
            string DateFilterMember = "";
            string DateFilterFinRep = "";
            string DateFilterService = "";
            string DateFilterFollowUp = "";
            string DateFilterReschedule = "";
            string PaidDateFilter = "";
            string SearchQuerySales = "";
            string SearchQueryTech = "";
            string SearchQueryAddMember = "";
            string SearchQueryFinRep = "";
            string SearchQueryServiceCall = "";
            string SearchQueryFollowUp = "";
            string SearchQueryReschedule = "";
            string FilterTextQuerySales = "";
            string FilterTextQueryTech = "";
            string FilterTextQueryMember = "";
            string FilterTextQueryFinRep = "";
            string FilterTextQueryCall = "";

            string FilterTextQueryFollowUp = "";
            string FilterTextQueryReschedule = "";

            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }

            string UserGroupFilterQuery = "";
            string DeleteEmployeQuery = "";
            if (!string.IsNullOrWhiteSpace(UserGroup) && UserGroup != "undefined" && UserGroup != "null" && UserGroup != "-1")
            {

                if (UserGroup == "404")
                {
                    DeleteEmployeQuery = " and ul.IsDeleted = 1";
                }
                else
                {
                    UserGroupFilterQuery = string.Format("and pg.Id ='{0}'", UserGroup);
                    DeleteEmployeQuery = " and ul.IsDeleted = 0";
                }
            }
            string ticketType = "";
            if (!string.IsNullOrWhiteSpace(TicketType) && TicketType != "''")
            {
                ticketType = string.Format("and t.TicketType in ({0})", TicketType);
            }

            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime() && string.IsNullOrEmpty(FilterText))
            {
                FilterStartDate = FilterStartDate.SetZeroHour();
                FilterEndDate = FilterEndDate.SetMaxHour();
                DateFilterSale = string.Format(" and salecom.CreatedDate between '{0}' and '{1}'"
                    , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));

                DateFilterTech = string.Format(" and techcom.CreatedDate between '{0}' and '{1}'"
                  , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                  , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));

                DateFilterMember = string.Format(" and addcom.CreatedDate between '{0}' and '{1}'"
                  , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                  , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));

                DateFilterFinRep = string.Format(" and finrep.CreatedDate between '{0}' and '{1}'"
                  , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                  , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));

                DateFilterService = string.Format(" and callcom.CreatedDate between '{0}' and '{1}'"
                  , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                  , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));

                DateFilterFollowUp = string.Format(" and followcom.CreatedDate between '{0}' and '{1}'"
                  , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                  , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));

                DateFilterReschedule = string.Format(" and reschedulecom.CreatedDate between '{0}' and '{1}'"
                  , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                  , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime() && !string.IsNullOrEmpty(FilterText))
            {
                PaidDateFilter = string.Format(" and PaidDate between '{0}' and '{1}'"
                  , FilterStartDate.SetZeroHour().ToString("yyyy-MM-dd HH:mm:ss")
                  , FilterEndDate.SetMaxHour().ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (IsPaid)
            {
                IsPaidSalesQuery = "where salecom.IsPaid = 1 ";
                IsPaidTechQuery = "where techcom.IsPaid = 1 ";
                IsPaidMemberQuery = "where addcom.IsPaid = 1 ";
                IsPaidFinRepQuery = "where finrep.IsPaid = 1 ";
                IsPaidCallQuery = "where callcom.IsPaid = 1 ";
                IsPaidFollowUpQuery = "where followcom.IsPaid = 1 ";
                IsPaidRescheduleQuery = "where reschedulecom.IsPaid =1 ";
            }
            else
            {
                IsPaidSalesQuery = " where (salecom.IsPaid = 0 or salecom.IsPaid is null)";
                IsPaidTechQuery = " where (techcom.IsPaid = 0 or techcom.IsPaid is  null)";
                IsPaidMemberQuery = " where (addcom.IsPaid = 0 or addcom.IsPaid is  null)";
                IsPaidFinRepQuery = " where (finrep.IsPaid = 0 or finrep.IsPaid is  null)";
                IsPaidCallQuery = " where (callcom.IsPaid = 0 or callcom.Ispaid is  null)";
                IsPaidFollowUpQuery = " where (followcom.IsPaid = 0 or followcom.IsPaid is  null)";
                IsPaidRescheduleQuery = " where (reschedulecom.IsPaid = 0 or reschedulecom.Ispaid is null)";
            }

            if (!string.IsNullOrEmpty(SearchText))
            {
                int searchId = 0;
                int.TryParse(SearchText, out searchId);
                SearchQuerySales = string.Format(" and (t.Id = '{0}' OR salecom.Batch= '{0}') ", searchId);
                SearchQueryTech = string.Format(" and (t.Id = '{0}' OR techcom.Batch= '{0}') ", searchId);
                SearchQueryAddMember = string.Format(" and (t.Id = '{0}' OR addcom.Batch= '{0}') ", searchId);
                SearchQueryFinRep = string.Format(" and (t.Id = '{0}' OR finrep.Batch= '{0}') ", searchId);
                SearchQueryServiceCall = string.Format(" and (t.Id = '{0}' OR callcom.Batch= '{0}') ", searchId);
                SearchQueryFollowUp = string.Format(" and (t.Id = '{0}' OR followcom.Batch= '{0}') ", searchId);
                SearchQueryReschedule = string.Format(" and (t.Id = '{0}' OR reschedulecom.Batch= '{0}') ", searchId);

            }
            if (!string.IsNullOrEmpty(FilterText) && FilterText != "-1")
            {
                FilterTextQuerySales = string.Format(" and salecom.UserId = '{0}'", FilterText);
                FilterTextQueryTech = string.Format(" and techcom.UserId = '{0}'", FilterText);
                FilterTextQueryMember = string.Format(" and addcom.UserId = '{0}'", FilterText);
                FilterTextQueryFinRep = string.Format(" and finrep.UserId = '{0}'", FilterText);
                FilterTextQueryCall = string.Format(" and callcom.UserId = '{0}'", FilterText);
                FilterTextQueryFollowUp = string.Format(" and followcom.UserId = '{0}'", FilterText);
                FilterTextQueryReschedule = string.Format(" and reschedulecom.UserId = '{0}'", FilterText);

            }
            string sqlQuery = @"
                                declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                select * into #FundedCom  from ( select salecom.TicketId TicketId, cus.Id CustomerIdValue,CASE 
	                            WHEN (cus.BusinessName = '' or cus.BusinessName IS NULL) THEN cus.FirstName +' '+cus.LastName
	                            ELSE  cus.BusinessName
                                END as CustomerName,t.Id ticid,t.CompletedDate CompletedDate,lpttype.DisplayText as TicketType,salecom.TotalCommission SalesTotalCommission,0 TechTotalCommission,0 AddCommission,0 FinRepCommission,0 CallCommission,0 FollowUpCommission,0 RescheduleCommission,(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                From Invoice where CustomerId=cus.CustomerId
                                and (Status='Open' or Status='Partial')) as BalanceDue,salecom.PaidDate PaidDate from SalesCommission salecom
                                left join ticket t on t.ticketid = salecom.TicketId
                                LEFT JOIN Lookup lpttype on lpttype.DataValue=t.TicketType and lpttype.DataKey='TicketType'
                                left join customer cus on cus.CustomerId = salecom.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = salecom.UserId
                                
                                left join   UserLogin ul  on emp.UserId = ul.UserId
								left join UserPermission up on up.UserId = ul.UserId
								left join PermissionGroup pg on pg.Id = up.PermissionGroupId
                                
                                {3}{7}{11}{12}{16} and t.IsClosed = 1 and ce.IsTestAccount != 1 {24}{25}{26}
                                UNION ALL
                                select techcom.TicketId TicketId,cus.Id CustomerIdValue,CASE 
	                            WHEN (cus.BusinessName = '' or cus.BusinessName IS NULL) THEN cus.FirstName +' '+cus.LastName
	                            ELSE  cus.BusinessName
                                END as CustomerName,t.Id ticid,t.CompletedDate CompletedDate,lpttype.DisplayText as TicketType,0 SalesTotalCommission,techcom.TotalCommission TechTotalCommission,0 AddCommission,0 FinRepCommission,0 CallCommission,0 FollowUpCommission,0 RescheduleCommission,(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                From Invoice where CustomerId=cus.CustomerId
                                and (Status='Open' or Status='Partial')) as BalanceDue,techcom.PaidDate PaidDate from TechCommission techcom
                                left join ticket t on t.ticketid = techcom.TicketId
                                LEFT JOIN Lookup lpttype on lpttype.DataValue=t.TicketType and lpttype.DataKey='TicketType'
                                left join customer cus on cus.CustomerId = techcom.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = techcom.UserId

                                left join   UserLogin ul  on emp.UserId = ul.UserId
								left join UserPermission up on up.UserId = ul.UserId
								left join PermissionGroup pg on pg.Id = up.PermissionGroupId

                                {4}{8}{27}{13}{16} and t.IsClosed = 1 and ce.IsTestAccount != 1 {24}{25}{26}
                                
                                UNION ALL
                                select addcom.TicketId TicketId,cus.Id CustomerIdValue,CASE 
                                WHEN (cus.BusinessName = '' or cus.BusinessName IS NULL) THEN cus.FirstName +' '+cus.LastName
                                ELSE  cus.BusinessName
                                END as CustomerName,t.Id ticid,t.CompletedDate CompletedDate,lpttype.DisplayText as TicketType,0 SalesTotalCommission,0 TechTotalCommission,addcom.Commission AddCommission,0 FinRepCommission,0 CallCommission,0 FollowUpCommission,0 RescheduleCommission,(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                From Invoice where CustomerId=cus.CustomerId
                                and (Status='Open' or Status='Partial')) as BalanceDue,addcom.PaidDate from addmembercommission addcom
                                left join ticket t on t.ticketid = addcom.TicketId
                                LEFT JOIN Lookup lpttype on lpttype.DataValue=t.TicketType and lpttype.DataKey='TicketType'
                                left join customer cus on cus.CustomerId = addcom.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = addcom.UserId

                                left join   UserLogin ul  on emp.UserId = ul.UserId
								left join UserPermission up on up.UserId = ul.UserId
								left join PermissionGroup pg on pg.Id = up.PermissionGroupId

                                {5}{9}{28}{14}{16} and t.IsClosed = 1 and ce.IsTestAccount != 1 {24}{25}{26}

                                UNION ALL
                                select finrep.TicketId TicketId,cus.Id CustomerIdValue,CASE 
                                WHEN (cus.BusinessName = '' or cus.BusinessName IS NULL) THEN cus.FirstName +' '+cus.LastName
                                ELSE  cus.BusinessName
                                END as CustomerName,t.Id ticid,t.CompletedDate CompletedDate,lpttype.DisplayText as TicketType,0 SalesTotalCommission,0 TechTotalCommission,0 AddCommission,finrep.Commission FinRepCommission,0 CallCommission,0 FollowUpCommission,0 RescheduleCommission,(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                From Invoice where CustomerId=cus.CustomerId
                                and (Status='Open' or Status='Partial')) as BalanceDue,finrep.PaidDate from FinRepCommission finrep
                                left join ticket t on t.ticketid = finrep.TicketId
                                LEFT JOIN Lookup lpttype on lpttype.DataValue=t.TicketType and lpttype.DataKey='TicketType'
                                left join customer cus on cus.CustomerId = finrep.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = finrep.UserId

                                left join   UserLogin ul  on emp.UserId = ul.UserId
								left join UserPermission up on up.UserId = ul.UserId
								left join PermissionGroup pg on pg.Id = up.PermissionGroupId

                                {32}{33}{34}{35}{16} and t.IsClosed = 1 and ce.IsTestAccount != 1 {24}{25}{26}
                                
                                UNION ALL
                                select callcom.TicketId TicketId,cus.Id CustomerIdValue,CASE 
	                            WHEN (cus.BusinessName = '' or cus.BusinessName IS NULL) THEN cus.FirstName +' '+cus.LastName
	                            ELSE  cus.BusinessName
                                END as CustomerName,t.Id ticid,t.CompletedDate CompletedDate,lpttype.DisplayText as TicketType,0 SalesTotalCommission,0 TechTotalCommission,0 AddCommission,0 FinRepCommission,callcom.Commission CallCommission,0 FollowUpCommission,0 RescheduleCommission,(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                From Invoice where CustomerId=cus.CustomerId
                                and (Status='Open' or Status='Partial')) as BalanceDue,callcom.PaidDate from ServiceCallCommission callcom
                                left join ticket t on t.ticketid = callcom.TicketId
                                LEFT JOIN Lookup lpttype on lpttype.DataValue=t.TicketType and lpttype.DataKey='TicketType'
                                left join customer cus on cus.CustomerId = callcom.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = callcom.UserId
                        
                                left join   UserLogin ul  on emp.UserId = ul.UserId
								left join UserPermission up on up.UserId = ul.UserId
								left join PermissionGroup pg on pg.Id = up.PermissionGroupId

                                {6}{10}{29}{15}{16} and t.IsClosed = 1 and ce.IsTestAccount != 1 {24}{25}{26}

                                UNION ALL
                                select followcom.TicketId TicketId,cus.Id CustomerIdValue,CASE 
	                            WHEN (cus.BusinessName = '' or cus.BusinessName IS NULL) THEN cus.FirstName +' '+cus.LastName
	                            ELSE  cus.BusinessName
                                END as CustomerName,t.Id ticid,t.CompletedDate CompletedDate,lpttype.DisplayText as TicketType,0 SalesTotalCommission,0 TechTotalCommission,0 AddCommission,0 FinRepCommission,0 CallCommission,followcom.Commission FollowUpCommission,0 RescheduleCommission,(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                From Invoice where CustomerId=cus.CustomerId
                                and (Status='Open' or Status='Partial')) as BalanceDue,followcom.PaidDate from FollowUpCommission followcom
                                left join ticket t on t.ticketid = followcom.TicketId
                               LEFT JOIN Lookup lpttype on lpttype.DataValue=t.TicketType and lpttype.DataKey='TicketType'
                                left join customer cus on cus.CustomerId = followcom.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = followcom.UserId

                                left join   UserLogin ul  on emp.UserId = ul.UserId
								left join UserPermission up on up.UserId = ul.UserId
								left join PermissionGroup pg on pg.Id = up.PermissionGroupId

                                {17}{21}{30}{19}{16} and t.IsClosed = 1 and ce.IsTestAccount != 1 {24}{25}{26}

                                UNION ALL
                                select reschedulecom.TicketId TicketId,cus.Id CustomerIdValue,CASE 
                                WHEN (cus.BusinessName = '' or cus.BusinessName IS NULL) THEN cus.FirstName +' '+cus.LastName
                                ELSE  cus.BusinessName
                                END as CustomerName,t.Id ticid,t.CompletedDate CompletedDate,lpttype.DisplayText as TicketType,0 SalesTotalCommission,0 TechTotalCommission,0 AddCommission,0 FinRepCommission,0 CallCommission,0 FollowUpCommission,reschedulecom.Commission RescheduleCommission,(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                From Invoice where CustomerId=cus.CustomerId
                                and (Status='Open' or Status='Partial')) as BalanceDue,reschedulecom.PaidDate  from RescheduleCommission reschedulecom
                                left join ticket t on t.ticketid = reschedulecom.TicketId
                               LEFT JOIN Lookup lpttype on lpttype.DataValue=t.TicketType and lpttype.DataKey='TicketType'
                                left join customer cus on cus.CustomerId = reschedulecom.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = reschedulecom.UserId
        
                                left join   UserLogin ul  on emp.UserId = ul.UserId
								left join UserPermission up on up.UserId = ul.UserId
								left join PermissionGroup pg on pg.Id = up.PermissionGroupId

                                 {18}{22}{31}{20}{16} and t.IsClosed = 1 and ce.IsTestAccount != 1 {24}{25}{26}
                         
     
                                ) d	

                                 select 
								TicketId,
								CustomerIdValue,
								CustomerName,
								ticid,
                                CompletedDate,
                                DATEADD(dd, 0, DATEDIFF(dd, 0, PaidDate)) PaidDate,
								TicketType,
								SUM(SalesTotalCommission) as SalesTotalCommission,
								SUM(TechTotalCommission) as TechTotalCommission,
								SUM(AddCommission) as AddCommission,
                                SUM(FinRepCommission) as FinRepCommission,
								SUM(CallCommission) as CallCommission,
								SUM(FollowUpCommission) as FollowUpCommission,
								SUM(RescheduleCommission) as RescheduleCommission,
								BalanceDue
								 into #FundedComFilter
								 from #FundedCom
								group by TicketId,CustomerIdValue,CustomerName,ticid,TicketType,BalanceDue,CompletedDate,DATEADD(dd, 0, DATEDIFF(dd, 0, PaidDate))

                                select * Into #FundedComFilterCount
                                from #FundedComFilter

								select top(@pagesize)
								* from #FundedComFilter
								where TicketId not in(select top(@pagestart) TicketId from #FundedComFilter #e {1})
                                {2}
								select count(*) CountTotal
                                from #FundedComFilterCount
                                
								drop table #FundedCom
								drop table #FundedComFilter
								drop table #FundedComFilterCount";

            string subquery = "";
            string subquery1 = "";
            if (IsPaid)
            {

                subquery = " order by PaidDate desc";
                subquery1 = " order by PaidDate desc";
            }
            else
            {
                subquery = " order by CompletedDate desc";
                subquery1 = " order by CompletedDate desc";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                    order,//0
                    subquery,//1 
                    subquery1,//2
                    IsPaidSalesQuery,//3
                    IsPaidTechQuery,//4
                    IsPaidMemberQuery,//5
                    IsPaidCallQuery,//6
                    DateFilterSale,//7
                    DateFilterTech,//8
                    DateFilterMember,//9
                    DateFilterService,//10 
                    SearchQuerySales,//11 
                    FilterTextQuerySales,//12
                    FilterTextQueryTech,//13
                    FilterTextQueryMember,//14
                    FilterTextQueryCall,//15
                    PaidDateFilter,//16
                    IsPaidFollowUpQuery,//17
                    IsPaidRescheduleQuery,//18
                    FilterTextQueryFollowUp,//19
                    FilterTextQueryReschedule,//20
                    DateFilterFollowUp,//21
                    DateFilterReschedule,//22
                    NameSql,//23
                    UserGroupFilterQuery,//24
                    DeleteEmployeQuery,//25
                    ticketType,//26
                    SearchQueryTech,//27
                    SearchQueryAddMember,//28
                    SearchQueryServiceCall,//29
                    SearchQueryFollowUp,//30
                    SearchQueryReschedule,//31
                    IsPaidFinRepQuery,//32
                    DateFilterFinRep,//33
                    FilterTextQueryFinRep,//34
                    SearchQueryFinRep//35
                    );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }

            }
            catch (Exception ee)
            {
                return null;
            }

        }
        public DataSet GetAllFundedReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, string SearchText, bool IsPaid, string FilterText, string UserGroup, string TicketType, int? TicketId)
        {
            var IsPaidSalesQuery = "";
            var IsPaidTechQuery = "";
            var IsPaidMemberQuery = "";
            var IsPaidFinRepQuery = "";
            var IsPaidCallQuery = "";

            var IsPaidFollowUpQuery = "";
            var IsPaidRescheduleQuery = "";
            string DateFilterSale = "";
            string DateFilterTech = "";
            string DateFilterMember = "";
            string DateFilterFinRep = "";
            string DateFilterService = "";
            string DateFilterFollowUp = "";
            string DateFilterReschedule = "";
            string PaidDateFilter = "";
            string SearchQuery = "";
            string FilterTextQuerySales = "";
            string FilterTextQueryTech = "";
            string FilterTextQueryMember = "";
            string FilterTextQueryFinRep = "";
            string FilterTextQueryCall = "";

            string FilterTextQueryFollowUp = "";
            string FilterTextQueryReschedule = "";

            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
                //NameSql = NameSql.Replace("cus.", "cu.");
            }

            string UserGroupFilterQuery = "";
            string DeleteEmployeQuery = "";
            if (!string.IsNullOrWhiteSpace(UserGroup) && UserGroup != "undefined" && UserGroup != "null" && UserGroup != "-1")
            {

                if (UserGroup == "404")
                {
                    DeleteEmployeQuery = " and ul.IsDeleted = 1";
                }
                else
                {
                    UserGroupFilterQuery = string.Format("and pg.Id ='{0}'", UserGroup);
                    DeleteEmployeQuery = " and ul.IsDeleted = 0";
                }

            }
            string TicketIdFilterQuery = "";
            if (TicketId > 0)
            {
                TicketIdFilterQuery = string.Format(" and t.Id={0}", TicketId);
            }
            string ticketType = "";
            if (!string.IsNullOrWhiteSpace(TicketType) && TicketType != "''")
            {
                ticketType = string.Format("and t.TicketType in ({0})", TicketType);
            }

            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime() && string.IsNullOrEmpty(FilterText))
            {
                FilterStartDate = FilterStartDate.SetZeroHour();
                FilterEndDate = FilterEndDate.SetMaxHour();
                DateFilterSale = string.Format(" and salecom.CreatedDate between '{0}' and '{1}'"
                    , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));

                DateFilterTech = string.Format(" and techcom.CreatedDate between '{0}' and '{1}'"
                  , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                  , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));

                DateFilterMember = string.Format(" and addcom.CreatedDate between '{0}' and '{1}'"
                  , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                  , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));

                DateFilterFinRep = string.Format(" and finrepcom.CreatedDate between '{0}' and '{1}'"
                  , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                  , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));

                DateFilterService = string.Format(" and callcom.CreatedDate between '{0}' and '{1}'"
                  , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                  , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));

                DateFilterFollowUp = string.Format(" and followcom.CreatedDate between '{0}' and '{1}'"
                  , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                  , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));

                DateFilterReschedule = string.Format(" and reschedulecom.CreatedDate between '{0}' and '{1}'"
                  , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                  , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime() && !string.IsNullOrEmpty(FilterText))
            {
                PaidDateFilter = string.Format(" and PaidDate between '{0}' and '{1}'"
                  , FilterStartDate.SetZeroHour().ToString("yyyy-MM-dd HH:mm:ss")
                  , FilterEndDate.SetMaxHour().ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (IsPaid)
            {
                IsPaidSalesQuery = "where salecom.IsPaid = 1 ";
                IsPaidTechQuery = "where techcom.IsPaid = 1 ";
                IsPaidMemberQuery = "where addcom.IsPaid = 1 ";
                IsPaidFinRepQuery = "where finrepcom.IsPaid = 1 ";
                IsPaidCallQuery = "where callcom.IsPaid = 1 ";
                IsPaidFollowUpQuery = "where followcom.IsPaid = 1 ";
                IsPaidRescheduleQuery = "where reschedulecom.IsPaid =1 ";
            }
            else
            {
                IsPaidSalesQuery = " where (salecom.IsPaid = 0 or salecom.IsPaid is null)";
                IsPaidTechQuery = " where (techcom.IsPaid = 0 or techcom.IsPaid is  null)";
                IsPaidMemberQuery = " where (addcom.IsPaid = 0 or addcom.IsPaid is  null)";
                IsPaidFinRepQuery = " where (finrepcom.IsPaid = 0 or finrepcom.IsPaid is  null)";
                IsPaidCallQuery = " where (callcom.IsPaid = 0 or callcom.Ispaid is  null)";
                IsPaidFollowUpQuery = " where (followcom.IsPaid = 0 or followcom.IsPaid is  null)";
                IsPaidRescheduleQuery = " where (reschedulecom.IsPaid = 0 or reschedulecom.Ispaid is null)";
            }

            if (!string.IsNullOrEmpty(SearchText))
            {
                int searchId = 0;
                int.TryParse(SearchText, out searchId);
                SearchQuery = string.Format(" and t.Id = '{0}' ", searchId);

            }
            if (!string.IsNullOrEmpty(FilterText) && FilterText != "-1")
            {
                FilterTextQuerySales = string.Format(" and salecom.UserId = '{0}'", FilterText);
                FilterTextQueryTech = string.Format(" and techcom.UserId = '{0}'", FilterText);
                FilterTextQueryMember = string.Format(" and addcom.UserId = '{0}'", FilterText);
                FilterTextQueryFinRep = string.Format(" and finrepcom.UserId = '{0}'", FilterText);
                FilterTextQueryCall = string.Format(" and callcom.UserId = '{0}'", FilterText);
                FilterTextQueryFollowUp = string.Format(" and followcom.UserId = '{0}'", FilterText);
                FilterTextQueryReschedule = string.Format(" and reschedulecom.UserId = '{0}'", FilterText);

            }
            string sqlQuery = @"
                                DECLARE @NextDayID INT;
                                SET @NextDayID = 2; -- Next Monday
                                declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                 select * into #FundedCom  from ( select salecom.Id SalesId,0 TechId,0 AddmemberId,0 FinRepId,0 ServiceCallId,0 FollowUpId,0 RescheduleId,salecom.TicketId TicketId, cus.Id CustomerIdValue,{23} as CustomerName,t.Id ticid,lpttype.DisplayText as TicketType,salecom.RMRSOLD RMR,salecom.SalesCommissionId CommissionId,salecom.Adjustment Adjustment,salecom.Batch Batch,IsPaid IsPaid,salecom.TotalCommission SalesTotalCommission,0 TechTotalCommission,0 AddCommission,0 FinRepCommission,0 CallCommission,0 FollowUpCommission,0 RescheduleCommission,salecom.CompletionDate CompletionDate,(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                From Invoice where CustomerId=cus.CustomerId
                                and (Status='Open' or Status='Partial')) as BalanceDue, 'Sales' Type, emp.FirstName +' '+emp.LastName as Technician,salecom.PaidDate PaidDate,ISNULL(salecom.OriginalPoint,0) as OriginalPoint,DATEADD(DAY, (DATEDIFF(DAY, ((@NextDayID + 5) % 7), salecom.PaidDate) / 7) * 7 + 7, ((@NextDayID + 5) % 7)) as PayrollDate  from SalesCommission salecom
                                left join ticket t on t.ticketid = salecom.TicketId
                                LEFT JOIN Lookup lpttype on lpttype.DataValue=t.TicketType and lpttype.DataKey='TicketType'
                                left join customer cus on cus.CustomerId = salecom.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = salecom.UserId
                                
                                left join   UserLogin ul  on emp.UserId = ul.UserId
								left join UserPermission up on up.UserId = ul.UserId
								left join PermissionGroup pg on pg.Id = up.PermissionGroupId
                                
                                {3}{7}{11}{12}{16} and t.IsClosed = 1 and ce.IsTestAccount != 1 {24}{25}{26}{27}
                                UNION ALL
                                 select 0 SalesId,techcom.Id TechId,0 AddmemberId,0 FinRepId,0 ServiceCallId,0 FollowUpId,0 RescheduleId,techcom.TicketId TicketId,cus.Id CustomerIdValue,{23} as CustomerName,t.Id ticid,lpttype.DisplayText as TicketType,0 RMR,techcom.TechCommissionId CommissionId,techcom.Adjustment Adjustment,techcom.Batch Batch,techcom.IsPaid IsPaid,0 SalesTotalCommission,techcom.TotalCommission TechTotalCommission,0 AddCommission,0 FinRepCommission,0 CallCommission,0 FollowUpCommission,0 RescheduleCommission,techcom.CompletionDate CompletionDate,(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                                                From Invoice where CustomerId=cus.CustomerId
                                                                and (Status='Open' or Status='Partial')) as BalanceDue, 'Tech' Type, emp.FirstName +' '+emp.LastName as Technician,techcom.PaidDate PaidDate,ISNULL(techcom.OriginalPoint,0) as OriginalPoint,DATEADD(DAY, (DATEDIFF(DAY, ((@NextDayID + 5) % 7), techcom.PaidDate) / 7) * 7 + 7, ((@NextDayID + 5) % 7)) as PayrollDate  from TechCommission techcom
                                left join ticket t on t.ticketid = techcom.TicketId
                                LEFT JOIN Lookup lpttype on lpttype.DataValue=t.TicketType and lpttype.DataKey='TicketType'
                                left join customer cus on cus.CustomerId = techcom.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = techcom.UserId

                                left join   UserLogin ul  on emp.UserId = ul.UserId
								left join UserPermission up on up.UserId = ul.UserId
								left join PermissionGroup pg on pg.Id = up.PermissionGroupId

                                {4}{8}{11}{13}{16} and t.IsClosed = 1 and ce.IsTestAccount != 1 {24}{25}{26}{27}
                                UNION ALL
                                 select 0 SalesId,0 TechId,addcom.Id AddMemberId,0 FinRepId,0 ServiceCallId,0 FollowUpId,0 RescheduleId,addcom.TicketId TicketId,cus.Id CustomerIdValue,{23} as CustomerName,t.Id ticid,lpttype.DisplayText as TicketType,0 RMR,addcom.addmembercommissionId CommissionId,addcom.Adjustment Adjustment,addcom.Batch Batch,addcom.IsPaid IsPaid,0 SalesTotalCommission,0 TechTotalCommission,addcom.Commission AddCommission,0 FinRepCommission,0 CallCommission,0 FollowUpCommission,0 RescheduleCommission,addcom.CompletionDate CompletionDate,(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                                                From Invoice where CustomerId=cus.CustomerId
                                                                and (Status='Open' or Status='Partial')) as BalanceDue, 'Added Member' Type, emp.FirstName +' '+emp.LastName as Technician,addcom.PaidDate PaidDate,ISNULL(addcom.OriginalPoint,0) as OriginalPoint,DATEADD(DAY, (DATEDIFF(DAY, ((@NextDayID + 5) % 7), addcom.PaidDate) / 7) * 7 + 7, ((@NextDayID + 5) % 7)) as PayrollDate  from addmembercommission addcom
                                left join ticket t on t.ticketid = addcom.TicketId
                                LEFT JOIN Lookup lpttype on lpttype.DataValue=t.TicketType and lpttype.DataKey='TicketType'
                                left join customer cus on cus.CustomerId = addcom.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = addcom.UserId

                                left join   UserLogin ul  on emp.UserId = ul.UserId
								left join UserPermission up on up.UserId = ul.UserId
								left join PermissionGroup pg on pg.Id = up.PermissionGroupId

                                {5}{9}{11}{14}{16} and t.IsClosed = 1 and ce.IsTestAccount != 1 {24}{25}{26}{27}
                                UNION ALL
                                select 0 SalesId,0 TechId,0 AddMemberId,finrepcom.Id as FinRepId,0 ServiceCallId,0 FollowUpId,0 RescheduleId,finrepcom.TicketId TicketId,cus.Id CustomerIdValue,{23} as CustomerName,t.Id ticid,lpttype.DisplayText as TicketType,0 RMR,finrepcom.FinRepCommissionId CommissionId,finrepcom.Adjustment Adjustment,finrepcom.Batch Batch,finrepcom.IsPaid IsPaid,0 SalesTotalCommission,0 TechTotalCommission,0 AddCommission,finrepcom.Commission FinRepCommission,0 CallCommission,0 FollowUpCommission,0 RescheduleCommission,finrepcom.CompletionDate CompletionDate,(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                                                From Invoice where CustomerId=cus.CustomerId
                                                                and (Status='Open' or Status='Partial')) as BalanceDue, 'Fin Rep' Type, emp.FirstName +' '+emp.LastName as Technician,finrepcom.PaidDate PaidDate,ISNULL(finrepcom.OriginalPoint,0) as OriginalPoint,DATEADD(DAY, (DATEDIFF(DAY, ((@NextDayID + 5) % 7), finrepcom.PaidDate) / 7) * 7 + 7, ((@NextDayID + 5) % 7)) as PayrollDate  from FinRepCommission finrepcom
                                left join ticket t on t.ticketid = finrepcom.TicketId
                                LEFT JOIN Lookup lpttype on lpttype.DataValue=t.TicketType and lpttype.DataKey='TicketType'
                                left join customer cus on cus.CustomerId = finrepcom.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = finrepcom.UserId

                                left join   UserLogin ul  on emp.UserId = ul.UserId
								left join UserPermission up on up.UserId = ul.UserId
								left join PermissionGroup pg on pg.Id = up.PermissionGroupId

                                {28}{29}{11}{30}{16} and t.IsClosed = 1 and ce.IsTestAccount != 1 {24}{25}{26}{27}
                                UNION ALL
                                select 0 SalesId,0 TechId,0 AddmemberId,0 FinRepId,callcom.Id ServiceCallId,0 FollowUpId,0 RescheduleId,callcom.TicketId TicketId,cus.Id CustomerIdValue,{23} as CustomerName,t.Id ticid,lpttype.DisplayText as TicketType,0 RMR,callcom.servicecallcommissionId CommissionId,callcom.Adjustment Adjustment,callcom.Batch Batch,callcom.IsPaid IsPaid,0 SalesTotalCommission,0 TechTotalCommission,0 AddCommission,0 FinRepCommission,callcom.Commission CallCommission,0 FollowUpCommission,0 RescheduleCommission,callcom.CompletionDate CompletionDate,(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                                                From Invoice where CustomerId=cus.CustomerId
                                                                and (Status='Open' or Status='Partial')) as BalanceDue, 'Service Call' Type, emp.FirstName +' '+emp.LastName as Technician,callcom.PaidDate PaidDate,0 as OriginalPoint,DATEADD(DAY, (DATEDIFF(DAY, ((@NextDayID + 5) % 7), callcom.PaidDate) / 7) * 7 + 7, ((@NextDayID + 5) % 7)) as PayrollDate from ServiceCallCommission callcom
                                left join ticket t on t.ticketid = callcom.TicketId
                                LEFT JOIN Lookup lpttype on lpttype.DataValue=t.TicketType and lpttype.DataKey='TicketType'
                                left join customer cus on cus.CustomerId = callcom.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = callcom.UserId
                        
                                left join   UserLogin ul  on emp.UserId = ul.UserId
								left join UserPermission up on up.UserId = ul.UserId
								left join PermissionGroup pg on pg.Id = up.PermissionGroupId

                                {6}{10}{11}{15}{16} and t.IsClosed = 1 and ce.IsTestAccount != 1 {24}{25}{26}{27}

                                UNION ALL
                                select 0 SalesId,0 TechId,0 AddmemberId,0 FinRepId,0 ServiceCallId,followcom.Id FollowUpId,0 RescheduleId,followcom.TicketId TicketId,cus.Id CustomerIdValue,{23} as CustomerName,t.Id ticid,lpttype.DisplayText as TicketType,0 RMR,followcom.FollowUpCommissionId CommissionId,followcom.Adjustment Adjustment,followcom.Batch Batch,followcom.IsPaid IsPaid,0 SalesTotalCommission,0 TechTotalCommission,0 AddCommission,0 FinRepCommission,0 CallCommission,followcom.Commission FollowUpCommission,0 RescheduleCommission,followcom.CompletionDate CompletionDate,(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                From Invoice where CustomerId=cus.CustomerId
                                and (Status='Open' or Status='Partial')) as BalanceDue, 'Follow Up' Type, emp.FirstName +' '+emp.LastName as Technician,followcom.PaidDate PaidDate,0 as OriginalPoint,DATEADD(DAY, (DATEDIFF(DAY, ((@NextDayID + 5) % 7), followcom.PaidDate) / 7) * 7 + 7, ((@NextDayID + 5) % 7)) as PayrollDate from FollowUpCommission followcom
                                left join ticket t on t.ticketid = followcom.TicketId
                                LEFT JOIN Lookup lpttype on lpttype.DataValue=t.TicketType and lpttype.DataKey='TicketType'
                                left join customer cus on cus.CustomerId = followcom.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = followcom.UserId

                                left join   UserLogin ul  on emp.UserId = ul.UserId
								left join UserPermission up on up.UserId = ul.UserId
								left join PermissionGroup pg on pg.Id = up.PermissionGroupId

                                {17}{21}{11}{19}{16} and t.IsClosed = 1 and ce.IsTestAccount != 1 {24}{25}{26}{27}

                                UNION ALL
                                select 0 SalesId,0 TechId,0 AddmemberId,0 FinRepId,0 ServiceCallId,0 FollowUpId,reschedulecom.Id RescheduleId,reschedulecom.TicketId TicketId,cus.Id CustomerIdValue,{23} as CustomerName,t.Id ticid,lpttype.DisplayText as TicketType,0 RMR,reschedulecom.RescheduleCommissionId CommissionId,reschedulecom.Adjustment Adjustment,reschedulecom.Batch Batch,reschedulecom.IsPaid IsPaid,0 SalesTotalCommission,0 TechTotalCommission,0 AddCommission,0 FinRepCommission,0 CallCommission,0 FollowUpCommission,reschedulecom.Commission RescheduleCommission,reschedulecom.CompletionDate CompletionDate,(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                From Invoice where CustomerId=cus.CustomerId
                                and (Status='Open' or Status='Partial')) as BalanceDue, 'Reschedule Ticket' Type, emp.FirstName +' '+emp.LastName as Technician,reschedulecom.PaidDate PaidDate,0 as OriginalPoint,DATEADD(DAY, (DATEDIFF(DAY, ((@NextDayID + 5) % 7), reschedulecom.PaidDate) / 7) * 7 + 7, ((@NextDayID + 5) % 7)) as PayrollDate from RescheduleCommission reschedulecom
                                left join ticket t on t.ticketid = reschedulecom.TicketId
                                LEFT JOIN Lookup lpttype on lpttype.DataValue=t.TicketType and lpttype.DataKey='TicketType'
                                left join customer cus on cus.CustomerId = reschedulecom.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = reschedulecom.UserId
        
                                left join   UserLogin ul  on emp.UserId = ul.UserId
								left join UserPermission up on up.UserId = ul.UserId
								left join PermissionGroup pg on pg.Id = up.PermissionGroupId

                                 {18}{22}{11}{20}{16} and t.IsClosed = 1 and ce.IsTestAccount != 1 {24}{25}{26}{27}
                         
     
                                ) d	

                                 select * into #FundedComFilter
								from #FundedCom
                                order by PaidDate desc

								select top(@pagesize)
								* into #SumTable from #FundedComFilter
								where TicketId not in(select top(@pagestart) TicketId from #FundedComFilter #e {1})
                                {2}
								select count(*) CountTotal
                                from #FundedComFilter
                                
								select * from #SumTable
								select sum(Adjustment) as TotalAdjustment
								,sum(SalesTotalCommission) as TotalSalesCommission
								,sum(TechTotalCommission) as TotalTechCommission
								,sum(AddCommission) as TotalAddCommission
                                ,sum(FinRepCommission) as TotalFinRepCommission
								,sum(CallCommission) as TotalCallCommission
								,sum(FollowUpCommission) as TotalFollowUpCommission
								,sum(RescheduleCommission) as TotalRescheduleCommission
								,sum(BalanceDue) as TotalUnpaid
								,sum(OriginalPoint) as TotalPoint
								from #SumTable
                                
								drop table #FundedCom
								drop table #FundedComFilter
								drop table #SumTable";

            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/ticketid")
                {
                    subquery = "order by #e.ticid asc";
                    subquery1 = "order by ticid asc";
                }
                else if (order == "descending/ticketid")
                {
                    subquery = "order by #e.ticid desc";
                    subquery1 = "order by ticid desc";
                }
                else if (order == "ascending/sales")
                {
                    subquery = "order by #e.SalesTotalCommission asc";
                    subquery1 = "order by SalesTotalCommission asc";
                }
                else if (order == "descending/sales")
                {
                    subquery = "order by #e.SalesTotalCommission desc";
                    subquery1 = "order by SalesTotalCommission desc";
                }
                else if (order == "ascending/tech")
                {
                    subquery = "order by #e.TechTotalCommission asc";
                    subquery1 = "order by TechTotalCommission asc";
                }
                else if (order == "descending/tech")
                {
                    subquery = "order by #e.TechTotalCommission desc";
                    subquery1 = "order by TechTotalCommission desc";
                }
                else if (order == "ascending/addedmember")
                {
                    subquery = "order by #e.AddCommission asc";
                    subquery1 = "order by AddCommission asc";
                }
                else if (order == "descending/addedmember")
                {
                    subquery = "order by #e.AddCommission desc";
                    subquery1 = "order by AddCommission desc";
                }
                else if (order == "ascending/callcommission")
                {
                    subquery = "order by #e.CallCommission asc";
                    subquery1 = "order by CallCommission asc";
                }
                else if (order == "descending/callcommission")
                {
                    subquery = "order by #e.CallCommission desc";
                    subquery1 = "order by CallCommission desc";
                }
                else if (order == "ascending/followup")
                {
                    subquery = "order by #e.FollowUpCommission asc";
                    subquery1 = "order by FollowUpCommission asc";
                }
                else if (order == "descending/followup")
                {
                    subquery = "order by #e.FollowUpCommission desc";
                    subquery1 = "order by FollowUpCommission desc";
                }

                else if (order == "ascending/reschedule")
                {
                    subquery = "order by #e.RescheduleCommission asc";
                    subquery1 = "order by RescheduleCommission asc";
                }
                else if (order == "descending/reschedule")
                {
                    subquery = "order by #e.RescheduleCommission desc";
                    subquery1 = "order by RescheduleCommission desc";
                }
                else if (order == "ascending/balancedue")
                {
                    subquery = "order by #e.CompletionDate asc";
                    subquery1 = "order by CompletionDate asc";
                }
                else if (order == "descending/balancedue")
                {
                    subquery = "order by #e.CompletionDate desc";
                    subquery1 = "order by CompletionDate desc";
                }
                else if (order == "descending/type")
                {
                    subquery = "order by #e.Type desc";
                    subquery1 = "order by Type desc";
                }
                else if (order == "ascending/type")
                {
                    subquery = "order by #e.Type asc";
                    subquery1 = "order by Type asc";
                }

                else if (order == "descending/tickettype")
                {
                    subquery = "order by #e.TicketType desc";
                    subquery1 = "order by TicketType desc";
                }
                else if (order == "ascending/tickettype")
                {
                    subquery = "order by #e.TicketType asc";
                    subquery1 = "order by TicketType asc";
                }

                else if (order == "descending/batchno")
                {
                    subquery = "order by #e.Batch desc";
                    subquery1 = "order by Batch desc";
                }
                else if (order == "ascending/batchno")
                {
                    subquery = "order by #e.Batch asc";
                    subquery1 = "order by Batch asc";
                }

                else if (order == "descending/paiddate")
                {
                    subquery = "order by #e.PaidDate desc";
                    subquery1 = "order by PaidDate desc";
                }
                else if (order == "ascending/paiddate")
                {
                    subquery = "order by #e.PaidDate asc";
                    subquery1 = "order by PaidDate asc";
                }

                else if (order == "descending/username")
                {
                    subquery = "order by #e.Technician desc";
                    subquery1 = "order by Technician desc";
                }
                else if (order == "ascending/username")
                {
                    subquery = "order by #e.Technician asc";
                    subquery1 = "order by Technician asc";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(FilterText))
                {
                    subquery = "order by #e.PaidDate desc";
                    subquery1 = "order by PaidDate desc";
                }
                else
                {
                    if (IsPaid)
                    {
                        subquery = "order by #e.PaidDate desc";
                        subquery1 = "order by PaidDate desc";
                    }
                    else
                    {
                        subquery = "order by #e.ticid desc";
                        subquery1 = "order by ticid desc";
                    }

                }

            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                    order,//0
                    subquery,//1 
                    subquery1,//2
                    IsPaidSalesQuery,//3
                    IsPaidTechQuery,//4
                    IsPaidMemberQuery,//5
                    IsPaidCallQuery,//6
                    DateFilterSale,//7
                    DateFilterTech,//8
                    DateFilterMember,//9
                    DateFilterService,//10 
                    SearchQuery,//11 
                    FilterTextQuerySales,//12
                    FilterTextQueryTech,//13
                    FilterTextQueryMember,//14
                    FilterTextQueryCall,//15
                    PaidDateFilter,//16
                    IsPaidFollowUpQuery,//17
                    IsPaidRescheduleQuery,//18
                    FilterTextQueryFollowUp,//19
                    FilterTextQueryReschedule,//20
                    DateFilterFollowUp,//21
                    DateFilterReschedule,//22
                    NameSql,//23
                    UserGroupFilterQuery,//24
                    DeleteEmployeQuery,//25
                    ticketType,//26
                    TicketIdFilterQuery,//27
                    IsPaidFinRepQuery,//28
                    FilterTextQueryFollowUp,//29
                    DateFilterFinRep);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }

            }
            catch (Exception ee)
            {
                return null;
            }

        }


        public DataSet GetAllAdjustmentFundingReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, bool IsPaid, string FilterText, string SearchText)
        {
            string DateFilterSale = "";
            string IsPaidSalesQuery = "";
            string FilterTextQuerySales = "";
            string SearchTextQuery = "";

            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime() && string.IsNullOrEmpty(FilterText))
            {
                FilterStartDate = FilterStartDate.SetZeroHour();
                FilterEndDate = FilterEndDate.SetMaxHour();
                DateFilterSale = string.Format(" and af.Date between '{0}' and '{1}'"
                    , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));

            }
            if (!string.IsNullOrEmpty(SearchText))
            {
                int Batch = 0;
                int.TryParse(SearchText, out Batch);
                if (Batch > 0)
                {
                    SearchTextQuery = string.Format(" and af.Batch = '{0}'", Batch);
                }
                else
                {
                    SearchTextQuery = string.Format(" and emp.FirstName+' '+emp.LastName like @SearchText", SearchText);
                }
            }
            if (IsPaid)
            {
                IsPaidSalesQuery = "where af.IsPaid = 1 ";

            }
            else
            {
                IsPaidSalesQuery = " where (af.IsPaid = 0 or af.IsPaid is null)";

            }
            if (!string.IsNullOrEmpty(FilterText) && FilterText != "-1")
            {
                FilterTextQuerySales = string.Format(" and af.UserId = '{0}'", FilterText);
            }
            string sqlQuery = @"
                                DECLARE @NextDayID INT;
                                SET @NextDayID = 2; -- Next Monday
                                declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                select * into #AdjustmentFunding from (select af.*,emp.FirstName+' '+emp.LastName as UserName,
                                DATEADD(DAY, (DATEDIFF(DAY, ((@NextDayID + 5) % 7), af.PaidDate) / 7) * 7 + 7, ((@NextDayID + 5) % 7)) as PayrollDate                                
                                from AdjustmentFunding af
                                left join employee emp on  emp.UserId = af.UserId

                                {2}{4}{5}
                                --{3}
                                ) d	

                                select * into #AdjustmentFundingFilter
								from #AdjustmentFunding
                                order by PaidDate desc

								select top(@pagesize)
								* into #TestTable from #AdjustmentFundingFilter
								where Id not in(select top(@pagestart) Id from #AdjustmentFundingFilter #e {0})
                                {1}
								select count(*) CountTotal
                                from #AdjustmentFundingFilter
                                
								select * from #TestTable
								select sum(Amount) as TotalAmount from #TestTable
                                
								drop table #AdjustmentFunding
								drop table #AdjustmentFundingFilter
								drop table #TestTable";

            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/username")
                {
                    subquery = "order by #e.UserName asc";
                    subquery1 = "order by UserName asc";
                }
                else if (order == "descending/username")
                {
                    subquery = "order by #e.UserName desc";
                    subquery1 = "order by UserName desc";
                }
                else if (order == "ascending/amount")
                {
                    subquery = "order by #e.Amount asc";
                    subquery1 = "order by Amount asc";
                }
                else if (order == "descending/amount")
                {
                    subquery = "order by #e.SalesTotalCommission desc";
                    subquery1 = "order by SalesTotalCommission desc";
                }
                else if (order == "ascending/reason")
                {
                    subquery = "order by #e.Reason asc";
                    subquery1 = "order by Reason asc";
                }
                else if (order == "descending/reason")
                {
                    subquery = "order by #e.Reason desc";
                    subquery1 = "order by Reason desc";
                }
                else if (order == "ascending/date")
                {
                    subquery = "order by #e.Date asc";
                    subquery1 = "order by Date asc";
                }
                else if (order == "descending/date")
                {
                    subquery = "order by #e.Date desc";
                    subquery1 = "order by Date desc";
                }
                else if (order == "ascending/callcommission")
                {
                    subquery = "order by #e.CallCommission asc";
                    subquery1 = "order by CallCommission asc";
                }
                else if (order == "descending/callcommission")
                {
                    subquery = "order by #e.CallCommission desc";
                    subquery1 = "order by CallCommission desc";
                }
                else if (order == "ascending/followup")
                {
                    subquery = "order by #e.FollowUpCommission asc";
                    subquery1 = "order by FollowUpCommission asc";
                }
                else if (order == "descending/followup")
                {
                    subquery = "order by #e.FollowUpCommission desc";
                    subquery1 = "order by FollowUpCommission desc";
                }

                else if (order == "ascending/reschedule")
                {
                    subquery = "order by #e.RescheduleCommission asc";
                    subquery1 = "order by RescheduleCommission asc";
                }
                else if (order == "descending/reschedule")
                {
                    subquery = "order by #e.RescheduleCommission desc";
                    subquery1 = "order by RescheduleCommission desc";
                }
                else if (order == "ascending/balancedue")
                {
                    subquery = "order by #e.CompletionDate asc";
                    subquery1 = "order by CompletionDate asc";
                }
                else if (order == "descending/balancedue")
                {
                    subquery = "order by #e.CompletionDate desc";
                    subquery1 = "order by CompletionDate desc";
                }
                else if (order == "descending/type")
                {
                    subquery = "order by #e.Type desc";
                    subquery1 = "order by Type desc";
                }
                else if (order == "ascending/type")
                {
                    subquery = "order by #e.Type asc";
                    subquery1 = "order by Type asc";
                }

                else if (order == "descending/tickettype")
                {
                    subquery = "order by #e.TicketType desc";
                    subquery1 = "order by TicketType desc";
                }
                else if (order == "ascending/tickettype")
                {
                    subquery = "order by #e.TicketType asc";
                    subquery1 = "order by TicketType asc";
                }

                else if (order == "descending/batch")
                {
                    subquery = "order by #e.Batch desc";
                    subquery1 = "order by Batch desc";
                }
                else if (order == "ascending/batch")
                {
                    subquery = "order by #e.Batch asc";
                    subquery1 = "order by Batch asc";
                }

                else if (order == "descending/paiddate")
                {
                    subquery = "order by #e.PaidDate desc";
                    subquery1 = "order by PaidDate desc";
                }
                else if (order == "ascending/paiddate")
                {
                    subquery = "order by #e.PaidDate asc";
                    subquery1 = "order by PaidDate asc";
                }

                else if (order == "descending/username")
                {
                    subquery = "order by #e.Technician desc";
                    subquery1 = "order by Technician desc";
                }
                else if (order == "ascending/username")
                {
                    subquery = "order by #e.Technician asc";
                    subquery1 = "order by Technician asc";
                }
                else
                {
                    subquery = "order by #e.Id desc";
                    subquery1 = "order by Id desc";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(FilterText))
                {
                    subquery = "order by #e.PaidDate desc";
                    subquery1 = "order by PaidDate desc";
                }
                else
                {
                    if (IsPaid)
                    {
                        subquery = "order by #e.PaidDate desc";
                        subquery1 = "order by PaidDate desc";
                    }
                    else
                    {
                        subquery = "order by #e.Id desc";
                        subquery1 = "order by Id desc";
                    }

                }


            }
            try
            {
                sqlQuery = string.Format(sqlQuery, subquery, subquery1, IsPaidSalesQuery, DateFilterSale, FilterTextQuerySales, SearchTextQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", SearchText)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }

            }
            catch (Exception ee)
            {
                return null;
            }

        }

        public DataSet GetDownloadAllFundedReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, string SearchText, bool IsPaid, string FilterText, string from, string UserGroup, string TicketType)
        {
            var IsPaidSalesQuery = "";
            var IsPaidTechQuery = "";
            var IsPaidMemberQuery = "";
            var IsPaidFinRepQuery = "";
            var IsPaidCallQuery = "";
            string BatchQuery = "";
            string PayrollDateQuery = "";
            var IsPaidFollowUpQuery = "";
            var IsPaidRescheduleQuery = "";
            string DateFilterSale = "";
            string DateFilterTech = "";
            string DateFilterMember = "";
            string DateFilterFinRep = "";
            string DateFilterService = "";
            string DateFilterFollowUp = "";
            string DateFilterReschedule = "";
            string PaidDateFilter = "";
            string SearchQuerySales = "";
            string SearchQueryTech = "";
            string SearchQueryAddMember = "";
            string SearchQueryFinRep = "";
            string SearchQueryServiceCall = "";
            string SearchQueryFollowUp = "";
            string SearchQueryReschedule = "";
            string FilterTextQuerySales = "";
            string FilterTextQueryTech = "";
            string FilterTextQueryMember = "";
            string FilterTextQueryFinRep = "";
            string FilterTextQueryCall = "";

            string FilterTextQueryFollowUp = "";
            string FilterTextQueryReschedule = "";

            string QueryString = "";

            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
                //NameSql = NameSql.Replace("cus.", "cu.");
            }

            if (from == "UserPayment")
            {
                QueryString = "select  CustomerIdValue [Customer Id],CSID [CS ID],Technician as [User],Case when SalesTotalCommission > 0 Then format(SalesTotalCommission,'N2') When TechTotalCommission> 0 Then format(TechTotalCommission,'N2') when AddCommission> 0 Then format(AddCommission,'N2') When CallCommission> 0 Then format(CallCommission,'N2') when FollowUpCommission> 0 Then format(FollowUpCommission,'N2') When RescheduleCommission> 0 Then format(RescheduleCommission,'N2') END AS Amount,Type as [Site Type],ticid[Ticket Details],TicketType[Ticket Type],TRY_CONVERT(date,PaidDate) as [Paid Date]";
            }
            else
            {
                QueryString = @"select CustomerIdValue [Customer Id],
                                CSID[CS ID],
								Technician[User],
								ticid[Ticket Id],
								TicketType,
								CustomerName,
								CompletionDate,
								ROUND(RMR,2)    as  [RMR],
								ROUND(SalesTotalCommission, 2) as  [Sales],
								ROUND(TechTotalCommission, 2) as [Techs],
								ROUND(AddCommission, 2) as [Added Members],
                                ROUND(FinRepCommission, 2) as [Fin Rep],
								ROUND(CallCommission, 2) as [Service Calls],
								ROUND(FollowUpCommission, 2) as  [Follow Up],
								ROUND(RescheduleCommission, 2) as  [Reschedule],
								ROUND(OriginalPoint, 2) as Point,
								ROUND(BalanceDue, 2) as [Unpaid Balance]";
            }

            string UserGroupFilterQuery = "";
            string DeleteEmployeQuery = "";
            if (!string.IsNullOrWhiteSpace(UserGroup) && UserGroup != "undefined" && UserGroup != "null" && UserGroup != "-1")
            {

                if (UserGroup == "404")
                {
                    DeleteEmployeQuery = " and ul.IsDeleted = 1";
                }
                else
                {
                    UserGroupFilterQuery = string.Format("and pg.Id ='{0}'", UserGroup);
                    DeleteEmployeQuery = " and ul.IsDeleted = 0";
                }

            }

            string ticketType = "";
            if (!string.IsNullOrWhiteSpace(TicketType) && TicketType != "''" && TicketType != "'null'")
            {
                ticketType = string.Format("and t.TicketType in ({0})", TicketType);
            }

            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime() && string.IsNullOrEmpty(FilterText))
            {
                FilterStartDate = FilterStartDate.SetZeroHour();
                FilterEndDate = FilterEndDate.SetMaxHour();
                DateFilterSale = string.Format(" and salecom.CreatedDate between '{0}' and '{1}'"
                    , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));

                DateFilterTech = string.Format(" and techcom.CreatedDate between '{0}' and '{1}'"
                  , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                  , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));

                DateFilterMember = string.Format(" and addcom.CreatedDate between '{0}' and '{1}'"
                  , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                  , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));

                DateFilterFinRep = string.Format(" and finrep.CreatedDate between '{0}' and '{1}'"
                  , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                  , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));

                DateFilterService = string.Format(" and callcom.CreatedDate between '{0}' and '{1}'"
                  , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                  , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));

                DateFilterFollowUp = string.Format(" and followcom.CreatedDate between '{0}' and '{1}'"
                  , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                  , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));

                DateFilterReschedule = string.Format(" and reschedulecom.CreatedDate between '{0}' and '{1}'"
                  , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                  , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime() && !string.IsNullOrEmpty(FilterText))
            {
                PaidDateFilter = string.Format(" and PaidDate between '{0}' and '{1}'"
                  , FilterStartDate.SetZeroHour().ToString("yyyy-MM-dd HH:mm:ss")
                  , FilterEndDate.SetMaxHour().ToString("yyyy-MM-dd HH:mm:ss"));
            }
            if (IsPaid)
            {
                IsPaidSalesQuery = "where salecom.IsPaid = 1 ";
                IsPaidTechQuery = "where techcom.IsPaid = 1 ";
                IsPaidMemberQuery = "where addcom.IsPaid = 1 ";
                IsPaidFinRepQuery = "where finrep.IsPaid = 1 ";
                IsPaidCallQuery = "where callcom.IsPaid = 1 ";
                IsPaidFollowUpQuery = "where followcom.IsPaid = 1 ";
                IsPaidRescheduleQuery = "where reschedulecom.IsPaid =1 ";
                BatchQuery = ",Batch [Original Batch]";
                PayrollDateQuery = ",PayrollDate";
            }
            else
            {
                IsPaidSalesQuery = " where (salecom.IsPaid = 0 or salecom.IsPaid is null)";
                IsPaidTechQuery = " where (techcom.IsPaid = 0 or techcom.IsPaid is  null)";
                IsPaidMemberQuery = " where (addcom.IsPaid = 0 or addcom.IsPaid is  null)";
                IsPaidFinRepQuery = " where (finrep.IsPaid = 0 or finrep.IsPaid is  null)";
                IsPaidCallQuery = " where (callcom.IsPaid = 0 or callcom.Ispaid is  null)";
                IsPaidFollowUpQuery = " where (followcom.IsPaid = 0 or followcom.IsPaid is  null)";
                IsPaidRescheduleQuery = " where (reschedulecom.IsPaid = 0 or reschedulecom.Ispaid is null)";

            }

            if (!string.IsNullOrEmpty(SearchText))
            {
                int searchId = 0;
                int.TryParse(SearchText, out searchId);
                SearchQuerySales = string.Format(" and (t.Id = '{0}' OR salecom.Batch= '{0}') ", searchId);
                SearchQueryTech = string.Format(" and (t.Id = '{0}' OR techcom.Batch= '{0}') ", searchId);
                SearchQueryAddMember = string.Format(" and (t.Id = '{0}' OR addcom.Batch= '{0}') ", searchId);
                SearchQueryFinRep = string.Format(" and (t.Id = '{0}' OR finrep.Batch= '{0}') ", searchId);
                SearchQueryServiceCall = string.Format(" and (t.Id = '{0}' OR callcom.Batch= '{0}') ", searchId);
                SearchQueryFollowUp = string.Format(" and (t.Id = '{0}' OR followcom.Batch= '{0}') ", searchId);
                SearchQueryReschedule = string.Format(" and (t.Id = '{0}' OR reschedulecom.Batch= '{0}') ", searchId);
            }
            if (!string.IsNullOrEmpty(FilterText) && FilterText != "-1")
            {
                FilterTextQuerySales = string.Format(" and salecom.UserId = '{0}'", FilterText);
                FilterTextQueryTech = string.Format(" and techcom.UserId = '{0}'", FilterText);
                FilterTextQueryMember = string.Format(" and addcom.UserId = '{0}'", FilterText);
                FilterTextQueryFinRep = string.Format(" and finrep.UserId = '{0}'", FilterText);
                FilterTextQueryCall = string.Format(" and callcom.UserId = '{0}'", FilterText);
                FilterTextQueryFollowUp = string.Format(" and followcom.UserId = '{0}'", FilterText);
                FilterTextQueryReschedule = string.Format(" and reschedulecom.UserId = '{0}'", FilterText);

            }
            string sqlQuery = @"
                                DECLARE @NextDayID INT;
                                SET @NextDayID = 2; -- Next Monday
                                declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                 select * into #FundedCom  from ( select salecom.Id SalesId,0 TechId,0 AddmemberId,0 FinRepId,0 ServiceCallId,0 FollowUpId,0 RescheduleId,salecom.TicketId TicketId, cus.Id CustomerIdValue,cus.CustomerNo CSID,{23} as CustomerName,t.Id ticid,lpttype.DisplayText as TicketType,ISNULL(salecom.RMRSOLD,0) RMR,salecom.SalesCommissionId CommissionId,salecom.Adjustment Adjustment,salecom.Batch Batch,IsPaid IsPaid,salecom.TotalCommission SalesTotalCommission,0 TechTotalCommission,0 AddCommission,0 FinRepCommission,0 CallCommission,0 FollowUpCommission,0 RescheduleCommission,CONVERT(date, t.CompletedDate) CompletionDate,(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                From Invoice where CustomerId=cus.CustomerId
                                and (Status='Open' or Status='Partial')) as BalanceDue, 'Sales' Type, emp.FirstName +' '+emp.LastName as Technician,salecom.PaidDate PaidDate,ISNULL(salecom.OriginalPoint,0) as OriginalPoint,DATEADD(DAY, (DATEDIFF(DAY, ((@NextDayID + 5) % 7), salecom.PaidDate) / 7) * 7 + 7, ((@NextDayID + 5) % 7)) as PayrollDate  from SalesCommission salecom
                                left join ticket t on t.ticketid = salecom.TicketId
                                LEFT JOIN Lookup lpttype on lpttype.DataValue=t.TicketType and lpttype.DataKey='TicketType'
                                left join customer cus on cus.CustomerId = salecom.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = salecom.UserId

                                left join   UserLogin ul  on emp.UserId = ul.UserId
								left join UserPermission up on up.UserId = ul.UserId
								left join PermissionGroup pg on pg.Id = up.PermissionGroupId

                                {3}{7}{11}{12}{16}  and t.IsClosed = 1 and ce.IsTestAccount != 1 {25}{26}{27}
                                UNION ALL
                                 select 0 SalesId,techcom.Id TechId,0 AddmemberId,0 FinRepId,0 ServiceCallId,0 FollowUpId,0 RescheduleId,techcom.TicketId TicketId,cus.Id CustomerIdValue,cus.CustomerNo CSID,{23} as CustomerName,t.Id ticid,lpttype.DisplayText as TicketType,0 RMR,techcom.TechCommissionId CommissionId,techcom.Adjustment Adjustment,techcom.Batch Batch,techcom.IsPaid IsPaid,0 SalesTotalCommission,techcom.TotalCommission TechTotalCommission,0 AddCommission,0 FinRepCommission,0 CallCommission,0 FollowUpCommission,0 RescheduleCommission,CONVERT(date, t.CompletedDate) CompletionDate,(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                                                From Invoice where CustomerId=cus.CustomerId
                                                                and (Status='Open' or Status='Partial')) as BalanceDue, 'Tech' Type, emp.FirstName +' '+emp.LastName as Technician,techcom.PaidDate PaidDate,ISNULL(techcom.OriginalPoint,0) as OriginalPoint ,DATEADD(DAY, (DATEDIFF(DAY, ((@NextDayID + 5) % 7), techcom.PaidDate) / 7) * 7 + 7, ((@NextDayID + 5) % 7)) as PayrollDate  from TechCommission techcom
                                left join ticket t on t.ticketid = techcom.TicketId
                                LEFT JOIN Lookup lpttype on lpttype.DataValue=t.TicketType and lpttype.DataKey='TicketType'
                                left join customer cus on cus.CustomerId = techcom.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = techcom.UserId

                                left join   UserLogin ul  on emp.UserId = ul.UserId
								left join UserPermission up on up.UserId = ul.UserId
								left join PermissionGroup pg on pg.Id = up.PermissionGroupId

                                {4}{8}{28}{13}{16}  and t.IsClosed = 1 and ce.IsTestAccount != 1 {25}{26}{27}
                               UNION ALL
                                select 0 SalesId,0 TechId,addcom.Id AddMemberId,0 FinRepId,0 ServiceCallId,0 FollowUpId,0 RescheduleId,addcom.TicketId TicketId,cus.Id CustomerIdValue,cus.CustomerNo CSID,{23} as CustomerName,t.Id ticid,lpttype.DisplayText as TicketType,0 RMR,addcom.addmembercommissionId CommissionId,addcom.Adjustment Adjustment,addcom.Batch Batch,addcom.IsPaid IsPaid,0 SalesTotalCommission,0 TechTotalCommission,addcom.Commission AddCommission,0 FinRepCommission,0 CallCommission,0 FollowUpCommission,0 RescheduleCommission,CONVERT(date, t.CompletedDate) CompletionDate,(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                                                From Invoice where CustomerId=cus.CustomerId
                                                                and (Status='Open' or Status='Partial')) as BalanceDue, 'Added Member' Type, emp.FirstName +' '+emp.LastName as Technician,addcom.PaidDate PaidDate,ISNULL(addcom.OriginalPoint,0) as OriginalPoint ,DATEADD(DAY, (DATEDIFF(DAY, ((@NextDayID + 5) % 7), addcom.PaidDate) / 7) * 7 + 7, ((@NextDayID + 5) % 7)) as PayrollDate  from addmembercommission addcom
                                left join ticket t on t.ticketid = addcom.TicketId
                                LEFT JOIN Lookup lpttype on lpttype.DataValue=t.TicketType and lpttype.DataKey='TicketType'
                                left join customer cus on cus.CustomerId = addcom.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = addcom.UserId

                                left join   UserLogin ul  on emp.UserId = ul.UserId
								left join UserPermission up on up.UserId = ul.UserId
								left join PermissionGroup pg on pg.Id = up.PermissionGroupId

                                {5}{9}{29}{14}{16}  and t.IsClosed = 1 and ce.IsTestAccount != 1 {25}{26}{27}
                                UNION ALL
                                select 0 SalesId,0 TechId,0 AddMemberId,finrep.Id FinRepId,0 ServiceCallId,0 FollowUpId,0 RescheduleId,finrep.TicketId TicketId,cus.Id CustomerIdValue,cus.CustomerNo CSID,{23} as CustomerName,t.Id ticid,lpttype.DisplayText as TicketType,0 RMR,finrep.FinRepcommissionId CommissionId,finrep.Adjustment Adjustment,finrep.Batch Batch,finrep.IsPaid IsPaid,0 SalesTotalCommission,0 TechTotalCommission,0 AddCommission,finrep.Commission FinRepCommission,0 CallCommission,0 FollowUpCommission,0 RescheduleCommission,CONVERT(date, t.CompletedDate) CompletionDate,(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                                                From Invoice where CustomerId=cus.CustomerId
                                                                and (Status='Open' or Status='Partial')) as BalanceDue, 'Fin Rep' Type, emp.FirstName +' '+emp.LastName as Technician,finrep.PaidDate PaidDate,ISNULL(finrep.OriginalPoint,0) as OriginalPoint ,DATEADD(DAY, (DATEDIFF(DAY, ((@NextDayID + 5) % 7), finrep.PaidDate) / 7) * 7 + 7, ((@NextDayID + 5) % 7)) as PayrollDate  from FinRepCommission finrep
                                left join ticket t on t.ticketid = finrep.TicketId
                                LEFT JOIN Lookup lpttype on lpttype.DataValue=t.TicketType and lpttype.DataKey='TicketType'
                                left join customer cus on cus.CustomerId = finrep.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = finrep.UserId

                                left join   UserLogin ul  on emp.UserId = ul.UserId
								left join UserPermission up on up.UserId = ul.UserId
								left join PermissionGroup pg on pg.Id = up.PermissionGroupId

                                {34}{35}{36}{37}{16}  and t.IsClosed = 1 and ce.IsTestAccount != 1 {25}{26}{27}
                                UNION ALL
                                select 0 SalesId,0 TechId,0 AddmemberId,0 FinRepId,callcom.Id ServiceCallId,0 FollowUpId,0 RescheduleId,callcom.TicketId TicketId,cus.Id CustomerIdValue,cus.CustomerNo CSID,{23} as CustomerName,t.Id ticid,lpttype.DisplayText as TicketType,0 RMR,callcom.servicecallcommissionId CommissionId,callcom.Adjustment Adjustment,callcom.Batch Batch,callcom.IsPaid IsPaid,0 SalesTotalCommission,0 TechTotalCommission,0 AddCommission,0 FinRepCommission,callcom.Commission CallCommission,0 FollowUpCommission,0 RescheduleCommission,CONVERT(date, t.CompletedDate) CompletionDate,(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                                                From Invoice where CustomerId=cus.CustomerId
                                                                and (Status='Open' or Status='Partial')) as BalanceDue, 'Service Call' Type, emp.FirstName +' '+emp.LastName as Technician,callcom.PaidDate PaidDate,0 as OriginalPoint,DATEADD(DAY, (DATEDIFF(DAY, ((@NextDayID + 5) % 7), callcom.PaidDate) / 7) * 7 + 7, ((@NextDayID + 5) % 7)) as PayrollDate  from ServiceCallCommission callcom
                                left join ticket t on t.ticketid = callcom.TicketId
                                LEFT JOIN Lookup lpttype on lpttype.DataValue=t.TicketType and lpttype.DataKey='TicketType'
                                left join customer cus on cus.CustomerId = callcom.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = callcom.UserId

                                left join   UserLogin ul  on emp.UserId = ul.UserId
								left join UserPermission up on up.UserId = ul.UserId
								left join PermissionGroup pg on pg.Id = up.PermissionGroupId

                                {6}{10}{30}{15}{16}  and t.IsClosed = 1 and ce.IsTestAccount != 1 {25}{26}{27}

                                UNION ALL
                                select 0 SalesId,0 TechId,0 AddmemberId,0 FinRepId,0 ServiceCallId,followcom.Id FollowUpId,0 RescheduleId,followcom.TicketId TicketId,cus.Id CustomerIdValue,cus.CustomerNo CSID,{23} as CustomerName,t.Id ticid,lpttype.DisplayText as TicketType,0 RMR,followcom.FollowUpCommissionId CommissionId,followcom.Adjustment Adjustment,followcom.Batch Batch,followcom.IsPaid IsPaid,0 SalesTotalCommission,0 TechTotalCommission,0 AddCommission,0 FinRepCommission,0 CallCommission,followcom.Commission FollowUpCommission,0 RescheduleCommission,CONVERT(date, t.CompletedDate) CompletionDate,(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                From Invoice where CustomerId=cus.CustomerId
                                and (Status='Open' or Status='Partial')) as BalanceDue, 'Follow Up' Type, emp.FirstName +' '+emp.LastName as Technician,followcom.PaidDate PaidDate,0 as OriginalPoint,DATEADD(DAY, (DATEDIFF(DAY, ((@NextDayID + 5) % 7), followcom.PaidDate) / 7) * 7 + 7, ((@NextDayID + 5) % 7)) as PayrollDate  from FollowUpCommission followcom
                                left join ticket t on t.ticketid = followcom.TicketId
                                LEFT JOIN Lookup lpttype on lpttype.DataValue=t.TicketType and lpttype.DataKey='TicketType'
                                left join customer cus on cus.CustomerId = followcom.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = followcom.UserId
        
                                left join   UserLogin ul  on emp.UserId = ul.UserId
								left join UserPermission up on up.UserId = ul.UserId
								left join PermissionGroup pg on pg.Id = up.PermissionGroupId

                                {17}{21}{31}{19}{16} and t.IsClosed = 1 and ce.IsTestAccount != 1 {25}{26}{27}

                                UNION ALL 
                                select 0 SalesId,0 TechId,0 AddmemberId,0 FinRepId,0 ServiceCallId,0 FollowUpId,reschedulecom.Id RescheduleId,reschedulecom.TicketId TicketId,cus.Id CustomerIdValue,cus.CustomerNo CSID,{23} as CustomerName,t.Id ticid,lpttype.DisplayText as TicketType,0 RMR,reschedulecom.RescheduleCommissionId CommissionId,reschedulecom.Adjustment Adjustment,reschedulecom.Batch Batch,reschedulecom.IsPaid IsPaid,0 SalesTotalCommission,0 TechTotalCommission,0 AddCommission,0 FinRepCommission,0 CallCommission,0 FollowUpCommission,reschedulecom.Commission RescheduleCommission,CONVERT(date, t.CompletedDate) CompletionDate,(select ISNULL(SUM(BalanceDue),0) BalanceDue 
                                From Invoice where CustomerId=cus.CustomerId
                                and (Status='Open' or Status='Partial')) as BalanceDue, 'Reschedule Ticket' Type, emp.FirstName +' '+emp.LastName as Technician,reschedulecom.PaidDate PaidDate,0 as OriginalPoint,DATEADD(DAY, (DATEDIFF(DAY, ((@NextDayID + 5) % 7), reschedulecom.PaidDate) / 7) * 7 + 7, ((@NextDayID + 5) % 7)) as PayrollDate  from RescheduleCommission reschedulecom
                                left join ticket t on t.ticketid = reschedulecom.TicketId
                                LEFT JOIN Lookup lpttype on lpttype.DataValue=t.TicketType and lpttype.DataKey='TicketType'
                                left join customer cus on cus.CustomerId = reschedulecom.CustomerId
                                LEFT JOIN CustomerExtended ce on ce.CustomerId=cus.CustomerId
                                left join employee emp on  emp.UserId = reschedulecom.UserId

                                left join   UserLogin ul  on emp.UserId = ul.UserId
								left join UserPermission up on up.UserId = ul.UserId
								left join PermissionGroup pg on pg.Id = up.PermissionGroupId

                                 {18}{22}{32}{20}{16} and t.IsClosed = 1 and ce.IsTestAccount != 1 {25}{26}{27}
                         
     
                                ) d	

                                 select * into #FundedComFilter
								from #FundedCom
                                order by PaidDate desc

								{24}
                                {33}
                                {38}
							    from #FundedComFilter
							
                                {2}
								select count(*) CountTotal
                                from #FundedComFilter
                                
								drop table #FundedCom
								drop table #FundedComFilter";

            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/ticketid")
                {
                    subquery = "order by #e.ticid asc";
                    subquery1 = "order by ticid asc";
                }
                else if (order == "descending/ticketid")
                {
                    subquery = "order by #e.ticid desc";
                    subquery1 = "order by ticid desc";
                }
                else if (order == "ascending/sales")
                {
                    subquery = "order by #e.SalesTotalCommission asc";
                    subquery1 = "order by SalesTotalCommission asc";
                }
                else if (order == "descending/sales")
                {
                    subquery = "order by #e.SalesTotalCommission desc";
                    subquery1 = "order by SalesTotalCommission desc";
                }
                else if (order == "ascending/tech")
                {
                    subquery = "order by #e.TechTotalCommission asc";
                    subquery1 = "order by TechTotalCommission asc";
                }
                else if (order == "descending/tech")
                {
                    subquery = "order by #e.TechTotalCommission desc";
                    subquery1 = "order by TechTotalCommission desc";
                }
                else if (order == "ascending/addedmember")
                {
                    subquery = "order by #e.AddCommission asc";
                    subquery1 = "order by AddCommission asc";
                }
                else if (order == "descending/addedmember")
                {
                    subquery = "order by #e.AddCommission desc";
                    subquery1 = "order by AddCommission desc";
                }
                else if (order == "ascending/callcommission")
                {
                    subquery = "order by #e.CallCommission asc";
                    subquery1 = "order by CallCommission asc";
                }
                else if (order == "descending/callcommission")
                {
                    subquery = "order by #e.CallCommission desc";
                    subquery1 = "order by CallCommission desc";
                }
                else if (order == "ascending/followup")
                {
                    subquery = "order by #e.FollowUpCommission asc";
                    subquery1 = "order by FollowUpCommission asc";
                }
                else if (order == "descending/followup")
                {
                    subquery = "order by #e.FollowUpCommission desc";
                    subquery1 = "order by FollowUpCommission desc";
                }

                else if (order == "ascending/reschedule")
                {
                    subquery = "order by #e.RescheduleCommission asc";
                    subquery1 = "order by RescheduleCommission asc";
                }
                else if (order == "descending/reschedule")
                {
                    subquery = "order by #e.RescheduleCommission desc";
                    subquery1 = "order by RescheduleCommission desc";
                }
                else if (order == "ascending/balancedue")
                {
                    subquery = "order by #e.CompletionDate asc";
                    subquery1 = "order by CompletionDate asc";
                }
                else if (order == "descending/balancedue")
                {
                    subquery = "order by #e.CompletionDate desc";
                    subquery1 = "order by CompletionDate desc";
                }
                else if (order == "descending/type")
                {
                    subquery = "order by #e.Type desc";
                    subquery1 = "order by Type desc";
                }
                else if (order == "ascending/type")
                {
                    subquery = "order by #e.Type asc";
                    subquery1 = "order by Type asc";
                }


                else if (order == "descending/tickettype")
                {
                    subquery = "order by #e.TicketType desc";
                    subquery1 = "order by TicketType desc";
                }
                else if (order == "ascending/tickettype")
                {
                    subquery = "order by #e.TicketType asc";
                    subquery1 = "order by TicketType asc";
                }


                else if (order == "descending/batchno")
                {
                    subquery = "order by #e.Batch desc";
                    subquery1 = "order by Batch desc";
                }
                else if (order == "ascending/batchno")
                {
                    subquery = "order by #e.Batch asc";
                    subquery1 = "order by Batch asc";
                }

                else if (order == "descending/paiddate")
                {
                    subquery = "order by #e.PaidDate desc";
                    subquery1 = "order by PaidDate desc";
                }
                else if (order == "ascending/paiddate")
                {
                    subquery = "order by #e.PaidDate asc";
                    subquery1 = "order by PaidDate asc";
                }

                else if (order == "descending/username")
                {
                    subquery = "order by #e.Technician desc";
                    subquery1 = "order by Technician desc";
                }
                else if (order == "ascending/username")
                {
                    subquery = "order by #e.Technician asc";
                    subquery1 = "order by Technician asc";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(FilterText))
                {
                    subquery = "order by #e.PaidDate desc";
                    subquery1 = "order by PaidDate desc";
                }
                else
                {
                    if (IsPaid)
                    {
                        subquery = "order by #e.PaidDate desc";
                        subquery1 = "order by PaidDate desc";
                    }
                    else
                    {
                        subquery = "order by #e.ticid desc";
                        subquery1 = "order by ticid desc";
                    }

                }

            }
            try
            {
                sqlQuery = string.Format(sqlQuery,
                    order,//0
                    subquery,//1 
                    subquery1,//2
                    IsPaidSalesQuery,//3
                    IsPaidTechQuery,//4
                    IsPaidMemberQuery,//5
                    IsPaidCallQuery,//6
                    DateFilterSale,//7
                    DateFilterTech,//8
                    DateFilterMember,//9
                    DateFilterService,//10 
                    SearchQuerySales,//11 
                    FilterTextQuerySales,//12
                    FilterTextQueryTech,//13
                    FilterTextQueryMember,//14
                    FilterTextQueryCall,//15
                    PaidDateFilter,//16
                     IsPaidFollowUpQuery,//17
                     IsPaidRescheduleQuery,//18
                     FilterTextQueryFollowUp,//19
                     FilterTextQueryReschedule,//20
                     DateFilterFollowUp,//21
                    DateFilterReschedule,//22
                    NameSql,//23
                    QueryString,//24
                    UserGroupFilterQuery,//25
                    DeleteEmployeQuery,//26
                     ticketType,//27
                    SearchQueryTech,//28
                    SearchQueryAddMember,//29
                    SearchQueryServiceCall,//30
                    SearchQueryFollowUp,//31
                    SearchQueryReschedule,//32 
                    BatchQuery,//33
                    IsPaidFinRepQuery,//34
                    DateFilterFinRep,//35
                    FilterTextQueryFinRep,//36
                    SearchQueryFinRep,//37
                    PayrollDateQuery//38
                    );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))


                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }

            }
            catch (Exception ee)
            {
                return null;
            }

        }
        public DataSet GetDownloadAdjustmentReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, bool IsPaid, string FilterText,string SearchText)
        {
            string DateFilterSale = "";

            string IsPaidSalesQuery = "";
            string BatchQuery = "";
            string PayrollDateQuery = "";
            string FilterTextQuerySales = "";
            string SearchTextQuery = "";

            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime() && string.IsNullOrEmpty(FilterText))
            {
                FilterStartDate = FilterStartDate.SetZeroHour();
                FilterEndDate = FilterEndDate.SetMaxHour();
                DateFilterSale = string.Format(" and af.Date between '{0}' and '{1}'"
                    , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));

            }

            if (IsPaid)
            {
                IsPaidSalesQuery = "where af.IsPaid = 1 ";
                BatchQuery = ",af.Batch as [Original Batch]";
                PayrollDateQuery = ",DATEADD(DAY, (DATEDIFF(DAY, ((@NextDayID + 5) % 7), af.PaidDate) / 7) * 7 + 7, ((@NextDayID + 5) % 7)) as PayrollDate";
            }
            else
            {
                IsPaidSalesQuery = " where (af.IsPaid = 0 or af.IsPaid is null)";

            }
            if (!string.IsNullOrEmpty(SearchText))
            {
                int Batch = 0;
                int.TryParse(SearchText, out Batch);
                if (Batch > 0)
                {
                    SearchTextQuery = string.Format(" and af.Batch = '{0}'", Batch);
                }
                else
                {
                    SearchTextQuery = string.Format(" and emp.FirstName+' '+emp.LastName like @SearchText", SearchText);
                }
            }
            if (!string.IsNullOrEmpty(FilterText) && FilterText != "-1")
            {
                FilterTextQuerySales = string.Format(" and af.UserId = '{0}'", FilterText);
            }
            string sqlQuery = @"
                                DECLARE @NextDayID INT;
                                SET @NextDayID = 2; -- Next Monday
                                select * into #AdjustmentFunding from (select af.Id, emp.FirstName+' '+emp.LastName as UserName,ROUND(af.Amount, 2) as  Amount,
                                CASE when LEN(CAST(SUBSTRING(af.Reason, 0, CHARINDEX('Ref', af.Reason)) AS VARCHAR(200))) = 0
								THEN af.Reason
								ELSE CAST(SUBSTRING(af.Reason, 0, CHARINDEX('Ref', af.Reason)) AS VARCHAR(200))
								END as Reason,
                                convert(Date,af.Date) as Date
                                {4}
                                {5}
                                from AdjustmentFunding af
                                left join employee emp on  emp.UserId = af.UserId

                                {0}{2}{3}
                                --{1}
                            
                                ) d	

                                select *
								from #AdjustmentFunding order by Id desc

								
								drop table #AdjustmentFunding";

            try
            {
                sqlQuery = string.Format(sqlQuery, IsPaidSalesQuery, DateFilterSale, FilterTextQuerySales,SearchTextQuery, BatchQuery, PayrollDateQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", SearchText)));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }

            }
            catch (Exception ee)
            {
                return null;
            }

        }

        public bool DeleteAddMemberCommissionByTicketId(Guid ticketId)
        {
            string SqlQuery = @"delete from AddMemberCommission where TicketId ='{0}' and IsManual != 1";
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
