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
	public partial class ResturantOrderDataAccess
	{
        public ResturantOrderDataAccess(string ConStr) : base(ConStr) { }
        public DataTable GetRestaurentOrderList(Guid comid, int pageNo, int pageSize, string searchText, string order, string startdate, string enddate, Guid customerid, bool filter, string ordertype, string orderstatus)
        {
            string searchquery = "";
            string subquery = "";
            string subquery1 = "";
            string datequery = "";
            string customerQuery = "";
            string cancelQuery = "";
            string cancelQuery1 = "";
            if (!string.IsNullOrWhiteSpace(ordertype))
            {
                ordertype = string.Format("and ro.OrderType = '{0}'", ordertype);
            }
            if (!string.IsNullOrWhiteSpace(orderstatus))
            {
                if(orderstatus.ToLower() == "progress")
                {
                    if (!string.IsNullOrWhiteSpace(ordertype) && ordertype.ToLower() == "pickup")
                    {
                        orderstatus = string.Format("and ro.[Status] in ('Pending','Accepted','Readypick')", orderstatus);
                    }
                    else
                    {
                        orderstatus = string.Format("and ro.[Status] in ('Pending','Accepted','Readydeliver')", orderstatus);
                    }
                }
                else
                {
                    orderstatus = string.Format("and ro.[Status] in ('{0}')", orderstatus);
                }
            }
            if (customerid != new Guid())
            {
                customerQuery = string.Format("and ro.CustomerId = '{0}'", customerid);
            }
            if(!string.IsNullOrWhiteSpace(startdate) && !string.IsNullOrWhiteSpace(enddate) && startdate != "01/01/0001" && enddate != "01/01/0001")
            {
                startdate = Convert.ToDateTime(startdate).ToString("yyyy-MM-dd 00:00:00.000");
                enddate = Convert.ToDateTime(enddate).ToString("yyyy-MM-dd 23:59:59.999");
                datequery = string.Format("and ro.CreatedDate between (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '{0}')) and (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '{1}'))", startdate, enddate);
            }
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                searchquery = string.Format("and (ro.Location like '%{0}%' or ro.Status like '%{0}%' or ro.ContactNo like '%{0}%')", searchText);
            }
            if(filter != null && filter == true)
            {
                cancelQuery = "where IsDeleted = 1";
                cancelQuery1 = "and IsDeleted = 1";
            }
            else
            {
                cancelQuery = "where IsDeleted != 1";
                cancelQuery1 = "and IsDeleted != 1";
            }
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/orderid")
                {
                    subquery = "order by #ROFilterdata.[Id] asc";
                    subquery1 = "order by [Id] asc";
                }
                else if (order == "descending/orderid")
                {
                    subquery = "order by  #ROFilterdata.[Id] desc";
                    subquery1 = "order by [Id] desc";
                }
                else if (order == "descending/ordercustomer")
                {
                    subquery = "order by #ROFilterdata.[CustomerId] desc";
                    subquery1 = "order by [CustomerId] desc";
                }
                else if (order == "ascending/ordercustomer")
                {
                    subquery = "order by #ROFilterdata.[CustomerId] asc";
                    subquery1 = "order by [CustomerId] asc";
                }
                //else if (order == "ascending/menuitemcount")
                //{
                //    subquery = "order by #MenuData.[NumberOfItems] asc";
                //    subquery1 = "order by [NumberOfItems] asc";
                //}
                //else if (order == "descending/menuitemcount")
                //{
                //    subquery = "order by #MenuData.[NumberOfItems] desc";
                //    subquery1 = "order by [NumberOfItems] desc";
                //}
                else if (order == "ascending/orderlocation")
                {
                    subquery = "order by #ROFilterdata.[Location] asc";
                    subquery1 = "order by [Location] asc";
                }
                else if (order == "descending/orderlocation")
                {
                    subquery = "order by #ROFilterdata.[Location] desc";
                    subquery1 = "order by [Location] desc";
                }
                else if (order == "ascending/ordercontactno")
                {
                    subquery = "order by #ROFilterdata.[ContactNo] asc";
                    subquery1 = "order by [ContactNo] asc";
                }
                else if (order == "descending/ordercontactno")
                {
                    subquery = "order by #ROFilterdata.[ContactNo] desc";
                    subquery1 = "order by [ContactNo] desc";
                }
                else if (order == "ascending/ordertype")
                {
                    subquery = "order by #ROFilterdata.[OrderType] asc";
                    subquery1 = "order by [OrderType] asc";
                }
                else if (order == "descending/ordertype")
                {
                    subquery = "order by #ROFilterdata.[OrderType] desc";
                    subquery1 = "order by [OrderType] desc";
                }
                else if (order == "ascending/amount")
                {
                    subquery = "order by #ROFilterdata.[Amount] asc";
                    subquery1 = "order by [Amount] asc";
                }
                else if (order == "descending/amount")
                {
                    subquery = "order by #ROFilterdata.[Amount] desc";
                    subquery1 = "order by [Amount] desc";
                }
                else if (order == "ascending/status")
                {
                    subquery = "order by #ROFilterdata.[Status] asc";
                    subquery1 = "order by [Status] asc";
                }
                else if (order == "descending/status")
                {
                    subquery = "order by #ROFilterdata.[Status] desc";
                    subquery1 = "order by [Status] desc";
                }
                else
                {
                    subquery = "order by #ROFilterdata.[Id] desc";
                    subquery1 = "order by Id desc";
                }

            }
            else
            {
                subquery = "order by #ROFilterdata.[Id] desc";
                subquery1 = "order by Id desc";
            }
            #endregion
            string sqlQuery = @"declare @pagestart int
                                declare @pageend int
                                set @pagestart=({1}-1)* {2} 
                                set @pageend = {2}
select distinct ro.Id, ro.OrderId, ro.CustomerId, ro.Location, ro.OrderType, ro.Amount, ro.Quantity
,ro.PickUpTime,ro.CreatedDate, ro.LastUpdatedDate, ro.CreatedBy, ro.LastUpdatedBy, ro.CompanyId
,ro.ContactNo, ro.SpecialInstruction, ISNULL(lk.DisplayText, 'Pending') as [Status], ro.OrderDate, cus.FirstName + ' ' + cus.LastName as CustomerName
,cus.Street as CustomerStreet, cus.City as CustomerCity, cus.State as CustomerState, cus.ZipCode as CustomerZip, ro.IsViewed, cus.Id as CustomerIntId
,ro.TaxAmount, ro.Notes, ro.AcceptDate, ro.RejectedDate, ro.RejectedReason, lkpay.DisplayText as PaymentMethod, isnull(ro.IsDeleted, 0) as IsDeleted
,ro.DiscountValue, ro.DiscountCode
into #OrdersData from ResturantOrder ro
left join [lookup] lk on ro.[Status] = lk.DataValue and (lk.DataKey = 'RestaurantOrderDeliveryStatus' or lk.DataKey = 'RestaurantOrderPickupStatus') and lk.CompanyId = '{0}'
left join [Lookup] lkpay on lkpay.DataValue = ro.PaymentMethod and lkpay.DataKey = 'IeateryPaymentOption' and lkpay.CompanyId = '{0}'
left join Customer cus on cus.CustomerId = ro.CustomerId
                                where ro.CompanyId = '{0}'
                                {3}
                                {6}
                                {7}
                                {10}
                                {11}
                              select * into #ROFilterdata from #OrdersData
                               SELECT TOP ({2})
                                  *
                                FROM #ROFilterdata
                                where Id NOT IN(Select TOP (@pagestart)  Id from #OrdersData {5})
                                {5}
                                drop table #OrdersData
								drop table #ROFilterdata";


            try
            {
                sqlQuery = string.Format(sqlQuery, comid, pageNo, pageSize, searchquery, subquery, subquery1, datequery, customerQuery, cancelQuery, cancelQuery1, ordertype, orderstatus);
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

        public DataTable GetRestaurantOrderSummeryByCompanyId(Guid comid, string startdate, string enddate, string ordertype, string orderstatus)
        {
            string datequery = "";
            if (!string.IsNullOrWhiteSpace(startdate) && !string.IsNullOrWhiteSpace(enddate) && startdate != "01/01/0001" && enddate != "01/01/0001")
            {
                startdate = Convert.ToDateTime(startdate).ToString("yyyy-MM-dd 00:00:00.000");
                enddate = Convert.ToDateTime(enddate).ToString("yyyy-MM-dd 23:59:59.999");
                datequery = string.Format("and ro.CreatedDate between (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '{0}')) and (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '{1}'))", startdate, enddate);
            }
            if (!string.IsNullOrWhiteSpace(ordertype))
            {
                ordertype = string.Format("and ro.OrderType = '{0}'", ordertype);
            }
            if (!string.IsNullOrWhiteSpace(orderstatus))
            {
                orderstatus = string.Format("and ro.[Status] in ('{0}')", orderstatus);
            }
            string sqlQuery = @"select ro.OrderType,
                                (select COUNT(*) from ResturantOrderDetail rod where ro.OrderId = rod.OrderId) as QuantityCount,
                                (select COUNT(*) from ResturantOrder _ro where _ro.OrderId = ro.OrderId and ([Status] = 'Pending' or [Status] = 'Accepted' or [Status] = 'Readypick' or [Status] = 'Readydeliver')) as InProgressCount,
                                (select COUNT(*) from ResturantOrder _ro where _ro.OrderId = ro.OrderId and [Status] = 'Cancelled') as CancellationCount,
                                (select COUNT(*) from ResturantOrder _ro where _ro.OrderId = ro.OrderId and [Status] = 'Rejected') as RejectedCount,
                                (select COUNT(*) from ResturantOrder _ro where _ro.OrderId = ro.OrderId and ([Status] = 'Pickedup' or [Status] = 'Delivered')) as CompletedCount,
                                ISNULL((select ro.Amount from ResturantOrder _ro where _ro.OrderId = ro.OrderId and ([Status] = 'Pickedup' or [Status] = 'Delivered')), 0) as AverageOrder,
                                ISNULL((select ro.Amount from ResturantOrder _ro where _ro.OrderId = ro.OrderId and ([Status] = 'Pickedup' or [Status] = 'Delivered')), 0) as TotalRev
                                into #TempRestaurant from ResturantOrder ro
                                where ro.CompanyId = '{0}'
                                {1}
                                {2}
                                {3}

                                select * from #TempRestaurant where (InProgressCount > 0 or CancellationCount > 0 or RejectedCount > 0 or CompletedCount > 0)

                                drop table #TempRestaurant";
            try
            {
                sqlQuery = string.Format(sqlQuery, comid, datequery, ordertype, orderstatus);
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

        public DataTable GetRestaurantOrderSummeryByCompanyIdAndCustomerId(Guid comid, Guid cusid)
        {
            string sqlQuery = @"select COUNT(*) as QuantityCount, SUM(ro.Amount) as TotalRev
                                
                                from ResturantOrder ro
                                left join Customer cus on cus.CustomerId = ro.CustomerId 
                                where ro.CompanyId = '{0}'
                                and ro.CustomerId = '{1}'";


            try
            {
                sqlQuery = string.Format(sqlQuery, comid, cusid);
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

        public DataTable GetAllResturantSystemSettingByCompanyId(Guid comid)
        {
            string sqlQuery = @"select rss.*, (select contact.FirstName + ' ' + contact.LastName from SiteContact contact where contact.CompanyId = rss.CompanyId and contact.ContactId = iif(rss.PrimaryContact != '-1' and rss.PrimaryContact != '', rss.PrimaryContact, '00000000-0000-0000-0000-000000000000')) as PrimaryContactVal from ResturantSystemSetting rss
                                where rss.CompanyId = '{0}'";


            try
            {
                sqlQuery = string.Format(sqlQuery, comid);
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

        public DataTable GetAllRestaurantCoupons(Guid comid)
        {
            string sqlQuery = @"select rc.*, (select COUNT(*) from ResturantOrder ro where ro.CompanyId = rc.CompanyId and ro.DiscountCode = rc.CouponCode) as CountRedeem
                                from RestaurantCoupons rc
                                where rc.CompanyId = '{0}'";


            try
            {
                sqlQuery = string.Format(sqlQuery, comid);
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
