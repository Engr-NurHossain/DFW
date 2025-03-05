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
	public partial class UserCompanyDeviceDataAccess
	{
        public DataTable GetDeviceIdForChangeCompany(Guid CompanyId, Guid UserId)
        {
            string sqlQuery = @"select DeviceId from UserCompanyDevice

                                where CompanyId = '{0}'
                                and UserId = '{1}'
                                and IsActive = 1";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        CompanyId,
                                        UserId
                                        );
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
