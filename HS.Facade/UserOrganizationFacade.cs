using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Facade
{
    public class UserOrganizationFacade : BaseFacade
    {
        UserOrganizationDataAccess _UserOrganizationDataAccessMaster = null;
        OrganizationDataAccess _OrganizationDataAccess = null;

        public UserOrganizationFacade(ClientContext clientContext)
            : base(clientContext)
        {
            if (_UserOrganizationDataAccessMaster == null)
                _UserOrganizationDataAccessMaster = new UserOrganizationDataAccess(clientContext, System.Configuration.ConfigurationManager.ConnectionStrings["hsCoonectionString"].ConnectionString);
            if (_OrganizationDataAccess == null)
                _OrganizationDataAccess = new OrganizationDataAccess(clientContext, System.Configuration.ConfigurationManager.ConnectionStrings["hsCoonectionString"].ConnectionString);
        }
        public UserOrganizationFacade() {
            if (_UserOrganizationDataAccessMaster == null)
                _UserOrganizationDataAccessMaster = new UserOrganizationDataAccess(System.Configuration.ConfigurationManager.ConnectionStrings["hsCoonectionString"].ConnectionString);
            if (_OrganizationDataAccess == null)
                _OrganizationDataAccess = new OrganizationDataAccess( System.Configuration.ConfigurationManager.ConnectionStrings["hsCoonectionString"].ConnectionString);
        }

        UserOrganizationDataAccess _UserOrganizationDataAccess
        {
            get
            {
                return (UserOrganizationDataAccess)_ClientContext[typeof(UserOrganizationDataAccess)];
            }
        }
        
        public string GetConnectionStringByCompanyId(Guid companyId)
        {
            return _OrganizationDataAccess.GetByQuery(string.Format("CompanyId='{0}'", companyId)).FirstOrDefault().ConnectionString;
        }
        public CompanyConneciton GetCompanyConnectionByCompanyId(Guid companyid)
        {
            return GetCompanyConnectionByCompanyId(companyid.ToString());
        }
        public CompanyConneciton GetCompanyConnectionByCompanyId(string companyid)
        {
            DataTable dt = _UserOrganizationDataAccessMaster.GetCompanyConnectionByCompanyId(companyid);
            List<CompanyConneciton> CompanyConnecitonList = new List<CompanyConneciton>();
            CompanyConnecitonList = (from DataRow dr in dt.Rows
                                     select new CompanyConneciton()
                                     {
                                         CompanyId = (Guid)dr["CompanyId"],
                                         ConnectionString = dr["ConnectionString"].ToString(),
                                         UserName = dr["UserName"].ToString()
                                     }).ToList();
            return CompanyConnecitonList.FirstOrDefault();
        }

        public CompanyConneciton GetIeateryCompanyConnectionByCompanyId(Guid companyid)
        {
            DataTable dt = _UserOrganizationDataAccessMaster.GetIeateryCompanyConnectionByCompanyId(companyid);
            List<CompanyConneciton> CompanyConnecitonList = new List<CompanyConneciton>();
            CompanyConnecitonList = (from DataRow dr in dt.Rows
                                     select new CompanyConneciton()
                                     {
                                         CompanyId = (Guid)dr["CompanyId"],
                                         ConnectionString = dr["ConnectionString"].ToString(),
                                         UserName = dr["UserName"].ToString()
                                     }).ToList();
            return CompanyConnecitonList.FirstOrDefault();
        }

        public CompanyConneciton GetCompanyConnectionByUsername(string userName)
        {
            DataTable dt = _UserOrganizationDataAccessMaster.GetCompanyConnectionByUsername(userName);
            List<CompanyConneciton> CompanyConnecitonList = new List<CompanyConneciton>();
            CompanyConnecitonList = (from DataRow dr in dt.Rows
                           select new CompanyConneciton()
                           {
                               CompanyId = (Guid)dr["CompanyId"],
                               ConnectionString = dr["ConnectionString"].ToString(),
                               MasterPassword = dr["MasterPassword"].ToString(),
                               CompanyName = dr["CompanyName"].ToString()
                           }).ToList();
            return CompanyConnecitonList.FirstOrDefault();
        }

        public CompanyConneciton GetCompanyConnectionByUsernameAndCompanyId(string userName, Guid comid)
        {
            DataTable dt = _UserOrganizationDataAccessMaster.GetCompanyConnectionByUsernameAndCompanyId(userName, comid);
            List<CompanyConneciton> CompanyConnecitonList = new List<CompanyConneciton>();
            CompanyConnecitonList = (from DataRow dr in dt.Rows
                                     select new CompanyConneciton()
                                     {
                                         CompanyId = (Guid)dr["CompanyId"],
                                         ConnectionString = dr["ConnectionString"].ToString(),
                                         MasterPassword = dr["MasterPassword"].ToString()
                                     }).ToList();
            return CompanyConnecitonList.FirstOrDefault();
        }

        public List<UserOrganization> GetUsersOrganizationListByUsername(string UserName)
        {
            DataTable dt = _UserOrganizationDataAccessMaster.GetUsersOrganizationListByUsername(UserName);
            List<UserOrganization> userCompanyList = new List<UserOrganization>();
            userCompanyList = (from DataRow dr in dt.Rows
                               select new UserOrganization()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   CompanyName = dr["CompanyName"].ToString(),
                                   UserName = dr["UserName"].ToString(),
                                   CompanyId = (Guid)dr["CompanyId"],
                                   IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false
                               }).ToList();
            return userCompanyList;
        }

        public UserOrganization GetUserOrganizationByUserName(string username)
        {
            return _UserOrganizationDataAccessMaster.GetByQuery(string.Format("UserName = '{0}'", username)).FirstOrDefault();
        }

        public List<UserOrganization> GetUserOrganizationListByUserName(string username)
        {
            return _UserOrganizationDataAccessMaster.GetByQuery(string.Format("UserName = '{0}'", username)).ToList();
        }

        public bool UserIsInCompany(string Username, Guid companyId)
        {
            return _UserOrganizationDataAccessMaster.GetByQuery(string.Format(" Username ='{0}'and CompanyId = '{1}' ", Username, companyId)).Count() > 0;
        }

        public List<UserOrganization> GetUserOrganizationListByUserNameAndCompanyId(string Username, Guid companyId)
        {
            return _UserOrganizationDataAccessMaster.GetByQuery(string.Format(" Username ='{0}'and CompanyId = '{1}' ", Username, companyId)).ToList();
        }

        public bool SetDefaultUserCompany(string name, Guid companyId)
        {
            return _UserOrganizationDataAccessMaster.SetDefaultUserCompany(name, companyId);
        }

        public int InsertUserOrganization(UserOrganization uo)
        {
            UserOrganization TemUo = _UserOrganizationDataAccessMaster.GetByQuery(string.Format("UserName = '{0}' and CompanyId ='{1}'", uo.UserName, uo.CompanyId)).FirstOrDefault();
            if (TemUo != null)
            {
                return TemUo.Id;
            }

            if (uo.IsActive)
            {
                List<UserOrganization> TemUoList = _UserOrganizationDataAccessMaster.GetByQuery(string.Format(" UserName = '{0}'", uo.UserName));
                if (TemUo != null && TemUoList.Count > 0)
                {
                    foreach (var item in TemUoList)
                    {
                        item.IsActive = false;
                        _UserOrganizationDataAccessMaster.Update(item);
                    }
                }
            }
            return (int)_UserOrganizationDataAccessMaster.Insert(uo); 
        }

        public int InsertUserOrganizationObj(UserOrganization uo)
        {
            return (int)_UserOrganizationDataAccessMaster.Insert(uo);
        }

        public List<Organization> GetAllOrganizations()
        {
            return _OrganizationDataAccess.GetAll();
        }

        public bool DeleteUserOrganizationByUsername(string userName)
        {
            return _UserOrganizationDataAccessMaster.DeleteUserOrganizationByUsername(userName);
        }
        public bool DeleteUserOrganizationByUsernameAndCompanyId(string userName,Guid CompanyId)
        {
            return _UserOrganizationDataAccessMaster.DeleteUserOrganizationByUsernameAndCompanyId(userName, CompanyId);
        }

        public List<UserOrganization> GetAllUserOrganizationsByUsername(string username)
        {
            return _UserOrganizationDataAccessMaster.GetByQuery(string.Format("UserName ='{0}'", username));
        }

        public List<Organization> GetAllOrganizationsByUsername(string username)
        {
            return _OrganizationDataAccess.GetAllOrganizationsByUsername(username);
        }

        public void UpdateUserNameByUserName(string oldUsername, string newusername)
        {
            _UserOrganizationDataAccessMaster.UpdateUserNameByUserName(oldUsername,newusername);
        }

        public UserOrganization GetOrganizationByUserName(string user, Guid companyid)
        {
            return _UserOrganizationDataAccessMaster.GetByQuery(string.Format("UserName = '{0}' and CompanyId = '{1}'", user, companyid)).FirstOrDefault();
        }

        public bool UpdateUserOrganiZation(UserOrganization uo)
        {
            return _UserOrganizationDataAccessMaster.Update(uo) > 0;
        }

        public bool DeleteUserOrganization(UserOrganization userOrganization)
        {
            return _UserOrganizationDataAccessMaster.Delete(userOrganization.Id) > 0;
        }

        public int InsertOrganization(Organization org)
        {
            return (int)_OrganizationDataAccess.Insert(org);
        }

        public bool DeleteResturantMasterDB(Guid comid)
        {
            return _UserOrganizationDataAccessMaster.DeleteResturantMasterDB(comid);
        }
    }
}
