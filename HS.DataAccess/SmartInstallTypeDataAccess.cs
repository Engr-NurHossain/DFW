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
	public partial class SmartInstallTypeDataAccess
	{
        public SmartInstallTypeDataAccess(string ConStr) : base(ConStr) { }
        public DataTable GetAllInstallTypeByCompanyIdSystemId(int SystemId, Guid companyId)
        {
            string sqlQuery = @"select sit.*, st.Name from SmartSystemInstallType sit
                                left join SmartInstallType st on st.Id = sit.InstallTypeId
                                where sit.SystemId='{0}' and sit.CompanyId='{1}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, SystemId, companyId);
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
