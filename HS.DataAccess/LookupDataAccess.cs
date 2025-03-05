using System;
using System.Data;
using System.Data.SqlClient; 
using HS.Framework;
using System.Linq; 
using System.Collections.Generic;

namespace HS.DataAccess
{
	public partial class LookupDataAccess
	{
        public LookupDataAccess() { }
        public LookupDataAccess(string ConStr):base(ConStr) { }
        public List<string> GetDataKeyList()
        { 
            string sqlQuery = @"Select DataKey from Lookup group by DataKey";
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd); 
                    return (from DataRow dr in dsResult.Tables[0].Rows select dr["DataKey"].ToString()).ToList();

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool SetAllDefaultItemsToFalseByDataKey(string dataKey)
        {
            string SqlQuery = @"UPDATE Lookup set IsDefaultItem = 0 where datakey = '{0}'";
            SqlQuery = string.Format(SqlQuery, dataKey);
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
        public bool DeleteLookUpWithChild(int Id)
        {
            string SqlQuery = @"delete from Lookup where ParentDataKey=(select DataValue from Lookup where Id={0})
                                delete from Lookup where Id={0}";
            SqlQuery = string.Format(SqlQuery, Id);
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
        public DataTable GetLookupByKeyWithParent(string key, Guid comid)
        {
            string sqlQuery = @"select lfirst.DisplayText,lfirst.DataValue,lfirst.DataOrder,lfirst.IsActive from Lookup lfirst where lfirst.DataKey='{0}' and lfirst.DataValue='-1' and lfirst.CompanyId = '{1}'
                                    UNION
                                    select 
                                    (Select DisplayText from Lookup linner where linner.DataValue=lp.ParentDataKey and linner.ParentDataKey='Parent' and linner.DataKey = '{0}' and linner.CompanyId = '{1}') +' -> ' +lp.DisplayText as DisplayText,
                                    lp.DataValue,
                                    lp.DataOrder,
									lp.IsActive
                                    from Lookup lp
                                    where lp.ParentDataKey<>'' and lp.ParentDataKey!='Parent' and lp.DataKey = '{0}' and lp.CompanyId = '{1}'
                                    UNION
                                    select louter.DisplayText,louter.DataValue,louter.DataOrder,louter.IsActive from Lookup louter where louter.DataKey='{0}' and (louter.ParentDataKey='' or louter.ParentDataKey=NULL)
                                     and louter.CompanyId = '{1}'
                                    order by DataOrder";
            try
            {
                sqlQuery = string.Format(sqlQuery, key, comid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

    }	
}
