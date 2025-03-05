using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HS.API.Grate.Models
{
    public class LabelHelper
    {
        public static class ResponseStatus
        {
            public static string Error { get { return "An error occured. Please contact system admin."; } }
            public static string DataFound { get { return "Data found"; } }
            public static string NoDataFound { get { return "No Data Found"; } }
        }
    }
}