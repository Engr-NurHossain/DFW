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
	public partial class CustomerViewDataAccess
	{
        public DataTable GetCustomerViewListByCompanyIdandCustomerId(Guid companyID,bool recent,string UserName)
        {
            string sqlQuery = @"select TOP {1} C.Id as CustomerViewid, C.FirstName+ ' '+C.LastName as CustomerViewName,convert(varchar(25), CV.LastVistited, 120) LastVistited,
                                C.BusinessName as CustomerViewBussiness
                                into #CustomerView from CustomerView CV
                                left join Customer C
                                on cv.CustomerId = c.CustomerId
                                where CV.CompanyId = '{0}'
                                and CV.LastVisitedBy ='{2}'
                                and (CV.IsLead = 'false' or CV.IsLead is null)
                                order by CV.LastVistited desc

                                select CustomerViewid,CustomerViewName, CustomerViewBussiness,MAX(LastVistited) LastVistited into #CustomerView2 from #CustomerView
                                group by CustomerViewid,CustomerViewName, CustomerViewBussiness
                              
								select * from #CustomerView2 where CustomerViewid IS NOT NULL    order by LastVistited Desc
                                drop table #CustomerView
								drop table #CustomerView2";
            try
            {
                var NoOfItem = 5;
                if (!recent)
                {
                    NoOfItem = 10;
                }
                sqlQuery = string.Format(sqlQuery, companyID, NoOfItem,UserName);
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
        public DataSet GetCustomerTimestampByCustomerId(Guid customerId, int PageNo, int PageSize)
        {
            string sqlQuery = @"DECLARE @pagestart int
	                                DECLARE @pageend int
	                                DECLARE @pageno int
	                                DECLARE @pagesize int

                                    SET @pageno = {0} --default 1
									SET @pagesize = {1} --default 10
                                    SET @pagestart=(@pageno-1)* @pagesize 
                                    SET @pageend = @pagesize

                                    select
									Id
									,LastVisitedBy
									,LastVistited

									into #TempData

									from CustomerView
                                    where CustomerId = '{2}'

									SELECT TOP (@pagesize) #ed.*
                                                                FROM #TempData #ed
                                                                where Id NOT IN(Select TOP (@pagestart) Id from #TempData Order by Id desc)
                                                                Order by Id desc
                                                                select count(Id) as [TotalCount] from #TempData
                                                                DROP TABLE #TempData";
            try
            {
                sqlQuery = string.Format(sqlQuery, PageNo, PageSize, customerId);
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
    }	
}
