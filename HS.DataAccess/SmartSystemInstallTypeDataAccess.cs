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
	public partial class SmartSystemInstallTypeDataAccess
	{
        public DataTable GetAllSmartSystemInstallTypeByCompanyId(Guid companyId)
        {
            string sqlQuery = @"select sit.*, st.Name as SystemType, it.Name as InstallType from SmartSystemInstallType as sit
                                left join SmartSystemType st on sit.SystemId=st.Id
                                left join SmartInstallType it on sit.InstallTypeId=it.Id
                                where sit.CompanyId='{0}' 
								And st.Name is Not Null
								And it.Name is Not Null";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId);
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
