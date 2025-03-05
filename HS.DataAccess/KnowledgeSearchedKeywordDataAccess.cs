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
	public partial class KnowledgeSearchedKeywordDataAccess
	{
        public DataSet GetKnowledgebaseSearchList(Guid CompanyId, DateTime? start, DateTime? end, string searchtext, int pageno, int pagesize, string order)
        {
            string sqlQuery = @"DECLARE @pagestart int
	                            DECLARE @pageend int
	                            DECLARE @pageno int
	                            DECLARE @pagesize int


                                SET @pageno = {0}
							    SET @pagesize = {1}
                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

								select distinct kw.Keyword
                                ,(select count(*) from KnowledgeSearchedKeyword where keyword = kw.keyword {5}) as [Count]

								into #temptable 
								from KnowledgeSearchedKeyword kw
								where kw.Id > 0
                                {3}
								{4}
					            select top(@pagesize) * from #temptable
                                where Keyword not in (Select TOP (@pagestart)  Keyword from #temptable #ft {2})
                                {2}
                                select COUNT(*) Total from #temptable 

								drop table #temptable";

            string sqldate = "";
            string sqldate2 = "";
            string sqlorderby = " order by Keyword asc";
            string sqlsearch = "";

            if (start != new DateTime() && end != new DateTime())
            {
                sqldate = string.Format(" and kw.SearchedDate between '{0}' and '{1}'", start.Value.ToString("yyyy/MM/dd HH:mm"), end.Value.ToString("yyyy/MM/dd HH:mm"));
                sqldate2 = string.Format(" and SearchedDate between '{0}' and '{1}'", start.Value.ToString("yyyy/MM/dd HH:mm"), end.Value.ToString("yyyy/MM/dd HH:mm"));
            }
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                sqlsearch = string.Format(" and kw.Keyword Like '%{0}%'", searchtext);
            }
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "descending/phrase")
                {
                    sqlorderby = " order by Keyword desc";
                }
                else if (order == "ascending/count")
                {
                    sqlorderby = " order by [Count] asc , Keyword asc";
                }
                else if (order == "descending/count")
                {
                    sqlorderby = " order by [Count] desc , Keyword asc";
                }
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, pageno, pagesize, sqlorderby, sqldate, sqlsearch, sqldate2);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    return GetDataSet(cmd);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetKnowledgebaseSearchListForDownload(Guid CompanyId, DateTime? start, DateTime? end, string searchtext, int pageno, int pagesize, string order)
        {
            string sqlQuery = @"
								select distinct kw.Keyword as [Search Phrase]
                                ,(select count(*) from KnowledgeSearchedKeyword where keyword = kw.keyword {5}) as [Count]

								into #temptable 
								from KnowledgeSearchedKeyword kw
								where kw.Id > 0
                                {3}
								{4}
					            select * from #temptable
                                {2}
								drop table #temptable";

            string sqldate = "";
            string sqldate2 = "";
            string sqlorderby = " order by [Search Phrase] asc";
            string sqlsearch = "";

            if (start != new DateTime() && end != new DateTime())
            {
                sqldate = string.Format(" and kw.SearchedDate between '{0}' and '{1}'", start.Value.ToString("yyyy/MM/dd HH:mm"), end.Value.ToString("yyyy/MM/dd HH:mm"));
                sqldate2 = string.Format(" and SearchedDate between '{0}' and '{1}'", start.Value.ToString("yyyy/MM/dd HH:mm"), end.Value.ToString("yyyy/MM/dd HH:mm"));
            }
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                sqlsearch = string.Format(" and kw.Keyword Like '%{0}%'", searchtext);
            }
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "descending/phrase")
                {
                    sqlorderby = " order by [Search Phrase] desc";
                }
                else if (order == "ascending/count")
                {
                    sqlorderby = " order by [Count] asc , [Search Phrase] asc";
                }
                else if (order == "descending/count")
                {
                    sqlorderby = " order by [Count] desc , [Search Phrase] asc";
                }
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, pageno, pagesize, sqlorderby, sqldate, sqlsearch, sqldate2);
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

        public DataSet GetKnowledgebaseSearchedHistoryList(DateTime start, DateTime end, string searchtext, string Order)
        {
            string sqlQuery = @"select kw.*, emp.FirstName +' '+ emp.LastName as SearchedByName 
								into #temptable 
								from KnowledgeSearchedKeyword kw
								left join Employee emp on emp.UserId = kw.SearchedBy
								where kw.Id > 0
                                {0}
								{1}
					            select * from #temptable
                                {2}
                                select COUNT(*) Total from #temptable 

								drop table #temptable";

            string sqldate = "";
            string sqlorderby = " order by SearchedDate desc";
            string sqlsearch = "";

            if (start != new DateTime() && end != new DateTime())
            {
                sqldate = string.Format(" and kw.SearchedDate between '{0}' and '{1}'", start.ToString("yyyy/MM/dd HH:mm"), end.ToString("yyyy/MM/dd HH:mm"));
            }
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                sqlsearch = string.Format(" and kw.Keyword = '{0}'", searchtext);
            }
            if (!string.IsNullOrWhiteSpace(Order))
            {
                if (Order == "descending/phrase")
                {
                    sqlorderby = " order by Keyword desc";
                }
                else if (Order == "ascending/count")
                {
                    sqlorderby = " order by [Count] asc ";
                }
                else if (Order == "descending/count")
                {
                    sqlorderby = " order by [Count] desc";
                }
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, sqlsearch, sqldate, sqlorderby);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    return GetDataSet(cmd);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }	
}