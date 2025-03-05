using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HS.Web.UI.Controllers
{
    public class AgreementController : BaseController
    {
        // GET: Agreement
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AgreementQuestionAnswerPDF() 
        {
            return View();
        }
    }
}