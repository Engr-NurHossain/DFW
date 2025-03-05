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
	public partial class ThirdPartySettingDataAccess
	{
        public DataTable GetAllAuthorizeSettingByCompanyId(Guid companyId)
        {
            string sqlQuery = @"select *
                                from ThirdPartySetting ts
                                where ts.Type='AuthorizeDotNet'
                                and ts.CompanyId='{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId);
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

        public DataTable GetAllAlarmSettingByCompanyId(Guid companyId)
        {
            string sqlQuery = @"select *
                                from ThirdPartySetting ts
                                where ts.Type='AlarmDotcom'
                                and ts.CompanyId='{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId);
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

        public DataTable GetAllTechSettingByCompanyId(Guid companyId)
        {
            string sqlQuery = @"select *
                                from ThirdPartySetting ts
                                where ts.Type='TechSchedule'
                                and ts.CompanyId='{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId);
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
