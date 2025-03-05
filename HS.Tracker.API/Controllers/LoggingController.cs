using HS.Entities;
using HS.Tracker.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HS.Tracker.API.Controllers
{
    public class LoggingController : ApiController
    {
        public string Get()
        {
            return "Welcome To Web API";
        }

        [HttpPost]
        public void Error(ErrorLog errLog)
        {
            try
            {
                if (errLog.TimeUtc.Equals(DateTime.MinValue))
                {
                    errLog.TimeUtc = DateTime.Now;
                }
                
                new Logging().AddLogging(errLog);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
