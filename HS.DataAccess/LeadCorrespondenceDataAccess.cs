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
	public partial class LeadCorrespondenceDataAccess
	{
        public LeadCorrespondenceDataAccess() { }
        public LeadCorrespondenceDataAccess(string ConStr) : base(ConStr) { }
        public DataTable GetAllMailCorrespondenceByCompanyIdAndCustomerId(Guid comid, Guid cusid)
        {

            string sqlQuery = @"select lc.*,emp.FirstName+' '+emp.LastName as EmpName from LeadCorrespondence lc
                                left join employee emp on emp.UserId = lc.SentBy
                                where lc.CompanyId = '{0}' and lc.CustomerId = '{1}' order by lc.Id desc";
            try
            {
                sqlQuery = string.Format(sqlQuery, comid, cusid);
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
