using HS.Entities;
using HS.Framework;
using HS.Framework.Utils;
using HS.Web.UI.Helper;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace HS.Web.UI.Controllers
{
    public class SurveyController : BaseController
    {

        #region Employee Survey
        // GET: Survey
        public ActionResult Index(Guid? Id)
        {
            if (Id.HasValue)
            {
                ViewBag.ReviewId = Id;
            }
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            return View();
        }
        [Authorize]
        public PartialViewResult EmployeeReviewsByUserId(Guid UserId)
        {
            List<EmployeeReview> Reviews = _Util.Facade.EmployeeReviewFacade.GetEmployeeReviewsByUserId(UserId);
            return PartialView("_EmployeeReviewsByUserId", Reviews);
        }

        [Authorize]
        [HttpPost]
        public JsonResult StartSurvey(Guid UserId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserLoginId(UserId);
            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(UserId);
            Guid SupervisorId = Guid.Empty;
            bool Result = false;
            string Message = "Supervisor not assigned.";

            if (Guid.TryParse(emp.SuperVisorId, out SupervisorId) && SupervisorId != Guid.Empty)
            {
                Employee Supervisor = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(SupervisorId);
                if (Supervisor != null)
                {
                    #region Survey for Supervisor

                    #region InsertSurvey
                    EmployeeReview review = new EmployeeReview()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        CreatedBy = CurrentUser.UserId,
                        UserId = emp.UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        ReviewId = Guid.NewGuid(),
                        LastUpdatedBy = CurrentUser.UserId,
                        LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                        ReviewedBy = Supervisor.UserId
                    };
                    review.Id = _Util.Facade.EmployeeReviewFacade.InsertEmployeeReview(review);
                    #endregion

                    #region Send Email
                    Message = "Employee review send successfully.";
                    Result = true;
                    string TicketUrl = string.Format("{2}{4}/Public/Open/?TicketId={0}&CompanyId={1}&Type={3}", review.ReviewId, CurrentUser.CompanyId.Value, AppConfig.SiteDomain, LabelHelper.PublicOpenTypes.Survey, AppConfig.DomainSitePath);
                    ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(TicketUrl, Guid.Empty);
                    SurveyEmail SupervisorSurvey = new SurveyEmail();
                    SupervisorSurvey.Name = Supervisor.FirstName + " " + Supervisor.LastName;
                    SupervisorSurvey.AssignTo = emp.FirstName.ToLower();
                    SupervisorSurvey.ToEmail = Supervisor.Email;
                    SupervisorSurvey.Subject = "Employee reviews";
                    SupervisorSurvey.RequestedBy = CurrentUser.GetFullName();
                    SupervisorSurvey.SurveyLink = AppConfig.ShortSiteDomain + "/shrt/" + ShortUrl.Code;
                    if (Supervisor.Email.IsValidEmailAddress())
                    {
                        if (_Util.Facade.MailFacade.SendSurveyEmail(SupervisorSurvey, CurrentUser.CompanyId.Value) == true)
                        {
                            Message = "Mail send successfully";
                        };
                    }
                    #endregion

                    #region SendNotification
                    Notification notification = new Notification()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        NotificationId = Guid.NewGuid(),
                        Type = LabelHelper.NotificationType.Employee,
                        Who = CurrentUser.UserId,
                        What = string.Format(@"{0} requesting for a review for {1} {2}", "{0}", emp.FirstName, emp.LastName),
                        NotificationUrl = "/EmployeeSurvey/" + review.ReviewId
                    };
                    _Util.Facade.NotificationFacade.InsertNotification(notification);
                    NotificationUser nu = new NotificationUser()
                    {
                        NotificationId = notification.NotificationId,
                        IsRead = false,
                        NotificationPerson = Supervisor.UserId,
                    };
                    _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
                    #endregion

                    #endregion

                    #region Survey for Employee

                    #region InsertSurvey
                    EmployeeReview reviewUser = new EmployeeReview()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        CreatedBy = CurrentUser.UserId,
                        UserId = emp.UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        ReviewId = Guid.NewGuid(),
                        LastUpdatedBy = CurrentUser.UserId,
                        LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                        ReviewedBy = emp.UserId
                    };
                    reviewUser.Id = _Util.Facade.EmployeeReviewFacade.InsertEmployeeReview(reviewUser);
                    #endregion

                    #region Send Email
                    string TicketUrlUsr = string.Format("{2}{4}/Public/Open/?TicketId={0}&CompanyId={1}&Type={3}", reviewUser.ReviewId, CurrentUser.CompanyId.Value, AppConfig.SiteDomain, LabelHelper.PublicOpenTypes.Survey, AppConfig.DomainSitePath);
                    ShortUrl ShortUrluser = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(TicketUrlUsr, Guid.Empty);
                    SurveyEmail surveyuser = new SurveyEmail();
                    surveyuser.Name = emp.FirstName + " " + emp.LastName;
                    surveyuser.AssignTo = emp.FirstName.ToLower();
                    surveyuser.ToEmail = emp.Email;
                    surveyuser.RequestedBy = CurrentUser.FirstName;
                    surveyuser.SurveyLink = AppConfig.ShortSiteDomain + "/shrt/" + ShortUrluser.Code;
                    surveyuser.Subject = "Employee reviews";
                    if (Supervisor.Email.IsValidEmailAddress())
                    {
                        if (_Util.Facade.MailFacade.SendSurveyEmailToUser(surveyuser, CurrentUser.CompanyId.Value) == true)
                        {
                            Message = "Mail Send  successfully";
                            Result = true;
                        };
                    }
                    #endregion

                    #region Send Notification
                    Notification notificationUser = new Notification()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        NotificationId = Guid.NewGuid(),
                        Type = LabelHelper.NotificationType.Employee,
                        Who = CurrentUser.UserId,
                        What = string.Format(@"{0} requesting for your personal review", "{0}", emp.FirstName),
                        NotificationUrl = "/EmployeeSurvey/" + reviewUser.ReviewId
                    };
                    _Util.Facade.NotificationFacade.InsertNotification(notificationUser);
                    NotificationUser nuUser = new NotificationUser()
                    {
                        NotificationId = notificationUser.NotificationId,
                        IsRead = false,
                        NotificationPerson = emp.UserId,
                    };
                    _Util.Facade.NotificationFacade.InsertNotificationUser(nuUser);
                    #endregion

                    #endregion
                }
            }

            return Json(new { result = Result, message = Message });
        }

        [Authorize]
        public PartialViewResult EmployeeSurvey(Guid ReviewId, Guid companyid)
        {
            base.SetLayoutCommons();
            ViewBag.ReviewId = ReviewId;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ViewBag.ReviewedBy = CurrentUser.UserId;
            EmployeeReview rev = _Util.Facade.EmployeeReviewFacade.GetEmployeeReviewByReviewId(ReviewId);

            #region RadioValues
            if (!rev.JobKnowledge.HasValue)
                rev.JobKnowledge = 0;
            if (!rev.WorkQuality.HasValue)
                rev.WorkQuality = 0;
            if (!rev.Attendance.HasValue)
                rev.Attendance = 0;
            if (!rev.Initiative.HasValue)
                rev.Initiative = 0;
            if (!rev.CommunicationSkills.HasValue)
                rev.CommunicationSkills = 0;
            if (!rev.Dependability.HasValue)
                rev.Dependability = 0;

            #endregion

            #region Dates
            if (!rev.ReviewDate.HasValue)
                rev.ReviewDate = DateTime.UtcNow;
            if (!rev.EmpSignatureDate.HasValue)
                rev.EmpSignatureDate =new DateTime();
            if (!rev.ManagerSignatureDate.HasValue)
                rev.ManagerSignatureDate = new DateTime();
            #endregion

            rev.Permission = "readonly";
            ViewBag.CurrentUserType = "";
            if(rev == null)
            {
                //Review Not found.
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            else if(rev.UserId == CurrentUser.UserId)
            {
                //Employee himself
                //Review for him
                rev.Permission = "edit";
                ViewBag.CurrentUserType = "employee";

                if (!rev.EmpSignatureDate.HasValue || rev.EmpSignatureDate == new DateTime())
                    rev.EmpSignatureDate = DateTime.UtcNow;
            }
            else if (CurrentUser.UserTags.IndexOf(LabelHelper.UserTags.HRManager) > -1)
            {
                //Any HR Manager
                rev.Permission = "readonly";
                ViewBag.CurrentUserType = "manager";
                if (!rev.ManagerSignatureDate.HasValue || rev.ManagerSignatureDate == new DateTime())
                    rev.ManagerSignatureDate = DateTime.UtcNow;
            }
            else if (rev.ReviewedBy == CurrentUser.UserId)
            {
                //The Person who set the review
                rev.Permission = "readonly";
                ViewBag.CurrentUserType = "manager";
                if (!rev.ManagerSignatureDate.HasValue || rev.ManagerSignatureDate == new DateTime())
                    rev.ManagerSignatureDate = DateTime.UtcNow;
            }
            else
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            if (string.IsNullOrWhiteSpace(rev.EmpName))
            {
                Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(rev.UserId);
                rev.EmpName = emp.FirstName + " " + emp.LastName;
                rev.JobTitle = emp.JobTitle;
                rev.Department = emp.Department;

                Guid SupervisorId = Guid.Empty;
                if (Guid.TryParse(emp.SuperVisorId, out SupervisorId) && SupervisorId != Guid.Empty)
                {
                    Employee empSupervisor = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(SupervisorId);
                    rev.Manager = empSupervisor.FirstName + ' ' + empSupervisor.LastName;
                }
            }
            return PartialView(rev);
        }

        [Authorize]
        [HttpPost]
        public JsonResult SaveEmployeeSurvey(EmployeeReview model)
        {
            bool result = false;
            string message = "";
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (model.ReviewId == new Guid())
            {
                return Json(new { result = false, message = "Review not found." });
            }
            EmployeeReview temp = _Util.Facade.EmployeeReviewFacade.GetEmployeeReviewByReviewId(model.ReviewId);
            if (model.ReviewedBy != CurrentUser.UserId)
            {
                return Json(new { result = false, message = "Access denied." });
            }

            model.CreatedBy = temp.CreatedBy;
            model.CreatedDate = temp.CreatedDate;
            model.UserId = temp.UserId;
            model.ReviewedBy = temp.ReviewedBy;

            model.LastUpdatedBy = CurrentUser.UserId;
            model.LastUpdatedDate = DateTime.Now.UTCCurrentTime();

            model.CompanyId = CurrentUser.CompanyId.Value;
            model.Id = temp.Id;

            model.ReviewedDate = DateTime.Now.UTCCurrentTime();

            #region Signature and signature date
            Random rand = new Random(); 
            string filePath = "";
            string[] datasplit = null;
            if (!string.IsNullOrWhiteSpace(model.Signature))
            {
                datasplit = model.Signature.Split(',');
            }
            //else if (!string.IsNullOrWhiteSpace(model.ManagerSignature))
            //{
            //    datasplit = model.ManagerSignature.Split(',');
            //}
            if(datasplit != null)
            {
                byte[] bytes = Convert.FromBase64String(datasplit[1]);
                System.Drawing.Image image;
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = System.Drawing.Image.FromStream(ms);
                    string tempFolder = ConfigurationManager.AppSettings["File.FileManagementCustomerSignature"];
                    var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                    var FtempFolderName = string.Format(tempFolder, comname) + CurrentUser.UserId.ToString() + "_Signatures";
                    string FileName = rand.Next().ToString() + "__" + "Signature.png";
                    filePath = string.Concat("/", FtempFolderName, "/", FileName);
                    string tempFolderPath = Server.MapPath("~/" + FtempFolderName);

                    if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                    {
                        try
                        {
                            image.Save(Path.Combine(tempFolderPath, FileName));
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    //filePath = string.Concat("/", FtempFolderName, "/", FileName);
                }
            }
            if (model.UserType == "employee" && !string.IsNullOrWhiteSpace(filePath) && !string.IsNullOrWhiteSpace(model.Signature) )
            {
                model.EmpSignature = filePath;
                model.EmpSignatureDate = DateTime.UtcNow;
                model.ReviewDate = DateTime.UtcNow;
            }
            else
            {
                model.EmpSignature = temp.EmpSignature;
                model.EmpSignatureDate = temp.EmpSignatureDate;
                model.ReviewDate = temp.ReviewDate;
            }

            if(model.UserType == "manager" && !string.IsNullOrWhiteSpace(filePath) && !string.IsNullOrWhiteSpace(model.Signature) )
            {
                model.ManagerSignature = filePath;
                model.ManagerSignatureDate = DateTime.UtcNow;
            }
            else
            {
                model.ManagerSignature = temp.ManagerSignature;
                model.ManagerSignatureDate = temp.ManagerSignatureDate;
            }
            #endregion

            _Util.Facade.EmployeeReviewFacade.Update(model);

            #region Surveypdf
            string SiteURL = ConfigurationManager.AppSettings["SiteURL"];
            if (!string.IsNullOrWhiteSpace(model.ManagerSignature))
            {
                model.ManagerSignature = SiteURL + model.ManagerSignature;
            }
            if (!string.IsNullOrWhiteSpace(model.EmpSignature))
            {
                model.EmpSignature = SiteURL + model.EmpSignature;
            }

            ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("~/Views/Survey/SurveyPdf.cshtml", model)
            {
                //FileName = "TestView.pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },

            };
            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
            #endregion


            SurveyEmailConfirmation surveyConfirmation = new SurveyEmailConfirmation();
            string MessageToHr = "";
            try
            {
                #region Send Email
                Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(temp.UserId);
                if (model.ReviewedBy == model.UserId)
                {
                    #region Email to Employee

                    MessageToHr = string.Format("{0} {1} has completed his/her own review. Please check.", emp.FirstName, emp.LastName);

                    surveyConfirmation.Name = emp.FirstName + " " + emp.LastName;
                    surveyConfirmation.Content = "You have completed your personal review successfully.";
                    surveyConfirmation.ToEmail = emp.Email;
                    surveyConfirmation.Subject = "Employee reviews.";

                    surveyConfirmation.ReviewPdf = new Attachment(new MemoryStream(applicationPDFData), "Review.pdf");
                    if (emp.Email.IsValidEmailAddress())
                    {
                        if (_Util.Facade.MailFacade.SendSurveyEmailConfirmation(surveyConfirmation, CurrentUser.CompanyId.Value) == true)
                        {
                            message = "Review saved successfully.";
                            result = true;
                        };
                    }
                    #endregion
                }
                else
                {
                    #region Email to Supervisor
                    Employee empUser = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(CurrentUser.UserId);
                    MessageToHr = string.Format("{0} {1} has completed review for {2} {3}. Please check.", empUser.FirstName, empUser.LastName, emp.FirstName, emp.LastName);
                    surveyConfirmation.Name = empUser.FirstName + " " + empUser.LastName;
                    surveyConfirmation.Content = string.Format(@"You have completed  review for {0} {1} successfully.", emp.FirstName, emp.LastName);
                    surveyConfirmation.ToEmail = CurrentUser.EmailAddress;
                    surveyConfirmation.Subject = "Employee reviews.";
                    //surveyConfirmation.ReplyEmail = "info@rmrcloud.com";
                    surveyConfirmation.ReviewPdf = new Attachment(new MemoryStream(applicationPDFData), "Review.pdf");
                    if (emp.Email.IsValidEmailAddress())
                    {
                        if (_Util.Facade.MailFacade.SendSurveyEmailConfirmation(surveyConfirmation, CurrentUser.CompanyId.Value) == true)
                        {
                            message = "Review saved successfully.";
                            result = true;
                        };
                    }
                    #endregion
                }
                #region Email to HR Manager
                //List<Employee> HRList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(CurrentUser.CompanyId.Value,LabelHelper.UserTags.HRManager, new Guid());
                //foreach(var item in HRList)
                //{
                //    if(item.UserId !=temp.UserId)
                //    {
                //        surveyConfirmation.Name = item.FirstName + " " + item.LastName;
                //        surveyConfirmation.Content = MessageToHr;
                //        surveyConfirmation.ToEmail = item.Email;
                //        surveyConfirmation.Subject = "Employee reviews.";
                //        //surveyConfirmation.ReplyEmail = "info@rmrcloud.com";
                //        surveyConfirmation.ReviewPdf = new Attachment(new MemoryStream(applicationPDFData), "Review.pdf");
                //        if (emp.Email.IsValidEmailAddress())
                //        {
                //            if (_Util.Facade.MailFacade.SendSurveyEmailConfirmation(surveyConfirmation, CurrentUser.CompanyId.Value) == true)
                //            {
                //                message = "Review saved successfully.";
                //                result = true;
                //            };
                //        }
                //    }
                //}
                #endregion

                #endregion
                return Json(new { result = true, message });
            }
            catch (Exception ex)
            {

                return Json(new { result = false, message = "Review not found." });
            }
        }
        public ActionResult GetSurveyPDF(Guid ReviewId)
        {
            EmployeeReview temp = _Util.Facade.EmployeeReviewFacade.GetEmployeeReviewByReviewId(ReviewId);
            #region Surveypdf

            return new Rotativa.ViewAsPdf("~/Views/Survey/SurveyPdf.cshtml", temp)
            {
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },

            };
            #endregion
        }

        public PartialViewResult SupervisorSurvey()
        {
            return PartialView();
        }
        #endregion

        #region Custom Survey
        public ActionResult RunSurvey(Guid SurveyUserId, Guid companyid)
        {
            #region Making connection string ready
            CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByCompanyId(companyid);
            if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
            {
                return RedirectToAction("Index", "Login");
            }
            string ConnectionString = CC.ConnectionString;
            if (!string.IsNullOrWhiteSpace(ConnectionString))
            {
                ConnectionString = DESEncryptionDecryption.DecryptCipherTextToPlainText(ConnectionString);

                Session[SessionKeys.CompanyConnectionString] = ConnectionString;
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
            #endregion
            CustomSurveyUser CSU = _Util.Facade.CustomSurveyFacade.GetCustomSurveyUserBySurveyUserId(SurveyUserId);
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CSU.UserId);
            CSU.ViewedDate = DateTime.Now.UTCCurrentTime();
            CSU.UserIP = AppConfig.GetIP;
            _Util.Facade.CustomSurveyFacade.UpdateCustomSurveyUser(CSU);

            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUsername(CC.UserName);

            if (CSU == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            CustomSurveyViewModel Model = new CustomSurveyViewModel();

            Model.CustomSurveyUser = CSU;
            Model.CustomSurvey = _Util.Facade.CustomSurveyFacade.GetCustomSurveyBySurveyId(CSU.SurveyId);
            if (Model.CustomSurvey == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            Model.SurveyQuestions = _Util.Facade.CustomSurveyFacade.GetSurveyQuestionsBySurveyId(CSU.SurveyId).OrderBy(x => x.OrderBy).ToList();
            ReplaceSurveyQuestionWithContractTerm(cus, Model.SurveyQuestions);
            TicketReply TR = new TicketReply()
            {
                Message = emp.FirstName + " viewed " + Model.CustomSurvey.SurveyName + " which was sent.",
                TicketId = new Guid(CSU.ReferenceId),
                RepliedDate = DateTime.Now.UTCCurrentTime(),
                IsPrivate = true,
                UserId = emp.UserId
            };

            TR.Id = _Util.Facade.TicketFacade.InsertTicketReply(TR);
            ViewBag.companyid = companyid;
            if (cus != null)
            {
                Model._customer = cus;
            }
            if (Model.SurveyQuestions == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (CSU.Status == LabelHelper.CustomSurveyStatus.Submitted)
            {
                Model.UserAnswers = _Util.Facade.CustomSurveyFacade.GetCustomSurveyUserAnswersByQuestionList(Model.SurveyQuestions.Select(x => x.QuestionId).ToList(), emp.UserId, CSU.SurveyUserId);
                return View("SurveyUserAnswers", Model);
            }
            else
            {
                Model.SurveyAnswers = _Util.Facade.CustomSurveyFacade.GetSurveyAnswersBySurveyQuestionIdList(Model.SurveyQuestions.Select(x => x.QuestionId).ToList()).OrderBy(x => x.OrderBy).ToList();
                return View(Model);
            }



        }
        [HttpPost]
        public JsonResult SaveSurvey(List<CustomSurveyUserAnswers> AnswerList, Guid companyid)
        {
            //var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            #region Making connection string ready
            CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByCompanyId(companyid);
            if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
            {
                return Json(new { result = false, message = "Survey not submitted successfully." });
            }
            string ConnectionString = CC.ConnectionString;
            if (!string.IsNullOrWhiteSpace(ConnectionString))
            {
                ConnectionString = DESEncryptionDecryption.DecryptCipherTextToPlainText(ConnectionString);

                Session[SessionKeys.CompanyConnectionString] = ConnectionString;
            }
            else
            {
                return Json(new { result = false, message = "Survey not submitted successfully." });
            }
            #endregion
            #region Validations
            if (AnswerList == null || AnswerList.Count() == 0)
            {
                return Json(new { result = false, message = "No answer is selected." });
            }

            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUsername(CC.UserName);

            Guid? CustomSurveyUserID = AnswerList.FirstOrDefault().SurveyUserId;
            if (!CustomSurveyUserID.HasValue || CustomSurveyUserID.Value == Guid.Empty)
            {
                return Json(new { result = false, message = "No survey assignment found" });
            }

            CustomSurveyUser CSU = _Util.Facade.CustomSurveyFacade.GetCustomSurveyUserBySurveyUserId(CustomSurveyUserID.Value);
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CSU.UserId);
            if (CSU == null)
            {
                return Json(new { result = false, message = "No survey assignment found" });
            }
            #endregion
            string filePathdr = "";
            string emptyCanvas = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAASwAAACWCAYAAABkW7XSAAAEYklEQVR4Xu3UAQkAAAwCwdm/9HI83BLIOdw5AgQIRAQWySkmAQIEzmB5AgIEMgIGK1OVoAQIGCw/QIBARsBgZaoSlAABg+UHCBDICBisTFWCEiBgsPwAAQIZAYOVqUpQAgQMlh8gQCAjYLAyVQlKgIDB8gMECGQEDFamKkEJEDBYfoAAgYyAwcpUJSgBAgbLDxAgkBEwWJmqBCVAwGD5AQIEMgIGK1OVoAQIGCw/QIBARsBgZaoSlAABg+UHCBDICBisTFWCEiBgsPwAAQIZAYOVqUpQAgQMlh8gQCAjYLAyVQlKgIDB8gMECGQEDFamKkEJEDBYfoAAgYyAwcpUJSgBAgbLDxAgkBEwWJmqBCVAwGD5AQIEMgIGK1OVoAQIGCw/QIBARsBgZaoSlAABg+UHCBDICBisTFWCEiBgsPwAAQIZAYOVqUpQAgQMlh8gQCAjYLAyVQlKgIDB8gMECGQEDFamKkEJEDBYfoAAgYyAwcpUJSgBAgbLDxAgkBEwWJmqBCVAwGD5AQIEMgIGK1OVoAQIGCw/QIBARsBgZaoSlAABg+UHCBDICBisTFWCEiBgsPwAAQIZAYOVqUpQAgQMlh8gQCAjYLAyVQlKgIDB8gMECGQEDFamKkEJEDBYfoAAgYyAwcpUJSgBAgbLDxAgkBEwWJmqBCVAwGD5AQIEMgIGK1OVoAQIGCw/QIBARsBgZaoSlAABg+UHCBDICBisTFWCEiBgsPwAAQIZAYOVqUpQAgQMlh8gQCAjYLAyVQlKgIDB8gMECGQEDFamKkEJEDBYfoAAgYyAwcpUJSgBAgbLDxAgkBEwWJmqBCVAwGD5AQIEMgIGK1OVoAQIGCw/QIBARsBgZaoSlAABg+UHCBDICBisTFWCEiBgsPwAAQIZAYOVqUpQAgQMlh8gQCAjYLAyVQlKgIDB8gMECGQEDFamKkEJEDBYfoAAgYyAwcpUJSgBAgbLDxAgkBEwWJmqBCVAwGD5AQIEMgIGK1OVoAQIGCw/QIBARsBgZaoSlAABg+UHCBDICBisTFWCEiBgsPwAAQIZAYOVqUpQAgQMlh8gQCAjYLAyVQlKgIDB8gMECGQEDFamKkEJEDBYfoAAgYyAwcpUJSgBAgbLDxAgkBEwWJmqBCVAwGD5AQIEMgIGK1OVoAQIGCw/QIBARsBgZaoSlAABg+UHCBDICBisTFWCEiBgsPwAAQIZAYOVqUpQAgQMlh8gQCAjYLAyVQlKgIDB8gMECGQEDFamKkEJEDBYfoAAgYyAwcpUJSgBAgbLDxAgkBEwWJmqBCVAwGD5AQIEMgIGK1OVoAQIGCw/QIBARsBgZaoSlAABg+UHCBDICBisTFWCEiBgsPwAAQIZAYOVqUpQAgQMlh8gQCAjYLAyVQlKgIDB8gMECGQEDFamKkEJEDBYfoAAgYyAwcpUJSgBAgbLDxAgkBEwWJmqBCVAwGD5AQIEMgIGK1OVoAQIGCw/QIBARsBgZaoSlACBB1YxAJfjJb2jAAAAAElFTkSuQmCC";
            _Util.Facade.CustomSurveyFacade.DeleteCustomSurveyUserAnswersByUserIdAndSurveyId(emp.UserId, CSU.SurveyId, CSU.SurveyUserId);

            foreach (var item in AnswerList)
            {
                #region signature save
                if (!string.IsNullOrWhiteSpace(item.SignPath) && item.SignPath == "true" && item.Answer != emptyCanvas)
                {
                    string[] datasplitdr = item.Answer.Split(',');
                    byte[] bytesdr = Convert.FromBase64String(datasplitdr[1]);
                    Image imagedr;
                    using (MemoryStream ms = new MemoryStream(bytesdr))
                    {
                        imagedr = Image.FromStream(ms);
                        string tempFolder = ConfigurationManager.AppSettings["File.CustomerSignatureFile"];
                        var comnname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(companyid).CompanyName.ReplaceSpecialChar();
                        var FtempFolderName = string.Format(tempFolder, comnname) + CSU.Id + "Signature";
                        Random rand = new Random();
                        string FileName = rand.Next().ToString();
                        FileName += "-___" + "Signature.png";
                        string tempFolderPath = Server.MapPath("~/" + FtempFolderName);

                        if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                        {
                            try
                            {
                                imagedr.Save(Path.Combine(tempFolderPath, FileName));
                            }
                            catch (Exception)
                            {

                            }
                        }
                        filePathdr = string.Concat("/", FtempFolderName, "/", FileName);
                    }
                    item.Answer = filePathdr;
                }
                #endregion

                item.CreatedDate = DateTime.Now.UTCCurrentTime();
                item.UserId = emp.UserId;
                if (item.QuestionId != Guid.Empty)
                {
                    _Util.Facade.CustomSurveyFacade.InsertCustomSurveyUserAnswer(item);
                }
            }

            CSU.Status = LabelHelper.CustomSurveyStatus.Submitted;
            _Util.Facade.CustomSurveyFacade.UpdateCustomSurveyUser(CSU);

            TicketReply TR = new TicketReply()
            {
                Message = emp.FirstName + " Submited his survey",
                TicketId = new Guid(CSU.ReferenceId),
                RepliedDate = DateTime.Now.UTCCurrentTime(),
                IsPrivate = true,
                UserId = emp.UserId
            };

            TR.Id = _Util.Facade.TicketFacade.InsertTicketReply(TR);

            #region Generate PDF

            #region Gather Info
            CustomSurveyViewModel Model = new CustomSurveyViewModel();
            if (cus != null)
            {
                Model._customer = cus;
            }
            Model.CustomSurveyUser = CSU;
            Model.CustomSurvey = _Util.Facade.CustomSurveyFacade.GetCustomSurveyBySurveyId(CSU.SurveyId);
            Model.SurveyQuestions = _Util.Facade.CustomSurveyFacade.GetSurveyQuestionsBySurveyId(CSU.SurveyId);
            ReplaceSurveyQuestionWithContractTerm(cus, Model.SurveyQuestions);
            Model.UserAnswers = _Util.Facade.CustomSurveyFacade.GetCustomSurveyUserAnswersByQuestionList(Model.SurveyQuestions.Select(x => x.QuestionId).ToList(), emp.UserId, CSU.SurveyUserId);
            #endregion
            ViewAsPdf EstimateActionPdf = new Rotativa.ViewAsPdf("CustomSurveyPdf", Model)
            {
                //FileName = "TestView.pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },

            };
            byte[] EstimatePdfData = EstimateActionPdf.BuildPdf(ControllerContext);
            Company com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(companyid);
            #region Save Estimate.pdf to file System  
            string estimateno = Model.CustomSurvey.SurveyName.ReplaceSpecialChar();
            string filename = ConfigurationManager.AppSettings["File.SurveyResults"];
            var comname = com.CompanyName.ReplaceSpecialChar();
            filename = string.Format(filename, comname);
            filename += DateTime.Now.UTCCurrentTime().Year.ToString()
                + "/" + DateTime.Now.UTCCurrentTime().Month.ToString()
                + "/" + estimateno + "_Survey.pdf";
            string Serverfilename = FileHelper.GetFileFullPath(filename);
            FileHelper.SaveFile(EstimatePdfData, Serverfilename);
            #endregion

            #region Save CustomerFile
            CustomerFile cuf = new CustomerFile()
            {
                CompanyId = companyid,
                FileId = Guid.NewGuid(),
                CustomerId = CSU.UserId,
                FileDescription = cus.Id + "_" + estimateno,
                FileFullName = string.Concat(estimateno, ".pdf"),
                Filename = AppConfig.DomainSitePath + (filename.IndexOf("/") == 0 ? filename : "/" + filename),
                IsActive = true,
                Uploadeddate = DateTime.UtcNow,
                CreatedBy = Guid.Empty,
                CreatedDate = DateTime.Now.UTCCurrentTime(),
                UpdatedBy = Guid.Empty,
                UpdatedDate = DateTime.Now.UTCCurrentTime()
            };
            _Util.Facade.CustomerFileFacade.InsertCustomerFile(cuf);
            #endregion
            //base.AddUserActivityForCustomer(estimateno + " Survey Is Submitted", LabelHelper.ActivityAction.AddFile, CSU.UserId, null, null);
            if (cus != null)
            {
                string Cusname = "";
                if (!String.IsNullOrWhiteSpace(cus.DBA))
                {
                    Cusname = cus.DBA;
                }
                else if (!String.IsNullOrWhiteSpace(cus.BusinessName))
                {
                    Cusname = cus.BusinessName;
                }
                else
                {
                    Cusname = cus.FirstName + ' ' + cus.LastName;
                }

                UserActivity ua = new UserActivity()
                {
                    ActivityId = Guid.NewGuid(),
                    PageUrl = Request.Url.AbsoluteUri != null ? Request.Url.AbsoluteUri : "",
                    ReferrerUrl = Request.UrlReferrer != null ? Request.UrlReferrer.AbsoluteUri : "",
                    // new paramiter
                    Action = "Survey Submitted",
                    StatsDate = DateTime.UtcNow,
                    UserId = cus.CustomerId != null ? cus.CustomerId : Guid.NewGuid(),
                    UserName = Cusname,
                    ActionDisplyText = Model.CustomSurvey.SurveyName + " Survey Is Submitted",


                    UserAgent = AppConfig.GetUserAgent != null ? AppConfig.GetUserAgent : "",
                    UserIp = AppConfig.GetIP != null ? AppConfig.GetIP : ""
                };
                Guid ActivityID = ua.ActivityId;
                _Util.Facade.UserActivityFacade.InsertUserActivity(ua);
                UserActivityCustomer uac = new UserActivityCustomer()
                {
                    ActivityId = ActivityID != null ? ActivityID : Guid.NewGuid(),

                    CustomerId = cus.CustomerId != null ? cus.CustomerId : Guid.NewGuid(),
                    RefId = cus.Id.ToString(),

                };
                _Util.Facade.UserActivityCustomerFacade.InsertUserActivityCustomer(uac);
            }
            #endregion

            return Json(new { result = true, message = "Survey submitted successfully." });
        }

        [Authorize]
        public ActionResult SurveySettingsPartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            return PartialView("SurveySettingsPartial");
        }
        [Authorize]
        public ActionResult ShowSurveyList()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<CustomSurvey> model = new List<CustomSurvey>();
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            if (CurrentUser.CompanyId.Value != new Guid())
            {
                model = _Util.Facade.CustomSurveyFacade.GetAllSurvey();
            }
            return View(model);
        }


        [Authorize]
        public ActionResult AddSurvey(int? id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CustomSurvey model = new CustomSurvey();
            if (id.HasValue && id > 0)
            {
                model = _Util.Facade.CustomSurveyFacade.GetCustomSurveyById(id.Value);
            }
            else
            {

            }

            return View(model);
        }

        [Authorize]
        public ActionResult AddQuestion(Guid SurveyId)
        {
            CustomSurveyQuestion surveyQues = new CustomSurveyQuestion();
            ViewBag.SurveyId = SurveyId;
            ViewBag.QuesTypeList = _Util.Facade.LookupFacade.GetLookupByKey("CustomQuesType").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();
            return View(surveyQues);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddQuestion(CustomSurveyQuestion customQues)
        {
            var result = false;
            var message = "";
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            try
            {
                if (customQues.QuestionId != Guid.Empty)
                {
                    CustomSurveyQuestion surveyQues = _Util.Facade.CustomSurveyFacade.GetCustomSurveyQuestionByQuestionId(customQues.QuestionId);
                    surveyQues.Question = customQues.Question;
                    surveyQues.QuestionType = customQues.QuestionType;
                    _Util.Facade.CustomSurveyFacade.UpdateCustomSurveyQuestion(surveyQues);
                    message = "Question saved successfully.";
                    result = true;
                }
                else
                {
                    customQues.QuestionId = Guid.NewGuid();
                    customQues.CreatedBy = currentLoggedIn.UserId;
                    customQues.CreatedDate = DateTime.Now.UTCCurrentTime();


                    _Util.Facade.CustomSurveyFacade.InsertCustomSurveyQuestion(customQues);
                    message = "Question saved successfully.";
                    result = true;
                }

            }
            catch (Exception ex)
            {
                message = "Question not saved";
                result = true;
            }
            return Json(new { result = result, message = message });
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddAnswer(CustomSurveyAnswer customAnswer)
        {
            var result = false;
            var message = "";
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            try
            {

                customAnswer.CreatedBy = currentLoggedIn.UserId;
                customAnswer.CreatedDate = DateTime.Now.UTCCurrentTime();
                customAnswer.AnswerId = Guid.NewGuid();


                _Util.Facade.CustomSurveyFacade.InsertCustomSurveyAnswer(customAnswer);
                message = "Answer saved successfully.";
                result = true;
            }
            catch (Exception ex)
            {
                message = "Answer not saved";
                result = true;
            }
            return Json(new { result = result, message = message });
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddSurvey(CustomSurvey customSurvey)
        {
            var result = false;
            var message = "";
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            try
            {
                if (customSurvey.Id > 0)
                {
                    //var SurveyName = _Util.Facade.CustomSurveyFacade.GetCustomSurveyById(customSurvey.Id);

                    CustomSurvey Survey = _Util.Facade.CustomSurveyFacade.GetCustomSurveyById(customSurvey.Id);
                    if (Survey == null)
                    {
                        return Json(new { result = false, message = "Survey Name not found." });
                    }
                    Survey.SurveyName = customSurvey.SurveyName;
                    _Util.Facade.CustomSurveyFacade.UpdateSurveyName(Survey);
                    message = "Survey Update successfully.";
                    result = true;
                }
                else
                {
                    List<CustomSurvey> SurveyList = _Util.Facade.CustomSurveyFacade.GetAllCustomSurveyBySurveyName(customSurvey.SurveyName);
                    foreach (var item in SurveyList)
                    {
                        if (item.SurveyName == customSurvey.SurveyName)
                        {
                            message = "Name already exist.";
                            result = false;
                            return Json(new { result = result, message = message });
                        }
                    }
                    customSurvey.SurveyId = Guid.NewGuid();
                    customSurvey.CreatedBy = currentLoggedIn.UserId;
                    customSurvey.CreatedDate = DateTime.Now.UTCCurrentTime();


                    _Util.Facade.CustomSurveyFacade.InsertCustomSurvey(customSurvey);
                    message = "Survey saved successfully.";
                    result = true;
                }

            }
            catch (Exception ex)
            {
                message = "Survey not saved";
                result = true;
            }
            return Json(new { result = result, message = message });
        }

        public ActionResult ManageSurveyUser()
        {
            return View();
        }

        public ActionResult ShowAllAssignSurveyUser(int? PageNumber, int? UnitPerPage, string SearchText)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (PageNumber == 0)
            {
                PageNumber = 1;
            }
            GlobalSetting glob = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "CustomerListPageSize");
            if (glob != null)
            {
                UnitPerPage = Convert.ToInt32(glob.Value);
            }
            else
            {
                UnitPerPage = 10;
            }
            ViewBag.OutOfNumber = _Util.Facade.CustomSurveyFacade.GetAllCustomSurveyUser().Count;
            if ((int)ViewBag.OutOfNumber == 0)
            {
                ViewBag.Message = "No Content Available !";
            }
            if (PageNumber == null || PageNumber == 0)
            {
                PageNumber = 1;
            }

            if (@ViewBag.OutOfNumber == 0)
            {
                PageNumber = 1;
            }
            ViewBag.PageNumber = PageNumber;

            if ((int)ViewBag.PageNumber * UnitPerPage > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * UnitPerPage;
            }

            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / UnitPerPage.Value);
            List<CustomSurveyUser> surveyUserList = new List<CustomSurveyUser>();
            surveyUserList = _Util.Facade.CustomSurveyFacade.GetAllCustomSurveyUserWithPagination(Convert.ToInt32(PageNumber), Convert.ToInt32(UnitPerPage), SearchText);
            return View(surveyUserList);
        }
        public ActionResult SeeResult(Guid SurveyUserId, Guid SurveyId) //int Id
        {
            CustomSurveyViewModel Model = new CustomSurveyViewModel();
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            CustomSurveyUser CSU = _Util.Facade.CustomSurveyFacade.GetCustomSurveyUserBySurveyUserId(SurveyUserId);
            Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CSU.UserId);
            Model.SurveyQuestions = _Util.Facade.CustomSurveyFacade.GetSurveyQuestionsBySurveyId(SurveyId).OrderBy(x => x.OrderBy).ToList();
            ReplaceSurveyQuestionWithContractTerm(cus, Model.SurveyQuestions);
            Model.CustomSurvey = _Util.Facade.CustomSurveyFacade.GetCustomSurveyBySurveyId(SurveyId);
            Model.SurveyAnswers = _Util.Facade.CustomSurveyFacade.GetSurveyAnswersBySurveyQuestionIdList(Model.SurveyQuestions.Select(x => x.QuestionId).ToList());
            Model.CustomSurveyUser = _Util.Facade.CustomSurveyFacade.GetCustomSurveyUserBySurveyUserId(SurveyUserId);
            //var SurveyUser = _Util.Facade.CustomSurveyFacade.GetCustomSurveyUserById(Id);
            //Model.UserAnswers = _Util.Facade.CustomSurveyFacade.GetCustomSurveyUserAnswersByQuestionList(Model.SurveyQuestions.Select(x => x.QuestionId).ToList(), Model.CustomSurveyUser.UserId, Model.CustomSurveyUser.SurveyUserId);
            Model.UserAnswers = _Util.Facade.CustomSurveyFacade.GetCustomSurveyUserAnswers(Model.SurveyQuestions.Select(x => x.QuestionId).ToList(), Model.CustomSurveyUser.SurveyUserId);
            return View(Model);
        }
        public ActionResult ShowQusAnsPanel(Guid SurveyId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            ViewBag.SurveyId = SurveyId;


            return View();
        }

        public ActionResult ShowQusestions(Guid SurveyId, string SearchText)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<CustomSurveyQuestion> surveyQues = new List<CustomSurveyQuestion>();
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            ViewBag.SurveyId = SurveyId;
            surveyQues = _Util.Facade.CustomSurveyFacade.GetAllSurveyQuestion(SurveyId, SearchText);

            return View(surveyQues);
        }

        public ActionResult ShowAnswers(Guid QuestionId)
        {
            List<CustomSurveyAnswer> surveyQuestion = new List<CustomSurveyAnswer>();
            surveyQuestion = _Util.Facade.CustomSurveyFacade.GetAllSurveyAnswerByQuestionId(QuestionId).OrderBy(x => x.OrderBy).ToList();
            ViewBag.QuestionId = QuestionId;
            return View(surveyQuestion);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteSurveyAssignUser(Guid SurveyUserId)
        {
            CustomSurveyUser surveyUser = _Util.Facade.CustomSurveyFacade.GetCustomSurveyUserBySurveyUserId(SurveyUserId);

            List<CustomSurveyUserAnswers> surveyuserans = _Util.Facade.CustomSurveyFacade.GetAllSurveyUserAnswersBySurveyUserId(SurveyUserId);
            bool result = _Util.Facade.CustomSurveyFacade.DeleteCustomSurveyUser(surveyUser.Id);

            if (result)
            {
                if (surveyuserans != null)
                {
                    foreach (var item in surveyuserans)
                    {
                        _Util.Facade.CustomSurveyFacade.DeleteCustomSurveyUserAnswersByUserIdAndSurveyId(item.UserId, item.SurveyId, item.SurveyUserId);
                    }
                }
                return Json(new { result = true, message = "Deleted successfully." });

            }
            else
            {
                return Json(new { result = false, message = "Internal error. Please report to system admin." });
            }

        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteQuestion(Guid QuestionId)
        {
            CustomSurveyQuestion surveyQues = _Util.Facade.CustomSurveyFacade.GetSurveyQuestionsByQuestionId(QuestionId);
            List<CustomSurveyAnswer> surveyAnsList = _Util.Facade.CustomSurveyFacade.GetAllSurveyAnswerByQuestionId(QuestionId);

            bool result = _Util.Facade.CustomSurveyFacade.DeleteQuestion(surveyQues.Id);
            if (result)
            {
                if (surveyAnsList != null)
                {
                    foreach (var item in surveyAnsList)
                    {
                        _Util.Facade.CustomSurveyFacade.DeleteAnswer(item.Id);
                    }
                }
                return Json(new { result = true, message = "Deleted successfully." });

            }
            else
            {
                return Json(new { result = false, message = "Internal error. Please report to system admin." });
            }

        }


        [Authorize]
        [HttpPost]
        public JsonResult DeleteAns(int Id)
        {

            bool result = _Util.Facade.CustomSurveyFacade.DeleteAnswer(Id);
            if (result)
            {
                return Json(new { result = true, message = "Deleted successfully." });
            }
            else
            {
                return Json(new { result = false, message = "Internal error. Please report to system admin." });
            }

        }

        [Authorize]
        [HttpPost]
        public JsonResult UpdateAnswer(int Id, string NewAns)
        {

            CustomSurveyAnswer surveyAns = _Util.Facade.CustomSurveyFacade.GetSurveyAnswerId(Id);
            surveyAns.Answer = NewAns;
            long result = _Util.Facade.CustomSurveyFacade.UpdateSurveyAnswer(surveyAns);
            if (result > 0)
            {

                return Json(new { result = true, message = "Answer updated successfully." });

            }
            else
            {
                return Json(new { result = false, message = "Internal error. Please report to system admin." });
            }

        }
        [Authorize]
        [HttpPost]
        public JsonResult UpdateQuesList(List<QuestionSort> QuesList)
        {
            if (QuesList.Count() == 0)
            {
                return Json(new { result = false, message = "No data selected." });
            }
            foreach (var item in QuesList)
            {
                //also can take all lookup list by data key
                //then update individually.

                CustomSurveyQuestion ques;
                if (item.Id > 0)
                {
                    ques = _Util.Facade.CustomSurveyFacade.GetCustomSurveyQuestionById(item.Id);
                    if (ques != null)
                    {
                        ques.OrderBy = item.OrderBy;

                        _Util.Facade.CustomSurveyFacade.UpdateCustomSurveyQuestion(ques);
                    }
                }


            }
            return Json(new { result = true, message = "Question Updated successfully." });

        }
        public JsonResult UpdateAnsList(List<QuestionSort> AnsList)
        {
            if (AnsList.Count() == 0)
            {
                return Json(new { result = false, message = "No data selected." });
            }
            foreach (var item in AnsList)
            {
                //also can take all lookup list by data key
                //then update individually.

                CustomSurveyAnswer Ans;
                if (item.Id > 0)
                {
                    Ans = _Util.Facade.CustomSurveyFacade.GetCustomSurveyAnswerById(item.Id);
                    if (Ans != null)
                    {
                        Ans.OrderBy = item.OrderBy;

                        _Util.Facade.CustomSurveyFacade.UpdateSurveyAnswer(Ans);
                    }
                }


            }
            return Json(new { result = true, message = "Question Updated successfully." });

        }
        #endregion

        [HttpPost]
        public JsonResult DeleteSurvey(int? Id)
        {
            bool result = false;
            if (Id.HasValue && Id.Value > 0)
            {
                var objsurvey = _Util.Facade.CustomSurveyFacade.GetCustomSurveyById(Id.Value);
                if (objsurvey != null)
                {
                    result = _Util.Facade.CustomSurveyFacade.DeleteCustomSurveyById(objsurvey.Id);
                }
            }
            return Json(result);
        }
        public List<CustomSurveyQuestion> ReplaceSurveyQuestionWithContractTerm(Customer cus,List<CustomSurveyQuestion> customSurveyQuestions)
        {
            int term = 0;
            string termMonth = "";
            string contractTerm = "";
            double contract = 0;
            if (cus != null && !string.IsNullOrEmpty(cus.ContractTeam) && cus.ContractTeam != "-1")
            {
                bool success = Double.TryParse(cus.ContractTeam, out contract);
                if (success)
                {
                    term = Convert.ToInt32(Math.Round(contract * 12));
                    if (term > 1)
                    {
                        termMonth = " month";
                    }
                    else
                    {
                        termMonth = " month";
                    }
                }
                contractTerm = string.Concat(term, termMonth);
                foreach (var item in customSurveyQuestions)
                {
                    item.Question = string.Format(item.Question, contractTerm);
                }
            }
            return customSurveyQuestions;
        }
    }
}