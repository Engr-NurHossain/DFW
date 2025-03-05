using System;
using System.Collections.Generic;
using Plivo;
using Plivo.Exception;
using Plivo.Resource.Message;
using HS.Framework.Utils;
using HS.Framework;
using System.Web;
using System.Net;

namespace HS.SMS
{
    public class SMSManager
    {
        public static bool SendASms(List<string> to, string From, string message, string AuthId, string AuthToken)
        {
            bool result =false;
            try {
                List<string> to2 = new List<string>();
                foreach (var item in to)
                {
                    string too = item.Replace("-","").Replace(")","").Replace("(", "").Replace(" ", "");
                    if (item.IndexOf("+") == 0)
                    {
                        to2.Add(too);
                    }
                    else
                    {
                       too=  string.Concat("+1", too);
                        to2.Add(too);
                    }
                }
                if (HttpContext.Current == null || HttpContext.Current.Request == null || !HttpContext.Current.Request.IsLocal)
                {
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    var api = new PlivoApi(AuthId, AuthToken);
                    MessageCreateResponse response = api.Message.Create(
                        dst: to2, 
                        
                        src: From, 
                        text: message
                        );
                    //Console.WriteLine(response);
                }
                result = true;
            }
            catch (Exception e)
            {
                if (AppConfig.Production.ToLower() == "true")
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~/SMSError.txt"), true))
                    {
                        file.WriteLine(e.Message);
                        file.WriteLine(e.InnerException);
                        file.WriteLine(e.StackTrace);
                        file.Close();
                    }
                    //System.IO.File.WriteLine(HttpContext.Current.Server.MapPath("~/SMSError.txt"), e.Message);
                }
                result = false;
            }
            return result;
        }
    }
}
