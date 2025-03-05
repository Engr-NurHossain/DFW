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
	public partial class PackageSystemInstallTypeDataAccess
	{
        public DataTable GetAllInstallTypeBySystemId(Guid companyId, int SystemId)
        {
            string sqlQuery = @"select sit.*
                               ,ssit.SystemId from
                                SmartSystemInstallType ssit
                                lEFT JOIN SmartInstallType sit
                                on ssit.InstallTypeId=sit.Id
                                where ssit.Systemid={1} and ssit.CompanyId='{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, SystemId);
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
