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
	public partial class CustomerSystemInfoDataAccess
	{
        public bool ReseedCustomerSystemInfoTable()
        {
            string SqlQuery = @"Delete from CustomerSystemInfo
                                DBCC CHECKIDENT('CustomerSystemInfo', RESEED, 0)
                                ";
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

        public bool UpdateCustomerSysinfo(Guid CustomerId, string ColumnName, string NewValue)
        {
            string sqlQuery = @"Update CustomerSystemInfo
                                Set {1} = '{2}' where CustomerId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId, ColumnName, NewValue);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    ExecuteCommand(cmd);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }
        public DataTable GetAllCustomerSystemInfoDetailsByCustomerId(Guid customerid)
        {
            string sqlQuery = @"select csi.* 
                                ,lk.DisplayText as InstallTypeVal
                                ,lkLead.DisplayText as LeadInstallTypeVal
                                from CustomerSystemInfo csi

                                left join Lookup lk on csi.InstallType = lk.DataValue and lk.DataKey = 'InstallType' 
                                and csi.InstallType is not null and csi.InstallType !=''

                                left join Lookup lkLead on csi.InstallType = lkLead.DataValue and lkLead.DataKey = 'LeadInstallType'
                                and csi.InstallType is not null and csi.InstallType !=''
                                where CustomerId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerid);
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

        public DataTable GetCustomerSystemInfoIdByCompanyId(Guid companyID, Guid customerID)
        {
            string sqlQuery = @"select CS.Id
                                from CustomerCompany CC
                                INNER JOIN CustomerSystemInfo CS
                                ON CC.CompanyId = CS.CompanyId AND CC.CustomerId = CS.CustomerId
                                WHERE CS.CompanyId = '{0}' AND CS.CustomerId = '{1}'
                                AND CC.IsLead = 0";
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

        public DataTable GetCustomerSystemInfoLastUpdateDateByCompanyIdandCustomerId(Guid companyID, Guid customerID)
        {
            string sqlQuery = @"select CC.IsLead from CustomerCompany CC
                                where cc.CompanyId='{0}'
                                and cc.CustomerId='{1}'";
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
