using Rotativa;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using HS.Framework.Utils;
using Localize = HS.Web.UI.Helper.LanguageHelper;
using System.Collections;
using System.Globalization;
using HS.Facade;
using System.IO;
using HS.Entities;
using HS.Framework;
using HS.Web.UI.Helper;
using Newtonsoft.Json;
using EO.Internal;
using iTextSharp.text.pdf.qrcode;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO; 
using System.Text.RegularExpressions; 
using HS.DataAccess;
using Microsoft.Web.Services3.Addressing;
using System.Security.Policy;

namespace HS.Web.UI.Controllers
{
    public class EstimatorController : BaseController
    {
        // GET: Estimator
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EstimatorPartial(int? CustomerId, EstimateFilter filter) 
        {
            //List<Estimator> model = new List<Estimator>();
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            EstimatorDashboard model = new EstimatorDashboard();
            //if(!string.IsNullOrWhiteSpace(filter.StrStartDate.ToString()))
            //{
            //    filter.StrStartDate = "";
            //}
            if (CustomerId.HasValue)
            {
                var customerInfo = _Util.Facade.CustomerFacade.GetCustomersById(CustomerId.Value);
                if (customerInfo != null)
                {
                    model = _Util.Facade.EstimatorFacade.GetAllEstimatorCountByCustomerIdAndCompanyId(customerInfo.CustomerId, CurrentUser.CompanyId.Value, filter);

                }
            }
            #region Date Offset
            string FirstDayOfWeek = _Util.Facade.GlobalSettingsFacade.GetStartDayOfWeek(CurrentUser.CompanyId.Value);
            int DateOffset = 0;
            if (FirstDayOfWeek == "Saturday")
            {
                DateOffset = -1;
            }
            else if (FirstDayOfWeek == "Monday")
            {
                DateOffset = 1;
            }
            CultureInfo ci = new CultureInfo("en-US");
            #endregion

            #region Week Filter Dropdown
            ////Copany start date will be retrive from global settings
            //DateTime CompanyStartDate = new DateTime(2018, 7, 1);
            //int Week = GetIso8601WeekOfYear(CompanyStartDate);

            //int CurrentWeek = GetIso8601WeekOfYear(DateTime.Now);
            //CompanyStartDate = FirstDateOfWeek(CompanyStartDate.Year, Week, ci, DateOffset);

            //DateTime StartDate = new DateTime();
            //DateTime EndDate = new DateTime();
            //string PtoFilter = "-1";

            ////#region CookieJobs
            ////bool fromCookie = false;
            ////string newCookie = "";
            ////if (Request.Cookies[CookieKeys.PtoFilter] != null && !string.IsNullOrWhiteSpace(Request.Cookies[CookieKeys.PtoFilter].Value))
            ////{
            ////    newCookie = Request.Cookies[CookieKeys.PtoFilter].Value;
            ////    newCookie = Server.UrlDecode(newCookie);
            ////    var CookieVals = newCookie.Split(',');
            ////    if (CookieVals.Length == 4)
            ////    {
            ////        StartDate = CookieVals[0].ToDateTime();
            ////        EndDate = CookieVals[1].ToDateTime();
            ////        string SelectedWeek = CookieVals[2];
            ////        if (SelectedWeek.Split('/').Length == 2)
            ////        {
            ////            int.TryParse(SelectedWeek.Split('/')[1], out CurrentWeek);
            ////        }
            ////        PtoFilter = CookieVals[3];

            ////        fromCookie = true;
            ////    }
            ////}
            ////#endregion

            //while (CompanyStartDate < DateTime.Now)
            //{
            //    string suffix = "th";
            //    if (CompanyStartDate.Day == 1 || CompanyStartDate.Day % 20 == 1 || CompanyStartDate.Day % 30 == 1)
            //    {
            //        suffix = "st";
            //    }
            //    else if (CompanyStartDate.Day == 2 || CompanyStartDate.Day % 20 == 2)
            //    {
            //        suffix = "nd";
            //    }
            //    else if (CompanyStartDate.Day == 3 || CompanyStartDate.Day % 20 == 3)
            //    {
            //        suffix = "rd";
            //    }


            //    CompanyStartDate = CompanyStartDate.AddDays(7);
            //    Week = GetIso8601WeekOfYear(CompanyStartDate);
            //}

            //if (StartDate == new DateTime() && EndDate == new DateTime())
            //{
            //    ViewBag.EndDate = CompanyStartDate;
            //    ViewBag.StartDate = CompanyStartDate.AddDays(-7);
            //}
            //else
            //{
            //    ViewBag.EndDate = EndDate;
            //    ViewBag.StartDate = StartDate;
            //}
            #region Common filter
            ViewBag.EndDate = new DateTime();
            ViewBag.StartDate = new DateTime();
            string PtoFilter = "-1";
            ViewBag.PTOFilterOptions = _Util.Facade.LookupFacade.GetLookupByKey("PTOFilterOptions").Select(x =>
               new SelectListItem()
               {
                   Text = x.DisplayText.ToString(),
                   Value = x.DataValue.ToString(),
                   Selected = x.DataValue == PtoFilter
               }).ToList();
            #endregion
            ViewBag.FirstDayOfWeek = FirstDayOfWeek;
            //ViewBag.IsLead = filter.IsLead;
            #endregion
            return PartialView("_EstimatorPartial", model);
        }

        public ActionResult EstimatorListPartial(int? CustomerId, EstimateFilter filter)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            //List<Estimator> model = new List<Estimator>();
            EstimatorDashboard model = new EstimatorDashboard();

            if (CustomerId.HasValue)
            {
                var customerInfo = _Util.Facade.CustomerFacade.GetCustomersById(CustomerId.Value);
                //var estimator = _Util.Facade.EstimatorFacade.GetEstimatorByCustomerId(customerInfo.CustomerId);

                if (customerInfo != null)
                {
                    model = _Util.Facade.EstimatorFacade.GetAllEstimatorListByCustomerIdAndCompanyId(customerInfo.CustomerId, CurrentUser.CompanyId.Value, filter);

                }
            }
            //ViewBag.IssLead = filter.IsLead;
            ViewBag.Status = filter.estimateStatus;
            return View(model);
        }
        [HttpPost]
        [Authorize]
        public JsonResult CloneEstimator(int EstimatorId)
        {

            string Status = "";
            bool IsApproved = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Estimator est = new Estimator();
            if (EstimatorId > 0)
            {
                 est = _Util.Facade.EstimatorFacade.GetEstimatorById(EstimatorId); 
            }
            else
            {
                return Json(new { result = false, message = "Estimate not found" });
            }
            
            string TempStatus = "";
            string OldEstimator = est.EstimatorId;
            
            #region validations
            if (est == null)
            {
                return Json(new { result = false, message = "Estimate not found." });
            }
            else if (Status == LabelHelper.EstimateStatus.Init)
            {
                return Json(new { result = false, message = "Estimate not found." });
            } 
            else if (est.CompanyId != CurrentUser.CompanyId.Value)
            {
                return Json(new { result = false, message = "Access denied." });
            }
            #endregion
            if (est != null)
            {
                //TempStatus = est.Status;
                est.Id = 0;
                est.IsApproved = false;
                est.CreatedDate = DateTime.Now.UTCCurrentTime();
                //est.StartDate = DateTime.Now.UTCCurrentTime();
                //est.CompletionDate = DateTime.Now.UTCCurrentTime();
                est.CreatedByName = User.Identity.Name;
                est.CreatedBy = CurrentUser.UserId;
                est.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                est.LastUpdatedBy = CurrentUser.UserId;
                est.Id = _Util.Facade.EstimatorFacade.InsertEstimator(est);
                est.EstimatorId = est.Id.GenerateEstimateNo(); 
                est.ParentEstimatorRef = "Cloned From " + OldEstimator;
                est.Status = LabelHelper.EstimateStatus.Open;
                _Util.Facade.EstimatorFacade.UpdateEstimator(est);
            } 
            List<EstimatorDetail> estDet = _Util.Facade.EstimatorFacade.GetEstimatorDetailListByEstimatorId(OldEstimator);
            List<EstimatorService> estservice = _Util.Facade.EstimatorFacade.GetEstimatorServicesByEstimatorId(OldEstimator);
            
            string EstimatorName = ""; 
             
            #region Insert Estimator Details
            foreach (var item in estDet)
            {
                item.EstimatorId = est.EstimatorId;
                item.CreatedDate = DateTime.Now.UTCCurrentTime();
                item.CreatedBy = CurrentUser.UserId;
                item.Id = 0;
                _Util.Facade.EstimatorFacade.InsertEstimatorDetails(item);
            }
            #endregion
            #region Insert Estimator service
            foreach (var item in estservice)
            {
                item.EstimatorId = est.EstimatorId;
                item.CreatedDate = DateTime.Now.UTCCurrentTime();
                item.CreatedBy = CurrentUser.UserId;
                item.Id = 0;
                _Util.Facade.EstimatorFacade.InsertEstimatorService(item);
            }
            #endregion

            #region Insert Estimator File
            List<EstimatorFile> estimatorFiles = _Util.Facade.EstimatorFacade.GetByEstimatorFileByEstimatorId(OldEstimator);

            if (estimatorFiles != null && estimatorFiles.Count()>0)
            {
                foreach(var item in estimatorFiles)
                {
                    EstimatorFile estfile = new EstimatorFile()
                    {
                        Filename = item.Filename,
                        FileDescription = item.FileDescription,
                        UpdatedDate = DateTime.UtcNow,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = new Guid(),
                        UpdatedBy = new Guid(),
                        EstimatorId = est.EstimatorId,
                        FileSize = item.FileSize,
                        FileFullName = item.Filename,
                        IsActive = true,
                        EstimatorType = item.EstimatorType
                    };
                    _Util.Facade.CustomerAppoinmentFacade.InsertEstimatorFile(estfile);
                }
            } 
            #endregion End Estimator File
            base.AddUserActivityForCustomer("Estimate " + OldEstimator + " Cloned Successfully With New Id " + est.EstimatorId, LabelHelper.ActivityAction.Cloned, est.CustomerId, null, est.EstimatorId);
            return Json(new { result = true, message = string.Format("Estimate Cloned Successfully With New {1} Id {0}.", est.EstimatorId, EstimatorName) });
        }

        [Authorize]
        [HttpPost]
        public JsonResult EstimeApproveById(int Id)
        {
            
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            int cusid = 0;
            string EstimateId;
            var EstimatorInfo = _Util.Facade.EstimatorFacade.GetByEstimatorID(Id);

            var customerInfo = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(EstimatorInfo.CustomerId);
            var JsonResult = false;
            if (EstimatorInfo == null && customerInfo == null)
            {
                return Json(new { result = JsonResult, message = "Estimate Not found" });
            } 
            else
            {
                EstimatorInfo.Status = LabelHelper.EstimateStatus.Accepted;
                EstimatorInfo.IsApproved = true;
                JsonResult = _Util.Facade.EstimatorFacade.UpdateEstimator(EstimatorInfo);
            }
            if (EstimatorInfo.IsApproved)
            {
                try
                {
                    GlobalSetting GlobalSettingModel = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("EstimatorApprovedSMS");
                    GlobalSetting ApprovedEmail = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("EstimatorApprovedEmail");

                    if (GlobalSettingModel != null && GlobalSettingModel.Value.ToLower() == "true")
                    {
                        SendSMSToApprovedEstimator(customerInfo.Id, EstimatorInfo.EstimatorId,EstimatorInfo.Status);
                    }
                    if (ApprovedEmail != null && ApprovedEmail.Value.ToLower() == "true")
                    {
                        SendEmailToApprovedEstimator(customerInfo.Id, EstimatorInfo.EstimatorId, EstimatorInfo.Status);
                    }
                }
                catch(Exception ex)
                {
                    HsErrorLog.AddElmah(ex);
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\FileMailError.txt"), true))
                    {
                        file.WriteLine(ex.Message);
                        file.WriteLine(ex.StackTrace);
                        file.WriteLine(ex.InnerException);
                        file.Close();
                    }
                }
                
            }
            return Json(new { result = JsonResult, CustomerId = customerInfo.Id, EstimatorId = EstimatorInfo.EstimatorId, message = string.Format("Estimate {0} has been approved.", EstimatorInfo.EstimatorId) });
        }
        public bool SendSMSToApprovedEstimator(int? leadid,string EstimatorId,string Estimatorstatus)
        {
            string Status = "";
            //Estimator estimator = _Util.Facade.EstimatorFacade.GetByEstimatorId(EstimatorId);
            if(!string.IsNullOrWhiteSpace(Estimatorstatus))
            {
                if(Estimatorstatus == LabelHelper.EstimateStatus.Declined)
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
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                CompanyId = CurrentUser.CompanyId.Value;
                UserId = CurrentUser.UserId;
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

            #region Total RMR Calculation, Lead Source, Sales Location, Ticket type And Sales Group
            int customerid = 0;
            string SalesGroup = "";
            string SalesPerson = "";
            string SalesLocation = "";
            double UpFront = 0.0;
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
                PrefferedNO = PrefferedNO.Replace("-", "").Replace("(", "").Replace(")", "");
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
            if(!string.IsNullOrWhiteSpace(ReceiverNumber))
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
            string shortUrl2 = ""; 
            ShortUrl ShortUrl2 = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(CustomerLink, null);
            shortUrl2 = string.Concat(AppConfig.SiteDomain, "/shrt/", ShortUrl2.Code);  

            if (ReceiverNumberList.Count() > 0)
            {    
                bool sendResult = _Util.Facade.SMSFacade.SendEstimatorApprovedSMS(EstimatorId,CompanyId, ReceiverNumberList, false, string.Empty, UserId,Cus.Id, shortUrl2, Status);
                return sendResult;
            }
            else
            {
                return false;
            }
        }
        public bool SendEmailToApprovedEstimator(int? leadid, string EstimatorId, string Estimatorstatus)
        {
            Estimator estimator = _Util.Facade.EstimatorFacade.GetByEstimatorId(EstimatorId);
            if (!leadid.HasValue)
                return false;
            Guid CompanyId = new Guid();
            Guid UserId = Guid.Empty;
            if (User.Identity.IsAuthenticated)
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                CompanyId = CurrentUser.CompanyId.Value;
                UserId = CurrentUser.UserId;
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
                    if(_emp != null && !string.IsNullOrWhiteSpace(_emp.Email))
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
            string Filename = "";
            if (GlobalSettingModel != null && GlobalSettingModel.Value.ToLower() == "true")
            {
                PrefferedEmail = GlobalSettingModel.OptionalValue.Replace(" ", "");
            }
            else
            {
                return false;
            } 
            string[] PrefferedNOList = PrefferedEmail.Split(',');
            if (PrefferedNOList != null && PrefferedNOList.Length > 0)
            {
                for (int i = 0; i < PrefferedNOList.Length; i++)
                { 
                    ReceiverEmailList.Add(PrefferedNOList[i]);
                }
            }
            ReceiverEmailList = ReceiverEmailList.Distinct().ToList();
            #endregion 
            bool sendResult = false;
            string phonenumber = string.Join(";", ReceiverEmailList);
            string Status = "";
            if (estimator != null && (estimator.IsApproved ||  estimator.Status == LabelHelper.EstimateStatus.Declined))
            {
                if(!string.IsNullOrWhiteSpace(Estimatorstatus))
                {
                    if (Estimatorstatus == LabelHelper.EstimateStatus.Declined)
                    {
                        Status = "declined";
                    }
                    else
                    {
                        Status = "approved";
                    }
                } 
                 Filename = SaveEstimatorToPdf(null, estimator.Id);
            }
            string CustomerLink = AppConfig.DomainSitePath + string.Format("/Customer/Customerdetail/?id={0}", Cus.Id);
             CustomerLink = string.Concat(AppConfig.SiteDomain,CustomerLink); 
            if (ReceiverEmailList.Count() > 0)
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
                    try
                    {
                        sendResult = _Util.Facade.MailFacade.SendEstimatorApprovedEmail(email, CompanyId);
                    }
                    catch(Exception ex)
                    {
                        HsErrorLog.AddElmah(ex);
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\FileMailError.txt"), true))
                        {
                            file.WriteLine(ex.Message);
                            file.WriteLine(ex.StackTrace);
                            file.WriteLine(ex.InnerException);
                            file.Close();
                        }
                    } 
                }
                return sendResult; 
            }
            else
            {
                return false;
            }
        }

        public bool SendnotificationEmail(int? leadid, string EstimatorId, string Estimatorstatus)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Estimator estimator = _Util.Facade.EstimatorFacade.GetByEstimatorId(EstimatorId);
            if (!leadid.HasValue)
                return false;
            Guid CompanyId = new Guid();
            Guid UserId = Guid.Empty;
            if (User.Identity.IsAuthenticated)
            { 
                CompanyId = CurrentUser.CompanyId.Value;
                UserId = CurrentUser.UserId;
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
            #region ReceiverNumber Setup

            GlobalSetting GlobalSettingModel = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("EstimatorSent");

            string PrefferedEmail = "";
            string Filename = "";
            if (GlobalSettingModel != null && GlobalSettingModel.Value.ToLower() == "true")
            {
                PrefferedEmail = GlobalSettingModel.OptionalValue.Replace(" ", "");
            }
            else
            {
                return false;
            }
            string[] PrefferedNOList = PrefferedEmail.Split(',');
            if (PrefferedNOList != null && PrefferedNOList.Length > 0)
            {
                for (int i = 0; i < PrefferedNOList.Length; i++)
                {
                    ReceiverEmailList.Add(PrefferedNOList[i]);
                }
            }
            ReceiverEmailList = ReceiverEmailList.Distinct().ToList();
            #endregion 
            bool sendResult = false;
            string phonenumber = string.Join(";", ReceiverEmailList);
            string Status = "";
             
            Filename = '/' + Session[SessionKeys.EstimatorPdfSession].ToString(); 
            string CustomerLink = AppConfig.DomainSitePath + string.Format("/Customer/Customerdetail/?id={0}", Cus.Id);
            CustomerLink = string.Concat(AppConfig.SiteDomain, CustomerLink); 
            if (ReceiverEmailList.Count() > 0)
            {
                for (int i = 0; i < ReceiverEmailList.Count; i++)
                {
                    EstimatorApprovedEmail email = new EstimatorApprovedEmail()
                    {
                        EstimatorId = EstimatorId,
                        CustomerName = string.Concat(Cus.FirstName, " ", Cus.LastName),
                        UserName = string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName),
                        Notes = "Estimate" + EstimatorId + " sent",
                        ToEmail = ReceiverEmailList[i], 
                        EstimatorPdf = new Attachment(
                                     FileHelper.GetFileFullPath(Filename),
                                     MediaTypeNames.Application.Octet),
                        CustomerLink = CustomerLink,
                        Status = Status 
                    };
                    try
                    {
                        sendResult = _Util.Facade.MailFacade.SendEstimatorNotificationEmail(email, CompanyId);
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
                    }
                }
                return sendResult;
            }
            else
            {
                return false;
            }
        } 
        [Authorize]
        public ActionResult AddEstimator(int? id, int? CustomerId, string EstimatorId)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            CreateEstimator model = new CreateEstimator();


            GlobalSetting VendorPriceStutas = _Util.Facade.GlobalSettingsFacade.GetInvoiceVendorPriceSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
            if (VendorPriceStutas != null)
            {
                ViewBag.VendorPriceValue = VendorPriceStutas.IsActive.Value;
            }
            if ((id.HasValue && id.Value > 0) || !string.IsNullOrWhiteSpace(EstimatorId))
            {
                if (id.HasValue && id > 0)
                {
                    model.Estimator = _Util.Facade.EstimatorFacade.GetByEstimatorID(id.Value);
                }
                else
                {
                    model.Estimator = _Util.Facade.EstimatorFacade.GetByEstimatorId(EstimatorId);
                }
                if (model.Estimator == null || model.Estimator.CompanyId != currentLoggedIn.CompanyId.Value)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }

                model.Customer = _Util.Facade.CustomerFacade.GetAllCustomerByCustomerId(model.Estimator.CustomerId);
                if (model.Customer == null)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                model.Ticket =  _Util.Facade.TicketFacade.GetAllTicketEstimatorByCustomerId(model.Estimator.CustomerId);
                //model.estimatorDetails = _Util.Facade.EstimatorFacade.GetEstimatorDetailListByEstimatorId(model.Estimator.EstimatorId);
                model.estimatorDetails = _Util.Facade.EstimatorFacade.NewGetEstimatorDetailListByEstimatorId(model.Estimator.EstimatorId);
                model.estimatorServices = _Util.Facade.EstimatorFacade.GetEstimatorServicesByEstimatorId(model.Estimator.EstimatorId);
                model.estimatorOneTimeServices = _Util.Facade.EstimatorFacade.GetEstimatorOneTimeServicesByEstimatorId(model.Estimator.EstimatorId);

                //model.estimatorDetails = _Util.Facade.EstimatorFacade.GetEstimatorDetailListByEstimatorIdForPrint(model.Estimator.EstimatorId);

                if (model.estimatorDetails != null && model.estimatorDetails.Count > 0)
                {
                    foreach (var item in model.estimatorDetails)
                    {
                        item.EquipmentManufacturers = _Util.Facade.EquipmentFacade.GetEquipmentManufacturerListByEquipmentIdAndManufacturerId(item.EquipmentId, item.ManufacturerId);
                        item.Manufacturers = _Util.Facade.EquipmentFacade.GetManufacturersByEquipmentId(item.EquipmentId);
                    }
                }

                if (string.IsNullOrWhiteSpace(model.Estimator.EmailAddress))
                {
                    model.Estimator.EmailAddress = model.Customer.EmailAddress;
                }

                var shippingStutas = _Util.Facade.GlobalSettingsFacade.GetInvoiceShippingSettingValueByCompanyId(currentLoggedIn.CompanyId.Value);
                if (shippingStutas != null)
                {
                    ViewBag.value = shippingStutas.IsActive.Value;
                }
                model.PurchaseOrderWarehouse = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderWarehouseByEstimatorId(model.Estimator.EstimatorId);

            }
            else
            {
                if (CustomerId.HasValue && CustomerId > 0)
                {
                    model.Customer = _Util.Facade.CustomerFacade.GetById(CustomerId.Value);
                }

                if (model.Customer == null)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                float Rate = 0;
                bool Plan = false;
                bool Show = false;
                GlobalSetting ServicePlanRate = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("EstimatorSettingsServicePlanRate");
                if (ServicePlanRate != null)
                {
                    Rate = ServicePlanRate.Value.ToFloat();
                }
                GlobalSetting ServicePlan = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("EstimatorSettingsServicePlan");
                if (ServicePlan != null)
                {
                    Plan = ServicePlan.Value.ToBool();
                }
                GlobalSetting ShowService = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("EstimatorSettingsShowService");
                if (ShowService != null)
                {
                    Show = ShowService.Value.ToBool();
                }
                model.Estimator = new Estimator()
                {
                    CustomerId = model.Customer.CustomerId,
                    EmailAddress = model.Customer.EmailAddress,
                    CompanyId = currentLoggedIn.CompanyId.Value,
                    TaxAmount = 0,
                    TotalCost = 0,
                    TotalPrice = 0,
                    TotalProfitAmount = 0,
                    TotalOverheadCostAmount = 0,
                    OverheadCostPercentage = 0,
                    TaxPercnetage = 0,
                    PoriftPercentage = 0,
                    Status = "Init",
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    StartDate = DateTime.Now.UTCCurrentTime(),
                    CompletionDate = DateTime.Now.UTCCurrentTime(),
                    CreatedBy = currentLoggedIn.UserId,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    LastUpdatedBy = currentLoggedIn.UserId,
                    ServicePlanRate = Rate,
                    ShowServicePlan = Plan,
                    ShowService = Show


                };

                var AddressTemplate = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(currentLoggedIn.CompanyId.Value);
                model.Estimator.BillingAddress = AddressHelper.MakeCustomerAddress(model.Customer, "BillingAddress", AddressTemplate);
                model.Estimator.ProjectAddress = AddressHelper.MakeCustomerAddress(model.Customer, "ShippingAddress", AddressTemplate);
                if (model.Customer.ChildOf != Guid.Empty)
                {
                    Customer ParentCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.Customer.ChildOf);
                    if (ParentCustomer != null)
                    {
                        model.Estimator.ParentBillingAddress = AddressHelper.MakeCustomerAddress(ParentCustomer, "BillingAddress", AddressTemplate);
                    }
                }
                model.Estimator.IsApproved = false;
                model.Estimator.Id = _Util.Facade.EstimatorFacade.InsertEstimator(model.Estimator);
                model.Estimator.EstimatorId = model.Estimator.Id.GenerateEstimateNo();
                _Util.Facade.EstimatorFacade.UpdateEstimator(model.Estimator);

                model.EstimatorNotes = _Util.Facade.EstimatorFacade.GetAllEstimatorNoteByEstimatorIdAndCompanyId(model.Estimator.Id, currentLoggedIn.CompanyId.Value);
                model.estimatorDetails = new List<EstimatorDetail>();
                model.estimatorServices = new List<EstimatorService>();
                model.InvoiceDetailList = new List<InvoiceDetail>();
                ViewBag.Status =  model.Estimator.Status;
                model.Estimator.Status = "Open";
            }

            model._EstimatorPDFFilter = _Util.Facade.EstimatorFacade.GetEstimatorPdfFilterByComIdCusIdUserId(currentLoggedIn.CompanyId.Value, currentLoggedIn.UserId, model.Customer.CustomerId);
            if (model._EstimatorPDFFilter == null)
            {
                model._EstimatorPDFFilter = new EstimatorPDFFilter();
            }
            GlobalSetting OverheadRate = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "EstimatorOverheadRate");
            GlobalSetting ProfitRate = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "EstimatorProfitRate");

            if (model.Estimator.ServicePlanRate == null || model.Estimator.ShowServicePlan == null || model.Estimator.ShowService == null)
            {
                if (model.Estimator.ShowServicePlan == null)
                {
                    GlobalSetting ServicePlan = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("EstimatorSettingsServicePlan");
                    if (ServicePlan != null)
                    {
                        model.Estimator.ShowServicePlan = ServicePlan.Value.ToBool();
                    }
                }
                if (model.Estimator.ServicePlanRate == null)
                {
                    GlobalSetting ServicePlanRate = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("EstimatorSettingsServicePlanRate");
                    if (ServicePlanRate != null)
                    {
                        model.Estimator.ServicePlanRate = ServicePlanRate.Value.ToFloat();
                    }
                }
                if (model.Estimator.ShowService == null)
                {
                    GlobalSetting ShowService = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("EstimatorSettingsShowService");
                    if (ShowService != null)
                    {
                        model.Estimator.ShowService = ShowService.Value.ToBool();
                    }
                }
                _Util.Facade.EstimatorFacade.UpdateEstimator(model.Estimator);
            }




            #region Customer select
            List<SelectListItem> CustomerSelectList = new List<SelectListItem>();
            Customer childCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.Estimator.CustomerId);
            if (childCustomer != null)
            {
                CustomerSelectList.Add(new SelectListItem
                {
                    Text = string.IsNullOrWhiteSpace(childCustomer.BusinessName) ? (childCustomer.FirstName + " " + childCustomer.LastName) : childCustomer.BusinessName,
                    Value = childCustomer.CustomerId.ToString()
                });
            }
            ViewBag.CustomerSelect = CustomerSelectList;
            #endregion

            List<Supplier> supplier = _Util.Facade.SupplierFacade.GetAllSupplier().OrderBy(x => x.CompanyName).ToList();

            ViewBag.SupplierList = supplier.Select(x =>
              new SelectListItem()
              {
                  Text = x.CompanyName.ToString(),
                  Value = x.SupplierId.ToString()
              }).ToList(); 
            ViewBag.ManufacturerList = new List<SelectListItem>();
            ViewBag.EstimatorContractTerm = _Util.Facade.LookupFacade.GetLookupByKey("EstimatorContractTerm").Select(x =>
             new SelectListItem()
             {
                 Text = x.DisplayText.ToString(),
                 Value = x.DataValue.ToString()
             }).ToList();
            List<EquipmentType> EquipmentTypes = _Util.Facade.EquipmentTypeFacade.GetAllProductCategory();
            ViewBag.EquipmentTypeList = EquipmentTypes.Select(x =>
              new SelectListItem()
              {
                  Text = x.Name.ToString(),
                  Value = x.Id.ToString()
              }).ToList();
            ViewBag.EquipmentUnitList = _Util.Facade.LookupFacade.GetDropdownsByKey("EquipmentUnit");
            #region View for TaxList

            List<SelectListItem> TaxListItem = new List<SelectListItem>();
            var GetCityTaxList = _Util.Facade.CompanyBranchFacade.GetCityTaxRate(model.Customer.CustomerId, currentLoggedIn.CompanyId.Value);
            if (GetCityTaxList.Count > 0)
            {
                foreach (var item in GetCityTaxList)
                {
                    TaxListItem.Add(new SelectListItem()
                    {
                        Text = " [" + item.City.ToString() + "-" + item.State.ToString() + "]",
                        Value = item.Rate.ToString()
                    });
                }
            }
            else
            {
                Guid tempCustomerId = new Guid();

                var GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(currentLoggedIn.CompanyId.Value, tempCustomerId);
                if (GetSalesTax != null)
                {
                    TaxListItem.Add(new SelectListItem()
                    {
                        Text = GetSalesTax.SearchKey.ToString(),
                        Value = GetSalesTax.Value.ToString()
                    });
                }
            }
            TaxListItem.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("NonTaxValue").Select(x => new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());

            //ViewBag.StatusList = _Util.Facade.LookupFacade.GetLookupByKey("EstimatorStatus").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
            //              new SelectListItem()
            //              {
            //                  Text = x.DisplayText.ToString(),
            //                  Value = x.DataValue.ToString()
            //              }).ToList();
            ViewBag.StatusList = _Util.Facade.LookupFacade.GetLookupByKey("EstimatorStatus")
                .OrderBy(x => x.DisplayText != "Select One")
                .ThenBy(x => x.DisplayText)
                .Select(x => new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString(),
                    Disabled = x.DataValue == "Sent To Customer" ||
                   x.DataValue == "Resend To Customer" || 
                   x.DataValue == "Contract Signed" ||
                   x.DataValue == "Customer Viewed"
                })
                .ToList();

            ViewBag.Term = _Util.Facade.LookupFacade.GetLookupByKey("EstimateTerms").Select(x =>
              new SelectListItem()
              {
                  Text = x.DisplayText.ToString(),
                  Value = x.DataValue.ToString()
              }).ToList();

            ViewBag.EstimatorPaymentTerms = _Util.Facade.LookupFacade.GetLookupByKey("EstimatePaymentTerms").Select(x =>
             new SelectListItem()
             {
                 Text = x.DisplayText.ToString(),
                 Value = x.DataValue.ToString()
             }).ToList();
            var GetOutOfStateTax = _Util.Facade.GlobalSettingsFacade.GetOutOfStateTax(currentLoggedIn.CompanyId.Value);
            if (GetOutOfStateTax != null)
            {
                TaxListItem.Add(new SelectListItem()
                {
                    Text = GetOutOfStateTax.SearchKey.ToString(),
                    Value = GetOutOfStateTax.Value.ToString()
                });
            }
            var GetNonProfitTax = _Util.Facade.GlobalSettingsFacade.GetNonProfitTax(currentLoggedIn.CompanyId.Value);
            if (GetNonProfitTax != null)
            {
                TaxListItem.Add(new SelectListItem()
                {
                    Text = GetNonProfitTax.SearchKey.ToString(),
                    Value = GetNonProfitTax.Value.ToString()
                });
            }
            ViewBag.TaxListItem = TaxListItem;
            #endregion 
            ViewBag.EstimateMessage = _Util.Facade.GlobalSettingsFacade.GetEstimateByCompanyId(currentLoggedIn.CompanyId.Value);
            ViewBag.ServicePlanTypeList = _Util.Facade.LookupFacade.GetDropdownsByKey("ServicePlans");
            GlobalSetting contractTerm = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "EstimatorContractTerm");
            if (contractTerm != null)
            {
                model.ShowEstimatorContractTerm = contractTerm.Value.ToLower() == "true" ? true : false;
            }
            else
            {
                model.ShowEstimatorContractTerm = false;
            }
            ViewBag.ContractType = _Util.Facade.LookupFacade.GetDropdownsByKey("ContractTypeSummary");
            #region Contrcat Type
            CustomerExtended cusEx = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(model.Customer.CustomerId);
            if (cusEx != null)
            {

                ViewBag.ContractType_customer = cusEx.ContractType;
            }
            ViewBag.TaxExemption = model.Customer.TaxExemption;
            #endregion
            //ViewBag.POCreated = result;
            //model.Estimator.Status = model.Estimator.Status.Replace(" ", "");
                
            return PartialView("AddEstimator", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddEstimator(CreateEstimator Model)
        {
            #region Validations
            if (Model == null || Model.Estimator == null || string.IsNullOrWhiteSpace(Model.Estimator.EstimatorId))
            {
                return Json(new { result = false, message = "Estimate not found." });
            }
            GlobalSetting GlobalSettingModelSMS = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("EstimatorApprovedSMS");
            GlobalSetting ApprovedEmail = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("EstimatorApprovedEmail");

            Estimator tempEstimator = _Util.Facade.EstimatorFacade.GetEstimatorByEstimatorId(Model.Estimator.EstimatorId);
            if (tempEstimator == null)
            {
                return Json(new { result = false, message = "Estimate not found." });
            }
            string OldStatus = tempEstimator.Status;
            #endregion
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var customerinfo = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(tempEstimator.CustomerId);
             
            #region Update Estimator
            if (Model.Estimator.CustomerId != null && Model.Estimator.CustomerId != Guid.Empty)
            {
                tempEstimator.CustomerId = Model.Estimator.CustomerId;
            }
            
            tempEstimator.EmailAddress = Model.Estimator.EmailAddress;
            tempEstimator.StartDate = Model.Estimator.StartDate;
            tempEstimator.CompletionDate = Model.Estimator.CompletionDate;
            tempEstimator.BillingAddress = Model.Estimator.BillingAddress;
            tempEstimator.ProjectAddress = Model.Estimator.ProjectAddress;
            tempEstimator.ContractTerm = Model.Estimator.ContractTerm;
            tempEstimator.Description = Model.Estimator.Description;
            tempEstimator.TotalCost = Model.Estimator.TotalCost;
            tempEstimator.TotalOverheadCostAmount = Model.Estimator.TotalOverheadCostAmount;
            tempEstimator.TotalPrice = Model.Estimator.TotalPrice;
            tempEstimator.TotalProfitAmount = Model.Estimator.TotalProfitAmount;
            tempEstimator.TaxAmount = Model.Estimator.TaxAmount;
            tempEstimator.TaxPercnetage = Model.Estimator.TaxPercnetage;
            tempEstimator.DefaultProfitRate = Model.Estimator.DefaultProfitRate;
            tempEstimator.DefaultOverheadRate = Model.Estimator.DefaultOverheadRate;
            tempEstimator.Status = Model.Estimator.Status;
            tempEstimator.EstimateDate = Model.Estimator.EstimateDate;
            tempEstimator.ExpiresOn = Model.Estimator.ExpiresOn;
            tempEstimator.PaymentTerm = Model.Estimator.PaymentTerm;
            tempEstimator.ServicePlanType = Model.Estimator.ServicePlanType;
            tempEstimator.ServicePlanRate = Model.Estimator.ServicePlanRate;
            tempEstimator.ServicePlanAmount = Model.Estimator.ServicePlanAmount;
            tempEstimator.ServiceTaxAmount = Model.Estimator.ServiceTaxAmount;
            tempEstimator.ServiceTotalAmount = Model.Estimator.ServiceTotalAmount;
            tempEstimator.ActivationFee = Model.Estimator.ActivationFee;
            tempEstimator.OneTimeServiceTaxAmount = Model.Estimator.OneTimeServiceTaxAmount;
            tempEstimator.OneTimeServiceTotalAmount = Model.Estimator.OneTimeServiceTotalAmount;
            tempEstimator.DefaultMaterialMarkupRate = Model.Estimator.DefaultMaterialMarkupRate;
            //tempEstimator.ShowServicePlan = Plan;
            
            Model.Estimator.CustomerId = tempEstimator.CustomerId;
            if(Model.Estimator.Status == LabelHelper.EstimateStatus.Accepted)  
            {
                tempEstimator.Status = LabelHelper.EstimateStatus.Accepted;
                tempEstimator.IsApproved = true;
            }   
            else if (Model.Estimator.Status == LabelHelper.EstimateStatus.Open) 
            {
                tempEstimator.Status = LabelHelper.EstimateStatus.Open;
                tempEstimator.IsApproved = false;
            }
            else if (Model.Estimator.Status == LabelHelper.EstimateStatus.Declined)
            {
                tempEstimator.Status = LabelHelper.EstimateStatus.Declined;
                tempEstimator.IsApproved = false;
            }
            _Util.Facade.EstimatorFacade.UpdateEstimator(tempEstimator);
            #endregion
            #region Send SMS and Email
            if ((tempEstimator.IsApproved  || tempEstimator.Status == LabelHelper.EstimateStatus.Declined) && Model.PrintEstimator != true)
            { 
                if (GlobalSettingModelSMS != null && GlobalSettingModelSMS.Value.ToLower() == "true")
                {
                    SendSMSToApprovedEstimator(customerinfo.Id, tempEstimator.EstimatorId, tempEstimator.Status);
                }
                if (ApprovedEmail != null && ApprovedEmail.Value.ToLower() == "true")
                {
                    SendEmailToApprovedEstimator(customerinfo.Id, tempEstimator.EstimatorId, tempEstimator.Status);
                }
            }
            #endregion Send SMS and Email

            #region Log
            if (OldStatus != "Init")
            {
                base.AddUserActivityForCustomer(tempEstimator.EstimatorId + " Status Changed from " + OldStatus + " To " + tempEstimator.Status, LabelHelper.ActivityAction.Cloned, tempEstimator.CustomerId, null, tempEstimator.EstimatorId);
            }
            else
            {
                base.AddUserActivityForCustomer(" New Estimate Created #Ref: " + tempEstimator.EstimatorId, LabelHelper.ActivityAction.Cloned, tempEstimator.CustomerId, null, tempEstimator.EstimatorId);
            }
            #endregion Log

            #region Update Estimator Details
            _Util.Facade.EstimatorFacade.DeleteEstimatorDetailsByEstimatorId(Model.Estimator.EstimatorId);
            if (Model.estimatorDetails != null && Model.estimatorDetails.Count > 0)
            {
                foreach (EstimatorDetail item in Model.estimatorDetails)
                {
                    item.CreatedBy = CurrentUser.UserId;
                    item.CreatedDate = DateTime.Now.UTCCurrentTime();
                    item.EstimatorId = Model.Estimator.EstimatorId;
                    _Util.Facade.EstimatorFacade.InsertEstimatorDetails(item);
                }
            }

            #endregion
            #region Update Estimator Services
            _Util.Facade.EstimatorFacade.DeleteEstimatorOneTimeServiceByEstimatorId(Model.Estimator.EstimatorId);
            if (Model.estimatorOneTimeServices != null)
            {
                foreach (var item in Model.estimatorOneTimeServices)
                {

                    item.CreatedBy = CurrentUser.UserId;
                    item.CreatedDate = DateTime.Now.UTCCurrentTime();
                    item.EstimatorId = Model.Estimator.EstimatorId;
                    item.IsOneTimeService = true;
                    _Util.Facade.EstimatorFacade.InsertEstimatorService(item);
                }
            }

            #endregion

            #region Update Estimator Services
            _Util.Facade.EstimatorFacade.DeleteEstimatorServiceByEstimatorId(Model.Estimator.EstimatorId);
            if (Model.estimatorServices != null)
            {
                foreach (var item in Model.estimatorServices)
                {
                    item.CreatedBy = CurrentUser.UserId;
                    item.CreatedDate = DateTime.Now.UTCCurrentTime();
                    item.EstimatorId = Model.Estimator.EstimatorId;
                    item.IsOneTimeService = false;
                    _Util.Facade.EstimatorFacade.InsertEstimatorService(item);
                }
            }

            #endregion
            #region Create PO
            List<PurchaseOrderWarehouse> PurchaseOrderList = new List<PurchaseOrderWarehouse>();
            if (Model.CreatePO == "True")
            {

                List<Guid> SupplierIdList = Model.estimatorDetails.Where(x => x.CategoryVal != "Labor").Select(x => x.SupplierId).GroupBy(x => x).Select(x => x.Key).ToList();
                //var SupplierIdList = _Util.Facade.EstimatorFacade.GetAllSuplierIdByEstimatorId(Model.Estimator.EstimatorId);
                PurchaseOrderList = CreatePurchaseOrder(SupplierIdList, Model);
            }
            #endregion
            #region Message  
            string message = "";
            if (Model.CreatePO == "True")
            {
                message = "Estimate saved And PO Created successfully.";
            } 
            else
            {
                message = "Estimate saved successfully.";
            }
            #endregion
            return Json(new { result = true, message = message, PurchaseOrderList = PurchaseOrderList });
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteEstimator(string EstimatorId)
        {
            Estimator estimator = _Util.Facade.EstimatorFacade.GetByEstimatorId(EstimatorId);
            if (estimator == null)
            {
                return Json(new { result = false, message = "Estimate Not found" });
            }

            _Util.Facade.EstimatorFacade.DeleteEstimatorDetailsByEstimatorId(EstimatorId);
            _Util.Facade.EstimatorFacade.DeleteEstimatorServiceByEstimatorId(EstimatorId);
            _Util.Facade.EstimatorFacade.DeleteEstimatorByEstimatorId(EstimatorId);

            return Json(new { result = true, message = "Estimate deleted successfully." });
        }


        [Authorize]
        [HttpPost]
        public JsonResult EstimatorCustomerChange(Guid CustomerId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Customer customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            if (customer != null)
            {
                var AddressTemplate = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(CurrentUser.CompanyId.Value);
                string BillingAddress = AddressHelper.MakeCustomerAddress(customer, "BillingAddress", AddressTemplate);
                string ProjectAddress = AddressHelper.MakeCustomerAddress(customer, "ShippingAddress", AddressTemplate);

                return Json(new { result = true, BillingAddress = BillingAddress, ProjectAddress = ProjectAddress });
            }
            return Json(new { result = false });
        }



        public ActionResult GetEstimator(int Id)
        {
            return View();
        }

        private List<PurchaseOrderWarehouse> CreatePurchaseOrder(List<Guid> SupplierIdList, CreateEstimator Model)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            double TotalPOAmount = 0.0;
            string CompanyAddress = "";
            string SupplierAddress = "";
            string EquipmentDetail = "";
            var Company = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);
            if (Company.City != null && Company.City != "")
            {
                CompanyAddress = Company.Street + ' ' + Company.City + ',' + "<br>" + Company.State + ' ' + Company.ZipCode;
            }
            else
            {
                CompanyAddress = Company.Address;
            }
            foreach (Guid SupplierId in SupplierIdList)
            {
                foreach (var pod in Model.estimatorDetails.Where(x => x.SupplierId == SupplierId))
                {
                    TotalPOAmount += pod.TotalCost.HasValue ? pod.TotalCost.Value : 0.0;
                }

                if (SupplierId != Guid.Empty)
                {
                    var Supplier = _Util.Facade.SupplierFacade.GetSupplierBySupplierId(SupplierId);
                    if (Supplier != null && Supplier.City != null && Supplier.City != "")
                    {
                        SupplierAddress = Supplier.Street + ' ' + Supplier.City + ',' + "<br>" + Supplier.State + ' ' + Supplier.Zipcode;
                    }
                    else
                    {
                        SupplierAddress = Supplier.SupplierAddress;
                    }
                }


                CreatePurchaseOrder model = new CreatePurchaseOrder();
                string poPreText = "PO";
                GlobalSetting _poPretxt = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PrefixForPurchaseOrderId");
                if (_poPretxt != null && !string.IsNullOrWhiteSpace(_poPretxt.Value) && _poPretxt.Value != "")
                {
                    poPreText = _poPretxt.Value;
                }
                model.PurchaseOrderWarehouse = new PurchaseOrderWarehouse()
                {
                    SuplierId = SupplierId,
                    CompanyId = CurrentUser.CompanyId.Value,
                    Status = LabelHelper.PurchaseOrderStatus.Created,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    LastUpdatedByUid = CurrentUser.UserId,
                    BillingAddress = SupplierAddress,
                    Amount = TotalPOAmount,
                    TotalAmount = TotalPOAmount,
                    Tax = Model.Estimator.TaxAmount,
                    ShippingCost = TotalPOAmount,
                    ShippingAddress = CompanyAddress,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    OrderDate = DateTime.Now.UTCCurrentTime(),
                    CreatedByUid = CurrentUser.UserId,
                    EstimatorId = Model.Estimator.EstimatorId,
                    Description = Model.Estimator.EstimatorId
                };


                if (Model.Customer == null && Model.Estimator.CustomerId != Guid.Empty)
                {
                    Model.Customer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Estimator.CustomerId);
                }

                //if (Model.Customer != null)
                //{
                //    model.PurchaseOrderWarehouse.Description = string.Format(@"Customer ID: {0}
                //                    Estimator ID: {1}", Model.Customer.Id, Model.Estimator.EstimatorId);
                //}

                model.PurchaseOrderWarehouse.Id = _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderWarehouse(model.PurchaseOrderWarehouse);
                model.PurchaseOrderWarehouse.PurchaseOrderId = model.PurchaseOrderWarehouse.Id.GeneratePONo(poPreText);
                result = _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderWarehouse(model.PurchaseOrderWarehouse);

                foreach (var pod in Model.estimatorDetails.Where(x => x.SupplierId == SupplierId && x.CategoryVal != "Labor"))
                {
                    Equipment eq = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(pod.EquipmentId);
                    //EquipmentVendor EqVendor = _Util.Facade.EquipmentFacade.GetEquipmentVendorByEquipmentIdAndSupplierId(pod.EquipmentId,pod.SupplierId);
                    //if(EqVendor != null)
                    //{
                    //    if (EqVendor.SKU != null)
                    //    {
                    //        EquipmentDetail = EqVendor.SKU;
                    //    }
                    //    else
                    //    {
                    //        EquipmentDetail = pod.PartNumber;
                    //    }
                    //}
                    //else
                    //{
                    //    EquipmentDetail = pod.PartNumber;
                    //}
                    int Qty = Convert.ToInt32(Math.Round(pod.Qunatity.Value, MidpointRounding.AwayFromZero));
                    PurchaseOrderDetail purDetl = new PurchaseOrderDetail()
                    {
                        EquipmentId = eq.EquipmentId,
                        EquipName = eq.Name,
                        EquipDetail = pod.PartNumber,//EquipmentDetail,
                        Quantity = Qty,
                        UnitPrice = pod.UnitCost,
                        TotalPrice = pod.Qunatity * pod.UnitCost,
                        BundleId = 0,
                        PurchaseOrderId = model.PurchaseOrderWarehouse.PurchaseOrderId,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        CreatedBy = CurrentUser.UserId
                    };
                    _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderDetail(purDetl);
                }

                TotalPOAmount = 0.0;
                SupplierAddress = "";
            }
            List<PurchaseOrderWarehouse> PurchaseOrderList = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderWarehouseByEstimatorId(Model.Estimator.EstimatorId);

            return PurchaseOrderList;
        }

        [Authorize]
        [HttpPost]
        public JsonResult SaveEstimatorPdf(CreateEstimator Model, int? EstimatorId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string CategoryVal;
            string SupplierVal;

            #region EstimatorPdfFilter Save
            if (Model != null && Model._EstimatorPDFFilter.Id > 0)
            {
                EstimatorPDFFilter temestfil = _Util.Facade.EstimatorFacade.GetEstimatorPdfFilterById(Model._EstimatorPDFFilter.Id);
                if (temestfil != null)
                {
                    temestfil.GroupedbyNone = Model._EstimatorPDFFilter.GroupedbyNone;
                    temestfil.GroupedbyCategory = Model._EstimatorPDFFilter.GroupedbyCategory;
                    temestfil.GroupedbyLabor = Model._EstimatorPDFFilter.GroupedbyLabor;
                    temestfil.GroupedbyLaborAndMaterial = Model._EstimatorPDFFilter.GroupedbyLaborAndMaterial;
                    temestfil.GroupedbyMaterial = Model._EstimatorPDFFilter.GroupedbyMaterial;
                    temestfil.GroupedbySupplier = Model._EstimatorPDFFilter.GroupedbySupplier;
                    temestfil.IncludeCost = Model._EstimatorPDFFilter.IncludeCost;
                    temestfil.IncludeImage = Model._EstimatorPDFFilter.IncludeImage;
                    temestfil.IncludeManufacturer = Model._EstimatorPDFFilter.IncludeManufacturer;
                    temestfil.IncludeMargin = Model._EstimatorPDFFilter.IncludeMargin;
                    temestfil.IncludeOverhead = Model._EstimatorPDFFilter.IncludeOverhead;
                    temestfil.IncludePDF = Model._EstimatorPDFFilter.IncludePDF;
                    temestfil.IncludeProfit = Model._EstimatorPDFFilter.IncludeProfit;
                    temestfil.IncludeService = Model._EstimatorPDFFilter.IncludeService;
                    temestfil.WithoutIndividualLaborPricing = Model._EstimatorPDFFilter.WithoutIndividualLaborPricing;
                    temestfil.WithoutIndividualMaterialPricing = Model._EstimatorPDFFilter.WithoutIndividualMaterialPricing;
                    temestfil.WithoutPricing = Model._EstimatorPDFFilter.WithoutPricing;
                    temestfil.OneTimeService = Model._EstimatorPDFFilter.OneTimeService;
                    temestfil.IncludeVariation = Model._EstimatorPDFFilter.IncludeVariation;
                    _Util.Facade.EstimatorFacade.UpdateEstimatorPDFFilter(temestfil);
                }
            }
            else
            {
                Model._EstimatorPDFFilter.CompanyId = CurrentUser.CompanyId.Value;
                Model._EstimatorPDFFilter.CreatedBy = CurrentUser.UserId;
                Model._EstimatorPDFFilter.CreatedDate = DateTime.Now.ClientToUTCTime();
                Model._EstimatorPDFFilter.CustomerId = Model.Estimator.CustomerId;
                _Util.Facade.EstimatorFacade.InsertEstimatorPDFFilter(Model._EstimatorPDFFilter);
            }
            #endregion
            var estimatorFile = _Util.Facade.EstimatorFacade.GetByEstimatorFileByEstimatorId(Model.Estimator.EstimatorId);
            if(estimatorFile != null && estimatorFile.Count()>0)
            {
                Model.Estimator.EstimatorFileList = estimatorFile;
            }
            string Filename = "";
            if (EstimatorId.HasValue && EstimatorId > 0)
            {

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SaveInvoicePdf.txt"), true))
                {
                    file.WriteLine("SaveInvoiceToPdf going to be started");
                }
                Filename = SaveEstimatorToPdf(null, EstimatorId.Value);
            }
            else if (Model != null)
            {
                Model.Estimator.CompanyId = CurrentUser.CompanyId.Value;
                List<CreateEstimator> CreateInvoiceList = new List<CreateEstimator>();
                CreateInvoiceList.Add(Model);

                Filename = SaveEstimatorToPdf(Model, 0);
            }
             

            return Json(new { result = true, message = "Invoice Successfully Saved", filePath = Filename });

        }
        private string SaveEstimatorToPdf(CreateEstimator Model, int EstimatorId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);
            tempCom.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);
            CreateEstimator CreateEstimator = new CreateEstimator() { };
            string pdfname = "";
            string SalesGroup = "";
            string SalesPerson = "";
            string SalesLocation = "";
            string settingskey = @" 'InvoiceSettingsShipping', 'InvoiceSettingsDiscount', 'InvoiceSettingsDeposit'";
            //List<GlobalSetting> printsetting = _Util.Facade.GlobalSettingsFacade.GetInvoiceSettingListByCompanyIdAndKey(settingskey, CurrentUser.CompanyId.Value);
            GlobalSetting PaymentStubs = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "InvPreviewPaymentStubs");

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

                Model = new CreateEstimator();
                Model.EstimatorSetting = new EstimatorSetting();
                Model.Company = tempCom;

                Model.Estimator = _Util.Facade.EstimatorFacade.GetById(EstimatorId);
                Model._EstimatorPDFFilter = _Util.Facade.EstimatorFacade.GetEstimatorPdfFilterByComIdCusIdUserId(CurrentUser.CompanyId.Value, CurrentUser.UserId, Model.Estimator.CustomerId);
                if (Model._EstimatorPDFFilter == null)
                {
                    Model._EstimatorPDFFilter = new EstimatorPDFFilter();
                }
                //Model.estimatorDetails = _Util.Facade.EstimatorFacade.GetEstimatorDetailListByEstimatorId(Model.Estimator.EstimatorId);
                Model.estimatorDetails = _Util.Facade.EstimatorFacade.NewGetEstimatorDetailListByEstimatorId(Model.Estimator.EstimatorId);
                Model.estimatorServices = _Util.Facade.EstimatorFacade.GetEstimatorServicesByEstimatorId(Model.Estimator.EstimatorId);
                Model.estimatorOneTimeServices = _Util.Facade.EstimatorFacade.GetEstimatorOneTimeServicesByEstimatorId(Model.Estimator.EstimatorId);
                if (Model.Estimator == null || Model.Estimator.CompanyId != CurrentUser.CompanyId.Value)
                {
                    return null;
                }
                if ((Model.estimatorDetails == null || Model.estimatorDetails.Count() == 0) && (Model.estimatorServices == null || Model.estimatorServices.Count() == 0) && (Model.estimatorOneTimeServices == null || Model.estimatorOneTimeServices.Count() == 0))
                {
                    return null;
                }
                Customer tempCUstomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Estimator.CustomerId);
                if (tempCUstomer == null)
                {
                    return null;
                }

                CreateEstimator processedModel = GetEstimatorModelById(Model.Estimator, Model.estimatorDetails, Model.estimatorServices, tempCom, tempCUstomer, Model._EstimatorPDFFilter, Model.estimatorOneTimeServices);
                CreateEstimator = processedModel;

                pdfname = EstimatorId.GenerateEstimateNo();

            }
            else if (Model != null)
            {
                if ((Model.estimatorDetails == null || Model.estimatorDetails.Count() == 0) && (Model.estimatorServices == null || Model.estimatorServices.Count() == 0) && (Model.estimatorOneTimeServices == null || Model.estimatorOneTimeServices.Count() == 0))
                {
                    return null;
                }
                Customer tempCUstomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Estimator.CustomerId);
                if (tempCUstomer == null)
                { 
                    return null;
                } 
                CreateEstimator processedModel = GetEstimatorModelById(Model.Estimator, Model.estimatorDetails, Model.estimatorServices, tempCom, tempCUstomer, Model._EstimatorPDFFilter, Model.estimatorOneTimeServices);
                Estimator estimator = _Util.Facade.EstimatorFacade.GetEstimatorByEstimatorId(Model.Estimator.EstimatorId);
                var estimatorFiledata = _Util.Facade.EstimatorFacade.GetByEstimatorFileByEstimatorId(Model.Estimator.EstimatorId);
                if (estimator != null)
                {
                    ViewBag.CoverLetter = estimator.CoverLetter;
                    processedModel.Estimator.CoverLetter = estimator.CoverLetter;
                    processedModel.Estimator.CoverLetterFile = estimator.CoverLetterFile;
                    processedModel.Estimator.EstimatorFileList = estimatorFiledata;
                    processedModel.Estimator.ServicePlanType = estimator.ServicePlanType;
                    processedModel.Estimator.ServicePlanRate = estimator.ServicePlanRate;
                    processedModel.Estimator.ServicePlanAmount = estimator.ServicePlanAmount;
                    processedModel.Estimator.ServiceTaxAmount = estimator.ServiceTaxAmount;
                    processedModel.Estimator.OneTimeServiceTaxAmount = estimator.OneTimeServiceTaxAmount;
                    processedModel.Estimator.ServiceTotalAmount = estimator.ServiceTotalAmount;
                    processedModel.Estimator.OneTimeServiceTotalAmount = estimator.OneTimeServiceTotalAmount; 
                    processedModel.Estimator.ShowServicePlan = estimator.ShowServicePlan;
                    processedModel.Estimator.ShowService = estimator.ShowService;
                    processedModel.Estimator.ServicePlanTypeName = "Service Plan";

                    SelectListItem selectListItem = _Util.Facade.LookupFacade.GetDropdownsByKey("ServicePlans").Where(x => x.Value == estimator.ServicePlanType).FirstOrDefault();
                    if (selectListItem != null)
                    {
                        processedModel.Estimator.ServicePlanTypeName = selectListItem.Text;
                    }

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
            }

            if (CreateEstimator == null)
            {
                return null;
            }


            ViewBag.CompanyId = tempCom.CompanyId.ToString();
            ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("~/Views/Estimator/EstimatorPdf.cshtml", CreateEstimator)
            { 
                PageSize = Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },

            };
            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
            string filename = ConfigurationManager.AppSettings["File.EstimatorFiles"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(tempCom.CompanyId).CompanyName.ReplaceSpecialChar();
            filename = string.Format(filename, comname);
            filename += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + pdfname + ".pdf";
            string Serverfilename = FileHelper.GetFileFullPath(filename);

            Session[SessionKeys.EstimatorPdfSession] = filename;
            FileHelper.SaveFile(applicationPDFData, Serverfilename);

            if (filename.IndexOf('/') != 0)
            {
                filename = "/" + filename;
            }


            #region ADD CoverLetter From PDF 
            try
            { 
                var outputFilePath = Server.MapPath(filename);
                var EndingpageFilePath = Server.MapPath("/Content/EstimatorCommEndPage/ProposalEndPage.pdf");
                var Defaultcoverpage = Server.MapPath("/Content/EstimatorCommEndPage/DefaultCoverpage.pdf");

                using (PdfDocument outPdf = new PdfDocument())
                {
                    HrDoc categoryDoc = new HrDoc();
                    Employee _emp = new Employee();
                    
                    if (!string.IsNullOrWhiteSpace(CreateEstimator.Soldby))
                    {
                        _emp = _Util.Facade.CustomerFacade.GetSalesGroupAndEmpNamBySoldby(new Guid(CreateEstimator.Soldby));  
                    } 
                    if (_emp != null && !string.IsNullOrWhiteSpace(_emp.UserName))
                    {
                        categoryDoc = _Util.Facade.HrDocFacade.GetHrDocByUsernameAndCategory(_emp.UserName, CurrentUser.CompanyId.Value, "ProfilePage");
                    }

                    if (categoryDoc != null && !string.IsNullOrWhiteSpace(categoryDoc.Filename))
                    {
                        var categoryDocPath = Server.MapPath(categoryDoc.Filename);
                        if (System.IO.File.Exists(categoryDocPath) && categoryDoc.Filename.ToLower().EndsWith(".pdf"))
                        { 
                            using (PdfDocument one = PdfReader.Open(categoryDocPath, PdfDocumentOpenMode.Import))
                            { 
                                CopyPages(one, outPdf); 
                            }
                        }
                        else
                        {

                            using (PdfDocument one = PdfReader.Open(Defaultcoverpage, PdfDocumentOpenMode.Import))
                            {
                                CopyPages(one, outPdf);

                            } 
                        }
                    }
                    else
                    {
                        using (PdfDocument one = PdfReader.Open(Defaultcoverpage, PdfDocumentOpenMode.Import))
                        {
                            CopyPages(one, outPdf);

                        }
                    }

                    if (Model.Estimator.EstimatorFileList != null && Model.Estimator.EstimatorFileList.Count() > 0)
                    {
                        var EstimatorFileEndpageList = Model.Estimator.EstimatorFileList.Where(x => x.EstimatorType == "EndPage").ToList();
                        foreach (var estitem in EstimatorFileEndpageList)
                        {
                            var estFilePath = Server.MapPath(estitem.FileDescription);
                            if (!string.IsNullOrWhiteSpace(estitem.FileDescription) &&
                               System.IO.File.Exists(estFilePath) && estitem.FileDescription.ToLower().EndsWith(".pdf"))
                            {
                                using (PdfDocument two = PdfReader.Open(estFilePath, PdfDocumentOpenMode.Import))
                                {
                                    CopyPages(two, outPdf);
                                }
                            }
                        }
                    } 
                    using (PdfDocument three = PdfReader.Open(outputFilePath, PdfDocumentOpenMode.Import))
                    { 
                        CopyPages(three, outPdf);
                    } 
                    using (PdfDocument four = PdfReader.Open(EndingpageFilePath, PdfDocumentOpenMode.Import))
                    { 
                        CopyPages(four, outPdf);
                            
                    }
                    outPdf.Save(outputFilePath);
                    outPdf.Close();
                    MemoryStream stream = new MemoryStream();
                    outPdf.Dispose();
                }
            }
            catch (Exception ex)
            { 
                Console.WriteLine("Error merging PDFs: " + ex.Message);
            }
             
            #endregion

            #region Include PDF
            if (Model._EstimatorPDFFilter != null && Model._EstimatorPDFFilter.IncludePDF.HasValue)
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
            return AppConfig.DomainSitePath + filename;
        }

        #region Estimator contract
        public ActionResult GetEstimatorContractPopup(int Id)
        {
            List<SelectListItem> estimatedoc = new List<SelectListItem>();
            estimatedoc.Add(new SelectListItem()
            {
                Text = "Select Document",
                Value = ""
            });
            estimatedoc.AddRange(_Util.Facade.FileFacade.GetAllContractAgreeemntTemplate().Select(x => new SelectListItem()
            {
                Text = x.Name.ToString(),
                Value = x.Id.ToString()
            }));
            ViewBag.multipledoc = estimatedoc;
            ViewBag.EstimatorIntId = Id;
            return View();
        }
        [Authorize]
        [HttpPost]
        public JsonResult OpenEstimatorContractReview(CreateEstimator Model, int? EstimatorId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            #region EstimatorPdfFilter Save
            if (Model != null && Model._EstimatorPDFFilter.Id > 0)
            {
                EstimatorPDFFilter temestfil = _Util.Facade.EstimatorFacade.GetEstimatorPdfFilterById(Model._EstimatorPDFFilter.Id);
                if (temestfil != null)
                {
                    temestfil.GroupedbyNone = Model._EstimatorPDFFilter.GroupedbyNone;
                    temestfil.GroupedbyCategory = Model._EstimatorPDFFilter.GroupedbyCategory;
                    temestfil.GroupedbyLabor = Model._EstimatorPDFFilter.GroupedbyLabor;
                    temestfil.GroupedbyLaborAndMaterial = Model._EstimatorPDFFilter.GroupedbyLaborAndMaterial;
                    temestfil.GroupedbyMaterial = Model._EstimatorPDFFilter.GroupedbyMaterial;
                    temestfil.GroupedbySupplier = Model._EstimatorPDFFilter.GroupedbySupplier;
                    temestfil.IncludeCost = Model._EstimatorPDFFilter.IncludeCost;
                    temestfil.IncludeImage = Model._EstimatorPDFFilter.IncludeImage;
                    temestfil.IncludeManufacturer = Model._EstimatorPDFFilter.IncludeManufacturer;
                    temestfil.IncludeMargin = Model._EstimatorPDFFilter.IncludeMargin;
                    temestfil.IncludeOverhead = Model._EstimatorPDFFilter.IncludeOverhead;
                    temestfil.IncludePDF = Model._EstimatorPDFFilter.IncludePDF;
                    temestfil.IncludeProfit = Model._EstimatorPDFFilter.IncludeProfit;
                    temestfil.IncludeService = Model._EstimatorPDFFilter.IncludeService;
                    temestfil.WithoutIndividualLaborPricing = Model._EstimatorPDFFilter.WithoutIndividualLaborPricing;
                    temestfil.WithoutIndividualMaterialPricing = Model._EstimatorPDFFilter.WithoutIndividualMaterialPricing;
                    temestfil.WithoutPricing = Model._EstimatorPDFFilter.WithoutPricing;
                    temestfil.IncludeService = Model._EstimatorPDFFilter.IncludeService;
                    temestfil.IncludeVariation = Model._EstimatorPDFFilter.IncludeVariation;
                    _Util.Facade.EstimatorFacade.UpdateEstimatorPDFFilter(temestfil);
                }
            }
            else
            {
                Model._EstimatorPDFFilter.CompanyId = CurrentUser.CompanyId.Value;
                Model._EstimatorPDFFilter.CreatedBy = CurrentUser.UserId;
                Model._EstimatorPDFFilter.CreatedDate = DateTime.Now.ClientToUTCTime();
                Model._EstimatorPDFFilter.CustomerId = Model.Estimator.CustomerId;
                _Util.Facade.EstimatorFacade.InsertEstimatorPDFFilter(Model._EstimatorPDFFilter);
            }
            #endregion

            return Json(new { result = true });
        }

        [Authorize]
        public ActionResult AddEstimatorAgreementtemplateReview(int Id, Guid cusId, int estId, int? customerintid)
        {
            ContractAgreementTemplate ft = new ContractAgreementTemplate();
            CustomerAgreementTemplate cft = new CustomerAgreementTemplate();
            if (Id > 0)
            {
                cft = _Util.Facade.FileFacade.GetCustomerAgreementTemplateByReferenceTemplateId(Id, cusId, false);
            }

            if (Id > 0 && cft == null)
            {
                ft = _Util.Facade.FileFacade.GetAgreementTemplateById(Id);
            }
            if (cft != null)
            {
                ft.Id = cft.ReferenceTemplateId.Value;
                ft.Name = cft.Name;
                ft.BodyContent = cft.BodyContent;
                ft.Description = cft.Description;
                ft.CompanyId = cft.CompanyId;
            }
            ViewBag.cusId = cusId;
            ViewBag.estId = estId;
            ViewBag.leadid = customerintid;
            return View(ft);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddEstimatorAgreementtemplateReview(CustomerAgreementTemplate ft)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //if (ft.leadid > 0)
            //{
            //    Customer temCus = _Util.Facade.CustomerFacade.GetCustomerById(ft.leadid);
            //    ft.CustomerId = temCus.CustomerId;
            //}
            CustomerAgreementTemplate tempet = new CustomerAgreementTemplate();
            if (ft.ReferenceTemplateId > 0 && ft.cusId != null)
            {
                tempet = _Util.Facade.FileFacade.GetCustomerAgreementTemplateByReferenceTemplateId(ft.ReferenceTemplateId.Value, ft.cusId, false);
            }
            if (ft.BodyContent == null)
            {
                return Json(new { result = false, message = "Please upload a file or fill up body content." });
            }
            //if (RestoreDefault && tempet != null)
            //{
            //    _Util.Facade.FileFacade.UpdateCustomerAgreementTemplate(tempet);
            //    return Json(new { result = true, message = "Agreement template restored to default successfully.", tempetId = tempet.Id });
            //}
            //else if (tempet != null)
            //{
            //    tempet.CompanyId = CurrentUser.CompanyId.Value;
            //    tempet.CustomerId = ft.CustomerId;
            //    tempet.ReferenceTemplateId = ft.ReferenceTemplateId;
            //    tempet.IsFileTemplate = false;
            //    tempet.LastUpdatedBy = CurrentUser.UserId;
            //    tempet.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            //    tempet.BodyContent = ft.BodyContent;
            //    if (!string.IsNullOrWhiteSpace(ft.Name) && ft.Name != "" && !string.IsNullOrWhiteSpace(ft.Description) && ft.Description != "")
            //    {
            //        tempet.Name = ft.Name;
            //        tempet.Description = ft.Description;
            //    }
            //    _Util.Facade.FileFacade.UpdateCustomerAgreementTemplate(tempet);
            //    return Json(new { result = true, message = "Agreement template updated successfully.", tempetId = tempet.Id });
            //}

            if (tempet == null || tempet != null)
            {
                tempet = new CustomerAgreementTemplate();
                tempet.CompanyId = CurrentUser.CompanyId.Value;
                tempet.ReferenceTemplateId = ft.ReferenceTemplateId;
                tempet.CustomerId = ft.cusId;
                tempet.Name = ft.Name;
                tempet.IsFileTemplate = false;
                tempet.BodyContent = ft.BodyContent;
                tempet.Description = ft.Description;
                tempet.CreatedBy = CurrentUser.UserId;
                tempet.CreatedDate = DateTime.Now.UTCCurrentTime();
                tempet.LastUpdatedBy = CurrentUser.UserId;
                tempet.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                long temId = _Util.Facade.FileFacade.InsertCustomerAgreementTemplate(tempet);
                return Json(new { result = true, tempetId = tempet.Id, message = "Agreement template saved successfully." });
            }
            else
            {
                return Json(new { result = false, message = "Agreement template saved failed." });
            }

        } 
        #endregion
        void CopyPages(PdfSharp.Pdf.PdfDocument from, PdfSharp.Pdf.PdfDocument to)
        {
            for (int i = 0; i < from.PageCount; i++)
            {
                to.AddPage(from.Pages[i]);
            }
        }

        private CreateEstimator GetEstimatorModelById(Estimator Invoice, List<EstimatorDetail> InvoiceDetialList, List<EstimatorService> EstimatorServiceList, Company tempCom, Customer tempCUstomer, EstimatorPDFFilter EstimatorPDFFilters, List<EstimatorService> EstimatorOneTimeServiceList)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CreateEstimator Model = new CreateEstimator();
            Model.Estimator = Invoice;
            Model.estimatorDetails = InvoiceDetialList;
            Model.estimatorServices = EstimatorServiceList;
            Model.estimatorOneTimeServices = EstimatorOneTimeServiceList;
            Model.Estimator.CustomerName = tempCUstomer.Title + " " + tempCUstomer.FirstName + " " + tempCUstomer.LastName;

            //  Model.Invoice.IsEstimate = false;


            //Model.Invoice.InvoiceDate = Invoice.InvoiceDate.HasValue ? Invoice.InvoiceDate.Value : Model.Invoice.InvoiceDate.Value.ClientToUTCTime();
            //if (!string.IsNullOrWhiteSpace(Model.Invoice.EstimateTerm) && Model.Invoice.EstimateTerm != "-1")
            //    Model.Invoice.EstimateTerm = _Util.Facade.LookupFacade.GetDisplayTextByDataValueFromLLookup(Model.Invoice.EstimateTerm);
            //Model.Invoice.DueDate = Invoice.DueDate.HasValue ? Invoice.DueDate.Value : Model.Invoice.DueDate.Value.ClientToUTCTime();
            #region Discount Calculation 
            //if (!string.IsNullOrWhiteSpace(Model.Invoice.DiscountType))
            //{
            //    if (Model.Invoice.DiscountType == "amount")
            //    {
            //        if (Invoice.Discountpercent != null)
            //        {
            //            Model.Discount = Invoice.Discountpercent.Value;
            //        }
            //    }
            //    else
            //    {
            //        if (Invoice.Discountpercent != null)
            //        {
            //            Model.Discount = ((Invoice.Discountpercent / 100) * Model.SubTotal).Value;
            //        }
            //    }
            //}
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

            //customer name is customer business name here 
            if (tempCUstomer.BusinessName != "")
            {
                Model.CustomerName = tempCUstomer.BusinessName;
            }
            else
            {
                Model.CustomerName = Model.Estimator.CustomerName;
            }
            Model.CusBussinessName = tempCUstomer.BusinessName;
            Model.Soldby = tempCUstomer.Soldby;
            Model.SubTotal = 0;
            if (Model.estimatorDetails != null)
            {
                foreach (var item in Model.estimatorDetails)
                {
                    //   item.CreatedBy = User.Identity.Name;
                    item.CreatedDate = DateTime.Now.UTCCurrentTime();
                    //   item.CompanyId = CurrentUser.CompanyId.Value;
                    Model.SubTotal = Model.SubTotal + item.TotalPrice.Value;
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
                ComCity = tempCom.City;//tempCom.City.UppercaseFirst() + ", ";
            }
            if (!string.IsNullOrWhiteSpace(tempCom.State))
            {
                ComState = tempCom.State + " ";
            }
            #region Company Info
            Model.companyStreetInfo = ComCity + ComState + tempCom.ZipCode;
            Model.CompanyCity = tempCom.City;//.UppercaseFirst();
            Model.CompanyState = tempCom.State;
            Model.CompanyZip = tempCom.ZipCode;
            Model.CompanyPhone = tempCom.Phone;
            Model.CompanyEmail = tempCom.EmailAdress;
            Model.CompanyName = tempCom.CompanyName;
            Model.PhoneNo = tempCom.Phone;
            Model.CompanyWebsite = tempCom.Website;
            #endregion
            #region Customer Info
            Model.CustomerInfo = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(CurrentUser.CompanyId.Value);
            Model.CustomerCity = tempCUstomer.City.UppercaseFirst();
            Model.CustomerState = tempCUstomer.State;
            Model.CustomerZipCode = tempCUstomer.ZipCode;
            Model.CustomerNo = tempCUstomer.CustomerNo;
            Model.CustomerStreet = tempCUstomer.Street;
            Model.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CurrentUser.CompanyId.Value);
            //ViewBag.ShippingValue = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(tempCom.CompanyId);
            #endregion
            Model._EstimatorPDFFilter = EstimatorPDFFilters;

            Model.ShowEstimatorShippingAddress = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(CurrentUser.CompanyId.Value).ToLower() == "true" ? true : false;

            if (string.IsNullOrWhiteSpace(tempCom.CompanyLogo))
            {
                tempCom.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);
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
        [Authorize]
        public JsonResult GetEquipmentListByKey(string key, Guid? SupplierId, int? CategoryId)
        {
            string result = "[]";
            if (!string.IsNullOrWhiteSpace(key))
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                int ItemsLoadCount = _Util.Facade.GlobalSettingsFacade.GetEquipmentSearchMaxLoad(CurrentUser.CompanyId.Value);

                double DefaultProfitRate = _Util.Facade.GlobalSettingsFacade.GetDefaultProfitRate(CurrentUser.CompanyId.Value);
                double DefaultOverHeadRate = _Util.Facade.GlobalSettingsFacade.GetDefaultOverHeadRate(CurrentUser.CompanyId.Value);


                List<EquipmentSearchModelEstimator> EqList = _Util.Facade.EstimatorFacade.GetEqupmentListBySearchKeySupplierIdAndCategory(key, ItemsLoadCount, SupplierId, CategoryId, DefaultProfitRate, DefaultOverHeadRate);
                foreach (var item in EqList)
                {
                    if (item.ProfitRate == -1)
                    {
                        if (item.EquipmentType == "Labor")
                        {
                            item.ProfitRate = DefaultProfitRate;
                        }
                        else
                        {
                            item.ProfitRate = 0;
                        }
                    }
                }
                if (EqList.Count > 0)
                    result = JsonConvert.SerializeObject(EqList);
            }

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult AddCoverLetter(string EstimatorId,string EstimatorType)
        {
            string BodyContent = "";
            string CoverLetterFile = "";
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<EstimatorFile> estimatorFiles = new List<EstimatorFile>();
            if (EstimatorId != null)
            {
                 estimatorFiles = _Util.Facade.EstimatorFacade.GetByEstimatorFileByEstimatorIdAndEstimatorType(EstimatorId,EstimatorType);
                var estimator = _Util.Facade.EstimatorFacade.GetByEstimatorId(EstimatorId);
                if (estimator.CoverLetter != null && estimator.CoverLetter != "")
                {
                    BodyContent = estimator.CoverLetter;
                }
                if (estimator.CoverLetterFile != null && estimator.CoverLetterFile != "")
                {
                    CoverLetterFile = estimator.CoverLetterFile;
                }
                else
                {
                    var cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(estimator.CustomerId);
                    var com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(estimator.CompanyId);
                    string branch = _Util.Facade.CompanyBranchFacade.GetCompanyEmailLogoByCompanyId(estimator.CompanyId);

                    CoverLetter CL = new CoverLetter()
                    {
                        CustomerName = cus.FirstName + ' ' + cus.LastName,
                        SalesGuy = CurrentUser.FirstName + ' ' + CurrentUser.LastName,
                        CompanyName = com.CompanyName
                    };
                    EmailTemplate et = new EmailTemplate();
                    et = _Util.Facade.MailFacade.GetTemplateByTemplateKey(EmailTemplateKey.CoverLetter.EstimatorPredefineEmailTemplate);

                    Hashtable templateVars = new Hashtable();

                    templateVars.Add("CustomerName", CL.CustomerName);
                    templateVars.Add("SalesGuy", CL.SalesGuy);
                    templateVars.Add("CompanyName", CL.CompanyName);
                    BodyContent = HS.Web.UI.Helper.LabelHelper.ParserHelper(et.BodyContent, templateVars);
                }

                ViewBag.BodyContent = BodyContent;
                ViewBag.EstimatorId = EstimatorId;
                ViewBag.CoverLetterFile = CoverLetterFile;
                ViewBag.EstimatorType = EstimatorType;
                if(estimatorFiles != null && estimatorFiles.Count()>0)
                {
                    ViewBag.EstimatorFile = estimatorFiles;
                } 
            } 
            return View(estimatorFiles);
        } 
        [Authorize]
        [HttpPost]
        public JsonResult GetManufacturerSKUByEquipmentIdAndManuId(Guid EquipmentId, Guid ManufacturerId, int RowNumber)
        {
            EquipmentManufacturer equipmentManufacturer = _Util.Facade.EquipmentFacade.GetEquipmentManufacturerByEquipmentIdAndManufacturerId(EquipmentId, ManufacturerId);
            if (equipmentManufacturer != null)
            {
                return Json(new { result = true, SKU = equipmentManufacturer.SKU, RowNumber = RowNumber });
            }
            if (EquipmentId != Guid.Empty)
            {
                Equipment equipment = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(EquipmentId);
                if (equipment != null)
                {
                    return Json(new { result = true, SKU = equipment.SKU, RowNumber = RowNumber });
                }
            }
            return Json(new { result = false });
        }
        [Authorize]
        [HttpPost]
        public JsonResult GetManuSKUByEquipIdAndManuId(Guid EquipmentId, Guid ManufacturerId, int RowNumber)
        {
            List<EquipmentManufacturer> EMList = _Util.Facade.EquipmentFacade.GetEquipmentManufacturerListByEquipmentIdAndManufacturerId(EquipmentId, ManufacturerId);
            if (EMList != null)
            {
                return Json(new { result = true, EMList = EMList, RowNumber = RowNumber });
            }

            return Json(new { result = false });
        }


        [Authorize]
        public JsonResult SaveCoverLetter(string EstimatorId, string BodyContent, List<EstimatorFile> CoverLetterFile,string EstimatorType)
        {
            bool result = false;
            bool DeleteEstimatorFile = false;
            if (!string.IsNullOrWhiteSpace(EstimatorId))
            {
                var ExistEstimatorFiledata = _Util.Facade.EstimatorFacade.GetEstimatorFileByEstimatorId(EstimatorId);
                if(ExistEstimatorFiledata != null)
                {
                    DeleteEstimatorFile = _Util.Facade.CustomerAppoinmentFacade.DeleteEstimatorFileById(ExistEstimatorFiledata.EstimatorId,EstimatorType);
                } 
            } 
            var ExistEstimator = _Util.Facade.EstimatorFacade.GetEstimatorByEstimatorId(EstimatorId); 
            if (ExistEstimator == null)
            {
                return Json(new { result = false, message = "Estimate not found." });
            }
            else
            {
                ExistEstimator.CoverLetter = BodyContent;
                if (CoverLetterFile != null && CoverLetterFile.Count()>0)
                {
                    //if(DeleteEstimatorFile)
                    //{
                        foreach (var item in CoverLetterFile)
                        {
                            EstimatorFile ImageModel = new EstimatorFile()
                            {
                                Filename = item.Filename,
                                FileDescription = item.FileDescription,
                                UpdatedDate = DateTime.UtcNow,
                                CreatedDate = DateTime.UtcNow,
                                CreatedBy = new Guid(),
                                UpdatedBy = new Guid(),
                                EstimatorId = EstimatorId,
                                FileSize = item.FileSize,
                                FileFullName = item.Filename,
                                IsActive = true,
                                EstimatorType = EstimatorType
                            };
                            _Util.Facade.CustomerAppoinmentFacade.InsertEstimatorFile(ImageModel);
                        }
                    //}
                    
                }
                else
                {
                    result = _Util.Facade.EstimatorFacade.UpdateEstimator(ExistEstimator);
                }
               //result = _Util.Facade.EstimatorFacade.UpdateEstimator(ExistEstimator);
            }


            return Json(result);

        }

        public ActionResult EstimatorFilter(int Id)
        {
            Estimator model = new Estimator();
            if (Id > 0)
            {
                model = _Util.Facade.EstimatorFacade.GetEstimatorById(Id);
                if (model != null)
                {
                    model.EstimatorService = _Util.Facade.EstimatorFacade.GetEstimatorServicesByEstimatorId(model.EstimatorId);
                }
            }
            return View("EstimatorFilter", model);
        }



        public ActionResult SendEmailEstimator(int Id, string EmailAddress)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CreateCustomerEstimator model = new CreateCustomerEstimator();
            model.Estimator = _Util.Facade.EstimatorFacade.GetEstimatorById(Id);
            model.EstimatorId = model.Estimator.EstimatorId;
            model.CustomerId = model.Estimator.CustomerId;
            var objcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(model.Estimator.CustomerId);
            var objcom = _Util.Facade.CompanyFacade.GetAllCompanyByCompanyId(CurrentUser.CompanyId.Value).FirstOrDefault();
            if (objcus != null)
            {
                model.CustomerName = objcus.FirstName + " " + objcus.LastName;
                model.CustomerEmailAddress = string.IsNullOrWhiteSpace(EmailAddress) ? objcus.EmailAddress : EmailAddress;

                if (!string.IsNullOrWhiteSpace(objcus.CellNo) && objcus.CellNo.Length > 6)
                {
                    model.CustomerContactNumber = objcus.CellNo;
                }
                else if (!string.IsNullOrWhiteSpace(objcus.PrimaryPhone) && objcus.PrimaryPhone.Length > 6)
                {
                    model.CustomerContactNumber = objcus.PrimaryPhone;
                }
                else if (!string.IsNullOrWhiteSpace(objcus.SecondaryPhone) && objcus.SecondaryPhone.Length > 6)
                {
                    model.CustomerContactNumber = objcus.SecondaryPhone;
                }
            }
            if (objcom != null)
            {
                model.CompanyName = objcom.CompanyName;
                model.CompanyEmail = objcom.EmailAdress;
            }
            model.ShortUrl = string.Concat(AppConfig.SiteDomain, "/shrt/");
            if (Session[SessionKeys.EstimatorPdfSession] != null)
            {
                ViewBag.pdfLocation = AppConfig.DomainSitePath + "/" + Session[SessionKeys.EstimatorPdfSession].ToString();
            }
            model.SMSBody = string.Concat("New Estimate from", " ", model.CompanyName, ": ", model.Estimator.EstimatorId, Environment.NewLine
                , Environment.NewLine, model.ShortUrl, "##url##");
            model.EmailTemplate = _Util.Facade.MailFacade.GetTemplateByTemplateKey("EstimatorEmailPredefineEmailTemplate");
            var objemp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
            if (objemp != null)
            {
                ViewBag.SalesGuy = objemp.FirstName + " " + objemp.LastName;
                if (!string.IsNullOrWhiteSpace(objemp.Phone))
                {
                    ViewBag.SalesPhone = objemp.Phone;
                }
                else
                {
                    ViewBag.SalesPhone = objcom.Phone;
                }
            }
            string filename = "";
            filename = '/' + Session[SessionKeys.EstimatorPdfSession].ToString();
            string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(model.Estimator.Id
                                        + "#"
                                        + CurrentUser.CompanyId.Value
                                        + "#"
                                        + model.CustomerId
                                        +"#"
                                        + filename);
            string fullurl = string.Concat(AppConfig.SiteDomain, "/Customer-Estimator/", encryptedurl);
            ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, model.CustomerId);
            ViewBag.url = AppConfig.SiteDomain + "/shrt/" + ShortUrl.Code;

            return View("SendEmailEstimator", model);
        }

        public JsonResult SendEstimatorInEmail(int EstimatorId, string BodyContent, string subject)
        {
            string Phone = "";
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var objcom = _Util.Facade.CompanyFacade.GetAllCompanyByCompanyId(CurrentUser.CompanyId.Value).FirstOrDefault();
            var objemp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
            if (objemp != null)
            {
                if (!string.IsNullOrWhiteSpace(objemp.Phone))
                {
                    Phone = objemp.Phone;
                }
                else
                {
                    if (objcom != null)
                    {
                        Phone = objcom.Phone;
                    }

                }
            }
            var CompanyName = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName;
            bool result = false;
            string body = "";
            if (EstimatorId != 0)
            {
                Estimator Estimator = _Util.Facade.EstimatorFacade.GetByEstimatorID(EstimatorId);
                var Estcustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Estimator.CustomerId);
                if (Estimator != null)
                {
                    Customer cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerGuidId(Estimator.CustomerId);

                    if (cus != null)
                    {
                        string filename = "";
                        CreateEstimator Model = new CreateEstimator();
                        Model.Estimator = Estimator;
                            Model.estimatorDetails = _Util.Facade.EstimatorFacade.GetEstimatorDetailListByEstimatorId(Estimator.EstimatorId);
                         

                        filename = '/' + Session[SessionKeys.EstimatorPdfSession].ToString(); 
                        int FileId = 0; 
                         
                        string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(Model.Estimator.Id
                            + "#"
                            + CurrentUser.CompanyId.Value
                            + "#"
                            + cus.CustomerId 
                            +"#"
                            + filename); 

                        string fullurl = string.Concat(AppConfig.SiteDomain, "/Customer-Estimator/", encryptedurl);
                        
                        ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, cus.CustomerId);

                        var predefinedemailtempobj = _Util.Facade.MailFacade.GetTemplateByTemplateKey("EstimatorEmailPredefineEmailTemplate");
                        if (predefinedemailtempobj != null)
                        { 
                            body = BodyContent; 
                        }
                        SendEstimatorInEmail SendEmail = new SendEstimatorInEmail()
                        {
                            CompanyName = CompanyName,
                            CustomerName = cus.FirstName + ' ' + cus.LastName,
                            EstimatorId = Estimator.EstimatorId,
                            ToEmail = cus.EmailAddress,
                            EmailBody = body,
                            ExpDate = Estimator.CompletionDate.HasValue ? Estimator.CompletionDate.Value.ToString("MM/dd/yy") : "",
                            FromEmail = CurrentUser.EmailAddress.IsValidEmailAddress() ? CurrentUser.EmailAddress : "info@rmrcloud.com",
                            FromName = string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName),
                            SalesGuy = CurrentUser.FirstName + ' ' + CurrentUser.LastName,
                            SalesPhone = Phone,
                            Subject = subject,
                            //Subject = "Estimator " + Estimator.EstimatorId + " from " + CompanyName,
                            Url = AppConfig.SiteDomain + filename,
                            EstimatorPdf = new Attachment(
                                                                 FileHelper.GetFileFullPath(filename),
                                                                 MediaTypeNames.Application.Octet)
                        };
                        result = _Util.Facade.MailFacade.SendEstimatorInEmail(SendEmail, CurrentUser.CompanyId.Value);
                        SendEmail.EstimatorPdf.Dispose();

                        if (Estimator.Status == LabelHelper.EstimateStatus.SentToCustomer)
                        {
                            Estimator.Status = LabelHelper.EstimateStatus.ResendToCustomer;
                        }
                        else if (Estimator.Status == LabelHelper.EstimateStatus.ResendToCustomer)
                        {
                            Estimator.Status = LabelHelper.EstimateStatus.ResendToCustomer;
                        }
                        else if (Estimator.Status == LabelHelper.EstimatorStatus.ContractSingned)
                        {
                            Estimator.Status = LabelHelper.EstimateStatus.ContractSingned;
                        }
                        else
                        {
                            Estimator.Status = LabelHelper.EstimateStatus.SentToCustomer;
                        }
                        Estimator.CreatedDate = DateTime.Now.UTCCurrentTime();
                        //Estimator.StartDate = DateTime.Now.UTCCurrentTime();
                        //Estimator.CompletionDate = DateTime.Now.UTCCurrentTime();
                        Estimator.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                        _Util.Facade.EstimatorFacade.UpdateEstimator(Estimator);
                        
                        if(Estcustomer != null)
                        { 
                            Estcustomer.IsAgreement = false;
                            _Util.Facade.CustomerFacade.UpdateCustomer(Estcustomer); 
                        }
                        GlobalSetting ApprovedEmail = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("EstimatorSent");
                        if (ApprovedEmail != null && ApprovedEmail.Value.ToLower() == "true")
                        {
                            SendnotificationEmail(Estcustomer.Id, Estimator.EstimatorId, Estimator.Status);
                        }

                        string file = Estimator.EstimatorId;
                        decimal _fileSize = 1.00m;
                        string message = Estimator.Status;
                        _fileSize = (decimal)filename.Length / 1024;
                        _fileSize = Math.Round(_fileSize, 2, MidpointRounding.AwayFromZero);

                        CustomerFile cfs = new CustomerFile()
                        {
                            FileDescription = file + "_Mail" + ".pdf",
                            Filename =  filename,
                            FileSize = (double)_fileSize,
                            FileId = Guid.NewGuid(),
                            FileFullName = Regex.Replace(file, @"\s+", String.Empty) + ".pdf",
                            Uploadeddate = DateTime.Now.UTCCurrentTime(),
                            CustomerId = cus.CustomerId,
                            CompanyId = CurrentUser.CompanyId.Value,
                            IsActive = true,
                            CreatedBy = CurrentUser.UserId,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            UpdatedBy = CurrentUser.UserId,
                            UpdatedDate = DateTime.Now.UTCCurrentTime(),
                            WMStatus = LabelHelper.WatermarkStatus.Pending,
                            AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                        };
                        _Util.Facade.CustomerFileFacade.InsertCustomerFile(cfs);
                        string logMessage = Regex.Replace(file, @"\s+", String.Empty) + ".pdf " + message.ToLower();
                        base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, cus.Id, null);

                    }
                }
            }

            
            return Json(result);
        }


        public ActionResult EstimatorLeftSetting(string EstimatorId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            EstimatorSetting model = new EstimatorSetting();
            float Rate = 0;
            bool Service = false;
            bool Plan = false;
            if (!string.IsNullOrWhiteSpace(EstimatorId))
            {
                Estimator estimator = _Util.Facade.EstimatorFacade.GetEstimatorByEstimatorId(EstimatorId);
                if (estimator != null && estimator.ShowServicePlan != null && estimator.ServicePlanRate != null)
                {
                    model.ShowServicePlan = estimator.ShowServicePlan.Value;
                    model.ShowService = estimator.ShowService.Value;
                    model.ServicePlanRate = (float)estimator.ServicePlanRate.Value;
                    return PartialView("EstimatorLeftSetting", model);
                }
            }

            var ServicePlanRate = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("EstimatorSettingsServicePlanRate");
            if (ServicePlanRate != null)
            {
                Rate = ServicePlanRate.Value.ToFloat();
            }
            var ShowService = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("EstimatorSettingsShowService");
            if (ShowService != null)
            {
                Service = ShowService.Value.ToBool();
            }
            var ServicePlan = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("EstimatorSettingsServicePlan");
            if (ServicePlan != null)
            {
                Plan = ServicePlan.Value.ToBool();
            }
            model.ShowServicePlan = Plan;
            model.ServicePlanRate = Rate;
            model.ShowService = Service;

            return PartialView("EstimatorLeftSetting", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult ChangeEstimatorSetting(string Value, string Datakey, string EstimatorId)
        {
            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            if (Datakey == "EstimatorSettingsServicePlanRate")
            {
                var ServicePlanRate = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("EstimatorSettingsServicePlanRate");
                if (ServicePlanRate != null)
                {
                    ServicePlanRate.Value = Value;
                    result = _Util.Facade.GlobalSettingsFacade.UpdateGlobalSetting(ServicePlanRate);
                }
                if (!string.IsNullOrWhiteSpace(EstimatorId))
                {
                    Estimator estimator = _Util.Facade.EstimatorFacade.GetByEstimatorId(EstimatorId);
                    if (estimator != null)
                    {
                        estimator.ServicePlanRate = Value.ToFloat();
                        _Util.Facade.EstimatorFacade.UpdateEstimator(estimator);
                    }
                }
            }
            else if (Datakey == "EstimatorSettingsServicePlan")
            {
                var ServicePlan = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("EstimatorSettingsServicePlan");
                if (ServicePlan != null)
                {
                    ServicePlan.Value = Value;
                    result = _Util.Facade.GlobalSettingsFacade.UpdateGlobalSetting(ServicePlan);
                }
                if (!string.IsNullOrWhiteSpace(EstimatorId))
                {
                    Estimator estimator = _Util.Facade.EstimatorFacade.GetByEstimatorId(EstimatorId);
                    if (estimator != null)
                    {
                        estimator.ShowServicePlan = Value.ToBool();
                        _Util.Facade.EstimatorFacade.UpdateEstimator(estimator);
                    }
                }
            }
            else if (Datakey == "EstimatorSettingsShowService")
            {
                var ShowService = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByOnlyKey("EstimatorSettingsShowService");
                if (ShowService != null)
                {
                    ShowService.Value = Value;
                    result = _Util.Facade.GlobalSettingsFacade.UpdateGlobalSetting(ShowService);
                }
                if (!string.IsNullOrWhiteSpace(EstimatorId))
                {
                    Estimator estimator = _Util.Facade.EstimatorFacade.GetByEstimatorId(EstimatorId);
                    if (estimator != null)
                    {
                        estimator.ShowService = Value.ToBool();
                        _Util.Facade.EstimatorFacade.UpdateEstimator(estimator);
                    }
                }
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult GetEquipmentManufacturerWithPartNumber(Guid EquipmentId)
        {
            bool result = false;        
            if (EquipmentId != null && EquipmentId != new Guid())
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

                List<Manufacturer> Manufacturer = _Util.Facade.ManufacturerFacade.GetManufacturerListByEquipmentId(EquipmentId);
                if (Manufacturer != null)
                {
                    return Json(new { result = true, Manufacturer = Manufacturer });
                }
                //List<Manufacturer> Manufacturer = _Util.Facade.ManufacturerFacade.GetManufacturerListByEquipmentId(EquipmentId).OrderBy(x => x.Name).ToList();
                //ViewBag.ManufacturerList = Manufacturer.Select(x =>
                //  new SelectListItem()
                //  {
                //      Text = x.Name.ToString(),
                //      Value = x.ManufacturerId.ToString()
                //  }).ToList();

                //if (Manufacturer.Count > 0)
                //    result = JsonConvert.SerializeObject(Manufacturer);
            }

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }
    }
}