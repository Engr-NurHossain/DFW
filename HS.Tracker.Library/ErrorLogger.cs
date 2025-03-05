using HS.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace HS.Tracker.Library
{
    public class ErrorLogger
    {
        public static void SetErrorLog(ErrorLog errLog)
        {
            try
            {
                string apiUrl = "http://localhost:55785/api/Logging/Error";

                string inputJson = (new JavaScriptSerializer()).Serialize(errLog);
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string json = client.UploadString(apiUrl, inputJson);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
