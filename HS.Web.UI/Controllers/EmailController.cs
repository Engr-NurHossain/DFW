using HS.Entities;
using HS.Framework;
using NReco.ImageGenerator;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace HS.Web.UI.Controllers
{
    public class EmailController : BaseController
    {
        // GET: Email
        public ActionResult Index()
        {
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            return View();
        }
        [Authorize]
        public ActionResult Templates()
        {

            return View();
        }

        [Authorize]
        [HttpPost]
        public JsonResult SaveBulkBcc(string EmailAddress)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (!EmailAddress.IsValidEmailAddress())
            {
                return Json(new { result = false, message = "Invalid email address." });
            }

            var result = _Util.Facade.MailFacade.UpdateBCCEmailByCompanyId(EmailAddress, CurrentUser.CompanyId.Value);
            if (result)
            {
                return Json(new { result = true, message = "BCC email address updated successfully." });
            }

            return Json(new { result = false, message = "Internal error, please contact system admin." });
        }


        [Authorize]
        [HttpPost]
        public JsonResult SaveReplyEmail(string EmailAddress)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (!EmailAddress.IsValidEmailAddress())
            {
                return Json(new { result = false, message = "Invalid email address." });
            }

            var result = _Util.Facade.MailFacade.UpdateReplyEmailByCompanyId(EmailAddress, CurrentUser.CompanyId.Value);
            if (result)
            {
                return Json(new { result = true, message = "Reply email address updated successfully." });
            }

            return Json(new { result = false, message = "Internal error, please contact system admin." });
        }
        [Authorize]
        public PartialViewResult EmailTemplate()
        {
            if (!base.IsPermitted(UserPermissions.MenuPermissions.QuickMenuMyCompanyEmailTemplateSettings))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<EmailTemplate> TemplteList = _Util.Facade.MailFacade.GetAllTemplateByCompanyId(CurrentUser.CompanyId.Value).Where(x => x.BodyFile != null && !string.IsNullOrWhiteSpace(x.BodyFile)).OrderBy(x => x.Name).ToList();
            return PartialView("_EmailTemplate", TemplteList);
        }

        public FileResult EmailTemplatePreview(int Id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            EmailTemplate et = _Util.Facade.MailFacade.GetEmailTemplateById(Id);
            if (et == null)
            {
                return null;
            }
            string HTML = "";
            if (!string.IsNullOrWhiteSpace(et.BodyContent))
            {
                HTML = et.BodyContent;
            }
            else if (!string.IsNullOrWhiteSpace(et.BodyFile))
            {
                string TemplateLoc = Server.MapPath(et.BodyFile);
                if (System.IO.File.Exists(TemplateLoc))
                {
                    HTML = System.IO.File.ReadAllText(TemplateLoc);
                }
            }

            if (string.IsNullOrWhiteSpace(HTML))
            {
                return null;
            }
            string CompanyLogo = _Util.Facade.MailFacade.GetEmailLogoByCompanyId(CurrentUser.CompanyId.Value);
            HTML = HTML.Replace("##Logo##", CompanyLogo);
            var htmlToImageConv = new NReco.ImageGenerator.HtmlToImageConverter();
            htmlToImageConv.Width = 700;
            byte[] jpegBytes = htmlToImageConv.GenerateImage(HTML, ImageFormat.Jpeg);
            return new FileStreamResult(new MemoryStream(jpegBytes), "image/jpeg");
            //using (var ms = new MemoryStream(jpegBytes))
            //{
            //    return Image.FromStream(ms,true);
            //}

        }
        [Authorize]
        public PartialViewResult EditEmailtemplate(int Id)
        {
            if (!base.IsPermitted(UserPermissions.MenuPermissions.QuickMenuMyCompanyEmailTemplateSettings))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            EmailTemplate et = _Util.Facade.MailFacade.GetEmailTemplateById(Id);
            if (et == null || et.CompanyId != CurrentUser.CompanyId.Value)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            if (string.IsNullOrWhiteSpace(et.BodyContent))
            {
                string TemplateLoc = Server.MapPath(et.BodyFile);
                if (System.IO.File.Exists(TemplateLoc))
                {
                    et.BodyContent = System.IO.File.ReadAllText(TemplateLoc);
                }
            }

            return PartialView("_EditEmailtemplate", et);
        }
        [HttpPost]
        [Authorize]
        public JsonResult EditEmailTemplate(EmailTemplate et, bool RestoreDefault)
        {
            if (!base.IsPermitted(UserPermissions.MenuPermissions.QuickMenuMyCompanyEmailTemplateSettings))
            {
                return Json(new { result = false, message = "Permission denied." });
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            EmailTemplate tempet = _Util.Facade.MailFacade.GetEmailTemplateById(et.Id);
            if (et == null || tempet.CompanyId != CurrentUser.CompanyId.Value)
            {
                return Json(new { result = false, messsage = "Access denied." });
            }
            if (RestoreDefault)
            {
                if (string.IsNullOrWhiteSpace(tempet.BodyFile))
                {
                    return Json(new { result = false, messsage = "Sorry this template can't set to default." });
                }
                else
                {
                    tempet.BodyContent = "";
                    _Util.Facade.MailFacade.UpdateEmailTemplate(tempet);
                    return Json(new { result = true, message = "Mail template restored to default successfully." });
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(et.Subject))
                {
                    tempet.Subject = et.Subject;
                }
                if (!string.IsNullOrWhiteSpace(et.BodyContent))
                {
                    tempet.BodyContent = et.BodyContent.Trim();
                }

                tempet.ToEmail = et.ToEmail;
                tempet.CcEmail = et.CcEmail;
                tempet.BccEmail = et.BccEmail;
                tempet.FromEmail = et.FromEmail;
                tempet.FromName = et.FromName;
                tempet.ReplyEmail = et.ReplyEmail;

                tempet.Description = et.Description;

                tempet.LastUpdatedBy = User.Identity.Name;
                tempet.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                _Util.Facade.MailFacade.UpdateEmailTemplate(tempet);
                return Json(new { result = true, message = "Email template updated successfully." });
            }

        }
        [Authorize]
        public ActionResult EditSMStemplate(int Id)
        {
            if (!base.IsPermitted(UserPermissions.MenuPermissions.QuickMenuMyCompanyEmailTemplateSettings))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            SMSTemplate et = _Util.Facade.SMSFacade.GetSMSTemplateById(Id);
            if (et == null || et.CompanyId != CurrentUser.CompanyId.Value)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }


            return View(et);
        }

        [HttpPost]
        [Authorize]
        public JsonResult EditSMSTemplate(SMSTemplate et, bool RestoreDefault)
        {
            if (!base.IsPermitted(UserPermissions.MenuPermissions.QuickMenuMyCompanyEmailTemplateSettings))
            {
                return Json(new { result = false, message = "Permission denied." });
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            SMSTemplate tempet = _Util.Facade.SMSFacade.GetSMSTemplateById(et.Id);
            if (et == null || tempet.CompanyId != CurrentUser.CompanyId.Value)
            {
                return Json(new { result = false, messsage = "Access denied." });
            }

            if (RestoreDefault)
            {
                if (string.IsNullOrWhiteSpace(tempet.Body))
                {
                    return Json(new { result = false, messsage = "Sorry this template can't set to default." });
                }
                else
                {
                    tempet.Body = "";
                    _Util.Facade.SMSFacade.UpdateSMSTemplate(tempet);
                    return Json(new { result = true, message = "SMS template restored to default successfully." });
                }
            }
            else
            {

                if (!string.IsNullOrWhiteSpace(et.Body))
                {
                    tempet.Body = et.Body.Trim();
                }

                tempet.TemplateKey = et.TemplateKey;
                tempet.Name = et.Name;
                tempet.CompanyId = CurrentUser.CompanyId.Value;

                tempet.Description = et.Description;

                tempet.LastUpdatedBy = CurrentUser.UserId;
                tempet.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                _Util.Facade.SMSFacade.UpdateSMSTemplate(tempet);
                return Json(new { result = true, message = "SMS template updated successfully." });
            }

        }
        [HttpPost]
        [Authorize]
        public JsonResult SendTestEmail(string ToEmail, string Subject, string BodyContent)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            if (_Util.Facade.MailFacade.SentTestEmail(ToEmail, Subject, BodyContent, CurrentUser.CompanyId.Value))
            {
                return Json(new { result = true, message = "Test email send successfully." });
            }
            return Json(new { result = false, message = "Test email send failed." });
        }
        [HttpPost]
        [Authorize]
        public JsonResult SendTestSMS(string ContactNumber, string BodyContent,string TemplateKey)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<string> ReceiverNumberList = new List<string>();
            ReceiverNumberList.Add(ContactNumber);
            if (_Util.Facade.SMSFacade.SendTestSMS(BodyContent,CurrentUser.UserId, CurrentUser.CompanyId.Value, ReceiverNumberList, false, CurrentUser.FirstName + " " + CurrentUser.LastName,TemplateKey))
            {
                return Json(new { result = true, message = "Test SMS send successfully." });
            }
            return Json(new { result = false, message = "Test SMS send failed." });
        }


    }
}