using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HS.Facade;
using HS.Entities;
using HS.Framework;
using HS.Framework.Utils;
using HS.Web.UI.Controllers;
using HS.Web.UI.Helper;

namespace HS.API.Controllers
{
    public class BadgerController : BaseAPIController
    {
        public HSApiFacade HSapiFacade = new HSApiFacade();
        public HSMainApiFacade HSMainApiFacade = new HSMainApiFacade();
        public static string ConnStr = "";
        string constrHudson = System.Configuration.ConfigurationManager.ConnectionStrings["hsCoonectionStringHudson"].ConnectionString;
        private object tempCustomer;

        private void APIInitializeHudson()
        {
            if (!string.IsNullOrWhiteSpace(constrHudson))
            {
                HSapiFacade = new HSApiFacade(constrHudson);
            }
        }
        [Route("SaveActivity")]
        public HttpResponseMessage SaveActivity()
        {
            APIInitializeHudson();
            string ActivityType = "";
            string Note = "";
            string Customer = "";
            string BadgerUserId = "";
            var re = Request;
            var headers = re.Headers;
            bool result = false;
            if (headers.Contains("ActivityType"))
            {
                ActivityType = headers.GetValues("ActivityType").First();
            }
            if (headers.Contains("Note"))
            {
                Note = headers.GetValues("Note").First();
            }
            if (headers.Contains("Customer"))
            {
                Customer = headers.GetValues("Customer").First();
            }
            if (headers.Contains("BadgerUserId"))
            {
                BadgerUserId = headers.GetValues("BadgerUserId").First();
            }
            try
            {
                if (!string.IsNullOrEmpty(BadgerUserId))
                {
                    var EmployeeDetails = HSapiFacade.GetEmployeeByBadgerUserId(BadgerUserId);

                    Activity activity = new Activity();
                    activity.ActivityId = Guid.NewGuid();
                    activity.ActivityType = ActivityType;
                    activity.Description = "";
                    activity.AssignedTo = EmployeeDetails != null ? EmployeeDetails.UserId : Guid.Empty;
                    activity.Status = "Completed";
                    activity.AssociatedType = "Account";
                    activity.AssociatedWith = Guid.NewGuid();
                    activity.Note = Note;
                    activity.CreatedBy = Guid.NewGuid();
                    activity.CreatedDate = DateTime.Now.UTCCurrentTime();
                    activity.DueDate = DateTime.Now.UTCCurrentTime();
                    activity.Origin = "Badger Map";
                    result = HSapiFacade.InsertActivity(activity);
                    return Request.CreateResponse(HttpStatusCode.OK, new { result });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Badger id field is required.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.ExpectationFailed, ex.Message);
            }
        }
    }
}
