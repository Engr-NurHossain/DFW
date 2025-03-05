using HS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static HS.Entities.AppPermission;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using HS.Web.UI.Helper;
using HS.Framework;
using HS.Entities.List;
using System.Web.Security;
using HS.Web.UI.App_Start;
using System.Net;
using HtmlAgilityPack;
using System.Data;
using System.Globalization;

namespace HS.Web.UI.Controllers
{
    public class CommonsController : BaseController
    {
        [Authorize]
        // GET: Home
        public ActionResult Index(string Key)
        {
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            if (!string.IsNullOrWhiteSpace(Key))
            {
                ViewBag.Key = Key;
            }
            //if (id != null)
            //{
            //    ViewBag.id = id;
            //}

            if (Session["RunScriptOnDocReady"] != null)
            {
                ViewBag.RunScriptOnLogin = Session["RunScriptOnDocReady"].ToString();
                Session["RunScriptOnDocReady"] = null;
            }

            return View();
        }

        

        #region Digiture

        public ActionResult PopDataPartial(string Id)
        {
            if (!base.IsPermitted(UserPermissions.MenuPermissions.LeftMenuInventory))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //var BranchList = _Util.Facade.CompanyBranchFacade.GetAllCompanyBranchByCompanyId(CurrentUser.CompanyId.Value);
            //ViewBag.BranchCount = BranchList.Count;
            //if (!string.IsNullOrWhiteSpace(Id))
            //{
            //    ViewBag.Id = Id;
            //}
            return PartialView("_PopData");
        }

        
        public ActionResult ViewTicket(string Id)
        {
            ViewBag.Id = Id;
            return PartialView("_ViewTicket");
        }

        #endregion Digiture
    }
}