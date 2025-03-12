using HS.DataAccess;
using HS.Entities;
using HS.Entities.Custom;
using HS.Framework;
using HS.Framework.Utils;
using HS.SMS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HS.Facade
{
    public class SMSFacade :BaseFacade
    {
        SMSHistoryDataAccess _SMSHistoryDataAccess = null;
        SMSTemplateDataAccess _SMSTemplateDataAccess = null;
        GlobalSettingDataAccess _GlobalSettingDataAccess = null;

        public SMSFacade(ClientContext clientContext)
            : base(clientContext)
        {
            if (_SMSHistoryDataAccess == null)
                _SMSHistoryDataAccess = (SMSHistoryDataAccess)_ClientContext[typeof(SMSHistoryDataAccess)];
            if (_SMSTemplateDataAccess == null)
                _SMSTemplateDataAccess = (SMSTemplateDataAccess)_ClientContext[typeof(SMSTemplateDataAccess)];
            if (_GlobalSettingDataAccess == null)
                _GlobalSettingDataAccess = (GlobalSettingDataAccess)_ClientContext[typeof(GlobalSettingDataAccess)];
        }
        public SMSFacade(string ConStr)
        {
            if (_SMSHistoryDataAccess == null)
                _SMSHistoryDataAccess = new SMSHistoryDataAccess(ConStr);
            if (_SMSTemplateDataAccess == null)
                _SMSTemplateDataAccess = new SMSTemplateDataAccess(ConStr);
            if (_GlobalSettingDataAccess == null)
                _GlobalSettingDataAccess = new GlobalSettingDataAccess(ConStr);
        }
        //SMSHistoryDataAccess _SMSHistoryDataAccess
        //{
        //    get
        //    {
        //        return (SMSHistoryDataAccess)_ClientContext[typeof(SMSHistoryDataAccess)];
        //    }
        //}
        //SMSTemplateDataAccess _SMSTemplateDataAccess
        //{
        //    get
        //    {
        //        return (SMSTemplateDataAccess)_ClientContext[typeof(SMSTemplateDataAccess)];
        //    }
        //}
        //GlobalSettingDataAccess _GlobalSettingDataAccess
        //{
        //    get
        //    {
        //        return (GlobalSettingDataAccess)_ClientContext[typeof(GlobalSettingDataAccess)];
        //    }
        //}

        public bool SendAgrementSMS(SMSAgreement sms, Guid SendBy, Guid CompanyId,List<string> ReceiverNumberList,bool IsSystemAutoSent, string FromName)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("AgreementLINK", sms.ShortUrl);
                templateVars.Add("ToNumber", sms.ToNumber);
                templateVars.Add("CompanyName", sms.CompanyName);
                if (SendSMS(templateVars, SendBy, SMSTemplateKey.AgreementSMS.SendAgrementSms, CompanyId, ReceiverNumberList, IsSystemAutoSent, FromName))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }

            return false;
        }
        public bool SendFileSMS(SMSFile sms, Guid SendBy, Guid CompanyId, List<string> ReceiverNumberList, bool IsSystemAutoSent, string FromName)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("FileLINK", sms.ShortUrl);
                templateVars.Add("ToNumber", sms.ToNumber);
                templateVars.Add("CompanyName", sms.CompanyName);
                if (sms.IsFileWithoutCustomerSign)
                {
                    if (SendSMS(templateVars, SendBy, SMSTemplateKey.FileSMS.SendFileSmsWithoutCustomerSign, CompanyId, ReceiverNumberList, IsSystemAutoSent, FromName))
                    {
                        return true;
                    }
                }
                else
                {
                    if (SendSMS(templateVars, SendBy, SMSTemplateKey.FileSMS.SendFileSms, CompanyId, ReceiverNumberList, IsSystemAutoSent, FromName))
                    {
                        return true;
                    }
                }
                //if (SendSMS(templateVars, SendBy, SMSTemplateKey.FileSMS.SendFileSms, CompanyId, ReceiverNumberList, IsSystemAutoSent, FromName))
                //{
                //    return true;
                //}
            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }

            return false;
        }

        public bool SendCancellationAgrementSMS(SMSAgreement sms, Guid SendBy, Guid CompanyId, List<string> ReceiverNumberList, bool IsSystemAutoSent, string FromName)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("AgreementLINK", sms.ShortUrl);
                templateVars.Add("ToNumber", sms.ToNumber);
                if (SendSMS(templateVars, SendBy, SMSTemplateKey.CancellationAgreementSMS.AgreementSMS, CompanyId, ReceiverNumberList, IsSystemAutoSent, FromName))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }

            return false;
        }

        public bool SendAddendumSMS(SMSAddendum sms, Guid SendBy, Guid CompanyId, List<string> ReceiverNumberList, bool IsSystemAutoSent, string FromName)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("AgreementLINK", sms.ShortUrl);
                templateVars.Add("ToNumber", sms.ToNumber);
                if (SendSMS(templateVars, SendBy, SMSTemplateKey.AddendumSMS.SMSAddendum, CompanyId, ReceiverNumberList, IsSystemAutoSent, FromName))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }

            return false;
        }

        public bool SendConvertLeadToCustomerSMS(string CustomerName,double TotalRMR, string SalesGroup,string LeadSource,string salesLocation,string ticketType, string SalesPerson , int cusid, double financedamount,Guid CompanyId, List<string> ReceiverNumberList, bool IsSystemAutoSent, string FromName,double UpFront,Guid SendBy)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("LeadName", CustomerName);
                templateVars.Add("TotalRMR", string.Format("${0}", TotalRMR) );
                templateVars.Add("SalesGroup", SalesGroup);
                templateVars.Add("LeadSource", LeadSource);
                templateVars.Add("SalesLocation",salesLocation);
                templateVars.Add("SalesPerson", SalesPerson);
                templateVars.Add("TicketType",ticketType);
                templateVars.Add("CustomerId", cusid);
                templateVars.Add("FinancedAmount", string.Format("${0:0.00}", financedamount));
                templateVars.Add("UpFront", string.Format("${0:0.00}", UpFront));
                //templateVars.Add("ToNumber", sms.ToNumber);
                if (SendSMS(templateVars, SendBy, SMSTemplateKey.ConvertLeadtoCustomerSMS.SendConvertLeadtoCustomerSMS, CompanyId, ReceiverNumberList, IsSystemAutoSent, FromName))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }

            return false;
        }

        public bool SendEstimateSignedSMS(string CustomerName, string EstNumber, double TotalAmount, Guid SendBy, Guid CompanyId, List<string> ReceiverNumberList, bool IsSystemAutoSent, string FromName)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("CustomerName", CustomerName);
                templateVars.Add("EstimateNumber", EstNumber);
                templateVars.Add("EstimateAmount", String.Format("{0:0.00}", TotalAmount));
                //templateVars.Add("ToNumber", sms.ToNumber);
                if (SendSMS(templateVars, SendBy,SMSTemplateKey.EstimateSignedSMS.SendEstimateSignedSMS, CompanyId, ReceiverNumberList, IsSystemAutoSent, FromName))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }

            return false;
        }
        public bool SendActivityNotificationSMS(ActivityNotificationSMS sms, Guid SendBy, Guid CompanyId, List<string> ReceiverNumberList, bool IsSystemAutoSent, string FromName)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("Message", sms.Message);
                templateVars.Add("ToNumber", sms.ReceiverNumber);
                if (SendSMS(templateVars, SendBy, SMSTemplateKey.ActivityNotificationSMS.ActivitySMS, CompanyId, ReceiverNumberList, IsSystemAutoSent, FromName))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }

            return false;
        }
        public bool SendEstimatorSMS(string EstimatorId, Guid CompanyId, List<string> ReceiverNumberList, bool IsSystemAutoSent, string FromName, Guid SendBy, int CustomerId, string CustomerLink, string Status)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("EstimatorId", EstimatorId);
                templateVars.Add("CustomerId", CustomerId);
                templateVars.Add("CustomerLink", CustomerLink);
                templateVars.Add("NAME", FromName);
                templateVars.Add("Status", Status);
                //templateVars.Add("ToNumber", sms.ToNumber);
                if (SendSMS(templateVars, SendBy, SMSTemplateKey.EstimatorSentSMS.SendEstimatorSMS, CompanyId, ReceiverNumberList, IsSystemAutoSent, FromName))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }

            return false;
        }
        public bool SendEstimatorApprovedSMS(string EstimatorId, Guid CompanyId, List<string> ReceiverNumberList, bool IsSystemAutoSent, string FromName, Guid SendBy,int CustomerId,string CustomerLink,string Status)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("EstimatorId", EstimatorId); 
                templateVars.Add("CustomerId", CustomerId); 
                templateVars.Add("CustomerLink", CustomerLink); 
                templateVars.Add("Status", Status); 
                //templateVars.Add("ToNumber", sms.ToNumber);
                if (SendSMS(templateVars, SendBy, SMSTemplateKey.EstimatorApprovedSMS.SendEstimatorApprovedSMS, CompanyId, ReceiverNumberList, IsSystemAutoSent, FromName))
                {
                    return true;
                } 
            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }

            return false;
        }
        public bool SendTicketStatusSMS(int IntTicketId, Guid CompanyId, List<string> ReceiverNumberList, bool IsSystemAutoSent, string FromName, Guid SendBy, int CustomerId, string CustomerLink, string Oldstatus, string Newstatus)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("TicketId", IntTicketId);
                templateVars.Add("oldstatus", Oldstatus);
                templateVars.Add("newstatus", Newstatus);
                templateVars.Add("CustomerId", CustomerId);
                templateVars.Add("CustomerLink", CustomerLink);  
                if (SendSMS(templateVars, SendBy, SMSTemplateKey.TicketStatusSMS.SendTicketStatusSMS, CompanyId, ReceiverNumberList, IsSystemAutoSent, FromName))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }

            return false;
        }
        public bool SendInvSMS(InvoiceSms sms, Guid SendBy,Guid CompanyId, List<string> ReceiverNumberList, bool IsSystemAutoSent, string FromName)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("Message", sms.Message);
            
               
                if (SendSMS(templateVars, SendBy, SMSTemplateKey.RequisitionSMS.SendRequisitionSms, CompanyId, ReceiverNumberList, IsSystemAutoSent, FromName))
                {
                    return true;
                }

                
            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }

            return false;
        }

 
        public bool SendCustomerInfoSMS(CustomerInfoSms sms, Guid SendBy, Guid CompanyId, List<string> ReceiverNumberList, bool IsSystemAutoSent, string FromName)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("Message", sms.Message);


                if (SendSMS(templateVars, SendBy, SMSTemplateKey.CustomerInfoSMS.SendCustomerInfoSms, CompanyId, ReceiverNumberList, IsSystemAutoSent, FromName))
                {
                    return true;
                }


            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }

            return false;
        }
        public bool SendReqSMS(RequisitionSms sms, Guid SendBy, Guid CompanyId, List<string> ReceiverNumberList, bool IsSystemAutoSent, string FromName)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("Message", sms.Message);


                if (SendSMS(templateVars, SendBy, SMSTemplateKey.InvoiceSMS.SendInvoiceSms, CompanyId, ReceiverNumberList, IsSystemAutoSent, FromName))
                {
                    return true;
                }


            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }

            return false;
        }
        public bool SendPurchesOrderSMS(PurchaseOrderSms sms, Guid SendBy, Guid CompanyId, List<string> ReceiverNumberList, bool IsSystemAutoSent, string FromName)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("Message", sms.Message);


                if (SendSMS(templateVars, SendBy,SMSTemplateKey.PurchaseOrderSMS.SendPurchaseOrderSMS, CompanyId, ReceiverNumberList, IsSystemAutoSent, FromName))
                {
                    return true;
                }


            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }

            return false;
        }
        public bool SendSalesSMS(SalesPersonSms sms,Guid SendBy, Guid CompanyId, List<string> ReceiverNumberList, bool IsSystemAutoSent, string FromName)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("Message", sms.Message);


                if (SendSMS(templateVars, SendBy, SMSTemplateKey.SalesSMS.SendSalesSms, CompanyId, ReceiverNumberList, IsSystemAutoSent, FromName))
                {
                    return true;
                }


            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }

            return false;
        }
        public bool SendLeadEstimateAgreement(LeadsEstimateAgree sms,Guid SendBy, Guid CompanyId, List<string> ReceiverNumberList, bool IsSystemAutoSent, string FromName)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("EnvoiceNo", sms.EnvoiceNo);


                if (SendSMS(templateVars, SendBy, SMSTemplateKey.SalesSMS.SendSalesSms, CompanyId, ReceiverNumberList, IsSystemAutoSent, FromName))
                {
                    return true;
                }


            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }

            return false;
        }
        public bool SendSMS(Guid CompanyId,Guid SendBy, string MessageBody, List<string> ReceiverNumberList)
        {
            return SendSMSPrivate(CompanyId, SendBy,"", MessageBody, ReceiverNumberList, false, string.Empty);
        }
        public bool SendSMS(Guid CompanyId, Guid SendBy, string MessageBody, List<string> ReceiverNumberList, bool IsSystemAutoSent, string FromName)
        {
            return SendSMSPrivate(CompanyId, SendBy,"", MessageBody, ReceiverNumberList, IsSystemAutoSent, FromName);
        }
        public bool ReminderFollowupSMS(Guid CompanyId,Guid SendBy, ReminderSms rs, List<string> ReceiverNumberList)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("NoteType", rs.NoteType);
                templateVars.Add("Message", rs.Message);
                templateVars.Add("CustomerName", rs.CustomerName);
                templateVars.Add("CustomerId", rs.CusId.ToString());
                templateVars.Add("CreatedBy", rs.CreatedBy);
                templateVars.Add("CreatedDate", rs.CreatedDate);
                templateVars.Add("AttnBy", rs.AttnBy);
                templateVars.Add("CompanyName", rs.CompanyName);
                if(!string.IsNullOrWhiteSpace(rs.NoteType))
                {
                    if (SendSMS(templateVars, SendBy, SMSTemplateKey.NoteSMS.SendNoteSms, CompanyId, ReceiverNumberList, false, string.Empty))
                    {
                        return true;
                    }
                }
                else
                {
                    if (SendSMS(templateVars, SendBy, SMSTemplateKey.ReminderSMS.SendReminderSms, CompanyId, ReceiverNumberList, false, string.Empty))
                    {
                        return true;
                    }
                }
                


            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }

            return false;
            //return SendSMSPrivate(CompanyId, SendBy, "", MessageBody, ReceiverNumberList, false, string.Empty);
        }
        
        public bool SendTicketStatusToCustomer(Guid CompanyId, Guid SendBy, string message, string cellNumber)
        {
            List<string> ReceiverNumberList = new List<string>();
            ReceiverNumberList.Add(cellNumber);

            return SendSMSPrivate(CompanyId, SendBy, "", message, ReceiverNumberList, false, string.Empty);
        }
        public SMSTemplate GetSMSTemplateByKey(string key)
        {
            return _SMSTemplateDataAccess.GetByQuery($"TemplateKey='{key}'").FirstOrDefault();
        }
        public bool SendSMS(Hashtable TemplateValue,Guid SendBy, string TemplateKey, Guid CompanyId, List<string> ReceiverNumberList, bool IsSystemAutoSent, string FromName)
        {
            SMSTemplate smsTemplate = _SMSTemplateDataAccess.GetByQuery($"TemplateKey='{TemplateKey}'").FirstOrDefault();
            MailMessage message = new MailMessage();
            #region Body
            if (smsTemplate.Body.IndexOf("##") > -1)
            {
                EmailParser BodyParser = new EmailParser(smsTemplate.Body, TemplateValue, false);
                message.Body = BodyParser.Parse();
            }
            else
            {
                message.Body = smsTemplate.Body;
            }
            #endregion
            return SendSMSPrivate(CompanyId, SendBy, TemplateKey, message.Body, ReceiverNumberList, IsSystemAutoSent, FromName);
        }

        public bool SendTestSMS(string bodycontent, Guid Sentby, Guid CompanyId, List<string> ReceiverNumberList, bool IsSystemAutoSent, string FromName,string TemplateKey)
        {
             return SendSMSPrivate(CompanyId, Sentby,TemplateKey, bodycontent, ReceiverNumberList, IsSystemAutoSent, FromName);
        }
        public bool SendSMSPrivate(Guid CompanyId,Guid SendBy, string TemplateKey, string MessageBody, List<string> ReceiverNumberList,bool IsSystemAutoSent,string FromName)
        {

            #region getting autth data
            GlobalSetting aid = _GlobalSettingDataAccess.GetByQuery(string.Format("SearchKey ='{1}' and CompanyId ='{0}'", CompanyId, "PlivoAuthId")).FirstOrDefault();
            if (aid == null)
            {
                return false;
            }
            GlobalSetting atok = _GlobalSettingDataAccess.GetByQuery(string.Format("SearchKey ='{1}' and CompanyId ='{0}'", CompanyId, "PlivoAuthToken")).FirstOrDefault();
            if (atok == null)
            {
                return false;
            }

            GlobalSetting SenderNum = _GlobalSettingDataAccess.GetByQuery(string.Format("SearchKey ='{1}' and CompanyId ='{0}'", CompanyId, "SMSSenderNumber")).FirstOrDefault();
            if (SenderNum == null)
            {
                return false;
            }
            #endregion

            string AuthId = aid.Value;//"MAZGIYZDHKOTY0MWYWMJ";
            string AuthToken = atok.Value;//"MjVkNzY4OWEwMzYzZDhhMGRhMTU2YTI4YjE3MTM4";
            string SenderNumber = SenderNum.Value;// "+18063020602";

            bool result = SMSManager.SendASms(ReceiverNumberList, SenderNumber, MessageBody, AuthId, AuthToken);

            if (result)
            {
                SMSHistory sh = new SMSHistory()
                {
                    FromMobileNo = SenderNumber,
                    IsRead = false,
                    LastUpdatedDate = DateTime.UtcNow,
                    SMSBodyContent = MessageBody,
                    SMSSentDate = DateTime.UtcNow,
                    TemplateKey = TemplateKey,
                    ReadCount = 0,
                    ToMobileNo = string.Join(",", ReceiverNumberList),
                    IsSystemAutoSent = IsSystemAutoSent,
                    FromName = FromName,
                    CompanyId = CompanyId,
                    CreatedBy = SendBy
                  
                };
                _SMSHistoryDataAccess.Insert(sh);
            }
            return result;
        }

        public List<SMSTemplate> GetAllTemplateByCompanyId(Guid CompanyId)
        {
            return _SMSTemplateDataAccess.GetByQuery(string.Format(" CompanyId='{0}'", CompanyId));
        }
        public SMSTemplate GetSMSTemplateById(int id)
        {
            return _SMSTemplateDataAccess.Get(id);
        }
        public bool UpdateSMSTemplate(SMSTemplate et)
        {
            return _SMSTemplateDataAccess.Update(et) > 0;
        }

        public bool SendEstimatorSMS(EstimatorSms sms,Guid SendBy, Guid CompanyId, List<string> ReceiverNumberList, bool IsSystemAutoSent, string FromName)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("Message", sms.Message);


                if (SendSMS(templateVars, SendBy, SMSTemplateKey.EstimatorSMS.SendEstimatorSms, CompanyId, ReceiverNumberList, IsSystemAutoSent, FromName))
                {
                    return true;
                }


            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }

            return false;
        }
    }
}
