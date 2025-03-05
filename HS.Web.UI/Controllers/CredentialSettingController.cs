using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HS.Web.UI.Controllers
{
    public class CredentialSettingController : BaseController
    {
        // GET: CredentialSetting
        public ActionResult Index()
        {
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            return View();
        }
        [Authorize]
        public ActionResult CredentialSettingPartial()
        {

            if (!base.IsPermitted(UserPermissions.ToolsPermissions.CredentialSettings))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<CredentialSetting> credential = _Util.Facade.CredentialSettingFacade.GetAllCredentialSettingListByCompanyId(currentLoggedIn.CompanyId.Value);
            return View("CredentialSettingPartial", credential);
        }
        [Authorize]
        public ActionResult AddCredentialSetting(int ? id)
        {
            CredentialSetting model;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            //if(currentLoggedIn == null)
            //{
            //    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            //}
            if (id.HasValue)
            {
                if (!base.IsPermitted(UserPermissions.ToolsPermissions.CredentialSettingsEdit))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                model = _Util.Facade.CredentialSettingFacade.GetById(id.Value);
            }
            else
            {
                if (!base.IsPermitted(UserPermissions.ToolsPermissions.CredentialSettingsAdd))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                model = new CredentialSetting();
            }

            ViewBag.AccountHolderList = _Util.Facade.AccountHolderFacade.GetAllAccountHolderByCompanyId(currentLoggedIn.CompanyId.Value).Select(x =>
                            new SelectListItem()
                            {
                                Text = x.Name,
                                Value = x.Id.ToString()
                            }).ToList();
            return PartialView("AddCredentialSetting",model);
            
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddCredentialSetting(CredentialSetting credential)
        {
            credential.IsActive = true;
            credential.CompanyId = ((HS.Web.UI.Helper.CustomPrincipal)(User)).CompanyId.Value;
            if (credential.Id > 0)
            {
                if (!base.IsPermitted(UserPermissions.ToolsPermissions.CredentialSettingsEdit))
                {
                    return Json(false);
                }

                _Util.Facade.CredentialSettingFacade.UpdateCredentialSetting(credential);
            }
            else
            {
                if (!base.IsPermitted(UserPermissions.ToolsPermissions.CredentialSettingsAdd))
                {
                    return Json(false);
                }
                _Util.Facade.CredentialSettingFacade.InsertCredentialSetting(credential);
            }
            return Json(true);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteCredentialSetting(int? id)
        {
            if (!base.IsPermitted(UserPermissions.ToolsPermissions.CredentialSettingsDelete))
            {
                return Json(false);
            }
            if (id.HasValue)
            {
                var credentialDelete = _Util.Facade.CredentialSettingFacade.DeleteCredentialSetting(id.Value);
            }

            return Json(true);
        }
    }
}