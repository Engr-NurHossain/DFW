using HS.Entities;
using HS.Entities.Custom;
using HS.Framework;
using HS.Framework.Utils;
using HS.Web.UI.App_Start;
using HS.Web.UI.Helper;
using Newtonsoft.Json;
using NLog;
using OS.AWS.S3.Services;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Rotativa;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Localize = HS.Web.UI.Helper.LanguageHelper;

namespace HS.Web.UI.Controllers
{
    public class PublicController : BaseController
    {
        public PublicController()
        {
            logger = LogManager.GetCurrentClassLogger();
        }
        public ActionResult DownloadFile(string Id)
        {
            if (Request.QueryString.Count == 0)
            {
                return RedirectPermanent("/");
            }
            else
            {
                string Idstr = DESEncryptionDecryption.DecryptCipherTextToPlainText(Id);
                try
                {
                    string FileName = Idstr.Substring(Idstr.LastIndexOf('/') + 1);
                    string fullName = Server.MapPath("~" + Idstr);

                    if (!System.IO.File.Exists(fullName))
                    {
                        return PartialView("~/Views/Shared/_FileNotFound.cshtml");
                    }

                    byte[] fileBytes = System.IO.File.ReadAllBytes(fullName);
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, FileName);
                }
                catch (Exception)
                {
                    return PartialView("~/Views/Shared/_FileNotFound.cshtml");
                }
            }
        }
        public ActionResult Open(Guid TicketId, Guid CompanyId, string Type)
        {
            #region Making connection string ready
            CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByCompanyId(CompanyId);
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
            if (Type == LabelHelper.PublicOpenTypes.Ticket)
            {
                Ticket Ticket = _Util.Facade.TicketFacade.GetTicketByTicketId(TicketId);
                if (Ticket != null && Ticket.CompanyId == CompanyId)
                {
                    Session["RunScriptOnDocReady"] = string.Format("OpenTicketById({0})", Ticket.Id);

                    if (User.Identity.IsAuthenticated
                        && ((HS.Web.UI.Helper.CustomPrincipal)User).CompanyId == CompanyId)
                    {
                        return RedirectToAction("Index", "App");
                    }
                    Session["PrefferedCompanyId"] = CompanyId;
                }
            }
            if (Type == LabelHelper.PublicOpenTypes.Survey)
            {
                EmployeeReview Review = _Util.Facade.EmployeeReviewFacade.GetEmployeeReviewByReviewId(TicketId);
                if (Review != null && Review.CompanyId == CompanyId)
                {
                    //Session["RunScriptOnDocReady"] = string.Format("OpenEmployeeReviewByReviewId(\'{0}\')", Review.ReviewId);

                    //if (User.Identity.IsAuthenticated
                    //    && ((HS.Web.UI.Helper.CustomPrincipal)User).CompanyId == CompanyId)
                    //{
                    //    return RedirectToAction("Index", "App");
                    //}
                    //Session["PrefferedCompanyId"] = CompanyId;
                    return RedirectToAction("EmployeeSurvey", "Survey", new { ReviewId = Review.ReviewId, companyid = CompanyId });
                }
            }
            if (Type == LabelHelper.PublicOpenTypes.CustomSurvey)
            {
                CustomSurveyUser SurveyUser = _Util.Facade.CustomSurveyFacade.GetCustomSurveyUserBySurveyUserId(TicketId);
                if (SurveyUser != null)
                {
                    //Session["RunScriptOnDocReady"] = string.Format("OpenCustomSurveyBySurveyId(\'{0}\')", TicketId); 
                    //if (User.Identity.IsAuthenticated
                    //    && ((HS.Web.UI.Helper.CustomPrincipal)User).CompanyId == CompanyId)
                    //{
                    //    return RedirectToAction("Index", "App");
                    //}
                    //Session["PrefferedCompanyId"] = CompanyId;
                    return RedirectToAction("RunSurvey", "Survey", new { SurveyUserId = TicketId, companyid = CompanyId });
                }
            }
            return RedirectToAction("Index", "Login"/*, new { personID = Person.personID }*/);
        }


        #region PurchaseOrder
        public ActionResult SupplierPO(string code)
        {
            #region Loguout if authenticated
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                SessionHelper hs = new SessionHelper();
                hs.ClearCurrentSession();
            }
            #endregion

            ViewBag.Token = code;
            string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(code).Split('#');
            Guid SupplierId = new Guid(Decryptval[2]);
            Guid CompanyId = new Guid(Decryptval[1]);
            string POID = Decryptval[0];

            #region SettingUp Company Connection
            CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByCompanyId(CompanyId.ToString());

            if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            else
            {
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
                Company Com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
                ViewBag.Title = string.Concat(Com.CompanyName, " | ", Localize.T("Purchase Order"));
            }
            #endregion

            #region SetCompanyLogo
            CompanyBranch cb = _Util.Facade.CompanyBranchFacade.GetMainBranchByCompanyId(CompanyId);
            ViewBag.CompanyLogo = cb.Logo;
            #endregion

            CreatePurchaseOrder Model = new CreatePurchaseOrder();

            Model.PurchaseOrderWarehouse = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderWarehouseByPurchaseOrderId(POID);
            if (Model.PurchaseOrderWarehouse == null)
            {
                return PartialView("~/Views/Shared/_NotFound.cshtml");
            }

            Model.PurchaseOrderDetail = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderDetailListByPurchaseOrderId(POID);
            Model.Company = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
            Model.Supplier = _Util.Facade.SupplierFacade.GetSupplierBySupplierId(Model.PurchaseOrderWarehouse.SuplierId);
            if (Model.Supplier == null)
            {
                Model.Supplier = new Supplier();
            }
            ViewBag.POShipVia = _Util.Facade.LookupFacade.GetLookupByKey("POShipVia").Select(x =>
                new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString()
                }).ToList();

            return View(Model);
        }
        #endregion

        #region Leads Agreement
        public ActionResult LeadsAgreementDocument(string code)
        {
            CustomerCompany custommerCompany = new CustomerCompany();
            string conterm = "";
            try
            {
                string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(code).Split('#');
                int LeadId = Convert.ToInt32(Decryptval[0]);
                string companyId = Decryptval[2].ToString();
                bool isRecreate = Convert.ToBoolean(Decryptval[3]);
                int templateid = Convert.ToInt32(Decryptval[4]);
                bool firstpage = Convert.ToBoolean(Decryptval[5]);
                int ticketid = 0;
                Guid TicketId = Guid.Empty;
                //Guid CustomerId = new Guid(Decryptval[0]);
                Guid GuidCompanyId = new Guid(Decryptval[2]);
                int InvoiceId = 0;//Convert.ToInt32(Decryptval[8]);
                if (Decryptval.Length > 6)
                {
                    int.TryParse(Decryptval[6], out ticketid);
                }
                // Convert.ToInt32(Decryptval[6]);

                bool isinvoice = false;

                if (Decryptval.Length > 7)
                {
                    bool.TryParse(Decryptval[7], out isinvoice);
                    //= Convert.ToBoolean(Decryptval[7]);
                }
                string invoiceid = "";
                if (Decryptval.Length > 8)
                {
                    invoiceid = Convert.ToString(Decryptval[8]);
                }
                bool isestimator = false;
                if (Decryptval.Length > 9)
                {
                    bool.TryParse(Decryptval[9], out isestimator);
                    //= Convert.ToBoolean(Decryptval[7]);
                }
                int estid = 0;
                if (Decryptval.Length > 10)
                {
                    Int32.TryParse(Decryptval[10], out estid);
                    //= Convert.ToBoolean(Decryptval[7]);
                }
                Guid userid = Guid.Empty;
                if (Decryptval.Length > 11)
                {
                    Guid.TryParse(Decryptval[11], out userid);
                    //= Convert.ToBoolean(Decryptval[7]);
                }

                bool commercial = false;
                if (Decryptval.Length > 12)
                {
                    commercial = Convert.ToBoolean(Decryptval[12]);
                    //= Convert.ToBoolean(Decryptval[7]);
                }
                string EstimatorId = "";
                if (Decryptval.Length > 13)
                {
                    EstimatorId = Convert.ToString(Decryptval[13]);
                }
                CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByCompanyId(companyId);

                if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
                {
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    Guid CompanyId = CC.CompanyId;
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
                }
                custommerCompany = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(LeadId);
                Customer cust = _Util.Facade.CustomerFacade.GetCustomerByLeadId(LeadId);
                logger.WithProperty("tags", "public,email,agreement,confirmation").WithProperty("params", JsonConvert.SerializeObject(cust)).Trace("Agreement confirmation, Lead Id {lead_id} for Customer Id {cust_id}", LeadId, cust.CustomerId);
                CustomerExtended customerExtended = new CustomerExtended();
                if (cust != null)
                {
                    customerExtended = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(cust.CustomerId);
                }
                custommerCompany.Customer = cust;
                if (custommerCompany != null)
                {
                    CompanyBranch cb = _Util.Facade.CompanyBranchFacade.GetMainBranchByCompanyId(custommerCompany.CompanyId);
                    ViewBag.CompanyLogo = cb.Logo;
                    ViewBag.LeadId = LeadId;
                    ViewBag.AgreementDocumentHeight = _Util.Facade.GlobalSettingsFacade.GetAgreementDocumentHeightByCompanyId(custommerCompany.CompanyId);
                    ViewBag.StringHeight = ViewBag.AgreementDocumentHeight + "px";
                    Guid custoid = _Util.Facade.CustomerFacade.GetCustomerById(LeadId).CustomerId;
                    //ViewBag.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(custommerCompany.CompanyId);
                    ViewBag.Singed = _Util.Facade.CustomerFacade.GetLeadSetupDetailByLeadId(custommerCompany.CustomerId);
                    custommerCompany.AgreementQuestion = _Util.Facade.AgreementFacade.GetAllAgreementQuestionByCustomerType(customerExtended.ContractType);
                    if (custommerCompany.AgreementQuestion != null && custommerCompany.AgreementQuestion.Count == 0)
                    {
                        ViewBag.NoQuestion = true;
                    }
                    else
                    {
                        ViewBag.NoQuestion = false;
                    }
                    custommerCompany.AgreementAnswer = _Util.Facade.AgreementFacade.GetAllAgreementAnswerByCustomerId(custoid);
                    var objcontract = _Util.Facade.CustomerFacade.GetCustomerById(LeadId);

                    if (!string.IsNullOrWhiteSpace(EstimatorId))
                    {
                        Estimator estimator = _Util.Facade.EstimatorFacade.GetEstimatorByEstimatorId(EstimatorId);
                        string oldStatus = "";
                        if (estimator != null)
                        {
                            oldStatus = estimator.Status;
                        }
                        if (estimator.Status != LabelHelper.EstimatorStatus.ContractSingned)
                        {
                            objcontract.IsAgreement = false;
                            _Util.Facade.CustomerFacade.UpdateCustomer(objcontract);
                        }
                        if (estimator.Status != LabelHelper.EstimatorStatus.ContractSingned && estimator.Status != LabelHelper.EstimatorStatus.Signed)
                        {
                            estimator.Status = LabelHelper.EstimatorStatus.CustomerViewed;
                        }
                        else if (estimator.Status == LabelHelper.EstimatorStatus.ContractSingned)
                        {
                            estimator.Status = LabelHelper.EstimatorStatus.ContractSingned;
                        }
                        estimator.CreatedDate = DateTime.Now.UTCCurrentTime();
                        //estimator.StartDate = DateTime.Now.UTCCurrentTime();
                        //estimator.CompletionDate = DateTime.Now.UTCCurrentTime();
                        estimator.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                        _Util.Facade.EstimatorFacade.UpdateEstimator(estimator);

                        #region log
                        UserActivity ua = new UserActivity()
                        {
                            ActivityId = Guid.NewGuid(),
                            PageUrl = Request.Url.AbsoluteUri != null ? Request.Url.AbsoluteUri : "",
                            ReferrerUrl = Request.UrlReferrer != null ? Request.UrlReferrer.AbsoluteUri : "",
                            // new paramiter
                            Action = "4264,Customer Estimate ,Public",
                            StatsDate = DateTime.UtcNow,
                            UserId = objcontract.CustomerId != null ? objcontract.CustomerId : Guid.NewGuid(),
                            UserName = objcontract.FirstName + " " + objcontract.LastName,
                            ActionDisplyText = estimator.EstimatorId + " Status Changed from " + oldStatus + " To " + estimator.Status,
                            IsARB = false,
                            UserAgent = AppConfig.GetUserAgent != null ? AppConfig.GetUserAgent : "",
                            UserIp = AppConfig.GetIP != null ? AppConfig.GetIP : ""
                        };
                        Guid ActivityID = ua.ActivityId;
                        _Util.Facade.UserActivityFacade.InsertUserActivity(ua);
                        UserActivityCustomer uac = new UserActivityCustomer()
                        {
                            ActivityId = ActivityID != null ? ActivityID : Guid.NewGuid(),

                            CustomerId = objcontract.CustomerId != null ? objcontract.CustomerId : Guid.NewGuid(),
                            RefId = estimator.EstimatorId,
                        };
                        _Util.Facade.UserActivityCustomerFacade.InsertUserActivityCustomer(uac);
                        #endregion

                        ViewBag.CustomerSignature = estimator.EstimatorSignature;
                    }
                    else
                    {
                        if (objcontract != null)
                        {
                            ViewBag.CustomerSignature = objcontract.Singature;
                        }
                    }

                    var objglo = _Util.Facade.GlobalSettingsFacade.GetGlobalsettingBySearchKeyAndCompanyId("AgreementDocumentDownload", custommerCompany.CompanyId);

                    if (objglo != null && objglo.Value.ToLower() == "true")
                    {
                        ViewBag.agreementdoc = true;
                    }
                    else
                    {
                        ViewBag.agreementdoc = false;
                    }

                    if (isRecreate == true || firstpage == true || commercial == true)
                    {
                        ViewBag.IsAgreement = false;
                    }
                    else if (isinvoice == true && !string.IsNullOrWhiteSpace(invoiceid))
                    {
                        ViewBag.IsAgreement = false;
                    }
                    else if (isestimator == true && estid > 0)
                    {
                        ViewBag.IsAgreement = false;
                    }
                    else
                    {
                        ViewBag.IsAgreement = objcontract.IsAgreement;
                    }
                    var objglobalsetting = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(custommerCompany.CompanyId, "ContractAgreementquestionnaire");
                    if (objglobalsetting != null)
                    {
                        ViewBag.IsAgreementQuestion = objglobalsetting.Value;
                    }
                    var objcontractsubmit = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(custommerCompany.CompanyId, "ContractSignAndSubmit");
                    if (objcontractsubmit != null && !string.IsNullOrWhiteSpace(objcontractsubmit.Value))
                    {
                        ViewBag.IsContractSubmit = Convert.ToBoolean(objcontractsubmit.Value);
                        logger.WithProperty("tags", "public,IsContractSubmit").WithProperty("params", JsonConvert.SerializeObject(ViewBag)).Trace("Agreement confirmation, Lead Id {lead_id} for Customer Id {cust_id}", LeadId, cust.CustomerId);

                    }
                    else
                    {
                        ViewBag.IsContractSubmit = false;
                    }
                    if (firstpage == true || commercial == true)
                    {
                        ViewBag.IsAgreementQuestion = "false";
                    }
                    ViewBag.IsRecreate = isRecreate;
                    ViewBag.firstpage = firstpage;
                    ViewBag.commercial = commercial;
                    ViewBag.ticketid = ticketid;
                    ViewBag.isinvoice = isinvoice;
                    ViewBag.invoiceid = invoiceid;
                    ViewBag.isestimator = isestimator;
                    ViewBag.estid = estid;
                    ViewBag.userid = userid;
                    ViewBag.EstimatorId = EstimatorId;

                    int term = 0;
                    double contract;
                    if (objcontract != null)
                    {
                        bool success = Double.TryParse(objcontract.ContractTeam, out contract);
                        if (success)
                        {
                            term = Convert.ToInt32(Math.Round(contract * 12));
                            ViewBag.termid = term;
                            if (term > 1)
                            {
                                ViewBag.TermMonth = " month";
                            }
                            else
                            {
                                ViewBag.TermMonth = " month";
                            }
                        }
                        conterm = string.Concat(ViewBag.termid, ViewBag.TermMonth);
                        foreach (var item in custommerCompany.AgreementQuestion)
                        {
                            item.Title = string.Format(item.Title, conterm);
                        }
                        //custommerCompany.AgreementQuestion.Where(x => x.Id == 5).FirstOrDefault().Title = string.Format(custommerCompany.AgreementQuestion.Where(x => x.Id == 5).FirstOrDefault().Title, ViewBag.termid);
                    }
                    var objcom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(custommerCompany.CompanyId);
                    if (objcom != null)
                    {
                        ViewBag.Title = string.Concat(objcom.CompanyName, " | ", Localize.T("Agreement Sign"));
                    }
                    var objglobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(custommerCompany.CompanyId, "ACHDiscount");
                    if (objglobal != null)
                    {
                        ViewBag.DiscountAmount = !string.IsNullOrWhiteSpace(objglobal.Value) ? objglobal.Value : "0";
                    }
                }
                ViewBag.Code = code;
                ViewBag.templateid = templateid;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                custommerCompany = null;
            }
            ViewBag.CustomerUiSetting = _Util.Facade.GlobalSettingsFacade.GetAllGlobalSettings().Where(x => x.Tag == "CustomerUiSettings").ToList();
            var isMobile = false;
            string u = Request.UserAgent;//ServerVariables["HTTP_USER_AGENT"];
            Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|iPad|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if ((b.IsMatch(u) || v.IsMatch(u.Substring(0, 4))))
            {
                isMobile = true;
            }

            return View(custommerCompany);

        }
        public ActionResult EstimatorSign(string code)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    FormsAuthentication.SignOut();
                    SessionHelper hs = new SessionHelper();
                    hs.ClearCurrentSession();
                }
                ViewBag.Token = code;
                List<string> estimatorlistfile = new List<string>();
                string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(code).Split('#');
                int EstimatorId = Convert.ToInt32(Decryptval[0]);
                Guid CompanyId = new Guid(Decryptval[1]);
                Guid CustomerId = new Guid(Decryptval[2]);
                string filename = Convert.ToString(Decryptval[3]);
                #region ConnectionStringSetup
                CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByCompanyId(CompanyId.ToString());

                if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                else
                {
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
                    Company Com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
                    ViewBag.Title = string.Concat(Com.CompanyName, " | ", Localize.T("Estimate Sign"));
                }
                System.Web.HttpContext.Current.Session[SessionKeys.CompanyId] = CompanyId;
                #endregion

                CompanyBranch cb = _Util.Facade.CompanyBranchFacade.GetMainBranchByCompanyId(CompanyId);
                ViewBag.CompanyLogo = cb.Logo;

                Estimator est = _Util.Facade.EstimatorFacade.GetEstimatorById(EstimatorId);

                if (CompanyId != est.CompanyId
                    || CustomerId != est.CustomerId
                    || est.Status == LabelHelper.EstimateStatus.Declined)
                {
                    if (est.Status == LabelHelper.EstimateStatus.Declined)
                    {
                        ViewBag.EstimateId = EstimatorId.GenerateEstimateNo();
                        ViewBag.IsDecline = "true";
                    }
                    return PartialView("~/Views/Shared/_LinkExpired.cshtml");
                }
                Customer tmpCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
                est.CustomerBussinessName = tmpCustomer.BusinessName;
                est.CustomerName = tmpCustomer.FirstName + " " + tmpCustomer.LastName;

                GlobalSetting EnableSign = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("EnableEstimatorSigningOption");
                if (EnableSign != null)
                {
                    ViewBag.EnableSign = EnableSign.Value.ToLower();
                }
                else
                {
                    ViewBag.EnableSign = "false";
                }

                //CustomerFile CFile = new CustomerFile();
                EstimatorFile CFile = new EstimatorFile();
                string Filename = "";
                //if (FileId > 0)
                //{
                //    bool isSinged = false;
                //    if (est.IsSigned.HasValue)
                //    {
                //        isSinged = est.IsSigned.Value;
                //    }
                //    if (isSinged)
                //    {
                //        return PartialView("~/Views/Shared/_LinkExpired.cshtml");
                //    }
                //    else
                //    {
                //        //CFile = _Util.Facade.CustomerFileFacade.GetCustomerFileById(FileId);
                //        CFile = _Util.Facade.EstimatorFacade.GetEstimatorFileById(FileId);
                //        if (CFile != null)
                //        {
                //            Filename = CFile.FileDescription;
                //        }
                //    }

                //}
                //else
                //{
                //    ViewBag.Pdf = filename;
                //}
                ViewBag.Pdf = filename;
                ViewBag.CompanyId = CompanyId.ToString();
                ViewBag.FooterCompanyInformation = _Util.Facade.MailFacade.GetFooterCompanyInformationByCompanyId(CompanyId).Replace("##Year##", string.Format("2017-{0}", DateTime.Now.Year.ToString()));
                string oldStatus = est.Status;
                if (est.Status != LabelHelper.EstimateStatus.ContractSingned || est.Status == LabelHelper.EstimatorStatus.SentToCustomer || est.Status == LabelHelper.EstimatorStatus.ResendToCustomer)
                {
                    est.Status = LabelHelper.EstimateStatus.CustomerViewed;
                }
                else if (est.Status == LabelHelper.EstimatorStatus.CustomerViewed)
                {
                    est.Status = LabelHelper.EstimateStatus.CustomerViewed;
                }
                else if (est.Status == LabelHelper.EstimatorStatus.ContractSingned)
                {
                    est.Status = LabelHelper.EstimateStatus.ContractSingned;
                }
                est.CreatedDate = DateTime.Now.UTCCurrentTime();
                //est.StartDate = DateTime.Now.UTCCurrentTime();
                //est.CompletionDate = DateTime.Now.UTCCurrentTime();
                est.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                _Util.Facade.EstimatorFacade.UpdateEstimator(est);
                if (tmpCustomer != null)
                {
                    tmpCustomer.IsAgreement = false;
                    _Util.Facade.CustomerFacade.UpdateCustomer(tmpCustomer);
                }
                string message = est.Status;
                #region log
                UserActivity ua = new UserActivity()
                {
                    ActivityId = Guid.NewGuid(),
                    PageUrl = Request.Url.AbsoluteUri != null ? Request.Url.AbsoluteUri : "",
                    ReferrerUrl = Request.UrlReferrer != null ? Request.UrlReferrer.AbsoluteUri : "",
                    // new paramiter
                    Action = "4264,Customer Estimate ,Public",
                    StatsDate = DateTime.UtcNow,
                    UserId = tmpCustomer.CustomerId != null ? tmpCustomer.CustomerId : Guid.NewGuid(),
                    UserName = tmpCustomer.FirstName + " " + tmpCustomer.LastName,
                    ActionDisplyText = est.EstimatorId + " Status Changed from " + oldStatus + " To " + est.Status,
                    IsARB = false,

                    UserAgent = AppConfig.GetUserAgent != null ? AppConfig.GetUserAgent : "",
                    UserIp = AppConfig.GetIP != null ? AppConfig.GetIP : ""
                };
                Guid ActivityID = ua.ActivityId;
                _Util.Facade.UserActivityFacade.InsertUserActivity(ua);
                UserActivityCustomer uac = new UserActivityCustomer()
                {
                    ActivityId = ActivityID != null ? ActivityID : Guid.NewGuid(),

                    CustomerId = tmpCustomer.CustomerId != null ? tmpCustomer.CustomerId : Guid.NewGuid(),
                    RefId = tmpCustomer.EstimatorId,

                };
                _Util.Facade.UserActivityCustomerFacade.InsertUserActivityCustomer(uac);
                #endregion
                return PartialView("_EstimatorSign", est);

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return PartialView("~/Views/Shared/_Loading.cshtml");
            }
        }
        [HttpPost]
        public JsonResult EstimeApproveById(int Id, string Status, string pdf)
        {
            int cusid = 0;
            string EstimateId;
            string oldStatus = "";
            var EstimatorInfo = _Util.Facade.EstimatorFacade.GetByEstimatorID(Id);
            if (EstimatorInfo != null)
            {
                oldStatus = EstimatorInfo.Status;
            }

            string EstStatus = "";
            var customerInfo = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(EstimatorInfo.CustomerId);
            var JsonResult = false;
            if (EstimatorInfo == null && customerInfo == null)
            {
                return Json(new { result = JsonResult, message = "Estimate Not found" });
            }
            else if (Status == "Approve")
            {
                EstimatorInfo.Status = LabelHelper.EstimateStatus.Accepted;
                EstimatorInfo.IsApproved = true;
                EstStatus = "approved";
            }
            else if (Status == "Decline")
            {
                EstimatorInfo.Status = LabelHelper.EstimateStatus.Declined;
                EstimatorInfo.IsApproved = false;
                EstStatus = "declined";
            }
            else
            {
                EstimatorInfo.IsApproved = true;
            }
            EstimatorInfo.CreatedDate = DateTime.Now.UTCCurrentTime();
            //EstimatorInfo.StartDate = DateTime.Now.UTCCurrentTime();
            //EstimatorInfo.CompletionDate = DateTime.Now.UTCCurrentTime();
            EstimatorInfo.CreatedByName = customerInfo.FirstName + " " + customerInfo.LastName;
            EstimatorInfo.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            JsonResult = _Util.Facade.EstimatorFacade.UpdateEstimator(EstimatorInfo);

            #region Send SMS and Email
            if (EstimatorInfo.IsApproved || EstimatorInfo.Status == LabelHelper.EstimateStatus.Declined)
            {
                GlobalSetting GlobalSettingModel = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("EstimatorApprovedSMS");
                GlobalSetting ApprovedEmail = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("EstimatorApprovedEmail");

                if (GlobalSettingModel != null && GlobalSettingModel.Value.ToLower() == "true")
                {
                    SendSMSToApprovedEstimator(customerInfo.Id, EstimatorInfo.EstimatorId, EstimatorInfo.CompanyId, EstimatorInfo.Status);
                }
                if (ApprovedEmail != null && ApprovedEmail.Value.ToLower() == "true")
                {
                    SendEmailToApprovedEstimator(customerInfo.Id, EstimatorInfo.EstimatorId, EstimatorInfo.CompanyId, pdf, EstimatorInfo.Status);
                }
            }
            #endregion Send SMS and Email

            #region log
            UserActivity ua = new UserActivity()
            {
                ActivityId = Guid.NewGuid(),
                PageUrl = Request.Url.AbsoluteUri != null ? Request.Url.AbsoluteUri : "",
                ReferrerUrl = Request.UrlReferrer != null ? Request.UrlReferrer.AbsoluteUri : "",
                // new paramiter
                Action = "4264,Customer Estimate ,Public",
                StatsDate = DateTime.UtcNow,
                UserId = customerInfo.CustomerId != null ? customerInfo.CustomerId : Guid.NewGuid(),
                UserName = customerInfo.FirstName + " " + customerInfo.LastName,
                ActionDisplyText = EstimatorInfo.EstimatorId + "Status Changed from " + oldStatus + " To " + EstimatorInfo.Status,
                IsARB = false,

                UserAgent = AppConfig.GetUserAgent != null ? AppConfig.GetUserAgent : "",
                UserIp = AppConfig.GetIP != null ? AppConfig.GetIP : ""
            };
            Guid ActivityID = ua.ActivityId;
            _Util.Facade.UserActivityFacade.InsertUserActivity(ua);
            UserActivityCustomer uac = new UserActivityCustomer()
            {
                ActivityId = ActivityID != null ? ActivityID : Guid.NewGuid(),

                CustomerId = customerInfo.CustomerId != null ? customerInfo.CustomerId : Guid.NewGuid(),
                RefId = EstimatorInfo.EstimatorId,

            };
            _Util.Facade.UserActivityCustomerFacade.InsertUserActivityCustomer(uac);
            #endregion

            return Json(new { result = JsonResult, CustomerId = customerInfo.Id, EstimatorId = EstimatorInfo.EstimatorId, message = string.Format("Estimate {0} has been {1}.", EstimatorInfo.EstimatorId, EstStatus) });
        }
        public bool SendSMSToApprovedEstimator(int? leadid, string EstimatorId, Guid CompanyGuidId, string EstimatorStatus)
        {
            string Status = "";
            //Estimator estimator = _Util.Facade.EstimatorFacade.GetByEstimatorId(EstimatorId);
            if (!string.IsNullOrWhiteSpace(EstimatorStatus))
            {
                if (EstimatorStatus == LabelHelper.EstimateStatus.Declined)
                {

                    Status = "declined";
                }
                else
                {
                    Status = "approved";
                }
            }

            if (!leadid.HasValue)
                return false;
            Guid CompanyId = new Guid();
            Guid UserId = Guid.Empty;
            if (User.Identity.IsAuthenticated)
            {
                CompanyId = CompanyGuidId;
            }
            else
            {
                CustomerCompany custommerCompany = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(leadid.Value);
                CompanyId = custommerCompany.CompanyId;
            }
            Company _Company = new Company { CompanyId = CompanyId };

            List<string> ReceiverNumberList = new List<string>();

            if (!_Util.Facade.CustomerFacade.CustomerIsInCompany(leadid.Value, CompanyId))
            {
                return false;
            }

            var Cus = _Util.Facade.CustomerFacade.GetCustomerById(leadid.Value);

            #region Send SMS

            string SalesGroup = "";
            string SalesPerson = "";
            string PrefferedNO = "";
            string[] PrefferedNOList = PrefferedNO.Split(',');
            Employee _emp = new Employee();
            if (Cus != null)
            {

                if (!string.IsNullOrWhiteSpace(Cus.Soldby))
                {
                    _emp = _Util.Facade.CustomerFacade.GetSalesGroupAndEmpNamBySoldby(new Guid(Cus.Soldby));
                    SalesGroup = _emp != null && !string.IsNullOrWhiteSpace(_emp.PermissionGroupName) ? _emp.PermissionGroupName : "";
                    SalesPerson = _emp != null && !string.IsNullOrWhiteSpace(_emp.EMPName) ? _emp.EMPName : "";
                }
            }
            #endregion


            #region ReceiverNumber Setup

            GlobalSetting GlobalSettingModel = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("EstimatorApprovedSMS");
            string ReceiverNumber = "";

            if (GlobalSettingModel != null && GlobalSettingModel.Value.ToLower() == "true")
            {
                PrefferedNO = GlobalSettingModel.OptionalValue.Replace(" ", "");
            }
            else
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(PrefferedNO))
            {
                PrefferedNO = PrefferedNO.Replace("-", "");
            }
            if (!string.IsNullOrWhiteSpace(_emp.Phone) && _emp.Phone != "administrator")
            {
                ReceiverNumber = _emp.Phone.Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            }
            PrefferedNOList = PrefferedNO.Split(',');
            if (PrefferedNOList != null && PrefferedNOList.Length > 0)
            {
                for (int i = 0; i < PrefferedNOList.Length; i++)
                {
                    if (PrefferedNOList[i].Count() == 10)
                    {
                        PrefferedNOList[i] = PrefferedNOList[i];
                    }
                    ReceiverNumberList.Add(PrefferedNOList[i]);
                }
            }
            if (!string.IsNullOrWhiteSpace(ReceiverNumber))
            {
                ReceiverNumberList.Add(ReceiverNumber);
            }

            List<string> phoneNumbers = ReceiverNumberList;
            //.Where(item => item.StartsWith("+"))
            //.ToList();

            ReceiverNumberList = phoneNumbers.Distinct().ToList();
            #endregion 
            string phonenumber = string.Join(";", ReceiverNumberList);
            string CustomerLink = AppConfig.DomainSitePath + string.Format("/Customer/Customerdetail/?id={0}", Cus.Id);
            CustomerLink = string.Concat(AppConfig.SiteDomain, CustomerLink);
            //string shortUrl2 = "";
            //ShortUrl ShortUrl2 = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(CustomerLink, null);
            //shortUrl2 = string.Concat(AppConfig.SiteDomain, "/shrt/", ShortUrl2.Code);

            if (ReceiverNumberList.Count() > 0)
            {
                bool sendResult = _Util.Facade.SMSFacade.SendEstimatorApprovedSMS(EstimatorId, CompanyId, ReceiverNumberList, false, string.Empty, UserId, Cus.Id, CustomerLink, Status);
                return sendResult;
            }
            else
            {
                return false;
            }
        }
        public bool SendEmailToApprovedEstimator(int? leadid, string EstimatorId, Guid CompanyGuidId, string pdf, string EstimatorStatus)
        {
            string Status = "";
            if (!string.IsNullOrWhiteSpace(EstimatorStatus))
            {
                if (EstimatorStatus == LabelHelper.EstimateStatus.Declined)
                {

                    Status = "declined";
                }
                else
                {
                    Status = "approved";
                }
            }
            if (!leadid.HasValue)
                return false;
            Guid CompanyId = new Guid();
            Guid UserId = Guid.Empty;
            if (User.Identity.IsAuthenticated)
            {
                CompanyId = CompanyGuidId;
            }
            else
            {
                CustomerCompany custommerCompany = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(leadid.Value);
                CompanyId = custommerCompany.CompanyId;
            }
            Company _Company = new Company { CompanyId = CompanyId };

            List<string> ReceiverEmailList = new List<string>();

            if (!_Util.Facade.CustomerFacade.CustomerIsInCompany(leadid.Value, CompanyId))
            {
                return false;
            }

            var Cus = _Util.Facade.CustomerFacade.GetCustomerById(leadid.Value);

            #region Total RMR Calculation, Lead Source, Sales Location, Ticket type And Sales Group

            string SalesGroup = "";
            string SalesPerson = "";
            if (Cus != null)
            {
                if (!string.IsNullOrWhiteSpace(Cus.Soldby))
                {
                    Employee _emp = _Util.Facade.CustomerFacade.GetSalesGroupAndEmpNamBySoldby(new Guid(Cus.Soldby));
                    if (_emp != null && !string.IsNullOrWhiteSpace(_emp.Email))
                    {
                        ReceiverEmailList.Add(_emp.Email);
                    }
                    SalesGroup = _emp != null && !string.IsNullOrWhiteSpace(_emp.PermissionGroupName) ? _emp.PermissionGroupName : "";
                    SalesPerson = _emp != null && !string.IsNullOrWhiteSpace(_emp.EMPName) ? _emp.EMPName : "";
                }
            }
            #endregion
            #region ReceiverNumber Setup

            GlobalSetting GlobalSettingModel = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("EstimatorApprovedEmail");

            string PrefferedEmail = "";
            if (GlobalSettingModel != null && GlobalSettingModel.Value.ToLower() == "true")
            {
                PrefferedEmail = GlobalSettingModel.OptionalValue.Replace(" ", "");
            }
            else
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(PrefferedEmail))
            {
                PrefferedEmail = PrefferedEmail.Replace("-", "");
            }
            else
            {
                return false;
            }

            string[] PrefferedEmailList = PrefferedEmail.Split(',');
            if (PrefferedEmailList != null && PrefferedEmailList.Length > 0)
            {
                for (int i = 0; i < PrefferedEmailList.Length; i++)
                {
                    ReceiverEmailList.Add(PrefferedEmailList[i]);
                }
            }
            ReceiverEmailList = ReceiverEmailList.Distinct().ToList();
            #endregion  
            bool sendResult = false;
            string phonenumber = string.Join(";", ReceiverEmailList);
            string Filename = "";
            Filename = pdf;
            string CustomerLink = AppConfig.DomainSitePath + string.Format("/Customer/Customerdetail/?id={0}", Cus.Id);
            CustomerLink = string.Concat(AppConfig.SiteDomain, CustomerLink);

            //string shortUrl2 = "";
            //ShortUrl ShortUrl2 = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(CustomerLink, null);
            //shortUrl2 = string.Concat(AppConfig.SiteDomain, "/shrt/", ShortUrl2.Code);

            if (ReceiverEmailList.Count() > 0 && System.IO.File.Exists(Filename))
            {
                for (int i = 0; i < ReceiverEmailList.Count; i++)
                {
                    EstimatorApprovedEmail email = new EstimatorApprovedEmail()
                    {
                        EstimatorId = EstimatorId,
                        CustomerName = string.Concat(Cus.FirstName, " ", Cus.LastName),
                        Notes = EstimatorId + " approved and sent to customer",
                        ToEmail = ReceiverEmailList[i],
                        SalesPersonsName = SalesPerson,
                        EstimatorPdf = new Attachment(
                                     FileHelper.GetFileFullPath(Filename),
                                     MediaTypeNames.Application.Octet),
                        CustomerLink = CustomerLink,
                        Status = Status
                    };
                    sendResult = _Util.Facade.MailFacade.SendEstimatorApprovedEmail(email, CompanyId);
                }
                return sendResult;
            }
            else
            {
                return false;
            }
        }


        [HttpPost]
        public JsonResult UploadEstimatorSignatureImage(string data, string token)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(data))
            {
                return Json(new { uploadImage = false, message = "invalid data" });
            }
            string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(token).Split('#');
            int EstimatorId = Convert.ToInt32(Decryptval[0]);
            Guid CompanyId = new Guid(Decryptval[1]);
            Guid CustomerId = new Guid(Decryptval[2]);
            string Estimatorfilename = Convert.ToString(Decryptval[3]);
            //int FileId = Convert.ToInt32(Decryptval[3]);
            Customer tmpCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            if (tmpCustomer == null)
            {
                return Json(new { result = false, message = "customer not found" });
            }
            if (!_Util.Facade.CustomerFacade.CustomerIsInCompany(CustomerId, CompanyId))
            {
                return Json(new { result = false, message = "invalid user" });
            }
            bool uploadImage = false;
            string filePath = "";
            string[] datasplit = data.Split(',');
            byte[] bytes = Convert.FromBase64String(datasplit[1]);
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
                string tempFolder = ConfigurationManager.AppSettings["File.CustomerSignatureFile"];
                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();
                var FtempFolderName = string.Format(tempFolder, comname) + tmpCustomer.Id + "Signature";
                Random rand = new Random();
                string FileName = rand.Next().ToString();
                FileName += "-___" + "Signature.png";
                string tempFolderPath = Server.MapPath("~/" + FtempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        image.Save(Path.Combine(tempFolderPath, FileName));
                        uploadImage = true;
                    }
                    catch (Exception)
                    {

                    }
                }
                filePath = string.Concat("/", FtempFolderName, "/", FileName);
            }
            var serverFile = Server.MapPath(filePath);
            bool rsult = _Util.Facade.CustomerFacade.CustomerIsInCompany(tmpCustomer.Id, CompanyId);

            if (!System.IO.File.Exists(serverFile))
            {
                return Json(new { result = false, message = "File not exsists" });
            }
            //tmpCustomer.Singature = AppConfig.DomainSitePath + filePath;
            //tmpCustomer.CustomerSignatureDate = DateTime.Now.UTCCurrentTime();
            //_Util.Facade.CustomerFacade.UpdateCustomer(tmpCustomer);

            Estimator est = _Util.Facade.EstimatorFacade.GetEstimatorById(EstimatorId);
            est.EstimatorSignature = AppConfig.DomainSitePath + filePath;
            est.Status = LabelHelper.EstimateStatus.Signed; //LabelHelper.EstimateStatus.Accepted;
            est.IsSigned = true;
            est.CreatedDate = DateTime.Now.UTCCurrentTime();
            _Util.Facade.EstimatorFacade.UpdateEstimator(est);

            var Estcustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(est.CustomerId);
            if (Estcustomer != null)
            {
                //if (est.IsApproved == false)
                //{
                Estcustomer.IsAgreement = false;
                _Util.Facade.CustomerFacade.UpdateCustomer(Estcustomer);
                //}
            }

            #region Make the Estimate_Signed.Pdf file

            Customer tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            Company tempComp = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
            tempComp.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CompanyId);
            CreateEstimator Model = new CreateEstimator();
            //List<CreateEstimator> CreateEstimator = new List<CreateEstimator>() { };
            CreateEstimator CreateEstimator = new CreateEstimator() { };
            string pdfname = "";

            string settingskey = @" 'InvoiceSettingsShipping', 'InvoiceSettingsDiscount', 'InvoiceSettingsDeposit'";
            //List<GlobalSetting> printsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceSettingListByCompanyIdAndKey(settingskey, CurrentUser.CompanyId.Value);
            GlobalSetting PaymentStubs = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CompanyId, "InvPreviewPaymentStubs");

            if (PaymentStubs != null)
            {
                ViewBag.PaymentStubs = PaymentStubs.Value;
            }
            else
            {
                ViewBag.PaymentStubs = "";
            }

            if (EstimatorId > 0)
            {
                Model.EstimatorSetting = new EstimatorSetting();
                Model.Company = tempComp;

                Model.Estimator = _Util.Facade.EstimatorFacade.GetById(EstimatorId);
                ViewBag.CoverLetter = Model.Estimator.CoverLetter;
                ViewBag.Signature = Model.Estimator.EstimatorSignature;
                ViewBag.SignatureDate = Model.Estimator.CreatedDate;

                Model._EstimatorPDFFilter = new EstimatorPDFFilter();

                //EstimatorPDFFilterForEmail filter = _Util.Facade.EstimatorFacade.GetEstimatorPDFFilterForEmailByFileId(FileId);
                //EstimatorFile filterestimatorfile = _Util.Facade.EstimatorFacade.GetEstimatorFileById(FileId);
                //if (filter != null)
                //{
                //    Model._EstimatorPDFFilter.AddBlankPriceColumn = filter.AddBlankPage.Value;
                //    Model._EstimatorPDFFilter.GroupedbyNone = filter.GroupedbyNone;
                //    Model._EstimatorPDFFilter.GroupedbyCategory = filter.GroupedbyCategory;
                //    Model._EstimatorPDFFilter.GroupedbyLabor = filter.GroupedbyLabor;
                //    Model._EstimatorPDFFilter.GroupedbyLaborAndMaterial = filter.GroupedbyLaborAndMaterial;
                //    Model._EstimatorPDFFilter.GroupedbyMaterial = filter.GroupedbyMaterial;
                //    Model._EstimatorPDFFilter.GroupedbySupplier = filter.GroupedbySupplier;
                //    Model._EstimatorPDFFilter.IncludeCost = filter.IncludeCost;
                //    Model._EstimatorPDFFilter.IncludeImage = filter.IncludeImage;
                //    Model._EstimatorPDFFilter.IncludeManufacturer = filter.IncludeManufacturer;
                //    Model._EstimatorPDFFilter.IncludeMargin = filter.IncludeMargin;
                //    Model._EstimatorPDFFilter.IncludeOverhead = filter.IncludeOverhead;
                //    Model._EstimatorPDFFilter.IncludePDF = filter.IncludePDF;
                //    Model._EstimatorPDFFilter.IncludeProfit = filter.IncludeProfit;
                //    Model._EstimatorPDFFilter.IncludeService = filter.IncludeService;
                //    Model._EstimatorPDFFilter.WithoutIndividualLaborPricing = filter.WithoutIndividualLaborPricing;
                //    Model._EstimatorPDFFilter.WithoutIndividualMaterialPricing = filter.WithoutIndividualMaterialPricing;
                //    Model._EstimatorPDFFilter.WithoutPricing = filter.WithoutPricing;
                //    Model._EstimatorPDFFilter.IncludeVariation = filter.IncludeVariation;
                //    Model._EstimatorPDFFilter.WithoutQTY = filter.WithoutQTY;
                //    Model._EstimatorPDFFilter.WithoutIndividualItem = filter.WithoutIndividualItem;
                //}
                //Model._EstimatorPDFFilter = _Util.Facade.EstimatorFacade.GetEstimatorPdfFilterByComIdCusId(CompanyId, Model.Estimator.CustomerId);
                Model._EstimatorPDFFilter = _Util.Facade.EstimatorFacade.NewGetEstimatorPdfFilterByComIdCusIdUserId(CompanyId, CustomerId);
                //Model.estimatorDetails = _Util.Facade.EstimatorFacade.GetEstimatorDetailListByEstimatorId(Model.Estimator.EstimatorId);
                //Model._EstimatorPDFFilter = _Util.Facade.EstimatorFacade.GetEstimatorPdfFilterByComIdCusIdUserId(CurrentUser.CompanyId.Value, CurrentUser.UserId, Model.Estimator.CustomerId);
                Model.estimatorDetails = _Util.Facade.EstimatorFacade.GetEstimatorDetailListByEstimatorIdForChild(Model.Estimator.EstimatorId);
                Model.estimatorServices = _Util.Facade.EstimatorFacade.GetEstimatorServicesByEstimatorId(Model.Estimator.EstimatorId);
                if (Model.Estimator == null || Model.Estimator.CompanyId != CompanyId)
                {
                    return null;
                }
                if ((Model.estimatorDetails == null || Model.estimatorDetails.Count() == 0) && (Model.estimatorServices == null || Model.estimatorServices.Count() == 0))
                {
                    return null;
                }
                Customer tempCUstomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Estimator.CustomerId);
                if (tempCUstomer == null)
                {
                    return null;
                }

                CreateEstimator processedModel = GetEstimatorModelById(Model.Estimator, Model.estimatorDetails, Model.estimatorServices, tempComp, tempCUstomer, Model._EstimatorPDFFilter);


                if (Model.estimatorDetails != null && Model.estimatorDetails.Count() > 0)
                {
                    foreach (var item in Model.estimatorDetails)
                    {
                        Manufacturer Manufacturer = _Util.Facade.EquipmentFacade.GetManufacturerByManufacturerId(item.ManufacturerId);
                        if (Manufacturer != null)
                        {
                            item.Manufacturer = Manufacturer.Name;
                        }
                        item.CreatedDate = DateTime.Now.UTCCurrentTime();
                        Model.SubTotal = Model.SubTotal + (item.TotalPrice.HasValue ? item.TotalPrice.Value : 0);
                        item.EquipmentFile = _Util.Facade.EquipmentFileFacade.GetEquipmentFilesByEquipmentIdAndFileType(item.EquipmentId, LabelHelper.EquipmentFileType.ProfilePicture).FirstOrDefault();
                        if (item.EquipmentFile == null)
                        {
                            item.EquipmentFile = new EquipmentFile();
                        }
                    }
                }
                //if (Model.estimatorServices != null && Model.estimatorServices.Count() > 0)
                //{
                //    foreach (var item in Model.estimatorServices)
                //    {
                //        processedModel.ServiceSubTotal += Model.ServiceSubTotal + item.Amount;
                //    }
                //    processedModel.TotalServiceAmount = processedModel.ServiceSubTotal + Model.ServiceTax;
                //}

                //CreateEstimator.Add(processedModel);

                //pdfname = EstimatorId.GenerateEstimateNo();



                if (Model.estimatorServices != null && Model.estimatorServices.Count() > 0)
                {
                    foreach (var item in Model.estimatorServices)
                    {
                        processedModel.ServiceSubTotal += Model.ServiceSubTotal + item.Amount;
                    }
                    processedModel.TotalServiceAmount = processedModel.ServiceSubTotal + Model.ServiceTax;
                }

                pdfname = processedModel.Estimator.EstimatorId;

                CreateEstimator = processedModel;

            }
            else if (Model != null)
            {
                if ((Model.estimatorDetails == null || Model.estimatorDetails.Count() == 0) && (Model.estimatorServices == null || Model.estimatorServices.Count() == 0))
                {
                    return null;
                }
                Customer tempCUstomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Estimator.CustomerId);
                if (tempCUstomer == null)
                {
                    return null;
                }
                CreateEstimator processedModel = GetEstimatorModelById(Model.Estimator, Model.estimatorDetails, Model.estimatorServices, tempComp, tempCUstomer, Model._EstimatorPDFFilter);




                Estimator estimator = _Util.Facade.EstimatorFacade.GetEstimatorByEstimatorId(Model.Estimator.EstimatorId);
                if (estimator != null)
                {
                    ViewBag.CoverLetter = estimator.CoverLetter;
                    processedModel.Estimator.CoverLetter = estimator.CoverLetter;
                    processedModel.Estimator.CoverLetterFile = estimator.CoverLetterFile;
                    processedModel.Estimator.ServicePlanType = estimator.ServicePlanType;
                    processedModel.Estimator.ServicePlanRate = estimator.ServicePlanRate;
                    processedModel.Estimator.ServicePlanAmount = estimator.ServicePlanAmount;
                    processedModel.Estimator.ServiceTaxAmount = estimator.ServiceTaxAmount;
                    processedModel.Estimator.ServiceTotalAmount = estimator.ServiceTotalAmount;
                    processedModel.Estimator.ShowServicePlan = estimator.ShowServicePlan;
                    processedModel.Estimator.ShowService = estimator.ShowService;
                    processedModel.Estimator.ServicePlanTypeName = "Service Plan";

                    SelectListItem selectListItem = _Util.Facade.LookupFacade.GetDropdownsByKey("ServicePlans").Where(x => x.Value == estimator.ServicePlanType).FirstOrDefault();
                    if (selectListItem != null)
                    {
                        processedModel.Estimator.ServicePlanTypeName = selectListItem.Text;
                    }
                    ViewBag.Signature = estimator.EstimatorSignature;
                    ViewBag.SignatureDate = estimator.CreatedDate;

                }
                if (Model.estimatorDetails != null)
                {
                    foreach (var item in Model.estimatorDetails)
                    {
                        Manufacturer Manufacturer = _Util.Facade.EquipmentFacade.GetManufacturerByManufacturerId(item.ManufacturerId);
                        if (Manufacturer != null)
                        {
                            item.Manufacturer = Manufacturer.Name;
                        }
                        item.CreatedDate = DateTime.Now.UTCCurrentTime();
                        Model.SubTotal = Model.SubTotal + (item.TotalPrice.HasValue ? item.TotalPrice.Value : 0);
                        item.EquipmentFile = _Util.Facade.EquipmentFileFacade.GetEquipmentFilesByEquipmentIdAndFileType(item.EquipmentId, LabelHelper.EquipmentFileType.ProfilePicture).FirstOrDefault();
                        if (item.EquipmentFile == null)
                        {
                            item.EquipmentFile = new EquipmentFile();
                        }
                    }
                }
                if (Model.estimatorServices != null)
                {
                    foreach (var item in Model.estimatorServices)
                    {
                        processedModel.ServiceSubTotal += Model.ServiceSubTotal + item.Amount;
                    }
                    processedModel.TotalServiceAmount = processedModel.ServiceSubTotal + Model.ServiceTax;
                }

                pdfname = processedModel.Estimator.EstimatorId;
                CreateEstimator = processedModel;
                // CreateEstimator.Add(processedModel);

                if (Model.ChildEstimator != null && Model.ChildEstimator.Count() > 0)
                {
                    foreach (int item in Model.ChildEstimator)
                    {
                        Model.Estimator = _Util.Facade.EstimatorFacade.GetById(item);
                        //Model._EstimatorPDFFilter = _Util.Facade.EstimatorFacade.GetEstimatorPdfFilterByComIdCusIdUserId(CurrentUser.CompanyId.Value, CurrentUser.UserId, Model.Estimator.CustomerId);
                        Model.estimatorDetails = _Util.Facade.EstimatorFacade.GetEstimatorDetailListByEstimatorIdForChild(Model.Estimator.EstimatorId);
                        Model.estimatorServices = _Util.Facade.EstimatorFacade.GetEstimatorServicesByEstimatorId(Model.Estimator.EstimatorId);
                        CreateEstimator processedModels = GetEstimatorModelById(Model.Estimator, Model.estimatorDetails, Model.estimatorServices, tempComp, tempCUstomer, Model._EstimatorPDFFilter);
                        CreateEstimator = processedModels;
                    }
                }
            }

            if (CreateEstimator == null)
            {
                return null;
            }
            ViewBag.CompanyId = tempComp.CompanyId.ToString();

            ViewAsPdf EstimateActionPdf = new Rotativa.ViewAsPdf("~/Views/Estimator/EstimatorPdf.cshtml", CreateEstimator)
            {
                //FileName = "TestView.pdf",
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },

            };

            byte[] EstimatePdfData = EstimateActionPdf.BuildPdf(ControllerContext);

            #region Save Estimate.Pdf to file System  
            string signedfilename = "";
            string filename = "";
            string estimateno = Model.Estimator.Id.GenerateEstimateNo();
            string filenamesigned = ConfigurationManager.AppSettings["File.EstimatorFiles"];
            var comnamee = tempComp.CompanyName.ReplaceSpecialChar();
            signedfilename = string.Format(filenamesigned, comnamee);
            Random ran = new Random();
            filename = Estimatorfilename;
            //EstimatorFile estimatorfiles = _Util.Facade.EstimatorFacade.GetEstimatorFileById(FileId);
            //if(estimatorfiles != null)
            //{
            //    filename = estimatorfiles.FileDescription;
            //}
            //else
            //{
            //    filename += DateTime.Now.UTCCurrentTime().Year.ToString()
            //    + "/" + DateTime.Now.UTCCurrentTime().Month.ToString()
            //    + "/" + ran.Next().ToString() + "_" + estimateno + "_Signed.pdf";
            //    string Serverfilename = FileHelper.GetFileFullPath(filename);
            //    FileHelper.SaveFile(EstimatePdfData, Serverfilename);
            //}
            signedfilename += DateTime.Now.UTCCurrentTime().Year.ToString()
                + "/" + DateTime.Now.UTCCurrentTime().Month.ToString()
                + "/" + ran.Next().ToString() + "_" + estimateno + "_Signed.pdf";
            string Serverfilename = FileHelper.GetFileFullPath(filenamesigned);
            FileHelper.SaveFile(EstimatePdfData, Serverfilename);

            #endregion

            if (filename.IndexOf('/') != 0)
            {
                filename = "/" + filename;
            }

            Session[SessionKeys.EstimatorPdfSession] = filename;

            #region ADD CoverLetter From PDF

            if (!string.IsNullOrWhiteSpace(Model.Estimator.CoverLetterFile)
                && Model.Estimator.CoverLetterFile.ToLower().IndexOf(".pdf") > -1)
            {
                try
                {
                    using (PdfSharp.Pdf.PdfDocument one = PdfReader.Open(Server.MapPath(Model.Estimator.CoverLetterFile), PdfDocumentOpenMode.Import))
                    using (PdfSharp.Pdf.PdfDocument two = PdfReader.Open(Server.MapPath(filename), PdfDocumentOpenMode.Import))
                    using (PdfSharp.Pdf.PdfDocument outPdf = new PdfDocument())
                    {
                        CopyPages(one, outPdf);
                        CopyPages(two, outPdf);
                        outPdf.Save(Server.MapPath(filename));
                        MemoryStream stream = new MemoryStream();
                        outPdf.Dispose();
                    }
                }
                catch (Exception ex)
                {

                }
            }

            #endregion

            #region Include PDF
            //if (Model._EstimatorPDFFilter != null && Model._EstimatorPDFFilter.IncludePDF.Value)
            //{
            //    foreach (var item in CreateEstimator)
            //    {
            //        List<EquipmentFile> AllPDFS = _Util.Facade.EquipmentFileFacade.GetAllPDFsByEquipmentIdList(item.estimatorDetails.Select(x => x.EquipmentId).ToList());
            //        if (AllPDFS.Count > 0)
            //        {
            //            foreach (var item1 in AllPDFS)
            //            {
            //                try
            //                {
            //                    using (PdfSharp.Pdf.PdfDocument one = PdfReader.Open(Server.MapPath(filename), PdfDocumentOpenMode.Import))
            //                    using (PdfSharp.Pdf.PdfDocument two = PdfReader.Open(Server.MapPath(item1.Filename), PdfDocumentOpenMode.Import))
            //                    using (PdfSharp.Pdf.PdfDocument outPdf = new PdfDocument())
            //                    {
            //                        CopyPages(one, outPdf);
            //                        CopyPages(two, outPdf);
            //                        outPdf.Save(Server.MapPath(filename));
            //                        MemoryStream stream = new MemoryStream();
            //                        outPdf.Dispose();
            //                    }
            //                }
            //                catch (Exception) { }
            //            }

            //        }
            //    }
            //}
            if (Model._EstimatorPDFFilter != null && Model._EstimatorPDFFilter.IncludePDF.Value)
            {
                List<EquipmentFile> AllPDFS = _Util.Facade.EquipmentFileFacade.GetAllPDFsByEquipmentIdList(CreateEstimator.estimatorDetails.Select(x => x.EquipmentId).ToList());
                if (AllPDFS.Count > 0)
                {
                    foreach (var item in AllPDFS)
                    {
                        try
                        {
                            using (PdfSharp.Pdf.PdfDocument one = PdfReader.Open(Server.MapPath(filename), PdfDocumentOpenMode.Import))
                            using (PdfSharp.Pdf.PdfDocument two = PdfReader.Open(Server.MapPath(item.Filename), PdfDocumentOpenMode.Import))
                            using (PdfSharp.Pdf.PdfDocument outPdf = new PdfDocument())
                            {
                                CopyPages(one, outPdf);
                                CopyPages(two, outPdf);
                                outPdf.Save(Server.MapPath(filename));
                                MemoryStream stream = new MemoryStream();
                                outPdf.Dispose();
                            }
                        }
                        catch (Exception) { }
                    }

                }
            }
            #endregion

            #region Insert into EstimatorFile
            _Util.Facade.CustomerFileFacade.SaveEstimatorPdfEstimatorFile(signedfilename, Model.Estimator.EstimatorId, CustomerId, CompanyId, true, false, false);
            #endregion

            #endregion


            return Json(new { uploadImage = uploadImage, UploadFilePath = filePath, SignPdfPath = filename }, "text/html");
        }
        void CopyPages(PdfSharp.Pdf.PdfDocument from, PdfSharp.Pdf.PdfDocument to)
        {
            for (int i = 0; i < from.PageCount; i++)
            {
                to.AddPage(from.Pages[i]);
            }
        }
        private CreateEstimator GetEstimatorModelById(Estimator Invoice, List<EstimatorDetail> InvoiceDetialList, List<EstimatorService> EstimatorServiceList, Company tempCom, Customer tempCUstomer, EstimatorPDFFilter EstimatorPDFFilters)
        {
            //var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CreateEstimator Model = new CreateEstimator();
            Model.Estimator = Invoice;
            Model.estimatorDetails = InvoiceDetialList;
            Model.estimatorServices = EstimatorServiceList;
            Model.Estimator.CustomerName = tempCUstomer.Title + " " + tempCUstomer.FirstName + " " + tempCUstomer.LastName;


            if (tempCUstomer.BusinessName != "")
            {
                Model.CustomerName = tempCUstomer.BusinessName;
            }
            else
            {
                Model.CustomerName = Model.Estimator.CustomerName;
            }
            Model.CusBussinessName = tempCUstomer.BusinessName;

            Model.SubTotal = 0;
            if (Model.estimatorDetails != null)
            {
                foreach (var item in Model.estimatorDetails)
                {
                    //   item.CreatedBy = User.Identity.Name;
                    item.CreatedDate = DateTime.Now.UTCCurrentTime();
                    //   item.CompanyId = CurrentUser.CompanyId.Value;
                    Model.SubTotal = Model.SubTotal + item.TotalPrice.Value;

                    Equipment eq = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(item.EquipmentId);
                    if (eq != null)
                    {
                        item.EquipmentDescription = eq.Comments;
                    }
                }
            }

            //if (string.IsNullOrWhiteSpace(Model.Invoice.InvoiceMessage))
            //{
            //    Model.Invoice.InvoiceMessage = _Util.Facade.GlobalSettingsFacade.GetInvoiceMessageByCompanyId(CurrentUser.CompanyId.Value);
            //}
            Model.CompanyAddress = tempCom.Address;
            Model.CompanyStreet = tempCom.Street;
            string ComCity = "";
            string ComState = "";
            if (!string.IsNullOrWhiteSpace(tempCom.City))
            {
                ComCity = tempCom.City.UppercaseFirst() + ", ";
            }
            if (!string.IsNullOrWhiteSpace(tempCom.State))
            {
                ComState = tempCom.State + " ";
            }
            #region Company Info
            Model.companyStreetInfo = ComCity + ComState + tempCom.ZipCode;
            Model.CompanyCity = tempCom.City.UppercaseFirst();
            Model.CompanyState = tempCom.State;
            Model.CompanyZip = tempCom.ZipCode;
            Model.CompanyPhone = tempCom.Phone;
            Model.CompanyEmail = tempCom.EmailAdress;
            Model.CompanyName = tempCom.CompanyName;
            Model.PhoneNo = tempCom.Phone;
            Model.CompanyWebsite = tempCom.Website;
            #endregion
            #region Customer Info
            Model.CustomerInfo = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(tempCom.CompanyId);
            Model.CustomerCity = tempCUstomer.City.UppercaseFirst();
            Model.CustomerState = tempCUstomer.State;
            Model.CustomerZipCode = tempCUstomer.ZipCode;
            Model.CustomerNo = tempCUstomer.CustomerNo;
            Model.CustomerStreet = tempCUstomer.Street;
            Model.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(tempCom.CompanyId);
            //ViewBag.ShippingValue = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(tempCom.CompanyId);
            #endregion
            Model._EstimatorPDFFilter = EstimatorPDFFilters;

            Model.ShowEstimatorShippingAddress = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(tempCom.CompanyId).ToLower() == "true" ? true : false;

            if (string.IsNullOrWhiteSpace(tempCom.CompanyLogo))
            {
                tempCom.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(tempCom.CompanyId);
            }
            Model.CompanyLogo = tempCom.CompanyLogo;
            //if (!string.IsNullOrWhiteSpace(Model.Invoice.DiscountType))
            //{
            //    if (Model.Invoice.DiscountType != "amount")
            //    {
            //        if (Model.Invoice.Discountpercent.HasValue && Model.Invoice.Discountpercent.Value > 0)
            //        {
            //            Model.Discount = ((Model.Invoice.Discountpercent / 100) * Model.SubTotal).Value;
            //        }
            //    }
            //}
            //if (Model.Invoice.BalanceDue > 0)
            //{
            //    Model.AmountInWord = NumberToWords(Model.Invoice.BalanceDue.Value); //NumberToWords(Model.Invoice.TotalAmount.Value);
            //}
            return Model;
        }

        public ActionResult LeadsCancellationAgreementDocument(string code)
        {
            CustomerCompany custommerCompany = new CustomerCompany();
            ViewBag.CustomerId = custommerCompany.CustomerId;
            CustomerCancellationQueue cusQueue = new CustomerCancellationQueue();
            string conterm = "";
            try
            {
                string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(code).Split('#');
                int LeadId = Convert.ToInt32(Decryptval[0]);
                string companyId = Decryptval[2].ToString();

                CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByCompanyId(companyId);

                if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
                {
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    Guid CompanyId = CC.CompanyId;
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
                }
                custommerCompany = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(LeadId);

                Customer cust = _Util.Facade.CustomerFacade.GetCustomerByLeadId(LeadId);
                ViewBag.LeadIdInt = cust.Id;
                cusQueue = _Util.Facade.CustomerFacade.GetCustomerCancellationQueueByCustomerId(cust.CustomerId);
                //LeadCorrespondence leadCorrespondence = _Util.Facade.LeadCorrespondenceFacade.GetLeadCorrespondenceByCustomerId(cust.CustomerId);
                //if(leadCorrespondence != null)
                //{
                //    if(leadCorrespondence.SentDate.HasValue)
                //    {
                //        //DateTime.Now.UTCCurrentTime()
                //        cusQueue.ExpireDate = leadCorrespondence.SentDate.Value.AddDays(30);
                //    } 
                //}
                System.Web.HttpContext.Current.Session[SessionKeys.CompanyId] = CC.CompanyId;
                ViewBag.CancelReasonList = _Util.Facade.LookupFacade.GetLookupByKey("CancellationReason").Select(x =>
                       new SelectListItem()
                       {
                           Text = x.DisplayText.ToString(),
                           Value = x.DataValue.ToString()
                       }).ToList();


                if (cusQueue != null)
                {
                    ViewBag.LeadId = LeadId;
                    ViewBag.AgreementDocumentHeight = _Util.Facade.GlobalSettingsFacade.GetAgreementDocumentHeightByCompanyId(custommerCompany.CompanyId);
                    ViewBag.StringHeight = ViewBag.AgreementDocumentHeight + "px";
                    Guid custoid = _Util.Facade.CustomerFacade.GetCustomerById(LeadId).CustomerId;
                    ViewBag.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(custommerCompany.CompanyId);
                    ViewBag.Singed = _Util.Facade.CustomerFacade.GetLeadSetupDetailByLeadId(custommerCompany.CustomerId);

                    var objcontract = _Util.Facade.CustomerFacade.GetCustomerById(LeadId);

                    var objcom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(custommerCompany.CompanyId);
                    if (objcom != null)
                    {
                        ViewBag.Title = string.Concat(objcom.CompanyName, " | ", Localize.T("Agreement Sign"));
                    }
                    ViewBag.IsExpaired = _Util.Facade.CustomerFacade.CheckCustomerQueueCancellationById(cusQueue.Id, cusQueue.CustomerId);
                }
                ViewBag.Code = code;
                bool CancellationReasonFillByCustomer = false;
                GlobalSetting GlobSet = _Util.Facade.GlobalSettingsFacade.GetGlobalsettingBySearchKeyAndCompanyId("CancellationReasonFillByCustomer", custommerCompany.CompanyId);
                if (GlobSet != null && !string.IsNullOrWhiteSpace(GlobSet.Value))
                {
                    if (GlobSet.Value == "true")
                    {
                        CancellationReasonFillByCustomer = true;
                    }
                }
                ViewBag.CancellationReasonFillByCustomer = CancellationReasonFillByCustomer;
            }
            catch (Exception ex)
            {
                cusQueue = null;
            }
            ViewBag.CustomerUiSetting = _Util.Facade.GlobalSettingsFacade.GetAllGlobalSettings().Where(x => x.Tag == "CustomerUiSettings").ToList();

            return View(cusQueue);
        }
        public ActionResult DisclousureLeadAgreement(Guid id)
        {
            CustomerCompany custommerCompany = new CustomerCompany();
            Company model = new Company();
            CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByCompanyId(id);

            if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
            {
                return RedirectToAction("ERRORPage", "Home");
            }
            else
            {
                Guid CompanyId = CC.CompanyId;
                string ConnectionString = CC.ConnectionString;
                if (!string.IsNullOrWhiteSpace(ConnectionString))
                {
                    ConnectionString = DESEncryptionDecryption.DecryptCipherTextToPlainText(ConnectionString);

                    Session[SessionKeys.CompanyConnectionString] = ConnectionString;
                }
                else
                {
                    return RedirectToAction("ERRORPage", "Home");
                }
            }
            if (id != new Guid())
            {
                model = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(id);
                ViewBag.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(id);
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult AgreementQuesAns(int LeadConvertId, List<ListAgreementAnswer> ListAgreementAnswer)
        {
            bool result = false;
            bool result1 = false;
            bool Ques1 = false;
            bool Ques3 = false;
            bool Ques4 = false;
            bool Ques5 = false;
            bool Ans1 = false;
            bool Ans3 = false;
            bool Ans4 = false;
            bool Ans5 = false;
            bool Additional5 = false;
            bool message5 = false;
            string message = "";
            string profilemessage = "";
            Guid comid = new Guid();
            string type = "";
            var objcus = _Util.Facade.CustomerFacade.GetCustomerById(LeadConvertId);
            var paymentprofilecustomer = _Util.Facade.PaymentInfoCustomerFacade.GetPaymentInfoCustomerByCustomerIdAndPayFor(objcus.CustomerId);
            if (paymentprofilecustomer != null)
            {
                comid = paymentprofilecustomer.CompanyId;
                type = paymentprofilecustomer.Type;
            }
            else
            {
                var custommerCompanyobj = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(LeadConvertId);
                if (custommerCompanyobj != null)
                {
                    comid = custommerCompanyobj.CompanyId;
                }
            }
            if (objcus != null)
            {
                foreach (var item in ListAgreementAnswer)
                {
                    AgreementAnswer objans1 = new AgreementAnswer()
                    {
                        QuestionId = item.SelectedQues,
                        CustomerId = objcus.CustomerId,
                        Answer = item.SelectedAns,
                        AnswerDate = DateTime.Now.UTCCurrentTime(),
                        Ip = AppConfig.GetIP,
                        UserAgent = AppConfig.GetUserAgent
                    };
                    result = _Util.Facade.AgreementFacade.InsertAgreementAnswer(objans1) > 0;
                }
                if (result == true)
                {
                    var objListAnswer = _Util.Facade.AgreementFacade.GetAllAgreementAnswerByCustomerIdAndQuestionId(objcus.CustomerId);
                    if (objListAnswer.Count > 0)
                    {
                        foreach (var item in objListAnswer)
                        {
                            if (item.QuestionId == 1)
                            {
                                Ques1 = true;
                                Ques3 = true;
                                if (item.Answer == "true")
                                {
                                    Ans1 = true;
                                }
                            }
                            if (item.QuestionId == 3)
                            {
                                if (item.Answer == "true")
                                {
                                    Ques4 = true;
                                    Ans3 = true;
                                }
                                else
                                {
                                    Ques5 = true;
                                    Additional5 = true;
                                }
                            }
                            if (item.QuestionId == 4)
                            {
                                Ques5 = true;
                                if (objcus.Type.ToLower() == "residential")
                                {
                                    if (item.Answer == "true")
                                    {
                                        Ans4 = true;
                                    }
                                }
                                else if (objcus.Type.ToLower() == "commercial")
                                {
                                    if (paymentprofilecustomer != null)
                                    {
                                        var objprofile = _Util.Facade.CustomerFacade.GetProfileByPaymentInfoId(paymentprofilecustomer.PaymentInfoId);
                                        if (objprofile != null)
                                        {
                                            string[] splitype = objprofile.Type.Split('_');
                                            if (splitype.Length > 0)
                                            {
                                                if (splitype[0] == "ACH")
                                                {
                                                    result1 = true;
                                                    if (item.Answer == "true")
                                                    {
                                                        Ans4 = true;
                                                    }
                                                }
                                                else
                                                {
                                                    result1 = true;
                                                    message5 = true;
                                                    if (item.Answer == "true")
                                                    {
                                                        Ans4 = true;
                                                    }
                                                    var objglobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(comid, "ACHDiscount");
                                                    if (objglobal != null)
                                                    {
                                                        var globalvalue = !string.IsNullOrWhiteSpace(objglobal.Value) ? objglobal.Value : "0";
                                                        if (Convert.ToDouble(globalvalue) > 0)
                                                        {
                                                            profilemessage = "Switch to ACH to get an additional " + LabelHelper.CurrentTransMakeCurrency.MakeCurrency(comid) + LabelHelper.FormatAmount(Convert.ToDouble(globalvalue)) + " discount on your monthly bill.";
                                                        }
                                                        else
                                                        {
                                                            profilemessage = "0";
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            result1 = true;
                                            message5 = true;
                                            if (item.Answer == "true")
                                            {
                                                Ans4 = true;
                                            }
                                            var objglobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(comid, "ACHDiscount");
                                            if (objglobal != null)
                                            {
                                                var globalvalue = !string.IsNullOrWhiteSpace(objglobal.Value) ? objglobal.Value : "0";
                                                if (Convert.ToDouble(globalvalue) > 0)
                                                {
                                                    profilemessage = "Switch to ACH to get an additional " + LabelHelper.CurrentTransMakeCurrency.MakeCurrency(comid) + LabelHelper.FormatAmount(Convert.ToDouble(globalvalue)) + " discount on your monthly bill.";
                                                }
                                                else
                                                {
                                                    profilemessage = "0";
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        result1 = true;
                                        message5 = true;
                                        if (item.Answer == "true")
                                        {
                                            Ans4 = true;
                                        }
                                        var objglobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(comid, "ACHDiscount");
                                        if (objglobal != null)
                                        {
                                            var globalvalue = !string.IsNullOrWhiteSpace(objglobal.Value) ? objglobal.Value : "0";
                                            if (Convert.ToDouble(globalvalue) > 0)
                                            {
                                                profilemessage = "Switch to ACH to get an additional " + LabelHelper.CurrentTransMakeCurrency.MakeCurrency(comid) + LabelHelper.FormatAmount(Convert.ToDouble(globalvalue)) + " discount on your monthly bill.";
                                            }
                                            else
                                            {
                                                profilemessage = "0";
                                            }
                                        }
                                    }
                                }
                            }
                            if (item.QuestionId == 5)
                            {
                                if (paymentprofilecustomer != null)
                                {
                                    var objprofile = _Util.Facade.CustomerFacade.GetProfileByPaymentInfoId(paymentprofilecustomer.PaymentInfoId);
                                    if (objprofile != null)
                                    {
                                        string[] splitype = objprofile.Type.Split('_');
                                        if (splitype.Length > 0)
                                        {
                                            if (splitype[0] == "ACH")
                                            {
                                                result1 = true;
                                                if (item.Answer == "true")
                                                {
                                                    Ans5 = true;
                                                }
                                                else
                                                {
                                                    Ans5 = false;
                                                }
                                            }
                                            else
                                            {
                                                result1 = true;
                                                message5 = true;
                                                if (item.Answer == "true")
                                                {
                                                    Ans5 = true;
                                                }
                                                else
                                                {
                                                    Ans5 = false;
                                                }
                                                var objglobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(comid, "ACHDiscount");
                                                if (objglobal != null)
                                                {
                                                    var globalvalue = !string.IsNullOrWhiteSpace(objglobal.Value) ? objglobal.Value : "0";
                                                    if (Convert.ToDouble(globalvalue) > 0)
                                                    {
                                                        profilemessage = "Switch to ACH to get an additional " + LabelHelper.CurrentTransMakeCurrency.MakeCurrency(comid) + LabelHelper.FormatAmount(Convert.ToDouble(globalvalue)) + " discount on your monthly bill.";
                                                    }
                                                    else
                                                    {
                                                        profilemessage = "0";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        result1 = true;
                                        message5 = true;
                                        if (item.Answer == "true")
                                        {
                                            Ans5 = true;
                                        }
                                        else
                                        {
                                            Ans5 = false;
                                        }
                                        var objglobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(comid, "ACHDiscount");
                                        if (objglobal != null)
                                        {
                                            var globalvalue = !string.IsNullOrWhiteSpace(objglobal.Value) ? objglobal.Value : "0";
                                            if (Convert.ToDouble(globalvalue) > 0)
                                            {
                                                profilemessage = "Switch to ACH to get an additional " + LabelHelper.CurrentTransMakeCurrency.MakeCurrency(comid) + LabelHelper.FormatAmount(Convert.ToDouble(globalvalue)) + " discount on your monthly bill.";
                                            }
                                            else
                                            {
                                                profilemessage = "0";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    result1 = true;
                                    message5 = true;
                                    if (item.Answer == "true")
                                    {
                                        Ans5 = true;
                                    }
                                    else
                                    {
                                        Ans5 = false;
                                    }
                                    var objglobal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(comid, "ACHDiscount");
                                    if (objglobal != null)
                                    {
                                        var globalvalue = !string.IsNullOrWhiteSpace(objglobal.Value) ? objglobal.Value : "0";
                                        if (Convert.ToDouble(globalvalue) > 0)
                                        {
                                            profilemessage = "Switch to ACH to get an additional " + LabelHelper.CurrentTransMakeCurrency.MakeCurrency(comid) + LabelHelper.FormatAmount(Convert.ToDouble(globalvalue)) + " discount on your monthly bill.";
                                        }
                                        else
                                        {
                                            profilemessage = "0";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Json(new { result = result, result1 = result1, Ques3 = Ques3, Ques4 = Ques4, Ques5 = Ques5, Ans1 = Ans1, Ans3 = Ans3, Ans4 = Ans4, Ans5 = Ans5, Additional5 = Additional5, message = message, Ques1 = Ques1, message5 = message5, profilemessage = profilemessage, customerid = objcus.CustomerId, companyid = comid, type = type });
        }

        public ActionResult PublicACHAddViewPaymentMethod(Guid customerid, Guid companyid)
        {
            System.Web.HttpContext.Current.Session[SessionKeys.CompanyId] = companyid;
            PaymentInfo model = new PaymentInfo();
            ViewBag.ECheckType = _Util.Facade.LookupFacade.GetLookupByKey("ECheckType").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();
            ViewBag.BankAccountType = _Util.Facade.LookupFacade.GetLookupByKey("BankAccountType").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();
            ViewBag.customerid = customerid;
            ViewBag.comid = companyid;
            var echecktypedefault = _Util.Facade.GlobalSettingsFacade.GetEcheckTypeDefault(companyid);
            if (!string.IsNullOrWhiteSpace(echecktypedefault))
            {
                ViewBag.echeckTypedefault = echecktypedefault.ToLower();
            }
            else
            {
                ViewBag.echeckTypedefault = "false";
            }
            return View(model);
        }

        public ActionResult PublicCCAddViewPaymentMethod(Guid customerid, Guid companyid)
        {
            System.Web.HttpContext.Current.Session[SessionKeys.CompanyId] = companyid;
            PaymentInfo model = new PaymentInfo();
            //if (IsFromBooking != null && IsFromBooking == true)
            //{
            //    model.IsFromBooking = IsFromBooking.Value;
            //}
            //else
            //{
            //    model.IsFromBooking = false;
            //}
            //if (customerid != null)
            //{
            //    model = _Util.Facade.PaymentInfoFacade.GetOldPaymentInfoByCustomerIdOnly(customerid);
            //}
            ViewBag.customerid = customerid;
            ViewBag.comid = companyid;

            return View(model);
        }

        [HttpPost]
        public JsonResult SavePaymentMethod(PaymentInfo PaymentInfo, Guid companyid, string type)
        {
            string methodtype = "";
            bool result = false;
            PaymentInfo.CompanyId = companyid;
            Customer customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(PaymentInfo.CustomerId);
            if (customer != null)
            {
                customer.IsACHDiscount = true;
                _Util.Facade.CustomerFacade.UpdateCustomer(customer);
            }
            if (PaymentInfo.MethodType == LabelHelper.PaymentMethod.Invoice)
            {
                List<PaymentProfileCustomer> AllCustomerPaymentProfile = _Util.Facade.CustomerFacade.GetAllPaymentProfileByCustomerId(PaymentInfo.CustomerId, PaymentInfo.CompanyId);
                if (AllCustomerPaymentProfile != null && AllCustomerPaymentProfile.Where(x => x.Type == "Invoice").Count() > 0)
                {
                    var PaymentProfile = AllCustomerPaymentProfile.Where(x => x.Type == "Invoice").FirstOrDefault();
                    return Json(new { result = true, message = "", PaymentProfileId = PaymentProfile.PaymentInfoId });
                }
                PaymentInfo.CardType = LabelHelper.PaymentMethod.Invoice;
                PaymentInfo.BankAccountType = LabelHelper.PaymentMethod.Invoice;
            }
            else if (PaymentInfo.MethodType == LabelHelper.PaymentMethod.Financed)
            {
                List<PaymentProfileCustomer> AllCustomerPaymentProfile = _Util.Facade.CustomerFacade.GetAllPaymentProfileByCustomerId(PaymentInfo.CustomerId, PaymentInfo.CompanyId);
                if (AllCustomerPaymentProfile != null && AllCustomerPaymentProfile.Where(x => x.Type == "Financed").Count() > 0)
                {
                    var PaymentProfile = AllCustomerPaymentProfile.Where(x => x.Type == "Financed").FirstOrDefault();
                    return Json(new { result = true, message = "", PaymentProfileId = PaymentProfile.PaymentInfoId });
                }
                PaymentInfo.CardType = LabelHelper.PaymentMethod.Financed;
                PaymentInfo.BankAccountType = LabelHelper.PaymentMethod.Financed;
            }
            result = _Util.Facade.PaymentInfoFacade.InsertPaymentInfo(PaymentInfo) > 0;
            if (result)
            {
                if (!string.IsNullOrWhiteSpace(PaymentInfo.MethodType) && PaymentInfo.MethodType == "ACH")
                {
                    methodtype = PaymentInfo.MethodType + "_" + PaymentInfo.AccountName + "_" + PaymentInfo.AcountNo.Substring(Math.Max(0, PaymentInfo.AcountNo.Length - 2));
                }
                else if (!string.IsNullOrWhiteSpace(PaymentInfo.MethodType) && PaymentInfo.MethodType == "Credit Card")
                {
                    string cardno = DESEncryptionDecryption.DecryptCipherTextToPlainText(PaymentInfo.CardNumber);
                    methodtype = "CC" + "_" + PaymentInfo.AccountName + "_" + cardno.Substring(Math.Max(0, cardno.Length - 4));
                }
                else if (!string.IsNullOrWhiteSpace(PaymentInfo.MethodType) && PaymentInfo.MethodType == LabelHelper.PaymentMethod.Invoice)
                {
                    methodtype = LabelHelper.PaymentMethod.Invoice;
                }
                else if (!string.IsNullOrWhiteSpace(PaymentInfo.MethodType) && PaymentInfo.MethodType == LabelHelper.PaymentMethod.Financed)
                {
                    methodtype = LabelHelper.PaymentMethod.Financed;
                }
                PaymentProfileCustomer PaymentProfileCustomer = new PaymentProfileCustomer()
                {
                    CompanyId = companyid,
                    CustomerId = PaymentInfo.CustomerId,
                    PaymentInfoId = PaymentInfo.Id,
                    Type = methodtype
                };
                result = _Util.Facade.CustomerFacade.InsertPaymentProfileCustomer(PaymentProfileCustomer) > 0;
                var paymentprofilecustomer = _Util.Facade.PaymentInfoCustomerFacade.GetPaymentInfoCustomerByCustomerIdAndPayFor(PaymentInfo.CustomerId);
                if (paymentprofilecustomer != null)
                {
                    paymentprofilecustomer.CompanyId = companyid;
                    paymentprofilecustomer.CustomerId = PaymentInfo.CustomerId;
                    paymentprofilecustomer.PaymentInfoId = PaymentInfo.Id;
                    paymentprofilecustomer.Type = type;
                    paymentprofilecustomer.Payfor = "MMR";
                    _Util.Facade.PaymentInfoCustomerFacade.UpdatePaymentInfoCustomer(paymentprofilecustomer);
                }
                else
                {
                    PaymentInfoCustomer model = new PaymentInfoCustomer()
                    {
                        CompanyId = companyid,
                        CustomerId = PaymentInfo.CustomerId,
                        PaymentInfoId = PaymentInfo.Id,
                        Type = type,
                        Payfor = "MMR"
                    };
                    _Util.Facade.PaymentInfoCustomerFacade.InsertPaymentInfoCustomer(model);
                }
                var paymentprofilecustomerService = _Util.Facade.PaymentInfoCustomerFacade.GetPaymentInfoCustomerServiceByCustomerId(PaymentInfo.CustomerId);
                if (paymentprofilecustomerService != null)
                {
                    paymentprofilecustomerService.CompanyId = companyid;
                    paymentprofilecustomerService.CustomerId = PaymentInfo.CustomerId;
                    paymentprofilecustomerService.PaymentInfoId = PaymentInfo.Id;
                    paymentprofilecustomerService.Type = type;
                    paymentprofilecustomerService.Payfor = "Service";
                    _Util.Facade.PaymentInfoCustomerFacade.UpdatePaymentInfoCustomer(paymentprofilecustomerService);
                }
                else
                {
                    PaymentInfoCustomer model = new PaymentInfoCustomer()
                    {
                        CompanyId = companyid,
                        CustomerId = PaymentInfo.CustomerId,
                        PaymentInfoId = PaymentInfo.Id,
                        Type = type,
                        Payfor = "Service"
                    };
                    _Util.Facade.PaymentInfoCustomerFacade.InsertPaymentInfoCustomer(model);
                }
            }
            return Json(new { result = result, message = "" });
        }
        #endregion

        #region File Template

        public ActionResult FileTemplateDocument(string code)
        {
            CustomerCompany custommerCompany = new CustomerCompany();
            try
            {
                string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(code).Split('#');
                int cusid = Convert.ToInt32(Decryptval[0]);
                string companyId = Decryptval[2].ToString();
                string emailAddress = Decryptval[1].ToString();
                int fileid = Convert.ToInt32(Decryptval[3]);
                string userId = Decryptval[4].ToString();
                CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByCompanyId(companyId);
                Guid CompanyId = Guid.Empty;
                if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
                {
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    CompanyId = CC.CompanyId;
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
                }
                custommerCompany = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(cusid);
                Customer cust = _Util.Facade.CustomerFacade.GetCustomerByLeadId(cusid);
                if (custommerCompany != null)
                {
                    ViewBag.LeadId = cusid;
                    ViewBag.FileTemplateId = fileid;
                    ViewBag.UserId = userId;
                    ViewBag.emailAddress = emailAddress;
                    ViewBag.AgreementDocumentHeight = _Util.Facade.GlobalSettingsFacade.GetAgreementDocumentHeightByCompanyId(custommerCompany.CompanyId);
                    ViewBag.StringHeight = ViewBag.AgreementDocumentHeight + "px";
                    ViewBag.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(custommerCompany.CompanyId);
                    var objcontract = _Util.Facade.CustomerFacade.GetCustomerById(cusid);

                    //string tempFolder = ConfigurationManager.AppSettings["File.FileManagementCustomerSignature"];
                    //var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CC.CompanyId).CompanyName.ReplaceSpecialChar();
                    //var FtempFolderName = string.Format(tempFolder, comname) + cusid.ToString()+fileid.ToString()+ "Signature";
                    //string FileName = cusid.ToString()+fileid.ToString()+"-___" + "Signature.png";
                    //string filePath = string.Concat("/", FtempFolderName, "/", FileName);
                    //ViewBag.CustomerSignature = filePath;
                    //ViewBag.IsSigned = "";
                    //var fullFilePath = Server.MapPath(filePath);
                    //if(System.IO.File.Exists(fullFilePath))
                    //{
                    //    ViewBag.IsSigned = "IsSigned";
                    //}
                }
                if (cust != null)
                {
                    GlobalSetting glb = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CompanyId, "ACHOrCCInfoFromFileManagement");
                    ViewBag.ACHOrCCAddInfo = glb != null ? glb.Value.ToLower() : "false";
                    System.Web.HttpContext.Current.Session[SessionKeys.CompanyId] = CompanyId;
                    ViewBag.PaymentMethodType = _Util.Facade.LookupFacade.GetLookupByKey("PaymentMethodTypeFromFileManagement").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();
                    ViewBag.CompanyId = CompanyId;
                    ViewBag.CustomerGuidId = cust.CustomerId;
                }
                ViewBag.Code = code;

                #region Customer sign required check
                FileTemplate ft = new FileTemplate();
                CustomerAgreementTemplate cat = new CustomerAgreementTemplate();
                if (fileid > 0)
                {
                    cat = _Util.Facade.FileFacade.GetCustomerAgreementTemplateById(fileid);
                }
                if (cat != null && cat.IsFileTemplate == true && cat.ReferenceTemplateId.HasValue)
                {
                    ft = _Util.Facade.FileFacade.GetFileTemplateById(cat.ReferenceTemplateId.Value);
                }
                if (ft != null && ft.IsCustomerSignRequired == false)
                {
                    ViewBag.CustomerSignRequired = "false";
                }

                #endregion
            }
            catch (Exception ex)
            {
                custommerCompany = null;
            }
            ViewBag.CustomerUiSetting = _Util.Facade.GlobalSettingsFacade.GetAllGlobalSettings().Where(x => x.Tag == "CustomerUiSettings").ToList();
            return View(custommerCompany);

        }

        #endregion

        #region LeadsEstimate

        public ActionResult LeadsEstimate(string code)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    FormsAuthentication.SignOut();
                    SessionHelper hs = new SessionHelper();
                    hs.ClearCurrentSession();
                }
                ViewBag.Token = code;
                string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(code).Split('#');
                Guid CustomerId = new Guid(Decryptval[0]);
                Guid CompanyId = new Guid(Decryptval[1]);
                int InvoiceId = Convert.ToInt32(Decryptval[2]);

                #region ConnectionStringSetup
                CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByCompanyId(CompanyId.ToString());

                if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                else
                {
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
                    Company Com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
                    ViewBag.Title = string.Concat(Com.CompanyName, " | ", Localize.T("Estimate Sign"));
                }
                #endregion

                CompanyBranch cb = _Util.Facade.CompanyBranchFacade.GetMainBranchByCompanyId(CompanyId);
                ViewBag.CompanyLogo = cb.Logo;

                Invoice inv = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);
                if (CompanyId != inv.CompanyId
                    || CustomerId != inv.CustomerId
                    || inv.Status == LabelHelper.EstimateStatus.Declined
                    //|| (inv.DueDate.HasValue && inv.DueDate.Value.AddDays(1) < DateTime.Now.UTCCurrentTime())
                    )
                {
                    if (inv.Status == LabelHelper.EstimateStatus.Declined)
                    {
                        ViewBag.EstimateId = InvoiceId.GenerateEstimateNo();
                        ViewBag.IsDecline = "true";
                    }
                    return PartialView("~/Views/Shared/_LinkExpired.cshtml");
                }
                if (!inv.IsEstimate)
                {
                    ViewBag.IsEstimate = false;
                    ViewBag.EstimateId = InvoiceId.GenerateEstimateNo();
                    return PartialView("~/Views/Shared/_LinkExpired.cshtml");
                }
                Customer tmpCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
                inv.CustomerBussinessName = tmpCustomer.BusinessName;
                inv.CustomerName = tmpCustomer.FirstName + " " + tmpCustomer.LastName;
                if (!string.IsNullOrWhiteSpace(inv.EstimateTerm) && inv.EstimateTerm != "-1")
                {
                    ViewBag.EstimateTerm = _Util.Facade.LookupFacade.GetDisplayTextByDataValueFromLLookup(inv.EstimateTerm);
                }
                //if(inv.EstimateTerm == "50UponAcceptance50UponCompletion")
                //{
                //    if(inv.TotalAmount != 0.0)
                //    {
                //        inv.TotalAmount = ((inv.TotalAmount * 50) / 100);
                //    }
                //}
                #region Insert Customer Agreement
                //Model.Invoice.CustomerAgreement
                CustomerAgreement CustomerAgreement = new CustomerAgreement()
                {
                    CompanyId = CompanyId,
                    CustomerId = CustomerId,
                    IP = AppConfig.GetIP,
                    UserAgent = AppConfig.GetUserAgent,
                    Type = LabelHelper.CustomerAgreementLog.LoadEstimate,
                    InvoiceId = InvoiceId.GenerateEstimateNo(),
                    AddedDate = DateTime.Now.UTCCurrentTime(),
                };
                _Util.Facade.CustomerAgreementFacade.InsertCustomerAgreement(CustomerAgreement);
                #endregion

                Guid SoldBy = new Guid();
                if (Guid.TryParse(tmpCustomer.Soldby, out SoldBy) && SoldBy != new Guid())
                {
                    CustomerCompany cusCom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(tmpCustomer.Id);
                    Notification notification = new Notification();
                    #region Insert notification
                    if (cusCom.IsLead == false)
                    {
                        notification = new Notification()
                        {
                            CompanyId = CompanyId,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            NotificationId = Guid.NewGuid(),
                            Type = LabelHelper.NotificationType.Customer,
                            Who = tmpCustomer.CustomerId,
                            What = string.Format(@"A customer <a class=""cus-anchor"" href=""{4}/Customer/Customerdetail/?id={1}"">{0}</a> just viewed an estimate  
                                            <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenEstById('{2}')"">{3}</a>", "{0}", tmpCustomer.Id, inv.Id, inv.InvoiceId, AppConfig.DomainSitePath),
                            NotificationUrl = AppConfig.DomainSitePath + "/Customer/Customerdetail/?id=" + tmpCustomer.Id

                        };
                    }
                    else
                    {
                        notification = new Notification()
                        {
                            CompanyId = CompanyId,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            NotificationId = Guid.NewGuid(),
                            Type = LabelHelper.NotificationType.Customer,
                            Who = tmpCustomer.CustomerId,
                            What = string.Format(@"A Lead <a class=""cus-anchor"" href=""{4}/Customer/Customerdetail/?id={1}"">{0}</a> just viewed an estimate  
                                            <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenEstById('{2}')"">{3}</a>", "{0}", tmpCustomer.Id, inv.Id, inv.InvoiceId, AppConfig.DomainSitePath),
                            NotificationUrl = AppConfig.DomainSitePath + "/Customer/Customerdetail/?id=" + tmpCustomer.Id
                        };
                    }

                    _Util.Facade.NotificationFacade.InsertNotification(notification);

                    #endregion
                    #region set user to notification
                    if (SoldBy != inv.CreatedByUid)
                    {
                        NotificationUser nu = new NotificationUser()
                        {
                            NotificationId = notification.NotificationId,
                            IsRead = false,
                            NotificationPerson = SoldBy,
                        };

                        _Util.Facade.NotificationFacade.InsertNotificationUser(nu);

                        NotificationUser nus = new NotificationUser()
                        {
                            NotificationId = notification.NotificationId,
                            IsRead = false,
                            NotificationPerson = inv.CreatedByUid,
                        };


                        _Util.Facade.NotificationFacade.InsertNotificationUser(nus);
                    }
                    else
                    {
                        NotificationUser nu = new NotificationUser()
                        {
                            NotificationId = notification.NotificationId,
                            IsRead = false,
                            NotificationPerson = SoldBy,
                        };
                        _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
                    }

                    #endregion

                }

                ViewBag.CompanyId = CompanyId.ToString();
                ViewBag.FooterCompanyInformation = _Util.Facade.MailFacade.GetFooterCompanyInformationByCompanyId(CompanyId).Replace("##Year##", string.Format("2017-{0}", DateTime.Now.Year.ToString()));
                return View(inv);

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return PartialView("~/Views/Shared/_Loading.cshtml");
            }
        }

        public ActionResult LeadsEstimatePdf(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            try
            {
                string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(code).Split('#');
                Guid CusotmerId = new Guid(Decryptval[0]);
                Guid CompanyId = new Guid(Decryptval[1]);
                int InvoiceId = Convert.ToInt32(Decryptval[2]);

                if (Session[SessionKeys.CompanyConnectionString] == null || string.IsNullOrWhiteSpace(Session[SessionKeys.CompanyConnectionString].ToString()))
                {
                    CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByCompanyId(CompanyId.ToString());

                    if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
                    {
                        return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                    }
                    else
                    {
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
                    }
                }


                Company Com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
                ViewBag.ComName = Com.CompanyName;
                CreateInvoice Model;
                Invoice Inv = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);
                if (!Inv.IsEstimate || CompanyId != Inv.CompanyId)
                {
                    return PartialView("~/Views/Shared/_LinkExpired.cshtml");
                }
                Inv.Status = LabelHelper.EstimateStatus.CustomerViewed;
                _Util.Facade.InvoiceFacade.UpdateInvoice(Inv);

                List<InvoiceDetail> InvDetailList = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(Inv.InvoiceId);
                Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Inv.CompanyId);
                Customer tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CusotmerId);
                Model = GetCreateInvoiceModel(Inv, InvDetailList, tempCom, tempCustomer);
                Model.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CompanyId);
                ViewBag.CompanyId = tempCom.CompanyId.ToString();

                Model.InvoiceSetting = new InvoiceSetting();
                string settingskey = @" 'InvoiceSettingsShipping', 'EstimateServiceSetting', 'InvoiceSettingsDiscount', 'InvoiceSettingsDeposit'";
                List<GlobalSetting> printsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceSettingListByCompanyIdAndKey(settingskey, tempCom.CompanyId);
                foreach (var print in printsetting)
                {
                    if (print.Value.ToLower() == "true")
                    {
                        if (print.SearchKey == "InvoiceSettingsDeposit")
                        {
                            Model.InvoiceSetting.DepositSetting = true;
                        }
                        if (print.SearchKey == "EstimateServiceSetting")
                        {
                            Model.InvoiceSetting.ServiceSetting = true;
                        }
                        if (print.SearchKey == "InvoiceSettingsDiscount")
                        {
                            Model.InvoiceSetting.DiscountSetting = true;
                        }
                        if (print.SearchKey == "InvoiceSettingsShipping")
                        {
                            Model.InvoiceSetting.ShippingSetting = true;
                        }
                    }
                }
                GlobalSetting monitoringVal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateMonitoringAmount");
                GlobalSetting contractTerm = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateContractTerm");
                GlobalSetting onitWater = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowONITWaterLLCForEstimate");
                GlobalSetting monitoringDes = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateMonitoringDescription");
                GlobalSetting thompsonEstimate = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowThompsonEstimateText");
                GlobalSetting gutterEqpImg = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowGutterEstimateEquipmentImage");
                if (gutterEqpImg != null)
                {
                    Model.ShowGutterEquipmentImage = gutterEqpImg.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowGutterEquipmentImage = false;
                }
                if (thompsonEstimate != null)
                {
                    Model.ShowThompsonEstimateText = thompsonEstimate.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowThompsonEstimateText = false;
                }
                if (monitoringDes != null)
                {
                    Model.ShowEstimateMonitoringDescription = monitoringDes.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowEstimateMonitoringDescription = false;
                }
                if (monitoringVal != null)
                {
                    Model.ShowEstimateMonitoringAmount = monitoringVal.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowEstimateMonitoringAmount = false;
                }
                if (contractTerm != null)
                {
                    Model.ShowEstimateContractTerm = contractTerm.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowEstimateContractTerm = false;
                }
                if (onitWater != null)
                {
                    Model.ShowOnitWaterTreatment = onitWater.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowOnitWaterTreatment = false;
                }
                GlobalSetting invCode3Sta = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowCode3InvoiceStaticBox");
                if (invCode3Sta != null)
                {
                    Model.ShowCode3InvoiceStaticBox = invCode3Sta.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowCode3InvoiceStaticBox = false;
                }
                GlobalSetting invComAddress = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowInvoiceCompanyAddress");
                if (invComAddress != null)
                {
                    Model.ShowInvoiceCompanyAddress = invComAddress.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowInvoiceCompanyAddress = false;
                }
                return new Rotativa.ViewAsPdf("~/Views/Estimate/EstimatePdf.cshtml", Model)
                {
                    //FileName = "TestView.pdf",
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
            }
            catch (Exception)
            {
                return PartialView("~/Views/Shared/_Loading.cshtml");
            }
        }
        public ActionResult LeadsEstimateHtml(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            try
            {
                string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(code).Split('#');
                Guid CusotmerId = new Guid(Decryptval[0]);
                Guid CompanyId = new Guid(Decryptval[1]);
                int InvoiceId = Convert.ToInt32(Decryptval[2]);

                if (Session[SessionKeys.CompanyConnectionString] == null || string.IsNullOrWhiteSpace(Session[SessionKeys.CompanyConnectionString].ToString()))
                {
                    CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByCompanyId(CompanyId.ToString());

                    if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
                    {
                        return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                    }
                    else
                    {
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
                    }
                }


                Company Com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
                ViewBag.ComName = Com.CompanyName;
                CreateInvoice Model;
                Invoice Inv = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);
                if (!Inv.IsEstimate || CompanyId != Inv.CompanyId)
                {
                    return PartialView("~/Views/Shared/_LinkExpired.cshtml");
                }
                Inv.Status = LabelHelper.EstimateStatus.CustomerViewed;
                _Util.Facade.InvoiceFacade.UpdateInvoice(Inv);

                List<InvoiceDetail> InvDetailList = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(Inv.InvoiceId);
                Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Inv.CompanyId);
                Customer tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CusotmerId);
                Model = GetCreateInvoiceModel(Inv, InvDetailList, tempCom, tempCustomer);
                Model.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CompanyId);
                ViewBag.CompanyId = tempCom.CompanyId.ToString();

                Model.InvoiceSetting = new InvoiceSetting();
                string settingskey = @" 'InvoiceSettingsShipping', 'EstimateServiceSetting', 'InvoiceSettingsDiscount', 'InvoiceSettingsDeposit'";
                List<GlobalSetting> printsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceSettingListByCompanyIdAndKey(settingskey, tempCom.CompanyId);
                foreach (var print in printsetting)
                {
                    if (print.Value.ToLower() == "true")
                    {
                        if (print.SearchKey == "InvoiceSettingsDeposit")
                        {
                            Model.InvoiceSetting.DepositSetting = true;
                        }
                        if (print.SearchKey == "EstimateServiceSetting")
                        {
                            Model.InvoiceSetting.ServiceSetting = true;
                        }
                        if (print.SearchKey == "InvoiceSettingsDiscount")
                        {
                            Model.InvoiceSetting.DiscountSetting = true;
                        }
                        if (print.SearchKey == "InvoiceSettingsShipping")
                        {
                            Model.InvoiceSetting.ShippingSetting = true;
                        }
                    }
                }
                return View("~/Views/Estimate/EstimatePdf.cshtml", Model);
            }
            catch (Exception)
            {
                return PartialView("~/Views/Shared/_Loading.cshtml");
            }
        }
        public JsonResult LeadsEstimateAgree(string code, bool status, string DeclinedReason)
        {
            Invoice createTempInv = new Invoice();
            InvoiceDetail createTempInvDetl = new InvoiceDetail();
            bool result = false;
            if (string.IsNullOrWhiteSpace(code))
            {
                return Json(new { result = false, message = "Access denied." });
            }
            string emailtemplate = "";
            string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(code).Split('#');
            Guid CustomerId = new Guid(Decryptval[0]);
            Guid CompanyId = new Guid(Decryptval[1]);
            int InvoiceId = Convert.ToInt32(Decryptval[2]);
            CreateInvoice Model = new CreateInvoice();
            #region Check Default Billing Tax
            bool defaultBillTaxVal = true;
            GlobalSetting defaultBillTax = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("DefaultCustomerBillingTax");
            if (defaultBillTax != null)
            {
                if (defaultBillTax.Value.ToLower() == "true")
                {
                    defaultBillTaxVal = true;
                }
                else
                {
                    defaultBillTaxVal = false;
                }
            }
            #endregion
            if (status == true)
            {
                List<InvoiceDetail> detlist = new List<InvoiceDetail>();
                Invoice inv = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);
                if (inv != null)
                {
                    detlist = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(inv.InvoiceId);
                }
                if (inv.IsEstimate == false)
                {
                    return Json(new { result = true, IsReload = true, message = "You have already submitted" });
                }
                Customer tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
                Company tempComp = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
                //List<Ticket> _ticList = _Util.Facade.TicketFacade.GetAllInstallationTicketByCustomerId(CustomerId);
                #region Make lead customer

                if (inv != null && inv.Status != LabelHelper.EstimateStatus.Signed)
                {

                    #region Insert Ticket 
                    Guid TicketId = Guid.Empty;
                    Guid SoldBy = new Guid();
                    Guid.TryParse(tempCustomer.Soldby, out SoldBy);
                    CustomerAppointmentEquipment caEquipment = new CustomerAppointmentEquipment();
                    Ticket newTicket = new Ticket();

                    newTicket = new Ticket()
                    {
                        TicketId = Guid.NewGuid(),
                        CompanyId = CompanyId,
                        CustomerId = CustomerId,
                        TicketType = LabelHelper.TicketType.Installtion,
                        CreatedBy = new Guid(LabelHelper.SystemUser.ID),
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        CompletionDate = DateTime.Now.UTCCurrentTime(),
                        Status = LabelHelper.TicketStatus.Created,
                        LastUpdatedBy = new Guid(LabelHelper.SystemUser.ID),
                        LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                        HasInvoice = false,
                        HasSurvey = false,
                        IsAgreementTicket = true
                    };
                    newTicket.Id = _Util.Facade.TicketFacade.InsertTicket(newTicket);
                    logger.WithProperty("tags", "ticket,insert").WithProperty("params", JsonConvert.SerializeObject(newTicket)).Trace("Ticket Id {Id}", newTicket.Id);
                    TicketId = newTicket.TicketId;

                    CustomerAppointment ca = new CustomerAppointment()
                    {
                        AppointmentId = newTicket.TicketId,
                        CompanyId = CompanyId,
                        CustomerId = CustomerId,
                        EmployeeId = new Guid(LabelHelper.SystemUser.ID),
                        AppointmentType = newTicket.TicketType,
                        AppointmentDate = DateTime.Now.UTCCurrentTime(),
                        CreatedBy = User.Identity.Name,
                        LastUpdatedBy = User.Identity.Name,
                        LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                        IsAllDay = true,
                    };
                    ca.Id = (int)_Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppoinment(ca);
                    TicketUser TU = new TicketUser()
                    {
                        IsPrimary = true,
                        NotificationOnly = false,
                        AddedBy = SoldBy,
                        AddedDate = DateTime.Now.UTCCurrentTime(),
                        TiketId = newTicket.TicketId,
                        UserId = new Guid(LabelHelper.SystemUser.ID),
                    };
                    TU.Id = _Util.Facade.TicketFacade.InsertTicketUser(TU);

                    Equipment EquipmentDetails = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentName("Monthly Monitoring Rate");
                    caEquipment = new CustomerAppointmentEquipment()
                    {
                        AppointmentId = newTicket.TicketId,
                        EquipmentId = EquipmentDetails != null && EquipmentDetails.EquipmentId != null ? EquipmentDetails.EquipmentId : Guid.Empty,
                        Quantity = 1,
                        UnitPrice = inv.MonitoringAmount.HasValue ? inv.MonitoringAmount.Value : 0.0,
                        TotalPrice = inv.MonitoringAmount.HasValue ? inv.MonitoringAmount.Value : 0.0,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        CreatedBy = User.Identity.Name,
                        EquipName = EquipmentDetails != null && !string.IsNullOrWhiteSpace(EquipmentDetails.Name) ? EquipmentDetails.Name : "",
                        EquipDetail = EquipmentDetails != null && !string.IsNullOrWhiteSpace(EquipmentDetails.Description) ? EquipmentDetails.Description : "",
                        IsEquipmentRelease = false,
                        IsService = true,
                        CreatedByUid = SoldBy,
                        IsAgreementItem = true,
                        IsBaseItem = true,
                        OriginalUnitPrice = inv.MonitoringAmount.HasValue ? inv.MonitoringAmount.Value : 0.0,
                        IsInvoiceCreate = true,
                        IsNonCommissionable = false
                    };
                    _Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppointmentEquipmentDetail(caEquipment);

                    #region Estimate Added in Ticket
                    //List<InvoiceDetail> detlist = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(inv.InvoiceId);
                    if (detlist != null && detlist.Count > 0)
                    {
                        foreach (var item in detlist)
                        {
                            caEquipment = new CustomerAppointmentEquipment()
                            {
                                AppointmentId = TicketId,
                                EquipmentId = item.EquipmentId,
                                CreatedBy = User.Identity.Name,
                                EquipDetail = item.EquipDetail,
                                EquipName = item.EquipName,
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                Quantity = item.Quantity.HasValue ? item.Quantity.Value : 0,
                                UnitPrice = item.UnitPrice.HasValue ? item.UnitPrice.Value : 0,
                                TotalPrice = item.TotalPrice.HasValue ? item.TotalPrice.Value : 0,
                                IsAgreementItem = true,
                                CreatedByUid = SoldBy
                            };
                            _Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppointmentEquipmentDetail(caEquipment);
                        }
                    }

                    #endregion
                    //var WarrentyEquipment = _Util.Facade.EquipmentFacade.GetWarrentyEquipmentListByEquipmentId(eqp.EquipmentId);
                    //if (WarrentyEquipment != null && WarrentyEquipment.Count > 0)
                    //{
                    //    var warrentypackagecus = _Util.Facade.PackageFacade.GetWarrentyPackageCustomerByCustomerIdAndPackageId(eqp.PackageId, eqp.CustomerId);
                    //    if (warrentypackagecus != null)
                    //    {
                    //        warrentypackagecus.WarrentyAvailable = true;
                    //        _Util.Facade.PackageFacade.UpdatePackageCustomer(warrentypackagecus);
                    //    }
                    //}

                    #endregion

                    double ServiceCost = inv.MonitoringAmount.HasValue ? inv.MonitoringAmount.Value : 0.0;
                    double ServiceCostTax = 0;
                    GlobalSetting GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(CompanyId, CustomerId);
                    if (GetSalesTax != null)
                    {
                        ServiceCostTax = Math.Round((ServiceCost * Convert.ToDouble(GetSalesTax.Value)) / 100, 2);
                    }
                    tempCustomer.MonthlyMonitoringFee = ServiceCost.ToString();
                    tempCustomer.BillAmount = defaultBillTaxVal ? (ServiceCost + ServiceCostTax) : ServiceCost;
                    tempCustomer.TotalTax = defaultBillTaxVal ? ServiceCostTax : 0;
                    tempCustomer.BillTax = defaultBillTaxVal;
                }

                CustomerCompany cc = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerIdAndCompanyId(CustomerId, CompanyId);
                cc.IsLead = false;
                _Util.Facade.CustomerFacade.UpdateCustomerCompany(cc);
                tempCustomer.JoinDate = DateTime.Now.UTCCurrentTime();
                tempCustomer.IsActive = true;
                _Util.Facade.CustomerFacade.UpdateCustomer(tempCustomer);
                #endregion
                //List<InvoiceDetail> invDetList = _Util.Facade.InvoiceFacade.GetInvoiceDetialsListByInvoiceId(inv.InvoiceId);



                //if Estimate terms is due upon completion and already signed then don't create invoice
                if (inv.EstimateTerm != LabelHelper.EstimatePaymentTerms.DueUponCompletion && inv.Status != LabelHelper.EstimateStatus.Signed)
                {
                    #region Make invoice  
                    inv.Status = LabelHelper.EstimateStatus.Signed;
                    inv.LastUpdatedDate = DateTime.UtcNow;
                    _Util.Facade.InvoiceFacade.UpdateInvoice(inv);
                    Invoice tempInv = new Invoice()
                    {
                        CustomerId = inv.CustomerId,
                        CompanyId = inv.CompanyId,
                        Amount = inv.Amount,
                        Tax = inv.Tax,
                        DiscountCode = inv.DiscountCode,
                        DiscountAmount = inv.DiscountAmount,
                        TotalAmount = inv.TotalAmount,
                        Status = LabelHelper.InvoiceStatus.Open,
                        InvoiceDate = inv.InvoiceDate,
                        IsEstimate = false,
                        IsBill = inv.IsBill,
                        BillingAddress = inv.BillingAddress,
                        DueDate = inv.DueDate,
                        Terms = inv.Terms,
                        ShippingAddress = inv.ShippingAddress,
                        ShippingVia = inv.ShippingVia,
                        ShippingDate = inv.ShippingDate,
                        TrackingNo = inv.TrackingNo,
                        ShippingCost = inv.ShippingCost,
                        Discountpercent = inv.Discountpercent,
                        BalanceDue = inv.BalanceDue,
                        Deposit = inv.Deposit,
                        Message = inv.Message,
                        TaxType = inv.TaxType,
                        Balance = inv.Balance,
                        Memo = inv.Memo,
                        InvoiceFor = LabelHelper.InvoiceFor.Invoice,
                        LateFee = inv.LateFee,
                        LateAmount = inv.LateAmount,
                        InstallDate = inv.InstallDate,
                        Description = inv.Description,
                        DiscountType = inv.DiscountType,
                        BillingCycle = inv.BillingCycle,
                        EstimateTerm = inv.EstimateTerm,
                        Signature = inv.Signature,
                        CancelReason = inv.CancelReason,
                        CreatedDate = inv.CreatedDate,
                        CreatedBy = inv.CreatedBy,
                        CreatedByUid = inv.CreatedByUid,
                        LastUpdatedDate = inv.LastUpdatedDate,
                        LastUpdatedByUid = inv.LastUpdatedByUid,
                        RefType = inv.RefType,
                        PaymentType = inv.PaymentType,
                        BookingId = inv.BookingId,
                        InstallationType = inv.InstallationType,
                        SignatureDate = inv.SignatureDate,
                        InvoiceEmailAddress = inv.InvoiceEmailAddress,
                        InvoiceCcEmailAddress = inv.InvoiceCcEmailAddress,
                        MonitoringAmount = inv.MonitoringAmount,
                        ContractTerm = inv.ContractTerm,
                        MonitoringDescription = inv.MonitoringDescription,
                        IsARBInvoice = inv.IsARBInvoice,
                        TransactionId = inv.TransactionId,
                        ForteStatus = inv.ForteStatus,
                        UpfrontMonth = inv.UpfrontMonth
                    };

                    //tempInv.IsEstimate = false;
                    //tempInv.InvoiceFor = LabelHelper.InvoiceFor.Invoice;
                    //tempInv.Status = LabelHelper.InvoiceStatus.Open;


                    tempInv.Id = _Util.Facade.InvoiceFacade.InsertInvoice(tempInv);
                    tempInv.InvoiceId = tempInv.Id.GenerateInvoiceNo();
                    _Util.Facade.InvoiceFacade.UpdateInvoice(tempInv);
                    createTempInv = tempInv;
                    if (detlist != null && detlist.Count > 0)
                    {
                        foreach (var item in detlist)
                        {
                            item.InvoiceId = tempInv.InvoiceId;
                            item.CreatedDate = DateTime.UtcNow;
                            _Util.Facade.InvoiceFacade.InsertInvoiceDetails(item);
                        }
                    }
                    //createTempInvDetls = detlist;
                    #region Added Monthly Fee as Invoice line item
                    Equipment _EquipmentDetails = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentName("Monthly Monitoring Rate");
                    InvoiceDetail _invDetls = new InvoiceDetail()
                    {
                        InvoiceId = tempInv.InvoiceId,
                        InventoryId = Guid.Empty,
                        EquipmentId = _EquipmentDetails != null ? _EquipmentDetails.EquipmentId : Guid.Empty,
                        EquipName = "Product and Service",
                        EquipDetail = "Product and Service",
                        CompanyId = tempInv.CompanyId,
                        Quantity = tempInv != null && !string.IsNullOrWhiteSpace(tempInv.UpfrontMonth) && tempInv.UpfrontMonth != "-1" ? Convert.ToInt32(tempInv.UpfrontMonth) : 0,
                        UnitPrice = tempInv.MonitoringAmount,
                        TotalPrice = tempInv.MonitoringAmount * (tempInv != null && !string.IsNullOrWhiteSpace(tempInv.UpfrontMonth) && tempInv.UpfrontMonth != "-1" ? Convert.ToInt32(tempInv.UpfrontMonth) : 0),
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = tempInv.CreatedBy,
                        Taxable = defaultBillTaxVal
                    };
                    _Util.Facade.InvoiceFacade.InsertInvoiceDetails(_invDetls);
                    createTempInvDetl = _invDetls;
                    #endregion

                    #endregion
                }
                Model = GetCreateInvoiceModel(inv, detlist, tempComp, tempCustomer);
                Model.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CompanyId);
                Model.Invoice.InvoiceId = Model.Invoice.Id.GenerateEstimateNo();
                bool AfterSubmitDocumentSave = false;
                GlobalSetting GlobSet = _Util.Facade.GlobalSettingsFacade.GetGlobalsettingBySearchKeyAndCompanyId("AfterSubmitDocumentSave", CompanyId);
                if (GlobSet != null && !string.IsNullOrWhiteSpace(GlobSet.Value))
                {
                    if (GlobSet.Value == "true")
                    {
                        AfterSubmitDocumentSave = true;
                    }
                }
                if (AfterSubmitDocumentSave == true)
                {
                    #region Make the Estimate_Signed.pdf file
                    Model = GetCreateInvoiceModel(inv, detlist, tempComp, tempCustomer);
                    Model.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CompanyId);
                    Model.Invoice.InvoiceId = Model.Invoice.Id.GenerateEstimateNo();

                    #region Insert Customer Agreement
                    //Model.Invoice.CustomerAgreement
                    CustomerAgreement CustomerAgreement = new CustomerAgreement()
                    {
                        CompanyId = CompanyId,
                        CustomerId = CustomerId,
                        IP = AppConfig.GetIP,
                        UserAgent = AppConfig.GetUserAgent,
                        Type = LabelHelper.CustomerAgreementLog.SubmitEstimate,
                        InvoiceId = InvoiceId.GenerateEstimateNo(),
                        AddedDate = DateTime.Now.UTCCurrentTime(),
                    };
                    _Util.Facade.CustomerAgreementFacade.InsertCustomerAgreement(CustomerAgreement);
                    #endregion

                    Model.Invoice.CustomerAgreement = _Util.Facade.CustomerAgreementFacade.GetCustomerAgreementByCompanyIdAndCustomerIdAndInvoiceId(CompanyId, CustomerId, InvoiceId.GenerateEstimateNo());
                    //ViewBag.CompanyId = tempComp.CompanyId.ToString();
                    Model.InvoiceSetting = new InvoiceSetting();
                    string settingskey = @" 'InvoiceSettingsShipping', 'EstimateServiceSetting', 'InvoiceSettingsDiscount', 'InvoiceSettingsDeposit'";
                    List<GlobalSetting> printsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceSettingListByCompanyIdAndKey(settingskey, CompanyId);
                    foreach (var print in printsetting)
                    {
                        if (print.Value.ToLower() == "true")
                        {
                            if (print.SearchKey == "InvoiceSettingsDeposit")
                            {
                                Model.InvoiceSetting.DepositSetting = true;
                            }
                            if (print.SearchKey == "EstimateServiceSetting")
                            {
                                Model.InvoiceSetting.ServiceSetting = true;
                            }
                            if (print.SearchKey == "InvoiceSettingsDiscount")
                            {
                                Model.InvoiceSetting.DiscountSetting = true;
                            }
                            if (print.SearchKey == "InvoiceSettingsShipping")
                            {
                                Model.InvoiceSetting.ShippingSetting = true;
                            }
                        }
                    }
                    GlobalSetting monitoringVal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateMonitoringAmount");
                    GlobalSetting contractTerm = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateContractTerm");
                    GlobalSetting onitWater = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowONITWaterLLCForEstimate");
                    GlobalSetting monitoringDes = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateMonitoringDescription");
                    GlobalSetting thompsonEstimate = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowThompsonEstimateText");
                    GlobalSetting gutterEqpImg = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowGutterEstimateEquipmentImage");
                    if (gutterEqpImg != null)
                    {
                        Model.ShowGutterEquipmentImage = gutterEqpImg.Value.ToLower() == "true" ? true : false;
                    }
                    else
                    {
                        Model.ShowGutterEquipmentImage = false;
                    }
                    if (thompsonEstimate != null)
                    {
                        Model.ShowThompsonEstimateText = thompsonEstimate.Value.ToLower() == "true" ? true : false;
                    }
                    else
                    {
                        Model.ShowThompsonEstimateText = false;
                    }
                    if (monitoringDes != null)
                    {
                        Model.ShowEstimateMonitoringDescription = monitoringDes.Value.ToLower() == "true" ? true : false;
                    }
                    else
                    {
                        Model.ShowEstimateMonitoringDescription = false;
                    }
                    if (monitoringVal != null)
                    {
                        Model.ShowEstimateMonitoringAmount = monitoringVal.Value.ToLower() == "true" ? true : false;
                    }
                    else
                    {
                        Model.ShowEstimateMonitoringAmount = false;
                    }
                    if (contractTerm != null)
                    {
                        Model.ShowEstimateContractTerm = contractTerm.Value.ToLower() == "true" ? true : false;
                    }
                    else
                    {
                        Model.ShowEstimateContractTerm = false;
                    }
                    if (onitWater != null)
                    {
                        Model.ShowOnitWaterTreatment = onitWater.Value.ToLower() == "true" ? true : false;
                    }
                    else
                    {
                        Model.ShowOnitWaterTreatment = false;
                    }
                    GlobalSetting invCode3Sta = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowCode3InvoiceStaticBox");
                    if (invCode3Sta != null)
                    {
                        Model.ShowCode3InvoiceStaticBox = invCode3Sta.Value.ToLower() == "true" ? true : false;
                    }
                    else
                    {
                        Model.ShowCode3InvoiceStaticBox = false;
                    }
                    GlobalSetting invComAddress = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowInvoiceCompanyAddress");
                    if (invComAddress != null)
                    {
                        Model.ShowInvoiceCompanyAddress = invComAddress.Value.ToLower() == "true" ? true : false;
                    }
                    else
                    {
                        Model.ShowInvoiceCompanyAddress = false;
                    }
                    ViewAsPdf EstimateActionPdf = new Rotativa.ViewAsPdf("~/Views/Estimate/EstimatePdf.cshtml", Model)
                    {
                        //FileName = "TestView.pdf",
                        PageSize = Rotativa.Options.Size.A4,
                        PageOrientation = Rotativa.Options.Orientation.Portrait,
                        PageMargins = { Left = 1, Right = 1 },

                    };
                    byte[] EstimatePdfData = EstimateActionPdf.BuildPdf(ControllerContext);

                    #region Save Estimate.pdf to file System  
                    string estimateno = Model.Invoice.Id.GenerateEstimateNo();
                    string filename = ConfigurationManager.AppSettings["File.EstimateFiles"];
                    var comname = tempComp.CompanyName.ReplaceSpecialChar();
                    filename = string.Format(filename, comname);
                    filename += DateTime.Now.UTCCurrentTime().Year.ToString()
                        + "/" + DateTime.Now.UTCCurrentTime().Month.ToString()
                        + "/" + estimateno + "_Signed.pdf";
                    string Serverfilename = FileHelper.GetFileFullPath(filename);
                    FileHelper.SaveFile(EstimatePdfData, Serverfilename);
                    #endregion

                    #region Save CustomerFile
                    _Util.Facade.CustomerFileFacade.SaveEstimatePdfFile(AppConfig.DomainSitePath + filename, estimateno, CustomerId, CompanyId, true);
                    #endregion

                    #endregion
                }
                GlobalSetting InvoiceEmail = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "SignedInvoiceEmail");
                if (inv.EstimateTerm != LabelHelper.EstimatePaymentTerms.DueUponCompletion)
                {

                    #region Send Invoice Created Email to Customer
                    if (tempCustomer.PreferedEmail.HasValue && tempCustomer.PreferedEmail.Value && tempCustomer.EmailAddress.IsValidEmailAddress())
                    {
                        string SalesGuy = "";
                        string SalesGuyEmail = "";
                        Guid EmployeeId;
                        string SalesPhone = "";

                        #region Reply Email Name And Address
                        if (!string.IsNullOrWhiteSpace(tempCustomer.Soldby) && Guid.TryParse(tempCustomer.Soldby, out EmployeeId) && EmployeeId != new Guid())
                        {
                            Notification notification = new Notification();
                            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(EmployeeId);
                            if (emp != null)
                            {
                                SalesGuy = string.Concat(emp.FirstName, " ", emp.LastName);
                                SalesGuyEmail = string.IsNullOrWhiteSpace(emp.Email) ? "info@rmrcloud.com" : emp.Email;
                                SalesPhone = emp.Phone;
                            }
                            CustomerCompany cusCom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(tempCustomer.Id);
                            if (cusCom.IsLead == false)
                            {
                                notification = new Notification()
                                {
                                    CompanyId = CompanyId,
                                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                                    NotificationId = Guid.NewGuid(),
                                    Type = LabelHelper.NotificationType.Customer,
                                    Who = tempCustomer.CustomerId,
                                    What = string.Format(@"A customer <a class=""cus-anchor"" href=""{4}/Customer/Customerdetail/?id={1}"">{0}</a> just signed on an estimate and  
                                            <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenInvById('{2}')"">{3}</a> generated", "{0}"
                                          , tempCustomer.Id
                                          , Model.Invoice.Id
                                          , Model.Invoice.Id.GenerateInvoiceNo(), AppConfig.DomainSitePath),
                                    NotificationUrl = AppConfig.DomainSitePath + "/Customer/Customerdetail/?id=" + tempCustomer.Id + "#InvoiceTab"

                                };
                            }
                            else
                            {
                                notification = new Notification()
                                {
                                    CompanyId = CompanyId,
                                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                                    NotificationId = Guid.NewGuid(),
                                    Type = LabelHelper.NotificationType.Customer,
                                    Who = tempCustomer.CustomerId,
                                    What = string.Format(@"A Lead <a class=""cus-anchor"" href=""{4}/Customer/Customerdetail/?id={1}"">{0}</a> just signed on an estimate and  
                                            <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenInvById('{2}')"">{3}</a> generated", "{0}"
                                     , tempCustomer.Id
                                     , Model.Invoice.Id
                                     , Model.Invoice.Id.GenerateInvoiceNo(), AppConfig.DomainSitePath),
                                    NotificationUrl = AppConfig.DomainSitePath + "/Customer/Customerdetail/?id=" + tempCustomer.Id + "#InvoiceTab"
                                };
                            }
                            #region Insert notification

                            _Util.Facade.NotificationFacade.InsertNotification(notification);

                            #endregion
                            #region set user to notification
                            if (EmployeeId != inv.CreatedByUid)
                            {
                                NotificationUser nu = new NotificationUser()
                                {
                                    NotificationId = notification.NotificationId,
                                    IsRead = false,
                                    NotificationPerson = EmployeeId,
                                };
                                _Util.Facade.NotificationFacade.InsertNotificationUser(nu);

                                NotificationUser nus = new NotificationUser()
                                {
                                    NotificationId = notification.NotificationId,
                                    IsRead = false,
                                    NotificationPerson = inv.CreatedByUid,
                                };
                                _Util.Facade.NotificationFacade.InsertNotificationUser(nus);
                            }
                            else
                            {
                                NotificationUser nu = new NotificationUser()
                                {
                                    NotificationId = notification.NotificationId,
                                    IsRead = false,
                                    NotificationPerson = EmployeeId,
                                };
                                _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
                            }

                            #endregion


                        }
                        else
                        {
                            SalesGuy = tempComp.CompanyName;
                            SalesGuyEmail = "info@rmrcloud.com";
                        }
                        #endregion Reply Email Name And Address

                        //var Model = GetCreateInvoiceModel(inv, invDetList, tempComp, tempCustomer);
                        Model.Invoice.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CompanyId);

                        Model.Invoice.InvoiceId = Model.Invoice.Id.GenerateInvoiceNo();
                        if (Model.Invoice.BalanceDue > 0)
                        {
                            Model.AmountInWord = NumberToWords(Model.Invoice.BalanceDue.Value); //NumberToWords(Model.Invoice.TotalAmount.Value);
                        }
                        Model.InvoiceSetting = new InvoiceSetting();
                        string settingskey = @" 'InvoiceSettingsShipping', 'EstimateServiceSetting', 'InvoiceSettingsDiscount', 'InvoiceSettingsDeposit'";
                        List<GlobalSetting> printsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceSettingListByCompanyIdAndKey(settingskey, CompanyId);
                        foreach (var print in printsetting)
                        {
                            if (print.Value.ToLower() == "true")
                            {
                                if (print.SearchKey == "InvoiceSettingsDeposit")
                                {
                                    Model.InvoiceSetting.DepositSetting = true;
                                }
                                if (print.SearchKey == "EstimateServiceSetting")
                                {
                                    Model.InvoiceSetting.ServiceSetting = true;
                                }
                                if (print.SearchKey == "InvoiceSettingsDiscount")
                                {
                                    Model.InvoiceSetting.DiscountSetting = true;
                                }
                                if (print.SearchKey == "InvoiceSettingsShipping")
                                {
                                    Model.InvoiceSetting.ShippingSetting = true;
                                }
                            }
                        }
                        List<CreateInvoice> ModelList = new List<CreateInvoice>();
                        CreateInvoice createInvModel = new CreateInvoice();
                        createInvModel = Model;
                        createInvModel.Invoice.Id = createTempInv.Id;
                        createInvModel.Invoice.Status = createTempInv.Status;
                        createInvModel.Invoice.InvoiceId = createTempInv.InvoiceId;
                        createInvModel.Invoice.IsEstimate = createTempInv.IsEstimate;
                        createInvModel.Invoice.InvoiceFor = createTempInv.InvoiceFor;
                        createInvModel.InvoiceDetailList.Add(createTempInvDetl);
                        InvoicePaymentDate PayDate = _Util.Facade.TransactionFacade.GetLatestPaymentDateByInvoiceId(createInvModel.Invoice.Id);
                        if (PayDate != null)
                        {
                            createInvModel.Invoice.TransacationDate = PayDate.PaymentDate;
                        }
                        ModelList.Add(createInvModel);
                        //ViewBag.CompanyId = tempComp.CompanyId.ToString();
                        if (InvoiceEmail.Value.ToLower() == "true")
                        {
                            ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("~/Views/Invoice/InvoicePdf.cshtml", ModelList)
                            {
                                //FileName = "TestView.pdf",
                                PageSize = Rotativa.Options.Size.A4,
                                PageOrientation = Rotativa.Options.Orientation.Portrait,
                                PageMargins = { Left = 1, Right = 1 },

                            };
                            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);

                            string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(createInvModel.Invoice.Id
                                                + "#"
                                                + CompanyId
                                                + "#"
                                                + CustomerId);
                            string fullurl = string.Concat(AppConfig.SiteDomain, "/Customer-Invoice/", encryptedurl);
                            ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, CustomerId);
                            var EmailTemplate = _Util.Facade.MailFacade.GetTemplateByTemplateKey("InvoicePredefineEmailTemplate");
                            Hashtable datatemplate = new Hashtable();
                            datatemplate.Add("CustomerName", tempCustomer.FirstName + " " + tempCustomer.LastName);
                            datatemplate.Add("ExpirationDate", inv.DueDate);
                            datatemplate.Add("SalesPhone Number", string.IsNullOrWhiteSpace(SalesPhone) ? tempComp.Phone : SalesPhone);
                            datatemplate.Add("CompanyName", tempComp.CompanyName);
                            datatemplate.Add("SalesGuy", SalesGuy);
                            datatemplate.Add("url", string.Concat(AppConfig.ShortSiteDomain, "/shrt/", ShortUrl.Code));
                            emailtemplate = HS.Web.UI.Helper.LabelHelper.ParserHelper(EmailTemplate.BodyContent, datatemplate);
                            InvoiceCreatedEmail email = new InvoiceCreatedEmail()
                            {
                                CompanyName = tempComp.CompanyName,
                                CustomerName = tempCustomer.FirstName + " " + tempCustomer.LastName,
                                EmailBody = HttpUtility.HtmlDecode(emailtemplate),
                                FromEmail = SalesGuyEmail,
                                Subject = string.Format("New Invoice From {0}:{1}", tempComp.CompanyName, createInvModel.Invoice.InvoiceId),
                                //Discuss with Inan Vai.......Change InvoiceId in before line
                                FromName = SalesGuy,
                                ToEmail = tempCustomer.EmailAddress,
                                InvoicePdf = new Attachment(new MemoryStream(applicationPDFData), createInvModel.Invoice.InvoiceId + ".pdf")
                            };
                            var EmailSent = _Util.Facade.MailFacade.SendInvoiceCreatedEmail(email, CompanyId);
                            email.InvoicePdf.Dispose();
                        }
                    }
                    #region Correspondence Log
                    LeadCorrespondence objCor = new LeadCorrespondence()
                    {
                        CompanyId = CompanyId,
                        CustomerId = Model.Invoice.CustomerId,
                        TemplateKey = "InvoiceEmail",
                        Type = LabelHelper.CorrespondenceMessageTyp.Email,
                        ToEmail = tempCustomer.EmailAddress,
                        Subject = string.Format("New Invoice From {0}:{1}", tempComp.CompanyName, InvoiceId.GenerateInvoiceNo()),
                        BodyContent = HttpUtility.HtmlDecode(emailtemplate),
                        SentDate = DateTime.Now.UTCCurrentTime(),
                        IsSystemAutoSent = true,
                        SentBy = Model.Invoice.CreatedByUid
                    };
                    _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(objCor);
                    #endregion
                    #endregion

                }
                if (InvoiceEmail.Value.ToLower() == "true")
                {
                    #region Signed Up Confirmation SMS to Customer
                    if (tempCustomer.PreferedSms.HasValue && tempCustomer.PreferedSms.Value)
                    {
                        List<string> ReceiverNumberList = new List<string>();
                        string SMSText = "";
                        string ReceiverNumber = "";
                        //SMSText = string.Format("Thank you! You have successfully signed up in our estimate {0}.", InvoiceId.GenerateEstimateNo());

                        #region ReceiverNumber Setup
                        if (!string.IsNullOrWhiteSpace(tempCustomer.PrimaryPhone))
                        {
                            ReceiverNumber = tempCustomer.PrimaryPhone;
                        }
                        else if (!string.IsNullOrWhiteSpace(tempCustomer.SecondaryPhone))
                        {
                            ReceiverNumber = tempCustomer.SecondaryPhone.Replace("-", "");
                        }
                        ReceiverNumberList.Add(ReceiverNumber);
                        #endregion

                        LeadsEstimateAgree poSms = new LeadsEstimateAgree();

                        poSms.EnvoiceNo = InvoiceId.GenerateEstimateNo();
                        if (!string.IsNullOrWhiteSpace(ReceiverNumber))
                        {
                            if (_Util.Facade.SMSFacade.SendLeadEstimateAgreement(poSms, Guid.Empty, CompanyId, ReceiverNumberList, true, "System") == true)
                            {
                                LeadCorrespondence objCorres = new LeadCorrespondence()
                                {
                                    CompanyId = CompanyId,
                                    CustomerId = CustomerId,
                                    Type = LabelHelper.CorrespondenceMessageTyp.SMS,
                                    ToMobileNo = string.Join(";", ReceiverNumber),
                                    BodyContent = SMSText,
                                    SentDate = DateTime.Now.UTCCurrentTime(),
                                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                    SentBy = Model.Invoice.CreatedByUid
                                };
                                _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(objCorres);
                            }

                        }

                        //    if (!string.IsNullOrWhiteSpace(ReceiverNumber))
                        //{
                        //    result = _Util.Facade.SMSFacade.SendSMS(CompanyId, SMSText, ReceiverNumberList, true, "System");
                        //}
                        //result = SMSManager.SendASms(ReceiverNumberList, SenderNumber, SMSText, AuthId, AuthToken);
                        //if (result == true)
                        //{
                        //    LeadCorrespondence objCorres = new LeadCorrespondence()
                        //    {
                        //        CompanyId = CompanyId,
                        //        CustomerId = CustomerId,
                        //        Type = LabelHelper.CorrespondenceMessageTyp.SMS,
                        //        ToMobileNo = string.Join(";", ReceiverNumber),
                        //        BodyContent = SMSText,
                        //        SentDate = DateTime.Now.UTCCurrentTime(),
                        //        LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                        //        SentBy = Model.Invoice.CreatedByUid
                        //    };
                        //    _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(objCorres);
                        //}
                    }
                    #endregion
                }


                #region Send Email To Sales Person
                if (!string.IsNullOrWhiteSpace(tempCustomer.Soldby))
                {
                    Guid EmployeeId;
                    if (Guid.TryParse(tempCustomer.Soldby, out EmployeeId) && EmployeeId != new Guid())
                    {

                        Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(EmployeeId);

                        Employee createdby = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(inv.CreatedByUid);
                        if (emp != null && inv.CreatedByUid != null && EmployeeId != inv.CreatedByUid)
                        {
                            EstimateSignNotificationEmail email = new EstimateSignNotificationEmail
                            {
                                CompanyId = CompanyId,
                                CustomerName = string.IsNullOrWhiteSpace(tempCustomer.BusinessName) ? string.Concat(tempCustomer.FirstName, " ", tempCustomer.LastName) : tempCustomer.BusinessName,

                                EstimateNO = InvoiceId.GenerateEstimateNo(),
                                TotalAmount = inv.TotalAmount.HasValue ? inv.TotalAmount.Value : 0.0,
                                CompanyName = tempComp.CompanyName,

                            };

                            email.SalesPersonsName = string.Concat(emp.FirstName, " ", emp.LastName);

                            EstimateSignNotificationEmail emailcreatedby = new EstimateSignNotificationEmail
                            {
                                CompanyId = CompanyId,
                                CustomerName = string.IsNullOrWhiteSpace(tempCustomer.BusinessName) ? string.Concat(tempCustomer.FirstName, " ", tempCustomer.LastName) : tempCustomer.BusinessName,

                                EstimateNO = InvoiceId.GenerateEstimateNo(),
                                TotalAmount = inv.TotalAmount.HasValue ? inv.TotalAmount.Value : 0.0,
                                CompanyName = tempComp.CompanyName,

                            };

                            emailcreatedby.CreatedByName = string.Concat(createdby.FirstName, " ", createdby.LastName);

                            if (!string.IsNullOrWhiteSpace(emp.Email) && emp.Email.IsValidEmailAddress())
                            {

                                email.ToEmail = emp.Email;
                                email.EmailTo = "SalesPerson";
                                _Util.Facade.MailFacade.EstimateSignNotificationEmail(email);
                            }

                            else if (!string.IsNullOrWhiteSpace(emp.UserName) && emp.UserName.IsValidEmailAddress())
                            {
                                email.ToEmail = emp.UserName;
                                email.EmailTo = "SalesPerson";
                                _Util.Facade.MailFacade.EstimateSignNotificationEmail(email);
                            }

                            if (!string.IsNullOrWhiteSpace(createdby.Email) && createdby.Email.IsValidEmailAddress())
                            {
                                emailcreatedby.ToEmail = createdby.Email;
                                emailcreatedby.EmailTo = "CreatedBy";
                                _Util.Facade.MailFacade.EstimateSignNotificationEmail(emailcreatedby);
                            }

                            else if (!string.IsNullOrWhiteSpace(createdby.UserName) && createdby.UserName.IsValidEmailAddress())
                            {

                                emailcreatedby.ToEmail = createdby.UserName;
                                emailcreatedby.EmailTo = "CreatedBy";
                                _Util.Facade.MailFacade.EstimateSignNotificationEmail(emailcreatedby);
                            }

                        }
                        else if (emp != null)
                        {
                            EstimateSignNotificationEmail email = new EstimateSignNotificationEmail
                            {
                                CompanyId = CompanyId,
                                CustomerName = string.IsNullOrWhiteSpace(tempCustomer.BusinessName) ? string.Concat(tempCustomer.FirstName, " ", tempCustomer.LastName) : tempCustomer.BusinessName,

                                EstimateNO = InvoiceId.GenerateEstimateNo(),
                                TotalAmount = inv.TotalAmount.HasValue ? inv.TotalAmount.Value : 0.0,
                                CompanyName = tempComp.CompanyName,

                            };

                            email.SalesPersonsName = string.Concat(emp.FirstName, " ", emp.LastName);

                            if (!string.IsNullOrWhiteSpace(emp.Email) && emp.Email.IsValidEmailAddress())
                            {
                                email.ToEmail = emp.Email;
                                email.EmailTo = "SalesPerson";
                                _Util.Facade.MailFacade.EstimateSignNotificationEmail(email);
                            }
                            else if (!string.IsNullOrWhiteSpace(emp.UserName) && emp.UserName.IsValidEmailAddress())
                            {
                                email.ToEmail = emp.UserName;
                                email.EmailTo = "SalesPerson";
                                _Util.Facade.MailFacade.EstimateSignNotificationEmail(email);
                            }
                        }
                    }
                }
                #endregion
            }
            else
            {

                if (string.IsNullOrWhiteSpace(DeclinedReason))
                {
                    return Json(new { result = true, IsReload = false, message = "Please give us a decline reason." });
                }

                Invoice objinv = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);

                if (objinv.IsEstimate == false)
                {
                    return Json(new { result = true, IsReload = true, message = "You have already submitted" });
                }
                if (objinv != null)
                {
                    objinv.Status = LabelHelper.EstimateStatus.Declined;
                    objinv.CancelReason = DeclinedReason;
                    _Util.Facade.InvoiceFacade.UpdateInvoice(objinv);
                }
                #region Declined Email To SalesPerson
                Guid EmployeeId;
                Customer tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);

                if (Guid.TryParse(tempCustomer.Soldby, out EmployeeId) && EmployeeId != new Guid())
                {
                    Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(EmployeeId);

                    Invoice inv = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);

                    Employee createdby = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(inv.CreatedByUid);

                    if (emp != null && inv.CreatedByUid != null && EmployeeId != inv.CreatedByUid)
                    {
                        Company tempComp = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
                        EstimateSignNotificationEmail email = new EstimateSignNotificationEmail
                        {
                            CompanyId = CompanyId,
                            CustomerName = string.IsNullOrWhiteSpace(tempCustomer.BusinessName) ? string.Concat(tempCustomer.FirstName, " ", tempCustomer.LastName) : tempCustomer.BusinessName,

                            EstimateNO = InvoiceId.GenerateEstimateNo(),
                            TotalAmount = objinv.TotalAmount.HasValue ? objinv.TotalAmount.Value : 0.0,
                            CompanyName = tempComp.CompanyName,
                        };
                        email.SalesPersonsName = string.Concat(emp.FirstName, " ", emp.LastName);

                        EstimateSignNotificationEmail emailcreatedby = new EstimateSignNotificationEmail
                        {
                            CompanyId = CompanyId,
                            CustomerName = string.IsNullOrWhiteSpace(tempCustomer.BusinessName) ? string.Concat(tempCustomer.FirstName, " ", tempCustomer.LastName) : tempCustomer.BusinessName,

                            EstimateNO = InvoiceId.GenerateEstimateNo(),
                            TotalAmount = objinv.TotalAmount.HasValue ? objinv.TotalAmount.Value : 0.0,
                            CompanyName = tempComp.CompanyName,
                        };
                        emailcreatedby.CreatedByName = string.Concat(createdby.FirstName, " ", createdby.LastName);


                        if (!string.IsNullOrWhiteSpace(emp.Email) && emp.Email.IsValidEmailAddress())
                        {
                            email.ToEmail = emp.Email;
                            email.EmailTo = "DeclinedEstimate";
                            email.DeclinationReason = DeclinedReason;
                            _Util.Facade.MailFacade.EstimateSignNotificationEmail(email);
                        }

                        else if (!string.IsNullOrWhiteSpace(emp.UserName) && emp.UserName.IsValidEmailAddress())
                        {

                            email.ToEmail = emp.UserName;
                            email.EmailTo = "DeclinedEstimate";
                            email.DeclinationReason = DeclinedReason;
                            _Util.Facade.MailFacade.EstimateSignNotificationEmail(email);
                        }

                        if (!string.IsNullOrWhiteSpace(createdby.Email) && createdby.Email.IsValidEmailAddress())
                        {
                            emailcreatedby.ToEmail = createdby.Email;
                            emailcreatedby.EmailTo = "DeclinedEstimateCreatedBy";
                            emailcreatedby.DeclinationReason = DeclinedReason;
                            _Util.Facade.MailFacade.EstimateSignNotificationEmail(emailcreatedby);
                        }

                        else if (!string.IsNullOrWhiteSpace(createdby.UserName) && createdby.UserName.IsValidEmailAddress())
                        {

                            emailcreatedby.ToEmail = createdby.UserName;
                            emailcreatedby.EmailTo = "DeclinedEstimateCreatedBy";
                            emailcreatedby.DeclinationReason = DeclinedReason;
                            _Util.Facade.MailFacade.EstimateSignNotificationEmail(emailcreatedby);
                        }

                    }

                    else if (emp != null)
                    {
                        Company tempComp = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
                        EstimateSignNotificationEmail email = new EstimateSignNotificationEmail
                        {
                            CompanyId = CompanyId,
                            CustomerName = string.IsNullOrWhiteSpace(tempCustomer.BusinessName) ? string.Concat(tempCustomer.FirstName, " ", tempCustomer.LastName) : tempCustomer.BusinessName,

                            EstimateNO = InvoiceId.GenerateEstimateNo(),
                            TotalAmount = objinv.TotalAmount.HasValue ? objinv.TotalAmount.Value : 0.0,
                            CompanyName = tempComp.CompanyName,
                        };
                        email.SalesPersonsName = string.Concat(emp.FirstName, " ", emp.LastName);

                        if (!string.IsNullOrWhiteSpace(emp.Email) && emp.Email.IsValidEmailAddress())
                        {
                            email.ToEmail = emp.Email;
                            email.EmailTo = "DeclinedEstimate";
                            email.DeclinationReason = DeclinedReason;
                            _Util.Facade.MailFacade.EstimateSignNotificationEmail(email);
                        }
                        else if (!string.IsNullOrWhiteSpace(emp.UserName) && emp.UserName.IsValidEmailAddress())
                        {
                            email.ToEmail = emp.UserName;
                            email.EmailTo = "DeclinedEstimate";
                            email.DeclinationReason = DeclinedReason;
                            _Util.Facade.MailFacade.EstimateSignNotificationEmail(email);
                        }
                    }
                }
                #endregion
            }
            bool IsReload = false;
            string Message = "Your estimate has been signed and sent to our office. You will also receive a confirmation about your signed estimate!";
            if (!status)
            {
                Message = "You have successfully declined your estimate.";
                IsReload = true;
            }
            return Json(new { result = true, IsReload = IsReload, message = Message });
        }
        #endregion LeadsEstimate

        [HttpPost]
        public JsonResult UploadCustomerSignatureImage_v2(string data, string token)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(data))
            {
                return Json(new { uploadImage = false, message = "invalid data" });
            }
            string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(token).Split('#');
            Guid CustomerId = new Guid(Decryptval[0]);
            Guid CompanyId = new Guid(Decryptval[1]);
            int InvoiceId = Convert.ToInt32(Decryptval[2]);
            Customer tmpCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            if (tmpCustomer == null)
            {
                return Json(new { result = false, message = "customer not found" });
            }
            if (!_Util.Facade.CustomerFacade.CustomerIsInCompany(CustomerId, CompanyId))
            {
                return Json(new { result = false, message = "invalid user" });
            }
            bool uploadImage = false;
            string filePath = "";
            string[] datasplit = data.Split(',');
            byte[] bytes = Convert.FromBase64String(datasplit[1]);
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
                string tempFolder = ConfigurationManager.AppSettings["File.CustomerSignatureFile"];
                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();
                var FtempFolderName = string.Format(tempFolder, comname) + tmpCustomer.Id + "Signature";
                Random rand = new Random();
                string FileName = rand.Next().ToString();
                FileName += "-___" + "Signature.png";
                string tempFolderPath = Server.MapPath("~/" + FtempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        image.Save(Path.Combine(tempFolderPath, FileName));
                        uploadImage = true;
                    }
                    catch (Exception)
                    {

                    }
                }
                filePath = string.Concat("/", FtempFolderName, "/", FileName);
            }
            var serverFile = Server.MapPath(filePath);
            bool rsult = _Util.Facade.CustomerFacade.CustomerIsInCompany(tmpCustomer.Id, CompanyId);

            if (!System.IO.File.Exists(serverFile))
            {
                return Json(new { result = false, message = "File not exsists" });
            }
            tmpCustomer.Singature = AppConfig.DomainSitePath + filePath;
            tmpCustomer.CustomerSignatureDate = DateTime.Now.UTCCurrentTime();
            _Util.Facade.CustomerFacade.UpdateCustomer(tmpCustomer);
            Invoice inv = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);
            inv.Signature = AppConfig.DomainSitePath + filePath;
            inv.Status = LabelHelper.EstimateStatus.Signed;
            inv.SignatureDate = DateTime.Now.UTCCurrentTime();
            _Util.Facade.InvoiceFacade.UpdateInvoice(inv);
            string status = "";
            var EmailReceiver = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("SendContractSignEstimateSignAndFileSignNotificationEmail");
            if (EmailReceiver != null)
            {
                status = EmailReceiver.Value;
            }
            if (status == "true")
            {
                SendEstimateSignNotificationEmail(inv.InvoiceId, CompanyId);
            }

            #region Estimate Signed SMS
            if (inv != null)
            {
                #region ReceiverNumber Setup
                List<string> ReceiverNumberList = new List<string>();
                GlobalSetting GlobalSettingModel = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("EstimateSignedSms");

                string PrefferedNO = "";
                if (GlobalSettingModel != null && GlobalSettingModel.Value.ToLower() == "true")
                {
                    PrefferedNO = GlobalSettingModel.OptionalValue.Replace(" ", ""); ;
                }

                if (!string.IsNullOrWhiteSpace(PrefferedNO))
                {
                    PrefferedNO = PrefferedNO.Replace("-", "");
                }

                string[] PrefferedNOList = PrefferedNO.Split(',');
                if (PrefferedNOList != null && PrefferedNOList.Length > 0)
                {
                    for (int i = 0; i < PrefferedNOList.Length; i++)
                    {

                        if (!(PrefferedNOList[i].IndexOf("+88") > -1) && PrefferedNOList[i].Count() == 11)
                        {
                            //PrefferedNOList[i] =  PrefferedNOList[i].Replace("-", "").Replace(")", "").Replace("(", "").Replace(" ", "");
                            PrefferedNOList[i] = "+88" + PrefferedNOList[i];
                        }
                        ReceiverNumberList.Add(PrefferedNOList[i]);


                    }
                }


                #endregion
                string phonenumber = string.Join(";", ReceiverNumberList);
                if (ReceiverNumberList.Count() > 0)
                {
                    _Util.Facade.SMSFacade.SendEstimateSignedSMS(tmpCustomer.FirstName + " " + tmpCustomer.LastName, inv.InvoiceId, inv.TotalAmount != null ? inv.TotalAmount.Value : 0, Guid.Empty, CompanyId, ReceiverNumberList, false, string.Empty);

                }
            }
            #endregion

            #region Make the Estimate_Signed.pdf file
            bool AfterSignDocumentSave = false;
            GlobalSetting GlobSet = _Util.Facade.GlobalSettingsFacade.GetGlobalsettingBySearchKeyAndCompanyId("AfterSignDocumentSave", CompanyId);
            if (GlobSet != null && !string.IsNullOrWhiteSpace(GlobSet.Value))
            {
                if (GlobSet.Value == "true")
                {
                    AfterSignDocumentSave = true;
                }
            }
            if (AfterSignDocumentSave == true)
            {
                Customer tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
                Company tempComp = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
                CreateInvoice Model = new CreateInvoice();
                List<InvoiceDetail> invDetList = _Util.Facade.InvoiceFacade.GetInvoiceDetialsListByInvoiceId(inv.InvoiceId);
                Model = GetCreateInvoiceModel(inv, invDetList, tempComp, tempCustomer);
                Model.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CompanyId);
                Model.Invoice.InvoiceId = Model.Invoice.Id.GenerateEstimateNo();

                #region Insert Customer Agreement
                CustomerAgreement CustomerAgreement = new CustomerAgreement()
                {
                    CompanyId = CompanyId,
                    CustomerId = CustomerId,
                    IP = AppConfig.GetIP,
                    UserAgent = AppConfig.GetUserAgent,
                    Type = LabelHelper.CustomerAgreementLog.SubmitEstimate,
                    InvoiceId = InvoiceId.GenerateEstimateNo(),
                    AddedDate = DateTime.Now.UTCCurrentTime(),
                };
                _Util.Facade.CustomerAgreementFacade.InsertCustomerAgreement(CustomerAgreement);
                #endregion

                Model.Invoice.CustomerAgreement = _Util.Facade.CustomerAgreementFacade.GetCustomerAgreementByCompanyIdAndCustomerIdAndInvoiceId(CompanyId, CustomerId, InvoiceId.GenerateEstimateNo());
                //ViewBag.CompanyId = tempComp.CompanyId.ToString();
                Model.InvoiceSetting = new InvoiceSetting();
                string settingskey = @" 'InvoiceSettingsShipping', 'EstimateServiceSetting', 'InvoiceSettingsDiscount', 'InvoiceSettingsDeposit'";
                List<GlobalSetting> printsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceSettingListByCompanyIdAndKey(settingskey, CompanyId);
                foreach (var print in printsetting)
                {
                    if (print.Value.ToLower() == "true")
                    {
                        if (print.SearchKey == "InvoiceSettingsDeposit")
                        {
                            Model.InvoiceSetting.DepositSetting = true;
                        }
                        if (print.SearchKey == "EstimateServiceSetting")
                        {
                            Model.InvoiceSetting.ServiceSetting = true;
                        }
                        if (print.SearchKey == "InvoiceSettingsDiscount")
                        {
                            Model.InvoiceSetting.DiscountSetting = true;
                        }
                        if (print.SearchKey == "InvoiceSettingsShipping")
                        {
                            Model.InvoiceSetting.ShippingSetting = true;
                        }
                    }
                }
                GlobalSetting monitoringVal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateMonitoringAmount");
                GlobalSetting contractTerm = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateContractTerm");
                GlobalSetting onitWater = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowONITWaterLLCForEstimate");
                GlobalSetting monitoringDes = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateMonitoringDescription");
                GlobalSetting thompsonEstimate = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowThompsonEstimateText");
                GlobalSetting gutterEqpImg = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowGutterEstimateEquipmentImage");
                if (gutterEqpImg != null)
                {
                    Model.ShowGutterEquipmentImage = gutterEqpImg.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowGutterEquipmentImage = false;
                }
                if (thompsonEstimate != null)
                {
                    Model.ShowThompsonEstimateText = thompsonEstimate.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowThompsonEstimateText = false;
                }
                if (monitoringDes != null)
                {
                    Model.ShowEstimateMonitoringDescription = monitoringDes.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowEstimateMonitoringDescription = false;
                }
                if (monitoringVal != null)
                {
                    Model.ShowEstimateMonitoringAmount = monitoringVal.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowEstimateMonitoringAmount = false;
                }
                if (contractTerm != null)
                {
                    Model.ShowEstimateContractTerm = contractTerm.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowEstimateContractTerm = false;
                }
                if (onitWater != null)
                {
                    Model.ShowOnitWaterTreatment = onitWater.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowOnitWaterTreatment = false;
                }
                GlobalSetting invCode3Sta = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowCode3InvoiceStaticBox");
                if (invCode3Sta != null)
                {
                    Model.ShowCode3InvoiceStaticBox = invCode3Sta.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowCode3InvoiceStaticBox = false;
                }
                GlobalSetting invComAddress = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowInvoiceCompanyAddress");
                if (invComAddress != null)
                {
                    Model.ShowInvoiceCompanyAddress = invComAddress.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowInvoiceCompanyAddress = false;
                }
                ViewAsPdf EstimateActionPdf = new Rotativa.ViewAsPdf("~/Views/Estimate/EstimatePdf.cshtml", Model)
                {
                    //FileName = "TestView.pdf",
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
                byte[] EstimatePdfData = EstimateActionPdf.BuildPdf(ControllerContext);

                #region Save Estimate.pdf to file System  
                string estimateno = Model.Invoice.Id.GenerateEstimateNo();
                string filename = ConfigurationManager.AppSettings["File.EstimateFiles"];
                var comnamee = tempComp.CompanyName.ReplaceSpecialChar();
                filename = string.Format(filename, comnamee);
                filename += DateTime.Now.UTCCurrentTime().Year.ToString()
                    + "/" + DateTime.Now.UTCCurrentTime().Month.ToString()
                    + "/" + estimateno + "_Signed.pdf";
                string Serverfilename = FileHelper.GetFileFullPath(filename);
                FileHelper.SaveFile(EstimatePdfData, Serverfilename);
                #endregion

                #region Save CustomerFile
                _Util.Facade.CustomerFileFacade.SaveEstimatePdfFile(AppConfig.DomainSitePath + filename, estimateno, CustomerId, CompanyId, true);
                #endregion
            }
            #endregion
            return Json(new { uploadImage = uploadImage, UploadFilePath = filePath }, "text/html");
        }

        public JsonResult UploadCustomerSignatureImage(string data, string token)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(data))
            {
                return Json(new { uploadImage = false, message = "invalid data" });
            }
            string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(token).Split('#');
            Guid CustomerId = new Guid(Decryptval[0]);
            Guid CompanyId = new Guid(Decryptval[1]);
            int InvoiceId = Convert.ToInt32(Decryptval[2]);
            Customer tmpCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            if (tmpCustomer == null)
            {
                return Json(new { result = false, message = "customer not found" });
            }
            if (!_Util.Facade.CustomerFacade.CustomerIsInCompany(CustomerId, CompanyId))
            {
                return Json(new { result = false, message = "invalid user" });
            }
            bool uploadImage = false;
            string filePath = "";
            string[] datasplit = data.Split(',');
            byte[] bytes = Convert.FromBase64String(datasplit[1]);
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                #region file save
                //image = Image.FromStream(ms);
                //string tempFolder = ConfigurationManager.AppSettings["File.CustomerSignatureFile"];
                //var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();
                //var FtempFolderName = string.Format(tempFolder, comname) + tmpCustomer.Id + "Signature";
                //Random rand = new Random();
                //string FileName = rand.Next().ToString();
                //FileName += "-___" + "Signature.png";
                //string tempFolderPath = Server.MapPath("~/" + FtempFolderName);

                //if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                //{
                //    try
                //    {
                //        image.Save(Path.Combine(tempFolderPath, FileName));
                //        uploadImage = true;
                //    }
                //    catch (Exception)
                //    {

                //    }
                //}
                //filePath = string.Concat("/", FtempFolderName, "/", FileName);
                #endregion

                //// "mayur" AWS S3 Changes //// Start

                #region File Save on AWS S3

                Random rand = new Random();
                image = Image.FromStream(ms);

                string filename = ConfigurationManager.AppSettings["File.CustomerSignatureFile"];
                filename = filename.TrimEnd('/');

                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();

                string FilePath = string.Format(filename, comname);
                FilePath += "/" + tmpCustomer.Id + "Signature";
                string FileName = rand.Next().ToString() + "-___" + "Signature.png"; ;

                string FileKey = string.Format($"{FilePath}/{FileName}");

                var returnurl = "";

                var task = Task.Run(async () =>
                {
                    AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                    await AWSobject.UploadFile(FileKey, bytes);
                    await AWSobject.MakePublic(FileName, FilePath);
                });

                task.Wait();

                /// "mayur" used thread for async s3 methods : start

                //Thread thread = new Thread(async () => {

                //    AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                //    await AWSobject.UploadFile(FileKey, bytes);
                //    await AWSobject.MakePublic(FileName, FilePath);

                //});
                //thread.Start();

                /// "mayur" used thread for async s3 methods : End



                returnurl = String.Format(AppConfig.AWSS3Url, AppConfig.AWSS3BucketName);
                returnurl = returnurl + FileKey;


                ViewBag.ReturnUrl = returnurl;
                ViewBag.FileName = FileName;
                ViewBag.FileKey = FileKey;

                #endregion

                //// "mayur" AWS S3 Changes //// End





            }
            //var serverFile = Server.MapPath(filePath);
            bool rsult = _Util.Facade.CustomerFacade.CustomerIsInCompany(tmpCustomer.Id, CompanyId);

            AWSS3ObjectService _AWSobject = new AWSS3ObjectService();

            bool IsfileExist = _AWSobject.CheckFileExists(ViewBag.ReturnUrl);

            if (!IsfileExist)
            {
                return Json(new { result = false, message = "File not exist" });
            }

            tmpCustomer.Singature = ViewBag.ReturnUrl;
            tmpCustomer.CustomerSignatureDate = DateTime.Now.UTCCurrentTime();
            _Util.Facade.CustomerFacade.UpdateCustomer(tmpCustomer);
            Invoice inv = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);
            inv.Signature = ViewBag.ReturnUrl;
            inv.Status = LabelHelper.EstimateStatus.Signed;
            inv.SignatureDate = DateTime.Now.UTCCurrentTime();
            _Util.Facade.InvoiceFacade.UpdateInvoice(inv);
            string status = "";
            var EmailReceiver = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("SendContractSignEstimateSignAndFileSignNotificationEmail");
            if (EmailReceiver != null)
            {
                status = EmailReceiver.Value;
            }
            if (status == "true")
            {
                SendEstimateSignNotificationEmail(inv.InvoiceId, CompanyId);
            }

            #region Estimate Signed SMS
            if (inv != null)
            {
                #region ReceiverNumber Setup
                List<string> ReceiverNumberList = new List<string>();
                GlobalSetting GlobalSettingModel = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("EstimateSignedSms");

                string PrefferedNO = "";
                if (GlobalSettingModel != null && GlobalSettingModel.Value.ToLower() == "true")
                {
                    PrefferedNO = GlobalSettingModel.OptionalValue.Replace(" ", ""); ;
                }

                if (!string.IsNullOrWhiteSpace(PrefferedNO))
                {
                    PrefferedNO = PrefferedNO.Replace("-", "");
                }

                string[] PrefferedNOList = PrefferedNO.Split(',');
                if (PrefferedNOList != null && PrefferedNOList.Length > 0)
                {
                    for (int i = 0; i < PrefferedNOList.Length; i++)
                    {

                        if (!(PrefferedNOList[i].IndexOf("+88") > -1) && PrefferedNOList[i].Count() == 11)
                        {
                            //PrefferedNOList[i] =  PrefferedNOList[i].Replace("-", "").Replace(")", "").Replace("(", "").Replace(" ", "");
                            PrefferedNOList[i] = "+88" + PrefferedNOList[i];
                        }
                        ReceiverNumberList.Add(PrefferedNOList[i]);


                    }
                }


                #endregion
                string phonenumber = string.Join(";", ReceiverNumberList);
                if (ReceiverNumberList.Count() > 0)
                {
                    _Util.Facade.SMSFacade.SendEstimateSignedSMS(tmpCustomer.FirstName + " " + tmpCustomer.LastName, inv.InvoiceId, inv.TotalAmount != null ? inv.TotalAmount.Value : 0, Guid.Empty, CompanyId, ReceiverNumberList, false, string.Empty);

                }
            }
            #endregion

            #region Make the Estimate_Signed.pdf file
            bool AfterSignDocumentSave = false;
            GlobalSetting GlobSet = _Util.Facade.GlobalSettingsFacade.GetGlobalsettingBySearchKeyAndCompanyId("AfterSignDocumentSave", CompanyId);
            if (GlobSet != null && !string.IsNullOrWhiteSpace(GlobSet.Value))
            {
                if (GlobSet.Value == "true")
                {
                    AfterSignDocumentSave = true;
                }
            }
            if (AfterSignDocumentSave == true)
            {
                Customer tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
                Company tempComp = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
                CreateInvoice Model = new CreateInvoice();
                List<InvoiceDetail> invDetList = _Util.Facade.InvoiceFacade.GetInvoiceDetialsListByInvoiceId(inv.InvoiceId);
                Model = GetCreateInvoiceModel(inv, invDetList, tempComp, tempCustomer);
                Model.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CompanyId);
                Model.Invoice.InvoiceId = Model.Invoice.Id.GenerateEstimateNo();

                #region Insert Customer Agreement
                CustomerAgreement CustomerAgreement = new CustomerAgreement()
                {
                    CompanyId = CompanyId,
                    CustomerId = CustomerId,
                    IP = AppConfig.GetIP,
                    UserAgent = AppConfig.GetUserAgent,
                    Type = LabelHelper.CustomerAgreementLog.SubmitEstimate,
                    InvoiceId = InvoiceId.GenerateEstimateNo(),
                    AddedDate = DateTime.Now.UTCCurrentTime(),
                };
                _Util.Facade.CustomerAgreementFacade.InsertCustomerAgreement(CustomerAgreement);
                #endregion

                Model.Invoice.CustomerAgreement = _Util.Facade.CustomerAgreementFacade.GetCustomerAgreementByCompanyIdAndCustomerIdAndInvoiceId(CompanyId, CustomerId, InvoiceId.GenerateEstimateNo());
                //ViewBag.CompanyId = tempComp.CompanyId.ToString();
                Model.InvoiceSetting = new InvoiceSetting();
                string settingskey = @" 'InvoiceSettingsShipping', 'EstimateServiceSetting', 'InvoiceSettingsDiscount', 'InvoiceSettingsDeposit'";
                List<GlobalSetting> printsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceSettingListByCompanyIdAndKey(settingskey, CompanyId);
                foreach (var print in printsetting)
                {
                    if (print.Value.ToLower() == "true")
                    {
                        if (print.SearchKey == "InvoiceSettingsDeposit")
                        {
                            Model.InvoiceSetting.DepositSetting = true;
                        }
                        if (print.SearchKey == "EstimateServiceSetting")
                        {
                            Model.InvoiceSetting.ServiceSetting = true;
                        }
                        if (print.SearchKey == "InvoiceSettingsDiscount")
                        {
                            Model.InvoiceSetting.DiscountSetting = true;
                        }
                        if (print.SearchKey == "InvoiceSettingsShipping")
                        {
                            Model.InvoiceSetting.ShippingSetting = true;
                        }
                    }
                }
                GlobalSetting monitoringVal = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateMonitoringAmount");
                GlobalSetting contractTerm = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateContractTerm");
                GlobalSetting onitWater = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowONITWaterLLCForEstimate");
                GlobalSetting monitoringDes = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "EstimateMonitoringDescription");
                GlobalSetting thompsonEstimate = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowThompsonEstimateText");
                GlobalSetting gutterEqpImg = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowGutterEstimateEquipmentImage");
                if (gutterEqpImg != null)
                {
                    Model.ShowGutterEquipmentImage = gutterEqpImg.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowGutterEquipmentImage = false;
                }
                if (thompsonEstimate != null)
                {
                    Model.ShowThompsonEstimateText = thompsonEstimate.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowThompsonEstimateText = false;
                }
                if (monitoringDes != null)
                {
                    Model.ShowEstimateMonitoringDescription = monitoringDes.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowEstimateMonitoringDescription = false;
                }
                if (monitoringVal != null)
                {
                    Model.ShowEstimateMonitoringAmount = monitoringVal.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowEstimateMonitoringAmount = false;
                }
                if (contractTerm != null)
                {
                    Model.ShowEstimateContractTerm = contractTerm.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowEstimateContractTerm = false;
                }
                if (onitWater != null)
                {
                    Model.ShowOnitWaterTreatment = onitWater.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowOnitWaterTreatment = false;
                }
                GlobalSetting invCode3Sta = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowCode3InvoiceStaticBox");
                if (invCode3Sta != null)
                {
                    Model.ShowCode3InvoiceStaticBox = invCode3Sta.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowCode3InvoiceStaticBox = false;
                }
                GlobalSetting invComAddress = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(Model.Invoice.CompanyId, "ShowInvoiceCompanyAddress");
                if (invComAddress != null)
                {
                    Model.ShowInvoiceCompanyAddress = invComAddress.Value.ToLower() == "true" ? true : false;
                }
                else
                {
                    Model.ShowInvoiceCompanyAddress = false;
                }
                ViewAsPdf EstimateActionPdf = new Rotativa.ViewAsPdf("~/Views/Estimate/EstimatePdf.cshtml", Model)
                {
                    //FileName = "TestView.pdf",
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
                byte[] EstimatePdfData = EstimateActionPdf.BuildPdf(ControllerContext);

                #region Save Estimate.pdf to file System  
                string estimateno = Model.Invoice.Id.GenerateEstimateNo();
                string filename = ConfigurationManager.AppSettings["File.EstimateFiles"];
                var comnamee = tempComp.CompanyName.ReplaceSpecialChar();
                filename = string.Format(filename, comnamee);
                filename += DateTime.Now.UTCCurrentTime().Year.ToString()
                    + "/" + DateTime.Now.UTCCurrentTime().Month.ToString()
                    + "/" + estimateno + "_Signed.pdf";
                string Serverfilename = FileHelper.GetFileFullPath(filename);
                FileHelper.SaveFile(EstimatePdfData, Serverfilename);
                #endregion

                #region Save CustomerFile
                _Util.Facade.CustomerFileFacade.SaveEstimatePdfFile(AppConfig.DomainSitePath + filename, estimateno, CustomerId, CompanyId, true);
                #endregion
            }
            #endregion
            return Json(new { uploadImage = uploadImage, UploadFilePath = ViewBag.FileKey }, "text/html");
        }

        [HttpPost]
        public JsonResult WindowLoadCookieData(string Currentdate, string Currenttime, string Currentzone, string currenttimezone, int? LeadConvertId)
        {
            bool delres = false;
            HttpCookie CurrentUserTimeZone = new HttpCookie(CookieKeys.CurrentUser);
            CurrentUserTimeZone[CookieKeys.CurrentUserTimeZone] = Currentzone;
            CurrentUserTimeZone[CookieKeys.CurrentUserDate] = Currentdate;
            CurrentUserTimeZone[CookieKeys.CurrentUserTime] = Currenttime;
            CurrentUserTimeZone[CookieKeys.CurrentUserTimeZoneName] = currenttimezone;
            //CurrentUserTimeZone.Value = CurrentUserTimeZone;
            CurrentUserTimeZone.Expires = DateTime.Now.UTCCurrentTime().AddDays(2d);
            Response.Cookies.Add(CurrentUserTimeZone);
            if (LeadConvertId.HasValue && LeadConvertId > 0)
            {
                var objcus = _Util.Facade.CustomerFacade.GetCustomerById(LeadConvertId.Value);
                if (objcus != null)
                {
                    var objans = _Util.Facade.AgreementFacade.GetAllAgreementAnswerByCustomerId(objcus.CustomerId);
                    if (objans.Count > 0)
                    {
                        delres = _Util.Facade.AgreementFacade.DeleteAgreementAnswerByCustomerId(objcus.CustomerId);
                    }
                    else
                    {
                        delres = true;
                    }
                    var objcusag = _Util.Facade.CustomerAgreementFacade.GetAllCustomerAgreementByCustomerId(objcus.CustomerId);
                    if (objcusag.Count < 3)
                    {
                        foreach (var item in objcusag)
                        {
                            _Util.Facade.CustomerAgreementFacade.DeleteAgreementInfoById(item.Id);
                        }
                    }
                }
            }
            return Json(new { delres = delres });
        }




        //==== Code For The Booking ====
        #region LeadsBooking

        public ActionResult LeadsBooking(string code)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    FormsAuthentication.SignOut();
                    SessionHelper hs = new SessionHelper();
                    hs.ClearCurrentSession();
                }
                ViewBag.Token = code;
                string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(code).Split('#');
                Guid CustomerId = new Guid(Decryptval[0]);
                Guid CompanyId = new Guid(Decryptval[1]);
                int BookingId = Convert.ToInt32(Decryptval[2]);

                #region ConnectionStringSetup
                CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByCompanyId(CompanyId.ToString());

                if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                else
                {
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
                    Company Com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
                    ViewBag.Title = string.Concat(Com.CompanyName, " | ", Localize.T("Booking Sign"));
                }
                #endregion

                CompanyBranch cb = _Util.Facade.CompanyBranchFacade.GetMainBranchByCompanyId(CompanyId);
                ViewBag.CompanyLogo = cb.Logo;

                Booking book = _Util.Facade.BookingFacade.GetBookingById(BookingId);
                if (CompanyId != book.CompanyId
                    || CustomerId != book.CustomerId
                    || book.Status == LabelHelper.BookingStatus.Declined)
                {
                    return PartialView("~/Views/Shared/_LinkExpired.cshtml");
                }
                Customer tmpCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
                book.CustomerBussinessName = tmpCustomer.BusinessName;
                book.CustomerName = tmpCustomer.FirstName + " " + tmpCustomer.LastName;

                if (book.Status != LabelHelper.BookingStatus.Approved
                    && book.Status != LabelHelper.BookingStatus.Declined
                    && book.Status != LabelHelper.BookingStatus.CancelBooking)
                {
                    #region Insert Customer Agreement
                    //Model.Booking.CustomerAgreement
                    CustomerAgreement CustomerAgreement = new CustomerAgreement()
                    {
                        CompanyId = CompanyId,
                        CustomerId = CustomerId,
                        IP = AppConfig.GetIP,
                        UserAgent = AppConfig.GetUserAgent,
                        Type = LabelHelper.CustomerAgreementLog.LoadBooking,
                        InvoiceId = BookingId.GenerateBookingNo(),
                        AddedDate = DateTime.Now.UTCCurrentTime(),
                    };
                    _Util.Facade.CustomerAgreementFacade.InsertCustomerAgreement(CustomerAgreement);
                    #endregion
                }

                ViewBag.FooterCompanyInformation = _Util.Facade.GlobalSettingsFacade.GetFooterCompanyInformation(CompanyId).Replace("##Year##", string.Format("2017-{0}", DateTime.Now.Year.ToString()));

                return View(book);
            }
            catch (Exception ex)
            {
                return PartialView("~/Views/Shared/_Loading.cshtml");
            }
        }

        public ActionResult LeadsBookingPdf(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            try
            {
                string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(code).Split('#');
                Guid CusotmerId = new Guid(Decryptval[0]);
                Guid CompanyId = new Guid(Decryptval[1]);
                int BookingId = Convert.ToInt32(Decryptval[2]);

                if (Session[SessionKeys.CompanyConnectionString] == null || string.IsNullOrWhiteSpace(Session[SessionKeys.CompanyConnectionString].ToString()))
                {
                    CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByCompanyId(CompanyId.ToString());

                    if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
                    {
                        return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                    }
                    else
                    {
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
                    }
                }

                Company Com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
                ViewBag.ComName = Com.CompanyName;
                CreateBooking Model;
                Booking Book = _Util.Facade.BookingFacade.GetBookingById(BookingId);
                if (CompanyId != Book.CompanyId)
                {
                    return PartialView("~/Views/Shared/_LinkExpired.cshtml");
                }
                //Book.Status = LabelHelper.BookingStatus.CustomerViewed;
                //_Util.Facade.BookingFacade.UpdateBooking(Book);

                List<BookingDetails> BookingDetailList = _Util.Facade.BookingFacade.GetBookingDetailsByBookingId(Book.BookingId);
                Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Book.CompanyId);
                Customer tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CusotmerId);
                Model = GetCreateBookingModel(Book, BookingDetailList, tempCom, tempCustomer);
                Model.BookingExtraItem = _Util.Facade.BookingFacade.GetBookingExtraItemListByBookingId(Book.Id);
                Model.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CompanyId);

                return new Rotativa.ViewAsPdf("~/Views/Booking/BookingPdf.cshtml", Model)
                {
                    //FileName = "TestView.pdf",
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },
                };

            }
            catch (Exception)
            {
                return PartialView("~/Views/Shared/_Loading.cshtml");
            }
        }

        public JsonResult LeadsBookingAgree_v2(string code, bool status, string DeclinedReason)
        {
            bool result = false;
            if (string.IsNullOrWhiteSpace(code))
            {
                return Json(new { result = false, message = "Access denied." });
            }
            string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(code).Split('#');
            Guid CustomerId = new Guid(Decryptval[0]);
            Guid CompanyId = new Guid(Decryptval[1]);
            int BookingId = Convert.ToInt32(Decryptval[2]);
            if (status == true)
            {
                Booking book = _Util.Facade.BookingFacade.GetBookingById(BookingId);
                if (book != null)
                {
                    book.Status = LabelHelper.BookingStatus.Approved;
                    book.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    book.LastUpdatedBy = new Guid("22222222-2222-2222-2222-222222222222");
                    _Util.Facade.BookingFacade.UpdateBooking(book);
                }

                #region Make lead customer
                CustomerCompany cc = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerIdAndCompanyId(CustomerId, CompanyId);
                Customer tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
                if (cc == null || tempCustomer == null)
                {
                    return Json(new { result = false, message = "Customer not found." });
                }
                //Signing up booking will convert lead to customer //decision changed 8/26/19
                else if (cc.IsLead)
                {
                    cc.IsLead = false;
                    cc.ConvertionDate = DateTime.Now.UTCCurrentTime();
                    cc.ConvertionType = LabelHelper.LeadConvertionType.System;
                    _Util.Facade.CustomerFacade.UpdateCustomerCompany(cc);
                }
                #endregion

                List<BookingDetails> bookDetList = _Util.Facade.BookingFacade.GetBookingDetialsListByBookingId(book.BookingId);


                Company tempComp = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);

                #region Make the Booking_Signed.pdf file
                var Model = GetCreateBookingModel(book, bookDetList, tempComp, tempCustomer);
                Model.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CompanyId);
                Model.Booking.BookingId = Model.Booking.Id.GenerateBookingNo();


                #region Insert Customer Agreement
                //Model.Booking.CustomerAgreement
                CustomerAgreement CustomerAgreement = new CustomerAgreement()
                {
                    CompanyId = CompanyId,
                    CustomerId = CustomerId,
                    IP = AppConfig.GetIP,
                    UserAgent = AppConfig.GetUserAgent,
                    Type = LabelHelper.CustomerAgreementLog.SubmitBooking,
                    InvoiceId = book.BookingId,
                    AddedDate = DateTime.Now.UTCCurrentTime(),
                };
                //Note -> Need to Booking Id in the CustomerAgreement Table For Log 
                _Util.Facade.CustomerAgreementFacade.InsertCustomerAgreement(CustomerAgreement);
                #endregion
                //Note -> Need to Booking Id in the CustomerAgreement Table For Log 
                //Model.Booking.CustomerAgreement = _Util.Facade.CustomerAgreementFacade.GetCustomerAgreementByCompanyIdAndCustomerIdAndInvoiceId(CompanyId, CustomerId, InvoiceId.GenerateEstimateNo());

                ViewAsPdf BookingActionPdf = new Rotativa.ViewAsPdf("~/Views/Booking/BookingPdf.cshtml", Model)
                {
                    //FileName = "TestView.pdf",
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
                byte[] BookingPdfData = BookingActionPdf.BuildPdf(ControllerContext);

                #region Save to file System

                string filename = ConfigurationManager.AppSettings["File.BookingFiles"];
                string comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();
                filename = string.Format(filename, comname);
                filename += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + Model.Booking.BookingId + "_Signed.pdf";
                string Serverfilename = FileHelper.GetFileFullPath(filename);
                //Session[SessionKeys. BookingPdfSession] = filename;
                FileHelper.SaveFile(BookingPdfData, Serverfilename);


                #endregion

                #region Save CustomerFile
                _Util.Facade.BookingFacade.SaveBookingPdfFile(filename, Model.Booking.BookingId, CustomerId, CompanyId, true);
                #endregion

                #endregion

                #region Signed Up Confirmation SMS to Customer
                if (tempCustomer.PreferedSms.HasValue && tempCustomer.PreferedSms.Value)
                {
                    List<string> ReceiverNumberList = new List<string>();
                    string SMSText = "";
                    string ReceiverNumber = "";
                    SMSText = string.Format("Thank you! You have successfully signed up in our booking {0}.", BookingId.GenerateBookingNo());

                    #region ReceiverNumber Setup
                    if (!string.IsNullOrWhiteSpace(tempCustomer.PrimaryPhone))
                    {
                        ReceiverNumber = tempCustomer.PrimaryPhone;
                    }
                    else if (!string.IsNullOrWhiteSpace(tempCustomer.SecondaryPhone))
                    {
                        ReceiverNumber = tempCustomer.SecondaryPhone.Replace("-", "");
                    }
                    ReceiverNumberList.Add(ReceiverNumber);
                    #endregion
                    if (!string.IsNullOrWhiteSpace(ReceiverNumber))
                    {
                        result = _Util.Facade.SMSFacade.SendSMS(CompanyId, Guid.Empty, SMSText, ReceiverNumberList, true, "System");
                    }
                    //result = SMSManager.SendASms(ReceiverNumberList, SenderNumber, SMSText, AuthId, AuthToken);
                }
                #endregion

                #region Send email to sales person
                if (!string.IsNullOrWhiteSpace(tempCustomer.Soldby))
                {
                    Guid EmployeeId;
                    BookingSignNotificationEmail email = new BookingSignNotificationEmail
                    {
                        CompanyId = CompanyId,
                        CustomerName = string.IsNullOrWhiteSpace(tempCustomer.BusinessName) ? string.Concat(tempCustomer.FirstName, " ", tempCustomer.LastName) : tempCustomer.BusinessName,

                        BookingNO = BookingId.GenerateBookingNo(),
                        TotalAmount = book.TotalAmount.HasValue ? book.TotalAmount.Value : 0.0,
                        CompanyName = tempComp.CompanyName,

                    };
                    if (tempCustomer.Soldby.IsValidEmailAddress())
                    {
                        Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUsername(tempCustomer.Soldby);
                        if (emp != null)
                        {
                            email.SalesPersonsName = string.Concat(emp.FirstName, " ", emp.LastName);
                        }
                        email.ToEmail = tempCustomer.Soldby;
                        email.EmailTo = "SalesPerson";
                        _Util.Facade.MailFacade.BookingSignNotificationEmail(email);
                    }
                    else if (Guid.TryParse(tempCustomer.Soldby, out EmployeeId) && EmployeeId != new Guid())
                    {
                        Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(EmployeeId);

                        if (emp != null)
                        {
                            email.SalesPersonsName = string.Concat(emp.FirstName, " ", emp.LastName);

                            if (!string.IsNullOrWhiteSpace(emp.Email) && emp.Email.IsValidEmailAddress())
                            {
                                email.ToEmail = emp.Email;
                                email.EmailTo = "SalesPerson";
                                _Util.Facade.MailFacade.BookingSignNotificationEmail(email);
                            }
                            else if (!string.IsNullOrWhiteSpace(emp.UserName) && emp.UserName.IsValidEmailAddress())
                            {
                                email.ToEmail = emp.UserName;
                                email.EmailTo = "SalesPerson";
                                _Util.Facade.MailFacade.BookingSignNotificationEmail(email);
                            }
                        }
                    }
                }
                #endregion

                _Util.Facade.TicketFacade.CreateTicketsForApprovingBooking(LabelHelper.TicketStatus.Created,
                   book,
                   //tempCustomer.CustomerId,
                   tempCustomer.Id,
                   //cc.CompanyId,
                   new Guid("22222222-2222-2222-2222-222222222222"),
                   "System",
                   //book.BookingId, //book.PickUpDate.HasValue ? book.PickUpDate.Value : DateTime.Now.UTCCurrentTime().AddDays(1), //book.PickUpLocation, //book.DropOffLocation,
                   LabelHelper.TicketType.PickUp,
                   LabelHelper.TicketType.Service,
                   LabelHelper.TicketType.DropOff,
                   LabelHelper.UserTags.Admin,
                   LabelHelper.UserTags.HRManager,
                   LabelHelper.NotificationType.Customer
               );

                #region Notification 
                /*
                #region Insert notification 
                Notification notification = new Notification()
                {
                    CompanyId = book.CompanyId,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    NotificationId = Guid.NewGuid(),
                    Type = LabelHelper.NotificationType.Customer,
                    Who = book.CustomerId,
                    What = string.Format(@"Customer <a class=""cus-anchor"" href=""{3}/Customer/Customerdetail/?id={1}"">{0}</a> has signed the booking  
                                    <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenBkById('{2}')"">{2}</a>", "{0}", tempCustomer.Id, book.BookingId, AppConfig.DomainSitePath),
                    NotificationUrl = AppConfig.DomainSitePath + "/Customer/Customerdetail/?id=" + tempCustomer.Id + "#BookingTab"
                };

                _Util.Facade.NotificationFacade.InsertNotification(notification);

                #endregion

                #region set user to notification 

                List<Guid> UserList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(book.CompanyId, LabelHelper.UserTags.Admin, new Guid()).Select(x => x.UserId).ToList();
                UserList.AddRange(_Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(book.CompanyId, LabelHelper.UserTags.HRManager, new Guid()).Select(x => x.UserId).ToList());
                UserList = UserList.GroupBy(x => x).Select(x => x.Key).ToList();

                foreach (var item in UserList)
                {
                    NotificationUser nu = new NotificationUser()
                    {
                        NotificationId = notification.NotificationId,
                        IsRead = false,
                        NotificationPerson = item,
                    };
                    _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
                }

                #endregion 
                */
                #endregion

            }
            else
            {
                if (string.IsNullOrWhiteSpace(DeclinedReason))
                {
                    return Json(new { result = true, IsReload = false, message = "Please give us a decline reason." });
                }
                Booking objbook = _Util.Facade.BookingFacade.GetBookingById(BookingId);

                if (objbook != null)
                {
                    objbook.Status = LabelHelper.BookingStatus.Declined;
                    objbook.CancelReason = DeclinedReason;
                    _Util.Facade.BookingFacade.UpdateBooking(objbook);
                }
                #region Declined Email
                Guid EmployeeId;
                Customer tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
                Company tempComp = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
                BookingSignNotificationEmail email = new BookingSignNotificationEmail
                {
                    CompanyId = CompanyId,
                    CustomerName = string.IsNullOrWhiteSpace(tempCustomer.BusinessName) ? string.Concat(tempCustomer.FirstName, " ", tempCustomer.LastName) : tempCustomer.BusinessName,

                    BookingNO = BookingId.GenerateBookingNo(),
                    TotalAmount = objbook.TotalAmount.HasValue ? objbook.TotalAmount.Value : 0.0,
                    CompanyName = tempComp.CompanyName,
                };

                if (tempCustomer.Soldby.IsValidEmailAddress())
                {
                    Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUsername(tempCustomer.Soldby);
                    if (emp != null)
                    {
                        email.SalesPersonsName = string.Concat(emp.FirstName, " ", emp.LastName);
                    }
                    email.ToEmail = tempCustomer.Soldby;
                    email.EmailTo = "DeclinedBooking";
                    email.DeclinationReason = DeclinedReason;
                    _Util.Facade.MailFacade.BookingSignNotificationEmail(email);
                }
                else if (Guid.TryParse(tempCustomer.Soldby, out EmployeeId) && EmployeeId != new Guid())
                {
                    Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(EmployeeId);

                    if (emp != null)
                    {
                        email.SalesPersonsName = string.Concat(emp.FirstName, " ", emp.LastName);

                        if (!string.IsNullOrWhiteSpace(emp.Email) && emp.Email.IsValidEmailAddress())
                        {
                            email.ToEmail = emp.Email;
                            email.EmailTo = "DeclinedBooking";
                            email.DeclinationReason = DeclinedReason;
                            _Util.Facade.MailFacade.BookingSignNotificationEmail(email);
                        }
                        else if (!string.IsNullOrWhiteSpace(emp.UserName) && emp.UserName.IsValidEmailAddress())
                        {
                            email.ToEmail = emp.UserName;
                            email.EmailTo = "DeclinedBooking";
                            email.DeclinationReason = DeclinedReason;
                            _Util.Facade.MailFacade.BookingSignNotificationEmail(email);
                        }
                    }
                }
                #endregion
            }
            bool IsReload = false;
            string Message = "Your booking has been signed and sent to our office. You will also receive a confirmation about your signed booking!";
            if (!status)
            {
                Message = "You have successfully declined your booking.";
                IsReload = true;
            }
            return Json(new { result = true, IsReload = IsReload, message = Message });
        }

        public JsonResult LeadsBookingAgree(string code, bool status, string DeclinedReason)
        {
            bool result = false;
            if (string.IsNullOrWhiteSpace(code))
            {
                return Json(new { result = false, message = "Access denied." });
            }
            string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(code).Split('#');
            Guid CustomerId = new Guid(Decryptval[0]);
            Guid CompanyId = new Guid(Decryptval[1]);
            int BookingId = Convert.ToInt32(Decryptval[2]);
            if (status == true)
            {
                Booking book = _Util.Facade.BookingFacade.GetBookingById(BookingId);
                if (book != null)
                {
                    book.Status = LabelHelper.BookingStatus.Approved;
                    book.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    book.LastUpdatedBy = new Guid("22222222-2222-2222-2222-222222222222");
                    _Util.Facade.BookingFacade.UpdateBooking(book);
                }

                #region Make lead customer
                CustomerCompany cc = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerIdAndCompanyId(CustomerId, CompanyId);
                Customer tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
                if (cc == null || tempCustomer == null)
                {
                    return Json(new { result = false, message = "Customer not found." });
                }
                //Signing up booking will convert lead to customer //decision changed 8/26/19
                else if (cc.IsLead)
                {
                    cc.IsLead = false;
                    cc.ConvertionDate = DateTime.Now.UTCCurrentTime();
                    cc.ConvertionType = LabelHelper.LeadConvertionType.System;
                    _Util.Facade.CustomerFacade.UpdateCustomerCompany(cc);
                }
                #endregion

                List<BookingDetails> bookDetList = _Util.Facade.BookingFacade.GetBookingDetialsListByBookingId(book.BookingId);


                Company tempComp = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);

                #region Make the Booking_Signed.pdf file
                var Model = GetCreateBookingModel(book, bookDetList, tempComp, tempCustomer);
                Model.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CompanyId);
                Model.Booking.BookingId = Model.Booking.Id.GenerateBookingNo();


                #region Insert Customer Agreement
                //Model.Booking.CustomerAgreement
                CustomerAgreement CustomerAgreement = new CustomerAgreement()
                {
                    CompanyId = CompanyId,
                    CustomerId = CustomerId,
                    IP = AppConfig.GetIP,
                    UserAgent = AppConfig.GetUserAgent,
                    Type = LabelHelper.CustomerAgreementLog.SubmitBooking,
                    InvoiceId = book.BookingId,
                    AddedDate = DateTime.Now.UTCCurrentTime(),
                };
                //Note -> Need to Booking Id in the CustomerAgreement Table For Log 
                _Util.Facade.CustomerAgreementFacade.InsertCustomerAgreement(CustomerAgreement);
                #endregion
                //Note -> Need to Booking Id in the CustomerAgreement Table For Log 
                //Model.Booking.CustomerAgreement = _Util.Facade.CustomerAgreementFacade.GetCustomerAgreementByCompanyIdAndCustomerIdAndInvoiceId(CompanyId, CustomerId, InvoiceId.GenerateEstimateNo());

                ViewAsPdf BookingActionPdf = new Rotativa.ViewAsPdf("~/Views/Booking/BookingPdf.cshtml", Model)
                {
                    //FileName = "TestView.pdf",
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
                byte[] BookingPdfData = BookingActionPdf.BuildPdf(ControllerContext);

                #region Save to file 

                //string filename = ConfigurationManager.AppSettings["File.BookingFiles"];
                //string comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();
                //filename = string.Format(filename, comname);
                //filename += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + Model.Booking.BookingId + "_Signed.pdf";
                //string Serverfilename = FileHelper.GetFileFullPath(filename);
                ////Session[SessionKeys. BookingPdfSession] = filename;
                //FileHelper.SaveFile(BookingPdfData, Serverfilename);


                //// "mayur" AWS S3 Changes //// Start

                #region File Save on AWS S3

                Random rand = new Random();
                string filename = ConfigurationManager.AppSettings["File.BookingFiles"];
                filename = filename.TrimEnd('/');

                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.ReplaceSpecialChar();

                string FilePath = string.Format(filename, comname);
                FilePath += "/" + DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString();
                string FileName = Model.Booking.BookingId + "_Signed.pdf";

                string FileKey = string.Format($"{FilePath}/{FileName}");

                var returnurl = "";

                var task = Task.Run(async () =>
                {
                    AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                    await AWSobject.UploadFile(FileKey, BookingPdfData);
                    await AWSobject.MakePublic(FileName, FilePath);
                });

                task.Wait();

                /// "mayur" used thread for async s3 methods : start

                //Thread thread = new Thread(async () => {

                //    AWSS3ObjectService AWSobject = new AWSS3ObjectService();

                //    await AWSobject.UploadFile(FileKey, BookingPdfData);
                //    await AWSobject.MakePublic(FileName, FilePath);

                //});
                //thread.Start();

                /// "mayur" used thread for async s3 methods : End



                returnurl = String.Format(AppConfig.AWSS3Url, AppConfig.AWSS3BucketName);
                returnurl = returnurl + FileKey;


                ViewBag.ReturnUrl = returnurl;
                ViewBag.FileName = FileName;
                ViewBag.FileKey = FileKey;

                #endregion

                //// "mayur" AWS S3 Changes //// End


                #endregion

                #region Save CustomerFile
                _Util.Facade.BookingFacade.SaveBookingPdfFile(FileKey, Model.Booking.BookingId, CustomerId, CompanyId, true);
                #endregion

                #endregion

                #region Signed Up Confirmation SMS to Customer
                if (tempCustomer.PreferedSms.HasValue && tempCustomer.PreferedSms.Value)
                {
                    List<string> ReceiverNumberList = new List<string>();
                    string SMSText = "";
                    string ReceiverNumber = "";
                    SMSText = string.Format("Thank you! You have successfully signed up in our booking {0}.", BookingId.GenerateBookingNo());

                    #region ReceiverNumber Setup
                    if (!string.IsNullOrWhiteSpace(tempCustomer.PrimaryPhone))
                    {
                        ReceiverNumber = tempCustomer.PrimaryPhone;
                    }
                    else if (!string.IsNullOrWhiteSpace(tempCustomer.SecondaryPhone))
                    {
                        ReceiverNumber = tempCustomer.SecondaryPhone.Replace("-", "");
                    }
                    ReceiverNumberList.Add(ReceiverNumber);
                    #endregion
                    if (!string.IsNullOrWhiteSpace(ReceiverNumber))
                    {
                        result = _Util.Facade.SMSFacade.SendSMS(CompanyId, Guid.Empty, SMSText, ReceiverNumberList, true, "System");
                    }
                    //result = SMSManager.SendASms(ReceiverNumberList, SenderNumber, SMSText, AuthId, AuthToken);
                }
                #endregion

                #region Send email to sales person
                if (!string.IsNullOrWhiteSpace(tempCustomer.Soldby))
                {
                    Guid EmployeeId;
                    BookingSignNotificationEmail email = new BookingSignNotificationEmail
                    {
                        CompanyId = CompanyId,
                        CustomerName = string.IsNullOrWhiteSpace(tempCustomer.BusinessName) ? string.Concat(tempCustomer.FirstName, " ", tempCustomer.LastName) : tempCustomer.BusinessName,

                        BookingNO = BookingId.GenerateBookingNo(),
                        TotalAmount = book.TotalAmount.HasValue ? book.TotalAmount.Value : 0.0,
                        CompanyName = tempComp.CompanyName,

                    };
                    if (tempCustomer.Soldby.IsValidEmailAddress())
                    {
                        Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUsername(tempCustomer.Soldby);
                        if (emp != null)
                        {
                            email.SalesPersonsName = string.Concat(emp.FirstName, " ", emp.LastName);
                        }
                        email.ToEmail = tempCustomer.Soldby;
                        email.EmailTo = "SalesPerson";
                        _Util.Facade.MailFacade.BookingSignNotificationEmail(email);
                    }
                    else if (Guid.TryParse(tempCustomer.Soldby, out EmployeeId) && EmployeeId != new Guid())
                    {
                        Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(EmployeeId);

                        if (emp != null)
                        {
                            email.SalesPersonsName = string.Concat(emp.FirstName, " ", emp.LastName);

                            if (!string.IsNullOrWhiteSpace(emp.Email) && emp.Email.IsValidEmailAddress())
                            {
                                email.ToEmail = emp.Email;
                                email.EmailTo = "SalesPerson";
                                _Util.Facade.MailFacade.BookingSignNotificationEmail(email);
                            }
                            else if (!string.IsNullOrWhiteSpace(emp.UserName) && emp.UserName.IsValidEmailAddress())
                            {
                                email.ToEmail = emp.UserName;
                                email.EmailTo = "SalesPerson";
                                _Util.Facade.MailFacade.BookingSignNotificationEmail(email);
                            }
                        }
                    }
                }
                #endregion

                _Util.Facade.TicketFacade.CreateTicketsForApprovingBooking(LabelHelper.TicketStatus.Created,
                   book,
                   //tempCustomer.CustomerId,
                   tempCustomer.Id,
                   //cc.CompanyId,
                   new Guid("22222222-2222-2222-2222-222222222222"),
                   "System",
                   //book.BookingId, //book.PickUpDate.HasValue ? book.PickUpDate.Value : DateTime.Now.UTCCurrentTime().AddDays(1), //book.PickUpLocation, //book.DropOffLocation,
                   LabelHelper.TicketType.PickUp,
                   LabelHelper.TicketType.Service,
                   LabelHelper.TicketType.DropOff,
                   LabelHelper.UserTags.Admin,
                   LabelHelper.UserTags.HRManager,
                   LabelHelper.NotificationType.Customer
               );

                #region Notification 
                /*
                #region Insert notification 
                Notification notification = new Notification()
                {
                    CompanyId = book.CompanyId,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    NotificationId = Guid.NewGuid(),
                    Type = LabelHelper.NotificationType.Customer,
                    Who = book.CustomerId,
                    What = string.Format(@"Customer <a class=""cus-anchor"" href=""{3}/Customer/Customerdetail/?id={1}"">{0}</a> has signed the booking  
                                    <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenBkById('{2}')"">{2}</a>", "{0}", tempCustomer.Id, book.BookingId, AppConfig.DomainSitePath),
                    NotificationUrl = AppConfig.DomainSitePath + "/Customer/Customerdetail/?id=" + tempCustomer.Id + "#BookingTab"
                };

                _Util.Facade.NotificationFacade.InsertNotification(notification);

                #endregion

                #region set user to notification 

                List<Guid> UserList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(book.CompanyId, LabelHelper.UserTags.Admin, new Guid()).Select(x => x.UserId).ToList();
                UserList.AddRange(_Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(book.CompanyId, LabelHelper.UserTags.HRManager, new Guid()).Select(x => x.UserId).ToList());
                UserList = UserList.GroupBy(x => x).Select(x => x.Key).ToList();

                foreach (var item in UserList)
                {
                    NotificationUser nu = new NotificationUser()
                    {
                        NotificationId = notification.NotificationId,
                        IsRead = false,
                        NotificationPerson = item,
                    };
                    _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
                }

                #endregion 
                */
                #endregion

            }
            else
            {
                if (string.IsNullOrWhiteSpace(DeclinedReason))
                {
                    return Json(new { result = true, IsReload = false, message = "Please give us a decline reason." });
                }
                Booking objbook = _Util.Facade.BookingFacade.GetBookingById(BookingId);

                if (objbook != null)
                {
                    objbook.Status = LabelHelper.BookingStatus.Declined;
                    objbook.CancelReason = DeclinedReason;
                    _Util.Facade.BookingFacade.UpdateBooking(objbook);
                }
                #region Declined Email
                Guid EmployeeId;
                Customer tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
                Company tempComp = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
                BookingSignNotificationEmail email = new BookingSignNotificationEmail
                {
                    CompanyId = CompanyId,
                    CustomerName = string.IsNullOrWhiteSpace(tempCustomer.BusinessName) ? string.Concat(tempCustomer.FirstName, " ", tempCustomer.LastName) : tempCustomer.BusinessName,

                    BookingNO = BookingId.GenerateBookingNo(),
                    TotalAmount = objbook.TotalAmount.HasValue ? objbook.TotalAmount.Value : 0.0,
                    CompanyName = tempComp.CompanyName,
                };

                if (tempCustomer.Soldby.IsValidEmailAddress())
                {
                    Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUsername(tempCustomer.Soldby);
                    if (emp != null)
                    {
                        email.SalesPersonsName = string.Concat(emp.FirstName, " ", emp.LastName);
                    }
                    email.ToEmail = tempCustomer.Soldby;
                    email.EmailTo = "DeclinedBooking";
                    email.DeclinationReason = DeclinedReason;
                    _Util.Facade.MailFacade.BookingSignNotificationEmail(email);
                }
                else if (Guid.TryParse(tempCustomer.Soldby, out EmployeeId) && EmployeeId != new Guid())
                {
                    Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(EmployeeId);

                    if (emp != null)
                    {
                        email.SalesPersonsName = string.Concat(emp.FirstName, " ", emp.LastName);

                        if (!string.IsNullOrWhiteSpace(emp.Email) && emp.Email.IsValidEmailAddress())
                        {
                            email.ToEmail = emp.Email;
                            email.EmailTo = "DeclinedBooking";
                            email.DeclinationReason = DeclinedReason;
                            _Util.Facade.MailFacade.BookingSignNotificationEmail(email);
                        }
                        else if (!string.IsNullOrWhiteSpace(emp.UserName) && emp.UserName.IsValidEmailAddress())
                        {
                            email.ToEmail = emp.UserName;
                            email.EmailTo = "DeclinedBooking";
                            email.DeclinationReason = DeclinedReason;
                            _Util.Facade.MailFacade.BookingSignNotificationEmail(email);
                        }
                    }
                }
                #endregion
            }
            bool IsReload = false;
            string Message = "Your booking has been signed and sent to our office. You will also receive a confirmation about your signed booking!";
            if (!status)
            {
                Message = "You have successfully declined your booking.";
                IsReload = true;
            }
            return Json(new { result = true, IsReload = IsReload, message = Message });
        }

        //Get Create Booking Model 
        private CreateBooking GetCreateBookingModel(Booking Booking, List<BookingDetails> BookingDetialList, Company tempCom, Customer tempCUstomer)
        {
            CreateBooking Model = new CreateBooking();
            //var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Model.Booking = Booking;
            Model.BookingDetailsList = BookingDetialList;
            Model.BookingExtraItem = _Util.Facade.BookingFacade.GetBookingExtraItemListByBookingId(Model.Booking.Id);
            Model.Booking.CustomerName = tempCUstomer.Title + " " + tempCUstomer.FirstName + " " + tempCUstomer.LastName;

            #region Discount Calculation 
            //if (!string.IsNullOrWhiteSpace(Model.Booking.DiscountType))
            //{
            //    if (Model.Booking.DiscountType == "amount")
            //    {
            //        if (Booking.Discountpercent != null)
            //        {
            //            Model.Discount = Booking.Discountpercent.Value;
            //        }
            //    }
            //    else
            //    {
            //        if (Booking.Discountpercent != null)
            //        {
            //            Model.Discount = ((Booking.Discountpercent / 100) * Model.SubTotal).Value;
            //        }
            //    }
            //}
            #endregion

            #region making Name of Address Bold
            //if (!string.IsNullOrWhiteSpace(Model.Booking.BillingAddress))
            //{
            //    var split = Model.Booking.BillingAddress.Split(new string[] { "\n" }, StringSplitOptions.None);

            //    if (split.Count() > 0)
            //    {
            //        string NewAddress = "";
            //        split[0] = "<b>" + split[0] + "</b>";

            //        foreach (var item in split)
            //        {
            //            NewAddress += item + Environment.NewLine;
            //        }
            //        Model.Booking.BillingAddress = NewAddress;
            //    }
            //}
            //if (!string.IsNullOrWhiteSpace(Model.Booking.ShippingAddress))
            //{
            //    var split = Model.Booking.ShippingAddress.Split(new string[] { "\n" }, StringSplitOptions.None);

            //    if (split.Count() > 0)
            //    {
            //        string NewAddress = "";
            //        split[0] = "<b>" + split[0] + "</b>";

            //        foreach (var item in split)
            //        {
            //            NewAddress += item + Environment.NewLine;
            //        }
            //        Model.Booking.ShippingAddress = NewAddress;
            //    }
            //}
            //if (!string.IsNullOrWhiteSpace(Model.BookingShipping))
            //{
            //    var split = Model.BookingShipping.Split(new string[] { "\n" }, StringSplitOptions.None);

            //    if (split.Count() > 0)
            //    {
            //        string NewAddress = "";
            //        split[0] = "<b>" + split[0] + "</b>";

            //        foreach (var item in split)
            //        {
            //            NewAddress += item + Environment.NewLine;
            //        }
            //        Model.BookingShipping = NewAddress;
            //    }
            //}
            #endregion making Name of Address Bold

            //customer name is customer business name here 
            if (tempCUstomer.BusinessName != "")
            {
                Model.CustomerName = tempCUstomer.BusinessName;
            }
            else
            {
                Model.CustomerName = Model.Booking.CustomerName;
            }
            Model.CusBussinessName = tempCUstomer.BusinessName;

            Model.SubTotal = 0;
            foreach (var item in Model.BookingDetailsList)
            {
                //item.AddedBy = CurrentUser.EmployeeId.Value;
                item.AddedDate = DateTime.Now.UTCCurrentTime();
                item.CompanyId = tempCom.CompanyId;
                Model.SubTotal = Model.SubTotal + item.TotalPrice.Value;
            }
            foreach (var item in Model.BookingExtraItem)
            {
                //item.AddedBy = CurrentUser.EmployeeId.Value;
                Model.SubTotal = Model.SubTotal + item.TotalPrice.Value;
            }
            //if(Model.Booking.Amount.HasValue && Model.Booking.Amount.Value > 0)
            //{
            //    Model.SubTotal = Model.Booking.Amount.Value;
            //}
            Model.CompanyAddress = tempCom.Address;
            Model.CompanyStreet = tempCom.Street;
            string ComCity = "";
            string ComState = "";
            if (!string.IsNullOrWhiteSpace(tempCom.City))
            {
                ComCity = tempCom.City.UppercaseFirst() + ", ";
            }
            if (!string.IsNullOrWhiteSpace(tempCom.State))
            {
                ComState = tempCom.State + " ";
            }
            Model.companyStreetInfo = ComCity + ComState + tempCom.ZipCode;
            Model.CompanyCity = tempCom.City.UppercaseFirst();
            Model.CompanyState = tempCom.State;
            Model.CompanyZip = tempCom.ZipCode;
            Model.CompanyPhone = tempCom.Phone;
            Model.CompanyEmail = tempCom.EmailAdress;
            Model.CompanyName = tempCom.CompanyName;
            Model.PhoneNum = tempCom.Phone;
            Model.CompanyWebsite = tempCom.Website;

            if (string.IsNullOrWhiteSpace(Model.CustomerInfo))
            {
                Model.CustomerInfo = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(tempCom.CompanyId);
            }
            if (string.IsNullOrWhiteSpace(Model.CustomerStreet))
            {
                Model.CustomerStreet = tempCUstomer.Street;
            }
            if (string.IsNullOrWhiteSpace(Model.CustomerCity))
            {
                Model.CustomerCity = tempCUstomer.City.UppercaseFirst();
            }
            if (string.IsNullOrWhiteSpace(Model.CustomerState))
            {
                Model.CustomerState = tempCUstomer.State;
            }
            if (string.IsNullOrWhiteSpace(Model.CustomerZipCode))
            {
                Model.CustomerZipCode = tempCUstomer.ZipCode;
            }
            if (string.IsNullOrWhiteSpace(Model.CustomerNo))
            {
                Model.CustomerNo = tempCUstomer.CustomerNo;
            }

            ViewBag.ShippingValue = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(tempCom.CompanyId);
            if (string.IsNullOrWhiteSpace(tempCom.CompanyLogo))
            {
                tempCom.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(tempCom.CompanyId);
            }
            Model.CompanyLogo = tempCom.CompanyLogo;
            if (string.IsNullOrWhiteSpace(Model.Booking.BookingMessage))
            {
                Model.Booking.BookingMessage = Model.Booking.Message;
            }
            return Model;
        }

        //Customer Signature For Booking 
        [HttpPost]
        public JsonResult UploadCustomerSignatureImageBooking(string data, string token)
        {
            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(data))
            {
                return Json(new { uploadImage = false, message = "Invalid data" });
            }
            string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(token).Split('#');
            Guid CustomerId = new Guid(Decryptval[0]);
            Guid CompanyId = new Guid(Decryptval[1]);
            int BookingId = Convert.ToInt32(Decryptval[2]);
            Customer tmpCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            if (tmpCustomer == null)
            {
                return Json(new { result = false, message = "Customer not found" });
            }
            if (!_Util.Facade.CustomerFacade.CustomerIsInCompany(CustomerId, CompanyId))
            {
                return Json(new { result = false, message = "Invalid user" });
            }
            bool uploadImage = false;
            string filePath = "";
            string[] datasplit = data.Split(',');
            byte[] bytes = Convert.FromBase64String(datasplit[1]);
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
                string tempFolder = ConfigurationManager.AppSettings["File.CustomerSignatureFile"];
                //leadID = _Util.Facade.CustomerFacade.GetLeadIdForCustomerSignatureByCustomerId(new Guid(LeadConvertId)).Id.ToString();
                var SplittempFolderName = tempFolder.Split('/');
                var comname = "/" + _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId).CompanyName.Replace(" ", "_") + "/";
                var FtempFolderName = SplittempFolderName[0] + comname + SplittempFolderName[1] + "/" + tmpCustomer.Id + "Signature";
                Random rand = new Random();
                string FileName = rand.Next().ToString();
                FileName += "-___" + "Signature.png";
                string tempFolderPath = Server.MapPath("~/" + FtempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        image.Save(Path.Combine(tempFolderPath, FileName));
                        uploadImage = true;
                    }
                    catch (Exception)
                    {

                    }
                }
                filePath = string.Concat("/", FtempFolderName, "/", FileName);
            }
            var serverFile = Server.MapPath(filePath);
            bool rsult = _Util.Facade.CustomerFacade.CustomerIsInCompany(tmpCustomer.Id, CompanyId);

            if (!System.IO.File.Exists(serverFile))
            {
                return Json(new { result = false, message = "File not exsists" });
            }
            tmpCustomer.Singature = filePath;
            _Util.Facade.CustomerFacade.UpdateCustomer(tmpCustomer);
            Booking book = _Util.Facade.BookingFacade.GetBookingById(BookingId);
            book.Signature = filePath;
            book.Status = LabelHelper.BookingStatus.Signed;
            _Util.Facade.BookingFacade.UpdateBooking(book);

            #region Insert Customer Agreement
            //Model.Booking.CustomerAgreement
            CustomerAgreement CustomerAgreement = new CustomerAgreement()
            {
                CompanyId = CompanyId,
                CustomerId = CustomerId,
                IP = AppConfig.GetIP,
                UserAgent = AppConfig.GetUserAgent,
                Type = LabelHelper.CustomerAgreementLog.SignBooking,
                InvoiceId = BookingId.GenerateBookingNo(),
                AddedDate = DateTime.Now.UTCCurrentTime(),
            };
            _Util.Facade.CustomerAgreementFacade.InsertCustomerAgreement(CustomerAgreement);
            #endregion
            return Json(new { uploadImage = uploadImage, UploadFilePath = filePath }, "text/html");
        }

        #endregion LeadsBooking

        #region Inventory Requisition Order 
        public ActionResult RequisitionOrder(string code)
        {
            try
            {
                #region Loguout if authenticated
                if (User.Identity.IsAuthenticated)
                {
                    FormsAuthentication.SignOut();
                    SessionHelper hs = new SessionHelper();
                    hs.ClearCurrentSession();
                }
                #endregion

                ViewBag.Token = code;
                string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(code).Split('#');
                Guid CompanyId = new Guid(Decryptval[0]);

                #region SettingUp Company Connection
                CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByCompanyId(CompanyId.ToString());

                if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                else
                {
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
                    Company Com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
                    ViewBag.Title = string.Concat(Com.CompanyName, " | ", Localize.T("Requisition Order"));
                }
                #endregion

                //Invoice inv = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);
                //Customer TempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
                //inv.CustomerName = TempCustomer.FirstName + " " + TempCustomer.LastName;
                //inv.CustomerBussinessName = TempCustomer.BusinessName;
                CompanyBranch cb = _Util.Facade.CompanyBranchFacade.GetMainBranchByCompanyId(CompanyId);
                ViewBag.CompanyLogo = cb.Logo;
                //if (inv == null
                //    || inv.IsEstimate
                //    || CompanyId != inv.CompanyId
                //    || CustomerId != inv.CustomerId
                //    //|| (inv.DueDate.HasValue && inv.DueDate.Value.AddDays(1) < DateTime.Now.UTCCurrentTime())
                //    )
                //{
                //    return PartialView("~/Views/Shared/_LinkExpired.cshtml");
                //}
                System.Web.HttpContext.Current.Session[SessionKeys.CompanyId] = CompanyId;

                //#region Amount Payable calculation
                //inv.PayableAmount = inv.BalanceDue.HasValue ? inv.BalanceDue.Value : 0;
                //if (!string.IsNullOrWhiteSpace(inv.EstimateTerm)
                //    && inv.EstimateTerm == LabelHelper.EstimatePaymentTerms.FiftyFifty
                //    && inv.Status != LabelHelper.InvoiceStatus.Partial)
                //{
                //    inv.PayableAmount = inv.BalanceDue.HasValue ? inv.BalanceDue.Value / 2 : 0;
                //    if (inv.PayableAmount > inv.BalanceDue)
                //    {
                //        inv.PayableAmount = inv.BalanceDue.Value;
                //    }
                //}
                //#endregion

                #region ViewBags
                //ViewBag.ECheckType = _Util.Facade.LookupFacade.GetLookupByKey("ECheckType").Select(x =>
                //           new SelectListItem()
                //           {
                //               Text = x.DisplayText.ToString(),
                //               Value = x.DataValue.ToString()
                //           }).ToList();
                //ViewBag.BankAccountType = _Util.Facade.LookupFacade.GetLookupByKey("BankAccountType").Select(x =>
                //               new SelectListItem()
                //               {
                //                   Text = x.DisplayText.ToString(),
                //                   Value = x.DataValue.ToString()
                //               }).ToList();
                ViewBag.CompanyId = CompanyId.ToString();
                ViewBag.FooterCompanyInformation = _Util.Facade.MailFacade.GetFooterCompanyInformationByCompanyId(CompanyId).Replace("##Year##", string.Format("2017-{0}", DateTime.Now.Year.ToString()));
                //ViewBag.PaymentMethodList = _Util.Facade.LookupFacade.GetLookupByKey("PublicPaymentMethod").ToList();
                #endregion

                //Guid SalesPersonId = new Guid();
                //if (Guid.TryParse(TempCustomer.Soldby, out SalesPersonId) && SalesPersonId != new Guid())
                //{
                //    Notification notification = new Notification();
                //    #region Insert notification
                //    CustomerCompany cusCom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(TempCustomer.Id);
                //    if (cusCom.IsLead == false)
                //    {
                //        notification = new Notification()
                //        {
                //            CompanyId = CompanyId,
                //            CreatedDate = DateTime.Now.UTCCurrentTime(),
                //            NotificationId = Guid.NewGuid(),
                //            Type = LabelHelper.NotificationType.Customer,
                //            Who = TempCustomer.CustomerId,
                //            What = string.Format(@"A customer <a class=""cus-anchor"" href=""{4}/Customer/Customerdetail/?id={1}"">{0}</a> just viewed an invoice  
                //                            <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenInvById('{2}')"">{3}</a>", "{0}", TempCustomer.Id, inv.Id, inv.InvoiceId, AppConfig.DomainSitePath),
                //            NotificationUrl = AppConfig.DomainSitePath + "/Customer/Customerdetail/?id=" + TempCustomer.Id + "#InvoiceTab"

                //        };
                //    }
                //    else
                //    {
                //        notification = new Notification()
                //        {
                //            CompanyId = CompanyId,
                //            CreatedDate = DateTime.Now.UTCCurrentTime(),
                //            NotificationId = Guid.NewGuid(),
                //            Type = LabelHelper.NotificationType.Customer,
                //            Who = TempCustomer.CustomerId,
                //            What = string.Format(@"A Lead <a class=""cus-anchor"" href=""{4}/Customer/Customerdetail/?id={1}"">{0}</a> just viewed an invoice  
                //                            <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenInvById('{2}')"">{3}</a>", "{0}", TempCustomer.Id, inv.Id, inv.InvoiceId, AppConfig.DomainSitePath),
                //            NotificationUrl = AppConfig.DomainSitePath + "/Customer/Customerdetail/?id=" + TempCustomer.Id + "#InvoiceTab"
                //        };
                //    }

                //    _Util.Facade.NotificationFacade.InsertNotification(notification);

                //#endregion
                //#region set user to notification
                //NotificationUser nu = new NotificationUser()
                //{
                //    NotificationId = notification.NotificationId,
                //    IsRead = false,
                //    NotificationPerson = SalesPersonId,
                //};
                //_Util.Facade.NotificationFacade.InsertNotificationUser(nu);
                //#endregion
                //}

                //CustomerAgreement objagree = new CustomerAgreement()
                //{
                //    CompanyId = CompanyId,
                //    CustomerId = CustomerId,
                //    InvoiceId = inv.InvoiceId,
                //    Type = "LoadInvoice",
                //    AddedDate = DateTime.Now.UTCCurrentTime()
                //};
                //_Util.Facade.CustomerAgreementFacade.InsertCustomerAgreement(objagree);
                return View();
            }
            catch (Exception)
            {
                return PartialView("~/Views/Shared/_LinkExpired.cshtml");
            }
        }
        #endregion

        #region ShortUrlHandler
        public ActionResult ShortUrlHandler(string code)
        {
            ShortUrl url = _Util.Facade.ShortUrlFacade.GetSortUrlCode(code);
            if (url == null || string.IsNullOrWhiteSpace(url.Url))
            {
                return PartialView("~/Views/Shared/_NotFound.cshtml");
            }
            else
            {
                return Redirect(url.Url);
            }
        }
        #endregion

        #region Public Invoice
        public ActionResult CustomerInvoice(string code)
        {
            try
            {
                #region Loguout if authenticated
                if (User.Identity.IsAuthenticated)
                {
                    FormsAuthentication.SignOut();
                    SessionHelper hs = new SessionHelper();
                    hs.ClearCurrentSession();
                }
                #endregion

                ViewBag.Token = code;
                string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(code).Split('#');
                Guid CustomerId = new Guid(Decryptval[2]);
                Guid CompanyId = new Guid(Decryptval[1]);
                int InvoiceId = Convert.ToInt32(Decryptval[0]);

                #region SettingUp Company Connection
                CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByCompanyId(CompanyId.ToString());

                if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                else
                {
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
                    Company Com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
                    ViewBag.Title = string.Concat(Com.CompanyName, " | ", Localize.T("Customer Invoice"));
                }
                #endregion

                Invoice inv = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);
                Customer TempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
                inv.CustomerName = TempCustomer.FirstName + " " + TempCustomer.LastName;
                inv.CustomerBussinessName = TempCustomer.BusinessName;
                CompanyBranch cb = _Util.Facade.CompanyBranchFacade.GetMainBranchByCompanyId(CompanyId);
                ViewBag.CompanyLogo = cb.Logo;
                if (inv == null
                    || inv.IsEstimate
                    || CompanyId != inv.CompanyId
                    || CustomerId != inv.CustomerId
                    //|| (inv.DueDate.HasValue && inv.DueDate.Value.AddDays(1) < DateTime.Now.UTCCurrentTime())
                    )
                {
                    return PartialView("~/Views/Shared/_LinkExpired.cshtml");
                }
                System.Web.HttpContext.Current.Session[SessionKeys.CompanyId] = CompanyId;

                #region Amount Payable calculation
                inv.PayableAmount = inv.BalanceDue.HasValue ? inv.BalanceDue.Value : 0;
                if (!string.IsNullOrWhiteSpace(inv.EstimateTerm)
                    && inv.EstimateTerm == LabelHelper.EstimatePaymentTerms.FiftyFifty
                    && inv.Status != LabelHelper.InvoiceStatus.Partial)
                {
                    inv.PayableAmount = inv.BalanceDue.HasValue ? inv.BalanceDue.Value / 2 : 0;
                    if (inv.PayableAmount > inv.BalanceDue)
                    {
                        inv.PayableAmount = inv.BalanceDue.Value;
                    }
                }
                #endregion

                #region ViewBags
                ViewBag.ECheckType = _Util.Facade.LookupFacade.GetLookupByKey("ECheckType").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();
                ViewBag.BankAccountType = _Util.Facade.LookupFacade.GetLookupByKey("BankAccountType").Select(x =>
                               new SelectListItem()
                               {
                                   Text = x.DisplayText.ToString(),
                                   Value = x.DataValue.ToString()
                               }).ToList();
                ViewBag.CompanyId = CompanyId.ToString();
                ViewBag.FooterCompanyInformation = _Util.Facade.MailFacade.GetFooterCompanyInformationByCompanyId(CompanyId).Replace("##Year##", string.Format("2017-{0}", DateTime.Now.Year.ToString()));
                ViewBag.PaymentMethodList = _Util.Facade.LookupFacade.GetLookupByKey("PublicPaymentMethod").ToList();
                #endregion

                Guid SalesPersonId = new Guid();
                if (Guid.TryParse(TempCustomer.Soldby, out SalesPersonId) && SalesPersonId != new Guid())
                {
                    Notification notification = new Notification();
                    #region Insert notification
                    CustomerCompany cusCom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(TempCustomer.Id);
                    if (cusCom.IsLead == false)
                    {
                        notification = new Notification()
                        {
                            CompanyId = CompanyId,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            NotificationId = Guid.NewGuid(),
                            Type = LabelHelper.NotificationType.Customer,
                            Who = TempCustomer.CustomerId,
                            What = string.Format(@"A customer <a class=""cus-anchor"" href=""{4}/Customer/Customerdetail/?id={1}"">{0}</a> just viewed an invoice  
                                            <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenInvById('{2}')"">{3}</a>", "{0}", TempCustomer.Id, inv.Id, inv.InvoiceId, AppConfig.DomainSitePath),
                            NotificationUrl = AppConfig.DomainSitePath + "/Customer/Customerdetail/?id=" + TempCustomer.Id + "#InvoiceTab"

                        };
                    }
                    else
                    {
                        notification = new Notification()
                        {
                            CompanyId = CompanyId,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            NotificationId = Guid.NewGuid(),
                            Type = LabelHelper.NotificationType.Customer,
                            Who = TempCustomer.CustomerId,
                            What = string.Format(@"A Lead <a class=""cus-anchor"" href=""{4}/Customer/Customerdetail/?id={1}"">{0}</a> just viewed an invoice  
                                            <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenInvById('{2}')"">{3}</a>", "{0}", TempCustomer.Id, inv.Id, inv.InvoiceId, AppConfig.DomainSitePath),
                            NotificationUrl = AppConfig.DomainSitePath + "/Customer/Customerdetail/?id=" + TempCustomer.Id + "#InvoiceTab"
                        };
                    }

                    _Util.Facade.NotificationFacade.InsertNotification(notification);

                    #endregion
                    #region set user to notification
                    NotificationUser nu = new NotificationUser()
                    {
                        NotificationId = notification.NotificationId,
                        IsRead = false,
                        NotificationPerson = SalesPersonId,
                    };
                    _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
                    #endregion
                }

                CustomerAgreement objagree = new CustomerAgreement()
                {
                    CompanyId = CompanyId,
                    CustomerId = CustomerId,
                    InvoiceId = inv.InvoiceId,
                    Type = "LoadInvoice",
                    AddedDate = DateTime.Now.UTCCurrentTime()
                };
                _Util.Facade.CustomerAgreementFacade.InsertCustomerAgreement(objagree);
                return View(inv);
            }
            catch (Exception)
            {
                return PartialView("~/Views/Shared/_LinkExpired.cshtml");
            }
        }

        public ActionResult CustomerRefer(string code)
        {

            #region Loguout if authenticated
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                SessionHelper hs = new SessionHelper();
                hs.ClearCurrentSession();
            }
            #endregion

            ViewBag.Token = code;
            string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(code).Split('#');
            Guid CustomerId = new Guid(Decryptval[2]);
            Guid CompanyId = new Guid(Decryptval[1]);
            int Id = Convert.ToInt32(Decryptval[0]);

            #region SettingUp Company Connection
            CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByCompanyId(CompanyId.ToString());

            if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            else
            {
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
                Company Com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
                ViewBag.Title = string.Concat(Com.CompanyName, " | ", Localize.T("Customer Invoice"));
            }
            #endregion


            CustomerRefer cus = _Util.Facade.CustomerDraftFacade.GetCustomerReferById(Id);
            cus.IsViewd = true;
            _Util.Facade.CustomerDraftFacade.UpdateCustomerRefer(cus);



            return RedirectToAction("Index", "Home");

        }
        public ActionResult CustomerInvoicePdf(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            try
            {
                string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(code).Split('#');
                Guid CusotmerId = new Guid(Decryptval[2]);
                Guid CompanyId = new Guid(Decryptval[1]);
                int InvoiceId = Convert.ToInt32(Decryptval[0]);

                if (Session[SessionKeys.CompanyConnectionString] == null || string.IsNullOrWhiteSpace(Session[SessionKeys.CompanyConnectionString].ToString()))
                {
                    CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByCompanyId(CompanyId.ToString());

                    if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
                    {
                        return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                    }
                    else
                    {
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
                    }
                }

                Company Com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
                ViewBag.ComName = Com.CompanyName;
                CreateInvoice Model;
                Invoice Inv = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);
                if (Inv.IsEstimate
                    || CompanyId != Inv.CompanyId
                    || Inv.CustomerId != CusotmerId
                    //|| Inv.Status == LabelHelper.InvoiceStatus.Paid
                    //|| Inv.Status == LabelHelper.InvoiceStatus.Cancelled
                    //|| Inv.Status == LabelHelper.InvoiceStatus.Declined
                    )
                {
                    return PartialView("~/Views/Shared/_LinkExpired.cshtml");
                }
                List<InvoiceDetail> InvDetailList = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(Inv.InvoiceId);
                Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Inv.CompanyId);
                Customer tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CusotmerId);
                Model = GetCreateInvoiceModel(Inv, InvDetailList, tempCom, tempCustomer);
                Model.Invoice.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CompanyId);
                if (Model.Invoice.BalanceDue > 0)
                {
                    Model.AmountInWord = NumberToWords(Model.Invoice.BalanceDue.Value); //NumberToWords(Model.Invoice.TotalAmount.Value);
                }
                //Model.AmountInWord = NumberToWords(Model.Invoice.TotalAmount.Value);
                Model.InvoiceSetting = new InvoiceSetting();
                string settingskey = @" 'InvoiceSettingsShipping', 'EstimateServiceSetting', 'InvoiceSettingsDiscount', 'InvoiceSettingsDeposit'";
                List<GlobalSetting> printsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceSettingListByCompanyIdAndKey(settingskey, CompanyId);
                foreach (var print in printsetting)
                {
                    if (print.Value.ToLower() == "true")
                    {
                        if (print.SearchKey == "InvoiceSettingsDeposit")
                        {
                            Model.InvoiceSetting.DepositSetting = true;
                        }
                        if (print.SearchKey == "EstimateServiceSetting")
                        {
                            Model.InvoiceSetting.ServiceSetting = true;
                        }
                        if (print.SearchKey == "InvoiceSettingsDiscount")
                        {
                            Model.InvoiceSetting.DiscountSetting = true;
                        }
                        if (print.SearchKey == "InvoiceSettingsShipping")
                        {
                            Model.InvoiceSetting.ShippingSetting = true;
                        }
                    }
                }
                InvoicePaymentDate PayDate = _Util.Facade.TransactionFacade.GetLatestPaymentDateByInvoiceId(Model.Invoice.Id);
                if (PayDate != null)
                {
                    Model.Invoice.TransacationDate = PayDate.PaymentDate;
                }
                List<CreateInvoice> ModelList = new List<CreateInvoice>();
                ModelList.Add(Model);
                ViewBag.CompanyId = tempCom.CompanyId.ToString();

                return new Rotativa.ViewAsPdf("~/Views/Invoice/InvoicePdf.cshtml", ModelList)
                {
                    //FileName = "TestView.pdf",
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },

                };
            }
            catch (Exception)
            {
                return PartialView("~/Views/Shared/_Loading.cshtml");
            }
        }
        public ActionResult CustomerInvoiceHtml(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            try
            {
                string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(code).Split('#');
                Guid CusotmerId = new Guid(Decryptval[2]);
                Guid CompanyId = new Guid(Decryptval[1]);
                int InvoiceId = Convert.ToInt32(Decryptval[0]);

                if (Session[SessionKeys.CompanyConnectionString] == null || string.IsNullOrWhiteSpace(Session[SessionKeys.CompanyConnectionString].ToString()))
                {
                    CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByCompanyId(CompanyId.ToString());

                    if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
                    {
                        return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                    }
                    else
                    {
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
                    }
                }

                Company Com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
                ViewBag.ComName = Com.CompanyName;
                CreateInvoice Model;
                Invoice Inv = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);
                if (Inv.IsEstimate
                    || CompanyId != Inv.CompanyId
                    || Inv.CustomerId != CusotmerId
                    //|| Inv.Status == LabelHelper.InvoiceStatus.Paid
                    //|| Inv.Status == LabelHelper.InvoiceStatus.Cancelled
                    //|| Inv.Status == LabelHelper.InvoiceStatus.Declined
                    )
                {
                    return PartialView("~/Views/Shared/_LinkExpired.cshtml");
                }
                List<InvoiceDetail> InvDetailList = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(Inv.InvoiceId);
                Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Inv.CompanyId);
                Customer tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CusotmerId);
                Model = GetCreateInvoiceModel(Inv, InvDetailList, tempCom, tempCustomer);
                Model.Invoice.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CompanyId);
                if (Model.Invoice.BalanceDue > 0)
                {
                    Model.AmountInWord = NumberToWords(Model.Invoice.BalanceDue.Value); //NumberToWords(Model.Invoice.TotalAmount.Value);
                }
                //Model.AmountInWord = NumberToWords(Model.Invoice.TotalAmount.Value);
                Model.InvoiceSetting = new InvoiceSetting();
                string settingskey = @" 'InvoiceSettingsShipping', 'EstimateServiceSetting', 'InvoiceSettingsDiscount', 'InvoiceSettingsDeposit'";
                List<GlobalSetting> printsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceSettingListByCompanyIdAndKey(settingskey, CompanyId);
                foreach (var print in printsetting)
                {
                    if (print.Value.ToLower() == "true")
                    {
                        if (print.SearchKey == "InvoiceSettingsDeposit")
                        {
                            Model.InvoiceSetting.DepositSetting = true;
                        }
                        if (print.SearchKey == "EstimateServiceSetting")
                        {
                            Model.InvoiceSetting.ServiceSetting = true;
                        }
                        if (print.SearchKey == "InvoiceSettingsDiscount")
                        {
                            Model.InvoiceSetting.DiscountSetting = true;
                        }
                        if (print.SearchKey == "InvoiceSettingsShipping")
                        {
                            Model.InvoiceSetting.ShippingSetting = true;
                        }
                    }
                }
                InvoicePaymentDate PayDate = _Util.Facade.TransactionFacade.GetLatestPaymentDateByInvoiceId(Model.Invoice.Id);
                if (PayDate != null)
                {
                    Model.Invoice.TransacationDate = PayDate.PaymentDate;
                }
                List<CreateInvoice> ModelList = new List<CreateInvoice>();
                ModelList.Add(Model);
                ViewBag.CompanyId = tempCom.CompanyId.ToString();

                return View("~/Views/Invoice/InvoicePdf.cshtml", ModelList);
            }
            catch (Exception)
            {
                return PartialView("~/Views/Shared/_Loading.cshtml");
            }
        }
        [HttpPost]
        public JsonResult CustomerInvoicePayment(string token, string PaymentMethod, ACHInfo ACHInfo, CardInfo CardInfo, string PaymentTransId)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return Json(new { result = false, message = "Invalid Token" });
            }
            if (string.IsNullOrWhiteSpace(PaymentMethod))
            {
                return Json(new { result = false, message = "Payment method not defined." });
            }
            try
            {
                string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(token).Split('#');
                Guid CustomerId = new Guid(Decryptval[2]);
                Guid CompanyId = new Guid(Decryptval[1]);
                int InvoiceId = Convert.ToInt32(Decryptval[0]);
                Customer CustomerDetails = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
                Invoice inv = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);
                string TempStatus = "";

                if (inv != null)
                {
                    TempStatus = inv.Status;

                }
                #region Amount Payable calculation
                inv.PayableAmount = inv.BalanceDue.HasValue ? inv.BalanceDue.Value : 0;
                if (!string.IsNullOrWhiteSpace(inv.EstimateTerm)
                    && inv.EstimateTerm == LabelHelper.EstimatePaymentTerms.FiftyFifty
                    && inv.Status != LabelHelper.InvoiceStatus.Partial)
                {
                    inv.PayableAmount = inv.TotalAmount.HasValue ? inv.TotalAmount.Value / 2 : 0;

                    if (inv.PayableAmount > inv.BalanceDue)
                    {
                        inv.PayableAmount = inv.BalanceDue.Value;
                    }
                }
                //double AmountPaid = inv.BalanceDue.Value;
                double AmountPaid = inv.PayableAmount;
                #endregion
                Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
                if (inv == null || CustomerDetails == null || tempCom == null)
                {
                    return Json(new { result = false, message = "Data not found." });
                }
                if (!(inv.BalanceDue.HasValue && inv.BalanceDue.Value > 0))
                {
                    return Json(new { result = false, message = "This invoice balance is already 0." });
                }


                if (inv.CompanyId != CompanyId || inv.CustomerId != CustomerId)
                {
                    return Json(new { result = false, message = "Access Denied." });
                }
                var response = new ReceivePaymentResponse();
                ReceivePaymentModel Model = new ReceivePaymentModel();
                Model.PaymentMethod = PaymentMethod;
                Model.CustomerGId = CustomerId;
                Model.CompanyId = CompanyId;
                Model.CustomerId = CustomerDetails.Id;

                if (PaymentMethod.Trim().Replace(" ", "").ToLower() == "ach")
                {
                    if (ACHInfo == null
                         || string.IsNullOrWhiteSpace(ACHInfo.RoutingNo)
                         || string.IsNullOrWhiteSpace(ACHInfo.AccountNo)
                         || string.IsNullOrWhiteSpace(ACHInfo.ECheckType)
                         || string.IsNullOrWhiteSpace(ACHInfo.AccountType)
                         || string.IsNullOrWhiteSpace(ACHInfo.AccountName)
                        )
                    {
                        return Json(new { result = false, message = "ACH data required. Please fillup all data to proceed." });
                    }
                    Model.ACHInfo = ACHInfo;
                    Model.ACHInfo.Amount = AmountPaid;
                    Model.ACHInfo.FirstName = CustomerDetails.FirstName;
                    Model.ACHInfo.Lastname = CustomerDetails.LastName;
                    Model.ACHInfo.CustomerId = CustomerDetails.Id.ToString();
                    Model.ACHInfo.EmailAddress = CustomerDetails.EmailAddress;
                    Model.ACHInfo.InvoiceNo = InvoiceId.GenerateInvoiceNo();
                    if (!string.IsNullOrWhiteSpace(CustomerDetails.CustomerNo))
                    {
                        Model.ACHInfo.Description = string.Format("[Customer No: {0}]", CustomerDetails.CustomerNo);
                    }

                }
                else if (PaymentMethod.Trim().Replace(" ", "").ToLower() == "creditcard"
                    || PaymentMethod.Trim().Replace(" ", "").ToLower() == "debitcard")
                {
                    if (CardInfo == null
                        || string.IsNullOrWhiteSpace(CardInfo.NameOnCard)
                        || string.IsNullOrWhiteSpace(CardInfo.SecurityCode)
                        || string.IsNullOrWhiteSpace(CardInfo.CardNumber)
                        || string.IsNullOrWhiteSpace(CardInfo.ExpiredDate))
                    {
                        return Json(new { result = false, message = "Card data required. Please fillup all data to proceed." });
                    }
                    Model.CardInfo = CardInfo;
                    Model.CardInfo.Amount = AmountPaid;
                    Model.CardInfo.FirstName = CustomerDetails.FirstName;
                    Model.CardInfo.Lastname = CustomerDetails.LastName;
                    Model.CardInfo.CustomerId = CustomerDetails.Id.ToString();
                    Model.CardInfo.EmailAddress = CustomerDetails.EmailAddress;
                    Model.CardInfo.InvoiceNo = InvoiceId.GenerateInvoiceNo();
                    Model.InvoiceList = InvoiceId.GenerateInvoiceNo();
                    if (!string.IsNullOrWhiteSpace(CustomerDetails.CustomerNo))
                    {
                        Model.CardInfo.Description = string.Format("[Customer No: {0}]", CustomerDetails.CustomerNo);
                    }
                }
                if (PaymentMethod.Trim().Replace(" ", "").ToLower() == "ach"
                    || PaymentMethod.Trim().Replace(" ", "").ToLower() == "creditcard"
                    || PaymentMethod.Trim().Replace(" ", "").ToLower() == "debitcard")
                {
                    response = _Util.Facade.ReceivePaymentFacade.ReceivePayment(Model);
                }
                else
                {
                    response.TransactionSuccess = true;
                    response.TransactionId = PaymentTransId;
                    response.Message = "Transaction successful";
                }
                if (!response.TransactionSuccess)
                {
                    return Json(new { result = false, message = response.Message });
                }
                else
                {
                    if (PaymentMethod.Trim().Replace(" ", "").ToLower() == "ach")
                    {
                        PaymentMethod = LabelHelper.PaymentMethod.ACH;
                    }
                    else if (PaymentMethod.Trim().Replace(" ", "").ToLower() == "creditcard")
                    {
                        PaymentMethod = LabelHelper.PaymentMethod.CreditCard;
                    }
                    else if (PaymentMethod.Trim().Replace(" ", "").ToLower() == "debitcard")
                    {
                        PaymentMethod = LabelHelper.PaymentMethod.DebitCard;
                    }
                    else
                    {
                        PaymentMethod = LabelHelper.PaymentMethod.Others;
                    }
                    #region InsertTransaction
                    Transaction transaction = new Transaction()
                    {
                        AddedBy = "Customer",
                        AddedDate = DateTime.Now.UTCCurrentTime(),
                        Amount = AmountPaid,
                        CompanyId = CompanyId,
                        CustomerId = CustomerId,
                        Type = "Payment",
                        Status = "Closed",
                        TransacationDate = DateTime.Now.UTCCurrentTime(),
                        TransactionId = response.TransactionId,
                        CardTransactionId = response.TransactionId,
                        PaymentMethod = PaymentMethod,
                        ReferenceNo = InvoiceId.GenerateInvoiceNo(),
                    };
                    transaction.Id = _Util.Facade.TransactionFacade.InsertTransaction(transaction);
                    TransactionHistory trh = new TransactionHistory()
                    {
                        Balance = inv.BalanceDue.Value,
                        Amout = AmountPaid,
                        InvoiceId = InvoiceId,
                        ReceivedBy = new Guid("11111111-1111-1111-1111-111111111111"),
                        TransactionId = transaction.Id
                    };
                    trh.Id = _Util.Facade.TransactionFacade.InsertTransactionHistory(trh);
                    #endregion

                    #region Update Invocie
                    inv.BalanceDue = inv.BalanceDue - AmountPaid;
                    inv.Status = inv.BalanceDue == 0 ? LabelHelper.InvoiceStatus.Paid : LabelHelper.InvoiceStatus.Partial;
                    _Util.Facade.InvoiceFacade.UpdateInvoice(inv);
                    if (inv != null && TempStatus != inv.Status)
                    {
                        bool newBool = inv.IsARBInvoice ?? false;

                        #region log
                        UserActivity ua = new UserActivity()
                        {
                            ActivityId = Guid.NewGuid(),
                            PageUrl = Request.Url.AbsoluteUri != null ? Request.Url.AbsoluteUri : "",
                            ReferrerUrl = Request.UrlReferrer != null ? Request.UrlReferrer.AbsoluteUri : "",
                            // new paramiter
                            Action = "4264,CustomerInvoicePayment,Public",
                            StatsDate = DateTime.UtcNow,
                            UserId = inv.CustomerId != null ? inv.CustomerId : Guid.NewGuid(),
                            UserName = CustomerDetails.FirstName + " " + CustomerDetails.LastName,
                            ActionDisplyText = "Invoice Status Changed from " + TempStatus + " To " + inv.Status + " #InvoiceId: " + inv.InvoiceId,
                            IsARB = newBool,

                            UserAgent = AppConfig.GetUserAgent != null ? AppConfig.GetUserAgent : "",
                            UserIp = AppConfig.GetIP != null ? AppConfig.GetIP : ""
                        };
                        Guid ActivityID = ua.ActivityId;
                        _Util.Facade.UserActivityFacade.InsertUserActivity(ua);
                        UserActivityCustomer uac = new UserActivityCustomer()
                        {
                            ActivityId = ActivityID != null ? ActivityID : Guid.NewGuid(),

                            CustomerId = inv.CustomerId != null ? inv.CustomerId : Guid.NewGuid(),
                            RefId = inv.InvoiceId,

                        };
                        _Util.Facade.UserActivityCustomerFacade.InsertUserActivityCustomer(uac);
                        #endregion
                    }

                    #endregion

                    #region Insert CustomerSnapshot
                    CustomerSnapshot objInvoicePayment = new CustomerSnapshot()
                    {
                        CustomerId = CustomerDetails.CustomerId,
                        CompanyId = CompanyId,
                        Description = "Invoice " + string.Format("<a onclick=OpenTopToBottomModal('{2}/Invoice/AddInvoice?id={0}&CustomerId={1}') style='cursor: pointer;'>", inv.Id, CustomerDetails.Id, CustomerDetails.CustomerId, AppConfig.DomainSitePath) + "<b>" + inv.InvoiceId + "</b>" + "</a>" + " created for " + "<b>" + CustomerDetails.FirstName + " " + CustomerDetails.LastName + "</b>",
                        Logdate = DateTime.Now.UTCCurrentTime(),
                        Updatedby = "Customer",
                        Type = "InvoicePaymentHistory"
                    };
                    _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objInvoicePayment);
                    #endregion
                    CustomerAgreement objagree = new CustomerAgreement()
                    {
                        CompanyId = CompanyId,
                        CustomerId = CustomerId,
                        InvoiceId = inv.InvoiceId,
                        Type = "PaymentInvoice",
                        AddedDate = DateTime.Now.UTCCurrentTime()
                    };
                    _Util.Facade.CustomerAgreementFacade.InsertCustomerAgreement(objagree);


                    #region Send Payment Recipt Email 

                    SetupLeadCustormer SetupLeadCustormer = new SetupLeadCustormer();

                    SetupLeadCustormer.CompanyName = tempCom.CompanyName;

                    SetupLeadCustormer.CustomerName = CustomerDetails.FirstName + " " + CustomerDetails.LastName;
                    if (!string.IsNullOrWhiteSpace(CustomerDetails.BusinessName))
                    {
                        SetupLeadCustormer.CustomerName = CustomerDetails.BusinessName;
                    }
                    SetupLeadCustormer.CustomerNo = CustomerDetails.CustomerNo;

                    SetupLeadCustormer.ToEmail = CustomerDetails.EmailAddress;

                    SetupLeadCustormer.InvoiceId = InvoiceId.GenerateInvoiceNo();
                    SetupLeadCustormer.PaymentMethod = PaymentMethod;
                    SetupLeadCustormer.AmountPaid = AmountPaid;
                    SetupLeadCustormer.TotalAmount = inv.TotalAmount.HasValue ? inv.TotalAmount.Value : 0;
                    SetupLeadCustormer.BalanceDue = inv.BalanceDue.HasValue ? inv.BalanceDue.Value : 0;
                    SetupLeadCustormer.TransactionId = transaction.TransactionId;
                    SetupLeadCustormer.CustomerId = CustomerId.ToString();
                    Guid SalesPersonId = new Guid();
                    if (Guid.TryParse(CustomerDetails.Soldby, out SalesPersonId) && SalesPersonId != new Guid())
                    {
                        Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(SalesPersonId);
                        if (emp != null)
                        {
                            SetupLeadCustormer.SalesPersonsName = string.Concat(emp.FirstName, " ", emp.LastName);
                            if (emp.Email.IsValidEmailAddress())
                            {
                                SetupLeadCustormer.ToSalesPersonsEmail = emp.Email;
                            }
                            else if (emp.UserName.IsValidEmailAddress())
                            {
                                SetupLeadCustormer.ToSalesPersonsEmail = emp.UserName;
                            }

                            #region Notification
                            #region Insert notification
                            Notification notification = new Notification();
                            CustomerCompany cusCom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(CustomerDetails.Id);
                            if (cusCom.IsLead == false)
                            {
                                notification = new Notification()
                                {
                                    CompanyId = CompanyId,
                                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                                    NotificationId = Guid.NewGuid(),
                                    Type = LabelHelper.NotificationType.Customer,
                                    Who = CustomerDetails.CustomerId,
                                    What = string.Format(@"A customer <a class=""cus-anchor"" href=""{4}/Customer/Customerdetail/?id={1}"">{0}</a> just make payment to an invoice 
                                            <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenInvById('{2}')"">{3}</a>", "{0}"
                                          , CustomerDetails.Id
                                          , inv.Id
                                          , inv.InvoiceId, AppConfig.DomainSitePath),
                                    NotificationUrl = AppConfig.DomainSitePath + "/Customer/Customerdetail/?id=" + CustomerDetails.Id + "#InvoiceTab"

                                };
                            }
                            else
                            {
                                notification = new Notification()
                                {
                                    CompanyId = CompanyId,
                                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                                    NotificationId = Guid.NewGuid(),
                                    Type = LabelHelper.NotificationType.Customer,
                                    Who = CustomerDetails.CustomerId,
                                    What = string.Format(@"A lead <a class=""cus-anchor"" href=""{4}/Customer/Customerdetail/?id={1}"">{0}</a> just make payment to an invoice 
                                            <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenInvById('{2}')"">{3}</a>", "{0}"
                                         , CustomerDetails.Id
                                         , inv.Id
                                         , inv.InvoiceId, AppConfig.DomainSitePath),
                                    NotificationUrl = AppConfig.DomainSitePath + "/Customer/Customerdetail/?id=" + CustomerDetails.Id + "#InvoiceTab"
                                };
                            }

                            _Util.Facade.NotificationFacade.InsertNotification(notification);

                            #endregion
                            #region set user to notification
                            NotificationUser nu = new NotificationUser()
                            {
                                NotificationId = notification.NotificationId,
                                IsRead = false,
                                NotificationPerson = emp.UserId,
                            };
                            _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
                            #endregion

                            #endregion

                        }
                    }
                    _Util.Facade.MailFacade.EmailToSuccessTransaction(SetupLeadCustormer, CompanyId);

                    #endregion

                    return Json(new { result = response.TransactionSuccess, message = response.Message });
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new { result = false, message = "Invalid Token" });
            }
        }

        #endregion

        #region Make address
        private string MakeAddress(string street, string city, string state, string zipcode, string country)
        {
            string address = "";
            if (!string.IsNullOrWhiteSpace(street))
            {
                address += street + ",";
            }
            if (!string.IsNullOrWhiteSpace(city))
            {
                if (city != "-1")
                {
                    address += city + ",";
                }
            }
            if (!string.IsNullOrWhiteSpace(state))
            {
                if (state != "-1")
                {
                    address += state + ",";
                }
            }
            if (!string.IsNullOrWhiteSpace(zipcode))
            {
                address += zipcode + ",";
            }
            if (!string.IsNullOrWhiteSpace(country))
            {
                address += country + ",";
            }
            return address.TrimEnd(',');
        }
        #endregion

        private CreateInvoice GetCreateInvoiceModel(Invoice Invoice, List<InvoiceDetail> InvoiceDetialList, Company tempCom, Customer tempCUstomer)
        {
            CreateInvoice Model = new CreateInvoice();
            Model.Invoice = Invoice;
            Model.InvoiceDetailList = InvoiceDetialList;

            Model.Invoice.CustomerName = tempCUstomer.Title + " " + tempCUstomer.FirstName + " " + tempCUstomer.LastName;

            Model.Invoice.IsEstimate = false;
            //Model.Invoice.InvoiceDate = Invoice.InvoiceDate.HasValue ? Invoice.InvoiceDate.Value : Model.Invoice.InvoiceDate.Value.ClientToUTCTime();
            //Model.Invoice.DueDate = Invoice.DueDate.HasValue ? Invoice.DueDate.Value : Model.Invoice.DueDate.Value.ClientToUTCTime();
            #region Discount Calculation 
            if (!string.IsNullOrWhiteSpace(Model.Invoice.DiscountType))
            {
                if (Model.Invoice.DiscountType == "amount")
                {
                    if (Invoice.Discountpercent != null)
                    {
                        Model.Discount = Invoice.Discountpercent.Value;
                    }
                }
                else
                {
                    if (Invoice.Discountpercent != null)
                    {
                        Model.Discount = ((Invoice.Discountpercent / 100) * Model.SubTotal).Value;
                    }
                }
            }
            #endregion

            #region making Name of Address Bold
            //if (!string.IsNullOrWhiteSpace(Model.Invoice.BillingAddress))
            //{
            //    var split = Model.Invoice.BillingAddress.Split(new string[] { "\n" }, StringSplitOptions.None);

            //    if (split.Count() > 0)
            //    {
            //        string NewAddress = "";
            //        split[0] = "<b>" + split[0] + "</b>";

            //        foreach (var item in split)
            //        {
            //            NewAddress += item + Environment.NewLine;
            //        }
            //        Model.Invoice.BillingAddress = NewAddress;
            //    }
            //}
            //if (!string.IsNullOrWhiteSpace(Model.Invoice.ShippingAddress))
            //{
            //    var split = Model.Invoice.ShippingAddress.Split(new string[] { "\n" }, StringSplitOptions.None);

            //    if (split.Count() > 0)
            //    {
            //        string NewAddress = "";
            //        split[0] = "<b>" + split[0] + "</b>";

            //        foreach (var item in split)
            //        {
            //            NewAddress += item + Environment.NewLine;
            //        }
            //        Model.Invoice.ShippingAddress = NewAddress;
            //    }
            //}
            //if (!string.IsNullOrWhiteSpace(Model.InvoiceShipping))
            //{
            //    var split = Model.InvoiceShipping.Split(new string[] { "\n" }, StringSplitOptions.None);

            //    if (split.Count() > 0)
            //    {
            //        string NewAddress = "";
            //        split[0] = "<b>" + split[0] + "</b>";

            //        foreach (var item in split)
            //        {
            //            NewAddress += item + Environment.NewLine;
            //        }
            //        Model.InvoiceShipping = NewAddress;
            //    }
            //}
            #endregion
            //Model.Invoice.EstimateTerm = "";
            if (!string.IsNullOrWhiteSpace(Model.Invoice.EstimateTerm) && Model.Invoice.EstimateTerm != "-1")
                Model.Invoice.EstimateTerm = _Util.Facade.LookupFacade.GetDisplayTextByDataValueFromLLookup(Model.Invoice.EstimateTerm);

            //customer name is customer business name here 
            if (tempCUstomer.BusinessName != "")
            {
                Model.CustomerName = tempCUstomer.BusinessName;
            }
            else
            {
                Model.CustomerName = Model.Invoice.CustomerName;
            }
            Model.CusBussinessName = tempCUstomer.BusinessName;

            Model.SubTotal = 0;
            foreach (var item in InvoiceDetialList)
            {
                item.CreatedBy = User.Identity.Name;
                item.CreatedDate = DateTime.Now.UTCCurrentTime();
                item.CompanyId = tempCom.CompanyId;
                Model.SubTotal = Model.SubTotal + (item.TotalPrice.HasValue ? item.TotalPrice.Value : 0);
            }

            Model.CompanyAddress = tempCom.Address;
            Model.CompanyStreet = tempCom.Street;
            string ComCity = "";
            string ComState = "";
            if (!string.IsNullOrWhiteSpace(tempCom.City))
            {
                ComCity = tempCom.City.UppercaseFirst() + ", ";
            }
            if (!string.IsNullOrWhiteSpace(tempCom.State))
            {
                ComState = tempCom.State + " ";
            }
            Model.companyStreetInfo = ComCity + ComState + tempCom.ZipCode;
            Model.CompanyCity = tempCom.City.UppercaseFirst();
            Model.CompanyState = tempCom.State;
            Model.CompanyZip = tempCom.ZipCode;
            Model.CompanyPhone = tempCom.Phone;
            var objpermit = _Util.Facade.PermissionFacade.IsPermittedPermission(3167, Model.Invoice.CreatedByUid, tempCom.CompanyId);
            if (objpermit)
            {
                var empobj = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(Model.Invoice.CreatedByUid);
                if (empobj != null)
                {
                    Model.CompanyEmail = empobj.Email;
                }
            }
            else
            {
                Model.CompanyEmail = tempCom.EmailAdress;
            }
            Model.CompanyName = tempCom.CompanyName;
            Model.PhoneNum = tempCom.Phone;
            Model.CompanyWebsite = tempCom.Website;

            if (string.IsNullOrWhiteSpace(Model.CustomerInfo))
            {
                Model.CustomerInfo = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(tempCom.CompanyId);
            }
            if (string.IsNullOrWhiteSpace(Model.CustomerStreet))
            {
                Model.CustomerStreet = tempCUstomer.Street;
            }
            if (string.IsNullOrWhiteSpace(Model.CustomerCity))
            {
                Model.CustomerCity = tempCUstomer.City.UppercaseFirst();
            }
            if (string.IsNullOrWhiteSpace(Model.CustomerState))
            {
                Model.CustomerState = tempCUstomer.State;
            }
            if (string.IsNullOrWhiteSpace(Model.CustomerZipCode))
            {
                Model.CustomerZipCode = tempCUstomer.ZipCode;
            }
            if (string.IsNullOrWhiteSpace(Model.CustomerNo))
            {
                Model.CustomerNo = tempCUstomer.CustomerNo;
            }
            if (string.IsNullOrWhiteSpace(Model.CustomerSSN))
            {
                Model.CustomerSSN = tempCUstomer.SSN;
            }
            if (Model.CustomerDOB == null || Model.CustomerDOB == new DateTime())
            {
                Model.CustomerDOB = tempCUstomer.DateofBirth.HasValue ? tempCUstomer.DateofBirth.Value : new DateTime();
            }
            CustomerExtended tempCusExtd = new CustomerExtended();
            if (tempCUstomer != null)
            {
                tempCusExtd = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(tempCUstomer.CustomerId);
            }
            Model.CustomerDrivingLicense = tempCusExtd != null ? tempCusExtd.DrivingLicense : "";

            //ViewBag.ShippingValue = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(tempCom.CompanyId);
            Model.ShowInvoiceShippingAddress = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(tempCom.CompanyId).ToLower() == "true" ? true : false;
            GlobalSetting invCode3Sta = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(tempCom.CompanyId, "ShowCode3InvoiceStaticBox");
            if (invCode3Sta != null)
            {
                Model.ShowCode3InvoiceStaticBox = invCode3Sta.Value.ToLower() == "true" ? true : false;
            }
            else
            {
                Model.ShowCode3InvoiceStaticBox = false;
            }
            GlobalSetting invComAddress = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(tempCom.CompanyId, "ShowInvoiceCompanyAddress");
            if (invComAddress != null)
            {
                Model.ShowInvoiceCompanyAddress = invComAddress.Value.ToLower() == "true" ? true : false;
            }
            else
            {
                Model.ShowInvoiceCompanyAddress = false;
            }
            GlobalSetting invPayAddress = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(tempCom.CompanyId, "ShowPaymentAddressForSendInvoice");
            if (invPayAddress != null)
            {
                Model.ShowPaymentAddressForSendInvoice = invPayAddress.Value.ToLower() == "true" ? true : false;
                if (Model.ShowPaymentAddressForSendInvoice)
                {
                    Model.PaymentAddress = !string.IsNullOrWhiteSpace(invPayAddress.OptionalValue) ? invPayAddress.OptionalValue : (!string.IsNullOrWhiteSpace(Model.CompanyStreet) ? Model.CompanyStreet + ", " : "") + Model.CompanyCity + " " + Model.CompanyState + " " + Model.CompanyZip;
                }
            }
            else
            {
                Model.ShowPaymentAddressForSendInvoice = false;
            }

            if (string.IsNullOrWhiteSpace(tempCom.CompanyLogo))
            {
                tempCom.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(tempCom.CompanyId);
            }
            Model.CompanyLogo = tempCom.CompanyLogo;
            if (string.IsNullOrWhiteSpace(Model.Invoice.InvoiceMessage))
            {
                Model.Invoice.InvoiceMessage = Model.Invoice.Message;
            }
            return Model;
        }

        #region NumberToWord
        private static string NumberToWords(double Amount)
        {

            string centval = (Amount - (int)Amount).ToString("#.##");
            double valcent = 0;
            if (!string.IsNullOrWhiteSpace(centval))
            {
                valcent = Convert.ToDouble(centval);
            }
            int cent = (int)(valcent * 100);
            int number = (int)Amount;

            if (number == 0)
                return "Zero";

            if (number < 0)
                return "Minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000000) > 0)
            {
                words += NumberToWords(number / 1000000000) + " Billion ";
                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " Million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }
            if (cent > 0)
            {
                words += " " + cent + "/100";
            }
            return words;
        }
        #endregion

        public ActionResult CustomerTicketAddendumDocument(string code)
        {
            CustomerCompany custommerCompany = new CustomerCompany();
            try
            {
                string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(code).Split('#');
                int ticketid = Convert.ToInt32(Decryptval[0]);
                int LeadId = Convert.ToInt32(Decryptval[1]);
                string companyId = Decryptval[2].ToString();

                CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByCompanyId(companyId);

                if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
                {
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    Guid CompanyId = CC.CompanyId;
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
                }
                custommerCompany = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(LeadId);
                Customer cust = _Util.Facade.CustomerFacade.GetCustomerByLeadId(LeadId);
                Ticket ticket = _Util.Facade.TicketFacade.GetTicketById(ticketid);
                if (custommerCompany != null)
                {
                    ViewBag.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(custommerCompany.CompanyId);
                }
                if (cust != null && ticket != null)
                {
                    ViewBag.CustomerId = cust.CustomerId;
                    ViewBag.TicketId = ticket.TicketId;
                    var objcusaddendum = _Util.Facade.CustomerFacade.GetCustomerAddendumByCustomerIdAndTicketId(ticket.TicketId, cust.CustomerId);
                    if (objcusaddendum != null)
                    {
                        ViewBag.Addendum = objcusaddendum.IsSigned;
                    }
                    else
                    {
                        ViewBag.Addendum = false;
                    }
                }
                ViewBag.Code = code;
            }
            catch (Exception ex)
            {
                custommerCompany = null;
            }
            return View(custommerCompany);
        }
        #region Send Estimate Sign Notification Email
        public bool SendEstimateSignNotificationEmail(string InvoiceId, Guid companyId)
        {
            CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByCompanyId(companyId);

            if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
            {
                //return RedirectToAction("Index", "Login");
            }
            else
            {
                Guid CompanyId = CC.CompanyId;
                string ConnectionString = CC.ConnectionString;
                if (!string.IsNullOrWhiteSpace(ConnectionString))
                {
                    ConnectionString = DESEncryptionDecryption.DecryptCipherTextToPlainText(ConnectionString);

                    Session[SessionKeys.CompanyConnectionString] = ConnectionString;
                }
                else
                {
                    //return RedirectToAction("Index", "Login");
                }
            }

            bool result = false;

            if (InvoiceId != null)
            {
                string recevieremail = "";

                var EmailReceiver = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("ContractSignAndEstimateSignNotificationReceiverEmail");
                if (EmailReceiver != null)
                {
                    recevieremail = EmailReceiver.Value;
                }
                Invoice inv = _Util.Facade.InvoiceFacade.GetInvoiceByInvoiceId(InvoiceId);

                if (inv != null)
                {
                    Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(inv.CustomerId);
                    if (cus != null)
                    {
                        NotificationEmail EmailNotifiaction = new NotificationEmail()
                        {
                            Subject = "Estimate " + inv.InvoiceId + " Has Signed.",
                            ToEmail = recevieremail,
                            EmailBody = "Estimate " + inv.InvoiceId + " Has Signed By " + cus.FirstName + " " + cus.LastName + ".",
                        };
                        result = _Util.Facade.MailFacade.SendSignNotificationEmail(EmailNotifiaction, companyId);
                    }
                }

            }
            return result;
        }
        #endregion


        #region Public Invoice Statement (Ataul) 
        public ActionResult CustomerInvoiceStatement(string code)
        {
            try
            {
                #region Loguout if authenticated
                if (User.Identity.IsAuthenticated)
                {
                    FormsAuthentication.SignOut();
                    SessionHelper hs = new SessionHelper();
                    hs.ClearCurrentSession();
                }
                #endregion

                ViewBag.Token = code;
                string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(code).Split('#');
                string StatementType = Decryptval[3].ToString();
                Guid CustomerId = new Guid(Decryptval[2]);
                Guid CompanyId = new Guid(Decryptval[1]);
                int InvoiceId = Convert.ToInt32(Decryptval[0]);
                #region SettingUp Company Connection
                CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByCompanyId(CompanyId.ToString());

                if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                else
                {
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
                    Company Com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
                    ViewBag.Title = string.Concat(Com.CompanyName, " | ", Localize.T("Customer Invoice Statement"));
                }
                #endregion

                Invoice inv = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);
                Customer TempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
                inv.CustomerName = TempCustomer.FirstName + " " + TempCustomer.LastName;
                inv.CustomerBussinessName = TempCustomer.BusinessName;
                CompanyBranch cb = _Util.Facade.CompanyBranchFacade.GetMainBranchByCompanyId(CompanyId);
                ViewBag.CompanyLogo = cb.Logo;
                if (inv == null
                    || inv.IsEstimate
                    || CompanyId != inv.CompanyId
                    || CustomerId != inv.CustomerId
                    )
                {
                    return PartialView("~/Views/Shared/_LinkExpired.cshtml");
                }
                System.Web.HttpContext.Current.Session[SessionKeys.CompanyId] = CompanyId;

                #region Amount Payable calculation
                var TotalAmountModel = _Util.Facade.InvoiceFacade.GetTotalDueAmountForStatement(CustomerId, StatementType);
                if (TotalAmountModel != null)
                {
                    inv.PayableAmount = TotalAmountModel.BalanceDue;
                    inv.TotalAmount = TotalAmountModel.TotalAmount;
                    inv.BalanceDue = TotalAmountModel.BalanceDue;
                }
                else
                {
                    inv.PayableAmount = 0;
                    inv.TotalAmount = 0;
                    inv.BalanceDue = 0;
                }
                #endregion

                #region ViewBags
                ViewBag.ECheckType = _Util.Facade.LookupFacade.GetLookupByKey("ECheckType").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();
                ViewBag.BankAccountType = _Util.Facade.LookupFacade.GetLookupByKey("BankAccountType").Select(x =>
                               new SelectListItem()
                               {
                                   Text = x.DisplayText.ToString(),
                                   Value = x.DataValue.ToString()
                               }).ToList();
                ViewBag.CompanyId = CompanyId.ToString();
                ViewBag.FooterCompanyInformation = _Util.Facade.MailFacade.GetFooterCompanyInformationByCompanyId(CompanyId).Replace("##Year##", string.Format("2017-{0}", DateTime.Now.Year.ToString()));
                ViewBag.PaymentMethodList = _Util.Facade.LookupFacade.GetLookupByKey("PublicPaymentMethod").ToList();
                #endregion

                Guid SalesPersonId = new Guid();
                if (Guid.TryParse(TempCustomer.Soldby, out SalesPersonId) && SalesPersonId != new Guid())
                {
                    Notification notification = new Notification();
                    #region Insert notification
                    CustomerCompany cusCom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(TempCustomer.Id);
                    if (cusCom.IsLead == false)
                    {
                        notification = new Notification()
                        {
                            CompanyId = CompanyId,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            NotificationId = Guid.NewGuid(),
                            Type = LabelHelper.NotificationType.Customer,
                            Who = TempCustomer.CustomerId,
                            What = string.Format(@"A customer <a class=""cus-anchor"" href=""{4}/Customer/Customerdetail/?id={1}"">{0}</a> just viewed an invoice statement
                                            <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenInvById('{2}')"">{3}</a>", "{0}", TempCustomer.Id, inv.Id, inv.InvoiceId, AppConfig.DomainSitePath),
                            NotificationUrl = AppConfig.DomainSitePath + "/Customer/Customerdetail/?id=" + TempCustomer.Id + "#InvoiceTab"

                        };
                    }
                    else
                    {
                        notification = new Notification()
                        {
                            CompanyId = CompanyId,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            NotificationId = Guid.NewGuid(),
                            Type = LabelHelper.NotificationType.Customer,
                            Who = TempCustomer.CustomerId,
                            What = string.Format(@"A Lead <a class=""cus-anchor"" href=""{4}/Customer/Customerdetail/?id={1}"">{0}</a> just viewed an invoice statement  
                                            <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenInvById('{2}')"">{3}</a>", "{0}", TempCustomer.Id, inv.Id, inv.InvoiceId, AppConfig.DomainSitePath),
                            NotificationUrl = AppConfig.DomainSitePath + "/Customer/Customerdetail/?id=" + TempCustomer.Id + "#InvoiceTab"
                        };
                    }

                    _Util.Facade.NotificationFacade.InsertNotification(notification);

                    #endregion
                    #region set user to notification
                    NotificationUser nu = new NotificationUser()
                    {
                        NotificationId = notification.NotificationId,
                        IsRead = false,
                        NotificationPerson = SalesPersonId,
                    };
                    _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
                    #endregion
                }

                CustomerAgreement objagree = new CustomerAgreement()
                {
                    CompanyId = CompanyId,
                    CustomerId = CustomerId,
                    InvoiceId = inv.InvoiceId,
                    Type = "LoadInvoice",
                    AddedDate = DateTime.Now.UTCCurrentTime()
                };
                _Util.Facade.CustomerAgreementFacade.InsertCustomerAgreement(objagree);


                #region User Log
                UserActivityCustomer uaclog = new UserActivityCustomer()
                {
                    ActivityId = Guid.NewGuid(),
                    CustomerId = CustomerId,
                    RefId = inv.InvoiceId
                };
                _Util.Facade.UserActivityCustomerFacade.InsertUserActivityCustomer(uaclog);
                UserActivity ulog = new UserActivity()
                {
                    ActivityId = uaclog.ActivityId,
                    PageUrl = Request.Url.AbsoluteUri != null ? Request.Url.AbsoluteUri : "",
                    ReferrerUrl = Request.UrlReferrer != null ? Request.UrlReferrer.AbsoluteUri : "",
                    Action = "CustomerInvoiceStatementPayment,Public",
                    StatsDate = DateTime.UtcNow,
                    UserId = CustomerId,
                    UserName = inv.CustomerName,
                    ActionDisplyText = "Emailed invoice#: " + inv.InvoiceId + " statement viewed by customer.",
                    IsARB = inv.IsARBInvoice.HasValue ? inv.IsARBInvoice.Value : false,
                    UserAgent = AppConfig.GetUserAgent != null ? AppConfig.GetUserAgent : "",
                    UserIp = AppConfig.GetIP != null ? AppConfig.GetIP : ""
                };
                _Util.Facade.UserActivityFacade.InsertUserActivity(ulog);
                #endregion
                return View(inv);
            }
            catch (Exception)
            {
                return PartialView("~/Views/Shared/_LinkExpired.cshtml");
            }
        }
        public ActionResult CustomerInvoiceStatementPdf(string Code, bool IsMobile = false)
        {
            if (string.IsNullOrWhiteSpace(Code))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            try
            {
                string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(Code).Split('#');
                string StatementType = Decryptval[3].ToString();
                Guid CustomerId = new Guid(Decryptval[2]);
                Guid CompanyId = new Guid(Decryptval[1]);
                int InvoiceId = Convert.ToInt32(Decryptval[0]);
                if (Session[SessionKeys.CompanyConnectionString] == null || string.IsNullOrWhiteSpace(Session[SessionKeys.CompanyConnectionString].ToString()))
                {
                    CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByCompanyId(CompanyId.ToString());

                    if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
                    {
                        return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                    }
                    else
                    {
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
                    }
                }

                Invoice Inv = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);
                if (Inv.IsEstimate
                    || CompanyId != Inv.CompanyId
                    || Inv.CustomerId != CustomerId
                    )
                {
                    return PartialView("~/Views/Shared/_LinkExpired.cshtml");
                }
                Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Inv.CompanyId);
                Customer tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
                List<int> CustomerIntIdList = new List<int>();
                CustomerIntIdList.Add(tempCustomer.Id);
                #region Company Info Viewbag section
                ViewBag.CompanyId = tempCom.CompanyId;
                ViewBag.CompanyName = tempCom.CompanyName;
                ViewBag.CompanyAddress = tempCom.Street;
                ViewBag.CompanyCity = tempCom.City + ", " + tempCom.State + " " + tempCom.ZipCode;
                ViewBag.CompanyEmail = tempCom.EmailAdress;
                ViewBag.CompanyPhone = tempCom.Phone;
                ViewBag.CompanyWebsite = tempCom.Website;
                #endregion
                List<GeneratePdfInvoiceStatementModelList> GetAllDateList = new List<GeneratePdfInvoiceStatementModelList>();
                string StrInvoiceIdList = "";
                if (Inv.Status == "Open" || Inv.Status == "Partial")
                {
                    if (CustomerIntIdList.Count == 1)
                    {
                        GetAllDateList = _Util.Facade.InvoiceFacade.GetAllForInvoiceStatementDataByCustomerIntIdListWithType(CustomerIntIdList, StatementType);
                    }
                    if (GetAllDateList != null && GetAllDateList.Count > 0)
                    {
                        var AllDateList = GetAllDateList[0];
                        StrInvoiceIdList = string.Format("{0}", AllDateList.InvoiceStatement.InvoiceId);
                        if (AllDateList.DueOpenInvoiceList != null && AllDateList.DueOpenInvoiceList.Count > 0 && !string.IsNullOrWhiteSpace(StrInvoiceIdList) && !string.IsNullOrEmpty(StrInvoiceIdList))
                        {
                            foreach (var item in AllDateList.DueOpenInvoiceList)
                            {
                                StrInvoiceIdList += string.Format(",{0}", item.InvoiceId);
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(StrInvoiceIdList) && !string.IsNullOrEmpty(StrInvoiceIdList))
                    {
                        System.Web.HttpContext.Current.Session[SessionKeys.StringInvoiceIdList] = StrInvoiceIdList;
                    }
                }
                else if (Inv.Status == "Paid")
                {
                    if (Session[SessionKeys.StringInvoiceIdList] != null || !string.IsNullOrWhiteSpace(Session[SessionKeys.StringInvoiceIdList].ToString()))
                    {
                        string StatementInvoiceList = Session[SessionKeys.StringInvoiceIdList].ToString();
                        GetAllDateList = _Util.Facade.InvoiceFacade.GetPaidInvoiceStatementDataByInvoiceIdList(StatementInvoiceList);
                    }
                    else
                    {
                        return PartialView("~/Views/Shared/_LinkExpired.cshtml");
                    }
                }
                else
                {
                    return PartialView("~/Views/Shared/_LinkExpired.cshtml");
                }
                if (GetAllDateList == null || GetAllDateList.Count < 1)
                {
                    return PartialView("~/Views/Shared/_Loading.cshtml");
                }
                if (!IsMobile)
                {
                    return new Rotativa.ViewAsPdf("~/Views/Invoice/InvoiceStatementPdf.cshtml", GetAllDateList)
                    {
                        //FileName = "TestView.pdf",
                        PageSize = Rotativa.Options.Size.A4,
                        PageOrientation = Rotativa.Options.Orientation.Portrait,
                        PageMargins = { Left = 1, Right = 1 },

                    };
                }
                return View("~/Views/Invoice/StatementInvoiceList.cshtml", GetAllDateList);
            }
            catch (Exception)
            {
                return PartialView("~/Views/Shared/_Loading.cshtml");
            }
        }

        [HttpPost]
        public JsonResult CustomerInvoiceStatementPayment(string token, string PaymentMethod, ACHInfo ACHInfo, CardInfo CardInfo, string PaymentTransId)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return Json(new { result = false, message = "Invalid Token" });
            }
            if (string.IsNullOrWhiteSpace(PaymentMethod))
            {
                return Json(new { result = false, message = "Payment method not defined." });
            }
            try
            {
                string[] Decryptval = DESEncryptionDecryption.DecryptCipherTextToPlainText(token).Split('#');
                string StatementType = Decryptval[3].ToString();
                Guid CustomerId = new Guid(Decryptval[2]);
                Guid CompanyId = new Guid(Decryptval[1]);
                int InvoiceId = Convert.ToInt32(Decryptval[0]);
                string StrInvoiceId = InvoiceId.GenerateInvoiceNo();
                Customer CustomerDetails = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
                Invoice inv = new Invoice();
                List<Invoice> InvoiceList = _Util.Facade.InvoiceFacade.GetUnpaidInvoiceListByCustomerIdandIsARB(CustomerId, StatementType);
                #region Amount Payable calculation
                double AmountPaid = 0, StatementTotalAmount = 0;
                var TotalAmountModel = _Util.Facade.InvoiceFacade.GetTotalDueAmountForStatement(CustomerId, StatementType);
                if (TotalAmountModel != null)
                {
                    AmountPaid = TotalAmountModel.BalanceDue;
                    StatementTotalAmount = TotalAmountModel.TotalAmount;
                }
                #endregion
                Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CompanyId);
                if (InvoiceList == null || CustomerDetails == null || tempCom == null)
                {
                    return Json(new { result = false, message = "Data not found." });
                }
                if (!(AmountPaid > 0))
                {
                    return Json(new { result = false, message = "This statement balance is already 0." });
                }
                if (InvoiceList != null && InvoiceList.Count > 0)
                {
                    inv = InvoiceList[0];
                }
                else
                {
                    return Json(new { result = false, message = "Data not found." });
                }
                if (inv.CompanyId != CompanyId || inv.CustomerId != CustomerId)
                {
                    return Json(new { result = false, message = "Access Denied." });
                }

                var response = new ReceivePaymentResponse();
                ReceivePaymentModel Model = new ReceivePaymentModel();
                Model.PaymentMethod = PaymentMethod;
                Model.CustomerGId = CustomerId;
                Model.CompanyId = CompanyId;
                Model.CustomerId = CustomerDetails.Id;

                if (PaymentMethod.Trim().Replace(" ", "").ToLower() == "ach")
                {
                    if (ACHInfo == null
                         || string.IsNullOrWhiteSpace(ACHInfo.RoutingNo)
                         || string.IsNullOrWhiteSpace(ACHInfo.AccountNo)
                         || string.IsNullOrWhiteSpace(ACHInfo.ECheckType)
                         || string.IsNullOrWhiteSpace(ACHInfo.AccountType)
                         || string.IsNullOrWhiteSpace(ACHInfo.AccountName)
                        )
                    {
                        return Json(new { result = false, message = "ACH data required. Please fillup all data to proceed." });
                    }
                    Model.ACHInfo = ACHInfo;
                    Model.ACHInfo.Amount = AmountPaid;
                    Model.ACHInfo.FirstName = CustomerDetails.FirstName;
                    Model.ACHInfo.Lastname = CustomerDetails.LastName;
                    Model.ACHInfo.CustomerId = CustomerDetails.Id.ToString();
                    Model.ACHInfo.EmailAddress = CustomerDetails.EmailAddress;
                    Model.ACHInfo.InvoiceNo = StrInvoiceId;
                    if (!string.IsNullOrWhiteSpace(CustomerDetails.CustomerNo))
                    {
                        Model.ACHInfo.Description = string.Format("[Customer No: {0}]", CustomerDetails.CustomerNo);
                    }
                }
                else if (PaymentMethod.Trim().Replace(" ", "").ToLower() == "creditcard"
                    || PaymentMethod.Trim().Replace(" ", "").ToLower() == "debitcard")
                {
                    if (CardInfo == null
                        || string.IsNullOrWhiteSpace(CardInfo.NameOnCard)
                        || string.IsNullOrWhiteSpace(CardInfo.SecurityCode)
                        || string.IsNullOrWhiteSpace(CardInfo.CardNumber)
                        || string.IsNullOrWhiteSpace(CardInfo.ExpiredDate))
                    {
                        return Json(new { result = false, message = "Card data required. Please fillup all data to proceed." });
                    }
                    Model.CardInfo = CardInfo;
                    Model.CardInfo.Amount = AmountPaid;
                    Model.CardInfo.FirstName = CustomerDetails.FirstName;
                    Model.CardInfo.Lastname = CustomerDetails.LastName;
                    Model.CardInfo.CustomerId = CustomerDetails.Id.ToString();
                    Model.CardInfo.EmailAddress = CustomerDetails.EmailAddress;
                    Model.CardInfo.InvoiceNo = StrInvoiceId;
                    Model.InvoiceList = StrInvoiceId;
                    if (!string.IsNullOrWhiteSpace(CustomerDetails.CustomerNo))
                    {
                        Model.CardInfo.Description = string.Format("[Customer No: {0}]", CustomerDetails.CustomerNo);
                    }
                }

                #region Insert Into Transaction Queue
                string Starttime = DateTime.Now.AddMinutes(-1).ToString("yyyy-MM-dd HH:mm:ss");
                string Endtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                List<TransactionQueue> transqueList = new List<TransactionQueue>();
                transqueList = _Util.Facade.TransactionFacade.GetTransactionQueueCustomerId(CustomerDetails.CustomerId, Starttime, Endtime, AmountPaid);
                if (transqueList.Count > 0)
                {
                    return Json(new { result = false, transactionSuccess = false, message = "Duplicate transection requested with same amount. Please try after 1 minute." });
                }
                else
                {
                    TransactionQueue transque = new TransactionQueue();
                    transque.CustomerId = CustomerDetails.CustomerId;
                    transque.Amount = AmountPaid;
                    transque.InvoiceId = StrInvoiceId;
                    transque.CreatedBy = new Guid("11111111-1111-1111-1111-111111111111");
                    transque.CreatedDate = DateTime.Now;
                    _Util.Facade.TransactionFacade.InsertTransactionQueue(transque);
                }
                #endregion

                if (PaymentMethod.Trim().Replace(" ", "").ToLower() == "ach"
                    || PaymentMethod.Trim().Replace(" ", "").ToLower() == "creditcard"
                    || PaymentMethod.Trim().Replace(" ", "").ToLower() == "debitcard")
                {
                    response = _Util.Facade.ReceivePaymentFacade.ReceivePayment(Model);
                }
                else
                {
                    response.TransactionSuccess = true;
                    response.TransactionId = PaymentTransId;
                    response.Message = "Transaction successful";
                }
                if (!response.TransactionSuccess)
                {
                    return Json(new { result = false, message = response.Message });
                }
                else
                {
                    if (PaymentMethod.Trim().Replace(" ", "").ToLower() == "ach")
                    {
                        PaymentMethod = LabelHelper.PaymentMethod.ACH;
                    }
                    else if (PaymentMethod.Trim().Replace(" ", "").ToLower() == "creditcard")
                    {
                        PaymentMethod = LabelHelper.PaymentMethod.CreditCard;
                    }
                    else if (PaymentMethod.Trim().Replace(" ", "").ToLower() == "debitcard")
                    {
                        PaymentMethod = LabelHelper.PaymentMethod.DebitCard;
                    }
                    else
                    {
                        PaymentMethod = LabelHelper.PaymentMethod.Others;
                    }
                    #region InsertTransaction
                    Transaction transaction = new Transaction()
                    {
                        AddedBy = "Customer",
                        AddedDate = DateTime.Now.UTCCurrentTime(),
                        Amount = Math.Round(AmountPaid, 2),
                        CompanyId = CompanyId,
                        CustomerId = CustomerId,
                        CreatedBy = new Guid("11111111-1111-1111-1111-111111111111"),
                        Type = "Payment",
                        Status = "Closed",
                        TransacationDate = DateTime.Now.UTCCurrentTime(),
                        TransactionId = response.TransactionId,
                        CardTransactionId = response.TransactionId,
                        PaymentMethod = PaymentMethod,
                        ReferenceNo = string.IsNullOrWhiteSpace(response.TransactionId) ? StrInvoiceId : response.TransactionId
                    };
                    transaction.Id = _Util.Facade.TransactionFacade.InsertTransaction(transaction);

                    List<TransactionHistory> trhistory = new List<TransactionHistory>();
                    InvoiceList = InvoiceList.OrderBy(x => x.Id).ToList();
                    double TotalCollectedAmount = Math.Round(AmountPaid, 2);
                    foreach (var item in InvoiceList)
                    {
                        string TempStatus = item.Status;
                        double PerPayment = 0;
                        if (item.BalanceDue.HasValue && item.BalanceDue.Value > 0 && TotalCollectedAmount > 0)
                        {
                            if (TotalCollectedAmount > item.BalanceDue.Value)
                            {
                                PerPayment = Math.Round(item.BalanceDue.Value, 2);
                                TotalCollectedAmount -= PerPayment;
                            }
                            else
                            {
                                PerPayment = Math.Round(TotalCollectedAmount, 2);
                                TotalCollectedAmount = 0;
                            }
                        }
                        else { continue; }
                        trhistory.Add(new TransactionHistory()
                        {
                            Amout = PerPayment,
                            InvoiceId = item.Id,
                            TransactionId = transaction.Id,
                            Balance = Math.Round(item.BalanceDue.HasValue ? item.BalanceDue.Value : 0, 2),
                            ReceivedBy = new Guid("11111111-1111-1111-1111-111111111111"),
                            InvoiceTotal = item.TotalAmount.HasValue ? item.TotalAmount.Value : 0,
                            InvoiceBalanceDue = item.BalanceDue.HasValue ? item.BalanceDue.Value : 0
                        });

                        #region Update Invocie
                        item.BalanceDue = Math.Round(item.BalanceDue.HasValue ? item.BalanceDue.Value : 0, 2) - Math.Round(PerPayment, 2);
                        item.Status = item.BalanceDue == 0 ? LabelHelper.InvoiceStatus.Paid : LabelHelper.InvoiceStatus.Partial;
                        item.PaymentType = Model.PaymentMethod;
                        _Util.Facade.InvoiceFacade.UpdateInvoice(item);
                        if (item != null && TempStatus != item.Status)
                        {
                            #region User Log
                            UserActivityCustomer uaclog = new UserActivityCustomer()
                            {
                                ActivityId = Guid.NewGuid(),
                                CustomerId = CustomerId,
                                RefId = item.InvoiceId
                            };
                            _Util.Facade.UserActivityCustomerFacade.InsertUserActivityCustomer(uaclog);
                            UserActivity ualog = new UserActivity()
                            {
                                ActivityId = uaclog.ActivityId,
                                PageUrl = Request.Url.AbsoluteUri != null ? Request.Url.AbsoluteUri : "",
                                ReferrerUrl = Request.UrlReferrer != null ? Request.UrlReferrer.AbsoluteUri : "",
                                Action = "CustomerInvoiceStatementPayment,Public",
                                StatsDate = DateTime.UtcNow,
                                UserId = CustomerId,
                                UserName = CustomerDetails.FirstName + " " + CustomerDetails.LastName,
                                ActionDisplyText = "Invoice status changed from " + TempStatus.ToLower() + " to " + item.Status.ToLower() + " #InvoiceId: " + item.InvoiceId + " collected amount $" + PerPayment.ToString("N2") + " by " + PaymentMethod.ToLower(),
                                IsARB = item.IsARBInvoice.HasValue ? item.IsARBInvoice.Value : false,
                                UserAgent = AppConfig.GetUserAgent != null ? AppConfig.GetUserAgent : "",
                                UserIp = AppConfig.GetIP != null ? AppConfig.GetIP : ""
                            };
                            _Util.Facade.UserActivityFacade.InsertUserActivity(ualog);

                            #endregion
                        }
                        #endregion
                        #region Insert CustomerSnapshot
                        CustomerSnapshot objInvoicePayment = new CustomerSnapshot()
                        {
                            CustomerId = CustomerDetails.CustomerId,
                            CompanyId = CompanyId,
                            Description = "Invoice " + string.Format("<a onclick=OpenTopToBottomModal('{2}/Invoice/AddInvoice?id={0}&CustomerId={1}') style='cursor: pointer;'>", item.Id, CustomerDetails.Id, CustomerDetails.CustomerId, AppConfig.DomainSitePath) + "<b>" + item.InvoiceId + "</b>" + "</a>" + " created for " + "<b>" + CustomerDetails.FirstName + " " + CustomerDetails.LastName + "</b>",
                            Logdate = DateTime.Now.UTCCurrentTime(),
                            Updatedby = "Customer",
                            Type = "InvoicePaymentHistory"
                        };
                        _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(objInvoicePayment);
                        #endregion
                    }
                    if (trhistory.Count() > 0)
                    {
                        _Util.Facade.TransactionFacade.InsertTransactionHistoryList(trhistory);
                    }
                    #endregion

                    #region Send Payment Recipt Email 

                    SetupLeadCustormer SetupLeadCustormer = new SetupLeadCustormer();
                    SetupLeadCustormer.CompanyName = tempCom.CompanyName;
                    SetupLeadCustormer.CustomerName = CustomerDetails.FirstName + " " + CustomerDetails.LastName;
                    if (!string.IsNullOrWhiteSpace(CustomerDetails.BusinessName))
                    {
                        SetupLeadCustormer.CustomerName = CustomerDetails.BusinessName;
                    }
                    SetupLeadCustormer.CustomerNo = CustomerDetails.CustomerNo;
                    SetupLeadCustormer.ToEmail = CustomerDetails.EmailAddress;
                    SetupLeadCustormer.InvoiceId = StrInvoiceId;
                    SetupLeadCustormer.PaymentMethod = PaymentMethod;
                    SetupLeadCustormer.AmountPaid = AmountPaid;
                    SetupLeadCustormer.TotalAmount = StatementTotalAmount;
                    var TotalBalanceDueModel = _Util.Facade.InvoiceFacade.GetTotalDueAmountForStatement(CustomerId, StatementType);
                    if (TotalBalanceDueModel != null)
                    {
                        SetupLeadCustormer.BalanceDue = TotalBalanceDueModel.BalanceDue;
                    }
                    SetupLeadCustormer.TransactionId = transaction.TransactionId;
                    SetupLeadCustormer.CustomerId = CustomerId.ToString();
                    Guid SalesPersonId = new Guid();
                    if (Guid.TryParse(CustomerDetails.Soldby, out SalesPersonId) && SalesPersonId != new Guid())
                    {
                        Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(SalesPersonId);
                        if (emp != null)
                        {
                            SetupLeadCustormer.SalesPersonsName = string.Concat(emp.FirstName, " ", emp.LastName);
                            if (emp.Email.IsValidEmailAddress())
                            {
                                SetupLeadCustormer.ToSalesPersonsEmail = emp.Email;
                            }
                            else if (emp.UserName.IsValidEmailAddress())
                            {
                                SetupLeadCustormer.ToSalesPersonsEmail = emp.UserName;
                            }

                            #region Notification
                            #region Insert notification
                            Notification notification = new Notification();
                            CustomerCompany cusCom = _Util.Facade.CustomerFacade.GetCustomerCompanyByCustomerId(CustomerDetails.Id);
                            if (cusCom.IsLead == false)
                            {
                                notification = new Notification()
                                {
                                    CompanyId = CompanyId,
                                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                                    NotificationId = Guid.NewGuid(),
                                    Type = LabelHelper.NotificationType.Customer,
                                    Who = CustomerDetails.CustomerId,
                                    What = string.Format(@"A customer <a class=""cus-anchor"" href=""{4}/Customer/Customerdetail/?id={1}"">{0}</a> just make payment to an invoice 
                                            <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenInvById('{2}')"">{3}</a>", "{0}"
                                          , CustomerDetails.Id
                                          , inv.Id
                                          , inv.InvoiceId, AppConfig.DomainSitePath),
                                    NotificationUrl = AppConfig.DomainSitePath + "/Customer/Customerdetail/?id=" + CustomerDetails.Id + "#InvoiceTab"

                                };
                            }
                            else
                            {
                                notification = new Notification()
                                {
                                    CompanyId = CompanyId,
                                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                                    NotificationId = Guid.NewGuid(),
                                    Type = LabelHelper.NotificationType.Customer,
                                    Who = CustomerDetails.CustomerId,
                                    What = string.Format(@"A lead <a class=""cus-anchor"" href=""{4}/Customer/Customerdetail/?id={1}"">{0}</a> just make payment to an invoice 
                                            <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenInvById('{2}')"">{3}</a>", "{0}"
                                         , CustomerDetails.Id
                                         , inv.Id
                                         , inv.InvoiceId, AppConfig.DomainSitePath),
                                    NotificationUrl = AppConfig.DomainSitePath + "/Customer/Customerdetail/?id=" + CustomerDetails.Id + "#InvoiceTab"
                                };
                            }

                            _Util.Facade.NotificationFacade.InsertNotification(notification);

                            #endregion
                            #region set user to notification
                            NotificationUser nu = new NotificationUser()
                            {
                                NotificationId = notification.NotificationId,
                                IsRead = false,
                                NotificationPerson = emp.UserId,
                            };
                            _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
                            #endregion

                            #endregion

                        }
                    }
                    bool SendEmailResult = _Util.Facade.MailFacade.EmailToSuccessTransaction(SetupLeadCustormer, CompanyId);
                    #region Email Send User Log
                    string LogMessage = "";
                    if (SendEmailResult) { LogMessage = "Payment receipt send successful"; }
                    else { LogMessage = "Payment receipt send failed"; }

                    LogMessage = LogMessage + " and collected amount $" + AmountPaid.ToString("N2") + " by " + PaymentMethod.ToLower() + " transaction number: " + transaction.TransactionId + " throw online.";
                    UserActivityCustomer uac = new UserActivityCustomer()
                    {
                        ActivityId = Guid.NewGuid(),
                        CustomerId = CustomerId,
                        RefId = inv.InvoiceId
                    };
                    _Util.Facade.UserActivityCustomerFacade.InsertUserActivityCustomer(uac);
                    UserActivity ua = new UserActivity()
                    {
                        ActivityId = uac.ActivityId,
                        PageUrl = Request.Url.AbsoluteUri != null ? Request.Url.AbsoluteUri : "",
                        ReferrerUrl = Request.UrlReferrer != null ? Request.UrlReferrer.AbsoluteUri : "",
                        Action = "CustomerInvoiceStatementPayment,Public",
                        StatsDate = DateTime.UtcNow,
                        UserId = CustomerId,
                        UserName = CustomerDetails.FirstName + " " + CustomerDetails.LastName,
                        ActionDisplyText = LogMessage,
                        IsARB = inv.IsARBInvoice.HasValue ? inv.IsARBInvoice.Value : false,
                        UserAgent = AppConfig.GetUserAgent != null ? AppConfig.GetUserAgent : "",
                        UserIp = AppConfig.GetIP != null ? AppConfig.GetIP : ""
                    };
                    _Util.Facade.UserActivityFacade.InsertUserActivity(ua);

                    #endregion

                    #endregion

                    return Json(new { result = response.TransactionSuccess, message = response.Message });
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new { result = false, message = "Invalid Token" });
            }
        }

        #endregion
    }
}