using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace HS.Web.UI.Controllers
{
    public class BillingController : BaseController
    {
        // GET: Billing
        public ActionResult Index(int? id)
        {
            if (id.HasValue)
            {
                ViewBag.id = id.Value;
            }
            return View();
        }
        public ActionResult BillingPartial(Guid? customerid)
        {
            var CurrentLogIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            
            Customer tempcus = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerid.Value);
            List<CustomerBill> customerBill = _Util.Facade.CustomerBillFacade.GetAllCustomerBillByCustomerIdAndCompanyId(tempcus.CustomerId, CurrentLogIn.CompanyId.Value);
            return PartialView("_BillingPartial", customerBill);
        }
        [Authorize]
        public ActionResult AddBilling(int? id, int customerid)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            CustomerBill cusbill;
            if (id.HasValue)
            {
                cusbill = _Util.Facade.CustomerBillFacade.GetById(id.Value);
                //if(cusbill == null)
                //{
                //    cusbill = new CustomerBill();
                //    cusbill.PaymentMethod = "-1";
                //    cusbill.BillCycle = "-1";
                //}
            }
            else
            {
                cusbill = new CustomerBill();
                cusbill.BillCycle = "-1";
                cusbill.PaymentMethod = "-1";
            }
            ViewBag.CustomerId = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(customerid).CustomerId.ToString();
            ViewBag.PaymentMethod = _Util.Facade.LookupFacade.GetLookupByKey("PaymentMethod").Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();
            ViewBag.BillCycle = _Util.Facade.LookupFacade.GetLookupByKey("BillCycle").Select(x =>
                            new SelectListItem()
                            {
                                Text = x.DisplayText.ToString(),
                                Value = x.DataValue.ToString()
                            }).ToList();
            return PartialView("AddBilling", cusbill);
        }
        [HttpPost]
        [Authorize]
        public JsonResult AddBilling(CustomerBill cb)
        {
            bool result = false;
            var currentlogin = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            cb.UpdatedBy = User.Identity.Name;
            cb.UpdatedDate = DateTime.Now.UTCCurrentTime();
            cb.CompanyId = ((HS.Web.UI.Helper.CustomPrincipal)(User)).CompanyId.Value;
            
            if (cb.Id > 0)
            {
                CustomerBill cbill = _Util.Facade.CustomerBillFacade.GetById(cb.Id);
                cb.BillNo = "cus-01";
                cb.Type = CustomerBillType.BillType;
                result = _Util.Facade.CustomerBillFacade.UpdateCustomerBill(cb);
            }
            else
            {
                cb.BillNo = "cus-01";
                cb.Type = CustomerBillType.BillType;
                result = _Util.Facade.CustomerBillFacade.InsertCustomerBill(cb) > 0;
            }
            return Json(result);
        }
    }
}