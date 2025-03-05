using HS.Entities;
using HS.Framework;
using HS.Framework.Utils;
using HS.Web.UI.Helper;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using ClosedXML.Excel;
using Excel = HS.Web.UI.Helper.ExcelFormatHelper;
using System.IO;
using System.Net;

namespace HS.Web.UI.Controllers
{
    public class SalesController : BaseController
    {
        // GET: Sales
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
            return View();
        }

        #region Probably Unused Modules
        [Authorize]
        public ActionResult SalesPartial(Guid? customerid)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));


            bool res = _Util.Facade.CustomerFacade.CustomerIsInCompanySalesPartial(customerid.Value, currentLoggedIn.CompanyId.Value);
            if (!res)
            {
                return null;
            }
            Customer tmpCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerid.Value);

            Guid employeeId = currentLoggedIn.UserId;
            //bool isSales = true; 

            List<CustomerAppointment> CustomerAppoint = _Util.Facade.CustomerAppoinmentFacade.GetAllSalesAppoinmentByEmployeeId(tmpCustomer.CustomerId, currentLoggedIn.CompanyId.Value, employeeId);
            var count = CustomerAppoint.Count();
            if (count > 0)
            {
                return PartialView("_SalesPartial", CustomerAppoint);
            }
            else
            {

                return PartialView("_SalesCalendar", CustomerAppoint);
            }
        }
        public JsonResult FlagedArtical(int Id, bool IsFlag, string comments)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool result = false;
            if (Id > 0)
            {
                Knowledgebase knowledge = _Util.Facade.CustomerAppoinmentFacade.GetKnowledgebase(Id);
                if (knowledge != null)
                {
                    if (!IsFlag)
                    {
                        _Util.Facade.CustomerAppoinmentFacade.UnFlagUserForKnowledgebase(knowledge.Id, currentLoggedIn.UserId);
                    }
                    string cmt = "";
                    if (IsFlag && string.IsNullOrWhiteSpace(comments))
                    {
                        cmt = "Flagged";
                    }
                    else if (!IsFlag && string.IsNullOrWhiteSpace(comments))
                    {
                        cmt = "Unflagged";
                    }
                    else
                    {
                        cmt = comments;
                    }
                    KnowledgeBaseFlagUser old = new KnowledgeBaseFlagUser()
                    {
                        IsFlag = IsFlag,
                        IsDocument = false,
                        LastUpdatedBy = currentLoggedIn.UserId,
                        LastUpdatedDate = DateTime.UtcNow,
                        CreatedBy = currentLoggedIn.UserId,
                        CreatedDate = DateTime.UtcNow,
                        UserId = currentLoggedIn.UserId,
                        KnowledgebaseId = knowledge.Id,
                        Comment = cmt
                    };
                    result = _Util.Facade.CustomerAppoinmentFacade.InsertFlagUserForKnowledgebase(old);
                }
            }
            return Json(new { result = result, Id = Id }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCommentForKnowledge(int id)
        {
            KnowledgeBaseFlagUserCustom comments = _Util.Facade.CustomerAppoinmentFacade.GetFlagUserCommentForKnowledgebase(id, false).FirstOrDefault();
            comments.DateC = comments.Date.ToString("M/d/yyyy h:mm tt");
            return Json(comments);
        }
        [Authorize]
        [HttpPost]
        public JsonResult SaveDocumentLibrary(DocumentLibrary model)
        {
            bool result = false;
            DocumentLibrary document = new DocumentLibrary();
            EstimateImage ImageModel = new EstimateImage();
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (model == null)
            {
                return Json(result);
            }

            if (model.Id > 0)
            {
                document = _Util.Facade.CustomerAppoinmentFacade.GetDocumentLibrary(model.Id);
                if (document != null)
                {
                    document.Title = model.Title;
                    document.Tags = (model.TagsStr != null && model.TagsStr.Count > 0 ? string.Join(", ", model.TagsStr) : "");
                    document.IsDocumentLibrary = model.IsDocumentLibrary;
                    document.IsHidden = model.IsHidden;
                    document.Answer = WebUtility.UrlDecode(model.Answer);
                    document.CarrierTag = model.CarrierTag;
                    document.PolicyTag = model.PolicyTag;
                    document.CreatedBy = currentLoggedIn.UserId;
                    document.CreatedDate = DateTime.UtcNow;
                    document.LastUpdatedBy = currentLoggedIn.UserId;
                    document.LastUpdatedDate = DateTime.UtcNow;
                    result = _Util.Facade.CustomerAppoinmentFacade.UpdateDocumentLibrary(document);
                    if (result)
                    {
                        _Util.Facade.CustomerAppoinmentFacade.DeleteDocumentLibraryWeblinkByKnowledgebaseId(model.Id);
                        if (model.DocumentLibraryWeblinkList != null && model.DocumentLibraryWeblinkList.Count() > 0)
                        {
                            foreach (var item in model.DocumentLibraryWeblinkList)
                            {
                                if (item != null && !string.IsNullOrWhiteSpace(item.Title) && !string.IsNullOrWhiteSpace(item.Link))
                                {
                                    item.KnowledgebaseId = document.Id;
                                    item.Title = System.Uri.UnescapeDataString(item.Title);
                                    item.Link = System.Uri.UnescapeDataString(item.Link);
                                    item.IsRelated = false;
                                    _Util.Facade.CustomerAppoinmentFacade.InsertDocumentLibraryWeblink(item);
                                }
                            }
                        }
                        if (model.RelatedArticleList != null && model.RelatedArticleList.Count() > 0)
                        {
                            foreach (var item in model.RelatedArticleList)
                            {
                                if (item != null && !string.IsNullOrWhiteSpace(item.Link))
                                {
                                    item.KnowledgebaseId = document.Id;
                                    item.Title = item.Title;
                                    item.Link = System.Uri.UnescapeDataString(item.Link);
                                    item.IsRelated = true;
                                    _Util.Facade.CustomerAppoinmentFacade.InsertDocumentLibraryWeblink(item);
                                }
                            }
                        }
                        _Util.Facade.CustomerAppoinmentFacade.DeleteEstimateImageByKnowledgeId(model.Id.ToString(), true);
                        if (model.Images != null && model.Images.Count() > 0)
                        {
                            foreach (var item in model.Images)
                            {
                                ImageModel.CompanyId = currentLoggedIn.CompanyId.Value;
                                ImageModel.CustomerId = Guid.Empty;
                                ImageModel.InvoiceId = document.Id.ToString();
                                ImageModel.ImageLoc = item.Location;
                                ImageModel.ImageType = item.Description;
                                ImageModel.UploadedDate = DateTime.UtcNow;
                                ImageModel.CreatedBy = currentLoggedIn.UserId;
                                ImageModel.IsDocument = true;
                                ImageModel.Size = item.Size;
                                _Util.Facade.CustomerAppoinmentFacade.InsertEstimateImage(ImageModel);
                            }
                        }
                    }
                }
            }
            else
            {
                document.Title = model.Title;
                document.Tags = (model.TagsStr != null && model.TagsStr.Count > 0 ? string.Join(", ", model.TagsStr) : "");
                document.IsDocumentLibrary = model.IsDocumentLibrary;
                document.Answer = WebUtility.UrlDecode(model.Answer);
                document.IsHidden = model.IsHidden;
                document.CarrierTag = model.CarrierTag;
                document.PolicyTag = model.PolicyTag;
                document.CreatedBy = currentLoggedIn.UserId;
                document.CreatedDate = DateTime.UtcNow;
                document.LastUpdatedBy = currentLoggedIn.UserId;
                document.LastUpdatedDate = DateTime.UtcNow;
                document.IsDeleted = false;
                document.Id = (int)_Util.Facade.CustomerAppoinmentFacade.InsertDocumentLibrary(document);
                if (document.Id > 0) { result = true; }
                if (result)
                {
                    if (model.DocumentLibraryWeblinkList != null && model.DocumentLibraryWeblinkList.Count() > 0)
                    {
                        foreach (var item in model.DocumentLibraryWeblinkList)
                        {
                            if (item != null && !string.IsNullOrWhiteSpace(item.Title) && !string.IsNullOrWhiteSpace(item.Link))
                            {
                                item.KnowledgebaseId = document.Id;
                                item.Title = System.Uri.UnescapeDataString(item.Title);
                                item.Link = System.Uri.UnescapeDataString(item.Link);
                                item.IsRelated = false;
                                _Util.Facade.CustomerAppoinmentFacade.InsertDocumentLibraryWeblink(item);

                            }
                        }
                    }
                    if (model.RelatedArticleList != null && model.RelatedArticleList.Count() > 0)
                    {
                        foreach (var item in model.RelatedArticleList)
                        {
                            if (item != null && !string.IsNullOrWhiteSpace(item.Link))
                            {
                                item.KnowledgebaseId = document.Id;
                                item.Title = item.Title;
                                item.Link = System.Uri.UnescapeDataString(item.Link);
                                item.IsRelated = true;
                                _Util.Facade.CustomerAppoinmentFacade.InsertDocumentLibraryWeblink(item);

                            }
                        }
                    }
                    if (model.Images != null && model.Images.Count() > 0)
                    {
                        foreach (var item in model.Images)
                        {
                            ImageModel.CompanyId = currentLoggedIn.CompanyId.Value;
                            ImageModel.CustomerId = Guid.Empty;
                            ImageModel.InvoiceId = document.Id.ToString();
                            ImageModel.ImageLoc = item.Location;
                            ImageModel.ImageType = item.Description;
                            ImageModel.UploadedDate = DateTime.UtcNow;
                            ImageModel.CreatedBy = currentLoggedIn.UserId;
                            ImageModel.IsDocument = true;
                            ImageModel.Size = item.Size;
                            _Util.Facade.CustomerAppoinmentFacade.InsertEstimateImage(ImageModel);
                        }
                    }
                }
            }

            //_Util.Facade.CredentialSettingFacade.DeleteTagMapByKnowladgeId(document.Id);
            //List<string> tagId = new List<string>();
            //if (model.TagsStr != null && model.TagsStr.Count() > 0)
            //{
            //    foreach (string name in model.TagsStr)
            //    {
            //        RMRTag tag = _Util.Facade.CredentialSettingFacade.GetTagByName(name);
            //        if (tag == null)
            //        {
            //            tag = new RMRTag();
            //            tag.TagName = name;
            //            tag.IsDeleted = false;
            //            tag.CreatedDate = DateTime.UtcNow;
            //            tag.LastUpdatedDate = DateTime.UtcNow;
            //            tag.CreatedBy = currentLoggedIn.UserId;
            //            tag.LastUpdatedBy = currentLoggedIn.UserId;
            //            tag.IsFavourite = false;
            //            tag.Id = (int)_Util.Facade.CredentialSettingFacade.InsertRMRTag(tag);
            //        }
            //        tagId.Add(tag.TagName.Trim());
            //        RMRTagMap map = new RMRTagMap();
            //        map.KnowledgebaseId = knowledge.Id;
            //        map.TagId = tag.Id;
            //        map.IsDeleted = false;
            //        map.CreatedDate = DateTime.UtcNow;
            //        map.LastUpdatedDate = DateTime.UtcNow;
            //        map.CreatedBy = currentLoggedIn.UserId;
            //        map.LastUpdatedBy = currentLoggedIn.UserId;
            //        _Util.Facade.CredentialSettingFacade.InsertTagMap(map);
            //    }
            //    knowledge.Tags = (tagId != null && tagId.Count > 0 ? string.Join(", ", tagId) : "");
            //    result = _Util.Facade.QtiManageFacade.UpdateKnowledgebase(knowledge);
            //}
            return Json(result);
        }
        public JsonResult GetCommentForDocumentLibrary(int id)
        {
            KnowledgeBaseFlagUserCustom comments = _Util.Facade.CustomerAppoinmentFacade.GetFlagUserCommentForKnowledgebase(id, true).FirstOrDefault();
            comments.DateC = comments.Date.ToString("M/d/yyyy h:mm tt");
            return Json(comments);
        }
        public ActionResult ShowDocumentLibraryMessage(int Id, string SearchText)
        {
            if (Session[SessionKeys.DocumentLibraryFilter] != null)
            {
                QtiFilter sessionvalue = (QtiFilter)Session[SessionKeys.DocumentLibraryFilter];
                if (!string.IsNullOrWhiteSpace(sessionvalue.SearchText))
                {
                    ViewBag.SearchText = sessionvalue.SearchText;
                }
                if (!string.IsNullOrWhiteSpace(sessionvalue.Tag) && sessionvalue.Tag != "null")
                {
                    ViewBag.Tag = sessionvalue.Tag;
                }
            }
            Knowledgebase knowledge = _Util.Facade.CustomerAppoinmentFacade.GetKnowledgebase(Id);
            if (knowledge.FlagBy != Guid.Empty)
            {
                Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(knowledge.FlagBy);
                if (emp != null)
                {
                    knowledge.FlagByName = emp.FirstName + " " + emp.LastName;
                }
            }
            knowledge.SavedImages = _Util.Facade.CustomerAppoinmentFacade.GetEstimateImage(knowledge.Id.ToString(), true);
            knowledge.KnowledgeWeblinkList = _Util.Facade.CustomerAppoinmentFacade.GetWeblinksListByKnowledgebaseId(knowledge.Id);
            ViewBag.SearchText = SearchText;
            ViewBag.RemovedFlag = IsPermitted(UserPermissions.QTIPermission.RemoveDocumentFlag);
            return View(knowledge);
        }
        [HttpPost]
        public JsonResult SortDocumentLibraryFileList(string order, int Id)
        {
            Knowledgebase model = new Knowledgebase();
            bool result = false;
            if (!string.IsNullOrWhiteSpace(order))
            {
                model.SavedImages = _Util.Facade.CustomerAppoinmentFacade.GetSortListOfEstimateImage(Id, order, true);
                if (model.SavedImages != null)
                {
                    foreach (var item in model.SavedImages)
                    {
                        item.StrUploadedDate = item.UploadedDate.UTCToClientTime().ToString("M/dd/yy");
                        item.Extension = item.ImageLoc.Substring(item.ImageLoc.LastIndexOf(".") + 1).ToUpper();
                        if (item.Size>0)
                        {
                            item.Size = System.Math.Round(item.Size, 2);
                        }
                    }
                    if (order == "descending/type")
                    {
                        model.SavedImages = model.SavedImages.OrderByDescending(x => x.Extension).ToList();
                    }
                    if (order == "ascending/type")
                    {
                        model.SavedImages = model.SavedImages.OrderBy(x => x.Extension).ToList();
                    }
                }
                result = true;
            }
            return Json(new { result = result, ImagesList = model.SavedImages });
        }
        public JsonResult FlagedDocumentLibraryArtical(int Id, bool IsFlag, string comments)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool result = false;
            if (Id > 0)
            {
                DocumentLibrary knowledge = _Util.Facade.CustomerAppoinmentFacade.GetDocumentLibrary(Id);
                if (knowledge != null)
                {
                    if (!IsFlag)
                    {
                        _Util.Facade.CustomerAppoinmentFacade.UnFlagUserForKnowledgebase(knowledge.Id, currentLoggedIn.UserId);
                    }
                    string cmt = "";
                    if (IsFlag && string.IsNullOrWhiteSpace(comments))
                    {
                        cmt = "Flagged";
                    }
                    else if (!IsFlag && string.IsNullOrWhiteSpace(comments))
                    {
                        cmt = "Unflagged";
                    }
                    else
                    {
                        cmt = comments;
                    }
                    KnowledgeBaseFlagUser old = new KnowledgeBaseFlagUser()
                    {
                        IsFlag = IsFlag,
                        IsDocument = true,
                        LastUpdatedBy = currentLoggedIn.UserId,
                        LastUpdatedDate = DateTime.UtcNow,
                        CreatedBy = currentLoggedIn.UserId,
                        CreatedDate = DateTime.UtcNow,
                        UserId = currentLoggedIn.UserId,
                        KnowledgebaseId = knowledge.Id,
                        Comment = cmt
                    };
                    result = _Util.Facade.CustomerAppoinmentFacade.InsertFlagUserForKnowledgebase(old);
                }
            }
            return Json(new { result = result, Id = Id }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ImagePreview(string url, string caption)
        {
            ImageUrl model = new ImageUrl();
            if (!string.IsNullOrWhiteSpace(url))
            {
                model.url = AppConfig.SiteDomain + url;
                model.caption = caption;
            }
            return View(model);
        }
        [Authorize]
        public ActionResult ShowDocumentLibraryArticle(int Id)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (Session[SessionKeys.DocumentLibraryFilter] != null)
            {
                QtiFilter sessionvalue = (QtiFilter)Session[SessionKeys.DocumentLibraryFilter];
                if (!string.IsNullOrWhiteSpace(sessionvalue.SearchText))
                {
                    ViewBag.SearchText = sessionvalue.SearchText;
                }
                if (!string.IsNullOrWhiteSpace(sessionvalue.Tag) && sessionvalue.Tag != "null")
                {
                    ViewBag.Tag = sessionvalue.Tag;
                }
            }
            DocumentLibrary document = _Util.Facade.CustomerAppoinmentFacade.GetDocumentLibrary(Id);
            if (document == null)
            {
                return PartialView("~/Views/Shared/_NotFound.cshtml");
            }
            else if (document.IsDeleted == true && !currentLoggedIn.UserRole.ToLower().Contains("admin"))
            {
                return PartialView("~/Views/Shared/_Deleted.cshtml");
            }
            bool isadmin = false;
            if (currentLoggedIn.UserRole.ToLower().IndexOf("admin") > -1)
            {
                isadmin = true;
            }
            var knowFlagUser = _Util.Facade.CustomerAppoinmentFacade.GetFlagUserForKnowledgebase(currentLoggedIn.UserId, document.Id, true, isadmin);
            if (knowFlagUser != null && knowFlagUser.IsFlag && knowFlagUser.CreatedBy != Guid.Empty)
            {
                Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(knowFlagUser.CreatedBy);
                if (emp != null)
                {
                    document.FlagByName = String.Format("{0} {1}", emp.FirstName, emp.LastName);
                }
            }
            document.SavedImages = _Util.Facade.CustomerAppoinmentFacade.GetEstimateImage(document.Id.ToString(), true);
            document.DocumentLibraryWeblinkList = _Util.Facade.CustomerAppoinmentFacade.GetWeblinksListByDocumentId(document.Id);
            ViewBag.RemovedFlag = IsPermitted(UserPermissions.QTIPermission.RemoveDocumentFlag);

            document.IsFlag = knowFlagUser != null ? knowFlagUser.IsFlag : false;
            document.Comments = knowFlagUser != null ? knowFlagUser.Comment : "";
            document.FlagDate = knowFlagUser != null ? knowFlagUser.LastUpdatedDate : new DateTime();
            if (currentLoggedIn.UserRole.ToLower().IndexOf("admin") > -1)
            {
                List<KnowledgeBaseFlagUserCustom> comments = _Util.Facade.CustomerAppoinmentFacade.GetFlagUserCommentForKnowledgebase(document.Id, true);
                document.ListKnowledgeBaseFlagUser = comments;
            }
            else
            {
                List<KnowledgeBaseFlagUserCustom> comments = _Util.Facade.CustomerAppoinmentFacade.GetFlagUserCommentForKnowledgebase(document.Id, currentLoggedIn.UserId, true);
                document.ListKnowledgeBaseFlagUser = comments;
            }
            return View(document);
        }
        [Authorize]
        public ActionResult AddSales(int? id, int customerid, int? Date, int? Month, int? Year)
        {
            CustomerAppointment model;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            if (Date.HasValue && Month.HasValue && Year.HasValue)
            {
                ViewBag.SelectedDate = Date.ToString() + "-" + Month.ToString() + "-" + Year.ToString();
            }

            if (id.HasValue)
            {

                model = _Util.Facade.CustomerAppoinmentFacade.GetAppointmentById(id.Value);
            }
            else
            {
                model = new CustomerAppointment();
            }
            ViewBag.CustomerId = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerid).CustomerId.ToString();

            List<SelectListItem> SalesPersons = new List<SelectListItem>();
            SalesPersons.Add(new SelectListItem()
            {
                Text = "Sales Persons",
                Value = "-1"
            });
            SalesPersons.AddRange(_Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.SalesPerson, new Guid()).Select(x => new SelectListItem()
            {
                Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                Value = x.UserId.ToString()
            }).ToList());
            ViewBag.CustomerList = SalesPersons.OrderBy(x => x.Text != "Sales Persons").ThenBy(x => x.Text).ToList();

            ViewBag.AppointmentTime = _Util.Facade.LookupFacade.GetLookupByKey("Arrival").Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();

            return PartialView("AddSales", model);
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddSales(CustomerAppointment ca)
        {
            bool result = false;

            //ca.EmployeeId = ((HS.Web.UI.Helper.CustomPrincipal)(User)).EmployeeId.Value;
            ca.CompanyId = ((HS.Web.UI.Helper.CustomPrincipal)User).CompanyId.Value;
            ca.AppointmentType = AppoinmentType.Sales;
            ca.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            ca.LastUpdatedBy = User.Identity.Name;
            if (ca.Id > 0)
            {
                CustomerAppointment tempCa = _Util.Facade.CustomerAppoinmentFacade.GetById(ca.Id);
                if (tempCa.CompanyId != ((HS.Web.UI.Helper.CustomPrincipal)User).CompanyId.Value)
                {
                    return Json(new { result = false, message = "Invalid Parameter" });
                }
                ca.CreatedBy = tempCa.CreatedBy;
                ca.AppointmentId = tempCa.AppointmentId;
                result = _Util.Facade.CustomerAppoinmentFacade.UpdateCustomerAppoinment(ca);
                //result = _Util.Facade.CustomerSnapshotFacade.UpdateSnapshot();
            }
            else
            {
                ca.CreatedBy = User.Identity.Name;
                ca.AppointmentId = Guid.NewGuid();
                result = _Util.Facade.CustomerAppoinmentFacade.InsertCustomerAppoinment(ca) > 0;

                string EmployeeName = "";
                var EmployeeDetails = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(ca.EmployeeId);
                if (EmployeeDetails != null)
                {
                    EmployeeName = EmployeeDetails.FirstName + " " + EmployeeDetails.LastName;
                }

                CustomerSnapshot Snapshotobj = new CustomerSnapshot()
                {
                    CompanyId = ca.CompanyId,
                    CustomerId = ca.CustomerId,
                    Description = AppoinmentType.Sales,
                    Logdate = ca.AppointmentDate.Value,
                    Updatedby = EmployeeName
                };
                result = _Util.Facade.CustomerSnapshotFacade.InsertSnapshot(Snapshotobj);
            }

            return Json(new { result = result, message = "Invalid Parameter" });
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteSalesAppointment(int? id)
        {
            if (id.HasValue)
            {
                var sales = _Util.Facade.CustomerAppoinmentFacade.DeleteSalesAppointment(id.Value);
            }
            return Json(true);
        }

        public ActionResult SalesCalendar()
        {
            return PartialView("_CustomerAppoinments");
        }

        #endregion
        [Authorize]
        public PartialViewResult AllSalesPartial()
        {
            return PartialView("_AllSalesPartial");
        }
        public PartialViewResult ErrorPage()
        {
            return PartialView("~/Views/Shared/_AccessDenied.cshtml");
        }

        #region Knowledgebase Start

        //public PartialViewResult Knowledgebase(bool? isdocumentlibrary, int? knowledgebaeid)
        public ActionResult Knowledgebase(bool? isdocumentlibrary, int? knowledgebaeid)
        {
            if (!base.IsPermitted(UserPermissions.MenuPermissions.LeftMenuKnowledgebase))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<string> navlist = new List<string>();
            string path = HttpContext.Request.Path;
            ViewBag.IsDocumentLibrary = isdocumentlibrary.HasValue && isdocumentlibrary.Value ? true : false;
            ViewBag.IsContact = false;
            ViewBag.KnowledgebaeId = knowledgebaeid.HasValue ? knowledgebaeid.Value : 0;
            KnowledgebaseHomeModel model = new KnowledgebaseHomeModel();
            if (path.ToLower().Contains("/contact-list"))
            {
                List<KnowledgebaseRMRTag> list = _Util.Facade.CustomerAppoinmentFacade.GetAllFavouriteAndKnowledgeNavTags();
                if (list != null && list.Count > 0)
                {
                    ViewBag.FavouriteList = list.Select(x => new SelectListItem { Text = x.TagName, Value = x.TagName }).ToList();
                    List<KnowledgebaseRMRTag> knowledgebaseNavlist = list.Where(x => x.IsKnowledgebaseNav).ToList();
                    if (knowledgebaseNavlist.Count() > 0)
                    {
                        foreach (KnowledgebaseRMRTag item in knowledgebaseNavlist)
                        {
                            navlist.Add(item.TagName);
                        }
                    }
                    if (navlist.Count == 0)
                    {
                        navlist.Add("0");
                    }
                }
                else
                {
                    ViewBag.FavouriteList = new List<SelectListItem>();
                }
                ViewBag.IsContact = true;
                ViewBag.NavList = navlist;
                return View("KnowledgebaseContactList");
            }
            else
            {
                List<KnowledgebaseRMRTag> list = _Util.Facade.CustomerAppoinmentFacade.GetAllFavouriteTags();
                if (list != null && list.Count > 0)
                {
                    ViewBag.FavouriteList = list.Select(x => new SelectListItem { Text = x.TagName, Value = x.TagName }).ToList();
                }
                else
                {
                    ViewBag.FavouriteList = new List<SelectListItem>();
                }
            }
            ViewBag.NavList = navlist;
            model = _Util.Facade.CustomerAppoinmentFacade.GetRecentViewedKnowledgebaseList(CurrentUser.UserId);
            return View(model);

            //return PartialView("_Knowledgebase");
        }
        public ActionResult AddKnowledgeBase(int id, bool? IsDocumentLibrary)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool IsDefault = false;
            Knowledgebase knowledge = new Knowledgebase();
            if (id > 0)
            {
                knowledge = _Util.Facade.CustomerAppoinmentFacade.GetKnowledgebaseWithTagName(id);
                if (knowledge != null)
                {
                    knowledge.KnowledgeWeblinkList = _Util.Facade.CustomerAppoinmentFacade.GetWeblinksListByKnowledgebaseId(knowledge.Id).Where(x => x.IsRelated == false).ToList();
                    knowledge.RelatedArticleList = _Util.Facade.CustomerAppoinmentFacade.GetWeblinksListByKnowledgebaseId(knowledge.Id).Where(x => x.IsRelated == true).ToList();
                    knowledge.SavedImages = _Util.Facade.CustomerAppoinmentFacade.GetEstimateImage(knowledge.Id.ToString(), false);
                    List<KnowledgebaseGroupAccess> userlist = _Util.Facade.CustomerAppoinmentFacade.GetKnowledgebaseGroupAccess(knowledge.Id);
                    if (userlist != null)
                    {
                        knowledge.StrUserGroups = new List<string>();
                        foreach (var item in userlist)
                        {
                            if (item.IsDefault && !IsDefault)
                            {
                                IsDefault = true;
                            }
                            knowledge.StrUserGroups.Add(item.UserGroupId.ToString());
                        }
                        knowledge.IsDefault = IsDefault;
                    }
                }
            }
            else
            {
                knowledge.IsDocumentLibrary = IsDocumentLibrary.HasValue ? IsDocumentLibrary.Value : false;
            }

            List<SelectListItem> UserGroupList = new List<SelectListItem>();
            UserGroupList.AddRange(_Util.Facade.UserLoginFacade.GetAllPermissionGroup(CurrentUser.CompanyId.Value).OrderBy(x => x.Name).Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            }).ToList());
            ViewBag.UserGroupList = UserGroupList;
            return View(knowledge);
        }
        public JsonResult DeleteKnowledgebase(int id)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool result = false;
            if (id > 0)
            {
                Knowledgebase knowledge = _Util.Facade.CustomerAppoinmentFacade.GetKnowledgebase(id);
                if (knowledge != null)
                {
                    knowledge.IsDeleted = true;
                    knowledge.LastUpdatedBy = currentLoggedIn.UserId;
                    knowledge.LastUpdatedDate = DateTime.UtcNow;
                    result = _Util.Facade.CustomerAppoinmentFacade.UpdateKnowledgebase(knowledge);
                }
            }
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        [HttpPost]
        public JsonResult SaveKnowledgeBase(Knowledgebase model)
        {
            bool result = false;
            Knowledgebase knowledge = new Knowledgebase();
            EstimateImage ImageModel = new EstimateImage();
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (model == null)
            {
                return Json(result);
            }

            if (model.Id > 0)
            {
                knowledge = _Util.Facade.CustomerAppoinmentFacade.GetKnowledgebase(model.Id);
                if (knowledge != null)
                {
                    knowledge.Title = model.Title;
                    knowledge.Tags = (model.TagsStr != null && model.TagsStr.Count > 0 ? string.Join(", ", model.TagsStr).Replace("&amp;", "&") : "");
                    knowledge.IsDocumentLibrary = model.IsDocumentLibrary;
                    knowledge.IsHidden = model.IsHidden;
                    knowledge.Answer = WebUtility.UrlDecode(model.Answer);
                    knowledge.CreatedBy = currentLoggedIn.UserId;
                    knowledge.CreatedDate = DateTime.UtcNow;
                    knowledge.LastUpdatedBy = currentLoggedIn.UserId;
                    knowledge.LastUpdatedDate = DateTime.UtcNow;
                    result = _Util.Facade.CustomerAppoinmentFacade.UpdateKnowledgebase(knowledge);
                    if (result)
                    {
                        _Util.Facade.CustomerAppoinmentFacade.DeleteKnowledgebaseWeblinkByKnowledgebaseId(model.Id);
                        if (model.KnowledgeWeblinkList != null && model.KnowledgeWeblinkList.Count() > 0)
                        {
                            foreach (var item in model.KnowledgeWeblinkList)
                            {
                                if (item != null && !string.IsNullOrWhiteSpace(item.Title) && !string.IsNullOrWhiteSpace(item.Link))
                                {
                                    item.KnowledgebaseId = knowledge.Id;
                                    item.Title = System.Uri.UnescapeDataString(item.Title);
                                    item.Link = System.Uri.UnescapeDataString(item.Link);
                                    item.IsRelated = false;
                                    _Util.Facade.CustomerAppoinmentFacade.InsertKnowledgebaseWeblinkClass(item);
                                }
                            }
                        }
                        if (model.RelatedArticleList != null && model.RelatedArticleList.Count() > 0)
                        {
                            foreach (var item in model.RelatedArticleList)
                            {
                                if (item != null && !string.IsNullOrWhiteSpace(item.Link))
                                {
                                    item.KnowledgebaseId = knowledge.Id;
                                    item.Title = item.Title;
                                    item.Link = System.Uri.UnescapeDataString(item.Link);
                                    item.IsRelated = true;
                                    _Util.Facade.CustomerAppoinmentFacade.InsertKnowledgebaseWeblinkClass(item);
                                }
                            }
                        }
                        _Util.Facade.CustomerAppoinmentFacade.DeleteEstimateImageByKnowledgeId(model.Id.ToString(), false);
                        if (model.Images != null && model.Images.Count() > 0)
                        {
                            foreach (var item in model.Images)
                            {
                                ImageModel.CompanyId = currentLoggedIn.CompanyId.Value;
                                ImageModel.CustomerId = Guid.Empty;
                                ImageModel.InvoiceId = knowledge.Id.ToString();
                                ImageModel.ImageLoc = item.Location;
                                ImageModel.ImageType = item.Description;
                                ImageModel.UploadedDate = DateTime.UtcNow;
                                ImageModel.CreatedBy = currentLoggedIn.UserId;
                                ImageModel.IsDocument = false;
                                ImageModel.Size = item.Size;
                                _Util.Facade.CustomerAppoinmentFacade.InsertEstimateImage(ImageModel);
                            }
                        }
                        _Util.Facade.CustomerAppoinmentFacade.DeleteKnowledgebaseGroupAccess(knowledge.Id, false);
                        if (model.UserGroups != null && model.UserGroups.Count() > 0)
                        {
                            foreach (var user in model.UserGroups.Where(x => x != 0))
                            {
                                KnowledgebaseGroupAccess access = new KnowledgebaseGroupAccess();
                                access.KnowledgebaseId = knowledge.Id;
                                access.IsDocumentLibrary = false;
                                access.IsDefault = model.IsDefault;
                                access.UserGroupId = user;
                                access.CreatedBy = currentLoggedIn.UserId;
                                access.CreatedDate = DateTime.UtcNow;
                                _Util.Facade.CustomerAppoinmentFacade.InsertKnowledgebaseGroupAccess(access);
                            }
                        }
                    }
                }
            }
            else
            {
                knowledge.Title = model.Title;
                knowledge.Tags = (model.TagsStr != null && model.TagsStr.Count > 0 ? string.Join(", ", model.TagsStr) : "");
                knowledge.IsDocumentLibrary = model.IsDocumentLibrary;
                knowledge.Answer = WebUtility.UrlDecode(model.Answer);
                knowledge.IsHidden = model.IsHidden;
                knowledge.CreatedBy = currentLoggedIn.UserId;
                knowledge.CreatedDate = DateTime.UtcNow;
                knowledge.LastUpdatedBy = currentLoggedIn.UserId;
                knowledge.LastUpdatedDate = DateTime.UtcNow;
                knowledge.IsDeleted = false;
                knowledge.Id = (int)_Util.Facade.CustomerAppoinmentFacade.InsertKnowledgebase(knowledge);
                if (knowledge.Id > 0) { result = true; }
                if (result)
                {
                    if (model.KnowledgeWeblinkList != null && model.KnowledgeWeblinkList.Count() > 0)
                    {
                        foreach (var item in model.KnowledgeWeblinkList)
                        {
                            if (item != null && !string.IsNullOrWhiteSpace(item.Title) && !string.IsNullOrWhiteSpace(item.Link))
                            {
                                item.KnowledgebaseId = knowledge.Id;
                                item.Title = System.Uri.UnescapeDataString(item.Title);
                                item.Link = System.Uri.UnescapeDataString(item.Link);
                                item.IsRelated = false;
                                _Util.Facade.CustomerAppoinmentFacade.InsertKnowledgebaseWeblinkClass(item);

                            }
                        }
                    }
                    if (model.RelatedArticleList != null && model.RelatedArticleList.Count() > 0)
                    {
                        foreach (var item in model.RelatedArticleList)
                        {
                            if (item != null && !string.IsNullOrWhiteSpace(item.Link))
                            {
                                item.KnowledgebaseId = knowledge.Id;
                                item.Title = item.Title;
                                item.Link = System.Uri.UnescapeDataString(item.Link);
                                item.IsRelated = true;
                                _Util.Facade.CustomerAppoinmentFacade.InsertKnowledgebaseWeblinkClass(item);

                            }
                        }
                    }
                    if (model.Images != null && model.Images.Count() > 0)
                    {
                        foreach (var item in model.Images)
                        {
                            ImageModel.CompanyId = currentLoggedIn.CompanyId.Value;
                            ImageModel.CustomerId = Guid.Empty;
                            ImageModel.InvoiceId = knowledge.Id.ToString();
                            ImageModel.ImageLoc = item.Location;
                            ImageModel.ImageType = item.Description;
                            ImageModel.UploadedDate = DateTime.UtcNow;
                            ImageModel.CreatedBy = currentLoggedIn.UserId;
                            ImageModel.IsDocument = false;
                            ImageModel.Size = item.Size;
                            _Util.Facade.CustomerAppoinmentFacade.InsertEstimateImage(ImageModel);
                        }
                    }
                    if (model.UserGroups != null && model.UserGroups.Count() > 0)
                    {
                        foreach (var user in model.UserGroups.Where(x => x != 0))
                        {
                            KnowledgebaseGroupAccess access = new KnowledgebaseGroupAccess();
                            access.KnowledgebaseId = knowledge.Id;
                            access.IsDocumentLibrary = false;
                            access.IsDefault = model.IsDefault;
                            access.UserGroupId = user;
                            access.CreatedBy = currentLoggedIn.UserId;
                            access.CreatedDate = DateTime.UtcNow;
                            _Util.Facade.CustomerAppoinmentFacade.InsertKnowledgebaseGroupAccess(access);
                        }
                    }
                }
            }

            _Util.Facade.CustomerAppoinmentFacade.DeleteTagMapByKnowladgeId(knowledge.Id);
            List<string> tagId = new List<string>();
            if (model.TagsStr != null && model.TagsStr.Count() > 0)
            {
                foreach (string name in model.TagsStr)
                {
                    KnowledgebaseRMRTag tag = _Util.Facade.CustomerAppoinmentFacade.GetTagByName(name);
                    if (tag == null)
                    {
                        tag = new KnowledgebaseRMRTag();
                        tag.TagName = name;
                        tag.IsDeleted = false;
                        tag.CreatedDate = DateTime.UtcNow;
                        tag.LastUpdatedDate = DateTime.UtcNow;
                        tag.CreatedBy = currentLoggedIn.UserId;
                        tag.LastUpdatedBy = currentLoggedIn.UserId;
                        tag.IsFavourite = false;
                        tag.Id = (int)_Util.Facade.CredentialSettingFacade.InsertKnowledgebaseRMRTag(tag);
                    }
                    tagId.Add(tag.TagName.Trim());
                    KnowledgebaseRMRTagMap map = new KnowledgebaseRMRTagMap();
                    map.KnowledgebaseId = knowledge.Id;
                    map.TagId = tag.Id;
                    map.IsDeleted = false;
                    map.CreatedDate = DateTime.UtcNow;
                    map.LastUpdatedDate = DateTime.UtcNow;
                    map.CreatedBy = currentLoggedIn.UserId;
                    map.LastUpdatedBy = currentLoggedIn.UserId;
                    _Util.Facade.CredentialSettingFacade.InsertKnowledgebaseRMRTagMap(map);
                }
                knowledge.Tags = (tagId != null && tagId.Count > 0 ? string.Join(", ", tagId) : "");
                result = _Util.Facade.CustomerAppoinmentFacade.UpdateKnowledgebase(knowledge);
            }
            return Json(result);
        }
        public ActionResult Classroom()
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            ViewBag.StartTab = "AssignedTab";
            return View();
        }
        //public ActionResult CompletedToClassroom()
        //{
        //    if (!base.IsPermitted(UserPermissions.MenuPermissions.LeftMenuAssignArticle))
        //    {
        //        return PartialView("~/Views/Shared/_AccessDenied.cshtml");
        //    }
        //    ViewBag.FavouriteList = _Util.Facade.LookupFacade.GetAllLookupByKey("CategorySystem").OrderBy(x => x.DisplayText).Select(x => new SelectListItem()
        //    {
        //        Text = x.DisplayText,
        //        Value = x.DataValue
        //    }).ToList();
        //    return View();
        //}
        public ActionResult CompletedToClassroom(string Start, string End, int pageno, int pagesize, bool IsAdmin)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (!base.IsPermitted(UserPermissions.MenuPermissions.LeftMenuAssignArticle))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            ViewBag.FavouriteList = _Util.Facade.LookupFacade.GetAllLookupByKey("CategorySystem").OrderBy(x => x.DisplayText).Select(x => new SelectListItem()
            {
                Text = x.DisplayText,
                Value = x.DataValue
            }).ToList();
            ViewBag.InstallerList = _Util.Facade.EmployeeFacade.GetAllUserListForMultiSelect(currentLoggedIn.CompanyId.Value);
            ViewBag.StartDate = Start;
            ViewBag.EndDate = End;
            ViewBag.pagesize = pagesize;
            ViewBag.pageno = pageno;
            ViewBag.IsAdminm = IsAdmin;
            return View();
        }
        public ActionResult CompletedClassroomList(QtiFilter filter, bool IsSearch)
        {
            //bool IsEmpty = true;
            List<string> navlist = filter.NavList;
            int pageno = filter.PageNo;
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            //if (!string.IsNullOrEmpty(filter.SearchText))
            //{
            //    IsEmpty = false;
            //}
            //else if (!string.IsNullOrEmpty(filter.Tag) && filter.Tag != "null")
            //{
            //    IsEmpty = false;
            //}
            //else if (filter.IsFlaged)
            //{
            //    IsEmpty = false;
            //}
            //if (!IsEmpty)
            //{
            //    if (IsContact.ToLower() == "true")
            //    {
            //        filter.Tag = "";
            //    }
            //    Session[SessionKeys.KnowledgebaseFilter] = filter;

            //    if (!string.IsNullOrWhiteSpace(filter.SearchText))
            //    {
            //        KnowledgeSearchedKeyword keyword = new KnowledgeSearchedKeyword();
            //        keyword.Keyword = filter.SearchText;
            //        keyword.SearchedBy = CurrentUser.UserId;
            //        keyword.SearchedDate = DateTime.UtcNow;
            //        _Util.Facade.QtiManageFacade.InsertKnowledgeSearchedKeyword(keyword);
            //    }
            //}
            //else if (Session[SessionKeys.KnowledgebaseFilter] != null && !IsSearch)
            //{
            //    bool IsDoc = filter.IsDocumentLibrary;
            //    var filter1 = (QtiFilter)Session[SessionKeys.KnowledgebaseFilter];
            //    if (filter1.IsDeleted != filter.IsDeleted)
            //    {
            //        Session[SessionKeys.KnowledgebaseFilter] = filter;
            //    }
            //    else
            //    {
            //        filter = filter1;
            //        filter.PageNo = pageno;
            //        filter.IsDocumentLibrary = IsDoc;
            //        filter.IsContact = IsContact;
            //        filter.NavList = navlist;
            //        if (IsContact.ToLower() == "true")
            //        {
            //            filter.Tag = "";
            //        }
            //    }
            //}
            //else
            //{
            //    Session[SessionKeys.KnowledgebaseFilter] = null;
            //}


            filter.UserRole = CurrentUser.UserRole.ToLower();
            if (filter.PageNo < 1)
            {
                filter.PageNo = 1;
            }
            filter.PageSize = _Util.Facade.GlobalSettingsFacade.GetLeadPageLength(CurrentUser.CompanyId.Value);
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                filter.SearchText = System.Uri.UnescapeDataString(filter.SearchText);
            }
            ViewBag.Source = filter.SearchText;
            filter.CompanyId = CurrentUser.CompanyId.Value;

            if (filter.IsDownload && !filter.IsDeleted)
            {
                DataTable dt;
                dt = _Util.Facade.CustomerAppoinmentFacade.DownloadKnowledgebasebyFilter(filter);
                int[] colarray = { };
                int[] rowarray = { dt.Rows.Count + 2 };
                dt.Columns.Remove("Id");
                return MakeExcelFromDataTable(dt, "Knowledgebase List", rowarray, colarray);
            }
            else if (filter.IsDownload && filter.IsDeleted)
            {
                DataTable dt;
                dt = _Util.Facade.CustomerAppoinmentFacade.DownloadKnowledgebasebyFilter(filter);
                int[] colarray = { };
                int[] rowarray = { dt.Rows.Count + 2 };
                dt.Columns.Remove("Id");
                return MakeExcelFromDataTable(dt, "Deleted Knowledgebase List", rowarray, colarray);
            }
            filter.UserId = CurrentUser.UserId;
            KnowledgebaseListModel model = _Util.Facade.CustomerAppoinmentFacade.GetKnowledgebasebyFilterForClassroom(filter);
            if (model.KnowledgebaseList.Count <= 0)
            {
                model.KnowledgebaseList = new List<Knowledgebase>();
            }
            model.CompanyId = CurrentUser.CompanyId.Value;
            ViewBag.Order = filter.Order;
            ViewBag.PageNumber = filter.PageNo;
            ViewBag.Source = filter.SearchText;
            ViewBag.OutOfNumber = 0;
            if (model.TotalCount > 0)
            {
                ViewBag.OutOfNumber = model.TotalCount;
            }

            if ((int)ViewBag.PageNumber * filter.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filter.PageSize;
            }

            List<string> list = new List<string>();
            if (!string.IsNullOrWhiteSpace(filter.Tag))
            {
                string[] CBList = filter.Tag.Split(',');
                if (CBList != null)
                {
                    foreach (var item in CBList)
                    {
                        list.Add(item);
                    }
                }
            }

            List<string> list2 = new List<string>();
            if (!string.IsNullOrWhiteSpace(filter.NavList[0]))
            {
                string[] CBList2 = filter.NavList[0].Split(',');
                if (CBList2 != null)
                {
                    foreach (var item in CBList2)
                    {
                        list2.Add(item);
                    }
                }
            }

            ViewBag.TagList = list;
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filter.PageSize);
            ViewBag.SearchText = !string.IsNullOrWhiteSpace(filter.SearchText) ? filter.SearchText : "";
            ViewBag.Flagged = filter.IsFlaged;
            ViewBag.CheckNav = list2;
            ViewBag.IsDeleted = filter.IsDeleted;
            if (filter.IsDeleted)
            {
                return View("DeletedKnowledgebaseList", model);
            }
            ViewBag.IsAdmin = filter.IsAdmin;
            return View(model);
        }
        public ActionResult ClassroomList(QtiFilter filter, bool IsSearch)
        {
            //bool IsEmpty = true;
            List<string> navlist = filter.NavList;
            int pageno = filter.PageNo;
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            //if (!string.IsNullOrEmpty(filter.SearchText))
            //{
            //    IsEmpty = false;
            //}
            //else if (!string.IsNullOrEmpty(filter.Tag) && filter.Tag != "null")
            //{
            //    IsEmpty = false;
            //}
            //else if (filter.IsFlaged)
            //{
            //    IsEmpty = false;
            //}
            //if (!IsEmpty)
            //{
            //    if (IsContact.ToLower() == "true")
            //    {
            //        filter.Tag = "";
            //    }
            //    Session[SessionKeys.KnowledgebaseFilter] = filter;

            //    if (!string.IsNullOrWhiteSpace(filter.SearchText))
            //    {
            //        KnowledgeSearchedKeyword keyword = new KnowledgeSearchedKeyword();
            //        keyword.Keyword = filter.SearchText;
            //        keyword.SearchedBy = CurrentUser.UserId;
            //        keyword.SearchedDate = DateTime.UtcNow;
            //        _Util.Facade.QtiManageFacade.InsertKnowledgeSearchedKeyword(keyword);
            //    }
            //}
            //else if (Session[SessionKeys.KnowledgebaseFilter] != null && !IsSearch)
            //{
            //    bool IsDoc = filter.IsDocumentLibrary;
            //    var filter1 = (QtiFilter)Session[SessionKeys.KnowledgebaseFilter];
            //    if (filter1.IsDeleted != filter.IsDeleted)
            //    {
            //        Session[SessionKeys.KnowledgebaseFilter] = filter;
            //    }
            //    else
            //    {
            //        filter = filter1;
            //        filter.PageNo = pageno;
            //        filter.IsDocumentLibrary = IsDoc;
            //        filter.IsContact = IsContact;
            //        filter.NavList = navlist;
            //        if (IsContact.ToLower() == "true")
            //        {
            //            filter.Tag = "";
            //        }
            //    }
            //}
            //else
            //{
            //    Session[SessionKeys.KnowledgebaseFilter] = null;
            //}


            filter.UserRole = CurrentUser.UserRole.ToLower();
            if (filter.PageNo < 1)
            {
                filter.PageNo = 1;
            }
            filter.PageSize = _Util.Facade.GlobalSettingsFacade.GetLeadPageLength(CurrentUser.CompanyId.Value);
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                filter.SearchText = System.Uri.UnescapeDataString(filter.SearchText);
            }
            ViewBag.Source = filter.SearchText;
            filter.CompanyId = CurrentUser.CompanyId.Value;

            if (filter.IsDownload && !filter.IsDeleted)
            {
                DataTable dt;
                dt = _Util.Facade.CustomerAppoinmentFacade.DownloadKnowledgebasebyFilter(filter);
                int[] colarray = { };
                int[] rowarray = { dt.Rows.Count + 2 };
                dt.Columns.Remove("Id");
                return MakeExcelFromDataTable(dt, "Knowledgebase List", rowarray, colarray);
            }
            else if (filter.IsDownload && filter.IsDeleted)
            {
                DataTable dt;
                dt = _Util.Facade.CustomerAppoinmentFacade.DownloadKnowledgebasebyFilter(filter);
                int[] colarray = { };
                int[] rowarray = { dt.Rows.Count + 2 };
                dt.Columns.Remove("Id");
                return MakeExcelFromDataTable(dt, "Deleted Knowledgebase List", rowarray, colarray);
            }
            filter.UserId = CurrentUser.UserId;
            KnowledgebaseListModel model = _Util.Facade.CustomerAppoinmentFacade.GetKnowledgebasebyFilterForClassroom(filter);
            if (model.KnowledgebaseList.Count <= 0)
            {
                model.KnowledgebaseList = new List<Knowledgebase>();
            }
            model.CompanyId = CurrentUser.CompanyId.Value;
            ViewBag.Order = filter.Order;
            ViewBag.PageNumber = filter.PageNo;
            ViewBag.Source = filter.SearchText;
            ViewBag.OutOfNumber = 0;
            if (model.TotalCount > 0)
            {
                ViewBag.OutOfNumber = model.TotalCount;
            }

            if ((int)ViewBag.PageNumber * filter.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filter.PageSize;
            }

            List<string> list = new List<string>();
            if (!string.IsNullOrWhiteSpace(filter.Tag))
            {
                string[] CBList = filter.Tag.Split(',');
                if (CBList != null)
                {
                    foreach (var item in CBList)
                    {
                        list.Add(item);
                    }
                }
            }

            List<string> list2 = new List<string>();
            if (!string.IsNullOrWhiteSpace(filter.NavList[0]))
            {
                string[] CBList2 = filter.NavList[0].Split(',');
                if (CBList2 != null)
                {
                    foreach (var item in CBList2)
                    {
                        list2.Add(item);
                    }
                }
            }

            ViewBag.TagList = list;
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filter.PageSize);
            ViewBag.SearchText = !string.IsNullOrWhiteSpace(filter.SearchText) ? filter.SearchText : "";
            ViewBag.Flagged = filter.IsFlaged;
            ViewBag.CheckNav = list2;
            ViewBag.IsDeleted = filter.IsDeleted;
            ViewBag.IsAdmin = filter.IsAdmin;
            if (filter.IsDeleted)
            {
                return View("DeletedKnowledgebaseList", model);
            }
            return View(model);
        }
        public ActionResult KnowledgebaseList(QtiFilter filter, bool IsSearch)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool IsEmpty = true;
            string IsContact = filter.IsContact;
            List<string> navlist = filter.NavList;
            filter.UserId = CurrentUser.UserId;
            int pageno = filter.PageNo; 
            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                IsEmpty = false;
            }
            else if (!string.IsNullOrEmpty(filter.Tag) && filter.Tag != "null")
            {
                IsEmpty = false;
            }
            else if (filter.IsFlaged)
            {
                IsEmpty = false;
            }
            if (!IsEmpty)
            {
                if (IsContact.ToLower() == "true")
                {
                    filter.Tag = "";
                }
                Session[SessionKeys.KnowledgebaseFilter] = filter;

                if (!string.IsNullOrWhiteSpace(filter.SearchText))
                {
                    KnowledgeSearchedKeyword keyword = new KnowledgeSearchedKeyword();
                    keyword.Keyword = filter.SearchText;
                    keyword.SearchedBy = CurrentUser.UserId;
                    keyword.SearchedDate = DateTime.UtcNow;
                    _Util.Facade.CustomerAppoinmentFacade.InsertKnowledgeSearchedKeyword(keyword);
                }
            }
            else if (Session[SessionKeys.KnowledgebaseFilter] != null && !IsSearch)
            {
                bool IsDoc = filter.IsDocumentLibrary;
                var filter1 = (QtiFilter)Session[SessionKeys.KnowledgebaseFilter];
                if (filter1.IsDeleted != filter.IsDeleted)
                {
                    Session[SessionKeys.KnowledgebaseFilter] = filter;
                }
                else
                {
                    filter = filter1;
                    filter.PageNo = pageno;
                    filter.IsDocumentLibrary = IsDoc;
                    filter.IsContact = IsContact;
                    filter.NavList = navlist;
                    if (IsContact.ToLower() == "true")
                    {
                        filter.Tag = "";
                    }
                }
            }
            else
            {
                Session[SessionKeys.KnowledgebaseFilter] = null;
            }


            filter.UserRole = CurrentUser.UserRole.ToLower();
            if (filter.PageNo < 1)
            {
                filter.PageNo = 1;
            }
            filter.PageSize = _Util.Facade.GlobalSettingsFacade.GetLeadPageLength(CurrentUser.CompanyId.Value);
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                filter.SearchText = System.Uri.UnescapeDataString(filter.SearchText);
            }
            ViewBag.Source = filter.SearchText;
            filter.CompanyId = CurrentUser.CompanyId.Value;

            if (filter.IsDownload && !filter.IsDeleted)
            {
                DataTable dt;
                dt = _Util.Facade.CustomerAppoinmentFacade.DownloadKnowledgebasebyFilter(filter);
                int[] colarray = { };
                int[] rowarray = { dt.Rows.Count + 2 };
                dt.Columns.Remove("Id");
                return MakeExcelFromDataTable(dt, "Knowledgebase List", rowarray, colarray);
            }
            else if (filter.IsDownload && filter.IsDeleted)
            {
                DataTable dt;
                dt = _Util.Facade.CustomerAppoinmentFacade.DownloadKnowledgebasebyFilter(filter);
                int[] colarray = { };
                int[] rowarray = { dt.Rows.Count + 2 };
                dt.Columns.Remove("Id");
                return MakeExcelFromDataTable(dt, "Deleted Knowledgebase List", rowarray, colarray);
            }
            KnowledgebaseListModel model = _Util.Facade.CustomerAppoinmentFacade.GetKnowledgebasebyFilter(filter);
            if (model.KnowledgebaseList.Count <= 0)
            {
                model.KnowledgebaseList = new List<Knowledgebase>();
            }
            model.CompanyId = CurrentUser.CompanyId.Value;
            ViewBag.Order = filter.Order;
            ViewBag.PageNumber = filter.PageNo;
            ViewBag.Source = filter.SearchText;
            ViewBag.OutOfNumber = 0;
            if (model.TotalCount > 0)
            {
                ViewBag.OutOfNumber = model.TotalCount;
            }

            if ((int)ViewBag.PageNumber * filter.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filter.PageSize;
            }

            List<string> list = new List<string>();
            if (!string.IsNullOrWhiteSpace(filter.Tag))
            {
                string[] CBList = filter.Tag.Split(',');
                if (CBList != null)
                {
                    foreach (var item in CBList)
                    {
                        list.Add(item);
                    }
                }
            }

            List<string> list2 = new List<string>();
            if (!string.IsNullOrWhiteSpace(filter.NavList[0]))
            {
                string[] CBList2 = filter.NavList[0].Split(',');
                if (CBList2 != null)
                {
                    foreach (var item in CBList2)
                    {
                        list2.Add(item);
                    }
                }
            }

            ViewBag.TagList = list;
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filter.PageSize);
            ViewBag.SearchText = !string.IsNullOrWhiteSpace(filter.SearchText) ? filter.SearchText : "";
            ViewBag.Flagged = filter.IsFlaged;
            ViewBag.CheckContact = IsContact.ToLower();
            ViewBag.CheckNav = list2;
            ViewBag.IsDeleted = filter.IsDeleted;
            if (filter.IsDeleted)
            {
                return View("DeletedKnowledgebaseList", model);
            }
            return View(model);
        }
        public JsonResult ReadAssignKnowledgebaseByUser(int Id, bool IsRead)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool result = false;
            KnowledgebaseAccountability exist = _Util.Facade.CustomerAppoinmentFacade.GetKnowledgebaseAccountabilityChecked(Id, currentLoggedIn.UserId.ToString());
            if (exist != null)
            {
                if (IsRead)
                {
                    exist.EndDate = DateTime.UtcNow;
                }
                else
                {
                    exist.EndDate = null;
                }
                exist.IsRead = IsRead;
                result = _Util.Facade.CustomerAppoinmentFacade.UpdateKnowledgebaseAccountability(exist);
            }
            return Json(new { result = result, Id = Id }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AssignedKnowledgebaseListForUser()
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<KnowledgebaseAccountability> assigned = _Util.Facade.CustomerAppoinmentFacade.GetAllAccessedKnowledgebaseListForUser(currentLoggedIn.UserId);
            if (assigned == null)
            {
                assigned = new List<KnowledgebaseAccountability>();
            }
            return View(assigned);
        }
        [HttpPost]
        public JsonResult SortKnowledgebaseFileList(string order, int Id)
        {
            Knowledgebase model = new Knowledgebase();
            bool result = false;
            if (!string.IsNullOrWhiteSpace(order))
            {
                model.SavedImages = _Util.Facade.CustomerAppoinmentFacade.GetSortListOfEstimateImage(Id, order, false);
                if (model.SavedImages != null)
                {
                    foreach (var item in model.SavedImages)
                    {
                        item.StrUploadedDate = item.UploadedDate.UTCToClientTime().ToString("M/dd/yy");
                        item.Extension = item.ImageLoc.Substring(item.ImageLoc.LastIndexOf(".") + 1).ToUpper();
                        if (item.Size> 0)
                        {
                            item.Size = System.Math.Round(item.Size, 2);
                        }
                    }
                    if (order == "descending/type")
                    {
                        model.SavedImages = model.SavedImages.OrderByDescending(x => x.Extension).ToList();
                    }
                    if (order == "ascending/type")
                    {
                        model.SavedImages = model.SavedImages.OrderBy(x => x.Extension).ToList();
                    }
                }
                result = true;
            }
            return Json(new { result = result, ImagesList = model.SavedImages });
        }
        public JsonResult SaveAssignKnowledgebaseToUser(int Id, List<string> UserIdList)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool result = false;
            if (UserIdList != null && UserIdList.Count > 0)
            {
                foreach (string item in UserIdList)
                {
                    KnowledgebaseAccountability exist = _Util.Facade.CustomerAppoinmentFacade.GetKnowledgebaseAccountability(Id, item);
                    if (exist == null)
                    {
                        KnowledgebaseAccountability ka = new KnowledgebaseAccountability()
                        {
                            KnowledgebaseId = Id,
                            AssignedUser = Guid.Parse(item),
                            AssignedDate = DateTime.UtcNow,
                            IsRead = false,
                            AssignedBy = currentLoggedIn.UserId
                        };
                        result = _Util.Facade.CustomerAppoinmentFacade.InsertKnowledgebaseAccountability(ka);

                        #region Insert notification
                        Knowledgebase knowledge = _Util.Facade.CustomerAppoinmentFacade.GetKnowledgebase(Id);
                        string knowledgeLink = AppConfig.DomainSitePath + string.Format("/knowledgebase/Id={0}", Id);
                        Notification notification = new Notification()
                        {
                            CompanyId = currentLoggedIn.CompanyId.Value,
                            CreatedDate = ka.AssignedDate,
                            NotificationId = Guid.NewGuid(),
                            Type = "Accountability",
                            Who = Guid.Parse(item),
                            What = string.Format(@"An article <a class='cus-anchor' target='blank' href='{1}'>{0}</a> has been assigned to you.", knowledge.Title, knowledgeLink),
                            NotificationUrl = knowledgeLink
                        };
                        _Util.Facade.NotificationFacade.InsertNotification(notification);
                        #endregion
                        #region set user to notification
                        NotificationUser nu = new NotificationUser()
                        {
                            NotificationId = notification.NotificationId,
                            IsRead = false,
                            NotificationPerson = Guid.Parse(item),
                        };
                        _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
                        #endregion
                    }
                    else
                    {
                        exist.AssignedDate = DateTime.UtcNow;
                        exist.AssignedBy = currentLoggedIn.UserId;
                        exist.IsRead = false;
                        result = _Util.Facade.CustomerAppoinmentFacade.UpdateKnowledgebaseAccountability(exist);

                        #region Insert notification
                        Knowledgebase knowledge = _Util.Facade.CustomerAppoinmentFacade.GetKnowledgebase(Id);
                        string knowledgeLink = AppConfig.DomainSitePath + string.Format("/knowledgebase/Id={0}", Id);
                        Notification notification = new Notification()
                        {
                            CompanyId = currentLoggedIn.CompanyId.Value,
                            CreatedDate = exist.AssignedDate,
                            NotificationId = Guid.NewGuid(),
                            Type = "Accountability",
                            Who = Guid.Parse(item),
                            What = string.Format(@"An article <a class='cus-anchor' target='blank' href='{1}'>{0}</a> has been re-assigned to you.", knowledge.Title, knowledgeLink),
                            NotificationUrl = knowledgeLink
                        };
                        _Util.Facade.NotificationFacade.InsertNotification(notification);
                        #endregion
                        #region set user to notification
                        NotificationUser nu = new NotificationUser()
                        {
                            NotificationId = notification.NotificationId,
                            IsRead = false,
                            NotificationPerson = Guid.Parse(item),
                        };
                        _Util.Facade.NotificationFacade.InsertNotificationUser(nu);
                        #endregion
                    }
                }
            }
            return Json(new { result = result, Id = Id }, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult AssignedToClassroom()
        //{
        //    if (!base.IsPermitted(UserPermissions.MenuPermissions.LeftMenuAssignArticle))
        //    {
        //        return PartialView("~/Views/Shared/_AccessDenied.cshtml");
        //    }
        //    ViewBag.FavouriteList = _Util.Facade.LookupFacade.GetAllLookupByKey("CategorySystem").OrderBy(x => x.DisplayText).Select(x => new SelectListItem()
        //    {
        //        Text = x.DisplayText,
        //        Value = x.DataValue
        //    }).ToList();
        //    return View();
        //}
        public ActionResult AssignedToClassroom(string Start, string End, int pageno, int pagesize, bool IsAdminAssigned)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (!base.IsPermitted(UserPermissions.MenuPermissions.LeftMenuAssignArticle))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            ViewBag.FavouriteList = _Util.Facade.LookupFacade.GetAllLookupByKey("CategorySystem").OrderBy(x => x.DisplayText).Select(x => new SelectListItem()
            {
                Text = x.DisplayText,
                Value = x.DataValue
            }).ToList();
            ViewBag.InstallerList = _Util.Facade.EmployeeFacade.GetAllUserListForMultiSelect(currentLoggedIn.CompanyId.Value);
            ViewBag.StartDate = Start;
            ViewBag.EndDate = End;
            ViewBag.pagesize = pagesize;
            ViewBag.pageno = pageno;
            ViewBag.IsAdminm = IsAdminAssigned;
            return View();
        }
        public JsonResult UnassignKnowledgebase(int Id, Guid UserId)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool result = false;
            KnowledgebaseAccountability exist = _Util.Facade.CustomerAppoinmentFacade.GetKnowledgebaseAccountabilityChecked(Id, UserId.ToString());
            if (exist != null)
            {
                result = _Util.Facade.CustomerAppoinmentFacade.DeleteKnowledgebaseAccountability(exist.Id);
                _Util.Facade.NotificationFacade.DeleteAssignedArticleNotificationByNotificationUrl("/knowledgebase/Id=" + Id);
            }

            return Json(new { result = result, Id = Id }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AssignKnowledgebaseToUser(int Id)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            ViewBag.InstallerList = _Util.Facade.EmployeeFacade.GetAllUserListForMultiSelect(currentLoggedIn.CompanyId.Value);
            ViewBag.KnowledgebaseId = Id;
            AssignedUserModel model = new AssignedUserModel();
            model.Users = new List<UserDetails>();
            model.UserIds = new List<Guid>();
            UserDetails us = new UserDetails();
            List<KnowledgebaseAccountability> list = _Util.Facade.CustomerAppoinmentFacade.GetKnowledgebaseAccountabilityByKnowledgebaseId(Id);
            if (list != null && list.Count() > 0)
            {
                foreach (var item in list)
                {
                    model.UserIds.Add(item.AssignedUser);
                    us.UserId = item.AssignedUser;
                    us.IsRead = item.IsRead;
                    Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(item.AssignedUser);
                    if (emp != null)
                    {
                        us.UserName = emp.FirstName + " " + emp.LastName;
                    }
                    model.Users.Add(us);
                    us = new UserDetails();
                }
            }
            return View(model);
        }
        public JsonResult ClearFilterSession()
        {
            bool result = true;
            Session[SessionKeys.KnowledgebaseFilter] = null;
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ArticleSetting()
        {
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            return View();
        }
        public ActionResult ArticleSettingList()
        {
            try
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                List<GlobalSetting> SettingList = new List<GlobalSetting>();
                List<GlobalSetting> Settings = _Util.Facade.GlobalSettingsFacade.GetSalesGlobalSettingsByCompanyIdAndTag(CurrentUser.CompanyId.Value, "KnowledgeBaseSetting");
                foreach (var item in Settings)
                {
                    if (item.SearchKey == "AssignedArticlesDefaultDueDate")
                    {
                        item.SearchKey = "Assigned Articles Default Due Date";
                        SettingList.Add(item);
                    }
                    else
                    {
                        SettingList.Add(item);
                    }
                }
                return PartialView("ArticleSettingList", SettingList);
            }
            catch (Exception)
            {
                return PartialView("~/Views/Shared/_NotFound.cshtml");
            }
        }
        [Authorize]
        public ActionResult ShowArticle(int Id)
        { 
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool IsDeleted = false;
            var favouritedata = _Util.Facade.CustomerFacade.GetKnowledgebaseFavouriteUser(Id, currentLoggedIn.UserId);
            if (favouritedata != null)
            {
                ViewBag.IsFavourite = favouritedata.IsFavourite;
            }
            else
            {
                ViewBag.IsFavourite = false;
            }
            
            ViewBag.RemovedFlag = IsPermitted(UserPermissions.QTIPermission.RemoveDocumentFlag);
            if (Session[SessionKeys.KnowledgebaseFilter] != null)
            {
                QtiFilter sessionvalue = (QtiFilter)Session[SessionKeys.KnowledgebaseFilter];
                if (!string.IsNullOrWhiteSpace(sessionvalue.SearchText))
                {
                    ViewBag.SearchText = sessionvalue.SearchText;
                }
                if (!string.IsNullOrWhiteSpace(sessionvalue.Tag) && sessionvalue.Tag != "null")
                {
                    ViewBag.Tag = sessionvalue.Tag;
                }
                IsDeleted = sessionvalue.IsDeleted;
            }
            Knowledgebase knowledge = _Util.Facade.CustomerAppoinmentFacade.GetKnowledgebase(Id);
            if (knowledge == null)
            {
                return PartialView("~/Views/Shared/_NotFound.cshtml");
            }
            else if (knowledge.IsDeleted == true && !currentLoggedIn.UserRole.ToLower().Contains("admin"))
            {
                bool admin = false;
                if (currentLoggedIn.UserRole.ToLower().IndexOf("admin") > -1)
                {
                    admin = true;
                }
                var FlagUser = _Util.Facade.CustomerAppoinmentFacade.GetFlagUserForKnowledgebase(currentLoggedIn.UserId, knowledge.Id, false, admin);
                knowledge.IsFlag = FlagUser != null ? FlagUser.IsFlag : false;
                return PartialView("~/Views/Shared/_Deleted.cshtml", knowledge);
            }
            bool isadmin = false;
            if (currentLoggedIn.UserRole.ToLower().IndexOf("admin") > -1)
            {
                isadmin = true;
            }
            var knowFlagUser = _Util.Facade.CustomerAppoinmentFacade.GetFlagUserForKnowledgebase(currentLoggedIn.UserId, knowledge.Id, false, isadmin);
            if (knowFlagUser != null && knowFlagUser.IsFlag && knowFlagUser.CreatedBy != Guid.Empty)
            {
                Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(knowFlagUser.CreatedBy);
                if (emp != null)
                {
                    knowledge.FlagByName = String.Format("{0} {1}", emp.FirstName, emp.LastName);
                }
            }
            knowledge.SavedImages = _Util.Facade.CustomerAppoinmentFacade.GetEstimateImage(knowledge.Id.ToString(), false);
            knowledge.KnowledgeWeblinkList = _Util.Facade.CustomerAppoinmentFacade.GetWeblinksListByKnowledgebaseId(knowledge.Id);
            if (!string.IsNullOrWhiteSpace(ViewBag.SearchText) || !string.IsNullOrWhiteSpace(ViewBag.Tag))
            {
                knowledge.SearchedKnowledgebase = _Util.Facade.CustomerAppoinmentFacade.SearchListOfKnowledgebase(ViewBag.SearchText, ViewBag.Tag, knowledge.Id, IsDeleted);
            }
            KnowledgebaseAccessedHistory visited = new KnowledgebaseAccessedHistory();
            visited.KnowledgebaseId = knowledge.Id;
            visited.IsDocumentLibrary = knowledge.IsDocumentLibrary;
            visited.VisitedBy = currentLoggedIn.UserId;
            visited.VisitedDate = DateTime.UtcNow;
            _Util.Facade.CustomerAppoinmentFacade.InsertKnowledgebaseAccessedHistory(visited);

            knowledge.IsFlag = knowFlagUser != null ? knowFlagUser.IsFlag : false;
            knowledge.Comments = knowFlagUser != null ? knowFlagUser.Comment : "";
            knowledge.FlagDate = knowFlagUser != null ? knowFlagUser.LastUpdatedDate : new DateTime();
            if (currentLoggedIn.UserRole.ToLower().IndexOf("admin") > -1)
            {
                List<KnowledgeBaseFlagUserCustom> comments = _Util.Facade.CustomerAppoinmentFacade.GetFlagUserCommentForKnowledgebase(knowledge.Id, false);
                knowledge.ListKnowledgeBaseFlagUser = comments;
            }
            else
            {
                List<KnowledgeBaseFlagUserCustom> comments = _Util.Facade.CustomerAppoinmentFacade.GetFlagUserCommentForKnowledgebase(knowledge.Id, currentLoggedIn.UserId, false);
                knowledge.ListKnowledgeBaseFlagUser = comments;
            }
            knowledge.IsAssigned = _Util.Facade.CustomerAppoinmentFacade.GetKnowledgebaseAccountabilityChecked(knowledge.Id, currentLoggedIn.UserId.ToString());
            if (knowledge.IsAssigned != null)
            {
                Employee assignedby = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(knowledge.IsAssigned.AssignedBy);
                if (assignedby != null)
                {
                    knowledge.IsAssigned.AssignedByUserName = assignedby.FirstName + " " + assignedby.LastName;
                }
            }

            return View(knowledge);
        }

        public JsonResult UndoKnowledgebase(int id)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool result = false;
            if (id > 0)
            {
                Knowledgebase knowledge = _Util.Facade.CustomerAppoinmentFacade.GetKnowledgebase(id);
                if (knowledge != null)
                {
                    knowledge.IsDeleted = false;
                    knowledge.LastUpdatedBy = currentLoggedIn.UserId;
                    knowledge.LastUpdatedDate = DateTime.UtcNow;
                    result = _Util.Facade.CustomerAppoinmentFacade.UpdateKnowledgebase(knowledge);
                }
            }
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }







        #endregion KnowledgeBase End
        [Authorize]
        public PartialViewResult AllFundingData(int? PageNo, string SearchText, string SearchBy, string ColumnName, string AscOrDescVal, string FromDate, string ToDate, string order, string InvoiceStatus)
        {
            if (!base.IsPermitted(UserPermissions.SalesPermissions.AllSalesTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            #region Initial DataReady
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (!PageNo.HasValue)
            {
                PageNo = 1;
            }
            if (SearchBy == "-1")
            {
                SearchBy = "";
            }
            int PageSize = 10;
            #endregion
            if (!string.IsNullOrWhiteSpace(AscOrDescVal))
            {
                ViewBag.AscOrDescVal = AscOrDescVal;
            }
            else
            {
                AscOrDescVal = "asc";
                ViewBag.AscOrDescVal = AscOrDescVal;
            }
            AllSalesInfoModel CustomerTransactions = _Util.Facade.TransactionFacade.GetAllTransactionsByCompanyId(CurrentUser.CompanyId.Value, PageSize, PageNo.Value, SearchText, SearchBy, ColumnName, AscOrDescVal, FromDate, ToDate, order, InvoiceStatus);


            ViewBag.SearchByList = _Util.Facade.LookupFacade.GetDropdownsByKey("InvoiceSearchBy");
            ViewBag.InvoiceTypeList = _Util.Facade.LookupFacade.GetDropdownsByKey("Invoice Status");

            CustomerTransactions.SearchBy = SearchBy;
            CustomerTransactions.SearchText = SearchText;
            CustomerTransactions.InvoiceStatus = InvoiceStatus;

            #region PagingReady
            ViewBag.order = order;
            ViewBag.PageNumber = PageNo;
            ViewBag.OutOfNumber = 0;
            if (CustomerTransactions.TransactionList.Count() > 0)
            {
                ViewBag.OutOfNumber = CustomerTransactions.TotalCount;
            }

            if ((int)ViewBag.PageNumber * PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / PageSize);
            #endregion

            return PartialView("_AllFundingData", CustomerTransactions);
        }

        [Authorize]
        public ActionResult DownLoadAllFundingData(int? PageNo, string SearchText, string SearchBy, string ColumnName, string AscOrDescVal, string FromDate, string ToDate, string order)
        {
            if (!base.IsPermitted(UserPermissions.SalesPermissions.AllSalesTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            #region Initial DataReady
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (!PageNo.HasValue)
            {
                PageNo = 1;
            }
            if (SearchBy == "-1")
            {
                SearchBy = "";
            }
            int PageSize = 10;
            #endregion
            if (!string.IsNullOrWhiteSpace(AscOrDescVal))
            {
                ViewBag.AscOrDescVal = AscOrDescVal;
            }
            else
            {
                AscOrDescVal = "asc";
                ViewBag.AscOrDescVal = AscOrDescVal;
            }
            DataTable dt;
            dt = _Util.Facade.TransactionFacade.DownLoadAllTransactionsByCompanyId(CurrentUser.CompanyId.Value, PageSize, PageNo.Value, SearchText, SearchBy, ColumnName, AscOrDescVal, FromDate, ToDate, order);

            int[] colarray = { 5, 6 };
            int[] rowarray = { dt.Rows.Count + 2 };

            return MakeExcelFromDataTable(dt, "AllSales", rowarray, colarray);
            //ViewBag.SearchByList = _Util.Facade.LookupFacade.GetLookupByKey("InvoiceSearchBy").ToList().Select(x =>
            //                new SelectListItem()
            //                {
            //                    Text = x.DisplayText.ToString(),
            //                    Value = x.DataValue.ToString()
            //                }).ToList();
            //CustomerTransactions.SearchBy = SearchBy;
            //CustomerTransactions.SearchText = SearchText;

            //#region PagingReady
            //ViewBag.order = order;
            //ViewBag.PageNumber = PageNo;
            //ViewBag.OutOfNumber = 0;
            //if (CustomerTransactions.TransactionList.Count() > 0)
            //{
            //    ViewBag.OutOfNumber = CustomerTransactions.TotalCount;
            //}

            //if ((int)ViewBag.PageNumber * PageSize > (int)ViewBag.OutOfNumber)
            //{
            //    ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            //}
            //else
            //{
            //    ViewBag.CurrentNumber = (int)ViewBag.PageNumber * PageSize;
            //}
            //ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / PageSize);
            //#endregion

            //return PartialView("_AllFundingData", CustomerTransactions);
        }



        [Authorize]
        public PartialViewResult AllInvoiceTabs()
        {
            return PartialView("_AllInvoiceTabs");
        }
        [Authorize]
        public PartialViewResult SalesARBInvoices(AllInvoicesFilter filter, string BillicycleIdList, string InvoicestatusIdList)
        {
            if (!base.IsPermitted(UserPermissions.SalesPermissions.SalesInvoiceTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (filter == null)
            {
                filter = new AllInvoicesFilter();
            }
            if (filter.StartDate != new DateTime() && filter.EndDate != new DateTime())
            {
                filter.StartDate = filter.StartDate.SetZeroHour();
                filter.EndDate = filter.EndDate.SetMaxHour();
            }
            List<string> billcycle = new List<string>();

            if (!string.IsNullOrWhiteSpace(BillicycleIdList))
            {
                string[] BillList = BillicycleIdList.Split(',');
                if (BillList != null)
                {
                    foreach (var item in BillList)
                    {
                        billcycle.Add(item);
                    }
                }

            }
            ViewBag.BilCycleList = billcycle;

            List<string> invoicestatus = new List<string>();

            if (!string.IsNullOrWhiteSpace(InvoicestatusIdList))
            {
                string[] InvList = InvoicestatusIdList.Split(',');
                if (InvList != null)
                {
                    foreach (var item in InvList)
                    {
                        invoicestatus.Add(item);
                    }
                }

            }
            ViewBag.InvoiceStatusList = invoicestatus;
            #region Initial DataReady
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (!filter.PageNo.HasValue)
            {
                filter.PageNo = 1;
            }
            if (filter.SearchBy == "-1")
            {
                filter.SearchBy = "";
            }
            filter.PageSize = 50;
            if (string.IsNullOrEmpty(filter.BillingCycle))
            {
                filter.BillingCycle = "-1";
            }
            filter.CompanyId = CurrentUser.CompanyId.Value;
            #endregion
            SalesARBInvoices Invoices = new SalesARBInvoices();
            Invoices = _Util.Facade.InvoiceFacade.GetAllARBInvoicesByfilter(filter, BillicycleIdList, InvoicestatusIdList);

            #region PagingReady
            ViewBag.PageNumber = filter.PageNo;
            ViewBag.OutOfNumber = 0;
            if (Invoices.InvoiceList.Count() > 0)
            {
                ViewBag.OutOfNumber = Invoices.Summary.TotalCount;
            }

            if ((int)ViewBag.PageNumber * filter.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filter.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filter.PageSize);

            //ViewBag.BillCycle = _Util.Facade.LookupFacade.GetLookupByKey("BillCycle").Select(x =>
            //              new SelectListItem()
            //              {
            //                  Text = x.DataValue.ToString() == "-1" ? "Billing Cycle" : x.DisplayText.ToString(),
            //                  Value = x.DataValue.ToString()
            //              }).ToList();
            List<SelectListItem> billcyclelist = new List<SelectListItem>();
            billcyclelist.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("BillCycle").Select(x => new SelectListItem()
            {
                Text = x.DataValue.ToString() == "-1" ? "Billing Cycle" : x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.BillCycle = billcyclelist;

            List<SelectListItem> invoicestatuslist = new List<SelectListItem>();
            invoicestatuslist.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("InvoiceStatusForSales").Select(x => new SelectListItem()
            {
                Text = x.DataValue.ToString() == "-1" ? "Invoice Status" : x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.InvoiceStatus = invoicestatuslist;
            #endregion
            ViewBag.invstatus = filter.Status;
            return PartialView("ACHInvoices", Invoices);
        }

        [Authorize]
        public PartialViewResult CCnvoices(AllInvoicesFilter filter)
        {
            if (!base.IsPermitted(UserPermissions.SalesPermissions.SalesInvoiceTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (filter == null)
            {
                filter = new AllInvoicesFilter();
            }
            #region Initial DataReady
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (!filter.PageNo.HasValue)
            {
                filter.PageNo = 1;
            }
            if (filter.SearchBy == "-1")
            {
                filter.SearchBy = "";
            }
            filter.PageSize = 50;
            if (string.IsNullOrEmpty(filter.BillingCycle))
            {
                filter.BillingCycle = "Monthly";
            }
            filter.CompanyId = CurrentUser.CompanyId.Value;
            #endregion
            SalesARBInvoices Invoices = new SalesARBInvoices();
            Invoices = _Util.Facade.InvoiceFacade.GetAllARBInvoicesByfilter(filter, null, null);
            #region PagingReady
            ViewBag.PageNumber = filter.PageNo;
            ViewBag.OutOfNumber = 0;
            if (Invoices.InvoiceList.Count() > 0)
            {
                ViewBag.OutOfNumber = Invoices.Summary.TotalCount;
            }

            if ((int)ViewBag.PageNumber * filter.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filter.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filter.PageSize);

            ViewBag.BillCycle = _Util.Facade.LookupFacade.GetLookupByKey("BillCycle").Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DataValue.ToString() == "-1" ? "Billing Cycle" : x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList();

            #endregion

            return PartialView();
        }

        [Authorize]
        public PartialViewResult AllInvoices(AllInvoicesFilter filter, string BillicycleIdList, string InvoicestatusIdList)
        {

            if (!base.IsPermitted(UserPermissions.SalesPermissions.SalesInvoiceTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (filter == null)
            {
                filter = new AllInvoicesFilter();
            }
            if (filter.StartDate != new DateTime() && filter.EndDate != new DateTime())
            {
                filter.StartDate = filter.StartDate.SetZeroHour();
                filter.EndDate = filter.EndDate.SetMaxHour();
            }
            List<int> idlist = new List<int>();
            ViewBag.order = filter.order;
            List<string> billcycle = new List<string>();

            if (!string.IsNullOrWhiteSpace(BillicycleIdList))
            {
                string[] BillList = BillicycleIdList.Split(',');
                if (BillList != null)
                {
                    foreach (var item in BillList)
                    {
                        billcycle.Add(item);
                    }
                }

            }
            ViewBag.BilCycleList = billcycle;

            List<string> invoicestatus = new List<string>();

            if (!string.IsNullOrWhiteSpace(InvoicestatusIdList))
            {
                string[] InvList = InvoicestatusIdList.Split(',');
                if (InvList != null)
                {
                    foreach (var item in InvList)
                    {
                        invoicestatus.Add(item);
                    }
                }

            }
            ViewBag.InvoiceStatusList = invoicestatus;
            //AllInvoicesModel Invoices = new AllInvoicesModel(); 
            SalesARBInvoices Invoices = new SalesARBInvoices();
            #region Initial DataReady
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (!filter.PageNo.HasValue)
            {
                filter.PageNo = 1;
            }
            if (filter.SearchBy == "-1")
            {
                filter.SearchBy = "";
            }
            filter.PageSize = 50;
            //if(string.IsNullOrEmpty(filter.BillingCycle))
            //{
            //    filter.BillingCycle = "Monthly";
            //}
            filter.CompanyId = CurrentUser.CompanyId.Value;
            #endregion
            TotalCustomerCount totalCustomer = _Util.Facade.InvoiceFacade.GetTotalInvoiceAmountByCompanyId(filter).FirstOrDefault();
            ViewBag.TotalCustomer = totalCustomer.Counter;
            ViewBag.TotalAmount = totalCustomer.TotalAmount;

            //Invoices = _Util.Facade.InvoiceFacade.GetAllInvoiceByCompanyIdAndFilter(filter);
            Invoices = _Util.Facade.InvoiceFacade.GetAllInvoicesByfilter(filter, BillicycleIdList, InvoicestatusIdList);
            Invoices.SearchText = filter.SearchText;
            ViewBag.invstatus = filter.Status;
            #region PagingReady
            ViewBag.PageNumber = filter.PageNo;
            ViewBag.OutOfNumber = 0;
            if (Invoices.InvoiceList.Count() > 0)
            {
                ViewBag.OutOfNumber = Invoices.Summary.TotalCount;
            }

            if ((int)ViewBag.PageNumber * filter.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filter.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filter.PageSize);
            ViewBag.BillCycle = _Util.Facade.LookupFacade.GetLookupByKey("BillCycle").Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DataValue.ToString() == "-1" ? "Billing Cycle" : x.DisplayText.ToString(),
                              Value = x.DataValue.ToString()
                          }).ToList();

            #endregion
            List<SelectListItem> billcyclelist = new List<SelectListItem>();
            billcyclelist.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("BillCycle").Select(x => new SelectListItem()
            {
                Text = x.DataValue.ToString() == "-1" ? "Billing Cycle" : x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.BillCycle = billcyclelist;

            List<SelectListItem> invoicestatuslist = new List<SelectListItem>();
            invoicestatuslist.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("InvoiceStatusForSales").Select(x => new SelectListItem()
            {
                Text = x.DataValue.ToString() == "-1" ? "Invoice Status" : x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.InvoiceStatus = invoicestatuslist;
            ViewBag.idlist = Invoices.Summary.InvoiceIdList;

            return PartialView("_AllInvoices", Invoices);
        }

        [Authorize]
        public PartialViewResult AllInventory()
        {
            return PartialView("_AllInventory");
        }
        [Authorize]
        public PartialViewResult AllReturns(string StartDate, string EndDate)
        {
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;

            ViewBag.ReturnedPaymentFilters = _Util.Facade.LookupFacade.GetDropdownsByKey("ReturnedPaymentFilters");

            return PartialView("_AllReturns");
        }
        [Authorize]
        public PartialViewResult SubscribedCustomers(string StartDate, string EndDate)
        {
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            List<string> SubscribtionStatusList = new List<string>();
            CustomerSubscriptionStatus customerstatus = new CustomerSubscriptionStatus();
            List<CustomerSubscriptionStatus> customerstatusList = new List<CustomerSubscriptionStatus>();
            SubscribtionStatusList.Add("active");
            SubscribtionStatusList.Add("suspended");

            SubscribtionStatusList.Add("others");



            foreach (var item in SubscribtionStatusList)
            {
                customerstatus = new CustomerSubscriptionStatus();
                customerstatus.status = item;
                customerstatusList.Add(customerstatus);
            }
            List<SelectListItem> StatusList = new List<SelectListItem>();

            StatusList.AddRange(customerstatusList.Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.status,
                                      Value = x.status
                                  }).ToList());
            ViewBag.StatusList = StatusList;

            List<string> Paymentmethodlist = new List<string>();
            CustomerPaymentMethod customerPaymentMethod = new CustomerPaymentMethod();
            List<CustomerPaymentMethod> customerpaymentmethodList = new List<CustomerPaymentMethod>();

            Paymentmethodlist.Add("Credit Card");
            Paymentmethodlist.Add("ACH");
            Paymentmethodlist.Add("Invoice");




            foreach (var item in Paymentmethodlist)
            {
                customerPaymentMethod = new CustomerPaymentMethod();
                customerPaymentMethod.Paymentmethod = item;
                customerpaymentmethodList.Add(customerPaymentMethod);
            }
            List<SelectListItem> PaymentMethodList = new List<SelectListItem>();

            PaymentMethodList.AddRange(customerpaymentmethodList.Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.Paymentmethod,
                                      Value = x.Paymentmethod
                                  }).ToList());
            ViewBag.PaymentMethodList = PaymentMethodList;
            return PartialView("_SubscribedCustomers");
        }
        [Authorize]
        public PartialViewResult AllReturnsByFilter(AllReturnsFilter filter)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (filter.PageNo < 1)
            {
                filter.PageNo = 1;
            }
            filter.PageSize = 20;
            filter.CompanyId = CurrentUser.CompanyId.Value;
            DeclinedTransactionView Model = _Util.Facade.DeclinedTransactionsFacade.GetAllDeclinedTransactionsByFilter(filter);
            ViewBag.PageNumber = filter.PageNo;
            ViewBag.OutOfNumber = 0;
            ViewBag.order = filter.Order;
            ViewBag.OutOfNumber = Model.TotalCount;
            if ((int)ViewBag.PageNumber * filter.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filter.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filter.PageSize);
            return PartialView("_AllReturnsByFilter", Model);
        }
        [Authorize]
        public ActionResult AllCustomerByFilter(AllCustomerFilter filter, bool? GetReport)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (filter.PageNo < 1)
            {
                filter.PageNo = 1;
            }
            filter.PageSize = 20;
            filter.CompanyId = CurrentUser.CompanyId.Value;
            CustomerListWithCountModel CustomerResultModel = new CustomerListWithCountModel();

            if (GetReport.HasValue && GetReport == true)
            {
                DataTable dt;
                //if (!string.IsNullOrWhiteSpace(filter.StartDate) && !string.IsNullOrWhiteSpace(End))
                //{
                //    StartDate = Convert.ToDateTime(Start).SetZeroHour().ClientToUTCTime();
                //    EndDate = Convert.ToDateTime(End).SetMaxHour().ClientToUTCTime();

                //    dt = _Util.Facade.CustomerFacade.GetAllHudsonFollowupReportByCompany(CurrentUser.CompanyId.Value, StartDate, EndDate, status, market, leads, soldBy, SearchText, StatusIDList, SalesopenerList, LeadsourceIdList, SalespersonList);
                //}
                //else
                //{
                //    if (StartDate != new DateTime() && EndDate != new DateTime())
                //    {
                //        dt = _Util.Facade.CustomerFacade.GetAllHudsonFollowupReportByCompany(CurrentUser.CompanyId.Value, StartDate, EndDate, status, market, leads, soldBy, SearchText, StatusIDList, SalesopenerList, LeadsourceIdList, SalespersonList);
                //    }
                //else
                //{
                dt = _Util.Facade.DeclinedTransactionsFacade.GetAllCustomerByfilterDownload(CurrentUser.CompanyId.Value, filter.StartDate, filter.EndDate, filter.Status, filter.Paymentmethod, filter.SearchText);
                //}

                //}
                // dt.Columns.Remove("Id");
                int[] colarray = { 7 };
                int[] rowarray = { dt.Rows.Count + 2 };
                return MakeExcelFromDataTable(dt, "Subscribed Customer ", rowarray, colarray);
            }


            CustomerResultModel = _Util.Facade.DeclinedTransactionsFacade.GetAllCustomerByFilter(filter);
            ViewBag.PageNumber = filter.PageNo;
            ViewBag.OutOfNumber = 0;
            ViewBag.order = filter.Order;
            ViewBag.OutOfNumber = CustomerResultModel.TotalCustomerCount.Counter;
            if ((int)ViewBag.PageNumber * filter.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filter.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filter.PageSize);
            return PartialView("_AllCustomersByFilter", CustomerResultModel);
        }

        [Authorize]
        public PartialViewResult UploadDeclineReport()
        {

            return PartialView("_UploadDeclineReport");
        }

        [Authorize]
        public PartialViewResult AllReceivePayments(int? PageNo, string SearchText, string SearchBy, string order, string StartDate, string EndDate)
        {
            if (!base.IsPermitted(UserPermissions.SalesPermissions.AllSalesReceivePayment))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            DateTime FromDatetime = new DateTime();
            DateTime EndDatetime = new DateTime();
            if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate))
            {

                DateTime.TryParse(StartDate, out FromDatetime);
                DateTime.TryParse(EndDate, out EndDatetime);
            }
            #region Initial DataReady
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (!PageNo.HasValue)
            {
                PageNo = 1;
            }
            if (SearchBy == "-1")
            {
                SearchBy = "";
            }
            int PageSize = 10;

            #endregion
            AllSalesInfoModel CustomerTransactions = new AllSalesInfoModel();
            //List<Invoice> InvoiceList = _Util.Facade.InvoiceFacade.GetAllReceivePaymentsByCompanyId(currentLoggedIn.CompanyId.Value);
            // List<Transaction> CustomerTransactions2 = _Util.Facade.TransactionFacade.GetAllTransactionsByCompanyId(CurrentUser.CompanyId.Value);

            CustomerTransactions = _Util.Facade.InvoiceFacade.GetAllReceivePaymentsByCompanyIdAndFilter(CurrentUser.CompanyId.Value, PageNo.Value, PageSize, SearchBy, SearchText, order, FromDatetime, EndDatetime);

            #region PagingReady
            ViewBag.order = order;
            ViewBag.PageNumber = PageNo;
            ViewBag.OutOfNumber = 0;
            if (CustomerTransactions.TransactionList.Count() > 0)
            {
                ViewBag.OutOfNumber = CustomerTransactions.TotalCount;
            }

            if ((int)ViewBag.PageNumber * PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / PageSize);
            #endregion
            ViewBag.Searchtext = SearchText;

            return PartialView("_AllReceivePayments", CustomerTransactions);
            //return PartialView();
        }
        //public PartialViewResult AllReceivePaymentsList(int? PageNo, string SearchText, string SearchBy, string order, string StartDate, string EndDate)
        //{
        //    if (!base.IsPermitted(UserPermissions.SalesPermissions.AllSalesReceivePayment))
        //    {
        //        return PartialView("~/Views/Shared/_AccessDenied.cshtml");
        //    }
        //    DateTime FromDatetime = new DateTime();
        //    DateTime EndDatetime = new DateTime();
        //    if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate))
        //    {

        //        DateTime.TryParse(StartDate, out FromDatetime);
        //        DateTime.TryParse(EndDate, out EndDatetime);
        //    }
        //    #region Initial DataReady
        //    var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
        //    if (!PageNo.HasValue)
        //    {
        //        PageNo = 1;
        //    }
        //    if (SearchBy == "-1")
        //    {
        //        SearchBy = "";
        //    }
        //    int PageSize = 10;

        //    #endregion
        //    AllSalesInfoModel CustomerTransactions = new AllSalesInfoModel();
        //    //List<Invoice> InvoiceList = _Util.Facade.InvoiceFacade.GetAllReceivePaymentsByCompanyId(currentLoggedIn.CompanyId.Value);
        //    // List<Transaction> CustomerTransactions2 = _Util.Facade.TransactionFacade.GetAllTransactionsByCompanyId(CurrentUser.CompanyId.Value);

        //    CustomerTransactions = _Util.Facade.InvoiceFacade.GetAllReceivePaymentsByCompanyIdAndFilter(CurrentUser.CompanyId.Value, PageNo.Value, PageSize, SearchBy, SearchText, order, FromDatetime, EndDatetime);

        //    #region PagingReady
        //    ViewBag.order = order;
        //    ViewBag.PageNumber = PageNo;
        //    ViewBag.OutOfNumber = 0;
        //    if (CustomerTransactions.TransactionList.Count() > 0)
        //    {
        //        ViewBag.OutOfNumber = CustomerTransactions.TotalCount;
        //    }

        //    if ((int)ViewBag.PageNumber * PageSize > (int)ViewBag.OutOfNumber)
        //    {
        //        ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
        //    }
        //    else
        //    {
        //        ViewBag.CurrentNumber = (int)ViewBag.PageNumber * PageSize;
        //    }
        //    ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / PageSize);
        //    #endregion


        //    return PartialView(CustomerTransactions);
        //}
        public ActionResult DownloadAllReceivePayments(int? PageNo, string SearchText, string SearchBy, string order, string StartDate, string EndDate)
        {
            DateTime FromDatetime = new DateTime();
            DateTime EndDatetime = new DateTime();
            if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate))
            {

                DateTime.TryParse(StartDate, out FromDatetime);
                DateTime.TryParse(EndDate, out EndDatetime);
            }
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (!PageNo.HasValue)
            {
                PageNo = 1;
            }
            if (SearchBy == "-1")
            {
                SearchBy = "";
            }
            int PageSize = 10;

            DataTable dt;
            dt = _Util.Facade.InvoiceFacade.DownloadAllReceivePaymentsByCompanyIdAndFilter(CurrentUser.CompanyId.Value, PageNo, PageSize, SearchBy, SearchText, order, FromDatetime, EndDatetime);
            int[] colarray = { 5, 6 };
            int[] rowarray = { dt.Rows.Count + 2 };
            return MakeExcelFromDataTable(dt, "AccountReceiveable", rowarray, colarray);
        }

        [Authorize]
        public ActionResult GetAllInvoiceData(int Id)
        {
            ViewBag.invoiceid = Id;
            return View("GetAllInvoiceData");
        }
        public ActionResult InvoiceDataPdf(CreateInvoice model)
        {
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public JsonResult AllInvoiceData(string InvId, CreateInvoice Model)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Model.Invoice = _Util.Facade.InvoiceFacade.GetInvoiceById(Convert.ToInt32(InvId));
            Model.InvoiceDetailList = _Util.Facade.InvoiceFacade.GetInvoiceDetailsByInvoiceId(Model.Invoice.InvoiceId);
            if (Model.Invoice == null || Model.Invoice.CompanyId != CurrentUser.CompanyId.Value)
            {
                return Json(new { result = false });
            }
            if (Model.InvoiceDetailList == null)
            {
                return Json(new { result = false, message = "Customer Equipment Found" });
            }
            Customer tempCUstomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(Model.Invoice.CustomerId);
            if (tempCUstomer == null)
            {
                return Json(new { result = false, message = "Customer Not Found" });
            }

            Model.Invoice.CustomerName = tempCUstomer.Title + " " + tempCUstomer.FirstName + " " + tempCUstomer.LastName;
            if (tempCUstomer.BusinessName != "")
            {
                Model.CustomerName = tempCUstomer.BusinessName;
            }
            else
            {
                Model.CustomerName = Model.Invoice.CustomerName;
            }
            foreach (var item in Model.InvoiceDetailList)
            {
                Model.InvoiceEquipmentName = item.EquipName;
                Model.InvoiceEquipmentDescription = item.EquipDetail;
                item.CreatedBy = User.Identity.Name;
                item.CreatedDate = DateTime.Now.UTCCurrentTime();
                item.CompanyId = CurrentUser.CompanyId.Value;
                Model.SubTotal = Model.SubTotal + item.TotalPrice.Value;
                item.EquipmentFile = _Util.Facade.EquipmentFileFacade.GetEquipmentFilesByEquipmentId(item.EquipmentId).Where(x => x.FileType == LabelHelper.EquipmentFileType.ProfilePicture).FirstOrDefault();
                if (item.EquipmentFile == null)
                {
                    item.EquipmentFile = new EquipmentFile();
                }
            }
            if (Model.Invoice.Discountpercent != null)
            {
                Model.Discount = ((Model.Invoice.Discountpercent * Model.SubTotal) / 100).Value;
            }

            Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(Model.Invoice.CompanyId);
            Model.CompanyAddress = tempCom.Address;
            Model.CompanyStreet = tempCom.Street;
            string ComCity = "";
            string ComState = "";
            if (!string.IsNullOrWhiteSpace(tempCom.City))
            {
                ComCity = tempCom.City + ", ";
            }
            if (!string.IsNullOrWhiteSpace(tempCom.State))
            {
                ComState = tempCom.State + " ";
            }
            Model.companyStreetInfo = ComCity + ComState + tempCom.ZipCode;
            Model.CompanyEmail = tempCom.EmailAdress;
            Model.CompanyName = tempCom.CompanyName;
            Model.PhoneNum = tempCom.Phone;
            string tempcompanyBranch = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);
            Model.CompanyLogo = tempcompanyBranch;
            ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("InvoiceDataPdf", Model)
            {
                //FileName = "TestView.pdf",
                PageSize = Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },

            };
            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
            Random rand = new Random();
            string filename = ConfigurationManager.AppSettings["File.InvoiceFiles"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(tempCom.CompanyId).CompanyName.ReplaceSpecialChar();
            filename = string.Format(filename, comname);
            filename += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + rand.Next().ToString() + "_" + rand.Next().ToString() + "_invoice.pdf";
            string Serverfilename = FileHelper.GetFileFullPath(filename);
            FileHelper.SaveFile(applicationPDFData, Serverfilename);
            return Json(new { result = true, message = "Invoice Successfully Saved", filePath = AppConfig.DomainSitePath + filename });
        }


        #region MakeExcel
        private FileContentResult MakeExcelFromDataTable(DataTable dtResult, string ReportFor, int[] rowIndex, int[] coloumnIndex)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                if (dtResult != null)
                {
                    var worksheet = wb.Worksheets.Add(dtResult);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;
                    var format = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey("CurrentCurrencyExcelFormat");
                    if (coloumnIndex != null && format != null && rowIndex != null)
                    {

                        foreach (int itemcol in coloumnIndex)
                        {
                            for (int i = 1; i < rowIndex[0]; i++)
                            {
                                worksheet.Cell(i, itemcol).Style.NumberFormat.Format = format.Value;

                            }
                        }
                    }
                    MemoryStream memorystreem = new MemoryStream();
                    wb.SaveAs(memorystreem);
                    var fName = string.Format("{0}-{1}.xlsx", ReportFor, DateTime.Now.UTCCurrentTime().ToString("MM/dd/yyyy"));

                    byte[] fileContents = memorystreem.ToArray();
                    var userAgent = HttpContext.Request.UserAgent.ToLower();
                    if (userAgent.Contains("iphone;") || userAgent.Contains("ipad;"))
                    {
                        //var newExFile= File(fileContents, Excel.Format("ExcelFormat"), fName);
                        //var excelApplicatiopn = new NsExcel.Application();
                        //excelApplicatiopn.Visible = true;
                        //NsExcel.Workbooks books = excelApplicatiopn.Workbooks;
                        //NsExcel.Workbook sheet = books.Open();
                        return File(fileContents, Excel.Format("ExcelFormat"), fName);
                    }
                    else
                    {
                        return File(fileContents, Excel.Format("ExcelFormat"), fName);
                    }
                }
                else
                {
                    byte[] fileContents = new byte[1];
                    return File(fileContents, Excel.Format("ExcelFormat"), "empty.xlsx");
                }
            }
        }

        private FileContentResult MakeExcelFromDataSet(DataSet dsResult, string ReportFor)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                if (dsResult != null)
                {
                    int cus = 0;
                    //wb.Worksheets.Add(dsResult.Tables[1]);
                    //wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    //wb.Style.Font.Bold = true;
                    for (int k = 0; k < dsResult.Tables.Count; k++)
                    {
                        DataTable dt = dsResult.Tables[k];
                        IXLWorksheet Sheet = wb.Worksheets.Add(dt.Columns[cus].ColumnName);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                Sheet.Cell((i + 2), (j + 1)).Value = dt.Rows[i][j].ToString();
                            }
                        }
                        cus = cus + 1;
                    }

                    MemoryStream memorystreem = new MemoryStream();
                    wb.SaveAs(memorystreem);
                    var fName = string.Format("{0}-{1}.xlsx", ReportFor, DateTime.Now.UTCCurrentTime().ToString("MM/dd/yyyy"));

                    byte[] fileContents = memorystreem.ToArray();

                    return File(fileContents, Excel.Format("ExcelFormat"), fName);

                }
                else
                {
                    byte[] fileContents = new byte[1];
                    return File(fileContents, Excel.Format("ExcelFormat"), "empty.xlsx");
                }
            }

        }

        #endregion


        //[Authorize]
        //public ActionResult FilterAllFundingData(string FromDate, string ToDate)
        //{
        //    var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
        //    AllSalesInfoModel FilterListTransaction = _Util.Facade.TransactionFacade.GetAllFilterTransactionsByCompanyId(CurrentUser.CompanyId.Value, FromDate, ToDate);
        //    return View("_FilterAllFundingData", FilterListTransaction);
        //}

        #region Recurring Billing Mismatch
        public ActionResult RecurringBillingMismatch()
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            return View("RecurringBillingMismatch");
        }
        public ActionResult UnResolvedRecurringBillingMismatch()
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            return View("UnResolvedRecurringBillingMismatch");
        }
        public ActionResult UnResolvedRecurringBillingMismatchPartial(DateTime? Start, DateTime? End, int pageno, int pagesize, string SearchText)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            ViewBag.StartDate = Start;
            ViewBag.EndDate = End;
            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();
            string newCookie = "";
            if (Request.Cookies[CookieKeys.DateViewFilter] != null && !string.IsNullOrWhiteSpace(Request.Cookies[CookieKeys.DateViewFilter].Value))
            {
                newCookie = Request.Cookies[CookieKeys.DateViewFilter].Value;
                newCookie = Server.UrlDecode(newCookie);
                var CookieVals = newCookie.Split(',');

                if (CookieVals.Length == 3)
                {
                    StartDate = CookieVals[0].ToDateTime();
                    EndDate = CookieVals[1].ToDateTime();
                }
            }

            RMRBillingMismatchModel Model = new RMRBillingMismatchModel();
            Model = _Util.Facade.TransactionFacade.GetUnResolveRecurringBillingMismatchList(StartDate, EndDate, pageno, pagesize, SearchText);

            ViewBag.PageNumber = pageno;
            ViewBag.OutOfNumber = 0;
            ViewBag.order = null;
            if (ViewBag.order == null)
            {
                ViewBag.order = 0;
            }
            if (Model.TotalCount.TotalCount > 0)
            {
                ViewBag.OutOfNumber = Model.TotalCount.TotalCount;
            }

            if ((int)ViewBag.PageNumber * pagesize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * pagesize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / pagesize);
            return View(Model);
        }
        public ActionResult DownloadUnResolvedRecurringBillingMismatchPartial(DateTime? Start, DateTime? End, string searchtext)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            ViewBag.StartDate = Start;
            ViewBag.EndDate = End;
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();
            string newCookie = "";
            if (Request.Cookies[CookieKeys.DateViewFilter] != null && !string.IsNullOrWhiteSpace(Request.Cookies[CookieKeys.DateViewFilter].Value))
            {
                newCookie = Request.Cookies[CookieKeys.DateViewFilter].Value;
                newCookie = Server.UrlDecode(newCookie);
                var CookieVals = newCookie.Split(',');

                if (CookieVals.Length == 3)
                {
                    StartDate = CookieVals[0].ToDateTime();
                    EndDate = CookieVals[1].ToDateTime();
                }
            }
            DataTable dt;
            dt = _Util.Facade.TransactionFacade.DownloadUnResolveRecurringBillingMismatchList(StartDate, EndDate, searchtext);
            int[] colarray = { 4, 5 };
            int[] rowarray = { dt.Rows.Count + 2 };
            return MakeExcelFromDataTable(dt, "UnResolvedRecurringBillingMismatch", rowarray, colarray);
        }

        public ActionResult ResolvedRecurringBillingMismatch()
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            return View("ResolvedRecurringBillingMismatch");
        }
        public ActionResult ResolvedRecurringBillingMismatchPartial(DateTime? Start, DateTime? End, int pageno, int pagesize, string SearchText)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            ViewBag.StartDate = Start;
            ViewBag.EndDate = End;
            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();
            string newCookie = "";
            if (Request.Cookies[CookieKeys.DateViewFilter] != null && !string.IsNullOrWhiteSpace(Request.Cookies[CookieKeys.DateViewFilter].Value))
            {
                newCookie = Request.Cookies[CookieKeys.DateViewFilter].Value;
                newCookie = Server.UrlDecode(newCookie);
                var CookieVals = newCookie.Split(',');

                if (CookieVals.Length == 3)
                {
                    StartDate = CookieVals[0].ToDateTime();
                    EndDate = CookieVals[1].ToDateTime();
                }
            }

            RMRBillingMismatchModel Model = new RMRBillingMismatchModel();
            Model = _Util.Facade.TransactionFacade.GetResolveRecurringBillingMismatchList(StartDate, EndDate, pageno, pagesize, SearchText);

            ViewBag.PageNumber = pageno;
            ViewBag.OutOfNumber = 0;
            ViewBag.order = null;
            if (ViewBag.order == null)
            {
                ViewBag.order = 0;
            }
            if (Model.TotalCount.TotalCount > 0)
            {
                ViewBag.OutOfNumber = Model.TotalCount.TotalCount;
            }

            if ((int)ViewBag.PageNumber * pagesize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * pagesize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / pagesize);
            return View(Model);
        }
        public ActionResult DownloadResolvedRecurringBillingMismatchPartial(DateTime? Start, DateTime? End, string searchtext)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            ViewBag.StartDate = Start;
            ViewBag.EndDate = End;
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime();
            string newCookie = "";
            if (Request.Cookies[CookieKeys.DateViewFilter] != null && !string.IsNullOrWhiteSpace(Request.Cookies[CookieKeys.DateViewFilter].Value))
            {
                newCookie = Request.Cookies[CookieKeys.DateViewFilter].Value;
                newCookie = Server.UrlDecode(newCookie);
                var CookieVals = newCookie.Split(',');

                if (CookieVals.Length == 3)
                {
                    StartDate = CookieVals[0].ToDateTime();
                    EndDate = CookieVals[1].ToDateTime();
                }
            }
            DataTable dt;
            dt = _Util.Facade.TransactionFacade.DownloadResolveRecurringBillingMismatchList(StartDate, EndDate, searchtext);
            int[] colarray = { 4, 5 };
            int[] rowarray = { dt.Rows.Count + 2 };
            return MakeExcelFromDataTable(dt, "ResolvedRecurringBillingMismatch", rowarray, colarray);
        }
        [HttpPost]
        public JsonResult ConvertUnResolvedToResolved(int Id, bool isResolved)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool result = false;
            if (CurrentUser == null)
            {
                return Json(result);
            }
            if (!CurrentUser.CompanyId.HasValue)
            {
                return Json(result);
            }
            if (Id != 0 && isResolved == true)
            {
                RMRBillingMismatch RMR = _Util.Facade.TransactionFacade.GetRecurringBillingMismatchById(Id);
                if (RMR != null)
                {
                    RMR.IsResolved = true;
                    RMR.ResolvedBy = CurrentUser.UserId;
                    RMR.ResolvedDate = DateTime.Now.UTCCurrentTime();
                    result = _Util.Facade.TransactionFacade.UpdateRecurringBillingMismatch(RMR);
                }
                else
                {
                    result = false;
                }
            }
            return Json(result);

        }

        #endregion

        #region Recurring Billing Invoice Generate
        [Authorize]
        public PartialViewResult AllRecurringBillingUppaidInvoice()
        {
            List<CustomerPaymentMethod> customerpaymentmethodList = new List<CustomerPaymentMethod>(){
                new CustomerPaymentMethod(){ Paymentmethod = "Credit Card"},
                new CustomerPaymentMethod(){ Paymentmethod = "ACH"},
                new CustomerPaymentMethod(){ Paymentmethod = "Invoice"}
            };
            List<SelectListItem> PaymentMethodList = new List<SelectListItem>();
            PaymentMethodList.Add(new SelectListItem() { Text = "Credit Card", Value = "CreditCard" });
            PaymentMethodList.Add(new SelectListItem() { Text = "ACH", Value = "ACH" });
            PaymentMethodList.Add(new SelectListItem() { Text = "Invoice", Value = "Invoice" });
            ViewBag.PaymentMethodList = PaymentMethodList;
            return PartialView("_AllRecurringBillingUppaidInvoice");
        }
        [Authorize]
        public PartialViewResult AllRecurringBillingUppaidInvoiceByFilter(AllCustomerFilter filter)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (!CurrentUser.CompanyId.HasValue)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            filter.CompanyId = CurrentUser.CompanyId.Value;
            List<ARBUnpaidInvoiceGenerateList> ViewModel = _Util.Facade.InvoiceFacade.GetAllRecurringBillingUppaidInvoiceByFilter(filter);
            if (ViewModel == null)
            {
                ViewModel = new List<ARBUnpaidInvoiceGenerateList>();
            }
            else
            {
                ViewModel = ViewModel.OrderByDescending(x => x.Id).ToList();
            }
            ViewBag.TotalCustomer = ViewModel.Count;
            return PartialView("_AllRecurringBillingUppaidInvoiceByFilter", ViewModel);
        }
        [Authorize]
        public ActionResult DownloadUnpaidRMRInvoicesExcel(string StrCustomerIdlist, string InvoiceType, string PaymentMethod)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (!CurrentUser.CompanyId.HasValue)
            {
                return View("~/Views/Shared/_AccessDenied.cshtml");
            }
            try
            {
                if (!string.IsNullOrEmpty(StrCustomerIdlist) && !string.IsNullOrWhiteSpace(StrCustomerIdlist))
                {
                    Guid CompanyId = CurrentUser.CompanyId.Value;
                    DataTable dt = _Util.Facade.InvoiceFacade.DownloadUnpaidRMRInvoices(StrCustomerIdlist, InvoiceType, PaymentMethod, CompanyId);
                    string BatchNumber = DateTime.Now.ToString("yyMMddHHmmss").GenerateInvoiceBatchNo();
                    return MakeExcelFromDataTable(dt, "BatchNo-" + BatchNumber + "-RMRExcel", null, null);
                }
                else
                {
                    return PartialView("~/Views/Shared/_FileNotFound.cshtml");
                }
            }
            catch (Exception)
            {
                return PartialView("~/Views/Shared/_FileNotFound.cshtml");
            }
        }
        #endregion
    }
}
