using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Web.Caching;
using NLog.Filters;

namespace HS.DataAccess
{
    public partial class RecurringBillingScheduleDataAccess
    {
        public RecurringBillingScheduleDataAccess() { }
        public RecurringBillingScheduleDataAccess(string ConnectionString) : base(ConnectionString) { }
        public DataTable GetAllScheduleByCustomerIdAndCompanyId(Guid CustomerId, Guid companyId, string SearchText, string Order)
        {
            string searchSql = "";
            string orderquery = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(Order))
            {
                if (Order == "ascending/name")
                {
                    orderquery = "order by TemplateName asc";
                }
                else if (Order == "descending/name")
                {
                    orderquery = "order by TemplateName desc";
                }
                else if (Order == "ascending/status")
                {
                    orderquery = "order by Status asc";
                }
                else if (Order == "descending/status")
                {
                    orderquery = "order by Status desc";
                }
                else if (Order == "ascending/bilcycle")
                {
                    orderquery = "order by BillCycle asc";
                }
                else if (Order == "descending/bilcycle")
                {
                    orderquery = "order by BillCycle desc";
                }
                else if (Order == "ascending/startdate")
                {
                    orderquery = "order by StartDate asc";
                }
                else if (Order == "descending/startdate")
                {
                    orderquery = "order by StartDate desc";
                }
                else if (Order == "ascending/enddate")
                {
                    orderquery = "order by EndDate asc";
                }
                else if (Order == "descending/enddate")
                {
                    orderquery = "order by EndDate desc";
                }
                else if (Order == "ascending/lastpaymentdate")
                {
                    orderquery = "order by PreviousDate asc";
                }
                else if (Order == "descending/lastpaymentdate")
                {
                    orderquery = "order by PreviousDate desc";
                }
                else if (Order == "ascending/nextpaymentdate")
                {
                    orderquery = "order by PaymentCollectionDate asc";
                }
                else if (Order == "descending/nextpaymentdate")
                {
                    orderquery = "order by PaymentCollectionDate desc";
                }
                else if (Order == "ascending/total")
                {
                    orderquery = "order by TotalBillAmount asc";
                }
                else if (Order == "descending/total")
                {
                    orderquery = "order by TotalBillAmount desc";
                }
                else
                {
                    orderquery = "order by Id desc";
                }
            }
            else
            {
                orderquery = "order by Id desc";
            }
            #endregion
            string sqlQuery = @"Declare @CustomerId uniqueidentifier
                                Declare @CompanyId uniqueidentifier
                                set @CustomerId ='{0}'
                                set @CompanyId = '{1}'

                                select
                                RB.Id,
                                RB.ScheduleId,
                                RB.TemplateName,
                                RB.Status,
                                RB.BillCycle,
                                RB.StartDate,
                                RB.EndDate,
                                RB.TotalBillAmount,
                                --case when RB.BillCycle = 'Monthly' then DATEADD(month, -RB.Interval, RB.NextDate)
									--when RB.BillCycle = 'Yearly' then DATEADD(YEAR, -RB.Interval, RB.NextDate)
									--when RB.BillCycle = 'Weekly' then DATEADD(WEEK, -RB.Interval, RB.NextDate)  end PreviousDate,
                                RB.PreviousDate,
                                RB.PaymentCollectionDate
                                from RecurringBillingSchedule RB
                                --left join Employee emp on emp.UserId = _est.CreatedBy
                                where RB.CustomerId = @CustomerId
                                and RB.CompanyId = @CompanyId
                                and RB.Status != 'Init'
                                {2}
                                {3}";

            if (!string.IsNullOrWhiteSpace(SearchText) && SearchText != "undefined" && SearchText != "null")
            {
                searchSql = string.Format(" and RB.TemplateName like '%{0}%'", SearchText);
            }
            //if (filter.StrStartDate.ToString("yyyy-MM-dd HH:mm:ss") != "0001-01-01 00:00:00")
            //{
            //    strStartDate = filter.StrStartDate.SetZeroHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss");
            //    strEndDate = filter.StrEndDate.SetMaxHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss");
            //    dateRange = string.Format("and _est.StartDate between '{0}' and '{1}'", strStartDate, strEndDate);
            //}
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId, companyId, searchSql, orderquery);
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

        public DataTable DownloadAllScheduleByCustomerIdAndCompanyId(Guid CustomerId, Guid companyId, string SearchText)
        {
            //var strStartDate = "";
            //var strEndDate = "";
            var dateRange = "";
            string searchSql = "";
            string subquery = "";
            string sqlQuery = @"Declare @CustomerId uniqueidentifier
                                Declare @CompanyId uniqueidentifier
                                set @CustomerId ='{0}'
                                set @CompanyId = '{1}'

                                select 
                                RB.TemplateName as [Template Name],
                                RB.BillCycle as [Bill Cycle],
                                RB.StartDate as [Start Date],
                                case when RB.BillCycle = 'Monthly' then DATEADD(month, RB.Interval, RB.StartDate)
									when RB.BillCycle = 'Yearly' then DATEADD(YEAR, RB.Interval, RB.StartDate)
									when RB.BillCycle = 'Weekly' then DATEADD(WEEK, RB.Interval, RB.StartDate)  end [Next Date],
                                cast(RB.TotalBillAmount as decimal(10,2)) as [Total]
                                from RecurringBillingSchedule RB
                                --left join Employee emp on emp.UserId = _est.CreatedBy
                                where RB.CustomerId = @CustomerId
                                and RB.CompanyId = @CompanyId
                                and RB.Status != 'Init'
                                {2}
                                order by RB.Id Desc";

            if (!string.IsNullOrWhiteSpace(SearchText) && SearchText != "undefined" && SearchText != "null")
            {
                searchSql = string.Format(" and RB.TemplateName like '%{0}%'", SearchText);
            }
            //if (filter.StrStartDate.ToString("yyyy-MM-dd HH:mm:ss") != "0001-01-01 00:00:00")
            //{
            //    strStartDate = filter.StrStartDate.SetZeroHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss");
            //    strEndDate = filter.StrEndDate.SetMaxHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss");
            //    dateRange = string.Format("and _est.StartDate between '{0}' and '{1}'", strStartDate, strEndDate);
            //}
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId, companyId, searchSql);
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

        public bool CloneRecurringBilling(Guid oldScheduleId, Guid createdbyuid)
        {
            string datetime = DateTime.Now.UTCCurrentTime().ToString("yyyy-MM-dd hh:mm:ss.000");
            string sqlQuery = @"declare @oldScheduleId uniqueidentifier 
                                set @oldScheduleId = '{0}'

                                declare @createdbyuid uniqueidentifier
                                set @createdbyuid = '{1}'

                                declare @datetime datetime
                                set @datetime = '{2}'

                                declare @newScheduleId uniqueidentifier
                                set @newScheduleId = (select NEWID())

                                --RecurringBillingSchedule Clone
                                INSERT INTO RecurringBillingSchedule ([ScheduleId],[CompanyId],[CustomerId],[TemplateName],[EmailAddress],[CCEmail],[BCCEmail],[Status],[AutomaticallySendEmail],[IncludeOpenInvoices],[CollectOnline],[CustomerPaymentProfileId],[BillCycle],[Interval],[StartDate],[EndDate],[BillingAddress],[BillAmount],[TaxPercentage],[TaxAmount],[TotalBillAmount],[MessageOnInvoice],[CreatedBy],[CreatedDate],[LastUpdatedBy],[LastUpdatedDate],[DayInAdvance],[PreviousDate],[NextDate])
                                SELECT @newScheduleId,[CompanyId],[CustomerId],[TemplateName],[EmailAddress],[CCEmail],[BCCEmail],[Status],[AutomaticallySendEmail],[IncludeOpenInvoices],[CollectOnline],[CustomerPaymentProfileId],[BillCycle],[Interval],[StartDate],[EndDate],[BillingAddress],[BillAmount],[TaxPercentage],[TaxAmount],[TotalBillAmount],[MessageOnInvoice],@createdbyuid,@datetime,[LastUpdatedBy],[LastUpdatedDate],[DayInAdvance],[PreviousDate],[NextDate] FROM RecurringBillingSchedule where ScheduleId = @oldScheduleId
                                
                                --RecurringBillingScheduleItems Clone
                                INSERT INTO RecurringBillingScheduleItems ([ScheduleId],[ProductName],[Description],[Qty],[Rate],[Amount],[IsTaxable],[AddedBy],[AddedDate])
                                SELECT @newScheduleId,[ProductName],[Description],[Qty],[Rate],[Amount],[IsTaxable],[AddedBy],[AddedDate] FROM RecurringBillingScheduleItems where ScheduleId = @oldScheduleId
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, oldScheduleId, createdbyuid, datetime);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool DeleteRecurringBillingScheduleByScheduleId(Guid ScheduleId)
        {
            string SqlQuery = @"delete from RecurringBillingSchedule where ScheduleId ='{0}' ";
            SqlQuery = string.Format(SqlQuery, ScheduleId);
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

        public DataSet GetSumOfRecurringBillingByCustomerId(Guid CustomerId)
        {
            string sqlQuery = @"select 
                                sum(BillAmount) as MonitoringFee
                                ,sum(TotalBillAmount) as BilligAmount
                                from RecurringBillingSchedule
                                where [status] ='Active'
                                and CustomerId ='{0}'";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        CustomerId
                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetReurringBillingScheduleList(string SearchText, string Start, string END, int? BillDay, String Interval,String BillingMethod,String BillingStatus, int PageNo, int PageSize, string Order)
        {
            string SearchTextQ = "";
            string BillDayQ = "";
            string IntervalQ = "";
            string BillingMethodQ = "";
            String BillingStatusQ = "";
            string orderquery = "";
            string orderquery1 = "";
            string filterbydaterange = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(Order) && Order != "null")
            {
                if (Order == "ascending/customer")
                {
                    orderquery = "order by tl.[CustomerName] asc";
                    orderquery1 = "order by [CustomerName] asc";
                }
                else if (Order == "descending/customer")
                {
                    orderquery = "order by tl.[CustomerName] desc";
                    orderquery1 = "order by [CustomerName] desc";
                }
                else if (Order == "ascending/template")
                {
                    orderquery = "order by tl.TemplateName asc";
                    orderquery1 = "order by TemplateName asc";
                }
                else if (Order == "descending/template")
                {
                    orderquery = "order by tl.TemplateName desc";
                    orderquery1 = "order by TemplateName desc";
                }
                else if (Order == "ascending/startdate")
                {
                    orderquery = "order by tl.StartDate asc";
                    orderquery1 = "order by StartDate asc";
                }
                else if (Order == "descending/startdate")
                {
                    orderquery = "order by tl.StartDate desc";
                    orderquery1 = "order by StartDate desc";
                }
                else if (Order == "ascending/billday")
                {
                    orderquery = "order by tl.BillDate asc";
                    orderquery1 = "order by BillDate asc";
                }
                else if (Order == "descending/billday")
                {
                    orderquery = "order by tl.BillDate desc";
                    orderquery1 = "order by BillDate desc";
                }
                else if (Order == "ascending/totalamount")
                {
                    orderquery = "order by tl.TotalBillAmount asc";
                    orderquery1 = "order by TotalBillAmount asc";
                }
                else if (Order == "descending/totalamount")
                {
                    orderquery = "order by tl.TotalBillAmount desc";
                    orderquery1 = "order by TotalBillAmount desc";
                }
                else if (Order == "ascending/interval")
                {
                    orderquery = "order by tl.Interval asc";
                    orderquery1 = "order by Interval asc";
                }
                else if (Order == "descending/interval")
                {
                    orderquery = "order by tl.Interval desc";
                    orderquery1 = "order by Interval desc";
                }
                else if (Order == "ascending/status")
                {
                    orderquery = "order by tl.Status asc";
                    orderquery1 = "order by Status asc";
                }
                else if (Order == "descending/status")
                {
                    orderquery = "order by tl.Status desc";
                    orderquery1 = "order by Status desc";
                }
                else if (Order == "ascending/lastinvoice")
                {
                    orderquery = "order by tl.LastInvoice asc";
                    orderquery1 = "order by LastInvoice asc";
                }
                else if (Order == "descending/lastinvoice")
                {
                    orderquery = "order by tl.LastInvoice desc";
                    orderquery1 = "order by LastInvoice desc";
                }
                else
                {
                    orderquery = "order by tl.[Id] desc";
                    orderquery1 = "order by Id desc";
                }
            }
            else
            {
                orderquery = "order by tl.[Id] desc";
                orderquery1 = "order by Id desc";
            }
            #endregion
            if (!string.IsNullOrEmpty(Start) && !string.IsNullOrEmpty(END))
            {
                DateTime StartDate = DateTime.Parse(Start).SetZeroHour().ClientToUTCTime();
                DateTime EndDate = DateTime.Parse(END).SetMaxHour().ClientToUTCTime();

                filterbydaterange = string.Format(@"
                AND (
                (r.StartDate BETWEEN '{0}' AND '{1}') 
                OR (r.StartDate BETWEEN '{0}' AND '{1}')
                )", StartDate, EndDate);
            }

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                SearchTextQ = string.Format("and (c.CustomerName like '%{0}%' OR r.TemplateName like '%{0}%')", SearchText);
            }
            if (BillDay > 0) 
            {
                BillDayQ = string.Format("AND DAY(r.BillDate) = {0}", BillDay);
            }
            if (!string.IsNullOrWhiteSpace(Interval) && Interval != "-1")
            {
                IntervalQ = string.Format("AND r.Interval = '{0}'", Interval);
            }
            if (!string.IsNullOrWhiteSpace(BillingMethod) && BillingMethod != "-1")
            {
                BillingMethodQ = string.Format("AND r.PaymentMethod LIKE '%{0}%'", BillingMethod);
            }
            if (string.IsNullOrWhiteSpace(BillingStatus)) 
            {
                BillingStatusQ = "'Active'"; 
            }
            else
            {
                BillingStatusQ = string.Format("'{0}'", BillingStatus);
            }
            string sqlQuery = @" DECLARE @pagestart int
	                                DECLARE @pageend int
	                                DECLARE @pageno int
	                                DECLARE @pagesize int
                                    SET @pageno = {0} --default 1
									SET @pagesize = {1} --default 10
                                    SET @pagestart=(@pageno-1)* @pagesize 
                                    SET @pageend = @pagesize   


                                select rbs.CustomerId, rbs.Id, rbs.ScheduleId, rbs.TemplateName, rbs.StartDate, rbs.TotalBillAmount, rbs.BillCycle as Interval, rbs.Status, rbs.PaymentMethod, rbs.LastRMRInvoiceRefId as LastInvoice, rbs.PaymentCollectionDate as BillDate
                                into #tempRecurring from RecurringBillingSchedule rbs

                                select cus.Id as CusId, cus.CustomerId, case when cus.BusinessName != '' then cus.BusinessName when cus.DBA != '' then cus.DBA else cus.FirstName+' '+ cus.LastName end as CustomerName
                                into #tempCustomer from Customer cus where cus.CustomerId in (select CustomerId from #tempRecurring)

                                select inv.Id as InvId, inv.InvoiceId, inv.InvoiceDate into #tempInvoice from Invoice inv where inv.InvoiceId in (select LastInvoice from #tempRecurring) and inv.IsARBInvoice =1

                                select  r.Id, r.ScheduleId, r.TemplateName, r.StartDate, r.TotalBillAmount, 
                                r.Interval, r.Status, r.PaymentMethod, r.LastInvoice, r.BillDate,
                                c.CusId, c.CustomerId, c.CustomerName,i.InvId, i.InvoiceId, i.InvoiceDate into #tempList from #tempRecurring r 
                                join #tempCustomer c on r.CustomerId = c.CustomerId 
                                left join #tempInvoice i on i.InvoiceId = r.LastInvoice
	                            where r.Status={6}
                                {2}
                                {3}
                                {4}
                                {5}
                                {9}
                                select inv.Id as Inv2Id, inv.Status, inv.BookingId into #tempInvoice2 from Invoice inv where inv.BookingId in (select cast(Id as nvarchar(20)) from #tempRecurring) and inv.IsARBInvoice =1 and inv.Status not in ('Paid','Cancel', 'Cancelled', 'Rolled Over', 'Init')

                                select top(@pagesize)*, 
                                ISNULL((select count(inv2.Inv2Id) from #tempInvoice2 inv2 where inv2.BookingId = cast(tl.Id as nvarchar(20))), 0) as UnpaidCount
                                from #tempList tl WHERE tl.Id NOT IN (
                                SELECT TOP (@pagestart) tl_inner.Id
                                FROM #tempList tl_inner {7}){8};
                                select count(tl.Id) as TemplateCount, cast(sum(isnull(tl.TotalBillAmount,0)) as decimal(18,2)) as TotalAmount
                                from #tempList tl

                                drop table #tempRecurring
                                drop table #tempCustomer
                                drop table #tempInvoice
                                drop table #tempInvoice2
                                drop table #tempList";

            try
            {
                sqlQuery = string.Format(sqlQuery, PageNo,
                                        PageSize, SearchTextQ, BillDayQ, IntervalQ, BillingMethodQ, BillingStatusQ, orderquery,
                                        orderquery1, filterbydaterange);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetReurringBillingScheduleListExportReport(string SearchText, string Start, string END, int? BillDay, String Interval, String BillingMethod, String BillingStatus, string Order)
        {
            string SearchTextQ = "";
            string BillDayQ = "";
            string IntervalQ = "";
            string BillingMethodQ = "";
            String BillingStatusQ = "";
            string orderquery = "";
            string orderquery1 = "";
            string filterbydaterange = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(Order) && Order != "null")
            {
                if (Order == "ascending/customer")
                {
                    orderquery = "order by tl.[CustomerName] asc";
                    orderquery1 = "order by [CustomerName] asc";
                }
                else if (Order == "descending/customer")
                {
                    orderquery = "order by tl.[CustomerName] desc";
                    orderquery1 = "order by [CustomerName] desc";
                }
                else if (Order == "ascending/template")
                {
                    orderquery = "order by tl.TemplateName asc";
                    orderquery1 = "order by TemplateName asc";
                }
                else if (Order == "descending/template")
                {
                    orderquery = "order by tl.TemplateName desc";
                    orderquery1 = "order by TemplateName desc";
                }
                else if (Order == "ascending/startdate")
                {
                    orderquery = "order by tl.StartDate asc";
                    orderquery1 = "order by StartDate asc";
                }
                else if (Order == "descending/startdate")
                {
                    orderquery = "order by tl.StartDate desc";
                    orderquery1 = "order by StartDate desc";
                }
                else if (Order == "ascending/billday")
                {
                    orderquery = "order by tl.BillDate asc";
                    orderquery1 = "order by BillDate asc";
                }
                else if (Order == "descending/billday")
                {
                    orderquery = "order by tl.BillDate desc";
                    orderquery1 = "order by BillDate desc";
                }
                else if (Order == "ascending/totalamount")
                {
                    orderquery = "order by tl.TotalBillAmount asc";
                    orderquery1 = "order by TotalBillAmount asc";
                }
                else if (Order == "descending/totalamount")
                {
                    orderquery = "order by tl.TotalBillAmount desc";
                    orderquery1 = "order by TotalBillAmount desc";
                }
                else if (Order == "ascending/interval")
                {
                    orderquery = "order by tl.Interval asc";
                    orderquery1 = "order by Interval asc";
                }
                else if (Order == "descending/interval")
                {
                    orderquery = "order by tl.Interval desc";
                    orderquery1 = "order by Interval desc";
                }
                else if (Order == "ascending/status")
                {
                    orderquery = "order by tl.Status asc";
                    orderquery1 = "order by Status asc";
                }
                else if (Order == "descending/status")
                {
                    orderquery = "order by tl.Status desc";
                    orderquery1 = "order by Status desc";
                }
                else if (Order == "ascending/lastinvoice")
                {
                    orderquery = "order by tl.LastInvoice asc";
                    orderquery1 = "order by LastInvoice asc";
                }
                else if (Order == "descending/lastinvoice")
                {
                    orderquery = "order by tl.LastInvoice desc";
                    orderquery1 = "order by LastInvoice desc";
                }
                else
                {
                    orderquery = "order by tl.[Id] desc";
                    orderquery1 = "order by Id desc";
                }
            }
            else
            {
                orderquery = "order by tl.[Id] desc";
                orderquery1 = "order by Id desc";
            }
            #endregion
            if (!string.IsNullOrEmpty(Start) && !string.IsNullOrEmpty(END))
            {
                DateTime StartDate = DateTime.Parse(Start).SetZeroHour().ClientToUTCTime();
                DateTime EndDate = DateTime.Parse(END).SetMaxHour().ClientToUTCTime();

                filterbydaterange = string.Format(@"
                AND (
                (r.StartDate BETWEEN '{0}' AND '{1}') 
                OR (r.StartDate BETWEEN '{0}' AND '{1}')
                )", StartDate, EndDate);
            }
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                SearchTextQ = string.Format("and (c.CustomerName like '%{0}%' OR r.TemplateName like '%{0}%')", SearchText);
            }
            if (BillDay > 0)
            {
                BillDayQ = string.Format("AND r.BillDate = {0}", BillDay);
            }
            if (!string.IsNullOrWhiteSpace(Interval) && Interval != "-1")
            {
                IntervalQ = string.Format("AND r.Interval = '{0}'", Interval);
            }
            if (!string.IsNullOrWhiteSpace(BillingMethod) && BillingMethod != "-1")
            {
                BillingMethodQ = string.Format("AND r.PaymentMethod LIKE '%{0}%'", BillingMethod);
            }
            if (string.IsNullOrWhiteSpace(BillingStatus))
            {
                BillingStatusQ = "'Active'";
            }
            else
            {
                BillingStatusQ = string.Format("'{0}'", BillingStatus);
            }
            string sqlQuery = @"select rbs.CustomerId, rbs.Id, rbs.ScheduleId, rbs.TemplateName, rbs.StartDate, rbs.TotalBillAmount, rbs.BillCycle as Interval,
                                   rbs.Status, rbs.PaymentMethod, rbs.LastRMRInvoiceRefId as LastInvoice,
                                   CASE 
                                    WHEN rbs.PaymentCollectionDate IS NULL THEN '' 
                                    ELSE CAST(DAY(rbs.PaymentCollectionDate) AS VARCHAR(2)) 
                                END AS BillDate
                                into #tempRecurring from RecurringBillingSchedule rbs

                                select cus.Id as CusId, cus.CustomerId, case when cus.BusinessName != '' then cus.BusinessName when cus.DBA != '' then cus.DBA else cus.FirstName+' '+ cus.LastName end as CustomerName
                                into #tempCustomer from Customer cus where cus.CustomerId in (select CustomerId from #tempRecurring)

                                select inv.Id as InvId, inv.InvoiceId, inv.InvoiceDate into #tempInvoice from Invoice inv where inv.InvoiceId in (select LastInvoice from #tempRecurring) and inv.IsARBInvoice =1

                                select  r.Id, r.ScheduleId, r.TemplateName, r.StartDate, r.TotalBillAmount, 
                                r.Interval, r.Status, r.PaymentMethod, r.LastInvoice, r.BillDate,
                                c.CusId, c.CustomerId, c.CustomerName,i.InvId, i.InvoiceId, i.InvoiceDate into #tempList from #tempRecurring r 
                                join #tempCustomer c on r.CustomerId = c.CustomerId 
                                left join #tempInvoice i on i.InvoiceId = r.LastInvoice
	                           where r.Status={4}
                                {0}
                                {1}
                                {2}
                                {3}
                                 {6}
                                Select Id, TemplateName,StartDate,TotalBillAmount,Interval,Status,	PaymentMethod,LastInvoice, BillDate,CustomerName,ISNULL(InvoiceId,'') As InvoiceId,InvoiceDate into #rmrtemplist from #tempList

                                select inv.Id as Inv2Id, inv.Status, inv.BookingId into #tempInvoice2 from Invoice inv where inv.BookingId in (select cast(Id as nvarchar(20)) from #tempRecurring) and inv.IsARBInvoice =1 and inv.Status not in ('Paid','Cancel', 'Cancelled', 'Rolled Over', 'Init')

                                select Id, CustomerName As [Customer],TemplateName As [Template],	CONVERT(VARCHAR(10), StartDate, 120) As [Start Date],BillDate As [Bill Day],cast(TotalBillAmount AS DECIMAL(18,2)) As [Total Amount],
								[Interval],	[Status], LastInvoice [Last Invoice],CONVERT(VARCHAR(10), invoiceDate, 120) As [Invoice Date],InvoiceId [InvoiceId], 
                                ISNULL((select count(inv2.Inv2Id) from #tempInvoice2 inv2 where inv2.BookingId = cast(tl.Id as nvarchar(20))), 0) as UnpaidCount
                                ,identity(int,1,1) as rmrmid
								into #dwnreccurring from #rmrtemplist tl  {5}
                                
                                declare @TemplateCount int
                                declare @TotalAmount float 

								set @TemplateCount = (select count(Id) as TemplateCount from #dwnreccurring)
								set @TotalAmount = (select (sum(isnull([Total Amount],0))) as TotalAmount from #dwnreccurring)							   
								
								 
								insert into #dwnreccurring (Id,	[Customer],	[Template],	[Start Date],	[Bill Day],	[Total Amount],	[Interval],	[Status],	[Last Invoice],	[Invoice Date],	[InvoiceId],[UnpaidCount]) 
								values(0,'Total','','',Cast(@TemplateCount As int),@TotalAmount,'','','','','','')

								
								 Select * from #dwnreccurring order by rmrmid asc


                                drop table #tempRecurring
                                drop table #tempCustomer
                                drop table #tempInvoice
                                drop table #tempInvoice2
                                drop table #tempList
								drop table #rmrtemplist
                                drop table #dwnreccurring ";
            try
            {
                sqlQuery = string.Format(sqlQuery, SearchTextQ, BillDayQ, IntervalQ, BillingMethodQ, BillingStatusQ, orderquery, filterbydaterange);
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

        public DataTable GetRMRInvoiceListByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId, string SearchText, string Order, DateTime? Start, DateTime? End)
        {
            string searchSql = "";
            string orderquery = "";
            string datequery = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(Order))
            {
                if (Order == "ascending/invoiceid")
                {
                    orderquery = "order by InvoiceId asc";
                }
                else if (Order == "descending/invoiceid")
                {
                    orderquery = "order by InvoiceId desc";
                }
                else if (Order == "ascending/date")
                {
                    orderquery = "order by InvoiceDate asc";
                }
                else if (Order == "descending/date")
                {
                    orderquery = "order by InvoiceDate desc";
                }
                else if (Order == "ascending/amountdue")
                {
                    orderquery = "order by TotalAmount asc";
                }
                else if (Order == "descending/amountdue")
                {
                    orderquery = "order by TotalAmount desc";
                }
                else if (Order == "ascending/netdue")
                {
                    orderquery = "order by BalanceDue asc";
                }
                else if (Order == "descending/netdue")
                {
                    orderquery = "order by BalanceDue desc";
                }
                else
                {
                    orderquery = "order by Id desc";
                }
            }
            else
            {
                orderquery = "order by Id desc";
            }
            #endregion
            string sqlQuery = @"select
                                Inv.Id
                                ,Inv.InvoiceId
                                ,Inv.InvoiceDate
                                ,Inv.TotalAmount
                                ,Inv.BalanceDue
                                ,Inv.[Status]
                                 from Invoice Inv
                                 where Inv.CustomerId = '{0}'
                                 and Inv.CompanyId ='{1}'
                                 and Inv.IsARBInvoice = 1
                                 and Inv.IsEstimate = 0
                                 and Inv.[Status] not in('Cancelled', 'Rolled Over', 'Init')
                                 {2}
                                 {3}
                                 {4}";

            if (!string.IsNullOrWhiteSpace(SearchText) && SearchText != "undefined" && SearchText != "null")
            {
                searchSql = string.Format(" and Inv.InvoiceId like '%{0}%'", SearchText);
            }
            if (Start != new DateTime() && End != new DateTime())
            {
                string StartDateQuery = Start.Value.SetZeroHour().ToString();
                string EndDateQuery = End.Value.SetMaxHour().ToString();

                datequery = string.Format("and Inv.InvoiceDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId, CompanyId, searchSql, datequery, orderquery);
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

        public DataTable GetRMRInvoiceListForDownload(Guid CustomerId, Guid CompanyId, string SearchText, string Order, DateTime? Start, DateTime? End)
        {
            string searchSql = "";
            string orderquery = "";
            string datequery = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(Order))
            {
                if (Order == "ascending/invoiceid")
                {
                    orderquery = "order by InvoiceId asc";
                }
                else if (Order == "descending/invoiceid")
                {
                    orderquery = "order by InvoiceId desc";
                }
                else if (Order == "ascending/date")
                {
                    orderquery = "order by InvoiceDate asc";
                }
                else if (Order == "descending/date")
                {
                    orderquery = "order by InvoiceDate desc";
                }
                else if (Order == "ascending/amountdue")
                {
                    orderquery = "order by TotalAmount asc";
                }
                else if (Order == "descending/amountdue")
                {
                    orderquery = "order by TotalAmount desc";
                }
                else if (Order == "ascending/netdue")
                {
                    orderquery = "order by BalanceDue asc";
                }
                else if (Order == "descending/netdue")
                {
                    orderquery = "order by BalanceDue desc";
                }
                else
                {
                    orderquery = "order by [Invoice Id] desc";
                }
            }
            else
            {
                orderquery = "order by [Invoice Id] desc";
            }
            #endregion
            string sqlQuery = @"select
                                Inv.InvoiceId as [Invoice Id]
                                ,FORMAT(Inv.InvoiceDate,'MM/dd/yyyy') as [Date]
                                ,Inv.TotalAmount as[Amount Due]
                                ,Inv.BalanceDue as [Net Due]
                                 from Invoice Inv
                                 where Inv.CustomerId = '{0}'
                                 and Inv.CompanyId ='{1}'
                                 and Inv.IsARBInvoice = 1
                                 and Inv.IsEstimate = 0
                                 and Inv.[Status] not in('Cancelled', 'Rolled Over', 'Init')
                                 {2}
                                 {3}
                                 {4}";

            if (!string.IsNullOrWhiteSpace(SearchText) && SearchText != "undefined" && SearchText != "null")
            {
                searchSql = string.Format(" and Inv.InvoiceId like '%{0}%'", SearchText);
            }
            if (Start != new DateTime() && End != new DateTime())
            {
                string StartDateQuery = Start.Value.SetZeroHour().ToString();
                string EndDateQuery = End.Value.SetMaxHour().ToString();

                datequery = string.Format("and Inv.InvoiceDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId, CompanyId, searchSql, datequery, orderquery);
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

        public DataTable GetRMRHistoryListByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId, string SearchText, string Order, DateTime? Start, DateTime? End)
        {
            string searchSql = "";
            string orderquery = "";
            string datequery = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(Order))
            {
                if (Order == "ascending/invoiceid")
                {
                    orderquery = "order by InvoiceId asc";
                }
                else if (Order == "descending/invoiceid")
                {
                    orderquery = "order by InvoiceId desc";
                }
                else if (Order == "ascending/date")
                {
                    orderquery = "order by InvoiceDate asc";
                }
                else if (Order == "descending/date")
                {
                    orderquery = "order by InvoiceDate desc";
                }
                else if (Order == "ascending/method")
                {
                    orderquery = "order by PaymentMethod asc";
                }
                else if (Order == "descending/method")
                {
                    orderquery = "order by PaymentMethod desc";
                }
                else if (Order == "ascending/checkno")
                {
                    orderquery = "order by CheckNo asc";
                }
                else if (Order == "descending/checkno")
                {
                    orderquery = "order by CheckNo desc";
                }
                else if (Order == "ascending/paymentdate")
                {
                    orderquery = "order by PaymentDate asc";
                }
                else if (Order == "descending/paymentdate")
                {
                    orderquery = "order by PaymentDate desc";
                }
                else if (Order == "ascending/amount")
                {
                    orderquery = "order by Amount asc";
                }
                else if (Order == "descending/amount")
                {
                    orderquery = "order by Amount desc";
                }
                else if (Order == "ascending/batchcode")
                {
                    orderquery = "order by BatchNumber asc";
                }
                else if (Order == "descending/batchcode")
                {
                    orderquery = "order by BatchNumber desc";
                }
                else if (Order == "ascending/funded")
                {
                    orderquery = "order by Funded asc";
                }
                else if (Order == "descending/funded")
                {
                    orderquery = "order by Funded desc";
                }
                else if (Order == "ascending/posted")
                {
                    orderquery = "order by Posted asc";
                }
                else if (Order == "descending/posted")
                {
                    orderquery = "order by Posted desc";
                }
                else
                {
                    orderquery = "order by PaymentDate desc";
                }
            }
            else
            {
                orderquery = "order by PaymentDate desc";
            }
            #endregion
            string sqlQuery = @"select 
                                Inv.Id
                                ,Inv.InvoiceId
                                ,Inv.InvoiceDate
                                ,TH.Amout
								,T.PaymentMethod
								,T.CheckNo
								,T.TransacationDate as PaymentDate
                                --,(select top (1) Tr.PaymentMethod from [Transaction] Tr where Tr.ReferenceNo = Inv.InvoiceId order by Id desc) as PaymentMethod
                                --,(select top (1) Tr.CheckNo from [Transaction] Tr where Tr.ReferenceNo = Inv.InvoiceId order by Id desc) as CheckNo
                                --,(select top (1) Tr.TransacationDate from [Transaction] Tr where Tr.ReferenceNo = Inv.InvoiceId order by Id desc) as PaymentDate
                                --,(select top (1) Tr.Amount from [Transaction] Tr where Tr.ReferenceNo = Inv.InvoiceId order by Id desc) as Amount
                                ,Inv.BatchNumber
                                ,CASE WHEN ((Cus.AlarmRefId is not null AND Cus.AlarmRefId !='') OR (Cus.BrinksRefId is not null AND Cus.BrinksRefId !='') OR (Cus.UCCRefId is not null AND Cus.UCCRefId !=''))  THEN 'Yes' ELSE 'No' end as Funded
                                ,'Posted' as Posted
                                 from Invoice Inv 
                                 left join Customer Cus on Cus.CustomerId = Inv.CustomerId
                                 left join TransactionHistory TH on TH.InvoiceId = Inv.Id
								 left join [Transaction] T on T.Id = TH.TransactionId
                                 where Inv.CustomerId = '{0}'
                                 and Inv.CompanyId ='{1}'
                                 and Inv.IsARBInvoice = 1
                                 and Inv.IsEstimate = 0
                                 and Inv.[Status] != 'Init'
                                 {2}
                                 {3}
                                 {4}";

            if (!string.IsNullOrWhiteSpace(SearchText) && SearchText != "undefined" && SearchText != "null")
            {
                searchSql = string.Format(" and Inv.InvoiceId like '%{0}%'", SearchText);
            }
            if (Start != new DateTime() && End != new DateTime())
            {
                string StartDateQuery = Start.Value.SetZeroHour().ToString();
                string EndDateQuery = End.Value.SetMaxHour().ToString();

                datequery = string.Format("and Inv.InvoiceDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId, CompanyId, searchSql, datequery, orderquery);
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

        public DataTable GetRMRHistoryListForDownload(Guid CustomerId, Guid CompanyId, string SearchText, string Order, DateTime? Start, DateTime? End)
        {
            string searchSql = "";
            string orderquery = "";
            string datequery = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(Order))
            {
                if (Order == "ascending/invoiceid")
                {
                    orderquery = "order by InvoiceId asc";
                }
                else if (Order == "descending/invoiceid")
                {
                    orderquery = "order by InvoiceId desc";
                }
                else if (Order == "ascending/date")
                {
                    orderquery = "order by InvoiceDate asc";
                }
                else if (Order == "descending/date")
                {
                    orderquery = "order by InvoiceDate desc";
                }
                else if (Order == "ascending/method")
                {
                    orderquery = "order by PaymentMethod asc";
                }
                else if (Order == "descending/method")
                {
                    orderquery = "order by PaymentMethod desc";
                }
                else if (Order == "ascending/checkno")
                {
                    orderquery = "order by CheckNo asc";
                }
                else if (Order == "descending/checkno")
                {
                    orderquery = "order by CheckNo desc";
                }
                else if (Order == "ascending/paymentdate")
                {
                    orderquery = "order by PaymentDate asc";
                }
                else if (Order == "descending/paymentdate")
                {
                    orderquery = "order by PaymentDate desc";
                }
                else if (Order == "ascending/amount")
                {
                    orderquery = "order by Amount asc";
                }
                else if (Order == "descending/amount")
                {
                    orderquery = "order by Amount desc";
                }
                else if (Order == "ascending/batchcode")
                {
                    orderquery = "order by BatchNumber asc";
                }
                else if (Order == "descending/batchcode")
                {
                    orderquery = "order by BatchNumber desc";
                }
                else if (Order == "ascending/funded")
                {
                    orderquery = "order by Funded asc";
                }
                else if (Order == "descending/funded")
                {
                    orderquery = "order by Funded desc";
                }
                else if (Order == "ascending/posted")
                {
                    orderquery = "order by Posted asc";
                }
                else if (Order == "descending/posted")
                {
                    orderquery = "order by Posted desc";
                }
                else
                {
                    orderquery = "order by [Invoice Id] desc";
                }
            }
            else
            {
                orderquery = "order by [Invoice Id] desc";
            }
            #endregion
            string sqlQuery = @"select 
                                Inv.InvoiceId as [Invoice Id]
                                ,FORMAT(Inv.InvoiceDate,'MM/dd/yyyy') as [Date]
                                ,(select top (1) Tr.PaymentMethod from [Transaction] Tr where Tr.ReferenceNo = Inv.InvoiceId order by Id desc) as Method
                                ,(select top (1) Tr.CheckNo from [Transaction] Tr where Tr.ReferenceNo = Inv.InvoiceId order by Id desc) as [Chec kNo]
                                ,(select top (1) FORMAT(Tr.TransacationDate,'MM/dd/yyyy') from [Transaction] Tr where Tr.ReferenceNo = Inv.InvoiceId order by Id desc) as [Payment Date]
                                ,(select top (1) Tr.Amount from [Transaction] Tr where Tr.ReferenceNo = Inv.InvoiceId order by Id desc) as Amount
                                ,Inv.BatchNumber as [Batch Code]
                                ,CASE WHEN ((Cus.AlarmRefId is not null AND Cus.AlarmRefId !='') OR (Cus.BrinksRefId is not null AND Cus.BrinksRefId !='') OR (Cus.UCCRefId is not null AND Cus.UCCRefId !=''))  THEN 'Yes' ELSE 'No' end as Funded
                                ,'Posted' as Posted
                                 from Invoice Inv 
                                 left join Customer Cus on Cus.CustomerId = Inv.CustomerId
                                 where Inv.CustomerId = '{0}'
                                 and Inv.CompanyId ='{1}'
                                 and Inv.IsARBInvoice = 1
                                 and Inv.IsEstimate = 0
                                 and Inv.[Status] != 'Init'
                                 {2}
                                 {3}
                                 {4}";

            if (!string.IsNullOrWhiteSpace(SearchText) && SearchText != "undefined" && SearchText != "null")
            {
                searchSql = string.Format(" and Inv.InvoiceId like '%{0}%'", SearchText);
            }
            if (Start != new DateTime() && End != new DateTime())
            {
                string StartDateQuery = Start.Value.SetZeroHour().ToString();
                string EndDateQuery = End.Value.SetMaxHour().ToString();

                datequery = string.Format("and Inv.InvoiceDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId, CompanyId, searchSql, datequery, orderquery);
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

        public DataSet GetRMRInvoiceListByCompanyId(RMRFilter Filter)
        {
            string subquery = "";
            string orderquery = "";
            string orderquery1 = "";
            if (Filter.StartDate.HasValue && Filter.StartDate.Value != new DateTime() && Filter.EndDate.HasValue && Filter.EndDate.Value != new DateTime())
            {
                Filter.StartDate = Filter.StartDate.Value.SetZeroHour();
                Filter.EndDate = Filter.EndDate.Value.SetMaxHour();
                subquery = string.Format("and Inv.InvoiceDate between '{0}' and '{1}'", Filter.StartDate, Filter.EndDate);
            }
            string searchquery = "";
            if (!string.IsNullOrWhiteSpace(Filter.SearchText))
            {
                searchquery += string.Format("and (Cus.FirstName like '%{0}%' or Cus.LastName like '%{0}%' or Cus.FirstName + ' ' + Cus.LastName like '%{0}%' or Cus.BusinessName like '%{0}%' or Inv.InvoiceId like '%{0}%')", Filter.SearchText);
            }
            #region Order
            if (!string.IsNullOrWhiteSpace(Filter.Order))
            {
                if (Filter.Order == "ascending/customer")
                {
                    orderquery = "order by #TempInv.[CustomerName] asc";
                    orderquery1 = "order by [CustomerName] asc";
                }
                else if (Filter.Order == "descending/customer")
                {
                    orderquery = "order by #TempInv.[CustomerName] desc";
                    orderquery1 = "order by [CustomerName] desc";
                }
                else if (Filter.Order == "ascending/invoiceid")
                {
                    orderquery = "order by #TempInv.InvoiceId asc";
                    orderquery1 = "order by InvoiceId asc";
                }
                else if (Filter.Order == "descending/invoiceid")
                {
                    orderquery = "order by #TempInv.InvoiceId desc";
                    orderquery1 = "order by InvoiceId desc";
                }
                else if (Filter.Order == "ascending/date")
                {
                    orderquery = "order by #TempInv.InvoiceDate asc";
                    orderquery1 = "order by InvoiceDate asc";
                }
                else if (Filter.Order == "descending/date")
                {
                    orderquery = "order by #TempInv.InvoiceDate desc";
                    orderquery1 = "order by InvoiceDate desc";
                }
                else if (Filter.Order == "ascending/amountdue")
                {
                    orderquery = "order by #TempInv.TotalAmount asc";
                    orderquery1 = "order by TotalAmount asc";
                }
                else if (Filter.Order == "descending/amountdue")
                {
                    orderquery = "order by #TempInv.TotalAmount desc";
                    orderquery1 = "order by TotalAmount desc";
                }
                else if (Filter.Order == "ascending/netdue")
                {
                    orderquery = "order by #TempInv.BalanceDue asc";
                    orderquery1 = "order by BalanceDue asc";
                }
                else if (Filter.Order == "descending/netdue")
                {
                    orderquery = "order by #TempInv.BalanceDue desc";
                    orderquery1 = "order by BalanceDue desc";
                }
                else
                {
                    orderquery = "order by #TempInv.Id desc";
                    orderquery1 = "order by Id desc";
                }
            }
            else
            {
                orderquery = "order by #TempInv.Id desc";
                orderquery1 = "order by Id desc";
            }
            #endregion
            string sqlQuery = @"DECLARE @pagestart int
	                            DECLARE @pageend int
	                            DECLARE @pageno int
	                            DECLARE @pagesize int

                                SET @pageno = {0}
								SET @pagesize = {1}
                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

                                
                                select
                                Inv.Id
                                ,Inv.InvoiceId
                                ,CASE 
								WHEN (Cus.BusinessName = '' or Cus.BusinessName IS NULL) THEN Cus.FirstName +' '+Cus.LastName
								ELSE  Cus.BusinessName
								END as CustomerName
								,Cus.Id as CustomerIntId
                                ,Inv.InvoiceDate
                                ,Inv.TotalAmount
                                ,Inv.BalanceDue
                                ,Inv.[Status]
		                         into #TempInvoice

								 from Invoice Inv
                                 left join Customer Cus on Cus.CustomerId = Inv.CustomerId
                                 where Inv.CompanyId ='{2}'
                                 and Inv.IsARBInvoice = 1
                                 and Inv.IsEstimate = 0
                                 and Inv.[Status] not in('Cancelled', 'Rolled Over', 'Init')
                                 {3}
                                 {4}

														SELECT TOP (@pagesize) #TempInv.* into #TestTable
														FROM #TempInvoice #TempInv
														where Id NOT IN(Select TOP (@pagestart) Id from #TempInvoice #TempInv {5})
														{6}
														select  count(Id) as [TotalCount] from #TempInvoice

														select * from #TestTable
														select sum(TotalAmount) as TotalAmount
														,sum(BalanceDue) as TotalBalanceDue from #TestTable

														DROP TABLE #TempInvoice
														DROP TABLE #TestTable";
            try
            {
                sqlQuery = string.Format(sqlQuery,Filter.PageNo,Filter.PageSize, Filter.CompanyId, searchquery, subquery, orderquery, orderquery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable DownloadRMRInvoiceListByCompanyId(RMRFilter Filter)
        {
            string subquery = "";
            if (Filter.StartDate.HasValue && Filter.StartDate.Value != new DateTime() && Filter.EndDate.HasValue && Filter.EndDate.Value != new DateTime())
            {
                Filter.StartDate = Filter.StartDate.Value.SetZeroHour();
                Filter.EndDate = Filter.EndDate.Value.SetMaxHour();
                subquery = string.Format("and Inv.InvoiceDate between '{0}' and '{1}'", Filter.StartDate, Filter.EndDate);
            }
            string searchquery = "";
            if (!string.IsNullOrWhiteSpace(Filter.SearchText))
            {
                searchquery += string.Format("and (Cus.FirstName like '%{0}%' or Cus.LastName like '%{0}%' or Cus.FirstName + ' ' + Cus.LastName like '%{0}%' or Cus.BusinessName like '%{0}%' or Inv.InvoiceId like '%{0}%')", Filter.SearchText);
            }

            string sqlQuery = @"select
                                CASE 
								WHEN (Cus.BusinessName = '' or Cus.BusinessName IS NULL) THEN Cus.FirstName +' '+Cus.LastName
								ELSE  Cus.BusinessName
								END as [Customer Name]
                                ,Inv.Id
                                ,Inv.InvoiceId as [Invoice Id]
								,Cus.Id as CustomerIntId
                                ,FORMAT(Inv.InvoiceDate,'M/d/yy') as [Date]
                                ,Inv.TotalAmount as [Amount Due]
                                ,Inv.BalanceDue as [Net Due]

								 from Invoice Inv
                                 left join Customer Cus on Cus.CustomerId = Inv.CustomerId
                                 where Inv.CompanyId ='{0}'
                                 and Inv.IsARBInvoice = 1
                                 and Inv.IsEstimate = 0
                                 and Inv.[Status] not in('Cancelled', 'Rolled Over', 'Init')
                                 {1}
                                 {2}
                                 order by id desc";
            try
            {
                sqlQuery = string.Format(sqlQuery, Filter.CompanyId, searchquery, subquery);
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

        public DataSet GetRMRHistoryListByCompanyId(RMRFilter Filter)
        {
            string subquery = "";
            string orderquery = "";
            string orderquery1 = "";
            if (Filter.StartDate.HasValue && Filter.StartDate.Value != new DateTime() && Filter.EndDate.HasValue && Filter.EndDate.Value != new DateTime())
            {
                Filter.StartDate = Filter.StartDate.Value.SetZeroHour();
                Filter.EndDate = Filter.EndDate.Value.SetMaxHour();
                subquery = string.Format("and Inv.InvoiceDate between '{0}' and '{1}'", Filter.StartDate, Filter.EndDate);
            }
            string searchquery = "";
            if (!string.IsNullOrWhiteSpace(Filter.SearchText))
            {
                searchquery += string.Format("and (Cus.FirstName like '%{0}%' or Cus.LastName like '%{0}%' or Cus.FirstName + ' ' + Cus.LastName like '%{0}%' or Cus.BusinessName like '%{0}%' or Inv.InvoiceId like '%{0}%')", Filter.SearchText);
            }
            #region Order
            if (!string.IsNullOrWhiteSpace(Filter.Order))
            {
                if (Filter.Order == "ascending/customer")
                {
                    orderquery = "order by #TempInv.[CustomerName] asc";
                    orderquery1 = "order by [CustomerName] asc";
                }
                else if (Filter.Order == "descending/customer")
                {
                    orderquery = "order by #TempInv.[CustomerName] desc";
                    orderquery1 = "order by [CustomerName] desc";
                }
                else if (Filter.Order == "ascending/invoiceid")
                {
                    orderquery = "order by #TempInv.InvoiceId asc";
                    orderquery1 = "order by InvoiceId asc";
                }
                else if (Filter.Order == "descending/invoiceid")
                {
                    orderquery = "order by #TempInv.InvoiceId desc";
                    orderquery1 = "order by InvoiceId desc";
                }
                else if (Filter.Order == "ascending/date")
                {
                    orderquery = "order by #TempInv.InvoiceDate asc";
                    orderquery1 = "order by InvoiceDate asc";
                }
                else if (Filter.Order == "descending/date")
                {
                    orderquery = "order by #TempInv.InvoiceDate desc";
                    orderquery1 = "order by InvoiceDate desc";
                }
                else if (Filter.Order == "ascending/method")
                {
                    orderquery = "order by #TempInv.PaymentMethod asc";
                    orderquery1 = "order by PaymentMethod asc";
                }
                else if (Filter.Order == "descending/method")
                {
                    orderquery = "order by #TempInv.PaymentMethod desc";
                    orderquery1 = "order by PaymentMethod desc";
                }
                else if (Filter.Order == "ascending/checkno")
                {
                    orderquery = "order by #TempInv.CheckNo asc";
                    orderquery1 = "order by CheckNo asc";
                }
                else if (Filter.Order == "descending/checkno")
                {
                    orderquery = "order by #TempInv.CheckNo desc";
                    orderquery1 = "order by CheckNo desc";
                }
                else if (Filter.Order == "ascending/paymentdate")
                {
                    orderquery = "order by #TempInv.PaymentDate asc";
                    orderquery1 = "order by PaymentDate asc";
                }
                else if (Filter.Order == "descending/paymentdate")
                {
                    orderquery = "order by #TempInv.PaymentDate desc";
                    orderquery1 = "order by PaymentDate desc";
                }
                else if (Filter.Order == "ascending/amount")
                {
                    orderquery = "order by #TempInv.Amount asc";
                    orderquery1 = "order by Amount asc";
                }
                else if (Filter.Order == "descending/amount")
                {
                    orderquery = "order by #TempInv.Amount desc";
                    orderquery1 = "order by Amount desc";
                }
                else if (Filter.Order == "ascending/batchcode")
                {
                    orderquery = "order by #TempInv.BatchNumber asc";
                    orderquery1 = "order by BatchNumber asc";
                }
                else if (Filter.Order == "descending/batchcode")
                {
                    orderquery = "order by #TempInv.BatchNumber desc";
                    orderquery1 = "order by BatchNumber desc";
                }
                else if (Filter.Order == "ascending/funded")
                {
                    orderquery = "order by #TempInv.Funded asc";
                    orderquery1 = "order by Funded asc";
                }
                else if (Filter.Order == "descending/funded")
                {
                    orderquery = "order by #TempInv.Funded desc";
                    orderquery1 = "order by Funded desc";
                }
                else if (Filter.Order == "ascending/posted")
                {
                    orderquery = "order by #TempInv.Posted asc";
                    orderquery1 = "order by Posted asc";
                }
                else if (Filter.Order == "descending/posted")
                {
                    orderquery = "order by #TempInv.Posted desc";
                    orderquery1 = "order by Posted desc";
                }
                else
                {
                    orderquery = "order by #TempInv.PaymentDate desc";
                    orderquery1 = "order by PaymentDate desc";
                }
            }
            else
            {
                orderquery = "order by #TempInv.Id desc";
                orderquery1 = "order by Id desc";
            }
            #endregion
            string sqlQuery = @"DECLARE @pagestart int
	                            DECLARE @pageend int
	                            DECLARE @pageno int
	                            DECLARE @pagesize int

                                SET @pageno = {0}
								SET @pagesize = {1}
                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

                                select
                                Inv.Id
                                ,Inv.InvoiceId
                                ,Inv.InvoiceDate
								,CASE 
								WHEN (Cus.BusinessName = '' or Cus.BusinessName IS NULL) THEN Cus.FirstName +' '+Cus.LastName
								ELSE  Cus.BusinessName
								END as CustomerName
								,Cus.Id as CustomerIntId
                                ,TH.Amout
								,T.PaymentMethod
								,T.CheckNo
								,T.TransacationDate as PaymentDate
                                --,(select top (1) Tr.PaymentMethod from [Transaction] Tr where Tr.ReferenceNo = Inv.InvoiceId order by Id desc) as PaymentMethod
                                --,(select top (1) Tr.CheckNo from [Transaction] Tr where Tr.ReferenceNo = Inv.InvoiceId order by Id desc) as CheckNo
                                --,(select top (1) Tr.TransacationDate from [Transaction] Tr where Tr.ReferenceNo = Inv.InvoiceId order by Id desc) as PaymentDate
                                --,(select top (1) Tr.Amount from [Transaction] Tr where Tr.ReferenceNo = Inv.InvoiceId order by Id desc) as Amount
                                ,Inv.BatchNumber
                                ,CASE WHEN ((Cus.AlarmRefId is not null AND Cus.AlarmRefId !='') OR (Cus.BrinksRefId is not null AND Cus.BrinksRefId !='') OR (Cus.UCCRefId is not null AND Cus.UCCRefId !=''))  THEN 'Yes' ELSE 'No' end as Funded
                                ,'Posted' as Posted
		                         into #TempInvoice

								 from Invoice Inv 
                                 left join Customer Cus on Cus.CustomerId = Inv.CustomerId
                                 left join TransactionHistory TH on TH.InvoiceId = Inv.Id
								 left join [Transaction] T on T.Id = TH.TransactionId
                                 where Inv.CompanyId ='{2}'
                                 and Inv.IsARBInvoice = 1
                                 and Inv.IsEstimate = 0
                                 and Inv.[Status] != 'Init'
                                 {3}
                                 {4}

														SELECT TOP (@pagesize) #TempInv.* into #TestTable
														FROM #TempInvoice #TempInv
														where Id NOT IN(Select TOP (@pagestart) Id from #TempInvoice #TempInv {5})
														{6}
														select  count(Id) as [TotalCount] from #TempInvoice

														select * from #TestTable
														select sum(Amout) as TotalAmount from #TestTable

														DROP TABLE #TempInvoice
														DROP TABLE #TestTable";
            try
            {
                sqlQuery = string.Format(sqlQuery, Filter.PageNo, Filter.PageSize, Filter.CompanyId, searchquery, subquery, orderquery, orderquery1);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable DownloadRMRHistoryListByCompanyId(RMRFilter Filter)
        {
            string subquery = "";
            if (Filter.StartDate.HasValue && Filter.StartDate.Value != new DateTime() && Filter.EndDate.HasValue && Filter.EndDate.Value != new DateTime())
            {
                Filter.StartDate = Filter.StartDate.Value.SetZeroHour();
                Filter.EndDate = Filter.EndDate.Value.SetMaxHour();
                subquery = string.Format("and Inv.InvoiceDate between '{0}' and '{1}'", Filter.StartDate, Filter.EndDate);
            }
            string searchquery = "";
            if (!string.IsNullOrWhiteSpace(Filter.SearchText))
            {
                searchquery += string.Format("and (Cus.FirstName like '%{0}%' or Cus.LastName like '%{0}%' or Cus.FirstName + ' ' + Cus.LastName like '%{0}%' or Cus.BusinessName like '%{0}%' or Inv.InvoiceId like '%{0}%')", Filter.SearchText);
            }

            string sqlQuery = @"select
                                Inv.Id
                                ,Inv.InvoiceId as [Invoice Id]
                                ,FORMAT(Inv.InvoiceDate,'M/d/yy') as [Date]
								,CASE 
								WHEN (Cus.BusinessName = '' or Cus.BusinessName IS NULL) THEN Cus.FirstName +' '+Cus.LastName
								ELSE  Cus.BusinessName
								END as [Customer]
								,Cus.Id as CustomerIntId
                                ,TH.Amout as Amount
								,T.PaymentMethod as [Payment Method]
								,T.CheckNo as [Check No.]
								,FORMAT(T.TransacationDate,'M/d/yy') as [Payment Date]
                                --,(select top (1) Tr.PaymentMethod from [Transaction] Tr where Tr.ReferenceNo = Inv.InvoiceId order by Id desc) as [Payment Method]
                                --,(select top (1) Tr.CheckNo from [Transaction] Tr where Tr.ReferenceNo = Inv.InvoiceId order by Id desc) as [Check No.]
                                --,(select top (1) FORMAT(Tr.TransacationDate,'M/d/yy') from [Transaction] Tr where Tr.ReferenceNo = Inv.InvoiceId order by Id desc) as [Payment Date]
                                --,(select top (1) Tr.Amount from [Transaction] Tr where Tr.ReferenceNo = Inv.InvoiceId order by Id desc) as Amount
                                ,Inv.BatchNumber as [Batch Number]
                                ,CASE WHEN ((Cus.AlarmRefId is not null AND Cus.AlarmRefId !='') OR (Cus.BrinksRefId is not null AND Cus.BrinksRefId !='') OR (Cus.UCCRefId is not null AND Cus.UCCRefId !=''))  THEN 'Yes' ELSE 'No' end as [Funded Status]
                                ,'Posted' as Posted
		                  

								 from Invoice Inv 
                                 left join Customer Cus on Cus.CustomerId = Inv.CustomerId
                                 left join TransactionHistory TH on TH.InvoiceId = Inv.Id
								 left join [Transaction] T on T.Id = TH.TransactionId
                                 Where Inv.CompanyId ='{0}'
                                 and Inv.IsARBInvoice = 1
                                 and Inv.IsEstimate = 0
                                 and Inv.[Status] != 'Init'
                                 {1}
                                 {2}
                                 order by T.TransacationDate desc";
            try
            {
                sqlQuery = string.Format(sqlQuery, Filter.CompanyId, searchquery, subquery);
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
