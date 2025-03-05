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
	public partial class RMRTagDataAccess
	{
        public DataTable GetAllRMRTag(string search)
        {
            string subquery = "";
            string sqlQuery = @"select tag.*, emp.FirstName + ' ' + emp.LastName as CreatedUser from RMRTag tag
                                left join Employee emp on emp.UserId = tag.CreatedBy
                                {0}";
            if (!string.IsNullOrWhiteSpace(search))
            {
                subquery = string.Format("where tag.TagName like '%{0}%'", search);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, subquery);
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
        public DataTable GetAllKnowledgebaseRMRTag(string search, string order)
        {
            string subquery = "";
            string sqlorderby = "";
            if (!string.IsNullOrWhiteSpace(order) && order != "undefined")
            {
                if (order == "descending/name")
                {
                    sqlorderby = " order by tag.TagName desc";
                }
                else if (order == "ascending/name")
                {
                    sqlorderby = " order by tag.TagName asc ";
                }
                else if (order == "descending/createdon")
                {
                    sqlorderby = " order by tag.CreatedDate desc";
                }
                else if (order == "ascending/createdon")
                {
                    sqlorderby = " order by tag.CreatedDate asc ";
                }
                else if (order == "descending/createdby")
                {
                    sqlorderby = " order by emp.FirstName + ' ' + emp.LastName desc";
                }
                else if (order == "ascending/createdby")
                {
                    sqlorderby = " order by emp.FirstName + ' ' + emp.LastName asc ";
                }
                if (order == "descending/utilized")
                {
                    sqlorderby = " order by Used desc";
                }
                else if (order == "ascending/utilized")
                {
                    sqlorderby = " order Used asc ";
                }
                else
                {
                    sqlorderby = " order by tag.TagName asc";
                }
            }
            else
            {
                sqlorderby = " order by tag.TagName asc";
            }
            string sqlQuery = @"select tag.*,
                                emp.FirstName + ' ' + emp.LastName as CreatedUser,
                                (select count(*) from Knowledgebase kn where IsDeleted = 0 and kn.Tags like CONCAT('%', tag.TagName, '%')) as Used
                                from KnowledgebaseRMRTag tag
                                left join Employee emp on emp.UserId = tag.CreatedBy
                                where tag.Id > 0 and tag.IsDeleted=0 {0}
                                {1}";
            if (!string.IsNullOrWhiteSpace(search))
            {
                subquery = string.Format("and tag.TagName like '%{0}%'", search);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, subquery, sqlorderby);
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
        public DataTable GetAllVersion(string search, string order)
        {
            string subquery = "";
            string sqlorderby = "";
            if (!string.IsNullOrWhiteSpace(order) && order != "undefined")
            {
                if (order == "ascending/vesion")
                {
                    sqlorderby = " order by blog.Version asc ";
                }
                else if (order == "descending/vesion")
                {
                    sqlorderby = " order by blog.Version desc";
                } 
                else if (order == "descending/builddate")
                {
                    sqlorderby = " order by blog.BuildDate desc";
                }
                else if (order == "ascending/builddate")
                {
                    sqlorderby = " order by blog.BuildDate asc ";
                }  
            }
            else
            {
                sqlorderby = " order by Id Desc";
            }
            string sqlQuery = @"Select * from BuildLog blog
                                where blog.Id > 0  {0}
                                {1}";
            if (!string.IsNullOrWhiteSpace(search) && search != "undefined")
            {
                subquery = string.Format("and blog.Version like '%{0}%'", search);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, subquery, sqlorderby);
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
        public DataTable GetAllTagListByContactId(Guid contactid)
        {
            string sqlQuery = @"select tag.TagName, tag.TagIdentifier from RMRTag tag
                                left join RMRTagMap map on map.TagId = tag.TagIdentifier
                                where map.ContactId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, contactid);
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
        public DataTable GetAllTagListByQuery(string query, string glist)
        {
            string subquery = "";
            string sqlQuery = @"SELECT *
                                FROM
                                  ( SELECT tag.TagName, tag.TagIdentifier
                                    FROM RMRTag tag
									where tag.TagName like '%{0}%'
                                    {2}
                                  ) tmp";

            if (!string.IsNullOrWhiteSpace(glist))
            {
                subquery = string.Format("and tag.TagIdentifier not in({0})", glist);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, query, glist, subquery);
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
        public bool DeleteTagMapByKnowladgeId(int id)
        {
            string SqlQuery = @"update RMRTagMap set IsDeleted = 1 where KnowledgebaseId = {0}";
            SqlQuery = string.Format(SqlQuery, id);
            try
            {
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
