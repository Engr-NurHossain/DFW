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
    public partial class PaymentInfoCustomerDataAccess
    {
        public PaymentInfoCustomerDataAccess(string ConStr) : base(ConStr) { }
        public bool DeletePaymentInfoCustomerById(int id)
        {
            string SqlQuery = @"delete from PaymentInfoCustomer
                                where Id = {0}";
            SqlQuery = string.Format(SqlQuery, id);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
                {

                    ExecuteCommand(cmd);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool DeletePaymentInfoCustomerByPayForAndCustomerId(string payfor,Guid CustomerId)
        {
            string SqlQuery = @"delete from PaymentInfoCustomer
                                where Payfor = '{0}' AND CustomerId = '{1}' ";
            SqlQuery = string.Format(SqlQuery, payfor,CustomerId);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
                {

                    ExecuteCommand(cmd);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool DeleteByCustomerIdCompanyIdAndPaymentInfoId(Guid customerId, Guid companyId, int paymentInfoId,int paymentinfoCusId)
        {
            string SqlQuery = @"delete from PaymentInfoCustomer where CustomerId ='{0}' and CompanyId = '{1}' and PaymentInfoId = {2} and Id={3}";
            SqlQuery = string.Format(SqlQuery, customerId,companyId,paymentInfoId, paymentinfoCusId);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
                {
                    ExecuteCommand(cmd);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public DataTable GetAllPaidPaymentInfoCustomer(Guid comid, Guid cusid)
        {
            string SqlQuery = @"select info.*, ppc.[Type] as PaymentType from PaymentInfoCustomer info
                                left join PaymentProfileCustomer ppc on ppc.PaymentInfoId = info.Id
                                where info.CustomerId = '{1}'
                                and info.CompanyId = '{0}'";
            SqlQuery = string.Format(SqlQuery, comid, cusid);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
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
