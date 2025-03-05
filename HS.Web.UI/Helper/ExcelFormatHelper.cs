using HS.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HS.Web.UI.Helper
{
    public class ExcelFormatHelper
    {
        public static string Format(string key)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)HttpContext.Current.User;
            return new GlobalSettingsFacade().GetExcelFormat(key,CurrentUser.CompanyId.Value);
        }
    }
}