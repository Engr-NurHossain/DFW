using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.DataAccess
{
	public partial class DocumentLibraryWeblinkDataAccess
	{
        public List<DocumentLibraryWeblink> GetWeblinkByDocumentLibraryId(int id)
        {
            string sqlQuery = @"select cw.* from DocumentLibraryWeblink cw where cw.KnowledgebaseId = {0}";
            try
            {
                sqlQuery = string.Format(sqlQuery, id);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    return GetList(cmd, -1);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool DeleteDocumentLibraryWeblinkByKnowledgebaseId(int id)
        {
            string SqlQuery = @"delete from DocumentLibraryWeblink where KnowledgebaseId = {0}";
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
    }	
}