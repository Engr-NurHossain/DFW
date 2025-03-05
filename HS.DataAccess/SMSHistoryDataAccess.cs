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
	public partial class SMSHistoryDataAccess
	{
        public DataSet GetSMSHistoryListByCompanyId(SMSHistoryFilter Filter)
        {
            string subquery = "";
            string orderquery = "";
            string orderquery1 = "";
            if (Filter.StartDate.HasValue && Filter.StartDate.Value != new DateTime() && Filter.EndDate.HasValue && Filter.EndDate.Value != new DateTime())
            {
                Filter.StartDate = Filter.StartDate.Value.SetClientZeroHourToUTC();
                Filter.EndDate = Filter.EndDate.Value.SetClientMaxHourToUTC();
                subquery = string.Format("and SH.SMSSentDate between '{0}' and '{1}'", Filter.StartDate, Filter.EndDate);
            }
            string searchquery = "";
            if (!string.IsNullOrWhiteSpace(Filter.SearchText))
            {
                searchquery += string.Format("and (Emp.FirstName +' '+ Emp.LastName like '%{0}%')", Filter.SearchText);
            }
            #region Order
            if (!string.IsNullOrWhiteSpace(Filter.Order))
            {
                if (Filter.Order == "ascending/createdby")
                {
                    orderquery = "order by #TempCre.FromName asc";
                    orderquery1 = "order by FromName asc";
                }
                else if (Filter.Order == "descending/createdby")
                {
                    orderquery = "order by #TempCre.FromName desc";
                    orderquery1 = "order by FromName desc";
                }
                else if (Filter.Order == "ascending/date")
                {
                    orderquery = "order by #TempCre.SMSSentDate asc";
                    orderquery1 = "order by SMSSentDate asc";
                }
                else if (Filter.Order == "descending/date")
                {
                    orderquery = "order by #TempCre.SMSSentDate desc";
                    orderquery1 = "order by SMSSentDate desc";
                }
                else if (Filter.Order == "ascending/from")
                {
                    orderquery = "order by #TempCre.FromMobileNo asc";
                    orderquery1 = "order by FromMobileNo asc";
                }
                else if (Filter.Order == "descending/from")
                {
                    orderquery = "order by #TempCre.FromMobileNo desc";
                    orderquery1 = "order by FromMobileNo desc";
                }
                else if (Filter.Order == "ascending/to")
                {
                    orderquery = "order by #TempCre.ToMobileNo asc";
                    orderquery1 = "order by ToMobileNo asc";
                }
                else if (Filter.Order == "descending/to")
                {
                    orderquery = "order by #TempCre.ToMobileNo desc";
                    orderquery1 = "order by ToMobileNo desc";
                }
                else
                {
                    orderquery = "order by #TempCre.SMSSentDate desc";
                    orderquery1 = "order by SH.SMSSentDate desc";
                }
            }
            else
            {
                orderquery = "order by #TempCre.SMSSentDate desc";
                orderquery1 = "order by SH.SMSSentDate desc";
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
                                SH.Id
                                ,SH.ToMobileNo
                                ,SH.FromMobileNo
                                ,SH.SMSSentDate
                                ,Emp.FirstName +' '+ Emp.LastName as FromName
		                         into #TempSMS

								 from SMSHistory SH
                                 left join Employee Emp on Emp.UserId = SH.CreatedBy
                                 where SH.CompanyId = '{2}'
                                 {3}
                                 {4}

														SELECT TOP (@pagesize) #TempCre.* into #TestTable
														FROM #TempSMS #TempCre
														where Id NOT IN(Select TOP (@pagestart) Id from #TempSMS #TempCre {5})
														{6}
														select  count(Id) as [TotalCount] from #TempSMS

														select * from #TestTable

														DROP TABLE #TempSMS
														DROP TABLE #TestTable";
            try
            {
                sqlQuery = string.Format(sqlQuery, Filter.PageNo, Filter.PageSize, Filter.CompanyId, searchquery, subquery, orderquery, orderquery);
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
        public DataTable DownloadSMSHistoryListByCompanyId(SMSHistoryFilter Filter)
        {
            string subquery = "";
            if (Filter.StartDate.HasValue && Filter.StartDate.Value != new DateTime() && Filter.EndDate.HasValue && Filter.EndDate.Value != new DateTime())
            {
                Filter.StartDate = Filter.StartDate.Value.SetClientZeroHourToUTC();
                Filter.EndDate = Filter.EndDate.Value.SetClientMaxHourToUTC();
                subquery = string.Format("and SH.SMSSentDate between '{0}' and '{1}'", Filter.StartDate, Filter.EndDate);
            }
            string searchquery = "";
            if (!string.IsNullOrWhiteSpace(Filter.SearchText))
            {
                searchquery += string.Format("and (Emp.FirstName +' '+ Emp.LastName like '%{0}%')", Filter.SearchText);
            }

            string sqlQuery = @"select
                                Emp.FirstName +' '+ Emp.LastName as [Sender Name]
                                ,FORMAT(SH.SMSSentDate,'MM/dd/yyyy') as [Sent Date]
                                ,dbo.PhoneNumFormat(SH.FromMobileNo) as [Sent From]
                                ,dbo.PhoneNumFormat(SH.ToMobileNo) as [Sent To]

                                from SMSHistory SH
                                left join Employee Emp on Emp.UserId = SH.CreatedBy
                                where SH.CompanyId ='{0}'
                                {1}
                                {2}
                                order by SH.SMSSentDate desc";
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
