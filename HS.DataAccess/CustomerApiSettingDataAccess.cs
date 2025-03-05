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
	public partial class CustomerApiSettingDataAccess
	{
        public DataTable GetAllApiSettingDetailByCustomerId(Guid customerID)
        {
            string sqlQuery = @"select app.Url, app.UserName, app.Password, app.AccountName
                                from CustomerApiSetting app
                                where app.CustomerId='{0}'
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerID);
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

        public DataTable GetCustomerApiAlarmIdByCompanyIdandCustomerId(Guid companyID, Guid customerID)
        {
            string sqlQuery = @"select cas.*
                                from CustomerCompany CC
                                INNER JOIN CustomerApiSetting cas
                                ON CC.CompanyId = cas.CompanyId AND CC.CustomerId = cas.CustomerId
                                WHERE cas.CompanyId = '{0}' AND cas.CustomerId = '{1}'
                                AND CC.IsLead = 0 and cas.AccountName='Alarm.com'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyID, customerID);
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

        public DataTable GetCustomerApiMonitronicsIdByCompanyIdandCustomerId(Guid companyID, Guid customerID)
        {
            string sqlQuery = @"select cas.*
                                from CustomerCompany CC
                                INNER JOIN CustomerApiSetting cas
                                ON CC.CompanyId = cas.CompanyId AND CC.CustomerId = cas.CustomerId
                                WHERE cas.CompanyId = '{0}' AND cas.CustomerId = '{1}'
                                AND CC.IsLead = 0 and cas.AccountName='Monitronics'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyID, customerID);
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

        public DataTable GetCustomerApiCentralIdByCompanyIdandCustomerId(Guid companyID, Guid customerID)
        {
            string sqlQuery = @"select cas.*
                                from CustomerCompany CC
                                INNER JOIN CustomerApiSetting cas
                                ON CC.CompanyId = cas.CompanyId AND CC.CustomerId = cas.CustomerId
                                WHERE cas.CompanyId = '{0}' AND cas.CustomerId = '{1}'
                                AND CC.IsLead = 0 and cas.AccountName='Central Station'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyID, customerID);
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
