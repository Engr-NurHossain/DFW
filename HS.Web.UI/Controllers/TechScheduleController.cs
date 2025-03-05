using HS.Entities;
using HS.Framework;
using HS.Web.UI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HS.Web.UI.Controllers
{
    public class TechScheduleController : BaseController
    {
        // GET: TechSchedule
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
            return View();
        }
        public ActionResult TechSchedulePartial(Guid customerid)
        { 
            if (!base.IsPermitted(UserPermissions.CustomerPermissions.CustomerScheduleTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            //Guid? employeeId =  null;
            //bool isInstaller = false;
            //if (currentLoggedIn.EmployeeId.HasValue)
            //{
            //    isInstaller = true;
            //    employeeId = currentLoggedIn.EmployeeId.Value;
            //}
            List<TechSchedule> techschedule = _Util.Facade.TechScheduleFacade.GetAllTechScheduleByEmployeeId(customerid, currentLoggedIn.CompanyId.Value);

            return PartialView("_TechSchedulePartial",techschedule);
        }
        [Authorize]
        public ActionResult AddTechSchedule(int? id, Guid? customerid, string LoadDate, string IsSchedule)
        {
            if (!base.IsPermitted(UserPermissions.CustomerPermissions.CustomerScheduleAdd))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            TechSchedule model;
            
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (id.HasValue && id.Value > 0)
            {
                model = _Util.Facade.TechScheduleFacade.GetTechScheduleById(id.Value);
                if(!string.IsNullOrWhiteSpace(IsSchedule) && IsSchedule == "true")
                {
                    model.IsSchedule = IsSchedule;
                    var objcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerid.Value);
                    if(objcus != null)
                    {
                        if (!string.IsNullOrWhiteSpace(objcus.BusinessName))
                        {
                            model.CustomerName = objcus.BusinessName;
                        }
                        else
                        {
                            model.CustomerName = objcus.FirstName + " " + objcus.LastName;
                        }
                    }
                }
            }
            else
            {
                model = new TechSchedule();
                if (!string.IsNullOrWhiteSpace(LoadDate) && !string.IsNullOrWhiteSpace(IsSchedule))
                {
                    DateTime datetime = Convert.ToDateTime(LoadDate).SetZeroHour();
                    model.InstallDate = datetime;
                    if (IsSchedule == "true")
                    {
                        model.IsSchedule = IsSchedule;
                    }
                }
            }
            if (customerid.HasValue)
            {
                ViewBag.CustomerId = customerid.Value;
            }
            ViewBag.TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid()).Select(x =>
                                new SelectListItem()
                                {
                                    Text = x.FirstName.ToString(),
                                    Value = x.UserId.ToString()
                                }).ToList();
            ViewBag.Arrival = _Util.Facade.LookupFacade.GetLookupByKey("Arrival").Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.DisplayText.ToString(),
                                      Value = x.DataValue.ToString()
                                  }).ToList();
            ViewBag.EstimatedArrival = _Util.Facade.LookupFacade.GetLookupByKey("EstimatedArrival").Select(x =>
                                     new SelectListItem()
                                     {
                                         Text = x.DisplayText.ToString(),
                                         Value = x.DataValue.ToString()
                                     }).ToList();
            
            return PartialView("AddTechSchedule", model);

        }
        [Authorize]
        [HttpPost]
        public JsonResult AddTechSchedule(TechSchedule ts)
        {
            bool result = false;

            //ts.EmployeeId = ((HS.Web.UI.Helper.CustomPrincipal)(User)).EmployeeId.Value;
            ts.CompanyId = ((HS.Web.UI.Helper.CustomPrincipal)(User)).CompanyId.Value;
            if (ts.Id > 0)
            {
                result = _Util.Facade.TechScheduleFacade.UpdateTechSchedule(ts);
            }
            else
            {
                result = _Util.Facade.TechScheduleFacade.InsertTechSchedule(ts) > 0;
            }

            return Json(new { result = result, schedule = ts.IsSchedule, cusId = ts.CustomerId});
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteTechSchedule(int? id)
        {
            if (id.HasValue)
            {
                var scheduletech = _Util.Facade.TechScheduleFacade.DeleteTechSchedule(id.Value);
            }
            return Json(true);
        }

        /*Tech Schedule Setting*/
        
    }
}