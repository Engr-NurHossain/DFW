using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HS.Web.UI.Controllers
{
    public class ServiceFeeController : BaseController
    {
        // GET: ServiceFee
        public ActionResult Index()
        {
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            return View();
        }
        public ActionResult ServiceFeePartial()
        {
            if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.ServiceFeeTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<ServiceFee> serviceFee = _Util.Facade.ServiceFeeFacade.GetAllServiceFeeByCompanyId(currentLoggedIn.CompanyId.Value);

            return PartialView("ServiceFeePartial", serviceFee);
        }

        [Authorize]
        public ActionResult AddServiceFee(int? id)
        {
            ServiceFee model;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if(currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (id.HasValue)
            {

                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.ServiceFeeEdit))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }

                model = _Util.Facade.ServiceFeeFacade.GetById(id.Value);
            }
            else
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.ServiceFeeAdd))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                model = new ServiceFee();
            }

            return PartialView("AddServiceFee", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddServiceFee(ServiceFee serviceFee)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            bool result = false;
            if(currentLoggedIn == null)
            {
                return Json(result);
            }
            if(currentLoggedIn != null)
            {
                serviceFee.CompanyId = ((HS.Web.UI.Helper.CustomPrincipal)(User)).CompanyId.Value;
                if (serviceFee.Id > 0)
                {
                    if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.ServiceFeeEdit))
                    {
                        return Json(result);
                    }
                    //var nameval = serviceFee.Fee;
                    //serviceFee.Name = nameval.ToString();
                    result = _Util.Facade.ServiceFeeFacade.UpdateServiceFee(serviceFee);
                  
                }
                else
                {
                    if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.ServiceFeeAdd))
                    {
                        return Json(result);
                    }
                    //double nameval = Fee;
                    //serviceFee = nameval.ToString("0.00");
                    ServiceFee DbServiceFee = new ServiceFee();
                    DbServiceFee.CompanyId = currentLoggedIn.CompanyId.Value;
                    DbServiceFee.Name = serviceFee.Name;
                    DbServiceFee.Fee = serviceFee.Fee;

                    result =  _Util.Facade.ServiceFeeFacade.InsertServiceFee(DbServiceFee) > 0;
                }
            }
            
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteServiceFee(int? id)
        {

            if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.ServiceFeeDelete))
            {
                return Json(false);
            }

            if (id.HasValue)
            {
                var service = _Util.Facade.ServiceFeeFacade.DeleteServiceFee(id.Value);
            }

            return Json(true);
        }
    }
}