using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HS.Web.UI.Controllers
{
    public class FundingController : BaseController
    {
        // GET: Funding
        public ActionResult Index(int? id)
        {
            if (id.HasValue)
            {
                ViewBag.id = id.Value;
            }
            return View();
        }
        public ActionResult FundingPartial(Guid? customerid)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<Fund> Funding = _Util.Facade.FundFacade.GetAllFundingByCompanyIdandCustomerId(customerid.Value, currentLoggedIn.CompanyId.Value);
            
            return PartialView("_FundingPartial", Funding);
        }
        [Authorize]
        public ActionResult AddExpense(int? id, int customerid)
        {
            Fund model;
            if (id.HasValue)
            {
                model = _Util.Facade.FundFacade.GetFundById(id.Value);
            }
            else
            {
                model = new Fund();
            }
            ViewBag.CustomerId = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerid).CustomerId.ToString();
            
            ViewBag.PaymentStatus = _Util.Facade.LookupFacade.GetLookupByKey("PaymentStatus").Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();
            ViewBag.PaymentMethod = _Util.Facade.LookupFacade.GetLookupByKey("PaymentMethod").Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();
            return PartialView("_AddExpense", model);
        }

        [Authorize]
        public ActionResult AddIncome(int? id, int customerid)
        {
            Fund model;
            if (id.HasValue)
            {
                model = _Util.Facade.FundFacade.GetFundById(id.Value);
            }
            else
            {
                model = new Fund();
            }
            ViewBag.CustomerId = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerid).CustomerId.ToString();
            ViewBag.PaymentStatus = _Util.Facade.LookupFacade.GetLookupByKey("PaymentStatus").Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();
            ViewBag.PaymentMethod = _Util.Facade.LookupFacade.GetLookupByKey("PaymentMethod").Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();
            return PartialView("_AddIncome", model);
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddIncome(Fund income)
        {
            bool result = false;
            income.CompanyId = ((HS.Web.UI.Helper.CustomPrincipal)User).CompanyId.Value;
            income.Type = FundingType.Income;
            
            if (income.Id > 0)
            {
                Fund tempfund = _Util.Facade.FundFacade.GetById(income.Id);
                if (tempfund.CompanyId != ((HS.Web.UI.Helper.CustomPrincipal)User).CompanyId.Value)
                {
                    return Json(new { result = false, message = "Invalid Parameter" });
                }

                result = _Util.Facade.FundFacade.UpdateFunding(income);
            }
            else
            {
                income.UpdatedBy = User.Identity.Name;
                income.UpdatedDate = DateTime.Now.UTCCurrentTime();

                result = _Util.Facade.FundFacade.InsertFunding(income) > 0;
                
            }
            return Json(new { result = result, message = "Invalid Parameter" });
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddExpense(Fund expense)
        {
            bool result = false;
            expense.CompanyId = ((HS.Web.UI.Helper.CustomPrincipal)User).CompanyId.Value;
            expense.Type = FundingType.Expense;
            
            if (expense.Id > 0)
            {
                Fund tempfund = _Util.Facade.FundFacade.GetById(expense.Id);
                if (tempfund.CompanyId != ((HS.Web.UI.Helper.CustomPrincipal)User).CompanyId.Value)
                {
                    return Json(new { result = false, message = "Invalid Parameter" });
                }

                result = _Util.Facade.FundFacade.UpdateFunding(expense);
            }
            else
            {
                expense.UpdatedBy = User.Identity.Name;
                expense.UpdatedDate = DateTime.Now.UTCCurrentTime();
                result = _Util.Facade.FundFacade.InsertFunding(expense) > 0;

            }
            return Json(new { result = result, message = "Invalid Parameter" });
        }
        [Authorize]
        public PartialViewResult RevenuePartial(Guid? CustomerId)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<Fund> revenue = _Util.Facade.FundFacade.GetRevenueByCustomerId(CustomerId.Value, currentLoggedIn.CompanyId.Value);
            return PartialView("_RevenuePartial", revenue);
        }
        
        [Authorize]
        [HttpPost]
        public JsonResult DeleteAddIncome(int? id)
        {
            if (id.HasValue)
            {
                var sales = _Util.Facade.FundFacade.DeleteAddIncome(id.Value);
            }
            return Json(true);
        }

        [Authorize]
        public ActionResult FundingCompanyPartial()
        {
            List<FundingCompany> ListFundingCompany = _Util.Facade.FundFacade.GetAllFundingCompany();
            return View(ListFundingCompany);
        }

        [Authorize]
        public ActionResult AddFundingCompany(int id)
        {
            FundingCompany model;
            if(id > 0)
            {
                model = _Util.Facade.FundFacade.GetFundingCompanyById(id);
                model.Value = model.Name;
            }
            else
            {
                model = new FundingCompany();
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddFundingCompany(FundingCompany fc)
        {
            bool result = false;
            if(fc.Id > 0)
            {
                fc.Value = fc.Name;
                result = _Util.Facade.FundFacade.UpdateFundingCompany(fc);
            }
            else
            {
                FundingCompany objFundingCompany = new FundingCompany()
                {
                    Name = fc.Name,
                    Value = fc.Name
                };
                result = _Util.Facade.FundFacade.InsertFundingCompany(objFundingCompany) > 0;
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteFundingCompany(int Id)
        {
            bool result = false;
            _Util.Facade.FundFacade.DeleteFundingCompany(Id);
            result = true;
            return Json(result);
        }
    }
}