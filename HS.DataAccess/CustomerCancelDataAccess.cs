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
	public partial class CustomerCancelDataAccess
	{
        public DataTable GetCustomerCancelDateByCustomerIdAndCompanyId(Guid companyId, Guid customerid)
        {
            string sqlQuery = @"select cancel.CancelDatet, cancel.CancelReason, emp.FirstName + ' ' + emp.LastName as EmpName
                                from CustomerCancel cancel
                                left join Customer cus
                                on cus.CustomerId = cancel.CustomerId
                                left join CustomerCompany cc
                                on cc.CustomerId = cus.CustomerId
                                left join Employee emp on emp.UserId = cancel.EmployeeId
                                where cc.CompanyId = '{0}'
                                and cus.CustomerId = '{1}'
                                order by cancel.CancelDatet desc";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, customerid);
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

        public DataTable GetAllCustomerCancellLogByCustomerId(Guid customerId)
        {
            string sqlQuery = @"select cancel.*, emp.FirstName + ' ' + emp.LastName as EmpName
                                from CustomerCancel cancel
                                left join Customer cus
                                on cus.CustomerId = cancel.CustomerId
                                left join CustomerCompany cc
                                on cc.CustomerId = cus.CustomerId
                                left join Employee emp on emp.UserId = cancel.EmployeeId 
                                where cus.CustomerId = '{0}'
                                order by cancel.CancelDatet asc";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerId);
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
