using HS.Entities;
using HS.Framework;
using HS.Framework.Utils;
using HS.Web.UI.App_Start;
using HS.Web.UI.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Rotativa;
using static HS.Entities.AppPermission;
using FrameWorkUserPermission = HS.Framework.UserPermissions;
using Excel = ClosedXML.Excel;
using System.Net;
using System.Web.Script.Serialization;

namespace HS.Web.UI.Controllers
{
    public class UserMgmtController : BaseController
    {

        public string PushNotification(Company model, Guid UserId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            string str = "";
            try
            {

                string applicationID = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(model.CompanyId, "ApplicationID").ToString();

                string senderId = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(model.CompanyId, "SenderId").ToString();

                var deviceId = _Util.Facade.UserCompanyFacade.GetDeviceIdForChangeCompany(CurrentUser.CompanyId.Value, UserId);

                List<string> DeviceIdList = new List<string>();

                foreach (var item in deviceId)
                {
                    DeviceIdList.Add(item.DeviceId.ToString());
                }

                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");

                tRequest.Method = "post";

                tRequest.ContentType = "application/json";


                var data = new

                {
                    to = string.Join(",", DeviceIdList),//deviceId, //

                    data = new

                    {
                        NotificationType = "company_change",

                        CompanyId = model.CompanyId.ToString(),

                        UserId = UserId.ToString(),

                        CompanyName = model.CompanyName.ToString(),
                    }
                };

                var serializer = new JavaScriptSerializer();

                var json = serializer.Serialize(data);

                Byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(json);

                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));

                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));

                tRequest.ContentLength = byteArray.Length;


                using (Stream dataStream = tRequest.GetRequestStream())
                {

                    dataStream.Write(byteArray, 0, byteArray.Length);


                    using (WebResponse tResponse = tRequest.GetResponse())
                    {

                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {

                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {

                                String sResponseFromServer = tReader.ReadToEnd();

                                return str = sResponseFromServer;

                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                return str = ex.Message;
            }
        }

        [Authorize]
        // GET: UserMgmt
        public ActionResult Index(int? Id)
        {
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            if (Id.HasValue)
            {
                ViewBag.Id = Id.Value;
            }
            return View();
        }
        [Authorize]
        public PartialViewResult UserGroup()
        {

            if (!base.IsPermitted(UserPermissions.UserMgmtPermissions.ManageUserGroup))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            Guid ComId = ((CustomPrincipal)(User)).CompanyId.Value;

            PermissionGroupWithUserList pgw = new PermissionGroupWithUserList();
            pgw.PermissionGroupList = _Util.Facade.UserLoginFacade.GetAllPermissionGroupForCurrentEmployee(ComId);

            foreach (var item in pgw.PermissionGroupList)
            {

                item.UserList = _Util.Facade.UserLoginFacade.GetAllIsCurrentUserMgmtListByCompanyId(ComId, null, null, item.Name, null);

            }

            return PartialView("_UserGroup", pgw);
        }

        [Authorize]
        public PartialViewResult UserTeam()
        {
            List<TeamSetting> model = new List<TeamSetting>();
            model = _Util.Facade.UserLoginFacade.GetAllTeam();
            return PartialView("_UserTeam", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult SavePermission(int gid, int pid, bool isChecked)
        {
            /*
                 if isChecked = true -> add
                 if isChecked = false -> Delete
            */

            var result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (!base.IsPermitted(UserPermissions.UserMgmtPermissions.UserChangePermissionGroup))
            {
                return Json(false);
            }

            if (gid > 0 && pid > 0)
            {
                bool IsParentId = _Util.Facade.UserLoginFacade.PIdIsParentId(pid);
                if (IsParentId)
                {
                    //List<Permission> Permisiionlist = _Util.Facade.PermissionFacade.GetPermissionListByParentId(pid);
                    _Util.Facade.PermissionFacade.DeleteExistingPermissionGroupMapByPermissionParentIdAndGroupId(pid, gid, CurrentUser.CompanyId.Value);
                    if (isChecked)
                    {
                        List<Permission> Permisiionlist = _Util.Facade.PermissionFacade.GetPermissionListByParentId(pid);

                        PermissionGroupMap pgm = new PermissionGroupMap()
                        {
                            IsActive = true,
                            PermissionGroupId = gid,
                            PermissionId = pid,
                            CompanyId = CurrentUser.CompanyId.Value
                        };
                        _Util.Facade.PermissionFacade.InsertPermissionGroupMap(pgm);

                        foreach (var item in Permisiionlist)
                        {
                            pgm = new PermissionGroupMap()
                            {
                                IsActive = true,
                                PermissionGroupId = gid,
                                PermissionId = item.Id,
                                CompanyId = CurrentUser.CompanyId.Value
                            };
                            _Util.Facade.PermissionFacade.InsertPermissionGroupMap(pgm);
                        }
                    }
                }
                else
                {
                    if (isChecked)
                    {
                        PermissionGroupMap pgm = new PermissionGroupMap()
                        {
                            IsActive = true,
                            PermissionGroupId = gid,
                            PermissionId = pid,
                            CompanyId = CurrentUser.CompanyId.Value
                        };
                        _Util.Facade.PermissionFacade.InsertPermissionGroupMap(pgm);
                    }
                    else
                    {
                        _Util.Facade.PermissionFacade.DeleteExistingPermissionGroupMapByPermissionIdAndGroupId(pid, gid, CurrentUser.CompanyId.Value);
                    }
                }
                //result = _Util.Facade.UserLoginFacade.UpdateGroupPermissionPermissionGroup(gid, pid);
                //List<Permission> pg = _Util.Facade.UserLoginFacade.GetAllPermissionGroupByPid(pid);
                //foreach (var item in pg)
                //{
                //    result = _Util.Facade.UserLoginFacade.UpdateGroupPermissionPermissionGroup(gid, item.Id);
                //}
                //PermissionGroup pg = _Util.Facade.UserLoginFacade.GetPermissionByGroupId(gid);
            }
            return Json(result);
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteUserGroup(int? id)
        {

            var result = false;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return Json(result);
            }
            if (!base.IsPermitted(UserPermissions.UserMgmtPermissions.UserChangePermissionGroup))
            {
                return Json(false);
            }
            if (id.HasValue)
            {
                result = _Util.Facade.UserLoginFacade.DeletePermissionGroup(id.Value);
            }
            return Json(result);
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeleteUserTeam(int? id)
        {

            var result = false;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return Json(result);
            }
            if (id.HasValue)
            {
                result = _Util.Facade.UserLoginFacade.DeleteTeamSetting(id.Value);
            }
            return Json(result);
        }
        [Authorize]
        public ActionResult AddUserGroup(int? id)
        {
            PermissionGroup model;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (!base.IsPermitted(UserPermissions.UserMgmtPermissions.ManageUserGroupAdd))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (id.HasValue)
            {
                model = _Util.Facade.UserLoginFacade.GetPermissionGroupById(id.Value);
            }
            else
            {
                model = new PermissionGroup();
            }
            Guid ComId = ((CustomPrincipal)(User)).CompanyId.Value;
            ViewBag.UserTag = "0";
            if (id.HasValue)
            {
                PermissionGroup pg = _Util.Facade.UserLoginFacade.GetPermissionGroupById(id.Value);
                ViewBag.UserTag = pg.Tag;
            }
            ViewBag.TagList = _Util.Facade.LookupFacade.GetLookupByKey("ParmissionTag").OrderBy(x => x.DisplayText).Select(x =>
                         new SelectListItem()
                         {
                             Text = x.DisplayText,
                             Value = x.DataValue
                         }).ToList();

            return PartialView("_AddUserGroup", model);
        }
        [Authorize]
        public ActionResult AddUserTeam(int? id)
        {
            TeamSetting model;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (id.HasValue)
            {
                model = _Util.Facade.UserLoginFacade.GetTeamSettingById(id.Value);
            }
            else
            {
                model = new TeamSetting();
            }
            List<SelectListItem> SalesPersonList = new List<SelectListItem>();
            List<Employee> empLsit = _Util.Facade.EmployeeFacade.GetCurrentEmployeeListByCompanyId(currentLoggedIn.CompanyId.Value);
            if (empLsit != null && empLsit.Count() > 0)
            {
                SalesPersonList.AddRange(empLsit.OrderBy(x => x.FirstName.ToString() + " " + x.LastName.ToString()).Select(x =>
                      new SelectListItem()
                      {
                          Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                          Value = x.UserId.ToString()
                      }).ToList());
            }
            ViewBag.SalesPersonList = SalesPersonList.OrderBy(x => x.Text != "Please Select One").ThenBy(x => x.Text).ToList();
            return PartialView("_AddUserTeam", model);
        }
        [Authorize]
        public ActionResult PermissionGroupList(int? id, string permissionName)
        {

            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (!base.IsPermitted(UserPermissions.UserMgmtPermissions.ManageUserGroupAssignPermission))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            PermissionGroup model;
            if (id.HasValue)
            {
                model = _Util.Facade.UserLoginFacade.GetPermissionGroupById(id.Value);
            }
            else
            {
                model = new PermissionGroup();
            }
            if (model != null && !string.IsNullOrWhiteSpace(model.Name))
            {
                ViewBag.userGroupName = model.Name;
            }
            else
            {
                ViewBag.userGroupName = "";
            }

            if (id.HasValue)
            {
                ViewBag.PermissionId = id;
            }
            ViewBag.PermissionName = permissionName;

            return PartialView();
        }

        [Authorize]
        public ActionResult ChangeGroupPermission(int? id, string permissionName)
        {
            List<Permission> model;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (!base.IsPermitted(UserPermissions.UserMgmtPermissions.ManageUserGroupAssignPermission))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            ViewBag.UserTag = " ";

            if (id.HasValue)
            {
                model = _Util.Facade.UserLoginFacade.GetPermissionByGroupId(id.Value, permissionName).OrderBy(x => x.DisplayText).ToList();
                PermissionGroup pg = _Util.Facade.UserLoginFacade.GetPermissionGroupById(id.Value);
                ViewBag.UserType = pg.Name;
            }
            else
            {
                model = new List<Permission>();
            }

            return PartialView("_ChangeUserGroupPermission", model);
        }
        [Authorize]
        [HttpGet]
        public ActionResult AddClonePermission(int? id)
        {
            PermissionGroup model;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (!base.IsPermitted(UserPermissions.UserMgmtPermissions.ManageUserGroupAdd))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (id.HasValue)
            {
                model = _Util.Facade.UserLoginFacade.GetPermissionGroupById(id.Value);
            }
            else
            {
                model = new PermissionGroup();
            }

            return View(model);
        }
        [HttpPost]
        public JsonResult AddClonePermission(int CloneId, string Name)
        {
            var result = false;
            var message = "Internal Error.Can not Clone.";
            string pIdList = "";
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return Json(result);
            }
            if (!base.IsPermitted(UserPermissions.UserMgmtPermissions.ManageUserGroupAdd))
            {
                return Json(false);
            }
            PermissionGroup permissionGroup = _Util.Facade.PermissionFacade.GetPermissionGroupById(CloneId);
            PermissionGroup pg = new PermissionGroup();
            pg.CompanyId = permissionGroup.CompanyId;
            pg.Name = Name;
            pg.Tag = permissionGroup.Tag;
            if (pg.Name != permissionGroup.Name)
            {
                _Util.Facade.PermissionFacade.InsertPermissionGroup(pg);
                List<PermissionGroupMap> pgm = _Util.Facade.PermissionFacade.GetAllPermissionGroupMapByPermissionGroupId(CloneId, currentLoggedIn.CompanyId.Value);
                foreach (var item in pgm)
                {
                    pIdList += item.PermissionId.ToString() + ",";
                }
                pIdList = pIdList.TrimEnd(',');

                try
                {

                    result = _Util.Facade.PermissionFacade.InsertGroupMapClone(pg.Id, pIdList, currentLoggedIn.CompanyId.Value);
                    message = "Cloning Successfull";
                }
                catch (Exception ex)
                {
                    result = false;
                }
            }
            else
            {
                result = false;
                message = "You can not insert clone with same name.";
            }

            return Json(new { result = result, message = message });
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddUserGroup(PermissionGroup permissionGroup)
        {
            var result = false;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return Json(result);
            }
            if (!base.IsPermitted(UserPermissions.UserMgmtPermissions.ManageUserGroupAdd))
            {
                return Json(false);
            }
            permissionGroup.CompanyId = currentLoggedIn.CompanyId.Value;
            if (permissionGroup.Id > 0)
            {
                //var nameval = activation.Fee;
                //activation.Name = nameval.ToString();
                result = _Util.Facade.UserLoginFacade.UpdatePermissionGroup(permissionGroup);
            }
            else
            {
                //var nameval = activation.Fee;
                //activation.Name = nameval.ToString();
                result = _Util.Facade.UserLoginFacade.InsertPermissionGroup(permissionGroup);
            }

            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddUserTeam(TeamSetting team)
        {
            var result = false;
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return Json(result);
            }
            if (team.UsersId != null && team.UsersId.Count() > 0)
            {
                team.UserId = (string.Join(",", team.UsersId.Select(x => x.ToString()).ToArray()));
            }
            if (team.Id > 0)
            {
                TeamSetting tempTeam = _Util.Facade.UserLoginFacade.GetTeamSettingById(team.Id);
                if (tempTeam != null)
                {
                    team.CreatedBy = tempTeam.CreatedBy;
                    team.CreatedDate = tempTeam.CreatedDate;
                }
                else
                {
                    team.CreatedBy = currentLoggedIn.UserId;
                    team.CreatedDate = DateTime.Now.UTCCurrentTime();
                }
                result = _Util.Facade.UserLoginFacade.UpdateTeamSetting(team);
            }
            else
            {
                team.CreatedBy = currentLoggedIn.UserId;
                team.CreatedDate = DateTime.Now.UTCCurrentTime();
                result = _Util.Facade.UserLoginFacade.InsertTeamSetting(team);
            }

            return Json(result);
        }

        [Authorize]
        public ActionResult UserImport()
        {

            return View();
        }
        [Authorize]
        [HttpPost]
        public JsonResult UserImport(string File, string isCustomer, string PlatForm)
        {

            string subPath = "~/LeadImportReports"; // your code goes here

            bool exists = System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
            if (!exists)
                System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var serverFile = Server.MapPath(File);
            if (System.IO.File.Exists(serverFile))
            {
                FileInfo ExcelFile = new FileInfo(serverFile);
                if (ExcelFile.Length > 0)
                {
                    var workbook = new Excel.XLWorkbook(ExcelFile.FullName);
                    Excel.IXLWorksheet workSheet = workbook.Worksheet(1);
                    var xlRange = workSheet.RangeUsed();
                    int rowCount = xlRange.Rows().Count();
                    int colCount = xlRange.Cells().Count();
                    var LookUpList = _Util.Facade.LookupFacade.GetAllLookup();
                    List<Employee> employees = _Util.Facade.EmployeeFacade.GetAllEmployee();
                    for (int i = 2; i <= rowCount; i++)
                    {
                        string FirstName = string.Empty;
                        string LastName = string.Empty;
                        try
                        {
                            for (int j = 1; j <= colCount; j++)
                            {
                                if (xlRange.Cell(i, j) != null && xlRange.Cell(i, j).Value != null)
                                {
                                    try
                                    {
                                        #region Migration Conditions
                                        var value = xlRange.Cell(i, j).Value.ToString();
                                        var header = xlRange.Cell(1, j).Value.ToString();
                                        if (string.IsNullOrWhiteSpace(header) || string.IsNullOrWhiteSpace(value))
                                        {
                                            break;
                                        }
                                        else if (header == "Full Name")
                                        {
                                            var str = value.Replace("  ", " ").Split(' ');
                                            if (str.Count() > 0)
                                            {
                                                var index = 1;
                                                FirstName = str[0];
                                                for (; index < str.Count() - index; index++)
                                                {
                                                    FirstName += " " + str[index];
                                                }
                                                if (str.Count() > 1)
                                                {
                                                    LastName = str[index];
                                                }
                                                else
                                                {
                                                    LastName = "";
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                            continue;
                                        }
                                        #endregion
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }

                            }
                        }
                        catch (Exception)
                        {

                        }
                        #region Insert Data
                        Employee tempEmp = employees.Where(x => x.FirstName + x.LastName == FirstName + LastName).FirstOrDefault();
                        if (tempEmp != null)
                        {
                            continue;
                        }
                        UserLogin userLogin = new UserLogin()
                        {
                            UserName = "Dallas" + i,
                            UserId = Guid.NewGuid(),
                            Password = "123456",
                            EmailAddress = "",
                            IsActive = false,
                            IsDeleted = false,
                            LastUpdatedBy = User.Identity.Name,
                            LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                            CompanyId = CurrentUser.CompanyId.Value,
                        };
                        _Util.Facade.UserLoginFacade.InsertUserLogin(userLogin);

                        Employee employee = new Employee()
                        {
                            UserId = userLogin.UserId,
                            UserName = userLogin.UserName,
                            FirstName = FirstName,
                            LastName = LastName,
                            Email = userLogin.EmailAddress,
                            IsActive = false,
                            IsDeleted = false,
                            RecruitmentProcess = false,
                            Recruited = true,
                            IsCalendar = false,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            LastUpdatedBy = User.Identity.Name,
                            LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                            IsSupervisor = false,
                            CompanyId = CurrentUser.CompanyId.Value,
                        };
                        _Util.Facade.EmployeeFacade.InsertEmployee(employee);
                        UserOrganization userOrganization = new UserOrganization()
                        {
                            CompanyId = CurrentUser.CompanyId.Value,
                            UserName = userLogin.UserName,
                            IsActive = true,
                        };
                        _Util.Facade.UserOrganizationFacade.InsertUserOrganization(userOrganization);
                        UserPermission userPermission = new UserPermission()
                        {
                            CompanyId = CurrentUser.CompanyId.Value,
                            UserId = userLogin.UserId,
                            PermissionGroupId = 4
                        };
                        _Util.Facade.PermissionFacade.InsertUserPermission(userPermission);
                        UserCompany userCompany = new UserCompany()
                        {
                            CompanyId = CurrentUser.CompanyId.Value,
                            UserId = userLogin.UserId,
                            IsDefault = true
                        };
                        _Util.Facade.UserCompanyFacade.InsertUserCompany(userCompany);

                        employees.Add(employee);
                        #endregion
                    }
                }
            }

            return Json(1);
        }



        [Authorize]
        public PartialViewResult UserMgmtPartial()
        {
            if (!base.IsPermitted(UserPermissions.UserMgmtPermissions.UserList))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            return PartialView("_UserMgmt");
        }
        [Authorize]
        public PartialViewResult UserListPartial(int? UserGroup, string searchText, string currentemp, int? PageNo, int? PageSize, string Order)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (!base.IsPermitted(UserPermissions.UserMgmtPermissions.UserList))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (!PageNo.HasValue || PageNo.Value < 1)
            {
                PageNo = 1;
            }
            if (!PageSize.HasValue || PageSize.Value < 1)
            {
                PageSize = 50;
            }
            Guid userID = new Guid();
            UserMgmtListModel model = new UserMgmtListModel();
            Guid ComId = ((CustomPrincipal)(User)).CompanyId.Value;
            List<SelectListItem> empStatus = _Util.Facade.LookupFacade.GetDropdownsByKey("EmployeeStatus");
            ViewBag.EmployeeStatus = empStatus;
            if (empStatus != null && empStatus.Count > 0 && string.IsNullOrEmpty(currentemp))
            {
                ViewBag.EmployeeStatus = empStatus;
                currentemp = empStatus.Where(x => x.Selected == true).FirstOrDefault().Value;
            }

            //if (((List<SelectListItem>)ViewBag.EmployeeStatus).Count > 0 && currentemp==null && ((List<SelectListItem>)ViewBag.EmployeeStatus).Where(x => x.Selected).Select(x => x.Value)!=null)
            //{
            //    currentemp = ((List<SelectListItem>)ViewBag.EmployeeStatus).Where(x => x.Selected).Select(x => x.Value).FirstOrDefault();
            //}
            if (CurrentUser != null && CurrentUser.UserRole.ToLower() == "ieateryadmin")
            {
                userID = CurrentUser.UserId;
            }
            if (!string.IsNullOrWhiteSpace(currentemp))
            {
                model = _Util.Facade.UserLoginFacade.GetAllUserMgmtListByCompanyId(ComId, UserGroup, currentemp, null, PageNo.Value, PageSize.Value, searchText, userID, Order);
            }
            else
            {
                model = _Util.Facade.UserLoginFacade.GetAllUserMgmtListByCompanyId(ComId, UserGroup, "1", null, PageNo.Value, PageSize.Value, searchText, userID, Order);
            }
            ViewBag.PageNumber = PageNo;
            ViewBag.OutOfNumber = 0;
            if (model.UserMgmtList.Count() > 0)
            {
                ViewBag.OutOfNumber = model.TotalCount;
            }

            if ((int)ViewBag.PageNumber * PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / PageSize.Value);

            List<SelectListItem> UserGroupList = new List<SelectListItem>();
            UserGroupList.Add(new SelectListItem
            {
                Text = "Select Group",
                Value = "-1",
                Selected = UserGroup == -1
            });
            UserGroupList.AddRange(_Util.Facade.UserLoginFacade.GetAllPermissionGroup(ComId).OrderBy(x => x.Name).Select(x => new SelectListItem()
            {
                Text = string.Format("{0} ({1})", x.Name, x.UserCount),
                Value = x.Id.ToString(),
                Selected = UserGroup == x.Id
            }).ToList());
            UserGroupList.Add(new SelectListItem()
            {
                Text = "Deleted",
                Value = "404",
                Selected = UserGroup == 404
            });
            //ViewBag.LeadUserList = SalesList;
            ViewBag.UserGroupList = UserGroupList;


            if (Session[SessionKeys.EmployeeStatusFilter] != null && currentemp == null)
            {
                currentemp = Session[SessionKeys.EmployeeStatusFilter].ToString();
            }
            else if (currentemp == null)
            {
                currentemp = "1";
            }

            //string value = Session[SessionKeys.EmployeeStatusFilter].ToString();
            //Session[SessionKeys.EmployeeStatusFilter] = "1";
            ViewBag.currentemp = currentemp;
            return PartialView("_UserList", model);
        }

        [HttpPost]
        [Authorize]
        public JsonResult CurrentEmployeeSessionSet(string SessionVal)
        {
            if (SessionVal == null)
            {
                return Json(new { result = false, message = "" });
            }
            bool result = true;
            string message = "";
            var CurrentUser = ((CustomPrincipal)(User));
            Session[SessionKeys.EmployeeStatusFilter] = SessionVal;
            return Json(new { result = result, message = message });
        }




        [Authorize]
        public PartialViewResult AssignForms(int UserId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            UserLogin ul = _Util.Facade.UserLoginFacade.GetUserLoginById(UserId);
            if (ul == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (!_Util.Facade.UserCompanyFacade.UserIsInCompany(ul.UserId, CurrentUser.CompanyId.Value))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(ul.UserId);
            if (emp == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            List<RecruitmentForm> Model = _Util.Facade.RecruitFacade.GetAllRecruitmentFormsByCompanyId(CurrentUser.CompanyId.Value);
            List<RecruitmentFormEmployee> Rec = _Util.Facade.RecruitFacade.GetAllEmployeeRecruitmentFormsByEmpId(emp.UserId);
            if (Rec.Count() > 0)
            {
                foreach (var item in Model)
                {
                    if (Rec.Select(x => x.RecruitmentFormId).Contains(item.Id))
                    {
                        item.IsChecked = true;
                    }
                    else
                    {
                        item.IsChecked = false;
                    }
                }
            }
            ViewBag.EmployeeId = emp.UserId;


            return PartialView("_AssignForms", Model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AssignForms(Guid EmployeeId, List<int> Forms)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(EmployeeId);
            if (emp == null)
            {
                return Json(new { result = false, message = "Invalid request." });
            }
            if (!_Util.Facade.UserCompanyFacade.UserIsInCompany(emp.UserId, CurrentUser.CompanyId.Value))
            {
                return Json(new { result = false, message = "Access Denied." });
            }

            List<RecruitmentFormEmployee> Rec = _Util.Facade.RecruitFacade.GetAllEmployeeRecruitmentFormsByEmpId(emp.UserId);
            foreach (var item in Forms)
            {
                if (!Rec.Select(x => x.RecruitmentFormId).Contains(item))
                {
                    RecruitmentFormEmployee RFE = new RecruitmentFormEmployee()
                    {
                        EmployeeId = emp.UserId,
                        RecruitmentFormId = item,
                        IsFillUp = false,
                        IsSubmitted = false,
                    };
                    _Util.Facade.RecruitFacade.InsertRecruitmentFormEmployee(RFE);
                }
            }
            foreach (var item in Rec)
            {
                if (!Forms.Contains(item.RecruitmentFormId))
                {
                    RecruitmentFormEmployee TempRfe = _Util.Facade.RecruitFacade.GetRecruitmentFormEmployeeByRecruitmentFormIdAndEmpId(item.RecruitmentFormId, EmployeeId);
                    if (TempRfe != null)
                    {
                        _Util.Facade.RecruitFacade.DeleteRecruitmentDocFormByFormId(TempRfe.FormId);
                        _Util.Facade.RecruitFacade.DeleteRecruitmentI9FromByFormId(TempRfe.FormId);
                        _Util.Facade.RecruitFacade.DeleteRecruitmentW4FromByFormId(TempRfe.FormId);
                        _Util.Facade.RecruitFacade.DeleteRecruitmentW9FromByFormId(TempRfe.FormId);
                        //Need to delete hr doc for this..
                    }
                    _Util.Facade.RecruitFacade.DeleteRecruitmentFormEmployeeByEmployeeIdAndRecruitmentFormId(EmployeeId, item.RecruitmentFormId);
                }
            }


            return Json(new { result = true, message = "Forms assigned successfully." });
        }

        [Authorize]
        public ActionResult ActivateUser(int userid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            UserLogin uLogin = _Util.Facade.UserLoginFacade.GetById(userid);
            bool UserIsInCompany = _Util.Facade.UserCompanyFacade.UserIsInCompany(uLogin.UserId, CurrentUser.CompanyId.Value);
            if (!UserIsInCompany)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            UserProfile up = new UserProfile();
            up.UserLogin = uLogin;
            up.Employee = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(uLogin.UserId);
            return View("_ActivateUser", up);
            //return PartialView("~/Views/Shared/_AccessDenied.cshtml");
        }

        [Authorize]
        public ActionResult ActiveUserRecruit(int userid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            UserLogin uLogin = _Util.Facade.UserLoginFacade.GetById(userid);
            bool UserIsInCompany = _Util.Facade.UserCompanyFacade.UserIsInCompany(uLogin.UserId, CurrentUser.CompanyId.Value);
            if (!UserIsInCompany)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            UserProfile up = new UserProfile();
            up.UserLogin = uLogin;
            up.Employee = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(uLogin.UserId);
            return View("_ActivateUserRecruit", up);
        }

        [Authorize]
        [HttpPost]
        public JsonResult ActivateUser(int Id, string LastName, string FirstName, string UserName, string Password)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            UserLogin ul;
            if (Id < 1 || string.IsNullOrWhiteSpace(UserName))
            {
                return Json(new { result = false, message = "Error UL0001 Please Contact System admin" });
            }
            ul = _Util.Facade.UserLoginFacade.GetById(Id);
            if (ul == null || ul.UserName != UserName)
            {
                return Json(new { result = false, message = "Error UL002 Please Contact System admin" });
            }

            ul.EmailAddress = UserName;
            ul.Password = MD5Encryption.GetMD5HashData(Password);
            //ul.Id = Id;
            ul.IsDeleted = false;
            ul.IsActive = true;
            ul.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            ul.LastUpdatedBy = ul.UserName;
            bool result = _Util.Facade.UserLoginFacade.UpdateUserLogin(ul);
            if (!result)
            {
                return Json(new { result = false, message = "Error UL003 Please Contact System admin" });
            }
            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(ul.UserId);
            if (emp != null)
            {
                emp.FirstName = FirstName;
                emp.LastName = LastName;
                emp.LastUpdatedBy = UserName;
                emp.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                emp.IsActive = true;
                emp.IsDeleted = false;
                result = _Util.Facade.EmployeeFacade.UpdateEmployee(emp);
            }
            else
            {
                emp = new Employee()
                {
                    UserId = ul.UserId,
                    FirstName = FirstName,
                    LastName = LastName,
                    LastUpdatedBy = UserName,
                    IsActive = true,
                    IsDeleted = false,
                    UserName = UserName,
                    Email = UserName,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    CompanyId = CurrentUser.CompanyId.Value
                };
                result = _Util.Facade.EmployeeFacade.InsertEmployee(emp) > 0;
            }
            if (!result)
            {
                return Json(new { result = false, message = "Error UL004 Please Contact System admin" });
            }

            UserOrganization userOrganization = _Util.Facade.UserOrganizationFacade.GetOrganizationByUserName(ul.UserName, CurrentUser.CompanyId.Value);
            if (userOrganization == null)
            {
                userOrganization = new UserOrganization()
                {
                    CompanyId = CurrentUser.CompanyId.Value,
                    UserName = ul.UserName,
                    IsActive = false
                };

                List<UserOrganization> userOrganizations = _Util.Facade.UserOrganizationFacade.GetAllUserOrganizationsByUsername(ul.UserName);
                if (userOrganizations == null || userOrganizations.Count == 0)
                {
                    userOrganization.IsActive = true;
                }
                result = _Util.Facade.UserOrganizationFacade.InsertUserOrganization(userOrganization) > 0;
            }

            return Json(new { result = true, message = "User successfully activated." });
        }

        [Authorize]
        [HttpPost]
        public JsonResult ActivateUserRecruit(int Id, string LastName, string FirstName, string UserName, string Password, int empid, string olduser)
        {
            UserLogin ul;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (Id < 1 || string.IsNullOrWhiteSpace(UserName))
            {
                return Json(new { result = false, message = "Error UL0001 Please Contact System admin" });
            }
            ul = _Util.Facade.UserLoginFacade.GetById(Id);
            if (ul == null || ul.UserName != olduser)
            {
                return Json(new { result = false, message = "Error UL002 Please Contact System admin" });
            }
            ul.UserName = UserName;
            ul.EmailAddress = UserName;
            ul.Password = MD5Encryption.GetMD5HashData(Password);
            //ul.Id = Id;
            ul.IsDeleted = false;
            ul.IsActive = true;
            ul.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            ul.LastUpdatedBy = UserName;
            bool result = _Util.Facade.UserLoginFacade.UpdateUserLogin(ul);
            if (!result)
            {
                return Json(new { result = false, message = "Error UL003 Please Contact System admin" });
            }
            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeById(empid);
            if (emp != null)
            {
                emp.FirstName = FirstName;
                emp.LastName = LastName;
                emp.LastUpdatedBy = UserName;
                emp.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                emp.IsActive = true;
                emp.IsDeleted = false;
                emp.UserName = UserName;
                emp.Email = UserName;
                result = _Util.Facade.EmployeeFacade.UpdateEmployee(emp);
            }
            else
            {
                emp = new Employee()
                {
                    UserId = ul.UserId,
                    FirstName = FirstName,
                    LastName = LastName,
                    LastUpdatedBy = UserName,
                    IsActive = true,
                    IsDeleted = false,
                    UserName = UserName,
                    Email = UserName,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    CompanyId = CurrentUser.CompanyId.Value
                };
                result = _Util.Facade.EmployeeFacade.InsertEmployee(emp) > 0;
            }
            UserOrganization uo = _Util.Facade.UserOrganizationFacade.GetOrganizationByUserName(olduser, CurrentUser.CompanyId.Value);
            if (uo != null)
            {
                uo.UserName = UserName;
                _Util.Facade.UserOrganizationFacade.UpdateUserOrganiZation(uo);
            }
            if (!result)
            {
                return Json(new { result = false, message = "Error UL004 Please Contact System admin" });
            }

            return Json(new { result = true, message = "User successfully activated." });
        }
        [Authorize]
        public JsonResult DeactivateUser(int UserId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            UserLogin uLogin = _Util.Facade.UserLoginFacade.GetById(UserId);
            if (uLogin != null)
            {
                bool UserIsInCompany = _Util.Facade.UserCompanyFacade.UserIsInCompany(uLogin.UserId, CurrentUser.CompanyId.Value);
                if (!UserIsInCompany)
                {
                    return Json(new { result = false, message = "Permission denied." });
                }
                Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(uLogin.UserId);
                if (emp == null)
                {
                    return Json(new { result = false, message = "User not found." });
                }
                else
                {
                    bool IsPermission = false;
                    GlobalSetting settings = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey("DeleteUserToSystemUser");
                    if (settings != null && bool.TryParse(settings.Value, out IsPermission))
                    {

                    }
                    if (IsPermission)
                    {
                        emp.IsActive = false;
                        emp.LastUpdatedBy = User.Identity.Name;
                        emp.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                        _Util.Facade.EmployeeFacade.UpdateEmployee(emp);
                    }
                    else
                    {
                        TicketUser user = _Util.Facade.TicketFacade.GetAllAssignedTicketCountByUserId(emp.UserId);
                        if (user != null && user.MyTicketCount > 0)
                        {
                            return Json(new { result = false, message = "This employee has jobs in his schedule. You can not deactive this employee." });
                        }
                        else
                        {
                            emp.IsActive = false;
                            emp.LastUpdatedBy = User.Identity.Name;
                            emp.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                            _Util.Facade.EmployeeFacade.UpdateEmployee(emp);
                        }
                    }
                }
                uLogin.IsActive = false;
                _Util.Facade.UserLoginFacade.UpdateUserLogin(uLogin);

                List<UserOrganization> userOrganizations = _Util.Facade.UserOrganizationFacade.GetAllUserOrganizationsByUsername(uLogin.UserName);
                if (userOrganizations != null && userOrganizations.Count == 1)
                {
                    _Util.Facade.UserOrganizationFacade.DeleteUserOrganization(userOrganizations.FirstOrDefault());
                }
                else if (userOrganizations != null && userOrganizations.Count > 1)
                {
                    UserOrganization UO = userOrganizations.Where(x => x.CompanyId == CurrentUser.CompanyId.Value).FirstOrDefault();
                    if (UO != null)
                    {
                        _Util.Facade.UserOrganizationFacade.DeleteUserOrganization(UO);
                        if (UO.IsActive)
                        {
                            UO = userOrganizations.Where(x => x.CompanyId != CurrentUser.CompanyId.Value).FirstOrDefault();
                            UO.IsActive = true;
                            _Util.Facade.UserOrganizationFacade.UpdateUserOrganiZation(UO);
                        }
                    }
                    //else UserOrganization is already deleted
                }


                return Json(new { result = true, message = "User deactivated successfully." });

            }
            else
            {
                return Json(new { result = false, message = "User not found." });
            }
        }

        [Authorize]
        [HttpPost]
        public JsonResult IsUserExists(string email, int? Id)
        {
            bool result = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (Id.HasValue && Id.Value > 0)
            {
                result = !_Util.Facade.UserLoginFacade.IsRightUser(email, Id.Value, CurrentUser.CompanyId.Value);
            }
            else
            {
                result = _Util.Facade.UserLoginFacade.IsUserExists(email, CurrentUser.CompanyId.Value);
            }
            return Json(result);
        }
        [Authorize]
        [HttpPost]
        public JsonResult IsUserExistsGlobally(string CurrentUsername, string DesiredUsername)
        {
            return Json(IsUserExistsGloballyPrivate(CurrentUsername, DesiredUsername));
        }
        [Authorize]
        public PartialViewResult AddUser(int? Id, bool? IsRecruitUser)
        {
            if (!base.IsPermitted(UserPermissions.UserMgmtPermissions.AddUser))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            AddUser user = new AddUser();
            var CurrentLoggedInUser = (CustomPrincipal)User;
            if (Id.HasValue && Id.Value > 0)
            {
                user.UserLogin = _Util.Facade.UserLoginFacade.GetById(Id.Value);
                if (user.UserLogin == null)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                bool uc = _Util.Facade.UserCompanyFacade.UserIsInCompany(user.UserLogin.UserId, CurrentLoggedInUser.CompanyId.Value);
                if (!uc)
                {
                    return PartialView("~/Views/Shared/_AccessDenied.cshtml");
                }
                user.Employee = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(user.UserLogin.UserId);
                if (user.Employee == null)
                {
                    user.Employee = new Employee();
                }

                UserPermission up = _Util.Facade.PermissionFacade.GetUserPermissionGroupByUserId(user.UserLogin.UserId);
                if (up != null)
                {
                    user.PermissionGroup = _Util.Facade.PermissionFacade.GetPermissionGroupById(up.PermissionGroupId.Value);
                }
                else
                {
                    user.PermissionGroup = new PermissionGroup();
                }
                List<UserPermission> UserpermissionList = _Util.Facade.PermissionFacade.GetUserPermissionIdListByUserId(user.UserLogin.UserId);
                if (UserpermissionList.Count() > 0)
                {
                    List<int> Ids = UserpermissionList.Select(x => x.PermissionId.Value).ToList();
                    ViewBag.PermissionIdList = JsonConvert.SerializeObject(Ids, Formatting.None);

                    user.Permisssions = _Util.Facade.PermissionFacade.GetPermissionListByIdList(UserpermissionList.Select(x => x.PermissionId.Value).ToList());
                }
                else
                {
                    user.Permisssions = new List<Permission>();
                }
            }
            else
            {
                user.UserLogin = new UserLogin();
                user.PermissionGroup = new PermissionGroup();
                user.Permisssions = new List<Permission>();
                user.Employee = new Employee();

                List<SelectListItem> RouteList = new List<SelectListItem>();
                //RouteList.Add(new SelectListItem()
                //{
                //    Text = "Please Select One",
                //    Value = ""
                //});

                List<GeeseRoute> GeeseRouteList = _Util.Facade.CustomerFacade.GetRouteList();
                if (GeeseRouteList != null && GeeseRouteList.Count > 0)
                {
                    RouteList.AddRange(GeeseRouteList.OrderBy(x => x.Name).Select(x =>
                               new SelectListItem()
                               {
                                   Text = x.Name,
                                   Value = x.RouteId.ToString()
                               }).ToList());
                }

                ViewBag.RouteList = RouteList;
            }

            var CustomPermission = _Util.Facade.PermissionFacade.GetAllCustomPermission();
            user.CustomUserPermissionList = CustomPermission;
            var DistincRoles = CustomPermission.Select(x => x.RoleName).Distinct().ToList();
            ViewBag.DistincRoles = DistincRoles;

            if (IsRecruitUser.HasValue && IsRecruitUser.Value)
            {
                user.PermissionGroupList = _Util.Facade.PermissionFacade.GetAllRecruitPermissionGroupListByCompanyId(CurrentLoggedInUser.CompanyId.Value);
                ViewBag.BranchList = _Util.Facade.CompanyBranchFacade.GetAllCompanyBranchWithStateAndTimeZone(CurrentLoggedInUser.CompanyId.Value).OrderByDescending(x => x.Name.Contains("DFW")).ThenBy(x => x.Name).Select(x =>
                              new SelectListItem()
                              {
                                  Text = x.Name + " [ " + x.State + ", " + x.TimeZoneDisplayText + " ]",
                                  Value = x.Id.ToString()
                              }).ToList();
                return PartialView("_AddRecruitUser", user);
            }
            else
            {
                user.PermissionGroupList = _Util.Facade.PermissionFacade.GetAllPermissionGroupListByCompanyId(CurrentLoggedInUser.CompanyId.Value);
                ViewBag.BranchList = _Util.Facade.CompanyBranchFacade.GetAllCompanyBranchWithStateAndTimeZone(CurrentLoggedInUser.CompanyId.Value).OrderByDescending(x => x.Name.Contains("DFW")).ThenBy(x => x.Name).Select(x =>
                            new SelectListItem()
                            {
                                Text = x.Name + " [ " + x.State + ", " + x.TimeZoneDisplayText + " ]",
                                Value = x.Id.ToString()
                            }).ToList();

                return PartialView(user);
            }
            //ViewBag.PermissionList = _Util.Facade.PermissionFacade.GetAllPermissions();
            //ViewBag.InstallerPermissions = JsonConvert.SerializeObject(EmployeePermissions.InstallerPermissions);
            //ViewBag.SalesPersonPermissions = JsonConvert.SerializeObject(EmployeePermissions.SalesPersonPermissions);
            //ViewBag.ServiceCallPermissions = JsonConvert.SerializeObject(EmployeePermissions.ServiceCallPermissions);
        }

        public void EmployeeContact(Employee employee, string pgroup)
        {
            Contact contact = new Contact();
            if (!string.IsNullOrWhiteSpace(employee.Email) || !string.IsNullOrWhiteSpace(employee.Phone))
            {
                bool existEmail = _Util.Facade.ContactFacade.ExistEmailorCellNo(employee.Email, employee.Phone, null);
                UserContact uc = _Util.Facade.ContactFacade.GetUserContactsByCustomerId(employee.UserId);

                if (existEmail == false)
                {


                    contact.FirstName = employee.FirstName;
                    contact.LastName = employee.LastName;
                    contact.ContactId = Guid.NewGuid();

                    contact.CreatedDate = employee.CreatedDate.Value;
                    contact.Mobile = employee.Phone;
                    contact.Work = employee.Phone;
                    contact.Email = employee.Email;
                    contact.Notes = employee.Phone;

                    _Util.Facade.ContactFacade.InsertContacts(contact);
                    if (uc == null)
                    {
                        UserContact userContact = new UserContact();
                        if (pgroup == "Customer")
                        {
                            userContact.UserType = LabelHelper.UserType.Customer;
                        }
                        else
                        {
                            userContact.UserType = LabelHelper.UserType.Employee;
                        }

                        userContact.UserId = employee.UserId;
                        userContact.ContactId = contact.ContactId;
                        _Util.Facade.ContactFacade.InsertUserContacts(userContact);
                    }


                }
            }


        }
        [Authorize]
        [HttpPost]
        public JsonResult AddUser(int? Id, string fName, string lName, string title, string email, List<int> plist, string pGroup, bool IsServiceCall, bool IsInstaller, bool IsSalesPerson, int branchId, bool SendEmail, string RouteId)
        {
            if (pGroup == "Recruit" && string.IsNullOrWhiteSpace(email))
            {
                email = Guid.NewGuid().ToString();
            }
            if (!base.IsPermitted(UserPermissions.UserMgmtPermissions.AddUser))
            {
                return Json(new { result = false, message = "Error UM001 Permission Denied" });
            }

            bool Result = false;
            var CurrentLoggedInUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<string> RouteList = new List<string>();
            if (!string.IsNullOrWhiteSpace(RouteId))
            {
                string[] splituser = RouteId.Split(',');
                if (splituser.Length > 0)
                {
                    RouteId = string.Format("{0}", string.Join(",", splituser));
                    foreach (var item in splituser)
                    {
                        RouteList.Add(item);
                    }
                }
            }
            if (Id.HasValue && Id.Value > 0)
            {
                UserLogin ul = _Util.Facade.UserLoginFacade.GetById(Id.Value);
                if (ul == null || ul.UserName != email)
                {
                    return Json(new { result = false, message = "Error UM002 Invalid parameters" });
                }
                if (ul.UserName != email)
                {
                    return Json(new { result = false, message = "Error UM006 Invalid parameters" });
                }

                //ul.FirstName = fName;
                //ul.LastName = lName;
                ul.LastUpdatedBy = User.Identity.Name;
                ul.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                Result = _Util.Facade.UserLoginFacade.UpdateUserLogin(ul);
                if (!Result)
                {
                    return Json(new { result = false, message = "Error UM003 Internal Error contact system admin." });
                }

                Result = _Util.Facade.PermissionFacade.DeleteAllUserPermissionsByUserId(ul.UserId);
                if (!Result)
                {
                    return Json(new { result = false, message = "Error UM004 Internal Error contact system admin." });
                }
                Result = InsertUserPermissions(CurrentLoggedInUser.CompanyId.Value, CurrentLoggedInUser.UserId, pGroup, plist);
                if (!Result)
                {
                    return Json(new { result = false, message = "Error UM005 Internal Error contact system admin." });
                }

                Employee tempEmp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(ul.UserId);
                if (tempEmp != null)
                {
                    PermissionGroup pGr = _Util.Facade.PermissionFacade.GetPGroupByNameAndCompanyId(pGroup, CurrentLoggedInUser.CompanyId.Value);
                    if (pGr.Tag.ToLower().IndexOf(LabelHelper.UserTags.Recruit.ToLower()) > -1)
                    {
                        tempEmp.Recruited = false;
                        tempEmp.RecruitmentProcess = true;
                    }
                    else
                    {
                        tempEmp.Recruited = true;
                    }
                    tempEmp.FirstName = fName;
                    tempEmp.LastName = lName;
                    tempEmp.Email = email;
                    tempEmp.LastUpdatedBy = User.Identity.Name;
                    tempEmp.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    tempEmp.Title = title;
                    //tempEmp.IsServiceCall = IsServiceCall;
                    //tempEmp.IsSalesPerson = IsSalesPerson;
                    //tempEmp.IsInstaller = IsInstaller;
                    tempEmp.CreatedDate = DateTime.Now.UTCCurrentTime();

                    _Util.Facade.EmployeeFacade.UpdateEmployee(tempEmp);
                }
                else
                {
                    tempEmp = new Employee();

                    PermissionGroup pGr = _Util.Facade.PermissionFacade.GetPGroupByNameAndCompanyId(pGroup, CurrentLoggedInUser.CompanyId.Value);
                    if (pGr.Tag.ToLower().IndexOf(LabelHelper.UserTags.Recruit.ToLower()) > -1)
                    {
                        tempEmp.Recruited = false;
                        tempEmp.RecruitmentProcess = true;
                    }
                    else
                    {
                        tempEmp.Recruited = true;
                    }
                    tempEmp.FirstName = fName;
                    tempEmp.LastName = lName;
                    tempEmp.Email = email;
                    tempEmp.LastUpdatedBy = User.Identity.Name;
                    tempEmp.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                    tempEmp.Title = title;
                    tempEmp.IsActive = true;
                    tempEmp.UserName = email;
                    tempEmp.UserId = ul.UserId;
                    //tempEmp.IsServiceCall = IsServiceCall;
                    //tempEmp.IsSalesPerson = IsSalesPerson;
                    //tempEmp.IsInstaller = IsInstaller;
                    tempEmp.CreatedDate = DateTime.Now.UTCCurrentTime();
                    tempEmp.CompanyId = CurrentLoggedInUser.CompanyId.Value;
                    if (pGroup == "Recruit")
                    {
                        tempEmp.RecruitmentProcess = true;
                        tempEmp.Recruited = false;
                    }
                    _Util.Facade.EmployeeFacade.InsertEmployee(tempEmp);
                }

                if (RouteId.Count() > 0 && RouteId[0].ToString() != "null" && RouteId != null && RouteList[0] != "undefined")
                {
                    Result = _Util.Facade.CustomerFacade.DeleteEmployeeRouteByUserId(ul.UserId);
                    foreach (string item in RouteList)
                    {
                        EmployeeRoute ER = new EmployeeRoute()
                        {
                            RouteId = Guid.Parse(item),
                            UserId = ul.UserId,
                            CreatedBy = CurrentLoggedInUser.UserId,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            UpdatedBy = new Guid(),
                            UpdatedDate = DateTime.Now.UTCCurrentTime()
                        };
                        Result = _Util.Facade.CustomerFacade.InsertEmployeeRoute(ER) > 0;

                    }
                }

                return Json(new { result = true, message = "User has been saved and updated." });

            }
            else
            {
                UserLogin ul2 = new UserLogin();
                var logobj = _Util.Facade.UserLoginFacade.GetUserLoginByUserName(email);
                if (logobj != null)
                {
                    ul2 = new UserLogin()
                    {
                        //FirstName = fName,
                        //LastName = lName,
                        //Title = title,
                        UserId = Guid.NewGuid(),
                        UserName = email,
                        EmailAddress = email,
                        IsActive = true,
                        Password = logobj.Password,
                        LastUpdatedBy = User.Identity.Name,
                        LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                        CompanyId = CurrentLoggedInUser.CompanyId.Value
                    };
                }
                else
                {
                    ul2 = new UserLogin()
                    {
                        //FirstName = fName,
                        //LastName = lName,
                        //Title = title,
                        UserId = Guid.NewGuid(),
                        UserName = email,
                        EmailAddress = email,
                        IsActive = false,
                        LastUpdatedBy = User.Identity.Name,
                        LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                        CompanyId = CurrentLoggedInUser.CompanyId.Value
                    };
                }
                long ulId = _Util.Facade.UserLoginFacade.InsertUserLogin(ul2);
                Result = ulId > 0;
                if (!Result)
                {
                    return Json(new { result = false, message = "Internal Error contact system admin." });
                }
                Result = InsertUserPermissions(CurrentLoggedInUser.CompanyId.Value, ul2.UserId, pGroup, plist);
                if (!Result)
                {
                    string errorm = CurrentLoggedInUser.CompanyId.Value.ToString() + " == " + email + " == " + pGroup;
                    return Json(new { result = false, message = errorm + " == Internal Error contact system admin." });
                }

                UserCompany uc = new UserCompany()
                {
                    UserId = ul2.UserId,
                    CompanyId = CurrentLoggedInUser.CompanyId.Value,
                    IsDefault = true
                };
                _Util.Facade.UserCompanyFacade.InsertUserCompany(uc);

                UserOrganization uo = new UserOrganization()
                {
                    UserName = email,
                    CompanyId = CurrentLoggedInUser.CompanyId.Value,
                    IsActive = true
                };
                _Util.Facade.UserOrganizationFacade.InsertUserOrganization(uo);

                /*if (pGroup == LabelHelper.UserTypes.SysAdmin)
                {
                    IsInstaller = true;
                    IsSalesPerson = true;
                    IsServiceCall = true;
                }
                if (pGroup == LabelHelper.UserTypes.Admin)
                {
                    IsInstaller = true;
                    IsSalesPerson = true;
                    IsServiceCall = true;
                }
                if (pGroup == LabelHelper.UserTypes.SalesManager
                || pGroup != LabelHelper.UserTypes.SalesRep
                 || pGroup != LabelHelper.UserTypes.CertifiedAffiliate
                 || pGroup != LabelHelper.UserTypes.Regional)
                {
                    IsSalesPerson = true;

                }
                if (pGroup == LabelHelper.UserTypes.SalesPerson || pGroup != LabelHelper.UserTypes.SalesRep
                 || pGroup != LabelHelper.UserTypes.CertifiedAffiliate
                 || pGroup != LabelHelper.UserTypes.Regional)
                {
                    IsSalesPerson = true;
                }
                if (pGroup == LabelHelper.UserTypes.ServiceManager)
                {
                    IsServiceCall = true;
                }
                if (pGroup == LabelHelper.UserTypes.ServicePerson)
                {
                    IsServiceCall = true;
                }
                if (pGroup == LabelHelper.UserTypes.Installer)
                {
                    IsInstaller = true;
                }
                */
                Employee emp = new Employee()
                {
                    FirstName = fName,
                    UserId = ul2.UserId,
                    LastName = lName,
                    LastUpdatedBy = User.Identity.Name,
                    Title = title,
                    UserName = email,
                    Email = email,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    IsActive = true,
                    IsCurrentEmployee = true,
                    /*IsSalesPerson = IsSalesPerson,
                    IsInstaller = IsInstaller,
                    IsServiceCall = IsServiceCall,*/
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    CompanyId = CurrentLoggedInUser.CompanyId.Value
                };
                PermissionGroup pGr = _Util.Facade.PermissionFacade.GetPGroupByNameAndCompanyId(pGroup, CurrentLoggedInUser.CompanyId.Value);
                if (!string.IsNullOrWhiteSpace(pGr.Tag) && pGr.Tag.ToLower().IndexOf(LabelHelper.UserTags.Recruit.ToLower()) > -1)
                {
                    emp.Recruited = false;
                    emp.RecruitmentProcess = true;
                }
                else
                {
                    emp.Recruited = true;
                }
                Result = _Util.Facade.EmployeeFacade.InsertEmployee(emp) > 0;

                if (Result)
                {
                    UserBranch ub = new UserBranch()
                    {
                        BranchId = branchId,
                        CompanyId = CurrentLoggedInUser.CompanyId.Value,
                        UserId = ul2.UserId
                    };

                    _Util.Facade.EmployeeFacade.InsertUserBranch(ub);
                }
                if (!Result)
                {
                    return Json(new { result = false, message = "Error UM006 Internal Error contact system admin." });
                }
                if (RouteList != null && RouteList[0] != "null" && RouteList.Count > 0 && RouteList[0] != "undefined")
                {
                    Employee empInfo = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(ul2.UserId);
                    if (empInfo != null)
                    {
                        empInfo.IsCurrentEmployee = true;
                        _Util.Facade.EmployeeFacade.UpdateEmployee(empInfo);
                    }

                    foreach (string item in RouteList)
                    {
                        EmployeeRoute ER = new EmployeeRoute()
                        {
                            RouteId = Guid.Parse(item),
                            UserId = ul2.UserId,
                            CreatedBy = CurrentLoggedInUser.UserId,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            UpdatedBy = new Guid(),
                            UpdatedDate = DateTime.Now.UTCCurrentTime()
                        };
                        Result = _Util.Facade.CustomerFacade.InsertEmployeeRoute(ER) > 0;

                    }
                }
                if (SendEmail)
                {
                    string cryptmessage = DESEncryptionDecryption.EncryptPlainTextToCipherText(ulId + "__" + email + "__" + ul2.LastUpdatedDate.ToString() + "__" + CurrentLoggedInUser.CompanyId.Value);

                    //UtilHelper.GetCryptMessage(ulId + email + ul2.LastUpdatedDate.ToString());

                    VerifyEmail verifyEmail = new VerifyEmail();
                    verifyEmail.Name = string.Format("{0} {1}", fName, lName);
                    string SiteURL = ConfigurationManager.AppSettings["SiteURL"];
                    verifyEmail.EmailVerificationLink = AppConfig.SiteDomain + string.Format("/accountverification/{1}/{0}", ulId, cryptmessage);
                    verifyEmail.ToEmail = email;
                    Result = _Util.Facade.MailFacade.SendEmailVerify(verifyEmail, CurrentLoggedInUser.CompanyId.Value);
                }
                #region employee contact
                var PopulateContact = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentLoggedInUser.CompanyId.Value, "AutomaticPopulateContactData");
                if (PopulateContact != null && PopulateContact.Value.ToLower() == "true")
                {
                    EmployeeContact(emp, pGroup);
                }

                #endregion

                return Json(new { result = true, message = "New user has been created" });
            }
        }

        [Authorize]
        public JsonResult ResendVerificationEmail(int UserId)
        {
            UserLogin ul = _Util.Facade.UserLoginFacade.GetById(UserId);
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (ul == null)
            {
                return Json(new { result = false, errorCode = 101, message = "Error UM014 User not exsists." });
            }
            bool result = _Util.Facade.UserCompanyFacade.UserIsInCompany(ul.UserId, CurrentUser.CompanyId.Value);
            if (!result)
            {//can't sent mail to othe companies User
                return Json(new { result = false, errorCode = 101, message = "Error UM010 Please contact system admin" });
            }
            if (ul.IsDeleted == true)
            {
                return Json(new { result = false, errorCode = 101, message = "Error UM011 User is deleted." });
            }
            // if(ul.IsActive==false )
            //{
            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(ul.UserId);

            string cryptmessage = DESEncryptionDecryption.EncryptPlainTextToCipherText(ul.Id + "__" + ul.UserName + "__" + ul.LastUpdatedDate.ToString() + "__" + CurrentUser.CompanyId.Value);

            //UtilHelper.GetCryptMessage(ulId + email + ul2.LastUpdatedDate.ToString());

            VerifyEmail verifyEmail = new VerifyEmail();
            verifyEmail.Name = string.Format("{0} {1}", emp.FirstName, emp.LastName);
            string SiteURL = ConfigurationManager.AppSettings["SiteURL"];
            verifyEmail.EmailVerificationLink = AppConfig.SiteDomain + string.Format("/accountverification/{1}/{0}", ul.Id, cryptmessage);
            verifyEmail.ToEmail = emp.Email;
            result = _Util.Facade.MailFacade.SendEmailVerify(verifyEmail, CurrentUser.CompanyId.Value);
            if (result == true)
            {
                return Json(new { result = result, message = "Verification mail send successfully." });
            }
            return Json(new { result = result, message = "Verification mail send failed." });
            //}
            //return Json(new { result = false, errorCode = 101, message = "Error UM013 User is already active." });

        }

        [Authorize]
        public PartialViewResult UserInformation(int? Id)
        {
            AddUser up = new AddUser();
            up.UserLogin = new UserLogin();
            up.Permisssions = new List<Permission>();
            up.PermissionGroup = new PermissionGroup();
            up.Employee = new Employee();
            up.employeeLeadSources = new List<EmployeeLeadSource>();

            var CurrentUserLoggedIn = (HS.Web.UI.Helper.CustomPrincipal)User;
            UserLogin UserLogIn = _Util.Facade.UserLoginFacade.GetByIdAndCompanyId(Id.Value, CurrentUserLoggedIn.CompanyId.Value);
            if (UserLogIn != null)
            {
                Employee Emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(UserLogIn.UserId);
                if (Emp != null)
                {
                    var Role = _Util.Facade.EmployeeFacade.GetEmployeeRoleByEmployeeIdAndCompanyId(Emp.UserId, Emp.CompanyId);
                    ViewBag.EmployeeRole = Role.Name;
                }
            }
            if (Id.HasValue)
            {
                up.UserLogin = _Util.Facade.UserLoginFacade.GetByIdAndCompanyId(Id.Value, CurrentUserLoggedIn.CompanyId.Value);
                //up.Employee = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(up.UserLogin.UserId);

                up.Employee = _Util.Facade.EmployeeFacade.GetNewEmployeeByuserIdandCompanyId(up.UserLogin.UserId);
                up.Employee.Phone = up.Employee.Phone.FormatedPhoneNum();
                up.UserBranchDetails = _Util.Facade.EmployeeFacade.GetUserBranchByCompanyIdAndUserId(CurrentUserLoggedIn.CompanyId.Value, up.UserLogin.UserId);
                up.PermissionGroup = _Util.Facade.PermissionFacade.GetPermissionGroupByUserId(up.UserLogin.UserId);
                up.Permisssions = _Util.Facade.PermissionFacade.GetAllPermissionsByUserIdAndCompanyId(up.UserLogin.UserId, CurrentUserLoggedIn.CompanyId.Value);
                //up.Permisssions = _Util.Facade.PermissionFacade.GetPermissionListByUsername(up.UserLogin.UserName);
                up.employeeLeadSources = _Util.Facade.EmployeeFacade.GetEmployeeLeadSourceByEmployeeId(up.UserLogin.UserId);
                up.RouteList = _Util.Facade.CustomerFacade.GetAllEmployeeRouteByUserId(up.UserLogin.UserId);
            }

            var PermissionGroupList = _Util.Facade.PermissionFacade.GetAllPermissionGroupListByCompanyId(CurrentUserLoggedIn.CompanyId.Value);
            ViewBag.PermissionGroupList = PermissionGroupList.OrderBy(x => x.Name).Select(x =>
                new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();


            List<SelectListItem> SupervisorList = new List<SelectListItem>();
            SupervisorList.Add(new SelectListItem()
            {
                Text = "Supervisors",
                Value = "-1"
            });

            SupervisorList.AddRange(_Util.Facade.EmployeeFacade.GetAllSupervisors().OrderBy(x => x.FirstName != "Supervisors").ThenBy(x => x.FirstName).Where(x => x.UserId != up.Employee.UserId).Select(x => new SelectListItem()
            {
                Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                Value = x.UserId.ToString()
            }).ToList());
            //ViewBag.LeadUserList = SalesList;
            ViewBag.SupervisorList = SupervisorList;
            List<SelectListItem> RouteList = new List<SelectListItem>();
            //RouteList.Add(new SelectListItem()
            //{
            //    Text = "Please Select One",
            //    Value = ""
            //});

            List<GeeseRoute> GeeseRouteList = _Util.Facade.CustomerFacade.GetRouteList();
            if (GeeseRouteList != null && GeeseRouteList.Count > 0)
            {
                RouteList.AddRange(GeeseRouteList.OrderBy(x => x.Name).Select(x =>
                           new SelectListItem()
                           {
                               Text = x.Name,
                               Value = x.RouteId.ToString(),
                               Selected = (up.RouteList.Where(y => y.RouteId == x.RouteId).Count() > 0)
                           }).ToList());
            }

            ViewBag.RouteList = RouteList;


            var cityListForDropdown = _Util.Facade.LookupFacade.GetLookupByKey("USACitiesList").ToList();
            ViewBag.cityListForDropdown = cityListForDropdown;

            ViewBag.CityList = _Util.Facade.LookupFacade.GetLookupByKey("USACitiesList").Select(x =>
                        new SelectListItem()
                        {
                            Text = x.DisplayText.ToString(),
                            Value = x.DataValue.ToString(),

                        }).ToList();

            ViewBag.StateList = _Util.Facade.LookupFacade.GetLookupByKey("StateList").Select(x =>
                        new SelectListItem()
                        {
                            Text = x.DisplayText.ToString(),
                            Value = x.DataValue.ToString()
                        }).ToList();

            ViewBag.TechCommission = _Util.Facade.LookupFacade.GetLookupByKey("TechComission").Select(x =>
                        new SelectListItem()
                        {
                            Text = x.DisplayText.ToString(),
                            Value = x.DataValue.ToString()
                        }).ToList();

            ViewBag.BranchList = _Util.Facade.CompanyBranchFacade.GetAllCompanyBranchWithStateAndTimeZone(CurrentUserLoggedIn.CompanyId.Value).Select(x =>
                           new SelectListItem()
                           {
                               Text = x.Name + " [ " + x.State + ", " + x.TimeZoneDisplayText + " ]",
                               Value = x.Id.ToString()
                           }).ToList();
            List<SelectListItem> UserList = _Util.Facade.EmployeeFacade.GetAllTimeClockSupervisorListByUserId(CurrentUserLoggedIn.UserId).OrderBy(x => x.FirstName + ' ' + x.LastName).Select(x => new SelectListItem()
            {
                Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                Value = x.UserId.ToString()
            }).ToList();
            UserList.Insert(0, new SelectListItem()
            {
                Text = "All",
                Value = new Guid().ToString()
            });
            ViewBag.UserList = UserList;
            var timeclocksupervisor = _Util.Facade.EmployeeFacade.GetTimeClockSupervisorListByUserId(up.Employee.UserId);
            ViewBag.timeclocksupervisor = timeclocksupervisor.Select(x => x.SupervisorId.ToString()).ToList();


            #region Lead Sources 
            ViewBag.LeadSourcesList = _Util.Facade.LookupFacade.GetLookupByKeyWithParent("LeadSource").Where(x => x.DataValue != "-1")
                .OrderBy(x => x.DisplayText).Select(x =>
                          new SelectListItem()
                          {
                              Text = x.DisplayText.ToString(),
                              Value = x.DataValue.ToString(),
                              Selected = up.employeeLeadSources.Where(y => y.LeadSource == x.DataValue).Count() > 0
                          }).ToList();

            #endregion

            return PartialView("_UserInformation", up);
        }
        [Authorize]
        [HttpPost]
        public JsonResult SaveUserInfo(SaveUserInfoModel Model)
        {
            if(string.IsNullOrWhiteSpace(Model.PasswordUpdateDays.ToString()) || Model.PasswordUpdateDays == 0)
            {
                Model.PasswordUpdateDays = 90;
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            #region Validations
            int logId = 0;
            if (CurrentUser.UserTags.ToLower().IndexOf("admin") < 0)
            {
                return Json(new { result = false, Logout = false, message = "You do not have enough permission." });
            }

            bool result = false;
            bool ChangeUsername = false;
            bool Logout = false;
            Guid empuserid = new Guid();
            string OldUsername = "";
            var empInfo = _Util.Facade.EmployeeFacade.GetEmployeeById(Model.empid);
            if (empInfo == null)
            {
                return Json(new { result = false, message = "User not found.", Logout = Logout });
            }
            result = _Util.Facade.UserOrganizationFacade.UserIsInCompany(User.Identity.Name, CurrentUser.CompanyId.Value);
            if (!result)
            {
                return Json(new { result = false, Logout = Logout, message = "You can not change another companies employees info." });
            }
            #endregion

            #region UserDatailBox
            DateTime HireDate = new DateTime();
            if (PermissionHelper.IsPermitted(FrameWorkUserPermission.UserMgmtPermissions.HireDate))
            {
                HireDate = empInfo.HireDate.HasValue ? empInfo.HireDate.Value : new DateTime();
                DateTime.TryParse(Model.hire, out HireDate);
            }
            else
            {
                HireDate = empInfo.HireDate.HasValue ? empInfo.HireDate.Value : new DateTime();

            }
            string UserLicenseNo = "";
            if (PermissionHelper.IsPermitted(FrameWorkUserPermission.UserMgmtPermissions.LicenseNo))
            {
                UserLicenseNo = Model.LicenseNo;
            }
            else
            {
                UserLicenseNo = empInfo.LicenseNo;
            }
            string UserJob = "";
            if (PermissionHelper.IsPermitted(FrameWorkUserPermission.UserMgmtPermissions.JobTitle))
            {
                UserJob = Model.job;
            }
            else
            {
                UserJob = empInfo.JobTitle;
            }
            //string UserSaleCommission = "";
            //if (PermissionHelper.IsPermitted(FrameWorkUserPermission.UserMgmtPermissions.SalesCommission))
            //{
            //    UserSaleCommission = Model.sales;
            //}
            //else
            //{
            //    UserSaleCommission = empInfo.SalesCommissionStructure;
            //}
            string UserTechCommission = "";
            if (PermissionHelper.IsPermitted(FrameWorkUserPermission.UserMgmtPermissions.TechCommission))
            {
                UserTechCommission = Model.tech;
            }
            else
            {
                UserTechCommission = empInfo.TechCommissionStructure;
            }

            string UserSeason = "";
            if (PermissionHelper.IsPermitted(FrameWorkUserPermission.UserMgmtPermissions.Season))
            {
                UserSeason = Model.session;
            }
            else
            {
                UserSeason = empInfo.Session;
            }

            string UserSupervisor = "";
            bool isSuppervisor = false;
            if (PermissionHelper.IsPermitted(FrameWorkUserPermission.UserMgmtPermissions.SuperVisor))
            {
                UserSupervisor = Model.SuperVisorId;
                isSuppervisor = Model.IsSupervisor;
            }
            else
            {
                UserSupervisor = empInfo.SuperVisorId;
                isSuppervisor = empInfo.IsSupervisor.Value;
            }
            #endregion

            #region Information Box
            string FirstName = "";
            string LastName = "";
            string UserName = "";
            string Email = "";
            string Phone = "";
            string SSN = "";
            string ZipCode = "";
            string Street = "";
            string City = "";
            string State = "";
            string ZipCode2 = "";
            string Street2 = "";
            string City2 = "";
            string State2 = "";
            string StreetPrevious = "";
            string DelearUserName = "";
            string DelearPassword = "";
            if (PermissionHelper.IsPermitted(FrameWorkUserPermission.UserMgmtPermissions.InformationBox))
            {
                FirstName = Model.fnum;
                LastName = Model.lnum;
                UserName = Model.username;
                Email = Model.email;
                Phone = Model.phn;
                SSN = Model.ssn;
                ZipCode = Model.zip;
                Street = Model.street;
                City = Model.city;
                State = Model.state;
                ZipCode2 = Model.zip2;
                Street2 = Model.street2;
                City2 = Model.city2;
                State2 = Model.state2;
                StreetPrevious = Model.StreetPrevious;
            }
            else
            {
                FirstName = empInfo.FirstName;
                LastName = empInfo.LastName;
                UserName = empInfo.UserName;
                Email = empInfo.Email;
                Phone = empInfo.Phone;
                SSN = empInfo.SSN;
                ZipCode = empInfo.ZipCode;
                Street = empInfo.Street;
                City = empInfo.City;
                State = empInfo.State;
                ZipCode2 = empInfo.ZipCode2;
                Street2 = empInfo.Street2;
                City2 = empInfo.City2;
                State2 = empInfo.State2;
                StreetPrevious = empInfo.StreetPrevious;
            }
            #endregion

            #region CalendarBox
            bool IsCalendar = false;
            string Colour = "";
            if (PermissionHelper.IsPermitted(FrameWorkUserPermission.UserMgmtPermissions.CalanderBox))
            {
                IsCalendar = Model.iscalendar;
                Colour = Model.color;

            }
            else
            {
                IsCalendar = empInfo.IsCalendar.Value;
                Colour = empInfo.CalendarColor;
            }
            #endregion

            #region Brinks Delear Credentials

            if (PermissionHelper.IsPermitted(FrameWorkUserPermission.UserMgmtPermissions.BrinksDelearCredential))
            {
                DelearUserName = Model.BrinksDelearUser;
                DelearPassword = Model.BrinksDelearPassword;

            }
            else
            {
                DelearUserName = empInfo.BrinksDealerUser;
                DelearPassword = empInfo.BrinksDealerPassword;
            }
            #endregion
            #region IpListBox
            string UserClockInIp = "";
            if (PermissionHelper.IsPermitted(FrameWorkUserPermission.UserMgmtPermissions.IPListBox))
            {
                UserClockInIp = Model.ClockInIp;
            }
            else
            {
                UserClockInIp = empInfo.ClockInIP;

            }
            #endregion


            #region BranchBox
            int UserBranchId = 0;
            if (PermissionHelper.IsPermitted(FrameWorkUserPermission.UserMgmtPermissions.BranchBox))
            {
                UserBranchId = Model.BranchId.Value;
            }
            else
            {
                UserBranchId = Model.BranchId.Value;

            }
            #endregion

            #region ExpirationDate Box
            DateTime InstallLicense = new DateTime();
            DateTime FireLicense = new DateTime();
            DateTime DriverLicense = new DateTime();
            DateTime SelesLicense = new DateTime();
            if (PermissionHelper.IsPermitted(FrameWorkUserPermission.UserMgmtPermissions.ExpirationDateBox))
            {
                InstallLicense = empInfo.InstallLicenseExpirationDate.HasValue ? empInfo.InstallLicenseExpirationDate.Value : new DateTime();
                DateTime.TryParse(Model.InstallLicenseExpirationDate, out InstallLicense);
                FireLicense = empInfo.FireLicenseExpirationDate.HasValue ? empInfo.FireLicenseExpirationDate.Value : new DateTime();
                DateTime.TryParse(Model.FireLicenseExpirationDate, out FireLicense);
                DriverLicense = empInfo.DriversLicenseExpirationDate.HasValue ? empInfo.DriversLicenseExpirationDate.Value : new DateTime();
                DateTime.TryParse(Model.DriversLicenseExpirationDate, out DriverLicense);

                SelesLicense = empInfo.SalesLicenseExpirationDate.HasValue ? empInfo.SalesLicenseExpirationDate.Value : new DateTime();
                DateTime.TryParse(Model.SalesLicenseExpirationDate, out SelesLicense);
            }
            else
            {
                InstallLicense = empInfo.InstallLicenseExpirationDate.HasValue ? empInfo.InstallLicenseExpirationDate.Value : new DateTime();
                FireLicense = empInfo.FireLicenseExpirationDate.HasValue ? empInfo.FireLicenseExpirationDate.Value : new DateTime();
                DriverLicense = empInfo.DriversLicenseExpirationDate.HasValue ? empInfo.DriversLicenseExpirationDate.Value : new DateTime();
                SelesLicense = empInfo.SalesLicenseExpirationDate.HasValue ? empInfo.SalesLicenseExpirationDate.Value : new DateTime();
            }
            #endregion

            #region Supervisor
            Guid SupervisorGId = new Guid();
            Guid.TryParse(Model.SuperVisorId, out SupervisorGId);
            if (SupervisorGId == new Guid() || SupervisorGId == null)
            {
                Model.SuperVisorId = string.Empty;
            }
            #endregion

            #region Employee table update
            if (empInfo != null)
            {
                OldUsername = empInfo.UserName;
                empInfo.FirstName = FirstName;
                empInfo.LastName = LastName;
                empInfo.Email = Email;
                empInfo.Street = Street;
                empInfo.City = City;
                empInfo.State = State;
                empInfo.ZipCode = ZipCode;
                empInfo.Phone = Phone;
                empInfo.SSN = SSN;

                empInfo.Street2 = Street2;
                empInfo.City2 = City2;
                empInfo.State2 = State2;
                empInfo.ZipCode2 = ZipCode2;
                empInfo.StreetPrevious = StreetPrevious;
                empInfo.BrinksDealerPassword = DelearPassword;
                empInfo.BrinksDealerUser = DelearUserName;

                empInfo.HireDate = HireDate;
                empInfo.InstallLicenseExpirationDate = InstallLicense;
                empInfo.FireLicenseExpirationDate = FireLicense;
                empInfo.DriversLicenseExpirationDate = DriverLicense;
                empInfo.SalesLicenseExpirationDate = SelesLicense;

                empInfo.NoAutoClockOut = Model.NoAutoClockout;
                empInfo.IsSupervisor = isSuppervisor;
                empInfo.SuperVisorId = UserSupervisor;
                empInfo.ClockInIP = UserClockInIp;

                empInfo.PlaceOfBirth = Model.place;
                empInfo.JobTitle = UserJob;
                empInfo.Session = UserSeason;
                //empInfo.SalesCommissionStructure = UserSaleCommission;
                empInfo.TechCommissionStructure = UserTechCommission;
                empInfo.LastUpdatedBy = User.Identity.Name;
                empInfo.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                empInfo.IsCalendar = IsCalendar;
                empInfo.CalendarColor = Colour;
                empInfo.IsPayroll = Model.ispayroll;
                empInfo.IsSalesMatrix = Model.IsSalesMatrix;
                empInfo.LicenseNo = UserLicenseNo;
                empInfo.AlarmId = Model.AlarmId;
                empInfo.BadgerUserId = Model.BadgerUserId;
                empInfo.IsCurrentEmployee = Model.currentemp;

                if (!string.IsNullOrWhiteSpace(Model.username) && empInfo.UserName != Model.username.Trim())
                {
                    ChangeUsername = true;
                    empuserid = empInfo.UserId;
                    if (IsUserExistsGloballyPrivate(OldUsername, Model.username))
                    {
                        return Json(new { result = false, Logout = Logout, message = "Username already exists.", userlogIntId = logId });
                    }

                }
                if (empInfo.RecruitmentProcess == true)
                {
                    empInfo.Status = Model.Status;
                }
                if (Model.Status == "Signed" || Model.Status == "Dropped")
                {
                    empInfo.Recruited = true;
                }
                empInfo.PasswordUpdateDays = Model.PasswordUpdateDays;
                result = _Util.Facade.EmployeeFacade.UpdateEmployee(empInfo);
               // DateTime? resetDate = uLogin.PasswordResetDate.Value.AddDays((int)empdata.PasswordUpdateDays);
                UserLogin uLogin = _Util.Facade.UserLoginFacade.GetUserLoginByUserId(empInfo.UserId);
                if (uLogin != null && uLogin.PasswordResetDate.HasValue)
                {
                    uLogin.PasswordResetDate = uLogin.PasswordResetDate.Value.AddDays((int)empInfo.PasswordUpdateDays);
                    _Util.Facade.UserLoginFacade.UpdateUserLogin(uLogin);
                }
                else
                {
                    uLogin.PasswordResetDate = DateTime.Now.AddDays((int)empInfo.PasswordUpdateDays);
                    _Util.Facade.UserLoginFacade.UpdateUserLogin(uLogin);
                }

                var UserBranchDetails = _Util.Facade.EmployeeFacade.GetUserBranchByCompanyIdAndUserId(CurrentUser.CompanyId.Value, empInfo.UserId);
                if (UserBranchDetails != null & UserBranchId > 0)
                {
                    UserBranchDetails.BranchId = UserBranchId;
                    _Util.Facade.EmployeeFacade.UpdateUserBranch(UserBranchDetails);
                }
                logId = _Util.Facade.UserLoginFacade.GetUserByUsername(empInfo.UserName).Id;
            }
            #endregion

            #region Employee Lead Source
            if (base.IsPermitted(UserPermissions.UserMgmtPermissions.LeadSourcesBox))
            {
                try
                {
                    _Util.Facade.EmployeeFacade.DeleteEmployeeLeadSourceByUserId(empInfo.UserId);
                    if (Model.LeadSources != null)
                    {
                        foreach (var item in Model.LeadSources)
                        {
                            EmployeeLeadSource el = new EmployeeLeadSource()
                            {
                                EmployeeId = empInfo.UserId,
                                LeadSource = item
                            };
                            _Util.Facade.EmployeeFacade.InsertEmployeeLeadSource(el);
                        }
                    }
                }
                catch (Exception) { }
            }
            #endregion


            #region Employee Route
            if (!string.IsNullOrWhiteSpace(Model.RouteList) && Model.RouteList != "undefined")
            {
                List<string> RouteId = new List<string>();
                string[] splituser = Model.RouteList.Split(',');
                if (splituser.Length > 0)
                {
                    Model.RouteList = string.Format("{0}", string.Join(",", splituser));
                    foreach (var item in splituser)
                    {

                        RouteId.Add(item);
                    }
                }
                if (RouteId.Count() > 0 && RouteId[0] != "null" & RouteId != null)
                {
                    Employee Emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(empInfo.UserId);
                    if (Emp != null)
                    {
                        Emp.IsCurrentEmployee = true;
                        _Util.Facade.EmployeeFacade.UpdateEmployee(Emp);
                    }
                    _Util.Facade.CustomerFacade.DeleteEmployeeRouteByUserId(empInfo.UserId);
                    foreach (string item in RouteId)
                    {
                        EmployeeRoute empRoute = new EmployeeRoute()
                        {
                            RouteId = Guid.Parse(item),
                            UserId = empInfo.UserId,
                            CreatedBy = CurrentUser.UserId,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            UpdatedBy = CurrentUser.UserId,
                            UpdatedDate = DateTime.Now.UTCCurrentTime()
                        };
                        _Util.Facade.CustomerFacade.InsertEmployeeRoute(empRoute);
                    }
                }
            }
            #endregion

            #region 
            if (result)
            {
                #region Username/Password Change
                if ((!string.IsNullOrWhiteSpace(Model.password) || ChangeUsername))
                {
                    List<Organization> orgList = _Util.Facade.UserOrganizationFacade.GetAllOrganizationsByUsername(empInfo.UserName);
                    string prevConStr = Session[SessionKeys.CompanyConnectionString].ToString();
                    if (orgList == null || orgList.Count == 0)
                    {
                        _Util.Facade.UserOrganizationFacade.InsertUserOrganization(new UserOrganization()
                        {
                            CompanyId = CurrentUser.CompanyId.Value,
                            UserName = Model.username.Trim(),
                            IsActive = true
                        });
                       orgList = _Util.Facade.UserOrganizationFacade.GetAllOrganizationsByUsername(Model.username.Trim());
                    }

                    foreach (var item in orgList)
                    {
                        try
                        {
                            Session[SessionKeys.CompanyConnectionString] = item.ConnectionString;
                            UserLogin ul = _Util.Facade.UserLoginFacade.GetUserByUsernameAndCompanyId(OldUsername, item.CompanyId);
                            if (ul != null)
                            {
                                if (!string.IsNullOrWhiteSpace(Model.password) && ul.Password != MD5Encryption.GetMD5HashData(Model.password.Trim()))
                                {
                                    ul.Password = MD5Encryption.GetMD5HashData(Model.password.Trim());
                                    ul.LastUpdatedBy = CurrentUser.UserId.ToString();
                                    ul.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                                }
                                if (ChangeUsername)
                                {
                                    Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUsernameAndCompanyId(OldUsername, true, item.CompanyId);
                                    if (emp != null && empuserid == emp.UserId)
                                    {
                                        ul.UserName = Model.username.Trim();
                                        ul.LastUpdatedBy = CurrentUser.UserId.ToString();
                                        ul.LastUpdatedDate = DateTime.Now.UTCCurrentTime();

                                        emp.UserName = Model.username.Trim();
                                        emp.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                                        emp.LastUpdatedBy = User.Identity.Name;

                                        _Util.Facade.EmployeeFacade.UpdateEmployee(emp);
                                    }
                                }
                                _Util.Facade.UserLoginFacade.UpdateUserLogin(ul);
                            }
                        }
                        catch (Exception)
                        {
                            Session[SessionKeys.CompanyConnectionString] = prevConStr;
                        }
                    }
                    Session[SessionKeys.CompanyConnectionString] = prevConStr;
                    
                    if (ChangeUsername)
                    {
                        List<UserOrganization> uoLst = _Util.Facade.UserOrganizationFacade.GetUserOrganizationListByUserNameAndCompanyId(OldUsername, CurrentUser.CompanyId.Value);
                        foreach (var item in uoLst)
                        {
                            item.UserName = Model.username.Trim();
                            if (uoLst.Count == 1 && item.IsActive == false)
                            {
                                item.IsActive = true;
                            }
                            _Util.Facade.UserOrganizationFacade.UpdateUserOrganiZation(item);
                        }
                        if (OldUsername == User.Identity.Name)
                        {
                            Logout = true;
                            FormsAuthentication.SignOut();
                            base.AddPageLoadUserActivity("Logout " + LabelHelper.ActivityAction.Success);
                            SessionHelper hs = new SessionHelper();
                            hs.ClearCurrentSession();
                        }
                    }
                }
                #endregion

                #region Email address Update
                if (!string.IsNullOrWhiteSpace(Model.email))
                {
                    var userlogobj = _Util.Facade.UserLoginFacade.GetUserLoginByUserId(empInfo.UserId);
                    if (userlogobj != null && Model.email != userlogobj.EmailAddress)
                    {
                        userlogobj.EmailAddress = Model.email;
                        _Util.Facade.UserLoginFacade.UpdateUserLogin(userlogobj);
                    }
                }
                #endregion

                #region timeclock supervisors
                if (Model.employeetimeclocksupervisor != null && Model.employeetimeclocksupervisor.Count > 0)
                {
                    bool delres = false;
                    if (Model.employeetimeclocksupervisor.Count > 0)
                    {
                        if (Model.employeetimeclocksupervisor[0] == "All")
                        {
                            Model.employeetimeclocksupervisor[0] = new Guid().ToString();
                        }
                        var timeclocksupervisorlist = _Util.Facade.EmployeeFacade.GetTimeClockSupervisorListByUserId(empInfo.UserId);
                        if (timeclocksupervisorlist != null && timeclocksupervisorlist.Count > 0)
                        {
                            delres = _Util.Facade.EmployeeFacade.DeleteAllTimeClockSupervisor(empInfo.UserId);
                        }
                        foreach (var item in Model.employeetimeclocksupervisor)
                        {
                            EmployeeTimeClockSupervisor model = new EmployeeTimeClockSupervisor()
                            {
                                UserId = empInfo.UserId,
                                SupervisorId = new Guid(item),
                                CreatedBy = CurrentUser.UserId,
                                CreatedDate = DateTime.Now.UTCCurrentTime()
                            };
                            _Util.Facade.EmployeeFacade.InsertTimeClockSupervisor(model);
                        }
                    }
                }
                #endregion


                return Json(new { result = true, Logout = Logout, message = "User information successfully updated.", userlogIntId = logId });
            }
            #endregion



            return Json(new { result = false, Logout = Logout, message = "Internal error occured. Please contact system admin.", userlogIntId = logId });
        }

        [HttpPost]
        [Authorize]
        public JsonResult DeleteUser(int? id)
        {
            if (!id.HasValue)
            {
                return Json(new { result = false, message = "Error UM002 Please Contact System admin" });
            }
            bool result = false;
            string message = "";
            var CurrentUser = ((CustomPrincipal)(User));
            if (id.HasValue && id > 0)
            {
                UserLogin ul = _Util.Facade.UserLoginFacade.GetById(id.Value);
                result = _Util.Facade.UserCompanyFacade.UserIsInCompany(ul.UserId, CurrentUser.CompanyId.Value);
                if (!result)
                {//can't delete other companys User
                    return Json(new { result = false, errorCode = 101, message = "Error UM003 Please Contact System admin" });
                }

                if (!(CurrentUser.UserRole == LabelHelper.UserTypes.Admin || CurrentUser.UserRole == LabelHelper.UserTypes.SysAdmin))
                {
                    return Json(new { result = false, message = "Error UM004 you do not have permission to delete this user." });
                }

                Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(ul.UserId);
                if (emp != null)
                {
                    bool IsPermission = false;
                    GlobalSetting settings = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey("DeleteUserToSystemUser");
                    if (settings != null && bool.TryParse(settings.Value, out IsPermission))
                    {

                    }
                    if (IsPermission)
                    {
                        // Update login user
                        ul.IsDeleted = true;
                        ul.IsActive = false;
                        _Util.Facade.UserLoginFacade.UpdateUserLogin(ul);

                        // Update employee
                        emp.IsDeleted = true;
                        emp.IsActive = false;
                        emp.LastUpdatedBy = User.Identity.Name;
                        emp.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                        _Util.Facade.EmployeeFacade.UpdateEmployee(emp);

                        // Update user to system user
                        _Util.Facade.TicketFacade.UpdateTicketUserToSystemUserById(ul.UserId);

                        result = true;
                        message = "User has been deleted successfully.";
                    }
                    else
                    {
                        TicketUser user = _Util.Facade.TicketFacade.GetAllAssignedTicketCountByUserId(emp.UserId);
                        if (user != null && user.MyTicketCount > 0)
                        {
                            return Json(new { result = false, message = "This employee has jobs in his schedule. You can not deactive this employee." });
                        }
                        else
                        {
                            ul.IsDeleted = true;
                            ul.IsActive = false;
                            _Util.Facade.UserLoginFacade.UpdateUserLogin(ul);


                            emp.IsDeleted = true;
                            emp.IsActive = false;
                            emp.LastUpdatedBy = User.Identity.Name;
                            emp.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                            _Util.Facade.EmployeeFacade.UpdateEmployee(emp);
                            result = true;
                            message = "User has been deleted successfully.";
                        }
                    }

                }
            }
            #region PriviousDeletedFunctionaliy
            //_Util.Facade.UserLoginFacade.DeleteUserLoginByUsername(ul.UserName);
            //_Util.Facade.EmployeeFacade.DeleteEmployeeByUserId(ul.UserId);
            //_Util.Facade.PermissionFacade.DeleteAllUserPermissionsByUserId(ul.UserId);
            //_Util.Facade.UserCompanyFacade.DeleteUserCompanyByUserIdAndCompanyId(ul.UserId, CurrentUser.CompanyId.Value);

            //_Util.Facade.UserOrganizationFacade.DeleteUserOrganizationByUsernameAndCompanyId(ul.UserName, CurrentUser.CompanyId.Value);
            #endregion

            return Json(new { result = result, message = message });
        }
        public JsonResult DeleteUserFile(int EmployeeId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Employee tmpEmployee = _Util.Facade.EmployeeFacade.GetEmployeeById(EmployeeId);
            if (tmpEmployee == null)
            {
                return Json(new { result = false });
            }

            var serverFile = Server.MapPath(tmpEmployee.ProfilePicture);
            if (System.IO.File.Exists(serverFile))
            {
                System.IO.File.Delete(serverFile);
            }
            tmpEmployee.ProfilePicture = "";
            tmpEmployee.LastUpdatedBy = User.Identity.Name;
            tmpEmployee.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            _Util.Facade.EmployeeFacade.UpdateEmployee(tmpEmployee);

            return Json(new { result = true });
        }
        [Authorize]
        public PartialViewResult UserProfilePartial()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            UserLogin ul = _Util.Facade.UserLoginFacade.GetUserByUsernameAndCompanyId(User.Identity.Name, CurrentUser.CompanyId.Value);
            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(ul.UserId);
            UserProfile up = new UserProfile()
            {
                Employee = emp,
                UserLogin = ul
            };
            var cityListForDropdown = _Util.Facade.LookupFacade.GetLookupByKey("USACitiesList").ToList();
            ViewBag.cityListForDropdown = cityListForDropdown;

            ViewBag.CityList = _Util.Facade.LookupFacade.GetLookupByKey("USACitiesList").Select(x =>
                        new SelectListItem()
                        {
                            Text = x.DisplayText.ToString(),
                            Value = x.DataValue.ToString(),

                        }).ToList();

            ViewBag.StateList = _Util.Facade.LookupFacade.GetLookupByKey("StateList").Select(x =>
                        new SelectListItem()
                        {
                            Text = x.DisplayText.ToString(),
                            Value = x.DataValue.ToString()
                        }).ToList();
            ViewBag.empid = emp.Id;
            ViewBag.RoleName = _Util.Facade.PermissionFacade.GetPermissionGroupByUserId(emp.UserId).Name;
            return PartialView("_UserProfile", up);
        }

        [Authorize]
        [HttpPost]
        public JsonResult ChangeCurrentUserCompany(Guid companyId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            // bool uc = _Util.Facade.UserCompanyFacade.UserIsInCompany(User.Identity.Name, companyId);
            bool uc = _Util.Facade.UserOrganizationFacade.UserIsInCompany(User.Identity.Name, companyId);
            if (!uc)
            {
                return Json(new { result = false, message = "Error UM001 Please Contact System admin" });
            }
            // _Util.Facade.UserCompanyFacade.RemoveDefaultCompanyByUsername(User.Identity.Name);

            //_Util.Facade.UserCompanyFacade.SetDefaultUserCompany(User.Identity.Name, companyId);
            _Util.Facade.UserOrganizationFacade.SetDefaultUserCompany(User.Identity.Name, companyId);
            Session[SessionKeys.CurrentUserCompanyList] = null;
            Session[SessionKeys.CurrentLoggedInUser] = null;
            Session[SessionKeys.UserPermissionList] = null;
            Session[SessionKeys.CompanyId] = companyId;
            _Util.Facade.PermissionFacade.GetAllUserPermissions(CurrentUser.UserId, companyId);
            CompanyConneciton CC = _Util.Facade.UserOrganizationFacade.GetCompanyConnectionByUsername(User.Identity.Name);
            //List<UserOrganization> UO = _Util.Facade.UserOrganizationFacade.UserOrganizationByUsername(UserName);

            Company model = new Company()
            {
                CompanyId = CC.CompanyId,
                CompanyName = CC.CompanyName.ToString()
            };

            //model = _Util.Facade.CompanyFacade.GetAllCompanyByCompanyId(companyId);
            if (CC == null || string.IsNullOrWhiteSpace(CC.ConnectionString) || CC.CompanyId == new Guid())
            {
                return Json("none", JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(CC.ConnectionString))
                {
                    PushNotification(model, CurrentUser.UserId);

                    Session[SessionKeys.CompanyConnectionString] = DESEncryptionDecryption.DecryptCipherTextToPlainText(CC.ConnectionString);
                }
                else
                {
                    return Json(new { result = false, message = "Internal Error.!" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { result = true, message = "Default company changed successfully" });
        }

        public JsonResult UserPermissionChange(int empid, int permissionId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            if (empid < 1 || permissionId < 1)
            {
                return Json(new { result = false, message = "Invalid selection." });
            }
            //if (!(CurrentUser.UserRole == LabelHelper.UserTypes.Admin || CurrentUser.UserRole == LabelHelper.UserTypes.SysAdmin) || !base.IsPermitted(UserPermissions.UserMgmtPermissions.UserChangePermissionGroup))
            if (!(CurrentUser.UserRole == LabelHelper.UserTypes.Admin || CurrentUser.UserRole == LabelHelper.UserTypes.SysAdmin || base.IsPermitted(UserPermissions.UserMgmtPermissions.UserChangePermissionGroup)))

            {
                return Json(new { result = false, message = "You do not have enough permission." });
            }
            bool result = false;
            var empInfo = _Util.Facade.EmployeeFacade.GetEmployeeById(empid);

            if (empInfo == null || string.IsNullOrWhiteSpace(empInfo.UserName))
            {
                return Json(new { result = false, message = "User not found." });
            }
            result = _Util.Facade.UserOrganizationFacade.UserIsInCompany(User.Identity.Name, CurrentUser.CompanyId.Value);
            if (!result)
            {
                return Json(new { result = false, message = "You can not change another companies employees info." });
            }

            var deleteOldPermissions = _Util.Facade.PermissionFacade.DeleteAllUserPermissionsByUserIdAndCompanyId(empInfo.UserId, CurrentUser.CompanyId.Value);

            if (deleteOldPermissions)
            {
                var UserPermissionId = _Util.Facade.PermissionFacade.InsertUserPermission(new UserPermission
                {
                    //UserName = empInfo.UserName,
                    CompanyId = CurrentUser.CompanyId.Value,
                    PermissionGroupId = permissionId,
                    UserId = empInfo.UserId
                });
                if (UserPermissionId > 0)
                {
                    return Json(new { result = true, message = "User permission updated successfully." });
                }
            }

            return Json(new { result = false, message = "Internal error occured. Please contact system admin." });
        }
        private bool InsertUserPermissions(Guid CompanyId, Guid UserId, string pGroup, List<int> plist)
        {
            try
            {
                if (pGroup != "Custom")
                {
                    var pGroupId = _Util.Facade.PermissionFacade.GetPGroupIdByNameAndCompanyId(pGroup, CompanyId);
                    _Util.Facade.PermissionFacade.InsertUserPermission(new UserPermission()
                    {
                        UserId = UserId,
                        PermissionGroupId = pGroupId,
                        CompanyId = CompanyId,
                    });
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(pGroup))
                    {
                        var pGroupId = _Util.Facade.PermissionFacade.GetPGroupIdByNameAndCompanyId(pGroup, CompanyId);
                        _Util.Facade.PermissionFacade.InsertUserPermission(new UserPermission()
                        {
                            UserId = UserId,
                            PermissionGroupId = pGroupId,
                            CompanyId = CompanyId
                        });
                    }
                    if (plist.Count > 0)
                    {
                        foreach (var item in plist)
                        {
                            if (item > 0)
                            {
                                _Util.Facade.PermissionFacade.InsertUserPermission(new UserPermission()
                                {
                                    CompanyId = CompanyId,
                                    UserId = UserId,
                                    PermissionId = item
                                });
                            }
                        }
                    }
                }

                #region No need
                //if (pGroup == "Regular")
                //{
                //    _Util.Facade.PermissionFacade.InsertUserPermission(new UserPermission()
                //    {
                //        UserName = email,
                //        PermissionGroupId = 5,
                //        CompanyId=CompanyId
                //    }); 
                //}
                //else if (pGroup == "Admin")
                //{
                //    _Util.Facade.PermissionFacade.InsertUserPermission(new UserPermission()
                //    {
                //        UserName = email,
                //        PermissionGroupId = 2,
                //        CompanyId = CompanyId
                //    });
                //}
                //else if (pGroup == "Reports")
                //{
                //    _Util.Facade.PermissionFacade.InsertUserPermission(new UserPermission()
                //    {
                //        UserName = email,
                //        PermissionGroupId = 3,
                //        CompanyId = CompanyId
                //    });
                //}
                //else if (pGroup == "Custom")
                //{
                //    _Util.Facade.PermissionFacade.InsertUserPermission(new UserPermission() {
                //        UserName = email,
                //        PermissionGroupId = 4,
                //        CompanyId = CompanyId
                //    });
                //    if (plist != null && plist.Count() > 0)
                //    {
                //        foreach (var perm in plist)
                //        {
                //            _Util.Facade.PermissionFacade.InsertUserPermission(new UserPermission()
                //            {
                //                UserName = email,
                //                PermissionId = perm,
                //                CompanyId = CompanyId
                //            });
                //        }
                //    }
                //}
                //if (IsInstaller)
                //{
                //    foreach (var perm in EmployeePermissions.InstallerPermissions)
                //    {
                //        _Util.Facade.PermissionFacade.InsertUserPermission(new UserPermission()
                //        {
                //            UserName = email,
                //            PermissionId = perm,
                //            CompanyId = CompanyId
                //        });
                //    }

                //}
                //if (IsSalesPerson)
                //{
                //    foreach (var perm in EmployeePermissions.SalesPersonPermissions)
                //    {
                //        _Util.Facade.PermissionFacade.InsertUserPermission(new UserPermission()
                //        {
                //            UserName = email,
                //            PermissionId = perm,
                //            CompanyId = CompanyId
                //        });
                //    }
                //}
                //if (IsServiceCall)
                //{
                //    foreach (var perm in EmployeePermissions.ServiceCallPermissions)
                //    {
                //        _Util.Facade.PermissionFacade.InsertUserPermission(new UserPermission()
                //        {
                //            UserName = email,
                //            PermissionId = perm,
                //            CompanyId = CompanyId
                //        });
                //    }

                //}
                #endregion

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        #region Useless
        /*
        [Authorize]
        public PartialViewResult EditUserPermissions(string username)
        {
            var CurrentUserLoggedIn = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUserLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            PermissionGroup userP= _Util.Facade.PermissionFacade.GetPermissionGroupByUserId(CurrentUserLoggedIn.UserId);
            if (userP != null)
            {
                ViewBag.UserRole = userP.Name;
            } 
            EidtUserPermission model = new EidtUserPermission();
            model.UserName = username;
            model.CustomUserPermissionList = new List<CustomUserPermission>();

            if (CurrentUserLoggedIn != null)
            {
                var CustomPermission = _Util.Facade.PermissionFacade.GetAllCustomPermission();
                model.CustomUserPermissionList = CustomPermission;
                var DistincRoles = CustomPermission.Select(x => x.RoleName).Distinct().ToList();
                ViewBag.DistincRoles = DistincRoles;
            }
            return PartialView("_EditUserPermissions", model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult EditUserPermissions(string Username, List<int> plist)
        {
            var CurrentUserLoggedIn = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (CurrentUserLoggedIn == null)
            {
                return Json(new { result = false, message = "Invalid Login" });
            }
            var result = false;
            var message = "Error Occured";
            var userId = 0;
            if (CurrentUserLoggedIn != null)
            {
                if (!string.IsNullOrWhiteSpace(Username))
                {
                    var deleteOldPermissions = _Util.Facade.PermissionFacade.DeleteAllUserPermissionsByUserIdAndCompanyId(CurrentUserLoggedIn.UserId, CurrentUserLoggedIn.CompanyId.Value);
                    if (deleteOldPermissions == true)
                    {
                        var CustomGroupId = _Util.Facade.PermissionFacade.GetCustomPermissionGroupId();
                        if (CustomGroupId > 0)
                        {
                            UserPermission _DbUserPermission = new UserPermission()
                            {
                                CompanyId = CurrentUserLoggedIn.CompanyId.Value,
                                UserId = CurrentUserLoggedIn.UserId,
                                PermissionGroupId = CustomGroupId
                            };
                            result = _Util.Facade.PermissionFacade.InsertUserPermission(_DbUserPermission) > 0;
                            foreach (var item in plist)
                            {
                                if (item > 0)
                                {
                                    UserPermission _UserPermission = new UserPermission()
                                    {
                                        CompanyId = CurrentUserLoggedIn.CompanyId.Value,
                                        UserId = CurrentUserLoggedIn.UserId,
                                        PermissionId = item
                                    };

                                    result = _Util.Facade.PermissionFacade.InsertUserPermission(_UserPermission) > 0;
                                }

                            }
                            if(result == true)
                            {
                                message = "Successfully updated permissions";
                                var user = _Util.Facade.UserLoginFacade.GetUserByUsername(Username);
                                if(user != null)
                                {
                                    userId = user.Id;
                                }
                            }
                        }
                    }
                }
            }
            return Json(new { result = result, message = message, userid = userId });
        }


        */
        #endregion

        [Authorize]
        public ActionResult AddUserFile(int id)
        {
            ViewBag.empid = id;
            return PartialView("AddUserFile");
        }

        [Authorize]
        [HttpPost]
        public JsonResult SaveUserFile(string File, int EmployeeId)
        {
            int UserLoginId = 0;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //File save

            bool isUploaded = false;
            HttpPostedFileBase httpPostedFileBase = Request.Files["CustomerFile"];

            string tempFolderName = ConfigurationManager.AppSettings["File.CustomerFiles"];
            tempFolderName = tempFolderName.TrimEnd('/');
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            tempFolderName = string.Format(tempFolderName, comname);
            tempFolderName += "/" + EmployeeId.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += "-___" + httpPostedFileBase.FileName;

            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
            {
                string tempFolderPath = Server.MapPath("~/" + tempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        Image orgImg = Bitmap.FromStream(httpPostedFileBase.InputStream);
                        foreach (var prop in orgImg.PropertyItems)
                        {
                            if (prop.Id == 0x0112) //value of EXIF
                            {
                                int orientationValue = orgImg.GetPropertyItem(prop.Id).Value[0];
                                RotateFlipType rotateFlipType = ImageHelper.GetOrientationToFlipType(orientationValue);
                                orgImg.RotateFlip(rotateFlipType);

                                break;
                            }
                        }
                        if (orgImg.Height > 500 || orgImg.Width > 500)
                        {
                            orgImg = ImageHelper.GetImageResize(500, 500, orgImg);
                        }
                        var qualityEncoder = Encoder.Quality;
                        var quality = (long)80;
                        var ratio = new EncoderParameter(qualityEncoder, quality);
                        var codecParams = new EncoderParameters(1);
                        codecParams.Param[0] = ratio;
                        var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                        orgImg.Save(Path.Combine(tempFolderPath, FileName), GetEncoder(ImageFormat.Jpeg), codecParams);

                        //httpPostedFileBase.SaveAs(Path.Combine(tempFolderPath, FileName));
                        isUploaded = true;
                    }
                    catch (Exception) {  /*TODO: You must process this exception.*/}
                }
            }

            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            var serverFile = Server.MapPath(File);
            Employee tmpEmployee = _Util.Facade.EmployeeFacade.GetEmployeeById(EmployeeId);
            File = filePath;
            var OnlyFileName = File.Split('/');
            float fileSize = OnlyFileName[6].Length;
            var delimeter = new string[] { "-___" };
            var fullFileName = OnlyFileName[6].Split(delimeter, StringSplitOptions.RemoveEmptyEntries);
            if (tmpEmployee != null)
            {
                if (!string.IsNullOrWhiteSpace(File))
                {
                    tmpEmployee.ProfilePicture = AppConfig.DomainSitePath + File;
                }
                UserLoginId = _Util.Facade.UserLoginFacade.GetUserByUsername(tmpEmployee.UserName).Id;
            }
            tmpEmployee.LastUpdatedBy = User.Identity.Name;
            tmpEmployee.LastUpdatedDate = DateTime.Now.UTCCurrentTime();

            bool result = _Util.Facade.EmployeeFacade.UpdateEmployee(tmpEmployee);
            return Json(new { result = result, UserLoginId = UserLoginId, message = "Profile Successfully saved", isUploaded = isUploaded, filePath = filePath }, "text/html");
        }

        [Authorize]
        [HttpPost]
        public ActionResult UploadUserFile(int EmployeeId)
        {
            bool isUploaded = false;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            HttpPostedFileBase httpPostedFileBase = Request.Files["CustomerFile"];

            string tempFolderName = ConfigurationManager.AppSettings["File.CustomerFiles"];
            tempFolderName = tempFolderName.TrimEnd('/');
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            tempFolderName = string.Format(tempFolderName, comname);
            tempFolderName += "/" + EmployeeId.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month + "-" + DateTime.Now.UTCCurrentTime().Day;

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += "-___" + httpPostedFileBase.FileName;

            if (httpPostedFileBase != null && httpPostedFileBase.ContentLength != 0)
            {
                string tempFolderPath = Server.MapPath("~/" + tempFolderName);

                if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
                {
                    try
                    {
                        Image orgImg = Bitmap.FromStream(httpPostedFileBase.InputStream);
                        foreach (var prop in orgImg.PropertyItems)
                        {
                            if (prop.Id == 0x0112) //value of EXIF
                            {
                                int orientationValue = orgImg.GetPropertyItem(prop.Id).Value[0];
                                RotateFlipType rotateFlipType = ImageHelper.GetOrientationToFlipType(orientationValue);
                                orgImg.RotateFlip(rotateFlipType);

                                break;
                            }
                        }
                        if (orgImg.Height > 500 || orgImg.Width > 500)
                        {
                            orgImg = ImageHelper.GetImageResize(500, 500, orgImg);
                        }
                        var qualityEncoder = Encoder.Quality;
                        var quality = (long)80;
                        var ratio = new EncoderParameter(qualityEncoder, quality);
                        var codecParams = new EncoderParameters(1);
                        codecParams.Param[0] = ratio;
                        var jpegCodecInfo = ImageCodecInfo.GetImageEncoders();
                        orgImg.Save(Path.Combine(tempFolderPath, FileName), GetEncoder(ImageFormat.Jpeg), codecParams);

                        //httpPostedFileBase.SaveAs(Path.Combine(tempFolderPath, FileName));
                        isUploaded = true;
                    }
                    catch (Exception) {  /*TODO: You must process this exception.*/}
                }
            }

            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            return Json(new { isUploaded = isUploaded, filePath = AppConfig.DomainSitePath + filePath }, "text/html");
        }
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            return codecs.Single(codec => codec.FormatID == format.Guid);
        }
        [HttpPost]
        public JsonResult SaveUserProfileInfo(int empid, string fnum, string lnum, string email, string street, string city, string state, string zip, string phn, string ssn, string pass, string username, string profilepicture)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //if (!(CurrentUser.UserRole == LabelHelper.UserTypes.Admin || CurrentUser.UserRole == LabelHelper.UserTypes.SysAdmin))
            //{
            //    return Json(new { result = false, message = "You do not have enough permission." });
            //}

            bool result = false;
            var empInfo = _Util.Facade.EmployeeFacade.GetEmployeeById(empid);
            if (empInfo == null)
            {
                return Json(new { result = false, message = "User not found." });
            }
            if (empInfo.UserId != CurrentUser.UserId)
            {
                return Json(new { result = false, message = "Access denied." });
            }

            #region No need
            //result = _Util.Facade.UserOrganizationFacade.UserIsInCompany(User.Identity.Name, CurrentUser.CompanyId.Value);
            //if (!result)
            //{
            //    return Json(new { result = false, message = "You can not change another companies employees info." });
            //}
            #endregion

            if (empInfo != null)
            {
                empInfo.FirstName = fnum;
                empInfo.LastName = lnum;
                empInfo.Email = email;
                empInfo.Street = street;
                empInfo.City = city;
                empInfo.State = state;
                empInfo.ZipCode = zip;
                empInfo.Phone = phn;
                empInfo.SSN = ssn;
                empInfo.ProfilePicture = profilepicture;
                empInfo.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                empInfo.LastUpdatedBy = User.Identity.Name;

                result = _Util.Facade.EmployeeFacade.UpdateEmployee(empInfo);
            }
            var LoginUser = _Util.Facade.UserLoginFacade.GetUserByUsername(empInfo.UserName);
            if (LoginUser != null)
            {
                string newpassword = LoginUser.Password;
                string newUsername = LoginUser.UserName;
                bool UsernameOrPassChanged = false;
                string encryptedpass = MD5Encryption.GetMD5HashData(pass);
                if (!string.IsNullOrWhiteSpace(pass) && encryptedpass != LoginUser.Password)
                {
                    newpassword = encryptedpass;
                    UsernameOrPassChanged = true;
                }
                if (!string.IsNullOrWhiteSpace(username) && username != LoginUser.UserName)
                {
                    newUsername = username;
                    /*We will not change username for now*/
                    UsernameOrPassChanged = false;
                }

                if (UsernameOrPassChanged)
                {
                    UpdateUsernameAndPasswordGlobally(LoginUser.UserName, newUsername, newpassword);
                }

                // result = _Util.Facade.UserLoginFacade.UpdateUserLogin(LoginUser);

            }
            return Json(new { result = result, message = "Profile information save successfully" });
        }
        private bool UpdateUsernameAndPasswordGlobally(string oldUsername, string newusername, string password)
        {

            string CurrentConnectionString = Session[SessionKeys.CompanyConnectionString].ToString();

            try
            {
                List<Organization> OrgList = _Util.Facade.UserOrganizationFacade.GetAllOrganizationsByUsername(oldUsername);
                foreach (var item in OrgList)
                {
                    Session[SessionKeys.CompanyConnectionString] = item.ConnectionString;
                    UserLogin tempUl = _Util.Facade.UserLoginFacade.GetUserByUsername(oldUsername, true);
                    if (tempUl != null)
                    {
                        //if(tempUl.UserName != newusername)
                        //{
                        //    _Util.Facade.UserLoginFacade.UpdateAllUserNameByUserName(tempUl.UserName,newusername);
                        //}
                        tempUl.UserName = newusername;
                        if (newusername.IsValidEmailAddress())
                        {
                            tempUl.EmailAddress = newusername;
                        }
                        tempUl.Password = password;

                        _Util.Facade.UserLoginFacade.UpdateUserLogin(tempUl);
                    }
                    else
                    {
                        continue;
                    }
                }
                Session[SessionKeys.CompanyConnectionString] = CurrentConnectionString;
                //need to change from userorganization table too...
                //_Util.Facade.UserOrganizationFacade.UpdateUserNameByUserName(oldUsername,newusername);

                return true;

            }
            catch (Exception)
            {
                Session[SessionKeys.CompanyConnectionString] = CurrentConnectionString;
                return false;
            }
        }

        public ActionResult UserInformationRecruit(int? id)
        {
            AddUser up = new AddUser();
            up.UserLogin = new UserLogin();
            up.Permisssions = new List<Permission>();
            up.PermissionGroup = new PermissionGroup();
            up.Employee = new Employee();
            var CurrentUserLoggedIn = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (id.HasValue)
            {
                up.UserLogin = _Util.Facade.UserLoginFacade.GetByIdAndCompanyId(id.Value, CurrentUserLoggedIn.CompanyId.Value);
                up.Employee = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(up.UserLogin.UserId);
                up.PermissionGroup = _Util.Facade.PermissionFacade.GetPermissionGroupByUserId(up.UserLogin.UserId);
                up.Permisssions = _Util.Facade.PermissionFacade.GetAllPermissionsByUserIdAndCompanyId(up.UserLogin.UserId, CurrentUserLoggedIn.CompanyId.Value);
                //up.Permisssions = _Util.Facade.PermissionFacade.GetPermissionListByUsername(up.UserLogin.UserName);
            }
            var cityListForDropdown = _Util.Facade.LookupFacade.GetLookupByKey("USACitiesList").ToList();
            ViewBag.cityListForDropdown = cityListForDropdown;

            ViewBag.CityList = _Util.Facade.LookupFacade.GetLookupByKey("USACitiesList").Select(x =>
                        new SelectListItem()
                        {
                            Text = x.DisplayText.ToString(),
                            Value = x.DataValue.ToString(),

                        }).ToList();

            ViewBag.StateList = _Util.Facade.LookupFacade.GetLookupByKey("StateList").Select(x =>
                        new SelectListItem()
                        {
                            Text = x.DisplayText.ToString(),
                            Value = x.DataValue.ToString()
                        }).ToList();
            List<SelectListItem> SalesCommisssion = new List<SelectListItem>();
            SalesCommisssion.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("CommissionType").Select(x =>
                        new SelectListItem()
                        {
                            Text = x.DisplayText.ToString(),
                            Value = x.DataValue.ToString()
                        }).ToList());
            ViewBag.SalesCommisssion = SalesCommisssion.OrderBy(x => x.Text != "Please Select").ThenBy(x => x.Text).ToList();
            ViewBag.TechCommission = _Util.Facade.LookupFacade.GetLookupByKey("TechComission").Select(x =>
                        new SelectListItem()
                        {
                            Text = x.DisplayText.ToString(),
                            Value = x.DataValue.ToString()
                        }).ToList();
            List<SelectListItem> session = new List<SelectListItem>();
            session.Add(new SelectListItem() { Text = "2018", Value = "2018" });
            ViewBag.Session = session;
            return View("_UserInformationRecruit", up);
        }

        //[Authorize]
        //public JsonResult ChangeAllPasswordToMd5()
        //{ 
        //    List<UserLogin> ULogList = _Util.Facade.UserLoginFacade.GetAllUserLogin();

        //    foreach (var item in ULogList)
        //    {
        //        if (!string.IsNullOrWhiteSpace(item.Password))
        //        {
        //            item.Password = MD5Encryption.GetMD5HashData(item.Password);
        //            _Util.Facade.UserLoginFacade.UpdateUserLogin(item);
        //        }
        //    }
        //    return Json(ULogList,JsonRequestBehavior.AllowGet);
        //}
        //[Authorize]
        //public JsonResult GetMD5Value(string Pass)
        //{
        //    return Json(MD5Encryption.GetMD5HashData(Pass),JsonRequestBehavior.AllowGet);
        //}

        [Authorize]
        public ActionResult GetUserMgmtList(int? UserGroup, string SearchText, string isCurrentEmployee)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            List<UserMgmtList> UserMgmtList = new List<UserMgmtList>();

            UserMgmtList = _Util.Facade.UserLoginFacade.GetAllUserMgmtListByCompanyId(CurrentUser.CompanyId.Value, UserGroup, isCurrentEmployee, null, SearchText);
            return new ViewAsPdf(UserMgmtList);

        }
        private bool IsUserExistsGloballyPrivate(string CurrentUsername, string DesiredUsername)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<UserOrganization> uoList = _Util.Facade.UserOrganizationFacade.GetUsersOrganizationListByUsername(DesiredUsername);
            if (uoList.Count() == 0)
            {
                return false;
            }
            else
            {
                return (uoList.Where(x => x.UserName != CurrentUsername).Count() > 0);
            }
        }

        [HttpPost]
        public JsonResult HierarchyValidationCheck(Guid Userid, Guid SupervisorId)
        {
            bool result = true;
            if (Userid != new Guid() && SupervisorId != Guid.Empty)
            {
                List<Partner> partnerlist = _Util.Facade.EmployeeFacade.GetEmployeeByPartnerId(Userid);
                if (partnerlist != null && partnerlist.Where(x => x.UserId == SupervisorId).Count() > 0)
                {
                    result = false;
                }
            }
            return Json(result);
        }
    }

}