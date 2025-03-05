using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Facade
{
    public class HSMainApiFacade : BaseFacade
    {
        UserOrganizationDataAccess _UserOrganizationDataAccess = null;
        
        public HSMainApiFacade(ClientContext clientContext)
            : base(clientContext)
        {
            if (_UserOrganizationDataAccess == null)
                _UserOrganizationDataAccess = new UserOrganizationDataAccess(clientContext);
        }
        public HSMainApiFacade()
        {
            if (_UserOrganizationDataAccess == null)
            {
                string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["hsCoonectionString"].ConnectionString;
                _UserOrganizationDataAccess = new UserOrganizationDataAccess(ConnectionString);
            }
        }
        public HSMainApiFacade(string ConnectionStr)
        {
            if (_UserOrganizationDataAccess == null)
                _UserOrganizationDataAccess = new UserOrganizationDataAccess(ConnectionStr);
        } 
        public CompanyConneciton GetCompanyConnectionByUserName(string userName)
        {
            string message = "Start:: ";
            DataTable dt = _UserOrganizationDataAccess.GetCompanyConnectionByUsername(userName);
            List<CompanyConneciton> CompanyConnecitonList = new List<CompanyConneciton>();
            if (dt == null)
                message += "NULL ";


            if (dt.Rows.Count > 0)
            {
                CompanyConnecitonList = (from DataRow dr in dt.Rows
                                         select new CompanyConneciton()
                                         {
                                             CompanyId = (Guid)dr["CompanyId"],
                                             ConnectionString = dr["ConnectionString"].ToString(),
                                             MasterPassword = dr["MasterPassword"].ToString()
                                         }).ToList();
            }
            return CompanyConnecitonList.FirstOrDefault();
        }

        public CompanyConneciton GetCompanyConnectionByUsernameAndCompanyId(string userName, Guid comid)
        {
            DataTable dt = _UserOrganizationDataAccess.GetIeateryCompanyConnectionByUsernameAndCompanyId(userName, comid);
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

        public bool SetDefaultUserCompany(string username, Guid companyId)
        {
            return _UserOrganizationDataAccess.SetDefaultUserCompany(username, companyId);
        }
        public bool UserIsInCompany(string Username, Guid companyId)
        {
            return _UserOrganizationDataAccess.GetByQuery(string.Format(" Username ='{0}'and CompanyId = '{1}' ", Username, companyId)).Count() > 0;
        }
        public List<UserOrganization> GetUsersOrganizationListByUsername(string UserName)
        {
            DataTable dt = _UserOrganizationDataAccess.GetUsersOrganizationListByUsername(UserName);
            List<UserOrganization> userCompanyList = new List<UserOrganization>();
            userCompanyList = (from DataRow dr in dt.Rows
                               select new UserOrganization()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   CompanyName = dr["CompanyName"].ToString(),
                                   UserName = dr["UserName"].ToString(),
                                   CompanyId = (Guid)dr["CompanyId"],
                                   IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                                   ConnectionString = dr["ConnectionString"].ToString(),
                               }).ToList();
            return userCompanyList;
        }
    }
}
