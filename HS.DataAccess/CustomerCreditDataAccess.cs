using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.DataAccess
{
    public partial class CustomerCreditDataAccess
    {
        public CustomerCreditDataAccess() { }
        public CustomerCreditDataAccess(string ConnectionString) : base(ConnectionString) { }
        public double GetCustomerCreditAmountByCustomerId(Guid customerId)
        {
            string sqlQuery = @"
                                select ISNULL(SUM(amount),0.00) as CreditBalance 
                                from CustomerCredit where CustomerId = '{0}' and (IsDeleted != 1 or IsDeleted is null)";  
            try
            {
                sqlQuery = string.Format(sqlQuery, customerId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    SqlDataReader reader;
                    long rows = SelectRecords(cmd, out reader);
                    using (reader)
                    {
                        if (reader.Read())
                        {
                            return reader.GetDouble(0);
                        }
                    }
                }
                return 0; 
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        public List<CustomerCredit> GetCustomerCreditListByCustomerId(Guid customerId)
        {
            string sqlQuery = @"select cc.*
                                ,emp.FirstName+' '+emp.LastName as CreatedByVal 
                                from CustomerCredit cc
                                left join Employee emp on emp.UserId = cc.CreatedBy 
                                where cc.CustomerId = '{0}' and (cc.IsDeleted != 1 or cc.IsDeleted is null)";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    SqlDataReader reader;
                    long result = SelectRecords(cmd, out reader); 
                    List<CustomerCredit> list = new List<CustomerCredit>();

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            CustomerCredit customerCreditObject = new CustomerCredit();
                            FillObject(customerCreditObject, reader);
                            customerCreditObject.CreatedByVal = reader["CreatedByVal"].ToString();

                            list.Add(customerCreditObject);
                        }
                        reader.Close();
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                return new List<CustomerCredit>();
            }
        }

        public double GetRMRCustomerCreditAmountByCustomerId(Guid customerId, bool IsRMR, bool IsOther)
        {
            string rmrQuery = "";
            // if IsRMR is true and IsOther is false then only  rmr credit amount can get (only RMR customer credit balance) from this query
            if (IsRMR && !IsOther) 
            {
                rmrQuery = string.Format("AND IsRMRCredit = 1"); 
            }
            // if IsRMR is false and IsOther is true then without rmr credit amount can get (only general customer credit balance) from this query
            else if (!IsRMR && IsOther)
            {
                rmrQuery = string.Format("AND (IsRMRCredit != 1 OR IsRMRCredit is null)");
            }
            else if (!IsRMR && !IsOther)
            {
                return 0;
            }
            // if IsRMR is true and IsOther is true then all credit amount can get (general + rmr customer credit balance) from this query

            string sqlQuery = @"
                                select ISNULL(SUM(amount),0.00) as CreditBalance 
                                from CustomerCredit where CustomerId = '{0}' and (IsDeleted != 1 or IsDeleted is null) {1} ";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerId, rmrQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    SqlDataReader reader;
                    long rows = SelectRecords(cmd, out reader);
                    using (reader)
                    {
                        if (reader.Read())
                        {
                            return reader.GetDouble(0);
                        }
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }
    }
}
