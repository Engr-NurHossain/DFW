using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Entities
{
    public class UserMgmtList
    {
        public int Id { set; get; }
        public Guid UserId { set; get; }
        public string Email { set; get; }
        public string ContactName { set; get; }
        public string AccessRights { set; get; }
        public string Tags { set; get; }
        public bool IsActive { set; get; }
        public bool IsRecruited { set; get; }
        public DateTime LastUpdate { get; set; }
        public string EmpPhone { get; set; }
        public string UserStreet { get; set; }
        public string UserLocation { get; set; }
        public DateTime Datestamp { get; set; }
        public string EmpStatus { get; set;}
        public bool IsCalendar { set; get; }
        public string EmpType { get; set; }
        public string CalendarColor { set; get; }
        public string Supervisor { set; get; }
        public bool IsDeleted { set; get; }
        public bool IsCurrentEmployee { get; set; }
        public string RouteList { get; set; }
        public string Status { get; set; }

        public string CurrentEmp { get; set; }
    }
    public class UserMgmtListModel
    {
        public List<UserMgmtList> UserMgmtList { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public string SearchText { set; get; }
    }
    public class AddUser
    {
        public UserLogin UserLogin { set; get; }
        public PermissionGroup PermissionGroup { set; get; }
        public UserBranch UserBranchDetails { get; set; }
        public List<Permission> Permisssions { set; get; }
        public Employee Employee { set; get; }
        public List<PermissionGroup> PermissionGroupList { get; set; }
        public List<CustomUserPermission> CustomUserPermissionList { get; set; }
        public List<RecruitmentForm> RecruitmentForms { set; get; }
        public List<EmployeeLeadSource> employeeLeadSources { set; get; }
        public List<RouteList> RouteList { get; set; }
    }
    public class CustomUserPermission
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int PermissionId { get; set; }
        public int PermissionParentId { get; set; }
        public string PermissionName { get; set; }
    }
    public class EidtUserPermission
    {
        public string UserName { set; get; }
        public List<CustomUserPermission> CustomUserPermissionList { get; set; }
    }
}
