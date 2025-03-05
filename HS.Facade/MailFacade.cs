using System;
using HS.Entities;
using System.Collections;
using HS.Framework.Utils;
using HS.DataAccess;
using System.Web;
using HS.Framework;
using System.Net.Mail;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using HS.MailGun.Core.Messages;
using System.IO;
using System.Net.Http;
namespace HS.Facade
{
    public class MailFacade : BaseFacade
    {
        EmailTemplateDataAccess _EmailTemplateDataAccess = null;
        EmailHistoryDataAccess _EmailHistoryDataAccess = null;
        UserLoginDataAccess _UserLoginDataAccess = null;
        GlobalSettingDataAccess _GlobalSettingDataAccess = null; 
        CompanyBranchDataAccess _CompanyBranchDataAccess = null;
        CompanyDataAccess _CompanyDataAccess = null;
        LeadCorrespondenceDataAccess _LeadCorrespondenceDataAccess = null;
        ActivityDataAccess _ActivityDataAccess = null;
        public MailFacade() { 
        } 
        #region Init With Connectionstring
        public MailFacade(string ConnectionString)
        {
            if (_GlobalSettingDataAccess == null)
                _GlobalSettingDataAccess = new GlobalSettingDataAccess(ConnectionString); 
            if (_UserLoginDataAccess == null)
                _UserLoginDataAccess = new UserLoginDataAccess(ConnectionString);
            if (_EmailTemplateDataAccess == null)
                _EmailTemplateDataAccess = new EmailTemplateDataAccess(ConnectionString);
            if (_EmailHistoryDataAccess == null)
                _EmailHistoryDataAccess = new EmailHistoryDataAccess(ConnectionString);
            if (_CompanyBranchDataAccess == null)
                _CompanyBranchDataAccess = new CompanyBranchDataAccess(ConnectionString);
            if (_CompanyDataAccess == null)
                _CompanyDataAccess = new CompanyDataAccess(ConnectionString);
            if (_LeadCorrespondenceDataAccess == null)
                _LeadCorrespondenceDataAccess = new LeadCorrespondenceDataAccess(ConnectionString);
            if (_ActivityDataAccess == null)
                _ActivityDataAccess = new ActivityDataAccess(ConnectionString);
        }

        #endregion

        public MailFacade(ClientContext clientContext)
           : base(clientContext)
        {
            if (_UserLoginDataAccess == null)
                _UserLoginDataAccess = (UserLoginDataAccess)_ClientContext[typeof(UserLoginDataAccess)];
            if (_EmailTemplateDataAccess == null)
                _EmailTemplateDataAccess = (EmailTemplateDataAccess)_ClientContext[typeof(EmailTemplateDataAccess)];
            if (_EmailHistoryDataAccess == null)
                _EmailHistoryDataAccess = (EmailHistoryDataAccess)_ClientContext[typeof(EmailHistoryDataAccess)];
            if (_GlobalSettingDataAccess == null)
                _GlobalSettingDataAccess = (GlobalSettingDataAccess)_ClientContext[typeof(GlobalSettingDataAccess)];
            if (_CompanyBranchDataAccess == null)
                _CompanyBranchDataAccess = (CompanyBranchDataAccess)_ClientContext[typeof(CompanyBranchDataAccess)];
            if (_CompanyDataAccess == null)
                _CompanyDataAccess = (CompanyDataAccess)_ClientContext[typeof(CompanyDataAccess)];
            if(_LeadCorrespondenceDataAccess == null)
                _LeadCorrespondenceDataAccess = (LeadCorrespondenceDataAccess)_ClientContext[typeof(LeadCorrespondenceDataAccess)];
            if (_ActivityDataAccess == null)
                _ActivityDataAccess = (ActivityDataAccess)_ClientContext[typeof(ActivityDataAccess)]; 
        }

        public bool SentEmail(Hashtable TemplateValue, string TemplateKey, Guid CompanyId, List<Attachment> Attachments, string from = "")
        {

            #region Common Templates
            var SiteDomain = ConfigurationManager.AppSettings["SiteDomain"];
            var FacebookTemplate = "<a href='{0}'><img style='height:30px;' src='{1}/Content/Icons/Social/facebook_circle.png' /></a>";
            var InstagramTemplate = "<a href='{0}'><img style='height: 30px;' src='{1}/Content/Icons/Social/instagram_circle.png' /></a>";
            var YoutubeTemplate = "<a href='{0}'><img style='height:30px;' src='{1}/Content/Icons/Social/youtube_circle.png' /></a>";
            var FacebookLink = GetFacebookUrlByCompanyId(CompanyId);
            var InstagramLink = GetInstagramUrlByCompanyId(CompanyId);
            var YoutubeLink = GetYoutubeUrlByCompanyId(CompanyId);
            if (!string.IsNullOrEmpty(FacebookLink))
            {
                FacebookTemplate = string.Format(FacebookTemplate, FacebookLink, SiteDomain);
                TemplateValue.Add("FacebookDiv", FacebookTemplate);
            }
            if (!string.IsNullOrEmpty(InstagramLink))
            {
                InstagramTemplate = string.Format(InstagramTemplate, InstagramLink, SiteDomain);
                TemplateValue.Add("InstagramDiv", InstagramTemplate);
            }
            if (!string.IsNullOrEmpty(YoutubeLink))
            {
                YoutubeTemplate = string.Format(YoutubeTemplate, YoutubeLink, SiteDomain);
                TemplateValue.Add("YoutubeDiv", YoutubeTemplate);
            }

            if (TemplateValue["Logo"] == null)
            {
                TemplateValue.Add("Logo", GetEmailLogoByCompanyId(CompanyId));
            }
            if (TemplateValue["TeamNameSignature"] == null)
            {
                TemplateValue.Add("TeamNameSignature", GetTeamNameSignatureByCompanyId(CompanyId));
            }
            if (TemplateValue["CompanyNameAlt"] == null)
            {
                TemplateValue.Add("CompanyNameAlt", GetCompanyNameByCompanyId(CompanyId));
            }
            if (TemplateValue["CompanyInformation"] == null)
            {
                string Footer = GetFooterCompanyInformationByCompanyId(CompanyId);
                if (Footer.IndexOf("##Year##") > -1)
                {
                    Footer = Footer.Replace("##Year##","2017-"+ DateTime.Now.Year.ToString());
                }
                TemplateValue.Add("CompanyInformation", Footer);
            }
            #endregion

            EmailTemplate emailTemplate = _EmailTemplateDataAccess.GetByQuery("TemplateKey='" + TemplateKey + "'").FirstOrDefault();
            //if(emailTemplate == null)
            //{
            //    try
            //    {
            //        using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\ErrorReports\EmailError.txt"), true))
            //        {
            //            file.WriteLine(string.Format("{0} Template Not Found. Template Key: {1}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"), TemplateKey));

            //        }
            //    }
            //    catch (Exception) { }
            //}
            EmailParser parser = null;
            string toEmailAddress = "";
            string FromName = "";
   
            #region BodyFile And BodyContent
            if (emailTemplate == null || emailTemplate.Id == 0)
            {
                //Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception("Template not found for key :" + TemplateKey));
                return false;
            }
            if (!string.IsNullOrWhiteSpace(emailTemplate.BodyContent))
            {
                parser = new EmailParser(emailTemplate.BodyContent, TemplateValue, false);
            }
            else if (!string.IsNullOrWhiteSpace(emailTemplate.BodyFile))
            {
                parser = new EmailParser(System.Web.Hosting.HostingEnvironment.MapPath(emailTemplate.BodyFile), TemplateValue, true);
            }
            #endregion

            MailMessage message = new MailMessage();
            message.Body = parser.Parse();
            
            #region ToEmail
            if (emailTemplate.ToEmail.IndexOf("##") > -1)
            {
                EmailParser ToEmailParser = new EmailParser(emailTemplate.ToEmail, TemplateValue, false);
                string EmailAddress = ToEmailParser.Parse();
                if (EmailAddress.IsValidEmailAddress())
                {
                    message.To.Add(new MailAddress(EmailAddress));
                    toEmailAddress = message.To[0].ToString();
                }else
                {
                    EmailAddress = EmailAddress.Replace(" ", "");
                    toEmailAddress = EmailAddress;
                    if (EmailAddress.Split(',').Count()>1)
                    { 
                        string[] addList = EmailAddress.Split(',');
                        foreach(var item in addList)
                        {
                            if (item.IsValidEmailAddress())
                            {
                                message.To.Add(new MailAddress(item));
                            }else if (item.Split('-').Count() > 0 && item.Split('-')[0].IsValidEmailAddress())
                            {
                                message.To.Add(new MailAddress(item.Split('-')[0], item.Split('-')[1]));
                            }
                        }
                    }
                    else if(EmailAddress.Split(';').Count() > 1)
                    {
                        string[] addList = EmailAddress.Split(';');
                        foreach (var item in addList)
                        {
                            if (item.IsValidEmailAddress())
                            {
                                message.To.Add(new MailAddress(item));
                            }
                            else if (item.Split('>').Count() > 0 && item.Split('>')[0].IsValidEmailAddress())
                            {
                                message.To.Add(new MailAddress(item.Split('>')[0], item.Split('>')[1]));
                            }
                        }
                    }
                    if(message.To.Count == 0)
                    {
                        return false;
                    }

                }
            }
            else
            {
                message.To.Add(emailTemplate.ToEmail);
                toEmailAddress = message.To[0].ToString();
            }
            #endregion

            #region FromName
            if (!string.IsNullOrWhiteSpace(emailTemplate.FromName) && emailTemplate.FromName.IndexOf("##") > -1)
            {
                try
                {
                    EmailParser FromNameParser = new EmailParser(emailTemplate.FromName, TemplateValue, false);
                    FromName = FromNameParser.Parse();
                }catch(Exception)
                {
                    FromName = "rmrcloud.com";
                }
            }
            else
            {
                FromName = emailTemplate.FromName;
            }
            #endregion

            #region Reply Email
            if (emailTemplate.ReplyEmail.IndexOf("##") > -1)
            {
                EmailParser ReplyEmailParser = new EmailParser(emailTemplate.ReplyEmail, TemplateValue, false);
                //message.ReplyToList.Add(new MailAddress(ReplyEmailParser.Parse(), FromName));


                string ReplyEmailAddress = ReplyEmailParser.Parse();
                if (ReplyEmailAddress.IsValidEmailAddress())
                {
                    message.ReplyToList.Add(new MailAddress(ReplyEmailAddress, FromName));
                }
                else
                {
                    ReplyEmailAddress = ReplyEmailAddress.Replace(" ", "");
                    //toEmailAddress = EmailAddress;
                    if (ReplyEmailAddress.Split(',').Count() > 1)
                    {
                        string[] addList = ReplyEmailAddress.Split(',');
                        foreach (var item in addList)
                        {
                            if (item.IsValidEmailAddress())
                            {
                                message.ReplyToList.Add(new MailAddress(item, FromName));
                            }
                            else if (item.Split('-').Count() > 0 && item.Split('-')[0].IsValidEmailAddress())
                            {
                                message.ReplyToList.Add(new MailAddress(item.Split('-')[0], item.Split('-')[1]));
                            }
                            else if (item.Split('>').Count() > 0 && item.Split('>')[0].IsValidEmailAddress())
                            {
                                message.ReplyToList.Add(new MailAddress(item.Split('>')[0], item.Split('>')[1]));
                            }
                        }
                    }
                    else if (ReplyEmailAddress.Split(';').Count() > 1)
                    {
                        string[] addList = ReplyEmailAddress.Split(';');
                        foreach (var item in addList)
                        {
                            if (item.IsValidEmailAddress())
                            {
                                message.ReplyToList.Add(new MailAddress(item,FromName));
                            }
                            else if (item.Split('>').Count() > 0 && item.Split('>')[0].IsValidEmailAddress())
                            {
                                message.ReplyToList.Add(new MailAddress(item.Split('>')[0], item.Split('>')[1]));
                            }
                            else if (item.Split('-').Count() > 0 && item.Split('-')[0].IsValidEmailAddress())
                            {
                                message.ReplyToList.Add(new MailAddress(item.Split('-')[0], item.Split('-')[1]));
                            }
                        }
                    }
                } 
            }
            else if(emailTemplate.ReplyEmail.IsValidEmailAddress())
            {
                message.ReplyToList.Add(new MailAddress(emailTemplate.ReplyEmail, FromName));
            }
            #endregion
            
            #region From Email
            if (emailTemplate.FromEmail.IndexOf("##") > -1)
            {
                EmailParser FromEmailParser = new EmailParser(emailTemplate.FromEmail, TemplateValue, false);
                message.From = new MailAddress(FromEmailParser.Parse(), FromName);
            }
            else
            {
                message.From = new MailAddress(emailTemplate.FromEmail, FromName); 
            }
            #endregion

            #region CC & BCC
            if (!string.IsNullOrWhiteSpace(emailTemplate.BccEmail))
            {
                if (emailTemplate.BccEmail.IndexOf("##") > -1)
                {
                    EmailParser BCCEmailParser = new EmailParser(emailTemplate.BccEmail, TemplateValue, false);
                    emailTemplate.BccEmail = BCCEmailParser.Parse();
                }
                if (emailTemplate.BccEmail.IndexOf(";") > -1)
                {
                    var ArrBcc = emailTemplate.BccEmail.Split(';');
                    foreach (var item in ArrBcc)
                    {
                        message.Bcc.Add(item);
                    }
                }
                else
                {
                    message.Bcc.Add(emailTemplate.BccEmail);
                }
            }
            else if (TemplateValue["BccEmail"] != null)
            {
                if (!string.IsNullOrWhiteSpace(TemplateValue["BccEmail"].ToString()))
                {
                    if (TemplateValue["BccEmail"].ToString().IndexOf(";") > -1)
                    {
                        var ArrCcEmail = TemplateValue["BccEmail"].ToString().Split(';');
                        foreach (var item in ArrCcEmail)
                        {
                            if (!string.IsNullOrEmpty(item) && item.IsValidEmailAddress())
                            {
                                message.Bcc.Add(item);
                            }
                        }
                    }
                    else if (TemplateValue["BccEmail"].ToString().IsValidEmailAddress())
                    {
                        message.Bcc.Add(TemplateValue["BccEmail"].ToString());
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(emailTemplate.CcEmail))
            {
                if (emailTemplate.CcEmail.IndexOf("##") > -1)
                {
                    EmailParser CCEmailParser = new EmailParser(emailTemplate.CcEmail, TemplateValue, false);
                    emailTemplate.CcEmail = CCEmailParser.Parse();
                }
                if (emailTemplate.CcEmail.IndexOf(";") > -1)
                {
                    var ArrCcEmail = emailTemplate.CcEmail.Split(';');
                    foreach (var item in ArrCcEmail)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            message.CC.Add(item);
                        }
                    }
                }
                else if(emailTemplate.CcEmail.IsValidEmailAddress())
                {
                    message.CC.Add(emailTemplate.CcEmail);
                }
            }
            else if (TemplateValue["CCEmail"] != null)
            {
                if (!string.IsNullOrWhiteSpace(TemplateValue["CCEmail"].ToString()))
                {
                    if (TemplateValue["CCEmail"].ToString().IndexOf(";") > -1)
                    {
                        var ArrCcEmail = TemplateValue["CCEmail"].ToString().Split(';');
                        foreach (var item in ArrCcEmail)
                        {
                            if (!string.IsNullOrEmpty(item) && item.IsValidEmailAddress())
                            {
                                message.CC.Add(item);
                            }
                        }
                    }
                    else if (TemplateValue["CCEmail"].ToString().IsValidEmailAddress())
                    {
                        message.CC.Add(TemplateValue["CCEmail"].ToString());
                    }
                }
            }

            #endregion

            #region Subject
            if (TemplateValue["Subject"] != null)
            {
                message.Subject = TemplateValue["Subject"].ToString();
            }
            else
            {
                if (emailTemplate.Subject.IndexOf("##") > -1)
                {
                    EmailParser SubjectParser = new EmailParser(emailTemplate.Subject, TemplateValue, false);
                    message.Subject = SubjectParser.Parse();
                }
                else
                {
                    message.Subject = emailTemplate.Subject;
                }
            }
            
            #endregion

            #region Attachments
            if (Attachments != null)
            {
                if (Attachments.Count > 0)
                {
                    foreach (var attachment in Attachments)
                    {
                        if(attachment!=null)
                        {
                            message.Attachments.Add(attachment);
                        }
                    }
                }
            }
            #endregion

            message.IsBodyHtml = true;
            message.BodyEncoding = System.Text.Encoding.UTF8;
             
            try
            {

                if (HttpContext.Current == null || HttpContext.Current.Request == null
                    || !HttpContext.Current.Request.IsLocal
                    )
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                      | SecurityProtocolType.Tls11
                                      | SecurityProtocolType.Tls12;

                    SmtpClient client = new SmtpClient();
                    GlobalSetting EmailHost = _GlobalSettingDataAccess.GetByQuery(string.Format(" SearchKey ='{0}' and CompanyId ='{1}'", "EmailHost", CompanyId)).FirstOrDefault();
                    GlobalSetting EmailHostUsername = _GlobalSettingDataAccess.GetByQuery(string.Format(" SearchKey ='{0}' and CompanyId ='{1}'", "EmailHostUsername", CompanyId)).FirstOrDefault();
                    GlobalSetting EmailHostPassword = _GlobalSettingDataAccess.GetByQuery(string.Format(" SearchKey ='{0}' and CompanyId ='{1}'", "EmailHostPassword", CompanyId)).FirstOrDefault();
                    GlobalSetting EmailPort = _GlobalSettingDataAccess.GetByQuery(string.Format(" SearchKey ='{0}' and CompanyId ='{1}'", "EmailPort", CompanyId)).FirstOrDefault();

                    if (EmailHost != null && !string.IsNullOrWhiteSpace(EmailHost.Value))
                    {
                        client.Host = EmailHost.Value;
                    }
                    if (EmailHostUsername != null && !string.IsNullOrWhiteSpace(EmailHostUsername.Value)
                        && EmailHostPassword != null && !string.IsNullOrWhiteSpace(EmailHostPassword.Value))
                    {
                        client.Credentials = new System.Net.NetworkCredential(EmailHostUsername.Value, EmailHostPassword.Value);
                    }
                    if (EmailPort != null && !string.IsNullOrWhiteSpace(EmailPort.Value))
                    {
                        int port = 587;
                        if (int.TryParse(EmailPort.Value, out port))
                        {
                            client.Port = port;
                        }
                    }
                    client.EnableSsl = false;

                    //message.From = new MailAddress("noreply@rmrcloud.com");
                    //message.From = new MailAddress("info@marketing.centralstationmarketing.com");
                    //Need to user From email of default domain

                    client.Send(message);
                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\FileMailError.txt"), true))
                {
                    file.WriteLine("insert");
                    file.Close();
                }
                #region No need now
                //if (!HttpContext.Current.Request.IsLocal)
                //{
                //    SmtpClient client = new SmtpClient();
                //    client.Credentials = new System.Net.NetworkCredential("noreply@piiscenter.com", "piiscenter.com");
                //    client.EnableSsl = false;
                //    client.Send(message);
                //}
                //SmtpClient smtp = new SmtpClient
                //{
                //    Host = "smtp.gmail.com",
                //    //change the port to prt 587. This seems to be the standard for Google smtp transmissions.
                //    Port = 587,
                //    //enable SSL to be true, otherwise it will get kicked back by the Google server.
                //    EnableSsl = true,
                //    //The following properties need set as well
                //    DeliveryMethod = SmtpDeliveryMethod.Network,
                //    UseDefaultCredentials = false,
                //    Credentials = new NetworkCredential("informrcloud@gmail.com", "Inf0rmrcl@ud")
                //};
                //smtp.Send(message);
                #endregion
                #region Insert into Email History table
                EmailHistory emailHistory = new EmailHistory();
                emailHistory.TemplateKey = TemplateKey;
                emailHistory.ToEmail = toEmailAddress;
                emailHistory.CcEmail = message.CC.ToString();
                emailHistory.BccEmail = message.Bcc.ToString();
                emailHistory.FromEmail = message.From.ToString();
                emailHistory.EmailBodyContent = message.Body;
                emailHistory.EmailSubject = message.Subject;
                emailHistory.EmailSentDate = DateTime.Now;
                emailHistory.LastUpdatedDate = DateTime.Now;
                _EmailHistoryDataAccess.Insert(emailHistory);
                #endregion
                #region LeadCorrespondence
                if (TemplateKey == "InvoiceEmail"
                    || TemplateKey == "EstimateEmail" 
                    || TemplateKey == "EmailtoLeadsAggrement" 
                    || TemplateKey == "mailtoCustomerforTransaction" 
                    || TemplateKey == "mailToLeadSignAgreement"
                    || TemplateKey == "FileManagementMail"
                    || TemplateKey == "FileManagementConfirmationMail")
                {
                    string CustomerId = "00000000-0000-0000-0000-000000000000";
                    string EmployeeId = "00000000-0000-0000-0000-000000000000";
                    if(TemplateValue["CustomerId"] != null &&  !string.IsNullOrWhiteSpace(TemplateValue["CustomerId"].ToString()))
                    {
                        CustomerId = TemplateValue["CustomerId"].ToString();
                    }
                    if(TemplateValue["EmployeeId"] != null && !string.IsNullOrWhiteSpace(TemplateValue["EmployeeId"].ToString()))
                    {
                        EmployeeId = TemplateValue["EmployeeId"].ToString();
                    }
                    LeadCorrespondence objcorres = new LeadCorrespondence();
                    objcorres.CompanyId = CompanyId;
                    objcorres.CustomerId = new Guid(CustomerId);
                    objcorres.ToEmail = toEmailAddress;
                    objcorres.TemplateKey = TemplateKey;
                    objcorres.Type = "Email";
                    if (from != "" && from != null)
                    {
                        objcorres.Subject = from;
                    }
                    else
                    {
                        objcorres.Subject = message.Subject;

                    }
                    objcorres.BodyContent = message.Body;
                    objcorres.SentDate = DateTime.Now.UTCCurrentTime();
                    objcorres.IsSystemAutoSent = true;
                    objcorres.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    objcorres.CcEmail = message.CC.ToString();
                    objcorres.SentBy = new Guid(EmployeeId);
                    _LeadCorrespondenceDataAccess.Insert(objcorres);
                    try
                    {
                        _ActivityDataAccess.Insert(new Activity()
                        {
                            ActivityId = Guid.NewGuid(),
                            ActivityType = objcorres.Type,
                            AssignedTo = objcorres.SentBy,
                            AssociatedWith = objcorres.CustomerId,
                            Status = "Completed",
                            //AssociatedType = 
                            CreatedBy = objcorres.SentBy,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            Note = objcorres.BodyContent,
                            NotifyBy = "",
                            
                        });
                    }
                    catch (Exception) { }
                }
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\FileMailError.txt"), true))
                {
                    file.WriteLine(ex.Message);
                    file.WriteLine(ex.StackTrace);
                    file.WriteLine(ex.InnerException);
                    file.Close();
                } 
                //Logger.AddElmah(ex);
            }


            return false;

        }

        #region PurchaseOrder Emails
        public bool EmailToSupplierForPurchaseOrder(PurchaseOrderCreatedEmail email)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("CompanyName", email.CompanyName);
                templateVars.Add("EmailBody", email.EmailBody);
                templateVars.Add("FromName", email.FromName);
                templateVars.Add("OrderDate", email.OrderDate);
                templateVars.Add("Subject", email.Subject);
                templateVars.Add("POId", email.POId);
                templateVars.Add("ReplyEmail", email.ReplyEmail);
                if (email.ccEmail != null)
                {
                    templateVars.Add("CCEmail", email.ccEmail);
                }
                else
                {
                    templateVars.Add("CCEmail", null);
                }
                List<Attachment> att = new List<Attachment>();
                att.Add(email.PurchaseOrderPdf);

                if (SentEmail(templateVars, EmailTemplateKey.PurchaseOrderEmailTemplates.POCreatedEmail, email.CompanyId, att))
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
        #endregion

        #region Ticket Mails
        public bool SendTicketCreatedNotificationEmail(TicketNotificationEmails email)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("CreatedByName", email.CreatedByName);
                templateVars.Add("CreatedForCustomerName", email.CreatedForCustomerName);
                templateVars.Add("TicketMessage", email.TicketMessage);
                templateVars.Add("TicketNumber", email.TicketNumber);
                templateVars.Add("Subject", email.Subject);
                templateVars.Add("BodyMessage", email.BodyMessage);
                templateVars.Add("TicketUrl", email.TicketUrl);
                templateVars.Add("Address", email.CustomerAddress);
               
                templateVars.Add("ScheduleOn", email.CompletionDate.ToString("MM/dd/yyyy"));

                
                templateVars.Add("StartTime", email.AppointmentStartTime);
                templateVars.Add("EndTime", email.AppointmentEndTime);

                if (SentEmail(templateVars, EmailTemplateKey.TicketEmailTemplates.SendTicketCreatedNotification, email.CompanyId, null))
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
        public bool SendTicketUpdatedNotificationEmail(TicketNotificationEmails email)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("EmployeeName", email.CreatedByName);
                templateVars.Add("CreatedForCustomerName", email.CreatedForCustomerName);
                templateVars.Add("Address", email.CustomerAddress);
                templateVars.Add("TicketNumber", email.TicketNumber);
                templateVars.Add("Subject", email.Subject);
                templateVars.Add("TicketStatus", email.TicketStatus);
                templateVars.Add("HeaderMessage", email.HeaderMessage);
       
                

                if (SentEmail(templateVars, EmailTemplateKey.TicketEmailTemplates.SendTicketUpdatedNotification, email.CompanyId, null))
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

        public bool SendEqpReleasedNotificationEmail(TicketNotificationEmails email)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("EmployeeName", email.CreatedByName);
                templateVars.Add("CreatedForCustomerName", email.CreatedForCustomerName);
                templateVars.Add("Address", email.CustomerAddress);
                templateVars.Add("TicketNumber", email.TicketNumber);
                templateVars.Add("Subject", email.Subject);
              
                templateVars.Add("HeaderMessage", email.HeaderMessage);
                templateVars.Add("Body", email.BodyMessage);



                if (SentEmail(templateVars, EmailTemplateKey.TicketEmailTemplates.SendEqpReleasedNotification, email.CompanyId, null))
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
        #endregion


        #region Request To Admin
        private bool SentRequest(Hashtable TemplateValue, string TemplateKey)
        {
            EmailParser parser = null;
            EmailTemplate emailTemplate = new EmailTemplate();
            emailTemplate.BodyFile = ConfigurationManager.AppSettings["RequestBodyFile"];
            emailTemplate.ToEmail = ConfigurationManager.AppSettings["RequestToEmail"];
            emailTemplate.CcEmail = ConfigurationManager.AppSettings["RequestCCEmail"];
            emailTemplate.BccEmail = ConfigurationManager.AppSettings["RequestBCCEmail"];
            emailTemplate.Subject = ConfigurationManager.AppSettings["RequestSubject"];
            emailTemplate.FromEmail = ConfigurationManager.AppSettings["RequestFromEmail"];

            string DefaultEmailHost = ConfigurationManager.AppSettings["DefaultEmailHost"];
            string DefaultEmailUsername = ConfigurationManager.AppSettings["DefaultEmailUsername"];
            string DefaultEmailPassword = ConfigurationManager.AppSettings["DefaultEmailPassowrd"];
            string EmailPort = ConfigurationManager.AppSettings["EmailPort"];

            if (!string.IsNullOrWhiteSpace(emailTemplate.BodyFile))
            {
                parser = new EmailParser(System.Web.Hosting.HostingEnvironment.MapPath(emailTemplate.BodyFile), TemplateValue, true);
            }
            MailMessage message = new MailMessage();
            message.Body = parser.Parse();

            #region To Email
            if (!string.IsNullOrWhiteSpace(emailTemplate.ToEmail))
            {
                var ToEmailList = emailTemplate.ToEmail.Split(',');
                foreach(var item in ToEmailList)
                {
                    if (item.IsValidEmailAddress())
                    {
                        message.To.Add(item);
                    }
                }
            }
            if (message.To.Count() == 0)
            {
                return false;
            }
            #endregion 

            #region From Email
            if (!string.IsNullOrWhiteSpace(emailTemplate.FromEmail) && emailTemplate.FromEmail.IsValidEmailAddress())
            { 
                message.From = new MailAddress(emailTemplate.FromEmail);
            }
            else
            {
                message.From = new MailAddress("noreply@rmrcloud.com");
            }
            #endregion

            #region CC Email
            if (!string.IsNullOrWhiteSpace(emailTemplate.CcEmail))
            { 
                var ccList = emailTemplate.CcEmail.Split(',');
                foreach(var item in ccList)
                {
                    if (item.IsValidEmailAddress())
                    {
                        message.CC.Add(new MailAddress(item));
                    }
                    
                }
            }
            #endregion

            #region BCC Email
            if (!string.IsNullOrWhiteSpace(emailTemplate.BccEmail))
            {
                var bccList = emailTemplate.BccEmail.Split(',');
                foreach (var item in bccList)
                {
                    if (item.IsValidEmailAddress())
                    {
                        message.Bcc.Add(new MailAddress(item)); 
                    }
                    
                }
                message.Bcc.Add(new MailAddress(emailTemplate.BccEmail));
            }
            #endregion

            message.Subject = emailTemplate.Subject;

            message.IsBodyHtml = true; 

            SmtpClient client = new SmtpClient();
            if (!string.IsNullOrWhiteSpace(DefaultEmailHost))
            {
                client.Host = DefaultEmailHost;
            }
            else
            {
                client.Host = "smtp.mailgun.org";
            }
            if(!string.IsNullOrWhiteSpace(DefaultEmailUsername) && !string.IsNullOrWhiteSpace(DefaultEmailUsername))
            {
                client.Credentials = new System.Net.NetworkCredential(DefaultEmailUsername, DefaultEmailPassword);
            }
            else
            {
                client.Credentials = new System.Net.NetworkCredential("postmaster@mail.rmrcloud.com", "5b1964ecbf158270c00d4d521c0dfca3-833f99c3-bc86a021");
            }
            client.Port = 587;
            int PortNumber = 587;
            if(int.TryParse(EmailPort,out PortNumber))
            {
                client.Port = PortNumber;
            }
            client.EnableSsl = false;
            //message.From = new MailAddress("noreply@rmrcloud.com");
            //message.From = new MailAddress("info@marketing.centralstationmarketing.com");
            //Need to user From email of default domain
            client.Send(message);

            return true;
             
            #region EmailHistory
            //try
            //{
            //    EmailHistory emailHistory = new EmailHistory();
            //    emailHistory.TemplateKey = TemplateKey;
            //    emailHistory.ToEmail = emailTemplate.ToEmail;
            //    emailHistory.CcEmail = emailTemplate.CcEmail;
            //    emailHistory.BccEmail = emailTemplate.BccEmail;
            //    emailHistory.FromEmail = emailTemplate.FromEmail;
            //    emailHistory.EmailBodyContent = message.Body;
            //    emailHistory.EmailSubject = emailTemplate.Subject;
            //    emailHistory.EmailSentDate = DateTime.Now;
            //    emailHistory.LastUpdatedDate = DateTime.Now;
            //    _EmailHistoryDataAccess.Insert(emailHistory);
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    HsErrorLog.AddElmah(ex);
            //    //Logger.AddElmah(ex);
            //}
            #endregion 
        }
        #endregion Request To Admin

        #region send via MailGunAPI
        //private bool SentMailGunEmail(Hashtable TemplateValue, string TemplateKey, Guid CompanyId, List<Attachment> Attachments)
        //{
        //    TemplateValue.Add("Logo", GetEmailLogoByCompanyId(CompanyId));
        //    TemplateValue.Add("Facebook", GetFacebookUrlByCompanyId(CompanyId));
        //    TemplateValue.Add("Youtube", GetYoutubeUrlByCompanyId(CompanyId));
        //    TemplateValue.Add("TeamNameSignature", GetTeamNameSignatureByCompanyId(CompanyId));

        //    EmailTemplate emailTemplate = _EmailTemplateDataAccess.GetByQuery("TemplateKey='" + TemplateKey + "'").FirstOrDefault();
        //    EmailParser parser = null;
        //    string toEmailAddress = "";
        //    if (emailTemplate == null || emailTemplate.Id == 0)
        //    {
        //        //Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception("Template not found for key :" + TemplateKey));
        //        return false;
        //    }
        //    if (!string.IsNullOrWhiteSpace(emailTemplate.BodyFile))
        //    {
        //        parser = new EmailParser(HttpContext.Current.Server.MapPath(emailTemplate.BodyFile), TemplateValue, true);
        //    }
        //    else if (!string.IsNullOrWhiteSpace(emailTemplate.BodyContent))
        //    {
        //        parser = new EmailParser(emailTemplate.BodyContent, TemplateValue, false);
        //    }

        //    MailMessage message = new MailMessage();
        //    message.Body = parser.Parse();

        //    if (emailTemplate.ToEmail.IndexOf("##") > -1)
        //    {
        //        EmailParser ToEmailParser = new EmailParser(emailTemplate.ToEmail, TemplateValue, false);
        //        message.To.Add(new MailAddress(ToEmailParser.Parse()));
        //        toEmailAddress = message.To[0].ToString();
        //    }
        //    else
        //    {
        //        message.To.Add(emailTemplate.ToEmail);
        //        toEmailAddress = message.To[0].ToString();
        //    }
        //    if (emailTemplate.ReplyEmail.IndexOf("##") > -1)
        //    {
        //        EmailParser ToEmailParser = new EmailParser(emailTemplate.ReplyEmail, TemplateValue, false);
        //        message.ReplyTo = new MailAddress(ToEmailParser.Parse());
        //        toEmailAddress = message.To[0].ToString();
        //    }
        //    else
        //    {
        //        message.ReplyTo = new MailAddress(emailTemplate.ReplyEmail);
        //    }

        //    if (emailTemplate.FromEmail.IndexOf("##") > -1)
        //    {
        //        EmailParser ToEmailParser = new EmailParser(emailTemplate.FromEmail, TemplateValue, false);
        //        message.From = new MailAddress(ToEmailParser.Parse());
        //    }
        //    else
        //    {
        //        if (!string.IsNullOrWhiteSpace(emailTemplate.FromName))
        //        {
        //            message.From = new MailAddress(emailTemplate.FromEmail, emailTemplate.FromName);
        //        }
        //        else
        //        {
        //            message.From = new MailAddress(emailTemplate.FromEmail);
        //        }

        //    }

        //    if (!string.IsNullOrWhiteSpace(emailTemplate.BccEmail))
        //    {
        //        if (emailTemplate.BccEmail.IndexOf(";") > -1)
        //        {
        //            var ArrBcc = emailTemplate.BccEmail.Split(';');
        //            foreach (var item in ArrBcc)
        //            {
        //                message.Bcc.Add(item);
        //            }
        //        }
        //        else
        //        {
        //            message.Bcc.Add(emailTemplate.BccEmail);
        //        }
        //    }

        //    if (!string.IsNullOrWhiteSpace(emailTemplate.CcEmail))
        //    {
        //        if (emailTemplate.CcEmail.IndexOf(";") > -1)
        //        {
        //            var ArrCcEmail = emailTemplate.CcEmail.Split(';');
        //            foreach (var item in ArrCcEmail)
        //            {
        //                message.CC.Add(item);
        //            }
        //        }
        //        else
        //        {
        //            message.CC.Add(emailTemplate.CcEmail);
        //        }
        //    }
        //    if (emailTemplate.Subject.IndexOf("##") > -1)
        //    {
        //        EmailParser SubjectParser = new EmailParser(emailTemplate.Subject, TemplateValue, false);
        //        message.Subject = SubjectParser.Parse();
        //    }
        //    else
        //    {
        //        message.Subject = emailTemplate.Subject;
        //    }
        //    if (Attachments != null)
        //    {
        //        if (Attachments.Count > 0)
        //        {
        //            foreach (var attachment in Attachments)
        //            {
        //                message.Attachments.Add(attachment);
        //            }
        //        }
        //    }
        //    message.IsBodyHtml = true;
        //    message.BodyEncoding = System.Text.Encoding.UTF8;


        //    try
        //    {
        //        SmtpClient client = new SmtpClient();
        //        client.Credentials = new System.Net.NetworkCredential("noreply@piiscenter.com", "piiscenter.com");
        //        client.EnableSsl = false;
        //        client.Send(message);






        //        //if (!HttpContext.Current.Request.IsLocal)
        //        //{
        //        //    SmtpClient client = new SmtpClient();
        //        //    client.Credentials = new System.Net.NetworkCredential("noreply@piiscenter.com", "piiscenter.com");
        //        //    client.EnableSsl = false;
        //        //    client.Send(message);
        //        //}
        //        //SmtpClient smtp = new SmtpClient
        //        //{
        //        //    Host = "smtp.gmail.com",
        //        //    //change the port to prt 587. This seems to be the standard for Google smtp transmissions.
        //        //    Port = 587,
        //        //    //enable SSL to be true, otherwise it will get kicked back by the Google server.
        //        //    EnableSsl = true,
        //        //    //The following properties need set as well
        //        //    DeliveryMethod = SmtpDeliveryMethod.Network,
        //        //    UseDefaultCredentials = false,
        //        //    Credentials = new NetworkCredential("informrcloud@gmail.com", "Inf0rmrcl@ud")
        //        //};
        //        //smtp.Send(message);
        //        EmailHistory emailHistory = new EmailHistory();
        //        emailHistory.TemplateKey = TemplateKey;
        //        emailHistory.ToEmail = toEmailAddress;
        //        emailHistory.CcEmail = message.CC.ToString();
        //        emailHistory.BccEmail = message.Bcc.ToString();
        //        emailHistory.FromEmail = message.From.ToString();
        //        emailHistory.EmailBodyContent = message.Body;
        //        emailHistory.EmailSubject = message.Subject;
        //        emailHistory.EmailSentDate = DateTime.Now;
        //        emailHistory.LastUpdatedDate = DateTime.Now;
        //        _EmailHistoryDataAccess.Insert(emailHistory);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        EmailHistory emailHistory = new EmailHistory();
        //        emailHistory.TemplateKey = TemplateKey;
        //        emailHistory.ToEmail = toEmailAddress;
        //        emailHistory.CcEmail = message.CC.ToString();
        //        emailHistory.BccEmail = message.Bcc.ToString();
        //        emailHistory.FromEmail = message.From.ToString();
        //        emailHistory.EmailBodyContent = message.Body;
        //        emailHistory.EmailSubject = message.Subject;

        //        emailHistory.LastUpdatedDate = DateTime.Now;
        //        _EmailHistoryDataAccess.Insert(emailHistory);
        //        //Logger.AddElmah(ex);
        //    }


        //    return false;
        //}

        //public async Task<dynamic> HelloEmail(string toEmail, string subject, string body)
        //{
        //    List<IRecipient> recepents = new List<IRecipient>();
        //    recepents.Add(new HSRecepent
        //    {
        //        DisplayName = "kaizar tariq inan",
        //        Email = "kaizar.tariq.inan@gmail.com"
        //    });
        //    List<IRecipient> ccs = new List<IRecipient>();
        //    ccs.Add(new HSRecepent
        //    {
        //        DisplayName = "parves kawser",
        //        Email = "rezakawser@gmail.com"
        //    });
        //    IRecipient from = new HSRecepent
        //    {
        //        DisplayName = "Test Email",
        //        Email = "info@rmrcloud.com"
        //    };

        //    HSEmail email = new HSEmail()
        //    {
        //        Attachments = null,
        //        Bcc = null,
        //        FileAttachments = null,

        //        Cc =ccs,
        //        To = recepents,
        //        From = from,
        //        Subject="test subject",
        //        Text="test body"
        //    };
        //    //domain//marketing.centralstationmarketing.com
        //    var mail = new MailGun.Messages.MessageBuilder().AddToRecipient(new HSRecepent
        //    {
        //        DisplayName = "kaizar tariq inan",
        //        Email = "kaizar.tariq.inan@gmail.com"
        //    }).SetFromAddress(new HSRecepent
        //    {
        //        DisplayName = "Test Email",
        //        Email = "info@marketing.centralstationmarketing.com"
        //    }).SetTestMode(false)
        //  .SetSubject("Plain text test")
        //  .SetTextBody("email body")
        //  .GetMessage();

        //    MailGun.Service.MessageService service = new MailGun.Service.MessageService("key-61a400d687e1f7e3445a26419505a516");
        //    var response = await service.SendMessageAsync("marketing.centralstationmarketing.com", mail);
        //    return response;
        //}
        //private class HSRecepent : IRecipient
        //{
        //    public string DisplayName
        //    {
        //        set;get;
        //    }

        //    public string Email
        //    {
        //        set; get;
        //    }

        //    public string ToFormattedString()
        //    {
        //        return string.Format("\"{0}\" <{1}>", DisplayName, Email);
        //    }
        //}
        //private class HSEmail : IMessage
        //{
        //    public ICollection<FileInfo> Attachments
        //    {
        //        set;get;
        //    }

        //    public ICollection<IRecipient> Bcc
        //    {
        //        set; get;
        //    }

        //    public string CampaignId
        //    {
        //        set; get;
        //    }

        //    public ICollection<IRecipient> Cc
        //    {
        //        set; get;
        //    }

        //    public IDictionary<string, global::Newtonsoft.Json.Linq.JObject> CustomData
        //    {
        //        set; get;
        //    }

        //    public IDictionary<string, string> CustomHeaders
        //    {
        //        set; get;
        //    }

        //    public IDictionary<string, string> CustomParameters
        //    {
        //        set; get;
        //    }

        //    public DateTime? DeliveryTime
        //    {
        //        set; get;
        //    }

        //    public bool Dkim
        //    {
        //        set; get;
        //    }

        //    public ICollection<IFileAttachment> FileAttachments
        //    {
        //        set; get;
        //    }

        //    public IRecipient From
        //    {
        //        set; get;
        //    }

        //    public string Html
        //    {
        //        set; get;
        //    }

        //    public ICollection<FileInfo> Inline
        //    {
        //        set; get;
        //    }

        //    public global::Newtonsoft.Json.Linq.JObject RecipientVariables
        //    {
        //        set; get;
        //    }

        //    public string Subject
        //    {
        //        set; get;
        //    }

        //    public ICollection<string> Tags
        //    {
        //        set; get;
        //    }

        //    public bool TestMode
        //    {
        //        set; get;
        //    }

        //    public string Text
        //    {
        //        set; get;
        //    }

        //    public ICollection<IRecipient> To
        //    {
        //        set; get;
        //    }

        //    public bool Tracking
        //    {
        //        set; get;
        //    }

        //    public bool TrackingClicks
        //    {
        //        set; get;
        //    }

        //    public bool TrackingOpen
        //    {
        //        set; get;
        //    }

        //    public HttpContent AsFormContent()
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public ICollection<KeyValuePair<string, string>> AsKeyValueCollection()
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
        #endregion

        #region SentTestEmail
        public bool SentTestEmail(string ToEmail,string Subject, string Body,Guid CompanyId, Hashtable TemplateVars = null)
        {
            if (TemplateVars == null)
            {
                TemplateVars = new Hashtable();
            }
            #region Common Templates
            var SiteDomain = ConfigurationManager.AppSettings["SiteDomain"];
            var FacebookTemplate = "<a href='{0}'><img style='height:30px;' src='{1}/Content/Icons/Social/facebook_circle.png' /></a>";
            var InstagramTemplate = "<a href='{0}'><img style='height: 30px;' src='{1}/Content/Icons/Social/instagram_circle.png' /></a>";
            var YoutubeTemplate = "<a href='{0}'><img style='height:30px;' src='{1}/Content/Icons/Social/youtube_circle.png' /></a>";
            var FacebookLink = GetFacebookUrlByCompanyId(CompanyId);
            var InstagramLink = GetInstagramUrlByCompanyId(CompanyId);
            var YoutubeLink = GetYoutubeUrlByCompanyId(CompanyId);
            if (!string.IsNullOrEmpty(FacebookLink))
            {
                FacebookTemplate = string.Format(FacebookTemplate, FacebookLink, SiteDomain);
                TemplateVars.Add("FacebookDiv", FacebookTemplate);
            }
            if (!string.IsNullOrEmpty(InstagramLink))
            {
                InstagramTemplate = string.Format(InstagramTemplate, InstagramLink, SiteDomain);
                TemplateVars.Add("InstagramDiv", InstagramTemplate);
            }
            if (!string.IsNullOrEmpty(YoutubeLink))
            {
                YoutubeTemplate = string.Format(YoutubeTemplate, YoutubeLink, SiteDomain);
                TemplateVars.Add("YoutubeDiv", YoutubeTemplate);
            }

            if (TemplateVars["Logo"] == null)
            {
                TemplateVars.Add("Logo", GetEmailLogoByCompanyId(CompanyId));
            }
            if (TemplateVars["TeamNameSignature"] == null)
            {
                TemplateVars.Add("TeamNameSignature", GetTeamNameSignatureByCompanyId(CompanyId));
            }
            if (TemplateVars["CompanyNameAlt"] == null)
            {
                TemplateVars.Add("CompanyNameAlt", GetCompanyNameByCompanyId(CompanyId));
            }
            if (TemplateVars["CompanyInformation"] == null)
            {
                TemplateVars.Add("CompanyInformation", GetFooterCompanyInformationByCompanyId(CompanyId));
            }
            #endregion

            MailMessage message = new MailMessage();
            EmailParser parser = new EmailParser(Body, TemplateVars, false);
            message.Body = parser.Parse();
            message.Subject = Subject;
            if(!string.IsNullOrWhiteSpace(ToEmail) && ToEmail.Split(',').Count() > 0)
            {
                foreach(var item in ToEmail.Split(','))
                {
                    if (item.IsValidEmailAddress())
                    {
                        message.To.Add(new MailAddress(item));
                    }
                }
            }
            if (message.To.Count() == 0)
            {
                return false;
            }
            message.Bcc.Add(new MailAddress("rezakawser@gmail.com"));
            message.From = new MailAddress("noreply@dfwsecurity.com", "dfw security");
            message.Body = Body;
            message.IsBodyHtml = true;

            #region SendMail
            SmtpClient client = new SmtpClient();
            
            GlobalSetting EmailHost = _GlobalSettingDataAccess.GetByQuery(string.Format(" SearchKey ='{0}' and CompanyId ='{1}'", "EmailHost", CompanyId)).FirstOrDefault();
            GlobalSetting EmailHostUsername = _GlobalSettingDataAccess.GetByQuery(string.Format(" SearchKey ='{0}' and CompanyId ='{1}'", "EmailHostUsername", CompanyId)).FirstOrDefault();
            GlobalSetting EmailHostPassword = _GlobalSettingDataAccess.GetByQuery(string.Format(" SearchKey ='{0}' and CompanyId ='{1}'", "EmailHostPassword", CompanyId)).FirstOrDefault();
            GlobalSetting EmailPort = _GlobalSettingDataAccess.GetByQuery(string.Format(" SearchKey ='{0}' and CompanyId ='{1}'", "EmailPort", CompanyId)).FirstOrDefault();

            if (EmailHost != null && !string.IsNullOrWhiteSpace(EmailHost.Value))
            {
                client.Host = EmailHost.Value;
            }
            if (EmailHostUsername != null && !string.IsNullOrWhiteSpace(EmailHostUsername.Value)
                && EmailHostPassword != null && !string.IsNullOrWhiteSpace(EmailHostPassword.Value))
            {
                client.Credentials = new System.Net.NetworkCredential(EmailHostUsername.Value, EmailHostPassword.Value);
            }
            if (EmailPort != null && !string.IsNullOrWhiteSpace(EmailPort.Value))
            {
                int port = 587;
                if (int.TryParse(EmailPort.Value, out port))
                {
                    client.Port = port;
                }
            }
            client.EnableSsl = false;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                      | SecurityProtocolType.Tls11
                                      | SecurityProtocolType.Tls12;
            client.Send(message);
            #endregion
            #region Insert into Email History table
            EmailHistory emailHistory = new EmailHistory();
            emailHistory.TemplateKey = "Custom";
            emailHistory.ToEmail = ToEmail; 
            emailHistory.BccEmail = message.Bcc.ToString();
            emailHistory.FromEmail = message.From.ToString();
            emailHistory.EmailBodyContent = message.Body;
            emailHistory.EmailSubject = message.Subject;
            emailHistory.EmailSentDate = DateTime.Now;
            emailHistory.LastUpdatedDate = DateTime.Now;
            _EmailHistoryDataAccess.Insert(emailHistory);
            #endregion

            return true;
        }
        #endregion

        #region Booking Related
        public bool BookingSignNotificationEmail(BookingSignNotificationEmail email)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("CustomerName", email.CustomerName);
                templateVars.Add("BookingNO", email.BookingNO);
                templateVars.Add("TotalAmount", email.TotalAmount);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("CompanyName", email.CompanyName);
                templateVars.Add("DeclinationReason", email.DeclinationReason);
                if (email.EmailTo == "SalesPerson")
                {
                    templateVars.Add("SalesPersonsName", email.SalesPersonsName);
                    if (SentEmail(templateVars, EmailTemplateKey.BookingSign.CustomerSignedBookingConfirmationToSalesPerson, email.CompanyId, null))
                    {
                        return true;
                    }
                }
                else if (email.EmailTo == "Customer")
                {
                    if (SentEmail(templateVars, EmailTemplateKey.BookingSign.CustomerSignedBookingConfirmationToCustomer, email.CompanyId, null))
                    {
                        return true;
                    }
                }
                else if (email.EmailTo == "DeclinedBooking")
                {
                    if (SentEmail(templateVars, EmailTemplateKey.BookingSign.CustomerDeclinedBookingToSalesPerson, email.CompanyId, null))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                //Logger.AddElmah(ex);
            }

            return false;
        }
        //Booking Created Email 
        public bool SendBookingCreatedEmail(BookingCreatedEmail email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();

                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("BalanceAmount", email.BalanceAmount);
                templateVars.Add("CompanyName", email.CompanyName);
                templateVars.Add("CustomerName", email.CustomerName);
                templateVars.Add("BookingId", email.BookingId);
                templateVars.Add("BookingLink", email.BookingLink);
                templateVars.Add("EmailBody", email.EmailBody);
                templateVars.Add("FromEmail", email.FromEmail);
                templateVars.Add("FromName", email.FromName);
                templateVars.Add("Subject", email.Subject);
                templateVars.Add("CustomerId", email.CustomerId);
                templateVars.Add("EmployeeId", email.EmployeeId);
                templateVars.Add("CCEmail", email.CCEmail);
                List<Attachment> att = new List<Attachment>();
                att.Add(email.BookingPdf);
                if (SentEmail(templateVars, EmailTemplateKey.Booking.BookingEmail, gCompanyId, null))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                //Logger.AddElmah(ex);
            }

            return false;
        }
        #endregion

        public bool SubscriptionNotificationEmail(SubscribedToAthorizeNotification email)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("CompanyName", email.CompanyName);
                templateVars.Add("CustomerName", email.CustomerName); 
                templateVars.Add("SubscriptionAmount", email.SubscriptionAmount);
                templateVars.Add("SubscriptionPeriod", email.SubscriptionPeriod);
                templateVars.Add("BillingCycle", email.BillingCycle);
                templateVars.Add("PaymentMethod", email.PaymentMethod);
                templateVars.Add("ReplyEmail", email.ReplyTo); 
                if (SentEmail(templateVars, EmailTemplateKey.SubscriptionNotification.SubscribedToAuthorizeCustomerNotification, email.CompanyId, null))
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

        public bool SendOTPEmail(OTPEmail email)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("Name", email.Name);
                templateVars.Add("OTP", email.OTP);
         
                if (SentEmail(templateVars, EmailTemplateKey.OTPEmail.SendOTPEmail, email.CompanyId, null))
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

        public bool SendFriendEmail(SendFriendEmail email)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("Name", email.Name);
                templateVars.Add("Message", email.Messsage);
                templateVars.Add("SenderName", email.SenderName);
                templateVars.Add("ShortLink", email.ShortLink);
                if (SentEmail(templateVars, EmailTemplateKey.FriendEmail.SendFriendEmail, email.CompanyId, null))
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

        public bool SendSurveyEmail(SendSurveyEmail email)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("NAME", email.Name);
                templateVars.Add("CompanyId", email.CompanyId);
                templateVars.Add("SenderName", email.SenderName);
                templateVars.Add("ShortLink", email.shortLink);
                templateVars.Add("CompanyName", email.CompanyName);
                templateVars.Add("Subject", email.Subject);
                templateVars.Add("Header", email.Header);
                if (SentEmail(templateVars, EmailTemplateKey.SendSurveyEmail.SurveyEmail, email.CompanyId, null))
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
        public bool SendUpdateCustomerEmail(UpdateCustomerConfirmation email)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("Name", email.Name);
                templateVars.Add("ColumnName", email.ColumnName);

                if (SentEmail(templateVars, EmailTemplateKey.UpdateCustomerEmail.SendUpdateCustomerEmail, email.CompanyId, null))
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
        private bool SentEmailRemainder(Hashtable TemplateValue, string TemplateKey, List<Attachment> Attachments)
        {
            EmailTemplateDataAccess maildata = new EmailTemplateDataAccess();
            EmailHistoryDataAccess mailhistory = new EmailHistoryDataAccess();
            EmailTemplate emailTemplate = maildata.GetByQuery("TemplateKey='" + TemplateKey + "'").FirstOrDefault();
            EmailParser parser = null;
            string toEmailAddress = "";
            if (emailTemplate == null || emailTemplate.Id == 0)
            {
                //Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception("Template not found for key :" + TemplateKey));
                return false;
            }
            if (!string.IsNullOrWhiteSpace(emailTemplate.BodyFile))
            {
                string FilePath = emailTemplate.BodyFile;
                FilePath = System.Web.Hosting.HostingEnvironment.MapPath(FilePath);
                parser = new EmailParser(FilePath, TemplateValue, true);
                //parser = new EmailParser(HttpContext.Current.Server.MapPath(emailTemplate.BodyFile), TemplateValue, true);
            }
            else if (!string.IsNullOrWhiteSpace(emailTemplate.BodyContent))
            {
                parser = new EmailParser(emailTemplate.BodyContent, TemplateValue, false);
            }

            MailMessage message = new MailMessage();
            message.Body = parser.Parse();

            if (emailTemplate.ToEmail.IndexOf("##") > -1)
            {
                EmailParser ToEmailParser = new EmailParser(emailTemplate.ToEmail, TemplateValue, false);
                message.To.Add(new MailAddress(ToEmailParser.Parse()));
                toEmailAddress = message.To[0].ToString();
            }
            else
            {
                message.To.Add(emailTemplate.ToEmail);
                toEmailAddress = message.To[0].ToString();
            }
            if (emailTemplate.ReplyEmail.IndexOf("##") > -1)
            {
                EmailParser ToEmailParser = new EmailParser(emailTemplate.ReplyEmail, TemplateValue, false);
                message.ReplyTo = new MailAddress(ToEmailParser.Parse());
                toEmailAddress = message.To[0].ToString();
            }
            else
            {
                message.ReplyTo = new MailAddress(emailTemplate.ReplyEmail);
            }

            if (emailTemplate.FromEmail.IndexOf("##") > -1)
            {
                EmailParser ToEmailParser = new EmailParser(emailTemplate.FromEmail, TemplateValue, false);
                message.From = new MailAddress(ToEmailParser.Parse());
            }
            else
            {
                message.From = new MailAddress(emailTemplate.FromEmail);
            }

            if (!string.IsNullOrWhiteSpace(emailTemplate.BccEmail))
            {
                message.Bcc.Add(emailTemplate.BccEmail);
            }

            if (!string.IsNullOrWhiteSpace(emailTemplate.CcEmail))
            {
                message.CC.Add(emailTemplate.CcEmail);
            }

            if (emailTemplate.Subject.IndexOf("##") > -1)
            {
                EmailParser SubjectParser = new EmailParser(emailTemplate.Subject, TemplateValue, false);
                message.Subject = SubjectParser.Parse();
            }
            else
            {
                message.Subject = emailTemplate.Subject;
            }
            if (Attachments != null)
            {
                if (Attachments.Count > 0)
                {
                    foreach (var attachment in Attachments)
                    {
                        message.Attachments.Add(attachment);
                    }
                }
            }
            message.IsBodyHtml = true;
            message.BodyEncoding = System.Text.Encoding.UTF8;


            try
            {
                //if (HttpContext.Current!=null && HttpContext.Current.Request.IsLocal)
                //{
                //SmtpClient client = new SmtpClient();
                //client.Credentials = new System.Net.NetworkCredential("noreply@piiscenter.com", "piiscenter.com");
                //client.EnableSsl = false;
                //client.Send(message);
                //}
                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    //change the port to prt 587. This seems to be the standard for Google smtp transmissions.
                    Port = 587,
                    //enable SSL to be true, otherwise it will get kicked back by the Google server.
                    EnableSsl = true,
                    //The following properties need set as well
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("informrcloud@gmail.com", "Inf0rmrcl@ud")
                };
                smtp.Send(message);


                EmailHistory emailHistory = new EmailHistory();
                emailHistory.TemplateKey = TemplateKey;
                emailHistory.ToEmail = toEmailAddress;
                emailHistory.CcEmail = message.CC.ToString();
                emailHistory.BccEmail = message.Bcc.ToString();
                emailHistory.FromEmail = message.From.ToString();
                emailHistory.EmailBodyContent = message.Body;
                emailHistory.EmailSubject = message.Subject;
                emailHistory.EmailSentDate = DateTime.Now;
                emailHistory.LastUpdatedDate = DateTime.Now;
                mailhistory.Insert(emailHistory);
                return true;
            }
            catch (Exception ex)
            {
                EmailHistory emailHistory = new EmailHistory();
                emailHistory.TemplateKey = TemplateKey;
                emailHistory.ToEmail = toEmailAddress;
                emailHistory.CcEmail = message.CC.ToString();
                emailHistory.BccEmail = message.Bcc.ToString();
                emailHistory.FromEmail = message.From.ToString();
                emailHistory.EmailBodyContent = message.Body;
                emailHistory.EmailSubject = message.Subject;

                emailHistory.LastUpdatedDate = DateTime.Now;
                mailhistory.Insert(emailHistory);
                //Logger.AddElmah(ex);
                HsErrorLog.AddElmah(ex);
            }


            return false;
        }
        public bool EstimateSignNotificationEmail(EstimateSignNotificationEmail email)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("CustomerName", email.CustomerName);
                templateVars.Add("EstimateNO", email.EstimateNO);
                templateVars.Add("TotalAmount", email.TotalAmount);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("CompanyName", email.CompanyName);
                templateVars.Add("DeclinationReason", email.DeclinationReason);
                if (email.EmailTo == "SalesPerson")
                {
                    templateVars.Add("SalesPersonsName", email.SalesPersonsName);
                    if (SentEmail(templateVars, EmailTemplateKey.EstimateSign.CustomerSignedEstimateConfirmationToSalesPerson, email.CompanyId, null))
                    {
                        return true;
                    }
                }
               else if (email.EmailTo == "CreatedBy")
                {
                    templateVars.Add("CreatedByName", email.CreatedByName);
                    if (SentEmail(templateVars, EmailTemplateKey.EstimateSign.CustomerSignedEstimateConfirmationToCreatedBy, email.CompanyId, null))
                    {
                        return true;
                    }
                }
                else if (email.EmailTo == "Customer")
                {
                    if (SentEmail(templateVars, EmailTemplateKey.EstimateSign.CustomerSignedEstimateConfirmationToCustomer, email.CompanyId, null))
                    {
                        return true;
                    }
                }
                else if (email.EmailTo == "DeclinedEstimate")
                {
                    if (SentEmail(templateVars, EmailTemplateKey.EstimateSign.CustomerDeclinedEstimateToSalesPerson, email.CompanyId, null))
                    {
                        return true;
                    }
                }
                else if (email.EmailTo == "DeclinedEstimateCreatedBy")
                {
                    if (SentEmail(templateVars, EmailTemplateKey.EstimateSign.CustomerDeclinedEstimateToCreatedBy, email.CompanyId, null))
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
        }
        public bool SendEmailVerify(VerifyEmail email, Guid CompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("NAME", email.Name);
                templateVars.Add("EMAILVERIFICATIONLINK", email.EmailVerificationLink);

                templateVars.Add("ToEmail", email.ToEmail);
                if (SentEmail(templateVars, EmailTemplateKey.Registration.VerifyEmailAddress, CompanyId, null))
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

        public bool SendCompanyFile(ShareCompanyFile email, Guid CompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                //templateVars.Add("NAME", email.Name);
                templateVars.Add("FILELOCATIONLINK", email.FileLocationLink);

                templateVars.Add("ToEmail", email.ToEmail);
                if (SentEmail(templateVars, EmailTemplateKey.Companies.ShareFileTemplate, CompanyId, null))
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

        public bool SendSurveyEmail(SurveyEmail email,Guid CompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("NAME", email.Name);
                templateVars.Add("Subject", email.Subject);
                templateVars.Add("SURVEYLINK", email.SurveyLink);
                templateVars.Add("ASSIGNTO", email.AssignTo);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("REQUESTEDBY", email.RequestedBy);
                if (SentEmail(templateVars, EmailTemplateKey.SurveyEmail.SurveyTemplate, CompanyId, null))
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


        public bool SendSurveyEmailToUser(SurveyEmail email, Guid CompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("NAME", email.Name);
                templateVars.Add("SURVEYLINK", email.SurveyLink);
  
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("Subject", email.Subject);

                if (SentEmail(templateVars, EmailTemplateKey.SurveyEmail.SurveyTemplateUser, CompanyId, null))
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

        public bool SendSurveyEmailConfirmation(SurveyEmailConfirmation email, Guid CompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("NAME", email.Name);
                templateVars.Add("CONTENT", email.Content);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("Subject", email.Subject);
                List<Attachment> att = new List<Attachment>();
                att.Add(email.ReviewPdf);
                if (SentEmail(templateVars, EmailTemplateKey.SurveyEmail.SurveyConfirmationTemplate, CompanyId, att))
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

        public bool SendEmailResetPassword(ResetPasswordEmail email, Guid CompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("NAME", email.Name);
                templateVars.Add("EMAILVERIFICATIONLINK", email.EmailVerificationLink);

                templateVars.Add("ToEmail", email.ToEmail);
                if (SentEmail(templateVars, EmailTemplateKey.PasswordReset.ResetPassword, CompanyId, null))
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

        public bool EmailToSalesPersonFromLeads(EmailToSalesPersonFromLeads email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();

                templateVars.Add("MailPersonName", email.MailPersonName);
                templateVars.Add("CompanyName", email.SendMailPersonName);
                templateVars.Add("EMAILBODY", email.EmailBody);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("Subject", email.Subject);
                templateVars.Add("NAME", email.SentBy);

                if (SentEmail(templateVars, EmailTemplateKey.EmailToSalesPersonFromLeads.EmailToSalesPersonFromLead, gCompanyId, null))
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

        public bool EmailToEmployeeCreateWorkOrder(EmailCreateWorkOrder email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();


                templateVars.Add("AppointmentDate", email.AppointmentDate.ToString("MM/dd/yyyy"));
                templateVars.Add("AppointmentEndTime", email.AppointmentEndTime);
                templateVars.Add("AppointmentStartTime", email.AppointmentStartTime);
                templateVars.Add("CreatedBy", email.CreatedBy);
                templateVars.Add("CreatedDate", email.CreatedDate.ToString("MM/dd/yyyy"));
                templateVars.Add("CustomerName", email.CustomerName);
                if (email.Notes == "")
                {
                    email.Notes = "N/A";
                }
                templateVars.Add("Notes", email.Notes);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("EmployeeName", email.EmployeeName);
                templateVars.Add("EMAILBODY", email.CustomerName);
                if (SentEmail(templateVars, EmailTemplateKey.EmailCreateWorkOrder.mailCreateWorkOrder, gCompanyId, null))
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

        public bool EmailToEmployeeCreateServiceOrder(EmailCreateServiceOrder email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();


                templateVars.Add("AppointmentDate", email.AppointmentDate.ToString("MM/dd/yyyy"));
                templateVars.Add("AppointmentEndTime", email.AppointmentEndTime);
                templateVars.Add("AppointmentStartTime", email.AppointmentStartTime);
                templateVars.Add("CreatedBy", email.CreatedBy);
                templateVars.Add("CreatedDate", email.CreatedDate.ToString("MM/dd/yyyy"));
                templateVars.Add("CustomerName", email.CustomerName);
                if (email.Notes == "")
                {
                    email.Notes = "N/A";
                }
                templateVars.Add("Notes", email.Notes);
                templateVars.Add("ToEmail", email.ToEmail);
                //templateVars.Add("NAME", email.Name);
                templateVars.Add("EmployeeName", email.EmployeeName);
                templateVars.Add("EMAILBODY", email.CustomerName);
                if (SentEmail(templateVars, EmailTemplateKey.EmailCreateServiceOrder.mailCreateServiceOrder, gCompanyId, null))
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

        public bool EmailToLeadSignAgreement(SetupLeadCustormer email, Guid gCompanyId,string from = "")
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("CustomerName", email.CustomerName);
                templateVars.Add("EMAILBODY", email.CustomerName);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("CompanyName", email.CompanyName);
                templateVars.Add("CustomerId", email.CustomerId);
                templateVars.Add("EmployeeId", email.EmployeeId);
                List<Attachment> att = new List<Attachment>();
                att.Add(email.PdfAggrement);
                if (SentEmail(templateVars, EmailTemplateKey.EmailToLeadSignAgreement.mailToLeadSignAgreement, gCompanyId, att,from))
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

        public bool EmailOnlyLeadsAggrement(LeadsAggrement email, Guid gCompanyId,string from)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("CustomerNum", email.CustomerNum);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("Body", email.BodyLink);
                templateVars.Add("CustomerId", email.CustomerId);
                templateVars.Add("EmployeeId", email.EmployeeId);
                List<Attachment> att = new List<Attachment>();
                att.Add(email.LeadsAggrementpdf);
                if (SentEmail(templateVars, EmailTemplateKey.mailtoLeadsAggrement.EmailtoLeadsAggrement, gCompanyId, att,from))
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

        public bool EmailFileManagement(FileManagement email, Guid gCompanyId, string from)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("CustomerNum", email.CustomerNum);
                templateVars.Add("CompanyName", email.CompanyName);
                templateVars.Add("ToEmail", email.ToEmail);
                //templateVars.Add("Subject", email.Subject);
                //templateVars.Add("Body", email.Body);
                templateVars.Add("BodyLink", email.BodyLink);
                templateVars.Add("CustomerId", email.CustomerId);
                templateVars.Add("EmployeeId", email.EmployeeId);
                List<Attachment> att = new List<Attachment>();
                if(email.fileManagementpdf!= null)
                {
                    att.Add(email.fileManagementpdf);
                }
                if(email.IsFileWithoutCustomerSign)
                {
                    if (SentEmail(templateVars, EmailTemplateKey.mailToFileManagement.FileManagementMailWithoutCustomerSignature, gCompanyId, att,from))
                    {
                        return true;
                    }
                }
                else
                {
                    if (SentEmail(templateVars, EmailTemplateKey.mailToFileManagement.FileManagementMail, gCompanyId, att,from))
                    {
                        return true;
                    }
                }
                
            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\FileMailError.txt"), true))
                {
                    file.WriteLine(ex.Message);
                    file.WriteLine(ex.StackTrace);
                    file.WriteLine(ex.InnerException);
                    file.Close();
                }
                //Logger.AddElmah(ex);
            }

            return false;
        }
        public bool EmailFileManagementConfirmation(FileManagement email, Guid gCompanyId,string from ="")
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("CustomerNum", email.CustomerNum);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("CustomerId", email.CustomerId);
                templateVars.Add("EmployeeId", email.EmployeeId);
                List<Attachment> att = new List<Attachment>();
                att.Add(email.fileManagementpdf);
                if (SentEmail(templateVars, EmailTemplateKey.mailToFileManagementConfirmation.FileManagementConfirmationMail, gCompanyId, att,from))
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
        public bool SendMailLeadCreation(LeadCreation lc, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("CustomerNum", lc.CustomerNum);
                templateVars.Add("BodyContent", lc.BodyContent);
                templateVars.Add("ToEmail", lc.ToEmail);
                templateVars.Add("CustomerId", lc.CustomerId);
                templateVars.Add("EmployeeId", lc.EmployeeId);
                if (SentEmail(templateVars, EmailTemplateKey.mailToLeadCreation.SendMailLeadCreation, gCompanyId, null))
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
        public bool EmailOnlyCustomerTicketAddendumDocument(CustomerTicketAddendumEmail email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("CustomerNum", email.CustomerName);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("Body", email.BodyLink);
                if (SentEmail(templateVars, EmailTemplateKey.MailToAddendum.EMailToAddendum, gCompanyId, null))
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

        public bool SendEmailToTicketAssignUser(SendEmailTicketAssignModel email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("Subject", email.Subject);
                templateVars.Add("Body", email.Body);
                templateVars.Add("Name", email.Name);
                if (SentEmail(templateVars, EmailTemplateKey.TicketAssignMail.TicketAssignEmail, gCompanyId, null))
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

        public bool EmailOnlyCancellationAggrement(LeadsAggrement email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("CustomerNum", email.CustomerNum);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("Body", email.BodyLink);
                templateVars.Add("CustomerId", email.CustomerId);
                templateVars.Add("EmployeeId", email.EmployeeId);
                List<Attachment> att = new List<Attachment>();
                att.Add(email.LeadsAggrementpdf);
                if (SentEmail(templateVars, EmailTemplateKey.CancellationSignAgreement.CancellationAgreement, gCompanyId, att))
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
        public bool EmailUserCredential(CredentialMail email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("CustomerNume", email.CustomerName);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("Message", email.Message);
                templateVars.Add("USERNAME", email.UserName);
                templateVars.Add("PASSWORD", email.Password);

                if (SentEmail(templateVars, EmailTemplateKey.CredentialEmail.CredentialMail, gCompanyId, null))
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

        public string GetCurrentCurrencyByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;
            var globalsetting = new GlobalSetting();
            string CurrentCurrency = RMRCacheKey.CurrentCurrency + CompanyId.ToString();

            if (System.Web.HttpRuntime.Cache[CurrentCurrency] == null)
            {
                string DataKey = "CurrentCurrency";
                globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    result = globalsetting.Value.ToString();
                }
                else
                {
                    result = "$";
                }

                System.Web.HttpRuntime.Cache[CurrentCurrency] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[CurrentCurrency];
            }
            return result;
        }
        public bool EmailToSuccessTransaction(SetupLeadCustormer email, Guid gCompanyId)
        {
            try
            {

                string CurrentCurrency = GetCurrentCurrencyByCompanyId(gCompanyId);

                EmailTemplate template = GetTemplateByTemplateKey("InvoicePredefinePaymentReceipt");
                Hashtable datatemplate = new Hashtable();
                datatemplate.Add("CompanyName", email.CompanyName);
                datatemplate.Add("InvoiceId", email.InvoiceId);//need To take from controller.
                datatemplate.Add("PaymentDate", DateTime.Now.UTCCurrentTime().UTCToClientTime());
                datatemplate.Add("PaymentMethod", email.PaymentMethod);//need To take from controller.
                datatemplate.Add("PaymentAmount", CurrentCurrency + string.Format("{0:0,0.00}", Convert.ToDouble(email.AmountPaid)));
                datatemplate.Add("InvoiceTotal", CurrentCurrency + string.Format("{0:0,0.00}", Convert.ToDouble(email.TotalAmount)));
                datatemplate.Add("InvoiceBalance", CurrentCurrency +  string.Format("{0:0,0.00}", Convert.ToDouble(email.BalanceDue)));
                datatemplate.Add("TransactionId", email.TransactionId);
                datatemplate.Add("Description", string.IsNullOrWhiteSpace(email.Description) ? "-" : email.Description);
                datatemplate.Add("CustomerId", email.CustomerId);

                EmailParser ParserHelper = new EmailParser(template.BodyContent, datatemplate, false); 
                string emailbody = ParserHelper.Parse(); 
                email.EmailBody = HttpUtility.HtmlDecode(emailbody);
                
                Hashtable templateVars = new Hashtable();
                templateVars.Add("EMAILBODY", email.EmailBody);
                templateVars.Add("CustomerName", email.CustomerName);
                templateVars.Add("CustomerNo", string.IsNullOrWhiteSpace(email.CustomerNo)?"-": email.CustomerNo);
                templateVars.Add("CompanyName", email.CompanyName);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("CustomerId", email.CustomerId);
                templateVars.Add("EmployeeId", email.EmployeeId);
                if (SentEmail(templateVars, EmailTemplateKey.EmailToCustomerForTransaction.mailtoCustomerforTransaction, gCompanyId, null))
                {

                    if (!string.IsNullOrWhiteSpace(email.ToSalesPersonsEmail) && !string.IsNullOrWhiteSpace(email.SalesPersonsName))
                    {
                        templateVars.Add("SalesPersonsName", email.SalesPersonsName);
                        templateVars.Add("ToSalesPersonsEmail", email.ToSalesPersonsEmail);
                        templateVars.Add("InvoiceId", email.InvoiceId);
                        SentEmail(templateVars, EmailTemplateKey.EmailToCustomerForTransaction.mailtoSalesPersonforTransaction, gCompanyId, null);

                    }

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

        public bool EmailToQALeadtoCusConverFor(LeadtoCustomer email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();


                templateVars.Add("CustomerName", email.CustomerName);
                templateVars.Add("CustomerAddress", email.CustomerAddress);
                templateVars.Add("EmployeeName", email.EmployeeName);

                templateVars.Add("ToEmail", email.ToEmail);
                if (SentEmail(templateVars, EmailTemplateKey.EmailToLeadtoCusforQA.QAforLeadtoCusConvert, gCompanyId, null))
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

        public bool EmailToCustomerForCancellation(LeadtoCustomerCancellation email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();


                templateVars.Add("CustomerName", email.CustomerName);
                templateVars.Add("CustomerAddress", email.CustomerAddress);
                templateVars.Add("EmployeeName", email.EmployeeName);

                List<Attachment> att = new List<Attachment>();
                att.Add(email.fileManagementpdf);

                templateVars.Add("ToEmail", email.ToEmail);
                if (SentEmail(templateVars, EmailTemplateKey.CustomerCancellationConfirm.CancellationConfirm, gCompanyId, att))
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
        public bool LeadCreateSuccessEmail(SetupLeadCustormer email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();

                templateVars.Add("CustomerName", email.CustomerName);
                templateVars.Add("EMAILBODY", email.CustomerName);
                templateVars.Add("ToEmail", email.ToEmail);
                if (SentEmail(templateVars, EmailTemplateKey.EmailLeadSuccess.mailleadSuccessSetup, gCompanyId, null))
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

        public bool SendRUGTicketAgreementEmail(RUGTicketAgreementEmail email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();

                templateVars.Add("Name", email.Name);
                templateVars.Add("Body", email.Body);
                templateVars.Add("ToEmail", email.ToEmail);
                List<Attachment> att = new List<Attachment>();
                if (email.TicketAgreementPdf != null)
                {
                    att.Add(email.TicketAgreementPdf);
                }
                if (SentEmail(templateVars, EmailTemplateKey.RUGTicketAgreementEmail.RUGTicketAgreement, gCompanyId, att))
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

        public bool SendTicketPDFEmail(TicketPDFEmailModel email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();

                templateVars.Add("Name", email.Name);
                templateVars.Add("Body", "Here is Ticket PDF Document");
                templateVars.Add("ToEmail", email.ToEmail);
                List<Attachment> att = new List<Attachment>();
                if (email.attachment != null)
                {
                    att.Add(email.attachment);
                }
                if (SentEmail(templateVars, EmailTemplateKey.TicketPDFMail.TicketPDFEmail, gCompanyId, att))
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

        public bool EmailToEmployeeWorkOrderSendMail(WorkOrderInformationSendEMail email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();


                templateVars.Add("NAME", email.Name);
                templateVars.Add("EMAILBODY", email.EmailBody);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("ServiceListTable", email.ServiceListTable);
                templateVars.Add("WorkInstallType", email.WorkInstallType);
                templateVars.Add("WorkAmount", email.WorkAmount);
                if (SentEmail(templateVars, EmailTemplateKey.WorkOrderInformationSendEMail.WorkOrderInformationSendMail, gCompanyId, null))
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

        public bool EmailToEmployeeServiceOrderSendMail(ServiceOrderInformationSendEMail email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();


                templateVars.Add("NAME", email.Name);
                templateVars.Add("EMAILBODY", email.EmailBody);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("ServiceList", email.ServiceList);
                if (SentEmail(templateVars, EmailTemplateKey.ServiceOrderInformationSendEMail.ServiceOrderInformationSendMail, gCompanyId, null))
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

        public bool EmailToCustomerWorkOrderComplete(EmailWorkOrderComplete email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();


                templateVars.Add("NAME", email.Name);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("ServiceName", email.WorkOrderProductName);
                templateVars.Add("Quantity", email.WorkOrderProductQuantity);
                templateVars.Add("UnitPrice", email.WorkOrderProductUnitPrice);
                templateVars.Add("TotalPrice", email.WorkOrderTotalPrice);
                if (SentEmail(templateVars, EmailTemplateKey.EmailWorkOrderComplete.mailWorkOrderComplete, gCompanyId, null))
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

        public bool EmailToCustomerServiceOrderComplete(EmailServicekOrderComplete email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();


                templateVars.Add("NAME", email.Name);
                templateVars.Add("ToEmail", email.ToEmail);
                if (SentEmail(templateVars, EmailTemplateKey.EmailServiceOrderComplete.mailServiceOrderComplete, gCompanyId, null))
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

        public bool EmailToLeadFromCurrentLoggedinUser(EmailToLeadFromCurrentLoggedinUser email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();


                templateVars.Add("NAME", email.Name);
                templateVars.Add("EMAILBODY", email.EmailBody);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("Subject", email.Subject);
                if (SentEmail(templateVars, EmailTemplateKey.EmailToLeadFromCurrentLoggedinUser.EmailToLeadFromCurrentLoggedInUser, gCompanyId, null))
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

        public bool SendMailRequestAdmin(RequestAdminEmail email)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("NAME", email.Name);
                templateVars.Add("Comapny", email.Comapny);
                templateVars.Add("Phone", email.Phone);
                templateVars.Add("Email", email.Email);
                if (SentRequest(templateVars, EmailTemplateKey.RequestAdmin.RequestToAdmin))
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

        public bool EmailConvertLeadToCustomer(EmailConvertLeadToCustomer email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();


                templateVars.Add("NAME", email.CustomerName);
                templateVars.Add("EMAILBODY", email.EmailBody);
                templateVars.Add("ToEmail", email.ToEmail);

                if (SentEmail(templateVars, EmailTemplateKey.EmailConvertLeadToCustomer.mailConvertLeadToCustomer, gCompanyId, null))
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

        public bool EmailToCreateRestaurant(SendEmailToCreateRestaurant email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();


                templateVars.Add("NAME", email.Name);
                templateVars.Add("EMAILBODY", email.Body);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("Subject", email.Subject);
                templateVars.Add("CopyrightYear", email.CopyrightYear);
                if (SentEmail(templateVars, EmailTemplateKey.EmailToCreateRestaurant.mailToCreateRestaurant, gCompanyId, null))
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

        public bool EmailNotConvertLeadToCustomer(EmailNotConvertLeadToCustomer email)
        {
            try
            {
                Hashtable templateVars = new Hashtable();


                templateVars.Add("NAME", email.CustomerName);
                templateVars.Add("EMAILBODY", email.EmailBody);
                templateVars.Add("ToEmail", email.ToEmail);

                if (SentEmailRemainder(templateVars, EmailTemplateKey.EmailNotConvertLeadToCustomer.mailNotConvertLeadToCustomer, null))
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

        public bool SendEmailAssignInventoryTechReceive(InventoryTechReceiveNotificationEmail email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("NAME", email.Name);
                templateVars.Add("EMAILBODY", email.Body);
                templateVars.Add("ToEmail", email.ToEmail);

                if (SentEmail(templateVars, EmailTemplateKey.EmailAssignInventoryTechReceive.mailAssignInventoryTechReceive, gCompanyId, null))
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

        public bool TicketCustomerNotificationmail(TicketCustomerNotificationEmail email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();


                templateVars.Add("CustomerName", email.CustomerName);
                templateVars.Add("Body", email.Body);
                templateVars.Add("ToEmail", email.ToEmail);

                if (SentEmail(templateVars, EmailTemplateKey.TicketNotificationEmail.TicketNotificationmail, gCompanyId, null))
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

        public bool EmailNotSetCustomerBilling(EmailNotSetCustomerBilling email)
        {
            try
            {
                Hashtable templateVars = new Hashtable();


                templateVars.Add("NAME", email.CustomerName);
                templateVars.Add("EMAILBODY", email.EmailBody);
                templateVars.Add("ToEmail", email.ToEmail);

                if (SentEmailRemainder(templateVars, EmailTemplateKey.EmailNotSetCustomerBilling.mailNotSetCustomerBilling, null))
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

        //Customer Note To Employee
        public bool EmailToEmployeeFromCustomerNote(EmailToEmployeeFromCustomerNote email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();


                templateVars.Add("NAME", email.CustomerName);
                templateVars.Add("EMAILBODY", email.Notes);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("RECIVERNAME", email.EmailReciver);
                if (SentEmail(templateVars, EmailTemplateKey.EmailToEmployeeFromCustomerNote.EmailToEmployeeFromCustomerNotes, gCompanyId, null))
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
        public bool SendEstimatorNotificationEmail(EstimatorApprovedEmail email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("EstimatorId", email.EstimatorId);
                templateVars.Add("Status", email.Status);
                templateVars.Add("USERNAME", email.UserName);
                templateVars.Add("NAME", email.CustomerName);
                templateVars.Add("CustomerLink", email.CustomerLink);
                templateVars.Add("EMAILBODY", email.Notes);
                templateVars.Add("ToEmail", email.ToEmail); 
                List<Attachment> att = new List<Attachment>();
                if (email.EstimatorPdf != null)
                {
                    att.Add(email.EstimatorPdf);
                }
                if (SentEmail(templateVars, EmailTemplateKey.BookingSign.EstimatorSent, gCompanyId, att))
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\FileMailError.txt"), true))
                {
                    file.WriteLine(ex.Message);
                    file.WriteLine(ex.StackTrace);
                    file.WriteLine(ex.InnerException);
                    file.Close();
                }
                //HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }

            return false;
        }

        public bool SendEstimatorApprovedEmail(EstimatorApprovedEmail email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("EstimatorId", email.EstimatorId);
                templateVars.Add("Status", email.Status);
                templateVars.Add("NAME", email.CustomerName);
                templateVars.Add("CustomerLink", email.CustomerLink);
                templateVars.Add("EMAILBODY", email.Notes);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("RECEIVERNAME", email.EmailReciver);
                templateVars.Add("SalesPersonsName", email.SalesPersonsName);
                List<Attachment> att = new List<Attachment>();
                if (email.EstimatorPdf != null)
                {
                    att.Add(email.EstimatorPdf);
                } 
                if (SentEmail(templateVars, EmailTemplateKey.BookingSign.EstimatorApprovedEmail, gCompanyId, att))
                {
                    return true;
                }
                 
            }
            catch (Exception ex)
            {
                HsErrorLog.AddElmah(ex);
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\FileMailError.txt"), true))
                {
                    file.WriteLine(ex.Message);
                    file.WriteLine(ex.StackTrace);
                    file.WriteLine(ex.InnerException);
                    file.Close();
                }
                //HsErrorLog.AddElmah(ex);
                //Logger.AddElmah(ex);
            }

            return false;
        }

        public bool EmailToResponsiblePersonFromCustomerNote(EmailToResponsiblePersonFromCustomerNote email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();


                templateVars.Add("NAME", email.CustomerNum);
                templateVars.Add("EMAILBODY", email.Notes);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("RECEIVERNAME", email.ReceiverName);
                if (SentEmail(templateVars, EmailTemplateKey.EmailToResponsiblePersonFromCustomerNote.mailToResponsiblePersonFromCustomerNote, gCompanyId, null))
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

        //[Shariful-16-9-19]
        public bool DeclineMail(DeclineMail email, Guid gCompanyId)
        {
            try
            {

                Hashtable templateVars = new Hashtable();
                templateVars.Add("CustomerName", email.CustomerName);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("DeclinationReason", email.DeclinationReason);
                if (SentEmail(templateVars, EmailTemplateKey.DeclineEmail.declineEmail, gCompanyId, null))
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
        //[~Shariful-16-9-19]

        public bool EmailToEmployeeFromFollowUpNotes(EmailToEmployeeFromFollowUpNote email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("NAME", email.CustomerName);
                templateVars.Add("EMAILBODY", email.EmailBody);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("AssignPerson", email.AssignPersonName);
                templateVars.Add("AttentionBy",email.AttnBy);
                templateVars.Add("CreatedOn",email.CreatedOn);
                templateVars.Add("CreatedByName",email.CreatedByName);
                templateVars.Add("CustomerIntId",email.CustomerIntId);

                if (SentEmail(templateVars, EmailTemplateKey.EmailToEmployeeFromFollowUpNote.mailToEmployeeFromFollowUpNote, gCompanyId, null))
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
        public bool EvaluationRemainderEmailSend(EvaluationRemainderEmail email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("EmployeeName", email.EmployeeName);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("EvaluationType", email.EvaluationType);
                templateVars.Add("LastEvaluation", email.LastEvaluation);
                templateVars.Add("NextEvaluation", email.NextEvaluation);


                if (SentEmail(templateVars, EmailTemplateKey.EvaluationRemainderEmail.mailForEvaluationRemainderEmail, gCompanyId, null))
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
        public bool SendInvoiceCreatedEmail(InvoiceCreatedEmail email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();

                templateVars.Add("FromEmail", email.FromEmail);
                templateVars.Add("ReplyEmail", email.FromEmail); 
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("BalanceDue", email.BalanceDue);
                templateVars.Add("CompanyName", email.CompanyName);
                templateVars.Add("CustomerName", email.CustomerName);
                templateVars.Add("DueDate", email.DueDate);
                templateVars.Add("InvoiceId", email.InvoiceId);
                templateVars.Add("InvoiceLink", email.InvoiceLink);
                templateVars.Add("EmailBody", email.EmailBody);
                templateVars.Add("FromName", email.FromName);
                templateVars.Add("Subject", email.Subject);
                templateVars.Add("CustomerId", email.CustomerId);
                templateVars.Add("EmployeeId", email.EmployeeId);
                if (email.ccEmail != null)
                {
                    templateVars.Add("CCEmail", email.ccEmail);
                }
                else
                {
                    templateVars.Add("CCEmail", null);
                }
                if (email.BccEmail != null)
                {
                    templateVars.Add("BccEmail", email.BccEmail);
                }
                else
                {
                    templateVars.Add("BccEmail", null);
                }
                List<Attachment> att = new List<Attachment>();
                if (email.InvoicePdf != null)
                {
                    att.Add(email.InvoicePdf);
                }
                if (SentEmail(templateVars, EmailTemplateKey.Invoice.InvoiceEmail, gCompanyId, att))
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

        public bool SendCustomerInfoEmail(SendCusInfoInEmail email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();

                templateVars.Add("FromEmail", email.FromEmail);
                templateVars.Add("ReplyEmail", email.FromEmail);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("CompanyName", email.CompanyName);
                templateVars.Add("CustomerName", email.CustomerName);
                templateVars.Add("EmailBody", email.EmailBody);
                templateVars.Add("FromName", email.FromName);

                templateVars.Add("Subject", email.Subject);

                //templateVars.Add("CustomerId", email.CustomerId);
                //templateVars.Add("EmployeeId", email.EmployeeId);
                if (email.ccEmail != null)
                {
                    templateVars.Add("CCEmail", email.ccEmail);
                }
                else
                {
                    templateVars.Add("CCEmail", null);
                }
           
              
                if (SentEmail(templateVars, EmailTemplateKey.CustomerInfo.CustomerInfoMail, gCompanyId,null))
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

        public bool SendRequisitionCreatedEmail(RequisitionCreatedEmail email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();

                templateVars.Add("FromEmail", email.FromEmail);
                templateVars.Add("ToEmail", email.ToEmail);
                //templateVars.Add("BalanceDue", email.BalanceDue);
                templateVars.Add("CompanyName", email.CompanyName);
                //templateVars.Add("CustomerName", email.CustomerName);
                //templateVars.Add("DueDate", email.DueDate);
                //templateVars.Add("InvoiceId", email.InvoiceId);
                //templateVars.Add("InvoiceLink", email.InvoiceLink);
                templateVars.Add("EmailBody", email.EmailBody);
                templateVars.Add("FromName", email.FromName);
                templateVars.Add("Subject", email.Subject);
                //templateVars.Add("CustomerId", email.CustomerId);
                //templateVars.Add("EmployeeId", email.EmployeeId);
                if (email.ccEmail != null)
                {
                    templateVars.Add("CCEmail", email.ccEmail);
                }
                else
                {
                    templateVars.Add("CCEmail", null);
                }
                List<Attachment> att = new List<Attachment>();
                if (email.RequisitionPdf != null)
                {
                    att.Add(email.RequisitionPdf);
                }
                if (SentEmail(templateVars, EmailTemplateKey.Requisition.RequisitionEmail, gCompanyId, att))
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

        public bool SendFundingEmail(FundingSendEmail email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();

                templateVars.Add("FromEmail", email.FromEmail);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("CompanyName", email.CompanyName);
                templateVars.Add("CustomerName", email.CustomerName);
                templateVars.Add("EmailBody", email.EmailBody);
                templateVars.Add("FromName", email.FromName);
                templateVars.Add("Subject", email.Subject);
                if (email.ccEmail != null)
                {
                    templateVars.Add("CCEmail", email.ccEmail);
                }
                else
                {
                    templateVars.Add("CCEmail", null);
                }
                List<Attachment> att = new List<Attachment>();
                if (email.FundingPdf != null)
                {
                    att.Add(email.FundingPdf);
                }
                if (SentEmail(templateVars, EmailTemplateKey.SendFundingEmail.SendFundingmail, gCompanyId, att))
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

        public bool SendFileAttachmentForCustomerReview(FileAttachmentEmail email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("CompanyName", email.CompanyName);
                templateVars.Add("EmailBody", email.EmailBody);
                templateVars.Add("Subject", email.Subject);
                List<Attachment> att = new List<Attachment>();
                att.Add(email.FileAttachmentPdf);
                if (SentEmail(templateVars, EmailTemplateKey.FileAttachment.FilesAttachment, gCompanyId, att))
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

        public bool SendActivityNotificationEmail(ActivityNotificationEmail email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("Name", email.Name);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("Body", email.Body);
                if (SentEmail(templateVars, EmailTemplateKey.ActivityNotification.ActivityNotifications, gCompanyId, null))
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

        public bool LateNotificationTicketEmail(LateNotificationTicketEmail email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("Name", email.CustomerName);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("Body", string.Format("Ticket #{0} from {1} is not completed yet in the due time", email.TicketId, email.CustomerName));
                if (SentEmail(templateVars, EmailTemplateKey.LateNotificationTicket.LateNotificationsTicket, gCompanyId, null))
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

        public bool EmailToPlaceOrder(EmailToOrderPlace email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();


                templateVars.Add("NAME", email.Name);
                templateVars.Add("EMAILBODY", email.Body);
                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("Subject", email.Subject);
                templateVars.Add("OrderNo", email.OrderNo);
                templateVars.Add("TotalAmount", email.TotalAmount);
                templateVars.Add("RestaurantPhone", email.RestaurantPhone);
                templateVars.Add("RestaurantLogo", email.RestaurantLogo);
                if (SentEmail(templateVars, EmailTemplateKey.EmailToOrderStatusTemp.mailToOrderStatusTemp, gCompanyId, null))
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

        public bool SendEstimateCreatedEmail(InvoiceCreatedEmail email, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();

                templateVars.Add("ToEmail", email.ToEmail);
                templateVars.Add("BalanceDue", email.BalanceDue);
                templateVars.Add("CompanyName", email.CompanyName);
                templateVars.Add("CustomerName", email.CustomerName);
                templateVars.Add("DueDate", email.DueDate);
                templateVars.Add("InvoiceId", email.InvoiceId);
                templateVars.Add("InvoiceLink", email.InvoiceLink);
                templateVars.Add("EmailBody", email.EmailBody);
                templateVars.Add("FromEmail", email.FromEmail);
                templateVars.Add("ReplyEmail", email.FromEmail);
                templateVars.Add("FromName", email.FromName);
                //templateVars.Add("Subject", email.Subject);
                templateVars.Add("CustomerId", email.CustomerId);
                templateVars.Add("EmployeeId", email.EmployeeId);
                if(email.ccEmail != null)
                {
                    templateVars.Add("CCEmail", email.ccEmail);
                }
                else
                {
                    templateVars.Add("CCEmail", null);
                }
              
                List<Attachment> att = new List<Attachment>();
                if (email.InvoicePdf != null)
                {
                    att.Add(email.InvoicePdf);
                }
                if(email.attachedmentList!=null && email.attachedmentList.Count()>0)
                {
                    att.AddRange(email.attachedmentList);
                }
                if (SentEmail(templateVars, EmailTemplateKey.Estimate.EstimateEmail, gCompanyId, att))
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

        public List<EmailTemplate> GetAllTemplateByCompanyId(Guid CompanyId)
        {
            return _EmailTemplateDataAccess.GetByQuery(string.Format(" CompanyId='{0}'",CompanyId));
        }
        
        public EmailTemplate GetTemplateByTemplateKey(string key)
        {
            return _EmailTemplateDataAccess.GetByQuery("TemplateKey='" + key + "'").FirstOrDefault();
        }

        public bool UpdateEmailTemplate(EmailTemplate et)
        {
            return _EmailTemplateDataAccess.Update(et) > 0;
        }

        public EmailHistory GetEmailHistoryByTemplateKey(string key)
        {
            return _EmailHistoryDataAccess.GetByQuery(string.Format("TemplateKey = '{0}'", key)).FirstOrDefault();
        }
        public EmailHistory GetLastEmailHistoryByTemplateKeyAndSubjectAndToEmail(string key, string Subject, string To)
        {
            return _EmailHistoryDataAccess.GetByQuery(string.Format("TemplateKey = '{0}' and EmailSubject ='{1}' And ToEmail = '{2}' order by id desc", key, Subject, To)).FirstOrDefault();
        }
        public EmailTemplate GetEmailTemplateById(int id)
        {
            return _EmailTemplateDataAccess.Get(id);
        }
       
        public bool UpdateBCCEmailByCompanyId(string emailAddress, Guid value)
        {
            return _EmailTemplateDataAccess.UpdateBCCEmailByCompanyId(emailAddress, value);
        }
        public bool UpdateReplyEmailByCompanyId(string emailAddress, Guid value)
        {
            return _EmailTemplateDataAccess.UpdateReplyEmailByCompanyId(emailAddress, value);
        }

        public long InsertEmailTemplate(EmailTemplate et)
        {
            return _EmailTemplateDataAccess.Insert(et);
        }

        #region Private 
        public string GetFacebookUrlByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;
            string key = RMRCacheKey.FacebookUrl + CompanyId.ToString();
            var globalsetting = new GlobalSetting();
            if (System.Web.HttpRuntime.Cache[key] == null)
            {
                string DataKey = "FaceBook";
                globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                result = globalsetting.Value;
                System.Web.HttpRuntime.Cache[key] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[key];
            }
            return result;
        }
        public string GetInstagramUrlByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;
            string key = RMRCacheKey.InstagramUrl + CompanyId.ToString();
            var globalsetting = new GlobalSetting();
            if (System.Web.HttpRuntime.Cache[key] == null)
            {
                string DataKey = "Instagram";
                globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                result = globalsetting!=null ? globalsetting.Value:"";
                System.Web.HttpRuntime.Cache[key] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[key];
            }
            return result;
        }
        public string GetYoutubeUrlByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;
            var globalsetting = new GlobalSetting();
            string Key = RMRCacheKey.YoutubeUrl + CompanyId.ToString();
            if (System.Web.HttpRuntime.Cache[Key] == null)
            {
                string DataKey = "Youtube";
                globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                result = globalsetting.Value;
                System.Web.HttpRuntime.Cache[Key] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[Key];
            }
            return result;
        }
        public string GetTeamNameSignatureByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;
            string TeamNameSignature = RMRCacheKey.TeamNameSignatureLoad + CompanyId.ToString();
            var globalsetting = new GlobalSetting();
            if (System.Web.HttpRuntime.Cache[TeamNameSignature] == null)
            {
                string DataKey = "TeamNameSignature";
                globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                if (globalsetting == null)
                {
                    return "";
                }
                result = globalsetting.Value;
                System.Web.HttpRuntime.Cache[TeamNameSignature] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[TeamNameSignature];
            }
            return result;
        }
        public string GetFooterCompanyInformationByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;
            string FooterCompanyInformation = RMRCacheKey.FooterCompanyInformation + CompanyId.ToString();
            var globalsetting = new GlobalSetting();
            System.Web.HttpRuntime.Cache.Remove(FooterCompanyInformation);
            if (System.Web.HttpRuntime.Cache[FooterCompanyInformation] == null)
            {
                string DataKey = "FooterCompanyInformation";
                globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                if (globalsetting == null)
                {
                    return "";
                }
                result = globalsetting.Value;
                System.Web.HttpRuntime.Cache[FooterCompanyInformation] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[FooterCompanyInformation];
            }
            return result;
        }
        public string GetCompanyNameByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;
            Company comp = _CompanyDataAccess.GetByQuery(string.Format(" Companyid  = '{0}'", CompanyId)).FirstOrDefault();
            if (comp != null)
            {
                result = comp.CompanyName;
            }
            return result;
        }
        public string GetEmailLogoByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;
            string key = RMRCacheKey.EmailLogoUrl + CompanyId.ToString();
            //var globalsetting = new GlobalSetting();
            if (System.Web.HttpRuntime.Cache[key] == null)
            {
                // string DataKey = "EmailLogo";
                //globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                //result = globalsetting.Value;
                CompanyBranch cb = _CompanyBranchDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and IsMainBranch =1", CompanyId)).FirstOrDefault();
                if (cb != null && !string.IsNullOrWhiteSpace(cb.EmailLogo))
                {
                    result = cb.EmailLogo;
                    result = AppConfig.ImageDomain + result;
                }
                else
                {
                    result = ConfigurationManager.AppSettings["Logo.DefaultEmailLogo"];
                }
                System.Web.HttpRuntime.Cache[key] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[key];
            }
            return result;
        }
        #endregion

        #region Send Email To Selected Customer
        public bool SendEmailToSelectedCustomer(SendEmailToSelectedCustomer SendEmail, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();

                templateVars.Add("FromEmail", SendEmail.FromEmail);
                templateVars.Add("ToEmail", SendEmail.ToEmail);
                templateVars.Add("BalanceDue", SendEmail.BalanceDue);
                templateVars.Add("CompanyName", SendEmail.CompanyName);
                templateVars.Add("CustomerName", SendEmail.CustomerName);
                templateVars.Add("DueDate", SendEmail.DueDate);
                templateVars.Add("InvoiceId", SendEmail.InvoiceId);
                templateVars.Add("InvoiceLink", SendEmail.InvoiceLink);
                templateVars.Add("EmailBody", SendEmail.EmailBody);
                templateVars.Add("FromName", SendEmail.FromName);
                //templateVars.Add("Subject", SendEmail.Subject);
                templateVars.Add("CustomerId", SendEmail.CustomerId);
                templateVars.Add("EmployeeId", SendEmail.EmployeeId);
                if (SendEmail.ccEmail != null)
                {
                    templateVars.Add("CCEmail", SendEmail.ccEmail);
                }
                else
                {
                    templateVars.Add("CCEmail", null);
                }
                List<Attachment> att = new List<Attachment>();
                if (SendEmail.InvoicePdf != null)
                {
                    att.Add(SendEmail.InvoicePdf);
                }
                if (SentEmail(templateVars, EmailTemplateKey.Invoice.InvoiceEmail, gCompanyId, att))
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
        #endregion
        #region Send Estimate & Contract Sign Email
        public bool SendSignNotificationEmail(NotificationEmail EmailNotifiaction, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();

                templateVars.Add("Subject", EmailNotifiaction.Subject);
                templateVars.Add("ToEmail", EmailNotifiaction.ToEmail);
                templateVars.Add("EmailBody", EmailNotifiaction.EmailBody);

                if (SentEmail(templateVars, EmailTemplateKey.Invoice.EstimateNotificationSendTemplate, gCompanyId, null))
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
        public bool SendSignNotificationEmailForFileManagement(FileManagementNotificationEmail EmailNotifiaction, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();

                //templateVars.Add("Subject", EmailNotifiaction.Subject);
                templateVars.Add("ToEmail", EmailNotifiaction.ToEmail);
                templateVars.Add("CustomerNameWithId", EmailNotifiaction.CustomerNameWithId);
                //templateVars.Add("EmailBody", EmailNotifiaction.EmailBody);

                if (SentEmail(templateVars, EmailTemplateKey.mailToFileManagement.FileManagementNotificationSendEmail, gCompanyId, null))
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
        #endregion

        #region Send Estimator In Mail
        public bool SendEstimatorInEmail(SendEstimatorInEmail SendEmail, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();

                templateVars.Add("FromEmail", SendEmail.FromEmail);
                templateVars.Add("ToEmail", SendEmail.ToEmail);
                templateVars.Add("EstimatorId", SendEmail.EstimatorId);
                templateVars.Add("CompanyName", SendEmail.CompanyName);
                templateVars.Add("CustomerName", SendEmail.CustomerName);
                templateVars.Add("FromName", SendEmail.FromName);
                templateVars.Add("Subject", SendEmail.Subject);
                templateVars.Add("ExpirationDate", SendEmail.ExpDate);
                templateVars.Add("SalesGuy", SendEmail.SalesGuy);
                templateVars.Add("SalesPhone Number", SendEmail.SalesPhone);
                templateVars.Add("url", SendEmail.Url);
                templateVars.Add("EmailBody", SendEmail.EmailBody);
                if (SendEmail.ccEmail != null)
                {
                    templateVars.Add("CCEmail", SendEmail.ccEmail);
                }
                else
                {
                    templateVars.Add("CCEmail", null);
                }
                List<Attachment> att = new List<Attachment>();
                if (SendEmail.EstimatorPdf != null)
                {
                    att.Add(SendEmail.EstimatorPdf);
                }
                if (SentEmail(templateVars, EmailTemplateKey.Estimator.EstimatorEmail, gCompanyId, att))
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
        #endregion

        public bool SendTicketNotificationEmail(NotificationEmail EmailNotifiaction, Guid gCompanyId)
        {
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("CustomerNum", EmailNotifiaction.ToWhom);
                templateVars.Add("Subject", EmailNotifiaction.Subject);
                templateVars.Add("ToEmail", EmailNotifiaction.ToEmail);
                templateVars.Add("EmailBody", EmailNotifiaction.EmailBody);

                if (SentEmail(templateVars, EmailTemplateKey.Invoice.EstimateNotificationSendTemplate, gCompanyId, null))
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