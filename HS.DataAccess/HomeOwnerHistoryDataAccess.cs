using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Linq;

namespace HS.DataAccess
{
	public partial class HomeOwnerHistoryDataAccess
	{
        public DataTable GetHomeOwnerListBCustomerId(Guid CustomerId,Guid CompanyId)
        {
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            string sqlQuery = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }


            sqlQuery = @"select hh.*,{2} [CustomerName],emp.FirstName+' '+emp.LastName as RequestedByVal from HomeOwnerHistory hh
                        left join Customer cus on cus.CustomerId = hh.CustomerId
                        left join Employee emp on emp.UserId = hh.RequestedBy
                        where hh.CustomerId= '{0}' and hh.CompanyId = '{1}'";


            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId, CompanyId, NameSql);
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
