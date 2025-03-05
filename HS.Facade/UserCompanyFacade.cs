using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using HS.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Facade
{
    public class UserCompanyFacade :BaseFacade
    {
        UserCompanyDataAccess _UserCompanyDataAccessMaster = null;
        UserCompanyDataAccess _UserCompanyDataAccess = null;
        UserBranchDataAccess _UserBranchDataAccess = null;
        UserCompanyDeviceDataAccess _UserCompanyDeviceDataAccess = null;
        
        public UserCompanyFacade(ClientContext clientContext)
            : base(clientContext)
        {
            if (_UserCompanyDeviceDataAccess == null)
                _UserCompanyDeviceDataAccess = (UserCompanyDeviceDataAccess)_ClientContext[typeof(UserCompanyDeviceDataAccess)];
            if (_UserCompanyDataAccess == null)
                _UserCompanyDataAccess = (UserCompanyDataAccess)_ClientContext[typeof(UserCompanyDataAccess)];
            if (_UserBranchDataAccess == null)
                _UserBranchDataAccess = (UserBranchDataAccess)_ClientContext[typeof(UserBranchDataAccess)];
            if (_UserCompanyDataAccessMaster == null)
                _UserCompanyDataAccessMaster = new UserCompanyDataAccess(System.Configuration.ConfigurationManager.ConnectionStrings["hsCoonectionString"].ConnectionString);
        }

        public UserCompanyFacade(string constr)
        {
            _UserCompanyDataAccess = new UserCompanyDataAccess(constr);
        }

        public List<UserCompany> GetUsersCompanyListByUserId(Guid Userid)
        {
            DataTable dt = _UserCompanyDataAccess.GetUsersCompanyListByUserId(Userid);
            List<UserCompany> userCompanyList = new List<UserCompany>();
            userCompanyList = (from DataRow dr in dt.Rows
                            select new UserCompany()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                CompanyName = dr["CompanyName"].ToString(),
                                UserId = (Guid)dr["UserId"], 
                                CompanyId = (Guid)dr["CompanyId"],
                                IsDefault = dr["IsDefault"] != DBNull.Value ? Convert.ToBoolean(dr["IsDefault"]) : false 
                            }).ToList();
            return userCompanyList; 
        } 
        public long InsertUserCompany(UserCompany uc)
        {
            UserCompany TemUc = _UserCompanyDataAccess.GetByQuery(string.Format("UserId = '{0}' and CompanyId ='{1}'", uc.UserId,uc.CompanyId)).FirstOrDefault();
            if (TemUc != null)
            {
                return TemUc.Id;
            }

            return _UserCompanyDataAccess.Insert(uc);
        }

        public UserCompany GetUserCompanyByUserIdAndCompanyId(Guid UserId, Guid companyId)
        {
            return _UserCompanyDataAccess.GetByQuery(string.Format(" UserId ='{0}'and CompanyId = '{1}' ", UserId, companyId)).FirstOrDefault();
        }

        public bool UserIsInCompany(Guid CompanyId,Guid ComId)
        {
            return _UserCompanyDataAccess.GetByQuery(string.Format(" UserId ='{0}'and CompanyId = '{1}' ", CompanyId, ComId)).Count()>0;
        }

        public bool RemoveDefaultCompanyByUserId(Guid UserId)
        {
            return _UserCompanyDataAccess.RemoveDefaultCompanyByUserId(UserId);
        }

        public bool SetDefaultUserCompany(Guid UserId, Guid companyId)
        {
            return _UserCompanyDataAccess.SetDefaultUserCompany(UserId, companyId);
        }

        public bool DeleteUserCompanyByUserId(Guid UserId)
        {
            return _UserCompanyDataAccess.DeleteUserCompanyByUserId(UserId);
        }

        public bool DeleteUserCompanyById(int value)
        {
            return _UserCompanyDataAccess.Delete(value) > 0;
        }
        public bool DeleteUserCompanyByUserIdAndCompanyId(Guid UserId,Guid CompanyId)
        {
            return _UserCompanyDataAccess.DeleteUserCompanyByUserIdAndCompanyId(UserId, CompanyId);
        }
        public List<Employee> GetAllUserByCompanyId(Guid CompanyId)
        {
            DataTable dt = _UserCompanyDataAccess.GetAllUsersListByCompanyId(CompanyId);
            List<Employee> userCompanyList = new List<Employee>();
            userCompanyList = (from DataRow dr in dt.Rows
                               select new Employee()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   FirstName = dr["FirstName"].ToString(),
                                   UserName = dr["UserName"].ToString(),
                                   LastName = dr["LastName"].ToString(),
                                   UserId = (Guid)dr["UserId"],
                               }).ToList();
            return userCompanyList;
        }

        public bool UpdateUserCompany(UserCompany uc)
        {
            return _UserCompanyDataAccess.Update(uc) > 0;
        }

        public UserBranch GetUserBranchByUserId(Guid userId)
        {
            return _UserBranchDataAccess.GetByQuery(string.Format(" UserId = '{0}'",userId)).FirstOrDefault();
        }
        //public UserCompanyDevice GetSingleDeviceIdForChangeCompany(Guid companyId, Guid UserId)
        //{
        //    return _UserCompanyDeviceDataAccess.GetByQuery(string.Format(" CompanyId ='{0}'and UserId = '{1}' and IsActive = 1 ", companyId, UserId)).FirstOrDefault();
        //}
        public List<UserCompanyDevice> GetDeviceIdForChangeCompany(Guid CompanyId, Guid UserId)
        {
            DataTable dt = _UserCompanyDeviceDataAccess.GetDeviceIdForChangeCompany(CompanyId,UserId);
            List<UserCompanyDevice> UserCompanyDeviceList = new List<UserCompanyDevice>();
            UserCompanyDeviceList = (from DataRow dr in dt.Rows
                               select new UserCompanyDevice()
                               {
                                   DeviceId = dr["DeviceId"].ToString()
                               }).ToList();

            return UserCompanyDeviceList;
        }
    }
}
