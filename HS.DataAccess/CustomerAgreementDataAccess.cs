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
	public partial class CustomerAgreementDataAccess
	{
        public CustomerAgreementDataAccess(string ConnStr) : base(ConnStr) { }
        public DataTable GetCustomerAgreementByCompanyIdAndCustomerIdAndInvoiceId(Guid CompanyId, Guid CustomerId,string InvoiceId)
        {
            string sqlQuery = @"DECLARE @CustomerId uniqueidentifier
                                DECLARE @CompanyId uniqueidentifier
                                DECLARE @InvoiceId nvarchar(50)

                                SET @CustomerId = '{0}'
                                SET @CompanyId = '{1}'
                                SET @InvoiceId = '{2}'

                                select *
                                from CustomerAgreement
                                where id = (select top 1 id from customeragreement 
	                                where CustomerId = @CustomerId
	                                and CompanyId = @CompanyId
	                                and Invoiceid = @InvoiceId
	                                and [Type] = 'LoadEstimate'
	                                order by id desc
	                                ) 
                                union
                                select *
                                from CustomerAgreement
                                where id = (select top 1 id from customeragreement 
	                                where CustomerId = @CustomerId
	                                and CompanyId = @CompanyId
	                                and Invoiceid = @InvoiceId
	                                and [Type] = 'SignEstimate'
	                                order by id desc
	                                )  
                                union 
                                select *
                                from CustomerAgreement
                                where id = (select top 1 id from customeragreement 
	                                where CustomerId = @CustomerId
	                                and CompanyId = @CompanyId
	                                and Invoiceid = @InvoiceId
	                                and [Type] = 'SubmitEstimate'
	                                order by id desc
	                                ) 
                                --group by  customerid,companyid,invoiceid,ip,useragent,[type]

                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId, CompanyId, InvoiceId);
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

        public DataTable GetIpAndUserAgentByCustomerIdAndCompanyId(Guid comid, Guid cusid)
        {
            string sqlQuery = @"select IP, UserAgent
                                from CustomerAgreement
                                where CustomerId = '{1}'
                                and CompanyId = '{0}'
                                group by IP, UserAgent";
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
