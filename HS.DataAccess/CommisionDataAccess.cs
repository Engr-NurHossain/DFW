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
	public partial class CommisionDataAccess
	{
        public DataTable GetAllCommissionRangeWithCommissionTypeAndSession()
        {
            string sqlQuery = @"select cr.*, ct.Name as TypeName, cs.Name as SessionName
                                from CommisionRange cr
                                join CommisionType ct
                                on ct.Id = cr.CommisionTypeId
                                join CommisionSession cs
                                on cs.Id = cr.CommisionSessionId";
            try
            {
                sqlQuery = string.Format(sqlQuery);
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

        public DataTable GetAllCommissionRangeByTypeId(string tid, string sid)
        {
            string sqlQuery = @"select cr.*, ct.Name as TypeName, cs.Name as SessionName
                                from CommisionRange cr
                                join CommisionType ct
                                on ct.Id = cr.CommisionTypeId
                                join CommisionSession cs
                                on cs.Id = cr.CommisionSessionId
                                where ";
            string subquery = "";
            if(!string.IsNullOrWhiteSpace(tid) && tid != "-1" && !string.IsNullOrWhiteSpace(sid) && sid != "-1")
            {
                subquery = string.Format("ct.Id = {0} and cs.Id = {1}", tid, sid);
                sqlQuery += subquery;
            }
            else if(!string.IsNullOrWhiteSpace(tid) && tid != "-1")
            {
                subquery = string.Format("ct.Id = {0}", tid);
                sqlQuery += subquery;
            }
            else if(!string.IsNullOrWhiteSpace(sid) && sid != "-1")
            {
                subquery = string.Format("cs.Id = {0}", sid);
                sqlQuery += subquery;
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, tid, sid);
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

        public DataTable GetAllCommissionWithCommissionTypeAndSession()
        {
            string sqlQuery = @"select c.*, ct.Name as TypeName, cs.Name as SessionName
                                from Commision c
                                join CommisionType ct
                                on ct.Id = c.CommisionTypeId
                                join CommisionSession cs
                                on cs.Id = c.CommisionSessionId";
            try
            {
                sqlQuery = string.Format(sqlQuery);
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

        public DataTable GetAllCommissionByTypeId(string tid, string sid)
        {
            string sqlQuery = @"select c.*, ct.Name as TypeName, cs.Name as SessionName
                                from Commision c
                                join CommisionType ct
                                on ct.Id = c.CommisionTypeId
                                join CommisionSession cs
                                on cs.Id = c.CommisionSessionId
                                where ";
            string subquery = "";
            if (!string.IsNullOrWhiteSpace(tid) && tid != "-1" && !string.IsNullOrWhiteSpace(sid) && sid != "-1")
            {
                subquery = string.Format("ct.Id = {0} and cs.Id = {1}", tid, sid);
                sqlQuery += subquery;
            }
            else if (!string.IsNullOrWhiteSpace(tid) && tid != "-1")
            {
                subquery = string.Format("ct.Id = {0}", tid);
                sqlQuery += subquery;
            }
            else if (!string.IsNullOrWhiteSpace(sid) && sid != "-1")
            {
                subquery = string.Format("cs.Id = {0}", sid);
                sqlQuery += subquery;
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, tid, sid);
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
