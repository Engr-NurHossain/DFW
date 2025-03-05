using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HS.Web.UI.Controllers
{
    public class PanelController : BaseController
    {
        // GET: Panel
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

        public ActionResult CustomerPanelPartial()
        {
            if (!base.IsPermitted(UserPermissions.MenuPermissions.QuickMenuProductsCustomerPanelType))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<PanelType> paneltype = _Util.Facade.PanelTypeFacade.GetAllPanelTypeByCompanyId(currentLoggedIn.CompanyId.Value);
            return PartialView("CustomerPanelPartial", paneltype);
        }
        [Authorize]
        public ActionResult AddPanelType(int? id)
        {
            PanelType model;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (id.HasValue)
            {
                if (!base.IsPermitted(UserPermissions.ProductsPermissions.CustomerPanelTypeEdit))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                model = _Util.Facade.PanelTypeFacade.GetById(id.Value);
                int EquipmentId = 0;
                if(int.TryParse(model.Value,out EquipmentId) && EquipmentId>0)
                {
                    model.Equipment = _Util.Facade.EquipmentFacade.GetEquipmentById(EquipmentId);
                }else
                {
                    model.Equipment = new Equipment();
                }
                
            }
            else
            {
                if (!base.IsPermitted(UserPermissions.ProductsPermissions.CustomerPanelTypeAdd))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                model = new PanelType();
            }
            return PartialView("AddPanelType", model);
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddPanelType(PanelType pt)
        {
            pt.IsActive = true;
            pt.CompanyId = ((HS.Web.UI.Helper.CustomPrincipal)(User)).CompanyId.Value;

            if (pt.Id > 0)
            {
                if (!base.IsPermitted(UserPermissions.ProductsPermissions.CustomerPanelTypeEdit))
                {
                    return Json(false);
                }
                //pt.Value = pt.Name;
                _Util.Facade.PanelTypeFacade.UpdatePanelType(pt);
            }
            else
            {
                if (!base.IsPermitted(UserPermissions.ProductsPermissions.CustomerPanelTypeAdd))
                {
                    return Json(false);
                }
                //pt.Value = pt.Name;
                _Util.Facade.PanelTypeFacade.InsertPanelType(pt);
            }
            return Json(true);
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeletePanel(int? id)
        {
            if (!base.IsPermitted(UserPermissions.ProductsPermissions.CustomerPanelTypeDelete))
            {
                return Json(false);
            }
            if (id.HasValue)
            {
                var panelval = _Util.Facade.PanelTypeFacade.DeletePanelType(id.Value);
            }

            return Json(true);
        }
    }
}