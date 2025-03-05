using HS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HS.Entities;
using System.Web;
using System.Data;
using HS.Framework;

namespace HS.Facade
{
    public class UserInitializer
    {
        UserLoginDataAccess _UserLoginDataAccess
        {
            get
            {
                return new UserLoginDataAccess();
            }
        }
        StaffDataAccess _StaffDataAccess
        {
            get
            {
                return new StaffDataAccess();
            }
        }
        CompanyDataAccess _CompanyDataAccess
        {
            get
            {
                return new CompanyDataAccess();
            }
        }
        UserCompanyDataAccess _UserCompanyDataAccess
        {
            get
            {
                return new UserCompanyDataAccess();
            }
        }
        UserPermissionDataAccess _UserPermissionDataAccess
        {
            get
            {
                return new UserPermissionDataAccess();
            }
        }
        PermissionGroupDataAccess _PermissionGroupDataAccess
        {
            get
            { 
                return new PermissionGroupDataAccess();
            }
        }
        UserOrganizationDataAccess _UserOrganizationDataAccess
        {
            get
            {
                return new UserOrganizationDataAccess();
            }
        }
        
        EmployeeDataAccess _EmployeeDataAccess
        {
            get
            {
                return new EmployeeDataAccess();
            }
        }
        SeoDataAccess _SeoDataAccess
        {
            get
            {
                return new SeoDataAccess();
            }
        }

        public Seo GetSeoDataByPageUrl(string pagUrl)
        {
            var result = _SeoDataAccess.GetByQuery(" PageUrl like '" + pagUrl + "%'");
            _SeoDataAccess.Dispose();
            return result != null && result.Count > 0 ? result.FirstOrDefault() : null;
        }
        public UserLogin GetCurrentUser(string name, Guid comid)
        {
            if (HttpContext.Current.Session == null)
            {
                return GetUserByUsername(name, new Guid());
            }
            if (HttpContext.Current.Session[SessionKeys.CurrentLoggedInUser] == null)
            {

                if (HttpContext.Current.Session[SessionKeys.CompanyConnectionString] == null)
                {

                    CompanyConneciton CC = GetCompanyConnectionByUsername(name);
                    if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
                    {
                        return null;
                    }
                    string ConnectionString = CC.ConnectionString;
                    if (!string.IsNullOrWhiteSpace(ConnectionString))
                    {
                        ConnectionString = DESEncryptionDecryption.DecryptCipherTextToPlainText(ConnectionString);
                        HttpContext.Current.Session[SessionKeys.CompanyConnectionString] = ConnectionString;
                    }
                }

                UserLogin ul = GetUserByUsername(name, comid);
                HttpContext.Current.Session[SessionKeys.CurrentLoggedInUser] = ul;
                return ul;
            }
            else
            {
                return (UserLogin)HttpContext.Current.Session[SessionKeys.CurrentLoggedInUser];
            }
        }

        private UserLogin GetUserByUsername(string name, Guid comid)
        {
            UserLogin ul = _UserLoginDataAccess.GetUserByUsername(name, comid);
            
            #region Dorkar nai :/ 
            /*
            UserLogin ul = _UserLoginDataAccess.GetByQuery(string.Format(" UserName ='{0}' and IsActive = 1 and IsDeleted =0", name)).FirstOrDefault();
            if (ul != null)
            {
                UserCompany ucom = _UserCompanyDataAccess.GetByQuery(string.Format("  UserId ='{0}' and IsDefault = 1", ul.UserId)).FirstOrDefault();
                if (ucom != null)
                {
                    ul.DefaultCompanyId = ucom.CompanyId;
                    ul.CompanyName = _CompanyDataAccess.GetByQuery(string.Format("CompanyId = '{0}'",ucom.CompanyId)).Select(x=>x.CompanyName).FirstOrDefault();
                }
                UserPermission up = _UserPermissionDataAccess.GetByQuery(string.Format(" UserId ='{0}' and PermissionGroupId is not null", ul.UserId)).FirstOrDefault();
                if (up != null)
                {
                    if (HttpContext.Current.Cache[RMRCacheKey.PermissionGroups] != null)
                    {
                        List<PermissionGroup> PermissionGroupList = (List<PermissionGroup>)HttpContext.Current.Cache[RMRCacheKey.PermissionGroups];
                        ul.UserType = PermissionGroupList.Where(x => x.Id == up.PermissionGroupId).FirstOrDefault().Name;
                        ul.UserTags = PermissionGroupList.Where(x => x.Id == up.PermissionGroupId).Select(x=>x.Tag).FirstOrDefault();
                    }
                    else
                    {
                        List<PermissionGroup> PermissionGroupList = _PermissionGroupDataAccess.GetAll();
                        HttpContext.Current.Cache[RMRCacheKey.PermissionGroups] = PermissionGroupList;
                        ul.UserType = PermissionGroupList.Where(x => x.Id == up.PermissionGroupId).FirstOrDefault().Name;
                        ul.UserTags = PermissionGroupList.Where(x => x.Id == up.PermissionGroupId).Select(x => x.Tag).FirstOrDefault();
                    }
                    //ul.UserType = "staff";
                }
                else
                {
                    ul.UserType = "none";
                }
                Employee tempEmp = _EmployeeDataAccess.GetByQuery(string.Format(" UserId  ='{0}'", ul.UserId)).FirstOrDefault();
                if (tempEmp != null)
                {
                    //ul.EmployeeId = tempEmp.UserId;
                    ul.FirstName = tempEmp.FirstName;
                    ul.LastName = tempEmp.LastName;
                    ul.EmailAddress = tempEmp.Email;
                    ul.PhoneNumber = tempEmp.Phone;
                }
            }*/
            #endregion

            if (ul == null)
            {
                ul = new UserLogin();
                ul.UserType = "none";
            }
            return ul;

        }
        private CompanyConneciton GetCompanyConnectionByUsername(string userName)
        {
            DataTable dt = _UserOrganizationDataAccess.GetCompanyConnectionByUsername(userName);
            List<CompanyConneciton> CompanyConnecitonList = new List<CompanyConneciton>();
            CompanyConnecitonList = (from DataRow dr in dt.Rows
                                     select new CompanyConneciton()
                                     {
                                         CompanyId = (Guid)dr["CompanyId"],
                                         ConnectionString = dr["ConnectionString"].ToString(),
                                     }).ToList();
            return CompanyConnecitonList.FirstOrDefault();
        }

    }
}
