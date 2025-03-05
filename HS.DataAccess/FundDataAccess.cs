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
	public partial class FundDataAccess
	{
        public FundDataAccess() { }
        public DataTable GetRevenueByComanyIdAndCustomerId(Guid customerId, Guid companyId)
        {
            string sqlQuery = @"select * into #tempfund from Fund 
                                where CustomerId = '{0}'
	                            and CompanyId= '{1}' 
	                            and PaymentStatus = 'Paid'
                                select ((select SUM(Amount) from #tempfund where [Type]='income')   
                                - (select SUM(Amount) from #tempfund where [Type]='expense')) Revenue  
                                drop table #tempfund";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerId, companyId);
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
