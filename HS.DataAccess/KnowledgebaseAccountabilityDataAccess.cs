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
	public partial class KnowledgebaseAccountabilityDataAccess
	{
        public DataSet GetAllAccessedKnowledgebaseListForUser(Guid UserId)
        {
            string sqlQuery = @"select ka.*, kb.Title,
                                emp.FirstName +' '+ emp.LastName as [AssignedByUserName]
                                from KnowledgebaseAccountability ka
                                left join Employee emp on emp.UserId = ka.AssignedBy
                                left join Knowledgebase kb on kb.Id = ka.KnowledgebaseId
                                where AssignedUser ='{0}' and IsRead = 0
                                order by AssignedDate desc";
            try
            {
                sqlQuery = string.Format(sqlQuery, UserId);
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
        public DataSet GetAllUnreadKnowledgebaseList(Guid UserId)
        {
            string StrUserId = "";
            if (UserId != Guid.Empty)
            {
                StrUserId = string.Format("and ka.AssignedUser = '{0}'", UserId);
            }
            string sqlQuery = @"select * from 
                                Knowledgebase kn
                                left join KnowledgebaseAccountability ka on ka.Knowledgebaseid= kn.Id
                                where ka.IsRead = 0 and ka.AssignedUser is not null and kn.IsDeleted = 0 and kn.IsDocumentLibrary = 0 {0}";
            try
            {
                sqlQuery = string.Format(sqlQuery, StrUserId);
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