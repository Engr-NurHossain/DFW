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
using PermissionList = HS.Framework.UserPermissions;
using PermissionChecker = HS.Web.UI.Helper.PermissionHelper;
using System.Web.Routing;

namespace HS.Web.UI.Controllers
{
    public class TemplateController : BaseController
    {

        protected override void ExecuteCore() { }
        public static string RenderViewToString(string controllerName, string viewName, object viewData)
        {
            using (var writer = new StringWriter())
            {
                var routeData = new RouteData();
                routeData.Values.Add("controller", controllerName);
                var fakeControllerContext = new ControllerContext(new HttpContextWrapper(new HttpContext(new HttpRequest(null, "http://google.com", null), new HttpResponse(null))), routeData, new TemplateController());
                var razorViewEngine = new RazorViewEngine();
                var razorViewResult = razorViewEngine.FindView(fakeControllerContext, viewName, "", false);

                var viewContext = new ViewContext(fakeControllerContext, razorViewResult.View, new ViewDataDictionary(viewData), new TempDataDictionary(), writer);
                razorViewResult.View.Render(viewContext, writer);
                return writer.ToString();

            }
        }
        // GET: Template
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GenerateCustsomerList()
        { 
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)(User);

            List<GridSetting> GridSettings = new List<GridSetting>();
            GridSettings = _Util.Facade.GridSettingsFacade.GetByKey("CustomerGrid", CurrentUser.CompanyId.Value);
            if (GridSettings.Count > 0)
            {
                GridSettings = GridSettings.OrderBy(x => x.OrderBy).Where(x => x.GridActive == true).ToList();
            } 
            List<GridSetting> GridGroupSettings = new List<GridSetting>();
            GridGroupSettings = _Util.Facade.GridSettingsFacade.GetByKey("CustomerGridGroup", CurrentUser.CompanyId.Value);
            if (GridGroupSettings.Count > 0)
            {
                GridGroupSettings = GridGroupSettings.OrderBy(x => x.OrderBy).Where(x => x.GridActive == true).ToList();
            }
            FinalGrid fgrid = new FinalGrid();
            fgrid.GridSetting = GridSettings;
            fgrid.GridGroupSetting = GridGroupSettings;
            //string html = this.RenderViewToString("~/Views/Customer/_GenerateCustomerList.cshtml", GridGroupSettings);
            String renderedHTML = RenderViewToString("Template", "~/Views/Customer/_GenerateCustomerList.cshtml", fgrid);
            string path = Server.MapPath(string.Format(AppConfig.CustomerTemplateFile, CurrentUser.CompanyId.ToString())); 

            if (!System.IO.File.Exists(path))
            { 
                using (FileStream fs = System.IO.File.Create(path))
                {
                    System.IO.File.WriteAllText(path, renderedHTML);
                } 
            }
            else if (System.IO.File.Exists(path))
            {
                System.IO.File.WriteAllText(path, renderedHTML);
            }

            return Json(0);
        }

        public JsonResult GenerateLeadList()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)(User);

            List<GridSetting> GridSettings = new List<GridSetting>();
            GridSettings = _Util.Facade.GridSettingsFacade.GetByKey("LeadGrid", CurrentUser.CompanyId.Value);
            if (GridSettings.Count > 0)
            {
                GridSettings = GridSettings.OrderBy(x => x.OrderBy).Where(x => x.GridActive == true).ToList();
            }
            List<GridSetting> GridGroupSettings = new List<GridSetting>();
            GridGroupSettings = _Util.Facade.GridSettingsFacade.GetByKey("LeadGridGroup", CurrentUser.CompanyId.Value);
            if (GridGroupSettings.Count > 0)
            {
                GridGroupSettings = GridGroupSettings.OrderBy(x => x.OrderBy).Where(x => x.GridActive == true).ToList();
            }
            FinalGrid fgrid = new FinalGrid();
            fgrid.GridSetting = GridSettings;
            fgrid.GridGroupSetting = GridGroupSettings;
            //string html = this.RenderViewToString("~/Views/Customer/_GenerateCustomerList.cshtml", GridGroupSettings);
            String renderedHTML = RenderViewToString("Template", "~/Views/Customer/_GenerateLeadList.cshtml", fgrid);
            string path = Server.MapPath(string.Format(AppConfig.LeadTemplateFile, CurrentUser.CompanyId.ToString()));

            if (!System.IO.File.Exists(path))
            {
                using (FileStream fs = System.IO.File.Create(path))
                {
                    System.IO.File.WriteAllText(path, renderedHTML);
                }
            }
            else if (System.IO.File.Exists(path))
            {
                System.IO.File.WriteAllText(path, renderedHTML);
            }

            return Json(0);
        }

    }
}