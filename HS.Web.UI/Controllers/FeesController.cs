using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HS.Web.UI.Controllers
{
    public class FeesController : BaseController
    {
        // GET: Fees
        public ActionResult Index()
        {
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            return View();
        }
        public ActionResult FeesPartial()
        {
            return PartialView("FeesPartial");
        }
    }
}