using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HS.Framework;
using HS.Entities;
using Localize = HS.Web.UI.Helper.LanguageHelper;
using System.Web.Mvc;

namespace HS.Web.UI.Controllers
{
    public class NotificationController : BaseController
    {
        // GET: Notification
        public ActionResult Index()
        {
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            return View();
        }

        public ActionResult AllNotifications()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            NotificationViewModel Model = _Util.Facade.NotificationFacade.GetUserNotificationsByUserId(CurrentUser.UserId,-1);
            if (Model.Notifications == null)
            {
                Model.Notifications = new List<Notification>();
            }
            else
            {
                foreach (var item in Model.Notifications)
                {
                   
                    item.LastVisited = string.Format(HS.Framework.Utils.ConvertDatetimeToAgo.TimeAgo(item.CreatedDate).ToString("MM/dd/yyyy hh:mm tt"));
                }
            }

            return View("_AllNotifications", Model);
        }
        //public ActionResult GetUserNotifications()
        //{
        //    var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

        //    NotificationViewModel Model = _Util.Facade.NotificationFacade.GetUserNotificationsByUserId(CurrentUser.UserId);
        //    if(Model.Notifications == null)
        //    {
        //        Model.Notifications = new List<Notification>();
        //    }
        //    else
        //    {
        //        foreach(var item in Model.Notifications)
        //        {

        //            item.LastVisited = string.Format(HS.Framework.Utils.ConvertDatetimeToAgo.TimeAgo(item.CreatedDate).ToString("MM/dd/yy hh:mm tt"));
        //        }
        //    }
        //    return View("_GetUserNotifications", Model);
        //}
        public ActionResult GetUserNotifications()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            NotificationViewModel Model = _Util.Facade.NotificationFacade.GetUserNotificationsByUserId(CurrentUser.UserId);
            if (Model.Notifications == null)
            {
                Model.Notifications = new List<Notification>();
            }
            else
            {
                foreach (var item in Model.Notifications)
                {

                    item.LastVisited = string.Format(HS.Framework.Utils.ConvertDatetimeToAgo.TimeAgo(item.CreatedDate).ToString("MM/dd/yy hh:mm tt"));
                }
            }
            int due = 0;
            GlobalSetting dueday = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey("AssignedArticlesDefaultDueDate");
            if (dueday != null)
            {

                int.TryParse(dueday.Value, out due);
            }
            Model.DueDay = due;
            return View("_GetUserNotifications", Model);
        }
        public JsonResult MarkAllNotificationAsRead()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            _Util.Facade.NotificationFacade.MarkAllAsRead(CurrentUser.UserId);
            return Json(new { result = true, message = "Done" });
        }

        [Authorize]
        public ActionResult GetAllAnnouncementByCurrentDate()
        {
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentLoggedInUser == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            AnnouncementExtend announcementExtend = new AnnouncementExtend();
            List<Announcement> AnnouncementList = new List<Announcement>();
            AnnouncementList = _Util.Facade.CustomerFacade.GetAllAnnouncementByCurrentDate();
            announcementExtend.AnnouncementList = AnnouncementList;
            announcementExtend.AnnouncementListCount = AnnouncementList.Count;
            return Json(announcementExtend, JsonRequestBehavior.AllowGet);
        }

    }
}