using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.DataAccess
{
	public partial class PaymentInfoDataAccess
	{
        public PaymentInfoDataAccess(string ConStr) : base(ConStr) { }
        public bool DeletePaymentInfoById(int payid)
        {
            string SqlQuery = @"delete from 
                                PaymentInfo
                                where Id = {0}";
            SqlQuery = string.Format(SqlQuery, payid);
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

        public DataTable GetPaymentInfoByCompanyIdandLeadId(Guid companyid, int Leadid)
        {
            string sqlQuery = @"select pinfo.*, cus.PaymentMethod as BillMethod
                                from PaymentInfo pinfo
                                join PaymentInfoCustomer pic
                                on pinfo.Id=pic.PaymentInfoId
								join Customer cus
								on cus.CustomerId=pic.CustomerId
                                where cus.Id='{1}'
                                and pic.CompanyId='{0}'
								and cus.PaymentMethod = 'ACH'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, Leadid);
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

        public DataTable GetPaymentInfoByCompanyIdandCustomerId(Guid companyid, Guid customerid)
        {
            string sqlQuery = @"Select pi.* , cus.PaymentMethod as BillMethod
                                from PaymentInfo pi 
                                left join PaymentInfoCustomer pic on pic.PaymentInfoId=pi.Id
                                left join Customer cus on cus.CustomerId=pic.CustomerId
                                where pic.CustomerId='{1}' 
                                and pic.CompanyId='{0}'
                                order by pi.Id desc";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, customerid);
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

        public DataTable GetPaymentInfoEFTByCompanyIdandLeadId(Guid companyid, int Leadid)
        {
            string sqlQuery = @"select pinfo.*, cus.PaymentMethod as BillMethod
                                from PaymentInfo pinfo
                                join PaymentInfoCustomer pic
                                on pinfo.Id=pic.PaymentInfoId
								join Customer cus
								on cus.CustomerId=pic.CustomerId
                                where cus.Id='{1}'
                                and pic.CompanyId='{0}'
								and cus.PaymentMethod = 'EFT'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, Leadid);
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
        public DataTable GetAllPaymentInfoByCustomerIdAndCompanyId(Guid customerId, Guid companyId)
        {
            string sqlQuery = @"Declare @CustomerId uniqueidentifier
                                Declare @CompanyId uniqueidentifier
                                SET @CompanyId = '{0}'
                                set @CustomerId = '{1}'
                                select pinfocustomer.CustomerId as PaymentCustomerId,
                                pinfocustomer.Payfor,
								pinfocustomer.Type,
                                pinfo.* from PaymentInfo pinfo 
                                left join PaymentInfoCustomer pinfocustomer 
                                on pinfocustomer.PaymentInfoId = pinfo.Id
                                where pinfocustomer.CustomerId = @CustomerId
                                and pinfocustomer.CompanyId =@CompanyId
                                and pinfo.CompanyId = @CompanyId";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, customerId);
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
        public DataTable GetPaymentInfoCreditTByCompanyIdandLeadId(Guid companyid, int Leadid)
        {
            string sqlQuery = @"select pinfo.*, cus.PaymentMethod as BillMethod
                                from PaymentInfo pinfo
                                join PaymentInfoCustomer pic
                                on pinfo.Id=pic.PaymentInfoId
								join Customer cus
								on cus.CustomerId=pic.CustomerId
                                where cus.Id='{1}'
                                and pic.CompanyId='{0}'
								and cus.PaymentMethod = 'Credit Card'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, Leadid);
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


        public DataTable GetPaymentInfoCheckByCompanyIdAndLeadId(Guid companyid, int Leadid)
        {
            string sqlQuery = @"select pinfo.*, cus.PaymentMethod as BillMethod
                                from PaymentInfo pinfo
                                join PaymentInfoCustomer pic
                                on pinfo.Id=pic.PaymentInfoId
								join Customer cus
								on cus.CustomerId=pic.CustomerId
                                where cus.Id='{1}'
                                and pic.CompanyId='{0}'
								and cus.PaymentMethod = 'Check'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, Leadid);
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

        public DataTable GetPaymentInfoCashByCompanyIdAndLeadId(Guid companyid, int Leadid)
        {
            string sqlQuery = @"select pinfo.*, cus.PaymentMethod as BillMethod
                                from PaymentInfo pinfo
                                join PaymentInfoCustomer pic
                                on pinfo.Id=pic.PaymentInfoId
								join Customer cus
								on cus.CustomerId=pic.CustomerId
                                where cus.Id='{1}'
                                and pic.CompanyId='{0}'
								and cus.PaymentMethod = 'cash'
								and pinfo.IsCash = 1";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, Leadid);
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

        public DataTable GetPaymentInfo1ByCompanyIdandLeadId(Guid companyid, int Leadid)
        {
            string sqlQuery = @"select pinfo.*, cus.PaymentMethod as BillMethod
                                from PaymentInfo pinfo
                                join PaymentInfoCustomer pic
                                on pinfo.Id=pic.PaymentInfoId
								join Customer cus
								on cus.CustomerId=pic.CustomerId
                                where cus.Id='{1}'
                                and pic.CompanyId='{0}'
								and cus.PaymentMethod = 'Debit Card'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, Leadid);
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

        public bool DeleteAllPaymentInfoByCustomerIdCompanyIdandPaymentInfoId(Guid CompanyId, int id)
        {
            string SqlQuery = @"delete from
                                PaymentInfo
                                where CompanyId='{0}' 
                                and Id='{1}'";
            SqlQuery = string.Format(SqlQuery, CompanyId, id);
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
        public DataTable GetOldPaymentInfoByCustomerIdandId(Guid customerid, int Leadid)
        {
            string sqlQuery = @"select info.*
                                from PaymentInfo info
                                join PaymentInfoCustomer pc
                                on info.CompanyId=pc.CompanyId
                                where pc.CustomerId='{0}'
                                and info.Id='{1}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerid, Leadid);
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
        public DataTable GetOldPaymentInfoByCustomerIdOnly(Guid customerid)
        {
            string sqlQuery = @"select info.*
                                from PaymentInfo info
                                join PaymentInfoCustomer pc
                                on info.CompanyId=pc.CompanyId
                                
                               
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerid);
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
        public DataTable GetOldPaymentInfoByCustomerId(Guid customerid,int Id)
        {
            string sqlQuery = @"select distinct info.*
                                from PaymentInfo info
                                --join PaymentInfoCustomer pc
                                --on info.CompanyId=pc.CompanyId
                                
                                where info.Id = '{1}'
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerid, Id);
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


        //public DataTable GetMMRPaymentInfoByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId)
        //{
        //    string sqlQuery = @"select 
	       //                         _pi.*
        //                        from 
	       //                         Customer _cus
	       //                         left join CustomerCompany _cc
		      //                          on _cus.CustomerId = _cc.CustomerId
	       //                         left join PaymentInfoCustomer _pic
		      //                          on _cus.CustomerId = _pic.CustomerId 
	       //                         left join PaymentInfo _pi
		      //                          on _pic.PaymentInfoId = _pi.Id
        //                        where
	       //                         _cc.CompanyId = '{0}'
	       //                         and _cus.CustomerId = '{1}'
        //                            order by _pic.Id desc ";
        //    try
        //    {
        //        sqlQuery = string.Format(sqlQuery, CompanyId, CustomerId);
        //        using (SqlCommand cmd = GetSQLCommand(sqlQuery))
        //        {
        //            DataSet dsResult = GetDataSet(cmd);
        //            return dsResult.Tables[0];
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public DataTable GetPaymentInfoListByCompanyIdAndCustomerId(Guid CustomerId,Guid CompanyId)
        {
            string sqlQuery = @"select pinfo.*
                                ,pinfocus.Payfor 
                                ,pinfocus.Type as PaymentMethod 
                                ,pinfocus.Id as PaymentInfoCustomerId
                                from PaymentInfo pinfo 
                                left join PaymentInfoCustomer pinfocus on pinfocus.PaymentInfoId = pinfo.Id
                                where pinfocus.CompanyId =  '{0}'
                                and pinfocus.CustomerId = '{1}'
                                and Payfor !=''";
            try
            {
                sqlQuery = string.Format(sqlQuery, CompanyId, CustomerId);
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


        public DataTable GetLeadPaymentInfoByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId,string Type)
        {
            string sqlQuery = @"select pi.* 
                                from 
	                                PaymentInfoCustomer pic
	                                left join PaymentInfo pi
	                                on pic.PaymentInfoId = pi.Id  
                                where 
	                                pic.Type = '{2}' and
	                                pic.CustomerId = '{1}' and
	                                pic.CompanyId = '{0}'
                                order by pic.Id desc ";
            try
            {
                sqlQuery = string.Format(sqlQuery, CompanyId, CustomerId,Type);
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
        public bool DeletePaymentInfoByIdAndCompanyId(int paymentInfoId, Guid companyId)
        {
            string sqlQuery = @" delete from PaymentInfo where CompanyId ='{1}'  and id ={0} ";
            try
            {
                sqlQuery = string.Format(sqlQuery, paymentInfoId, companyId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
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

        public PaymentInfo GetPaymentInfo_Card_ByCompanyIdAndCustomerId(Guid companyId, Guid customerId)
        {
            string sqlQuery = @"select pinfo.* from PaymentInfo pinfo
                                left join PaymentInfoCustomer pic on pic.PaymentInfoId = pinfo.Id
                                where pic.CompanyId ='{0}'
                                and pic.CustomerId ='{1}'
                                --and pinfo.CardNumber != ''
                                --and pinfo.CardExpireDate !=''
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, customerId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    return ((List<PaymentInfo>)GetList(cmd, 5)).FirstOrDefault();
                     
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetLeadAgreementPaymentInfoByCustomerId(Guid CustomerId)
        {
            string sqlQuery = @"select pay.AccountName, pay.BankAccountType, pay.RoutingNo, pay.AcountNo, Pay.CardNumber, pay.CardType, pay.CardExpireDate, pay.CardSecurityCode, ppc.[Type] from PaymentInfoCustomer pic
                                left join PaymentInfo pay on pay.Id = pic.PaymentInfoId
                                left join PaymentProfileCustomer ppc on ppc.PaymentInfoId = pay.Id
                                where pic.CustomerId = '{0}'
                                and pic.Payfor = 'MMR'
                                group by pay.AccountName, pay.BankAccountType, pay.RoutingNo, pay.AcountNo, Pay.CardNumber, pay.CardType, pay.CardExpireDate, pay.CardSecurityCode, ppc.[Type]";
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId);
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
        public DataTable GetRMRPaymentInfoByCustomerId(Guid CustomerId, Guid CompanyId)
        {
            string sqlQuery = @"select top 1 pic.PaymentInfoId as PaymentId, ppc.Type as PaymentMethod from PaymentInfoCustomer pic
                                join PaymentProfileCustomer ppc on pic.PaymentInfoId = ppc.PaymentInfoId and pic.CustomerId=ppc.CustomerId
                                where pic.CompanyId = '{0}' and pic.CustomerId = '{1}' and pic.Payfor = 'MMR'
                                order by pic.Id desc;
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, CompanyId, CustomerId);
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
