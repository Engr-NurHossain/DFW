using HS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HS.Web.UI.Controllers
{
    public class TechScheduleSettingController : BaseController
    {
        // GET: TechScheduleSetting
        public ActionResult Index()
        {
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            return View();
        }

        public ActionResult TechScheduleSettingPartial()
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<TechSchedule> Techsetting = _Util.Facade.TechScheduleFacade.GetAllTechSettingByCompanyId(currentLoggedIn.CompanyId.Value);
            return View("TechScheduleSettingPartial", Techsetting);
        }

        [Authorize]
        public ActionResult AddTechScheduleSetting(int? id)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            TechSchedule model;
            if (id.HasValue)
            {
                model = _Util.Facade.TechScheduleFacade.GetTechScheduleById(id.Value);
            }
            else
            {
                model = new TechSchedule();
            }
            ViewBag.EmployeeList = _Util.Facade.EmployeeFacade.GetAllEmployee(currentLoggedIn.CompanyId.Value).Select(x =>
                                    new SelectListItem()
                                    {
                                        Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                                        Value = x.UserId.ToString()
                                    }).ToList();
            ViewBag.CustomerList = _Util.Facade.CustomerFacade.GetAllCustomerByCompanyId(currentLoggedIn.CompanyId.Value).Select(x =>
                                      new SelectListItem()
                                      {
                                          Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                                          Value = x.CustomerId.ToString()
                                      }).ToList();
            return View("AddTechScheduleSetting", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddTechScheduleSetting(TechSchedule tss)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            tss.CompanyId = currentLoggedIn.CompanyId.Value;

            if (tss.Id > 0)
            {

                _Util.Facade.TechScheduleFacade.UpdateTechSchedule(tss);
            }
            else
            {

                _Util.Facade.TechScheduleFacade.InsertTechSchedule(tss);
            }
            return Json(true);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteTechSetting(int? id)
        {
            if (id.HasValue)
            {
                var Techval = _Util.Facade.TechScheduleFacade.DeleteTechSchedule(id.Value);
            }

            return Json(true);
        }
    }
}