using HS.Entities;
using HS.Framework;
using HS.Framework.Utils;
using HS.Web.UI.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HS.Web.UI.Controllers
{
    public class CompanyBranchController : BaseController
    {
        // GET: CompanyBranch
        public ActionResult Index(int? id)
        {
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            if (id.HasValue)
            {
                ViewBag.id = id.Value;
            }
            else
            {
                ViewBag.id = 0;
            }
            return View();
        }
        [Authorize]
        public ActionResult CompanyBranchPartial()
        { 
            if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.BranchSetting))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<CompanyBranch> CompanyBranch = _Util.Facade.CompanyBranchFacade.GetAllCompanyBranchByCompanyId(currentLoggedIn.CompanyId.Value);
            return PartialView("CompanyBranchPartial", CompanyBranch);
        }

        [Authorize]
        public ActionResult AddCompanyBranch(int? id)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            CompanyBranch model;

            if (id.HasValue && id.Value>0)
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.BranchSettingEdit))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }

                model = _Util.Facade.CompanyBranchFacade.GetCompanyBranchById(id.Value);
                if (!model.IsMainBranch.HasValue)
                {
                    model.IsMainBranch = false;
                }
            }
            else
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.BranchSettingAdd))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }

                model = new CompanyBranch();
                model.IsMainBranch = false;
            }
            ViewBag.StateList = _Util.Facade.LookupFacade.GetLookupByKey("StateList").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();
            ViewBag.TimezoneList = _Util.Facade.LookupFacade.GetLookupByKey("TimeZone").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();
            ViewBag.DivisionList = _Util.Facade.LookupFacade.GetLookupByKey("Division").Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList();
            ViewBag.RegionList = _Util.Facade.LookupFacade.GetLookupByKey("Region").Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList();
            return PartialView("AddCompanyBranch", model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult UploadCompanyBranchLogo()
        {
            bool isUploaded = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            HttpPostedFileBase httpPostedFileBaseBranch = Request.Files["CompanyBranchFile"];

            string tempFolderName = ConfigurationManager.AppSettings["File.CompanyBranchFile"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            tempFolderName = string.Format(tempFolderName, comname);
            tempFolderName += DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += httpPostedFileBaseBranch.FileName.ReplaceSpecialCharFile();

            if (httpPostedFileBaseBranch != null && httpPostedFileBaseBranch.ContentLength != 0)
            {

                string tempFolderPath = Server.MapPath("~/" + tempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        httpPostedFileBaseBranch.SaveAs(Path.Combine(tempFolderPath, FileName));
                        isUploaded = true;
                    }
                    catch (Exception) {  /*TODO: You must process this exception.*/}
                }
            }

            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            return Json(new { isUploaded = isUploaded, filePath = AppConfig.DomainSitePath + filePath }, "text/html");
        }

        [Authorize]
        [HttpPost]
        public ActionResult UploadCompanyBranchColordLogo()
        {
            bool isUploaded = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            HttpPostedFileBase httpPostedFileBaseBranch = Request.Files["CompanyBranchFileColored"];

            string tempFolderName = ConfigurationManager.AppSettings["File.CompanyBranchFile"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            tempFolderName = string.Format(tempFolderName, comname);
            tempFolderName += DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += "CoL-___" + httpPostedFileBaseBranch.FileName;

            if (httpPostedFileBaseBranch != null && httpPostedFileBaseBranch.ContentLength != 0)
            {

                string tempFolderPath = Server.MapPath("~/" + tempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        httpPostedFileBaseBranch.SaveAs(Path.Combine(tempFolderPath, FileName));
                        isUploaded = true;
                    }
                    catch (Exception) {  /*TODO: You must process this exception.*/}
                }
            }

            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            return Json(new { isUploaded = isUploaded, filePath = AppConfig.DomainSitePath + filePath }, "text/html");
        }
        [Authorize]
        [HttpPost]
        public ActionResult UploadCompanyBranchEmailLogo()
        {
            bool isUploaded = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            HttpPostedFileBase httpPostedFileBaseBranch = Request.Files["CompanyBranchFileEmail"];

            string tempFolderName = ConfigurationManager.AppSettings["File.CompanyBranchFile"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            tempFolderName = string.Format(tempFolderName, comname);
            tempFolderName += DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += "EL-___" + httpPostedFileBaseBranch.FileName;

            if (httpPostedFileBaseBranch != null && httpPostedFileBaseBranch.ContentLength != 0)
            {

                string tempFolderPath = Server.MapPath("~/" + tempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        httpPostedFileBaseBranch.SaveAs(Path.Combine(tempFolderPath, FileName));
                        isUploaded = true;
                    }
                    catch (Exception) {  /*TODO: You must process this exception.*/}
                }
            }

            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            return Json(new { isUploaded = isUploaded, filePath = AppConfig.DomainSitePath + filePath }, "text/html");
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddCompanyBranch(CompanyBranch cb)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            cb.CompanyId = currentLoggedIn.CompanyId.Value; 

            if(cb.Id > 0)
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.BranchSettingEdit))
                {
                    return Json(false);
                }

                CompanyBranch branch = _Util.Facade.CompanyBranchFacade.GetCompanyBranchByIdAndCompanyId(currentLoggedIn.CompanyId.Value, cb.Id);
                 
                if(branch == null)
                {
                    cb.CompanyId = currentLoggedIn.CompanyId.Value;
                    cb.Id = (int)_Util.Facade.CompanyBranchFacade.InsertCompanyBranch(cb);
                }
                else
                {
                    cb.CompanyId = currentLoggedIn.CompanyId.Value;
                    _Util.Facade.CompanyBranchFacade.UpdateCompanyBranch(cb);
                }
            }
            else
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.BranchSettingAdd))
                {
                    return Json(false);
                }
                
                cb.CompanyId = currentLoggedIn.CompanyId.Value;

                cb.Id = (int) _Util.Facade.CompanyBranchFacade.InsertCompanyBranch(cb);
            }

            if(cb.Id>0 && cb.IsMainBranch.Value)
            {
                _Util.Facade.CompanyBranchFacade.RemoveAllMainBranchOfaCompanyExcludingId(currentLoggedIn.CompanyId.Value, cb.Id);
            }

            HttpRuntime.Cache.Remove(RMRCacheKey.EmailLogoUrl + currentLoggedIn.CompanyId.ToString());
            HttpRuntime.Cache.Remove(RMRCacheKey.CompanyLogoColored + currentLoggedIn.CompanyId.ToString());
            HttpRuntime.Cache.Remove(RMRCacheKey.CompanyBlacknWhiteLogo + currentLoggedIn.CompanyId.ToString());

            return Json(true);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteCompanyBranch(int? id)
        {
            if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.BrnachSettingDelete))
            {
                return Json(false);
            }

            if (id.HasValue)
            {
                var panelval = _Util.Facade.CompanyBranchFacade.DeleteCompanyBranch(id.Value);
            }

            return Json(true);
        }
    }
}