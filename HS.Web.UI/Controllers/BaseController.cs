using HS.Entities;
using HS.Framework;
using HS.Framework.Utils;
using HS.Web.UI.App_Start;
using HS.Web.UI.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using System.Web.Caching;

namespace HS.Web.UI.Controllers
{
    public class BaseController : Controller
    {
        #region Util
        private WebUtil __Util;

        protected WebUtil _Util
        {
            get
            {
                if (null == __Util)
                    __Util = new WebUtil();
                return __Util;
            }
        }
        private static WebUtil __staticUtil;
        protected static WebUtil _staticUtill
        {
            get
            {
                if (null == __staticUtil)
                    __staticUtil = new WebUtil();
                return __staticUtil;
            }
        }


        private static string _S3Domain;
        protected static string S3Domain
        {
            get
            {
                if (null == _S3Domain)
                    _S3Domain = String.Format(AppConfig.AWSS3Url, AppConfig.AWSS3BucketName);
                return _S3Domain;
            }
        }

        #endregion

        #region Digiture

        protected Logger logger { get; set; }

        public BaseController()
        {
            if (Session!=null)
                Session["Version"] = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey("ATDeployedVersion");
        }

        #endregion Digiture

        public bool IsPermitted(int Id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            return _Util.Facade.PermissionFacade.IsPermitted(Id, CurrentUser.UserId, CurrentUser.CompanyId.Value);
        }
        public void AddPageLoadUserActivity(string actiontextdisplay = "Success")
        {
            UserActivity useractivity = new UserActivity();

            useractivity.PageUrl = Request.Url.AbsoluteUri;
            useractivity.ReferrerUrl = Request.UrlReferrer != null ? Request.UrlReferrer.AbsoluteUri : "";
            if (User.Identity.IsAuthenticated)
                useractivity.UserName = User.Identity.Name;
            useractivity.Action = LabelHelper.ActivityAction.PageLoad;
            useractivity.ActionDisplyText = actiontextdisplay;

            _Util.Facade.UserActivityFacade.AddUserActivity(useractivity);

        }
        [Authorize]
        public void AddUserActivityForCustomer(string actiontextdisplay ,string action,Guid? CustomerGID, int? CustomerID,string RefId, bool IsARB = false)
        {
            UserActivity useractivity = new UserActivity();
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
          
            useractivity.ActivityId = Guid.NewGuid();
            useractivity.PageUrl = Request.Url.AbsoluteUri != null ? Request.Url.AbsoluteUri : "";
            useractivity.ReferrerUrl = Request.UrlReferrer != null ? Request.UrlReferrer.AbsoluteUri : "";
            // new paramiter
            useractivity.Action = action != null ? action : "";
            

            useractivity.UserAgent = AppConfig.GetUserAgent != null ? AppConfig.GetUserAgent : "";
            useractivity.UserIp = AppConfig.GetIP != null ? AppConfig.GetIP : "";
            if (CurrentUser.FirstName != null || CurrentUser.LastName != null)
            {
                useractivity.UserName = CurrentUser.FirstName + CurrentUser.LastName;
            }
            else
            {
                useractivity.UserName = "";
            }
            useractivity.UserId = CurrentUser.UserId != null ? CurrentUser.UserId : Guid.NewGuid();
            useractivity.StatsDate = DateTime.UtcNow;
            // end new paramiter
            if (User.Identity.IsAuthenticated)
                useractivity.UserName = User.Identity.Name;
            //useractivity.Action = LabelHelper.ActivityAction.PageSubmit;
            useractivity.ActionDisplyText = actiontextdisplay;
            useractivity.IsARB = IsARB;
            _Util.Facade.UserActivityFacade.AddUserActivity(useractivity);
            UserActivityCustomer useractivityCustomer = new UserActivityCustomer();
            if(CustomerGID != null)
            {
                useractivityCustomer.CustomerId =CustomerGID.Value;
            }
            else if(CustomerID != null)
            {
                useractivityCustomer.CustomerId =_Util.Facade.CustomerFacade.GetCustomerById(CustomerID.Value).CustomerId;
            }
            else
            {
                useractivityCustomer.CustomerId = new Guid();
            }
            if(RefId!= null)
            {
                useractivityCustomer.RefId = RefId;

            }
            useractivityCustomer.ActivityId = useractivity.ActivityId;
            _Util.Facade.UserActivityCustomerFacade.AddUserActivityCustomer(useractivityCustomer);

        }
        [Authorize]
        public bool SetLayoutCommons()
        {
            Guid comid = new Guid();
            List<UserOrganization> UserOrgList = null;
            if (Session[SessionKeys.CurrentUserCompanyList] == null)
            {
                UserOrgList = _Util.Facade.UserOrganizationFacade.GetUsersOrganizationListByUsername(User.Identity.Name);
                Session[SessionKeys.CurrentUserCompanyList] = UserOrgList;
            }
            else
            {
                UserOrgList = (List<UserOrganization>)Session[SessionKeys.CurrentUserCompanyList];
            }

            if (UserOrgList.Count() > 0)
            {
                UserOrganization Tempcom = UserOrgList//.OrderByDescending(x => x.Id)
                    .Where(x => x.IsActive == true).FirstOrDefault();
                if (Tempcom == null)
                {
                    Tempcom = UserOrgList.FirstOrDefault();
                }

                ViewBag.DefaultCompanyIdLayout = Tempcom.CompanyId;
                Session[SessionKeys.CompanyId] = Tempcom.CompanyId;
                ViewBag.UserCompanyListLayout = UserOrgList.OrderBy(ss=>ss.CompanyName).Select(x =>
                new SelectListItem()
                {
                    Text = x.CompanyName.ToString(),
                    Value = x.CompanyId.ToString()
                }).ToList();
                comid = Tempcom.CompanyId;
            }

            #region Comment
            /* List<UserCompany> userCompanyList = null;
             if (Session["CurrentUserCompanyList"] == null)
             {
                 userCompanyList = _Util.Facade.UserCompanyFacade.GetUsersCompanyListByUsername(User.Identity.Name);
                 Session["CurrentUserCompanyList"] = userCompanyList;
             }
             else
             {
                 userCompanyList = (List<UserCompany>)Session["CurrentUserCompanyList"];
             }*/

            /*if (userCompanyList.Count() > 0)
            {
                UserCompany Tempcom = userCompanyList.Where(x => x.IsDefault == true).FirstOrDefault();
                if (Tempcom == null)
                {
                    Tempcom = userCompanyList.FirstOrDefault();
                }

                ViewBag.DefaultCompanyIdLayout = Tempcom.CompanyId;

                ViewBag.UserCompanyListLayout = userCompanyList.Select(x =>
                new SelectListItem()
                {
                    Text = x.CompanyName.ToString(),
                    Value = x.CompanyId.ToString()
                }).ToList();
            }*/
            #endregion

            //ViewBag.AdvancedSearchOptionList = _Util.Facade.LookupFacade.GetLookupByKey("AdvancedSearchOption").Select(x => 
            //new SelectListItem()
            //{
            //    Text = x.DisplayText.ToString(),
            //    Value = x.DataValue.ToString()
            //}).ToList();
            Session[SessionKeys.CurrentLoggedInUser] = null;
            Session[SessionKeys.CompanyConnectionString] = null;
            UserLogin AppUser = new Facade.UserInitializer().GetCurrentUser(User.Identity.Name, comid);
            CustomPrincipal UserPrincipal = new CustomPrincipal(AppUser, User.Identity);
            HttpContext.User = UserPrincipal;
            var _CurrentUser = UserPrincipal;

            if(_CurrentUser.UserRole == "none")
            {
                return false;
            }
            string CacheKey = RMRCacheKey.CompanyBlacknWhiteLogo + _CurrentUser.CompanyId.Value.ToString();

            HttpRuntime.Cache.Remove(CacheKey);
            if (HttpRuntime.Cache[CacheKey] == null)
            {
                CompanyBranch cb = _Util.Facade.CompanyBranchFacade.GetMainBranchByCompanyId(ViewBag.DefaultCompanyIdLayout);
                if (cb != null && !string.IsNullOrWhiteSpace(cb.Logo))
                {
                    ViewBag.UserCompanyLogo = cb.Logo;
                    ViewBag.CompanyLogo = cb.Logo;
                    HttpRuntime.Cache[CacheKey] = cb.Logo;
                }
                else
                {

                    HttpRuntime.Cache[CacheKey] = ConfigurationManager.AppSettings["Logo.DefaultWhiteLogo"];
                    ViewBag.UserCompanyLogo = ConfigurationManager.AppSettings["Logo.DefaultWhiteLogo"];
                    ViewBag.CompanyLogo = ConfigurationManager.AppSettings["Logo.DefaultWhiteLogo"];
                }
            }
            else
            {
                ViewBag.UserCompanyLogo = HttpRuntime.Cache[CacheKey].ToString();
            } 
            //Session[SessionKeys.UserName] = User.Identity.Name;
            
            //This UserRole will be used in facade
            Session[SessionKeys.UserRole] = ((HS.Web.UI.Helper.CustomPrincipal)User).UserRole;
            GlobalSetting gs = _Util.Facade.GlobalSettingsFacade.GetAreaZipcodeMenu(_CurrentUser.CompanyId.Value);
            ViewBag.AreaZipcode = "";
            if (gs != null)
            {
                ViewBag.AreaZipcode = gs.Value;
            }
            int NotificationCount = 0;
            var NotificationDetails = _Util.Facade.NotificationFacade.GetNotificationCountByUserId(((HS.Web.UI.Helper.CustomPrincipal)User).UserId);
            if (NotificationDetails != null)
            {
                NotificationCount = NotificationDetails.NotificationCount;
            }
            ViewBag.NotificationCount = NotificationCount;
            ViewBag.IsClockedIn = _CurrentUser.IsClockedIn;
            if (_CurrentUser.IsClockedIn && _CurrentUser.ClockInOutTime.HasValue && _CurrentUser.ClockInOutTime != new DateTime())
                ViewBag.LastClockedInTime = _CurrentUser.ClockInOutTime.Value.UTCToClientTime().ToString("hh:mm tt");
            var objticketuser = _Util.Facade.TicketFacade.GetAllAssignedTicketCountByUserId(_CurrentUser.UserId);
            if(objticketuser != null)
            {
                ViewBag.MyTicketCount = objticketuser.MyTicketCount;
            }
            return true;
        }


        #region Cache

        public List<SelectListItem> TermListCache
        {
            get 
            {
                List<SelectListItem> TermList = (List<SelectListItem>)HttpContext.Cache[RMRCacheKey.TermList] ?? 
                    _Util.Facade.LookupFacade.GetLookupByKey("InvoiceTerms").Select(x =>
                     new SelectListItem()
                     {
                         Text = x.DisplayText.ToString(),
                         Value = x.DataValue.ToString()
                     }).ToList();
                //HttpContext.Cache[RMRCacheKey.TermList] = TermList;
                HttpContext.Cache.Insert(RMRCacheKey.TermList,TermList,null,Cache.NoAbsoluteExpiration,TimeSpan.FromHours(2));
                return TermList;
            }
        }


        #endregion Cache

    }
}