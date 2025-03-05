using HS.Entities;
using HS.Framework;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HS.Web.UI.Controllers
{
    public class ActivationFeeController : BaseController
    {
        // GET: ActivationFee
        public ActivationFeeController()
        {
            logger = LogManager.GetCurrentClassLogger();
        }
        public ActionResult Index()
        {
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            return View();
        }

        public ActionResult ActivationFeePartial()
        {

            if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.ActivationFeeTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<ActivationFee> activation = _Util.Facade.ActivationFeeFacade.GetAllActivationFeeByCompanyId(currentLoggedIn.CompanyId.Value);
            return PartialView("ActivationFeePartial", activation);
        }

        [Authorize]
        public ActionResult AddActivationFee(int? id)
        {
            ActivationFee model;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (id.HasValue)
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.ActivationFeeEdit))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }

                model = _Util.Facade.ActivationFeeFacade.GetById(id.Value);
            }
            else
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.ActivationFeeAdd))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                model = new ActivationFee();
            }

            return PartialView("AddActivationFee", model);
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddActivationFee(ActivationFee activation)
        { 
            var result = false;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return Json(result);
            }
            activation.CompanyId = currentLoggedIn.CompanyId.Value;
            if (activation.Id > 0)
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.ActivationFeeEdit))
                {
                    return Json(false);
                }

                //var nameval = activation.Fee;
                //activation.Name = nameval.ToString();
                result = _Util.Facade.ActivationFeeFacade.UpdateActivationFee(activation);
            }
            else
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.ActivationFeeAdd))
                {
                    return Json(false);
                }
                //var nameval = activation.Fee;
                //activation.Name = nameval.ToString();
                result = _Util.Facade.ActivationFeeFacade.InsertActivationFee(activation) > 0;
            }

            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteActivationFee(int? id)
        { 
            if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.ActivationFeeDelete))
            {
                return Json(false);
            }
            var result = false;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return Json(result);
            }
            if (id.HasValue)
            {


                result = _Util.Facade.ActivationFeeFacade.DeleteActivationFee(id.Value);
            }
            return Json(result);
        }
    }
}