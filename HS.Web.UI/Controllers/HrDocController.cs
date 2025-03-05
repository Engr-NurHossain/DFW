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
using System.Web.Mvc.Html;

namespace HS.Web.UI.Controllers
{
    public class HrDocController : BaseController
    {
        // GET: HrDoc
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HrDocPartial(string usernum,string SearchText,string FilterText)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ViewBag.FilterText = FilterText;
            ViewBag.SearchText = SearchText;
            List<HrDoc> hrdoc = _Util.Facade.HrDocFacade.GetAllUserFilesByCompanyIdAndFilter(CurrentUser.CompanyId.Value, usernum,SearchText,FilterText);
            ViewBag.DocCategory = _Util.Facade.LookupFacade.GetLookupByKey("DocCatagory").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                         new SelectListItem()
                         {
                             Text = x.DisplayText.ToString(),
                             Value = x.DataValue.ToString()
                         }).ToList();
            ViewBag.usernumlist = usernum;
            return View("HrDocPartial", hrdoc);
        }

        public ActionResult Download()
        {

            if (Request.QueryString.Count == 0)
            {
                return RedirectPermanent("/");
            }
            else
            {
                string Idstr = DESEncryptionDecryption.DecryptCipherTextToPlainText(HttpUtility.UrlDecode(Request.QueryString[0].ToString()));

                CustomerFile filename = _Util.Facade.CustomerFileFacade.GetFileNameById(Int32.Parse(Idstr));
                // string Filenum = Encryption.Encrypt(filename.Id.ToString(),true);

                byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~" + filename.Filename));
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename.Filename);
            }
        }

        [Authorize]
        public ActionResult AddHrDoc(string user,int? id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            HrDoc model = new HrDoc();
            if(id.HasValue && id > 0)
            {
                model = _Util.Facade.HrDocFacade.GetHrDocById(id.Value);
            }
            
            ViewBag.DocCategory = _Util.Facade.LookupFacade.GetLookupByKey("DocCatagory").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList(); 
            ViewBag.Adduser = user;
            return View("AddHrDoc", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteUserFile(int? id)
        {
            if (id.HasValue)
            {
                var userval = _Util.Facade.HrDocFacade.DeleteUser(id.Value);
            }
            return Json(true);
        }

        [Authorize]
        [HttpPost]
        public ActionResult UploadUserFile()
        {
            bool isUploaded = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            HttpPostedFileBase httpPostedFileBase = Request.Files["CustomerFile"];

            string tempFolderName = ConfigurationManager.AppSettings["File.UserFile"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            tempFolderName = string.Format(tempFolderName, comname);
            tempFolderName += DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += "-___" + httpPostedFileBase.FileName;

            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
            {

                string tempFolderPath = Server.MapPath("~/" + tempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        httpPostedFileBase.SaveAs(Path.Combine(tempFolderPath, FileName));
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
        public JsonResult SaveUserFile(HrDoc hrdoc)
        {
            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var serverFile = Server.MapPath(hrdoc.Filename);
            if(hrdoc.Id > 0)
            {
                HrDoc oldhrDoc = _Util.Facade.HrDocFacade.GetHrDocById(hrdoc.Id);
                oldhrDoc.Filename = hrdoc.Filename;
                oldhrDoc.FileDescription = hrdoc.FileDescription;
                oldhrDoc.Category = hrdoc.Category;
                result  = _Util.Facade.HrDocFacade.UpdateHrDoc(oldhrDoc);
            }
            else if(!string.IsNullOrWhiteSpace(hrdoc.Category) && hrdoc.Category == "ProfilePage")
            {
                HrDoc categoryDoc = _Util.Facade.HrDocFacade.GetHrDocByUsernameAndCategory(hrdoc.UserName,CurrentUser.CompanyId.Value,hrdoc.Category);
                if(categoryDoc != null)
                { 
                    categoryDoc.Filename = hrdoc.Filename;
                    categoryDoc.FileDescription = hrdoc.FileDescription;
                    categoryDoc.Category = hrdoc.Category;
                    result = _Util.Facade.HrDocFacade.UpdateHrDoc(categoryDoc);
                }
                else
                {
                    HrDoc hd = new HrDoc()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        UserName = hrdoc.UserName,
                        FileDescription = hrdoc.FileDescription,
                        Filename = hrdoc.Filename,
                        Uploadeddate = DateTime.Now.UTCCurrentTime(),
                        CreatedBy = CurrentUser.UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        Category = hrdoc.Category


                    };
                    result = _Util.Facade.HrDocFacade.InsertHrDoc(hd) > 0;
                }
            }
            else
            {
                HrDoc hd = new HrDoc()
                {
                    CompanyId = CurrentUser.CompanyId.Value,
                    UserName = hrdoc.UserName,
                    FileDescription = hrdoc.FileDescription,
                    Filename = hrdoc.Filename,
                    Uploadeddate = DateTime.Now.UTCCurrentTime(),
                    CreatedBy = CurrentUser.UserId,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    Category = hrdoc.Category


                };
                result = _Util.Facade.HrDocFacade.InsertHrDoc(hd) > 0;
            }
           
            
            return Json(new { result = result });
        }
    }
}