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
	public partial class KnowledgebaseAccessedHistoryDataAccess
	{
        public DataSet GetKnowledgebaseAccessedHistoryList(Guid CompanyId, DateTime? start, DateTime? end, string searchtext, int pageno, int pagesize, string order)
        {
            string sqlQuery = @"DECLARE @pagestart int
	                            DECLARE @pageend int
	                            DECLARE @pageno int
	                            DECLARE @pagesize int


                                SET @pageno = {0}
							    SET @pagesize = {1}
                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize

								select kn.Id,
								kn.Title,
                                kn.IsDocumentLibrary,
                                kn.IsDeleted,
								(select count(*) from KnowledgebaseAccessedHistory where KnowledgebaseId = kn.Id {3}) as [Visited]

								into #temptable 
								from Knowledgebase kn
								where kn.Id in (select KnowledgebaseId from KnowledgebaseAccessedHistory where KnowledgebaseId > 0 {3})
								{4}
					            select top(@pagesize) * from #temptable
                                where Id not in (Select TOP (@pagestart)  Id from #temptable #ft {2})
                                {2}
                                select COUNT(*) Total from #temptable 

								drop table #temptable";

            string sqldate = "";
            string sqldate2 = "";
            string sqlorderby = " order by Title asc";
            string sqlsearch = "";

            if (start != new DateTime() && end != new DateTime())
            {
                sqldate = string.Format(" and VisitedDate between '{0}' and '{1}'", start.Value.ToString("yyyy/MM/dd HH:mm"), end.Value.ToString("yyyy/MM/dd HH:mm"));
            }
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                sqlsearch = string.Format(" and Title Like '%{0}%'", searchtext);
            }
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "descending/phrase")
                {
                    sqlorderby = " order by Title desc";
                }
                else if (order == "ascending/count")
                {
                    sqlorderby = " order by [Visited] asc, Title asc ";
                }
                else if (order == "descending/count")
                {
                    sqlorderby = " order by [Visited] desc, Title asc";
                }
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, pageno, pagesize, sqlorderby, sqldate, sqlsearch);
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
        public DataTable GetKnowledgebaseAccessedHistoryListForDownload(Guid CompanyId, DateTime? start, DateTime? end, string searchtext, int pageno, int pagesize, string order)
        {
            string sqlQuery = @"
								select 
								kn.Title,
								(select count(*) from KnowledgebaseAccessedHistory where KnowledgebaseId = kn.Id {3}) as [Visited]

								into #temptable 
								from Knowledgebase kn
								where kn.Id in (select KnowledgebaseId from KnowledgebaseAccessedHistory where KnowledgebaseId > 0 {3})
								{4}
					            select * from #temptable

                                {2}

								drop table #temptable";

            string sqldate = "";
            string sqlorderby = " order by Title asc";
            string sqlsearch = "";

            if (start != new DateTime() && end != new DateTime())
            {
                sqldate = string.Format(" and VisitedDate between '{0}' and '{1}'", start.Value.ToString("yyyy/MM/dd HH:mm"), end.Value.ToString("yyyy/MM/dd HH:mm"));
            }
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                sqlsearch = string.Format(" and Title Like '%{0}%'", searchtext);
            }
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "descending/phrase")
                {
                    sqlorderby = " order by Title desc";
                }
                else if (order == "ascending/count")
                {
                    sqlorderby = " order by [Visited] asc, Title asc ";
                }
                else if (order == "descending/count")
                {
                    sqlorderby = " order by [Visited] desc, Title asc";
                }
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, pageno, pagesize, sqlorderby, sqldate, sqlsearch);
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

        public DataSet GetKnowledgebaseAccessedHistoryList(DateTime start, DateTime end, string searchtext, int Id)
        {
            string sqlQuery = @"select kah.VisitedDate, emp.FirstName +' '+ emp.LastName as [VisitedByName] 
								into #temptable 
								from KnowledgebaseAccessedHistory kah
                                left join Employee emp on emp.UserId = kah.VisitedBy
								where kah.KnowledgebaseId ='{0}'
								{1}
					            select * from #temptable
                                {2}
                                select COUNT(*) Total from #temptable 

								drop table #temptable";

            string sqldate = "";
            string sqlorderby = " order by VisitedDate desc";

            if (start != new DateTime() && end != new DateTime())
            {
                sqldate = string.Format(" and kah.VisitedDate between '{0}' and '{1}'", start.ToString("yyyy/MM/dd HH:mm"), end.ToString("yyyy/MM/dd HH:mm"));
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, Id, sqldate, sqlorderby);
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