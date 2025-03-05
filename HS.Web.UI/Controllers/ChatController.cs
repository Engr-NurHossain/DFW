using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HS.Web.UI.Controllers
{
    public class ChatController : BaseController
    {
        // GET: Chat
        public ActionResult Index()
        {
            return View();
        }
    }
}