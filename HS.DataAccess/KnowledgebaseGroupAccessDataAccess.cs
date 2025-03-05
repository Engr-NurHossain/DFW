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
	public partial class KnowledgebaseGroupAccessDataAccess
	{
        public DataSet GetAllAccessedKnowledgebase(string UserRole)
        {
            string sqlQuery = @"select kn.Id, kn.Title, kng.IsDefault 
                                from KnowledgebaseGroupAccess kng
                                left join Knowledgebase kn on kn.Id = kng.KnowledgebaseId
                                Where kn.IsDeleted = 0 and kng.UserGroupId  = (select Id from PermissionGroup Where Name = '{0}')
                                order by Title asc";
            try
            {
                sqlQuery = string.Format(sqlQuery, UserRole);
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
        public bool DeleteKnowledgebaseGroupAccess(int id, bool IsDocument)
        {
            string conditional = "";
            if (IsDocument)
            {
                conditional = "and IsDocumentLibrary = 1";
            }
            else
            {
                conditional = "and IsDocumentLibrary = 0";
            }
            string SqlQuery = @"delete from KnowledgebaseGroupAccess where KnowledgebaseId = {0} {1}";
            try
            {
                SqlQuery = string.Format(SqlQuery, id, conditional);
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