using System.Web.Services;
using HS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Protocols;
using HS.Framework;
using NLog;

namespace HS.Web.UI.Controllers
{
    public class AccountHolderController : BaseController
    {
        // GET: AccountHolder

        public AccountHolderController()
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
        
        [Authorize]
        public ActionResult AccountHolderPartial()
        {
            if (!base.IsPermitted(UserPermissions.MenuPermissions.QuickMenuToolsAccountHolder))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            //CusManagment.CreateCustomerInput inputcustomer = new CusManagment.CreateCustomerInput();
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<AccountHolder> accholder = _Util.Facade.AccountHolderFacade.GetAllAccountHolderByCompanyId(currentLoggedIn.CompanyId.Value);

            return PartialView("AccountHolderPartial", accholder);
        }

        [Authorize]
        public ActionResult AddAccountHolder(int? id)
        {
            if (!base.IsPermitted(UserPermissions.ToolsPermissions.AccountHolderAdd))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
         
            AccountHolder model;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (id.HasValue)
            {
                model = _Util.Facade.AccountHolderFacade.GetById(id.Value);
            }
            else
            {
                model = new AccountHolder();
            }
            return PartialView("AddAccountHolder", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddAccountHolder(AccountHolder accHolder)
        {
            accHolder.IsActive = true;
            accHolder.CompanyId = ((HS.Web.UI.Helper.CustomPrincipal)(User)).CompanyId.Value;
            if (accHolder.Id > 0)
            {
                if (!base.IsPermitted(UserPermissions.ToolsPermissions.AccountHolderEdit))
                {
                    return Json(false);
                }
                _Util.Facade.AccountHolderFacade.UpdateAccountHolder(accHolder);
            }
            else
            {
                if (!base.IsPermitted(UserPermissions.ToolsPermissions.AccountHolderAdd))
                {
                    return Json(false);
                }
                _Util.Facade.AccountHolderFacade.InsertAccountHolder(accHolder);
            }
            return Json(true);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteAccHolder(int? id)
        {
            if (!base.IsPermitted(UserPermissions.ToolsPermissions.AccountHolderDelete))
            {
                return Json(false);
            }
            if (id.HasValue)
            {
                var accholderDelete = _Util.Facade.AccountHolderFacade.DeleteAccountHolder(id.Value);
            }

            return Json(true);
        }

        public class AlarmApiService : SoapHeader
        {
            public string UserName { get { return "rezaparves"; } }
            public string Password { get { return "P@rves1200"; } }
        }

        
        
    }
}