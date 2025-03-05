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
	public partial class CredentialSettingDataAccess
	{
        public DataTable GetAllCredentialSettingListByCompanyId(Guid companyid)
        {
            string sqlQuery = @"select cs.*,Ah.Name as DisplayName 
                                from CredentialSetting cs
                                join AccountHolder Ah
                                on cs.AcountHolderId = Ah.Id 
                                and ah.CompanyId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid);
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
