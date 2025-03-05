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
using System.Web;

namespace HS.DataAccess
{
	public partial class KnowledgebaseWeblinkDataAccess
	{
        public bool DeleteKnowledgebaseWeblinkByKnowledgebaseId(int id)
        {
            string SqlQuery = @"delete from KnowledgebaseWeblink where KnowledgebaseId = {0}";
            try
            {
                SqlQuery = string.Format(SqlQuery, id);
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
        public List<KnowledgebaseWeblink> GetWeblinksListByKnowledgebaseId(int id)
        {
            string sqlQuery = @"select cw.*, k.Title as RelatedArticalTitle 
                                from KnowledgebaseWeblink cw 
                                left join Knowledgebase k on cw.link like '%='+cast(k.Id as nvarchar(50))
                                where cw.KnowledgebaseId = {0}";
            try
            {
                List<KnowledgebaseWeblink> list = new List<KnowledgebaseWeblink>();
                sqlQuery = string.Format(sqlQuery, id);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    //return GetList(cmd, -1);
                    DataSet ds = GetDataSet(cmd);
                    list = (from DataRow dr in ds.Tables[0].Rows
                            select new KnowledgebaseWeblink()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                KnowledgebaseId = dr["KnowledgebaseId"] != DBNull.Value ? Convert.ToInt32(dr["KnowledgebaseId"]) : 0,
                                Title = dr["Title"].ToString(),
                                RelatedArticalTitle = dr["RelatedArticalTitle"].ToString(),
                                Link = dr["Link"].ToString(),
                                IsRelated = dr["IsRelated"] != DBNull.Value ? Convert.ToBoolean(dr["IsRelated"]) : false,
                            }).ToList();
                }
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetDocumentLibraryListWithFile()
        {
            string sqlQuery = @"select 
                                kn.Id,
                                kn.Title,
                                (select count(*) from EstimateImage where InvoiceId = kn.Id and IsDocument = 1) as FileCount
                                into  #DocumentTable
                                from DocumentLibrary kn
                                Where kn.IsDeleted = 0 
                                select * from  #DocumentTable Where FileCount > 0 
                                drop table #DocumentTable";
            sqlQuery = string.Format(sqlQuery);
            try
            {
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
        public DataTable GetFlagUserCommentForKnowledgebase(int KnowledgebaseId, Guid uid, bool IsDocument)
        {
            string DocumentQuery = "";
            if (IsDocument)
            {
                DocumentQuery = string.Format("and kn.IsDocument = 1");
            }
            else
            {
                DocumentQuery = string.Format("and kn.IsDocument = 0");
            }
            string sqlQuery = @"select kn.Comment,CONCAT(e.FirstName,' ',e.LastName) Name,kn.CreatedDate

                                from KnowledgeBaseFlagUser kn
                                left join Employee e on e.UserId=kn.CreatedBy
                                Where KnowledgebaseId={0} AND kn.Comment!='' AND kn.CreatedBy='{1}' {2} Order by kn.Id desc";
            sqlQuery = string.Format(sqlQuery, KnowledgebaseId, uid, DocumentQuery);
            try
            {
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
        public bool UnFlagUserForKnowledgebase(int KnowledgebaseId, Guid uid)
        {
            string sqlQuery = @"UPDATE KnowledgeBaseFlagUser SET IsFlag=0
                                Where KnowledgebaseId={0}";
            sqlQuery = string.Format(sqlQuery, KnowledgebaseId, uid);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public DataTable GetFlagUserCommentForKnowledgebase(int KnowledgebaseId, bool IsDocument)
        {
            string DocumentQuery = "";
            if (IsDocument)
            {
                DocumentQuery = string.Format("and kn.IsDocument = 1");
            }
            else
            {
                DocumentQuery = string.Format("and kn.IsDocument = 0");
            }
            string sqlQuery = @"select kn.Comment,CONCAT(e.FirstName,' ',e.LastName) Name,kn.CreatedDate

                                from KnowledgeBaseFlagUser kn
                                left join Employee e on e.UserId=kn.CreatedBy
                                Where kn.Comment!='' AND KnowledgebaseId={0} {1} Order By kn.Id desc";
            sqlQuery = string.Format(sqlQuery, KnowledgebaseId, DocumentQuery);
            try
            {
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