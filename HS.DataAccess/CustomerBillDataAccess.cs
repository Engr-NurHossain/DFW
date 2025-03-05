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
	public partial class CustomerBillDataAccess
	{
        public DataTable GetAllCustomerBillByCustomerIdAndCompanyId(Guid customerId, Guid companyId)
        {
            string sqlQuery = @"select bill.*
                                from CustomerBill bill
                                left join Customer cus
                                on bill.CustomerId = cus.CustomerId
                                where  bill.CustomerId ='{0}'
                                and bill.CompanyId='{1}'";
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
