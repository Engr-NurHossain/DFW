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
	public partial class GlobalSettingDataAccess
	{
        public GlobalSettingDataAccess() { }
        public GlobalSettingDataAccess(string ConnectionStr):base(ConnectionStr) { }
        private const string GETGLOBALSETTINGBYKEY = "GetGlobalSettingsByKey";

        #region Get By Key Method

        public GlobalSetting Get(String _Key,Guid CompanyId)
        {
            using (SqlCommand cmd = GetSPCommand(GETGLOBALSETTINGBYKEY))
            {
                AddParameter(cmd, pNVarChar(GlobalSettingBase.Property_SearchKey, _Key));
                AddParameter(cmd, pGuid(GlobalSettingBase.Property_CompanyId, CompanyId));
                return GetObject(cmd);
            }
        }
        #endregion

        public DataTable GetQATechCallGlobSettingsBycompanyId(Guid Companyid)
        {
            string sqlQuery = @"select 
		                            _glob.Value as Value 
	                            from 
		                            GlobalSetting _glob
	                            where 
		                            _glob.CompanyId = '{0}' and
		                            _glob.IsActive = 1 and
		                            _glob.SearchKey = 'QA1 Setting'
                                Union All 
	                            select 
		                            _glob.Value as Value 
	                            from 
		                            GlobalSetting _glob
	                            where 
		                            _glob.CompanyId = '{0}' and
		                            _glob.IsActive = 1 and
		                            _glob.SearchKey = 'Tech Call Setting'
                                Union All
	                            select 
		                            _glob.Value as Value 
	                            from 
		                            GlobalSetting _glob
	                            where 
		                            _glob.CompanyId = '{0}' and
		                            _glob.IsActive = 1 and
		                            _glob.SearchKey = 'QA2 Setting'";
            try
            {
                sqlQuery = string.Format(sqlQuery, Companyid);
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

        public DataTable GetQATechCallValueBycompanyId(Guid Companyid, Guid customerid)
        {
            string sqlQuery = @"select isnull(cus.IsTechCallPassed,0) as Techval
                                from Customer cus
                                join CustomerCompany cc
                                on cc.CustomerId = cus.CustomerId
                                where cc.CustomerId = '{1}'
                                and cc.CompanyId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, Companyid, customerid);
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
