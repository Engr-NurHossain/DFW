using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace HS.Facade
{
    public class UserLoginFacade : BaseFacade
    {
        UserOrganizationDataAccess _UserOrganizationDataAccessMaster = null;
        UserLoginDataAccess _UserLoginDataAccess;
        public UserLoginFacade(ClientContext clientContext)
            : base(clientContext)
        {
            _UserOrganizationDataAccessMaster = new UserOrganizationDataAccess(ConfigurationBlock.ConnectionString);
            _UserLoginDataAccess = (UserLoginDataAccess)_ClientContext[typeof(UserLoginDataAccess)];
        }

        public UserLoginFacade()
        {
            _UserLoginDataAccess = new UserLoginDataAccess();
        }

        public UserLoginFacade(string constr)
        {
            _UserLoginDataAccess = new UserLoginDataAccess(constr);
        }

        PermissionDataAccess _PermissionDataAccess
        {
            get
            {
                return (PermissionDataAccess)_ClientContext[typeof(PermissionDataAccess)];
            }
        }
        PermissionGroupMapDataAccess _PermissionGroupMapDataAccess
        {
            get
            {
                return (PermissionGroupMapDataAccess)_ClientContext[typeof(PermissionGroupMapDataAccess)];
            }
        }

        StaffDataAccess _StaffDataAccess
        {
            get
            {
                return (StaffDataAccess)_ClientContext[typeof(StaffDataAccess)];
            }
        }
        public UserPermissionDataAccess _UserPermissionDataAccess
        {
            get
            {
                return (UserPermissionDataAccess)_ClientContext[typeof(UserPermissionDataAccess)];
            }
        }
        public PermissionGroupDataAccess _PermissionGroupDataAccess
        {
            get
            {
                return (PermissionGroupDataAccess)_ClientContext[typeof(PermissionGroupDataAccess)];
            }
        }
        public TeamSettingDataAccess _TeamSettingDataAccess
        {
            get
            {
                return (TeamSettingDataAccess)_ClientContext[typeof(TeamSettingDataAccess)];
            }
        }

        public List<int> GetPermissionParentIdList()
        {
            return _PermissionDataAccess.GetPermissionParentIdList();
        }

        public bool PIdIsParentId(int pid)
        {
            return GetPermissionParentIdList().Contains(pid);
        }

        public UserLogin GetUserByUsername(string username)
        {
            return _UserLoginDataAccess.GetByQuery(string.Format(" UserName ='{0}'", username), true).FirstOrDefault();
        }

        public UserLogin GetUserByUsernameAndCompanyId(string username, Guid comid)
        {
            return _UserLoginDataAccess.GetByQuery(string.Format(" UserName ='{0}' and CompanyId = '{1}'", username, comid), true).FirstOrDefault();
        }

        public List<UserLogin> GetUserListByUsername(string username)
        {
            return _UserLoginDataAccess.GetByQuery(string.Format(" UserName ='{0}'", username), true).ToList();
        }

        public UserLogin GetUserByUsername(string username, bool ResetDb)
        {
            return _UserLoginDataAccess.GetByQuery(string.Format(" UserName ='{0}'", username), ResetDb).FirstOrDefault();
        }
        public bool InsertPermissionGroup(PermissionGroup permissionGroup)
        {
            return _PermissionGroupDataAccess.Insert(permissionGroup) > 0;
        }
        public bool InsertTeamSetting(TeamSetting team)
        {
            return _TeamSettingDataAccess.Insert(team) > 0;
        }
        public bool UpdatePermissionGroup(PermissionGroup permissionGroup)
        {
            return _PermissionGroupDataAccess.Update(permissionGroup) > 0;
        }
        public bool UpdateTeamSetting(TeamSetting team)
        {
            return _TeamSettingDataAccess.Update(team) > 0;
        }
        public bool UpdatePermission(Permission permission)
        {
            return _PermissionDataAccess.Update(permission) > 0;
        }
        public bool DeletePermissionGroup(int id)
        {
            return _PermissionGroupDataAccess.Delete(id) > 0;
        }
        public bool DeleteTeamSetting(int id)
        {
            return _TeamSettingDataAccess.Delete(id) > 0;
        }
        public bool UpdateGroupPermissionPermissionGroup(int gid, int pid, Guid comid)
        {
            PermissionGroupMap maplist = _PermissionGroupMapDataAccess.GetByQuery(string.Format(" [PermissionGroupId]={0} AND [PermissionId]={1} and CompanyId = '{2}'", gid, pid, comid)).FirstOrDefault();

            if (maplist != null && maplist.Id > 0)
            {
                return _PermissionGroupMapDataAccess.Delete(maplist.Id) > 0;
            }
            else
            {
                maplist = new PermissionGroupMap();
                maplist.PermissionGroupId = gid;
                maplist.PermissionId = pid;
                maplist.IsActive = true;
                maplist.CompanyId = comid;
                return _PermissionGroupMapDataAccess.Insert(maplist) > 0;
            }
        }

        public List<Permission> GetPermissionByGroupId(int ComId,string PermissionName)
        {
            return _PermissionDataAccess.GetPermissionByGroupId(ComId, PermissionName);
        }
        public PermissionGroup GetPermissionGroupById(int ComId)
        {
            return _PermissionGroupDataAccess.Get(ComId);
        }
        public TeamSetting GetTeamSettingById(int Id)
        {
            return _TeamSettingDataAccess.Get(Id);
        }
        public List<PermissionGroup> GetAllPermissionGroup(Guid ComId)
        {
            return _PermissionGroupDataAccess.GetAllPermissionGroup(ComId);

        }
        public List<PermissionGroup> GetAllPermissionGroupForCurrentEmployee(Guid ComId)
        {
            return _PermissionGroupDataAccess.GetAllPermissionGroupForCurrentEmployee(ComId);

        }
        public List<TeamSetting> GetAllTeam()
        {
            return _TeamSettingDataAccess.GetAll();

        }

        public UserMgmtListModel GetAllUserMgmtListByCompanyId(Guid ComId, int? UserGroup, string currentemp, string grpname, int pageNo, int pageSize,string searchText, Guid userid, string Order)
        {
            DataSet dt = _UserLoginDataAccess.GetAllUserMgmtListByCompanyId(ComId, UserGroup, currentemp, grpname,pageNo, pageSize, searchText, userid, Order);
            UserMgmtListModel UserList = new UserMgmtListModel();
            UserList.UserMgmtList = (from DataRow dr in dt.Tables[0].Rows
                        select new UserMgmtList()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            UserId = dr["UserId"] != DBNull.Value ? (Guid)dr["UserId"] : Guid.Empty,
                            ContactName = dr["ContactName"].ToString(),
                            AccessRights = dr["AccessRights"].ToString(),
                            Email = dr["Email"].ToString(),
                            Tags = dr["Tags"].ToString(),
                            IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                            IsCalendar = dr["IsCalendar"] != DBNull.Value ? Convert.ToBoolean(dr["IsCalendar"]) : false,
                            EmpType= dr["EmployeeType"].ToString(),
                            CalendarColor = dr["CalendarColor"].ToString(),
                            Supervisor = dr["Supervisor"].ToString(),
                            RouteList = dr["RouteList"].ToString(),
                            IsDeleted = dr["IsDeleted"] != DBNull.Value ? Convert.ToBoolean(dr["IsDeleted"]) : false,
                            IsCurrentEmployee = dr["IsCurrentEmployee"] != DBNull.Value ? Convert.ToBoolean(dr["IsCurrentEmployee"]) : false,
                        }).ToList();
            UserList.TotalCount = dt.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(dt.Tables[1].Rows[0]["TotalCount"]) : 0;
            UserList.PageNo = pageNo;
            UserList.PageSize = pageSize;
            UserList.SearchText = searchText;
            return UserList;
        }
        public List<UserMgmtList> GetAllUserMgmtListByCompanyId(Guid ComId, int? UserGroup, string currentemp, string grpname, string searchText)
        {
            DataTable dt = _UserLoginDataAccess.GetAllUserMgmtListByCompanyId(ComId, UserGroup, currentemp, grpname, searchText);
            List<UserMgmtList> UserList = new List<UserMgmtList>();
            UserList = (from DataRow dr in dt.Rows
                                     select new UserMgmtList()
                                     {
                                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                         UserId = dr["UserId"] != DBNull.Value ? (Guid)dr["UserId"] : Guid.Empty,
                                         ContactName = dr["ContactName"].ToString(),
                                         AccessRights = dr["AccessRights"].ToString(),
                                         Email = dr["Email"].ToString(),
                                         Tags = dr["Tags"].ToString(),
                                         IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                                         IsCalendar = dr["IsCalendar"] != DBNull.Value ? Convert.ToBoolean(dr["IsCalendar"]) : false,
                                         CalendarColor = dr["CalendarColor"].ToString(),
                                         Supervisor = dr["Supervisor"].ToString(),
                                         IsDeleted = dr["IsDeleted"] != DBNull.Value ? Convert.ToBoolean(dr["IsDeleted"]) : false,
                                         IsCurrentEmployee = dr["IsCurrentEmployee"] != DBNull.Value ? Convert.ToBoolean(dr["IsCurrentEmployee"]) : false,
                                         Status = dr["Status"].ToString(),
                                         CurrentEmp = dr["CurrentEmployee"].ToString()
                                     }).ToList();
            return UserList;
        }
        public List<UserMgmtList> GetAllIsCurrentUserMgmtListByCompanyId(Guid ComId, int? UserGroup, string currentemp, string grpname, string searchText)
        {
            DataTable dt = _UserLoginDataAccess.GetAllIsCurrentUserMgmtListByCompanyId(ComId, UserGroup, currentemp, grpname, searchText);
            List<UserMgmtList> UserList = new List<UserMgmtList>();
            UserList = (from DataRow dr in dt.Rows
                        select new UserMgmtList()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            UserId = dr["UserId"] != DBNull.Value ? (Guid)dr["UserId"] : Guid.Empty,
                            ContactName = dr["ContactName"].ToString(),
                            AccessRights = dr["AccessRights"].ToString(),
                            Email = dr["Email"].ToString(),
                            Tags = dr["Tags"].ToString(),
                            IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                            IsCalendar = dr["IsCalendar"] != DBNull.Value ? Convert.ToBoolean(dr["IsCalendar"]) : false,
                            CalendarColor = dr["CalendarColor"].ToString(),
                            Supervisor = dr["Supervisor"].ToString(),
                            IsDeleted = dr["IsDeleted"] != DBNull.Value ? Convert.ToBoolean(dr["IsDeleted"]) : false,
                            IsCurrentEmployee = dr["IsCurrentEmployee"] != DBNull.Value ? Convert.ToBoolean(dr["IsCurrentEmployee"]) : false,
                            Status = dr["Status"].ToString(),
                            CurrentEmp = dr["CurrentEmployee"].ToString()
                        }).ToList();
            return UserList;
        }
        public DataTable GetAllUserForExport(int? UserGroup, string searchText, string currentemp, Guid ComId)
        {
            return _UserLoginDataAccess.GetAllUserForExport(UserGroup, searchText, currentemp, ComId);
        }
        public List<UserMgmtList> GetAllRecruitUserListByCompanyId(Guid ComId, string UserStatus)
        {
            DataTable dt = _UserLoginDataAccess.GetAllRecruitUserListByCompanyId(ComId, UserStatus);
            List<UserMgmtList> UserList = new List<UserMgmtList>();
            UserList = (from DataRow dr in dt.Rows
                        select new UserMgmtList()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            ContactName = dr["ContactName"].ToString(),
                            AccessRights = dr["AccessRights"].ToString(),
                            Email = dr["Email"].ToString(),
                            Tags = dr["Tags"].ToString(),
                            IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                            IsRecruited = dr["IsRecruited"] != DBNull.Value ? Convert.ToBoolean(dr["IsRecruited"]) : false,
                            LastUpdate = dr["LastUpdate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdate"]) : new DateTime(),
                            EmpPhone = dr["EmpPhone"].ToString(),
                            UserStreet = dr["UserStreet"].ToString(),
                            UserLocation = dr["UserLocation"].ToString(),
                            Datestamp = dr["Datestamp"] != DBNull.Value ? Convert.ToDateTime(dr["Datestamp"]) : new DateTime(),
                            EmpStatus = dr["EmpStatus"].ToString()
                        }).ToList();
            return UserList;
        }

        public bool IsRightUser(string email, int value, Guid comid)
        {
            UserLogin ul = _UserLoginDataAccess.GetByQuery(string.Format("UserName ='{0}' and id = {1} and CompanyId = '{1}'", email, value, comid)).FirstOrDefault();
            return ul != null;
        }
        public bool IsUserExists(string email, Guid comid)
        {
            UserLogin ul = _UserLoginDataAccess.GetByQuery(string.Format(" UserName = '{0}' and IsActive=1 and CompanyId = '{1}'", email, comid)).FirstOrDefault();
            return ul != null;
        }

        public UserLogin GetById(int value)
        {
            return _UserLoginDataAccess.Get(value);
        }
        public Permission GetPermissionById(int value)
        {
            return _PermissionDataAccess.Get(value);
        }

        public UserLogin GetUserType(string email, string password, string MasterPassword, bool RememberMe, Guid comid)
        {
            UserLogin ul = _UserLoginDataAccess.GetUserLogin(email, password, MasterPassword, RememberMe, comid);

                //string.Format(" UserName ='{0}' and ([Password] = '{1}' or '{2}' ='{1}' )  and IsActive = 1 and IsDeleted =0 and CompanyId = '{3}'",
                //email, 
                //password, 
                //MasterPassword, 
                //comid)).FirstOrDefault();

            // SQLParameter

            if (ul != null)
            {

                #region Previous
                //Staff st = _StaffDataAccess.GetByQuery(string.Format(" UserName ='{0}'",email)).FirstOrDefault();
                //if (st != null)
                //{
                //    ul.UserType= "staff";
                //    return ul;
                //}
                //else
                //{
                //    ul.UserType= "customer";
                //    return ul;
                //}
                #endregion
                UserPermission up = _UserPermissionDataAccess.GetByQuery(string.Format(" UserId ='{0}' and PermissionGroupId is not null and CompanyId = '{1}'", ul.UserId, comid)).FirstOrDefault();
                if (up != null)
                {
                    //if (HttpContext.Current.Cache[RMRCacheKey.PermissionGroups] != null)
                    //{
                    //    List<PermissionGroup> PermissionGroupList = (List<PermissionGroup>)HttpContext.Current.Cache[RMRCacheKey.PermissionGroups];
                    //    PermissionGroup PermissionGroup  = PermissionGroupList.Where(x => x.Id == up.PermissionGroupId).FirstOrDefault();
                    //    if (PermissionGroup != null)
                    //    {
                    //        ul.UserType = PermissionGroup.Name;
                    //    }
                    //}
                    //else
                    //{
                        List<PermissionGroup> PermissionGroupList = _PermissionGroupDataAccess.GetAll();
                        //HttpContext.Current.Cache[RMRCacheKey.PermissionGroups] = PermissionGroupList;
                        ul.UserType = PermissionGroupList.Where(x => x.Id == up.PermissionGroupId).FirstOrDefault().Name;
                    //}
                    //FormsAuthentication.SetAuthCookie(ul.UserName, RememberMe);

                    //ul.UserType = "staff";
                }
                else
                {
                    ul.UserType = "none";
                }
                return ul;
            }
            else
            {
                ul = new UserLogin();
                ul.UserType = "none";
                return ul;
            }
        }

        public UserLogin GetUserLoginById(int Id)
        {
            return _UserLoginDataAccess.Get(Id);
        }

        public long InsertUserLogin(UserLogin ul2)
        {
            return _UserLoginDataAccess.Insert(ul2);
        }

        public bool UpdateUserLogin(UserLogin ul)
        {
            return _UserLoginDataAccess.Update(ul) > 0;
        }

        public bool DeleteUserLogin(int id)
        {
            return _UserLoginDataAccess.Delete(id) > 0;
        }
        public bool DeleteUserLoginByUsername(string Username)
        {
            return _UserLoginDataAccess.DeleteUserLoginByUsername(Username);
        }
        public bool InsertUserCredential(string UserName, Guid UserId, Guid CompanyId, string Email, string Password)
        {
            _UserOrganizationDataAccessMaster.InsertUserCredential(UserName, CompanyId);
            return _UserLoginDataAccess.InsertUserCredential(UserName, UserId, CompanyId, Email, Password);
        }
        public UserLogin GetByIdAndCompanyId(int Id, Guid CompanyId)
        {
            return _UserLoginDataAccess.GetByQuery(string.Format("Id = {0}", Id)).FirstOrDefault();

            /*
             SELECT  ul.*
  
              FROM [UserCompany] uc

              left join UserLogin ul 
              on uc.UserName = ul.UserName

              where Uc.CompanyId =''


            next time this script will run..
            for now I am going with this..
             */
        }

        public void UpdateAllUserNameByUserName(string OldUserName, string NewUserName)
        {
            _UserLoginDataAccess.UpdateAllUserNameByUserName(OldUserName, NewUserName);
        }

        public List<UserLogin> GetAllRecruitUserListByCompanyId(Guid CompanyId)
        {
            return _UserLoginDataAccess.GetByQuery(string.Format(" CompanyId='{0}'", CompanyId));
        }
        public List<Permission> GetAllPermissionGroupByPid(int pid)
        {
            return _PermissionDataAccess.GetByQuery(string.Format("ParentId='{0}'", pid));
        }
        public List<UserLogin> GetAllUserLogin()
        {
            return _UserLoginDataAccess.GetAll();
        }
        public UserLogin GetUserLoginByUserId(Guid id)
        {
            return _UserLoginDataAccess.GetByQuery(string.Format("UserId = '{0}'", id)).FirstOrDefault();
        }

        public UserLogin GetUserLoginByUserName(string user)
        {
            return _UserLoginDataAccess.GetByQuery(string.Format("UserName = '{0}'", user)).FirstOrDefault();
        }
    }
}
