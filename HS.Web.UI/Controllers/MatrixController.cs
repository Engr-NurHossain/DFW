using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HS.Entities;
using HS.Framework;
using static HS.Framework.UserPermissions;

namespace HS.Web.UI.Controllers
{
    public class MatrixController : BaseController
    {
        // GET: Matrix
        public ActionResult Index()
        {
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            return View();
        }
        public PartialViewResult InstallMatrix()
        {
            if (!base.IsPermitted(MenuPermissions.QuickMenuMyCompanyInstallationMatrix))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
           // List<> matrix = _Util.Facade.MatrixFacade.GetAllSalesMatrixByCompanyId(CurrentUser.CompanyId.Value);
            return PartialView("_InstallMatrix");
        }
        public PartialViewResult SalesMatrixPartial()
        {
            if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.SalesMatrix))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<SalesMatrix> matrix = _Util.Facade.MatrixFacade.GetAllSalesMatrix();
            List<Lookup> LookupList = _Util.Facade.LookupFacade.GetLookupByKey("SalesMatrixType");
            if(matrix != null && matrix.Count() > 0)
            {
                foreach(var item in matrix)
                {
                    if(LookupList.Where(x => x.DataValue == item.Type) != null && LookupList.Where(x => x.DataValue == item.Type).Count() > 0)
                    {
                        item.Type = LookupList.Where(x => x.DataValue == item.Type).FirstOrDefault().DisplayText;
                    }
                    
                }
            }


            return PartialView("_SalesMatrix",matrix);
        }
        [Authorize]
        public PartialViewResult AddSalesMatrix(int? Id)
        {

            SalesMatrix sm = null;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (Id.HasValue)
            {

                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.SalesMatrixEdit))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }

                sm = _Util.Facade.MatrixFacade.GetSalesMatrixById(Id.Value);
            }
            else
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.SalesMatrixAdd))
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }

                sm = new SalesMatrix();
            }

            ViewBag.SalesMatrixType = _Util.Facade.LookupFacade.GetLookupByKey("SalesMatrixType").Select(x =>
                new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString()
                }).ToList();
            
            return PartialView("_AddSalesMatrix",sm);
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddSalesMatrix(SalesMatrix sm)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
             
            if(sm.Id > 0)
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.SalesMatrixEdit))
                {
                    return Json(new { result = false, Message = "Error Mat100 Access denied." });
                }

                SalesMatrix tmpsm = _Util.Facade.MatrixFacade.GetSalesMatrixById(sm.Id);
                tmpsm.Type = sm.Type;
                tmpsm.Min = sm.Min;
                tmpsm.Max = sm.Max;
                tmpsm.UserX = sm.UserX;
                if (sm.Difference == null)
                {
                    sm.Difference = 0;
                }
                tmpsm.Difference = sm.Difference;
                
                bool res =  _Util.Facade.MatrixFacade.UpdateSalesMatrix(tmpsm);
                return Json(new {result = res, message="Sales matrix updated successfully." });
            }
            else
            {
                if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.SalesMatrixAdd))
                {
                    return Json(new { result = false, Message = "Error Mat101 Access denied." });
                }
                sm.SalesMatrixId = Guid.NewGuid();
                bool res = _Util.Facade.MatrixFacade.InsertSalesMatrix(sm)>0;
                return Json(new { result = res, message = "Sales matrix Inserted successfully." });
            }
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteSalesMatrix(int Id)
        {
            if (!base.IsPermitted(UserPermissions.MyCompanyPermissions.SalesMatrixDelete))
            {
                return Json(new { result = false, Message = "Error Mat102 Access denied." });
            }

            _Util.Facade.MatrixFacade.DeleteSalesMatrix(Id);
            return Json(new { result = true, message = "Deelted" });
        }

        public ActionResult MatrixPartial()
        {
            return View("_MatrixPartial");
        }

        public ActionResult CreditClassPartial()
        {
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<CreditClass> CreditClassList = _Util.Facade.CommissionFacade.GetAllCreditClass();
            return View(CreditClassList);
        }

        [Authorize]
        public ActionResult AddCreditClass(int? id)
        {
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CreditClass model = new CreditClass();
            if (id > 0)
            {
                model = _Util.Facade.CommissionFacade.GetCreditClassById(id.Value);
            }
            else
            {
                model = new CreditClass();
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddCreditClass(CreditClass CreditClass)
        {
            bool result = false;
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CreditClass.Id > 0)
            {
                _Util.Facade.CommissionFacade.UpdateCreditClass(CreditClass);
            }
            else
            {
                _Util.Facade.CommissionFacade.InsertCreditClass(CreditClass);
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteCreditClass(int Id)
        {
            bool result = false;
            _Util.Facade.CommissionFacade.DeleteCreditClass(Id);
            return Json(result);
        }
    }
}