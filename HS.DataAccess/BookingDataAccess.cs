using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;
using System.Linq;

namespace HS.DataAccess
{
    public partial class BookingDataAccess
    {
        public BookingDataAccess() { }
        public BookingDataAccess(string ConnectionString) : base(ConnectionString)
        {

        }


        public DataTable GetRugShapeListBySearchKey(string key)
        {
            string sqlQuery = @" select * from Lookup where DataKey = 'RugShape' AND IsActive = 1";
            try
            {
                sqlQuery = string.Format(sqlQuery, key);
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

        public DataTable GetPackageList()
        {
            string sqlQuery = @" SELECT p.Id, p.Name, p.CompanyId, p.OptionEqpMaxLimit FROM Package p ";
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

        public DataTable GetPackageListIncludeAndRate()
        {
            string sqlQuery = @" select pak.Id as PackageId, pak.Name as PackageName, eq.Name as Included, mr.MinMMR as Rate 
                                    from PackageInclude pn
                                    left join Package pak on pak.Id = pn.PackageId
                                    left join Equipment eq on eq.EquipmentId = pn.EquipmentId
                                    left join MMRRange mr on mr.PackageId = pn.PackageId ";
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

        public DataSet GetShapeAndPackageList()
        {
            string sqlQuery = @" (select * from Lookup where DataKey = 'RugShape')
                                 (select * from package)";
            try
            {
                sqlQuery = string.Format(sqlQuery);
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

        public DataSet GetPackageAndIncludeList()
        {
            string sqlQuery = @"select pak.Name as PackageName, pak.Id as PackageId, mr.MinMMR as PackageRate 
                                from Package pak
                                left join PackageInclude pn on pak.Id = pn.PackageId
                                left join Equipment eq on eq.EquipmentId = pn.EquipmentId
                                left join MMRRange mr on mr.PackageId = pak.PackageId
                                group by pak.Name, pak.Id  , mr.MinMMR

                                select pin.PackageId as PackId, equ.Name as PackageInclude from PackageInclude pin
                                left join Equipment equ on equ.EquipmentId = pin.EquipmentId   
                                order by pin.OrderBy asc";
            try
            {
                sqlQuery = string.Format(sqlQuery);
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

        //Get All Booking List By CustomerId And CompanyId Query 
        public DataTable GetAllBookingListByCustomerIdAndCompanyId(Guid CustomerId, Guid companyId)
        {
            string sqlQuery = @"Declare @CustomerId uniqueidentifier
                                Declare @CompanyId uniqueidentifier
                                set @CustomerId ='{0}' 
                                set @CompanyId = '{1}'

                                select _Customer.FirstName+' '+_Customer.MiddleName +' '+_Customer.LastName CustomerName
                                ,_Booking.*
                                ,(select top 1 AddedDate from CustomerAgreement where InvoiceId = _Booking.BookingId and CompanyId = @CompanyId order by id desc) as CustomerViewedTime
                                ,(select top 1 Type from CustomerAgreement where InvoiceId = _Booking.BookingId and CompanyId = @CompanyId order by id desc) as CustomerViewedType
                                ,emp.FirstName + ' ' + emp.LastName as UserNum
                                
                                from Booking _Booking
                                left join Customer _Customer 
                                on _Booking.CustomerId = _Customer.CustomerId
                                left join Employee emp
                                on emp.UserId = _Booking.CreatedBy
								 
                                where _Booking.CompanyId =  @CompanyId
                                and _Booking.CustomerId = @CustomerId
                                and _Booking.Status != 'Init'
                                order by _Booking.Id Desc  ";
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId, companyId);
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


        //public DataTable GetAllUserActivityCustomerListByCustomerId(Guid CustomerId)
        //{
        //    string sqlQuery = @"Declare @CustomerId uniqueidentifier
        //                        set @CustomerId ='{0}' 

        //                        select _Customer.FirstName+' '+_Customer.MiddleName +' '+_Customer.LastName CustomerName 
        //                        ,_UserActivity.*,_UserActivityCustomer.CustomerId as UACCustomerId
                                
        //                        from UserActivityCustomer _UserActivityCustomer
        //                        left join Customer _Customer 
        //                        on _UserActivityCustomer.CustomerId = _Customer.CustomerId
        //                        left join UserActivity _UserActivity
        //                        on _UserActivity.ActivityId = _UserActivityCustomer.ActivityId
								 
        //                        where _UserActivityCustomer.CompanyId =  @CompanyId
        //                        and _UserActivityCustomer.CustomerId = @CustomerId
        //                        order by _UserActivity.Id Desc  ";
        //    try
        //    {
        //        sqlQuery = string.Format(sqlQuery, CustomerId);
        //        using (SqlCommand cmd = GetSQLCommand(sqlQuery))
        //        {
        //            DataSet dsResult = GetDataSet(cmd);
        //            return dsResult.Tables[0];
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public DataSet GetAllUserActivityCustomerListByCustomerId(int pageno, int pagesize, DateTime? startdate, DateTime? enddate, string searchtext, Guid CustomerGId, string order,string logstartdate,string logenddate)
        {
            string sqlQuery = @"";
            string subquery = "";
            string subquery1 = "";
            string setext = "";
            string statusquery = "";
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                setext = string.Format("and (_Customer.FirstName+' '+_Customer.MiddleName +' '+_Customer.LastName like '%{0}%' or _UserActivity.StatsDate like '%{0}%' or _UserActivity.UserName like '%{0}%'  or _UserActivity.ActionDisplyText like '%{0}%' or _UserActivity.Action like '%{0}%' )", searchtext);
            }
         

            sqlQuery = @"DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int
                                Declare @CustomerId uniqueidentifier
                                set @CustomerId ='{7}' 
                                SET @pageno = {0}
                                SET @pagesize = {1}
                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

                                select distinct _Customer.FirstName+' '+_Customer.MiddleName +' '+_Customer.LastName CustomerName 
                                ,_UserActivity.*,_UserActivityCustomer.CustomerId as UACCustomerId
                                
                                into #CustomerData from UserActivityCustomer _UserActivityCustomer
                                left join Customer _Customer 
                                on _UserActivityCustomer.CustomerId = _Customer.CustomerId
                                left join UserActivity _UserActivity
                                on _UserActivity.ActivityId = _UserActivityCustomer.ActivityId
                                where _UserActivityCustomer.CustomerId = @CustomerId
                                and _UserActivity.Id is  not null
                                --{6}
                                {5}
                                {3}
                                {8}

                                select * into #CustomerDataFilter from #CustomerData

                                SELECT TOP(@pagesize) * FROM #CustomerDataFilter where Id NOT IN(Select TOP (@pagestart) #cus.Id from #CustomerData #cus {2})
                                {4}

                                select COUNT(Id) as TotalCount from #CustomerDataFilter
                                drop table #CustomerData
                                drop table #CustomerDataFilter";

            if (!string.IsNullOrWhiteSpace(order) && order != "undefined" && order != "null")
            {
                if (order == "ascending/customername")
                {
                    subquery = "order by #cus.CustomerName asc";
                    subquery1 = "order by CustomerName asc";
                }
                else if (order == "descending/customername")
                {
                    subquery = "order by #cus.CustomerName desc";
                    subquery1 = "order by CustomerName desc";
                }

                else if (order == "ascending/action")
                {
                    subquery = "order by #cus.Action asc";
                    subquery1 = "order by Action asc";
                }
                else if (order == "descending/action")
                {
                    subquery = "order by #cus.Action desc";
                    subquery1 = "order by Action desc";
                }

                else if (order == "ascending/actiondisplytext")
                {
                    subquery = "order by #cus.ActionDisplyText asc";
                    subquery1 = "order by ActionDisplyText asc";
                }
                else if (order == "descending/actiondisplytext")
                {
                    subquery = "order by #cus.ActionDisplyText desc";
                    subquery1 = "order by ActionDisplyText desc";
                }

                else if (order == "ascending/username")
                {
                    subquery = "order by #cus.UserName asc";
                    subquery1 = "order by UserName asc";
                }
                else if (order == "descending/username")
                {
                    subquery = "order by #cus.UserName desc";
                    subquery1 = "order by UserName desc";
                }

                else if (order == "ascending/statsdate")
                {
                    subquery = "order by #cus.StatsDate asc";
                    subquery1 = "order by StatsDate asc";
                }
                else if (order == "descending/statsdate")
                {
                    subquery = "order by #cus.StatsDate desc";
                    subquery1 = "order by StatsDate desc";
                }
            }
            else
            {
                subquery = "order by #cus.Id desc";
                subquery1 = "order by Id desc";
            }

            string dateFilter = "";
            if (startdate.HasValue && enddate.HasValue && startdate.Value != new DateTime() && enddate.Value != new DateTime())
            {
                dateFilter = string.Format(@"and _UserActivity.StatsDate >= '{0}' and _UserActivity.StatsDate <= '{1}'", startdate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), enddate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            string filterquery = "";
            if (!string.IsNullOrWhiteSpace(logstartdate) && !string.IsNullOrWhiteSpace(logenddate))
            {
                var datemin = Convert.ToDateTime(logstartdate);
                var date = Convert.ToDateTime(logenddate);
                filterquery += string.Format("and _UserActivity.StatsDate between '{0}' and '{1}'", datemin.SetZeroHour().ToString("yyyy-MM-dd HH:mm:ss.fff"), date.SetMaxHour().ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            else if (!string.IsNullOrWhiteSpace(logstartdate))
            {
                var date = Convert.ToDateTime(logstartdate);
                filterquery += string.Format("and _UserActivity.StatsDate between '{0}' and '{1}'", date.SetZeroHour().ToString("yyyy-MM-dd HH:mm:ss.fff"), date.SetMaxHour().ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            else if (!string.IsNullOrWhiteSpace(logenddate))
            {
                var date = Convert.ToDateTime(logenddate);
                filterquery += string.Format("and _UserActivity.StatsDate between '{0}' and '{1}'", date.SetZeroHour().ToString("yyyy-MM-dd HH:mm:ss.fff"), date.SetMaxHour().ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            sqlQuery = string.Format(sqlQuery, pageno, pagesize, subquery, statusquery, subquery1, setext, dateFilter, CustomerGId,filterquery);
            try
            {
                sqlQuery = string.Format(sqlQuery);
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


        public DataTable GetAllUserActivityCustomerListByCustomerIdExport(Guid CustomerGId,DateTime? startdate, DateTime? enddate, string searchtext)
        {
            string sqlQuery = @"";
            string setext = "";
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                setext = string.Format("and _Customer.FirstName+' '+_Customer.MiddleName +' '+_Customer.LastName like '%{0}%' or _UserActivity.StatsDate like '%{0}%' or _UserActivity.UserName like '%{0}%'  or _UserActivity.ActionDisplyText like '%{0}%' or _UserActivity.Action like '%{0}%'", searchtext);
            }

            sqlQuery = @"    select _Customer.FirstName+' '+_Customer.MiddleName +' '+_Customer.LastName CustomerName 
                                ,_UserActivity.*,_UserActivityCustomer.CustomerId as UACCustomerId
                                
                                from UserActivityCustomer _UserActivityCustomer
                                left join Customer _Customer 
                                on _UserActivityCustomer.CustomerId = _Customer.CustomerId
                                left join UserActivity _UserActivity
                                on _UserActivity.ActivityId = _UserActivityCustomer.ActivityId
                                where _UserActivityCustomer.CustomerId = '{0}'
                                {2}
                                {1}

                                ";
           
           
           
            string pageno = "";
            

            string dateFilter = "";
            if (startdate.HasValue && enddate.HasValue && startdate.Value != new DateTime() && enddate.Value != new DateTime())
            {
                dateFilter = string.Format(@"and _UserActivity.StatsDate >= '{0}' and _UserActivity.StatsDate <= '{1}'", startdate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), enddate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }

            sqlQuery = string.Format(sqlQuery, CustomerGId,  setext, dateFilter);
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


        //Get Booking Details By Booking Id
        public DataTable GetBookingDetialsListByBookingId(string bookingId)
        {
            string sqlQuery = @" Select * from BookingDetails
                                where BookingId = '{0}' ";
            try
            {
                sqlQuery = string.Format(sqlQuery, bookingId);
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

        //Get IsVip Details By Booking Id
        public DataTable IsVipFromBookingByCompanyIdAndCustomerId(Guid CompanyId, Guid CustomerId)
        {
            int year = DateTime.UtcNow.UTCToClientTime().Year;
            DateTime firstDay = new DateTime(year, 1, 1);
            DateTime lastDay = new DateTime(year, 12, 31).AddSeconds(86399);

            string sqlQuery = @"select CASE WHEN ((select CASE WHEN (Select COUNT(Id) from Booking where CustomerId='{0}' and CompanyId='{1}' 
                                and LOWER(Status) = 'approved' and CreatedDate BETWEEN '{2}' and '{3}') > 1 THEN CAST(1 AS BIT)
                                ELSE CAST(0 AS BIT) END) > 0 and (select CASE WHEN (Select COUNT(Id) from Booking where CustomerId='{0}' and CompanyId='{1}' 
                                and LOWER(Status) = 'approved' and CreatedDate BETWEEN '{4}' and '{5}') > 1 THEN CAST(1 AS BIT)
                                ELSE CAST(0 AS BIT) END) > 0 and (select CASE WHEN (Select COUNT(Id) from Booking where CustomerId='{0}' and CompanyId='{1}' 
                                and LOWER(Status) = 'approved' and CreatedDate BETWEEN '{6}' and '{7}') > 1 THEN CAST(1 AS BIT)
                                ELSE CAST(0 AS BIT) END) > 0 )
                                THEN CAST(1 AS BIT)
                                ELSE CAST(0 AS BIT) END as IsVip";
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId, CompanyId, firstDay.AddYears(-1), lastDay.AddYears(-1) , firstDay.AddYears(-2), lastDay.AddYears(-2), firstDay.AddYears(-3), lastDay.AddYears(-3));
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

        //Delete By Booking ID
        public bool DeleteByBookingId(string bookingId)
        {
            string SqlQuery = @" DELETE FROM BookingDetails WHERE BookingId ='{0}' ";
            SqlQuery = string.Format(SqlQuery, bookingId);
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

        //Get Email Content 
        public DataTable GetEmailSubAndDescriptionByBookingId(string bookingId)
        {
            string sqlQuery = @" SELECT * from EmailHistory eh WHERE 
                            eh.TemplateKey = 'BookingEmail' AND eh.EmailSubject LIKE '%{0}%' ";
            try
            {
                sqlQuery = string.Format(sqlQuery, bookingId);
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
        public DataSet GetAllJobsReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, bool IsPaid, string source)
        {
      
            string DateFilter1 = "";
            string sourcequery = "";
            string tempQuery = "";
            string[] TempSource = source.Split(',');
            if (!string.IsNullOrWhiteSpace(source) && source != "'null'" && source != "null")
            {
                if (source.Contains("Online") && TempSource.Length == 1)
                {
                    tempQuery += " and bk.BookingSource = 'Online'";
                }
                else if(source.Contains("System") && TempSource.Length == 1)
                {
                    tempQuery += " and (bk.BookingSource is null or bk.BookingSource != 'Online')";
                }
                //sourcequery = string.Format("and bk.BookingSource in ({0})", source);
            }
            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime())
            {

                FilterStartDate = FilterStartDate.SetZeroHour();
                FilterEndDate = FilterEndDate.SetMaxHour();
                DateFilter1 = string.Format("and bk.CreatedDate between '{0}' and '{1}'"
                    , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            //if (IsPaid)
            //{
            //    IsPaidQuery = "where mc.IsPaid = 1";
            //}
            //else
            //{
            //    IsPaidQuery = "where mc.IsPaid = 0 or mc.IsPaid is null    ";
            //}
            string sqlQuery = @"
                                declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                 select * into #JobsReport  from (select bk.*,cus.FirstName+' '+cus.LastName as CustomerName,cus.Id as CustomerIntId,emp.FirstName+' '+emp.LastName as CreatedByVal from booking bk
                                left join customer cus on cus.CustomerId = bk.CustomerId
                                left join employee emp on emp.UserId = bk.CreatedBy
                                 where bk.Status = 'Approved'   {3}  {4}                      
                                ) d	

                                 select * into #JobsReportFilter
								from #JobsReport


								select top(@pagesize)
								* from #JobsReportFilter
								where Id not in(select top(@pagestart) Id from #JobsReport #e {1})
                                {2}
								select count(*) CountTotal
                                from #JobsReportFilter

                                select count(*) TotalOnline 
                                from #JobsReportFilter Where BookingSource ='Online'
								select count(*) TotalSystem
                                from #JobsReportFilter Where (BookingSource is null or BookingSource != 'Online')
                                select sum(TotalAmount) TotalAmount
                                from #JobsReportFilter

								drop table #JobsReport
								drop table #JobsReportFilter";

            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/bookingid")
                {
                    subquery = "order by #e.BookingId asc";
                    subquery1 = "order by BookingId asc";
                }
                else if (order == "descending/bookingid")
                {
                    subquery = "order by #e.BookingId desc";
                    subquery1 = "order by BookingId desc";
                }
                else if (order == "ascending/amount")
                {
                    subquery = "order by #e.TotalAmount asc";
                    subquery1 = "order by TotalAmount asc";
                }
                else if (order == "descending/amount")
                {
                    subquery = "order by #e.TotalAmount desc";
                    subquery1 = "order by TotalAmount desc";
                }
                else if (order == "ascending/billing")
                {
                    subquery = "order by #e.BillingAddress asc";
                    subquery1 = "order by BillingAddress asc";
                }
                else if (order == "descending/billing")
                {
                    subquery = "order by #e.BillingAddress desc";
                    subquery1 = "order by BillingAddress desc";
                }
                else if (order == "ascending/createddate")
                {
                    subquery = "order by #e.CreatedDate asc";
                    subquery1 = "order by CreatedDate asc";
                }
                else if (order == "descending/createddate")
                {
                    subquery = "order by #e.CreatedDate desc";
                    subquery1 = "order by CreatedDate desc";
                }
                else if (order == "ascending/pickupdate")
                {
                    subquery = "order by #e.PickupDate asc";
                    subquery1 = "order by PickupDate asc";
                }
                else if (order == "descending/pickupdate")
                {
                    subquery = "order by #e.PickupDate desc";
                    subquery1 = "order by PickupDate desc";
                }

                else if (order == "ascending/createdby")
                {
                    subquery = "order by #e.CreatedByVal asc";
                    subquery1 = "order by CreatedByVal asc";
                }
                else if (order == "descending/createdby")
                {
                    subquery = "order by #e.CreatedByVal desc";
                    subquery1 = "order by CreatedByVal desc";
                }
            }
            else
            {
                subquery = "order by #e.CreatedDate desc";
                subquery1 = "order by CreatedDate desc";
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, order, subquery, subquery1, DateFilter1, tempQuery);
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
        public DataTable GetJobReportExport(DateTime FilterStartDate, DateTime FilterEndDate, string order, bool IsPaid, string source)
        {
            string DateFilter1 = "";
            string tempQuery="";
            string sourcequery = "";
            string[] TempSource = source.Split(',');
            if (!string.IsNullOrWhiteSpace(source) && source != "'null'" && source != "null")
            {
                if (source.Contains("Online") && TempSource.Length == 1)
                {
                    tempQuery += " and bk.BookingSource = 'Online'";
                }
                else if (source.Contains("System") && TempSource.Length == 1)
                {
                    tempQuery += " and (bk.BookingSource is null or bk.BookingSource != 'Online')";
                }
                //sourcequery = string.Format("and bk.BookingSource in ({0})", source);
            }
            if (FilterStartDate != new DateTime() && FilterEndDate != new DateTime())
            {

                FilterStartDate = FilterStartDate.SetZeroHour();
                FilterEndDate = FilterEndDate.SetMaxHour();
                DateFilter1 = string.Format("and bk.CreatedDate between '{0}' and '{1}'"
                    , FilterStartDate.ToString("yyyy-MM-dd HH:mm:ss")
                    , FilterEndDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            //if (IsPaid)
            //{
            //    IsPaidQuery = "where mc.IsPaid = 1";
            //}
            //else
            //{
            //    IsPaidQuery = "where mc.IsPaid = 0 or mc.IsPaid is null    ";
            //}
            string sqlQuery = @"
                                select * into #JobsReport  from (select bk.BookingId as [Booking Id],cus.FirstName+' '+cus.LastName as [Customer Name], iif(bk.BookingSource = 'Online', 'Online', 'System Generated') as [Source],replace((select [dbo].[udf_StripHTML] (bk.BillingAddress)),'&nbsp;','') as [Billing Address], convert(date,bk.PickUpDate) as [Pickup Date], convert(date, bk.CreatedDate) as [Created Date],emp.FirstName+' '+emp.LastName as [Created By], format(bk.TotalAmount,'N2') as [Amount] from booking bk
                                left join customer cus on cus.CustomerId = bk.CustomerId
                                left join employee emp on emp.UserId = bk.CreatedBy
                                 where bk.Status = 'Approved'   {3}  {4}                      
                                ) d	

                                 select * into #JobsReportFilter
								from #JobsReport


								select
								* from #JobsReportFilter
                                {2}
                                
								drop table #JobsReport
								drop table #JobsReportFilter";

            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/bookingid")
                {
                    subquery = "order by #e.BookingId asc";
                    subquery1 = "order by [Booking Id] asc";
                }
                else if (order == "descending/bookingid")
                {
                    subquery = "order by #e.BookingId desc";
                    subquery1 = "order by [Booking Id] desc";
                }
                else if (order == "ascending/amount")
                {
                    subquery = "order by #e.TotalAmount asc";
                    subquery1 = "order by [Amount] asc";
                }
                else if (order == "descending/amount")
                {
                    subquery = "order by #e.TotalAmount desc";
                    subquery1 = "order by [Amount] desc";
                }
                else if (order == "ascending/billing")
                {
                    subquery = "order by #e.BillingAddress asc";
                    subquery1 = "order by [Billing Address] asc";
                }
                else if (order == "descending/billing")
                {
                    subquery = "order by #e.BillingAddress desc";
                    subquery1 = "order by [Billing Address] desc";
                }
                else if (order == "ascending/createddate")
                {
                    subquery = "order by #e.CreatedDate asc";
                    subquery1 = "order by [Created Date] asc";
                }
                else if (order == "descending/createddate")
                {
                    subquery = "order by #e.CreatedDate desc";
                    subquery1 = "order by [Created Date] desc";
                }
                else
                {
                    subquery = "order by #e.CreatedDate desc";
                    subquery1 = "order by [Created Date] desc";
                }
            }
            else
            {
                subquery = "order by #e.CreatedDate desc";
                subquery1 = "order by [Created Date] desc";
            }

            try
            {
                sqlQuery = string.Format(sqlQuery, order, subquery, subquery1, DateFilter1, tempQuery);
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

        public DataSet GetFinishedJobreportsByFilter(FinishedJobFilter filter)
        {
            string DateFilter = "";
            string SearchText = "";

            if (filter.StartDate.HasValue && filter.StartDate.Value != new DateTime()
                &&filter.EndDate.HasValue && filter.EndDate.Value != new DateTime())
            {
                string startDate = filter.StartDate.Value.SetZeroHour().ToString("yyyy-MM-dd HH:mm:ss");
                string endDate = filter.EndDate.Value.SetMaxHour().ToString("yyyy-MM-dd HH:mm:ss");

                DateFilter = string.Format(" and tic.CompletionDate between '{0}' and '{1}' ", startDate, endDate);
            }
            if(!string.IsNullOrWhiteSpace(filter.SearchText) && filter.SearchText != "undefined" && filter.SearchText != "null")
            {
                SearchText = string.Format(" and (CustomerName like '%{0}%' or BookingId like '%{0}%' or Id like '%{0}%' or Address like '%{0}%' or Phone like '%{0}%'  or Email like '%{0}%' or Discount like '%{0}%' or TotalPrice like '%{0}%')", filter.SearchText);
            }

            string SQLQuery = @"declare @pagestart int
                                declare @pageend int
                                declare @pageno int
                                declare @pagesize int
                                set @pageno = {0}
                                set @pagesize = {1}

                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                select tic.Id ,
                                cus.Id as CustomerId,
                                {3} as CustomerName,
                                bk.BookingId as BookingId,
                                bk.Id as BookingIntId,
                                cus.PrimaryPhone as Phone,
                                dbo.MakeAddress(cus.Street,cus.StreetType, cus.Appartment,cus.ZipCode,cus.City,cus.[State]) as [Address],
                                cus.EmailAddress as Email,
                                inv.DiscountAmount as Discount,
                                inv.TotalAmount as TotalPrice,
                                tic.CompletionDate
                                into #JobsReport
                                from Ticket tic
                                left join Customer cus on  tic.CustomerId = cus.CustomerId
                                left join Booking bk on bk.BookingId = tic.BookingId
                                left join Invoice inv on inv.BookingId = tic.BookingId

                                where tic.TicketType ='Drop Off'
                                and (tic.Status = 'Completed' Or tic.Status = 'Closed' )
                                and tic.BookingId != ''
                                {2}

                                 select top (@pagesize)
                                 * from #JobsReport
                                 where Id not in(select top(@pagestart) Id from #JobsReport {4}) {6}
                                 {5} 

                                select count(ID) TotalCount
                                from #JobsReport where Id is not null {6}

                                drop table #JobsReport
                                ";

            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(filter.order))
            {
                if (filter.order == "ascending/customername")
                {
                    subquery = "order by CustomerName asc";
                    subquery1 = "order by CustomerName asc";
                }
                else if (filter.order == "descending/customername")
                {
                    subquery = "order by CustomerName desc";
                    subquery1 = "order by CustomerName desc";
                }


                else if (filter.order == "ascending/bookingid")
                {
                    subquery = "order by BookingId asc";
                    subquery1 = "order by BookingId asc";
                }
                else if (filter.order == "descending/bookingid")
                {
                    subquery = "order by BookingId desc";
                    subquery1 = "order by BookingId desc";
                }

                ///////////
                else if (filter.order == "ascending/id")
                {
                    subquery = "order by ID asc";
                    subquery1 = "order by Id asc";
                }
                else if (filter.order == "descending/id")
                {
                    subquery = "order by ID desc";
                    subquery1 = "order by Id desc";
                }
                //////////////

                else if (filter.order == "ascending/address")
                {
                    subquery = "order by Address asc";
                    subquery1 = "order by Address asc";
                }
                else if (filter.order == "descending/address")
                {
                    subquery = "order by Address desc";
                    subquery1 = "order by Address desc";
                }


                else if (filter.order == "ascending/phone")
                {
                    subquery = "order by Phone asc";
                    subquery1 = "order by Phone asc";
                }
                else if (filter.order == "descending/phone")
                {
                    subquery = "order by Phone desc";
                    subquery1 = "order by Phone desc";
                }

                else if (filter.order == "ascending/email")
                {
                    subquery = "order by Email asc";
                    subquery1 = "order by Email asc";
                }
                else if (filter.order == "descending/email")
                {
                    subquery = "order by Email desc";
                    subquery1 = "order by Email desc";
                }


                else if (filter.order == "ascending/discount")
                {
                    subquery = "order by Discount asc";
                    subquery1 = "order by Discount asc";
                }
                else if (filter.order == "descending/discount")
                {
                    subquery = "order by Discount desc";
                    subquery1 = "order by Discount desc";
                }

                else if (filter.order == "ascending/totalprice")
                {
                    subquery = "order by TotalPrice asc";
                    subquery1 = "order by TotalPrice asc";
                }
                else if (filter.order == "descending/totalprice")
                {
                    subquery = "order by TotalPrice desc";
                    subquery1 = "order by TotalPrice desc";
                }
                else
                {
                    subquery = "Order BY ID desc";
                    subquery1 = "Order By Id desc";
                }

            }
            else
            {
                subquery = "Order BY ID desc";
                subquery1 = "Order By Id desc";
            }


            try
            {
                #region Naming Condition
                string NamingSql = "''";
                GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
                GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
                if (gs != null)
                {
                    NamingSql = gs.Value;
                }
                #endregion

                SQLQuery = string.Format(SQLQuery, filter.PageNO, filter.PageSize,DateFilter, NamingSql, subquery, subquery1, SearchText);
                using (SqlCommand cmd = GetSQLCommand(SQLQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ee)
            {
                return null;
            }
        }

        public DataTable DownloadFinishedJobreportsByFilter(FinishedJobFilter filter)
        {
            string DateFilter = "";
            string SearchText = "";

            if (filter.StartDate.HasValue && filter.StartDate.Value != new DateTime()
                && filter.EndDate.HasValue && filter.EndDate.Value != new DateTime())
            {
                string startDate = filter.StartDate.Value.SetZeroHour().ToString("yyyy-MM-dd HH:mm:ss");
                string endDate = filter.EndDate.Value.SetMaxHour().ToString("yyyy-MM-dd HH:mm:ss");

                DateFilter = string.Format(" and tic.CompletionDate between '{0}' and '{1}' ", startDate, endDate);
            }
            if (!string.IsNullOrWhiteSpace(filter.SearchText) && filter.SearchText != "undefined" && filter.SearchText != "null")
            {
                SearchText = string.Format(" and ([Customer Name] like '%{0}%' or [Booking Id] like '%{0}%' or [Ticket Id] like '%{0}%' or Address like '%{0}%' or Phone like '%{0}%'  or Email like '%{0}%' or Discount like '%{0}%' or [Total Price] like '%{0}%')", filter.SearchText);
            }

            string SQLQuery = @"declare @pagestart int
                                declare @pageend int
                                declare @pageno int
                                declare @pagesize int
                                set @pageno = {0}
                                set @pagesize = {1}

                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                select 
                                {3} as [Customer Name],
                                bk.BookingId as [Booking Id],
                                tic.Id as [Ticket Id],
                                dbo.MakeAddress(cus.Street,cus.StreetType, cus.Appartment,cus.ZipCode,cus.City,cus.[State]) as [Address],
                                dbo.PhoneNumFormat(cus.PrimaryPhone) as Phone,
                                cus.EmailAddress as Email,
                                inv.DiscountAmount as Discount,
                                cast(inv.TotalAmount as decimal(12,2)) as [Total Price]
                                into #JobsReport
                                from Ticket tic
                                left join Customer cus on  tic.CustomerId = cus.CustomerId
                                left join Booking bk on bk.BookingId = tic.BookingId
                                left join Invoice inv on inv.BookingId = tic.BookingId

                                where tic.TicketType ='Drop Off'
                                and (tic.Status = 'Completed' Or tic.Status = 'Closed' )
                                and tic.BookingId != ''
                                {2}

                                 select
                                 * from #JobsReport
                                 where [Ticket Id] not in(select top(@pagestart) [Ticket Id] from #JobsReport {4}) {6}
                                 {5} 
                                drop table #JobsReport
                                ";

            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(filter.order))
            {
                if (filter.order == "ascending/customername")
                {
                    subquery = "order by CustomerName asc";
                    subquery1 = "order by CustomerName asc";
                }
                else if (filter.order == "descending/customername")
                {
                    subquery = "order by CustomerName desc";
                    subquery1 = "order by CustomerName desc";
                }


                else if (filter.order == "ascending/bookingid")
                {
                    subquery = "order by BookingId asc";
                    subquery1 = "order by BookingId asc";
                }
                else if (filter.order == "descending/bookingid")
                {
                    subquery = "order by BookingId desc";
                    subquery1 = "order by BookingId desc";
                }

                ///////////
                else if (filter.order == "ascending/id")
                {
                    subquery = "order by ID asc";
                    subquery1 = "order by Id asc";
                }
                else if (filter.order == "descending/id")
                {
                    subquery = "order by ID desc";
                    subquery1 = "order by Id desc";
                }
                //////////////

                else if (filter.order == "ascending/address")
                {
                    subquery = "order by Address asc";
                    subquery1 = "order by Address asc";
                }
                else if (filter.order == "descending/address")
                {
                    subquery = "order by Address desc";
                    subquery1 = "order by Address desc";
                }


                else if (filter.order == "ascending/phone")
                {
                    subquery = "order by Phone asc";
                    subquery1 = "order by Phone asc";
                }
                else if (filter.order == "descending/phone")
                {
                    subquery = "order by Phone desc";
                    subquery1 = "order by Phone desc";
                }

                else if (filter.order == "ascending/email")
                {
                    subquery = "order by Email asc";
                    subquery1 = "order by Email asc";
                }
                else if (filter.order == "descending/email")
                {
                    subquery = "order by Email desc";
                    subquery1 = "order by Email desc";
                }


                else if (filter.order == "ascending/discount")
                {
                    subquery = "order by Discount asc";
                    subquery1 = "order by Discount asc";
                }
                else if (filter.order == "descending/discount")
                {
                    subquery = "order by Discount desc";
                    subquery1 = "order by Discount desc";
                }

                else if (filter.order == "ascending/totalprice")
                {
                    subquery = "order by TotalPrice asc";
                    subquery1 = "order by TotalPrice asc";
                }
                else if (filter.order == "descending/totalprice")
                {
                    subquery = "order by TotalPrice desc";
                    subquery1 = "order by TotalPrice desc";
                }
                else
                {
                    subquery = "Order BY ID desc";
                    subquery1 = "Order By Id desc";
                }

            }
            else
            {
                subquery = "Order BY [Ticket Id] desc";
                subquery1 = "Order By [Ticket Id] desc";
            }


            try
            {
                #region Naming Condition
                string NamingSql = "''";
                GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
                GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
                if (gs != null)
                {
                    NamingSql = gs.Value;
                }
                #endregion

                SQLQuery = string.Format(SQLQuery, filter.PageNO, filter.PageSize, DateFilter, NamingSql, subquery, subquery1, SearchText);
                using (SqlCommand cmd = GetSQLCommand(SQLQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ee)
            {
                return null;
            }
        }
        public DataSet GetPackageSummaryreportsByFilter(PackageSummaryFilter filter)
        {
            string DateFilter = "";
            if (filter.StartDate.HasValue && filter.StartDate.Value != new DateTime()
                && filter.EndDate.HasValue && filter.EndDate.Value != new DateTime())
            {
                string startDate = filter.StartDate.Value.ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss");
                string endDate = filter.EndDate.Value.ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss");

                DateFilter = string.Format(" and tic.CompletionDate between '{0}' and '{1}' ", startDate, endDate);
            }

            throw new NotImplementedException();
        }
    }
}
