using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HS.Web.UI.Helper
{
    public class PermissionHelper
    {
        public static bool IsPermitted(int pid)
        {
            if (HttpContext.Current != null && HttpContext.Current.User != null)
            {

                //string username = HttpContext.Current.Session[SessionKeys.UserName].ToString();
                //if (string.IsNullOrWhiteSpace(username))
                //{
                //    return false;
                //}
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)HttpContext.Current.User;
                Facade.PermissionFacade permission = new Facade.PermissionFacade();
                //bool result = permission.CheckCurrentLogInUserHasPermissionByUsernameAndPermissionId(username, pid);
                return permission.IsPermitted(pid, CurrentUser.UserId, CurrentUser.CompanyId.Value);
            }
            return false;
        }

        public static bool IsPermitted(int pid, Guid UserId)
        {
            Facade.PermissionFacade permission = new Facade.PermissionFacade();
            bool result = permission.CheckCurrentLogInUserHasPermissionByUserIdAndPermissionId(UserId, pid);
            return !result;
        }

    }
}