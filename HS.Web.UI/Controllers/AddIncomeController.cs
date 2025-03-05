using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HS.Web.UI.Controllers
{
    public class AddIncomeController : Controller
    {
        // GET: AddIncome
        public ActionResult Index(int? id)
        {
            if (id.HasValue)
            {
                ViewBag.id = id.Value;
            }
            return View();
        }
        public ActionResult AddIncome()
        {
            return PartialView("_AddIncome");
        }
    }
}