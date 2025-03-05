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
	public partial class AdjustmentDataAccess
	{
        public DataTable GetAllAdjustmentSchemeWithCommissionSession()
        {
            string sqlQuery = @"select ascheme.*, cs.Name as SessionName
                                from AdjustmentScheme ascheme
                                join CommisionSession cs
                                on cs.Id = ascheme.ComissionSessionId";
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

        public DataTable GetAllAdjustmentRuleWithCommissionSessionandScheme()
        {
            string sqlQuery = @"select ar.*, cs.Name as SessionName, ads.Name as SchemeName
                                from AdjustmentRule ar
                                join CommisionSession cs
                                on cs.Id = ar.ComissionSessionId
                                join AdjustmentScheme ads
                                on ar.AdjustSchemeId = ads.Id
                                ";
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

        public DataTable GetAllAdjustmentWithCommissionSessionandScheme()
        {
            string sqlQuery = @"select aa.*, cs.Name as SessionName, ads.Name as SchemeName
                                from Adjustment aa
                                join CommisionSession cs
                                on cs.Id = aa.ComissionSessionId
                                join AdjustmentScheme ads
                                on aa.AdjustSchemeId = ads.Id
                                ";
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

        public DataTable GetAllOverrideRangeWithOverrideName()
        {
            string sqlQuery = @"select orv.*, ovr.Name as OverName
                                from OverrideRange orv
                                join Override ovr
                                on ovr.Id = orv.OverrideId
                                ";
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

        public DataTable GetAllAdjustmentRuleBySchemeId(string tid, string sid)
        {
            string sqlQuery = @"select ar.*, cs.Name as SessionName, ads.Name as SchemeName
                                from AdjustmentRule ar
                                join CommisionSession cs
                                on cs.Id = ar.ComissionSessionId
                                join AdjustmentScheme ads
                                on ar.AdjustSchemeId = ads.Id
                                where ";
            string subquery = "";
            if (!string.IsNullOrWhiteSpace(tid) && tid != "-1" && !string.IsNullOrWhiteSpace(sid) && sid != "-1")
            {
                subquery = string.Format("ads.Id = {0} and cs.Id = {1}", tid, sid);
                sqlQuery += subquery;
            }
            else if (!string.IsNullOrWhiteSpace(tid) && tid != "-1")
            {
                subquery = string.Format("ads.Id = {0}", tid);
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

        public DataTable GetAllAdjustmentBySchemeId(string tid, string sid)
        {
            string sqlQuery = @"select aa.*, cs.Name as SessionName, ads.Name as SchemeName
                                from Adjustment aa
                                join CommisionSession cs
                                on cs.Id = aa.ComissionSessionId
                                join AdjustmentScheme ads
                                on aa.AdjustSchemeId = ads.Id
                                where ";
            string subquery = "";
            if (!string.IsNullOrWhiteSpace(tid) && tid != "-1" && !string.IsNullOrWhiteSpace(sid) && sid != "-1")
            {
                subquery = string.Format("ads.Id = {0} and cs.Id = {1}", tid, sid);
                sqlQuery += subquery;
            }
            else if (!string.IsNullOrWhiteSpace(tid) && tid != "-1")
            {
                subquery = string.Format("ads.Id = {0}", tid);
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
