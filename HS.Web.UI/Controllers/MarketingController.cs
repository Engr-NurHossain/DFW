using HS.Entities;
using Rotativa;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Web;
using System.Web.Mvc;
using Rotativa.Options;
using HS.Web.UI.Helper;
using System.Configuration;
using HS.Payments.RecurringBilling;
using HS.Payments.CustomerProfiles;
using AuthorizeNet.Api.Contracts.V1;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using HS.Framework;
using HS.Alarm.CustomerManager;
using HtmlAgilityPack;
using System.Net;
using HS.Framework.Utils;
using System.Reflection;
using HS.Web.UI.Controllers;
using Forte.Entities;
using Forte;
using System.Globalization;
using HS.Kickbox.Models;
using System.Net.Http;
using HS.Entities.Custom;
using System.Xml.Linq;
using System.Text;
using System.Diagnostics;
using HS.Web.UI.Models;

namespace HS.Web.UI.Controllers
{
    public class MarketingController : BaseController
    {
        [Authorize]
        // GET: Customer
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

        public PartialViewResult MarketingPartial(string firstdate, string lastdate)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            ViewBag.firstdate = firstdate;
            ViewBag.lastdate = lastdate;
            if (currentLoggedIn != null)
            {
                return PartialView("_Marketing");
            }
            else
            {
                return PartialView();
            }
        }

        public ActionResult Marketing()
        {
            if (!base.IsPermitted(UserPermissions.MenuPermissions.LeftMenuMarketing))
            {
                return View("~/Views/Shared/_AccessDenied.cshtml");
            }

            if (base.IsPermitted(UserPermissions.MarketingPermissions.SMSTab))
            {
                ViewBag.StartTab = "SMSTab";
            }
            else if (base.IsPermitted(UserPermissions.MarketingPermissions.EmailTab))
            {
                ViewBag.StartTab = "EmailTab";
            }
            else if (base.IsPermitted(UserPermissions.MarketingPermissions.SocialMediaTab))
            {
                ViewBag.StartTab = "SocialMediaTab";
            }
            else if (base.IsPermitted(UserPermissions.MarketingPermissions.PaidAdvertisementTab))
            {
                ViewBag.StartTab = "PaidAdvertisementTab";
            }
           
            else
            {
                return View("~/Views/Shared/_AccessDenied.cshtml");
            }


            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            //ViewBag.ispayroll = _Util.Facade.EmployeeFacade.GetEmployeeByUsername(CurrentUser.Identity.Name).IsPayroll;
            return View();
        }
        public PartialViewResult LoadSMSPartial(int? PageNo, int? PageSize, string SearchText, string order)
        {
            //var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //if (!PageNo.HasValue || PageNo.Value < 1)
            //{
            //    PageNo = 1;
            //}
            //if (SearchText == "undefined" || SearchText == null)
            //{
            //    SearchText = "";
            //}
            //if (!PageSize.HasValue || PageSize.Value < 1)
            //{
            //    PageSize = 50;
            //}

            //CategoryListModel Model = _Util.Facade.CategoryFacade.GetCategoryList(PageNo.Value, PageSize.Value, SearchText, order);
            //ViewBag.PageNumber = PageNo;
            //ViewBag.OutOfNumber = 0;
            //ViewBag.order = order;

            //if (Model.Categoriess.Count() > 0)
            //{
            //    ViewBag.OutOfNumber = Model.TotalCount;
            //}

            //if ((int)ViewBag.PageNumber * PageSize > (int)ViewBag.OutOfNumber)
            //{
            //    ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            //}
            //else
            //{
            //    ViewBag.CurrentNumber = (int)ViewBag.PageNumber * PageSize;
            //}
            //ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / PageSize.Value);

            //return PartialView("_CategoryPartial", Model);
            return PartialView("_Marketing");
        }
        public PartialViewResult LoadEmailPartial(int? PageNo, int? PageSize, string SearchText, string order)
        {
            //var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //if (!PageNo.HasValue || PageNo.Value < 1)
            //{
            //    PageNo = 1;
            //}
            //if (SearchText == "undefined" || SearchText == null)
            //{
            //    SearchText = "";
            //}
            //if (!PageSize.HasValue || PageSize.Value < 1)
            //{
            //    PageSize = 50;
            //}

            //CategoryListModel Model = _Util.Facade.CategoryFacade.GetCategoryList(PageNo.Value, PageSize.Value, SearchText, order);
            //ViewBag.PageNumber = PageNo;
            //ViewBag.OutOfNumber = 0;
            //ViewBag.order = order;

            //if (Model.Categoriess.Count() > 0)
            //{
            //    ViewBag.OutOfNumber = Model.TotalCount;
            //}

            //if ((int)ViewBag.PageNumber * PageSize > (int)ViewBag.OutOfNumber)
            //{
            //    ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            //}
            //else
            //{
            //    ViewBag.CurrentNumber = (int)ViewBag.PageNumber * PageSize;
            //}
            //ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / PageSize.Value);

            //return PartialView("_CategoryPartial", Model);
            return PartialView("_Marketing");
        }
        public PartialViewResult LoadSocialMediaPartial(int? PageNo, int? PageSize, string SearchText, string order)
        {
            //var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //if (!PageNo.HasValue || PageNo.Value < 1)
            //{
            //    PageNo = 1;
            //}
            //if (SearchText == "undefined" || SearchText == null)
            //{
            //    SearchText = "";
            //}
            //if (!PageSize.HasValue || PageSize.Value < 1)
            //{
            //    PageSize = 50;
            //}

            //CategoryListModel Model = _Util.Facade.CategoryFacade.GetCategoryList(PageNo.Value, PageSize.Value, SearchText, order);
            //ViewBag.PageNumber = PageNo;
            //ViewBag.OutOfNumber = 0;
            //ViewBag.order = order;

            //if (Model.Categoriess.Count() > 0)
            //{
            //    ViewBag.OutOfNumber = Model.TotalCount;
            //}

            //if ((int)ViewBag.PageNumber * PageSize > (int)ViewBag.OutOfNumber)
            //{
            //    ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            //}
            //else
            //{
            //    ViewBag.CurrentNumber = (int)ViewBag.PageNumber * PageSize;
            //}
            //ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / PageSize.Value);

            //return PartialView("_CategoryPartial", Model);
            return PartialView("_Marketing");
        }
        public PartialViewResult LoadPaidAdvertisementPartial(int? PageNo, int? PageSize, string SearchText, string order)
        {
            //var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //if (!PageNo.HasValue || PageNo.Value < 1)
            //{
            //    PageNo = 1;
            //}
            //if (SearchText == "undefined" || SearchText == null)
            //{
            //    SearchText = "";
            //}
            //if (!PageSize.HasValue || PageSize.Value < 1)
            //{
            //    PageSize = 50;
            //}

            //CategoryListModel Model = _Util.Facade.CategoryFacade.GetCategoryList(PageNo.Value, PageSize.Value, SearchText, order);
            //ViewBag.PageNumber = PageNo;
            //ViewBag.OutOfNumber = 0;
            //ViewBag.order = order;

            //if (Model.Categoriess.Count() > 0)
            //{
            //    ViewBag.OutOfNumber = Model.TotalCount;
            //}

            //if ((int)ViewBag.PageNumber * PageSize > (int)ViewBag.OutOfNumber)
            //{
            //    ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            //}
            //else
            //{
            //    ViewBag.CurrentNumber = (int)ViewBag.PageNumber * PageSize;
            //}
            //ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / PageSize.Value);

            //return PartialView("_CategoryPartial", Model);
            return PartialView("_Marketing");
        }
    }
}
