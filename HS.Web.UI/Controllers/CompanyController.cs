using HS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HS.Framework;
using HS.Web.UI.Helper;
using System.Configuration;
using System.IO;
using Rotativa;
using Rotativa.Options;
using System.Drawing;
using System.Net.Mime;
using System.Net.Mail;
using Newtonsoft.Json;
using System.Globalization;
using HS.Framework.Utils;
using HS.SMS;
using System.Drawing.Imaging;
using System.Collections;


namespace HS.Web.UI.Controllers
{
    public class CompanyController : BaseController
    {
        // GET: Company  
        public ActionResult Index()
        {
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            return View();
        }
        public ActionResult CreateResturant()
        {
          return PartialView("CreateResturant");
        }
        [Authorize]
        public ActionResult CompanyrPartial()
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            Company Model = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);
            if (String.IsNullOrWhiteSpace(Model.CompanyLogo))
            {
                var companybranchobj = _Util.Facade.CompanyBranchFacade.GetMainCompanyBranchByCompanyId(CurrentUser.CompanyId.Value);
                if(companybranchobj != null)
                {
                    Model.CompanyLogo = companybranchobj.Logo;
                }
            }

            return PartialView("CompanyrPartial", Model);
        }
        [HttpPost]
        //public ActionResult UploadCompanyFile(string CompanyId)
        //{
        //    bool isUploaded = false;
        //    var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
        //    HttpPostedFileBase httpPostedFileBase = Request.Files["CustomerFile"];

        //    string tempFolderName = ConfigurationManager.AppSettings["File.CustomerFiles"];
        //    var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
        //    tempFolderName = string.Format(tempFolderName, comname);
        //    tempFolderName += "/" + CompanyId.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

        //    Random rand = new Random();
        //    string FileName = rand.Next().ToString();
        //    FileName += "-___" + httpPostedFileBase.FileName;

        //    if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
        //    {

        //        string tempFolderPath = Server.MapPath("~/" + tempFolderName);

        //        if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
        //        {
        //            try
        //            {
        //                httpPostedFileBase.SaveAs(Path.Combine(tempFolderPath, FileName));
        //                isUploaded = true;
        //            }
        //            catch (Exception) {  /*TODO: You must process this exception.*/}
        //        }
        //    }
        //    string FullFilePath = "";
        //    string filePath = string.Concat("/", tempFolderName, "/", FileName);
        //    FullFilePath = ConfigurationManager.AppSettings["SiteDomain"] + filePath;
        //    return Json(new { isUploaded = isUploaded, filePath = filePath, FullFilePath = FullFilePath }, "text/html");
        //}

        [Authorize]
        public ActionResult AddCompanyBranch(int? Id)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            CompanyBranch Model = new CompanyBranch();
            if (Id.HasValue)
            {
                Model = _Util.Facade.CompanyBranchFacade.GetCompanyBranchByIdAndCompanyId(CurrentUser.CompanyId.Value,Id.Value);
            }

            ViewBag.StateList = ViewBag.StateList = _Util.Facade.LookupFacade.GetLookupByKey("StateList").Select(x =>
                           new SelectListItem()
                           {
                               Text = x.DisplayText.ToString(),
                               Value = x.DataValue.ToString()
                           }).ToList();



            return View("_AddCompanyBranchPartial");
        }

        [Authorize]
        public ActionResult AddCompany(int? id)
        {
            Company model;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (id.HasValue)
            {
                model = _Util.Facade.CompanyFacade.GetById(id.Value);
            }
            else
            {
                model = new Company();
            }
            return PartialView("AddCompany", model);
        }

        [Authorize]
        public ActionResult ShareCompanyFile(int? id)
        {
            ViewBag.companyFileId = id.Value;
            var currentUser = ((CustomPrincipal)(User));
            List<SelectListItem> permissions = new List<SelectListItem>();
            permissions.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1",
                Selected=true
            });
            List<PermissionGroup> PermissionGroupDropDown = _Util.Facade.PermissionFacade.GetAllPermissionGroupListByCompanyId(currentUser.CompanyId.Value);
            if(PermissionGroupDropDown != null && PermissionGroupDropDown.Count > 0)
            {
                permissions.AddRange(PermissionGroupDropDown.OrderBy(x => x.Name).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.Name.ToString(),
                              Value = x.Id.ToString()
                          }).ToList());
            }
            
            ViewBag.permissions = permissions;
            return View();
        }

        [Authorize]
        public ActionResult SendCompanyFiles(int CompanyFileId, string EmailAddresss)
        {
            var result = false;
            var CurrentUser = (CustomPrincipal)(User);
            CompanyFile companyFile = _Util.Facade.CompanyFileFacade.GetById(CompanyFileId);
            ShareCompanyFile shareCompanyFile = new ShareCompanyFile();
            JsonRequestController jsonRequest = new JsonRequestController();
            jsonRequest.Encode(companyFile.Filename);
            shareCompanyFile.FileLocationLink = AppConfig.DomainSitePath +  "/CompanyFileDownload/?id=" + jsonRequest.Encode(companyFile.Filename);
            shareCompanyFile.ToEmail = EmailAddresss;
            result = _Util.Facade.MailFacade.SendCompanyFile(shareCompanyFile, CurrentUser.CompanyId.Value);
            return Json(new { result = result });
        }
        [Authorize]
        public JsonResult LoadUser(int id)
        {
            var currentUser = (CustomPrincipal)(User);
            var userList = _Util.Facade.PermissionFacade.GetAllUserPermissionsPermissionGroupId(id, currentUser.CompanyId.Value);
            return Json(userList,JsonRequestBehavior.AllowGet);
        }
        //[Authorize]
        //[HttpPost]
        //public ActionResult UploadCompanyLogo()
        //{
        //    bool isUploaded = false;
        //    var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
        //    HttpPostedFileBase httpPostedFileBaseBranch = Request.Files["CompanyBranchFile"];

        //    string tempFolderName = ConfigurationManager.AppSettings["File.CompanyBranchFile"];

        //    tempFolderName += DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

        //    Random rand = new Random();
        //    string FileName = rand.Next().ToString();
        //    FileName += "-___" + httpPostedFileBaseBranch.FileName;

        //    if (httpPostedFileBaseBranch != null && httpPostedFileBaseBranch.ContentLength != 0)
        //    {

        //        string tempFolderPath = Server.MapPath("~/" + tempFolderName);

        //        if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
        //        {
        //            try
        //            {
        //                httpPostedFileBaseBranch.SaveAs(Path.Combine(tempFolderPath, FileName));
        //                isUploaded = true;
        //            }
        //            catch (Exception) {  /*TODO: You must process this exception.*/}
        //        }
        //    }

        //    string filePath = string.Concat("/", tempFolderName, "/", FileName);
        //    return Json(new { isUploaded = isUploaded, filePath = filePath }, "text/html");
        //}
        [Authorize]
        [HttpPost]
        public JsonResult AddCompany(Company company)
        {

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            Company Model = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);

            if (Model != null)
            {
                Model.EmailAdress = company.EmailAdress;
                Model.Street = company.Street;
                Model.City = company.City;
                Model.State = company.State;
                Model.ZipCode = company.ZipCode;
                Model.Website = company.Website;
                Model.FirstName = company.FirstName;
                Model.LastName = company.LastName;
                Model.Note = company.Note;
                Model.Fax = company.Fax;
                Model.Phone = company.Phone;
                Model.CompanyName = company.CompanyName;
                Model.CompanyLogo = company.CompanyImage;
                if (_Util.Facade.CompanyFacade.UpdateCompany(Model))
                {
                    if (!string.IsNullOrWhiteSpace(Model.CompanyLogo) && Model.CompanyLogo.Length >10)//basic validation. length validation not required.
                    {
                        var companybranchobj = _Util.Facade.CompanyBranchFacade.GetMainCompanyBranchByCompanyId(CurrentUser.CompanyId.Value);
                        if (companybranchobj != null)
                        {
                            companybranchobj.Logo = company.CompanyImage;
                            companybranchobj.EmailLogo = company.CompanyImage;
                            companybranchobj.ColorLogo = company.CompanyImage;
                            _Util.Facade.CompanyBranchFacade.UpdateCompanyBranch(companybranchobj);
                        }
                    }
                    return Json(new { result = true, message = "Company info updated successfully." });
                }
                else
                {
                    return Json(new { result = false, message = "Company info update failed." });
                }
                
            }

            return Json(new { result = false, message = "Company not found." });
        }
    }
}