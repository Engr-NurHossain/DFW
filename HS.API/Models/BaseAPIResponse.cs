using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HS.API.Models
{
    public class BaseAPIResponse
    {
        public BaseAPIResponse(bool result,string message, dynamic data = null)
        {
            this.result = result;
            this.message = message;
            this.data = data;
        }
        public bool result { set; get; }
        public string message { set; get; }
        public dynamic data { set; get; }
    }
    public class ThirdPartyAPIResponse
    {
        public ThirdPartyAPIResponse(bool success, string error, dynamic result = null)
        {
            this.success = success;
            this.error = error;
            this.result = result;
        }
        public bool success { set; get; }
        public string error { set; get; }
        public dynamic result { set; get; }
    }
}