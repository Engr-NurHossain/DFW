using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HS.Framework;
using HS.Entities;
using Microsoft.Office.Interop.Excel;
using System.Configuration;
using HS.Framework.Utils;

namespace HS.Web.UI.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Index()
        {           
            var u = "~/Content/Icons/"+(HS.Framework.Utils.AppConfig.HomePageImage)+"/favicon.ico";
            return View();
        }
        public ActionResult PartialViewForHomePage()
        {
            string Company = AppConfig.HomePageImage;

            if (Company.ToLower() == "hudson")
            {
                ViewBag.Title = "New and Used Bus Sales, Maintenance, Parts Provision | Hudson Bus Sales ";
                return PartialView("Home_Hudson");
            }
            else if (Company.ToLower() == "central")
            {
                ViewBag.Title = "Online Marketing Agency in Dallas-Fort Worth | Central Station Marketing";
                return PartialView("Home_CentralStation");
            }
            else if (Company.ToLower() == "gratecrm")
            {
                ViewBag.Title = "Basement Waterproofing Products | Grate Products";
                return PartialView("Home_Grate");
            }
            else if (Company.ToLower() == "alif")
            {
                ViewBag.Title = "Security & Alarm Systems in Houston, TX | Alif Security";
                return PartialView("Home_Alif");
            }
            else if (Company.ToLower() == "onit")
            {
                ViewBag.Title = "ONIT Smart Home Security Service in Dallas-Fort Worth, TX";
                return PartialView("Home_Onit");
            }
            else if (Company.ToLower() == "apprmr")
            {
                ViewBag.Title = "RMR Cloud | Cloud Based Business Management Software for Security & Automation Dealers";
                return PartialView("Home_app_RMR");
            }
            else if (Company.ToLower() == "rugcleaning")
            {
                ViewBag.Title = "Area Rug Cleaning in Dallas and Fort Worth | Dalworth Rug Cleaning";
                return PartialView("Home_Myrug");
            }
            else if (Company.ToLower() == "dfw")
            {
                ViewBag.Title = "Security & Smart Home | Dallas-Fort Worth | DFW Security";
                return PartialView("Home_dfw");
            }
            else if (Company.ToLower() == "ieatery")
            {
                ViewBag.Title = "iEatery";
                return PartialView("Home_iEatery");
            }
            else
            {
                ViewBag.Title = "RMR Cloud | Cloud Based Business Management Software for Security & Automation Dealers";
                return PartialView("Home_RMR");
            }
        }
        public ActionResult Croncall()
        {
            return null;
        }
        public ActionResult v2()
        {
            return View();
        }
        public ActionResult Time()
        {
            DateTime dt = DateTime.Now.UTCCurrentTime();
            ViewBag.LocalTime = dt.ToString();

            ViewBag.ToUniversalTime = dt.ToUniversalTime().ToString();
            ViewBag.ClientToUTCTime = dt.ClientToUTCTime();
            return View();
        }

        ////[Shariful-20-9-19]
        //public ActionResult GetPartnerList(Guid SupervisorId)
        //{
        //    List<Partner> Model = _Util.Facade.EmployeeFacade.GetEmployeeByPartnerId(SupervisorId);
        //    return View(Model.ToList());
        //}
        ////[~Shariful-20-9-19]

        [HttpPost]
        public JsonResult RequestAdmin(RequestAdmin rd)
        {
            bool res = false;
            RequestAdminEmail email = new RequestAdminEmail();
            email.Name = rd.Name;
            email.Comapny = rd.Company;
            email.Phone = rd.Phone;
            email.Email = rd.Email;
            res = _Util.Facade.MailFacade.SendMailRequestAdmin(email);
            return Json(res);
        }

        public ActionResult ERRORPage()
        {
            ViewBag.CompanyUrl = ConfigurationManager.AppSettings["SiteURL"];
            return View();
        }
    }
}