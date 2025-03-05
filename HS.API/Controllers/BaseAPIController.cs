using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HS.API.Models;
using System.Security.Claims;
using HS.Entities;
using HS.Facade;
using System.Security.Cryptography;

namespace HS.API.Controllers
{
    public class BaseAPIController : ApiController
    {
        public HSApiFacade HSapiFacade = new HSApiFacade();
        public HSMainApiFacade HSMainApiFacade = new HSMainApiFacade();
        public static string ConnStr = "";
        public static string ComId = "";
        public static string UserNAME = "";

        public void APIInitialize()
        {
            //var identity = (ClaimsIdentity)User.Identity;
            //string connectionstring = identity.Claims.Where(c => c.Type == "connectionstring").Select(c => c.Value).SingleOrDefault();
            //CompanyConneciton CC = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
            //HSapiFacade = new HSApiFacade(CC.ConnectionString);

            if (!string.IsNullOrWhiteSpace(ConnStr))
            {
                if (!string.IsNullOrWhiteSpace(ComId))
                {
                    CompanyConneciton CC = new CompanyConneciton();
                    var identity = (ClaimsIdentity)User.Identity;
                    if (!string.IsNullOrWhiteSpace(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault()))
                    {
                        CC = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault(), new Guid(ComId));
                    }
                    else
                    {
                        CC = HSMainApiFacade.GetCompanyConnectionByUsernameAndCompanyId(UserNAME, new Guid(ComId));
                    }
                    ComId = CC.CompanyId.ToString();
                    HSapiFacade = new HSApiFacade(CC.ConnectionString);
                }
                else
                {
                    var identity = (ClaimsIdentity)User.Identity;
                    CompanyConneciton CC = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                    ComId = CC.CompanyId.ToString();
                    HSapiFacade = new HSApiFacade(CC.ConnectionString);
                }
            }
            else
            {
                var identity = (ClaimsIdentity)User.Identity;
                string connectionstring = identity.Claims.Where(c => c.Type == "connectionstring").Select(c => c.Value).SingleOrDefault();
                CompanyConneciton CC = HSMainApiFacade.GetCompanyConnectionByUserName(identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault());
                ComId = CC.CompanyId.ToString();
                HSapiFacade = new HSApiFacade(CC.ConnectionString);
            }
        }

        public string GetClientIp(HttpRequestMessage request = null)
        {
            if (request == null)
            {
                return null;
            }

            if (request.Properties.ContainsKey("MS_OwinContext"))
            {
                return ((OwinContext)request.Properties["MS_OwinContext"]).Request.RemoteIpAddress;
            }
            return null;
        }

        public CustomPrincipal GetUserInfo()
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                string Username = identity.Claims.Where(c => c.Type == "username").Select(c => c.Value).SingleOrDefault();
                Guid Userid = Guid.Empty;
                string strUserId = identity.Claims.Where(c => c.Type == "userid").Select(c => c.Value).SingleOrDefault();
                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(strUserId) || !Guid.TryParse(strUserId, out Userid))
                {
                    return null;
                }

                CustomPrincipal customPrincipal = new CustomPrincipal()
                {
                    Userid = Userid,
                    Username = Username
                };

                return customPrincipal;
            }
            catch(Exception e)
            {
                return null;
            }
            

        }
        public void AddUserActivityForCustomer(string actiontextdisplay, string AbsoluteUri, string action, string Agent, string Ip, string UserName, string UserId, string CustomerId, string LogType, string RefId)
        {

            Guid uid = new Guid();
            Guid cid = new Guid();
            UserActivity useractivity = new UserActivity();
            useractivity.ActivityId = Guid.NewGuid();
            useractivity.PageUrl = AbsoluteUri != null ? AbsoluteUri : "";
            useractivity.ReferrerUrl = AbsoluteUri != null ? AbsoluteUri : "";
            useractivity.Action = action != null ? action : "";
            useractivity.UserAgent = Agent != null ? Agent : "";
            useractivity.UserIp = Ip != null ? Ip : "";
            useractivity.UserName = UserName;
            useractivity.UserId = Guid.TryParse(UserId, out uid) ? uid : Guid.Parse("22222222-2222-2222-2222-222222222222");
            useractivity.StatsDate = DateTime.UtcNow;
            useractivity.ActionDisplyText = actiontextdisplay != null ? actiontextdisplay : "";
          //  useractivity.RefId = RefId != null ? RefId : "";
           // useractivity.LogFor = LogType != null ? LogType : "";
            useractivity.IsARB = false;

            if (string.IsNullOrWhiteSpace(HSapiFacade.ConnectionString))
            {
                APIInitialize();
            }

            HSapiFacade.AddUserActivity(useractivity);
            UserActivityCustomer useractivityCustomer = new UserActivityCustomer();
            if (Guid.TryParse(CustomerId, out cid))
            {
                useractivityCustomer.CustomerId = cid;
            }
            else
            {
                useractivityCustomer.CustomerId = Guid.Empty;
            }
            useractivityCustomer.ActivityId = useractivity.ActivityId;
            HSapiFacade.AddUserActivityCustomer(useractivityCustomer);

        }

    }
}
