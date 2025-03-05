using HS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HS.Web.UI.Controllers
{
    public class NoteAssignController : BaseController
    {
        // GET: NoteAssign
        public ActionResult Index(int? id)
        {
            if (id.HasValue)
            {
                ViewBag.id = id.Value;
            }
            return View();
        }
        public ActionResult NoteAssignPartial(Guid employeeid)
        {
            //List<NoteAssign> noteassign = _Util.Facade.NoteAssignFacade.GetAllEmployeeNameeByEmployeeId(employeeid);
            return PartialView("_NoteAssignPartial");
        }
    }
}