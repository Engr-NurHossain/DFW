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
using System.Web;

namespace HS.Facade
{
    public class PermissionFacade : BaseFacade
    {
        PermissionDataAccess _PermissionDataAccess;
        UserPermissionDataAccess _UserPermissionDataAccess;
        PermissionGroupMapDataAccess _PermissionGroupMapDataAccess;
        PermissionGroupDataAccess _PermissionGroupDataAccess;
        public PermissionFacade(ClientContext clientContext)
            : base(clientContext)
        {
            if (_PermissionDataAccess == null)
                _PermissionDataAccess = (PermissionDataAccess)_ClientContext[typeof(PermissionDataAccess)];
            if (_UserPermissionDataAccess == null)
                _UserPermissionDataAccess = (UserPermissionDataAccess)_ClientContext[typeof(UserPermissionDataAccess)];
            if (_PermissionGroupMapDataAccess == null)
                _PermissionGroupMapDataAccess = (PermissionGroupMapDataAccess)_ClientContext[typeof(PermissionGroupMapDataAccess)];
            if (_PermissionGroupDataAccess == null)
                _PermissionGroupDataAccess = (PermissionGroupDataAccess)_ClientContext[typeof(PermissionGroupDataAccess)];

        }
        public PermissionFacade()
        {
            if(HttpContext.Current!=null && HttpContext.Current.Session!=null && !string.IsNullOrWhiteSpace(HttpContext.Current.Session[SessionKeys.CompanyConnectionString].ToString()))
            {
                string CompanyConnectionstr = HttpContext.Current.Session[SessionKeys.CompanyConnectionString].ToString();

                if (_PermissionDataAccess == null)
                    _PermissionDataAccess = new PermissionDataAccess(CompanyConnectionstr);
                if (_UserPermissionDataAccess == null)
                    _UserPermissionDataAccess = new UserPermissionDataAccess(CompanyConnectionstr);
                if (_PermissionGroupMapDataAccess == null)
                    _PermissionGroupMapDataAccess = new PermissionGroupMapDataAccess(CompanyConnectionstr);
                if (_PermissionGroupDataAccess == null)
                    _PermissionGroupDataAccess = new PermissionGroupDataAccess(CompanyConnectionstr);
            } 
        }

        public PermissionFacade(string constr)
        {
            _PermissionDataAccess = new PermissionDataAccess(constr);
            _PermissionGroupDataAccess = new PermissionGroupDataAccess(constr);
            _UserPermissionDataAccess = new UserPermissionDataAccess(constr);
            _PermissionGroupMapDataAccess = new PermissionGroupMapDataAccess(constr);
        }

        public bool DeleteExistingPermissionGroupMapByPermissionParentIdAndGroupId(int ParentId,int GroupId, Guid comid)
        {
            return _PermissionGroupMapDataAccess.DeleteExistingPermissionGroupMapByPermissionParentIdAndGroupId(ParentId,GroupId, comid);
        }

        public List<Permission> GetPermissionListByParentId(int pid)
        {
            return _PermissionDataAccess.GetByQuery(string.Format(" ParentId='{0}'", pid));
        }

        public bool IsPermitted(int id, Guid UserId, Guid comid)
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null
                ///*Development perpouse*/ || HttpContext.Current.Session[SessionKeys.UserRole] == null by digiture : mayur
                )
            {
                return false;
            }
            /*Development perpouse*/
            //string UserRole = HttpContext.Current.Session[SessionKeys.UserRole].ToString(); by digiture mayur
            /*Development perpouse*/
            //if (AppConfig.Production == "false" && ( UserRole == "SysAdmin" || UserRole == "Admin"))
            //{
            //    return true;
            //}

            List<int> permissions = GetAllUserPermissions(UserId, comid);
            bool result = permissions.Contains(id);
            return result;
        }

        public bool IsPermitted_old(int id, Guid UserId, Guid comid)
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null
                /*Development perpouse*/ || HttpContext.Current.Session[SessionKeys.UserRole] == null
                )
            {
                return false;
            }
            /*Development perpouse*/
            string UserRole = HttpContext.Current.Session[SessionKeys.UserRole].ToString();
            /*Development perpouse*/
            //if (AppConfig.Production == "false" && ( UserRole == "SysAdmin" || UserRole == "Admin"))
            //{
            //    return true;
            //}

            List<int> permissions = GetAllUserPermissions(UserId, comid);
            bool result = permissions.Contains(id);
            return result;
        }

        public bool IsPermittedPermission(int id, Guid UserId, Guid comid)
        {
            List<int> permissions = GetAllUserPermissions(UserId, comid);
            bool result = permissions.Contains(id);
            return result;
        }

        public bool DeleteExistingPermissionGroupMapByPermissionIdAndGroupId(int pid, int gid, Guid comid)
        {
            return _PermissionGroupMapDataAccess.DeleteExistingPermissionGroupMapByPermissionIdAndGroupId(pid,gid, comid);
        }

        public int InsertPermissionGroupMap(PermissionGroupMap pgm)
        {
            return (int)_PermissionGroupMapDataAccess.Insert(pgm);
        }

        public List<int> GetAllUserPermissions(Guid UserId, Guid comid)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session[SessionKeys.UserPermissionList] != null)
                {
                    List<int> permissions = (List<int>)HttpContext.Current.Session[SessionKeys.UserPermissionList];
                    return permissions;
                }
                else
                {
                    List<UserPermission> upermissions = _UserPermissionDataAccess.GetByQuery(string.Format(" UserId ='{0}' and CompanyId = '{1}'", UserId, comid));
                    List<int> Ids = new List<int>();
                    if (upermissions.Count() > 0)
                    {
                        Ids = upermissions.Where(x => x.PermissionId.HasValue).Select(x => x.PermissionId.Value).ToList();
                        int? GroupId = upermissions.Where(x => x.PermissionGroupId.HasValue).Select(x => x.PermissionGroupId).FirstOrDefault();
                        if (GroupId.HasValue && GroupId.Value > 0)
                        {  
                            List<PermissionGroupMap> permisssions = _PermissionGroupMapDataAccess.GetByQuery(string.Format("PermissionGroupId ={0} and CompanyId = '{1}'", GroupId, comid)).ToList();
                            Ids.AddRange(permisssions.Select(x => x.PermissionId).ToList());
                        }

                        HttpContext.Current.Session[SessionKeys.UserPermissionList] = Ids;
                    }
                    return Ids;
                }
            }
            return null;
            //return _UserPermissionDataAccess.GetByQuery(string.Format(" UserName ='{0}'", username));
        }

        public List<Permission> GetAllPermissions()
        {
            return _PermissionDataAccess.GetAll();
        }

        public List<Permission> GetPermissionListByIdList(List<int> list)
        {
            if (list.Count() == 0)
            {
                return null;
            }
            string idlist = "0";
            foreach (var item in list)
            {
                idlist += "," + item;
            }
            return _PermissionDataAccess.GetByQuery(string.Format("Id in ({0})", idlist));
        }

        public List<UserPermission> GetUserPermissionIdListByUserId(Guid UserId)
        {
            return _UserPermissionDataAccess.GetByQuery(string.Format(" (PermissionGroupId is null or PermissionId ='') and PermissionId is not null  and UserId = '{0}'", UserId));
        }

        public bool DeleteAllUserPermissionsByUserId(Guid UserId)
        {
            return _UserPermissionDataAccess.DeleteAllUserPermissionsByUserId(UserId);
        }

        public PermissionGroup GetPermissionGroupById(int permissionGroupId)
        {
            return _PermissionGroupDataAccess.Get(permissionGroupId);
        }

        public PermissionGroup GetPermissionGroupByTag(string tag)
        {
            return _PermissionGroupDataAccess.GetByQuery(string.Format("Tag like '%{0}%'", tag)).FirstOrDefault();
        }

        public UserPermission GetUserPermissionGroupByUserId(Guid UserId)
        {
            return _UserPermissionDataAccess.GetByQuery(string.Format(" PermissionGroupId is not null and UserId = '{0}'", UserId)).FirstOrDefault();
        }

        public long InsertUserPermission(UserPermission up)
        {
            return _UserPermissionDataAccess.Insert(up);
        }
        public long InsertPermissionGroup(PermissionGroup pg)
        {
            return _PermissionGroupDataAccess.Insert(pg);
        }

        public bool IsUserMasterAdmin(Guid UserId)
        {
            return _UserPermissionDataAccess.GetByQuery(string.Format(@" UserId = '{0}' and(
                                                                                                PermissionGroupId = 1 
                                                                                                --or PermissionGroupId = 2
                                                                                            )", UserId)).Count() > 0;
        }

        public PermissionGroup GetPermissionGroupByUserId(Guid UserId)
        {
            UserPermission up = _UserPermissionDataAccess.GetByQuery(string.Format(" PermissionGroupId is not null and UserId = '{0}'", UserId)).FirstOrDefault();
            if (up != null && up.PermissionGroupId.HasValue)
            {
                return _PermissionGroupDataAccess.Get(up.PermissionGroupId.Value);
            }
            return null;
        }

        //public List<Permission> GetPermissionListByUsername(string userName)
        //{
        //    List<UserPermission> upList = _UserPermissionDataAccess.GetByQuery(string.Format("PermissionGroupId is null and username = '{0}'", userName));
        //    if (upList.Count() > 0)
        //    {
        //        return GetPermissionListByIdList(upList.Where(x => x.PermissionId.HasValue).Select(x => x.PermissionId.Value).ToList());
        //    }
        //    return null;

        //}

        public List<PermissionGroup> GetAllPermissionGroupListByCompanyId(Guid CompanyId)
        {
            return _PermissionGroupDataAccess.GetByQuery(string.Format("CompanyId='{0}'",CompanyId));
        }

        public List<PermissionGroup> GetAllPermissionGroup()
        {
            return _PermissionGroupDataAccess.GetAll();
        }

        public List<CustomUserPermission> GetAllCustomPermission()
        {
            DataTable dt = _PermissionDataAccess.GetAllCustomPermission();
            List<CustomUserPermission> CusList = new List<CustomUserPermission>();
            CusList = (from DataRow dr in dt.Rows
                       select new CustomUserPermission()
                       {
                           RoleId = dr["RoleId"] != DBNull.Value ? Convert.ToInt32(dr["RoleId"]) : 0,
                           RoleName = dr["RoleName"].ToString(),
                           PermissionId = dr["PermissionId"] != DBNull.Value ? Convert.ToInt32(dr["PermissionId"]) : 0,
                           PermissionParentId = dr["PermissionParentId"] != DBNull.Value ? Convert.ToInt32(dr["PermissionParentId"]) : 0,
                           PermissionName = dr["PermissionName"].ToString()
                       }).ToList();
            return CusList;
        }
        public bool InsertGroupMapClone(int cloneId,string PermissionId, Guid comid)
        {
           
           return _PermissionGroupMapDataAccess.InsertGroupMapClone(cloneId,PermissionId, comid);
        }
        public int GetPGroupIdByNameAndCompanyId(string PGName , Guid CompanyId)
        {
            return _PermissionGroupDataAccess.GetByQuery(string.Format("Name = '{0}' and CompanyId ='{1}'", PGName, CompanyId)).FirstOrDefault().Id;
        }
        public bool CheckCurrentLogInUserHasPermissionByUserIdAndPermissionId(Guid UserId, int PermissionId)
        {
            PermissionDataAccess permissionDataAccess = new PermissionDataAccess();
            bool result = permissionDataAccess.CheckCurrentLogInUserHasPermissionByUserIdAndPermissionId(UserId, PermissionId);
            return result;
        }
        public List<Permission> GetAllPermissionsByUserIdAndCompanyId(Guid UserId, Guid CompanyId)
        {
            DataTable dt = _PermissionDataAccess.GetAllPermissionsByUserIdAndCompanyId(UserId, CompanyId);
            List<Permission> PermissionList = new List<Permission>();
            if(dt != null)
            PermissionList = (from DataRow dr in dt.Rows
                              select new Permission()
                              {
                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                  ParentId = dr["ParentId"] != DBNull.Value ? Convert.ToInt32(dr["ParentId"]) : 0,
                                  Name = dr["Name"].ToString()
                              }).ToList();
            return PermissionList;
        }
        public List<PermissionGroupMap> GetAllPermissionGroupMapByPermissionGroupId(int groupId, Guid comid)
        {
            DataTable dt = _PermissionDataAccess.GetAllPermissionsGroupmapByGropupId(groupId, comid);
            List<PermissionGroupMap> PermissionList = new List<PermissionGroupMap>();
            if (dt != null)
                PermissionList = (from DataRow dr in dt.Rows
                                  select new PermissionGroupMap()
                                  {
                                      Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                      PermissionGroupId = dr["PermissionGroupId"] != DBNull.Value ? Convert.ToInt32(dr["PermissionGroupId"]) : 0,
                                      PermissionId = dr["PermissionId"] != DBNull.Value ? Convert.ToInt32(dr["PermissionId"]) : 0,
                                  }).ToList();
            return PermissionList;
        }
        public bool DeleteAllUserPermissionsByUserIdAndCompanyId(Guid UserId, Guid Companyid)
        {
            var result = _UserPermissionDataAccess.DeleteAllUserPermissionsByUserIdAndCompanyId(UserId, Companyid);
            return result;
        }
        public int GetCustomPermissionGroupId()
        {
            int result = 0;
            var CustomGroupId = _PermissionGroupDataAccess.GetByQuery(string.Format("Name = 'Custom'")).FirstOrDefault();
            if (CustomGroupId != null)
            {
                result = CustomGroupId.Id;
            }
            return result;
        }

        public List<PermissionGroup> GetAllRecruitPermissionGroupListByCompanyId(Guid CompanyId)
        {
            return _PermissionGroupDataAccess.GetByQuery(string.Format("CompanyId='{0}' and  Tag like '%recruit%'", CompanyId));
        }

        public PermissionGroup GetPGroupByNameAndCompanyId(string pGroup, Guid CompanyId)
        {
            return _PermissionGroupDataAccess.GetByQuery(string.Format("Name = '{0}' and CompanyId ='{1}'", pGroup, CompanyId)).FirstOrDefault();
        }

        public List<UserPermission> GetAllSysAdminUserPermissions()
        {
            return _UserPermissionDataAccess.GetByQuery(string.Format("PermissionGroupId =1"));
        }

        public List<UserPermission> GetAllUserPermissionsPermissionGroupId(int id, Guid CompanyId)
        {
            DataTable dt =  _UserPermissionDataAccess.GetAllUserPermissionsPermissionGroupId(id, CompanyId);
            List<UserPermission> PermissionList = new List<UserPermission>();
            if (dt != null)
                PermissionList = (from DataRow dr in dt.Rows
                                  select new UserPermission()
                                  {
                                      Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                      PermissionGroupId = dr["PermissionGroupId"] != DBNull.Value ? Convert.ToInt32(dr["PermissionGroupId"]) : 0,
                                      PermissionId = dr["PermissionId"] != DBNull.Value ? Convert.ToInt32(dr["PermissionId"]) : 0,
                                      EmailAddress = dr["EmailAddress"].ToString(),
                                      FullName = dr["FullName"].ToString()
                                  }).ToList();
            return PermissionList;
        }

        public bool UpdateUserPermission(UserPermission up)
        {
            return _UserPermissionDataAccess.Update(up) > 0;
        }

        public PermissionGroupMap GetPermissionGroupMapByPermissionIdAndGroupId(int groupid, int permissionid)
        {
            return _PermissionGroupMapDataAccess.GetByQuery(string.Format("PermissionGroupId = {0} and PermissionId = '{1}'", groupid, permissionid)).FirstOrDefault();
        }

        public List<PermissionGroupMap> GetAllPermissionGroupMapByCompanyId(Guid comid)
        {
            return _PermissionGroupMapDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and PermissionGroupId = 1", comid)).ToList();
        }
    }
}
