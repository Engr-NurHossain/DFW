using HS.Entities;
using HS.Framework;
using HS.Web.UI.Helper;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HS.Web.UI.Controllers
{
    public class ServiceAreaController : BaseController
    {
        public ServiceAreaController()
        {
            logger = LogManager.GetCurrentClassLogger();
        }
        // GET: ServiceArea
        public ActionResult Index()
        {
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            return View();
        }
        public ActionResult ZipcodeList()
        {
            
            return View();
        }
        public ActionResult AreaZipCode(int? PageNumber, int? UnitPerPage, string SearchText)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (PageNumber == 0)
            {
                PageNumber = 1;
            }
            GlobalSetting glob = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "CustomerListPageSize");
            if (glob != null)
            {
                UnitPerPage = Convert.ToInt32(glob.Value);
            }
            else
            {
                UnitPerPage = 10;
            }
            ViewBag.OutOfNumber = _Util.Facade.ServiceAreaFacade.GetAllZip().Count;
            if ((int)ViewBag.OutOfNumber == 0)
            {
                ViewBag.Message = "No Content Available !";
            }
            if (PageNumber == null || PageNumber == 0)
            {
                PageNumber = 1;
            }

            if (@ViewBag.OutOfNumber == 0)
            {
                PageNumber = 1;
            }
            ViewBag.PageNumber = PageNumber;

            if ((int)ViewBag.PageNumber * UnitPerPage > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * UnitPerPage;
            }

            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / UnitPerPage.Value);

            List<ServiceAreaZipcode> zipcodeList = _Util.Facade.ServiceAreaFacade.GetAllZipCode(Convert.ToInt32(PageNumber), Convert.ToInt32(UnitPerPage), SearchText);
            return View(zipcodeList);
        }
        [Authorize]
        public ActionResult AddZipCode(int? Id)
        {
            var CurrenUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            ServiceAreaZipcode Model = new ServiceAreaZipcode();

            if (Id.HasValue && Id > 0)
            {
                Model = _Util.Facade.ServiceAreaFacade.GetById(Id.Value);        
            } 
            return View(Model);
        }
        [Authorize]
        public ActionResult DeleteZipCode(int? Id)
        {
            bool result = false;
            try
            {
                _Util.Facade.ServiceAreaFacade.DeleteServiceZipCode(Id.Value);
                result = true;
            }
            catch(Exception ex)
            {
                logger.Error(ex);
            }
            return Json(new { result = true});
        }
        [HttpPost]
        public ActionResult SaveZipCode(ServiceAreaZipcode AreaZipCode)
        {
            bool result = false;
            if(AreaZipCode.Id > 0)
            {
                _Util.Facade.ServiceAreaFacade.UpdateServiceZipCode(AreaZipCode);
                result = true;
            }
            else
            {
                try
                {
                    AreaZipCode.Id = (int)_Util.Facade.ServiceAreaFacade.InsertServiceZipCode(AreaZipCode);

                    result = true;
                }
                catch (Exception ex)
                {
                    result = false;
                }
            }
            return Json(new { result = result, message = "Saved Successfully." });
        }

    }
}