using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Facade
{
    public class PaymentInfoFacade : BaseFacade
    {
        public PaymentInfoFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        PaymentInfoDataAccess _PaymentInfoDataAccess
        {
            get
            {
                return (PaymentInfoDataAccess)_ClientContext[typeof(PaymentInfoDataAccess)];
            }
        }
        PaymentProfileCustomerDataAccess _PaymentProfileCustomerDataAccess
        {
            get
            {
                return (PaymentProfileCustomerDataAccess)_ClientContext[typeof(PaymentProfileCustomerDataAccess)];
            }
        }
        AAAAlifSecuirtyCCInfo2DataAccess _AAAAlifSecuirtyCCInfo2DataAccess
        {
            get
            {
                return (AAAAlifSecuirtyCCInfo2DataAccess)_ClientContext[typeof(AAAAlifSecuirtyCCInfo2DataAccess)];
            }
        }
        public long InsertPaymentInfo(PaymentInfo payment)
        {
            if (!string.IsNullOrWhiteSpace(payment.CardNumber))
            {
                payment.CardNumber = DESEncryptionDecryption.EncryptPlainTextToCipherText(payment.CardNumber);
            }
            if (!string.IsNullOrWhiteSpace(payment.CardSecurityCode))
            {
                payment.CardSecurityCode = DESEncryptionDecryption.EncryptPlainTextToCipherText(payment.CardSecurityCode);
            }
            if (!string.IsNullOrWhiteSpace(payment.AcountNo))
            {
                payment.CardNumber = DESEncryptionDecryption.EncryptPlainTextToCipherText(payment.AcountNo);
            }
            return _PaymentInfoDataAccess.Insert(payment);
        }

        public PaymentInfo GetPaymentInfoById(int value)
        {
            PaymentInfo Temp =  _PaymentInfoDataAccess.Get(value);
            if(Temp != null)
            {
                Temp.RoutingNo = DESEncryptionDecryption.DecryptCipherTextToPlainText(Temp.RoutingNo);
                Temp.AcountNo = DESEncryptionDecryption.DecryptCipherTextToPlainText(Temp.AcountNo);
                Temp.CardNumber = DESEncryptionDecryption.DecryptCipherTextToPlainText(Temp.CardNumber);
                Temp.CardSecurityCode = DESEncryptionDecryption.DecryptCipherTextToPlainText(Temp.CardSecurityCode);
            }
            return Temp;
        }

        public PaymentInfo GetPaymentInfoByCompanyIdandLeadId(Guid Companyid, int ID)
        {
            DataTable dt = _PaymentInfoDataAccess.GetPaymentInfoByCompanyIdandLeadId(Companyid, ID);
            PaymentInfo PaymentList = new PaymentInfo();
            PaymentList = (from DataRow dr in dt.Rows
                           select new PaymentInfo()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               CompanyId = (Guid)dr["CompanyId"],
                               AccountName = dr["AccountName"].ToString(),
                               BankAccountType = dr["BankAccountType"].ToString(),
                               RoutingNo = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["RoutingNo"].ToString()),
                               AcountNo = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["AcountNo"].ToString()),
                               CardType = dr["CardType"].ToString(),
                               CardNumber = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardNumber"].ToString()),
                               CardExpireDate = dr["CardExpireDate"].ToString(),
                               CardSecurityCode = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardSecurityCode"].ToString()),
                               BillMethod = dr["BillMethod"].ToString()
                           }).ToList().FirstOrDefault();
            return PaymentList;
        }

        public PaymentInfo GetPaymentInfoByCompanyIdandCustomerId(Guid Companyid, Guid customerId)
        {
            DataTable dt = _PaymentInfoDataAccess.GetPaymentInfoByCompanyIdandCustomerId(Companyid, customerId);
            PaymentInfo PaymentList = new PaymentInfo();
            PaymentList = (from DataRow dr in dt.Rows
                           select new PaymentInfo()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               CompanyId = (Guid)dr["CompanyId"],
                               AccountName = dr["AccountName"].ToString(),
                               BankAccountType = dr["BankAccountType"].ToString(),
                               RoutingNo = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["RoutingNo"].ToString()),
                               AcountNo = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["AcountNo"].ToString()),
                               CardType = dr["CardType"].ToString(),
                               CardNumber = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardNumber"].ToString()),
                               CardExpireDate = dr["CardExpireDate"].ToString(),
                               CardSecurityCode = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardSecurityCode"].ToString()),
                               BillMethod = dr["BillMethod"].ToString()
                           }).ToList().FirstOrDefault();
            return PaymentList;
        }

        public PaymentInfo GetPaymentInfo1ByCompanyIdandLeadId(Guid Companyid, int ID)
        {
            DataTable dt = _PaymentInfoDataAccess.GetPaymentInfo1ByCompanyIdandLeadId(Companyid, ID);
            PaymentInfo PaymentList1 = new PaymentInfo();
            PaymentList1 = (from DataRow dr in dt.Rows
                           select new PaymentInfo()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               CompanyId = (Guid)dr["CompanyId"],
                               AccountName = dr["AccountName"].ToString(),
                               BankAccountType = dr["BankAccountType"].ToString(),
                               RoutingNo = dr["RoutingNo"].ToString(),
                               AcountNo = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["AcountNo"].ToString()),
                               CardType = dr["CardType"].ToString(),
                               CardNumber = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardNumber"].ToString()),
                               CardExpireDate = dr["CardExpireDate"].ToString(),
                               CardSecurityCode = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardSecurityCode"].ToString()),
                               BillMethod = dr["BillMethod"].ToString()
                           }).ToList().FirstOrDefault();
            return PaymentList1;
        }

        public PaymentInfo GetPaymentInfoEFTByCompanyIdandLeadId(Guid Companyid, int ID)
        {
            DataTable dt = _PaymentInfoDataAccess.GetPaymentInfoEFTByCompanyIdandLeadId(Companyid, ID);
            PaymentInfo PaymentListEFT = new PaymentInfo();
            PaymentListEFT = (from DataRow dr in dt.Rows
                            select new PaymentInfo()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                CompanyId = (Guid)dr["CompanyId"],
                                AccountName = dr["AccountName"].ToString(),
                                BankAccountType = dr["BankAccountType"].ToString(),
                                RoutingNo = dr["RoutingNo"].ToString(),
                                AcountNo = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["AcountNo"].ToString()),
                                CardType = dr["CardType"].ToString(),
                                CardNumber = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardNumber"].ToString()),
                                CardExpireDate = dr["CardExpireDate"].ToString(),
                                CardSecurityCode = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardSecurityCode"].ToString()),
                                BillMethod = dr["BillMethod"].ToString()
                            }).ToList().FirstOrDefault();
            return PaymentListEFT;
        }

        public PaymentInfo GetPaymentInfoCreditTByCompanyIdandLeadId(Guid Companyid, int ID)
        {
            DataTable dt = _PaymentInfoDataAccess.GetPaymentInfoCreditTByCompanyIdandLeadId(Companyid, ID);
            PaymentInfo PaymentListCredit = new PaymentInfo();
            PaymentListCredit = (from DataRow dr in dt.Rows
                            select new PaymentInfo()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                CompanyId = (Guid)dr["CompanyId"],
                                AccountName = dr["AccountName"].ToString(),
                                BankAccountType = dr["BankAccountType"].ToString(),
                                RoutingNo = dr["RoutingNo"].ToString(),
                                AcountNo = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["AcountNo"].ToString()),
                                CardType = dr["CardType"].ToString(),
                                CardNumber = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardNumber"].ToString()),
                                CardExpireDate = dr["CardExpireDate"].ToString(),
                                CardSecurityCode = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardSecurityCode"].ToString()),
                                BillMethod = dr["BillMethod"].ToString(),
                            }).ToList().FirstOrDefault();
            return PaymentListCredit;
        }

        public List<PaymentInfo> GetAllPaymentInfoByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId)
        {
            DataTable dt = _PaymentInfoDataAccess.GetAllPaymentInfoByCustomerIdAndCompanyId(CustomerId, CompanyId);
            List<PaymentInfo> PaymentInfoList = new List<PaymentInfo>();
            PaymentInfoList = (from DataRow dr in dt.Rows
                                 select new PaymentInfo()
                                 {
                                     Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                     CompanyId = (Guid)dr["CompanyId"],
                                     AccountName = dr["AccountName"].ToString(),
                                     BankAccountType = dr["BankAccountType"].ToString(),
                                     RoutingNo = dr["RoutingNo"].ToString(),
                                     AcountNo = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["AcountNo"].ToString()),
                                     CardType = dr["CardType"].ToString(),
                                     CardNumber = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardNumber"].ToString()),
                                     CardExpireDate = dr["CardExpireDate"].ToString(),
                                     CardSecurityCode = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardSecurityCode"].ToString()),
                                     PayFor = dr["PayFor"].ToString(),
                                     Type = dr["Type"].ToString(),
                                     PaymentCustomerId = (Guid)dr["PaymentCustomerId"],
                                     CheckNo = dr["CheckNo"].ToString(),
                                     EcheckType = dr["EcheckType"].ToString(),
                                     BankName = dr["BankName"].ToString(),
                                 }).ToList();
            return PaymentInfoList;
        }

        public PaymentInfo GetPaymentInfoCheckByCompanyIdAndLeadId(Guid Companyid, int ID)
        {
            DataTable dt = _PaymentInfoDataAccess.GetPaymentInfoCheckByCompanyIdAndLeadId(Companyid, ID);
            PaymentInfo PaymentListCredit = new PaymentInfo();
            PaymentListCredit = (from DataRow dr in dt.Rows
                                 select new PaymentInfo()
                                 {
                                     Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                     CompanyId = (Guid)dr["CompanyId"],
                                     AccountName = dr["AccountName"].ToString(),
                                     BankAccountType = dr["BankAccountType"].ToString(),
                                     RoutingNo = dr["RoutingNo"].ToString(),
                                     AcountNo = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["AcountNo"].ToString()),
                                     CardType = dr["CardType"].ToString(),
                                     CardNumber = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardNumber"].ToString()),
                                     CardExpireDate = dr["CardExpireDate"].ToString(),
                                     CardSecurityCode = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardSecurityCode"].ToString()),
                                     BillMethod = dr["BillMethod"].ToString(),
                                     CheckNo = dr["CheckNo"].ToString()
                                 }).ToList().FirstOrDefault();
            return PaymentListCredit;
        }

        public PaymentInfo GetPaymentInfoCashByCompanyIdAndLeadId(Guid Companyid, int ID)
        {
            DataTable dt = _PaymentInfoDataAccess.GetPaymentInfoCashByCompanyIdAndLeadId(Companyid, ID);
            PaymentInfo PaymentListCredit = new PaymentInfo();
            PaymentListCredit = (from DataRow dr in dt.Rows
                                 select new PaymentInfo()
                                 {
                                     Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                     CompanyId = (Guid)dr["CompanyId"],
                                     AccountName = dr["AccountName"].ToString(),
                                     BankAccountType = dr["BankAccountType"].ToString(),
                                     RoutingNo = dr["RoutingNo"].ToString(),
                                     AcountNo = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["AcountNo"].ToString()),
                                     CardType = dr["CardType"].ToString(),
                                     CardNumber = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardNumber"].ToString()),
                                     CardExpireDate = dr["CardExpireDate"].ToString(),
                                     CardSecurityCode = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardSecurityCode"].ToString()),
                                     BillMethod = dr["BillMethod"].ToString(),
                                     CheckNo = dr["CheckNo"].ToString(),
                                     IsCash = dr["IsCash"] != DBNull.Value ? Convert.ToBoolean(dr["IsCash"]) : false
                                 }).ToList().FirstOrDefault();
            return PaymentListCredit;
        }

        public bool UpdatePaymentInfo(PaymentInfo payment)
        {
            if (!string.IsNullOrWhiteSpace(payment.CardNumber))
            {
                payment.CardNumber = DESEncryptionDecryption.EncryptPlainTextToCipherText(payment.CardNumber);
            }
            if (!string.IsNullOrWhiteSpace(payment.CardSecurityCode))
            {
                payment.CardSecurityCode = DESEncryptionDecryption.EncryptPlainTextToCipherText(payment.CardSecurityCode);
            }
            if (!string.IsNullOrWhiteSpace(payment.AcountNo))
            {
                payment.CardNumber = DESEncryptionDecryption.EncryptPlainTextToCipherText(payment.AcountNo);
            }
            return _PaymentInfoDataAccess.Update(payment) > 0;
        }

        public bool DeleteAllPaymentInfoByCustomerIdCompanyIdandPaymentInfoId(Guid CompanyId, int ID)
        {
            var result = _PaymentInfoDataAccess.DeleteAllPaymentInfoByCustomerIdCompanyIdandPaymentInfoId(CompanyId, ID);
            return result;
        }

        public PaymentInfo GetOldPaymentInfoByCustomerIdandId(Guid Customerid, int id)
        {
            DataTable dt = _PaymentInfoDataAccess.GetOldPaymentInfoByCustomerIdandId(Customerid, id);
            PaymentInfo OldPaymentList = new PaymentInfo();
            OldPaymentList = (from DataRow dr in dt.Rows
                            select new PaymentInfo()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                CompanyId = (Guid)dr["CompanyId"],
                                AccountName = dr["AccountName"].ToString(),
                                BankAccountType = dr["BankAccountType"].ToString(),
                                RoutingNo = dr["RoutingNo"].ToString(),
                                AcountNo = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["AcountNo"].ToString()),
                                CardType = dr["CardType"].ToString(),
                                CardNumber = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardNumber"].ToString()),
                                CardExpireDate = dr["CardExpireDate"].ToString(),
                                CardSecurityCode = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardSecurityCode"].ToString()),
                                IsForBrinks = dr["IsForBrinks"] != DBNull.Value ? Convert.ToBoolean(dr["IsForBrinks"]) : false,
                                IsInitialPayment = dr["IsInitialPayment"] != DBNull.Value ? Convert.ToBoolean(dr["IsInitialPayment"]) : false,
                                IsPartialPayment = dr["IsPartialPayment"] != DBNull.Value ? Convert.ToBoolean(dr["IsPartialPayment"]) : false

                            }).ToList().FirstOrDefault();
            return OldPaymentList;
        }
        public PaymentInfo GetOldPaymentInfoByCustomerIdOnly(Guid Customerid)
        {
            DataTable dt = _PaymentInfoDataAccess.GetOldPaymentInfoByCustomerIdOnly(Customerid);
            PaymentInfo OldPaymentList = new PaymentInfo();
            OldPaymentList = (from DataRow dr in dt.Rows
                              select new PaymentInfo()
                              {
                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                  CompanyId = (Guid)dr["CompanyId"],
                                  AccountName = dr["AccountName"].ToString(),
                                  BankAccountType = dr["BankAccountType"].ToString(),
                                  RoutingNo = dr["RoutingNo"].ToString(),
                                  AcountNo = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["AcountNo"].ToString()),
                                  CardType = dr["CardType"].ToString(),
                                  CardNumber = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardNumber"].ToString()),
                                  CardExpireDate = dr["CardExpireDate"].ToString(),
                                  CardSecurityCode = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardSecurityCode"].ToString()),

                              }).FirstOrDefault();
            return OldPaymentList;
        }
        public PaymentInfo GetOldPaymentInfoByCustomerId(Guid Customerid,int Id)
        {
            DataTable dt = _PaymentInfoDataAccess.GetOldPaymentInfoByCustomerId(Customerid, Id);
            PaymentInfo OldPaymentList = new PaymentInfo();
            OldPaymentList = (from DataRow dr in dt.Rows
                              select new PaymentInfo()
                              {
                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                  CompanyId = (Guid)dr["CompanyId"],
                                  AccountName = dr["AccountName"].ToString(),
                                  BankAccountType = dr["BankAccountType"].ToString(),
                                  RoutingNo = dr["RoutingNo"].ToString(),
                                  AcountNo = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["AcountNo"].ToString()),
                                  CardType = dr["CardType"].ToString(),
                                  CardNumber = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardNumber"].ToString()),
                                  CardExpireDate = dr["CardExpireDate"].ToString(),
                                  ZipCode = dr["ZipCode"].ToString(),
                                  State = dr["State"].ToString(),
                                  City = dr["City"].ToString(),
                                  Street = dr["Street"].ToString(),
                                  CardSecurityCode = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardSecurityCode"].ToString()),
                                  IsForBrinks = dr["IsForBrinks"] != DBNull.Value ? Convert.ToBoolean(dr["IsForBrinks"]) : false,
                                  IsInitialPayment = dr["IsInitialPayment"] != DBNull.Value ? Convert.ToBoolean(dr["IsInitialPayment"]) : false,
                                  IsPartialPayment = dr["IsPartialPayment"] != DBNull.Value ? Convert.ToBoolean(dr["IsPartialPayment"]) : false
                              }).FirstOrDefault();
            return OldPaymentList;
        }
        public PaymentInfo GetPaymentInfoByIdAndCompanyId(int Id, Guid CompanyId)
        {
            PaymentInfo PaymentInfo =  _PaymentInfoDataAccess.GetByQuery(string.Format("Id = '{0}' and CompanyId = '{1}'",Id,CompanyId)).FirstOrDefault();
            PaymentInfo.AcountNo = DESEncryptionDecryption.DecryptCipherTextToPlainText(PaymentInfo.AcountNo);
            PaymentInfo.CardNumber = DESEncryptionDecryption.DecryptCipherTextToPlainText(PaymentInfo.CardNumber);
            PaymentInfo.CardSecurityCode = DESEncryptionDecryption.DecryptCipherTextToPlainText(PaymentInfo.CardSecurityCode);
            return PaymentInfo;
        }

        //public PaymentInfo GetMMRPaymentInfoByCustomerIdAndCompanyId(Guid CustomerId,Guid CompanyId)
        //{
        //    DataTable dt = _PaymentInfoDataAccess.GetMMRPaymentInfoByCustomerIdAndCompanyId(CustomerId, CompanyId);
        //    PaymentInfo OldPaymentList = new PaymentInfo();
        //    OldPaymentList = (from DataRow dr in dt.Rows
        //                      select new PaymentInfo()
        //                      {
        //                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
        //                          CompanyId = dr["CompanyId"] != DBNull.Value ? (Guid)dr["CompanyId"] : new Guid(),
        //                          AccountName = dr["AccountName"].ToString(),
        //                          BankAccountType = dr["BankAccountType"].ToString(),
        //                          RoutingNo = dr["RoutingNo"].ToString(),
        //                          AcountNo = dr["AcountNo"].ToString(),
        //                          CardType = dr["CardType"].ToString(),
        //                          CardNumber = dr["CardNumber"].ToString(),
        //                          CardExpireDate = dr["CardExpireDate"].ToString(),
        //                          CardSecurityCode = dr["CardSecurityCode"].ToString(),
        //                          CheckNo = dr["CheckNo"].ToString()
        //                      }).ToList().FirstOrDefault();
        //    return OldPaymentList;
        //}


        public List<PaymentInfo> GetPaymentInfoListByCompanyIdAndCustomerId(Guid CompanyId, Guid CustomerId)
        { 
            DataTable dt = _PaymentInfoDataAccess.GetPaymentInfoListByCompanyIdAndCustomerId(CustomerId, CompanyId);
            List<PaymentInfo> OldPaymentList = new List<PaymentInfo>();
            OldPaymentList = (from DataRow dr in dt.Rows
                              select new PaymentInfo()
                              {
                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                  CompanyId = dr["CompanyId"] != DBNull.Value ? (Guid)dr["CompanyId"] : new Guid(),
                                  AccountName = dr["AccountName"].ToString(),
                                  BankAccountType = dr["BankAccountType"].ToString(),
                                  RoutingNo = dr["RoutingNo"].ToString(),
                                  AcountNo = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["AcountNo"].ToString()),
                                  CardType = dr["CardType"].ToString(),
                                  CardNumber = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardNumber"].ToString()),
                                  CardExpireDate = dr["CardExpireDate"].ToString(),
                                  CardSecurityCode = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardSecurityCode"].ToString()),
                                  CheckNo = dr["CheckNo"].ToString(),
                                  FileName = dr["FileName"].ToString(),
                                  PayFor = dr["PayFor"].ToString(),
                                  PaymentMethod = dr["PaymentMethod"].ToString(),
                                  PaymentInfoCustomerId = dr["PaymentInfoCustomerId"] != DBNull.Value ? Convert.ToInt32(dr["PaymentInfoCustomerId"]) : 0,
                              }).ToList();
            return OldPaymentList;
        }

        public PaymentInfo GetLeadPaymentInfoByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId,string Type)
        {
            DataTable dt = _PaymentInfoDataAccess.GetLeadPaymentInfoByCustomerIdAndCompanyId(CustomerId, CompanyId,Type);
            PaymentInfo OldPaymentList = new PaymentInfo();
            OldPaymentList = (from DataRow dr in dt.Rows
                              select new PaymentInfo()
                              {
                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                  CompanyId = dr["CompanyId"] != DBNull.Value ? (Guid)dr["CompanyId"] : new Guid(),
                                  AccountName = dr["AccountName"].ToString(),
                                  BankAccountType = dr["BankAccountType"].ToString(),
                                  RoutingNo = dr["RoutingNo"].ToString(),
                                  AcountNo = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["AcountNo"].ToString()),
                                  CardType = dr["CardType"].ToString(),
                                  CardNumber = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardNumber"].ToString()),
                                  CardExpireDate = dr["CardExpireDate"].ToString(),
                                  CardSecurityCode = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardSecurityCode"].ToString()),
                                  CheckNo = dr["CheckNo"].ToString()
                              }).ToList().FirstOrDefault();
            return OldPaymentList;
        }

        public bool DeletePaymentInfoById(int payid)
        {
            return _PaymentInfoDataAccess.DeletePaymentInfoById(payid);
        }

        public PaymentInfo GetPaymentInfo_Card_ByCompanyIdAndCustomerId(Guid CompanyId,Guid CustomerId)
        {
            PaymentInfo PaymentInfo =  _PaymentInfoDataAccess.GetPaymentInfo_Card_ByCompanyIdAndCustomerId(CompanyId, CustomerId);
            PaymentInfo.CardNumber = DESEncryptionDecryption.DecryptCipherTextToPlainText(PaymentInfo.CardNumber);
            //PaymentInfo.CardExpireDate = DESEncryptionDecryption.DecryptCipherTextToPlainText(PaymentInfo.CardExpireDate);
            PaymentInfo.CardSecurityCode = DESEncryptionDecryption.DecryptCipherTextToPlainText(PaymentInfo.CardSecurityCode);
            PaymentInfo.AcountNo = DESEncryptionDecryption.DecryptCipherTextToPlainText(PaymentInfo.AcountNo);
            PaymentInfo.RoutingNo = DESEncryptionDecryption.DecryptCipherTextToPlainText(PaymentInfo.RoutingNo);
            return PaymentInfo;
        }

        public bool DeletePaymentInfoByIdAndCompanyId(int paymentInfoId, Guid CompanyId)
        {
            return _PaymentInfoDataAccess.DeletePaymentInfoByIdAndCompanyId(paymentInfoId,CompanyId);
        }

        public void DeletePaymentProfile(int value)
        {
            _PaymentProfileCustomerDataAccess.Delete(value);
        }

        public List<PaymentInfo> GetLeadAgreementPaymentInfoByCustomerId(Guid CustomerId)
        {
            DataTable dt = _PaymentInfoDataAccess.GetLeadAgreementPaymentInfoByCustomerId(CustomerId);
            List<PaymentInfo> OldPaymentList = new List<PaymentInfo>();
            OldPaymentList = (from DataRow dr in dt.Rows
                              select new PaymentInfo()
                              {
                                  AccountName = dr["AccountName"].ToString(),
                                  BankAccountType = dr["BankAccountType"].ToString(),
                                  RoutingNo = dr["RoutingNo"].ToString(),
                                  AcountNo = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["AcountNo"].ToString()),
                                  CardType = dr["CardType"].ToString(),
                                  CardNumber = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardNumber"].ToString()),
                                  CardExpireDate = dr["CardExpireDate"].ToString(),
                                  CardSecurityCode = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardSecurityCode"].ToString()),
                                  Type = dr["Type"].ToString()
                              }).ToList();
            return OldPaymentList;
        }

        public PaymentProfileCustomer GetPaymentProfileCustomerByType(string type)
        {
            return _PaymentProfileCustomerDataAccess.GetByQuery(string.Format("[Type] = '{0}' order by Id desc", type)).FirstOrDefault();
        }
        public string GetPaymentProfileCustomerByPaymentInfoId(int paymentInfoId)
        {
            return _PaymentProfileCustomerDataAccess.GetByQuery(string.Format("[PaymentInfoId] = '{0}'", paymentInfoId)).Select( x => x.Type ).FirstOrDefault();
        }
        public RecurringBillingPaymentInfoModel GetPaymentProfileCustomerForRMRByCustomerId(Guid customerId, Guid CompanyId)
        {
            DataTable dt = _PaymentInfoDataAccess.GetRMRPaymentInfoByCustomerId(customerId, CompanyId);
            RecurringBillingPaymentInfoModel OldPayment = new RecurringBillingPaymentInfoModel();
            if (dt != null && dt.Rows.Count > 0)
            {
                    DataRow row = dt.Rows[0];
                OldPayment = new RecurringBillingPaymentInfoModel()
                    {
                        PaymentId = row["PaymentId"] != DBNull.Value ? Convert.ToInt32(row["PaymentId"]) : 0,
                        PaymentMethod = row["PaymentMethod"].ToString()
                    };
            }
            return OldPayment;
        } 
        public List<AAAAlifSecuirtyCCInfo2> GetAllAAAAlifSecuirtyCCInfo2s()
        {
            return _AAAAlifSecuirtyCCInfo2DataAccess.GetAll();
        }

        public bool UpdateAAAAlifSecuirtyCCInfo2(AAAAlifSecuirtyCCInfo2 data)
        {
           return _AAAAlifSecuirtyCCInfo2DataAccess.Update(data) > 0;
        }
        public int GetPaymentInfoIdByPaymentProfileCustomerType(string paymentMethod)
        {
            return _PaymentProfileCustomerDataAccess.GetByQuery(string.Format("[Type] = '{0}'", paymentMethod)).Select(x => x.PaymentInfoId).FirstOrDefault();
        }
    }
}
