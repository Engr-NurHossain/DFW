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
	public partial class CustomerCreditCheckDataAccess
	{
        public DataTable GetAllCustomerCreditCheckByCustomerId(Guid CustomerId)
        {
            string sqlQuery = @"select _cst.*,emp.FirstName+' '+emp.LastName as CreatedByval, (select Grade from CreditScoreGrade where MinScore<= Convert(int,_cst.Score) and MaxScore>= Convert(int,_cst.Score)) as Grade from CustomerCreditCheck _cst
                                left join Employee emp on emp.UserId = _cst.CreatedBy
                                where _cst.CustomerId = '{0}' order by _cst.Id desc";
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId);
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
        public DataSet GetCreditHistoryListByCompanyId(CreditHistoryFilter Filter)
        {
            string subquery = "";
            string orderquery = "";
            string orderquery1 = "";
            if (Filter.StartDate.HasValue && Filter.StartDate.Value != new DateTime() && Filter.EndDate.HasValue && Filter.EndDate.Value != new DateTime())
            {
                Filter.StartDate = Filter.StartDate.Value.SetClientZeroHourToUTC();
                Filter.EndDate = Filter.EndDate.Value.SetClientMaxHourToUTC();
                subquery = string.Format("and CCC.CreditCheckDate between '{0}' and '{1}'", Filter.StartDate, Filter.EndDate);
            }
            string searchquery = "";
            if (!string.IsNullOrWhiteSpace(Filter.SearchText))
            {
                searchquery += string.Format("and (Cus.FirstName like '%{0}%' or Cus.LastName like '%{0}%' or Cus.FirstName + ' ' + Cus.LastName like '%{0}%' or Cus.BusinessName like '%{0}%')", Filter.SearchText);
            }
            #region Order
            if (!string.IsNullOrWhiteSpace(Filter.Order))
            {
                if (Filter.Order == "ascending/customer")
                {
                    orderquery = "order by #TempCre.[CustomerName] asc";
                    orderquery1 = "order by [CustomerName] asc";
                }
                else if (Filter.Order == "descending/customer")
                {
                    orderquery = "order by #TempCre.[CustomerName] desc";
                    orderquery1 = "order by [CustomerName] desc";
                }
                else if (Filter.Order == "ascending/address")
                {
                    orderquery = "order by #TempCre.Address asc";
                    orderquery1 = "order by Address asc";
                }
                else if (Filter.Order == "descending/address")
                {
                    orderquery = "order by #TempCre.Address desc";
                    orderquery1 = "order by Address desc";
                }
                else if (Filter.Order == "ascending/date")
                {
                    orderquery = "order by #TempCre.CreditCheckDate asc";
                    orderquery1 = "order by CreditCheckDate asc";
                }
                else if (Filter.Order == "descending/date")
                {
                    orderquery = "order by #TempCre.CreditCheckDate desc";
                    orderquery1 = "order by CreditCheckDate desc";
                }
                else if (Filter.Order == "ascending/score")
                {
                    orderquery = "order by #TempCre.Score asc";
                    orderquery1 = "order by Score asc";
                }
                else if (Filter.Order == "descending/score")
                {
                    orderquery = "order by #TempCre.Score desc";
                    orderquery1 = "order by Score desc";
                }
                else if (Filter.Order == "ascending/bureau")
                {
                    orderquery = "order by #TempCre.CreditBureau asc";
                    orderquery1 = "order by CreditBureau asc";
                }
                else if (Filter.Order == "descending/bureau")
                {
                    orderquery = "order by #TempCre.CreditBureau desc";
                    orderquery1 = "order by CreditBureau desc";
                }
                else
                {
                    orderquery = "order by #TempCre.CreditCheckDate desc";
                    orderquery1 = "order by CCC.CreditCheckDate desc";
                }
            }
            else
            {
                orderquery = "order by #TempCre.CreditCheckDate desc";
                orderquery1 = "order by CCC.CreditCheckDate desc";
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
                                 CCC.Id
                                ,Cus.Id as CusIntId
                                ,CASE 
	                                WHEN (Cus.BusinessName = '' or Cus.BusinessName IS NULL) THEN Cus.FirstName +' '+Cus.LastName
	                                ELSE  Cus.BusinessName
	                                END as CustomerName
                                ,dbo.MakeAddress(Cus.Street,Cus.StreetType,Cus.Appartment,Cus.City,Cus.[State],Cus.ZipCode) as [Address]
                                ,CCC.CreditBureau
                                ,CCC.CreditCheckDate
                                ,CCC.Score
		                         into #TempCredit

								 from CustomerCreditCheck CCC
                                 left join Customer Cus on Cus.CustomerId = CCC.CustomerId
                                 where CCC.CompanyId = '{2}'
                                 and Cus.Id is not null 
                                 {3}
                                 {4}

														SELECT TOP (@pagesize) #TempCre.* into #TestTable
														FROM #TempCredit #TempCre
														where Id NOT IN(Select TOP (@pagestart) Id from #TempCredit #TempCre {5})
														{6}
														select  count(Id) as [TotalCount] from #TempCredit

														select * from #TestTable

														DROP TABLE #TempCredit
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

        public DataTable DownloadCreditHistoryListByCompanyId(CreditHistoryFilter Filter)
        {
            string subquery = "";
            if (Filter.StartDate.HasValue && Filter.StartDate.Value != new DateTime() && Filter.EndDate.HasValue && Filter.EndDate.Value != new DateTime())
            {
                Filter.StartDate = Filter.StartDate.Value.SetClientZeroHourToUTC();
                Filter.EndDate = Filter.EndDate.Value.SetClientMaxHourToUTC();
                subquery = string.Format("and CCC.CreditCheckDate between '{0}' and '{1}'", Filter.StartDate, Filter.EndDate);
            }
            string searchquery = "";
            if (!string.IsNullOrWhiteSpace(Filter.SearchText))
            {
                searchquery += string.Format("and (Cus.FirstName like '%{0}%' or Cus.LastName like '%{0}%' or Cus.FirstName + ' ' + Cus.LastName like '%{0}%' or Cus.BusinessName like '%{0}%')", Filter.SearchText);
            }

            string sqlQuery = @"select
                                Cus.Id
                                ,CASE 
	                                WHEN (Cus.BusinessName = '' or Cus.BusinessName IS NULL) THEN Cus.FirstName +' '+Cus.LastName
	                                ELSE  Cus.BusinessName
	                                END as [Customer]
                                ,dbo.MakeAddress(Cus.Street,Cus.StreetType,Cus.Appartment,Cus.City,Cus.[State],Cus.ZipCode) as [Address]
                                ,FORMAT(CCC.CreditCheckDate,'MM/dd/yyyy') as [Date]
                                ,CCC.Score
                                ,CCC.CreditBureau as [Bureau]
                                from CustomerCreditCheck CCC
                                left join Customer Cus on Cus.CustomerId = CCC.CustomerId
                                where CCC.CompanyId ='{0}'
                                and Cus.Id is not null
                                {1}
                                {2}
                                order by CCC.CreditCheckDate desc";
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
